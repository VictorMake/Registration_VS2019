<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEditiorGraphByParameter
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormEditiorGraphByParameter))
        Me.ToolTipForm = New System.Windows.Forms.ToolTip(Me.components)
        Me.TextBoxNameGraph = New System.Windows.Forms.TextBox()
        Me.ComboBoxAxisXFormulaParamaters = New System.Windows.Forms.ComboBox()
        Me.ComboBoxAxisYFormulaParamaters = New System.Windows.Forms.ComboBox()
        Me.ButtonTestFormulaX = New System.Windows.Forms.Button()
        Me.ButtonTestFormulaY = New System.Windows.Forms.Button()
        Me.ButtonAddGraph = New System.Windows.Forms.Button()
        Me.ButtonDeleteGraph = New System.Windows.Forms.Button()
        Me.ButtonLoadGraph = New System.Windows.Forms.Button()
        Me.ButtonSaveGraph = New System.Windows.Forms.Button()
        Me.ButtonAddNewLimitationGraph = New System.Windows.Forms.Button()
        Me.ButtonDeleteSelectedLimitationGraph = New System.Windows.Forms.Button()
        Me.ButtonEditLimitationGraph = New System.Windows.Forms.Button()
        Me.ButtonFindChannelAxisXFormula = New System.Windows.Forms.Button()
        Me.ButtonFindChannelAxisYFormula = New System.Windows.Forms.Button()
        Me.ImageListExplorer = New System.Windows.Forms.ImageList(Me.components)
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.MenuStripForm = New System.Windows.Forms.MenuStrip()
        Me.TSMenuItemGraph = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemPrintGraph = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemSaveGraf = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.ToolStripContainerForm = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainerForm = New System.Windows.Forms.SplitContainer()
        Me.XmlTreeView = New System.Windows.Forms.TreeView()
        Me.LabelAvailableLimitationGrap = New System.Windows.Forms.Label()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelEditGraph = New System.Windows.Forms.Label()
        Me.SplitContainerGraphParam = New System.Windows.Forms.SplitContainer()
        Me.ScatterGraphParam = New NationalInstruments.UI.WindowsForms.ScatterGraph()
        Me.ScatterPlot1 = New NationalInstruments.UI.ScatterPlot()
        Me.XAxis1 = New NationalInstruments.UI.XAxis()
        Me.YAxis1 = New NationalInstruments.UI.YAxis()
        Me.TabControlGraphParam = New System.Windows.Forms.TabControl()
        Me.TabPageEditAxisX = New System.Windows.Forms.TabPage()
        Me.SplitContainerAxisX = New System.Windows.Forms.SplitContainer()
        Me.CheckBoxAxisXParameterReduce = New System.Windows.Forms.CheckBox()
        Me.LabelSelectParameterX = New System.Windows.Forms.Label()
        Me.TableLayoutPanelAxisX = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridViewAxisXARG = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumnARGX = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewComboBoxColumnNameX = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.LabelArgX = New System.Windows.Forms.Label()
        Me.TextBoxMathAxisX = New System.Windows.Forms.TextBox()
        Me.LabelMathX = New System.Windows.Forms.Label()
        Me.PanelToolX = New System.Windows.Forms.Panel()
        Me.LabelForX = New System.Windows.Forms.Label()
        Me.PropertyEditorDefaultXAxis = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.LabelAxisXRange = New System.Windows.Forms.Label()
        Me.RadioButtonAxisXOneParameter = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAxisXisFormula = New System.Windows.Forms.RadioButton()
        Me.TabPageEditAxisY = New System.Windows.Forms.TabPage()
        Me.SplitContainerAxisY = New System.Windows.Forms.SplitContainer()
        Me.CheckBoxAxisYParameterReduce = New System.Windows.Forms.CheckBox()
        Me.LabelSelectParameterY = New System.Windows.Forms.Label()
        Me.TableLayoutPanelAxisY = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridViewAxisYARG = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumnARGY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewComboBoxColumnNameY = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.LabelArgY = New System.Windows.Forms.Label()
        Me.TextBoxMathAxisY = New System.Windows.Forms.TextBox()
        Me.LabelMathY = New System.Windows.Forms.Label()
        Me.PanelToolY = New System.Windows.Forms.Panel()
        Me.LabelForY = New System.Windows.Forms.Label()
        Me.PropertyEditorDefaultYAxis = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.LabelAxisYRange = New System.Windows.Forms.Label()
        Me.RadioButtonAxisYOneParameter = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAxisYFormula = New System.Windows.Forms.RadioButton()
        Me.TabPageEditPoints = New System.Windows.Forms.TabPage()
        Me.SplitContainerEditorLine = New System.Windows.Forms.SplitContainer()
        Me.ListBoxLimitationGraphs = New System.Windows.Forms.ListBox()
        Me.TableLayoutPanelEditPoints = New System.Windows.Forms.TableLayoutPanel()
        Me.PropertyEditorPlotStile = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.PropertyEditorPlotColor = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.TextBoxNameNewLimitationGraph = New System.Windows.Forms.TextBox()
        Me.LabelStyleLine = New System.Windows.Forms.Label()
        Me.LabelColorLine = New System.Windows.Forms.Label()
        Me.LabelNameLine = New System.Windows.Forms.Label()
        Me.LabelCaptionEditorLine = New System.Windows.Forms.Label()
        Me.DataGridViewTablePoit = New System.Windows.Forms.DataGridView()
        Me.ColumnIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnValueX = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnValueY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LabelCaptionEditorPoints = New System.Windows.Forms.Label()
        Me.ImageListTab = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStripMessage = New System.Windows.Forms.StatusStrip()
        Me.TSStatusLabelMessage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolBarForm = New System.Windows.Forms.ToolStrip()
        Me.TSComboBoxMathematicalExpression = New System.Windows.Forms.ToolStripComboBox()
        Me.TSButtonAddMathematicalExpression = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSComboBoxArguments = New System.Windows.Forms.ToolStripComboBox()
        Me.TSButtonAddArgument = New System.Windows.Forms.ToolStripButton()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStripForm.SuspendLayout()
        Me.ToolStripContainerForm.ContentPanel.SuspendLayout()
        Me.ToolStripContainerForm.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainerForm.SuspendLayout()
        CType(Me.SplitContainerForm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerForm.Panel1.SuspendLayout()
        Me.SplitContainerForm.Panel2.SuspendLayout()
        Me.SplitContainerForm.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        Me.TableLayoutPanelButtons.SuspendLayout()
        CType(Me.SplitContainerGraphParam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGraphParam.Panel1.SuspendLayout()
        Me.SplitContainerGraphParam.Panel2.SuspendLayout()
        Me.SplitContainerGraphParam.SuspendLayout()
        CType(Me.ScatterGraphParam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlGraphParam.SuspendLayout()
        Me.TabPageEditAxisX.SuspendLayout()
        CType(Me.SplitContainerAxisX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerAxisX.Panel1.SuspendLayout()
        Me.SplitContainerAxisX.Panel2.SuspendLayout()
        Me.SplitContainerAxisX.SuspendLayout()
        Me.TableLayoutPanelAxisX.SuspendLayout()
        CType(Me.DataGridViewAxisXARG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelToolX.SuspendLayout()
        Me.TabPageEditAxisY.SuspendLayout()
        CType(Me.SplitContainerAxisY, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerAxisY.Panel1.SuspendLayout()
        Me.SplitContainerAxisY.Panel2.SuspendLayout()
        Me.SplitContainerAxisY.SuspendLayout()
        Me.TableLayoutPanelAxisY.SuspendLayout()
        CType(Me.DataGridViewAxisYARG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelToolY.SuspendLayout()
        Me.TabPageEditPoints.SuspendLayout()
        CType(Me.SplitContainerEditorLine, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerEditorLine.Panel1.SuspendLayout()
        Me.SplitContainerEditorLine.Panel2.SuspendLayout()
        Me.SplitContainerEditorLine.SuspendLayout()
        Me.TableLayoutPanelEditPoints.SuspendLayout()
        CType(Me.DataGridViewTablePoit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStripMessage.SuspendLayout()
        Me.ToolBarForm.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBoxNameGraph
        '
        Me.TextBoxNameGraph.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxNameGraph.Location = New System.Drawing.Point(0, 16)
        Me.TextBoxNameGraph.Name = "TextBoxNameGraph"
        Me.TextBoxNameGraph.Size = New System.Drawing.Size(219, 20)
        Me.TextBoxNameGraph.TabIndex = 13
        Me.ToolTipForm.SetToolTip(Me.TextBoxNameGraph, "Наименование условия")
        '
        'ComboBoxAxisXFormulaParamaters
        '
        Me.ComboBoxAxisXFormulaParamaters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxAxisXFormulaParamaters.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxAxisXFormulaParamaters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxAxisXFormulaParamaters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxAxisXFormulaParamaters.DropDownWidth = 141
        Me.ComboBoxAxisXFormulaParamaters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxAxisXFormulaParamaters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxAxisXFormulaParamaters.ItemHeight = 18
        Me.ComboBoxAxisXFormulaParamaters.Location = New System.Drawing.Point(0, 23)
        Me.ComboBoxAxisXFormulaParamaters.MaxDropDownItems = 32
        Me.ComboBoxAxisXFormulaParamaters.Name = "ComboBoxAxisXFormulaParamaters"
        Me.ComboBoxAxisXFormulaParamaters.Size = New System.Drawing.Size(116, 24)
        Me.ComboBoxAxisXFormulaParamaters.TabIndex = 104
        Me.ToolTipForm.SetToolTip(Me.ComboBoxAxisXFormulaParamaters, "Выбрать параметр")
        '
        'ComboBoxAxisYFormulaParamaters
        '
        Me.ComboBoxAxisYFormulaParamaters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxAxisYFormulaParamaters.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxAxisYFormulaParamaters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxAxisYFormulaParamaters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxAxisYFormulaParamaters.DropDownWidth = 141
        Me.ComboBoxAxisYFormulaParamaters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxAxisYFormulaParamaters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxAxisYFormulaParamaters.ItemHeight = 18
        Me.ComboBoxAxisYFormulaParamaters.Location = New System.Drawing.Point(0, 23)
        Me.ComboBoxAxisYFormulaParamaters.MaxDropDownItems = 32
        Me.ComboBoxAxisYFormulaParamaters.Name = "ComboBoxAxisYFormulaParamaters"
        Me.ComboBoxAxisYFormulaParamaters.Size = New System.Drawing.Size(116, 24)
        Me.ComboBoxAxisYFormulaParamaters.TabIndex = 104
        Me.ToolTipForm.SetToolTip(Me.ComboBoxAxisYFormulaParamaters, "Выбрать параметр")
        '
        'ButtonTestFormulaX
        '
        Me.ButtonTestFormulaX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonTestFormulaX.Enabled = False
        Me.ButtonTestFormulaX.Location = New System.Drawing.Point(550, 8)
        Me.ButtonTestFormulaX.Name = "ButtonTestFormulaX"
        Me.ButtonTestFormulaX.Size = New System.Drawing.Size(203, 22)
        Me.ButtonTestFormulaX.TabIndex = 110
        Me.ButtonTestFormulaX.Text = "Тест формулы оси X"
        Me.ToolTipForm.SetToolTip(Me.ButtonTestFormulaX, "Тест формулы оси X на корректность")
        Me.ButtonTestFormulaX.UseVisualStyleBackColor = True
        '
        'ButtonTestFormulaY
        '
        Me.ButtonTestFormulaY.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonTestFormulaY.Enabled = False
        Me.ButtonTestFormulaY.Location = New System.Drawing.Point(550, 8)
        Me.ButtonTestFormulaY.Name = "ButtonTestFormulaY"
        Me.ButtonTestFormulaY.Size = New System.Drawing.Size(203, 22)
        Me.ButtonTestFormulaY.TabIndex = 111
        Me.ButtonTestFormulaY.Text = "Тест формулы оси Y"
        Me.ToolTipForm.SetToolTip(Me.ButtonTestFormulaY, "Тест формулы оси Y на корректность")
        Me.ButtonTestFormulaY.UseVisualStyleBackColor = True
        '
        'ButtonAddGraph
        '
        Me.ButtonAddGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonAddGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddGraph.Image = CType(resources.GetObject("ButtonAddGraph.Image"), System.Drawing.Image)
        Me.ButtonAddGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddGraph.Location = New System.Drawing.Point(3, 36)
        Me.ButtonAddGraph.Name = "ButtonAddGraph"
        Me.ButtonAddGraph.Size = New System.Drawing.Size(103, 28)
        Me.ButtonAddGraph.TabIndex = 19
        Me.ButtonAddGraph.Text = "&Добавить"
        Me.ToolTipForm.SetToolTip(Me.ButtonAddGraph, "Добавить график")
        Me.ButtonAddGraph.UseVisualStyleBackColor = False
        '
        'ButtonDeleteGraph
        '
        Me.ButtonDeleteGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonDeleteGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonDeleteGraph.Enabled = False
        Me.ButtonDeleteGraph.Image = CType(resources.GetObject("ButtonDeleteGraph.Image"), System.Drawing.Image)
        Me.ButtonDeleteGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDeleteGraph.Location = New System.Drawing.Point(112, 36)
        Me.ButtonDeleteGraph.Name = "ButtonDeleteGraph"
        Me.ButtonDeleteGraph.Size = New System.Drawing.Size(104, 28)
        Me.ButtonDeleteGraph.TabIndex = 18
        Me.ButtonDeleteGraph.Text = "&Удалить"
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteGraph, "Удалить график")
        Me.ButtonDeleteGraph.UseVisualStyleBackColor = False
        '
        'ButtonLoadGraph
        '
        Me.ButtonLoadGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonLoadGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonLoadGraph.Enabled = False
        Me.ButtonLoadGraph.Image = CType(resources.GetObject("ButtonLoadGraph.Image"), System.Drawing.Image)
        Me.ButtonLoadGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonLoadGraph.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLoadGraph.Name = "ButtonLoadGraph"
        Me.ButtonLoadGraph.Size = New System.Drawing.Size(103, 27)
        Me.ButtonLoadGraph.TabIndex = 15
        Me.ButtonLoadGraph.Text = "&Считать"
        Me.ToolTipForm.SetToolTip(Me.ButtonLoadGraph, "Считать график")
        Me.ButtonLoadGraph.UseVisualStyleBackColor = False
        '
        'ButtonSaveGraph
        '
        Me.ButtonSaveGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonSaveGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSaveGraph.Enabled = False
        Me.ButtonSaveGraph.Image = CType(resources.GetObject("ButtonSaveGraph.Image"), System.Drawing.Image)
        Me.ButtonSaveGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSaveGraph.Location = New System.Drawing.Point(112, 3)
        Me.ButtonSaveGraph.Name = "ButtonSaveGraph"
        Me.ButtonSaveGraph.Size = New System.Drawing.Size(104, 27)
        Me.ButtonSaveGraph.TabIndex = 14
        Me.ButtonSaveGraph.Text = "&Записать"
        Me.ToolTipForm.SetToolTip(Me.ButtonSaveGraph, "Записать график")
        Me.ButtonSaveGraph.UseVisualStyleBackColor = False
        '
        'ButtonAddNewLimitationGraph
        '
        Me.ButtonAddNewLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonAddNewLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddNewLimitationGraph.Image = CType(resources.GetObject("ButtonAddNewLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonAddNewLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAddNewLimitationGraph.Location = New System.Drawing.Point(5, 113)
        Me.ButtonAddNewLimitationGraph.Name = "ButtonAddNewLimitationGraph"
        Me.ButtonAddNewLimitationGraph.Size = New System.Drawing.Size(101, 47)
        Me.ButtonAddNewLimitationGraph.TabIndex = 33
        Me.ButtonAddNewLimitationGraph.Text = "&Добавить новую границу"
        Me.ButtonAddNewLimitationGraph.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipForm.SetToolTip(Me.ButtonAddNewLimitationGraph, "Добавить новую границу ограничения")
        Me.ButtonAddNewLimitationGraph.UseVisualStyleBackColor = False
        '
        'ButtonDeleteSelectedLimitationGraph
        '
        Me.ButtonDeleteSelectedLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonDeleteSelectedLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonDeleteSelectedLimitationGraph.Image = CType(resources.GetObject("ButtonDeleteSelectedLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonDeleteSelectedLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonDeleteSelectedLimitationGraph.Location = New System.Drawing.Point(5, 59)
        Me.ButtonDeleteSelectedLimitationGraph.Name = "ButtonDeleteSelectedLimitationGraph"
        Me.ButtonDeleteSelectedLimitationGraph.Size = New System.Drawing.Size(101, 46)
        Me.ButtonDeleteSelectedLimitationGraph.TabIndex = 32
        Me.ButtonDeleteSelectedLimitationGraph.Text = "&Удалить выделенную границу"
        Me.ButtonDeleteSelectedLimitationGraph.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteSelectedLimitationGraph, "Удалить выделенную границу ограничения")
        Me.ButtonDeleteSelectedLimitationGraph.UseVisualStyleBackColor = False
        '
        'ButtonEditLimitationGraph
        '
        Me.ButtonEditLimitationGraph.BackColor = System.Drawing.Color.Silver
        Me.ButtonEditLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonEditLimitationGraph.Image = CType(resources.GetObject("ButtonEditLimitationGraph.Image"), System.Drawing.Image)
        Me.ButtonEditLimitationGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonEditLimitationGraph.Location = New System.Drawing.Point(5, 5)
        Me.ButtonEditLimitationGraph.Name = "ButtonEditLimitationGraph"
        Me.ButtonEditLimitationGraph.Size = New System.Drawing.Size(101, 46)
        Me.ButtonEditLimitationGraph.TabIndex = 41
        Me.ButtonEditLimitationGraph.Text = "&Обновить границу"
        Me.ButtonEditLimitationGraph.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipForm.SetToolTip(Me.ButtonEditLimitationGraph, "Обновить границу ограничения")
        Me.ButtonEditLimitationGraph.UseVisualStyleBackColor = False
        '
        'ButtonFindChannelAxisXFormula
        '
        Me.ButtonFindChannelAxisXFormula.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonFindChannelAxisXFormula.Image = CType(resources.GetObject("ButtonFindChannelAxisXFormula.Image"), System.Drawing.Image)
        Me.ButtonFindChannelAxisXFormula.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonFindChannelAxisXFormula.Location = New System.Drawing.Point(0, 47)
        Me.ButtonFindChannelAxisXFormula.Name = "ButtonFindChannelAxisXFormula"
        Me.ButtonFindChannelAxisXFormula.Size = New System.Drawing.Size(116, 24)
        Me.ButtonFindChannelAxisXFormula.TabIndex = 106
        Me.ButtonFindChannelAxisXFormula.Text = "Поиск канала"
        Me.ButtonFindChannelAxisXFormula.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.ToolTipForm.SetToolTip(Me.ButtonFindChannelAxisXFormula, "Быстрый поиск канала")
        Me.ButtonFindChannelAxisXFormula.UseVisualStyleBackColor = True
        '
        'ButtonFindChannelAxisYFormula
        '
        Me.ButtonFindChannelAxisYFormula.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonFindChannelAxisYFormula.Image = CType(resources.GetObject("ButtonFindChannelAxisYFormula.Image"), System.Drawing.Image)
        Me.ButtonFindChannelAxisYFormula.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonFindChannelAxisYFormula.Location = New System.Drawing.Point(0, 47)
        Me.ButtonFindChannelAxisYFormula.Name = "ButtonFindChannelAxisYFormula"
        Me.ButtonFindChannelAxisYFormula.Size = New System.Drawing.Size(116, 24)
        Me.ButtonFindChannelAxisYFormula.TabIndex = 107
        Me.ButtonFindChannelAxisYFormula.Text = "Поиск канала"
        Me.ButtonFindChannelAxisYFormula.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.ToolTipForm.SetToolTip(Me.ButtonFindChannelAxisYFormula, "Быстрый поиск канала")
        Me.ButtonFindChannelAxisYFormula.UseVisualStyleBackColor = True
        '
        'ImageListExplorer
        '
        Me.ImageListExplorer.ImageStream = CType(resources.GetObject("ImageListExplorer.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListExplorer.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListExplorer.Images.SetKeyName(0, "Root0")
        Me.ImageListExplorer.Images.SetKeyName(1, "Description1")
        Me.ImageListExplorer.Images.SetKeyName(2, "NameGraph2")
        Me.ImageListExplorer.Images.SetKeyName(3, "AxisParameterPoint3")
        Me.ImageListExplorer.Images.SetKeyName(4, "Name4")
        Me.ImageListExplorer.Images.SetKeyName(5, "Max5")
        Me.ImageListExplorer.Images.SetKeyName(6, "Min6")
        Me.ImageListExplorer.Images.SetKeyName(7, "UseFormulaReduce7")
        Me.ImageListExplorer.Images.SetKeyName(8, "Formula8")
        Me.ImageListExplorer.Images.SetKeyName(9, "Index9")
        Me.ImageListExplorer.Images.SetKeyName(10, "NameChannel10")
        Me.ImageListExplorer.Images.SetKeyName(11, "IndexParameter11")
        Me.ImageListExplorer.Images.SetKeyName(12, "12")
        Me.ImageListExplorer.Images.SetKeyName(13, "GraphTU13")
        Me.ImageListExplorer.Images.SetKeyName(14, "Color14")
        Me.ImageListExplorer.Images.SetKeyName(15, "LineStyle15")
        Me.ImageListExplorer.Images.SetKeyName(16, "RulerXY16")
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'MenuStripForm
        '
        Me.MenuStripForm.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStripForm.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MenuStripForm.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStripForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemGraph})
        Me.MenuStripForm.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripForm.Name = "MenuStripForm"
        Me.MenuStripForm.Size = New System.Drawing.Size(1007, 24)
        Me.MenuStripForm.TabIndex = 1
        Me.MenuStripForm.Text = "MenuStrip1"
        '
        'TSMenuItemGraph
        '
        Me.TSMenuItemGraph.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemPrintGraph, Me.TSMenuItemSaveGraf, Me.TSMenuItemExit})
        Me.TSMenuItemGraph.Name = "TSMenuItemGraph"
        Me.TSMenuItemGraph.Size = New System.Drawing.Size(60, 20)
        Me.TSMenuItemGraph.Text = "&График"
        '
        'TSMenuItemPrintGraph
        '
        Me.TSMenuItemPrintGraph.Image = CType(resources.GetObject("TSMenuItemPrintGraph.Image"), System.Drawing.Image)
        Me.TSMenuItemPrintGraph.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemPrintGraph.Name = "TSMenuItemPrintGraph"
        Me.TSMenuItemPrintGraph.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.TSMenuItemPrintGraph.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemPrintGraph.Text = "&Печать графика"
        '
        'TSMenuItemSaveGraf
        '
        Me.TSMenuItemSaveGraf.Image = CType(resources.GetObject("TSMenuItemSaveGraf.Image"), System.Drawing.Image)
        Me.TSMenuItemSaveGraf.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemSaveGraf.Name = "TSMenuItemSaveGraf"
        Me.TSMenuItemSaveGraf.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.TSMenuItemSaveGraf.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemSaveGraf.Text = "&Запись графика"
        '
        'TSMenuItemExit
        '
        Me.TSMenuItemExit.Image = CType(resources.GetObject("TSMenuItemExit.Image"), System.Drawing.Image)
        Me.TSMenuItemExit.Name = "TSMenuItemExit"
        Me.TSMenuItemExit.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemExit.Text = "&Закрыть окно"
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(902, 570)
        '
        'ToolStripContainerForm
        '
        '
        'ToolStripContainerForm.ContentPanel
        '
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.SplitContainerForm)
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.StatusStripMessage)
        Me.ToolStripContainerForm.ContentPanel.Size = New System.Drawing.Size(1007, 663)
        Me.ToolStripContainerForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainerForm.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainerForm.Name = "ToolStripContainerForm"
        Me.ToolStripContainerForm.Size = New System.Drawing.Size(1007, 726)
        Me.ToolStripContainerForm.TabIndex = 21
        Me.ToolStripContainerForm.Text = "ToolStripContainer1"
        '
        'ToolStripContainerForm.TopToolStripPanel
        '
        Me.ToolStripContainerForm.TopToolStripPanel.Controls.Add(Me.MenuStripForm)
        Me.ToolStripContainerForm.TopToolStripPanel.Controls.Add(Me.ToolBarForm)
        '
        'SplitContainerForm
        '
        Me.SplitContainerForm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerForm.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerForm.Name = "SplitContainerForm"
        '
        'SplitContainerForm.Panel1
        '
        Me.SplitContainerForm.Panel1.Controls.Add(Me.XmlTreeView)
        Me.SplitContainerForm.Panel1.Controls.Add(Me.LabelAvailableLimitationGrap)
        Me.SplitContainerForm.Panel1.Controls.Add(Me.PanelButtons)
        '
        'SplitContainerForm.Panel2
        '
        Me.SplitContainerForm.Panel2.Controls.Add(Me.SplitContainerGraphParam)
        Me.SplitContainerForm.Size = New System.Drawing.Size(1007, 638)
        Me.SplitContainerForm.SplitterDistance = 227
        Me.SplitContainerForm.TabIndex = 35
        '
        'XmlTreeView
        '
        Me.XmlTreeView.AllowDrop = True
        Me.XmlTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XmlTreeView.ForeColor = System.Drawing.SystemColors.WindowText
        Me.XmlTreeView.HideSelection = False
        Me.XmlTreeView.HotTracking = True
        Me.XmlTreeView.ImageIndex = 0
        Me.XmlTreeView.ImageList = Me.ImageListExplorer
        Me.XmlTreeView.Indent = 19
        Me.XmlTreeView.ItemHeight = 16
        Me.XmlTreeView.Location = New System.Drawing.Point(0, 16)
        Me.XmlTreeView.Name = "XmlTreeView"
        Me.XmlTreeView.SelectedImageIndex = 1
        Me.XmlTreeView.Size = New System.Drawing.Size(223, 511)
        Me.XmlTreeView.TabIndex = 16
        Me.XmlTreeView.Text = "treeView1"
        '
        'LabelAvailableLimitationGrap
        '
        Me.LabelAvailableLimitationGrap.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelAvailableLimitationGrap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelAvailableLimitationGrap.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelAvailableLimitationGrap.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelAvailableLimitationGrap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelAvailableLimitationGrap.Location = New System.Drawing.Point(0, 0)
        Me.LabelAvailableLimitationGrap.Name = "LabelAvailableLimitationGrap"
        Me.LabelAvailableLimitationGrap.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelAvailableLimitationGrap.Size = New System.Drawing.Size(223, 16)
        Me.LabelAvailableLimitationGrap.TabIndex = 18
        Me.LabelAvailableLimitationGrap.Text = "Доступные графики в базе данных"
        Me.LabelAvailableLimitationGrap.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PanelButtons
        '
        Me.PanelButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelButtons.Controls.Add(Me.TableLayoutPanelButtons)
        Me.PanelButtons.Controls.Add(Me.TextBoxNameGraph)
        Me.PanelButtons.Controls.Add(Me.LabelEditGraph)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelButtons.Location = New System.Drawing.Point(0, 527)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(223, 107)
        Me.PanelButtons.TabIndex = 17
        '
        'TableLayoutPanelButtons
        '
        Me.TableLayoutPanelButtons.ColumnCount = 2
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonAddGraph, 0, 1)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonDeleteGraph, 0, 1)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonLoadGraph, 0, 0)
        Me.TableLayoutPanelButtons.Controls.Add(Me.ButtonSaveGraph, 1, 0)
        Me.TableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelButtons.Location = New System.Drawing.Point(0, 36)
        Me.TableLayoutPanelButtons.Name = "TableLayoutPanelButtons"
        Me.TableLayoutPanelButtons.RowCount = 2
        Me.TableLayoutPanelButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Size = New System.Drawing.Size(219, 67)
        Me.TableLayoutPanelButtons.TabIndex = 107
        '
        'LabelEditGraph
        '
        Me.LabelEditGraph.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelEditGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelEditGraph.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelEditGraph.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelEditGraph.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelEditGraph.Location = New System.Drawing.Point(0, 0)
        Me.LabelEditGraph.Name = "LabelEditGraph"
        Me.LabelEditGraph.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelEditGraph.Size = New System.Drawing.Size(219, 16)
        Me.LabelEditGraph.TabIndex = 108
        Me.LabelEditGraph.Text = "Редактируемый график"
        Me.LabelEditGraph.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SplitContainerGraphParam
        '
        Me.SplitContainerGraphParam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGraphParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGraphParam.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGraphParam.Name = "SplitContainerGraphParam"
        Me.SplitContainerGraphParam.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerGraphParam.Panel1
        '
        Me.SplitContainerGraphParam.Panel1.Controls.Add(Me.ScatterGraphParam)
        '
        'SplitContainerGraphParam.Panel2
        '
        Me.SplitContainerGraphParam.Panel2.Controls.Add(Me.TabControlGraphParam)
        Me.SplitContainerGraphParam.Size = New System.Drawing.Size(776, 638)
        Me.SplitContainerGraphParam.SplitterDistance = 406
        Me.SplitContainerGraphParam.TabIndex = 132
        '
        'ScatterGraphParam
        '
        Me.ScatterGraphParam.Border = NationalInstruments.UI.Border.Sunken
        Me.ScatterGraphParam.Caption = "График параметр У от Х"
        Me.ScatterGraphParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScatterGraphParam.Location = New System.Drawing.Point(0, 0)
        Me.ScatterGraphParam.Name = "ScatterGraphParam"
        Me.ScatterGraphParam.Plots.AddRange(New NationalInstruments.UI.ScatterPlot() {Me.ScatterPlot1})
        Me.ScatterGraphParam.Size = New System.Drawing.Size(772, 402)
        Me.ScatterGraphParam.TabIndex = 132
        Me.ScatterGraphParam.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
        Me.ScatterGraphParam.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
        '
        'ScatterPlot1
        '
        Me.ScatterPlot1.LineColor = System.Drawing.Color.White
        Me.ScatterPlot1.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.ScatterPlot1.PointColor = System.Drawing.Color.Red
        Me.ScatterPlot1.PointStyle = NationalInstruments.UI.PointStyle.SolidDiamond
        Me.ScatterPlot1.XAxis = Me.XAxis1
        Me.ScatterPlot1.YAxis = Me.YAxis1
        '
        'XAxis1
        '
        Me.XAxis1.Caption = "Параметр Х"
        Me.XAxis1.MajorDivisions.GridVisible = True
        Me.XAxis1.MinorDivisions.GridVisible = True
        Me.XAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        '
        'YAxis1
        '
        Me.YAxis1.Caption = "Параметр У"
        Me.YAxis1.MajorDivisions.GridVisible = True
        Me.YAxis1.MinorDivisions.GridVisible = True
        Me.YAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        '
        'TabControlGraphParam
        '
        Me.TabControlGraphParam.Controls.Add(Me.TabPageEditAxisX)
        Me.TabControlGraphParam.Controls.Add(Me.TabPageEditAxisY)
        Me.TabControlGraphParam.Controls.Add(Me.TabPageEditPoints)
        Me.TabControlGraphParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlGraphParam.ImageList = Me.ImageListTab
        Me.TabControlGraphParam.Location = New System.Drawing.Point(0, 0)
        Me.TabControlGraphParam.Name = "TabControlGraphParam"
        Me.TabControlGraphParam.SelectedIndex = 0
        Me.TabControlGraphParam.Size = New System.Drawing.Size(772, 224)
        Me.TabControlGraphParam.TabIndex = 107
        '
        'TabPageEditAxisX
        '
        Me.TabPageEditAxisX.Controls.Add(Me.SplitContainerAxisX)
        Me.TabPageEditAxisX.Controls.Add(Me.PanelToolX)
        Me.TabPageEditAxisX.ImageIndex = 0
        Me.TabPageEditAxisX.Location = New System.Drawing.Point(4, 29)
        Me.TabPageEditAxisX.Name = "TabPageEditAxisX"
        Me.TabPageEditAxisX.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageEditAxisX.Size = New System.Drawing.Size(764, 191)
        Me.TabPageEditAxisX.TabIndex = 0
        Me.TabPageEditAxisX.Text = "Редактор оси X"
        Me.TabPageEditAxisX.UseVisualStyleBackColor = True
        '
        'SplitContainerAxisX
        '
        Me.SplitContainerAxisX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerAxisX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerAxisX.IsSplitterFixed = True
        Me.SplitContainerAxisX.Location = New System.Drawing.Point(3, 39)
        Me.SplitContainerAxisX.Name = "SplitContainerAxisX"
        '
        'SplitContainerAxisX.Panel1
        '
        Me.SplitContainerAxisX.Panel1.Controls.Add(Me.CheckBoxAxisXParameterReduce)
        Me.SplitContainerAxisX.Panel1.Controls.Add(Me.ButtonFindChannelAxisXFormula)
        Me.SplitContainerAxisX.Panel1.Controls.Add(Me.ComboBoxAxisXFormulaParamaters)
        Me.SplitContainerAxisX.Panel1.Controls.Add(Me.LabelSelectParameterX)
        '
        'SplitContainerAxisX.Panel2
        '
        Me.SplitContainerAxisX.Panel2.Controls.Add(Me.TableLayoutPanelAxisX)
        Me.SplitContainerAxisX.Size = New System.Drawing.Size(758, 149)
        Me.SplitContainerAxisX.SplitterDistance = 120
        Me.SplitContainerAxisX.TabIndex = 113
        '
        'CheckBoxAxisXParameterReduce
        '
        Me.CheckBoxAxisXParameterReduce.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBoxAxisXParameterReduce.Location = New System.Drawing.Point(0, 71)
        Me.CheckBoxAxisXParameterReduce.Name = "CheckBoxAxisXParameterReduce"
        Me.CheckBoxAxisXParameterReduce.Size = New System.Drawing.Size(116, 74)
        Me.CheckBoxAxisXParameterReduce.TabIndex = 105
        Me.CheckBoxAxisXParameterReduce.Text = "Приводить параметр к нормальным температурным условиям (для оборотов)"
        Me.CheckBoxAxisXParameterReduce.UseVisualStyleBackColor = True
        '
        'LabelSelectParameterX
        '
        Me.LabelSelectParameterX.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectParameterX.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectParameterX.Name = "LabelSelectParameterX"
        Me.LabelSelectParameterX.Size = New System.Drawing.Size(116, 23)
        Me.LabelSelectParameterX.TabIndex = 103
        Me.LabelSelectParameterX.Text = "Выбрать параметр для оси X"
        Me.LabelSelectParameterX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanelAxisX
        '
        Me.TableLayoutPanelAxisX.ColumnCount = 2
        Me.TableLayoutPanelAxisX.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.06061!))
        Me.TableLayoutPanelAxisX.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.93939!))
        Me.TableLayoutPanelAxisX.Controls.Add(Me.DataGridViewAxisXARG, 1, 1)
        Me.TableLayoutPanelAxisX.Controls.Add(Me.LabelArgX, 1, 0)
        Me.TableLayoutPanelAxisX.Controls.Add(Me.TextBoxMathAxisX, 0, 1)
        Me.TableLayoutPanelAxisX.Controls.Add(Me.LabelMathX, 0, 0)
        Me.TableLayoutPanelAxisX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAxisX.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelAxisX.Name = "TableLayoutPanelAxisX"
        Me.TableLayoutPanelAxisX.RowCount = 2
        Me.TableLayoutPanelAxisX.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.TableLayoutPanelAxisX.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAxisX.Size = New System.Drawing.Size(630, 145)
        Me.TableLayoutPanelAxisX.TabIndex = 5
        '
        'DataGridViewAxisXARG
        '
        Me.DataGridViewAxisXARG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewAxisXARG.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        Me.DataGridViewAxisXARG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAxisXARG.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnARGX, Me.DataGridViewComboBoxColumnNameX})
        Me.DataGridViewAxisXARG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewAxisXARG.Location = New System.Drawing.Point(356, 47)
        Me.DataGridViewAxisXARG.Name = "DataGridViewAxisXARG"
        Me.DataGridViewAxisXARG.Size = New System.Drawing.Size(271, 95)
        Me.DataGridViewAxisXARG.TabIndex = 3
        '
        'DataGridViewTextBoxColumnARGX
        '
        Me.DataGridViewTextBoxColumnARGX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnARGX.HeaderText = "Имя аргумента"
        Me.DataGridViewTextBoxColumnARGX.Name = "DataGridViewTextBoxColumnARGX"
        Me.DataGridViewTextBoxColumnARGX.ReadOnly = True
        '
        'DataGridViewComboBoxColumnNameX
        '
        Me.DataGridViewComboBoxColumnNameX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewComboBoxColumnNameX.HeaderText = "Имя канала"
        Me.DataGridViewComboBoxColumnNameX.MaxDropDownItems = 32
        Me.DataGridViewComboBoxColumnNameX.Name = "DataGridViewComboBoxColumnNameX"
        '
        'LabelArgX
        '
        Me.LabelArgX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelArgX.Location = New System.Drawing.Point(356, 0)
        Me.LabelArgX.Name = "LabelArgX"
        Me.LabelArgX.Size = New System.Drawing.Size(271, 44)
        Me.LabelArgX.TabIndex = 2
        Me.LabelArgX.Text = "Выберите для аргумента соответствующий канал"
        '
        'TextBoxMathAxisX
        '
        Me.TextBoxMathAxisX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxMathAxisX.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxMathAxisX.Location = New System.Drawing.Point(3, 47)
        Me.TextBoxMathAxisX.Multiline = True
        Me.TextBoxMathAxisX.Name = "TextBoxMathAxisX"
        Me.TextBoxMathAxisX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxMathAxisX.Size = New System.Drawing.Size(347, 95)
        Me.TextBoxMathAxisX.TabIndex = 0
        '
        'LabelMathX
        '
        Me.LabelMathX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelMathX.Location = New System.Drawing.Point(3, 0)
        Me.LabelMathX.Name = "LabelMathX"
        Me.LabelMathX.Size = New System.Drawing.Size(347, 44)
        Me.LabelMathX.TabIndex = 1
        Me.LabelMathX.Text = "Введите математическое выражение используя вместо парамета соответствующий ему ар" &
    "гумент например: Math.sqrt((ARG2+ARG1/735.6)/(ARG1/735.6))"
        '
        'PanelToolX
        '
        Me.PanelToolX.Controls.Add(Me.ButtonTestFormulaX)
        Me.PanelToolX.Controls.Add(Me.LabelForX)
        Me.PanelToolX.Controls.Add(Me.PropertyEditorDefaultXAxis)
        Me.PanelToolX.Controls.Add(Me.LabelAxisXRange)
        Me.PanelToolX.Controls.Add(Me.RadioButtonAxisXOneParameter)
        Me.PanelToolX.Controls.Add(Me.RadioButtonAxisXisFormula)
        Me.PanelToolX.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelToolX.Location = New System.Drawing.Point(3, 3)
        Me.PanelToolX.Name = "PanelToolX"
        Me.PanelToolX.Size = New System.Drawing.Size(758, 36)
        Me.PanelToolX.TabIndex = 112
        '
        'LabelForX
        '
        Me.LabelForX.AutoSize = True
        Me.LabelForX.Location = New System.Drawing.Point(188, 11)
        Me.LabelForX.Name = "LabelForX"
        Me.LabelForX.Size = New System.Drawing.Size(135, 13)
        Me.LabelForX.TabIndex = 109
        Me.LabelForX.Text = "Для оси X используется:"
        '
        'PropertyEditorDefaultXAxis
        '
        Me.PropertyEditorDefaultXAxis.Location = New System.Drawing.Point(85, 8)
        Me.PropertyEditorDefaultXAxis.Name = "PropertyEditorDefaultXAxis"
        Me.PropertyEditorDefaultXAxis.Size = New System.Drawing.Size(86, 20)
        Me.PropertyEditorDefaultXAxis.TabIndex = 108
        '
        'LabelAxisXRange
        '
        Me.LabelAxisXRange.AutoSize = True
        Me.LabelAxisXRange.Location = New System.Drawing.Point(3, 12)
        Me.LabelAxisXRange.Name = "LabelAxisXRange"
        Me.LabelAxisXRange.Size = New System.Drawing.Size(76, 13)
        Me.LabelAxisXRange.TabIndex = 3
        Me.LabelAxisXRange.Text = "X Ось предел"
        '
        'RadioButtonAxisXOneParameter
        '
        Me.RadioButtonAxisXOneParameter.AutoSize = True
        Me.RadioButtonAxisXOneParameter.Checked = True
        Me.RadioButtonAxisXOneParameter.Location = New System.Drawing.Point(329, 11)
        Me.RadioButtonAxisXOneParameter.Name = "RadioButtonAxisXOneParameter"
        Me.RadioButtonAxisXOneParameter.Size = New System.Drawing.Size(139, 17)
        Me.RadioButtonAxisXOneParameter.TabIndex = 106
        Me.RadioButtonAxisXOneParameter.TabStop = True
        Me.RadioButtonAxisXOneParameter.Text = "только один параметр"
        Me.RadioButtonAxisXOneParameter.UseVisualStyleBackColor = True
        '
        'RadioButtonAxisXisFormula
        '
        Me.RadioButtonAxisXisFormula.AutoSize = True
        Me.RadioButtonAxisXisFormula.Location = New System.Drawing.Point(474, 11)
        Me.RadioButtonAxisXisFormula.Name = "RadioButtonAxisXisFormula"
        Me.RadioButtonAxisXisFormula.Size = New System.Drawing.Size(70, 17)
        Me.RadioButtonAxisXisFormula.TabIndex = 107
        Me.RadioButtonAxisXisFormula.Text = "формула"
        Me.RadioButtonAxisXisFormula.UseVisualStyleBackColor = True
        '
        'TabPageEditAxisY
        '
        Me.TabPageEditAxisY.Controls.Add(Me.SplitContainerAxisY)
        Me.TabPageEditAxisY.Controls.Add(Me.PanelToolY)
        Me.TabPageEditAxisY.ImageIndex = 0
        Me.TabPageEditAxisY.Location = New System.Drawing.Point(4, 29)
        Me.TabPageEditAxisY.Name = "TabPageEditAxisY"
        Me.TabPageEditAxisY.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageEditAxisY.Size = New System.Drawing.Size(764, 191)
        Me.TabPageEditAxisY.TabIndex = 2
        Me.TabPageEditAxisY.Text = "Редактор оси Y"
        Me.TabPageEditAxisY.UseVisualStyleBackColor = True
        '
        'SplitContainerAxisY
        '
        Me.SplitContainerAxisY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerAxisY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerAxisY.IsSplitterFixed = True
        Me.SplitContainerAxisY.Location = New System.Drawing.Point(3, 39)
        Me.SplitContainerAxisY.Name = "SplitContainerAxisY"
        '
        'SplitContainerAxisY.Panel1
        '
        Me.SplitContainerAxisY.Panel1.Controls.Add(Me.CheckBoxAxisYParameterReduce)
        Me.SplitContainerAxisY.Panel1.Controls.Add(Me.ButtonFindChannelAxisYFormula)
        Me.SplitContainerAxisY.Panel1.Controls.Add(Me.ComboBoxAxisYFormulaParamaters)
        Me.SplitContainerAxisY.Panel1.Controls.Add(Me.LabelSelectParameterY)
        '
        'SplitContainerAxisY.Panel2
        '
        Me.SplitContainerAxisY.Panel2.Controls.Add(Me.TableLayoutPanelAxisY)
        Me.SplitContainerAxisY.Size = New System.Drawing.Size(758, 149)
        Me.SplitContainerAxisY.SplitterDistance = 120
        Me.SplitContainerAxisY.TabIndex = 114
        '
        'CheckBoxAxisYParameterReduce
        '
        Me.CheckBoxAxisYParameterReduce.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBoxAxisYParameterReduce.Location = New System.Drawing.Point(0, 71)
        Me.CheckBoxAxisYParameterReduce.Name = "CheckBoxAxisYParameterReduce"
        Me.CheckBoxAxisYParameterReduce.Size = New System.Drawing.Size(116, 74)
        Me.CheckBoxAxisYParameterReduce.TabIndex = 105
        Me.CheckBoxAxisYParameterReduce.Text = "Приводить параметр к нормальным температурным условиям (для оборотов)"
        Me.CheckBoxAxisYParameterReduce.UseVisualStyleBackColor = True
        '
        'LabelSelectParameterY
        '
        Me.LabelSelectParameterY.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectParameterY.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectParameterY.Name = "LabelSelectParameterY"
        Me.LabelSelectParameterY.Size = New System.Drawing.Size(116, 23)
        Me.LabelSelectParameterY.TabIndex = 103
        Me.LabelSelectParameterY.Text = "Выбрать параметр для оси Y"
        Me.LabelSelectParameterY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanelAxisY
        '
        Me.TableLayoutPanelAxisY.ColumnCount = 2
        Me.TableLayoutPanelAxisY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.06061!))
        Me.TableLayoutPanelAxisY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.93939!))
        Me.TableLayoutPanelAxisY.Controls.Add(Me.DataGridViewAxisYARG, 1, 1)
        Me.TableLayoutPanelAxisY.Controls.Add(Me.LabelArgY, 1, 0)
        Me.TableLayoutPanelAxisY.Controls.Add(Me.TextBoxMathAxisY, 0, 1)
        Me.TableLayoutPanelAxisY.Controls.Add(Me.LabelMathY, 0, 0)
        Me.TableLayoutPanelAxisY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAxisY.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelAxisY.Name = "TableLayoutPanelAxisY"
        Me.TableLayoutPanelAxisY.RowCount = 2
        Me.TableLayoutPanelAxisY.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.TableLayoutPanelAxisY.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAxisY.Size = New System.Drawing.Size(630, 145)
        Me.TableLayoutPanelAxisY.TabIndex = 5
        '
        'DataGridViewAxisYARG
        '
        Me.DataGridViewAxisYARG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewAxisYARG.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        Me.DataGridViewAxisYARG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAxisYARG.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnARGY, Me.DataGridViewComboBoxColumnNameY})
        Me.DataGridViewAxisYARG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewAxisYARG.Location = New System.Drawing.Point(356, 47)
        Me.DataGridViewAxisYARG.Name = "DataGridViewAxisYARG"
        Me.DataGridViewAxisYARG.Size = New System.Drawing.Size(271, 95)
        Me.DataGridViewAxisYARG.TabIndex = 3
        '
        'DataGridViewTextBoxColumnARGY
        '
        Me.DataGridViewTextBoxColumnARGY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnARGY.HeaderText = "Имя аргумента"
        Me.DataGridViewTextBoxColumnARGY.Name = "DataGridViewTextBoxColumnARGY"
        Me.DataGridViewTextBoxColumnARGY.ReadOnly = True
        '
        'DataGridViewComboBoxColumnNameY
        '
        Me.DataGridViewComboBoxColumnNameY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewComboBoxColumnNameY.HeaderText = "Имя канала"
        Me.DataGridViewComboBoxColumnNameY.MaxDropDownItems = 32
        Me.DataGridViewComboBoxColumnNameY.Name = "DataGridViewComboBoxColumnNameY"
        '
        'LabelArgY
        '
        Me.LabelArgY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelArgY.Location = New System.Drawing.Point(356, 0)
        Me.LabelArgY.Name = "LabelArgY"
        Me.LabelArgY.Size = New System.Drawing.Size(271, 44)
        Me.LabelArgY.TabIndex = 2
        Me.LabelArgY.Text = "Выберите для аргумента соответствующий канал"
        '
        'TextBoxMathAxisY
        '
        Me.TextBoxMathAxisY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxMathAxisY.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxMathAxisY.Location = New System.Drawing.Point(3, 47)
        Me.TextBoxMathAxisY.Multiline = True
        Me.TextBoxMathAxisY.Name = "TextBoxMathAxisY"
        Me.TextBoxMathAxisY.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxMathAxisY.Size = New System.Drawing.Size(347, 95)
        Me.TextBoxMathAxisY.TabIndex = 0
        '
        'LabelMathY
        '
        Me.LabelMathY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelMathY.Location = New System.Drawing.Point(3, 0)
        Me.LabelMathY.Name = "LabelMathY"
        Me.LabelMathY.Size = New System.Drawing.Size(347, 44)
        Me.LabelMathY.TabIndex = 1
        Me.LabelMathY.Text = "Введите математическое выражение используя вместо парамета соответствующий ему ар" &
    "гумент например: Math.sqrt((ARG2+ARG1/735.6)/(ARG1/735.6))"
        '
        'PanelToolY
        '
        Me.PanelToolY.Controls.Add(Me.ButtonTestFormulaY)
        Me.PanelToolY.Controls.Add(Me.LabelForY)
        Me.PanelToolY.Controls.Add(Me.PropertyEditorDefaultYAxis)
        Me.PanelToolY.Controls.Add(Me.LabelAxisYRange)
        Me.PanelToolY.Controls.Add(Me.RadioButtonAxisYOneParameter)
        Me.PanelToolY.Controls.Add(Me.RadioButtonAxisYFormula)
        Me.PanelToolY.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelToolY.Location = New System.Drawing.Point(3, 3)
        Me.PanelToolY.Name = "PanelToolY"
        Me.PanelToolY.Size = New System.Drawing.Size(758, 36)
        Me.PanelToolY.TabIndex = 113
        '
        'LabelForY
        '
        Me.LabelForY.AutoSize = True
        Me.LabelForY.Location = New System.Drawing.Point(188, 11)
        Me.LabelForY.Name = "LabelForY"
        Me.LabelForY.Size = New System.Drawing.Size(135, 13)
        Me.LabelForY.TabIndex = 110
        Me.LabelForY.Text = "Для оси Y используется:"
        '
        'PropertyEditorDefaultYAxis
        '
        Me.PropertyEditorDefaultYAxis.Location = New System.Drawing.Point(85, 8)
        Me.PropertyEditorDefaultYAxis.Name = "PropertyEditorDefaultYAxis"
        Me.PropertyEditorDefaultYAxis.Size = New System.Drawing.Size(86, 20)
        Me.PropertyEditorDefaultYAxis.TabIndex = 109
        '
        'LabelAxisYRange
        '
        Me.LabelAxisYRange.AutoSize = True
        Me.LabelAxisYRange.Location = New System.Drawing.Point(3, 12)
        Me.LabelAxisYRange.Name = "LabelAxisYRange"
        Me.LabelAxisYRange.Size = New System.Drawing.Size(76, 13)
        Me.LabelAxisYRange.TabIndex = 3
        Me.LabelAxisYRange.Text = "Y Ось предел"
        '
        'RadioButtonAxisYOneParameter
        '
        Me.RadioButtonAxisYOneParameter.AutoSize = True
        Me.RadioButtonAxisYOneParameter.Checked = True
        Me.RadioButtonAxisYOneParameter.Location = New System.Drawing.Point(329, 11)
        Me.RadioButtonAxisYOneParameter.Name = "RadioButtonAxisYOneParameter"
        Me.RadioButtonAxisYOneParameter.Size = New System.Drawing.Size(139, 17)
        Me.RadioButtonAxisYOneParameter.TabIndex = 106
        Me.RadioButtonAxisYOneParameter.TabStop = True
        Me.RadioButtonAxisYOneParameter.Text = "только один параметр"
        Me.RadioButtonAxisYOneParameter.UseVisualStyleBackColor = True
        '
        'RadioButtonAxisYFormula
        '
        Me.RadioButtonAxisYFormula.AutoSize = True
        Me.RadioButtonAxisYFormula.Location = New System.Drawing.Point(474, 11)
        Me.RadioButtonAxisYFormula.Name = "RadioButtonAxisYFormula"
        Me.RadioButtonAxisYFormula.Size = New System.Drawing.Size(70, 17)
        Me.RadioButtonAxisYFormula.TabIndex = 107
        Me.RadioButtonAxisYFormula.Text = "формула"
        Me.RadioButtonAxisYFormula.UseVisualStyleBackColor = True
        '
        'TabPageEditPoints
        '
        Me.TabPageEditPoints.Controls.Add(Me.SplitContainerEditorLine)
        Me.TabPageEditPoints.ImageIndex = 1
        Me.TabPageEditPoints.Location = New System.Drawing.Point(4, 29)
        Me.TabPageEditPoints.Name = "TabPageEditPoints"
        Me.TabPageEditPoints.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageEditPoints.Size = New System.Drawing.Size(764, 191)
        Me.TabPageEditPoints.TabIndex = 1
        Me.TabPageEditPoints.Text = "Редактор границ"
        Me.TabPageEditPoints.UseVisualStyleBackColor = True
        '
        'SplitContainerEditorLine
        '
        Me.SplitContainerEditorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerEditorLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerEditorLine.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainerEditorLine.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainerEditorLine.Name = "SplitContainerEditorLine"
        '
        'SplitContainerEditorLine.Panel1
        '
        Me.SplitContainerEditorLine.Panel1.Controls.Add(Me.ListBoxLimitationGraphs)
        Me.SplitContainerEditorLine.Panel1.Controls.Add(Me.TableLayoutPanelEditPoints)
        Me.SplitContainerEditorLine.Panel1.Controls.Add(Me.LabelCaptionEditorLine)
        '
        'SplitContainerEditorLine.Panel2
        '
        Me.SplitContainerEditorLine.Panel2.Controls.Add(Me.DataGridViewTablePoit)
        Me.SplitContainerEditorLine.Panel2.Controls.Add(Me.LabelCaptionEditorPoints)
        Me.SplitContainerEditorLine.Size = New System.Drawing.Size(758, 185)
        Me.SplitContainerEditorLine.SplitterDistance = 498
        Me.SplitContainerEditorLine.TabIndex = 5
        '
        'ListBoxLimitationGraphs
        '
        Me.ListBoxLimitationGraphs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxLimitationGraphs.FormattingEnabled = True
        Me.ListBoxLimitationGraphs.Location = New System.Drawing.Point(0, 16)
        Me.ListBoxLimitationGraphs.Name = "ListBoxLimitationGraphs"
        Me.ListBoxLimitationGraphs.Size = New System.Drawing.Size(164, 165)
        Me.ListBoxLimitationGraphs.TabIndex = 3
        '
        'TableLayoutPanelEditPoints
        '
        Me.TableLayoutPanelEditPoints.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset
        Me.TableLayoutPanelEditPoints.ColumnCount = 3
        Me.TableLayoutPanelEditPoints.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.ButtonEditLimitationGraph, 0, 0)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.PropertyEditorPlotStile, 2, 2)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.PropertyEditorPlotColor, 2, 1)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.TextBoxNameNewLimitationGraph, 2, 0)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelStyleLine, 1, 2)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelColorLine, 1, 1)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.LabelNameLine, 1, 0)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.ButtonAddNewLimitationGraph, 0, 2)
        Me.TableLayoutPanelEditPoints.Controls.Add(Me.ButtonDeleteSelectedLimitationGraph, 0, 1)
        Me.TableLayoutPanelEditPoints.Dock = System.Windows.Forms.DockStyle.Right
        Me.TableLayoutPanelEditPoints.Location = New System.Drawing.Point(164, 16)
        Me.TableLayoutPanelEditPoints.Name = "TableLayoutPanelEditPoints"
        Me.TableLayoutPanelEditPoints.RowCount = 3
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelEditPoints.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelEditPoints.Size = New System.Drawing.Size(330, 165)
        Me.TableLayoutPanelEditPoints.TabIndex = 22
        '
        'PropertyEditorPlotStile
        '
        Me.PropertyEditorPlotStile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyEditorPlotStile.Location = New System.Drawing.Point(223, 113)
        Me.PropertyEditorPlotStile.Name = "PropertyEditorPlotStile"
        Me.PropertyEditorPlotStile.Size = New System.Drawing.Size(102, 20)
        Me.PropertyEditorPlotStile.TabIndex = 39
        '
        'PropertyEditorPlotColor
        '
        Me.PropertyEditorPlotColor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyEditorPlotColor.Location = New System.Drawing.Point(223, 59)
        Me.PropertyEditorPlotColor.Name = "PropertyEditorPlotColor"
        Me.PropertyEditorPlotColor.Size = New System.Drawing.Size(102, 20)
        Me.PropertyEditorPlotColor.TabIndex = 38
        '
        'TextBoxNameNewLimitationGraph
        '
        Me.TextBoxNameNewLimitationGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxNameNewLimitationGraph.Location = New System.Drawing.Point(223, 5)
        Me.TextBoxNameNewLimitationGraph.Multiline = True
        Me.TextBoxNameNewLimitationGraph.Name = "TextBoxNameNewLimitationGraph"
        Me.TextBoxNameNewLimitationGraph.Size = New System.Drawing.Size(102, 46)
        Me.TextBoxNameNewLimitationGraph.TabIndex = 37
        '
        'LabelStyleLine
        '
        Me.LabelStyleLine.AutoSize = True
        Me.LabelStyleLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelStyleLine.Location = New System.Drawing.Point(114, 110)
        Me.LabelStyleLine.Name = "LabelStyleLine"
        Me.LabelStyleLine.Size = New System.Drawing.Size(101, 53)
        Me.LabelStyleLine.TabIndex = 36
        Me.LabelStyleLine.Text = "Тип линии границы"
        Me.LabelStyleLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelColorLine
        '
        Me.LabelColorLine.AutoSize = True
        Me.LabelColorLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelColorLine.Location = New System.Drawing.Point(114, 56)
        Me.LabelColorLine.Name = "LabelColorLine"
        Me.LabelColorLine.Size = New System.Drawing.Size(101, 52)
        Me.LabelColorLine.TabIndex = 35
        Me.LabelColorLine.Text = "Цвет линии границы"
        Me.LabelColorLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelNameLine
        '
        Me.LabelNameLine.AutoSize = True
        Me.LabelNameLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameLine.Location = New System.Drawing.Point(114, 2)
        Me.LabelNameLine.Name = "LabelNameLine"
        Me.LabelNameLine.Size = New System.Drawing.Size(101, 52)
        Me.LabelNameLine.TabIndex = 34
        Me.LabelNameLine.Text = "Имя линии границы"
        Me.LabelNameLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelCaptionEditorLine
        '
        Me.LabelCaptionEditorLine.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelCaptionEditorLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionEditorLine.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaptionEditorLine.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCaptionEditorLine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaptionEditorLine.Location = New System.Drawing.Point(0, 0)
        Me.LabelCaptionEditorLine.Name = "LabelCaptionEditorLine"
        Me.LabelCaptionEditorLine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaptionEditorLine.Size = New System.Drawing.Size(494, 16)
        Me.LabelCaptionEditorLine.TabIndex = 19
        Me.LabelCaptionEditorLine.Text = "Выделить границу из списка"
        Me.LabelCaptionEditorLine.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'DataGridViewTablePoit
        '
        Me.DataGridViewTablePoit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewTablePoit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        Me.DataGridViewTablePoit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewTablePoit.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnIndex, Me.ColumnValueX, Me.ColumnValueY})
        Me.DataGridViewTablePoit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewTablePoit.Location = New System.Drawing.Point(0, 16)
        Me.DataGridViewTablePoit.Name = "DataGridViewTablePoit"
        Me.DataGridViewTablePoit.Size = New System.Drawing.Size(252, 165)
        Me.DataGridViewTablePoit.TabIndex = 2
        '
        'ColumnIndex
        '
        Me.ColumnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnIndex.HeaderText = "Номер точки"
        Me.ColumnIndex.Name = "ColumnIndex"
        Me.ColumnIndex.ReadOnly = True
        '
        'ColumnValueX
        '
        Me.ColumnValueX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnValueX.HeaderText = "Значение X"
        Me.ColumnValueX.Name = "ColumnValueX"
        '
        'ColumnValueY
        '
        Me.ColumnValueY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColumnValueY.HeaderText = "Значение Y"
        Me.ColumnValueY.Name = "ColumnValueY"
        '
        'LabelCaptionEditorPoints
        '
        Me.LabelCaptionEditorPoints.BackColor = System.Drawing.Color.LightSteelBlue
        Me.LabelCaptionEditorPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionEditorPoints.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaptionEditorPoints.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCaptionEditorPoints.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaptionEditorPoints.Location = New System.Drawing.Point(0, 0)
        Me.LabelCaptionEditorPoints.Name = "LabelCaptionEditorPoints"
        Me.LabelCaptionEditorPoints.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaptionEditorPoints.Size = New System.Drawing.Size(252, 16)
        Me.LabelCaptionEditorPoints.TabIndex = 20
        Me.LabelCaptionEditorPoints.Text = "Ввести по точкам X и Y график границы"
        Me.LabelCaptionEditorPoints.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ImageListTab
        '
        Me.ImageListTab.ImageStream = CType(resources.GetObject("ImageListTab.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTab.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTab.Images.SetKeyName(0, "Axis0")
        Me.ImageListTab.Images.SetKeyName(1, "GraphBound1")
        '
        'StatusStripMessage
        '
        Me.StatusStripMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSStatusLabelMessage})
        Me.StatusStripMessage.Location = New System.Drawing.Point(0, 638)
        Me.StatusStripMessage.Name = "StatusStripMessage"
        Me.StatusStripMessage.Size = New System.Drawing.Size(1007, 25)
        Me.StatusStripMessage.TabIndex = 34
        '
        'TSStatusLabelMessage
        '
        Me.TSStatusLabelMessage.AutoSize = False
        Me.TSStatusLabelMessage.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.TSStatusLabelMessage.Image = CType(resources.GetObject("TSStatusLabelMessage.Image"), System.Drawing.Image)
        Me.TSStatusLabelMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TSStatusLabelMessage.Name = "TSStatusLabelMessage"
        Me.TSStatusLabelMessage.Size = New System.Drawing.Size(992, 20)
        Me.TSStatusLabelMessage.Spring = True
        Me.TSStatusLabelMessage.Text = "Готов"
        Me.TSStatusLabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolBarForm
        '
        Me.ToolBarForm.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolBarForm.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolBarForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSComboBoxMathematicalExpression, Me.TSButtonAddMathematicalExpression, Me.ToolStripSeparator2, Me.TSComboBoxArguments, Me.TSButtonAddArgument})
        Me.ToolBarForm.Location = New System.Drawing.Point(3, 24)
        Me.ToolBarForm.Name = "ToolBarForm"
        Me.ToolBarForm.Size = New System.Drawing.Size(589, 39)
        Me.ToolBarForm.TabIndex = 32
        '
        'TSComboBoxMathematicalExpression
        '
        Me.TSComboBoxMathematicalExpression.Name = "TSComboBoxMathematicalExpression"
        Me.TSComboBoxMathematicalExpression.Size = New System.Drawing.Size(190, 39)
        '
        'TSButtonAddMathematicalExpression
        '
        Me.TSButtonAddMathematicalExpression.Image = CType(resources.GetObject("TSButtonAddMathematicalExpression.Image"), System.Drawing.Image)
        Me.TSButtonAddMathematicalExpression.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonAddMathematicalExpression.Name = "TSButtonAddMathematicalExpression"
        Me.TSButtonAddMathematicalExpression.Size = New System.Drawing.Size(157, 36)
        Me.TSButtonAddMathematicalExpression.Tag = "ВставитьМатематВыражение"
        Me.TSButtonAddMathematicalExpression.Text = "Вставить выражение"
        Me.TSButtonAddMathematicalExpression.ToolTipText = "Вставить в текст формулы математическое выражение"
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
        'FormEditiorGraphByParameter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1007, 726)
        Me.Controls.Add(Me.ToolStripContainerForm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormEditiorGraphByParameter"
        Me.Text = "Редактор графиков ограничений от параметров"
        Me.MenuStripForm.ResumeLayout(False)
        Me.MenuStripForm.PerformLayout()
        Me.ToolStripContainerForm.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.ContentPanel.PerformLayout()
        Me.ToolStripContainerForm.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainerForm.ResumeLayout(False)
        Me.ToolStripContainerForm.PerformLayout()
        Me.SplitContainerForm.Panel1.ResumeLayout(False)
        Me.SplitContainerForm.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerForm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerForm.ResumeLayout(False)
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelButtons.PerformLayout()
        Me.TableLayoutPanelButtons.ResumeLayout(False)
        Me.SplitContainerGraphParam.Panel1.ResumeLayout(False)
        Me.SplitContainerGraphParam.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGraphParam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraphParam.ResumeLayout(False)
        CType(Me.ScatterGraphParam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlGraphParam.ResumeLayout(False)
        Me.TabPageEditAxisX.ResumeLayout(False)
        Me.SplitContainerAxisX.Panel1.ResumeLayout(False)
        Me.SplitContainerAxisX.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerAxisX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerAxisX.ResumeLayout(False)
        Me.TableLayoutPanelAxisX.ResumeLayout(False)
        Me.TableLayoutPanelAxisX.PerformLayout()
        CType(Me.DataGridViewAxisXARG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelToolX.ResumeLayout(False)
        Me.PanelToolX.PerformLayout()
        Me.TabPageEditAxisY.ResumeLayout(False)
        Me.SplitContainerAxisY.Panel1.ResumeLayout(False)
        Me.SplitContainerAxisY.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerAxisY, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerAxisY.ResumeLayout(False)
        Me.TableLayoutPanelAxisY.ResumeLayout(False)
        Me.TableLayoutPanelAxisY.PerformLayout()
        CType(Me.DataGridViewAxisYARG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelToolY.ResumeLayout(False)
        Me.PanelToolY.PerformLayout()
        Me.TabPageEditPoints.ResumeLayout(False)
        Me.SplitContainerEditorLine.Panel1.ResumeLayout(False)
        Me.SplitContainerEditorLine.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerEditorLine, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerEditorLine.ResumeLayout(False)
        Me.TableLayoutPanelEditPoints.ResumeLayout(False)
        Me.TableLayoutPanelEditPoints.PerformLayout()
        CType(Me.DataGridViewTablePoit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStripMessage.ResumeLayout(False)
        Me.StatusStripMessage.PerformLayout()
        Me.ToolBarForm.ResumeLayout(False)
        Me.ToolBarForm.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTipForm As System.Windows.Forms.ToolTip
    Friend WithEvents ImageListExplorer As System.Windows.Forms.ImageList
    Friend WithEvents ButtonSaveGraph As System.Windows.Forms.Button
    Friend WithEvents MenuStripForm As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripContainerForm As System.Windows.Forms.ToolStripContainer
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Private WithEvents ToolBarForm As System.Windows.Forms.ToolStrip
    Friend WithEvents TSButtonAddMathematicalExpression As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonAddArgument As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainerForm As System.Windows.Forms.SplitContainer
    Protected WithEvents XmlTreeView As System.Windows.Forms.TreeView
    Public WithEvents LabelAvailableLimitationGrap As System.Windows.Forms.Label
    Friend WithEvents PanelButtons As System.Windows.Forms.Panel
    Friend WithEvents ButtonLoadGraph As System.Windows.Forms.Button
    Friend WithEvents TextBoxNameGraph As System.Windows.Forms.TextBox
    Friend WithEvents StatusStripMessage As System.Windows.Forms.StatusStrip
    Friend WithEvents TSStatusLabelMessage As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SplitContainerGraphParam As System.Windows.Forms.SplitContainer
    Friend WithEvents ScatterGraphParam As NationalInstruments.UI.WindowsForms.ScatterGraph
    Friend WithEvents ScatterPlot1 As NationalInstruments.UI.ScatterPlot
    Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
    Friend WithEvents ListBoxLimitationGraphs As System.Windows.Forms.ListBox
    Friend WithEvents DataGridViewTablePoit As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanelButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TabControlGraphParam As System.Windows.Forms.TabControl
    Friend WithEvents TabPageEditAxisX As System.Windows.Forms.TabPage
    Friend WithEvents TabPageEditPoints As System.Windows.Forms.TabPage
    Friend WithEvents ImageListTab As System.Windows.Forms.ImageList
    Friend WithEvents SplitContainerEditorLine As System.Windows.Forms.SplitContainer
    Public WithEvents LabelCaptionEditorLine As System.Windows.Forms.Label
    Friend WithEvents ButtonAddGraph As System.Windows.Forms.Button
    Friend WithEvents ButtonDeleteGraph As System.Windows.Forms.Button
    Public WithEvents LabelCaptionEditorPoints As System.Windows.Forms.Label
    Friend WithEvents TabPageEditAxisY As System.Windows.Forms.TabPage
    Friend WithEvents LabelAxisXRange As System.Windows.Forms.Label
    Friend WithEvents RadioButtonAxisXisFormula As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAxisXOneParameter As System.Windows.Forms.RadioButton
    Friend WithEvents SplitContainerAxisX As System.Windows.Forms.SplitContainer
    Friend WithEvents CheckBoxAxisXParameterReduce As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxAxisXFormulaParamaters As System.Windows.Forms.ComboBox
    Friend WithEvents LabelSelectParameterX As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanelAxisX As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridViewAxisXARG As System.Windows.Forms.DataGridView
    Friend WithEvents LabelArgX As System.Windows.Forms.Label
    Friend WithEvents TextBoxMathAxisX As System.Windows.Forms.TextBox
    Friend WithEvents LabelMathX As System.Windows.Forms.Label
    Friend WithEvents PanelToolX As System.Windows.Forms.Panel
    Friend WithEvents SplitContainerAxisY As System.Windows.Forms.SplitContainer
    Friend WithEvents CheckBoxAxisYParameterReduce As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxAxisYFormulaParamaters As System.Windows.Forms.ComboBox
    Friend WithEvents LabelSelectParameterY As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanelAxisY As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridViewAxisYARG As System.Windows.Forms.DataGridView
    Friend WithEvents LabelArgY As System.Windows.Forms.Label
    Friend WithEvents TextBoxMathAxisY As System.Windows.Forms.TextBox
    Friend WithEvents LabelMathY As System.Windows.Forms.Label
    Friend WithEvents PanelToolY As System.Windows.Forms.Panel
    Friend WithEvents LabelAxisYRange As System.Windows.Forms.Label
    Friend WithEvents RadioButtonAxisYOneParameter As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAxisYFormula As System.Windows.Forms.RadioButton
    Friend WithEvents PropertyEditorDefaultXAxis As NationalInstruments.UI.WindowsForms.PropertyEditor
    Friend WithEvents PropertyEditorDefaultYAxis As NationalInstruments.UI.WindowsForms.PropertyEditor
    Friend WithEvents LabelForX As System.Windows.Forms.Label
    Friend WithEvents LabelForY As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanelEditPoints As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonDeleteSelectedLimitationGraph As System.Windows.Forms.Button
    Friend WithEvents PropertyEditorPlotStile As NationalInstruments.UI.WindowsForms.PropertyEditor
    Private WithEvents PropertyEditorPlotColor As NationalInstruments.UI.WindowsForms.PropertyEditor
    Friend WithEvents TextBoxNameNewLimitationGraph As System.Windows.Forms.TextBox
    Friend WithEvents LabelStyleLine As System.Windows.Forms.Label
    Friend WithEvents LabelColorLine As System.Windows.Forms.Label
    Friend WithEvents LabelNameLine As System.Windows.Forms.Label
    Friend WithEvents ButtonAddNewLimitationGraph As System.Windows.Forms.Button
    Friend WithEvents ButtonTestFormulaX As System.Windows.Forms.Button
    Friend WithEvents ButtonTestFormulaY As System.Windows.Forms.Button
    Public WithEvents LabelEditGraph As System.Windows.Forms.Label
    Friend WithEvents TSComboBoxMathematicalExpression As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents TSComboBoxArguments As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents TSMenuItemGraph As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemPrintGraph As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemSaveGraf As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonEditLimitationGraph As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumnARGX As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnARGY As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumnNameX As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewComboBoxColumnNameY As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ColumnIndex As DataGridViewTextBoxColumn
    Friend WithEvents ColumnValueX As DataGridViewTextBoxColumn
    Friend WithEvents ColumnValueY As DataGridViewTextBoxColumn
    Friend WithEvents ImageListChannel As ImageList
    Friend WithEvents ButtonFindChannelAxisXFormula As Button
    Friend WithEvents ButtonFindChannelAxisYFormula As Button
End Class
