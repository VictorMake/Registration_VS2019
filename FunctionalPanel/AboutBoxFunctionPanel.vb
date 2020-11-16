Public NotInheritable Class AboutBoxFunctionPanel

    Private Sub AboutBoxFunctionPanel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Установить заголовок формы.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Text = String.Format("О программе {0}", ApplicationTitle)
        ' Инициализировать текст, отображаемый в окне "О программе".
        ' настроить сведения о сборке приложения в области "Приложение" диалогового окна 
        '    свойств проекта (в меню "Проект").
        LabelProductName.Text = My.Application.Info.ProductName
        LabelVersion.Text = String.Format("Версия {0}", My.Application.Info.Version.ToString)
        LabelCopyright.Text = My.Application.Info.Copyright
        LabelCompanyName.Text = My.Application.Info.CompanyName
        TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OKButton.Click
        Close()
    End Sub

End Class
