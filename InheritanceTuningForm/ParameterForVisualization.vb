Imports System.Runtime.Serialization

''' <summary>
'''  Упрощённый класс выборочных параметров для контроля при сериализации и дессириализации
''' </summary>
<DataContract>
Public Class ParameterForVisualization
    <DataMember>
    Public Property Name As String
    <DataMember>
    Public Property Position As Integer

    Public Sub New(ByVal inName As String, ByVal inPosition As Integer)
        Name = inName
        Position = inPosition
    End Sub
End Class
