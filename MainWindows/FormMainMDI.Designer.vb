<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMainMDI
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMainMDI))
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonShowConnectPanel = New System.Windows.Forms.Button()
        Me.PictureBoxConnectionServer = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MenuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuApplicationExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuNewWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowRegistration = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowSnapshot = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowTarir = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowClient = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowDBaseChannels = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowEvents = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowFunctionPanel = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEditorPanel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuShowPanel = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowModuleCalculation = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuConfigurationCalculationModule = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSetComPort = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuWindowModuleCalculationKT = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuConfigurationCalculationModuleKT = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowCascade = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowTileHorizontal = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowTileVertical = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.TimerAwait = New System.Windows.Forms.Timer(Me.components)
        Me.tspTop = New System.Windows.Forms.ToolStripPanel()
        Me.tspRight = New System.Windows.Forms.ToolStripPanel()
        Me.tspLeft = New System.Windows.Forms.ToolStripPanel()
        Me.ToolStripPanel1 = New System.Windows.Forms.ToolStripPanel()
        Me.ToolStripPanel2 = New System.Windows.Forms.ToolStripPanel()
        Me.ToolStripPanel3 = New System.Windows.Forms.ToolStripPanel()
        Me.tspBottom = New System.Windows.Forms.ToolStripPanel()
        Me.RichTextBoxClient = New System.Windows.Forms.RichTextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.StatusStripClient = New System.Windows.Forms.StatusStrip()
        Me.LabelStatusClient_adapter = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelStatusClient_receive = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelНепр = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.TextBoxServerIP = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.TextBoxServerPort = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.TextBoxConnectOk = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxConnectBad = New System.Windows.Forms.ToolStripTextBox()
        Me.ButtonConnectRefresh = New System.Windows.Forms.ToolStripButton()
        Me.SplitterConnectionPanel = New System.Windows.Forms.Splitter()
        Me.TableLayoutPanelConnection = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.PictureBoxConnectionServer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStripPanel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.StatusStripClient.SuspendLayout()
        Me.ToolStrip.SuspendLayout()
        Me.TableLayoutPanelConnection.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonShowConnectPanel
        '
        Me.ButtonShowConnectPanel.Image = Global.Registration.My.Resources.Resources.forward
        Me.ButtonShowConnectPanel.Location = New System.Drawing.Point(3, 3)
        Me.ButtonShowConnectPanel.Name = "ButtonShowConnectPanel"
        Me.ButtonShowConnectPanel.Size = New System.Drawing.Size(25, 25)
        Me.ButtonShowConnectPanel.TabIndex = 0
        Me.ButtonShowConnectPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip.SetToolTip(Me.ButtonShowConnectPanel, "Скрыть дополнительную панель >>")
        Me.ButtonShowConnectPanel.UseVisualStyleBackColor = True
        '
        'PictureBoxConnectionServer
        '
        Me.PictureBoxConnectionServer.BackgroundImage = Global.Registration.My.Resources.Resources.ledCornerGray
        Me.PictureBoxConnectionServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBoxConnectionServer.Dock = System.Windows.Forms.DockStyle.Top
        Me.PictureBoxConnectionServer.Location = New System.Drawing.Point(3, 34)
        Me.PictureBoxConnectionServer.Name = "PictureBoxConnectionServer"
        Me.PictureBoxConnectionServer.Size = New System.Drawing.Size(26, 21)
        Me.PictureBoxConnectionServer.TabIndex = 46
        Me.PictureBoxConnectionServer.TabStop = False
        Me.ToolTip.SetToolTip(Me.PictureBoxConnectionServer, "Соединение с Сервером")
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuFile, Me.TSMenuNewWindow, Me.MenuWindow})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.MdiWindowListItem = Me.MenuWindow
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(866, 24)
        Me.MenuStrip1.TabIndex = 153
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuFile
        '
        Me.MenuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuApplicationExit})
        Me.MenuFile.Name = "MenuFile"
        Me.MenuFile.Size = New System.Drawing.Size(66, 20)
        Me.MenuFile.Text = "&Система"
        '
        'MenuApplicationExit
        '
        Me.MenuApplicationExit.Image = CType(resources.GetObject("MenuApplicationExit.Image"), System.Drawing.Image)
        Me.MenuApplicationExit.Name = "MenuApplicationExit"
        Me.MenuApplicationExit.Size = New System.Drawing.Size(187, 22)
        Me.MenuApplicationExit.Text = "&Завершение работы"
        Me.MenuApplicationExit.ToolTipText = "Завершение работы и закрытие программы"
        '
        'TSMenuNewWindow
        '
        Me.TSMenuNewWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuNewWindowRegistration, Me.MenuNewWindowSnapshot, Me.MenuNewWindowTarir, Me.MenuNewWindowClient, Me.MenuNewWindowDBaseChannels, Me.MenuNewWindowEvents, Me.MenuNewWindowFunctionPanel, Me.MenuWindowModuleCalculation, Me.MenuWindowModuleCalculationKT})
        Me.TSMenuNewWindow.Name = "TSMenuNewWindow"
        Me.TSMenuNewWindow.Size = New System.Drawing.Size(84, 20)
        Me.TSMenuNewWindow.Text = "&Новое окно"
        '
        'MenuNewWindowRegistration
        '
        Me.MenuNewWindowRegistration.Image = CType(resources.GetObject("MenuNewWindowRegistration.Image"), System.Drawing.Image)
        Me.MenuNewWindowRegistration.Name = "MenuNewWindowRegistration"
        Me.MenuNewWindowRegistration.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowRegistration.Text = "&Регистратор"
        Me.MenuNewWindowRegistration.ToolTipText = "Окно регистрации измерений"
        '
        'MenuNewWindowSnapshot
        '
        Me.MenuNewWindowSnapshot.Image = CType(resources.GetObject("MenuNewWindowSnapshot.Image"), System.Drawing.Image)
        Me.MenuNewWindowSnapshot.Name = "MenuNewWindowSnapshot"
        Me.MenuNewWindowSnapshot.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowSnapshot.Text = "&Снимок"
        Me.MenuNewWindowSnapshot.ToolTipText = "Окно просмотра записей"
        '
        'MenuNewWindowTarir
        '
        Me.MenuNewWindowTarir.Image = CType(resources.GetObject("MenuNewWindowTarir.Image"), System.Drawing.Image)
        Me.MenuNewWindowTarir.Name = "MenuNewWindowTarir"
        Me.MenuNewWindowTarir.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowTarir.Text = "&Тарировка"
        Me.MenuNewWindowTarir.ToolTipText = "Окно тарировки каналов"
        '
        'MenuNewWindowClient
        '
        Me.MenuNewWindowClient.Image = CType(resources.GetObject("MenuNewWindowClient.Image"), System.Drawing.Image)
        Me.MenuNewWindowClient.Name = "MenuNewWindowClient"
        Me.MenuNewWindowClient.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowClient.Text = "&Клиент"
        Me.MenuNewWindowClient.ToolTipText = "Окно наблюдения измерений по сети"
        '
        'MenuNewWindowDBaseChannels
        '
        Me.MenuNewWindowDBaseChannels.Image = CType(resources.GetObject("MenuNewWindowDBaseChannels.Image"), System.Drawing.Image)
        Me.MenuNewWindowDBaseChannels.Name = "MenuNewWindowDBaseChannels"
        Me.MenuNewWindowDBaseChannels.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowDBaseChannels.Text = "&База каналов"
        Me.MenuNewWindowDBaseChannels.ToolTipText = "Редактирование базы каналов из программы"
        '
        'MenuNewWindowEvents
        '
        Me.MenuNewWindowEvents.Image = CType(resources.GetObject("MenuNewWindowEvents.Image"), System.Drawing.Image)
        Me.MenuNewWindowEvents.Name = "MenuNewWindowEvents"
        Me.MenuNewWindowEvents.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowEvents.Text = "&Управление по событиям"
        Me.MenuNewWindowEvents.ToolTipText = "Управление внешними исполнительными устройствами"
        '
        'MenuNewWindowFunctionPanel
        '
        Me.MenuNewWindowFunctionPanel.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuEditorPanel, Me.ToolStripSeparator1, Me.MenuShowPanel})
        Me.MenuNewWindowFunctionPanel.Image = CType(resources.GetObject("MenuNewWindowFunctionPanel.Image"), System.Drawing.Image)
        Me.MenuNewWindowFunctionPanel.Name = "MenuNewWindowFunctionPanel"
        Me.MenuNewWindowFunctionPanel.Size = New System.Drawing.Size(216, 22)
        Me.MenuNewWindowFunctionPanel.Text = "Графические &панели"
        Me.MenuNewWindowFunctionPanel.ToolTipText = "Графические мнемосхемы испытываемого оборудования"
        '
        'MenuEditorPanel
        '
        Me.MenuEditorPanel.Image = CType(resources.GetObject("MenuEditorPanel.Image"), System.Drawing.Image)
        Me.MenuEditorPanel.Name = "MenuEditorPanel"
        Me.MenuEditorPanel.Size = New System.Drawing.Size(180, 22)
        Me.MenuEditorPanel.Text = "Р&едактор Панелей"
        Me.MenuEditorPanel.ToolTipText = "Окно редактора графических панелей"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'MenuShowPanel
        '
        Me.MenuShowPanel.Enabled = False
        Me.MenuShowPanel.Image = CType(resources.GetObject("MenuShowPanel.Image"), System.Drawing.Image)
        Me.MenuShowPanel.Name = "MenuShowPanel"
        Me.MenuShowPanel.Size = New System.Drawing.Size(180, 22)
        Me.MenuShowPanel.Text = "Пок&азать Панели"
        Me.MenuShowPanel.ToolTipText = "Окно вкладок для выбора панелей"
        '
        'MenuWindowModuleCalculation
        '
        Me.MenuWindowModuleCalculation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuConfigurationCalculationModule, Me.MenuSetComPort, Me.ToolStripSeparator2})
        Me.MenuWindowModuleCalculation.Enabled = False
        Me.MenuWindowModuleCalculation.Image = CType(resources.GetObject("MenuWindowModuleCalculation.Image"), System.Drawing.Image)
        Me.MenuWindowModuleCalculation.Name = "MenuWindowModuleCalculation"
        Me.MenuWindowModuleCalculation.Size = New System.Drawing.Size(216, 22)
        Me.MenuWindowModuleCalculation.Text = "М&одули расчёта"
        Me.MenuWindowModuleCalculation.ToolTipText = "Управление подключенными модулями расчета"
        '
        'MenuConfigurationCalculationModule
        '
        Me.MenuConfigurationCalculationModule.Enabled = False
        Me.MenuConfigurationCalculationModule.Image = CType(resources.GetObject("MenuConfigurationCalculationModule.Image"), System.Drawing.Image)
        Me.MenuConfigurationCalculationModule.Name = "MenuConfigurationCalculationModule"
        Me.MenuConfigurationCalculationModule.Size = New System.Drawing.Size(251, 22)
        Me.MenuConfigurationCalculationModule.Text = "&Конфигуратор модулей расчёта"
        Me.MenuConfigurationCalculationModule.ToolTipText = "Управление списком подключения модулей расчета"
        '
        'MenuSetComPort
        '
        Me.MenuSetComPort.Enabled = False
        Me.MenuSetComPort.Image = CType(resources.GetObject("MenuSetComPort.Image"), System.Drawing.Image)
        Me.MenuSetComPort.Name = "MenuSetComPort"
        Me.MenuSetComPort.Size = New System.Drawing.Size(251, 22)
        Me.MenuSetComPort.Text = "&Настройка барометров БРС-1М"
        Me.MenuSetComPort.ToolTipText = "Конфигурирование ComPort для подключённых барометров БРС-1М"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(248, 6)
        '
        'MenuWindowModuleCalculationKT
        '
        Me.MenuWindowModuleCalculationKT.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuConfigurationCalculationModuleKT, Me.ToolStripSeparator7})
        Me.MenuWindowModuleCalculationKT.Enabled = False
        Me.MenuWindowModuleCalculationKT.Image = CType(resources.GetObject("MenuWindowModuleCalculationKT.Image"), System.Drawing.Image)
        Me.MenuWindowModuleCalculationKT.Name = "MenuWindowModuleCalculationKT"
        Me.MenuWindowModuleCalculationKT.Size = New System.Drawing.Size(216, 22)
        Me.MenuWindowModuleCalculationKT.Text = "Мо&дули сбора КТ"
        Me.MenuWindowModuleCalculationKT.ToolTipText = "Управление подключенными модулями сбора КТ"
        '
        'MenuConfigurationCalculationModuleKT
        '
        Me.MenuConfigurationCalculationModuleKT.Enabled = False
        Me.MenuConfigurationCalculationModuleKT.Image = CType(resources.GetObject("MenuConfigurationCalculationModuleKT.Image"), System.Drawing.Image)
        Me.MenuConfigurationCalculationModuleKT.Name = "MenuConfigurationCalculationModuleKT"
        Me.MenuConfigurationCalculationModuleKT.Size = New System.Drawing.Size(257, 22)
        Me.MenuConfigurationCalculationModuleKT.Text = "&Конфигуратор модулей сбора КТ"
        Me.MenuConfigurationCalculationModuleKT.ToolTipText = "Управление списком подключения модулей сбора КТ"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(254, 6)
        '
        'MenuWindow
        '
        Me.MenuWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuWindowCascade, Me.MenuWindowTileHorizontal, Me.MenuWindowTileVertical})
        Me.MenuWindow.Name = "MenuWindow"
        Me.MenuWindow.Size = New System.Drawing.Size(48, 20)
        Me.MenuWindow.Text = "&Окно"
        '
        'MenuWindowCascade
        '
        Me.MenuWindowCascade.Image = CType(resources.GetObject("MenuWindowCascade.Image"), System.Drawing.Image)
        Me.MenuWindowCascade.Name = "MenuWindowCascade"
        Me.MenuWindowCascade.Size = New System.Drawing.Size(180, 22)
        Me.MenuWindowCascade.Text = "&Каскад"
        Me.MenuWindowCascade.ToolTipText = "Расположение окон каскадом"
        '
        'MenuWindowTileHorizontal
        '
        Me.MenuWindowTileHorizontal.Image = CType(resources.GetObject("MenuWindowTileHorizontal.Image"), System.Drawing.Image)
        Me.MenuWindowTileHorizontal.Name = "MenuWindowTileHorizontal"
        Me.MenuWindowTileHorizontal.Size = New System.Drawing.Size(180, 22)
        Me.MenuWindowTileHorizontal.Text = "&Горизонтально"
        Me.MenuWindowTileHorizontal.ToolTipText = "Расположение окон сверху"
        '
        'MenuWindowTileVertical
        '
        Me.MenuWindowTileVertical.Image = CType(resources.GetObject("MenuWindowTileVertical.Image"), System.Drawing.Image)
        Me.MenuWindowTileVertical.Name = "MenuWindowTileVertical"
        Me.MenuWindowTileVertical.Size = New System.Drawing.Size(180, 22)
        Me.MenuWindowTileVertical.Text = "&Вертикально"
        Me.MenuWindowTileVertical.ToolTipText = "Расположение окон рядом"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "doc1"
        '
        'TimerAwait
        '
        Me.TimerAwait.Interval = 1000
        '
        'tspTop
        '
        Me.tspTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.tspTop.Location = New System.Drawing.Point(0, 0)
        Me.tspTop.Name = "tspTop"
        Me.tspTop.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.tspTop.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tspTop.Size = New System.Drawing.Size(866, 0)
        '
        'tspRight
        '
        Me.tspRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.tspRight.Location = New System.Drawing.Point(866, 0)
        Me.tspRight.Name = "tspRight"
        Me.tspRight.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.tspRight.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.tspRight.Size = New System.Drawing.Size(0, 358)
        '
        'tspLeft
        '
        Me.tspLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.tspLeft.Location = New System.Drawing.Point(0, 0)
        Me.tspLeft.Name = "tspLeft"
        Me.tspLeft.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.tspLeft.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.tspLeft.Size = New System.Drawing.Size(0, 358)
        '
        'ToolStripPanel1
        '
        Me.ToolStripPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.ToolStripPanel1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStripPanel1.Name = "ToolStripPanel1"
        Me.ToolStripPanel1.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.ToolStripPanel1.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.ToolStripPanel1.Size = New System.Drawing.Size(0, 334)
        '
        'ToolStripPanel2
        '
        Me.ToolStripPanel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.ToolStripPanel2.Location = New System.Drawing.Point(866, 24)
        Me.ToolStripPanel2.Name = "ToolStripPanel2"
        Me.ToolStripPanel2.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.ToolStripPanel2.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.ToolStripPanel2.Size = New System.Drawing.Size(0, 334)
        '
        'ToolStripPanel3
        '
        Me.ToolStripPanel3.Controls.Add(Me.MenuStrip1)
        Me.ToolStripPanel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.ToolStripPanel3.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripPanel3.Name = "ToolStripPanel3"
        Me.ToolStripPanel3.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.ToolStripPanel3.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.ToolStripPanel3.Size = New System.Drawing.Size(866, 24)
        '
        'tspBottom
        '
        Me.tspBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tspBottom.Location = New System.Drawing.Point(0, 358)
        Me.tspBottom.Name = "tspBottom"
        Me.tspBottom.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.tspBottom.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.tspBottom.Size = New System.Drawing.Size(866, 0)
        '
        'RichTextBoxClient
        '
        Me.RichTextBoxClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxClient.Location = New System.Drawing.Point(0, 23)
        Me.RichTextBoxClient.Name = "RichTextBoxClient"
        Me.RichTextBoxClient.ReadOnly = True
        Me.RichTextBoxClient.Size = New System.Drawing.Size(245, 251)
        Me.RichTextBoxClient.TabIndex = 9
        Me.RichTextBoxClient.Text = ""
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.RichTextBoxClient)
        Me.Panel1.Controls.Add(Me.StatusStripClient)
        Me.Panel1.Controls.Add(Me.ToolStrip)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(41, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(249, 328)
        Me.Panel1.TabIndex = 18
        '
        'StatusStripClient
        '
        Me.StatusStripClient.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LabelStatusClient_adapter, Me.LabelStatusClient_receive, Me.LabelНепр})
        Me.StatusStripClient.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.StatusStripClient.Location = New System.Drawing.Point(0, 274)
        Me.StatusStripClient.MinimumSize = New System.Drawing.Size(361, 22)
        Me.StatusStripClient.Name = "StatusStripClient"
        Me.StatusStripClient.Size = New System.Drawing.Size(361, 50)
        Me.StatusStripClient.TabIndex = 15
        Me.StatusStripClient.Text = "StatusStripClient"
        '
        'LabelStatusClient_adapter
        '
        Me.LabelStatusClient_adapter.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelStatusClient_adapter.BorderStyle = System.Windows.Forms.Border3DStyle.Raised
        Me.LabelStatusClient_adapter.Image = Global.Registration.My.Resources.Resources.ledCornerGray
        Me.LabelStatusClient_adapter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LabelStatusClient_adapter.Name = "LabelStatusClient_adapter"
        Me.LabelStatusClient_adapter.Size = New System.Drawing.Size(100, 20)
        Me.LabelStatusClient_adapter.Text = "adapter name"
        Me.LabelStatusClient_adapter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LabelStatusClient_adapter.ToolTipText = "Статус подключения ССД к Серверу"
        '
        'LabelStatusClient_receive
        '
        Me.LabelStatusClient_receive.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelStatusClient_receive.BorderStyle = System.Windows.Forms.Border3DStyle.Raised
        Me.LabelStatusClient_receive.Image = Global.Registration.My.Resources.Resources.ledCornerGray
        Me.LabelStatusClient_receive.Name = "LabelStatusClient_receive"
        Me.LabelStatusClient_receive.Size = New System.Drawing.Size(64, 20)
        Me.LabelStatusClient_receive.Text = "receive"
        Me.LabelStatusClient_receive.ToolTipText = "Статус приёма данных от Сервера"
        '
        'LabelНепр
        '
        Me.LabelНепр.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelНепр.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.LabelНепр.Image = Global.Registration.My.Resources.Resources.Disconnect
        Me.LabelНепр.Name = "LabelНепр"
        Me.LabelНепр.Size = New System.Drawing.Size(243, 20)
        Me.LabelНепр.Text = "Приём каналов от Сервера отсутствует"
        Me.LabelНепр.ToolTipText = "Индикатор приёма каналов от Сервера"
        '
        'ToolStrip
        '
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel3, Me.TextBoxServerIP, Me.ToolStripLabel4, Me.TextBoxServerPort, Me.ToolStripSeparator8, Me.TextBoxConnectOk, Me.TextBoxConnectBad, Me.ButtonConnectRefresh})
        Me.ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(245, 23)
        Me.ToolStrip.TabIndex = 10
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(50, 15)
        Me.ToolStripLabel3.Text = "Сервер:"
        '
        'TextBoxServerIP
        '
        Me.TextBoxServerIP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxServerIP.Name = "TextBoxServerIP"
        Me.TextBoxServerIP.ReadOnly = True
        Me.TextBoxServerIP.Size = New System.Drawing.Size(75, 23)
        Me.TextBoxServerIP.Text = "127.0.0.1"
        Me.TextBoxServerIP.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBoxServerIP.ToolTipText = "Сетевой адрес Сервера"
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(38, 15)
        Me.ToolStripLabel4.Text = "Порт:"
        '
        'TextBoxServerPort
        '
        Me.TextBoxServerPort.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxServerPort.Name = "TextBoxServerPort"
        Me.TextBoxServerPort.ReadOnly = True
        Me.TextBoxServerPort.Size = New System.Drawing.Size(57, 23)
        Me.TextBoxServerPort.Text = "1701"
        Me.TextBoxServerPort.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBoxServerPort.ToolTipText = "Порт подключения Сервера"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 23)
        '
        'TextBoxConnectOk
        '
        Me.TextBoxConnectOk.BackColor = System.Drawing.Color.Lime
        Me.TextBoxConnectOk.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxConnectOk.Name = "TextBoxConnectOk"
        Me.TextBoxConnectOk.ReadOnly = True
        Me.TextBoxConnectOk.Size = New System.Drawing.Size(175, 23)
        Me.TextBoxConnectOk.Text = "Связь с сервером установлена"
        Me.TextBoxConnectOk.ToolTipText = "Индикатор подключения к Серверу"
        Me.TextBoxConnectOk.Visible = False
        '
        'TextBoxConnectBad
        '
        Me.TextBoxConnectBad.BackColor = System.Drawing.Color.Red
        Me.TextBoxConnectBad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxConnectBad.ForeColor = System.Drawing.Color.White
        Me.TextBoxConnectBad.Name = "TextBoxConnectBad"
        Me.TextBoxConnectBad.ReadOnly = True
        Me.TextBoxConnectBad.Size = New System.Drawing.Size(175, 23)
        Me.TextBoxConnectBad.Text = "Связь с сервером отсутствует"
        Me.TextBoxConnectBad.ToolTipText = "Индикатор отсутствия связи с Сервером"
        Me.TextBoxConnectBad.Visible = False
        '
        'ButtonConnectRefresh
        '
        Me.ButtonConnectRefresh.Image = CType(resources.GetObject("ButtonConnectRefresh.Image"), System.Drawing.Image)
        Me.ButtonConnectRefresh.ImageTransparentColor = System.Drawing.Color.Black
        Me.ButtonConnectRefresh.Name = "ButtonConnectRefresh"
        Me.ButtonConnectRefresh.Size = New System.Drawing.Size(109, 20)
        Me.ButtonConnectRefresh.Text = "Подключиться"
        Me.ButtonConnectRefresh.ToolTipText = "Обновить подключение"
        Me.ButtonConnectRefresh.Visible = False
        '
        'SplitterConnectionPanel
        '
        Me.SplitterConnectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitterConnectionPanel.Dock = System.Windows.Forms.DockStyle.Right
        Me.SplitterConnectionPanel.Location = New System.Drawing.Point(569, 24)
        Me.SplitterConnectionPanel.Name = "SplitterConnectionPanel"
        Me.SplitterConnectionPanel.Size = New System.Drawing.Size(4, 334)
        Me.SplitterConnectionPanel.TabIndex = 27
        Me.SplitterConnectionPanel.TabStop = False
        '
        'TableLayoutPanelConnection
        '
        Me.TableLayoutPanelConnection.ColumnCount = 2
        Me.TableLayoutPanelConnection.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.TableLayoutPanelConnection.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelConnection.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanelConnection.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanelConnection.Dock = System.Windows.Forms.DockStyle.Right
        Me.TableLayoutPanelConnection.Location = New System.Drawing.Point(573, 24)
        Me.TableLayoutPanelConnection.Name = "TableLayoutPanelConnection"
        Me.TableLayoutPanelConnection.RowCount = 1
        Me.TableLayoutPanelConnection.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelConnection.Size = New System.Drawing.Size(293, 334)
        Me.TableLayoutPanelConnection.TabIndex = 36
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PictureBoxConnectionServer, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonShowConnectPanel, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(32, 328)
        Me.TableLayoutPanel1.TabIndex = 45
        '
        'FormMainMDI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(866, 358)
        Me.Controls.Add(Me.SplitterConnectionPanel)
        Me.Controls.Add(Me.TableLayoutPanelConnection)
        Me.Controls.Add(Me.ToolStripPanel1)
        Me.Controls.Add(Me.ToolStripPanel2)
        Me.Controls.Add(Me.ToolStripPanel3)
        Me.Controls.Add(Me.tspBottom)
        Me.Controls.Add(Me.tspLeft)
        Me.Controls.Add(Me.tspRight)
        Me.Controls.Add(Me.tspTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormMainMDI"
        Me.Text = " Автоматизированная система регистрации"
        CType(Me.PictureBoxConnectionServer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStripPanel3.ResumeLayout(False)
        Me.ToolStripPanel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.StatusStripClient.ResumeLayout(False)
        Me.StatusStripClient.PerformLayout()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.TableLayoutPanelConnection.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuApplicationExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuNewWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowCascade As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowTileHorizontal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowTileVertical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Public WithEvents TimerAwait As System.Windows.Forms.Timer
    Friend WithEvents tspTop As System.Windows.Forms.ToolStripPanel
    Friend WithEvents tspRight As System.Windows.Forms.ToolStripPanel
    Friend WithEvents tspLeft As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ToolStripPanel1 As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ToolStripPanel2 As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ToolStripPanel3 As System.Windows.Forms.ToolStripPanel
    Friend WithEvents tspBottom As System.Windows.Forms.ToolStripPanel
    Public WithEvents MenuNewWindowRegistration As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowSnapshot As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowTarir As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowClient As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowDBaseChannels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuNewWindowEvents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuNewWindowFunctionPanel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuEditorPanel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuWindowModuleCalculation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuConfigurationCalculationModule As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuWindowModuleCalculationKT As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuConfigurationCalculationModuleKT As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuShowPanel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RichTextBoxClient As RichTextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ButtonConnectRefresh As ToolStripButton
    Friend WithEvents ToolStripLabel3 As ToolStripLabel
    Friend WithEvents TextBoxServerIP As ToolStripTextBox
    Friend WithEvents ToolStripLabel4 As ToolStripLabel
    Friend WithEvents TextBoxServerPort As ToolStripTextBox
    Friend WithEvents StatusStripClient As StatusStrip
    Friend WithEvents LabelStatusClient_adapter As ToolStripStatusLabel
    Friend WithEvents LabelStatusClient_receive As ToolStripStatusLabel
    Friend WithEvents TextBoxConnectOk As ToolStripTextBox
    Friend WithEvents TextBoxConnectBad As ToolStripTextBox
    Friend WithEvents SplitterConnectionPanel As Splitter
    Friend WithEvents LabelНепр As ToolStripStatusLabel
    Friend WithEvents TableLayoutPanelConnection As TableLayoutPanel
    Friend WithEvents ButtonShowConnectPanel As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PictureBoxConnectionServer As PictureBox
    Friend WithEvents MenuSetComPort As ToolStripMenuItem
End Class
