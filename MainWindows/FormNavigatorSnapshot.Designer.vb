<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormNavigatorSnapshot
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormNavigatorSnapshot))
        Me.ButtonForward = New System.Windows.Forms.Button()
        Me.ButtonBack = New System.Windows.Forms.Button()
        Me.ListBoxSnapshots = New System.Windows.Forms.ListBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdВперед
        '
        Me.ButtonForward.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonForward.BackColor = System.Drawing.Color.DarkGray
        Me.ButtonForward.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonForward.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonForward.Image = CType(resources.GetObject("cmdВперед.Image"), System.Drawing.Image)
        Me.ButtonForward.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonForward.Location = New System.Drawing.Point(288, 110)
        Me.ButtonForward.Name = "cmdВперед"
        Me.ButtonForward.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonForward.Size = New System.Drawing.Size(173, 28)
        Me.ButtonForward.TabIndex = 3
        Me.ButtonForward.Tag = "103"
        Me.ButtonForward.Text = "&Вперед"
        Me.ButtonForward.UseVisualStyleBackColor = False
        '
        'cmdНазад
        '
        Me.ButtonBack.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonBack.BackColor = System.Drawing.Color.DarkGray
        Me.ButtonBack.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonBack.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonBack.Image = CType(resources.GetObject("cmdНазад.Image"), System.Drawing.Image)
        Me.ButtonBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonBack.Location = New System.Drawing.Point(3, 110)
        Me.ButtonBack.Name = "cmdНазад"
        Me.ButtonBack.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonBack.Size = New System.Drawing.Size(173, 28)
        Me.ButtonBack.TabIndex = 4
        Me.ButtonBack.Tag = "102"
        Me.ButtonBack.Text = "&Назад"
        Me.ButtonBack.UseVisualStyleBackColor = False
        '
        'lstItems
        '
        Me.ListBoxSnapshots.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanel1.SetColumnSpan(Me.ListBoxSnapshots, 2)
        Me.ListBoxSnapshots.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListBoxSnapshots.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxSnapshots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ListBoxSnapshots.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListBoxSnapshots.Location = New System.Drawing.Point(3, 3)
        Me.ListBoxSnapshots.Name = "lstItems"
        Me.ListBoxSnapshots.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListBoxSnapshots.Size = New System.Drawing.Size(458, 99)
        Me.ListBoxSnapshots.TabIndex = 5
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonForward, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ListBoxSnapshots, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonBack, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(464, 141)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'FormNavigatorSnapshot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 141)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1024, 768)
        Me.MinimumSize = New System.Drawing.Size(280, 140)
        Me.Name = "FormNavigatorSnapshot"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "FormIsNavigatorSnapshot"
        Me.Text = "Двойной связанный список"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents ButtonForward As System.Windows.Forms.Button
    Public WithEvents ButtonBack As System.Windows.Forms.Button
    Friend WithEvents ListBoxSnapshots As System.Windows.Forms.ListBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
End Class
