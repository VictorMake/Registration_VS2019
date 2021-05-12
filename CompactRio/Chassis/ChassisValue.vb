''' <summary>
''' Класс для отображения состояния шасси как списка в таблице
''' </summary>
''' <remarks></remarks>
Public Class ChassisValue
    Public Sub New(inNameChassis As String,
                    inStatusAdapter As String,
                    inStatusAdapterImage As Image,
                    inStatusSendImage As Image,
                    inStatusReceiveImage As Image,
                    inPacketsReceive As String)

        NameChassis = inNameChassis
        StatusAdapter = inStatusAdapter
        StatusAdapterImage = inStatusAdapterImage
        StatusSendImage = inStatusSendImage
        StatusReceiveImage = inStatusReceiveImage
        PacketsReceive = inPacketsReceive
    End Sub

    Public Property NameChassis As String
    Public Property StatusAdapter As String
    Public Property StatusAdapterImage As Image
    Public Property StatusSendImage As Image
    Public Property StatusReceiveImage As Image
    Public Property PacketsReceive As String
End Class