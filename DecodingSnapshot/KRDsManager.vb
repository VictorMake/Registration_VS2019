Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection

''' <summary>
''' Менеджер управления классами расшифровок.
''' Абстрактная фабрика осведомлена о всех конкретных класссах и способна порождать их экземпляры. 
''' Метод CreateKRD создает класс заданного типа. 
''' </summary>
''' <remarks></remarks>
Friend Class KRDsManager
    Implements IEnumerable
    Implements IEnumerable(Of KRD)

#Region "Interface"
    ''' <summary>
    ''' Оболочка коллекции окон
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property AllKRD() As Dictionary(Of String, KRD)
        Get
            Return KRDs
        End Get
    End Property

    ''' <summary>
    ''' элемент коллекции
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public ReadOnly Property Item(ByVal Key As String) As KRD
        Get
            If Not KRDs.Keys.Contains(Key) Then
                Throw New ArgumentOutOfRangeException(Key, "Данный КРД недоступен.")
            End If
            Return KRDs.Item(Key)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of KRD) Implements IEnumerable(Of KRD).GetEnumerator
        For Each key As String In KRDs.Keys.ToArray
            Yield KRDs(key)
        Next
    End Function

    ''' <summary>
    ''' перечислитель
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        'Return KRDs.GetEnumerator
        GetEnumerator()
    End Function

    Public ReadOnly Property AllKRDKeysToArray() As String()
        Get
            Return KRDs.Keys.ToArray
        End Get
    End Property

    ''' <summary>
    ''' число текущих загруженных форм
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count() As Integer
        Get
            Return KRDs.Count
        End Get
    End Property

    '''' <summary>
    '''' удаление по номеру или имени или объекту?
    '''' </summary>
    '''' <param name="vntIndexKey"></param>
    '''' <remarks></remarks>
    'Public Sub Remove(ByRef vntIndexKey As String)
    '    ' если целый тип то по плавающему индексу, а если строковый то по ключу
    '    KRDs.Remove(vntIndexKey)
    '    Me.mKRDCreated -= 1
    'End Sub

    'Public Sub Clear()
    '    KRDs.Clear()
    'End Sub
#End Region

    Private ReadOnly KRDs As New Dictionary(Of String, KRD) ' внутренняя коллекция для управления расшифровками
    Private enumDescriptionsList As List(Of String)    ' список описаний из типа перечислителя
    Private enumNamesList As List(Of String)           ' список значений элементов из типа перечислителя
    Private mKRDCreated As Integer ' внутренний счетчик для подсчета созданных КРД можно использовать в заголовке

    Public Sub New()
        InitializeKRDManager()
    End Sub

    Private Sub InitializeKRDManager()
        Dim caption As String = $"Метод: {NameOf(InitializeKRDManager)}"
        Try
            'Паттерн        Абстрактная фабрика
            'Имя в проекте  KRDsManager()
            'Задача         Создавать конкретные модули проекта. Скрыть от главной формы все конкретные классы модулей проекта. 
            'Решение        Скрыть знание о конкретных классах 
            'Результат      Главная форма не знает конкретных классов модулей, и ее код остается неизменен при добавлении новых модулей в проект. 

            ' В качестве <Главного модуля> выступает форма настроек. Она создает множество модулей через абстрактную фабрику KRDsManager. 
            ' Для перечисления всех модулей программы используется перечисление KRDType. 
            ' при инициализации пробегается по значениям этого типа (через mEnumDescriptionsList) и вызывает фабрику для создания конкретной формы. 

            PopulateListEnumNamesAndDescriptions()

            ' по описанию перечислителя КРД, фабрика создаёт экземпляр класса расшифровки и добавляется в словарь менеджера
            For Each itemDescription As String In enumDescriptionsList
                Try
                    ' при создании автоматом добавляется в коллекцию
                    If Not CreateKRD(itemDescription) Then ' там проверка на корректность
                        Dim text As String = $"Ошибка при добавлении класса КРД с именем: {itemDescription}"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    End If
                Catch ex As Exception
                    Dim text As String = $"Ошибка при создании класса КРД с именем {itemDescription}:{vbCrLf}"
                    MessageBox.Show(text & ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                End Try
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
        For Each value In [Enum].GetValues(GetType(KRDType))
            Dim fi As FieldInfo = GetType(KRDType).GetField([Enum].GetName(GetType(KRDType), value))
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

        enumNamesList.AddRange([Enum].GetNames(GetType(KRDType)).ToArray)
    End Sub

    ''' <summary>
    ''' Добавление класса КРД в коллекцию
    ''' </summary>
    ''' <param name="descriptionKRD"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateKRD(ByVal descriptionKRD As String) As Boolean
        Dim clsKRD As KRD = Nothing
        Dim caption As String = $"Метод: {NameOf(CreateKRD)}"

        Try
            Select Case descriptionKRD
                Case cKRD_A
                    clsKRD = New KRD_A
                Case cKRD_B
                    clsKRD = New KRD_B
                Case cARD_39
                    clsKRD = New ARD_39
                Case cKRD_99_C
                    clsKRD = New KRD_99_C
                Case cCRD_99
                    clsKRD = New CRD_99
                Case Else
                    Exit Select
            End Select

            If clsKRD Is Nothing Then
                Dim text As String = $"Имя класса типа КРД {descriptionKRD} не найдено при расшифровке режима."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return False
            End If

            KRDs.Add(descriptionKRD, clsKRD)
            Me.mKRDCreated += 1

            ' здесь провести проверку на корректность и только если прошло продолжить
            If KRDs.ContainsKey(descriptionKRD) Then
                Return True
            Else
                Return False
            End If
        Catch exp As Exception
            Dim text As String = exp.Message
            MessageBox.Show(text, $"{caption}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Return False
        End Try
    End Function
End Class
