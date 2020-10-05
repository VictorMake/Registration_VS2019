Public Class FormDialogRangeEditor

    Private mainRangeEditorUI As RangeEditorUI

    Public Sub New(ByVal minimum As Double, ByVal maximum As Double)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        okButton.DialogResult = DialogResult.OK
        CancelButton.DialogResult = DialogResult.Cancel
        StartPosition = FormStartPosition.CenterParent
        mainRangeEditorUI = New RangeEditorUI(Minimum, Maximum)
        Controls.Add(mainRangeEditorUI)
    End Sub

    Public ReadOnly Property Maximum() As Double
        Get
            Return mainRangeEditorUI.Maximum
        End Get
    End Property

    Public ReadOnly Property Minimum() As Double
        Get
            Return mainRangeEditorUI.Minimum
        End Get
    End Property

End Class