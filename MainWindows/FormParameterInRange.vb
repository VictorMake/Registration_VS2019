Friend Class FormParameterInRange
    ''' <summary>
    ''' ссылка на форму откуда вызвана
    ''' </summary>
    ''' <returns></returns>
    Friend Property ParentFormMain() As FormMain

    Public Sub New(ByVal inParentForm As FormMain)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        Me.ParentFormMain = inParentForm
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        Tag = "ПараметрВДиапазоне"
        CollectionForms.Add(Text, Me)
    End Sub

    Private Sub CWNumMin_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles CWNumMin.AfterChangeValue
        MinLimitParameter = CDbl(e.NewValue)
    End Sub

    Private Sub CWNumMax_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles CWNumMax.AfterChangeValue
        MaxLimitParameter = CDbl(e.NewValue)
    End Sub

    Private Sub FormParameterInRange_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        MinLimitParameter = CWNumMin.Value
        MaxLimitParameter = CWNumMax.Value
        ParentFormMain.IsShowFormParametersInRange = True
    End Sub

    Private Sub FormParameterInRange_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormMain.IsShowFormParametersInRange = False
        ParentFormMain = Nothing
        CollectionForms.Remove(Text)
    End Sub
End Class