<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormExceptionDialog
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormExceptionDialog))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCopyTextToClipboard = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCopyTextToClipboard
        '
        Me.cmdCopyTextToClipboard.Location = New System.Drawing.Point(12, 15)
        Me.cmdCopyTextToClipboard.Name = "cmdCopyTextToClipboard"
        Me.cmdCopyTextToClipboard.Size = New System.Drawing.Size(90, 23)
        Me.cmdCopyTextToClipboard.TabIndex = 1
        Me.cmdCopyTextToClipboard.Text = "Скопировать"
        Me.ToolTip1.SetToolTip(Me.cmdCopyTextToClipboard, "Copy above messages to your Windows clipboard")
        Me.cmdCopyTextToClipboard.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(310, 15)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 0
        Me.cmdClose.Text = "Закрыть"
        Me.ToolTip1.SetToolTip(Me.cmdClose, "Close this window")
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(402, 220)
        Me.WebBrowser1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdCopyTextToClipboard)
        Me.Panel1.Controls.Add(Me.cmdClose)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 220)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(402, 52)
        Me.Panel1.TabIndex = 2
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Stop 2.ico")
        Me.ImageList1.Images.SetKeyName(1, "Help.ico")
        '
        'frmExceptionDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 272)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExceptionDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Предупреждение"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
   Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
   Friend WithEvents cmdCopyTextToClipboard As System.Windows.Forms.Button
   Friend WithEvents cmdClose As System.Windows.Forms.Button
   Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
