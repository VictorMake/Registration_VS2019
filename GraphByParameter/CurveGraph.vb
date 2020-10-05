Imports System.Collections.Generic

Friend Class CurveGraph
    Implements IEnumerable

    Private PointsCurve As Dictionary(Of Integer, PointCurve)

    Public Sub New()
        PointsCurve = New Dictionary(Of Integer, PointCurve)
    End Sub

    Public Sub New(ByVal inNameCurveGraph As String)
        Me.NameCurveGraph = inNameCurveGraph
        PointsCurve = New Dictionary(Of Integer, PointCurve)
    End Sub

    Public ReadOnly Property GetPointsCurve() As Dictionary(Of Integer, PointCurve)
        Get
            Return PointsCurve
        End Get
    End Property

    Public ReadOnly Property Item(ByVal index As Integer) As PointCurve
        Get
            Item = PointsCurve.Item(index)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Count = PointsCurve.Count()
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        GetEnumerator = PointsCurve.GetEnumerator
    End Function

    Public Sub Remove(ByRef index As Integer)
        ' удаление по индексу объекта
        PointsCurve.Remove(index)
    End Sub

    Public Sub Clear()
        PointsCurve.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        PointsCurve = Nothing
        MyBase.Finalize()
    End Sub

    Public Property NameCurveGraph() As String

    Public Overrides Function ToString() As String
        Return NameCurveGraph
    End Function

    Public Property Color() As String
    Public Property LineStyle() As String

    Public Sub Add(ByVal index As Integer, ByVal X As Double, ByVal Y As Double)
        If Not CheckPoint(index) Then Exit Sub

        PointsCurve.Add(index, New PointCurve(index, X, Y))
    End Sub

    Private Function CheckPoint(ByVal index As Integer) As Boolean
        If PointsCurve.ContainsKey(index) Then
            Const caption As String = "Ошибка добавления точки"
            Dim text As String = $"Точка с индексом {index} уже существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        If index < 0 Then
            Const caption As String = "Ошибка добавления точки"
            Const text As String = "Индекс точки быть в больше 0!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        Return True
    End Function

    Public Class PointCurve
        Public Sub New(ByVal inIndex As Integer, ByVal X As Double, ByVal Y As Double)
            Me.Index = inIndex
            Me.X = X
            Me.Y = Y
        End Sub

        Public Property Index() As Integer
        Public Property X() As Double
        Public Property Y() As Double
    End Class
End Class
