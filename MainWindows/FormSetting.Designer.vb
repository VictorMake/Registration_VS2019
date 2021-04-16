<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSetting
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSetting))
        Me.OpenFileDialogPuth = New System.Windows.Forms.OpenFileDialog()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.ComboNumberStand = New System.Windows.Forms.ComboBox()
        Me.ComboEngine = New System.Windows.Forms.ComboBox()
        Me.TabOption = New System.Windows.Forms.TabControl()
        Me.TabPageProduct = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelProduct = New System.Windows.Forms.TableLayoutPanel()
        Me.DTPickerDate = New System.Windows.Forms.DateTimePicker()
        Me.FraProduct = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelNumberProduct = New System.Windows.Forms.Label()
        Me.TextNumberProduct = New System.Windows.Forms.TextBox()
        Me.LabelEngine = New System.Windows.Forms.Label()
        Me.ComboTypeKrd = New System.Windows.Forms.ComboBox()
        Me.LabelTypeKrd = New System.Windows.Forms.Label()
        Me.ComboRegime = New System.Windows.Forms.ComboBox()
        Me.LabelRegime = New System.Windows.Forms.Label()
        Me.LabelStend = New System.Windows.Forms.Label()
        Me.LabelDate = New System.Windows.Forms.Label()
        Me.LinkLabelSettingCompactRio = New System.Windows.Forms.LinkLabel()
        Me.TabPageStartStop = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelStartStop = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelNameVarStopWrite = New System.Windows.Forms.Panel()
        Me.ButtonFindChannelStopWrite = New System.Windows.Forms.Button()
        Me.ComboBoxNameVarStopWrite = New System.Windows.Forms.ComboBox()
        Me.PanelNameVarStartWrite = New System.Windows.Forms.Panel()
        Me.ComboBoxNameVarStartWrite = New System.Windows.Forms.ComboBox()
        Me.ButtonFindChannelStartWrite = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.LabelWaitStopWrite = New System.Windows.Forms.Label()
        Me.LabelSeparate = New System.Windows.Forms.Label()
        Me.LabelValueStopWrite = New System.Windows.Forms.Label()
        Me.TextWaitStartWrite = New System.Windows.Forms.TextBox()
        Me.LabelNameVarStop = New System.Windows.Forms.Label()
        Me.LabelWaitStartWrite = New System.Windows.Forms.Label()
        Me.LabelNameVarStartWrite = New System.Windows.Forms.Label()
        Me.TextValueStopWrite = New System.Windows.Forms.TextBox()
        Me.TextWaitStopWrite = New System.Windows.Forms.TextBox()
        Me.TabPageMeasure = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.ComboBoxCountClientOrNumberClient = New System.Windows.Forms.ComboBox()
        Me.LabelComboBoxCountClientOrNumberClient = New System.Windows.Forms.Label()
        Me.LabelPrecision = New System.Windows.Forms.Label()
        Me.FrameStandClient = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelClient = New System.Windows.Forms.TableLayoutPanel()
        Me.TextPathClient = New System.Windows.Forms.TextBox()
        Me.ButtonPathClientExplorer = New System.Windows.Forms.Button()
        Me.ComboPathClient = New System.Windows.Forms.ComboBox()
        Me.LabelPathClient = New System.Windows.Forms.Label()
        Me.FrameStandServer = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelServer = New System.Windows.Forms.TableLayoutPanel()
        Me.TextPathServer = New System.Windows.Forms.TextBox()
        Me.ComboPathServer = New System.Windows.Forms.ComboBox()
        Me.LabelPathServer = New System.Windows.Forms.Label()
        Me.ButtonPathServerExplorer = New System.Windows.Forms.Button()
        Me.NumPrecision = New System.Windows.Forms.NumericUpDown()
        Me.LabelIntervalSnapshot = New System.Windows.Forms.Label()
        Me.PanelKadr = New System.Windows.Forms.Panel()
        Me.ComboIntervalSnapshot = New System.Windows.Forms.ComboBox()
        Me.LabelIntervalSnapshotFact = New System.Windows.Forms.Label()
        Me.ComboFrequencyCollection = New System.Windows.Forms.ComboBox()
        Me.LabelFrequencyCollection = New System.Windows.Forms.Label()
        Me.LabelFrequencySamplingChannel = New System.Windows.Forms.Label()
        Me.LabelDiscredit = New System.Windows.Forms.Label()
        Me.LabelDiscreditFact = New System.Windows.Forms.Label()
        Me.TextFrequencySamplingChannel = New System.Windows.Forms.Label()
        Me.TabPageChannels = New System.Windows.Forms.TabPage()
        Me.PanelConstantChannels = New System.Windows.Forms.Panel()
        Me.DataGridViewConstantChannels = New System.Windows.Forms.DataGridView()
        Me.BindingNavigatorConstantChannels = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.LabelSelectedChannel = New System.Windows.Forms.Label()
        Me.GroupBoxWeather = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelControls = New System.Windows.Forms.TableLayoutPanel()
        Me.TextTbox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelTbox = New System.Windows.Forms.Label()
        Me.GroupBoxNameConst = New System.Windows.Forms.GroupBox()
        Me.PanelGrid = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelAlias = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelTbox = New System.Windows.Forms.Panel()
        Me.ButtonFindChannelTbox = New System.Windows.Forms.Button()
        Me.ComboBoxChannelTbox = New System.Windows.Forms.ComboBox()
        Me.LabelNameChannel = New System.Windows.Forms.Label()
        Me.PanelTxc = New System.Windows.Forms.Panel()
        Me.ButtonFindChannelTxc = New System.Windows.Forms.Button()
        Me.ComboBoxTxc = New System.Windows.Forms.ComboBox()
        Me.LabelNameParam = New System.Windows.Forms.Label()
        Me.LabelTxc = New System.Windows.Forms.Label()
        Me.LabelNameTbox = New System.Windows.Forms.Label()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonCompactRio = New System.Windows.Forms.RadioButton()
        Me.ButtonWithClient = New System.Windows.Forms.RadioButton()
        Me.ButtonWithSnapshop = New System.Windows.Forms.RadioButton()
        Me.ButtonWithController = New System.Windows.Forms.RadioButton()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.FrameTypeWork = New System.Windows.Forms.GroupBox()
        Me.hpPlainHTML = New System.Windows.Forms.HelpProvider()
        Me.TableLayoutPanelTypeWork = New System.Windows.Forms.TableLayoutPanel()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelSettingCompactRio = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.TabOption.SuspendLayout()
        Me.TabPageProduct.SuspendLayout()
        Me.TableLayoutPanelProduct.SuspendLayout()
        Me.FraProduct.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TabPageStartStop.SuspendLayout()
        Me.TableLayoutPanelStartStop.SuspendLayout()
        Me.PanelNameVarStopWrite.SuspendLayout()
        Me.PanelNameVarStartWrite.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageMeasure.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.FrameStandClient.SuspendLayout()
        Me.TableLayoutPanelClient.SuspendLayout()
        Me.FrameStandServer.SuspendLayout()
        Me.TableLayoutPanelServer.SuspendLayout()
        CType(Me.NumPrecision, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelKadr.SuspendLayout()
        Me.TabPageChannels.SuspendLayout()
        Me.PanelConstantChannels.SuspendLayout()
        CType(Me.DataGridViewConstantChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorConstantChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorConstantChannels.SuspendLayout()
        Me.GroupBoxWeather.SuspendLayout()
        Me.TableLayoutPanelControls.SuspendLayout()
        Me.GroupBoxNameConst.SuspendLayout()
        Me.PanelGrid.SuspendLayout()
        Me.TableLayoutPanelAlias.SuspendLayout()
        Me.PanelTbox.SuspendLayout()
        Me.PanelTxc.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FrameTypeWork.SuspendLayout()
        Me.TableLayoutPanelTypeWork.SuspendLayout()
        Me.PanelSettingCompactRio.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboNumberStand
        '
        Me.ComboNumberStand.BackColor = System.Drawing.SystemColors.Window
        Me.ComboNumberStand.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboNumberStand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboNumberStand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HelpProvider1.SetHelpString(Me.ComboNumberStand, "Выбор номера стенда")
        Me.ComboNumberStand.Location = New System.Drawing.Point(209, 29)
        Me.ComboNumberStand.Name = "ComboNumberStand"
        Me.ComboNumberStand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.ComboNumberStand, True)
        Me.ComboNumberStand.Size = New System.Drawing.Size(80, 21)
        Me.ComboNumberStand.TabIndex = 72
        Me.ToolTip1.SetToolTip(Me.ComboNumberStand, "Выбор номера стенда")
        '
        'ComboEngine
        '
        Me.ComboEngine.BackColor = System.Drawing.SystemColors.Window
        Me.ComboEngine.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboEngine.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HelpProvider1.SetHelpString(Me.ComboEngine, "Выбор модификации изделия")
        Me.ComboEngine.Location = New System.Drawing.Point(203, 29)
        Me.ComboEngine.Name = "ComboEngine"
        Me.ComboEngine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.ComboEngine, True)
        Me.ComboEngine.Size = New System.Drawing.Size(80, 21)
        Me.ComboEngine.TabIndex = 30
        Me.ToolTip1.SetToolTip(Me.ComboEngine, "Выбор типа изделия")
        '
        'TabOption
        '
        Me.TabOption.Controls.Add(Me.TabPageProduct)
        Me.TabOption.Controls.Add(Me.TabPageStartStop)
        Me.TabOption.Controls.Add(Me.TabPageMeasure)
        Me.TabOption.Controls.Add(Me.TabPageChannels)
        Me.TabOption.ImageList = Me.ImageList1
        Me.TabOption.Location = New System.Drawing.Point(12, 12)
        Me.TabOption.Name = "TabOption"
        Me.TabOption.SelectedIndex = 0
        Me.HelpProvider1.SetShowHelp(Me.TabOption, False)
        Me.TabOption.Size = New System.Drawing.Size(424, 381)
        Me.TabOption.TabIndex = 68
        '
        'TabPageProduct
        '
        Me.TabPageProduct.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageProduct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageProduct.Controls.Add(Me.TableLayoutPanelProduct)
        Me.TabPageProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TabPageProduct.ImageIndex = 0
        Me.TabPageProduct.Location = New System.Drawing.Point(4, 23)
        Me.TabPageProduct.Name = "TabPageProduct"
        Me.TabPageProduct.Size = New System.Drawing.Size(416, 354)
        Me.TabPageProduct.TabIndex = 0
        Me.TabPageProduct.Text = "Изделие"
        Me.TabPageProduct.ToolTipText = "Вкладка настроек типа работы приложения"
        '
        'TableLayoutPanelProduct
        '
        Me.TableLayoutPanelProduct.ColumnCount = 2
        Me.TableLayoutPanelProduct.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelProduct.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelProduct.Controls.Add(Me.DTPickerDate, 1, 0)
        Me.TableLayoutPanelProduct.Controls.Add(Me.FraProduct, 0, 3)
        Me.TableLayoutPanelProduct.Controls.Add(Me.LabelStend, 0, 1)
        Me.TableLayoutPanelProduct.Controls.Add(Me.LabelDate, 0, 0)
        Me.TableLayoutPanelProduct.Controls.Add(Me.ComboNumberStand, 1, 1)
        Me.TableLayoutPanelProduct.Controls.Add(Me.PanelSettingCompactRio, 0, 4)
        Me.TableLayoutPanelProduct.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelProduct.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelProduct.Name = "TableLayoutPanelProduct"
        Me.TableLayoutPanelProduct.RowCount = 6
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 116.0!))
        Me.TableLayoutPanelProduct.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelProduct.Size = New System.Drawing.Size(412, 350)
        Me.TableLayoutPanelProduct.TabIndex = 72
        '
        'DTPickerDate
        '
        Me.DTPickerDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DTPickerDate.Location = New System.Drawing.Point(209, 3)
        Me.DTPickerDate.Name = "DTPickerDate"
        Me.DTPickerDate.Size = New System.Drawing.Size(80, 20)
        Me.DTPickerDate.TabIndex = 78
        Me.ToolTip1.SetToolTip(Me.DTPickerDate, "Установка текущей даты испытания")
        '
        'FraProduct
        '
        Me.FraProduct.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanelProduct.SetColumnSpan(Me.FraProduct, 2)
        Me.FraProduct.Controls.Add(Me.TableLayoutPanel4)
        Me.FraProduct.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FraProduct.ForeColor = System.Drawing.Color.Blue
        Me.FraProduct.Location = New System.Drawing.Point(3, 81)
        Me.FraProduct.Name = "FraProduct"
        Me.FraProduct.Size = New System.Drawing.Size(406, 122)
        Me.FraProduct.TabIndex = 76
        Me.FraProduct.TabStop = False
        Me.FraProduct.Text = "Изделие"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.LabelNumberProduct, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.TextNumberProduct, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.ComboEngine, 1, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.LabelEngine, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.ComboTypeKrd, 1, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.LabelTypeKrd, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.ComboRegime, 1, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.LabelRegime, 0, 3)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 4
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(400, 103)
        Me.TableLayoutPanel4.TabIndex = 73
        '
        'LabelNumberProduct
        '
        Me.LabelNumberProduct.BackColor = System.Drawing.SystemColors.Control
        Me.LabelNumberProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelNumberProduct.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNumberProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNumberProduct.Location = New System.Drawing.Point(3, 0)
        Me.LabelNumberProduct.Name = "LabelNumberProduct"
        Me.LabelNumberProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelNumberProduct.Size = New System.Drawing.Size(194, 26)
        Me.LabelNumberProduct.TabIndex = 73
        Me.LabelNumberProduct.Text = "Номер изделия:"
        Me.LabelNumberProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextNumberProduct
        '
        Me.TextNumberProduct.AcceptsReturn = True
        Me.TextNumberProduct.BackColor = System.Drawing.SystemColors.Control
        Me.TextNumberProduct.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextNumberProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextNumberProduct.Location = New System.Drawing.Point(203, 3)
        Me.TextNumberProduct.MaxLength = 0
        Me.TextNumberProduct.Name = "TextNumberProduct"
        Me.TextNumberProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextNumberProduct.Size = New System.Drawing.Size(80, 20)
        Me.TextNumberProduct.TabIndex = 69
        Me.TextNumberProduct.Text = "123"
        Me.ToolTip1.SetToolTip(Me.TextNumberProduct, "Ввод номера изделия")
        '
        'LabelEngine
        '
        Me.LabelEngine.BackColor = System.Drawing.SystemColors.Control
        Me.LabelEngine.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelEngine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelEngine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelEngine.Location = New System.Drawing.Point(3, 26)
        Me.LabelEngine.Name = "LabelEngine"
        Me.LabelEngine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelEngine.Size = New System.Drawing.Size(194, 26)
        Me.LabelEngine.TabIndex = 33
        Me.LabelEngine.Text = "Модификация изделия:"
        Me.LabelEngine.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboTypeKrd
        '
        Me.ComboTypeKrd.BackColor = System.Drawing.SystemColors.Window
        Me.ComboTypeKrd.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboTypeKrd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTypeKrd.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HelpProvider1.SetHelpString(Me.ComboTypeKrd, "Выбор типа КРД")
        Me.ComboTypeKrd.Location = New System.Drawing.Point(203, 55)
        Me.ComboTypeKrd.Name = "ComboTypeKrd"
        Me.ComboTypeKrd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.ComboTypeKrd, True)
        Me.ComboTypeKrd.Size = New System.Drawing.Size(80, 21)
        Me.ComboTypeKrd.TabIndex = 31
        Me.ToolTip1.SetToolTip(Me.ComboTypeKrd, "Выбор типа КРД")
        '
        'LabelTypeKrd
        '
        Me.LabelTypeKrd.AutoSize = True
        Me.LabelTypeKrd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTypeKrd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTypeKrd.Location = New System.Drawing.Point(3, 52)
        Me.LabelTypeKrd.Name = "LabelTypeKrd"
        Me.LabelTypeKrd.Size = New System.Drawing.Size(194, 26)
        Me.LabelTypeKrd.TabIndex = 6
        Me.LabelTypeKrd.Text = "Тип КРД:"
        Me.LabelTypeKrd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboRegime
        '
        Me.ComboRegime.BackColor = System.Drawing.SystemColors.Window
        Me.ComboRegime.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboRegime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboRegime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HelpProvider1.SetHelpString(Me.ComboRegime, "Выбор режима")
        Me.ComboRegime.Location = New System.Drawing.Point(203, 81)
        Me.ComboRegime.Name = "ComboRegime"
        Me.ComboRegime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HelpProvider1.SetShowHelp(Me.ComboRegime, True)
        Me.ComboRegime.Size = New System.Drawing.Size(80, 21)
        Me.ComboRegime.TabIndex = 33
        Me.ToolTip1.SetToolTip(Me.ComboRegime, "Выбор режима")
        '
        'LabelRegime
        '
        Me.LabelRegime.AutoSize = True
        Me.LabelRegime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelRegime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelRegime.Location = New System.Drawing.Point(3, 78)
        Me.LabelRegime.Name = "LabelRegime"
        Me.LabelRegime.Size = New System.Drawing.Size(194, 26)
        Me.LabelRegime.TabIndex = 32
        Me.LabelRegime.Text = "Режим:"
        Me.LabelRegime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelStend
        '
        Me.LabelStend.AutoSize = True
        Me.LabelStend.BackColor = System.Drawing.SystemColors.Control
        Me.LabelStend.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelStend.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelStend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelStend.Location = New System.Drawing.Point(3, 26)
        Me.LabelStend.Name = "LabelStend"
        Me.LabelStend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelStend.Size = New System.Drawing.Size(200, 26)
        Me.LabelStend.TabIndex = 73
        Me.LabelStend.Text = "Номер стенда:"
        Me.LabelStend.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelDate
        '
        Me.LabelDate.AutoSize = True
        Me.LabelDate.BackColor = System.Drawing.SystemColors.Control
        Me.LabelDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelDate.Location = New System.Drawing.Point(3, 0)
        Me.LabelDate.Name = "LabelDate"
        Me.LabelDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDate.Size = New System.Drawing.Size(200, 26)
        Me.LabelDate.TabIndex = 74
        Me.LabelDate.Text = "Дата:"
        Me.LabelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LinkLabelSettingCompactRio
        '
        Me.LinkLabelSettingCompactRio.AutoSize = True
        Me.LinkLabelSettingCompactRio.Location = New System.Drawing.Point(41, 12)
        Me.LinkLabelSettingCompactRio.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.LinkLabelSettingCompactRio.Name = "LinkLabelSettingCompactRio"
        Me.LinkLabelSettingCompactRio.Size = New System.Drawing.Size(193, 13)
        Me.LinkLabelSettingCompactRio.TabIndex = 79
        Me.LinkLabelSettingCompactRio.TabStop = True
        Me.LinkLabelSettingCompactRio.Text = "Конфигурировать шасси CompactRio"
        '
        'TabPageStartStop
        '
        Me.TabPageStartStop.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageStartStop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageStartStop.Controls.Add(Me.TableLayoutPanelStartStop)
        Me.TabPageStartStop.ImageIndex = 4
        Me.TabPageStartStop.Location = New System.Drawing.Point(4, 23)
        Me.TabPageStartStop.Name = "TabPageStartStop"
        Me.TabPageStartStop.Size = New System.Drawing.Size(416, 354)
        Me.TabPageStartStop.TabIndex = 3
        Me.TabPageStartStop.Text = "Старт-Стоп Запись"
        Me.TabPageStartStop.ToolTipText = "Вкладка настроек автоматического включения записи"
        '
        'TableLayoutPanelStartStop
        '
        Me.TableLayoutPanelStartStop.ColumnCount = 2
        Me.TableLayoutPanelStartStop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.23301!))
        Me.TableLayoutPanelStartStop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.76699!))
        Me.TableLayoutPanelStartStop.Controls.Add(Me.PanelNameVarStopWrite, 1, 3)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.PanelNameVarStartWrite, 1, 0)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.PictureBox1, 0, 6)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelWaitStopWrite, 0, 5)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelSeparate, 0, 2)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelValueStopWrite, 0, 4)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.TextWaitStartWrite, 1, 1)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelNameVarStop, 0, 3)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelWaitStartWrite, 0, 1)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.LabelNameVarStartWrite, 0, 0)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.TextValueStopWrite, 1, 4)
        Me.TableLayoutPanelStartStop.Controls.Add(Me.TextWaitStopWrite, 1, 5)
        Me.TableLayoutPanelStartStop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelStartStop.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelStartStop.Name = "TableLayoutPanelStartStop"
        Me.TableLayoutPanelStartStop.RowCount = 7
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanelStartStop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelStartStop.Size = New System.Drawing.Size(412, 350)
        Me.TableLayoutPanelStartStop.TabIndex = 72
        '
        'PanelNameVarStopWrite
        '
        Me.PanelNameVarStopWrite.Controls.Add(Me.ButtonFindChannelStopWrite)
        Me.PanelNameVarStopWrite.Controls.Add(Me.ComboBoxNameVarStopWrite)
        Me.PanelNameVarStopWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelNameVarStopWrite.Location = New System.Drawing.Point(279, 65)
        Me.PanelNameVarStopWrite.Name = "PanelNameVarStopWrite"
        Me.PanelNameVarStopWrite.Size = New System.Drawing.Size(130, 24)
        Me.PanelNameVarStopWrite.TabIndex = 72
        '
        'ButtonFindChannelStopWrite
        '
        Me.ButtonFindChannelStopWrite.Image = CType(resources.GetObject("ButtonFindChannelStopWrite.Image"), System.Drawing.Image)
        Me.ButtonFindChannelStopWrite.Location = New System.Drawing.Point(104, 0)
        Me.ButtonFindChannelStopWrite.Name = "ButtonFindChannelStopWrite"
        Me.ButtonFindChannelStopWrite.Size = New System.Drawing.Size(24, 22)
        Me.ButtonFindChannelStopWrite.TabIndex = 74
        Me.ButtonFindChannelStopWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannelStopWrite, "Быстрый поиск канала")
        Me.ButtonFindChannelStopWrite.UseVisualStyleBackColor = True
        '
        'ComboBoxNameVarStopWrite
        '
        Me.ComboBoxNameVarStopWrite.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxNameVarStopWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxNameVarStopWrite.Dock = System.Windows.Forms.DockStyle.Left
        Me.ComboBoxNameVarStopWrite.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxNameVarStopWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNameVarStopWrite.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxNameVarStopWrite.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxNameVarStopWrite.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxNameVarStopWrite.MaxDropDownItems = 32
        Me.ComboBoxNameVarStopWrite.Name = "ComboBoxNameVarStopWrite"
        Me.ComboBoxNameVarStopWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxNameVarStopWrite.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxNameVarStopWrite.TabIndex = 73
        Me.ToolTip1.SetToolTip(Me.ComboBoxNameVarStopWrite, "Имя параметра для автоматического выключения записи, <N1> по умолчанию.")
        '
        'PanelNameVarStartWrite
        '
        Me.PanelNameVarStartWrite.Controls.Add(Me.ComboBoxNameVarStartWrite)
        Me.PanelNameVarStartWrite.Controls.Add(Me.ButtonFindChannelStartWrite)
        Me.PanelNameVarStartWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelNameVarStartWrite.Location = New System.Drawing.Point(279, 3)
        Me.PanelNameVarStartWrite.Name = "PanelNameVarStartWrite"
        Me.PanelNameVarStartWrite.Size = New System.Drawing.Size(130, 24)
        Me.PanelNameVarStartWrite.TabIndex = 72
        '
        'ComboBoxNameVarStartWrite
        '
        Me.ComboBoxNameVarStartWrite.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxNameVarStartWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxNameVarStartWrite.Dock = System.Windows.Forms.DockStyle.Left
        Me.ComboBoxNameVarStartWrite.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxNameVarStartWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNameVarStartWrite.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxNameVarStartWrite.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxNameVarStartWrite.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxNameVarStartWrite.MaxDropDownItems = 32
        Me.ComboBoxNameVarStartWrite.Name = "ComboBoxNameVarStartWrite"
        Me.ComboBoxNameVarStartWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxNameVarStartWrite.Size = New System.Drawing.Size(103, 21)
        Me.ComboBoxNameVarStartWrite.TabIndex = 72
        Me.ToolTip1.SetToolTip(Me.ComboBoxNameVarStartWrite, "Имя дискретного параметра, <Запуск> по умолчанию.")
        '
        'ButtonFindChannelStartWrite
        '
        Me.ButtonFindChannelStartWrite.Image = CType(resources.GetObject("ButtonFindChannelStartWrite.Image"), System.Drawing.Image)
        Me.ButtonFindChannelStartWrite.Location = New System.Drawing.Point(104, 0)
        Me.ButtonFindChannelStartWrite.Name = "ButtonFindChannelStartWrite"
        Me.ButtonFindChannelStartWrite.Size = New System.Drawing.Size(24, 22)
        Me.ButtonFindChannelStartWrite.TabIndex = 73
        Me.ButtonFindChannelStartWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannelStartWrite, "Быстрый поиск канала")
        Me.ButtonFindChannelStartWrite.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutPanelStartStop.SetColumnSpan(Me.PictureBox1, 2)
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 151)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(406, 196)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Пояснение назначения настроек")
        '
        'LabelWaitStopWrite
        '
        Me.LabelWaitStopWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelWaitStopWrite.Location = New System.Drawing.Point(3, 120)
        Me.LabelWaitStopWrite.Name = "LabelWaitStopWrite"
        Me.LabelWaitStopWrite.Size = New System.Drawing.Size(270, 28)
        Me.LabelWaitStopWrite.TabIndex = 4
        Me.LabelWaitStopWrite.Text = "Время задержки после срабатывания порога(сек.):"
        Me.LabelWaitStopWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelSeparate
        '
        Me.LabelSeparate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanelStartStop.SetColumnSpan(Me.LabelSeparate, 2)
        Me.LabelSeparate.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelSeparate.Location = New System.Drawing.Point(3, 60)
        Me.LabelSeparate.Name = "LabelSeparate"
        Me.LabelSeparate.Size = New System.Drawing.Size(406, 2)
        Me.LabelSeparate.TabIndex = 11
        '
        'LabelValueStopWrite
        '
        Me.LabelValueStopWrite.AutoSize = True
        Me.LabelValueStopWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelValueStopWrite.Location = New System.Drawing.Point(3, 92)
        Me.LabelValueStopWrite.Name = "LabelValueStopWrite"
        Me.LabelValueStopWrite.Size = New System.Drawing.Size(270, 28)
        Me.LabelValueStopWrite.TabIndex = 2
        Me.LabelValueStopWrite.Text = "Пороговое значение выключения записи:"
        Me.LabelValueStopWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextWaitStartWrite
        '
        Me.TextWaitStartWrite.Location = New System.Drawing.Point(279, 33)
        Me.TextWaitStartWrite.Name = "TextWaitStartWrite"
        Me.TextWaitStartWrite.Size = New System.Drawing.Size(64, 20)
        Me.TextWaitStartWrite.TabIndex = 9
        Me.TextWaitStartWrite.Text = "1"
        Me.ToolTip1.SetToolTip(Me.TextWaitStartWrite, "Время выхода на рабочий режим")
        '
        'LabelNameVarStop
        '
        Me.LabelNameVarStop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameVarStop.Location = New System.Drawing.Point(3, 62)
        Me.LabelNameVarStop.Name = "LabelNameVarStop"
        Me.LabelNameVarStop.Size = New System.Drawing.Size(270, 30)
        Me.LabelNameVarStop.TabIndex = 0
        Me.LabelNameVarStop.Text = "Имя параметра для автоматического выключения записи:"
        Me.LabelNameVarStop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelWaitStartWrite
        '
        Me.LabelWaitStartWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelWaitStartWrite.Location = New System.Drawing.Point(3, 30)
        Me.LabelWaitStartWrite.Name = "LabelWaitStartWrite"
        Me.LabelWaitStartWrite.Size = New System.Drawing.Size(270, 28)
        Me.LabelWaitStartWrite.TabIndex = 8
        Me.LabelWaitStartWrite.Text = "Время выхода на рабочий режим (сек.) в течении которого параметр выключения не де" &
    "йствует:"
        Me.LabelWaitStartWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelNameVarStartWrite
        '
        Me.LabelNameVarStartWrite.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameVarStartWrite.Location = New System.Drawing.Point(3, 0)
        Me.LabelNameVarStartWrite.Name = "LabelNameVarStartWrite"
        Me.LabelNameVarStartWrite.Size = New System.Drawing.Size(270, 30)
        Me.LabelNameVarStartWrite.TabIndex = 6
        Me.LabelNameVarStartWrite.Text = "Имя дискретного параметра для автоматического включения записи:"
        Me.LabelNameVarStartWrite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextValueStopWrite
        '
        Me.TextValueStopWrite.Location = New System.Drawing.Point(279, 95)
        Me.TextValueStopWrite.Name = "TextValueStopWrite"
        Me.TextValueStopWrite.Size = New System.Drawing.Size(64, 20)
        Me.TextValueStopWrite.TabIndex = 3
        Me.TextValueStopWrite.Text = "1"
        Me.ToolTip1.SetToolTip(Me.TextValueStopWrite, "Пороговое значение выключения записи")
        '
        'TextWaitStopWrite
        '
        Me.TextWaitStopWrite.Location = New System.Drawing.Point(279, 123)
        Me.TextWaitStopWrite.Name = "TextWaitStopWrite"
        Me.TextWaitStopWrite.Size = New System.Drawing.Size(64, 20)
        Me.TextWaitStopWrite.TabIndex = 5
        Me.TextWaitStopWrite.Text = "1"
        Me.ToolTip1.SetToolTip(Me.TextWaitStopWrite, "Время задержки после срабатывания порога")
        '
        'TabPageMeasure
        '
        Me.TabPageMeasure.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageMeasure.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageMeasure.Controls.Add(Me.TableLayoutPanel6)
        Me.TabPageMeasure.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TabPageMeasure.ImageIndex = 2
        Me.TabPageMeasure.Location = New System.Drawing.Point(4, 23)
        Me.TabPageMeasure.Name = "TabPageMeasure"
        Me.TabPageMeasure.Size = New System.Drawing.Size(416, 354)
        Me.TabPageMeasure.TabIndex = 2
        Me.TabPageMeasure.Text = "Сбор"
        Me.TabPageMeasure.ToolTipText = "Вкладка настроек режима сбора и записи"
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 2
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.10551!))
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.89449!))
        Me.TableLayoutPanel6.Controls.Add(Me.ComboBoxCountClientOrNumberClient, 1, 7)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelComboBoxCountClientOrNumberClient, 0, 7)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelPrecision, 0, 4)
        Me.TableLayoutPanel6.Controls.Add(Me.FrameStandClient, 0, 6)
        Me.TableLayoutPanel6.Controls.Add(Me.FrameStandServer, 0, 5)
        Me.TableLayoutPanel6.Controls.Add(Me.NumPrecision, 1, 4)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelIntervalSnapshot, 0, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.PanelKadr, 1, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.ComboFrequencyCollection, 1, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelFrequencyCollection, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelFrequencySamplingChannel, 0, 2)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelDiscredit, 0, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.LabelDiscreditFact, 1, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.TextFrequencySamplingChannel, 1, 2)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 10
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(412, 350)
        Me.TableLayoutPanel6.TabIndex = 72
        '
        'ComboBoxCountClientOrNumberClient
        '
        Me.ComboBoxCountClientOrNumberClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCountClientOrNumberClient.FormattingEnabled = True
        Me.ComboBoxCountClientOrNumberClient.Items.AddRange(New Object() {"1", "2", "3", "4", "5"})
        Me.ComboBoxCountClientOrNumberClient.Location = New System.Drawing.Point(283, 295)
        Me.ComboBoxCountClientOrNumberClient.Name = "ComboBoxCountClientOrNumberClient"
        Me.ComboBoxCountClientOrNumberClient.Size = New System.Drawing.Size(45, 21)
        Me.ComboBoxCountClientOrNumberClient.TabIndex = 75
        Me.ToolTip1.SetToolTip(Me.ComboBoxCountClientOrNumberClient, "Чисто клиентов (компьютеров получающих данные)")
        '
        'LabelComboBoxCountClientOrNumberClient
        '
        Me.LabelComboBoxCountClientOrNumberClient.BackColor = System.Drawing.SystemColors.Control
        Me.LabelComboBoxCountClientOrNumberClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelComboBoxCountClientOrNumberClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelComboBoxCountClientOrNumberClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelComboBoxCountClientOrNumberClient.Location = New System.Drawing.Point(3, 292)
        Me.LabelComboBoxCountClientOrNumberClient.Name = "LabelComboBoxCountClientOrNumberClient"
        Me.LabelComboBoxCountClientOrNumberClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelComboBoxCountClientOrNumberClient.Size = New System.Drawing.Size(274, 26)
        Me.LabelComboBoxCountClientOrNumberClient.TabIndex = 74
        Me.LabelComboBoxCountClientOrNumberClient.Text = "Настраивается"
        Me.LabelComboBoxCountClientOrNumberClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelPrecision
        '
        Me.LabelPrecision.BackColor = System.Drawing.SystemColors.Control
        Me.LabelPrecision.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPrecision.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelPrecision.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPrecision.Location = New System.Drawing.Point(3, 110)
        Me.LabelPrecision.Name = "LabelPrecision"
        Me.LabelPrecision.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPrecision.Size = New System.Drawing.Size(274, 26)
        Me.LabelPrecision.TabIndex = 49
        Me.LabelPrecision.Text = "Количество цифр после точки при отображении:"
        Me.LabelPrecision.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FrameStandClient
        '
        Me.FrameStandClient.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanel6.SetColumnSpan(Me.FrameStandClient, 2)
        Me.FrameStandClient.Controls.Add(Me.TableLayoutPanelClient)
        Me.FrameStandClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameStandClient.ForeColor = System.Drawing.Color.Blue
        Me.FrameStandClient.Location = New System.Drawing.Point(3, 217)
        Me.FrameStandClient.Name = "FrameStandClient"
        Me.FrameStandClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameStandClient.Size = New System.Drawing.Size(406, 72)
        Me.FrameStandClient.TabIndex = 61
        Me.FrameStandClient.TabStop = False
        Me.FrameStandClient.Text = "Настройка путей к базе данных АРМ Клиент"
        '
        'TableLayoutPanelClient
        '
        Me.TableLayoutPanelClient.ColumnCount = 3
        Me.TableLayoutPanelClient.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69.0!))
        Me.TableLayoutPanelClient.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelClient.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanelClient.Controls.Add(Me.TextPathClient, 0, 1)
        Me.TableLayoutPanelClient.Controls.Add(Me.ButtonPathClientExplorer, 2, 1)
        Me.TableLayoutPanelClient.Controls.Add(Me.ComboPathClient, 0, 0)
        Me.TableLayoutPanelClient.Controls.Add(Me.LabelPathClient, 1, 0)
        Me.TableLayoutPanelClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelClient.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanelClient.Name = "TableLayoutPanelClient"
        Me.TableLayoutPanelClient.RowCount = 2
        Me.TableLayoutPanelClient.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelClient.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelClient.Size = New System.Drawing.Size(400, 53)
        Me.TableLayoutPanelClient.TabIndex = 73
        '
        'TextPathClient
        '
        Me.TextPathClient.AcceptsReturn = True
        Me.TextPathClient.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanelClient.SetColumnSpan(Me.TextPathClient, 2)
        Me.TextPathClient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextPathClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextPathClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextPathClient.Location = New System.Drawing.Point(3, 29)
        Me.TextPathClient.MaxLength = 0
        Me.TextPathClient.Name = "TextPathClient"
        Me.TextPathClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextPathClient.Size = New System.Drawing.Size(362, 20)
        Me.TextPathClient.TabIndex = 63
        Me.ToolTip1.SetToolTip(Me.TextPathClient, "Путь к базе данных для выбранного стенда")
        '
        'ButtonPathClientExplorer
        '
        Me.ButtonPathClientExplorer.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonPathClientExplorer.BackgroundImage = CType(resources.GetObject("ButtonPathClientExplorer.BackgroundImage"), System.Drawing.Image)
        Me.ButtonPathClientExplorer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonPathClientExplorer.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonPathClientExplorer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonPathClientExplorer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonPathClientExplorer.Location = New System.Drawing.Point(371, 29)
        Me.ButtonPathClientExplorer.Name = "ButtonPathClientExplorer"
        Me.ButtonPathClientExplorer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonPathClientExplorer.Size = New System.Drawing.Size(25, 21)
        Me.ButtonPathClientExplorer.TabIndex = 64
        Me.ButtonPathClientExplorer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTip1.SetToolTip(Me.ButtonPathClientExplorer, "Настроить путь к базе данных для выбранного стенда")
        Me.ButtonPathClientExplorer.UseVisualStyleBackColor = False
        '
        'ComboPathClient
        '
        Me.ComboPathClient.BackColor = System.Drawing.SystemColors.Window
        Me.ComboPathClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboPathClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPathClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboPathClient.Location = New System.Drawing.Point(3, 3)
        Me.ComboPathClient.Name = "ComboPathClient"
        Me.ComboPathClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboPathClient.Size = New System.Drawing.Size(61, 21)
        Me.ComboPathClient.TabIndex = 62
        Me.ToolTip1.SetToolTip(Me.ComboPathClient, "Выбор номера стенда, где программа работает в режиме получения данных от другого " &
        "АРМ")
        '
        'LabelPathClient
        '
        Me.LabelPathClient.AutoSize = True
        Me.LabelPathClient.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanelClient.SetColumnSpan(Me.LabelPathClient, 2)
        Me.LabelPathClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPathClient.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelPathClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPathClient.Location = New System.Drawing.Point(72, 0)
        Me.LabelPathClient.Name = "LabelPathClient"
        Me.LabelPathClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPathClient.Size = New System.Drawing.Size(325, 26)
        Me.LabelPathClient.TabIndex = 65
        Me.LabelPathClient.Text = "Стенд АРМ Клиент"
        Me.LabelPathClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FrameStandServer
        '
        Me.FrameStandServer.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanel6.SetColumnSpan(Me.FrameStandServer, 2)
        Me.FrameStandServer.Controls.Add(Me.TableLayoutPanelServer)
        Me.FrameStandServer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameStandServer.ForeColor = System.Drawing.Color.Blue
        Me.FrameStandServer.Location = New System.Drawing.Point(3, 139)
        Me.FrameStandServer.Name = "FrameStandServer"
        Me.FrameStandServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameStandServer.Size = New System.Drawing.Size(406, 72)
        Me.FrameStandServer.TabIndex = 55
        Me.FrameStandServer.TabStop = False
        Me.FrameStandServer.Text = "Настройка путей к базе данных АРМ Регистратор"
        '
        'TableLayoutPanelServer
        '
        Me.TableLayoutPanelServer.ColumnCount = 3
        Me.TableLayoutPanelServer.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69.0!))
        Me.TableLayoutPanelServer.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelServer.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanelServer.Controls.Add(Me.TextPathServer, 0, 1)
        Me.TableLayoutPanelServer.Controls.Add(Me.ComboPathServer, 0, 0)
        Me.TableLayoutPanelServer.Controls.Add(Me.LabelPathServer, 1, 0)
        Me.TableLayoutPanelServer.Controls.Add(Me.ButtonPathServerExplorer, 2, 1)
        Me.TableLayoutPanelServer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelServer.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanelServer.Name = "TableLayoutPanelServer"
        Me.TableLayoutPanelServer.RowCount = 2
        Me.TableLayoutPanelServer.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelServer.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelServer.Size = New System.Drawing.Size(400, 53)
        Me.TableLayoutPanelServer.TabIndex = 73
        '
        'TextPathServer
        '
        Me.TextPathServer.AcceptsReturn = True
        Me.TextPathServer.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanelServer.SetColumnSpan(Me.TextPathServer, 2)
        Me.TextPathServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextPathServer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextPathServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextPathServer.Location = New System.Drawing.Point(3, 29)
        Me.TextPathServer.MaxLength = 0
        Me.TextPathServer.Name = "TextPathServer"
        Me.TextPathServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextPathServer.Size = New System.Drawing.Size(362, 20)
        Me.TextPathServer.TabIndex = 57
        Me.ToolTip1.SetToolTip(Me.TextPathServer, "Путь к базе данных для выбранного стенда")
        '
        'ComboPathServer
        '
        Me.ComboPathServer.BackColor = System.Drawing.SystemColors.Window
        Me.ComboPathServer.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboPathServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPathServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboPathServer.Location = New System.Drawing.Point(3, 3)
        Me.ComboPathServer.Name = "ComboPathServer"
        Me.ComboPathServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboPathServer.Size = New System.Drawing.Size(61, 21)
        Me.ComboPathServer.TabIndex = 58
        Me.ToolTip1.SetToolTip(Me.ComboPathServer, "Выбор номера стенда, где программа работает в режиме Регистратор")
        '
        'LabelPathServer
        '
        Me.LabelPathServer.AutoSize = True
        Me.LabelPathServer.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanelServer.SetColumnSpan(Me.LabelPathServer, 2)
        Me.LabelPathServer.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPathServer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelPathServer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPathServer.Location = New System.Drawing.Point(72, 0)
        Me.LabelPathServer.Name = "LabelPathServer"
        Me.LabelPathServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPathServer.Size = New System.Drawing.Size(325, 26)
        Me.LabelPathServer.TabIndex = 59
        Me.LabelPathServer.Text = "Стенд АРМ Регистратор"
        Me.LabelPathServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ButtonPathServerExplorer
        '
        Me.ButtonPathServerExplorer.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonPathServerExplorer.BackgroundImage = CType(resources.GetObject("ButtonPathServerExplorer.BackgroundImage"), System.Drawing.Image)
        Me.ButtonPathServerExplorer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonPathServerExplorer.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonPathServerExplorer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonPathServerExplorer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonPathServerExplorer.Location = New System.Drawing.Point(371, 29)
        Me.ButtonPathServerExplorer.Name = "ButtonPathServerExplorer"
        Me.ButtonPathServerExplorer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonPathServerExplorer.Size = New System.Drawing.Size(25, 21)
        Me.ButtonPathServerExplorer.TabIndex = 56
        Me.ButtonPathServerExplorer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTip1.SetToolTip(Me.ButtonPathServerExplorer, "Настроить путь к базе данных для выбранного стенда")
        Me.ButtonPathServerExplorer.UseVisualStyleBackColor = False
        '
        'NumPrecision
        '
        Me.NumPrecision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.NumPrecision.Location = New System.Drawing.Point(283, 113)
        Me.NumPrecision.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.NumPrecision.Name = "NumPrecision"
        Me.NumPrecision.Size = New System.Drawing.Size(45, 20)
        Me.NumPrecision.TabIndex = 51
        Me.NumPrecision.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.NumPrecision, "Количество цифр после точки при отображении")
        Me.NumPrecision.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'LabelIntervalSnapshot
        '
        Me.LabelIntervalSnapshot.BackColor = System.Drawing.SystemColors.Control
        Me.LabelIntervalSnapshot.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelIntervalSnapshot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelIntervalSnapshot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelIntervalSnapshot.Location = New System.Drawing.Point(3, 78)
        Me.LabelIntervalSnapshot.Name = "LabelIntervalSnapshot"
        Me.LabelIntervalSnapshot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelIntervalSnapshot.Size = New System.Drawing.Size(274, 32)
        Me.LabelIntervalSnapshot.TabIndex = 47
        Me.LabelIntervalSnapshot.Text = "Длительность кадра:"
        Me.LabelIntervalSnapshot.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PanelKadr
        '
        Me.PanelKadr.Controls.Add(Me.ComboIntervalSnapshot)
        Me.PanelKadr.Controls.Add(Me.LabelIntervalSnapshotFact)
        Me.PanelKadr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelKadr.Location = New System.Drawing.Point(280, 81)
        Me.PanelKadr.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.PanelKadr.Name = "PanelKadr"
        Me.PanelKadr.Size = New System.Drawing.Size(129, 26)
        Me.PanelKadr.TabIndex = 73
        '
        'ComboIntervalSnapshot
        '
        Me.ComboIntervalSnapshot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboIntervalSnapshot.FormattingEnabled = True
        Me.ComboIntervalSnapshot.Location = New System.Drawing.Point(3, 3)
        Me.ComboIntervalSnapshot.Name = "ComboIntervalSnapshot"
        Me.ComboIntervalSnapshot.Size = New System.Drawing.Size(77, 21)
        Me.ComboIntervalSnapshot.TabIndex = 49
        Me.ToolTip1.SetToolTip(Me.ComboIntervalSnapshot, "Длительность кадра")
        Me.ComboIntervalSnapshot.Visible = False
        '
        'LabelIntervalSnapshotFact
        '
        Me.LabelIntervalSnapshotFact.BackColor = System.Drawing.SystemColors.Control
        Me.LabelIntervalSnapshotFact.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelIntervalSnapshotFact.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelIntervalSnapshotFact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelIntervalSnapshotFact.Location = New System.Drawing.Point(3, 3)
        Me.LabelIntervalSnapshotFact.Name = "LabelIntervalSnapshotFact"
        Me.LabelIntervalSnapshotFact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelIntervalSnapshotFact.Size = New System.Drawing.Size(77, 17)
        Me.LabelIntervalSnapshotFact.TabIndex = 48
        Me.LabelIntervalSnapshotFact.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ComboFrequencyCollection
        '
        Me.ComboFrequencyCollection.BackColor = System.Drawing.SystemColors.Window
        Me.ComboFrequencyCollection.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboFrequencyCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboFrequencyCollection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboFrequencyCollection.Location = New System.Drawing.Point(283, 3)
        Me.ComboFrequencyCollection.Name = "ComboFrequencyCollection"
        Me.ComboFrequencyCollection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboFrequencyCollection.Size = New System.Drawing.Size(77, 21)
        Me.ComboFrequencyCollection.TabIndex = 39
        Me.ToolTip1.SetToolTip(Me.ComboFrequencyCollection, "Частота сбора данных")
        '
        'LabelFrequencyCollection
        '
        Me.LabelFrequencyCollection.BackColor = System.Drawing.SystemColors.Control
        Me.LabelFrequencyCollection.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelFrequencyCollection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelFrequencyCollection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFrequencyCollection.Location = New System.Drawing.Point(3, 0)
        Me.LabelFrequencyCollection.Name = "LabelFrequencyCollection"
        Me.LabelFrequencyCollection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelFrequencyCollection.Size = New System.Drawing.Size(274, 26)
        Me.LabelFrequencyCollection.TabIndex = 40
        Me.LabelFrequencyCollection.Text = "Частота сбора данных Гц:"
        Me.LabelFrequencyCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelFrequencySamplingChannel
        '
        Me.LabelFrequencySamplingChannel.BackColor = System.Drawing.SystemColors.Control
        Me.LabelFrequencySamplingChannel.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelFrequencySamplingChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelFrequencySamplingChannel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFrequencySamplingChannel.Location = New System.Drawing.Point(3, 52)
        Me.LabelFrequencySamplingChannel.Name = "LabelFrequencySamplingChannel"
        Me.LabelFrequencySamplingChannel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelFrequencySamplingChannel.Size = New System.Drawing.Size(274, 26)
        Me.LabelFrequencySamplingChannel.TabIndex = 45
        Me.LabelFrequencySamplingChannel.Text = "Фактическая частота опроса канала Гц:"
        Me.LabelFrequencySamplingChannel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelDiscredit
        '
        Me.LabelDiscredit.BackColor = System.Drawing.SystemColors.Control
        Me.LabelDiscredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelDiscredit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelDiscredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelDiscredit.Location = New System.Drawing.Point(3, 26)
        Me.LabelDiscredit.Name = "LabelDiscredit"
        Me.LabelDiscredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDiscredit.Size = New System.Drawing.Size(274, 26)
        Me.LabelDiscredit.TabIndex = 43
        Me.LabelDiscredit.Text = "Степень передискретизации:"
        Me.LabelDiscredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelDiscreditFact
        '
        Me.LabelDiscreditFact.BackColor = System.Drawing.SystemColors.Control
        Me.LabelDiscreditFact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelDiscreditFact.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelDiscreditFact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelDiscreditFact.Location = New System.Drawing.Point(283, 26)
        Me.LabelDiscreditFact.Name = "LabelDiscreditFact"
        Me.LabelDiscreditFact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelDiscreditFact.Size = New System.Drawing.Size(77, 20)
        Me.LabelDiscreditFact.TabIndex = 44
        Me.LabelDiscreditFact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.LabelDiscreditFact, "Степень передискретизации")
        '
        'TextFrequencySamplingChannel
        '
        Me.TextFrequencySamplingChannel.BackColor = System.Drawing.SystemColors.Control
        Me.TextFrequencySamplingChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextFrequencySamplingChannel.Cursor = System.Windows.Forms.Cursors.Default
        Me.TextFrequencySamplingChannel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TextFrequencySamplingChannel.Location = New System.Drawing.Point(283, 52)
        Me.TextFrequencySamplingChannel.Name = "TextFrequencySamplingChannel"
        Me.TextFrequencySamplingChannel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextFrequencySamplingChannel.Size = New System.Drawing.Size(77, 20)
        Me.TextFrequencySamplingChannel.TabIndex = 46
        Me.TextFrequencySamplingChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.TextFrequencySamplingChannel, "Фактическая частота опроса канала")
        '
        'TabPageChannels
        '
        Me.TabPageChannels.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageChannels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPageChannels.Controls.Add(Me.PanelConstantChannels)
        Me.TabPageChannels.Controls.Add(Me.GroupBoxWeather)
        Me.TabPageChannels.Controls.Add(Me.GroupBoxNameConst)
        Me.TabPageChannels.ImageIndex = 5
        Me.TabPageChannels.Location = New System.Drawing.Point(4, 23)
        Me.TabPageChannels.Name = "TabPageChannels"
        Me.TabPageChannels.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageChannels.Size = New System.Drawing.Size(416, 354)
        Me.TabPageChannels.TabIndex = 4
        Me.TabPageChannels.Text = "Каналы"
        Me.TabPageChannels.ToolTipText = "Вкладка настроек отслеживаемых параметров"
        '
        'PanelConstantChannels
        '
        Me.PanelConstantChannels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelConstantChannels.Controls.Add(Me.DataGridViewConstantChannels)
        Me.PanelConstantChannels.Controls.Add(Me.BindingNavigatorConstantChannels)
        Me.PanelConstantChannels.Controls.Add(Me.LabelSelectedChannel)
        Me.PanelConstantChannels.Location = New System.Drawing.Point(5, 218)
        Me.PanelConstantChannels.Name = "PanelConstantChannels"
        Me.PanelConstantChannels.Size = New System.Drawing.Size(406, 131)
        Me.PanelConstantChannels.TabIndex = 80
        '
        'DataGridViewConstantChannels
        '
        Me.DataGridViewConstantChannels.AllowUserToAddRows = False
        Me.DataGridViewConstantChannels.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        Me.DataGridViewConstantChannels.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewConstantChannels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridViewConstantChannels.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.DataGridViewConstantChannels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewConstantChannels.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewConstantChannels.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewConstantChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewConstantChannels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewConstantChannels.Location = New System.Drawing.Point(0, 47)
        Me.DataGridViewConstantChannels.MultiSelect = False
        Me.DataGridViewConstantChannels.Name = "DataGridViewConstantChannels"
        Me.DataGridViewConstantChannels.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DataGridViewConstantChannels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewConstantChannels.Size = New System.Drawing.Size(402, 80)
        Me.DataGridViewConstantChannels.TabIndex = 36
        '
        'BindingNavigatorConstantChannels
        '
        Me.BindingNavigatorConstantChannels.AddNewItem = Me.BindingNavigatorAddNewItem
        Me.BindingNavigatorConstantChannels.CountItem = Me.BindingNavigatorCountItem
        Me.BindingNavigatorConstantChannels.DeleteItem = Me.BindingNavigatorDeleteItem
        Me.BindingNavigatorConstantChannels.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem})
        Me.BindingNavigatorConstantChannels.Location = New System.Drawing.Point(0, 22)
        Me.BindingNavigatorConstantChannels.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.BindingNavigatorConstantChannels.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.BindingNavigatorConstantChannels.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.BindingNavigatorConstantChannels.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.BindingNavigatorConstantChannels.Name = "BindingNavigatorConstantChannels"
        Me.BindingNavigatorConstantChannels.PositionItem = Me.BindingNavigatorPositionItem
        Me.BindingNavigatorConstantChannels.Size = New System.Drawing.Size(402, 25)
        Me.BindingNavigatorConstantChannels.TabIndex = 37
        Me.BindingNavigatorConstantChannels.Text = "BindingNavigatorConstantChannels"
        '
        'BindingNavigatorAddNewItem
        '
        Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
        Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorAddNewItem.Text = "Добавить"
        Me.BindingNavigatorAddNewItem.ToolTipText = "Добавить константный параметр"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(43, 22)
        Me.BindingNavigatorCountItem.Text = "для {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Общее число элементов"
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Удалить"
        Me.BindingNavigatorDeleteItem.ToolTipText = "Удалить константный параметр"
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
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'LabelSelectedChannel
        '
        Me.LabelSelectedChannel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LabelSelectedChannel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSelectedChannel.ForeColor = System.Drawing.Color.Blue
        Me.LabelSelectedChannel.Location = New System.Drawing.Point(0, 0)
        Me.LabelSelectedChannel.Name = "LabelSelectedChannel"
        Me.LabelSelectedChannel.Size = New System.Drawing.Size(402, 22)
        Me.LabelSelectedChannel.TabIndex = 34
        Me.LabelSelectedChannel.Text = "Назначить константные каналы"
        Me.LabelSelectedChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBoxWeather
        '
        Me.GroupBoxWeather.Controls.Add(Me.TableLayoutPanelControls)
        Me.GroupBoxWeather.ForeColor = System.Drawing.Color.Blue
        Me.GroupBoxWeather.Location = New System.Drawing.Point(6, 124)
        Me.GroupBoxWeather.Name = "GroupBoxWeather"
        Me.GroupBoxWeather.Size = New System.Drawing.Size(405, 91)
        Me.GroupBoxWeather.TabIndex = 79
        Me.GroupBoxWeather.TabStop = False
        Me.GroupBoxWeather.Text = "Контроль значения канала"
        '
        'TableLayoutPanelControls
        '
        Me.TableLayoutPanelControls.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble
        Me.TableLayoutPanelControls.ColumnCount = 2
        Me.TableLayoutPanelControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelControls.Controls.Add(Me.TextTbox, 1, 1)
        Me.TableLayoutPanelControls.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanelControls.Controls.Add(Me.Label2, 0, 0)
        Me.TableLayoutPanelControls.Controls.Add(Me.LabelTbox, 0, 1)
        Me.TableLayoutPanelControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelControls.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanelControls.Name = "TableLayoutPanelControls"
        Me.TableLayoutPanelControls.RowCount = 3
        Me.TableLayoutPanelControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelControls.Size = New System.Drawing.Size(399, 72)
        Me.TableLayoutPanelControls.TabIndex = 5
        '
        'TextTbox
        '
        Me.TextTbox.AcceptsReturn = True
        Me.TextTbox.BackColor = System.Drawing.SystemColors.Control
        Me.TextTbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextTbox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextTbox.Location = New System.Drawing.Point(204, 35)
        Me.TextTbox.MaxLength = 0
        Me.TextTbox.Name = "TextTbox"
        Me.TextTbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextTbox.Size = New System.Drawing.Size(80, 20)
        Me.TextTbox.TabIndex = 76
        Me.TextTbox.Text = "20"
        Me.ToolTip1.SetToolTip(Me.TextTbox, "Ввод температуры в случае отсутствия датчика температуры")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(204, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(189, 26)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Значение канала"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(6, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 26)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Имя канала"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelTbox
        '
        Me.LabelTbox.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTbox.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTbox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTbox.Location = New System.Drawing.Point(6, 32)
        Me.LabelTbox.Name = "LabelTbox"
        Me.LabelTbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTbox.Size = New System.Drawing.Size(189, 26)
        Me.LabelTbox.TabIndex = 77
        Me.LabelTbox.Text = "Температура бокса:"
        Me.LabelTbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBoxNameConst
        '
        Me.GroupBoxNameConst.Controls.Add(Me.PanelGrid)
        Me.GroupBoxNameConst.ForeColor = System.Drawing.Color.Blue
        Me.GroupBoxNameConst.Location = New System.Drawing.Point(5, 6)
        Me.GroupBoxNameConst.Name = "GroupBoxNameConst"
        Me.GroupBoxNameConst.Size = New System.Drawing.Size(406, 115)
        Me.GroupBoxNameConst.TabIndex = 78
        Me.GroupBoxNameConst.TabStop = False
        Me.GroupBoxNameConst.Text = "Использовать в расчётах значения каналов"
        '
        'PanelGrid
        '
        Me.PanelGrid.AutoScroll = True
        Me.PanelGrid.Controls.Add(Me.TableLayoutPanelAlias)
        Me.PanelGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelGrid.Location = New System.Drawing.Point(3, 16)
        Me.PanelGrid.Name = "PanelGrid"
        Me.PanelGrid.Size = New System.Drawing.Size(400, 96)
        Me.PanelGrid.TabIndex = 5
        '
        'TableLayoutPanelAlias
        '
        Me.TableLayoutPanelAlias.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble
        Me.TableLayoutPanelAlias.ColumnCount = 2
        Me.TableLayoutPanelAlias.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelAlias.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelAlias.Controls.Add(Me.PanelTbox, 1, 2)
        Me.TableLayoutPanelAlias.Controls.Add(Me.LabelNameChannel, 0, 0)
        Me.TableLayoutPanelAlias.Controls.Add(Me.PanelTxc, 1, 1)
        Me.TableLayoutPanelAlias.Controls.Add(Me.LabelNameParam, 0, 0)
        Me.TableLayoutPanelAlias.Controls.Add(Me.LabelTxc, 0, 1)
        Me.TableLayoutPanelAlias.Controls.Add(Me.LabelNameTbox, 0, 2)
        Me.TableLayoutPanelAlias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAlias.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelAlias.Name = "TableLayoutPanelAlias"
        Me.TableLayoutPanelAlias.RowCount = 3
        Me.TableLayoutPanelAlias.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanelAlias.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelAlias.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelAlias.Size = New System.Drawing.Size(400, 96)
        Me.TableLayoutPanelAlias.TabIndex = 4
        '
        'PanelTbox
        '
        Me.PanelTbox.Controls.Add(Me.ButtonFindChannelTbox)
        Me.PanelTbox.Controls.Add(Me.ComboBoxChannelTbox)
        Me.PanelTbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTbox.Location = New System.Drawing.Point(204, 68)
        Me.PanelTbox.Name = "PanelTbox"
        Me.PanelTbox.Size = New System.Drawing.Size(190, 24)
        Me.PanelTbox.TabIndex = 81
        '
        'ButtonFindChannelTbox
        '
        Me.ButtonFindChannelTbox.Image = CType(resources.GetObject("ButtonFindChannelTbox.Image"), System.Drawing.Image)
        Me.ButtonFindChannelTbox.Location = New System.Drawing.Point(163, 0)
        Me.ButtonFindChannelTbox.Name = "ButtonFindChannelTbox"
        Me.ButtonFindChannelTbox.Size = New System.Drawing.Size(24, 22)
        Me.ButtonFindChannelTbox.TabIndex = 82
        Me.ButtonFindChannelTbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannelTbox, "Быстрый поиск канала")
        Me.ButtonFindChannelTbox.UseVisualStyleBackColor = True
        '
        'ComboBoxChannelTbox
        '
        Me.ComboBoxChannelTbox.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxChannelTbox.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxChannelTbox.Dock = System.Windows.Forms.DockStyle.Left
        Me.ComboBoxChannelTbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxChannelTbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxChannelTbox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxChannelTbox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxChannelTbox.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxChannelTbox.MaxDropDownItems = 32
        Me.ComboBoxChannelTbox.Name = "ComboBoxChannelTbox"
        Me.ComboBoxChannelTbox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxChannelTbox.Size = New System.Drawing.Size(160, 21)
        Me.ComboBoxChannelTbox.TabIndex = 80
        Me.ToolTip1.SetToolTip(Me.ComboBoxChannelTbox, "Ввод имени канала измеряющего температуру бокса, <tбокса> по умолчанию.")
        '
        'LabelNameChannel
        '
        Me.LabelNameChannel.AutoSize = True
        Me.LabelNameChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameChannel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNameChannel.Location = New System.Drawing.Point(204, 3)
        Me.LabelNameChannel.Name = "LabelNameChannel"
        Me.LabelNameChannel.Size = New System.Drawing.Size(190, 26)
        Me.LabelNameChannel.TabIndex = 5
        Me.LabelNameChannel.Text = "Имя канала"
        Me.LabelNameChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelTxc
        '
        Me.PanelTxc.Controls.Add(Me.ButtonFindChannelTxc)
        Me.PanelTxc.Controls.Add(Me.ComboBoxTxc)
        Me.PanelTxc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTxc.Location = New System.Drawing.Point(204, 35)
        Me.PanelTxc.Name = "PanelTxc"
        Me.PanelTxc.Size = New System.Drawing.Size(190, 24)
        Me.PanelTxc.TabIndex = 80
        '
        'ButtonFindChannelTxc
        '
        Me.ButtonFindChannelTxc.Image = CType(resources.GetObject("ButtonFindChannelTxc.Image"), System.Drawing.Image)
        Me.ButtonFindChannelTxc.Location = New System.Drawing.Point(163, 0)
        Me.ButtonFindChannelTxc.Name = "ButtonFindChannelTxc"
        Me.ButtonFindChannelTxc.Size = New System.Drawing.Size(24, 22)
        Me.ButtonFindChannelTxc.TabIndex = 82
        Me.ButtonFindChannelTxc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannelTxc, "Быстрый поиск канала")
        Me.ButtonFindChannelTxc.UseVisualStyleBackColor = True
        '
        'ComboBoxTxc
        '
        Me.ComboBoxTxc.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxTxc.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxTxc.Dock = System.Windows.Forms.DockStyle.Left
        Me.ComboBoxTxc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxTxc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxTxc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxTxc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxTxc.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxTxc.MaxDropDownItems = 32
        Me.ComboBoxTxc.Name = "ComboBoxTxc"
        Me.ComboBoxTxc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxTxc.Size = New System.Drawing.Size(160, 21)
        Me.ComboBoxTxc.TabIndex = 74
        Me.ToolTip1.SetToolTip(Me.ComboBoxTxc, "Имя канала измеряющего температуру холодного спая, <Т хс> по умолчанию.")
        '
        'LabelNameParam
        '
        Me.LabelNameParam.AutoSize = True
        Me.LabelNameParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameParam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNameParam.Location = New System.Drawing.Point(6, 3)
        Me.LabelNameParam.Name = "LabelNameParam"
        Me.LabelNameParam.Size = New System.Drawing.Size(189, 26)
        Me.LabelNameParam.TabIndex = 4
        Me.LabelNameParam.Text = "Имя параметра"
        Me.LabelNameParam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelTxc
        '
        Me.LabelTxc.AutoSize = True
        Me.LabelTxc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTxc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTxc.Location = New System.Drawing.Point(6, 32)
        Me.LabelTxc.Name = "LabelTxc"
        Me.LabelTxc.Size = New System.Drawing.Size(189, 30)
        Me.LabelTxc.TabIndex = 2
        Me.LabelTxc.Text = "Температура холодного спая:"
        Me.LabelTxc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelNameTbox
        '
        Me.LabelNameTbox.AutoSize = True
        Me.LabelNameTbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNameTbox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNameTbox.Location = New System.Drawing.Point(6, 65)
        Me.LabelNameTbox.Name = "LabelNameTbox"
        Me.LabelNameTbox.Size = New System.Drawing.Size(189, 30)
        Me.LabelNameTbox.TabIndex = 0
        Me.LabelNameTbox.Text = "Температура бокса:"
        Me.LabelNameTbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Истребитель-48.png")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "gg_connecting.png")
        Me.ImageList1.Images.SetKeyName(5, "FileAssocTDMS.ico")
        Me.ImageList1.Images.SetKeyName(6, "")
        '
        'ButtonCompactRio
        '
        Me.ButtonCompactRio.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonCompactRio.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonCompactRio.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonCompactRio.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonCompactRio.Image = CType(resources.GetObject("ButtonCompactRio.Image"), System.Drawing.Image)
        Me.ButtonCompactRio.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonCompactRio.Location = New System.Drawing.Point(6, 36)
        Me.ButtonCompactRio.Name = "ButtonCompactRio"
        Me.ButtonCompactRio.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonCompactRio.Size = New System.Drawing.Size(402, 25)
        Me.ButtonCompactRio.TabIndex = 68
        Me.ButtonCompactRio.Text = "Мобильный ИВК CompactRio (измерение, запись и передача Клиентам)"
        Me.ButtonCompactRio.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.ButtonCompactRio, "Мобильный Измерительно-Вычислительный Комплекс на шасси CompactRio")
        Me.ButtonCompactRio.UseVisualStyleBackColor = False
        '
        'ButtonWithClient
        '
        Me.ButtonWithClient.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonWithClient.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonWithClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonWithClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonWithClient.Image = CType(resources.GetObject("ButtonWithClient.Image"), System.Drawing.Image)
        Me.ButtonWithClient.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonWithClient.Location = New System.Drawing.Point(6, 84)
        Me.ButtonWithClient.Name = "ButtonWithClient"
        Me.ButtonWithClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonWithClient.Size = New System.Drawing.Size(402, 25)
        Me.ButtonWithClient.TabIndex = 52
        Me.ButtonWithClient.Text = "Клиент (приём от Сервера и отображение)"
        Me.ButtonWithClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.ButtonWithClient, "Получение значений каналов от Сервера")
        Me.ButtonWithClient.UseVisualStyleBackColor = False
        '
        'ButtonWithSnapshop
        '
        Me.ButtonWithSnapshop.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonWithSnapshop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonWithSnapshop.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonWithSnapshop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonWithSnapshop.Image = CType(resources.GetObject("ButtonWithSnapshop.Image"), System.Drawing.Image)
        Me.ButtonWithSnapshop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonWithSnapshop.Location = New System.Drawing.Point(6, 60)
        Me.ButtonWithSnapshop.Name = "ButtonWithSnapshop"
        Me.ButtonWithSnapshop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonWithSnapshop.Size = New System.Drawing.Size(402, 25)
        Me.ButtonWithSnapshop.TabIndex = 21
        Me.ButtonWithSnapshop.Text = "Автономно (просмотр записей, смена текущей базы)"
        Me.ButtonWithSnapshop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.ButtonWithSnapshop, "Работа с записанными значениями сбора")
        Me.ButtonWithSnapshop.UseVisualStyleBackColor = False
        '
        'ButtonWithController
        '
        Me.ButtonWithController.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonWithController.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonWithController.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonWithController.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonWithController.Image = CType(resources.GetObject("ButtonWithController.Image"), System.Drawing.Image)
        Me.ButtonWithController.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonWithController.Location = New System.Drawing.Point(6, 12)
        Me.ButtonWithController.Name = "ButtonWithController"
        Me.ButtonWithController.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonWithController.Size = New System.Drawing.Size(402, 25)
        Me.ButtonWithController.TabIndex = 20
        Me.ButtonWithController.Text = "С контроллером DAQ (измерение, запись и передача Клиентам)"
        Me.ButtonWithController.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.ButtonWithController, "Работа с платами сбора и шасси DAQ (измерение, управление, запись и передача Клие" &
        "нтам)")
        Me.ButtonWithController.UseVisualStyleBackColor = False
        '
        'ButtonApply
        '
        Me.ButtonApply.BackColor = System.Drawing.Color.Orange
        Me.TableLayoutPanelTypeWork.SetColumnSpan(Me.ButtonApply, 2)
        Me.ButtonApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonApply.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonApply.Location = New System.Drawing.Point(3, 122)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonApply.Size = New System.Drawing.Size(414, 26)
        Me.ButtonApply.TabIndex = 66
        Me.ButtonApply.Text = "Ввести данные"
        Me.ToolTip1.SetToolTip(Me.ButtonApply, "Подтвердить для продолжения работы")
        Me.ButtonApply.UseVisualStyleBackColor = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'FrameTypeWork
        '
        Me.FrameTypeWork.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanelTypeWork.SetColumnSpan(Me.FrameTypeWork, 2)
        Me.FrameTypeWork.Controls.Add(Me.ButtonCompactRio)
        Me.FrameTypeWork.Controls.Add(Me.ButtonWithClient)
        Me.FrameTypeWork.Controls.Add(Me.ButtonWithSnapshop)
        Me.FrameTypeWork.Controls.Add(Me.ButtonWithController)
        Me.FrameTypeWork.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameTypeWork.ForeColor = System.Drawing.Color.Blue
        Me.FrameTypeWork.Location = New System.Drawing.Point(3, 3)
        Me.FrameTypeWork.Name = "FrameTypeWork"
        Me.FrameTypeWork.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameTypeWork.Size = New System.Drawing.Size(414, 113)
        Me.FrameTypeWork.TabIndex = 67
        Me.FrameTypeWork.TabStop = False
        Me.FrameTypeWork.Text = "Тип работы"
        '
        'hpPlainHTML
        '
        Me.hpPlainHTML.HelpNamespace = "..\..\help.htm"
        '
        'TableLayoutPanelTypeWork
        '
        Me.TableLayoutPanelTypeWork.ColumnCount = 2
        Me.TableLayoutPanelTypeWork.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelTypeWork.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelTypeWork.Controls.Add(Me.ButtonApply, 0, 1)
        Me.TableLayoutPanelTypeWork.Controls.Add(Me.FrameTypeWork, 0, 0)
        Me.TableLayoutPanelTypeWork.Location = New System.Drawing.Point(12, 399)
        Me.TableLayoutPanelTypeWork.Name = "TableLayoutPanelTypeWork"
        Me.TableLayoutPanelTypeWork.RowCount = 2
        Me.TableLayoutPanelTypeWork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119.0!))
        Me.TableLayoutPanelTypeWork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.TableLayoutPanelTypeWork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelTypeWork.Size = New System.Drawing.Size(420, 152)
        Me.TableLayoutPanelTypeWork.TabIndex = 71
        '
        'ImageListChannel
        '
        Me.ImageListChannel.ImageStream = CType(resources.GetObject("ImageListChannel.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListChannel.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListChannel.Images.SetKeyName(0, "")
        Me.ImageListChannel.Images.SetKeyName(1, "")
        Me.ImageListChannel.Images.SetKeyName(2, "")
        Me.ImageListChannel.Images.SetKeyName(3, "")
        Me.ImageListChannel.Images.SetKeyName(4, "")
        Me.ImageListChannel.Images.SetKeyName(5, "")
        Me.ImageListChannel.Images.SetKeyName(6, "")
        Me.ImageListChannel.Images.SetKeyName(7, "")
        Me.ImageListChannel.Images.SetKeyName(8, "")
        Me.ImageListChannel.Images.SetKeyName(9, "")
        Me.ImageListChannel.Images.SetKeyName(10, "")
        Me.ImageListChannel.Images.SetKeyName(11, "")
        Me.ImageListChannel.Images.SetKeyName(12, "")
        '
        'PanelSettingCompactRio
        '
        Me.TableLayoutPanelProduct.SetColumnSpan(Me.PanelSettingCompactRio, 2)
        Me.PanelSettingCompactRio.Controls.Add(Me.PictureBox2)
        Me.PanelSettingCompactRio.Controls.Add(Me.LinkLabelSettingCompactRio)
        Me.PanelSettingCompactRio.Location = New System.Drawing.Point(100, 209)
        Me.PanelSettingCompactRio.Margin = New System.Windows.Forms.Padding(100, 3, 3, 3)
        Me.PanelSettingCompactRio.Name = "PanelSettingCompactRio"
        Me.PanelSettingCompactRio.Size = New System.Drawing.Size(247, 39)
        Me.PanelSettingCompactRio.TabIndex = 80
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'FormSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(445, 552)
        Me.Controls.Add(Me.TableLayoutPanelTypeWork)
        Me.Controls.Add(Me.TabOption)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настройка"
        Me.TopMost = True
        Me.TabOption.ResumeLayout(False)
        Me.TabPageProduct.ResumeLayout(False)
        Me.TableLayoutPanelProduct.ResumeLayout(False)
        Me.TableLayoutPanelProduct.PerformLayout()
        Me.FraProduct.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.TabPageStartStop.ResumeLayout(False)
        Me.TableLayoutPanelStartStop.ResumeLayout(False)
        Me.TableLayoutPanelStartStop.PerformLayout()
        Me.PanelNameVarStopWrite.ResumeLayout(False)
        Me.PanelNameVarStartWrite.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageMeasure.ResumeLayout(False)
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.FrameStandClient.ResumeLayout(False)
        Me.TableLayoutPanelClient.ResumeLayout(False)
        Me.TableLayoutPanelClient.PerformLayout()
        Me.FrameStandServer.ResumeLayout(False)
        Me.TableLayoutPanelServer.ResumeLayout(False)
        Me.TableLayoutPanelServer.PerformLayout()
        CType(Me.NumPrecision, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelKadr.ResumeLayout(False)
        Me.TabPageChannels.ResumeLayout(False)
        Me.PanelConstantChannels.ResumeLayout(False)
        Me.PanelConstantChannels.PerformLayout()
        CType(Me.DataGridViewConstantChannels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorConstantChannels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorConstantChannels.ResumeLayout(False)
        Me.BindingNavigatorConstantChannels.PerformLayout()
        Me.GroupBoxWeather.ResumeLayout(False)
        Me.TableLayoutPanelControls.ResumeLayout(False)
        Me.TableLayoutPanelControls.PerformLayout()
        Me.GroupBoxNameConst.ResumeLayout(False)
        Me.PanelGrid.ResumeLayout(False)
        Me.TableLayoutPanelAlias.ResumeLayout(False)
        Me.TableLayoutPanelAlias.PerformLayout()
        Me.PanelTbox.ResumeLayout(False)
        Me.PanelTxc.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FrameTypeWork.ResumeLayout(False)
        Me.TableLayoutPanelTypeWork.ResumeLayout(False)
        Me.PanelSettingCompactRio.ResumeLayout(False)
        Me.PanelSettingCompactRio.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents OpenFileDialogPuth As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents hpPlainHTML As System.Windows.Forms.HelpProvider
    Public WithEvents FrameTypeWork As System.Windows.Forms.GroupBox
    Public WithEvents ButtonWithClient As System.Windows.Forms.RadioButton
    Public WithEvents ButtonWithSnapshop As System.Windows.Forms.RadioButton
    Public WithEvents ButtonWithController As System.Windows.Forms.RadioButton
    Public WithEvents ButtonApply As System.Windows.Forms.Button
    Friend WithEvents TabOption As System.Windows.Forms.TabControl
    Friend WithEvents TabPageProduct As System.Windows.Forms.TabPage
    Friend WithEvents DTPickerDate As System.Windows.Forms.DateTimePicker
    Public WithEvents LabelDate As System.Windows.Forms.Label
    Public WithEvents LabelStend As System.Windows.Forms.Label
    Friend WithEvents FraProduct As System.Windows.Forms.GroupBox
    Public WithEvents TextNumberProduct As System.Windows.Forms.TextBox
    Public WithEvents LabelNumberProduct As System.Windows.Forms.Label
    Public WithEvents LabelEngine As System.Windows.Forms.Label
    Public WithEvents ComboEngine As System.Windows.Forms.ComboBox
    Public WithEvents ComboNumberStand As System.Windows.Forms.ComboBox
    Friend WithEvents TabPageMeasure As System.Windows.Forms.TabPage
    Friend WithEvents ComboIntervalSnapshot As System.Windows.Forms.ComboBox
    Public WithEvents ComboFrequencyCollection As System.Windows.Forms.ComboBox
    Public WithEvents TextFrequencySamplingChannel As System.Windows.Forms.Label
    Public WithEvents LabelIntervalSnapshotFact As System.Windows.Forms.Label
    Public WithEvents LabelIntervalSnapshot As System.Windows.Forms.Label
    Public WithEvents LabelFrequencySamplingChannel As System.Windows.Forms.Label
    Public WithEvents LabelDiscreditFact As System.Windows.Forms.Label
    Public WithEvents LabelDiscredit As System.Windows.Forms.Label
    Public WithEvents LabelFrequencyCollection As System.Windows.Forms.Label
    Public WithEvents FrameStandServer As System.Windows.Forms.GroupBox
    Public WithEvents ComboPathServer As System.Windows.Forms.ComboBox
    Public WithEvents TextPathServer As System.Windows.Forms.TextBox
    Public WithEvents ButtonPathServerExplorer As System.Windows.Forms.Button
    Public WithEvents LabelPathServer As System.Windows.Forms.Label
    Public WithEvents FrameStandClient As System.Windows.Forms.GroupBox
    Public WithEvents ButtonPathClientExplorer As System.Windows.Forms.Button
    Public WithEvents TextPathClient As System.Windows.Forms.TextBox
    Public WithEvents ComboPathClient As System.Windows.Forms.ComboBox
    Public WithEvents LabelPathClient As System.Windows.Forms.Label
    Friend WithEvents NumPrecision As System.Windows.Forms.NumericUpDown
    Public WithEvents LabelPrecision As System.Windows.Forms.Label
    Public WithEvents TextTbox As System.Windows.Forms.TextBox
    Public WithEvents LabelTbox As System.Windows.Forms.Label
    Friend WithEvents TabPageStartStop As System.Windows.Forms.TabPage
    Friend WithEvents TextWaitStopWrite As System.Windows.Forms.TextBox
    Friend WithEvents LabelWaitStopWrite As System.Windows.Forms.Label
    Friend WithEvents TextValueStopWrite As System.Windows.Forms.TextBox
    Friend WithEvents LabelValueStopWrite As System.Windows.Forms.Label
    Friend WithEvents LabelNameVarStop As System.Windows.Forms.Label
    Friend WithEvents LabelSeparate As System.Windows.Forms.Label
    Friend WithEvents TextWaitStartWrite As System.Windows.Forms.TextBox
    Friend WithEvents LabelWaitStartWrite As System.Windows.Forms.Label
    Friend WithEvents LabelNameVarStartWrite As System.Windows.Forms.Label
    Public WithEvents ButtonCompactRio As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBoxNameConst As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanelAlias As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LabelNameTbox As System.Windows.Forms.Label
    Friend WithEvents LabelTxc As System.Windows.Forms.Label
    Friend WithEvents LabelNameChannel As System.Windows.Forms.Label
    Friend WithEvents LabelNameParam As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TableLayoutPanelProduct As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTypeWork As TableLayoutPanel
    Friend WithEvents TableLayoutPanelStartStop As TableLayoutPanel
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents TableLayoutPanelClient As TableLayoutPanel
    Friend WithEvents TableLayoutPanelServer As TableLayoutPanel
    Friend WithEvents PanelKadr As Panel
    Public WithEvents ComboTypeKrd As ComboBox
    Friend WithEvents LabelTypeKrd As Label
    Public WithEvents ComboRegime As ComboBox
    Friend WithEvents LabelRegime As Label
    Friend WithEvents PanelGrid As Panel
    Friend WithEvents TabPageChannels As TabPage
    Friend WithEvents GroupBoxWeather As GroupBox
    Friend WithEvents TableLayoutPanelControls As TableLayoutPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ImageListChannel As ImageList
    Public WithEvents ComboBoxNameVarStartWrite As ComboBox
    Public WithEvents ComboBoxNameVarStopWrite As ComboBox
    Public WithEvents ComboBoxChannelTbox As ComboBox
    Public WithEvents ComboBoxTxc As ComboBox
    Friend WithEvents ComboBoxCountClientOrNumberClient As ComboBox
    Public WithEvents LabelComboBoxCountClientOrNumberClient As Label
    Friend WithEvents PanelNameVarStartWrite As Panel
    Friend WithEvents ButtonFindChannelStartWrite As Button
    Friend WithEvents PanelNameVarStopWrite As Panel
    Friend WithEvents ButtonFindChannelStopWrite As Button
    Friend WithEvents PanelTbox As Panel
    Friend WithEvents ButtonFindChannelTbox As Button
    Friend WithEvents PanelTxc As Panel
    Friend WithEvents ButtonFindChannelTxc As Button
    Friend WithEvents PanelConstantChannels As Panel
    Friend WithEvents LabelSelectedChannel As Label
    Public WithEvents DataGridViewConstantChannels As DataGridView
    Friend WithEvents BindingNavigatorAddNewItem As ToolStripButton
    Friend WithEvents BindingNavigatorCountItem As ToolStripLabel
    Friend WithEvents BindingNavigatorDeleteItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveFirstItem As ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As ToolStripSeparator
    Public WithEvents BindingNavigatorConstantChannels As BindingNavigator
    Friend WithEvents LinkLabelSettingCompactRio As LinkLabel
    Friend WithEvents PanelSettingCompactRio As Panel
    Friend WithEvents PictureBox2 As PictureBox
End Class
