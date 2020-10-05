Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Design

' условия триггеров  и формул в действии работают по логике AND, 
' т.е. когда в действии сработают все триггеры и формулы, то сработает линия,
' а действия работают как OR (например на одину линию порта в двух действиях могут быть разные услови, 
' тогда по любому выполнению из условий срабатывает линия)

' Очень часто в программах встречаются сложные структуры, представляющие собой дерево или граф, 
' состоящий из разнотипных узлов. И, конечно же, при этом имеется необходимость обрабатывать этот граф. 
' Самое очевидное решение - добавить в базовый класс виртуальный метод, который перекрыть в наследниках 
' для выполнения нужного Конфигурация и осуществления дальнейшей навигации по дереву.
' Однако у кода в этом примере есть серьезный недостаток: в нем структура данных оказывается увязанной 
' с обрабатывающими ее алгоритмами. Если нам понадобится алгоритм, отличный от реализованного, 
' то придется добавлять еще один виртуальный метод. Еще хуже, если классы, составляющие дерево, 
' содержатся в недоступном для модификации коде.

' Одним из вариантов решения проблемы высокой связности в данном случае является паттерн Посетитель.

Module ModuleExplorer


    Enum TypeGridDigitalNode
        <Description("Все Конфигурации")>
        AllConfigurations
        <Description("Конфигурация")>
        Configuration
        <Description("Все Действия")>
        AllActions
        <Description("Действие")>
        Action
        <Description("Все Триггеры")>
        AllTriggers
        <Description("Все Формулы")>
        AllFormulas
        <Description("Все Порты")>
        AllPorts
        <Description("Триггер")>
        Trigger
        <Description("Формула")>
        Formula
        <Description("Порт")>
        Port
        <Description("Все Аргументы")>
        AllArguments
        <Description("Все установленные Биты")>
        AllEnableBits
        <Description("Аргумент")>
        Argument
        <Description("Бит")>
        Bit
        <Description("Неизв.")>
        Unknown
    End Enum
    Public TypeNode As TypeGridDigitalNode = TypeGridDigitalNode.Unknown

    Public GlobalAllConfigurations As ВсеКонфигурации
    Public IsNotCreatePropertyEditorSource As Boolean ' Не Создавать Редакторы Свойств
    Public PropertiesChanging As Boolean

    Public MemoKeyConfigurationAction As Integer
    Public MemoKeyAction As Integer
    Public MemoKeyTrigger As Integer
    Public MemoKeyFormula As Integer
    Public MemoКеуArgument As Integer
    Public MemoKeyPort As Integer
    Public MemoKeyBit As Integer

    Private isLoadingFromDataSet As Boolean ' идет Считывание Из DataSet 

#Region "CreateTree"
    Public Sub CreateTree()
        'Dim strSql As String
        'запрос выбирающий всё не работает
        'strSql = "SELECT КонфигурацияДействий.*, Действие.*, Порты.*, БитПорта.*, ФормулаСрабатыванияЦифровогоВыхода.*, АргументыДляФормулы.*, ТриггерСрабатыванияЦифровогоВыхода.* " & _
        '"FROM (((((КонфигурацияДействий RIGHT JOIN Действие ON КонфигурацияДействий.keyКонфигурацияДействия = Действие.keyКонфигурацияДействия) RIGHT JOIN ТриггерСрабатыванияЦифровогоВыхода ON Действие.keyДействие = ТриггерСрабатыванияЦифровогоВыхода.keyДествия) RIGHT JOIN ФормулаСрабатыванияЦифровогоВыхода ON Действие.keyДействие = ФормулаСрабатыванияЦифровогоВыхода.keyДействие) RIGHT JOIN АргументыДляФормулы ON ФормулаСрабатыванияЦифровогоВыхода.KeyFormula = АргументыДляФормулы.KeyFormula) RIGHT JOIN Порты ON Действие.keyДействие = Порты.keyДействие) RIGHT JOIN БитПорта ON Порты.KeyПорта = БитПорта.KeyПорта;"
        'запрос выбирающий всё но с подзапросами
        'SELECT КонфигурацияДействий.*, Действие_Остальное.*, КонфигурацияДействий.ИмяКонфигурации
        'FROM КонфигурацияДействий INNER JOIN Действие_Остальное ON КонфигурацияДействий.keyКонфигурацияДействия = Действие_Остальное.keyКонфигурацияДействия
        'WHERE (((КонфигурацияДействий.ИмяКонфигурации)="Проба"));
        'надо сделать типизированную таблицу в памяти и работать с ней
        GlobalAllConfigurations = New ВсеКонфигурации(GetNewConfigurations(DigitalPortForm.ChannelsDigitalOutputDataSet.КонфигурацияДействий))
        ' устанавливаем редактируемый объект
        DigitalPortForm.propertyGrid1.SelectedObject = GlobalAllConfigurations
        DigitalPortForm.PopulateComboBoxAndListBoxConfigurations()
    End Sub

    Private Function GetNewConfigurations(tblConfigurationAction As ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable) As List(Of Конфигурация)
        isLoadingFromDataSet = True
        Dim Configurations As List(Of Конфигурация) = New List(Of Конфигурация)

        For Each rowConfiguration As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow In tblConfigurationAction.Rows
            Dim newConfiguration As Конфигурация = New Конфигурация(rowConfiguration.ИмяКонфигурации, New Действие() {})

            Dim newActions As List(Of Действие) = GetNewActions(rowConfiguration, newConfiguration)

            newConfiguration.Действия = newActions
            newConfiguration.KeyКонфигурацияДействия = rowConfiguration.keyКонфигурацияДействия

            If Not IsDBNull(rowConfiguration.Описание) Then
                newConfiguration.Описание = rowConfiguration.Описание
            End If

            Configurations.Add(newConfiguration)
        Next

        isLoadingFromDataSet = False
        Return Configurations
    End Function

    Private Function GetNewActions(rowConfiguration As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow, newConfiguration As Конфигурация) As List(Of Действие)
        Dim newActions As New List(Of Действие)

        For Each rowAction As ChannelsDigitalOutputDataSet.ДействиеRow In rowConfiguration.GetДействиеRows
            Dim newAction As Действие = New Действие(rowAction.ИмяДействия, New ТриггерСрабатыванияЦифровогоВыхода() {}, New ФормулаСрабатыванияЦифровогоВыхода() {}, New Порт() {})
            Dim newTriggers As List(Of ТриггерСрабатыванияЦифровогоВыхода) = GetNewTriggers(rowAction, newAction)
            Dim newFormulas As List(Of ФормулаСрабатыванияЦифровогоВыхода) = GetNewFormulas(rowAction, newAction)
            Dim newPorts As List(Of Порт) = GetNewPorts(rowAction, newAction)

            newAction.Parent = newConfiguration
            newAction.ТриггерыСрабатыванияЦифровогоВыхода = newTriggers
            newAction.ФормулыСрабатыванияЦифровогоВыхода = newFormulas
            newAction.Порты = newPorts
            newAction.keyДействие = rowAction.keyДействие
            newAction.keyКонфигурацияДействия = rowAction.keyКонфигурацияДействия
            newActions.Add(newAction)
        Next

        Return newActions
    End Function

    Private Function GetNewTriggers(rowAction As ChannelsDigitalOutputDataSet.ДействиеRow, newAction As Действие) As List(Of ТриггерСрабатыванияЦифровогоВыхода)
        Dim newTriggers As New List(Of ТриггерСрабатыванияЦифровогоВыхода)
        For Each rowTrigger As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow In rowAction.GetТриггерСрабатыванияЦифровогоВыходаRows
            newTriggers.Add(New ТриггерСрабатыванияЦифровогоВыхода(rowTrigger.name) With {.Parent = newAction,
                                                                                                    .ИмяКанала = rowTrigger.ИмяКанала,
                                                                                                    .ОперацияСравнения = rowTrigger.ОперацияСравнения,
                                                                                                    .ВеличинаУсловия = rowTrigger.ВеличинаУсловия,
                                                                                                    .ВеличинаУсловия2 = rowTrigger.ВеличинаУсловия2,
                                                                                                    .KeyТриггер = rowTrigger.KeyТриггер,
                                                                                                    .keyДействие = rowTrigger.keyДествие})
        Next

        Return newTriggers
    End Function

    Private Function GetNewFormulas(rowAction As ChannelsDigitalOutputDataSet.ДействиеRow, newAction As Действие) As List(Of ФормулаСрабатыванияЦифровогоВыхода)
        Dim newFormulas As New List(Of ФормулаСрабатыванияЦифровогоВыхода)
        For Each rowFormula As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow In rowAction.GetФормулаСрабатыванияЦифровогоВыходаRows
            Dim newFormula As New ФормулаСрабатыванияЦифровогоВыхода(rowFormula.name, New АргументДляФормулы() {})
            Dim newArguments As List(Of АргументДляФормулы) = GetNewArguments(rowFormula, newFormula)

            newFormula.Parent = newAction
            newFormula.АргументыДляФормулы = newArguments
            newFormula.Формула = rowFormula.Формула
            newFormula.ОперацияСравнения = rowFormula.ОперацияСравнения
            newFormula.ВеличинаУсловия = rowFormula.ВеличинаУсловия
            newFormula.KeyFormula = rowFormula.KeyFormula
            newFormula.keyДействие = rowFormula.keyДействие
            newFormulas.Add(newFormula)
        Next

        Return newFormulas
    End Function

    Private Function GetNewArguments(rowFormula As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow, newFormula As ФормулаСрабатыванияЦифровогоВыхода) As List(Of АргументДляФормулы)
        Dim newArguments As New List(Of АргументДляФормулы)

        For Each rowArgument As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow In rowFormula.GetАргументыДляФормулыRows
            newArguments.Add(New АргументДляФормулы(rowArgument.ИмяАргумента) With {.Parent = newFormula,
                                                                                    .ИмяКанала = rowArgument.ИмяКанала,
                                                                                    .Приведение = rowArgument.Приведение,
                                                                                    .КеуArgument = rowArgument.КеуArgument,
                                                                                    .KeyFormula = rowArgument.KeyFormula})
        Next

        Return newArguments
    End Function

    Private Function GetNewPorts(rowAction As ChannelsDigitalOutputDataSet.ДействиеRow, newAction As Действие) As List(Of Порт)
        Dim newPorts As New List(Of Порт)
        For Each rowPort As ChannelsDigitalOutputDataSet.ПортыRow In rowAction.GetПортыRows
            Dim newPort As New Порт(rowPort.name, New Бит() {})
            Dim newBits As List(Of Бит) = GetNewBits(rowPort, newPort)

            newPort.Parent = newAction
            newPort.Биты = newBits
            newPort.НомерУстройства = rowPort.НомерУстройства
            newPort.НомерМодуляКорзины = rowPort.НомерМодуляКорзины
            newPort.НомерПорта = rowPort.НомерПорта
            newPort.KeyПорта = rowPort.KeyПорта
            newPort.keyДействие = rowPort.keyДействие
            newPorts.Add(newPort)
        Next

        Return newPorts
    End Function

    Private Function GetNewBits(rowPort As ChannelsDigitalOutputDataSet.ПортыRow, newPort As Порт) As List(Of Бит)
        Dim newBits As New List(Of Бит)

        For Each rowBit As ChannelsDigitalOutputDataSet.БитПортаRow In rowPort.GetБитПортаRows
            newBits.Add(New Бит(rowBit.name) With {.Parent = newPort,
                                                    .keyБитПорта = rowBit.keyБитПорта,
                                                    .KeyПорта = rowBit.KeyПорта,
                                                    .НомерБита = rowBit.НомерБита})
        Next

        Return newBits
    End Function
#End Region

#Region "SaveTree"
    Public Sub SaveTree()
        With DigitalPortForm
            .Cursor = Cursors.WaitCursor
            ' очистка
            .FillConfigurationTableAdapter()
            'Dim tblКонфигурацияДействий As ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable = frmDigitalOutputPort.ChannelsDigitalOutputDataSet.КонфигурацияДействий
            'tblКонфигурацияДействий.Clear()
            If .ChannelsDigitalOutputDataSet.КонфигурацияДействий.Rows.Count > 0 Then
                For I As Integer = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.Rows.Count - 1 To 0 Step -1
                    .ChannelsDigitalOutputDataSet.КонфигурацияДействий.Rows(I).Delete()
                Next
                .SaveModificationConfiguration()
            End If

            For Each itemConfiguration As Конфигурация In GlobalAllConfigurations.ВсеКонфигурации
                Dim newRowConfigurationAction As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow _
                = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.NewКонфигурацияДействийRow ' новая строка

                'newrowКонфигурация.Key = нет ' Для новой записи обязательно внешний ключ
                newRowConfigurationAction.ИмяКонфигурации = itemConfiguration.Name
                newRowConfigurationAction.Описание = itemConfiguration.Описание
                ' запись и получение нового keyКонфигурацияДействия
                .ChannelsDigitalOutputDataSet.КонфигурацияДействий.AddКонфигурацияДействийRow(newRowConfigurationAction)
                .UpdateConfigurationTableAdapter()
                .FillConfigurationTableAdapter()
                itemConfiguration.KeyКонфигурацияДействия = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.Last.keyКонфигурацияДействия
                MemoKeyConfigurationAction = itemConfiguration.KeyКонфигурацияДействия

                UpdateActionTable()
            Next

            .FillAllAdapters()
            CreateTree()
            .DtvwDirectory.RefreshTree()
            PropertiesChanging = False
            .Cursor = Cursors.Default
        End With
    End Sub

    Private Sub UpdateActionTable()
        With DigitalPortForm
            .FillActionTableAdapter(MemoKeyConfigurationAction) ' пустая таблица
            For Each itemAction As Действие In GlobalAllConfigurations.ItemКонфигурация.Действия
                Dim newRowAction As ChannelsDigitalOutputDataSet.ДействиеRow _
                = .ChannelsDigitalOutputDataSet.Действие.NewДействиеRow ' новая строка

                newRowAction.keyКонфигурацияДействия = MemoKeyConfigurationAction ' Для новой записи обязательно внешний ключ
                newRowAction.ИмяДействия = itemAction.Name
                ' запись и получение нового keyДействие
                .ChannelsDigitalOutputDataSet.Действие.AddДействиеRow(newRowAction)
                .UpdateActionTableAdapter()
                .FillActionTableAdapter(MemoKeyConfigurationAction)
                itemAction.keyДействие = .ChannelsDigitalOutputDataSet.Действие.Last.keyДействие
                MemoKeyAction = itemAction.keyДействие

                UpdateTriggerFireDigitalOutputTable()
                UpdateFormulaFireDigitalOutputTable()
                UpdatePortsTable()
            Next
        End With
    End Sub

    Private Sub UpdateTriggerFireDigitalOutputTable()
        With DigitalPortForm
            .FillTriggerFireDigitalOutputTableAdapter(MemoKeyAction) ' пустая таблица
            For Each itemTrigger As ТриггерСрабатыванияЦифровогоВыхода In GlobalAllConfigurations.ItemКонфигурация.ItemДействие.ТриггерыСрабатыванияЦифровогоВыхода
                Dim newRowTrigger As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow _
                = .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.NewТриггерСрабатыванияЦифровогоВыходаRow ' новая строка

                newRowTrigger.keyДествие = MemoKeyAction ' Для новой записи обязательно
                newRowTrigger.name = itemTrigger.Name
                newRowTrigger.ИмяКанала = itemTrigger.ИмяКанала
                newRowTrigger.ОперацияСравнения = itemTrigger.ОперацияСравнения
                newRowTrigger.ВеличинаУсловия = Convert.ToSingle(itemTrigger.ВеличинаУсловия)
                newRowTrigger.ВеличинаУсловия2 = Convert.ToSingle(itemTrigger.ВеличинаУсловия2)
                ' запись 
                .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.AddТриггерСрабатыванияЦифровогоВыходаRow(newRowTrigger)
                .UpdateTriggerFireDigitalOutputTableAdapter()
            Next
        End With
    End Sub

    Private Sub UpdateFormulaFireDigitalOutputTable()
        With DigitalPortForm
            .FillFormulaFireDigitalOutputTableAdapter(MemoKeyAction) ' пустая таблица
            For Each itemFormula As ФормулаСрабатыванияЦифровогоВыхода In GlobalAllConfigurations.ItemКонфигурация.ItemДействие.ФормулыСрабатыванияЦифровогоВыхода
                Dim newRowFormula As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow _
                = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.NewФормулаСрабатыванияЦифровогоВыходаRow ' новая строка

                newRowFormula.keyДействие = MemoKeyAction ' Для новой записи обязательно внешний ключ
                newRowFormula.name = itemFormula.Name
                newRowFormula.Формула = itemFormula.Формула
                newRowFormula.ОперацияСравнения = itemFormula.ОперацияСравнения
                newRowFormula.ВеличинаУсловия = Convert.ToSingle(itemFormula.ВеличинаУсловия)

                .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.AddФормулаСрабатыванияЦифровогоВыходаRow(newRowFormula)
                ' запись и получение нового .KeyFormula
                .UpdateFormulaFireDigitalOutputTableAdapter()
                .FillFormulaFireDigitalOutputTableAdapter(MemoKeyAction)
                itemFormula.KeyFormula = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.Last.KeyFormula
                MemoKeyFormula = itemFormula.KeyFormula

                UpdateArgumentsOfFormulaTable()
            Next
        End With
    End Sub

    Private Sub UpdateArgumentsOfFormulaTable()
        With DigitalPortForm
            .FillArgumentsOfFormulaTableAdapter(MemoKeyFormula) ' пустая таблица
            For Each itemArgument As АргументДляФормулы In GlobalAllConfigurations.ItemКонфигурация.ItemДействие.ItemФормула.АргументыДляФормулы
                Dim newRowArgument As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow _
                = .ChannelsDigitalOutputDataSet.АргументыДляФормулы.NewАргументыДляФормулыRow ' новая строка

                newRowArgument.KeyFormula = MemoKeyFormula ' Для новой записи обязательно внешний ключ
                newRowArgument.ИмяАргумента = itemArgument.Name
                newRowArgument.ИмяКанала = itemArgument.ИмяКанала
                newRowArgument.Приведение = itemArgument.Приведение

                .ChannelsDigitalOutputDataSet.АргументыДляФормулы.AddАргументыДляФормулыRow(newRowArgument)
            Next
            .UpdateArgumentsOfFormulaTableAdapter()
        End With
    End Sub

    Private Sub UpdatePortsTable()
        With DigitalPortForm
            .FillPortsTableAdapter(MemoKeyAction) ' пустая таблица
            For Each itenPort As Порт In GlobalAllConfigurations.ItemКонфигурация.ItemДействие.Порты
                Dim newRowPort As ChannelsDigitalOutputDataSet.ПортыRow _
                = .ChannelsDigitalOutputDataSet.Порты.NewПортыRow ' новая строка

                newRowPort.keyДействие = MemoKeyAction ' Для новой записи обязательно внешний ключ
                newRowPort.name = itenPort.Name
                newRowPort.НомерУстройства = Convert.ToInt16(itenPort.НомерУстройства)
                newRowPort.НомерМодуляКорзины = itenPort.НомерМодуляКорзины
                newRowPort.НомерПорта = Convert.ToInt16(itenPort.НомерПорта)
                ' запись и получение нового .KeyПорта
                .ChannelsDigitalOutputDataSet.Порты.AddПортыRow(newRowPort)
                .UpdatePortsTableAdapter() ' здесь обновляется .KeyПорта
                .FillPortsTableAdapter(MemoKeyAction)
                ' получить новый KeyПорта
                itenPort.KeyПорта = .ChannelsDigitalOutputDataSet.Порты.Last.KeyПорта
                MemoKeyPort = itenPort.KeyПорта

                UpdateBitsOfPortTable()
            Next
        End With
    End Sub

    Private Sub UpdateBitsOfPortTable()
        With DigitalPortForm
            .FillBitsOfPortTableAdapter(MemoKeyPort) ' таблица пустая
            For Each itenBit As Бит In GlobalAllConfigurations.ItemКонфигурация.ItemДействие.ItemПорт.Биты
                Dim newRowBit As ChannelsDigitalOutputDataSet.БитПортаRow _
                = .ChannelsDigitalOutputDataSet.БитПорта.NewБитПортаRow ' новая строка

                newRowBit.KeyПорта = MemoKeyPort ' Для новой записи обязательно внешний ключ
                newRowBit.name = itenBit.Name
                newRowBit.НомерБита = Convert.ToInt16(itenBit.НомерБита)
                ' .keyБитПорта присвоится после записи
                .ChannelsDigitalOutputDataSet.БитПорта.AddБитПортаRow(newRowBit)
            Next
            .UpdateBitsOfPortTableAdapter() 'запись
        End With
    End Sub
#End Region

    Public Interface IContextVisitor(Of C)
        Sub Visit(ByVal context As C, ByVal node As ВсеКонфигурации)
        Sub Visit(ByVal context As C, ByVal node As Конфигурация)
        Sub Visit(ByVal context As C, ByVal node As Действие)
        Sub Visit(ByVal context As C, ByVal node As ТриггерСрабатыванияЦифровогоВыхода)
        Sub Visit(ByVal context As C, ByVal node As ФормулаСрабатыванияЦифровогоВыхода)
        Sub Visit(ByVal context As C, ByVal node As Порт)
        Sub Visit(ByVal context As C, ByVal node As АргументДляФормулы)
        Sub Visit(ByVal context As C, ByVal node As Бит)
    End Interface

    Public Interface IRefreshContextVisitor(Of C)
        Sub UpdateNode(ByVal context As C, ByVal node As ВсеКонфигурации)
        Sub UpdateNode(ByVal context As C, ByVal node As Конфигурация)
        Sub UpdateNode(ByVal context As C, ByVal node As Действие)
        Sub UpdateNode(ByVal context As C, ByVal node As ТриггерСрабатыванияЦифровогоВыхода)
        Sub UpdateNode(ByVal context As C, ByVal node As ФормулаСрабатыванияЦифровогоВыхода)
        Sub UpdateNode(ByVal context As C, ByVal node As Порт)
        Sub UpdateNode(ByVal context As C, ByVal node As АргументДляФормулы)
        Sub UpdateNode(ByVal context As C, ByVal node As Бит)
    End Interface
    ' Использование перегрузки методов
    ' В языках, поддерживающих перегрузку методов, ее можно использовать для упрощения реализации метода Accept.

    ' Для использования этой техники необходимо, чтобы все методы посетителя назывались одинаково и различались 
    ' лишь типом параметра. В этом случае код метода Accept будет одинаковым. Компилятор при компиляции сам выберет необходимый метод.

    'DirectoryTreeView
    Public Class MyContextClass
        ' сделал для чего-то общего

        Public Sub New(ByVal ПозицияValue As DirectoryTreeView)
            DirectoryTreeView = ПозицияValue
            SingletonContextClass = Me
        End Sub

        ''' <summary>
        ''' Экземпляр Context Class
        ''' </summary>
        Public Shared SingletonContextClass As MyContextClass
        'Public ReadOnly Property ЭкземплярContextClass() As MyContextClass
        '    Get
        '        Me()
        '    End Get
        'End Property

        Public Property DirectoryTreeView() As DirectoryTreeView

        Public Property CurrentDir() As TreeNode
    End Class

    'Public Class MyContextClass
    '    'сделал для чего-то общего
    '    Private myПозицияValue As Integer

    '    Public Sub New(ByVal ПозицияValue As Integer)
    '        myПозицияValue = ПозицияValue
    '    End Sub

    '    Public Property ПозицияProperty() As Integer
    '        Get
    '            Return myПозицияValue
    '        End Get
    '        Set(ByVal value As Integer)
    '            myПозицияValue = value
    '        End Set
    '    End Property

    '    'Public Shared Narrowing Operator CType(ByVal initialData As String) As MyContextClass
    '    '    Return New MyContextClass(0)
    '    'End Operator

    '    'Public Shared Widening Operator CType(ByVal initialData As MyContextClass) As String
    '    '    Return "Replace Me"
    '    'End Operator
    'End Class


    ''' <summary>
    ''' Данные для редактирования в PropertyGrid
    ''' </summary>
    <Serializable()>
    <TypeConverter(GetType(PropertySorter))>
    Public MustInherit Class TreeNodeBase
        Inherits FilterablePropertyBase
        ' Указываем базовый класс для класса настраиваемого объекта:
        ' Задаем атрибут TypeConverter с параметром PropertySorter для всего класса с настраиваемыми свойствами:

        Protected Sub New(ByVal name As String)
            If name Is Nothing Then
                Throw New ArgumentNullException("Имя ???")
            End If
            mName = name
        End Sub

        Private mName As String
        '<Browsable(False)> _
        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Property Name() As String
            Get
                TypeNode = Тип
                Return mName
            End Get
            Set(ByVal value As String)
                mName = value
                'RefreshContextVisitor.Обновить(Me, frmDigitalOutputPort.dtvwDirectory)
            End Set
        End Property
        <Browsable(False)>
        Public Property Тип() As TypeGridDigitalNode = TypeGridDigitalNode.Unknown

        'Public ReadOnly Property Name() As String
        '    Get
        '        Return _name
        '    End Get
        'End Property
        Public Overloads Overrides Function ToString() As String
            Return Name
        End Function

        Public MustOverride Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C)) 'было
        Public MustOverride Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))

        'ByVal visitor As IContextVisitor(Of C)) - это посетитель
        'ByVal context As C - это в методы посетителя требуется передать параметр, этот контекст можно сделать статически типизированным
    End Class

    ' Реализация посетителя при помощи функторов
    ' Однако даже при использовании перегрузки написание кучи методов Accept весьма утомительно. 
    ' Полностью избавиться от этих методов можно, если язык программирования поддерживает функторы (функциональные объекты).
    ' ПРИМЕЧАНИЕ

    ' Функциональный объект, часто называемый функтором, - это конструкция, позволяющая объекту быть вызванным, 
    ' как если бы он был простой функцией, обычно с тем же синтаксисом.

    ' В .NET имеется специализированный тип – делегат, реализующий функциональность функторов.

    ' Идея использования функциональных объектов заключается в том, что мы во время выполнения строим карту (map) 
    ' типов на эти объекты и затем, вместо вызова Accept, используем эту карту. С использованием механизма 
    ' рефлексии такую карту можно построить автоматически, на основании интерфейса посетителя.

    ' Для ускорения работы такого посетителя крайне рекомендуется кэшировать карты в статическом контексте. 
    ' Еще один вариант повышения производительности – замена карты с делегатами на динамическую кодогенерацию.
#Region "PrintFuncVisitor"

    'Public Class PrintFuncVisitor
    '    Implements IContextVisitor(Of MyContextClass) ' вместо Integer
    '    'Implements IContextVisitor(Of Integer) ' вместо Integer


    '    Private ReadOnly _writer As TextWriter

    '    'MyContextClass
    '    'Private ReadOnly _helper As FuncVisitHelper(Of IContextVisitor(Of Integer), Integer) 'было
    '    Private ReadOnly _helper As FuncVisitHelper(Of IContextVisitor(Of MyContextClass), MyContextClass) 'это функтором

    '    Private Sub New(ByVal writer As TextWriter)
    '        _writer = writer 'куда выводим
    '        'просмотр Interface IContextVisitor(Of C) и составление карты всех методов данного интерфейса
    '        '_helper = New FuncVisitHelper(Of IContextVisitor(Of Integer), Integer)(Me)
    '        _helper = New FuncVisitHelper(Of IContextVisitor(Of MyContextClass), MyContextClass)(Me)
    '        '_helper = New FuncVisitHelper(Of IContextVisitor(Of MyContextClass), Integer)(Me)

    '    End Sub

    '    Public Shared Sub Print(ByVal tree As TreeNodeBase, ByVal writer As TextWriter)
    '        'вызывается общий метод (конструктор) New для составление карты
    '        'и далее метод AcceptVisitor общего класса _helper 
    '        'в параметре которого передается корневой узел
    '        'New PrintFuncVisitor(writer)._helper.AcceptVisitor(Of ВсеКонфигурации)(0, DirectCast(tree, ВсеКонфигурации))'было
    '        New PrintFuncVisitor(writer)._helper.AcceptVisitor(Of ВсеКонфигурации)(New MyContextClass(0), DirectCast(tree, ВсеКонфигурации))
    '        'New PrintFuncVisitor(writer)._helper.AcceptVisitor(Of ВсеКонфигурации)(New MyContextClass(0).ПозицияProperty, DirectCast(tree, ВсеКонфигурации))
    '    End Sub

    '    'перегруженные методы обработки делегатов для разных типов узлов
    '    Private Sub Visit(ByVal context As MyContextClass, ByVal rootNode As ВсеКонфигурации) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal rootNode As ВсеКонфигурации) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + rootNode.Name + " : Type_ВсеКонфигурации")
    '        For Each node As Конфигурация In rootNode.DictionaryВсеКонфигурации.Values
    '            'context.ПозицияProperty += 1
    '            _helper.AcceptVisitor(context, node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, node)
    '        Next
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As Конфигурация) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal rootNode As Конфигурация) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Конфигурация")
    '        For Each t1node As Действие In node.DictionaryДействие.Values
    '            '_helper.AcceptVisitor(context + 1, t1node)
    '            _helper.AcceptVisitor(context, t1node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t1node)

    '        Next
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As Действие) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As Действие) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Действие")
    '        For Each t21node As ТриггерСрабатыванияЦифровогоВыхода In node.DictionaryТриггерСрабатыванияЦифровогоВыхода.Values
    '            '_helper.AcceptVisitor(context + 1, t21node)
    '            _helper.AcceptVisitor(context, t21node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t21node)
    '        Next
    '        For Each t22node As ФормулаСрабатыванияЦифровогоВыхода In node.DictionaryФормулаСрабатыванияЦифровогоВыхода.Values
    '            '_helper.AcceptVisitor(context + 1, t22node)
    '            _helper.AcceptVisitor(context, t22node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t22node)
    '        Next
    '        For Each t23node As Порт In node.DictionaryПорт.Values
    '            '_helper.AcceptVisitor(context + 1, t23node)
    '            _helper.AcceptVisitor(context, t23node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t23node)
    '        Next
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As ТриггерСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As ТриггерСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_ТриггерСрабатыванияЦифровогоВыхода")
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As ФормулаСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As ФормулаСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_ФормулаСрабатыванияЦифровогоВыхода")
    '        For Each t31Node As АргументДляФормулы In node.DictionaryАргументДляФормулы.Values
    '            '_helper.AcceptVisitor(context + 1, t31Node)
    '            _helper.AcceptVisitor(context, t31Node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t31Node)
    '        Next
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As Порт) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As Порт) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Порт")
    '        For Each t32Node As Бит In node.DictionaryБит.Values
    '            '_helper.AcceptVisitor(context + 1, t32Node)
    '            _helper.AcceptVisitor(context, t32Node)
    '            '_helper.AcceptVisitor(context.ПозицияProperty, t32Node)
    '        Next
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As АргументДляФормулы) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As АргументДляФормулы) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_АргументДляФормулы")
    '    End Sub

    '    Private Sub Visit(ByVal context As MyContextClass, ByVal node As Бит) Implements IContextVisitor(Of MyContextClass).Visit
    '        'Private Sub Visit(ByVal context As Integer, ByVal node As Бит) Implements IContextVisitor(Of Integer).Visit
    '        _writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Бит")
    '    End Sub
    'End Class

    ''FuncVisitHelper(Of IContextVisitor(Of MyContextClass), Integer)
    'Public Class FuncVisitHelper(Of V, C)
    '    Friend Delegate Sub VisitDelegate(Of I)(ByVal context As C, ByVal item As I) 'было

    '    Private ReadOnly _map As IDictionary(Of Type, [Delegate])

    '    Public Sub New(ByVal visitor As V)
    '        _map = BuildMap(visitor)
    '    End Sub

    '    Private Function BuildMap(ByVal visitor As V) As IDictionary(Of Type, [Delegate])
    '        Dim map As New Dictionary(Of Type, [Delegate])()
    '        For Each mi As MethodInfo In GetType(V).GetMethods()
    '            'через интерфейс это метод - Sub Visit(ByVal context As C, ByVal node As RootNode)
    '            'для класса PrintFuncVisitor 
    '            Dim pis As ParameterInfo() = mi.GetParameters()
    '            If pis.Length <> 2 Then
    '                Throw New ArgumentException(String.Format("Метод '{0}' должен иметь 2 параметра", mi.Name))
    '            End If
    '            'pis(1) это в методе Sub Visit(ByVal context As C, ByVal node As RootNode) есть узел node
    '            'будут добавлены RootNode, Type1Node, Type2Node, Type3Node

    '            'Parameters CreateDelegate:
    '            'type: The System.Type of delegate to create.
    '            'target: The class instance on which method is invoked. (visitor)
    '            'method: The name of the instance method that the delegate is to represent. (mi)

    '            'map.Add(pis(1).ParameterType, [Delegate].CreateDelegate(GetType(VisitDelegate(Of )).MakeGenericType(GetType(V), GetType(C), pis(1).ParameterType), visitor, mi))
    '            'в коллекцию будет добалены TKey как тип производного узла, а TValue как делегат


    '            'Friend Delegate Sub VisitDelegate(Of I)(ByVal context As C, ByVal item As I) 'было

    '            Dim myType As Type = GetType(VisitDelegate(Of )).MakeGenericType(GetType(V), GetType(C), pis(1).ParameterType) 'было
    '            'Dim myType As Type = GetType(VisitDelegate(Of )) '.MakeGenericType(GetType(V), GetType(C), pis(1).ParameterType) 'было
    '            'myType.MakeGenericType(GetType(V), GetType(C), pis(1).ParameterType)

    '            'Dim myTypeV As Type = GetType(V)
    '            'Dim myTypeC As Type = GetType(C)
    '            'myType.MakeGenericType(myTypeV, myTypeC, pis(1).ParameterType) 'node

    '            'myType.MakeGenericType(myTypeC, pis(1).ParameterType)

    '            Dim myDelegate As [Delegate] = [Delegate].CreateDelegate(myType, visitor, mi)
    '            'Dim myDelegate As [Delegate] = [Delegate].CreateDelegate(myType, Nothing, mi)
    '            'Dim myDelegate As [Delegate] = [Delegate].CreateDelegate(myType, visitor, "Visit")
    '            'Dim myDelegate As [Delegate] = [Delegate].CreateDelegate(myType, mi)

    '            map.Add(pis(1).ParameterType, myDelegate)


    '            'Delegate.CreateDelegate(
    '            'typeof (VisitDelegate<>).MakeGenericType( сгенерировать тип
    '            '  typeof (V),
    '            '  typeof (C),
    '            '  pis[1].ParameterType),
    '            'visitor,                       первый аргумент
    '            'mi));                          метод As MethodInfo

    '        Next
    '        Return map
    '    End Function

    '    Public Sub AcceptVisitor(Of I)(ByVal context As C, ByVal item As I)
    '        Dim del As [Delegate]
    '        If Not _map.TryGetValue(GetType(I), del) Then
    '            Throw New ApplicationException(String.Format("Тип '{0}' не содержится в интерфейсе посетителя '{1}'", GetType(I), GetType(V)))
    '        End If
    '        'вызов делегата (ссылки на метод Visit) который содержит перегруженный версии
    '        DirectCast(del, VisitDelegate(Of I))(context, item) 'было

    '    End Sub
    'End Class
#End Region

#Region "PrintContextVisitor"

    Public Class PrintContextVisitor
        Implements IContextVisitor(Of MyContextClass) ' вместо Integer
        'Private ReadOnly _writer As TextWriter

        'Private Sub New(ByVal writer As TextWriter)
        '    _writer = writer ' куда выводим
        'End Sub

        ' было
        'Public Shared Sub Print(ByVal tree As TreeNodeBase, ByVal writer As TextWriter)
        '    tree.AcceptVisitor(New MyContextClass(0), New PrintContextVisitor(writer))
        'End Sub

        Public Shared Sub Print(ByVal tree As TreeNodeBase, ByVal clDirectoryTreeView As DirectoryTreeView)
            tree.AcceptVisitor(New MyContextClass(clDirectoryTreeView), New PrintContextVisitor())
        End Sub

        ' перегруженные методы обработки делегатов для разных типов узлов
        Private Sub Visit(ByVal context As MyContextClass, ByVal rootNode As ВсеКонфигурации) Implements IContextVisitor(Of MyContextClass).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + rootNode.Name + " : Type_ВсеКонфигурации")
            Dim tnDir As New TreeNode(rootNode.Name, 0, 0) With {.Tag = rootNode}

            context.DirectoryTreeView.Nodes.Add(tnDir) ' добавить в корень
            context.CurrentDir = tnDir

            'For Each nodeКонфигурация As Конфигурация In rootNode.DictionaryВсеКонфигурации.Values
            If rootNode.ВсеКонфигурации IsNot Nothing Then
                For Each nodeКонфигурация As Конфигурация In rootNode.ВсеКонфигурации
                    'context.ПозицияProperty = 1
                    nodeКонфигурация.AcceptVisitor(context, Me)
                Next
            End If
            context.CurrentDir = tnDir
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As Конфигурация) Implements IContextVisitor(Of MyContextClass).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Конфигурация")
            Dim tnDir As New TreeNode(node.Name, 1, 8) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            context.CurrentDir = tnDir ' сменить

            'context.ПозицияProperty += 1
            'For Each nodeДействие As Действие In node.DictionaryДействие.Values
            If node.Действия IsNot Nothing Then
                For Each nodeДействие As Действие In node.Действия
                    'context.ПозицияProperty = 2
                    nodeДействие.AcceptVisitor(context, Me)
                Next
            End If
            context.CurrentDir = tnDir.Parent
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As Действие) Implements IContextVisitor(Of MyContextClass).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Действие")
            'context.ПозицияProperty += 1
            Dim tnDir As New TreeNode(node.Name, 2, 9) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            context.CurrentDir = tnDir ' сменить

            'If node.DictionaryТриггерСрабатыванияЦифровогоВыхода IsNot Nothing Then
            '    For Each nodeTrigger As ТриггерСрабатыванияЦифровогоВыхода In node.DictionaryТриггерСрабатыванияЦифровогоВыхода.Values
            If node.ТриггерыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                If node.ТриггерыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                    For Each nodeTrigger As ТриггерСрабатыванияЦифровогоВыхода In node.ТриггерыСрабатыванияЦифровогоВыхода
                        'context.ПозицияProperty = 3
                        nodeTrigger.AcceptVisitor(context, Me)
                    Next
                End If
            End If

            'If node.DictionaryФормулаСрабатыванияЦифровогоВыхода IsNot Nothing Then
            '    For Each nodeФормула As ФормулаСрабатыванияЦифровогоВыхода In node.DictionaryФормулаСрабатыванияЦифровогоВыхода.Values
            If node.ФормулыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                If node.ФормулыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                    For Each nodeФормула As ФормулаСрабатыванияЦифровогоВыхода In node.ФормулыСрабатыванияЦифровогоВыхода
                        'context.ПозицияProperty = 3
                        nodeФормула.AcceptVisitor(context, Me)
                    Next
                End If
            End If

            'For Each nodeПорт As Порт In node.DictionaryПорт.Values
            If node.Порты IsNot Nothing Then
                For Each nodeПорт As Порт In node.Порты
                    'context.ПозицияProperty = 3
                    nodeПорт.AcceptVisitor(context, Me)
                Next
            End If

            context.CurrentDir = tnDir.Parent
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As ТриггерСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of MyContextClass).Visit
            'Private Sub Visit(ByVal context As Integer, ByVal node As ТриггерСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of Integer).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_ТриггерСрабатыванияЦифровогоВыхода")
            Dim tnDir As New TreeNode(node.Name, 3, 10) With {.Tag = node}
            context.CurrentDir.Nodes.Add(tnDir)
            'context.CurrentDir = tnDir ' сменить
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As ФормулаСрабатыванияЦифровогоВыхода) Implements IContextVisitor(Of MyContextClass).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_ФормулаСрабатыванияЦифровогоВыхода")
            'context.ПозицияProperty += 1
            Dim tnDir As New TreeNode(node.Name, 4, 11) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            context.CurrentDir = tnDir ' сменить

            'For Each NodeАргумент As АргументДляФормулы In node.DictionaryАргументДляФормулы.Values
            If node.АргументыДляФормулы IsNot Nothing Then
                For Each NodeАргумент As АргументДляФормулы In node.АргументыДляФормулы
                    'context.ПозицияProperty = 4
                    NodeАргумент.AcceptVisitor(context, Me)
                Next
            End If

            context.CurrentDir = tnDir.Parent
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As Порт) Implements IContextVisitor(Of MyContextClass).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Порт")
            'context.ПозицияProperty += 1
            Dim tnDir As New TreeNode(node.Name, 6, 13) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            context.CurrentDir = tnDir ' сменить

            'For Each NodeБит As Бит In node.DictionaryБит.Values
            If node.Биты IsNot Nothing Then
                For Each NodeБит As Бит In node.Биты
                    'context.ПозицияProperty = 4
                    NodeБит.AcceptVisitor(context, Me)
                Next
            End If
            context.CurrentDir = tnDir.Parent
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As АргументДляФормулы) Implements IContextVisitor(Of MyContextClass).Visit
            'Private Sub Visit(ByVal context As Integer, ByVal node As АргументДляФормулы) Implements IContextVisitor(Of Integer).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_АргументДляФормулы")
            Dim tnDir As New TreeNode(node.Name, 5, 12) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            'context.CurrentDir = tnDir ' сменить
        End Sub

        Private Sub Visit(ByVal context As MyContextClass, ByVal node As Бит) Implements IContextVisitor(Of MyContextClass).Visit
            'Private Sub Visit(ByVal context As Integer, ByVal node As Бит) Implements IContextVisitor(Of Integer).Visit
            '_writer.WriteLine(New String(" "c, context.ПозицияProperty) + node.Name + " : Type_Бит")
            Dim tnDir As New TreeNode(node.Name, 7, 14) With {.Tag = node}

            context.CurrentDir.Nodes.Add(tnDir)
            'context.CurrentDir = tnDir ' сменить
        End Sub
    End Class

    Public Class RefreshContextVisitor
        Implements IRefreshContextVisitor(Of MyContextClass)

        Public Shared Sub UpdateTree(ByVal tree As TreeNodeBase) ', ByVal clDirectoryTreeView As DirectoryTreeView)
            If DigitalPortForm.DtvwDirectory Is Nothing Then Exit Sub ' если ещё не создан при начальной загрузки окна
            If isLoadingFromDataSet Then Exit Sub

            IsNotCreatePropertyEditorSource = True
            PropertiesChanging = True
            ' MyContextClass уже создан
            'tree.AcceptVisitorRefresh(New MyContextClass(clDirectoryTreeView), New RefreshContextVisitor())
            tree.AcceptVisitorRefresh(MyContextClass.SingletonContextClass, New RefreshContextVisitor()) ' там вызов visitor.ОбновитьУзел(context, Me)
            IsNotCreatePropertyEditorSource = False
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal rootNode As ВсеКонфигурации) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As Конфигурация) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As Действие) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As ТриггерСрабатыванияЦифровогоВыхода) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As ФормулаСрабатыванияЦифровогоВыхода) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As Порт) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As АргументДляФормулы) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
            'If context.DirectoryTreeView.SelectedNode IsNot Nothing Then
            '    context.DirectoryTreeView.SelectedNode.Text = node.Name
            'End If
        End Sub

        Private Sub UpdateNode(ByVal context As MyContextClass, ByVal node As Бит) Implements IRefreshContextVisitor(Of MyContextClass).UpdateNode
            context.DirectoryTreeView.SelectCureentNode()
        End Sub
    End Class
#End Region

    '    Как заменить имя переменной в левой колонке “человеческим” именем свойства?
    ' Для этого предназначен атрибут DisplayName:
    '    Как отобразить расширенную подсказку по свойству в нижнем окне?
    ' Для этого предназначен атрибут Description:
    '    Как сгруппировать свойства по категориям?
    ' Используйте атрибут Category:
    '    Как отобразить свойство, недоступное для редактирования?
    ' Можно либо само свойство сделать read-only (оставив только get), либо использовать атрибут ReadOnly:

    '''' <summary>
    '''' Идентификатор
    '''' </summary>
    '<DisplayName("ID")> _
    '<Description("Идентификатор")> _
    '<Category("1. Идентификация")> _
    '<[ReadOnly](True)> _
    'Public Property Id() As Integer
    '    Get
    '        Return id_
    '    End Get
    '    Set(ByVal value As Integer)
    '        id_ = value
    '    End Set
    'End Property

    ' в корне содержится вся структура какого-то иерархического представления
    ' каждый узел реализует общий интерфейс например для навигации
#Region "ВсеКонфигурации"

    <DisplayName("ВсеКонфигурации")>
    <Description("Состав всех созданных конфигураций")>
    Public Class ВсеКонфигурации
        Inherits TreeNodeBase

        ' Указываем базовый класс для класса настраиваемого объекта:
        ' Задаем атрибут TypeConverter с параметром PropertySorter для всего класса с настраиваемыми свойствами:

        'Private mListВсеКонфигурации As List(Of Конфигурация)

        Public Sub New(ByVal Configurations As Конфигурация())
            MyBase.New("ВсеКонфигурации")
            'AsReadOnly - из массива в лист IList

            ВсеКонфигурации = New List(Of Конфигурация)
            ВсеКонфигурации.AddRange(Array.AsReadOnly(Configurations))
            Тип = TypeGridDigitalNode.AllConfigurations
        End Sub

        Public Sub New(ByVal Configurations As List(Of Конфигурация))
            MyBase.New("ВсеКонфигурации")
            ВсеКонфигурации = Configurations
            Тип = TypeGridDigitalNode.AllConfigurations
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        '    Как избавиться от стандартного “(Collection)” в правой колонке для свойств-коллекций?
        ' Используйте атрибут TypeConverter:
        ' При переходе к редактированию коллекции отобразится стандартное окно Collection Editor с данными редактируемой коллекции:
        ''' <summary>
        ''' Все Конфигураци
        ''' </summary>
        <DisplayName("Все Конфигураци")>
        <Description("Состав всех созданных конфигураций")>
        <PropertyOrder(50)>
        <Category("2. Состав")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property ВсеКонфигурации() As List(Of Конфигурация)

        <Browsable(False)>
        Public ReadOnly Property ItemКонфигурация() As Конфигурация
            Get
                Dim Index As Integer = -1
                Index = ВсеКонфигурации.FindIndex(AddressOf FindItemConfiguration)

                If Index <> -1 Then
                    Return ВсеКонфигурации(Index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemConfiguration(ByVal s As Конфигурация) As Boolean
            If s.KeyКонфигурацияДействия = MemoKeyConfigurationAction Then
                Return True
            Else
                Return False
            End If
        End Function

        ' поиск predicate возвращающего true если строка оканчивается на "saurus".
        'Private Shared Function EndsWithSaurus(ByVal s As Конфигурация) As Boolean
        '    'If (s.Length > 5) AndAlso (s.Substring(s.Length - 6).ToLower() = "saurus") Then
        '    '    Return True
        '    'Else
        '    '    Return False
        '    'End If
        '    Return True
        'End Function
    End Class
#End Region

#Region "Конфигурация"
    <DisplayName("Конфигурация")>
    <Description("Название созданной конфигурации")>
    Public Class Конфигурация
        Inherits TreeNodeBase

        Public Sub New()
            Me.New("Введите имя Конфигурации", GetNewAction)
            Тип = TypeGridDigitalNode.Configuration
        End Sub

        Public Sub New(ByVal name As String, ByVal Actions As Действие())
            MyBase.New(name)

            keyConfigurationAction = DigitalPortForm.GetKeyId()
            ' присвоить mkeyКонфигурацияДействия из  Me.New("Введите имя Конфигурации", НовоеДействие)
            If Actions.Count > 0 Then
                Actions(0).Parent = Me
                Actions(0).keyКонфигурацияДействия = keyConfigurationAction
            End If

            Действия = New List(Of Действие)
            Действия.AddRange(Array.AsReadOnly(Actions))
            Тип = TypeGridDigitalNode.Configuration
        End Sub

        'Public Sub New(ByVal name As String, ByVal ListДействие As List(Of Действие))
        '    MyBase.New(name)
        '    ListДействие = ListДействие
        '    Тип = TypeGridDigitalNode.Configuration
        'End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                ' ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyConfigurationAction = keyConfigurationAction
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private description As String = "Заполните назначение конфигурации"
        ''' <summary>
        ''' Описание
        ''' </summary>
        <DisplayName("Описание")>
        <Description("Описание конфигурации")>
        <Category("2. Описание")>
        <PropertyOrder(40)>
        Public Property Описание() As String
            Get
                Return description
            End Get
            Set(ByVal value As String)
                description = value
                ' ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        'Private mListДействие As List(Of Действие)

        <DisplayName("Все Действия")>
        <Description("Состав всех созданных действий")>
        <PropertyOrder(50)>
        <Category("3. Состав")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property Действия() As List(Of Действие)

        <Browsable(False)>
        Public ReadOnly Property ItemДействие() As Действие
            Get
                Dim Index As Integer = -1
                Index = Действия.FindIndex(AddressOf FindItemДействие)

                If Index <> -1 Then
                    Return Действия(Index)
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Private Shared Function FindItemДействие(ByVal s As Действие) As Boolean
            If s.keyДействие = MemoKeyAction Then
                Return True
            Else
                Return False
            End If
        End Function

        Private keyConfigurationAction As Integer
        <Browsable(False)>
        Public Property KeyКонфигурацияДействия() As Integer
            Get
                Return keyConfigurationAction
            End Get
            Set(ByVal value As Integer)
                MemoKeyConfigurationAction = value
                keyConfigurationAction = value
            End Set
        End Property

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьКонфигурацияTableAdapter()
        '            Dim rowFindКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow _
        '            = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.FindBykeyКонфигурацияДействия(mkeyКонфигурацияДействия)
        '            If rowFindКонфигурация IsNot Nothing Then
        '                rowFindКонфигурация.ИмяКонфигурации = Name
        '                rowFindКонфигурация.Описание = Описание

        '                .ОбновитьКонфигурацияTableAdapter()
        '                .ЗаполнитьКонфигурацияTableAdapter()
        '            End If
        '        End With
        '    End If
        'End Sub

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        ''' <summary>
        ''' Представление в виде строки, используется для показа списка вариантов 
        ''' при редактировании в PropertyGrid
        ''' </summary>
        Public Overloads Overrides Function ToString() As String
            Return Name
        End Function

        Shared Function GetNewAction() As Действие()
            '"Введите имя Действия", Nothing, Nothing, arrПорт)
            Return New Действие() {New Действие()}
        End Function
    End Class
#End Region

#Region "Действие"
    <DisplayName("Действие")>
    <Description("Название созданного Действия")>
    Public Class Действие
        Inherits TreeNodeBase

        'Private mListТриггерСрабатыванияЦифровогоВыхода As List(Of ТриггерСрабатыванияЦифровогоВыхода)
        'Private mListФормулаСрабатыванияЦифровогоВыхода As List(Of ФормулаСрабатыванияЦифровогоВыхода)
        'Private mListПорт As List(Of Порт)

        Public Sub New()
            Me.New("Введите имя Действия", Nothing, Nothing, GetNewPort)
            Тип = TypeGridDigitalNode.Action
        End Sub

        Public Sub New(ByVal name As String, ByVal Triggers As ТриггерСрабатыванияЦифровогоВыхода(), ByVal Formulas As ФормулаСрабатыванияЦифровогоВыхода(), ByVal Ports As Порт())
            MyBase.New(name)

            keyAction = DigitalPortForm.GetKeyId()
            ТриггерыСрабатыванияЦифровогоВыхода = New List(Of ТриггерСрабатыванияЦифровогоВыхода)
            ФормулыСрабатыванияЦифровогоВыхода = New List(Of ФормулаСрабатыванияЦифровогоВыхода)
            Порты = New List(Of Порт)

            If Triggers IsNot Nothing Then
                If Triggers.Count > 0 Then
                    Triggers(0).Parent = Me
                    Triggers(0).keyДействие = keyAction
                End If

                ТриггерыСрабатыванияЦифровогоВыхода.AddRange(Array.AsReadOnly(Triggers))
            End If

            If Formulas IsNot Nothing Then
                If Formulas.Count > 0 Then
                    Formulas(0).Parent = Me
                    Formulas(0).keyДействие = keyAction
                End If

                ФормулыСрабатыванияЦифровогоВыхода.AddRange(Array.AsReadOnly(Formulas))
            End If

            If Ports IsNot Nothing Then
                If Ports.Count > 0 Then
                    Ports(0).Parent = Me
                    Ports(0).keyДействие = keyAction
                End If

                Порты.AddRange(Array.AsReadOnly(Ports))
            End If

            Тип = TypeGridDigitalNode.Action
        End Sub

        'Public Sub New(ByVal name As String, ByVal ListТриггерСрабатыванияЦифровогоВыхода As List(Of ТриггерСрабатыванияЦифровогоВыхода), ByVal ListФормулаСрабатыванияЦифровогоВыхода As List(Of ФормулаСрабатыванияЦифровогоВыхода), ByVal ListПорт As List(Of Порт))
        '    MyBase.New(name)

        '    If ListТриггерСрабатыванияЦифровогоВыхода IsNot Nothing Then
        '        ListТриггерСрабатыванияЦифровогоВыхода = ListТриггерСрабатыванияЦифровогоВыхода
        '    End If

        '    If ListФормулаСрабатыванияЦифровогоВыхода IsNot Nothing Then
        '        ListФормулаСрабатыванияЦифровогоВыхода = ListФормулаСрабатыванияЦифровогоВыхода
        '    End If

        '    If ListПорт IsNot Nothing Then
        '        ListПорт = ListПорт
        '    End If

        '    Тип = TypeGridDigitalNode.Action
        'End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                ' ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyAction = keyAction
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        <DisplayName("Все Триггеры")>
        <Description("Состав всех созданных Триггеров")>
        <PropertyOrder(50)>
        <Category("2. Состав Триггеров")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property ТриггерыСрабатыванияЦифровогоВыхода() As List(Of ТриггерСрабатыванияЦифровогоВыхода)

        <Browsable(False)>
        Public ReadOnly Property ItemТриггер() As ТриггерСрабатыванияЦифровогоВыхода
            Get
                Dim index As Integer = -1
                index = ТриггерыСрабатыванияЦифровогоВыхода.FindIndex(AddressOf FindItemTrigger)

                If index <> -1 Then
                    Return ТриггерыСрабатыванияЦифровогоВыхода(index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemTrigger(ByVal s As ТриггерСрабатыванияЦифровогоВыхода) As Boolean
            If s.KeyТриггер = MemoKeyTrigger Then
                Return True
            Else
                Return False
            End If
        End Function

        <DisplayName("Все Формулы")>
        <Description("Состав всех созданных Формул")>
        <PropertyOrder(50)>
        <Category("3. Состав Формул")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property ФормулыСрабатыванияЦифровогоВыхода() As List(Of ФормулаСрабатыванияЦифровогоВыхода)

        <Browsable(False)>
        Public ReadOnly Property ItemФормула() As ФормулаСрабатыванияЦифровогоВыхода
            Get
                Dim index As Integer = -1
                index = ФормулыСрабатыванияЦифровогоВыхода.FindIndex(AddressOf FindItemФормула)

                If index <> -1 Then
                    Return ФормулыСрабатыванияЦифровогоВыхода(index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemФормула(ByVal s As ФормулаСрабатыванияЦифровогоВыхода) As Boolean
            If s.KeyFormula = MemoKeyFormula Then
                Return True
            Else
                Return False
            End If
        End Function

        <DisplayName("Все Порты")>
        <Description("Состав всех созданных Портов")>
        <PropertyOrder(50)>
        <Category("4. Состав Портов")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property Порты() As List(Of Порт)

        <Browsable(False)>
        Public ReadOnly Property ItemПорт() As Порт
            Get
                Dim Index As Integer = -1
                Index = Порты.FindIndex(AddressOf FindItemПорт)

                If Index <> -1 Then
                    Return Порты(Index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemПорт(ByVal s As Порт) As Boolean
            If s.KeyПорта = MemoKeyPort Then
                Return True
            Else
                Return False
            End If
        End Function

        Private keyAction As Integer
        <Browsable(False)>
        Public Property keyДействие() As Integer
            Get
                Return keyAction
            End Get
            Set(ByVal value As Integer)
                MemoKeyAction = value
                keyAction = value
            End Set
        End Property
        <Browsable(False)>
        Public Property keyКонфигурацияДействия() As Integer
        <Browsable(False)>
        Public Property Parent() As Конфигурация

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьДействиеTableAdapter(mkeyКонфигурацияДействия)
        '            Dim rowFindДействие As ChannelsDigitalOutputDataSet.ДействиеRow _
        '            = .ChannelsDigitalOutputDataSet.Действие.FindBykeyДействие(mkeyДействие)
        '            If rowFindДействие IsNot Nothing Then
        '                rowFindДействие.keyКонфигурацияДействия = gkeyКонфигурацияДействия
        '                rowFindДействие.ИмяДействия = Name

        '                .ОбновитьДействиеTableAdapter()
        '                .ЗаполнитьДействиеTableAdapter(mkeyКонфигурацияДействия)
        '            End If
        '        End With
        '    End If
        'End Sub

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        Shared Function GetNewTrigger() As ТриггерСрабатыванияЦифровогоВыхода()
            Return New ТриггерСрабатыванияЦифровогоВыхода() {} ' вернуть пустой массив
        End Function

        Shared Function GetNewFormula() As ФормулаСрабатыванияЦифровогоВыхода()
            Return New ФормулаСрабатыванияЦифровогоВыхода() {} ' вернуть пустой массив
        End Function

        Shared Function GetNewPort() As Порт()
            Return New Порт() {New Порт()} ' вернуть пустой массив
        End Function

    End Class
#End Region

#Region "ТриггерСрабатыванияЦифровогоВыхода"
    <DisplayName("Триггер")>
    <Description("Название созданного Триггера")>
    Public Class ТриггерСрабатыванияЦифровогоВыхода
        Inherits TreeNodeBase

        Public Sub New()
            Me.New("Введите имя Триггера")
            Тип = TypeGridDigitalNode.Trigger
            'mkeyДействие = Parent.keyДействие
            keyTrigger = DigitalPortForm.GetKeyId()
        End Sub

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = TypeGridDigitalNode.Trigger
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyTrigger = keyTrigger
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private keyTrigger As Integer
        <Browsable(False)>
        Public Property KeyТриггер() As Integer
            Get
                Return keyTrigger
            End Get
            Set(ByVal value As Integer)
                MemoKeyTrigger = value
                keyTrigger = value
            End Set
        End Property
        <Browsable(False)>
        Public Property keyДействие() As Integer
        <Browsable(False)>
        Public Property Parent() As Действие
        <Browsable(False)>
        Public Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        ' Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        ' передачу ей редактируемого значения и получение результата:
        ' смотри public class IPAddressEditor : UITypeEditor 
        ' а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private nameChannel As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала используемого при определении условия срабатывания триггера")>
        <Category("2. Условие")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Property ИмяКанала() As String
            Get
                Return nameChannel
            End Get
            Set(ByVal value As String)
                nameChannel = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        '="=" Or ="<>" Or ="<" Or =">" Or ="между"  Or ="вне"
        Private compareOperation As String = "="

        ''' <summary>
        ''' Операция Сравнения
        ''' </summary>
        <DisplayName("Операция Сравнения")>
        <Description("Операция сравнения значения канала при определении условия срабатывания триггера")>
        <Category("2. Условие")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(OperationTypeConverter))>
        Public Property ОперацияСравнения() As String
            Get
                Return compareOperation
            End Get
            Set(ByVal value As String)
                compareOperation = value
                'ОбновитьЗаписьВТаблице()
                DigitalPortForm.CreatePropertyEditorSource(GetType(ТриггерСрабатыванияЦифровогоВыхода).Name, Me)
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Private valueCondition As Double

        ''' <summary>
        ''' Величина Условия 1
        ''' </summary>
        <DisplayName("Значение")>
        <Description("Значение величины в условии сравнения со значением канала для срабатывания триггера")>
        <Category("2. Условие")>
        <PropertyOrder(120)>
        Public Property ВеличинаУсловия() As Double
            Get
                Return valueCondition
            End Get
            Set(ByVal value As Double)
                'If value <> "" Then
                '    If Not IsNumeric(value) Then
                '        Dim TextMessage As String = "Должна быть цифра!"
                '        MessageBox.Show(TextMessage, "Проверка величины загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '        'СообщениеНаПанель(TextMessage)
                '        Exit Property
                '    End If
                'End If
                valueCondition = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        '    Как организовать выбор файла с заданным расширением?
        ' Задайте атрибут Editor:
        ' Собственно фильтр расширений задается в DocFileEditor:

        ' А для свойства, видимость которого зависит от другого свойства – атрибут DynamicPropertyFilter:
        ' здесь "Post" атрибут от которого зависит видимость со списком, где видимость включается
        ' Также нужно добавить обработчик события PropertyGrid – PropertyValueChanged:

        Private valueCondition2 As Double

        ''' <summary>
        ''' Величина Условия 2
        ''' </summary>
        <DisplayName("Значение 2")>
        <Description("Значение величины в условии сравнения со значением канала для срабатывания триггера")>
        <Category("2. Условие")>
        <PropertyOrder(130)>
        <DynamicPropertyFilter("ОперацияСравнения", "между,вне")>
        Public Property ВеличинаУсловия2() As Double
            Get
                Return valueCondition2
            End Get
            Set(ByVal value As Double)
                valueCondition2 = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьТриггерСрабатыванияЦифровогоВыходаTableAdapter(mkeyДействие)
        '            Dim rowFindТриггер As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow _
        '            = .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.FindByKeyТриггер(mKeyТриггер)
        '            If rowFindТриггер IsNot Nothing Then
        '                rowFindТриггер.keyДествие = gkeyДействие
        '                rowFindТриггер.name = Name
        '                rowFindТриггер.ИмяКанала = mИмяКанала
        '                rowFindТриггер.ОперацияСравнения = ОперацияСравнения
        '                rowFindТриггер.ВеличинаУсловия = ВеличинаУсловия
        '                rowFindТриггер.ВеличинаУсловия2 = ВеличинаУсловия2

        '                .ОбновитьТриггерСрабатыванияЦифровогоВыходаTableAdapter()
        '                .ЗаполнитьТриггерСрабатыванияЦифровогоВыходаTableAdapter(mkeyДействие)
        '            End If
        '        End With
        '    End If
        'End Sub
    End Class
#End Region

#Region "ФормулаСрабатыванияЦифровогоВыхода"
    <DisplayName("Формула")>
    <Description("Описание созданной Формулы")>
    Public Class ФормулаСрабатыванияЦифровогоВыхода
        Inherits TreeNodeBase

        'Private mListАргументДляФормулы As List(Of АргументДляФормулы)

        Public Sub New()
            Me.New("Введите описание формулы", GetNewArgument)
            Тип = TypeGridDigitalNode.Formula
        End Sub

        Public Sub New(ByVal name As String, ByVal formulaArguments As АргументДляФормулы())
            MyBase.New(name)

            mKeyFormula = DigitalPortForm.GetKeyId()

            If formulaArguments.Count > 0 Then
                formulaArguments(0).Parent = Me
                formulaArguments(0).KeyFormula = mKeyFormula
            End If

            АргументыДляФормулы = New List(Of АргументДляФормулы)
            АргументыДляФормулы.AddRange(Array.AsReadOnly(formulaArguments))
            Тип = TypeGridDigitalNode.Formula
        End Sub

        'Public Sub New(ByVal name As String, ByVal ListАргументДляФормулы As List(Of АргументДляФормулы))
        '    MyBase.New(name)

        '    ListАргументДляФормулы = ListАргументДляФормулы
        '    Тип = TypeGridDigitalNode.Formula
        'End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyFormula = mKeyFormula
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private mKeyFormula As Integer
        <Browsable(False)>
        Public Property KeyFormula() As Integer
            Get
                Return mKeyFormula
            End Get
            Set(ByVal value As Integer)
                MemoKeyFormula = value
                mKeyFormula = value
            End Set
        End Property
        <Browsable(False)>
        Public Property keyДействие() As Integer
        <Browsable(False)>
        Public Property Parent() As Действие

        '<[ReadOnly](True)> _

        '    Как организовать редактирование свойства в собственной форме?
        ' Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        ' передачу ей редактируемого значения и получение результата:
        ' смотри public class IPAddressEditor : UITypeEditor 
        ' а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private formula As String = "ARG1+ARG2"
        ''' <summary>
        ''' Формула
        ''' </summary>
        <DisplayName("Формула")>
        <Description("Формула условия срабатывания цифровых линий порта")>
        <Category("2. Формула")>
        <PropertyOrder(100)>
        <Editor(GetType(FormulaEditor), GetType(UITypeEditor))>
        Public Property Формула() As String
            Get
                MemoKeyFormula = mKeyFormula
                Return formula
            End Get
            Set(ByVal value As String)
                formula = value

                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                If isLoadingFromDataSet = False Then
                    DigitalPortForm.DtvwDirectory.RefreshTree()
                    MemoKeyFormula = mKeyFormula
                    DigitalPortForm.FindLastNode(Тип)
                End If

                'ОбновитьЗаписьВТаблице()
                'RefreshContextVisitor.Обновить(Me, frmDigitalOutputPort.dtvwDirectory)
            End Set
        End Property

        ' ="<" Or =">"
        Private compareOperation As String = ">"

        ''' <summary>
        ''' Операция Сравнения
        ''' </summary>
        <DisplayName("Операция Сравнения")>
        <Description("Операция сравнения значения формулы при определении условия срабатывания линий порта")>
        <Category("2. Формула")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(OperationTypeConverterFormula))>
        Public Property ОперацияСравнения() As String
            Get
                Return compareOperation
            End Get
            Set(ByVal value As String)
                compareOperation = value
                'ОбновитьЗаписьВТаблице()
                DigitalPortForm.CreatePropertyEditorSource(GetType(ФормулаСрабатыванияЦифровогоВыхода).Name, Me)
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Private valueCondition As Double

        ''' <summary>
        ''' Величина Условия
        ''' </summary>
        <DisplayName("Значение")>
        <Description("Значение величины со значением формулы при определении условия срабатывания линий порта")>
        <Category("2. Формула")>
        <PropertyOrder(120)>
        Public Property ВеличинаУсловия() As Double
            Get
                Return valueCondition
            End Get
            Set(ByVal value As Double)
                'If value <> "" Then
                '    If Not IsNumeric(value) Then
                '        Dim TextMessage As String = "Должна быть цифра!"
                '        MessageBox.Show(TextMessage, "Проверка величины загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '        'СообщениеНаПанель(TextMessage)
                '        Exit Property
                '    End If
                'End If
                valueCondition = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        <DisplayName("Все Аргументы")>
        <Description("Состав всех созданных Аргументов")>
        <PropertyOrder(50)>
        <Category("3. Состав")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property АргументыДляФормулы() As List(Of АргументДляФормулы)

        <Browsable(False)>
        Public ReadOnly Property ItemАргументДляФормулы() As АргументДляФормулы
            Get
                Dim index As Integer = -1
                index = АргументыДляФормулы.FindIndex(AddressOf FindItemArgumentForFormula)

                If index <> -1 Then
                    Return АргументыДляФормулы(index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemArgumentForFormula(ByVal s As АргументДляФормулы) As Boolean
            If s.КеуArgument = MemoКеуArgument Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьФормулаСрабатыванияЦифровогоВыходаTableAdapter(mkeyДействие)
        '            Dim rowFindФормула As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow _
        '            = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.FindByKeyFormula(mKeyFormula)
        '            If rowFindФормула IsNot Nothing Then
        '                rowFindФормула.keyДействие = gkeyДействие
        '                rowFindФормула.name = Name
        '                rowFindФормула.Формула = Формула

        '                .ОбновитьФормулаСрабатыванияЦифровогоВыходаTableAdapter()
        '                .ЗаполнитьФормулаСрабатыванияЦифровогоВыходаTableAdapter(mkeyДействие)
        '            End If
        '        End With
        '    End If
        'End Sub

        Shared Function GetNewArgument() As АргументДляФормулы()
            Return New АргументДляФормулы() {New АргументДляФормулы()}
        End Function

    End Class
#End Region

#Region "АргументДляФормулы"
    <DisplayName("Аргумент")>
    <Description("Имя созданного Аргумента")>
    Public Class АргументДляФормулы
        Inherits TreeNodeBase

        Public Sub New()
            Me.New("ARG1")
            Тип = TypeGridDigitalNode.Argument
            mКеуArgument = DigitalPortForm.GetKeyId()
        End Sub

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = TypeGridDigitalNode.Argument
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoКеуArgument = mКеуArgument
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private mКеуArgument As Integer
        <Browsable(False)>
        Public Property КеуArgument() As Integer
            Get
                Return mКеуArgument
            End Get
            Set(ByVal value As Integer)
                MemoКеуArgument = value
                mКеуArgument = value
            End Set
        End Property
        <Browsable(False)>
        Public Property KeyFormula() As Integer
        <Browsable(False)>
        Public Property Parent() As ФормулаСрабатыванияЦифровогоВыхода

        Private nameChannel As String = "n1"
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала значение которого пойдёт вместо Аргумента в формуле для условия срабатывания цифровых линий порта")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Property ИмяКанала() As String
            Get
                Return nameChannel
            End Get
            Set(ByVal value As String)
                nameChannel = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property
        <Browsable(False)>
        Public Property ИндексВМассивеПараметров() As Integer

        ' Как заменить стандартные True/False в отображении свойств типа bool?
        ' Используйте атрибут TypeConverter:
        Private isConvert As Boolean = False
        ''' <summary>
        ''' Приведение
        ''' </summary>
        <DisplayName("Приведение к САУ")>
        <Description("Использовать приведение к САУ параметра (используется для оборотов)")>
        <Category("2. Канал")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(BooleanTypeConverter))>
        Public Property Приведение() As Boolean
            Get
                Return isConvert
            End Get
            Set(ByVal value As Boolean)
                isConvert = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьАргументыДляФормулыTableAdapter(mKeyFormula)
        '            Dim rowFindАргументыДляФормулы As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow _
        '            = .ChannelsDigitalOutputDataSet.АргументыДляФормулы.FindByКеуArgument(mКеуArgument)
        '            If rowFindАргументыДляФормулы IsNot Nothing Then
        '                rowFindАргументыДляФормулы.KeyFormula = gKeyFormula
        '                rowFindАргументыДляФормулы.ИмяАргумента = Name
        '                rowFindАргументыДляФормулы.ИмяКанала = ИмяКанала
        '                rowFindАргументыДляФормулы.Приведение = Приведение

        '                .ОбновитьАргументыДляФормулыTableAdapter()
        '                .ЗаполнитьАргументыДляФормулыTableAdapter(mKeyFormula)
        '            End If
        '        End With
        '    End If
        'End Sub
    End Class
#End Region

#Region "Порт"
    <DisplayName("Порт")>
    <Description("Название созданного Порта")>
    Public Class Порт
        Inherits TreeNodeBase

        Public Sub New()
            Me.New("Введите описание Порта", GetNewBit)
            Тип = TypeGridDigitalNode.Port
        End Sub

        Public Sub New(ByVal name As String, ByVal Bits As Бит())
            MyBase.New(name)

            keyPort = DigitalPortForm.GetKeyId()

            If Bits.Count > 0 Then
                Bits(0).Parent = Me
                Bits(0).KeyПорта = keyPort
            End If

            Биты = New List(Of Бит)
            Биты.AddRange(Array.AsReadOnly(Bits))
            Тип = TypeGridDigitalNode.Port
        End Sub

        Public Sub New(ByVal name As String, ByVal inListBits As List(Of Бит))
            MyBase.New(name)

            Биты = inListBits
            Тип = TypeGridDigitalNode.Port
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyPort = keyPort
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private keyPort As Integer
        <Browsable(False)>
        Public Property KeyПорта() As Integer
            Get
                Return keyPort
            End Get
            Set(ByVal value As Integer)
                MemoKeyPort = value
                keyPort = value
            End Set
        End Property
        <Browsable(False)>
        Public Property keyДействие() As Integer
        <Browsable(False)>
        Public Property Parent() As Действие

        Private numberDevice As Integer = 1
        ''' <summary>
        ''' Номер Устройства
        ''' </summary>
        <DisplayName("Номер Устройства")>
        <Description("Номер Устройства платы сбора или корзины сбора")>
        <Category("2. Железо")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberDeviceTypeConverter))>
        Public Property НомерУстройства() As Integer
            Get
                Return numberDevice
            End Get
            Set(ByVal value As Integer)
                numberDevice = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Private numberModuleChassis As String = ""
        ''' <summary>
        ''' Номер Модуля Корзины
        ''' </summary>
        <DisplayName("Номер Модуля Корзины")>
        <Description("Номер Модуля SCXI в корзине сбора")>
        <Category("2. Железо")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(NumberModuleTypeConverter))>
        Public Property НомерМодуляКорзины() As String
            Get
                Return numberModuleChassis
            End Get
            Set(ByVal value As String)
                numberModuleChassis = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Private numberPort As Integer = 0
        ''' <summary>
        ''' Номер Порта
        ''' </summary>
        <DisplayName("Номер Порта")>
        <Description("Номер Порта в плате сбора или в корзине сбора SCXI")>
        <Category("2. Железо")>
        <PropertyOrder(120)>
        <TypeConverter(GetType(NumberPortTypeConverter))>
        Public Property НомерПорта() As Integer
            Get
                Return numberPort
            End Get
            Set(ByVal value As Integer)
                numberPort = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        <DisplayName("Все Биты")>
        <Description("Все установленные Биты")>
        <PropertyOrder(50)>
        <Category("3. Состав")>
        <TypeConverter(GetType(CollectionTypeConverter))>
        <Editor(GetType(ListCollectionEditor), GetType(UITypeEditor))>
        Public Property Биты() As List(Of Бит)

        'Private listBits As List(Of Бит)

        <Browsable(False)>
        Public ReadOnly Property ItemБит() As Бит
            Get
                Dim Index As Integer = -1
                Index = Биты.FindIndex(AddressOf FindItemБит)

                If Index <> -1 Then
                    Return Биты(Index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Shared Function FindItemБит(ByVal s As Бит) As Boolean
            If s.keyБитПорта = MemoKeyBit Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьПортыTableAdapter(mkeyДействие)
        '            Dim rowFindПорты As ChannelsDigitalOutputDataSet.ПортыRow _
        '            = .ChannelsDigitalOutputDataSet.Порты.FindByKeyПорта(mKeyПорта)
        '            If rowFindПорты IsNot Nothing Then
        '                rowFindПорты.keyДействие = gkeyДействие
        '                rowFindПорты.name = Name
        '                rowFindПорты.НомерУстройства = НомерУстройства
        '                rowFindПорты.НомерМодуляКорзины = НомерМодуляКорзины
        '                rowFindПорты.НомерПорта = НомерПорта

        '                .ОбновитьПортыTableAdapter()
        '                .ЗаполнитьПортыTableAdapter(mkeyДействие)
        '            End If
        '        End With
        '        'System.Threading.Thread.Sleep(100)
        '        'Application.DoEvents()
        '    End If
        'End Sub

        Shared Function GetNewBit() As Бит()
            Return New Бит() {New Бит()}
        End Function
    End Class

#End Region

#Region "Бит"
    <DisplayName("Бит")>
    <Description("Номер установленного Бита")>
    Public Class Бит
        Inherits TreeNodeBase

        Public Sub New()
            Me.New("Введите описание чем управляет данный Бит")
            Тип = TypeGridDigitalNode.Bit
            keyBitPort = DigitalPortForm.GetKeyId()
        End Sub

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = TypeGridDigitalNode.Bit
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
                DigitalPortForm.DtvwDirectory.RefreshTree()
                MemoKeyBit = keyBitPort
                DigitalPortForm.FindLastNode(Тип)
            End Set
        End Property

        Private keyBitPort As Integer
        <Browsable(False)>
        Public Property keyБитПорта() As Integer
            Get
                Return keyBitPort
            End Get
            Set(ByVal value As Integer)
                MemoKeyBit = value
                keyBitPort = value
            End Set
        End Property
        <Browsable(False)>
        Public Property KeyПорта() As Integer
        <Browsable(False)>
        Public Property Parent() As Порт

        Private numberBit As Integer = 0
        ''' <summary>
        ''' Номер Бита
        ''' </summary>
        <DisplayName("Номер Бита")>
        <Description("Номер бита является номером цифровой линии порта, значение которого установится в высокий уровень при срабатывании условия")>
        <Category("2. Номер Бита")>
        <PropertyOrder(140)>
        <TypeConverter(GetType(NumberBitTypeConverter))>
        Public Property НомерБита() As Integer
            Get
                Return numberBit
            End Get
            Set(ByVal value As Integer)
                numberBit = value
                'ОбновитьЗаписьВТаблице()
                RefreshContextVisitor.UpdateTree(Me) ', DigitalPortForm.DtvwDirectory)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
            visitor.Visit(context, Me)
        End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateNode(context, Me)
        End Sub

        'Private Sub ОбновитьЗаписьВТаблице()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьБитПортаTableAdapter(mKeyПорта)
        '            Dim rowFindБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
        '            = .ChannelsDigitalOutputDataSet.БитПорта.FindBykeyБитПорта(mkeyБитПорта)
        '            If rowFindБитПорта IsNot Nothing Then
        '                rowFindБитПорта.KeyПорта = gKeyПорта
        '                rowFindБитПорта.name = Name
        '                rowFindБитПорта.НомерБита = НомерБита

        '                .ОбновитьБитПортаTableAdapter()
        '                .ЗаполнитьБитПортаTableAdapter(mKeyПорта)
        '            End If
        '        End With
        '    End If
        'End Sub

        'Private Sub Создать()
        '    'добавить если загрузка то выход
        '    'сделать общий метод изенения всех свойст, чтобы не вызывать конкретно из каждого метода
        '    If Not blnИдетСчитываниеИзDataSet Then
        '        With frmDigitalOutputPort
        '            .ЗаполнитьБитПортаTableAdapter(mKeyПорта)
        '            'Dim rowFindБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
        '            '= .ChannelsDigitalOutputDataSet.БитПорта.FindBykeyБитПорта(mkeyБитПорта)
        '            'If rowFindБитПорта IsNot Nothing Then
        '            Dim newrowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
        '            = .ChannelsDigitalOutputDataSet.БитПорта.NewБитПортаRow 'новая строка

        '            newrowБитПорта.KeyПорта = gKeyПорта
        '            newrowБитПорта.name = Name
        '            newrowБитПорта.НомерБита = НомерБита
        '            .ChannelsDigitalOutputDataSet.БитПорта.AddБитПортаRow(newrowБитПорта)

        '            .ОбновитьБитПортаTableAdapter()
        '            .ЗаполнитьБитПортаTableAdapter(mKeyПорта)
        '            'End If
        '        End With
        '    End If
        'End Sub

    End Class

#End Region

    'Private Function CreateTree() As TreeNodeBase
    '    'Return New RootNode(New Type1Node() {New Type1Node("T11"), New Type1Node("T12"), New Type1Node("T13")}, _
    '    '                    New Type2Node() _
    '    '                    {New Type2Node("T21", _
    '    '                                    New Type3Node() {New Type3Node("T311"), New Type3Node("T312")}), _
    '    '                    New Type2Node("T22", _
    '    '                                    New Type3Node() {New Type3Node("T321"), New Type3Node("T322")})})

    '    Dim mДействие_1 As Действие
    '    Dim arrДействие() As Действие

    '    Dim mТриггерСрабатыванияЦифровогоВыхода_1 As ТриггерСрабатыванияЦифровогоВыхода
    '    Dim arrТриггерСрабатыванияЦифровогоВыхода() As ТриггерСрабатыванияЦифровогоВыхода


    '    Dim mФормулаСрабатыванияЦифровогоВыхода_1 As ФормулаСрабатыванияЦифровогоВыхода
    '    Dim arrФормулаСрабатыванияЦифровогоВыхода() As ФормулаСрабатыванияЦифровогоВыхода

    '    Dim mПорт As Порт
    '    Dim arrПорт() As Порт

    '    Dim mАргументДляФормулы_1 As АргументДляФормулы
    '    Dim mАргументДляФормулы_2 As АргументДляФормулы
    '    Dim arrАргументДляФормулы() As АргументДляФормулы

    '    Dim mБит_1 As Бит
    '    Dim mБит_2 As Бит
    '    Dim arrБит() As Бит

    '    mБит_1 = New Бит("Бит_1")
    '    mБит_2 = New Бит("Бит_2")
    '    arrБит = New Бит() {mБит_1, mБит_2}

    '    mАргументДляФормулы_1 = New АргументДляФормулы("АргументДляФормулы_1")
    '    mАргументДляФормулы_2 = New АргументДляФормулы("АргументДляФормулы_2")
    '    arrАргументДляФормулы = New АргументДляФормулы() {mАргументДляФормулы_1, mАргументДляФормулы_2}


    '    mТриггерСрабатыванияЦифровогоВыхода_1 = New ТриггерСрабатыванияЦифровогоВыхода("ТриггерСрабатыванияЦифровогоВыхода_1")
    '    mФормулаСрабатыванияЦифровогоВыхода_1 = New ФормулаСрабатыванияЦифровогоВыхода("ФормулаСрабатыванияЦифровогоВыхода_1", arrАргументДляФормулы)
    '    mПорт = New Порт("Порт_1", arrБит)

    '    Dim mТриггерСрабатыванияЦифровогоВыхода_2 = New ТриггерСрабатыванияЦифровогоВыхода("ТриггерСрабатыванияЦифровогоВыхода_2")
    '    Dim mФормулаСрабатыванияЦифровогоВыхода_2 = New ФормулаСрабатыванияЦифровогоВыхода("ФормулаСрабатыванияЦифровогоВыхода_2", arrАргументДляФормулы)
    '    Dim mПорт_2 = New Порт("Порт_2", arrБит)


    '    arrТриггерСрабатыванияЦифровогоВыхода = New ТриггерСрабатыванияЦифровогоВыхода() {mТриггерСрабатыванияЦифровогоВыхода_1, mТриггерСрабатыванияЦифровогоВыхода_2}
    '    arrФормулаСрабатыванияЦифровогоВыхода = New ФормулаСрабатыванияЦифровогоВыхода() {mФормулаСрабатыванияЦифровогоВыхода_1, mФормулаСрабатыванияЦифровогоВыхода_2}
    '    arrПорт = New Порт() {mПорт, mПорт_2}


    '    mДействие_1 = New Действие("Действие_1", Nothing, arrФормулаСрабатыванияЦифровогоВыхода, arrПорт)
    '    Dim mДействие_2 As Действие = New Действие("Действие_2", arrТриггерСрабатыванияЦифровогоВыхода, Nothing, arrПорт)
    '    Dim mДействие_3 As Действие = New Действие("Действие_3", Nothing, Nothing, arrПорт)
    '    Dim mДействие_4 As Действие = New Действие("Действие_4", arrТриггерСрабатыванияЦифровогоВыхода, arrФормулаСрабатыванияЦифровогоВыхода, arrПорт)

    '    'mДействие_1 = New Действие("Действие_1", arrТриггерСрабатыванияЦифровогоВыхода, arrФормулаСрабатыванияЦифровогоВыхода, arrПорт)
    '    'Dim mДействие_2 As Действие = New Действие("Действие_2", arrТриггерСрабатыванияЦифровогоВыхода, arrФормулаСрабатыванияЦифровогоВыхода, arrПорт)

    '    arrДействие = New Действие() {mДействие_1, mДействие_2, mДействие_3, mДействие_4}

    '    Dim mКонфигурация_1 As Конфигурация
    '    Dim mКонфигурация_2 As Конфигурация
    '    mКонфигурация_1 = New Конфигурация("Конфигурация_1", arrДействие)
    '    mКонфигурация_2 = New Конфигурация("Конфигурация_2", arrДействие)

    '    Dim arrКонфигурация() As Конфигурация
    '    arrКонфигурация = New Конфигурация() {mКонфигурация_1, mКонфигурация_2}

    '    'Return New ВсеКонфигурации(arrКонфигурация)
    '    mВсеКонфигурации = New ВсеКонфигурации(arrКонфигурация)
    '    Return mВсеКонфигурации
    'End Function


    'Основная идея этого паттерна состоит в том, что каждый элемент объектной структуры содержит метод Accept, 
    'который принимает на вход в качестве аргумента специальный объект, Посетитель, реализующий заранее известный интерфейс. 
    'Этот интерфейс содержит по одному методу Visit для каждого типа узла. Метод Accept в каждом узле должен 
    'вызывать методы Visit для осуществления навигации по структуре.

    'через контекст вызова context передается дополнительный параметр, нужный для программы


    'Public Sub ЗаписатьОбновленияВБазу()
    '    blnИдетСчитываниеИзDataSet = True
    '    Select Case ТипУровня
    '        'Case EnumDigitalNode.ВсеКонфигурации
    '        '    Stop
    '        Case EnumDigitalNode.Конфигурация, EnumDigitalNode.ВсеКонфигурации
    '            ОбновитьКонфигурация()
    '            'Case EnumDigitalNode.ВсеДействия
    '            '    Stop
    '        Case EnumDigitalNode.Действие, EnumDigitalNode.ВсеДействия
    '            ОбновитьДействие()
    '            'Case EnumDigitalNode.ВсеТриггеры
    '            '    Stop
    '        Case EnumDigitalNode.Триггер, EnumDigitalNode.ВсеТриггеры
    '            ОбновитьТриггер()
    '            'Case EnumDigitalNode.ВсеФормулы
    '            '    Stop
    '        Case EnumDigitalNode.Формула, EnumDigitalNode.ВсеФормулы
    '            ОбновитьФормула()
    '            'Case EnumDigitalNode.ВсеАргументы
    '            '    Stop
    '        Case EnumDigitalNode.Аргумент, EnumDigitalNode.ВсеАргументы
    '            ОбновитьАргумент()
    '            'Case EnumDigitalNode.ВсеПорты
    '            '    Stop
    '        Case EnumDigitalNode.Порт, EnumDigitalNode.ВсеПорты
    '            ОбновитьПорт()
    '            'Case EnumDigitalNode.ВсеУстановленныеБиты
    '            '    Stop
    '        Case EnumDigitalNode.Бит, EnumDigitalNode.ВсеУстановленныеБиты
    '            ОбновитьБит()
    '    End Select
    '    blnИдетСчитываниеИзDataSet = False
    '    frmDigitalOutputPort.dtvwDirectory.RefreshTree()

    'End Sub


    'Private Sub ОбновитьКонфигурация()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьКонфигурацияTableAdapter()
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mКонфигурация As Конфигурация In mВсеКонфигурации.ListВсеКонфигурации
    '                Dim rowFindКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow _
    '                = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.FindBykeyКонфигурацияДействия(mКонфигурация.keyКонфигурацияДействия)

    '                If rowFindКонфигурация Is Nothing Then 'создать новую запись
    '                    Dim newrowКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow _
    '                    = .ChannelsDigitalOutputDataSet.КонфигурацияДействий.NewКонфигурацияДействийRow 'новая строка
    '                    'newrowКонфигурация.Key = нет 'Для новой записи обязательно внешний ключ
    '                    newrowКонфигурация.ИмяКонфигурации = mКонфигурация.Name
    '                    newrowКонфигурация.Описание = mКонфигурация.Описание

    '                    .ChannelsDigitalOutputDataSet.КонфигурацияДействий.AddКонфигурацияДействийRow(newrowКонфигурация)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow In .ChannelsDigitalOutputDataSet.КонфигурацияДействий
    '                blnЗаписьНайдена = False
    '                For Each mКонфигурация As Конфигурация In mВсеКонфигурации.ListВсеКонфигурации
    '                    If rowКонфигурация.keyКонфигурацияДействия > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                        If rowКонфигурация.keyКонфигурацияДействия = mКонфигурация.keyКонфигурацияДействия Then
    '                            blnЗаписьНайдена = True
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnЗаписьНайдена = True
    '                    End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.КонфигурацияДействий.FindBykeyКонфигурацияДействия(rowКонфигурация.keyКонфигурацияДействия).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Конфигурация", "Процедура ОбновитьКонфигурация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            frmDigitalOutputPort.ОбновитьКонфигурацияTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьКонфигурацияTableAdapter()
    '            'а затем заново обновить первичные ключи для добавленных
    '            'сначало считать
    '            Dim crListКонфигурация As List(Of Конфигурация) = New List(Of Конфигурация)
    '            For Each rowКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.КонфигурацияДействий 'tblКонфигурацияДействий.Rows
    '                'Dim crКонфигурация As Конфигурация = New Конфигурация(rowКонфигурация.ИмяКонфигурации, crListДействие)
    '                Dim crКонфигурация As Конфигурация = New Конфигурация(rowКонфигурация.ИмяКонфигурации, mВсеКонфигурации.ItemКонфигурация.ListДействие)
    '                crКонфигурация.keyКонфигурацияДействия = rowКонфигурация.keyКонфигурацияДействия
    '                If Not IsDBNull(rowКонфигурация.Описание) Then
    '                    crКонфигурация.Описание = rowКонфигурация.Описание
    '                End If
    '                crListКонфигурация.Add(crКонфигурация)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ListВсеКонфигурации = crListКонфигурация
    '        End If
    '    End Try
    'End Sub

    'Private Sub ОбновитьДействие()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьДействиеTableAdapter(gkeyКонфигурацияДействия)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mДействие As Действие In mВсеКонфигурации.ItemКонфигурация.ListДействие
    '                Dim rowFindДействие As ChannelsDigitalOutputDataSet.ДействиеRow _
    '                = .ChannelsDigitalOutputDataSet.Действие.FindBykeyДействие(mДействие.keyДействие)

    '                If rowFindДействие Is Nothing Then 'создать новую запись
    '                    Dim newrowДействие As ChannelsDigitalOutputDataSet.ДействиеRow _
    '                    = .ChannelsDigitalOutputDataSet.Действие.NewДействиеRow 'новая строка
    '                    newrowДействие.keyКонфигурацияДействия = gkeyКонфигурацияДействия 'Для новой записи обязательно внешний ключ
    '                    newrowДействие.ИмяДействия = mДействие.Name

    '                    .ChannelsDigitalOutputDataSet.Действие.AddДействиеRow(newrowДействие)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowДействие As ChannelsDigitalOutputDataSet.ДействиеRow In .ChannelsDigitalOutputDataSet.Действие
    '                blnЗаписьНайдена = False
    '                For Each mДействие As Действие In mВсеКонфигурации.ItemКонфигурация.ListДействие
    '                    If rowДействие.keyДействие > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                        If rowДействие.keyДействие = mДействие.keyДействие Then
    '                            blnЗаписьНайдена = True
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnЗаписьНайдена = True
    '                    End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.Действие.FindBykeyДействие(rowДействие.keyДействие).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Действие", "Процедура ОбновитьДействие", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        'а затем заново обновить первичные ключи для добавленных
    '        'сначало считать
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            frmDigitalOutputPort.ОбновитьДействиеTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьДействиеTableAdapter(gkeyКонфигурацияДействия)

    '            Dim crListДействие As List(Of Действие) = New List(Of Действие)
    '            For Each rowДействие As ChannelsDigitalOutputDataSet.ДействиеRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.Действие 'rowКонфигурация.GetДействиеRows
    '                'Dim crДействие As Действие = New Действие(rowДействие.ИмяДействия, crListТриггерСрабатыванияЦифровогоВыхода, crListФормулаСрабатыванияЦифровогоВыхода, crListПорт)
    '                Dim crДействие As Действие = New Действие(rowДействие.ИмяДействия, mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListТриггерСрабатыванияЦифровогоВыхода, mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListФормулаСрабатыванияЦифровогоВыхода, mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListПорт)
    '                crДействие.keyДействие = rowДействие.keyДействие
    '                crДействие.keyКонфигурацияДействия = rowДействие.keyКонфигурацияДействия
    '                crListДействие.Add(crДействие)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ItemКонфигурация.ListДействие = crListДействие
    '        End If
    '    End Try
    'End Sub

    'Private Sub ОбновитьТриггер()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьТриггерСрабатыванияЦифровогоВыходаTableAdapter(gkeyДействие)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mТриггерСрабатыванияЦифровогоВыхода As ТриггерСрабатыванияЦифровогоВыхода In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListТриггерСрабатыванияЦифровогоВыхода
    '                Dim rowFindТриггер As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow _
    '                = .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.FindByKeyТриггер(mТриггерСрабатыванияЦифровогоВыхода.KeyТриггер)

    '                If rowFindТриггер Is Nothing Then
    '                    Dim newrowТриггер As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow _
    '                    = .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.NewТриггерСрабатыванияЦифровогоВыходаRow 'новая строка
    '                    newrowТриггер.keyДествие = gkeyДействие 'Для новой записи обязательно
    '                    newrowТриггер.name = mТриггерСрабатыванияЦифровогоВыхода.Name
    '                    newrowТриггер.ИмяКанала = mТриггерСрабатыванияЦифровогоВыхода.ИмяКанала
    '                    newrowТриггер.ОперацияСравнения = mТриггерСрабатыванияЦифровогоВыхода.ОперацияСравнения
    '                    newrowТриггер.ВеличинаУсловия = mТриггерСрабатыванияЦифровогоВыхода.ВеличинаУсловия
    '                    newrowТриггер.ВеличинаУсловия2 = mТриггерСрабатыванияЦифровогоВыхода.ВеличинаУсловия2

    '                    .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.AddТриггерСрабатыванияЦифровогоВыходаRow(newrowТриггер)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице ТриггерСрабатыванияЦифровогоВыхода которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowТриггер As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow In .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода
    '                blnЗаписьНайдена = False
    '                For Each mТриггерСрабатыванияЦифровогоВыхода As ТриггерСрабатыванияЦифровогоВыхода In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListТриггерСрабатыванияЦифровогоВыхода
    '                    If rowТриггер.KeyТриггер > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                        If rowТриггер.KeyТриггер = mТриггерСрабатыванияЦифровогоВыхода.KeyТриггер Then
    '                            blnЗаписьНайдена = True
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnЗаписьНайдена = True
    '                    End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.FindByKeyТриггер(rowТриггер.KeyТриггер).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Триггера", "Процедура ОбновитьТриггер", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            frmDigitalOutputPort.ОбновитьТриггерСрабатыванияЦифровогоВыходаTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьТриггерСрабатыванияЦифровогоВыходаTableAdapter(gkeyДействие)
    '            'а затем заново обновить первичные ключи для добавленных
    '            Dim crListТриггерСрабатыванияЦифровогоВыхода As List(Of ТриггерСрабатыванияЦифровогоВыхода) = New List(Of ТриггерСрабатыванияЦифровогоВыхода)
    '            For Each rowТриггер As ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода 'rowДействие.GetТриггерСрабатыванияЦифровогоВыходаRows
    '                Dim crТриггерСрабатыванияЦифровогоВыхода As ТриггерСрабатыванияЦифровогоВыхода = New ТриггерСрабатыванияЦифровогоВыхода(rowТриггер.name)
    '                crТриггерСрабатыванияЦифровогоВыхода.ИмяКанала = rowТриггер.ИмяКанала
    '                crТриггерСрабатыванияЦифровогоВыхода.ОперацияСравнения = rowТриггер.ОперацияСравнения
    '                crТриггерСрабатыванияЦифровогоВыхода.ВеличинаУсловия = rowТриггер.ВеличинаУсловия
    '                crТриггерСрабатыванияЦифровогоВыхода.ВеличинаУсловия2 = rowТриггер.ВеличинаУсловия2
    '                crТриггерСрабатыванияЦифровогоВыхода.KeyТриггер = rowТриггер.KeyТриггер
    '                crТриггерСрабатыванияЦифровогоВыхода.keyДействие = rowТриггер.keyДествие
    '                crListТриггерСрабатыванияЦифровогоВыхода.Add(crТриггерСрабатыванияЦифровогоВыхода)
    '            Next
    '            mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListТриггерСрабатыванияЦифровогоВыхода = crListТриггерСрабатыванияЦифровогоВыхода
    '        End If
    '    End Try
    'End Sub

    'Private Sub ОбновитьФормула()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьФормулаСрабатыванияЦифровогоВыходаTableAdapter(gkeyДействие)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mФормула As ФормулаСрабатыванияЦифровогоВыхода In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListФормулаСрабатыванияЦифровогоВыхода
    '                Dim rowFindФормула As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow _
    '                = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.FindByKeyFormula(mФормула.KeyFormula)

    '                If rowFindФормула Is Nothing Then 'создать новую запись
    '                    Dim newrowФормула As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow _
    '                    = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.NewФормулаСрабатыванияЦифровогоВыходаRow 'новая строка
    '                    newrowФормула.keyДействие = gkeyДействие 'Для новой записи обязательно внешний ключ
    '                    newrowФормула.name = mФормула.Name
    '                    newrowФормула.Формула = mФормула.Формула

    '                    .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.AddФормулаСрабатыванияЦифровогоВыходаRow(newrowФормула)
    '                    frmDigitalOutputPort.ОбновитьФормулаСрабатыванияЦифровогоВыходаTableAdapter()
    '                    frmDigitalOutputPort.ЗаполнитьФормулаСрабатыванияЦифровогоВыходаTableAdapter(gkeyДействие)
    '                    'затем нужно добавить все созданные Аргументы т.к. mФормула.KeyFormula уже определен

    '                    Dim Newkey As Integer = .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.Last.KeyFormula
    '                    mФормула.KeyFormula = Newkey
    '                    gKeyFormula = Newkey
    '                    'ЗаписатьListФормула(mФормула.ListАргументДляФормулы, Newkey)

    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowФормула As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow In .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода
    '                blnЗаписьНайдена = False
    '                For Each mФормула As ФормулаСрабатыванияЦифровогоВыхода In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListФормулаСрабатыванияЦифровогоВыхода
    '                    'If rowФормула.KeyFormula > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                    If rowФормула.KeyFormula = mФормула.KeyFormula Then
    '                        blnЗаписьНайдена = True
    '                        Exit For
    '                    End If
    '                    'Else
    '                    'blnЗаписьНайдена = True
    '                    'End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.FindByKeyFormula(rowФормула.KeyFormula).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '            If blnБылоУдаление Then
    '                frmDigitalOutputPort.ОбновитьФормулаСрабатыванияЦифровогоВыходаTableAdapter()
    '                frmDigitalOutputPort.ЗаполнитьФормулаСрабатыванияЦифровогоВыходаTableAdapter(gkeyДействие)
    '            End If
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Формула", "Процедура ОбновитьФормула", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            Dim crListФормулаСрабатыванияЦифровогоВыхода As List(Of ФормулаСрабатыванияЦифровогоВыхода) = New List(Of ФормулаСрабатыванияЦифровогоВыхода)
    '            For Each rowФормула As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода 'rowДействие.GetФормулаСрабатыванияЦифровогоВыходаRows
    '                Dim crListАргументДляФормулы As List(Of АргументДляФормулы) = New List(Of АргументДляФормулы)
    '                'обновить таблицу
    '                frmDigitalOutputPort.ЗаполнитьАргументыДляФормулыTableAdapter(rowФормула.KeyFormula)
    '                For Each rowАргумент As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow In rowФормула.GetАргументыДляФормулыRows
    '                    Dim crАргументДляФормулы As АргументДляФормулы = New АргументДляФормулы(rowАргумент.ИмяАргумента)
    '                    crАргументДляФормулы.ИмяКанала = rowАргумент.ИмяКанала
    '                    crАргументДляФормулы.КеуArgument = rowАргумент.КеуArgument
    '                    crАргументДляФормулы.KeyFormula = rowАргумент.KeyFormula
    '                    crListАргументДляФормулы.Add(crАргументДляФормулы)
    '                Next

    '                Dim crФормулаСрабатыванияЦифровогоВыхода As ФормулаСрабатыванияЦифровогоВыхода = New ФормулаСрабатыванияЦифровогоВыхода(rowФормула.name, crListАргументДляФормулы)
    '                'Dim crФормулаСрабатыванияЦифровогоВыхода As ФормулаСрабатыванияЦифровогоВыхода = New ФормулаСрабатыванияЦифровогоВыхода(rowФормула.name, mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemФормула.ListАргументДляФормулы)

    '                crФормулаСрабатыванияЦифровогоВыхода.Формула = rowФормула.Формула
    '                crФормулаСрабатыванияЦифровогоВыхода.KeyFormula = rowФормула.KeyFormula
    '                crФормулаСрабатыванияЦифровогоВыхода.keyДействие = rowФормула.keyДействие
    '                crListФормулаСрабатыванияЦифровогоВыхода.Add(crФормулаСрабатыванияЦифровогоВыхода)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListФормулаСрабатыванияЦифровогоВыхода = crListФормулаСрабатыванияЦифровогоВыхода
    '        End If
    '    End Try
    'End Sub

    'Private Sub ОбновитьАргумент()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьАргументыДляФормулыTableAdapter(gKeyFormula)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mАргумент As АргументДляФормулы In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemФормула.ListАргументДляФормулы
    '                Dim rowFindАргумент As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow _
    '                = .ChannelsDigitalOutputDataSet.АргументыДляФормулы.FindByКеуArgument(mАргумент.КеуArgument)

    '                If rowFindАргумент Is Nothing Then 'создать новую запись
    '                    Dim newrowАргумент As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow _
    '                    = .ChannelsDigitalOutputDataSet.АргументыДляФормулы.NewАргументыДляФормулыRow 'новая строка
    '                    newrowАргумент.KeyFormula = gKeyFormula 'Для новой записи обязательно внешний ключ
    '                    newrowАргумент.ИмяАргумента = mАргумент.Name
    '                    newrowАргумент.ИмяКанала = mАргумент.ИмяКанала
    '                    newrowАргумент.Приведение = mАргумент.Приведение

    '                    .ChannelsDigitalOutputDataSet.АргументыДляФормулы.AddАргументыДляФормулыRow(newrowАргумент)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowАргумент As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow In .ChannelsDigitalOutputDataSet.АргументыДляФормулы
    '                blnЗаписьНайдена = False
    '                For Each mАргумент As АргументДляФормулы In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemФормула.ListАргументДляФормулы
    '                    If rowАргумент.КеуArgument > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                        If rowАргумент.КеуArgument = mАргумент.КеуArgument Then
    '                            blnЗаписьНайдена = True
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnЗаписьНайдена = True
    '                    End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.АргументыДляФормулы.FindByКеуArgument(rowАргумент.КеуArgument).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Аргумента", "Процедура ОбновитьАргумент", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            frmDigitalOutputPort.ОбновитьАргументыДляФормулыTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьАргументыДляФормулыTableAdapter(gKeyFormula)
    '            'а затем заново обновить первичные ключи для добавленных
    '            'сначало считать
    '            Dim crListАргументДляФормулы As List(Of АргументДляФормулы) = New List(Of АргументДляФормулы)
    '            For Each rowАргумент As ChannelsDigitalOutputDataSet.АргументыДляФормулыRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.АргументыДляФормулы 'rowФормула.GetАргументыДляФормулыRows
    '                Dim crАргументДляФормулы As АргументДляФормулы = New АргументДляФормулы(rowАргумент.ИмяАргумента)
    '                crАргументДляФормулы.ИмяКанала = rowАргумент.ИмяКанала
    '                crАргументДляФормулы.Приведение = rowАргумент.Приведение
    '                crАргументДляФормулы.КеуArgument = rowАргумент.КеуArgument
    '                crАргументДляФормулы.KeyFormula = rowАргумент.KeyFormula
    '                crListАргументДляФормулы.Add(crАргументДляФормулы)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemФормула.ListАргументДляФормулы = crListАргументДляФормулы
    '        End If
    '    End Try
    'End Sub

    'Private Sub ОбновитьПорт()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьПортыTableAdapter(gkeyДействие)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mПорт As Порт In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListПорт
    '                Dim rowFindПорт As ChannelsDigitalOutputDataSet.ПортыRow _
    '                = .ChannelsDigitalOutputDataSet.Порты.FindByKeyПорта(mПорт.KeyПорта)

    '                If rowFindПорт Is Nothing Then 'создать новую запись
    '                    Dim newrowПорт As ChannelsDigitalOutputDataSet.ПортыRow _
    '                    = .ChannelsDigitalOutputDataSet.Порты.NewПортыRow 'новая строка
    '                    newrowПорт.keyДействие = gkeyДействие 'Для новой записи обязательно внешний ключ
    '                    newrowПорт.name = mПорт.Name
    '                    newrowПорт.НомерУстройства = mПорт.НомерУстройства
    '                    newrowПорт.НомерМодуляКорзины = mПорт.НомерМодуляКорзины
    '                    newrowПорт.НомерПорта = mПорт.НомерПорта

    '                    .ChannelsDigitalOutputDataSet.Порты.AddПортыRow(newrowПорт)
    '                    frmDigitalOutputPort.ОбновитьПортыTableAdapter()
    '                    frmDigitalOutputPort.ЗаполнитьПортыTableAdapter(gkeyДействие)

    '                    'затем нужно добавить все созданные Биты т.к. newrowПорт.KeyПорта уже определен
    '                    'NewkeyКонфигурацияДействия = .ChannelsDigitalOutputDataSet.Порты.Last.KeyПорта
    '                    Dim Newkey As Integer = .ChannelsDigitalOutputDataSet.Порты.Last.KeyПорта
    '                    mПорт.KeyПорта = Newkey
    '                    gKeyПорта = Newkey
    '                    ЗаписатьListБит(mПорт.ListБит, Newkey)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowПорт As ChannelsDigitalOutputDataSet.ПортыRow In .ChannelsDigitalOutputDataSet.Порты
    '                blnЗаписьНайдена = False
    '                For Each mПорт As Порт In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListПорт
    '                    'If rowПорт.KeyПорта > 0 Or mПорт.KeyПорта = 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                    If rowПорт.KeyПорта = mПорт.KeyПорта Then
    '                        blnЗаписьНайдена = True
    '                        Exit For
    '                    End If
    '                    'Else
    '                    'blnЗаписьНайдена = True
    '                    'End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.Порты.FindByKeyПорта(rowПорт.KeyПорта).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '            If blnБылоУдаление Then
    '                frmDigitalOutputPort.ОбновитьПортыTableAdapter()
    '                frmDigitalOutputPort.ЗаполнитьПортыTableAdapter(gkeyДействие)
    '            End If
    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Порт", "Процедура ОбновитьПорт", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            Dim crListПорт As List(Of Порт) = New List(Of Порт)
    '            For Each rowПорт As ChannelsDigitalOutputDataSet.ПортыRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.Порты.Rows 'rowДействие.GetПортыRows
    '                Dim crListБит As List(Of Бит) = New List(Of Бит)
    '                'обновить таблицу
    '                frmDigitalOutputPort.ЗаполнитьБитПортаTableAdapter(rowПорт.KeyПорта)
    '                For Each rowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow In rowПорт.GetБитПортаRows
    '                    Dim crБит As Бит = New Бит(rowБитПорта.name)
    '                    crБит.keyБитПорта = rowБитПорта.keyБитПорта
    '                    crБит.KeyПорта = rowБитПорта.KeyПорта
    '                    crБит.НомерБита = rowБитПорта.НомерБита
    '                    crListБит.Add(crБит)
    '                Next
    '                Dim crПорт As Порт = New Порт(rowПорт.name, crListБит)
    '                'Dim crПорт As Порт = New Порт(rowПорт.name, mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemПорт.ListБит)
    '                crПорт.НомерУстройства = rowПорт.НомерУстройства
    '                If Not IsDBNull(rowПорт.НомерМодуляКорзины) Then
    '                    crПорт.НомерМодуляКорзины = rowПорт.НомерМодуляКорзины
    '                End If
    '                crПорт.НомерПорта = rowПорт.НомерПорта
    '                crПорт.KeyПорта = rowПорт.KeyПорта
    '                crПорт.keyДействие = rowПорт.keyДействие
    '                crListПорт.Add(crПорт)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ItemКонфигурация.ItemДействие.ListПорт = crListПорт
    '        End If
    '    End Try
    'End Sub


    'Private Sub ListФормула(ByVal mПортListБит As List(Of Бит), ByVal newrowПортKeyПорта As Integer)
    '    Dim blnБылоДобавление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьБитПортаTableAdapter(newrowПортKeyПорта)
    '            For Each mБит As Бит In mПортListБит
    '                Dim newrowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
    '                    = .ChannelsDigitalOutputDataSet.БитПорта.NewБитПортаRow 'новая строка
    '                newrowБитПорта.KeyПорта = newrowПортKeyПорта 'Для новой записи обязательно внешний ключ
    '                newrowБитПорта.name = mБит.Name
    '                newrowБитПорта.НомерБита = mБит.НомерБита

    '                .ChannelsDigitalOutputDataSet.БитПорта.AddБитПортаRow(newrowБитПорта)
    '                blnБылоДобавление = True
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Бит", "Процедура ЗаписатьListБит", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        'If blnБылоДобавление Then
    '        '    frmDigitalOutputPort.ОбновитьБитПортаTableAdapter()
    '        '    frmDigitalOutputPort.ЗаполнитьБитПортаTableAdapter(newrowПортKeyПорта)
    '        'End If
    '    End Try
    '    'System.Threading.Thread.Sleep(500)
    '    Application.DoEvents()
    'End Sub

    'Private Sub ЗаписатьListБит(ByVal mПортListБит As List(Of Бит), ByVal newrowПортKeyПорта As Integer)
    '    Dim blnБылоДобавление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьБитПортаTableAdapter(newrowПортKeyПорта)
    '            For Each mБит As Бит In mПортListБит
    '                Dim newrowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
    '                    = .ChannelsDigitalOutputDataSet.БитПорта.NewБитПортаRow 'новая строка
    '                newrowБитПорта.KeyПорта = newrowПортKeyПорта 'Для новой записи обязательно внешний ключ
    '                newrowБитПорта.name = mБит.Name
    '                newrowБитПорта.НомерБита = mБит.НомерБита

    '                .ChannelsDigitalOutputDataSet.БитПорта.AddБитПортаRow(newrowБитПорта)
    '                blnБылоДобавление = True
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Бит", "Процедура ЗаписатьListБит", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление Then
    '            frmDigitalOutputPort.ОбновитьБитПортаTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьБитПортаTableAdapter(newrowПортKeyПорта)
    '        End If
    '    End Try
    '    'System.Threading.Thread.Sleep(500)
    '    Application.DoEvents()
    'End Sub

    'Private Sub ОбновитьБит()
    '    'здесь определить узел из которого вызвана сетка свойств и в которой произошло удаление
    '    Dim blnБылоДобавление As Boolean
    '    Dim blnБылоУдаление As Boolean
    '    Try
    '        With frmDigitalOutputPort
    '            .ЗаполнитьБитПортаTableAdapter(gKeyПорта)
    '            'здесь в цикле по коллекции определить удаленные записи из коллекции класса и удалить их из таблицыDataSet
    '            'а затем обновить
    '            'смотреть в коллекцию 2 случая удаления и добавления
    '            'ищем добавленные строки в коллекцию mВсеКонфигурации по их отсутствию в таблице ТриггерСрабатыванияЦифровогоВыхода
    '            For Each mБит As Бит In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemПорт.ListБит
    '                Dim rowFindБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
    '                = .ChannelsDigitalOutputDataSet.БитПорта.FindBykeyБитПорта(mБит.keyБитПорта)

    '                If rowFindБитПорта Is Nothing Then 'создать новую запись
    '                    Dim newrowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow _
    '                    = .ChannelsDigitalOutputDataSet.БитПорта.NewБитПортаRow 'новая строка
    '                    newrowБитПорта.KeyПорта = gKeyПорта 'Для новой записи обязательно внешний ключ
    '                    newrowБитПорта.name = mБит.Name
    '                    newrowБитПорта.НомерБита = mБит.НомерБита

    '                    .ChannelsDigitalOutputDataSet.БитПорта.AddБитПортаRow(newrowБитПорта)
    '                    blnБылоДобавление = True
    '                End If
    '            Next

    '            'затем ищем удаленные в коллекции mВсеКонфигурации 
    '            'по наличию записей в таблице БитПорта которых нет в mВсеКонфигурации 
    '            Dim blnЗаписьНайдена As Boolean
    '            For Each rowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow In .ChannelsDigitalOutputDataSet.БитПорта
    '                blnЗаписьНайдена = False
    '                For Each mБит As Бит In mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemПорт.ListБит
    '                    If rowБитПорта.keyБитПорта > 0 Then 'сравниваем только положительные индексы т.к. отрицательные это новые записи
    '                        If rowБитПорта.keyБитПорта = mБит.keyБитПорта Then
    '                            blnЗаписьНайдена = True
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnЗаписьНайдена = True
    '                    End If
    '                Next
    '                If Not blnЗаписьНайдена Then
    '                    .ChannelsDigitalOutputDataSet.БитПорта.FindBykeyБитПорта(rowБитПорта.keyБитПорта).Delete()
    '                    blnБылоУдаление = True
    '                End If
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MessageBox.Show("Ошибка при записи обновлений Бит", "Процедура ОбновитьБит", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Finally
    '        If blnБылоДобавление OrElse blnБылоУдаление Then
    '            frmDigitalOutputPort.ОбновитьБитПортаTableAdapter()
    '            frmDigitalOutputPort.ЗаполнитьБитПортаTableAdapter(gKeyПорта)

    '            'mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemПорт.ListБит = New List(Of Бит)
    '            Dim crListБит As List(Of Бит) = New List(Of Бит)
    '            For Each rowБитПорта As ChannelsDigitalOutputDataSet.БитПортаRow In frmDigitalOutputPort.ChannelsDigitalOutputDataSet.БитПорта 'rowПорт.GetБитПортаRows
    '                Dim crБит As Бит = New Бит(rowБитПорта.name)
    '                crБит.keyБитПорта = rowБитПорта.keyБитПорта
    '                crБит.KeyПорта = rowБитПорта.KeyПорта
    '                crБит.НомерБита = rowБитПорта.НомерБита
    '                crListБит.Add(crБит)
    '            Next
    '            'затем присвоить
    '            mВсеКонфигурации.ItemКонфигурация.ItemДействие.ItemПорт.ListБит = crListБит
    '        End If
    '    End Try
    'End Sub

    'Public treeRoot As TreeNodeBase

    'Public Sub ИммитаторCreateTree()
    '    'Dim tree As TreeNodeBase = CreateTree() 'CreateTree возвращает RootNode содержащий все другие узлы

    '    ''Теперь для того, чтобы обеспечить работу алгоритма по объектной структуре, достаточно реализовать интерфейс IVisitor.
    '    ''Этот прием позволяет устранить ошибки, связанные с вызовом неверного метода в методе Accept.

    '    ''PrintFuncVisitor.Print(tree, Console.Out) 'в общий метод PrintFuncVisitor.Print подается RootNode и writer
    '    'PrintContextVisitor.Print(tree, Console.Out) '3 способ
    '    'For Each node As TreeNodeBase In mВсеКонфигурации.DictionaryВсеКонфигурации("Конфигурация_1").DictionaryДействие.Values
    '    '    Console.WriteLine(node.Name)
    '    'Next

    '    'treeRoot = CreateTree() 'CreateTree возвращает RootNode содержащий все другие узлы
    '    CreateTree()
    'End Sub


    'Private Function НовоеДействие() As Действие()
    '    'Public Sub New()
    '    '    MyBase.New("Новое действие")


    '    Dim mДействие_1 As Действие
    '    Dim arrДействие() As Действие

    '    Dim mТриггерСрабатыванияЦифровогоВыхода_1 As ТриггерСрабатыванияЦифровогоВыхода
    '    Dim arrТриггерСрабатыванияЦифровогоВыхода() As ТриггерСрабатыванияЦифровогоВыхода


    '    Dim mФормулаСрабатыванияЦифровогоВыхода_1 As ФормулаСрабатыванияЦифровогоВыхода
    '    Dim arrФормулаСрабатыванияЦифровогоВыхода() As ФормулаСрабатыванияЦифровогоВыхода

    '    Dim mПорт As Порт
    '    Dim arrПорт() As Порт

    '    Dim mАргументДляФормулы_1 As АргументДляФормулы
    '    Dim mАргументДляФормулы_2 As АргументДляФормулы
    '    Dim arrАргументДляФормулы() As АргументДляФормулы

    '    Dim mБит_1 As Бит
    '    Dim mБит_2 As Бит
    '    Dim arrБит() As Бит



    '    mБит_1 = New Бит("Бит_1")
    '    mБит_2 = New Бит("Бит_2")
    '    arrБит = New Бит() {mБит_1, mБит_2}

    '    mАргументДляФормулы_1 = New АргументДляФормулы("АргументДляФормулы_1")
    '    mАргументДляФормулы_2 = New АргументДляФормулы("АргументДляФормулы_2")
    '    arrАргументДляФормулы = New АргументДляФормулы() {mАргументДляФормулы_1, mАргументДляФормулы_2}


    '    mТриггерСрабатыванияЦифровогоВыхода_1 = New ТриггерСрабатыванияЦифровогоВыхода("ТриггерСрабатыванияЦифровогоВыхода_1")
    '    mФормулаСрабатыванияЦифровогоВыхода_1 = New ФормулаСрабатыванияЦифровогоВыхода("ФормулаСрабатыванияЦифровогоВыхода_1", arrАргументДляФормулы)
    '    mПорт = New Порт("Порт_1", arrБит)

    '    Dim mТриггерСрабатыванияЦифровогоВыхода_2 = New ТриггерСрабатыванияЦифровогоВыхода("ТриггерСрабатыванияЦифровогоВыхода_2")
    '    Dim mФормулаСрабатыванияЦифровогоВыхода_2 = New ФормулаСрабатыванияЦифровогоВыхода("ФормулаСрабатыванияЦифровогоВыхода_2", arrАргументДляФормулы)
    '    Dim mПорт_2 = New Порт("Порт_2", arrБит)


    '    arrТриггерСрабатыванияЦифровогоВыхода = New ТриггерСрабатыванияЦифровогоВыхода() {mТриггерСрабатыванияЦифровогоВыхода_1, mТриггерСрабатыванияЦифровогоВыхода_2}
    '    arrФормулаСрабатыванияЦифровогоВыхода = New ФормулаСрабатыванияЦифровогоВыхода() {mФормулаСрабатыванияЦифровогоВыхода_1, mФормулаСрабатыванияЦифровогоВыхода_2}
    '    arrПорт = New Порт() {mПорт, mПорт_2}


    '    mДействие_1 = New Действие("Действие_1", Nothing, arrФормулаСрабатыванияЦифровогоВыхода, arrПорт)
    '    arrДействие = New Действие() {mДействие_1}
    '    'arrДействие = New Действие() {Nothing}

    '    'Dim strName As String = "Новое действие"
    '    'Me.New(strName, arrДействие)
    '    Return arrДействие
    'End Function


#Region "DynamicObject"
    '    'В одном из проектов мне необходимо было реализовать редактор свойств объекта. Загвоздка заключалась в том 
    '    'что в проекте не используются средства ORM, и ко мне данные, которые надо было отредактировать и отослать обратно, 
    '    'попадали просто как двумерный массив object[,]. Ужас! То есть бизнес-классы отсутствовали вообще, 
    '    'и данные просто загружались из базы данных, изменялись и снова сохранялись в БД. 

    '    'Известным редактором свойств объекта является PropertyGrid. Данный элемент управления используется практически повсеместно. 
    '    'Он просто очень удобен и предоставляет интуитивно понятный интерфейс для редактирования различных типов данных (int, string, double, массивов и коллекций). 
    '    'Он, конечно, предоставляет огромные возможности для расширения функциональности и редактирования сложных типов данных.

    '    'Но проблема была в том что PropertyGrid может редактировать объекты с уже существующими свойствами. 
    '    'В моем же случае все данные хранятся в БД, и не существует соответствующих бизнес-классов, куда отображаются данные из БД. 
    '    'Следовательно, на первый взгляд, использовать PropertyGrid (со всеми его встроенными редакторами простых типов, массивов, 
    '    'коллекций и автоматической проверкой корректности введенного значения) не получится. Но это только на первый взгляд...

    '    'Реализация
    '    'После некоторых изысканий элегантное решение было найдено. Использовать PropertyGrid все-таки можно. 
    '    'Просто необходимо создать некий класс DynamicObject, реализующий интерфейс ICustomTypeDescriptor: 
    '    'В этом классе нужно реализовать методы, которые предоставляют динамическую информацию об объекте и, в частности, о его свойствах. 
    '    'Вся работа будет сконцентрирована в основном вокруг реализации одного перегруженного метода – GetProperties. 
    '    'Данный метод будет вызван элементом управления PropertyGrid, когда вы присвоите экземпляр класса свойству SelectedObject:

    '    'var dynamicObject = new DynamicObject();
    '    'propertyGrid1.SelectedObject = dynamicObject;


    '    Public Class DynamicObject
    '        Implements ICustomTypeDescriptor

    '        Private _Filter As String = [String].Empty
    '        Private _FilteredPropertyDescriptors As New PropertyDescriptorCollection(Nothing)
    '        Private _FullPropertyDescriptors As New PropertyDescriptorCollection(Nothing)
    '        'По умолчанию коллекция объектов PropertyDescriptor пуста, и необходимо реализовать методы для изменения её содержимого. 
    '        'Для корректного отображения свойства в PropertyGrid и для инициализации класса GenericPropertyDescriptor<T> 
    '        'реализуем метод (назовем его AddProperty<T>), добавляющий новое свойство. Метод должен принимать имя свойства, 
    '        'его текущее значение, описание свойства для отображения в PropertyGrid, имя категории (если мы хотим, чтобы свойства в PropertyGrid были разбиты по категориям), 
    '        'флаг readOnly для указания, должно ли свойство быть доступным только для чтения, и массив атрибутов, на случай, 
    '        'если нужно пометить данное свойство дополнительными атрибутами. 

    '        'ПРИМЕЧАНИЕ
    '        'Дополнительные атрибуты могут понадобиться, например, для указания PropertyGrid специфического редактора или специального конвертора типа данных.
    '        'Реализация перегруженного метода AddProperty<T> представлена ниже:

    '        Public Sub AddProperty(Of T)(ByVal name As String, ByVal value As T, ByVal displayName As String, ByVal description As String, ByVal category As String, ByVal [readOnly] As Boolean, _
    '         ByVal attributes As IEnumerable(Of Attribute))

    '            'Dim attrs As var = IIf(attributes Is Nothing, New List(Of Attribute)(), New List(Of Attribute)(attributes))
    '            'Dim attrs = IIf(attributes Is Nothing, New List(Of Attribute)(), New List(Of Attribute)(attributes))
    '            Dim attrs
    '            If attributes Is Nothing Then 'пустой массив
    '                attrs = New List(Of Attribute)()
    '            Else
    '                attrs = New List(Of Attribute)(attributes)
    '            End If
    '            'проверка на отсутствие:
    '            If Not [String].IsNullOrEmpty(displayName) Then
    '                attrs.Add(New DisplayNameAttribute(displayName))
    '            End If

    '            If Not [String].IsNullOrEmpty(description) Then
    '                attrs.Add(New DescriptionAttribute(description))
    '            End If

    '            If Not [String].IsNullOrEmpty(category) Then
    '                attrs.Add(New CategoryAttribute(category))
    '            End If

    '            If [readOnly] Then
    '                attrs.Add(New ReadOnlyAttribute(True))
    '            End If

    '            _FullPropertyDescriptors.Add(New GenericPropertyDescriptor(Of T)(name, value, attrs.ToArray()))
    '        End Sub
    '        'перегруженный вариант без дополнительного массива атрибутов
    '        Public Sub AddProperty(Of T)(ByVal name As String, ByVal value As T, ByVal description As String, ByVal category As String, ByVal [readOnly] As Boolean)
    '            AddProperty(Of T)(name, value, name, description, category, [readOnly], _
    '             Nothing)
    '        End Sub

    '        'Также можно предусмотреть метод удаления свойств из коллекции свойств объекта. Для этого служит метод RemoveProperty:
    '        Public Sub RemoveProperty(ByVal propertyName As String)
    '            'Dim descriptor As var = _FullPropertyDescriptors.Find(propertyName, True)
    '            Dim descriptor = _FullPropertyDescriptors.Find(propertyName, True)
    '            If descriptor IsNot Nothing Then
    '                _FullPropertyDescriptors.Remove(descriptor)
    '            Else
    '                Throw New Exception("Property is not found")
    '            End If
    '        End Sub

    '        Public Property Filter() As String
    '            Get
    '                Return _Filter
    '            End Get
    '            Set(ByVal value As String)
    '                _Filter = value.Trim().ToLower()
    '                If _Filter.Length <> 0 Then
    '                    FilterProperties(_Filter)
    '                End If
    '            End Set
    '        End Property

    '        Private Sub FilterProperties(ByVal filter As String)
    '            _FilteredPropertyDescriptors.Clear()

    '            For Each descriptor As PropertyDescriptor In _FullPropertyDescriptors
    '                If descriptor.Name.ToLower().IndexOf(filter) > -1 Then
    '                    _FilteredPropertyDescriptors.Add(descriptor)
    '                End If
    '            Next
    '        End Sub

    '        'Ну и для удобства работы с классом DynamicObject добавим indexer, чтобы можно было обращаться к свойствам как к элементам коллекции:
    '        Default Public Property Item(ByVal propertyName As String) As Object
    '            Get
    '                Return GetPropertyValue(propertyName)
    '            End Get
    '            Set(ByVal value As Object)
    '                SetPropertyValue(propertyName, value)
    '            End Set
    '        End Property

    '        'Кроме того, необходима возможность прочитать/изменить значение того или иного свойства извне элемента управления PropertyGrid. 
    '        'Для этого у класса DynamicObject нужно реализовать методы GetPropertyValue и SetPropertyValue:
    '        Private Function GetPropertyValue(ByVal propertyName As String) As Object
    '            'Dim descriptor As var = _FullPropertyDescriptors.Find(propertyName, True)
    '            Dim descriptor = _FullPropertyDescriptors.Find(propertyName, True)
    '            If descriptor IsNot Nothing Then
    '                Return descriptor.GetValue(Nothing)
    '            Else
    '                Throw New Exception("Property is not found")
    '            End If
    '        End Function

    '        Private Sub SetPropertyValue(ByVal propertyName As String, ByVal value As Object)
    '            'Dim descriptor As var = _FullPropertyDescriptors.Find(propertyName, True)
    '            Dim descriptor = _FullPropertyDescriptors.Find(propertyName, True)
    '            If descriptor IsNot Nothing Then
    '                descriptor.SetValue(Nothing, value)
    '            Else
    '                Throw New Exception("Property is not found")
    '            End If
    '        End Sub



    '#Region "Implementation of ICustomTypeDescriptor"

    '        Public Function GetConverter() As System.ComponentModel.TypeConverter Implements System.ComponentModel.ICustomTypeDescriptor.GetConverter
    '            Return TypeDescriptor.GetConverter(Me, True)
    '        End Function

    '        Public Function GetEvents(ByVal attributes As System.Attribute()) As System.ComponentModel.EventDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetEvents
    '            Return TypeDescriptor.GetEvents(Me, attributes, True)
    '        End Function

    '        Public Function GetEvents() As System.ComponentModel.EventDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetEvents
    '            Return TypeDescriptor.GetEvents(Me, True)
    '        End Function

    '        Public Function GetComponentName() As String Implements System.ComponentModel.ICustomTypeDescriptor.GetComponentName
    '            Return TypeDescriptor.GetComponentName(Me, True)
    '        End Function

    '        Public Function GetPropertyOwner(ByVal pd As System.ComponentModel.PropertyDescriptor) As Object Implements System.ComponentModel.ICustomTypeDescriptor.GetPropertyOwner
    '            Return Me
    '        End Function

    '        Public Function GetAttributes() As System.ComponentModel.AttributeCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetAttributes
    '            Return TypeDescriptor.GetAttributes(Me, True)
    '        End Function


    '        'PropertyGrid вызовет метод GetProperties и передаст туда массив атрибутов Attribute[] с одним элементом BrowsableAttribute. 
    '        'То есть PropertyGrid запрашивает у объекта список свойств объекта, которые можно отобразить.
    '        'Метод GetProperties возвращает PropertyDescriptorCollection – коллекцию объектов PropertyDescriptor. 
    '        'PropertyDescriptor – это абстрактный класс, описывающий свойство объекта. В данном случае необходимо реализовать собственный класс-наследник PropertyDescriptor, 
    '        'который будет использоваться элементом управления PropertyGrid для получения информации о конкретном свойстве объекта. 
    '        'смотри реализацию
    '        'метод GetProperties возвращает PropertyDescriptorCollection – коллекцию объектов PropertyDescriptor. 
    '        'Соответственно, в классе DynamicObject нужно где-то хранить описатели свойств. Для этого создадим private-поле типа PropertyDescriptorCollection. 

    '        'private PropertyDescriptorCollection propertyDescriptors = new PropertyDescriptorCollection(null);



    '        Public Function GetProperties(ByVal attributes As System.Attribute()) As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetProperties
    '            'в зависимости применили фильтр или нет, выдается та, или иная коллекция
    '            If _Filter.Length <> 0 Then
    '                Return _FilteredPropertyDescriptors
    '            Else
    '                Return _FullPropertyDescriptors
    '            End If
    '        End Function

    '        Public Function GetProperties() As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ICustomTypeDescriptor.GetProperties
    '            Return GetProperties(New Attribute(-1) {})
    '            'Dim arrInt As Integer()
    '            'arrInt = New Integer(-1) {}
    '        End Function

    '        Public Function GetEditor(ByVal editorBaseType As System.Type) As Object Implements System.ComponentModel.ICustomTypeDescriptor.GetEditor
    '            Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
    '        End Function

    '        Public Function GetDefaultProperty() As System.ComponentModel.PropertyDescriptor Implements System.ComponentModel.ICustomTypeDescriptor.GetDefaultProperty
    '            Return TypeDescriptor.GetDefaultProperty(Me, True)
    '        End Function

    '        Public Function GetDefaultEvent() As System.ComponentModel.EventDescriptor Implements System.ComponentModel.ICustomTypeDescriptor.GetDefaultEvent
    '            Return TypeDescriptor.GetDefaultEvent(Me, True)
    '        End Function

    '        Public Function GetClassName() As String Implements System.ComponentModel.ICustomTypeDescriptor.GetClassName
    '            Return TypeDescriptor.GetClassName(Me, True)
    '        End Function

    '#End Region
    '    End Class


    '    'Так как тип свойства может быть любым типом из .Net Framework, то я решил реализовать generic-класс и назвал его 
    '    'просто – GenericPropertyDescriptor<T>, где T – тип свойства.

    '    Public Class GenericPropertyDescriptor(Of T)
    '        Inherits PropertyDescriptor
    '        Private _value As T

    '        Public Sub New(ByVal name As String, ByVal attrs As Attribute())
    '            MyBase.New(name, attrs)
    '        End Sub

    '        Public Sub New(ByVal name As String, ByVal value As T, ByVal attrs As Attribute())
    '            MyBase.New(name, attrs)
    '            _value = value
    '        End Sub

    '        Public Overloads Overrides Function CanResetValue(ByVal component As Object) As Boolean
    '            Return False
    '        End Function

    '        Public Overloads Overrides ReadOnly Property ComponentType() As System.Type
    '            Get
    '                Return GetType(GenericPropertyDescriptor(Of T))
    '            End Get
    '        End Property

    '        Public Overloads Overrides Function GetValue(ByVal component As Object) As Object
    '            Return _value
    '        End Function

    '        Public Overloads Overrides ReadOnly Property IsReadOnly() As Boolean
    '            Get
    '                Return Array.Exists(Me.AttributeArray, Function(attr) attr Is ReadOnlyAttribute.Default)
    '                'Return Array.Exists(Me.AttributeArray, Function(attr) attr.GetType Is ReadOnlyAttribute.Yes)
    '                'Return Array.Exists(Me.AttributeArray, Function(attr) attr = System.ComponentModel.ReadOnlyAttribute)
    '                '                return Array.Exists(this.AttributeArray, attr => attr is ReadOnlyAttribute);
    '                'CType(attr, ParamArrayAttribute).g()
    '            End Get
    '        End Property

    '        Public Overloads Overrides ReadOnly Property PropertyType() As System.Type
    '            Get
    '                Return GetType(T)
    '            End Get
    '        End Property

    '        Public Overloads Overrides Sub ResetValue(ByVal component As Object)
    '        End Sub

    '        Public Overloads Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
    '            _value = DirectCast(value, T)
    '        End Sub

    '        Public Overloads Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
    '            Return False
    '        End Function
    '    End Class


    '    'public class GenericPropertyDescriptor<T> : PropertyDescriptor
    '    '{
    '    '    private T m_value;

    '    '    public GenericPropertyDescriptor(string name, Attribute[] attrs)
    '    '        : base(name, attrs)
    '    '    {
    '    '    }

    '    '    public GenericPropertyDescriptor(string name, T value, Attribute[] attrs)
    '    '        : base(name, attrs)
    '    '    {
    '    '        this.m_value = value;
    '    '    }

    '    '    public override bool CanResetValue(object component)
    '    '    {
    '    '        return false;
    '    '    }

    '    '    public override System.Type ComponentType
    '    '    {
    '    '        get
    '    '        {
    '    '            return typeof(GenericPropertyDescriptor<T>);
    '    '        }
    '    '    }

    '    '    public override object GetValue(object component)
    '    '    {
    '    '        return this.m_value;
    '    '    }

    '    '    public override bool IsReadOnly
    '    '    {
    '    '        get
    '    '        {
    '    '            foreach (Attribute attribute in this.AttributeArray)
    '    '            {
    '    '                if (attribute is ReadOnlyAttribute)
    '    '                {
    '    '                    return true;
    '    '                }
    '    '            }
    '    '            return false;
    '    '        }
    '    '    }

    '    '    public override System.Type PropertyType
    '    '    {
    '    '        get
    '    '        {
    '    '            return typeof(T);
    '    '        }
    '    '    }

    '    '    public override void ResetValue(object component)
    '    '    {
    '    '    }

    '    '    public override void SetValue(object component, object value)
    '    '    {
    '    '        this.m_value = (T)value;
    '    '    }

    '    '    public override bool ShouldSerializeValue(object component)
    '    '    {
    '    '        return false;
    '    '    }
    '    '}
#End Region

End Module