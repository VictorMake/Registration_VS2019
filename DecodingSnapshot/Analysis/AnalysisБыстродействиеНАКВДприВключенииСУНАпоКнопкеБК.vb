Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 9 "Быстродействие НА КВД при включении СУНА по кнопке БК"
''' </summary>
Friend Class AnalysisБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 5, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(4, {"tБК", "сек в ТУ", GetEngineNormTUParameter(52)})
        PopulateProtocol(5, {"tСУНА", "сек в ТУ", GetEngineNormTUParameter(41)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        ' вычисление длительность КнопкаБК
        parameter = conКнопкаБК
        Dim mДлительностьЗабросаПровалаБК = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаБК
            .Astart = 1
            .Astop = 4.99
            .Calculation()
        End With
        If mДлительностьЗабросаПровалаБК.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьЗабросаПровалаБК.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьЗабросаПровалаБК
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(4, 2) = Round(.TimeDuration, 2) & " сек."
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
                        $"{parameter}:dT ={Round(.TimeDuration, 2)} сек.")
                    Protocol(5, 2) = Round(.TimeDuration, 2) & " сек."
                End With

                ' значение a2 в точке начала БК
                parameter = cona2
                Dim mЗначениеПараметраА2ВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
                With mЗначениеПараметраА2ВИндексе
                    .IndexTstart = CInt(mДлительностьЗабросаПровалаБК.IndexTstart - 1)
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                        Parent.TracingDecodingArrow(
                            mДлительностьЗабросаПровалаБК.Tstart - 1 / Parent.FrequencyBackgroundSnapshot,
                            CastToAxesStandard(.IndexParameter, .ParameterValue - 2),
                            mДлительностьЗабросаПровалаБК.Tstart - 1 / Parent.FrequencyBackgroundSnapshot,
                            CastToAxesStandard(.IndexParameter, .ParameterValue + 2),
                            ArrowType.Inclined,
                            $"{parameter}:={Round(.ParameterValue, 2)} дел.")
                    End If
                End With

                ' нахождение минимального значения параметра a2 за время СУНА 
                Dim mМинимальноеЗначениеA2 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
                With mМинимальноеЗначениеA2
                    .IndexTstart = mДлительностьЗабросаПровалаКлапанСУНА.IndexTstart
                    .IndexTstop = mДлительностьЗабросаПровалаКлапанСУНА.IndexTstop
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                        Parent.TracingDecodingArrow(
                            mДлительностьЗабросаПровалаБК.Tstart,
                            CastToAxesStandard(.IndexParameter, mЗначениеПараметраА2ВИндексе.ParameterValue),
                            .TimeMinValue,
                            CastToAxesStandard(.IndexParameter, .MinValue),
                            ArrowType.Horizontal,
                            $"{parameter}:мин. dТ={Round(.TimeMinValue - mЗначениеПараметраА2ВИндексе.Tstart, 2)} сек.")
                    End If
                End With
            End If
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class