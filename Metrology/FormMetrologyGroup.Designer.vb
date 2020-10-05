<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMetrologyGroup
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMetrologyGroup))
        Me.ButtonPrint = New System.Windows.Forms.Button()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.ButtonDelete = New System.Windows.Forms.Button()
        Me.ButtonClearAll = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PanelReceiver = New System.Windows.Forms.Panel()
        Me.ListViewReceiver = New System.Windows.Forms.ListView()
        Me.ImageChannels = New System.Windows.Forms.ImageList(Me.components)
        Me.ListViewSource = New System.Windows.Forms.ListView()
        Me.LabelCaption = New System.Windows.Forms.Label()
        Me.PanelSource = New System.Windows.Forms.Panel()
        Me.PanelCaption = New System.Windows.Forms.Panel()
        Me.PanelButtons.SuspendLayout()
        Me.PanelReceiver.SuspendLayout()
        Me.PanelSource.SuspendLayout()
        Me.PanelCaption.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonPrint
        '
        Me.ButtonPrint.BackgroundImage = CType(resources.GetObject("ButtonPrint.BackgroundImage"), System.Drawing.Image)
        Me.ButtonPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonPrint.ForeColor = System.Drawing.Color.Black
        Me.ButtonPrint.Location = New System.Drawing.Point(4, 453)
        Me.ButtonPrint.Name = "ButtonPrint"
        Me.ButtonPrint.Size = New System.Drawing.Size(90, 90)
        Me.ButtonPrint.TabIndex = 5
        Me.ButtonPrint.Text = "Печать"
        Me.ButtonPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonPrint, "Печать тарировок всех каналов")
        Me.ButtonPrint.UseVisualStyleBackColor = True
        Me.ButtonPrint.Visible = False
        '
        'ButtonApply
        '
        Me.ButtonApply.BackgroundImage = CType(resources.GetObject("ButtonApply.BackgroundImage"), System.Drawing.Image)
        Me.ButtonApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonApply.ForeColor = System.Drawing.Color.Black
        Me.ButtonApply.Location = New System.Drawing.Point(4, 273)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.Size = New System.Drawing.Size(90, 90)
        Me.ButtonApply.TabIndex = 4
        Me.ButtonApply.Text = "Готово"
        Me.ButtonApply.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonApply, "Отбор каналов завершен")
        Me.ButtonApply.UseVisualStyleBackColor = True
        Me.ButtonApply.Visible = False
        '
        'ButtonSave
        '
        Me.ButtonSave.BackgroundImage = CType(resources.GetObject("ButtonSave.BackgroundImage"), System.Drawing.Image)
        Me.ButtonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonSave.ForeColor = System.Drawing.Color.Black
        Me.ButtonSave.Location = New System.Drawing.Point(4, 363)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(90, 90)
        Me.ButtonSave.TabIndex = 3
        Me.ButtonSave.Text = "Записать"
        Me.ButtonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonSave, "Записать тарировки всех каналов")
        Me.ButtonSave.UseVisualStyleBackColor = True
        Me.ButtonSave.Visible = False
        '
        'PanelButtons
        '
        Me.PanelButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelButtons.Controls.Add(Me.ButtonPrint)
        Me.PanelButtons.Controls.Add(Me.ButtonApply)
        Me.PanelButtons.Controls.Add(Me.ButtonSave)
        Me.PanelButtons.Controls.Add(Me.ButtonAdd)
        Me.PanelButtons.Controls.Add(Me.ButtonDelete)
        Me.PanelButtons.Controls.Add(Me.ButtonClearAll)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelButtons.Location = New System.Drawing.Point(232, 24)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(101, 549)
        Me.PanelButtons.TabIndex = 22
        '
        'ButtonAdd
        '
        Me.ButtonAdd.BackgroundImage = CType(resources.GetObject("ButtonAdd.BackgroundImage"), System.Drawing.Image)
        Me.ButtonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonAdd.ForeColor = System.Drawing.Color.Black
        Me.ButtonAdd.Location = New System.Drawing.Point(3, 3)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(90, 90)
        Me.ButtonAdd.TabIndex = 2
        Me.ButtonAdd.Text = "Добавить"
        Me.ButtonAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonAdd, "Добавить канал в лист")
        Me.ButtonAdd.UseVisualStyleBackColor = True
        '
        'ButtonDelete
        '
        Me.ButtonDelete.BackgroundImage = CType(resources.GetObject("ButtonDelete.BackgroundImage"), System.Drawing.Image)
        Me.ButtonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonDelete.ForeColor = System.Drawing.Color.Black
        Me.ButtonDelete.Location = New System.Drawing.Point(3, 93)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.Size = New System.Drawing.Size(90, 90)
        Me.ButtonDelete.TabIndex = 1
        Me.ButtonDelete.Text = "Удалить"
        Me.ButtonDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonDelete, "Удалить канал из листа")
        Me.ButtonDelete.UseVisualStyleBackColor = True
        '
        'ButtonClearAll
        '
        Me.ButtonClearAll.BackgroundImage = CType(resources.GetObject("ButtonClearAll.BackgroundImage"), System.Drawing.Image)
        Me.ButtonClearAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonClearAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ButtonClearAll.ForeColor = System.Drawing.Color.Black
        Me.ButtonClearAll.Location = New System.Drawing.Point(4, 183)
        Me.ButtonClearAll.Name = "ButtonClearAll"
        Me.ButtonClearAll.Size = New System.Drawing.Size(90, 90)
        Me.ButtonClearAll.TabIndex = 0
        Me.ButtonClearAll.Text = "Очистить"
        Me.ButtonClearAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ButtonClearAll, "Очистить весь список")
        Me.ButtonClearAll.UseVisualStyleBackColor = True
        '
        'PanelReceiver
        '
        Me.PanelReceiver.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelReceiver.Controls.Add(Me.ListViewReceiver)
        Me.PanelReceiver.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelReceiver.Location = New System.Drawing.Point(512, 24)
        Me.PanelReceiver.Name = "PanelReceiver"
        Me.PanelReceiver.Size = New System.Drawing.Size(232, 549)
        Me.PanelReceiver.TabIndex = 20
        '
        'ListViewReceiver
        '
        Me.ListViewReceiver.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewReceiver.FullRowSelect = True
        Me.ListViewReceiver.GridLines = True
        Me.ListViewReceiver.HideSelection = False
        Me.ListViewReceiver.Location = New System.Drawing.Point(0, 0)
        Me.ListViewReceiver.Name = "ListViewReceiver"
        Me.ListViewReceiver.Size = New System.Drawing.Size(228, 545)
        Me.ListViewReceiver.SmallImageList = Me.ImageChannels
        Me.ListViewReceiver.TabIndex = 8
        Me.ListViewReceiver.UseCompatibleStateImageBehavior = False
        Me.ListViewReceiver.View = System.Windows.Forms.View.Details
        '
        'ImageChannels
        '
        Me.ImageChannels.ImageStream = CType(resources.GetObject("ImageChannels.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageChannels.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageChannels.Images.SetKeyName(0, "")
        Me.ImageChannels.Images.SetKeyName(1, "")
        Me.ImageChannels.Images.SetKeyName(2, "")
        Me.ImageChannels.Images.SetKeyName(3, "")
        Me.ImageChannels.Images.SetKeyName(4, "")
        Me.ImageChannels.Images.SetKeyName(5, "")
        Me.ImageChannels.Images.SetKeyName(6, "")
        Me.ImageChannels.Images.SetKeyName(7, "")
        Me.ImageChannels.Images.SetKeyName(8, "")
        Me.ImageChannels.Images.SetKeyName(9, "")
        Me.ImageChannels.Images.SetKeyName(10, "")
        Me.ImageChannels.Images.SetKeyName(11, "")
        '
        'ListViewSource
        '
        Me.ListViewSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewSource.FullRowSelect = True
        Me.ListViewSource.GridLines = True
        Me.ListViewSource.HideSelection = False
        Me.ListViewSource.Location = New System.Drawing.Point(0, 0)
        Me.ListViewSource.Name = "ListViewSource"
        Me.ListViewSource.Size = New System.Drawing.Size(228, 545)
        Me.ListViewSource.SmallImageList = Me.ImageChannels
        Me.ListViewSource.TabIndex = 11
        Me.ListViewSource.UseCompatibleStateImageBehavior = False
        Me.ListViewSource.View = System.Windows.Forms.View.Details
        '
        'LabelCaption
        '
        Me.LabelCaption.BackColor = System.Drawing.SystemColors.Control
        Me.LabelCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCaption.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaption.Location = New System.Drawing.Point(0, 0)
        Me.LabelCaption.Name = "LabelCaption"
        Me.LabelCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCaption.Size = New System.Drawing.Size(740, 20)
        Me.LabelCaption.TabIndex = 2
        Me.LabelCaption.Text = "Отметьте  параметры для групповой тарировки и нажмите кнопку <Готово>"
        Me.LabelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelSource
        '
        Me.PanelSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelSource.Controls.Add(Me.ListViewSource)
        Me.PanelSource.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelSource.Location = New System.Drawing.Point(0, 24)
        Me.PanelSource.Name = "PanelSource"
        Me.PanelSource.Size = New System.Drawing.Size(232, 549)
        Me.PanelSource.TabIndex = 19
        '
        'PanelCaption
        '
        Me.PanelCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelCaption.Controls.Add(Me.LabelCaption)
        Me.PanelCaption.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelCaption.Location = New System.Drawing.Point(0, 0)
        Me.PanelCaption.Name = "PanelCaption"
        Me.PanelCaption.Size = New System.Drawing.Size(744, 24)
        Me.PanelCaption.TabIndex = 21
        '
        'FormMetrologyGroup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 573)
        Me.Controls.Add(Me.PanelButtons)
        Me.Controls.Add(Me.PanelReceiver)
        Me.Controls.Add(Me.PanelSource)
        Me.Controls.Add(Me.PanelCaption)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormMetrologyGroup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Тарировка группы параметров"
        Me.TopMost = True
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelReceiver.ResumeLayout(False)
        Me.PanelSource.ResumeLayout(False)
        Me.PanelCaption.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonPrint As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ButtonApply As System.Windows.Forms.Button
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents PanelButtons As System.Windows.Forms.Panel
    Friend WithEvents ButtonAdd As System.Windows.Forms.Button
    Friend WithEvents ButtonDelete As System.Windows.Forms.Button
    Friend WithEvents ButtonClearAll As System.Windows.Forms.Button
    Friend WithEvents PanelReceiver As System.Windows.Forms.Panel
    Friend WithEvents ListViewReceiver As System.Windows.Forms.ListView
    Friend WithEvents ImageChannels As System.Windows.Forms.ImageList
    Friend WithEvents ListViewSource As System.Windows.Forms.ListView
    Public WithEvents LabelCaption As System.Windows.Forms.Label
    Friend WithEvents PanelSource As System.Windows.Forms.Panel
    Friend WithEvents PanelCaption As System.Windows.Forms.Panel
End Class
