<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTestCompactRio
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
        Me.ButtonStartAcquisitionTimer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonStartAcquisitionTimer
        '
        Me.ButtonStartAcquisitionTimer.Location = New System.Drawing.Point(12, 12)
        Me.ButtonStartAcquisitionTimer.Name = "ButtonStartAcquisitionTimer"
        Me.ButtonStartAcquisitionTimer.Size = New System.Drawing.Size(165, 23)
        Me.ButtonStartAcquisitionTimer.TabIndex = 0
        Me.ButtonStartAcquisitionTimer.Text = "StartAcquisitionTimer"
        Me.ButtonStartAcquisitionTimer.UseVisualStyleBackColor = True
        '
        'FormTestCompactRio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 392)
        Me.Controls.Add(Me.ButtonStartAcquisitionTimer)
        Me.Name = "FormTestCompactRio"
        Me.Text = "FormTestCompactRio"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ButtonStartAcquisitionTimer As Button
End Class
