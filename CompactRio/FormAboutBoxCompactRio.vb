Public NotInheritable Class FormAboutBoxCompactRio

    Private Sub AboutBoxProgramm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Установить заголовок формы.
        Dim ApplicationTitle As String

        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Text = $"О программе {ApplicationTitle}"
        ' Инициализировать текст, отображаемый в окне "О программе".
        LabelProductName.Text = My.Application.Info.ProductName
        LabelVersion.Text = $"Версия {My.Application.Info.Version}"
        LabelCopyright.Text = My.Application.Info.Copyright
        LabelCompanyName.Text = My.Application.Info.CompanyName
        TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OKButton.Click
        Close()
    End Sub
End Class