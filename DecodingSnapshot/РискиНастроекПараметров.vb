﻿Friend Class РискиНастроекПараметров
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное As Double
    Dim mОшибка As Boolean
    Dim mИмяПараметра, mТекстОшибки As String
    Dim marrПараметрыЛиста() As TypeSmallParameter
    Dim mGraphMinimum, mGraphMaximum As Short

    Public Sub New(ByVal ИмяПараметра As String,
                   ByVal ЧастотаКадра As Short,
                   ByVal arrПараметрыЛиста() As TypeSmallParameter,
                   ByVal Minimum As Double, ByVal Maximum As Double)

        mИмяПараметра = ИмяПараметра
        marrПараметрыЛиста = CType(arrПараметрыЛиста.Clone, TypeSmallParameter())
        mДлительностьТакта = 1 / ЧастотаКадра
        mТекстОшибки = "Параметр: " & mИмяПараметра & vbCrLf
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

    Public ReadOnly Property ДлительностьТакта() As Double
        Get
            Return mДлительностьТакта
        End Get
    End Property

    Public WriteOnly Property ИндексТначальное() As Short
        Set(ByVal Value As Short)
            mИндексТначальное = Value
        End Set
    End Property

    Public WriteOnly Property ИндексТконечное() As Short
        Set(ByVal Value As Short)
            mИндексТконечное = Value
        End Set
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

    Public Sub Расчет()
        Dim J As Integer
        Dim параметрНайден As Boolean

        'находим индекс параметра
        For J = 1 To UBound(marrПараметрыЛиста)
            If marrПараметрыЛиста(J).NameParameter = mИмяПараметра AndAlso marrПараметрыЛиста(J).IsVisible Then
                mИндексПараметра = J - 1
                параметрНайден = True
                Exit For
            End If
        Next J

        If Not параметрНайден Then
            mОшибка = True
            mТекстОшибки += "Параметр " & mИмяПараметра & " не найден" & vbCrLf
            Exit Sub
        End If

        mИндексТконечное = mGraphMaximum
        mИндексТначальное = CInt(mGraphMaximum - 5 / mДлительностьТакта) 'длина риски 5 сек
        mТначальное = mИндексТначальное * mДлительностьТакта
        mТконечное = mИндексТконечное * mДлительностьТакта
    End Sub
End Class