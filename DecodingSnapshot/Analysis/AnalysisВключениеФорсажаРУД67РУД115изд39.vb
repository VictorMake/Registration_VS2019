Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 23 '"Включение Форсажа РУД67 -> РУД115 (106) изделия 39"
''' </summary>
Friend Class AnalysisВключениеФорсажаРУД67РУД115изд39
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 15, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"t руд", "сек в ТУ", GetEngineNormTUParameter(32)})
        PopulateProtocol(6, {"t вкл Форсажа", "сек в ТУ", GetEngineNormTUParameter(26)})
        PopulateProtocol(7, {"t выхода на max Форсажа", "сек в ТУ", GetEngineNormTUParameter(27)})
        PopulateProtocol(8, {"J1 ионизации", "мкА", GetEngineNormTUParameter(38)})
        PopulateProtocol(9, {"J2 ионизации", "мкА", GetEngineNormTUParameter(38)})
        PopulateProtocol(10, {"Заброс N1 от устан. значения", "%", ""})
        PopulateProtocol(11, {"Заброс N1 от КРД", "%", GetEngineNormTUParameter(12)})
        PopulateProtocol(12, {"Заброс N2 от КРД", "Нет", GetEngineNormTUParameter(13)})
        PopulateProtocol(13, {"Провал N2 от устан. значения", "%", ""})
        PopulateProtocol(14, {"dN=dN1прев.+dN2пров.", "% в ТУ", GetEngineNormTUParameter(16)})
        PopulateProtocol(15, {"Заброс Ттвг", "Нет", GetEngineNormTUParameter(36)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim mFormAdjustment As New FormAdjustment(Parent.TypeKRDinSnapshot)
        mFormAdjustment.ShowDialog()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' риски настройки КРД параметра N1
        parameter = conN1
        Dim mРискиНастроекПараметровN1 = CType(mFiguresManager(EnumFigures.РискиНастроекПараметров, parameter), РискиНастроекПараметров)
        With mРискиНастроекПараметровN1
            .Calculation()
            If .IsErrors = False Then
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.N1TuningKrd),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.N1TuningKrd),
                    ArrowType.Inclined,
                    $"настройка N1 КРД={CStr(mFormAdjustment.N1TuningKrd)} %")
            End If
        End With

        ' риски настройки КРД параметра N2
        parameter = conN2
        Dim mРискиНастроекПараметровN2 = CType(mFiguresManager(EnumFigures.РискиНастроекПараметров, parameter), РискиНастроекПараметров)
        With mРискиНастроекПараметровN2
            .Calculation()
            If .IsErrors = False Then
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.N2TuningKrd),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.N2TuningKrd),
                    ArrowType.Inclined,
                    $"настройка N2 КРД={CStr(mFormAdjustment.N2TuningKrd)} %")
            End If
        End With

        ' риски настройки КРД параметра Т4
        parameter = conT4
        Dim mРискиНастроекПараметровТ4 = CType(mFiguresManager(EnumFigures.РискиНастроекПараметров, parameter), РискиНастроекПараметров)
        With mРискиНастроекПараметровТ4
            .Calculation()
            If .IsErrors = False Then
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.T4Krd),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, mFormAdjustment.T4Krd),
                    ArrowType.Inclined,
                    $"настройка Т4 КРД={CStr(mFormAdjustment.T4Krd)} гр.")
            End If
        End With

        ' время приемистости
        parameter = conаРУД
        Dim mДлительностьФронтаСпадаРУД = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУД
            .Astart = 73
            .Astop = GetRud()
            .Calculation()
        End With

        If mДлительностьФронтаСпадаРУД.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьФронтаСпадаРУД.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьФронтаСпадаРУД
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astart),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(5, 2) = Round(.TimeDuration, 2) & " сек."
            End With

            ' вычисление время первой форсажной приемистости
            parameter = conТокJправый
            Dim mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            Dim mМаксимальноеЗначениеТокJправый = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                .Astop = 60
                .Calculation()
            End With
            If mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.ErrorsMessage & vbCrLf
            Else
                ' нахождение минимального и максимального значения параметра ТокJправый
                With mМаксимальноеЗначениеТокJправый
                    .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                    .Calculation()
                End With
            End If

            ' наибольшее
            parameter = conТокJлевый
            Dim mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            Dim mМаксимальноеЗначениеТокJлевый = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
            With mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                .Astop = 60
                .Calculation()
            End With
            If mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.ErrorsMessage & vbCrLf
            Else
                ' нахождение минимального и максимального значения параметра ТокJправый
                With mМаксимальноеЗначениеТокJлевый
                    .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                    .Calculation()
                End With
            End If

            If mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.IsErrors = False AndAlso mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.IsErrors = False Then
                If mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.TimeDuration <= mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.TimeDuration Then
                    ' отрисовать(стрелки)
                    With mДлительностьФронтаСпадаОтИндексаДоУровняТокJправый
                        Parent.TracingDecodingArrow(
                            .Tstart,
                            CastToAxesStandard(.IndexParameter, .Astart),
                            .Tstop,
                            CastToAxesStandard(.IndexParameter, .Astop),
                            ArrowType.Horizontal,
                            $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                        Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."
                    End With
                Else
                    ' отрисовать стрелки
                    With mДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый
                        Parent.TracingDecodingArrow(
                            .Tstart,
                            CastToAxesStandard(.IndexParameter, .Astart),
                            .Tstop,
                            CastToAxesStandard(.IndexParameter, .Astop),
                            ArrowType.Horizontal,
                            $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                        Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."
                    End With
                End If
                Protocol(8, 2) = Round(mМаксимальноеЗначениеТокJправый.MaxValue, 2) & " мка."
                Protocol(9, 2) = Round(mМаксимальноеЗначениеТокJлевый.MaxValue, 2) & " мка."
            End If

            ' вычисление время второй форсажной приемистости
            parameter = conПолныйФорсаж
            Dim mДлительностьФронтаСпадаОтИндексаДоУровняМСТ = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            With mДлительностьФронтаСпадаОтИндексаДоУровняМСТ
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                .Astop = 4
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .Astart),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Horizontal,
                        $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                    Protocol(7, 2) = Round(.TimeDuration, 2) & " сек."
                End If
            End With
        End If

        ' вычисление заброс N1
        parameter = conN1
        Dim mДлительностьЗабросаПровалаN1 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаN1
            '.Astart = 50
            .IndexStart = mДлительностьФронтаСпадаРУД.IndexTstop
            .Astop = mFormAdjustment.N1TuningKrd
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue),
                    ArrowType.Vertical,
                    $"{parameter}:заброс={Round(.MaxValue - .Astop, 2)} %")
                Protocol(11, 2) = $"{Round(.MaxValue - .Astop, 2)} %"
            End If
        End With

        ' вычисление заброс N2
        parameter = conN2
        Dim mДлительностьЗабросаПровалаN2 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаN2
            .Astart = 50
            .Astop = mFormAdjustment.N2TuningKrd
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue),
                    ArrowType.Vertical,
                    $"{parameter}:заброс={Round(.MaxValue - .Astop, 2)} %")
                Protocol(12, 2) = $"{Round(.MaxValue - .Astop, 2)} %"
            End If
        End With

        ' вычисление заброс Т4
        parameter = conT4
        Dim mДлительностьЗабросаПровалаТ4 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаТ4
            .Astart = 700
            .Astop = mFormAdjustment.T4Krd
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений
            Else
                ' отрисовать стрелки
                Dim overshootT4 As Double = Round(.MaxValue - .Astop, 2) ' Заброс T4
                If overshootT4 > 0 AndAlso overshootT4 < 3 Then
                    overshootT4 = 0
                Else
                    Parent.TracingDecodingArrow(
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        ArrowType.Vertical,
                        $"{parameter}:заброс={overshootT4} гр.")
                End If
                Protocol(15, 2) = $"{overshootT4} гр."
            End If
        End With

        ' провал N1относительно установившегося
        parameter = conN1
        Dim mПровалN1ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ПровалN1ОтносительноУстановившегося, parameter), ПровалN1ОтносительноУстановившегося)
        With mПровалN1ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astart),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Vertical,
                    $"{parameter}:провал={Round(.DeltaA, 2)} %")
                'Protocol(10, 2) = параметр & ":провал=" & Round(.DeltaA, 2) & " %"
            End If
        End With

        ' провал N2 относительно установившегося
        parameter = conN2
        Dim mПровалN2ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ПровалN1ОтносительноУстановившегося, parameter), ПровалN1ОтносительноУстановившегося)
        With mПровалN2ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astart),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Vertical,
                    $"{parameter}:уст. провал={Round(.DeltaA, 2)} %")
                Protocol(13, 2) = $"{Round(.DeltaA, 2)} %"
            End If
        End With

        ' заброс N1 относительно установившегося
        parameter = conN1
        Dim mЗабросN1ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        With mЗабросN1ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                If .DeltaA > 0 Then
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .Astart),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Vertical,
                        $"{parameter}:уст. заброс={Round(.DeltaA, 2)} %")
                    Protocol(10, 2) = $"{Round(.DeltaA, 2)} %"
                End If
            End If
        End With

        Dim dN As Double
        If mЗабросN1ОтносительноУстановившегося.DeltaA > 0 AndAlso mПровалN2ОтносительноУстановившегося.DeltaA > 0 Then
            dN = mЗабросN1ОтносительноУстановившегося.DeltaA + mПровалN2ОтносительноУстановившегося.DeltaA
        ElseIf mЗабросN1ОтносительноУстановившегося.DeltaA > 0 AndAlso mПровалN2ОтносительноУстановившегося.DeltaA <= 0 Then
            dN = mЗабросN1ОтносительноУстановившегося.DeltaA
        ElseIf mЗабросN1ОтносительноУстановившегося.DeltaA <= 0 AndAlso mПровалN2ОтносительноУстановившегося.DeltaA > 0 Then
            dN = mПровалN2ОтносительноУстановившегося.DeltaA
        End If
        Protocol(14, 2) = $"dN={Round(dN, 2)} %"

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class