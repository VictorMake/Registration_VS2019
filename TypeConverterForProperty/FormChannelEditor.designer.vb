<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormChannelEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormChannelEditor))
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ImageListChannel = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboBoxParameters = New System.Windows.Forms.ComboBox()
        Me.ButtonFindChannel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(186, 39)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 4
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
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
        Me.ImageListChannel.Images.SetKeyName(12, "")
        '
        'ComboBoxParameters
        '
        Me.ComboBoxParameters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxParameters.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxParameters.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBoxParameters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxParameters.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ComboBoxParameters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBoxParameters.Location = New System.Drawing.Point(12, 12)
        Me.ComboBoxParameters.MaxDropDownItems = 32
        Me.ComboBoxParameters.Name = "ComboBoxParameters"
        Me.ComboBoxParameters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxParameters.Size = New System.Drawing.Size(219, 21)
        Me.ComboBoxParameters.TabIndex = 7
        '
        'ButtonFindChannel
        '
        Me.ButtonFindChannel.Image = CType(resources.GetObject("ButtonFindChannel.Image"), System.Drawing.Image)
        Me.ButtonFindChannel.Location = New System.Drawing.Point(237, 11)
        Me.ButtonFindChannel.Name = "ButtonFindChannel"
        Me.ButtonFindChannel.Size = New System.Drawing.Size(24, 24)
        Me.ButtonFindChannel.TabIndex = 74
        Me.ButtonFindChannel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonFindChannel.UseVisualStyleBackColor = True
        '
        'FormChannelEditor
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(273, 72)
        Me.Controls.Add(Me.ButtonFindChannel)
        Me.Controls.Add(Me.ComboBoxParameters)
        Me.Controls.Add(Me.ButtonOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormChannelEditor"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Выбрать канал"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ImageListChannel As System.Windows.Forms.ImageList
    Public WithEvents ComboBoxParameters As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonFindChannel As Button
End Class
