<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectChannelDialogForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectChannelDialogForm))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.label3 = New System.Windows.Forms.Label()
        Me.ChannelsChassisDataSetFrm = New ChannelsChassisData()
        Me.label2 = New System.Windows.Forms.Label()
        Me.TextBoxChannelName = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ParentIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsChannelDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ChannelsChassisDataBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.label5 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.TextBoxShassis = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DataTreeView1 = New DataTreeViewDLL.Chaliy.Windows.Forms.DataTreeView()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.ChannelsChassisDataSetFrm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChannelsChassisDataBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonOK, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonCancel, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(126, 449)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.ButtonOK.Enabled = False
        Me.ButtonOK.Location = New System.Drawing.Point(3, 3)
        Me.ButtonOK.Name = "OK_Button"
        Me.ButtonOK.Size = New System.Drawing.Size(67, 23)
        Me.ButtonOK.TabIndex = 0
        Me.ButtonOK.Text = "ОК"
        '
        'Cancel_Button
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(76, 3)
        Me.ButtonCancel.Name = "Cancel_Button"
        Me.ButtonCancel.Size = New System.Drawing.Size(67, 23)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Отмена"
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.imageList1.Images.SetKeyName(0, "NotSelected")
        Me.imageList1.Images.SetKeyName(1, "Selected")
        Me.imageList1.Images.SetKeyName(2, "")
        Me.imageList1.Images.SetKeyName(3, "")
        Me.imageList1.Images.SetKeyName(4, "endturn.png")
        '
        'label3
        '
        Me.label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ChannelsChassisDataSetFrm, "Test.ParentID", True))
        Me.label3.Location = New System.Drawing.Point(60, 424)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(212, 16)
        Me.label3.TabIndex = 31
        Me.label3.Visible = False
        '
        'ChannelsChassisDataSetFrm
        '
        Me.ChannelsChassisDataSetFrm.DataSetName = "ChannelsChassisData"
        Me.ChannelsChassisDataSetFrm.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ChannelsChassisDataSetFrm.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'label2
        '
        Me.label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ChannelsChassisDataSetFrm, "Test.ID", True))
        Me.label2.Location = New System.Drawing.Point(60, 383)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(212, 15)
        Me.label2.TabIndex = 30
        Me.label2.Visible = False
        '
        'ChannelNameTextBox
        '
        Me.TextBoxChannelName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxChannelName.BackColor = System.Drawing.SystemColors.Control
        Me.TextBoxChannelName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ChannelsChassisDataSetFrm, "Test.Name", True))
        Me.TextBoxChannelName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxChannelName.Location = New System.Drawing.Point(60, 401)
        Me.TextBoxChannelName.Name = "ChannelNameTextBox"
        Me.TextBoxChannelName.ReadOnly = True
        Me.TextBoxChannelName.Size = New System.Drawing.Size(212, 20)
        Me.TextBoxChannelName.TabIndex = 29
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.NameDataGridViewTextBoxColumn, Me.ParentIDDataGridViewTextBoxColumn, Me.IsChannelDataGridViewCheckBoxColumn})
        Me.DataGridView1.DataMember = "Test"
        Me.DataGridView1.DataSource = Me.ChannelsChassisDataSetFrm
        Me.DataGridView1.Location = New System.Drawing.Point(12, 177)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(260, 177)
        Me.DataGridView1.TabIndex = 33
        '
        'IDDataGridViewTextBoxColumn
        '
        Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
        Me.IDDataGridViewTextBoxColumn.HeaderText = "ID"
        Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
        Me.IDDataGridViewTextBoxColumn.ReadOnly = True
        Me.IDDataGridViewTextBoxColumn.Visible = False
        Me.IDDataGridViewTextBoxColumn.Width = 43
        '
        'NameDataGridViewTextBoxColumn
        '
        Me.NameDataGridViewTextBoxColumn.DataPropertyName = "Name"
        Me.NameDataGridViewTextBoxColumn.HeaderText = "Имя"
        Me.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn"
        Me.NameDataGridViewTextBoxColumn.ReadOnly = True
        Me.NameDataGridViewTextBoxColumn.Width = 54
        '
        'ParentIDDataGridViewTextBoxColumn
        '
        Me.ParentIDDataGridViewTextBoxColumn.DataPropertyName = "ParentID"
        Me.ParentIDDataGridViewTextBoxColumn.HeaderText = "ParentID"
        Me.ParentIDDataGridViewTextBoxColumn.Name = "ParentIDDataGridViewTextBoxColumn"
        Me.ParentIDDataGridViewTextBoxColumn.ReadOnly = True
        Me.ParentIDDataGridViewTextBoxColumn.Visible = False
        Me.ParentIDDataGridViewTextBoxColumn.Width = 74
        '
        'IsChannelDataGridViewCheckBoxColumn
        '
        Me.IsChannelDataGridViewCheckBoxColumn.DataPropertyName = "IsChannel"
        Me.IsChannelDataGridViewCheckBoxColumn.HeaderText = "Можно выбрать"
        Me.IsChannelDataGridViewCheckBoxColumn.Name = "IsChannelDataGridViewCheckBoxColumn"
        Me.IsChannelDataGridViewCheckBoxColumn.ReadOnly = True
        Me.IsChannelDataGridViewCheckBoxColumn.Width = 85
        '
        'ChannelsChassisDataBindingSource
        '
        Me.ChannelsChassisDataBindingSource.DataSource = Me.ChannelsChassisDataSetFrm
        Me.ChannelsChassisDataBindingSource.Position = 0
        '
        'label5
        '
        Me.label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.label5.Location = New System.Drawing.Point(12, 424)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(48, 16)
        Me.label5.TabIndex = 27
        Me.label5.Text = "Parent:"
        Me.label5.Visible = False
        '
        'label4
        '
        Me.label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.label4.Location = New System.Drawing.Point(12, 404)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(48, 16)
        Me.label4.TabIndex = 25
        Me.label4.Text = "Имя:"
        '
        'label1
        '
        Me.label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.label1.Location = New System.Drawing.Point(12, 383)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(32, 15)
        Me.label1.TabIndex = 24
        Me.label1.Text = "ID:"
        Me.label1.Visible = False
        '
        'ShassisTextBox
        '
        Me.TextBoxShassis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxShassis.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxShassis.Location = New System.Drawing.Point(60, 360)
        Me.TextBoxShassis.Name = "ShassisTextBox"
        Me.TextBoxShassis.ReadOnly = True
        Me.TextBoxShassis.Size = New System.Drawing.Size(212, 20)
        Me.TextBoxShassis.TabIndex = 36
        Me.TextBoxShassis.Visible = False
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 363)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 16)
        Me.Label6.TabIndex = 35
        Me.Label6.Text = "Шасси:"
        '
        'DataTreeView1
        '
        Me.DataTreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataTreeView1.DataMember = "Test"
        Me.DataTreeView1.DataSource = Me.ChannelsChassisDataSetFrm
        Me.DataTreeView1.FullRowSelect = True
        Me.DataTreeView1.HideSelection = False
        Me.DataTreeView1.HotTracking = True
        Me.DataTreeView1.IDColumn = "ID"
        Me.DataTreeView1.ImageIndex = 0
        Me.DataTreeView1.ImageList = Me.imageList1
        Me.DataTreeView1.IsChannelColumn = "IsChannel"
        Me.DataTreeView1.Location = New System.Drawing.Point(12, 12)
        Me.DataTreeView1.Name = "DataTreeView1"
        Me.DataTreeView1.NameColumn = "Name"
        Me.DataTreeView1.ParentIDColumn = "ParentID"
        Me.DataTreeView1.SelectedImageIndex = 1
        Me.DataTreeView1.Size = New System.Drawing.Size(260, 159)
        Me.DataTreeView1.TabIndex = 28
        Me.DataTreeView1.ValueColumn = "ID"
        '
        'SelectChannelDialogForm
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(284, 482)
        Me.Controls.Add(Me.TextBoxShassis)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.DataTreeView1)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.TextBoxChannelName)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectChannelDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Выбрать канал для тарировки"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.ChannelsChassisDataSetFrm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChannelsChassisDataBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents DataTreeView1 As DataTreeViewDLL.Chaliy.Windows.Forms.DataTreeView
    Private WithEvents imageList1 As System.Windows.Forms.ImageList
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents TextBoxChannelName As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ParentIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsChannelDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ChannelsChassisDataBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ChannelsChassisDataSetFrm As ChannelsChassisData
    Private WithEvents TextBoxShassis As System.Windows.Forms.TextBox
    Private WithEvents Label6 As System.Windows.Forms.Label

End Class
