Friend Class ДлительностьОтИндексаДоСтабильногоРоста
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
    ''' Порог Роста От Минимального
    ''' </summary>
    ''' <returns></returns>
    Public Property ThresholdGrowthFromMinimal() As Double

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
    End Sub

    Public Overrides Sub Calculation()
        Dim I, IndexStart As Integer
        Dim isTstopFound, isHoleFound As Boolean ' провал Найден
        Dim minimumWithThreshold As Double ' минимальное Значение С Порогом

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        For I = mIndexTstart To mGraphMaximum
            ' поиск минимального  в диапазоне
            If MeasuredValues(mIndexParameter, I) < mMinValue Then
                mMinValue = MeasuredValues(mIndexParameter, I)
                mIndexMinValue = I
            End If
        Next

        For I = mIndexMinValue To mGraphMaximum
            ' поиск максимального от mIndexMinValue до конца
            If MeasuredValues(mIndexParameter, I) > mMaxValue Then
                mMaxValue = MeasuredValues(mIndexParameter, I)
                mIndexMaxValue = I
                isTstopFound = True
            End If
        Next

        ' поиск первого значения превышающего порог роста
        IndexStart = mIndexMinValue
        minimumWithThreshold = MeasuredValues(mIndexParameter, mIndexMinValue) + ThresholdGrowthFromMinimal
        For I = mIndexMinValue To mIndexMaxValue
            If MeasuredValues(mIndexParameter, I) > minimumWithThreshold Then
                IndexStart = I
                Exit For
            End If
        Next

        ' первоначальное присваивание
        mIndexTstop = IndexStart
        ' по фронту
        For I = IndexStart To mIndexMaxValue - 2
            'If MeasuredValues(mIndexParameter, I) - MeasuredValues(mIndexParameter, I + 1) > mПорогРостаОтМинимального Then
            If MeasuredValues(mIndexParameter, I) > MeasuredValues(mIndexParameter, I + 1) Then
                ' есть локальный провал в точке MeasuredValues(mIndexParameter, I + 1)
                mIndexTstop = I + 2
                isHoleFound = True
                Exit For
            End If
        Next

        If isHoleFound Then
            IndexStart = mIndexTstop
            For I = IndexStart To mIndexMaxValue - 2
                If MeasuredValues(mIndexParameter, I + 1) > MeasuredValues(mIndexParameter, I) Then
                    ' есть подьем после провала
                    mIndexTstop = I + 2
                    Exit For
                End If
            Next
        End If

        Astart = MeasuredValues(mIndexParameter, mIndexTstart)
        Astop = MeasuredValues(mIndexParameter, mIndexTstop)
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class