Imports System.Threading

''' <summary>
''' Приостановка (асинхронная задержка).
''' Возможна модернизация с добавлением события во время какой либо работы и по окончанию работы.
''' </summary>
Public Class TaskRecordStartStop
    Private _cancelled As Boolean = False

    Public Function Work(ByVal token As CancellationToken, millisecondsTimeout As Integer) As Boolean
        'For i As Integer = 0 To 99
        '    token.ThrowIfCancellationRequested()
        '    Thread.Sleep(50)
        '    If i = 30 Then Throw New Exception("Что-то пошло не так...")
        '    OnProcessChanged(i)
        'Next

        Thread.Sleep(millisecondsTimeout)

        Return _cancelled
    End Function

    Public Sub OnProcessChanged(ByVal i As Object)
        'ProcessChanged?.Invoke(CInt(i))
        RaiseEvent ProcessChanged(CInt(i))
    End Sub

    Public Sub OnWorkCompleted(ByVal cancelled As Object)
        'WorkCompleted?.Invoke(CBool(cancelled))
        RaiseEvent WorkCompleted(CBool(cancelled))
    End Sub

    Public Event ProcessChanged As Action(Of Integer)
    Public Event WorkCompleted As Action(Of Boolean)
End Class
