Imports System.Math
Imports MathematicalLibrary

Friend Class AnalysisПриемистостьМГ_РУД67
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
        PopulateProtocol(4, {"Автоматика изделия и НА сработали", "По ТУ", ""})
        PopulateProtocol(5, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(6, {"t руд", "сек в ТУ", GetEngineNormTUParameter(32)})
        PopulateProtocol(7, {"t приём.", "сек в ТУ", GetEngineNormTUParameter(28)})
        PopulateProtocol(8, {"Заброс N1", "Нет", GetEngineNormTUParameter(12)})
        PopulateProtocol(9, {"Провал N1", "% в ТУ", GetEngineNormTUParameter(14)})
        PopulateProtocol(10, {"Заброс N2", "Нет", GetEngineNormTUParameter(13)})
        PopulateProtocol(11, {"Заброс Ттвг", "Нет", GetEngineNormTUParameter(36)})
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
            .Astart = 17
            .Astop = 60
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
                Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."
            End With

            ' время восстановления по N1 минус 2%
            parameter = conN1
            Dim mДлительностьФронтаОтИндексаДоN1Уст_2 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаОтИндексаДоN1Уст_2, parameter), ДлительностьФронтаОтИндексаДоN1Уст_2)
            With mДлительностьФронтаОтИндексаДоN1Уст_2
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
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
                        $"{parameter}уст-2%:dT={Round(.TimeDuration, 2)} сек.")
                    Protocol(7, 2) = $"{Round(.TimeDuration, 2)} сек."
                End If
            End With
        End If

        ' вычисление заброс N1
        parameter = conN1
        Dim mДлительностьЗабросаПровалаN1 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаN1
            .Astart = 50
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
                Protocol(8, 2) = $"{Round(.MaxValue - .Astop, 2)} %"
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
                Protocol(10, 2) = $"{Round(.MaxValue - .Astop, 2)} %"
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
                Protocol(11, 2) = $"{overshootT4} гр."
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
                Protocol(9, 2) = $"{Round(.DeltaA, 2)} %"
            End If
        End With

        ' нахождение минимального значения параметра Рт за НП-96М
        parameter = conРтзаНП96М
        Dim mМинимальноеЗначениеРтзаНП96М = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
        With mМинимальноеЗначениеРтзаНП96М
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .TimeMinValue,
                    CastToAxesStandard(.IndexParameter, .MinValue - 2),
                    .TimeMinValue,
                    CastToAxesStandard(.IndexParameter, .MinValue + 2),
                    ArrowType.Inclined,
                    $"{parameter}:минимум={Round(.MinValue, 2)} кг")
            End If
        End With

        ' значение ДиаметрРС в конце
        parameter = conДиаметрРС
        Dim mЗначениеПараметраДиаметрРСВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
        With mЗначениеПараметраДиаметрРСВИндексе
            .IndexTstart = CInt(Parent.XAxisTime.Range.Maximum - 1)
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .ParameterValue - 2),
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .ParameterValue + 2),
                    ArrowType.Inclined,
                    $"{parameter}={Round(.ParameterValue, 2)} дел.")
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class