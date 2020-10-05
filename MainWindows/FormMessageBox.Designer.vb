<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMessageBox
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
        Me.lblСообщение = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblСообщение
        '
        Me.lblСообщение.BackColor = System.Drawing.SystemColors.HighlightText
        Me.lblСообщение.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblСообщение.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblСообщение.Font = New System.Drawing.Font("Times New Roman", 19.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblСообщение.ForeColor = System.Drawing.Color.Red
        Me.lblСообщение.Location = New System.Drawing.Point(0, 0)
        Me.lblСообщение.Name = "lblСообщение"
        Me.lblСообщение.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblСообщение.Size = New System.Drawing.Size(425, 37)
        Me.lblСообщение.TabIndex = 1
        Me.lblСообщение.Text = "Подождите, идет печать"
        Me.lblСообщение.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(425, 37)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblСообщение)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormMessageBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Печать"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblСообщение As System.Windows.Forms.Label
End Class
