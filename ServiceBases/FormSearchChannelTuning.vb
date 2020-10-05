Imports System.Data.OleDb
Imports System.Threading

Public Class FormSearchChannelTuning
#Region "Properties"
    Private mNumberChannel As String
    Private mChannelName As String

    ''' <summary>
    ''' Номер Канала
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NumberChannel() As String
        Get
            Return mNumberChannel
        End Get
    End Property

    ''' <summary>
    ''' Имя Канала
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ChannelName() As String
        Get
            Return mChannelName
        End Get
    End Property
#End Region

    Private dataTableChannel As DataTable
    Private dataView As DataView

    Public Sub New() 'ByRef inDv As DataView, ByRef DataTableChannel As DataTable)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
    End Sub

    Private Sub FormFindChannel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)

        PopulateChannelsNBaseDataSet(StandNumber)
        LoadTableChannels()
    End Sub

    ''' <summary>
    ''' Заполнить Отсоединённый Набор BaseChannels
    ''' Вначале копируется база Channel(НомерСтенда) на место ChannelN
    ''' затем создаётся подключение
    ''' </summary>
    ''' <param name="standNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateChannelsNBaseDataSet(standNumber As String)
        Dim tadleFrom As String = "Channel" & standNumber

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            If CheckExistTable(cn, tadleFrom) AndAlso CheckExistTable(cn, CHANNEL_N) Then
                Try
                    Using cmd As OleDbCommand = cn.CreateCommand
                        cmd.CommandType = CommandType.Text
                        'strSQL = "DROP TABLE ChannelN;"
                        cmd.CommandText = $"DELETE * FROM {CHANNEL_N};"
                        cmd.ExecuteNonQuery()
                        'strSQL = "SELECT " & tadleFrom & ".* INTO ChannelN FROM " & tadleFrom
                        cmd.CommandText = $"INSERT INTO {CHANNEL_N} SELECT * FROM {tadleFrom}" '& " IN " & """" & strПутьChannels & """" & ";"
                        cmd.ExecuteNonQuery()
                    End Using
                    Thread.Sleep(500)
                    Application.DoEvents()
                Catch ex As Exception
                    Dim caption As String = $"Ошибка копирования данных в процедуре <{NameOf(PopulateChannelsNBaseDataSet)}>."
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                End Try
            Else
                Dim caption As String = $"Проверка наличия таблицы в процедуре <{NameOf(PopulateChannelsNBaseDataSet)}>."
                Dim text As String = $"Таблицы {tadleFrom} или {CHANNEL_N} не существует!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Загрузить Таблицу Данными
    ''' </summary>
    Private Sub LoadTableChannels()
        ChannelNTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        ' сделать типизированный TableAdapter, который заполняется по номеру стенда
        ' и имеет поиск канала по имени
        ' данная строка кода позволяет загрузить данные в таблицу "ChannelsNBaseDataSet.ChannelN".
        ChannelNTableAdapter.Fill(ChannelsNDataSet1.ChannelN)
        dataTableChannel = ChannelsNDataSet1.Tables(CHANNEL_N)
        dataView = dataTableChannel.DefaultView
        dataView.Sort = "НомерПараметра"
        ApplyFilterBindingSource()
        ' При связывании grid с DataView,  grid будет показывать только согласованные записи
        DataGridFindChannels.AutoGenerateColumns = False
        DataGridFindChannels.DataSource = dataView
        dataTableChannel.DefaultView.RowFilter = ""
    End Sub

    Private Sub ApplyFilterBindingSource()
        BindingSourceChannelsN.Filter = dataView.RowFilter
        BindingSourceChannelsN.Sort = dataView.Sort
    End Sub

    Private Sub SetFilter()
        Try
            With dataTableChannel
                .DefaultView.RowFilter = $"НаименованиеПараметра like '*{BindingNavigatorTextBoxFilter.Text}%'"
                'If .DefaultView.Count = 0 Then
                '    'MessageBox.Show("Не найдено ни одной записи.", "Фильтрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'End If
                ShowCurrentRowOnTable()
                ButtonOK.Enabled = .DefaultView.Count <> 0
            End With
        Catch ex As Exception
            Dim caption As String = $"Ошибка задания фильтра в процедуре <{NameOf(SetFilter)}>."
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            BindingNavigatorTextBoxFilter.Text = ""
        End Try
    End Sub

    Private Sub BindingNavigatorTextBoxFilter_TextChanged(sender As Object, e As EventArgs) Handles BindingNavigatorTextBoxFilter.TextChanged
        SetFilter()
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonOK.Click
        If DataGridFindChannels.CurrentRow IsNot Nothing OrElse DataGridFindChannels.CurrentRow.Index <> -1 Then
            Try
                Dim drvChannel As DataRowView = dataTableChannel.DefaultView(DataGridFindChannels.CurrentRow.Index)
                Dim rowChannel As DataRow = drvChannel.Row
                ' НомерКанала= "0" 'неважно (всегда=0)
                ' ИмяКанала = "R изм-2"
                mChannelName = CStr(rowChannel("НаименованиеПараметра"))
                mNumberChannel = rowChannel("НомерКанала").ToString 'целое
            Catch ex As Exception
                Dim caption As String = $"Ошибка выбора отфильтрованной записи из таблицы в процедуре <{NameOf(ButtonOK_Click)}>."
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
            RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
            DialogResult = DialogResult.OK
        Else
            MessageBox.Show("Необходимо выделить запись.", "Нет выделенной записи!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCancel.Click
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        DialogResult = DialogResult.Cancel
    End Sub

#Region "Навигация по таблице"
    Private tabRowSelectPosition As Integer
    Private isNeedSelectionChanged As Boolean = True

    Private Sub ShowCurrentRow()
        isNeedSelectionChanged = False
        DataGridFindChannels.CurrentRow.Selected = False ' UnSelect
        BindingContext(dataView).Position = tabRowSelectPosition
        ShowCurrentRowOnTable()
        isNeedSelectionChanged = True
    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        FirstRecord()
    End Sub
    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        PreviousRecord()
    End Sub
    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        NextRecord()
    End Sub
    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        LastRecord()
    End Sub

    Private Sub FirstRecord()
        If dataView.Count > 0 AndAlso tabRowSelectPosition <> 0 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = 0
            ShowCurrentRow()
        End If
    End Sub

    Private Sub PreviousRecord()
        If dataView.Count > 0 AndAlso tabRowSelectPosition > 0 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition -= 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub NextRecord()
        If dataView.Count > tabRowSelectPosition + 1 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition += 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub LastRecord()
        If dataView.Count > 0 AndAlso dataView.Count <> tabRowSelectPosition + 1 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = dataView.Count - 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub DataGridFindChannels_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridFindChannels.SelectionChanged
        If dataTableChannel IsNot Nothing AndAlso isNeedSelectionChanged AndAlso DataGridFindChannels.CurrentRow IsNot Nothing Then
            tabRowSelectPosition = DataGridFindChannels.CurrentRow.Index
            ShowCurrentRowOnTable()
        End If
    End Sub

    ''' <summary>
    ''' Заполнить контролы данными выделенной строки
    ''' </summary>
    Private Sub ShowCurrentRowOnTable()
        BindingNavigatorMoveFirstItem.Enabled = True
        BindingNavigatorMovePreviousItem.Enabled = True
        BindingNavigatorMoveNextItem.Enabled = True
        BindingNavigatorMoveLastItem.Enabled = True

        If dataView.Count > 0 AndAlso tabRowSelectPosition <> -1 Then
            LabelDescriptionStage.Text = $"Задать фильтр и выбрать канал в таблице (запись {tabRowSelectPosition + 1} из {dataView.Count})" ' или {DataGridChannels.Rows.Count}
            If tabRowSelectPosition = 0 Then
                BindingNavigatorMoveFirstItem.Enabled = False
                BindingNavigatorMovePreviousItem.Enabled = False
            ElseIf tabRowSelectPosition = dataView.Count - 1 Then
                BindingNavigatorMoveNextItem.Enabled = False
                BindingNavigatorMoveLastItem.Enabled = False
            End If

            DataGridFindChannels.Rows(tabRowSelectPosition).Selected = True
        Else
            LabelDescriptionStage.Text = "Ни чего не найдено"
            BindingNavigatorMoveFirstItem.Enabled = False
            BindingNavigatorMovePreviousItem.Enabled = False
            BindingNavigatorMoveNextItem.Enabled = False
            BindingNavigatorMoveLastItem.Enabled = False
        End If

        BindingNavigatorPositionItem.Text = CStr(tabRowSelectPosition + 1)
        BindingNavigatorCountItem.Text = $"для {{{dataView.Count}}}"
    End Sub
#End Region
End Class