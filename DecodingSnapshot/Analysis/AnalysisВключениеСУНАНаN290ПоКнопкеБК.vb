Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 8 '"Включение СУНА на N2=90 по кнопке БК"
''' </summary>
Friend Class AnalysisВключениеСУНАНаN290ПоКнопкеБК
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
        PopulateProtocol(5, {"N2привед.", "%", ""})
        PopulateProtocol(6, {"t БК", "сек", GetEngineNormTUParameter(52)})
        PopulateProtocol(7, {"t СУНА", "сек в ТУ", GetEngineNormTUParameter(41)})
        PopulateProtocol(8, {"dРС", "дел. В ТУ", GetEngineNormTUParameter(48)})
        PopulateProtocol(9, {"a2", "дел. В ТУ", GetEngineNormTUParameter(49)})
        PopulateProtocol(10, {"Автоматика а1=f(N2прив)", "По ТУ", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' вычисление длительность КнопкаБК
        parameter = conКнопкаБК
        Dim mДлительностьЗабросаПровалаБК = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаБК
            .Astart = 1
            .Astop = 4.99
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."
            End If
        End With

        ' вычисление длительность КлапанСУНА
        parameter = conКлапанСУНА
        Dim mДлительностьЗабросаПровалаКлапанСУНА = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаКлапанСУНА
            .Astart = 1
            .Astop = 4.99
            .Calculation()
        End With
        If mДлительностьЗабросаПровалаКлапанСУНА.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьЗабросаПровалаКлапанСУНА.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьЗабросаПровалаКлапанСУНА
                Parent.TracingDecodingArrow(
                .Tstart,
                CastToAxesStandard(.IndexParameter, .Astop),
                .Tstop,
                CastToAxesStandard(.IndexParameter, .Astop),
                ArrowType.Horizontal,
                $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(7, 2) = Round(.TimeDuration, 2) & " сек."
            End With

            ' провал ДиаметрРС относительно установившегося здесь нужно значение за последние 2 сек
            parameter = conДиаметрРС
            Dim mПровалДиаметрРСОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ПровалN1ОтносительноУстановившегося, parameter), ПровалN1ОтносительноУстановившегося)
            With mПровалДиаметрРСОтносительноУстановившегося
                .IndexTstart = mДлительностьЗабросаПровалаКлапанСУНА.IndexTstart
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' значение ДиаметрРС в точке окончания СУНА
                    Dim mЗначениеПараметраВИндексеДиаметрРС = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
                    With mЗначениеПараметраВИндексеДиаметрРС
                        .IndexTstart = mДлительностьЗабросаПровалаКлапанСУНА.IndexTstop - 1S
                        .Calculation()
                    End With
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        mДлительностьЗабросаПровалаКлапанСУНА.Tstop - 1 / Parent.FrequencyBackgroundSnapshot,
                        CastToAxesStandard(.IndexParameter, mЗначениеПараметраВИндексеДиаметрРС.ParameterValue),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Vertical,
                        $"{parameter}:dA={Round(mЗначениеПараметраВИндексеДиаметрРС.ParameterValue - .Astop, 2)} дел.")
                    Protocol(8, 2) = $":dA={Round(mЗначениеПараметраВИндексеДиаметрРС.ParameterValue - .Astop, 2)} дел."
                End If
            End With

            ' значение a2 в точке окончания СУНА
            parameter = cona2
            Dim mЗначениеПараметраА2ВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
            With mЗначениеПараметраА2ВИндексе
                .IndexTstart = CInt(mДлительностьЗабросаПровалаКлапанСУНА.IndexTstop - 1)
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        mДлительностьЗабросаПровалаКлапанСУНА.Tstop - 1 / Parent.FrequencyBackgroundSnapshot,
                        CastToAxesStandard(.IndexParameter, .ParameterValue - 2),
                        mДлительностьЗабросаПровалаКлапанСУНА.Tstop - 1 / Parent.FrequencyBackgroundSnapshot,
                        CastToAxesStandard(.IndexParameter, .ParameterValue + 2),
                        ArrowType.Inclined,
                        $"{parameter}:={Round(.ParameterValue, 2)} дел.")
                    Protocol(9, 2) = Round(.ParameterValue, 2) & " дел."
                End If
            End With

            ' значение N2приведенное в точке перед СУНА
            parameter = conN2
            Dim mЗначениеПараметраN2ВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
            With mЗначениеПараметраN2ВИндексе
                .IndexTstart = mДлительностьЗабросаПровалаКлапанСУНА.IndexTstart - 5S
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    Dim transferConstant As Double = Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin))
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        mДлительностьЗабросаПровалаКлапанСУНА.Tstart - 5 / Parent.FrequencyBackgroundSnapshot,
                        CastToAxesStandard(.IndexParameter, .ParameterValue - 2),
                        mДлительностьЗабросаПровалаКлапанСУНА.Tstart - 5 / Parent.FrequencyBackgroundSnapshot,
                        CastToAxesStandard(.IndexParameter, .ParameterValue + 2),
                        ArrowType.Inclined,
                        $"{parameter}привед:={Round(.ParameterValue * transferConstant, 2)} дел.")
                    Protocol(5, 2) = Round(.ParameterValue * transferConstant, 2) & " дел."
                End If
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class