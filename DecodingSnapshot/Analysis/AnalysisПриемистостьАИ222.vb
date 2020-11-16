Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 28 '"Приемистость АИ-222"
''' </summary>
Friend Class AnalysisПриемистостьАИ222
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 10, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"t руд", "сек в ТУ", "не более 1 сек"})
        PopulateProtocol(6, {"t nпр. ЗМГ или ПНГ", "%", ""}) 'меняется в программе
        PopulateProtocol(7, {"Заброс nндпр от устан. значения", "%", "<=0.01%"})
        PopulateProtocol(8, {"Заброс ntТНД от устан. значения", "%", ""})
        PopulateProtocol(9, {"Превышение tТНД (790гр.)", "Нет", "не более 1.5 сек"}) ' более 1.5 сек или "Есть" - меняется в программе
        PopulateProtocol(10, {"Автоматика изделия и НА сработали", "По ТУ", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim addition As String = "ПМГ"
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время приемистости
        parameter = conаРУДАИ222
        Dim mДлительностьФронтаСпадаРУДАИ222 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУДАИ222
            .Astart = 20
            .Astop = 65
            .Calculation()
        End With
        If mДлительностьФронтаСпадаРУДАИ222.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьФронтаСпадаРУДАИ222.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьФронтаСпадаРУДАИ222
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astart),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(5, 2) = Round(.TimeDuration, 2) & " сек."
            End With

            ' значение conЗемляАИ222 в начале движения Руд
            parameter = conЗемляАИ222
            Dim mЗначениеПараметраЗемляАИ222ВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
            With mЗначениеПараметраЗемляАИ222ВИндексе
                .IndexTstart = mДлительностьФронтаСпадаРУДАИ222.IndexTstart
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .ParameterValue - 0.5),
                        .Tstart, CastToAxesStandard(.IndexParameter, .ParameterValue + 0.5),
                        ArrowType.Inclined, $"{parameter}={Round(.ParameterValue, 2)}")
                    If .ParameterValue >= 1 Then
                        addition = "ЗМГ"
                    Else
                        addition = "ПМГ"
                    End If
                End If
            End With

            ' время восстановления по conNндпрАИ222 минус 5%
            parameter = conNндпрАИ222
            Dim mДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаОтИндексаДоN1Уст_2, parameter), ДлительностьФронтаОтИндексаДоN1Уст_2)
            With mДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5
                .Percent = 5
                .IndexTstart = mДлительностьФронтаСпадаРУДАИ222.IndexTstart
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
                        $"{parameter}{addition}уст-5%:dT={Round(.TimeDuration, 2)} сек.")
                    Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."

                    If mЗначениеПараметраЗемляАИ222ВИндексе.ParameterValue >= 1 Then
                        ' "ЗМГ"
                        Protocol(6, 3) = "<=6 сек."
                    Else
                        ' "ПМГ"
                        Protocol(6, 3) = "<=4 сек."
                    End If
                    Protocol(6, 1) = parameter & addition
                End If
            End With
        End If

        ' заброс NндпрАИ222 относительно установившегося
        parameter = conNндпрАИ222
        Dim mЗабросNндпрАИ222ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ЗабросN1ОтносительноУстановившегося, parameter), ЗабросN1ОтносительноУстановившегося)
        With mЗабросNндпрАИ222ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУДАИ222.IndexTstop
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
                        $"{parameter}{addition}:уст. заброс={Round(.DeltaA, 2)} %")
                    Protocol(7, 2) = $"{parameter}{addition}:уст. заброс={Round(.DeltaA, 2)} %"
                End If
            End If
        End With

        Dim ТндАИ222_790 As Double = 790
        ' t*тнд
        ' риски ТндАИ222 для величины 790 гр.
        parameter = conТндАИ222
        Dim mРискиНастроекПараметровТндАИ222 = CType(mFiguresManager(EnumFigures.РискиНастроекПараметров, parameter), РискиНастроекПараметров)
        With mРискиНастроекПараметровТндАИ222
            .Calculation()
            If .IsErrors = False Then
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, ТндАИ222_790),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, ТндАИ222_790),
                    ArrowType.Inclined,
                    $"{parameter}={CStr(ТндАИ222_790)} гр.")
            End If
        End With

        ' вычисление заброс conТндАИ222 от величины 790 гр
        parameter = conТндАИ222
        Dim mДлительностьЗабросаПровалаТндАИ222 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаТндАИ222
            .Astart = 600
            .Astop = ТндАИ222_790
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
                    $"{parameter}:заброс={Round(.MaxValue - .Astop, 2)} гр.")
                Protocol(8, 2) = $"{Round(.MaxValue - .Astop, 2)} гр."

                If .MaxValue > ТндАИ222_790 Then
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Horizontal,
                        $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                    Protocol(9, 2) = Round(.TimeDuration, 2) & " сек."
                End If
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class