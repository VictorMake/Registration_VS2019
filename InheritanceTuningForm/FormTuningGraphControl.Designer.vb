<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTuningGraphControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTuningGraphControl))
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.ListViewName = New System.Windows.Forms.ListView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSelected = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonSave
        '
        Me.ButtonSave.BackColor = System.Drawing.Color.Silver
        Me.ButtonSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonSave.Dock = System.Windows.Forms.DockStyle.Left
        Me.ButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ButtonSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonSave.Location = New System.Drawing.Point(575, 3)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonSave.Size = New System.Drawing.Size(129, 26)
        Me.ButtonSave.TabIndex = 0
        Me.ButtonSave.Text = "Записать изменения"
        Me.ButtonSave.UseVisualStyleBackColor = False
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
        'ListViewName
        '
        Me.ListViewName.CheckBoxes = True
        Me.ListViewName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewName.FullRowSelect = True
        Me.ListViewName.GridLines = True
        Me.ListViewName.HideSelection = False
        Me.ListViewName.Location = New System.Drawing.Point(0, 32)
        Me.ListViewName.MultiSelect = False
        Me.ListViewName.Name = "ListViewName"
        Me.ListViewName.Size = New System.Drawing.Size(712, 481)
        Me.ListViewName.SmallImageList = Me.ImageListChannel
        Me.ListViewName.TabIndex = 15
        Me.ListViewName.UseCompatibleStateImageBehavior = False
        Me.ListViewName.View = System.Windows.Forms.View.Details
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSelected, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonSave, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(712, 32)
        Me.TableLayoutPanel1.TabIndex = 16
        '
        'LabelSelected
        '
        Me.LabelSelected.AutoSize = True
        Me.LabelSelected.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSelected.Location = New System.Drawing.Point(539, 0)
        Me.LabelSelected.Name = "LabelSelected"
        Me.LabelSelected.Size = New System.Drawing.Size(30, 32)
        Me.LabelSelected.TabIndex = 11
        Me.LabelSelected.Text = "0"
        Me.LabelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(462, 32)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Отметьте какие параметры выводить на форму графического контроля"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(471, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 32)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Отмечено:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FormTuningGraphControl
        '
        Me.AcceptButton = Me.ButtonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(712, 513)
        Me.Controls.Add(Me.ListViewName)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormTuningGraphControl"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Настройка выборочного графического контроля параметров"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents ImageListChannel As System.Windows.Forms.ImageList
    Friend WithEvents ListViewName As System.Windows.Forms.ListView
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelSelected As System.Windows.Forms.Label
End Class
