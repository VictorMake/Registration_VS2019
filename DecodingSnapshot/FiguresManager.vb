Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection

''' <summary>
''' Менеджер управления классами примитивов построений в расшифровках.
''' Абстрактная фабрика осведомлена о всех конкретных класссах и способна порождать их экземпляры. 
''' Метод CreateFigure создает класс заданного типа и назначает ему медиатор. 
''' </summary>
''' <remarks></remarks>
Friend Class FiguresManager
    Implements IEnumerable
    Implements IEnumerable(Of Figure)

#Region "Interface IEnumerable"
    ''' <summary>
    ''' Оболочка коллекции Figure
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property DecodingFigures() As Dictionary(Of EnumFigures, Figure)
        Get
            Return DecryptionsFigures
        End Get
    End Property

    ''' <summary>
    ''' элемент коллекции
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public ReadOnly Property Item(ByVal Key As EnumFigures, parameter As String) As Figure
        Get
            If Not figuresEnabled.Contains(Key.ToString) Then
                Throw New ArgumentOutOfRangeException(Key.ToString, "Данный примитив построения отсутствует в коллекции.")
            End If
            Return GetFigure(Key, parameter)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of Figure) Implements IEnumerable(Of Figure).GetEnumerator
        For Each key As EnumFigures In DecryptionsFigures.Keys.ToArray
            Yield DecryptionsFigures(key)
        Next
    End Function

    ''' <summary>
    ''' перечислитель
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        'Return Figures.GetEnumerator
        GetEnumerator()
    End Function

    ''' <summary>
    ''' число загруженных примитивов
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count() As Integer
        Get
            Return DecryptionsFigures.Count
        End Get
    End Property

    '''' <summary>
    '''' удаление по номеру или имени или объекту?
    '''' </summary>
    '''' <param name="vntIndexKey"></param>
    '''' <remarks></remarks>
    'Public Sub Remove(ByRef vntIndexKey As String)
    '    ' если целый тип то по плавающему индексу, а если строковый то по ключу
    '    Figures.Remove(vntIndexKey)
    '    Me.mFigureCreated -= 1
    'End Sub

    'Public Sub Clear()
    '    Figures.Clear()
    'End Sub

    'Protected Overrides Sub Finalize()
    '    Figures = Nothing
    '    MyBase.Finalize()
    'End Sub
#End Region

    Private Property Mediator As FormSnapshotViewingDiagram
    Private ReadOnly figuresEnabled As New List(Of String)  ' список разрешенных к загрузке расшифровоки
    Private descriptionsEnumFigures As List(Of String)      ' список описаний из типа перечислителя
    Private namesEnumFigures As List(Of EnumFigures)        ' список значений элементов из типа перечислителя
    Private ReadOnly DecryptionsFigures As New Dictionary(Of EnumFigures, Figure)    ' внутренняя ленивая коллекция для управления расшифровками
    ' коллекция фабрик расшифровок
    Private ReadOnly CreatorsFigures As Dictionary(Of EnumFigures, CreatorFigure) = New Dictionary(Of EnumFigures, CreatorFigure) From {
            {EnumFigures.ДлительностьЗабросаПровала, New CreatorFigureДлительностьЗабросаПровала},
            {EnumFigures.ДлительностьОтИндексаДоСтабильногоРоста, New CreatorFigureДлительностьОтИндексаДоСтабильногоРоста},
            {EnumFigures.ДлительностьФронтаОтИндексаДоN1Уст_2, New CreatorFigureДлительностьФронтаОтИндексаДоN1Уст_2},
            {EnumFigures.ДлительностьФронтаСпада, New CreatorFigureДлительностьФронтаСпада},
            {EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, New CreatorFigureДлительностьФронтаСпадаОтИндексаДоУровня},
            {EnumFigures.ДлительностьФронтаСпадаПрОборотов, New CreatorFigureДлительностьФронтаСпадаПрОборотов},
            {EnumFigures.ЗабросN1ОтносительноУстановившегося, New CreatorFigureЗабросN1ОтносительноУстановившегося},
            {EnumFigures.ЗначениеПараметраВИндексе, New CreatorFigureЗначениеПараметраВИндексе},
            {EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, New CreatorFigureМинимальноеМаксимальноеЗначениеПараметра},
            {EnumFigures.ПостроениеНаклонной, New CreatorFigureПостроениеНаклонной},
            {EnumFigures.ПровалN1ОтносительноУстановившегося, New CreatorFigureПровалN1ОтносительноУстановившегося},
            {EnumFigures.ПровалЗаНП96, New CreatorFigureПровалЗаНП96},
            {EnumFigures.РискиНастроекПараметров, New CreatorFigureРискиНастроекПараметров}
        }

    Public Sub New(mediator As FormSnapshotViewingDiagram)
        Me.Mediator = mediator
    End Sub

    Public Sub Initialize()
        PopulateFiguresEnabledList()
    End Sub

    ''' <summary>
    ''' Заполняется лист имён доступных расшифровок на основании дескрипторов перечислителя 
    ''' и списка расшифровок DecryptionsFigures чтобы не было пропущено.
    ''' </summary>
    Private Sub PopulateFiguresEnabledList()
        Dim caption As String = $"Метод: {NameOf(PopulateFiguresEnabledList)}"

        Try
            PopulateListEnumNamesAndDescriptions()

            For Each itemDescription As String In descriptionsEnumFigures
                Dim result As EnumFigures ' itemDescription привести к типу EnumFigures
                If [Enum].TryParse(itemDescription, result) Then
                    'If CreatorsFigures.ContainsKey(itemDescription) Then
                    If CreatorsFigures.Keys.Contains(result) Then
                        figuresEnabled.Add(itemDescription)
                    Else
                        Dim text As String = $"Имя класса расшифровки <{itemDescription}> не найдено в среди доступных фабрик-создателей."
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    End If
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
        descriptionsEnumFigures = New List(Of String)
        namesEnumFigures = New List(Of EnumFigures)

        ' получить все аттрибуты перечислителя для создания списка возможных окон в системе
        For Each value In [Enum].GetValues(GetType(EnumFigures))
            Dim fi As FieldInfo = GetType(EnumFigures).GetField([Enum].GetName(GetType(EnumFigures), value))
            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

            If dna IsNot Nothing Then
                descriptionsEnumFigures.Add(dna.Description)
            Else
                descriptionsEnumFigures.Add("Нет описания")
            End If

            namesEnumFigures.Add(CType(value, EnumFigures))
        Next

        ' то же самое по другому
        'For Each c In TypeDescriptor.GetConverter(GetType(WindowsForms)).GetStandardValues
        '    Dim dna As DescriptionAttribute = GetType(WindowsForms).GetField([Enum].GetName(GetType(WindowsForms), c)).GetCustomAttributes(GetType(DescriptionAttribute), True)(0)
        '    If dna IsNot Nothing Then
        '        descriptionsEnumFigures.Add(dna.Description)
        '    Else
        '        descriptionsEnumFigures.Add(WindowsForms.РедакторПерекладок.ToString())
        '    End If
        'Next     

        'namesEnumFigures.AddRange([Enum].GetNames(GetType(EnumFigures)).ToArray)
    End Sub

    ''' <summary>
    ''' Выдать класс примитива прямо из коллекции если он был создан хотя-бы один раз,
    ''' в противном случае создать его и добавить в коллекцию.
    ''' Ленивая инициализвция.
    ''' </summary>
    ''' <param name="nameFigure"></param>
    ''' <returns></returns>
    Private Function GetFigure(ByVal nameFigure As EnumFigures, parameter As String) As Figure
        'If DecryptionsFigures.ContainsKey(nameFigure) Then
        '    Return DecryptionsFigures.Item(nameFigure)
        'Else
        Return CreateNewFigure(nameFigure, parameter)
        'End If
    End Function

    ''' <summary>
    ''' Добавление класса примитива в коллекцию
    ''' </summary>
    ''' <param name="nameFigure"></param>
    ''' <returns></returns>
    Private Function CreateNewFigure(ByVal nameFigure As EnumFigures, parameter As String) As Figure
        Dim newFigure As Figure = Nothing
        Dim caption As String = $"Метод: {NameOf(CreateNewFigure)}"

        Try
            ' по описанию перечислителя примитивов, фабрика создаёт экземпляр класса расшифровки и добавляется в словарь менеджера

            'Паттерн        Абстрактная фабрика
            'Имя в проекте  FigureManager()
            'Задача         Создавать конкретные модули проекта. Скрыть от главной формы все конкретные классы модулей проекта. 
            'Решение        Скрыть знание о конкретных классах 
            'Результат      Главная форма не знает конкретных классов модулей, и ее код остается неизменен при добавлении новых модулей в проект. 
            If CreatorsFigures.Keys.Contains(nameFigure) Then newFigure = CType(CreatorsFigures(nameFigure).GetFigure(parameter, Mediator), Figure)

            If newFigure Is Nothing Then
                Dim text As String = $"Ошибка при создании класса расшифровки:<{nameFigure}> из фабрики-создателя."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return Nothing
            End If

            ' при создании автоматом добавляется в коллекцию
            'DecryptionsFigures.Add(nameFigure, newFigure)

            ' здесь провести проверку на корректность и только если прошло продолжить
            'If DecryptionsFigures.ContainsKey(nameFigure) Then
            Return newFigure
            'Else
            '    Return Nothing
            'End If
        Catch exp As Exception
            Dim text As String = exp.Message
            MessageBox.Show(text, $"{caption}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Return Nothing
        End Try
    End Function
End Class