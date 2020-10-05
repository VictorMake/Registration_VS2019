<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormConditionFind
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormConditionFind))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LabelSelectParam = New System.Windows.Forms.Label()
        Me.ComboBoxValueParameters = New System.Windows.Forms.ComboBox()
        Me.LabelSelectCondition = New System.Windows.Forms.Label()
        Me.ToolTipHelp = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonBackToStep2 = New System.Windows.Forms.Button()
        Me.ButtonExit = New System.Windows.Forms.Button()
        Me.TextBoxValue = New System.Windows.Forms.TextBox()
        Me.TextBoxStart = New System.Windows.Forms.TextBox()
        Me.ComboBoxEnumConditions = New System.Windows.Forms.ComboBox()
        Me.TextBoxEnd = New System.Windows.Forms.TextBox()
        Me.ComboBoxPatternParameters = New System.Windows.Forms.ComboBox()
        Me.NumericTimeWindow = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideTimeWindow = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericTime = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideTime = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericSlideValueFinishUp = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueFinishUp = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericValueMax = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.NumericSlideValueFinishDown = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueFinishDown = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericSlideValueStartUp = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueStartUp = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericSlideValueStartDown = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueStartDown = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericSlideValueMiddleDown = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueMiddleDown = New NationalInstruments.UI.WindowsForms.Slide()
        Me.NumericSlideValueMiddleUp = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SlideValueMiddleUp = New NationalInstruments.UI.WindowsForms.Slide()
        Me.ButtonDeleteCondition = New System.Windows.Forms.Button()
        Me.ButtonLoadCondition = New System.Windows.Forms.Button()
        Me.ButtonSaveCondition = New System.Windows.Forms.Button()
        Me.TextBoxNameConfiguration = New System.Windows.Forms.TextBox()
        Me.ListViewAllSnapshot = New System.Windows.Forms.ListView()
        Me.ImageListFolder = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonBackToStep1 = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonFind = New System.Windows.Forms.Button()
        Me.ButtonDelete = New System.Windows.Forms.Button()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.ButtonForwardToStep2 = New System.Windows.Forms.Button()
        Me.ButtonSelectAll = New System.Windows.Forms.Button()
        Me.ButtonEraseSelect = New System.Windows.Forms.Button()
        Me.TabControlAll = New System.Windows.Forms.TabControl()
        Me.TabPageCondition = New System.Windows.Forms.TabPage()
        Me.TabControlCondition = New System.Windows.Forms.TabControl()
        Me.TabPageTrigger = New System.Windows.Forms.TabPage()
        Me.TabPageTemplate = New System.Windows.Forms.TabPage()
        Me.ImageListCondition = New System.Windows.Forms.ImageList(Me.components)
        Me.DataGridFoundSnapshot = New System.Windows.Forms.DataGridView()
        Me.ButtonFindChannelPattern = New System.Windows.Forms.Button()
        Me.ButtonFindChanneValue = New System.Windows.Forms.Button()
        Me.PanelSetValue = New System.Windows.Forms.Panel()
        Me.LabelAnd = New System.Windows.Forms.Label()
        Me.PanelTemplate = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelChannelPattern = New System.Windows.Forms.Panel()
        Me.LabelSelect = New System.Windows.Forms.Label()
        Me.LabelSet = New System.Windows.Forms.Label()
        Me.GraphPatternWave = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.XYCursorUpLeft = New NationalInstruments.UI.XYCursor()
        Me.PlotUp = New NationalInstruments.UI.WaveformPlot()
        Me.PlotDown = New NationalInstruments.UI.WaveformPlot()
        Me.XAxis = New NationalInstruments.UI.XAxis()
        Me.YAxis = New NationalInstruments.UI.YAxis()
        Me.XYCursorDownLeft = New NationalInstruments.UI.XYCursor()
        Me.XYCursorUpMiddle = New NationalInstruments.UI.XYCursor()
        Me.XYCursorDownMiddle = New NationalInstruments.UI.XYCursor()
        Me.XYCursorDownRight = New NationalInstruments.UI.XYCursor()
        Me.XYCursorUpRight = New NationalInstruments.UI.XYCursor()
        Me.PanelNumericTime = New System.Windows.Forms.Panel()
        Me.PanelNumericTimeWindow = New System.Windows.Forms.Panel()
        Me.PanelFoundSnapshot = New System.Windows.Forms.Panel()
        Me.LabelDescriptionStage = New System.Windows.Forms.Label()
        Me.PanelNavigation = New System.Windows.Forms.Panel()
        Me.LabelPosition = New System.Windows.Forms.Label()
        Me.ButtonNext = New System.Windows.Forms.Button()
        Me.ButtonPrevious = New System.Windows.Forms.Button()
        Me.PanelStep2 = New System.Windows.Forms.Panel()
        Me.PanelButtonsStep2 = New System.Windows.Forms.Panel()
        Me.ImageListSteps = New System.Windows.Forms.ImageList(Me.components)
        Me.ImageListSetting = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStripMessage = New System.Windows.Forms.StatusStrip()
        Me.TSLabelMessage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.SplitContainerCondition = New System.Windows.Forms.SplitContainer()
        Me.XmlTreeView = New System.Windows.Forms.TreeView()
        Me.LabelAllSetting = New System.Windows.Forms.Label()
        Me.PanelConfiguration = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelConfiguration = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSelectFolder = New System.Windows.Forms.Label()
        Me.TreeViewAllSnapshot = New System.Windows.Forms.TreeView()
        Me.LabbelSelectSnapshot = New System.Windows.Forms.Label()
        Me.SplitContainerEngine = New System.Windows.Forms.SplitContainer()
        Me.PanelSelectRecord = New System.Windows.Forms.Panel()
        Me.SplitContainerStep3 = New System.Windows.Forms.SplitContainer()
        Me.ToolStripSteps = New System.Windows.Forms.ToolStrip()
        Me.TSButtonSourceStep1 = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonConditionStep2 = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonResultStep3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.LabelCaptionSteps = New System.Windows.Forms.ToolStripLabel()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.BindingSourceFoundedSnapshotDataTable = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.NumericTimeWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideTimeWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueFinishUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueFinishUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericValueMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueFinishDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueFinishDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueStartUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueStartUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueStartDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueStartDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueMiddleDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueMiddleDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSlideValueMiddleUp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideValueMiddleUp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlAll.SuspendLayout()
        Me.TabPageCondition.SuspendLayout()
        Me.TabControlCondition.SuspendLayout()
        CType(Me.DataGridFoundSnapshot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSetValue.SuspendLayout()
        Me.PanelTemplate.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.PanelChannelPattern.SuspendLayout()
        CType(Me.GraphPatternWave, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorUpLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorDownLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorUpMiddle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorDownMiddle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorDownRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursorUpRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelNumericTime.SuspendLayout()
        Me.PanelNumericTimeWindow.SuspendLayout()
        Me.PanelFoundSnapshot.SuspendLayout()
        Me.PanelNavigation.SuspendLayout()
        Me.PanelStep2.SuspendLayout()
        Me.PanelButtonsStep2.SuspendLayout()
        Me.StatusStripMessage.SuspendLayout()
        CType(Me.SplitContainerCondition, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerCondition.Panel1.SuspendLayout()
        Me.SplitContainerCondition.Panel2.SuspendLayout()
        Me.SplitContainerCondition.SuspendLayout()
        Me.PanelConfiguration.SuspendLayout()
        Me.TableLayoutPanelConfiguration.SuspendLayout()
        CType(Me.SplitContainerEngine, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerEngine.Panel1.SuspendLayout()
        Me.SplitContainerEngine.Panel2.SuspendLayout()
        Me.SplitContainerEngine.SuspendLayout()
        Me.PanelSelectRecord.SuspendLayout()
        CType(Me.SplitContainerStep3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerStep3.Panel1.SuspendLayout()
        Me.SplitContainerStep3.Panel2.SuspendLayout()
        Me.SplitContainerStep3.SuspendLayout()
        Me.ToolStripSteps.SuspendLayout()
        CType(Me.BindingSourceFoundedSnapshotDataTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelSelectParam
        '
        Me.LabelSelectParam.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LabelSelectParam.Location = New System.Drawing.Point(16, 32)
        Me.LabelSelectParam.Name = "LabelSelectParam"
        Me.LabelSelectParam.Size = New System.Drawing.Size(104, 16)
        Me.LabelSelectParam.TabIndex = 50
        Me.LabelSelectParam.Text = "Выбрать параметр"
        Me.LabelSelectParam.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxValueParameters
        '
        Me.ComboBoxValueParameters.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ComboBoxValueParameters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxValueParameters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxValueParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxValueParameters.DropDownWidth = 141
        Me.ComboBoxValueParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxValueParameters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxValueParameters.Location = New System.Drawing.Point(16, 56)
        Me.ComboBoxValueParameters.MaxDropDownItems = 32
        Me.ComboBoxValueParameters.Name = "ComboBoxValueParameters"
        Me.ComboBoxValueParameters.Size = New System.Drawing.Size(116, 21)
        Me.ComboBoxValueParameters.TabIndex = 101
        Me.ToolTipHelp.SetToolTip(Me.ComboBoxValueParameters, "Выбрать параметр")
        '
        'LabelSelectCondition
        '
        Me.LabelSelectCondition.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LabelSelectCondition.Location = New System.Drawing.Point(168, 32)
        Me.LabelSelectCondition.Name = "LabelSelectCondition"
        Me.LabelSelectCondition.Size = New System.Drawing.Size(96, 16)
        Me.LabelSelectCondition.TabIndex = 51
        Me.LabelSelectCondition.Text = "Выбрать условие"
        Me.LabelSelectCondition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ButtonBackToStep2
        '
        Me.ButtonBackToStep2.BackColor = System.Drawing.Color.Silver
        Me.ButtonBackToStep2.Image = CType(resources.GetObject("ButtonBackToStep2.Image"), System.Drawing.Image)
        Me.ButtonBackToStep2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonBackToStep2.Location = New System.Drawing.Point(8, 6)
        Me.ButtonBackToStep2.Name = "ButtonBackToStep2"
        Me.ButtonBackToStep2.Size = New System.Drawing.Size(160, 27)
        Me.ButtonBackToStep2.TabIndex = 4
        Me.ButtonBackToStep2.Text = "<<&Вернуться к условиям"
        Me.ButtonBackToStep2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonBackToStep2, "Вернуться на предыдущий шаг")
        Me.ButtonBackToStep2.UseVisualStyleBackColor = False
        '
        'ButtonExit
        '
        Me.ButtonExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExit.BackColor = System.Drawing.Color.Silver
        Me.ButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonExit.Image = CType(resources.GetObject("ButtonExit.Image"), System.Drawing.Image)
        Me.ButtonExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExit.Location = New System.Drawing.Point(537, 6)
        Me.ButtonExit.Name = "ButtonExit"
        Me.ButtonExit.Size = New System.Drawing.Size(160, 27)
        Me.ButtonExit.TabIndex = 3
        Me.ButtonExit.Text = "&Выход из поиска"
        Me.ToolTipHelp.SetToolTip(Me.ButtonExit, "Закрыть окно")
        Me.ButtonExit.UseVisualStyleBackColor = False
        '
        'TextBoxValue
        '
        Me.TextBoxValue.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TextBoxValue.Location = New System.Drawing.Point(304, 56)
        Me.TextBoxValue.Name = "TextBoxValue"
        Me.TextBoxValue.Size = New System.Drawing.Size(160, 20)
        Me.TextBoxValue.TabIndex = 6
        Me.TextBoxValue.Text = "0"
        Me.TextBoxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTipHelp.SetToolTip(Me.TextBoxValue, "Ввести значение")
        Me.TextBoxValue.Visible = False
        Me.TextBoxValue.WordWrap = False
        '
        'TextBoxStart
        '
        Me.TextBoxStart.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TextBoxStart.Location = New System.Drawing.Point(304, 56)
        Me.TextBoxStart.Name = "TextBoxStart"
        Me.TextBoxStart.Size = New System.Drawing.Size(64, 20)
        Me.TextBoxStart.TabIndex = 1
        Me.TextBoxStart.Text = "0"
        Me.TextBoxStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTipHelp.SetToolTip(Me.TextBoxStart, "Ввести нижнее значение")
        Me.TextBoxStart.Visible = False
        Me.TextBoxStart.WordWrap = False
        '
        'ComboBoxEnumConditions
        '
        Me.ComboBoxEnumConditions.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ComboBoxEnumConditions.Items.AddRange(New Object() {"между", "вне", "равно", "не равно", "больше", "меньше", "больше или равно", "меньше или равно", "найти максимум", "найти минимум"})
        Me.ComboBoxEnumConditions.Location = New System.Drawing.Point(168, 56)
        Me.ComboBoxEnumConditions.MaxDropDownItems = 10
        Me.ComboBoxEnumConditions.Name = "ComboBoxEnumConditions"
        Me.ComboBoxEnumConditions.Size = New System.Drawing.Size(128, 21)
        Me.ComboBoxEnumConditions.TabIndex = 0
        Me.ComboBoxEnumConditions.Tag = "EnumCondition"
        Me.ComboBoxEnumConditions.Text = "EnumCondition"
        Me.ToolTipHelp.SetToolTip(Me.ComboBoxEnumConditions, "Выбрать условие")
        '
        'TextBoxEnd
        '
        Me.TextBoxEnd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TextBoxEnd.Location = New System.Drawing.Point(400, 56)
        Me.TextBoxEnd.Name = "TextBoxEnd"
        Me.TextBoxEnd.Size = New System.Drawing.Size(64, 20)
        Me.TextBoxEnd.TabIndex = 5
        Me.TextBoxEnd.Text = "0"
        Me.TextBoxEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTipHelp.SetToolTip(Me.TextBoxEnd, "Ввести вверхнее значение")
        Me.TextBoxEnd.Visible = False
        Me.TextBoxEnd.WordWrap = False
        '
        'ComboBoxPatternParameters
        '
        Me.ComboBoxPatternParameters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxPatternParameters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxPatternParameters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxPatternParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPatternParameters.DropDownWidth = 141
        Me.ComboBoxPatternParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxPatternParameters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxPatternParameters.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxPatternParameters.MaxDropDownItems = 32
        Me.ComboBoxPatternParameters.Name = "ComboBoxPatternParameters"
        Me.ComboBoxPatternParameters.Size = New System.Drawing.Size(94, 21)
        Me.ComboBoxPatternParameters.TabIndex = 102
        Me.ToolTipHelp.SetToolTip(Me.ComboBoxPatternParameters, "Выбрать параметр")
        '
        'NumericTimeWindow
        '
        Me.NumericTimeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericTimeWindow.Location = New System.Drawing.Point(3, 56)
        Me.NumericTimeWindow.Name = "NumericTimeWindow"
        Me.NumericTimeWindow.Size = New System.Drawing.Size(56, 20)
        Me.NumericTimeWindow.Source = Me.SlideTimeWindow
        Me.NumericTimeWindow.TabIndex = 35
        Me.ToolTipHelp.SetToolTip(Me.NumericTimeWindow, "Ввести значение")
        Me.NumericTimeWindow.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideTimeWindow
        '
        Me.SlideTimeWindow.AutoDivisionSpacing = False
        Me.SlideTimeWindow.Border = NationalInstruments.UI.Border.Raised
        Me.SlideTimeWindow.Caption = "Окно времени шаблона сек."
        Me.SlideTimeWindow.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        Me.TableLayoutPanel1.SetColumnSpan(Me.SlideTimeWindow, 2)
        Me.SlideTimeWindow.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SlideTimeWindow.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideTimeWindow.Location = New System.Drawing.Point(203, 489)
        Me.SlideTimeWindow.MajorDivisions.Base = 1.0R
        Me.SlideTimeWindow.MajorDivisions.Interval = 10.0R
        Me.SlideTimeWindow.Name = "SlideTimeWindow"
        Me.SlideTimeWindow.PointerColor = System.Drawing.Color.Maroon
        Me.SlideTimeWindow.Range = New NationalInstruments.UI.Range(1.0R, 21.0R)
        Me.SlideTimeWindow.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        Me.SlideTimeWindow.Size = New System.Drawing.Size(146, 79)
        Me.SlideTimeWindow.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideTimeWindow.TabIndex = 34
        Me.SlideTimeWindow.Value = 10.0R
        '
        'NumericTime
        '
        Me.NumericTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericTime.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2)
        Me.NumericTime.Location = New System.Drawing.Point(3, 56)
        Me.NumericTime.Name = "NumericTime"
        Me.NumericTime.Size = New System.Drawing.Size(56, 20)
        Me.NumericTime.Source = Me.SlideTime
        Me.NumericTime.TabIndex = 33
        Me.ToolTipHelp.SetToolTip(Me.NumericTime, "Ввести значение")
        Me.NumericTime.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideTime
        '
        Me.SlideTime.AutoDivisionSpacing = False
        Me.SlideTime.Border = NationalInstruments.UI.Border.Raised
        Me.SlideTime.Caption = "Время перегиба сек."
        Me.TableLayoutPanel1.SetColumnSpan(Me.SlideTime, 2)
        Me.SlideTime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SlideTime.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideTime.Location = New System.Drawing.Point(203, 404)
        Me.SlideTime.Name = "SlideTime"
        Me.SlideTime.PointerColor = System.Drawing.Color.Maroon
        Me.SlideTime.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        Me.SlideTime.Size = New System.Drawing.Size(146, 79)
        Me.SlideTime.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideTime.TabIndex = 32
        Me.SlideTime.ToolTipFormat = New NationalInstruments.UI.FormatString(NationalInstruments.UI.FormatStringMode.Numeric, "F3")
        Me.SlideTime.Value = 5.0R
        '
        'NumericSlideValueFinishUp
        '
        Me.NumericSlideValueFinishUp.Dock = System.Windows.Forms.DockStyle.Right
        Me.NumericSlideValueFinishUp.Location = New System.Drawing.Point(393, 233)
        Me.NumericSlideValueFinishUp.Name = "NumericSlideValueFinishUp"
        Me.NumericSlideValueFinishUp.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueFinishUp.Source = Me.SlideValueFinishUp
        Me.NumericSlideValueFinishUp.TabIndex = 43
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueFinishUp, "Ввести значение")
        Me.NumericSlideValueFinishUp.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueFinishUp
        '
        Me.SlideValueFinishUp.AutoDivisionSpacing = False
        Me.SlideValueFinishUp.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueFinishUp.Caption = "Верхнее значение конца"
        Me.SlideValueFinishUp.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueFinishUp.Dock = System.Windows.Forms.DockStyle.Left
        Me.SlideValueFinishUp.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueFinishUp.Location = New System.Drawing.Point(355, 263)
        Me.SlideValueFinishUp.MajorDivisions.Interval = 10.0R
        Me.SlideValueFinishUp.Name = "SlideValueFinishUp"
        Me.SlideValueFinishUp.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueFinishUp.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.SlideValueFinishUp.Size = New System.Drawing.Size(84, 135)
        Me.SlideValueFinishUp.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueFinishUp.TabIndex = 42
        Me.SlideValueFinishUp.Value = 100.0R
        '
        'NumericValueMax
        '
        Me.NumericValueMax.Location = New System.Drawing.Point(103, 56)
        Me.NumericValueMax.Name = "NumericValueMax"
        Me.NumericValueMax.Size = New System.Drawing.Size(56, 20)
        Me.NumericValueMax.TabIndex = 26
        Me.ToolTipHelp.SetToolTip(Me.NumericValueMax, "Ввести значение")
        Me.NumericValueMax.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.NumericValueMax.Value = 100.0R
        '
        'NumericSlideValueFinishDown
        '
        Me.NumericSlideValueFinishDown.Dock = System.Windows.Forms.DockStyle.Right
        Me.NumericSlideValueFinishDown.Location = New System.Drawing.Point(493, 233)
        Me.NumericSlideValueFinishDown.Name = "NumericSlideValueFinishDown"
        Me.NumericSlideValueFinishDown.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueFinishDown.Source = Me.SlideValueFinishDown
        Me.NumericSlideValueFinishDown.TabIndex = 41
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueFinishDown, "Ввести значение")
        Me.NumericSlideValueFinishDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueFinishDown
        '
        Me.SlideValueFinishDown.AutoDivisionSpacing = False
        Me.SlideValueFinishDown.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueFinishDown.Caption = "Нижнее значение конца"
        Me.SlideValueFinishDown.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueFinishDown.Dock = System.Windows.Forms.DockStyle.Left
        Me.SlideValueFinishDown.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueFinishDown.Location = New System.Drawing.Point(455, 263)
        Me.SlideValueFinishDown.MajorDivisions.Interval = 10.0R
        Me.SlideValueFinishDown.Name = "SlideValueFinishDown"
        Me.SlideValueFinishDown.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueFinishDown.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.SlideValueFinishDown.Size = New System.Drawing.Size(84, 135)
        Me.SlideValueFinishDown.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueFinishDown.TabIndex = 40
        Me.SlideValueFinishDown.Value = 100.0R
        '
        'NumericSlideValueStartUp
        '
        Me.NumericSlideValueStartUp.Dock = System.Windows.Forms.DockStyle.Right
        Me.NumericSlideValueStartUp.Location = New System.Drawing.Point(41, 233)
        Me.NumericSlideValueStartUp.Name = "NumericSlideValueStartUp"
        Me.NumericSlideValueStartUp.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueStartUp.Source = Me.SlideValueStartUp
        Me.NumericSlideValueStartUp.TabIndex = 39
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueStartUp, "Ввести значение")
        Me.NumericSlideValueStartUp.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueStartUp
        '
        Me.SlideValueStartUp.AutoDivisionSpacing = False
        Me.SlideValueStartUp.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueStartUp.Caption = "Верхнее значение начала"
        Me.SlideValueStartUp.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueStartUp.Dock = System.Windows.Forms.DockStyle.Left
        Me.SlideValueStartUp.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueStartUp.Location = New System.Drawing.Point(3, 263)
        Me.SlideValueStartUp.MajorDivisions.Interval = 10.0R
        Me.SlideValueStartUp.Name = "SlideValueStartUp"
        Me.SlideValueStartUp.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueStartUp.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.SlideValueStartUp.Size = New System.Drawing.Size(84, 135)
        Me.SlideValueStartUp.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueStartUp.TabIndex = 38
        Me.SlideValueStartUp.Value = 100.0R
        '
        'NumericSlideValueStartDown
        '
        Me.NumericSlideValueStartDown.Dock = System.Windows.Forms.DockStyle.Right
        Me.NumericSlideValueStartDown.Location = New System.Drawing.Point(141, 233)
        Me.NumericSlideValueStartDown.Name = "NumericSlideValueStartDown"
        Me.NumericSlideValueStartDown.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueStartDown.Source = Me.SlideValueStartDown
        Me.NumericSlideValueStartDown.TabIndex = 37
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueStartDown, "Ввести значение")
        Me.NumericSlideValueStartDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueStartDown
        '
        Me.SlideValueStartDown.AutoDivisionSpacing = False
        Me.SlideValueStartDown.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueStartDown.Caption = "Нижнее значение начала"
        Me.SlideValueStartDown.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueStartDown.Dock = System.Windows.Forms.DockStyle.Left
        Me.SlideValueStartDown.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueStartDown.Location = New System.Drawing.Point(103, 263)
        Me.SlideValueStartDown.MajorDivisions.Interval = 10.0R
        Me.SlideValueStartDown.Name = "SlideValueStartDown"
        Me.SlideValueStartDown.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueStartDown.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.SlideValueStartDown.Size = New System.Drawing.Size(84, 135)
        Me.SlideValueStartDown.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueStartDown.TabIndex = 36
        Me.SlideValueStartDown.Value = 100.0R
        '
        'NumericSlideValueMiddleDown
        '
        Me.NumericSlideValueMiddleDown.Location = New System.Drawing.Point(279, 3)
        Me.NumericSlideValueMiddleDown.Name = "NumericSlideValueMiddleDown"
        Me.NumericSlideValueMiddleDown.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueMiddleDown.Source = Me.SlideValueMiddleDown
        Me.NumericSlideValueMiddleDown.TabIndex = 45
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueMiddleDown, "Ввести значение")
        Me.NumericSlideValueMiddleDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueMiddleDown
        '
        Me.SlideValueMiddleDown.AutoDivisionSpacing = False
        Me.SlideValueMiddleDown.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueMiddleDown.Caption = "Нижнее значение перегиба"
        Me.SlideValueMiddleDown.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueMiddleDown.Dock = System.Windows.Forms.DockStyle.Left
        Me.SlideValueMiddleDown.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueMiddleDown.Location = New System.Drawing.Point(279, 56)
        Me.SlideValueMiddleDown.MajorDivisions.Interval = 10.0R
        Me.SlideValueMiddleDown.Name = "SlideValueMiddleDown"
        Me.SlideValueMiddleDown.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueMiddleDown.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.TableLayoutPanel1.SetRowSpan(Me.SlideValueMiddleDown, 2)
        Me.SlideValueMiddleDown.Size = New System.Drawing.Size(70, 201)
        Me.SlideValueMiddleDown.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueMiddleDown.TabIndex = 44
        Me.SlideValueMiddleDown.Value = 100.0R
        '
        'NumericSlideValueMiddleUp
        '
        Me.NumericSlideValueMiddleUp.Dock = System.Windows.Forms.DockStyle.Right
        Me.NumericSlideValueMiddleUp.Location = New System.Drawing.Point(217, 3)
        Me.NumericSlideValueMiddleUp.Name = "NumericSlideValueMiddleUp"
        Me.NumericSlideValueMiddleUp.Size = New System.Drawing.Size(56, 20)
        Me.NumericSlideValueMiddleUp.Source = Me.SlideValueMiddleUp
        Me.NumericSlideValueMiddleUp.TabIndex = 47
        Me.ToolTipHelp.SetToolTip(Me.NumericSlideValueMiddleUp, "Ввести значение")
        Me.NumericSlideValueMiddleUp.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'SlideValueMiddleUp
        '
        Me.SlideValueMiddleUp.AutoDivisionSpacing = False
        Me.SlideValueMiddleUp.Border = NationalInstruments.UI.Border.Raised
        Me.SlideValueMiddleUp.Caption = "Верхнее значение перегиба"
        Me.SlideValueMiddleUp.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.SlideValueMiddleUp.Dock = System.Windows.Forms.DockStyle.Right
        Me.SlideValueMiddleUp.FillMode = NationalInstruments.UI.NumericFillMode.None
        Me.SlideValueMiddleUp.Location = New System.Drawing.Point(203, 56)
        Me.SlideValueMiddleUp.MajorDivisions.Interval = 10.0R
        Me.SlideValueMiddleUp.Name = "SlideValueMiddleUp"
        Me.SlideValueMiddleUp.PointerColor = System.Drawing.Color.Maroon
        Me.SlideValueMiddleUp.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.TableLayoutPanel1.SetRowSpan(Me.SlideValueMiddleUp, 2)
        Me.SlideValueMiddleUp.Size = New System.Drawing.Size(70, 201)
        Me.SlideValueMiddleUp.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideValueMiddleUp.TabIndex = 46
        Me.SlideValueMiddleUp.Value = 100.0R
        '
        'ButtonDeleteCondition
        '
        Me.ButtonDeleteCondition.BackColor = System.Drawing.Color.Silver
        Me.ButtonDeleteCondition.Enabled = False
        Me.ButtonDeleteCondition.Image = CType(resources.GetObject("ButtonDeleteCondition.Image"), System.Drawing.Image)
        Me.ButtonDeleteCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDeleteCondition.Location = New System.Drawing.Point(191, 3)
        Me.ButtonDeleteCondition.Name = "ButtonDeleteCondition"
        Me.ButtonDeleteCondition.Size = New System.Drawing.Size(80, 30)
        Me.ButtonDeleteCondition.TabIndex = 16
        Me.ButtonDeleteCondition.Text = "&Удалить"
        Me.ButtonDeleteCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonDeleteCondition, "Удалить условие")
        Me.ButtonDeleteCondition.UseVisualStyleBackColor = False
        '
        'ButtonLoadCondition
        '
        Me.ButtonLoadCondition.BackColor = System.Drawing.Color.Silver
        Me.ButtonLoadCondition.Enabled = False
        Me.ButtonLoadCondition.Image = CType(resources.GetObject("ButtonLoadCondition.Image"), System.Drawing.Image)
        Me.ButtonLoadCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonLoadCondition.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLoadCondition.Name = "ButtonLoadCondition"
        Me.ButtonLoadCondition.Size = New System.Drawing.Size(80, 30)
        Me.ButtonLoadCondition.TabIndex = 15
        Me.ButtonLoadCondition.Text = "&Считать"
        Me.ButtonLoadCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonLoadCondition, "Считать условие")
        Me.ButtonLoadCondition.UseVisualStyleBackColor = False
        '
        'ButtonSaveCondition
        '
        Me.ButtonSaveCondition.BackColor = System.Drawing.Color.Silver
        Me.ButtonSaveCondition.Enabled = False
        Me.ButtonSaveCondition.Image = CType(resources.GetObject("ButtonSaveCondition.Image"), System.Drawing.Image)
        Me.ButtonSaveCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSaveCondition.Location = New System.Drawing.Point(97, 3)
        Me.ButtonSaveCondition.Name = "ButtonSaveCondition"
        Me.ButtonSaveCondition.Size = New System.Drawing.Size(84, 30)
        Me.ButtonSaveCondition.TabIndex = 14
        Me.ButtonSaveCondition.Text = "&Записать"
        Me.ButtonSaveCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonSaveCondition, "Записать условие")
        Me.ButtonSaveCondition.UseVisualStyleBackColor = False
        '
        'TextBoxNameConfiguration
        '
        Me.TextBoxNameConfiguration.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxNameConfiguration.Location = New System.Drawing.Point(8, 8)
        Me.TextBoxNameConfiguration.Name = "TextBoxNameConfiguration"
        Me.TextBoxNameConfiguration.Size = New System.Drawing.Size(282, 20)
        Me.TextBoxNameConfiguration.TabIndex = 13
        Me.ToolTipHelp.SetToolTip(Me.TextBoxNameConfiguration, "Наименование условия")
        '
        'ListViewAllSnapshot
        '
        Me.ListViewAllSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewAllSnapshot.FullRowSelect = True
        Me.ListViewAllSnapshot.GridLines = True
        Me.ListViewAllSnapshot.HideSelection = False
        Me.ListViewAllSnapshot.Location = New System.Drawing.Point(0, 20)
        Me.ListViewAllSnapshot.Name = "ListViewAllSnapshot"
        Me.ListViewAllSnapshot.Size = New System.Drawing.Size(689, 89)
        Me.ListViewAllSnapshot.SmallImageList = Me.ImageListFolder
        Me.ListViewAllSnapshot.TabIndex = 14
        Me.ToolTipHelp.SetToolTip(Me.ListViewAllSnapshot, "Выделить снимки")
        Me.ListViewAllSnapshot.UseCompatibleStateImageBehavior = False
        Me.ListViewAllSnapshot.View = System.Windows.Forms.View.Details
        '
        'ImageListFolder
        '
        Me.ImageListFolder.ImageStream = CType(resources.GetObject("ImageListFolder.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListFolder.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListFolder.Images.SetKeyName(0, "Close0")
        Me.ImageListFolder.Images.SetKeyName(1, "Open1")
        Me.ImageListFolder.Images.SetKeyName(2, "GraphSnapshot2")
        Me.ImageListFolder.Images.SetKeyName(3, "GraphRegistration3")
        Me.ImageListFolder.Images.SetKeyName(4, "Apply4")
        '
        'ButtonBackToStep1
        '
        Me.ButtonBackToStep1.BackColor = System.Drawing.Color.Silver
        Me.ButtonBackToStep1.Image = CType(resources.GetObject("ButtonBackToStep1.Image"), System.Drawing.Image)
        Me.ButtonBackToStep1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonBackToStep1.Location = New System.Drawing.Point(14, 6)
        Me.ButtonBackToStep1.Name = "ButtonBackToStep1"
        Me.ButtonBackToStep1.Size = New System.Drawing.Size(152, 28)
        Me.ButtonBackToStep1.TabIndex = 4
        Me.ButtonBackToStep1.Text = "<<&Вернуться к выбору"
        Me.ButtonBackToStep1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonBackToStep1, "Вернуться на предыдущий шаг")
        Me.ButtonBackToStep1.UseVisualStyleBackColor = False
        '
        'ButtonCancel
        '
        Me.ButtonCancel.BackColor = System.Drawing.Color.Silver
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Enabled = False
        Me.ButtonCancel.Image = CType(resources.GetObject("ButtonCancel.Image"), System.Drawing.Image)
        Me.ButtonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonCancel.Location = New System.Drawing.Point(560, 6)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(120, 28)
        Me.ButtonCancel.TabIndex = 3
        Me.ButtonCancel.Text = "&Прервать поиск"
        Me.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonCancel, "Прервать поиск")
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'ButtonFind
        '
        Me.ButtonFind.BackColor = System.Drawing.Color.Silver
        Me.ButtonFind.Enabled = False
        Me.ButtonFind.Image = CType(resources.GetObject("ButtonFind.Image"), System.Drawing.Image)
        Me.ButtonFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFind.Location = New System.Drawing.Point(454, 6)
        Me.ButtonFind.Name = "ButtonFind"
        Me.ButtonFind.Size = New System.Drawing.Size(104, 28)
        Me.ButtonFind.TabIndex = 2
        Me.ButtonFind.Text = "&Искать >>"
        Me.ToolTipHelp.SetToolTip(Me.ButtonFind, "Запустить поиск")
        Me.ButtonFind.UseVisualStyleBackColor = False
        '
        'ButtonDelete
        '
        Me.ButtonDelete.BackColor = System.Drawing.Color.Silver
        Me.ButtonDelete.Image = CType(resources.GetObject("ButtonDelete.Image"), System.Drawing.Image)
        Me.ButtonDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDelete.Location = New System.Drawing.Point(310, 6)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.Size = New System.Drawing.Size(136, 28)
        Me.ButtonDelete.TabIndex = 1
        Me.ButtonDelete.Text = "&Удалить условие..."
        Me.ButtonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonDelete, "Удалить лишнее условие")
        Me.ButtonDelete.UseVisualStyleBackColor = False
        '
        'ButtonAdd
        '
        Me.ButtonAdd.BackColor = System.Drawing.Color.Silver
        Me.ButtonAdd.Image = CType(resources.GetObject("ButtonAdd.Image"), System.Drawing.Image)
        Me.ButtonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAdd.Location = New System.Drawing.Point(174, 6)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(128, 28)
        Me.ButtonAdd.TabIndex = 0
        Me.ButtonAdd.Text = "&Добавить условие"
        Me.ButtonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipHelp.SetToolTip(Me.ButtonAdd, "Добавить шаблон нового условия")
        Me.ButtonAdd.UseVisualStyleBackColor = False
        '
        'ButtonForwardToStep2
        '
        Me.ButtonForwardToStep2.BackColor = System.Drawing.Color.Silver
        Me.ButtonForwardToStep2.Enabled = False
        Me.ButtonForwardToStep2.Image = CType(resources.GetObject("ButtonForwardToStep2.Image"), System.Drawing.Image)
        Me.ButtonForwardToStep2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonForwardToStep2.Location = New System.Drawing.Point(276, 6)
        Me.ButtonForwardToStep2.Name = "ButtonForwardToStep2"
        Me.ButtonForwardToStep2.Size = New System.Drawing.Size(137, 28)
        Me.ButtonForwardToStep2.TabIndex = 5
        Me.ButtonForwardToStep2.Text = "&Далее >>"
        Me.ToolTipHelp.SetToolTip(Me.ButtonForwardToStep2, "Перейти к заданию условий")
        Me.ButtonForwardToStep2.UseVisualStyleBackColor = False
        '
        'ButtonSelectAll
        '
        Me.ButtonSelectAll.BackColor = System.Drawing.Color.Silver
        Me.ButtonSelectAll.Image = CType(resources.GetObject("ButtonSelectAll.Image"), System.Drawing.Image)
        Me.ButtonSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSelectAll.Location = New System.Drawing.Point(8, 6)
        Me.ButtonSelectAll.Name = "ButtonSelectAll"
        Me.ButtonSelectAll.Size = New System.Drawing.Size(128, 28)
        Me.ButtonSelectAll.TabIndex = 16
        Me.ButtonSelectAll.Text = "&Выделить все"
        Me.ToolTipHelp.SetToolTip(Me.ButtonSelectAll, "Пометить все кадры  изделия")
        Me.ButtonSelectAll.UseVisualStyleBackColor = False
        '
        'ButtonEraseSelect
        '
        Me.ButtonEraseSelect.BackColor = System.Drawing.Color.Silver
        Me.ButtonEraseSelect.Image = CType(resources.GetObject("ButtonEraseSelect.Image"), System.Drawing.Image)
        Me.ButtonEraseSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonEraseSelect.Location = New System.Drawing.Point(142, 6)
        Me.ButtonEraseSelect.Name = "ButtonEraseSelect"
        Me.ButtonEraseSelect.Size = New System.Drawing.Size(128, 28)
        Me.ButtonEraseSelect.TabIndex = 17
        Me.ButtonEraseSelect.Text = "&Снять выделение"
        Me.ToolTipHelp.SetToolTip(Me.ButtonEraseSelect, "Снять выделение с прерванных кадров")
        Me.ButtonEraseSelect.UseVisualStyleBackColor = False
        '
        'TabControlAll
        '
        Me.TabControlAll.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.TabControlAll.Controls.Add(Me.TabPageCondition)
        Me.TabControlAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlAll.HotTrack = True
        Me.TabControlAll.ImageList = Me.ImageListCondition
        Me.TabControlAll.Location = New System.Drawing.Point(0, 0)
        Me.TabControlAll.Multiline = True
        Me.TabControlAll.Name = "TabControlAll"
        Me.TabControlAll.SelectedIndex = 0
        Me.TabControlAll.Size = New System.Drawing.Size(615, 176)
        Me.TabControlAll.TabIndex = 6
        Me.ToolTipHelp.SetToolTip(Me.TabControlAll, "Выбрать условие")
        '
        'TabPageCondition
        '
        Me.TabPageCondition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageCondition.Controls.Add(Me.TabControlCondition)
        Me.TabPageCondition.ImageIndex = 0
        Me.TabPageCondition.Location = New System.Drawing.Point(4, 26)
        Me.TabPageCondition.Name = "TabPageCondition"
        Me.TabPageCondition.Size = New System.Drawing.Size(607, 146)
        Me.TabPageCondition.TabIndex = 0
        Me.TabPageCondition.Text = "Условие 1"
        '
        'TabControlCondition
        '
        Me.TabControlCondition.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.TabControlCondition.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.TabControlCondition.Controls.Add(Me.TabPageTrigger)
        Me.TabControlCondition.Controls.Add(Me.TabPageTemplate)
        Me.TabControlCondition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlCondition.HotTrack = True
        Me.TabControlCondition.ImageList = Me.ImageListCondition
        Me.TabControlCondition.Location = New System.Drawing.Point(0, 0)
        Me.TabControlCondition.Multiline = True
        Me.TabControlCondition.Name = "TabControlCondition"
        Me.TabControlCondition.SelectedIndex = 0
        Me.TabControlCondition.Size = New System.Drawing.Size(603, 142)
        Me.TabControlCondition.TabIndex = 1
        Me.ToolTipHelp.SetToolTip(Me.TabControlCondition, "Задать тип условия")
        '
        'TabPageTrigger
        '
        Me.TabPageTrigger.ImageIndex = 2
        Me.TabPageTrigger.Location = New System.Drawing.Point(77, 4)
        Me.TabPageTrigger.Name = "TabPageTrigger"
        Me.TabPageTrigger.Size = New System.Drawing.Size(522, 134)
        Me.TabPageTrigger.TabIndex = 0
        Me.TabPageTrigger.Tag = "Триггер"
        Me.TabPageTrigger.Text = "Триггер"
        '
        'TabPageTemplate
        '
        Me.TabPageTemplate.ImageIndex = 3
        Me.TabPageTemplate.Location = New System.Drawing.Point(77, 4)
        Me.TabPageTemplate.Name = "TabPageTemplate"
        Me.TabPageTemplate.Size = New System.Drawing.Size(522, 134)
        Me.TabPageTemplate.TabIndex = 1
        Me.TabPageTemplate.Tag = "Шаблон"
        Me.TabPageTemplate.Text = "Шаблон"
        Me.TabPageTemplate.Visible = False
        '
        'ImageListCondition
        '
        Me.ImageListCondition.ImageStream = CType(resources.GetObject("ImageListCondition.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListCondition.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListCondition.Images.SetKeyName(0, "Filter0")
        Me.ImageListCondition.Images.SetKeyName(1, "Question1")
        Me.ImageListCondition.Images.SetKeyName(2, "Trigger2")
        Me.ImageListCondition.Images.SetKeyName(3, "Template3")
        '
        'DataGridFoundSnapshot
        '
        Me.DataGridFoundSnapshot.AllowUserToAddRows = False
        Me.DataGridFoundSnapshot.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGridFoundSnapshot.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridFoundSnapshot.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridFoundSnapshot.BackgroundColor = System.Drawing.Color.Lavender
        Me.DataGridFoundSnapshot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridFoundSnapshot.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridFoundSnapshot.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridFoundSnapshot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridFoundSnapshot.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridFoundSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridFoundSnapshot.GridColor = System.Drawing.Color.RoyalBlue
        Me.DataGridFoundSnapshot.Location = New System.Drawing.Point(0, 17)
        Me.DataGridFoundSnapshot.MultiSelect = False
        Me.DataGridFoundSnapshot.Name = "DataGridFoundSnapshot"
        Me.DataGridFoundSnapshot.ReadOnly = True
        Me.DataGridFoundSnapshot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridFoundSnapshot.Size = New System.Drawing.Size(705, 72)
        Me.DataGridFoundSnapshot.TabIndex = 39
        Me.ToolTipHelp.SetToolTip(Me.DataGridFoundSnapshot, "Щёлкни по строке для загрузки")
        '
        'ButtonFindChannelPattern
        '
        Me.ButtonFindChannelPattern.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonFindChannelPattern.Image = CType(resources.GetObject("ButtonFindChannelPattern.Image"), System.Drawing.Image)
        Me.ButtonFindChannelPattern.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonFindChannelPattern.Location = New System.Drawing.Point(0, 23)
        Me.ButtonFindChannelPattern.Name = "ButtonFindChannelPattern"
        Me.ButtonFindChannelPattern.Size = New System.Drawing.Size(94, 24)
        Me.ButtonFindChannelPattern.TabIndex = 103
        Me.ButtonFindChannelPattern.Text = "Поиск"
        Me.ButtonFindChannelPattern.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.ToolTipHelp.SetToolTip(Me.ButtonFindChannelPattern, "Быстрый поиск канала")
        Me.ButtonFindChannelPattern.UseVisualStyleBackColor = True
        '
        'ButtonFindChanneValue
        '
        Me.ButtonFindChanneValue.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonFindChanneValue.Image = CType(resources.GetObject("ButtonFindChanneValue.Image"), System.Drawing.Image)
        Me.ButtonFindChanneValue.Location = New System.Drawing.Point(138, 55)
        Me.ButtonFindChanneValue.Name = "ButtonFindChanneValue"
        Me.ButtonFindChanneValue.Size = New System.Drawing.Size(24, 24)
        Me.ButtonFindChanneValue.TabIndex = 104
        Me.ButtonFindChanneValue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipHelp.SetToolTip(Me.ButtonFindChanneValue, "Быстрый поиск канала")
        Me.ButtonFindChanneValue.UseVisualStyleBackColor = True
        '
        'PanelSetValue
        '
        Me.PanelSetValue.Controls.Add(Me.ButtonFindChanneValue)
        Me.PanelSetValue.Controls.Add(Me.TextBoxValue)
        Me.PanelSetValue.Controls.Add(Me.LabelSelectParam)
        Me.PanelSetValue.Controls.Add(Me.LabelAnd)
        Me.PanelSetValue.Controls.Add(Me.TextBoxStart)
        Me.PanelSetValue.Controls.Add(Me.ComboBoxEnumConditions)
        Me.PanelSetValue.Controls.Add(Me.ComboBoxValueParameters)
        Me.PanelSetValue.Controls.Add(Me.LabelSelectCondition)
        Me.PanelSetValue.Controls.Add(Me.TextBoxEnd)
        Me.PanelSetValue.Location = New System.Drawing.Point(655, 607)
        Me.PanelSetValue.Name = "PanelSetValue"
        Me.PanelSetValue.Size = New System.Drawing.Size(481, 97)
        Me.PanelSetValue.TabIndex = 7
        '
        'LabelAnd
        '
        Me.LabelAnd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LabelAnd.Location = New System.Drawing.Point(376, 56)
        Me.LabelAnd.Name = "LabelAnd"
        Me.LabelAnd.Size = New System.Drawing.Size(24, 16)
        Me.LabelAnd.TabIndex = 4
        Me.LabelAnd.Text = "и"
        Me.LabelAnd.Visible = False
        '
        'PanelTemplate
        '
        Me.PanelTemplate.Controls.Add(Me.TableLayoutPanel1)
        Me.PanelTemplate.Location = New System.Drawing.Point(742, 45)
        Me.PanelTemplate.Name = "PanelTemplate"
        Me.PanelTemplate.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelTemplate.Size = New System.Drawing.Size(572, 591)
        Me.PanelTemplate.TabIndex = 6
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PanelChannelPattern, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSelect, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideTimeWindow, 2, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueMiddleUp, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideTime, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueFinishDown, 5, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueFinishUp, 4, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueFinishDown, 5, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueFinishUp, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueMiddleDown, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSet, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericValueMax, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueMiddleUp, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueStartDown, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueStartUp, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueStartUp, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericSlideValueStartDown, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SlideValueMiddleDown, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GraphPatternWave, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.PanelNumericTime, 4, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.PanelNumericTimeWindow, 4, 5)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 177.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 571)
        Me.TableLayoutPanel1.TabIndex = 103
        '
        'PanelChannelPattern
        '
        Me.PanelChannelPattern.Controls.Add(Me.ComboBoxPatternParameters)
        Me.PanelChannelPattern.Controls.Add(Me.ButtonFindChannelPattern)
        Me.PanelChannelPattern.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChannelPattern.Location = New System.Drawing.Point(103, 3)
        Me.PanelChannelPattern.Name = "PanelChannelPattern"
        Me.PanelChannelPattern.Size = New System.Drawing.Size(94, 47)
        Me.PanelChannelPattern.TabIndex = 0
        '
        'LabelSelect
        '
        Me.LabelSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSelect.Location = New System.Drawing.Point(3, 0)
        Me.LabelSelect.Name = "LabelSelect"
        Me.LabelSelect.Size = New System.Drawing.Size(94, 53)
        Me.LabelSelect.TabIndex = 49
        Me.LabelSelect.Text = "Выбрать параметр для шаблона"
        Me.LabelSelect.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelSet
        '
        Me.LabelSet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSet.Location = New System.Drawing.Point(3, 53)
        Me.LabelSet.Name = "LabelSet"
        Me.LabelSet.Size = New System.Drawing.Size(94, 177)
        Me.LabelSet.TabIndex = 50
        Me.LabelSet.Text = "Установить максимальное значение сигнала"
        Me.LabelSet.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GraphPatternWave
        '
        Me.GraphPatternWave.Border = NationalInstruments.UI.Border.Raised
        Me.GraphPatternWave.Caption = "Создан шаблон для поиска сигнала"
        Me.TableLayoutPanel1.SetColumnSpan(Me.GraphPatternWave, 2)
        Me.GraphPatternWave.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XYCursorUpLeft, Me.XYCursorDownLeft, Me.XYCursorUpMiddle, Me.XYCursorDownMiddle, Me.XYCursorDownRight, Me.XYCursorUpRight})
        Me.GraphPatternWave.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GraphPatternWave.Location = New System.Drawing.Point(203, 263)
        Me.GraphPatternWave.Name = "GraphPatternWave"
        Me.GraphPatternWave.PlotAreaColor = System.Drawing.Color.White
        Me.GraphPatternWave.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.PlotUp, Me.PlotDown})
        Me.GraphPatternWave.Size = New System.Drawing.Size(146, 135)
        Me.GraphPatternWave.TabIndex = 48
        Me.GraphPatternWave.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis})
        Me.GraphPatternWave.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis})
        '
        'XYCursorUpLeft
        '
        Me.XYCursorUpLeft.Color = System.Drawing.Color.Red
        Me.XYCursorUpLeft.Plot = Me.PlotUp
        Me.XYCursorUpLeft.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorUpLeft.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        Me.XYCursorUpLeft.VerticalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        '
        'PlotUp
        '
        Me.PlotUp.BasePlot = Me.PlotDown
        Me.PlotUp.FillBase = NationalInstruments.UI.XYPlotFillBase.Plot
        Me.PlotUp.FillMode = NationalInstruments.UI.PlotFillMode.Fill
        Me.PlotUp.FillToBaseColor = System.Drawing.Color.LimeGreen
        Me.PlotUp.FillToBaseStyle = NationalInstruments.UI.FillStyle.VerticalGradient
        Me.PlotUp.LineColor = System.Drawing.Color.Blue
        Me.PlotUp.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.PlotUp.XAxis = Me.XAxis
        Me.PlotUp.YAxis = Me.YAxis
        '
        'PlotDown
        '
        Me.PlotDown.LineColor = System.Drawing.Color.Blue
        Me.PlotDown.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.PlotDown.XAxis = Me.XAxis
        Me.PlotDown.YAxis = Me.YAxis
        '
        'XAxis
        '
        Me.XAxis.Caption = "Время сек"
        Me.XAxis.MajorDivisions.GridColor = System.Drawing.Color.DodgerBlue
        Me.XAxis.MajorDivisions.GridVisible = True
        Me.XAxis.Mode = NationalInstruments.UI.AxisMode.Fixed
        '
        'YAxis
        '
        Me.YAxis.Caption = "Значение параметра"
        Me.YAxis.MajorDivisions.GridColor = System.Drawing.Color.DodgerBlue
        Me.YAxis.MajorDivisions.GridVisible = True
        Me.YAxis.MinorDivisions.GridColor = System.Drawing.Color.Chartreuse
        Me.YAxis.Mode = NationalInstruments.UI.AxisMode.Fixed
        '
        'XYCursorDownLeft
        '
        Me.XYCursorDownLeft.Color = System.Drawing.Color.Red
        Me.XYCursorDownLeft.Plot = Me.PlotUp
        Me.XYCursorDownLeft.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorDownLeft.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        Me.XYCursorDownLeft.VerticalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        '
        'XYCursorUpMiddle
        '
        Me.XYCursorUpMiddle.Color = System.Drawing.Color.Red
        Me.XYCursorUpMiddle.Plot = Me.PlotUp
        Me.XYCursorUpMiddle.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorUpMiddle.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        '
        'XYCursorDownMiddle
        '
        Me.XYCursorDownMiddle.Color = System.Drawing.Color.Red
        Me.XYCursorDownMiddle.Plot = Me.PlotUp
        Me.XYCursorDownMiddle.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorDownMiddle.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        '
        'XYCursorDownRight
        '
        Me.XYCursorDownRight.Color = System.Drawing.Color.Red
        Me.XYCursorDownRight.Plot = Me.PlotUp
        Me.XYCursorDownRight.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorDownRight.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        Me.XYCursorDownRight.VerticalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        '
        'XYCursorUpRight
        '
        Me.XYCursorUpRight.Color = System.Drawing.Color.Red
        Me.XYCursorUpRight.Plot = Me.PlotUp
        Me.XYCursorUpRight.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle
        Me.XYCursorUpRight.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        Me.XYCursorUpRight.VerticalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        '
        'PanelNumericTime
        '
        Me.PanelNumericTime.Controls.Add(Me.NumericTime)
        Me.PanelNumericTime.Location = New System.Drawing.Point(355, 404)
        Me.PanelNumericTime.Name = "PanelNumericTime"
        Me.PanelNumericTime.Size = New System.Drawing.Size(94, 79)
        Me.PanelNumericTime.TabIndex = 103
        '
        'PanelNumericTimeWindow
        '
        Me.PanelNumericTimeWindow.Controls.Add(Me.NumericTimeWindow)
        Me.PanelNumericTimeWindow.Location = New System.Drawing.Point(355, 489)
        Me.PanelNumericTimeWindow.Name = "PanelNumericTimeWindow"
        Me.PanelNumericTimeWindow.Size = New System.Drawing.Size(94, 79)
        Me.PanelNumericTimeWindow.TabIndex = 104
        '
        'PanelFoundSnapshot
        '
        Me.PanelFoundSnapshot.Controls.Add(Me.DataGridFoundSnapshot)
        Me.PanelFoundSnapshot.Controls.Add(Me.LabelDescriptionStage)
        Me.PanelFoundSnapshot.Controls.Add(Me.PanelNavigation)
        Me.PanelFoundSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelFoundSnapshot.Location = New System.Drawing.Point(0, 0)
        Me.PanelFoundSnapshot.Name = "PanelFoundSnapshot"
        Me.PanelFoundSnapshot.Size = New System.Drawing.Size(705, 129)
        Me.PanelFoundSnapshot.TabIndex = 19
        '
        'LabelDescriptionStage
        '
        Me.LabelDescriptionStage.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelDescriptionStage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelDescriptionStage.ForeColor = System.Drawing.Color.Blue
        Me.LabelDescriptionStage.Location = New System.Drawing.Point(0, 0)
        Me.LabelDescriptionStage.Name = "LabelDescriptionStage"
        Me.LabelDescriptionStage.Size = New System.Drawing.Size(705, 17)
        Me.LabelDescriptionStage.TabIndex = 38
        Me.LabelDescriptionStage.Text = "Снимки удовлетворяющие условиям поиска"
        Me.LabelDescriptionStage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PanelNavigation
        '
        Me.PanelNavigation.Controls.Add(Me.LabelPosition)
        Me.PanelNavigation.Controls.Add(Me.ButtonNext)
        Me.PanelNavigation.Controls.Add(Me.ButtonPrevious)
        Me.PanelNavigation.Controls.Add(Me.ButtonBackToStep2)
        Me.PanelNavigation.Controls.Add(Me.ButtonExit)
        Me.PanelNavigation.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelNavigation.Location = New System.Drawing.Point(0, 89)
        Me.PanelNavigation.Name = "PanelNavigation"
        Me.PanelNavigation.Size = New System.Drawing.Size(705, 40)
        Me.PanelNavigation.TabIndex = 4
        '
        'LabelPosition
        '
        Me.LabelPosition.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LabelPosition.BackColor = System.Drawing.Color.White
        Me.LabelPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPosition.Location = New System.Drawing.Point(289, 8)
        Me.LabelPosition.Name = "LabelPosition"
        Me.LabelPosition.Size = New System.Drawing.Size(127, 24)
        Me.LabelPosition.TabIndex = 8
        Me.LabelPosition.Text = "Запись 0 из 0"
        Me.LabelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonNext
        '
        Me.ButtonNext.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonNext.BackColor = System.Drawing.Color.Silver
        Me.ButtonNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonNext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonNext.Image = CType(resources.GetObject("ButtonNext.Image"), System.Drawing.Image)
        Me.ButtonNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonNext.Location = New System.Drawing.Point(424, 6)
        Me.ButtonNext.Name = "ButtonNext"
        Me.ButtonNext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonNext.Size = New System.Drawing.Size(152, 28)
        Me.ButtonNext.TabIndex = 5
        Me.ButtonNext.Tag = "103"
        Me.ButtonNext.Text = "&Вперед"
        Me.ButtonNext.UseVisualStyleBackColor = False
        '
        'ButtonPrevious
        '
        Me.ButtonPrevious.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonPrevious.BackColor = System.Drawing.Color.Silver
        Me.ButtonPrevious.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonPrevious.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonPrevious.Image = CType(resources.GetObject("ButtonPrevious.Image"), System.Drawing.Image)
        Me.ButtonPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonPrevious.Location = New System.Drawing.Point(128, 6)
        Me.ButtonPrevious.Name = "ButtonPrevious"
        Me.ButtonPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonPrevious.Size = New System.Drawing.Size(154, 28)
        Me.ButtonPrevious.TabIndex = 6
        Me.ButtonPrevious.Tag = "102"
        Me.ButtonPrevious.Text = "&Назад"
        Me.ButtonPrevious.UseVisualStyleBackColor = False
        '
        'PanelStep2
        '
        Me.PanelStep2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelStep2.Controls.Add(Me.TabControlAll)
        Me.PanelStep2.Controls.Add(Me.PanelButtonsStep2)
        Me.PanelStep2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelStep2.Location = New System.Drawing.Point(0, 0)
        Me.PanelStep2.Name = "PanelStep2"
        Me.PanelStep2.Size = New System.Drawing.Size(619, 220)
        Me.PanelStep2.TabIndex = 14
        '
        'PanelButtonsStep2
        '
        Me.PanelButtonsStep2.Controls.Add(Me.ButtonBackToStep1)
        Me.PanelButtonsStep2.Controls.Add(Me.ButtonCancel)
        Me.PanelButtonsStep2.Controls.Add(Me.ButtonFind)
        Me.PanelButtonsStep2.Controls.Add(Me.ButtonDelete)
        Me.PanelButtonsStep2.Controls.Add(Me.ButtonAdd)
        Me.PanelButtonsStep2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelButtonsStep2.Location = New System.Drawing.Point(0, 176)
        Me.PanelButtonsStep2.Name = "PanelButtonsStep2"
        Me.PanelButtonsStep2.Size = New System.Drawing.Size(615, 40)
        Me.PanelButtonsStep2.TabIndex = 5
        '
        'ImageListSteps
        '
        Me.ImageListSteps.ImageStream = CType(resources.GetObject("ImageListSteps.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListSteps.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListSteps.Images.SetKeyName(0, "Step1")
        Me.ImageListSteps.Images.SetKeyName(1, "Step2")
        Me.ImageListSteps.Images.SetKeyName(2, "Step3")
        '
        'ImageListSetting
        '
        Me.ImageListSetting.ImageStream = CType(resources.GetObject("ImageListSetting.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListSetting.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListSetting.Images.SetKeyName(0, "Select0")
        Me.ImageListSetting.Images.SetKeyName(1, "Root1")
        Me.ImageListSetting.Images.SetKeyName(2, "Conditions2")
        Me.ImageListSetting.Images.SetKeyName(3, "Term3")
        Me.ImageListSetting.Images.SetKeyName(4, "StartUp4")
        Me.ImageListSetting.Images.SetKeyName(5, "StartDown5")
        Me.ImageListSetting.Images.SetKeyName(6, "BendUp6")
        Me.ImageListSetting.Images.SetKeyName(7, "BendDown7")
        Me.ImageListSetting.Images.SetKeyName(8, "FinishUp8")
        Me.ImageListSetting.Images.SetKeyName(9, "FinishDown9")
        Me.ImageListSetting.Images.SetKeyName(10, "TimeBend10")
        Me.ImageListSetting.Images.SetKeyName(11, "WidthWindow11")
        Me.ImageListSetting.Images.SetKeyName(12, "BetweenUpTrigger12")
        Me.ImageListSetting.Images.SetKeyName(13, "BetweenDownTrigger13")
        Me.ImageListSetting.Images.SetKeyName(14, "MaxLimit14")
        Me.ImageListSetting.Images.SetKeyName(15, "ParameterTemplate15")
        Me.ImageListSetting.Images.SetKeyName(16, "ConditionalTrigger16")
        Me.ImageListSetting.Images.SetKeyName(17, "EqualTrigger17")
        Me.ImageListSetting.Images.SetKeyName(18, "ParameterTrigger18")
        Me.ImageListSetting.Images.SetKeyName(19, "")
        Me.ImageListSetting.Images.SetKeyName(20, "")
        Me.ImageListSetting.Images.SetKeyName(21, "")
        Me.ImageListSetting.Images.SetKeyName(22, "")
        Me.ImageListSetting.Images.SetKeyName(23, "")
        Me.ImageListSetting.Images.SetKeyName(24, "Equal")
        Me.ImageListSetting.Images.SetKeyName(25, "GreaterOrEqual")
        Me.ImageListSetting.Images.SetKeyName(26, "LessOrEqual")
        Me.ImageListSetting.Images.SetKeyName(27, "NotEqual")
        Me.ImageListSetting.Images.SetKeyName(28, "Greater")
        Me.ImageListSetting.Images.SetKeyName(29, "Less")
        Me.ImageListSetting.Images.SetKeyName(30, "")
        Me.ImageListSetting.Images.SetKeyName(31, "Between")
        Me.ImageListSetting.Images.SetKeyName(32, "GreaterThan")
        Me.ImageListSetting.Images.SetKeyName(33, "Question")
        Me.ImageListSetting.Images.SetKeyName(34, "AddCalculatedField")
        '
        'StatusStripMessage
        '
        Me.StatusStripMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSLabelMessage, Me.TSProgressBar})
        Me.StatusStripMessage.Location = New System.Drawing.Point(0, 707)
        Me.StatusStripMessage.Name = "StatusStripMessage"
        Me.StatusStripMessage.Size = New System.Drawing.Size(1326, 22)
        Me.StatusStripMessage.TabIndex = 22
        Me.StatusStripMessage.Text = "StatusStrip1"
        '
        'TSLabelMessage
        '
        Me.TSLabelMessage.AutoSize = False
        Me.TSLabelMessage.Image = CType(resources.GetObject("TSLabelMessage.Image"), System.Drawing.Image)
        Me.TSLabelMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TSLabelMessage.Name = "TSLabelMessage"
        Me.TSLabelMessage.Size = New System.Drawing.Size(1009, 17)
        Me.TSLabelMessage.Spring = True
        Me.TSLabelMessage.Text = "Готов"
        Me.TSLabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TSProgressBar
        '
        Me.TSProgressBar.Name = "TSProgressBar"
        Me.TSProgressBar.Size = New System.Drawing.Size(300, 16)
        '
        'SplitContainerCondition
        '
        Me.SplitContainerCondition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerCondition.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainerCondition.IsSplitterFixed = True
        Me.SplitContainerCondition.Location = New System.Drawing.Point(17, 172)
        Me.SplitContainerCondition.Name = "SplitContainerCondition"
        '
        'SplitContainerCondition.Panel1
        '
        Me.SplitContainerCondition.Panel1.Controls.Add(Me.XmlTreeView)
        Me.SplitContainerCondition.Panel1.Controls.Add(Me.LabelAllSetting)
        Me.SplitContainerCondition.Panel1.Controls.Add(Me.PanelConfiguration)
        '
        'SplitContainerCondition.Panel2
        '
        Me.SplitContainerCondition.Panel2.Controls.Add(Me.PanelStep2)
        Me.SplitContainerCondition.Size = New System.Drawing.Size(933, 224)
        Me.SplitContainerCondition.SplitterDistance = 306
        Me.SplitContainerCondition.TabIndex = 23
        '
        'XmlTreeView
        '
        Me.XmlTreeView.AllowDrop = True
        Me.XmlTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XmlTreeView.ForeColor = System.Drawing.SystemColors.WindowText
        Me.XmlTreeView.HideSelection = False
        Me.XmlTreeView.HotTracking = True
        Me.XmlTreeView.ImageIndex = 0
        Me.XmlTreeView.ImageList = Me.ImageListSetting
        Me.XmlTreeView.Indent = 19
        Me.XmlTreeView.ItemHeight = 16
        Me.XmlTreeView.Location = New System.Drawing.Point(0, 20)
        Me.XmlTreeView.Name = "XmlTreeView"
        Me.XmlTreeView.SelectedImageIndex = 1
        Me.XmlTreeView.Size = New System.Drawing.Size(302, 128)
        Me.XmlTreeView.TabIndex = 17
        Me.XmlTreeView.Text = "treeView1"
        '
        'LabelAllSetting
        '
        Me.LabelAllSetting.BackColor = System.Drawing.Color.Lavender
        Me.LabelAllSetting.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelAllSetting.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAllSetting.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelAllSetting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAllSetting.Location = New System.Drawing.Point(0, 0)
        Me.LabelAllSetting.Name = "LabelAllSetting"
        Me.LabelAllSetting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAllSetting.Size = New System.Drawing.Size(302, 20)
        Me.LabelAllSetting.TabIndex = 16
        Me.LabelAllSetting.Text = "Доступные условия в базе данных"
        Me.LabelAllSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelConfiguration
        '
        Me.PanelConfiguration.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelConfiguration.Controls.Add(Me.TableLayoutPanelConfiguration)
        Me.PanelConfiguration.Controls.Add(Me.TextBoxNameConfiguration)
        Me.PanelConfiguration.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelConfiguration.Location = New System.Drawing.Point(0, 148)
        Me.PanelConfiguration.Name = "PanelConfiguration"
        Me.PanelConfiguration.Size = New System.Drawing.Size(302, 72)
        Me.PanelConfiguration.TabIndex = 12
        '
        'TableLayoutPanelConfiguration
        '
        Me.TableLayoutPanelConfiguration.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanelConfiguration.ColumnCount = 3
        Me.TableLayoutPanelConfiguration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelConfiguration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelConfiguration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelConfiguration.Controls.Add(Me.ButtonLoadCondition, 0, 0)
        Me.TableLayoutPanelConfiguration.Controls.Add(Me.ButtonSaveCondition, 1, 0)
        Me.TableLayoutPanelConfiguration.Controls.Add(Me.ButtonDeleteCondition, 2, 0)
        Me.TableLayoutPanelConfiguration.Location = New System.Drawing.Point(8, 34)
        Me.TableLayoutPanelConfiguration.Name = "TableLayoutPanelConfiguration"
        Me.TableLayoutPanelConfiguration.RowCount = 1
        Me.TableLayoutPanelConfiguration.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelConfiguration.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanelConfiguration.Size = New System.Drawing.Size(282, 36)
        Me.TableLayoutPanelConfiguration.TabIndex = 17
        '
        'LabelSelectFolder
        '
        Me.LabelSelectFolder.BackColor = System.Drawing.Color.Lavender
        Me.LabelSelectFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelSelectFolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelSelectFolder.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectFolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSelectFolder.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectFolder.Name = "LabelSelectFolder"
        Me.LabelSelectFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelSelectFolder.Size = New System.Drawing.Size(236, 20)
        Me.LabelSelectFolder.TabIndex = 7
        Me.LabelSelectFolder.Text = "Выбрать изделие"
        Me.LabelSelectFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TreeViewAllSnapshot
        '
        Me.TreeViewAllSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeViewAllSnapshot.FullRowSelect = True
        Me.TreeViewAllSnapshot.HideSelection = False
        Me.TreeViewAllSnapshot.ImageIndex = 4
        Me.TreeViewAllSnapshot.ImageList = Me.ImageListFolder
        Me.TreeViewAllSnapshot.Location = New System.Drawing.Point(0, 20)
        Me.TreeViewAllSnapshot.Name = "TreeViewAllSnapshot"
        Me.TreeViewAllSnapshot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TreeViewAllSnapshot.SelectedImageIndex = 4
        Me.TreeViewAllSnapshot.Size = New System.Drawing.Size(236, 129)
        Me.TreeViewAllSnapshot.TabIndex = 12
        '
        'LabbelSelectSnapshot
        '
        Me.LabbelSelectSnapshot.BackColor = System.Drawing.Color.Lavender
        Me.LabbelSelectSnapshot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabbelSelectSnapshot.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabbelSelectSnapshot.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabbelSelectSnapshot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabbelSelectSnapshot.Location = New System.Drawing.Point(0, 0)
        Me.LabbelSelectSnapshot.Name = "LabbelSelectSnapshot"
        Me.LabbelSelectSnapshot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabbelSelectSnapshot.Size = New System.Drawing.Size(689, 20)
        Me.LabbelSelectSnapshot.TabIndex = 8
        Me.LabbelSelectSnapshot.Text = "Выбрать с помощью <Shift> снимки для поиска"
        Me.LabbelSelectSnapshot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainerEngine
        '
        Me.SplitContainerEngine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerEngine.IsSplitterFixed = True
        Me.SplitContainerEngine.Location = New System.Drawing.Point(15, 13)
        Me.SplitContainerEngine.Name = "SplitContainerEngine"
        '
        'SplitContainerEngine.Panel1
        '
        Me.SplitContainerEngine.Panel1.Controls.Add(Me.TreeViewAllSnapshot)
        Me.SplitContainerEngine.Panel1.Controls.Add(Me.LabelSelectFolder)
        '
        'SplitContainerEngine.Panel2
        '
        Me.SplitContainerEngine.Panel2.Controls.Add(Me.ListViewAllSnapshot)
        Me.SplitContainerEngine.Panel2.Controls.Add(Me.PanelSelectRecord)
        Me.SplitContainerEngine.Panel2.Controls.Add(Me.LabbelSelectSnapshot)
        Me.SplitContainerEngine.Size = New System.Drawing.Size(937, 153)
        Me.SplitContainerEngine.SplitterDistance = 240
        Me.SplitContainerEngine.TabIndex = 6
        '
        'PanelSelectRecord
        '
        Me.PanelSelectRecord.Controls.Add(Me.ButtonEraseSelect)
        Me.PanelSelectRecord.Controls.Add(Me.ButtonSelectAll)
        Me.PanelSelectRecord.Controls.Add(Me.ButtonForwardToStep2)
        Me.PanelSelectRecord.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelSelectRecord.Location = New System.Drawing.Point(0, 109)
        Me.PanelSelectRecord.Name = "PanelSelectRecord"
        Me.PanelSelectRecord.Size = New System.Drawing.Size(689, 40)
        Me.PanelSelectRecord.TabIndex = 14
        '
        'SplitContainerStep3
        '
        Me.SplitContainerStep3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerStep3.Location = New System.Drawing.Point(12, 42)
        Me.SplitContainerStep3.Name = "SplitContainerStep3"
        Me.SplitContainerStep3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerStep3.Panel1
        '
        Me.SplitContainerStep3.Panel1.Controls.Add(Me.SplitContainerCondition)
        Me.SplitContainerStep3.Panel1.Controls.Add(Me.SplitContainerEngine)
        '
        'SplitContainerStep3.Panel2
        '
        Me.SplitContainerStep3.Panel2.Controls.Add(Me.PanelFoundSnapshot)
        Me.SplitContainerStep3.Size = New System.Drawing.Size(709, 553)
        Me.SplitContainerStep3.SplitterDistance = 416
        Me.SplitContainerStep3.TabIndex = 23
        '
        'ToolStripSteps
        '
        Me.ToolStripSteps.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStripSteps.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSButtonSourceStep1, Me.TSButtonConditionStep2, Me.TSButtonResultStep3, Me.ToolStripSeparator1, Me.LabelCaptionSteps})
        Me.ToolStripSteps.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripSteps.Name = "ToolStripSteps"
        Me.ToolStripSteps.Size = New System.Drawing.Size(1326, 39)
        Me.ToolStripSteps.TabIndex = 24
        Me.ToolStripSteps.Text = "ToolStrip1"
        '
        'TSButtonSourceStep1
        '
        Me.TSButtonSourceStep1.Image = CType(resources.GetObject("TSButtonSourceStep1.Image"), System.Drawing.Image)
        Me.TSButtonSourceStep1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonSourceStep1.Name = "TSButtonSourceStep1"
        Me.TSButtonSourceStep1.Size = New System.Drawing.Size(97, 36)
        Me.TSButtonSourceStep1.Text = "Источник"
        Me.TSButtonSourceStep1.ToolTipText = "Шаг 1"
        '
        'TSButtonConditionStep2
        '
        Me.TSButtonConditionStep2.Image = CType(resources.GetObject("TSButtonConditionStep2.Image"), System.Drawing.Image)
        Me.TSButtonConditionStep2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonConditionStep2.Name = "TSButtonConditionStep2"
        Me.TSButtonConditionStep2.Size = New System.Drawing.Size(89, 36)
        Me.TSButtonConditionStep2.Text = "Условия"
        Me.TSButtonConditionStep2.ToolTipText = "Шаг 2"
        '
        'TSButtonResultStep3
        '
        Me.TSButtonResultStep3.Image = CType(resources.GetObject("TSButtonResultStep3.Image"), System.Drawing.Image)
        Me.TSButtonResultStep3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonResultStep3.Name = "TSButtonResultStep3"
        Me.TSButtonResultStep3.Size = New System.Drawing.Size(96, 36)
        Me.TSButtonResultStep3.Text = "Результат"
        Me.TSButtonResultStep3.ToolTipText = "Шаг 3"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'LabelCaptionSteps
        '
        Me.LabelCaptionSteps.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelCaptionSteps.ForeColor = System.Drawing.Color.Blue
        Me.LabelCaptionSteps.Name = "LabelCaptionSteps"
        Me.LabelCaptionSteps.Size = New System.Drawing.Size(400, 36)
        Me.LabelCaptionSteps.Text = "Выбрать снимки изделия, где будет произведен поиск"
        '
        'ImageListChannel
        '
        Me.ImageListChannel.ImageStream = CType(resources.GetObject("ImageListChannel.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListChannel.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListChannel.Images.SetKeyName(0, "Rotation0")
        Me.ImageListChannel.Images.SetKeyName(1, "Discrete1")
        Me.ImageListChannel.Images.SetKeyName(2, "Evacuation2")
        Me.ImageListChannel.Images.SetKeyName(3, "Temperature3")
        Me.ImageListChannel.Images.SetKeyName(4, "Petrol4")
        Me.ImageListChannel.Images.SetKeyName(5, "Vibration5")
        Me.ImageListChannel.Images.SetKeyName(6, "Current6")
        Me.ImageListChannel.Images.SetKeyName(7, "Presure7")
        Me.ImageListChannel.Images.SetKeyName(8, "Traction8")
        Me.ImageListChannel.Images.SetKeyName(9, "SliderVertical9")
        Me.ImageListChannel.Images.SetKeyName(10, "SliderHorizontal10")
        Me.ImageListChannel.Images.SetKeyName(11, "SliderVertical11")
        Me.ImageListChannel.Images.SetKeyName(12, "Close12")
        '
        'FormConditionFind
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonExit
        Me.ClientSize = New System.Drawing.Size(1326, 729)
        Me.Controls.Add(Me.PanelTemplate)
        Me.Controls.Add(Me.ToolStripSteps)
        Me.Controls.Add(Me.SplitContainerStep3)
        Me.Controls.Add(Me.PanelSetValue)
        Me.Controls.Add(Me.StatusStripMessage)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1024, 768)
        Me.Name = "FormConditionFind"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Поиск кадров по условиям"
        CType(Me.NumericTimeWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideTimeWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueFinishUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueFinishUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericValueMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueFinishDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueFinishDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueStartUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueStartUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueStartDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueStartDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueMiddleDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueMiddleDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSlideValueMiddleUp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideValueMiddleUp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlAll.ResumeLayout(False)
        Me.TabPageCondition.ResumeLayout(False)
        Me.TabControlCondition.ResumeLayout(False)
        CType(Me.DataGridFoundSnapshot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSetValue.ResumeLayout(False)
        Me.PanelSetValue.PerformLayout()
        Me.PanelTemplate.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.PanelChannelPattern.ResumeLayout(False)
        CType(Me.GraphPatternWave, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorUpLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorDownLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorUpMiddle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorDownMiddle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorDownRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursorUpRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelNumericTime.ResumeLayout(False)
        Me.PanelNumericTimeWindow.ResumeLayout(False)
        Me.PanelFoundSnapshot.ResumeLayout(False)
        Me.PanelNavigation.ResumeLayout(False)
        Me.PanelStep2.ResumeLayout(False)
        Me.PanelButtonsStep2.ResumeLayout(False)
        Me.StatusStripMessage.ResumeLayout(False)
        Me.StatusStripMessage.PerformLayout()
        Me.SplitContainerCondition.Panel1.ResumeLayout(False)
        Me.SplitContainerCondition.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerCondition, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerCondition.ResumeLayout(False)
        Me.PanelConfiguration.ResumeLayout(False)
        Me.PanelConfiguration.PerformLayout()
        Me.TableLayoutPanelConfiguration.ResumeLayout(False)
        Me.SplitContainerEngine.Panel1.ResumeLayout(False)
        Me.SplitContainerEngine.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerEngine, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerEngine.ResumeLayout(False)
        Me.PanelSelectRecord.ResumeLayout(False)
        Me.SplitContainerStep3.Panel1.ResumeLayout(False)
        Me.SplitContainerStep3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerStep3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerStep3.ResumeLayout(False)
        Me.ToolStripSteps.ResumeLayout(False)
        Me.ToolStripSteps.PerformLayout()
        CType(Me.BindingSourceFoundedSnapshotDataTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelSelectParam As System.Windows.Forms.Label
    Friend WithEvents ComboBoxValueParameters As System.Windows.Forms.ComboBox
    Friend WithEvents ToolTipHelp As System.Windows.Forms.ToolTip
    Friend WithEvents LabelSelectCondition As System.Windows.Forms.Label
    Friend WithEvents ButtonBackToStep2 As System.Windows.Forms.Button
    Friend WithEvents ImageListFolder As System.Windows.Forms.ImageList
    Friend WithEvents ButtonExit As System.Windows.Forms.Button
    Friend WithEvents TextBoxValue As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxStart As System.Windows.Forms.TextBox
    Friend WithEvents ComboBoxEnumConditions As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxEnd As System.Windows.Forms.TextBox
    Friend WithEvents ComboBoxPatternParameters As System.Windows.Forms.ComboBox
    Friend WithEvents NumericTimeWindow As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideTimeWindow As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericTime As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideTime As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericSlideValueFinishUp As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueFinishUp As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericValueMax As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents NumericSlideValueFinishDown As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueFinishDown As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericSlideValueStartUp As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueStartUp As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericSlideValueStartDown As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueStartDown As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericSlideValueMiddleDown As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueMiddleDown As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents NumericSlideValueMiddleUp As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SlideValueMiddleUp As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents ImageListCondition As System.Windows.Forms.ImageList
    Friend WithEvents PanelSetValue As System.Windows.Forms.Panel
    Friend WithEvents LabelAnd As System.Windows.Forms.Label
    Friend WithEvents PanelTemplate As System.Windows.Forms.Panel
    Friend WithEvents GraphPatternWave As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents XYCursorUpLeft As NationalInstruments.UI.XYCursor
    Friend WithEvents PlotUp As NationalInstruments.UI.WaveformPlot
    Friend WithEvents PlotDown As NationalInstruments.UI.WaveformPlot
    Friend WithEvents XAxis As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis As NationalInstruments.UI.YAxis
    Friend WithEvents XYCursorDownLeft As NationalInstruments.UI.XYCursor
    Friend WithEvents XYCursorUpMiddle As NationalInstruments.UI.XYCursor
    Friend WithEvents XYCursorDownMiddle As NationalInstruments.UI.XYCursor
    Friend WithEvents XYCursorDownRight As NationalInstruments.UI.XYCursor
    Friend WithEvents XYCursorUpRight As NationalInstruments.UI.XYCursor
    Friend WithEvents LabelSelect As System.Windows.Forms.Label
    Friend WithEvents LabelSet As System.Windows.Forms.Label
    Friend WithEvents PanelFoundSnapshot As System.Windows.Forms.Panel
    Friend WithEvents PanelNavigation As System.Windows.Forms.Panel
    Friend WithEvents LabelPosition As System.Windows.Forms.Label
    Public WithEvents ButtonNext As System.Windows.Forms.Button
    Public WithEvents ButtonPrevious As System.Windows.Forms.Button
    Friend WithEvents PanelStep2 As System.Windows.Forms.Panel
    Friend WithEvents ImageListSteps As System.Windows.Forms.ImageList
    Friend WithEvents ImageListSetting As System.Windows.Forms.ImageList
    Friend WithEvents StatusStripMessage As StatusStrip
    Friend WithEvents TSLabelMessage As ToolStripStatusLabel
    Friend WithEvents TSProgressBar As ToolStripProgressBar
    Friend WithEvents SplitContainerCondition As SplitContainer
    Protected WithEvents XmlTreeView As TreeView
    Public WithEvents LabelAllSetting As Label
    Friend WithEvents PanelConfiguration As Panel
    Friend WithEvents ButtonDeleteCondition As Button
    Friend WithEvents ButtonLoadCondition As Button
    Friend WithEvents ButtonSaveCondition As Button
    Friend WithEvents TextBoxNameConfiguration As TextBox
    Friend WithEvents TreeViewAllSnapshot As TreeView
    Public WithEvents LabelSelectFolder As Label
    Friend WithEvents ListViewAllSnapshot As ListView
    Public WithEvents LabbelSelectSnapshot As Label
    Friend WithEvents SplitContainerEngine As SplitContainer
    Friend WithEvents PanelButtonsStep2 As Panel
    Friend WithEvents ButtonBackToStep1 As Button
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents ButtonFind As Button
    Friend WithEvents ButtonDelete As Button
    Friend WithEvents ButtonAdd As Button
    Friend WithEvents SplitContainerStep3 As SplitContainer
    Friend WithEvents ToolStripSteps As ToolStrip
    Friend WithEvents TSButtonSourceStep1 As ToolStripButton
    Friend WithEvents TSButtonConditionStep2 As ToolStripButton
    Friend WithEvents TSButtonResultStep3 As ToolStripButton
    Friend WithEvents PanelSelectRecord As Panel
    Friend WithEvents ButtonEraseSelect As Button
    Friend WithEvents ButtonSelectAll As Button
    Friend WithEvents ButtonForwardToStep2 As Button
    Friend WithEvents LabelCaptionSteps As ToolStripLabel
    Friend WithEvents TabControlAll As TabControl
    Friend WithEvents TabPageCondition As TabPage
    Friend WithEvents TabControlCondition As TabControl
    Friend WithEvents TabPageTrigger As TabPage
    Friend WithEvents TabPageTemplate As TabPage
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents TableLayoutPanelConfiguration As TableLayoutPanel
    Friend WithEvents ImageListChannel As ImageList
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents LabelDescriptionStage As Label
    Friend WithEvents DataGridFoundSnapshot As DataGridView
    Friend WithEvents BindingSourceFoundedSnapshotDataTable As BindingSource
    Friend WithEvents PanelNumericTime As Panel
    Friend WithEvents PanelNumericTimeWindow As Panel
    Friend WithEvents PanelChannelPattern As Panel
    Friend WithEvents ButtonFindChannelPattern As Button
    Friend WithEvents ButtonFindChanneValue As Button
End Class
