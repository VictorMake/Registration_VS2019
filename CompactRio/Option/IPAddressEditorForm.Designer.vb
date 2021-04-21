<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPAddressEditorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.IPmaskedTextBox = New System.Windows.Forms.MaskedTextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'IPmaskedTextBox
        '
        Me.IPmaskedTextBox.BeepOnError = True
        Me.IPmaskedTextBox.Font = New System.Drawing.Font("Tahoma", 12.22642!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.IPmaskedTextBox.Location = New System.Drawing.Point(12, 12)
        Me.IPmaskedTextBox.Mask = "099\.099\.099\.099"
        Me.IPmaskedTextBox.Name = "IPmaskedTextBox"
        Me.IPmaskedTextBox.Size = New System.Drawing.Size(163, 27)
        Me.IPmaskedTextBox.TabIndex = 3
        Me.IPmaskedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(100, 47)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'IPAddressEditorForm
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(183, 81)
        Me.Controls.Add(Me.IPmaskedTextBox)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IPAddressEditorForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IP адрес"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents IPmaskedTextBox As System.Windows.Forms.MaskedTextBox
    Private WithEvents btnOK As System.Windows.Forms.Button
End Class
