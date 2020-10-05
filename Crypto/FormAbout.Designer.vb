<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAbout
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAbout))
        Me.LabelY = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LabelX = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdSysInfo = New System.Windows.Forms.Button()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.Line2 = New System.Windows.Forms.Label()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.lblDisclaimer = New System.Windows.Forms.Label()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelY
        '
        Me.LabelY.AutoSize = True
        Me.LabelY.Location = New System.Drawing.Point(193, 32)
        Me.LabelY.Name = "LabelY"
        Me.LabelY.Size = New System.Drawing.Size(40, 13)
        Me.LabelY.TabIndex = 23
        Me.LabelY.Text = "LabelY"
        Me.LabelY.Visible = False
        '
        'LabelX
        '
        Me.LabelX.AutoSize = True
        Me.LabelX.Location = New System.Drawing.Point(193, 10)
        Me.LabelX.Name = "LabelX"
        Me.LabelX.Size = New System.Drawing.Size(40, 13)
        Me.LabelX.TabIndex = 22
        Me.LabelX.Text = "LabelX"
        Me.LabelX.Visible = False
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.Location = New System.Drawing.Point(79, 65)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVersion.Size = New System.Drawing.Size(111, 24)
        Me.lblVersion.TabIndex = 21
        Me.lblVersion.Tag = "Version"
        Me.lblVersion.Text = ".net Версия"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Info
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Courier New", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTitle.Location = New System.Drawing.Point(73, 144)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(262, 43)
        Me.lblTitle.TabIndex = 20
        Me.lblTitle.Tag = "Product"
        Me.lblTitle.Text = "Регистратор"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'picIcon
        '
        Me.picIcon.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.picIcon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.picIcon.Image = CType(resources.GetObject("picIcon.Image"), System.Drawing.Image)
        Me.picIcon.Location = New System.Drawing.Point(-1, 1)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.picIcon.Size = New System.Drawing.Size(63, 335)
        Me.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picIcon.TabIndex = 15
        Me.picIcon.TabStop = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(287, 257)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(98, 23)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Tag = "OK"
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdSysInfo
        '
        Me.cmdSysInfo.BackColor = System.Drawing.Color.Silver
        Me.cmdSysInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSysInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSysInfo.Location = New System.Drawing.Point(287, 289)
        Me.cmdSysInfo.Name = "cmdSysInfo"
        Me.cmdSysInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSysInfo.Size = New System.Drawing.Size(97, 23)
        Me.cmdSysInfo.TabIndex = 14
        Me.cmdSysInfo.Tag = "&System Info..."
        Me.cmdSysInfo.Text = "&Компьютер..."
        Me.cmdSysInfo.UseVisualStyleBackColor = False
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.ForeColor = System.Drawing.Color.Black
        Me.lblDescription.Location = New System.Drawing.Point(79, 209)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(256, 16)
        Me.lblDescription.TabIndex = 17
        Me.lblDescription.Tag = "App Description"
        Me.lblDescription.Text = "Авторским правом обладает Макеенко В.В."
        '
        'Line2
        '
        Me.Line2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Line2.Location = New System.Drawing.Point(67, 238)
        Me.Line2.Name = "Line2"
        Me.Line2.Size = New System.Drawing.Size(324, 1)
        Me.Line2.TabIndex = 18
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.Color.White
        Me.Line1.Location = New System.Drawing.Point(68, 239)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(323, 1)
        Me.Line1.TabIndex = 19
        '
        'lblDisclaimer
        '
        Me.lblDisclaimer.BackColor = System.Drawing.SystemColors.Control
        Me.lblDisclaimer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisclaimer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblDisclaimer.ForeColor = System.Drawing.Color.Black
        Me.lblDisclaimer.Location = New System.Drawing.Point(63, 257)
        Me.lblDisclaimer.Name = "lblDisclaimer"
        Me.lblDisclaimer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDisclaimer.Size = New System.Drawing.Size(216, 55)
        Me.lblDisclaimer.TabIndex = 16
        Me.lblDisclaimer.Tag = "Warning: ..."
        Me.lblDisclaimer.Text = "Внимание: данный продукт подлежит лицензированию"
        Me.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAbout
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdOK
        Me.ClientSize = New System.Drawing.Size(392, 336)
        Me.Controls.Add(Me.LabelY)
        Me.Controls.Add(Me.LabelX)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.picIcon)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdSysInfo)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.Line2)
        Me.Controls.Add(Me.Line1)
        Me.Controls.Add(Me.lblDisclaimer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "About Project1"
        Me.Text = "Универсальная программа регистрации измерений"
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelY As System.Windows.Forms.Label
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents LabelX As System.Windows.Forms.Label
    Public WithEvents lblVersion As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Public WithEvents picIcon As System.Windows.Forms.PictureBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdSysInfo As System.Windows.Forms.Button
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents Line2 As System.Windows.Forms.Label
    Public WithEvents Line1 As System.Windows.Forms.Label
    Public WithEvents lblDisclaimer As System.Windows.Forms.Label

End Class
