Friend Class ЗабросN1ОтносительноУстановившегося
    Inherits Figure

    '''' <summary>
    '''' Индекс Т начальное
    '''' </summary>
    '''' <returns></returns>
    Public Overloads Property IndexTstart() As Integer
        Get
            Return mIndexTstart
        End Get
        Set(ByVal Value As Integer)
            mIndexTstart = Value
        End Set
    End Property

    Public ReadOnly Property DeltaA() As Double
        Get
            Return mDeltaA
        End Get
    End Property

    ''' <summary>
    ''' Время Осреднения
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeAverage() As Double

    Private mDeltaA As Double

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        TimeAverage = 2 ' по умолчанию
        IndexTstart = GraphMinimum
    End Sub

    Public Overrides Sub Calculation()
        Dim I As Integer
        Dim count As Integer
        Dim maximum As Double = Double.MinValue
        Dim indexMaximum As Integer
        Dim average As Double ' Среднее

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        ' среднее за последние 2 сек
        For I = mGraphMaximum - CInt(TimeAverage / ClockPeriod) To mGraphMaximum
            average += MeasuredValues(mIndexParameter, I)
            count += 1
        Next
        average /= count

        ' первый максимум 
        For I = mIndexTstart To mGraphMaximum
            'поиск максимального
            If MeasuredValues(mIndexParameter, I) > maximum Then
                maximum = MeasuredValues(mIndexParameter, I)
                indexMaximum = I
            End If
        Next

        mIndexTstop = mGraphMaximum
        mIndexTstart = indexMaximum
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        Astart = maximum
        Astop = average
        mDeltaA = maximum - average

        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class
