<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTextEditor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FoldersToolStripButton As System.Windows.Forms.ToolStrip
    Friend WithEvents BackToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ForwardToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IndexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTextEditor))
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.FoldersToolStripButton = New System.Windows.Forms.ToolStrip
        Me.newToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.openToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.saveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.BackToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ForwardToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.MenuStrip = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRecentFile0 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecentFile1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecentFile2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecentFile3 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecentFile4 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRecentFile5 = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.formatToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.boldToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.italicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.underlineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.FontFaceMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiSansSerif = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiSerif = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiMonoSpace = New System.Windows.Forms.ToolStripMenuItem
        Me.FontSizeMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiSmall = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiMedium = New System.Windows.Forms.ToolStripMenuItem
        Me.mmiLarge = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSearchFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSearchFindNext = New System.Windows.Forms.ToolStripMenuItem
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.changeOpacityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TwentyfivePercent = New System.Windows.Forms.ToolStripMenuItem
        Me.fiftyPercent = New System.Windows.Forms.ToolStripMenuItem
        Me.seventyfivePercent = New System.Windows.Forms.ToolStripMenuItem
        Me.OnehundredPercent = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer
        Me.txtNote = New System.Windows.Forms.RichTextBox
        Me.docContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.formatContextMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.formatBoldContextMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.formatItalicsContextMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.formatUnderlineContextMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiSansSerif = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiSerif = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiMonoSpace = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiSmall = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiMedium = New System.Windows.Forms.ToolStripMenuItem
        Me.cmiLarge = New System.Windows.Forms.ToolStripMenuItem
        Me.formatToolStrip = New System.Windows.Forms.ToolStrip
        Me.formatToolStripLabel = New System.Windows.Forms.ToolStripLabel
        Me.formatToolStripBoldButton = New System.Windows.Forms.ToolStripButton
        Me.formatToolStripItalicsButton = New System.Windows.Forms.ToolStripButton
        Me.formatToolStripUnderlineButton = New System.Windows.Forms.ToolStripButton
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ЗаменаToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip.SuspendLayout()
        Me.FoldersToolStripButton.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.ToolStripContainer.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.ContentPanel.SuspendLayout()
        Me.ToolStripContainer.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.SuspendLayout()
        Me.docContextMenuStrip.SuspendLayout()
        Me.formatToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(632, 22)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'FoldersToolStripButton
        '
        Me.FoldersToolStripButton.Dock = System.Windows.Forms.DockStyle.None
        Me.FoldersToolStripButton.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.newToolStripButton, Me.openToolStripButton, Me.saveToolStripButton, Me.ToolStripSeparator7, Me.BackToolStripButton, Me.ForwardToolStripButton})
        Me.FoldersToolStripButton.Location = New System.Drawing.Point(3, 24)
        Me.FoldersToolStripButton.Name = "FoldersToolStripButton"
        Me.FoldersToolStripButton.Size = New System.Drawing.Size(209, 25)
        Me.FoldersToolStripButton.TabIndex = 0
        Me.FoldersToolStripButton.Text = "ToolStrip1"
        '
        'newToolStripButton
        '
        Me.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.newToolStripButton.Image = CType(resources.GetObject("newToolStripButton.Image"), System.Drawing.Image)
        Me.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.newToolStripButton.Name = "newToolStripButton"
        Me.newToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.newToolStripButton.Text = "&Новый"
        '
        'openToolStripButton
        '
        Me.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.openToolStripButton.Image = CType(resources.GetObject("openToolStripButton.Image"), System.Drawing.Image)
        Me.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.openToolStripButton.Name = "openToolStripButton"
        Me.openToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.openToolStripButton.Text = "&Открыть"
        '
        'saveToolStripButton
        '
        Me.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.saveToolStripButton.Image = CType(resources.GetObject("saveToolStripButton.Image"), System.Drawing.Image)
        Me.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.saveToolStripButton.Name = "saveToolStripButton"
        Me.saveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.saveToolStripButton.Text = "&Сохранить"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'BackToolStripButton
        '
        Me.BackToolStripButton.Enabled = False
        Me.BackToolStripButton.Image = CType(resources.GetObject("BackToolStripButton.Image"), System.Drawing.Image)
        Me.BackToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.BackToolStripButton.Name = "BackToolStripButton"
        Me.BackToolStripButton.Size = New System.Drawing.Size(52, 22)
        Me.BackToolStripButton.Text = "Back"
        Me.BackToolStripButton.ToolTipText = "Back to Previous Item"
        '
        'ForwardToolStripButton
        '
        Me.ForwardToolStripButton.Enabled = False
        Me.ForwardToolStripButton.Image = CType(resources.GetObject("ForwardToolStripButton.Image"), System.Drawing.Image)
        Me.ForwardToolStripButton.ImageTransparentColor = System.Drawing.Color.Black
        Me.ForwardToolStripButton.Name = "ForwardToolStripButton"
        Me.ForwardToolStripButton.Size = New System.Drawing.Size(70, 22)
        Me.ForwardToolStripButton.Text = "Forward"
        '
        'MenuStrip
        '
        Me.MenuStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(18, 18)
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.formatToolStripMenuItem, Me.mnuSearch, Me.OptionsToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(632, 24)
        Me.MenuStrip.TabIndex = 0
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.ToolStripSeparator1, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.ToolStripSeparator2, Me.PrintToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.ToolStripSeparator3, Me.ExitToolStripMenuItem, Me.ToolStripSeparator8, Me.mnuRecentFile0, Me.mnuRecentFile1, Me.mnuRecentFile2, Me.mnuRecentFile3, Me.mnuRecentFile4, Me.mnuRecentFile5})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(129, 20)
        Me.FileToolStripMenuItem.Text = "&Справка по режиму"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.NewToolStripMenuItem.Text = "&Новый"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.OpenToolStripMenuItem.Text = "&Открыть"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(230, 6)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.SaveToolStripMenuItem.Text = "&Сохранить"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.SaveAsToolStripMenuItem.Text = "Сохранить &Как"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(230, 6)
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Image = CType(resources.GetObject("PrintToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.PrintToolStripMenuItem.Text = "&Печать"
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Предварительный прос&мотр"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(230, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = CType(resources.GetObject("ExitToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(233, 22)
        Me.ExitToolStripMenuItem.Text = "&Закрыть окно"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(230, 6)
        '
        'mnuRecentFile0
        '
        Me.mnuRecentFile0.Name = "mnuRecentFile0"
        Me.mnuRecentFile0.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile0.Text = "НедавнийФайл0"
        Me.mnuRecentFile0.Visible = False
        '
        'mnuRecentFile1
        '
        Me.mnuRecentFile1.Name = "mnuRecentFile1"
        Me.mnuRecentFile1.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile1.Text = "НедавнийФайл1"
        Me.mnuRecentFile1.Visible = False
        '
        'mnuRecentFile2
        '
        Me.mnuRecentFile2.Name = "mnuRecentFile2"
        Me.mnuRecentFile2.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile2.Text = "НедавнийФайл2"
        Me.mnuRecentFile2.Visible = False
        '
        'mnuRecentFile3
        '
        Me.mnuRecentFile3.Name = "mnuRecentFile3"
        Me.mnuRecentFile3.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile3.Text = "НедавнийФайл3"
        Me.mnuRecentFile3.Visible = False
        '
        'mnuRecentFile4
        '
        Me.mnuRecentFile4.Name = "mnuRecentFile4"
        Me.mnuRecentFile4.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile4.Text = "НедавнийФайл4"
        Me.mnuRecentFile4.Visible = False
        '
        'mnuRecentFile5
        '
        Me.mnuRecentFile5.Name = "mnuRecentFile5"
        Me.mnuRecentFile5.Size = New System.Drawing.Size(233, 22)
        Me.mnuRecentFile5.Text = "НедавнийФайл5"
        Me.mnuRecentFile5.Visible = False
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.ToolStripSeparator4, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.mnuEditDelete, Me.ToolStripSeparator5, Me.SelectAllToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(99, 20)
        Me.EditToolStripMenuItem.Text = "&Редактировать"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Image = CType(resources.GetObject("UndoToolStripMenuItem.Image"), System.Drawing.Image)
        Me.UndoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.UndoToolStripMenuItem.Text = "&Отменить"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Image = CType(resources.GetObject("RedoToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RedoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.RedoToolStripMenuItem.Text = "&Вернуться"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(188, 6)
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.CutToolStripMenuItem.Text = "&Вырезать"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.CopyToolStripMenuItem.Text = "&Копировать"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.PasteToolStripMenuItem.Text = "В&ставить"
        '
        'mnuEditDelete
        '
        Me.mnuEditDelete.Image = CType(resources.GetObject("mnuEditDelete.Image"), System.Drawing.Image)
        Me.mnuEditDelete.Name = "mnuEditDelete"
        Me.mnuEditDelete.Size = New System.Drawing.Size(191, 22)
        Me.mnuEditDelete.Text = "&Удалить"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(188, 6)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.SelectAllToolStripMenuItem.Text = "В&ыделить Все"
        '
        'formatToolStripMenuItem
        '
        Me.formatToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.boldToolStripMenuItem, Me.italicsToolStripMenuItem, Me.underlineToolStripMenuItem, Me.ToolStripSeparator9, Me.FontFaceMenu, Me.FontSizeMenu})
        Me.formatToolStripMenuItem.Name = "formatToolStripMenuItem"
        Me.formatToolStripMenuItem.Size = New System.Drawing.Size(62, 20)
        Me.formatToolStripMenuItem.Text = "Фор&мат"
        '
        'boldToolStripMenuItem
        '
        Me.boldToolStripMenuItem.CheckOnClick = True
        Me.boldToolStripMenuItem.Image = Global.Registration.My.Resources.Resources.Bold
        Me.boldToolStripMenuItem.Name = "boldToolStripMenuItem"
        Me.boldToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.boldToolStripMenuItem.Text = "&Жирный"
        '
        'italicsToolStripMenuItem
        '
        Me.italicsToolStripMenuItem.CheckOnClick = True
        Me.italicsToolStripMenuItem.Image = Global.Registration.My.Resources.Resources.Italics
        Me.italicsToolStripMenuItem.Name = "italicsToolStripMenuItem"
        Me.italicsToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.italicsToolStripMenuItem.Text = "&Наклонный"
        '
        'underlineToolStripMenuItem
        '
        Me.underlineToolStripMenuItem.CheckOnClick = True
        Me.underlineToolStripMenuItem.Image = Global.Registration.My.Resources.Resources.Underline
        Me.underlineToolStripMenuItem.Name = "underlineToolStripMenuItem"
        Me.underlineToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.underlineToolStripMenuItem.Text = "&Подчеркнутый"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(159, 6)
        '
        'FontFaceMenu
        '
        Me.FontFaceMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mmiSansSerif, Me.mmiSerif, Me.mmiMonoSpace})
        Me.FontFaceMenu.Name = "FontFaceMenu"
        Me.FontFaceMenu.Size = New System.Drawing.Size(162, 22)
        Me.FontFaceMenu.Text = "Тип &Шрифта"
        '
        'mmiSansSerif
        '
        Me.mmiSansSerif.Checked = True
        Me.mmiSansSerif.CheckOnClick = True
        Me.mmiSansSerif.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mmiSansSerif.Name = "mmiSansSerif"
        Me.mmiSansSerif.Size = New System.Drawing.Size(190, 22)
        Me.mmiSansSerif.Text = "&1. Microsoft Sans Serif"
        '
        'mmiSerif
        '
        Me.mmiSerif.CheckOnClick = True
        Me.mmiSerif.Name = "mmiSerif"
        Me.mmiSerif.Size = New System.Drawing.Size(190, 22)
        Me.mmiSerif.Text = "&2. Times New Roman"
        '
        'mmiMonoSpace
        '
        Me.mmiMonoSpace.CheckOnClick = True
        Me.mmiMonoSpace.Name = "mmiMonoSpace"
        Me.mmiMonoSpace.Size = New System.Drawing.Size(190, 22)
        Me.mmiMonoSpace.Text = "&3. Courier New"
        '
        'FontSizeMenu
        '
        Me.FontSizeMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mmiSmall, Me.mmiMedium, Me.mmiLarge})
        Me.FontSizeMenu.Name = "FontSizeMenu"
        Me.FontSizeMenu.Size = New System.Drawing.Size(162, 22)
        Me.FontSizeMenu.Text = "&Размер шрифта"
        '
        'mmiSmall
        '
        Me.mmiSmall.CheckOnClick = True
        Me.mmiSmall.Name = "mmiSmall"
        Me.mmiSmall.Size = New System.Drawing.Size(137, 22)
        Me.mmiSmall.Text = "&Маленький"
        '
        'mmiMedium
        '
        Me.mmiMedium.Checked = True
        Me.mmiMedium.CheckOnClick = True
        Me.mmiMedium.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mmiMedium.Name = "mmiMedium"
        Me.mmiMedium.Size = New System.Drawing.Size(137, 22)
        Me.mmiMedium.Text = "&Средний"
        '
        'mmiLarge
        '
        Me.mmiLarge.CheckOnClick = True
        Me.mmiLarge.Name = "mmiLarge"
        Me.mmiLarge.Size = New System.Drawing.Size(137, 22)
        Me.mmiLarge.Text = "&Большой"
        '
        'mnuSearch
        '
        Me.mnuSearch.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSearchFind, Me.mnuSearchFindNext, Me.ЗаменаToolStripMenuItem})
        Me.mnuSearch.Name = "mnuSearch"
        Me.mnuSearch.Size = New System.Drawing.Size(54, 20)
        Me.mnuSearch.Text = "&Поиск"
        '
        'mnuSearchFind
        '
        Me.mnuSearchFind.Image = CType(resources.GetObject("mnuSearchFind.Image"), System.Drawing.Image)
        Me.mnuSearchFind.Name = "mnuSearchFind"
        Me.mnuSearchFind.Size = New System.Drawing.Size(157, 24)
        Me.mnuSearchFind.Text = "&Найти"
        '
        'mnuSearchFindNext
        '
        Me.mnuSearchFindNext.Image = CType(resources.GetObject("mnuSearchFindNext.Image"), System.Drawing.Image)
        Me.mnuSearchFindNext.Name = "mnuSearchFindNext"
        Me.mnuSearchFindNext.Size = New System.Drawing.Size(157, 24)
        Me.mnuSearchFindNext.Text = "Найти &Далее"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.changeOpacityToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.OptionsToolStripMenuItem.Text = "&Опции"
        '
        'changeOpacityToolStripMenuItem
        '
        Me.changeOpacityToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TwentyfivePercent, Me.fiftyPercent, Me.seventyfivePercent, Me.OnehundredPercent})
        Me.changeOpacityToolStripMenuItem.Image = CType(resources.GetObject("changeOpacityToolStripMenuItem.Image"), System.Drawing.Image)
        Me.changeOpacityToolStripMenuItem.Name = "changeOpacityToolStripMenuItem"
        Me.changeOpacityToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.changeOpacityToolStripMenuItem.Text = "&Изменить прозрачность"
        '
        'TwentyfivePercent
        '
        Me.TwentyfivePercent.CheckOnClick = True
        Me.TwentyfivePercent.Name = "TwentyfivePercent"
        Me.TwentyfivePercent.Size = New System.Drawing.Size(102, 22)
        Me.TwentyfivePercent.Tag = ".25"
        Me.TwentyfivePercent.Text = "&25%"
        '
        'fiftyPercent
        '
        Me.fiftyPercent.CheckOnClick = True
        Me.fiftyPercent.Name = "fiftyPercent"
        Me.fiftyPercent.Size = New System.Drawing.Size(102, 22)
        Me.fiftyPercent.Tag = ".50"
        Me.fiftyPercent.Text = "&50%"
        '
        'seventyfivePercent
        '
        Me.seventyfivePercent.CheckOnClick = True
        Me.seventyfivePercent.Name = "seventyfivePercent"
        Me.seventyfivePercent.Size = New System.Drawing.Size(102, 22)
        Me.seventyfivePercent.Tag = ".75"
        Me.seventyfivePercent.Text = "&75%"
        '
        'OnehundredPercent
        '
        Me.OnehundredPercent.CheckOnClick = True
        Me.OnehundredPercent.Name = "OnehundredPercent"
        Me.OnehundredPercent.Size = New System.Drawing.Size(102, 22)
        Me.OnehundredPercent.Tag = "1"
        Me.OnehundredPercent.Text = "&100%"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolBarToolStripMenuItem, Me.StatusBarToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.ViewToolStripMenuItem.Text = "&Вид"
        '
        'ToolBarToolStripMenuItem
        '
        Me.ToolBarToolStripMenuItem.Checked = True
        Me.ToolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolBarToolStripMenuItem.Name = "ToolBarToolStripMenuItem"
        Me.ToolBarToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.ToolBarToolStripMenuItem.Text = "Панель &инструментов"
        '
        'StatusBarToolStripMenuItem
        '
        Me.StatusBarToolStripMenuItem.Checked = True
        Me.StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        Me.StatusBarToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.StatusBarToolStripMenuItem.Text = "Панель &статуса"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.IndexToolStripMenuItem, Me.SearchToolStripMenuItem, Me.ToolStripSeparator6, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(68, 20)
        Me.HelpToolStripMenuItem.Text = "&Помощь"
        '
        'ContentsToolStripMenuItem
        '
        Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
        Me.ContentsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.ContentsToolStripMenuItem.Text = "&Contents"
        '
        'IndexToolStripMenuItem
        '
        Me.IndexToolStripMenuItem.Image = CType(resources.GetObject("IndexToolStripMenuItem.Image"), System.Drawing.Image)
        Me.IndexToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.IndexToolStripMenuItem.Name = "IndexToolStripMenuItem"
        Me.IndexToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.IndexToolStripMenuItem.Text = "&Index"
        '
        'SearchToolStripMenuItem
        '
        Me.SearchToolStripMenuItem.Image = CType(resources.GetObject("SearchToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SearchToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.SearchToolStripMenuItem.Text = "&Search"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(165, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'ToolStripContainer
        '
        '
        'ToolStripContainer.BottomToolStripPanel
        '
        Me.ToolStripContainer.BottomToolStripPanel.Controls.Add(Me.StatusStrip)
        '
        'ToolStripContainer.ContentPanel
        '
        Me.ToolStripContainer.ContentPanel.Controls.Add(Me.txtNote)
        Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(632, 382)
        Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer.Name = "ToolStripContainer"
        Me.ToolStripContainer.Size = New System.Drawing.Size(632, 453)
        Me.ToolStripContainer.TabIndex = 7
        Me.ToolStripContainer.Text = "ToolStripContainer1"
        '
        'ToolStripContainer.TopToolStripPanel
        '
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.MenuStrip)
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.formatToolStrip)
        Me.ToolStripContainer.TopToolStripPanel.Controls.Add(Me.FoldersToolStripButton)
        '
        'txtNote
        '
        Me.txtNote.ContextMenuStrip = Me.docContextMenuStrip
        Me.txtNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtNote.Location = New System.Drawing.Point(0, 0)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(632, 382)
        Me.txtNote.TabIndex = 9
        Me.txtNote.Text = ""
        '
        'docContextMenuStrip
        '
        Me.docContextMenuStrip.ImageScalingSize = New System.Drawing.Size(18, 18)
        Me.docContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.formatContextMenuItem, Me.ToolStripMenuItem1, Me.ToolStripMenuItem2})
        Me.docContextMenuStrip.Name = "docContextMenuStrip"
        Me.docContextMenuStrip.Size = New System.Drawing.Size(163, 70)
        '
        'formatContextMenuItem
        '
        Me.formatContextMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.formatBoldContextMenuItem, Me.formatItalicsContextMenuItem, Me.formatUnderlineContextMenuItem})
        Me.formatContextMenuItem.Name = "formatContextMenuItem"
        Me.formatContextMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.formatContextMenuItem.Text = "Формат"
        '
        'formatBoldContextMenuItem
        '
        Me.formatBoldContextMenuItem.Image = Global.Registration.My.Resources.Resources.Bold
        Me.formatBoldContextMenuItem.Name = "formatBoldContextMenuItem"
        Me.formatBoldContextMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.formatBoldContextMenuItem.Text = "&Жирный"
        '
        'formatItalicsContextMenuItem
        '
        Me.formatItalicsContextMenuItem.Image = Global.Registration.My.Resources.Resources.Italics
        Me.formatItalicsContextMenuItem.Name = "formatItalicsContextMenuItem"
        Me.formatItalicsContextMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.formatItalicsContextMenuItem.Text = "&Наклонный"
        '
        'formatUnderlineContextMenuItem
        '
        Me.formatUnderlineContextMenuItem.Image = Global.Registration.My.Resources.Resources.Underline
        Me.formatUnderlineContextMenuItem.Name = "formatUnderlineContextMenuItem"
        Me.formatUnderlineContextMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.formatUnderlineContextMenuItem.Text = "&Подчеркнутый"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmiSansSerif, Me.cmiSerif, Me.cmiMonoSpace})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem1.Text = "Тип &Шрифта"
        '
        'cmiSansSerif
        '
        Me.cmiSansSerif.Checked = True
        Me.cmiSansSerif.CheckOnClick = True
        Me.cmiSansSerif.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cmiSansSerif.Name = "cmiSansSerif"
        Me.cmiSansSerif.Size = New System.Drawing.Size(190, 22)
        Me.cmiSansSerif.Text = "&1. Microsoft Sans Serif"
        '
        'cmiSerif
        '
        Me.cmiSerif.CheckOnClick = True
        Me.cmiSerif.Name = "cmiSerif"
        Me.cmiSerif.Size = New System.Drawing.Size(190, 22)
        Me.cmiSerif.Text = "&2. Times New Roman"
        '
        'cmiMonoSpace
        '
        Me.cmiMonoSpace.CheckOnClick = True
        Me.cmiMonoSpace.Name = "cmiMonoSpace"
        Me.cmiMonoSpace.Size = New System.Drawing.Size(190, 22)
        Me.cmiMonoSpace.Text = "&3. Courier New"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmiSmall, Me.cmiMedium, Me.cmiLarge})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem2.Text = "&Размер Шрифта"
        '
        'cmiSmall
        '
        Me.cmiSmall.CheckOnClick = True
        Me.cmiSmall.Name = "cmiSmall"
        Me.cmiSmall.Size = New System.Drawing.Size(137, 22)
        Me.cmiSmall.Text = "&Маленький"
        '
        'cmiMedium
        '
        Me.cmiMedium.Checked = True
        Me.cmiMedium.CheckOnClick = True
        Me.cmiMedium.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cmiMedium.Name = "cmiMedium"
        Me.cmiMedium.Size = New System.Drawing.Size(137, 22)
        Me.cmiMedium.Text = "&Средний"
        '
        'cmiLarge
        '
        Me.cmiLarge.CheckOnClick = True
        Me.cmiLarge.Name = "cmiLarge"
        Me.cmiLarge.Size = New System.Drawing.Size(137, 22)
        Me.cmiLarge.Text = "&Большой"
        '
        'formatToolStrip
        '
        Me.formatToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.formatToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.formatToolStripLabel, Me.formatToolStripBoldButton, Me.formatToolStripItalicsButton, Me.formatToolStripUnderlineButton})
        Me.formatToolStrip.Location = New System.Drawing.Point(212, 24)
        Me.formatToolStrip.Name = "formatToolStrip"
        Me.formatToolStrip.Size = New System.Drawing.Size(126, 25)
        Me.formatToolStrip.TabIndex = 1
        '
        'formatToolStripLabel
        '
        Me.formatToolStripLabel.Name = "formatToolStripLabel"
        Me.formatToolStripLabel.Size = New System.Drawing.Size(45, 22)
        Me.formatToolStripLabel.Text = "Format"
        '
        'formatToolStripBoldButton
        '
        Me.formatToolStripBoldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.formatToolStripBoldButton.Image = Global.Registration.My.Resources.Resources.Bold
        Me.formatToolStripBoldButton.Name = "formatToolStripBoldButton"
        Me.formatToolStripBoldButton.Size = New System.Drawing.Size(23, 22)
        Me.formatToolStripBoldButton.Text = "&Bold"
        '
        'formatToolStripItalicsButton
        '
        Me.formatToolStripItalicsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.formatToolStripItalicsButton.Image = Global.Registration.My.Resources.Resources.Italics
        Me.formatToolStripItalicsButton.Name = "formatToolStripItalicsButton"
        Me.formatToolStripItalicsButton.Size = New System.Drawing.Size(23, 22)
        Me.formatToolStripItalicsButton.Text = "&Italics"
        '
        'formatToolStripUnderlineButton
        '
        Me.formatToolStripUnderlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.formatToolStripUnderlineButton.Image = Global.Registration.My.Resources.Resources.Underline
        Me.formatToolStripUnderlineButton.Name = "formatToolStripUnderlineButton"
        Me.formatToolStripUnderlineButton.Size = New System.Drawing.Size(23, 22)
        Me.formatToolStripUnderlineButton.Text = "&Underline"
        '
        'openFileDialog
        '
        Me.openFileDialog.FileName = "openFileDialog1"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "doc1"
        '
        'ЗаменаToolStripMenuItem
        '
        Me.ЗаменаToolStripMenuItem.Name = "ЗаменаToolStripMenuItem"
        Me.ЗаменаToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.ЗаменаToolStripMenuItem.Size = New System.Drawing.Size(157, 24)
        Me.ЗаменаToolStripMenuItem.Text = "Замена"
        '
        'frmTextEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 453)
        Me.Controls.Add(Me.ToolStripContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTextEditor"
        Me.Text = "Блокнот"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.FoldersToolStripButton.ResumeLayout(False)
        Me.FoldersToolStripButton.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ToolStripContainer.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ResumeLayout(False)
        Me.ToolStripContainer.PerformLayout()
        Me.docContextMenuStrip.ResumeLayout(False)
        Me.formatToolStrip.ResumeLayout(False)
        Me.formatToolStrip.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtNote As System.Windows.Forms.RichTextBox
    Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents docContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents formatContextMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents formatBoldContextMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents formatItalicsContextMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents formatUnderlineContextMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents formatToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents boldToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents italicsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents underlineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents changeOpacityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TwentyfivePercent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents fiftyPercent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents seventyfivePercent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OnehundredPercent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents openToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents saveToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents formatToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents formatToolStripLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents formatToolStripBoldButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents formatToolStripItalicsButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents formatToolStripUnderlineButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents mnuSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRecentFile1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecentFile2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecentFile3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecentFile4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecentFile5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSearchFind As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSearchFindNext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FontFaceMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiSansSerif As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiSerif As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiMonoSpace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FontSizeMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiSmall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiMedium As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mmiLarge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiSansSerif As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiSerif As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiMonoSpace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiSmall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiMedium As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiLarge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecentFile0 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ЗаменаToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
