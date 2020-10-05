<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFindAndReplace
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.btnReplaceAll = New System.Windows.Forms.Button
        Me.btnReplace = New System.Windows.Forms.Button
        Me.btnFindNext = New System.Windows.Forms.Button
        Me.Close_Button = New System.Windows.Forms.Button
        Me.txtReplace = New System.Windows.Forms.TextBox
        Me.txtFind = New System.Windows.Forms.TextBox
        Me.chkSearchUp = New System.Windows.Forms.CheckBox
        Me.chkMatchWholeWord = New System.Windows.Forms.CheckBox
        Me.chkMatchCase = New System.Windows.Forms.CheckBox
        Me.lblReplace = New System.Windows.Forms.Label
        Me.lblFind = New System.Windows.Forms.Label
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnReplaceAll, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btnReplace, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnFindNext, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Close_Button, 0, 3)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(349, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(113, 132)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnReplaceAll
        '
        Me.btnReplaceAll.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnReplaceAll.Location = New System.Drawing.Point(12, 69)
        Me.btnReplaceAll.Name = "btnReplaceAll"
        Me.btnReplaceAll.Size = New System.Drawing.Size(88, 27)
        Me.btnReplaceAll.TabIndex = 10
        Me.btnReplaceAll.Text = "Заменить &всё"
        '
        'btnReplace
        '
        Me.btnReplace.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnReplace.Location = New System.Drawing.Point(12, 36)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(88, 27)
        Me.btnReplace.TabIndex = 9
        Me.btnReplace.Text = "&Заменить"
        '
        'btnFindNext
        '
        Me.btnFindNext.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnFindNext.Location = New System.Drawing.Point(12, 3)
        Me.btnFindNext.Name = "btnFindNext"
        Me.btnFindNext.Size = New System.Drawing.Size(88, 27)
        Me.btnFindNext.TabIndex = 8
        Me.btnFindNext.Text = "&Следующий"
        '
        'Close_Button
        '
        Me.Close_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Close_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close_Button.Location = New System.Drawing.Point(12, 102)
        Me.Close_Button.Name = "Close_Button"
        Me.Close_Button.Size = New System.Drawing.Size(88, 27)
        Me.Close_Button.TabIndex = 1
        Me.Close_Button.Text = "Закрыть"
        '
        'txtReplace
        '
        Me.txtReplace.Location = New System.Drawing.Point(92, 48)
        Me.txtReplace.Name = "txtReplace"
        Me.txtReplace.Size = New System.Drawing.Size(192, 20)
        Me.txtReplace.TabIndex = 10
        '
        'txtFind
        '
        Me.txtFind.Location = New System.Drawing.Point(92, 16)
        Me.txtFind.Name = "txtFind"
        Me.txtFind.Size = New System.Drawing.Size(192, 20)
        Me.txtFind.TabIndex = 8
        '
        'chkSearchUp
        '
        Me.chkSearchUp.Location = New System.Drawing.Point(12, 128)
        Me.chkSearchUp.Name = "chkSearchUp"
        Me.chkSearchUp.Size = New System.Drawing.Size(130, 16)
        Me.chkSearchUp.TabIndex = 13
        Me.chkSearchUp.Text = "Поиск &вверx"
        '
        'chkMatchWholeWord
        '
        Me.chkMatchWholeWord.Location = New System.Drawing.Point(12, 104)
        Me.chkMatchWholeWord.Name = "chkMatchWholeWord"
        Me.chkMatchWholeWord.Size = New System.Drawing.Size(130, 16)
        Me.chkMatchWholeWord.TabIndex = 12
        Me.chkMatchWholeWord.Text = "Слово &целиком"
        '
        'chkMatchCase
        '
        Me.chkMatchCase.Location = New System.Drawing.Point(12, 80)
        Me.chkMatchCase.Name = "chkMatchCase"
        Me.chkMatchCase.Size = New System.Drawing.Size(130, 16)
        Me.chkMatchCase.TabIndex = 11
        Me.chkMatchCase.Text = "С &учетом регистра"
        '
        'lblReplace
        '
        Me.lblReplace.Location = New System.Drawing.Point(12, 52)
        Me.lblReplace.Name = "lblReplace"
        Me.lblReplace.Size = New System.Drawing.Size(80, 16)
        Me.lblReplace.TabIndex = 9
        Me.lblReplace.Text = "Заменить &на:"
        '
        'lblFind
        '
        Me.lblFind.Location = New System.Drawing.Point(12, 20)
        Me.lblFind.Name = "lblFind"
        Me.lblFind.Size = New System.Drawing.Size(72, 16)
        Me.lblFind.TabIndex = 7
        Me.lblFind.Text = "&Что искать:"
        '
        'txtTarget
        '
        Me.txtTarget.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTarget.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTarget.Location = New System.Drawing.Point(12, 150)
        Me.txtTarget.Multiline = True
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTarget.Size = New System.Drawing.Size(450, 290)
        Me.txtTarget.TabIndex = 14
        Me.txtTarget.WordWrap = False
        '
        'FindAndReplaceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Close_Button
        Me.ClientSize = New System.Drawing.Size(474, 452)
        Me.Controls.Add(Me.txtTarget)
        Me.Controls.Add(Me.txtReplace)
        Me.Controls.Add(Me.txtFind)
        Me.Controls.Add(Me.chkSearchUp)
        Me.Controls.Add(Me.chkMatchWholeWord)
        Me.Controls.Add(Me.chkMatchCase)
        Me.Controls.Add(Me.lblReplace)
        Me.Controls.Add(Me.lblFind)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindAndReplaceForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Заменить"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Close_Button As System.Windows.Forms.Button
    Friend WithEvents btnReplaceAll As System.Windows.Forms.Button
    Friend WithEvents btnReplace As System.Windows.Forms.Button
    Friend WithEvents btnFindNext As System.Windows.Forms.Button
    Friend WithEvents txtReplace As System.Windows.Forms.TextBox
    Friend WithEvents txtFind As System.Windows.Forms.TextBox
    Friend WithEvents chkSearchUp As System.Windows.Forms.CheckBox
    Friend WithEvents chkMatchWholeWord As System.Windows.Forms.CheckBox
    Friend WithEvents chkMatchCase As System.Windows.Forms.CheckBox
    Friend WithEvents lblReplace As System.Windows.Forms.Label
    Friend WithEvents lblFind As System.Windows.Forms.Label
    Public WithEvents txtTarget As System.Windows.Forms.TextBox

End Class
