Friend Class CreatorRegistrationTCP
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormRegistrationTCP As IMdiChildrenWindow = New FormRegistrationTCP(inMainForm, FormExamination.RegistrationTCP, captionText)
        'newFormRegistrationTCP.Set**** (20.3)
        Return newFormRegistrationTCP
    End Function
End Class
