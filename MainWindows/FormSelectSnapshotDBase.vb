Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports MathematicalLibrary
Imports VB = Microsoft.VisualBasic

Friend Class FormSelectSnapshotDBase
    Public Property ParentFormMain() As FormSnapshotBase

    Private rootNode As TreeNode
    Private numberEngine As Integer ' номер Изделия
    Private catalogReceiver, fileReceiver As String
    Private isEnvokeFromTreeView As Boolean ' вызван Из Дерева
    Private isFormLoaded As Boolean = False ' форма Загружена
    Private isUseTdmsRead As Boolean

    Private Const cLoad As String = "TSMenuItemLoad1"
    Private Const cDeleteSelect As String = "TSMenuItemDeleteSelect1"
    Private Const cDeleteAll As String = "TSMenuItemDeleteAll1"
    Private Const cRemoveToArchive As String = "TSMenuItemRemoveToArchive1"
    Private Const cAvtomaticArchives As String = "TSMenuItemAvtomaticArchives"
    Private Const cConvertAllToTXT As String = "TSMenuItemConvertAllToTXT"
    Private Const cConvertSelectToTXT As String = "TSMenuItemConvertSelectToTXT"

    Delegate Sub ContinueActionHandler(cn As OleDbConnection)

    ''' <summary>
    ''' Автоматический разбор в Архив
    ''' </summary>
    Public Async Function AutomaticArchives() As Task
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Автоматический Разбор В Архив")

        For I As Integer = 0 To rootNode.Nodes.Count - 1
            If TreeViewSnapshot.Nodes(0).Nodes.Count > 0 Then
                TreeViewSnapshot.Nodes(0).FirstNode.Expand()
                Await RemoveToArchiveTask()
            End If
        Next
    End Function

    ''' <summary>
    ''' Подготовка при вызове из ServiceBase
    ''' </summary>
    Public Sub PreparationForCallFromServiceBase()
        TreeViewSnapshot.Nodes(0).Expand()
        isEnvokeFromTreeView = True
    End Sub

#Region "Form"
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        InitializeListViewSnapshot()
        isFormLoaded = True
        ' загрузка
        UpdateForm()
        InitializeStatusBar()
        FormResize()
    End Sub
    Private Sub InitializeListViewSnapshot()
        '' Очистка ColumnHeaders коллекции.
        'lvwDBЛист.Columns.Clear() 
        'lvwDBЛист.View = View.Details
        '' разрешить редактирование текста.
        'lvwDBЛист.LabelEdit = False
        '' Позволить пользователю перестраивать столбцы.
        'lvwDBЛист.AllowColumnReorder = False
        '' Display check boxes.
        'lvwDBЛист.CheckBoxes = False
        '' включить режим выделения всех пунктов в строке.
        'lvwDBЛист.FullRowSelect = True
        '' Показывать grid lines.
        'lvwDBЛист.GridLines = False
        ''Сортировать элементы в списке в возрастающем порядке.
        ''lvwDBЛист.Sorting = SortOrder.Ascending'доработка
        '' создать колонки для пунктов и подпунктов.
        ListViewSnapshot.Columns.Add("Дата", ListViewSnapshot.Width * 4 \ 20, HorizontalAlignment.Left)
        ListViewSnapshot.Columns.Add("Время", ListViewSnapshot.Width * 3 \ 20, HorizontalAlignment.Left)
        ListViewSnapshot.Columns.Add("Режим", ListViewSnapshot.Width * 10 \ 20 - 4, HorizontalAlignment.Left)
        ListViewSnapshot.Columns.Add("Запись", ListViewSnapshot.Width * 3 \ 20, HorizontalAlignment.Left)
    End Sub

    ''' <summary>
    ''' Заполнение дерева проводника по базе данных снимков
    ''' </summary>
    Private Sub UpdateForm()
        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        For Each itemForm In CollectionForms.Forms.Values
            If CStr(itemForm.Tag) = TagFormSnapshot Then ' закрыть навигационную панель
                itemForm.Close()
                Exit For
            End If
        Next

        Text = "Проводник по базе данных снимков " & PathChannels
        ListViewSnapshot.Items.Clear()
        TreeViewSnapshot.BeginUpdate()
        TreeViewSnapshot.Nodes.Clear()

        rootNode = New TreeNode("Root") With {.Text = "Изделия", .Tag = "Корневой", .ImageIndex = 0}
        TreeViewSnapshot.Nodes.Add(rootNode)

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text

            Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("SELECT DISTINCT БазаСнимков.НомерИзделия FROM БазаСнимков ORDER BY НомерИзделия", cn)
                Dim dtDataTable As New DataTable
                odaDataAdapter.Fill(dtDataTable)
                Dim rowsCount As Integer = dtDataTable.Rows.Count
                Dim counter As Integer

                TSProgressBar.Visible = True
                isUseTdmsRead = False

                For Each drDataRow As DataRow In dtDataTable.Rows
                    TSProgressBar.Value = CInt(CDbl(counter / rowsCount) * 100)
                    ' Идентифицирует таблицу.
                    Dim mNodeEngine As New TreeNode(CStr(drDataRow("НомерИзделия"))) With {.Tag = 0 & "Изделия", .ImageIndex = 0}
                    rootNode.Nodes.Add(mNodeEngine)
                    cmd.CommandText = $"SELECT * FROM [БазаСнимков] WHERE НомерИзделия = {drDataRow("НомерИзделия")} ORDER BY БазаСнимков.KeyID"

                    Using rdr As OleDbDataReader = cmd.ExecuteReader
                        Do While rdr.Read
                            Dim mNodeSnapshot As New TreeNode($"Дата:{Convert.ToDateTime(rdr("Дата")).ToShortDateString} Время:{Convert.ToDateTime(rdr("ВремяНачалаСбора")).ToShortTimeString} Режим:{rdr("Режим")} {rdr("Примечание")}", If(CStr(rdr("Режим")) = cРегистратор, 3, 2), 4) With {
                                .Tag = CStr(rdr("KeyID")) & "Снимок" ' Идентифицирует таблицу.
                                }

                            If Not isUseTdmsRead Then ' чтобы не каждый раз проверять
                                If Path.GetExtension(rdr("ПутьНаДиске").ToString).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then isUseTdmsRead = True
                            End If

                            mNodeEngine.Nodes.Add(mNodeSnapshot)
                        Loop ' Переместить на следующую запись по данному изделию
                    End Using
                    counter += 1
                Next ' Переместить на следующую запись номер Изделия.
            End Using
        End Using

        TSProgressBar.Visible = False
        TSProgressBar.Value = 0
        ' раскрыть высший узел.
        TreeViewSnapshot.Nodes(0).Expand()
        TreeViewSnapshot.EndUpdate()
        GKeyID = 0
        Windows.Forms.Cursor.Current = Cursors.Default
    End Sub

    Private Sub InitializeStatusBar()
        Using conn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            conn.Open()
            Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)

            For Each row As DataRow In schemaTable.Rows
                If row("TABLE_NAME").ToString = "БазаСнимков" Then
                    ' Открытие основной таблицы и вывод основных параметров
                    StatusStripMessage.Items(0).Text = "Имя: " & row("TABLE_NAME").ToString
                    StatusStripMessage.Items(1).Text = "Создана:" & row("DATE_CREATED").ToString
                    StatusStripMessage.Items(2).Text = "Модификация:" & row("DATE_MODIFIED").ToString
                    StatusStripMessage.Items(3).Text = "Тип:" & row("TABLE_TYPE").ToString
                    'TABLE_CATALOG()
                    'TABLE_SCHEMA()
                    'TABLE_NAME()
                    'TABLE_TYPE()
                    'TABLE_GUID()
                    'DESCRIPTION()
                    'TABLE_PROPID()
                    'DATE_CREATED()
                    'DATE_MODIFIED()
                    Exit For
                End If
            Next row
        End Using
    End Sub

    Private Sub FormTreeview_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
    End Sub

    Private Sub CloseForm(ByVal sender As Object, ByVal e As EventArgs) Handles TreeViewSnapshot.DoubleClick,
                                                                                ListViewSnapshot.DoubleClick,
                                                                                TSMenuItemLoad1.Click,
                                                                                TSActionMenuItemLoad1.Click,
                                                                                TSMenuItemLoad2.Click,
                                                                                TSActionMenuItemLoad2.Click,
                                                                                ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub SplitContainer_SplitterMoved(ByVal sender As Object, ByVal e As SplitterEventArgs) Handles SplitContainer.SplitterMoved
        FormResize()
    End Sub

    Private Sub FormTreeview_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        FormResize()
    End Sub

    Private Sub FormResize()
        If isFormLoaded Then
            ListViewSnapshot.Columns.Item(0).Width = ListViewSnapshot.Width * 4 \ 20
            ListViewSnapshot.Columns.Item(1).Width = ListViewSnapshot.Width * 3 \ 20
            ListViewSnapshot.Columns.Item(2).Width = ListViewSnapshot.Width * 10 \ 20 - 4
            ListViewSnapshot.Columns.Item(3).Width = ListViewSnapshot.Width * 3 \ 20
            TSProgressBar.Width = StatusStripMessage.Width - TSStatusLabel2.Width - TSStatusLabel1.Width - TSStatusLabel3.Width - 100
        End If
    End Sub
#End Region

#Region "TreeViewSnapshot Events"
    ''' <summary>
    ''' Делать доступным пункты меню
    ''' </summary>
    ''' <param name="isLoad"></param>
    ''' <param name="isDeleteSelect"></param>
    ''' <param name="isDeleteAll"></param>
    Private Sub SetContextMenuStripTreeView(isLoad As Boolean, isDeleteSelect As Boolean, isDeleteAll As Boolean)
        ContextMenuStripTreeView.Items(cLoad).Enabled = isLoad
        ContextMenuStripTreeView.Items(cDeleteSelect).Enabled = isDeleteSelect
        ContextMenuStripTreeView.Items(cDeleteAll).Enabled = isDeleteAll
        ContextMenuStripTreeView.Items(cRemoveToArchive).Enabled = isDeleteAll
        ContextMenuStripTreeView.Items(cAvtomaticArchives).Enabled = isDeleteAll
        ContextMenuStripTreeView.Items(cConvertAllToTXT).Enabled = isUseTdmsRead
        ContextMenuStripListView.Items(cConvertSelectToTXT).Enabled = isUseTdmsRead
    End Sub

    Private Sub TreeViewSnapshot_BeforeCollapse(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewSnapshot.BeforeCollapse
        e.Node.ImageIndex = 0
        ListViewSnapshot.Items.Clear()
        SetContextMenuStripTreeView(False, False, False)
    End Sub

    Private Sub TreeViewSnapshot_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewSnapshot.BeforeExpand
        For Each itemNode As TreeNode In rootNode.Nodes
            If itemNode.IsExpanded Then itemNode.Collapse()
        Next

        SetContextMenuStripTreeView(False, False, False)
    End Sub

    Private Sub TreeViewSnapshot_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewSnapshot.AfterExpand
        e.Node.ImageIndex = 1 ' "open"

        If CBool(InStr(e.Node.Tag.ToString, "Изделия")) Then
            GKeyID = CInt(Val(e.Node.Tag.ToString))
            numberEngine = CInt(e.Node.Text)
            PopulateListViewByEngine(numberEngine)
        End If

        SetContextMenuStripTreeView(False, False, True)
    End Sub

    ''' <summary>
    ''' Заполнить ListView снимками выбранного в узле дерева номером изделия
    ''' </summary>
    ''' <param name="inNumberEngine"></param>
    Private Sub PopulateListViewByEngine(inNumberEngine As Integer)
        TSProgressBar.Visible = True

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = $"SELECT COUNT(*) FROM БазаСнимков Where БазаСнимков.НомерИзделия = {inNumberEngine}"
            cn.Open()
            Dim countRows As Integer = CInt(cmd.ExecuteScalar)

            cmd.CommandText = "SELECT БазаСнимков.KeyID, БазаСнимков.НомерИзделия, БазаСнимков.Дата, БазаСнимков.ВремяНачалаСбора,БазаСнимков.Режим, БазаСнимков.Примечание " &
                "From БазаСнимков Where ((БазаСнимков.НомерИзделия) =  " & inNumberEngine.ToString & ")  " &
                "ORDER BY БазаСнимков.KeyID"
            Using rdr As OleDbDataReader = cmd.ExecuteReader
                ' Очистите старые заглавия
                ListViewSnapshot.BeginUpdate()
                ListViewSnapshot.Items.Clear()
                Dim countProgress As Integer = 1

                Do While rdr.Read
                    TSProgressBar.Value = CInt(CDbl(countProgress / countRows) * 100)

                    Dim newListViewItem As New ListViewItem(Convert.ToDateTime(rdr("Дата")).ToShortDateString, If(CStr(rdr("Режим")) = cРегистратор, 3, 2)) With {
                        .Tag = CStr(rdr("KeyID")) & " ID"
                    }

                    newListViewItem.SubItems.Add(CStr(rdr("ВремяНачалаСбора")))
                    newListViewItem.SubItems.Add(CStr(rdr("Режим")))

                    If Not IsDBNull(rdr("Примечание")) Then
                        If CBool(InStr(1, CStr(rdr("Примечание")), "Фоновая запись ")) Then
                            newListViewItem.SubItems.Add(VB.Right(CStr(rdr("Примечание")), Len(rdr("Примечание")) - 15))  '3
                        Else
                            newListViewItem.SubItems.Add(CStr(rdr("Примечание")))
                            'ElseIf InStr(1, rdr("Примечание"), "Объединённая") Then
                        End If
                    End If

                    ListViewSnapshot.Items.Add(newListViewItem)
                    countProgress += 1
                Loop

                ListViewSnapshot.EndUpdate()
            End Using
        End Using

        TSProgressBar.Visible = False
        TSProgressBar.Value = 0
    End Sub

    Private Sub TreeViewSnapshot_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewSnapshot.AfterSelect
        If CBool(InStr(e.Node.Tag.ToString, "Снимок")) Then
            GKeyID = CInt(Val(e.Node.Tag.ToString))
            numberEngine = CInt(e.Node.Parent.Text)
            SetContextMenuStripTreeView(True, True, True)
        ElseIf CBool(InStr(e.Node.Tag.ToString, "Изделия")) Then
            If Not e.Node.IsExpanded Then e.Node.Expand()
            numberEngine = CInt(e.Node.Text)
            SetContextMenuStripTreeView(False, False, True)
        End If

        VisibleMenuAction(sender)
    End Sub

    Private Sub ListViewSnapshot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListViewSnapshot.SelectedIndexChanged
        TSMenuItemLoad2.Enabled = ListViewSnapshot.SelectedIndices.Count > 0

        If ListViewSnapshot.SelectedIndices.Count > 0 Then
            GKeyID = CInt(Val(ListViewSnapshot.SelectedItems(0).Tag.ToString))
        End If

        VisibleMenuAction(sender)
    End Sub

    Private Sub VisibleMenuAction(sender As Object)
        If sender Is TreeViewSnapshot Then
            TSMenuItemActionsTreeView.Visible = True
            TSMenuItemActionsListView.Visible = False
        ElseIf sender Is ListViewSnapshot Then
            TSMenuItemActionsTreeView.Visible = False
            TSMenuItemActionsListView.Visible = True
        End If

        ' TSMenuItemActionsTreeView
        TSActionMenuItemLoad1.Enabled = TSMenuItemLoad1.Enabled
        TSActionMenuItemDeleteSelect1.Enabled = TSMenuItemDeleteSelect1.Enabled
        TSActionMenuItemDeleteAll1.Enabled = TSMenuItemDeleteAll1.Enabled
        TSActionMenuItemRemoveToArchive1.Enabled = TSMenuItemRemoveToArchive1.Enabled
        TSActionMenuItemAvtomaticArchives.Enabled = TSMenuItemAvtomaticArchives.Enabled
        TSActionMenuItemConvertAllToTXT.Enabled = TSMenuItemConvertAllToTXT.Enabled

        ' TSMenuItemActionsListView
        TSActionMenuItemLoad2.Enabled = TSMenuItemLoad2.Enabled
        TSActionMenuItemDeleteSelect2.Enabled = TSMenuItemDeleteSelect2.Enabled
        TSActionMenuItemExportSnapshotToExcel.Enabled = TSMenuItemExportSnapshotToExcel.Enabled
        TSActionMenuItemJoinSelectSnapshot.Enabled = TSMenuItemJoinSelectSnapshot.Enabled
        TSActionMenuItemConvertSelectToTXT.Enabled = TSMenuItemConvertSelectToTXT.Enabled
    End Sub
#End Region

#Region "Delete"
    ''' <summary>
    ''' Удалить один снимок из дерева проводника.
    ''' </summary>
    Private Sub DeleteOneSnapshotFromTreeView()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(DeleteOneSnapshotFromTreeView)}> [KeyID]={GKeyID}")

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim dtDataTable As New DataTable
            cn.Open()
            Using odaDataAdapter As New OleDbDataAdapter($"Select KeyID, ПутьНаДиске FROM БазаСнимков WHERE ([KeyID]={GKeyID})", cn)
                odaDataAdapter.Fill(dtDataTable)
            End Using

            ' делаем проверку на его отсутствие
            If dtDataTable.Rows.Count <> 0 Then
                ' удалить файл
                Dim fileName As String = CStr(dtDataTable.Rows(0)("ПутьНаДиске"))

                DeleteDataFile(fileName)

                If Path.GetExtension(fileName).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then DeleteDataFile(fileName & "_index")

                Dim success As Boolean = DeleteRecord(cn, GKeyID)
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Удалить все снимки изделия
    ''' </summary>
    Private Sub DeleteAllSnapshots()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(DeleteAllSnapshots)}> [НомерИзделия]={numberEngine}")
        Dim dtDataTable As New DataTable
        Dim rowsCount As Integer
        Dim pathCatalogueSourceDB As String = VB.Left(PathChannels, InStrRev(PathChannels, Separator))

        Invoke(New MethodInvoker(Sub() TSProgressBar.Visible = True))

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter($"Select KeyID, ПутьНаДиске FROM БазаСнимков WHERE (НомерИзделия = {numberEngine})", cn)
                odaDataAdapter.Fill(dtDataTable)
                ' делаем проверку на его отсутствие
                rowsCount = dtDataTable.Rows.Count

                If rowsCount <> 0 Then
                    Dim countProgress As Integer = 1

                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        Invoke(New MethodInvoker(Sub() TSProgressBar.Value = CInt(CDbl(countProgress / rowsCount) * 100)))
                        ' удалить файл
                        Dim deleteFile As String = itemDataRow("ПутьНаДиске").ToString
                        deleteFile = $"{pathCatalogueSourceDB}База снимков\{deleteFile.Substring(InStrRev(deleteFile, Separator))}"
                        DeleteDataFile(deleteFile)

                        If Path.GetExtension(deleteFile).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then DeleteDataFile(deleteFile & "_index")

                        ' надо удалить данную запись, а вслед за ней каскадно удаляются записи 
                        'drDataRow.Delete()
                        countProgress += 1
                    Next ' Переместить на следующую запись
                End If

                ' не получается удалить запись с полем = [Время].ToOADate.ToString из odaDataAdapter внутреннего запроса на удаление
                ' пришлось послать запрос на JET процессор базы Access
                Try
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = $"DELETE FROM БазаСнимков WHERE (НомерИзделия = {numberEngine})"
                    cmd.ExecuteNonQuery()
                Catch ex As InvalidOperationException
                    Const caption As String = "Ошибка при удалении строк"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                End Try

                'Dim cb As OleDbCommandBuilder
                'cb = New OleDbCommandBuilder(odaDataAdapter) 
                'odaDataAdapter.DeleteCommand = cb.GetDeleteCommand 
                'Try
                '    odaDataAdapter.Update(dtDataTable)
                'Catch ex As DBConcurrencyException
                '    'Почему-то на снимках сделанных SCXI удаление ошибку конкуренции, а на записях Осциллографа удаляет нормально
                '    'Console.WriteLine(ex.ToString)
                '    MessageBox.Show(ex.ToString, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                '    If dtDataTable.HasErrors Then
                '        Dim mDeleteCommand As OleDb.OleDbCommand = cn.CreateCommand
                '        For Each drDataRow In dtDataTable.GetErrors
                '            If drDataRow.RowState = DataRowState.Deleted Then 'DataRowState.Modified Then
                '                'Console.WriteLine(drDataRow("KeyID"))
                '                Console.WriteLine(countProgress)
                '                'mDeleteCommand.CommandText = "DELETE FROM БазаСнимков WHERE KeyID = " & drDataRow("KeyID")
                '                'odaDataAdapter.DeleteCommand = mDeleteCommand
                '                'Dim arrDataRow() As DataRow = {drDataRow}
                '                'odaDataAdapter.Update(arrDataRow)
                '            End If
                '        Next
                '    End If
                'Finally
                '    cn.Close()
                'End Try
            End Using
        End Using

        Invoke(New MethodInvoker(Sub()
                                     TSProgressBar.Visible = False
                                     TSProgressBar.Value = 0
                                 End Sub))
        numberEngine = 0
    End Sub

    ''' <summary>
    ''' Удалить помеченные снимки из листа
    ''' </summary>
    Private Sub DeleteSelectedSnapshotsFromListView()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(DeleteSelectedSnapshotsFromListView)}> [НомерИзделия]={numberEngine}")
        Invoke(New MethodInvoker(Sub() TSProgressBar.Visible = True))

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim dtDataTable As New DataTable
            Dim pathCatalogueSourceDB As String = VB.Left(PathChannels, InStrRev(PathChannels, Separator))

            cn.Open()
            Using odaDataAdapter As New OleDbDataAdapter($"Select KeyID, ПутьНаДиске FROM БазаСнимков WHERE БазаСнимков.НомерИзделия = {numberEngine}", cn)
                odaDataAdapter.Fill(dtDataTable)
            End Using
            ' делаем проверку на его отсутствие
            Dim rowsCount As Integer = dtDataTable.Rows.Count
            Dim countProgress As Integer = 1

            Try
                If rowsCount <> 0 Then
                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        Invoke(New MethodInvoker(Sub() TSProgressBar.Value = CInt(CDbl(countProgress / rowsCount) * 100)))
                        GKeyID = CInt(itemDataRow("KeyID"))

                        Invoke(New MethodInvoker(Sub()
                                                     For Each listSelectedItems As ListViewItem In ListViewSnapshot.SelectedItems
                                                         If Val(listSelectedItems.Tag) = GKeyID Then
                                                             Dim deleteFile As String = itemDataRow("ПутьНаДиске").ToString
                                                             deleteFile = $"{pathCatalogueSourceDB}База снимков\{deleteFile.Substring(InStrRev(deleteFile, Separator))}"
                                                             DeleteDataFile(deleteFile)

                                                             If Path.GetExtension(deleteFile).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then DeleteDataFile(deleteFile & "_index")

                                                             Dim success As Boolean = DeleteRecord(cn, GKeyID)
                                                             Exit For
                                                         End If
                                                     Next
                                                 End Sub))
                        countProgress += 1
                    Next
                End If
            Catch ex As InvalidOperationException
                Const caption As String = "Ошибка при удалении строк"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            End Try
        End Using

        Invoke(New MethodInvoker(Sub()
                                     TSProgressBar.Visible = False
                                     TSProgressBar.Value = 0
                                 End Sub))
    End Sub

    ''' <summary>
    ''' Удалить помеченные снимки из листа которые не равны numberEngine
    ''' </summary>
    ''' <param name="delegateContinueAction"></param>
    ''' <returns></returns>
    Private Function DeleteSelectedSnapshotsNotEngineWithContinueAction(delegateContinueAction As ContinueActionHandler) As Boolean
        ' открываем скопированную базу
        ' выделяем все записи новой таблицы снимков и удаляем те, которые не выделенны на листе
        Using cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, fileReceiver))
            cn.Open()
            Using dtDataTable As New DataTable
                Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("Select KeyID FROM БазаСнимков", cn)
                    odaDataAdapter.Fill(dtDataTable)
                End Using

                Dim rowsCount As Integer = dtDataTable.Rows.Count
                Dim isDelete As Boolean
                Dim countProgress As Integer = 1

                ' делаем проверку на его отсутствие
                If rowsCount <> 0 Then
                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        isDelete = True
                        GKeyID = CInt(itemDataRow("KeyID"))
                        ' взять вызов контролов в обёртку Invoke
                        Invoke(New MethodInvoker(Sub()
                                                     TSProgressBar.Value = CInt(CDbl(countProgress / rowsCount) * 100)
                                                     For I As Integer = 0 To ListViewSnapshot.Items.Count - 1
                                                         If ListViewSnapshot.Items(I).Selected AndAlso Val(ListViewSnapshot.Items(I).Tag) = GKeyID Then
                                                             isDelete = False
                                                             Exit For
                                                         End If
                                                     Next
                                                 End Sub))
                        ' надо удалить данную запись, а вслед за ней каскадно удаляются записи 
                        If isDelete Then
                            If DeleteRecord(cn, GKeyID) = False Then
                                cn.Close()
                                Return False
                            End If
                        End If
                        countProgress += 1
                    Next
                End If
            End Using

            delegateContinueAction(cn)
        End Using

        Return True
    End Function

    ''' <summary>
    '''  Удалить снимки которые не равны numberEngine (вызов из дерева)
    ''' </summary>
    ''' <param name="delegateContinueAction"></param>
    ''' <returns></returns>
    Private Function DeleteAllSnapshotsNotEngineWithContinueAction(delegateContinueAction As ContinueActionHandler) As Boolean
        ' открываем скопированную базу
        ' выделяем все записи таблицы снимков и удаляем те которые не равны numberEngine
        Using cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, fileReceiver))
            cn.Open()
            ' не получается удалить запись с полем = [Время].ToOADate.ToString из odaDataAdapter внутреннего запроса на удаление
            ' пришлось послать запрос на JET процессор базы Access
            'odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            'odaDataAdapter.Fill(dtDataTable)

            'rowsCount = dtDataTable.Rows.Count
            'If rowsCount <> 0 Then
            '    countProgress = 1
            '    For Each drDataRow In dtDataTable.Rows
            '        If countProgress Mod 10 = 0 Then TSProgressBar.Value = CDbl(countProgress / rowsCount) * 100
            '        'надо удалить данную запись, а вслед за ней каскадно удаляются записи 
            '        drDataRow.Delete()
            '        countProgress += 1
            '    Next 'Переместить на следующую запись
            'End If
            'cb = New OleDbCommandBuilder(odaDataAdapter)
            'odaDataAdapter.Update(dtDataTable)

            Try
                Dim cmd As OleDbCommand = cn.CreateCommand
                cmd.CommandType = CommandType.Text
                cmd.CommandText = $"DELETE FROM БазаСнимков WHERE (НомерИзделия <> {numberEngine})"
                cmd.ExecuteNonQuery()
            Catch ex As InvalidOperationException
                Const caption As String = "Ошибка при удалении строк"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                cn.Close()
                Invoke(New MethodInvoker(Sub()
                                             TSProgressBar.Visible = False
                                             TSProgressBar.Value = 0
                                         End Sub))
                Return False
            End Try

            delegateContinueAction(cn)
        End Using

        Return True
    End Function

    ''' <summary>
    ''' Удалить запись снимка в базе данных.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="keyID"></param>
    ''' <returns></returns>
    Private Function DeleteRecord(cn As OleDbConnection, keyID As Integer) As Boolean
        'dtDataTable.Rows(0).Delete()
        ' не получается удалить запись с полем = [Время].ToOADate.ToString из odaDataAdapter внутреннего запроса на удаление
        ' пришлось послать запрос на JET процессор базы Access
        Try
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = $"DELETE FROM БазаСнимков WHERE (KeyId = {keyID})"
            cmd.ExecuteNonQuery()
        Catch ex As InvalidOperationException
            Const caption As String = "Ошибка при удалении строк"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "RemoveSnapshotsToArchive"
    ''' <summary>
    ''' Перенести в Архив все снимки изделия
    ''' </summary>
    ''' <returns></returns>
    Private Function RemoveAllSnapshotsToArchiveFromTreeView() As Boolean
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(RemoveAllSnapshotsToArchiveFromTreeView)}> [НомерИзделия]={numberEngine}")

        If DeleteAllSnapshotsNotEngineWithContinueAction(AddressOf CopySnapshots) = False Then Return False

        ' по номеру двигателя запрос на удаление
        DeleteAllSnapshots()
        Return True
    End Function

    ''' <summary>
    ''' Перенести в Архив отмеченные снимки изделия
    ''' </summary>
    ''' <returns></returns>
    Private Function RemoveToArchiveFromListViewSnapshot() As Boolean
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(RemoveToArchiveFromListViewSnapshot)}> [НомерИзделия]={numberEngine}")

        If DeleteSelectedSnapshotsNotEngineWithContinueAction(AddressOf CopySnapshots) = False Then Return False

        ' удалить выделенные снимки в базе источнике
        DeleteSelectedSnapshotsFromListView()
        Return True
    End Function

    ''' <summary>
    ''' Копирование файлов данных.
    ''' Замена записей в снимках путь на новый каталог.
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub CopySnapshots(cn As OleDbConnection)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(CopySnapshots)}> [НомерИзделия]={numberEngine}")

        Dim pathCatalogueSourceDB As String = VB.Left(PathChannels, InStrRev(PathChannels, Separator))

        Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("Select KeyID, ПутьНаДиске FROM БазаСнимков", cn)
            Dim dtDataTable As New DataTable
            odaDataAdapter.Fill(dtDataTable)
            ' делаем проверку на отсутствие записей
            Dim rowsCount As Integer = dtDataTable.Rows.Count

            If rowsCount <> 0 Then
                Dim countProgress As Integer = 1

                Try
                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        Invoke(New MethodInvoker(Sub() TSProgressBar.Value = CInt(CDbl(countProgress / rowsCount) * 100)))
                        ' переименовать путь к данным испытаний
                        Dim fileData As String = itemDataRow("ПутьНаДиске").ToString
                        Dim oldPath As String = $"{pathCatalogueSourceDB}База снимков\{fileData.Substring(InStrRev(fileData, Separator))}"
                        Dim newPath As String = $"{catalogReceiver}База снимков\{VB.Right(fileData, Len(fileData) - InStrRev(fileData, Separator))}"

                        itemDataRow("ПутьНаДиске") = newPath

                        ' скопировать файл с данными в заданный каталог
                        If File.Exists(oldPath) Then
                            File.Copy(oldPath, newPath, True)
                            If Path.GetExtension(fileData).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then File.Copy(oldPath & "_index", newPath & "_index", True)
                        End If
                        countProgress += 1
                    Next

                    Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
                    odaDataAdapter.Update(dtDataTable)
                    'Catch ex As DBConcurrencyException
                    '    ' Почему-то первая запись показывает ошибку параллелелизма
                    '    ' если ограничить число полей не содержащих дата, то наверно ошибки не будет
                    '    'Console.WriteLine(ex.ToString)
                    '    If dtDataTable.HasErrors Then
                    '        Dim mUpdateCommand As OleDbCommand = cn.CreateCommand

                    '        For Each itemDataRow As DataRow In dtDataTable.GetErrors
                    '            If itemDataRow.RowState = DataRowState.Modified Then
                    '                'Console.WriteLine(drDataRow("KeyID"))
                    '                mUpdateCommand.CommandText = $"Update БазаСнимков Set ПутьНаДиске = '{itemDataRow("ПутьНаДиске")}' WHERE KeyID = {itemDataRow("KeyID")}"
                    '                odaDataAdapter.UpdateCommand = mUpdateCommand
                    '                Dim arrDataRow As DataRow() = {itemDataRow}
                    '                odaDataAdapter.Update(arrDataRow)
                    '            End If
                    '        Next
                    '    End If
                Catch ex As Exception ' InvalidOperationException
                    Const caption As String = "Ошибка переноса в Архив снимков."
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                End Try
            End If
        End Using
    End Sub
#End Region

#Region "ConversionSnapshots"
    ' Можно вызвать как Async, результат тот же
    Private Async Sub TSMenuItemConvertSelectToTXT_Click(sender As Object, e As EventArgs) Handles TSMenuItemConvertSelectToTXT.Click, TSActionMenuItemConvertSelectToTXT.Click, TSMenuItemConvertAllToTXT.Click, TSActionMenuItemConvertAllToTXT.Click
        Await ConversionSnapshotsToTXTTaskAsync(CType(sender, ToolStripMenuItem)).ConfigureAwait(False)
    End Sub

    Private Function ConversionSnapshotsToTXTTaskAsync(sender As ToolStripMenuItem) As Tasks.Task
        Return Task.Run(Sub()
                            ConversionSnapshotsToTXT(sender)
                        End Sub)
    End Function

    'Private Sub TSMenuItemConvertSelectToTXT_Click(sender As Object, e As EventArgs) Handles TSMenuItemConvertSelectToTXT.Click, TSMenuItemConvertAllToTXT.Click
    '    Dim m_Thread As Thread = New Thread(AddressOf ConversionSnapshotsToTXT)
    '    'то же самое  m_Thread = New Thread(New ThreadStart(AddressOf ConversionSnapshotsToTXT))
    '    m_Thread.Start(sender)
    'End Sub

    ''' <summary>
    ''' Конвертирование снимков в старый формат .txt
    ''' </summary>
    Private Sub ConversionSnapshotsToTXT(sender As ToolStripMenuItem)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ConversionSnapshotsToTXT)}> [НомерИзделия]={numberEngine}")
        Invoke(New MethodInvoker(Sub() TSProgressBar.Visible = True))

        CreateEngineFolder("конвертирование")
        '--- процесс конвертирования ------------------------------------------------
        ' скопировать базу данных в заданный каталог
        File.Copy(PathChannels, fileReceiver, True)
        ' создать каталог для текстовых или TDMS данных
        Directory.CreateDirectory(catalogReceiver & "База снимков")

        If sender Is TSMenuItemConvertSelectToTXT OrElse sender Is TSActionMenuItemConvertSelectToTXT Then
            ' Конвертировать помеченные снимки из листа в старый формат .txt
            If DeleteSelectedSnapshotsNotEngineWithContinueAction(AddressOf ConversionSnapshots) = False Then Exit Sub
        ElseIf sender Is TSMenuItemConvertAllToTXT OrElse sender Is TSActionMenuItemConvertAllToTXT Then
            ' Конвертировать все снимки изделия из дерева проводника в старый формат .txt
            If DeleteAllSnapshotsNotEngineWithContinueAction(AddressOf ConversionSnapshots) = False Then Exit Sub
        End If

        Invoke(New MethodInvoker(Sub()
                                     TSProgressBar.Visible = False
                                     TSProgressBar.Value = 0
                                     MessageBox.Show($"Конвертирование записей в старый формат произведено в каталог:{Environment.NewLine}{catalogReceiver}",
                                                     "Конвертирование в старый формат", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                 End Sub))
    End Sub

    ''' <summary>
    ''' Конвертирование файлов данных.
    ''' Замена записей в снимках путь на новый каталог.
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub ConversionSnapshots(cn As OleDbConnection)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ConversionSnapshots)}> [НомерИзделия]={numberEngine}")

        Dim pathCatalogueSourceDB As String = VB.Left(PathChannels, InStrRev(PathChannels, Separator))

        Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("Select KeyID, ПутьНаДиске FROM БазаСнимков", cn)
            Dim dtDataTable As New DataTable
            odaDataAdapter.Fill(dtDataTable)
            ' делаем проверку на отсутствие записей
            Dim rowsCount As Integer = dtDataTable.Rows.Count

            If rowsCount <> 0 Then
                Dim countProgress As Integer = 1

                Try
                    For Each itemDataRow As DataRow In dtDataTable.Rows
                        Invoke(New MethodInvoker(Sub() TSProgressBar.Value = CInt(CDbl(countProgress / rowsCount) * 100)))
                        ' конвертировать файл TDMS в старый формат
                        Dim fileData As String = itemDataRow("ПутьНаДиске").ToString
                        If Path.GetExtension(fileData).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then
                            Dim oldPathTDMS As String = $"{pathCatalogueSourceDB}База снимков\{fileData.Substring(InStrRev(fileData, Separator))}"
                            Dim newPathTXT As String = $"{catalogReceiver}База снимков\{VB.Right(fileData, Len(fileData) - InStrRev(fileData, Separator))}".Replace("tdms", "txt")

                            If File.Exists(oldPathTDMS) Then
                                ConvertFromTDMSToTextFile(oldPathTDMS, newPathTXT)
                                itemDataRow("ПутьНаДиске") = newPathTXT
                            End If
                        End If
                        countProgress += 1
                    Next

                    Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
                    odaDataAdapter.UpdateCommand = cb.GetUpdateCommand
                    odaDataAdapter.Update(dtDataTable)
                    dtDataTable.AcceptChanges()
                Catch ex As Exception ' InvalidOperationException
                    Const caption As String = "Ошибка конвертирования снимков в старый формат."
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                End Try
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Конверирование файла из формата TDMS в старый TXT формат.
    ''' </summary>
    ''' <param name="oldPathTDMS"></param>
    ''' <param name="newPathTXT"></param>
    Private Sub ConvertFromTDMSToTextFile(oldPathTDMS As String, newPathTXT As String)
        'File.Copy(oldPathTDMS, newPathTXT, True) ' отладка

        Dim MeasuredValues As Double(,)
        Using readTdmsFileProcessor As New TdmsFileProcessor
            MeasuredValues = readTdmsFileProcessor.LoadTDMSFile(oldPathTDMS)
        End Using

        ' 200 каналов * 8 символов * 54000 строк(максимум) = 86 400 000
        Dim dataMeasuredValuesString As New StringBuilder(86400000)

        ' делаем ссылку и открываем поток
        Using FS As New FileStream(newPathTXT, FileMode.Create, FileAccess.Write)
            Using SW As New StreamWriter(FS)
                Dim ParametersCount As Integer = UBound(MeasuredValues)

                For J As Integer = 0 To UBound(MeasuredValues, 2)
                    For I As Integer = 0 To ParametersCount - 1 ' параметры
                        dataMeasuredValuesString.Append(CStr(Math.Round(MeasuredValues(I, J), Precision)) & vbTab)
                    Next
                    dataMeasuredValuesString.Append(CStr(Math.Round(MeasuredValues(ParametersCount, J), Precision)) & vbCrLf)
                Next

                SW.Write(dataMeasuredValuesString.ToString)
            End Using
        End Using
    End Sub
#End Region

#Region "RemoveToArchive"
    Private Async Sub RemoveToArchiveAsync()
        Await RemoveToArchiveTask()
    End Sub

    Private Function RemoveToArchiveTask() As Tasks.Task
        Return Task.Run(Sub() RemoveToArchive())
    End Function

    ''' <summary>
    ''' Перенести В Архив
    ''' </summary>
    Private Sub RemoveToArchive()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(RemoveToArchive)}> [НомерИзделия]={numberEngine}")
        Dim isRecordEnable As Boolean ' состояние Запись

        If numberEngine = 0 Then
            Invoke(New MethodInvoker(Sub() If TreeViewSnapshot.Nodes(0).Nodes.Count > 0 Then TreeViewSnapshot.Nodes(0).FirstNode.Expand()))
        End If

        If numberEngine = 0 Then Exit Sub ' еще раз проверка если корень пуст

        Dim memonumberEngine = numberEngine

        If ParentFormMain IsNot Nothing Then ' текущий диалог может создаваться из 2-х родительских форм из frmServiceBases РодительскаяФорма не присваимвается
            Invoke(New MethodInvoker(Sub()
                                         isRecordEnable = ParentFormMain.IsRecordEnable
                                         ParentFormMain.IsRecordEnable = False
                                     End Sub))
        End If

        Dim ListViewSnapshotItemsCount As Integer
        Invoke(New MethodInvoker(Sub() ListViewSnapshotItemsCount = ListViewSnapshot.Items.Count))
        'If ListViewSnapshot.Items.Count = 0 Then Exit Sub

        CreateEngineFolder("архивирование")

        ' проверка на ранее созданный архив
        If Directory.Exists(catalogReceiver & "База снимков") Then
            Const caption As String = "Архивирование"
            Dim text As String = $"Создайте новую папку для архива {numberEngine} изделия!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            If ParentFormMain IsNot Nothing Then
                Invoke(New MethodInvoker(Sub() ParentFormMain.IsRecordEnable = isRecordEnable))
            End If
            Exit Sub
        End If

        '--- процесс переноса ------------------------------------------------
        Invoke(New MethodInvoker(Sub() TSProgressBar.Visible = True))
        ' скопировать базу данных в заданный каталог
        File.Copy(PathChannels, fileReceiver, True)
        ' создать каталог для текстовых или TDMS данных
        Directory.CreateDirectory(catalogReceiver & "База снимков")

        Dim success As Boolean
        If isEnvokeFromTreeView Then
            success = RemoveAllSnapshotsToArchiveFromTreeView()
        Else
            success = RemoveToArchiveFromListViewSnapshot()
        End If

        If success Then
            CompressionDBase(memonumberEngine)
            Invoke(New MethodInvoker(Sub() UpdateForm())) ' обновление дерева

            If ParentFormMain IsNot Nothing Then
                Invoke(New MethodInvoker(Sub() ParentFormMain.IsRecordEnable = isRecordEnable))
            End If
        End If

        Invoke(New MethodInvoker(Sub()
                                     TSProgressBar.Visible = False
                                     TSProgressBar.Value = 0
                                 End Sub))
    End Sub

    ''' <summary>
    ''' Сжатие баз источника и приемника
    ''' </summary>
    Private Sub CompressionDBase(numberEngine As Integer)
        ' заменил работу JRO в отдельном исполняемом файле
        ' было
        'Восстанавление(True)
        'Восстанавление(False)
        ' сообщение о проводимой процедуре
        '****************************
        Dim messageBoxForm As FormMessageBox = New FormMessageBox("Подождите, идет сжатие и восстановление", "Сжатие базы")
        messageBoxForm.Show()
        messageBoxForm.Activate()
        messageBoxForm.Refresh()
        Thread.Sleep(500) ' нужно время после удаления

        Dim caption As String = "При сжатии базы превышено время ожидания." & Environment.NewLine
        ' сжать рабочую базу
        If CompressionBaseAfterCopying(PathChannels) = BackupingResult.BackupingIsSuccess Then
            Thread.Sleep(5000) ' ждать закрытия BackupJRO.exe
            ' сжать архивную базу
            If CompressionBaseAfterCopying(fileReceiver) = BackupingResult.BackupingIsSuccess Then caption = "Сжатие произведено успешно." & Environment.NewLine
        End If

        caption += $"Успешный перенос записей {numberEngine} изделия"
        Dim text As String = $"Записи изделия {numberEngine} были перенесены в папку{vbCrLf}{catalogReceiver}"
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        messageBoxForm.Close()
    End Sub

    '''' <summary>
    '''' Восстанавливает базу данных и выполняет ее сжатие.
    '''' </summary>
    '''' <param name="сжатьИсточник"></param>
    'Private Sub Восстанавление(сжатьИсточник As Boolean)
    '    Dim strBackup As String = PathResourses & "\Temp\Copy.mdb"
    '    Dim je As New JRO.JetEngine
    '    Dim strFilename As String ' где будет копия 'доработка

    '    If сжатьИсточник Then ' сжать источник
    '        strFilename = PathChannels
    '    Else ' сжать приемник
    '        strFilename = файлПриемник
    '    End If

    '    System.Threading.Thread.Sleep(200)

    '    '--- Сделать рабочую копию базы данных --------------------------------
    '    Try
    '        File.Copy(strFilename, strBackup, True)
    '    Catch ex As Exception
    '        Const caption As String = "Ошибка копирования базы данных при сжатии"
    '        Dim text As String = ex.ToString
    '        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
    '    End Try

    '    Application.DoEvents()

    '    Try
    '        File.Delete(strFilename)
    '        System.Threading.Thread.Sleep(200)
    '        '--- Восстановленные базы данных должны быть сжаты. ---------------
    '        je.CompactDatabase(ProviderJet & "Data Source=" & strBackup & ";", ProviderJet & "Data Source=" & strFilename & ";Jet OLEDB:Encrypt Database=True")
    '        '--- В случае успеха удалить резервную копию. ---------------------
    '        File.Delete(strBackup)
    '        Application.DoEvents()
    '    Catch ex As Exception
    '        Const caption As String = "Ошибка восстановления базы данных при сжатии"
    '        Dim text As String = ex.ToString
    '        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
    '    Finally
    '        Beep()
    '    End Try
    'End Sub
#End Region

    ''' <summary>
    ''' Создание каталога с названием дат записей,
    ''' копирование базы данных
    ''' </summary>
    ''' <param name="appointment"></param>
    Private Sub CreateEngineFolder(appointment As String)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() CreateEngineFolder(appointment)))
        Else
            ' создание папки с датой и временем
            Dim descriptionTimePeriod As String
            If isEnvokeFromTreeView Then
                descriptionTimePeriod = $" записи с {ListViewSnapshot.Items(0).Text} ({Replace(Trim(ListViewSnapshot.Items(0).SubItems(1).Text), ":", "-")}) по {ListViewSnapshot.Items(ListViewSnapshot.Items.Count - 1).Text} ({Replace(Trim(ListViewSnapshot.Items(ListViewSnapshot.Items.Count - 1).SubItems(1).Text), ":", "-")})"
            Else
                descriptionTimePeriod = $" записи с {ListViewSnapshot.SelectedItems(0).Text} ({Replace(Trim(ListViewSnapshot.SelectedItems(0).SubItems(1).Text), ":", "-")}) по {ListViewSnapshot.SelectedItems(ListViewSnapshot.SelectedItems.Count - 1).Text} ({Replace(Trim(ListViewSnapshot.SelectedItems(ListViewSnapshot.SelectedItems.Count - 1).SubItems(1).Text), ":", "-")})"
            End If

            descriptionTimePeriod = $"{numberEngine.ToString & descriptionTimePeriod} {appointment} произведено {Today.ToShortDateString} ({Now.Hour}ч{Now.Minute}м{Now.Second}с)"
            Dim pathCopyFolder As String = Path.Combine(CreateFolderArchive, descriptionTimePeriod)
            Directory.CreateDirectory(pathCopyFolder) ' создать папку перед переносом
            fileReceiver = Path.Combine(pathCopyFolder, numberEngine.ToString & ".mdb")
            catalogReceiver = VB.Left(fileReceiver, InStrRev(fileReceiver, Separator))
        End If
    End Sub

    ''' <summary>
    ''' Проверка существования или создание папки "Архивы" 
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateFolderArchive() As String
        Dim pathArchive As String = Path.Combine(Directory.GetDirectoryRoot(PathChannels), "Архивы")

        If Not Directory.Exists(pathArchive) Then ' если нет то создание в папке "Архивы" новой папки
            'MessageBox.Show("В корне нет папки Архивы!", "Проверка свободного места", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Directory.CreateDirectory(pathArchive)
        End If

        Return pathArchive
    End Function

    ''' <summary>
    ''' Экспорт в Excel выделенных снимков
    ''' </summary>
    Private Sub ExportSelectedSnapshotsToExcel()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Экспорт Excel снимков")
        Dim I As Integer

        FolderBrowserDialog1.Description = "Выберите папку для экспорта снимков"

        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            PathExportFolderXLS = FolderBrowserDialog1.SelectedPath
            Re.Dim(IndexForGroupExport, ListViewSnapshot.SelectedItems.Count - 1)

            For Each itemListView As ListView In ListViewSnapshot.SelectedItems()
                IndexForGroupExport(I) = CInt(Val(itemListView.Tag))
                I += 1
            Next

            GKeyID = CInt(Val(ListViewSnapshot.SelectedItems(0).Tag))
            IsGroupExportSnapshot = True
            Close()
        End If
    End Sub

    ''' <summary>
    ''' Объединить выделенные снимки в первый
    ''' </summary>
    Private Sub JoinSelectedSnapshots()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Объединить Выделенные Снимки")
        Dim I As Integer

        For Each selectedItem As ListViewItem In ListViewSnapshot.SelectedItems
            If CBool(InStr(1, selectedItem.SubItems(3).Text, "Объединённая")) Then
                Const caption As String = "Ошибка объединения снимков"
                Const text As String = "Невозможно объединять уже слитые снимки"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If
        Next

        If ListViewSnapshot.SelectedItems.Count > 1 Then
            Re.Dim(IndexForMergerSnapshot, ListViewSnapshot.SelectedItems.Count - 1)

            For Each selectedItem As ListViewItem In ListViewSnapshot.SelectedItems()
                IndexForMergerSnapshot(I) = CInt(Val(selectedItem.Tag))
                I += 1
            Next

            GKeyID = CInt(Val(ListViewSnapshot.SelectedItems(0).Tag))
            IsUniteDetachedSnapshot = True
            Close()
        End If
    End Sub

#Region "MenuItem Events"
    Private Sub ContextMenuStripTreeView_Opened(ByVal sender As Object, ByVal e As EventArgs) Handles ContextMenuStripTreeView.Opened, TSMenuItemActionsTreeView.DropDownOpened
        isEnvokeFromTreeView = True
    End Sub

    Private Sub ContextMenuStripListView_Opened(ByVal sender As Object, ByVal e As EventArgs) Handles ContextMenuStripListView.Opened, TSMenuItemActionsListView.DropDownOpened
        isEnvokeFromTreeView = False
        TSMenuItemJoinSelectSnapshot.Enabled = ListViewSnapshot.SelectedItems.Count > 1 AndAlso isUseTdmsRead = False
        TSActionMenuItemJoinSelectSnapshot.Enabled = TSMenuItemJoinSelectSnapshot.Enabled
    End Sub

    Private Sub TSMenuItemDeleteSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemDeleteSelect1.Click,
                                                                                                    TSActionMenuItemDeleteSelect1.Click,
                                                                                                    TSMenuItemDeleteSelect2.Click,
                                                                                                    TSActionMenuItemDeleteSelect2.Click
        ' обработка удаления KeyID а затем присвоить 0
        If sender Is TSMenuItemDeleteSelect1 OrElse sender Is TSActionMenuItemDeleteSelect1 Then
            DeleteOneSnapshotFromTreeView()
        ElseIf sender Is TSMenuItemDeleteSelect2 OrElse sender Is TSActionMenuItemDeleteSelect2 Then
            DeleteSelectedSnapshotsFromListView()
        End If
        ' после этого обновление дерева
        UpdateForm()
    End Sub

    Private Sub TSMenuItemDeleteAll1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemDeleteAll1.Click, TSActionMenuItemDeleteAll1.Click
        Dim caption As String = "Удаление изделия №" & numberEngine
        Dim text As String = "Подтвердите удаление изделия " & numberEngine

        RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")
        If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            ' по номеру двигателя запрос на удаление
            DeleteAllSnapshots()
            ' после этого обновление дерева
            UpdateForm()
        End If
    End Sub
    Private Sub TSMenuItemRemoveToArchive_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemRemoveToArchive1.Click, TSActionMenuItemRemoveToArchive1.Click
        RemoveToArchiveAsync()
    End Sub

    Private Sub TSMenuItemAvtomaticArchives_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemAvtomaticArchives.Click, TSActionMenuItemAvtomaticArchives.Click
        AutomaticArchives()
    End Sub

    Private Sub TSMenuItemExportSnapshotToExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemExportSnapshotToExcel.Click, TSActionMenuItemExportSnapshotToExcel.Click
        ExportSelectedSnapshotsToExcel()
    End Sub

    ''' <summary>
    ''' Подготовить индексы строк для последующего слияния
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TSMenuItemJoinSelectSnapshot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemJoinSelectSnapshot.Click, TSActionMenuItemJoinSelectSnapshot.Click
        JoinSelectedSnapshots()
    End Sub
#End Region

#Region "ToolStripMenuItem Events"
    ''' <summary>
    ''' Преключить видимость toolstrip, а также установить ассоциированные пункты меню
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        ToolBarToolStripMenuItem.Checked = Not ToolBarToolStripMenuItem.Checked
        ToolStrip.Visible = ToolBarToolStripMenuItem.Checked
    End Sub

    ''' <summary>
    ''' Преключить видимость  statusstrip, а также установить ассоциированные пункты меню
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        StatusBarToolStripMenuItem.Checked = Not StatusBarToolStripMenuItem.Checked
        StatusStripMessage.Visible = StatusBarToolStripMenuItem.Checked
    End Sub

    ''' <summary>
    ''' проверить устанавливать или нет видимым панель папок
    ''' </summary>
    Private Sub ToggleFoldersVisible()
        ' вначале установить чек состояния ассоциированного меню
        FoldersToolStripMenuItem.Checked = Not FoldersToolStripMenuItem.Checked
        ' синхронизировать кнопку с папками 
        FoldersToolStripButton.Checked = FoldersToolStripMenuItem.Checked
        ' свернуть панель содержащую TreeView.
        SplitContainer.Panel1Collapsed = Not FoldersToolStripMenuItem.Checked
    End Sub

    Private Sub FoldersToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles FoldersToolStripMenuItem.Click
        ToggleFoldersVisible()
    End Sub

    Private Sub FoldersToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles FoldersToolStripButton.Click
        ToggleFoldersVisible()
    End Sub

    Private Sub SetView(ByVal View As View)
        ' определить, какой пункт меню должен быть отмечен
        Dim MenuItemToCheck As ToolStripMenuItem

        Select Case View
            Case View.Details
                MenuItemToCheck = DetailsToolStripMenuItem
                Exit Select
            Case View.LargeIcon
                MenuItemToCheck = LargeIconsToolStripMenuItem
                Exit Select
            Case View.List
                MenuItemToCheck = ListToolStripMenuItem
                Exit Select
            Case View.SmallIcon
                MenuItemToCheck = SmallIconsToolStripMenuItem
                Exit Select
            Case View.Tile
                MenuItemToCheck = TileToolStripMenuItem
                Exit Select
            Case Else
                Debug.Fail("Unexpected View")
                View = View.Details
                MenuItemToCheck = DetailsToolStripMenuItem
                Exit Select
        End Select

        ' проверить подходящий пункт меню и снять все другие нижележащие Views меню
        For Each MenuItem As ToolStripMenuItem In ListViewToolStripButton.DropDownItems
            If MenuItem Is MenuItemToCheck Then
                MenuItem.Checked = True
            Else
                MenuItem.Checked = False
            End If
        Next

        ' В конце, установить требуемый вид
        ListViewSnapshot.View = View
    End Sub

    Private Sub ListToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ListToolStripMenuItem.Click
        SetView(View.List)
    End Sub

    Private Sub DetailsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DetailsToolStripMenuItem.Click
        SetView(View.Details)
    End Sub

    Private Sub LargeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LargeIconsToolStripMenuItem.Click
        SetView(View.LargeIcon)
    End Sub

    Private Sub SmallIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SmallIconsToolStripMenuItem.Click
        SetView(View.SmallIcon)
    End Sub

    Private Sub TileToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileToolStripMenuItem.Click
        SetView(View.Tile)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click
        'Dim OpenFileDialog As New OpenFileDialog
        'OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        'OpenFileDialog.Filter = "Text Files (*.txt)|*.txt"
        'OpenFileDialog.ShowDialog(Me)
        'Dim FileName As String = OpenFileDialog.FileName

        ' доступно только при автономном режиме
        With MainMdiForm.OpenFileDialog1
            .FileName = vbNullString
            .Title = "Текущий каталог базы-> " & PathChannels
            .DefaultExt = "mdb"
            ' установить флаг атрибутов
            .Filter = "База испытаний (*.mdb)|*.mdb"
            .RestoreDirectory = True

            If .ShowDialog() = DialogResult.OK AndAlso Len(.FileName) <> 0 Then
                PathChannels = .FileName
                'ПроверкаПравильностиПути()
                ' записать путь к рабочей базе
                WriteINI(PathOptions, "TheCurrentBase", "Base", PathChannels)
                IsDataBaseChanged = True
            End If
        End With

        UpdateForm()
    End Sub

    'Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
    '    Dim SaveFileDialog As New SaveFileDialog() With {.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments,
    '                                                    .Filter = "Text Files (*.txt)|*.txt"}
    '    SaveFileDialog.ShowDialog(Me)
    '    'Dim FileName As String = SaveFileDialog.FileName
    'End Sub

    'Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
    'If (result = DialogResult.OK) Then
    '    folderName = FolderBrowserDialog1.SelectedPath
    '    If (Not fileOpened) Then
    '        ' No file is opened, bring up openFileDialog in selected path.
    '        openFileDialog1.InitialDirectory = folderName
    '        openFileDialog1.FileName = Nothing
    '        openMenuItem.PerformClick()
    '    End If
    'End If
#End Region
End Class


'Private Async Sub RemoveToArchive()
'    Await RemoveToArchiveAsync()
'End Sub

'Private Function RemoveToArchiveAsync() As Tasks.Task
'    Return Tasks.Task.Run(Sub()
'                              RemoveToArchiveTask()
'                          End Sub)
'End Function

'Invoke(New MethodInvoker(Sub()
'                           await Task.Delay(5000)
'                     End Sub))



'Private Function RemoveAllSSnapshotsToArchiveFromTreeView() As Boolean
'    Return RemoveAllSSnapshotsToArchiveFromTreeViewAsync().Result
'End Function

'Private Async Function RemoveAllSSnapshotsToArchiveFromTreeViewAsync() As Task(Of Boolean)
'    Return Await Tasks.Task(Of Boolean).Run(Function()
'                                                Return RemoveAllSSnapshotsToArchiveFromTreeViewTask()
'                                            End Function)

'    '    'Dim task2 As Task(Of Boolean) = Task.Factory.StartNew(Function()
'    '    '                                                          Return RemoveAllSSnapshotsToArchiveFromTreeViewTask()
'    '    '            
'End Function


'Private Function RemoveAllSSnapshotsToArchiveFromTreeView() As Boolean
'    Return RemoveAllSSnapshotsToArchiveFromTreeViewAsync()
'End Function

'Private Function RemoveAllSSnapshotsToArchiveFromTreeViewAsync() As Boolean
'    'Return Task.Run(Function()
'    '                    Return RemoveAllSSnapshotsToArchiveFromTreeViewTask()
'    '                End Function).Result

'    Return Task(Of Boolean).Factory.StartNew(Function()
'                                                 Return RemoveAllSSnapshotsToArchiveFromTreeViewTask()
'                                             End Function).Result
'End Function