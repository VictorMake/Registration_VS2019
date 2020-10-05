<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormTuningSelectiveList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningSelectiveList))
        Me.TableLayoutPanelAll = New System.Windows.Forms.TableLayoutPanel()
        Me.cmbВыборРежима = New System.Windows.Forms.ComboBox()
        Me.PanelCentre = New System.Windows.Forms.Panel()
        Me.TableLayoutPanelCentre = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.cmdДобавить = New System.Windows.Forms.Button()
        Me.cmdУдалить = New System.Windows.Forms.Button()
        Me.ButtonDown = New System.Windows.Forms.Button()
        Me.ButtonUp = New System.Windows.Forms.Button()
        Me.cmdОчиститьВсе = New System.Windows.Forms.Button()
        Me.TableLayoutPanelConfig = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.cmdВосстановитьСписки = New System.Windows.Forms.Button()
        Me.cmdУдалитьСписок = New System.Windows.Forms.Button()
        Me.cmdЗапись = New System.Windows.Forms.Button()
        Me.PanelListConfig = New System.Windows.Forms.Panel()
        Me.comСписки = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Метка1 = New System.Windows.Forms.Label()
        Me.lvwSource = New System.Windows.Forms.ListView()
        Me.ImlКанал = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwReceiver = New Registration.ListViewCustomReorder.ListViewEx()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanelAll.SuspendLayout()
        Me.PanelCentre.SuspendLayout()
        Me.TableLayoutPanelCentre.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanelConfig.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.PanelListConfig.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelAll
        '
        Me.TableLayoutPanelAll.ColumnCount = 3
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelAll.Controls.Add(Me.cmbВыборРежима, 0, 0)
        Me.TableLayoutPanelAll.Controls.Add(Me.PanelCentre, 1, 1)
        Me.TableLayoutPanelAll.Controls.Add(Me.Метка1, 1, 0)
        Me.TableLayoutPanelAll.Controls.Add(Me.lvwSource, 0, 1)
        Me.TableLayoutPanelAll.Controls.Add(Me.lvwReceiver, 2, 1)
        Me.TableLayoutPanelAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAll.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelAll.Name = "TableLayoutPanelAll"
        Me.TableLayoutPanelAll.RowCount = 2
        Me.TableLayoutPanelAll.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.405694!))
        Me.TableLayoutPanelAll.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.59431!))
        Me.TableLayoutPanelAll.Size = New System.Drawing.Size(884, 561)
        Me.TableLayoutPanelAll.TabIndex = 24
        '
        'cmbВыборРежима
        '
        Me.cmbВыборРежима.BackColor = System.Drawing.SystemColors.Window
        Me.cmbВыборРежима.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbВыборРежима.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbВыборРежима.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbВыборРежима.Location = New System.Drawing.Point(3, 3)
        Me.cmbВыборРежима.Name = "cmbВыборРежима"
        Me.cmbВыборРежима.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbВыборРежима.Size = New System.Drawing.Size(288, 21)
        Me.cmbВыборРежима.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.cmbВыборРежима, "Выбор списка каналов связанный с режимом")
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
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.cmdДобавить, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cmdУдалить, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonDown, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonUp, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.cmdОчиститьВсе, 1, 4)
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
        'cmdДобавить
        '
        Me.cmdДобавить.BackgroundImage = CType(resources.GetObject("cmdДобавить.BackgroundImage"), System.Drawing.Image)
        Me.cmdДобавить.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdДобавить.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdДобавить.ForeColor = System.Drawing.Color.Black
        Me.cmdДобавить.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.cmdДобавить.Location = New System.Drawing.Point(94, 3)
        Me.cmdДобавить.Name = "cmdДобавить"
        Me.cmdДобавить.Size = New System.Drawing.Size(90, 44)
        Me.cmdДобавить.TabIndex = 2
        Me.cmdДобавить.Text = "Добавить"
        Me.cmdДобавить.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdДобавить, "Добавить канал в список")
        Me.cmdДобавить.UseVisualStyleBackColor = True
        '
        'cmdУдалить
        '
        Me.cmdУдалить.BackgroundImage = CType(resources.GetObject("cmdУдалить.BackgroundImage"), System.Drawing.Image)
        Me.cmdУдалить.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdУдалить.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdУдалить.ForeColor = System.Drawing.Color.Black
        Me.cmdУдалить.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.cmdУдалить.Location = New System.Drawing.Point(94, 53)
        Me.cmdУдалить.Name = "cmdУдалить"
        Me.cmdУдалить.Size = New System.Drawing.Size(90, 44)
        Me.cmdУдалить.TabIndex = 1
        Me.cmdУдалить.Text = "Удалить"
        Me.cmdУдалить.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdУдалить, "Удалить канал из списка")
        Me.cmdУдалить.UseVisualStyleBackColor = True
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
        Me.ToolTip1.SetToolTip(Me.ButtonDown, "Переместить строку вниз")
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
        Me.ToolTip1.SetToolTip(Me.ButtonUp, "Переместить строку вверх")
        Me.ButtonUp.UseVisualStyleBackColor = True
        '
        'cmdОчиститьВсе
        '
        Me.cmdОчиститьВсе.BackgroundImage = CType(resources.GetObject("cmdОчиститьВсе.BackgroundImage"), System.Drawing.Image)
        Me.cmdОчиститьВсе.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdОчиститьВсе.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdОчиститьВсе.ForeColor = System.Drawing.Color.Black
        Me.cmdОчиститьВсе.Location = New System.Drawing.Point(94, 203)
        Me.cmdОчиститьВсе.Name = "cmdОчиститьВсе"
        Me.cmdОчиститьВсе.Size = New System.Drawing.Size(90, 46)
        Me.cmdОчиститьВсе.TabIndex = 0
        Me.cmdОчиститьВсе.Text = "Очистить"
        Me.cmdОчиститьВсе.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdОчиститьВсе, "Очистить весь список")
        Me.cmdОчиститьВсе.UseVisualStyleBackColor = True
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
        Me.TableLayoutPanelConfig.RowCount = 1
        Me.TableLayoutPanelConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelConfig.Size = New System.Drawing.Size(282, 254)
        Me.TableLayoutPanelConfig.TabIndex = 0
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.cmdВосстановитьСписки, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdУдалитьСписок, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdЗапись, 0, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(185, 54)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 3
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(94, 197)
        Me.TableLayoutPanel4.TabIndex = 24
        '
        'cmdВосстановитьСписки
        '
        Me.cmdВосстановитьСписки.BackgroundImage = CType(resources.GetObject("cmdВосстановитьСписки.BackgroundImage"), System.Drawing.Image)
        Me.cmdВосстановитьСписки.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdВосстановитьСписки.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdВосстановитьСписки.ForeColor = System.Drawing.Color.Black
        Me.cmdВосстановитьСписки.Location = New System.Drawing.Point(3, 3)
        Me.cmdВосстановитьСписки.Name = "cmdВосстановитьСписки"
        Me.cmdВосстановитьСписки.Size = New System.Drawing.Size(88, 55)
        Me.cmdВосстановитьСписки.TabIndex = 4
        Me.cmdВосстановитьСписки.Text = "Считать"
        Me.cmdВосстановитьСписки.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdВосстановитьСписки, "Считать выбранный список")
        Me.cmdВосстановитьСписки.UseVisualStyleBackColor = True
        '
        'cmdУдалитьСписок
        '
        Me.cmdУдалитьСписок.BackgroundImage = CType(resources.GetObject("cmdУдалитьСписок.BackgroundImage"), System.Drawing.Image)
        Me.cmdУдалитьСписок.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdУдалитьСписок.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdУдалитьСписок.ForeColor = System.Drawing.Color.Black
        Me.cmdУдалитьСписок.Location = New System.Drawing.Point(3, 133)
        Me.cmdУдалитьСписок.Name = "cmdУдалитьСписок"
        Me.cmdУдалитьСписок.Size = New System.Drawing.Size(88, 55)
        Me.cmdУдалитьСписок.TabIndex = 5
        Me.cmdУдалитьСписок.Text = "Удалить"
        Me.cmdУдалитьСписок.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdУдалитьСписок, "Удалить имя списка из базы")
        Me.cmdУдалитьСписок.UseVisualStyleBackColor = True
        '
        'cmdЗапись
        '
        Me.cmdЗапись.BackgroundImage = CType(resources.GetObject("cmdЗапись.BackgroundImage"), System.Drawing.Image)
        Me.cmdЗапись.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdЗапись.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cmdЗапись.ForeColor = System.Drawing.Color.Black
        Me.cmdЗапись.Location = New System.Drawing.Point(3, 68)
        Me.cmdЗапись.Name = "cmdЗапись"
        Me.cmdЗапись.Size = New System.Drawing.Size(88, 55)
        Me.cmdЗапись.TabIndex = 3
        Me.cmdЗапись.Text = "Записать"
        Me.cmdЗапись.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdЗапись, "Записать список под именем")
        Me.cmdЗапись.UseVisualStyleBackColor = True
        '
        'PanelListConfig
        '
        Me.PanelListConfig.Controls.Add(Me.comСписки)
        Me.PanelListConfig.Controls.Add(Me.Label1)
        Me.PanelListConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelListConfig.Location = New System.Drawing.Point(3, 3)
        Me.PanelListConfig.Name = "PanelListConfig"
        Me.PanelListConfig.Size = New System.Drawing.Size(176, 248)
        Me.PanelListConfig.TabIndex = 25
        '
        'comСписки
        '
        Me.comСписки.BackColor = System.Drawing.SystemColors.Window
        Me.comСписки.Cursor = System.Windows.Forms.Cursors.Default
        Me.comСписки.Dock = System.Windows.Forms.DockStyle.Fill
        Me.comСписки.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.comСписки.ForeColor = System.Drawing.SystemColors.WindowText
        Me.comСписки.Location = New System.Drawing.Point(0, 24)
        Me.comСписки.Name = "comСписки"
        Me.comСписки.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.comСписки.Size = New System.Drawing.Size(176, 224)
        Me.comСписки.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.comСписки, "Список конфигураций")
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
        'Метка1
        '
        Me.Метка1.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanelAll.SetColumnSpan(Me.Метка1, 2)
        Me.Метка1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Метка1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Метка1.ForeColor = System.Drawing.Color.Blue
        Me.Метка1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Метка1.Location = New System.Drawing.Point(297, 0)
        Me.Метка1.Name = "Метка1"
        Me.Метка1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Метка1.Size = New System.Drawing.Size(584, 35)
        Me.Метка1.TabIndex = 2
        Me.Метка1.Text = "Отметьте какие параметры выводить в список ""Выборочно"" в нужном порядке или выбра" &
    "ть отображение из существующего режима"
        '
        'lvwSource
        '
        Me.lvwSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwSource.FullRowSelect = True
        Me.lvwSource.GridLines = True
        Me.lvwSource.HideSelection = False
        Me.lvwSource.Location = New System.Drawing.Point(3, 38)
        Me.lvwSource.Name = "lvwSource"
        Me.lvwSource.Size = New System.Drawing.Size(288, 520)
        Me.lvwSource.SmallImageList = Me.ImlКанал
        Me.lvwSource.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.lvwSource, "Каналы Сервера")
        Me.lvwSource.UseCompatibleStateImageBehavior = False
        Me.lvwSource.View = System.Windows.Forms.View.Details
        '
        'ImlКанал
        '
        Me.ImlКанал.ImageStream = CType(resources.GetObject("ImlКанал.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImlКанал.TransparentColor = System.Drawing.Color.Transparent
        Me.ImlКанал.Images.SetKeyName(0, "")
        Me.ImlКанал.Images.SetKeyName(1, "")
        Me.ImlКанал.Images.SetKeyName(2, "")
        Me.ImlКанал.Images.SetKeyName(3, "")
        Me.ImlКанал.Images.SetKeyName(4, "")
        Me.ImlКанал.Images.SetKeyName(5, "")
        Me.ImlКанал.Images.SetKeyName(6, "")
        Me.ImlКанал.Images.SetKeyName(7, "")
        Me.ImlКанал.Images.SetKeyName(8, "")
        Me.ImlКанал.Images.SetKeyName(9, "")
        Me.ImlКанал.Images.SetKeyName(10, "")
        Me.ImlКанал.Images.SetKeyName(11, "")
        '
        'lvwReceiver
        '
        Me.lvwReceiver.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwReceiver.dropIndex = 0
        Me.lvwReceiver.FullRowSelect = True
        Me.lvwReceiver.GridLines = True
        Me.lvwReceiver.HideSelection = False
        Me.lvwReceiver.Location = New System.Drawing.Point(591, 38)
        Me.lvwReceiver.Name = "lvwReceiver"
        Me.lvwReceiver.Size = New System.Drawing.Size(290, 520)
        Me.lvwReceiver.SmallImageList = Me.ImlКанал
        Me.lvwReceiver.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.lvwReceiver, "Выбранные каналы для конфигурации")
        Me.lvwReceiver.UseCompatibleStateImageBehavior = False
        Me.lvwReceiver.View = System.Windows.Forms.View.Details
        '
        'FormTuningSelectiveList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 561)
        Me.Controls.Add(Me.TableLayoutPanelAll)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormTuningSelectiveList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настройка выборочного отображения параметров в списке ""Выборочно"""
        Me.TableLayoutPanelAll.ResumeLayout(False)
        Me.PanelCentre.ResumeLayout(False)
        Me.TableLayoutPanelCentre.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanelConfig.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.PanelListConfig.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelAll As TableLayoutPanel
    Public WithEvents cmbВыборРежима As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PanelCentre As Panel
    Friend WithEvents TableLayoutPanelCentre As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents cmdДобавить As Button
    Friend WithEvents cmdУдалить As Button
    Friend WithEvents ButtonDown As Button
    Friend WithEvents ButtonUp As Button
    Friend WithEvents cmdОчиститьВсе As Button
    Friend WithEvents TableLayoutPanelConfig As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents cmdВосстановитьСписки As Button
    Friend WithEvents cmdУдалитьСписок As Button
    Friend WithEvents cmdЗапись As Button
    Friend WithEvents PanelListConfig As Panel
    Public WithEvents comСписки As ComboBox
    Friend WithEvents Label1 As Label
    Public WithEvents Метка1 As Label
    Friend WithEvents lvwSource As ListView
    Friend WithEvents ImlКанал As ImageList
    Friend WithEvents lvwReceiver As ListViewCustomReorder.ListViewEx
End Class
