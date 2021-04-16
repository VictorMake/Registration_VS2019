Imports System.Data.OleDb
Imports MathematicalLibrary

Friend Class FormTuningVisibleTail
    Public Property ParentFormMain() As FormMain

    Private Structure TypeTuningMode
        Dim numberRegime As Short ' номер Режима
        Dim name As String ' наименование
        Dim listChannels As String ' перечень Параметров
    End Structure
    Private arrTypeTuningMode() As FormTuningVisibleTail.TypeTuningMode

    Private strSQL As String
    ''' <summary>
    ''' настройка Для Считанного Снимка
    ''' </summary>
    Private isChannelShaphot As Boolean
    Private isFormLoaded As Boolean = False
    ''' <summary>
    ''' Локальная копия массива структуры настроек каналов
    ''' </summary>
    Private parameters As TypeBaseParameter()
    Private sKey As String ' запись INI для снимка или регистратора
    Private colunmName As String
    Private indexesOfParameter As Integer()

    Public Sub New(ByVal parentForm As FormMain,
                   ByRef inParametersType As TypeBaseParameter(),
                   ByRef inParametersShaphotType As TypeBaseParameter(),
                   ByVal inCopyListOfParameter As Integer())
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        ParentFormMain = parentForm

        ParentFormMain.GetSqlForDbase(strSQL, isChannelShaphot)

        If isChannelShaphot Then
            parameters = inParametersShaphotType
            sKey = "Снимок"
            colunmName = "Видимость"
        Else
            parameters = inParametersType
            sKey = "Регистратор"
            colunmName = "ВидимостьРегистратор"
        End If

        indexesOfParameter = inCopyListOfParameter
        InitializeForm()
    End Sub

    ' если в форме произошли изменения по включению или выключению,
    ' то режим Регистратор в событии меню клик надо перезапустить и запустить сбор
    ' записать изменения в массив и базу данных не по номеру а по имени?
    Private Sub InitializeForm()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        InitializeListView()
        PopulateComboBoxRegime()
        isFormLoaded = True
        FormTuningVisibleTail_Resize(Me, New EventArgs)
    End Sub

    Private Sub InitializeListView()
        ListViewName.Items.Clear()
        ListViewName.Columns.Clear()
        ListViewName.Columns.Add("Параметр", ListViewName.Width * 2 \ 8 - 8, HorizontalAlignment.Left)
        ListViewName.Columns.Add("№ Параметра", ListViewName.Width * 1 \ 8 - 4, HorizontalAlignment.Left)
        ListViewName.Columns.Add("Назначение", ListViewName.Width * 5 \ 8 - 4, HorizontalAlignment.Left)
    End Sub

    Private Sub FormTuningVisibleTail_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isFormLoaded Then
            ListViewName.Columns(0).Width = CInt(ListViewName.Width * 2 / 8 - 8)
            ListViewName.Columns(1).Width = CInt(ListViewName.Width * 1 / 8 - 4)
            ListViewName.Columns(2).Width = CInt(ListViewName.Width * 5 / 8 - 4)
        End If
    End Sub

    ''' <summary>
    ''' Загрузка Режимов
    ''' </summary>
    Private Sub PopulateComboBoxRegime()
        CheckTableNameRegime()
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        Const имяРежима As String = "ТекстовыйКонтроль"
        Dim strSQL As String = $"SELECT COUNT(*) FROM [Режимы{StandNumber}] WHERE [Наименование]<> '{имяРежима}'"

        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cn.Open()
        Re.Dim(arrTypeTuningMode, CInt(cmd.ExecuteScalar))

        strSQL = $"SELECT * FROM [Режимы{StandNumber}] WHERE [Наименование]<> '{имяРежима}'"
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        ' при добавлении полей надо модифицировать запрос в базе
        Dim index As Integer
        Do While rdr.Read
            index = CInt(rdr("НомерРежима"))
            arrTypeTuningMode(index).numberRegime = CShort(rdr("НомерРежима"))
            arrTypeTuningMode(index).name = CStr(rdr("Наименование"))
            arrTypeTuningMode(index).listChannels = CStr(rdr("ПереченьПараметров"))
        Loop

        rdr.Close()
        cn.Close()

        For I As Integer = 1 To UBound(arrTypeTuningMode)
            ComboBoxSelectRegime.Items.Add(arrTypeTuningMode(I).name)
        Next

        ComboBoxSelectRegime.SelectedIndex = 0 ' по умолчанию первый элемент активный, далее обработка ComboBoxSelectRegime.SelectedIndexChanged
    End Sub

    Private Sub ComboBoxSelectRegime_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxSelectRegime.SelectedIndexChanged
        PopulateSelectiveControl(ComboBoxSelectRegime.SelectedIndex + 1)
    End Sub

    ''' <summary>
    ''' Отобразить Видимость Режима
    ''' в зависимости от типа работы сбора или расшифровки.
    ''' </summary>
    ''' <param name="numberRegime"></param>
    Private Sub PopulateSelectiveControl(numberRegime As Integer)
        Dim I, J As Integer

        With ListViewName
            .BeginUpdate()
            .Items.Clear()

            For I = 1 To UBound(parameters)
                ' Создать переменную, чтобы добавлять объекты ListItem.
                Dim newListViewItem As ListViewItem = New ListViewItem(parameters(I).NameParameter) With {.Name = parameters(I).NameParameter}
                newListViewItem.SubItems.Add(parameters(I).NumberParameter.ToString)
                newListViewItem.SubItems.Add(parameters(I).Description)

                If CBool(InStr(1, UnitOfMeasureString, parameters(I).UnitOfMeasure)) Then
                    newListViewItem.ImageIndex = Array.IndexOf(UnitOfMeasureArray, parameters(I).UnitOfMeasure)
                Else
                    newListViewItem.ImageIndex = 6
                End If

                newListViewItem.ForeColor = Color.Red ' красный как будто его нет
                .Items.Add(newListViewItem)

                For J = 1 To UBound(indexesOfParameter)
                    ' проверить на совпадение номеров и пометить параметр который есть в конфигурации
                    If indexesOfParameter(J) = I Then
                        .Items(I - 1).ForeColor = Color.Black ' черный
                        Exit For
                    End If
                Next

                If numberRegime = 1 Then newListViewItem.Checked = parameters(I).IsVisible
            Next

            If numberRegime <> 1 Then
                ' список параметров в упаковке
                Dim names As String() = DecryptionString(arrTypeTuningMode(numberRegime).listChannels)

                For I = 1 To UBound(parameters)
                    For J = 1 To UBound(names)
                        If parameters(I).NameParameter = names(J) Then
                            .Items(I - 1).Checked = True
                            Exit For
                        End If
                    Next
                Next
            End If

            .EndUpdate()
        End With
    End Sub

    Private Sub ButtonSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonSave.Click
        SaveSelection()
    End Sub
    Private Sub ButtonSaveAsPattern_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonSaveAsPattern.Click
        SaveAsPattern()
    End Sub
    Private Sub ButtonRestore_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonRestore.Click
        RestoreFromPattern()
    End Sub

    ''' <summary>
    ''' Применить конфигурацию выделенныы каналов.
    ''' </summary>
    Private Sub SaveSelection()
        Const limitSelection As Integer = 16 ' Ограничение Видимых Шлефов
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись составленного списка видимых шлейфов")

        If Not isChannelShaphot Then ' проверка на ограничение
            If ListViewName.CheckedIndices.Count > limitSelection Then
                Const caption As String = "Видимость"
                Dim text As String = $"Число видимых шлейфов в режиме регистрации не должно превышать {limitSelection}!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If
        End If

        RewriteToParametersType()
        SaveSelectionToDBase(strSQL)
    End Sub

    ''' <summary>
    ''' Запись Видимых Шлейфов в базу
    ''' </summary>
    ''' <param name="strSQL"></param>
    Private Sub SaveSelectionToDBase(ByRef strSQL As String)
        Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter(strSQL, BuildCnnStr(ProviderJet, PathChannels))
        Dim dtDataTable As New DataTable
        Dim cb As OleDbCommandBuilder
        Dim drDataRow As DataRow
        Dim aFindValue(0) As Object
        Dim dcDataColumn(1) As DataColumn

        odaDataAdapter.Fill(dtDataTable)
        dcDataColumn(0) = dtDataTable.Columns("НомерПараметра")
        dtDataTable.PrimaryKey = dcDataColumn

        For I As Integer = 1 To parameters.Length - 1
            aFindValue(0) = parameters(I).NumberParameter
            drDataRow = dtDataTable.Rows.Find(aFindValue)

            If drDataRow IsNot Nothing Then
                drDataRow(colunmName) = parameters(I).IsVisible
            End If
        Next

        cb = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.Update(dtDataTable)
    End Sub

    ''' <summary>
    ''' Синхронизировать локальную копия массива структуры настроек каналов
    ''' </summary>
    Private Sub RewriteToParametersType()
        For I As Integer = 1 To UBound(parameters)
            parameters(I).IsVisible = ListViewName.Items(I - 1).Checked
        Next
    End Sub

    ''' <summary>
    ''' Записать список каналов в шаблон
    ''' </summary>
    Private Sub SaveAsPattern()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись составленного списка как Шаблон")

        Dim checkedChannelList As String = Nothing ' список Видимых Каналов

        For I As Integer = 1 To UBound(parameters)
            If ListViewName.Items(I - 1).Checked Then
                checkedChannelList &= parameters(I).NameParameter & "\"
            End If
        Next

        If Len(checkedChannelList) > 254 Then
            Const caption As String = "Видимость"
            Const text As String = "Число отмеченных параметров должно быть уменьшено"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        Else
            WriteINI(PathOptions, "Visual", sKey, checkedChannelList)
        End If
    End Sub

    ''' <summary>
    ''' Восстановить список каналов из шаблона
    ''' </summary>
    Private Sub RestoreFromPattern()
        Dim I, J As Integer

        RegistrationEventLog.EventLog_MSG_USER_ACTION("Восстановить ранее записанный список из шаблона")
        ' считать из файла строку с параметрами контроля и расшифровать ее в массив
        ' список параметров в упаковке
        Dim names As String() = DecryptionString(GetIni(PathOptions, "Visual", sKey, "N1\")) ' список Видимых Каналов

        ' из массива на листt пометить какие параметры на контроль
        For I = 1 To UBound(parameters)
            ListViewName.Items(I - 1).Checked = False
            For J = 1 To UBound(names)
                If parameters(I).NameParameter = names(J) Then
                    ListViewName.Items(I - 1).Checked = True
                    Exit For
                End If
            Next
        Next

        RewriteToParametersType()
    End Sub

    Private Sub FormTuningVisibleTail_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormMain = Nothing
    End Sub

    Private Sub ListViewName_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListViewName.ItemChecked
        LabelSelected.Text = ListViewName.CheckedItems.Count.ToString
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ListViewName)
        mSearchChannel.SelectChannel()
    End Sub
End Class