Friend Class РискиНастроекПараметров
    Inherits Figure

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
    End Sub

    Public Overrides Sub Calculation()
        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        mIndexTstop = mGraphMaximum
        mIndexTstart = CInt(mGraphMaximum - 5 / ClockPeriod) ' длина риски 5 сек
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
    End Sub
End Class