Imports NationalInstruments.Net
Imports System.Threading

Friend Class FormServer
    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        MdiParent = MainMdiForm
        Form_Initialize_Renamed()
    End Sub

    Private Sub Form_Initialize_Renamed()
        edURL.Text = AddressURL
    End Sub

    Friend Sub cmdConnect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConnect.Click
        If DataSocketSend.IsConnected Then DataSocketSend.Disconnect()
        DataSocketSend.Connect(edURL.Text, AccessMode.WriteAutoUpdate)
    End Sub

    Private Sub cmdDisconnect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDisconnect.Click
        ' Разъединить DataSocket из источника, с которым связан.
        DataSocketSend.Disconnect()
    End Sub

    Private Sub DataSocketSend_ConnectionStatusUpdated(ByVal sender As Object, ByVal e As ConnectionStatusUpdatedEventArgs) Handles DataSocketSend.ConnectionStatusUpdated
        ' соединение обновлено
        edStatus.Text = e.Message
    End Sub

    Private Sub FormServer_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        Size = New Size(390, 150)
        TimerConnect.Enabled = True
        CollectionForms.Add(Text, Me)
    End Sub

    Private Sub FormServer_Closed(ByVal eventSender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        DataSocketSend.Disconnect()
        IsServerOn = False
        IsFormSereverStart = False
        CollectionForms.Remove(Text)
        Application.DoEvents()
    End Sub

    Private Sub TimerConnect_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TimerConnect.Tick
        DataSocketSend.Connect(edURL.Text, AccessMode.WriteAutoUpdate)
        Application.DoEvents()
        Thread.Sleep(100)

        If DataSocketSend.LastError = 0 Then
            TimerConnect.Enabled = False
            IsServerOn = True ' установить глобальный флаг для передачи данных

            If RegistrationFormName <> "" Then
                For Each tempForm In CollectionForms.Forms.Values
                    If tempForm.Text = RegistrationFormName Then
                        tempForm.WindowState = FormWindowState.Maximized
                        tempForm.Activate()
                        Application.DoEvents()

                        If CBool(CType(tempForm, FormMain).KindFormExamination And (FormExamination.RegistrationSCXI Or FormExamination.RegistrationTCP Or FormExamination.RegistrationCompactRio)) Then
                            RunRegistrationForm()
                        End If

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdQuit.Click
        ' DataSocket разъединит автоматически при закрытии.
        IsFocusToClient = False
        ActivateFormMain()
    End Sub

    ''' <summary>
    ''' Развернуть Основную Форму
    ''' </summary>
    Private Sub ActivateFormMain()
        For Each tempForm In CollectionForms.Forms.Values
            If CStr(tempForm.Tag) = TagFormDaughter Then
                tempForm.WindowState = FormWindowState.Maximized
                tempForm.Activate()
                Exit For
            End If
        Next
    End Sub
End Class