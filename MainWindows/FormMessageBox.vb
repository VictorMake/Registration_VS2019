Public Class FormMessageBox
    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
    End Sub

    Public Sub New(message As String, capption As String)
        MyBase.New()

        InitializeComponent()

        Height = 63
        Width = message.Length * 16
        Text = capption
        lblСообщение.Text = message
    End Sub
End Class