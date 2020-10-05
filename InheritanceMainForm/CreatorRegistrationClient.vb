Friend Class CreatorRegistrationClient
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormRegistrationClient As IMdiChildrenWindow = New FormRegistrationClient(inMainForm, FormExamination.RegistrationClient, captionText)
        Return newFormRegistrationClient
    End Function
End Class
