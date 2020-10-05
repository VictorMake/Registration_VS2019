<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSearchChannelTuning
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSearchChannelTuning))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ChannelNTableAdapter = New Registration.ChannelsNDataSetTableAdapters.ChannelNTableAdapter()
        Me.BindingNavigatorChannelsN = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorLabelName = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorTextBoxFilter = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.LabelDescriptionStage = New System.Windows.Forms.Label()
        Me.DataGridFindChannels = New System.Windows.Forms.DataGridView()
        Me.BindingSourceChannelsN = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChannelsNDataSet1 = New Registration.ChannelsNDataSet()
        Me.PanelTable = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.НомерПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НаименованиеПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ПримечанияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерКаналаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерУстройстваDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ТипПодключенияDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НижнийПределDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ВерхнийПределDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ТипСигналаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.НомерФормулыDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A0DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A1DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A2DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A3DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A4DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.A5DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.СмещениеDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.КомпенсацияХСDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ДопускМинимумDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ДопускМаксимумDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.РазносУминDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.РазносУмаксDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.БлокировкаDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ДатаDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ВидимостьDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ПогрешностьDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.BindingNavigatorChannelsN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorChannelsN.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridFindChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceChannelsN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelsNDataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelTable.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.BackColor = System.Drawing.Color.Silver
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Image = CType(resources.GetObject("ButtonCancel.Image"), System.Drawing.Image)
        Me.ButtonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonCancel.Location = New System.Drawing.Point(703, 9)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(128, 32)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Прервать"
        Me.ToolTip1.SetToolTip(Me.ButtonCancel, "Закрыть форму без поиска")
        Me.ButtonCancel.UseVisualStyleBackColor = False
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.BackColor = System.Drawing.Color.Silver
        Me.ButtonOK.Image = CType(resources.GetObject("ButtonOK.Image"), System.Drawing.Image)
        Me.ButtonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonOK.Location = New System.Drawing.Point(567, 9)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(128, 32)
        Me.ButtonOK.TabIndex = 0
        Me.ButtonOK.Text = "&Выбор сделан"
        Me.ToolTip1.SetToolTip(Me.ButtonOK, "Закрыть форму с выбранным каналом")
        Me.ButtonOK.UseVisualStyleBackColor = False
        '
        'ChannelNTableAdapter
        '
        Me.ChannelNTableAdapter.ClearBeforeFill = True
        '
        'BindingNavigatorChannelsN
        '
        Me.BindingNavigatorChannelsN.AddNewItem = Nothing
        Me.BindingNavigatorChannelsN.CountItem = Nothing
        Me.BindingNavigatorChannelsN.DeleteItem = Nothing
        Me.BindingNavigatorChannelsN.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BindingNavigatorChannelsN.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.BindingNavigatorChannelsN.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.BindingNavigatorChannelsN.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorLabelName, Me.BindingNavigatorTextBoxFilter, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator3})
        Me.BindingNavigatorChannelsN.Location = New System.Drawing.Point(0, 5)
        Me.BindingNavigatorChannelsN.MoveFirstItem = Nothing
        Me.BindingNavigatorChannelsN.MoveLastItem = Nothing
        Me.BindingNavigatorChannelsN.MoveNextItem = Nothing
        Me.BindingNavigatorChannelsN.MovePreviousItem = Nothing
        Me.BindingNavigatorChannelsN.Name = "BindingNavigatorChannelsN"
        Me.BindingNavigatorChannelsN.PositionItem = Nothing
        Me.BindingNavigatorChannelsN.Size = New System.Drawing.Size(562, 39)
        Me.BindingNavigatorChannelsN.TabIndex = 39
        Me.BindingNavigatorChannelsN.Text = "BindingNavigator1"
        '
        'BindingNavigatorLabelName
        '
        Me.BindingNavigatorLabelName.Name = "BindingNavigatorLabelName"
        Me.BindingNavigatorLabelName.Size = New System.Drawing.Size(170, 36)
        Me.BindingNavigatorLabelName.Text = "Предполагаемое имя канала:"
        '
        'BindingNavigatorTextBoxFilter
        '
        Me.BindingNavigatorTextBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BindingNavigatorTextBoxFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BindingNavigatorTextBoxFilter.Name = "BindingNavigatorTextBoxFilter"
        Me.BindingNavigatorTextBoxFilter.Size = New System.Drawing.Size(100, 39)
        Me.BindingNavigatorTextBoxFilter.ToolTipText = "Фильтр канала"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(36, 36)
        Me.BindingNavigatorMoveFirstItem.Text = "Переместить в начало"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(36, 36)
        Me.BindingNavigatorMovePreviousItem.Text = "Переместить назад"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 39)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Положение"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 25)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Текущее положение"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(52, 36)
        Me.BindingNavigatorCountItem.Text = "для {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Общее число элементов"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 39)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(36, 36)
        Me.BindingNavigatorMoveNextItem.Text = "Переместить вперед"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(36, 36)
        Me.BindingNavigatorMoveLastItem.Text = "Переместить в конец"
        '
        'BindingNavigatorSeparator3
        '
        Me.BindingNavigatorSeparator3.Name = "BindingNavigatorSeparator3"
        Me.BindingNavigatorSeparator3.Size = New System.Drawing.Size(6, 39)
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'LabelDescriptionStage
        '
        Me.LabelDescriptionStage.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelDescriptionStage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelDescriptionStage.ForeColor = System.Drawing.Color.Blue
        Me.LabelDescriptionStage.Location = New System.Drawing.Point(5, 5)
        Me.LabelDescriptionStage.Name = "LabelDescriptionStage"
        Me.LabelDescriptionStage.Size = New System.Drawing.Size(834, 17)
        Me.LabelDescriptionStage.TabIndex = 37
        Me.LabelDescriptionStage.Text = "Задать фильтр и выбрать канал в таблице"
        Me.LabelDescriptionStage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'DataGridFindChannels
        '
        Me.DataGridFindChannels.AllowUserToAddRows = False
        Me.DataGridFindChannels.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGridFindChannels.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridFindChannels.AutoGenerateColumns = False
        Me.DataGridFindChannels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridFindChannels.BackgroundColor = System.Drawing.Color.Lavender
        Me.DataGridFindChannels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridFindChannels.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridFindChannels.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridFindChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridFindChannels.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.НомерПараметраDataGridViewTextBoxColumn, Me.НаименованиеПараметраDataGridViewTextBoxColumn, Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn, Me.ПримечанияDataGridViewTextBoxColumn, Me.НомерКаналаDataGridViewTextBoxColumn, Me.НомерУстройстваDataGridViewTextBoxColumn, Me.НомерМодуляКорзиныDataGridViewTextBoxColumn, Me.НомерКаналаМодуляDataGridViewTextBoxColumn, Me.ТипПодключенияDataGridViewTextBoxColumn, Me.НижнийПределDataGridViewTextBoxColumn, Me.ВерхнийПределDataGridViewTextBoxColumn, Me.ТипСигналаDataGridViewTextBoxColumn, Me.НомерФормулыDataGridViewTextBoxColumn, Me.СтепеньАппроксимацииDataGridViewTextBoxColumn, Me.A0DataGridViewTextBoxColumn, Me.A1DataGridViewTextBoxColumn, Me.A2DataGridViewTextBoxColumn, Me.A3DataGridViewTextBoxColumn, Me.A4DataGridViewTextBoxColumn, Me.A5DataGridViewTextBoxColumn, Me.СмещениеDataGridViewTextBoxColumn, Me.КомпенсацияХСDataGridViewCheckBoxColumn, Me.ДопускМинимумDataGridViewTextBoxColumn, Me.ДопускМаксимумDataGridViewTextBoxColumn, Me.РазносУминDataGridViewTextBoxColumn, Me.РазносУмаксDataGridViewTextBoxColumn, Me.АварийноеЗначениеМинDataGridViewTextBoxColumn, Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn, Me.БлокировкаDataGridViewCheckBoxColumn, Me.ДатаDataGridViewTextBoxColumn, Me.ВидимостьРегистраторDataGridViewCheckBoxColumn, Me.ВидимостьDataGridViewCheckBoxColumn, Me.ПогрешностьDataGridViewTextBoxColumn})
        Me.DataGridFindChannels.DataSource = Me.BindingSourceChannelsN
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridFindChannels.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridFindChannels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridFindChannels.GridColor = System.Drawing.Color.RoyalBlue
        Me.DataGridFindChannels.Location = New System.Drawing.Point(5, 22)
        Me.DataGridFindChannels.MultiSelect = False
        Me.DataGridFindChannels.Name = "DataGridFindChannels"
        Me.DataGridFindChannels.ReadOnly = True
        Me.DataGridFindChannels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridFindChannels.Size = New System.Drawing.Size(834, 302)
        Me.DataGridFindChannels.TabIndex = 38
        '
        'BindingSourceChannelsN
        '
        Me.BindingSourceChannelsN.DataMember = "ChannelN"
        Me.BindingSourceChannelsN.DataSource = Me.ChannelsNDataSet1
        '
        'ChannelsNDataSet1
        '
        Me.ChannelsNDataSet1.DataSetName = "ChannelsNDataSet"
        Me.ChannelsNDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PanelTable
        '
        Me.PanelTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelTable.Controls.Add(Me.DataGridFindChannels)
        Me.PanelTable.Controls.Add(Me.TableLayoutPanel1)
        Me.PanelTable.Controls.Add(Me.LabelDescriptionStage)
        Me.PanelTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTable.Location = New System.Drawing.Point(0, 0)
        Me.PanelTable.Name = "PanelTable"
        Me.PanelTable.Padding = New System.Windows.Forms.Padding(5)
        Me.PanelTable.Size = New System.Drawing.Size(848, 377)
        Me.PanelTable.TabIndex = 40
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonOK, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonCancel, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BindingNavigatorChannelsN, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(5, 324)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(834, 44)
        Me.TableLayoutPanel1.TabIndex = 40
        '
        'НомерПараметраDataGridViewTextBoxColumn
        '
        Me.НомерПараметраDataGridViewTextBoxColumn.DataPropertyName = "НомерПараметра"
        Me.НомерПараметраDataGridViewTextBoxColumn.Frozen = True
        Me.НомерПараметраDataGridViewTextBoxColumn.HeaderText = "Номер"
        Me.НомерПараметраDataGridViewTextBoxColumn.Name = "НомерПараметраDataGridViewTextBoxColumn"
        Me.НомерПараметраDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерПараметраDataGridViewTextBoxColumn.Width = 76
        '
        'НаименованиеПараметраDataGridViewTextBoxColumn
        '
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.DataPropertyName = "НаименованиеПараметра"
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.Frozen = True
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.HeaderText = "Имя"
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.Name = "НаименованиеПараметраDataGridViewTextBoxColumn"
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.ReadOnly = True
        Me.НаименованиеПараметраDataGridViewTextBoxColumn.Width = 60
        '
        'ЕдиницаИзмеренияDataGridViewTextBoxColumn
        '
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn.DataPropertyName = "ЕдиницаИзмерения"
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn.HeaderText = "Единица Измерения"
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn.Name = "ЕдиницаИзмеренияDataGridViewTextBoxColumn"
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn.ReadOnly = True
        Me.ЕдиницаИзмеренияDataGridViewTextBoxColumn.Width = 153
        '
        'ПримечанияDataGridViewTextBoxColumn
        '
        Me.ПримечанияDataGridViewTextBoxColumn.DataPropertyName = "Примечания"
        Me.ПримечанияDataGridViewTextBoxColumn.HeaderText = "Примечания"
        Me.ПримечанияDataGridViewTextBoxColumn.Name = "ПримечанияDataGridViewTextBoxColumn"
        Me.ПримечанияDataGridViewTextBoxColumn.ReadOnly = True
        Me.ПримечанияDataGridViewTextBoxColumn.Width = 115
        '
        'НомерКаналаDataGridViewTextBoxColumn
        '
        Me.НомерКаналаDataGridViewTextBoxColumn.DataPropertyName = "НомерКанала"
        Me.НомерКаналаDataGridViewTextBoxColumn.HeaderText = "Номер Канала"
        Me.НомерКаналаDataGridViewTextBoxColumn.Name = "НомерКаналаDataGridViewTextBoxColumn"
        Me.НомерКаналаDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерКаналаDataGridViewTextBoxColumn.Width = 118
        '
        'НомерУстройстваDataGridViewTextBoxColumn
        '
        Me.НомерУстройстваDataGridViewTextBoxColumn.DataPropertyName = "НомерУстройства"
        Me.НомерУстройстваDataGridViewTextBoxColumn.HeaderText = "Номер Устройства"
        Me.НомерУстройстваDataGridViewTextBoxColumn.Name = "НомерУстройстваDataGridViewTextBoxColumn"
        Me.НомерУстройстваDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерУстройстваDataGridViewTextBoxColumn.Width = 145
        '
        'НомерМодуляКорзиныDataGridViewTextBoxColumn
        '
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.DataPropertyName = "НомерМодуляКорзины"
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.HeaderText = "Номер Модуля Корзины"
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.Name = "НомерМодуляКорзиныDataGridViewTextBoxColumn"
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерМодуляКорзиныDataGridViewTextBoxColumn.Width = 175
        '
        'НомерКаналаМодуляDataGridViewTextBoxColumn
        '
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn.DataPropertyName = "НомерКаналаМодуля"
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn.HeaderText = "Номер Канала Модуля"
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn.Name = "НомерКаналаМодуляDataGridViewTextBoxColumn"
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерКаналаМодуляDataGridViewTextBoxColumn.Width = 167
        '
        'ТипПодключенияDataGridViewTextBoxColumn
        '
        Me.ТипПодключенияDataGridViewTextBoxColumn.DataPropertyName = "ТипПодключения"
        Me.ТипПодключенияDataGridViewTextBoxColumn.HeaderText = "Тип Подключения"
        Me.ТипПодключенияDataGridViewTextBoxColumn.Name = "ТипПодключенияDataGridViewTextBoxColumn"
        Me.ТипПодключенияDataGridViewTextBoxColumn.ReadOnly = True
        Me.ТипПодключенияDataGridViewTextBoxColumn.Width = 137
        '
        'НижнийПределDataGridViewTextBoxColumn
        '
        Me.НижнийПределDataGridViewTextBoxColumn.DataPropertyName = "НижнийПредел"
        Me.НижнийПределDataGridViewTextBoxColumn.HeaderText = "Нижний Предел"
        Me.НижнийПределDataGridViewTextBoxColumn.Name = "НижнийПределDataGridViewTextBoxColumn"
        Me.НижнийПределDataGridViewTextBoxColumn.ReadOnly = True
        Me.НижнийПределDataGridViewTextBoxColumn.Width = 126
        '
        'ВерхнийПределDataGridViewTextBoxColumn
        '
        Me.ВерхнийПределDataGridViewTextBoxColumn.DataPropertyName = "ВерхнийПредел"
        Me.ВерхнийПределDataGridViewTextBoxColumn.HeaderText = "Верхний Предел"
        Me.ВерхнийПределDataGridViewTextBoxColumn.Name = "ВерхнийПределDataGridViewTextBoxColumn"
        Me.ВерхнийПределDataGridViewTextBoxColumn.ReadOnly = True
        Me.ВерхнийПределDataGridViewTextBoxColumn.Width = 130
        '
        'ТипСигналаDataGridViewTextBoxColumn
        '
        Me.ТипСигналаDataGridViewTextBoxColumn.DataPropertyName = "ТипСигнала"
        Me.ТипСигналаDataGridViewTextBoxColumn.HeaderText = "Тип Сигнала"
        Me.ТипСигналаDataGridViewTextBoxColumn.Name = "ТипСигналаDataGridViewTextBoxColumn"
        Me.ТипСигналаDataGridViewTextBoxColumn.ReadOnly = True
        Me.ТипСигналаDataGridViewTextBoxColumn.Width = 105
        '
        'НомерФормулыDataGridViewTextBoxColumn
        '
        Me.НомерФормулыDataGridViewTextBoxColumn.DataPropertyName = "НомерФормулы"
        Me.НомерФормулыDataGridViewTextBoxColumn.HeaderText = "Номер Формулы"
        Me.НомерФормулыDataGridViewTextBoxColumn.Name = "НомерФормулыDataGridViewTextBoxColumn"
        Me.НомерФормулыDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерФормулыDataGridViewTextBoxColumn.Width = 130
        '
        'СтепеньАппроксимацииDataGridViewTextBoxColumn
        '
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn.DataPropertyName = "СтепеньАппроксимации"
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn.HeaderText = "Степень Аппроксимации"
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn.Name = "СтепеньАппроксимацииDataGridViewTextBoxColumn"
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn.ReadOnly = True
        Me.СтепеньАппроксимацииDataGridViewTextBoxColumn.Width = 179
        '
        'A0DataGridViewTextBoxColumn
        '
        Me.A0DataGridViewTextBoxColumn.DataPropertyName = "A0"
        Me.A0DataGridViewTextBoxColumn.HeaderText = "A0"
        Me.A0DataGridViewTextBoxColumn.Name = "A0DataGridViewTextBoxColumn"
        Me.A0DataGridViewTextBoxColumn.ReadOnly = True
        Me.A0DataGridViewTextBoxColumn.Width = 48
        '
        'A1DataGridViewTextBoxColumn
        '
        Me.A1DataGridViewTextBoxColumn.DataPropertyName = "A1"
        Me.A1DataGridViewTextBoxColumn.HeaderText = "A1"
        Me.A1DataGridViewTextBoxColumn.Name = "A1DataGridViewTextBoxColumn"
        Me.A1DataGridViewTextBoxColumn.ReadOnly = True
        Me.A1DataGridViewTextBoxColumn.Width = 48
        '
        'A2DataGridViewTextBoxColumn
        '
        Me.A2DataGridViewTextBoxColumn.DataPropertyName = "A2"
        Me.A2DataGridViewTextBoxColumn.HeaderText = "A2"
        Me.A2DataGridViewTextBoxColumn.Name = "A2DataGridViewTextBoxColumn"
        Me.A2DataGridViewTextBoxColumn.ReadOnly = True
        Me.A2DataGridViewTextBoxColumn.Width = 48
        '
        'A3DataGridViewTextBoxColumn
        '
        Me.A3DataGridViewTextBoxColumn.DataPropertyName = "A3"
        Me.A3DataGridViewTextBoxColumn.HeaderText = "A3"
        Me.A3DataGridViewTextBoxColumn.Name = "A3DataGridViewTextBoxColumn"
        Me.A3DataGridViewTextBoxColumn.ReadOnly = True
        Me.A3DataGridViewTextBoxColumn.Width = 48
        '
        'A4DataGridViewTextBoxColumn
        '
        Me.A4DataGridViewTextBoxColumn.DataPropertyName = "A4"
        Me.A4DataGridViewTextBoxColumn.HeaderText = "A4"
        Me.A4DataGridViewTextBoxColumn.Name = "A4DataGridViewTextBoxColumn"
        Me.A4DataGridViewTextBoxColumn.ReadOnly = True
        Me.A4DataGridViewTextBoxColumn.Width = 48
        '
        'A5DataGridViewTextBoxColumn
        '
        Me.A5DataGridViewTextBoxColumn.DataPropertyName = "A5"
        Me.A5DataGridViewTextBoxColumn.HeaderText = "A5"
        Me.A5DataGridViewTextBoxColumn.Name = "A5DataGridViewTextBoxColumn"
        Me.A5DataGridViewTextBoxColumn.ReadOnly = True
        Me.A5DataGridViewTextBoxColumn.Width = 48
        '
        'СмещениеDataGridViewTextBoxColumn
        '
        Me.СмещениеDataGridViewTextBoxColumn.DataPropertyName = "Смещение"
        Me.СмещениеDataGridViewTextBoxColumn.HeaderText = "Смещение"
        Me.СмещениеDataGridViewTextBoxColumn.Name = "СмещениеDataGridViewTextBoxColumn"
        Me.СмещениеDataGridViewTextBoxColumn.ReadOnly = True
        Me.СмещениеDataGridViewTextBoxColumn.Width = 101
        '
        'КомпенсацияХСDataGridViewCheckBoxColumn
        '
        Me.КомпенсацияХСDataGridViewCheckBoxColumn.DataPropertyName = "КомпенсацияХС"
        Me.КомпенсацияХСDataGridViewCheckBoxColumn.HeaderText = "Компенсация ХС"
        Me.КомпенсацияХСDataGridViewCheckBoxColumn.Name = "КомпенсацияХСDataGridViewCheckBoxColumn"
        Me.КомпенсацияХСDataGridViewCheckBoxColumn.ReadOnly = True
        Me.КомпенсацияХСDataGridViewCheckBoxColumn.Width = 113
        '
        'ДопускМинимумDataGridViewTextBoxColumn
        '
        Me.ДопускМинимумDataGridViewTextBoxColumn.DataPropertyName = "ДопускМинимум"
        Me.ДопускМинимумDataGridViewTextBoxColumn.HeaderText = "Допуск Минимум"
        Me.ДопускМинимумDataGridViewTextBoxColumn.Name = "ДопускМинимумDataGridViewTextBoxColumn"
        Me.ДопускМинимумDataGridViewTextBoxColumn.ReadOnly = True
        Me.ДопускМинимумDataGridViewTextBoxColumn.Width = 132
        '
        'ДопускМаксимумDataGridViewTextBoxColumn
        '
        Me.ДопускМаксимумDataGridViewTextBoxColumn.DataPropertyName = "ДопускМаксимум"
        Me.ДопускМаксимумDataGridViewTextBoxColumn.HeaderText = "Допуск Максимум"
        Me.ДопускМаксимумDataGridViewTextBoxColumn.Name = "ДопускМаксимумDataGridViewTextBoxColumn"
        Me.ДопускМаксимумDataGridViewTextBoxColumn.ReadOnly = True
        Me.ДопускМаксимумDataGridViewTextBoxColumn.Width = 137
        '
        'РазносУминDataGridViewTextBoxColumn
        '
        Me.РазносУминDataGridViewTextBoxColumn.DataPropertyName = "РазносУмин"
        Me.РазносУминDataGridViewTextBoxColumn.HeaderText = "Разнос Умин"
        Me.РазносУминDataGridViewTextBoxColumn.Name = "РазносУминDataGridViewTextBoxColumn"
        Me.РазносУминDataGridViewTextBoxColumn.ReadOnly = True
        Me.РазносУминDataGridViewTextBoxColumn.Width = 108
        '
        'РазносУмаксDataGridViewTextBoxColumn
        '
        Me.РазносУмаксDataGridViewTextBoxColumn.DataPropertyName = "РазносУмакс"
        Me.РазносУмаксDataGridViewTextBoxColumn.HeaderText = "Разнос Умакс"
        Me.РазносУмаксDataGridViewTextBoxColumn.Name = "РазносУмаксDataGridViewTextBoxColumn"
        Me.РазносУмаксDataGridViewTextBoxColumn.ReadOnly = True
        Me.РазносУмаксDataGridViewTextBoxColumn.Width = 114
        '
        'АварийноеЗначениеМинDataGridViewTextBoxColumn
        '
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn.DataPropertyName = "АварийноеЗначениеМин"
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn.HeaderText = "Аварийное Значение Мин"
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn.Name = "АварийноеЗначениеМинDataGridViewTextBoxColumn"
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn.ReadOnly = True
        Me.АварийноеЗначениеМинDataGridViewTextBoxColumn.Width = 160
        '
        'АварийноеЗначениеМаксDataGridViewTextBoxColumn
        '
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn.DataPropertyName = "АварийноеЗначениеМакс"
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn.HeaderText = "Аварийное Значение Макс"
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn.Name = "АварийноеЗначениеМаксDataGridViewTextBoxColumn"
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn.ReadOnly = True
        Me.АварийноеЗначениеМаксDataGridViewTextBoxColumn.Width = 160
        '
        'БлокировкаDataGridViewCheckBoxColumn
        '
        Me.БлокировкаDataGridViewCheckBoxColumn.DataPropertyName = "Блокировка"
        Me.БлокировкаDataGridViewCheckBoxColumn.HeaderText = "Блокировка"
        Me.БлокировкаDataGridViewCheckBoxColumn.Name = "БлокировкаDataGridViewCheckBoxColumn"
        Me.БлокировкаDataGridViewCheckBoxColumn.ReadOnly = True
        Me.БлокировкаDataGridViewCheckBoxColumn.Width = 92
        '
        'ДатаDataGridViewTextBoxColumn
        '
        Me.ДатаDataGridViewTextBoxColumn.DataPropertyName = "Дата"
        Me.ДатаDataGridViewTextBoxColumn.HeaderText = "Дата"
        Me.ДатаDataGridViewTextBoxColumn.Name = "ДатаDataGridViewTextBoxColumn"
        Me.ДатаDataGridViewTextBoxColumn.ReadOnly = True
        Me.ДатаDataGridViewTextBoxColumn.Width = 66
        '
        'ВидимостьРегистраторDataGridViewCheckBoxColumn
        '
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn.DataPropertyName = "ВидимостьРегистратор"
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn.HeaderText = "Видимость Регистратор"
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn.Name = "ВидимостьРегистраторDataGridViewCheckBoxColumn"
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn.ReadOnly = True
        Me.ВидимостьРегистраторDataGridViewCheckBoxColumn.Width = 159
        '
        'ВидимостьDataGridViewCheckBoxColumn
        '
        Me.ВидимостьDataGridViewCheckBoxColumn.DataPropertyName = "Видимость"
        Me.ВидимостьDataGridViewCheckBoxColumn.HeaderText = "Видимость"
        Me.ВидимостьDataGridViewCheckBoxColumn.Name = "ВидимостьDataGridViewCheckBoxColumn"
        Me.ВидимостьDataGridViewCheckBoxColumn.ReadOnly = True
        Me.ВидимостьDataGridViewCheckBoxColumn.Width = 87
        '
        'ПогрешностьDataGridViewTextBoxColumn
        '
        Me.ПогрешностьDataGridViewTextBoxColumn.DataPropertyName = "Погрешность"
        Me.ПогрешностьDataGridViewTextBoxColumn.HeaderText = "Погрешность"
        Me.ПогрешностьDataGridViewTextBoxColumn.Name = "ПогрешностьDataGridViewTextBoxColumn"
        Me.ПогрешностьDataGridViewTextBoxColumn.ReadOnly = True
        Me.ПогрешностьDataGridViewTextBoxColumn.Width = 121
        '
        'FormSearchChannelTuning
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(848, 377)
        Me.Controls.Add(Me.PanelTable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(868, 420)
        Me.Name = "FormSearchChannelTuning"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Быстрый поиск канала по предполагаемому имени"
        Me.TopMost = True
        CType(Me.BindingNavigatorChannelsN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorChannelsN.ResumeLayout(False)
        Me.BindingNavigatorChannelsN.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridFindChannels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceChannelsN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelsNDataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelTable.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ChannelNTableAdapter As ChannelsNDataSetTableAdapters.ChannelNTableAdapter
    Friend WithEvents BindingNavigatorChannelsN As BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator2 As ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator3 As ToolStripSeparator
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents PanelTable As Panel
    Friend WithEvents DataGridFindChannels As DataGridView
    Friend WithEvents LabelDescriptionStage As Label
    Friend WithEvents BindingNavigatorTextBoxFilter As ToolStripTextBox
    Friend WithEvents BindingNavigatorLabelName As ToolStripLabel
    Friend WithEvents BindingNavigatorSeparator1 As ToolStripSeparator
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ChannelsNDataSet1 As ChannelsNDataSet
    Friend WithEvents BindingSourceChannelsN As BindingSource
    Friend WithEvents НомерПараметраDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НаименованиеПараметраDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ЕдиницаИзмеренияDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ПримечанияDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НомерКаналаDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НомерУстройстваDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НомерМодуляКорзиныDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НомерКаналаМодуляDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ТипПодключенияDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НижнийПределDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ВерхнийПределDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ТипСигналаDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents НомерФормулыDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents СтепеньАппроксимацииDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A0DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A1DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A2DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A3DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A4DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents A5DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents СмещениеDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents КомпенсацияХСDataGridViewCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents ДопускМинимумDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ДопускМаксимумDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents РазносУминDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents РазносУмаксDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents АварийноеЗначениеМинDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents АварийноеЗначениеМаксDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents БлокировкаDataGridViewCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents ДатаDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ВидимостьРегистраторDataGridViewCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents ВидимостьDataGridViewCheckBoxColumn As DataGridViewCheckBoxColumn
    Friend WithEvents ПогрешностьDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
End Class
