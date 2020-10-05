<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTestChannel
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTestChannel))
        Dim ScaleCustomDivision40 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision41 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision42 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision43 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision44 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision45 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision46 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision47 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision48 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision49 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision50 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision51 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim ScaleCustomDivision52 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Me.LabelPhisic = New System.Windows.Forms.Label()
        Me.peakPower = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.rateNumeric = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.peakFrequency = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.SettingsGroupBox = New System.Windows.Forms.GroupBox()
        Me.window = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.units = New System.Windows.Forms.ComboBox()
        Me.unitsLabel = New System.Windows.Forms.Label()
        Me.scalecomboBox = New System.Windows.Forms.ComboBox()
        Me.scaleLabel = New System.Windows.Forms.Label()
        Me.samplesNumeric = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.AcquisitionGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.AcquisitionStateSwitch = New NationalInstruments.UI.WindowsForms.Switch()
        Me.WaveformGraph = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.waveformPlot = New NationalInstruments.UI.WaveformPlot()
        Me.waveformXAxis = New NationalInstruments.UI.XAxis()
        Me.waveformYAxis = New NationalInstruments.UI.YAxis()
        Me.powerSpectrumGraph = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.XYCursor = New NationalInstruments.UI.XYCursor()
        Me.powerSpectrumPlot = New NationalInstruments.UI.WaveformPlot()
        Me.powerSpectrumxAxis = New NationalInstruments.UI.XAxis()
        Me.powerSpectrumYAxis = New NationalInstruments.UI.YAxis()
        Me.YAxis2 = New NationalInstruments.UI.YAxis()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.XAxis2 = New NationalInstruments.UI.XAxis()
        Me.WaveformPlot2 = New NationalInstruments.UI.WaveformPlot()
        Me.CWGraph2 = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.HScale = New NationalInstruments.UI.WindowsForms.Knob()
        Me.VScaletext = New System.Windows.Forms.TextBox()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me.TriggerLED = New NationalInstruments.UI.WindowsForms.Led()
        Me.ResetOsc = New System.Windows.Forms.Button()
        Me.Trigger = New System.Windows.Forms.Button()
        Me.VOffsetText = New System.Windows.Forms.TextBox()
        Me.HScaleText = New System.Windows.Forms.TextBox()
        Me.VOffset = New NationalInstruments.UI.WindowsForms.Knob()
        Me.Метка4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.VScale = New NationalInstruments.UI.WindowsForms.Knob()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.CWGraph1 = New NationalInstruments.UI.WindowsForms.WaveformGraph()
        Me.WaveformPlot1 = New NationalInstruments.UI.WaveformPlot()
        Me.XAxis1 = New NationalInstruments.UI.XAxis()
        Me.YAxis1 = New NationalInstruments.UI.YAxis()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Acquire = New System.Windows.Forms.Button()
        Me.ButtonHide = New System.Windows.Forms.Button()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.CWNumEdit2 = New System.Windows.Forms.TextBox()
        Me.CWNumEdit1 = New System.Windows.Forms.TextBox()
        Me.TabControlTools = New System.Windows.Forms.TabControl()
        Me.Метка3 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ComboBoxParameters = New System.Windows.Forms.ComboBox()
        Me.ButtonFindChannel = New System.Windows.Forms.Button()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.peakPower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.peakFrequency, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SettingsGroupBox.SuspendLayout()
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.AcquisitionGroupBox.SuspendLayout()
        CType(Me.AcquisitionStateSwitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WaveformGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerSpectrumGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XYCursor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CWGraph2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HScale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame3.SuspendLayout()
        CType(Me.TriggerLED, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        CType(Me.VScale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame1.SuspendLayout()
        CType(Me.CWGraph1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabControlTools.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelPhisic
        '
        Me.LabelPhisic.BackColor = System.Drawing.Color.Black
        Me.LabelPhisic.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPhisic.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelPhisic.ForeColor = System.Drawing.Color.Lime
        Me.LabelPhisic.Location = New System.Drawing.Point(222, 48)
        Me.LabelPhisic.Name = "LabelPhisic"
        Me.LabelPhisic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPhisic.Size = New System.Drawing.Size(192, 24)
        Me.LabelPhisic.TabIndex = 33
        Me.LabelPhisic.Text = "Физическая величина"
        '
        'peakPower
        '
        Me.peakPower.BackColor = System.Drawing.SystemColors.Control
        Me.peakPower.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(9)
        Me.peakPower.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator
        Me.peakPower.Location = New System.Drawing.Point(662, 462)
        Me.peakPower.Name = "peakPower"
        Me.peakPower.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.peakPower.Size = New System.Drawing.Size(88, 20)
        Me.peakPower.TabIndex = 52
        Me.peakPower.TabStop = False
        '
        'rateNumeric
        '
        Me.rateNumeric.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval
        Me.rateNumeric.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0)
        Me.rateNumeric.Location = New System.Drawing.Point(31, 48)
        Me.rateNumeric.Name = "rateNumeric"
        Me.rateNumeric.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.rateNumeric.Range = New NationalInstruments.UI.Range(1.0R, 33554432.0R)
        Me.rateNumeric.Size = New System.Drawing.Size(89, 20)
        Me.rateNumeric.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.rateNumeric, "Скорость сбора (Hz)")
        Me.rateNumeric.Value = 1024.0R
        '
        'peakFrequency
        '
        Me.peakFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.peakFrequency.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(4)
        Me.peakFrequency.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Indicator
        Me.peakFrequency.Location = New System.Drawing.Point(334, 462)
        Me.peakFrequency.Name = "peakFrequency"
        Me.peakFrequency.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.peakFrequency.Size = New System.Drawing.Size(88, 20)
        Me.peakFrequency.TabIndex = 54
        Me.peakFrequency.TabStop = False
        '
        'SettingsGroupBox
        '
        Me.SettingsGroupBox.BackColor = System.Drawing.Color.Silver
        Me.SettingsGroupBox.Controls.Add(Me.window)
        Me.SettingsGroupBox.Controls.Add(Me.Label6)
        Me.SettingsGroupBox.Controls.Add(Me.units)
        Me.SettingsGroupBox.Controls.Add(Me.unitsLabel)
        Me.SettingsGroupBox.Controls.Add(Me.scalecomboBox)
        Me.SettingsGroupBox.Controls.Add(Me.scaleLabel)
        Me.SettingsGroupBox.Location = New System.Drawing.Point(8, 192)
        Me.SettingsGroupBox.Name = "SettingsGroupBox"
        Me.SettingsGroupBox.Size = New System.Drawing.Size(144, 176)
        Me.SettingsGroupBox.TabIndex = 47
        Me.SettingsGroupBox.TabStop = False
        Me.SettingsGroupBox.Text = "Настройки дисплея"
        '
        'window
        '
        Me.window.Items.AddRange(New Object() {"Hamming", "Rectangular", "Hanning", "Blackman-Harris", "Exact Blackman", "Blackman", "FlatTop", "4Term B-Harris", "7Term B-Harris"})
        Me.window.Location = New System.Drawing.Point(31, 48)
        Me.window.Name = "window"
        Me.window.Size = New System.Drawing.Size(89, 21)
        Me.window.TabIndex = 1
        Me.window.Text = "Hamming"
        Me.ToolTip1.SetToolTip(Me.window, "Окно цифрового фильтра")
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(16, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 32)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Окно цифрового фильтра"
        '
        'units
        '
        Me.units.Items.AddRange(New Object() {"Vrms", "Vrms^2", "Vrms/rt(Hz)", "Vpk^2/Hz", "Vpk", "Vpk^2", "Vpk/rt(Hz)", "Vrms^2/Hz"})
        Me.units.Location = New System.Drawing.Point(31, 96)
        Me.units.Name = "units"
        Me.units.Size = New System.Drawing.Size(89, 21)
        Me.units.TabIndex = 3
        Me.units.Text = "Vrms"
        Me.ToolTip1.SetToolTip(Me.units, "Единица измерения")
        '
        'unitsLabel
        '
        Me.unitsLabel.Location = New System.Drawing.Point(16, 80)
        Me.unitsLabel.Name = "unitsLabel"
        Me.unitsLabel.Size = New System.Drawing.Size(112, 16)
        Me.unitsLabel.TabIndex = 2
        Me.unitsLabel.Text = "Единица измерения"
        '
        'scalecomboBox
        '
        Me.scalecomboBox.Items.AddRange(New Object() {"dB", "dBm", "Linear"})
        Me.scalecomboBox.Location = New System.Drawing.Point(31, 144)
        Me.scalecomboBox.Name = "scalecomboBox"
        Me.scalecomboBox.Size = New System.Drawing.Size(89, 21)
        Me.scalecomboBox.TabIndex = 5
        Me.scalecomboBox.Text = "dB"
        Me.ToolTip1.SetToolTip(Me.scalecomboBox, "Масштаб шкалы")
        '
        'scaleLabel
        '
        Me.scaleLabel.Location = New System.Drawing.Point(16, 128)
        Me.scaleLabel.Name = "scaleLabel"
        Me.scaleLabel.Size = New System.Drawing.Size(112, 16)
        Me.scaleLabel.TabIndex = 4
        Me.scaleLabel.Text = "Масштаб шкалы"
        '
        'samplesNumeric
        '
        Me.samplesNumeric.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(0)
        Me.samplesNumeric.Location = New System.Drawing.Point(31, 112)
        Me.samplesNumeric.Name = "samplesNumeric"
        Me.samplesNumeric.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.samplesNumeric.Range = New NationalInstruments.UI.Range(1.0R, 6666666.0R)
        Me.samplesNumeric.Size = New System.Drawing.Size(89, 20)
        Me.samplesNumeric.TabIndex = 3
        Me.samplesNumeric.Value = 1024.0R
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(24, 464)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(128, 16)
        Me.label4.TabIndex = 49
        Me.label4.Text = "Измерение Вкл/Выкл"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Длина выборки"
        Me.ToolTip1.SetToolTip(Me.Label3, "Длина выборки в замерах")
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.DarkGray
        Me.TabPage3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPage3.Controls.Add(Me.AcquisitionGroupBox)
        Me.TabPage3.Controls.Add(Me.Label13)
        Me.TabPage3.Controls.Add(Me.Label14)
        Me.TabPage3.Controls.Add(Me.AcquisitionStateSwitch)
        Me.TabPage3.Controls.Add(Me.label4)
        Me.TabPage3.Controls.Add(Me.peakPower)
        Me.TabPage3.Controls.Add(Me.peakFrequency)
        Me.TabPage3.Controls.Add(Me.SettingsGroupBox)
        Me.TabPage3.Controls.Add(Me.WaveformGraph)
        Me.TabPage3.Controls.Add(Me.powerSpectrumGraph)
        Me.TabPage3.ImageIndex = 2
        Me.TabPage3.Location = New System.Drawing.Point(4, 23)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(768, 493)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Спектр"
        Me.ToolTip1.SetToolTip(Me.TabPage3, "Виртуальный анализатор спектра")
        '
        'AcquisitionGroupBox
        '
        Me.AcquisitionGroupBox.BackColor = System.Drawing.Color.Silver
        Me.AcquisitionGroupBox.Controls.Add(Me.rateNumeric)
        Me.AcquisitionGroupBox.Controls.Add(Me.samplesNumeric)
        Me.AcquisitionGroupBox.Controls.Add(Me.Label3)
        Me.AcquisitionGroupBox.Controls.Add(Me.Label5)
        Me.AcquisitionGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.AcquisitionGroupBox.Name = "AcquisitionGroupBox"
        Me.AcquisitionGroupBox.Size = New System.Drawing.Size(144, 176)
        Me.AcquisitionGroupBox.TabIndex = 55
        Me.AcquisitionGroupBox.TabStop = False
        Me.AcquisitionGroupBox.Text = "Настройки измерения"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(16, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 16)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Скорость сбора (Hz):"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.DarkGray
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label13.Location = New System.Drawing.Point(224, 464)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(104, 16)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "Частота пика:"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.DarkGray
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label14.Location = New System.Drawing.Point(552, 464)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(104, 16)
        Me.Label14.TabIndex = 24
        Me.Label14.Text = "Мощность пика:"
        '
        'AcquisitionStateSwitch
        '
        Me.AcquisitionStateSwitch.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left
        Me.AcquisitionStateSwitch.Location = New System.Drawing.Point(56, 376)
        Me.AcquisitionStateSwitch.Name = "AcquisitionStateSwitch"
        Me.AcquisitionStateSwitch.OffColor = System.Drawing.Color.OrangeRed
        Me.AcquisitionStateSwitch.OnColor = System.Drawing.Color.LawnGreen
        Me.AcquisitionStateSwitch.Size = New System.Drawing.Size(56, 80)
        Me.AcquisitionStateSwitch.SwitchStyle = NationalInstruments.UI.SwitchStyle.VerticalToggle3D
        Me.AcquisitionStateSwitch.TabIndex = 50
        '
        'WaveformGraph
        '
        Me.WaveformGraph.BackColor = System.Drawing.Color.Silver
        Me.WaveformGraph.Border = NationalInstruments.UI.Border.Raised
        Me.WaveformGraph.Caption = "График сигнала"
        Me.WaveformGraph.Location = New System.Drawing.Point(168, 8)
        Me.WaveformGraph.Name = "WaveformGraph"
        Me.WaveformGraph.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.waveformPlot})
        Me.WaveformGraph.Size = New System.Drawing.Size(584, 176)
        Me.WaveformGraph.TabIndex = 45
        Me.WaveformGraph.TabStop = False
        Me.WaveformGraph.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.waveformXAxis})
        Me.WaveformGraph.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.waveformYAxis})
        '
        'waveformPlot
        '
        Me.waveformPlot.LineColor = System.Drawing.Color.Gold
        Me.waveformPlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.waveformPlot.XAxis = Me.waveformXAxis
        Me.waveformPlot.YAxis = Me.waveformYAxis
        '
        'waveformXAxis
        '
        Me.waveformXAxis.MajorDivisions.GridVisible = True
        Me.waveformXAxis.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.waveformXAxis.Range = New NationalInstruments.UI.Range(0R, 500.0R)
        '
        'waveformYAxis
        '
        Me.waveformYAxis.Caption = "Volts"
        Me.waveformYAxis.MajorDivisions.GridVisible = True
        '
        'powerSpectrumGraph
        '
        Me.powerSpectrumGraph.BackColor = System.Drawing.Color.Silver
        Me.powerSpectrumGraph.Border = NationalInstruments.UI.Border.Raised
        Me.powerSpectrumGraph.Caption = "Спектральная плотность мощности сигнала"
        Me.powerSpectrumGraph.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XYCursor})
        Me.powerSpectrumGraph.Location = New System.Drawing.Point(168, 192)
        Me.powerSpectrumGraph.Name = "powerSpectrumGraph"
        Me.powerSpectrumGraph.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.powerSpectrumPlot})
        Me.powerSpectrumGraph.Size = New System.Drawing.Size(584, 264)
        Me.powerSpectrumGraph.TabIndex = 48
        Me.powerSpectrumGraph.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.powerSpectrumxAxis})
        Me.powerSpectrumGraph.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.powerSpectrumYAxis})
        '
        'XYCursor
        '
        Me.XYCursor.Color = System.Drawing.Color.Magenta
        Me.XYCursor.Plot = Me.powerSpectrumPlot
        '
        'powerSpectrumPlot
        '
        Me.powerSpectrumPlot.LineColor = System.Drawing.Color.Yellow
        Me.powerSpectrumPlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor
        Me.powerSpectrumPlot.XAxis = Me.powerSpectrumxAxis
        Me.powerSpectrumPlot.YAxis = Me.powerSpectrumYAxis
        '
        'powerSpectrumxAxis
        '
        Me.powerSpectrumxAxis.AutoMinorDivisionFrequency = 10
        Me.powerSpectrumxAxis.Caption = "Hertz"
        Me.powerSpectrumxAxis.MajorDivisions.GridColor = System.Drawing.Color.DodgerBlue
        Me.powerSpectrumxAxis.MajorDivisions.GridVisible = True
        Me.powerSpectrumxAxis.MajorDivisions.Interval = 10.0R
        Me.powerSpectrumxAxis.MinorDivisions.GridColor = System.Drawing.Color.Green
        Me.powerSpectrumxAxis.MinorDivisions.GridVisible = True
        Me.powerSpectrumxAxis.MinorDivisions.Interval = 100.0R
        Me.powerSpectrumxAxis.MinorDivisions.TickVisible = True
        Me.powerSpectrumxAxis.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.powerSpectrumxAxis.Range = New NationalInstruments.UI.Range(0R, 500.0R)
        '
        'powerSpectrumYAxis
        '
        Me.powerSpectrumYAxis.Caption = "Vms"
        Me.powerSpectrumYAxis.MajorDivisions.GridColor = System.Drawing.Color.DodgerBlue
        Me.powerSpectrumYAxis.MajorDivisions.GridVisible = True
        Me.powerSpectrumYAxis.MinorDivisions.GridColor = System.Drawing.Color.Green
        Me.powerSpectrumYAxis.MinorDivisions.GridVisible = True
        Me.powerSpectrumYAxis.Range = New NationalInstruments.UI.Range(-2.0R, 8.0R)
        '
        'YAxis2
        '
        Me.YAxis2.Caption = "Volts"
        Me.YAxis2.MajorDivisions.GridVisible = True
        Me.YAxis2.Mode = NationalInstruments.UI.AxisMode.Fixed
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        '
        'XAxis2
        '
        Me.XAxis2.MajorDivisions.GridVisible = True
        Me.XAxis2.MajorDivisions.Interval = 10.0R
        Me.XAxis2.MajorDivisions.LabelVisible = False
        Me.XAxis2.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.XAxis2.Range = New NationalInstruments.UI.Range(0R, 200.0R)
        '
        'WaveformPlot2
        '
        Me.WaveformPlot2.XAxis = Me.XAxis2
        Me.WaveformPlot2.YAxis = Me.YAxis2
        '
        'CWGraph2
        '
        Me.CWGraph2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.CWGraph2.Border = NationalInstruments.UI.Border.ThinFrame3D
        Me.CWGraph2.Location = New System.Drawing.Point(8, 8)
        Me.CWGraph2.Name = "CWGraph2"
        Me.CWGraph2.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.WaveformPlot2})
        Me.CWGraph2.Size = New System.Drawing.Size(536, 416)
        Me.CWGraph2.TabIndex = 47
        Me.CWGraph2.TabStop = False
        Me.CWGraph2.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis2})
        Me.CWGraph2.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis2})
        '
        'HScale
        '
        Me.HScale.Border = NationalInstruments.UI.Border.ThinFrame3D
        Me.HScale.Caption = "Масштаб"
        Me.HScale.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        ScaleCustomDivision40.LineWidth = 2.0!
        ScaleCustomDivision40.Text = "100 ms/Div"
        ScaleCustomDivision40.TickLength = 7.0!
        ScaleCustomDivision41.LineWidth = 2.0!
        ScaleCustomDivision41.Text = "50 ms/Div"
        ScaleCustomDivision41.TickLength = 7.0!
        ScaleCustomDivision41.Value = 1.0R
        ScaleCustomDivision42.LineWidth = 2.0!
        ScaleCustomDivision42.Text = "20 ms/Div"
        ScaleCustomDivision42.TickLength = 7.0!
        ScaleCustomDivision42.Value = 2.0R
        ScaleCustomDivision43.LineWidth = 2.0!
        ScaleCustomDivision43.Text = "10 ms/Div"
        ScaleCustomDivision43.TickLength = 7.0!
        ScaleCustomDivision43.Value = 3.0R
        ScaleCustomDivision44.LineWidth = 2.0!
        ScaleCustomDivision44.Text = "5 ms/Div"
        ScaleCustomDivision44.TickLength = 7.0!
        ScaleCustomDivision44.Value = 4.0R
        ScaleCustomDivision45.LineWidth = 2.0!
        ScaleCustomDivision45.Text = "2 ms/Div"
        ScaleCustomDivision45.TickLength = 7.0!
        ScaleCustomDivision45.Value = 5.0R
        ScaleCustomDivision46.LineWidth = 2.0!
        ScaleCustomDivision46.Text = "1 ms/Div"
        ScaleCustomDivision46.TickLength = 7.0!
        ScaleCustomDivision46.Value = 6.0R
        Me.HScale.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision40, ScaleCustomDivision41, ScaleCustomDivision42, ScaleCustomDivision43, ScaleCustomDivision44, ScaleCustomDivision45, ScaleCustomDivision46})
        Me.HScale.DialColor = System.Drawing.Color.Navy
        Me.HScale.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThinNeedle
        Me.HScale.Location = New System.Drawing.Point(8, 16)
        Me.HScale.MajorDivisions.LabelVisible = False
        Me.HScale.MajorDivisions.TickVisible = False
        Me.HScale.MinorDivisions.TickVisible = False
        Me.HScale.Name = "HScale"
        Me.HScale.PointerColor = System.Drawing.Color.Lime
        Me.HScale.Range = New NationalInstruments.UI.Range(0R, 6.0R)
        Me.HScale.Size = New System.Drawing.Size(184, 140)
        Me.HScale.TabIndex = 30
        Me.ToolTip1.SetToolTip(Me.HScale, "Развёртка по горизонтали")
        '
        'VScaletext
        '
        Me.VScaletext.AcceptsReturn = True
        Me.VScaletext.BackColor = System.Drawing.SystemColors.Window
        Me.VScaletext.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.VScaletext.ForeColor = System.Drawing.SystemColors.WindowText
        Me.VScaletext.Location = New System.Drawing.Point(65, 148)
        Me.VScaletext.MaxLength = 0
        Me.VScaletext.Name = "VScaletext"
        Me.VScaletext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VScaletext.Size = New System.Drawing.Size(81, 20)
        Me.VScaletext.TabIndex = 15
        Me.VScaletext.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.TriggerLED)
        Me.Frame3.Controls.Add(Me.ResetOsc)
        Me.Frame3.Controls.Add(Me.Trigger)
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 432)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(536, 48)
        Me.Frame3.TabIndex = 4
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Режим измерения"
        '
        'TriggerLED
        '
        Me.TriggerLED.LedStyle = NationalInstruments.UI.LedStyle.Round3D
        Me.TriggerLED.Location = New System.Drawing.Point(6, 15)
        Me.TriggerLED.Name = "TriggerLED"
        Me.TriggerLED.Size = New System.Drawing.Size(30, 30)
        Me.TriggerLED.TabIndex = 12
        '
        'ResetOsc
        '
        Me.ResetOsc.BackColor = System.Drawing.Color.Maroon
        Me.ResetOsc.ForeColor = System.Drawing.Color.White
        Me.ResetOsc.Image = CType(resources.GetObject("ResetOsc.Image"), System.Drawing.Image)
        Me.ResetOsc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ResetOsc.Location = New System.Drawing.Point(337, 12)
        Me.ResetOsc.Name = "ResetOsc"
        Me.ResetOsc.Size = New System.Drawing.Size(118, 28)
        Me.ResetOsc.TabIndex = 11
        Me.ResetOsc.Text = "Перезапуск"
        Me.ToolTip1.SetToolTip(Me.ResetOsc, "Перезапуск опроса")
        Me.ResetOsc.UseVisualStyleBackColor = False
        '
        'Trigger
        '
        Me.Trigger.BackColor = System.Drawing.Color.Maroon
        Me.Trigger.ForeColor = System.Drawing.Color.White
        Me.Trigger.Image = CType(resources.GetObject("Trigger.Image"), System.Drawing.Image)
        Me.Trigger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Trigger.Location = New System.Drawing.Point(124, 12)
        Me.Trigger.Name = "Trigger"
        Me.Trigger.Size = New System.Drawing.Size(118, 28)
        Me.Trigger.TabIndex = 10
        Me.Trigger.Text = "Запуск"
        Me.ToolTip1.SetToolTip(Me.Trigger, "Ожидание запуска")
        Me.Trigger.UseVisualStyleBackColor = False
        '
        'VOffsetText
        '
        Me.VOffsetText.AcceptsReturn = True
        Me.VOffsetText.BackColor = System.Drawing.SystemColors.Window
        Me.VOffsetText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.VOffsetText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.VOffsetText.Location = New System.Drawing.Point(65, 286)
        Me.VOffsetText.MaxLength = 0
        Me.VOffsetText.Name = "VOffsetText"
        Me.VOffsetText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VOffsetText.Size = New System.Drawing.Size(81, 20)
        Me.VOffsetText.TabIndex = 28
        Me.VOffsetText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'HScaleText
        '
        Me.HScaleText.AcceptsReturn = True
        Me.HScaleText.BackColor = System.Drawing.SystemColors.Window
        Me.HScaleText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HScaleText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HScaleText.Location = New System.Drawing.Point(65, 140)
        Me.HScaleText.MaxLength = 0
        Me.HScaleText.Name = "HScaleText"
        Me.HScaleText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HScaleText.Size = New System.Drawing.Size(81, 20)
        Me.HScaleText.TabIndex = 29
        Me.HScaleText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'VOffset
        '
        Me.VOffset.AutoDivisionSpacing = False
        Me.VOffset.Border = NationalInstruments.UI.Border.ThinFrame3D
        Me.VOffset.Caption = "Смещение"
        Me.VOffset.DialColor = System.Drawing.Color.Navy
        Me.VOffset.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThinNeedle
        Me.VOffset.Location = New System.Drawing.Point(8, 165)
        Me.VOffset.MajorDivisions.Interval = 0.2R
        Me.VOffset.MinorDivisions.Interval = 0.05R
        Me.VOffset.Name = "VOffset"
        Me.VOffset.Range = New NationalInstruments.UI.Range(-1.0R, 1.0R)
        Me.VOffset.Size = New System.Drawing.Size(184, 135)
        Me.VOffset.TabIndex = 30
        Me.ToolTip1.SetToolTip(Me.VOffset, "Смещение шлейфа")
        Me.VOffset.Value = 0.01R
        '
        'Метка4
        '
        Me.Метка4.BackColor = System.Drawing.Color.Black
        Me.Метка4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Метка4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Метка4.ForeColor = System.Drawing.Color.Lime
        Me.Метка4.Location = New System.Drawing.Point(222, 16)
        Me.Метка4.Name = "Метка4"
        Me.Метка4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Метка4.Size = New System.Drawing.Size(160, 24)
        Me.Метка4.TabIndex = 32
        Me.Метка4.Text = "Напряжение вольт"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(214, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(336, 80)
        Me.Label1.TabIndex = 35
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Frame2.Controls.Add(Me.HScaleText)
        Me.Frame2.Controls.Add(Me.HScale)
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(550, 320)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(202, 160)
        Me.Frame2.TabIndex = 11
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Горизонтально"
        '
        'VScale
        '
        Me.VScale.Border = NationalInstruments.UI.Border.ThinFrame3D
        Me.VScale.Caption = "Масштаб"
        Me.VScale.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToDivisions
        ScaleCustomDivision47.LineWidth = 2.0!
        ScaleCustomDivision47.Text = "0.05 V/Div"
        ScaleCustomDivision47.TickLength = 7.0!
        ScaleCustomDivision48.LineWidth = 2.0!
        ScaleCustomDivision48.Text = "0.1 V/Div"
        ScaleCustomDivision48.TickLength = 7.0!
        ScaleCustomDivision48.Value = 1.0R
        ScaleCustomDivision49.LineWidth = 2.0!
        ScaleCustomDivision49.Text = "0.2 V/Div"
        ScaleCustomDivision49.TickLength = 7.0!
        ScaleCustomDivision49.Value = 2.0R
        ScaleCustomDivision50.LineWidth = 2.0!
        ScaleCustomDivision50.Text = "0.5 V/Div"
        ScaleCustomDivision50.TickLength = 7.0!
        ScaleCustomDivision50.Value = 3.0R
        ScaleCustomDivision51.LineWidth = 2.0!
        ScaleCustomDivision51.Text = "1 V/Div"
        ScaleCustomDivision51.TickLength = 7.0!
        ScaleCustomDivision51.Value = 4.0R
        ScaleCustomDivision52.LineWidth = 2.0!
        ScaleCustomDivision52.Text = "2 V/Div"
        ScaleCustomDivision52.TickLength = 7.0!
        ScaleCustomDivision52.Value = 5.0R
        Me.VScale.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision47, ScaleCustomDivision48, ScaleCustomDivision49, ScaleCustomDivision50, ScaleCustomDivision51, ScaleCustomDivision52})
        Me.VScale.DialColor = System.Drawing.Color.Navy
        Me.VScale.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThinNeedle
        Me.VScale.Location = New System.Drawing.Point(8, 19)
        Me.VScale.MajorDivisions.LabelVisible = False
        Me.VScale.MajorDivisions.TickVisible = False
        Me.VScale.MinorDivisions.TickVisible = False
        Me.VScale.Name = "VScale"
        Me.VScale.PointerColor = System.Drawing.Color.Lime
        Me.VScale.Range = New NationalInstruments.UI.Range(0R, 5.0R)
        Me.VScale.Size = New System.Drawing.Size(184, 140)
        Me.VScale.TabIndex = 29
        Me.ToolTip1.SetToolTip(Me.VScale, "Развёртка по вертикали")
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.Color.Beige
        Me.Frame1.Controls.Add(Me.VScaletext)
        Me.Frame1.Controls.Add(Me.VOffsetText)
        Me.Frame1.Controls.Add(Me.VOffset)
        Me.Frame1.Controls.Add(Me.VScale)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(550, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(202, 306)
        Me.Frame1.TabIndex = 14
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Вертикально"
        '
        'CWGraph1
        '
        Me.CWGraph1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.CWGraph1.Border = NationalInstruments.UI.Border.ThickFrame3D
        Me.CWGraph1.Caption = "График сигнала"
        Me.CWGraph1.Location = New System.Drawing.Point(16, 96)
        Me.CWGraph1.Name = "CWGraph1"
        Me.CWGraph1.Plots.AddRange(New NationalInstruments.UI.WaveformPlot() {Me.WaveformPlot1})
        Me.CWGraph1.Size = New System.Drawing.Size(736, 376)
        Me.CWGraph1.TabIndex = 46
        Me.CWGraph1.TabStop = False
        Me.CWGraph1.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
        Me.CWGraph1.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
        '
        'WaveformPlot1
        '
        Me.WaveformPlot1.XAxis = Me.XAxis1
        Me.WaveformPlot1.YAxis = Me.YAxis1
        '
        'XAxis1
        '
        Me.XAxis1.MajorDivisions.GridVisible = True
        Me.XAxis1.Mode = NationalInstruments.UI.AxisMode.Fixed
        Me.XAxis1.Range = New NationalInstruments.UI.Range(0R, 1000.0R)
        '
        'YAxis1
        '
        Me.YAxis1.Caption = "Volts"
        Me.YAxis1.MajorDivisions.GridVisible = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.DarkGray
        Me.TabPage2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPage2.Controls.Add(Me.CWGraph2)
        Me.TabPage2.Controls.Add(Me.Frame3)
        Me.TabPage2.Controls.Add(Me.Frame2)
        Me.TabPage2.Controls.Add(Me.Frame1)
        Me.TabPage2.ImageIndex = 1
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(768, 493)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Осциллограф"
        Me.TabPage2.ToolTipText = "Виртуальный осциллограф"
        '
        'Acquire
        '
        Me.Acquire.BackColor = System.Drawing.SystemColors.Control
        Me.Acquire.Image = CType(resources.GetObject("Acquire.Image"), System.Drawing.Image)
        Me.Acquire.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Acquire.Location = New System.Drawing.Point(600, 35)
        Me.Acquire.Name = "Acquire"
        Me.Acquire.Size = New System.Drawing.Size(88, 33)
        Me.Acquire.TabIndex = 48
        Me.Acquire.Text = "Опросить"
        Me.Acquire.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Acquire, "Опрос серии замеров")
        Me.Acquire.UseVisualStyleBackColor = False
        '
        'ButtonHide
        '
        Me.ButtonHide.Image = CType(resources.GetObject("ButtonHide.Image"), System.Drawing.Image)
        Me.ButtonHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonHide.Location = New System.Drawing.Point(680, 3)
        Me.ButtonHide.Margin = New System.Windows.Forms.Padding(3, 3, 12, 3)
        Me.ButtonHide.Name = "ButtonHide"
        Me.ButtonHide.Size = New System.Drawing.Size(101, 24)
        Me.ButtonHide.TabIndex = 53
        Me.ButtonHide.Text = "Закрыть"
        Me.ToolTip1.SetToolTip(Me.ButtonHide, "Закрыть форму опроса")
        Me.ButtonHide.UseVisualStyleBackColor = True
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.DarkGray
        Me.TabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TabPage1.Controls.Add(Me.Acquire)
        Me.TabPage1.Controls.Add(Me.CWGraph1)
        Me.TabPage1.Controls.Add(Me.CWNumEdit2)
        Me.TabPage1.Controls.Add(Me.CWNumEdit1)
        Me.TabPage1.Controls.Add(Me.LabelPhisic)
        Me.TabPage1.Controls.Add(Me.Метка4)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.ImageIndex = 0
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(768, 493)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Вольтметр"
        Me.ToolTip1.SetToolTip(Me.TabPage1, "Виртуальный вольтметр")
        '
        'CWNumEdit2
        '
        Me.CWNumEdit2.BackColor = System.Drawing.Color.Black
        Me.CWNumEdit2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.CWNumEdit2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CWNumEdit2.ForeColor = System.Drawing.Color.Lime
        Me.CWNumEdit2.Location = New System.Drawing.Point(398, 40)
        Me.CWNumEdit2.Name = "CWNumEdit2"
        Me.CWNumEdit2.ReadOnly = True
        Me.CWNumEdit2.Size = New System.Drawing.Size(136, 28)
        Me.CWNumEdit2.TabIndex = 43
        Me.CWNumEdit2.TabStop = False
        Me.CWNumEdit2.Text = "0.00000"
        Me.CWNumEdit2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CWNumEdit1
        '
        Me.CWNumEdit1.BackColor = System.Drawing.Color.Black
        Me.CWNumEdit1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.CWNumEdit1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CWNumEdit1.ForeColor = System.Drawing.Color.Lime
        Me.CWNumEdit1.Location = New System.Drawing.Point(398, 8)
        Me.CWNumEdit1.Name = "CWNumEdit1"
        Me.CWNumEdit1.ReadOnly = True
        Me.CWNumEdit1.Size = New System.Drawing.Size(136, 28)
        Me.CWNumEdit1.TabIndex = 42
        Me.CWNumEdit1.TabStop = False
        Me.CWNumEdit1.Text = "0.00000"
        Me.CWNumEdit1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TabControlTools
        '
        Me.TabControlTools.Controls.Add(Me.TabPage1)
        Me.TabControlTools.Controls.Add(Me.TabPage2)
        Me.TabControlTools.Controls.Add(Me.TabPage3)
        Me.TabControlTools.ImageList = Me.ImageList1
        Me.TabControlTools.Location = New System.Drawing.Point(9, 7)
        Me.TabControlTools.Name = "TabControlTools"
        Me.TabControlTools.SelectedIndex = 0
        Me.TabControlTools.Size = New System.Drawing.Size(776, 520)
        Me.TabControlTools.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.TabControlTools, "Виртуальный прибор")
        '
        'Метка3
        '
        Me.Метка3.BackColor = System.Drawing.SystemColors.Control
        Me.Метка3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Метка3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Метка3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Метка3.Location = New System.Drawing.Point(3, 0)
        Me.Метка3.Name = "Метка3"
        Me.Метка3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Метка3.Size = New System.Drawing.Size(289, 27)
        Me.Метка3.TabIndex = 51
        Me.Метка3.Text = "Наименование параметра"
        Me.Метка3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxParameters
        '
        Me.ComboBoxParameters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxParameters.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxParameters.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxParameters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxParameters.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxParameters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxParameters.ItemHeight = 18
        Me.ComboBoxParameters.Location = New System.Drawing.Point(308, 3)
        Me.ComboBoxParameters.MaxDropDownItems = 32
        Me.ComboBoxParameters.Name = "ComboBoxParameters"
        Me.ComboBoxParameters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxParameters.Size = New System.Drawing.Size(186, 24)
        Me.ComboBoxParameters.TabIndex = 54
        Me.ToolTip1.SetToolTip(Me.ComboBoxParameters, "Выбрать параметр для опроса")
        '
        'ButtonFindChannel
        '
        Me.ButtonFindChannel.Image = CType(resources.GetObject("ButtonFindChannel.Image"), System.Drawing.Image)
        Me.ButtonFindChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFindChannel.Location = New System.Drawing.Point(500, 3)
        Me.ButtonFindChannel.Name = "ButtonFindChannel"
        Me.ButtonFindChannel.Size = New System.Drawing.Size(110, 24)
        Me.ButtonFindChannel.TabIndex = 55
        Me.ButtonFindChannel.Text = "Поиск канала"
        Me.ButtonFindChannel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannel, "Быстрый поиск канала по имени")
        Me.ButtonFindChannel.UseVisualStyleBackColor = True
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
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 305.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonFindChannel, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxParameters, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonHide, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Метка3, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 535)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(794, 40)
        Me.TableLayoutPanel1.TabIndex = 56
        '
        'FormTestChannel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 575)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.TabControlTools)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormTestChannel"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Опрос Параметра"
        Me.TopMost = True
        CType(Me.peakPower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.peakFrequency, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SettingsGroupBox.ResumeLayout(False)
        CType(Me.samplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.AcquisitionGroupBox.ResumeLayout(False)
        CType(Me.AcquisitionStateSwitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WaveformGraph, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerSpectrumGraph, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XYCursor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CWGraph2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HScale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame3.ResumeLayout(False)
        CType(Me.TriggerLED, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VOffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        CType(Me.VScale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.CWGraph1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControlTools.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents LabelPhisic As System.Windows.Forms.Label
    Friend WithEvents peakPower As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents rateNumeric As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents peakFrequency As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents SettingsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents window As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents units As System.Windows.Forms.ComboBox
    Friend WithEvents unitsLabel As System.Windows.Forms.Label
    Friend WithEvents scalecomboBox As System.Windows.Forms.ComboBox
    Friend WithEvents scaleLabel As System.Windows.Forms.Label
    Friend WithEvents samplesNumeric As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents AcquisitionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents AcquisitionStateSwitch As NationalInstruments.UI.WindowsForms.Switch
    Friend WithEvents WaveformGraph As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents waveformPlot As NationalInstruments.UI.WaveformPlot
    Friend WithEvents waveformXAxis As NationalInstruments.UI.XAxis
    Friend WithEvents waveformYAxis As NationalInstruments.UI.YAxis
    Friend WithEvents powerSpectrumGraph As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents XYCursor As NationalInstruments.UI.XYCursor
    Friend WithEvents powerSpectrumPlot As NationalInstruments.UI.WaveformPlot
    Friend WithEvents powerSpectrumxAxis As NationalInstruments.UI.XAxis
    Friend WithEvents powerSpectrumYAxis As NationalInstruments.UI.YAxis
    Friend WithEvents YAxis2 As NationalInstruments.UI.YAxis
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents XAxis2 As NationalInstruments.UI.XAxis
    Friend WithEvents WaveformPlot2 As NationalInstruments.UI.WaveformPlot
    Friend WithEvents CWGraph2 As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents HScale As NationalInstruments.UI.WindowsForms.Knob
    Public WithEvents VScaletext As System.Windows.Forms.TextBox
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Friend WithEvents TriggerLED As NationalInstruments.UI.WindowsForms.Led
    Friend WithEvents ResetOsc As System.Windows.Forms.Button
    Friend WithEvents Trigger As System.Windows.Forms.Button
    Public WithEvents VOffsetText As System.Windows.Forms.TextBox
    Public WithEvents HScaleText As System.Windows.Forms.TextBox
    Friend WithEvents VOffset As NationalInstruments.UI.WindowsForms.Knob
    Public WithEvents Метка4 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Friend WithEvents VScale As NationalInstruments.UI.WindowsForms.Knob
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Friend WithEvents CWGraph1 As NationalInstruments.UI.WindowsForms.WaveformGraph
    Friend WithEvents WaveformPlot1 As NationalInstruments.UI.WaveformPlot
    Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Acquire As System.Windows.Forms.Button
    Friend WithEvents ButtonHide As System.Windows.Forms.Button
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents CWNumEdit2 As System.Windows.Forms.TextBox
    Public WithEvents CWNumEdit1 As System.Windows.Forms.TextBox
    Public WithEvents TabControlTools As System.Windows.Forms.TabControl
    Public WithEvents Метка3 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ImageListChannel As ImageList
    Public WithEvents ComboBoxParameters As ComboBox
    Friend WithEvents ButtonFindChannel As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
End Class
