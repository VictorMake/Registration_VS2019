Friend Class ДлительностьФронтаСпадаОтИндексаДоУровня
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

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
    End Sub

    Public Overrides Sub Calculation()
        Dim I As Integer
        Dim isTstopFound As Boolean

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        Astart = MeasuredValues(mIndexParameter, mIndexTstart)

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

        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class
