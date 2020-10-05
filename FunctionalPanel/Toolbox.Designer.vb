<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Toolbox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.toolboxTitleButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'toolboxTitleButton
        '
        Me.toolboxTitleButton.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.toolboxTitleButton.Dock = System.Windows.Forms.DockStyle.Top
        Me.toolboxTitleButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.toolboxTitleButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.toolboxTitleButton.Location = New System.Drawing.Point(0, 0)
        Me.toolboxTitleButton.Name = "toolboxTitleButton"
        Me.toolboxTitleButton.Size = New System.Drawing.Size(127, 20)
        Me.toolboxTitleButton.TabIndex = 2
        Me.toolboxTitleButton.Text = "Toolbox"
        Me.toolboxTitleButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.toolboxTitleButton.UseVisualStyleBackColor = False
        '
        'Toolbox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.toolboxTitleButton)
        Me.Name = "Toolbox"
        Me.Size = New System.Drawing.Size(127, 283)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents toolboxTitleButton As System.Windows.Forms.Button

End Class
