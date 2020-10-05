<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegularExpressionsReplace
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRegularExpressionsReplace))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.tpSetings = New System.Windows.Forms.TabPage()
        Me.tbExtensions = New System.Windows.Forms.TextBox()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.pbBroseFolder = New System.Windows.Forms.Button()
        Me.lblFilesPatern = New System.Windows.Forms.Label()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainerSetting = New System.Windows.Forms.SplitContainer()
        Me.tbFind = New System.Windows.Forms.TextBox()
        Me.TextBoxCaptionToSearch = New System.Windows.Forms.TextBox()
        Me.tbReplaceTo = New System.Windows.Forms.TextBox()
        Me.TextBoxCaptionToReplace = New System.Windows.Forms.TextBox()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.gbRegExpOptions = New System.Windows.Forms.GroupBox()
        Me.tbReplCount = New System.Windows.Forms.TextBox()
        Me.cbReplAll = New System.Windows.Forms.CheckBox()
        Me.cbIgnoreCase = New System.Windows.Forms.CheckBox()
        Me.cbMultiline = New System.Windows.Forms.CheckBox()
        Me.cbExplicitCapture = New System.Windows.Forms.CheckBox()
        Me.cbSingleline = New System.Windows.Forms.CheckBox()
        Me.cbIgnorePatternWhitespace = New System.Windows.Forms.CheckBox()
        Me.cbCompiled = New System.Windows.Forms.CheckBox()
        Me.tpPreview = New System.Windows.Forms.TabPage()
        Me.SplitContainerPrivewVert = New System.Windows.Forms.SplitContainer()
        Me.tvFiles = New System.Windows.Forms.TreeView()
        Me.imageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.TextBoxCaptionTree = New System.Windows.Forms.TextBox()
        Me.SplitContainerPreviewHor = New System.Windows.Forms.SplitContainer()
        Me.lbFileMatch = New System.Windows.Forms.ListBox()
        Me.TextBoxCaptionFileMath = New System.Windows.Forms.TextBox()
        Me.tabPreview = New System.Windows.Forms.TabControl()
        Me.tabPgFindeOnly = New System.Windows.Forms.TabPage()
        Me.tbFileBody = New System.Windows.Forms.TextBox()
        Me.TextBoxCaptionFileBody = New System.Windows.Forms.TextBox()
        Me.tabPgFindeAndReplace = New System.Windows.Forms.TabPage()
        Me.SplitContainerFindReplace = New System.Windows.Forms.SplitContainer()
        Me.pnFindeAndReplace = New System.Windows.Forms.Panel()
        Me.TextBoxCaptionFindeAndReplace = New System.Windows.Forms.TextBox()
        Me.tbReplaceResult = New System.Windows.Forms.TextBox()
        Me.TextBoxCaptionReplaceResult = New System.Windows.Forms.TextBox()
        Me.tpLog = New System.Windows.Forms.TabPage()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tbbClearLog = New System.Windows.Forms.ToolStripButton()
        Me.tbbSaveLog = New System.Windows.Forms.ToolStripButton()
        Me.statusBar1 = New System.Windows.Forms.StatusStrip()
        Me.sbpMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sbpFileCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.miNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.miOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.miSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.miSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.miBrowseForFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.miRecent = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.miFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EscapesSelectedTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.miRunGo = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolBar1 = New System.Windows.Forms.ToolStrip()
        Me.tbbNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbbOpen = New System.Windows.Forms.ToolStripSplitButton()
        Me.tbbSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbbReplaceInFile = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tbbEsc = New System.Windows.Forms.ToolStripButton()
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.SaveLog = New System.Windows.Forms.SaveFileDialog()
        Me.BrowseFolderDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.tpSetings.SuspendLayout()
        Me.panel1.SuspendLayout()
        CType(Me.SplitContainerSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerSetting.Panel1.SuspendLayout()
        Me.SplitContainerSetting.Panel2.SuspendLayout()
        Me.SplitContainerSetting.SuspendLayout()
        Me.gbRegExpOptions.SuspendLayout()
        Me.tpPreview.SuspendLayout()
        CType(Me.SplitContainerPrivewVert, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerPrivewVert.Panel1.SuspendLayout()
        Me.SplitContainerPrivewVert.Panel2.SuspendLayout()
        Me.SplitContainerPrivewVert.SuspendLayout()
        CType(Me.SplitContainerPreviewHor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerPreviewHor.Panel1.SuspendLayout()
        Me.SplitContainerPreviewHor.Panel2.SuspendLayout()
        Me.SplitContainerPreviewHor.SuspendLayout()
        Me.tabPreview.SuspendLayout()
        Me.tabPgFindeOnly.SuspendLayout()
        Me.tabPgFindeAndReplace.SuspendLayout()
        CType(Me.SplitContainerFindReplace, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerFindReplace.Panel1.SuspendLayout()
        Me.SplitContainerFindReplace.Panel2.SuspendLayout()
        Me.SplitContainerFindReplace.SuspendLayout()
        Me.tpLog.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.statusBar1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.toolBar1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.tabControl1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.statusBar1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(624, 393)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(624, 442)
        Me.ToolStripContainer1.TabIndex = 0
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.toolBar1)
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tpSetings)
        Me.tabControl1.Controls.Add(Me.tpPreview)
        Me.tabControl1.Controls.Add(Me.tpLog)
        Me.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl1.Location = New System.Drawing.Point(0, 0)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(624, 369)
        Me.tabControl1.TabIndex = 3
        '
        'tpSetings
        '
        Me.tpSetings.Controls.Add(Me.tbExtensions)
        Me.tpSetings.Controls.Add(Me.tbPath)
        Me.tpSetings.Controls.Add(Me.pbBroseFolder)
        Me.tpSetings.Controls.Add(Me.lblFilesPatern)
        Me.tpSetings.Controls.Add(Me.panel1)
        Me.tpSetings.Controls.Add(Me.lblPath)
        Me.tpSetings.Controls.Add(Me.gbRegExpOptions)
        Me.tpSetings.Location = New System.Drawing.Point(4, 22)
        Me.tpSetings.Name = "tpSetings"
        Me.tpSetings.Size = New System.Drawing.Size(616, 343)
        Me.tpSetings.TabIndex = 0
        Me.tpSetings.Text = "Настройки поиска и замены"
        '
        'tbExtensions
        '
        Me.tbExtensions.AccessibleDescription = "tbExtensions"
        Me.tbExtensions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbExtensions.Location = New System.Drawing.Point(513, 10)
        Me.tbExtensions.Name = "tbExtensions"
        Me.tbExtensions.Size = New System.Drawing.Size(74, 20)
        Me.tbExtensions.TabIndex = 13
        Me.tbExtensions.Text = "*.*"
        Me.ToolTip1.SetToolTip(Me.tbExtensions, "Шаблон в формате расширения файлов (*.xml) (маска может быть такой «*.txt;*.htm*;" & _
        "*.asp»)")
        '
        'tbPath
        '
        Me.tbPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPath.ForeColor = System.Drawing.Color.Maroon
        Me.tbPath.Location = New System.Drawing.Point(198, 320)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(377, 20)
        Me.tbPath.TabIndex = 9
        Me.tbPath.Text = "textBox1"
        Me.ToolTip1.SetToolTip(Me.tbPath, "Установленный путь к  папке ""Функциональные Панели""")
        '
        'pbBroseFolder
        '
        Me.pbBroseFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbBroseFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.pbBroseFolder.Location = New System.Drawing.Point(581, 318)
        Me.pbBroseFolder.Name = "pbBroseFolder"
        Me.pbBroseFolder.Size = New System.Drawing.Size(28, 22)
        Me.pbBroseFolder.TabIndex = 8
        Me.pbBroseFolder.Text = "..."
        Me.pbBroseFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.pbBroseFolder, "Изменить папку ""Функциональные Панели""")
        '
        'lblFilesPatern
        '
        Me.lblFilesPatern.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilesPatern.Location = New System.Drawing.Point(433, 13)
        Me.lblFilesPatern.Name = "lblFilesPatern"
        Me.lblFilesPatern.Size = New System.Drawing.Size(74, 32)
        Me.lblFilesPatern.TabIndex = 7
        Me.lblFilesPatern.Text = "Шаблон типа файлов"
        '
        'panel1
        '
        Me.panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.Controls.Add(Me.SplitContainerSetting)
        Me.panel1.Location = New System.Drawing.Point(8, 8)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(419, 309)
        Me.panel1.TabIndex = 3
        '
        'SplitContainerSetting
        '
        Me.SplitContainerSetting.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerSetting.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerSetting.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerSetting.Name = "SplitContainerSetting"
        Me.SplitContainerSetting.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerSetting.Panel1
        '
        Me.SplitContainerSetting.Panel1.Controls.Add(Me.tbFind)
        Me.SplitContainerSetting.Panel1.Controls.Add(Me.TextBoxCaptionToSearch)
        '
        'SplitContainerSetting.Panel2
        '
        Me.SplitContainerSetting.Panel2.Controls.Add(Me.tbReplaceTo)
        Me.SplitContainerSetting.Panel2.Controls.Add(Me.TextBoxCaptionToReplace)
        Me.SplitContainerSetting.Size = New System.Drawing.Size(419, 309)
        Me.SplitContainerSetting.SplitterDistance = 148
        Me.SplitContainerSetting.TabIndex = 6
        '
        'tbFind
        '
        Me.tbFind.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.tbFind.ForeColor = System.Drawing.Color.Blue
        Me.tbFind.Location = New System.Drawing.Point(0, 20)
        Me.tbFind.Multiline = True
        Me.tbFind.Name = "tbFind"
        Me.tbFind.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbFind.Size = New System.Drawing.Size(415, 124)
        Me.tbFind.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.tbFind, "Ввести текст для поиска")
        Me.tbFind.WordWrap = False
        '
        'TextBoxCaptionToSearch
        '
        Me.TextBoxCaptionToSearch.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionToSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionToSearch.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionToSearch.Name = "TextBoxCaptionToSearch"
        Me.TextBoxCaptionToSearch.ReadOnly = True
        Me.TextBoxCaptionToSearch.Size = New System.Drawing.Size(415, 20)
        Me.TextBoxCaptionToSearch.TabIndex = 4
        Me.TextBoxCaptionToSearch.Text = "Текст для поиска:"
        Me.TextBoxCaptionToSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbReplaceTo
        '
        Me.tbReplaceTo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbReplaceTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.tbReplaceTo.ForeColor = System.Drawing.Color.Red
        Me.tbReplaceTo.Location = New System.Drawing.Point(0, 20)
        Me.tbReplaceTo.Multiline = True
        Me.tbReplaceTo.Name = "tbReplaceTo"
        Me.tbReplaceTo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbReplaceTo.Size = New System.Drawing.Size(415, 133)
        Me.tbReplaceTo.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.tbReplaceTo, "Ввести заменяемый текст")
        Me.tbReplaceTo.WordWrap = False
        '
        'TextBoxCaptionToReplace
        '
        Me.TextBoxCaptionToReplace.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionToReplace.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionToReplace.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionToReplace.Name = "TextBoxCaptionToReplace"
        Me.TextBoxCaptionToReplace.ReadOnly = True
        Me.TextBoxCaptionToReplace.Size = New System.Drawing.Size(415, 20)
        Me.TextBoxCaptionToReplace.TabIndex = 6
        Me.TextBoxCaptionToReplace.Text = "Текст для замены:"
        Me.TextBoxCaptionToReplace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPath
        '
        Me.lblPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(8, 323)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(184, 13)
        Me.lblPath.TabIndex = 5
        Me.lblPath.Text = "Папка ""Функциональные панели"":"
        '
        'gbRegExpOptions
        '
        Me.gbRegExpOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbRegExpOptions.Controls.Add(Me.tbReplCount)
        Me.gbRegExpOptions.Controls.Add(Me.cbReplAll)
        Me.gbRegExpOptions.Controls.Add(Me.cbIgnoreCase)
        Me.gbRegExpOptions.Controls.Add(Me.cbMultiline)
        Me.gbRegExpOptions.Controls.Add(Me.cbExplicitCapture)
        Me.gbRegExpOptions.Controls.Add(Me.cbSingleline)
        Me.gbRegExpOptions.Controls.Add(Me.cbIgnorePatternWhitespace)
        Me.gbRegExpOptions.Controls.Add(Me.cbCompiled)
        Me.gbRegExpOptions.Location = New System.Drawing.Point(436, 48)
        Me.gbRegExpOptions.Name = "gbRegExpOptions"
        Me.gbRegExpOptions.Size = New System.Drawing.Size(172, 269)
        Me.gbRegExpOptions.TabIndex = 14
        Me.gbRegExpOptions.TabStop = False
        Me.gbRegExpOptions.Text = "Опции рег. выражения"
        Me.ToolTip1.SetToolTip(Me.gbRegExpOptions, "Опции регулярного выражения")
        '
        'tbReplCount
        '
        Me.tbReplCount.Location = New System.Drawing.Point(122, 22)
        Me.tbReplCount.Name = "tbReplCount"
        Me.tbReplCount.Size = New System.Drawing.Size(44, 20)
        Me.tbReplCount.TabIndex = 6
        Me.tbReplCount.Text = "1"
        Me.tbReplCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.tbReplCount.Visible = False
        '
        'cbReplAll
        '
        Me.cbReplAll.Checked = True
        Me.cbReplAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbReplAll.Location = New System.Drawing.Point(8, 24)
        Me.cbReplAll.Name = "cbReplAll"
        Me.cbReplAll.Size = New System.Drawing.Size(158, 16)
        Me.cbReplAll.TabIndex = 5
        Me.cbReplAll.Text = "&Заменять Все"
        Me.ToolTip1.SetToolTip(Me.cbReplAll, "Заменять все вхождения или определенное количество")
        '
        'cbIgnoreCase
        '
        Me.cbIgnoreCase.Checked = True
        Me.cbIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbIgnoreCase.Location = New System.Drawing.Point(8, 48)
        Me.cbIgnoreCase.Name = "cbIgnoreCase"
        Me.cbIgnoreCase.Size = New System.Drawing.Size(158, 16)
        Me.cbIgnoreCase.TabIndex = 7
        Me.cbIgnoreCase.Text = "&Игнорировать Регистр"
        Me.ToolTip1.SetToolTip(Me.cbIgnoreCase, "Игнорировать регистр букв")
        '
        'cbMultiline
        '
        Me.cbMultiline.Checked = True
        Me.cbMultiline.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbMultiline.Location = New System.Drawing.Point(8, 72)
        Me.cbMultiline.Name = "cbMultiline"
        Me.cbMultiline.Size = New System.Drawing.Size(158, 16)
        Me.cbMultiline.TabIndex = 8
        Me.cbMultiline.Text = "&Текст как единая строка"
        Me.ToolTip1.SetToolTip(Me.cbMultiline, "Воспринимать текст как единую строку")
        '
        'cbExplicitCapture
        '
        Me.cbExplicitCapture.Location = New System.Drawing.Point(8, 96)
        Me.cbExplicitCapture.Name = "cbExplicitCapture"
        Me.cbExplicitCapture.Size = New System.Drawing.Size(158, 16)
        Me.cbExplicitCapture.TabIndex = 9
        Me.cbExplicitCapture.Text = "&Точный захват"
        Me.ToolTip1.SetToolTip(Me.cbExplicitCapture, "Не учитывать неименованные группы ""()"" как группы захватывающие значения ($x) ")
        '
        'cbSingleline
        '
        Me.cbSingleline.Checked = True
        Me.cbSingleline.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSingleline.Location = New System.Drawing.Point(8, 120)
        Me.cbSingleline.Name = "cbSingleline"
        Me.cbSingleline.Size = New System.Drawing.Size(158, 16)
        Me.cbSingleline.TabIndex = 10
        Me.cbSingleline.Text = "&Разделять линии"
        Me.ToolTip1.SetToolTip(Me.cbSingleline, "Интерпретировать «.» (подстановочный символ точка), как любой символ, включая кон" & _
        "ец строки")
        '
        'cbIgnorePatternWhitespace
        '
        Me.cbIgnorePatternWhitespace.Location = New System.Drawing.Point(8, 144)
        Me.cbIgnorePatternWhitespace.Name = "cbIgnorePatternWhitespace"
        Me.cbIgnorePatternWhitespace.Size = New System.Drawing.Size(158, 16)
        Me.cbIgnorePatternWhitespace.TabIndex = 11
        Me.cbIgnorePatternWhitespace.Text = "&Игнорировать пробелы"
        Me.ToolTip1.SetToolTip(Me.cbIgnorePatternWhitespace, "Игнорировать лишние пробелы")
        '
        'cbCompiled
        '
        Me.cbCompiled.Location = New System.Drawing.Point(8, 168)
        Me.cbCompiled.Name = "cbCompiled"
        Me.cbCompiled.Size = New System.Drawing.Size(158, 16)
        Me.cbCompiled.TabIndex = 12
        Me.cbCompiled.Text = "&Компилировать"
        Me.ToolTip1.SetToolTip(Me.cbCompiled, "Компилировать выражение перед выполнением")
        '
        'tpPreview
        '
        Me.tpPreview.Controls.Add(Me.SplitContainerPrivewVert)
        Me.tpPreview.Location = New System.Drawing.Point(4, 22)
        Me.tpPreview.Name = "tpPreview"
        Me.tpPreview.Size = New System.Drawing.Size(776, 463)
        Me.tpPreview.TabIndex = 2
        Me.tpPreview.Text = "Просмотр"
        '
        'SplitContainerPrivewVert
        '
        Me.SplitContainerPrivewVert.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerPrivewVert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerPrivewVert.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainerPrivewVert.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerPrivewVert.Name = "SplitContainerPrivewVert"
        '
        'SplitContainerPrivewVert.Panel1
        '
        Me.SplitContainerPrivewVert.Panel1.Controls.Add(Me.tvFiles)
        Me.SplitContainerPrivewVert.Panel1.Controls.Add(Me.TextBoxCaptionTree)
        '
        'SplitContainerPrivewVert.Panel2
        '
        Me.SplitContainerPrivewVert.Panel2.Controls.Add(Me.SplitContainerPreviewHor)
        Me.SplitContainerPrivewVert.Size = New System.Drawing.Size(776, 463)
        Me.SplitContainerPrivewVert.SplitterDistance = 183
        Me.SplitContainerPrivewVert.TabIndex = 3
        '
        'tvFiles
        '
        Me.tvFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvFiles.ForeColor = System.Drawing.Color.Maroon
        Me.tvFiles.HideSelection = False
        Me.tvFiles.ImageIndex = 0
        Me.tvFiles.ImageList = Me.imageList2
        Me.tvFiles.Location = New System.Drawing.Point(0, 20)
        Me.tvFiles.Name = "tvFiles"
        Me.tvFiles.SelectedImageIndex = 0
        Me.tvFiles.Size = New System.Drawing.Size(179, 439)
        Me.tvFiles.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.tvFiles, "Файлы в выбранной папке")
        '
        'imageList2
        '
        Me.imageList2.ImageStream = CType(resources.GetObject("imageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList2.TransparentColor = System.Drawing.Color.Maroon
        Me.imageList2.Images.SetKeyName(0, "folder_side.png")
        Me.imageList2.Images.SetKeyName(1, "folder_open.png")
        Me.imageList2.Images.SetKeyName(2, "binary.png")
        '
        'TextBoxCaptionTree
        '
        Me.TextBoxCaptionTree.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionTree.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionTree.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionTree.Name = "TextBoxCaptionTree"
        Me.TextBoxCaptionTree.ReadOnly = True
        Me.TextBoxCaptionTree.Size = New System.Drawing.Size(179, 20)
        Me.TextBoxCaptionTree.TabIndex = 5
        Me.TextBoxCaptionTree.Text = "Файлы в выбранной папке:"
        Me.TextBoxCaptionTree.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SplitContainerPreviewHor
        '
        Me.SplitContainerPreviewHor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerPreviewHor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerPreviewHor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainerPreviewHor.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerPreviewHor.Name = "SplitContainerPreviewHor"
        Me.SplitContainerPreviewHor.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerPreviewHor.Panel1
        '
        Me.SplitContainerPreviewHor.Panel1.Controls.Add(Me.lbFileMatch)
        Me.SplitContainerPreviewHor.Panel1.Controls.Add(Me.TextBoxCaptionFileMath)
        '
        'SplitContainerPreviewHor.Panel2
        '
        Me.SplitContainerPreviewHor.Panel2.Controls.Add(Me.tabPreview)
        Me.SplitContainerPreviewHor.Size = New System.Drawing.Size(589, 463)
        Me.SplitContainerPreviewHor.SplitterDistance = 127
        Me.SplitContainerPreviewHor.TabIndex = 4
        '
        'lbFileMatch
        '
        Me.lbFileMatch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbFileMatch.ForeColor = System.Drawing.Color.Olive
        Me.lbFileMatch.Location = New System.Drawing.Point(0, 20)
        Me.lbFileMatch.Name = "lbFileMatch"
        Me.lbFileMatch.Size = New System.Drawing.Size(585, 103)
        Me.lbFileMatch.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.lbFileMatch, "Найденный текст")
        '
        'TextBoxCaptionFileMath
        '
        Me.TextBoxCaptionFileMath.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionFileMath.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionFileMath.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionFileMath.Name = "TextBoxCaptionFileMath"
        Me.TextBoxCaptionFileMath.ReadOnly = True
        Me.TextBoxCaptionFileMath.Size = New System.Drawing.Size(585, 20)
        Me.TextBoxCaptionFileMath.TabIndex = 6
        Me.TextBoxCaptionFileMath.Text = "Найденный текст:"
        Me.TextBoxCaptionFileMath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabPreview
        '
        Me.tabPreview.Controls.Add(Me.tabPgFindeOnly)
        Me.tabPreview.Controls.Add(Me.tabPgFindeAndReplace)
        Me.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabPreview.Location = New System.Drawing.Point(0, 0)
        Me.tabPreview.Name = "tabPreview"
        Me.tabPreview.SelectedIndex = 0
        Me.tabPreview.Size = New System.Drawing.Size(585, 328)
        Me.tabPreview.TabIndex = 4
        '
        'tabPgFindeOnly
        '
        Me.tabPgFindeOnly.Controls.Add(Me.tbFileBody)
        Me.tabPgFindeOnly.Controls.Add(Me.TextBoxCaptionFileBody)
        Me.tabPgFindeOnly.Location = New System.Drawing.Point(4, 22)
        Me.tabPgFindeOnly.Name = "tabPgFindeOnly"
        Me.tabPgFindeOnly.Size = New System.Drawing.Size(577, 302)
        Me.tabPgFindeOnly.TabIndex = 0
        Me.tabPgFindeOnly.Text = "Только поиск"
        '
        'tbFileBody
        '
        Me.tbFileBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbFileBody.HideSelection = False
        Me.tbFileBody.Location = New System.Drawing.Point(0, 20)
        Me.tbFileBody.Multiline = True
        Me.tbFileBody.Name = "tbFileBody"
        Me.tbFileBody.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbFileBody.Size = New System.Drawing.Size(577, 282)
        Me.tbFileBody.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.tbFileBody, "Содержание файла")
        Me.tbFileBody.WordWrap = False
        '
        'TextBoxCaptionFileBody
        '
        Me.TextBoxCaptionFileBody.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionFileBody.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionFileBody.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionFileBody.Name = "TextBoxCaptionFileBody"
        Me.TextBoxCaptionFileBody.ReadOnly = True
        Me.TextBoxCaptionFileBody.Size = New System.Drawing.Size(577, 20)
        Me.TextBoxCaptionFileBody.TabIndex = 8
        Me.TextBoxCaptionFileBody.Text = "Содержание файла:"
        Me.TextBoxCaptionFileBody.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabPgFindeAndReplace
        '
        Me.tabPgFindeAndReplace.Controls.Add(Me.SplitContainerFindReplace)
        Me.tabPgFindeAndReplace.Location = New System.Drawing.Point(4, 22)
        Me.tabPgFindeAndReplace.Name = "tabPgFindeAndReplace"
        Me.tabPgFindeAndReplace.Size = New System.Drawing.Size(577, 302)
        Me.tabPgFindeAndReplace.TabIndex = 1
        Me.tabPgFindeAndReplace.Text = "Поиск и Замена"
        '
        'SplitContainerFindReplace
        '
        Me.SplitContainerFindReplace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerFindReplace.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerFindReplace.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerFindReplace.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerFindReplace.Name = "SplitContainerFindReplace"
        Me.SplitContainerFindReplace.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerFindReplace.Panel1
        '
        Me.SplitContainerFindReplace.Panel1.Controls.Add(Me.pnFindeAndReplace)
        Me.SplitContainerFindReplace.Panel1.Controls.Add(Me.TextBoxCaptionFindeAndReplace)
        '
        'SplitContainerFindReplace.Panel2
        '
        Me.SplitContainerFindReplace.Panel2.Controls.Add(Me.tbReplaceResult)
        Me.SplitContainerFindReplace.Panel2.Controls.Add(Me.TextBoxCaptionReplaceResult)
        Me.SplitContainerFindReplace.Size = New System.Drawing.Size(577, 302)
        Me.SplitContainerFindReplace.SplitterDistance = 212
        Me.SplitContainerFindReplace.TabIndex = 7
        '
        'pnFindeAndReplace
        '
        Me.pnFindeAndReplace.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnFindeAndReplace.Location = New System.Drawing.Point(0, 20)
        Me.pnFindeAndReplace.Name = "pnFindeAndReplace"
        Me.pnFindeAndReplace.Size = New System.Drawing.Size(573, 188)
        Me.pnFindeAndReplace.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.pnFindeAndReplace, "Найденное вхождение в тексте")
        '
        'TextBoxCaptionFindeAndReplace
        '
        Me.TextBoxCaptionFindeAndReplace.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionFindeAndReplace.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionFindeAndReplace.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionFindeAndReplace.Name = "TextBoxCaptionFindeAndReplace"
        Me.TextBoxCaptionFindeAndReplace.ReadOnly = True
        Me.TextBoxCaptionFindeAndReplace.Size = New System.Drawing.Size(573, 20)
        Me.TextBoxCaptionFindeAndReplace.TabIndex = 7
        Me.TextBoxCaptionFindeAndReplace.Text = "Найденное вхождение в тексте:"
        Me.TextBoxCaptionFindeAndReplace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbReplaceResult
        '
        Me.tbReplaceResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbReplaceResult.ForeColor = System.Drawing.Color.Red
        Me.tbReplaceResult.Location = New System.Drawing.Point(0, 20)
        Me.tbReplaceResult.Multiline = True
        Me.tbReplaceResult.Name = "tbReplaceResult"
        Me.tbReplaceResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbReplaceResult.Size = New System.Drawing.Size(573, 62)
        Me.tbReplaceResult.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.tbReplaceResult, "Текст замены в найденном вхождении")
        Me.tbReplaceResult.WordWrap = False
        '
        'TextBoxCaptionReplaceResult
        '
        Me.TextBoxCaptionReplaceResult.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TextBoxCaptionReplaceResult.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBoxCaptionReplaceResult.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxCaptionReplaceResult.Name = "TextBoxCaptionReplaceResult"
        Me.TextBoxCaptionReplaceResult.ReadOnly = True
        Me.TextBoxCaptionReplaceResult.Size = New System.Drawing.Size(573, 20)
        Me.TextBoxCaptionReplaceResult.TabIndex = 7
        Me.TextBoxCaptionReplaceResult.Text = "Найденное вхождение будет заменено на:"
        Me.TextBoxCaptionReplaceResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tpLog
        '
        Me.tpLog.Controls.Add(Me.tbLog)
        Me.tpLog.Controls.Add(Me.ToolStrip1)
        Me.tpLog.Location = New System.Drawing.Point(4, 22)
        Me.tpLog.Name = "tpLog"
        Me.tpLog.Size = New System.Drawing.Size(617, 362)
        Me.tpLog.TabIndex = 1
        Me.tpLog.Text = "Журнал работы"
        '
        'tbLog
        '
        Me.tbLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbLog.ForeColor = System.Drawing.Color.DarkGreen
        Me.tbLog.Location = New System.Drawing.Point(0, 25)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(617, 337)
        Me.tbLog.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.tbLog, "Журнал работы")
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbbClearLog, Me.tbbSaveLog})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(617, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tbbClearLog
        '
        Me.tbbClearLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbClearLog.Image = CType(resources.GetObject("tbbClearLog.Image"), System.Drawing.Image)
        Me.tbbClearLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbClearLog.Name = "tbbClearLog"
        Me.tbbClearLog.Size = New System.Drawing.Size(23, 22)
        Me.tbbClearLog.Tag = "ClearLog"
        Me.tbbClearLog.Text = "ClearLog"
        Me.tbbClearLog.ToolTipText = "Очистить историю"
        '
        'tbbSaveLog
        '
        Me.tbbSaveLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbSaveLog.Image = CType(resources.GetObject("tbbSaveLog.Image"), System.Drawing.Image)
        Me.tbbSaveLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbSaveLog.Name = "tbbSaveLog"
        Me.tbbSaveLog.Size = New System.Drawing.Size(23, 22)
        Me.tbbSaveLog.Tag = "SaveLog"
        Me.tbbSaveLog.Text = "SaveLog"
        Me.tbbSaveLog.ToolTipText = "Сохранить историю"
        '
        'statusBar1
        '
        Me.statusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sbpMain, Me.sbpFileCount})
        Me.statusBar1.Location = New System.Drawing.Point(0, 369)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(624, 24)
        Me.statusBar1.TabIndex = 0
        Me.statusBar1.Text = "StatusStrip1"
        '
        'sbpMain
        '
        Me.sbpMain.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sbpMain.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.sbpMain.Name = "sbpMain"
        Me.sbpMain.Size = New System.Drawing.Size(593, 19)
        Me.sbpMain.Spring = True
        Me.sbpMain.Text = "Файл не выделен..."
        Me.sbpMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sbpMain.ToolTipText = "Текущий файл/папка"
        '
        'sbpFileCount
        '
        Me.sbpFileCount.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.sbpFileCount.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me.sbpFileCount.Name = "sbpFileCount"
        Me.sbpFileCount.Size = New System.Drawing.Size(17, 19)
        Me.sbpFileCount.Text = "0"
        Me.sbpFileCount.ToolTipText = "Колличество обработанных"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.RunToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(624, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miNew, Me.miOpen, Me.miSave, Me.miSaveAs, Me.ToolStripSeparator1, Me.miBrowseForFolder, Me.ToolStripSeparator2, Me.miRecent, Me.ToolStripSeparator3, Me.miFileExit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.FileToolStripMenuItem.Text = "&Файл"
        '
        'miNew
        '
        Me.miNew.Image = CType(resources.GetObject("miNew.Image"), System.Drawing.Image)
        Me.miNew.Name = "miNew"
        Me.miNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.miNew.Size = New System.Drawing.Size(320, 22)
        Me.miNew.Text = "&Новые настройки"
        Me.miNew.ToolTipText = "Создать новый конфигурационный файл"
        '
        'miOpen
        '
        Me.miOpen.Image = CType(resources.GetObject("miOpen.Image"), System.Drawing.Image)
        Me.miOpen.Name = "miOpen"
        Me.miOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.miOpen.Size = New System.Drawing.Size(320, 22)
        Me.miOpen.Text = "&Открыть файл настроек..."
        Me.miOpen.ToolTipText = "Открыть имеющийся конфигурационный файл"
        '
        'miSave
        '
        Me.miSave.Image = CType(resources.GetObject("miSave.Image"), System.Drawing.Image)
        Me.miSave.Name = "miSave"
        Me.miSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.miSave.Size = New System.Drawing.Size(320, 22)
        Me.miSave.Text = "&Сохранить файл настроек"
        Me.miSave.ToolTipText = "Записать текущие настройки в конфигурационном файле"
        '
        'miSaveAs
        '
        Me.miSaveAs.Name = "miSaveAs"
        Me.miSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.miSaveAs.Size = New System.Drawing.Size(320, 22)
        Me.miSaveAs.Text = "Сохранить файл настроек &Как..."
        Me.miSaveAs.ToolTipText = "Записать текущие настройки в новом конфигурационном файле"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(317, 6)
        '
        'miBrowseForFolder
        '
        Me.miBrowseForFolder.Image = CType(resources.GetObject("miBrowseForFolder.Image"), System.Drawing.Image)
        Me.miBrowseForFolder.Name = "miBrowseForFolder"
        Me.miBrowseForFolder.Size = New System.Drawing.Size(320, 22)
        Me.miBrowseForFolder.Text = "&Изменить папку ""Функциональные Панели"""
        Me.miBrowseForFolder.ToolTipText = "Изменить папку ""Функциональные Панели"""
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(317, 6)
        '
        'miRecent
        '
        Me.miRecent.Image = CType(resources.GetObject("miRecent.Image"), System.Drawing.Image)
        Me.miRecent.Name = "miRecent"
        Me.miRecent.Size = New System.Drawing.Size(320, 22)
        Me.miRecent.Text = "&Последние файлы"
        Me.miRecent.ToolTipText = "Открыть имеющийся конфигурационный файл"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(317, 6)
        '
        'miFileExit
        '
        Me.miFileExit.Name = "miFileExit"
        Me.miFileExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.miFileExit.Size = New System.Drawing.Size(320, 22)
        Me.miFileExit.Text = "&Выход"
        Me.miFileExit.ToolTipText = "Закрыть окно"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EscapesSelectedTextToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.EditToolStripMenuItem.Text = "&Правка"
        '
        'EscapesSelectedTextToolStripMenuItem
        '
        Me.EscapesSelectedTextToolStripMenuItem.Image = CType(resources.GetObject("EscapesSelectedTextToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EscapesSelectedTextToolStripMenuItem.Name = "EscapesSelectedTextToolStripMenuItem"
        Me.EscapesSelectedTextToolStripMenuItem.Size = New System.Drawing.Size(307, 22)
        Me.EscapesSelectedTextToolStripMenuItem.Text = "&Заменить метасимволы их escape-кодами"
        Me.EscapesSelectedTextToolStripMenuItem.ToolTipText = "Заменить метасимволы их escape-кодами в выделенном тексте ""Для поиска"""
        '
        'RunToolStripMenuItem
        '
        Me.RunToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miRunGo})
        Me.RunToolStripMenuItem.Name = "RunToolStripMenuItem"
        Me.RunToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.RunToolStripMenuItem.Text = "&Пуск"
        '
        'miRunGo
        '
        Me.miRunGo.Image = CType(resources.GetObject("miRunGo.Image"), System.Drawing.Image)
        Me.miRunGo.Name = "miRunGo"
        Me.miRunGo.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.miRunGo.Size = New System.Drawing.Size(190, 22)
        Me.miRunGo.Text = "&Запустить замену"
        Me.miRunGo.ToolTipText = "Запустить пакетную замену текста во всех файлах папки ""Функциональные панели"""
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.HelpToolStripMenuItem.Text = "&Справка"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Image = CType(resources.GetObject("AboutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.AboutToolStripMenuItem.Text = "&О программе"
        Me.AboutToolStripMenuItem.ToolTipText = "О программе ""Регулярные выражения"""
        '
        'toolBar1
        '
        Me.toolBar1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.toolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.toolBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbbNew, Me.ToolStripSeparator4, Me.tbbOpen, Me.tbbSave, Me.ToolStripSeparator5, Me.tbbReplaceInFile, Me.ToolStripSeparator6, Me.tbbEsc})
        Me.toolBar1.Location = New System.Drawing.Point(3, 24)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.Size = New System.Drawing.Size(154, 25)
        Me.toolBar1.TabIndex = 2
        '
        'tbbNew
        '
        Me.tbbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbNew.Image = CType(resources.GetObject("tbbNew.Image"), System.Drawing.Image)
        Me.tbbNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbNew.Name = "tbbNew"
        Me.tbbNew.Size = New System.Drawing.Size(23, 22)
        Me.tbbNew.Tag = "New"
        Me.tbbNew.ToolTipText = "Создать новый конфигурационный файл"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tbbOpen
        '
        Me.tbbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbOpen.Image = CType(resources.GetObject("tbbOpen.Image"), System.Drawing.Image)
        Me.tbbOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbOpen.Name = "tbbOpen"
        Me.tbbOpen.Size = New System.Drawing.Size(32, 22)
        Me.tbbOpen.Tag = "Open"
        Me.tbbOpen.Text = "ToolStripSplitButton1"
        Me.tbbOpen.ToolTipText = "Открыть имеющийся конфигурационный файл"
        '
        'tbbSave
        '
        Me.tbbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbSave.Image = CType(resources.GetObject("tbbSave.Image"), System.Drawing.Image)
        Me.tbbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbSave.Name = "tbbSave"
        Me.tbbSave.Size = New System.Drawing.Size(23, 22)
        Me.tbbSave.Tag = "Save"
        Me.tbbSave.ToolTipText = "Записать текущие настройки в конфигурационном файле"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'tbbReplaceInFile
        '
        Me.tbbReplaceInFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbReplaceInFile.Image = CType(resources.GetObject("tbbReplaceInFile.Image"), System.Drawing.Image)
        Me.tbbReplaceInFile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbReplaceInFile.Name = "tbbReplaceInFile"
        Me.tbbReplaceInFile.Size = New System.Drawing.Size(23, 22)
        Me.tbbReplaceInFile.Tag = "ReplaceInFile"
        Me.tbbReplaceInFile.ToolTipText = "Запустить пакетную замену текста во всех файлах папки ""Функциональные панели"""
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'tbbEsc
        '
        Me.tbbEsc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbbEsc.Image = CType(resources.GetObject("tbbEsc.Image"), System.Drawing.Image)
        Me.tbbEsc.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbbEsc.Name = "tbbEsc"
        Me.tbbEsc.Size = New System.Drawing.Size(23, 22)
        Me.tbbEsc.Tag = "Esc"
        Me.tbbEsc.ToolTipText = "Заменить метасимволы их escape-кодами в выделенном тексте ""Для поиска"""
        '
        'OpenFile
        '
        Me.OpenFile.DefaultExt = "shr"
        Me.OpenFile.Filter = "Asc Search|*.shr"
        Me.OpenFile.Title = "Выберите файл с настройками"
        '
        'SaveFile
        '
        Me.SaveFile.DefaultExt = "shr"
        Me.SaveFile.FileName = "config1"
        Me.SaveFile.Filter = "Asc Search|*.shr"
        Me.SaveFile.Title = "Выберите или введите имя файла в который будут сохранены настройки"
        '
        'SaveLog
        '
        Me.SaveLog.DefaultExt = "txt"
        Me.SaveLog.FileName = "log"
        Me.SaveLog.Filter = "Asc Search|*.txt"
        Me.SaveLog.Title = "Выберите или введите имя файла в который будут сохранен Log"
        '
        'frmRegularExpressionsReplace
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "frmRegularExpressionsReplace"
        Me.Text = "Регулярные выражения"
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ContentPanel.PerformLayout()
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.tabControl1.ResumeLayout(False)
        Me.tpSetings.ResumeLayout(False)
        Me.tpSetings.PerformLayout()
        Me.panel1.ResumeLayout(False)
        Me.SplitContainerSetting.Panel1.ResumeLayout(False)
        Me.SplitContainerSetting.Panel1.PerformLayout()
        Me.SplitContainerSetting.Panel2.ResumeLayout(False)
        Me.SplitContainerSetting.Panel2.PerformLayout()
        CType(Me.SplitContainerSetting, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerSetting.ResumeLayout(False)
        Me.gbRegExpOptions.ResumeLayout(False)
        Me.gbRegExpOptions.PerformLayout()
        Me.tpPreview.ResumeLayout(False)
        Me.SplitContainerPrivewVert.Panel1.ResumeLayout(False)
        Me.SplitContainerPrivewVert.Panel1.PerformLayout()
        Me.SplitContainerPrivewVert.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerPrivewVert, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerPrivewVert.ResumeLayout(False)
        Me.SplitContainerPreviewHor.Panel1.ResumeLayout(False)
        Me.SplitContainerPreviewHor.Panel1.PerformLayout()
        Me.SplitContainerPreviewHor.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerPreviewHor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerPreviewHor.ResumeLayout(False)
        Me.tabPreview.ResumeLayout(False)
        Me.tabPgFindeOnly.ResumeLayout(False)
        Me.tabPgFindeOnly.PerformLayout()
        Me.tabPgFindeAndReplace.ResumeLayout(False)
        Me.SplitContainerFindReplace.Panel1.ResumeLayout(False)
        Me.SplitContainerFindReplace.Panel1.PerformLayout()
        Me.SplitContainerFindReplace.Panel2.ResumeLayout(False)
        Me.SplitContainerFindReplace.Panel2.PerformLayout()
        CType(Me.SplitContainerFindReplace, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerFindReplace.ResumeLayout(False)
        Me.tpLog.ResumeLayout(False)
        Me.tpLog.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.statusBar1.ResumeLayout(False)
        Me.statusBar1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.toolBar1.ResumeLayout(False)
        Me.toolBar1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusStrip
    Friend WithEvents sbpMain As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sbpFileCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents toolBar1 As System.Windows.Forms.ToolStrip
    Private WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
    Private WithEvents SaveFile As System.Windows.Forms.SaveFileDialog
    Private WithEvents SaveLog As System.Windows.Forms.SaveFileDialog
    Private WithEvents imageList2 As System.Windows.Forms.ImageList
    Private WithEvents tabControl1 As System.Windows.Forms.TabControl
    Private WithEvents tpSetings As System.Windows.Forms.TabPage
    Private WithEvents tbExtensions As System.Windows.Forms.TextBox
    Private WithEvents tbPath As System.Windows.Forms.TextBox
    Private WithEvents pbBroseFolder As System.Windows.Forms.Button
    Private WithEvents lblFilesPatern As System.Windows.Forms.Label
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents tbReplaceTo As System.Windows.Forms.TextBox
    Private WithEvents tbFind As System.Windows.Forms.TextBox
    Private WithEvents lblPath As System.Windows.Forms.Label
    Private WithEvents gbRegExpOptions As System.Windows.Forms.GroupBox
    Private WithEvents tbReplCount As System.Windows.Forms.TextBox
    Private WithEvents cbReplAll As System.Windows.Forms.CheckBox
    Private WithEvents cbIgnoreCase As System.Windows.Forms.CheckBox
    Private WithEvents cbMultiline As System.Windows.Forms.CheckBox
    Private WithEvents cbExplicitCapture As System.Windows.Forms.CheckBox
    Private WithEvents cbSingleline As System.Windows.Forms.CheckBox
    Private WithEvents cbIgnorePatternWhitespace As System.Windows.Forms.CheckBox
    Private WithEvents cbCompiled As System.Windows.Forms.CheckBox
    Private WithEvents tpPreview As System.Windows.Forms.TabPage
    Private WithEvents tabPreview As System.Windows.Forms.TabControl
    Private WithEvents tabPgFindeOnly As System.Windows.Forms.TabPage
    Private WithEvents tbFileBody As System.Windows.Forms.TextBox
    Private WithEvents tabPgFindeAndReplace As System.Windows.Forms.TabPage
    Private WithEvents pnFindeAndReplace As System.Windows.Forms.Panel
    Private WithEvents tbReplaceResult As System.Windows.Forms.TextBox
    Private WithEvents lbFileMatch As System.Windows.Forms.ListBox
    Private WithEvents tvFiles As System.Windows.Forms.TreeView
    Private WithEvents tpLog As System.Windows.Forms.TabPage
    Private WithEvents tbLog As System.Windows.Forms.TextBox
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miBrowseForFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miRecent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EscapesSelectedTextToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miRunGo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tbbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbbReplaceInFile As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbbEsc As System.Windows.Forms.ToolStripButton
    Friend WithEvents BrowseFolderDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents tbbOpen As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents SplitContainerSetting As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerFindReplace As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tbbClearLog As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbbSaveLog As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainerPrivewVert As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerPreviewHor As System.Windows.Forms.SplitContainer
    Friend WithEvents TextBoxCaptionToSearch As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionToReplace As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionTree As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionFileMath As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionFileBody As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionFindeAndReplace As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxCaptionReplaceResult As System.Windows.Forms.TextBox

End Class
