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
        Re.Dim(Protocol, 11, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"N1пр.min", "%", ""})
        PopulateProtocol(5, {"N1пр.max", "%", ""})
        PopulateProtocol(6, {"a1min-a1max", "дел.", ""})
        PopulateProtocol(7, {"a1min-a1max без влияния N1", "дел.", GetEngineNormTUParameter(33)})
        PopulateProtocol(8, {"N2пр.min", "%", ""})
        PopulateProtocol(9, {"N2пр.max", "%", ""})
        PopulateProtocol(10, {"a2min-a2max", "дел.", ""})
        PopulateProtocol(11, {"a2min-a2max без влияния N2", "дел.", GetEngineNormTUParameter(33)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()

        Dim transferConstant As Double = Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin)) ' КоэПриведения
        Dim pointsA1(5) As RotationAngleCoordinate ' для графика а1 КНД
        Dim pointsA2(4) As RotationAngleCoordinate ' для графика а2 КНД
        ' Приведенные значения
        Dim minTransferN1, minTransferN2, maxTransferN1, maxTransferN2 As Double
        Dim minTransferA1, minTransferA2, maxTransferA1, maxTransferA2 As Double

        ' J=1 для 99 изделия, J=2 для 39 изделия
        Select Case Parent.TypeKRDinSnapshot
            Case cKRD_A, cKRD_B
                'график теоретическая 5 точек
                pointsA1(1).RotationReduced = 70
                pointsA1(1).Angle = 4
                pointsA1(2).RotationReduced = 80
                pointsA1(2).Angle = 4
                pointsA1(3).RotationReduced = 87.5
                pointsA1(3).Angle = 74
                pointsA1(4).RotationReduced = 95
                pointsA1(4).Angle = 104
                pointsA1(5).RotationReduced = 105
                pointsA1(5).Angle = 104

                'график теоретическая 4 точки
                pointsA2(1).RotationReduced = 68
                pointsA2(1).Angle = 4
                pointsA2(2).RotationReduced = 73.5
                pointsA2(2).Angle = 4
                pointsA2(3).RotationReduced = 88
                pointsA2(3).Angle = 104
                pointsA2(4).RotationReduced = 105
                pointsA2(4).Angle = 104
            Case cARD_39
                'график теоретическая 5 точек
                pointsA1(1).RotationReduced = 70
                pointsA1(1).Angle = 4
                pointsA1(2).RotationReduced = 80
                pointsA1(2).Angle = 4
                pointsA1(3).RotationReduced = 87.5
                pointsA1(3).Angle = 74
                pointsA1(4).RotationReduced = 95
                pointsA1(4).Angle = 104
                pointsA1(5).RotationReduced = 105
                pointsA1(5).Angle = 104

                'график теоретическая 4 точки
                pointsA2(1).RotationReduced = 68
                pointsA2(1).Angle = 4
                pointsA2(2).RotationReduced = 73.25
                pointsA2(2).Angle = 4
                pointsA2(3).RotationReduced = 88
                pointsA2(3).Angle = 104
                pointsA2(4).RotationReduced = 105
                pointsA2(4).Angle = 104
                Exit Select
            Case Else 'cKRD_99_C, cCRD_99, 99 изделие КНД924
                'j=3 - 99 изделие КНД924
                'график теоретическая 5 точек
                pointsA1(1).RotationReduced = 70
                pointsA1(1).Angle = 4
                pointsA1(2).RotationReduced = 82
                pointsA1(2).Angle = 4
                pointsA1(3).RotationReduced = 90
                pointsA1(3).Angle = 70
                pointsA1(4).RotationReduced = 94
                pointsA1(4).Angle = 104
                pointsA1(5).RotationReduced = 105
                pointsA1(5).Angle = 104

                'график теоретическая 4 точки
                pointsA2(1).RotationReduced = 68
                pointsA2(1).Angle = 4
                pointsA2(2).RotationReduced = 73.5
                pointsA2(2).Angle = 4
                pointsA2(3).RotationReduced = 88
                pointsA2(3).Angle = 104
                pointsA2(4).RotationReduced = 105
                pointsA2(4).Angle = 104
        End Select

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

        ' заброс N1 относительно установившегося
        parameter = conN1
        Dim mЗабросN1ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        mЗабросN1ОтносительноУстановившегося.Calculation()
        If mЗабросN1ОтносительноУстановившегося.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mЗабросN1ОтносительноУстановившегося.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mЗабросN1ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                    Parent.XAxisTime.Range.Maximum - 5,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    Parent.XAxisTime.Range.Maximum,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Inclined,
                    $"{parameter}:уст.={Round(.Astop, 2)} %")
            End With

            ' нахождение минимального и максимального значения параметра N1
            Dim mМинимальноеМаксимальноеЗначениеN1 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mМинимальноеМаксимальноеЗначениеN1
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    minTransferN1 = .MinValue * transferConstant
                    maxTransferN1 = .MaxValue * transferConstant

                    Parent.TracingDecodingArrow(
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        .TimeMinValue,
                        CastToAxesStandard(.IndexParameter, .MinValue),
                        ArrowType.Vertical,
                        $"{parameter}:max-min={Round(.MaxValue - .MinValue, 2)} %")
                    Protocol(4, 2) = $"привед.min={Round(minTransferN1, 2)} %"
                    Protocol(5, 2) = $"привед.max={Round(maxTransferN1, 2)} %"
                End If
            End With
        End If

        ' заброс N2 относительно установившегося
        parameter = conN2
        Dim mЗабросN2ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        mЗабросN2ОтносительноУстановившегося.Calculation()
        If mЗабросN2ОтносительноУстановившегося.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mЗабросN2ОтносительноУстановившегося.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mЗабросN2ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                    Parent.XAxisTime.Range.Maximum - 5,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    Parent.XAxisTime.Range.Maximum,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Inclined,
                    $"{parameter}:уст.={Round(.Astop, 2)} %")
            End With

            ' нахождение минимального и максимального значения параметра N2
            Dim mМинимальноеМаксимальноеЗначениеN2 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mМинимальноеМаксимальноеЗначениеN2
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    minTransferN2 = .MinValue * transferConstant
                    maxTransferN2 = .MaxValue * transferConstant

                    Parent.TracingDecodingArrow(
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        .TimeMinValue,
                        CastToAxesStandard(.IndexParameter, .MinValue),
                        ArrowType.Vertical,
                        $"{parameter}:max-min={Round(.MaxValue - .MinValue, 2)} %")
                    Protocol(8, 2) = $"привед.min={Round(minTransferN2, 2)} %"
                    Protocol(9, 2) = $"привед.max={Round(maxTransferN2, 2)} %"
                End If
            End With
        End If

        ' заброс a1 относительно установившегося
        parameter = cona1
        Dim mЗабросaОтносительноУстановившегосяA1 = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        mЗабросaОтносительноУстановившегосяA1.Calculation()
        If mЗабросaОтносительноУстановившегосяA1.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mЗабросaОтносительноУстановившегосяA1.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mЗабросaОтносительноУстановившегосяA1
                Parent.TracingDecodingArrow(
                        Parent.XAxisTime.Range.Maximum - 5.0,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    Parent.XAxisTime.Range.Maximum,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Inclined,
                    $"{parameter}:уст.={Round(.Astop, 2)} дел.")
            End With

            ' нахождение минимального и максимального значения параметра a1
            Dim mМинимальноеМаксимальноеЗначениеA1 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mМинимальноеМаксимальноеЗначениеA1
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        .TimeMinValue,
                        CastToAxesStandard(.IndexParameter, .MinValue),
                        ArrowType.Vertical,
                        parameter & ":max-min=" & Round(.MaxValue - .MinValue, 2) & " дел.")

                    Protocol(6, 2) = Round(.MaxValue - .MinValue, 2) & " дел."
                    minTransferA1 = IntermediateRotationReducedAlpha(minTransferN1, pointsA1)
                    maxTransferA1 = IntermediateRotationReducedAlpha(maxTransferN1, pointsA1)
                    ' Вождение Угла А1 Без N1
                    Protocol(7, 2) = Round((.MaxValue - .MinValue) - (maxTransferA1 - minTransferA1), 2) & " дел."
                End If
            End With
        End If

        ' заброс a2 относительно установившегося
        parameter = cona2
        Dim mЗабросaОтносительноУстановившегосяA2 = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        mЗабросaОтносительноУстановившегосяA2.Calculation()
        If mЗабросaОтносительноУстановившегосяA2.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mЗабросaОтносительноУстановившегосяA2.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mЗабросaОтносительноУстановившегосяA2
                Parent.TracingDecodingArrow(
                    Parent.XAxisTime.Range.Maximum - 5,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    Parent.XAxisTime.Range.Maximum,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Inclined,
                    $"{parameter}:уст.={Round(.Astop, 2)} дел.")
            End With

            ' нахождение минимального и максимального значения параметра a2
            Dim mМинимальноеМаксимальноеЗначениеA2 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mМинимальноеМаксимальноеЗначениеA2
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        .TimeMinValue,
                        CastToAxesStandard(.IndexParameter, .MinValue),
                        ArrowType.Vertical,
                        parameter & ":max-min=" & Round(.MaxValue - .MinValue, 2) & " дел.")

                    Protocol(10, 2) = Round(.MaxValue - .MinValue, 2) & " дел."
                    minTransferA2 = IntermediateRotationReducedAlpha(minTransferN2, pointsA2)
                    maxTransferA2 = IntermediateRotationReducedAlpha(maxTransferN2, pointsA2)
                    ' Вождение Угла А2 Без N2
                    Protocol(11, 2) = Round((.MaxValue - .MinValue) - (maxTransferA2 - minTransferA2), 2) & " дел."
                End If
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class