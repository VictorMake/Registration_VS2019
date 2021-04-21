''' <summary>
''' Хранит IP адрес
''' </summary>
<Serializable()>
Public Class IPAddressCls
    ''' <summary>
    ''' Конструктор
    ''' </summary>
    Public Sub New(ip As String)
        ip_ = ip
    End Sub

    ''' <summary>
    ''' mIp
    ''' </summary>
    Private ip_ As String

    ''' <summary>
    ''' Строка с IP адресом
    ''' </summary>
    Public Property IP() As String
        Get
            Return ip_
        End Get
        Set(ByVal value As String)
            ip_ = value
        End Set
    End Property

    ''' <summary>
    ''' Представление в виде строки
    ''' </summary>
    Public Overloads Overrides Function ToString() As String
        Return ip_
    End Function
End Class