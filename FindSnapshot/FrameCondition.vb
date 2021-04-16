Imports MathematicalLibrary
''' <summary>
''' Кадр Условия
''' </summary>
Public Class FrameCondition
    Private mConditionWhereOK() As Boolean

    Public Property KeyIDFrame() As Integer

    ''' <summary>
    ''' Условия Где Сработало
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <returns></returns>
    Public Property ConditionWhereOK(ByVal Index As Integer) As Boolean
        Get
            Return mConditionWhereOK(Index)
        End Get

        Set(ByVal Value As Boolean)
            mConditionWhereOK(Index) = Value
        End Set
    End Property

    ''' <summary>
    ''' Время Выполнения Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeConditionIsOk() As Single

    Public Property Frequency() As Integer

    ''' <summary>
    ''' Надпись
    ''' </summary>
    ''' <returns></returns>
    Public Property Title() As String

    Public Property Text() As String

    Public Sub New(ByVal conditionsCount As Integer)
        Re.Dim(mConditionWhereOK, conditionsCount)
    End Sub

    Public Overrides Function ToString() As String
        Dim strConditionWhereOK As String = Nothing

        For I As Integer = 1 To UBound(mConditionWhereOK)
            If mConditionWhereOK(I) Then
                strConditionWhereOK &= "1 "
            Else
                strConditionWhereOK &= "0 "
            End If
        Next
        Return $"mKeyID ={KeyIDFrame}; Время Выполнения Условия ={TimeConditionIsOk}; Условия Где Сработало ={strConditionWhereOK}; Надпись ={Title}; Text={Text}"
    End Function
End Class
