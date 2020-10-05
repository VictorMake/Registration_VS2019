<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTuningSelectiveBase
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningSelectiveBase))
        Me.ToolTipForm = New System.Windows.Forms.ToolTip(Me.components)
        Me.ListViewSource = New System.Windows.Forms.ListView()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboBoxListConfig = New System.Windows.Forms.ComboBox()
        Me.ButtonDeleteSelectedList = New System.Windows.Forms.Button()
        Me.ButtonSaveConfig = New System.Windows.Forms.Button()
        Me.ButtonRemove = New System.Windows.Forms.Button()
        Me.ButtonDown = New System.Windows.Forms.Button()
        Me.ButtonUp = New System.Windows.Forms.Button()
        Me.ButtonErase = New System.Windows.Forms.Button()
        Me.ButtonInsert = New System.Windows.Forms.Button()
        Me.ButtonAddNewConfig = New System.Windows.Forms.Button()
        Me.ButtonFindChannel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelListConfig = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelCentre = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelConfig = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelCentre = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelAll = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSelected = New System.Windows.Forms.Label()
        Me.LabelCaption = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ListViewReceiver = New Registration.ListViewCustomReorder.ListViewEx()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.PanelListConfig.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanelCentre.SuspendLayout()
        Me.TableLayoutPanelConfig.SuspendLayout()
        Me.PanelCentre.SuspendLayout()
        Me.TableLayoutPanelAll.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListViewSource
        '
        Me.ListViewSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewSource.FullRowSelect = True
        Me.ListViewSource.GridLines = True
        Me.ListViewSource.HideSelection = False
        Me.ListViewSource.Location = New System.Drawing.Point(3, 38)
        Me.ListViewSource.Name = "ListViewSource"
        Me.ListViewSource.Size = New System.Drawing.Size(288, 520)
        Me.ListViewSource.SmallImageList = Me.ImageListChannel
        Me.ListViewSource.TabIndex = 11
        Me.ToolTipForm.SetToolTip(Me.ListViewSource, "Каналы оборудования")
        Me.ListViewSource.UseCompatibleStateImageBehavior = False
        Me.ListViewSource.View = System.Windows.Forms.View.Details
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
        '
        'ComboBoxListConfig
        '
        Me.ComboBoxListConfig.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxListConfig.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxListConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxListConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBoxListConfig.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxListConfig.Location = New System.Drawing.Point(0, 24)
        Me.ComboBoxListConfig.Name = "ComboBoxListConfig"
        Me.ComboBoxListConfig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxListConfig.Size = New System.Drawing.Size(176, 204)
        Me.ComboBoxListConfig.TabIndex = 8
        Me.ToolTipForm.SetToolTip(Me.ComboBoxListConfig, "Список конфигураций")
        '
        'ButtonDeleteSelectedList
        '
        Me.ButtonDeleteSelectedList.BackgroundImage = CType(resources.GetObject("ButtonDeleteSelectedList.BackgroundImage"), System.Drawing.Image)
        Me.ButtonDeleteSelectedList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonDeleteSelectedList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDeleteSelectedList.ForeColor = System.Drawing.Color.Black
        Me.ButtonDeleteSelectedList.Location = New System.Drawing.Point(3, 155)
        Me.ButtonDeleteSelectedList.Name = "ButtonDeleteSelectedList"
        Me.ButtonDeleteSelectedList.Size = New System.Drawing.Size(88, 70)
        Me.ButtonDeleteSelectedList.TabIndex = 5
        Me.ButtonDeleteSelectedList.Text = "Удалить"
        Me.ButtonDeleteSelectedList.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonDeleteSelectedList, "Удалить имя списка из конфигурации")
        Me.ButtonDeleteSelectedList.UseVisualStyleBackColor = True
        '
        'ButtonSaveConfig
        '
        Me.ButtonSaveConfig.BackgroundImage = CType(resources.GetObject("ButtonSaveConfig.BackgroundImage"), System.Drawing.Image)
        Me.ButtonSaveConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonSaveConfig.Enabled = False
        Me.ButtonSaveConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSaveConfig.ForeColor = System.Drawing.Color.Black
        Me.ButtonSaveConfig.Location = New System.Drawing.Point(3, 79)
        Me.ButtonSaveConfig.Name = "ButtonSaveConfig"
        Me.ButtonSaveConfig.Size = New System.Drawing.Size(88, 70)
        Me.ButtonSaveConfig.TabIndex = 3
        Me.ButtonSaveConfig.Text = "Записать"
        Me.ButtonSaveConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonSaveConfig, "Записать список под именем")
        Me.ButtonSaveConfig.UseVisualStyleBackColor = True
        '
        'ButtonRemove
        '
        Me.ButtonRemove.BackgroundImage = CType(resources.GetObject("ButtonRemove.BackgroundImage"), System.Drawing.Image)
        Me.ButtonRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonRemove.ForeColor = System.Drawing.Color.Black
        Me.ButtonRemove.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonRemove.Location = New System.Drawing.Point(94, 53)
        Me.ButtonRemove.Name = "ButtonRemove"
        Me.ButtonRemove.Size = New System.Drawing.Size(90, 44)
        Me.ButtonRemove.TabIndex = 1
        Me.ButtonRemove.Text = "Удалить"
        Me.ButtonRemove.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonRemove, "Удалить канал из списка")
        Me.ButtonRemove.UseVisualStyleBackColor = True
        '
        'ButtonDown
        '
        Me.ButtonDown.BackgroundImage = CType(resources.GetObject("ButtonDown.BackgroundImage"), System.Drawing.Image)
        Me.ButtonDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDown.ForeColor = System.Drawing.Color.Black
        Me.ButtonDown.Location = New System.Drawing.Point(94, 103)
        Me.ButtonDown.Name = "ButtonDown"
        Me.ButtonDown.Size = New System.Drawing.Size(90, 44)
        Me.ButtonDown.TabIndex = 14
        Me.ButtonDown.Text = "Вниз"
        Me.ButtonDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonDown, "Переместить канал вниз по списку")
        Me.ButtonDown.UseVisualStyleBackColor = True
        '
        'ButtonUp
        '
        Me.ButtonUp.BackgroundImage = CType(resources.GetObject("ButtonUp.BackgroundImage"), System.Drawing.Image)
        Me.ButtonUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonUp.ForeColor = System.Drawing.Color.Black
        Me.ButtonUp.Location = New System.Drawing.Point(94, 153)
        Me.ButtonUp.Name = "ButtonUp"
        Me.ButtonUp.Size = New System.Drawing.Size(90, 44)
        Me.ButtonUp.TabIndex = 15
        Me.ButtonUp.Text = "Вверх"
        Me.ButtonUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonUp, "Переместить канал вверх по списку")
        Me.ButtonUp.UseVisualStyleBackColor = True
        '
        'ButtonErase
        '
        Me.ButtonErase.BackgroundImage = CType(resources.GetObject("ButtonErase.BackgroundImage"), System.Drawing.Image)
        Me.ButtonErase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonErase.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonErase.ForeColor = System.Drawing.Color.Black
        Me.ButtonErase.Location = New System.Drawing.Point(94, 203)
        Me.ButtonErase.Name = "ButtonErase"
        Me.ButtonErase.Size = New System.Drawing.Size(90, 44)
        Me.ButtonErase.TabIndex = 0
        Me.ButtonErase.Text = "Очистить"
        Me.ButtonErase.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonErase, "Очистить список")
        Me.ButtonErase.UseVisualStyleBackColor = True
        '
        'ButtonInsert
        '
        Me.ButtonInsert.BackgroundImage = CType(resources.GetObject("ButtonInsert.BackgroundImage"), System.Drawing.Image)
        Me.ButtonInsert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonInsert.ForeColor = System.Drawing.Color.Black
        Me.ButtonInsert.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonInsert.Location = New System.Drawing.Point(94, 3)
        Me.ButtonInsert.Name = "ButtonInsert"
        Me.ButtonInsert.Size = New System.Drawing.Size(90, 44)
        Me.ButtonInsert.TabIndex = 2
        Me.ButtonInsert.Text = "Добавить"
        Me.ButtonInsert.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonInsert, "Добавить канал в список")
        Me.ButtonInsert.UseVisualStyleBackColor = True
        '
        'ButtonAddNewConfig
        '
        Me.ButtonAddNewConfig.BackgroundImage = CType(resources.GetObject("ButtonAddNewConfig.BackgroundImage"), System.Drawing.Image)
        Me.ButtonAddNewConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonAddNewConfig.Enabled = False
        Me.ButtonAddNewConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonAddNewConfig.ForeColor = System.Drawing.Color.Black
        Me.ButtonAddNewConfig.Location = New System.Drawing.Point(3, 3)
        Me.ButtonAddNewConfig.Name = "ButtonAddNewConfig"
        Me.ButtonAddNewConfig.Size = New System.Drawing.Size(88, 70)
        Me.ButtonAddNewConfig.TabIndex = 26
        Me.ButtonAddNewConfig.Text = "Создать"
        Me.ButtonAddNewConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTipForm.SetToolTip(Me.ButtonAddNewConfig, "Добавить новую конфигурацию в список")
        Me.ButtonAddNewConfig.UseVisualStyleBackColor = True
        '
        'ButtonFindChannel
        '
        Me.ButtonFindChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonFindChannel.Image = CType(resources.GetObject("ButtonFindChannel.Image"), System.Drawing.Image)
        Me.ButtonFindChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFindChannel.Location = New System.Drawing.Point(0, 0)
        Me.ButtonFindChannel.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonFindChannel.Name = "ButtonFindChannel"
        Me.ButtonFindChannel.Size = New System.Drawing.Size(110, 26)
        Me.ButtonFindChannel.TabIndex = 13
        Me.ButtonFindChannel.Text = "Поиск канала"
        Me.ButtonFindChannel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTipForm.SetToolTip(Me.ButtonFindChannel, "Поиск канала по имени")
        Me.ButtonFindChannel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 24)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Существующие конфигурации"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.ButtonSaveConfig, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.ButtonAddNewConfig, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.ButtonDeleteSelectedList, 0, 2)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(185, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 3
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(94, 228)
        Me.TableLayoutPanel4.TabIndex = 24
        '
        'PanelListConfig
        '
        Me.PanelListConfig.Controls.Add(Me.ComboBoxListConfig)
        Me.PanelListConfig.Controls.Add(Me.Label1)
        Me.PanelListConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelListConfig.Location = New System.Drawing.Point(3, 3)
        Me.PanelListConfig.Name = "PanelListConfig"
        Me.PanelListConfig.Size = New System.Drawing.Size(176, 228)
        Me.PanelListConfig.TabIndex = 25
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonInsert, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonRemove, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonDown, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonUp, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonErase, 1, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(282, 254)
        Me.TableLayoutPanel1.TabIndex = 23
        '
        'TableLayoutPanelCentre
        '
        Me.TableLayoutPanelCentre.ColumnCount = 1
        Me.TableLayoutPanelCentre.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelCentre.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanelCentre.Controls.Add(Me.TableLayoutPanelConfig, 0, 1)
        Me.TableLayoutPanelCentre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelCentre.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelCentre.Name = "TableLayoutPanelCentre"
        Me.TableLayoutPanelCentre.RowCount = 2
        Me.TableLayoutPanelCentre.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelCentre.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelCentre.Size = New System.Drawing.Size(288, 520)
        Me.TableLayoutPanelCentre.TabIndex = 6
        '
        'TableLayoutPanelConfig
        '
        Me.TableLayoutPanelConfig.ColumnCount = 2
        Me.TableLayoutPanelConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanelConfig.Controls.Add(Me.TableLayoutPanel4, 1, 0)
        Me.TableLayoutPanelConfig.Controls.Add(Me.PanelListConfig, 0, 0)
        Me.TableLayoutPanelConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelConfig.Location = New System.Drawing.Point(3, 263)
        Me.TableLayoutPanelConfig.Name = "TableLayoutPanelConfig"
        Me.TableLayoutPanelConfig.RowCount = 2
        Me.TableLayoutPanelConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelConfig.Size = New System.Drawing.Size(282, 254)
        Me.TableLayoutPanelConfig.TabIndex = 0
        '
        'PanelCentre
        '
        Me.PanelCentre.Controls.Add(Me.TableLayoutPanelCentre)
        Me.PanelCentre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelCentre.Location = New System.Drawing.Point(297, 38)
        Me.PanelCentre.Name = "PanelCentre"
        Me.PanelCentre.Size = New System.Drawing.Size(288, 520)
        Me.PanelCentre.TabIndex = 22
        '
        'TableLayoutPanelAll
        '
        Me.TableLayoutPanelAll.ColumnCount = 3
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanelAll.Controls.Add(Me.PanelCentre, 1, 1)
        Me.TableLayoutPanelAll.Controls.Add(Me.ListViewSource, 0, 1)
        Me.TableLayoutPanelAll.Controls.Add(Me.ListViewReceiver, 2, 1)
        Me.TableLayoutPanelAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAll.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelAll.Name = "TableLayoutPanelAll"
        Me.TableLayoutPanelAll.RowCount = 2
        Me.TableLayoutPanelAll.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.405694!))
        Me.TableLayoutPanelAll.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.59431!))
        Me.TableLayoutPanelAll.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelAll.Size = New System.Drawing.Size(884, 561)
        Me.TableLayoutPanelAll.TabIndex = 25
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 4
        Me.TableLayoutPanelAll.SetColumnSpan(Me.TableLayoutPanel2, 3)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.LabelSelected, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.LabelCaption, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.ButtonFindChannel, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(878, 26)
        Me.TableLayoutPanel2.TabIndex = 24
        '
        'LabelSelected
        '
        Me.LabelSelected.AutoSize = True
        Me.LabelSelected.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSelected.Location = New System.Drawing.Point(829, 0)
        Me.LabelSelected.Name = "LabelSelected"
        Me.LabelSelected.Size = New System.Drawing.Size(46, 26)
        Me.LabelSelected.TabIndex = 12
        Me.LabelSelected.Text = "0"
        Me.LabelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCaption
        '
        Me.LabelCaption.AutoSize = True
        Me.LabelCaption.BackColor = System.Drawing.SystemColors.Control
        Me.LabelCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaption.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelCaption.ForeColor = System.Drawing.Color.Blue
        Me.LabelCaption.Location = New System.Drawing.Point(113, 0)
        Me.LabelCaption.Name = "LabelCaption"
        Me.LabelCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaption.Size = New System.Drawing.Size(642, 26)
        Me.LabelCaption.TabIndex = 8
        Me.LabelCaption.Text = "Отметьте какие параметры выводить на форму базового контроля"
        Me.LabelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(761, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 26)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Отмечено:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ListViewReceiver
        '
        Me.ListViewReceiver.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewReceiver.dropIndex = 0
        Me.ListViewReceiver.FullRowSelect = True
        Me.ListViewReceiver.GridLines = True
        Me.ListViewReceiver.HideSelection = False
        Me.ListViewReceiver.Location = New System.Drawing.Point(591, 38)
        Me.ListViewReceiver.Name = "ListViewReceiver"
        Me.ListViewReceiver.Size = New System.Drawing.Size(290, 520)
        Me.ListViewReceiver.SmallImageList = Me.ImageListChannel
        Me.ListViewReceiver.TabIndex = 23
        Me.ToolTipForm.SetToolTip(Me.ListViewReceiver, "Выбранные каналы для конфигурации")
        Me.ListViewReceiver.UseCompatibleStateImageBehavior = False
        Me.ListViewReceiver.View = System.Windows.Forms.View.Details
        '
        'FormTuningSelectiveBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 561)
        Me.Controls.Add(Me.TableLayoutPanelAll)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormTuningSelectiveBase"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Базовая форма настройки выборочных параметров в списке"
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.PanelListConfig.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanelCentre.ResumeLayout(False)
        Me.TableLayoutPanelConfig.ResumeLayout(False)
        Me.PanelCentre.ResumeLayout(False)
        Me.TableLayoutPanelAll.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ToolTipForm As ToolTip
    Friend WithEvents ListViewSource As ListView
    Friend WithEvents ImageListChannel As ImageList
    Public WithEvents ComboBoxListConfig As ComboBox
    Friend WithEvents ButtonDeleteSelectedList As Button
    Friend WithEvents ButtonSaveConfig As Button
    Friend WithEvents ListViewReceiver As ListViewCustomReorder.ListViewEx
    Friend WithEvents ButtonRemove As Button
    Friend WithEvents ButtonDown As Button
    Friend WithEvents ButtonUp As Button
    Friend WithEvents ButtonErase As Button
    Friend WithEvents ButtonInsert As Button
    Friend WithEvents TableLayoutPanelAll As TableLayoutPanel
    Friend WithEvents PanelCentre As Panel
    Friend WithEvents TableLayoutPanelCentre As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanelConfig As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents PanelListConfig As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents LabelSelected As Label
    Public WithEvents LabelCaption As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ButtonAddNewConfig As Button
    Friend WithEvents ButtonFindChannel As Button
End Class
