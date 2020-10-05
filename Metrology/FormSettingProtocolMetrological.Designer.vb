<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSettingProtocolMetrological
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSettingProtocolMetrological))
        Me.ListViewMetrological = New System.Windows.Forms.ListView()
        Me.TextBoxADCmin = New System.Windows.Forms.TextBox()
        Me.LabelMin = New System.Windows.Forms.Label()
        Me.TextBoxADCmax = New System.Windows.Forms.TextBox()
        Me.LabelMax = New System.Windows.Forms.Label()
        Me.GroupBoxADC = New System.Windows.Forms.GroupBox()
        Me.GroupBoxPhysical = New System.Windows.Forms.GroupBox()
        Me.TextBoxPhysicalMin = New System.Windows.Forms.TextBox()
        Me.LabelPhysicalMin = New System.Windows.Forms.Label()
        Me.TextBoxPhysicalMax = New System.Windows.Forms.TextBox()
        Me.LabelPhysicalMax = New System.Windows.Forms.Label()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.ButtonCalculate = New System.Windows.Forms.Button()
        Me.GroupBoxADC.SuspendLayout()
        Me.GroupBoxPhysical.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListViewMetrological
        '
        Me.ListViewMetrological.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ListViewMetrological.GridLines = True
        Me.ListViewMetrological.HideSelection = False
        Me.ListViewMetrological.Location = New System.Drawing.Point(223, 17)
        Me.ListViewMetrological.MultiSelect = False
        Me.ListViewMetrological.Name = "ListViewMetrological"
        Me.ListViewMetrological.Size = New System.Drawing.Size(216, 208)
        Me.ListViewMetrological.TabIndex = 14
        Me.ListViewMetrological.UseCompatibleStateImageBehavior = False
        Me.ListViewMetrological.View = System.Windows.Forms.View.Details
        '
        'TextBoxADCmin
        '
        Me.TextBoxADCmin.AcceptsReturn = True
        Me.TextBoxADCmin.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxADCmin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxADCmin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxADCmin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxADCmin.Location = New System.Drawing.Point(48, 32)
        Me.TextBoxADCmin.MaxLength = 0
        Me.TextBoxADCmin.Name = "TextBoxADCmin"
        Me.TextBoxADCmin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxADCmin.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxADCmin.TabIndex = 0
        Me.TextBoxADCmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelMin
        '
        Me.LabelMin.BackColor = System.Drawing.SystemColors.Control
        Me.LabelMin.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelMin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMin.Location = New System.Drawing.Point(8, 32)
        Me.LabelMin.Name = "LabelMin"
        Me.LabelMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelMin.Size = New System.Drawing.Size(32, 16)
        Me.LabelMin.TabIndex = 19
        Me.LabelMin.Text = "min"
        Me.LabelMin.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBoxADCmax
        '
        Me.TextBoxADCmax.AcceptsReturn = True
        Me.TextBoxADCmax.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxADCmax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxADCmax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxADCmax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxADCmax.Location = New System.Drawing.Point(144, 32)
        Me.TextBoxADCmax.MaxLength = 0
        Me.TextBoxADCmax.Name = "TextBoxADCmax"
        Me.TextBoxADCmax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxADCmax.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxADCmax.TabIndex = 1
        Me.TextBoxADCmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelMax
        '
        Me.LabelMax.BackColor = System.Drawing.SystemColors.Control
        Me.LabelMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelMax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMax.Location = New System.Drawing.Point(104, 32)
        Me.LabelMax.Name = "LabelMax"
        Me.LabelMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelMax.Size = New System.Drawing.Size(32, 16)
        Me.LabelMax.TabIndex = 20
        Me.LabelMax.Text = "max"
        Me.LabelMax.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBoxADC
        '
        Me.GroupBoxADC.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxADC.Controls.Add(Me.TextBoxADCmin)
        Me.GroupBoxADC.Controls.Add(Me.LabelMin)
        Me.GroupBoxADC.Controls.Add(Me.TextBoxADCmax)
        Me.GroupBoxADC.Controls.Add(Me.LabelMax)
        Me.GroupBoxADC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxADC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxADC.Location = New System.Drawing.Point(7, 97)
        Me.GroupBoxADC.Name = "GroupBoxADC"
        Me.GroupBoxADC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxADC.Size = New System.Drawing.Size(208, 80)
        Me.GroupBoxADC.TabIndex = 11
        Me.GroupBoxADC.TabStop = False
        Me.GroupBoxADC.Text = "Значения АЦП"
        '
        'GroupBoxPhysical
        '
        Me.GroupBoxPhysical.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxPhysical.Controls.Add(Me.TextBoxPhysicalMin)
        Me.GroupBoxPhysical.Controls.Add(Me.LabelPhysicalMin)
        Me.GroupBoxPhysical.Controls.Add(Me.TextBoxPhysicalMax)
        Me.GroupBoxPhysical.Controls.Add(Me.LabelPhysicalMax)
        Me.GroupBoxPhysical.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxPhysical.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxPhysical.Location = New System.Drawing.Point(7, 9)
        Me.GroupBoxPhysical.Name = "GroupBoxPhysical"
        Me.GroupBoxPhysical.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxPhysical.Size = New System.Drawing.Size(208, 80)
        Me.GroupBoxPhysical.TabIndex = 10
        Me.GroupBoxPhysical.TabStop = False
        Me.GroupBoxPhysical.Text = "Физика"
        '
        'TextBoxPhysicalMin
        '
        Me.TextBoxPhysicalMin.AcceptsReturn = True
        Me.TextBoxPhysicalMin.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxPhysicalMin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxPhysicalMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxPhysicalMin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxPhysicalMin.Location = New System.Drawing.Point(48, 32)
        Me.TextBoxPhysicalMin.MaxLength = 0
        Me.TextBoxPhysicalMin.Name = "TextBoxPhysicalMin"
        Me.TextBoxPhysicalMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxPhysicalMin.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxPhysicalMin.TabIndex = 0
        Me.TextBoxPhysicalMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelPhysicalMin
        '
        Me.LabelPhysicalMin.BackColor = System.Drawing.SystemColors.Control
        Me.LabelPhysicalMin.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPhysicalMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelPhysicalMin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPhysicalMin.Location = New System.Drawing.Point(8, 32)
        Me.LabelPhysicalMin.Name = "LabelPhysicalMin"
        Me.LabelPhysicalMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPhysicalMin.Size = New System.Drawing.Size(32, 16)
        Me.LabelPhysicalMin.TabIndex = 19
        Me.LabelPhysicalMin.Text = "min"
        Me.LabelPhysicalMin.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBoxPhysicalMax
        '
        Me.TextBoxPhysicalMax.AcceptsReturn = True
        Me.TextBoxPhysicalMax.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxPhysicalMax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxPhysicalMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxPhysicalMax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxPhysicalMax.Location = New System.Drawing.Point(144, 32)
        Me.TextBoxPhysicalMax.MaxLength = 0
        Me.TextBoxPhysicalMax.Name = "TextBoxPhysicalMax"
        Me.TextBoxPhysicalMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxPhysicalMax.Size = New System.Drawing.Size(48, 20)
        Me.TextBoxPhysicalMax.TabIndex = 1
        Me.TextBoxPhysicalMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelPhysicalMax
        '
        Me.LabelPhysicalMax.BackColor = System.Drawing.SystemColors.Control
        Me.LabelPhysicalMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPhysicalMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelPhysicalMax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPhysicalMax.Location = New System.Drawing.Point(104, 32)
        Me.LabelPhysicalMax.Name = "LabelPhysicalMax"
        Me.LabelPhysicalMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPhysicalMax.Size = New System.Drawing.Size(32, 16)
        Me.LabelPhysicalMax.TabIndex = 20
        Me.LabelPhysicalMax.Text = "max"
        Me.LabelPhysicalMax.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ButtonApply
        '
        Me.ButtonApply.BackColor = System.Drawing.Color.Silver
        Me.ButtonApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonApply.Location = New System.Drawing.Point(119, 201)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonApply.Size = New System.Drawing.Size(96, 25)
        Me.ButtonApply.TabIndex = 13
        Me.ButtonApply.Text = "&Применить"
        Me.ButtonApply.UseVisualStyleBackColor = False
        '
        'ButtonCalculate
        '
        Me.ButtonCalculate.BackColor = System.Drawing.Color.Silver
        Me.ButtonCalculate.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonCalculate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonCalculate.Location = New System.Drawing.Point(7, 201)
        Me.ButtonCalculate.Name = "ButtonCalculate"
        Me.ButtonCalculate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonCalculate.Size = New System.Drawing.Size(96, 25)
        Me.ButtonCalculate.TabIndex = 12
        Me.ButtonCalculate.Text = "&Рассчитать"
        Me.ButtonCalculate.UseVisualStyleBackColor = False
        '
        'FormSettingProtocolMetrological
        '
        Me.AcceptButton = Me.ButtonApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(446, 235)
        Me.Controls.Add(Me.ListViewMetrological)
        Me.Controls.Add(Me.GroupBoxADC)
        Me.Controls.Add(Me.GroupBoxPhysical)
        Me.Controls.Add(Me.ButtonApply)
        Me.Controls.Add(Me.ButtonCalculate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormSettingProtocolMetrological"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настройка протокола метрологии"
        Me.GroupBoxADC.ResumeLayout(False)
        Me.GroupBoxADC.PerformLayout()
        Me.GroupBoxPhysical.ResumeLayout(False)
        Me.GroupBoxPhysical.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListViewMetrological As ListView
    Public WithEvents TextBoxADCmin As TextBox
    Public WithEvents LabelMin As Label
    Public WithEvents TextBoxADCmax As TextBox
    Public WithEvents LabelMax As Label
    Public WithEvents GroupBoxADC As GroupBox
    Public WithEvents GroupBoxPhysical As GroupBox
    Public WithEvents TextBoxPhysicalMin As TextBox
    Public WithEvents LabelPhysicalMin As Label
    Public WithEvents TextBoxPhysicalMax As TextBox
    Public WithEvents LabelPhysicalMax As Label
    Public WithEvents ButtonApply As Button
    Public WithEvents ButtonCalculate As Button
End Class
