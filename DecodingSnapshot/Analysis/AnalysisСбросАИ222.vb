Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 29 '"Сброс АИ-222"
''' </summary>
Friend Class AnalysisСбросАИ222
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        ' EngineDefineTU() <- В базе "ТехническиеУсловия" настроек для АИ222 нет
        ' очистку сделать здесь
        totalErrorsMessage = String.Empty
        IsTotalErrors = False

        Re.Dim(Protocol, 8, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"t руд", "сек в ТУ", "не более 1 сек"})
        PopulateProtocol(6, {"Провал nвдпр ЗМГ или ПНГ", "Нет", ">= 57.5"}) ' ЗМГ - ниже 57.5 или ПНГ - ниже 67.5
        PopulateProtocol(7, {"t сброса", "сек в ТУ", "не более 2 сек"}) ' от начала Руд до Gt=360
        PopulateProtocol(8, {"Автоматика изделия и НА сработали", "По ТУ", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim addition As String = "ПМГ"
        Dim levelNbdpr_67_5 As Double = 67.5 ' Значение Уровня Nвдпр
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время приемистости
        parameter = conаРУДАИ222
        Dim mДлительностьФронтаСпадаРУДАИ222 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУДАИ222
            .Astart = 65
            .Astop = 20
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
                Protocol(5, 2) = $"{Round(.TimeDuration, 2)} сек."
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
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .ParameterValue + 0.5),
                        ArrowType.Inclined,
                        $"{parameter}={Round(.ParameterValue, 2)}")

                    If .ParameterValue >= 1 Then
                        addition = "ЗМГ"
                        levelNbdpr_67_5 = 57.5
                        Protocol(6, 3) = ">= 57.5 %"
                    Else
                        addition = "ПМГ"
                        levelNbdpr_67_5 = 67.5
                        Protocol(6, 3) = ">= 67.5 %"
                    End If
                End If
            End With

            ' вычисление время сброса от начала спада Руд до conGтопливаАИ222 = 360
            parameter = conGтопливаАИ222
            Dim mДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            With mДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня
                .IndexTstart = mДлительностьФронтаСпадаРУДАИ222.IndexTstart
                .Astop = 360
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
                    Protocol(7, 2) = $"{Round(.TimeDuration, 2)} сек."
                End If
            End With
        End If

        ' время провала conNвдпрАИ222 от уровня ЗначениеУровняNвдпр
        parameter = conNвдпрАИ222
        Dim mДлительностьЗабросаПровалаNвдпрАИ222 = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаNвдпрАИ222
            .Astart = 80
            .Astop = levelNbdpr_67_5
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .TimeMinValue,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .TimeMinValue,
                    CastToAxesStandard(.IndexParameter, .MinValue),
                    ArrowType.Vertical,
                    $"{parameter}{addition}:провал={Round(.Astop - .MinValue, 2)} %")
                Protocol(6, 2) = $"{parameter}{addition}:провал={Round(.Astop - .MinValue, 2)}% от уровня {levelNbdpr_67_5}"

                If .MinValue < levelNbdpr_67_5 Then
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Horizontal,
                        parameter & addition & ":dT=" & Round(.TimeDuration, 2) & " сек.")
                End If
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class