Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports MathematicalLibrary
Imports Registration.FormCompactRio

''' <summary>
''' Предоставляет оболочку управления состоянием для асинхронного клиента и потокового управления.
''' </summary>
''' <remarks></remarks>
Public Class ConnectionInfoServer

#Region "Property"
    Private ReadOnly mListener As TcpListener
    ''' <summary>
    ''' Прослушивает подключения от TCP-клиентов сети.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Listener As TcpListener
        Get
            Return mListener
        End Get
    End Property

    Private mClient As TcpClient
    Public ReadOnly Property Client As TcpClient
        Get
            Return mClient
        End Get
    End Property

    Private mStream As NetworkStream
    ''' <summary>
    ''' Возвращает объект System.Net.Sockets.NetworkStream,
    ''' используемый для отправки и получения данных.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Stream As NetworkStream
        Get
            Return mStream
        End Get
    End Property

    Private iRemote, pLocal As EndPoint
    ''' <summary>
    ''' Удалённая конечная точка
    ''' </summary>
    Public ReadOnly Property Remote() As EndPoint
        Get
            Return iRemote
        End Get
    End Property

    ''' <summary>
    ''' Локальный конечная точка
    ''' </summary>
    Public ReadOnly Property Local() As EndPoint
        Get
            Return pLocal
        End Get
    End Property

    ' попытка №1
    'Private _DataQueue As Queue(Of Byte)
    'Public ReadOnly Property DataQueue As Queue(Of Byte)
    ''    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    '    Get
    '        Return _DataQueue
    '    End Get
    'End Property

    ' попытка №2
    'Private _DataMemoryStream As MemoryStream
    'Public ReadOnly Property DataMemoryStream As MemoryStream
    '    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    '    Get
    '        Return _DataMemoryStream
    '    End Get
    'End Property

    'Private _DataMemoryStream As Byte()

    'Public Property DataMemoryStream As Byte()
    '    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    '    Get
    '        Return _DataMemoryStream
    '    End Get
    '    Set(value As Byte())
    '        _DataMemoryStream = value
    '    End Set
    'End Property

    ' попытка №3
    Private ReadOnly mDataQueue As Queue(Of Byte())
    ''' <summary>
    ''' Очередь сообщений полученных от клиента.
    ''' Заблокирован для выполнения только одним потоком.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DataQueue As Queue(Of Byte())
        <MethodImplAttribute(MethodImplOptions.Synchronized)>
        Get
            Return mDataQueue
        End Get
    End Property

    Private mLastReadLength As Integer
    ''' <summary>
    ''' Длина принятого буфера
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastReadLength As Integer
        Get
            Return mLastReadLength
        End Get
    End Property

    Private mRemoteEndPointIPAddress As String
    ''' <summary>
    ''' IP-адрес подключаемого Клиента
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RemoteEndPointIPAddress() As String
        Get
            Return mRemoteEndPointIPAddress
        End Get
        Set(ByVal value As String)
            mRemoteEndPointIPAddress = value
            ' здесь в менеджере шасси произвести поиск шасси с IPAddress 
            Dim itemChassis As Chassis = mCompactRioForm.ManagerChassis.FindChassisInManager(mRemoteEndPointIPAddress)

            If itemChassis IsNot Nothing Then
                ' установить _NameChassis, _ArrayLength, ModeWork
                NameChassis = itemChassis.HostName ' поле связывающее 2 коллекции: менеджера шасси и коллекцию подключённых сокетов
                ' если шасси отключится, а затем сново подключится, то порядок следования его элементов в массиве собранных данных и хеш пинов не будет нарушен
                mArrayLength = itemChassis.ChannelsCount
                ModeWork = itemChassis.ModeWork
                ' установить размернось массива буфера значений каналов
                Re.Dim(AcquisitionData, mArrayLength - 1)
                ListChannelsToAcquire = itemChassis.ListChannelsToAcquire
                itemChassis.IsConnected = True
                IndexRow = itemChassis.IndexRow
                ' зарегистрировать свой Key в itemChassis
                'Else
                ' надо отключиться
            End If

            ' для теста
            'NameChassis = Me.iRemote.ToString
            'mArrayLength = ChannelsNumder 'для теста
            ' установить режим из шасси
            'ModeWork = EnumModeWork.Measurement
        End Set
    End Property

    Private mArrayLength As Integer
    ''' <summary>
    ''' Размерность массива значений каналов собранных клиентом (шасси).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ArrayLength As Integer
        Get
            Return mArrayLength
        End Get
    End Property

    ''' <summary>
    ''' Буфер значений каналов собранных клиентом (шасси).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AcquisitionData() As Double()

    ''' <summary>
    ''' Список каналов подлежащих сбору
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListChannelsToAcquire() As String()

    ' TODO: Закоментировал, так как infoConnWithServer.AcquisitionQueue не используется для передачи внешнему Серверу
    'Private ReadOnly mAcquisitionQueue As Queue(Of Double())
    '''' <summary>
    '''' Очередь сообщений полученных от Сервера.
    '''' Заблокирован для выполнения только одним потоком.
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public ReadOnly Property AcquisitionQueue As Queue(Of Double())
    '    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    '    Get
    '        Return mAcquisitionQueue
    '    End Get
    'End Property

    ''' <summary>
    ''' Режим измерения или ещё и управление
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property ModeWork As EnumModeWork = EnumModeWork.Measurement

    ''' <summary>
    ''' всего получено пакетов
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PacketsReceive As Long
    ''' <summary>
    ''' ' Счётчик полученных пакетов, для определения необходимости обновить сообщение
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PacketsThresholdCount As Integer

    ''' <summary>
    ''' Вывести предупреждение не более 10 раз
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountWarning As Integer

    ''' <summary>
    ''' Индекс для связи со строкой таблицы
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IndexRow As Integer

    ''' <summary>
    ''' Имя корреспондируемого с сокетом шасси связывает ключи из двух коллекций:
    ''' 1. менеджера шасси и 2. коллекцию подключённых сокетов (клиентов)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NameChassis() As String

    ' Размер буфера имеет случайное значение которое должно быть определяться на основе
    ' количества данных что нужно передать, скорости передачи и ожидаемое число клиентов.
    ' Предположения по дизайну коммуникационного протокола для передачи данных и размер для чтения буфера
    ' должны быть основаны на разрабатываемом протоколе.
    ''' <summary>
    ''' Временный буфер для хранения разорванного пакета
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LostBytes As Byte()
#End Region

    Private Const receiveBufferSize As Integer = 63
    Private byteBuffer(receiveBufferSize) As Byte ' Буфер сделать переменным объёмом
    Private ReadOnly mCompactRioForm As FormCompactRio

    Public Sub New(ssdListener As TcpListener, inCompactRioForm As FormCompactRio)
        mListener = ssdListener
        mCompactRioForm = inCompactRioForm

        mDataQueue = New Queue(Of Byte())
        'mAcquisitionQueue = New Queue(Of Double())(1024) ' TODO: Закоментировал, так как infoConnWithServer.AcquisitionQueue не используется для передачи внешнему Серверу
        LostBytes = New Byte() {}
    End Sub

    ''' <summary>
    ''' Завершить асинхронную операцию принятия запроса на подключения.
    ''' </summary>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Public Sub AcceptClient(result As IAsyncResult)
        ' Асинхронная операция BeginAcceptTcpClient должна быть завершена путем вызова метода EndAcceptTcpClient. 
        ' Обычно данный метод вызывается делегатом callback.
        ' ssdListener асинхронно принимает входящие попытки подключения и создает новый объект System.Net.Sockets.TcpClient для связи с удаленным шасси.
        mClient = mListener.EndAcceptTcpClient(result)

        If mClient IsNot Nothing AndAlso mClient.Connected Then
            mClient.NoDelay = True ' true, если задержка отключена
            mStream = mClient.GetStream

            iRemote = mClient.Client.RemoteEndPoint
            pLocal = mClient.Client.LocalEndPoint
        End If
    End Sub

    ''' <summary>
    ''' Начать операцию асинхронного чтения из входных сетевых буферов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AwaitData()
        ' Начинает асинхронное чтение из объекта NetworkStream. 
        mStream.BeginRead(byteBuffer, 0, byteBuffer.Length, AddressOf DoReadData, Me)
    End Sub

    Private tempBytes As Byte()
    Private isReadingFragment As Boolean ' дочитать Остаток
    Private newLength As Integer

    Private Sub DoReadData(result As IAsyncResult)
        Dim info As ConnectionInfoServer = CType(result.AsyncState, ConnectionInfoServer)

        With info
            Try
                ' Если поток можно прочитать, получить текущие данные и затем начать другое асинхронное чтение
                If .Stream IsNot Nothing AndAlso .Stream.CanRead Then
                    ' Метод обратного вызова должен вызвать метод EndRead. 
                    .mLastReadLength = .Stream.EndRead(result)

                    If .Client.Available > 0 Then
                        ' накопить временный буфер
                        ' пока данные в буфере ._Buffer
                        .tempBytes = New Byte(.mLastReadLength - 1) {}
                        Array.Copy(.byteBuffer, 0, .tempBytes, 0, .mLastReadLength)
                        ' теперь данные в Temp
                        ' определим новый размер буфера для будующего считывания, предполагая, что он будет максимальный
                        .newLength = .byteBuffer.Length + .Client.Available
                        ' определим новый размер буфера для остатка
                        Re.Dim(.byteBuffer, .Client.Available - 1)
                        ' установить флаг продолжения чтения
                        .isReadingFragment = True
                        .AwaitData() ' заново запустить чтение остатка
                    Else
                        If .isReadingFragment Then
                            ' дописать в Temp остаток
                            .tempBytes = (.tempBytes.Concat(.byteBuffer)).ToArray
                            .mDataQueue.Enqueue(.tempBytes)

                            .tempBytes = New Byte() {} ' временный буфер очистить
                            ' определим новый размер буфера адаптированный под максимальный размер
                            Re.Dim(.byteBuffer, .newLength - 1)
                            .isReadingFragment = False
                        Else
                            .tempBytes = New Byte(.mLastReadLength - 1) {}
                            Array.Copy(.byteBuffer, 0, .tempBytes, 0, .mLastReadLength)
                            .mDataQueue.Enqueue(.tempBytes)
                        End If

                        ' втавил только отсылку ответа при отключениии клиента (признак  ._LastReadLength = 0), иначе он остаётся подключённым
                        If .mLastReadLength = 0 Then ' обработать отключение
                            ' так на шасси послать нельзя .SendMessage("Disconnected: " & ._LastReadLength & " Bytes")
                            If .Client.Connected Then
                                ' Послать команду Стоп из потока сокета
                                Dim YesNo As Short = CShort(vbTrue) ' TriState.True
                                Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' тело сообщения
                                SendCommandToClient(info, CommandSet.Stop_3, messageData)
                            End If
                        End If

                        .AwaitData() ' заново запустить чтение
                    End If
                Else
                    ' Если невозможно чтение из потока, здесь принимается решение об ошибке и закрывается клиентское соединение
                    ' Необходимо изменить поведение при реализации собственного протокола
                    mCompactRioForm.ManagerChassis.ChangeChassisConnection(.NameChassis, False) '.Client.Connected
                    .Client.Close()
                End If
            Catch ex As Exception
                .mLastReadLength = -1 ' клиент отключился
            End Try
        End With
    End Sub

    ''' <summary>
    ''' Копия подпрограммы из основного окна.
    ''' Послать Клиенту Команду
    ''' </summary>
    ''' <param name="info"></param>
    ''' <param name="cmdSet"></param>
    ''' <param name="BodyMessageData"></param>
    ''' <remarks></remarks>
    Private Sub SendCommandToClient(info As ConnectionInfoServer, cmdSet As CommandSet, bodyMessageData() As Byte)
        Dim listPacketOut As New List(Of Byte)
        Dim pakcetOutByte As Byte() = BitConverter.GetBytes(cmdSet)         ' номер команды

        pakcetOutByte = (pakcetOutByte.Concat(bodyMessageData)).ToArray()   ' соединить тело сообщения
        listPacketOut.AddRange(BitConverter.GetBytes(pakcetOutByte.Length)) ' пакет начинается с длины команды
        listPacketOut.AddRange(pakcetOutByte)                               ' длина команды + номер команды + тело сообщения
        pakcetOutByte = listPacketOut.ToArray
        info.Stream.Write(pakcetOutByte, 0, pakcetOutByte.Length)           ' отправить клиенту
    End Sub
End Class