<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchReplace
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchReplace))
        Me.lbInfo = New System.Windows.Forms.Label()
        Me.pbCancel = New System.Windows.Forms.Button()
        Me.ProgBar = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'lbInfo
        '
        Me.lbInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbInfo.Location = New System.Drawing.Point(20, 36)
        Me.lbInfo.Name = "lbInfo"
        Me.lbInfo.Size = New System.Drawing.Size(240, 16)
        Me.lbInfo.TabIndex = 5
        Me.lbInfo.Text = "информация"
        Me.lbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbCancel
        '
        Me.pbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.pbCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.pbCancel.Location = New System.Drawing.Point(80, 60)
        Me.pbCancel.Name = "pbCancel"
        Me.pbCancel.Size = New System.Drawing.Size(112, 24)
        Me.pbCancel.TabIndex = 3
        Me.pbCancel.Text = "&Прервать"
        '
        'ProgBar
        '
        Me.ProgBar.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.ProgBar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ProgBar.Location = New System.Drawing.Point(12, 12)
        Me.ProgBar.Name = "ProgBar"
        Me.ProgBar.Size = New System.Drawing.Size(248, 16)
        Me.ProgBar.Step = 1
        Me.ProgBar.TabIndex = 4
        '
        'frmBatchReplace
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.CancelButton = Me.pbCancel
        Me.ClientSize = New System.Drawing.Size(277, 96)
        Me.Controls.Add(Me.lbInfo)
        Me.Controls.Add(Me.pbCancel)
        Me.Controls.Add(Me.ProgBar)
        Me.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBatchReplace"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asc Replacer - поиск и замена"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents lbInfo As System.Windows.Forms.Label
    Private WithEvents pbCancel As System.Windows.Forms.Button
    Private WithEvents ProgBar As System.Windows.Forms.ProgressBar
End Class
