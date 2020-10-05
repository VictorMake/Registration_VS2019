<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTuningVisibleTail
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningVisibleTail))
        Me.ListViewName = New System.Windows.Forms.ListView()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboBoxSelectRegime = New System.Windows.Forms.ComboBox()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ButtonRestore = New System.Windows.Forms.Button()
        Me.ButtonSaveAsPattern = New System.Windows.Forms.Button()
        Me.LabelTitle = New System.Windows.Forms.Label()
        Me.ButtonFindChannel = New System.Windows.Forms.Button()
        Me.LabelSelected = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListViewName
        '
        Me.ListViewName.CheckBoxes = True
        Me.ListViewName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewName.FullRowSelect = True
        Me.ListViewName.GridLines = True
        Me.ListViewName.HideSelection = False
        Me.ListViewName.Location = New System.Drawing.Point(0, 68)
        Me.ListViewName.MultiSelect = False
        Me.ListViewName.Name = "ListViewName"
        Me.ListViewName.Size = New System.Drawing.Size(784, 493)
        Me.ListViewName.SmallImageList = Me.ImageListChannel
        Me.ListViewName.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.ListViewName, "Красный - канал отключён:; Чёрный - канал включён")
        Me.ListViewName.UseCompatibleStateImageBehavior = False
        Me.ListViewName.View = System.Windows.Forms.View.Details
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
        'ComboBoxSelectRegime
        '
        Me.ComboBoxSelectRegime.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxSelectRegime.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxSelectRegime.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBoxSelectRegime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxSelectRegime.Location = New System.Drawing.Point(3, 35)
        Me.ComboBoxSelectRegime.Name = "ComboBoxSelectRegime"
        Me.ComboBoxSelectRegime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxSelectRegime.Size = New System.Drawing.Size(307, 21)
        Me.ComboBoxSelectRegime.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ComboBoxSelectRegime, "Выбор списка каналов связанный с режимом")
        '
        'ButtonSave
        '
        Me.ButtonSave.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSave.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSave.Location = New System.Drawing.Point(628, 35)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonSave.Size = New System.Drawing.Size(153, 23)
        Me.ButtonSave.TabIndex = 0
        Me.ButtonSave.Text = "Применить изменения"
        Me.ToolTip1.SetToolTip(Me.ButtonSave, "Записать и применить произведённую настройку видимости шлейфов для каналов")
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ButtonRestore
        '
        Me.ButtonRestore.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonRestore.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonRestore.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonRestore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonRestore.Location = New System.Drawing.Point(472, 35)
        Me.ButtonRestore.Name = "ButtonRestore"
        Me.ButtonRestore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonRestore.Size = New System.Drawing.Size(150, 23)
        Me.ButtonRestore.TabIndex = 1
        Me.ButtonRestore.Text = "Восстановить"
        Me.ToolTip1.SetToolTip(Me.ButtonRestore, "Восстановить видимость шлейфов каналов из ранее записанного шаблона")
        Me.ButtonRestore.UseVisualStyleBackColor = True
        '
        'ButtonSaveAsPattern
        '
        Me.ButtonSaveAsPattern.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonSaveAsPattern.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSaveAsPattern.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonSaveAsPattern.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSaveAsPattern.Location = New System.Drawing.Point(316, 35)
        Me.ButtonSaveAsPattern.Name = "ButtonSaveAsPattern"
        Me.ButtonSaveAsPattern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonSaveAsPattern.Size = New System.Drawing.Size(150, 23)
        Me.ButtonSaveAsPattern.TabIndex = 2
        Me.ButtonSaveAsPattern.Text = "Записать как шаблон"
        Me.ToolTip1.SetToolTip(Me.ButtonSaveAsPattern, "Записать видимость шлейфов отмеченных каналов для последующего восстановления")
        Me.ButtonSaveAsPattern.UseVisualStyleBackColor = True
        '
        'LabelTitle
        '
        Me.LabelTitle.AutoSize = True
        Me.LabelTitle.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanel3.SetColumnSpan(Me.LabelTitle, 2)
        Me.LabelTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTitle.ForeColor = System.Drawing.Color.Blue
        Me.LabelTitle.Location = New System.Drawing.Point(3, 0)
        Me.LabelTitle.Name = "LabelTitle"
        Me.LabelTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTitle.Size = New System.Drawing.Size(463, 32)
        Me.LabelTitle.TabIndex = 8
        Me.LabelTitle.Text = "Отметьте какие шлейфы выводить на график или выбрать видимость из существующего р" &
    "ежима"
        Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonFindChannel
        '
        Me.ButtonFindChannel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ButtonFindChannel.Image = CType(resources.GetObject("ButtonFindChannel.Image"), System.Drawing.Image)
        Me.ButtonFindChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonFindChannel.Location = New System.Drawing.Point(628, 3)
        Me.ButtonFindChannel.Name = "ButtonFindChannel"
        Me.ButtonFindChannel.Size = New System.Drawing.Size(153, 26)
        Me.ButtonFindChannel.TabIndex = 14
        Me.ButtonFindChannel.Text = "Поиск канала"
        Me.ToolTip1.SetToolTip(Me.ButtonFindChannel, "Быстрый поиск канала")
        Me.ButtonFindChannel.UseVisualStyleBackColor = True
        '
        'LabelSelected
        '
        Me.LabelSelected.AutoSize = True
        Me.LabelSelected.Location = New System.Drawing.Point(100, 6)
        Me.LabelSelected.Name = "LabelSelected"
        Me.LabelSelected.Size = New System.Drawing.Size(13, 13)
        Me.LabelSelected.TabIndex = 12
        Me.LabelSelected.Text = "0"
        Me.LabelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Отмечено:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 4
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Panel1, 2, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonSave, 3, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.ComboBoxSelectRegime, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonRestore, 2, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonFindChannel, 3, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonSaveAsPattern, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.LabelTitle, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(784, 68)
        Me.TableLayoutPanel3.TabIndex = 11
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.LabelSelected)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(472, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(150, 26)
        Me.Panel1.TabIndex = 12
        '
        'FormTuningVisibleTail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.ListViewName)
        Me.Controls.Add(Me.TableLayoutPanel3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "FormTuningVisibleTail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Видимость шлейфов для режима ""Регистратор"""
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListViewName As System.Windows.Forms.ListView
    Friend WithEvents ImageListChannel As System.Windows.Forms.ImageList
    Public WithEvents LabelTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelSelected As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Public WithEvents ComboBoxSelectRegime As ComboBox
    Friend WithEvents ButtonSave As Button
    Friend WithEvents ButtonRestore As Button
    Friend WithEvents ButtonSaveAsPattern As Button
    Friend WithEvents ButtonFindChannel As Button
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
End Class
