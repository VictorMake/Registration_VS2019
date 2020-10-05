<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormClient
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormClient))
        Me.TimerUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.ListViewValueReceived = New System.Windows.Forms.ListView()
        Me.ButtonConnect = New System.Windows.Forms.Button()
        Me.ButtonQuit = New System.Windows.Forms.Button()
        Me.DataSocketReceive = New NationalInstruments.Net.DataSocket(Me.components)
        Me.TextBoxStatus = New System.Windows.Forms.TextBox()
        Me.TimerReceive = New System.Windows.Forms.Timer(Me.components)
        Me.ButtonDisconnect = New System.Windows.Forms.Button()
        Me.TextBoxURL = New System.Windows.Forms.TextBox()
        Me.LabelStatus = New System.Windows.Forms.Label()
        Me.LabelSource = New System.Windows.Forms.Label()
        CType(Me.DataSocketReceive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TimerUpdate
        '
        '
        'ListView1
        '
        Me.ListViewValueReceived.BackColor = System.Drawing.Color.Black
        Me.ListViewValueReceived.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ListViewValueReceived.ForeColor = System.Drawing.Color.White
        Me.ListViewValueReceived.GridLines = True
        Me.ListViewValueReceived.Location = New System.Drawing.Point(3, 101)
        Me.ListViewValueReceived.Name = "ListView1"
        Me.ListViewValueReceived.Size = New System.Drawing.Size(336, 281)
        Me.ListViewValueReceived.TabIndex = 34
        Me.ListViewValueReceived.UseCompatibleStateImageBehavior = False
        Me.ListViewValueReceived.View = System.Windows.Forms.View.Details
        '
        'ButtonConnect
        '
        Me.ButtonConnect.BackColor = System.Drawing.Color.Silver
        Me.ButtonConnect.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonConnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonConnect.Location = New System.Drawing.Point(3, 69)
        Me.ButtonConnect.Name = "ButtonConnect"
        Me.ButtonConnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonConnect.Size = New System.Drawing.Size(101, 25)
        Me.ButtonConnect.TabIndex = 29
        Me.ButtonConnect.Text = "Возобновить"
        Me.ButtonConnect.UseVisualStyleBackColor = False
        '
        'cmdQuit
        '
        Me.ButtonQuit.BackColor = System.Drawing.Color.Silver
        Me.ButtonQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonQuit.Location = New System.Drawing.Point(235, 69)
        Me.ButtonQuit.Name = "cmdQuit"
        Me.ButtonQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonQuit.Size = New System.Drawing.Size(101, 25)
        Me.ButtonQuit.TabIndex = 27
        Me.ButtonQuit.Text = "Свернуть"
        Me.ButtonQuit.UseVisualStyleBackColor = False
        '
        'DataSocketReceive
        '
        Me.DataSocketReceive.AutoConnect = False
        '
        'edStatus
        '
        Me.TextBoxStatus.AcceptsReturn = True
        Me.TextBoxStatus.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxStatus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxStatus.Location = New System.Drawing.Point(79, 29)
        Me.TextBoxStatus.MaxLength = 0
        Me.TextBoxStatus.Multiline = True
        Me.TextBoxStatus.Name = "edStatus"
        Me.TextBoxStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxStatus.Size = New System.Drawing.Size(256, 33)
        Me.TextBoxStatus.TabIndex = 32
        '
        'Часы1
        '
        Me.TimerReceive.Interval = 5000
        '
        'cmdDisconnect
        '
        Me.ButtonDisconnect.BackColor = System.Drawing.Color.Silver
        Me.ButtonDisconnect.Cursor = System.Windows.Forms.Cursors.Default
        Me.ButtonDisconnect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonDisconnect.Location = New System.Drawing.Point(119, 69)
        Me.ButtonDisconnect.Name = "cmdDisconnect"
        Me.ButtonDisconnect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ButtonDisconnect.Size = New System.Drawing.Size(101, 25)
        Me.ButtonDisconnect.TabIndex = 30
        Me.ButtonDisconnect.Text = "Прервать"
        Me.ButtonDisconnect.UseVisualStyleBackColor = False
        '
        'edURL
        '
        Me.TextBoxURL.AcceptsReturn = True
        Me.TextBoxURL.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxURL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxURL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBoxURL.Location = New System.Drawing.Point(79, 5)
        Me.TextBoxURL.MaxLength = 0
        Me.TextBoxURL.Name = "edURL"
        Me.TextBoxURL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxURL.Size = New System.Drawing.Size(256, 20)
        Me.TextBoxURL.TabIndex = 33
        Me.TextBoxURL.Text = "dstp://localhost/wave"
        '
        'Label2
        '
        Me.LabelStatus.BackColor = System.Drawing.SystemColors.Control
        Me.LabelStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelStatus.Location = New System.Drawing.Point(7, 29)
        Me.LabelStatus.Name = "Label2"
        Me.LabelStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelStatus.Size = New System.Drawing.Size(72, 17)
        Me.LabelStatus.TabIndex = 31
        Me.LabelStatus.Text = "Статус:"
        Me.LabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.LabelSource.BackColor = System.Drawing.SystemColors.Control
        Me.LabelSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSource.Location = New System.Drawing.Point(7, 5)
        Me.LabelSource.Name = "Label1"
        Me.LabelSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelSource.Size = New System.Drawing.Size(72, 17)
        Me.LabelSource.TabIndex = 28
        Me.LabelSource.Text = "Источник:"
        Me.LabelSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FormClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(342, 387)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListViewValueReceived)
        Me.Controls.Add(Me.ButtonConnect)
        Me.Controls.Add(Me.ButtonQuit)
        Me.Controls.Add(Me.TextBoxStatus)
        Me.Controls.Add(Me.ButtonDisconnect)
        Me.Controls.Add(Me.TextBoxURL)
        Me.Controls.Add(Me.LabelStatus)
        Me.Controls.Add(Me.LabelSource)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormClient"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Прием от сервера"
        CType(Me.DataSocketReceive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TimerUpdate As System.Windows.Forms.Timer
    Friend WithEvents ListViewValueReceived As System.Windows.Forms.ListView
    Public WithEvents ButtonConnect As System.Windows.Forms.Button
    Public WithEvents ButtonQuit As System.Windows.Forms.Button
    Public WithEvents DataSocketReceive As NationalInstruments.Net.DataSocket
    Public WithEvents TextBoxStatus As System.Windows.Forms.TextBox
    Public WithEvents TimerReceive As System.Windows.Forms.Timer
    Public WithEvents ButtonDisconnect As System.Windows.Forms.Button
    Public WithEvents TextBoxURL As System.Windows.Forms.TextBox
    Public WithEvents LabelStatus As System.Windows.Forms.Label
    Public WithEvents LabelSource As System.Windows.Forms.Label
End Class
