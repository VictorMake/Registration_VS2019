Public Class FormSearchChannel
#Region "Properties"
    Private mNumberChannel As String
    Private mChassis As String
    Private mModule As String
    Private mNumberChannelModule As String
    Private mChannelName As String
    Private mUnit As String

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
    ''' Корзина
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Chassis() As String
        Get
            Return mChassis
        End Get
    End Property

    ''' <summary>
    ''' Модуль
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ModuleInChassis() As String
        Get
            Return mModule
        End Get
    End Property

    ''' <summary>
    ''' Номер Канала Модуля
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NumberChannelModule() As String
        Get
            Return mNumberChannelModule
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

    ''' <summary>
    ''' Ед Измерения
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Unit() As String
        Get
            Return mUnit
        End Get
    End Property
#End Region

    Private ReadOnly refDataTableChannel As DataTable
    Private ReadOnly refDataView As DataView

    Public Sub New(ByRef inDv As DataView, ByRef DataTableChannel As DataTable)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        refDataTableChannel = DataTableChannel
        refDataView = inDv
    End Sub

    Private Sub FormFindChannel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        refDataTableChannel.DefaultView.RowFilter = ""
        ' При связывании grid с DataView,  grid будет показывать только согласованные записи
        DataGridFindChannels.DataSource = refDataTableChannel.DefaultView
    End Sub

    Private Sub SetFilter()
        Try
            With refDataTableChannel
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
                Dim drvChannel As DataRowView = refDataTableChannel.DefaultView(DataGridFindChannels.CurrentRow.Index)
                Dim rowChannel As DataRow = drvChannel.Row
                ' НомерКанала= "0" 'неважно (всегда=0)
                ' Корзина  = "2" 'НомерУстройства
                ' Модуль  = "1" 'НомерМодуляКорзины
                ' НомерКаналаМодуля  = "0" 'неважно 0-31
                ' ИмяКанала = "R изм-2"
                ' ЕдИзмерения = "кгс"
                'LabelNumber.Text = rowСтрокаChannels("НомерПараметра").ToString 'целое
                mChannelName = CStr(rowChannel("НаименованиеПараметра"))
                mNumberChannel = rowChannel("НомерКанала").ToString 'целое
                mChassis = rowChannel("НомерУстройства").ToString 'целое

                If Not IsDBNull(rowChannel("НомерМодуляКорзины")) Then
                    mModule = CStr(rowChannel("НомерМодуляКорзины"))
                Else
                    mModule = "" 'vbNullString
                End If

                If Not IsDBNull(rowChannel("НомерКаналаМодуля")) Then
                    mNumberChannelModule = CStr(rowChannel("НомерКаналаМодуля"))
                Else
                    mNumberChannelModule = "" 'vbNullString
                End If

                If Array.IndexOf(UnitOfMeasureArray, rowChannel("ЕдиницаИзмерения")) <> -1 Then
                    mUnit = CStr(rowChannel("ЕдиницаИзмерения"))
                Else
                    mUnit = "Кгсм"
                End If
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
        BindingContext(refDataView).Position = tabRowSelectPosition
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
        If refDataView.Count > 0 AndAlso tabRowSelectPosition <> 0 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = 0
            ShowCurrentRow()
        End If
    End Sub

    Private Sub PreviousRecord()
        If refDataView.Count > 0 AndAlso tabRowSelectPosition > 0 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition -= 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub NextRecord()
        If refDataView.Count > tabRowSelectPosition + 1 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition += 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub LastRecord()
        If refDataView.Count > 0 AndAlso refDataView.Count <> tabRowSelectPosition + 1 AndAlso DataGridFindChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = refDataView.Count - 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub DataGridFindChannels_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridFindChannels.SelectionChanged
        If isNeedSelectionChanged AndAlso DataGridFindChannels.CurrentRow IsNot Nothing Then
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

        If refDataView.Count > 0 AndAlso tabRowSelectPosition <> -1 Then
            LabelDescriptionStage.Text = $"Задать фильтр и выбрать канал в таблице (запись {tabRowSelectPosition + 1} из {refDataView.Count})" ' или {DataGridChannels.Rows.Count}
            If tabRowSelectPosition = 0 Then
                BindingNavigatorMoveFirstItem.Enabled = False
                BindingNavigatorMovePreviousItem.Enabled = False
            ElseIf tabRowSelectPosition = refDataView.Count - 1 Then
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
        BindingNavigatorCountItem.Text = $"для {{{refDataView.Count}}}"
    End Sub
#End Region
End Class