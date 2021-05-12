Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormCompactRio
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

    Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ImageListFolder As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents BackToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ForwardToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ChannelsToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ListViewToolStripButton As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LargeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SmallIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChannelsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainerTree As System.Windows.Forms.SplitContainer
    Friend WithEvents ChannelsTree As System.Windows.Forms.TreeView
    Friend WithEvents ListView As System.Windows.Forms.ListView
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCompactRio))
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSStatusLabelRecord = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ImageListFolder = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.BackToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ForwardToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ChannelsToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ListViewToolStripButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LargeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SmallIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChannelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdViewLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ListView = New System.Windows.Forms.ListView()
        Me.ListContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuRebootChassis = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRebootAllChassis = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmbBoxCommand = New System.Windows.Forms.ComboBox()
        Me.ApplyCommandButton = New System.Windows.Forms.Button()
        Me.RadioButtonViewModule = New System.Windows.Forms.RadioButton()
        Me.RadioButtonViewType = New System.Windows.Forms.RadioButton()
        Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainerMain = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerTree = New System.Windows.Forms.SplitContainer()
        Me.ChannelsTree = New System.Windows.Forms.TreeView()
        Me.ImageListTree = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelView = New System.Windows.Forms.Panel()
        Me.LabelGroup = New System.Windows.Forms.Label()
        Me.SplitContainerGridAndList = New System.Windows.Forms.SplitContainer()
        Me.grdChassis = New System.Windows.Forms.DataGridView()
        Me.NameChassisDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusAdapterDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusAdapterImageDataGridViewImageColumn = New System.Windows.Forms.DataGridViewImageColumn()
        Me.StatusSendImageDataGridViewImageColumn = New System.Windows.Forms.DataGridViewImageColumn()
        Me.StatusReceiveImageDataGridViewImageColumn = New System.Windows.Forms.DataGridViewImageColumn()
        Me.PacketsReceiveDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BindingSourceChassis = New System.Windows.Forms.BindingSource(Me.components)
        Me.PanelServerSSD = New System.Windows.Forms.Panel()
        Me.TabLogControl = New System.Windows.Forms.TabControl()
        Me.TabPageServer = New System.Windows.Forms.TabPage()
        Me.RichTextBoxServer = New System.Windows.Forms.RichTextBox()
        Me.ToolStripServer = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.SSDPortTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me.StartStopButton = New System.Windows.Forms.ToolStripButton()
        Me.ActivateTargetButton = New System.Windows.Forms.ToolStripButton()
        Me.TabPageClient = New System.Windows.Forms.TabPage()
        Me.RichTextBoxClient = New System.Windows.Forms.RichTextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ImageListStatus = New System.Windows.Forms.ImageList(Me.components)
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.StatusStrip.SuspendLayout()
        Me.ToolStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.ListContextMenu.SuspendLayout()
        Me.ToolStripContainer.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.ContentPanel.SuspendLayout()
        Me.ToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.SuspendLayout()
        CType(Me.SplitContainerMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerMain.Panel1.SuspendLayout()
        Me.SplitContainerMain.Panel2.SuspendLayout()
        Me.SplitContainerMain.SuspendLayout()
        CType(Me.SplitContainerTree, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerTree.Panel1.SuspendLayout()
        Me.SplitContainerTree.Panel2.SuspendLayout()
        Me.SplitContainerTree.SuspendLayout()
        Me.PanelView.SuspendLayout()
        CType(Me.SplitContainerGridAndList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGridAndList.Panel1.SuspendLayout()
        Me.SplitContainerGridAndList.Panel2.SuspendLayout()
        Me.SplitContainerGridAndList.SuspendLayout()
        CType(Me.grdChassis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceChassis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelServerSSD.SuspendLayout()
        Me.TabLogControl.SuspendLayout()
        Me.TabPageServer.SuspendLayout()
        Me.ToolStripServer.SuspendLayout()
        Me.TabPageClient.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel, Me.TSStatusLabelRecord})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1264, 25)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.ToolStripStatusLabel.Image = CType(resources.GetObject("ToolStripStatusLabel.Image"), System.Drawing.Image)
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(108, 20)
        Me.ToolStripStatusLabel.Text = "0 Подключено"
        Me.ToolStripStatusLabel.ToolTipText = "Количество подключённых шасси"
        '
        'TSStatusLabelRecord
        '
        Me.TSStatusLabelRecord.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.TSStatusLabelRecord.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.TSStatusLabelRecord.Image = Global.Registration.My.Resources.Resources.AcquisitionStop
        Me.TSStatusLabelRecord.Name = "TSStatusLabelRecord"
        Me.TSStatusLabelRecord.Size = New System.Drawing.Size(20, 20)
        Me.TSStatusLabelRecord.ToolTipText = "Индикатор включения записи замеров на ИВК"
        '
        'ImageListFolder
        '
        Me.ImageListFolder.ImageStream = CType(resources.GetObject("ImageListFolder.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListFolder.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListFolder.Images.SetKeyName(0, "ClosedFolder")
        Me.ImageListFolder.Images.SetKeyName(1, "OpenFolder")
        '
        'ToolStrip
        '
        Me.ToolStrip.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackToolStripButton, Me.ForwardToolStripButton, Me.ToolStripSeparator7, Me.ChannelsToolStripButton, Me.ToolStripSeparator8, Me.ListViewToolStripButton})
        Me.ToolStrip.Location = New System.Drawing.Point(3, 24)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(247, 25)
        Me.ToolStrip.TabIndex = 0
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'BackToolStripButton
        '
        Me.BackToolStripButton.Enabled = False
        Me.BackToolStripButton.Image = CType(resources.GetObject("BackToolStripButton.Image"), System.Drawing.Image)
        Me.BackToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.BackToolStripButton.Name = "BackToolStripButton"
        Me.BackToolStripButton.Size = New System.Drawing.Size(59, 22)
        Me.BackToolStripButton.Text = "Назад"
        Me.BackToolStripButton.ToolTipText = "Назад к предыдущему шасси"
        '
        'ForwardToolStripButton
        '
        Me.ForwardToolStripButton.Enabled = False
        Me.ForwardToolStripButton.Image = CType(resources.GetObject("ForwardToolStripButton.Image"), System.Drawing.Image)
        Me.ForwardToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.ForwardToolStripButton.Name = "ForwardToolStripButton"
        Me.ForwardToolStripButton.Size = New System.Drawing.Size(66, 22)
        Me.ForwardToolStripButton.Text = "Вперед"
        Me.ForwardToolStripButton.ToolTipText = "Вперед к следующему шасси"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'ChannelsToolStripButton
        '
        Me.ChannelsToolStripButton.Image = CType(resources.GetObject("ChannelsToolStripButton.Image"), System.Drawing.Image)
        Me.ChannelsToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.ChannelsToolStripButton.Name = "ChannelsToolStripButton"
        Me.ChannelsToolStripButton.Size = New System.Drawing.Size(69, 22)
        Me.ChannelsToolStripButton.Text = "Каналы"
        Me.ChannelsToolStripButton.ToolTipText = "Отобразить проводник по каналам Шасси"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'ListViewToolStripButton
        '
        Me.ListViewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ListViewToolStripButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ListToolStripMenuItem, Me.DetailsToolStripMenuItem, Me.LargeIconsToolStripMenuItem, Me.SmallIconsToolStripMenuItem, Me.TileToolStripMenuItem})
        Me.ListViewToolStripButton.Image = CType(resources.GetObject("ListViewToolStripButton.Image"), System.Drawing.Image)
        Me.ListViewToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.ListViewToolStripButton.Name = "ListViewToolStripButton"
        Me.ListViewToolStripButton.Size = New System.Drawing.Size(29, 22)
        Me.ListViewToolStripButton.Text = "Представления"
        Me.ListViewToolStripButton.ToolTipText = "Представления списка шасси"
        '
        'ListToolStripMenuItem
        '
        Me.ListToolStripMenuItem.Name = "ListToolStripMenuItem"
        Me.ListToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ListToolStripMenuItem.Text = "Список"
        '
        'DetailsToolStripMenuItem
        '
        Me.DetailsToolStripMenuItem.Checked = True
        Me.DetailsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DetailsToolStripMenuItem.Name = "DetailsToolStripMenuItem"
        Me.DetailsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.DetailsToolStripMenuItem.Text = "Таблица"
        '
        'LargeIconsToolStripMenuItem
        '
        Me.LargeIconsToolStripMenuItem.Name = "LargeIconsToolStripMenuItem"
        Me.LargeIconsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.LargeIconsToolStripMenuItem.Text = "Крупные значки"
        '
        'SmallIconsToolStripMenuItem
        '
        Me.SmallIconsToolStripMenuItem.Name = "SmallIconsToolStripMenuItem"
        Me.SmallIconsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.SmallIconsToolStripMenuItem.Text = "Мелкие значки"
        '
        'TileToolStripMenuItem
        '
        Me.TileToolStripMenuItem.Name = "TileToolStripMenuItem"
        Me.TileToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.TileToolStripMenuItem.Text = "Рядом"
        '
        'MenuStrip
        '
        Me.MenuStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1264, 24)
        Me.MenuStrip.TabIndex = 0
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolBarToolStripMenuItem, Me.StatusBarToolStripMenuItem, Me.ChannelsToolStripMenuItem, Me.cmdViewLog})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.ViewToolStripMenuItem.Text = "&Вид"
        '
        'ToolBarToolStripMenuItem
        '
        Me.ToolBarToolStripMenuItem.Checked = True
        Me.ToolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolBarToolStripMenuItem.Name = "ToolBarToolStripMenuItem"
        Me.ToolBarToolStripMenuItem.Size = New System.Drawing.Size(317, 22)
        Me.ToolBarToolStripMenuItem.Text = "&Панель инструментов"
        Me.ToolBarToolStripMenuItem.ToolTipText = "Отобразить панель инструментов"
        '
        'StatusBarToolStripMenuItem
        '
        Me.StatusBarToolStripMenuItem.Checked = True
        Me.StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        Me.StatusBarToolStripMenuItem.Size = New System.Drawing.Size(317, 22)
        Me.StatusBarToolStripMenuItem.Text = "&Строка состояния"
        Me.StatusBarToolStripMenuItem.ToolTipText = "Отобразить панель состояния"
        '
        'ChannelsToolStripMenuItem
        '
        Me.ChannelsToolStripMenuItem.Name = "ChannelsToolStripMenuItem"
        Me.ChannelsToolStripMenuItem.Size = New System.Drawing.Size(317, 22)
        Me.ChannelsToolStripMenuItem.Text = "&Каналы"
        Me.ChannelsToolStripMenuItem.ToolTipText = "Отобразить проводник по каналам Шасси"
        '
        'cmdViewLog
        '
        Me.cmdViewLog.Name = "cmdViewLog"
        Me.cmdViewLog.Size = New System.Drawing.Size(317, 22)
        Me.cmdViewLog.Text = "Просмотреть необработанные исключения"
        '
        'ListView
        '
        Me.ListView.ContextMenuStrip = Me.ListContextMenu
        Me.ListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView.HideSelection = False
        Me.ListView.Location = New System.Drawing.Point(0, 0)
        Me.ListView.MultiSelect = False
        Me.ListView.Name = "ListView"
        Me.ListView.Size = New System.Drawing.Size(636, 338)
        Me.ListView.TabIndex = 0
        Me.ToolTip.SetToolTip(Me.ListView, "Лист свойста шасси")
        Me.ListView.UseCompatibleStateImageBehavior = False
        Me.ListView.View = System.Windows.Forms.View.SmallIcon
        '
        'ListContextMenu
        '
        Me.ListContextMenu.ImageScalingSize = New System.Drawing.Size(18, 18)
        Me.ListContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRebootChassis, Me.mnuRebootAllChassis})
        Me.ListContextMenu.Name = "КонтекстМенюДерева"
        Me.ListContextMenu.Size = New System.Drawing.Size(196, 52)
        '
        'mnuRebootChassis
        '
        Me.mnuRebootChassis.Image = CType(resources.GetObject("mnuRebootChassis.Image"), System.Drawing.Image)
        Me.mnuRebootChassis.Name = "mnuRebootChassis"
        Me.mnuRebootChassis.Size = New System.Drawing.Size(195, 24)
        Me.mnuRebootChassis.Text = "&Перезагрузить шасси"
        Me.mnuRebootChassis.ToolTipText = "Перезагрузить выделенное шасси"
        '
        'mnuRebootAllChassis
        '
        Me.mnuRebootAllChassis.Image = CType(resources.GetObject("mnuRebootAllChassis.Image"), System.Drawing.Image)
        Me.mnuRebootAllChassis.Name = "mnuRebootAllChassis"
        Me.mnuRebootAllChassis.Size = New System.Drawing.Size(195, 24)
        Me.mnuRebootAllChassis.Text = "Перезагрузить ИВК"
        Me.mnuRebootAllChassis.ToolTipText = "Перезагрузить все шасси"
        '
        'CmbBoxCommand
        '
        Me.CmbBoxCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CmbBoxCommand.FormattingEnabled = True
        Me.CmbBoxCommand.Location = New System.Drawing.Point(3, 3)
        Me.CmbBoxCommand.Name = "CmbBoxCommand"
        Me.CmbBoxCommand.Size = New System.Drawing.Size(488, 21)
        Me.CmbBoxCommand.TabIndex = 5
        Me.ToolTip.SetToolTip(Me.CmbBoxCommand, "Список предопределённых команд для исполнения на ИВК")
        '
        'ApplyCommandButton
        '
        Me.ApplyCommandButton.AutoSize = True
        Me.ApplyCommandButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ApplyCommandButton.Enabled = False
        Me.ApplyCommandButton.Location = New System.Drawing.Point(497, 3)
        Me.ApplyCommandButton.Name = "ApplyCommandButton"
        Me.ApplyCommandButton.Size = New System.Drawing.Size(104, 24)
        Me.ApplyCommandButton.TabIndex = 4
        Me.ApplyCommandButton.Text = "Выполнить"
        Me.ToolTip.SetToolTip(Me.ApplyCommandButton, "Выполнить выбранную в списке команду")
        Me.ApplyCommandButton.UseVisualStyleBackColor = True
        '
        'RadioButtonViewModule
        '
        Me.RadioButtonViewModule.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonViewModule.Appearance = System.Windows.Forms.Appearance.Button
        Me.RadioButtonViewModule.BackColor = System.Drawing.Color.Blue
        Me.RadioButtonViewModule.Checked = True
        Me.RadioButtonViewModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButtonViewModule.ForeColor = System.Drawing.Color.Yellow
        Me.RadioButtonViewModule.Image = CType(resources.GetObject("RadioButtonViewModule.Image"), System.Drawing.Image)
        Me.RadioButtonViewModule.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RadioButtonViewModule.Location = New System.Drawing.Point(8, 24)
        Me.RadioButtonViewModule.Name = "RadioButtonViewModule"
        Me.RadioButtonViewModule.Size = New System.Drawing.Size(268, 24)
        Me.RadioButtonViewModule.TabIndex = 20
        Me.RadioButtonViewModule.TabStop = True
        Me.RadioButtonViewModule.Text = "Параметры по модулям"
        Me.RadioButtonViewModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip.SetToolTip(Me.RadioButtonViewModule, "Группировать параметры по модулям")
        Me.RadioButtonViewModule.UseVisualStyleBackColor = False
        '
        'RadioButtonViewType
        '
        Me.RadioButtonViewType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButtonViewType.Appearance = System.Windows.Forms.Appearance.Button
        Me.RadioButtonViewType.BackColor = System.Drawing.Color.Silver
        Me.RadioButtonViewType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButtonViewType.Image = CType(resources.GetObject("RadioButtonViewType.Image"), System.Drawing.Image)
        Me.RadioButtonViewType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RadioButtonViewType.Location = New System.Drawing.Point(8, 56)
        Me.RadioButtonViewType.Name = "RadioButtonViewType"
        Me.RadioButtonViewType.Size = New System.Drawing.Size(268, 24)
        Me.RadioButtonViewType.TabIndex = 21
        Me.RadioButtonViewType.Text = "Параметры по типам"
        Me.RadioButtonViewType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip.SetToolTip(Me.RadioButtonViewType, "Группировать параметры по типу замеров")
        Me.RadioButtonViewType.UseVisualStyleBackColor = False
        '
        'ToolStripContainer
        '
        '
        'ToolStripContainer.BottomToolStripPanel
        '
        Me.ToolStripContainer.BottomToolStripPanel.Controls.Add(Me.StatusStrip)
        '
        'ToolStripContainer.ContentPanel
        '
        Me.ToolStripContainer.ContentPanel.Controls.Add(Me.SplitContainerMain)
        Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(1264, 687)
        Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer.Name = "ToolStripContainer"
        Me.ToolStripContainer.Size = New System.Drawing.Size(1264, 761)
        Me.ToolStripContainer.TabIndex = 7
        Me.ToolStripContainer.Text = "ToolStripContainer1"
        '
        'ToolStripContainer.TopToolStripPanel
        '
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.MenuStrip)
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip)
        '
        'SplitContainerMain
        '
        Me.SplitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerMain.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerMain.Name = "SplitContainerMain"
        '
        'SplitContainerMain.Panel1
        '
        Me.SplitContainerMain.Panel1.Controls.Add(Me.SplitContainerTree)
        Me.SplitContainerMain.Panel1MinSize = 100
        '
        'SplitContainerMain.Panel2
        '
        Me.SplitContainerMain.Panel2.Controls.Add(Me.PanelServerSSD)
        Me.SplitContainerMain.Panel2MinSize = 100
        Me.SplitContainerMain.Size = New System.Drawing.Size(1264, 687)
        Me.SplitContainerMain.SplitterDistance = 640
        Me.SplitContainerMain.TabIndex = 2
        '
        'SplitContainerTree
        '
        Me.SplitContainerTree.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerTree.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerTree.Name = "SplitContainerTree"
        '
        'SplitContainerTree.Panel1
        '
        Me.SplitContainerTree.Panel1.Controls.Add(Me.ChannelsTree)
        Me.SplitContainerTree.Panel1.Controls.Add(Me.PanelView)
        Me.SplitContainerTree.Panel1Collapsed = True
        Me.SplitContainerTree.Panel1MinSize = 100
        '
        'SplitContainerTree.Panel2
        '
        Me.SplitContainerTree.Panel2.Controls.Add(Me.SplitContainerGridAndList)
        Me.SplitContainerTree.Panel2MinSize = 100
        Me.SplitContainerTree.Size = New System.Drawing.Size(640, 687)
        Me.SplitContainerTree.SplitterDistance = 300
        Me.SplitContainerTree.TabIndex = 0
        Me.SplitContainerTree.Text = "SplitContainer1"
        '
        'ChannelsTree
        '
        Me.ChannelsTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChannelsTree.HideSelection = False
        Me.ChannelsTree.HotTracking = True
        Me.ChannelsTree.ImageIndex = 13
        Me.ChannelsTree.ImageList = Me.ImageListTree
        Me.ChannelsTree.Location = New System.Drawing.Point(0, 96)
        Me.ChannelsTree.Name = "ChannelsTree"
        Me.ChannelsTree.SelectedImageIndex = 14
        Me.ChannelsTree.Size = New System.Drawing.Size(296, 0)
        Me.ChannelsTree.TabIndex = 0
        '
        'ImageListTree
        '
        Me.ImageListTree.ImageStream = CType(resources.GetObject("ImageListTree.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTree.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTree.Images.SetKeyName(0, "Rotation0")
        Me.ImageListTree.Images.SetKeyName(1, "Discrete1")
        Me.ImageListTree.Images.SetKeyName(2, "Evacuation2")
        Me.ImageListTree.Images.SetKeyName(3, "Temperature3")
        Me.ImageListTree.Images.SetKeyName(4, "Petrol4")
        Me.ImageListTree.Images.SetKeyName(5, "Vibration5")
        Me.ImageListTree.Images.SetKeyName(6, "Current6")
        Me.ImageListTree.Images.SetKeyName(7, "Presure7")
        Me.ImageListTree.Images.SetKeyName(8, "Traction8")
        Me.ImageListTree.Images.SetKeyName(9, "SliderVertical9")
        Me.ImageListTree.Images.SetKeyName(10, "SliderHorizontal10")
        Me.ImageListTree.Images.SetKeyName(11, "SliderVertical11")
        Me.ImageListTree.Images.SetKeyName(12, "Close12")
        Me.ImageListTree.Images.SetKeyName(13, "Chassis13")
        Me.ImageListTree.Images.SetKeyName(14, "ChassisSelected14")
        Me.ImageListTree.Images.SetKeyName(15, "Module15")
        Me.ImageListTree.Images.SetKeyName(16, "ModuleSelected16")
        Me.ImageListTree.Images.SetKeyName(17, "DAQBoard17")
        Me.ImageListTree.Images.SetKeyName(18, "DAQBoardSelected18")
        Me.ImageListTree.Images.SetKeyName(19, "Selected19")
        Me.ImageListTree.Images.SetKeyName(20, "ColumnModule")
        Me.ImageListTree.Images.SetKeyName(21, "ColumnChassis")
        Me.ImageListTree.Images.SetKeyName(22, "ColumnIPAddress")
        Me.ImageListTree.Images.SetKeyName(23, "ColumnMode")
        Me.ImageListTree.Images.SetKeyName(24, "ColumnAllChannels")
        Me.ImageListTree.Images.SetKeyName(25, "ColumnRevolution")
        Me.ImageListTree.Images.SetKeyName(26, "ColumnDiscrete")
        Me.ImageListTree.Images.SetKeyName(27, "ColumnEvacuation")
        Me.ImageListTree.Images.SetKeyName(28, "ColumnTemperature")
        Me.ImageListTree.Images.SetKeyName(29, "ColumnPresure")
        Me.ImageListTree.Images.SetKeyName(30, "ColumnVibration")
        Me.ImageListTree.Images.SetKeyName(31, "ColumnCurrent")
        Me.ImageListTree.Images.SetKeyName(32, "ColumnPetrol")
        Me.ImageListTree.Images.SetKeyName(33, "ColumnTraction")
        Me.ImageListTree.Images.SetKeyName(34, "ColumnNet")
        Me.ImageListTree.Images.SetKeyName(35, "ColumnDac")
        Me.ImageListTree.Images.SetKeyName(36, "ColumnCommand")
        '
        'PanelView
        '
        Me.PanelView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelView.Controls.Add(Me.LabelGroup)
        Me.PanelView.Controls.Add(Me.RadioButtonViewModule)
        Me.PanelView.Controls.Add(Me.RadioButtonViewType)
        Me.PanelView.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelView.Location = New System.Drawing.Point(0, 0)
        Me.PanelView.Name = "PanelView"
        Me.PanelView.Size = New System.Drawing.Size(296, 96)
        Me.PanelView.TabIndex = 24
        '
        'LabelGroup
        '
        Me.LabelGroup.BackColor = System.Drawing.SystemColors.Control
        Me.LabelGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelGroup.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelGroup.Image = CType(resources.GetObject("LabelGroup.Image"), System.Drawing.Image)
        Me.LabelGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LabelGroup.Location = New System.Drawing.Point(0, 0)
        Me.LabelGroup.Name = "LabelGroup"
        Me.LabelGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelGroup.Size = New System.Drawing.Size(292, 22)
        Me.LabelGroup.TabIndex = 12
        Me.LabelGroup.Text = "Группировать параметры по:"
        Me.LabelGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainerGridAndList
        '
        Me.SplitContainerGridAndList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGridAndList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGridAndList.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGridAndList.Name = "SplitContainerGridAndList"
        Me.SplitContainerGridAndList.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerGridAndList.Panel1
        '
        Me.SplitContainerGridAndList.Panel1.Controls.Add(Me.grdChassis)
        Me.SplitContainerGridAndList.Panel1MinSize = 100
        '
        'SplitContainerGridAndList.Panel2
        '
        Me.SplitContainerGridAndList.Panel2.Controls.Add(Me.ListView)
        Me.SplitContainerGridAndList.Panel2MinSize = 100
        Me.SplitContainerGridAndList.Size = New System.Drawing.Size(640, 687)
        Me.SplitContainerGridAndList.SplitterDistance = 341
        Me.SplitContainerGridAndList.TabIndex = 34
        '
        'grdChassis
        '
        Me.grdChassis.AllowUserToAddRows = False
        Me.grdChassis.AllowUserToDeleteRows = False
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Lavender
        Me.grdChassis.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle5
        Me.grdChassis.AutoGenerateColumns = False
        Me.grdChassis.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.grdChassis.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.grdChassis.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.grdChassis.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdChassis.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.grdChassis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdChassis.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameChassisDataGridViewTextBoxColumn, Me.StatusAdapterDataGridViewTextBoxColumn, Me.StatusAdapterImageDataGridViewImageColumn, Me.StatusSendImageDataGridViewImageColumn, Me.StatusReceiveImageDataGridViewImageColumn, Me.PacketsReceiveDataGridViewTextBoxColumn})
        Me.grdChassis.DataSource = Me.BindingSourceChassis
        Me.grdChassis.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdChassis.Location = New System.Drawing.Point(0, 0)
        Me.grdChassis.MultiSelect = False
        Me.grdChassis.Name = "grdChassis"
        Me.grdChassis.ReadOnly = True
        Me.grdChassis.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdChassis.Size = New System.Drawing.Size(636, 337)
        Me.grdChassis.TabIndex = 33
        '
        'NameChassisDataGridViewTextBoxColumn
        '
        Me.NameChassisDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.NameChassisDataGridViewTextBoxColumn.DataPropertyName = "NameChassis"
        Me.NameChassisDataGridViewTextBoxColumn.HeaderText = "Шасси"
        Me.NameChassisDataGridViewTextBoxColumn.Name = "NameChassisDataGridViewTextBoxColumn"
        Me.NameChassisDataGridViewTextBoxColumn.ReadOnly = True
        Me.NameChassisDataGridViewTextBoxColumn.Width = 5
        '
        'StatusAdapterDataGridViewTextBoxColumn
        '
        Me.StatusAdapterDataGridViewTextBoxColumn.DataPropertyName = "StatusAdapter"
        Me.StatusAdapterDataGridViewTextBoxColumn.HeaderText = "Статус"
        Me.StatusAdapterDataGridViewTextBoxColumn.Name = "StatusAdapterDataGridViewTextBoxColumn"
        Me.StatusAdapterDataGridViewTextBoxColumn.ReadOnly = True
        '
        'StatusAdapterImageDataGridViewImageColumn
        '
        Me.StatusAdapterImageDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.StatusAdapterImageDataGridViewImageColumn.DataPropertyName = "StatusAdapterImage"
        Me.StatusAdapterImageDataGridViewImageColumn.FillWeight = 40.0!
        Me.StatusAdapterImageDataGridViewImageColumn.HeaderText = ""
        Me.StatusAdapterImageDataGridViewImageColumn.Name = "StatusAdapterImageDataGridViewImageColumn"
        Me.StatusAdapterImageDataGridViewImageColumn.ReadOnly = True
        Me.StatusAdapterImageDataGridViewImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.StatusAdapterImageDataGridViewImageColumn.Width = 5
        '
        'StatusSendImageDataGridViewImageColumn
        '
        Me.StatusSendImageDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.StatusSendImageDataGridViewImageColumn.DataPropertyName = "StatusSendImage"
        Me.StatusSendImageDataGridViewImageColumn.FillWeight = 50.0!
        Me.StatusSendImageDataGridViewImageColumn.HeaderText = "Послать"
        Me.StatusSendImageDataGridViewImageColumn.Name = "StatusSendImageDataGridViewImageColumn"
        Me.StatusSendImageDataGridViewImageColumn.ReadOnly = True
        Me.StatusSendImageDataGridViewImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.StatusSendImageDataGridViewImageColumn.Width = 63
        '
        'StatusReceiveImageDataGridViewImageColumn
        '
        Me.StatusReceiveImageDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.StatusReceiveImageDataGridViewImageColumn.DataPropertyName = "StatusReceiveImage"
        Me.StatusReceiveImageDataGridViewImageColumn.FillWeight = 50.0!
        Me.StatusReceiveImageDataGridViewImageColumn.HeaderText = "Получить"
        Me.StatusReceiveImageDataGridViewImageColumn.Name = "StatusReceiveImageDataGridViewImageColumn"
        Me.StatusReceiveImageDataGridViewImageColumn.ReadOnly = True
        Me.StatusReceiveImageDataGridViewImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.StatusReceiveImageDataGridViewImageColumn.Width = 68
        '
        'PacketsReceiveDataGridViewTextBoxColumn
        '
        Me.PacketsReceiveDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.PacketsReceiveDataGridViewTextBoxColumn.DataPropertyName = "PacketsReceive"
        Me.PacketsReceiveDataGridViewTextBoxColumn.FillWeight = 50.0!
        Me.PacketsReceiveDataGridViewTextBoxColumn.HeaderText = "Получено пакетов"
        Me.PacketsReceiveDataGridViewTextBoxColumn.Name = "PacketsReceiveDataGridViewTextBoxColumn"
        Me.PacketsReceiveDataGridViewTextBoxColumn.ReadOnly = True
        Me.PacketsReceiveDataGridViewTextBoxColumn.Width = 126
        '
        'BindingSourceChassis
        '
        Me.BindingSourceChassis.DataSource = GetType(System.ComponentModel.BindingList(Of Registration.ChassisValue))
        '
        'PanelServerSSD
        '
        Me.PanelServerSSD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelServerSSD.Controls.Add(Me.TabLogControl)
        Me.PanelServerSSD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelServerSSD.Location = New System.Drawing.Point(0, 0)
        Me.PanelServerSSD.Name = "PanelServerSSD"
        Me.PanelServerSSD.Size = New System.Drawing.Size(616, 683)
        Me.PanelServerSSD.TabIndex = 1
        '
        'TabLogControl
        '
        Me.TabLogControl.Controls.Add(Me.TabPageServer)
        Me.TabLogControl.Controls.Add(Me.TabPageClient)
        Me.TabLogControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabLogControl.ImageList = Me.ImageListStatus
        Me.TabLogControl.Location = New System.Drawing.Point(0, 0)
        Me.TabLogControl.Name = "TabLogControl"
        Me.TabLogControl.SelectedIndex = 0
        Me.TabLogControl.Size = New System.Drawing.Size(612, 679)
        Me.TabLogControl.TabIndex = 69
        '
        'TabPageServer
        '
        Me.TabPageServer.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageServer.Controls.Add(Me.RichTextBoxServer)
        Me.TabPageServer.Controls.Add(Me.ToolStripServer)
        Me.TabPageServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TabPageServer.ImageIndex = 0
        Me.TabPageServer.Location = New System.Drawing.Point(4, 23)
        Me.TabPageServer.Name = "TabPageServer"
        Me.TabPageServer.Size = New System.Drawing.Size(604, 652)
        Me.TabPageServer.TabIndex = 0
        Me.TabPageServer.Text = "Сервер"
        Me.TabPageServer.ToolTipText = "Панель отображения Серверных функциий сопровождения шасси"
        '
        'RichTextBoxServer
        '
        Me.RichTextBoxServer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxServer.Location = New System.Drawing.Point(0, 25)
        Me.RichTextBoxServer.Name = "RichTextBoxServer"
        Me.RichTextBoxServer.ReadOnly = True
        Me.RichTextBoxServer.Size = New System.Drawing.Size(604, 627)
        Me.RichTextBoxServer.TabIndex = 1
        Me.RichTextBoxServer.Text = ""
        '
        'ToolStripServer
        '
        Me.ToolStripServer.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripServer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStripServer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.SSDPortTextBox, Me.StartStopButton, Me.ActivateTargetButton})
        Me.ToolStripServer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripServer.Name = "ToolStripServer"
        Me.ToolStripServer.Padding = New System.Windows.Forms.Padding(4, 0, 1, 0)
        Me.ToolStripServer.Size = New System.Drawing.Size(604, 25)
        Me.ToolStripServer.TabIndex = 4
        Me.ToolStripServer.Text = "ToolStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(41, 22)
        Me.ToolStripLabel1.Text = "Порт:"
        '
        'SSDPortTextBox
        '
        Me.SSDPortTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSDPortTextBox.Name = "SSDPortTextBox"
        Me.SSDPortTextBox.ReadOnly = True
        Me.SSDPortTextBox.Size = New System.Drawing.Size(64, 25)
        Me.SSDPortTextBox.Text = "5555"
        Me.SSDPortTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.SSDPortTextBox.ToolTipText = "Порт подключения ИВК"
        '
        'StartStopButton
        '
        Me.StartStopButton.CheckOnClick = True
        Me.StartStopButton.Image = Global.Registration.My.Resources.Resources.Connect
        Me.StartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.StartStopButton.Name = "StartStopButton"
        Me.StartStopButton.Size = New System.Drawing.Size(55, 22)
        Me.StartStopButton.Text = "Пуск"
        Me.StartStopButton.ToolTipText = "Ожидание подключений Шасси"
        '
        'ActivateTargetButton
        '
        Me.ActivateTargetButton.CheckOnClick = True
        Me.ActivateTargetButton.Image = Global.Registration.My.Resources.Resources.Connect
        Me.ActivateTargetButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ActivateTargetButton.Name = "ActivateTargetButton"
        Me.ActivateTargetButton.Size = New System.Drawing.Size(178, 22)
        Me.ActivateTargetButton.Text = "Запустить сбор на шасси"
        Me.ActivateTargetButton.ToolTipText = "Послать команду Запуска/Остановки Сбора на шасси"
        '
        'TabPageClient
        '
        Me.TabPageClient.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageClient.Controls.Add(Me.RichTextBoxClient)
        Me.TabPageClient.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPageClient.ImageIndex = 1
        Me.TabPageClient.Location = New System.Drawing.Point(4, 23)
        Me.TabPageClient.Name = "TabPageClient"
        Me.TabPageClient.Size = New System.Drawing.Size(604, 652)
        Me.TabPageClient.TabIndex = 3
        Me.TabPageClient.Text = "Клиент"
        Me.TabPageClient.ToolTipText = "Панель отображения Клиентских функциий связи с Сервером стенда"
        '
        'RichTextBoxClient
        '
        Me.RichTextBoxClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxClient.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBoxClient.Name = "RichTextBoxClient"
        Me.RichTextBoxClient.ReadOnly = True
        Me.RichTextBoxClient.Size = New System.Drawing.Size(604, 622)
        Me.RichTextBoxClient.TabIndex = 1
        Me.RichTextBoxClient.Text = ""
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.CmbBoxCommand, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ApplyCommandButton, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 622)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(604, 30)
        Me.TableLayoutPanel1.TabIndex = 7
        '
        'ImageListStatus
        '
        Me.ImageListStatus.ImageStream = CType(resources.GetObject("ImageListStatus.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListStatus.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListStatus.Images.SetKeyName(0, "netcenter_22.ico")
        Me.ImageListStatus.Images.SetKeyName(1, "netcenter_23.ico")
        Me.ImageListStatus.Images.SetKeyName(2, "signal-1.png")
        Me.ImageListStatus.Images.SetKeyName(3, "")
        Me.ImageListStatus.Images.SetKeyName(4, "gg_connecting.png")
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'FormCompactRio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 761)
        Me.Controls.Add(Me.ToolStripContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormCompactRio"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Мобильный Измерительно-Вычислительный Комплекс CompactRio"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ListContextMenu.ResumeLayout(False)
        Me.ToolStripContainer.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ResumeLayout(False)
        Me.ToolStripContainer.PerformLayout()
        Me.SplitContainerMain.Panel1.ResumeLayout(False)
        Me.SplitContainerMain.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerMain.ResumeLayout(False)
        Me.SplitContainerTree.Panel1.ResumeLayout(False)
        Me.SplitContainerTree.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerTree, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerTree.ResumeLayout(False)
        Me.PanelView.ResumeLayout(False)
        Me.SplitContainerGridAndList.Panel1.ResumeLayout(False)
        Me.SplitContainerGridAndList.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGridAndList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGridAndList.ResumeLayout(False)
        CType(Me.grdChassis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceChassis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelServerSSD.ResumeLayout(False)
        Me.TabLogControl.ResumeLayout(False)
        Me.TabPageServer.ResumeLayout(False)
        Me.TabPageServer.PerformLayout()
        Me.ToolStripServer.ResumeLayout(False)
        Me.ToolStripServer.PerformLayout()
        Me.TabPageClient.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelServerSSD As System.Windows.Forms.Panel
    Friend WithEvents SplitContainerMain As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripServer As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents SSDPortTextBox As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents StartStopButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ActivateTargetButton As System.Windows.Forms.ToolStripButton
    Public WithEvents grdChassis As System.Windows.Forms.DataGridView
    Public WithEvents BindingSourceChassis As System.Windows.Forms.BindingSource
    Friend WithEvents SplitContainerGridAndList As System.Windows.Forms.SplitContainer
    Friend WithEvents ImageListStatus As System.Windows.Forms.ImageList
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents TabLogControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPageServer As System.Windows.Forms.TabPage
    Friend WithEvents TabPageClient As System.Windows.Forms.TabPage
    Friend WithEvents RichTextBoxServer As System.Windows.Forms.RichTextBox
    Friend WithEvents RichTextBoxClient As System.Windows.Forms.RichTextBox
    Friend WithEvents ListContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuRebootChassis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRebootAllChassis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanelView As System.Windows.Forms.Panel
    Public WithEvents LabelGroup As System.Windows.Forms.Label
    Friend WithEvents RadioButtonViewModule As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonViewType As System.Windows.Forms.RadioButton
    Friend WithEvents ImageListTree As ImageList
    Friend WithEvents NameChassisDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents StatusAdapterDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents StatusAdapterImageDataGridViewImageColumn As DataGridViewImageColumn
    Friend WithEvents StatusSendImageDataGridViewImageColumn As DataGridViewImageColumn
    Friend WithEvents StatusReceiveImageDataGridViewImageColumn As DataGridViewImageColumn
    Friend WithEvents PacketsReceiveDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents cmdViewLog As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents CmbBoxCommand As ComboBox
    Friend WithEvents ApplyCommandButton As Button
    Friend WithEvents TSStatusLabelRecord As ToolStripStatusLabel
End Class
