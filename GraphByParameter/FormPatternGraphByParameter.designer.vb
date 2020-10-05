<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPatternGraphByParameter
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ScaleCustomDivision1 As NationalInstruments.UI.ScaleCustomDivision = New NationalInstruments.UI.ScaleCustomDivision()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormPatternGraphByParameter))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.CWGraphA1 = New NationalInstruments.UI.WindowsForms.ScatterGraph()
        Me.XyCursor1 = New NationalInstruments.UI.XYCursor()
        Me.ScatterPlot1 = New NationalInstruments.UI.ScatterPlot()
        Me.XAxis1 = New NationalInstruments.UI.XAxis()
        Me.YAxis1 = New NationalInstruments.UI.YAxis()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelIndicatorY = New System.Windows.Forms.Label()
        Me.lblY = New System.Windows.Forms.Label()
        Me.lblX = New System.Windows.Forms.Label()
        Me.LabelIndicatorX = New System.Windows.Forms.Label()
        Me.SlideTime = New NationalInstruments.UI.WindowsForms.Slide()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemPrintGraf = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemSaveGraf = New System.Windows.Forms.ToolStripMenuItem()
        Me.TSMenuItemExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        CType(Me.CWGraphA1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.CWGraphA1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.TableLayoutPanel1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SlideTime)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(754, 479)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(754, 503)
        Me.ToolStripContainer1.TabIndex = 1
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        '
        'CWGraphA1
        '
        Me.CWGraphA1.Border = NationalInstruments.UI.Border.Sunken
        Me.CWGraphA1.Caption = "График параметр У от Х"
        Me.CWGraphA1.Cursors.AddRange(New NationalInstruments.UI.XYCursor() {Me.XyCursor1})
        Me.CWGraphA1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CWGraphA1.Location = New System.Drawing.Point(0, 30)
        Me.CWGraphA1.Name = "CWGraphA1"
        Me.CWGraphA1.Plots.AddRange(New NationalInstruments.UI.ScatterPlot() {Me.ScatterPlot1})
        Me.CWGraphA1.Size = New System.Drawing.Size(754, 395)
        Me.CWGraphA1.TabIndex = 129
        Me.CWGraphA1.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
        Me.CWGraphA1.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
        '
        'XyCursor1
        '
        Me.XyCursor1.Color = System.Drawing.Color.Gold
        Me.XyCursor1.LabelVisible = True
        Me.XyCursor1.Plot = Me.ScatterPlot1
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
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.Black
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.46154!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.53846!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.46154!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.53846!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelIndicatorY, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblY, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblX, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelIndicatorX, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(754, 30)
        Me.TableLayoutPanel1.TabIndex = 128
        '
        'ИндикаторY
        '
        Me.LabelIndicatorY.BackColor = System.Drawing.Color.Black
        Me.LabelIndicatorY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelIndicatorY.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelIndicatorY.ForeColor = System.Drawing.Color.Lime
        Me.LabelIndicatorY.Location = New System.Drawing.Point(668, 3)
        Me.LabelIndicatorY.Name = "ИндикаторY"
        Me.LabelIndicatorY.Size = New System.Drawing.Size(80, 24)
        Me.LabelIndicatorY.TabIndex = 12
        Me.LabelIndicatorY.Text = "100.0"
        Me.LabelIndicatorY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblY
        '
        Me.lblY.BackColor = System.Drawing.Color.Black
        Me.lblY.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblY.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblY.ForeColor = System.Drawing.Color.Lime
        Me.lblY.Location = New System.Drawing.Point(381, 3)
        Me.lblY.Name = "lblY"
        Me.lblY.Size = New System.Drawing.Size(278, 24)
        Me.lblY.TabIndex = 11
        Me.lblY.Text = "Параметр Y"
        Me.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblX
        '
        Me.lblX.BackColor = System.Drawing.Color.Black
        Me.lblX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblX.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblX.ForeColor = System.Drawing.Color.Lime
        Me.lblX.Location = New System.Drawing.Point(6, 3)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(278, 24)
        Me.lblX.TabIndex = 5
        Me.lblX.Text = "Параметр X"
        Me.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ИндикаторX
        '
        Me.LabelIndicatorX.BackColor = System.Drawing.Color.Black
        Me.LabelIndicatorX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelIndicatorX.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelIndicatorX.ForeColor = System.Drawing.Color.Lime
        Me.LabelIndicatorX.Location = New System.Drawing.Point(293, 3)
        Me.LabelIndicatorX.Name = "ИндикаторX"
        Me.LabelIndicatorX.Size = New System.Drawing.Size(79, 24)
        Me.LabelIndicatorX.TabIndex = 6
        Me.LabelIndicatorX.Text = "100.0"
        Me.LabelIndicatorX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CWSlideВремя
        '
        Me.SlideTime.Border = NationalInstruments.UI.Border.Sunken
        ScaleCustomDivision1.LineWidth = 2.0!
        ScaleCustomDivision1.Value = 3.5R
        Me.SlideTime.CustomDivisions.AddRange(New NationalInstruments.UI.ScaleCustomDivision() {ScaleCustomDivision1})
        Me.SlideTime.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SlideTime.FillBackColor = System.Drawing.Color.Silver
        Me.SlideTime.InteractionMode = CType(((NationalInstruments.UI.LinearNumericPointerInteractionModes.DragPointer Or NationalInstruments.UI.LinearNumericPointerInteractionModes.SnapPointer) _
                    Or NationalInstruments.UI.LinearNumericPointerInteractionModes.EditRange), NationalInstruments.UI.LinearNumericPointerInteractionModes)
        Me.SlideTime.Location = New System.Drawing.Point(0, 425)
        Me.SlideTime.Name = "CWSlideВремя"
        Me.SlideTime.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.SlideTime.PointerColor = System.Drawing.Color.Maroon
        Me.SlideTime.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        Me.SlideTime.Size = New System.Drawing.Size(754, 54)
        Me.SlideTime.SlideStyle = NationalInstruments.UI.SlideStyle.RaisedWithRoundedGrip
        Me.SlideTime.TabIndex = 125
        Me.SlideTime.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(754, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSMenuItemPrintGraf, Me.TSMenuItemSaveGraf, Me.TSMenuItemExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(60, 20)
        Me.mnuFile.Text = "&График"
        '
        'mnuPrintGraf
        '
        Me.TSMenuItemPrintGraf.Image = CType(resources.GetObject("mnuPrintGraf.Image"), System.Drawing.Image)
        Me.TSMenuItemPrintGraf.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemPrintGraf.Name = "mnuPrintGraf"
        Me.TSMenuItemPrintGraf.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.TSMenuItemPrintGraf.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemPrintGraf.Text = "&Печать графика"
        '
        'mnuSaveGraf
        '
        Me.TSMenuItemSaveGraf.Image = CType(resources.GetObject("mnuSaveGraf.Image"), System.Drawing.Image)
        Me.TSMenuItemSaveGraf.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSMenuItemSaveGraf.Name = "mnuSaveGraf"
        Me.TSMenuItemSaveGraf.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.TSMenuItemSaveGraf.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemSaveGraf.Text = "&Запись графика"
        '
        'mnuExit
        '
        Me.TSMenuItemExit.Image = CType(resources.GetObject("mnuExit.Image"), System.Drawing.Image)
        Me.TSMenuItemExit.Name = "mnuExit"
        Me.TSMenuItemExit.Size = New System.Drawing.Size(203, 22)
        Me.TSMenuItemExit.Text = "&Закрыть окно"
        '
        'frmШаблонГрафикаОтПараметра
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(754, 503)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmШаблонГрафикаОтПараметра"
        Me.Text = "Функция"
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        CType(Me.CWGraphA1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XyCursor1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.SlideTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Public WithEvents LabelIndicatorX As System.Windows.Forms.Label
    Public WithEvents lblX As System.Windows.Forms.Label
    Public WithEvents SlideTime As NationalInstruments.UI.WindowsForms.Slide
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemPrintGraf As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemSaveGraf As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TSMenuItemExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents CWGraphA1 As NationalInstruments.UI.WindowsForms.ScatterGraph
    Friend WithEvents ScatterPlot1 As NationalInstruments.UI.ScatterPlot
    Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
    Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
    Friend WithEvents XyCursor1 As NationalInstruments.UI.XYCursor
    Public WithEvents LabelIndicatorY As System.Windows.Forms.Label
    Public WithEvents lblY As System.Windows.Forms.Label
End Class
