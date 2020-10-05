Friend Class CreatorRegistrationSCXI
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormRegistrationSCXI As IMdiChildrenWindow = New FormRegistrationSCXI(inMainForm, FormExamination.RegistrationSCXI, captionText)
        'newFormRegistrationSCXI.Set**** (20.3)
        Return newFormRegistrationSCXI
    End Function
End Class
