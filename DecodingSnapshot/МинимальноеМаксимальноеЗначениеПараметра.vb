Friend Class МинимальноеМаксимальноеЗначениеПараметра
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта As Double
    Dim mМаксимальноеЗначение, mМинимальноеЗначение As Double
    Dim mИндексМаксимальногоЗначения, mИндексМинимальногоЗначения As Integer
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

    Public ReadOnly Property ДлительностьТакта() As Double
        Get
            Return mДлительностьТакта
        End Get
    End Property

    Public WriteOnly Property ИндексТначальное() As Integer
        Set(ByVal Value As Integer)
            mИндексТначальное = Value
        End Set
    End Property

    Public WriteOnly Property ИндексТконечное() As Integer
        Set(ByVal Value As Integer)
            mИндексТконечное = Value
        End Set
    End Property

    Public ReadOnly Property МаксимальноеЗначение() As Double
        Get
            Return mМаксимальноеЗначение
        End Get
    End Property

    Public ReadOnly Property МинимальноеЗначение() As Double
        Get
            Return mМинимальноеЗначение
        End Get
    End Property

    Public ReadOnly Property ИндексМаксимальногоЗначения() As Integer
        Get
            Return mИндексМаксимальногоЗначения
        End Get
    End Property

    Public ReadOnly Property ИндексМинимальногоЗначения() As Integer
        Get
            Return mИндексМинимальногоЗначения
        End Get
    End Property

    Public ReadOnly Property ТМаксимальногоЗначения() As Double
        Get
            Return mИндексМаксимальногоЗначения * mДлительностьТакта
        End Get
    End Property

    Public ReadOnly Property ТМинимальногоЗначения() As Double
        Get
            Return mИндексМинимальногоЗначения * mДлительностьТакта
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
        mМинимальноеЗначение = Double.MaxValue
        mМаксимальноеЗначение = Double.MinValue
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N, стартовыйИндекс, стоповыйИндекс As Integer
        Dim success As Boolean

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
        стартовыйИндекс = mИндексТначальное : стоповыйИндекс = mИндексТконечное

        If mИндексТначальное = mИндексТконечное Then стартовыйИндекс = mGraphMinimum : стоповыйИндекс = N
        If mИндексТначальное = mGraphMinimum AndAlso mИндексТконечное <> mGraphMinimum Then стартовыйИндекс = mGraphMinimum : стоповыйИндекс = mИндексТконечное
        If mИндексТначальное <> mGraphMinimum AndAlso mИндексТконечное = mGraphMinimum Then стартовыйИндекс = mИндексТначальное : стоповыйИндекс = N
        If mИндексТначальное <> 0 AndAlso mИндексТконечное = 0 Then стартовыйИндекс = mИндексТначальное : стоповыйИндекс = N

        For I = стартовыйИндекс To стоповыйИндекс
            'поиск максимального
            If marrЗначения(mИндексПараметра, I) > mМаксимальноеЗначение Then
                mМаксимальноеЗначение = marrЗначения(mИндексПараметра, I)
                mИндексМаксимальногоЗначения = I
            End If

            'поиск минимального
            If marrЗначения(mИндексПараметра, I) < mМинимальноеЗначение Then
                mМинимальноеЗначение = marrЗначения(mИндексПараметра, I)
                mИндексМинимальногоЗначения = I
            End If
        Next
    End Sub
End Class
