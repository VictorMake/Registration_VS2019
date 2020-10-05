Imports System.Data.OleDb
Imports MathematicalLibrary

''' <summary>
''' Формула условия срабатывания цифровых линий порта
''' Редактор формулы для управления исполнительными устройствами
''' </summary>
Friend Class FormFormulaEditor
    ''' <summary>
    ''' Формула условия срабатывания цифровых линий порта
    ''' </summary>
    ''' <returns></returns>
    Public Property Formula() As String
        Get
            Return GetFormula()
        End Get
        Set(ByVal value As String)
            SetFormula(value)
        End Set
    End Property

    Public Sub New(ByVal mFormula As String)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        FormulaEditorFormLoad()
        Formula = mFormula
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
    End Sub

    Private mFormulaFireDiscretInput As ФормулаСрабатыванияЦифровогоВыхода
    Private NameParameters() As String

    Private Structure MyTypeNameUnit
        Dim NameParameter As String  'наименование Параметра
        Dim UnitMeasure As String ' единица Измерения
    End Structure
    Private arrMyTypeNameUnit() As MyTypeNameUnit

    ''' <summary>
    ''' ' были изменения в текущем графике
    ''' </summary>
    Private isDirtyCurrentGraph As Boolean
    Private mMathMethods As New MathMethods
    Private Const NameColumnName As String = "DataGridViewComboBoxColumnName"

    Private Sub FormulaEditorFormLoad()
        If DigitalPortForm.FormMainReference Is Nothing Then
            'ReDim_CopyListOfParameter(ParametersType.Length - 1)
            Re.Dim(CopyListOfParameter, ParametersType.Length - 1)
            For I As Integer = 1 To ParametersType.Length - 1
                CopyListOfParameter(I) = I
            Next
        End If

        'ReDim_NameParameters(UBound(CopyListOfParameter) - 1)
        Re.Dim(NameParameters, UBound(CopyListOfParameter) - 1)
        For I = 1 To UBound(CopyListOfParameter)
            NameParameters(I - 1) = ParametersType(CopyListOfParameter(I)).NameParameter
        Next

        Dim arrTemp(NameParameters.Length) As String
        Array.ConstrainedCopy(NameParameters, 0, arrTemp, 1, NameParameters.Length)
        arrTemp(0) = MissingParameter

        Dim TempComboBoxColumnИмяПараметраИзмерения As DataGridViewComboBoxColumn = CType(DataGridViewARG.Columns(NameColumnName), DataGridViewComboBoxColumn)
        TempComboBoxColumnИмяПараметраИзмерения.Items.AddRange(arrTemp)
        LoadChannels()
        isDirtyCurrentGraph = False

        PopulateMathComboBox()
    End Sub

    Private Sub SetFormula(value As String)
        TextBoxMathExpression.Text = value
        DataGridViewARG.Rows.Clear()
        DigitalPortForm.FindLastNode(TypeNode)

        If CType(DigitalPortForm.DtvwDirectory.CurrentTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Formula Then
            mFormulaFireDiscretInput = CType(DigitalPortForm.DtvwDirectory.CurrentTreeNode.Tag, ФормулаСрабатыванияЦифровогоВыхода)

            If mFormulaFireDiscretInput.АргументыДляФормулы IsNot Nothing Then
                For Each itemARG As АргументДляФормулы In mFormulaFireDiscretInput.АргументыДляФормулы
                    Dim nameARG As String = itemARG.Name
                    Dim nameChannel As String = itemARG.ИмяКанала

                    Dim heter_row As DataGridViewRow = New DataGridViewRow
                    ' создаем строку, считывая описания колонок с _grid
                    heter_row.CreateCells(DataGridViewARG)
                    heter_row.Cells(0).Value = CType(nameARG, Object)
                    ' сделать проверку на присутствие канала
                    If CheckNameChannel(nameChannel) Then
                        'cmbПараметрФорма.Text = TempУсловие.ИмяФормаПараметр
                        heter_row.Cells(1).Value = DataGridViewComboBoxColumnName.Items(DataGridViewComboBoxColumnName.Items.IndexOf(nameChannel))
                    Else
                        heter_row.Cells(1).Value = DataGridViewComboBoxColumnName.Items(0)
                        'cmbПараметрФорма.SelectedIndex = 0
                    End If

                    DataGridViewARG.Rows.Add(heter_row)
                Next
            End If

            IsGeneralTestCorrectInput()
            UpdateDataGridView()
            isDirtyCurrentGraph = False
        End If
    End Sub

    Private Function GetFormula() As String
        Dim outFormula As String = TextBoxMathExpression.Text

        If isDirtyCurrentGraph Then
            If IsGeneralTestCorrectInput() Then
                If mFormulaFireDiscretInput IsNot Nothing Then
                    mFormulaFireDiscretInput.АргументыДляФормулы.Clear()

                    For Each itemRow As DataGridViewRow In DataGridViewARG.Rows
                        If itemRow.Index < DataGridViewARG.Rows.Count - 1 Then
                            'crАргументДляФормулы.Приведение = rowАргумент.Приведение
                            Dim crАргументДляФормулы As АргументДляФормулы = New АргументДляФормулы(itemRow.Cells(0).Value.ToString) With {
                                .Parent = mFormulaFireDiscretInput,
                                .ИмяКанала = itemRow.Cells(1).Value.ToString,
                                .КеуArgument = DigitalPortForm.GetKeyId(),
                                .KeyFormula = mFormulaFireDiscretInput.KeyFormula
                            }
                            mFormulaFireDiscretInput.АргументыДляФормулы.Add(crАргументДляФормулы)
                        End If
                    Next
                End If
            Else
                Return "Исправить " & outFormula
            End If
        End If

        Return outFormula
    End Function

    Private Sub PopulateMathComboBox()
        mMathMethods.PopulateMath()

        For Each tempMethod As MathMethods.Method In mMathMethods.GetMethods.Values
            TSComboBoxMathExpression.Items.Add(tempMethod.NameMethod)
        Next

        TSComboBoxMathExpression.SelectedIndex = 0
    End Sub

    Private Sub DataGridViewARG_UserAddedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewARG.UserAddedRow
        DataGridViewARG.Rows(e.Row.Index - 1).Cells(0).Value = "ARG" & DataGridViewARG.RowCount - 1
        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewARG_UserDeletedRow(ByVal sender As Object, ByVal e As DataGridViewRowEventArgs) Handles DataGridViewARG.UserDeletedRow
        For I As Integer = 0 To DataGridViewARG.RowCount - 2
            DataGridViewARG.Rows(I).Cells(0).Value = "ARG" & (I + 1).ToString
        Next

        UpdateDataGridView()
        isDirtyCurrentGraph = True
    End Sub

    Private Sub TextBoxMathExpression_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathExpression.TextChanged
        isDirtyCurrentGraph = True
    End Sub

    Private Sub DataGridViewARG_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridViewARG.CellValueChanged
        If IsHandleCreated Then isDirtyCurrentGraph = True
    End Sub

    Private Sub TSButtonAddMathExpression_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathExpression.Click
        Dim mathText As String = mMathMethods.GetMethods(TSComboBoxMathExpression.Text).TextMethod
        TextBoxMathExpression.SelectedText = mathText
        ShowMessageOnStatusPanel("В формулу добавлено математическое выражение " & mathText)
    End Sub

    Private Sub TSButtonAddArgument_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.Click
        If TSComboBoxArguments.SelectedIndex = -1 Then Exit Sub
        Dim textArg As String = TSComboBoxArguments.Text

        TextBoxMathExpression.SelectedText = textArg
        ShowMessageOnStatusPanel("В формулу добавлено имя аргумента " & textArg)
    End Sub

    Private Sub UpdateDataGridView()
        TSComboBoxArguments.Items.Clear()
        For Each rows As DataGridViewRow In DataGridViewARG.Rows
            If rows.Index < DataGridViewARG.Rows.Count - 1 Then
                TSComboBoxArguments.Items.Add(rows.Cells(0).Value.ToString)
            End If
        Next
        If TSComboBoxArguments.Items.Count > 0 Then TSComboBoxArguments.SelectedIndex = 0
    End Sub

#Region "ToolTip"
    Private Sub ShowMessageOnStatusPanel(ByVal strMessage As String)
        TSStatusMessageLabel.Text = strMessage
    End Sub

    Private Sub CleareStatusLabelMessage()
        TSStatusMessageLabel.Text = ""
    End Sub

    Private Sub TSButtonAddArgument_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.MouseEnter
        ShowMessageOnStatusPanel("Вставить в формулу имя аргумента")
    End Sub

    Private Sub TSButtonAddArgument_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddArgument.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    Private Sub TSButtonAddMathExpression_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathExpression.MouseEnter
        ShowMessageOnStatusPanel("Вставить в формулу математическое выражение")
    End Sub

    Private Sub TSButtonAddMathExpression_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonAddMathExpression.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    Private Sub ButtonTestFormula_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormula.MouseEnter
        ShowMessageOnStatusPanel("Тест формулы для проверки корректности")
    End Sub

    Private Sub ButtonTestFormula_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormula.MouseLeave
        CleareStatusLabelMessage()
    End Sub

    Private Sub TextBoxMathExpression_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathExpression.MouseEnter
        ShowMessageOnStatusPanel("Текст формулы")
    End Sub

    Private Sub TextBoxMathExpression_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxMathExpression.MouseLeave
        CleareStatusLabelMessage()
    End Sub
#End Region

#Region "Function"
    ''' <summary>
    ''' Загрузка Каналов Модуля
    ''' </summary>
    Private Sub LoadChannels()
        Dim I As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim strSQL As String = "SELECT * FROM " & ChannelLast

        cmd.CommandType = CommandType.Text
        cn.Open()
        'ReDim_arrMyTypeNameUnit(NameParameters.Length - 1)
        Re.Dim(arrMyTypeNameUnit, NameParameters.Length - 1)
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        ' при добавлении полей надо модифицировать запрос в базе
        Do While rdr.Read
            If Array.IndexOf(NameParameters, rdr("НаименованиеПараметра")) <> -1 Then
                arrMyTypeNameUnit(I).NameParameter = CStr(rdr("НаименованиеПараметра"))
                arrMyTypeNameUnit(I).UnitMeasure = CStr(rdr("ЕдиницаИзмерения"))
                I += 1
            End If
        Loop

        rdr.Close()
        cn.Close()
    End Sub

    Private Function CheckNameChannel(ByVal inNameChannel As String) As Boolean
        For I As Integer = 0 To UBound(arrMyTypeNameUnit)
            If arrMyTypeNameUnit(I).NameParameter = inNameChannel Then Return True
        Next

        Return False
    End Function

    Private Sub ButtonTestFormula_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTestFormula.Click
        TestFunctionParametr(DataGridViewARG, TextBoxMathExpression.Text)
    End Sub

    Private Function TestFunction(ByVal expression As String) As Boolean
        Dim result As Boolean = mMathMethods.TestFunction(expression)
        ShowMessageOnStatusPanel(mMathMethods.TestResult)
        Return result
    End Function

    Private Function TestFunctionParametr(ByVal DataGridViewOsTemp As DataGridView, ByVal Expression As String) As Boolean
        For Each rows As DataGridViewRow In DataGridViewOsTemp.Rows
            If rows.Index < DataGridViewOsTemp.Rows.Count - 1 Then
                If rows.Cells(1).Value.ToString = MissingParameter Then
                    Const caption As String = "Тест канала для параметра"
                    Dim text As String = $"Для аргумента формулы {rows.Cells(0).Value} выбран несуществующий канал!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    ShowMessageOnStatusPanel("Имя канала для параметра неправильное")
                    Return False
                Else
                    If Expression.IndexOf(rows.Cells(0).Value.ToString) = -1 Then
                        Const caption As String = "Тест аргумента в формуле"
                        Dim text As String = $"Для параметра формулы {rows.Cells(1).Value} соответствующий ему аргумент {rows.Cells(0).Value} в формулу не введен!"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                        ShowMessageOnStatusPanel("Для параметра нет аргумента")
                        Return False
                    End If
                End If
            End If
        Next

        Return TestFunction(Expression)
    End Function

    ''' <summary>
    ''' Общий Тест Корректного Ввода
    ''' </summary>
    ''' <returns></returns>
    Private Function IsGeneralTestCorrectInput() As Boolean
        If Not TestFunctionParametr(DataGridViewARG, TextBoxMathExpression.Text) Then Return False
        Return True
    End Function
#End Region

End Class