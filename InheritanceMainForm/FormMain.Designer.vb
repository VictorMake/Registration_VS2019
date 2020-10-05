<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
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
        Dim ScaleCustomDivision1 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Масштабирование", "", "Панорамирование"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte)))
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+ВыделитьЛевойКнопкойМыши", "Масштаб по выделенному", "Ctrl+ТянутьЛевойКнопкойМыши", "Панорамирование"}, -1)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+Alt+ВыделитьЛевойКнопкойМыши", "Масштаб пропорционально выделенному", "Ctrl+ЛеваяСтрелка", "Панорама влево"}, -1)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+ЛевыйЩелчок", "Масштаб вокруг точки", "Ctrl+ПраваяСтрелка", "Панорама вправо"}, -1)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+ВерхняяСтрелка", "+ Масштаб изображения в середине области", "Ctrl+ВерхняяСтрелка", "Панорама вверх"}, -1)
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+НижняяСтрелка", "-  Масштаб изображения в середине области", "Ctrl+НижняяСтрелка", "Панорама вниз"}, -1)
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+КолесоМыши", "Масштаб +/Масштаб -", "Ctrl+ПравыйЩелчок", "Откат"}, -1)
        Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+ПравыйЩелчок", "Откат", "Ctrl+Забой", "Сброс"}, -1)
        Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Shift+Забой", "Сброс"}, -1)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainerForm = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SlidePlot = New NationalInstruments.UI.WindowsForms.Slide()
        Me.PanelSlide = New System.Windows.Forms.Panel()
        Me.SlideAxis = New NationalInstruments.UI.WindowsForms.Slide()
        Me.PanelSlideBottom = New System.Windows.Forms.Panel()
        Me.SplitContainerGraph = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerGraph1 = New System.Windows.Forms.SplitContainer()
        Me.TextBoxCaption = New System.Windows.Forms.TextBox()
        Me.WaveformGraphTime = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.XyCursorStart = New NationalInstruments.UI.XYCursor()
        Me.WaveformPlot1 = New NationalInstruments.UI.WaveformPlot()
        Me.XAxisTime = New NationalInstruments.UI.XAxis()
        Me.YAxisTime = New NationalInstruments.UI.YAxis()
        Me.XyCursorEnd = New NationalInstruments.UI.XYCursor()
        Me.XyCursorTime = New NationalInstruments.UI.XYCursor()
        Me.XyCursorParametr = New NationalInstruments.UI.XYCursor()
        Me.ListViewAcquisition = New Registration.DbListView(Me.components)
        Me.PanelMode = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelMode = New System.Windows.Forms.TableLayoutPanel()
        Me.ComboBoxDisplayRate = New System.Windows.Forms.ComboBox()
        Me.NumericUpDownPrecisionScreen = New System.Windows.Forms.NumericUpDown()
        Me.ComboBoxSelectiveList = New System.Windows.Forms.ComboBox()
        Me.SplitContainerGraph2 = New System.Windows.Forms.SplitContainer()
        Me.ScatterGraphParameter = New NationalInstruments.UI.WindowsForms.ScatterGraph()
        Me.XyCursor1 = New NationalInstruments.UI.XYCursor()
        Me.ScatterPlot2 = New NationalInstruments.UI.ScatterPlot()
        Me.XAxis1 = New NationalInstruments.UI.XAxis()
        Me.YAxis1 = New NationalInstruments.UI.YAxis()
        Me.SlideTime = New NationalInstruments.UI.WindowsForms.Slide()
        Me.ListViewParametr = New Registration.DbListView(Me.components)
        Me.TableLayoutPanelУправленияДляСнимка = New System.Windows.Forms.TableLayoutPanel()
        Me.InstrumentControlStrip1 = New NationalInstruments.UI.WindowsForms.InstrumentControlStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor1 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor2 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor3 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor4 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor5 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.ToolStripLabel6 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripPropertyEditor6 = New NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor()
        Me.TableLayoutPanelFrameCursor = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBoxAxis = New System.Windows.Forms.GroupBox()
        Me.GroupBoxDelete = New System.Windows.Forms.GroupBox()
        Me.ComboBoxDescriptionPointer = New System.Windows.Forms.ComboBox()
        Me.ButtonDelete = New System.Windows.Forms.Button()
        Me.TextBoxCount = New System.Windows.Forms.TextBox()
        Me.TextBoxDescriptionOnAxes = New System.Windows.Forms.TextBox()
        Me.ButtonShowAxes = New System.Windows.Forms.Button()
        Me.ComboBoxPointers = New System.Windows.Forms.ComboBox()
        Me.ButtonFixLine = New System.Windows.Forms.Button()
        Me.PanelKey = New System.Windows.Forms.Panel()
        Me.ListViewAction = New System.Windows.Forms.ListView()
        Me.ColumnHeaderKeys1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderAction1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderKeys2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderAction2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolStripMain2 = New System.Windows.Forms.ToolStrip()
        Me.ButtonOneCursor = New System.Windows.Forms.ToolStripButton()
        Me.ButtonTwoCursor = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonXRange = New System.Windows.Forms.ToolStripButton()
        Me.ButtonYRange = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.EnableLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ButtonDragCursor = New System.Windows.Forms.ToolStripButton()
        Me.ButtonPanX = New System.Windows.Forms.ToolStripButton()
        Me.ButtonPanY = New System.Windows.Forms.ToolStripButton()
        Me.ButtonZoomPoint = New System.Windows.Forms.ToolStripButton()
        Me.ButtonZoomX = New System.Windows.Forms.ToolStripButton()
        Me.ButtonZoomY = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonMore = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonAddTiks = New System.Windows.Forms.ToolStripButton()
        Me.ButtonDeleteTiks = New System.Windows.Forms.ToolStripButton()
        Me.GroupeBoxCursorStart = New System.Windows.Forms.GroupBox()
        Me.ButtonСursorStartMoveForward = New System.Windows.Forms.Button()
        Me.ButtonСursorStartMoveBack = New System.Windows.Forms.Button()
        Me.XPosition = New System.Windows.Forms.TextBox()
        Me.YPosition = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupeBoxCursorEnd = New System.Windows.Forms.GroupBox()
        Me.PeriodVal = New System.Windows.Forms.TextBox()
        Me.AmplitudeVal = New System.Windows.Forms.TextBox()
        Me.ButtonСursorEndMoveForward = New System.Windows.Forms.Button()
        Me.ButtonСursorEndMoveBack = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MenuStripShap = New System.Windows.Forms.MenuStrip()
        Me.MenuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpenBaseSnapshot = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFindSnapshot = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRecordGraph = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPrintGraph = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExportSnapshotInExcel = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuChangingBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuModeOfOperation = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRegistration = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDebugging = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuDecoding = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuProtocol = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuComeBackToBeginning = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuWriteDecodingSnapshotToDBase = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWriteDecodingSnapshotToExcel = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPrintDecodingSnapshot = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuXRange = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuYRange = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuInteractionModes = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDragCursor = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPanX = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPanY = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuZoomPoint = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuZoomX = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuZoomY = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExecute = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuGraphCheckedList = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEditGraphOfParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripBar = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuShowTextControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuShowGraphControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuServerEnable = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuReceiveFromServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSimulator = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCommandClientServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuGraphAlongTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuGraphAlongParameters = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuParameterInRange = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPens = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuQuestioningParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTune = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuConfigurationChannels = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuVisibilityTrend = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuSelectiveList = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSelectiveTextControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSelectiveGraphControl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuShowSetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuNewWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowRegistration = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowSnapshot = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowTarir = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowClient = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowDBaseChannels = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewWindowEvents = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowCascade = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowTileHorizontal = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWindowTileVertical = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuContents = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuIndex = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExplorerHelpApplication = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpRegime = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNormTU = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuAboutProgramm = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMain = New System.Windows.Forms.ToolStrip()
        Me.ButtonContinuously = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonRecord = New System.Windows.Forms.ToolStripButton()
        Me.TextBoxRecordToDisc = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonGraphTime = New System.Windows.Forms.ToolStripButton()
        Me.ButtonGraphParameter = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonSnapshot = New System.Windows.Forms.ToolStripButton()
        Me.LabelSec = New System.Windows.Forms.ToolStripLabel()
        Me.ComboBoxTimeMeasurement = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonDetailSelective = New System.Windows.Forms.ToolStripButton()
        Me.ButtonTuneTrand = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ComboBoxSelectAxis = New System.Windows.Forms.ToolStripComboBox()
        Me.LabelIndicator = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.TextBoxConnectOk = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxConnectBad = New System.Windows.Forms.ToolStripTextBox()
        Me.ButtonConnectRefresh = New System.Windows.Forms.ToolStripButton()
        Me.LabelMessageTcpClient = New System.Windows.Forms.ToolStripLabel()
        Me.TextBoxIndicatorN1physics = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxIndicatorN1reduce = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxIndicatorN2physics = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxIndicatorN2reduce = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxIndicatorRud = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxRegime = New System.Windows.Forms.ToolStripLabel()
        Me.StatusStripMain = New System.Windows.Forms.StatusStrip()
        Me.LabelRegistration = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelProduct = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelDisc = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelDiscValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelSampleRate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBarExport = New System.Windows.Forms.ToolStripProgressBar()
        Me.TimerButtonRun = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSwitchOffRecord = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCursor = New System.Windows.Forms.Timer(Me.components)
        Me.TimerTimeOutClient = New System.Windows.Forms.Timer(Me.components)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.HelpProviderAdvancedCHM = New System.Windows.Forms.HelpProvider()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PulseButtonTakeOffAlarm = New PulseButton.PulseButton()
        Me.ContextMenuCursor = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuAnnotation = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuСhangeText = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerRealize3D = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        CType(Me.SplitContainerForm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerForm.Panel1.SuspendLayout()
        Me.SplitContainerForm.Panel2.SuspendLayout()
        Me.SplitContainerForm.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SlidePlot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSlide.SuspendLayout()
        CType(Me.SlideAxis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainerGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGraph.Panel1.SuspendLayout()
        Me.SplitContainerGraph.Panel2.SuspendLayout()
        Me.SplitContainerGraph.SuspendLayout()
        CType(Me.SplitContainerGraph1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGraph1.Panel1.SuspendLayout()
        Me.SplitContainerGraph1.Panel2.SuspendLayout()
        Me.SplitContainerGraph1.SuspendLayout()
        CType(Me.WaveformGraphTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursorStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursorEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursorTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursorParametr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelMode.SuspendLayout()
        Me.TableLayoutPanelMode.SuspendLayout()
        CType(Me.NumericUpDownPrecisionScreen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainerGraph2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerGraph2.Panel1.SuspendLayout()
        Me.SplitContainerGraph2.Panel2.SuspendLayout()
        Me.SplitContainerGraph2.SuspendLayout()
        CType(Me.ScatterGraphParameter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanelУправленияДляСнимка.SuspendLayout()
        Me.InstrumentControlStrip1.SuspendLayout()
        Me.TableLayoutPanelFrameCursor.SuspendLayout()
        Me.GroupBoxAxis.SuspendLayout()
        Me.GroupBoxDelete.SuspendLayout()
        Me.PanelKey.SuspendLayout()
        Me.ToolStripMain2.SuspendLayout()
        Me.GroupeBoxCursorStart.SuspendLayout()
        Me.GroupeBoxCursorEnd.SuspendLayout()
        Me.MenuStripShap.SuspendLayout()
        Me.ToolStripMain.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.StatusStripMain.SuspendLayout()
        Me.ContextMenuAnnotation.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainerForm)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1016, 563)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1016, 670)
        Me.ToolStripContainer1.TabIndex = 0
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStripShap)
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStripMain)
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip2)
        '
        'SplitContainerForm
        '
        Me.SplitContainerForm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerForm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerForm.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerForm.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerForm.Name = "SplitContainerForm"
        Me.SplitContainerForm.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerForm.Panel1
        '
        Me.SplitContainerForm.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainerForm.Panel2
        '
        Me.SplitContainerForm.Panel2.AutoScroll = True
        Me.SplitContainerForm.Panel2.Controls.Add(Me.TableLayoutPanelУправленияДляСнимка)
        Me.SplitContainerForm.Size = New System.Drawing.Size(1016, 563)
        Me.SplitContainerForm.SplitterDistance = 380
        Me.SplitContainerForm.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.SlidePlot)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PanelSlide)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainerGraph)
        Me.SplitContainer2.Size = New System.Drawing.Size(1016, 380)
        Me.SplitContainer2.SplitterDistance = 120
        Me.SplitContainer2.TabIndex = 0
        '
        'SlidePlot
        '
        Me.SlidePlot.AutoDivisionSpacing = False
        Me.SlidePlot.Caption = "№ шлейфа"
        Me.SlidePlot.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval
        Me.SlidePlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SlidePlot.FillBackColor = System.Drawing.Color.Gainsboro
        Me.SlidePlot.InvertedScale = True
        Me.SlidePlot.Location = New System.Drawing.Point(0, 0)
        Me.SlidePlot.MajorDivisions.Base = 1.0R
        Me.SlidePlot.MajorDivisions.Interval = 1.0R
        Me.SlidePlot.MinorDivisions.TickVisible = False
        Me.SlidePlot.Name = "SlidePlot"
        Me.SlidePlot.PointerColor = System.Drawing.Color.Silver
        Me.SlidePlot.Range = New NationalInstruments.UI.Range(1.0R, 10.0R)
        Me.SlidePlot.ScaleBaseLineVisible = True
        Me.SlidePlot.Size = New System.Drawing.Size(85, 376)
        Me.SlidePlot.SlideStyle = NationalInstruments.UI.SlideStyle.SunkenWithGrip
        Me.SlidePlot.TabIndex = 142
        Me.ToolTip1.SetToolTip(Me.SlidePlot, "Имя канала при построения Оси Y")
        Me.SlidePlot.Value = 1.0R
        '
        'PanelSlide
        '
        Me.PanelSlide.Controls.Add(Me.SlideAxis)
        Me.PanelSlide.Controls.Add(Me.PanelSlideBottom)
        Me.PanelSlide.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelSlide.Location = New System.Drawing.Point(85, 0)
        Me.PanelSlide.Name = "PanelSlide"
        Me.PanelSlide.Size = New System.Drawing.Size(31, 376)
        Me.PanelSlide.TabIndex = 144
        '
        'SlideAxis
        '
        Me.SlideAxis.CaptionVisible = False
        Me.SlideAxis.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SlideAxis.FillBackColor = System.Drawing.Color.Transparent
        Me.SlideAxis.FillColor = System.Drawing.Color.Transparent
        Me.SlideAxis.FillStyle = NationalInstruments.UI.FillStyle.None
        Me.SlideAxis.InteractionMode = NationalInstruments.UI.LinearNumericPointerInteractionModes.Indicator
        Me.SlideAxis.Location = New System.Drawing.Point(0, 0)
        Me.SlideAxis.MajorDivisions.LabelVisible = False
        Me.SlideAxis.MajorDivisions.TickLength = 1.0!
        Me.SlideAxis.MajorDivisions.TickVisible = False
        Me.SlideAxis.MinorDivisions.TickLength = 1.0!
        Me.SlideAxis.MinorDivisions.TickVisible = False
        Me.SlideAxis.Name = "SlideAxis"
        Me.SlideAxis.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.SlideAxis.PointerColor = System.Drawing.Color.Red
        Me.SlideAxis.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        Me.SlideAxis.ScalePosition = NationalInstruments.UI.NumericScalePosition.Right
        Me.SlideAxis.ScaleVisible = False
        Me.SlideAxis.Size = New System.Drawing.Size(31, 361)
        Me.SlideAxis.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideAxis.TabIndex = 143
        Me.SlideAxis.Value = 50.0R
        '
        'PanelSlideBottom
        '
        Me.PanelSlideBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelSlideBottom.Location = New System.Drawing.Point(0, 361)
        Me.PanelSlideBottom.Name = "PanelSlideBottom"
        Me.PanelSlideBottom.Size = New System.Drawing.Size(31, 15)
        Me.PanelSlideBottom.TabIndex = 144
        '
        'SplitContainerGraph
        '
        Me.SplitContainerGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGraph.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGraph.Name = "SplitContainerGraph"
        '
        'SplitContainerGraph.Panel1
        '
        Me.SplitContainerGraph.Panel1.Controls.Add(Me.SplitContainerGraph1)
        Me.SplitContainerGraph.Panel1MinSize = 0
        '
        'SplitContainerGraph.Panel2
        '
        Me.SplitContainerGraph.Panel2.Controls.Add(Me.SplitContainerGraph2)
        Me.SplitContainerGraph.Panel2MinSize = 0
        Me.SplitContainerGraph.Size = New System.Drawing.Size(892, 380)
        Me.SplitContainerGraph.SplitterDistance = 436
        Me.SplitContainerGraph.TabIndex = 1
        '
        'SplitContainerGraph1
        '
        Me.SplitContainerGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGraph1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGraph1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerGraph1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGraph1.Name = "SplitContainerGraph1"
        '
        'SplitContainerGraph1.Panel1
        '
        Me.SplitContainerGraph1.Panel1.Controls.Add(Me.TextBoxCaption)
        Me.SplitContainerGraph1.Panel1.Controls.Add(Me.WaveformGraphTime)
        '
        'SplitContainerGraph1.Panel2
        '
        Me.SplitContainerGraph1.Panel2.Controls.Add(Me.ListViewAcquisition)
        Me.SplitContainerGraph1.Panel2.Controls.Add(Me.PanelMode)
        Me.SplitContainerGraph1.Size = New System.Drawing.Size(436, 380)
        Me.SplitContainerGraph1.SplitterDistance = 253
        Me.SplitContainerGraph1.TabIndex = 0
        '
        'TextBoxCaption
        '
        Me.TextBoxCaption.BackColor = System.Drawing.Color.LightYellow
        Me.TextBoxCaption.ForeColor = System.Drawing.Color.Blue
        Me.TextBoxCaption.Location = New System.Drawing.Point(98, 210)
        Me.TextBoxCaption.Name = "TextBoxCaption"
        Me.TextBoxCaption.Size = New System.Drawing.Size(69, 20)
        Me.TextBoxCaption.TabIndex = 136
        Me.TextBoxCaption.Text = "123456789"
        Me.TextBoxCaption.Visible = False
        '
        'WaveformGraphTime
        '
        Me.WaveformGraphTime.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XyCursorStart, Me.XyCursorEnd, Me.XyCursorTime, Me.XyCursorParametr})
        Me.WaveformGraphTime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WaveformGraphTime.Location = New System.Drawing.Point(0, 0)
        Me.WaveformGraphTime.Name = "WaveformGraphTime"
        Me.WaveformGraphTime.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.WaveformPlot1})
        Me.WaveformGraphTime.Size = New System.Drawing.Size(249, 376)
        Me.WaveformGraphTime.TabIndex = 135
        Me.WaveformGraphTime.UseColorGenerator = True
        Me.WaveformGraphTime.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxisTime})
        Me.WaveformGraphTime.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxisTime})
        '
        'XyCursorStart
        '
        Me.XyCursorStart.Color = System.Drawing.Color.Yellow
        Me.XyCursorStart.LineStyle = NationalInstruments.UI.LineStyle.Dot
        Me.XyCursorStart.Plot = Me.WaveformPlot1
        Me.XyCursorStart.SnapMode = NationalInstruments.UI.CursorSnapMode.Floating
        Me.XyCursorStart.Visible = False
        '
        'WaveformPlot1
        '
        Me.WaveformPlot1.LineColor = System.Drawing.Color.White
        Me.WaveformPlot1.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.WaveformPlot1.Visible = False
        Me.WaveformPlot1.XAxis = Me.XAxisTime
        Me.WaveformPlot1.YAxis = Me.YAxisTime
        '
        'XAxisTime
        '
        Me.XAxisTime.InteractionMode = NationalInstruments.UI.ScaleInteractionMode.None
        Me.XAxisTime.MajorDivisions.GridVisible = True
        Me.XAxisTime.MinorDivisions.GridVisible = True
        Me.XAxisTime.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.XAxisTime.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'YAxisTime
        '
        Me.YAxisTime.InteractionMode = NationalInstruments.UI.ScaleInteractionMode.None
        Me.YAxisTime.MajorDivisions.GridVisible = True
        Me.YAxisTime.MinorDivisions.GridVisible = True
        Me.YAxisTime.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.YAxisTime.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'XyCursorEnd
        '
        Me.XyCursorEnd.Color = System.Drawing.Color.Cyan
        Me.XyCursorEnd.LineStyle = NationalInstruments.UI.LineStyle.Dot
        Me.XyCursorEnd.Plot = Me.WaveformPlot1
        Me.XyCursorEnd.SnapMode = NationalInstruments.UI.CursorSnapMode.Floating
        Me.XyCursorEnd.Visible = False
        '
        'XyCursorTime
        '
        Me.XyCursorTime.Color = System.Drawing.Color.White
        Me.XyCursorTime.HorizontalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        Me.XyCursorTime.Plot = Me.WaveformPlot1
        Me.XyCursorTime.PointStyle = NationalInstruments.UI.PointStyle.SolidTriangleUp
        Me.XyCursorTime.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        '
        'XyCursorParametr
        '
        Me.XyCursorParametr.Color = System.Drawing.Color.White
        Me.XyCursorParametr.HorizontalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.Custom
        Me.XyCursorParametr.Plot = Me.WaveformPlot1
        Me.XyCursorParametr.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        Me.XyCursorParametr.VerticalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.Custom
        Me.XyCursorParametr.Visible = False
        '
        'ListViewAcquisition
        '
        Me.ListViewAcquisition.BackColor = System.Drawing.Color.Black
        Me.ListViewAcquisition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewAcquisition.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ListViewAcquisition.ForeColor = System.Drawing.Color.White
        Me.ListViewAcquisition.GridLines = True
        Me.ListViewAcquisition.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewAcquisition.HideSelection = False
        Me.ListViewAcquisition.Location = New System.Drawing.Point(0, 60)
        Me.ListViewAcquisition.MultiSelect = False
        Me.ListViewAcquisition.Name = "ListViewAcquisition"
        Me.ListViewAcquisition.Size = New System.Drawing.Size(175, 316)
        Me.ListViewAcquisition.TabIndex = 142
        Me.ListViewAcquisition.UseCompatibleStateImageBehavior = False
        Me.ListViewAcquisition.View = System.Windows.Forms.View.Details
        '
        'PanelMode
        '
        Me.PanelMode.Controls.Add(Me.TableLayoutPanelMode)
        Me.PanelMode.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelMode.Location = New System.Drawing.Point(0, 0)
        Me.PanelMode.Name = "PanelMode"
        Me.PanelMode.Size = New System.Drawing.Size(175, 60)
        Me.PanelMode.TabIndex = 141
        '
        'TableLayoutPanelMode
        '
        Me.TableLayoutPanelMode.ColumnCount = 2
        Me.TableLayoutPanelMode.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMode.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelMode.Controls.Add(Me.ComboBoxDisplayRate, 0, 0)
        Me.TableLayoutPanelMode.Controls.Add(Me.NumericUpDownPrecisionScreen, 1, 0)
        Me.TableLayoutPanelMode.Controls.Add(Me.ComboBoxSelectiveList, 0, 1)
        Me.TableLayoutPanelMode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMode.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMode.Name = "TableLayoutPanelMode"
        Me.TableLayoutPanelMode.RowCount = 2
        Me.TableLayoutPanelMode.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanelMode.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMode.Size = New System.Drawing.Size(175, 60)
        Me.TableLayoutPanelMode.TabIndex = 142
        '
        'ComboBoxDisplayRate
        '
        Me.ComboBoxDisplayRate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxDisplayRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxDisplayRate.FormattingEnabled = True
        Me.ComboBoxDisplayRate.Location = New System.Drawing.Point(3, 3)
        Me.ComboBoxDisplayRate.Name = "ComboBoxDisplayRate"
        Me.ComboBoxDisplayRate.Size = New System.Drawing.Size(129, 24)
        Me.ComboBoxDisplayRate.TabIndex = 141
        Me.ToolTip1.SetToolTip(Me.ComboBoxDisplayRate, "Скорость обновления значений")
        '
        'NumericUpDownPrecisionScreen
        '
        Me.NumericUpDownPrecisionScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.NumericUpDownPrecisionScreen.Location = New System.Drawing.Point(138, 3)
        Me.NumericUpDownPrecisionScreen.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.NumericUpDownPrecisionScreen.Name = "NumericUpDownPrecisionScreen"
        Me.NumericUpDownPrecisionScreen.Size = New System.Drawing.Size(34, 24)
        Me.NumericUpDownPrecisionScreen.TabIndex = 140
        Me.ToolTip1.SetToolTip(Me.NumericUpDownPrecisionScreen, "Формат точности представления числа при выводе на экран")
        '
        'ComboBoxSelectiveList
        '
        Me.ComboBoxSelectiveList.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanelMode.SetColumnSpan(Me.ComboBoxSelectiveList, 2)
        Me.ComboBoxSelectiveList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxSelectiveList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxSelectiveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSelectiveList.DropDownWidth = 141
        Me.ComboBoxSelectiveList.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxSelectiveList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxSelectiveList.Location = New System.Drawing.Point(3, 31)
        Me.ComboBoxSelectiveList.MaxDropDownItems = 10
        Me.ComboBoxSelectiveList.Name = "ComboBoxSelectiveList"
        Me.ComboBoxSelectiveList.Size = New System.Drawing.Size(169, 27)
        Me.ComboBoxSelectiveList.TabIndex = 139
        '
        'SplitContainerGraph2
        '
        Me.SplitContainerGraph2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerGraph2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerGraph2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerGraph2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerGraph2.Name = "SplitContainerGraph2"
        '
        'SplitContainerGraph2.Panel1
        '
        Me.SplitContainerGraph2.Panel1.Controls.Add(Me.ScatterGraphParameter)
        Me.SplitContainerGraph2.Panel1.Controls.Add(Me.SlideTime)
        '
        'SplitContainerGraph2.Panel2
        '
        Me.SplitContainerGraph2.Panel2.Controls.Add(Me.ListViewParametr)
        Me.SplitContainerGraph2.Size = New System.Drawing.Size(452, 380)
        Me.SplitContainerGraph2.SplitterDistance = 264
        Me.SplitContainerGraph2.TabIndex = 0
        '
        'ScatterGraphParameter
        '
        Me.ScatterGraphParameter.BackColor = System.Drawing.Color.DimGray
        Me.ScatterGraphParameter.Border = NationalInstruments.UI.Border.None
        Me.ScatterGraphParameter.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XyCursor1})
        Me.ScatterGraphParameter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScatterGraphParameter.InteractionMode = NationalInstruments.UI.GraphInteractionModes.None
        Me.ScatterGraphParameter.Location = New System.Drawing.Point(0, 0)
        Me.ScatterGraphParameter.Name = "ScatterGraphParameter"
        Me.ScatterGraphParameter.Plots.AddRange(New NationalInstruments.UI.ScatterPlot() {Me.ScatterPlot2})
        Me.ScatterGraphParameter.Size = New System.Drawing.Size(260, 322)
        Me.ScatterGraphParameter.TabIndex = 125
        Me.ScatterGraphParameter.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
        Me.ScatterGraphParameter.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
        '
        'XyCursor1
        '
        Me.XyCursor1.Color = System.Drawing.Color.White
        Me.XyCursor1.HorizontalCrosshairMode = NationalInstruments.UI.CursorCrosshairMode.None
        Me.XyCursor1.PointStyle = NationalInstruments.UI.PointStyle.SolidTriangleUp
        Me.XyCursor1.SnapMode = NationalInstruments.UI.CursorSnapMode.Fixed
        '
        'ScatterPlot2
        '
        Me.ScatterPlot2.XAxis = Me.XAxis1
        Me.ScatterPlot2.YAxis = Me.YAxis1
        '
        'XAxis1
        '
        Me.XAxis1.AutoMinorDivisionFrequency = 4
        Me.XAxis1.CaptionForeColor = System.Drawing.Color.White
        Me.XAxis1.MajorDivisions.GridVisible = True
        Me.XAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
        Me.XAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
        Me.XAxis1.MinorDivisions.GridVisible = True
        Me.XAxis1.MinorDivisions.TickColor = System.Drawing.Color.White
        Me.XAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.XAxis1.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'YAxis1
        '
        Me.YAxis1.AutoMinorDivisionFrequency = 4
        Me.YAxis1.CaptionForeColor = System.Drawing.Color.White
        Me.YAxis1.MajorDivisions.GridVisible = True
        Me.YAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
        Me.YAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
        Me.YAxis1.MinorDivisions.GridVisible = True
        Me.YAxis1.MinorDivisions.TickColor = System.Drawing.Color.White
        Me.YAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.YAxis1.Range = New NationalInstruments.UI.Range(0R, 100.0R)
        '
        'SlideTime
        '
        Me.SlideTime.Border = NationalInstruments.UI.Border.ThinFrame3D
        ScaleCustomDivision1.LineWidth = 2.0!
        ScaleCustomDivision1.Value = 3.5R
        Me.SlideTime.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision1})
        Me.SlideTime.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SlideTime.FillBackColor = System.Drawing.Color.Silver
        Me.SlideTime.InteractionMode = NationalInstruments.UI.LinearNumericPointerInteractionModes.Indicator
        Me.SlideTime.Location = New System.Drawing.Point(0, 322)
        Me.SlideTime.Name = "SlideTime"
        Me.SlideTime.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.SlideTime.PointerColor = System.Drawing.Color.Maroon
        Me.SlideTime.ScalePosition = NationalInstruments.UI.NumericScalePosition.Top
        Me.SlideTime.Size = New System.Drawing.Size(260, 54)
        Me.SlideTime.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideTime.TabIndex = 123
        Me.SlideTime.TabStop = False
        '
        'ListViewParametr
        '
        Me.ListViewParametr.BackColor = System.Drawing.Color.Black
        Me.ListViewParametr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewParametr.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ListViewParametr.ForeColor = System.Drawing.Color.White
        Me.ListViewParametr.GridLines = True
        Me.ListViewParametr.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewParametr.HideSelection = False
        Me.ListViewParametr.Location = New System.Drawing.Point(0, 0)
        Me.ListViewParametr.MultiSelect = False
        Me.ListViewParametr.Name = "ListViewParametr"
        Me.ListViewParametr.Size = New System.Drawing.Size(180, 376)
        Me.ListViewParametr.TabIndex = 0
        Me.ListViewParametr.UseCompatibleStateImageBehavior = False
        Me.ListViewParametr.View = System.Windows.Forms.View.Details
        '
        'TableLayoutPanelУправленияДляСнимка
        '
        Me.TableLayoutPanelУправленияДляСнимка.ColumnCount = 1
        Me.TableLayoutPanelУправленияДляСнимка.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelУправленияДляСнимка.Controls.Add(Me.InstrumentControlStrip1, 0, 1)
        Me.TableLayoutPanelУправленияДляСнимка.Controls.Add(Me.TableLayoutPanelFrameCursor, 0, 0)
        Me.TableLayoutPanelУправленияДляСнимка.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelУправленияДляСнимка.Name = "TableLayoutPanelУправленияДляСнимка"
        Me.TableLayoutPanelУправленияДляСнимка.RowCount = 2
        Me.TableLayoutPanelУправленияДляСнимка.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelУправленияДляСнимка.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanelУправленияДляСнимка.Size = New System.Drawing.Size(869, 152)
        Me.TableLayoutPanelУправленияДляСнимка.TabIndex = 141
        '
        'InstrumentControlStrip1
        '
        Me.InstrumentControlStrip1.AutoSize = False
        Me.InstrumentControlStrip1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.InstrumentControlStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InstrumentControlStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.InstrumentControlStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.ToolStripPropertyEditor1, Me.ToolStripLabel2, Me.ToolStripPropertyEditor2, Me.ToolStripLabel3, Me.ToolStripPropertyEditor3, Me.ToolStripLabel4, Me.ToolStripPropertyEditor4, Me.ToolStripLabel5, Me.ToolStripPropertyEditor5, Me.ToolStripLabel6, Me.ToolStripPropertyEditor6})
        Me.InstrumentControlStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.InstrumentControlStrip1.Location = New System.Drawing.Point(0, 128)
        Me.InstrumentControlStrip1.Name = "InstrumentControlStrip1"
        Me.InstrumentControlStrip1.Size = New System.Drawing.Size(869, 24)
        Me.InstrumentControlStrip1.TabIndex = 36
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(72, 21)
        Me.ToolStripLabel1.Text = "Разрешено:"
        '
        'ToolStripPropertyEditor1
        '
        Me.ToolStripPropertyEditor1.AutoSize = False
        Me.ToolStripPropertyEditor1.Name = "ToolStripPropertyEditor1"
        Me.ToolStripPropertyEditor1.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor1.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor1.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "InteractionMode")
        Me.ToolStripPropertyEditor1.Text = "ZoomX, ZoomY, ZoomAroundPoint, PanX, PanY, DragCursor, DragAnnotationCaption"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(59, 21)
        Me.ToolStripLabel2.Text = "Курсоры:"
        '
        'ToolStripPropertyEditor2
        '
        Me.ToolStripPropertyEditor2.AutoSize = False
        Me.ToolStripPropertyEditor2.Name = "ToolStripPropertyEditor2"
        Me.ToolStripPropertyEditor2.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor2.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor2.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "Cursors")
        Me.ToolStripPropertyEditor2.Text = "(Коллекция)"
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(81, 21)
        Me.ToolStripLabel3.Text = "Примечания:"
        '
        'ToolStripPropertyEditor3
        '
        Me.ToolStripPropertyEditor3.AutoSize = False
        Me.ToolStripPropertyEditor3.Name = "ToolStripPropertyEditor3"
        Me.ToolStripPropertyEditor3.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor3.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor3.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "Annotations")
        Me.ToolStripPropertyEditor3.Text = "(Коллекция)"
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(59, 21)
        Me.ToolStripLabel4.Text = "Шлейфы:"
        '
        'ToolStripPropertyEditor4
        '
        Me.ToolStripPropertyEditor4.AutoSize = False
        Me.ToolStripPropertyEditor4.Name = "ToolStripPropertyEditor4"
        Me.ToolStripPropertyEditor4.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor4.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor4.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "Plots")
        Me.ToolStripPropertyEditor4.Text = "(Коллекция)"
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(39, 21)
        Me.ToolStripLabel5.Text = "XОси:"
        '
        'ToolStripPropertyEditor5
        '
        Me.ToolStripPropertyEditor5.AutoSize = False
        Me.ToolStripPropertyEditor5.Name = "ToolStripPropertyEditor5"
        Me.ToolStripPropertyEditor5.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor5.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor5.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "XAxes")
        Me.ToolStripPropertyEditor5.Text = "(Коллекция)"
        '
        'ToolStripLabel6
        '
        Me.ToolStripLabel6.Name = "ToolStripLabel6"
        Me.ToolStripLabel6.Size = New System.Drawing.Size(39, 15)
        Me.ToolStripLabel6.Text = "YОси:"
        '
        'ToolStripPropertyEditor6
        '
        Me.ToolStripPropertyEditor6.AutoSize = False
        Me.ToolStripPropertyEditor6.Name = "ToolStripPropertyEditor6"
        Me.ToolStripPropertyEditor6.RenderMode = NationalInstruments.UI.PropertyEditorRenderMode.Inherit
        Me.ToolStripPropertyEditor6.Size = New System.Drawing.Size(120, 23)
        Me.ToolStripPropertyEditor6.Source = New NationalInstruments.UI.PropertyEditorSource(Me.WaveformGraphTime, "YAxes")
        Me.ToolStripPropertyEditor6.Text = "(Коллекция)"
        '
        'TableLayoutPanelFrameCursor
        '
        Me.TableLayoutPanelFrameCursor.ColumnCount = 4
        Me.TableLayoutPanelFrameCursor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 127.0!))
        Me.TableLayoutPanelFrameCursor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 127.0!))
        Me.TableLayoutPanelFrameCursor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 237.0!))
        Me.TableLayoutPanelFrameCursor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelFrameCursor.Controls.Add(Me.GroupBoxAxis, 2, 0)
        Me.TableLayoutPanelFrameCursor.Controls.Add(Me.PanelKey, 3, 0)
        Me.TableLayoutPanelFrameCursor.Controls.Add(Me.GroupeBoxCursorStart, 0, 0)
        Me.TableLayoutPanelFrameCursor.Controls.Add(Me.GroupeBoxCursorEnd, 1, 0)
        Me.TableLayoutPanelFrameCursor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelFrameCursor.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelFrameCursor.Name = "TableLayoutPanelFrameCursor"
        Me.TableLayoutPanelFrameCursor.RowCount = 1
        Me.TableLayoutPanelFrameCursor.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelFrameCursor.Size = New System.Drawing.Size(863, 122)
        Me.TableLayoutPanelFrameCursor.TabIndex = 37
        '
        'GroupBoxAxis
        '
        Me.GroupBoxAxis.BackColor = System.Drawing.Color.Silver
        Me.GroupBoxAxis.Controls.Add(Me.GroupBoxDelete)
        Me.GroupBoxAxis.Controls.Add(Me.TextBoxDescriptionOnAxes)
        Me.GroupBoxAxis.Controls.Add(Me.ButtonShowAxes)
        Me.GroupBoxAxis.Controls.Add(Me.ComboBoxPointers)
        Me.GroupBoxAxis.Controls.Add(Me.ButtonFixLine)
        Me.GroupBoxAxis.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxAxis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxAxis.Location = New System.Drawing.Point(257, 3)
        Me.GroupBoxAxis.Name = "GroupBoxAxis"
        Me.GroupBoxAxis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxAxis.Size = New System.Drawing.Size(230, 89)
        Me.GroupBoxAxis.TabIndex = 119
        Me.GroupBoxAxis.TabStop = False
        Me.GroupBoxAxis.Text = "Стрелки и надписи"
        Me.GroupBoxAxis.Visible = False
        '
        'GroupBoxDelete
        '
        Me.GroupBoxDelete.BackColor = System.Drawing.Color.Silver
        Me.GroupBoxDelete.Controls.Add(Me.ComboBoxDescriptionPointer)
        Me.GroupBoxDelete.Controls.Add(Me.ButtonDelete)
        Me.GroupBoxDelete.Controls.Add(Me.TextBoxCount)
        Me.GroupBoxDelete.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBoxDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBoxDelete.Location = New System.Drawing.Point(120, 9)
        Me.GroupBoxDelete.Name = "GroupBoxDelete"
        Me.GroupBoxDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBoxDelete.Size = New System.Drawing.Size(105, 72)
        Me.GroupBoxDelete.TabIndex = 60
        Me.GroupBoxDelete.TabStop = False
        Me.GroupBoxDelete.Text = "Удаление"
        '
        'ComboBoxDescriptionPointer
        '
        Me.ComboBoxDescriptionPointer.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxDescriptionPointer.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxDescriptionPointer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDescriptionPointer.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxDescriptionPointer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxDescriptionPointer.Location = New System.Drawing.Point(4, 16)
        Me.ComboBoxDescriptionPointer.Name = "ComboBoxDescriptionPointer"
        Me.ComboBoxDescriptionPointer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxDescriptionPointer.Size = New System.Drawing.Size(97, 22)
        Me.ComboBoxDescriptionPointer.TabIndex = 63
        Me.ToolTip1.SetToolTip(Me.ComboBoxDescriptionPointer, "Выбрать разметку для удаления")
        '
        'ButtonDelete
        '
        Me.ButtonDelete.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonDelete.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonDelete.Location = New System.Drawing.Point(4, 40)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonDelete.Size = New System.Drawing.Size(61, 22)
        Me.ButtonDelete.TabIndex = 62
        Me.ButtonDelete.Text = "Удалить"
        Me.ToolTip1.SetToolTip(Me.ButtonDelete, "Удалить выбранную разметку")
        Me.ButtonDelete.UseVisualStyleBackColor = True
        Me.ButtonDelete.Visible = False
        '
        'TextBoxCount
        '
        Me.TextBoxCount.AcceptsReturn = True
        Me.TextBoxCount.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxCount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxCount.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxCount.Location = New System.Drawing.Point(68, 40)
        Me.TextBoxCount.MaxLength = 0
        Me.TextBoxCount.Name = "TextBoxCount"
        Me.TextBoxCount.ReadOnly = True
        Me.TextBoxCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxCount.Size = New System.Drawing.Size(33, 20)
        Me.TextBoxCount.TabIndex = 61
        Me.ToolTip1.SetToolTip(Me.TextBoxCount, "Количество разметок")
        Me.TextBoxCount.WordWrap = False
        '
        'TextBoxDescriptionOnAxes
        '
        Me.TextBoxDescriptionOnAxes.AcceptsReturn = True
        Me.TextBoxDescriptionOnAxes.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxDescriptionOnAxes.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxDescriptionOnAxes.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxDescriptionOnAxes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxDescriptionOnAxes.Location = New System.Drawing.Point(4, 64)
        Me.TextBoxDescriptionOnAxes.MaxLength = 0
        Me.TextBoxDescriptionOnAxes.Name = "TextBoxDescriptionOnAxes"
        Me.TextBoxDescriptionOnAxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxDescriptionOnAxes.Size = New System.Drawing.Size(113, 20)
        Me.TextBoxDescriptionOnAxes.TabIndex = 36
        Me.TextBoxDescriptionOnAxes.Text = "t="
        Me.ToolTip1.SetToolTip(Me.TextBoxDescriptionOnAxes, "Ввод текста для метки наклонной линии")
        '
        'ButtonShowAxes
        '
        Me.ButtonShowAxes.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonShowAxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonShowAxes.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonShowAxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonShowAxes.Location = New System.Drawing.Point(4, 36)
        Me.ButtonShowAxes.Name = "ButtonShowAxes"
        Me.ButtonShowAxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonShowAxes.Size = New System.Drawing.Size(66, 26)
        Me.ButtonShowAxes.TabIndex = 35
        Me.ButtonShowAxes.Text = "Показать"
        Me.ToolTip1.SetToolTip(Me.ButtonShowAxes, "Показать выбранную разметку между курсорами")
        Me.ButtonShowAxes.UseVisualStyleBackColor = True
        '
        'ComboBoxPointers
        '
        Me.ComboBoxPointers.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxPointers.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxPointers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPointers.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxPointers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxPointers.Location = New System.Drawing.Point(4, 12)
        Me.ComboBoxPointers.Name = "ComboBoxPointers"
        Me.ComboBoxPointers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxPointers.Size = New System.Drawing.Size(113, 22)
        Me.ComboBoxPointers.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.ComboBoxPointers, "Выбрать вид разметки")
        '
        'ButtonFixLine
        '
        Me.ButtonFixLine.BackColor = System.Drawing.Color.Red
        Me.ButtonFixLine.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonFixLine.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonFixLine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonFixLine.Location = New System.Drawing.Point(76, 36)
        Me.ButtonFixLine.Name = "ButtonFixLine"
        Me.ButtonFixLine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonFixLine.Size = New System.Drawing.Size(41, 26)
        Me.ButtonFixLine.TabIndex = 37
        Me.ButtonFixLine.Text = "OK"
        Me.ToolTip1.SetToolTip(Me.ButtonFixLine, "Подтвердить правильность расположения наклонной линии")
        Me.ButtonFixLine.UseVisualStyleBackColor = False
        Me.ButtonFixLine.Visible = False
        '
        'PanelKey
        '
        Me.PanelKey.BackColor = System.Drawing.Color.Silver
        Me.PanelKey.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelKey.Controls.Add(Me.ListViewAction)
        Me.PanelKey.Controls.Add(Me.ToolStripMain2)
        Me.PanelKey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelKey.Location = New System.Drawing.Point(494, 3)
        Me.PanelKey.Name = "PanelKey"
        Me.PanelKey.Size = New System.Drawing.Size(366, 116)
        Me.PanelKey.TabIndex = 121
        '
        'ListViewAction
        '
        Me.ListViewAction.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ListViewAction.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderKeys1, Me.ColumnHeaderAction1, Me.ColumnHeaderKeys2, Me.ColumnHeaderAction2})
        Me.ListViewAction.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewAction.ForeColor = System.Drawing.Color.Black
        Me.ListViewAction.FullRowSelect = True
        Me.ListViewAction.GridLines = True
        Me.ListViewAction.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewAction.HideSelection = False
        Me.ListViewAction.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9})
        Me.ListViewAction.Location = New System.Drawing.Point(0, 25)
        Me.ListViewAction.Name = "ListViewAction"
        Me.ListViewAction.Size = New System.Drawing.Size(362, 87)
        Me.ListViewAction.TabIndex = 9
        Me.ListViewAction.UseCompatibleStateImageBehavior = False
        Me.ListViewAction.View = System.Windows.Forms.View.Details
        '
        'ColumnHeaderKeys1
        '
        Me.ColumnHeaderKeys1.Text = "Сочетание клавиш"
        Me.ColumnHeaderKeys1.Width = 214
        '
        'ColumnHeaderAction1
        '
        Me.ColumnHeaderAction1.Text = "Действие"
        Me.ColumnHeaderAction1.Width = 247
        '
        'ColumnHeaderKeys2
        '
        Me.ColumnHeaderKeys2.Text = "Сочетание клавиш"
        Me.ColumnHeaderKeys2.Width = 214
        '
        'ColumnHeaderAction2
        '
        Me.ColumnHeaderAction2.Text = "Действие"
        Me.ColumnHeaderAction2.Width = 247
        '
        'ToolStripMain2
        '
        Me.ToolStripMain2.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripMain2.CanOverflow = False
        Me.ToolStripMain2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStripMain2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ButtonOneCursor, Me.ButtonTwoCursor, Me.ToolStripSeparator16, Me.ButtonXRange, Me.ButtonYRange, Me.ToolStripSeparator15, Me.EnableLabel, Me.ButtonDragCursor, Me.ButtonPanX, Me.ButtonPanY, Me.ButtonZoomPoint, Me.ButtonZoomX, Me.ButtonZoomY, Me.ToolStripSeparator17, Me.ButtonMore, Me.ToolStripSeparator19, Me.ButtonAddTiks, Me.ButtonDeleteTiks})
        Me.ToolStripMain2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripMain2.Name = "ToolStripMain2"
        Me.ToolStripMain2.Size = New System.Drawing.Size(362, 25)
        Me.ToolStripMain2.TabIndex = 8
        Me.ToolStripMain2.Text = "ToolStrip1"
        '
        'ButtonOneCursor
        '
        Me.ButtonOneCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ButtonOneCursor.Image = CType(resources.GetObject("ButtonOneCursor.Image"), System.Drawing.Image)
        Me.ButtonOneCursor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonOneCursor.Name = "ButtonOneCursor"
        Me.ButtonOneCursor.Size = New System.Drawing.Size(82, 22)
        Me.ButtonOneCursor.Text = "Один курсор"
        Me.ButtonOneCursor.ToolTipText = "Показать только курсор начала"
        '
        'ButtonTwoCursor
        '
        Me.ButtonTwoCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ButtonTwoCursor.Image = CType(resources.GetObject("ButtonTwoCursor.Image"), System.Drawing.Image)
        Me.ButtonTwoCursor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonTwoCursor.Name = "ButtonTwoCursor"
        Me.ButtonTwoCursor.Size = New System.Drawing.Size(79, 22)
        Me.ButtonTwoCursor.Text = "Два курсора"
        Me.ButtonTwoCursor.ToolTipText = "Показать оба курсора"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 25)
        '
        'ButtonXRange
        '
        Me.ButtonXRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonXRange.Image = CType(resources.GetObject("ButtonXRange.Image"), System.Drawing.Image)
        Me.ButtonXRange.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonXRange.Name = "ButtonXRange"
        Me.ButtonXRange.Size = New System.Drawing.Size(23, 22)
        Me.ButtonXRange.ToolTipText = "Задать X диапазон"
        '
        'ButtonYRange
        '
        Me.ButtonYRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonYRange.Image = CType(resources.GetObject("ButtonYRange.Image"), System.Drawing.Image)
        Me.ButtonYRange.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonYRange.Name = "ButtonYRange"
        Me.ButtonYRange.Size = New System.Drawing.Size(23, 22)
        Me.ButtonYRange.ToolTipText = "Задать Y диапазон"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 25)
        '
        'EnableLabel
        '
        Me.EnableLabel.Name = "EnableLabel"
        Me.EnableLabel.Size = New System.Drawing.Size(72, 22)
        Me.EnableLabel.Text = "Разрешено:"
        '
        'ButtonDragCursor
        '
        Me.ButtonDragCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonDragCursor.Image = CType(resources.GetObject("ButtonDragCursor.Image"), System.Drawing.Image)
        Me.ButtonDragCursor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonDragCursor.Name = "ButtonDragCursor"
        Me.ButtonDragCursor.Size = New System.Drawing.Size(23, 22)
        Me.ButtonDragCursor.ToolTipText = "Перемещать Курсор"
        '
        'ButtonPanX
        '
        Me.ButtonPanX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonPanX.Image = CType(resources.GetObject("ButtonPanX.Image"), System.Drawing.Image)
        Me.ButtonPanX.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonPanX.Name = "ButtonPanX"
        Me.ButtonPanX.Size = New System.Drawing.Size(23, 22)
        Me.ButtonPanX.ToolTipText = "Панорама X"
        '
        'ButtonPanY
        '
        Me.ButtonPanY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonPanY.Image = CType(resources.GetObject("ButtonPanY.Image"), System.Drawing.Image)
        Me.ButtonPanY.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonPanY.Name = "ButtonPanY"
        Me.ButtonPanY.Size = New System.Drawing.Size(23, 22)
        Me.ButtonPanY.ToolTipText = "Панарома Y"
        '
        'ButtonZoomPoint
        '
        Me.ButtonZoomPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonZoomPoint.Image = CType(resources.GetObject("ButtonZoomPoint.Image"), System.Drawing.Image)
        Me.ButtonZoomPoint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonZoomPoint.Name = "ButtonZoomPoint"
        Me.ButtonZoomPoint.Size = New System.Drawing.Size(23, 22)
        Me.ButtonZoomPoint.ToolTipText = "Масштаб Вокруг Точки"
        '
        'ButtonZoomX
        '
        Me.ButtonZoomX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonZoomX.Image = CType(resources.GetObject("ButtonZoomX.Image"), System.Drawing.Image)
        Me.ButtonZoomX.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonZoomX.Name = "ButtonZoomX"
        Me.ButtonZoomX.Size = New System.Drawing.Size(23, 22)
        Me.ButtonZoomX.ToolTipText = "Масштаб X"
        '
        'ButtonZoomY
        '
        Me.ButtonZoomY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonZoomY.Image = CType(resources.GetObject("ButtonZoomY.Image"), System.Drawing.Image)
        Me.ButtonZoomY.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonZoomY.Name = "ButtonZoomY"
        Me.ButtonZoomY.Size = New System.Drawing.Size(23, 22)
        Me.ButtonZoomY.ToolTipText = "Масштаб Y"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 25)
        '
        'ButtonMore
        '
        Me.ButtonMore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonMore.Image = Global.Registration.My.Resources.Resources.down
        Me.ButtonMore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonMore.Name = "ButtonMore"
        Me.ButtonMore.Size = New System.Drawing.Size(23, 22)
        Me.ButtonMore.Text = "Показать"
        Me.ButtonMore.ToolTipText = "Показать дополнительную панель ..."
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(6, 25)
        '
        'ButtonAddTiks
        '
        Me.ButtonAddTiks.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonAddTiks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ButtonAddTiks.Image = CType(resources.GetObject("ButtonAddTiks.Image"), System.Drawing.Image)
        Me.ButtonAddTiks.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonAddTiks.Name = "ButtonAddTiks"
        Me.ButtonAddTiks.Size = New System.Drawing.Size(42, 22)
        Me.ButtonAddTiks.Text = "+ Сек"
        Me.ButtonAddTiks.ToolTipText = "Добавить разметку секунд"
        Me.ButtonAddTiks.Visible = False
        '
        'ButtonDeleteTiks
        '
        Me.ButtonDeleteTiks.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ButtonDeleteTiks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ButtonDeleteTiks.Image = CType(resources.GetObject("ButtonDeleteTiks.Image"), System.Drawing.Image)
        Me.ButtonDeleteTiks.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonDeleteTiks.Name = "ButtonDeleteTiks"
        Me.ButtonDeleteTiks.Size = New System.Drawing.Size(39, 22)
        Me.ButtonDeleteTiks.Text = "- Сек"
        Me.ButtonDeleteTiks.ToolTipText = "Уменьшить разметку секунд"
        Me.ButtonDeleteTiks.Visible = False
        '
        'GroupeBoxCursorStart
        '
        Me.GroupeBoxCursorStart.BackColor = System.Drawing.Color.Silver
        Me.GroupeBoxCursorStart.Controls.Add(Me.ButtonСursorStartMoveForward)
        Me.GroupeBoxCursorStart.Controls.Add(Me.ButtonСursorStartMoveBack)
        Me.GroupeBoxCursorStart.Controls.Add(Me.XPosition)
        Me.GroupeBoxCursorStart.Controls.Add(Me.YPosition)
        Me.GroupeBoxCursorStart.Controls.Add(Me.Label2)
        Me.GroupeBoxCursorStart.Controls.Add(Me.Label1)
        Me.GroupeBoxCursorStart.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupeBoxCursorStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupeBoxCursorStart.Location = New System.Drawing.Point(3, 3)
        Me.GroupeBoxCursorStart.Name = "GroupeBoxCursorStart"
        Me.GroupeBoxCursorStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupeBoxCursorStart.Size = New System.Drawing.Size(120, 88)
        Me.GroupeBoxCursorStart.TabIndex = 117
        Me.GroupeBoxCursorStart.TabStop = False
        Me.GroupeBoxCursorStart.Text = "Курсор начала"
        '
        'ButtonСursorStartMoveForward
        '
        Me.ButtonСursorStartMoveForward.BackColor = System.Drawing.Color.Khaki
        Me.ButtonСursorStartMoveForward.Image = CType(resources.GetObject("ButtonСursorStartMoveForward.Image"), System.Drawing.Image)
        Me.ButtonСursorStartMoveForward.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonСursorStartMoveForward.Location = New System.Drawing.Point(61, 55)
        Me.ButtonСursorStartMoveForward.Name = "ButtonСursorStartMoveForward"
        Me.ButtonСursorStartMoveForward.Size = New System.Drawing.Size(53, 29)
        Me.ButtonСursorStartMoveForward.TabIndex = 90
        Me.ButtonСursorStartMoveForward.Text = "Вперед"
        Me.ToolTip1.SetToolTip(Me.ButtonСursorStartMoveForward, "Переместить первый курсоор вперёд")
        Me.ButtonСursorStartMoveForward.UseVisualStyleBackColor = False
        '
        'ButtonСursorStartMoveBack
        '
        Me.ButtonСursorStartMoveBack.BackColor = System.Drawing.Color.Khaki
        Me.ButtonСursorStartMoveBack.Image = CType(resources.GetObject("ButtonСursorStartMoveBack.Image"), System.Drawing.Image)
        Me.ButtonСursorStartMoveBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonСursorStartMoveBack.Location = New System.Drawing.Point(4, 55)
        Me.ButtonСursorStartMoveBack.Name = "ButtonСursorStartMoveBack"
        Me.ButtonСursorStartMoveBack.Size = New System.Drawing.Size(54, 29)
        Me.ButtonСursorStartMoveBack.TabIndex = 89
        Me.ButtonСursorStartMoveBack.Text = "Назад"
        Me.ToolTip1.SetToolTip(Me.ButtonСursorStartMoveBack, "Переместить первый курсоор назад")
        Me.ButtonСursorStartMoveBack.UseVisualStyleBackColor = False
        '
        'XPosition
        '
        Me.XPosition.AcceptsReturn = True
        Me.XPosition.BackColor = System.Drawing.SystemColors.Window
        Me.XPosition.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.XPosition.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.XPosition.ForeColor = System.Drawing.SystemColors.WindowText
        Me.XPosition.Location = New System.Drawing.Point(72, 12)
        Me.XPosition.MaxLength = 0
        Me.XPosition.Name = "XPosition"
        Me.XPosition.ReadOnly = True
        Me.XPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.XPosition.Size = New System.Drawing.Size(41, 20)
        Me.XPosition.TabIndex = 17
        Me.XPosition.TabStop = False
        Me.ToolTip1.SetToolTip(Me.XPosition, "X позиция курсора на временной оси")
        '
        'YPosition
        '
        Me.YPosition.AcceptsReturn = True
        Me.YPosition.BackColor = System.Drawing.SystemColors.Window
        Me.YPosition.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.YPosition.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.YPosition.ForeColor = System.Drawing.SystemColors.WindowText
        Me.YPosition.Location = New System.Drawing.Point(72, 32)
        Me.YPosition.MaxLength = 0
        Me.YPosition.Name = "YPosition"
        Me.YPosition.ReadOnly = True
        Me.YPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.YPosition.Size = New System.Drawing.Size(41, 20)
        Me.YPosition.TabIndex = 16
        Me.YPosition.TabStop = False
        Me.ToolTip1.SetToolTip(Me.YPosition, "X позиция курсора на временной оси")
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(7, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(69, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Y Позиция:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(7, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(69, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "X Позиция:"
        '
        'GroupeBoxCursorEnd
        '
        Me.GroupeBoxCursorEnd.BackColor = System.Drawing.Color.Silver
        Me.GroupeBoxCursorEnd.Controls.Add(Me.PeriodVal)
        Me.GroupeBoxCursorEnd.Controls.Add(Me.AmplitudeVal)
        Me.GroupeBoxCursorEnd.Controls.Add(Me.ButtonСursorEndMoveForward)
        Me.GroupeBoxCursorEnd.Controls.Add(Me.ButtonСursorEndMoveBack)
        Me.GroupeBoxCursorEnd.Controls.Add(Me.Label4)
        Me.GroupeBoxCursorEnd.Controls.Add(Me.Label3)
        Me.GroupeBoxCursorEnd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupeBoxCursorEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupeBoxCursorEnd.Location = New System.Drawing.Point(130, 3)
        Me.GroupeBoxCursorEnd.Name = "GroupeBoxCursorEnd"
        Me.GroupeBoxCursorEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupeBoxCursorEnd.Size = New System.Drawing.Size(120, 88)
        Me.GroupeBoxCursorEnd.TabIndex = 115
        Me.GroupeBoxCursorEnd.TabStop = False
        Me.GroupeBoxCursorEnd.Text = "Курсор конца"
        '
        'PeriodVal
        '
        Me.PeriodVal.AcceptsReturn = True
        Me.PeriodVal.BackColor = System.Drawing.SystemColors.Window
        Me.PeriodVal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PeriodVal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.PeriodVal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.PeriodVal.Location = New System.Drawing.Point(72, 12)
        Me.PeriodVal.MaxLength = 0
        Me.PeriodVal.Name = "PeriodVal"
        Me.PeriodVal.ReadOnly = True
        Me.PeriodVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PeriodVal.Size = New System.Drawing.Size(41, 20)
        Me.PeriodVal.TabIndex = 15
        Me.PeriodVal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PeriodVal, "Время между первым и вторым курсором")
        '
        'AmplitudeVal
        '
        Me.AmplitudeVal.AcceptsReturn = True
        Me.AmplitudeVal.BackColor = System.Drawing.SystemColors.Window
        Me.AmplitudeVal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.AmplitudeVal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.AmplitudeVal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.AmplitudeVal.Location = New System.Drawing.Point(72, 32)
        Me.AmplitudeVal.MaxLength = 0
        Me.AmplitudeVal.Name = "AmplitudeVal"
        Me.AmplitudeVal.ReadOnly = True
        Me.AmplitudeVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AmplitudeVal.Size = New System.Drawing.Size(41, 20)
        Me.AmplitudeVal.TabIndex = 14
        Me.AmplitudeVal.TabStop = False
        Me.ToolTip1.SetToolTip(Me.AmplitudeVal, "Амплитуда между первым и вторым курсором")
        '
        'ButtonСursorEndMoveForward
        '
        Me.ButtonСursorEndMoveForward.BackColor = System.Drawing.Color.Aqua
        Me.ButtonСursorEndMoveForward.Image = CType(resources.GetObject("ButtonСursorEndMoveForward.Image"), System.Drawing.Image)
        Me.ButtonСursorEndMoveForward.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonСursorEndMoveForward.Location = New System.Drawing.Point(60, 55)
        Me.ButtonСursorEndMoveForward.Name = "ButtonСursorEndMoveForward"
        Me.ButtonСursorEndMoveForward.Size = New System.Drawing.Size(53, 29)
        Me.ButtonСursorEndMoveForward.TabIndex = 118
        Me.ButtonСursorEndMoveForward.Text = "Вперед"
        Me.ToolTip1.SetToolTip(Me.ButtonСursorEndMoveForward, "Переместить второй курсоор вперёд")
        Me.ButtonСursorEndMoveForward.UseVisualStyleBackColor = False
        '
        'ButtonСursorEndMoveBack
        '
        Me.ButtonСursorEndMoveBack.BackColor = System.Drawing.Color.Aqua
        Me.ButtonСursorEndMoveBack.Image = CType(resources.GetObject("ButtonСursorEndMoveBack.Image"), System.Drawing.Image)
        Me.ButtonСursorEndMoveBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonСursorEndMoveBack.Location = New System.Drawing.Point(3, 55)
        Me.ButtonСursorEndMoveBack.Name = "ButtonСursorEndMoveBack"
        Me.ButtonСursorEndMoveBack.Size = New System.Drawing.Size(54, 29)
        Me.ButtonСursorEndMoveBack.TabIndex = 117
        Me.ButtonСursorEndMoveBack.Text = "Назад"
        Me.ToolTip1.SetToolTip(Me.ButtonСursorEndMoveBack, "Переместить второй курсоор назад")
        Me.ButtonСursorEndMoveBack.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(4, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(74, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Амплитуда:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(4, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(74, 17)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Время:"
        '
        'MenuStripShap
        '
        Me.MenuStripShap.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStripShap.ImageScalingSize = New System.Drawing.Size(18, 18)
        Me.MenuStripShap.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuFile, Me.MenuModeOfOperation, Me.MenuDecoding, Me.MenuExecute, Me.MenuTune, Me.TSMenuNewWindows, Me.TSMenuWindow, Me.MenuHelp})
        Me.MenuStripShap.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripShap.MdiWindowListItem = Me.TSMenuWindow
        Me.MenuStripShap.Name = "MenuStripShap"
        Me.MenuStripShap.ShowItemToolTips = True
        Me.MenuStripShap.Size = New System.Drawing.Size(1016, 24)
        Me.MenuStripShap.TabIndex = 30
        Me.MenuStripShap.Text = "&Окно"
        '
        'MenuFile
        '
        Me.MenuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOpenBaseSnapshot, Me.MenuFindSnapshot, Me.MenuRecordGraph, Me.MenuPrintGraph, Me.MenuExportSnapshotInExcel, Me.MenuChangingBase, Me.toolStripSeparator5, Me.MenuExit})
        Me.MenuFile.Name = "MenuFile"
        Me.MenuFile.Size = New System.Drawing.Size(63, 20)
        Me.MenuFile.Text = "&Снимок"
        '
        'MenuOpenBaseSnapshot
        '
        Me.MenuOpenBaseSnapshot.Image = CType(resources.GetObject("MenuOpenBaseSnapshot.Image"), System.Drawing.Image)
        Me.MenuOpenBaseSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MenuOpenBaseSnapshot.Name = "MenuOpenBaseSnapshot"
        Me.MenuOpenBaseSnapshot.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.MenuOpenBaseSnapshot.Size = New System.Drawing.Size(289, 22)
        Me.MenuOpenBaseSnapshot.Text = "&Открыть снимки текущей базы"
        Me.MenuOpenBaseSnapshot.ToolTipText = "Открыть снимки текущей базы"
        '
        'MenuFindSnapshot
        '
        Me.MenuFindSnapshot.Enabled = False
        Me.MenuFindSnapshot.Image = CType(resources.GetObject("MenuFindSnapshot.Image"), System.Drawing.Image)
        Me.MenuFindSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MenuFindSnapshot.Name = "MenuFindSnapshot"
        Me.MenuFindSnapshot.Size = New System.Drawing.Size(289, 22)
        Me.MenuFindSnapshot.Text = "&Найти кадры по условию"
        Me.MenuFindSnapshot.ToolTipText = "Найти кадры по фильтру условия"
        '
        'MenuRecordGraph
        '
        Me.MenuRecordGraph.Image = CType(resources.GetObject("MenuRecordGraph.Image"), System.Drawing.Image)
        Me.MenuRecordGraph.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MenuRecordGraph.Name = "MenuRecordGraph"
        Me.MenuRecordGraph.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.MenuRecordGraph.Size = New System.Drawing.Size(289, 22)
        Me.MenuRecordGraph.Text = "Записать &график"
        Me.MenuRecordGraph.ToolTipText = "Записать график в файл изображения"
        '
        'MenuPrintGraph
        '
        Me.MenuPrintGraph.Image = CType(resources.GetObject("MenuPrintGraph.Image"), System.Drawing.Image)
        Me.MenuPrintGraph.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MenuPrintGraph.Name = "MenuPrintGraph"
        Me.MenuPrintGraph.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.MenuPrintGraph.Size = New System.Drawing.Size(289, 22)
        Me.MenuPrintGraph.Text = "Пе&чать графика"
        Me.MenuPrintGraph.ToolTipText = "Печать графика"
        '
        'MenuExportSnapshotInExcel
        '
        Me.MenuExportSnapshotInExcel.Image = CType(resources.GetObject("MenuExportSnapshotInExcel.Image"), System.Drawing.Image)
        Me.MenuExportSnapshotInExcel.Name = "MenuExportSnapshotInExcel"
        Me.MenuExportSnapshotInExcel.Size = New System.Drawing.Size(289, 22)
        Me.MenuExportSnapshotInExcel.Text = "&Экспорт данных снимка в Excel"
        Me.MenuExportSnapshotInExcel.ToolTipText = "Экспорт исходных данных снимка в Excel"
        '
        'MenuChangingBase
        '
        Me.MenuChangingBase.Enabled = False
        Me.MenuChangingBase.Image = CType(resources.GetObject("MenuChangingBase.Image"), System.Drawing.Image)
        Me.MenuChangingBase.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MenuChangingBase.Name = "MenuChangingBase"
        Me.MenuChangingBase.Size = New System.Drawing.Size(289, 22)
        Me.MenuChangingBase.Text = "&Сменить текущую базу"
        Me.MenuChangingBase.ToolTipText = "Сменить текущую базу с испытаниями"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(286, 6)
        '
        'MenuExit
        '
        Me.MenuExit.Image = CType(resources.GetObject("MenuExit.Image"), System.Drawing.Image)
        Me.MenuExit.Name = "MenuExit"
        Me.MenuExit.Size = New System.Drawing.Size(289, 22)
        Me.MenuExit.Text = "&Закрыть окно"
        Me.MenuExit.ToolTipText = "Закрыть окно"
        '
        'MenuModeOfOperation
        '
        Me.MenuModeOfOperation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuRegistration, Me.MenuDebugging, Me.ToolStripSeparator2})
        Me.MenuModeOfOperation.Name = "MenuModeOfOperation"
        Me.MenuModeOfOperation.Size = New System.Drawing.Size(57, 20)
        Me.MenuModeOfOperation.Text = "&Режим"
        '
        'MenuRegistration
        '
        Me.MenuRegistration.Image = CType(resources.GetObject("MenuRegistration.Image"), System.Drawing.Image)
        Me.MenuRegistration.Name = "MenuRegistration"
        Me.MenuRegistration.Size = New System.Drawing.Size(144, 22)
        Me.MenuRegistration.Text = "Регистратор"
        Me.MenuRegistration.ToolTipText = "Режим регистрации каналов измерения"
        '
        'MenuDebugging
        '
        Me.MenuDebugging.Image = CType(resources.GetObject("MenuDebugging.Image"), System.Drawing.Image)
        Me.MenuDebugging.Name = "MenuDebugging"
        Me.MenuDebugging.Size = New System.Drawing.Size(144, 22)
        Me.MenuDebugging.Text = "Отладочный"
        Me.MenuDebugging.ToolTipText = "Отладочный режим записи выборочных каналов"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(141, 6)
        '
        'MenuDecoding
        '
        Me.MenuDecoding.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuProtocol, Me.MenuComeBackToBeginning, Me.ToolStripSeparator4, Me.MenuWriteDecodingSnapshotToDBase, Me.MenuWriteDecodingSnapshotToExcel, Me.MenuPrintDecodingSnapshot, Me.ToolStripSeparator14, Me.MenuXRange, Me.MenuYRange, Me.MenuInteractionModes})
        Me.MenuDecoding.Name = "MenuDecoding"
        Me.MenuDecoding.Size = New System.Drawing.Size(102, 20)
        Me.MenuDecoding.Text = "Рас&шифровать"
        '
        'MenuProtocol
        '
        Me.MenuProtocol.Image = CType(resources.GetObject("MenuProtocol.Image"), System.Drawing.Image)
        Me.MenuProtocol.Name = "MenuProtocol"
        Me.MenuProtocol.Size = New System.Drawing.Size(327, 22)
        Me.MenuProtocol.Text = "Заполнить п&ротокол"
        Me.MenuProtocol.ToolTipText = "Редактировать протокол расшифровки"
        '
        'MenuComeBackToBeginning
        '
        Me.MenuComeBackToBeginning.Image = CType(resources.GetObject("MenuComeBackToBeginning.Image"), System.Drawing.Image)
        Me.MenuComeBackToBeginning.Name = "MenuComeBackToBeginning"
        Me.MenuComeBackToBeginning.Size = New System.Drawing.Size(327, 22)
        Me.MenuComeBackToBeginning.Text = "Стереть и &вернуться к началу"
        Me.MenuComeBackToBeginning.ToolTipText = "Удалить все маркеры расшифровки"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(324, 6)
        '
        'MenuWriteDecodingSnapshotToDBase
        '
        Me.MenuWriteDecodingSnapshotToDBase.Image = CType(resources.GetObject("MenuWriteDecodingSnapshotToDBase.Image"), System.Drawing.Image)
        Me.MenuWriteDecodingSnapshotToDBase.Name = "MenuWriteDecodingSnapshotToDBase"
        Me.MenuWriteDecodingSnapshotToDBase.Size = New System.Drawing.Size(327, 22)
        Me.MenuWriteDecodingSnapshotToDBase.Text = "&Записать расшифровку снимка в базе"
        Me.MenuWriteDecodingSnapshotToDBase.ToolTipText = "Записать расшифровку снимка в базе со всеми маркерами"
        '
        'MenuWriteDecodingSnapshotToExcel
        '
        Me.MenuWriteDecodingSnapshotToExcel.Image = CType(resources.GetObject("MenuWriteDecodingSnapshotToExcel.Image"), System.Drawing.Image)
        Me.MenuWriteDecodingSnapshotToExcel.Name = "MenuWriteDecodingSnapshotToExcel"
        Me.MenuWriteDecodingSnapshotToExcel.Size = New System.Drawing.Size(327, 22)
        Me.MenuWriteDecodingSnapshotToExcel.Text = "Записать расшифровку снимка в &Excel"
        Me.MenuWriteDecodingSnapshotToExcel.ToolTipText = "Записать расшифровку снимка в Excel со всеми маркерами"
        '
        'MenuPrintDecodingSnapshot
        '
        Me.MenuPrintDecodingSnapshot.Image = CType(resources.GetObject("MenuPrintDecodingSnapshot.Image"), System.Drawing.Image)
        Me.MenuPrintDecodingSnapshot.Name = "MenuPrintDecodingSnapshot"
        Me.MenuPrintDecodingSnapshot.Size = New System.Drawing.Size(327, 22)
        Me.MenuPrintDecodingSnapshot.Text = "&Печать протокола расшифрованного снимка"
        Me.MenuPrintDecodingSnapshot.ToolTipText = "Печать протокола расшифрованного снимка"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(324, 6)
        '
        'MenuXRange
        '
        Me.MenuXRange.Name = "MenuXRange"
        Me.MenuXRange.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.MenuXRange.Size = New System.Drawing.Size(327, 22)
        Me.MenuXRange.Tag = "Helper"
        Me.MenuXRange.Text = "Задать X Диапазон..."
        Me.MenuXRange.ToolTipText = "Задать X диапазон минимума и максимума оси"
        '
        'MenuYRange
        '
        Me.MenuYRange.Name = "MenuYRange"
        Me.MenuYRange.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.MenuYRange.Size = New System.Drawing.Size(327, 22)
        Me.MenuYRange.Tag = "Helper"
        Me.MenuYRange.Text = "Задать Y Диапазон..."
        Me.MenuYRange.ToolTipText = "Задать Y диапазон минимума и максимума оси"
        '
        'MenuInteractionModes
        '
        Me.MenuInteractionModes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuDragCursor, Me.MenuPanX, Me.MenuPanY, Me.MenuZoomPoint, Me.MenuZoomX, Me.MenuZoomY})
        Me.MenuInteractionModes.Name = "MenuInteractionModes"
        Me.MenuInteractionModes.Size = New System.Drawing.Size(327, 22)
        Me.MenuInteractionModes.Tag = "Helper"
        Me.MenuInteractionModes.Text = "Режим взаимодействия с графиком"
        Me.MenuInteractionModes.ToolTipText = "Изменить режим взаимодействия с графиком."
        '
        'MenuDragCursor
        '
        Me.MenuDragCursor.Name = "MenuDragCursor"
        Me.MenuDragCursor.Size = New System.Drawing.Size(203, 22)
        Me.MenuDragCursor.Tag = "Helper"
        Me.MenuDragCursor.Text = "Перемещать Курсор"
        Me.MenuDragCursor.ToolTipText = "Перемещать Курсор."
        '
        'MenuPanX
        '
        Me.MenuPanX.Name = "MenuPanX"
        Me.MenuPanX.Size = New System.Drawing.Size(203, 22)
        Me.MenuPanX.Tag = "Helper"
        Me.MenuPanX.Text = "Панорама X"
        Me.MenuPanX.ToolTipText = "Панорама в направлении X."
        '
        'MenuPanY
        '
        Me.MenuPanY.Checked = True
        Me.MenuPanY.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuPanY.Name = "MenuPanY"
        Me.MenuPanY.Size = New System.Drawing.Size(203, 22)
        Me.MenuPanY.Tag = "Helper"
        Me.MenuPanY.Text = "Панарома Y"
        Me.MenuPanY.ToolTipText = "Панарома в направлении Y."
        '
        'MenuZoomPoint
        '
        Me.MenuZoomPoint.Checked = True
        Me.MenuZoomPoint.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuZoomPoint.Name = "MenuZoomPoint"
        Me.MenuZoomPoint.Size = New System.Drawing.Size(203, 22)
        Me.MenuZoomPoint.Tag = "Helper"
        Me.MenuZoomPoint.Text = "Масштаб Вокруг Точки"
        Me.MenuZoomPoint.ToolTipText = "Масштаб Вокруг Точки."
        '
        'MenuZoomX
        '
        Me.MenuZoomX.Name = "MenuZoomX"
        Me.MenuZoomX.Size = New System.Drawing.Size(203, 22)
        Me.MenuZoomX.Tag = "Helper"
        Me.MenuZoomX.Text = "Масштаб X"
        Me.MenuZoomX.ToolTipText = "Масштаб в направлении X."
        '
        'MenuZoomY
        '
        Me.MenuZoomY.Checked = True
        Me.MenuZoomY.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuZoomY.Name = "MenuZoomY"
        Me.MenuZoomY.Size = New System.Drawing.Size(203, 22)
        Me.MenuZoomY.Tag = "Helper"
        Me.MenuZoomY.Text = "Масштаб Y"
        Me.MenuZoomY.ToolTipText = "Масштаб в направлении Y."
        '
        'MenuExecute
        '
        Me.MenuExecute.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuGraphCheckedList, Me.MenuShowTextControl, Me.MenuShowGraphControl, Me.ToolStripSeparator9, Me.MenuServerEnable, Me.MenuReceiveFromServer, Me.MenuSimulator, Me.MenuCommandClientServer, Me.ToolStripSeparator10, Me.MenuGraphAlongTime, Me.MenuGraphAlongParameters, Me.ToolStripSeparator11, Me.MenuParameterInRange, Me.MenuPens, Me.MenuQuestioningParameter})
        Me.MenuExecute.Name = "MenuExecute"
        Me.MenuExecute.Size = New System.Drawing.Size(81, 20)
        Me.MenuExecute.Text = "&Выполнить"
        '
        'MenuGraphCheckedList
        '
        Me.MenuGraphCheckedList.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuEditGraphOfParameter, Me.ToolStripBar})
        Me.MenuGraphCheckedList.Image = CType(resources.GetObject("MenuGraphCheckedList.Image"), System.Drawing.Image)
        Me.MenuGraphCheckedList.Name = "MenuGraphCheckedList"
        Me.MenuGraphCheckedList.Size = New System.Drawing.Size(211, 24)
        Me.MenuGraphCheckedList.Text = "&Графики от параметров"
        Me.MenuGraphCheckedList.ToolTipText = "Настройка графиков от параметра"
        '
        'MenuEditGraphOfParameter
        '
        Me.MenuEditGraphOfParameter.Image = Global.Registration.My.Resources.Resources.kiconedit
        Me.MenuEditGraphOfParameter.Name = "MenuEditGraphOfParameter"
        Me.MenuEditGraphOfParameter.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.MenuEditGraphOfParameter.Size = New System.Drawing.Size(305, 22)
        Me.MenuEditGraphOfParameter.Text = "Редактор графиков от параметров"
        Me.MenuEditGraphOfParameter.ToolTipText = "Загрузка редактора графиков от параметров"
        '
        'ToolStripBar
        '
        Me.ToolStripBar.Name = "ToolStripBar"
        Me.ToolStripBar.Size = New System.Drawing.Size(302, 6)
        '
        'MenuShowTextControl
        '
        Me.MenuShowTextControl.Enabled = False
        Me.MenuShowTextControl.Image = CType(resources.GetObject("MenuShowTextControl.Image"), System.Drawing.Image)
        Me.MenuShowTextControl.Name = "MenuShowTextControl"
        Me.MenuShowTextControl.Size = New System.Drawing.Size(211, 24)
        Me.MenuShowTextControl.Text = "Те&кстовый контроль"
        Me.MenuShowTextControl.ToolTipText = "Показать окно с цифровыми значениями выбранных параметров"
        '
        'MenuShowGraphControl
        '
        Me.MenuShowGraphControl.Image = CType(resources.GetObject("MenuShowGraphControl.Image"), System.Drawing.Image)
        Me.MenuShowGraphControl.Name = "MenuShowGraphControl"
        Me.MenuShowGraphControl.Size = New System.Drawing.Size(211, 24)
        Me.MenuShowGraphControl.Text = "Гра&фический контроль"
        Me.MenuShowGraphControl.ToolTipText = "Показать окно с графическим представлением выбранных параметров"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(208, 6)
        '
        'MenuServerEnable
        '
        Me.MenuServerEnable.Image = CType(resources.GetObject("MenuServerEnable.Image"), System.Drawing.Image)
        Me.MenuServerEnable.Name = "MenuServerEnable"
        Me.MenuServerEnable.Size = New System.Drawing.Size(211, 24)
        Me.MenuServerEnable.Text = "&Данные -> в сеть"
        Me.MenuServerEnable.ToolTipText = "Передать значения параметров в сеть"
        '
        'MenuReceiveFromServer
        '
        Me.MenuReceiveFromServer.Enabled = False
        Me.MenuReceiveFromServer.Image = CType(resources.GetObject("MenuReceiveFromServer.Image"), System.Drawing.Image)
        Me.MenuReceiveFromServer.Name = "MenuReceiveFromServer"
        Me.MenuReceiveFromServer.Size = New System.Drawing.Size(211, 24)
        Me.MenuReceiveFromServer.Text = "Прием от &сервера"
        Me.MenuReceiveFromServer.ToolTipText = "Показать соединение с Сервером"
        '
        'MenuSimulator
        '
        Me.MenuSimulator.CheckOnClick = True
        Me.MenuSimulator.Enabled = False
        Me.MenuSimulator.Image = CType(resources.GetObject("MenuSimulator.Image"), System.Drawing.Image)
        Me.MenuSimulator.Name = "MenuSimulator"
        Me.MenuSimulator.Size = New System.Drawing.Size(211, 24)
        Me.MenuSimulator.Text = "&Имитатор"
        Me.MenuSimulator.ToolTipText = "Запустить режим имитации измерения по записям"
        '
        'MenuCommandClientServer
        '
        Me.MenuCommandClientServer.CheckOnClick = True
        Me.MenuCommandClientServer.Enabled = False
        Me.MenuCommandClientServer.Image = CType(resources.GetObject("MenuCommandClientServer.Image"), System.Drawing.Image)
        Me.MenuCommandClientServer.Name = "MenuCommandClientServer"
        Me.MenuCommandClientServer.Size = New System.Drawing.Size(211, 24)
        Me.MenuCommandClientServer.Text = "Показать сетевой обмен"
        Me.MenuCommandClientServer.ToolTipText = "Показать панель сетевого командного обмена сообщениями"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(208, 6)
        '
        'MenuGraphAlongTime
        '
        Me.MenuGraphAlongTime.Checked = True
        Me.MenuGraphAlongTime.CheckOnClick = True
        Me.MenuGraphAlongTime.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuGraphAlongTime.Name = "MenuGraphAlongTime"
        Me.MenuGraphAlongTime.Size = New System.Drawing.Size(211, 24)
        Me.MenuGraphAlongTime.Text = "График по &времени"
        Me.MenuGraphAlongTime.ToolTipText = "Показать панель сбора параметров по оси времени"
        '
        'MenuGraphAlongParameters
        '
        Me.MenuGraphAlongParameters.Checked = True
        Me.MenuGraphAlongParameters.CheckOnClick = True
        Me.MenuGraphAlongParameters.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuGraphAlongParameters.Name = "MenuGraphAlongParameters"
        Me.MenuGraphAlongParameters.Size = New System.Drawing.Size(211, 24)
        Me.MenuGraphAlongParameters.Text = "График по па&раметру"
        Me.MenuGraphAlongParameters.ToolTipText = "Показать панель сбора параметров по оси другого параметра"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(208, 6)
        '
        'MenuParameterInRange
        '
        Me.MenuParameterInRange.Enabled = False
        Me.MenuParameterInRange.Image = CType(resources.GetObject("MenuParameterInRange.Image"), System.Drawing.Image)
        Me.MenuParameterInRange.Name = "MenuParameterInRange"
        Me.MenuParameterInRange.Size = New System.Drawing.Size(211, 24)
        Me.MenuParameterInRange.Text = "П&араметр в диапазоне"
        Me.MenuParameterInRange.ToolTipText = "Показать панель нахождения значения канала в заданном диапазоне напряжения"
        '
        'MenuPens
        '
        Me.MenuPens.CheckOnClick = True
        Me.MenuPens.Image = CType(resources.GetObject("MenuPens.Image"), System.Drawing.Image)
        Me.MenuPens.Name = "MenuPens"
        Me.MenuPens.Size = New System.Drawing.Size(211, 24)
        Me.MenuPens.Text = "&Перья"
        Me.MenuPens.ToolTipText = "Включение/отключение маркера рядом с шлейфом параметра"
        '
        'MenuQuestioningParameter
        '
        Me.MenuQuestioningParameter.Image = CType(resources.GetObject("MenuQuestioningParameter.Image"), System.Drawing.Image)
        Me.MenuQuestioningParameter.Name = "MenuQuestioningParameter"
        Me.MenuQuestioningParameter.Size = New System.Drawing.Size(211, 24)
        Me.MenuQuestioningParameter.Text = "&Опрос параметра"
        Me.MenuQuestioningParameter.ToolTipText = "Паказать панель опроса канала измерения"
        '
        'MenuTune
        '
        Me.MenuTune.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuConfigurationChannels, Me.MenuVisibilityTrend, Me.ToolStripSeparator3, Me.MenuSelectiveList, Me.MenuSelectiveTextControl, Me.MenuSelectiveGraphControl, Me.ToolStripSeparator12, Me.MenuShowSetting})
        Me.MenuTune.Name = "MenuTune"
        Me.MenuTune.Size = New System.Drawing.Size(77, 20)
        Me.MenuTune.Text = "Нас&троить"
        '
        'MenuConfigurationChannels
        '
        Me.MenuConfigurationChannels.Image = CType(resources.GetObject("MenuConfigurationChannels.Image"), System.Drawing.Image)
        Me.MenuConfigurationChannels.Name = "MenuConfigurationChannels"
        Me.MenuConfigurationChannels.Size = New System.Drawing.Size(205, 24)
        Me.MenuConfigurationChannels.Text = "&Конфигурация каналов"
        Me.MenuConfigurationChannels.ToolTipText = "Показать окно выбора каналов для регистрации их значений"
        '
        'MenuVisibilityTrend
        '
        Me.MenuVisibilityTrend.Image = CType(resources.GetObject("MenuVisibilityTrend.Image"), System.Drawing.Image)
        Me.MenuVisibilityTrend.Name = "MenuVisibilityTrend"
        Me.MenuVisibilityTrend.Size = New System.Drawing.Size(205, 24)
        Me.MenuVisibilityTrend.Text = "Видимость &шлейфов"
        Me.MenuVisibilityTrend.ToolTipText = "Показать редактор отбора параметров для отображения их шлейфов на графике"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(202, 6)
        '
        'MenuSelectiveList
        '
        Me.MenuSelectiveList.Image = CType(resources.GetObject("MenuSelectiveList.Image"), System.Drawing.Image)
        Me.MenuSelectiveList.Name = "MenuSelectiveList"
        Me.MenuSelectiveList.Size = New System.Drawing.Size(205, 24)
        Me.MenuSelectiveList.Text = "Выборочный &список"
        Me.MenuSelectiveList.ToolTipText = "Показать редактор отбора параметров для выборочного списка"
        '
        'MenuSelectiveTextControl
        '
        Me.MenuSelectiveTextControl.Image = CType(resources.GetObject("MenuSelectiveTextControl.Image"), System.Drawing.Image)
        Me.MenuSelectiveTextControl.Name = "MenuSelectiveTextControl"
        Me.MenuSelectiveTextControl.Size = New System.Drawing.Size(205, 24)
        Me.MenuSelectiveTextControl.Text = "&Текстовый контроль"
        Me.MenuSelectiveTextControl.ToolTipText = "Показать редактор отбора параметров для отображения значений в текстовом виде"
        '
        'MenuSelectiveGraphControl
        '
        Me.MenuSelectiveGraphControl.Image = CType(resources.GetObject("MenuSelectiveGraphControl.Image"), System.Drawing.Image)
        Me.MenuSelectiveGraphControl.Name = "MenuSelectiveGraphControl"
        Me.MenuSelectiveGraphControl.Size = New System.Drawing.Size(205, 24)
        Me.MenuSelectiveGraphControl.Text = "&Графический контроль"
        Me.MenuSelectiveGraphControl.ToolTipText = "Показать редактор отбора параметров для отображения их графического представления" &
    ""
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(202, 6)
        '
        'MenuShowSetting
        '
        Me.MenuShowSetting.Image = CType(resources.GetObject("MenuShowSetting.Image"), System.Drawing.Image)
        Me.MenuShowSetting.Name = "MenuShowSetting"
        Me.MenuShowSetting.Size = New System.Drawing.Size(205, 24)
        Me.MenuShowSetting.Text = "&Опции"
        Me.MenuShowSetting.ToolTipText = "Показать панель настроек приложения при запуске программы"
        '
        'TSMenuNewWindows
        '
        Me.TSMenuNewWindows.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuNewWindowRegistration, Me.MenuNewWindowSnapshot, Me.MenuNewWindowTarir, Me.MenuNewWindowClient, Me.MenuNewWindowDBaseChannels, Me.MenuNewWindowEvents})
        Me.TSMenuNewWindows.Name = "TSMenuNewWindows"
        Me.TSMenuNewWindows.Size = New System.Drawing.Size(84, 20)
        Me.TSMenuNewWindows.Text = "&Новое окно"
        Me.TSMenuNewWindows.Visible = False
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
        'TSMenuWindow
        '
        Me.TSMenuWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuWindowCascade, Me.MenuWindowTileHorizontal, Me.MenuWindowTileVertical})
        Me.TSMenuWindow.Name = "TSMenuWindow"
        Me.TSMenuWindow.Size = New System.Drawing.Size(48, 20)
        Me.TSMenuWindow.Text = "&Окно"
        Me.TSMenuWindow.Visible = False
        '
        'MenuWindowCascade
        '
        Me.MenuWindowCascade.Image = CType(resources.GetObject("MenuWindowCascade.Image"), System.Drawing.Image)
        Me.MenuWindowCascade.Name = "MenuWindowCascade"
        Me.MenuWindowCascade.Size = New System.Drawing.Size(158, 22)
        Me.MenuWindowCascade.Text = "&Каскад"
        Me.MenuWindowCascade.ToolTipText = "Расположение окон каскадом"
        '
        'MenuWindowTileHorizontal
        '
        Me.MenuWindowTileHorizontal.Image = CType(resources.GetObject("MenuWindowTileHorizontal.Image"), System.Drawing.Image)
        Me.MenuWindowTileHorizontal.Name = "MenuWindowTileHorizontal"
        Me.MenuWindowTileHorizontal.Size = New System.Drawing.Size(158, 22)
        Me.MenuWindowTileHorizontal.Text = "&Горизонтально"
        Me.MenuWindowTileHorizontal.ToolTipText = "Расположение окон сверху"
        '
        'MenuWindowTileVertical
        '
        Me.MenuWindowTileVertical.Image = CType(resources.GetObject("MenuWindowTileVertical.Image"), System.Drawing.Image)
        Me.MenuWindowTileVertical.Name = "MenuWindowTileVertical"
        Me.MenuWindowTileVertical.Size = New System.Drawing.Size(158, 22)
        Me.MenuWindowTileVertical.Text = "&Вертикально"
        Me.MenuWindowTileVertical.ToolTipText = "Расположение окон рядом"
        '
        'MenuHelp
        '
        Me.MenuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuContents, Me.MenuIndex, Me.MenuSearch, Me.MenuExplorerHelpApplication, Me.MenuHelpRegime, Me.MenuNormTU, Me.ToolStripSeparator13, Me.MenuAboutProgramm})
        Me.MenuHelp.Name = "MenuHelp"
        Me.MenuHelp.Size = New System.Drawing.Size(65, 20)
        Me.MenuHelp.Text = "&Справка"
        '
        'MenuContents
        '
        Me.MenuContents.Image = CType(resources.GetObject("MenuContents.Image"), System.Drawing.Image)
        Me.MenuContents.Name = "MenuContents"
        Me.MenuContents.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.MenuContents.Size = New System.Drawing.Size(225, 22)
        Me.MenuContents.Text = "&Содержание"
        '
        'MenuIndex
        '
        Me.MenuIndex.Image = CType(resources.GetObject("MenuIndex.Image"), System.Drawing.Image)
        Me.MenuIndex.ImageTransparentColor = System.Drawing.Color.Black
        Me.MenuIndex.Name = "MenuIndex"
        Me.MenuIndex.Size = New System.Drawing.Size(225, 22)
        Me.MenuIndex.Text = "&Указатель"
        '
        'MenuSearch
        '
        Me.MenuSearch.Image = CType(resources.GetObject("MenuSearch.Image"), System.Drawing.Image)
        Me.MenuSearch.ImageTransparentColor = System.Drawing.Color.Black
        Me.MenuSearch.Name = "MenuSearch"
        Me.MenuSearch.Size = New System.Drawing.Size(225, 22)
        Me.MenuSearch.Text = "&Поиск"
        '
        'MenuExplorerHelpApplication
        '
        Me.MenuExplorerHelpApplication.Image = CType(resources.GetObject("MenuExplorerHelpApplication.Image"), System.Drawing.Image)
        Me.MenuExplorerHelpApplication.Name = "MenuExplorerHelpApplication"
        Me.MenuExplorerHelpApplication.Size = New System.Drawing.Size(225, 22)
        Me.MenuExplorerHelpApplication.Text = "Спра&вочник по программе"
        '
        'MenuHelpRegime
        '
        Me.MenuHelpRegime.Image = CType(resources.GetObject("MenuHelpRegime.Image"), System.Drawing.Image)
        Me.MenuHelpRegime.Name = "MenuHelpRegime"
        Me.MenuHelpRegime.Size = New System.Drawing.Size(225, 22)
        Me.MenuHelpRegime.Text = "Справка по &режиму"
        '
        'MenuNormTU
        '
        Me.MenuNormTU.Enabled = False
        Me.MenuNormTU.Image = CType(resources.GetObject("MenuNormTU.Image"), System.Drawing.Image)
        Me.MenuNormTU.Name = "MenuNormTU"
        Me.MenuNormTU.Size = New System.Drawing.Size(225, 22)
        Me.MenuNormTU.Text = "&Нормы ТУ"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(222, 6)
        '
        'MenuAboutProgramm
        '
        Me.MenuAboutProgramm.Image = CType(resources.GetObject("MenuAboutProgramm.Image"), System.Drawing.Image)
        Me.MenuAboutProgramm.Name = "MenuAboutProgramm"
        Me.MenuAboutProgramm.Size = New System.Drawing.Size(225, 22)
        Me.MenuAboutProgramm.Text = "О &программе"
        '
        'ToolStripMain
        '
        Me.ToolStripMain.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ButtonContinuously, Me.ToolStripSeparator1, Me.ButtonRecord, Me.TextBoxRecordToDisc, Me.ToolStripSeparator6, Me.ButtonGraphTime, Me.ButtonGraphParameter, Me.ToolStripSeparator7, Me.ButtonSnapshot, Me.LabelSec, Me.ComboBoxTimeMeasurement, Me.ToolStripSeparator8, Me.ButtonDetailSelective, Me.ButtonTuneTrand})
        Me.ToolStripMain.Location = New System.Drawing.Point(3, 24)
        Me.ToolStripMain.Name = "ToolStripMain"
        Me.ToolStripMain.Size = New System.Drawing.Size(883, 57)
        Me.ToolStripMain.TabIndex = 31
        '
        'ButtonContinuously
        '
        Me.ButtonContinuously.AutoSize = False
        Me.ButtonContinuously.CheckOnClick = True
        Me.ButtonContinuously.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonContinuously.Image = CType(resources.GetObject("ButtonContinuously.Image"), System.Drawing.Image)
        Me.ButtonContinuously.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonContinuously.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonContinuously.Name = "ButtonContinuously"
        Me.ButtonContinuously.Size = New System.Drawing.Size(100, 54)
        Me.ButtonContinuously.Tag = "Непр"
        Me.ButtonContinuously.Text = "Пуск"
        Me.ButtonContinuously.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonContinuously.ToolTipText = "Начать сбор"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 57)
        '
        'ButtonRecord
        '
        Me.ButtonRecord.AutoSize = False
        Me.ButtonRecord.CheckOnClick = True
        Me.ButtonRecord.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRecord.Image = CType(resources.GetObject("ButtonRecord.Image"), System.Drawing.Image)
        Me.ButtonRecord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonRecord.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonRecord.Name = "ButtonRecord"
        Me.ButtonRecord.Size = New System.Drawing.Size(100, 54)
        Me.ButtonRecord.Tag = "Запись"
        Me.ButtonRecord.Text = "Запись"
        Me.ButtonRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonRecord.ToolTipText = "Запись испытания"
        '
        'TextBoxRecordToDisc
        '
        Me.TextBoxRecordToDisc.BackColor = System.Drawing.Color.Red
        Me.TextBoxRecordToDisc.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxRecordToDisc.ForeColor = System.Drawing.Color.Yellow
        Me.TextBoxRecordToDisc.Name = "TextBoxRecordToDisc"
        Me.TextBoxRecordToDisc.ReadOnly = True
        Me.TextBoxRecordToDisc.Size = New System.Drawing.Size(100, 57)
        Me.TextBoxRecordToDisc.Text = "Запись"
        Me.TextBoxRecordToDisc.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TextBoxRecordToDisc.Visible = False
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 57)
        '
        'ButtonGraphTime
        '
        Me.ButtonGraphTime.AutoSize = False
        Me.ButtonGraphTime.Checked = True
        Me.ButtonGraphTime.CheckOnClick = True
        Me.ButtonGraphTime.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ButtonGraphTime.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonGraphTime.Image = CType(resources.GetObject("ButtonGraphTime.Image"), System.Drawing.Image)
        Me.ButtonGraphTime.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonGraphTime.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonGraphTime.Name = "ButtonGraphTime"
        Me.ButtonGraphTime.Size = New System.Drawing.Size(100, 54)
        Me.ButtonGraphTime.Tag = "Graf1"
        Me.ButtonGraphTime.Text = "Время"
        Me.ButtonGraphTime.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonGraphTime.ToolTipText = "График по времени"
        '
        'ButtonGraphParameter
        '
        Me.ButtonGraphParameter.AutoSize = False
        Me.ButtonGraphParameter.Checked = True
        Me.ButtonGraphParameter.CheckOnClick = True
        Me.ButtonGraphParameter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ButtonGraphParameter.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonGraphParameter.Image = CType(resources.GetObject("ButtonGraphParameter.Image"), System.Drawing.Image)
        Me.ButtonGraphParameter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonGraphParameter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonGraphParameter.Name = "ButtonGraphParameter"
        Me.ButtonGraphParameter.Size = New System.Drawing.Size(100, 54)
        Me.ButtonGraphParameter.Tag = "Graf2"
        Me.ButtonGraphParameter.Text = "Параметр"
        Me.ButtonGraphParameter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonGraphParameter.ToolTipText = "График по параметру"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 57)
        '
        'ButtonSnapshot
        '
        Me.ButtonSnapshot.AutoSize = False
        Me.ButtonSnapshot.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSnapshot.Image = CType(resources.GetObject("ButtonSnapshot.Image"), System.Drawing.Image)
        Me.ButtonSnapshot.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonSnapshot.Name = "ButtonSnapshot"
        Me.ButtonSnapshot.Size = New System.Drawing.Size(100, 54)
        Me.ButtonSnapshot.Tag = "New"
        Me.ButtonSnapshot.Text = "Снимок"
        Me.ButtonSnapshot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonSnapshot.ToolTipText = "Снимок 200 Гц"
        '
        'LabelSec
        '
        Me.LabelSec.Name = "LabelSec"
        Me.LabelSec.Size = New System.Drawing.Size(70, 54)
        Me.LabelSec.Text = "снимок сек"
        '
        'ComboBoxTimeMeasurement
        '
        Me.ComboBoxTimeMeasurement.Name = "ComboBoxTimeMeasurement"
        Me.ComboBoxTimeMeasurement.Size = New System.Drawing.Size(75, 57)
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 57)
        '
        'ButtonDetailSelective
        '
        Me.ButtonDetailSelective.AutoSize = False
        Me.ButtonDetailSelective.CheckOnClick = True
        Me.ButtonDetailSelective.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDetailSelective.Image = CType(resources.GetObject("ButtonDetailSelective.Image"), System.Drawing.Image)
        Me.ButtonDetailSelective.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonDetailSelective.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonDetailSelective.Name = "ButtonDetailSelective"
        Me.ButtonDetailSelective.Size = New System.Drawing.Size(100, 54)
        Me.ButtonDetailSelective.Tag = "Подробно"
        Me.ButtonDetailSelective.Text = "Подробно"
        Me.ButtonDetailSelective.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonDetailSelective.ToolTipText = "Подробный или выборочный список"
        '
        'ButtonTuneTrand
        '
        Me.ButtonTuneTrand.AutoSize = False
        Me.ButtonTuneTrand.CheckOnClick = True
        Me.ButtonTuneTrand.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonTuneTrand.Image = CType(resources.GetObject("ButtonTuneTrand.Image"), System.Drawing.Image)
        Me.ButtonTuneTrand.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ButtonTuneTrand.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonTuneTrand.Name = "ButtonTuneTrand"
        Me.ButtonTuneTrand.Size = New System.Drawing.Size(100, 54)
        Me.ButtonTuneTrand.Tag = "Шлейфы"
        Me.ButtonTuneTrand.Text = "Шлейфы"
        Me.ButtonTuneTrand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonTuneTrand.ToolTipText = "Настройка осей и шлейфов для графика от параметра"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ComboBoxSelectAxis, Me.LabelIndicator, Me.ToolStripSeparator18, Me.TextBoxConnectOk, Me.TextBoxConnectBad, Me.ButtonConnectRefresh, Me.LabelMessageTcpClient, Me.TextBoxIndicatorN1physics, Me.TextBoxIndicatorN1reduce, Me.TextBoxIndicatorN2physics, Me.TextBoxIndicatorN2reduce, Me.TextBoxIndicatorRud, Me.TextBoxRegime})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 81)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(758, 26)
        Me.ToolStrip2.TabIndex = 32
        '
        'ComboBoxSelectAxis
        '
        Me.ComboBoxSelectAxis.DropDownWidth = 200
        Me.ComboBoxSelectAxis.Enabled = False
        Me.ComboBoxSelectAxis.MaxDropDownItems = 32
        Me.ComboBoxSelectAxis.Name = "ComboBoxSelectAxis"
        Me.ComboBoxSelectAxis.Size = New System.Drawing.Size(150, 26)
        Me.ComboBoxSelectAxis.ToolTipText = "Имя канала при построения Оси Y"
        '
        'LabelIndicator
        '
        Me.LabelIndicator.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelIndicator.ForeColor = System.Drawing.Color.Red
        Me.LabelIndicator.Name = "LabelIndicator"
        Me.LabelIndicator.Size = New System.Drawing.Size(38, 23)
        Me.LabelIndicator.Text = "Ось Y"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(6, 26)
        '
        'TextBoxConnectOk
        '
        Me.TextBoxConnectOk.BackColor = System.Drawing.Color.Lime
        Me.TextBoxConnectOk.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxConnectOk.Name = "TextBoxConnectOk"
        Me.TextBoxConnectOk.ReadOnly = True
        Me.TextBoxConnectOk.Size = New System.Drawing.Size(175, 28)
        Me.TextBoxConnectOk.Text = "Связь с сервером установлена"
        Me.TextBoxConnectOk.Visible = False
        '
        'TextBoxConnectBad
        '
        Me.TextBoxConnectBad.BackColor = System.Drawing.Color.Red
        Me.TextBoxConnectBad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxConnectBad.ForeColor = System.Drawing.Color.White
        Me.TextBoxConnectBad.Name = "TextBoxConnectBad"
        Me.TextBoxConnectBad.ReadOnly = True
        Me.TextBoxConnectBad.Size = New System.Drawing.Size(175, 28)
        Me.TextBoxConnectBad.Text = "Связь с сервером отсутствует"
        Me.TextBoxConnectBad.Visible = False
        '
        'ButtonConnectRefresh
        '
        Me.ButtonConnectRefresh.Image = CType(resources.GetObject("ButtonConnectRefresh.Image"), System.Drawing.Image)
        Me.ButtonConnectRefresh.ImageTransparentColor = System.Drawing.Color.Black
        Me.ButtonConnectRefresh.Name = "ButtonConnectRefresh"
        Me.ButtonConnectRefresh.Size = New System.Drawing.Size(109, 23)
        Me.ButtonConnectRefresh.Text = "Подключиться"
        Me.ButtonConnectRefresh.ToolTipText = "Обновить подключение"
        Me.ButtonConnectRefresh.Visible = False
        '
        'LabelMessageTcpClient
        '
        Me.LabelMessageTcpClient.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LabelMessageTcpClient.ForeColor = System.Drawing.Color.Blue
        Me.LabelMessageTcpClient.Name = "LabelMessageTcpClient"
        Me.LabelMessageTcpClient.Size = New System.Drawing.Size(0, 23)
        Me.LabelMessageTcpClient.ToolTipText = "Сообщения от TcpClient"
        '
        'TextBoxIndicatorN1physics
        '
        Me.TextBoxIndicatorN1physics.BackColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxIndicatorN1physics.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxIndicatorN1physics.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxIndicatorN1physics.Name = "TextBoxIndicatorN1physics"
        Me.TextBoxIndicatorN1physics.ReadOnly = True
        Me.TextBoxIndicatorN1physics.Size = New System.Drawing.Size(110, 26)
        Me.TextBoxIndicatorN1physics.Text = "N1ф=100.1%"
        '
        'TextBoxIndicatorN1reduce
        '
        Me.TextBoxIndicatorN1reduce.BackColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxIndicatorN1reduce.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxIndicatorN1reduce.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxIndicatorN1reduce.Name = "TextBoxIndicatorN1reduce"
        Me.TextBoxIndicatorN1reduce.ReadOnly = True
        Me.TextBoxIndicatorN1reduce.Size = New System.Drawing.Size(110, 26)
        Me.TextBoxIndicatorN1reduce.Text = "N1п=100.1%"
        '
        'TextBoxIndicatorN2physics
        '
        Me.TextBoxIndicatorN2physics.BackColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxIndicatorN2physics.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxIndicatorN2physics.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxIndicatorN2physics.Name = "TextBoxIndicatorN2physics"
        Me.TextBoxIndicatorN2physics.ReadOnly = True
        Me.TextBoxIndicatorN2physics.Size = New System.Drawing.Size(110, 26)
        Me.TextBoxIndicatorN2physics.Text = "N2ф=100.1%"
        '
        'TextBoxIndicatorN2reduce
        '
        Me.TextBoxIndicatorN2reduce.BackColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxIndicatorN2reduce.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxIndicatorN2reduce.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxIndicatorN2reduce.Name = "TextBoxIndicatorN2reduce"
        Me.TextBoxIndicatorN2reduce.ReadOnly = True
        Me.TextBoxIndicatorN2reduce.Size = New System.Drawing.Size(110, 26)
        Me.TextBoxIndicatorN2reduce.Text = "N2п=100.1%"
        '
        'TextBoxIndicatorRud
        '
        Me.TextBoxIndicatorRud.BackColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxIndicatorRud.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxIndicatorRud.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxIndicatorRud.Name = "TextBoxIndicatorRud"
        Me.TextBoxIndicatorRud.ReadOnly = True
        Me.TextBoxIndicatorRud.Size = New System.Drawing.Size(100, 26)
        Me.TextBoxIndicatorRud.Text = "Руд=115.0"
        '
        'TextBoxRegime
        '
        Me.TextBoxRegime.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TextBoxRegime.ForeColor = System.Drawing.Color.Blue
        Me.TextBoxRegime.Name = "TextBoxRegime"
        Me.TextBoxRegime.Size = New System.Drawing.Size(0, 23)
        '
        'StatusStripMain
        '
        Me.StatusStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LabelRegistration, Me.LabelProduct, Me.LabelDisc, Me.LabelDiscValue, Me.LabelSampleRate, Me.ProgressBarExport})
        Me.StatusStripMain.Location = New System.Drawing.Point(0, 670)
        Me.StatusStripMain.Name = "StatusStripMain"
        Me.StatusStripMain.Size = New System.Drawing.Size(1016, 25)
        Me.StatusStripMain.TabIndex = 33
        '
        'LabelRegistration
        '
        Me.LabelRegistration.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelRegistration.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.LabelRegistration.Image = CType(resources.GetObject("LabelRegistration.Image"), System.Drawing.Image)
        Me.LabelRegistration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LabelRegistration.Name = "LabelRegistration"
        Me.LabelRegistration.Size = New System.Drawing.Size(775, 20)
        Me.LabelRegistration.Spring = True
        Me.LabelRegistration.Text = "Регистратор"
        Me.LabelRegistration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelProduct
        '
        Me.LabelProduct.AutoSize = False
        Me.LabelProduct.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelProduct.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.LabelProduct.Name = "LabelProduct"
        Me.LabelProduct.Size = New System.Drawing.Size(90, 20)
        Me.LabelProduct.Text = "Установка 1"
        '
        'LabelDisc
        '
        Me.LabelDisc.AutoSize = False
        Me.LabelDisc.Name = "LabelDisc"
        Me.LabelDisc.Size = New System.Drawing.Size(36, 20)
        Me.LabelDisc.Text = "Диск"
        '
        'LabelDiscValue
        '
        Me.LabelDiscValue.AutoSize = False
        Me.LabelDiscValue.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelDiscValue.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.LabelDiscValue.Name = "LabelDiscValue"
        Me.LabelDiscValue.Size = New System.Drawing.Size(60, 20)
        '
        'LabelSampleRate
        '
        Me.LabelSampleRate.AutoSize = False
        Me.LabelSampleRate.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelSampleRate.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.LabelSampleRate.Name = "LabelSampleRate"
        Me.LabelSampleRate.Size = New System.Drawing.Size(40, 20)
        '
        'ProgressBarExport
        '
        Me.ProgressBarExport.Name = "ProgressBarExport"
        Me.ProgressBarExport.Size = New System.Drawing.Size(400, 19)
        Me.ProgressBarExport.Visible = False
        '
        'TimerButtonRun
        '
        Me.TimerButtonRun.Interval = 60000
        '
        'TimerSwitchOffRecord
        '
        Me.TimerSwitchOffRecord.Interval = 40000
        '
        'TimerCursor
        '
        Me.TimerCursor.Interval = 5000
        '
        'TimerTimeOutClient
        '
        Me.TimerTimeOutClient.Interval = 5000
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "")
        Me.ImageList2.Images.SetKeyName(5, "")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "")
        Me.ImageList2.Images.SetKeyName(10, "")
        Me.ImageList2.Images.SetKeyName(11, "")
        '
        'HelpProviderAdvancedCHM
        '
        Me.HelpProviderAdvancedCHM.HelpNamespace = "..\..\HelpRegistrationTCP.chm"
        '
        'PulseButtonTakeOffAlarm
        '
        Me.PulseButtonTakeOffAlarm.ButtonColorBottom = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.PulseButtonTakeOffAlarm.ButtonColorTop = System.Drawing.Color.Red
        Me.PulseButtonTakeOffAlarm.CornerRadius = 5
        Me.PulseButtonTakeOffAlarm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.PulseButtonTakeOffAlarm.ForeColor = System.Drawing.Color.Yellow
        Me.PulseButtonTakeOffAlarm.Location = New System.Drawing.Point(250, 250)
        Me.PulseButtonTakeOffAlarm.Name = "PulseButtonTakeOffAlarm"
        Me.PulseButtonTakeOffAlarm.NumberOfPulses = 2
        Me.PulseButtonTakeOffAlarm.PulseColor = System.Drawing.Color.Maroon
        Me.PulseButtonTakeOffAlarm.PulseSpeed = 0.6!
        Me.PulseButtonTakeOffAlarm.PulseWidth = 20
        Me.PulseButtonTakeOffAlarm.ShapeType = PulseButton.PulseButton.Shape.Rectangle
        Me.PulseButtonTakeOffAlarm.Size = New System.Drawing.Size(556, 88)
        Me.PulseButtonTakeOffAlarm.TabIndex = 35
        Me.PulseButtonTakeOffAlarm.Text = "Сработала блокировка по аварийному значению. Внимание! Работа с авариями перенесе" &
    "на в окно ""Управление по событиям"""
        Me.ToolTip1.SetToolTip(Me.PulseButtonTakeOffAlarm, "Нажмите сюда для снятия сигнала блокировки")
        Me.PulseButtonTakeOffAlarm.UseVisualStyleBackColor = True
        Me.PulseButtonTakeOffAlarm.Visible = False
        '
        'ContextMenuCursor
        '
        Me.ContextMenuCursor.Name = "cursorContextMenu"
        Me.ContextMenuCursor.Size = New System.Drawing.Size(61, 4)
        '
        'ContextMenuAnnotation
        '
        Me.ContextMenuAnnotation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuСhangeText})
        Me.ContextMenuAnnotation.Name = "ContextMenuStrip1"
        Me.ContextMenuAnnotation.Size = New System.Drawing.Size(162, 26)
        '
        'MenuСhangeText
        '
        Me.MenuСhangeText.Name = "MenuСhangeText"
        Me.MenuСhangeText.Size = New System.Drawing.Size(161, 22)
        Me.MenuСhangeText.Text = "Поменять Текст"
        '
        'TimerRealize3D
        '
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 695)
        Me.Controls.Add(Me.PulseButtonTakeOffAlarm)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.StatusStripMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Tag = "FormIsDaughter"
        Me.Text = "Регистратор"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.SplitContainerForm.Panel1.ResumeLayout(False)
        Me.SplitContainerForm.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerForm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerForm.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.SlidePlot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSlide.ResumeLayout(False)
        CType(Me.SlideAxis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraph.Panel1.ResumeLayout(False)
        Me.SplitContainerGraph.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGraph, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraph.ResumeLayout(False)
        Me.SplitContainerGraph1.Panel1.ResumeLayout(False)
        Me.SplitContainerGraph1.Panel1.PerformLayout()
        Me.SplitContainerGraph1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGraph1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraph1.ResumeLayout(False)
        CType(Me.WaveformGraphTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursorStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursorEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursorTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursorParametr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelMode.ResumeLayout(False)
        Me.TableLayoutPanelMode.ResumeLayout(False)
        CType(Me.NumericUpDownPrecisionScreen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraph2.Panel1.ResumeLayout(False)
        Me.SplitContainerGraph2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerGraph2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerGraph2.ResumeLayout(False)
        CType(Me.ScatterGraphParameter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanelУправленияДляСнимка.ResumeLayout(False)
        Me.InstrumentControlStrip1.ResumeLayout(False)
        Me.InstrumentControlStrip1.PerformLayout()
        Me.TableLayoutPanelFrameCursor.ResumeLayout(False)
        Me.GroupBoxAxis.ResumeLayout(False)
        Me.GroupBoxAxis.PerformLayout()
        Me.GroupBoxDelete.ResumeLayout(False)
        Me.GroupBoxDelete.PerformLayout()
        Me.PanelKey.ResumeLayout(False)
        Me.PanelKey.PerformLayout()
        Me.ToolStripMain2.ResumeLayout(False)
        Me.ToolStripMain2.PerformLayout()
        Me.GroupeBoxCursorStart.ResumeLayout(False)
        Me.GroupeBoxCursorStart.PerformLayout()
        Me.GroupeBoxCursorEnd.ResumeLayout(False)
        Me.GroupeBoxCursorEnd.PerformLayout()
        Me.MenuStripShap.ResumeLayout(False)
        Me.MenuStripShap.PerformLayout()
        Me.ToolStripMain.ResumeLayout(False)
        Me.ToolStripMain.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.StatusStripMain.ResumeLayout(False)
        Me.StatusStripMain.PerformLayout()
        Me.ContextMenuAnnotation.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents StatusStripMain As System.Windows.Forms.StatusStrip
    Friend WithEvents LabelRegistration As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ProgressBarExport As System.Windows.Forms.ToolStripProgressBar
    Private WithEvents ToolStripMain As System.Windows.Forms.ToolStrip
    Friend WithEvents ButtonSnapshot As System.Windows.Forms.ToolStripButton
    Protected WithEvents ButtonContinuously As System.Windows.Forms.ToolStripButton
    Protected WithEvents ButtonRecord As System.Windows.Forms.ToolStripButton
    Private WithEvents ButtonGraphTime As System.Windows.Forms.ToolStripButton
    Protected WithEvents ButtonGraphParameter As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuStripShap As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuFindSnapshot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOpenBaseSnapshot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuRecordGraph As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExportSnapshotInExcel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPrintGraph As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuChangingBase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainerForm As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerGraph1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SlidePlot As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents SlideAxis As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents ComboBoxSelectiveList As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxRecordToDisc As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents LabelProduct As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LabelDisc As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LabelDiscValue As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LabelSampleRate As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents GroupBoxAxis As System.Windows.Forms.GroupBox
    Public WithEvents GroupBoxDelete As System.Windows.Forms.GroupBox
    Public WithEvents ComboBoxDescriptionPointer As System.Windows.Forms.ComboBox
    Public WithEvents ButtonDelete As System.Windows.Forms.Button
    Public WithEvents TextBoxCount As System.Windows.Forms.TextBox
    Public WithEvents TextBoxDescriptionOnAxes As System.Windows.Forms.TextBox
    Public WithEvents ButtonShowAxes As System.Windows.Forms.Button
    Public WithEvents ComboBoxPointers As System.Windows.Forms.ComboBox
    Public WithEvents ButtonFixLine As System.Windows.Forms.Button
    Public WithEvents GroupeBoxCursorStart As System.Windows.Forms.GroupBox
    Public WithEvents XPosition As System.Windows.Forms.TextBox
    Public WithEvents YPosition As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents GroupeBoxCursorEnd As System.Windows.Forms.GroupBox
    Public WithEvents PeriodVal As System.Windows.Forms.TextBox
    Public WithEvents AmplitudeVal As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SplitContainerGraph As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerGraph2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SlideTime As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents LabelSec As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ComboBoxTimeMeasurement As System.Windows.Forms.ToolStripComboBox
    Public WithEvents TimerButtonRun As System.Windows.Forms.Timer
    Public WithEvents TimerSwitchOffRecord As System.Windows.Forms.Timer
    Public WithEvents TimerCursor As System.Windows.Forms.Timer
    Friend WithEvents TimerTimeOutClient As System.Windows.Forms.Timer
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend WithEvents MenuModeOfOperation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuRegistration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuDecoding As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExecute As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuTune As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuDebugging As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuProtocol As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuComeBackToBeginning As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuWriteDecodingSnapshotToDBase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWriteDecodingSnapshotToExcel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPrintDecodingSnapshot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuShowTextControl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuShowGraphControl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuServerEnable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuReceiveFromServer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSimulator As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuGraphAlongTime As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuGraphAlongParameters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuParameterInRange As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuQuestioningParameter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuShowSetting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuConfigurationChannels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuVisibilityTrend As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSelectiveList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSelectiveGraphControl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExplorerHelpApplication As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuHelpRegime As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAboutProgramm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonСursorStartMoveBack As System.Windows.Forms.Button
    Friend WithEvents ButtonСursorStartMoveForward As System.Windows.Forms.Button
    Friend WithEvents ButtonСursorEndMoveForward As System.Windows.Forms.Button
    Friend WithEvents ButtonСursorEndMoveBack As System.Windows.Forms.Button
    Friend WithEvents LabelIndicator As System.Windows.Forms.ToolStripLabel
    Public WithEvents TSMenuNewWindows As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuWindow As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowRegistration As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowSnapshot As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowTarir As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowDBaseChannels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowCascade As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowTileHorizontal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuWindowTileVertical As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MenuNewWindowClient As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ComboBoxSelectAxis As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents MenuPens As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonDetailSelective As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuGraphCheckedList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuEditGraphOfParameter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripBar As System.Windows.Forms.ToolStripSeparator
    Public WithEvents MenuNewWindowEvents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProviderAdvancedCHM As System.Windows.Forms.HelpProvider
    Friend WithEvents MenuContents As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonTuneTrand As System.Windows.Forms.ToolStripButton
    Friend WithEvents ScatterGraphParameter As NationalInstruments.UI.WindowsForms.ScatterGraph
    Friend WithEvents XyCursor1 As NationalInstruments.UI.XYCursor
    Friend WithEvents ScatterPlot2 As NationalInstruments.UI.ScatterPlot
    Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
    Friend WithEvents WaveformGraphTime As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents WaveformPlot1 As NationalInstruments.UI.WaveformPlot
    Friend WithEvents YAxisTime As NationalInstruments.UI.YAxis
    Friend WithEvents XyCursorStart As NationalInstruments.UI.XYCursor
    Friend WithEvents XyCursorEnd As NationalInstruments.UI.XYCursor
    Friend WithEvents XyCursorTime As NationalInstruments.UI.XYCursor
    Friend WithEvents XyCursorParametr As NationalInstruments.UI.XYCursor
    Friend WithEvents PanelKey As System.Windows.Forms.Panel
    Friend WithEvents ListViewAction As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeaderKeys1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderAction1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStripMain2 As System.Windows.Forms.ToolStrip
    Friend WithEvents ButtonXRange As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonYRange As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonDragCursor As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonPanX As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonPanY As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonZoomPoint As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonZoomX As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonZoomY As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuXRange As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuYRange As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuInteractionModes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuDragCursor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPanX As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPanY As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuZoomPoint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuZoomX As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuZoomY As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonOneCursor As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonTwoCursor As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EnableLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonMore As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuCursor As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuAnnotation As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuСhangeText As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBoxCaption As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanelУправленияДляСнимка As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents InstrumentControlStrip1 As NationalInstruments.UI.WindowsForms.InstrumentControlStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor1 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor2 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor3 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ToolStripLabel4 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor4 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ToolStripLabel5 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor5 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ToolStripLabel6 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripPropertyEditor6 As NationalInstruments.UI.WindowsForms.ToolStripPropertyEditor
    Friend WithEvents ColumnHeaderKeys2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderAction2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TableLayoutPanelFrameCursor As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TimerRealize3D As System.Windows.Forms.Timer
    Friend WithEvents TextBoxConnectOk As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TextBoxConnectBad As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ButtonConnectRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents LabelMessageTcpClient As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ButtonAddTiks As System.Windows.Forms.ToolStripButton
    Friend WithEvents ButtonDeleteTiks As System.Windows.Forms.ToolStripButton
    Friend WithEvents ListViewAcquisition As DbListView
    Friend WithEvents PanelMode As Panel
    Friend WithEvents NumericUpDownPrecisionScreen As NumericUpDown
    Friend WithEvents ListViewParametr As DbListView
    Friend WithEvents PulseButtonTakeOffAlarm As PulseButton.PulseButton
    Friend WithEvents PanelSlide As Panel
    Friend WithEvents PanelSlideBottom As Panel
    Friend WithEvents XAxisTime As XAxis
    Friend WithEvents MenuNormTU As ToolStripMenuItem
    Friend WithEvents TextBoxIndicatorN1physics As ToolStripTextBox
    Friend WithEvents TextBoxIndicatorN1reduce As ToolStripTextBox
    Friend WithEvents TextBoxIndicatorN2physics As ToolStripTextBox
    Friend WithEvents TextBoxIndicatorN2reduce As ToolStripTextBox
    Friend WithEvents TextBoxIndicatorRud As ToolStripTextBox
    Friend WithEvents TextBoxRegime As ToolStripLabel
    Friend WithEvents TableLayoutPanelMode As TableLayoutPanel
    Friend WithEvents ComboBoxDisplayRate As ComboBox
    Friend WithEvents MenuCommandClientServer As ToolStripMenuItem
    Friend WithEvents MenuSelectiveTextControl As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
End Class
