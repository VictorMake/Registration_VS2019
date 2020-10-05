Imports System.Data.OleDb
Imports System.Text
Imports MathematicalLibrary

Friend Class FormTuningRegime
    Private Structure ParameterType
        Dim NumberParameter As Short ' Номер Параметра
        Dim NameParameter As String ' Наименование Параметра
        Dim Unit As String ' Единица Измерения
        Dim Description As String ' Примечание
    End Structure
    Private aParameterType() As FormTuningRegime.ParameterType

    Private Structure RegimeType
        Dim NumbeRegime As Short ' Номер Режима
        Dim NameRegime As String ' Наименование
        Dim Configuration As String ' Перечень Параметров
    End Structure
    Private aRegimeType() As FormTuningRegime.RegimeType

    Private isFormLoaded As Boolean = False

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        LoadFormTuningRegime()
    End Sub

    Private Sub LoadFormTuningRegime()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Me.Text)
        PopulateChannelsAndRegime()
        InitializeListView()

        For I As Integer = 1 To UBound(aRegimeType)
            ComboBoxSelectRegime.Items.Add(aRegimeType(I).NameRegime)
        Next

        ComboBoxSelectRegime.SelectedIndex = 0 ' по умолчанию первый злемент активный
        ComboBoxSelectRegimeSelectedIndexChanged()
        isFormLoaded = True
        FormTuningRegimeResize()
    End Sub

    Private Sub InitializeListView()
        ListViewSelectedChannels.Items.Clear()
        ListViewSelectedChannels.Columns.Clear()
        ListViewSelectedChannels.Columns.Add("Параметр", ListViewSelectedChannels.Width * 2 \ 8 - 8, HorizontalAlignment.Left)
        ListViewSelectedChannels.Columns.Add("№ Параметра", ListViewSelectedChannels.Width * 1 \ 8 - 4, HorizontalAlignment.Left)
        ListViewSelectedChannels.Columns.Add("Назначение", ListViewSelectedChannels.Width * 5 \ 8 - 4, HorizontalAlignment.Left)
    End Sub

    Private Sub FormTuningRegime_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        FormTuningRegimeResize()
    End Sub

    Private Sub FormTuningRegimeResize()
        If isFormLoaded Then
            ListViewSelectedChannels.Columns(0).Width = CInt(ListViewSelectedChannels.Width * 2 / 8 - 8)
            ListViewSelectedChannels.Columns(1).Width = CInt(ListViewSelectedChannels.Width * 1 / 8 - 4)
            ListViewSelectedChannels.Columns(2).Width = CInt(ListViewSelectedChannels.Width * 5 / 8 - 4)
        End If
    End Sub

    Private Sub BottonSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles BottonSave.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Сохранение составленной конфигурации каналов")
        Dim sb As New StringBuilder

        ' сначало делается запись, а затем считывание по новой
        For I As Integer = 1 To UBound(aParameterType)
            If ListViewSelectedChannels.Items(I - 1).Checked Then
                If sb.Length = 0 Then
                    sb.Append(aParameterType(I).NameParameter)
                Else
                    sb.Append($"\{aParameterType(I).NameParameter}")
                End If
            End If
        Next

        sb.Append("\")
        ' теперь запись
        UpdateNewConfigurationOnDbase(sb.ToString)
        ' теперь считывание
        PopulateChannelsAndRegime()
        ComboBoxSelectRegimeSelectedIndexChanged()
    End Sub

    ''' <summary>
    ''' Загрузка списка каналов и режимов рабочего стенда
    ''' </summary>
    Private Sub PopulateChannelsAndRegime()
        Dim strSQL As String = "SELECT COUNT(*) FROM " & ChannelLast
        Dim index As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cn.Open()
        'ReDim_aParameterType(CInt(cmd.ExecuteScalar))
        Re.Dim(aParameterType, CInt(cmd.ExecuteScalar))

        strSQL = "SELECT * FROM " & ChannelLast
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        ' при добавлении полей надо модифицировать запрос в базе
        Do While rdr.Read
            index = CInt(rdr("НомерПараметра"))
            aParameterType(index).NumberParameter = CShort(rdr("НомерПараметра"))
            aParameterType(index).NameParameter = CStr(rdr("НаименованиеПараметра"))
            aParameterType(index).Unit = CStr(rdr("ЕдиницаИзмерения"))

            If Not IsDBNull(rdr("Примечания")) Then
                aParameterType(index).Description = CStr(rdr("Примечания"))
            Else
                aParameterType(index).Description = CStr(0)
            End If
        Loop

        rdr.Close()

        strSQL = $"SELECT COUNT(*) FROM [Режимы{StandNumber}]"
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        'ReDim_aRegimeType(CInt(cmd.ExecuteScalar))
        Re.Dim(aRegimeType, CInt(cmd.ExecuteScalar))

        strSQL = $"SELECT * FROM [Режимы{StandNumber}]"
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        ' при добавлении полей надо модифицировать запрос в базе
        Do While rdr.Read
            index = CInt(rdr("НомерРежима"))
            aRegimeType(index).NumbeRegime = CShort(rdr("НомерРежима"))
            aRegimeType(index).NameRegime = CStr(rdr("Наименование"))
            aRegimeType(index).Configuration = CStr(rdr("ПереченьПараметров"))
        Loop

        rdr.Close()
        cn.Close()
    End Sub

    Private Sub ComboBoxSelectRegimeSelectedIndexChanged()
        ShowRegimeOnList(ComboBoxSelectRegime.SelectedIndex + 1)
    End Sub
    Private Sub ComboBoxSelectRegime_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxSelectRegime.SelectedIndexChanged
        ComboBoxSelectRegimeSelectedIndexChanged()
    End Sub

    ''' <summary>
    ''' Отобразить режимы на листе
    ''' </summary>
    ''' <param name="numberRegime"></param>
    Private Sub ShowRegimeOnList(numberRegime As Integer)
        With ListViewSelectedChannels
            .BeginUpdate()
            .Items.Clear()
            ' Создать переменную, чтобы добавлять объекты ListItem.
            For I As Integer = 1 To UBound(aParameterType)
                Dim itemLView As New ListViewItem(aParameterType(I).NameParameter) With {.Name = aParameterType(I).NameParameter}
                itemLView.SubItems.Add(aParameterType(I).NumberParameter.ToString)
                itemLView.SubItems.Add(aParameterType(I).Description)

                If CBool(InStr(1, UnitOfMeasureString, aParameterType(I).Unit)) Then
                    itemLView.ImageIndex = Array.IndexOf(UnitOfMeasureArray, aParameterType(I).Unit)
                Else
                    itemLView.ImageIndex = 6
                End If

                itemLView.ForeColor = Color.Red ' красный - как будто его нет
                .Items.Add(itemLView)
            Next

            ' список параметров в упаковке
            Dim names As String() = DecryptionString(aRegimeType(numberRegime).Configuration)

            For I As Integer = 1 To UBound(aParameterType)
                For J As Integer = 1 To UBound(names)
                    If aParameterType(I).NameParameter = names(J) Then
                        .Items(I - 1).Checked = True
                        .Items(I - 1).ForeColor = Color.Black ' черный - есть
                        Exit For
                    End If
                Next
            Next

            .EndUpdate()
        End With
    End Sub

    ''' <summary>
    ''' Записать новую конфигурацию в базу данных
    ''' </summary>
    ''' <param name="configuration"></param>
    Private Sub UpdateNewConfigurationOnDbase(configuration As String)
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        Dim strSQL As String = "Update Режимы" & StandNumber &
                        " SET ПереченьПараметров = '" & configuration &
                        "' WHERE ([НомерРежима]= " & (ComboBoxSelectRegime.SelectedIndex + 1).ToString & ")"
        cmd.CommandText = strSQL
        cn.Open()
        cmd.ExecuteNonQuery()
        cn.Close()
    End Sub

    Private Sub ListViewSelectedChannels_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) Handles ListViewSelectedChannels.ItemCheck
        If e.NewValue = CheckState.Checked Then
            ListViewSelectedChannels.Items(e.Index).ForeColor = Color.Black
        Else
            ListViewSelectedChannels.Items(e.Index).ForeColor = Color.Red
        End If
    End Sub

    Private Sub ButtonSelectAll_Click(sender As Object, e As EventArgs) Handles ButtonSelectAll.Click
        CheckedAllListViewItem(True)
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        CheckedAllListViewItem(False)
    End Sub

    Private Sub CheckedAllListViewItem(isChecked As Boolean)
        ListViewSelectedChannels.BeginUpdate()
        For Each itemListView As ListViewItem In ListViewSelectedChannels.Items
            itemListView.Checked = isChecked
        Next
        ListViewSelectedChannels.EndUpdate()
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ListViewSelectedChannels)
        mSearchChannel.SelectChannel()
    End Sub
End Class