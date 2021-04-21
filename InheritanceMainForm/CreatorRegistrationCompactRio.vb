Friend Class CreatorRegistrationCompactRio
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormRegistrationCompactRio As IMdiChildrenWindow = New FormRegistrationCompactRio(inMainForm, FormExamination.RegistrationCompactRio, captionText)
        'newFormRegistrationTCP.Set**** (20.3)
        Return newFormRegistrationCompactRio
    End Function
End Class
