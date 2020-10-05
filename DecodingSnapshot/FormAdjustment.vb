Public Class FormAdjustment
    Public ReadOnly Property N1НастройкаКРД() As Double
        Get
            Return CalcKrd.N1НастройкаКРД
        End Get
    End Property

    Public ReadOnly Property N2НастройкаКРД() As Double
        Get
            Return CalcKrd.N2НастройкаКРД
        End Get
    End Property

    Private mТ4КРД As Double
    Public ReadOnly Property Т4КРД() As Double
        Get
            Return mТ4КРД
        End Get
    End Property

    Private CalcKrd As KRD
    Private sTypeKRDSnapshot As String ' Тип Крд В Снимке

    Public Sub New(ByVal inTypeKRDSnapshot As String)
        sTypeKRDSnapshot = inTypeKRDSnapshot
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавить код инициализации после вызова InitializeComponent().
    End Sub

    Private Sub FormAdjustment_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' 13)Т4 настройки КРД
        mТ4КРД = CSng(GetIni(PathOptions, "KRD", "T4KRD", "840"))
        TextT4KRD.Text = CStr(Т4КРД)
        '14)Настройка N1 КРД при +15 град.
        TextN1KRD15.Text = Format(CDbl(GetIni(PathOptions, "KRD", "N1KRD", "90")), "##0.0##")
        '15)Настройка N2 КРД при +15 град.
        TextN2KRD15.Text = Format(CDbl(GetIni(PathOptions, "KRD", "N2KRD", "90")), "##0.0##")
        TextTbox.Text = CStr(Math.Round(TemperatureBoxInSnaphot, 2))

        ' проанализировать наличие sТипКрдВСнимке и в случае отсутствия присвоить по умолчанию
        Dim mKRDsManager As New KRDsManager

        If Not mKRDsManager.AllKRDKeysToArray.Contains(sTypeKRDSnapshot) Then
            MessageBox.Show(Me, $"При записи испытания был введён тип КРД с названием: {sTypeKRDSnapshot} !" & Environment.NewLine &
                            $"Этот тип неправильный или устарел и при расшифровке будет заменён на тип с именем: {cKRD_B}",
                            "Неправильный тип КРД", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            sTypeKRDSnapshot = cKRD_B
        End If

        TextTypeKRD.Text = sTypeKRDSnapshot
        ' по типу typeKrdInSnapshot фабричный метод метод создаёт экземпляр класса, унаследоанного от базового класса KRD
        CalcKrd = mKRDsManager(sTypeKRDSnapshot)
        Calculate()
    End Sub

    Private Sub Calculate()
        Dim inTbox, inN1КРД15, inN2КРД15 As Double ' Настройка N1 N2 КРД при +15 град.

        If Double.TryParse(TextTbox.Text, inTbox) AndAlso
            Double.TryParse(TextT4KRD.Text, mТ4КРД) AndAlso
            Double.TryParse(TextN1KRD15.Text, inN1КРД15) AndAlso
            Double.TryParse(TextN2KRD15.Text, inN2КРД15) Then

            CalcKrd.Calculate(inTbox, inN1КРД15, inN2КРД15)
            TextN1KRD.Text = Format(CalcKrd.N1НастройкаКРД, "fixed")
            TextN2KRD.Text = Format(CalcKrd.N2НастройкаКРД, "fixed")
            WriteSetting()
        Else
            MessageBox.Show(Me, "В полях ввода должны быть цифровые значения.", "Некорректный ввод", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    Private Sub WriteSetting()
        '13)Т4 настройки КРД
        WriteINI(PathOptions, "KRD", "T4KRD", CStr(Т4КРД))
        '14)Настройка N1 КРД при +15 град.
        WriteINI(PathOptions, "KRD", "N1KRD", CStr(CSng(TextN1KRD15.Text)))
        '15)Настройка N2 КРД при +15 град.
        WriteINI(PathOptions, "KRD", "N2KRD", CStr(CSng(TextN2KRD15.Text)))
    End Sub

    Private Sub ButtonCalculate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCalculate.Click
        Calculate()
    End Sub

    Private Sub ButtonApply_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonApply.Click
        Calculate()
        Me.Close()
    End Sub
End Class