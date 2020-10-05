Friend Class CreatorSnapshotPhotograph
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormSnapshotPhotograph As IMdiChildrenWindow = New FormSnapshotPhotograph(inMainForm, FormExamination.SnapshotPhotograph, captionText)
        'newFormSnapshotPhotograph.Set**** (20.3)
        Return newFormSnapshotPhotograph
    End Function
End Class
