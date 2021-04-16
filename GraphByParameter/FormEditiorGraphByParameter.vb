Option Strict Off

Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Xml.Linq
Imports MathematicalLibrary
Imports Registration.FormConditionFind

''' <summary>
''' Редактор графиков параметр от параметра
''' </summary>
Friend Class FormEditiorGraphByParameter
    Public Property ParentFormMain() As FormMain
    Private arrTypeNameUnit() As TypeNameUnit

#Region "Enum"
    ''' <summary>
    ''' Иконка узла условий
    ''' </summary>
    Private Enum XmlImage
        ''' <summary>
        ''' Корневой, XmlAttributeDescription
        ''' </summary>
        Root0 = 0
        ''' <summary>
        ''' Имеющиеся Графики
        ''' </summary>
        Description1 = 1
        ''' <summary>
        ''' XmlNodeNameGraph
        ''' </summary>
        NameGraph2 = 2
        ''' <summary>
        ''' XmlNodeAxis XmlNodeParameter XmlNodePoint
        ''' </summary>
        AxisParameterPoint3 = 3
        ''' <summary>
        ''' XmlAttributeAxisName XmlAttributeName XmlAttributeNameTU
        ''' </summary>
        Name4 = 4
        ''' <summary>
        ''' XmlAttributeMax
        ''' </summary>
        Max5 = 5
        ''' <summary>
        ''' XmlAttributeMin
        ''' </summary>
        Min6 = 6
        ''' <summary>
        ''' XmlAttributeIsUseFormula	XmlAttributeReduce
        ''' </summary>
        UseFormulaReduce7 = 7
        ''' <summary>
        ''' XmlNodeFormula
        ''' </summary>
        Formula8 = 8
        ''' <summary>
        ''' XmlAttributeIndex
        ''' </summary>
        Index9 = 9
        ''' <summary>
        ''' XmlAttributeNameChannel
        ''' </summary>
        NameChannel10 = 10
        ''' <summary>
        ''' XmlAttributeIndexParameter
        ''' </summary>
        IndexParameter11 = 11
        ''' <summary>
        ''' 
        ''' </summary>
        Nothing12 = 12
        ''' <summary>
        ''' XmlNodeGraphTU
        ''' </summary>
        GraphTU13 = 13
        ''' <summary>
        ''' XmlAttributeColor
        ''' </summary>
        Color14 = 14
        ''' <summary>
        ''' XmlAttributeLineStyle
        ''' </summary>
        LineStyle15 = 15
        ''' <summary>
        ''' XmlAttributeX	XmlAttributeY
        ''' </summary>
        RulerXY16 = 16
    End Enum
#End Region

    Private NameParameters() As String
    Private XDoc As XDocument
    ''' <summary>
    ''' индексы ненайденных параметров
    ''' </summary>
    Private IndexesEmptyParameters() As Integer
    ''' <summary>
    ''' были зменения в точках
    ''' </summary>
    Private isDirtyPoints As Boolean
    ''' <summary>
    ''' индекс текущей редактируемой линии
    ''' </summary>
    Private indexSelectedLine As Integer
    Private isFormClosing As Boolean
    ''' <summary>
    ''' ' были изменения в текущем графике
    ''' </summary>
    Private isDirtyCurrentGraph As Boolean
    Private ReadOnly mMathMethods As New MathMethods

#Region "String Const"
    Private strBox As String = ""
    Private Const NameColumnValueX As String = "ColumnValueX"
    Private Const NameColumnValueY As String = "ColumnValueY"
    Private Const NameColumnIndex As String = "ColumnIndex"
    Private Const NameColumnNameX As String = "DataGridViewComboBoxColumnNameX"
    Private Const NameColumnNameY As String = "DataGridViewComboBoxColumnNameY"

    Private Const XmlNodeParamGraphs As String = "ГрафикиОтПараметров"
    Public Const XmlNodeNameGraph As String = "ИмяГрафика"
    Public Const XmlNodeAxis As String = "Ось"
    Public Const XmlNodeParameter As String = "Параметр"
    Private Const XmlNodeGraphTU As String = "ГрафикТУ"
    Public Const XmlNodePoint As String = "Точка"
    Public Const XmlNodeFormula As String = "Формула"

    Public Const XmlAttributeDescription As String = "Наименование"
    Public Const XmlAttributeAxisName As String = "ИмяОси"
    Public Const XmlAttributeMin As String = "Min"
    Public Const XmlAttributeMax As String = "Max"
    Public Const XmlAttributeIsUseFormula As String = "ИспользоватьФормулу"
    Public Const XmlAttributeName As String = "Имя"
    Public Const XmlAttributeNameChannel As String = "ИмяКанала"
    Private Const XmlAttributeIndexParameter As String = "ИндексВМассивеПараметров"
    Public Const XmlAttributeReduce As String = "Приведение"
    Public Const XmlAttributeFormula As String = "Формула"
    Public Const XmlAttributeNameTU As String = "ИмяТУ"
    Public Const XmlAttributeColor As String = "Color"
    Public Const XmlAttributeLineStyle As String = "LineStyle"
    Private Const XmlAttributeLineColor As String = "LineColor"
    Public Const XmlAttributeIndex As String = "Индекс"
    Public Const XmlAttributeX As String = "X"
    Public Const XmlAttributeY As String = "Y"
#End Region

    Public Sub New(ByVal parentFormMain As FormMain)
        MyBase.New()
        Me.ParentFormMain = parentFormMain
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        MdiParent = MainMdiForm
    End Sub

    Private Sub FormEditiorGraphByGraph_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        PropertyEditorDefaultXAxis.Source = New PropertyEditorSource(XAxis1, "Range")
        PropertyEditorDefaultYAxis.Source = New PropertyEditorSource(YAxis1, "Range")

        ' занести в список. эдесь работаем с копией
        Re.Dim(NameParameters, UBound(CopyListOfParameter) - 1)

        For I As Integer = 1 To UBound(CopyListOfParameter)
            If IsDataBaseChanged Then
                NameParameters(I - 1) = ParametersShaphotType(CopyListOfParameter(I)).NameParameter
            Else
                NameParameters(I - 1) = ParametersType(CopyListOfParameter(I)).NameParameter
            End If
        Next

        Dim arrTemp(NameParameters.Length) As String
        Array.ConstrainedCopy(NameParameters, 0, arrTemp, 1, NameParameters.Length)
        arrTemp(0) = MissingParameter

        Dim tempDataGridViewComboBoxColumn As DataGridViewComboBoxColumn = CType(DataGridViewAxisXARG.Columns(NameColumnNameX), DataGridViewComboBoxColumn)
        tempDataGridViewComboBoxColumn.Items.AddRange(arrTemp)
        'теперь для Y
        tempDataGridViewComboBoxColumn = CType(DataGridViewAxisYARG.Columns(NameColumnNameY), DataGridViewComboBoxColumn)
        tempDataGridViewComboBoxColumn.Items.AddRange(arrTemp)

        Re.Dim(IndexesEmptyParameters, 1)
        LoadChannels()
        LoadGraphsXmlDoc()
        PopulateMathComboBox()
        isDirtyCurrentGraph = False

        PropertyEditorPlotColor.Source = New PropertyEditorSource(ScatterPlot1, XmlAttributeLineColor)
        PropertyEditorPlotStile.Source = New PropertyEditorSource(ScatterPlot1, XmlAttributeLineStyle)
    End Sub

    Private Sub FormEditiorGraphByGraph_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If isDirtyCurrentGraph Then
            Const caption As String = "Сохранение изменений"
            Dim text As String = "Были изменения при редактировании текущего графика." & vbCrLf & "Произвести сохранение изменений?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                If IsGeneralTestCorrectInput() Then
                    UpdateLimitationGraphs()
                    SaveXMLGraphs()
                Else
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        End If

        isFormClosing = True
        'defaultXAxisPropertyEditor.Source = Nothing'не работает
        'defaultYAxisPropertyEditor.Source = Nothing'не работает
        PropertyEditorDefaultXAxis.Source = New PropertyEditorSource(ButtonAddNewLimitationGraph, "Text")
        PropertyEditorDefaultYAxis.Source = New PropertyEditorSource(ButtonAddNewLimitationGraph, "Text")
        PropertyEditorPlotColor.Source = New PropertyEditorSource(ButtonAddNewLimitationGraph, "Text")
        PropertyEditorPlotStile.Source = New PropertyEditorSource(ButtonAddNewLimitationGraph, "Text")
    End Sub

    Private Sub FormEditiorGraphByGraph_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormMain.PopulateAllMenuPatternGraphByParameter()

        If ParentFormMain.KindFormExamination = FormExamination.RegistrationClient Then
            ParentFormMain.TimerTimeOutClient.Enabled = True
            ParentFormMain.FormClientDataSocket.ButtonConnect_Click(ParentFormMain.FormClientDataSocket.ButtonConnect, New EventArgs)
        End If

        ParentFormMain.MenuEditGraphOfParameter.Enabled = True
        ParentFormMain = Nothing
        CollectionForms.Remove(Text)
    End Sub

    ''' <summary>
    ''' Заполнить ComboBox списком математических функций.
    ''' </summary>
    Private Sub PopulateMathComboBox()
        mMathMethods.PopulateMath()

        For Each itemMethod As MathMethods.Method In mMathMethods.GetMethods.Values
            TSComboBoxMathematicalExpression.Items.Add(itemMethod.NameMethod)
        Next

        TSComboBoxMathematicalExpression.SelectedIndex = 0
    End Sub

#Region "isDirtyCurrentGraph = True"
    Private Sub DataGridViewAxisXARG_UserAddedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewAxisXARG.UserAddedRow
        DataGridViewAxisXARG.Rows(e.Row.Index - 1).Cells(0).Value = "ARG" & DataGridViewAxisXARG.RowCount - 1
        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewAxisXARG_UserDeletedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewAxisXARG.UserDeletedRow
        For I As Integer = 0 To DataGridViewAxisXARG.RowCount - 2
            DataGridViewAxisXARG.Rows(I).Cells(0).Value = "ARG" & (I + 1).ToString
        Next

        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewTablePoit_RowValidating(ByVal sender As Object, ByVal e As DataGridViewCellCancelEventArgs) Handles DataGridViewTablePoit.RowValidating
        Dim X, Y As Double
        Dim XObj, YObj As Object

        If e.ColumnIndex > 0 AndAlso e.RowIndex > 0 Then
            isDirtyCurrentGraph = True
            indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
            XObj = DataGridViewTablePoit.Rows(e.RowIndex).Cells(NameColumnValueX).Value
            YObj = DataGridViewTablePoit.Rows(e.RowIndex).Cells(NameColumnValueY).Value

            If XObj IsNot Nothing Then
                Try
                    X = Double.Parse(XObj, CultureInfo.CurrentCulture)
                Catch ex As FormatException
                    'Throw New FormatException("Значение X недопустимо", ex)
                    Const caption As String = "Ввод данных"
                    Dim text As String = $"Значение X= {XObj} недопустимо"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {text}")
                    e.Cancel = True
                End Try
            Else
                MessageBox.Show("Необходимо ввести Значение X", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If YObj IsNot Nothing Then
                Try
                    Y = Double.Parse(YObj, CultureInfo.CurrentCulture)
                Catch ex As FormatException
                    Const caption As String = "Ввод данных"
                    Dim text As String = $"Значение Y= {YObj} недопустимо"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {text}")
                    'Throw New FormatException("Значение Y недопустимо", ex)
                    e.Cancel = True
                End Try
            Else
                Const caption As String = "Ввод данных"
                Const text As String = "Необходимо ввести Значение Y"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If
        End If

        ShowMessageOnStatusPanel($"Введено X ={X} ; Y ={Y}")
    End Sub

    Private Sub DataGridViewAxisYARG_UserAddedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewAxisYARG.UserAddedRow
        DataGridViewAxisYARG.Rows(e.Row.Index - 1).Cells(0).Value = "ARG" & DataGridViewAxisYARG.RowCount - 1
        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewAxisYARG_UserDeletedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewAxisYARG.UserDeletedRow
        For I As Integer = 0 To DataGridViewAxisYARG.RowCount - 2
            DataGridViewAxisYARG.Rows(I).Cells(0).Value = "ARG" & (I + 1).ToString
        Next

        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewTablePoit_UserAddedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewTablePoit.UserAddedRow
        DataGridViewTablePoit.Rows(e.Row.Index - 1).Cells(0).Value = DataGridViewTablePoit.RowCount - 1
        isDirtyPoints = True
        isDirtyCurrentGraph = True
        indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
    End Sub

    Private Sub DataGridViewTablePoit_UserDeletedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewTablePoit.UserDeletedRow
        For I As Integer = 0 To DataGridViewTablePoit.RowCount - 2
            DataGridViewTablePoit.Rows(I).Cells(0).Value = (I + 1).ToString
        Next

        isDirtyPoints = True
        isDirtyCurrentGraph = True
        indexSelectedLine = ListBoxLimitationGraphs.SelectedIndex
    End Sub

    Private Sub ButtonEditLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.Click
        UpdateLimitationGraphs()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub RadioButtonAxisXOneParameter_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXOneParameter.CheckedChanged
        SplitContainerAxisX.Panel1.Enabled = RadioButtonAxisXOneParameter.Checked
        TextBoxMathAxisX.Enabled = Not RadioButtonAxisXOneParameter.Checked
        DataGridViewAxisXARG.Enabled = Not RadioButtonAxisXOneParameter.Checked
        ButtonTestFormulaX.Enabled = Not RadioButtonAxisXOneParameter.Checked
        isDirtyCurrentGraph = True
        ShowMessageOnStatusPanel("Для оси X выбран один параметр")
    End Sub

    Private Sub RadioButtonAxisXisFormula_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXisFormula.CheckedChanged
        SplitContainerAxisX.Panel1.Enabled = Not RadioButtonAxisXisFormula.Checked
        TextBoxMathAxisX.Enabled = RadioButtonAxisXisFormula.Checked
        DataGridViewAxisXARG.Enabled = RadioButtonAxisXisFormula.Checked
        ButtonTestFormulaX.Enabled = RadioButtonAxisXisFormula.Checked
        isDirtyCurrentGraph = True
        ShowMessageOnStatusPanel("Для оси X выбрана формула")
    End Sub

    Private Sub RadioButtonAxisYOneParameter_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYOneParameter.CheckedChanged
        SplitContainerAxisY.Panel1.Enabled = RadioButtonAxisYOneParameter.Checked
        TextBoxMathAxisY.Enabled = Not RadioButtonAxisYOneParameter.Checked
        DataGridViewAxisYARG.Enabled = Not RadioButtonAxisYOneParameter.Checked
        ButtonTestFormulaY.Enabled = Not RadioButtonAxisYOneParameter.Checked
        isDirtyCurrentGraph = True
        ShowMessageOnStatusPanel("Для оси Y выбран один параметр")
    End Sub

    Private Sub RadioButtonAxisYFormula_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYFormula.CheckedChanged
        SplitContainerAxisY.Panel1.Enabled = Not RadioButtonAxisYFormula.Checked
        TextBoxMathAxisY.Enabled = RadioButtonAxisYFormula.Checked
        DataGridViewAxisYARG.Enabled = RadioButtonAxisYFormula.Checked
        ButtonTestFormulaY.Enabled = RadioButtonAxisYFormula.Checked
        isDirtyCurrentGraph = True
        ShowMessageOnStatusPanel("Для оси Y выбрана формула")
    End Sub

    Private Sub ComboBoxAxisXFormulaParamaters_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxAxisXFormulaParamaters.SelectedIndexChanged,
                                                                                                                    ComboBoxAxisYFormulaParamaters.SelectedIndexChanged,
                                                                                                                    TextBoxMathAxisX.TextChanged,
                                                                                                                    TextBoxMathAxisY.TextChanged,
                                                                                                                    PropertyEditorDefaultXAxis.TextChanged,
                                                                                                                    PropertyEditorDefaultYAxis.TextChanged,
                                                                                                                    CheckBoxAxisXParameterReduce.CheckedChanged,
                                                                                                                    CheckBoxAxisYParameterReduce.CheckedChanged
        isDirtyCurrentGraph = True
    End Sub

    Private Sub TSButtonAddMathematicalExpression_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathematicalExpression.Click
        Dim mathText As String = mMathMethods.GetMethods(TSComboBoxMathematicalExpression.Text).TextMethod

        If TabControlGraphParam.TabPages(0) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisXisFormula.Checked Then TextBoxMathAxisX.SelectedText = mathText
        ElseIf TabControlGraphParam.TabPages(1) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisYFormula.Checked Then TextBoxMathAxisY.SelectedText = mathText
        End If

        ShowMessageOnStatusPanel("В формулу добавлено математическое выражение " & mathText)
    End Sub

    Private Sub TSButtonAddArgument_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.Click
        If TSComboBoxArguments.SelectedIndex = -1 Then Exit Sub

        Dim textArg As String = TSComboBoxArguments.Text

        If TabControlGraphParam.TabPages(0) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisXisFormula.Checked Then TextBoxMathAxisX.SelectedText = textArg
        ElseIf TabControlGraphParam.TabPages(1) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisYFormula.Checked Then TextBoxMathAxisY.SelectedText = textArg
        End If

        ShowMessageOnStatusPanel("В формулу добавлено имя аргумента " & textArg)
    End Sub

    Private Sub TabControlGraphParam_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlGraphParam.SelectedIndexChanged
        UpdateDataGridView()
    End Sub

    ''' <summary>
    ''' Обновить Список Аргументов
    ''' </summary>
    Private Sub UpdateDataGridView()
        If TabControlGraphParam.TabPages(0) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisXisFormula.Checked Then UpdateDataGridView(DataGridViewAxisXARG)
            ShowMessageOnStatusPanel("Выбрана панель редактирования оси X")
        ElseIf TabControlGraphParam.TabPages(1) Is TabControlGraphParam.SelectedTab Then
            If RadioButtonAxisYFormula.Checked Then UpdateDataGridView(DataGridViewAxisYARG)
            ShowMessageOnStatusPanel("Выбрана панель редактирования оси Y")
        Else
            ShowMessageOnStatusPanel("Выбрана панель редактирования графиков линий границ")
        End If
    End Sub

    Private Sub UpdateDataGridView(inDataGridView As DataGridView)
        TSComboBoxArguments.Items.Clear()

        For Each rows As DataGridViewRow In inDataGridView.Rows
            If rows.Index < inDataGridView.Rows.Count - 1 Then TSComboBoxArguments.Items.Add(rows.Cells(0).Value.ToString)
        Next

        If TSComboBoxArguments.Items.Count > 0 Then TSComboBoxArguments.SelectedIndex = 0
    End Sub
#End Region

#Region "DrawItem"
    Private Sub ComboBoxAxisXYFormulaParamaters_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ComboBoxAxisXFormulaParamaters.DrawItem, ComboBoxAxisYFormulaParamaters.DrawItem
        DrawItemComboBox(sender, die, IndexesEmptyParameters, ImageListChannel, False)
    End Sub

    Private Sub ComboBoxAxisXYFormulaParamaters_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ComboBoxAxisXFormulaParamaters.MeasureItem, ComboBoxAxisYFormulaParamaters.MeasureItem
        Dim cb As ComboBox = CType(sender, ComboBox)
        mie.ItemHeight = cb.ItemHeight - 2
    End Sub
#End Region

#Region "Дерево"
    Private Sub ButtonLoadGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.Click
        LoadGraph()
    End Sub

    Private Sub ButtonSaveGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Записать график от параметра")

        If IsGeneralTestCorrectInput() Then
            UpdateLimitationGraphs()
            SaveXMLGraphs()
        End If

        isDirtyCurrentGraph = False
    End Sub

    Private Sub ButtonDeleteGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.Click
        DeleteGraph()
    End Sub

    Private Sub ButtonAddGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.Click
        AddGraph()
    End Sub

    ''' <summary>
    ''' Выдать список Xml узлов с именем как в текстовом поле.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetFoundedGraphsList() As IEnumerable(Of XElement)
        Return From el In XDoc.Root...<ИмяГрафика>
               Where el.@Наименование = TextBoxNameGraph.Text
               Select el
    End Function

    ''' <summary>
    ''' Загрузка созданной конфигурации графика по параметру
    ''' </summary>
    Private Sub LoadGraph()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Считать график от параметра")
        PrepareLoadGraph()
        Dim findingGraphsList As IEnumerable(Of XElement) = GetFoundedGraphsList()

        If findingGraphsList.Any Then
            PopulateAxes(findingGraphsList(0))
            PopulationLimitationGraph(findingGraphsList(0))
        End If

        IsGeneralTestCorrectInput()
        UpdateDataGridView()
        ButtonSaveGraph.Enabled = True
        ButtonDeleteGraph.Enabled = True
        isDirtyCurrentGraph = False
    End Sub

    ''' <summary>
    ''' Конфигурировать Оси X и Y
    ''' </summary>
    ''' <param name="foundGraph"></param>
    Private Sub PopulateAxes(foundGraph As XElement)
        Dim axesXElementList As IEnumerable(Of XElement) = From axis In foundGraph.<Ось>
                                                           Select axis
        For Each itemAxisXElement As XElement In axesXElementList
            ' анализ значения атрибута 
            Dim min As Double = CDbl(itemAxisXElement.Attribute(XmlAttributeMin).Value)
            Dim max As Double = CDbl(itemAxisXElement.Attribute(XmlAttributeMax).Value)
            Dim isUseFormula As Boolean = Convert.ToBoolean(itemAxisXElement.Attribute(XmlAttributeIsUseFormula).Value)

            If itemAxisXElement.Attribute(XmlAttributeAxisName).Value = XmlAttributeX Then
                PopulateAxisX(itemAxisXElement, isUseFormula, New Range(min, max))
            Else
                PopulateAxisY(itemAxisXElement, isUseFormula, New Range(min, max))
            End If
            'Private Const Separator As Char = "-"c
            'Dim range As Range = String.Format(CultureInfo.InvariantCulture, "{0:R} {1} {2:R}", range.Minimum, Separator, range.Maximum)
        Next
    End Sub

    ''' <summary>
    ''' Конфигурировать Ось X
    ''' </summary>
    ''' <param name="axisXElement"></param>
    ''' <param name="isUseFormula"></param>
    ''' <param name="range"></param>
    Private Sub PopulateAxisX(axisXElement As XElement, isUseFormula As Boolean, range As Range)
        PropertyEditorDefaultXAxis.Source.Value = range
        XAxis1.Range = range
        RadioButtonAxisXisFormula.Checked = isUseFormula
        RadioButtonAxisXOneParameter.Checked = Not isUseFormula

        PopulateAxis(axisXElement, isUseFormula,
                     DataGridViewAxisXARG, DataGridViewComboBoxColumnNameX, TextBoxMathAxisX, ComboBoxAxisXFormulaParamaters, CheckBoxAxisXParameterReduce)
    End Sub

    ''' <summary>
    ''' Конфигурировать Ось Y
    ''' </summary>
    ''' <param name="axisXElement"></param>
    ''' <param name="isUseFormula"></param>
    ''' <param name="range"></param>
    Private Sub PopulateAxisY(axisXElement As XElement, isUseFormula As Boolean, range As Range)
        PropertyEditorDefaultYAxis.Source.Value = range
        YAxis1.Range = range
        RadioButtonAxisYFormula.Checked = isUseFormula
        RadioButtonAxisYOneParameter.Checked = Not isUseFormula

        PopulateAxis(axisXElement, isUseFormula,
                     DataGridViewAxisYARG, DataGridViewComboBoxColumnNameY, TextBoxMathAxisY, ComboBoxAxisYFormulaParamaters, CheckBoxAxisYParameterReduce)
    End Sub

    ''' <summary>
    ''' Параметризованная процедура Конфигурировавания Оси
    ''' </summary>
    ''' <param name="axisXElement"></param>
    ''' <param name="isUseFormula"></param>
    ''' <param name="refDataGridViewAxisARG"></param>
    ''' <param name="refDataGridViewComboBoxColumnName"></param>
    ''' <param name="refTextBoxMathAxis"></param>
    ''' <param name="refComboBoxAxisFormulaParamaters"></param>
    ''' <param name="refCheckBoxAxisParameterReduce"></param>
    Private Sub PopulateAxis(axisXElement As XElement, isUseFormula As Boolean,
                             ByRef refDataGridViewAxisARG As DataGridView,
                             ByRef refDataGridViewComboBoxColumnName As DataGridViewComboBoxColumn,
                             ByRef refTextBoxMathAxis As TextBox,
                             ByRef refComboBoxAxisFormulaParamaters As ComboBox,
                             ByRef refCheckBoxAxisParameterReduce As CheckBox)

        If isUseFormula Then
            refDataGridViewAxisARG.Rows.Clear()
            For Each itemParameter As XElement In axisXElement.Elements(XmlNodeParameter)
                'If itemParameter.Name = XmlNodeParameter Then
                Dim name As String = itemParameter.Attribute(XmlAttributeName).Value
                Dim nameChannel As String = itemParameter.Attribute(XmlAttributeNameChannel).Value
                Dim newRow As DataGridViewRow = New DataGridViewRow
                ' создаем строку, считывая описания колонок с _grid
                newRow.CreateCells(refDataGridViewAxisARG)

                newRow.Cells(0).Value = CType(name, Object)
                'newRow.Cells(0).Value = CType("ARG" & DataGridViewOsXARG.RowCount - 1, Object)
                'newRow.Cells(1).Value = CType(ИмяКанала, Object)
                ' сделать проверку на присутствие канала
                If CheckNameChannel(nameChannel) Then
                    newRow.Cells(1).Value = refDataGridViewComboBoxColumnName.Items(refDataGridViewComboBoxColumnName.Items.IndexOf(nameChannel))
                Else
                    newRow.Cells(1).Value = refDataGridViewComboBoxColumnName.Items(0)
                End If

                refDataGridViewAxisARG.Rows.Add(newRow)
                'End If
            Next

            ' на том же уровне
            refTextBoxMathAxis.Text = axisXElement.Elements(XmlNodeFormula).First.Value
        Else ' только один параметр
            Dim parameterXElement As XElement = axisXElement.Elements(XmlNodeParameter).First
            ' эдесь имя не используется
            Dim nameChannel As String = parameterXElement.Attribute(XmlAttributeNameChannel).Value
            If CheckNameChannel(nameChannel) Then
                refComboBoxAxisFormulaParamaters.Text = nameChannel
            Else
                refComboBoxAxisFormulaParamaters.SelectedIndex = 0
            End If

            ' эдесь приведение может быть использовано
            refCheckBoxAxisParameterReduce.Checked = Convert.ToBoolean(parameterXElement.Attribute(XmlAttributeReduce).Value)
        End If
    End Sub

    ''' <summary>
    ''' Конфигурирование линий ограничений
    ''' </summary>
    ''' <param name="foundGraph"></param>
    Private Sub PopulationLimitationGraph(foundGraph As XElement)
        ' здесь коллекция по графикам ТУ
        ListBoxLimitationGraphs.Items.Clear()
        ' удалить все Plot кроме первого в коллекции
        For I As Integer = ScatterGraphParam.Plots.Count - 1 To 1 Step -1
            ScatterGraphParam.Plots.RemoveAt(I)
        Next
        ' Заполнить лист. Индицировать, какой из членов добавленных элементов 
        ' в ListBox.DataSource должен быть показан
        ListBoxLimitationGraphs.DisplayMember = "ИмяГрафикаГраниц"

        Dim lineTUXElementList As IEnumerable(Of XElement) = From lineTU In foundGraph.<ГрафикТУ>
                                                             Select lineTU
        For Each itemlineTU As XElement In lineTUXElementList
            Dim nameTU As String = itemlineTU.Attribute(XmlAttributeNameTU).Value
            Dim stringColor As String = itemlineTU.Attribute(XmlAttributeColor).Value
            Dim stringLineStyle As String = itemlineTU.Attribute(XmlAttributeLineStyle).Value
            Dim tempCurveGraph As CurveGraph = New CurveGraph(nameTU) With {.Color = stringColor, .LineStyle = stringLineStyle}
            Dim values As Array = EnumObject.GetValues(ScatterPlot1.LineStyle.UnderlyingType)
            Dim valueTemp As LineStyle = LineStyle.Dot ' по умолчанию

            For I As Integer = 0 To values.Length - 1
                If values.GetValue(I).ToString = stringLineStyle Then
                    valueTemp = CType(values.GetValue(I), LineStyle)
                    Exit For
                End If
            Next

            Dim tempScatterPlot As ScatterPlot = New ScatterPlot() With {.LineColor = Color.FromName(stringColor),
                                                                        .PointColor = Color.Red,
                                                                        .PointStyle = PointStyle.SolidDiamond,
                                                                        .LineStyle = valueTemp,
                                                                        .XAxis = XAxis1,
                                                                        .YAxis = YAxis1,
                                                                        .Tag = nameTU}

            For Each itemPointXElement As XElement In itemlineTU.Elements(XmlNodePoint)
                Dim index As Integer = CInt(itemPointXElement.Attribute(XmlAttributeIndex).Value)
                Dim X As Double = CDbl(itemPointXElement.Attribute(XmlAttributeX).Value)
                Dim Y As Double = CDbl(itemPointXElement.Attribute(XmlAttributeY).Value)
                tempCurveGraph.Add(index, X, Y)
                tempScatterPlot.PlotXYAppend(X, Y)
            Next

            ListBoxLimitationGraphs.Items.Add(tempCurveGraph)
            ScatterGraphParam.Plots.Add(tempScatterPlot)
        Next

        If ListBoxLimitationGraphs.Items.Count > 0 Then ListBoxLimitationGraphs.SelectedIndex = ListBoxLimitationGraphs.Items.Count - 1
    End Sub

    ''' <summary>
    ''' Очистка
    ''' </summary>
    Private Sub PrepareLoadGraph()
        RadioButtonAxisXisFormula.Checked = False
        RadioButtonAxisYFormula.Checked = False

        ComboBoxAxisXFormulaParamaters.SelectedItem = 0
        ComboBoxAxisYFormulaParamaters.SelectedItem = 0

        CheckBoxAxisXParameterReduce.Checked = False
        CheckBoxAxisYParameterReduce.Checked = False

        DataGridViewAxisXARG.Rows.Clear()
        DataGridViewAxisYARG.Rows.Clear()
        DataGridViewTablePoit.Rows.Clear()

        TextBoxMathAxisX.Text = ""
        TextBoxMathAxisY.Text = ""
        indexSelectedLine = -1
        ListBoxLimitationGraphs.Items.Clear()
    End Sub

    ''' <summary>
    ''' Добавление созданной конфигурации графика по параметру
    ''' </summary>
    Private Sub AddGraph()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавить график параметр от параметра")

        If TextBoxNameGraph.Text <> "" Then
            ' если имя новое, то новое Условие добавляется
            If GetFoundedGraphsList().Any Then
                Const caption As String = "Новый график параметр от параметра"
                Dim text As String = $"График параметр от параметра с именем {TextBoxNameGraph.Text} уже существует!{vbCrLf}Необходимо ввести новое имя."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                ShowMessageOnStatusPanel("Повтор имени графика")
            Else ' узла нет, значит можно записать новый
                If IsGeneralTestCorrectInput() Then SaveXMLGraphs()
            End If

            isDirtyCurrentGraph = False
        Else
            Const caption As String = "Новый график параметр от параметра"
            Const text As String = "Необходимо ввести имя графика параметр от параметра!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ShowMessageOnStatusPanel(text)
        End If
    End Sub

    ''' <summary>
    ''' Удаление выделенной конфигурации графика по параметру
    ''' </summary>
    Private Sub DeleteGraph()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить график от параметра")
        RemoveEditingGraph()

        If TextBoxNameGraph.Text <> "" Then
            XDoc.Save(PathXmlFileGraphByParameters)
            LoadGraphsXmlDoc()
            TextBoxNameGraph.Text = ""
            ButtonLoadGraph.Enabled = False
            ButtonSaveGraph.Enabled = False
            ButtonDeleteGraph.Enabled = False
            ShowMessageOnStatusPanel($"Удаление условия {TextBoxNameGraph.Text} произведено успешно")
            isDirtyCurrentGraph = False
        End If
    End Sub

    ''' <summary>
    ''' Удаление выбранного Графика
    ''' </summary>
    Private Sub RemoveEditingGraph()
        If TextBoxNameGraph.Text <> "" Then
            '--- удалить существующий элемент -------------------------------------
            Dim removingList As IEnumerable(Of XElement) = GetFoundedGraphsList()
            If removingList.Any Then
                Try
                    removingList(0).Remove()
                    ' то же самое
                    'XDoc.Root.Elements.Where(Function(el) el.FirstAttribute = TextBoxNameGraph.Text).First.Remove()
                Catch ex As Exception
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, NameOf(RemoveEditingGraph), MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{NameOf(RemoveEditingGraph)}> {text}")
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Запись созданной конфигурации Графика
    ''' </summary>
    Private Sub SaveXMLGraphs()
        ' если имя пусто вопрос  "введите имя" и так как модально нельзя, то выход из данной процедуры
        If TextBoxNameGraph.Text <> "" Then
            ' если переписать то удаление из базы старых и запись новых
            ' если имя новое, то новое Условие добавляется
            RemoveEditingGraph()
            XDoc.Root.Add(CreateXElementGraph(TextBoxNameGraph.Text))
            XDoc.Save(PathXmlFileGraphByParameters)
            LoadGraphsXmlDoc()
            isDirtyCurrentGraph = False
            ShowMessageOnStatusPanel("Сохранение изменений произведено успешно")
        Else ' надо ввести имя
            Const caption As String = "Необходимо ввести имя графика параметр от параметра!"
            Const text As String = "Новый график параметр от параметра"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ShowMessageOnStatusPanel(caption)
        End If
    End Sub

    ''' <summary>
    ''' Создать Xml элемент узла XmlNodeNameGraph
    ''' </summary>
    Private Function CreateXElementGraph(nameGraph As String) As XElement
        ' создать элемент txtИмяПерекладки.Text
        '<ГрафикиОтПараметров>
        '   <ИмяГрафика Наименование="Первый">

        ' создать и добавить аттрибут
        Dim newGraphXElement As New XElement(XmlNodeNameGraph)
        newGraphXElement.SetAttributeValue(XmlAttributeDescription, nameGraph)

        newGraphXElement.Add(CreateXmlAxixX)
        newGraphXElement.Add(CreateXmlAxixY)
        CreateXmlLinesPoints(newGraphXElement)

        Return newGraphXElement
    End Function

    ''' <summary>
    ''' Создать Xml элемент узла Оси X
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateXmlAxixX() As XElement
        Return CreateXmlAxix(XmlAttributeX,
                             XAxis1.Range,
                             RadioButtonAxisXisFormula.Checked,
                             TextBoxMathAxisX.Text,
                             DataGridViewAxisXARG,
                             ComboBoxAxisXFormulaParamaters.Text,
                             ComboBoxAxisXFormulaParamaters.SelectedIndex,
                             CheckBoxAxisXParameterReduce.Checked)
    End Function

    ''' <summary>
    ''' Создать Xml элемент узла Оси Y
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateXmlAxixY() As XElement
        Return CreateXmlAxix(XmlAttributeY,
                             YAxis1.Range,
                             RadioButtonAxisYFormula.Checked,
                             TextBoxMathAxisY.Text,
                             DataGridViewAxisYARG,
                             ComboBoxAxisYFormulaParamaters.Text,
                             ComboBoxAxisYFormulaParamaters.SelectedIndex,
                             CheckBoxAxisYParameterReduce.Checked)
    End Function

    ''' <summary>
    ''' Создать Xml элемент узла Оси.
    ''' Функция параметризованная.
    ''' </summary>
    ''' <param name="inAxisName"></param>
    ''' <param name="inRange"></param>
    ''' <param name="isAxisFormula"></param>
    ''' <param name="inDataGridViewAxisARG"></param>
    ''' <param name="inParamaterName"></param>
    ''' <param name="inSelectedIndex"></param>
    ''' <param name="isAxisParameterReduce"></param>
    ''' <returns></returns>
    Private Function CreateXmlAxix(inAxisName As String,
                                   inRange As Range,
                                   isAxisFormula As Boolean,
                                   inFormula As String,
                                   inDataGridViewAxisARG As DataGridView,
                                   inParamaterName As String,
                                   inSelectedIndex As Integer,
                                   isAxisParameterReduce As Boolean) As XElement
        '<Ось ИмяОси=XmlAttributeX Min="10" Max="100" ИспользоватьФормулу="False">
        '  <Параметр Имя="N1 приведенный" ИмяКанала="ап" ИндексВМассивеПараметров="25" Приведение="True" ></Параметр>
        '  <Формула>ТекстФормулы</Формула>
        '</Ось>
        '<Ось ИмяОси=XmlAttributeY Min="20" Max="200" ИспользоватьФормулу="True">
        '  <Параметр Имя="ARG1" ИмяКанала="Bo" ИндексВМассивеПараметров="25" Приведение="False" ></Параметр>
        '  <Параметр Имя="ARG2" ИмяКанала="аРУД" ИндексВМассивеПараметров="25" Приведение="False" ></Параметр>
        '  <Формула>Math.sqrt((ARG2+ARG1/755.6)/(ARG1/735.6))</Формула>
        '</Ось>
        Dim axisXElement As New XElement(XmlNodeAxis)
        axisXElement.SetAttributeValue(XmlAttributeAxisName, inAxisName)
        axisXElement.SetAttributeValue(XmlAttributeMin, inRange.Minimum.ToString)
        axisXElement.SetAttributeValue(XmlAttributeMax, inRange.Maximum.ToString)
        axisXElement.SetAttributeValue(XmlAttributeIsUseFormula, isAxisFormula.ToString)

        If isAxisFormula Then
            For Each row As DataGridViewRow In inDataGridViewAxisARG.Rows
                If row.Index < inDataGridViewAxisARG.Rows.Count - 1 Then
                    Dim parameterXElement As New XElement(XmlNodeParameter)
                    parameterXElement.SetAttributeValue(XmlAttributeName, row.Cells(0).Value.ToString)
                    parameterXElement.SetAttributeValue(XmlAttributeNameChannel, row.Cells(1).Value.ToString)
                    parameterXElement.SetAttributeValue(XmlAttributeIndexParameter, Array.IndexOf(NameParameters, row.Cells(1).Value).ToString)
                    parameterXElement.SetAttributeValue(XmlAttributeReduce, "False")

                    axisXElement.Add(parameterXElement)
                End If
            Next

            '  <Формула>ТекстФормулы</Формула>
            Dim formulaXElement As New XElement(XmlNodeFormula, inFormula)
            axisXElement.Add(formulaXElement)
        Else ' только один параметр
            '  <Параметр Имя="N1 приведенный" ИмяКанала="ап" ИндексВМассивеПараметров="25" Приведение="True" ></Параметр>
            Dim name As String = inParamaterName
            If isAxisParameterReduce Then name += " приведенный"

            Dim parameterXElement As New XElement(XmlNodeParameter)
            parameterXElement.SetAttributeValue(XmlAttributeName, name)
            parameterXElement.SetAttributeValue(XmlAttributeNameChannel, inParamaterName)
            parameterXElement.SetAttributeValue(XmlAttributeIndexParameter, inSelectedIndex.ToString)
            parameterXElement.SetAttributeValue(XmlAttributeReduce, isAxisParameterReduce.ToString)

            axisXElement.Add(parameterXElement)
        End If

        Return axisXElement
    End Function

    ''' <summary>
    ''' Создать Xml элементы узлов точек диний ограничений.
    ''' </summary>
    ''' <param name="graphXElement"></param>
    Private Sub CreateXmlLinesPoints(graphXElement As XElement)
        '<ГрафикТУ ИмяТУ="НижнийПредел" Color="Red" LineStyle="Dot">
        '  <Точка Индекс="1" X="10" Y="10"></Точка>
        '  <Точка Индекс="2" X="20" Y="20"></Точка>
        '  <Точка Индекс="3" X="30" Y="30"></Точка>
        '  <Точка Индекс="4" X="40" Y="40"></Точка>
        '  <Точка Индекс="5" X="50" Y="50"></Точка>
        '</ГрафикТУ>

        For I As Integer = 0 To ListBoxLimitationGraphs.Items.Count - 1
            Dim tempCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.Items(I), CurveGraph)
            Dim limitationGraphTUXElement As New XElement(XmlNodeGraphTU)

            limitationGraphTUXElement.SetAttributeValue(XmlAttributeNameTU, tempCurveGraph.NameCurveGraph)
            limitationGraphTUXElement.SetAttributeValue(XmlAttributeColor, tempCurveGraph.Color)
            limitationGraphTUXElement.SetAttributeValue(XmlAttributeLineStyle, tempCurveGraph.LineStyle)

            For Each itemPointCurve As CurveGraph.PointCurve In tempCurveGraph.GetPointsCurve.Values
                Dim pointXElement As New XElement(XmlNodePoint)
                pointXElement.SetAttributeValue(XmlAttributeIndex, itemPointCurve.Index)
                pointXElement.SetAttributeValue(XmlAttributeX, itemPointCurve.X)
                pointXElement.SetAttributeValue(XmlAttributeY, itemPointCurve.Y)

                limitationGraphTUXElement.Add(pointXElement)
            Next

            graphXElement.Add(limitationGraphTUXElement)
        Next
    End Sub

    Private Sub ButtonAddNewLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.Click
        AddNewLimitationGraph()
    End Sub

    Private Sub ButtonDeleteSelectedLimitationGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.Click
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub
        DeleteSelectedLimitationGraph()
    End Sub

    Private Sub ListBoxLimitationGraphs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.SelectedIndexChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub
        ChangedLimitationLine()
    End Sub

    ''' <summary>
    ''' Смена редактирования линии границ ограничений.
    ''' </summary>
    Private Sub ChangedLimitationLine()
        ' здесь колличество в ГрафикГраниц равно DataGridViewГрафикГраниц.Rows и сравнивать можно
        ' явно устанавливаются при добавлении и удалении
        If Not isDirtyPoints Then CheckOnDirtyPoint()

        If isDirtyPoints Then
            Const caption As String = "Смена линии"
            Dim text As String = "Были изменения в таблице задания точек линии." & vbCrLf & "Произвести сохранение изменений?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")
            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateLimitationGraphs()
            End If
        End If

        isDirtyPoints = False
        indexSelectedLine = -1

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        DataGridViewTablePoit.Rows.Clear()

        Dim tempScatterPlot As ScatterPlot = Nothing
        'For I As Integer = 0 To ScatterGraphParam.Plots.Count - 1
        '    If ScatterGraphParam.Plots.Item(I).Tag = selectedCurveGraph.NameCurveGraph Then
        '        Exit For
        '    End If
        'Next

        For Each itemScatterPlot As ScatterPlot In ScatterGraphParam.Plots
            If CStr(itemScatterPlot.Tag) = selectedCurveGraph.NameCurveGraph Then tempScatterPlot = itemScatterPlot

            itemScatterPlot.LineWidth = 1
        Next

        If tempScatterPlot IsNot Nothing Then
            For Each itemPointCurve As CurveGraph.PointCurve In selectedCurveGraph.GetPointsCurve.Values
                ' создаем строку, считывая описания колонок с _grid
                Dim heter_row As DataGridViewRow = New DataGridViewRow
                heter_row.CreateCells(DataGridViewTablePoit)

                heter_row.Cells(0).Value = CType(itemPointCurve.Index, Object)
                heter_row.Cells(1).Value = CType(itemPointCurve.X, Object)
                heter_row.Cells(2).Value = CType(itemPointCurve.Y, Object)
                DataGridViewTablePoit.Rows.Add(heter_row)
            Next

            tempScatterPlot.LineWidth = 2
            TextBoxNameNewLimitationGraph.Text = selectedCurveGraph.NameCurveGraph
            PropertyEditorPlotColor.Source = New PropertyEditorSource(tempScatterPlot, XmlAttributeLineColor)
            PropertyEditorPlotStile.Source = New PropertyEditorSource(tempScatterPlot, XmlAttributeLineStyle)
        End If

        ShowMessageOnStatusPanel($"График границы {selectedCurveGraph.NameCurveGraph} выбран для редактирования")
    End Sub

    ''' <summary>
    ''' Обновить График Границ
    ''' </summary>
    Private Sub UpdateLimitationGraphs()
        If ListBoxLimitationGraphs.SelectedIndex = -1 Then Exit Sub

        Dim tempCurveGraph As CurveGraph

        If isDirtyPoints Then
            If indexSelectedLine = -1 Then
                Const caption As String = "Новый график параметр от параметра"
                Const text As String = "Необходимо вначале добавить график границ!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If

            tempCurveGraph = CType(ListBoxLimitationGraphs.Items(indexSelectedLine), CurveGraph)
        Else ' редактировать границу или добавить новую границу
            tempCurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        End If

        Dim X, Y As Double
        Dim index As Integer
        tempCurveGraph.Clear()

        Dim tempScatterPlot As ScatterPlot = Nothing
        ' поиск
        isDirtyPoints = False
        indexSelectedLine = -1

        For Each itemScatterPlot As ScatterPlot In ScatterGraphParam.Plots
            If CStr(itemScatterPlot.Tag) = tempCurveGraph.NameCurveGraph Then
                tempScatterPlot = itemScatterPlot
                Exit For
            End If
        Next

        If tempScatterPlot IsNot Nothing Then
            tempScatterPlot.ClearData()

            For Each row As DataGridViewRow In DataGridViewTablePoit.Rows
                If row.Index < DataGridViewTablePoit.Rows.Count - 1 Then
                    index = CInt(row.Cells(NameColumnIndex).Value)
                    X = CDbl(row.Cells(NameColumnValueX).Value)
                    Y = CDbl(row.Cells(NameColumnValueY).Value)
                    ' перезапись в коллекцию
                    tempScatterPlot.PlotXYAppend(X, Y)
                    tempCurveGraph.Add(index, X, Y)
                End If
            Next
        End If

        ShowMessageOnStatusPanel("Обновления графика границ произведено успешно")
    End Sub

    ''' <summary>
    ''' Добавление линии ограничений.
    ''' </summary>
    Private Sub AddNewLimitationGraph()
        If isDirtyPoints Then
            Const caption As String = "Смена линии"
            Dim text As String = "Были изменения в таблице задания точек линии." & vbCrLf & "Произвести сохранение изменений?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateLimitationGraphs()
            Else
                isDirtyPoints = False
                indexSelectedLine = -1
            End If
        End If

        If TextBoxNameNewLimitationGraph.Text = "" Then
            Const caption As String = "Ввод нового графика границ"
            Const text As String = "Необходимо ввести имя графика линии границ !"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ShowMessageOnStatusPanel(text)
            Exit Sub
        End If

        ' проверка на имя
        'If ListBoxСписокГраниц.Items.Contains(tempГрафикГраниц) Then
        Dim isGraphContain As Boolean 'имя Графика Уже Есть

        For I As Integer = 0 To ListBoxLimitationGraphs.Items.Count - 1
            If CType(ListBoxLimitationGraphs.Items(I), CurveGraph).NameCurveGraph = TextBoxNameNewLimitationGraph.Text Then
                isGraphContain = True
                Exit For
            End If
        Next

        If isGraphContain Then
            Const caption As String = "Ввод нового графика границ"
            Dim text As String = $"График линии границ с именем {TextBoxNameNewLimitationGraph.Text} уже существует!{vbCrLf}Введите другое имя в поле имя линии границ."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ShowMessageOnStatusPanel("Повтор имени линии границ")
        Else
            'If plotStilPropertyEditor.Source.Value.ToString = "Nothing" Then
            '    ScatterPlotTemp.LineStyle = plotStilPropertyEditor.Source.Value
            'Else
            '    ScatterPlotTemp.LineStyle = NationalInstruments.UI.LineStyle.Dot
            'End If
            Dim tempScatterPlot As ScatterPlot = New ScatterPlot With {
                .LineColor = CType(PropertyEditorPlotColor.Source.Value, Color), ' System.Drawing.Color.White
                .PointColor = Color.Red,
                .PointStyle = PointStyle.SolidDiamond,
                .LineStyle = CType(PropertyEditorPlotStile.Source.Value, LineStyle), 'NationalInstruments.UI.LineStyle.Dot
                .XAxis = XAxis1,
                .YAxis = YAxis1,
                .Tag = TextBoxNameNewLimitationGraph.Text
            }
            'ScatterPlotTemp.LineColor = System.Drawing.Color.FromName(Color)

            PropertyEditorPlotColor.Source = New PropertyEditorSource(tempScatterPlot, XmlAttributeLineColor)
            PropertyEditorPlotStile.Source = New PropertyEditorSource(tempScatterPlot, XmlAttributeLineStyle)

            ScatterGraphParam.Plots.Add(tempScatterPlot)

            Dim tempCurveGraph As CurveGraph = New CurveGraph(TextBoxNameNewLimitationGraph.Text) With {
                .Color = PropertyEditorPlotColor.Source.Value.Name,
                .LineStyle = PropertyEditorPlotStile.Source.Value.ToString
            }

            ListBoxLimitationGraphs.Items.Add(tempCurveGraph)
            ListBoxLimitationGraphs.SelectedIndex = ListBoxLimitationGraphs.Items.Count - 1
            ShowMessageOnStatusPanel($"График линии границ {TextBoxNameNewLimitationGraph.Text} добавлен успешно")
        End If
    End Sub

    ''' <summary>
    ''' Удаление линии ограничений.
    ''' </summary>
    Private Sub DeleteSelectedLimitationGraph()
        Dim tempCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)
        Dim removingScatterPlot As ScatterPlot = Nothing
        ' поиск
        For Each ScatterPlotTempPlots As ScatterPlot In ScatterGraphParam.Plots
            If CStr(ScatterPlotTempPlots.Tag) = tempCurveGraph.NameCurveGraph Then
                removingScatterPlot = ScatterPlotTempPlots
                Exit For
            End If
        Next

        If removingScatterPlot IsNot Nothing Then
            ShowMessageOnStatusPanel($"График границ {tempCurveGraph.NameCurveGraph} удален")
            ScatterGraphParam.Plots.Remove(removingScatterPlot)
            ListBoxLimitationGraphs.Items.Remove(ListBoxLimitationGraphs.SelectedItem)
        End If
    End Sub

    ''' <summary>
    ''' Проверка изменений значений точек линий ограничений.
    ''' </summary>
    Private Sub CheckOnDirtyPoint()
        If indexSelectedLine = -1 Then Exit Sub

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.Items.Item(indexSelectedLine), CurveGraph) ' .SelectedItem

        For Each rows As DataGridViewRow In DataGridViewTablePoit.Rows
            If rows.Index < DataGridViewTablePoit.Rows.Count - 1 Then
                Dim index As Integer = CInt(rows.Cells(NameColumnIndex).Value)

                If CDbl(rows.Cells(NameColumnValueX).Value) <> selectedCurveGraph.GetPointsCurve(index).X OrElse CDbl(rows.Cells(NameColumnValueY).Value) <> selectedCurveGraph.GetPointsCurve(index).Y Then
                    isDirtyPoints = True
                    isDirtyCurrentGraph = True
                    Exit For
                End If
            End If
        Next
    End Sub
#End Region

#Region "ToolTip"
    Private Sub ShowMessageOnStatusPanel(ByVal inMessage As String)
        TSStatusLabelMessage.Text = inMessage
    End Sub

    Private Sub CleareStatusLabelMessage()
        TSStatusLabelMessage.Text = ""
    End Sub
    Private Sub TSButtonAddArgument_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.MouseEnter
        ShowMessageOnStatusPanel("Вставить в формулу имя аргумента")
    End Sub

    Private Sub TSButtonAddArgument_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.MouseLeave
        CleareStatusLabelMessage()
    End Sub
    Private Sub TSButtonAddMathematicalExpression_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathematicalExpression.MouseEnter
        ShowMessageOnStatusPanel("Вставить в формулу математическое выражение")
    End Sub

    Private Sub TSButtonAddMathematicalExpression_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathematicalExpression.MouseLeave
        CleareStatusLabelMessage()
    End Sub

#Region "ScatterGraphParam"
    Private Sub ScatterGraphParam_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ScatterGraphParam.MouseEnter
        ShowMessageOnStatusPanel("Внешний вид редактируемого графика")
    End Sub

    Private Sub ScatterGraphParam_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ScatterGraphParam.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "Считать Graph"
    Private Sub ButtonLoadGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.MouseEnter
        ShowMessageOnStatusPanel("Считать выбранный график от параметров")
    End Sub

    Private Sub ButtonLoadGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLoadGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "Записать Graph"
    Private Sub ButtonSaveGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.MouseEnter
        ShowMessageOnStatusPanel("Записать редактируемый график от параметров")
    End Sub

    Private Sub ButtonSaveGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "Добавить Graph"
    Private Sub ButtonAddGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.MouseEnter
        ShowMessageOnStatusPanel("Добавить новый редактируемый график от параметров")
    End Sub

    Private Sub ButtonAddGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "Удалить Graph"
    Private Sub ButtonDeleteGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.MouseEnter
        ShowMessageOnStatusPanel("Удалить данный редактируемый график от параметров")
    End Sub

    Private Sub ButtonDeleteGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "PropertyEditorDefaultXAxis"
    Private Sub PropertyEditorDefaultXAxis_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorDefaultXAxis.MouseEnter
        ShowMessageOnStatusPanel("Определение диапазона построения оси X")
    End Sub

    Private Sub PropertyEditorDefaultXAxis_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorDefaultXAxis.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "RadioButtonAxisXOneParameter"
    Private Sub RadioButtonAxisXOneParameter_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXOneParameter.MouseEnter
        ShowMessageOnStatusPanel("Для построения оси X использовать только один параметр")
    End Sub

    Private Sub RadioButtonAxisXOneParameter_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXOneParameter.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "RadioButtonAxisXisFormula"
    Private Sub RadioButtonAxisXisFormula_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXisFormula.MouseEnter
        ShowMessageOnStatusPanel("Для построения оси X использовать формулу с параметрами")
    End Sub

    Private Sub RadioButtonAxisXisFormula_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisXisFormula.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ButtonTestFormulaX"
    Private Sub ButtonTestFormulaX_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaX.MouseEnter
        ShowMessageOnStatusPanel("Тест формулы для проверки корректности")
    End Sub

    Private Sub ButtonTestFormulaX_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaX.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "TextBoxMathAxisX"
    Private Sub TextBoxMathAxisX_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathAxisX.MouseEnter
        ShowMessageOnStatusPanel("Текст формулы оси X")
    End Sub

    Private Sub TextBoxMathAxisX_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathAxisX.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ComboBoxAxisXFormulaParamaters"
    Private Sub ComboBoxAxisXFormulaParamaters_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxAxisXFormulaParamaters.MouseEnter
        ShowMessageOnStatusPanel("Выбор параметра используемого для построения оси X")
    End Sub

    Private Sub ComboBoxAxisXFormulaParamaters_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxAxisXFormulaParamaters.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "CheckBoxAxisXParameterReduce"
    Private Sub CheckBoxAxisXParameterReduce_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBoxAxisXParameterReduce.MouseEnter
        ShowMessageOnStatusPanel("Параметр используемый для построения оси X приводится к нормальным условиям")
    End Sub

    Private Sub CheckBoxAxisXParameterReduce_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBoxAxisXParameterReduce.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "PropertyEditorDefaultYAxis"
    Private Sub PropertyEditorDefaultYAxis_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorDefaultYAxis.MouseEnter
        ShowMessageOnStatusPanel("Определение диапазона построения оси Y")
    End Sub

    Private Sub PropertyEditorDefaultYAxis_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorDefaultYAxis.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "RadioButtonAxisYOneParameter"
    Private Sub RadioButtonAxisYOneParameter_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYOneParameter.MouseEnter
        ShowMessageOnStatusPanel("Для построения оси Y использовать только один параметр")
    End Sub

    Private Sub RadioButtonAxisYOneParameter_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYOneParameter.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "RadioButtonAxisYFormula"
    Private Sub RadioButtonAxisYFormula_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYFormula.MouseEnter
        ShowMessageOnStatusPanel("Для построения оси Y использовать формулу с параметрами")
    End Sub

    Private Sub RadioButtonAxisYFormula_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonAxisYFormula.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ButtonTestFormulaY"
    Private Sub ButtonTestFormulaY_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaY.MouseEnter
        ShowMessageOnStatusPanel("Тест формулы для проверки корректности")
    End Sub

    Private Sub ButtonTestFormulaY_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaY.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "TextBoxMathAxisY"
    Private Sub TextBoxMathAxisY_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathAxisY.MouseEnter
        ShowMessageOnStatusPanel("Текст формулы оси Y")
    End Sub

    Private Sub TextBoxMathAxisY_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathAxisY.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ComboBoxAxisYFormulaParamaters"
    Private Sub ComboBoxAxisYFormulaParamaters_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxAxisYFormulaParamaters.MouseEnter
        ShowMessageOnStatusPanel("Выбор параметра используемого для построения оси Y")
    End Sub

    Private Sub ComboBoxAxisYFormulaParamaters_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxAxisYFormulaParamaters.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "CheckBoxAxisYParameterReduce"
    Private Sub CheckBoxAxisYParameterReduce_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBoxAxisYParameterReduce.MouseEnter
        ShowMessageOnStatusPanel("Параметр используемый для построения оси Y приводится к нормальным условиям")
    End Sub

    Private Sub CheckBoxAxisYParameterReduce_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBoxAxisYParameterReduce.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ListBoxLimitationGraphs"
    Private Sub ListBoxLimitationGraphs_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.MouseEnter
        ShowMessageOnStatusPanel("Выбор из списка границы ограничения для редактирования")
    End Sub

    Private Sub ListBoxLimitationGraphs_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxLimitationGraphs.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ButtonEditLimitationGraph"
    Private Sub ButtonEditLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Сохранение произведенных изменений для выбранной границы ограничения")
    End Sub

    Private Sub ButtonEditLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonEditLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ButtonDeleteSelectedLimitationGraph"
    Private Sub ButtonDeleteSelectedLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Удаление из списка выбранной границы ограничения")
    End Sub

    Private Sub ButtonDeleteSelectedLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "ButtonAddNewLimitationGraph"
    Private Sub ButtonAddNewLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Добавление новой границы ограничения в список")
    End Sub

    Private Sub ButtonAddNewLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddNewLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "TextBoxNameNewLimitationGraph"
    Private Sub TextBoxNameNewLimitationGraph_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameNewLimitationGraph.MouseEnter
        ShowMessageOnStatusPanel("Выбранное имя границы ограничения или ввод нового имени при добавлении")
    End Sub

    Private Sub TextBoxNameNewLimitationGraph_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameNewLimitationGraph.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "PropertyEditorPlotColor"
    Private Sub PropertyEditorPlotColor_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.MouseEnter
        ShowMessageOnStatusPanel("Назначение цвета для границы ограничения")
    End Sub

    Private Sub PropertyEditorPlotColor_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "PropertyEditorPlotStil"
    Private Sub PropertyEditorPlotStil_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.MouseEnter
        ShowMessageOnStatusPanel("Назначение стиля линии для границы ограничения")
    End Sub

    Private Sub PropertyEditorPlotStil_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region
#End Region

#Region "XmlView работа с узлами XML документа"
    Private Sub XmlTreeView_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles XmlTreeView.AfterCollapse
        If e.Node.Tag IsNot Nothing AndAlso CType(e.Node.Tag, String).IndexOf(XmlNodeNameGraph) > -1 Then
            e.Node.ImageIndex = XmlImage.NameGraph2
            TextBoxNameGraph.Text = ""
            ButtonLoadGraph.Enabled = False
            ButtonSaveGraph.Enabled = False
            ButtonDeleteGraph.Enabled = False
        End If
    End Sub

    Private Sub XmlTreeView_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles XmlTreeView.BeforeExpand
        For Each itemNode As TreeNode In XmlTreeView.Nodes(0).Nodes
            'If NodeLoop.IsExpanded And Not (NodeLoop Is e.Node.Parent) Then
            If itemNode.IsExpanded AndAlso (e.Node.Parent Is XmlTreeView.Nodes(0)) Then
                ' и его родитель равен корневому то свернуть
                TrySaveXMLGraphs() ' здесь модальное окно и в нем blnБылиИзмененияВТекущемГрафике = False сделать нельзя
                isDirtyCurrentGraph = False
                itemNode.Collapse()
                e.Node.ImageIndex = XmlImage.NameGraph2
            End If
        Next
    End Sub

    Private Sub XmlTreeView_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles XmlTreeView.AfterExpand
        If e.Node.Tag IsNot Nothing AndAlso CType(e.Node.Tag, String).IndexOf(XmlNodeNameGraph) > -1 Then
            e.Node.ImageIndex = XmlImage.Root0
            TextBoxNameGraph.Text = e.Node.Text
            ButtonLoadGraph.Enabled = True
            ButtonSaveGraph.Enabled = True
            ButtonDeleteGraph.Enabled = True
            XmlTreeView.SelectedNode = e.Node
            ShowMessageOnStatusPanel("Выбран график " & TextBoxNameGraph.Text)
        End If
    End Sub

    Private Sub XmlTreeView_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles XmlTreeView.AfterSelect
        If e.Node.Tag IsNot Nothing AndAlso CType(e.Node.Tag, String).IndexOf(XmlNodeNameGraph) > -1 Then
            'e.Node.ImageIndex = XmlImage.Root0
            TextBoxNameGraph.Text = e.Node.Text
            ShowMessageOnStatusPanel("Выбран график " & TextBoxNameGraph.Text)
        End If
    End Sub

    ''' <summary>
    ''' Сохранить изменения
    ''' </summary>
    Private Sub TrySaveXMLGraphs()
        If isDirtyCurrentGraph Then
            Const caption As String = "Сохранение изменений"
            Dim text As String = "Были изменения при редактировании текущего графика." & vbCrLf & "Произвести сохранение изменений?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")
            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                If IsGeneralTestCorrectInput() Then
                    UpdateLimitationGraphs()
                    SaveXMLGraphs()
                Else
                    Exit Sub
                End If
            Else
                ShowMessageOnStatusPanel("Сохранение отменено")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub TextBoxNameGraph_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxNameGraph.TextChanged
        ButtonSaveGraph.Enabled = True
    End Sub

    ''' <summary>
    ''' Загрузить XML и парсить в дерево
    ''' </summary>
    Public Sub LoadGraphsXmlDoc()
        'Private Sub readXML()
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
        ''End Sub 'MainForm_Load

        Try
            XDoc = XDocument.Load(PathXmlFileGraphByParameters)
            XmlTreeView.Nodes.Clear()
            AddNodeAndChildren(XDoc.Root, Nothing)
        Catch ex As Exception
            Dim text As String = ex.ToString
            MessageBox.Show(text, NameOf(LoadGraphsXmlDoc), MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{NameOf(LoadGraphsXmlDoc)}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Рекурсивное добавление узлов в дерево проводника в зависимости от узлов XmlNode документа
    ''' </summary>
    ''' <param name="xnode"></param>
    ''' <param name="tnode"></param>
    Private Sub AddNodeAndChildren(ByVal xnode As XElement, ByVal tnode As TreeNode)
        Dim newChildTreeNode As TreeNode = AddNode(xnode, tnode)

        If Not (xnode.Attributes Is Nothing) Then
            For Each attribute As XAttribute In xnode.Attributes
                AddAttribute(attribute, newChildTreeNode)
            Next
        End If

        If xnode.HasElements Then
            For Each itemXElement As XElement In xnode.Elements
                AddNodeAndChildren(itemXElement, newChildTreeNode)
            Next
        End If
    End Sub

    Private Function AddNode(ByVal itemXElement As XElement, ByVal inTreeNode As TreeNode) As TreeNode
        Dim childNewTreeNode As TreeNode = Nothing
        Dim treeNodes As TreeNodeCollection
        Dim nodeName As String = String.Empty
        Dim imageIndex As Integer

        If inTreeNode Is Nothing Then
            treeNodes = XmlTreeView.Nodes ' корневой Root
        Else
            treeNodes = inTreeNode.Nodes
        End If

        Select Case itemXElement.Name
            Case XmlNodeParamGraphs
                imageIndex = XmlImage.Description1
                nodeName = "Имеющиеся Графики"
            Case XmlNodeNameGraph
                imageIndex = XmlImage.NameGraph2
                nodeName = itemXElement.Attributes(XmlAttributeDescription).First.Value
            Case XmlNodeAxis
                imageIndex = XmlImage.AxisParameterPoint3
                nodeName = itemXElement.Attributes(XmlAttributeAxisName).First.Value
            Case XmlNodeParameter
                imageIndex = XmlImage.AxisParameterPoint3
                nodeName = itemXElement.Attributes(XmlAttributeName).First.Value
            Case XmlNodeGraphTU
                imageIndex = XmlImage.GraphTU13
                nodeName = itemXElement.Attributes(XmlAttributeNameTU).First.Value
            Case XmlNodePoint
                imageIndex = XmlImage.AxisParameterPoint3
                nodeName = itemXElement.Attributes(XmlAttributeIndex).First.Value
            Case XmlNodeFormula
                If itemXElement.Nodes.Count > 0 Then
                    imageIndex = XmlImage.Formula8
                    nodeName = $"<{itemXElement.Name} {itemXElement.Value}>"
                End If
        End Select

        If nodeName <> String.Empty Then
            If itemXElement.Name = XmlNodeNameGraph Then
                childNewTreeNode = New TreeNode(nodeName, imageIndex, imageIndex) With {.Tag = XmlNodeNameGraph}
            Else
                childNewTreeNode = New TreeNode(nodeName, imageIndex, imageIndex)
            End If
            treeNodes.Add(childNewTreeNode)
        End If

        Return childNewTreeNode
    End Function

    Private Sub AddAttribute(ByVal attribute As XAttribute, ByVal inTreeNode As TreeNode)
        Dim imageIndex As Integer
        Dim nodeName As String = $"{attribute.Name}={attribute.Value}"
        Select Case attribute.Name
            Case XmlAttributeDescription
                imageIndex = XmlImage.Description1

            Case XmlAttributeAxisName
                imageIndex = XmlImage.Name4
            Case XmlAttributeMax
                imageIndex = XmlImage.Max5
            Case XmlAttributeMin
                imageIndex = XmlImage.Min6
            Case XmlAttributeIsUseFormula
                imageIndex = XmlImage.UseFormulaReduce7
                '-------------------------
            Case XmlAttributeName
                imageIndex = XmlImage.Name4
            Case XmlAttributeNameChannel
                imageIndex = XmlImage.NameChannel10
            Case XmlAttributeIndexParameter
                imageIndex = XmlImage.IndexParameter11
            Case XmlAttributeReduce
                imageIndex = XmlImage.UseFormulaReduce7
                '-------------------------
            Case XmlAttributeFormula
                imageIndex = XmlImage.Formula8

            Case XmlAttributeNameTU
                imageIndex = XmlImage.Name4
            Case XmlAttributeColor
                imageIndex = XmlImage.Color14
            Case XmlAttributeLineStyle
                imageIndex = XmlImage.LineStyle15
                '-------------------------
            Case XmlAttributeIndex
                imageIndex = XmlImage.Index9
            Case XmlAttributeX
                imageIndex = XmlImage.RulerXY16
            Case XmlAttributeY
                imageIndex = XmlImage.RulerXY16
        End Select

        inTreeNode.Nodes.Add(New TreeNode(nodeName, imageIndex, imageIndex))
    End Sub

#End Region

#Region "Function"
    ''' <summary>
    ''' Загрузка Каналов Модуля
    ''' </summary>
    Private Sub LoadChannels()
        ' Эта процедура отличается от аналогичной в FormConditionFind
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand
            Dim strSQL As String
            If IsCompactRio Then
                strSQL = $"Select * FROM {ChannelLast} WHERE UseCompactRio <> 0 Order By НомерПараметра"
            Else
                strSQL = $"Select * FROM {ChannelLast} Order By НомерПараметра"
            End If

            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cn.Open()
            Re.Dim(arrTypeNameUnit, NameParameters.Length - 1)

            Using rdr As OleDbDataReader = cmd.ExecuteReader
                Dim I As Integer
                ' загрузка по параметрам с базы с помощью запроса
                ' при добавлении полей надо модифицировать запрос в базе
                Do While rdr.Read
                    If Array.IndexOf(NameParameters, rdr("НаименованиеПараметра")) <> -1 Then
                        arrTypeNameUnit(I).NameParameter = CStr(rdr("НаименованиеПараметра"))
                        arrTypeNameUnit(I).UnitMeasure = CStr(rdr("ЕдиницаИзмерения"))
                        I += 1
                    End If
                Loop
            End Using
        End Using
        FillListParametersOnComboBox(ComboBoxAxisXFormulaParamaters)
        FillListParametersOnComboBox(ComboBoxAxisYFormulaParamaters)
    End Sub

    ''' <summary>
    ''' Заполнить ComboBox объектми StringIntObject
    ''' </summary>
    ''' <param name="cmb"></param>
    Private Sub FillListParametersOnComboBox(ByRef cmb As ComboBox)
        ' Эта процедура отличается от аналогичной в FormConditionFind
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

        For I As Integer = 0 To UBound(arrTypeNameUnit)
            If CBool(InStr(1, UnitOfMeasureString, arrTypeNameUnit(I).UnitMeasure)) Then
                cmb.Items.Add(New StringIntObject(arrTypeNameUnit(I).NameParameter, Array.IndexOf(UnitOfMeasureArray, arrTypeNameUnit(I).UnitMeasure)))
            Else
                cmb.Items.Add(New StringIntObject(arrTypeNameUnit(I).NameParameter, 1)) 'Discrete1
            End If
        Next

        cmb.SelectedIndex = 0
    End Sub

    Private Function CheckNameChannel(ByVal inNameChannel As String) As Boolean
        For I As Integer = 0 To UBound(arrTypeNameUnit)
            If arrTypeNameUnit(I).NameParameter = inNameChannel Then Return True
        Next

        Return False
    End Function

    Private Sub ButtonTestFormulaX_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaX.Click
        If TestFunctionParametr(DataGridViewAxisXARG, TextBoxMathAxisX.Text) Then ChangeAxesXYCaption()
    End Sub

    Private Sub ButtonTestFormulaY_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormulaY.Click
        If TestFunctionParametr(DataGridViewAxisYARG, TextBoxMathAxisY.Text) Then ChangeAxesXYCaption()
    End Sub

    Private Function TestFunction(ByVal expression As String) As Boolean
        Dim result As Boolean = mMathMethods.TestFunction(expression)
        ShowMessageOnStatusPanel(mMathMethods.TestResult)
        Return result
    End Function

    Private Function TestFunctionParametr(ByVal tempDataGridViewOs As DataGridView, ByVal expression As String) As Boolean
        For Each row As DataGridViewRow In tempDataGridViewOs.Rows
            If row.Index < tempDataGridViewOs.Rows.Count - 1 Then
                If row.Cells(1).Value.ToString = MissingParameter Then
                    Const caption As String = "Тест канала для параметра"
                    Dim text As String = $"Для аргумента формулы {row.Cells(0).Value} выбран несуществующий канал!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    ShowMessageOnStatusPanel("Имя канала для параметра неправильное")
                    Return False
                Else
                    If expression.IndexOf(row.Cells(0).Value.ToString) = -1 Then
                        Const caption As String = "Тест аргумента в формуле"
                        Dim text As String = $"Для параметра формулы {row.Cells(1).Value} соответствующий ему аргумент {row.Cells(0).Value} в формулу не введен!"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                        ShowMessageOnStatusPanel("Для параметра нет аргумента")
                        Return False
                    End If
                End If
            End If
        Next

        Return TestFunction(expression)
    End Function

    ''' <summary>
    ''' Изменить подписи Осей X и Y Графика
    ''' </summary>
    Private Sub ChangeAxesXYCaption()
        XAxis1.Caption = ChangeAxesCaption("X",
                                           RadioButtonAxisXOneParameter.Checked,
                                           ComboBoxAxisXFormulaParamaters.Text,
                                           CheckBoxAxisXParameterReduce.Checked,
                                           TextBoxMathAxisX.Text,
                                           DataGridViewAxisXARG)

        YAxis1.Caption = ChangeAxesCaption("Y",
                                           RadioButtonAxisYOneParameter.Checked,
                                           ComboBoxAxisYFormulaParamaters.Text,
                                           CheckBoxAxisYParameterReduce.Checked,
                                           TextBoxMathAxisY.Text,
                                           DataGridViewAxisYARG)
    End Sub

    ''' <summary>
    ''' Параметризованная функция изменения подписи Оси Графика
    ''' </summary>
    ''' <param name="nameAxis"></param>
    ''' <param name="isAxisOneParameter"></param>
    ''' <param name="inParameter"></param>
    ''' <param name="isParameterReduce"></param>
    ''' <param name="expression"></param>
    ''' <param name="inDataGridViewAxisARG"></param>
    ''' <returns></returns>
    Private Function ChangeAxesCaption(nameAxis As String,
                                       isAxisOneParameter As Boolean,
                                       inParameter As String,
                                       isParameterReduce As Boolean,
                                       expression As String,
                                       inDataGridViewAxisARG As DataGridView) As String
        Dim captionAxis As String

        If isAxisOneParameter Then
            ' ось с одним параметром
            If inParameter = MissingParameter Then
                Const caption As String = "Тест канала для параметра"
                Dim text As String = $"Для параметра оси {nameAxis} выбран несуществующий канал!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                captionAxis = MissingParameter
            Else
                If isParameterReduce Then
                    ' параметр приводится к норм. условиям
                    captionAxis = inParameter & " приведенный"
                Else
                    captionAxis = inParameter
                End If
            End If
        Else
            ' ось с формулой
            For Each rows As DataGridViewRow In inDataGridViewAxisARG.Rows
                If rows.Index < inDataGridViewAxisARG.Rows.Count - 1 AndAlso rows.Cells(1).Value.ToString <> MissingParameter Then
                    'MessageBox.Show("Для аргумента формулы " & rows.Cells(0).Value.ToString & " выбран несуществующий канал!", "Тест канала для параметра", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Return False
                    'Else
                    If expression.IndexOf(rows.Cells(0).Value.ToString) <> -1 Then
                        'MessageBox.Show("Для параметра формулы " & rows.Cells(1).Value.ToString & " соответствующий ему аргумент " & rows.Cells(0).Value.ToString & " в формулу не введен!", "Тест аргумента в формуле", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Return False
                        expression = expression.Replace(rows.Cells(0).Value.ToString, rows.Cells(1).Value.ToString)
                    End If
                End If
            Next

            If expression.Length > 40 Then expression = expression.Substring(0, 40) & "..."
            captionAxis = expression
        End If

        Return captionAxis
    End Function

    ''' <summary>
    ''' Общий Тест Корректного Ввода
    ''' </summary>
    ''' <returns></returns>
    Private Function IsGeneralTestCorrectInput() As Boolean
        If Not RadioButtonAxisXOneParameter.Checked AndAlso Not TestFunctionParametr(DataGridViewAxisXARG, TextBoxMathAxisX.Text) Then Return False
        If Not RadioButtonAxisYOneParameter.Checked AndAlso Not TestFunctionParametr(DataGridViewAxisYARG, TextBoxMathAxisY.Text) Then Return False

        ChangeAxesXYCaption()
        Return True
    End Function

    Private Sub PropertyEditorPlotColor_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotColor.TextChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 OrElse isFormClosing Then Exit Sub

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)

        selectedCurveGraph.Color = PropertyEditorPlotColor.Source.Value.Name
        isDirtyCurrentGraph = True
    End Sub

    Private Sub PropertyEditorPlotStile_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PropertyEditorPlotStile.TextChanged
        If ListBoxLimitationGraphs.SelectedIndex = -1 OrElse isFormClosing Then Exit Sub

        Dim selectedCurveGraph As CurveGraph = CType(ListBoxLimitationGraphs.SelectedItem, CurveGraph)

        selectedCurveGraph.LineStyle = PropertyEditorPlotStile.Source.Value.ToString
        isDirtyCurrentGraph = True
    End Sub

#End Region

#Region "Event Menu"
    Private Sub TSMenuItemExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemExit.Click
        Close()
    End Sub

    Private Sub TSMenuItemSaveGraf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemSaveGraf.Click
        InputDescriptionToGraph()
        Dim ConvertColor As New ConvertColorForGraph(ScatterGraphParam, False)
        ScatterGraphParam.BackColor = Color.White
        ScatterGraphParam.Caption = CaptionGraf
        'ScatterGraphParam.Cursors.Item(1).Visible = False
        'XyCursor1.Visible = False

        Try
            ConvertColor.ChangeColorOnGraph(Color.White, Color.Black)
            Dim graphSave As New GraphSave(ScatterGraphParam)
            graphSave.Save()
        Catch ex As Exception
            Const caption As String = "Ошибка записи графика"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            ConvertColor.ChangeColorOnGraph(Color.Black, Color.White)
            'If РежимСнимок Then
            '    'ScatterGraphParam.Cursors.Item(1).Visible = True
            '    XyCursor1.Visible = True
            'End If
            ScatterGraphParam.Caption = ""
        End Try
    End Sub

    Private Sub TSMenuItemPrintGraph_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemPrintGraph.Click
        Dim caption As String = "Печать графика"
        Dim text As String = "Печать на весь лист?"
        RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

        If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            ' пользователь нажал да.
            WindowState = FormWindowState.Normal
            If Screen.PrimaryScreen.Bounds.Width = 1024 OrElse Screen.PrimaryScreen.Bounds.Height = 768 Then
                Width = 1036
                Height = 780
                caption = "Печать графика"
                text = "Установите альбомную ориентацию!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                PrintGraph()
                WindowState = FormWindowState.Maximized
            ElseIf Screen.PrimaryScreen.Bounds.Width > 1024 OrElse Screen.PrimaryScreen.Bounds.Height > 768 Then
                Width = 762
                Height = 1036
                PrintGraph()
                WindowState = FormWindowState.Maximized
            Else
                PrintGraph()
            End If
        Else
            PrintGraph()
        End If
    End Sub

    Private Sub PrintGraph()
        InputDescriptionToGraph()
        Dim ConvertColor As ConvertColorForGraph = New ConvertColorForGraph(ScatterGraphParam, False)
        ScatterGraphParam.BackColor = Color.White
        ScatterGraphParam.Caption = CaptionGraf
        'ScatterGraphParam.Cursors.Item(0).Visible = False
        'XyCursor1.Visible = False
        Try
            'ConvertColor.ChangeColorOnGraph(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black))
            ConvertColor.ChangeColorOnGraph(Color.White, Color.Black)
            Dim printer As GraphPrinter = New GraphPrinter(ScatterGraphParam)
            printer.Print()
        Catch ex As Exception
            Const caption As String = "Ошибка печати графика"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            'ConvertColor.ChangeColorOnGraph(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White))
            ConvertColor.ChangeColorOnGraph(Color.Black, Color.White)
            'If РежимСнимок Then
            '    'ScatterGraphParam.Cursors.Item(0).Visible = True
            '    'XyCursor1.Visible = True
            'End If
            ScatterGraphParam.Caption = ""
        End Try
    End Sub

    ''' <summary>
    ''' Ввести Примечание
    ''' </summary>
    Private Sub InputDescriptionToGraph()
        strBox = InputBox("Введите текст примечания к графику (не обязательно)", CaptionGraf, strBox)
        If strBox <> "" AndAlso CaptionGraf.LastIndexOf(strBox) = -1 Then CaptionGraf = $"{CaptionGraf} {strBox}"
    End Sub

    Private Sub ButtonFindChannelAxisXFormula_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelAxisXFormula.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxAxisXFormulaParamaters)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub ButtonFindChannelAxisYFormula_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelAxisYFormula.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxAxisYFormulaParamaters)
        mSearchChannel.SelectChannel()
    End Sub
#End Region

End Class