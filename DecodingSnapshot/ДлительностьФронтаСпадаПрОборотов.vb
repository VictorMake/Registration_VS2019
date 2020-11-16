Friend Class ДлительностьФронтаСпадаПрОборотов
    Inherits Figure

    ''' <summary>
    ''' Температура
    ''' </summary>
    ''' <returns></returns>
    Public Property Temperature() As Double

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
    End Sub

    Public Overrides Sub Calculation()
        Dim I, IndexStart As Integer
        Dim isTstartFound, isTstopFound As Boolean
        Dim transferConstant As Double = Math.Sqrt(Const288 / (Temperature + Kelvin)) ' Коэ Приведения
        Dim AstopPhysical As Double = Astop / transferConstant ' Аконечное это приведенный, а АконечноеФиз наоборот уменьшенный

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, Astart, Astop, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        If Astart < AstopPhysical Then
            ' если фронт
            ' ищем первое значение которое меньше Аначальное
            For I = mGraphMinimum To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) < Astart Then
                    IndexStart = I
                    Exit For
                End If
            Next

            For I = IndexStart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > Astart Then
                    mIndexTstart = I
                    isTstartFound = True
                    Exit For
                End If
            Next

            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(mIndexParameter, I) > AstopPhysical Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next
        Else
            ' если спад
            ' ищем первое значение которое больше Аначальное
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
                If MeasuredValues(mIndexParameter, I) < AstopPhysical Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next
        End If

        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        Astop = AstopPhysical
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstartFound, isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class