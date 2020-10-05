Imports System.Globalization
Imports System.Net.NetworkInformation
Imports System.Threading
Imports System.Timers

Public Class PingClient
    Public Property PingDetails As String
    Public Property AddressBox As String
    Public Property SuccessPing As Boolean ' результат вызова класса
    Private WithEvents PingClient As New Ping()
    Private aTimer As Timers.Timer ' серверный таймер работает в другом потоке
    'Private WithEvents aTimer As System.Windows.Forms.Timer ' серверный таймер работает в потоке приложения
    Private syncPoint As Integer = 0
    Private startTime As DateTime
    Private Const TimerInterval As Integer = 50 ' миллисекунд
    Private Const WaitPingAnswer As Integer = 5 ' сек ждать Ответа Пинга
    Private isPingCompleted As Boolean ' признак получения какого-то результата из события

    Public Sub New(addressBox As String)
        Me.AddressBox = addressBox
        ' создать таймер с заданным интервалом.
        aTimer = New System.Timers.Timer(TimerInterval)
        'aTimer = New System.Windows.Forms.Timer
        ' обработчик события Elapsed таймера.
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
    End Sub

    'Private Sub aTimer_Tick(sender As Object, e As System.EventArgs) Handles aTimer.Tick
    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

        If sync = 0 Then
            Dim intervalTime As TimeSpan = DateTime.Now - startTime

            If isPingCompleted = True OrElse SuccessPing = True Then
                syncPoint = 0  ' освободить
                TimerStop()
                Exit Sub
            End If

            If intervalTime.TotalSeconds > WaitPingAnswer Then
                CancelPing() ' прервать пинг
                TimerStop()
            End If

            syncPoint = 0  ' освободить
        End If
    End Sub

    Private Sub TimerStop()
        aTimer.Stop()
    End Sub

    Private Sub PingClient_PingCompleted(ByVal sender As Object, ByVal e As PingCompletedEventArgs) Handles PingClient.PingCompleted
        ' Проверить возможную ошибку. Если нет ошибки, то показать 
        ' используемый адрес и время ответа в милисекундах
        If e.Error Is Nothing Then
            If e.Cancelled Then
                PingDetails &= "Ping прерван по истечению таймаута." & Environment.NewLine
            Else
                If e.Reply.Status = IPStatus.Success Then
                    PingDetails &= $"  {e.Reply.Address} {e.Reply.RoundtripTime.ToString(NumberFormatInfo.CurrentInfo)}ms {Environment.NewLine}"
                    SuccessPing = True ' единственый правильный результат
                Else
                    PingDetails &= $"  {GetStatusString(e.Reply.Status)}{Environment.NewLine}"
                End If
            End If
        Else
            ' Иначе показать ошибку
            PingDetails &= $"Ping error.{Environment.NewLine}Попытка пинга вызвала следующую ошибку: {e.Error.InnerException.Message}"
        End If

        isPingCompleted = True
        TimerStop()
    End Sub

    Private Function GetStatusString(ByVal status As IPStatus) As String
        Select Case status
            Case IPStatus.Success
                Return "Успешно."
            Case IPStatus.DestinationHostUnreachable
                Return "Целевой хост недоступен."
            Case IPStatus.DestinationNetworkUnreachable
                Return "Целевой сетевой адрес недоступен."
            Case IPStatus.DestinationPortUnreachable
                Return "Целевой сетевой порт недоступен."
            Case IPStatus.DestinationProtocolUnreachable
                Return "Целевой протокол недоступен."
            Case IPStatus.PacketTooBig
                Return "Пакет слишком большой."
            Case IPStatus.TtlExpired
                Return "TTL неисправен."
            Case IPStatus.ParameterProblem
                Return "Неверные параметры."
            Case IPStatus.SourceQuench
                Return "Источник замолчал."
            Case IPStatus.TimedOut
                Return "Время таймаута истекло."
            Case Else
                Return "Ошибка Ping."
        End Select
    End Function

    Public Sub SendPing()
        If AddressBox.Length <> 0 Then
            'pingDetails &= "Пингую " & addressBox & " . . ." & Environment.NewLine
            startTime = Date.Now
            ' Отправить ping запрос
            PingClient.SendAsync(AddressBox, Nothing)
            aTimer.Interval = TimerInterval
            aTimer.Enabled = True

            ' здесь ожидание срабатывания
            Do While aTimer.Enabled
                Thread.Sleep(100)
                Application.DoEvents()
            Loop
        Else
            PingDetails = "Пожалуйста введите IP address или host имя."
            SuccessPing = False
        End If
    End Sub

    ''' <summary>
    ''' Закончить любой незавершенный пинг.
    ''' </summary>
    Private Sub CancelPing()
        PingDetails &= "Ping прерван по истечению таймаута." & Environment.NewLine
        PingClient.SendAsyncCancel()
    End Sub
End Class

'Public Class IsThreadPool

'    <MTAThread()> _
'    Shared Sub MainIsThreadPool()
'        Dim autoEvent As New AutoResetEvent(False)

'        Dim regularThread As New Thread(AddressOf ThreadMethod)
'        regularThread.Start()
'        ThreadPool.QueueUserWorkItem(AddressOf WorkMethod, autoEvent)

'        ' Wait for foreground thread to end.
'        regularThread.Join()

'        ' Wait for background thread to end.
'        autoEvent.WaitOne()
'    End Sub

'    Shared Sub ThreadMethod()
'        Console.WriteLine("ThreadOne, executing ThreadMethod, " & _
'            "is from the thread pool? {0}", _
'            Thread.CurrentThread.IsThreadPoolThread)
'    End Sub

'    Shared Sub WorkMethod(stateInfo As Object)
'        Console.WriteLine("ThreadTwo, executing WorkMethod, " & _
'            "is from the thread pool? {0}", _
'            Thread.CurrentThread.IsThreadPoolThread)

'        ' Signal that this thread is finished.
'        DirectCast(stateInfo, AutoResetEvent).Set()
'    End Sub

'End Class