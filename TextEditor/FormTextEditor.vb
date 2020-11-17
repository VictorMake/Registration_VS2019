Imports VB = Microsoft.VisualBasic
Imports System.Drawing.Text

''' <summary>
''' Основная форма для SDI Записной книжки
''' </summary>
''' <remarks></remarks>
Friend Class FormTextEditor
    Private Class FontSizes
        Public Shared Small As Single = 8.0F
        Public Shared Medium As Single = 12.0F
        Public Shared Large As Single = 24.0F
    End Class

    Private fontSize As Single = FontSizes.Medium

    Private miMainFormatFontChecked As ToolStripMenuItem
    Private miMainFormatSizeChecked As ToolStripMenuItem
    Private miContextFormatFontChecked As ToolStripMenuItem
    Private miContextFormatSizeChecked As ToolStripMenuItem

    Private currentFontFamily As FontFamily
    Private monoSpaceFontFamily As FontFamily
    Private sansSerifFontFamily As FontFamily
    Private serifFontFamily As FontFamily

    Private isFormLoaded As Boolean = False
    Public mnuRecentFile(5) As ToolStripMenuItem
    Private frmFindAndReplace As New FormFindAndReplace()

    Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        MdiParent = MainMdiForm
        monoSpaceFontFamily = New FontFamily(GenericFontFamilies.Monospace)
        sansSerifFontFamily = New FontFamily(GenericFontFamilies.SansSerif)
        serifFontFamily = New FontFamily(GenericFontFamilies.Serif)
        currentFontFamily = sansSerifFontFamily

        ' Связать меню и контекстное меню
        miMainFormatFontChecked = mmiSansSerif
        miMainFormatSizeChecked = mmiMedium
        miContextFormatFontChecked = cmiSansSerif
        miContextFormatSizeChecked = cmiMedium
    End Sub

    Private Sub FormTextEditor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Установить рабочий каталог в каталог, содержащий приложение.
        'ChDir(VB6.GetPath)
        FileIO.FileSystem.CurrentDirectory = IO.Path.Combine(IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), RootDirectory) ' Ресурсы

        ' Читать Системный  реестр, и установите недавний массив контроля списков файлов меню соответственно.
        '    GetRecentFiles
        FindDirection = 1

        ' Добавить набор шрифтов в меню
        'Dim ff As FontFamily
        'Dim I As Short 
        'I = 1
        'For Each ff In System.Drawing.FontFamily.Families
        '    mnuFontName.Load(I)
        '    mnuFontName(I).Text = ff.Name
        '    I += 1
        'Next

        txtNote.Text = HelpString
        FrmState.Dirty = False

        mnuRecentFile(0) = mnuRecentFile0
        mnuRecentFile(1) = mnuRecentFile1
        mnuRecentFile(2) = mnuRecentFile2
        mnuRecentFile(3) = mnuRecentFile3
        mnuRecentFile(4) = mnuRecentFile4
        mnuRecentFile(5) = mnuRecentFile5
        isFormLoaded = True

        'ResizeNote() 
        '    txtNote.Rtf = "{\rtf1\ansi \b me\b0  is a \ul RichTextBox\ul0. \i Try\i0  some \b formatting\b0!  Also try the right-click Context Menu.}"
        ' по умолчанию прозрачность формы установлена в 100%, таким образом эта опция отмечена.
        OnehundredPercent.Checked = True
    End Sub

    'Private Sub FormTextEditor_Closed(ByVal eventSender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    '    ' Вызвать недавнюю процедуру списка файла
    '    '    GetRecentFiles
    'End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        EditCopyProc()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        EditCutProc()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        EditPasteProc()
    End Sub

    Public Sub mnuEditDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditDelete.Click
        ' если указатель не в конце записной книжки ...
        If txtNote.SelectionStart <> Len(txtNote.Text) Then
            ' если ничто не выделено, расширять выбор на один.
            If txtNote.SelectionLength = 0 Then
                txtNote.SelectionLength = 1
                ' если указатель находится на пустой строке, расширять выбор на два.
                If Asc(txtNote.SelectedText) = 13 Then
                    txtNote.SelectionLength = 2
                End If
            End If
            ' Удалить выделенный текст.
            txtNote.SelectedText = "" 'vbNullString
        End If
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        ' Использовать SelStart и SelLength, чтобы выбрать текст.
        txtNote.SelectionStart = 0
        txtNote.SelectionLength = Len(txtNote.Text)
    End Sub

    Public Sub mnuFileSaveAs()
        Dim saveFileName As String
        Dim defaultName As String

        If Text = "Блокнот" Then
            ' Получите имя файла, и затем вызвать strSaveFileName.
            saveFileName = GetFileName("Untitled.txt")
            If saveFileName <> "" Then SaveFileAs((saveFileName))
            ' Модифицировать список недавно открытых файлов в массиве контрола меню File.
            UpdateFileMenu((saveFileName))
        Else
            ' Назначить заголовок формы на переменную.
            defaultName = VB.Right(Text, Len(Text) - 14) ' - 11)
            ' Заголовок формы содержит название открытого файла.
            saveFileName = GetFileName(defaultName)
            If saveFileName <> "" Then SaveFileAs((saveFileName))
            ' Модифицировать список недавно открытых файлов в массиве контрола меню File.
            UpdateFileMenu((saveFileName))
        End If
    End Sub

    Public Sub OpenRecentFile(ByVal index As Integer)
        ' открыть недавний файл
        OpenFile((mnuRecentFile(index).Text))
        ' Модифицировать список недавно открытых файлов в массиве контрола меню File.
        '    GetRecentFiles
    End Sub

    Private Sub mnuRecentFile0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile0.Click
        OpenRecentFile(0)
    End Sub
    Private Sub mnuRecentFile1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile1.Click
        OpenRecentFile(1)
    End Sub
    Private Sub mnuRecentFile2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile2.Click
        OpenRecentFile(2)
    End Sub
    Private Sub mnuRecentFile3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile3.Click
        OpenRecentFile(3)
    End Sub
    Private Sub mnuRecentFile4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile4.Click
        OpenRecentFile(4)
    End Sub
    Private Sub mnuRecentFile5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRecentFile5.Click
        OpenRecentFile(5)
    End Sub

    Public Sub mnuSearchFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSearchFind.Click
        ' если имеется текст в textbox, то он используется в форме поиска
        ' иначе используется последнее значение поиска текста.
        Dim mfrmFind As FormFind = New FormFind()

        If txtNote.SelectedText <> "" Then
            mfrmFind.txtFind.Text = txtNote.SelectedText
        Else
            mfrmFind.txtFind.Text = FindString
        End If

        ' установить глобальную переменную на поиск с началае.
        IsFirstTime = True
        ' применить переключатель регистра из глобальной переменой
        If (FindCase) Then
            mfrmFind.chkCase.CheckState = CheckState.Checked
        End If

        ' показать не модальный диалог.
        mfrmFind.Show(Me)
        mfrmFind.Activate()
    End Sub

    Public Sub mnuSearchFindNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSearchFindNext.Click
        ' если глобальная переменная не пуста, вызвать метод
        ' иначе вызвать подходящее меню
        If Len(FindString) > 0 Then
            FindIt()
        Else
            mnuSearchFind_Click(mnuSearchFind, New EventArgs())
        End If
    End Sub

    ' другой обработчик поиска с помощью сочетаний клавиш
    Private Sub txtNote_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtNote.KeyUp
        If e.Control AndAlso e.KeyCode = Keys.F Then
            FindAndReplaceDialog()
        End If
    End Sub

    Private Sub ЗаменаToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ЗаменаToolStripMenuItem.Click
        FindAndReplaceDialog()
    End Sub

    Private Sub FindAndReplaceDialog()
        frmFindAndReplace.SetTarget(txtNote.Text)
        frmFindAndReplace.txtFind.Focus()
        frmFindAndReplace.ShowDialog()

        ' после e.Cancel = True обработка If FindAndReplace.ShowDialog = Windows.Forms.DialogResult.OK Then не работает
        'If FindAndReplace.ShowDialog = Windows.Forms.DialogResult.OK Then
        txtNote.Text = frmFindAndReplace.txtTarget.Text
        'End If
    End Sub

    Public Sub mnuWindowCascade_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        MainMdiForm.MenuWindowCascade_Click(eventSender, eventArgs)
    End Sub

    Public Sub mnuWindowTileHorizontal_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        MainMdiForm.MenuWindowTileHorizontal_Click(eventSender, eventArgs)
    End Sub

    Public Sub mnuWindowTileVertical_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        MainMdiForm.MenuWindowTileVertical_Click(eventSender, eventArgs)
    End Sub

    Private Sub txtNote_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNote.TextChanged
        ' текст изменился.
        FrmState.Dirty = True
    End Sub

    Private Sub FormatFont_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles mmiSansSerif.Click,
                                                                                        mmiSerif.Click,
                                                                                        mmiMonoSpace.Click,
                                                                                        cmiSansSerif.Click,
                                                                                        cmiSerif.Click,
                                                                                        cmiMonoSpace.Click
        Dim miClicked As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        miMainFormatFontChecked.Checked = False
        miContextFormatFontChecked.Checked = False

        If miClicked Is mmiSansSerif OrElse miClicked Is cmiSansSerif Then
            miMainFormatFontChecked = mmiSansSerif
            miContextFormatFontChecked = cmiSansSerif
            currentFontFamily = sansSerifFontFamily

            miMainFormatFontChecked = mmiSansSerif
            miContextFormatFontChecked = cmiSansSerif
        ElseIf miClicked Is mmiSerif OrElse miClicked Is cmiSerif Then
            miMainFormatFontChecked = mmiSerif
            miContextFormatFontChecked = cmiSerif
            currentFontFamily = serifFontFamily

            miMainFormatFontChecked = mmiSerif
            miContextFormatFontChecked = cmiSerif
        Else
            miMainFormatFontChecked = mmiMonoSpace
            miContextFormatFontChecked = cmiMonoSpace
            currentFontFamily = monoSpaceFontFamily

            miMainFormatFontChecked = mmiMonoSpace
            miContextFormatFontChecked = cmiMonoSpace
        End If

        miMainFormatFontChecked.Checked = True
        miContextFormatFontChecked.Checked = True
        txtNote.Font = New Font(currentFontFamily, fontSize)
    End Sub

    'Format->Size Menu item handler
    Private Sub FormatSize_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles mmiSmall.Click,
                                                                                        mmiMedium.Click,
                                                                                        mmiLarge.Click,
                                                                                        cmiSmall.Click,
                                                                                        cmiMedium.Click,
                                                                                        cmiLarge.Click
        Dim miClicked As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        miMainFormatSizeChecked.Checked = False
        miContextFormatSizeChecked.Checked = False

        Dim fontSizeString As String = miClicked.Text

        If (fontSizeString = "&Маленький") Then
            miMainFormatSizeChecked = mmiSmall
            miContextFormatSizeChecked = cmiSmall
            fontSize = FontSizes.Small
        ElseIf (fontSizeString = "&Большой") Then
            miMainFormatSizeChecked = mmiLarge
            miContextFormatSizeChecked = cmiLarge
            fontSize = FontSizes.Large
        Else
            miMainFormatSizeChecked = mmiMedium
            miContextFormatSizeChecked = cmiMedium
            fontSize = FontSizes.Medium
        End If

        miMainFormatSizeChecked.Checked = True
        miContextFormatSizeChecked.Checked = True

        txtNote.Font = New Font(currentFontFamily, fontSize)
    End Sub

    Private Sub fileMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click,
                                                                                        SaveToolStripMenuItem.Click,
                                                                                        SaveAsToolStripMenuItem.Click,
                                                                                        OpenToolStripMenuItem.Click,
                                                                                        ExitToolStripMenuItem.Click
        manipulateFile(CType(sender, ToolStripMenuItem).Text)
    End Sub

    Private Sub FoldersToolStripButton_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs) Handles FoldersToolStripButton.ItemClicked
        manipulateFile(CType(e.ClickedItem, ToolStripItem).Text)
    End Sub

    Private Sub formatMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles boldToolStripMenuItem.Click,
                                                                                        underlineToolStripMenuItem.Click,
                                                                                        italicsToolStripMenuItem.Click,
                                                                                        formatBoldContextMenuItem.Click,
                                                                                        formatItalicsContextMenuItem.Click,
                                                                                        formatUnderlineContextMenuItem.Click
        formatText(CType(sender, ToolStripItem).Text)
    End Sub
    Private Sub formatToolStrip_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs) Handles formatToolStrip.ItemClicked
        formatText(CType(e.ClickedItem, ToolStripItem).Text)
    End Sub

    Private Sub changeOpacityToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TwentyfivePercent.Click,
                                                                                                        OnehundredPercent.Click,
                                                                                                        seventyfivePercent.Click,
                                                                                                        fiftyPercent.Click
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim opacity As Double = Convert.ToDouble(menuItem.Tag.ToString())
        Me.Opacity = opacity
        ' настройки прозрачности взаимоисключающие. Установка только отмеченного пункта 
        Dim menuChangeOpacity As ToolStripMenuItem = CType(OptionsToolStripMenuItem.DropDownItems("changeOpacityToolStripMenuItem"), ToolStripMenuItem)

        For Each item As ToolStripMenuItem In menuChangeOpacity.DropDownItems
            item.Checked = False
        Next

        menuItem.Checked = True
    End Sub

    ''' <summary>
    ''' метод обработчика операций форматирования
    ''' для menu strip, tool bar, и context menu.
    ''' </summary>
    ''' <param name="menuItemText"></param>
    ''' <remarks></remarks>
    Private Sub formatText(ByVal menuItemText As String)
        Dim fontOfSelectedText As Font = txtNote.SelectionFont
        Dim styleApplied As FontStyle

        Select Case (menuItemText)
            Case "&Bold"
                If txtNote.SelectionFont.Bold = True Then
                    styleApplied = FontStyle.Regular
                Else
                    styleApplied = FontStyle.Bold
                End If

                Exit Select
            Case "&Italics"
                If txtNote.SelectionFont.Italic = True Then
                    styleApplied = FontStyle.Regular
                Else
                    styleApplied = FontStyle.Italic
                End If

                Exit Select
            Case Else
                If txtNote.SelectionFont.Underline = True Then
                    styleApplied = FontStyle.Regular
                Else
                    styleApplied = FontStyle.Underline
                End If
        End Select

        Dim FontToApply As New Font(fontOfSelectedText, styleApplied)
        txtNote.SelectionFont = FontToApply
    End Sub

    ''' <summary>
    ''' Метод обработчика файловых манипуляций для
    ''' File menu strip и tool bar
    ''' </summary>
    ''' <param name="menuItemText"></param>
    ''' <remarks></remarks>
    Private Sub manipulateFile(ByVal menuItemText As String)
        Select Case menuItemText
            Case "&Новый"
                ' произвести создание нового документа 
                FileNew()
                txtNote.Focus()
                Exit Select
            Case "&Открыть"
                FileOpenProc()
                'If Me.openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    MessageBox.Show("Указанный  " + _
                '        vbCrLf + "the " + Me.openFileDialog.FileName + " файл не существует.", "Sample Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'End If

                'Private Sub OpenToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
                '    Dim OpenFileDialog As New OpenFileDialog
                '    OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                '    OpenFileDialog.Filter = "Text Files (*.txt)|*.txt"
                '    OpenFileDialog.ShowDialog(Me)

                '    Dim FileName As String = OpenFileDialog.FileName
                '    ' добавить код открытия файла
                'End Sub

                Exit Select
            Case "&Сохранить"
                'If Me.saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    MessageBox.Show("Для файла " + _
                '        vbCrLf + "the " + Me.saveFileDialog.FileName + " файл не сохранен.", "Sample Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'End If
                FileSave()
                Exit Select
                'Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
                '    Dim SaveFileDialog As New SaveFileDialog
                '    SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                '    SaveFileDialog.Filter = "Text Files (*.txt)|*.txt"
                '    SaveFileDialog.ShowDialog(Me)

                '    Dim FileName As String = SaveFileDialog.FileName
                '    ' добавить код для сохранения текущего содержимого в файле.
                'End Sub
            Case "Сохранить &Как"
                mnuFileSaveAs()
            Case "&Закрыть окно"
                ' Закрыть форму.
                Close()
                Dispose()
            Case Else
        End Select
    End Sub

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        ' переключатель видимости toolstrip и установка чека связанного меню
        ToolBarToolStripMenuItem.Checked = Not ToolBarToolStripMenuItem.Checked
        formatToolStrip.Visible = ToolBarToolStripMenuItem.Checked
        FoldersToolStripButton.Visible = ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        ' переключатель видимости statusstrip и установка чека связанного меню
        StatusBarToolStripMenuItem.Checked = Not StatusBarToolStripMenuItem.Checked
        StatusStrip.Visible = StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PrintToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub UndoToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles UndoToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RedoToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ContentsToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub IndexToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles IndexToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub SearchToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SearchToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AboutToolStripMenuItem.Click
        ToDo()
    End Sub

    Private Sub ToDo()
        MessageBox.Show("Функция будет доделана позже.", "Обработка меню", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#Region "Недоделан"

    'Private Sub imgFileNewButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles imgFileNewButton.Click
    '    ' Обновить изображение.
    '    imgFileNewButton.Refresh()
    '    ' Вызвать процедуру создания файла
    '    FileNew()
    'End Sub
    'Public Sub mnuFileNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileNew.Click
    '    ' Вызвать новую процедуру формы
    '    FileNew()
    'End Sub

    'Private Sub imgFileOpenButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles imgFileOpenButton.Click
    '    ' Обновить изображение.
    '    imgFileOpenButton.Refresh()
    '    ' Вызвать процедурой открытия файла 
    '    FileOpenProc()
    'End Sub
    'Public Sub mnuFileOpen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileOpen.Click
    '    ' Вызвать процедурой открытия файла
    '    FileOpenProc()
    'End Sub

    'Private Sub imgPasteButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles imgPasteButton.Click
    '    ' Обновить образ.
    '    imgPasteButton.Refresh()
    '    ' Вызвать процедуру вставки
    '    EditPasteProc()
    'End Sub
    'Public Sub mnuEditPaste_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEditPaste.Click
    '    ' Вызвать процедуру вставки.
    '    EditPasteProc()
    'End Sub

    'Public Sub mnuEditCopy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEditCopy.Click
    '    ' Вызвать процедуру копирования
    '    EditCopyProc()
    'End Sub

    'Public Sub mnuEditCut_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEditCut.Click
    '    ' Вызвать процедуру вырезки
    '    EditCutProc()
    'End Sub

    'Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSave.Click
    '    ' Вызвать процедуру сохранения файл
    '    FileSave()
    'End Sub

    'Public Sub mnuEditTime_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuEditTime.Click
    '    ' Вставьте текущее время и дату.
    '    txtNote.SelectedText = CStr(Now)
    'End Sub

    'Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileExit.Click
    '    ' Завершить приложение.
    '    Me.Close()
    'End Sub

    'Private Sub imgCutButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles imgCutButton.Click
    '    ' Обновить изображение.
    '    imgCutButton.Refresh()
    '    ' Вызвать процедуру вырезки
    '    EditCutProc()
    'End Sub
    'Private Sub imgCopyButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles imgCopyButton.Click
    '    ' Обновить изображение.
    '    imgCopyButton.Refresh()
    '    ' Вызвать процедуру копии
    '    EditCopyProc()
    'End Sub

    'Public Sub mnuFontName_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
    '    Dim Index As Short = mnuFontName.GetIndex(eventSender)
    '    ' Применить отобранный шрифт на textbox fontname свойство.
    '    txtNote.Font = VB6.FontChangeName(txtNote.Font, mnuFontName(Index).Text)
    'End Sub

    'Public Sub mnuOptions_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuOptions.Click
    '    ' Переключить чек, чтобы соответствовать .Visible свойству.
    '    mnuOptionsToolbar.Checked = picToolbar.Visible
    'End Sub

    'Public Sub mnuOptionsToolbar_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuOptionsToolbar.Click
    '    ' Переключить видимое свойство инструментальной панели
    '    FoldersToolStripButton.Visible = Not FoldersToolStripButton.Visible
    '    ' Изменить проверку, чтобы соответствовать текущему состоянию
    '    mnuOptionsToolbar.Checked = FoldersToolStripButton.Visible
    '    formatToolStrip.Visible = mnuOptionsToolbar.Checked
    '    'ResizeNote()
    'End Sub

    '--- MouseDown -----------------------------------------------------------
    'Private Sub imgCutButton_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCutButton.MouseDown
    '    ' Показать изображение при при нажатии кнопки.
    '    imgCutButton.Image = imgCutButtonDn.Image
    'End Sub
    'Private Sub imgCopyButton_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCopyButton.MouseDown
    '    ' Показать изображение при при нажатии кнопки.
    '    imgCopyButton.Image = imgCopyButtonDn.Image
    'End Sub
    'Private Sub imgFileNewButton_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileNewButton.MouseDown
    '    ' Показать изображение при при нажатии кнопки.
    '    imgFileNewButton.Image = imgFileNewButtonDn.Image
    'End Sub
    'Private Sub imgFileOpenButton_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileOpenButton.MouseDown
    '    ' Показать изображение при при нажатии кнопки.
    '    imgFileOpenButton.Image = imgFileOpenButtonDn.Image
    'End Sub
    'Private Sub imgPasteButton_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgPasteButton.MouseDown
    '    ' Показать изображение при при нажатии кнопки.
    '    imgPasteButton.Image = imgPasteButtonDn.Image
    'End Sub

    '--- MouseMove -----------------------------------------------------------
    'Private Sub imgCutButton_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCutButton.MouseMove
    '    Dim Button As Short = eventArgs.Button \ &H100000
    '    Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    ' если кнопка нажата, вывести точечный рисунок когда
    '    ' Мышь перемещают вне области кнопки; 
    '    ' иначе, отобразить точечный рисунок нажатия.
    '    Select Case Button
    '        Case 1
    '            If x <= 0 Or x > imgCutButton.Width Or y < 0 Or y > imgCutButton.Height Then
    '                imgCutButton.Image = imgCutButtonUp.Image
    '            Else
    '                imgCutButton.Image = imgCutButtonDn.Image
    '            End If
    '    End Select
    'End Sub
    'Private Sub imgCopyButton_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCopyButton.MouseMove
    '    Dim Button As Short = eventArgs.Button \ &H100000
    '    Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    ' если кнопка нажата, вывести точечный рисунок когда
    '    ' Мышь перемещают вне области кнопки; 
    '    ' иначе, отобразить точечный рисунок нажатия.
    '    Select Case Button
    '        Case 1
    '            If x <= 0 Or x > imgCopyButton.Width Or y < 0 Or y > imgCopyButton.Height Then
    '                imgCopyButton.Image = imgCopyButtonUp.Image
    '            Else
    '                imgCopyButton.Image = imgCopyButtonDn.Image
    '            End If
    '    End Select
    'End Sub
    'Private Sub imgFileNewButton_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileNewButton.MouseMove
    '    Dim Button As Short = eventArgs.Button \ &H100000
    '    Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    ' если кнопка нажата, вывести точечный рисунок когда
    '    ' Мышь перемещают вне области кнопки; 
    '    ' иначе, отобразить точечный рисунок нажатия.
    '    Select Case Button
    '        Case 1
    '            If x <= 0 Or x > imgFileNewButton.Width Or y < 0 Or y > imgFileNewButton.Height Then
    '                imgFileNewButton.Image = imgFileNewButtonUp.Image
    '            Else
    '                imgFileNewButton.Image = imgFileNewButtonDn.Image
    '            End If
    '    End Select
    'End Sub
    'Private Sub imgFileOpenButton_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileOpenButton.MouseMove
    '    Dim Button As Short = eventArgs.Button \ &H100000
    '    Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    ' если кнопка нажата, вывести точечный рисунок когда
    '    ' Мышь перемещают вне области кнопки; 
    '    ' иначе, отобразить точечный рисунок нажатия.
    '    Select Case Button
    '        Case 1
    '            If x <= 0 Or x > imgFileOpenButton.Width Or y < 0 Or y > imgFileOpenButton.Height Then
    '                imgFileOpenButton.Image = imgFileOpenButtonUp.Image
    '            Else
    '                imgFileOpenButton.Image = imgFileOpenButtonDn.Image
    '            End If
    '    End Select
    'End Sub
    'Private Sub imgPasteButton_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgPasteButton.MouseMove
    '    Dim Button As Short = eventArgs.Button \ &H100000
    '    Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    ' если кнопка нажата, вывести точечный рисунок когда
    '    ' Мышь перемещают вне области кнопки; 
    '    ' иначе, отобразить точечный рисунок нажатия.
    '    Select Case Button
    '        Case 1
    '            If x <= 0 Or x > imgPasteButton.Width Or y < 0 Or y > imgPasteButton.Height Then
    '                imgPasteButton.Image = imgPasteButtonUp.Image
    '            Else
    '                imgPasteButton.Image = imgPasteButtonDn.Image
    '            End If
    '    End Select
    'End Sub

    '--- MouseUp -------------------------------------------------------------
    'Private Sub imgCutButton_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCutButton.MouseUp
    '    ' Показать изображение для текущего состояния.
    '    imgCutButton.Image = imgCutButtonUp.Image
    'End Sub
    'Private Sub imgCopyButton_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgCopyButton.MouseUp
    '    ' Показать изображение для текущего состояния.
    '    imgCopyButton.Image = imgCopyButtonUp.Image
    'End Sub
    'Private Sub imgFileNewButton_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileNewButton.MouseUp
    '    ' Показать изображение для текущего состояния.
    '    imgFileNewButton.Image = imgFileNewButtonUp.Image
    'End Sub
    'Private Sub imgFileOpenButton_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgFileOpenButton.MouseUp
    '    ' Показать изображение для текущего состояния.
    '    imgFileOpenButton.Image = imgFileOpenButtonUp.Image
    'End Sub
    'Private Sub imgPasteButton_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles imgPasteButton.MouseUp
    '    ' Показать изображение для текущего состояния.
    '    imgPasteButton.Image = imgPasteButtonUp.Image
    'End Sub

#End Region

End Class
