Imports System.Collections.Generic
Imports NationalInstruments.UI.WindowsForms
Imports System.ComponentModel
Imports Registration.FormMain

Friend Class FormGraphControl
    Implements IUpdateSelectiveControls

#Region "Interface IBaseClassIndicator"

    'Friend Class BaseClassIndicator
    '    Friend WithEvents mControlBase As NationalInstruments.UI.WindowsForms.ControlBase
    '    Dim mИмя As String
    '    Dim mLocation As System.Drawing.Point
    '    Dim mSize As System.Drawing.Size
    '    Dim mValue As Double
    '    Dim mParameterName As String

    '    'Public Sub New(ByVal Имя As String, ByVal Родитель As System.Windows.Forms.Form)
    '    '    MyBase.New()
    '    '    mLocation = New System.Drawing.Point(0, 0)
    '    '    mSize = New System.Drawing.Size(100, 100)
    '    '    MyClass.Имя = Имя
    '    '    'mControlBase = New NationalInstruments.UI.WindowsForms.ControlBase
    '    '    mControlBase.Name = mИмя

    '    '    'CType(mControlBase, System.ComponentModel.ISupportInitialize).BeginInit()
    '    '    'inParentForm.Controls.Add(mControlBase)
    '    '    'CType(mControlBase, System.ComponentModel.ISupportInitialize).EndInit()
    '    '    'ContinueCreate()
    '    'End Sub

    '    Public Overridable ReadOnly Property Экземпляр() As NationalInstruments.UI.WindowsForms.ControlBase 'WindowsForms.ControlBase
    '        Get
    '            Return mControlBase
    '        End Get
    '    End Property

    '    Public Overridable Property Имя() As String
    '        Get
    '            Return mИмя
    '        End Get
    '        Set(ByVal Value As String)
    '            mИмя = Value
    '        End Set
    '    End Property

    '    Public Overridable Property Точка() As System.Drawing.Point
    '        Get
    '            Return mLocation
    '        End Get
    '        Set(ByVal Value As System.Drawing.Point)
    '            mLocation = Value
    '        End Set
    '    End Property

    '    Public Overridable Property Размер() As System.Drawing.Size
    '        Get
    '            Return mSize
    '        End Get
    '        Set(ByVal Value As System.Drawing.Size)
    '            mSize = Value
    '        End Set
    '    End Property

    '    Public Overridable Property Значение() As Double
    '        Get
    '            Return mValue
    '        End Get
    '        Set(ByVal Value As Double)
    '            mValue = Value
    '        End Set
    '    End Property

    '    Public Overridable Property ИмяПараметра() As String
    '        Get
    '            Return mParameterName
    '        End Get
    '        Set(ByVal Value As String)
    '            mParameterName = Value
    '        End Set
    '    End Property

    '    Private Sub ContinueCreate()
    '    End Sub

    '    Public Overridable Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer)
    '    End Sub

    '    Public Overridable Property ДопускМинимум As Double
    '    Public Overridable Property ДопускМаксимум As Double
    '    Public Overridable Property АварийноеЗначениеМин As Double
    '    Public Overridable Property АварийноеЗначениеМакс As Double
    '    Public Overridable Property Range As NationalInstruments.UI.Range

    'End Class

    Friend Interface IBaseClassIndicator
        'ReadOnly Property Экземпляр() As NationalInstruments.UI.WindowsForms.ControlBase
        Property Name() As String
        Property LocationIndicator() As Point
        Property SizeIndicator() As Size
        Property Value() As Double
        Property ParameterName() As String
        Property NumberParameter() As Integer
        Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer)
        Property LimitMin As Double
        Property LimitMax As Double
        Property AlarmMin As Double
        Property AlarmMax As Double
        Property Range As Range
    End Interface

    ''' <summary>
    ''' Проверка нахождения значения параметра в диапазоне
    ''' и изменение цвета цифрового индикатора.
    ''' Перегруженная совместная версия экземпляра типа
    ''' </summary>
    ''' <param name="baseIndicator"></param>
    ''' <param name="NiUiWindowsFormsRadialNumericPointer"></param>
    ''' <param name="Value"></param>
    Shared Sub CheckValueInRange(baseIndicator As IBaseClassIndicator,
                                  NiUiWindowsFormsRadialNumericPointer As RadialNumericPointer,
                                  Value As Double)

        Dim isAlarm As Boolean ' для аварийных индикаторов

        With NiUiWindowsFormsRadialNumericPointer
            ' 1. Заданы аварийные пределы
            If (baseIndicator.AlarmMin <> 0 OrElse baseIndicator.AlarmMax <> 0) Then
                isAlarm = False

                If Value > baseIndicator.AlarmMax Then
                    isAlarm = True
                    If .CaptionBackColor <> Color.Red Then
                        .CaptionBackColor = Color.Red
                        .CaptionForeColor = Color.Black
                    End If
                ElseIf Value < baseIndicator.AlarmMin Then
                    isAlarm = True
                    If .CaptionBackColor <> Color.Orange Then
                        .CaptionBackColor = Color.Orange
                        .CaptionForeColor = Color.Black
                    End If
                End If

                If isAlarm = False Then
                    If .CaptionBackColor <> Color.Navy Then
                        .CaptionBackColor = Color.Navy
                        .CaptionForeColor = Color.Lime
                    End If
                End If

                Exit Sub
            End If

            ' 2. Проверка на выход из рабочего диапазона
            If Value < baseIndicator.LimitMin OrElse Value > baseIndicator.LimitMax Then
                If .CaptionBackColor <> Color.Yellow Then
                    .CaptionBackColor = Color.Yellow
                    .CaptionForeColor = Color.Red
                End If

                Exit Sub
            End If

            ' 3. Не превысил аварийные допуски и находится в рабочем диапазоне
            If .CaptionBackColor <> Color.Navy Then
                .CaptionBackColor = Color.Navy
                .CaptionForeColor = Color.Lime
            End If
        End With
    End Sub

    ''' <summary>
    ''' Проверка нахождения значения параметра в диапазоне
    ''' и изменение цвета цифрового индикатора.
    ''' Перегруженная совместная версия экземпляра типа
    ''' </summary>
    ''' <param name="baseIndicator"></param>
    ''' <param name="NiUiWindowsFormsLinearNumericPointer"></param>
    ''' <param name="Value"></param>
    Shared Sub CheckValueInRange(baseIndicator As IBaseClassIndicator,
                                  NiUiWindowsFormsLinearNumericPointer As LinearNumericPointer,
                                  Value As Double)

        Dim isAlarm As Boolean ' для аварийных индикаторов

        With NiUiWindowsFormsLinearNumericPointer
            ' 1. Заданы аварийные пределы
            If (baseIndicator.AlarmMin <> 0 OrElse baseIndicator.AlarmMax <> 0) Then
                isAlarm = False

                If Value > baseIndicator.AlarmMax Then
                    isAlarm = True
                    If .CaptionBackColor <> Color.Red Then
                        .CaptionBackColor = Color.Red
                        .CaptionForeColor = Color.Black
                    End If
                ElseIf Value < baseIndicator.AlarmMin Then
                    isAlarm = True
                    If .CaptionBackColor <> Color.Orange Then
                        .CaptionBackColor = Color.Orange
                        .CaptionForeColor = Color.Black
                    End If
                End If

                If isAlarm = False Then
                    If .CaptionBackColor <> Color.Navy Then
                        .CaptionBackColor = Color.Navy
                        .CaptionForeColor = Color.Lime
                    End If
                End If

                Exit Sub
            End If

            ' 2. Проверка на выход из рабочего диапазона
            If Value < baseIndicator.LimitMin OrElse Value > baseIndicator.LimitMax Then
                If .CaptionBackColor <> Color.Yellow Then
                    .CaptionBackColor = Color.Yellow
                    .CaptionForeColor = Color.Red
                End If

                Exit Sub
            End If

            ' 3. Не превысил аварийные допуски и находится в рабочем диапазоне
            If .CaptionBackColor <> Color.Navy Then
                .CaptionBackColor = Color.Navy
                .CaptionForeColor = Color.Lime
            End If
        End With
    End Sub

    ''' <summary>
    ''' Класс Давление
    ''' </summary>
    Friend Class PressureIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mGauge As Gauge
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision1 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mGauge = New Gauge() With {.Name = Name}

            CType(mGauge, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mGauge)
            CType(mGauge, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр As WindowsForms.Gauge 
        '    Get
        '        Return ДавлениеKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mGauge.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mGauge.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mGauge.Value = Value
                mGauge.Caption = $"{mParameterName}: {Format(Value, "F")}"
                'ScaleCustomDivision1.Value = Value ' не надо
                CheckValueInRange(Me, mGauge, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                'ДавлениеKnob.Caption = Value
                ScaleCustomDivision1.Text = mParameterName
                ScaleCustomDivision1.Value = (mGauge.Range.Minimum + mGauge.Range.Maximum) / 2 ' - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mGauge
                '.AutoDivisionSpacing = False
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision1.DisplayStyle = CustomDivisionDisplayStyle.ShowText
                'ScaleCustomDivision1.LabelFont = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision1.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision1.LabelForeColor = Color.Yellow
                ScaleCustomDivision1.Text = "Имя"
                'ScaleCustomDivision1.TickVisible = False
                ScaleCustomDivision1.TickColor = Color.Olive
                ScaleCustomDivision1.TickLength = 25.0!
                ScaleCustomDivision1.Value = 25.27
                ScaleCustomDivision1.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision1})
                .DialColor = Color.Olive
                .Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .ForeColor = Color.White
                .Location = mLocation
                .MajorDivisions.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .Name = "Давление"
                .PointerColor = Color.Red
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .Size = mSize
                .TabIndex = 1
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mGauge.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mGauge.Range
            End Get
            Set(ByVal Value As Range)
                mGauge.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Обороты
    ''' </summary>
    Friend Class RotationIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mKnob As Knob
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision6 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mKnob = New Knob With {
                .Name = Name
            }

            CType(mKnob, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mKnob)
            CType(mKnob, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Knob
        '    Get
        '        Return ОборотыKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mKnob.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mKnob.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mKnob.Value = Value
                ScaleCustomDivision6.Value = Value
                mKnob.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mKnob, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mKnob.Caption = Value
                'ScaleCustomDivision6.Text = mParameterName
                'ScaleCustomDivision6.Value = (ОборотыKnob.Range.Minimum + ОборотыKnob.Range.Maximum) / 2 - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mKnob
                .AutoDivisionSpacing = False
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                '.Border = NationalInstruments.UI.Border.Sunken
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision6.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision6.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision6.LabelForeColor = Color.Yellow
                ScaleCustomDivision6.LineWidth = 2.0!
                ScaleCustomDivision6.Text = "Имя"
                ScaleCustomDivision6.Value = 25.27
                ScaleCustomDivision6.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision6})
                .InteractionMode = RadialNumericPointerInteractionModes.Indicator
                .KnobStyle = KnobStyle.RaisedWithThinNeedle3D
                '.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThumb
                .Location = mLocation
                .MajorDivisions.LabelForeColor = Color.White
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .MinorDivisions.TickColor = Color.White
                .Name = "Обороты"
                .PointerColor = Color.Blue
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .Size = mSize
                .TabIndex = 44
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mKnob.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mKnob.Range
            End Get
            Set(ByVal Value As Range)
                mKnob.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Ток
    ''' </summary>
    Friend Class CurrentIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mMeter As Meter
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision2 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mMeter = New Meter With {
                .Name = Name
            }

            CType(mMeter, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mMeter)
            CType(mMeter, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Meter
        '    Get
        '        Return ТокKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mMeter.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mMeter.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mMeter.Value = Value
                ScaleCustomDivision2.Value = Value
                mMeter.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mMeter, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mMeter.Caption = Value
                'ScaleCustomDivision2.Text = mParameterName
                'ScaleCustomDivision2.Value = (ТокKnob.Range.Minimum + ТокKnob.Range.Maximum) / 2 - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mMeter
                .AutoDivisionSpacing = False
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision2.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision2.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision2.LabelForeColor = Color.Yellow
                ScaleCustomDivision2.LineWidth = 2.0!
                ScaleCustomDivision2.Text = "Имя"
                ScaleCustomDivision2.TickLength = 7.0!
                ScaleCustomDivision2.Value = 25.27
                ScaleCustomDivision2.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision2})
                .DialColor = Color.Silver
                .Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .ForeColor = Color.White
                .Location = mLocation
                .MajorDivisions.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                .MajorDivisions.LabelForeColor = Color.White
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .MinorDivisions.TickColor = Color.White
                .MinorDivisions.TickLength = 5.0!
                .Name = "Ток"
                .PointerColor = Color.Red
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .ScaleArc = New Arc(190.0!, -200.0!)
                .Size = mSize
                .SpindleColor = Color.DimGray
                .TabIndex = 38
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mMeter.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mMeter.Range
            End Get
            Set(ByVal Value As Range)
                mMeter.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Вибрация
    ''' </summary>
    Friend Class VibrationIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mSlide As Slide
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision7 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mSlide = New Slide With {
                .Name = Name
            }

            CType(mSlide, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mSlide)
            CType(mSlide, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Slide
        '    Get
        '        Return ВибрацияKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mSlide.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mSlide.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mSlide.Value = Value
                ScaleCustomDivision7.Value = Value
                mSlide.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mSlide, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mSlide.Caption = Value
                'ScaleCustomDivision7.Text = mParameterName
                'ScaleCustomDivision7.Value = (ВибрацияKnob.Range.Minimum + ВибрацияKnob.Range.Maximum) / 2 - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mSlide
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision7.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision7.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision7.LabelForeColor = Color.Yellow
                ScaleCustomDivision7.LineWidth = 2.0!
                ScaleCustomDivision7.Text = "Имя"
                ScaleCustomDivision7.TickLength = 25.0!
                ScaleCustomDivision7.Value = 25.27
                ScaleCustomDivision7.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision7})
                .FillBackColor = Color.DarkGray
                .FillColor = Color.Gold
                .FillStyle = FillStyle.HorizontalGradient
                .Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .ForeColor = Color.White
                .InteractionMode = LinearNumericPointerInteractionModes.Indicator
                .Location = mLocation
                .MajorDivisions.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .MinorDivisions.TickColor = Color.White
                .Name = "Деления"
                .PointerColor = Color.Red
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .ScaleBaseLineColor = Color.White
                .ScalePosition = NumericScalePosition.Top
                .Size = mSize
                .SlideStyle = SlideStyle.SunkenWithGrip
                .TabIndex = 45
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mSlide.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mSlide.Range
            End Get
            Set(ByVal Value As Range)
                mSlide.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Температура
    ''' </summary>
    Friend Class TemperatureIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mThermometer As Thermometer
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision5 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mThermometer = New Thermometer With {
                .Name = Name
            }

            CType(mThermometer, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mThermometer)
            CType(mThermometer, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Thermometer
        '    Get
        '        Return ТемператураKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mThermometer.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mThermometer.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mThermometer.Value = Value
                ScaleCustomDivision5.Value = Value
                mThermometer.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mThermometer, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mThermometer.Caption = Value
                'ScaleCustomDivision5.Text = mParameterName
                'ScaleCustomDivision5.Value = ТемператураKnob.Range.Maximum - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mThermometer
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision5.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision5.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision5.LabelForeColor = Color.Yellow
                ScaleCustomDivision5.LineWidth = 2.0!
                ScaleCustomDivision5.Text = "Имя"
                ScaleCustomDivision5.TickLength = 25.0!
                ScaleCustomDivision5.Value = 25.27
                ScaleCustomDivision5.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision5})
                .ForeColor = Color.White
                .Location = mLocation
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .Name = "Температура"
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                '.ThermometerStyle = NationalInstruments.UI.ThermometerStyle.Flat
                .Size = mSize
                .TabIndex = 43
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mThermometer.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mThermometer.Range
            End Get
            Set(ByVal Value As Range)
                mThermometer.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Столбы
    ''' </summary>
    Friend Class TankIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mTank As Tank
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision3 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mTank = New Tank With {
                .Name = Name
            }

            CType(mTank, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mTank)
            CType(mTank, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Tank
        '    Get
        '        Return СтолбыKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mTank.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mTank.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mTank.Value = Value
                ScaleCustomDivision3.Value = Value
                mTank.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mTank, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mTank.Caption = Value
                'ScaleCustomDivision3.Text = mParameterName
                'ScaleCustomDivision3.Value = СтолбыKnob.Range.Maximum - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mTank
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision3.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision3.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision3.LabelForeColor = Color.Yellow
                ScaleCustomDivision3.LineWidth = 2.0!
                ScaleCustomDivision3.Text = "Имя"
                ScaleCustomDivision3.TickLength = 25.0!
                ScaleCustomDivision3.Value = 25.27
                ScaleCustomDivision3.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")

                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision3})
                .FillColor = Color.Blue
                .FillStyle = FillStyle.HorizontalGradient
                .Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .ForeColor = Color.White
                .Location = mLocation
                .MajorDivisions.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                .MajorDivisions.LabelForeColor = Color.White
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .Name = "Барометр"
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .Size = mSize
                .TabIndex = 41
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mTank.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mTank.Range
            End Get
            Set(ByVal Value As Range)
                mTank.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс Расход
    ''' </summary>
    Friend Class MassFlowIndicator
        Implements IBaseClassIndicator

        Friend WithEvents mSlide As Slide
        Dim mLocation As Point
        Dim mSize As Size
        Dim mValue As Double
        Dim mParameterName As String
        Dim ScaleCustomDivision4 As ScaleCustomDivision = New ScaleCustomDivision

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mSlide = New Slide With {
                .Name = Name
            }

            CType(mSlide, ISupportInitialize).BeginInit()
            inParentForm.Controls.Add(mSlide)
            CType(mSlide, ISupportInitialize).EndInit()
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As WindowsForms.Slide
        '    Get
        '        Return РасходKnob
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mSlide.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mSlide.Size = mSize
            End Set
        End Property

        Public Property Value() As Double Implements IBaseClassIndicator.Value
            Get
                Return mValue
            End Get
            Set(ByVal Value As Double)
                mValue = Value
                mSlide.Value = Value
                ScaleCustomDivision4.Value = Value
                mSlide.Caption = $"{mParameterName}: {Format(Value, "F")}"
                CheckValueInRange(Me, mSlide, Value)
            End Set
        End Property

        Public Property ParameterName() As String Implements IBaseClassIndicator.ParameterName
            Get
                Return mParameterName
            End Get
            Set(ByVal Value As String)
                mParameterName = Value
                mSlide.Caption = Value
                'ScaleCustomDivision4.Text = mParameterName
                'ScaleCustomDivision4.Value = (РасходKnob.Range.Minimum + РасходKnob.Range.Maximum) / 2 - 1
            End Set
        End Property

        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mSlide
                .BackColor = Color.DimGray
                .Border = Border.ThinFrame3D
                .Caption = "100.0"
                .CaptionBackColor = Color.Navy
                .CaptionFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .CaptionForeColor = Color.Lime

                ScaleCustomDivision4.DisplayStyle = CustomDivisionDisplayStyle.ShowValue
                ScaleCustomDivision4.LabelFont = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                ScaleCustomDivision4.LabelForeColor = Color.Yellow
                ScaleCustomDivision4.LineWidth = 2.0!
                ScaleCustomDivision4.Text = "Имя"
                ScaleCustomDivision4.TickLength = 25.0!
                ScaleCustomDivision4.Value = 25.27
                ScaleCustomDivision4.LabelFormat = New FormatString(FormatStringMode.Numeric, "F1")


                .CustomDivisions.AddRange(New ScaleCustomDivision() {ScaleCustomDivision4})
                .FillColor = Color.Gold
                .FillStyle = FillStyle.VerticalGradient
                .Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
                .ForeColor = Color.White
                .InteractionMode = LinearNumericPointerInteractionModes.Indicator
                .Location = mLocation
                .MajorDivisions.LabelFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
                .MajorDivisions.LineWidth = 2.0!
                .MajorDivisions.TickColor = Color.Yellow
                .MajorDivisions.TickLength = 7.0!
                .MinorDivisions.TickColor = Color.White
                .Name = "Расход"
                .PointerColor = Color.Red
                .OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                .Range = New Range(0, 100)
                .ScaleBaseLineColor = Color.White
                .ScalePosition = NumericScalePosition.Top
                .Size = mSize
                .TabIndex = 42
                .Value = 50
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mSlide.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range
            Get
                Return mSlide.Range
            End Get
            Set(ByVal Value As Range)
                mSlide.Range = Value
            End Set
        End Property

        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
    End Class

    ''' <summary>
    ''' Класс GroupBox
    ''' </summary>
    Friend Class GroupBoxIndicator
        Implements IBaseClassIndicator

        Friend mGroupBox As GroupBox
        Dim mLocation As Point
        Dim mSize As Size

        Public Sub New(ByVal inName As String, ByVal inParentForm As Form)
            MyBase.New()
            mLocation = New Point(0, 0)
            mSize = New Size(100, 100)
            Name = inName
            mGroupBox = New GroupBox

            inParentForm.Controls.Add(mGroupBox)
            ContinueCreate()
        End Sub

        'Public ReadOnly Property Экземпляр As NationalInstruments.UI.WindowsForms.ControlBase Implements IBaseClassIndicator.Экземпляр
        '    'Public ReadOnly Property Экземпляр() As GroupBox
        '    Get
        '        Return mGroupBox
        '    End Get
        'End Property

        Public Property Name() As String Implements IBaseClassIndicator.Name
        Public Property NumberParameter As Integer Implements IBaseClassIndicator.NumberParameter

        Public Property LocationIndicator() As Point Implements IBaseClassIndicator.LocationIndicator
            Get
                Return mLocation
            End Get
            Set(ByVal Value As Point)
                mLocation = Value
                mGroupBox.Location = mLocation
            End Set
        End Property

        Public Property SizeIndicator() As Size Implements IBaseClassIndicator.SizeIndicator
            Get
                Return mSize
            End Get
            Set(ByVal Value As Size)
                mSize = Value
                mGroupBox.Size = mSize
            End Set
        End Property
        ''' <summary>
        ''' Продолжить Создавать
        ''' </summary>
        Private Sub ContinueCreate()
            With mGroupBox
                .Visible = True
                .Text = ""
                .ForeColor = Color.Black
                .BackColor = Color.DimGray 'Color.Black
                .FlatStyle = FlatStyle.Flat
                .Location = mLocation
                .Size = mSize
            End With
        End Sub

        Public Sub SetBounds(x As Integer, y As Integer, width As Integer, height As Integer) Implements IBaseClassIndicator.SetBounds
            mGroupBox.SetBounds(x, y, width, height)
        End Sub

        Public Property Range As Range Implements IBaseClassIndicator.Range

        Public Property AlarmMax As Double Implements IBaseClassIndicator.AlarmMax
        Public Property AlarmMin As Double Implements IBaseClassIndicator.AlarmMin
        Public Property LimitMax As Double Implements IBaseClassIndicator.LimitMax
        Public Property LimitMin As Double Implements IBaseClassIndicator.LimitMin

        Public Property Value As Double Implements IBaseClassIndicator.Value
        Public Property ParameterName As String Implements IBaseClassIndicator.ParameterName
    End Class

#End Region

    Private Structure LocationSize
        Dim Left As Single
        Dim Top As Single
        Dim Width As Single
        Dim Height As Single
    End Structure

    Private parentFormRegistration As FormRegistrationBase
    Private indicatorControls As New List(Of IBaseClassIndicator)
    Private countControl As Integer
    Private arrLocationSize(LimitControl) As FormGraphControl.LocationSize
    Private Const EmptyName As String = "Пусто"

    Public Sub New(ByVal inParentForm As FormRegistrationBase)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
        Me.parentFormRegistration = inParentForm
    End Sub

    Private Sub FormSelectiveControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        PopulateSelectiveControl()
        Left = CInt(CSng(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlLeft", CStr(0))))
        Top = CInt(CSng(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlTop", CStr(0))))
        Width = CInt(CSng(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlWidth", CStr(640))))
        Height = CInt(CSng(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlHeight", CStr(480))))

        FormSelectiveControl_Resize(Me, New EventArgs)
        'CollectionForms.Add(Text, Me)
        parentFormRegistration.IsShowGraphControl = True
    End Sub

    Private Sub FormSelectiveControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If IsHandleCreated Then
            Dim J, I, K As Integer
            Dim WidthDivHeight As Single
            Dim mWidth As Single = ClientRectangle.Width
            Dim mHeight As Single = ClientRectangle.Height

            WidthDivHeight = mWidth / mHeight
            If WidthDivHeight < 0.2 Then
                arrLocationSize(1).Left = 0
                arrLocationSize(1).Top = 0
                K = 1

                For J = 2 To countControl - 1
                    arrLocationSize(J).Left = 0
                    arrLocationSize(J).Top = mHeight - mHeight * (countControl - K) / countControl
                    K += 1
                Next

                arrLocationSize(countControl).Left = 0
                arrLocationSize(countControl).Top = mHeight - mHeight / countControl

                For I = 1 To countControl
                    arrLocationSize(I).Height = mHeight / countControl
                    arrLocationSize(I).Width = mWidth
                Next
            ElseIf WidthDivHeight >= 0.2 And WidthDivHeight < 0.4 Then
                Select Case countControl
                    Case 4
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth / 2
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = 0
                        arrLocationSize(3).Top = mHeight - mHeight / 2
                        arrLocationSize(4).Left = mWidth - mWidth / 2
                        arrLocationSize(4).Top = mHeight - mHeight / 2

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 2
                            arrLocationSize(I).Width = mWidth / 2
                        Next
                        Exit Select
                    Case 9
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = 0
                        arrLocationSize(2).Top = mHeight - mHeight * 4 / 5
                        arrLocationSize(3).Left = 0
                        arrLocationSize(3).Top = mHeight - mHeight * 3 / 5
                        arrLocationSize(4).Left = 0
                        arrLocationSize(4).Top = mHeight - mHeight * 2 / 5
                        arrLocationSize(5).Left = 0
                        arrLocationSize(5).Top = mHeight - mHeight * 1 / 5

                        arrLocationSize(6).Left = mWidth / 2
                        arrLocationSize(6).Top = 0
                        arrLocationSize(7).Left = mWidth / 2
                        arrLocationSize(7).Top = mHeight - mHeight * 4 / 5
                        arrLocationSize(8).Left = mWidth / 2
                        arrLocationSize(8).Top = mHeight - mHeight * 3 / 5
                        arrLocationSize(9).Left = mWidth / 2
                        arrLocationSize(9).Top = mHeight - mHeight * 2 / 5

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 5
                            arrLocationSize(I).Width = mWidth / 2
                        Next
                        Exit Select
                    Case 16
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = 0
                        arrLocationSize(2).Top = mHeight - mHeight * 7 / 8
                        arrLocationSize(3).Left = 0
                        arrLocationSize(3).Top = mHeight - mHeight * 6 / 8
                        arrLocationSize(4).Left = 0
                        arrLocationSize(4).Top = mHeight - mHeight * 5 / 8
                        arrLocationSize(5).Left = 0
                        arrLocationSize(5).Top = mHeight - mHeight * 4 / 8
                        arrLocationSize(6).Left = 0
                        arrLocationSize(6).Top = mHeight - mHeight * 3 / 8
                        arrLocationSize(7).Left = 0
                        arrLocationSize(7).Top = mHeight - mHeight * 2 / 8
                        arrLocationSize(8).Left = 0
                        arrLocationSize(8).Top = mHeight - mHeight / 8
                        arrLocationSize(9).Left = mWidth / 2
                        arrLocationSize(9).Top = 0
                        arrLocationSize(10).Left = mWidth / 2
                        arrLocationSize(10).Top = mHeight - mHeight * 7 / 8
                        arrLocationSize(11).Left = mWidth / 2
                        arrLocationSize(11).Top = mHeight - mHeight * 6 / 8
                        arrLocationSize(12).Left = mWidth / 2
                        arrLocationSize(12).Top = mHeight - mHeight * 5 / 8
                        arrLocationSize(13).Left = mWidth / 2
                        arrLocationSize(13).Top = mHeight - mHeight * 4 / 8
                        arrLocationSize(14).Left = mWidth / 2
                        arrLocationSize(14).Top = mHeight - mHeight * 3 / 8
                        arrLocationSize(15).Left = mWidth / 2
                        arrLocationSize(15).Top = mHeight - mHeight * 2 / 8
                        arrLocationSize(16).Left = mWidth / 2
                        arrLocationSize(16).Top = mHeight - mHeight / 8

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 8
                            arrLocationSize(I).Width = mWidth / 2
                        Next
                        Exit Select
                End Select
            ElseIf WidthDivHeight >= 0.4 And WidthDivHeight < 2.5 Then
                Select Case countControl
                    Case 4
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth / 2
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = 0
                        arrLocationSize(3).Top = mHeight - mHeight / 2
                        arrLocationSize(4).Left = mWidth - mWidth / 2
                        arrLocationSize(4).Top = mHeight - mHeight / 2

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 2
                            arrLocationSize(I).Width = mWidth / 2
                        Next
                        Exit Select
                    Case 9
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth * 2 / 3
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = mWidth - mWidth / 3
                        arrLocationSize(3).Top = 0
                        arrLocationSize(4).Left = 0
                        arrLocationSize(4).Top = mHeight - mHeight * 2 / 3
                        arrLocationSize(5).Left = mWidth - mWidth * 2 / 3
                        arrLocationSize(5).Top = mHeight - mHeight * 2 / 3
                        arrLocationSize(6).Left = mWidth - mWidth / 3
                        arrLocationSize(6).Top = mHeight - mHeight * 2 / 3
                        arrLocationSize(7).Left = 0
                        arrLocationSize(7).Top = mHeight - mHeight / 3
                        arrLocationSize(8).Left = mWidth - mWidth * 2 / 3
                        arrLocationSize(8).Top = mHeight - mHeight / 3
                        arrLocationSize(9).Left = mWidth - mWidth / 3
                        arrLocationSize(9).Top = mHeight - mHeight / 3

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 3
                            arrLocationSize(I).Width = mWidth / 3
                        Next
                        Exit Select
                    Case 16
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth * 3 / 4
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = mWidth - mWidth * 2 / 4
                        arrLocationSize(3).Top = 0
                        arrLocationSize(4).Left = mWidth - mWidth / 4
                        arrLocationSize(4).Top = 0

                        arrLocationSize(5).Left = 0
                        arrLocationSize(5).Top = mHeight - mHeight * 3 / 4
                        arrLocationSize(6).Left = mWidth - mWidth * 3 / 4
                        arrLocationSize(6).Top = mHeight - mHeight * 3 / 4
                        arrLocationSize(7).Left = mWidth - mWidth * 2 / 4
                        arrLocationSize(7).Top = mHeight - mHeight * 3 / 4
                        arrLocationSize(8).Left = mWidth - mWidth / 4
                        arrLocationSize(8).Top = mHeight - mHeight * 3 / 4

                        arrLocationSize(9).Left = 0
                        arrLocationSize(9).Top = mHeight - mHeight * 2 / 4
                        arrLocationSize(10).Left = mWidth - mWidth * 3 / 4
                        arrLocationSize(10).Top = mHeight - mHeight * 2 / 4
                        arrLocationSize(11).Left = mWidth - mWidth * 2 / 4
                        arrLocationSize(11).Top = mHeight - mHeight * 2 / 4
                        arrLocationSize(12).Left = mWidth - mWidth / 4
                        arrLocationSize(12).Top = mHeight - mHeight * 2 / 4

                        arrLocationSize(13).Left = 0
                        arrLocationSize(13).Top = mHeight - mHeight / 4
                        arrLocationSize(14).Left = mWidth - mWidth * 3 / 4
                        arrLocationSize(14).Top = mHeight - mHeight / 4
                        arrLocationSize(15).Left = mWidth - mWidth * 2 / 4
                        arrLocationSize(15).Top = mHeight - mHeight / 4
                        arrLocationSize(16).Left = mWidth - mWidth / 4
                        arrLocationSize(16).Top = mHeight - mHeight / 4

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 4
                            arrLocationSize(I).Width = mWidth / 4
                        Next
                        Exit Select
                End Select
            ElseIf WidthDivHeight >= 2.5 And WidthDivHeight < 5 Then
                Select Case countControl
                    Case 4
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth / 2
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = 0
                        arrLocationSize(3).Top = mHeight - mHeight / 2
                        arrLocationSize(4).Left = mWidth - mWidth / 2
                        arrLocationSize(4).Top = mHeight - mHeight / 2

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 2
                            arrLocationSize(I).Width = mWidth / 2
                        Next
                        Exit Select
                    Case 9
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth * 4 / 5
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = mWidth - mWidth * 3 / 5
                        arrLocationSize(3).Top = 0
                        arrLocationSize(4).Left = mWidth - mWidth * 2 / 5
                        arrLocationSize(4).Top = 0
                        arrLocationSize(5).Left = mWidth - mWidth / 5
                        arrLocationSize(5).Top = 0

                        arrLocationSize(6).Left = 0
                        arrLocationSize(6).Top = mHeight / 2
                        arrLocationSize(7).Left = mWidth - mWidth * 4 / 5
                        arrLocationSize(7).Top = mHeight / 2
                        arrLocationSize(8).Left = mWidth - mWidth * 3 / 5
                        arrLocationSize(8).Top = mHeight / 2
                        arrLocationSize(9).Left = mWidth - mWidth * 2 / 5
                        arrLocationSize(9).Top = mHeight / 2

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 2
                            arrLocationSize(I).Width = mWidth / 5
                        Next
                        Exit Select
                    Case 16
                        arrLocationSize(1).Left = 0
                        arrLocationSize(1).Top = 0
                        arrLocationSize(2).Left = mWidth - mWidth * 7 / 8
                        arrLocationSize(2).Top = 0
                        arrLocationSize(3).Left = mWidth - mWidth * 6 / 8
                        arrLocationSize(3).Top = 0
                        arrLocationSize(4).Left = mWidth - mWidth * 5 / 8
                        arrLocationSize(4).Top = 0
                        arrLocationSize(5).Left = mWidth - mWidth * 4 / 8
                        arrLocationSize(5).Top = 0
                        arrLocationSize(6).Left = mWidth - mWidth * 3 / 8
                        arrLocationSize(6).Top = 0
                        arrLocationSize(7).Left = mWidth - mWidth * 2 / 8
                        arrLocationSize(7).Top = 0
                        arrLocationSize(8).Left = mWidth - mWidth / 8
                        arrLocationSize(8).Top = 0

                        arrLocationSize(9).Left = 0
                        arrLocationSize(9).Top = mHeight / 2
                        arrLocationSize(10).Left = mWidth - mWidth * 7 / 8
                        arrLocationSize(10).Top = mHeight / 2
                        arrLocationSize(11).Left = mWidth - mWidth * 6 / 8
                        arrLocationSize(11).Top = mHeight / 2
                        arrLocationSize(12).Left = mWidth - mWidth * 5 / 8
                        arrLocationSize(12).Top = mHeight / 2
                        arrLocationSize(13).Left = mWidth - mWidth * 4 / 8
                        arrLocationSize(13).Top = mHeight / 2
                        arrLocationSize(14).Left = mWidth - mWidth * 3 / 8
                        arrLocationSize(14).Top = mHeight / 2
                        arrLocationSize(15).Left = mWidth - mWidth * 2 / 8
                        arrLocationSize(15).Top = mHeight / 2
                        arrLocationSize(16).Left = mWidth - mWidth / 8
                        arrLocationSize(16).Top = mHeight / 2

                        For I = 1 To countControl
                            arrLocationSize(I).Height = mHeight / 2
                            arrLocationSize(I).Width = mWidth / 8
                        Next
                        Exit Select
                End Select
            ElseIf WidthDivHeight >= 5 Then
                arrLocationSize(1).Left = 0
                arrLocationSize(1).Top = 0
                K = 1

                For J = 2 To countControl - 1
                    arrLocationSize(J).Left = mWidth - mWidth * (countControl - K) / countControl
                    arrLocationSize(J).Top = 0
                    K += 1
                Next

                arrLocationSize(countControl).Left = mWidth - mWidth / countControl
                arrLocationSize(countControl).Top = 0

                For I = 1 To countControl
                    arrLocationSize(I).Height = mHeight
                    arrLocationSize(I).Width = mWidth / countControl
                Next
            End If

            For I = 1 To countControl
                indicatorControls(I - 1).SetBounds(CInt(arrLocationSize(I).Left), CInt(arrLocationSize(I).Top), CInt(arrLocationSize(I).Width), CInt(arrLocationSize(I).Height))
            Next
        End If
    End Sub

    Private Sub FormSelectiveControl_Closed(ByVal eventSender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)

        If WindowState <> FormWindowState.Minimized Then
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlLeft", CStr(Left))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlTop", CStr(Top))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlWidth", CStr(Width))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "SelectiveControlHeight", CStr(Height))
        End If

        parentFormRegistration.IsShowGraphControl = False
        indicatorControls.Clear()
        parentFormRegistration.MenuShowGraphControl.Enabled = True
        'CollectionForms.Remove(Text)
        parentFormRegistration = Nothing
    End Sub

    Private Sub PopulateSelectiveControl()
        ' считать из файла строку с параметрами контроля и расшифровать ее в массив
        Dim I, J As Integer
        Dim countSelected As Integer ' счетчики количества элементов в коллекции
        Dim unitOfMeasure As String ' Единица Измерения

        indicatorControls.Clear()
        Controls.Clear()
        ' менеджер настроек
        ' считать из файла строку с параметрами контроля и расшифровать ее в массив
        Dim mSettingSelectedParameters As New SettingSelectedParameters(PathResourses, [Enum].GetName(GetType(TypeFormTuningSelective), TypeFormTuningSelective.GraphControl))

        ' создается массив индексов и конфигуриуется Контроль(i)
        For J = 0 To UBound(mSettingSelectedParameters.SelectedParametersAsString)
            For I = 1 To UBound(IndexParametersForControl)
                If ParametersType(IndexParametersForControl(I)).NameParameter = mSettingSelectedParameters.SelectedParametersAsString(J) Then
                    countSelected += 1
                    unitOfMeasure = ParametersType(IndexParametersForControl(I)).UnitOfMeasure
                    '"%";"дел";"мм";"градус";"Кгсм";"мм/с";"мкА";"кг/ч";"кгс"
                    Select Case unitOfMeasure
                        Case "Кгсм"
                            indicatorControls.Add(New PressureIndicator("Давление" + countSelected.ToString, Me))
                            Exit Select
                        Case "%", "кгс"
                            indicatorControls.Add(New RotationIndicator("Обороты" + countSelected.ToString, Me))
                            Exit Select
                        Case "кг/ч"
                            indicatorControls.Add(New VibrationIndicator("Вибрация" + countSelected.ToString, Me))
                            Exit Select
                        Case "мкА"
                            indicatorControls.Add(New CurrentIndicator("Ток" + countSelected.ToString, Me))
                            Exit Select
                        Case "градус"
                            indicatorControls.Add(New TemperatureIndicator("Температура" + countSelected.ToString, Me))
                            Exit Select
                        Case "мм"
                            indicatorControls.Add(New TankIndicator("Столбы" + countSelected.ToString, Me))
                            Exit Select
                        Case "дел", "мм/с"
                            indicatorControls.Add(New MassFlowIndicator("Расход" + countSelected.ToString, Me))
                            'Case "кгс"
                            '    mКлассТяга = New КлассТяга("Тяга" + type8.ToString, Me)
                            '    Контроль.Add(mКлассТяга)
                            Exit Select
                        Case Else
                            indicatorControls.Add(New PressureIndicator("Давление" + countSelected.ToString, Me))
                            Exit Select
                    End Select
                    ' мин, мах, наименование из arrПараметры
                    'Контроль(intОтмечено - 1).Экземпляр.Axis.Minimum = arrПараметры(arrСписокПараметровКонтроля(I)).sgnДопускМинимум
                    'Контроль(intОтмечено - 1).Экземпляр.Axis.Maximum = arrПараметры(arrСписокПараметровКонтроля(I)).sgnДопускМаксимум
                    'Контроль(intОтмечено - 1).Экземпляр.Range = New NationalInstruments.UI.Range(arrПараметры(arrСписокПараметровКонтроля(I)).sgnДопускМинимум, arrПараметры(arrСписокПараметровКонтроля(I)).sgnДопускМаксимум)

                    indicatorControls(countSelected - 1).Range = New Range(ParametersType(IndexParametersForControl(I)).LowerLimit, ParametersType(IndexParametersForControl(I)).UpperLimit)
                    indicatorControls(countSelected - 1).LimitMin = ParametersType(IndexParametersForControl(I)).LowerLimit
                    indicatorControls(countSelected - 1).LimitMax = ParametersType(IndexParametersForControl(I)).UpperLimit
                    indicatorControls(countSelected - 1).AlarmMin = ParametersType(IndexParametersForControl(I)).AlarmValueMin
                    indicatorControls(countSelected - 1).AlarmMax = ParametersType(IndexParametersForControl(I)).AlarmValueMax
                    indicatorControls(countSelected - 1).ParameterName = ParametersType(IndexParametersForControl(I)).NameParameter

                    Exit For
                End If
            Next I
        Next J

        If countSelected <= 4 Then
            countControl = 4
        ElseIf countSelected > 4 And countSelected <= 9 Then
            countControl = 9
        ElseIf countSelected > 9 And countSelected <= 16 Then
            countControl = 16
        End If

        ' добавить панели для пустых контролов
        For I = 1 To countControl - countSelected
            countSelected += 1
            indicatorControls.Add(New GroupBoxIndicator(EmptyName, Me))
        Next

        If countSelected <= 4 Then
            MinimumSize = New Size(300, 300)
        ElseIf countSelected > 4 And countSelected <= 9 Then
            MinimumSize = New Size(450, 450)
        ElseIf countSelected > 9 And countSelected <= 16 Then
            MinimumSize = New Size(600, 600)
        End If

        For Each itemIndicator As IBaseClassIndicator In indicatorControls
            If itemIndicator.Name <> EmptyName Then
                For J = 1 To UBound(IndexParametersForControl)
                    If ParametersType(IndexParametersForControl(J)).NameParameter = itemIndicator.ParameterName Then
                        itemIndicator.NumberParameter = J - 1
                        Exit For
                    End If
                Next
            End If
        Next
    End Sub

    ''' <summary>
    ''' Задать значение параметра для индикатора с последующим обновлением
    ''' </summary>
    Friend Sub UpdateValueIndicatorControls(ByRef arrСреднее(,) As Double, ByVal x As Integer)
        For Each itemIndicator As IBaseClassIndicator In indicatorControls
            If itemIndicator.Name <> EmptyName Then
                itemIndicator.Value = arrСреднее(itemIndicator.NumberParameter, x)
            End If
        Next
    End Sub

    Public Sub UpdateControls() Implements IUpdateSelectiveControls.UpdateControls
        parentFormRegistration.IsShowGraphControl = False
        PopulateSelectiveControl()
        FormSelectiveControl_Resize(Me, New EventArgs)
        parentFormRegistration.IsShowGraphControl = True
    End Sub
End Class