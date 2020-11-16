Friend Class ДлительностьФронтаСпада
    Inherits Figure

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        mIndexTstart = GraphMinimum
    End Sub

    Public Overrides Sub Calculation()
        Dim I, IndexStart As Integer
        Dim isTstartFound, isTstopFound As Boolean

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, Astart, Astop, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        If Astart < Astop Then
            ' если фронт
            ' ищем первое значение которое меньше mАначальное
            For I = mGraphMinimum To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) < Astart Then
                    IndexStart = I
                    Exit For
                End If
            Next

            For I = IndexStart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > Astart Then
                    mIndexTstart = I - 1
                    isTstartFound = True
                    Exit For
                End If
            Next

            If mIndexTstart < 0 Then mIndexTstart = 0

            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > Astop Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next
        Else
            ' если спад
            ' ищем первое значение которое больше mАначальное
            For I = mGraphMinimum To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > Astart Then
                    IndexStart = I
                    Exit For
                End If
            Next

            For I = IndexStart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) < Astart Then
                    mIndexTstart = I
                    isTstartFound = True
                    Exit For
                End If
            Next

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
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstartFound, isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class
