Friend Class ПровалN1ОтносительноУстановившегося
    Dim mИндексПараметра, mИндексТначальное, mИндексТконечное As Integer
    Dim mДлительностьТакта, mТначальное, mТконечное, mТдлительность As Double
    Dim mАначальное, mАконечное As Double
    Dim mDeltaA As Double
    Dim mВремяОсреднения As Double
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

    Public ReadOnly Property DeltaA() As Double
        Get
            Return mDeltaA
        End Get
    End Property

    Public Property ВремяОсреднения() As Double
        Get
            Return mВремяОсреднения
        End Get
        Set(ByVal Value As Double)
            mВремяОсреднения = Value
        End Set
    End Property

    Public Sub New(ByVal ИмяПараметра As String,
                   ByVal ЧастотаКадра As Short,
                   ByVal arrЗначенияПараметров(,) As Double,
                   ByVal myTypeList() As TypeSmallParameter,
                   ByVal Minimum As Double,
                   ByVal Maximum As Double)

        mИмяПараметра = ИмяПараметра
        marrЗначения = CType(arrЗначенияПараметров.Clone, Double(,))
        mmyTypeList = CType(myTypeList.Clone, TypeSmallParameter())
        mДлительностьТакта = 1 / ЧастотаКадра
        mТекстОшибки = "Параметр: " & mИмяПараметра & vbCrLf
        mВремяОсреднения = 2 'по умолчанию
        GraphMinimum = Minimum
        GraphMaximum = Maximum
    End Sub

    Public Sub Расчет()
        Dim I, J, N As Integer
        Dim параметрНайден, минимумНайден As Boolean
        Dim количество As Integer
        Dim минимальноеЗначение As Double = Double.MaxValue
        Dim максимальноеЗначение As Double = Double.MinValue
        Dim индексМаксимальногоЗначения, индексМинимальногоЗначения As Integer
        Dim скачокЗначение As Double = 0.2
        Dim скачокИндекс As Integer = CInt(0.2 / mДлительностьТакта) ' 0.2 секунды '2

        'находим индекс параметра
        For J = 1 To UBound(mmyTypeList)
            If mmyTypeList(J).NameParameter = mИмяПараметра AndAlso mmyTypeList(J).IsVisible Then
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

        N = mGraphMaximum
        'находим среднее за последние 2 сек
        For I = N - CInt(mВремяОсреднения / mДлительностьТакта) To N
            mАконечное += marrЗначения(mИндексПараметра, I)
            количество += 1
        Next I

        mАконечное /= количество

        'находим первый максимум 
        For I = mИндексТначальное To N
            'поиск максимального
            If marrЗначения(mИндексПараметра, I) > максимальноеЗначение Then
                максимальноеЗначение = marrЗначения(mИндексПараметра, I)
                индексМаксимальногоЗначения = I
            End If
        Next I

        If (индексМаксимальногоЗначения - mИндексТначальное) * mДлительностьТакта > 10 Then
            'если максимальное значение отстоит от mИндексТначальное более 10 сек то от него ищем локальный минимум

            '1 случай когда локальный максимум превышающего среднее зачение в пределах 10 сек до абсолютного максимума есть
            Dim ИндексПервогоПревышенияСреднего, ИндексПоследнегоПревышенияСреднего As Integer
            For I = mИндексТначальное To индексМаксимальногоЗначения
                If marrЗначения(mИндексПараметра, I) > mАконечное Then
                    ИндексПервогоПревышенияСреднего = I
                    Exit For
                End If
            Next I

            For I = индексМаксимальногоЗначения To mИндексТначальное Step -1
                If marrЗначения(mИндексПараметра, I) < mАконечное Then
                    ИндексПоследнегоПревышенияСреднего = I
                    Exit For
                End If
            Next I

            If ИндексПервогоПревышенияСреднего < ИндексПоследнегоПревышенияСреднего Then
                For I = ИндексПервогоПревышенияСреднего To ИндексПоследнегоПревышенияСреднего
                    'поиск минимального
                    If marrЗначения(mИндексПараметра, I) < минимальноеЗначение Then
                        минимальноеЗначение = marrЗначения(mИндексПараметра, I)
                        индексМинимальногоЗначения = I
                        минимумНайден = True
                    End If
                Next I
            End If

            'For I = mИндексМаксимальногоЗначения To mИндексТначальное Step -1
            '    If blnНайдемМеньшеСреднего = False AndAlso marrЗначения(mИндексПараметра, I) < mАконечное Then
            '        blnНайдемМеньшеСреднего = True
            '    End If
            '    If blnНайдемМеньшеСреднего = True Then
            '        'до первого превышения
            '        If marrЗначения(mИндексПараметра, I) > mАконечное Then
            '            blnМинимумНайден = True
            '            Exit For
            '        End If

            '        'поиск минимального
            '        If marrЗначения(mИндексПараметра, I) < mМинимальноеЗначение Then
            '            mМинимальноеЗначение = marrЗначения(mИндексПараметра, I)
            '            mИндексМинимальногоЗначения = I
            '        End If
            '    End If
            'Next I

            If минимумНайден = False Then
                минимальноеЗначение = 1.0E+38
                '2 случай когда локальный максимум превышающего среднее зачение в пределах 10 сек до абсолютного максимума нет
                For I = mИндексТначальное To индексМаксимальногоЗначения - скачокИндекс * 2
                    'поиск локального минимального
                    If (marrЗначения(mИндексПараметра, I) - marrЗначения(mИндексПараметра, I + скачокИндекс) > скачокЗначение) AndAlso (marrЗначения(mИндексПараметра, I + скачокИндекс * 2) - marrЗначения(mИндексПараметра, I + скачокИндекс) > скачокЗначение) Then
                        If I - скачокИндекс > 0 Then
                            минимальноеЗначение = marrЗначения(mИндексПараметра, I - скачокИндекс)
                            индексМинимальногоЗначения = I + скачокИндекс
                            минимумНайден = True
                            Exit For
                        End If
                    End If
                Next I
            End If

            If минимумНайден = False Then
                For I = индексМаксимальногоЗначения To mИндексТначальное Step -1
                    'поиск минимального
                    If marrЗначения(mИндексПараметра, I) < минимальноеЗначение Then
                        минимальноеЗначение = marrЗначения(mИндексПараметра, I)
                        индексМинимальногоЗначения = I
                    End If
                Next I
            End If
        Else
            'находим минимум начиная с mИндексМаксимальногоЗначения
            For I = индексМаксимальногоЗначения To N
                'поиск минимального
                If marrЗначения(mИндексПараметра, I) < минимальноеЗначение Then
                    минимальноеЗначение = marrЗначения(mИндексПараметра, I)
                    индексМинимальногоЗначения = I
                End If
            Next I
        End If

        mИндексТконечное = N
        mИндексТначальное = индексМинимальногоЗначения
        mАначальное = marrЗначения(mИндексПараметра, mИндексТначальное)
        mТначальное = mИндексТначальное * mДлительностьТакта
        mТконечное = mИндексТконечное * mДлительностьТакта
        mТдлительность = mТконечное - mТначальное
        mDeltaA = mАконечное - mАначальное

        If (mИндексТначальное = mИндексТконечное) AndAlso Not (mИндексТначальное = mGraphMinimum And mИндексТконечное = mGraphMinimum) Then
            mОшибка = True
            mТекстОшибки += "Тначальное и Тконечное равны" & vbCrLf
        End If
    End Sub
End Class
