<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDialogRangeEditor
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.canButton = New System.Windows.Forms.Button()
        Me.okButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'canButton
        '
        Me.canButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.canButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.canButton.Location = New System.Drawing.Point(108, 60)
        Me.canButton.Name = "canButton"
        Me.canButton.Size = New System.Drawing.Size(75, 23)
        Me.canButton.TabIndex = 5
        Me.canButton.Text = "Cancel"
        '
        'okButton
        '
        Me.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.okButton.Location = New System.Drawing.Point(12, 60)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 4
        Me.okButton.Text = "OK"
        '
        'RangeEditorDlg2
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.canButton
        Me.ClientSize = New System.Drawing.Size(194, 95)
        Me.Controls.Add(Me.canButton)
        Me.Controls.Add(Me.okButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RangeEditorDlg2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Редактор диапазона"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents canButton As System.Windows.Forms.Button
    Friend WithEvents okButton As System.Windows.Forms.Button
End Class
