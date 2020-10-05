<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormWebBrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormWebBrowser))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StopB = New System.Windows.Forms.ToolBarButton()
        Me.RefreshB = New System.Windows.Forms.ToolBarButton()
        Me.Home = New System.Windows.Forms.ToolBarButton()
        Me.Search = New System.Windows.Forms.ToolBarButton()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Forward = New System.Windows.Forms.ToolBarButton()
        Me.Back = New System.Windows.Forms.ToolBarButton()
        Me.timTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.tbToolBar = New System.Windows.Forms.ToolBar()
        Me.brwWebBrowser = New AxSHDocVw.AxWebBrowser()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboAddress = New System.Windows.Forms.ComboBox()
        CType(Me.brwWebBrowser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StopB
        '
        Me.StopB.ImageIndex = 2
        Me.StopB.Name = "StopB"
        Me.StopB.Tag = "Stop"
        Me.StopB.ToolTipText = "Стоп"
        '
        'RefreshB
        '
        Me.RefreshB.ImageIndex = 3
        Me.RefreshB.Name = "RefreshB"
        Me.RefreshB.Tag = "Refresh"
        Me.RefreshB.ToolTipText = "Обновить"
        '
        'Home
        '
        Me.Home.ImageIndex = 4
        Me.Home.Name = "Home"
        Me.Home.Tag = "Home"
        Me.Home.ToolTipText = "Домашняя страница"
        '
        'Search
        '
        Me.Search.ImageIndex = 5
        Me.Search.Name = "Search"
        Me.Search.Tag = "Search"
        Me.Search.ToolTipText = "Поиск"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "1leftarrow.png")
        Me.ImageList1.Images.SetKeyName(1, "1rightarrow.png")
        Me.ImageList1.Images.SetKeyName(2, "agt_stop.png")
        Me.ImageList1.Images.SetKeyName(3, "reload.png")
        Me.ImageList1.Images.SetKeyName(4, "home.png")
        Me.ImageList1.Images.SetKeyName(5, "browser.png")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        '
        'Forward
        '
        Me.Forward.ImageIndex = 1
        Me.Forward.Name = "Forward"
        Me.Forward.Tag = "Forward"
        Me.Forward.ToolTipText = "Вперед"
        '
        'Back
        '
        Me.Back.ImageIndex = 0
        Me.Back.Name = "Back"
        Me.Back.Tag = "Back"
        Me.Back.ToolTipText = "Назад"
        '
        'timTimer
        '
        Me.timTimer.Interval = 5
        '
        'lblAddress
        '
        Me.lblAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress.Location = New System.Drawing.Point(8, 8)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress.Size = New System.Drawing.Size(48, 17)
        Me.lblAddress.TabIndex = 1
        Me.lblAddress.Tag = "&Address:"
        Me.lblAddress.Text = "&Адрес:"
        Me.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbToolBar
        '
        Me.tbToolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.Back, Me.Forward, Me.StopB, Me.RefreshB, Me.Home, Me.Search})
        Me.tbToolBar.ButtonSize = New System.Drawing.Size(30, 30)
        Me.tbToolBar.DropDownArrows = True
        Me.tbToolBar.ImageList = Me.ImageList1
        Me.tbToolBar.Location = New System.Drawing.Point(0, 0)
        Me.tbToolBar.Name = "tbToolBar"
        Me.tbToolBar.ShowToolTips = True
        Me.tbToolBar.Size = New System.Drawing.Size(451, 38)
        Me.tbToolBar.TabIndex = 8
        '
        'brwWebBrowser
        '
        Me.brwWebBrowser.Enabled = True
        Me.brwWebBrowser.Location = New System.Drawing.Point(10, 83)
        Me.brwWebBrowser.OcxState = CType(resources.GetObject("brwWebBrowser.OcxState"), System.Windows.Forms.AxHost.State)
        Me.brwWebBrowser.Size = New System.Drawing.Size(360, 249)
        Me.brwWebBrowser.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblAddress)
        Me.Panel1.Controls.Add(Me.cboAddress)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(451, 37)
        Me.Panel1.TabIndex = 9
        '
        'cboAddress
        '
        Me.cboAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAddress.BackColor = System.Drawing.SystemColors.Window
        Me.cboAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAddress.Location = New System.Drawing.Point(56, 8)
        Me.cboAddress.Name = "cboAddress"
        Me.cboAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAddress.Size = New System.Drawing.Size(387, 21)
        Me.cboAddress.TabIndex = 2
        '
        'frmBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 440)
        Me.Controls.Add(Me.brwWebBrowser)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.tbToolBar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBrowser"
        Me.ShowInTaskbar = False
        Me.Text = "Справка по программе"
        CType(Me.brwWebBrowser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents StopB As System.Windows.Forms.ToolBarButton
    Friend WithEvents RefreshB As System.Windows.Forms.ToolBarButton
    Friend WithEvents Home As System.Windows.Forms.ToolBarButton
    Friend WithEvents Search As System.Windows.Forms.ToolBarButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Forward As System.Windows.Forms.ToolBarButton
    Friend WithEvents Back As System.Windows.Forms.ToolBarButton
    Public WithEvents timTimer As System.Windows.Forms.Timer
    Public WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents tbToolBar As System.Windows.Forms.ToolBar
    Public WithEvents brwWebBrowser As AxSHDocVw.AxWebBrowser
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents cboAddress As System.Windows.Forms.ComboBox
End Class
