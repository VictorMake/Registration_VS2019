Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml.Linq
Imports MathematicalLibrary
Imports NationalInstruments.Analysis.Math
Imports NationalInstruments.Analysis.Math.CurveFit
Imports VB = Microsoft.VisualBasic

Friend Class FormConditionFind
    Friend Property ParentFormSnapshot() As FormSnapshotViewingDiagram

    Private mFrequencySnapshot As Integer
    Friend ReadOnly Property FrequencySnapshot() As Integer
        Get
            Return mFrequencySnapshot
        End Get
    End Property

    Private mCurrentPosition As Integer
    Friend ReadOnly Property CurrentPosition() As Integer
        Get
            Return mCurrentPosition
        End Get
    End Property

#Region "Enum"
    ''' <summary>
    ''' Тип вкладки условия
    ''' </summary>
    Public Enum Marker
        ''' <summary>
        ''' Триггер
        ''' </summary>
        Trigger = 0
        ''' <summary>
        ''' Шаблон
        ''' </summary>
        Template = 1
    End Enum

    ''' <summary>
    ''' Тип логического объединения условия
    ''' </summary>
    Public Enum ConditionType
        ''' <summary>
        ''' Обязательное
        ''' </summary>
        Mandatory = 0
        ''' <summary>
        ''' Обязательно, если выполнено. Обязательное при анализе.
        ''' </summary>
        MinMax = 1
        ''' <summary>
        ''' Шаблон если выполнено. Обязательное и на данном снимке еще ни разу не было найдено.
        ''' </summary>
        Template = 2 '
    End Enum

    ''' <summary>
    ''' Условие Поиска
    ''' </summary>
    Private Enum ConditionFind
        MinMaxTemplate = 0
        Pattern = 1
        MinMax = 2
    End Enum
    Private memoConditionFind As ConditionFind

    ''' <summary>
    ''' Перечислитель математического выражения сравнения
    ''' </summary>
    Public Enum SelectiveCondition
        ''' <summary>
        ''' Между
        ''' </summary>
        Between = 0
        ''' <summary>
        ''' Вне
        ''' </summary>
        OutOfRange = 1
        ''' <summary>
        ''' Равно
        ''' </summary>
        Equal = 2
        ''' <summary>
        ''' НеРавно
        ''' </summary>
        NotEqual = 3
        ''' <summary>
        ''' Больше
        ''' </summary>
        Greater = 4
        ''' <summary>
        ''' Меньше
        ''' </summary>
        Less = 5
        ''' <summary>
        ''' Больше Или Равно
        ''' </summary>
        GreaterThanOrEqual = 6
        ''' <summary>
        ''' Меньше Или Равно
        ''' </summary>
        LessThanOrEqual = 7
        ''' <summary>
        ''' Найти Максимум
        ''' </summary>
        FindMaximum = 8
        ''' <summary>
        ''' Найти Минимум
        ''' </summary>
        FindMinimum = 9
    End Enum

    ''' <summary>
    ''' Иконка вкладки
    ''' </summary>
    Private Enum PageImage
        Filter0 = 0
        Question1 = 1
        Trigger2 = 2
        Template3 = 3
    End Enum

    ''' <summary>
    ''' Иконка узла снимков
    ''' </summary>
    Private Enum NodeImage
        Close0 = 0
        Open1 = 1
        GraphSnapshot2 = 2
        GraphRegistration3 = 3
        Apply4 = 4
    End Enum

    ''' <summary>
    ''' Иконка узла условий
    ''' </summary>
    Private Enum XmlImage
        ''' <summary>
        ''' Выделен
        ''' </summary>
        Select0 = 0
        ''' <summary>
        ''' Имеющиеся условия
        ''' </summary>
        Root1 = 1
        ''' <summary>
        ''' ЧтоЗаУсловие
        ''' </summary>
        Conditions2 = 2
        ''' <summary>
        ''' ДочернееУсловиеN
        ''' </summary>
        Term3 = 3
        ''' <summary>
        ''' "СтартВерхнее"
        ''' </summary>
        StartUp4 = 4
        ''' <summary>
        ''' "СтартНижнее"
        ''' </summary>
        StartDown5 = 5
        ''' <summary>
        ''' "ПерегибВерхнее"
        ''' </summary>
        BendUp6 = 6
        ''' <summary>
        ''' "ПерегибНижнее"
        ''' </summary>
        BendDown7 = 7
        ''' <summary>
        ''' "ФинишВерхнее"
        ''' </summary>
        FinishUp8 = 8
        ''' <summary>
        ''' "ФинишНижнее"
        ''' </summary>
        FinishDown9 = 9
        ''' <summary>
        ''' "ВремяПерегиба"
        ''' </summary>
        TimeBend10 = 10
        ''' <summary>
        ''' "ШиринаВременногоОкна"
        ''' </summary>
        WidthWindow11 = 11
        ''' <summary>
        ''' "ВерхнееЗначениеУсловия"
        ''' </summary>
        BetweenUpTrigger12 = 12
        ''' <summary>
        ''' "НижнееЗначениеУсловия"
        ''' </summary>
        BetweenDownTrigger13 = 13
        ''' <summary>
        ''' "МаксимальныйПределПараметра"
        ''' </summary>
        MaxLimit14 = 14
        ''' <summary>
        ''' "ИмяФормаПараметр"
        ''' </summary>
        ParameterTemplate15 = 15
        ''' <summary>
        ''' "УсловиеОтбора"
        ''' </summary>
        ConditionalTrigger16 = 16
        ''' <summary>
        '''  "ЗначениеУсловия"
        ''' </summary>
        EqualTrigger17 = 17
        ''' <summary>
        ''' "ИмяЗначениеПараметр"
        ''' </summary>
        ParameterTrigger18 = 18
    End Enum
#End Region

    Public Structure TypeNameUnit
        ''' <summary>
        ''' наименование Параметра
        ''' </summary>
        Dim NameParameter As String
        ''' <summary>
        ''' единица Измерения
        ''' </summary>
        Dim UnitMeasure As String
    End Structure
    Private arrTypeNameUnit As TypeNameUnit()

#Region "String Const"
    'Me.ComboBoxEnumConditions.Items.AddRange(New Object() {"между", "вне", "равно", "не равно", "больше", "меньше", "больше или равно", "меньше или равно", "найти максимум", "найти минимум"})
    Private Const cBetween As String = "Между"
    Private Const cOutOfRange As String = "Вне"
    Private Const cEqual As String = "Равно"
    Private Const cNotEqual As String = "НеРавно"
    Private Const cGreater As String = "Больше"
    Private Const cLess As String = "Меньше"
    Private Const cGreaterThanOrEqual As String = "БольшеИлиРавно"
    Private Const cLessThanOrEqual As String = "МеньшеИлиРавно"
    Private Const cFindMaximum As String = "НайтиМаксимум"
    Private Const cFindMinimum As String = "НайтиМинимум"

    Public Const cTrigger As String = "Триггер"
    Public Const cTriggerValue As String = "Значение"
    Public Const cTemplate As String = "Шаблон"
    Public Const cTermCondition As String = "УсловияПоиска"
    Public Const cWhatIsCondition As String = "ЧтоЗаУсловие"
    Public Const cChildConditionN As String = "ДочернееУсловиеN"
    Public Const cParameterTrigger As String = "ИмяЗначениеПараметр"
    Public Const cConditionalTrigger As String = "УсловиеОтбора"
    Public Const cEqualTrigger As String = "ЗначениеУсловия"
    Public Const cBetweenDownTrigger As String = "НижнееЗначениеУсловия"
    Public Const cBetweenUpTrigger As String = "ВерхнееЗначениеУсловия"
    Public Const cParameterTemplate As String = "ИмяФормаПараметр"
    Public Const cMaxLimit As String = "МаксимальныйПределПараметра"
    Public Const cStartUp As String = "СтартВерхнее"
    Public Const cStartDown As String = "СтартНижнее"
    Public Const cBendUp As String = "ПерегибВерхнее"
    Public Const cBendDown As String = "ПерегибНижнее"
    Public Const cFinishUp As String = "ФинишВерхнее"
    Public Const cFinishDown As String = "ФинишНижнее"
    Public Const cTimeBend As String = "ВремяПерегиба"
    Public Const cWidthWindow As String = "ШиринаВременногоОкна"

    Public Const cCondition As String = "Условие"
#End Region

    ''' <summary>
    ''' Диапазон по оси Х и число замеров
    ''' </summary>
    Protected Const arraysize As Integer = 1800
    Private isFormLoaded As Boolean
    ''' <summary>
    ''' Тип Условия Обязательное
    ''' </summary>
    Private isConditionMandatory As Boolean
    Private isConditionMinMax As Boolean
    Private isConditionPattern As Boolean
    ''' <summary>
    ''' Продолжение условия на втором кадре
    ''' </summary>
    Private isContinuationNextFrame As Boolean

    ''' <summary>
    ''' Максимальное число условий и соответственно вкладок
    ''' </summary>
    Private Const maxItemTabPages As Integer = 20
    Private pagesCount As Integer = 1
    Private indexPreviousPage As Integer
    Private indexCurrentPage As Integer
    Private numberEngine As Integer
    Private frequencyBuf1, frequencyBuf2 As Integer
    Private countRowsBuf1 As Integer

    Private TabConditionSelect As TabControl
    Private PageConditionSelect As TabPage
    Private PageSelected As TabPage
    Private Conditions As ConditionCollection
    Private FramesOk As New List(Of FrameCondition)
    Private XDoc As XDocument
    Private measureFromFile(1, 1), Buf(1, 1), Buf1(1, 1), Buf2(1, 1) As Double
    Private parameterNames, parameterNamesConfiguration As String()
    ''' <summary>
    ''' Индексы Ненайденных Параметров
    ''' </summary>
    Private IndexesAbsentParameters As Integer()
    Private backgroundCalculator As BackgroundWorker

    Private dataRowBuf1, dataRowBuf2 As DataRow
    Private engineSnapshotDataTable As DataTable
    Private foundSnapshotDataTable As DataTable

    Public Sub New(ByVal parent As FormSnapshotViewingDiagram)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        ParentFormSnapshot = parent
        Tag = "Поиск кадров по условиям"
        CollectionForms.Add(Text, Me)
        Conditions = New ConditionCollection(Me)
    End Sub

    Private Sub FormConditionFind_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ParentFormSnapshot.IsLoadedFormConditionFind = True
        'ReDim_IndexesAbsentParameters(1)
        Re.Dim(IndexesAbsentParameters, 1)

        ComboBoxEnumConditions.SelectedIndex = 0
        NumericValueMax.Value = 100.0
        SlideValueStartUp.Value = 100.0
        SlideValueStartDown.Value = SlideValueStartUp.Value / 1.1

        SlideValueFinishUp.Value = 100.0
        SlideValueFinishDown.Value = SlideValueFinishUp.Value / 1.1

        SlideValueMiddleUp.Value = 100.0
        SlideValueMiddleDown.Value = SlideValueMiddleUp.Value / 1.1

        PageConditionSelect = TabControlAll.SelectedTab
        TabConditionSelect = CType(PageConditionSelect.Controls(0), TabControl)
        PageSelected = TabConditionSelect.SelectedTab

        PopulatePages()
        CreateNewConditionOnDefault()
        indexPreviousPage = 0

        SplitContainerStep3.Dock = DockStyle.Fill
        SplitContainerStep3.BringToFront()
        SplitContainerEngine.Dock = DockStyle.Fill

        Step1()
        SetWidhtColumnsListViewAllSnapshot()
        PopulateTreeViewEngine()
        LoadConditionsXmlDoc()

        isFormLoaded = True
        FormResize()

        ' Подготовить рабочий фоновый поток для асинхронной задачи
        ' Специфицировать что рабочий фоновый поток предоставляет уведомление для прогресса
        ' Специфицировать что рабочий фоновый поток поддерживает прерывание
        backgroundCalculator = New BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}
        ' DoWork обработчик события есть основная рабочая функция фонового потока выполняющая длительную операцию
        AddHandler backgroundCalculator.DoWork, New DoWorkEventHandler(AddressOf BackgroundCalculator_DoWork)
        ' Назначить функцию для уведомления прогресса выполнения
        AddHandler backgroundCalculator.ProgressChanged, New ProgressChangedEventHandler(AddressOf BackgroundCalculator_ProgressChanged)
        ' Назначить функцию запускаемую когда фоновая работа завершена
        ' Три условия обрабатываемые в этой функции по результатам:
        ' 1. Работа завершена успешно
        ' 2. Работа прервана с ошибками
        ' 3. Пользователь прервал процесс
        AddHandler backgroundCalculator.RunWorkerCompleted, New RunWorkerCompletedEventHandler(AddressOf BackgroundCalculator_RunWorkerCompleted)
    End Sub

#Region "PageCondition"
    Private Sub FormConditionFind_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormSnapshot.IsLoadedFormConditionFind = False
        ParentFormSnapshot = Nothing
        CollectionForms.Remove(Text)
    End Sub

    ''' <summary>
    ''' Создать Новое Условие По Умолчанию
    ''' </summary>
    Private Sub CreateNewConditionOnDefault()
        ' создать и извлечь
        Dim newCondition As Condition = Conditions.Add(pagesCount)

        With newCondition
            If PageSelected.Tag.ToString = cTrigger Then
                .MarkerProperty = Marker.Trigger
                .ConditionTypeProperty = If(ComboBoxEnumConditions.SelectedIndex = SelectiveCondition.FindMinimum OrElse ComboBoxEnumConditions.SelectedIndex = SelectiveCondition.FindMaximum,
                ConditionType.MinMax,
                ConditionType.Mandatory)
            Else
                .MarkerProperty = Marker.Template
                .ConditionTypeProperty = ConditionType.Template
            End If
        End With
    End Sub

    ''' <summary>
    ''' Запомнить Условие При Смене Закладки при SelectedIndexChanged
    ''' </summary>
    ''' <param name="indexPage"></param>
    Private Sub StorageConditionOnPageChanged(ByVal indexPage As Integer)
        ' извлечь и запомнить
        Dim tempCondition As Condition = Conditions(indexPage + 1)
        ' PageValueSelect еще не сменена и с нее можно взять данные

        With tempCondition
            If PageSelected.Tag.ToString = cTrigger Then
                .MarkerProperty = Marker.Trigger
                .ConditionTypeProperty = If(ComboBoxEnumConditions.SelectedIndex = SelectiveCondition.FindMinimum OrElse ComboBoxEnumConditions.SelectedIndex = SelectiveCondition.FindMaximum,
                    ConditionType.MinMax,
                    ConditionType.Mandatory)
            Else
                .MarkerProperty = Marker.Template
                .ConditionTypeProperty = ConditionType.Template
            End If

            ' Триггер
            .NameValueParameter = ComboBoxValueParameters.Text
            .SelectiveConditionProperty = CType(ComboBoxEnumConditions.SelectedIndex, SelectiveCondition)
            .LowerValueCondition = Val(TextBoxStart.Text)
            .UpperValueCondition = Val(TextBoxEnd.Text)
            .Condition = Val(TextBoxValue.Text)

            ' Шаблон
            .NameFormParameter = ComboBoxPatternParameters.Text
            .MaxLimitParameter = NumericValueMax.Value
            .StartUpper = NumericSlideValueStartUp.Value
            .StartLover = NumericSlideValueStartDown.Value
            .TwistTop = NumericSlideValueMiddleUp.Value
            .TwistLower = NumericSlideValueMiddleDown.Value
            .FinishTop = NumericSlideValueFinishUp.Value
            .FinishLower = NumericSlideValueFinishDown.Value
            .TimeTwist = NumericTime.Value
            .WidthTemporaryWindow = NumericTimeWindow.Value
        End With
    End Sub

    ''' <summary>
    ''' Восстановить Условие При Смене Закладки
    ''' </summary>
    ''' <param name="indexPage"></param>
    Private Sub RestoreConditionOnPageChanged(ByVal indexPage As Integer)
        ' извлечь и запомнить
        Dim tempCondition As Condition = Conditions(indexPage + 1)

        With tempCondition
            TabConditionSelect.SelectedIndex = If(.MarkerProperty = Marker.Trigger, 0, 1)

            ' Триггер
            ComboBoxValueParameters.Text = .NameValueParameter
            ComboBoxEnumConditions.SelectedIndex = .SelectiveConditionProperty
            TextBoxStart.Text = .LowerValueCondition.ToString
            TextBoxEnd.Text = .UpperValueCondition.ToString
            TextBoxValue.Text = .Condition.ToString
            ' Шаблон
            ComboBoxPatternParameters.Text = .NameFormParameter
            NumericValueMax.Value = .MaxLimitParameter
            NumericSlideValueStartUp.Value = .StartUpper
            NumericSlideValueStartDown.Value = .StartLover
            NumericSlideValueMiddleUp.Value = .TwistTop
            NumericSlideValueMiddleDown.Value = .TwistLower
            NumericSlideValueFinishUp.Value = .FinishTop
            NumericSlideValueFinishDown.Value = .FinishLower
            NumericTimeWindow.Value = .WidthTemporaryWindow
            NumericTime.Value = .TimeTwist
        End With
    End Sub

    Private Sub ComboBoxEnumConditions_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxEnumConditions.SelectedIndexChanged
        OnComboBoxEnumConditionsSelectedIndexChanged(CType(sender, ComboBox))
    End Sub

    Private Sub OnComboBoxEnumConditionsSelectedIndexChanged(ByRef sender As ComboBox)
        'Select Case sender.Tag.ToString
        '    Case "EnumCondition"
        Select Case (sender.SelectedIndex)
            Case SelectiveCondition.Between, SelectiveCondition.OutOfRange
                TextBoxValue.Visible = False
                TextBoxStart.Visible = True
                LabelAnd.Visible = True
                TextBoxEnd.Visible = True
                Exit Select
            Case SelectiveCondition.FindMaximum, SelectiveCondition.FindMinimum
                TextBoxValue.Visible = False
                TextBoxStart.Visible = False
                LabelAnd.Visible = False
                TextBoxEnd.Visible = False
                Exit Select
            Case Else '.Равно .НеРавно .Больше .Меньше .БольшеИлиРавно .МеньшеИлиРавно
                TextBoxValue.Visible = True
                TextBoxStart.Visible = False
                LabelAnd.Visible = False
                TextBoxEnd.Visible = False
                Exit Select
        End Select
    End Sub

    Private Sub NumericValueMax_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NumericValueMax.AfterChangeValue
        SlideValueStartUp.Range = New Range(0, NumericValueMax.Value)
        SlideValueStartDown.Range = New Range(0, NumericValueMax.Value)
        'SlideValueStartUp.Value = NumericValueMax1.Value
        'SlideValueStartDown.Value = SlideValueStartUp1.Value / 1.1

        SlideValueMiddleUp.Range = New Range(0, NumericValueMax.Value)
        SlideValueMiddleDown.Range = New Range(0, NumericValueMax.Value)
        'SlideValueMiddleUp.Value = NumericValueMax1.Value
        'SlideValueMiddleDown.Value = SlideValueMiddleUp1.Value / 1.1

        SlideValueFinishUp.Range = New Range(0, NumericValueMax.Value)
        SlideValueFinishDown.Range = New Range(0, NumericValueMax.Value)
        'SlideValueFinishUp.Value = NumericValueMax1.Value
        'SlideValueFinishDown.Value = SlideValueFinishUp1.Value / 1.1
    End Sub

    Private Sub SlideTime_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideTime.AfterChangeValue
        If SlideTime.Value = SlideTime.Range.Minimum Then
            SlideTime.Value = SlideTime.Range.Minimum + 0.1
            CheckTimeRange()
        End If

        If SlideTime.Value = SlideTime.Range.Maximum Then
            SlideTime.Value = SlideTime.Range.Maximum - 0.1
            CheckTimeRange()
        End If

        CreateTemplate()
    End Sub

    ''' <summary>
    ''' Проверка Времени На Концы
    ''' </summary>
    Private Sub CheckTimeRange()
        If SlideTime.Value = SlideTime.Range.Minimum + 0.1 Then
            SlideValueMiddleUp.Value = SlideValueStartUp.Value
            SlideValueMiddleDown.Value = SlideValueStartDown.Value
        End If

        If SlideTime.Value = SlideTime.Range.Maximum - 0.1 Then
            SlideValueMiddleUp.Value = SlideValueFinishUp.Value
            SlideValueMiddleDown.Value = SlideValueFinishDown.Value
        End If

        CreateTemplate()
    End Sub

    Private Sub SlideTimeWindow_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideTimeWindow.AfterChangeValue
        If SlideTimeWindow.Value < 1 Then Exit Sub

        SlideTime.Range = New Range(0, SlideTimeWindow.Value)
        XAxis.Range = New Range(0, SlideTimeWindow.Value)
        CreateTemplate()

        If SlideTimeWindow.Value < SlideTime.Value Then SlideTime.Value = SlideTimeWindow.Value - 0.1
    End Sub

    ''' <summary>
    ''' Построить Шаблон
    ''' </summary>
    Private Sub CreateTemplate()
        Const points As Integer = 2 ' Количество точек построения линии полинома
        Dim xArray(points) As Double
        Dim yArray(points) As Double
        Dim coeffArray As Double()

        Try
            Dim dataArray(CInt(SlideTimeWindow.Value * 10)) As Double
            ' низ
            xArray(0) = 0
            xArray(1) = SlideTime.Value
            xArray(2) = SlideTimeWindow.Value
            yArray(0) = SlideValueStartUp.Value
            yArray(1) = SlideValueMiddleUp.Value
            yArray(2) = SlideValueFinishUp.Value

            coeffArray = SplineInterpolant(xArray, yArray, 0, 0)
            For I As Integer = 0 To CInt(SlideTimeWindow.Value * 10)
                dataArray(I) = SplineInterpolation(xArray, yArray, coeffArray, I / 10)
            Next

            YAxis.Range = New Range(0, ArrayOperation.GetMax(dataArray))

            PlotUp.PlotY(dataArray, 0, 0.1)

            XYCursorUpLeft.MoveCursor(xArray(0), yArray(0))
            XYCursorUpMiddle.MoveCursor(xArray(1), yArray(1))
            XYCursorUpRight.MoveCursor(xArray(2), yArray(2))

            ' верх
            yArray(0) = SlideValueStartDown.Value
            yArray(1) = SlideValueMiddleDown.Value
            yArray(2) = SlideValueFinishDown.Value

            coeffArray = SplineInterpolant(xArray, yArray, 0, 0)
            For I As Integer = 0 To CInt(SlideTimeWindow.Value * 10)
                dataArray(I) = SplineInterpolation(xArray, yArray, coeffArray, I / 10)
            Next

            PlotDown.PlotY(dataArray, 0, 0.1)

            XYCursorDownLeft.MoveCursor(xArray(0), yArray(0))
            XYCursorDownMiddle.MoveCursor(xArray(1), yArray(1))
            XYCursorDownRight.MoveCursor(xArray(2), yArray(2))

        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub TabControlAll_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlAll.SelectedIndexChanged
        PopulatePages()
    End Sub

    Private Sub PopulatePages()
        If isFormLoaded Then StorageConditionOnPageChanged(indexPreviousPage)

        indexCurrentPage = TabControlAll.SelectedIndex

        For I As Integer = 0 To TabControlAll.TabCount - 1
            TabControlAll.TabPages.Item(I).ImageIndex = PageImage.Filter0
        Next

        TabControlAll.SelectedTab.ImageIndex = PageImage.Question1
        PageConditionSelect = TabControlAll.SelectedTab
        TabConditionSelect = CType(PageConditionSelect.Controls(0), TabControl)
        TabControlCondition_SelectedIndexChanged(TabConditionSelect, New EventArgs)
        SetSizePanels()

        If isFormLoaded Then RestoreConditionOnPageChanged(indexCurrentPage)
        indexPreviousPage = TabControlAll.SelectedIndex
    End Sub

    Private Sub TabControlCondition_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlCondition.SelectedIndexChanged
        TabConditionSelect = CType(sender, TabControl)
        PageSelected = TabConditionSelect.SelectedTab

        If PageSelected.Tag.ToString = cTrigger Then
            PanelTemplate.Visible = False
            PanelSetValue.Visible = True
            PanelSetValue.BringToFront()
        Else
            PanelTemplate.Visible = True
            PanelSetValue.Visible = False
            PanelTemplate.BringToFront()
        End If

        SetSizePanels()
    End Sub

    Private Sub TabControlAll_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlAll.Resize
        If isFormLoaded Then SetSizePanels()
    End Sub

    ''' <summary>
    ''' Установить Размер Форм Условий
    ''' </summary>
    Private Sub SetSizePanels()
        ' 35 вместо PageSelected.Location.X
        Const PageSelectedLocationX As Integer = 35
        PanelTemplate.Size = New Size(PageSelected.Size.Width + PageSelected.Location.X - PageSelectedLocationX, PageSelected.Size.Height)
        PanelTemplate.Location = New Point(PageSelectedLocationX + SplitContainerStep3.Location.X + SplitContainerStep3.Panel1.Location.X + SplitContainerCondition.Panel2.Location.X + TabControlAll.Location.X + PageConditionSelect.Location.X + TabConditionSelect.Location.X,
                                        5 + SplitContainerStep3.Location.Y + SplitContainerStep3.Panel1.Location.Y + SplitContainerCondition.Panel2.Location.Y + TabControlAll.Location.Y + PageConditionSelect.Location.Y + TabConditionSelect.Location.Y + PageSelected.Location.Y)

        PanelSetValue.Size = PanelTemplate.Size
        PanelSetValue.Location = PanelTemplate.Location
    End Sub

    ''' <summary>
    ''' Добавить Закладку Условия
    ''' </summary>
    Private Sub AddTabPage()
        '1.Page Триггер
        Dim newPageTrigger As New TabPage With {
            .ImageIndex = PageImage.Trigger2,
            .Location = New Point(4, 26),
            .Name = "newPageTrigger" & pagesCount.ToString(),
            .Size = New Size(809, 599),
            .TabIndex = 0,
            .Text = cTrigger,
            .Tag = cTrigger
        }
        '2.Page Шаблон
        Dim newPageTemplate As New TabPage With {
            .ImageIndex = PageImage.Template3,
            .Location = New Point(4, 26),
            .Name = "newPageTemplate" & pagesCount.ToString(),
            .Size = New Size(809, 599),
            .TabIndex = 1,
            .Text = cTemplate,
            .Tag = cTemplate,
            .Visible = False
        }
        '3.Tab Условие
        Dim newTabControlCondition As New TabControl With {
            .Alignment = TabAlignment.Left,
            .Appearance = TabAppearance.Buttons,'TabAppearance.Normal TabAppearance.Buttons глюка
            .Dock = DockStyle.Fill,
            .HotTrack = True,
            .ImageList = ImageListCondition,
            .Location = New Point(0, 0),
            .Multiline = True,
            .Name = "newTabControlCondition" & pagesCount.ToString(),
            .SelectedIndex = 0,
            .Size = New Size(817, 629),
            .TabIndex = 1
        }
        ToolTipHelp.SetToolTip(newTabControlCondition, "Задать тип условия")
        With newTabControlCondition
            .Controls.Add(newPageTrigger)
            .Controls.Add(newPageTemplate)
            AddHandler .SelectedIndexChanged, AddressOf TabControlCondition_SelectedIndexChanged
        End With

        '4.Page Условие
        Dim newPageCondition As New TabPage With {
        .ImageIndex = PageImage.Filter0,
        .BorderStyle = BorderStyle.Fixed3D,
        .Location = New Point(4, 26),
        .Name = "newPageCondition" & pagesCount.ToString(),
        .Size = New Size(817, 629),
        .TabIndex = 0,
        .Text = "Условие " & pagesCount.ToString()
        }
        newPageCondition.Controls.Add(newTabControlCondition)
        TabControlAll.Controls.Add(newPageCondition)
        TabControlAll.SelectedIndex = pagesCount - 1
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAdd.Click
        AddCondition()
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDelete.Click
        DeleteCondition()
    End Sub

    ''' <summary>
    ''' Добавить новое условие
    ''' </summary>
    Private Sub AddCondition()
        If TabControlAll.TabPages.Count < maxItemTabPages Then
            pagesCount += 1
            CreateNewConditionOnDefault()
            AddTabPage()
        Else
            ButtonAdd.Enabled = False
            Const caption As String = "Новое условие"
            Dim text As String = $"Вы имеете уже {maxItemTabPages} условий. Для добаления новых условий удалите какое-либо и попробуйте снова."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Вызов формы для выборочного удаления условий.
    ''' Обновление коллекции условий после удалений.
    ''' </summary>
    Private Sub DeleteCondition()
        If pagesCount = 1 Then Exit Sub
        Dim mFormDeleteCondition As New FormDeleteCondition(pagesCount)

        If mFormDeleteCondition.ShowDialog() = DialogResult.OK Then
            Dim indicesForDelete As Boolean() = mFormDeleteCondition.GetConditionsToDelete()
            ' 1 пометить элементы колекции условий на удаление
            ' 2 удалить закладки
            ' 3 в цикле удалить помеченные элементы
            Dim I, totalCount, memoControlCount As Integer

            For I = 0 To pagesCount - 1
                If indicesForDelete(I) Then
                    totalCount += 1
                    TabControlAll.Controls.RemoveAt(TabControlAll.Controls.Count - 1)
                End If
            Next

            If totalCount = 0 Then Exit Sub

            memoControlCount = pagesCount
            pagesCount = TabControlAll.Controls.Count
            ButtonAdd.Enabled = pagesCount < maxItemTabPages

            ' 4 в цикле присвоить условиям новые индексы соответствия закладке и условию
            ' и настроить закладки заново
            ' удаление по индексу с верха коллекции
            For I = memoControlCount To 1 Step -1
                If indicesForDelete(I - 1) Then Conditions.Remove(I)
            Next

            Dim memoConditions As ConditionCollection = New ConditionCollection(Me)
            Dim addedCount As Integer

            For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                addedCount += 1
                memoConditions.Add(addedCount).CopyCondition(itemCondition)
            Next

            ' копируем назад
            Conditions.Clear()
            addedCount = 0

            For Each itemCondition As Condition In memoConditions.GetDictionaryConditions.Values
                addedCount += 1
                Conditions.Add(addedCount).CopyCondition(itemCondition)
            Next

            ' 5 переместиться на 1 закладку для обновления
            indexPreviousPage = 0
            indexCurrentPage = 0
            RestoreConditionOnPageChanged(indexCurrentPage)
            TabControlAll.SelectedIndex = 0
        End If
    End Sub

    Private Sub ComboBox_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ComboBoxValueParameters.DrawItem, ComboBoxPatternParameters.DrawItem
        DrawItemComboBox(sender, die, IndexesAbsentParameters, ImageListChannel, False)
    End Sub

    Private Sub ComboBox_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ComboBoxValueParameters.MeasureItem, ComboBoxPatternParameters.MeasureItem
        Dim cb As ComboBox = CType(sender, ComboBox)
        mie.ItemHeight = cb.ItemHeight - 2
    End Sub
#End Region

#Region "Check Slide"
#Region "Check Start"
    Private Sub SlideValueStartUp_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueStartUp.AfterChangeValue
        CheckMinLessStartUp()
        CheckTimeRange()
    End Sub

    Private Sub SlideValueStartDown_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueStartDown.AfterChangeValue
        CheckMinLessStartUp()
        CheckTimeRange()
    End Sub

    ''' <summary>
    ''' Проверка Мин Меньше Мах Start
    ''' </summary>
    Private Sub CheckMinLessStartUp()
        If SlideValueStartUp.Value < SlideValueStartDown.Value Then SlideValueStartDown.Value = SlideValueStartUp.Value
    End Sub
#End Region

#Region "Check Middle"
    Private Sub SlideValueMiddleUp_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueMiddleUp.AfterChangeValue
        CheckMinLessMiddle()
        CheckTimeRange()
    End Sub

    Private Sub SlideValueMiddleDown_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueMiddleDown.AfterChangeValue
        CheckMinLessMiddle()
        CheckTimeRange()
    End Sub

    ''' <summary>
    ''' Проверка Мин Меньше Middle
    ''' </summary>
    Private Sub CheckMinLessMiddle()
        If SlideValueMiddleUp.Value < SlideValueMiddleDown.Value Then SlideValueMiddleDown.Value = SlideValueMiddleUp.Value
    End Sub
#End Region

#Region "Check Finish"
    Private Sub SlideValueFinishUp_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueFinishUp.AfterChangeValue
        CheckMinLessMaxFinish()
        CheckTimeRange()
    End Sub

    Private Sub SlideValueFinishDown_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValueFinishDown.AfterChangeValue
        CheckMinLessMaxFinish()
        CheckTimeRange()
    End Sub

    ''' <summary>
    ''' Проверка Мин Меньше Мах Finish
    ''' </summary>
    Private Sub CheckMinLessMaxFinish()
        If SlideValueFinishUp.Value < SlideValueFinishDown.Value Then SlideValueFinishDown.Value = SlideValueFinishUp.Value
    End Sub
#End Region
#End Region

#Region "Поиск"
    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFind.Click
        StartTaskFind()
    End Sub

    ''' <summary>
    ''' Подготовка и запуск задачи поиска кадров удовлетворяющих условиям поиска
    ''' </summary>
    Private Sub StartTaskFind()
        ' могло быть редактирование на закладке без смены и последующей записи условия 
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StartTaskFind)}> поиск кадров отвечающих условию")
        ' и чтобы изменения условия не записалось в коллекцию
        StorageConditionOnPageChanged(indexCurrentPage)

        If Not Conditions.CheckCorrectConditions() OrElse Not CheckFilesAndParameters() Then Exit Sub

        If engineSnapshotDataTable.Rows.Count = 0 Then
            Const Caption As String = "Некорректные условия"
            Dim text As String = "Не выделены снимки изделия для поиска!" & vbCrLf
            MessageBox.Show(text, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{Caption}> {text}")
            Exit Sub
        End If

        ' здесь начало цикла поиска
        ' очистка коллекции найденных кадров
        TSProgressBar.Value = 0
        FramesOk.Clear()
        EnabledControls(False)
        PrepareFlagsFind()
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        ' запустить фоновый поток работы
        backgroundCalculator.RunWorkerAsync(ListViewAllSnapshot.SelectedItems.Count)
    End Sub

    ''' <summary>
    ''' Определить логические переменные наличия условий формы и мин мах
    ''' </summary>
    Private Sub PrepareFlagsFind()
        ' В цикле просмотр условий и присвоить True, если данный тип условий встречается
        ' 3  логические переменные комбинаций по всем типам условий
        isConditionMandatory = False
        isConditionMinMax = False
        isConditionPattern = False

        For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
            With itemCondition
                If .ConditionTypeProperty = ConditionType.Mandatory Then
                    isConditionMandatory = True
                ElseIf .ConditionTypeProperty = ConditionType.MinMax Then
                    isConditionMinMax = True
                ElseIf .ConditionTypeProperty = ConditionType.Template Then
                    isConditionPattern = True
                End If
            End With
        Next

        If isConditionMinMax AndAlso isConditionPattern Then
            memoConditionFind = ConditionFind.MinMaxTemplate
        ElseIf Not (isConditionMinMax) AndAlso isConditionPattern Then
            memoConditionFind = ConditionFind.Pattern
        ElseIf isConditionMinMax AndAlso Not (isConditionPattern) Then
            memoConditionFind = ConditionFind.MinMax
        End If

        If isConditionMinMax Then
            For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                With itemCondition
                    If .ConditionTypeProperty = ConditionType.MinMax Then
                        If .SelectiveConditionProperty = SelectiveCondition.FindMaximum Then
                            .Maximum = Double.MinValue
                        Else ' SelectiveCondition.НайтиМинимум()
                            .Minimum = Double.MaxValue
                        End If
                        .Frame = Nothing
                    End If
                End With
            Next
        End If
    End Sub

    ' Этот регион реализует вычисления в асинхронной манере
#Region "Asynchronous calculation"
    ''' <summary>
    ''' Основная функция фонового потока.
    ''' В процессе работы выводит прогресс в делегате BackgroundWorker.ReportProgress
    ''' </summary>
    ''' <param name="selectedItemCount"></param>
    ''' <param name="worker"></param>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function GetApproachSnapshotAsync(ByVal selectedItemCount As Integer, ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs) As Integer
        Dim fileName As String = String.Empty
        ' подготовительные работы
        ' первая запись копируется в буфер 2, а в цикле она перенесется в буфер 1
        LoadSnapshot(engineSnapshotDataTable.Rows(0), fileName)
        CopyBuffer(Buf, Buf2)

        'For Each drDataRow In mdtDataTable.Rows ' Цикл по кадрам с нулевого индекса
        Dim endRow As Integer = engineSnapshotDataTable.Rows.Count - 1
        Dim counterProgress As Integer = 0

        For I As Integer = 0 To endRow ' Цикл по кадрам с нулевого индекса
            ' проверить прерывание
            If worker.CancellationPending = True Then
                e.Cancel = True
                Exit For
            Else
                dataRowBuf1 = engineSnapshotDataTable.Rows(I)
                If I <> endRow Then dataRowBuf2 = engineSnapshotDataTable.Rows(I + 1)

                frequencyBuf1 = CInt(dataRowBuf1("ЧастотаОпроса"))
                frequencyBuf2 = CInt(dataRowBuf2("ЧастотаОпроса"))
                countRowsBuf1 = CInt(dataRowBuf1("КолСтрок"))

                ' Очистка для в коллеции условий формы что для данного кадра условие найдено
                '            новый(буфер = False)
                If isConditionPattern Then ' сброс значения НайденНаДанномКадре
                    Conditions.ResetAllIsFounded()
                End If

                ' считывание буферов и нахождения индексов параметров
                ' Цикл по именам параметров для данного снимка эдесь разбор строки drDataRow("СтрокаКонфигурации") 
                CopyBuffer(Buf2, Buf1)
                If I <> endRow Then
                    ' с опережением
                    LoadSnapshot(dataRowBuf2, fileName)
                    CopyBuffer(Buf, Buf2)
                End If

                CycleForLine(dataRowBuf1)
                ' обновить прогресс
                counterProgress += 1
                'Dim userState As String ' текущее сообщение
                'worker.ReportProgress(percentComplete Mod 100) ', userState)
                'If counterProgress Mod 5 = 0 Then TSProgressBar.Value = CDbl(counterProgress / selectedItemCount) * 100
                worker.ReportProgress(CInt(CDbl(counterProgress / selectedItemCount) * 100), fileName)
                'Thread.Sleep(1000) ' иммитация работы
            End If
        Next

        Return counterProgress ' что-то выдать по результатам работы
    End Function

    ''' <summary>
    ''' Получить информацию о возможных исключениях в DoWork. 
    ''' Кроме того, код в обработчике RunWorkerCompleted может обновлять элементы управления Windows Forms без явного маршалинга.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackgroundCalculator_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
        TSProgressBar.Value = 0
        ' восстановить        
        EnabledControls(True)
        Windows.Forms.Cursor.Current = Cursors.Default

        If e.Cancelled Then
            UpdateStatus("Работа была прервана пользователем!")
            Exit Sub
        ElseIf e.Error IsNot Nothing Then
            ReportError(e.Error)
            Exit Sub
        Else
            UpdateStatus($"Выполнен поиск в: {CInt(e.Result)} кадрах.")
        End If

        If isConditionMinMax Then
            ' здесь содержится самые мах и мин значения
            For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                With itemCondition
                    If .ConditionTypeProperty = ConditionType.MinMax AndAlso .Frame IsNot Nothing Then
                        ' иногда условия MinMax есть, но они не сработали из-за того, что не сработали обязательные условия
                        FramesOk.Add(.Frame)
                    End If
                End With
            Next
        End If

        ' убрать источник данных для очистки таблицы
        BindingSourceFoundedSnapshotDataTable.DataSource = Nothing
        DataGridFoundSnapshot.DataSource = Nothing

        If FramesOk.Count = 0 Then
            Const Caption As String = "Некорректные условия"
            Dim text As String = $"По заданным Вами условиям ничего не найдено.{vbCrLf}Возможно условия взаимоисключающие или допуски слишком малы.{vbCrLf}Попробуйте изменить условия или расширить допуски в шаблоне."
            MessageBox.Show(text, "Ни чего не найдено", MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{Caption}> {text}")
        Else
            PopulateTableFoundSnapshot()
            ButtonPrevious.Enabled = True
            ButtonNext.Enabled = True
            ShowCurrentRecordNumber()
            Step3()
        End If
    End Sub

    ''' <summary>
    ''' Код в обработчике ProgressChanged может свободно обращаться к элементам управления UI так же, как и в RunWorkerCompleted. 
    ''' Обычно это нужно для обновления индикатора прогресса.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackgroundCalculator_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        UpdateProgress(e.ProgressPercentage)
        TSLabelMessage.Text = Convert.ToString(e.UserState) 'fileName
    End Sub

    ''' <summary>
    ''' DoWork метод запускает другой поток и здесь нет доступа к элеементам созданными
    ''' в UI потоке. Попытка доступа к таким UI элементам будет вызывать исключение.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackgroundCalculator_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
        Dim selectedItemCount As Integer = CInt(e.Argument)
        e.Result = GetApproachSnapshotAsync(selectedItemCount, CType(sender, BackgroundWorker), e)
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCancel.Click
        If backgroundCalculator.IsBusy Then
            UpdateStatus("Прерывание...")
            backgroundCalculator.CancelAsync()
        End If
    End Sub

    Private Sub EnabledControls(isEnabled As Boolean)
        ButtonAdd.Enabled = isEnabled
        ButtonDelete.Enabled = isEnabled
        ButtonFind.Enabled = isEnabled
        ButtonBackToStep1.Enabled = isEnabled
        ToolStripSteps.Enabled = isEnabled
        TabControlAll.Enabled = isEnabled

        ButtonLoadCondition.Enabled = isEnabled
        ButtonSaveCondition.Enabled = isEnabled
        ButtonDeleteCondition.Enabled = isEnabled

        ButtonCancel.Enabled = Not isEnabled
        TSProgressBar.Visible = Not isEnabled
    End Sub
#End Region

    ' Пользовательский интерфейс обратной связи
#Region "User Feedback"
    ''' <summary>
    ''' Обновить сообщение о работе фоновой задачи
    ''' </summary>
    ''' <param name="status"></param>
    Private Sub UpdateStatus(ByVal status As String)
        TSLabelMessage.Text = status
    End Sub

    ''' <summary>
    ''' Индикация прогресса используя progress bar
    ''' </summary>
    ''' <param name="percentComplete"></param>
    Private Sub UpdateProgress(ByVal percentComplete As Integer)
        TSProgressBar.Value = percentComplete
    End Sub

    ''' <summary>
    ''' Сообщение об ошибке
    ''' </summary>
    ''' <param name="e"></param>
    Private Sub ReportError(ByVal e As Exception)
        UpdateStatus("Ошибка!")
        MessageBox.Show($"Была получена следующая ошибка: {ControlChars.CrLf}{e}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
    ''' <summary>
    '''  Сообщение об ошибке
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub ReportError(ByVal message As String)
        UpdateStatus("Ошибка!")
        MessageBox.Show($"Была получена следующая ошибка: {ControlChars.CrLf}{message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
#End Region

    ''' <summary>
    ''' Цикл по строкам
    ''' </summary>
    ''' <param name="refDataRow"></param>
    Private Sub CycleForLine(ByRef refDataRow As DataRow)
        Dim success As Boolean ' все Обязательные Условия Выполнены
        Dim isNotFoundedTemplate As Boolean ' есть Ненайденные Шаблоны
        Dim isNeedFindTemplate As Boolean = True ' искать Ненайденные Шаблоны

        For I As Integer = 0 To countRowsBuf1
            success = True
            If isConditionMandatory Then
                For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                    With itemCondition
                        If .ConditionTypeProperty = ConditionType.Mandatory Then
                            Select Case .SelectiveConditionProperty
                                Case SelectiveCondition.Between
                                    If Not (Buf1(I, .IndexParameter) >= .LowerValueCondition AndAlso Buf1(I, .IndexParameter) <= .UpperValueCondition) Then success = False : Exit For
                                Case SelectiveCondition.OutOfRange
                                    If Not (Buf1(I, .IndexParameter) > .UpperValueCondition OrElse Buf1(I, .IndexParameter) < .LowerValueCondition) Then success = False : Exit For
                                Case SelectiveCondition.Equal
                                    If Not (Buf1(I, .IndexParameter) = .Condition) Then success = False : Exit For
                                Case SelectiveCondition.NotEqual
                                    If Not (Buf1(I, .IndexParameter) <> .Condition) Then success = False : Exit For
                                Case SelectiveCondition.Greater
                                    If Not (Buf1(I, .IndexParameter) > .Condition) Then success = False : Exit For
                                Case SelectiveCondition.Less
                                    If Not (Buf1(I, .IndexParameter) < .Condition) Then success = False : Exit For
                                Case SelectiveCondition.GreaterThanOrEqual
                                    If Not (Buf1(I, .IndexParameter) >= .Condition) Then success = False : Exit For
                                Case SelectiveCondition.LessThanOrEqual
                                    If Not (Buf1(I, .IndexParameter) <= .Condition) Then success = False : Exit For
                            End Select
                        End If
                    End With
                Next
            End If

            If success Then
                If isConditionPattern OrElse isConditionMinMax Then
                    Select Case memoConditionFind
                        Case ConditionFind.MinMaxTemplate ' ЕстьУсловияMinMax AndAlso ЕстьУсловияШаблон
                            ' 1
                            ' почти копия двух следующих
                            ' На данном кадре есть шаблоны формы еще не найденные Цикл по условиям  формы
                            If isNeedFindTemplate Then
                                isNotFoundedTemplate = False
                                For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                                    With itemCondition
                                        If .ConditionTypeProperty = ConditionType.Template AndAlso .IsFoundedOnThisFrame = False Then
                                            isNotFoundedTemplate = True : Exit For
                                        End If
                                    End With
                                Next

                                If isNotFoundedTemplate Then
                                    CycleTemplateCondition(refDataRow, I)
                                Else 'все шаблоны на даннам кадре найдены
                                    isNeedFindTemplate = False
                                End If
                            End If
                            ' выполняется всегда
                            CycleConditionMinMax(refDataRow, I)
                        Case ConditionFind.Pattern ' ЕстьУсловияMinMax = False AndAlso ЕстьУсловияШаблон = True 
                            ' 2
                            ' На данном кадре есть шаблоны формы еще не найденные Цикл по условиям  формы
                            For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                                With itemCondition
                                    If .ConditionTypeProperty = ConditionType.Template AndAlso .IsFoundedOnThisFrame = False Then
                                        isNotFoundedTemplate = True : Exit For
                                    End If
                                End With
                            Next

                            If isNotFoundedTemplate = True Then
                                CycleTemplateCondition(refDataRow, I)
                            Else ' все шаблоны на даннам кадре найдены выход из цикла по строкам
                                Exit For
                            End If
                        Case ConditionFind.MinMax ' ЕстьТипУсловияMinMax = True AndAlso ЕстьТипУсловияШаблон = False 
                            ' 3
                            CycleConditionMinMax(refDataRow, I)
                    End Select

                Else ' ЕстьУсловияMinMax OrElse ЕстьУсловияШаблон = False
                    FramesOk.Add(GetFrameConditionWhereAllOk(refDataRow, I))
                    Exit For ' выход из цикла по строкам
                End If
            End If ' Все Обязательные Условия Выполнены
        Next
    End Sub

    ''' <summary>
    ''' Условия Цикла Шаблон
    ''' </summary>
    ''' <param name="refDataRow"></param>
    ''' <param name="I"></param>
    Private Sub CycleTemplateCondition(ByRef refDataRow As DataRow, ByVal I As Integer)
        Dim timeCurveBend As Double ' Время Перегиба
        Dim widthWindowTime As Double ' ширина Временного Окна
        Dim indexTimeCurveBend As Integer ' index Время Перегиба
        Dim indexWidthWindowTime As Integer ' index Ширина Временного Окна
        Dim startValue As Double ' старт Значение
        Dim curveBendValue As Double ' перегиб Значение
        Dim finishValue As Double ' финиш Значение
        ' Цикл по Элементам класса коллекции шаблонов которые не найдены
        ' для последнего кадра поиск надо прекратить когда конец окна достиг КолСтрокBuf1
        ' но ошибки не происходит так как для последнего кадра Buf2 равен Buf1 и могут быть только коллизии 
        ' что условия сработают 2 раза на последнем кадре
        For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
            With itemCondition
                If .ConditionTypeProperty = ConditionType.Template AndAlso .IsFoundedOnThisFrame = False Then
                    isContinuationNextFrame = False
                    timeCurveBend = .TimeTwist
                    widthWindowTime = .WidthTemporaryWindow
                    indexTimeCurveBend = I + CInt(timeCurveBend * frequencyBuf1)
                    indexWidthWindowTime = I + CInt(widthWindowTime * frequencyBuf1)

                    If I < countRowsBuf1 AndAlso indexTimeCurveBend < countRowsBuf1 AndAlso indexWidthWindowTime <= countRowsBuf1 Then
                        ' весь шаблон в буфере1
                        startValue = Buf1(I, .IndexParameter)
                        curveBendValue = Buf1(indexTimeCurveBend, .IndexParameter)
                        finishValue = Buf1(indexWidthWindowTime, .IndexParameter)
                    ElseIf I < countRowsBuf1 AndAlso indexTimeCurveBend <= countRowsBuf1 AndAlso indexWidthWindowTime > countRowsBuf1 Then
                        ' начало и перегиб в буфере1 конец  в буфере2
                        startValue = Buf1(I, .IndexParameter)
                        curveBendValue = Buf1(indexTimeCurveBend, .IndexParameter)
                        finishValue = Buf2(CInt((widthWindowTime - (countRowsBuf1 - I) / frequencyBuf1) * frequencyBuf2), .IndexParameter)
                        isContinuationNextFrame = True
                    ElseIf I <= countRowsBuf1 AndAlso indexTimeCurveBend > countRowsBuf1 AndAlso indexWidthWindowTime > countRowsBuf1 Then
                        ' начало в буфере1 перегиб и конец в буфере2
                        startValue = Buf1(I, .IndexParameter)
                        curveBendValue = Buf2(CInt((timeCurveBend - (countRowsBuf1 - I) / frequencyBuf1) * frequencyBuf2), .IndexParameter)
                        finishValue = Buf2(CInt((widthWindowTime - (countRowsBuf1 - I) / frequencyBuf1) * frequencyBuf2), .IndexParameter)
                        isContinuationNextFrame = True
                    End If

                    If (.StartLover <= startValue AndAlso startValue <= .StartUpper) AndAlso (.TwistLower <= curveBendValue AndAlso curveBendValue <= .TwistTop) AndAlso (.FinishLower <= finishValue AndAlso finishValue <= .FinishTop) Then
                        ' Шаблон подходит?
                        ' Записать в свойство что для данного снимка условие уже найдено
                        .IsFoundedOnThisFrame = True
                        ' Запись в коллекцию найденных снимков
                        .Frame = GetFrameConditionWhereAllOk(refDataRow, I)
                        .Frame.ConditionWhereOK(.ID) = True
                        .Frame.Title = "Начало шаблона " & .NameFormParameter
                        FramesOk.Add(.Frame)

                        ' Если условие выполнилось на 2 снимках то запись 2 снимка с нулевого индекса
                        If isContinuationNextFrame Then
                            .Frame = GetFrameConditionWhereAllOk(dataRowBuf2, 0)
                            .Frame.ConditionWhereOK(.ID) = True
                            .Frame.Title = "Продолжение шаблона " & .NameFormParameter
                            FramesOk.Add(.Frame)
                        End If
                    End If
                End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' Условия Цикла MinMax
    ''' </summary>
    ''' <param name="refDataRow"></param>
    ''' <param name="I"></param>
    Private Sub CycleConditionMinMax(ByRef refDataRow As DataRow, ByVal I As Integer)
        ' Сравнение мин мах и если найдены новые значения, то перезапись в коллекцию найденных снимков
        For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
            With itemCondition
                If .ConditionTypeProperty = ConditionType.MinMax Then
                    If .SelectiveConditionProperty = SelectiveCondition.FindMaximum AndAlso Buf1(I, .IndexParameter) > .Maximum Then
                        .Maximum = Buf1(I, .IndexParameter)
                        .Frame = GetFrameConditionWhereAllOk(refDataRow, I)
                        .Frame.ConditionWhereOK(.ID) = True
                        .Frame.Title = $"Найден максимум { .NameValueParameter}={ .Maximum}"
                    ElseIf .SelectiveConditionProperty = SelectiveCondition.FindMinimum AndAlso Buf1(I, .IndexParameter) < .Minimum Then
                        .Minimum = Buf1(I, .IndexParameter)
                        .Frame = GetFrameConditionWhereAllOk(refDataRow, I)
                        .Frame.ConditionWhereOK(.ID) = True
                        .Frame.Title = $"Найден минимум { .NameValueParameter}={ .Minimum}"
                    End If
                End If
            End With
        Next
    End Sub

    ''' <summary>
    ''' Выдать FrameCondition где все обязательные условия выполнены
    ''' </summary>
    ''' <param name="refDataRow"></param>
    ''' <param name="I"></param>
    ''' <returns></returns>
    Private Function GetFrameConditionWhereAllOk(ByRef refDataRow As DataRow, ByVal I As Integer) As FrameCondition
        ' Занести в коллекцию найденных и переход к след кадру
        Dim newFrameCondition As New FrameCondition(Conditions.Count)

        With newFrameCondition
            For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
                If itemCondition.ConditionTypeProperty = ConditionType.Mandatory Then .ConditionWhereOK(itemCondition.ID) = True
            Next

            .Text = $"Дата:{Convert.ToDateTime(refDataRow("Дата")).ToShortDateString} Время:{Convert.ToDateTime(refDataRow("ВремяНачалаСбора")).ToShortTimeString} Режим:{refDataRow("Режим")} {refDataRow("Примечание")}"
            .KeyIDFrame = CInt(refDataRow("KeyID"))
            .TimeConditionIsOk = CSng(I / frequencyBuf1)
            .Frequency = frequencyBuf1
            .Title = "Выполнены все условия"
        End With

        Return newFrameCondition
    End Function

    ''' <summary>
    ''' Считать кадр с диска
    ''' </summary>
    ''' <param name="refDataRow"></param>
    ''' <param name="fileName"></param>
    Private Sub LoadSnapshot(ByRef refDataRow As DataRow, ByRef fileName As String)
        Dim measure As Integer
        Dim allText As String = Nothing
        Dim arrLine As String()
        Dim arrWord As String()
        Dim delimiterCrLf As Char() = vbCr.ToCharArray 'vbCrLf
        Dim delimiterTab As Char() = vbTab.ToCharArray

        ' кол. строк и стобцов востановилось
        Dim I As Integer = CInt(refDataRow("КолСтолбцов"))
        Dim J As Integer = CInt(refDataRow("КолСтрок"))

        'If measureFromFile.GetUpperBound(0) <> J OrElse measureFromFile.GetUpperBound(1) <> I Then ReDim_measureFromFile(J, I)
        If measureFromFile.GetUpperBound(0) <> J OrElse measureFromFile.GetUpperBound(1) <> I Then Re.Dim(measureFromFile, J, I)

        fileName = CStr(refDataRow("ПутьНаДиске"))
        'TSLabelMessage.Text = fileName

        Dim fi As FileInfo = New FileInfo(fileName)

        If Path.GetExtension(fi.Name).ToUpper() = ".TDMS" Then
            Dim ReadTdmsFileProcessor = New TdmsFileProcessor
            'Dim waveform As AnalogWaveform(Of Double)() = AnalogWaveform(Of Double).FromArray2D(varTdmsFileProcessor.LoadTDMSFile(strПутьТекстовогоПотока))
            measureFromFile = ReadTdmsFileProcessor.LoadTDMSFile(fileName)
            measureFromFile = NationalInstruments.Analysis.Math.LinearAlgebra.Transpose(measureFromFile)
            measure = UBound(measureFromFile, 1) + 1
        Else
            Dim isException As Boolean
            Using FS = New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using SR = New StreamReader(FS)
                    Try
                        allText = SR.ReadToEnd
                    Catch ex As Exception
                        Dim caption As String = "Загрузка файла " & fileName
                        Dim text As String = ex.ToString
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        isException = True
                        backgroundCalculator.CancelAsync()
                    End Try
                End Using
            End Using

            If isException Then Exit Sub

            arrLine = allText.Split(delimiterCrLf)
            measure = UBound(arrLine)
            If measure < arraysize + 1 Then measure -= 1 ' для прерванных кадров

            For I = 0 To measure - 1
                arrWord = arrLine(I).Split(delimiterTab)
                For J = 0 To UBound(arrWord)
                    measureFromFile(I, J) = CDbl(arrWord(J))
                Next
            Next
        End If

        ' в массиве arrНаименованиеTemp все имена измеренных параметров
        ' в массиве arrСписокПараметров имена параметров поиска
        UnpackStringConfiguration(CStr(refDataRow("СтрокаКонфигурации")))

        If Buf.GetUpperBound(0) <> measureFromFile.GetUpperBound(0) OrElse Buf.GetUpperBound(1) <> parameterNames.GetUpperBound(0) Then
            'ReDim_Buf(measureFromFile.GetUpperBound(0), parameterNames.GetUpperBound(0))
            Re.Dim(Buf, measureFromFile.GetUpperBound(0), parameterNames.GetUpperBound(0))
        End If

        Dim indexParam As Integer
        For indexName As Integer = 0 To UBound(parameterNames)
            indexParam = Array.IndexOf(parameterNamesConfiguration, parameterNames(indexName))
            For I = 0 To measure - 1
                Buf(I, indexName) = measureFromFile(I, indexParam)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Копировать буфера из Источник в Приемник
    ''' </summary>
    ''' <param name="bufSource"></param>
    ''' <param name="bufReceiver"></param>
    Private Sub CopyBuffer(ByRef bufSource(,) As Double, ByRef bufReceiver(,) As Double)
        If bufSource.GetUpperBound(0) <> bufReceiver.GetUpperBound(0) OrElse bufSource.GetUpperBound(1) <> bufReceiver.GetUpperBound(1) Then
            'ReDim_bufReceiver(bufSource.GetUpperBound(0), bufSource.GetUpperBound(1))
            Re.Dim(bufReceiver, bufSource.GetUpperBound(0), bufSource.GetUpperBound(1))
        End If
        Array.Copy(bufSource, bufReceiver, bufSource.Length)
    End Sub

    ''' <summary>
    ''' Проверить наличие всех кадров и наличия параметров
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckFilesAndParameters() As Boolean
        ' удалить записи не выделенные в ListViewAllSnapshot.SelectedItems()
        Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter($"SELECT * FROM [БазаСнимков] WHERE НомерИзделия = {numberEngine} ORDER BY БазаСнимков.KeyID",
                                                                      BuildCnnStr(ProviderJet, PathChannels))
            engineSnapshotDataTable = New DataTable
            odaDataAdapter.Fill(engineSnapshotDataTable)
        End Using

        ' удалить из таблицы записи не выделенные в листе
        For Each itemDataRow As DataRow In engineSnapshotDataTable.Rows
            Dim success As Boolean = False ' строка Найдена
            For Each lvItem As ListViewItem In ListViewAllSnapshot.SelectedItems()
                If CInt(itemDataRow("KeyID")) = CInt(Val(lvItem.Tag)) Then success = True : Exit For
            Next

            If success = False Then itemDataRow.Delete()
        Next

        engineSnapshotDataTable.AcceptChanges()

        If CheckFiles() AndAlso CheckParameters() Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Проверить наличия параметров
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckParameters() As Boolean
        ' В цикле проверить наличие всех кадров и наличия параметров просто вхождения подстроки
        Dim configurationChannels As String ' строка Конфигурации
        Dim reportAbsenceParameters As String = String.Empty ' сообщения Отсутствия Параметров
        Dim name As String ' имя Параметр
        Dim nameParameterWithSeparate As String ' имя Параметр С Разделителем        
        Dim errorCount As Integer
        ' проверка наличия параметров в коллекции условий поиска
        'ReDim_parameterNames(0)
        Re.Dim(parameterNames, 0)
        'Array.Clear(parameterNames, 0, parameterNames.Length)

        For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
            With itemCondition
                ' обязательно с разделителем иначе может найти похожее
                If .ConditionTypeProperty = ConditionType.Template Then
                    name = .NameFormParameter
                    nameParameterWithSeparate = Separator & name & Separator
                Else ' Триггер
                    name = .NameValueParameter
                    nameParameterWithSeparate = Separator & name & Separator
                End If

                For Each itemDataRow As DataRow In engineSnapshotDataTable.Rows
                    configurationChannels = Separator & CStr(itemDataRow("СтрокаКонфигурации")) ' в начало добавить разделитель
                    If configurationChannels.IndexOf(nameParameterWithSeparate) = -1 AndAlso errorCount < 30 Then
                        reportAbsenceParameters = $"{reportAbsenceParameters}<{name}> в снимке {itemDataRow("Примечание")}{vbCrLf}"
                        errorCount += 1
                        If errorCount >= 30 Then reportAbsenceParameters &= " и далее..."
                    Else
                        If Array.IndexOf(parameterNames, name) = -1 Then
                            'If parameterNames.Length = 1 Then
                            '    parameterNames(UBound(parameterNames)) = nameParameter
                            'Else
                            parameterNames(UBound(parameterNames)) = name
                            'ReDimPreserve parameterNames(UBound(parameterNames) + 1)
                            Re.DimPreserve(parameterNames, UBound(parameterNames) + 1)
                            'End If
                        End If
                        .IndexParameter = Array.IndexOf(parameterNames, name)
                    End If
                Next
            End With
        Next

        ' обрезать последнюю пустую строку
        'ReDimPreserve parameterNames(UBound(parameterNames) - 1)
        Re.DimPreserve(parameterNames, UBound(parameterNames) - 1)

        If reportAbsenceParameters Is String.Empty Then
            Return True
        Else
            PopulateIndexAbsentParametersForDrawItem()
            Const caption As String = "Проверка параметров"
            Dim text As String = $"Отсутствие параметров в снимках:{vbCrLf}{reportAbsenceParameters}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If
    End Function

    ''' <summary>
    ''' Проверить наличие всех кадров
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckFiles() As Boolean
        ' из базы по mItem.Tag считываем нужные настройки и пути к записям(проверять путь к файлу и есть ли он на диске)
        Dim errorCount As Integer
        Dim pathFileSnapshot As String ' путь Текстового Потока
        Dim reportAbsenceFiles As String = String.Empty ' сообщения Отсутствия Снимков
        ' проверка наличия файла на диске
        For Each itemDataRow As DataRow In engineSnapshotDataTable.Rows
            pathFileSnapshot = ProduceRightPath(CStr(itemDataRow("ПутьНаДиске")))
            If FileNotExists(pathFileSnapshot) AndAlso errorCount < 30 Then
                errorCount += 1
                If errorCount >= 30 Then
                    reportAbsenceFiles = $"{reportAbsenceFiles}{pathFileSnapshot}{vbCrLf} и далее..."
                Else
                    reportAbsenceFiles &= pathFileSnapshot & vbCrLf
                End If
            Else
                itemDataRow("ПутьНаДиске") = pathFileSnapshot
            End If
        Next

        If reportAbsenceFiles Is String.Empty Then
            Return True
        Else
            Const caption As String = "Проверка файлов"
            Dim text As String = $"Отсутствие на диске файлов:{vbCrLf}{reportAbsenceFiles}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If
    End Function

    ''' <summary>
    ''' Определить индексы для DrawItem
    ''' </summary>
    Private Sub PopulateIndexAbsentParametersForDrawItem()
        Dim regexFind As New Regex("\\")
        Dim tableParametersInBase As New Dictionary(Of String, String)(arrTypeNameUnit.Length) ' параметры из базы
        Dim tableAbsentParameters As New Dictionary(Of String, String)(arrTypeNameUnit.Length) ' параметры НеНайденные 
        Dim tableTemp As New Dictionary(Of String, String)(arrTypeNameUnit.Length)

        ' скопировать имена
        For I As Integer = 1 To UBound(arrTypeNameUnit)
            tableParametersInBase(arrTypeNameUnit(I).NameParameter) = arrTypeNameUnit(I).NameParameter
        Next

        ' разобрать СтрокаКонфигурации
        For Each itemDataRow As DataRow In engineSnapshotDataTable.Rows
            Dim words As String() = regexFind.Split(CStr(itemDataRow("СтрокаКонфигурации")))
            'For Each Word In Words'это пример поиска подсчета числа слов в тексте
            '    'Dim iWord As String = Word.ToLower
            '    'If Not Table.ContainsKey(iWord) Then
            '    '    Table(iWord) = CInt(Table(iWord) + 1)
            '    'Else
            '    '    Table(iWord) = 1
            '    'End If
            'Next

            ' копировать в Hashtable для увеличения скорости работы
            tableTemp.Clear()
            For Each word As String In words
                tableTemp(word) = word
            Next

            For Each key As String In tableParametersInBase.Keys
                If Not tableTemp.ContainsKey(key) Then ' параметра нет хотя бы один раз
                    tableAbsentParameters(key) = key
                End If
            Next
        Next

        '' отладка
        'For Each key As String In tableParametersInBase.Keys
        '    Debug.WriteLine(Key)
        'Next

        Dim J As Integer
        ' индексы ненайденных соответствуют индексам в списках на форме
        'ReDim_IndexesAbsentParameters(tableAbsentParameters.Count - 1)
        Re.Dim(IndexesAbsentParameters, tableAbsentParameters.Count - 1)

        For I As Integer = 1 To UBound(arrTypeNameUnit)
            If tableAbsentParameters.ContainsKey(arrTypeNameUnit(I).NameParameter) Then
                IndexesAbsentParameters(J) = I
                J += 1
            End If
        Next
    End Sub

    ''' <summary>
    ''' Заполнить таблицу найденными кадрами
    ''' </summary>
    Private Sub PopulateTableFoundSnapshot()
        ' создать таблицу, создать колонки и заполнить из коллекци сКадрУсловия
        foundSnapshotDataTable = New DataTable("НайденныеКадры")
        foundSnapshotDataTable.Columns.Add("KeyID", GetType(Integer))
        foundSnapshotDataTable.Columns.Add("Снимок", GetType(String))

        For I As Integer = 1 To Conditions.Count
            foundSnapshotDataTable.Columns.Add(cCondition & I.ToString, GetType(Boolean))
        Next

        For Each itemFrameCondition As FrameCondition In FramesOk
            Dim foundedDataRow As DataRow = foundSnapshotDataTable.NewRow

            foundedDataRow.BeginEdit()
            foundedDataRow("KeyID") = itemFrameCondition.KeyIDFrame
            foundedDataRow("Снимок") = itemFrameCondition.Text
            For J As Integer = 1 To Conditions.Count
                foundedDataRow(cCondition & J.ToString) = itemFrameCondition.ConditionWhereOK(J)
            Next
            foundedDataRow.EndEdit()

            foundSnapshotDataTable.Rows.Add(foundedDataRow)
        Next

        foundSnapshotDataTable.AcceptChanges()
        ' связывание
        BindingSourceFoundedSnapshotDataTable.DataSource = foundSnapshotDataTable
        DataGridFoundSnapshot.DataSource = BindingSourceFoundedSnapshotDataTable
        ' применить стиль
        DataGridFoundSnapshot.Columns(0).Visible = False
        DataGridFoundSnapshot.Columns(1).Frozen = True
        Dim indexCondition As Integer = 1

        For Each itemCondition As Condition In Conditions.GetDictionaryConditions.Values
            DataGridFoundSnapshot.Columns(indexCondition + 1).HeaderText = $"Условие {indexCondition}{Environment.NewLine}{{{itemCondition.GetAttributeCondition}}}"
            indexCondition += 1
        Next
    End Sub
#End Region

#Region "NavigationSteps"
    Private Sub FormConditionFind_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        FormResize()
    End Sub

    Private Sub ButtonExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonExit.Click
        Close()
    End Sub

    Private Sub ButtonBackToStep1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonBackToStep1.Click, TSButtonSourceStep1.Click
        Step1()
    End Sub
    Private Sub ButtonForwardToStep2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonForwardToStep2.Click, ButtonBackToStep2.Click, TSButtonConditionStep2.Click
        Step2()
    End Sub
    Private Sub TSButtonResultStep3_Click(sender As Object, e As EventArgs) Handles TSButtonResultStep3.Click
        Step3()
    End Sub

    Private Sub Step1()
        MinimumSize = New Size(1024, 768)
        WindowState = FormWindowState.Maximized
        SplitContainerStep3.Panel1Collapsed = False
        SplitContainerStep3.Panel2Collapsed = True

        SplitContainerCondition.Visible = False
        SplitContainerCondition.Dock = DockStyle.None
        SplitContainerEngine.Visible = True
        SplitContainerEngine.Dock = DockStyle.Fill

        PanelTemplate.Visible = False
        PanelSetValue.Visible = False

        LabelCaptionSteps.Text = "Выбрать снимки изделия, где будет произведен поиск"
        TopMost = False
    End Sub

    Private Sub Step2()
        MinimumSize = New Size(1024, 768)
        WindowState = FormWindowState.Maximized
        SplitContainerStep3.Panel1Collapsed = False
        SplitContainerStep3.Panel2Collapsed = True

        SplitContainerCondition.Visible = True
        SplitContainerCondition.Dock = DockStyle.Fill
        SplitContainerEngine.Visible = False
        SplitContainerEngine.Dock = DockStyle.None

        PanelTemplate.Visible = True
        PanelSetValue.Visible = True

        SetSizePanels()

        LabelCaptionSteps.Text = "Создать условия поиска или выбрать из ранее созданных"
        TopMost = False
    End Sub

    Private Sub Step3()
        MinimumSize = New Size(800, 300)
        WindowState = FormWindowState.Normal
        Size = New Size(800, 300)
        SplitContainerStep3.Panel1Collapsed = True
        SplitContainerStep3.Panel2Collapsed = False

        PanelTemplate.Visible = False
        PanelSetValue.Visible = False

        LabelCaptionSteps.Text = "Щёлкните по списку для загрузки снимка"
        TopMost = True
    End Sub

    Private Sub ListViewAllSnapshot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListViewAllSnapshot.SelectedIndexChanged
        ButtonForwardToStep2.Enabled = ListViewAllSnapshot.SelectedItems.Count > 0
        ButtonFind.Enabled = ListViewAllSnapshot.SelectedItems.Count > 1 'должно быть выделено минимум 2 строки
    End Sub

    Private Sub ButtonSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSelectAll.Click
        For Each lvItem As ListViewItem In ListViewAllSnapshot.Items
            lvItem.Selected = True
        Next
        ListViewAllSnapshot.Select()
    End Sub

    Private Sub ButtonEraseSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEraseSelect.Click
        For Each lvItem As ListViewItem In ListViewAllSnapshot.SelectedItems()
            If InStr(1, "- прерванный снимок", lvItem.SubItems(3).Text) = 1 OrElse Val(lvItem.SubItems(3).Text) = 1 Then
                lvItem.Selected = False
            Else
                lvItem.Selected = True
            End If
        Next
        ListViewAllSnapshot.Select()
    End Sub
#End Region

#Region "ВыборСнимков"
    ''' <summary>
    ''' Заполнить форму испытаниями
    ''' </summary>
    Private Sub PopulateTreeViewEngine()
        Dim progressCount As Integer = 0
        Dim nameChannelsTable As String = String.Empty

        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        TSProgressBar.Visible = True ' открыть  Progressbar.

        ListViewAllSnapshot.Items.Clear()
        TreeViewAllSnapshot.BeginUpdate()
        TreeViewAllSnapshot.Nodes.Clear()

        Dim newRootNode As New TreeNode() With {.Name = "Root", .Text = "Изделия", .Tag = "Root", .ImageIndex = NodeImage.Close0}
        TreeViewAllSnapshot.Nodes.Add(newRootNode)

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text

            Using odaDataAdapter As New OleDbDataAdapter("SELECT DISTINCT БазаСнимков.НомерИзделия FROM БазаСнимков ORDER BY НомерИзделия", cn)
                Using dtDataTable As New DataTable
                    odaDataAdapter.Fill(dtDataTable)
                    Dim rowsCount As Integer = dtDataTable.Rows.Count

                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        TSProgressBar.Value = CInt(CDbl(progressCount / rowsCount) * 100)
                        ' Идентифицирует таблицу.
                        Dim newNodeEngine As New TreeNode(CStr(itemDataRow("НомерИзделия"))) With {.Tag = 0 & "Изделия", .ImageIndex = NodeImage.Close0}
                        newRootNode.Nodes.Add(newNodeEngine)
                        'indexNodeEngine = mNode.Index
                        cmd.CommandText = $"SELECT * FROM [БазаСнимков] WHERE НомерИзделия = {itemDataRow("НомерИзделия")} ORDER BY БазаСнимков.KeyID"
                        Using rdr As OleDbDataReader = cmd.ExecuteReader
                            Do While rdr.Read
                                ' Идентифицирует таблицу.
                                Dim newNodeSnapshot As New TreeNode($"Дата:{Convert.ToDateTime(rdr("Дата")).ToShortDateString} Время:{Convert.ToDateTime(rdr("ВремяНачалаСбора")).ToShortTimeString} Режим:{rdr("Режим")} {rdr("Примечание")}") With {.Tag = CStr(rdr("KeyID")) & "Снимок"}
                                If CStr(rdr("Режим")) = cРегистратор Then
                                    newNodeSnapshot.ImageIndex = NodeImage.GraphRegistration3
                                Else
                                    newNodeSnapshot.ImageIndex = NodeImage.GraphSnapshot2
                                End If
                                newNodeEngine.Nodes.Add(newNodeSnapshot)
                                If nameChannelsTable = String.Empty Then nameChannelsTable = CStr(rdr("ТаблицаКаналов"))
                            Loop 'Переместить на следующую запись Изделия Потомок.
                        End Using
                        progressCount += 1
                    Next ' Переместить на следующую запись Изделия.
                End Using
            End Using
        End Using

        TSProgressBar.Visible = False
        ' раскрыть высший узел.
        TreeViewAllSnapshot.EndUpdate()
        TreeViewAllSnapshot.Nodes(0).Expand()
        Windows.Forms.Cursor.Current = Cursors.Default
        LoadChannelsFromDBTable(nameChannelsTable)
    End Sub

    ''' <summary>
    ''' Обновить снимки по изделии
    ''' </summary>
    ''' <param name="pubID"></param>
    Private Sub PopulateAllSnapshotByEngine(ByRef pubID As Integer)
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT COUNT(*) FROM  БазаСнимков Where БазаСнимков.НомерИзделия = " & pubID.ToString
            cn.Open()
            Dim snapshotsCount As Integer = CInt(cmd.ExecuteScalar)

            cmd.CommandType = CommandType.Text
            cmd.CommandText = $"SELECT БазаСнимков.KeyID, БазаСнимков.НомерИзделия, БазаСнимков.Дата, БазаСнимков.ВремяНачалаСбора,БазаСнимков.Режим, БазаСнимков.Примечание From БазаСнимков Where ((БазаСнимков.НомерИзделия) = {pubID}) ORDER BY БазаСнимков.KeyID"
            Using rdr As OleDbDataReader = cmd.ExecuteReader
                TSProgressBar.Visible = True
                ' Очистите старые заглавия
                ListViewAllSnapshot.BeginUpdate()
                ListViewAllSnapshot.Items.Clear()
                Dim percentComplete As Integer = 1

                Do While rdr.Read
                    'If percentComplete Mod 5 = 0 Then 
                    TSProgressBar.Value = CInt(CDbl(percentComplete / snapshotsCount) * 100)
                    Dim newListViewItem As ListViewItem
                    If CStr(rdr("Режим")) = cРегистратор Then
                        newListViewItem = New ListViewItem(Convert.ToDateTime(rdr("Дата")).ToShortDateString, 3)
                    Else
                        newListViewItem = New ListViewItem(Convert.ToDateTime(rdr("Дата")).ToShortDateString, 2)
                    End If

                    newListViewItem.Tag = CStr(rdr("KeyID")) & " ID"
                    newListViewItem.SubItems.Add(Convert.ToDateTime(rdr("ВремяНачалаСбора")).ToShortTimeString) ' 1
                    newListViewItem.SubItems.Add(CStr(rdr("Режим")))   ' 2

                    If Not IsDBNull(rdr("Примечание")) Then
                        If CBool(InStr(1, CStr(rdr("Примечание")), "Фоновая запись ")) Then
                            newListViewItem.SubItems.Add(VB.Right(CStr(rdr("Примечание")), Len(rdr("Примечание")) - 15))  ' 3
                        Else
                            newListViewItem.SubItems.Add(CStr(rdr("Примечание")))
                            'ElseIf InStr(1, rdr("Примечание"), "Объединённая") Then
                        End If
                    End If

                    ListViewAllSnapshot.Items.Add(newListViewItem)
                    percentComplete += 1
                Loop
            End Using
        End Using
        ListViewAllSnapshot.EndUpdate()
        TSProgressBar.Visible = False
    End Sub

    Private Sub SplitContainerEngine_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainerEngine.SplitterMoved
        FormResize()
    End Sub

    Private Sub FormResize()
        If isFormLoaded Then
            LabelSelectFolder.Width = TreeViewAllSnapshot.Width - 2
            With ListViewAllSnapshot
                LabbelSelectSnapshot.Width = .Width - 2
                .Columns.Item(0).Width = .Width * 4 \ 20
                .Columns.Item(1).Width = .Width * 3 \ 20
                .Columns.Item(2).Width = .Width * 9 \ 20 - 4
                .Columns.Item(3).Width = .Width * 4 \ 20
            End With
            SetSizePanels()
        End If
    End Sub

    ''' <summary>
    ''' Установить Колонки Изделия
    ''' </summary>
    Private Sub SetWidhtColumnsListViewAllSnapshot()
        With ListViewAllSnapshot
            .Columns.Add("Дата", .Width * 4 \ 20, HorizontalAlignment.Left)
            .Columns.Add("Время", .Width * 3 \ 20, HorizontalAlignment.Left)
            .Columns.Add("Режим", .Width * 9 \ 20 - 4, HorizontalAlignment.Left)
            .Columns.Add("Запись", .Width * 4 \ 20, HorizontalAlignment.Left)
        End With
    End Sub

    Private Sub TreeViewAllSnapshot_BeforeCollapse(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewAllSnapshot.BeforeCollapse
        e.Node.ImageIndex = NodeImage.Close0
        ListViewAllSnapshot.Items.Clear()
    End Sub

    Private Sub TreeViewAllSnapshot_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewAllSnapshot.BeforeExpand
        For Each itemNode As TreeNode In TreeViewAllSnapshot.Nodes("Root").Nodes
            If itemNode.IsExpanded Then itemNode.Collapse()
        Next
    End Sub

    Private Sub TreeViewAllSnapshot_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewAllSnapshot.AfterExpand
        e.Node.ImageIndex = NodeImage.Open1
        If CBool(InStr(e.Node.Tag.ToString, "Изделия")) Then
            numberEngine = CInt(e.Node.Text)
            PopulateAllSnapshotByEngine(numberEngine)
            'ReDim_IndexesAbsentParameters(1) ' чтобы на записях нового изделия не отражались прежние индексы
            Re.Dim(IndexesAbsentParameters, 1) ' чтобы на записях нового изделия не отражались прежние индексы
            ButtonFind.Enabled = False
        End If
    End Sub

    Private Sub TreeViewAllSnapshot_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewAllSnapshot.AfterSelect
        If CBool(InStr(e.Node.Tag.ToString, "Снимок")) Then
            numberEngine = CInt(e.Node.Parent.Text)
        ElseIf CBool(InStr(e.Node.Tag.ToString, "Изделия")) Then
            If Not e.Node.IsExpanded Then e.Node.Expand()
            numberEngine = CInt(e.Node.Text)
        End If
    End Sub

    ''' <summary>
    ''' Загрузка каналов из базы и заполнение
    ''' arrTypeNameUnit, ComboBoxValueParameters и ComboBoxPatternParameters
    ''' </summary>
    Private Sub LoadChannelsFromDBTable(nameChannelsTable As String)
        If nameChannelsTable = String.Empty Then
            Const caption As String = "Загрузка каналов из снимков"
            Const text As String = "База снимков не содержит записи испытаний или отсутствует таблица каналов."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand

            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT COUNT(*) FROM " & nameChannelsTable
            cn.Open()
            'ReDim_arrTypeNameUnit(CInt(cmd.ExecuteScalar))
            Re.Dim(arrTypeNameUnit, CInt(cmd.ExecuteScalar))
            cmd.CommandText = "SELECT * FROM " & nameChannelsTable

            Using rdr As OleDbDataReader = cmd.ExecuteReader
                ' загрузка коэффициентов по параметрам с базы с помощью запроса
                ' при добавлении полей надо модифицировать запрос в базе
                Do While rdr.Read
                    Dim numberParam As Integer = Convert.ToInt32(rdr("НомерПараметра"))
                    arrTypeNameUnit(numberParam).NameParameter = CStr(rdr("НаименованиеПараметра"))
                    arrTypeNameUnit(numberParam).UnitMeasure = CStr(rdr("ЕдиницаИзмерения"))
                Loop
            End Using
        End Using
        FillListParametersOnComboBox(ComboBoxValueParameters)
        FillListParametersOnComboBox(ComboBoxPatternParameters)
    End Sub

    ''' <summary>
    ''' Заполнить ComboBox объектми StringIntObject
    ''' </summary>
    ''' <param name="cmb"></param>
    Private Sub FillListParametersOnComboBox(ByRef cmb As ComboBox)
        ' "Обороты", 0 -    '1"%"
        ' "Дискретные", 1 - '2"дел"
        ' "Разрежения", 2 - '3"мм"
        ' "Температуры", 3 - '4"градус"
        ' "Давления", 4 -   '5"Кгсм"
        ' "Токи", 5 -       '7"мкА"
        ' "Вибрация", 6 -   '6"мм/с"
        ' "Расходы", 7 -    '8"кг/ч"
        ' "Тяга", 8 -       '9"кгс"
        cmb.Items.Clear()
        cmb.Items.Add(New StringIntObject(MissingParameter, 12)) 'Close12

        For I As Integer = 1 To UBound(arrTypeNameUnit)
            If CBool(InStr(1, UnitOfMeasureString, arrTypeNameUnit(I).UnitMeasure)) Then
                cmb.Items.Add(New StringIntObject(arrTypeNameUnit(I).NameParameter, Array.IndexOf(UnitOfMeasureArray, arrTypeNameUnit(I).UnitMeasure)))
            Else
                cmb.Items.Add(New StringIntObject(arrTypeNameUnit(I).NameParameter, 1)) 'Discrete1
            End If
        Next

        cmb.SelectedIndex = 0
    End Sub
#End Region

#Region "Дерево"
    Private Sub ButtonLoadCondition_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadCondition.Click
        LoadCondition()
    End Sub

    Private Sub ButtonSaveCondition_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveCondition.Click
        SaveConditionsXmlDoc()
    End Sub

    Private Sub ButtonDeleteCondition_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteCondition.Click
        RemoveEditingCondition()
        XDoc.Save(PathXmlFileCondition)
        LoadConditionsXmlDoc()
        TextBoxNameConfiguration.Text = ""
        ButtonLoadCondition.Enabled = False
        ButtonSaveCondition.Enabled = False
        ButtonDeleteCondition.Enabled = False
    End Sub

    ''' <summary>
    ''' Загрузка созданной конфигурации поиска кадров по условию
    ''' </summary>
    Private Sub LoadCondition()
        Dim findingConditionsList As IEnumerable(Of XElement) = From el In XDoc.Root...<ЧтоЗаУсловие>
                                                                Where el.@Примечание = TextBoxNameConfiguration.Text
                                                                Select el
        If findingConditionsList.Any Then
            ' очистить закладки, очищать до первой все время удаляя второй элемент в коллекции
            For J As Integer = 1 To TabControlAll.Controls.Count - 1
                TabControlAll.Controls.RemoveAt(1)
            Next

            Conditions.Clear()
            pagesCount = 0
            ' разбор узла <ЧтоЗаУсловие>
            Dim conditionsXElementList As IEnumerable(Of XElement) = From condition In findingConditionsList(0).<ДочернееУсловиеN>
                                                                     Select condition
            For Each itemConditionXElement As XElement In conditionsXElementList
                Dim nodesXElement As IEnumerable(Of XElement) = itemConditionXElement.Elements()
                pagesCount += 1
                ' создать и извлечь
                Dim newCondition As Condition = Conditions.Add(pagesCount)
                With newCondition
                    ' анализ значения атрибута 
                    If itemConditionXElement.Attribute("ТипЗакладки").Value = cTriggerValue Then
                        ' Триггер
                        .MarkerProperty = Marker.Trigger
                        ' Условие Отбора
                        For Each itemNode As XElement In nodesXElement
                            .SetTriggerConditionFromXmlAttribute(itemNode.Name.ToString, itemNode.Value)
                        Next
                    Else ' Шаблон
                        .MarkerProperty = Marker.Template
                        .ConditionTypeProperty = ConditionType.Template

                        For Each itemNode As XElement In nodesXElement
                            .SetTemplateConditionFromXmlAttribute(itemNode.Name.ToString, itemNode.Value)
                        Next
                    End If

                    If pagesCount = 1 Then
                        PopulateControlsWithNewCondition(newCondition)
                    Else
                        AddTabPage()
                    End If
                End With
            Next
        End If

        ButtonSaveCondition.Enabled = True
        ButtonDeleteCondition.Enabled = True
    End Sub

    ''' <summary>
    ''' Заполнить контроля вкладки значениями нового Условия
    ''' </summary>
    ''' <param name="refCondition"></param>
    Private Sub PopulateControlsWithNewCondition(ByRef refCondition As Condition)
        With refCondition
            ' чтобы отобразилось на экране
            NumericTimeWindow.Value = .WidthTemporaryWindow
            TabControlAll.SelectedIndex = 0
            PageConditionSelect = TabControlAll.SelectedTab
            TabConditionSelect = CType(PageConditionSelect.Controls(0), TabControl)

            ' Триггер
            If IfExistNameParameter(.NameValueParameter) Then
                ComboBoxValueParameters.Text = .NameValueParameter
            Else
                ComboBoxValueParameters.SelectedIndex = 0
            End If

            ComboBoxEnumConditions.SelectedIndex = .SelectiveConditionProperty
            TextBoxStart.Text = .LowerValueCondition.ToString
            TextBoxEnd.Text = .UpperValueCondition.ToString
            TextBoxValue.Text = .Condition.ToString

            ' Шаблон
            If IfExistNameParameter(.NameFormParameter) Then
                ComboBoxPatternParameters.Text = .NameFormParameter
            Else
                ComboBoxPatternParameters.SelectedIndex = 0
            End If

            NumericValueMax.Value = .MaxLimitParameter

            NumericSlideValueStartUp.Value = .StartUpper
            SlideValueStartUp.Value = .StartUpper

            NumericSlideValueStartDown.Value = .StartLover
            SlideValueStartDown.Value = .StartLover

            NumericSlideValueMiddleUp.Value = .TwistTop
            SlideValueMiddleUp.Value = .TwistTop

            NumericSlideValueMiddleDown.Value = .TwistLower
            SlideValueMiddleDown.Value = .TwistLower

            NumericSlideValueFinishUp.Value = .FinishTop
            SlideValueFinishUp.Value = .FinishTop

            NumericSlideValueFinishDown.Value = .FinishLower
            SlideValueFinishDown.Value = .FinishLower

            NumericTime.Value = .TimeTwist
            SlideTime.Value = .TimeTwist

            NumericTimeWindow.Value = .WidthTemporaryWindow
            SlideTimeWindow.Value = .WidthTemporaryWindow

            TabControlAll.SelectedIndex = 0
            PageConditionSelect = TabControlAll.SelectedTab
            TabControlCondition.SelectedIndex = If(.MarkerProperty = Marker.Trigger, 0, 1)
            TabConditionSelect = CType(PageConditionSelect.Controls(0), TabControl)
        End With
    End Sub

    Private Function IfExistNameParameter(ByVal name As String) As Boolean
        For I As Integer = 1 To UBound(arrTypeNameUnit)
            If arrTypeNameUnit(I).NameParameter = name Then Return True
        Next
        Return False
    End Function

    ''' <summary>
    ''' Удаление выбранного Условия
    ''' </summary>
    Private Sub RemoveEditingCondition()
        If TextBoxNameConfiguration.Text <> "" Then
            '--- удалить существующий элемент -------------------------------------
            Dim removingList As IEnumerable(Of XElement) = From el In XDoc.Root...<ЧтоЗаУсловие>
                                                           Where el.@Примечание = TextBoxNameConfiguration.Text
                                                           Select el
            If removingList.Any Then
                Try
                    removingList(0).Remove()
                    ' то же самое
                    'XDoc.Root.Elements.Where(Function(el) el.FirstAttribute = TextBoxNameConfiguration.Text).First.Remove()
                Catch ex As Exception
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, NameOf(RemoveEditingCondition), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{NameOf(RemoveEditingCondition)}> {text}")
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Запись созданной конфигурации поиска кадров по условию
    ''' </summary>
    Private Sub SaveConditionsXmlDoc()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonSaveCondition_Click)} -> {NameOf(SaveConditionsXmlDoc)}> запись созданной конфигурации поиска кадров по условию")
        ' для того чтобы редактируемые поля отразились в коллекции условий
        ' могло быть редактирование на закладке без смены и последующей записи условия 
        ' и чтобы изменения условия не записалось в коллекцию
        StorageConditionOnPageChanged(indexCurrentPage)

        ' если имя пусто вопрос  "введите имя" и так как модально нельзя то выход из данной процедуры
        If TextBoxNameConfiguration.Text <> "" Then
            ' если переписать, то удаление из базы старых и запись новых
            ' или если имя новое, то новое Условие добавляется            
            RemoveEditingCondition()
            XDoc.Root.Add(Conditions.GetXElement(TextBoxNameConfiguration.Text))
            XDoc.Save(PathXmlFileCondition)
            LoadConditionsXmlDoc()
        Else 'надо ввести имя
            Const caption As String = "Новое условие"
            Const text As String = "Необходимо ввести имя условия!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub
#End Region

#Region "ToolTip"
    ''' <summary>
    ''' Сообщение На Панель
    ''' </summary>
    ''' <param name="strMessage"></param>
    Private Sub SetMessageToStatusPanel(ByVal strMessage As String)
        TSLabelMessage.Text = strMessage
    End Sub

    Private Sub Controls_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles _
        ButtonBackToStep2.MouseLeave,
        ButtonExit.MouseLeave,
        ToolStripSteps.MouseLeave,
        ButtonForwardToStep2.MouseLeave,
        ListViewAllSnapshot.MouseLeave,
        ComboBoxPatternParameters.MouseLeave,
        ComboBoxValueParameters.MouseLeave,
        ComboBoxEnumConditions.MouseLeave,
        TextBoxValue.MouseLeave,
        TextBoxEnd.MouseLeave,
        TextBoxStart.MouseLeave,
        ButtonBackToStep1.MouseLeave,
        ButtonAdd.MouseLeave,
        ButtonDelete.MouseLeave,
        ButtonFind.MouseLeave,
        ButtonCancel.MouseLeave,
        TextBoxNameConfiguration.MouseLeave,
        ButtonSaveCondition.MouseLeave,
        ButtonLoadCondition.MouseLeave,
        ButtonDeleteCondition.MouseLeave,
        ButtonPrevious.MouseLeave,
        ButtonNext.MouseLeave,
        ButtonSelectAll.MouseLeave,
        ButtonEraseSelect.MouseLeave

        ClearPanelMessage()
    End Sub

    Private Sub ClearPanelMessage()
        TSLabelMessage.Text = ""
    End Sub

    Private Sub ButtonBackToStep2_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonBackToStep2.MouseEnter
        SetMessageToStatusPanel("Возврат на шаг задания условий")
    End Sub

    Private Sub ButtonExit_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonExit.MouseEnter
        SetMessageToStatusPanel("Закрыть окно поиска кадров")
    End Sub

    Private Sub DataGridFoundSnapshot_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridFoundSnapshot.MouseEnter
        SetMessageToStatusPanel("Выбрать снимок для загрузки")
    End Sub

    Private Sub ToolStripSteps_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripSteps.MouseEnter
        SetMessageToStatusPanel("Выбор шага мастера поиска нужных кадров")
    End Sub

    Private Sub ButtonForwardToStep2_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonForwardToStep2.MouseEnter
        SetMessageToStatusPanel("Перейти на шаг задания условий")
    End Sub

    Private Sub ListViewAllSnapshot_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ListViewAllSnapshot.MouseEnter
        SetMessageToStatusPanel("Выделить снимки для поиска по условиям")
    End Sub

    Private Sub ComboBoxPatternParameters_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxPatternParameters.MouseEnter
        SetMessageToStatusPanel("Выбор параметра для задания условия")
    End Sub

    Private Sub ComboBoxValueParameters_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxValueParameters.MouseEnter
        SetMessageToStatusPanel("Выбор параметра для задания условия")
    End Sub

    Private Sub ComboBoxEnumConditions_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxEnumConditions.MouseEnter
        SetMessageToStatusPanel("Выбор типа условия")
    End Sub

    Private Sub TextBoxValue_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxValue.MouseEnter
        SetMessageToStatusPanel("Ввод значения для ограничения")
    End Sub

    Private Sub TextBoxEnd_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxEnd.MouseEnter
        SetMessageToStatusPanel("Ввод конечного значения")
    End Sub

    Private Sub TextBoxStart_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxStart.MouseEnter
        SetMessageToStatusPanel("Ввод начального значения")
    End Sub

    Private Sub ButtonBackToStep1_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonBackToStep1.MouseEnter
        SetMessageToStatusPanel("Возврат на шаг выбора снимков")
    End Sub

    Private Sub ButtonAdd_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAdd.MouseEnter
        SetMessageToStatusPanel("Добавляет закладку с шаблоном нового условия")
    End Sub

    Private Sub ButtonDelete_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDelete.MouseEnter
        SetMessageToStatusPanel("Удалить ненужные условия")
    End Sub

    Private Sub ButtonFind_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFind.MouseEnter
        SetMessageToStatusPanel("Запуск процесса поиска снимков")
    End Sub

    Private Sub ButtonCancel_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCancel.MouseEnter
        SetMessageToStatusPanel("Прервать процесс поиска снимков")
    End Sub

    Private Sub TextBoxNameConfiguration_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameConfiguration.MouseEnter
        SetMessageToStatusPanel("Имя текущей перекладки или ввод нового имени")
    End Sub

    Private Sub ButtonSaveCondition_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveCondition.MouseEnter
        SetMessageToStatusPanel("Запись условия с заданным именем")
    End Sub

    Private Sub ButtonLoadCondition_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadCondition.MouseEnter
        SetMessageToStatusPanel("Считать условие с заданным именем")
    End Sub

    Private Sub ButtonDeleteCondition_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteCondition.MouseEnter
        SetMessageToStatusPanel("Удалить условие с заданным именем")
    End Sub

    Private Sub ButtonPrevious_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPrevious.MouseEnter
        SetMessageToStatusPanel("Перейти к предыдущему снимку")
    End Sub

    Private Sub ButtonNext_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonNext.MouseEnter
        SetMessageToStatusPanel("Перейти к следующему снимку")
    End Sub

    Private Sub ButtonSelectAll_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSelectAll.MouseEnter
        SetMessageToStatusPanel("Пометить все записи изделия для поиска")
    End Sub

    Private Sub ButtonСнятьВыделение_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEraseSelect.MouseEnter
        SetMessageToStatusPanel("Снять выделение с кадров запуска и прерванных снимков")
    End Sub
#End Region

#Region "XmlView работа с атрибутами"
    Private Sub ButtonsTreeEnable(isEnabled As Boolean)
        ButtonLoadCondition.Enabled = isEnabled
        ButtonSaveCondition.Enabled = isEnabled
        ButtonDeleteCondition.Enabled = isEnabled
    End Sub

    Private Sub XmlView_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles XmlTreeView.AfterCollapse
        TextBoxNameConfiguration.Text = ""
        ButtonsTreeEnable(False)

        If CBool(InStr(e.Node.Tag.ToString, cWhatIsCondition)) Then
            e.Node.ImageIndex = XmlImage.Conditions2
        End If
    End Sub

    Private Sub XmlView_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles XmlTreeView.BeforeExpand
        ' Свернуть все узлы, кроме родительского
        For Each itemTreeNode As TreeNode In XmlTreeView.Nodes(0).Nodes
            If itemTreeNode.IsExpanded AndAlso Not (itemTreeNode Is e.Node.Parent) Then
                itemTreeNode.Collapse()
                e.Node.ImageIndex = XmlImage.Conditions2
            End If
        Next
    End Sub

    Private Sub XmlView_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles XmlTreeView.AfterExpand
        If CBool(InStr(CStr(e.Node.Tag), cWhatIsCondition)) Then
            e.Node.ImageIndex = XmlImage.Select0
            TextBoxNameConfiguration.Text = e.Node.Text
            ButtonsTreeEnable(True)
        End If
    End Sub

    Private Sub TextBoxNameConfiguration_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameConfiguration.TextChanged
        ButtonSaveCondition.Enabled = True
    End Sub

    ''' <summary>
    ''' Загрузить XML и парсить в дерево
    ''' </summary>
    Public Sub LoadConditionsXmlDoc()
        'Dim XmlFile As String
        'Dim directoryInfo As System.IO.DirectoryInfo
        'Dim directoryXML As System.IO.DirectoryInfo

        ''получить каталог установки приложения
        'directoryInfo = System.IO.Directory.GetParent(Application.StartupPath)

        ''установить выходной путь
        'If directoryInfo.Name.ToString() = "bin" Then
        '    directoryXML = System.IO.Directory.GetParent(directoryInfo.FullName)
        '    XmlFile = directoryXML.FullName + "\Условия.xml"
        'Else
        '    XmlFile = directoryInfo.FullName + "\Условия.xml"
        'End If

        'fileName.Text = XmlFile
        ''загрузить xml файл внутрь XmlTextReader объекта.
        'Dim XmlRdr = New System.Xml.XmlTextReader(XmlFile)
        ''пермещаться вдоль xml документа.
        'While XmlRdr.Read()
        '    'проверить тип узла и тип элемента с проверкой имени свойства
        '    If XmlRdr.NodeType = XmlNodeType.Element AndAlso XmlRdr.Name = "name" Then
        '        'XMLOutput.Text += XmlRdr.ReadString() + vbCr + vbLf
        '    End If
        'End While

        Try
            XDoc = XDocument.Load(PathXmlFileCondition)
            XmlTreeView.Nodes.Clear()
            AddNodeAndChildren(XDoc.Root, Nothing)
        Catch ex As Exception
            Dim text As String = ex.ToString
            MessageBox.Show(text, NameOf(LoadConditionsXmlDoc), MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{NameOf(LoadConditionsXmlDoc)}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Рекурсивное добавление узлов в дерево проводника в зависимости от узлов XmlNode документа
    ''' </summary>
    ''' <param name="xnode"></param>
    ''' <param name="tnode"></param>
    Private Sub AddNodeAndChildren(ByVal xnode As XElement, ByVal tnode As TreeNode)
        Dim newChildTreeNode As TreeNode = AddNode(xnode, tnode)
        If xnode.HasElements Then
            For Each itemXElement As XElement In xnode.Elements
                AddNodeAndChildren(itemXElement, newChildTreeNode)
            Next
        End If
    End Sub

    Private Function AddNode(ByVal itemXElement As XElement, ByVal inTreeNode As TreeNode) As TreeNode
        Dim childNewTreeNode As TreeNode
        Dim treeNodes As TreeNodeCollection
        Dim nodeName As String = String.Empty
        Dim imageIndex As Integer

        If inTreeNode Is Nothing Then
            treeNodes = XmlTreeView.Nodes ' корневой Root
        Else
            treeNodes = inTreeNode.Nodes
        End If

        Select Case itemXElement.Name
            Case cTermCondition
                imageIndex = XmlImage.Root1
                nodeName = "Имеющиеся условия"
            Case cWhatIsCondition
                imageIndex = XmlImage.Conditions2
                nodeName = itemXElement.Attribute("Примечание").Value
            Case cChildConditionN
                imageIndex = XmlImage.Term3
                nodeName = itemXElement.Attribute("ИмяУсловия").Value
            Case Else
                If itemXElement.Nodes.Count > 0 Then
                    imageIndex = GetImageIndexFromXmlNodeName(itemXElement.Name.ToString)
                    nodeName = $"<{itemXElement.Name} {itemXElement.Value}>"
                End If
        End Select

        If itemXElement.Name = cWhatIsCondition Then
            childNewTreeNode = New TreeNode(nodeName, imageIndex, imageIndex) With {.Tag = cWhatIsCondition}
        Else
            childNewTreeNode = New TreeNode(nodeName, imageIndex, imageIndex)
        End If

        treeNodes.Add(childNewTreeNode)

        Return childNewTreeNode
    End Function

    ''' <summary>
    ''' Определить индекс иконки в зависимости от имени узла
    ''' </summary>
    ''' <param name="XmlNodeName"></param>
    ''' <returns></returns>
    Private Function GetImageIndexFromXmlNodeName(XmlNodeName As String) As Integer
        Dim imageIndex As Integer

        Select Case XmlNodeName
            ' --- Template --------------------------------
            Case cStartUp
                imageIndex = XmlImage.StartUp4
            Case cStartDown
                imageIndex = XmlImage.StartDown5
            Case cBendUp
                imageIndex = XmlImage.BendUp6
            Case cBendDown
                imageIndex = XmlImage.BendDown7
            Case cFinishUp
                imageIndex = XmlImage.FinishUp8
            Case cFinishDown
                imageIndex = XmlImage.FinishDown9
            Case cTimeBend
                imageIndex = XmlImage.TimeBend10
            Case cWidthWindow
                imageIndex = XmlImage.WidthWindow11
            Case cBetweenUpTrigger
                imageIndex = XmlImage.BetweenUpTrigger12
            Case cBetweenDownTrigger
                imageIndex = XmlImage.BetweenDownTrigger13
                ' --- Trigger --------------------------------
            Case cMaxLimit
                imageIndex = XmlImage.MaxLimit14
            Case cParameterTemplate
                imageIndex = XmlImage.ParameterTemplate15
            Case cConditionalTrigger
                imageIndex = XmlImage.ConditionalTrigger16
            Case cEqualTrigger
                imageIndex = XmlImage.EqualTrigger17
            Case cParameterTrigger
                imageIndex = XmlImage.ParameterTrigger18
        End Select

        Return imageIndex
    End Function
#End Region

#Region "Навигация"
    'Private Sub FormConditionFind_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    '    If e.KeyCode = Keys.Right Then NextRecord()
    '    If e.KeyCode = Keys.Left Then PreviousRecord()
    'End Sub

    Private Sub ButtonPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonPrevious.Click
        If BindingSourceFoundedSnapshotDataTable.Position - 1 < 0 Then
            'If DataGridFoundSnapshot.CurrentRow.Index - 1 < 0 Then
            MessageBox.Show("Конец списка!", "Навигация по списку", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ButtonPrevious.Enabled = False
        Else
            PreviousRecord()
            ButtonPrevious.Enabled = True
        End If

        ButtonNext.Enabled = True
    End Sub

    Private Sub ButtonNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonNext.Click
        If foundSnapshotDataTable Is Nothing Then Exit Sub
        If BindingSourceFoundedSnapshotDataTable.Position + 1 > BindingSourceFoundedSnapshotDataTable.Count - 1 Then
            'If DataGridFoundSnapshot.CurrentRow.Index + 1 > foundSnapshotDataTable.Rows.Count - 1 Then
            MessageBox.Show("Конец списка!", "Навигация по списку", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ButtonNext.Enabled = False
        Else
            NextRecord()
            ButtonNext.Enabled = True
        End If

        ButtonPrevious.Enabled = True
    End Sub

    ''' <summary>
    ''' Загрузить График Фонового Снимка
    ''' </summary>
    Private Sub LoadFrameBackgraundSnapshot()
        If foundSnapshotDataTable Is Nothing Then Exit Sub

        ShowCurrentRecordNumber()
        ParentFormSnapshot.ShowFrameSnapshotFromDBase(CInt(DataGridFoundSnapshot.Item(0, DataGridFoundSnapshot.CurrentCell.RowIndex).Value)) ' загрузить снимок по RowNumber->ColumnIndex(0)-> keyID
        Dim selectCondition As FrameCondition = FramesOk(DataGridFoundSnapshot.CurrentRow.Index)
        mFrequencySnapshot = selectCondition.Frequency
        mCurrentPosition = CInt(selectCondition.TimeConditionIsOk * selectCondition.Frequency)
        ParentFormSnapshot.AddMarkingFrameFoundCondition(selectCondition.Title)
    End Sub

    Private Sub DataGridFoundSnapshot_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridFoundSnapshot.SelectionChanged
        If BindingSourceFoundedSnapshotDataTable.DataSource Is Nothing Then Exit Sub ' убрать при срабатывании, когда источник данных равен Nothing
        ButtonNext.Enabled = True
        ButtonPrevious.Enabled = True
        'BindingSourceFoundedSnapshotDataTable.Position = DataGridFoundSnapshot.CurrentRow.Index' лишнее
        LoadFrameBackgraundSnapshot()
    End Sub

    Private Sub DataGridFoundSnapshot_Click(sender As Object, e As EventArgs) Handles DataGridFoundSnapshot.Click
        ' отрабатывать щелчок только когда одна строка
        If BindingSourceFoundedSnapshotDataTable.Count = 1 Then
            Try
                'DataGridFoundSnapshot.Rows(DataGridFoundSnapshot.CurrentRow.Index).Selected = True
                DataGridFoundSnapshot.Rows(BindingSourceFoundedSnapshotDataTable.Position).Selected = True
                LoadFrameBackgraundSnapshot()
            Catch ex As Exception
                Const caption As String = "Навигация"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
        End If
    End Sub

    Private Sub ButtonFindChannelPattern_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelPattern.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxPatternParameters)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub ButtonFindChanneValue_Click(sender As Object, e As EventArgs) Handles ButtonFindChanneValue.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxValueParameters)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub PreviousRecord()
        Try
            If BindingSourceFoundedSnapshotDataTable.Count > 0 Then
                If BindingSourceFoundedSnapshotDataTable.Position > 0 Then
                    BindingSourceFoundedSnapshotDataTable.MovePrevious()
                Else
                    Beep()
                End If
            End If

            ShowCurrentRecordNumber()
        Catch ex As Exception
            Const caption As String = "Навигация"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub NextRecord()
        Try
            If BindingSourceFoundedSnapshotDataTable.Count > 0 Then
                If BindingSourceFoundedSnapshotDataTable.Position + 1 < BindingSourceFoundedSnapshotDataTable.Count Then
                    BindingSourceFoundedSnapshotDataTable.MoveNext()
                Else
                    Beep()
                End If
            End If

            ShowCurrentRecordNumber()
        Catch ex As Exception
            Const caption As String = "Навигация"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub ShowCurrentRecordNumber()
        LabelPosition.Text = $"Запись №: {BindingSourceFoundedSnapshotDataTable.Position + 1} из {BindingSourceFoundedSnapshotDataTable.Count}"
        RegistrationEventLog.EventLog_MSG_USER_ACTION(LabelPosition.Text)
    End Sub
#End Region

#Region "Function"
    Public Shared Function ConvertSelectiveConditionToString(ByVal condition As SelectiveCondition) As String
        Select Case condition
            Case SelectiveCondition.Between
                Return cBetween
            Case SelectiveCondition.OutOfRange
                Return cOutOfRange
            Case SelectiveCondition.Equal
                Return cEqual
            Case SelectiveCondition.NotEqual
                Return cNotEqual
            Case SelectiveCondition.Greater
                Return cGreater
            Case SelectiveCondition.Less
                Return cLess
            Case SelectiveCondition.GreaterThanOrEqual
                Return cGreaterThanOrEqual
            Case SelectiveCondition.LessThanOrEqual
                Return cLessThanOrEqual
            Case SelectiveCondition.FindMaximum
                Return cFindMaximum
            Case SelectiveCondition.FindMinimum
                Return cFindMinimum
            Case Else
                Return vbNullString
        End Select
    End Function

    Public Function ConvertStringToEnumSelectiveCondition(ByVal strEnumCondition As String) As SelectiveCondition
        Select Case strEnumCondition
            Case cBetween
                Return SelectiveCondition.Between
            Case cOutOfRange
                Return SelectiveCondition.OutOfRange
            Case cEqual
                Return SelectiveCondition.Equal
            Case cNotEqual
                Return SelectiveCondition.NotEqual
            Case cGreater
                Return SelectiveCondition.Greater
            Case cLess
                Return SelectiveCondition.Less
            Case cGreaterThanOrEqual
                Return SelectiveCondition.GreaterThanOrEqual
            Case cLessThanOrEqual
                Return SelectiveCondition.LessThanOrEqual
            Case cFindMaximum
                Return SelectiveCondition.FindMaximum
            Case cFindMinimum
                Return SelectiveCondition.FindMinimum
        End Select
    End Function

    ''' <summary>
    ''' Распаковка строки конфигурации в массив имён
    ''' </summary>
    Private Sub UnpackStringConfiguration(ByVal configurationString As String)
        Dim count As Integer
        Dim start As Integer = 1
        Dim lenghtString As Integer = Len(configurationString)

        'ReDim_parameterNamesConfiguration(arrTypeNameUnit.Length) ' вначале определить массив максимально возможного размера
        Re.Dim(parameterNamesConfiguration, arrTypeNameUnit.Length) ' вначале определить массив максимально возможного размера
        Do
            parameterNamesConfiguration(count) = Mid(configurationString, start, InStr(start, configurationString, Separator) - start)
            start = InStr(start, configurationString, Separator) + 1
            count += 1
        Loop While start < lenghtString

        'ReDimPreserve parameterNamesConfiguration(count - 1) ' отсечь всё лишнее
        Re.DimPreserve(parameterNamesConfiguration, count - 1)
    End Sub
#End Region

    'Public Shared Function EnumNamedValues(Of T As [Enum])() As Dictionary(Of Integer, String)
    '    Dim result = New Dictionary(Of Integer, String)()
    '    Dim values = [Enum].GetValues(GetType(T))

    '    For Each item As Integer In values
    '        result.Add(item, [Enum].GetName(GetType(T), item))
    '    Next

    '    Return result
    'End Function

    'Private Sub SurroundingSub()
    '    Dim map = EnumNamedValues(Of SelectiveCondition)()

    '    For Each pair In map
    '        Console.WriteLine($"{pair.Key}:\t{pair.Value}")
    '    Next
    'End Sub
End Class

'Public Shared Function SplineInterpolant( _   ByVal inputXData As Double(), _   ByVal inputYData As Double(), _   ByVal initialBoundary As Double, _   ByVal finalBoundary As Double _) As Double()
'Parameters
'inputXData
'The known x-values for the planar function. 
'inputYData
'The known y-values for the planar function. 
'initialBoundary
'The first derivative of the interpolant at the first element of inputXData. 
'finalBoundary
'The first derivative of the interpolant at the last element of inputXData. 
'Return Value
'The second derivatives to be used in a cubic spline interpolation. This array can be used with the method SplineInterpolation to calculate an interpolation value. 


'Public Shared Function SplineInterpolation( _   ByVal inputXData As Double(), _   ByVal inputYData As Double(), _   ByVal secondDerivatives As Double(), _   ByVal xValue As Double _) As Double
'Parameters
'inputXData
'The known x-values for the planar function. 
'inputYData
'The known y-values for the planar function. 
'secondDerivatives
'The array of second derivatives which specify the interpolant. This array should be generated by the SplineInterpolant method. 
'xValue
'The x-value at which the tabulated function is to be interpolated. 
'Return Value
'An estimate of the y-value corresponding to the given xValue for the tabulated function. 

'Private Sub updateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
'    Try
'        ' Initialization.
'        Dim mean As Double
'        Dim coeffArray() As Double
'        Dim samples As Integer = max + 1 'samplesNumeric.Value
'        Dim xArray() As Double = New Double(samples - 1) {}
'        Dim dataArray() As Double = New Double(samples - 1) {}
'        Dim fittedArray() As Double = New Double(samples - 1) {}
'        Dim functionGen As NationalInstruments.Analysis.SignalGeneration.BasicFunctionGenerator = New BasicFunctionGenerator(BasicFunctionGeneratorSignal.Sine, 2.0 / samples, BasicFunctionGenerator.DefaultAmplitude, BasicFunctionGenerator.DefaultPhase, BasicFunctionGenerator.DefaultOffset, 1.0, samples)

'        ' Generate the sine wave and the fitted plot.
'        xArray = PatternGeneration.Ramp(samples, 0, samples - 1)
'        dataArray = functionGen.Generate()
'        fittedArray = CurveFit.PolynomialFit(xArray, dataArray, 2, coeffArray, mean)

'        ' Plot the data on the graph.
'        'dataPlot.PlotXY(xArray, dataArray)
'        'fittedPlot.PlotXY(xArray, fittedArray)

'        GraphSignal1.PlotY(fittedArray, 0, 1)
'    Catch ex As Exception
'MessageBox.Show(exception.Message)
'    End Try
'End Sub
