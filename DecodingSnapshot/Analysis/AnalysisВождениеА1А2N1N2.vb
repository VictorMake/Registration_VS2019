Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 14 '"Вождение a1,a2,N1,N2"
''' </summary>
Friend Class AnalysisВождениеА1А2N1N2
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(11, 3)
        Re.Dim(Protocol, 11, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "N1пр.min"
        Protocol(5, 1) = "N1пр.max"
        Protocol(6, 1) = "a1min-a1max"
        Protocol(7, 1) = "a1min-a1max без влияния N1"
        Protocol(8, 1) = "N2пр.min"
        Protocol(9, 1) = "N2пр.max"
        Protocol(10, 1) = "a2min-a2max"
        Protocol(11, 1) = "a2min-a2max без влияния N2"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "%"
        Protocol(5, 2) = "%"
        Protocol(6, 2) = "дел."
        Protocol(7, 2) = "дел."
        Protocol(8, 2) = "%"
        Protocol(9, 2) = "%"
        Protocol(10, 2) = "дел."
        Protocol(11, 2) = "дел."

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = ""
        Protocol(7, 3) = GetEngineNormTUParameter(33)
        Protocol(8, 3) = ""
        Protocol(9, 3) = ""
        Protocol(10, 3) = ""
        Protocol(11, 3) = GetEngineNormTUParameter(33)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String
        Dim КоэПриведения As Double = System.Math.Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin))
        Dim J As Integer
        'J=1 для 99 изделия, J=2 для 39 изделия
        Dim arrТочкиА1(5) As КоординатыОборотыУглы 'для графика а1'КНД
        Dim arrТочкиА2(4) As КоординатыОборотыУглы 'для графика а2'КНД
        Dim МинимальноеПриведенноеЗначениеN1, МинимальноеПриведенноеЗначениеN2, МаксимальноеПриведенноеЗначениеN1, МаксимальноеПриведенноеЗначениеN2 As Double
        Dim МинимальноеПриведенноеЗначениеА1, МинимальноеПриведенноеЗначениеА2, МаксимальноеПриведенноеЗначениеА1, МаксимальноеПриведенноеЗначениеА2 As Double

        J = 1 'КНД
        Select Case Parent.TypeKRDinSnapshot
            Case "КРД-А", "КРД-Б"
                J = 1
                'j=1 - 99 изделие
                'график теоретическая 5 точек
                '1
                arrТочкиА1(1).ОборотыПриведенные = 70
                arrТочкиА1(1).Угол = 4
                '2
                arrТочкиА1(2).ОборотыПриведенные = 80
                arrТочкиА1(2).Угол = 4
                '3
                arrТочкиА1(3).ОборотыПриведенные = 87.5
                arrТочкиА1(3).Угол = 74
                '4
                arrТочкиА1(4).ОборотыПриведенные = 95
                arrТочкиА1(4).Угол = 104
                '5
                arrТочкиА1(5).ОборотыПриведенные = 105
                arrТочкиА1(5).Угол = 104

                '*********************************
                'j=1 - 99 изделие
                'график теоретическая 4 точки
                '1
                arrТочкиА2(1).ОборотыПриведенные = 68
                arrТочкиА2(1).Угол = 4
                '2
                arrТочкиА2(2).ОборотыПриведенные = 73.5
                arrТочкиА2(2).Угол = 4
                '3
                arrТочкиА2(3).ОборотыПриведенные = 88
                arrТочкиА2(3).Угол = 104
                '4
                arrТочкиА2(4).ОборотыПриведенные = 105
                arrТочкиА2(4).Угол = 104

                'Case "КРД-Б"
                '    J = 1
                Exit Select
            Case "АРД-39"
                J = 2
                '************************************
                'j=2 - 39 изделие
                'график теоретическая 5 точек
                '1
                arrТочкиА1(1).ОборотыПриведенные = 70
                arrТочкиА1(1).Угол = 4
                '2
                arrТочкиА1(2).ОборотыПриведенные = 80
                arrТочкиА1(2).Угол = 4
                '3
                arrТочкиА1(3).ОборотыПриведенные = 87.5
                arrТочкиА1(3).Угол = 74
                '4
                arrТочкиА1(4).ОборотыПриведенные = 95
                arrТочкиА1(4).Угол = 104
                '5
                arrТочкиА1(5).ОборотыПриведенные = 105
                arrТочкиА1(5).Угол = 104

                '************************************
                'j=2 - 39 изделие
                'график теоретическая 4 точки
                '1
                arrТочкиА2(1).ОборотыПриведенные = 68
                arrТочкиА2(1).Угол = 4
                '2
                arrТочкиА2(2).ОборотыПриведенные = 73.25
                arrТочкиА2(2).Угол = 4
                '3
                arrТочкиА2(3).ОборотыПриведенные = 88
                arrТочкиА2(3).Угол = 104
                '4
                arrТочкиА2(4).ОборотыПриведенные = 105
                arrТочкиА2(4).Угол = 104
                Exit Select
            Case "КНД924"
                J = 3
                'j=3 - 99 изделие КНД924
                'график теоретическая 5 точек
                '1
                arrТочкиА1(1).ОборотыПриведенные = 70
                arrТочкиА1(1).Угол = 4
                '2
                arrТочкиА1(2).ОборотыПриведенные = 82
                arrТочкиА1(2).Угол = 4
                '3
                arrТочкиА1(3).ОборотыПриведенные = 90
                arrТочкиА1(3).Угол = 70
                '4
                arrТочкиА1(4).ОборотыПриведенные = 94
                arrТочкиА1(4).Угол = 104
                '5
                arrТочкиА1(5).ОборотыПриведенные = 105
                arrТочкиА1(5).Угол = 104
                'j=3 - 99 изделие КНД924
                'график теоретическая 4 точки
                '1
                arrТочкиА2(1).ОборотыПриведенные = 68
                arrТочкиА2(1).Угол = 4
                '2
                arrТочкиА2(2).ОборотыПриведенные = 73.5
                arrТочкиА2(2).Угол = 4
                '3
                arrТочкиА2(3).ОборотыПриведенные = 88
                arrТочкиА2(3).Угол = 104
                '4
                arrТочкиА2(4).ОборотыПриведенные = 105
                arrТочкиА2(4).Угол = 104
        End Select

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

        'находим заброс N1 относительно установившегося
        параметр = conN1
        Dim clsЗабросN1ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsЗабросN1ОтносительноУстановившегося
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 
            .Расчет()
        End With

        If clsЗабросN1ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросN1ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросN1ОтносительноУстановившегося
                'If .DeltaA > 0 Then
                Parent.TracingDecodingArrow(
                Parent.XAxisTime.Range.Maximum - 5,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                Parent.XAxisTime.Range.Maximum,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Inclined,
                параметр & ":уст.=" & Round(.Аконечное, 2) & " %")
                'Protocol(10, 2) = параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                'End If
            End With
            '************************************************
            'нахождение минимального и максимального значения параметра N1
            Dim clsМинимальноеМаксимальноеЗначениеПараметраN1 As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                                              Parent.MeasuredValues,
                                                                                                              Parent.SnapshotSmallParameters,
                                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                                              Parent.XAxisTime.Range.Maximum)
            With clsМинимальноеМаксимальноеЗначениеПараметраN1
                '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Расчет()
            End With
            If clsМинимальноеМаксимальноеЗначениеПараметраN1.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsМинимальноеМаксимальноеЗначениеПараметраN1.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsМинимальноеМаксимальноеЗначениеПараметраN1
                    МинимальноеПриведенноеЗначениеN1 = .МинимальноеЗначение * КоэПриведения
                    МаксимальноеПриведенноеЗначениеN1 = .МаксимальноеЗначение * КоэПриведения

                    Parent.TracingDecodingArrow(
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    .ТМинимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                    ArrowType.Vertical,
                    параметр & ":max-min=" & Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " %")
                    Protocol(4, 2) = "привед.min=" & Round(МинимальноеПриведенноеЗначениеN1, 2) & " %"
                    Protocol(5, 2) = "привед.max=" & Round(МаксимальноеПриведенноеЗначениеN1, 2) & " %"
                End With
            End If
        End If

        'находим заброс N2 относительно установившегося
        параметр = conN2
        Dim clsЗабросN2ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsЗабросN2ОтносительноУстановившегося
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 
            .Расчет()
        End With

        If clsЗабросN2ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросN2ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросN2ОтносительноУстановившегося
                'If .DeltaA > 0 Then
                Parent.TracingDecodingArrow(
                Parent.XAxisTime.Range.Maximum - 5,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                Parent.XAxisTime.Range.Maximum,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Inclined,
                параметр & ":уст.=" & Round(.Аконечное, 2) & " %")
                'Protocol(10, 2) = параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                'End If
            End With
            '************************************************
            'нахождение минимального и максимального значения параметра N2
            Dim clsМинимальноеМаксимальноеЗначениеПараметраN2 As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                                              Parent.MeasuredValues,
                                                                                                              Parent.SnapshotSmallParameters,
                                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                                              Parent.XAxisTime.Range.Maximum)
            With clsМинимальноеМаксимальноеЗначениеПараметраN2
                '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Расчет()
            End With
            If clsМинимальноеМаксимальноеЗначениеПараметраN2.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsМинимальноеМаксимальноеЗначениеПараметраN2.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsМинимальноеМаксимальноеЗначениеПараметраN2
                    МинимальноеПриведенноеЗначениеN2 = .МинимальноеЗначение * КоэПриведения
                    МаксимальноеПриведенноеЗначениеN2 = .МаксимальноеЗначение * КоэПриведения

                    Parent.TracingDecodingArrow(
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    .ТМинимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                    ArrowType.Vertical,
                    параметр & ":max-min=" & Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " %")
                    Protocol(8, 2) = "привед.min=" & Round(МинимальноеПриведенноеЗначениеN2, 2) & " %"
                    Protocol(9, 2) = "привед.max=" & Round(МаксимальноеПриведенноеЗначениеN2, 2) & " %"
                End With
            End If
        End If

        'находим заброс a1 относительно установившегося
        параметр = cona1
        Dim clsЗабросa1ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsЗабросa1ОтносительноУстановившегося
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 
            .Расчет()
        End With

        If clsЗабросa1ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросa1ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросa1ОтносительноУстановившегося
                'If .DeltaA > 0 Then
                Parent.TracingDecodingArrow(
                Parent.XAxisTime.Range.Maximum - 5,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                Parent.XAxisTime.Range.Maximum,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Inclined,
                параметр & ":уст.=" & Round(.Аконечное, 2) & " дел.")
                'Protocol(10, 2) = параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                'End If
            End With
            '************************************************
            'нахождение минимального и максимального значения параметра a1
            Dim clsМинимальноеМаксимальноеЗначениеПараметраa1 As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                                              Parent.MeasuredValues,
                                                                                                              Parent.SnapshotSmallParameters,
                                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                                              Parent.XAxisTime.Range.Maximum)
            With clsМинимальноеМаксимальноеЗначениеПараметраa1
                '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Расчет()
            End With
            If clsМинимальноеМаксимальноеЗначениеПараметраa1.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsМинимальноеМаксимальноеЗначениеПараметраa1.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsМинимальноеМаксимальноеЗначениеПараметраa1
                    Parent.TracingDecodingArrow(
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    .ТМинимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                    ArrowType.Vertical,
                    параметр & ":max-min=" & Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " дел.")
                    Protocol(6, 2) = Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " дел."

                    МинимальноеПриведенноеЗначениеА1 = ПромежуточныеОборотыПриведАльфа(МинимальноеПриведенноеЗначениеN1, arrТочкиА1)
                    МаксимальноеПриведенноеЗначениеА1 = ПромежуточныеОборотыПриведАльфа(МаксимальноеПриведенноеЗначениеN1, arrТочкиА1)
                    Dim ВождениеУглаА1БезN1 As Double
                    ВождениеУглаА1БезN1 = (.МаксимальноеЗначение - .МинимальноеЗначение) - (МаксимальноеПриведенноеЗначениеА1 - МинимальноеПриведенноеЗначениеА1)
                    Protocol(7, 2) = Round(ВождениеУглаА1БезN1, 2) & " дел."
                End With
            End If
        End If

        'находим заброс a2 относительно установившегося
        параметр = cona2
        Dim clsЗабросa2ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsЗабросa2ОтносительноУстановившегося
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 
            .Расчет()
        End With

        If clsЗабросa2ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросa2ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросa2ОтносительноУстановившегося
                'If .DeltaA > 0 Then
                Parent.TracingDecodingArrow(
                Parent.XAxisTime.Range.Maximum - 5,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                Parent.XAxisTime.Range.Maximum,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Inclined,
                параметр & ":уст.=" & Round(.Аконечное, 2) & " дел.")
                'Protocol(10, 2) = параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                'End If
            End With
            '************************************************
            'нахождение минимального и максимального значения параметра a2
            Dim clsМинимальноеМаксимальноеЗначениеПараметраa2 As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                                              Parent.MeasuredValues,
                                                                                                              Parent.SnapshotSmallParameters,
                                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                                              Parent.XAxisTime.Range.Maximum)
            With clsМинимальноеМаксимальноеЗначениеПараметраa2
                '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Расчет()
            End With
            If clsМинимальноеМаксимальноеЗначениеПараметраa2.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsМинимальноеМаксимальноеЗначениеПараметраa2.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsМинимальноеМаксимальноеЗначениеПараметраa2
                    Parent.TracingDecodingArrow(
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    .ТМинимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                    ArrowType.Vertical,
                    параметр & ":max-min=" & Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " дел.")
                    Protocol(10, 2) = Round(.МаксимальноеЗначение - .МинимальноеЗначение, 2) & " дел."

                    МинимальноеПриведенноеЗначениеА2 = ПромежуточныеОборотыПриведАльфа(МинимальноеПриведенноеЗначениеN2, arrТочкиА2)
                    МаксимальноеПриведенноеЗначениеА2 = ПромежуточныеОборотыПриведАльфа(МаксимальноеПриведенноеЗначениеN2, arrТочкиА2)
                    Dim ВождениеУглаА2БезN2 As Double
                    ВождениеУглаА2БезN2 = (.МаксимальноеЗначение - .МинимальноеЗначение) - (МаксимальноеПриведенноеЗначениеА2 - МинимальноеПриведенноеЗначениеА2)
                    Protocol(11, 2) = Round(ВождениеУглаА2БезN2, 2) & " дел."
                End With
            End If
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

