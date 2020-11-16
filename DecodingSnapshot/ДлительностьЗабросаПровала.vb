Friend Class ДлительностьЗабросаПровала
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
        Dim isTstartFound, isTstopFound As Boolean

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, Astart, Astop, ErrorsMessage, Parameters, IndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        If Astart < Astop Then
            ' если заброс
            ' ищем первое значение которое меньше Аначальное
            If IndexStart = 0 Then
                For I = mGraphMinimum To mGraphMaximum
                    If MeasuredValues(IndexParameter, I) < Astart Then
                        IndexStart = I
                        Exit For
                    End If
                Next
            End If

            For I = IndexStart To mGraphMaximum
                If MeasuredValues(IndexParameter, I) > Astop Then
                    mIndexTstart = I
                    isTstartFound = True
                    Exit For
                End If
            Next

            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(IndexParameter, I) < Astop Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next

            ' поиск максимального
            For I = mIndexTstart To mIndexTstop
                If MeasuredValues(IndexParameter, I) > mMaxValue Then
                    mMaxValue = MeasuredValues(IndexParameter, I)
                    mIndexMaxValue = I
                End If
            Next
        Else
            ' если провал
            ' ищем первое значение которое больше Аначальное
            If IndexStart = 0 Then
                For I = mGraphMinimum To mGraphMaximum
                    If MeasuredValues(IndexParameter, I) > Astart Then
                        IndexStart = I
                        Exit For
                    End If
                Next
            End If

            For I = IndexStart To mGraphMaximum
                If MeasuredValues(IndexParameter, I) < Astop Then
                    mIndexTstart = I
                    isTstartFound = True
                    Exit For
                End If
            Next

            For I = mIndexTstart To mGraphMaximum
                If MeasuredValues(IndexParameter, I) > Astop Then
                    mIndexTstop = I
                    isTstopFound = True
                    Exit For
                End If
            Next

            ' поиск минимального
            For I = mIndexTstart To mIndexTstop
                If MeasuredValues(IndexParameter, I) < mMinValue Then
                    mMinValue = MeasuredValues(IndexParameter, I)
                    mIndexMinValue = I
                End If
            Next
        End If

        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstartFound, isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class