<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFind
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFind))
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.optDirection1 = New System.Windows.Forms.RadioButton()
        Me.optDirection0 = New System.Windows.Forms.RadioButton()
        Me.chkCase = New System.Windows.Forms.CheckBox()
        Me.txtFind = New System.Windows.Forms.TextBox()
        Me.cmdcancel = New System.Windows.Forms.Button()
        Me.cmdFind = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.optDirection1)
        Me.Frame1.Controls.Add(Me.optDirection0)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(92, 52)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(125, 41)
        Me.Frame1.TabIndex = 9
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Направление"
        '
        'optDirection1
        '
        Me.optDirection1.BackColor = System.Drawing.SystemColors.Control
        Me.optDirection1.Checked = True
        Me.optDirection1.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDirection1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDirection1.Location = New System.Drawing.Point(69, 16)
        Me.optDirection1.Name = "optDirection1"
        Me.optDirection1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDirection1.Size = New System.Drawing.Size(52, 17)
        Me.optDirection1.TabIndex = 1
        Me.optDirection1.TabStop = True
        Me.optDirection1.Text = "В&низ"
        Me.optDirection1.UseVisualStyleBackColor = False
        '
        'optDirection0
        '
        Me.optDirection0.BackColor = System.Drawing.SystemColors.Control
        Me.optDirection0.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDirection0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDirection0.Location = New System.Drawing.Point(15, 16)
        Me.optDirection0.Name = "optDirection0"
        Me.optDirection0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDirection0.Size = New System.Drawing.Size(57, 17)
        Me.optDirection0.TabIndex = 0
        Me.optDirection0.TabStop = True
        Me.optDirection0.Text = "&Вверх"
        Me.optDirection0.UseVisualStyleBackColor = False
        '
        'chkCase
        '
        Me.chkCase.BackColor = System.Drawing.SystemColors.Control
        Me.chkCase.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCase.Location = New System.Drawing.Point(4, 52)
        Me.chkCase.Name = "chkCase"
        Me.chkCase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCase.Size = New System.Drawing.Size(82, 33)
        Me.chkCase.TabIndex = 7
        Me.chkCase.Text = "Учет &Регистра"
        Me.chkCase.UseVisualStyleBackColor = False
        '
        'txtFind
        '
        Me.txtFind.AcceptsReturn = True
        Me.txtFind.BackColor = System.Drawing.SystemColors.Window
        Me.txtFind.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFind.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFind.Location = New System.Drawing.Point(70, 12)
        Me.txtFind.MaxLength = 0
        Me.txtFind.Name = "txtFind"
        Me.txtFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFind.Size = New System.Drawing.Size(147, 20)
        Me.txtFind.TabIndex = 8
        '
        'cmdcancel
        '
        Me.cmdcancel.BackColor = System.Drawing.Color.Silver
        Me.cmdcancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdcancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdcancel.Location = New System.Drawing.Point(224, 44)
        Me.cmdcancel.Name = "cmdcancel"
        Me.cmdcancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdcancel.Size = New System.Drawing.Size(66, 25)
        Me.cmdcancel.TabIndex = 6
        Me.cmdcancel.Text = "Отмена"
        Me.cmdcancel.UseVisualStyleBackColor = False
        '
        'cmdFind
        '
        Me.cmdFind.BackColor = System.Drawing.Color.Silver
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Location = New System.Drawing.Point(224, 12)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(66, 25)
        Me.cmdFind.TabIndex = 4
        Me.cmdFind.Text = "&Поиск"
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(4, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(60, 17)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "&Поиск как:"
        '
        'frmFind2
        '
        Me.AcceptButton = Me.cmdFind
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdcancel
        Me.ClientSize = New System.Drawing.Size(298, 98)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.chkCase)
        Me.Controls.Add(Me.txtFind)
        Me.Controls.Add(Me.cmdcancel)
        Me.Controls.Add(Me.cmdFind)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 125)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFind2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Поиск"
        Me.TopMost = True
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents optDirection1 As System.Windows.Forms.RadioButton
    Public WithEvents optDirection0 As System.Windows.Forms.RadioButton
    Public WithEvents chkCase As System.Windows.Forms.CheckBox
    Public WithEvents txtFind As System.Windows.Forms.TextBox
    Public WithEvents cmdcancel As System.Windows.Forms.Button
    Public WithEvents cmdFind As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
End Class
