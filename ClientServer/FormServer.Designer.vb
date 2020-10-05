<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormServer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormServer))
        Me.DataSocketSend = New NationalInstruments.Net.DataSocket(Me.components)
        Me.cmdQuit = New System.Windows.Forms.Button()
        Me.cmdDisconnect = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdConnect = New System.Windows.Forms.Button()
        Me.edURL = New System.Windows.Forms.TextBox()
        Me.TimerConnect = New System.Windows.Forms.Timer(Me.components)
        Me.edStatus = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DataSocketServer = New NationalInstruments.Net.DataSocketServer(Me.components)
        CType(Me.DataSocketSend, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSocketServer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataSocketSend
        '
        Me.DataSocketSend.AccessMode = NationalInstruments.Net.AccessMode.Read
        Me.DataSocketSend.AutoConnect = False
        '
        'cmdQuit
        '
        Me.cmdQuit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdQuit.BackColor = System.Drawing.Color.Silver
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(237, 72)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(128, 27)
        Me.cmdQuit.TabIndex = 10
        Me.cmdQuit.Text = "Скрыть"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'cmdDisconnect
        '
        Me.cmdDisconnect.BackColor = System.Drawing.Color.Silver
        Me.cmdDisconnect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisconnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisconnect.Location = New System.Drawing.Point(120, 72)
        Me.cmdDisconnect.Name = "cmdDisconnect"
        Me.cmdDisconnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisconnect.Size = New System.Drawing.Size(111, 27)
        Me.cmdDisconnect.TabIndex = 13
        Me.cmdDisconnect.Text = "Прервать связь"
        Me.cmdDisconnect.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(65, 17)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Источник:"
        '
        'cmdConnect
        '
        Me.cmdConnect.BackColor = System.Drawing.Color.Silver
        Me.cmdConnect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConnect.Location = New System.Drawing.Point(3, 72)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConnect.Size = New System.Drawing.Size(111, 27)
        Me.cmdConnect.TabIndex = 11
        Me.cmdConnect.Text = "Установить связь"
        Me.cmdConnect.UseVisualStyleBackColor = False
        '
        'edURL
        '
        Me.edURL.AcceptsReturn = True
        Me.edURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edURL.BackColor = System.Drawing.SystemColors.Window
        Me.edURL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.edURL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.edURL.Location = New System.Drawing.Point(72, 9)
        Me.edURL.MaxLength = 0
        Me.edURL.Name = "edURL"
        Me.edURL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.edURL.Size = New System.Drawing.Size(293, 20)
        Me.edURL.TabIndex = 16
        Me.edURL.Text = "dstp://localhost/wave"
        '
        'TimerConnect
        '
        Me.TimerConnect.Interval = 2000
        '
        'edStatus
        '
        Me.edStatus.AcceptsReturn = True
        Me.edStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edStatus.BackColor = System.Drawing.SystemColors.Window
        Me.edStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.edStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.edStatus.Location = New System.Drawing.Point(72, 33)
        Me.edStatus.MaxLength = 0
        Me.edStatus.Multiline = True
        Me.edStatus.Name = "edStatus"
        Me.edStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.edStatus.Size = New System.Drawing.Size(293, 33)
        Me.edStatus.TabIndex = 14
        Me.edStatus.TabStop = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(65, 17)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Статус:"
        '
        'FormServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 107)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdDisconnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdConnect)
        Me.Controls.Add(Me.edURL)
        Me.Controls.Add(Me.edStatus)
        Me.Controls.Add(Me.Label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormServer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Сервер"
        CType(Me.DataSocketSend, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSocketServer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents DataSocketSend As NationalInstruments.Net.DataSocket
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents cmdDisconnect As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmdConnect As System.Windows.Forms.Button
    Public WithEvents edURL As System.Windows.Forms.TextBox
    Public WithEvents TimerConnect As System.Windows.Forms.Timer
    Public WithEvents edStatus As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DataSocketServer As NationalInstruments.Net.DataSocketServer
End Class
