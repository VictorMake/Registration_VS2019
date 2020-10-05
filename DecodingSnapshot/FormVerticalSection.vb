Public Class FormVerticalSection
    Private Sub FormVerticalSection_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, 152) '166) 'Me.Height)
        ListViewGrid.SetBounds(8, 8, Me.Width - 20, Me.Height - 36)
    End Sub
End Class