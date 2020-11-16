Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports Registration.SettingSelectedParameters.ManagerSelectedParameters
Imports Registration.SettingSelectedParameters.ManagerSelectedParameters.Book
Imports Registration.SettingSelectedParameters.ManagerSelectedParameters.Book.ListParameter

''' <summary>
''' Класс реализует обновление, восстановление и хранение настроек списков выборочного контроля,
''' навигацию и доступ к спискам для окна настройки или формы отображения выбранных параметров.
''' </summary>
Public Class SettingSelectedParameters
    Implements IEnumerable

#Region "ManagerSelectedParameters"
    '{"Books"
    '	[{"Key""TextControl","Value":{
    '		"CollectionLists":[{"
    '			NameList""Test",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]},{
    '			"NameList":"Список1",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]}],
    '		"LastKeyListParameter":"Test","NameBook":"TextControl"}},

    '	{"Key""GraphControl","Value":{
    '		"CollectionLists":[{
    '			"NameList":"Test",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]},{
    '			"NameList":"Список1",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]}],
    '		"LastKeyListParameter":"Test","NameBook":"GraphControl"}},

    '	{"Key""SelectiveControl","Value":{
    '		"CollectionLists":[{
    '			"NameList":"Test",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]},{
    '			"NameList":"Список1",
    '				"SelectedParameters":[{"NameParameter":"N1"},{"NameParameter":"N2"},{"NameParameter":"аРУД"}]}],
    '		"LastKeyListParameter":"Test","NameBook":"SelectiveControl"}}]}

    ''' <summary>
    '''  Упрощённый класс используемый для сохранения в файл настроек 
    '''  выборочных параметров для контроля при сериализации и дессириализации.
    ''' </summary>
    <DataContract>
    Public Class ManagerSelectedParameters

        ''' <summary>
        ''' Представляет выборочный список, тестовый контроль или графический контроль.
        ''' </summary>
        <DataContract>
        Public Class Book

            ''' <summary>
            ''' Список (группа) параметров отображаемая в 
            ''' выборочном списке, тестовом контроле или графическом контроле.
            ''' </summary>
            <DataContract>
            Public Class ListParameter

                ''' <summary>
                ''' Параметр для выборочного контроля
                ''' </summary>
                <DataContract>
                Public Class SelectedParameter
                    <DataMember>
                    Public Property NameParameter As String

                    Public Sub New(ByVal inName As String)
                        ' возможно при расширении добавлять новые свойства
                        NameParameter = inName
                    End Sub

                    Public Overrides Function ToString() As String
                        Return NameParameter
                    End Function
                End Class

                ''' <summary>
                ''' Список с выбранными параметрами
                ''' </summary>
                ''' <returns></returns>
                <DataMember>
                Public Property SelectedParameters As New List(Of SelectedParameter)

                ''' <summary>
                ''' Имя списка
                ''' </summary>
                ''' <returns></returns>
                <DataMember>
                Public Property NameList As String

                Public Overrides Function ToString() As String
                    Return NameList
                End Function
            End Class

            ''' <summary>
            ''' Коллекция списков с выбранными параметрами
            ''' </summary>
            ''' <returns></returns>
            <DataMember>
            Public Property CollectionLists As New List(Of ListParameter)

            ''' <summary>
            ''' Имя настроечного окна
            ''' </summary>
            ''' <returns></returns>
            <DataMember>
            Public Property NameBook As String

            ''' <summary>
            ''' Последний активный используемый список параметров
            ''' </summary>
            ''' <returns></returns>
            <DataMember>
            Public Property LastKeyListParameter As String
            Public Overrides Function ToString() As String
                Return NameBook
            End Function
        End Class

        ''' <summary>
        ''' Коллекция настроечных окон
        ''' </summary>
        ''' <returns></returns>
        <DataMember>
        Public Property Books As New Dictionary(Of String, Book)
    End Class

#End Region

    ''' <summary>
    ''' Набор окон выборочной настройки:
    ''' выборочный список, тестовый контроль или графический контроль.
    ''' </summary>
    Private Books As New Dictionary(Of String, Book)

    Default Public Property Item(key As String) As Book
        Get
            Return Books(key)
        End Get
        Set(value As Book)
            Books(key) = value
        End Set
    End Property

    'Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
    '    Return CalcDictionary.GetEnumerator()
    'End Function

    ' Реализация интерфейса IEnumerable предполагает стандартную реализацию перечислителя.
    ' Однако мы можем не полагаться на стандартную реализацию, а создать свою логику итератора с помощью ключевых слов Iterator и Yield.
    ' Конструкция итератора представляет метод, в котором используется ключевое слово Yield для перебора по коллекции или массиву.
    Public Iterator Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        For Each keyBook As String In Books.Keys.ToArray
            Yield Books(keyBook)
        Next
    End Function

    ''' <summary>
    ''' Текущее имя окна настройки
    ''' </summary>
    Public ReadOnly Property NameWorkBook As String

    ''' <summary>
    ''' Текущая рабочая книга (окна настройки)
    ''' </summary>
    Private workBook As Book

    ''' <summary>
    ''' Текущий список (для группы параметров) в рабочей книге
    ''' </summary>
    Private workListParameter As ListParameter

    Private _NameListParameter As String

    Public ReadOnly Property NameListParameter As String
        Get
            Return _NameListParameter
        End Get
    End Property

    ''' <summary>
    ''' Задать текущий список при выделении в контроле ComboBox
    ''' </summary>
    ''' <param name="inLastKeyListParameter"></param>
    Public Sub SetWorkListParameter(inLastKeyListParameter As String)
        workBook.LastKeyListParameter = inLastKeyListParameter
        'workListParameter = workBook.CollectionLists.Single(Function(List) List.NameList = inLastKeyListParameter)
        workListParameter = workBook.CollectionLists.Find(Function(List) List.NameList = inLastKeyListParameter)
        _NameListParameter = inLastKeyListParameter
    End Sub

    ''' <summary>
    ''' Коллекция списков (для группы параметров) в рабочей книге
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CollectionLists As String()
        Get
            Return workBook.CollectionLists.Select(Function(list) list.NameList).ToArray
        End Get
    End Property
    'Public ReadOnly Property CollectionLists As ListParameter()
    '    Get
    '        Return workBook.CollectionLists.ToArray
    '    End Get
    'End Property

    ''' <summary>
    ''' Текущий массив отмеченных параметров для текущего списка
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedParameters As List(Of SelectedParameter)
        Get
            Return workListParameter.SelectedParameters
        End Get
        Set
            workListParameter.SelectedParameters = Value
        End Set
    End Property

    ''' <summary>
    ''' Текущий строковый массив отмеченных параметров для текущего списка
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedParametersAsString As String()
        Get
            Return workListParameter.SelectedParameters.Select(Function(Param) Param.NameParameter).ToArray
        End Get
        Set
            workListParameter.SelectedParameters.Clear()
            'Dim lSelectedParameter As New List(Of SelectedParameter)
            'For Each itemName As String In Value
            '    lSelectedParameter.Add(New SelectedParameter(itemName))
            'Next
            'workListParameter.SelectedParameters = lSelectedParameter.ToArray

            For Each itemName As String In Value
                workListParameter.SelectedParameters.Add(New SelectedParameter(itemName))
            Next
        End Set
    End Property

    ''' <summary>
    ''' Количество параметров в рабочем списке.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property SelectedParametersCount As Integer
        Get
            Return workListParameter.SelectedParameters.Count
        End Get
    End Property

    ''' <summary>
    ''' Полный путь к файлу настроек
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PathFileControlParameters As String
    Private Const FileControlParameters As String = "ControlParameters.json"

    Public Sub New(inPathResourses As String, inNameBook As String)
        PathFileControlParameters = Path.Combine(inPathResourses, FileControlParameters)

        If Not File.Exists(PathFileControlParameters) Then CreateCongigByDefault()

        NameWorkBook = inNameBook
        ReadControlParameters()

        ' инициализировать рабочую конфигурацию
        If Not Books.TryGetValue(NameWorkBook, workBook) Then
            ' в случае отсутствия создать и записать
            'Books.Add(NameWorkBook, CreateBookByDefault(NameWorkBook))
            Books(NameWorkBook) = CreateBookByDefault(NameWorkBook)
            workBook = Books(NameWorkBook)
        End If

        If workBook.CollectionLists.Find(Function(list) list.NameList = workBook.LastKeyListParameter) Is Nothing Then
            SetWorkListParameter(workBook.CollectionLists.Last.NameList)
        Else
            SetWorkListParameter(workBook.LastKeyListParameter)
        End If
    End Sub

    ''' <summary>
    ''' Восстановление после записи при добавлении и удалении
    ''' </summary>
    Public Sub SaveAndRestoreAfterSaving()
        SaveControlParameters()
        ReadControlParameters()
        SetWorkListParameter(workBook.LastKeyListParameter)
    End Sub

    ''' <summary>
    ''' Обновление рабочего списка парметров в текущей книге
    ''' </summary>
    ''' <param name="inNames"></param>
    Public Sub UpdateListParameter(inNames As List(Of String))
        Dim newSelectedParameters As New List(Of SelectedParameter)

        For Each itemName As String In inNames
            newSelectedParameters.Add(New SelectedParameter(itemName))
        Next

        workListParameter.SelectedParameters = newSelectedParameters
        SaveAndRestoreAfterSaving()
    End Sub

    ''' <summary>
    ''' Создание и добавление нового списка парметров в текущей книге
    ''' </summary>
    ''' <param name="newNameList"></param>
    ''' <param name="inNames"></param>
    Public Sub InsertNewListParameter(newNameList As String, inNames As List(Of String))
        Dim newSelectedParameters As New List(Of SelectedParameter)

        For Each itemName As String In inNames
            newSelectedParameters.Add(New SelectedParameter(itemName))
        Next

        Dim newListParameter As New ListParameter With {
            .NameList = newNameList,
            .SelectedParameters = newSelectedParameters
        }

        workBook.CollectionLists.Add(newListParameter)
        SetWorkListParameter(newNameList)
        SaveAndRestoreAfterSaving()
    End Sub

    ''' <summary>
    ''' Удаление списка парметров в текущей книге
    ''' </summary>
    ''' <param name="deleteNameList"></param>
    Public Sub DeleteListParameter(deleteNameList As String)
        'workBook.CollectionLists.Remove(workBook.CollectionLists.Single(Function(list) list.NameList = deleteNameList))
        workBook.CollectionLists.Remove(workBook.CollectionLists.Find(Function(list) list.NameList = deleteNameList))
        SetWorkListParameter(workBook.CollectionLists.Last.NameList)
        SaveAndRestoreAfterSaving()
    End Sub

#Region "Read/Save ControlParameters"
    ''' <summary>
    ''' Сохранение произведенных изменений
    ''' </summary>
    Public Sub SaveControlParameters()
        Books(NameWorkBook) = workBook

        Dim saveManagerSelectedParameters As New ManagerSelectedParameters
        ' произвести полное отображение на прокси
        For Each itemBook As Book In Books.Values
            saveManagerSelectedParameters.Books(itemBook.NameBook) = itemBook
        Next

        ' отображение текущего содержимого в временный прокси для записи
        SerializerControlParameters(saveManagerSelectedParameters)
    End Sub

    ''' <summary>
    ''' Сериализация массива настроек для выборочного отображения в файл.
    ''' </summary>
    ''' <param name="inManagerInheritsConfigurations"></param>
    Private Sub SerializerControlParameters(inManagerInheritsConfigurations As ManagerSelectedParameters)
        Dim jsonFormatter As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(ManagerSelectedParameters))

        Using fs As New FileStream(PathFileControlParameters, FileMode.Create)
            jsonFormatter.WriteObject(fs, inManagerInheritsConfigurations)
        End Using
    End Sub

    ''' <summary>
    ''' Десериализация из файла в массив настроек для выборочного отображения.
    ''' </summary>
    Private Sub ReadControlParameters()
        Dim jsonFormatter As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(ManagerSelectedParameters))
        Dim mManagerSelectedParameters As ManagerSelectedParameters

        Using fs As New FileStream(PathFileControlParameters, FileMode.Open)
            mManagerSelectedParameters = CType(jsonFormatter.ReadObject(fs), ManagerSelectedParameters)
            'For Each p As ControlParameter In arrControlParameter
            '    Console.WriteLine("Имя: {0} --- Возраст: {1}", p.Name, p.Age)
            'Next
        End Using

        ' произвести полное отображение (копирование) с прокси на реальную коллекцию
        For Each itemBook As Book In mManagerSelectedParameters.Books.Values
            Books(itemBook.NameBook) = itemBook
        Next
    End Sub
#End Region

#Region "Создание и запись в файл конфигурации по умолчанию"

    ''' <summary>
    ''' Создание списка парметров по умолчанию
    ''' </summary>
    ''' <param name="inNameList"></param>
    ''' <returns></returns>
    Private Function CreateListParameterByDefault(inNameList As String) As ListParameter
        ' 1. --- SelectedParameter ------------------------------------------------
        'Dim selParam1 As SelectedParameter = New SelectedParameter("N1") ', 0)
        'Dim selParam2 As SelectedParameter = New SelectedParameter("N2") ', 1)
        'Dim selParam3 As SelectedParameter = New SelectedParameter("аРУД") ', 2)

        ' 2. --- ListParameter ----------------------------------------------------
        'Dim arrSelectedParameter1 As SelectedParameter() = {selParam1, selParam2, selParam3}
        'myListParameters1.SelectedParameters.AddRange(arrSelectedParameter1)
        'myListParameters1.SelectedParameters = arrSelectedParameter1.ToDictionary(Function(sr) sr.NameParameter)
        'Dim _SelectedParameters As New List(Of SelectedParameter)
        '_SelectedParameters.AddRange({selParam1, selParam2, selParam3})
        'Dim _SelectedParameters As New List(Of SelectedParameter)(arrSelectedParameter1)

        Return New ListParameter With {
            .NameList = inNameList,
            .SelectedParameters = New List(Of SelectedParameter)({New SelectedParameter("N1"), New SelectedParameter("N2"), New SelectedParameter("аРУД")})
        }
    End Function

    ''' <summary>
    ''' Создание набора списков для окна выборочной настройки
    ''' </summary>
    ''' <param name="inNameBook"></param>
    ''' <returns></returns>
    Private Function CreateBookByDefault(inNameBook As String) As Book
        ' 3. --- Book -------------------------------------------------------------
        Return New Book With {
            .NameBook = inNameBook,
            .LastKeyListParameter = "Test",
            .CollectionLists = {CreateListParameterByDefault("Test"), CreateListParameterByDefault("Список1")}.ToList
        }
    End Function

    ''' <summary>
    ''' Создание и запись в файл конфигурации по умолчанию
    ''' </summary>
    Private Sub CreateCongigByDefault()
        ' 4. --- Configurations ---------------------------------------------------
        ' TextControl - "Текстовый контроль"
        ' GraphControl - "Графический контроль"
        ' SelectiveControl - "Выборочный контроль"
        Dim defaultManagerSelectedParameters As New ManagerSelectedParameters With {
            .Books = {CreateBookByDefault(TextControlFormTuning),
            CreateBookByDefault(GraphControlFormTuning),
            CreateBookByDefault(SelectiveControlFormTuning)}.ToDictionary(Function(book) book.NameBook)
        }
        ' Запись
        SerializerControlParameters(defaultManagerSelectedParameters)
    End Sub
#End Region

End Class