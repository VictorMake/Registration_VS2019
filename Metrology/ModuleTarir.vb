Imports MathematicalLibrary

Module ModuleTarir

    Public PointTarirCount As Integer  ' КолТочекТарировки
    Public AverageInput() As Double      ' средние значения (вольт) прямого и обратного для контрольной точке для расчета полинома
    Public PhisicalEtalon() As Double   ' Значения измеряемой физической величины (эталон)
    Public CoefficientsPolynomial() As Double
    Public YOutput() As Double          ' вольт АЦП для расчета коэффициентов полинома
    Public XPhisical() As Double        ' воздействие (физика) ось Х
    Public PathReportTaring As String   ' strПутьПротоколТарировки
    ' для аппроксимации
    Public CoefficientOut() As Double   ' массив для передачи и приёма коэффициентов
    Private N As Integer                ' количество точек по оси X

    ''' <summary>
    ''' Расчет КоэфQ
    ''' </summary>
    ''' <param name="колСтепениСвободы"></param>
    ''' <returns></returns>
    Public Function SolveKofQ(ByRef колСтепениСвободы As Single) As Single
        Dim K(26) As Double
        Dim P(26) As Double
        K(1) = 17 : P(1) = 0.4
        K(2) = 18 : P(2) = 0.385
        K(3) = 19 : P(3) = 0.371
        K(4) = 20 : P(4) = 0.358
        K(5) = 22 : P(5) = 0.336
        K(6) = 24 : P(6) = 0.318
        K(7) = 26 : P(7) = 0.302
        K(8) = 28 : P(8) = 0.288
        K(9) = 30 : P(9) = 0.276
        K(10) = 35 : P(10) = 0.253
        K(11) = 40 : P(11) = 0.234
        K(12) = 45 : P(12) = 0.219
        K(13) = 50 : P(13) = 0.207
        K(14) = 55 : P(14) = 0.196
        K(15) = 60 : P(15) = 0.187
        K(16) = 65 : P(16) = 0.179
        K(17) = 70 : P(17) = 0.172
        K(18) = 80 : P(18) = 0.16
        K(19) = 90 : P(19) = 0.15
        K(20) = 100 : P(20) = 0.142
        K(21) = 110 : P(21) = 0.135
        K(22) = 120 : P(22) = 0.129
        K(23) = 150 : P(23) = 0.115
        K(24) = 200 : P(24) = 0.099
        K(25) = 250 : P(25) = 0.089
        K(26) = 300 : P(26) = 0.081
        N = 26
        'ReDim_CoefficientOut(N)
        Re.Dim(CoefficientOut, N)
        InputApproximation(N, K, P, CoefficientOut)
        ' по ЧислоСтепениСиободы вычисляtтся коэффициент Q для вероятности Р=0,95
        SolveKofQ = CSng(SplineApproximation(N, K, P, CoefficientOut, колСтепениСвободы))
    End Function

    ''' <summary>
    ''' Функция расчитывает значение Y по текущему Xtek
    ''' Nkon последнее значение X, Xg(1 to Nkon) значения по X, Yg(1 to Nkon) результат функции
    ''' M(1 to Nkon) коэффициенты по результату работы процедуры ВводАппроксимация
    ''' </summary>
    ''' <param name="Nkon"></param>
    ''' <param name="Xg"></param>
    ''' <param name="Yg"></param>
    ''' <param name="M"></param>
    ''' <param name="Xtek"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SplineApproximation(ByVal Nkon As Integer, ByRef Xg() As Double, ByRef Yg() As Double, ByRef M() As Double, ByVal Xtek As Double) As Double
        Dim I, J As Integer
        Dim P, f, D, H, R As Double

        If Xtek > Xg(Nkon) Then
            D = Xg(Nkon) - Xg(Nkon - 1)
            f = D * M(Nkon - 1) / 6 + (Yg(Nkon) - Yg(Nkon - 1)) / D
            Return f * (Xtek - Xg(Nkon)) + Yg(Nkon)

        End If

        If Xtek <= Xg(1) Then
            D = Xg(2) - Xg(1)
            f = -D * M(2) / 6 + (Yg(2) - Yg(1)) / D
            Return f * (Xtek - Xg(1)) + Yg(1)
        End If

        Do
            I += 1
        Loop While Xtek > Xg(I)

        J = I - 1 : D = Xg(I) - Xg(J) : H = Xtek - Xg(J) : R = Xg(I) - Xtek
        P = D * D / 6 : f = (M(J) * R * R * R + M(I) * H * H * H) / 6 / D

        Return f + ((Yg(J) - M(J) * P) * R + (Yg(I) - M(I) * P) * H) / D
    End Function

    ''' <summary>
    ''' Процедура по входным массивам Xg(1 to Nkon) значения по X, Yg(1 to Nkon) результат функции
    ''' вычисляет коэффициенты M(1 to Nkon) для сплайн аппроксимации - функции Аппроксимация
    ''' и заносит их в временный массив M() который можно переписать в общий массив для коэффициентов
    ''' </summary>
    ''' <param name="Nkon"></param>
    ''' <param name="Xg"></param>
    ''' <param name="Yg"></param>
    ''' <param name="M"></param>
    ''' <remarks></remarks>
    Public Sub InputApproximation(ByVal Nkon As Integer, ByRef Xg() As Double, ByRef Yg() As Double, ByRef M() As Double)
        Dim L(Nkon) As Double
        Dim R(Nkon) As Double
        Dim S(Nkon) As Double
        Dim K As Integer
        Dim f, E, D, H, P As Double

        D = Xg(2) - Xg(1)
        E = (Yg(2) - Yg(1)) / D

        For K = 2 To Nkon - 1
            H = D
            D = Xg(K + 1) - Xg(K)
            f = E
            E = (Yg(K + 1) - Yg(K)) / D
            L(K) = D / (D + H)
            R(K) = 1 - L(K)
            S(K) = 6 * (E - f) / (H + D)
        Next K

        For K = 2 To Nkon - 1
            P = 1 / (R(K) * L(K - 1) + 2)
            L(K) = -L(K) * P
            S(K) = (S(K) - R(K) * S(K - 1)) * P
        Next K

        M(Nkon) = 0
        L(Nkon - 1) = S(Nkon - 1)
        M(Nkon - 1) = L(Nkon - 1)

        For K = Nkon - 2 To 1 Step -1
            L(K) = L(K) * L(K + 1) + S(K)
            M(K) = L(K)
        Next K
    End Sub
End Module