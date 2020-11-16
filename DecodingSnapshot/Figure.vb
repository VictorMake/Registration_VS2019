''' <summary>
''' Базовый класс примитива расчёта одного параметра
''' </summary>
Friend MustInherit Class Figure
    Public Property GraphMinimum() As Integer
        Get
            Return mGraphMinimum
        End Get
        Set(ByVal Value As Integer)
            mGraphMinimum = Value
        End Set
    End Property

    Public Property GraphMaximum() As Integer
        Get
            Return mGraphMaximum
        End Get
        Set(ByVal Value As Integer)
            mGraphMaximum = Value
        End Set
    End Property

    Public Property IndexParameter() As Integer
        Get
            Return mIndexParameter
        End Get
        Set(ByVal Value As Integer)
            mIndexParameter = Value
        End Set
    End Property

    ''' <summary>
    ''' Индекс Т начальное
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IndexTstart() As Integer
        Get
            Return mIndexTstart
        End Get
    End Property

    ''' <summary>
    ''' Индекс Т конечное
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IndexTstop() As Integer
        Get
            Return mIndexTstop
        End Get
    End Property

    ''' <summary>
    ''' Длительность Такта
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ClockPeriod() As Double

    ''' <summary>
    ''' Т начальное
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Tstart() As Double
        Get
            Return mTstart
        End Get
    End Property

    ''' <summary>
    ''' T конечное
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Tstop() As Double
        Get
            Return mTstop
        End Get
    End Property

    ''' <summary>
    ''' Т длительность
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TimeDuration() As Double
        Get
            Return mTimeDuration
        End Get
    End Property

    ''' <summary>
    ''' Максимальное Значение
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MaxValue() As Double
        Get
            Return mMaxValue
        End Get
    End Property

    ''' <summary>
    ''' Минимальное Значение
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MinValue() As Double
        Get
            Return mMinValue
        End Get
    End Property

    ''' <summary>
    ''' Индекс Максимального Значения
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IndexMaxValue() As Integer
        Get
            Return mIndexMaxValue
        End Get
    End Property

    ''' <summary>
    ''' Индекс Минимального Значения
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IndexMinValue() As Integer
        Get
            Return mIndexMinValue
        End Get
    End Property

    ''' <summary>
    ''' Т Максимального Значения
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TimeMaxValue() As Double
        Get
            Return IndexMaxValue * ClockPeriod
        End Get
    End Property

    ''' <summary>
    ''' Т Минимального Значения
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TimeMinValue() As Double
        Get
            Return IndexMinValue * ClockPeriod
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

    Public Property Astart() As Double
    Public Property Astop() As Double
    Public Property IndexStart() As Integer

    Protected Friend mIndexParameter, mIndexTstart, mIndexTstop As Integer
    Protected Friend mTstart, mTstop, mTimeDuration As Double
    Protected Friend mMaxValue, mMinValue As Double
    Protected Friend mIndexMaxValue, mIndexMinValue As Integer
    Protected Friend mIsErrors As Boolean
    Protected Friend ReadOnly nameParameter As String
    Protected Friend mErrorsMessage As String
    Protected Friend ReadOnly MeasuredValues As Double(,)
    Protected Friend ReadOnly Parameters As TypeSmallParameter()
    Protected Friend mGraphMinimum, mGraphMaximum As Integer

    Public Sub New(inNameParameter As String,
                   frequency As Integer,
                   ByRef inMeasuredValues As Double(,),
                   ByRef inTypeSmallParameter As TypeSmallParameter(),
                   minimum As Double, ByVal maximum As Double)

        nameParameter = inNameParameter
        MeasuredValues = inMeasuredValues
        Parameters = inTypeSmallParameter
        ClockPeriod = 1 / frequency
        mErrorsMessage = "Параметр: " & nameParameter & vbCrLf
        mMinValue = Double.MaxValue
        mMaxValue = Double.MinValue
        GraphMinimum = CInt(minimum)
        GraphMaximum = CInt(maximum)
    End Sub

    ''' <summary>
    ''' Расшифровать
    ''' </summary>
    Public MustOverride Sub Calculation()
End Class