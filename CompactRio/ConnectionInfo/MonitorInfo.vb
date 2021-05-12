Imports System.Collections.Generic
Imports System.Net.Sockets

''' <summary>
''' Представляет простую объектную оболочку для использования состояния объекта 
''' годного для мониторинга соединения в потоке.
''' </summary>
''' <remarks></remarks>
Public Class MonitorInfo
    Public Property Cancel As Boolean

    Private ReadOnly mConnections As Dictionary(Of String, ConnectionInfoServer)
    Public ReadOnly Property Connections As Dictionary(Of String, ConnectionInfoServer)
        Get
            Return mConnections
        End Get
    End Property

    Private ReadOnly mListener As TcpListener
    Public ReadOnly Property Listener As TcpListener
        Get
            Return mListener
        End Get
    End Property

    Public Sub New(tcpListener As TcpListener, connectionInfoList As Dictionary(Of String, ConnectionInfoServer))
        mListener = tcpListener
        mConnections = connectionInfoList
    End Sub
End Class