Friend Class ДлительностьФронтаОтИндексаДоN1Уст_2
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

    ''' <summary>
    ''' Время Осреднения
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeAverage() As Double

    ''' <summary>
    ''' Процент
    ''' </summary>
    ''' <returns></returns>
    Public Property Percent() As Double

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        TimeAverage = 2 ' по умолчанию
        Percent = 2 ' по умолчанию
        IndexTstart = GraphMinimum
    End Sub

    Public Overrides Sub Calculation()
        Dim I As Integer
        Dim isTstopFound As Boolean
        Dim count As Integer

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        ' среднее за последние 2 сек
        For I = mGraphMaximum - CInt(TimeAverage / ClockPeriod) To mGraphMaximum
            Astop += MeasuredValues(mIndexParameter, I)
            count += 1
        Next

        Astop = Astop / count - Percent ' минус 2 %

        If Astart < Astop Then
            ' если фронт
            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > Astop Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next
        Else
            ' если спад
            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) < Astop Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next
        End If

        Astart = MeasuredValues(mIndexParameter, mIndexTstart)
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class