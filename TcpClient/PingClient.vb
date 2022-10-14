Imports System.Globalization
Imports System.Net.NetworkInformation
Imports System.Threading
Imports System.Timers

''' <summary>
''' Проверка доступности компьютера по сети
''' </summary>
Public Class PingClient
    Implements IDisposable

    Public Property PingDetails As String
    Public Property AddressBox As String
    Public Property SuccessPing As Boolean              ' результат вызова класса

    Private WithEvents infoPing As New Ping()           ' Позволяет приложению определить, доступен ли удаленный компьютер по сети.

    Private ReadOnly timerInterval As Integer = 50      ' миллисекунд
    Private ReadOnly waitResponseFromPing As Integer = 5 ' ждать ответа пинга сек
    Private aTimer As System.Timers.Timer               ' серверный таймер работает в другом потоке
    Private syncPoint As Integer = 0
    Private startTime As DateTime
    Private isPingCompleted As Boolean                   ' признак получения какого-то результата из события

    Public Sub New(addressBox As String)
        Me.AddressBox = addressBox
        ' Создать таймер с заданным интервалом.
        aTimer = New System.Timers.Timer(timerInterval)
        ' назначить делегата обработки события таймера.
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
    End Sub

    Public Sub New(addressBox As String, timerInterval As Integer, waitResponse As Integer)
        Me.AddressBox = addressBox
        Me.timerInterval = timerInterval
        Me.waitResponseFromPing = waitResponse

        ' Создать таймер с заданным интервалом.
        aTimer = New System.Timers.Timer(timerInterval)
        ' назначить делегата обработки события таймера.
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
    End Sub

    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

        If sync = 0 Then
            Dim intervalTime As TimeSpan = DateTime.Now - startTime

            If isPingCompleted Then
                If SuccessPing Then
                    syncPoint = 0  ' освободить
                    TimerStop()
                    Exit Sub
                Else
                    infoPing.SendAsync(AddressBox, Nothing)
                End If
            End If

            If intervalTime.TotalSeconds > waitResponseFromPing Then
                CancelPing() ' прервать пинг
                TimerStop()
            End If

            syncPoint = 0  ' освободить
        End If
    End Sub

    Private Sub TimerStop()
        aTimer.Stop()
    End Sub

    ''' <summary>
    ''' Завершение ассинхронной операции проверки связи
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InfoPing_PingCompleted(ByVal sender As Object, ByVal e As PingCompletedEventArgs) Handles infoPing.PingCompleted
        ' Проверить возможную ошибку.  Если нет ошибки, то показать 
        ' используемый адрес и время ответа в милисекундах.
        If e.Error Is Nothing Then
            If e.Cancelled Then
                PingDetails &= "Ping прерван по истечению таймаута." & Environment.NewLine
            Else
                If e.Reply.Status = IPStatus.Success Then
                    PingDetails &= $"  {e.Reply.Address} {e.Reply.RoundtripTime.ToString(NumberFormatInfo.CurrentInfo)}ms {Environment.NewLine}"
                    SuccessPing = True ' единственый правильный результат
                    isPingCompleted = True
                    TimerStop()
                Else
                    PingDetails &= $"  {GetStatusString(e.Reply.Status)}{Environment.NewLine}"
                End If
            End If
        Else
            ' Иначе показать ошибку
            PingDetails &= $"Ping error.{Environment.NewLine}Попытка пинга вызвала следующую ошибку. {e.Error.InnerException.Message}"
        End If

        isPingCompleted = True
    End Sub

    ''' <summary>
    ''' Отдаёт вразумительное сообщение о состоянии отправки сообщения проверки связи по протоколу ICMP на компьютер.
    ''' </summary>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetStatusString(ByVal status As IPStatus) As String
        Select Case status
            Case IPStatus.Success
                Return "Успешно."
            Case IPStatus.DestinationHostUnreachable
                Return "Целевой хост недоступен."
            Case IPStatus.DestinationNetworkUnreachable
                Return "Целевой сетевой адресс недоступен."
            Case IPStatus.DestinationPortUnreachable
                Return "Целевой сетевой порт недоступен."
            Case IPStatus.DestinationProtocolUnreachable
                Return "Целевой протокол недоступен."
            Case IPStatus.PacketTooBig
                Return "Пакет слишком большой."
            Case IPStatus.TtlExpired
                Return "TTL неисправен. Срок жизни пакета стал равным нулю, в следствие чего он был удален маршрутищирующим узелом."
            Case IPStatus.ParameterProblem
                Return "Неверные параметры. В заголовке содержатся недопустимые данные полей или нераспознанный параметр."
            Case IPStatus.SourceQuench
                Return "Источник замолчал. Пакет был удален. Это происходит, если в очереди вывода компьютера-источника недостаточно места для хранения или если пакеты приходят к получателю слишком быстро, чтобы успеть их обработать."
            Case IPStatus.TimedOut
                Return "Время таймаута истекло. Значение допустимого времени ожидания ответа по умолчанию — 5 секунд."
            Case Else
                Return "Ошибка Ping."
        End Select
    End Function

    ''' <summary>
    ''' Вызов из потока задачи или приложения проверку доступности компьютера по сети
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendPing()
        If AddressBox.Length <> 0 Then
            startTime = Date.Now
            ' Отправить ping запрос
            infoPing.SendAsync(AddressBox, Nothing)
            aTimer.Interval = timerInterval
            aTimer.Enabled = True

            ' здесь ожидание срабатывания ответа Ping или окончания работы таймера опроса результата
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
        infoPing.SendAsyncCancel()
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' флаг не более одного раза Чтобы обнаружить избыточные вызовы

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' сборщик мусора не должен иметь сюда доступ
                ' освободить управляемое состояние (управляемые объекты).
                If aTimer IsNot Nothing Then
                    If aTimer.Enabled Then aTimer.Stop()
                    aTimer.Dispose()
                    aTimer = Nothing
                End If

                infoPing = Nothing
            End If

            ' освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже Finalize().
            ' задать большие поля как null.
        End If
        Me.disposedValue = True
    End Sub

    ' Этот код добавлен редактором Visual Basic для правильной реализации шаблона высвобождаемого класса.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Не изменяйте этот код. Разместите код очистки выше в методе Dispose(disposing As Boolean).
        Dispose(True) ' это вызывает пользователь, значит все управляемуе и неуправляемые ресурсы ассоциированные с данным объектом должны быть очищены
        GC.SuppressFinalize(Me) ' сообщает о том, что класс не нуждается в вызове деструктора(сборщик мусора вызывать ещё не нужно)
    End Sub
#End Region

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