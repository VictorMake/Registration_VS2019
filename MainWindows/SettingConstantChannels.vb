Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.IO
Imports System.Text.Encodings.Web
Imports System.Text.Json
Imports System.Text.Unicode
Imports System.Timers

#Region "ProxyConstantChannels"
''' <summary>
'''  Упрощённый класс для сериализации и дессириализации,
'''  используемый для сохранения в файл списков каналов, 
'''  значения которых используемых как константы.
''' </summary>
Public Class ProxyConstantChannels
    ''' <summary>
    ''' Список с выбранными параметрами
    ''' </summary>
    ''' <returns></returns>
    Public Property ConstantChannels As New List(Of ConstantChannel)
End Class

''' <summary>
''' Параметр, используемый как константа.
''' </summary>
Public Class ConstantChannel
    Public Property NameParameter As String
    Public Property ConstantValue As Double

    Public Sub New()
    End Sub

    Public Sub New(ByVal inName As String, inValue As Double)
        NameParameter = inName
        ConstantValue = inValue
    End Sub

    Public Overrides Function ToString() As String
        Return $"{NameParameter} = {ConstantValue}"
    End Function
End Class
#End Region

''' <summary>
''' Для заполнения таблицы константных параметров.
''' </summary>
Public Class ConstantChannelsBindingList
    Inherits System.ComponentModel.BindingList(Of ConstantChannel)

    Public Sub AddRange(inConstantChannels As ConstantChannel())
        For Each itemChannel As ConstantChannel In inConstantChannels
            Add(itemChannel)
        Next
    End Sub
End Class

''' <summary>
''' Класс реализует обновление, восстановление и хранение настроек списков каналов значения которых используемых как константы.
''' Навигацию и доступ к списку для окна настройки и обновления базы данных каналов.
''' </summary>
Public Class SettingConstantChannels
    Implements IEnumerable
    Implements IEnumerable(Of ConstantChannel)

    ' 1. Проверка наличия файла .json в рабочем каталоге и в случае отсутствия создать с заглушкой с пустой коллекцией
    ' 2. Дессериализация в коллекцию
    ' 3. Связывание коллекции с DataGridView.DataSource = BindingSource
    ' 4. Проверка наличия записей в коллекции и в базе данных и удаления в случае отсутствия
    ' 5. При добавлении записи вызов формы поиска канала, если ни чего не выбрано или канал выбран повторно, то временная запись не добавляется
    ' 6. При изменении DataGridView производит вылидацию значения
    ' 7. При добавлении, удалении и изменении отслеживается флаг перезаписи
    ' 8. При закрытии формы и флаге перезаписи призвести изменение коэффициентов полинома в базе и сериализацию и запись .json файла
    ' 9. Добавлены обработчики ошибок при считывании и записи
    ' 10. Ведётся журнал событий при добавлении, удалении и изменении константных параметров

    Public Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        GetEnumerator()
    End Function

    ' Реализация интерфейса IEnumerable предполагает стандартную реализацию перечислителя.
    ' Однако мы можем не полагаться на стандартную реализацию, а создать свою логику итератора с помощью ключевых слов Iterator и Yield.
    ' Конструкция итератора представляет метод, в котором используется ключевое слово Yield для перебора по коллекции или массиву.
    Private Iterator Function GetEnumerator() As IEnumerator(Of ConstantChannel) Implements IEnumerable(Of ConstantChannel).GetEnumerator
        For Each itemChannel As ConstantChannel In ConstantChannels.ToArray
            Yield itemChannel
        Next
    End Function

    Default Public ReadOnly Property Item(ByVal Key As String) As ConstantChannel
        Get
            Return ConstantChannels.ToList.Find(Function(param) param.NameParameter = Key)
        End Get
    End Property

    ' очистить
    'mSettingSelectedParameters.ConstantChannelAsArray = Array.Empty(Of SettingConstantChannels.ConstantChannel)()

    '''' <summary>
    '''' Текущий массив константных параметров
    '''' </summary>
    '''' <returns></returns>
    'Public Property ConstantChannelAsArray As ConstantChannel()
    '    Get
    '        Return ConstantChannels.ToArray
    '    End Get
    '    Set
    '        ConstantChannels.Clear()
    '        ConstantChannels.AddRange(Value)
    '        IsNeedSaveDBase = True
    '        SaveAndRestoreAfterSaving()
    '    End Set
    'End Property

    ''' <summary>
    ''' Количество параметров в рабочем списке.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ConstantChannelCount As Integer
        Get
            Return ConstantChannels.Count
        End Get
    End Property

    ''' <summary>
    ''' Текущий список константных параметров
    ''' </summary>
    Private ReadOnly ConstantChannels As New ConstantChannelsBindingList 'Private ReadOnly Property ConstantChannels As New ConstantChannelsBindingList
    Private memoConstantChannels As New List(Of ConstantChannel)

    ''' <summary>
    ''' Полный путь к файлу настроек
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PathSettingConstantChannels As String

    Private Const FileControlParameters As String = "SettingConstantChannels.json"
    Private Const con9999999 As Double = 9999999.0
    Private Const TimerInterval As Integer = 200 ' миллисекунд   
    ''' <summary>
    ''' Родительская форма
    ''' </summary>
    Private ReadOnly mFormSetting As FormSetting
    ''' <summary>
    ''' Источник данных для таблицы
    ''' </summary>
    Private WithEvents BindingSourceInputChannels As BindingSource
    ''' <summary>
    ''' Имя параметра из поискового диалога
    ''' </summary>
    Private nameParameter As String
    ''' <summary>
    ''' Серверный таймер работает в другом потоке
    ''' </summary>
    Private aTimer As Timer
    ''' <summary>
    ''' Флаг необходимости перезаписи базы и конфигурации
    ''' </summary>
    Private IsNeedSaveDBase As Boolean
    ''' <summary>
    ''' Флаг добавления параметра
    ''' </summary>
    Private IsUserAddingConstantChannel As Boolean
    ''' <summary>
    ''' Опции сериализации
    ''' </summary>
    Private ReadOnly options As JsonSerializerOptions = New JsonSerializerOptions With {
        .WriteIndented = True,
        .Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        }

    Public Sub New(inPathResourses As String, inForm As FormSetting)
        PathSettingConstantChannels = Path.Combine(inPathResourses, FileControlParameters)

        If Not File.Exists(PathSettingConstantChannels) Then CreateCongigByDefault()

        mFormSetting = inForm
        InitializeComponent()
        ReadConstantChannels()
    End Sub

    Private Sub InitializeComponent()
        If mFormSetting.FormComponents IsNot Nothing Then
            BindingSourceInputChannels = New BindingSource(mFormSetting.FormComponents)
            CType(BindingSourceInputChannels, ComponentModel.ISupportInitialize).BeginInit()
            mFormSetting.DataGridViewConstantChannels.DataSource = BindingSourceInputChannels
            BindingSourceInputChannels.DataSource = GetType(ConstantChannelsBindingList)
            mFormSetting.BindingNavigatorConstantChannels.BindingSource = BindingSourceInputChannels
            CType(BindingSourceInputChannels, ComponentModel.ISupportInitialize).EndInit()
        End If

        AddHandler mFormSetting.BindingNavigatorDeleteItem.Click, AddressOf BindingNavigatorDeleteItem_Click
        AddHandler mFormSetting.DataGridViewConstantChannels.RowsAdded, AddressOf DataGridViewConstantChannels_RowsAdded
        'AddHandler mFormSetting.DataGridViewConstantChannels.SelectionChanged, AddressOf DataGridViewConstantChannels_SelectionChanged
        ' заменил на обработчик BindingNavigatorDeleteItem_Click
        'AddHandler mFormSetting.DataGridViewConstantChannels.RowsRemoved, AddressOf DataGridViewConstantChannels_RowsRemoved
        AddHandler mFormSetting.DataGridViewConstantChannels.CellValueChanged, AddressOf DataGridViewConstantChannels_CellValueChanged
        aTimer = New Timer(TimerInterval)
        ' обработчик события Elapsed таймера.
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
    End Sub

    Private Sub InitializeDataGrid()
        ' сделать русские имена столбцов
        ' растянуть столбцы по ширине
        ' запретить редактировать 1 столбец
        mFormSetting.DataGridViewConstantChannels.Columns(0).HeaderText = "Имя параметра"
        mFormSetting.DataGridViewConstantChannels.Columns(1).HeaderText = "Константное значение"
        mFormSetting.DataGridViewConstantChannels.Columns(0).ReadOnly = True
    End Sub

    ''' <summary>
    ''' Восстановление после записи при добавлении и удалении
    ''' </summary>
    Public Sub SaveAndRestoreAfterSaving()
        If IsNeedSaveDBase Then
            CleaningFromUnusedParameters()
            SaveConstantChannels()
            ReadConstantChannels()
            SaveConstantCoefficientToSelectDBChannels()
            IsNeedSaveDBase = False
        End If
    End Sub

    ' если переписать то удаление из списка старых и запись новых
    ' создать новый список
    'Dim inNames As New List(Of ConstantChannel)
    'mSettingSelectedParameters.PopulateConstantChannels(inNames)
    '''' <summary>
    '''' Обновление рабочего списка парметров в текущей книге
    '''' </summary>
    '''' <param name="inConstantChannels"></param>
    'Public Sub PopulateConstantChannels(inConstantChannels As List(Of ConstantChannel))
    '    ConstantChannels.Clear()
    '    ConstantChannels.AddRange(inConstantChannels.ToArray)
    '    IsNeedSaveDBase = True
    '    SaveAndRestoreAfterSaving()
    'End Sub

#Region "Read/Save ConstantChannels"
    ''' <summary>
    ''' Сохранение произведенных изменений
    ''' </summary>
    Public Sub SaveConstantChannels()
        ' произвести полное отображение на прокси
        Dim proxy As New ProxyConstantChannels With {.ConstantChannels = ConstantChannels.ToList}
        ' отображение текущего содержимого в временный прокси для записи
        SerializerConstantChannels(proxy)
    End Sub

    ''' <summary>
    ''' Сериализация массива настроек для отображения в файл.
    ''' </summary>
    ''' <param name="proxy"></param>
    Private Sub SerializerConstantChannels(proxy As ProxyConstantChannels)
        Try
            Dim jsonString = JsonSerializer.Serialize(proxy, options)
            File.WriteAllText(PathSettingConstantChannels, jsonString)
        Catch ex As IOException
            '     При открытии файла произошла ошибка ввода-вывода.
            Dim text As String = $"При открытии файла <{PathSettingConstantChannels}> произошла ошибка ввода-вывода.{Environment.NewLine}Описание: {ex}"
            Dim caption As String = $"Процедура {NameOf(SerializerConstantChannels)}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Десериализация из файла в массив настроек для отображения.
    ''' </summary>
    Private Sub ReadConstantChannels()
        Dim proxy As ProxyConstantChannels

        Try
            Dim jsonString = File.ReadAllText(PathSettingConstantChannels)
            proxy = JsonSerializer.Deserialize(Of ProxyConstantChannels)(jsonString, options)
            ' произвести полное отображение (копирование) с прокси на реальную коллекцию
            ConstantChannels.Clear()
            ConstantChannels.AddRange(proxy.ConstantChannels.ToArray)
            PopulateDataSource()
            PopulateMemoConstantChannels()
        Catch ex As IOException
            '     При открытии файла произошла ошибка ввода-вывода.
            Dim text As String = $"При открытии файла <{PathSettingConstantChannels}> произошла ошибка ввода-вывода.{Environment.NewLine}Описание: {ex}"
            Dim caption As String = $"Процедура {NameOf(ReadConstantChannels)}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Заполнение источника данных таблицы листом поддерживающий привязку данных.
    ''' </summary>
    Private Sub PopulateDataSource()
        BindingSourceInputChannels.DataSource = Nothing
        BindingSourceInputChannels.DataSource = ConstantChannels
        BindingSourceInputChannels.Position = ConstantChannels.Count
        InitializeDataGrid()

        With mFormSetting.DataGridViewConstantChannels
            .Focus()
            If .Rows.Count > 0 Then
                .Rows(.Rows.Count - 1).Selected = True
                .FirstDisplayedScrollingRowIndex = .Rows.Count - 1
            End If
        End With
    End Sub
#End Region

#Region "Создание и запись в файл конфигурации по умолчанию"
    ''' <summary>
    ''' Создание списка парметров по умолчанию.
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateListParameterByDefault() As List(Of ConstantChannel)
        Return New List(Of ConstantChannel)({New ConstantChannel("Не выбран 1", con9999999), New ConstantChannel("Не выбран 2", con9999999)})
    End Function

    ''' <summary>
    ''' Создание и запись в файл конфигурации по умолчанию.
    ''' </summary>
    Private Sub CreateCongigByDefault()
        Dim proxy As New ProxyConstantChannels With {
            .ConstantChannels = CreateListParameterByDefault()
        }
        ' Запись
        SerializerConstantChannels(proxy)
    End Sub
#End Region

    ''' <summary>
    ''' Проверка на наличие параметра в коллекции константных параметров.
    ''' </summary>
    ''' <param name="inSelectChannel"></param>
    ''' <returns></returns>
    Private Function IsContains(inSelectChannel As String) As Boolean
        If ConstantChannels.ToList.Find(Function(param) param.NameParameter = inSelectChannel) IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Обработчик добавления новой строки в таблицу и
    ''' запуск таймера для последующей коррекции.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DataGridViewConstantChannels_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs)
        If IsUserAddingConstantChannel Then
            IsUserAddingConstantChannel = False
            aTimer.Interval = TimerInterval
            aTimer.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Обработчик события удаления строки таблицы.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BindingNavigatorDeleteItem_Click(sender As Object, e As EventArgs)
        DataGridViewConstantChannelsRowsRemoved()
    End Sub

    ''' <summary>
    ''' Обработчик события удаления строки таблицы.
    ''' </summary>
    Private Sub DataGridViewConstantChannelsRowsRemoved()
        Dim memoConstantChannel As ConstantChannel

        IsNeedSaveDBase = True

        For Each itemConstantChannel As ConstantChannel In memoConstantChannels
            If Not IsContains(itemConstantChannel.NameParameter) Then
                memoConstantChannel = itemConstantChannel
                Exit For
            End If
        Next

        If memoConstantChannel IsNot Nothing Then
            Dim text As String = $"Пользователь удалил из таблицы константный параметр <{memoConstantChannel.NameParameter} = {memoConstantChannel.ConstantValue}>"
            Dim caption As String = $"Процедура {NameOf(DataGridViewConstantChannelsRowsRemoved)}"
            RegistrationEventLog.EventLog_MSG_DB_UPDATE($"<{caption}> {text}")
            'Else
            '    MessageBox.Show(mFormSetting, "Параметр не выделен в таблице.", "Удаление константного параметра", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        PopulateMemoConstantChannels()
    End Sub

    ''' <summary>
    ''' Клонировать текущий набор.
    ''' </summary>
    Private Sub PopulateMemoConstantChannels()
        memoConstantChannels.Clear()
        memoConstantChannels.AddRange(ConstantChannels.ToArray)
    End Sub

    ' Все события после уже завершившихся действий и поэтому бесполезны
    'Private Sub DataGridViewConstantChannels_SelectionChanged(sender As Object, e As EventArgs) 
    '    If mFormSetting.DataGridViewConstantChannels.CurrentRow IsNot Nothing AndAlso mFormSetting.DataGridViewConstantChannels.CurrentRow.Selected Then
    '        If mFormSetting.DataGridViewConstantChannels.CurrentRow.Cells(0).Value IsNot Nothing Then
    '            memoConstantChannel.NameParameter = mFormSetting.DataGridViewConstantChannels.CurrentRow.Cells(0).Value.ToString
    '            memoConstantChannel.ConstantValue = CDbl(mFormSetting.DataGridViewConstantChannels.CurrentRow.Cells(1).Value)
    '        End If
    '    End If
    'End Sub

    ''' <summary>
    ''' Обработчик события изменения значения строки таблицы.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DataGridViewConstantChannels_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex = 1 AndAlso e.RowIndex <> -1 Then
            IsNeedSaveDBase = True
            Dim text As String = $"Пользователь изменил константный параметр <{mFormSetting.DataGridViewConstantChannels(0, e.RowIndex).Value} = {mFormSetting.DataGridViewConstantChannels(1, e.RowIndex).Value}>"
            Dim caption As String = $"Процедура {NameOf(DataGridViewConstantChannelsRowsRemoved)}"
            RegistrationEventLog.EventLog_MSG_DB_UPDATE($"<{caption}> {text}")
            PopulateMemoConstantChannels()
        End If
    End Sub

    ''' <summary>
    ''' В событии добавления нового константного параметра присвоить имя из диалогового окна
    ''' </summary>
    Public Sub BindingNavigatorAddNewItem()
        Dim fakeComboBox As New ComboBox
        IsUserAddingConstantChannel = True

        For I As Integer = 0 To UBound(ParametersType)
            fakeComboBox.Items.Add(New StringIntObject(ParametersType(I).NameParameter, 1))
        Next

        Dim mSearchChannel As New SearchChannel(fakeComboBox)
        mSearchChannel.SelectChannel()

        nameParameter = ""
        If Not IsContains(fakeComboBox.Text) AndAlso fakeComboBox.Text <> "" Then
            nameParameter = fakeComboBox.Text
        End If
    End Sub

    ''' <summary>
    ''' Обработчик таймера для последующей коррекции записи или её удаления.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        aTimer.Stop()

        If nameParameter = "" Then
            ' удалять надо последнюю временную строку
            DataGridViewRemoveAT(ConstantChannels.Count - 1)
        Else
            DataGridViewUpdateAddingConstantChannel()
        End If

        DataGridViewConstantChannelsRefresh()
    End Sub

    ''' <summary>
    ''' Модификация нужным именем и значением по умолчанию вновь добавленной строки таблицы.
    ''' Выполняет указанный делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления.
    ''' </summary>
    Private Sub DataGridViewUpdateAddingConstantChannel()
        If mFormSetting.InvokeRequired Then
            mFormSetting.Invoke(New MethodInvoker(Sub() DataGridViewUpdateAddingConstantChannel()))
        Else
            ConstantChannels.Item(ConstantChannels.Count - 1).NameParameter = nameParameter
            ConstantChannels.Item(ConstantChannels.Count - 1).ConstantValue = con9999999
            PopulateMemoConstantChannels()
            IsNeedSaveDBase = True
        End If
    End Sub

    ''' <summary>
    ''' Удаление элемента списка с указанным индексом.
    ''' Выполняет указанный делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления.
    ''' </summary>
    ''' <param name="index"></param>
    Private Sub DataGridViewRemoveAT(index As Integer)
        If mFormSetting.InvokeRequired Then
            mFormSetting.Invoke(New MethodInvoker(Sub() DataGridViewRemoveAT(index)))
        Else
            ConstantChannels.RemoveAt(index)
            MessageBox.Show(mFormSetting, "Параметр уже имеется в таблице.", "Добавление константного параметра", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' Перерисовка клиентской области.
    ''' Выполняет указанный делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления.
    ''' </summary>
    Private Sub DataGridViewConstantChannelsRefresh()
        If mFormSetting.InvokeRequired Then
            mFormSetting.Invoke(New MethodInvoker(Sub() DataGridViewConstantChannelsRefresh()))
        Else
            mFormSetting.DataGridViewConstantChannels.Refresh()
        End If
    End Sub

    ''' <summary>
    ''' Удалить параметры отсутствующие в каналах выбранного стенда.
    ''' В шапке номер стенда уже выбран.
    ''' </summary>
    Private Sub CleaningFromUnusedParameters()
        Dim namesForDelete As New List(Of String)
        Dim success As Boolean
        Dim nameParameter As String

        For Each itemConstantChannel As ConstantChannel In ConstantChannels
            nameParameter = itemConstantChannel.NameParameter
            success = False

            For I As Integer = 1 To UBound(ParametersType)
                If ParametersType(I).NameParameter = nameParameter Then
                    success = True
                    Exit For
                End If
            Next

            If Not success Then namesForDelete.Add(nameParameter)
        Next

        If namesForDelete.Count > 0 Then
            For Each nameParameter In namesForDelete
                'If ConstantChannels.ToList.Find(Function(param) param.NameParameter = nameParameter) Is Nothing Then
                ConstantChannels.Remove(Me(nameParameter))
                'End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Записать для выбранных параметров коэффициенты полинома, смещение и компенсацию ХС так,
    ''' чтобы параметр при опросе показывал константное значение.
    ''' В шапке номер стенда ChannelLast уже выбран.
    ''' </summary>
    Private Sub SaveConstantCoefficientToSelectDBChannels()
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Using odaDataAdapter As New OleDbDataAdapter($"SELECT * FROM {ChannelLast}", cn)
                Using dtDataTable As New DataTable
                    odaDataAdapter.Fill(dtDataTable)

                    If dtDataTable.Rows.Count <> 0 Then
                        For Each itemConstantChannel As ConstantChannel In ConstantChannels
                            For I As Integer = 0 To dtDataTable.Rows.Count - 1
                                If CStr(dtDataTable.Rows(I)("НаименованиеПараметра")) = itemConstantChannel.NameParameter Then
                                    dtDataTable.Rows(I)("НомерФормулы") = 2
                                    dtDataTable.Rows(I)("СтепеньАппроксимации") = 1
                                    dtDataTable.Rows(I)("A0") = itemConstantChannel.ConstantValue
                                    dtDataTable.Rows(I)("A1") = 0
                                    dtDataTable.Rows(I)("A2") = 0
                                    dtDataTable.Rows(I)("A3") = 0
                                    dtDataTable.Rows(I)("A4") = 0
                                    dtDataTable.Rows(I)("A5") = 0
                                    dtDataTable.Rows(I)("Смещение") = 0
                                    dtDataTable.Rows(I)("КомпенсацияХС") = False
                                    Exit For
                                End If
                            Next
                        Next

                        Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
                        odaDataAdapter.Update(dtDataTable)
                    End If
                End Using
            End Using
        End Using

        LoadChannels() ' в шапке выбирается номер стенда
    End Sub

    '''' <summary>
    '''' Восстановление после записи при добавлении и удалении
    '''' </summary>
    'Public Async Sub SaveAndRestoreAfterSaving()
    '    SaveConstantChannelsAsync()
    '    Await ReadConstantChannelsAsync().ConfigureAwait(True)
    'End Sub

    '''' <summary>
    '''' Сохранение произведенных изменений
    '''' </summary>
    'Public Async Sub SaveConstantChannelsAsync()
    '    ' произвести полное отображение на прокси
    '    Dim proxy As New ProxyConstantChannels With {
    '        .ConstantChannels = ConstantChannels.ToList
    '    }
    '    ' отображение текущего содержимого в временный прокси для записи
    '    Await SerializerConstantChannelsAsync(proxy).ConfigureAwait(True)
    'End Sub

    '''' <summary>
    '''' Сериализация массива настроек для отображения в файл.
    '''' </summary>
    '''' <param name="proxy"></param>
    'Private Async Function SerializerConstantChannelsAsync(proxy As ProxyConstantChannels) As Task
    '    Dim options = New JsonSerializerOptions With {.WriteIndented = True}

    '    Using fs As New FileStream(PathSettingConstantChannels, FileMode.Create)
    '        Await JsonSerializer.SerializeAsync(fs, proxy, options).ConfigureAwait(True)
    '    End Using
    'End Function

    '''' <summary>
    '''' Десериализация из файла в массив настроек для отображения.
    '''' </summary>
    'Private Async Function ReadConstantChannelsAsync() As Task
    '    Dim proxy As ProxyConstantChannels

    '    Using fs As FileStream = New FileStream(PathSettingConstantChannels, FileMode.Open)
    '        proxy = Await JsonSerializer.DeserializeAsync(Of ProxyConstantChannels)(fs)
    '    End Using

    '    ' произвести полное отображение (копирование) с прокси на реальную коллекцию
    '    ConstantChannels.Clear()
    '    ConstantChannels.ToList.AddRange(CType(proxy.ConstantChannels.ToArray.Clone, IEnumerable(Of ConstantChannel)))
    '    PopulateDataSource()
    'End Function

    '''' <summary>
    '''' Создание и запись в файл конфигурации по умолчанию
    '''' </summary>
    'Private Async Sub CreateCongigByDefault()
    '    Dim proxy As New ProxyConstantChannels With {
    '        .ConstantChannels = CreateListParameterByDefault()
    '    }
    '    ' Запись
    '    Await SerializerConstantChannelsAsync(proxy).ConfigureAwait(True)
    'End Sub
End Class