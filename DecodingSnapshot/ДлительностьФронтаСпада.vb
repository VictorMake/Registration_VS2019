﻿Friend Class ДлительностьФронтаСпада
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
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
        GraphMinimum = Minimum '* ЧастотаКадра
        GraphMaximum = Maximum '* ЧастотаКадра
    End Sub

    Public Sub Расчет()
        Dim I, J, N, стартовыйИндекс As Integer
        Dim параметрНайден, индексТначальноеНайден, индексТконечноеНайден As Boolean

        'проверка на корректность введенныы параметров
        If mАначальное = mАконечное Then
            mIsErrors = True
            mErrorsMessage += "Не введены Аначальное или Аконечное" & vbCrLf
            Exit Sub
        End If

        'находим индекс параметра
        For J = 1 To UBound(mmyTypeList)
            If mmyTypeList(J).NameParameter = mИмяПараметра AndAlso mmyTypeList(J).IsVisible Then
                mИндексПараметра = J - 1
                параметрНайден = True
                Exit For
            End If
        Next J

        If Not параметрНайден Then
            mIsErrors = True
            mErrorsMessage += "Параметр " & mИмяПараметра & " не найден" & vbCrLf
            Exit Sub
        End If

        'N = UBound(marrЗначения, 2)
        N = mGraphMaximum

        If mАначальное < mАконечное Then
            'For I = mGraphMinimum To N
            '    If marrЗначения(mИндексПараметра, I) > mАначальное Then
            '        СтартовыйИндекс = I '- 1
            '        Exit For
            '    End If
            'Next I


            'если фронт
            'ищем первое значение которое меньше mАначальное
            For I = mGraphMinimum To N
                If marrЗначения(mИндексПараметра, I) < mАначальное Then
                    стартовыйИндекс = I
                    Exit For
                End If
            Next I

            For I = стартовыйИндекс To N
                If marrЗначения(mИндексПараметра, I) > mАначальное Then
                    mИндексТначальное = I - 1
                    индексТначальноеНайден = True
                    Exit For
                End If
            Next I

            If mИндексТначальное < 0 Then mИндексТначальное = 0

            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) > mАконечное Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next I
        Else
            'если спад
            'ищем первое значение которое больше mАначальное
            For I = mGraphMinimum To N
                If marrЗначения(mИндексПараметра, I) > mАначальное Then
                    стартовыйИндекс = I
                    Exit For
                End If
            Next I

            For I = стартовыйИндекс To N
                If marrЗначения(mИндексПараметра, I) < mАначальное Then
                    mИндексТначальное = I
                    индексТначальноеНайден = True
                    Exit For
                End If
            Next I

            For I = mИндексТначальное To N
                If marrЗначения(mИндексПараметра, I) < mАконечное Then
                    mИндексТконечное = I
                    индексТконечноеНайден = True
                    Exit For
                End If
            Next I
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
