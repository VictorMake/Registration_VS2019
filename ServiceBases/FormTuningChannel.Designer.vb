<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTuningChannel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningChannel))
        Me.RadioButtonAC = New System.Windows.Forms.RadioButton()
        Me.ComboBoxCountOfChannels = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadioButtonGround = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButtonDC = New System.Windows.Forms.RadioButton()
        Me.RadioButtonRSE = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ComboBoxUpperLimit = New System.Windows.Forms.ComboBox()
        Me.ComboBoxLowLimit = New System.Windows.Forms.ComboBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.RadioButtonNRSE = New System.Windows.Forms.RadioButton()
        Me.RadioButtonDIFF = New System.Windows.Forms.RadioButton()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.RadioButton4WRITE = New System.Windows.Forms.RadioButton()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ListBoxFormula = New System.Windows.Forms.ListBox()
        Me.LabelMeasurement = New System.Windows.Forms.Label()
        Me.PictureBoxControl = New System.Windows.Forms.PictureBox()
        Me.GroupBoxFormula = New System.Windows.Forms.GroupBox()
        Me.ListBoxUnit = New System.Windows.Forms.ListBox()
        Me.LabelLevelMin = New System.Windows.Forms.Label()
        Me.LabelLevelMax = New System.Windows.Forms.Label()
        Me.NumericLevelMax = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.NumericLevelMin = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.GroupBoxParametr = New System.Windows.Forms.GroupBox()
        Me.Panel1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PictureBoxControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxFormula.SuspendLayout()
        CType(Me.NumericLevelMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericLevelMin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.GroupBoxParametr.SuspendLayout()
        Me.SuspendLayout()
        '
        'RadioButtonAC
        '
        Me.RadioButtonAC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonAC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonAC.Location = New System.Drawing.Point(8, 32)
        Me.RadioButtonAC.Name = "RadioButtonAC"
        Me.RadioButtonAC.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonAC.TabIndex = 1
        Me.RadioButtonAC.Text = "Переменный"
        '
        'ComboBoxCountOfChannels
        '
        Me.ComboBoxCountOfChannels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxCountOfChannels.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxCountOfChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCountOfChannels.DropDownWidth = 60
        Me.ComboBoxCountOfChannels.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxCountOfChannels.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxCountOfChannels.Items.AddRange(New Object() {"1", "2", "4", "8", "16", "32"})
        Me.ComboBoxCountOfChannels.Location = New System.Drawing.Point(104, 256)
        Me.ComboBoxCountOfChannels.MaxDropDownItems = 32
        Me.ComboBoxCountOfChannels.Name = "ComboBoxCountOfChannels"
        Me.ComboBoxCountOfChannels.Size = New System.Drawing.Size(56, 21)
        Me.ComboBoxCountOfChannels.TabIndex = 106
        Me.ComboBoxCountOfChannels.Visible = False
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(8, 240)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(152, 3)
        Me.Label2.TabIndex = 108
        Me.Label2.Visible = False
        '
        'RadioButtonGround
        '
        Me.RadioButtonGround.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonGround.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonGround.Location = New System.Drawing.Point(8, 48)
        Me.RadioButtonGround.Name = "RadioButtonGround"
        Me.RadioButtonGround.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonGround.TabIndex = 2
        Me.RadioButtonGround.Text = "Заземленный"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(8, 256)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 48)
        Me.Label1.TabIndex = 107
        Me.Label1.Text = "Количество каналов для платы"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Visible = False
        '
        'RadioButtonDC
        '
        Me.RadioButtonDC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonDC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonDC.Checked = True
        Me.RadioButtonDC.Location = New System.Drawing.Point(8, 16)
        Me.RadioButtonDC.Name = "RadioButtonDC"
        Me.RadioButtonDC.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonDC.TabIndex = 0
        Me.RadioButtonDC.TabStop = True
        Me.RadioButtonDC.Text = "Постоянный"
        '
        'RadioButtonRSE
        '
        Me.RadioButtonRSE.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonRSE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonRSE.Location = New System.Drawing.Point(8, 32)
        Me.RadioButtonRSE.Name = "RadioButtonRSE"
        Me.RadioButtonRSE.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonRSE.TabIndex = 1
        Me.RadioButtonRSE.Text = "Несим.незаземлен."
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.ButtonOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 460)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(362, 40)
        Me.Panel1.TabIndex = 113
        '
        'btnOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.BackColor = System.Drawing.Color.Silver
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Image = CType(resources.GetObject("btnOK.Image"), System.Drawing.Image)
        Me.ButtonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonOK.Location = New System.Drawing.Point(184, 4)
        Me.ButtonOK.Name = "btnOK"
        Me.ButtonOK.Size = New System.Drawing.Size(168, 29)
        Me.ButtonOK.TabIndex = 44
        Me.ButtonOK.Text = "&OK"
        Me.ButtonOK.UseVisualStyleBackColor = False
        '
        'ComboBoxUpperLimit
        '
        Me.ComboBoxUpperLimit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxUpperLimit.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxUpperLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxUpperLimit.DropDownWidth = 60
        Me.ComboBoxUpperLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxUpperLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxUpperLimit.Items.AddRange(New Object() {"-10", "-5", "-2.5", "-2", "-1", "-0.5", "-0.25", "-0.2", "-0.1", "-0.05", "0", "0.05", "0.1", "0.2", "0.25", "0.5", "1", "2", "2.5", "5", "10"})
        Me.ComboBoxUpperLimit.Location = New System.Drawing.Point(104, 208)
        Me.ComboBoxUpperLimit.MaxDropDownItems = 32
        Me.ComboBoxUpperLimit.Name = "ComboBoxUpperLimit"
        Me.ComboBoxUpperLimit.Size = New System.Drawing.Size(56, 21)
        Me.ComboBoxUpperLimit.TabIndex = 3
        '
        'ComboBoxLowLimit
        '
        Me.ComboBoxLowLimit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxLowLimit.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxLowLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxLowLimit.DropDownWidth = 60
        Me.ComboBoxLowLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxLowLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxLowLimit.Items.AddRange(New Object() {"-10", "-5", "-2.5", "-2", "-1", "-0.5", "-0.25", "-0.2", "-0.1", "-0.05", "0", "0.05", "0.1", "0.2", "0.25", "0.5", "1", "2", "2.5", "5", "10"})
        Me.ComboBoxLowLimit.Location = New System.Drawing.Point(104, 184)
        Me.ComboBoxLowLimit.MaxDropDownItems = 32
        Me.ComboBoxLowLimit.Name = "ComboBoxLowLimit"
        Me.ComboBoxLowLimit.Size = New System.Drawing.Size(56, 21)
        Me.ComboBoxLowLimit.TabIndex = 2
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.RadioButtonAC)
        Me.GroupBox6.Controls.Add(Me.RadioButtonGround)
        Me.GroupBox6.Controls.Add(Me.RadioButtonDC)
        Me.GroupBox6.Location = New System.Drawing.Point(8, 104)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(150, 72)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Тип сигнала"
        '
        'RadioButtonNRSE
        '
        Me.RadioButtonNRSE.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonNRSE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonNRSE.Location = New System.Drawing.Point(8, 48)
        Me.RadioButtonNRSE.Name = "RadioButtonNRSE"
        Me.RadioButtonNRSE.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonNRSE.TabIndex = 2
        Me.RadioButtonNRSE.Text = "Несим.заземленный"
        '
        'RadioButtonDIFF
        '
        Me.RadioButtonDIFF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonDIFF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButtonDIFF.Checked = True
        Me.RadioButtonDIFF.Location = New System.Drawing.Point(8, 16)
        Me.RadioButtonDIFF.Name = "RadioButtonDIFF"
        Me.RadioButtonDIFF.Size = New System.Drawing.Size(134, 16)
        Me.RadioButtonDIFF.TabIndex = 0
        Me.RadioButtonDIFF.TabStop = True
        Me.RadioButtonDIFF.Text = "Дифференциальный"
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.RadioButtonRSE)
        Me.GroupBox5.Controls.Add(Me.RadioButtonNRSE)
        Me.GroupBox5.Controls.Add(Me.RadioButton4WRITE)
        Me.GroupBox5.Controls.Add(Me.RadioButtonDIFF)
        Me.GroupBox5.Location = New System.Drawing.Point(8, 16)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(150, 88)
        Me.GroupBox5.TabIndex = 20
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Тип подключения"
        '
        'RadioButton4WRITE
        '
        Me.RadioButton4WRITE.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton4WRITE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton4WRITE.Location = New System.Drawing.Point(8, 64)
        Me.RadioButton4WRITE.Name = "RadioButton4WRITE"
        Me.RadioButton4WRITE.Size = New System.Drawing.Size(134, 16)
        Me.RadioButton4WRITE.TabIndex = 3
        Me.RadioButton4WRITE.Text = "Псевдодифференциальный"
        '
        'Label17
        '
        Me.Label17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.Location = New System.Drawing.Point(8, 208)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(96, 16)
        Me.Label17.TabIndex = 105
        Me.Label17.Text = "Верхний предел"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.Location = New System.Drawing.Point(8, 184)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(96, 16)
        Me.Label15.TabIndex = 103
        Me.Label15.Text = "Нижний предел"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ListBoxFormula
        '
        Me.ListBoxFormula.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxFormula.Items.AddRange(New Object() {"Частота - 1", "Полином - 2", "Дискретный - 3"})
        Me.ListBoxFormula.Location = New System.Drawing.Point(8, 16)
        Me.ListBoxFormula.Name = "ListBoxFormula"
        Me.ListBoxFormula.Size = New System.Drawing.Size(152, 43)
        Me.ListBoxFormula.TabIndex = 0
        '
        'LabelMeasurement
        '
        Me.LabelMeasurement.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelMeasurement.Location = New System.Drawing.Point(8, 72)
        Me.LabelMeasurement.Name = "LabelMeasurement"
        Me.LabelMeasurement.Size = New System.Drawing.Size(152, 16)
        Me.LabelMeasurement.TabIndex = 14
        Me.LabelMeasurement.Text = "Ед. измерения"
        Me.LabelMeasurement.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PictureBoxControl
        '
        Me.PictureBoxControl.Location = New System.Drawing.Point(56, 272)
        Me.PictureBoxControl.Name = "PictureBoxControl"
        Me.PictureBoxControl.Size = New System.Drawing.Size(64, 64)
        Me.PictureBoxControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxControl.TabIndex = 105
        Me.PictureBoxControl.TabStop = False
        '
        'GroupBoxFormula
        '
        Me.GroupBoxFormula.BackColor = System.Drawing.Color.Silver
        Me.GroupBoxFormula.Controls.Add(Me.ListBoxFormula)
        Me.GroupBoxFormula.Controls.Add(Me.LabelMeasurement)
        Me.GroupBoxFormula.Controls.Add(Me.PictureBoxControl)
        Me.GroupBoxFormula.Controls.Add(Me.ListBoxUnit)
        Me.GroupBoxFormula.Controls.Add(Me.LabelLevelMin)
        Me.GroupBoxFormula.Controls.Add(Me.LabelLevelMax)
        Me.GroupBoxFormula.Controls.Add(Me.NumericLevelMax)
        Me.GroupBoxFormula.Controls.Add(Me.NumericLevelMin)
        Me.GroupBoxFormula.Location = New System.Drawing.Point(184, 8)
        Me.GroupBoxFormula.Name = "GroupBoxFormula"
        Me.GroupBoxFormula.Size = New System.Drawing.Size(168, 440)
        Me.GroupBoxFormula.TabIndex = 0
        Me.GroupBoxFormula.TabStop = False
        Me.GroupBoxFormula.Text = "Параметр"
        '
        'ListBoxЕдИзмерения
        '
        Me.ListBoxUnit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ListBoxUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ListBoxUnit.IntegralHeight = False
        Me.ListBoxUnit.ItemHeight = 16
        Me.ListBoxUnit.Location = New System.Drawing.Point(8, 88)
        Me.ListBoxUnit.Name = "ListBoxЕдИзмерения"
        Me.ListBoxUnit.Size = New System.Drawing.Size(152, 168)
        Me.ListBoxUnit.TabIndex = 0
        '
        'LabelLevelMin
        '
        Me.LabelLevelMin.Location = New System.Drawing.Point(8, 344)
        Me.LabelLevelMin.Name = "LabelLevelMin"
        Me.LabelLevelMin.Size = New System.Drawing.Size(104, 40)
        Me.LabelLevelMin.TabIndex = 50
        Me.LabelLevelMin.Text = "Установить минимальное значение сигнала"
        Me.LabelLevelMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelLevelMax
        '
        Me.LabelLevelMax.Location = New System.Drawing.Point(8, 392)
        Me.LabelLevelMax.Name = "LabelLevelMax"
        Me.LabelLevelMax.Size = New System.Drawing.Size(104, 40)
        Me.LabelLevelMax.TabIndex = 52
        Me.LabelLevelMax.Text = "Установить максимальное значение сигнала"
        Me.LabelLevelMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'NumericLevelMax
        '
        Me.NumericLevelMax.Location = New System.Drawing.Point(112, 400)
        Me.NumericLevelMax.Name = "NumericLevelMax"
        Me.NumericLevelMax.Size = New System.Drawing.Size(48, 20)
        Me.NumericLevelMax.TabIndex = 1
        Me.NumericLevelMax.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.NumericLevelMax.Value = 100.0R
        '
        'NumericLevelMin
        '
        Me.NumericLevelMin.Location = New System.Drawing.Point(112, 352)
        Me.NumericLevelMin.Name = "NumericLevelMin"
        Me.NumericLevelMin.Size = New System.Drawing.Size(48, 20)
        Me.NumericLevelMin.TabIndex = 0
        Me.NumericLevelMin.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel5.Controls.Add(Me.GroupBoxParametr)
        Me.Panel5.Controls.Add(Me.GroupBoxFormula)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(362, 500)
        Me.Panel5.TabIndex = 114
        '
        'GroupBoxParametr
        '
        Me.GroupBoxParametr.BackColor = System.Drawing.Color.Silver
        Me.GroupBoxParametr.Controls.Add(Me.Label2)
        Me.GroupBoxParametr.Controls.Add(Me.ComboBoxCountOfChannels)
        Me.GroupBoxParametr.Controls.Add(Me.Label1)
        Me.GroupBoxParametr.Controls.Add(Me.ComboBoxUpperLimit)
        Me.GroupBoxParametr.Controls.Add(Me.ComboBoxLowLimit)
        Me.GroupBoxParametr.Controls.Add(Me.GroupBox6)
        Me.GroupBoxParametr.Controls.Add(Me.GroupBox5)
        Me.GroupBoxParametr.Controls.Add(Me.Label17)
        Me.GroupBoxParametr.Controls.Add(Me.Label15)
        Me.GroupBoxParametr.Location = New System.Drawing.Point(8, 8)
        Me.GroupBoxParametr.Name = "GroupBoxParametr"
        Me.GroupBoxParametr.Size = New System.Drawing.Size(168, 440)
        Me.GroupBoxParametr.TabIndex = 104
        Me.GroupBoxParametr.TabStop = False
        Me.GroupBoxParametr.Text = "Физика"
        '
        'frmНастройкаКанала2
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 500)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmНастройкаКанала2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настроить тип добавляемых каналов"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.PictureBoxControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxFormula.ResumeLayout(False)
        CType(Me.NumericLevelMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericLevelMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.GroupBoxParametr.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadioButtonAC As System.Windows.Forms.RadioButton
    Friend WithEvents ComboBoxCountOfChannels As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RadioButtonGround As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RadioButtonDC As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonRSE As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ComboBoxUpperLimit As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxLowLimit As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonNRSE As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonDIFF As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton4WRITE As System.Windows.Forms.RadioButton
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ListBoxFormula As System.Windows.Forms.ListBox
    Friend WithEvents LabelMeasurement As System.Windows.Forms.Label
    Friend WithEvents PictureBoxControl As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBoxFormula As System.Windows.Forms.GroupBox
    Friend WithEvents ListBoxUnit As System.Windows.Forms.ListBox
    Friend WithEvents LabelLevelMin As System.Windows.Forms.Label
    Friend WithEvents LabelLevelMax As System.Windows.Forms.Label
    Friend WithEvents NumericLevelMax As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents NumericLevelMin As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents GroupBoxParametr As System.Windows.Forms.GroupBox
End Class
