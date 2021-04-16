Imports MathematicalLibrary
Imports NationalInstruments.Net

Friend Class FormClient
    Private aNameDataServer() As String
    Private aValueDataServer() As Double
    Private aIndexDataServer() As Integer
    Private ReadOnly mainForm As FormRegistrationClient
    Private lengthArray As Integer
    Private stringDataSocket As String
    Private partString As String
    Private lengthMemo As Integer
    Private isDataChanged As Boolean

    Public ReadOnly Property ValueDataServer(ByVal index As Integer) As Double
        Get
            Return aValueDataServer(index)
        End Get
    End Property
    ''' <summary>
    ''' Форма открыта из меню
    ''' </summary>
    ''' <returns></returns>
    Public Property IsFormOpenedFromMenu() As Boolean

    Public Sub New(InMainForm As FormRegistrationClient)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        mainForm = InMainForm
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        MdiParent = MainMdiForm
        TextBoxURL.Text = AddressURLServer
    End Sub

    Private Sub FormClient_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        'edURL.Text = "file:\\Victor\f\ComponentWorks\Tutorials\Visual Basic\ppp.dsd"  'работает
        'edURL.Text = "dstp://Victor/wave"'работает

        'dstp://localhost/wave
        'edURL.Text = "file:ppp.dsd"'работает
        'edURL.Text = "dstp://localhost/wave"
        'edURL.Text = "dstp://Soplo/wave"

        InitializeListViewValueReceived()

        'If Not IsClient Then' для сопла
        '    ReDim_aNameDataServer(17)
        '    ReDim_aValueDataServer(17)
        '    ReDim_aIndexDataServer(17)
        '    ДобавитьСтрокиЛиста()
        'End If

        TextBoxURL.Text = AddressURLServer
        IsFocusToClient = True
        TimerReceive.Enabled = True
        CollectionForms.Add(Text, Me)
    End Sub

    Private Sub FormClient_Activated(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Activated
        WindowState = FormWindowState.Normal
    End Sub

    'Private Sub FormClient_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.FormClosing
    '    cmdQuit.PerformClick()
    '    e.Cancel = True
    'End Sub

    Private Sub FormClient_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        IsFormClientStart = False
        IsFocusToClient = False
        ActivateMainForm()
        CollectionForms.Remove(Text)
    End Sub

    Private Sub FormClient_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        IsFocusToClient = True
    End Sub

    Private Sub InitializeListViewValueReceived()
        ListViewValueReceived.Columns.Add("Параметр", ListViewValueReceived.Width * 2 \ 4, HorizontalAlignment.Left)
        ListViewValueReceived.Columns.Add("Значение", ListViewValueReceived.Width \ 4, HorizontalAlignment.Left)
        ListViewValueReceived.Columns.Add("Индекс", ListViewValueReceived.Width \ 4, HorizontalAlignment.Left)
    End Sub

    ''' <summary>
    ''' Подключите соединение данных с источником, и запросить их.
    ''' Строка соединенния как текстовый файл.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Public Sub ButtonConnect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonConnect.Click
        ' Контрол DataSocket получает модификации автоматически.
        If DataSocketReceive.IsConnected Then
            DataSocketReceive.Disconnect()
        End If
        DataSocketReceive.Connect(TextBoxURL.Text, AccessMode.ReadAutoUpdate)
    End Sub
    ''' <summary>
    ''' Отключить DataSocket от источника, с которым это связано.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Public Sub ButtonDisconnect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonDisconnect.Click
        DataSocketReceive.Disconnect()
    End Sub

    Private Sub ButtonQuit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonQuit.Click
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Сворачивание окна " & Text)
        WindowState = FormWindowState.Minimized
        IsFocusToClient = False
        ActivateMainForm()
    End Sub

    'Private data As String
    Private Sub TimerUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerUpdate.Tick
        If DataSocketReceive.IsDataUpdated Then
            'data = CStr(DataSocketReceive.Data.Value)
            'DataSocketOnDataUpdated(Data)
            DataSocketOnDataUpdated(CStr(DataSocketReceive.Data.Value))
        End If
    End Sub

    Private Sub DataSocketOnDataUpdated(ByRef data As String)
        'Private Sub DataSocketReceive_OnDataUpdated(ByVal eventSender As System.Object, ByVal eventArgs As AxCWDSLib._INIDSCtlEvents_OnDataUpdatedEvent) Handles DataSocketReceive.OnDataUpdated
        Dim I, splitterNumber As Integer
        Dim count As Integer
        Dim position As Integer
        Dim lenghtString As Integer = Len(data)
        Dim countInString As Integer

        'Application.DoEvents()
        'data = CStr(eventArgs.data.Value) 
        'lenghtString = Len(eventArgs.data.Value)
        If lenghtString <= 1 OrElse Mid(data, lenghtString, 1) <> Separator Then
            'DataSocketReceive.Disconnect()
            'DataSocketReceive.ConnectTo(edURL.Text, CWUIControlsLib.CWDSAccessModes.cwdsReadAutoUpdate)
            Exit Sub
        End If

        Do
            'position = InStr(intПозиция + 2, eventArgs.data.Value, сРазделитель)
            position = InStr(position + 2, data, Separator)
            countInString += 1
            If countInString > lenghtString Then Exit Sub
        Loop Until position >= lenghtString

        countInString \= 3
        isDataChanged = Not lengthMemo = countInString

        If isDataChanged Then
            DetermineArrayDimension(data) '(eventArgs.data)
            ' повторная расшифровка
            count = 1
            position = InStr(count, data, Separator)
            splitterNumber = 1
            I = 1

            Do
                partString = Mid(data, count, position - count)

                Select Case splitterNumber
                    Case 1
                        aNameDataServer(I) = partString
                        If I <= lengthArray Then
                            If NameParametersFromServer(I) <> aNameDataServer(I) Then NameParametersFromServer(I) = aNameDataServer(I)
                        End If
                        Exit Select
                    Case 3
                        aIndexDataServer(I) = CInt(partString)
                        Exit Select
                End Select

                count = position + 1
                position = InStr(count, data, Separator)
                splitterNumber += 1

                If splitterNumber > 3 Then
                    splitterNumber = 1
                    I += 1
                End If
            Loop Until position >= lenghtString

            ' вынесен из цикла
            If position = lenghtString Then
                aIndexDataServer(I) = CInt(Mid(data, count, position - count))
            End If

            PopulateListViewValueReceived()
            lengthMemo = countInString ' надо запомнить новое число
            stringDataSocket = vbNullString
            stringDataSocket = Join(NameParametersFromServer, Separator)
            stringDataSocket &= Separator

            If mainForm.ConfigurationString <> stringDataSocket Then 'AndAlso IsClient
                mainForm.ConfigurationString = stringDataSocket
                mainForm.NewChannelsInImitatorSnapshot()
                Exit Sub
            End If
        End If

        count = 1
        position = InStr(count, data, Separator)
        splitterNumber = 1
        I = 1

        Do
            If splitterNumber = 2 Then
                'aValueDataServer(I) = CDbl(Mid(eventArgs.data.Value, intСчетчик, intПозиция - intСчетчик))
                'aValueDataServer(I) = Double.Parse(Val(Mid(data, intСчетчик, intПозиция - intСчетчик))) ' CDbl(Mid(data, intСчетчик, intПозиция - intСчетчик))
                aValueDataServer(I) = Val(Mid(data, count, position - count))
            End If

            count = position + 1
            position = InStr(count, data, Separator)
            splitterNumber += 1

            If splitterNumber > 3 Then
                splitterNumber = 1
                I += 1
            End If
        Loop Until position >= lenghtString

        If Not mainForm.IsNewImitatorSnapshot Then 'IsClient AndAlso
            mainForm.DataValuesFromServer = aValueDataServer
            mainForm.AcquiredData()
        End If

        If IsFocusToClient Then RewriteList()
    End Sub

    Private Sub DataSocketReceive_ConnectionStatusUpdated(ByVal sender As Object, ByVal e As ConnectionStatusUpdatedEventArgs) Handles DataSocketReceive.ConnectionStatusUpdated
        ' DataSocket наблюдает изменение состояния подключения(связи), вызываемое в событии OnStatusUpdated.
        TextBoxStatus.Text = e.Message
    End Sub

    ''' <summary>
    ''' Количество обновлений экрана
    ''' </summary>
    Private countUpdateScreen As Integer
    ''' <summary>
    ''' Переписать Лист
    ''' </summary>
    Private Sub RewriteList()
        countUpdateScreen += 1

        If countUpdateScreen >= RefreshScreen Then
            With ListViewValueReceived ' обновить столбец значений для листа
                .BeginUpdate()
                For I As Integer = 1 To UBound(aValueDataServer)
                    .Items(I - 1).SubItems(1).Text = CStr(aValueDataServer(I))
                Next
                .EndUpdate()
            End With
            countUpdateScreen = 0
        End If
    End Sub
    ''' <summary>
    ''' Определить значения строк
    ''' </summary>
    Private Sub PopulateListViewValueReceived()
        With ListViewValueReceived ' обновить столбец значений для листа
            .BeginUpdate()
            For I As Integer = 1 To UBound(aNameDataServer)
                .Items(I - 1).Text = aNameDataServer(I)
                .Items(I - 1).SubItems(2).Text = CStr(aIndexDataServer(I))
            Next
            .EndUpdate()
        End With
    End Sub

    ''' <summary>
    ''' Определить размерность массива
    ''' </summary>
    ''' <param name="data"></param>
    Private Sub DetermineArrayDimension(ByRef data As String)
        ' по числу вхождений "\" определим размерность
        Dim countInString As Integer
        Dim lenghtString As Integer = Len(data) 'Len(data.Value)

        If lenghtString < 3 Then
            MessageBox.Show("Выполните пункт на Сервере <Данные в сеть>!", "Клиент", MessageBoxButtons.OK, MessageBoxIcon.Error)
            MainMdiForm.Close()
        End If

        Dim position As Integer = 0
        Do
            position = InStr(position + 2, data, Separator)
            countInString += 1
        Loop Until position >= lenghtString

        countInString \= 3
        lengthMemo = countInString
        Re.Dim(aNameDataServer, countInString)
        Re.Dim(aValueDataServer, countInString)
        Re.Dim(aIndexDataServer, countInString)
        Re.Dim(NameParametersFromServer, countInString)
        lengthArray = countInString
        AddListViewItem()
    End Sub

    ''' <summary>
    ''' Добавить строки листа
    ''' </summary>
    Private Sub AddListViewItem()
        Dim itmX As ListViewItem
        Dim I As Integer

        With ListViewValueReceived
            .Items.Clear()
            ' заполнить пустышками
            .BeginUpdate()
            For I = 1 To UBound(aNameDataServer)
                itmX = New ListViewItem(aNameDataServer(I)) With {
                    .ForeColor = ColorsNet((I - 1) Mod 7)
                }
                itmX.SubItems.Add(CStr(aValueDataServer(I)))
                itmX.SubItems.Add(CStr(aIndexDataServer(I)))
                .Items.Add(itmX)
            Next
            .EndUpdate()
        End With
    End Sub

    ''' <summary>
    ''' Дождаться пока соединение не установится
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub TimerReceive_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TimerReceive.Tick
        ' делаем соединение
        DataSocketReceive.Connect(TextBoxURL.Text, AccessMode.ReadAutoUpdate)
        Threading.Thread.Sleep(500)

        'If DataSocketReceive.Status = CWDSLib.CWDSStatus.cwdsConnectionActive AndAlso DataSocketReceive.LastError = 0 Then
        'If DataSocketReceive.Status = (CWDSLib.CWDSStatus.cwdsConnecting OrElse CWDSLib.CWDSStatus.cwdsConnectionActive) AndAlso DataSocketReceive.LastError = 0 Then
        If DataSocketReceive.LastError = 0 Then
            TimerReceive.Enabled = False

            If Not IsFormOpenedFromMenu Then
                WindowState = FormWindowState.Minimized
                IsFocusToClient = False
                ActivateMainForm()
            End If

            TimerUpdate.Interval = CInt(DeltaX * 1000)
            TimerUpdate.Enabled = True
        End If
    End Sub
    ''' <summary>
    ''' Развернуть основную форму
    ''' </summary>
    Private Sub ActivateMainForm()
        If RegistrationFormName <> "" Then
            For Each tempForm In CollectionForms.Forms.Values
                If tempForm.Text = RegistrationFormName Then
                    tempForm.WindowState = FormWindowState.Maximized
                    mainForm.Activate()
                    IsFormOpenedFromMenu = False
                    Exit For
                End If
            Next
        End If
    End Sub
End Class