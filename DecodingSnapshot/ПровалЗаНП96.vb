Friend Class ПровалЗаНП96
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
    Dim mВремяОсреднения As Double
    Dim mУменьшитьСреднее As Double
    Dim mIsErrors As Boolean
    Dim mИмяПараметра, mErrorsMessage As String
    Dim marrЗначения(,) As Double
    Dim marrПараметрыЛиста() As TypeSmallParameter
    Dim mGraphMinimum, mGraphMaximum As Short

    Public Sub New(ByVal ИмяПараметра As String,
                   ByVal ЧастотаКадра As Short,
                   ByVal arrЗначенияПараметров(,) As Double,
                   ByVal arrПараметрыЛиста() As TypeSmallParameter,
                   ByVal Minimum As Double, ByVal Maximum As Double)

        mИмяПараметра = ИмяПараметра
        marrЗначения = CType(arrЗначенияПараметров.Clone, Double(,))
        marrПараметрыЛиста = CType(arrПараметрыЛиста.Clone, TypeSmallParameter())
        mДлительностьТакта = 1 / ЧастотаКадра
        mErrorsMessage = "Параметр: " & mИмяПараметра & vbCrLf
        mВремяОсреднения = 2 'по умолчанию
        mУменьшитьСреднее = 2
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

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

    Public Property УменьшитьСреднее() As Double
        Get
            Return mУменьшитьСреднее
        End Get
        Set(ByVal Value As Double)
            mУменьшитьСреднее = Value
        End Set
    End Property

    Public Sub Расчет()
        Dim I, J, N, стартовыйИндекс As Integer
        Dim success, индексТначальноеНайден, индексТконечноеНайден As Boolean
        Dim count As Integer

        'находим индекс параметра
        For J = 1 To UBound(marrПараметрыЛиста)
            If marrПараметрыЛиста(J).NameParameter = mИмяПараметра AndAlso marrПараметрыЛиста(J).IsVisible Then
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

        'находим среднее за предыдущие 2 сек от mИндексТначальное
        If mИндексТначальное - mВремяОсреднения / mДлительностьТакта < mGraphMinimum Then
            стартовыйИндекс = mGraphMinimum
        Else
            стартовыйИндекс = mИндексТначальное - CInt(mВремяОсреднения / mДлительностьТакта)
        End If

        For I = стартовыйИндекс To mИндексТначальное
            mАначальное += marrЗначения(mИндексПараметра, I)
            count += 1
        Next

        mАначальное = mАначальное / count - mУменьшитьСреднее 'начало спада на 2 меньше среднего
        'находим первое значение где значение меньше mАначальное -это начало спада параметра
        For I = mИндексТначальное To N
            If marrЗначения(mИндексПараметра, I) < mАначальное Then
                mИндексТначальное = I
                индексТначальноеНайден = True
                Exit For
            End If
        Next

        'находим значение меньшее mАконечное для точной фиксации прохождения провала
        For I = mИндексТначальное To N
            If marrЗначения(mИндексПараметра, I) < mАконечное Then
                стартовыйИндекс = I
                Exit For
            End If
        Next

        'находим значение большее mАконечное
        For I = стартовыйИндекс To N
            If marrЗначения(mИндексПараметра, I) > mАконечное Then
                mИндексТконечное = I
                индексТконечноеНайден = True
                Exit For
            End If
        Next

        mАначальное = marrЗначения(mИндексПараметра, mИндексТначальное)
        mТначальное = mИндексТначальное * mДлительностьТакта
        mТконечное = mИндексТконечное * mДлительностьТакта
        mТдлительность = mТконечное - mТначальное

        If Not индексТначальноеНайден OrElse mИндексТначальное = mGraphMinimum Then
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
