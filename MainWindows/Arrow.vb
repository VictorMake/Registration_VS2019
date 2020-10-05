''' <summary>
''' Стрелки на графике
''' </summary>
Friend Class Arrow
    Private mPointAnnotation As XYPointAnnotation
    Public Property PointAnnotation() As XYPointAnnotation
        Get
            Return mPointAnnotation
        End Get
        Set(ByVal Value As XYPointAnnotation)
            mPointAnnotation = Value
            mPointAnnotation.Tag = Me
        End Set
    End Property
    Public Property Plot1() As WaveformPlot
    Public Property Plot2() As WaveformPlot

    Public Property Y2() As Double

    Public Property X2() As Double

    Public Property Y1() As Double

    Public Property X1() As Double

    ''' <summary>
    ''' Надпись
    ''' </summary>
    ''' <returns></returns>
    Public Property Legend() As String

    ''' <summary>
    ''' Вид Стрелки
    ''' </summary>
    ''' <returns></returns>
    Public Property ViewArrow() As ArrowType

    Public Overrides Function ToString() As String
        Return Legend
    End Function
End Class