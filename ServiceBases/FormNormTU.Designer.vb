<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormNormTU
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormNormTU))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.NormTuBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorNormTu = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PanelTable = New System.Windows.Forms.Panel()
        Me.DataGridChannels = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.LabelDescriptionStage = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.NormTuDataSet = New Registration.NormTuDataSet()
        Me.NormTuTableAdapter = New Registration.NormTuDataSetTableAdapters.ТехническиеУсловияTableAdapter()
        Me.НомерПараметраDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ПараметрDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие99АБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие99АУБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие99АОРDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие99ББDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие99БУБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие39БDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие39УБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИзделиеМ1БDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие39сер3БDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Изделие39сер3УБDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.NormTuBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorNormTu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorNormTu.SuspendLayout()
        Me.PanelTable.SuspendLayout()
        CType(Me.DataGridChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.NormTuDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NormTuBindingSource
        '
        Me.NormTuBindingSource.DataMember = "ТехническиеУсловия"
        Me.NormTuBindingSource.DataSource = Me.NormTuDataSet
        '
        'BindingNavigatorNormTu
        '
        Me.BindingNavigatorNormTu.AddNewItem = Nothing
        Me.BindingNavigatorNormTu.BindingSource = Me.NormTuBindingSource
        Me.BindingNavigatorNormTu.CountItem = Me.BindingNavigatorCountItem
        Me.BindingNavigatorNormTu.DeleteItem = Nothing
        Me.BindingNavigatorNormTu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.BindingNavigatorNormTu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2})
        Me.BindingNavigatorNormTu.Location = New System.Drawing.Point(5, 5)
        Me.BindingNavigatorNormTu.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.BindingNavigatorNormTu.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.BindingNavigatorNormTu.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.BindingNavigatorNormTu.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.BindingNavigatorNormTu.Name = "BindingNavigatorNormTu"
        Me.BindingNavigatorNormTu.PositionItem = Me.BindingNavigatorPositionItem
        Me.BindingNavigatorNormTu.Size = New System.Drawing.Size(770, 25)
        Me.BindingNavigatorNormTu.TabIndex = 19
        Me.BindingNavigatorNormTu.Text = "BindingNavigatorChannelsN"
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
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'PanelTable
        '
        Me.PanelTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelTable.Controls.Add(Me.DataGridChannels)
        Me.PanelTable.Controls.Add(Me.TableLayoutPanel1)
        Me.PanelTable.Controls.Add(Me.LabelDescriptionStage)
        Me.PanelTable.Controls.Add(Me.BindingNavigatorNormTu)
        Me.PanelTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTable.Location = New System.Drawing.Point(0, 0)
        Me.PanelTable.Name = "PanelTable"
        Me.PanelTable.Padding = New System.Windows.Forms.Padding(5)
        Me.PanelTable.Size = New System.Drawing.Size(784, 561)
        Me.PanelTable.TabIndex = 41
        '
        'DataGridChannels
        '
        Me.DataGridChannels.AllowUserToAddRows = False
        Me.DataGridChannels.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.DataGridChannels.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridChannels.AutoGenerateColumns = False
        Me.DataGridChannels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridChannels.BackgroundColor = System.Drawing.Color.Lavender
        Me.DataGridChannels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridChannels.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridChannels.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridChannels.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.НомерПараметраDataGridViewTextBoxColumn, Me.ПараметрDataGridViewTextBoxColumn, Me.Изделие99АБDataGridViewTextBoxColumn, Me.Изделие99АУБDataGridViewTextBoxColumn, Me.Изделие99АОРDataGridViewTextBoxColumn, Me.Изделие99ББDataGridViewTextBoxColumn, Me.Изделие99БУБDataGridViewTextBoxColumn, Me.Изделие39БDataGridViewTextBoxColumn, Me.Изделие39УБDataGridViewTextBoxColumn, Me.ИзделиеМ1БDataGridViewTextBoxColumn, Me.ИзделиеМ1УБDataGridViewTextBoxColumn, Me.ИзделиеМ1251БDataGridViewTextBoxColumn, Me.ИзделиеМ1251УБDataGridViewTextBoxColumn, Me.Изделие39сер3БDataGridViewTextBoxColumn, Me.Изделие39сер3УБDataGridViewTextBoxColumn})
        Me.DataGridChannels.DataSource = Me.NormTuBindingSource
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.PaleGreen
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridChannels.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridChannels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridChannels.GridColor = System.Drawing.Color.RoyalBlue
        Me.DataGridChannels.Location = New System.Drawing.Point(5, 47)
        Me.DataGridChannels.MultiSelect = False
        Me.DataGridChannels.Name = "DataGridChannels"
        Me.DataGridChannels.ReadOnly = True
        Me.DataGridChannels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridChannels.Size = New System.Drawing.Size(770, 465)
        Me.DataGridChannels.TabIndex = 38
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonClose, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(5, 512)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(770, 40)
        Me.TableLayoutPanel1.TabIndex = 40
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.BackColor = System.Drawing.Color.Silver
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Image = CType(resources.GetObject("ButtonClose.Image"), System.Drawing.Image)
        Me.ButtonClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonClose.Location = New System.Drawing.Point(639, 5)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(128, 32)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "&Закрыть"
        Me.ToolTip1.SetToolTip(Me.ButtonClose, "Закрыть форму")
        Me.ButtonClose.UseVisualStyleBackColor = False
        '
        'LabelDescriptionStage
        '
        Me.LabelDescriptionStage.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelDescriptionStage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelDescriptionStage.ForeColor = System.Drawing.Color.Blue
        Me.LabelDescriptionStage.Location = New System.Drawing.Point(5, 30)
        Me.LabelDescriptionStage.Name = "LabelDescriptionStage"
        Me.LabelDescriptionStage.Size = New System.Drawing.Size(770, 17)
        Me.LabelDescriptionStage.TabIndex = 37
        Me.LabelDescriptionStage.Text = "Технические условия изделий"
        Me.LabelDescriptionStage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'NormTuDataSet
        '
        Me.NormTuDataSet.DataSetName = "NormTuDataSet"
        Me.NormTuDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'NormTuTableAdapter
        '
        Me.NormTuTableAdapter.ClearBeforeFill = True
        '
        'НомерПараметраDataGridViewTextBoxColumn
        '
        Me.НомерПараметраDataGridViewTextBoxColumn.DataPropertyName = "НомерПараметра"
        Me.НомерПараметраDataGridViewTextBoxColumn.Frozen = True
        Me.НомерПараметраDataGridViewTextBoxColumn.HeaderText = "Номер"
        Me.НомерПараметраDataGridViewTextBoxColumn.Name = "НомерПараметраDataGridViewTextBoxColumn"
        Me.НомерПараметраDataGridViewTextBoxColumn.ReadOnly = True
        Me.НомерПараметраDataGridViewTextBoxColumn.Visible = False
        Me.НомерПараметраDataGridViewTextBoxColumn.Width = 76
        '
        'ПараметрDataGridViewTextBoxColumn
        '
        Me.ПараметрDataGridViewTextBoxColumn.DataPropertyName = "Параметр"
        Me.ПараметрDataGridViewTextBoxColumn.Frozen = True
        Me.ПараметрDataGridViewTextBoxColumn.HeaderText = "Параметр"
        Me.ПараметрDataGridViewTextBoxColumn.Name = "ПараметрDataGridViewTextBoxColumn"
        Me.ПараметрDataGridViewTextBoxColumn.ReadOnly = True
        '
        'Изделие99АБDataGridViewTextBoxColumn
        '
        Me.Изделие99АБDataGridViewTextBoxColumn.DataPropertyName = "Изделие99А-Б"
        Me.Изделие99АБDataGridViewTextBoxColumn.HeaderText = "99А-Б"
        Me.Изделие99АБDataGridViewTextBoxColumn.Name = "Изделие99АБDataGridViewTextBoxColumn"
        Me.Изделие99АБDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие99АБDataGridViewTextBoxColumn.Width = 70
        '
        'Изделие99АУБDataGridViewTextBoxColumn
        '
        Me.Изделие99АУБDataGridViewTextBoxColumn.DataPropertyName = "Изделие99А-УБ"
        Me.Изделие99АУБDataGridViewTextBoxColumn.HeaderText = "99А-УБ"
        Me.Изделие99АУБDataGridViewTextBoxColumn.Name = "Изделие99АУБDataGridViewTextBoxColumn"
        Me.Изделие99АУБDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие99АУБDataGridViewTextBoxColumn.Width = 79
        '
        'Изделие99АОРDataGridViewTextBoxColumn
        '
        Me.Изделие99АОРDataGridViewTextBoxColumn.DataPropertyName = "Изделие99А-ОР"
        Me.Изделие99АОРDataGridViewTextBoxColumn.HeaderText = "99А-ОР"
        Me.Изделие99АОРDataGridViewTextBoxColumn.Name = "Изделие99АОРDataGridViewTextBoxColumn"
        Me.Изделие99АОРDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие99АОРDataGridViewTextBoxColumn.Width = 80
        '
        'Изделие99ББDataGridViewTextBoxColumn
        '
        Me.Изделие99ББDataGridViewTextBoxColumn.DataPropertyName = "Изделие99Б-Б"
        Me.Изделие99ББDataGridViewTextBoxColumn.HeaderText = "99Б-Б"
        Me.Изделие99ББDataGridViewTextBoxColumn.Name = "Изделие99ББDataGridViewTextBoxColumn"
        Me.Изделие99ББDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие99ББDataGridViewTextBoxColumn.Width = 71
        '
        'Изделие99БУБDataGridViewTextBoxColumn
        '
        Me.Изделие99БУБDataGridViewTextBoxColumn.DataPropertyName = "Изделие99Б-УБ"
        Me.Изделие99БУБDataGridViewTextBoxColumn.HeaderText = "99Б-УБ"
        Me.Изделие99БУБDataGridViewTextBoxColumn.Name = "Изделие99БУБDataGridViewTextBoxColumn"
        Me.Изделие99БУБDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие99БУБDataGridViewTextBoxColumn.Width = 80
        '
        'Изделие39БDataGridViewTextBoxColumn
        '
        Me.Изделие39БDataGridViewTextBoxColumn.DataPropertyName = "Изделие39-Б"
        Me.Изделие39БDataGridViewTextBoxColumn.HeaderText = "39-Б"
        Me.Изделие39БDataGridViewTextBoxColumn.Name = "Изделие39БDataGridViewTextBoxColumn"
        Me.Изделие39БDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие39БDataGridViewTextBoxColumn.Width = 62
        '
        'Изделие39УБDataGridViewTextBoxColumn
        '
        Me.Изделие39УБDataGridViewTextBoxColumn.DataPropertyName = "Изделие39-УБ"
        Me.Изделие39УБDataGridViewTextBoxColumn.HeaderText = "39-УБ"
        Me.Изделие39УБDataGridViewTextBoxColumn.Name = "Изделие39УБDataGridViewTextBoxColumn"
        Me.Изделие39УБDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие39УБDataGridViewTextBoxColumn.Width = 71
        '
        'ИзделиеМ1БDataGridViewTextBoxColumn
        '
        Me.ИзделиеМ1БDataGridViewTextBoxColumn.DataPropertyName = "ИзделиеМ1-Б"
        Me.ИзделиеМ1БDataGridViewTextBoxColumn.HeaderText = "М1-Б"
        Me.ИзделиеМ1БDataGridViewTextBoxColumn.Name = "ИзделиеМ1БDataGridViewTextBoxColumn"
        Me.ИзделиеМ1БDataGridViewTextBoxColumn.ReadOnly = True
        Me.ИзделиеМ1БDataGridViewTextBoxColumn.Width = 66
        '
        'ИзделиеМ1УБDataGridViewTextBoxColumn
        '
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn.DataPropertyName = "ИзделиеМ1-УБ"
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn.HeaderText = "М1-УБ"
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn.Name = "ИзделиеМ1УБDataGridViewTextBoxColumn"
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn.ReadOnly = True
        Me.ИзделиеМ1УБDataGridViewTextBoxColumn.Width = 75
        '
        'ИзделиеМ1251БDataGridViewTextBoxColumn
        '
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn.DataPropertyName = "ИзделиеМ1_25_1-Б"
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn.HeaderText = "М1 25 1-Б"
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn.Name = "ИзделиеМ1251БDataGridViewTextBoxColumn"
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn.ReadOnly = True
        Me.ИзделиеМ1251БDataGridViewTextBoxColumn.Width = 98
        '
        'ИзделиеМ1251УБDataGridViewTextBoxColumn
        '
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn.DataPropertyName = "ИзделиеМ1_25_1-УБ"
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn.HeaderText = "М1 25 1-УБ"
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn.Name = "ИзделиеМ1251УБDataGridViewTextBoxColumn"
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn.ReadOnly = True
        Me.ИзделиеМ1251УБDataGridViewTextBoxColumn.Width = 107
        '
        'Изделие39сер3БDataGridViewTextBoxColumn
        '
        Me.Изделие39сер3БDataGridViewTextBoxColumn.DataPropertyName = "Изделие39_сер_3-Б"
        Me.Изделие39сер3БDataGridViewTextBoxColumn.HeaderText = "39 сер. 3-Б"
        Me.Изделие39сер3БDataGridViewTextBoxColumn.Name = "Изделие39сер3БDataGridViewTextBoxColumn"
        Me.Изделие39сер3БDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие39сер3БDataGridViewTextBoxColumn.Width = 105
        '
        'Изделие39сер3УБDataGridViewTextBoxColumn
        '
        Me.Изделие39сер3УБDataGridViewTextBoxColumn.DataPropertyName = "Изделие39_сер_3-УБ"
        Me.Изделие39сер3УБDataGridViewTextBoxColumn.HeaderText = "39 сер. 3-УБ"
        Me.Изделие39сер3УБDataGridViewTextBoxColumn.Name = "Изделие39сер3УБDataGridViewTextBoxColumn"
        Me.Изделие39сер3УБDataGridViewTextBoxColumn.ReadOnly = True
        Me.Изделие39сер3УБDataGridViewTextBoxColumn.Width = 114
        '
        'FormNormTU2
        '
        Me.AcceptButton = Me.ButtonClose
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.PanelTable)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FormNormTU2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Нормы ТУ"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.NormTuBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorNormTu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorNormTu.ResumeLayout(False)
        Me.BindingNavigatorNormTu.PerformLayout()
        Me.PanelTable.ResumeLayout(False)
        Me.PanelTable.PerformLayout()
        CType(Me.DataGridChannels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.NormTuDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NormTuBindingSource As BindingSource
    Friend WithEvents BindingNavigatorNormTu As BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As ToolStripSeparator
    Friend WithEvents PanelTable As Panel
    Friend WithEvents DataGridChannels As DataGridView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ButtonClose As Button
    Friend WithEvents LabelDescriptionStage As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents NormTuDataSet As NormTuDataSet
    Friend WithEvents NormTuTableAdapter As NormTuDataSetTableAdapters.ТехническиеУсловияTableAdapter
    Friend WithEvents НомерПараметраDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ПараметрDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие99АБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие99АУБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие99АОРDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие99ББDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие99БУБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие39БDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие39УБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ИзделиеМ1БDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ИзделиеМ1УБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ИзделиеМ1251БDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents ИзделиеМ1251УБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие39сер3БDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Изделие39сер3УБDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
End Class
