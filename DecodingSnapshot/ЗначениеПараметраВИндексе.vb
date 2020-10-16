Friend Class ЗначениеПараметраВИндексе
    Dim mИндексПараметра, mИндексТначальное As Integer
    Dim mДлительностьТакта, mТначальное As Double
    Dim mЗначениеПараметра As Double
    Dim mIsErrors As Boolean
    Dim mИмяПараметра, mErrorsMessage As String
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
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim success As Boolean

        'находим индекс параметра
        For J As Integer = 1 To UBound(mmyTypeList)
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

        mЗначениеПараметра = marrЗначения(mИндексПараметра, mИндексТначальное)
        mТначальное = mИндексТначальное * mДлительностьТакта
    End Sub
End Class
