Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System.Text
Imports MathematicalLibrary

''' <summary>
''' Инкапсулирует клиентское соединение и предоставляет объект состояния для асинхронных операций чтения.
''' </summary>
''' <remarks></remarks>
Public Class ConnectionInfoClient
    Public Property IsCancel As Boolean
    Public Property PacketsThresholdCount As Integer ' для обновления экрана
    Public Property PacketsThresholdSendCount As Integer ' для обновления экрана
    Public Property PacketsReceive As Long ' всего получено
    Public Property PacketsSend As Long ' всего отправлено
    Public Property IsEnableStartTimer As Boolean ' если разбор конфигурации успешен Запускать Таймер Можно
    Public Property AcquisitionValueOfDouble As Double() ' полученные данные каналов от Сервера получаются потребителями из основного окна, 
    Public Property CountSelectedNameChannels As Integer     ' число все каналы данных
    'Public Property МожноВызыватьСново As Boolean ' Можно Вызывать Сново

    Private msgReqParList_Insys_sbk As Byte() ' подготовленный эапрос всех физических данных
    Private arrSelectNameChannels As String()
    Private timerInterval As Integer = 10 ' миллисекунд
    Private timerIntervalSend As Integer = timerInterval \ 2 ' миллисекунд
    Private WithEvents mmTimer As Multimedia.Timer
    'Friend WithEvents Timer1 As System.Windows.Forms.Timer

    Private syncPoint As Integer = 0 ' для синхронизации

    ' Буфер сделать переменным объёмом
    Private Const RECEIVE_BUFFER_SIZE As Integer = 8192 '60000 'TODO: 30000 ' по всей видимости реально пакеты с числом каналов порядка 1300 вписываются
    Private Const BUFFER_SIZE_MEASURE_DATA As Integer = 8192 ' 1024 для приёма набранного пакета значений
    Private _Buffer(RECEIVE_BUFFER_SIZE) As Byte
    Private Const CAPACITY As Integer = 127 ' ёмкость очереди

    Private keyConfig As Integer ' последняя загруженная конфигурация LastTCPkeyConfig
    ' а передаются с частотой сбора таймера как параметр делегата события 
    Private countAwaitData As Integer ' для контроля зависания
    Private oldCountAwaitData As Integer  ' для проверки зависания делегата чтения сокета
    Private varCountWait As Integer = 1000
    Private Const TIME_WAIT As Integer = 15  ' 15 секунд ждать зависания
    Private countWait As Integer

    Public _AppendMethod As Action(Of String, MessageBoxIcon) ' делегат как свойство не может быть непосредственно вызван, поэтому сделан как публичный член, значение которому присваивается через свойство
    ''' <summary>
    ''' Метод в основном окне для вывода сообщения
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AppendMethod As Action(Of String, MessageBoxIcon)
        Get
            Return _AppendMethod
        End Get
        Set(value As Action(Of String, MessageBoxIcon))
            _AppendMethod = value
        End Set
    End Property

    Private _StopMethod As Action(Of Boolean)
    ''' <summary>
    ''' Метод в основном окне для вызова остановки сбора
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StopMethod As Action(Of Boolean)
        Get
            Return _StopMethod
        End Get
        Set(value As Action(Of Boolean))
            _StopMethod = value
        End Set
    End Property

    Private _Tcp_Client As TcpClient
    Public ReadOnly Property Tcp_Client As TcpClient
        Get
            Return _Tcp_Client
        End Get
    End Property

    Private _Stream As NetworkStream
    Public ReadOnly Property Stream As NetworkStream
        Get
            Return _Stream
        End Get
    End Property

    Private _LastReadLength As Integer
    Public ReadOnly Property LastReadLength As Integer
        Get
            Return _LastReadLength
        End Get
    End Property

    Private ReadOnly remoteEndPoint As EndPoint
    Private ReadOnly localEndPoint As EndPoint

    ''' <summary>
    ''' чтение endpoints
    ''' </summary>
    Public ReadOnly Property Remote() As EndPoint
        Get
            Return remoteEndPoint
        End Get
    End Property

    ''' <summary>
    ''' чтение local point
    ''' </summary>
    Public ReadOnly Property Local() As EndPoint
        Get
            Return localEndPoint
        End Get
    End Property

    Private _DataQueue As Queue(Of Byte())
    Public ReadOnly Property DataQueue As Queue(Of Byte())
        <MethodImplAttribute(MethodImplOptions.Synchronized)>
        Get
            Return _DataQueue
        End Get
    End Property

    Private _FrequencyAcquisition As Integer
    Private Property FrequencyAcquisition() As Integer
        Get
            Return _FrequencyAcquisition
        End Get
        Set(ByVal value As Integer)
            _FrequencyAcquisition = value
            timerInterval = 1000 \ value
            timerIntervalSend = 500 \ value ' в 2 раза быстрее
            varCountWait = TIME_WAIT * value ' сколько секунд ждать зависания
        End Set
    End Property
    Public Property IsStartAcquisition() As Boolean

    'Public Event SyncSocketClientAcquiredData()
    'или
    Public Event SyncSocketClientAcquiredData(ByVal sender As Object, ByVal e As AcquiredDataEventArgs)

    ''' <summary>
    ''' AcquiredDataEventArgs: пользовательское событие наследуется от EventArgs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AcquiredDataEventArgs
        Inherits EventArgs

        ' получить массив собранных значений
        Public Sub New(ByRef arrData As Double())
            ArrDataTCP = arrData
        End Sub

        'Public Sub New()
        'End Sub
        ' сюда можно напихать другие свойства

        ' можно передать все накопленные
        ' arrСреднее(TempПараметр.ИндексВМассивеПараметров, x)
        ' а можно и конкретно осредненные или собранные
        'arrПарамНакопленные(N) = dblСреднее
        Public Property ArrDataTCP() As Double()
    End Class

    ' Размер буфера имеет случайное значение которое должно быть определяться на основе
    ' количества данных что нужно передать, скорости передачи и ожидаемое число клиентов.
    ' Предположения по дизайну коммуникационного протокола для передачи данных и размер для чтения буфера
    ' должны быть основаны на разрабатываемом протоколе.
    Public Property LostBytes As Byte() ' временный буфер

    Private ParametersIndex As Integer()
    Private ParametersIndexCopy As Integer() ' глобальный для копирования локального
    Private ParametersName As String() ' для поиска отсутствующих каналов arrНаименование2.Contains(rowChannel.НаименованиеПараметра)
    Private SendDataPakcetOutByte As Byte()

    Public Sub New(address As IPAddress, port As Integer, append As Action(Of String, MessageBoxIcon))
        Initialize()

        'TaskSendReciveWithServer = New Task(Sub() DoMonitorConnectionsWithServer(Nothing)) ' пустышка
        _AppendMethod = append
        _StopMethod = Nothing

        _Tcp_Client = New TcpClient
        _Tcp_Client.Connect(address, port)
        ' для соединения с Сервером пока выключил
        '_Tcp_Client.NoDelay = True ' true, если задержка отключена
        '_Client.Client.DontFragment = True ' Установка этого свойства на сокете протокола TCP (Transmission Control Protocol) не будет иметь эффекта.

        _Tcp_Client.ReceiveTimeout = 5000
        _Tcp_Client.SendTimeout = 5000

        ' Возвращает или задает основной объект Socket.
        _Tcp_Client.Client.ReceiveBufferSize = RECEIVE_BUFFER_SIZE
        _Tcp_Client.Client.SendBufferSize = BUFFER_SIZE_MEASURE_DATA

        _Stream = _Tcp_Client.GetStream

        remoteEndPoint = _Tcp_Client.Client.RemoteEndPoint
        localEndPoint = _Tcp_Client.Client.LocalEndPoint

        _DataQueue = New Queue(Of Byte())(CAPACITY)
        LostBytes = New Byte() {}
        'If ЗапускатьТаймерМожно Then  StartAcquisition()
    End Sub

    ''' <summary>
    ''' Запустить Сбор
    ''' </summary>
    Public Sub StartAcquisitionTCP()
        If IsEnableStartTimer Then StartAcquisitionTimer()
    End Sub

    ''' <summary>
    ''' Начальная инициализация массивов используемых в программе в зависимости от каналов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ' Часть от CharacteristicForRegime
        Dim I, J As Integer

        IsEnableStartTimer = False
        LoadChannelsTCPclient() ' т.к. массив структуры arrПараметры при работе с контроллером в процессе испытаний не менняется, а с ТСРКлиент меняется, необходимо загрузить
        DecodingString() ' (strСтрокаКонфигурации)
        'ReDim_ParametersIndex(0)
        Re.Dim(ParametersIndex, 0)

        ' Массив arrСписокПараметров() состоит только из параметров подлежащих измерению
        For I = 1 To UBound(ParametersName)
            For J = 1 To UBound(ParametersTCP)
                If ParametersTCP(J).NameParameter = ParametersName(I) Then
                    'ReDimPreserve ParametersIndex(UBound(ParametersIndex) + 1)
                    Re.DimPreserve(ParametersIndex, UBound(ParametersIndex) + 1)
                    ParametersIndex(UBound(ParametersIndex)) = J
                    Exit For
                End If
            Next
        Next

        If Not IsNothing(ParametersIndex) Then
            'ReDim_ParametersIndexCopy(ParametersIndex.Length - 1)
            Re.Dim(ParametersIndexCopy, ParametersIndex.Length - 1)
            Array.Copy(ParametersIndex, ParametersIndexCopy, ParametersIndex.Length)
        End If

        ' Часть от Непрерывный()
        ' 1.сформировать  m_OutputChannelsBindingList(используется в _ConnectionClient для формирования списка каналов опроса) на основании записанной конфигурации каналов
        ' 2.т.к. используются экранные имена clsChannelOutput.НаименованиеПараметра (не исключён повтор имён) то сделать поиск clsChannelOutput.Name
        ' 3.arrПараметры - arrНаименование2 - arrСписокПараметров(номера)

        If LoadOptionsTcpClient() Then
            If TuneByStendConfiguration() Then
                FrequencyAcquisition = FrequencyBackground
                ' подготовка
                ' эагрузка ArrSelectNameChannels
                ' проверить что инициализация проведена
                arrSelectNameChannels = New String(OutputChannelsBindingList.Count - 1) {}
                CountSelectedNameChannels = 0

                For I = 0 To OutputChannelsBindingList.Count - 1
                    arrSelectNameChannels(I) = OutputChannelsBindingList(I).Name
                Next

                If arrSelectNameChannels IsNot Nothing Then
                    CountSelectedNameChannels = arrSelectNameChannels.Length
                    'ReDim_AcquisitionValueOfDouble(CountSelectedNameChannels - 1)
                    Re.Dim(AcquisitionValueOfDouble, CountSelectedNameChannels - 1)
                End If

                msgReqParList_Insys_sbk = ReqParList_Insys_sbk(arrSelectNameChannels)
                IsEnableStartTimer = True
            End If
        End If
    End Sub

    Public Sub Close()
        If _Tcp_Client IsNot Nothing Then
            StopAcquisition()
            ' gMainForm.StopAcquisition()
            ' новое: Когда прием и отправка данных завершены, использовать метод Shutdown для того, 
            ' чтобы отключить объект Socket. После вызова метода Shutdown обратиться к методу Close, 
            ' чтобы освободить все связанные с объектом Socket ресурсы.
            _Tcp_Client.Client.Shutdown(SocketShutdown.Both)
            _Tcp_Client.Close()
        End If

        _Tcp_Client = Nothing
        _Stream = Nothing
    End Sub

#Region "Вспомогательные"
    ' ''' <summary>
    ' ''' Объединить массив имён каналов времени и данных
    ' ''' </summary>
    ' ''' <param name="ArrSelectNameChannels"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function BuildArrayString(ArrSelectNameChannels As String()) As String() '(ArrSelectTimeChannels As String(), ArrSelectNameChannels As String()) As String()
    '    Dim Arr_Chs() As String = Nothing ' объединённый кластер имён 
    '    Dim list_Chs As New List(Of String)

    '    'If ArrSelectTimeChannels IsNot Nothing Then
    '    '    If ArrSelectTimeChannels.Length > 0 Then
    '    '        For Each NameChannel As String In ArrSelectTimeChannels
    '    '            list_Chs.Add(NameChannel)
    '    '        Next
    '    '    End If
    '    'End If

    '    If ArrSelectNameChannels IsNot Nothing Then
    '        If ArrSelectNameChannels.Length > 0 Then
    '            For Each NameChannel As String In ArrSelectNameChannels
    '                list_Chs.Add(NameChannel)
    '            Next
    '        End If
    '    End If
    '    'используя  LINQ
    '    'If ArrSelectTimeChannels IsNot Nothing Then
    '    '    If ArrSelectTimeChannels.Length > 0 Then
    '    '        Arr_Chs = ArrSelectTimeChannels.ToArray
    '    '    End If
    '    'End If

    '    'If ArrSelectNameChannels IsNot Nothing Then
    '    '    If ArrSelectNameChannels.Length > 0 Then
    '    '        If Arr_Chs IsNot Nothing Then
    '    '            Dim allNumbers = Arr_Chs.Concat(ArrSelectNameChannels)
    '    '            Arr_Chs = allNumbers.ToArray
    '    '        Else
    '    '            Arr_Chs = ArrSelectNameChannels.ToArray
    '    '        End If
    '    '    End If
    '    'End If

    '    If list_Chs.Count > 0 Then Arr_Chs = list_Chs.ToArray
    '    Return Arr_Chs
    'End Function

#End Region

#Region "LabView"
    ''' <summary>
    ''' Формирование битового запроса всех выходных физических данных каналов
    ''' </summary>
    ''' <param name="ArrNameChannels"></param>
    ''' <param name="Time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReqParList_Insys_sbk(ArrNameChannels() As String, Optional Time As UInt32 = 0) As Byte()
        ' переменная Time может не использоваться
        ' выход Packet out - пакет запроса с серверу
        ' (можно оптимизировать и сделать суммирование бит)
        Const COMMAND As Byte = 100 ' Алекса описание: CMD_GETDATA - данный пакет, запрашивает данные каналов сервера с учетом коэффициентов полинома. Список HASH сумм запрашиваемых пакетов указывается в поле "Данные". В поле "Количество HASH сумм" указывается количество HASH сумм в списке запрашиваемых пакетов.
        Dim ChannelsCount As UShort = CUShort(ArrNameChannels.Count)
        Dim PakcetOutByteChannels((ChannelsCount * 4) - 1) As Byte ' 12 байт заголовок, ChannelsCount * 4 -содержит список Хеш каналов
        Dim PakcetOutByteHeader(0) As Byte ' Заголовок
        Const HasUInt32 As UInt32 = 0

        PakcetOutByteHeader(0) = COMMAND
        PakcetOutByteHeader = (PakcetOutByteHeader.Concat(BitConverter.GetBytes(HasUInt32))).ToArray()
        ' добавить бит валидности
        Dim valid(0) As Byte
        valid(0) = 0
        PakcetOutByteHeader = PakcetOutByteHeader.Concat(valid).ToArray
        ' добавить Time
        PakcetOutByteHeader = (PakcetOutByteHeader.Concat(BitConverter.GetBytes(Time))).ToArray()
        ' добавить  число хеш каналов
        PakcetOutByteHeader = (PakcetOutByteHeader.Concat(BitConverter.GetBytes(ChannelsCount))).ToArray()

        ' теперь должно получиться 12 байт
        Dim DestinationIndex As Integer
        For Each itemName As String In ArrNameChannels
            Array.Copy(BitConverter.GetBytes(NameToHash(itemName)), 0, PakcetOutByteChannels, DestinationIndex, 4)
            DestinationIndex += 4
        Next

        Return (PakcetOutByteHeader.Concat(PakcetOutByteChannels)).ToArray()
    End Function
#End Region

    ''' <summary>
    ''' Запуск таймера
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StartAcquisitionTimer()
        syncPoint = 0
        mmTimer = New Multimedia.Timer() With {.Mode = Multimedia.TimerMode.Periodic,
                                               .Period = timerInterval,
                                               .Resolution = 1}

        Thread.CurrentThread.Priority = ThreadPriority.Normal

        ' для отслеживания события в форме назначить объект синхронизации (должен быть компонент)
        ' если таймер работает самостоятельно, то форму назначать не надо
        mmTimer.SynchronizingObject = MainMdiForm 'gMainForm ' необходимо для отслеживания вызова событий

        'Timer1 = New System.Windows.Forms.Timer ' (components)
        'Timer1.Interval = TimerInterval
        Try
            'Timer1.Enabled = True 
            RunSendRecive(True)
            mmTimer.Start()
        Catch ex As Exception
            Const CAPTION As String = "Error StartAcquisition"
            Dim text As String = ex.ToString
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            RegistrationEventLog.EventLog_CONNECT_FAILED(String.Format("<{0}> {1}", CAPTION, text))
        End Try

        IsStartAcquisition = True
    End Sub

    ''' <summary>
    ''' Остановить таймер
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopAcquisition()
        'Timer1.Enabled = False 
        RunSendRecive(False)
        If mmTimer IsNot Nothing Then mmTimer.Stop() ' может быть не создан. если было нарушение соотвествия конфигурации каналов Сервера
        IsStartAcquisition = False
    End Sub

    Private Sub MmTimer_Tick(sender As Object, e As EventArgs) Handles mmTimer.Tick
        If IsStartAcquisition AndAlso _Tcp_Client IsNot Nothing AndAlso _Tcp_Client.Connected Then
            ' _OnTimedEvent()' синхронное чтение
            OnTimed_mmTimer() ' асинхронное чтение
        Else
            'Timer1.Enabled = False 
            mmTimer.Stop()
            RunSendRecive(False)
        End If
    End Sub

    Private TokenSource As CancellationTokenSource
    Private taskSendReciveWithServer As Task

    Public Sub RunSendRecive(sendStart As Boolean)
        If sendStart Then
            'TestSend = "RunSendRecive"
            TokenSource = New CancellationTokenSource
            taskSendReciveWithServer = Task.Factory.StartNew(Sub() LoopSendReciveWithServer(TokenSource.Token), TokenSource.Token, TaskCreationOptions.LongRunning)
        Else
            If TokenSource IsNot Nothing Then TokenSource.Cancel() ' прервать задачу 
            taskSendReciveWithServer = Nothing
        End If
    End Sub

    Private Sub LoopSendReciveWithServer(ByVal ct As CancellationToken)
        ' Прерывание уже было запрошено?
        If ct.IsCancellationRequested = True Then
            'Console.WriteLine("Прерывание уже было запрошено до запуска.")
            'Console.WriteLine("Press Enter to quit.")
            ct.ThrowIfCancellationRequested()
        End If

        ' Внимание!!! Ошибка "OperationCanceledException was unhandled by user code"
        ' было вызвано здесь если "Just My Code"
        ' был включён и не может быть выключен. Исключение случилось
        ' Просто нажать F5 для продолжения выполнения кода

        'If TaskSendReciveWithServer IsNot Nothing Then
        'TestSend = "DoMonitorConnectionsWithServer TaskSendReciveWithServer=True"
        Do
            If ct.IsCancellationRequested Then
                'ct.ThrowIfCancellationRequested() ' выйти по исключению
                Exit Do ' завершить задачу
            End If

            If taskSendReciveWithServer Is Nothing Then
                ' скорее всего обрыв соединения
                Exit Sub
            End If

            StreamWritePacketToServer()

            ' Завершить цикл избегая напрасную трату времени CPU
            If taskSendReciveWithServer IsNot Nothing Then taskSendReciveWithServer.Wait(timerIntervalSend)
        Loop While True
        'Else
        ' TestSend = "DoMonitorConnectionsWithServer TaskSendReciveWithServer=False"
        'End If
    End Sub

    'Private Shared MonitorConnHandlerLockObject As New Object ' объект синхронизации для блокирования доступа
    'Private OnTimedEventLock As New Object
    Private sumArrByte As Byte()
    ''' <summary>
    ''' Общая функция для всех таймеров
    ''' Осуществляет посылку, приём и разборку по массивам полученных данных на синхронном блокирующем сокете
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnTimed_mmTimer()    ' <MethodImplAttribute(MethodImplOptions.Synchronized)> по всей видимости тормозит
        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

        If sync = 0 Then
            If _Tcp_Client.Connected Then ' не знаю, что правильнее 'If _Client.Client.Connected Then
                'Try
                '    'If gMainForm.СетевыеПеременныеПроверены AndAlso _Client.Client.Connected AndAlso ClientSendData Then
                '    If gMainForm.СетевыеПеременныеПроверены AndAlso _Tcp_Client.Connected AndAlso ClientSendData Then
                '        ''*****************************ЗАКОМЕНТИРОВАЛ***********************
                '        '' отправить на Сервер запрос на получение каналов для визуализации и идущих в формулы и в расчёт
                '        'If _Stream.CanWrite Then
                '        '    '_Stream.Write(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
                '        '    _Stream.BeginWrite(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
                '        '    'WaitOne()
                '        'End If
                '        ''*****************************ЗАКОМЕНТИРОВАЛ***********************

                '        ' подготовить массив со значениями сетевых переменных всех видов для передачи их Серверу
                '        If varEditNetworkVariablesForm Is Nothing AndAlso gMainForm.TimerChekNetVar.Enabled = False Then
                '            Dim I As Integer
                '            For Each mШасси As Шасси In mВсеШасси.ListВсеШасси
                '                For Each mСетеваяПеременная As СетеваяПеременная In mШасси.ListСетеваяПеременная
                '                    DataAllNetVarChassis(I) = mСетеваяПеременная.ValueDouble
                '                    I += 1
                '                Next
                '            Next
                '            ' получить пакет для отправки
                '        End If
                '        SendDataPakcetOutByte = ComposePacket(PacketArray, DataAllNetVarChassis, gManagerChassis.ChPinHashAllNetVar)

                '        '''*****************************ЗАКОМЕНТИРОВАЛ***********************
                '        ''TODO:   
                '        'Thread.Sleep(1) '(1)
                '        ''SyncLock OnTimedEventLock 'не спасает
                '        'If _Tcp_Client.Connected AndAlso ClientSendData Then 'If _Client.Client.Connected AndAlso ClientSendData Then
                '        '    If _Stream.CanWrite Then
                '        '        '_Stream.Write(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
                '        '        _Stream.BeginWrite(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
                '        '    End If

                '        '    'TODO:                       проба()
                '        '    'If МожноВызыватьСново Then AwaitData(Me)
                '        '    'AwaitData(Me)
                '        '    'Thread.Sleep(1)
                '        'End If
                '        ''End SyncLock
                '        ' ''*****************************ЗАКОМЕНТИРОВАЛ***********************

                '        ' ''*****************************ЗАМЕНИЛ НА ОТПРАВКУ ОДНОГО ОБЩЕГО БУФЕРА***********************
                '        'SendDataPakcetOutByte = (SendDataPakcetOutByte.Concat(msgReqParList_Insys_sbk)).ToArray 'не работает
                '        SumArr = (msgReqParList_Insys_sbk.Concat(SendDataPakcetOutByte)).ToArray


                '        If _Stream.CanWrite Then
                '            '_Stream.BeginWrite(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
                '            '_Stream.BeginWrite(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '

                '            '_Stream.Write(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length) ' Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
                '            '_Stream.Write(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length) ' Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
                '            _Stream.BeginWrite(SumArr, 0, SumArr.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '

                '        End If
                '        ' ''*****************************ЗАМЕНИЛ***********************

                '        If OldCountAwaitData = CountAwaitData Then 'зависание делегата асинхронного чтения 
                '            CountWait += 1
                '            If CountWait > constCountWait Then
                '                ' остановить и возобновить связь с сервером
                '                _AppendMethod("Делегат асинхронного чтения из входных сетевых буферов не вернул управления.", MessageBoxIcon.Error)
                '                _StopMethod(False)
                '                CountWait = 0
                '                OldCountAwaitData = -1
                '                CountAwaitData = 0
                '            End If
                '        Else
                '            CountWait = 0
                '            OldCountAwaitData = CountAwaitData
                '        End If
                '    End If
                'Catch ex As SocketException
                '    _AppendMethod("Socket exception: " + ex.SocketErrorCode.ToString, MessageBoxIcon.Error)
                'Catch ex As Exception
                '    _AppendMethod("Exception: " & Convert.ToString(ex), MessageBoxIcon.Error)
                'End Try

                'TestSend = "_OnTimed_mmTimer"

                Dim fireAcquiredDataEventArgs As New AcquiredDataEventArgs(AcquisitionValueOfDouble)
                RaiseEvent SyncSocketClientAcquiredData(Me, fireAcquiredDataEventArgs)
                'Application.DoEvents()' теряются необработанные события
                syncPoint = 0 ' освободить
            Else
                syncPoint = 0  ' освободить
                StopAcquisition()
            End If
        End If
    End Sub

    ' ''' <summary>
    ' ''' В событии таймера происходит отправка данных Серверу
    ' ''' при отправки данных от мультимедийного таймера возможны ошибки из-за его многопоточности
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    '    _OnTimer1TickEventAsync()
    'End Sub

    ''' <summary>
    ''' Асинхронная запись запроса для получения значений сконфигурированных кналов
    ''' </summary>
    Private Sub StreamWritePacketToServer()
        If _Tcp_Client Is Nothing Then
            'TestSend = "StreamWrite Exit Sub"
            Exit Sub
        End If

        If _Tcp_Client.Connected Then ' не знаю, что правильнее 'If _Client.Client.Connected Then
            Try
                If _Tcp_Client.Connected AndAlso gClientSendData Then
                    SendDataPakcetOutByte = ComposePacket(gPacketArray)

                    '*****************************ЗАМЕНИЛ НА ОТПРАВКУ ОДНОГО ОБЩЕГО БУФЕРА***********************
                    'SendDataPakcetOutByte = (SendDataPakcetOutByte.Concat(msgReqParList_Insys_sbk)).ToArray 'не работает
                    sumArrByte = (msgReqParList_Insys_sbk.Concat(SendDataPakcetOutByte)).ToArray

                    If _Stream.CanWrite Then
                        '_Stream.BeginWrite(sumArrByte, 0, sumArrByte.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
                        '_Stream.BeginWrite(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
                        _Stream.BeginWrite(sumArrByte, 0, sumArrByte.Length, New AsyncCallback(AddressOf MyWriteCallBack), Me) '_Stream) '

                        'TestSend = "StreamWrite"
                    End If

                    If oldCountAwaitData = countAwaitData Then ' зависание делегата асинхронного чтения 
                        countWait += 1

                        If countWait > varCountWait Then
                            ' остановить и возобновить связь с сервером
                            _AppendMethod("Делегат асинхронного чтения из входных сетевых буферов не вернул управления.", MessageBoxIcon.Error)
                            _StopMethod(False)
                            countWait = 0
                            oldCountAwaitData = -1
                            countAwaitData = 0
                        End If
                    Else
                        countWait = 0
                        oldCountAwaitData = countAwaitData
                    End If
                End If
            Catch ex As SocketException
                _AppendMethod("Socket exception: " & ex.SocketErrorCode.ToString, MessageBoxIcon.Error)
            Catch ex As Exception
                _AppendMethod("Exception: " & Convert.ToString(ex), MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    'Private Sub _OnTimedEventAsync()    '<MethodImplAttribute(MethodImplOptions.Synchronized)> по всей видимости тормозит
    '    Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

    '    If sync = 0 Then
    '        If _Tcp_Client.Connected Then ' не знаю, что правильнее 'If _Client.Client.Connected Then
    '            Try
    '                'If gMainForm.СетевыеПеременныеПроверены AndAlso _Client.Client.Connected AndAlso ClientSendData Then
    '                If gMainForm.СетевыеПеременныеПроверены AndAlso _Tcp_Client.Connected AndAlso ClientSendData Then
    '                    ''*****************************ЗАКОМЕНТИРОВАЛ***********************
    '                    ''отправить на Сервер запрос на получение каналов для визуализации и идущих в формулы и в расчёт
    '                    'If _Stream.CanWrite Then
    '                    '    '_Stream.Write(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                    '    _Stream.BeginWrite(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
    '                    '    'WaitOne()
    '                    'End If
    '                    ''*****************************ЗАКОМЕНТИРОВАЛ***********************

    '                    ' подготовить массив со значениями сетевых переменных всех видов для передачи их Серверу
    '                    If varEditNetworkVariablesForm Is Nothing AndAlso gMainForm.TimerChekNetVar.Enabled = False Then
    '                        Dim I As Integer
    '                        For Each mШасси As Шасси In mВсеШасси.ListВсеШасси
    '                            For Each mСетеваяПеременная As СетеваяПеременная In mШасси.ListСетеваяПеременная
    '                                DataAllNetVarChassis(I) = mСетеваяПеременная.ValueDouble
    '                                I += 1
    '                            Next
    '                        Next
    '                        ' получить пакет для отправки
    '                    End If
    '                    SendDataPakcetOutByte = ComposePacket(PacketArray, DataAllNetVarChassis, gManagerChassis.ChPinHashAllNetVar)

    '                    ' ''*****************************ЗАКОМЕНТИРОВАЛ***********************
    '                    ''TODO:   
    '                    'Thread.Sleep(1) '(1)
    '                    ''SyncLock OnTimedEventLock 'не спасает
    '                    'If _Tcp_Client.Connected AndAlso ClientSendData Then 'If _Client.Client.Connected AndAlso ClientSendData Then
    '                    '    If _Stream.CanWrite Then
    '                    '        '_Stream.Write(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                    '        _Stream.BeginWrite(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
    '                    '    End If

    '                    '    'TODO:                       проба()
    '                    '    'If МожноВызыватьСново Then AwaitData(Me)
    '                    '    'AwaitData(Me)
    '                    '    'Thread.Sleep(1)
    '                    'End If
    '                    ''End SyncLock
    '                    ' ''*****************************ЗАКОМЕНТИРОВАЛ***********************

    '                    ' ''*****************************ЗАМЕНИЛ НА ОТПРАВКУ ОДНОГО ОБЩЕГО БУФЕРА***********************
    '                    'SendDataPakcetOutByte = (SendDataPakcetOutByte.Concat(msgReqParList_Insys_sbk)).ToArray 'не работает
    '                    SumArr = (msgReqParList_Insys_sbk.Concat(SendDataPakcetOutByte)).ToArray

    '                    'ODD = Not ODD
    '                    If _Stream.CanWrite Then
    '                        '_Stream.BeginWrite(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '
    '                        '_Stream.BeginWrite(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '

    '                        ''If ODD Then
    '                        '_Stream.Write(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                        ''Else
    '                        '_Stream.Write(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                        ''End If
    '                        _Stream.BeginWrite(SumArr, 0, SumArr.Length, New AsyncCallback(AddressOf myWriteCallBack), Me) '_Stream) '

    '                    End If
    '                    '''*****************************ЗАМЕНИЛ***********************

    '                    If OldCountAwaitData = CountAwaitData Then 'зависание делегата асинхронного чтения 
    '                        CountWait += 1
    '                        If CountWait > constCountWait Then
    '                            'остановить и возобновить связь с сервером
    '                            _AppendMethod("Делегат асинхронного чтения из входных сетевых буферов не вернул управления.", MessageBoxIcon.Error)
    '                            _StopMethod(False)
    '                            CountWait = 0
    '                            OldCountAwaitData = -1
    '                            CountAwaitData = 0
    '                        End If
    '                    Else
    '                        CountWait = 0
    '                        OldCountAwaitData = CountAwaitData
    '                    End If
    '                End If
    '            Catch ex As SocketException
    '                _AppendMethod("Socket exception: " + ex.SocketErrorCode.ToString, MessageBoxIcon.Error)
    '            Catch ex As Exception
    '                _AppendMethod("Exception: " & Convert.ToString(ex), MessageBoxIcon.Error)
    '            End Try

    '            Dim fireAcquiredDataEventArgs As New AcquiredDataEventArgs(AcquisitionDoubleValue)
    '            RaiseEvent SyncSocketClientAcquiredData(Me, fireAcquiredDataEventArgs)
    '            'Application.DoEvents()' теряются необработанные события
    '            syncPoint = 0 ' освободить
    '        Else
    '            syncPoint = 0 ' освободить
    '            StopAcquisition()
    '        End If
    '    End If
    'End Sub

#Region "Асинхронная версия чтения буфера сокета"
    Public Sub MyWriteCallBack(result As IAsyncResult)
        'Dim myNetworkStream As NetworkStream = CType(ar.AsyncState, NetworkStream)
        'myNetworkStream.EndWrite(ar)
        Dim info As ConnectionInfoClient = CType(result.AsyncState, ConnectionInfoClient)

        info._Stream.EndWrite(result)
        'TestSend = "myWriteCallBack"
    End Sub

    ' убрал не знаю зачем проверить   если убрать Сервер создаёт по 2 подключения '<MethodImplAttribute(MethodImplOptions.Synchronized)>
    '<MethodImplAttribute(MethodImplOptions.Synchronized)>

    ''' <summary>
    ''' Начать операцию асинхронного чтения из входных сетевых буферов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AwaitData() 'info As ConnectionInfoClient)
        'info.CountAwaitData += 1
        countAwaitData += 1
        'info.МожноВызыватьСново = False

        'If info.CountAwaitData > 100 Then
        '    Stop
        '    info.CountAwaitData = 0
        '    info.Tcp_Client.Close()
        'End If
        'Public Sub AwaitData()
        ' Начинает асинхронное чтение из объекта NetworkStream. 
        'Public Overrides Function BeginRead( buffer As Byte(), offset As Integer, size As Integer, callback As AsyncCallback, state As Object ) As IAsyncResult
        ' Buffer()
        ' Тип:    System.Byte()
        ' Массив типа Byte, который является местоположением в памяти для хранения данных, прочитанных из NetworkStream. 
        '        offset()
        ' Тип:    System.Int32()
        ' Местоположение в объекте buffer, начиная с которого записываются сохраняемые данные. 
        '        Size()
        ' Тип:    System.Int32()
        ' Число байтов, читаемых из объекта NetworkStream. 
        '        callback()
        ' Тип:    System.AsyncCallback()
        ' Делегат AsyncCallback, который выполняется после выполнения метода BeginRead. 
        '        state()
        ' Тип:    System.Object()
        ' Объект, содержащий любые дополнительные данные, определенные пользователем. 
        ' Метод BeginRead начинает операцию асинхронного чтения из входных сетевых буферов. 
        ' Вызов метода BeginRead дает возможность получения данных в рамках отдельного исполняемого потока. 
        '_Stream.BeginRead(_Buffer, 0, _Buffer.Length, AddressOf DoReadDataAsync, Me)
        _Stream.BeginRead(_Buffer, 0, _Buffer.Length, AddressOf DoReadDataAsync, Me) ' info)

        ' Необходимо создать метод ответного вызова, который реализует делегат AsyncCallback, и передать его имя в метод BeginRead. 
        ' Параметр state должен содержать, по крайней мере, объект NetworkStream. 
        ' Так как может потребоваться получить поступившие данные в рамках метода ответного вызова, 
        ' необходимо создать небольшой класс или структуру для хранения буфера чтения и любых других полезных сведений.
        ' Передать структуру или экземпляр класса в метод BeginRead, используя параметр state. 
        ' Когда приложение вызывает метод BeginRead, система будет находится в ожидании до тех пор, пока не будут получены данные или произойдет ошибка, 
        ' после чего система будет использовать отдельный поток для выполнения указанного метода обратного вызова и заблокирует метод EndRead до тех пор, 
        ' пока объект NetworkStream не выполнит чтение данных или не создаст исключение. 
        ' Метод BeginRead будет выполнять чтение такого объема доступных данных, который указан (в байтах) в параметре size.
    End Sub

    Private arrTempByte As Byte()
    Private isReadRemainder As Boolean ' дочитать Остаток
    'Private NewLength As Integer

    Private Sub DoReadDataAsync(result As IAsyncResult)
        Dim info As ConnectionInfoClient = CType(result.AsyncState, ConnectionInfoClient)

        Try
            ' Если поток можно прочитать, получить текущие данные и затем начать другое асинхронное чтение
            If info._Stream IsNot Nothing AndAlso info._Stream.CanRead Then
                ' Метод обратного вызова должен вызвать метод EndRead.
                ' Завершить асинхронную операцию и прочитать буфер до конца
                info._LastReadLength = info._Stream.EndRead(result)

                ' проверить за это время не добавилось ли чего-нибудь
                If info._Tcp_Client.Available > 0 Then
                    ' ''*****************************ЗАКОМЕНТИРОВАЛ***********************
                    If info.isReadRemainder Then ' уже массив info.Temp заполнен
                        Dim TempOnce As Byte() = New Byte(info._LastReadLength - 1) {}
                        Array.Copy(info._Buffer, 0, TempOnce, 0, info._LastReadLength)

                        info.arrTempByte = (info.arrTempByte.Concat(TempOnce)).ToArray
                    Else
                        ' накопить временный буфер
                        ' пока данные в буфере info._Buffer
                        info.arrTempByte = New Byte(info._LastReadLength - 1) {}
                        Array.Copy(info._Buffer, 0, info.arrTempByte, 0, info._LastReadLength)
                    End If
                    ' ''*****************************ЗАКОМЕНТИРОВАЛ***********************

                    ' '' ''*****************************ВСТАВИЛ***********************
                    '' накопить временный буфер
                    '' пока данные в буфере info._Buffer
                    'info.Temp = New Byte(info._LastReadLength - 1) {}
                    'Array.Copy(info._Buffer, 0, Temp, 0, info._LastReadLength)
                    '' теперь данные в Temp
                    '' определим новый размер буфера для будующего считывания, предполагая, что он будет максимальный
                    'info.NewLength = info._Buffer.Length + info._Tcp_Client.Available
                    '' определим новый размер буфера для остатка
                    'ReDim_info._Buffer(info._Tcp_Client.Available - 1)
                    '' установить флаг продолжения чтения
                    'info.ДочитатьОстаток = True
                    'info.AwaitData() 'заново запустить чтение остатка
                    ' '' ''*****************************ВСТАВИЛ***********************

                    ' теперь данные в Temp

                    '********************
                    '' определим новый размер буфера для будующего считывания, предполагая, что он будет максимальный
                    'info.NewLength = info._Buffer.Length + info.Client.Available
                    '' определим новый размер буфера для остатка
                    'ReDim_info._Buffer(info.Client.Available - 1)
                    ''********************

                    ' установить флаг продолжения чтения
                    info.isReadRemainder = True
                    info.AwaitData() 'info) ' заново запустить чтение остатка
                Else
                    If info._LastReadLength > 0 Then
                        If info.isReadRemainder Then
                            ' дописать в Temp остаток
                            info.arrTempByte = (info.arrTempByte.Concat(info._Buffer)).ToArray
                            'Dim message As String = System.Text.Encoding.ASCII.GetString(info.Temp)
                            'info._AppendMethod(message) ' вывести сообщение(в потоке формы через делегат вызывается AppendOutput)

                            'If gMainForm._TaskConnectionMontiorWithServer IsNot Nothing Then
                            If info._DataQueue.Count < CAPACITY Then
                                info._DataQueue.Enqueue(info.arrTempByte)
                            End If
                            'End If
                            info.arrTempByte = New Byte() {} ' временный буфер очистить

                            ' '' ''*****************************ВСТАВИЛ***********************
                            ' ''TODO:
                            '' ''********************
                            ' '' определим новый размер буфера адаптированный под максимальный размер
                            'ReDim_info._Buffer(info.NewLength - 1)
                            '' ''********************
                            ' '' ''*****************************ВСТАВИЛ***********************
                            info.isReadRemainder = False
                        Else
                            'Dim message As String = System.Text.Encoding.ASCII.GetString(info._Buffer)
                            'info._AppendMethod(message)  'вывести сообщение(в потоке формы через делегат вызывается AppendOutput)
                            ' здесь надо сделать разбор протокола ответа от сервера

                            info.arrTempByte = New Byte(info._LastReadLength - 1) {}
                            Array.Copy(info._Buffer, 0, info.arrTempByte, 0, info._LastReadLength)

                            'If gMainForm._TaskConnectionMontiorWithServer IsNot Nothing Then
                            If info._DataQueue.Count < CAPACITY Then
                                info._DataQueue.Enqueue(info.arrTempByte)
                            End If
                            'End If
                        End If
                    End If

                    If info._Tcp_Client Is Nothing Then
                        Exit Sub ' при отключении Сервером Клиента
                    End If

                    'info.МожноВызыватьСново = True
                    'info.CountAwaitData = 0

                    'проба()
                    'info.AwaitData(info) 'заново запустить чтение
                    info.AwaitData()
                End If
                'Else
                '    Stop
            End If
        Catch ex As Exception ' если во время приёма от Сервера была ошибка, то стоп
            info._LastReadLength = -1
            info._AppendMethod(ex.ToString, MessageBoxIcon.Error)
            info._StopMethod(False)
        End Try
    End Sub
#End Region

    '#Region "Синхронная версия чтения буфера сокета"

    '    ''' <summary>
    '    ''' Общая функция для всех таймеров
    '    ''' Осуществляет посылку, приём и разборку по массивам полученных данных на синхронном блокирующем сокете
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Sub _OnTimedEvent()    '<MethodImplAttribute(MethodImplOptions.Synchronized)> по всей видимости тормозит
    '        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

    '        If sync = 0 Then
    '            If _Tcp_Client.Connected Then 'не знаю, что правильнее 'If _Client.Client.Connected Then
    '                Try
    '                    'If gMainForm.СетевыеПеременныеПроверены AndAlso _Client.Client.Connected AndAlso ClientSendData Then
    '                    If gMainForm.СетевыеПеременныеПроверены AndAlso _Tcp_Client.Connected AndAlso ClientSendData Then

    '                        'отправить на Сервер запрос на получение каналов для визуализации и идущих в формулы и в расчёт
    '                        'не знаю, что правильнее
    '                        Dim bytesSent As Integer = _Tcp_Client.Client.Send(msgReqParList_Insys_sbk) 'Это синхронная передача
    '                        Thread.Sleep(1)
    '                        DoReadDataSync()
    '                        'или сбоит
    '                        'If _Stream.CanWrite Then
    '                        '    _Stream.Write(msgReqParList_Insys_sbk, 0, msgReqParList_Insys_sbk.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                        '    DoReadData()
    '                        'End If

    '                        'подготовить массив со значениями сетевых переменных всех видов для передачи их Серверу
    '                        Dim I As Integer
    '                        For Each mШасси As Шасси In mВсеШасси.ListВсеШасси
    '                            For Each mСетеваяПеременная As СетеваяПеременная In mШасси.ListСетеваяПеременная
    '                                DataAllNetVarChassis(I) = mСетеваяПеременная.ValueDouble
    '                                I += 1
    '                            Next
    '                        Next
    '                        'получить пакет для отправки
    '                        SendDataPakcetOutByte = ComposePacket(PacketArray, DataAllNetVarChassis, gManagerChassis.ChPinHashAllNetVar)

    '                        'TODO:
    '                        'SyncLock OnTimedEventLock 'не спасает
    '                        If _Tcp_Client.Connected AndAlso ClientSendData Then
    '                            bytesSent = _Tcp_Client.Client.Send(SendDataPakcetOutByte) 'Это синхронная передача
    '                            Thread.Sleep(1)
    '                            DoReadDataSync()

    '                            'или сбоит
    '                            'If _Stream.CanWrite Then
    '                            '    _Stream.Write(SendDataPakcetOutByte, 0, SendDataPakcetOutByte.Length) 'Метод Write будет выполнять блокирование, пока не будет передано запрошенное количество байтов, или возникнет исключение SocketException
    '                            '    DoReadData()
    '                            'End If
    '                        End If
    '                        'End SyncLock
    '                    End If
    '                Catch ex As SocketException
    '                    _AppendMethod("Socket exception: " + ex.SocketErrorCode.ToString, MessageBoxIcon.Error)
    '                Catch ex As Exception
    '                    _AppendMethod("Exception: " & Convert.ToString(ex), MessageBoxIcon.Error)
    '                End Try

    '                Dim fireAcquiredDataEventArgs As New AcquiredDataEventArgs(AcquisitionDoubleValue)
    '                RaiseEvent SyncSocketClientAcquiredData(Me, fireAcquiredDataEventArgs)
    '                'Application.DoEvents()'теряются необработанные события
    '                'End If
    '                syncPoint = 0 'освободить
    '            Else
    '                syncPoint = 0 'освободить
    '                StopAcquisition()
    '            End If
    '        End If
    '    End Sub

    '    Private TempBuffer() As Byte = New Byte() {} 'для заполнения полученных данных

    '    ''' <summary>
    '    ''' Синхронная операцию чтения буфера сокета
    '    ''' В процессе чтения буфер может добавляться, поэтому чтение в цикле до его исчерпания
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Private Sub DoReadDataSync()
    '        Try
    '            TempBuffer = New Byte() {}
    '            Dim bytesRec As Integer
    '            Do While _Tcp_Client.Available > 0
    '                bytesRec = Tcp_Client.Client.Receive(_Buffer, _Buffer.Length, 0)
    '                'или
    '                'If _Stream IsNot Nothing AndAlso _Stream.CanRead Then
    '                '    Dim bytesRec As Integer = _Stream.Read(_Buffer, 0, _Buffer.Length)

    '                If bytesRec = 0 Then
    '                    Exit Do
    '                End If

    '                Temp = New Byte(bytesRec - 1) {}
    '                Array.Copy(_Buffer, 0, Temp, 0, bytesRec)
    '                TempBuffer = (TempBuffer.Concat(Temp)).ToArray 'нарастить в цикле
    '                'End If
    '            Loop

    '            If bytesRec > 0 Then
    '                If _DataQueue.Count < Capacity Then
    '                    _DataQueue.Enqueue(TempBuffer)
    '                End If
    '            End If

    '            If _Tcp_Client Is Nothing Then
    '                Exit Sub 'при отключении Сервером Клиента
    '            End If
    '        Catch ex As Exception 'если во время приёма от Сервера была ошибка, то стоп
    '            _AppendMethod(ex.ToString, MessageBoxIcon.Error)
    '            _StopMethod(False)
    '        End Try
    '    End Sub

    '#End Region

    Private Sub DecodingString()
        Dim configurations As New List(Of String) From {
            String.Empty ' нулевой элемент Nothing
            }
        For J = 1 To UBound(ParametersTCP)
            configurations.Add(ParametersTCP(J).NameParameter)
        Next

        ' заново сформировать строку - она передана ссылке
        If configurations.Count = 1 Then ' значит есть только String.Empty
            'ReDim_ParametersName(1)
            Re.Dim(ParametersName, 1)
            ParametersName(0) = String.Empty
            ParametersName(1) = ParametersTCP(1).NameParameter
        Else
            ParametersName = configurations.ToArray()
        End If
    End Sub

    ''' <summary>
    ''' Настроить В Зависимости От Стенда И Конфигурации
    ''' Вызывается при смене номера стенда из списка
    ''' </summary>
    ''' <remarks></remarks>
    Private Function TuneByStendConfiguration() As Boolean
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(TuneByStendConfiguration)}> Считывание последней конфигурации")
        Dim success As Boolean = False
        Dim channelConfigTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter = Nothing
        Dim channelsDataSet As Channels_cfg_lmzDataSet

        Try
            'Dim xdoc As XDocument = XDocument.Load(PathServerCfglmzXml)
            'HostName = xdoc.<Cell>.<Config>.<Net>.<Server>.@HostIP
            'port = xdoc.<Cell>.<Config>.<Net>.<Server>.@TcpPort

            channelConfigTableAdapter = New Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter With {
                .ClearBeforeFill = True ' взят с дизайнера
                }

            channelsDataSet = New Channels_cfg_lmzDataSet With {
                .DataSetName = "Channels_cfg_lmzDataSet",
                .SchemaSerializationMode = SchemaSerializationMode.IncludeSchema ' взят с дизайнера
                }

            ' строка подключения сделал сам т.к. в дизайнере ссылка на строку созданную при создании дизайнера
            channelConfigTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
            channelConfigTableAdapter.FillBykeyConfig(channelsDataSet.ChannelConfig, keyConfig)
            OutputChannelsBindingList.Clear()

            If UBound(ParametersIndex) <> channelsDataSet.ChannelConfig.Rows.Count Then
                Const CAPTION As String = "Загрузка выбранной конфигурации"
                Dim text As String = $"Нарушено соответствие списка каналов в базе данных выбранного стенда и списка выбранных для опроса каналов в последней рабочей конфигурации!{Environment.NewLine}Повторно произведите составление списка каналов по пункту меню:{Environment.NewLine}Настроить -> Конфигурация режимов."

                MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
                MainMdiForm.LabelНепр.Text = "Приём каналов от Сервера отсутствует"
                MainMdiForm.LabelНепр.Image = My.Resources.Disconnect

                Return False
            End If

            Dim emptyChannels As New List(Of String) ' список Отсутствующих Каналов
            For Each rowChannel As Channels_cfg_lmzDataSet.ChannelConfigRow In channelsDataSet.ChannelConfig.Rows
                If Not ParametersName.Contains(rowChannel.НаименованиеПараметра) Then
                    'MessageBox.Show("Канала с именем <" & rowChannel.НаименованиеПараметра & "> не существует в данной конфигурации!" & Environment.NewLine & "Обновите список каналов в конфигурации," & Environment.NewLine & "или измените экранное имя канала, чтобы его длина не превышала 50 символов.", "Загрузка выбранной конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    emptyChannels.Add(rowChannel.НаименованиеПараметра)
                End If

                Dim clsChannelOutput As New ChannelOutput() With {.Id = rowChannel.id,
                                                                    .KeyConfig = rowChannel.keyConfig,
                                                                    .NumberParameter = rowChannel.НомерПараметра,
                                                                    .Name = rowChannel.Name,
                                                                    .NameParameter = rowChannel.НаименованиеПараметра}

                If Not IsDBNull(rowChannel("ЕдиницаИзмерения")) Then clsChannelOutput.UnitOfMeasure = rowChannel.ЕдиницаИзмерения
                'If clsChannelOutput.Pin IsNot Nothing Then clsChannelOutput.Pin = rowChannel.Pin

                clsChannelOutput.LowerLimit = rowChannel.ДопускМинимум
                clsChannelOutput.UpperLimit = rowChannel.ДопускМаксимум
                clsChannelOutput.RangeYmin = rowChannel.РазносУмин
                clsChannelOutput.RangeYmax = rowChannel.РазносУмакс
                clsChannelOutput.AlarmValueMin = rowChannel.АварийноеЗначениеМин
                clsChannelOutput.AlarmValueMax = rowChannel.АварийноеЗначениеМакс
                clsChannelOutput.GroupCh = rowChannel.GroupCh
                clsChannelOutput.NumberFormula = 2 ' забыл внести поле при создании базы, поэтому здесь заплатка

                OutputChannelsBindingList.Add(clsChannelOutput)
                'End If
                'Else
                '    MessageBox.Show("Канала с именем <" & rowChannel.НаименованиеПараметра & "> не существует в данной конфигурации!" & Environment.NewLine & "Обновите список каналов в конфигурации," & Environment.NewLine & "или измените экранное имя канала, чтобы его длина не превышала 50 символов.", "Загрузка выбранной конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'End If
            Next

            channelConfigTableAdapter.Connection.Close()
            success = True

            If emptyChannels.Count > 0 Then
                Dim I As Integer
                Dim result As New StringBuilder

                For Each nameChannel As String In emptyChannels
                    result.AppendLine(nameChannel)
                    I += 1

                    If I > 10 Then
                        result.AppendLine("и еще более...")
                        Exit For
                    End If
                Next

                Const CAPTION As String = "Загрузка выбранной конфигурации"
                Dim text As String = String.Format("Следующие каналы:{0}{1}не существуют в данной конфигурации или были переименованы!{0}1) Обновите список каналов в конфигурации, или{0}2) измените экранное имя канала, чтобы его длина не превышала 50 символов.", Environment.NewLine, result.ToString)
                MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
            End If
        Catch ex As Exception
            Const CAPTION As String = "Настройка каналов сокета в зависимости от стенда и конфигурации"
            Dim text As String = ex.ToString
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", CAPTION, text))
        Finally
            If channelConfigTableAdapter.Connection.State = ConnectionState.Open Then
                channelConfigTableAdapter.Connection.Close()
            End If
        End Try

        ' Успех = СоздатьСокетсСервером()
        'Else
        ''StartTSButton.Enabled = False
        ''GroupBoxStend.Enabled = True
        ''GroupBoxСтендКонфигСервер.Enabled = True
        'End If

        Return success
    End Function

    ''' <summary>
    ''' Считать Настройки Тср Клиента
    ''' Считывание настроек из файла Options.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function LoadOptionsTcpClient() As Boolean
        Dim success As Boolean = False
        'Dim cReadWriteIni As New ReadWriteIni(gPathXmlОпции)
        'Dim xmlDoc As New XmlDocument

        'xmlDoc.Load(gPathXmlОпции)

        'Dim StendNumbers As New List(Of String)
        'Dim PathServerCfg_xml() As String 'массив путей к конфигурационным файлам серверов
        'Dim numberStend As String
        'StendNumbers.AddRange(StendServerTypeConverter.ToString.Split(","))
        'ReDim_PathServerCfg_xml(StendNumbers.Count - 1)
        '' считать пути ServerCfg_xml
        'For I = 0 To StendNumbers.Count - 1
        '    PathServerCfg_xml(I) = cReadWriteIni.GetIni(xmlDoc, stsРаздел, strСекция, "Путь_к_конфигурационному_файлу_сервера" & I + 1.ToString, "Неправильный путь")
        'Next 

        'numberStend = "1" 'по умолчанию
        Try
            '' узнать путь конфигурации XML Сервера для выбранного стенда (при настройки TCP конфигуратор мог переписать)
            'numberStend = myOptionData.StendServer
            'For I = 0 To StendNumbers.Count - 1
            '    If StendNumbers(I) = numberStend Then
            '        PathServerCfglmzXml = PathServerCfg_xml(I) ' обновляется путь (при настройки TCP конфигуратор мог переписать)
            '        Exit For
            '    End If
            'Next 
            ' индекс последней конфигурации каналов Сервера
            'keyConfig = Convert.ToInt32(cReadWriteIni.GetIni(xmlDoc, "Камера", "Дополнительно", "LastTCPkeyConfig", "0"))
            keyConfig = Convert.ToInt32(GetIni(PathOptions, "Options", "LastTCPkeyConfig", "0"))

            If keyConfig = 0 Then
                Const CAPTION As String = "Некорректный индекс конфигурации"
                Dim text As String = $"Конфигурация каналов Сервера не создана.{Environment.NewLine}Необходимо произвести настройку каналов в окне <Конфигуратор каналов Сервера>."
                MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
                success = False
            Else
                'If numberStend <> myOptionData.StendServer Then
                '    Const caption As String = "Изменён номер стенда"
                '    'Dim text As String = "Программа первоначально была загружена под номером стенда " & strНомерСтенда & Environment.NewLine &
                '    Dim text As String = "Программа первоначально была загружена под номером стенда " & myOptionData.StendServer & Environment.NewLine &
                '                    "В конфигураторе каналов был выбран стенд " & numberStend & Environment.NewLine &
                '                    "Необходимо перезапустить программу!"
                '    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                '    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
                '    System.Environment.Exit(0)
                '    success = False
                'Else
                '    success = True
                'End If
                success = True
            End If
        Catch ex As Exception
            Const CAPTION As String = "Ошибка при считывании настроек из файла <Options.xml>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", CAPTION, text))
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Загрузка каналов из таблицы ChannelN в структуру arrПараметры, которая требует меньше ресурсов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadChannelsTCPclient()
        Dim count As Integer
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim aFindValue(0) As Object
        Dim dcDataColumn(1) As DataColumn
        Dim success As Boolean = False

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            success = CheckExistTable(cn, CHANNEL_N)
        End Using

        If Not success Then
            Const CAPTION As String = "Запуск приложения"
            Const text As String = "Отсутствует в базе данных таблица: <" & CHANNEL_N & ">. Работа невозможна!"
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
            Environment.Exit(0) 'End
        End If

        Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter("SELECT * FROM " & CHANNEL_N & " Order By НомерПараметра", BuildCnnStr(ProviderJet, PathChannels))

        odaDataAdapter.Fill(dtDataTable)
        count = dtDataTable.Rows.Count

        If count = 0 Then
            Const CAPTION As String = "Запуск приложения"
            Const text As String = "В базе каналов " & CHANNEL_N & " нет записей!"
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
            Environment.Exit(0) 'End
        End If

        ' на всякий случай заново переименуем номера, сначало заведомо нереальными, затем настоящими
        Dim number As Integer = 10000
        For Each drDataRow In dtDataTable.Rows
            drDataRow.Item("НомерПараметра") = number
            number += 1
        Next

        number = 1
        For Each drDataRow In dtDataTable.Rows
            drDataRow.Item("НомерПараметра") = number
            number += 1
        Next

        Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.UpdateCommand = myDataRowsCommandBuilder.GetUpdateCommand
        odaDataAdapter.Update(dtDataTable)
        dtDataTable.AcceptChanges()

        dcDataColumn(0) = dtDataTable.Columns("НомерПараметра")
        dtDataTable.PrimaryKey = dcDataColumn
        'ReDim_ParametersTCP(count)
        Re.Dim(ParametersTCP, count)

        For number = 1 To count
            aFindValue(0) = number
            drDataRow = dtDataTable.Rows.Find(aFindValue)

            If drDataRow IsNot Nothing Then
                With drDataRow
                    ParametersTCP(number).NumberParameter = CInt(.Item("НомерПараметра"))
                    ParametersTCP(number).NameParameter = CStr(.Item("НаименованиеПараметра"))
                    ParametersTCP(number).UnitOfMeasure = CStr(.Item("ЕдиницаИзмерения"))
                    ParametersTCP(number).LowerLimit = CSng(.Item("ДопускМинимум"))
                    ParametersTCP(number).UpperLimit = CSng(.Item("ДопускМаксимум"))
                    ParametersTCP(number).RangeYmin = CInt(.Item("РазносУмин"))
                    ParametersTCP(number).RangeYmax = CInt(.Item("РазносУмакс"))
                    ParametersTCP(number).AlarmValueMin = CSng(.Item("АварийноеЗначениеМин"))
                    ParametersTCP(number).AlarmValueMax = CSng(.Item("АварийноеЗначениеМакс"))
                    ParametersTCP(number).IsVisibleRegistration = CBool(.Item("ВидимостьРегистратор"))

                    If Not IsDBNull(.Item("Примечания")) Then
                        ParametersTCP(number).Description = CStr(.Item("Примечания"))
                    Else
                        ParametersTCP(number).Description = CStr(0)
                    End If

                    ParametersTCP(number).LevelWarning = LevelWarningWhat.Normal
                End With
            End If
        Next
    End Sub
End Class