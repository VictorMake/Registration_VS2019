' Copyright (c) Microsoft Corporation. All rights reserved.
Partial Class FormDigitalPort
    Inherits System.Windows.Forms.Form

    '<System.Diagnostics.DebuggerNonUserCode()> _
    'Public Sub New()
    '    MyBase.New()

    '    'me call is required by the Windows Form Designer.
    '    InitializeComponent()
    '    ''// dynamically create sub-menu for main menu
    '    '_subMenu(0) = New ToolStripMenuItem("GripStyle", Nothing)
    '    '_subMenu(1) = New ToolStripMenuItem("Raft on...", Nothing)
    '    '_subMenu(2) = New ToolStripMenuItem("ToolStripTabControl...", Nothing)
    '    ''_subMenuGripStyle(0) = New ToolStripMenuItem("Visible", Nothing, New EventHandler(Me.GripStyleMenuHandler))
    '    ''_subMenuGripStyle(1) = New ToolStripMenuItem("Hidden", Nothing, New EventHandler(Me.GripStyleMenuHandler))
    '    '_subMenuGripStyle(1).Enabled = False
    '    ''_subMenuRaftSide(0) = New ToolStripMenuItem("Top", Nothing, New EventHandler(Me.RaftMenuHandler))
    '    ''_subMenuRaftSide(1) = New ToolStripMenuItem("Bottom", Nothing, New EventHandler(Me.RaftMenuHandler))
    '    ''_subMenuRaftSide(2) = New ToolStripMenuItem("Left", Nothing, New EventHandler(Me.RaftMenuHandler))
    '    ''_subMenuRaftSide(3) = New ToolStripMenuItem("Right", Nothing, New EventHandler(Me.RaftMenuHandler))
    '    '_subMenuTabCntr(0) = New ToolStripMenuItem("Add on", Nothing, New EventHandler(AddressOf Me.TabCntrHandler))
    '    '_subMenuTabCntr(1) = New ToolStripMenuItem("Remove from", Nothing, New EventHandler(AddressOf Me.TabCntrHandler))
    '    '_subMenuTabCntr(1).Enabled = False
    '    '_subMenu(0).DropDownItems.AddRange(Me._subMenuGripStyle)
    '    ''_subMenu(1).DropDownItems.AddRange(Me._subMenuRaftSide)
    '    '_subMenu(2).DropDownItems.AddRange(Me._subMenuTabCntr)

    '    'utilityHelper = New UtilityHelper
    '    'InitializeMenuHelperStrings(Me.MenuStrip1)
    '    'InitializeToolTips(Me.toolStrip1)
    '    'MapToolBarAndMenuItems()

    'End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDigitalPort))
        Me.SplitContainerGlobal = New System.Windows.Forms.SplitContainer()
        Me.TabControlConfig = New System.Windows.Forms.TabControl()
        Me.TabPageWatch = New System.Windows.Forms.TabPage()
        Me.SplitContainerОтборКонигураций = New System.Windows.Forms.SplitContainer()
        Me.ListBoxGroupConfigurations = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolStrip6 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonDelete = New System.Windows.Forms.ToolStripButton()
        Me.ComboBoxConfigurations = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ListViewAlarms = New System.Windows.Forms.ListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TabPageEdit = New System.Windows.Forms.TabPage()
        Me.SplitContainerExplorer = New System.Windows.Forms.SplitContainer()
        Me.propertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.InstrumentControlStrip1 = New NationalInstruments.UI.WindowsForms.InstrumentControlStrip()
        Me.TSButtonSaveInDataSet = New System.Windows.Forms.ToolStripButton()
        Me.TabPageBases = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelDataSet = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridViewБит = New System.Windows.Forms.DataGridView()
        Me.KeyБитПортаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyПортаDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameDataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерБитаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceBit = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelsDigitalOutputDataSet = New Registration.ChannelsDigitalOutputDataSet()
        Me.DataGridViewАргумент = New System.Windows.Forms.DataGridView()
        Me.КеуArgumentDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyFormulaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИмяАргументаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИмяКаналаDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ПриведениеDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.BindingSourceАргумент = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataGridViewТриггер = New System.Windows.Forms.DataGridView()
        Me.KeyТриггерDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyДествиеDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИмяКаналаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ОперацияСравненияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ВеличинаУсловияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ВеличинаУсловия2DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceTrigger = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorТриггер = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.ТриггерBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.ТриггерBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ТриггерBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ТриггерBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.ТриггерBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ТриггерBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonTriggerDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonTriggerSave = New System.Windows.Forms.ToolStripButton()
        Me.DataGridViewAction = New System.Windows.Forms.DataGridView()
        Me.KeyДействиеDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИмяДействияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceAction = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorКонфигурация = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingSourceConfiguration = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.КонфигурацияBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.КонфигурацияBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.КонфигурацияBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.КонфигурацияBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.КонфигурацияBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.КонфигурацияBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonConfigurationDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonConfigurationSave = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorФормула = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingSourceFormula = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.ФормулаBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.ФормулаBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ФормулаBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ФормулаBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.ФормулаBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ФормулаBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonFormulaDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonFormulaSave = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorАргумент = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.АргументBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.АргументBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.АргументBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.АргументBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.АргументBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.АргументBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonArgumenDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonArgumentSave = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorПорт = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingSourcePort = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolStripLabel6 = New System.Windows.Forms.ToolStripLabel()
        Me.ПортBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.ПортBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.ПортBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.ПортBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.ПортBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.ПортBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonSave = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorБит = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.ToolStripLabel7 = New System.Windows.Forms.ToolStripLabel()
        Me.БитBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.БитBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.БитBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.БитBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.БитBNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.БитBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonBitDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonBitSave = New System.Windows.Forms.ToolStripButton()
        Me.DataGridViewPort = New System.Windows.Forms.DataGridView()
        Me.KeyПортаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyДействиеDataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameDataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерУстройстваDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерПортаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GridViewConfiguration = New System.Windows.Forms.DataGridView()
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИмяКонфигурацииDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ОписаниеDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewFormula = New System.Windows.Forms.DataGridView()
        Me.KeyFormulaDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KeyДействиеDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ФормулаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ОперацияСравненияDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ВеличинаУсловияDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingNavigatorДействие = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.ToolStripLabel8 = New System.Windows.Forms.ToolStripLabel()
        Me.ДействиеBNMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.ДействиеBNMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator()
        Me.ДействиеBNPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator28 = New System.Windows.Forms.ToolStripSeparator()
        Me.ДействиеBNMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.ДействиеNMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.ДействиеBNAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonActionDelete = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonActionSave = New System.Windows.Forms.ToolStripButton()
        Me.TabPageDevice = New System.Windows.Forms.TabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBoxPort = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ComboDigitalWriteTask = New System.Windows.Forms.ComboBox()
        Me.ComboBoxLineCount = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.physicalChannelComboBox = New System.Windows.Forms.ComboBox()
        Me.ComboBoxPhysicalPort = New System.Windows.Forms.ComboBox()
        Me.deviceComboBox = New System.Windows.Forms.ComboBox()
        Me.automaticScaleModeGroupBox = New System.Windows.Forms.GroupBox()
        Me.scaleModeLabel = New System.Windows.Forms.Label()
        Me.ScaleModePropertyEditor = New NationalInstruments.UI.WindowsForms.PropertyEditor()
        Me.ScalingSwitchArray = New NationalInstruments.UI.WindowsForms.SwitchArray()
        Me.automaticScaleModePanel = New System.Windows.Forms.Panel()
        Me.selectValueLabel = New System.Windows.Forms.Label()
        Me.valuesListBox = New System.Windows.Forms.ListBox()
        Me.ButtonRemove = New System.Windows.Forms.Button()
        Me.valuesLabel = New System.Windows.Forms.Label()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.booleanComboBox = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.NumericEditArray2 = New NationalInstruments.UI.WindowsForms.NumericEditArray()
        Me.scalingLedArray = New NationalInstruments.UI.WindowsForms.LedArray()
        Me.NumericEditArray1 = New NationalInstruments.UI.WindowsForms.NumericEditArray()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tsContainer = New System.Windows.Forms.ToolStripContainer()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.Status = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ПанельУстановка = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindowCascade = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindowTileHorizontal = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindowTileVertical = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolBar1 = New System.Windows.Forms.ToolStrip()
        Me.TSButtonStart = New System.Windows.Forms.ToolStripButton()
        Me.TSButtonInterruptObservation = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSButtonHandCheck = New System.Windows.Forms.ToolStripButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TimerUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.ErrorProviderБит = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderПорт = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderАргумент = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderФормула = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderТриггер = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderДействие = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ErrorProviderКонфигурация = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ActionTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.ДействиеTableAdapter()
        Me.ConfigurationTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.КонфигурацияДействийTableAdapter()
        Me.TriggerFireDigitalOutputTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.ТриггерСрабатыванияЦифровогоВыходаTableAdapter()
        Me.FormulaFireDigitalOutputTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.ФормулаСрабатыванияЦифровогоВыходаTableAdapter()
        Me.ArgumentsOfFormulaTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.АргументыДляФормулыTableAdapter()
        Me.PortsTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.ПортыTableAdapter()
        Me.BitsOfPortTableAdapter = New Registration.ChannelsDigitalOutputDataSetTableAdapters.БитПортаTableAdapter()
        Me.ImageListTree = New System.Windows.Forms.ImageList(Me.components)
        Me.TimerResize = New System.Windows.Forms.Timer(Me.components)
        CType(Me.SplitContainerGlobal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGlobal.Panel2.SuspendLayout()
        Me.SplitContainerGlobal.SuspendLayout()
        Me.TabControlConfig.SuspendLayout()
        Me.TabPageWatch.SuspendLayout()
        CType(Me.SplitContainerОтборКонигураций, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerОтборКонигураций.Panel1.SuspendLayout()
        Me.SplitContainerОтборКонигураций.Panel2.SuspendLayout()
        Me.SplitContainerОтборКонигураций.SuspendLayout()
        Me.ToolStrip6.SuspendLayout()
        Me.TabPageEdit.SuspendLayout()
        CType(Me.SplitContainerExplorer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerExplorer.SuspendLayout()
        Me.InstrumentControlStrip1.SuspendLayout()
        Me.TabPageBases.SuspendLayout()
        Me.TableLayoutPanelDataSet.SuspendLayout()
        CType(Me.DataGridViewБит, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceBit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelsDigitalOutputDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewАргумент, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceАргумент, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewТриггер, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceTrigger, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorТриггер, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorТриггер.SuspendLayout()
        CType(Me.DataGridViewAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorКонфигурация, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorКонфигурация.SuspendLayout()
        CType(Me.BindingSourceConfiguration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorФормула, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorФормула.SuspendLayout()
        CType(Me.BindingSourceFormula, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorАргумент, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorАргумент.SuspendLayout()
        CType(Me.BindingNavigatorПорт, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorПорт.SuspendLayout()
        CType(Me.BindingSourcePort, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorБит, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorБит.SuspendLayout()
        CType(Me.DataGridViewPort, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewConfiguration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewFormula, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorДействие, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorДействие.SuspendLayout()
        Me.TabPageDevice.SuspendLayout()
        Me.GroupBoxPort.SuspendLayout()
        Me.automaticScaleModeGroupBox.SuspendLayout()
        CType(Me.ScalingSwitchArray.ItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.automaticScaleModePanel.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.NumericEditArray2.ItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scalingLedArray.ItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericEditArray1.ItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsContainer.BottomToolStripPanel.SuspendLayout()
        Me.tsContainer.ContentPanel.SuspendLayout()
        Me.tsContainer.TopToolStripPanel.SuspendLayout()
        Me.tsContainer.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolBar1.SuspendLayout()
        CType(Me.ErrorProviderБит, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderПорт, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderАргумент, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderФормула, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderТриггер, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderДействие, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProviderКонфигурация, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainerGlobal
        '
        Me.SplitContainerGlobal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGlobal.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerGlobal.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGlobal.Name = "SplitContainerGlobal"
        Me.SplitContainerGlobal.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerGlobal.Panel2
        '
        Me.SplitContainerGlobal.Panel2.Controls.Add(Me.TabControlConfig)
        Me.SplitContainerGlobal.Size = New System.Drawing.Size(1016, 440)
        Me.SplitContainerGlobal.SplitterDistance = 110
        Me.SplitContainerGlobal.TabIndex = 26
        '
        'TabControlConfig
        '
        Me.TabControlConfig.Controls.Add(Me.TabPageWatch)
        Me.TabControlConfig.Controls.Add(Me.TabPageEdit)
        Me.TabControlConfig.Controls.Add(Me.TabPageBases)
        Me.TabControlConfig.Controls.Add(Me.TabPageDevice)
        Me.TabControlConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlConfig.ImageList = Me.ImageList1
        Me.TabControlConfig.Location = New System.Drawing.Point(0, 0)
        Me.TabControlConfig.Name = "TabControlConfig"
        Me.TabControlConfig.SelectedIndex = 0
        Me.TabControlConfig.Size = New System.Drawing.Size(1012, 322)
        Me.TabControlConfig.TabIndex = 1
        '
        'TabPageWatch
        '
        Me.TabPageWatch.BackColor = System.Drawing.Color.Transparent
        Me.TabPageWatch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageWatch.Controls.Add(Me.SplitContainerОтборКонигураций)
        Me.TabPageWatch.ImageIndex = 2
        Me.TabPageWatch.Location = New System.Drawing.Point(4, 23)
        Me.TabPageWatch.Name = "TabPageWatch"
        Me.TabPageWatch.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageWatch.Size = New System.Drawing.Size(1004, 295)
        Me.TabPageWatch.TabIndex = 0
        Me.TabPageWatch.Text = "Выбор конфигураций событий"
        Me.TabPageWatch.UseVisualStyleBackColor = True
        '
        'SplitContainerОтборКонигураций
        '
        Me.SplitContainerОтборКонигураций.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerОтборКонигураций.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerОтборКонигураций.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainerОтборКонигураций.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainerОтборКонигураций.Name = "SplitContainerОтборКонигураций"
        '
        'SplitContainerОтборКонигураций.Panel1
        '
        Me.SplitContainerОтборКонигураций.Panel1.Controls.Add(Me.ListBoxGroupConfigurations)
        Me.SplitContainerОтборКонигураций.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainerОтборКонигураций.Panel1.Controls.Add(Me.ToolStrip6)
        Me.SplitContainerОтборКонигураций.Panel1.Controls.Add(Me.ComboBoxConfigurations)
        Me.SplitContainerОтборКонигураций.Panel1.Controls.Add(Me.Label4)
        '
        'SplitContainerОтборКонигураций.Panel2
        '
        Me.SplitContainerОтборКонигураций.Panel2.Controls.Add(Me.ListViewAlarms)
        Me.SplitContainerОтборКонигураций.Panel2.Controls.Add(Me.Label5)
        Me.SplitContainerОтборКонигураций.Size = New System.Drawing.Size(994, 285)
        Me.SplitContainerОтборКонигураций.SplitterDistance = 228
        Me.SplitContainerОтборКонигураций.TabIndex = 5
        '
        'ListBoxGroupConfigurations
        '
        Me.ListBoxGroupConfigurations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxGroupConfigurations.FormattingEnabled = True
        Me.ListBoxGroupConfigurations.Location = New System.Drawing.Point(0, 93)
        Me.ListBoxGroupConfigurations.Margin = New System.Windows.Forms.Padding(0)
        Me.ListBoxGroupConfigurations.Name = "ListBoxGroupConfigurations"
        Me.ListBoxGroupConfigurations.Size = New System.Drawing.Size(224, 188)
        Me.ListBoxGroupConfigurations.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ListBoxGroupConfigurations, "Выбрать устройство для редактирования циклограммы")
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(0, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Выбраны следующие конфигурации:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolStrip6
        '
        Me.ToolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip6.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonNew, Me.ToolStripButtonDelete})
        Me.ToolStrip6.Location = New System.Drawing.Point(0, 55)
        Me.ToolStrip6.Name = "ToolStrip6"
        Me.ToolStrip6.Size = New System.Drawing.Size(224, 25)
        Me.ToolStrip6.TabIndex = 2
        Me.ToolStrip6.Text = "ToolStripЛистГруппыУстройст"
        '
        'ToolStripButtonNew
        '
        Me.ToolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonNew.Enabled = False
        Me.ToolStripButtonNew.Image = CType(resources.GetObject("ToolStripButtonNew.Image"), System.Drawing.Image)
        Me.ToolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonNew.Name = "ToolStripButtonNew"
        Me.ToolStripButtonNew.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonNew.Text = "Добавить выбранную конфигурацию в список"
        Me.ToolStripButtonNew.ToolTipText = "Добавить выбранную конфигурацию в список"
        '
        'ToolStripButtonDelete
        '
        Me.ToolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonDelete.Enabled = False
        Me.ToolStripButtonDelete.Image = CType(resources.GetObject("ToolStripButtonDelete.Image"), System.Drawing.Image)
        Me.ToolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonDelete.Name = "ToolStripButtonDelete"
        Me.ToolStripButtonDelete.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonDelete.Text = "Удалить выбранную конфигурацию из списока"
        '
        'ComboBoxConfigurations
        '
        Me.ComboBoxConfigurations.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxConfigurations.FormattingEnabled = True
        Me.ComboBoxConfigurations.Location = New System.Drawing.Point(0, 34)
        Me.ComboBoxConfigurations.Name = "ComboBoxConfigurations"
        Me.ComboBoxConfigurations.Size = New System.Drawing.Size(224, 21)
        Me.ComboBoxConfigurations.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.ComboBoxConfigurations, "Выбрать конфигурацию для добавления в группу")
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(224, 34)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Для наблюдения существуют следующие конфигурации:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ListViewAlarms
        '
        Me.ListViewAlarms.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewAlarms.FormattingEnabled = True
        Me.ListViewAlarms.Location = New System.Drawing.Point(0, 23)
        Me.ListViewAlarms.Name = "ListViewAlarms"
        Me.ListViewAlarms.ScrollAlwaysVisible = True
        Me.ListViewAlarms.Size = New System.Drawing.Size(758, 258)
        Me.ListViewAlarms.TabIndex = 20
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(758, 23)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Список последних сработавших линий"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageEdit
        '
        Me.TabPageEdit.Controls.Add(Me.SplitContainerExplorer)
        Me.TabPageEdit.Controls.Add(Me.propertyGrid1)
        Me.TabPageEdit.Controls.Add(Me.InstrumentControlStrip1)
        Me.TabPageEdit.ImageIndex = 0
        Me.TabPageEdit.Location = New System.Drawing.Point(4, 23)
        Me.TabPageEdit.Name = "TabPageEdit"
        Me.TabPageEdit.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageEdit.Size = New System.Drawing.Size(1004, 295)
        Me.TabPageEdit.TabIndex = 2
        Me.TabPageEdit.Text = "Редактор конфигураций"
        Me.TabPageEdit.UseVisualStyleBackColor = True
        '
        'SplitContainerExplorer
        '
        Me.SplitContainerExplorer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerExplorer.Location = New System.Drawing.Point(3, 32)
        Me.SplitContainerExplorer.Name = "SplitContainerExplorer"
        Me.SplitContainerExplorer.Size = New System.Drawing.Size(759, 260)
        Me.SplitContainerExplorer.SplitterDistance = 330
        Me.SplitContainerExplorer.TabIndex = 25
        '
        'propertyGrid1
        '
        Me.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right
        Me.propertyGrid1.Font = New System.Drawing.Font("Tahoma", 8.830189!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.propertyGrid1.HelpBackColor = System.Drawing.SystemColors.Info
        Me.propertyGrid1.Location = New System.Drawing.Point(762, 32)
        Me.propertyGrid1.Name = "propertyGrid1"
        Me.propertyGrid1.Size = New System.Drawing.Size(239, 260)
        Me.propertyGrid1.TabIndex = 24
        '
        'InstrumentControlStrip1
        '
        Me.InstrumentControlStrip1.ImageScalingSize = New System.Drawing.Size(22, 22)
        Me.InstrumentControlStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSButtonSaveInDataSet})
        Me.InstrumentControlStrip1.Location = New System.Drawing.Point(3, 3)
        Me.InstrumentControlStrip1.Name = "InstrumentControlStrip1"
        Me.InstrumentControlStrip1.Size = New System.Drawing.Size(998, 29)
        Me.InstrumentControlStrip1.TabIndex = 23
        Me.InstrumentControlStrip1.Text = "InstrumentControlStrip1"
        '
        'TSButtonSaveInDataSet
        '
        Me.TSButtonSaveInDataSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonSaveInDataSet.Image = CType(resources.GetObject("TSButtonSaveInDataSet.Image"), System.Drawing.Image)
        Me.TSButtonSaveInDataSet.Name = "TSButtonSaveInDataSet"
        Me.TSButtonSaveInDataSet.Size = New System.Drawing.Size(26, 26)
        Me.TSButtonSaveInDataSet.Text = "Сохранить"
        Me.TSButtonSaveInDataSet.ToolTipText = "Сохранить изменения в базе данных"
        '
        'TabPageBases
        '
        Me.TabPageBases.Controls.Add(Me.TableLayoutPanelDataSet)
        Me.TabPageBases.ImageIndex = 1
        Me.TabPageBases.Location = New System.Drawing.Point(4, 23)
        Me.TabPageBases.Name = "TabPageBases"
        Me.TabPageBases.Size = New System.Drawing.Size(1004, 295)
        Me.TabPageBases.TabIndex = 3
        Me.TabPageBases.Text = "Контроль базы конфигураций"
        Me.TabPageBases.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelDataSet
        '
        Me.TableLayoutPanelDataSet.ColumnCount = 3
        Me.TableLayoutPanelDataSet.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDataSet.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDataSet.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewБит, 2, 5)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewАргумент, 2, 3)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewТриггер, 1, 1)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorТриггер, 1, 0)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewAction, 0, 3)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorКонфигурация, 0, 0)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorФормула, 1, 2)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorАргумент, 2, 2)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorПорт, 1, 4)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorБит, 2, 4)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewPort, 1, 5)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.GridViewConfiguration, 0, 1)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.DataGridViewFormula, 1, 3)
        Me.TableLayoutPanelDataSet.Controls.Add(Me.BindingNavigatorДействие, 0, 2)
        Me.TableLayoutPanelDataSet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelDataSet.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelDataSet.Name = "TableLayoutPanelDataSet"
        Me.TableLayoutPanelDataSet.RowCount = 6
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelDataSet.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelDataSet.Size = New System.Drawing.Size(1004, 295)
        Me.TableLayoutPanelDataSet.TabIndex = 3
        '
        'DataGridViewБит
        '
        Me.DataGridViewБит.AllowUserToAddRows = False
        Me.DataGridViewБит.AllowUserToDeleteRows = False
        Me.DataGridViewБит.AutoGenerateColumns = False
        Me.DataGridViewБит.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewБит.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyБитПортаDataGridViewTextBoxColumn, Me.KeyПортаDataGridViewTextBoxColumn1, Me.NameDataGridViewTextBoxColumn3, Me.НомерБитаDataGridViewTextBoxColumn})
        Me.DataGridViewБит.DataSource = Me.BindingSourceBit
        Me.DataGridViewБит.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewБит.Location = New System.Drawing.Point(671, 224)
        Me.DataGridViewБит.Name = "DataGridViewБит"
        Me.DataGridViewБит.Size = New System.Drawing.Size(330, 68)
        Me.DataGridViewБит.TabIndex = 36
        '
        'KeyБитПортаDataGridViewTextBoxColumn
        '
        Me.KeyБитПортаDataGridViewTextBoxColumn.DataPropertyName = "keyБитПорта"
        Me.KeyБитПортаDataGridViewTextBoxColumn.HeaderText = "keyБитПорта"
        Me.KeyБитПортаDataGridViewTextBoxColumn.Name = "KeyБитПортаDataGridViewTextBoxColumn"
        '
        'KeyПортаDataGridViewTextBoxColumn1
        '
        Me.KeyПортаDataGridViewTextBoxColumn1.DataPropertyName = "KeyПорта"
        Me.KeyПортаDataGridViewTextBoxColumn1.HeaderText = "KeyПорта"
        Me.KeyПортаDataGridViewTextBoxColumn1.Name = "KeyПортаDataGridViewTextBoxColumn1"
        '
        'NameDataGridViewTextBoxColumn3
        '
        Me.NameDataGridViewTextBoxColumn3.DataPropertyName = "name"
        Me.NameDataGridViewTextBoxColumn3.HeaderText = "name"
        Me.NameDataGridViewTextBoxColumn3.Name = "NameDataGridViewTextBoxColumn3"
        '
        'НомерБитаDataGridViewTextBoxColumn
        '
        Me.НомерБитаDataGridViewTextBoxColumn.DataPropertyName = "НомерБита"
        Me.НомерБитаDataGridViewTextBoxColumn.HeaderText = "НомерБита"
        Me.НомерБитаDataGridViewTextBoxColumn.Name = "НомерБитаDataGridViewTextBoxColumn"
        '
        'BindingSourceBit
        '
        Me.BindingSourceBit.DataMember = "БитПорта"
        Me.BindingSourceBit.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'ChannelsDigitalOutputDataSet
        '
        Me.ChannelsDigitalOutputDataSet.DataSetName = "ChannelsDigitalOutputDataSet"
        Me.ChannelsDigitalOutputDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DataGridViewАргумент
        '
        Me.DataGridViewАргумент.AllowUserToAddRows = False
        Me.DataGridViewАргумент.AllowUserToDeleteRows = False
        Me.DataGridViewАргумент.AutoGenerateColumns = False
        Me.DataGridViewАргумент.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewАргумент.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.КеуArgumentDataGridViewTextBoxColumn, Me.KeyFormulaDataGridViewTextBoxColumn, Me.ИмяАргументаDataGridViewTextBoxColumn, Me.ИмяКаналаDataGridViewTextBoxColumn1, Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn1, Me.ПриведениеDataGridViewCheckBoxColumn})
        Me.DataGridViewАргумент.DataSource = Me.BindingSourceАргумент
        Me.DataGridViewАргумент.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewАргумент.Location = New System.Drawing.Point(671, 126)
        Me.DataGridViewАргумент.Name = "DataGridViewАргумент"
        Me.DataGridViewАргумент.Size = New System.Drawing.Size(330, 67)
        Me.DataGridViewАргумент.TabIndex = 35
        '
        'КеуArgumentDataGridViewTextBoxColumn
        '
        Me.КеуArgumentDataGridViewTextBoxColumn.DataPropertyName = "КеуArgument"
        Me.КеуArgumentDataGridViewTextBoxColumn.HeaderText = "КеуArgument"
        Me.КеуArgumentDataGridViewTextBoxColumn.Name = "КеуArgumentDataGridViewTextBoxColumn"
        '
        'KeyFormulaDataGridViewTextBoxColumn
        '
        Me.KeyFormulaDataGridViewTextBoxColumn.DataPropertyName = "KeyFormula"
        Me.KeyFormulaDataGridViewTextBoxColumn.HeaderText = "KeyFormula"
        Me.KeyFormulaDataGridViewTextBoxColumn.Name = "KeyFormulaDataGridViewTextBoxColumn"
        '
        'ИмяАргументаDataGridViewTextBoxColumn
        '
        Me.ИмяАргументаDataGridViewTextBoxColumn.DataPropertyName = "ИмяАргумента"
        Me.ИмяАргументаDataGridViewTextBoxColumn.HeaderText = "ИмяАргумента"
        Me.ИмяАргументаDataGridViewTextBoxColumn.Name = "ИмяАргументаDataGridViewTextBoxColumn"
        '
        'ИмяКаналаDataGridViewTextBoxColumn1
        '
        Me.ИмяКаналаDataGridViewTextBoxColumn1.DataPropertyName = "ИмяКанала"
        Me.ИмяКаналаDataGridViewTextBoxColumn1.HeaderText = "ИмяКанала"
        Me.ИмяКаналаDataGridViewTextBoxColumn1.Name = "ИмяКаналаDataGridViewTextBoxColumn1"
        '
        'ИндексВМассивеПараметровDataGridViewTextBoxColumn1
        '
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn1.DataPropertyName = "ИндексВМассивеПараметров"
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn1.HeaderText = "ИндексВМассивеПараметров"
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn1.Name = "ИндексВМассивеПараметровDataGridViewTextBoxColumn1"
        '
        'ПриведениеDataGridViewCheckBoxColumn
        '
        Me.ПриведениеDataGridViewCheckBoxColumn.DataPropertyName = "Приведение"
        Me.ПриведениеDataGridViewCheckBoxColumn.HeaderText = "Приведение"
        Me.ПриведениеDataGridViewCheckBoxColumn.Name = "ПриведениеDataGridViewCheckBoxColumn"
        '
        'BindingSourceАргумент
        '
        Me.BindingSourceАргумент.DataMember = "АргументыДляФормулы"
        Me.BindingSourceАргумент.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'DataGridViewТриггер
        '
        Me.DataGridViewТриггер.AllowUserToAddRows = False
        Me.DataGridViewТриггер.AllowUserToDeleteRows = False
        Me.DataGridViewТриггер.AutoGenerateColumns = False
        Me.DataGridViewТриггер.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewТриггер.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyТриггерDataGridViewTextBoxColumn, Me.KeyДествиеDataGridViewTextBoxColumn, Me.NameDataGridViewTextBoxColumn, Me.ИмяКаналаDataGridViewTextBoxColumn, Me.ОперацияСравненияDataGridViewTextBoxColumn, Me.ВеличинаУсловияDataGridViewTextBoxColumn, Me.ВеличинаУсловия2DataGridViewTextBoxColumn, Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn})
        Me.DataGridViewТриггер.DataSource = Me.BindingSourceTrigger
        Me.DataGridViewТриггер.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewТриггер.Location = New System.Drawing.Point(337, 28)
        Me.DataGridViewТриггер.Name = "DataGridViewТриггер"
        Me.DataGridViewТриггер.Size = New System.Drawing.Size(328, 67)
        Me.DataGridViewТриггер.TabIndex = 32
        '
        'KeyТриггерDataGridViewTextBoxColumn
        '
        Me.KeyТриггерDataGridViewTextBoxColumn.DataPropertyName = "KeyТриггер"
        Me.KeyТриггерDataGridViewTextBoxColumn.HeaderText = "KeyТриггер"
        Me.KeyТриггерDataGridViewTextBoxColumn.Name = "KeyТриггерDataGridViewTextBoxColumn"
        '
        'KeyДествиеDataGridViewTextBoxColumn
        '
        Me.KeyДествиеDataGridViewTextBoxColumn.DataPropertyName = "keyДествие"
        Me.KeyДествиеDataGridViewTextBoxColumn.HeaderText = "keyДествие"
        Me.KeyДествиеDataGridViewTextBoxColumn.Name = "KeyДествиеDataGridViewTextBoxColumn"
        '
        'NameDataGridViewTextBoxColumn
        '
        Me.NameDataGridViewTextBoxColumn.DataPropertyName = "name"
        Me.NameDataGridViewTextBoxColumn.HeaderText = "name"
        Me.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn"
        '
        'ИмяКаналаDataGridViewTextBoxColumn
        '
        Me.ИмяКаналаDataGridViewTextBoxColumn.DataPropertyName = "ИмяКанала"
        Me.ИмяКаналаDataGridViewTextBoxColumn.HeaderText = "ИмяКанала"
        Me.ИмяКаналаDataGridViewTextBoxColumn.Name = "ИмяКаналаDataGridViewTextBoxColumn"
        '
        'ОперацияСравненияDataGridViewTextBoxColumn
        '
        Me.ОперацияСравненияDataGridViewTextBoxColumn.DataPropertyName = "ОперацияСравнения"
        Me.ОперацияСравненияDataGridViewTextBoxColumn.HeaderText = "ОперацияСравнения"
        Me.ОперацияСравненияDataGridViewTextBoxColumn.Name = "ОперацияСравненияDataGridViewTextBoxColumn"
        '
        'ВеличинаУсловияDataGridViewTextBoxColumn
        '
        Me.ВеличинаУсловияDataGridViewTextBoxColumn.DataPropertyName = "ВеличинаУсловия"
        Me.ВеличинаУсловияDataGridViewTextBoxColumn.HeaderText = "ВеличинаУсловия"
        Me.ВеличинаУсловияDataGridViewTextBoxColumn.Name = "ВеличинаУсловияDataGridViewTextBoxColumn"
        '
        'ВеличинаУсловия2DataGridViewTextBoxColumn
        '
        Me.ВеличинаУсловия2DataGridViewTextBoxColumn.DataPropertyName = "ВеличинаУсловия2"
        Me.ВеличинаУсловия2DataGridViewTextBoxColumn.HeaderText = "ВеличинаУсловия2"
        Me.ВеличинаУсловия2DataGridViewTextBoxColumn.Name = "ВеличинаУсловия2DataGridViewTextBoxColumn"
        '
        'ИндексВМассивеПараметровDataGridViewTextBoxColumn
        '
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn.DataPropertyName = "ИндексВМассивеПараметров"
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn.HeaderText = "ИндексВМассивеПараметров"
        Me.ИндексВМассивеПараметровDataGridViewTextBoxColumn.Name = "ИндексВМассивеПараметровDataGridViewTextBoxColumn"
        '
        'BindingSourceTrigger
        '
        Me.BindingSourceTrigger.DataMember = "ТриггерСрабатыванияЦифровогоВыхода"
        Me.BindingSourceTrigger.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'BindingNavigatorТриггер
        '
        Me.BindingNavigatorТриггер.AddNewItem = Nothing
        Me.BindingNavigatorТриггер.BindingSource = Me.BindingSourceTrigger
        Me.BindingNavigatorТриггер.CountItem = Me.ToolStripLabel2
        Me.BindingNavigatorТриггер.DeleteItem = Nothing
        Me.BindingNavigatorТриггер.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorТриггер.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ТриггерBNMoveFirstItem, Me.ТриггерBNMovePreviousItem, Me.ToolStripSeparator6, Me.ТриггерBNPositionItem, Me.ToolStripLabel2, Me.ToolStripSeparator7, Me.ТриггерBNMoveNextItem, Me.ТриггерBNMoveLastItem, Me.ToolStripSeparator10, Me.ТриггерBNAddNewItem, Me.TSButtonTriggerDelete, Me.TSButtonTriggerSave})
        Me.BindingNavigatorТриггер.Location = New System.Drawing.Point(334, 0)
        Me.BindingNavigatorТриггер.MoveFirstItem = Me.ТриггерBNMoveFirstItem
        Me.BindingNavigatorТриггер.MoveLastItem = Me.ТриггерBNMoveLastItem
        Me.BindingNavigatorТриггер.MoveNextItem = Me.ТриггерBNMoveNextItem
        Me.BindingNavigatorТриггер.MovePreviousItem = Me.ТриггерBNMovePreviousItem
        Me.BindingNavigatorТриггер.Name = "BindingNavigatorТриггер"
        Me.BindingNavigatorТриггер.PositionItem = Me.ТриггерBNPositionItem
        Me.BindingNavigatorТриггер.Size = New System.Drawing.Size(334, 25)
        Me.BindingNavigatorТриггер.TabIndex = 27
        Me.BindingNavigatorТриггер.Text = "BindingNavigatorТриггер"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel2.Text = "для {0}"
        Me.ToolStripLabel2.ToolTipText = "Total number of items"
        '
        'ТриггерBNMoveFirstItem
        '
        Me.ТриггерBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ТриггерBNMoveFirstItem.Image = CType(resources.GetObject("ТриггерBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.ТриггерBNMoveFirstItem.Name = "ТриггерBNMoveFirstItem"
        Me.ТриггерBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.ТриггерBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.ТриггерBNMoveFirstItem.Text = "Move first"
        '
        'ТриггерBNMovePreviousItem
        '
        Me.ТриггерBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ТриггерBNMovePreviousItem.Image = CType(resources.GetObject("ТриггерBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.ТриггерBNMovePreviousItem.Name = "ТриггерBNMovePreviousItem"
        Me.ТриггерBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.ТриггерBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.ТриггерBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'ТриггерBNPositionItem
        '
        Me.ТриггерBNPositionItem.AccessibleName = "Position"
        Me.ТриггерBNPositionItem.AutoSize = False
        Me.ТриггерBNPositionItem.Name = "ТриггерBNPositionItem"
        Me.ТриггерBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.ТриггерBNPositionItem.Text = "0"
        Me.ТриггерBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'ТриггерBNMoveNextItem
        '
        Me.ТриггерBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ТриггерBNMoveNextItem.Image = CType(resources.GetObject("ТриггерBNMoveNextItem.Image"), System.Drawing.Image)
        Me.ТриггерBNMoveNextItem.Name = "ТриггерBNMoveNextItem"
        Me.ТриггерBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.ТриггерBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.ТриггерBNMoveNextItem.Text = "Move next"
        '
        'ТриггерBNMoveLastItem
        '
        Me.ТриггерBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ТриггерBNMoveLastItem.Image = CType(resources.GetObject("ТриггерBNMoveLastItem.Image"), System.Drawing.Image)
        Me.ТриггерBNMoveLastItem.Name = "ТриггерBNMoveLastItem"
        Me.ТриггерBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.ТриггерBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.ТриггерBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'ТриггерBNAddNewItem
        '
        Me.ТриггерBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ТриггерBNAddNewItem.Enabled = False
        Me.ТриггерBNAddNewItem.Image = CType(resources.GetObject("ТриггерBNAddNewItem.Image"), System.Drawing.Image)
        Me.ТриггерBNAddNewItem.Name = "ТриггерBNAddNewItem"
        Me.ТриггерBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.ТриггерBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.ТриггерBNAddNewItem.Text = "Add new"
        '
        'TSButtonTriggerDelete
        '
        Me.TSButtonTriggerDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonTriggerDelete.Enabled = False
        Me.TSButtonTriggerDelete.Image = CType(resources.GetObject("TSButtonTriggerDelete.Image"), System.Drawing.Image)
        Me.TSButtonTriggerDelete.Name = "TSButtonTriggerDelete"
        Me.TSButtonTriggerDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonTriggerDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonTriggerDelete.Text = "Delete"
        '
        'TSButtonTriggerSave
        '
        Me.TSButtonTriggerSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonTriggerSave.Enabled = False
        Me.TSButtonTriggerSave.Image = CType(resources.GetObject("TSButtonTriggerSave.Image"), System.Drawing.Image)
        Me.TSButtonTriggerSave.Name = "TSButtonTriggerSave"
        Me.TSButtonTriggerSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonTriggerSave.Text = "Save Data"
        '
        'DataGridViewAction
        '
        Me.DataGridViewAction.AllowUserToAddRows = False
        Me.DataGridViewAction.AllowUserToDeleteRows = False
        Me.DataGridViewAction.AutoGenerateColumns = False
        Me.DataGridViewAction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAction.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyДействиеDataGridViewTextBoxColumn, Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn, Me.ИмяДействияDataGridViewTextBoxColumn})
        Me.DataGridViewAction.DataSource = Me.BindingSourceAction
        Me.DataGridViewAction.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewAction.Location = New System.Drawing.Point(3, 126)
        Me.DataGridViewAction.Name = "DataGridViewAction"
        Me.DataGridViewAction.Size = New System.Drawing.Size(328, 67)
        Me.DataGridViewAction.TabIndex = 1
        '
        'KeyДействиеDataGridViewTextBoxColumn
        '
        Me.KeyДействиеDataGridViewTextBoxColumn.DataPropertyName = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn.HeaderText = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn.Name = "KeyДействиеDataGridViewTextBoxColumn"
        '
        'KeyКонфигурацияДействияDataGridViewTextBoxColumn
        '
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn.DataPropertyName = "keyКонфигурацияДействия"
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn.HeaderText = "keyКонфигурацияДействия"
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn.Name = "KeyКонфигурацияДействияDataGridViewTextBoxColumn"
        '
        'ИмяДействияDataGridViewTextBoxColumn
        '
        Me.ИмяДействияDataGridViewTextBoxColumn.DataPropertyName = "ИмяДействия"
        Me.ИмяДействияDataGridViewTextBoxColumn.HeaderText = "ИмяДействия"
        Me.ИмяДействияDataGridViewTextBoxColumn.Name = "ИмяДействияDataGridViewTextBoxColumn"
        '
        'BindingSourceAction
        '
        Me.BindingSourceAction.DataMember = "Действие"
        Me.BindingSourceAction.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'BindingNavigatorКонфигурация
        '
        Me.BindingNavigatorКонфигурация.AddNewItem = Nothing
        Me.BindingNavigatorКонфигурация.BindingSource = Me.BindingSourceConfiguration
        Me.BindingNavigatorКонфигурация.CountItem = Me.ToolStripLabel3
        Me.BindingNavigatorКонфигурация.DeleteItem = Nothing
        Me.BindingNavigatorКонфигурация.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorКонфигурация.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.КонфигурацияBNMoveFirstItem, Me.КонфигурацияBNMovePreviousItem, Me.ToolStripSeparator11, Me.КонфигурацияBNPositionItem, Me.ToolStripLabel3, Me.ToolStripSeparator13, Me.КонфигурацияBNMoveNextItem, Me.КонфигурацияBNMoveLastItem, Me.ToolStripSeparator14, Me.КонфигурацияBNAddNewItem, Me.TSButtonConfigurationDelete, Me.TSButtonConfigurationSave})
        Me.BindingNavigatorКонфигурация.Location = New System.Drawing.Point(0, 0)
        Me.BindingNavigatorКонфигурация.MoveFirstItem = Me.КонфигурацияBNMoveFirstItem
        Me.BindingNavigatorКонфигурация.MoveLastItem = Me.КонфигурацияBNMoveLastItem
        Me.BindingNavigatorКонфигурация.MoveNextItem = Me.КонфигурацияBNMoveNextItem
        Me.BindingNavigatorКонфигурация.MovePreviousItem = Me.КонфигурацияBNMovePreviousItem
        Me.BindingNavigatorКонфигурация.Name = "BindingNavigatorКонфигурация"
        Me.BindingNavigatorКонфигурация.PositionItem = Me.КонфигурацияBNPositionItem
        Me.BindingNavigatorКонфигурация.Size = New System.Drawing.Size(334, 25)
        Me.BindingNavigatorКонфигурация.TabIndex = 26
        Me.BindingNavigatorКонфигурация.Text = "BindingNavigator1"
        '
        'BindingSourceConfiguration
        '
        Me.BindingSourceConfiguration.DataMember = "КонфигурацияДействий"
        Me.BindingSourceConfiguration.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel3.Text = "для {0}"
        Me.ToolStripLabel3.ToolTipText = "Total number of items"
        '
        'КонфигурацияBNMoveFirstItem
        '
        Me.КонфигурацияBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.КонфигурацияBNMoveFirstItem.Image = CType(resources.GetObject("КонфигурацияBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.КонфигурацияBNMoveFirstItem.Name = "КонфигурацияBNMoveFirstItem"
        Me.КонфигурацияBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.КонфигурацияBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.КонфигурацияBNMoveFirstItem.Text = "Move first"
        '
        'КонфигурацияBNMovePreviousItem
        '
        Me.КонфигурацияBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.КонфигурацияBNMovePreviousItem.Image = CType(resources.GetObject("КонфигурацияBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.КонфигурацияBNMovePreviousItem.Name = "КонфигурацияBNMovePreviousItem"
        Me.КонфигурацияBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.КонфигурацияBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.КонфигурацияBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 25)
        '
        'КонфигурацияBNPositionItem
        '
        Me.КонфигурацияBNPositionItem.AccessibleName = "Position"
        Me.КонфигурацияBNPositionItem.AutoSize = False
        Me.КонфигурацияBNPositionItem.Name = "КонфигурацияBNPositionItem"
        Me.КонфигурацияBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.КонфигурацияBNPositionItem.Text = "0"
        Me.КонфигурацияBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 25)
        '
        'КонфигурацияBNMoveNextItem
        '
        Me.КонфигурацияBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.КонфигурацияBNMoveNextItem.Image = CType(resources.GetObject("КонфигурацияBNMoveNextItem.Image"), System.Drawing.Image)
        Me.КонфигурацияBNMoveNextItem.Name = "КонфигурацияBNMoveNextItem"
        Me.КонфигурацияBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.КонфигурацияBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.КонфигурацияBNMoveNextItem.Text = "Move next"
        '
        'КонфигурацияBNMoveLastItem
        '
        Me.КонфигурацияBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.КонфигурацияBNMoveLastItem.Image = CType(resources.GetObject("КонфигурацияBNMoveLastItem.Image"), System.Drawing.Image)
        Me.КонфигурацияBNMoveLastItem.Name = "КонфигурацияBNMoveLastItem"
        Me.КонфигурацияBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.КонфигурацияBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.КонфигурацияBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 25)
        '
        'КонфигурацияBNAddNewItem
        '
        Me.КонфигурацияBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.КонфигурацияBNAddNewItem.Enabled = False
        Me.КонфигурацияBNAddNewItem.Image = CType(resources.GetObject("КонфигурацияBNAddNewItem.Image"), System.Drawing.Image)
        Me.КонфигурацияBNAddNewItem.Name = "КонфигурацияBNAddNewItem"
        Me.КонфигурацияBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.КонфигурацияBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.КонфигурацияBNAddNewItem.Text = "Add new"
        '
        'TSButtonConfigurationDelete
        '
        Me.TSButtonConfigurationDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonConfigurationDelete.Enabled = False
        Me.TSButtonConfigurationDelete.Image = CType(resources.GetObject("TSButtonConfigurationDelete.Image"), System.Drawing.Image)
        Me.TSButtonConfigurationDelete.Name = "TSButtonConfigurationDelete"
        Me.TSButtonConfigurationDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonConfigurationDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonConfigurationDelete.Text = "Delete"
        '
        'TSButtonConfigurationSave
        '
        Me.TSButtonConfigurationSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonConfigurationSave.Enabled = False
        Me.TSButtonConfigurationSave.Image = CType(resources.GetObject("TSButtonConfigurationSave.Image"), System.Drawing.Image)
        Me.TSButtonConfigurationSave.Name = "TSButtonConfigurationSave"
        Me.TSButtonConfigurationSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonConfigurationSave.Text = "Save Data"
        '
        'BindingNavigatorФормула
        '
        Me.BindingNavigatorФормула.AddNewItem = Nothing
        Me.BindingNavigatorФормула.BindingSource = Me.BindingSourceFormula
        Me.BindingNavigatorФормула.CountItem = Me.ToolStripLabel4
        Me.BindingNavigatorФормула.DeleteItem = Nothing
        Me.BindingNavigatorФормула.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorФормула.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ФормулаBNMoveFirstItem, Me.ФормулаBNMovePreviousItem, Me.ToolStripSeparator15, Me.ФормулаBNPositionItem, Me.ToolStripLabel4, Me.ToolStripSeparator16, Me.ФормулаBNMoveNextItem, Me.ФормулаBNMoveLastItem, Me.ToolStripSeparator17, Me.ФормулаBNAddNewItem, Me.TSButtonFormulaDelete, Me.TSButtonFormulaSave})
        Me.BindingNavigatorФормула.Location = New System.Drawing.Point(334, 98)
        Me.BindingNavigatorФормула.MoveFirstItem = Me.ФормулаBNMoveFirstItem
        Me.BindingNavigatorФормула.MoveLastItem = Me.ФормулаBNMoveLastItem
        Me.BindingNavigatorФормула.MoveNextItem = Me.ФормулаBNMoveNextItem
        Me.BindingNavigatorФормула.MovePreviousItem = Me.ФормулаBNPositionItem
        Me.BindingNavigatorФормула.Name = "BindingNavigatorФормула"
        Me.BindingNavigatorФормула.PositionItem = Me.ФормулаBNPositionItem
        Me.BindingNavigatorФормула.Size = New System.Drawing.Size(334, 25)
        Me.BindingNavigatorФормула.TabIndex = 28
        Me.BindingNavigatorФормула.Text = "BindingNavigator1"
        '
        'BindingSourceFormula
        '
        Me.BindingSourceFormula.DataMember = "ФормулаСрабатыванияЦифровогоВыхода"
        Me.BindingSourceFormula.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel4.Text = "для {0}"
        Me.ToolStripLabel4.ToolTipText = "Total number of items"
        '
        'ФормулаBNMoveFirstItem
        '
        Me.ФормулаBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ФормулаBNMoveFirstItem.Image = CType(resources.GetObject("ФормулаBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.ФормулаBNMoveFirstItem.Name = "ФормулаBNMoveFirstItem"
        Me.ФормулаBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.ФормулаBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.ФормулаBNMoveFirstItem.Text = "Move first"
        '
        'ФормулаBNMovePreviousItem
        '
        Me.ФормулаBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ФормулаBNMovePreviousItem.Image = CType(resources.GetObject("ФормулаBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.ФормулаBNMovePreviousItem.Name = "ФормулаBNMovePreviousItem"
        Me.ФормулаBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.ФормулаBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.ФормулаBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 25)
        '
        'ФормулаBNPositionItem
        '
        Me.ФормулаBNPositionItem.AccessibleName = "Position"
        Me.ФормулаBNPositionItem.AutoSize = False
        Me.ФормулаBNPositionItem.Name = "ФормулаBNPositionItem"
        Me.ФормулаBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.ФормулаBNPositionItem.Text = "0"
        Me.ФормулаBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 25)
        '
        'ФормулаBNMoveNextItem
        '
        Me.ФормулаBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ФормулаBNMoveNextItem.Image = CType(resources.GetObject("ФормулаBNMoveNextItem.Image"), System.Drawing.Image)
        Me.ФормулаBNMoveNextItem.Name = "ФормулаBNMoveNextItem"
        Me.ФормулаBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.ФормулаBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.ФормулаBNMoveNextItem.Text = "Move next"
        '
        'ФормулаBNMoveLastItem
        '
        Me.ФормулаBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ФормулаBNMoveLastItem.Image = CType(resources.GetObject("ФормулаBNMoveLastItem.Image"), System.Drawing.Image)
        Me.ФормулаBNMoveLastItem.Name = "ФормулаBNMoveLastItem"
        Me.ФормулаBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.ФормулаBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.ФормулаBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 25)
        '
        'ФормулаBNAddNewItem
        '
        Me.ФормулаBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ФормулаBNAddNewItem.Enabled = False
        Me.ФормулаBNAddNewItem.Image = CType(resources.GetObject("ФормулаBNAddNewItem.Image"), System.Drawing.Image)
        Me.ФормулаBNAddNewItem.Name = "ФормулаBNAddNewItem"
        Me.ФормулаBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.ФормулаBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.ФормулаBNAddNewItem.Text = "Add new"
        '
        'TSButtonFormulaDelete
        '
        Me.TSButtonFormulaDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonFormulaDelete.Enabled = False
        Me.TSButtonFormulaDelete.Image = CType(resources.GetObject("TSButtonFormulaDelete.Image"), System.Drawing.Image)
        Me.TSButtonFormulaDelete.Name = "TSButtonFormulaDelete"
        Me.TSButtonFormulaDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonFormulaDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonFormulaDelete.Text = "Delete"
        '
        'TSButtonFormulaSave
        '
        Me.TSButtonFormulaSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonFormulaSave.Enabled = False
        Me.TSButtonFormulaSave.Image = CType(resources.GetObject("TSButtonFormulaSave.Image"), System.Drawing.Image)
        Me.TSButtonFormulaSave.Name = "TSButtonFormulaSave"
        Me.TSButtonFormulaSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonFormulaSave.Text = "Save Data"
        '
        'BindingNavigatorАргумент
        '
        Me.BindingNavigatorАргумент.AddNewItem = Nothing
        Me.BindingNavigatorАргумент.BindingSource = Me.BindingSourceАргумент
        Me.BindingNavigatorАргумент.CountItem = Me.ToolStripLabel5
        Me.BindingNavigatorАргумент.DeleteItem = Nothing
        Me.BindingNavigatorАргумент.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorАргумент.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.АргументBNMoveFirstItem, Me.АргументBNMovePreviousItem, Me.ToolStripSeparator18, Me.АргументBNPositionItem, Me.ToolStripLabel5, Me.ToolStripSeparator19, Me.АргументBNMoveNextItem, Me.АргументBNMoveLastItem, Me.ToolStripSeparator20, Me.АргументBNAddNewItem, Me.TSButtonArgumenDelete, Me.TSButtonArgumentSave})
        Me.BindingNavigatorАргумент.Location = New System.Drawing.Point(668, 98)
        Me.BindingNavigatorАргумент.MoveFirstItem = Me.АргументBNMoveFirstItem
        Me.BindingNavigatorАргумент.MoveLastItem = Me.АргументBNMoveLastItem
        Me.BindingNavigatorАргумент.MoveNextItem = Me.АргументBNMoveNextItem
        Me.BindingNavigatorАргумент.MovePreviousItem = Me.АргументBNMovePreviousItem
        Me.BindingNavigatorАргумент.Name = "BindingNavigatorАргумент"
        Me.BindingNavigatorАргумент.PositionItem = Me.АргументBNPositionItem
        Me.BindingNavigatorАргумент.Size = New System.Drawing.Size(336, 25)
        Me.BindingNavigatorАргумент.TabIndex = 29
        Me.BindingNavigatorАргумент.Text = "BindingNavigator1"
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel5.Text = "для {0}"
        Me.ToolStripLabel5.ToolTipText = "Total number of items"
        '
        'АргументBNMoveFirstItem
        '
        Me.АргументBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.АргументBNMoveFirstItem.Image = CType(resources.GetObject("АргументBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.АргументBNMoveFirstItem.Name = "АргументBNMoveFirstItem"
        Me.АргументBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.АргументBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.АргументBNMoveFirstItem.Text = "Move first"
        '
        'АргументBNMovePreviousItem
        '
        Me.АргументBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.АргументBNMovePreviousItem.Image = CType(resources.GetObject("АргументBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.АргументBNMovePreviousItem.Name = "АргументBNMovePreviousItem"
        Me.АргументBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.АргументBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.АргументBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(6, 25)
        '
        'АргументBNPositionItem
        '
        Me.АргументBNPositionItem.AccessibleName = "Position"
        Me.АргументBNPositionItem.AutoSize = False
        Me.АргументBNPositionItem.Name = "АргументBNPositionItem"
        Me.АргументBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.АргументBNPositionItem.Text = "0"
        Me.АргументBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(6, 25)
        '
        'АргументBNMoveNextItem
        '
        Me.АргументBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.АргументBNMoveNextItem.Image = CType(resources.GetObject("АргументBNMoveNextItem.Image"), System.Drawing.Image)
        Me.АргументBNMoveNextItem.Name = "АргументBNMoveNextItem"
        Me.АргументBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.АргументBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.АргументBNMoveNextItem.Text = "Move next"
        '
        'АргументBNMoveLastItem
        '
        Me.АргументBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.АргументBNMoveLastItem.Image = CType(resources.GetObject("АргументBNMoveLastItem.Image"), System.Drawing.Image)
        Me.АргументBNMoveLastItem.Name = "АргументBNMoveLastItem"
        Me.АргументBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.АргументBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.АргументBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(6, 25)
        '
        'АргументBNAddNewItem
        '
        Me.АргументBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.АргументBNAddNewItem.Enabled = False
        Me.АргументBNAddNewItem.Image = CType(resources.GetObject("АргументBNAddNewItem.Image"), System.Drawing.Image)
        Me.АргументBNAddNewItem.Name = "АргументBNAddNewItem"
        Me.АргументBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.АргументBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.АргументBNAddNewItem.Text = "Add new"
        '
        'TSButtonArgumenDelete
        '
        Me.TSButtonArgumenDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonArgumenDelete.Enabled = False
        Me.TSButtonArgumenDelete.Image = CType(resources.GetObject("TSButtonArgumenDelete.Image"), System.Drawing.Image)
        Me.TSButtonArgumenDelete.Name = "TSButtonArgumenDelete"
        Me.TSButtonArgumenDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonArgumenDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonArgumenDelete.Text = "Delete"
        '
        'TSButtonArgumentSave
        '
        Me.TSButtonArgumentSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonArgumentSave.Enabled = False
        Me.TSButtonArgumentSave.Image = CType(resources.GetObject("TSButtonArgumentSave.Image"), System.Drawing.Image)
        Me.TSButtonArgumentSave.Name = "TSButtonArgumentSave"
        Me.TSButtonArgumentSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonArgumentSave.Text = "Save Data"
        '
        'BindingNavigatorПорт
        '
        Me.BindingNavigatorПорт.AddNewItem = Nothing
        Me.BindingNavigatorПорт.BindingSource = Me.BindingSourcePort
        Me.BindingNavigatorПорт.CountItem = Me.ToolStripLabel6
        Me.BindingNavigatorПорт.DeleteItem = Nothing
        Me.BindingNavigatorПорт.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorПорт.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ПортBNMoveFirstItem, Me.ПортBNMovePreviousItem, Me.ToolStripSeparator21, Me.ПортBNPositionItem, Me.ToolStripLabel6, Me.ToolStripSeparator22, Me.ПортBNMoveNextItem, Me.ПортBNMoveLastItem, Me.ToolStripSeparator23, Me.ПортBNAddNewItem, Me.TSButtonDelete, Me.TSButtonSave})
        Me.BindingNavigatorПорт.Location = New System.Drawing.Point(334, 196)
        Me.BindingNavigatorПорт.MoveFirstItem = Me.ПортBNMoveFirstItem
        Me.BindingNavigatorПорт.MoveLastItem = Me.ПортBNMoveLastItem
        Me.BindingNavigatorПорт.MoveNextItem = Me.ПортBNMoveNextItem
        Me.BindingNavigatorПорт.MovePreviousItem = Me.ПортBNMovePreviousItem
        Me.BindingNavigatorПорт.Name = "BindingNavigatorПорт"
        Me.BindingNavigatorПорт.PositionItem = Me.ПортBNPositionItem
        Me.BindingNavigatorПорт.Size = New System.Drawing.Size(334, 25)
        Me.BindingNavigatorПорт.TabIndex = 30
        Me.BindingNavigatorПорт.Text = "BindingNavigator1"
        '
        'BindingSourcePort
        '
        Me.BindingSourcePort.DataMember = "Порты"
        Me.BindingSourcePort.DataSource = Me.ChannelsDigitalOutputDataSet
        '
        'ToolStripLabel6
        '
        Me.ToolStripLabel6.Name = "ToolStripLabel6"
        Me.ToolStripLabel6.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel6.Text = "для {0}"
        Me.ToolStripLabel6.ToolTipText = "Total number of items"
        '
        'ПортBNMoveFirstItem
        '
        Me.ПортBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ПортBNMoveFirstItem.Image = CType(resources.GetObject("ПортBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.ПортBNMoveFirstItem.Name = "ПортBNMoveFirstItem"
        Me.ПортBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.ПортBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.ПортBNMoveFirstItem.Text = "Move first"
        '
        'ПортBNMovePreviousItem
        '
        Me.ПортBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ПортBNMovePreviousItem.Image = CType(resources.GetObject("ПортBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.ПортBNMovePreviousItem.Name = "ПортBNMovePreviousItem"
        Me.ПортBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.ПортBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.ПортBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(6, 25)
        '
        'ПортBNPositionItem
        '
        Me.ПортBNPositionItem.AccessibleName = "Position"
        Me.ПортBNPositionItem.AutoSize = False
        Me.ПортBNPositionItem.Name = "ПортBNPositionItem"
        Me.ПортBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.ПортBNPositionItem.Text = "0"
        Me.ПортBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(6, 25)
        '
        'ПортBNMoveNextItem
        '
        Me.ПортBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ПортBNMoveNextItem.Image = CType(resources.GetObject("ПортBNMoveNextItem.Image"), System.Drawing.Image)
        Me.ПортBNMoveNextItem.Name = "ПортBNMoveNextItem"
        Me.ПортBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.ПортBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.ПортBNMoveNextItem.Text = "Move next"
        '
        'ПортBNMoveLastItem
        '
        Me.ПортBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ПортBNMoveLastItem.Image = CType(resources.GetObject("ПортBNMoveLastItem.Image"), System.Drawing.Image)
        Me.ПортBNMoveLastItem.Name = "ПортBNMoveLastItem"
        Me.ПортBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.ПортBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.ПортBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(6, 25)
        '
        'ПортBNAddNewItem
        '
        Me.ПортBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ПортBNAddNewItem.Enabled = False
        Me.ПортBNAddNewItem.Image = CType(resources.GetObject("ПортBNAddNewItem.Image"), System.Drawing.Image)
        Me.ПортBNAddNewItem.Name = "ПортBNAddNewItem"
        Me.ПортBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.ПортBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.ПортBNAddNewItem.Text = "Add new"
        '
        'TSButtonDelete
        '
        Me.TSButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonDelete.Enabled = False
        Me.TSButtonDelete.Image = CType(resources.GetObject("TSButtonDelete.Image"), System.Drawing.Image)
        Me.TSButtonDelete.Name = "TSButtonDelete"
        Me.TSButtonDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonDelete.Text = "Delete"
        '
        'TSButtonSave
        '
        Me.TSButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonSave.Enabled = False
        Me.TSButtonSave.Image = CType(resources.GetObject("TSButtonSave.Image"), System.Drawing.Image)
        Me.TSButtonSave.Name = "TSButtonSave"
        Me.TSButtonSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonSave.Text = "Save Data"
        '
        'BindingNavigatorБит
        '
        Me.BindingNavigatorБит.AddNewItem = Nothing
        Me.BindingNavigatorБит.BindingSource = Me.BindingSourceBit
        Me.BindingNavigatorБит.CountItem = Me.ToolStripLabel7
        Me.BindingNavigatorБит.DeleteItem = Nothing
        Me.BindingNavigatorБит.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorБит.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.БитBNMoveFirstItem, Me.БитBNMovePreviousItem, Me.ToolStripSeparator24, Me.БитBNPositionItem, Me.ToolStripLabel7, Me.ToolStripSeparator25, Me.БитBNMoveNextItem, Me.БитBNMoveLastItem, Me.ToolStripSeparator26, Me.БитBNAddNewItem, Me.TSButtonBitDelete, Me.TSButtonBitSave})
        Me.BindingNavigatorБит.Location = New System.Drawing.Point(668, 196)
        Me.BindingNavigatorБит.MoveFirstItem = Me.БитBNMoveFirstItem
        Me.BindingNavigatorБит.MoveLastItem = Me.БитBNMoveLastItem
        Me.BindingNavigatorБит.MoveNextItem = Me.БитBNMoveNextItem
        Me.BindingNavigatorБит.MovePreviousItem = Me.БитBNMovePreviousItem
        Me.BindingNavigatorБит.Name = "BindingNavigatorБит"
        Me.BindingNavigatorБит.PositionItem = Me.БитBNPositionItem
        Me.BindingNavigatorБит.Size = New System.Drawing.Size(336, 25)
        Me.BindingNavigatorБит.TabIndex = 31
        Me.BindingNavigatorБит.Text = "BindingNavigator1"
        '
        'ToolStripLabel7
        '
        Me.ToolStripLabel7.Name = "ToolStripLabel7"
        Me.ToolStripLabel7.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel7.Text = "для {0}"
        Me.ToolStripLabel7.ToolTipText = "Total number of items"
        '
        'БитBNMoveFirstItem
        '
        Me.БитBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.БитBNMoveFirstItem.Image = CType(resources.GetObject("БитBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.БитBNMoveFirstItem.Name = "БитBNMoveFirstItem"
        Me.БитBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.БитBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.БитBNMoveFirstItem.Text = "Move first"
        '
        'БитBNMovePreviousItem
        '
        Me.БитBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.БитBNMovePreviousItem.Image = CType(resources.GetObject("БитBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.БитBNMovePreviousItem.Name = "БитBNMovePreviousItem"
        Me.БитBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.БитBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.БитBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(6, 25)
        '
        'БитBNPositionItem
        '
        Me.БитBNPositionItem.AccessibleName = "Position"
        Me.БитBNPositionItem.AutoSize = False
        Me.БитBNPositionItem.Name = "БитBNPositionItem"
        Me.БитBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.БитBNPositionItem.Text = "0"
        Me.БитBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(6, 25)
        '
        'БитBNMoveNextItem
        '
        Me.БитBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.БитBNMoveNextItem.Image = CType(resources.GetObject("БитBNMoveNextItem.Image"), System.Drawing.Image)
        Me.БитBNMoveNextItem.Name = "БитBNMoveNextItem"
        Me.БитBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.БитBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.БитBNMoveNextItem.Text = "Move next"
        '
        'БитBNMoveLastItem
        '
        Me.БитBNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.БитBNMoveLastItem.Image = CType(resources.GetObject("БитBNMoveLastItem.Image"), System.Drawing.Image)
        Me.БитBNMoveLastItem.Name = "БитBNMoveLastItem"
        Me.БитBNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.БитBNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.БитBNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(6, 25)
        '
        'БитBNAddNewItem
        '
        Me.БитBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.БитBNAddNewItem.Enabled = False
        Me.БитBNAddNewItem.Image = CType(resources.GetObject("БитBNAddNewItem.Image"), System.Drawing.Image)
        Me.БитBNAddNewItem.Name = "БитBNAddNewItem"
        Me.БитBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.БитBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.БитBNAddNewItem.Text = "Add new"
        '
        'TSButtonBitDelete
        '
        Me.TSButtonBitDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonBitDelete.Enabled = False
        Me.TSButtonBitDelete.Image = CType(resources.GetObject("TSButtonBitDelete.Image"), System.Drawing.Image)
        Me.TSButtonBitDelete.Name = "TSButtonBitDelete"
        Me.TSButtonBitDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonBitDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonBitDelete.Text = "Delete"
        '
        'TSButtonBitSave
        '
        Me.TSButtonBitSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonBitSave.Enabled = False
        Me.TSButtonBitSave.Image = CType(resources.GetObject("TSButtonBitSave.Image"), System.Drawing.Image)
        Me.TSButtonBitSave.Name = "TSButtonBitSave"
        Me.TSButtonBitSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonBitSave.Text = "Save Data"
        '
        'DataGridViewPort
        '
        Me.DataGridViewPort.AllowUserToAddRows = False
        Me.DataGridViewPort.AllowUserToDeleteRows = False
        Me.DataGridViewPort.AutoGenerateColumns = False
        Me.DataGridViewPort.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewPort.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyПортаDataGridViewTextBoxColumn, Me.KeyДействиеDataGridViewTextBoxColumn2, Me.NameDataGridViewTextBoxColumn2, Me.НомерУстройстваDataGridViewTextBoxColumn, Me.НомерМодуляКорзиныDataGridViewTextBoxColumn, Me.НомерПортаDataGridViewTextBoxColumn})
        Me.DataGridViewPort.DataSource = Me.BindingSourcePort
        Me.DataGridViewPort.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewPort.Location = New System.Drawing.Point(337, 224)
        Me.DataGridViewPort.Name = "DataGridViewPort"
        Me.DataGridViewPort.Size = New System.Drawing.Size(328, 68)
        Me.DataGridViewPort.TabIndex = 38
        '
        'KeyПортаDataGridViewTextBoxColumn
        '
        Me.KeyПортаDataGridViewTextBoxColumn.DataPropertyName = "KeyПорта"
        Me.KeyПортаDataGridViewTextBoxColumn.HeaderText = "KeyПорта"
        Me.KeyПортаDataGridViewTextBoxColumn.Name = "KeyПортаDataGridViewTextBoxColumn"
        '
        'KeyДействиеDataGridViewTextBoxColumn2
        '
        Me.KeyДействиеDataGridViewTextBoxColumn2.DataPropertyName = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn2.HeaderText = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn2.Name = "KeyДействиеDataGridViewTextBoxColumn2"
        '
        'NameDataGridViewTextBoxColumn2
        '
        Me.NameDataGridViewTextBoxColumn2.DataPropertyName = "name"
        Me.NameDataGridViewTextBoxColumn2.HeaderText = "name"
        Me.NameDataGridViewTextBoxColumn2.Name = "NameDataGridViewTextBoxColumn2"
        '
        'НомерУстройстваDataGridViewTextBoxColumn
        '
        Me.НомерУстройстваDataGridViewTextBoxColumn.DataPropertyName = "НомерУстройства"
        Me.НомерУстройстваDataGridViewTextBoxColumn.HeaderText = "НомерУстройства"
        Me.НомерУстройстваDataGridViewTextBoxColumn.Name = "НомерУстройстваDataGridViewTextBoxColumn"
        '
        'НомерМодуляКорзиныDataGridViewTextBoxColumn
        '
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.DataPropertyName = "НомерМодуляКорзины"
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.HeaderText = "НомерМодуляКорзины"
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.Name = "НомерМодуляКорзиныDataGridViewTextBoxColumn"
        '
        'НомерПортаDataGridViewTextBoxColumn
        '
        Me.НомерПортаDataGridViewTextBoxColumn.DataPropertyName = "НомерПорта"
        Me.НомерПортаDataGridViewTextBoxColumn.HeaderText = "НомерПорта"
        Me.НомерПортаDataGridViewTextBoxColumn.Name = "НомерПортаDataGridViewTextBoxColumn"
        '
        'GridViewConfiguration
        '
        Me.GridViewConfiguration.AllowUserToAddRows = False
        Me.GridViewConfiguration.AllowUserToDeleteRows = False
        Me.GridViewConfiguration.AutoGenerateColumns = False
        Me.GridViewConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewConfiguration.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn1, Me.ИмяКонфигурацииDataGridViewTextBoxColumn, Me.ОписаниеDataGridViewTextBoxColumn})
        Me.GridViewConfiguration.DataSource = Me.BindingSourceConfiguration
        Me.GridViewConfiguration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridViewConfiguration.Location = New System.Drawing.Point(3, 28)
        Me.GridViewConfiguration.Name = "GridViewConfiguration"
        Me.GridViewConfiguration.Size = New System.Drawing.Size(328, 67)
        Me.GridViewConfiguration.TabIndex = 0
        '
        'KeyКонфигурацияДействияDataGridViewTextBoxColumn1
        '
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn1.DataPropertyName = "keyКонфигурацияДействия"
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn1.HeaderText = "keyКонфигурацияДействия"
        Me.KeyКонфигурацияДействияDataGridViewTextBoxColumn1.Name = "KeyКонфигурацияДействияDataGridViewTextBoxColumn1"
        '
        'ИмяКонфигурацииDataGridViewTextBoxColumn
        '
        Me.ИмяКонфигурацииDataGridViewTextBoxColumn.DataPropertyName = "ИмяКонфигурации"
        Me.ИмяКонфигурацииDataGridViewTextBoxColumn.HeaderText = "ИмяКонфигурации"
        Me.ИмяКонфигурацииDataGridViewTextBoxColumn.Name = "ИмяКонфигурацииDataGridViewTextBoxColumn"
        '
        'ОписаниеDataGridViewTextBoxColumn
        '
        Me.ОписаниеDataGridViewTextBoxColumn.DataPropertyName = "Описание"
        Me.ОписаниеDataGridViewTextBoxColumn.HeaderText = "Описание"
        Me.ОписаниеDataGridViewTextBoxColumn.Name = "ОписаниеDataGridViewTextBoxColumn"
        '
        'DataGridViewFormula
        '
        Me.DataGridViewFormula.AllowUserToAddRows = False
        Me.DataGridViewFormula.AllowUserToDeleteRows = False
        Me.DataGridViewFormula.AutoGenerateColumns = False
        Me.DataGridViewFormula.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewFormula.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KeyFormulaDataGridViewTextBoxColumn1, Me.KeyДействиеDataGridViewTextBoxColumn1, Me.NameDataGridViewTextBoxColumn1, Me.ФормулаDataGridViewTextBoxColumn, Me.ОперацияСравненияDataGridViewTextBoxColumn1, Me.ВеличинаУсловияDataGridViewTextBoxColumn1})
        Me.DataGridViewFormula.DataSource = Me.BindingSourceFormula
        Me.DataGridViewFormula.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewFormula.Location = New System.Drawing.Point(337, 126)
        Me.DataGridViewFormula.Name = "DataGridViewFormula"
        Me.DataGridViewFormula.Size = New System.Drawing.Size(328, 67)
        Me.DataGridViewFormula.TabIndex = 37
        '
        'KeyFormulaDataGridViewTextBoxColumn1
        '
        Me.KeyFormulaDataGridViewTextBoxColumn1.DataPropertyName = "KeyFormula"
        Me.KeyFormulaDataGridViewTextBoxColumn1.HeaderText = "KeyFormula"
        Me.KeyFormulaDataGridViewTextBoxColumn1.Name = "KeyFormulaDataGridViewTextBoxColumn1"
        '
        'KeyДействиеDataGridViewTextBoxColumn1
        '
        Me.KeyДействиеDataGridViewTextBoxColumn1.DataPropertyName = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn1.HeaderText = "keyДействие"
        Me.KeyДействиеDataGridViewTextBoxColumn1.Name = "KeyДействиеDataGridViewTextBoxColumn1"
        '
        'NameDataGridViewTextBoxColumn1
        '
        Me.NameDataGridViewTextBoxColumn1.DataPropertyName = "name"
        Me.NameDataGridViewTextBoxColumn1.HeaderText = "name"
        Me.NameDataGridViewTextBoxColumn1.Name = "NameDataGridViewTextBoxColumn1"
        '
        'ФормулаDataGridViewTextBoxColumn
        '
        Me.ФормулаDataGridViewTextBoxColumn.DataPropertyName = "Формула"
        Me.ФормулаDataGridViewTextBoxColumn.HeaderText = "Формула"
        Me.ФормулаDataGridViewTextBoxColumn.Name = "ФормулаDataGridViewTextBoxColumn"
        '
        'ОперацияСравненияDataGridViewTextBoxColumn1
        '
        Me.ОперацияСравненияDataGridViewTextBoxColumn1.DataPropertyName = "ОперацияСравнения"
        Me.ОперацияСравненияDataGridViewTextBoxColumn1.HeaderText = "ОперацияСравнения"
        Me.ОперацияСравненияDataGridViewTextBoxColumn1.Name = "ОперацияСравненияDataGridViewTextBoxColumn1"
        '
        'ВеличинаУсловияDataGridViewTextBoxColumn1
        '
        Me.ВеличинаУсловияDataGridViewTextBoxColumn1.DataPropertyName = "ВеличинаУсловия"
        Me.ВеличинаУсловияDataGridViewTextBoxColumn1.HeaderText = "ВеличинаУсловия"
        Me.ВеличинаУсловияDataGridViewTextBoxColumn1.Name = "ВеличинаУсловияDataGridViewTextBoxColumn1"
        '
        'BindingNavigatorДействие
        '
        Me.BindingNavigatorДействие.AddNewItem = Nothing
        Me.BindingNavigatorДействие.BindingSource = Me.BindingSourceAction
        Me.BindingNavigatorДействие.CountItem = Me.ToolStripLabel8
        Me.BindingNavigatorДействие.DeleteItem = Nothing
        Me.BindingNavigatorДействие.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BindingNavigatorДействие.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ДействиеBNMoveFirstItem, Me.ДействиеBNMovePreviousItem, Me.ToolStripSeparator27, Me.ДействиеBNPositionItem, Me.ToolStripLabel8, Me.ToolStripSeparator28, Me.ДействиеBNMoveNextItem, Me.ДействиеNMoveLastItem, Me.ToolStripSeparator29, Me.ДействиеBNAddNewItem, Me.TSButtonActionDelete, Me.TSButtonActionSave})
        Me.BindingNavigatorДействие.Location = New System.Drawing.Point(0, 98)
        Me.BindingNavigatorДействие.MoveFirstItem = Me.ДействиеBNMoveFirstItem
        Me.BindingNavigatorДействие.MoveLastItem = Me.ДействиеNMoveLastItem
        Me.BindingNavigatorДействие.MoveNextItem = Me.ДействиеBNMoveNextItem
        Me.BindingNavigatorДействие.MovePreviousItem = Me.ДействиеBNMovePreviousItem
        Me.BindingNavigatorДействие.Name = "BindingNavigatorДействие"
        Me.BindingNavigatorДействие.PositionItem = Me.ДействиеBNPositionItem
        Me.BindingNavigatorДействие.Size = New System.Drawing.Size(334, 25)
        Me.BindingNavigatorДействие.TabIndex = 39
        Me.BindingNavigatorДействие.Text = "BindingNavigator1"
        '
        'ToolStripLabel8
        '
        Me.ToolStripLabel8.Name = "ToolStripLabel8"
        Me.ToolStripLabel8.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel8.Text = "для {0}"
        Me.ToolStripLabel8.ToolTipText = "Total number of items"
        '
        'ДействиеBNMoveFirstItem
        '
        Me.ДействиеBNMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ДействиеBNMoveFirstItem.Image = CType(resources.GetObject("ДействиеBNMoveFirstItem.Image"), System.Drawing.Image)
        Me.ДействиеBNMoveFirstItem.Name = "ДействиеBNMoveFirstItem"
        Me.ДействиеBNMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.ДействиеBNMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.ДействиеBNMoveFirstItem.Text = "Move first"
        '
        'ДействиеBNMovePreviousItem
        '
        Me.ДействиеBNMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ДействиеBNMovePreviousItem.Image = CType(resources.GetObject("ДействиеBNMovePreviousItem.Image"), System.Drawing.Image)
        Me.ДействиеBNMovePreviousItem.Name = "ДействиеBNMovePreviousItem"
        Me.ДействиеBNMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.ДействиеBNMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.ДействиеBNMovePreviousItem.Text = "Move previous"
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(6, 25)
        '
        'ДействиеBNPositionItem
        '
        Me.ДействиеBNPositionItem.AccessibleName = "Position"
        Me.ДействиеBNPositionItem.AutoSize = False
        Me.ДействиеBNPositionItem.Name = "ДействиеBNPositionItem"
        Me.ДействиеBNPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.ДействиеBNPositionItem.Text = "0"
        Me.ДействиеBNPositionItem.ToolTipText = "Current position"
        '
        'ToolStripSeparator28
        '
        Me.ToolStripSeparator28.Name = "ToolStripSeparator28"
        Me.ToolStripSeparator28.Size = New System.Drawing.Size(6, 25)
        '
        'ДействиеBNMoveNextItem
        '
        Me.ДействиеBNMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ДействиеBNMoveNextItem.Image = CType(resources.GetObject("ДействиеBNMoveNextItem.Image"), System.Drawing.Image)
        Me.ДействиеBNMoveNextItem.Name = "ДействиеBNMoveNextItem"
        Me.ДействиеBNMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.ДействиеBNMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.ДействиеBNMoveNextItem.Text = "Move next"
        '
        'ДействиеNMoveLastItem
        '
        Me.ДействиеNMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ДействиеNMoveLastItem.Image = CType(resources.GetObject("ДействиеNMoveLastItem.Image"), System.Drawing.Image)
        Me.ДействиеNMoveLastItem.Name = "ДействиеNMoveLastItem"
        Me.ДействиеNMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.ДействиеNMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.ДействиеNMoveLastItem.Text = "Move last"
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(6, 25)
        '
        'ДействиеBNAddNewItem
        '
        Me.ДействиеBNAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ДействиеBNAddNewItem.Enabled = False
        Me.ДействиеBNAddNewItem.Image = CType(resources.GetObject("ДействиеBNAddNewItem.Image"), System.Drawing.Image)
        Me.ДействиеBNAddNewItem.Name = "ДействиеBNAddNewItem"
        Me.ДействиеBNAddNewItem.RightToLeftAutoMirrorImage = True
        Me.ДействиеBNAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.ДействиеBNAddNewItem.Text = "Add new"
        '
        'TSButtonActionDelete
        '
        Me.TSButtonActionDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonActionDelete.Enabled = False
        Me.TSButtonActionDelete.Image = CType(resources.GetObject("TSButtonActionDelete.Image"), System.Drawing.Image)
        Me.TSButtonActionDelete.Name = "TSButtonActionDelete"
        Me.TSButtonActionDelete.RightToLeftAutoMirrorImage = True
        Me.TSButtonActionDelete.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonActionDelete.Text = "Delete"
        '
        'TSButtonActionSave
        '
        Me.TSButtonActionSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSButtonActionSave.Enabled = False
        Me.TSButtonActionSave.Image = CType(resources.GetObject("TSButtonActionSave.Image"), System.Drawing.Image)
        Me.TSButtonActionSave.Name = "TSButtonActionSave"
        Me.TSButtonActionSave.Size = New System.Drawing.Size(23, 22)
        Me.TSButtonActionSave.Text = "Save Data"
        '
        'TabPageDevice
        '
        Me.TabPageDevice.Controls.Add(Me.Label8)
        Me.TabPageDevice.Controls.Add(Me.GroupBoxPort)
        Me.TabPageDevice.Controls.Add(Me.automaticScaleModeGroupBox)
        Me.TabPageDevice.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPageDevice.ImageIndex = 3
        Me.TabPageDevice.Location = New System.Drawing.Point(4, 23)
        Me.TabPageDevice.Name = "TabPageDevice"
        Me.TabPageDevice.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageDevice.Size = New System.Drawing.Size(1004, 295)
        Me.TabPageDevice.TabIndex = 4
        Me.TabPageDevice.Text = "Устройства и порты"
        Me.TabPageDevice.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Location = New System.Drawing.Point(3, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(998, 23)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Текущая конфигурация оборудования"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBoxPort
        '
        Me.GroupBoxPort.Controls.Add(Me.Label7)
        Me.GroupBoxPort.Controls.Add(Me.ComboDigitalWriteTask)
        Me.GroupBoxPort.Controls.Add(Me.ComboBoxLineCount)
        Me.GroupBoxPort.Controls.Add(Me.Label6)
        Me.GroupBoxPort.Controls.Add(Me.Label1)
        Me.GroupBoxPort.Controls.Add(Me.Label3)
        Me.GroupBoxPort.Controls.Add(Me.physicalChannelComboBox)
        Me.GroupBoxPort.Controls.Add(Me.ComboBoxPhysicalPort)
        Me.GroupBoxPort.Controls.Add(Me.deviceComboBox)
        Me.GroupBoxPort.Location = New System.Drawing.Point(6, 29)
        Me.GroupBoxPort.Name = "GroupBoxPort"
        Me.GroupBoxPort.Size = New System.Drawing.Size(294, 131)
        Me.GroupBoxPort.TabIndex = 5
        Me.GroupBoxPort.TabStop = False
        Me.GroupBoxPort.Text = "Существующие устройства и порты"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 99)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(116, 13)
        Me.Label7.TabIndex = 120
        Me.Label7.Text = "Разрешенные задачи"
        '
        'ComboDigitalWriteTask
        '
        Me.ComboDigitalWriteTask.Location = New System.Drawing.Point(158, 99)
        Me.ComboDigitalWriteTask.Name = "ComboDigitalWriteTask"
        Me.ComboDigitalWriteTask.Size = New System.Drawing.Size(129, 21)
        Me.ComboDigitalWriteTask.TabIndex = 119
        '
        'ComboBoxLineCount
        '
        Me.ComboBoxLineCount.Location = New System.Drawing.Point(108, 46)
        Me.ComboBoxLineCount.Name = "ComboBoxLineCount"
        Me.ComboBoxLineCount.Size = New System.Drawing.Size(44, 21)
        Me.ComboBoxLineCount.TabIndex = 118
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 13)
        Me.Label6.TabIndex = 116
        Me.Label6.Text = "Физические каналы"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.TabIndex = 115
        Me.Label1.Text = "Физические порты"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(185, 27)
        Me.Label3.TabIndex = 114
        Me.Label3.Text = "Имя устройства дискретной платы управления"
        '
        'physicalChannelComboBox
        '
        Me.physicalChannelComboBox.Location = New System.Drawing.Point(158, 73)
        Me.physicalChannelComboBox.Name = "physicalChannelComboBox"
        Me.physicalChannelComboBox.Size = New System.Drawing.Size(129, 21)
        Me.physicalChannelComboBox.TabIndex = 113
        '
        'ComboBoxPhysicalPort
        '
        Me.ComboBoxPhysicalPort.Location = New System.Drawing.Point(158, 46)
        Me.ComboBoxPhysicalPort.Name = "ComboBoxPhysicalPort"
        Me.ComboBoxPhysicalPort.Size = New System.Drawing.Size(129, 21)
        Me.ComboBoxPhysicalPort.TabIndex = 112
        '
        'deviceComboBox
        '
        Me.deviceComboBox.Location = New System.Drawing.Point(224, 16)
        Me.deviceComboBox.Name = "deviceComboBox"
        Me.deviceComboBox.Size = New System.Drawing.Size(63, 21)
        Me.deviceComboBox.TabIndex = 111
        '
        'automaticScaleModeGroupBox
        '
        Me.automaticScaleModeGroupBox.Controls.Add(Me.scaleModeLabel)
        Me.automaticScaleModeGroupBox.Controls.Add(Me.ScaleModePropertyEditor)
        Me.automaticScaleModeGroupBox.Controls.Add(Me.automaticScaleModePanel)
        Me.automaticScaleModeGroupBox.Location = New System.Drawing.Point(306, 29)
        Me.automaticScaleModeGroupBox.Name = "automaticScaleModeGroupBox"
        Me.automaticScaleModeGroupBox.Size = New System.Drawing.Size(240, 209)
        Me.automaticScaleModeGroupBox.TabIndex = 19
        Me.automaticScaleModeGroupBox.TabStop = False
        Me.automaticScaleModeGroupBox.Text = "Automatic Scale Mode Settings"
        '
        'scaleModeLabel
        '
        Me.scaleModeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scaleModeLabel.AutoSize = True
        Me.scaleModeLabel.Location = New System.Drawing.Point(6, 18)
        Me.scaleModeLabel.Name = "scaleModeLabel"
        Me.scaleModeLabel.Size = New System.Drawing.Size(67, 13)
        Me.scaleModeLabel.TabIndex = 17
        Me.scaleModeLabel.Text = "Scale Mode:"
        '
        'ScaleModePropertyEditor
        '
        Me.ScaleModePropertyEditor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScaleModePropertyEditor.Location = New System.Drawing.Point(90, 15)
        Me.ScaleModePropertyEditor.Name = "ScaleModePropertyEditor"
        Me.ScaleModePropertyEditor.Size = New System.Drawing.Size(120, 20)
        Me.ScaleModePropertyEditor.Source = New NationalInstruments.UI.PropertyEditorSource(Me.ScalingSwitchArray, "ScaleMode")
        Me.ScaleModePropertyEditor.TabIndex = 16
        '
        'ScalingSwitchArray
        '
        Me.ScalingSwitchArray.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.ScalingSwitchArray.ItemTemplate.Location = New System.Drawing.Point(0, 0)
        Me.ScalingSwitchArray.ItemTemplate.Name = ""
        Me.ScalingSwitchArray.ItemTemplate.Size = New System.Drawing.Size(48, 80)
        Me.ScalingSwitchArray.ItemTemplate.SwitchStyle = NationalInstruments.UI.SwitchStyle.VerticalToggle3D
        Me.ScalingSwitchArray.ItemTemplate.TabIndex = 0
        Me.ScalingSwitchArray.ItemTemplate.TabStop = False
        Me.ScalingSwitchArray.LayoutMode = NationalInstruments.UI.ControlArrayLayoutMode.Horizontal
        Me.ScalingSwitchArray.Location = New System.Drawing.Point(6, 115)
        Me.ScalingSwitchArray.Name = "ScalingSwitchArray"
        Me.ScalingSwitchArray.Size = New System.Drawing.Size(427, 86)
        Me.ScalingSwitchArray.TabIndex = 4
        '
        'automaticScaleModePanel
        '
        Me.automaticScaleModePanel.Controls.Add(Me.selectValueLabel)
        Me.automaticScaleModePanel.Controls.Add(Me.valuesListBox)
        Me.automaticScaleModePanel.Controls.Add(Me.ButtonRemove)
        Me.automaticScaleModePanel.Controls.Add(Me.valuesLabel)
        Me.automaticScaleModePanel.Controls.Add(Me.ButtonAdd)
        Me.automaticScaleModePanel.Controls.Add(Me.booleanComboBox)
        Me.automaticScaleModePanel.Location = New System.Drawing.Point(6, 41)
        Me.automaticScaleModePanel.Name = "automaticScaleModePanel"
        Me.automaticScaleModePanel.Size = New System.Drawing.Size(228, 162)
        Me.automaticScaleModePanel.TabIndex = 15
        '
        'selectValueLabel
        '
        Me.selectValueLabel.AutoSize = True
        Me.selectValueLabel.Location = New System.Drawing.Point(9, 4)
        Me.selectValueLabel.Name = "selectValueLabel"
        Me.selectValueLabel.Size = New System.Drawing.Size(70, 13)
        Me.selectValueLabel.TabIndex = 14
        Me.selectValueLabel.Text = "Select Value:"
        '
        'valuesListBox
        '
        Me.valuesListBox.FormattingEnabled = True
        Me.valuesListBox.Location = New System.Drawing.Point(12, 65)
        Me.valuesListBox.Name = "valuesListBox"
        Me.valuesListBox.Size = New System.Drawing.Size(120, 82)
        Me.valuesListBox.TabIndex = 9
        '
        'ButtonRemove
        '
        Me.ButtonRemove.Location = New System.Drawing.Point(139, 65)
        Me.ButtonRemove.Name = "ButtonRemove"
        Me.ButtonRemove.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRemove.TabIndex = 13
        Me.ButtonRemove.Text = "Remove"
        Me.ButtonRemove.UseVisualStyleBackColor = True
        '
        'valuesLabel
        '
        Me.valuesLabel.AutoSize = True
        Me.valuesLabel.Location = New System.Drawing.Point(9, 48)
        Me.valuesLabel.Name = "valuesLabel"
        Me.valuesLabel.Size = New System.Drawing.Size(42, 13)
        Me.valuesLabel.TabIndex = 10
        Me.valuesLabel.Text = "Values:"
        '
        'ButtonAdd
        '
        Me.ButtonAdd.Location = New System.Drawing.Point(139, 18)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAdd.TabIndex = 12
        Me.ButtonAdd.Text = "Add"
        Me.ButtonAdd.UseVisualStyleBackColor = True
        '
        'booleanComboBox
        '
        Me.booleanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.booleanComboBox.FormattingEnabled = True
        Me.booleanComboBox.Items.AddRange(New Object() {"True", "False"})
        Me.booleanComboBox.Location = New System.Drawing.Point(12, 21)
        Me.booleanComboBox.Name = "booleanComboBox"
        Me.booleanComboBox.Size = New System.Drawing.Size(121, 21)
        Me.booleanComboBox.TabIndex = 11
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.NumericEditArray2, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ScalingSwitchArray, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.scalingLedArray, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.NumericEditArray1, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(552, 29)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(439, 245)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'NumericEditArray2
        '
        Me.NumericEditArray2.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.NumericEditArray2.ItemTemplate.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0)
        Me.NumericEditArray2.ItemTemplate.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Text
        Me.NumericEditArray2.ItemTemplate.Location = New System.Drawing.Point(0, 0)
        Me.NumericEditArray2.ItemTemplate.Name = ""
        Me.NumericEditArray2.ItemTemplate.Range = New NationalInstruments.UI.Range(0R, 31.0R)
        Me.NumericEditArray2.ItemTemplate.Size = New System.Drawing.Size(48, 20)
        Me.NumericEditArray2.ItemTemplate.TabIndex = 0
        Me.NumericEditArray2.ItemTemplate.TabStop = False
        Me.NumericEditArray2.ItemTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.NumericEditArray2.LayoutMode = NationalInstruments.UI.ControlArrayLayoutMode.Horizontal
        Me.NumericEditArray2.Location = New System.Drawing.Point(6, 210)
        Me.NumericEditArray2.Name = "NumericEditArray2"
        Me.NumericEditArray2.Size = New System.Drawing.Size(427, 29)
        Me.NumericEditArray2.TabIndex = 8
        '
        'scalingLedArray
        '
        Me.scalingLedArray.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.scalingLedArray.ItemTemplate.LedStyle = NationalInstruments.UI.LedStyle.Round3D
        Me.scalingLedArray.ItemTemplate.Location = New System.Drawing.Point(0, 0)
        Me.scalingLedArray.ItemTemplate.Name = ""
        Me.scalingLedArray.ItemTemplate.Size = New System.Drawing.Size(48, 48)
        Me.scalingLedArray.ItemTemplate.TabIndex = 0
        Me.scalingLedArray.ItemTemplate.TabStop = False
        Me.scalingLedArray.LayoutMode = NationalInstruments.UI.ControlArrayLayoutMode.Horizontal
        Me.scalingLedArray.Location = New System.Drawing.Point(6, 43)
        Me.scalingLedArray.Name = "scalingLedArray"
        Me.scalingLedArray.Size = New System.Drawing.Size(427, 63)
        Me.scalingLedArray.TabIndex = 5
        '
        'NumericEditArray1
        '
        Me.NumericEditArray1.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.NumericEditArray1.ItemTemplate.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0)
        Me.NumericEditArray1.ItemTemplate.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Text
        Me.NumericEditArray1.ItemTemplate.Location = New System.Drawing.Point(0, 0)
        Me.NumericEditArray1.ItemTemplate.Name = ""
        Me.NumericEditArray1.ItemTemplate.Range = New NationalInstruments.UI.Range(0R, 31.0R)
        Me.NumericEditArray1.ItemTemplate.Size = New System.Drawing.Size(48, 20)
        Me.NumericEditArray1.ItemTemplate.TabIndex = 0
        Me.NumericEditArray1.ItemTemplate.TabStop = False
        Me.NumericEditArray1.ItemTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.NumericEditArray1.LayoutMode = NationalInstruments.UI.ControlArrayLayoutMode.Horizontal
        Me.NumericEditArray1.Location = New System.Drawing.Point(6, 6)
        Me.NumericEditArray1.Name = "NumericEditArray1"
        Me.NumericEditArray1.Size = New System.Drawing.Size(427, 28)
        Me.NumericEditArray1.TabIndex = 6
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "kate.png")
        Me.ImageList1.Images.SetKeyName(1, "view_tree.png")
        Me.ImageList1.Images.SetKeyName(2, "viewmag+.png")
        Me.ImageList1.Images.SetKeyName(3, "configure.png")
        '
        'tsContainer
        '
        '
        'tsContainer.BottomToolStripPanel
        '
        Me.tsContainer.BottomToolStripPanel.Controls.Add(Me.MainStatusStrip)
        '
        'tsContainer.ContentPanel
        '
        Me.tsContainer.ContentPanel.Controls.Add(Me.SplitContainerGlobal)
        Me.tsContainer.ContentPanel.Size = New System.Drawing.Size(1016, 440)
        Me.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tsContainer.Location = New System.Drawing.Point(0, 0)
        Me.tsContainer.Name = "tsContainer"
        Me.tsContainer.Size = New System.Drawing.Size(1016, 534)
        Me.tsContainer.TabIndex = 28
        Me.tsContainer.Text = "ToolStripContainer1"
        '
        'tsContainer.TopToolStripPanel
        '
        Me.tsContainer.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        Me.tsContainer.TopToolStripPanel.Controls.Add(Me.ToolBar1)
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Status, Me.ПанельУстановка})
        Me.MainStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.Size = New System.Drawing.Size(1016, 24)
        Me.MainStatusStrip.TabIndex = 32
        '
        'Status
        '
        Me.Status.AutoSize = False
        Me.Status.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Status.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.Status.Image = CType(resources.GetObject("Status.Image"), System.Drawing.Image)
        Me.Status.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(919, 19)
        Me.Status.Spring = True
        Me.Status.Text = "Готов"
        Me.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ПанельУстановка
        '
        Me.ПанельУстановка.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ПанельУстановка.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.ПанельУстановка.Name = "ПанельУстановка"
        Me.ПанельУстановка.Size = New System.Drawing.Size(82, 19)
        Me.ПанельУстановка.Text = "Установка 11"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.mnuWindow})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.MdiWindowListItem = Me.mnuWindow
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.ShowItemToolTips = True
        Me.MenuStrip1.Size = New System.Drawing.Size(1016, 24)
        Me.MenuStrip1.TabIndex = 29
        Me.MenuStrip1.Text = "MenuStrip"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.TSMenuItemStart, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.FileMenu.MergeIndex = 1
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(136, 20)
        Me.FileMenu.Text = "&Управление портами"
        Me.FileMenu.ToolTipText = "Программа"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.SaveToolStripMenuItem.Text = "&Сохранить изменения"
        Me.SaveToolStripMenuItem.ToolTipText = "Сохранить изменения в базе данных"
        '
        'TSMenuItemStart
        '
        Me.TSMenuItemStart.CheckOnClick = True
        Me.TSMenuItemStart.Enabled = False
        Me.TSMenuItemStart.Image = CType(resources.GetObject("TSMenuItemStart.Image"), System.Drawing.Image)
        Me.TSMenuItemStart.Name = "TSMenuItemStart"
        Me.TSMenuItemStart.Size = New System.Drawing.Size(195, 22)
        Me.TSMenuItemStart.Text = "&Запуск цикла"
        Me.TSMenuItemStart.ToolTipText = "Запуск наблюдения за событиями"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(192, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = CType(resources.GetObject("ExitToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.ExitToolStripMenuItem.Text = "&Выход"
        Me.ExitToolStripMenuItem.ToolTipText = "Выход"
        '
        'mnuWindow
        '
        Me.mnuWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuWindowCascade, Me.mnuWindowTileHorizontal, Me.mnuWindowTileVertical})
        Me.mnuWindow.Name = "mnuWindow"
        Me.mnuWindow.Size = New System.Drawing.Size(48, 20)
        Me.mnuWindow.Text = "&Окно"
        Me.mnuWindow.Visible = False
        '
        'mnuWindowCascade
        '
        Me.mnuWindowCascade.Image = CType(resources.GetObject("mnuWindowCascade.Image"), System.Drawing.Image)
        Me.mnuWindowCascade.Name = "mnuWindowCascade"
        Me.mnuWindowCascade.Size = New System.Drawing.Size(158, 22)
        Me.mnuWindowCascade.Text = "&Каскад"
        '
        'mnuWindowTileHorizontal
        '
        Me.mnuWindowTileHorizontal.Image = CType(resources.GetObject("mnuWindowTileHorizontal.Image"), System.Drawing.Image)
        Me.mnuWindowTileHorizontal.Name = "mnuWindowTileHorizontal"
        Me.mnuWindowTileHorizontal.Size = New System.Drawing.Size(158, 22)
        Me.mnuWindowTileHorizontal.Text = "&Горизонтально"
        '
        'mnuWindowTileVertical
        '
        Me.mnuWindowTileVertical.Image = CType(resources.GetObject("mnuWindowTileVertical.Image"), System.Drawing.Image)
        Me.mnuWindowTileVertical.Name = "mnuWindowTileVertical"
        Me.mnuWindowTileVertical.Size = New System.Drawing.Size(158, 22)
        Me.mnuWindowTileVertical.Text = "&Вертикально"
        '
        'ToolBar1
        '
        Me.ToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolBar1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSButtonStart, Me.TSButtonInterruptObservation, Me.ToolStripSeparator2, Me.TSButtonHandCheck})
        Me.ToolBar1.Location = New System.Drawing.Point(3, 24)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.Size = New System.Drawing.Size(312, 46)
        Me.ToolBar1.TabIndex = 36
        '
        'TSButtonStart
        '
        Me.TSButtonStart.CheckOnClick = True
        Me.TSButtonStart.Enabled = False
        Me.TSButtonStart.Image = CType(resources.GetObject("TSButtonStart.Image"), System.Drawing.Image)
        Me.TSButtonStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonStart.Name = "TSButtonStart"
        Me.TSButtonStart.Size = New System.Drawing.Size(110, 43)
        Me.TSButtonStart.Tag = "Пуск"
        Me.TSButtonStart.Text = "Пуск наблюдения"
        Me.TSButtonStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TSButtonStart.ToolTipText = "Пуск наблюдения за событиями"
        '
        'TSButtonInterruptObservation
        '
        Me.TSButtonInterruptObservation.Enabled = False
        Me.TSButtonInterruptObservation.Image = CType(resources.GetObject("TSButtonInterruptObservation.Image"), System.Drawing.Image)
        Me.TSButtonInterruptObservation.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonInterruptObservation.Name = "TSButtonInterruptObservation"
        Me.TSButtonInterruptObservation.Size = New System.Drawing.Size(63, 43)
        Me.TSButtonInterruptObservation.Tag = "Прервать"
        Me.TSButtonInterruptObservation.Text = "Прервать"
        Me.TSButtonInterruptObservation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TSButtonInterruptObservation.ToolTipText = "Прервать наблюдения за событиями"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 46)
        '
        'TSButtonHandCheck
        '
        Me.TSButtonHandCheck.CheckOnClick = True
        Me.TSButtonHandCheck.Image = CType(resources.GetObject("TSButtonHandCheck.Image"), System.Drawing.Image)
        Me.TSButtonHandCheck.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonHandCheck.Name = "TSButtonHandCheck"
        Me.TSButtonHandCheck.Size = New System.Drawing.Size(121, 43)
        Me.TSButtonHandCheck.Text = "Ручное исполнение"
        Me.TSButtonHandCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TSButtonHandCheck.ToolTipText = "Ручное исполнение механизма на линии порта"
        '
        'TimerUpdate
        '
        Me.TimerUpdate.Interval = 1000
        '
        'ErrorProviderБит
        '
        Me.ErrorProviderБит.ContainerControl = Me
        Me.ErrorProviderБит.DataSource = Me.BindingSourceBit
        '
        'ErrorProviderПорт
        '
        Me.ErrorProviderПорт.ContainerControl = Me
        Me.ErrorProviderПорт.DataSource = Me.BindingSourcePort
        '
        'ErrorProviderАргумент
        '
        Me.ErrorProviderАргумент.ContainerControl = Me
        Me.ErrorProviderАргумент.DataSource = Me.BindingSourceАргумент
        '
        'ErrorProviderФормула
        '
        Me.ErrorProviderФормула.ContainerControl = Me
        Me.ErrorProviderФормула.DataSource = Me.BindingSourceFormula
        '
        'ErrorProviderТриггер
        '
        Me.ErrorProviderТриггер.ContainerControl = Me
        Me.ErrorProviderТриггер.DataSource = Me.BindingSourceTrigger
        '
        'ErrorProviderДействие
        '
        Me.ErrorProviderДействие.ContainerControl = Me
        Me.ErrorProviderДействие.DataSource = Me.BindingSourceAction
        '
        'ErrorProviderКонфигурация
        '
        Me.ErrorProviderКонфигурация.ContainerControl = Me
        Me.ErrorProviderКонфигурация.DataSource = Me.BindingSourceConfiguration
        '
        'ActionTableAdapter
        '
        Me.ActionTableAdapter.ClearBeforeFill = True
        '
        'ConfigurationTableAdapter
        '
        Me.ConfigurationTableAdapter.ClearBeforeFill = True
        '
        'TriggerFireDigitalOutputTableAdapter
        '
        Me.TriggerFireDigitalOutputTableAdapter.ClearBeforeFill = True
        '
        'FormulaFireDigitalOutputTableAdapter
        '
        Me.FormulaFireDigitalOutputTableAdapter.ClearBeforeFill = True
        '
        'ArgumentsOfFormulaTableAdapter
        '
        Me.ArgumentsOfFormulaTableAdapter.ClearBeforeFill = True
        '
        'PortsTableAdapter
        '
        Me.PortsTableAdapter.ClearBeforeFill = True
        '
        'BitsOfPortTableAdapter
        '
        Me.BitsOfPortTableAdapter.ClearBeforeFill = True
        '
        'ImageListTree
        '
        Me.ImageListTree.ImageStream = CType(resources.GetObject("ImageListTree.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTree.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTree.Images.SetKeyName(0, "DIAdiademLogo.bmp")
        Me.ImageListTree.Images.SetKeyName(1, "Header_Generate.png")
        Me.ImageListTree.Images.SetKeyName(2, "alarmd.png")
        Me.ImageListTree.Images.SetKeyName(3, "jabber_serv_on.png")
        Me.ImageListTree.Images.SetKeyName(4, "math_int.png")
        Me.ImageListTree.Images.SetKeyName(5, "math_paren.png")
        Me.ImageListTree.Images.SetKeyName(6, "connect_creating.png")
        Me.ImageListTree.Images.SetKeyName(7, "daemons.png")
        Me.ImageListTree.Images.SetKeyName(8, "Header_Generate2.png")
        Me.ImageListTree.Images.SetKeyName(9, "alarmd2.png")
        Me.ImageListTree.Images.SetKeyName(10, "jabber_serv_on2.png")
        Me.ImageListTree.Images.SetKeyName(11, "math_int2.png")
        Me.ImageListTree.Images.SetKeyName(12, "math_paren2.png")
        Me.ImageListTree.Images.SetKeyName(13, "connect_creating2.png")
        Me.ImageListTree.Images.SetKeyName(14, "daemons2.png")
        '
        'TimerResize
        '
        Me.TimerResize.Interval = 2000
        '
        'FormDigitalPort
        '
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1016, 534)
        Me.Controls.Add(Me.tsContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormDigitalPort"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Управление выходными портами"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.SplitContainerGlobal.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGlobal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGlobal.ResumeLayout(False)
        Me.TabControlConfig.ResumeLayout(False)
        Me.TabPageWatch.ResumeLayout(False)
        Me.SplitContainerОтборКонигураций.Panel1.ResumeLayout(False)
        Me.SplitContainerОтборКонигураций.Panel1.PerformLayout()
        Me.SplitContainerОтборКонигураций.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerОтборКонигураций, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerОтборКонигураций.ResumeLayout(False)
        Me.ToolStrip6.ResumeLayout(False)
        Me.ToolStrip6.PerformLayout()
        Me.TabPageEdit.ResumeLayout(False)
        Me.TabPageEdit.PerformLayout()
        CType(Me.SplitContainerExplorer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerExplorer.ResumeLayout(False)
        Me.InstrumentControlStrip1.ResumeLayout(False)
        Me.InstrumentControlStrip1.PerformLayout()
        Me.TabPageBases.ResumeLayout(False)
        Me.TableLayoutPanelDataSet.ResumeLayout(False)
        Me.TableLayoutPanelDataSet.PerformLayout()
        CType(Me.DataGridViewБит, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceBit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelsDigitalOutputDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewАргумент, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceАргумент, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewТриггер, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceTrigger, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorТриггер, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorТриггер.ResumeLayout(False)
        Me.BindingNavigatorТриггер.PerformLayout()
        CType(Me.DataGridViewAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorКонфигурация, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorКонфигурация.ResumeLayout(False)
        Me.BindingNavigatorКонфигурация.PerformLayout()
        CType(Me.BindingSourceConfiguration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorФормула, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorФормула.ResumeLayout(False)
        Me.BindingNavigatorФормула.PerformLayout()
        CType(Me.BindingSourceFormula, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorАргумент, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorАргумент.ResumeLayout(False)
        Me.BindingNavigatorАргумент.PerformLayout()
        CType(Me.BindingNavigatorПорт, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorПорт.ResumeLayout(False)
        Me.BindingNavigatorПорт.PerformLayout()
        CType(Me.BindingSourcePort, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorБит, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorБит.ResumeLayout(False)
        Me.BindingNavigatorБит.PerformLayout()
        CType(Me.DataGridViewPort, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewConfiguration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewFormula, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorДействие, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorДействие.ResumeLayout(False)
        Me.BindingNavigatorДействие.PerformLayout()
        Me.TabPageDevice.ResumeLayout(False)
        Me.GroupBoxPort.ResumeLayout(False)
        Me.GroupBoxPort.PerformLayout()
        Me.automaticScaleModeGroupBox.ResumeLayout(False)
        Me.automaticScaleModeGroupBox.PerformLayout()
        CType(Me.ScalingSwitchArray.ItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.automaticScaleModePanel.ResumeLayout(False)
        Me.automaticScaleModePanel.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.NumericEditArray2.ItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.scalingLedArray.ItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericEditArray1.ItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsContainer.BottomToolStripPanel.ResumeLayout(False)
        Me.tsContainer.BottomToolStripPanel.PerformLayout()
        Me.tsContainer.ContentPanel.ResumeLayout(False)
        Me.tsContainer.TopToolStripPanel.ResumeLayout(False)
        Me.tsContainer.TopToolStripPanel.PerformLayout()
        Me.tsContainer.ResumeLayout(False)
        Me.tsContainer.PerformLayout()
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolBar1.ResumeLayout(False)
        Me.ToolBar1.PerformLayout()
        CType(Me.ErrorProviderБит, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderПорт, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderАргумент, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderФормула, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderТриггер, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderДействие, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProviderКонфигурация, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainerGlobal As System.Windows.Forms.SplitContainer
    Friend WithEvents tsContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents Status As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ToolStrip6 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButtonNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TSMenuItemStart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControlConfig As System.Windows.Forms.TabControl
    Friend WithEvents TabPageWatch As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainerОтборКонигураций As System.Windows.Forms.SplitContainer
    Friend WithEvents ListBoxGroupConfigurations As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxConfigurations As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Private WithEvents ToolBar1 As System.Windows.Forms.ToolStrip
    Friend WithEvents TSButtonStart As System.Windows.Forms.ToolStripButton
    Friend WithEvents TSButtonInterruptObservation As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowCascade As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowTileHorizontal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindowTileVertical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ПанельУстановка As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents GroupBoxPort As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBoxLineCount As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents physicalChannelComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxPhysicalPort As System.Windows.Forms.ComboBox
    Friend WithEvents deviceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents ScalingSwitchArray As NationalInstruments.UI.WindowsForms.SwitchArray
    Private WithEvents scalingLedArray As NationalInstruments.UI.WindowsForms.LedArray
    Friend WithEvents NumericEditArray1 As NationalInstruments.UI.WindowsForms.NumericEditArray
    Private WithEvents automaticScaleModeGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents automaticScaleModePanel As System.Windows.Forms.Panel
    Private WithEvents selectValueLabel As System.Windows.Forms.Label
    Private WithEvents valuesListBox As System.Windows.Forms.ListBox
    Private WithEvents ButtonRemove As System.Windows.Forms.Button
    Private WithEvents valuesLabel As System.Windows.Forms.Label
    Private WithEvents ButtonAdd As System.Windows.Forms.Button
    Private WithEvents booleanComboBox As System.Windows.Forms.ComboBox
    Private WithEvents scaleModeLabel As System.Windows.Forms.Label
    Private WithEvents ScaleModePropertyEditor As NationalInstruments.UI.WindowsForms.PropertyEditor
    Friend WithEvents NumericEditArray2 As NationalInstruments.UI.WindowsForms.NumericEditArray
    Friend WithEvents TabPageEdit As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainerExplorer As System.Windows.Forms.SplitContainer
    Public WithEvents propertyGrid1 As System.Windows.Forms.PropertyGrid
    Public WithEvents InstrumentControlStrip1 As NationalInstruments.UI.WindowsForms.InstrumentControlStrip
    Friend WithEvents TabPageBases As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanelDataSet As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DataGridViewБит As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSourceBit As System.Windows.Forms.BindingSource
    Friend WithEvents DataGridViewАргумент As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSourceАргумент As System.Windows.Forms.BindingSource
    Friend WithEvents DataGridViewТриггер As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSourceTrigger As System.Windows.Forms.BindingSource
    Friend WithEvents BindingNavigatorТриггер As System.Windows.Forms.BindingNavigator
    Friend WithEvents ТриггерBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonTriggerDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ТриггерBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ТриггерBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ТриггерBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ТриггерBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ТриггерBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonTriggerSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents DataGridViewAction As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSourceAction As System.Windows.Forms.BindingSource
    Friend WithEvents BindingNavigatorКонфигурация As System.Windows.Forms.BindingNavigator
    Friend WithEvents КонфигурацияBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingSourceConfiguration As System.Windows.Forms.BindingSource
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonConfigurationDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents КонфигурацияBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents КонфигурацияBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents КонфигурацияBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents КонфигурацияBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents КонфигурацияBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonConfigurationSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorФормула As System.Windows.Forms.BindingNavigator
    Friend WithEvents ФормулаBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingSourceFormula As System.Windows.Forms.BindingSource
    Friend WithEvents ToolStripLabel4 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonFormulaDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ФормулаBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ФормулаBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ФормулаBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ФормулаBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ФормулаBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonFormulaSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorАргумент As System.Windows.Forms.BindingNavigator
    Friend WithEvents АргументBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel5 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonArgumenDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents АргументBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents АргументBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents АргументBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents АргументBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents АргументBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonArgumentSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorПорт As System.Windows.Forms.BindingNavigator
    Friend WithEvents ПортBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingSourcePort As System.Windows.Forms.BindingSource
    Friend WithEvents ToolStripLabel6 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ПортBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ПортBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ПортBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ПортBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ПортBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorБит As System.Windows.Forms.BindingNavigator
    Friend WithEvents БитBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel7 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonBitDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents БитBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents БитBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents БитBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents БитBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents БитBNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonBitSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents DataGridViewPort As System.Windows.Forms.DataGridView
    Friend WithEvents GridViewConfiguration As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewFormula As System.Windows.Forms.DataGridView
    Friend WithEvents BindingNavigatorДействие As System.Windows.Forms.BindingNavigator
    Friend WithEvents ДействиеBNAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel8 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents TSButtonActionDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ДействиеBNMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ДействиеBNMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ДействиеBNPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator28 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ДействиеBNMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ДействиеNMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSButtonActionSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents TimerUpdate As System.Windows.Forms.Timer
    Friend WithEvents ErrorProviderБит As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderПорт As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderАргумент As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderФормула As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderТриггер As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderДействие As System.Windows.Forms.ErrorProvider
    Friend WithEvents ErrorProviderКонфигурация As System.Windows.Forms.ErrorProvider
    Friend WithEvents KeyДействиеDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyКонфигурацияДействияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяДействияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActionTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.ДействиеTableAdapter
    Friend WithEvents KeyКонфигурацияДействияDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяКонфигурацииDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ОписаниеDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConfigurationTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.КонфигурацияДействийTableAdapter
    Friend WithEvents KeyДествияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TriggerFireDigitalOutputTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.ТриггерСрабатыванияЦифровогоВыходаTableAdapter
    Friend WithEvents FormulaFireDigitalOutputTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.ФормулаСрабатыванияЦифровогоВыходаTableAdapter
    Friend WithEvents ArgumentsOfFormulaTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.АргументыДляФормулыTableAdapter
    Friend WithEvents PortsTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.ПортыTableAdapter
    Friend WithEvents BitsOfPortTableAdapter As ChannelsDigitalOutputDataSetTableAdapters.БитПортаTableAdapter
    Public WithEvents ChannelsDigitalOutputDataSet As ChannelsDigitalOutputDataSet
    Friend WithEvents KeyТриггерDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyДествиеDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяКаналаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ОперацияСравненияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ВеличинаУсловияDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ВеличинаУсловия2DataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИндексВМассивеПараметровDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyБитПортаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyПортаDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameDataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НомерБитаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents КеуArgumentDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyFormulaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяАргументаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяКаналаDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИндексВМассивеПараметровDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ПриведениеDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents KeyПортаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyДействиеDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НомерУстройстваDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НомерМодуляКорзиныDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НомерПортаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TSButtonSaveInDataSet As System.Windows.Forms.ToolStripButton
    Public WithEvents ImageListTree As System.Windows.Forms.ImageList
    Friend WithEvents TSButtonHandCheck As System.Windows.Forms.ToolStripButton
    Friend WithEvents TimerResize As System.Windows.Forms.Timer
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents KeyFormulaDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyДействиеDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ФормулаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ОперацияСравненияDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ВеличинаУсловияDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboDigitalWriteTask As System.Windows.Forms.ComboBox
    Friend WithEvents ListViewAlarms As System.Windows.Forms.ListBox
    Friend WithEvents TabPageDevice As System.Windows.Forms.TabPage
    Friend WithEvents Label8 As System.Windows.Forms.Label

End Class
