<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTuningRegime
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningRegime))
        Me.ComboBoxSelectRegime = New System.Windows.Forms.ComboBox()
        Me.ListViewSelectedChannels = New System.Windows.Forms.ListView()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.BottonSave = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonFindChannel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonSelectAll = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBoxSelectRegime
        '
        Me.ComboBoxSelectRegime.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxSelectRegime.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxSelectRegime.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxSelectRegime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxSelectRegime.Location = New System.Drawing.Point(3, 35)
        Me.ComboBoxSelectRegime.Name = "ComboBoxSelectRegime"
        Me.ComboBoxSelectRegime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxSelectRegime.Size = New System.Drawing.Size(267, 21)
        Me.ComboBoxSelectRegime.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.ComboBoxSelectRegime, "Выбрать режим для настройки")
        '
        'ListViewSelectedChannels
        '
        Me.ListViewSelectedChannels.CheckBoxes = True
        Me.ListViewSelectedChannels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewSelectedChannels.FullRowSelect = True
        Me.ListViewSelectedChannels.GridLines = True
        Me.ListViewSelectedChannels.HideSelection = False
        Me.ListViewSelectedChannels.Location = New System.Drawing.Point(0, 66)
        Me.ListViewSelectedChannels.MultiSelect = False
        Me.ListViewSelectedChannels.Name = "ListViewSelectedChannels"
        Me.ListViewSelectedChannels.Size = New System.Drawing.Size(684, 695)
        Me.ListViewSelectedChannels.SmallImageList = Me.ImageListChannel
        Me.ListViewSelectedChannels.TabIndex = 11
        Me.ListViewSelectedChannels.UseCompatibleStateImageBehavior = False
        Me.ListViewSelectedChannels.View = System.Windows.Forms.View.Details
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
        'BottonSave
        '
        Me.BottonSave.BackColor = System.Drawing.SystemColors.Control
        Me.BottonSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.BottonSave.Dock = System.Windows.Forms.DockStyle.Top
        Me.BottonSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BottonSave.Location = New System.Drawing.Point(548, 35)
        Me.BottonSave.Name = "BottonSave"
        Me.BottonSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BottonSave.Size = New System.Drawing.Size(133, 23)
        Me.BottonSave.TabIndex = 0
        Me.BottonSave.Text = "Записать изменения"
        Me.ToolTip1.SetToolTip(Me.BottonSave, "Сохранить изменения")
        Me.BottonSave.UseVisualStyleBackColor = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonFindChannel, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BottonSave, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxSelectRegime, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonSelectAll, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonClear, 2, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(684, 66)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'ButtonFindChannel
        '
        Me.ButtonFindChannel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonFindChannel.Image = CType(resources.GetObject("ButtonFindChannel.Image"), System.Drawing.Image)
        Me.ButtonFindChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFindChannel.Location = New System.Drawing.Point(548, 3)
        Me.ButtonFindChannel.Name = "ButtonFindChannel"
        Me.ButtonFindChannel.Size = New System.Drawing.Size(133, 26)
        Me.ButtonFindChannel.TabIndex = 15
        Me.ButtonFindChannel.Text = "Поиск канала"
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannel, "Быстрый поиск канала")
        Me.ButtonFindChannel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label1, 3)
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(539, 32)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Выберите режим и отметьте  какие параметры участвуют в измерении"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonSelectAll
        '
        Me.ButtonSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSelectAll.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSelectAll.Location = New System.Drawing.Point(276, 35)
        Me.ButtonSelectAll.Name = "ButtonSelectAll"
        Me.ButtonSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonSelectAll.Size = New System.Drawing.Size(130, 23)
        Me.ButtonSelectAll.TabIndex = 2
        Me.ButtonSelectAll.Text = "Выделить всё"
        Me.ToolTip1.SetToolTip(Me.ButtonSelectAll, "Выделить все каналы")
        Me.ButtonSelectAll.UseVisualStyleBackColor = False
        '
        'ButtonClear
        '
        Me.ButtonClear.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonClear.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonClear.Location = New System.Drawing.Point(412, 35)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonClear.Size = New System.Drawing.Size(130, 23)
        Me.ButtonClear.TabIndex = 3
        Me.ButtonClear.Text = "Снять выделения"
        Me.ToolTip1.SetToolTip(Me.ButtonClear, "Снять выделения со всех каналов")
        Me.ButtonClear.UseVisualStyleBackColor = False
        '
        'FormTuningRegime
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 761)
        Me.Controls.Add(Me.ListViewSelectedChannels)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(600, 600)
        Me.Name = "FormTuningRegime"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Конфигурация каналов для режимов"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents ComboBoxSelectRegime As System.Windows.Forms.ComboBox
    Friend WithEvents ListViewSelectedChannels As System.Windows.Forms.ListView
    Friend WithEvents ImageListChannel As System.Windows.Forms.ImageList
    Public WithEvents BottonSave As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Public WithEvents ButtonSelectAll As Button
    Public WithEvents ButtonClear As Button
    Friend WithEvents ButtonFindChannel As Button
End Class
