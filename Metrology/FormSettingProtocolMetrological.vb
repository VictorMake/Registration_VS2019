Public Class FormSettingProtocolMetrological
    ''' <summary>
    ''' MinФизика
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PhysicalMin() As Double
        Get
            Return mPhysicalMin
        End Get
    End Property
    ''' <summary>
    ''' MaxФизика
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PhysicalMax() As Double
        Get
            Return mPhysicalMax
        End Get
    End Property
    ''' <summary>
    ''' MinАцп
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ADCmin() As Double
        Get
            Return mADCmin
        End Get
    End Property
    ''' <summary>
    ''' MaxАцп
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ADCmax() As Double
        Get
            Return mADCmax
        End Get
    End Property

    Private mPhysicalMin, mPhysicalMax, mADCmin, mADCmax As Double
    Private success As Boolean

    Private Sub Calculate()
        success = True

        If Not CheckTextBox() Then
            success = False
            Exit Sub
        End If

        Dim tempPhysicalMin As Double = CDbl(TextBoxPhysicalMin.Text)
        Dim tempPhysicalMax As Double = CDbl(TextBoxPhysicalMax.Text)
        Dim tempADCmin As Double = CDbl(TextBoxADCmin.Text)
        Dim tempADCmax As Double = CDbl(TextBoxADCmax.Text)

        If tempPhysicalMax <= tempPhysicalMin OrElse tempADCmax <= tempADCmin OrElse tempADCmin < 0.0 OrElse tempADCmax > 10.0 Then
            MessageBox.Show($"Введены некорректные значения!{vbCrLf}Диапазон АЦП должен быть в пределах <0 - 10> вольт.{vbCrLf}Значения <min> физики или АЦП должны быть меньше <max>.",
                            "Ввод новых данных", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            success = False
            Exit Sub
        End If

        mPhysicalMin = tempPhysicalMin
        mPhysicalMax = tempPhysicalMax
        mADCmin = tempADCmin
        mADCmax = tempADCmax
        ' прибавка АЦП
        Dim deltaADC As Double = (tempADCmax - tempADCmin) / PointTarirCount
        ' прибавка Физика
        Dim deltaPhysical As Double = Math.Round((tempPhysicalMax - tempPhysicalMin) / PointTarirCount, 2)
        Dim items(2) As String

        ListViewMetrological.Items.Clear()

        For I As Integer = 1 To PointTarirCount
            items(0) = I.ToString
            items(1) = Format(tempPhysicalMin, "###0.0#")
            items(2) = Format(tempADCmin, "##0.0###")
            ListViewMetrological.Items.Add(New ListViewItem(items))
            tempADCmin += deltaADC
            tempPhysicalMin += deltaPhysical
        Next
    End Sub

    ''' <summary>
    ''' Проверка Заполнения Полей
    ''' возврат false - есть Незаполненные Поля
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckTextBox() As Boolean
        Return IsTextBoxNotEmpty("Физика Мин", TextBoxPhysicalMin) AndAlso
                IsDigitCheck("Физика Мин", TextBoxPhysicalMin.Text) AndAlso
                IsTextBoxNotEmpty("Физика Макс", TextBoxPhysicalMax) AndAlso
                IsDigitCheck("Физика Макс", TextBoxPhysicalMax.Text) AndAlso
                IsTextBoxNotEmpty("АЦП Мин", TextBoxADCmin) AndAlso
                IsDigitCheck("АЦП Мин", TextBoxADCmin.Text) AndAlso
                IsTextBoxNotEmpty("АЦП Макс", TextBoxADCmax) AndAlso
                IsDigitCheck("АЦП Макс", TextBoxADCmax.Text)
    End Function

    ''' <summary>
    ''' Проверка На Пустое Поле
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="inTextBox"></param>
    ''' <returns></returns>
    Private Function IsTextBoxNotEmpty(ByVal text As String, ByVal inTextBox As TextBox) As Boolean
        If inTextBox.Text = vbNullString Then
            MessageBox.Show($"В поле ({text}) ни чего не заведено!{vbCrLf}Необходимо заполнить все поля.",
                            "Ввод новых данных", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub ButtonCalculate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCalculate.Click
        Calculate()
    End Sub

    Private Sub ButtonApply_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonApply.Click
        Close()
    End Sub

    Private Sub FormSettingProtocolMetrological_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ListViewMetrological.Columns.Clear()
        ListViewMetrological.Items.Clear()
        ListViewMetrological.Columns.Add("№", (ListViewMetrological.Width * 1 \ 5) - 1, HorizontalAlignment.Center)
        ListViewMetrological.Columns.Add("Эталон", (ListViewMetrological.Width * 2 \ 5) - 1, HorizontalAlignment.Right)
        ListViewMetrological.Columns.Add("АЦП", (ListViewMetrological.Width * 2 \ 5) - 1, HorizontalAlignment.Right)
    End Sub

    Private Sub FormSettingProtocolMetrological_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Calculate()
        e.Cancel = Not success
    End Sub
End Class