Friend Class ПровалN1ОтносительноУстановившегося
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
        Dim isMinimumFound As Boolean ' минимум Найден
        Dim count As Integer
        Dim minimum As Double = Double.MaxValue ' минимальное Значение
        Dim maximum As Double = Double.MinValue ' максимальное Значение
        Dim indexMaximum, indexMinimum As Integer
        Dim jumpN1 As Double = 0.2 ' скачок Значение
        Dim indexJump As Integer = CInt(jumpN1 / ClockPeriod) ' 0.2 секунды 

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        ' среднее за последние 2 сек
        For I = mGraphMaximum - CInt(TimeAverage / ClockPeriod) To mGraphMaximum
            Astop += MeasuredValues(mIndexParameter, I)
            count += 1
        Next

        Astop /= count

        ' первый максимум 
        For I = mIndexTstart To mGraphMaximum
            ' поиск максимального
            If MeasuredValues(mIndexParameter, I) > maximum Then
                maximum = MeasuredValues(mIndexParameter, I)
                indexMaximum = I
            End If
        Next

        If (indexMaximum - mIndexTstart) * ClockPeriod > 10 Then
            ' если максимальное значение отстоит от mIndexTstart более 10 сек, то от него ищем локальный минимум
            ' 1 случай когда локальный максимум превышающего среднее зачение в пределах 10 сек до абсолютного максимума есть
            Dim indexFirstExcessAverage, indexLastExcessAverage As Integer ' ИндексПервогоПревышенияСреднего, ИндексПоследнегоПревышенияСреднего
            For I = mIndexTstart To indexMaximum
                If MeasuredValues(mIndexParameter, I) > Astop Then
                    indexFirstExcessAverage = I
                    Exit For
                End If
            Next

            For I = indexMaximum To mIndexTstart Step -1
                If MeasuredValues(mIndexParameter, I) < Astop Then
                    indexLastExcessAverage = I
                    Exit For
                End If
            Next

            If indexFirstExcessAverage < indexLastExcessAverage Then
                For I = indexFirstExcessAverage To indexLastExcessAverage
                    ' поиск минимального
                    If MeasuredValues(mIndexParameter, I) < minimum Then
                        minimum = MeasuredValues(mIndexParameter, I)
                        indexMinimum = I
                        isMinimumFound = True
                    End If
                Next
            End If

            If isMinimumFound = False Then
                minimum = Double.MaxValue
                ' 2 случай когда локальный максимум превышающего среднее зачение в пределах 10 сек до абсолютного максимума нет
                For I = mIndexTstart To indexMaximum - indexJump * 2
                    ' поиск локального минимального
                    If (MeasuredValues(mIndexParameter, I) - MeasuredValues(mIndexParameter, I + indexJump) > jumpN1) AndAlso (MeasuredValues(mIndexParameter, I + indexJump * 2) - MeasuredValues(mIndexParameter, I + indexJump) > jumpN1) Then
                        If I - indexJump > 0 Then
                            minimum = MeasuredValues(mIndexParameter, I - indexJump)
                            indexMinimum = I + indexJump
                            isMinimumFound = True
                            Exit For
                        End If
                    End If
                Next
            End If

            If isMinimumFound = False Then
                For I = indexMaximum To mIndexTstart Step -1
                    ' поиск минимального
                    If MeasuredValues(mIndexParameter, I) < minimum Then
                        minimum = MeasuredValues(mIndexParameter, I)
                        indexMinimum = I
                    End If
                Next
            End If
        Else
            ' минимум начиная с mIndexMaxValue
            For I = indexMaximum To mGraphMaximum
                'поиск минимального
                If MeasuredValues(mIndexParameter, I) < minimum Then
                    minimum = MeasuredValues(mIndexParameter, I)
                    indexMinimum = I
                End If
            Next
        End If

        mIndexTstop = mGraphMaximum
        mIndexTstart = indexMinimum
        Astart = MeasuredValues(mIndexParameter, mIndexTstart)
        mTstart = mIndexTstart * ClockPeriod
        mTstop = mIndexTstop * ClockPeriod
        mTimeDuration = mTstop - mTstart
        mDeltaA = Astop - Astart

        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub
End Class