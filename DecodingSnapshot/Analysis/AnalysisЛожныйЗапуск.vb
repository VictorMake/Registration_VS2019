Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 6 '"Ложный запуск"
''' </summary>
Friend Class AnalysisЛожныйЗапуск
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 8, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"N2физ.", "%", GetEngineNormTUParameter(50)})
        PopulateProtocol(6, {"Ртбр", "кг/см2", GetEngineNormTUParameter(39)})
        PopulateProtocol(7, {"РтОК", "кг/см2", GetEngineNormTUParameter(20)})
        PopulateProtocol(8, {"t бр", "сек в ТУ", GetEngineNormTUParameter(40)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' вычисление длительность заброса
        parameter = conРтОК1К
        Dim mДлительностьЗабросаПровалаРтОК1К = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаРтОК1К
            .Astart = 5
            .Astop = 7
            .Calculation()
        End With

        If mДлительностьЗабросаПровалаРтОК1К.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьЗабросаПровалаРтОК1К.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьЗабросаПровалаРтОК1К
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
                Protocol(8, 2) = Round(.TimeDuration, 2) & " сек."

                ' отрисовать риску максимального значения
                Parent.TracingDecodingArrow(
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue - 2),
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue + 2),
                    ArrowType.Inclined,
                    $"{parameter}:максимум={Round(.MaxValue, 2)} кг")
                Protocol(6, 2) = "максимум=" & Round(.MaxValue, 2) & " кг"
            End With

            ' вычисление время первого спада ниже уровня 1
            Dim mДлительностьФронтаСпадаОтИндексаДоУровняРтОК1К = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            With mДлительностьФронтаСпадаОтИндексаДоУровняРтОК1К
                .IndexTstart = mДлительностьЗабросаПровалаРтОК1К.IndexTstop
                .Astop = 1
                .Calculation()
            End With
            If mДлительностьФронтаСпадаОтИндексаДоУровняРтОК1К.IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += mДлительностьФронтаСпадаОтИндексаДоУровняРтОК1К.ErrorsMessage & vbCrLf
            Else
                Dim mЗначениеПараметраВИндексеРтОК1К = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
                With mЗначениеПараметраВИндексеРтОК1К
                    .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняРтОК1К.IndexTstop - 5 * Parent.FrequencyBackgroundSnapshot
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
                            $"{parameter}={Round(.ParameterValue, 2)} кг")
                        Protocol(7, 2) = Round(.ParameterValue, 2) & " кг"
                    End If
                End With
            End If
        End If

        ' нахождение минимального и максимального значения параметра N2
        parameter = conN2
        Dim mМаксимальноеЗначениеN2 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
        With mМаксимальноеЗначениеN2
            .Calculation()
            If .IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += .ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                Parent.TracingDecodingArrow(
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue - 2),
                    .TimeMaxValue,
                    CastToAxesStandard(.IndexParameter, .MaxValue + 2),
                    ArrowType.Inclined,
                    $"{parameter}:максимум={Round(.MaxValue, 2)} %")
                Protocol(5, 2) = "максимум=" & Round(.MaxValue, 2) & " %"
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class