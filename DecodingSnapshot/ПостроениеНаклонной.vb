Friend Class ПостроениеНаклонной
    Inherits Figure

    ''' <summary>
    ''' Время Осреднения
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeAverage() As Double

    ''' <summary>
    ''' Превышение Над Средним
    ''' </summary>
    ''' <returns></returns>
    Public Property ExcessOverAverage() As Double

    ''' <summary>
    ''' Аначальное Плюс 5
    ''' </summary>
    ''' <returns></returns>
    Public Property AstartPlus5() As Double

    Public Property ТBx() As Double

    ''' <summary>
    ''' Уровень 2 Линии
    ''' </summary>
    ''' <returns></returns>
    Public Property LevelLine2() As Double


    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        TimeAverage = 2 ' по умолчанию
        ExcessOverAverage = 2
        LevelLine2 = 5
    End Sub

    Public Overrides Sub Calculation()
        Dim I As Integer
        Dim isTstartFound, isTstopFound As Boolean
        Dim count As Integer
        Dim Ax, Ay, Bx, By, Cy, Dx, Dy, Ky As Double
        Dim currentTime1, currentTime2 As Double
        Dim A1, B1, C1, A2, B2, C2 As Double

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        ' среднее за первые 2 сек 
        For I = mGraphMinimum To mGraphMinimum + CInt(TimeAverage / ClockPeriod)
            Astart += MeasuredValues(mIndexParameter, I)
            count += 1
        Next

        Astart = Astart / count + ExcessOverAverage

        ' поиск максимального до конца
        For I = mGraphMinimum To mGraphMaximum
            If MeasuredValues(mIndexParameter, I) > mMaxValue Then
                mMaxValue = MeasuredValues(mIndexParameter, I)
                mIndexMaxValue = I
            End If
        Next

        ' первое значение где значение больше среднего плюс порог
        For I = mGraphMinimum To mIndexMaxValue
            If MeasuredValues(mIndexParameter, I) > Astart Then
                mIndexTstart = I
                isTstartFound = True
                Exit For
            End If
        Next

        Astart = MeasuredValues(mIndexParameter, mIndexTstart)
        mTstart = mIndexTstart * ClockPeriod
        AstartPlus5 = Astart + LevelLine2
        Ax = mTstart
        Ay = Astart
        'Dim Cx As Double = TimeMaxValue
        Cy = mMaxValue
        Bx = Ax
        By = Cy

        For I = mIndexTstart + 1 To mIndexMaxValue
            currentTime1 = I * ClockPeriod ' сек
            Bx = currentTime1

            For J As Integer = mIndexTstart To I
                ' вычисление координаты Y точки К на координате X для прямой АВ
                currentTime2 = J * ClockPeriod 'сек
                Ky = LinearInterpolation(currentTime2, Ax, Ay, Bx, By)
                If MeasuredValues(mIndexParameter, J) > Ky Then
                    ' найдено превышение значение параметра на текущей координате над графиком
                    ТBx = Bx
                    isTstopFound = True
                    Exit For
                End If
            Next

            If isTstopFound Then Exit For
        Next

        ' надо найти пересечение прямых (Ax,Ay,Bx,By) и (Ax,mАначальноеПлюс5,mТМаксимальногоЗначения,АначальноеПлюс5)
        EquationOfLine(Ax, Ay, Bx, By, A1, B1, C1)
        EquationOfLine(Ax, AstartPlus5, TimeMaxValue, AstartPlus5, A2, B2, C2)
        Dy = (A2 * C1 / A1 - C2) / (B2 - A2 * B1 / A1) ' в принципе зто значение mАначальноеПлюс5 т.к. параллельно
        Dx = (-B1 * Dy - C1) / A1

        Astop = Dy
        mIndexTstop = CInt(Dx / ClockPeriod)
        mTstop = Dx
        mTimeDuration = mTstop - mTstart

        ' было немного другое услови 
        'If Not isTstartFound Then
        '    mIsErrors = True
        '    mErrorsMessage += "Тначальное не найдено" & vbCrLf
        'End If
        mIsErrors = ShowTotalErrorsMessage.IsTimeFound(isTstartFound, isTstopFound, mIndexTstart, mIndexTstop, mGraphMinimum, mErrorsMessage)
    End Sub

    ''' <summary>
    ''' Уравнение Прямой
    ''' </summary>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <param name="A"></param>
    ''' <param name="B"></param>
    ''' <param name="C"></param>
    Private Sub EquationOfLine(X1 As Double, Y1 As Double, X2 As Double, Y2 As Double, ByRef A As Double, ByRef B As Double, ByRef C As Double)
        A = Y2 - Y1
        B = -(X2 - X1)
        C = (X2 + X1) * Y1 - (Y2 + Y1) * X1
    End Sub

    'Private Function График(ByVal ЗаданX As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Double
    '    If X2 - X1 = 0 Then
    '        Return Y1
    '    Else
    '        Return Y1 + (Y2 - Y1) * (ЗаданX - X1) / (X2 - X1)
    '    End If
    'End Function
End Class
