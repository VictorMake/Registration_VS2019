Imports System.Data.OleDb

''' <summary>
''' Класс заменен в проекте FormTuningSelectiveBase
''' </summary>
Friend Class FormTuningSelectiveList
    '    'Public WriteOnly Property FormParent() As FormMain
    '    '    Set(ByVal Value As FormMain)
    '    '        mFormParent = Value
    '    '    End Set
    '    'End Property
    '    Private mFormParent As FormMain
    '    Private ListSelectively As String ' вставил при отключении данной формы

    '    Private Structure TypeTuningMode
    '        Dim номерРежима As Short
    '        Dim наименование As String
    '        Dim переченьПараметров As String
    '    End Structure
    '    Private arrTypeTuningMode() As TypeTuningMode

    '    Private списокИмен() As String
    '    Private формаЗагружена As Boolean = False
    '    Private lvSelectedIndices, lvPreviousSelectedIndices As Integer
    '    Private lastIdConfig As Integer

    '    Private img As Bitmap
    '    Private hotSpot As Point

    '    Public Sub New(FormParent As FormMain)
    '        ' Этот вызов является обязательным для конструктора.
    '        InitializeComponent()
    '        ' Добавить все инициализирующие действия после вызова InitializeComponent().

    '        mFormParent = FormParent
    '        InitializeForm()
    '    End Sub

    '    Private Sub InitializeForm()
    '        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
    '        InitializeListViews()
    '        ЗагрузкаРежимов()
    '        ОтобразитьРежимы()
    '        ' считывание строки из файла
    '        ' считать из файла строку с параметрами контроля и расшифровать ее в массив
    '        ' из массива на лист
    '        ' ОтобразитьРежимыНаЛистах(sGetIni(PathOptions, "ConfigList", strStendN, "N1\"))
    '        ЗагрузитьКонфигурацииСписков()
    '        cmdВосстановитьСписки_Click(cmdВосстановитьСписки, New EventArgs)
    '        ' cmdВосстановитьСписки.PerformClick()'не срабатывает
    '        формаЗагружена = True
    '        frmНастройкаЛиста_Resize(Me, New EventArgs)
    '    End Sub

    '    Private Sub InitializeListViews()
    '        lvwSource.Items.Clear()
    '        lvwSource.Columns.Clear()
    '        lvwSource.Columns.Add("Параметр", lvwSource.Width \ 2 - 4, HorizontalAlignment.Left)
    '        lvwSource.Columns.Add("Назначение", lvwSource.Width \ 2 - 4, HorizontalAlignment.Left)
    '        lvwReceiver.Items.Clear()
    '        lvwReceiver.Columns.Clear()
    '        lvwReceiver.Columns.Add("Параметр", lvwReceiver.Width \ 2 - 4, HorizontalAlignment.Left)
    '        lvwReceiver.Columns.Add("Назначение", lvwReceiver.Width \ 2 - 4, HorizontalAlignment.Left)
    '    End Sub

    '    Private Sub FormTuningList_Load(sender As Object, e As EventArgs) Handles Me.Load
    '        AllowDrop = True
    '        lvwSource.AllowDrop = True
    '        lvwReceiver.AllowDrop = True

    '        ' избавиться от мерцания
    '        DoubleBuffered = True
    '    End Sub

    '    Private Sub ОтобразитьРежимы()
    '        Dim I, J As Integer

    '        lvwSource.BeginUpdate()
    '        lvwSource.Items.Clear()
    '        lvwReceiver.Items.Clear()

    '        ' заполнить первый лист
    '        If IsDataBaseChanged Then
    '            For I = 1 To UBound(ParametersShaphotType)
    '                Dim newListViewItem As ListViewItem = New ListViewItem(ParametersShaphotType(I).NameParameter)
    '                newListViewItem.SubItems.Add(ParametersShaphotType(I).Description)

    '                If InStr(1, UnitOfMeasureString, ParametersShaphotType(I).UnitOfMeasure) Then
    '                    newListViewItem.ImageIndex = Array.IndexOf(UnitOfMeasureArray, ParametersShaphotType(I).UnitOfMeasure)
    '                Else
    '                    newListViewItem.ImageIndex = 6
    '                End If

    '                newListViewItem.ForeColor = Color.Red ' красный как будто его нет
    '                lvwSource.Items.Add(newListViewItem)

    '                For J = 1 To UBound(CopyListOfParameter)
    '                    ' проверить на совпадение номеров и пометить параметр который есть в конфигурации
    '                    If CopyListOfParameter(J) = I Then
    '                        lvwSource.Items(I - 1).ForeColor = Color.Black ' черный
    '                        Exit For
    '                    End If
    '                Next J
    '            Next I
    '        Else
    '            For I = 1 To UBound(ParametersType)
    '                Dim newListViewItem As ListViewItem = New ListViewItem(ParametersType(I).NameParameter)
    '                newListViewItem.SubItems.Add(ParametersType(I).Description)

    '                If InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure) Then
    '                    newListViewItem.ImageIndex = Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)
    '                Else
    '                    newListViewItem.ImageIndex = 6
    '                End If

    '                newListViewItem.ForeColor = Color.Red  ' красный как будто его нет
    '                lvwSource.Items.Add(newListViewItem)

    '                For J = 1 To UBound(CopyListOfParameter)
    '                    ' проверить на совпадение номеров и пометить параметр который есть в конфигурации
    '                    If CopyListOfParameter(J) = I Then
    '                        lvwSource.Items(I - 1).ForeColor = Color.Black ' черный
    '                        Exit For
    '                    End If
    '                Next J
    '            Next I
    '        End If

    '        lvwSource.EndUpdate()
    '    End Sub

    '    Private Sub ОтобразитьРежимыНаЛистах(ByVal конфигурация As String)
    '        Dim I, J As Integer
    '        Dim arrНаименованиеОчищен() As String

    '        ReDim_arrНаименованиеОчищен(0)
    '        DecryptionString(конфигурация)
    '        lvwSource.BeginUpdate()
    '        lvwReceiver.BeginUpdate()

    '        ' заполнить очищенный массив (параметра может не быть) по количеству, если найдены
    '        If IsDataBaseChanged Then
    '            For I = 1 To UBound(NameParam)
    '                For J = 1 To UBound(ParametersShaphotType)
    '                    If ParametersShaphotType(J).NameParameter = NameParam(I) Then
    '                        Dim newListViewItem As ListViewItem = New ListViewItem("NULL")
    '                        newListViewItem.SubItems.Add("NULL")
    '                        lvwReceiver.Items.Add(newListViewItem)
    '                        ReDimPreserve arrНаименованиеОчищен(UBound(arrНаименованиеОчищен) + 1)
    '                        arrНаименованиеОчищен(UBound(arrНаименованиеОчищен)) = NameParam(I)
    '                        Exit For
    '                    End If
    '                Next J
    '            Next I
    '        Else
    '            For I = 1 To UBound(NameParam)
    '                For J = 1 To UBound(ParametersType)
    '                    If ParametersType(J).NameParameter = NameParam(I) Then
    '                        Dim newListViewItem As ListViewItem = New ListViewItem("NULL")
    '                        newListViewItem.SubItems.Add("NULL")
    '                        lvwReceiver.Items.Add(newListViewItem)
    '                        ReDimPreserve arrНаименованиеОчищен(UBound(arrНаименованиеОчищен) + 1)
    '                        arrНаименованиеОчищен(UBound(arrНаименованиеОчищен)) = NameParam(I)
    '                        Exit For
    '                    End If
    '                Next J
    '            Next I
    '        End If

    '        ' заполнить по содержанию второй лист
    '        Dim List1ItemsCount, List2ItemsCount As Integer

    '        For Each itemListView As ListViewItem In lvwSource.Items
    '            List1ItemsCount += 1

    '            If List1ItemsCount > lvwSource.Items.Count Then Exit For

    '            'System.Diagnostics.Debug.WriteLine(itmXLoop.Text & "  " & List1ItemsCount.ToString) 
    '            For J = 1 To UBound(arrНаименованиеОчищен)
    '                If itemListView.Text = arrНаименованиеОчищен(J) Then
    '                    Dim tempListViewItem As ListViewItem = lvwReceiver.Items(J - 1)
    '                    tempListViewItem.Text = itemListView.Text
    '                    tempListViewItem.SubItems(1).Text = itemListView.SubItems(1).Text
    '                    tempListViewItem.ImageIndex = itemListView.ImageIndex
    '                    Exit For
    '                End If
    '            Next J
    '        Next

    '        List1ItemsCount = 0
    '        ' удалить из первого листа повторы
    '        For Each itemListView As ListViewItem In lvwReceiver.Items
    '            List2ItemsCount += 1
    '            If List2ItemsCount > lvwReceiver.Items.Count Then Exit For

    '            For Each newListViewItem As ListViewItem In lvwSource.Items
    '                List1ItemsCount += 1
    '                If List1ItemsCount > lvwSource.Items.Count Then Exit For

    '                If newListViewItem.Text = itemListView.Text Then
    '                    lvwSource.Items.Remove(newListViewItem) ' требует объект, а не индекс
    '                    Exit For
    '                End If
    '            Next newListViewItem

    '            List1ItemsCount = 0
    '        Next

    '        lvwSource.EndUpdate()
    '        lvwReceiver.EndUpdate()
    '    End Sub

    '    Private Sub ЗагрузкаРежимов()
    '        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '        Dim rdr As OleDbDataReader
    '        Dim cmd As OleDbCommand = cn.CreateCommand
    '        Const имяРежима As String = "ТекстовыйКонтроль"
    '        Dim strSQL As String = $"SELECT COUNT(*) FROM [Режимы{StandNumber}] WHERE [Наименование]<> '{имяРежима}'"

    '        cmd.CommandType = CommandType.Text
    '        cmd.CommandText = strSQL
    '        cn.Open()
    '        ReDim_arrTypeTuningMode(CInt(cmd.ExecuteScalar))

    '        strSQL = $"SELECT * FROM [Режимы{StandNumber}] WHERE [Наименование]<> '{имяРежима}'"
    '        cmd.CommandText = strSQL
    '        rdr = cmd.ExecuteReader

    '        ' загрузка коэффициентов по параметрам с базы с помощью запроса
    '        ' при добавлении полей надо модифицировать запрос в базе
    '        Dim index As Integer
    '        Do While rdr.Read
    '            index = rdr("НомерРежима")
    '            arrTypeTuningMode(index).номерРежима = rdr("НомерРежима")
    '            arrTypeTuningMode(index).наименование = rdr("Наименование")
    '            arrTypeTuningMode(index).переченьПараметров = rdr("ПереченьПараметров")
    '        Loop

    '        rdr.Close()
    '        cn.Close()

    '        For I As Integer = 1 To UBound(arrTypeTuningMode)
    '            cmbВыборРежима.Items.Add(arrTypeTuningMode(I).наименование)
    '        Next

    '        cmbВыборРежима.SelectedIndex = 0 ' по умолчанию первый злемент активный
    '    End Sub

    '    Private Sub cmbВыборРежима_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbВыборРежима.SelectedIndexChanged
    '        ОтобразитьРежимы()
    '        ОтобразитьРежимыНаЛистах(arrTypeTuningMode(cmbВыборРежима.SelectedIndex + 1).переченьПараметров)
    '    End Sub

    '    Private Sub frmНастройкаЛиста_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
    '        If формаЗагружена Then
    '            lvwSource.Columns(0).Width = lvwSource.Width \ 2 - 4
    '            lvwSource.Columns(1).Width = lvwSource.Width \ 2 - 4
    '            lvwReceiver.Columns(0).Width = lvwReceiver.Width \ 2 - 4
    '            lvwReceiver.Columns(1).Width = lvwReceiver.Width \ 2 - 4
    '        End If
    '    End Sub

    '    Private Sub Переставить(ByVal intТекущий As Integer, ByVal intЗаданный As Integer)
    '        Dim targetListViewItem, currentListViewItem As ListViewItem
    '        Dim text As String
    '        Dim description As String
    '        Dim indexIcon As Integer

    '        With lvwReceiver
    '            targetListViewItem = .Items(intЗаданный)
    '            currentListViewItem = .Items(intТекущий)

    '            ' запомнить предыдущего
    '            text = targetListViewItem.Text
    '            description = targetListViewItem.SubItems(1).Text
    '            indexIcon = targetListViewItem.ImageIndex

    '            ' запись перемещаемого в предыдущий
    '            targetListViewItem.Text = currentListViewItem.Text
    '            targetListViewItem.SubItems(1).Text = currentListViewItem.SubItems(1).Text
    '            targetListViewItem.ImageIndex = currentListViewItem.ImageIndex

    '            ' перезапись в перемещаемый сохраненные
    '            currentListViewItem.Text = text
    '            currentListViewItem.SubItems(1).Text = description
    '            currentListViewItem.ImageIndex = indexIcon

    '            ' выделить
    '            .Items(intТекущий).Selected = False
    '            targetListViewItem.EnsureVisible()
    '            .Items(intЗаданный).Selected = True
    '        End With
    '    End Sub

    '    Private Sub LVSelectedIndicesValueChanged()
    '        'MessageBox.Show("ОбновитьГрафик не работает пока нет записи")
    '        If lvwReceiver.SelectedIndices.Count = 0 Then Exit Sub

    '        lvwReceiver.Focus()

    '        If lvPreviousSelectedIndices < lvSelectedIndices Then ' вверх
    '            Dim selectedIndex, indexPrevious As Integer
    '            With lvwReceiver
    '                If .Items.Count > 0 Then
    '                    selectedIndex = .SelectedIndices(.SelectedIndices.Count - 1)
    '                    If .SelectedIndices.Count <> 0 Then
    '                        If selectedIndex <> 0 Then
    '                            indexPrevious = selectedIndex - 1
    '                            Переставить(selectedIndex, indexPrevious)
    '                        End If
    '                    End If
    '                End If
    '            End With
    '        Else
    '            Dim selectedIndex, indexNext As Integer
    '            With lvwReceiver
    '                If .Items.Count > 0 Then
    '                    selectedIndex = .SelectedIndices(.SelectedIndices.Count - 1)
    '                    If .SelectedIndices.Count <> 0 Then
    '                        If selectedIndex <> .Items.Count - 1 Then
    '                            indexNext = selectedIndex + 1
    '                            Переставить(selectedIndex, indexNext)
    '                        End If
    '                    End If
    '                End If
    '            End With
    '        End If
    '    End Sub

    '    '************************************
    '    'Private Sub ЗаписатьСписокВБазу(ByRef Структура As String)
    '    '    Dim strSQL As String
    '    '    Dim I As Short
    '    '    Dim itmXтекущий As ListViewItem
    '    '    Dim strИмяСписки As String
    '    '    Dim blnИмяСпискиЕсть As Boolean
    '    '    Dim cn As OleDbConnection
    '    '    Dim odaDataAdapter As OleDbDataAdapter
    '    '    Dim dtDataTable As New DataTable
    '    '    Dim drDataRow As DataRow
    '    '    Dim cb As OleDbCommandBuilder

    '    '    If Структура <> "" Then
    '    '        strИмяСписки = comСписки.Text
    '    '        'если имя пусто вопрос  "введите имя" и так как модально нельзя то выход из данной процедуры
    '    '        If strИмяСписки <> "" Then
    '    '            'если имя новое, то новая перекладка добавляется
    '    '            'если переписать то удаление из базы старых и запись новых
    '    '            'если добавить то запись в базу, очистка comСписки и считывание в лист заново
    '    '            For I = 1 To UBound(arrИмяСписки)
    '    '                If arrИмяСписки(I) = strИмяСписки Then
    '    '                    blnИмяСпискиЕсть = True
    '    '                    Exit For
    '    '                End If
    '    '            Next I
    '    '            If blnИмяСпискиЕсть Then
    '    '                ' Открыть подключение
    '    '                strSQL = "SELECT Списки.* FROM Списки WHERE Name= '" & strИмяСписки & "'"
    '    '            Else
    '    '                strSQL = "SELECT Списки.* FROM Списки"
    '    '            End If
    '    '            ' Строки множества
    '    '            cn = New OleDbConnection(BuildCnnStr(strProviderJet, PathChannels))
    '    '            ' Создание recordset
    '    '            cn.Open()
    '    '            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
    '    '            odaDataAdapter.Fill(dtDataTable)
    '    '            Try
    '    '                If blnИмяСпискиЕсть Then 'удаляется старая группа
    '    '                    If dtDataTable.Rows.Count > 0 Then
    '    '                        'dtDataTable.Rows(0).Delete()
    '    '                        'dtDataTable.Rows(0)("Name") = strИмяСписки 'имя
    '    '                        dtDataTable.Rows(0)("Text") = Структура
    '    '                        drDataRow = dtDataTable.Rows(0)

    '    '                        cb = New OleDbCommandBuilder(odaDataAdapter)
    '    '                        Dim mUpdateCommand As OleDb.OleDbCommand = cn.CreateCommand
    '    '                        'drDataRow = dtDataTable.Rows(0)
    '    '                        '        strSQL = "Update Режимы" & StandNumber & " SET ПереченьПараметров = '" & strСтрокаКонфигурации & "' WHERE ([НомерРежима]= " & ShНомерРежима.ToString & ")"

    '    '                        'mUpdateCommand.CommandText = "UPDATE Списки SET Name = '" & drDataRow("Name") & "' , Text =  '" & drDataRow("Text") & "' WHERE IdKey = " & drDataRow("IdKey")
    '    '                        mUpdateCommand.CommandText = "UPDATE Списки SET Text =  '" & drDataRow("Text") & "' WHERE (IdKey = " & drDataRow("IdKey") & ")"
    '    '                        odaDataAdapter.UpdateCommand = mUpdateCommand 'cb.GetUpdateCommand
    '    '                        odaDataAdapter.Update(dtDataTable)
    '    '                        dtDataTable.AcceptChanges()
    '    '                        'odaDataAdapter.UpdateCommand.Connection.Close()

    '    '                    End If
    '    '                Else 'добавить в комбо и в массив
    '    '                    comСписки.Items.Add(strИмяСписки)
    '    '                    ReDimPreserve arrИмяСписки(UBound(arrИмяСписки) + 1)
    '    '                    arrИмяСписки(UBound(arrИмяСписки)) = strИмяСписки

    '    '                    'добавляется новая перекладка или перезаписывается старая
    '    '                    drDataRow = dtDataTable.NewRow
    '    '                    drDataRow.BeginEdit()
    '    '                    drDataRow("Name") = strИмяСписки 'имя
    '    '                    drDataRow("Text") = Структура
    '    '                    drDataRow.EndEdit()
    '    '                    dtDataTable.Rows.Add(drDataRow)
    '    '                End If

    '    '                'cb = New OleDbCommandBuilder(odaDataAdapter)
    '    '                'odaDataAdapter.Update(dtDataTable)

    '    '                'Catch ex As System.Data.OleDb.OleDbException
    '    '                '    Try
    '    '                '        'Почему-то первая запись показывает ошибку параллелелизма
    '    '                '        'Console.WriteLine(ex.ToString)
    '    '                '        If dtDataTable.HasErrors Then
    '    '                '            Dim mUpdateCommand As OleDb.OleDbCommand = cn.CreateCommand
    '    '                '            For Each drDataRow In dtDataTable.GetErrors
    '    '                '                If drDataRow.RowState = DataRowState.Modified Then
    '    '                '                    'Console.WriteLine(drDataRow("KeyID"))
    '    '                '                    mUpdateCommand.CommandText = "UPDATE Списки SET Name = '" & drDataRow("Name") & "' , Text =  '" & drDataRow("Text") & "' WHERE Id = " & drDataRow("Id")
    '    '                '                    'mUpdateCommand.CommandText = "UPDATE БазаСнимков SET ПутьНаДиске = '" & drDataRow("ПутьНаДиске") & "' WHERE KeyID = " & drDataRow("KeyID")
    '    '                '                    odaDataAdapter.UpdateCommand = mUpdateCommand
    '    '                '                    Dim arrDataRow() As DataRow = {drDataRow}
    '    '                '                    odaDataAdapter.Update(arrDataRow)
    '    '                '                End If
    '    '                '                If drDataRow.RowState = DataRowState.Added Then
    '    '                '                    'Console.WriteLine(drDataRow("KeyID"))
    '    '                '                    mUpdateCommand.CommandText = "INSERT INTO Списки( Name , Text ) VALUES ( '" & strИмяСписки & "' , '" & Структура & "')"
    '    '                '                    odaDataAdapter.UpdateCommand = mUpdateCommand
    '    '                '                    Dim arrDataRow() As DataRow = {drDataRow}
    '    '                '                    odaDataAdapter.Update(arrDataRow)
    '    '                '                End If

    '    '                '            Next
    '    '                '        End If

    '    '                '    Catch ex2 As Exception
    '    '                '        MessageBox.Show(ex2.ToString, "Запись списка в базу", MessageBoxButtons.OK, MessageBoxIcon.Warning)

    '    '                '    End Try


    '    '            Catch ex As Exception
    '    '                MessageBox.Show(ex.ToString, "Запись списка в базу", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    '            Finally
    '    '                cn.Close()
    '    '            End Try
    '    '        Else 'надо ввести имя
    '    '            MessageBox.Show("Введите имя", "Список", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    '        End If
    '    '    End If
    '    'End Sub

    '    Private Sub ЗаписатьСписокВБазу(ByVal упаковка As String)
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись выборочного списка в базу")

    '        Dim strSQL As String
    '        Dim имяСписки As String
    '        Dim имяСпискаНайден As Boolean
    '        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '        Dim cmd As OleDbCommand = cn.CreateCommand
    '        Dim rdr As OleDbDataReader

    '        cmd.CommandType = CommandType.Text

    '        If упаковка <> "" Then
    '            имяСписки = comСписки.Text
    '            ' если имя пусто вопрос  "введите имя" и так как модально нельзя то выход из данной процедуры
    '            If имяСписки <> "" Then
    '                ' если имя новое, то новая перекладка добавляется
    '                ' если переписать то удаление из базы старых и запись новых
    '                ' если добавить то запись в базу, очистка comСписки и считывание в лист заново
    '                For I As Integer = 1 To UBound(списокИмен)
    '                    If списокИмен(I) = имяСписки Then
    '                        имяСпискаНайден = True
    '                        Exit For
    '                    End If
    '                Next

    '                If имяСпискаНайден Then
    '                    ' Открыть подключение
    '                    strSQL = $"UPDATE Списки SET [Text] =  '{упаковка}' WHERE ( Name = '{имяСписки}')"
    '                Else
    '                    strSQL = $"INSERT INTO Списки( Name , [Text] ) VALUES ( '{имяСписки}' , '{упаковка}')"
    '                    ReDimPreserve списокИмен(списокИмен.Count)
    '                    списокИмен(списокИмен.Count - 1) = имяСписки
    '                    comСписки.Items.Add(имяСписки)
    '                End If

    '                Try
    '                    cmd.CommandText = strSQL
    '                    cn.Open()
    '                    cmd.ExecuteNonQuery()
    '                    Threading.Thread.Sleep(200)
    '                    Application.DoEvents()
    '                    ' теперь узнать Id с записи с именем ИмяСписки
    '                    strSQL = $"SELECT Списки.* FROM Списки WHERE (((Списки.Name)='{имяСписки}'));"
    '                    cmd.CommandText = strSQL
    '                    rdr = cmd.ExecuteReader

    '                    If rdr.Read Then
    '                        lastIdConfig = rdr("Id")
    '                    End If

    '                    rdr.Close()
    '                Catch ex As Exception
    '                    Const caption As String = "Запись списка в базу"
    '                    Dim text As String = ex.ToString
    '                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
    '                Finally
    '                    cn.Close()
    '                End Try
    '            Else ' надо ввести имя
    '                MessageBox.Show("Введите имя", "Список", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            End If
    '        End If
    '    End Sub

    '    Private Sub ЗагрузитьКонфигурацииСписков()
    '        Dim intКолИмен As Short
    '        'Dim strSQL As String = "SELECT DISTINCT Name FROM Списки"
    '        Const strSQL As String = "SELECT * FROM Списки ORDER BY Id"
    '        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '        Dim odaDataAdapter As OleDbDataAdapter
    '        Dim dtDataTable As New DataTable
    '        Dim drDataRow As DataRow
    '        Dim success As Boolean
    '        Dim selectedIndex As Integer

    '        Try
    '            lastIdConfig = CInt(GetIni(PathOptions, "ConfigList", "LastIdСписки", "0"))
    '            cn.Open()
    '            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
    '            odaDataAdapter.Fill(dtDataTable)
    '            intКолИмен = dtDataTable.Rows.Count

    '            If intКолИмен <> 0 Then
    '                ReDim_списокИмен(intКолИмен)
    '            Else
    '                ReDim_списокИмен(1)
    '            End If

    '            Dim index As Integer = 1
    '            comСписки.Items.Clear()

    '            For Each drDataRow In dtDataTable.Rows
    '                списокИмен(index) = drDataRow("Name")
    '                comСписки.Items.Add(списокИмен(index))

    '                If drDataRow("Id") = lastIdConfig Then
    '                    success = True
    '                    selectedIndex = index - 1
    '                End If

    '                index += 1
    '            Next

    '            cn.Close()
    '        Catch ex As Exception
    '            Const caption As String = "Загрузка конфигурации"
    '            Dim text As String = ex.ToString
    '            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
    '        Finally
    '            comСписки.Focus()

    '            If success Then
    '                comСписки.SelectedIndex = selectedIndex
    '            Else
    '                comСписки.SelectedIndex = comСписки.Items.Count - 1
    '            End If
    '        End Try
    '    End Sub

    '    Private Sub frmНастройкаЛиста_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
    '        Dim упаковка As String = Nothing

    '        For I As Integer = 0 To lvwReceiver.Items.Count - 1
    '            упаковка = упаковка & lvwReceiver.Items(I).Text & "\"
    '        Next

    '        If упаковка <> Nothing Then
    '            ListSelectively = упаковка
    '        End If

    '        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
    '        mFormParent = Nothing
    '    End Sub

    '    Private Sub cmdДобавить_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdДобавить.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавление канала в список")

    '        Dim receiverListViewItem, sourceListViewItem As ListViewItem
    '        Dim lastIndex As Integer

    '        ' цикл по листу1 в поисках выделенного
    '        For I As Integer = 0 To lvwSource.Items.Count - 1
    '            If lvwSource.Items(I).Selected Then
    '                sourceListViewItem = lvwSource.Items(I)
    '                receiverListViewItem = New ListViewItem(sourceListViewItem.Text)
    '                receiverListViewItem.SubItems.Add(sourceListViewItem.SubItems(1).Text)
    '                receiverListViewItem.ImageIndex = sourceListViewItem.ImageIndex
    '                lvwReceiver.Items.Add(receiverListViewItem)
    '                lastIndex = I
    '            End If
    '        Next

    '        MakeUpdate(lvwSource, lastIndex)
    '    End Sub

    '    Private Sub cmdУдалить_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdУдалить.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удаление канала из списка")

    '        Dim deleteListViewItem As ListViewItem ', sourceListViewItem
    '        Dim lastIndex As Integer

    '        ' цикл по листу2 в поисках выделенного
    '        lvwReceiver.Focus()
    '        For I As Integer = lvwReceiver.Items.Count - 1 To 0 Step -1
    '            If lvwReceiver.Items(I).Selected Then
    '                deleteListViewItem = lvwReceiver.Items(I)
    '                lvwReceiver.Items.Remove(deleteListViewItem) ' требует объект а не индекс
    '                lastIndex = I
    '            End If
    '        Next

    '        MakeUpdate(lvwReceiver, lastIndex)
    '    End Sub

    '    ''' <summary>
    '    ''' Произвести Изменения
    '    ''' </summary>
    '    ''' <param name="tempListView"></param>
    '    ''' <param name="last"></param>
    '    Private Sub MakeUpdate(tempListView As ListView, last As Integer)
    '        Dim упаковка As String = Nothing
    '        Dim listViewИсточник As ListViewItem

    '        For I As Integer = 0 To lvwReceiver.Items.Count - 1
    '            упаковка &= lvwReceiver.Items(I).Text & "\"
    '        Next

    '        ОтобразитьРежимы()
    '        ОтобразитьРежимыНаЛистах(упаковка)

    '        tempListView.Focus()
    '        For I As Integer = 0 To tempListView.Items.Count - 1
    '            tempListView.Items(I).Selected = False
    '        Next

    '        If last > tempListView.Items.Count - 1 Then last = tempListView.Items.Count - 1

    '        If last > 0 Then
    '            listViewИсточник = tempListView.Items(last)
    '            listViewИсточник.EnsureVisible()
    '            listViewИсточник.Selected = True
    '        End If
    '    End Sub

    '    Private Sub cmdОчиститьВсе_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdОчиститьВсе.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Очистить список")
    '        ' первый лист полностью заполнить, а второй очистить
    '        ОтобразитьРежимы()
    '    End Sub

    '    Private Sub cmdЗапись_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdЗапись.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись составленного списка каналов в конфигурацию")

    '        Dim конфигурация As String = vbNullString

    '        For I As Integer = 0 To lvwReceiver.Items.Count - 1
    '            конфигурация &= lvwReceiver.Items(I).Text & "\"
    '        Next

    '        If конфигурация <> "" Then
    '            ЗаписатьСписокВБазу(конфигурация)
    '            WriteINILastId()
    '            ' перезаписать в глобальную переменную
    '            ListSelectively = конфигурация
    '        End If
    '    End Sub

    '    Private Sub cmdВосстановитьСписки_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdВосстановитьСписки.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Восстановление составленного списка каналов из конфигурации")

    '        Dim strSQL As String
    '        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '        Dim rdr As OleDbDataReader
    '        Dim cmd As OleDbCommand = cn.CreateCommand
    '        Dim конфигурация As String = Nothing
    '        Dim имяСписка As String = comСписки.Text

    '        If имяСписка <> "" Then
    '            ОтобразитьРежимы()
    '            strSQL = $"SELECT * FROM [Списки] WHERE Name= '{имяСписка}'"

    '            Try
    '                ' Создание читателя
    '                cmd.CommandType = CommandType.Text
    '                cmd.CommandText = strSQL
    '                ' Открыть подключение
    '                cn.Open()
    '                rdr = cmd.ExecuteReader

    '                If rdr.Read Then
    '                    конфигурация = rdr("Text")
    '                    lastIdConfig = rdr("Id")
    '                    ListSelectively = конфигурация
    '                    ОтобразитьРежимыНаЛистах(конфигурация)
    '                End If

    '                rdr.Close()
    '                cn.Close()
    '            Catch ex As Exception
    '                Const caption As String = "Восстановление списка параметров из базы"
    '                Dim text As String = ex.ToString
    '                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
    '            Finally
    '                WriteINILastId()
    '            End Try
    '        End If
    '    End Sub

    '    Private Sub WriteINILastId()
    '        WriteINI(PathOptions, "ConfigList", "LastIdСписки", CStr(lastIdConfig))
    '    End Sub

    '    Private Sub cmdУдалитьСписок_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdУдалитьСписок.Click
    '        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удаление составленного списка каналов из конфигурации")

    '        Dim имяСписка As String = comСписки.Text
    '        Dim strSQL As String
    '        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '        Dim cmd As OleDbCommand = cn.CreateCommand

    '        cmd.CommandType = CommandType.Text

    '        If имяСписка <> "" And comСписки.SelectedIndex <> -1 Then
    '            ' удалить из листа
    '            With comСписки
    '                .Items.RemoveAt(.SelectedIndex)
    '                ' удалить из массива
    '                If .Items.Count <> 0 Then
    '                    ReDim_списокИмен(.Items.Count)
    '                    For I As Integer = 1 To .Items.Count
    '                        списокИмен(I) = comСписки.Items(I - 1)
    '                    Next I
    '                End If
    '            End With

    '            Try
    '                ' Открыть подключение
    '                strSQL = $"DELETE * FROM [Списки] WHERE Name= '{имяСписка}'"
    '                cmd.CommandText = strSQL
    '                cn.Open()
    '                cmd.ExecuteNonQuery()
    '                cn.Close()
    '            Catch ex As Exception
    '                Const caption As String = "Удаление из базы"
    '                Dim text As String = ex.ToString
    '                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
    '            Finally
    '                ОтобразитьРежимы()
    '            End Try
    '        End If
    '    End Sub

    '    Private Sub ButtonDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDown.Click
    '        lvPreviousSelectedIndices = lvSelectedIndices
    '        lvSelectedIndices -= 1
    '        LVSelectedIndicesValueChanged()
    '    End Sub

    '    Private Sub ButtonUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUp.Click
    '        lvPreviousSelectedIndices = lvSelectedIndices
    '        lvSelectedIndices += 1
    '        LVSelectedIndicesValueChanged()
    '    End Sub

    '#Region "DragDrop"
    '    ''' <summary>
    '    ''' удостовериться что пользовательский курсор использован
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Private Sub Form1_GiveFeedback(ByVal sender As Object, ByVal e As GiveFeedbackEventArgs) Handles Me.GiveFeedback, lvwSource.GiveFeedback, lvwReceiver.GiveFeedback
    '        If img IsNot Nothing Then
    '            e.UseDefaultCursors = False
    '            Windows.Forms.Cursor.Current = CreaterCursor.CreateCursor(img, hotSpot.X, hotSpot.Y)
    '        End If
    '    End Sub

    '    Private Sub lvwCode_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles lvwReceiver.DragDrop
    '        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
    '            If lvwReceiver.dropIndex > -1 Then
    '                lvwReceiver.Items.Insert(lvwReceiver.dropIndex, DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem))
    '            Else
    '                lvwReceiver.Items.Add(DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem))
    '            End If

    '            lvwReceiver.Alignment = ListViewAlignment.Default
    '            lvwReceiver.Alignment = ListViewAlignment.Top
    '            lvwReceiver.Refresh()

    '            Dim last As Integer = DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem).Index
    '            MakeUpdate(lvwSource, last)
    '        End If
    '    End Sub

    '    Private Sub ListViews_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles lvwSource.DragOver, lvwReceiver.DragOver
    '        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
    '            e.Effect = DragDropEffects.Copy
    '        End If
    '    End Sub

    '    Private Sub lvwFunctionsList_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles lvwSource.MouseDown
    '        Dim itemListView As ListViewItem = lvwSource.HitTest(e.Location).Item

    '        If itemListView IsNot Nothing Then
    '            Dim index As Integer = itemListView.Index

    '            lvwSource.DoDragDrop(DirectCast(lvwSource.Items(index).Clone, ListViewItem), DragDropEffects.Copy)

    '            Dim p As Point = e.Location
    '            p.Offset(0, -lvwSource.Items(index).Bounds.Top)

    '            Dim EffectCursor As New System.Windows.Forms.Cursor(New IO.MemoryStream(My.Resources.move1))

    '            img = New Bitmap(lvwSource.Width, lvwSource.Items(index).Bounds.Height + EffectCursor.Size.Height)

    '            Dim gr As Graphics = Graphics.FromImage(img)
    '            gr.Clear(Color.White)

    '            For I As Integer = 0 To lvwSource.Items(index).SubItems.Count - 1
    '                gr.DrawString(lvwSource.Items(index).SubItems(I).Text, lvwSource.Font, Brushes.Black, lvwSource.Items(index).SubItems(I).Bounds.Left, 0)
    '            Next

    '            EffectCursor.Draw(gr, New Rectangle(p, EffectCursor.Size))

    '            img.MakeTransparent(Color.White)

    '            hotSpot = p
    '        End If
    '    End Sub
    '#End Region

End Class