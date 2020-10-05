Friend Class CreatorSnapshotViewingDiagram
    Inherits Creator

    Protected Overrides Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Dim newFormSnapshotViewingDiagram As IMdiChildrenWindow = New FormSnapshotViewingDiagram(inMainForm, FormExamination.SnapshotViewingDiagram, captionText)
        'newFormSnapshotViewingDiagram.Set**** (20.3)
        Return newFormSnapshotViewingDiagram
    End Function
End Class
