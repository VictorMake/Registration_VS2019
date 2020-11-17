Friend Class ПровалЗаНП96
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
    ''' Уменьшить Среднее
    ''' </summary>
    ''' <returns></returns>
    Public Property DecreaseAverage() As Double

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        TimeAverage = 2 ' по умолчанию
        DecreaseAverage = 2
    End Sub

    Public Overrides Sub Calculation()
        Dim I, IndexStart As Integer
        Dim isTstartFound, isTstopFound As Boolean
        Dim count As Integer

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        ' среднее за предыдущие 2 сек от mIndexTstart
        If mIndexTstart - TimeAverage / ClockPeriod < mGraphMinimum Then
            IndexStart = mGraphMinimum
        Else
            IndexStart -= CInt(TimeAverage / ClockPeriod)
        End If

        For I = IndexStart To mIndexTstart
            Astart += MeasuredValues(mIndexParameter, I)
            count += 1
        Next

        Astart = Astart / count - DecreaseAverage ' начало спада на 2 меньше среднего
        ' первое значение, где значение меньше mАначальное - это начало спада параметра
        For I = mIndexTstart To mGraphMaximum
            If MeasuredValues(mIndexParameter, I) < Astart Then
                mIndexTstart = I
                isTstartFound = True
                Exit For
            End If
        Next

        ' значение меньшее Аконечное для точной фиксации прохождения провала
        For I = mIndexTstart To mGraphMaximum
            If MeasuredValues(mIndexParameter, I) < Astop Then
                IndexStart = I
                Exit For
            End If
        Next

        ' значение большее Аконечное
        For I = IndexStart To mGraphMaximum
            If MeasuredValues(mIndexParameter, I) > Astop Then
                mIndexTstop = I
                isTstopFound = True
                Exit For
            End If
        Next

        Astart = MeasuredValues(mIndexParameter, mIndexTstart)
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart

        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstartFound, isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class
