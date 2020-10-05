<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFormulaEditor
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFormulaEditor))
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ImageListChannels = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStripContainerForm = New System.Windows.Forms.ToolStripContainer()
        Me.TableLayoutPanelFormula = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridViewARG = New System.Windows.Forms.DataGridView()
        Me.LabelSelect = New System.Windows.Forms.Label()
        Me.TextBoxMathExpression = New System.Windows.Forms.TextBox()
        Me.LabelInputExpression = New System.Windows.Forms.Label()
        Me.TableLayoutPanelButton = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonTestFormula = New System.Windows.Forms.Button()
        Me.ToolStripForm = New System.Windows.Forms.ToolStrip()
        Me.TSComboBoxMathExpression = New System.Windows.Forms.ToolStripComboBox()
        Me.TSButtonAddMathExpression = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSComboBoxArguments = New System.Windows.Forms.ToolStripComboBox()
        Me.TSButtonAddArgument = New System.Windows.Forms.ToolStripButton()
        Me.StatusStripMessage = New System.Windows.Forms.StatusStrip()
        Me.TSStatusMessageLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DataGridViewTextBoxColumnARG = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewComboBoxColumnName = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.ToolStripContainerForm.ContentPanel.SuspendLayout()
        Me.ToolStripContainerForm.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainerForm.SuspendLayout()
        Me.TableLayoutPanelFormula.SuspendLayout()
        CType(Me.DataGridViewARG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanelButton.SuspendLayout()
        Me.ToolStripForm.SuspendLayout()
        Me.StatusStripMessage.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(626, 3)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 4
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ImageListChannels
        '
        Me.ImageListChannels.ImageStream = CType(resources.GetObject("ImageListChannels.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListChannels.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListChannels.Images.SetKeyName(0, "")
        Me.ImageListChannels.Images.SetKeyName(1, "")
        Me.ImageListChannels.Images.SetKeyName(2, "")
        Me.ImageListChannels.Images.SetKeyName(3, "")
        Me.ImageListChannels.Images.SetKeyName(4, "")
        Me.ImageListChannels.Images.SetKeyName(5, "")
        Me.ImageListChannels.Images.SetKeyName(6, "")
        Me.ImageListChannels.Images.SetKeyName(7, "")
        Me.ImageListChannels.Images.SetKeyName(8, "")
        Me.ImageListChannels.Images.SetKeyName(9, "")
        Me.ImageListChannels.Images.SetKeyName(10, "")
        Me.ImageListChannels.Images.SetKeyName(11, "")
        Me.ImageListChannels.Images.SetKeyName(12, "")
        '
        'ToolStripContainerForm
        '
        Me.ToolStripContainerForm.BottomToolStripPanelVisible = False
        '
        'ToolStripContainerForm.ContentPanel
        '
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.TableLayoutPanelFormula)
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.TableLayoutPanelButton)
        Me.ToolStripContainerForm.ContentPanel.Size = New System.Drawing.Size(704, 277)
        Me.ToolStripContainerForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainerForm.LeftToolStripPanelVisible = False
        Me.ToolStripContainerForm.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainerForm.Name = "ToolStripContainerForm"
        Me.ToolStripContainerForm.RightToolStripPanelVisible = False
        Me.ToolStripContainerForm.Size = New System.Drawing.Size(704, 316)
        Me.ToolStripContainerForm.TabIndex = 6
        Me.ToolStripContainerForm.Text = "ToolStripContainer1"
        '
        'ToolStripContainerForm.TopToolStripPanel
        '
        Me.ToolStripContainerForm.TopToolStripPanel.Controls.Add(Me.ToolStripForm)
        '
        'TableLayoutPanelFormula
        '
        Me.TableLayoutPanelFormula.ColumnCount = 2
        Me.TableLayoutPanelFormula.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.06061!))
        Me.TableLayoutPanelFormula.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.93939!))
        Me.TableLayoutPanelFormula.Controls.Add(Me.DataGridViewARG, 1, 1)
        Me.TableLayoutPanelFormula.Controls.Add(Me.LabelSelect, 1, 0)
        Me.TableLayoutPanelFormula.Controls.Add(Me.TextBoxMathExpression, 0, 1)
        Me.TableLayoutPanelFormula.Controls.Add(Me.LabelInputExpression, 0, 0)
        Me.TableLayoutPanelFormula.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelFormula.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelFormula.Name = "TableLayoutPanelFormula"
        Me.TableLayoutPanelFormula.RowCount = 2
        Me.TableLayoutPanelFormula.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.TableLayoutPanelFormula.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelFormula.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelFormula.Size = New System.Drawing.Size(704, 246)
        Me.TableLayoutPanelFormula.TabIndex = 113
        '
        'DataGridViewARG
        '
        Me.DataGridViewARG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewARG.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        Me.DataGridViewARG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewARG.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnARG, Me.DataGridViewComboBoxColumnName})
        Me.DataGridViewARG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewARG.Location = New System.Drawing.Point(397, 47)
        Me.DataGridViewARG.Name = "DataGridViewARG"
        Me.DataGridViewARG.Size = New System.Drawing.Size(304, 196)
        Me.DataGridViewARG.TabIndex = 3
        '
        'LabelSelect
        '
        Me.LabelSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSelect.Location = New System.Drawing.Point(397, 0)
        Me.LabelSelect.Name = "LabelSelect"
        Me.LabelSelect.Size = New System.Drawing.Size(304, 44)
        Me.LabelSelect.TabIndex = 2
        Me.LabelSelect.Text = "Выберите для аргумента соответствующий канал"
        '
        'TextBoxMathExpression
        '
        Me.TextBoxMathExpression.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxMathExpression.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxMathExpression.Location = New System.Drawing.Point(3, 47)
        Me.TextBoxMathExpression.Multiline = True
        Me.TextBoxMathExpression.Name = "TextBoxMathExpression"
        Me.TextBoxMathExpression.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxMathExpression.Size = New System.Drawing.Size(388, 196)
        Me.TextBoxMathExpression.TabIndex = 0
        '
        'LabelInputExpression
        '
        Me.LabelInputExpression.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelInputExpression.Location = New System.Drawing.Point(3, 0)
        Me.LabelInputExpression.Name = "LabelInputExpression"
        Me.LabelInputExpression.Size = New System.Drawing.Size(388, 44)
        Me.LabelInputExpression.TabIndex = 1
        Me.LabelInputExpression.Text = "Введите математическое выражение используя вместо парамета соответствующий ему ар" &
    "гумент например: Math.sqrt((ARG2+ARG1/735.6)/(ARG1/735.6))"
        '
        'TableLayoutPanelButton
        '
        Me.TableLayoutPanelButton.ColumnCount = 2
        Me.TableLayoutPanelButton.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.05536!))
        Me.TableLayoutPanelButton.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.94464!))
        Me.TableLayoutPanelButton.Controls.Add(Me.ButtonTestFormula, 0, 0)
        Me.TableLayoutPanelButton.Controls.Add(Me.ButtonOK, 1, 0)
        Me.TableLayoutPanelButton.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelButton.Location = New System.Drawing.Point(0, 246)
        Me.TableLayoutPanelButton.Name = "TableLayoutPanelButton"
        Me.TableLayoutPanelButton.RowCount = 1
        Me.TableLayoutPanelButton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButton.Size = New System.Drawing.Size(704, 31)
        Me.TableLayoutPanelButton.TabIndex = 114
        '
        'ButtonTestFormula
        '
        Me.ButtonTestFormula.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonTestFormula.Location = New System.Drawing.Point(256, 3)
        Me.ButtonTestFormula.Name = "ButtonTestFormula"
        Me.ButtonTestFormula.Size = New System.Drawing.Size(135, 22)
        Me.ButtonTestFormula.TabIndex = 111
        Me.ButtonTestFormula.Text = "Тест формулы"
        Me.ButtonTestFormula.UseVisualStyleBackColor = True
        '
        'ToolStripForm
        '
        Me.ToolStripForm.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStripForm.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStripForm.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStripForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSComboBoxMathExpression, Me.TSButtonAddMathExpression, Me.ToolStripSeparator2, Me.TSComboBoxArguments, Me.TSButtonAddArgument})
        Me.ToolStripForm.Location = New System.Drawing.Point(3, 0)
        Me.ToolStripForm.Name = "ToolStripForm"
        Me.ToolStripForm.Size = New System.Drawing.Size(580, 39)
        Me.ToolStripForm.TabIndex = 33
        '
        'TSComboBoxMathExpression
        '
        Me.TSComboBoxMathExpression.Name = "TSComboBoxMathExpression"
        Me.TSComboBoxMathExpression.Size = New System.Drawing.Size(190, 39)
        '
        'TSButtonAddMathExpression
        '
        Me.TSButtonAddMathExpression.Image = CType(resources.GetObject("TSButtonAddMathExpression.Image"), System.Drawing.Image)
        Me.TSButtonAddMathExpression.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonAddMathExpression.Name = "TSButtonAddMathExpression"
        Me.TSButtonAddMathExpression.Size = New System.Drawing.Size(157, 36)
        Me.TSButtonAddMathExpression.Tag = "ВставитьМатематВыражение"
        Me.TSButtonAddMathExpression.Text = "Вставить выражение"
        Me.TSButtonAddMathExpression.ToolTipText = "Вставить в текст формулы математическое выражение"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 39)
        '
        'TSComboBoxArguments
        '
        Me.TSComboBoxArguments.Name = "TSComboBoxArguments"
        Me.TSComboBoxArguments.Size = New System.Drawing.Size(75, 39)
        '
        'TSButtonAddArgument
        '
        Me.TSButtonAddArgument.Image = CType(resources.GetObject("TSButtonAddArgument.Image"), System.Drawing.Image)
        Me.TSButtonAddArgument.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonAddArgument.Name = "TSButtonAddArgument"
        Me.TSButtonAddArgument.Size = New System.Drawing.Size(145, 36)
        Me.TSButtonAddArgument.Tag = "ВставитьАргумент"
        Me.TSButtonAddArgument.Text = "Вставить аргумент"
        Me.TSButtonAddArgument.ToolTipText = "Вставить в текст формулы имя аргумента"
        '
        'StatusStripMessage
        '
        Me.StatusStripMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSStatusMessageLabel})
        Me.StatusStripMessage.Location = New System.Drawing.Point(0, 316)
        Me.StatusStripMessage.Name = "StatusStripMessage"
        Me.StatusStripMessage.Size = New System.Drawing.Size(704, 25)
        Me.StatusStripMessage.TabIndex = 35
        '
        'TSStatusMessageLabel
        '
        Me.TSStatusMessageLabel.AutoSize = False
        Me.TSStatusMessageLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.TSStatusMessageLabel.Image = CType(resources.GetObject("TSStatusMessageLabel.Image"), System.Drawing.Image)
        Me.TSStatusMessageLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TSStatusMessageLabel.Name = "TSStatusMessageLabel"
        Me.TSStatusMessageLabel.Size = New System.Drawing.Size(689, 20)
        Me.TSStatusMessageLabel.Spring = True
        Me.TSStatusMessageLabel.Text = "Готов"
        Me.TSStatusMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DataGridViewTextBoxColumnARG
        '
        Me.DataGridViewTextBoxColumnARG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnARG.HeaderText = "Имя аргумента"
        Me.DataGridViewTextBoxColumnARG.Name = "DataGridViewTextBoxColumnARG"
        Me.DataGridViewTextBoxColumnARG.ReadOnly = True
        '
        'DataGridViewComboBoxColumnName
        '
        Me.DataGridViewComboBoxColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewComboBoxColumnName.HeaderText = "Имя канала"
        Me.DataGridViewComboBoxColumnName.MaxDropDownItems = 32
        Me.DataGridViewComboBoxColumnName.Name = "DataGridViewComboBoxColumnName"
        '
        'FormFormulaEditor
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 341)
        Me.Controls.Add(Me.ToolStripContainerForm)
        Me.Controls.Add(Me.StatusStripMessage)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(720, 380)
        Me.Name = "FormFormulaEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Редактор формулы"
        Me.ToolStripContainerForm.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainerForm.ResumeLayout(False)
        Me.ToolStripContainerForm.PerformLayout()
        Me.TableLayoutPanelFormula.ResumeLayout(False)
        Me.TableLayoutPanelFormula.PerformLayout()
        CType(Me.DataGridViewARG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanelButton.ResumeLayout(False)
        Me.ToolStripForm.ResumeLayout(False)
        Me.ToolStripForm.PerformLayout()
        Me.StatusStripMessage.ResumeLayout(False)
        Me.StatusStripMessage.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ImageListChannels As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripContainerForm As System.Windows.Forms.ToolStripContainer
    Private WithEvents ToolStripForm As System.Windows.Forms.ToolStrip
    Friend WithEvents TSComboBoxMathExpression As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents TSButtonAddMathExpression As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSComboBoxArguments As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents TSButtonAddArgument As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonTestFormula As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanelFormula As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridViewARG As System.Windows.Forms.DataGridView
    Friend WithEvents LabelSelect As System.Windows.Forms.Label
    Friend WithEvents TextBoxMathExpression As System.Windows.Forms.TextBox
    Friend WithEvents LabelInputExpression As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanelButton As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents StatusStripMessage As System.Windows.Forms.StatusStrip
    Friend WithEvents TSStatusMessageLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents DataGridViewTextBoxColumnARG As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumnName As DataGridViewComboBoxColumn
End Class
