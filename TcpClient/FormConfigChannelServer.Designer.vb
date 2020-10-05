<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormConfigChannelServer
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

    Friend WithEvents ToolStripContainerForm As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ImageListTreeNode As System.Windows.Forms.ImageList
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents MenuStripForm As System.Windows.Forms.MenuStrip
    Friend WithEvents TSMenuItemFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSMenuItemSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSMenuItemPrint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemPrintPreview As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSMenuItemExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemUndo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemRedo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSMenuItemCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSMenuItemSelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemToolBar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemStatusBar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemFolders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemContents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolTipForm As System.Windows.Forms.ToolTip
    Friend WithEvents ImageListLarge As System.Windows.Forms.ImageList
    Friend WithEvents ImageListSmall As System.Windows.Forms.ImageList

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormConfigChannelServer))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ImageListTreeNode = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.TSButtonRemoveChannels = New System.Windows.Forms.ToolStripButton()
        Me.MenuStripForm = New System.Windows.Forms.MenuStrip()
        Me.TSMenuItemFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMenuItemSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMenuItemPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemPrintPreview = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMenuItemExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemUndo = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemRedo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMenuItemCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSMenuItemSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemView = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemToolBar = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemStatusBar = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemFolders = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemContents = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemIndex = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTipForm = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonRestoreConfiguration = New System.Windows.Forms.Button()
        Me.ButtonSaveConfiguration = New System.Windows.Forms.Button()
        Me.ButtonRemoveSelectedConfiguration = New System.Windows.Forms.Button()
        Me.ButtonAddChannel = New System.Windows.Forms.Button()
        Me.ButtonUp = New System.Windows.Forms.Button()
        Me.ButtonRmoveChannel = New System.Windows.Forms.Button()
        Me.ButtonDown = New System.Windows.Forms.Button()
        Me.ButtonClearChannels = New System.Windows.Forms.Button()
        Me.ComboBoxFilter = New System.Windows.Forms.ComboBox()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonСhoiceConfirmed = New System.Windows.Forms.Button()
        Me.TextBoxFilter = New System.Windows.Forms.TextBox()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ComboBoxPathServerConfigXML = New System.Windows.Forms.ComboBox()
        Me.ButtonExplorerServerConfigXML = New System.Windows.Forms.Button()
        Me.ButtonUdateTCP = New System.Windows.Forms.Button()
        Me.ToolStripContainerForm = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.PanelDrdFindChannel = New System.Windows.Forms.Panel()
        Me.DataGridViewFindChannel = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanelShadowButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridViewInputChannel = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumnNumberCh = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnInputName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnPin = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnScrName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnState = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnScrUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnChUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TextBoxColumnGroupChDataGridView = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceInputChannels = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.PanelFindChannel = New System.Windows.Forms.Panel()
        Me.LabelRowPosition = New System.Windows.Forms.Label()
        Me.LabelFindBy = New System.Windows.Forms.Label()
        Me.LabelCaptionGrid = New System.Windows.Forms.Label()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.DataGridViewOutputChannel = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumnOutputName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnGroupCh = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceoOutputChannels = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelConfigBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigator2CountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigator2MoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigator2MovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigator2PositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigator2MoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigator2MoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.LabelSelectedChannel = New System.Windows.Forms.Label()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.PanelConfigurations = New System.Windows.Forms.Panel()
        Me.ComboBoxConfigurations = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanelBottonCFG = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelConfigurations = New System.Windows.Forms.Label()
        Me.TableLayoutPanelBotton = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonSeparate1 = New System.Windows.Forms.Button()
        Me.ButtonSeparate2 = New System.Windows.Forms.Button()
        Me.PanelOptions = New System.Windows.Forms.Panel()
        Me.GroupBoxСтендКонфигСервер = New System.Windows.Forms.GroupBox()
        Me.TextBoxPathServerConfigXML = New System.Windows.Forms.TextBox()
        Me.LabelStendServer = New System.Windows.Forms.Label()
        Me.TimeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelsDataSet = New Registration.Channels_cfg_lmzDataSet()
        Me.ChannelBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelConfigBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ImageListLarge = New System.Windows.Forms.ImageList(Me.components)
        Me.ImageListSmall = New System.Windows.Forms.ImageList(Me.components)
        Me.OpenFileDialogPathDB = New System.Windows.Forms.OpenFileDialog()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ChannelTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.ChannelTableAdapter()
        Me.ChannelConfigTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter()
        Me.BindingSourceEdIzm = New System.Windows.Forms.BindingSource(Me.components)
        Me.UnitTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter()
        Me.TimeChannelTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.TimeChannelTableAdapter()
        Me.TimeTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.ChannelTableAdapter()
        Me.ChannelsNBaseDataSet = New Registration.ChannelsNDataSet()
        Me.BindingSourceChannelNBase = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelNTableAdapter = New Registration.ChannelsNDataSetTableAdapters.ChannelNTableAdapter()
        Me.StatusStrip.SuspendLayout()
        Me.ToolStrip.SuspendLayout()
        Me.MenuStripForm.SuspendLayout()
        Me.ToolStripContainerForm.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainerForm.ContentPanel.SuspendLayout()
        Me.ToolStripContainerForm.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainerForm.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.PanelDrdFindChannel.SuspendLayout()
        CType(Me.DataGridViewFindChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanelShadowButtons.SuspendLayout()
        CType(Me.DataGridViewInputChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceInputChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ChannelBindingNavigator.SuspendLayout()
        Me.PanelFindChannel.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.DataGridViewOutputChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceoOutputChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelConfigBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ChannelConfigBindingNavigator.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        Me.PanelConfigurations.SuspendLayout()
        Me.TableLayoutPanelBottonCFG.SuspendLayout()
        Me.TableLayoutPanelBotton.SuspendLayout()
        Me.PanelOptions.SuspendLayout()
        Me.GroupBoxСтендКонфигСервер.SuspendLayout()
        CType(Me.TimeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelConfigBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceEdIzm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelsNBaseDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceChannelNBase, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1008, 25)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel.Image = CType(resources.GetObject("ToolStripStatusLabel.Image"), System.Drawing.Image)
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(58, 20)
        Me.ToolStripStatusLabel.Text = "Готов"
        Me.ToolStripStatusLabel.ToolTipText = "Количество подключённых шасси"
        '
        'ImageListTreeNode
        '
        Me.ImageListTreeNode.ImageStream = CType(resources.GetObject("ImageListTreeNode.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTreeNode.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTreeNode.Images.SetKeyName(0, "ClosedFolder")
        Me.ImageListTreeNode.Images.SetKeyName(1, "OpenFolder")
        '
        'ToolStrip
        '
        Me.ToolStrip.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSButtonRemoveChannels})
        Me.ToolStrip.Location = New System.Drawing.Point(3, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(171, 25)
        Me.ToolStrip.TabIndex = 0
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'TSButtonRemoveChannels
        '
        Me.TSButtonRemoveChannels.Checked = True
        Me.TSButtonRemoveChannels.CheckOnClick = True
        Me.TSButtonRemoveChannels.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TSButtonRemoveChannels.Image = CType(resources.GetObject("TSButtonRemoveChannels.Image"), System.Drawing.Image)
        Me.TSButtonRemoveChannels.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSButtonRemoveChannels.Name = "TSButtonRemoveChannels"
        Me.TSButtonRemoveChannels.Size = New System.Drawing.Size(159, 22)
        Me.TSButtonRemoveChannels.Text = "Отсев каналов включен"
        Me.TSButtonRemoveChannels.ToolTipText = "Вкл/Выкл отсев каналов (выключенных или начинающихся с '_')"
        '
        'MenuStripForm
        '
        Me.MenuStripForm.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStripForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemFile, Me.TSMenuItemEdit, Me.TSMenuItemView, Me.ToolsToolStripMenuItem, Me.TSMenuItemHelp})
        Me.MenuStripForm.Location = New System.Drawing.Point(0, 25)
        Me.MenuStripForm.Name = "MenuStripForm"
        Me.MenuStripForm.Size = New System.Drawing.Size(278, 24)
        Me.MenuStripForm.TabIndex = 0
        Me.MenuStripForm.Text = "MenuStripForm"
        Me.MenuStripForm.Visible = False
        '
        'TSMenuItemFile
        '
        Me.TSMenuItemFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemNew, Me.TSMenuItemOpen, Me.ToolStripSeparator1, Me.TSMenuItemSave, Me.TSMenuItemSaveAs, Me.ToolStripSeparator2, Me.TSMenuItemPrint, Me.TSMenuItemPrintPreview, Me.ToolStripSeparator3, Me.TSMenuItemExit})
        Me.TSMenuItemFile.Name = "TSMenuItemFile"
        Me.TSMenuItemFile.Size = New System.Drawing.Size(48, 20)
        Me.TSMenuItemFile.Text = "&Файл"
        '
        'TSMenuItemNew
        '
        Me.TSMenuItemNew.Image = CType(resources.GetObject("TSMenuItemNew.Image"), System.Drawing.Image)
        Me.TSMenuItemNew.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemNew.Name = "TSMenuItemNew"
        Me.TSMenuItemNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.TSMenuItemNew.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemNew.Text = "&Создать"
        '
        'TSMenuItemOpen
        '
        Me.TSMenuItemOpen.Image = CType(resources.GetObject("TSMenuItemOpen.Image"), System.Drawing.Image)
        Me.TSMenuItemOpen.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemOpen.Name = "TSMenuItemOpen"
        Me.TSMenuItemOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.TSMenuItemOpen.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemOpen.Text = "&Открыть"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(230, 6)
        '
        'TSMenuItemSave
        '
        Me.TSMenuItemSave.Image = CType(resources.GetObject("TSMenuItemSave.Image"), System.Drawing.Image)
        Me.TSMenuItemSave.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemSave.Name = "TSMenuItemSave"
        Me.TSMenuItemSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.TSMenuItemSave.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemSave.Text = "&Сохранить"
        '
        'TSMenuItemSaveAs
        '
        Me.TSMenuItemSaveAs.Name = "TSMenuItemSaveAs"
        Me.TSMenuItemSaveAs.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemSaveAs.Text = "Сохранить &как"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(230, 6)
        '
        'TSMenuItemPrint
        '
        Me.TSMenuItemPrint.Image = CType(resources.GetObject("TSMenuItemPrint.Image"), System.Drawing.Image)
        Me.TSMenuItemPrint.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemPrint.Name = "TSMenuItemPrint"
        Me.TSMenuItemPrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.TSMenuItemPrint.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemPrint.Text = "&Печать"
        '
        'TSMenuItemPrintPreview
        '
        Me.TSMenuItemPrintPreview.Image = CType(resources.GetObject("TSMenuItemPrintPreview.Image"), System.Drawing.Image)
        Me.TSMenuItemPrintPreview.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemPrintPreview.Name = "TSMenuItemPrintPreview"
        Me.TSMenuItemPrintPreview.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemPrintPreview.Text = "&Предварительный просмотр"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(230, 6)
        '
        'TSMenuItemExit
        '
        Me.TSMenuItemExit.Name = "TSMenuItemExit"
        Me.TSMenuItemExit.Size = New System.Drawing.Size(233, 22)
        Me.TSMenuItemExit.Text = "В&ыход"
        '
        'TSMenuItemEdit
        '
        Me.TSMenuItemEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemUndo, Me.TSMenuItemRedo, Me.ToolStripSeparator4, Me.TSMenuItemCut, Me.TSMenuItemCopy, Me.TSMenuItemPaste, Me.ToolStripSeparator5, Me.TSMenuItemSelectAll})
        Me.TSMenuItemEdit.Name = "TSMenuItemEdit"
        Me.TSMenuItemEdit.Size = New System.Drawing.Size(59, 20)
        Me.TSMenuItemEdit.Text = "&Правка"
        '
        'TSMenuItemUndo
        '
        Me.TSMenuItemUndo.Image = CType(resources.GetObject("TSMenuItemUndo.Image"), System.Drawing.Image)
        Me.TSMenuItemUndo.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemUndo.Name = "TSMenuItemUndo"
        Me.TSMenuItemUndo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.TSMenuItemUndo.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemUndo.Text = "&Отменить"
        '
        'TSMenuItemRedo
        '
        Me.TSMenuItemRedo.Image = CType(resources.GetObject("TSMenuItemRedo.Image"), System.Drawing.Image)
        Me.TSMenuItemRedo.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemRedo.Name = "TSMenuItemRedo"
        Me.TSMenuItemRedo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.TSMenuItemRedo.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemRedo.Text = "&Вернуть"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(187, 6)
        '
        'TSMenuItemCut
        '
        Me.TSMenuItemCut.Image = CType(resources.GetObject("TSMenuItemCut.Image"), System.Drawing.Image)
        Me.TSMenuItemCut.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemCut.Name = "TSMenuItemCut"
        Me.TSMenuItemCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.TSMenuItemCut.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemCut.Text = "&Вырезать"
        '
        'TSMenuItemCopy
        '
        Me.TSMenuItemCopy.Image = CType(resources.GetObject("TSMenuItemCopy.Image"), System.Drawing.Image)
        Me.TSMenuItemCopy.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemCopy.Name = "TSMenuItemCopy"
        Me.TSMenuItemCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.TSMenuItemCopy.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemCopy.Text = "&Копировать"
        '
        'TSMenuItemPaste
        '
        Me.TSMenuItemPaste.Image = CType(resources.GetObject("TSMenuItemPaste.Image"), System.Drawing.Image)
        Me.TSMenuItemPaste.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemPaste.Name = "TSMenuItemPaste"
        Me.TSMenuItemPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.TSMenuItemPaste.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemPaste.Text = "&Вставить"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(187, 6)
        '
        'TSMenuItemSelectAll
        '
        Me.TSMenuItemSelectAll.Name = "TSMenuItemSelectAll"
        Me.TSMenuItemSelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.TSMenuItemSelectAll.Size = New System.Drawing.Size(190, 22)
        Me.TSMenuItemSelectAll.Text = "&Выделить все"
        '
        'TSMenuItemView
        '
        Me.TSMenuItemView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemToolBar, Me.TSMenuItemStatusBar, Me.TSMenuItemFolders})
        Me.TSMenuItemView.Name = "TSMenuItemView"
        Me.TSMenuItemView.Size = New System.Drawing.Size(39, 20)
        Me.TSMenuItemView.Text = "&Вид"
        '
        'TSMenuItemToolBar
        '
        Me.TSMenuItemToolBar.Checked = True
        Me.TSMenuItemToolBar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TSMenuItemToolBar.Name = "TSMenuItemToolBar"
        Me.TSMenuItemToolBar.Size = New System.Drawing.Size(196, 22)
        Me.TSMenuItemToolBar.Text = "&Панель инструментов"
        '
        'TSMenuItemStatusBar
        '
        Me.TSMenuItemStatusBar.Checked = True
        Me.TSMenuItemStatusBar.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TSMenuItemStatusBar.Name = "TSMenuItemStatusBar"
        Me.TSMenuItemStatusBar.Size = New System.Drawing.Size(196, 22)
        Me.TSMenuItemStatusBar.Text = "&Строка состояния"
        '
        'TSMenuItemFolders
        '
        Me.TSMenuItemFolders.Checked = True
        Me.TSMenuItemFolders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TSMenuItemFolders.Name = "TSMenuItemFolders"
        Me.TSMenuItemFolders.Size = New System.Drawing.Size(196, 22)
        Me.TSMenuItemFolders.Text = "&Папки"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemOptions})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ToolsToolStripMenuItem.Text = "&Сервис"
        '
        'TSMenuItemOptions
        '
        Me.TSMenuItemOptions.Name = "TSMenuItemOptions"
        Me.TSMenuItemOptions.Size = New System.Drawing.Size(138, 22)
        Me.TSMenuItemOptions.Text = "&Параметры"
        '
        'TSMenuItemHelp
        '
        Me.TSMenuItemHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemContents, Me.TSMenuItemIndex, Me.TSMenuItemSearch, Me.ToolStripSeparator6, Me.AboutToolStripMenuItem})
        Me.TSMenuItemHelp.Name = "TSMenuItemHelp"
        Me.TSMenuItemHelp.Size = New System.Drawing.Size(65, 20)
        Me.TSMenuItemHelp.Text = "&Справка"
        '
        'TSMenuItemContents
        '
        Me.TSMenuItemContents.Name = "TSMenuItemContents"
        Me.TSMenuItemContents.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.TSMenuItemContents.Size = New System.Drawing.Size(189, 22)
        Me.TSMenuItemContents.Text = "Содер&жание"
        '
        'TSMenuItemIndex
        '
        Me.TSMenuItemIndex.Image = CType(resources.GetObject("TSMenuItemIndex.Image"), System.Drawing.Image)
        Me.TSMenuItemIndex.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemIndex.Name = "TSMenuItemIndex"
        Me.TSMenuItemIndex.Size = New System.Drawing.Size(189, 22)
        Me.TSMenuItemIndex.Text = "&Указатель"
        '
        'TSMenuItemSearch
        '
        Me.TSMenuItemSearch.Image = CType(resources.GetObject("TSMenuItemSearch.Image"), System.Drawing.Image)
        Me.TSMenuItemSearch.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemSearch.Name = "TSMenuItemSearch"
        Me.TSMenuItemSearch.Size = New System.Drawing.Size(189, 22)
        Me.TSMenuItemSearch.Text = "&Поиск"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(186, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.AboutToolStripMenuItem.Text = "&О программе ..."
        '
        'ButtonRestoreConfiguration
        '
        Me.ButtonRestoreConfiguration.BackgroundImage = CType(resources.GetObject("ButtonRestoreConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonRestoreConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonRestoreConfiguration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonRestoreConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRestoreConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonRestoreConfiguration.Location = New System.Drawing.Point(3, 3)
        Me.ButtonRestoreConfiguration.Name = "ButtonRestoreConfiguration"
        Me.ButtonRestoreConfiguration.Size = New System.Drawing.Size(73, 40)
        Me.ButtonRestoreConfiguration.TabIndex = 4
        Me.ButtonRestoreConfiguration.Text = "Считать"
        Me.ButtonRestoreConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonRestoreConfiguration, "Считать выбранный список каналов")
        Me.ButtonRestoreConfiguration.UseVisualStyleBackColor = True
        '
        'ButtonSaveConfiguration
        '
        Me.ButtonSaveConfiguration.BackgroundImage = CType(resources.GetObject("ButtonSaveConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonSaveConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonSaveConfiguration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSaveConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSaveConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonSaveConfiguration.Location = New System.Drawing.Point(82, 3)
        Me.ButtonSaveConfiguration.Name = "ButtonSaveConfiguration"
        Me.ButtonSaveConfiguration.Size = New System.Drawing.Size(73, 40)
        Me.ButtonSaveConfiguration.TabIndex = 3
        Me.ButtonSaveConfiguration.Text = "Записать"
        Me.ButtonSaveConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonSaveConfiguration, "Записать список каналов под именем")
        Me.ButtonSaveConfiguration.UseVisualStyleBackColor = True
        '
        'ButtonRemoveSelectedConfiguration
        '
        Me.ButtonRemoveSelectedConfiguration.BackgroundImage = CType(resources.GetObject("ButtonRemoveSelectedConfiguration.BackgroundImage"), System.Drawing.Image)
        Me.ButtonRemoveSelectedConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonRemoveSelectedConfiguration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonRemoveSelectedConfiguration.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRemoveSelectedConfiguration.ForeColor = System.Drawing.Color.Black
        Me.ButtonRemoveSelectedConfiguration.Location = New System.Drawing.Point(161, 3)
        Me.ButtonRemoveSelectedConfiguration.Name = "ButtonRemoveSelectedConfiguration"
        Me.ButtonRemoveSelectedConfiguration.Size = New System.Drawing.Size(75, 40)
        Me.ButtonRemoveSelectedConfiguration.TabIndex = 5
        Me.ButtonRemoveSelectedConfiguration.Text = "Удалить"
        Me.ButtonRemoveSelectedConfiguration.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonRemoveSelectedConfiguration, "Удалить имя списка из базы")
        Me.ButtonRemoveSelectedConfiguration.UseVisualStyleBackColor = True
        '
        'ButtonAddChannel
        '
        Me.ButtonAddChannel.BackgroundImage = CType(resources.GetObject("ButtonAddChannel.BackgroundImage"), System.Drawing.Image)
        Me.ButtonAddChannel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonAddChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonAddChannel.ForeColor = System.Drawing.Color.Black
        Me.ButtonAddChannel.Location = New System.Drawing.Point(3, 3)
        Me.ButtonAddChannel.Name = "ButtonAddChannel"
        Me.ButtonAddChannel.Size = New System.Drawing.Size(73, 40)
        Me.ButtonAddChannel.TabIndex = 2
        Me.ButtonAddChannel.Text = "Добавить"
        Me.ButtonAddChannel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonAddChannel, "Добавить канал в список")
        Me.ButtonAddChannel.UseVisualStyleBackColor = True
        '
        'ButtonUp
        '
        Me.ButtonUp.BackgroundImage = CType(resources.GetObject("ButtonUp.BackgroundImage"), System.Drawing.Image)
        Me.ButtonUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonUp.ForeColor = System.Drawing.Color.Black
        Me.ButtonUp.Location = New System.Drawing.Point(3, 168)
        Me.ButtonUp.Name = "ButtonUp"
        Me.ButtonUp.Size = New System.Drawing.Size(73, 40)
        Me.ButtonUp.TabIndex = 25
        Me.ButtonUp.Text = "Вверх"
        Me.ButtonUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonUp, "Переместить строку вверх")
        Me.ButtonUp.UseVisualStyleBackColor = True
        '
        'ButtonRmoveChannel
        '
        Me.ButtonRmoveChannel.BackgroundImage = CType(resources.GetObject("ButtonRmoveChannel.BackgroundImage"), System.Drawing.Image)
        Me.ButtonRmoveChannel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonRmoveChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRmoveChannel.ForeColor = System.Drawing.Color.Black
        Me.ButtonRmoveChannel.Location = New System.Drawing.Point(3, 53)
        Me.ButtonRmoveChannel.Name = "ButtonRmoveChannel"
        Me.ButtonRmoveChannel.Size = New System.Drawing.Size(73, 40)
        Me.ButtonRmoveChannel.TabIndex = 1
        Me.ButtonRmoveChannel.Text = "Удалить"
        Me.ButtonRmoveChannel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonRmoveChannel, "Удалить канал из списка")
        Me.ButtonRmoveChannel.UseVisualStyleBackColor = True
        '
        'ButtonDown
        '
        Me.ButtonDown.BackgroundImage = CType(resources.GetObject("ButtonDown.BackgroundImage"), System.Drawing.Image)
        Me.ButtonDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDown.ForeColor = System.Drawing.Color.Black
        Me.ButtonDown.Location = New System.Drawing.Point(3, 218)
        Me.ButtonDown.Name = "ButtonDown"
        Me.ButtonDown.Size = New System.Drawing.Size(73, 40)
        Me.ButtonDown.TabIndex = 24
        Me.ButtonDown.Text = "Вниз"
        Me.ButtonDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonDown, "Переместить строку вниз")
        Me.ButtonDown.UseVisualStyleBackColor = True
        '
        'ButtonClearChannels
        '
        Me.ButtonClearChannels.BackgroundImage = CType(resources.GetObject("ButtonClearChannels.BackgroundImage"), System.Drawing.Image)
        Me.ButtonClearChannels.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonClearChannels.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonClearChannels.ForeColor = System.Drawing.Color.Black
        Me.ButtonClearChannels.Location = New System.Drawing.Point(3, 103)
        Me.ButtonClearChannels.Name = "ButtonClearChannels"
        Me.ButtonClearChannels.Size = New System.Drawing.Size(73, 40)
        Me.ButtonClearChannels.TabIndex = 0
        Me.ButtonClearChannels.Text = "Очистить"
        Me.ButtonClearChannels.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonClearChannels, "Очистить весь список")
        Me.ButtonClearChannels.UseVisualStyleBackColor = True
        '
        'ComboBoxFilter
        '
        Me.ComboBoxFilter.FormattingEnabled = True
        Me.ComboBoxFilter.Items.AddRange(New Object() {"Имя", "Экр_Имя", "Pin", "Группа"})
        Me.ComboBoxFilter.Location = New System.Drawing.Point(67, 5)
        Me.ComboBoxFilter.Name = "ComboBoxFilter"
        Me.ComboBoxFilter.Size = New System.Drawing.Size(79, 21)
        Me.ComboBoxFilter.TabIndex = 21
        Me.ComboBoxFilter.Text = "Имя"
        Me.ToolTipForm.SetToolTip(Me.ComboBoxFilter, "Выбор столбца для фильтра")
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonCancel.Location = New System.Drawing.Point(118, 3)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(67, 23)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Отмена"
        Me.ToolTipForm.SetToolTip(Me.ButtonCancel, "Канал отсутствует")
        '
        'ButtonСhoiceConfirmed
        '
        Me.ButtonСhoiceConfirmed.BackColor = System.Drawing.Color.Orange
        Me.ButtonСhoiceConfirmed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonСhoiceConfirmed.Location = New System.Drawing.Point(3, 3)
        Me.ButtonСhoiceConfirmed.Name = "ButtonСhoiceConfirmed"
        Me.ButtonСhoiceConfirmed.Size = New System.Drawing.Size(106, 23)
        Me.ButtonСhoiceConfirmed.TabIndex = 30
        Me.ButtonСhoiceConfirmed.Text = "Подтвердите выбор"
        Me.ToolTipForm.SetToolTip(Me.ButtonСhoiceConfirmed, "Выделенная строка соответствует поиску")
        Me.ButtonСhoiceConfirmed.UseVisualStyleBackColor = False
        '
        'TextBoxFilter
        '
        Me.TextBoxFilter.Location = New System.Drawing.Point(148, 6)
        Me.TextBoxFilter.Name = "TextBoxFilter"
        Me.TextBoxFilter.Size = New System.Drawing.Size(90, 20)
        Me.TextBoxFilter.TabIndex = 30
        Me.ToolTipForm.SetToolTip(Me.TextBoxFilter, "Ввод предполагаемого имени канала")
        '
        'ButtonSave
        '
        Me.ButtonSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSave.BackColor = System.Drawing.Color.DarkGoldenrod
        Me.ButtonSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSave.Image = CType(resources.GetObject("ButtonSave.Image"), System.Drawing.Image)
        Me.ButtonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonSave.Location = New System.Drawing.Point(833, 14)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonSave.Size = New System.Drawing.Size(145, 23)
        Me.ButtonSave.TabIndex = 76
        Me.ButtonSave.Text = "Сохранить настройки"
        Me.ButtonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipForm.SetToolTip(Me.ButtonSave, "Сохранить настройки назначенных путей к файлам конфигураций")
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ComboBoxPathServerConfigXML
        '
        Me.ComboBoxPathServerConfigXML.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxPathServerConfigXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxPathServerConfigXML.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPathServerConfigXML.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxPathServerConfigXML.Location = New System.Drawing.Point(102, 16)
        Me.ComboBoxPathServerConfigXML.Name = "ComboBoxPathServerConfigXML"
        Me.ComboBoxPathServerConfigXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxPathServerConfigXML.Size = New System.Drawing.Size(51, 21)
        Me.ComboBoxPathServerConfigXML.TabIndex = 58
        Me.ToolTipForm.SetToolTip(Me.ComboBoxPathServerConfigXML, "Выбор стенда для назначения пути к файлу конфигурации")
        '
        'ButtonExplorerServerConfigXML
        '
        Me.ButtonExplorerServerConfigXML.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExplorerServerConfigXML.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonExplorerServerConfigXML.BackgroundImage = CType(resources.GetObject("ButtonExplorerServerConfigXML.BackgroundImage"), System.Drawing.Image)
        Me.ButtonExplorerServerConfigXML.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonExplorerServerConfigXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonExplorerServerConfigXML.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonExplorerServerConfigXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonExplorerServerConfigXML.Location = New System.Drawing.Point(802, 15)
        Me.ButtonExplorerServerConfigXML.Name = "ButtonExplorerServerConfigXML"
        Me.ButtonExplorerServerConfigXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonExplorerServerConfigXML.Size = New System.Drawing.Size(25, 21)
        Me.ButtonExplorerServerConfigXML.TabIndex = 56
        Me.ButtonExplorerServerConfigXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTipForm.SetToolTip(Me.ButtonExplorerServerConfigXML, "Задать расположение")
        Me.ButtonExplorerServerConfigXML.UseVisualStyleBackColor = False
        '
        'ButtonUdateTCP
        '
        Me.ButtonUdateTCP.BackgroundImage = CType(resources.GetObject("ButtonUdateTCP.BackgroundImage"), System.Drawing.Image)
        Me.ButtonUdateTCP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonUdateTCP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonUdateTCP.ForeColor = System.Drawing.Color.Black
        Me.ButtonUdateTCP.Location = New System.Drawing.Point(3, 283)
        Me.ButtonUdateTCP.Name = "ButtonUdateTCP"
        Me.ButtonUdateTCP.Size = New System.Drawing.Size(73, 40)
        Me.ButtonUdateTCP.TabIndex = 28
        Me.ButtonUdateTCP.Text = "Обновить"
        Me.ButtonUdateTCP.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonUdateTCP, "Обновить конфигурацию")
        Me.ButtonUdateTCP.UseVisualStyleBackColor = True
        '
        'ToolStripContainerForm
        '
        '
        'ToolStripContainerForm.BottomToolStripPanel
        '
        Me.ToolStripContainerForm.BottomToolStripPanel.Controls.Add(Me.StatusStrip)
        '
        'ToolStripContainerForm.ContentPanel
        '
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainerForm.ContentPanel.Controls.Add(Me.PanelOptions)
        Me.ToolStripContainerForm.ContentPanel.Size = New System.Drawing.Size(1008, 682)
        Me.ToolStripContainerForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainerForm.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainerForm.Name = "ToolStripContainerForm"
        Me.ToolStripContainerForm.Size = New System.Drawing.Size(1008, 732)
        Me.ToolStripContainerForm.TabIndex = 7
        Me.ToolStripContainerForm.Text = "ToolStripContainer1"
        '
        'ToolStripContainerForm.TopToolStripPanel
        '
        Me.ToolStripContainerForm.TopToolStripPanel.Controls.Add(Me.ToolStrip)
        Me.ToolStripContainerForm.TopToolStripPanel.Controls.Add(Me.MenuStripForm)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 60)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.PanelDrdFindChannel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DataGridViewInputChannel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ChannelBindingNavigator)
        Me.SplitContainer1.Panel1.Controls.Add(Me.PanelFindChannel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelCaptionGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.PanelButtons)
        Me.SplitContainer1.Size = New System.Drawing.Size(1008, 622)
        Me.SplitContainer1.SplitterDistance = 299
        Me.SplitContainer1.TabIndex = 6
        '
        'PanelDrdFindChannel
        '
        Me.PanelDrdFindChannel.Controls.Add(Me.DataGridViewFindChannel)
        Me.PanelDrdFindChannel.Controls.Add(Me.TableLayoutPanelShadowButtons)
        Me.PanelDrdFindChannel.Location = New System.Drawing.Point(46, 139)
        Me.PanelDrdFindChannel.Name = "PanelDrdFindChannel"
        Me.PanelDrdFindChannel.Size = New System.Drawing.Size(192, 230)
        Me.PanelDrdFindChannel.TabIndex = 31
        Me.PanelDrdFindChannel.Visible = False
        '
        'DataGridViewFindChannel
        '
        Me.DataGridViewFindChannel.AllowUserToAddRows = False
        Me.DataGridViewFindChannel.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGridViewFindChannel.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewFindChannel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridViewFindChannel.BackgroundColor = System.Drawing.Color.Lavender
        Me.DataGridViewFindChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewFindChannel.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.DarkBlue
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Teal
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewFindChannel.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewFindChannel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewFindChannel.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewFindChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewFindChannel.GridColor = System.Drawing.Color.RoyalBlue
        Me.DataGridViewFindChannel.Location = New System.Drawing.Point(0, 0)
        Me.DataGridViewFindChannel.MultiSelect = False
        Me.DataGridViewFindChannel.Name = "DataGridViewFindChannel"
        Me.DataGridViewFindChannel.ReadOnly = True
        Me.DataGridViewFindChannel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewFindChannel.Size = New System.Drawing.Size(192, 201)
        Me.DataGridViewFindChannel.TabIndex = 29
        '
        'TableLayoutPanelShadowButtons
        '
        Me.TableLayoutPanelShadowButtons.ColumnCount = 2
        Me.TableLayoutPanelShadowButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelShadowButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelShadowButtons.Controls.Add(Me.ButtonCancel, 1, 0)
        Me.TableLayoutPanelShadowButtons.Controls.Add(Me.ButtonСhoiceConfirmed, 0, 0)
        Me.TableLayoutPanelShadowButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelShadowButtons.Location = New System.Drawing.Point(0, 201)
        Me.TableLayoutPanelShadowButtons.Name = "TableLayoutPanelShadowButtons"
        Me.TableLayoutPanelShadowButtons.RowCount = 1
        Me.TableLayoutPanelShadowButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelShadowButtons.Size = New System.Drawing.Size(192, 29)
        Me.TableLayoutPanelShadowButtons.TabIndex = 31
        '
        'DataGridViewInputChannel
        '
        Me.DataGridViewInputChannel.AllowUserToAddRows = False
        Me.DataGridViewInputChannel.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender
        Me.DataGridViewInputChannel.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewInputChannel.AutoGenerateColumns = False
        Me.DataGridViewInputChannel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridViewInputChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewInputChannel.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewInputChannel.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewInputChannel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewInputChannel.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnNumberCh, Me.DataGridViewTextBoxColumnInputName, Me.DataGridViewTextBoxColumnPin, Me.DataGridViewTextBoxColumnScrName, Me.DataGridViewTextBoxColumnState, Me.DataGridViewTextBoxColumnScrUnit, Me.DataGridViewTextBoxColumnChUnit, Me.TextBoxColumnGroupChDataGridView})
        Me.DataGridViewInputChannel.DataSource = Me.BindingSourceInputChannels
        Me.DataGridViewInputChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewInputChannel.Location = New System.Drawing.Point(0, 77)
        Me.DataGridViewInputChannel.Name = "DataGridViewInputChannel"
        Me.DataGridViewInputChannel.ReadOnly = True
        Me.DataGridViewInputChannel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewInputChannel.Size = New System.Drawing.Size(295, 541)
        Me.DataGridViewInputChannel.TabIndex = 9
        '
        'DataGridViewTextBoxColumnNumberCh
        '
        Me.DataGridViewTextBoxColumnNumberCh.DataPropertyName = "NumberCh"
        Me.DataGridViewTextBoxColumnNumberCh.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnNumberCh.HeaderText = "Номер"
        Me.DataGridViewTextBoxColumnNumberCh.Name = "DataGridViewTextBoxColumnNumberCh"
        Me.DataGridViewTextBoxColumnNumberCh.ReadOnly = True
        Me.DataGridViewTextBoxColumnNumberCh.Width = 50
        '
        'DataGridViewTextBoxColumnInputName
        '
        Me.DataGridViewTextBoxColumnInputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnInputName.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumnInputName.HeaderText = "Имя"
        Me.DataGridViewTextBoxColumnInputName.Name = "DataGridViewTextBoxColumnInputName"
        Me.DataGridViewTextBoxColumnInputName.ReadOnly = True
        Me.DataGridViewTextBoxColumnInputName.Width = 57
        '
        'DataGridViewTextBoxColumnPin
        '
        Me.DataGridViewTextBoxColumnPin.DataPropertyName = "Pin"
        Me.DataGridViewTextBoxColumnPin.FillWeight = 80.0!
        Me.DataGridViewTextBoxColumnPin.HeaderText = "Pin"
        Me.DataGridViewTextBoxColumnPin.Name = "DataGridViewTextBoxColumnPin"
        Me.DataGridViewTextBoxColumnPin.ReadOnly = True
        Me.DataGridViewTextBoxColumnPin.Width = 80
        '
        'DataGridViewTextBoxColumnScrName
        '
        Me.DataGridViewTextBoxColumnScrName.DataPropertyName = "Scr_Name"
        Me.DataGridViewTextBoxColumnScrName.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumnScrName.HeaderText = "Экр_Имя"
        Me.DataGridViewTextBoxColumnScrName.Name = "DataGridViewTextBoxColumnScrName"
        Me.DataGridViewTextBoxColumnScrName.ReadOnly = True
        Me.DataGridViewTextBoxColumnScrName.Width = 150
        '
        'DataGridViewTextBoxColumnState
        '
        Me.DataGridViewTextBoxColumnState.DataPropertyName = "State"
        Me.DataGridViewTextBoxColumnState.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnState.HeaderText = "Вкл/Выкл"
        Me.DataGridViewTextBoxColumnState.Name = "DataGridViewTextBoxColumnState"
        Me.DataGridViewTextBoxColumnState.ReadOnly = True
        Me.DataGridViewTextBoxColumnState.Width = 50
        '
        'DataGridViewTextBoxColumnScrUnit
        '
        Me.DataGridViewTextBoxColumnScrUnit.DataPropertyName = "Scr_EdIzm"
        Me.DataGridViewTextBoxColumnScrUnit.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnScrUnit.HeaderText = "Экр. Ед.Изм."
        Me.DataGridViewTextBoxColumnScrUnit.Name = "DataGridViewTextBoxColumnScrUnit"
        Me.DataGridViewTextBoxColumnScrUnit.ReadOnly = True
        Me.DataGridViewTextBoxColumnScrUnit.Width = 50
        '
        'DataGridViewTextBoxColumnChUnit
        '
        Me.DataGridViewTextBoxColumnChUnit.DataPropertyName = "Ch_Unit"
        Me.DataGridViewTextBoxColumnChUnit.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnChUnit.HeaderText = "Физ. Ед.Изм."
        Me.DataGridViewTextBoxColumnChUnit.Name = "DataGridViewTextBoxColumnChUnit"
        Me.DataGridViewTextBoxColumnChUnit.ReadOnly = True
        Me.DataGridViewTextBoxColumnChUnit.Width = 50
        '
        'TextBoxColumnGroupChDataGridView
        '
        Me.TextBoxColumnGroupChDataGridView.DataPropertyName = "GroupCh"
        Me.TextBoxColumnGroupChDataGridView.HeaderText = "Группа"
        Me.TextBoxColumnGroupChDataGridView.Name = "TextBoxColumnGroupChDataGridView"
        Me.TextBoxColumnGroupChDataGridView.ReadOnly = True
        Me.TextBoxColumnGroupChDataGridView.Width = 50
        '
        'BindingSourceInputChannels
        '
        Me.BindingSourceInputChannels.DataSource = GetType(Registration.ChannelsInputBindingList)
        '
        'ChannelBindingNavigator
        '
        Me.ChannelBindingNavigator.AddNewItem = Nothing
        Me.ChannelBindingNavigator.BindingSource = Me.BindingSourceInputChannels
        Me.ChannelBindingNavigator.CountItem = Me.BindingNavigatorCountItem
        Me.ChannelBindingNavigator.DeleteItem = Nothing
        Me.ChannelBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem})
        Me.ChannelBindingNavigator.Location = New System.Drawing.Point(0, 52)
        Me.ChannelBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.ChannelBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.ChannelBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.ChannelBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.ChannelBindingNavigator.Name = "ChannelBindingNavigator"
        Me.ChannelBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me.ChannelBindingNavigator.Size = New System.Drawing.Size(295, 25)
        Me.ChannelBindingNavigator.TabIndex = 3
        Me.ChannelBindingNavigator.Text = "ChannelBindingNavigator"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(43, 22)
        Me.BindingNavigatorCountItem.Text = "для {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Общее число элементов"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Переместить в начало"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Переместить назад"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Положение"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 23)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Текущее положение"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Переместить вперед"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Переместить в конец"
        '
        'PanelFindChannel
        '
        Me.PanelFindChannel.Controls.Add(Me.ComboBoxFilter)
        Me.PanelFindChannel.Controls.Add(Me.LabelRowPosition)
        Me.PanelFindChannel.Controls.Add(Me.TextBoxFilter)
        Me.PanelFindChannel.Controls.Add(Me.LabelFindBy)
        Me.PanelFindChannel.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFindChannel.Location = New System.Drawing.Point(0, 22)
        Me.PanelFindChannel.Name = "PanelFindChannel"
        Me.PanelFindChannel.Size = New System.Drawing.Size(295, 30)
        Me.PanelFindChannel.TabIndex = 31
        '
        'LabelRowPosition
        '
        Me.LabelRowPosition.AutoSize = True
        Me.LabelRowPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelRowPosition.Location = New System.Drawing.Point(244, 9)
        Me.LabelRowPosition.Name = "LabelRowPosition"
        Me.LabelRowPosition.Size = New System.Drawing.Size(2, 15)
        Me.LabelRowPosition.TabIndex = 31
        Me.LabelRowPosition.Visible = False
        '
        'LabelFindBy
        '
        Me.LabelFindBy.AutoSize = True
        Me.LabelFindBy.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LabelFindBy.Location = New System.Drawing.Point(3, 9)
        Me.LabelFindBy.Name = "LabelFindBy"
        Me.LabelFindBy.Size = New System.Drawing.Size(57, 13)
        Me.LabelFindBy.TabIndex = 29
        Me.LabelFindBy.Text = "Поиск по:"
        Me.LabelFindBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCaptionGrid
        '
        Me.LabelCaptionGrid.BackColor = System.Drawing.Color.RoyalBlue
        Me.LabelCaptionGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCaptionGrid.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCaptionGrid.ForeColor = System.Drawing.Color.White
        Me.LabelCaptionGrid.Location = New System.Drawing.Point(0, 0)
        Me.LabelCaptionGrid.Name = "LabelCaptionGrid"
        Me.LabelCaptionGrid.Size = New System.Drawing.Size(295, 22)
        Me.LabelCaptionGrid.TabIndex = 30
        Me.LabelCaptionGrid.Text = "Каналы конфигурации ИВК"
        Me.LabelCaptionGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(243, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.DataGridViewOutputChannel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ChannelConfigBindingNavigator)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelSelectedChannel)
        Me.SplitContainer2.Panel2Collapsed = True
        Me.SplitContainer2.Size = New System.Drawing.Size(462, 622)
        Me.SplitContainer2.SplitterDistance = 255
        Me.SplitContainer2.TabIndex = 0
        '
        'DataGridViewOutputChannel
        '
        Me.DataGridViewOutputChannel.AllowDrop = True
        Me.DataGridViewOutputChannel.AllowUserToAddRows = False
        Me.DataGridViewOutputChannel.AllowUserToDeleteRows = False
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Lavender
        Me.DataGridViewOutputChannel.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewOutputChannel.AutoGenerateColumns = False
        Me.DataGridViewOutputChannel.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridViewOutputChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewOutputChannel.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewOutputChannel.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewOutputChannel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewOutputChannel.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnOutputName, Me.DataGridViewTextBoxColumnGroupCh})
        Me.DataGridViewOutputChannel.DataSource = Me.BindingSourceoOutputChannels
        Me.DataGridViewOutputChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewOutputChannel.Location = New System.Drawing.Point(0, 47)
        Me.DataGridViewOutputChannel.Name = "DataGridViewOutputChannel"
        Me.DataGridViewOutputChannel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewOutputChannel.Size = New System.Drawing.Size(458, 571)
        Me.DataGridViewOutputChannel.TabIndex = 32
        '
        'DataGridViewTextBoxColumnOutputName
        '
        Me.DataGridViewTextBoxColumnOutputName.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumnOutputName.HeaderText = "Имя"
        Me.DataGridViewTextBoxColumnOutputName.Name = "DataGridViewTextBoxColumnOutputName"
        Me.DataGridViewTextBoxColumnOutputName.ReadOnly = True
        '
        'DataGridViewTextBoxColumnGroupCh
        '
        Me.DataGridViewTextBoxColumnGroupCh.DataPropertyName = "GroupCh"
        Me.DataGridViewTextBoxColumnGroupCh.HeaderText = "Группа"
        Me.DataGridViewTextBoxColumnGroupCh.Name = "DataGridViewTextBoxColumnGroupCh"
        Me.DataGridViewTextBoxColumnGroupCh.ReadOnly = True
        '
        'BindingSourceoOutputChannels
        '
        Me.BindingSourceoOutputChannels.DataSource = GetType(Registration.ChannelsOutputBindingList)
        '
        'ChannelConfigBindingNavigator
        '
        Me.ChannelConfigBindingNavigator.AddNewItem = Nothing
        Me.ChannelConfigBindingNavigator.BindingSource = Me.BindingSourceoOutputChannels
        Me.ChannelConfigBindingNavigator.CountItem = Me.BindingNavigator2CountItem
        Me.ChannelConfigBindingNavigator.DeleteItem = Nothing
        Me.ChannelConfigBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigator2MoveFirstItem, Me.BindingNavigator2MovePreviousItem, Me.ToolStripSeparator9, Me.BindingNavigator2PositionItem, Me.BindingNavigator2CountItem, Me.ToolStripSeparator10, Me.BindingNavigator2MoveNextItem, Me.BindingNavigator2MoveLastItem})
        Me.ChannelConfigBindingNavigator.Location = New System.Drawing.Point(0, 22)
        Me.ChannelConfigBindingNavigator.MoveFirstItem = Me.BindingNavigator2MoveFirstItem
        Me.ChannelConfigBindingNavigator.MoveLastItem = Me.BindingNavigator2MoveLastItem
        Me.ChannelConfigBindingNavigator.MoveNextItem = Me.BindingNavigator2MoveNextItem
        Me.ChannelConfigBindingNavigator.MovePreviousItem = Me.BindingNavigator2MovePreviousItem
        Me.ChannelConfigBindingNavigator.Name = "ChannelConfigBindingNavigator"
        Me.ChannelConfigBindingNavigator.PositionItem = Me.BindingNavigator2PositionItem
        Me.ChannelConfigBindingNavigator.Size = New System.Drawing.Size(458, 25)
        Me.ChannelConfigBindingNavigator.TabIndex = 31
        Me.ChannelConfigBindingNavigator.Text = "ChannelConfigBindingNavigator"
        '
        'BindingNavigator2CountItem
        '
        Me.BindingNavigator2CountItem.Name = "BindingNavigator2CountItem"
        Me.BindingNavigator2CountItem.Size = New System.Drawing.Size(43, 22)
        Me.BindingNavigator2CountItem.Text = "для {0}"
        Me.BindingNavigator2CountItem.ToolTipText = "Общее число элементов"
        '
        'BindingNavigator2MoveFirstItem
        '
        Me.BindingNavigator2MoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigator2MoveFirstItem.Image = CType(resources.GetObject("BindingNavigator2MoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigator2MoveFirstItem.Name = "BindingNavigator2MoveFirstItem"
        Me.BindingNavigator2MoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigator2MoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigator2MoveFirstItem.Text = "Переместить в начало"
        '
        'BindingNavigator2MovePreviousItem
        '
        Me.BindingNavigator2MovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigator2MovePreviousItem.Image = CType(resources.GetObject("BindingNavigator2MovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigator2MovePreviousItem.Name = "BindingNavigator2MovePreviousItem"
        Me.BindingNavigator2MovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigator2MovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigator2MovePreviousItem.Text = "Переместить назад"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigator2PositionItem
        '
        Me.BindingNavigator2PositionItem.AccessibleName = "Положение"
        Me.BindingNavigator2PositionItem.AutoSize = False
        Me.BindingNavigator2PositionItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BindingNavigator2PositionItem.Name = "BindingNavigator2PositionItem"
        Me.BindingNavigator2PositionItem.Size = New System.Drawing.Size(50, 23)
        Me.BindingNavigator2PositionItem.Text = "0"
        Me.BindingNavigator2PositionItem.ToolTipText = "Текущее положение"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigator2MoveNextItem
        '
        Me.BindingNavigator2MoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigator2MoveNextItem.Image = CType(resources.GetObject("BindingNavigator2MoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigator2MoveNextItem.Name = "BindingNavigator2MoveNextItem"
        Me.BindingNavigator2MoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigator2MoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigator2MoveNextItem.Text = "Переместить вперед"
        '
        'BindingNavigator2MoveLastItem
        '
        Me.BindingNavigator2MoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigator2MoveLastItem.Image = CType(resources.GetObject("BindingNavigator2MoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigator2MoveLastItem.Name = "BindingNavigator2MoveLastItem"
        Me.BindingNavigator2MoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigator2MoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigator2MoveLastItem.Text = "Переместить в конец"
        '
        'LabelSelectedChannel
        '
        Me.LabelSelectedChannel.BackColor = System.Drawing.Color.RoyalBlue
        Me.LabelSelectedChannel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelSelectedChannel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectedChannel.ForeColor = System.Drawing.Color.White
        Me.LabelSelectedChannel.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectedChannel.Name = "LabelSelectedChannel"
        Me.LabelSelectedChannel.Size = New System.Drawing.Size(458, 22)
        Me.LabelSelectedChannel.TabIndex = 33
        Me.LabelSelectedChannel.Text = "Выбранные каналы"
        Me.LabelSelectedChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelButtons
        '
        Me.PanelButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelButtons.Controls.Add(Me.PanelConfigurations)
        Me.PanelButtons.Controls.Add(Me.TableLayoutPanelBotton)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelButtons.Location = New System.Drawing.Point(0, 0)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(243, 622)
        Me.PanelButtons.TabIndex = 23
        '
        'PanelConfigurations
        '
        Me.PanelConfigurations.Controls.Add(Me.ComboBoxConfigurations)
        Me.PanelConfigurations.Controls.Add(Me.TableLayoutPanelBottonCFG)
        Me.PanelConfigurations.Controls.Add(Me.LabelConfigurations)
        Me.PanelConfigurations.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelConfigurations.Location = New System.Drawing.Point(0, 437)
        Me.PanelConfigurations.Name = "PanelConfigurations"
        Me.PanelConfigurations.Size = New System.Drawing.Size(239, 181)
        Me.PanelConfigurations.TabIndex = 36
        '
        'ComboBoxConfigurations
        '
        Me.ComboBoxConfigurations.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxConfigurations.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxConfigurations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBoxConfigurations.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxConfigurations.Location = New System.Drawing.Point(0, 22)
        Me.ComboBoxConfigurations.Name = "ComboBoxConfigurations"
        Me.ComboBoxConfigurations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxConfigurations.Size = New System.Drawing.Size(239, 113)
        Me.ComboBoxConfigurations.TabIndex = 34
        '
        'TableLayoutPanelBottonCFG
        '
        Me.TableLayoutPanelBottonCFG.ColumnCount = 3
        Me.TableLayoutPanelBottonCFG.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelBottonCFG.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelBottonCFG.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelBottonCFG.Controls.Add(Me.ButtonRestoreConfiguration, 0, 0)
        Me.TableLayoutPanelBottonCFG.Controls.Add(Me.ButtonSaveConfiguration, 1, 0)
        Me.TableLayoutPanelBottonCFG.Controls.Add(Me.ButtonRemoveSelectedConfiguration, 2, 0)
        Me.TableLayoutPanelBottonCFG.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanelBottonCFG.Location = New System.Drawing.Point(0, 135)
        Me.TableLayoutPanelBottonCFG.Name = "TableLayoutPanelBottonCFG"
        Me.TableLayoutPanelBottonCFG.RowCount = 1
        Me.TableLayoutPanelBottonCFG.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBottonCFG.Size = New System.Drawing.Size(239, 46)
        Me.TableLayoutPanelBottonCFG.TabIndex = 36
        '
        'LabelConfigurations
        '
        Me.LabelConfigurations.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelConfigurations.ForeColor = System.Drawing.Color.Blue
        Me.LabelConfigurations.Location = New System.Drawing.Point(0, 0)
        Me.LabelConfigurations.Name = "LabelConfigurations"
        Me.LabelConfigurations.Size = New System.Drawing.Size(239, 22)
        Me.LabelConfigurations.TabIndex = 35
        Me.LabelConfigurations.Text = "Существующие конфигурации"
        Me.LabelConfigurations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanelBotton
        '
        Me.TableLayoutPanelBotton.ColumnCount = 1
        Me.TableLayoutPanelBotton.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonUdateTCP, 0, 7)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonDown, 0, 5)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonAddChannel, 0, 0)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonRmoveChannel, 0, 1)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonClearChannels, 0, 2)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonSeparate1, 0, 3)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonSeparate2, 0, 6)
        Me.TableLayoutPanelBotton.Controls.Add(Me.ButtonUp, 0, 4)
        Me.TableLayoutPanelBotton.Location = New System.Drawing.Point(85, 20)
        Me.TableLayoutPanelBotton.Name = "TableLayoutPanelBotton"
        Me.TableLayoutPanelBotton.RowCount = 8
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelBotton.Size = New System.Drawing.Size(79, 331)
        Me.TableLayoutPanelBotton.TabIndex = 26
        '
        'ButtonSeparate1
        '
        Me.ButtonSeparate1.BackColor = System.Drawing.Color.Silver
        Me.ButtonSeparate1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonSeparate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSeparate1.ForeColor = System.Drawing.Color.Black
        Me.ButtonSeparate1.Location = New System.Drawing.Point(3, 153)
        Me.ButtonSeparate1.Name = "ButtonSeparate1"
        Me.ButtonSeparate1.Size = New System.Drawing.Size(73, 9)
        Me.ButtonSeparate1.TabIndex = 23
        Me.ButtonSeparate1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonSeparate1.UseVisualStyleBackColor = False
        '
        'ButtonSeparate2
        '
        Me.ButtonSeparate2.BackColor = System.Drawing.Color.Silver
        Me.ButtonSeparate2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonSeparate2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSeparate2.ForeColor = System.Drawing.Color.Black
        Me.ButtonSeparate2.Location = New System.Drawing.Point(3, 268)
        Me.ButtonSeparate2.Name = "ButtonSeparate2"
        Me.ButtonSeparate2.Size = New System.Drawing.Size(73, 9)
        Me.ButtonSeparate2.TabIndex = 26
        Me.ButtonSeparate2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonSeparate2.UseVisualStyleBackColor = False
        '
        'PanelOptions
        '
        Me.PanelOptions.Controls.Add(Me.GroupBoxСтендКонфигСервер)
        Me.PanelOptions.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelOptions.Location = New System.Drawing.Point(0, 0)
        Me.PanelOptions.Name = "PanelOptions"
        Me.PanelOptions.Size = New System.Drawing.Size(1008, 60)
        Me.PanelOptions.TabIndex = 16
        '
        'GroupBoxСтендКонфигСервер
        '
        Me.GroupBoxСтендКонфигСервер.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxСтендКонфигСервер.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBoxСтендКонфигСервер.Controls.Add(Me.ButtonSave)
        Me.GroupBoxСтендКонфигСервер.Controls.Add(Me.ComboBoxPathServerConfigXML)
        Me.GroupBoxСтендКонфигСервер.Controls.Add(Me.TextBoxPathServerConfigXML)
        Me.GroupBoxСтендКонфигСервер.Controls.Add(Me.ButtonExplorerServerConfigXML)
        Me.GroupBoxСтендКонфигСервер.Controls.Add(Me.LabelStendServer)
        Me.GroupBoxСтендКонфигСервер.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxСтендКонфигСервер.Location = New System.Drawing.Point(12, 7)
        Me.GroupBoxСтендКонфигСервер.Name = "GroupBoxСтендКонфигСервер"
        Me.GroupBoxСтендКонфигСервер.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxСтендКонфигСервер.Size = New System.Drawing.Size(984, 48)
        Me.GroupBoxСтендКонфигСервер.TabIndex = 56
        Me.GroupBoxСтендКонфигСервер.TabStop = False
        Me.GroupBoxСтендКонфигСервер.Text = "Настройка путей к файлам конфигураций Серверов"
        '
        'TextBoxPathServerConfigXML
        '
        Me.TextBoxPathServerConfigXML.AcceptsReturn = True
        Me.TextBoxPathServerConfigXML.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxPathServerConfigXML.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxPathServerConfigXML.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxPathServerConfigXML.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxPathServerConfigXML.Location = New System.Drawing.Point(159, 16)
        Me.TextBoxPathServerConfigXML.MaxLength = 0
        Me.TextBoxPathServerConfigXML.Name = "TextBoxPathServerConfigXML"
        Me.TextBoxPathServerConfigXML.ReadOnly = True
        Me.TextBoxPathServerConfigXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxPathServerConfigXML.Size = New System.Drawing.Size(637, 20)
        Me.TextBoxPathServerConfigXML.TabIndex = 57
        '
        'LabelStendServer
        '
        Me.LabelStendServer.BackColor = System.Drawing.SystemColors.Control
        Me.LabelStendServer.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelStendServer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelStendServer.Location = New System.Drawing.Point(6, 19)
        Me.LabelStendServer.Name = "LabelStendServer"
        Me.LabelStendServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelStendServer.Size = New System.Drawing.Size(90, 17)
        Me.LabelStendServer.TabIndex = 59
        Me.LabelStendServer.Text = "Стенд сервера"
        '
        'TimeBindingSource
        '
        Me.TimeBindingSource.DataMember = "TimeChannel"
        Me.TimeBindingSource.DataSource = Me.ChannelsDataSet
        '
        'ChannelsDataSet
        '
        Me.ChannelsDataSet.DataSetName = "Channels_cfg_lmzDataSet"
        Me.ChannelsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ChannelBindingSource
        '
        Me.ChannelBindingSource.DataMember = "Channel"
        Me.ChannelBindingSource.DataSource = Me.ChannelsDataSet
        '
        'ChannelConfigBindingSource
        '
        Me.ChannelConfigBindingSource.DataMember = "ChannelConfig"
        Me.ChannelConfigBindingSource.DataSource = Me.ChannelsDataSet
        '
        'ImageListLarge
        '
        Me.ImageListLarge.ImageStream = CType(resources.GetObject("ImageListLarge.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListLarge.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListLarge.Images.SetKeyName(0, "Graph1")
        Me.ImageListLarge.Images.SetKeyName(1, "Graph2")
        Me.ImageListLarge.Images.SetKeyName(2, "Graph3")
        '
        'ImageListSmall
        '
        Me.ImageListSmall.ImageStream = CType(resources.GetObject("ImageListSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListSmall.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListSmall.Images.SetKeyName(0, "Graph1")
        Me.ImageListSmall.Images.SetKeyName(1, "Graph2")
        Me.ImageListSmall.Images.SetKeyName(2, "Graph3")
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'ChannelTableAdapter
        '
        Me.ChannelTableAdapter.ClearBeforeFill = True
        '
        'ChannelConfigTableAdapter
        '
        Me.ChannelConfigTableAdapter.ClearBeforeFill = True
        '
        'BindingSourceEdIzm
        '
        Me.BindingSourceEdIzm.DataMember = "ЕдиницаИзмерения"
        Me.BindingSourceEdIzm.DataSource = Me.ChannelsDataSet
        '
        'UnitTableAdapter
        '
        Me.UnitTableAdapter.ClearBeforeFill = True
        '
        'TimeChannelTableAdapter
        '
        Me.TimeChannelTableAdapter.ClearBeforeFill = True
        '
        'TimeTableAdapter
        '
        Me.TimeTableAdapter.ClearBeforeFill = True
        '
        'ChannelsNBaseDataSet
        '
        Me.ChannelsNBaseDataSet.DataSetName = "ChannelsNDataSet"
        Me.ChannelsNBaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'BindingSourceChannelNBase
        '
        Me.BindingSourceChannelNBase.DataMember = "ChannelN"
        Me.BindingSourceChannelNBase.DataSource = Me.ChannelsNBaseDataSet
        '
        'ChannelNTableAdapter
        '
        Me.ChannelNTableAdapter.ClearBeforeFill = True
        '
        'FormConfigChannelServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 732)
        Me.Controls.Add(Me.ToolStripContainerForm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(940, 690)
        Me.Name = "FormConfigChannelServer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TCP клиент"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.MenuStripForm.ResumeLayout(False)
        Me.MenuStripForm.PerformLayout()
        Me.ToolStripContainerForm.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainerForm.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainerForm.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainerForm.ResumeLayout(False)
        Me.ToolStripContainerForm.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.PanelDrdFindChannel.ResumeLayout(False)
        CType(Me.DataGridViewFindChannel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanelShadowButtons.ResumeLayout(False)
        CType(Me.DataGridViewInputChannel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceInputChannels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ChannelBindingNavigator.ResumeLayout(False)
        Me.ChannelBindingNavigator.PerformLayout()
        Me.PanelFindChannel.ResumeLayout(False)
        Me.PanelFindChannel.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.DataGridViewOutputChannel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceoOutputChannels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelConfigBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ChannelConfigBindingNavigator.ResumeLayout(False)
        Me.ChannelConfigBindingNavigator.PerformLayout()
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelConfigurations.ResumeLayout(False)
        Me.TableLayoutPanelBottonCFG.ResumeLayout(False)
        Me.TableLayoutPanelBotton.ResumeLayout(False)
        Me.PanelOptions.ResumeLayout(False)
        Me.GroupBoxСтендКонфигСервер.ResumeLayout(False)
        Me.GroupBoxСтендКонфигСервер.PerformLayout()
        CType(Me.TimeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelConfigBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceEdIzm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelsNBaseDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceChannelNBase, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Public WithEvents DataGridViewInputChannel As System.Windows.Forms.DataGridView
    Friend WithEvents ChannelBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ChannelsDataSet As Channels_cfg_lmzDataSet
    Friend WithEvents LabelCaptionGrid As System.Windows.Forms.Label
    Friend WithEvents ChannelBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents PanelButtons As System.Windows.Forms.Panel
    Friend WithEvents ButtonUp As System.Windows.Forms.Button
    Friend WithEvents ButtonDown As System.Windows.Forms.Button
    Friend WithEvents ButtonSeparate1 As System.Windows.Forms.Button
    Friend WithEvents ButtonRemoveSelectedConfiguration As System.Windows.Forms.Button
    Friend WithEvents ButtonRestoreConfiguration As System.Windows.Forms.Button
    Friend WithEvents ButtonSaveConfiguration As System.Windows.Forms.Button
    Friend WithEvents ButtonAddChannel As System.Windows.Forms.Button
    Friend WithEvents ButtonRmoveChannel As System.Windows.Forms.Button
    Friend WithEvents ButtonClearChannels As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Public WithEvents DataGridViewOutputChannel As System.Windows.Forms.DataGridView
    Friend WithEvents LabelSelectedChannel As System.Windows.Forms.Label
    Friend WithEvents ChannelConfigBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigator2CountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigator2MoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigator2MovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigator2PositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigator2MoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigator2MoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ChannelTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ChannelTableAdapter
    Friend WithEvents PanelFindChannel As System.Windows.Forms.Panel
    Friend WithEvents LabelRowPosition As System.Windows.Forms.Label
    Friend WithEvents TextBoxFilter As System.Windows.Forms.TextBox
    Friend WithEvents LabelFindBy As System.Windows.Forms.Label
    Friend WithEvents LabelConfigurations As System.Windows.Forms.Label
    Public WithEvents ComboBoxConfigurations As System.Windows.Forms.ComboBox
    Friend WithEvents PanelConfigurations As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanelBottonCFG As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanelBotton As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ChannelConfigBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ChannelConfigTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter
    Friend WithEvents BindingSourceEdIzm As System.Windows.Forms.BindingSource
    Friend WithEvents UnitTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter
    Friend WithEvents TSButtonRemoveChannels As System.Windows.Forms.ToolStripButton
    Friend WithEvents НомерDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НазваниеDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ChannelDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UnitDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LengthDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MinimumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MaximumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NIDigitalLineDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WfxunitstringDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NIChannelNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NIUnitDescriptionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WfincrementDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WfsamplesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WfstartoffsetDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents WfstarttimeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ВключениеDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ЕдИзмерDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ТипDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents МинАварDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents МаксАварDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents МинГрафDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents МаксГрафDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents КонтрольDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ПримечаниеDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ТипОпросаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ИмяDAQDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ГруппаDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Public WithEvents BindingSourceInputChannels As System.Windows.Forms.BindingSource
    Public WithEvents BindingSourceoOutputChannels As System.Windows.Forms.BindingSource
    Friend WithEvents KeyConfigDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НомерПараметраDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents НаименованиеПараметраDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ЕдиницаИзмеренияDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ДопускМинимумDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ДопускМаксимумDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents РазносУминDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents РазносУмаксDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents АварийноеЗначениеМинDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents АварийноеЗначениеМаксDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PanelDrdFindChannel As System.Windows.Forms.Panel
    Friend WithEvents DataGridViewFindChannel As System.Windows.Forms.DataGridView
    Friend WithEvents TableLayoutPanelShadowButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonСhoiceConfirmed As System.Windows.Forms.Button
    Friend WithEvents IdDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KeyConfigDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnParameterNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnOutputName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnParameterName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnUnit As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnRamgeMin As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnRamgeMax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnRangeAxisYmin As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnRangeAxisYmax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnAlarmMin As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnAlarmMax As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnGroupCh As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ComboBoxFilter As System.Windows.Forms.ComboBox
    Friend WithEvents IdDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnNumberCh As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnInputName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnPin As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnScrName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnState As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnScrUnit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnChUnit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextBoxColumnGroupChDataGridView As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonSeparate2 As System.Windows.Forms.Button
    Friend WithEvents TimeChannelTableAdapter As Channels_cfg_lmzDataSetTableAdapters.TimeChannelTableAdapter
    Friend WithEvents TimeBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TimeTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ChannelTableAdapter
    Public WithEvents GroupBoxСтендКонфигСервер As System.Windows.Forms.GroupBox
    Public WithEvents ComboBoxPathServerConfigXML As System.Windows.Forms.ComboBox
    Public WithEvents TextBoxPathServerConfigXML As System.Windows.Forms.TextBox
    Public WithEvents ButtonExplorerServerConfigXML As System.Windows.Forms.Button
    Public WithEvents LabelStendServer As System.Windows.Forms.Label
    Public WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialogPathDB As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents ChannelsNBaseDataSet As ChannelsNDataSet
    Friend WithEvents BindingSourceChannelNBase As System.Windows.Forms.BindingSource
    Friend WithEvents ChannelNTableAdapter As ChannelsNDataSetTableAdapters.ChannelNTableAdapter
    Friend WithEvents ToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ButtonUdateTCP As Button
    Friend WithEvents PanelOptions As Panel
End Class
