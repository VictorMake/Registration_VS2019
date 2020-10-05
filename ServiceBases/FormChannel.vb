Imports System.Data.OleDb
Imports System.Threading

Friend Class FormChannel
    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
    End Sub

    Private Sub FormChannel_Load(sender As Object, e As EventArgs) Handles Me.Load
        LabelDescriptionStage.Text = "Каналы стенда №:" & StandNumber
        PopulateChannelsNBaseDataSet(StandNumber)
        DataGridChannels.AutoGenerateColumns = False
        LoadTableChannels()
    End Sub

    Private Sub FormChannel_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Close()
    End Sub

    ''' <summary>
    ''' Загрузить Таблицу Данными
    ''' </summary>
    Private Sub LoadTableChannels()
        ChannelNTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        ' сделать типизированный TableAdapter, который заполняется по номеру стенда
        ' и имеет поиск канала по имени
        ' данная строка кода позволяет загрузить данные в таблицу "ChannelsNBaseDataSet.ChannelN".
        ChannelNTableAdapter.Fill(ChannelsNDataSet.ChannelN)
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
                        cmd.CommandText = $"DELETE * FROM {CHANNEL_N};"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = $"INSERT INTO {CHANNEL_N} SELECT * FROM {tadleFrom}"
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
End Class