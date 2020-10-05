Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection

''' <summary>
''' Менеджер управления классами моторов.
''' Абстрактная фабрика осведомлена о всех конкретных класссах и способна порождать их экземпляры. 
''' Метод CreateEngine создает класс заданного типа. 
''' </summary>
''' <remarks></remarks>
Friend Class EngineManager
    Implements IEnumerable
    Implements IEnumerable(Of Engine)

#Region "Interface"
    ''' <summary>
    ''' Оболочка коллекции окон
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property AllEngine() As Dictionary(Of String, Engine)
        Get
            Return Engines
        End Get
    End Property

    ''' <summary>
    ''' элемент коллекции
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public ReadOnly Property Item(ByVal Key As String) As Engine
        Get
            If Not Engines.Keys.Contains(Key) Then
                Throw New ArgumentOutOfRangeException(Key, "Данный Изделие недоступен.")
            End If
            Return Engines.Item(Key)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of Engine) Implements IEnumerable(Of Engine).GetEnumerator
        For Each key As String In Engines.Keys.ToArray
            Yield Engines(key)
        Next
    End Function

    ''' <summary>
    ''' перечислитель
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        'Return Engines.GetEnumerator
        GetEnumerator()
    End Function

    Public ReadOnly Property AllEnginesKeysToArray() As String()
        Get
            Return Engines.Keys.ToArray
        End Get
    End Property

    '''' <summary>
    '''' число текущих загруженных форм
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public ReadOnly Property Count() As Integer
    '    Get
    '        Return Engines.Count
    '    End Get
    'End Property

    '''' <summary>
    '''' удаление по номеру или имени или объекту?
    '''' </summary>
    '''' <param name="vntIndexKey"></param>
    '''' <remarks></remarks>
    'Public Sub Remove(ByRef vntIndexKey As String)
    '    ' если целый тип то по плавающему индексу, а если строковый то по ключу
    '    Engines.Remove(vntIndexKey)
    '    Me.mEngineCreated -= 1
    'End Sub

    'Public Sub Clear()
    '    Engines.Clear()
    'End Sub
#End Region

    Private ReadOnly Engines As New Dictionary(Of String, Engine) ' внутренняя коллекция для управления расшифровками
    Private enumDescriptionsList As List(Of String)    ' список описаний из типа перечислителя
    Private enumNamesList As List(Of String)           ' список значений элементов из типа перечислителя
    Private mEngineCreated As Integer ' внутренний счетчик для подсчета созданных Изделие можно использовать в заголовке

    Public Sub New()
        InitializeEngineManager()
    End Sub

    Private Sub InitializeEngineManager()
        Dim caption As String = $"Метод: {NameOf(InitializeEngineManager)}"
        Try
            'Паттерн        Абстрактная фабрика
            'Имя в проекте  EnginesManager()
            'Задача         Создавать конкретные модули проекта. Скрыть от главной формы все конкретные классы модулей проекта. 
            'Решение        Скрыть знание о конкретных классах 
            'Результат      Главная форма не знает конкретных классов модулей, и ее код остается неизменен при добавлении новых модулей в проект. 

            ' В качестве <Главного модуля> выступает форма настроек. Она создает множество модулей через абстрактную фабрику EnginesManager. 
            ' Для перечисления всех модулей программы используется перечисление EngineType. 
            ' при инициализации пробегается по значениям этого типа (через mEnumDescriptionsList) и вызывает фабрику для создания конкретной формы. 

            PopulateListEnumNamesAndDescriptions()

            ' по описанию перечислителя Изделие, фабрика создаёт экземпляр класса расшифровки и добавляется в словарь менеджера
            For Each itemDescription As String In enumDescriptionsList
                Try
                    ' при создании автоматом добавляется в коллекцию
                    If Not CreateEngine(itemDescription) Then ' там проверка на корректность
                        Dim text As String = $"Ошибка при добавлении класса Изделия с именем: {itemDescription}"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    End If
                Catch ex As Exception
                    Dim text As String = $"Ошибка при создании класса Изделия с именем: {itemDescription}:{vbCrLf}"
                    MessageBox.Show(text & ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                End Try
            Next
        Catch ex As Exception
            MessageBox.Show($"Ошибка загрузки классов Изделия в {NameOf(InitializeEngineManager)}",
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
        For Each value In [Enum].GetValues(GetType(EngineType))
            Dim fi As FieldInfo = GetType(EngineType).GetField([Enum].GetName(GetType(EngineType), value))
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

        enumNamesList.AddRange([Enum].GetNames(GetType(EngineType)).ToArray)
    End Sub

    ''' <summary>
    ''' Добавление класса Изделие в коллекцию
    ''' </summary>
    ''' <param name="descriptionEngine"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateEngine(ByVal descriptionEngine As String) As Boolean
        Dim clsEngine As Engine = Nothing
        Dim caption As String = $"Метод: {NameOf(CreateEngine)}"

        Try
            Select Case descriptionEngine
                Case cEngine99A
                    clsEngine = New Engine99A(ModeRegime)
                Case cEngine99B
                    clsEngine = New Engine99B(ModeRegime)
                Case cEngine39
                    clsEngine = New Engine39(ModeRegime)
                Case cEngineM1
                    clsEngine = New EngineM1(ModeRegime)
                Case cEngineM1_25_1
                    clsEngine = New EngineM1_25_1(ModeRegime)
                Case cEngine39_3
                    clsEngine = New Engine39_3(ModeRegime)
                Case Else
                    Exit Select
            End Select

            If clsEngine Is Nothing Then
                Dim text As String = $"Имя класса типа Изделие {descriptionEngine} не найдено при создании коллекции."
                MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return False
            End If

            Engines.Add(descriptionEngine, clsEngine)
            Me.mEngineCreated += 1

            ' здесь провести проверку на корректность и только если прошло продолжить
            If Engines.ContainsKey(descriptionEngine) Then
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