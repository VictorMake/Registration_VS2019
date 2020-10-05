Imports System.IO

''' <summary>
''' Стандартный модуль с процедурами для работы с файлами.
''' Часть Записной книжки производит загрузку файла.
''' </summary>
''' <remarks></remarks>
Module ModuleTextEditor
    ' Определяемый пользователем тип, чтобы сохранить информацию относительно дочерних форм
    Structure FormState
        Dim Deleted As Boolean
        Dim Dirty As Boolean
        Dim Color As Integer
    End Structure

    Public FrmState As FormState                        ' Определяемые пользователем типы
    Public FindString As String                         ' текст поиска.
    Public FindCase As Boolean                          ' для чувствительного к регистру поиска
    Public FindDirection As Short                       ' (ключ) для определения направления поиска.
    Public CurPos As Integer                            ' расположение курсора.
    Public IsFirstTime As Boolean                       ' для позиции начала.

    Private Const thisApp As String = "MDINote"         ' Константа Приложения Системного реестра.
    Private Const thisKey As String = "Recent Files"    ' Константа Ключа регистрации.
    Private datobj As New DataObject()                  ' для Буфера обмена

    Sub FileOpenProc()
        Dim success As Boolean

        ' если файл изменился, сохранить его
        If FrmState.Dirty = True Then
            success = FileSave()
            If success = False Then Exit Sub
        End If

        TextEditorForm.OpenFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        TextEditorForm.OpenFileDialog1.FilterIndex = 1
        TextEditorForm.OpenFileDialog1.RestoreDirectory = True

        If TextEditorForm.OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Иначе, открыть файл с доступом чтения-записи...
            OpenFile((TextEditorForm.OpenFileDialog1.FileName))
            UpdateFileMenu((TextEditorForm.OpenFileDialog1.FileName))
        End If
    End Sub

    ''' <summary>
    ''' Отобразите диалоговое окно Save As, и возвратить имя файла.
    ''' Если пользователь выбирает Cancel, возвратить пустую строку.
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetFileName(ByVal FileName As String) As String
        TextEditorForm.SaveFileDialog1.FileName = FileName
        TextEditorForm.SaveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        TextEditorForm.SaveFileDialog1.FilterIndex = 2
        TextEditorForm.SaveFileDialog1.RestoreDirectory = True

        If TextEditorForm.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            Return TextEditorForm.SaveFileDialog1.FileName
        Else
            Return "Мой блокнот"
        End If
    End Function

    Function OnRecentFilesList(ByVal FileName As String) As Boolean
        For I As Integer = 1 To 4
            If TextEditorForm.mnuRecentFile(I).Text = FileName Then
                Return True
            End If
        Next

        Return False
    End Function

    Sub OpenFile(ByVal FileName As String)
        Dim fs As FileStream = Nothing
        Dim sr As StreamReader = Nothing

        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            fs = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            sr = New StreamReader(fs, System.Text.UnicodeEncoding.Default)
            TextEditorForm.txtNote.Text = sr.ReadToEnd
            TextEditorForm.Text = "Мой блокнот - " & UCase(FileName)
        Catch ex As Exception
            Const caption As String = "Открытие файла"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            sr.Close()
            fs.Close()
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try

        FrmState.Dirty = False
    End Sub

    Sub SaveFileAs(ByVal FileName As String)
        Dim fs As FileStream = New FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write)
        Dim myStream As StreamWriter = New StreamWriter(fs, System.Text.UnicodeEncoding.Default)

        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Try
            myStream.Write(TextEditorForm.txtNote.Text)
            TextEditorForm.Text = "Мой блокнот - " & FileName
            ' Сбросить флажок.
            FrmState.Dirty = False
        Catch ex As Exception
            Const caption As String = "SaveFileAs"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            If myStream IsNot Nothing Then
                ' после записи потока попали сюда.
                myStream.Close()
            End If
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub

    Sub UpdateFileMenu(ByVal FileName As String)
        ' Проверить, является ли открытое имя файла уже в массиве меню File.
        If Not OnRecentFilesList(FileName) Then
            ' Записать открытое имя файла в системный реестр.
            WriteRecentFiles((FileName))
        End If
        ' Модифицировать список недавно открытых файлов в массиве меню File.
        '    GetRecentFiles
    End Sub

    ''' <summary>
    ''' Копировать отобранный текст на Буфер обмена.
    ''' </summary>
    ''' <remarks></remarks>
    Sub EditCopyProc()
        datobj.SetData(DataFormats.Text, TextEditorForm.txtNote.SelectedText)
        Clipboard.SetDataObject(datobj)
    End Sub

    ''' <summary>
    ''' Копируйте отобранный текст на Буфер обмена.
    ''' </summary>
    ''' <remarks></remarks>
    Sub EditCutProc()
        datobj.SetData(DataFormats.Text, TextEditorForm.txtNote.SelectedText)
        Clipboard.SetDataObject(datobj)
        ' Удалите отобранный текст.
        TextEditorForm.txtNote.SelectedText = "" 'vbNullString
    End Sub

    ''' <summary>
    ''' Разместите текст от Буфера обмена в активный контроль.
    ''' </summary>
    ''' <remarks></remarks>
    Sub EditPasteProc()
        If Clipboard.GetDataObject.GetDataPresent(DataFormats.Text) Then
            TextEditorForm.txtNote.SelectedText = CType(Clipboard.GetDataObject.GetData(DataFormats.Text), String)
        End If
    End Sub

    ''' <summary>
    ''' если файл изменился, сохранить
    ''' </summary>
    ''' <remarks></remarks>
    Sub FileNew()
        Dim success As Boolean

        If FrmState.Dirty = True Then
            success = FileSave()
            If success = False Then Exit Sub
        End If

        ' Очистить textbox, и модифицировать заголовок.
        TextEditorForm.txtNote.Text = "" 'vbNullString
        TextEditorForm.Text = "Мой блокнот"
    End Sub

    Function FileSave() As Boolean
        Dim strFilename As String = Nothing

        If TextEditorForm.Text = "Мой блокнот" Then
            ' Файл не был сохранен.
            ' Получите имя файла, и затем вызовите сохраняющуюся процедуру, GetFileName.
            strFilename = GetFileName(strFilename)
        ElseIf TextEditorForm.Text = "Блокнот" Then
            strFilename = GetFileName("Untitled.txt")
        Else
            ' Заголовок(надпись) формы содержит название открытого файла.
            strFilename = Right(TextEditorForm.Text, Len(TextEditorForm.Text) - 14)
        End If

        ' Вызвать процедуру сохранения. Если Имя файла = Пусто, то
        ' Пользователь выбрал Отмену в диалоговом окне Save As; 
        ' иначе, Сохранить файл.
        If strFilename <> "" Then
            SaveFileAs(strFilename)
            Return True
        Else
            Return False
        End If
    End Function

    'Public Sub FindIt()
    '    'если не чувствительно к регистру, конвертируйте строку к верхнему регистру
    '    If gFindCase Then
    '        mfrmSDI.txtNote.Find(gFindString, RichTextBoxFinds.MatchCase)
    '    Else
    '        mfrmSDI.txtNote.Find(gFindString, RichTextBoxFinds.None)
    '    End If
    'End Sub

    Sub FindIt()
        Dim start As Integer
        Dim pos As Integer
        Dim findString As String = String.Empty
        Dim sourceString As String
        Dim offset As Integer

        ' Набор сместил переменную, основанную на позиции курсора.
        If (CurPos = TextEditorForm.txtNote.SelectionStart) Then
            offset = 1
        Else
            offset = 0
        End If

        ' определить переменную для позиции начала.
        If IsFirstTime Then offset = 0
        ' Назначить значение на значение начала.
        start = TextEditorForm.txtNote.SelectionStart + offset

        'если не чувствительно к регистру, конвертировать строку к верхнему регистру
        If FindCase Then
            findString = FindString
            sourceString = TextEditorForm.txtNote.Text
        Else
            findString = UCase(FindString)
            sourceString = UCase(TextEditorForm.txtNote.Text)
        End If

        ' найти строку.
        If FindDirection = 1 Then
            pos = InStr(start + 1, sourceString, findString)
        Else
            For pos = start - 1 To 0 Step -1
                If pos = 0 Then Exit For
                If Mid(sourceString, pos, Len(findString)) = findString Then Exit For
            Next
        End If

        ' если строка найдена ...
        If CBool(pos) Then
            TextEditorForm.txtNote.SelectionStart = pos - 1
            TextEditorForm.txtNote.SelectionLength = Len(findString)
        Else
            MessageBox.Show($"Невозможно найти {Chr(34)}{findString}{Chr(34)}",
                            Reflection.Assembly.GetExecutingAssembly.GetName.Name, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ' Сбросить переменные
        CurPos = TextEditorForm.txtNote.SelectionStart
        IsFirstTime = False
        TextEditorForm.txtNote.Focus()
        Application.DoEvents()
    End Sub

    'Sub GetRecentFiles()
    '    ' Процедура  демонстрирует использование функции GetAllSettings,
    '    ' которыая возвращает массив значений от системного реестра Windows. В этом
    '    ' регистре, системный реестр содержит файлы, последние недавно открытые. Использовать
    '    ' Инструкция SaveSetting, чтобы записать названия самых файлов.
    '    ' Инструкция что используется в процедуре WriteRecentFiles.
    '    Dim I As Integer
    '    Dim varFiles As Variant ' Varible, чтобы сохранить возвращенный массив.
    '
    '    ' Получить недавние файлы от системного реестра, используя инструкцию GetAllSettings.
    '    ' ThisApp и ThisKey - константы, определенные в этом модуле.
    '    If GetSetting(ThisApp, ThisKey, "RecentFile1") = Empty Then Exit Sub
    '
    '    varFiles = GetAllSettings(ThisApp, ThisKey)
    '
    '    For I = 0 To UBound(varFiles, 1)
    '        frmSDI.mnuRecentFile(0).Visible = True
    '        frmSDI.mnuRecentFile(I + 1).Caption = varFiles(I, 1)
    '        frmSDI.mnuRecentFile(I + 1).Visible = True
    '    Next I
    'End Sub
    'Sub ResizeNote()
    '    ' Текстовое поле Expand, чтобы заполнить внутреннюю область формы.
    '    'On Error Resume Next'доработка
    '    If mfrmSDI.picToolbar.Visible Then
    '        mfrmSDI.txtNote.Height = mfrmSDI.ClientRectangle.Height - mfrmSDI.picToolbar.Height
    '        mfrmSDI.txtNote.Width = mfrmSDI.ClientRectangle.Width
    '        mfrmSDI.txtNote.Top = mfrmSDI.picToolbar.Height
    '    Else
    '        mfrmSDI.txtNote.Height = mfrmSDI.ClientRectangle.Height
    '        mfrmSDI.txtNote.Width = mfrmSDI.ClientRectangle.Width
    '        mfrmSDI.txtNote.Top = 0
    '    End If
    'End Sub

    Sub WriteRecentFiles(ByRef OpenFileName As String)
        ' Процедура  использует инструкцию SaveSettings, чтобы записать названия
        ' недавно открытых файлов в Системном системному реестре. 
        ' SaveSetting инструкция требует трех параметров. 
        ' Два из параметров постоянны, поскольку константы и определены в этом модуле. 
        ' GetAllSettings - Функция используется в процедуре GetRecentFiles, чтобы восстановить
        ' имена файла, сохраненные в этой процедуре.
        Dim strFile As String
        Dim strKey As String

        ' Копируйте RecentFile1 к RecentFile2, и так далее.
        For I As Integer = 3 To 1 Step -1
            strKey = "RecentFile" & I
            strFile = GetSetting(thisApp, thisKey, strKey)
            If strFile <> "" Then
                strKey = "RecentFile" & (I + 1)
                SaveSetting(thisApp, thisKey, strKey, strFile)
            End If
        Next

        ' Запиcать открытый файл к первому недавнему файлу.
        SaveSetting(thisApp, thisKey, "RecentFile1", OpenFileName)
    End Sub
End Module