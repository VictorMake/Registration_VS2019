Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Reflection

''' <summary>
''' Менеджер управления классами расшифровок.
''' Абстрактная фабрика осведомлена о всех конкретных класссах и способна порождать их экземпляры. 
''' Метод CreateAnalysis создает класс заданного типа и назначает ему медиатор. 
''' </summary>
''' <remarks></remarks>
Friend Class AnalysisManager
    Implements IEnumerable
    Implements IEnumerable(Of Analysis)

#Region "Interface"
    ''' <summary>
    ''' Оболочка коллекции окон
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property DecodingAnalysis() As Dictionary(Of String, Analysis)
        Get
            Return DecryptionsAnalysis
        End Get
    End Property

    ''' <summary>
    ''' элемент коллекции
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public ReadOnly Property Item(ByVal Key As String) As Analysis
        Get
            If Not analysisEnabled.Contains(Key) Then
                Throw New ArgumentOutOfRangeException(Key, "Данный режим предназначен для испытаний, а не для расшифровки.")
            End If
            Return GetAnalysis(Key)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of Analysis) Implements IEnumerable(Of Analysis).GetEnumerator
        For Each key As String In DecryptionsAnalysis.Keys.ToArray
            Yield DecryptionsAnalysis(key)
        Next
    End Function

    ''' <summary>
    ''' перечислитель
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        'Return Analysis.GetEnumerator
        GetEnumerator()
    End Function

    ''' <summary>
    ''' число текущих загруженных форм
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count() As Integer
        Get
            Return DecryptionsAnalysis.Count
        End Get
    End Property

    '''' <summary>
    '''' удаление по номеру или имени или объекту?
    '''' </summary>
    '''' <param name="vntIndexKey"></param>
    '''' <remarks></remarks>
    'Public Sub Remove(ByRef vntIndexKey As String)
    '    ' если целый тип то по плавающему индексу, а если строковый то по ключу
    '    Analysis.Remove(vntIndexKey)
    '    Me.mAnalysisCreated -= 1
    'End Sub

    'Public Sub Clear()
    '    Analysis.Clear()
    'End Sub

    'Protected Overrides Sub Finalize()
    '    mКоллекцияРасшифровок = Nothing
    '    MyBase.Finalize()
    'End Sub
#End Region

    Private Property Mediator As FormSnapshotViewingDiagram
    Private analysisToLoad As List(Of String)                           ' список подлежащих загрузке классов расшифровок
    Private ReadOnly analysisEnabled As New List(Of String)             ' список разрешенных к загрузке расшифровоки
    Private enumDescriptionsList As List(Of String)                     ' список описаний из типа перечислителя
    Private enumNamesList As List(Of String)                            ' список значений элементов из типа перечислителя
    'Private mAnalysisCreated As Integer                                 ' внутренний счетчик для подсчета созданных расшифровок
    Private ReadOnly DecryptionsAnalysis As New Dictionary(Of String, Analysis)    ' внутренняя ленивая коллекция для управления расшифровками
    ' коллекция фабрик расшифровок
    Private ReadOnly CreatorsAnalysis As Dictionary(Of String, CreatorAnalysis) = New Dictionary(Of String, CreatorAnalysis) From {
        {cРегистратор, New CreatorAnalysisРегистратор},
        {cПриемистостьМГ_РУД67, New CreatorAnalysisПриемистостьМГ_РУД67},
        {cПриемистостьсФорсажомМГ_РУД115, New CreatorAnalysisФорсажомМГ_РУД115},
        {cСбросРУД67_МГ, New CreatorAnalysisСбросРУД67_МГ},
        {cСбросРУД115_МГ, New CreatorAnalysisСбросРУД115_МГ},
        {cЛожныйЗапуск, New CreatorAnalysisЛожныйЗапуск},
        {cВключениеФорсажаРУД67_РУД115, New CreatorAnalysisВключениеФорсажаРУД67_РУД115},
        {cВключениеСУНАНаN290ПоКнопкеБК, New CreatorAnalysisВключениеСУНАНаN290ПоКнопкеБК},
        {cБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК, New CreatorAnalysisБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК},
        {cБыстродействиеРСприВключенииСУНАпоКнопкеБК, New CreatorAnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК},
        {cВключениеКОнаN290, New CreatorAnalysisВключениеКОнаN290},
        {cВключениеКОнаРУД115, New CreatorAnalysisВключениеКОнаРУД115},
        {cПриемистостьМГ_N290, New CreatorAnalysisПриемистостьМГ_N290},
        {cВождениеА1А2N1N2, New CreatorAnalysisВождениеА1А2N1N2},
        {cГорячийЗапуск, New CreatorAnalysisГорячийЗапуск},
        {cПриемистостьМГ_N295сКРД99Б, New CreatorAnalysisПриемистостьМГ_N295сКРД99Б},
        {cОтладочныйРежим, New CreatorAnalysisОтладочныйРежим},
        {cСбросN295_МГ, New CreatorAnalysisСбросN295_МГ},
        {cВключениеФорсажаРУД67РУД115изд39, New CreatorAnalysisВключениеФорсажаРУД67РУД115изд39},
        {cВыключениеФорсажаРУД115РУД67изд39, New CreatorAnalysisВыключениеФорсажаРУД115РУД67изд39},
        {cПлавноеРезкоеДросселирование, New CreatorAnalysisПлавноеРезкоеДросселирование},
        {cПовторноеВключениеФорсажаРУД67РУД115изд39, New CreatorAnalysisПовторноеВключениеФорсажаРУД67РУД115изд39},
        {cПовторноеВыключениеФорсажаРУД115РУД67изд39, New CreatorAnalysisПовторноеВыключениеФорсажаРУД115РУД67изд39},
        {cПриемистостьАИ222, New CreatorAnalysisПриемистостьАИ222},
        {cСбросАИ222, New CreatorAnalysisСбросАИ222}
        }

    Public Sub New(mediator As FormSnapshotViewingDiagram)
        Me.Mediator = mediator
    End Sub

    Public Sub Initialize()
        AddMenuItemAdditionalRegime(analysisToLoad)
        PopulateAnalysisEnabledList()
    End Sub

    ''' <summary>
    ''' Создание меню для дополнительных режимов.
    ''' В зависимости от наличия таблицы с именем "Изделие" в новых версиях базы Channel
    ''' добавляются меню выбора режима в подменю изделия или же просто в корень главного меню.
    ''' </summary>
    Private Sub AddMenuItemAdditionalRegime(ByRef refAnalysisList As List(Of String))
        Const tableEngine As String = "Изделие" ' таблица Изделие
        Dim rdr As OleDbDataReader
        ' создать список заложенных в таблицу расшифровок, по ним будут находится соответствующие классы 
        refAnalysisList = New List(Of String)
        refAnalysisList.AddRange({cРегистратор, cОтладочныйРежим})

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand

            cmd.CommandType = CommandType.Text
            'Dim command As New OleDbCommand(strSQL)
            '    command.Connection = cn
            Try
                cn.Open()

                If CheckExistTable(cn, tableEngine) Then
                    ' добавление подменю на основе типа изделия
                    Dim productList As New List(Of String)
                    Dim indexMenuProduct As Integer = 2
                    cmd.CommandText = "SELECT Изделие.NameEngine FROM Изделие;"
                    rdr = cmd.ExecuteReader '.ExecuteNonQuery()

                    Do While rdr.Read
                        productList.Add(CStr(rdr("NameEngine")))
                    Loop

                    rdr.Close()

                    For Each itemName In productList
                        ' создать меню изделия
                        Dim tsMenuItemEngine As New ToolStripMenuItem With {
                            .Image = Global.Registration.My.Resources.Resources.product,
                            .Name = "tsMenuProduct" & itemName,
                            .Size = New Size(172, 24),
                            .Text = $"Изделие {itemName} ..."
                        }

                        ' добавить в него подменю расшифровки режимов
                        cmd.CommandText = $"SELECT * FROM Режимы{StandNumber} WHERE ((Изделие)='{itemName}' And ((Наименование)<>'Регистратор' And (Наименование)<>'Отладочный режим'));"
                        rdr = cmd.ExecuteReader

                        Dim indexNewMenuItem As Integer = 1
                        Do While rdr.Read
                            tsMenuItemEngine.DropDownItems.Add(Mediator.NewToolStripMenuItem("NewRegime" & itemName & indexNewMenuItem.ToString,
                                                                                        CStr(rdr("Наименование")),
                                                                                        CStr(rdr("номерРежима")),
                                                                                        CStr(If(Not IsDBNull(rdr("ТекстСправки")), rdr("ТекстСправки"), "Описания нет"))))
                            indexNewMenuItem += 1
                            refAnalysisList.Add(CStr(rdr("Наименование")))
                        Loop

                        indexMenuProduct += 1
                        rdr.Close()
                        Mediator.MenuModeOfOperation.DropDownItems.Insert(indexMenuProduct, tsMenuItemEngine)
                    Next
                Else
                    ' простые добавления меню без подуровней
                    cmd.CommandText = $"SELECT * FROM Режимы{StandNumber} WHERE ((Наименование)<>'Регистратор' And (Наименование)<>'Отладочный режим');"
                    rdr = cmd.ExecuteReader
                    Dim indexNewMenuItem As Integer = 3

                    Do While rdr.Read
                        Mediator.MenuModeOfOperation.DropDownItems.Insert(indexNewMenuItem, Mediator.NewToolStripMenuItem("NewRegime" & indexNewMenuItem.ToString,
                                                                                        CStr(rdr("Наименование")),
                                                                                        CStr(rdr("номерРежима")),
                                                                                        CStr(If(Not IsDBNull(rdr("ТекстСправки")), rdr("ТекстСправки"), "Описания нет")))
                                                                                        )
                        indexNewMenuItem += 1
                        refAnalysisList.Add(CStr(rdr("Наименование")))
                    Loop
                End If

            Catch ex As Exception
                Dim caption As String = $"Инициализация меню режимов испытания в процедуре {NameOf(AddMenuItemAdditionalRegime)}"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
            ' Соединение автоматически закрывается когда код попадает в этот блок
        End Using
    End Sub

    ''' <summary>
    ''' Заполняется лист имён доступных расшифровок на основании дескрипторов перечислителя 
    ''' и списка расшифровок в базе данных.
    ''' </summary>
    Private Sub PopulateAnalysisEnabledList()
        Dim caption As String = $"Метод: {NameOf(PopulateAnalysisEnabledList)}"
        Try
            PopulateListEnumNamesAndDescriptions()

            For Each itemDescription As String In enumDescriptionsList
                If analysisToLoad.Contains(itemDescription) Then
                    If CreatorsAnalysis.ContainsKey(itemDescription) Then
                        analysisEnabled.Add(itemDescription)
                    Else
                        Dim text As String = $"Имя класса расшифровки {itemDescription} не найдено в среди доступных фабрик-создателей."
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    End If
                Else
                    Dim text As String = $"Имя класса расшифровки {itemDescription} не найдено при загрузки таблицы <Режимы ###> из базы данных."
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                End If
            Next
        Catch ex As Exception
            MessageBox.Show($"Ошибка загрузки классов расшифровки режимов в {caption}",
                            $"Error: {ex}", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Заполнение списков описаний и
    ''' списков значений элементов из типа перечислителя.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateListEnumNamesAndDescriptions()
        enumDescriptionsList = New List(Of String)
        enumNamesList = New List(Of String)

        ' получить все аттрибуты перечислителя для создания списка возможных окон в системе
        For Each value In [Enum].GetValues(GetType(EnumAnalysis))
            Dim fi As FieldInfo = GetType(EnumAnalysis).GetField([Enum].GetName(GetType(EnumAnalysis), value))
            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

            If dna IsNot Nothing Then
                enumDescriptionsList.Add(dna.Description)
            Else
                enumDescriptionsList.Add("Нет описания")
            End If
        Next

        ' то же самое по другому
        'For Each c In TypeDescriptor.GetConverter(GetType(WindowsForms)).GetStandardValues
        '    Dim dna As DescriptionAttribute = GetType(WindowsForms).GetField([Enum].GetName(GetType(WindowsForms), c)).GetCustomAttributes(GetType(DescriptionAttribute), True)(0)
        '    If dna IsNot Nothing Then
        '        mListEnumDescriptions.Add(dna.Description)
        '    Else
        '        mListEnumDescriptions.Add(WindowsForms.РедакторПерекладок.ToString())
        '    End If
        'Next     

        enumNamesList.AddRange([Enum].GetNames(GetType(EnumAnalysis)).ToArray)
    End Sub

    ''' <summary>
    ''' Выдать класс расшифровки прямо из коллекции если он был создан хотя-бы один раз,
    ''' в противном случае создать его и добавить в коллекцию.
    ''' Ленивая инициализвция.
    ''' </summary>
    ''' <param name="nameAnalysis"></param>
    ''' <returns></returns>
    Private Function GetAnalysis(ByVal nameAnalysis As String) As Analysis
        If DecryptionsAnalysis.ContainsKey(nameAnalysis) Then
            Return DecryptionsAnalysis.Item(nameAnalysis)
        Else
            Return CreateNewAnalysis(nameAnalysis)
        End If
    End Function

    ''' <summary>
    ''' Добавление класса расшифровки в коллекцию
    ''' </summary>
    ''' <param name="nameAnalysis"></param>
    ''' <returns></returns>
    Private Function CreateNewAnalysis(ByVal nameAnalysis As String) As Analysis
        Dim newAnalysis As Analysis = Nothing
        Dim caption As String = $"Метод: {NameOf(CreateNewAnalysis)}"

        Try
            ' по описанию перечислителя расшифровок, фабрика создаёт экземпляр класса расшифровки и добавляется в словарь менеджера

            'Паттерн        Абстрактная фабрика
            'Имя в проекте  AnalysisManager()
            'Задача         Создавать конкретные модули проекта. Скрыть от главной формы все конкретные классы модулей проекта. 
            'Решение        Скрыть знание о конкретных классах 
            'Результат      Главная форма не знает конкретных классов модулей, и ее код остается неизменен при добавлении новых модулей в проект. 
            If CreatorsAnalysis.Keys.Contains(nameAnalysis) Then newAnalysis = CType(CreatorsAnalysis(nameAnalysis).GetAnalysis(nameAnalysis, Mediator), Analysis)

            If newAnalysis Is Nothing Then
                Dim text As String = $"Ошибка при создании класса расшифровки:<{nameAnalysis}> из фабрики-создателя."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return Nothing
            End If

            ' при создании автоматом добавляется в коллекцию
            DecryptionsAnalysis.Add(nameAnalysis, newAnalysis)
            'Me.mAnalysisCreated += 1

            ' здесь провести проверку на корректность и только если прошло продолжить
            If DecryptionsAnalysis.ContainsKey(nameAnalysis) Then
                Return newAnalysis
            Else
                Return Nothing
            End If
        Catch exp As Exception
            Dim text As String = exp.Message
            MessageBox.Show(text, $"{caption}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Return Nothing
        End Try
    End Function
End Class
