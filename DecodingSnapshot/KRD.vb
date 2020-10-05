Imports MathematicalLibrary.Spline3

''' <summary>
''' "КРД-А", "КРД-Б", "АРД-39", "КРД99Ц", "ЦРД99"
''' </summary>
Friend MustInherit Class KRD
    ''' <summary>
    ''' Тип КРД
    ''' </summary>
    ''' <returns></returns>
    Public Property Type As KRDType

    Protected mN1НастройкаКРД As Double
    ''' <summary>
    ''' Пересчитанное N1 КРД
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property N1НастройкаКРД() As Double
        Get
            Return mN1НастройкаКРД
        End Get
    End Property

    Protected mN2НастройкаКРД As Double
    ''' <summary>
    ''' Пересчитанное N2 КРД
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property N2НастройкаКРД() As Double
        Get
            Return mN2НастройкаКРД
        End Get
    End Property

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public MustOverride Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)

    Protected PointN1_X(), PointN1_Y(), PointN2_X(), PointN2_Y() As Double
    Protected TuningN1KRD_15, TuningN2KRD_15 As Double ' Настройка N1 N2 КРД при +15 град.

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Protected Sub CalculateBase(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        Dim N1КРД, N2КРД As Double

        'для N1
        N1КРД = InterpLine(PointN1_X, PointN1_Y, inTbox)
        N1КРД += inN1КРД15 - TuningN1KRD_15 'для 15 градусов прибавка      
        'для N2
        N2КРД = InterpLine(PointN2_X, PointN2_Y, inTbox)
        N2КРД += inN2КРД15 - TuningN2KRD_15 'для 15 градусов прибавка

        mN1НастройкаКРД = Math.Round(N1КРД, 2)
        mN2НастройкаКРД = Math.Round(N2КРД, 2)
    End Sub

    'Protected PointN1(), PointN2() As PointF
    '' Точки перегиба характеристик регулирования
    'Protected Point1BendN1, Point2BendN1 As Double
    'Protected Point1BendN2, Point2BendN2, Point3BendN2, Point4BendN2 As Double

    'Protected Sub CalculateBaseOld(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
    '    Dim N1КРД, N2КРД As Double
    '    'для N1
    '    If inTbox < Point1BendN1 Then
    '        N1КРД = PointN1(1).Y
    '    ElseIf inTbox >= Point1BendN1 AndAlso inTbox < Point2BendN1 Then
    '        N1КРД = LinearInterpolation(inTbox, PointN1(1).X, PointN1(1).Y, PointN1(2).X, PointN1(2).Y)
    '    ElseIf inTbox >= Point2BendN1 Then
    '        N1КРД = PointN1(2).Y
    '    End If

    '    N1КРД += inN1КРД15 - PointN1(2).Y 'для 15 градусов прибавка

    '    'для N2
    '    If inTbox < Point1BendN2 Then
    '        N2КРД = PointN2(1).Y
    '    ElseIf inTbox >= Point1BendN2 AndAlso inTbox < Point2BendN2 Then
    '        N2КРД = LinearInterpolation(inTbox, PointN2(1).X, PointN2(1).Y, PointN2(2).X, PointN2(2).Y)
    '    ElseIf inTbox >= Point2BendN2 AndAlso inTbox < Point3BendN2 Then
    '        N2КРД = LinearInterpolation(inTbox, PointN2(2).X, PointN2(2).Y, PointN2(3).X, PointN2(3).Y)
    '    ElseIf inTbox >= Point3BendN2 AndAlso inTbox < Point4BendN2 Then
    '        N2КРД = LinearInterpolation(inTbox, PointN2(3).X, PointN2(3).Y, PointN2(4).X, PointN2(4).Y)
    '    ElseIf inTbox >= Point4BendN2 Then
    '        N2КРД = PointN2(4).Y
    '    End If

    '    N2КРД += inN2КРД15 - PointN2(3).Y 'для 15 градусов прибавка

    '    mN1НастройкаКРД = Math.Round(N1КРД, 2)
    '    mN2НастройкаКРД = Math.Round(N2КРД, 2)
    'End Sub
End Class

''' <summary>
''' КРД-А
''' </summary>
Friend Class KRD_A
    Inherits KRD

    Public Sub New()
        Type = KRDType.KRD_A

        ' Настройка N1 N2 КРД при +15 град.
        TuningN1KRD_15 = 98.0
        TuningN2KRD_15 = 99.7
        ' для расчета используется INTERP1 - массив начинается с 1
        PointN1_X = {-1, -60.0, -20.0, 60.0}
        PointN1_Y = {-1, 90.3, 98.0, 98.0}
        PointN2_X = {-1, -60.0, -27.0, 15.0, 60.0}
        PointN2_Y = {-1, 89.6, 96.5, 99.7, 99.3}
    End Sub

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public Overrides Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        CalculateBase(inTbox, inN1КРД15, inN2КРД15)
        ' или может реализовать собственную логику расчёта
    End Sub

    'Private fPointN1(2) As PointF
    'Private fPointN2(4) As PointF

    'Public Sub New()
    '    Type = KRDType.KRD_A

    '    Point1BendN1 = -60.0
    '    Point2BendN1 = -20.0

    '    Point1BendN2 = -60.0
    '    Point2BendN2 = -27.0
    '    Point3BendN2 = 15.0
    '    Point4BendN2 = 60.0

    '    '--- j=1 - КРДА -------------------------------------------------------
    '    'график N1 2 точки
    '    fPointN1(1).X = Point1BendN1
    '    fPointN1(1).Y = 90.3
    '    fPointN1(2).X = Point2BendN1
    '    fPointN1(2).Y = 98.0
    '    'график N2 4 точки
    '    fPointN2(1).X = Point1BendN2
    '    fPointN2(1).Y = 89.6
    '    fPointN2(2).X = Point2BendN2
    '    fPointN2(2).Y = 96.5
    '    fPointN2(3).X = Point3BendN2
    '    fPointN2(3).Y =
    '    fPointN2(4).X = Point4BendN2
    '    fPointN2(4).Y = 99.3

    '    PointN1 = fPointN1
    '    PointN2 = fPointN2
    'End Sub
End Class

''' <summary>
''' КРД-Б
''' </summary>
Friend Class KRD_B
    Inherits KRD

    Public Sub New()
        Type = KRDType.KRD_B

        ' Настройка N1 N2 КРД при +15 град.
        TuningN1KRD_15 = 98.0
        TuningN2KRD_15 = 99.7
        ' для расчета используется INTERP1 - массив начинается с 1
        PointN1_X = {-1, -60.0, -20.0, 60.0}
        PointN1_Y = {-1, 90.3, 98.0, 98.0}
        PointN2_X = {-1, -60.0, -21.0, 15.0, 60.0}
        PointN2_Y = {-1, 89.5, 97.8, 99.7, 99.3}
    End Sub

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public Overrides Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        CalculateBase(inTbox, inN1КРД15, inN2КРД15)
        ' или может реализовать собственную логику расчёта
    End Sub
End Class

''' <summary>
''' АРД-39
''' </summary>
Friend Class ARD_39
    Inherits KRD

    Public Sub New()
        Type = KRDType.ARD_39

        ' Настройка N1 N2 КРД при +15 град.
        TuningN1KRD_15 = 98.0
        TuningN2KRD_15 = 99.7
        ' для расчета используется INTERP1 - массив начинается с 1
        PointN1_X = {-1, -60.0, -21.0, 60.0}
        PointN1_Y = {-1, 90.3, 98.0, 98.0}
        PointN2_X = {-1, -60.0, -21.0, 15.0, 60.0}
        PointN2_Y = {-1, 89.6, 97.8, 99.7, 98.8}
    End Sub

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public Overrides Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        CalculateBase(inTbox, inN1КРД15, inN2КРД15)
        ' или может реализовать собственную логику расчёта
    End Sub
End Class

''' <summary>
''' КРД99Ц
''' </summary>
Friend Class KRD_99_C
    Inherits KRD

    Public Sub New()
        Type = KRDType.KRD_99_C

        ' Настройка N1 N2 КРД при +15 град.
        TuningN1KRD_15 = 98.875
        TuningN2KRD_15 = 101.5
        ' для расчета используется INTERP1 - массив начинается с 1
        PointN1_X = {-1, -60.0, -20.0, 60.0}
        PointN1_Y = {-1, 90.0, 98.0, 100.0}
        PointN2_X = {-1, -60.0, -20.0, 15.0, 40.0}
        PointN2_Y = {-1, 90.4, 98.0, 101.5, 101.3}
    End Sub

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public Overrides Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        CalculateBase(inTbox, inN1КРД15, inN2КРД15)
        ' или может реализовать собственную логику расчёта
    End Sub
End Class

''' <summary>
''' ЦРД99
''' </summary>
Friend Class CRD_99
    Inherits KRD

    Public Sub New()
        Type = KRDType.CRD_99

        ' Настройка N1 N2 КРД при +15 град.
        TuningN1KRD_15 = 98.875
        TuningN2KRD_15 = 102.8
        ' для расчета используется INTERP1 - массив начинается с 1
        PointN1_X = {-1, -60.0, -20.0, 60.0}
        PointN1_Y = {-1, 90.0, 98.0, 100.0}
        PointN2_X = {-1, -60.0, -20.0, 15.0, 40.0}
        PointN2_Y = {-1, 90.2, 98.0, 102.8, 100.7}
    End Sub

    ''' <summary>
    ''' Рассчитать настройки КРД
    ''' </summary>
    ''' <param name="inTbox"></param>
    ''' <param name="inN1КРД15"></param>
    ''' <param name="inN2КРД15"></param>
    Public Overrides Sub Calculate(inTbox As Double, inN1КРД15 As Double, inN2КРД15 As Double)
        CalculateBase(inTbox, inN1КРД15, inN2КРД15)
        ' или может реализовать собственную логику расчёта
    End Sub
End Class
