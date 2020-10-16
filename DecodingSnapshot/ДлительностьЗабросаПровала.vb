Friend Class ДлительностьЗабросаПровала
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАпорога As Double 'для заброса может быть чуть меньше mАпорога или для провала может быть чуть больше mАначальное
    Dim mМаксимальноеЗначение, mМинимальноеЗначение As Double
    Dim mИндексМаксимальногоЗначения, mИндексМинимальногоЗначения As Integer
    Dim mСтартовыйИндекс As Integer
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

    Public ReadOnly Property ИндексТначальное() As Integer
        Get
            Return mИндексТначальное
        End Get
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

    Public Property Аначальное() As Double
        Get
            Return mАначальное
        End Get
        Set(ByVal Value As Double)
            mАначальное = Value
        End Set
    End Property

    Public Property Апорога() As Double
        Get
            Return mАпорога
        End Get
        Set(ByVal Value As Double)
            mАпорога = Value
        End Set
    End Property

    Public Property СтартовыйИндекс() As Integer
        Get
            Return mСтартовыйИндекс
        End Get
        Set(ByVal Value As Integer)
            mСтартовыйИндекс = Value
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
        mМинимальноеЗначение = 1.0E+38
        mМаксимальноеЗначение = -1.0E+38
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N As Integer
        Dim параметрНайден, индексТначальноеНайден, индексТконечноеНайден As Boolean

        'проверка на корректность введенныы параметров
        If mАначальное = mАпорога Then
            mIsErrors = True
            mErrorsMessage += "Не правильно введены Аначальное или Апорога" & vbCrLf
            Exit Sub
        End If

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

        N = mGraphMaximum

        If mАначальное < mАпорога Then
            'если заброс
            'ищем первое значение которое меньше mАначальное
            If mСтартовыйИндекс = 0 Then
                For I = mGraphMinimum To N
                    If marrЗначения(mИндексПараметра, I) < mАначальное Then
                        mСтартовыйИндекс = I
                        Exit For
                    End If
                Next
            End If

            For I = mСтартовыйИндекс To N
                If marrЗначения(mИндексПараметра, I) > mАпорога Then
                    mИндексТначальное = I
                    индексТначальноеНайден = True
                    Exit For
                End If
            Next

            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) < mАпорога Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next

            'поиск максимального
            For I = mИндексТначальное To mИндексТконечное
                If marrЗначения(mИндексПараметра, I) > mМаксимальноеЗначение Then
                    mМаксимальноеЗначение = marrЗначения(mИндексПараметра, I)
                    mИндексМаксимальногоЗначения = I
                End If
            Next
        Else
            'если провал
            'ищем первое значение которое больше mАначальное
            If mСтартовыйИндекс = 0 Then
                For I = mGraphMinimum To N
                    If marrЗначения(mИндексПараметра, I) > mАначальное Then
                        mСтартовыйИндекс = I
                        Exit For
                    End If
                Next
            End If

            For I = mСтартовыйИндекс To N
                If marrЗначения(mИндексПараметра, I) < mАпорога Then
                    mИндексТначальное = I
                    индексТначальноеНайден = True
                    Exit For
                End If
            Next

            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) > mАпорога Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next

            'поиск минимального
            For I = mИндексТначальное To mИндексТконечное
                If marrЗначения(mИндексПараметра, I) < mМинимальноеЗначение Then
                    mМинимальноеЗначение = marrЗначения(mИндексПараметра, I)
                    mИндексМинимальногоЗначения = I
                End If
            Next
        End If

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
