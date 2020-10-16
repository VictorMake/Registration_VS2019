Friend Class ДлительностьОтИндексаДоСтабильногоРоста
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
    Dim mМаксимальноеЗначение, mМинимальноеЗначение As Double
    Dim mИндексМаксимальногоЗначения, mИндексМинимальногоЗначения As Integer
    Dim mIsErrors As Boolean
    Dim mИмяПараметра, mErrorsMessage As String
    Dim marrЗначения(,) As Double
    Dim mmyTypeList() As TypeSmallParameter
    Dim mПорогРостаОтМинимального As Double
    Dim mGraphMinimum, mGraphMaximum As Short

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

    Public Property ПорогРостаОтМинимального() As Double
        Get
            Return mПорогРостаОтМинимального
        End Get
        Set(ByVal Value As Double)
            mПорогРостаОтМинимального = Value
        End Set
    End Property

    Public Sub New(ByVal ИмяПараметра As String,
                   ByVal ЧастотаКадра As Integer,
                   ByVal arrЗначенияПараметров(,) As Double,
                   ByVal myTypeList() As TypeSmallParameter,
                   ByVal Minimum As Double, ByVal Maximum As Double)

        mИмяПараметра = ИмяПараметра
        marrЗначения = CType(arrЗначенияПараметров.Clone, Double(,))
        mmyTypeList = CType(myTypeList.Clone, TypeSmallParameter())
        mДлительностьТакта = 1 / ЧастотаКадра
        mErrorsMessage = "Параметр: " & mИмяПараметра & vbCrLf
        mМинимальноеЗначение = Double.MaxValue
        mМаксимальноеЗначение = Double.MinValue
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N, СтартовыйИндекс As Integer
        Dim параметрНайден, индексТконечноеНайден, провалНайден As Boolean
        Dim минимальноеЗначениеСПорогом As Double

        'находим индекс параметра
        For J = 1 To UBound(mmyTypeList)
            If mmyTypeList(J).NameParameter = mИмяПараметра AndAlso mmyTypeList(J).IsVisible Then
                mИндексПараметра = J - 1
                параметрНайден = True
                Exit For
            End If
        Next

        If Not параметрНайден Then
            mIsErrors = True
            mErrorsMessage += "Параметр " & mИмяПараметра & " не найден" & vbCrLf
            Exit Sub
        End If

        'N = UBound(marrЗначения, 2)
        N = mGraphMaximum
        For I = mИндексТначальное To N
            'поиск минимального  в диапазоне
            If marrЗначения(mИндексПараметра, I) < mМинимальноеЗначение Then
                mМинимальноеЗначение = marrЗначения(mИндексПараметра, I)
                mИндексМинимальногоЗначения = I
            End If
        Next

        For I = mИндексМинимальногоЗначения To N
            'поиск максимального от mИндексМинимальногоЗначения до конца
            If marrЗначения(mИндексПараметра, I) > mМаксимальноеЗначение Then
                mМаксимальноеЗначение = marrЗначения(mИндексПараметра, I)
                mИндексМаксимальногоЗначения = I
                индексТконечноеНайден = True
            End If
        Next

        'поиск первого значения превышающего порог роста
        СтартовыйИндекс = mИндексМинимальногоЗначения
        минимальноеЗначениеСПорогом = marrЗначения(mИндексПараметра, mИндексМинимальногоЗначения) + mПорогРостаОтМинимального
        For I = mИндексМинимальногоЗначения To mИндексМаксимальногоЗначения
            If marrЗначения(mИндексПараметра, I) > минимальноеЗначениеСПорогом Then
                СтартовыйИндекс = I
                Exit For
            End If
        Next

        'первоначальное присваивание
        mИндексТконечное = СтартовыйИндекс
        'по фронту
        For I = СтартовыйИндекс To mИндексМаксимальногоЗначения - 2
            'If marrЗначения(mИндексПараметра, I) - marrЗначения(mИндексПараметра, I + 1) > mПорогРостаОтМинимального Then
            If marrЗначения(mИндексПараметра, I) > marrЗначения(mИндексПараметра, I + 1) Then
                'есть локальный провал в точке marrЗначения(mИндексПараметра, I + 1)
                mИндексТконечное = I + 2
                провалНайден = True
                Exit For
            End If
        Next

        If провалНайден Then
            СтартовыйИндекс = mИндексТконечное
            For I = СтартовыйИндекс To mИндексМаксимальногоЗначения - 2
                If marrЗначения(mИндексПараметра, I + 1) > marrЗначения(mИндексПараметра, I) Then
                    'есть подьем после провала
                    mИндексТконечное = I + 2
                    Exit For
                End If
            Next
        End If

        mАначальное = marrЗначения(mИндексПараметра, mИндексТначальное)
        mАконечное = marrЗначения(mИндексПараметра, mИндексТконечное)
        mТначальное = mИндексТначальное * mДлительностьТакта
        mТконечное = mИндексТконечное * mДлительностьТакта
        mТдлительность = mТконечное - mТначальное

        If mИндексТначальное <= mGraphMinimum Then
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
End Class
