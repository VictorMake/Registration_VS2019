<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDeleteCondition
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDeleteCondition))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(230, 75)
        Me.Panel2.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(184, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Укажите, какие условия удалять:"
        '
        'cmdОтменить
        '
        Me.ButtonCancel.BackColor = System.Drawing.Color.Silver
        Me.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonCancel.Location = New System.Drawing.Point(120, 8)
        Me.ButtonCancel.Name = "cmdОтменить"
        Me.ButtonCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonCancel.Size = New System.Drawing.Size(104, 25)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "&Отмена"
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ButtonApply)
        Me.Panel1.Controls.Add(Me.ButtonCancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 75)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(230, 40)
        Me.Panel1.TabIndex = 9
        '
        'cmdПрименить
        '
        Me.ButtonApply.BackColor = System.Drawing.Color.Silver
        Me.ButtonApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonApply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonApply.Location = New System.Drawing.Point(8, 8)
        Me.ButtonApply.Name = "cmdПрименить"
        Me.ButtonApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonApply.Size = New System.Drawing.Size(104, 25)
        Me.ButtonApply.TabIndex = 0
        Me.ButtonApply.Text = "&Удалить"
        Me.ButtonApply.UseVisualStyleBackColor = False
        '
        'FormDeleteCondition2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(230, 115)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormDeleteCondition2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Удаление условий"
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label1 As Label
    Public WithEvents ButtonCancel As Button
    Friend WithEvents Panel1 As Panel
    Public WithEvents ButtonApply As Button
End Class
