<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAdjustment
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAdjustment))
        Me.GroupBoxInput = New System.Windows.Forms.GroupBox()
        Me.TextTypeKRD = New System.Windows.Forms.TextBox()
        Me.TextN2KRD15 = New System.Windows.Forms.TextBox()
        Me.TextN1KRD15 = New System.Windows.Forms.TextBox()
        Me.LabelN2KRD15 = New System.Windows.Forms.Label()
        Me.LabelN1KRD15 = New System.Windows.Forms.Label()
        Me.LabelTypeKRD = New System.Windows.Forms.Label()
        Me.TextTbox = New System.Windows.Forms.TextBox()
        Me.LabelT4KRD = New System.Windows.Forms.Label()
        Me.TextT4KRD = New System.Windows.Forms.TextBox()
        Me.LabelTbox = New System.Windows.Forms.Label()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.GroupBoxCalculate = New System.Windows.Forms.GroupBox()
        Me.TextN2KRD = New System.Windows.Forms.TextBox()
        Me.TextN1KRD = New System.Windows.Forms.TextBox()
        Me.LabelN2KRD = New System.Windows.Forms.Label()
        Me.LabelN1KRD = New System.Windows.Forms.Label()
        Me.ButtonCalculate = New System.Windows.Forms.Button()
        Me.GroupBoxInput.SuspendLayout()
        Me.GroupBoxCalculate.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxInput
        '
        Me.GroupBoxInput.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxInput.Controls.Add(Me.TextTypeKRD)
        Me.GroupBoxInput.Controls.Add(Me.TextN2KRD15)
        Me.GroupBoxInput.Controls.Add(Me.TextN1KRD15)
        Me.GroupBoxInput.Controls.Add(Me.LabelN2KRD15)
        Me.GroupBoxInput.Controls.Add(Me.LabelN1KRD15)
        Me.GroupBoxInput.Controls.Add(Me.LabelTypeKRD)
        Me.GroupBoxInput.Controls.Add(Me.TextTbox)
        Me.GroupBoxInput.Controls.Add(Me.LabelT4KRD)
        Me.GroupBoxInput.Controls.Add(Me.TextT4KRD)
        Me.GroupBoxInput.Controls.Add(Me.LabelTbox)
        Me.GroupBoxInput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxInput.ForeColor = System.Drawing.Color.Maroon
        Me.GroupBoxInput.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxInput.Name = "GroupBoxInput"
        Me.GroupBoxInput.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxInput.Size = New System.Drawing.Size(257, 152)
        Me.GroupBoxInput.TabIndex = 25
        Me.GroupBoxInput.TabStop = False
        Me.GroupBoxInput.Text = "Исходные данные"
        '
        'TextTypeKRD
        '
        Me.TextTypeKRD.AcceptsReturn = True
        Me.TextTypeKRD.BackColor = System.Drawing.SystemColors.Control
        Me.TextTypeKRD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextTypeKRD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextTypeKRD.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextTypeKRD.Location = New System.Drawing.Point(197, 24)
        Me.TextTypeKRD.MaxLength = 0
        Me.TextTypeKRD.Name = "TextTypeKRD"
        Me.TextTypeKRD.ReadOnly = True
        Me.TextTypeKRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextTypeKRD.Size = New System.Drawing.Size(48, 20)
        Me.TextTypeKRD.TabIndex = 22
        Me.TextTypeKRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextN2KRD15
        '
        Me.TextN2KRD15.AcceptsReturn = True
        Me.TextN2KRD15.BackColor = System.Drawing.SystemColors.Window
        Me.TextN2KRD15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextN2KRD15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextN2KRD15.Location = New System.Drawing.Point(197, 120)
        Me.TextN2KRD15.MaxLength = 0
        Me.TextN2KRD15.Name = "TextN2KRD15"
        Me.TextN2KRD15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextN2KRD15.Size = New System.Drawing.Size(48, 20)
        Me.TextN2KRD15.TabIndex = 5
        Me.TextN2KRD15.Text = "99,7"
        Me.TextN2KRD15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextN1KRD15
        '
        Me.TextN1KRD15.AcceptsReturn = True
        Me.TextN1KRD15.BackColor = System.Drawing.SystemColors.Window
        Me.TextN1KRD15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextN1KRD15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextN1KRD15.Location = New System.Drawing.Point(197, 96)
        Me.TextN1KRD15.MaxLength = 0
        Me.TextN1KRD15.Name = "TextN1KRD15"
        Me.TextN1KRD15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextN1KRD15.Size = New System.Drawing.Size(48, 20)
        Me.TextN1KRD15.TabIndex = 4
        Me.TextN1KRD15.Text = "98"
        Me.TextN1KRD15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelN2KRD15
        '
        Me.LabelN2KRD15.BackColor = System.Drawing.SystemColors.Control
        Me.LabelN2KRD15.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelN2KRD15.ForeColor = System.Drawing.Color.Maroon
        Me.LabelN2KRD15.Location = New System.Drawing.Point(8, 120)
        Me.LabelN2KRD15.Name = "LabelN2KRD15"
        Me.LabelN2KRD15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelN2KRD15.Size = New System.Drawing.Size(183, 17)
        Me.LabelN2KRD15.TabIndex = 13
        Me.LabelN2KRD15.Text = "Настройка N2 КРД при +15 град.:"
        Me.LabelN2KRD15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelN1KRD15
        '
        Me.LabelN1KRD15.BackColor = System.Drawing.SystemColors.Control
        Me.LabelN1KRD15.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelN1KRD15.ForeColor = System.Drawing.Color.Maroon
        Me.LabelN1KRD15.Location = New System.Drawing.Point(8, 96)
        Me.LabelN1KRD15.Name = "LabelN1KRD15"
        Me.LabelN1KRD15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelN1KRD15.Size = New System.Drawing.Size(183, 17)
        Me.LabelN1KRD15.TabIndex = 12
        Me.LabelN1KRD15.Text = "Настройка N1 КРД при +15 град.:"
        Me.LabelN1KRD15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelTypeKRD
        '
        Me.LabelTypeKRD.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTypeKRD.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTypeKRD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelTypeKRD.ForeColor = System.Drawing.Color.Maroon
        Me.LabelTypeKRD.Location = New System.Drawing.Point(8, 24)
        Me.LabelTypeKRD.Name = "LabelTypeKRD"
        Me.LabelTypeKRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTypeKRD.Size = New System.Drawing.Size(183, 17)
        Me.LabelTypeKRD.TabIndex = 19
        Me.LabelTypeKRD.Text = "Тип КРД:"
        Me.LabelTypeKRD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextTbox
        '
        Me.TextTbox.AcceptsReturn = True
        Me.TextTbox.BackColor = System.Drawing.SystemColors.Control
        Me.TextTbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextTbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextTbox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextTbox.Location = New System.Drawing.Point(197, 48)
        Me.TextTbox.MaxLength = 0
        Me.TextTbox.Name = "TextTbox"
        Me.TextTbox.ReadOnly = True
        Me.TextTbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextTbox.Size = New System.Drawing.Size(48, 20)
        Me.TextTbox.TabIndex = 16
        Me.TextTbox.Text = "20"
        Me.TextTbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelT4KRD
        '
        Me.LabelT4KRD.BackColor = System.Drawing.SystemColors.Control
        Me.LabelT4KRD.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelT4KRD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelT4KRD.ForeColor = System.Drawing.Color.Maroon
        Me.LabelT4KRD.Location = New System.Drawing.Point(8, 72)
        Me.LabelT4KRD.Name = "LabelT4KRD"
        Me.LabelT4KRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelT4KRD.Size = New System.Drawing.Size(183, 17)
        Me.LabelT4KRD.TabIndex = 21
        Me.LabelT4KRD.Text = "Т4 настроки КРД:"
        Me.LabelT4KRD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextT4KRD
        '
        Me.TextT4KRD.AcceptsReturn = True
        Me.TextT4KRD.BackColor = System.Drawing.SystemColors.Window
        Me.TextT4KRD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextT4KRD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextT4KRD.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextT4KRD.Location = New System.Drawing.Point(197, 72)
        Me.TextT4KRD.MaxLength = 0
        Me.TextT4KRD.Name = "TextT4KRD"
        Me.TextT4KRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextT4KRD.Size = New System.Drawing.Size(48, 20)
        Me.TextT4KRD.TabIndex = 17
        Me.TextT4KRD.Text = "600"
        Me.TextT4KRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelTbox
        '
        Me.LabelTbox.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTbox.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelTbox.ForeColor = System.Drawing.Color.Maroon
        Me.LabelTbox.Location = New System.Drawing.Point(8, 48)
        Me.LabelTbox.Name = "LabelTbox"
        Me.LabelTbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTbox.Size = New System.Drawing.Size(183, 17)
        Me.LabelTbox.TabIndex = 20
        Me.LabelTbox.Text = "Температура бокса в снимке:"
        Me.LabelTbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonApply
        '
        Me.ButtonApply.BackColor = System.Drawing.Color.Orange
        Me.ButtonApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonApply.Location = New System.Drawing.Point(20, 272)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonApply.Size = New System.Drawing.Size(237, 25)
        Me.ButtonApply.TabIndex = 0
        Me.ButtonApply.Text = "&Применить"
        Me.ButtonApply.UseVisualStyleBackColor = False
        '
        'GroupBoxCalculate
        '
        Me.GroupBoxCalculate.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxCalculate.Controls.Add(Me.TextN2KRD)
        Me.GroupBoxCalculate.Controls.Add(Me.TextN1KRD)
        Me.GroupBoxCalculate.Controls.Add(Me.LabelN2KRD)
        Me.GroupBoxCalculate.Controls.Add(Me.LabelN1KRD)
        Me.GroupBoxCalculate.Controls.Add(Me.ButtonCalculate)
        Me.GroupBoxCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxCalculate.ForeColor = System.Drawing.Color.Blue
        Me.GroupBoxCalculate.Location = New System.Drawing.Point(12, 170)
        Me.GroupBoxCalculate.Name = "GroupBoxCalculate"
        Me.GroupBoxCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxCalculate.Size = New System.Drawing.Size(257, 96)
        Me.GroupBoxCalculate.TabIndex = 26
        Me.GroupBoxCalculate.TabStop = False
        Me.GroupBoxCalculate.Text = "Результат расчета"
        '
        'TextN2KRD
        '
        Me.TextN2KRD.Location = New System.Drawing.Point(197, 37)
        Me.TextN2KRD.Name = "TextN2KRD"
        Me.TextN2KRD.ReadOnly = True
        Me.TextN2KRD.Size = New System.Drawing.Size(48, 20)
        Me.TextN2KRD.TabIndex = 20
        Me.TextN2KRD.Text = "0.0"
        Me.TextN2KRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextN1KRD
        '
        Me.TextN1KRD.Location = New System.Drawing.Point(197, 13)
        Me.TextN1KRD.Name = "TextN1KRD"
        Me.TextN1KRD.ReadOnly = True
        Me.TextN1KRD.Size = New System.Drawing.Size(48, 20)
        Me.TextN1KRD.TabIndex = 19
        Me.TextN1KRD.Text = "0.0"
        Me.TextN1KRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelN2KRD
        '
        Me.LabelN2KRD.BackColor = System.Drawing.SystemColors.Control
        Me.LabelN2KRD.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelN2KRD.ForeColor = System.Drawing.Color.Blue
        Me.LabelN2KRD.Location = New System.Drawing.Point(8, 40)
        Me.LabelN2KRD.Name = "LabelN2KRD"
        Me.LabelN2KRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelN2KRD.Size = New System.Drawing.Size(183, 17)
        Me.LabelN2KRD.TabIndex = 16
        Me.LabelN2KRD.Text = "N2 КРД физическая:"
        Me.LabelN2KRD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelN1KRD
        '
        Me.LabelN1KRD.BackColor = System.Drawing.SystemColors.Control
        Me.LabelN1KRD.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelN1KRD.ForeColor = System.Drawing.Color.Blue
        Me.LabelN1KRD.Location = New System.Drawing.Point(8, 16)
        Me.LabelN1KRD.Name = "LabelN1KRD"
        Me.LabelN1KRD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelN1KRD.Size = New System.Drawing.Size(183, 17)
        Me.LabelN1KRD.TabIndex = 15
        Me.LabelN1KRD.Text = "N1 КРД физическая:"
        Me.LabelN1KRD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonCalculate
        '
        Me.ButtonCalculate.BackColor = System.Drawing.Color.Silver
        Me.ButtonCalculate.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonCalculate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonCalculate.Location = New System.Drawing.Point(8, 64)
        Me.ButtonCalculate.Name = "ButtonCalculate"
        Me.ButtonCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonCalculate.Size = New System.Drawing.Size(237, 25)
        Me.ButtonCalculate.TabIndex = 0
        Me.ButtonCalculate.Text = "&Рассчитать"
        Me.ButtonCalculate.UseVisualStyleBackColor = False
        '
        'FormAdjustment
        '
        Me.AcceptButton = Me.ButtonApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(275, 302)
        Me.Controls.Add(Me.GroupBoxCalculate)
        Me.Controls.Add(Me.GroupBoxInput)
        Me.Controls.Add(Me.ButtonApply)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormAdjustment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Регулировка"
        Me.GroupBoxInput.ResumeLayout(False)
        Me.GroupBoxInput.PerformLayout()
        Me.GroupBoxCalculate.ResumeLayout(False)
        Me.GroupBoxCalculate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Public WithEvents GroupBoxInput As GroupBox
    Public WithEvents TextTypeKRD As TextBox
    Public WithEvents TextN2KRD15 As TextBox
    Public WithEvents TextN1KRD15 As TextBox
    Public WithEvents LabelN2KRD15 As Label
    Public WithEvents LabelN1KRD15 As Label
    Public WithEvents LabelTypeKRD As Label
    Public WithEvents TextTbox As TextBox
    Public WithEvents LabelT4KRD As Label
    Public WithEvents TextT4KRD As TextBox
    Public WithEvents LabelTbox As Label
    Public WithEvents ButtonApply As Button
    Public WithEvents GroupBoxCalculate As GroupBox
    Friend WithEvents TextN2KRD As TextBox
    Friend WithEvents TextN1KRD As TextBox
    Public WithEvents LabelN2KRD As Label
    Public WithEvents LabelN1KRD As Label
    Public WithEvents ButtonCalculate As Button
End Class
