Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports MathematicalLibrary
Imports Registration.FormCompactRio

''' <summary>
''' ������������� �������� ���������� ���������� ��� ������������ ������� � ���������� ����������.
''' </summary>
''' <remarks></remarks>
Public Class ConnectionInfoServer

#Region "Property"
    Private ReadOnly mListener As TcpListener
    ''' <summary>
    ''' ������������ ����������� �� TCP-�������� ����.
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
    ''' ���������� ������ System.Net.Sockets.NetworkStream,
    ''' ������������ ��� �������� � ��������� ������.
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
    ''' �������� �������� �����
    ''' </summary>
    Public ReadOnly Property Remote() As EndPoint
        Get
            Return iRemote
        End Get
    End Property

    ''' <summary>
    ''' ��������� �������� �����
    ''' </summary>
    Public ReadOnly Property Local() As EndPoint
        Get
            Return pLocal
        End Get
    End Property

    ' ������� �1
    'Private _DataQueue As Queue(Of Byte)
    'Public ReadOnly Property DataQueue As Queue(Of Byte)
    ''    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    '    Get
    '        Return _DataQueue
    '    End Get
    'End Property

    ' ������� �2
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

    ' ������� �3
    Private ReadOnly mDataQueue As Queue(Of Byte())
    ''' <summary>
    ''' ������� ��������� ���������� �� �������.
    ''' ������������ ��� ���������� ������ ����� �������.
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
    ''' ����� ��������� ������
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
    ''' IP-����� ������������� �������
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
            ' ����� � ��������� ����� ���������� ����� ����� � IPAddress 
            Dim itemChassis As Chassis = mCompactRioForm.ManagerChassis.FindChassisInManager(mRemoteEndPointIPAddress)

            If itemChassis IsNot Nothing Then
                ' ���������� _NameChassis, _ArrayLength, ModeWork
                NameChassis = itemChassis.HostName ' ���� ����������� 2 ���������: ��������� ����� � ��������� ������������ �������
                ' ���� ����� ����������, � ����� ����� �����������, �� ������� ���������� ��� ��������� � ������� ��������� ������ � ��� ����� �� ����� �������
                mArrayLength = itemChassis.ChannelsCount
                ModeWork = itemChassis.ModeWork
                ' ���������� ���������� ������� ������ �������� �������
                Re.Dim(AcquisitionData, mArrayLength - 1)
                ListChannelsToAcquire = itemChassis.ListChannelsToAcquire
                itemChassis.IsConnected = True
                IndexRow = itemChassis.IndexRow
                ' ���������������� ���� Key � itemChassis
                'Else
                ' ���� �����������
            End If

            ' ��� �����
            'NameChassis = Me.iRemote.ToString
            'mArrayLength = ChannelsNumder '��� �����
            ' ���������� ����� �� �����
            'ModeWork = EnumModeWork.Measurement
        End Set
    End Property

    Private mArrayLength As Integer
    ''' <summary>
    ''' ����������� ������� �������� ������� ��������� �������� (�����).
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
    ''' ����� �������� ������� ��������� �������� (�����).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AcquisitionData() As Double()

    ''' <summary>
    ''' ������ ������� ���������� �����
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListChannelsToAcquire() As String()

    ' TODO: ��������������, ��� ��� infoConnWithServer.AcquisitionQueue �� ������������ ��� �������� �������� �������
    'Private ReadOnly mAcquisitionQueue As Queue(Of Double())
    '''' <summary>
    '''' ������� ��������� ���������� �� �������.
    '''' ������������ ��� ���������� ������ ����� �������.
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
    ''' ����� ��������� ��� ��� � ����������
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property ModeWork As EnumModeWork = EnumModeWork.Measurement

    ''' <summary>
    ''' ����� �������� �������
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PacketsReceive As Long
    ''' <summary>
    ''' ' ������� ���������� �������, ��� ����������� ������������� �������� ���������
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PacketsThresholdCount As Integer

    ''' <summary>
    ''' ������� �������������� �� ����� 10 ���
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountWarning As Integer

    ''' <summary>
    ''' ������ ��� ����� �� ������� �������
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IndexRow As Integer

    ''' <summary>
    ''' ��� ������������������ � ������� ����� ��������� ����� �� ���� ���������:
    ''' 1. ��������� ����� � 2. ��������� ������������ ������� (��������)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NameChassis() As String

    ' ������ ������ ����� ��������� �������� ������� ������ ���� ������������ �� ������
    ' ���������� ������ ��� ����� ��������, �������� �������� � ��������� ����� ��������.
    ' ������������� �� ������� ����������������� ��������� ��� �������� ������ � ������ ��� ������ ������
    ' ������ ���� �������� �� ��������������� ���������.
    ''' <summary>
    ''' ��������� ����� ��� �������� ������������ ������
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LostBytes As Byte()
#End Region

    Private Const receiveBufferSize As Integer = 63
    Private byteBuffer(receiveBufferSize) As Byte ' ����� ������� ���������� �������
    Private ReadOnly mCompactRioForm As FormCompactRio

    Public Sub New(ssdListener As TcpListener, inCompactRioForm As FormCompactRio)
        mListener = ssdListener
        mCompactRioForm = inCompactRioForm

        mDataQueue = New Queue(Of Byte())
        'mAcquisitionQueue = New Queue(Of Double())(1024) ' TODO: ��������������, ��� ��� infoConnWithServer.AcquisitionQueue �� ������������ ��� �������� �������� �������
        LostBytes = New Byte() {}
    End Sub

    ''' <summary>
    ''' ��������� ����������� �������� �������� ������� �� �����������.
    ''' </summary>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Public Sub AcceptClient(result As IAsyncResult)
        ' ����������� �������� BeginAcceptTcpClient ������ ���� ��������� ����� ������ ������ EndAcceptTcpClient. 
        ' ������ ������ ����� ���������� ��������� callback.
        ' ssdListener ���������� ��������� �������� ������� ����������� � ������� ����� ������ System.Net.Sockets.TcpClient ��� ����� � ��������� �����.
        mClient = mListener.EndAcceptTcpClient(result)

        If mClient IsNot Nothing AndAlso mClient.Connected Then
            mClient.NoDelay = True ' true, ���� �������� ���������
            mStream = mClient.GetStream

            iRemote = mClient.Client.RemoteEndPoint
            pLocal = mClient.Client.LocalEndPoint
        End If
    End Sub

    ''' <summary>
    ''' ������ �������� ������������ ������ �� ������� ������� �������
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AwaitData()
        ' �������� ����������� ������ �� ������� NetworkStream. 
        mStream.BeginRead(byteBuffer, 0, byteBuffer.Length, AddressOf DoReadData, Me)
    End Sub

    Private tempBytes As Byte()
    Private isReadingFragment As Boolean ' �������� �������
    Private newLength As Integer

    Private Sub DoReadData(result As IAsyncResult)
        Dim info As ConnectionInfoServer = CType(result.AsyncState, ConnectionInfoServer)

        With info
            Try
                ' ���� ����� ����� ���������, �������� ������� ������ � ����� ������ ������ ����������� ������
                If .Stream IsNot Nothing AndAlso .Stream.CanRead Then
                    ' ����� ��������� ������ ������ ������� ����� EndRead. 
                    .mLastReadLength = .Stream.EndRead(result)

                    If .Client.Available > 0 Then
                        ' �������� ��������� �����
                        ' ���� ������ � ������ ._Buffer
                        .tempBytes = New Byte(.mLastReadLength - 1) {}
                        Array.Copy(.byteBuffer, 0, .tempBytes, 0, .mLastReadLength)
                        ' ������ ������ � Temp
                        ' ��������� ����� ������ ������ ��� ��������� ����������, �����������, ��� �� ����� ������������
                        .newLength = .byteBuffer.Length + .Client.Available
                        ' ��������� ����� ������ ������ ��� �������
                        Re.Dim(.byteBuffer, .Client.Available - 1)
                        ' ���������� ���� ����������� ������
                        .isReadingFragment = True
                        .AwaitData() ' ������ ��������� ������ �������
                    Else
                        If .isReadingFragment Then
                            ' �������� � Temp �������
                            .tempBytes = (.tempBytes.Concat(.byteBuffer)).ToArray
                            .mDataQueue.Enqueue(.tempBytes)

                            .tempBytes = New Byte() {} ' ��������� ����� ��������
                            ' ��������� ����� ������ ������ �������������� ��� ������������ ������
                            Re.Dim(.byteBuffer, .newLength - 1)
                            .isReadingFragment = False
                        Else
                            .tempBytes = New Byte(.mLastReadLength - 1) {}
                            Array.Copy(.byteBuffer, 0, .tempBytes, 0, .mLastReadLength)
                            .mDataQueue.Enqueue(.tempBytes)
                        End If

                        ' ������ ������ ������� ������ ��� ����������� ������� (�������  ._LastReadLength = 0), ����� �� ������� ������������
                        If .mLastReadLength = 0 Then ' ���������� ����������
                            ' ��� �� ����� ������� ������ .SendMessage("Disconnected: " & ._LastReadLength & " Bytes")
                            If .Client.Connected Then
                                ' ������� ������� ���� �� ������ ������
                                Dim YesNo As Short = CShort(vbTrue) ' TriState.True
                                Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' ���� ���������
                                SendCommandToClient(info, CommandSet.Stop_3, messageData)
                            End If
                        End If

                        .AwaitData() ' ������ ��������� ������
                    End If
                Else
                    ' ���� ���������� ������ �� ������, ����� ����������� ������� �� ������ � ����������� ���������� ����������
                    ' ���������� �������� ��������� ��� ���������� ������������ ���������
                    mCompactRioForm.ManagerChassis.ChangeChassisConnection(.NameChassis, False) '.Client.Connected
                    .Client.Close()
                End If
            Catch ex As Exception
                .mLastReadLength = -1 ' ������ ����������
            End Try
        End With
    End Sub

    ''' <summary>
    ''' ����� ������������ �� ��������� ����.
    ''' ������� ������� �������
    ''' </summary>
    ''' <param name="info"></param>
    ''' <param name="cmdSet"></param>
    ''' <param name="BodyMessageData"></param>
    ''' <remarks></remarks>
    Private Sub SendCommandToClient(info As ConnectionInfoServer, cmdSet As CommandSet, bodyMessageData() As Byte)
        Dim listPacketOut As New List(Of Byte)
        Dim pakcetOutByte As Byte() = BitConverter.GetBytes(cmdSet)         ' ����� �������

        pakcetOutByte = (pakcetOutByte.Concat(bodyMessageData)).ToArray()   ' ��������� ���� ���������
        listPacketOut.AddRange(BitConverter.GetBytes(pakcetOutByte.Length)) ' ����� ���������� � ����� �������
        listPacketOut.AddRange(pakcetOutByte)                               ' ����� ������� + ����� ������� + ���� ���������
        pakcetOutByte = listPacketOut.ToArray
        info.Stream.Write(pakcetOutByte, 0, pakcetOutByte.Length)           ' ��������� �������
    End Sub
End Class