Friend Class ДлительностьФронтаСпадаОтИндексаДоУровня
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
    Dim mОшибка As Boolean
    Dim mИмяПараметра, mТекстОшибки As String
    Dim marrЗначения(,) As Double
    Dim mmyTypeList() As TypeSmallParameter
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

    Public ReadOnly Property Ошибка() As Boolean
        Get
            Return mОшибка
        End Get
    End Property

    Public ReadOnly Property ТекстОшибки() As String
        Get
            Return mТекстОшибки
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
        mТекстОшибки = "Параметр: " & mИмяПараметра & vbCrLf
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N As Integer
        Dim параметрНайден, индексТконечноеНайден As Boolean

        'находим индекс параметра
        For J = 1 To UBound(mmyTypeList)
            If mmyTypeList(J).NameParameter = mИмяПараметра AndAlso mmyTypeList(J).IsVisible Then
                mИндексПараметра = J - 1
                параметрНайден = True
                Exit For
            End If
        Next

        If Not параметрНайден Then
            mОшибка = True
            mТекстОшибки += "Параметр " & mИмяПараметра & " не найден" & vbCrLf
            Exit Sub
        End If

        N = mGraphMaximum
        mАначальное = marrЗначения(mИндексПараметра, mИндексТначальное)

        If mАначальное < mАконечное Then
            'если фронт
            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) > mАконечное Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next
        Else
            'если спад
            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) < mАконечное Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next
        End If

        mТначальное = mИндексТначальное * mДлительностьТакта
        mТконечное = mИндексТконечное * mДлительностьТакта
        mТдлительность = mТконечное - mТначальное

        If mИндексТначальное <= mGraphMinimum Then
            mОшибка = True
            mТекстОшибки += "Тначальное не найдено" & vbCrLf
        End If

        If Not индексТконечноеНайден OrElse mИндексТконечное = mGraphMinimum Then
            mОшибка = True
            mТекстОшибки += "Тконечное не найдено" & vbCrLf
        End If

        If (mИндексТначальное = mИндексТконечное) AndAlso Not (mИндексТначальное = mGraphMinimum And mИндексТконечное = mGraphMinimum) Then
            mОшибка = True
            mТекстОшибки += "Тначальное и Тконечное равны" & vbCrLf
        End If
    End Sub
End Class
