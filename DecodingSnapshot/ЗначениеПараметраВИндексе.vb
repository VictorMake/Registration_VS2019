Friend Class ЗначениеПараметраВИндексе
    Dim mИндексПараметра, mИндексТначальное As Integer
    Dim mДлительностьТакта, mТначальное As Double
    Dim mЗначениеПараметра As Double
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

    Public WriteOnly Property ИндексТначальное() As Integer
        Set(ByVal Value As Integer)
            mИндексТначальное = Value
        End Set
    End Property

    Public ReadOnly Property Тначальное() As Double
        Get
            Return mТначальное
        End Get
    End Property

    Public ReadOnly Property ЗначениеПараметра() As Double
        Get
            Return mЗначениеПараметра
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
        Dim параметрНайден As Boolean

        'находим индекс параметра
        For J As Integer = 1 To UBound(mmyTypeList)
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

        mЗначениеПараметра = marrЗначения(mИндексПараметра, mИндексТначальное)
        mТначальное = mИндексТначальное * mДлительностьТакта
    End Sub
End Class
