Friend Class ПостроениеНаклонной
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
    Dim mМаксимальноеЗначение As Double
    Dim mИндексМаксимальногоЗначения As Integer
    Dim mВремяОсреднения As Double
    Dim mПревышениеНадСредним As Double
    Dim mАначальноеПлюс5, mТМаксимальногоЗначения, mBx, mУровень2Линии As Double
    Dim mIsErrors As Boolean
    Dim mИмяПараметра, mErrorsMessage As String
    Dim marrЗначения(,) As Double
    Dim mGraphMinimum, mGraphMaximum As Short
    Dim mmyTypeList() As TypeSmallParameter

    Public Property GraphMinimum() As Double
        Get
            Return CDbl(mGraphMinimum)
        End Get
        Set(ByVal Value As Double)
            mGraphMinimum = CShort(Value)
        End Set
    End Property

    Public Property GraphMaximum() As Double
        Get
            Return CDbl(mGraphMaximum)
        End Get
        Set(ByVal Value As Double)
            mGraphMaximum = CShort(Value)
        End Set
    End Property

    Public ReadOnly Property ИндексПараметра() As Integer
        Get
            Return mИндексПараметра
        End Get
    End Property

    Public Property ИндексТначальное() As Integer
        Get
            Return mИндексТначальное
        End Get
        Set(ByVal Value As Integer)
            mИндексТначальное = Value
        End Set
    End Property

    Public ReadOnly Property ИндексТконечное() As Integer
        Get
            Return mИндексТконечное
        End Get
    End Property

    Public ReadOnly Property ДлительностьТакта() As Double
        Get
            Return mДлительностьТакта
        End Get
    End Property

    Public ReadOnly Property Тначальное() As Double
        Get
            Return mТначальное
        End Get
    End Property

    Public ReadOnly Property Тконечное() As Double
        Get
            Return mТконечное
        End Get
    End Property

    Public ReadOnly Property Тдлительность() As Double
        Get
            Return mТдлительность
        End Get
    End Property

    Public ReadOnly Property IsErrors() As Boolean
        Get
            Return mIsErrors
        End Get
    End Property

    Public ReadOnly Property ErrorsMessage() As String
        Get
            Return mErrorsMessage
        End Get
    End Property

    Public Property Аначальное() As Double
        Get
            Return mАначальное
        End Get
        Set(ByVal Value As Double)
            mАначальное = Value
        End Set
    End Property

    Public Property Аконечное() As Double
        Get
            Return mАконечное
        End Get
        Set(ByVal Value As Double)
            mАконечное = Value
        End Set
    End Property

    Public Property ВремяОсреднения() As Double
        Get
            Return mВремяОсреднения
        End Get
        Set(ByVal Value As Double)
            mВремяОсреднения = Value
        End Set
    End Property

    Public Property ПревышениеНадСредним() As Double
        Get
            Return mПревышениеНадСредним
        End Get
        Set(ByVal Value As Double)
            mПревышениеНадСредним = Value
        End Set
    End Property

    Public ReadOnly Property МаксимальноеЗначение() As Double
        Get
            Return mМаксимальноеЗначение
        End Get
    End Property

    Public ReadOnly Property ИндексМаксимальногоЗначения() As Integer
        Get
            Return mИндексМаксимальногоЗначения
        End Get
    End Property

    Public Property АначальноеПлюс5() As Double
        Get
            Return mАначальноеПлюс5
        End Get
        Set(ByVal Value As Double)
            mАначальноеПлюс5 = Value
        End Set
    End Property

    Public ReadOnly Property ТМаксимальногоЗначения() As Double
        Get
            Return mТМаксимальногоЗначения
        End Get
    End Property

    Public Property ТBx() As Double
        Get
            Return mBx
        End Get
        Set(ByVal Value As Double)
            mBx = Value
        End Set
    End Property

    Public Property Уровень2Линии() As Double
        Get
            Return mУровень2Линии
        End Get
        Set(ByVal Value As Double)
            mУровень2Линии = Value
        End Set
    End Property

    Public Sub New(ByVal ИмяПараметра As String,
                   ByVal ЧастотаКадра As Integer,
                   ByVal arrЗначенияПараметров(,) As Double,
                   ByVal myTypeList() As TypeSmallParameter,
                   ByVal Minimum As Double,
                   ByVal Maximum As Double)

        mИмяПараметра = ИмяПараметра
        marrЗначения = CType(arrЗначенияПараметров.Clone, Double(,))
        mmyTypeList = CType(myTypeList.Clone, TypeSmallParameter())
        mДлительностьТакта = 1 / ЧастотаКадра
        mErrorsMessage = "Параметр: " & mИмяПараметра & vbCrLf
        mВремяОсреднения = 2 'по умолчанию
        mПревышениеНадСредним = 2
        mУровень2Линии = 5
        mМаксимальноеЗначение = Double.MinValue
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N As Integer
        Dim success, индексТначальноеНайден, индексТконечноеНайден As Boolean
        Dim count As Integer
        Dim Ax, Ay, Bx, By, Cx, Cy, Dx, Dy, Ky As Double
        Dim текущееВремя1, текущееВремя2 As Double
        Dim A1, B1, C1, A2, B2, C2 As Double

        'находим индекс параметра
        For J = 1 To UBound(mmyTypeList)
            If mmyTypeList(J).NameParameter = mИмяПараметра AndAlso mmyTypeList(J).IsVisible Then
                mИндексПараметра = J - 1
                success = True
                Exit For
            End If
        Next

        If Not success Then
            mIsErrors = True
            mErrorsMessage += "Параметр " & mИмяПараметра & " не найден" & vbCrLf
            Exit Sub
        End If

        N = mGraphMaximum
        'находим среднее за первые 2 сек 
        For I = mGraphMinimum To mGraphMinimum + CInt(mВремяОсреднения / mДлительностьТакта)
            mАначальное += marrЗначения(mИндексПараметра, I)
            count += 1
        Next

        mАначальное = mАначальное / count + mПревышениеНадСредним

        'поиск максимального до конца
        For I = mGraphMinimum To N
            If marrЗначения(mИндексПараметра, I) > mМаксимальноеЗначение Then
                mМаксимальноеЗначение = marrЗначения(mИндексПараметра, I)
                mИндексМаксимальногоЗначения = I
            End If
        Next

        'находим первое значение где значение больше среднего плюс порог
        For I = mGraphMinimum To mИндексМаксимальногоЗначения
            If marrЗначения(mИндексПараметра, I) > mАначальное Then
                mИндексТначальное = I
                индексТначальноеНайден = True
                Exit For
            End If
        Next

        mТМаксимальногоЗначения = mИндексМаксимальногоЗначения * mДлительностьТакта
        mАначальное = marrЗначения(mИндексПараметра, mИндексТначальное)
        mТначальное = mИндексТначальное * mДлительностьТакта
        mАначальноеПлюс5 = mАначальное + mУровень2Линии
        Ax = mТначальное
        Ay = mАначальное
        Cx = mТМаксимальногоЗначения
        Cy = mМаксимальноеЗначение
        Bx = Ax
        By = Cy

        For I = mИндексТначальное + 1 To mИндексМаксимальногоЗначения
            текущееВремя1 = I * mДлительностьТакта 'сек
            Bx = текущееВремя1

            For J = mИндексТначальное To I
                'вычисляем координаты Y точки К на координате X для прямой АВ
                текущееВремя2 = J * mДлительностьТакта 'сек
                Ky = LinearInterpolation(текущееВремя2, Ax, Ay, Bx, By)
                If marrЗначения(mИндексПараметра, J) > Ky Then
                    'найдено превышение значение параметра на текущей координате над графиком
                    mBx = Bx
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next

            If индексТконечноеНайден Then Exit For
        Next

        'надо найти пересечение прямых (Ax,Ay,Bx,By) и (Ax,mАначальноеПлюс5,mТМаксимальногоЗначения,АначальноеПлюс5)
        УравнениеПрямой(Ax, Ay, Bx, By, A1, B1, C1)
        УравнениеПрямой(Ax, mАначальноеПлюс5, mТМаксимальногоЗначения, АначальноеПлюс5, A2, B2, C2)
        Dy = (A2 * C1 / A1 - C2) / (B2 - A2 * B1 / A1) 'в принципе зто значение mАначальноеПлюс5 т.к. параллельно
        Dx = (-B1 * Dy - C1) / A1

        mАконечное = Dy
        mИндексТконечное = CInt(Dx / mДлительностьТакта)
        mТконечное = Dx
        mТдлительность = mТконечное - mТначальное

        If Not индексТначальноеНайден Then
            mIsErrors = True
            mErrorsMessage += "Тначальное не найдено" & vbCrLf
        End If

        If Not индексТконечноеНайден OrElse mИндексТконечное = mGraphMinimum Then
            mIsErrors = True
            mErrorsMessage += "Тконечное не найдено" & vbCrLf
        End If

        If (mИндексТначальное = mИндексТконечное) AndAlso Not (mИндексТначальное = mGraphMinimum And mИндексТконечное = mGraphMinimum) Then
            mIsErrors = True
            mErrorsMessage += "Тначальное и Тконечное равны" & vbCrLf
        End If
    End Sub

    Private Sub УравнениеПрямой(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double, ByRef A As Double, ByRef B As Double, ByRef C As Double)
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
