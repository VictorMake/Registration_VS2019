Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 15 '"Горячий запуск"
''' </summary>
Friend Class AnalysisГорячийЗапуск
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 9, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Запуск прошел", "По ТУ", ""})
        PopulateProtocol(5, {"t2 МГ->откл. стартера", "сек в ТУ", "По графику"})
        PopulateProtocol(6, {"Запальное устройство", "лев/прав/оба", ""})
        PopulateProtocol(7, {"Время запуска", "", ""})
        PopulateProtocol(8, {"T4 максимум", "", ""})
        PopulateProtocol(9, {"t броска РтОК1К", "", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()

        Dim tStartUp As String = Nothing ' Т запуска Верх
        Dim tStartBottom As String = Nothing ' Т запуска Низ
        Dim timeStart As Double ' Т запуска
        Dim pointsStartUp(3), pointsStartBottom(3) As PointF

        pointsStartUp(1).X = -38
        pointsStartUp(1).Y = 3.8
        pointsStartUp(2).X = 26
        pointsStartUp(2).Y = 6
        pointsStartUp(3).X = 40
        pointsStartUp(3).Y = 6

        pointsStartBottom(1).X = -40
        pointsStartBottom(1).Y = 3
        pointsStartBottom(2).X = 0
        pointsStartBottom(2).Y = 3
        pointsStartBottom(3).X = 30
        pointsStartBottom(3).Y = 4
        ' по верху
        If TemperatureBoxInSnaphot < 26.0 Then
            tStartUp = CStr(Round(LinearInterpolation(TemperatureBoxInSnaphot, pointsStartUp(1).X, pointsStartUp(1).Y, pointsStartUp(2).X, pointsStartUp(2).Y), 1))
        ElseIf TemperatureBoxInSnaphot >= 26.0 Then
            tStartUp = CStr(pointsStartUp(3).Y)
        End If
        ' по низу
        If TemperatureBoxInSnaphot < 0 Then
            tStartBottom = CStr(pointsStartBottom(1).Y)
        ElseIf TemperatureBoxInSnaphot >= 0 Then
            tStartBottom = CStr(Round(LinearInterpolation(TemperatureBoxInSnaphot, pointsStartBottom(2).X, pointsStartBottom(2).Y, pointsStartBottom(3).X, pointsStartBottom(3).Y), 1))
        End If
        ' для изделия 99 время запуска от кнопки запуск до 67 %
        timeStart = Round(LinearInterpolation(TemperatureBoxInSnaphot, -40, 34, 40, 50.8), 1)
        Protocol(7, 3) = timeStart.ToString & " сек."
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

#Region "1.	Замерить по осциллограмме время t2 набора частоты вращения от момента N2=53% до выхода на оборты N2 пр.=67%"
        parameter = conN2
        Dim mДлительностьФронтаСпадаПрОборотов = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаПрОборотов, parameter), ДлительностьФронтаСпадаПрОборотов)
        With mДлительностьФронтаСпадаПрОборотов
            .Temperature = TemperatureBoxInSnaphot
            .Astart = 53.0
            .Astop = 67.0 ' приведенный в вычислении будет уменьшен на transferConstant
            .Calculation()
        End With
        If mДлительностьФронтаСпадаПрОборотов.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьФронтаСпадаПрОборотов.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьФронтаСпадаПрОборотов
                Parent.TracingDecodingArrow(
                    .Tstart,
                    CastToAxesStandard(.IndexParameter, .Astart),
                    .Tstop,
                    CastToAxesStandard(.IndexParameter, .Astop),
                    ArrowType.Horizontal,
                    $"{parameter}:53физ-67пр dT={Round(.TimeDuration, 2)} сек.")
                Protocol(5, 2) = Round(.TimeDuration, 2) & " сек."
                Protocol(5, 3) = "от " & tStartBottom & " до " & tStartUp & " сек."
            End With

            parameter = conЗапуск
            Dim mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            With mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск
                .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.GraphMinimum + 1
                .Astop = 4
                .Calculation()
            End With
            If mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IsErrors Then
                ' анализ для последующих построений, накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.ErrorsMessage & vbCrLf
            Else
                ' отрисовать стрелки
                With mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск
                    Parent.TracingDecodingArrow(
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astart),
                        mДлительностьФронтаСпадаПрОборотов.Tstop,
                        CastToAxesStandard(mДлительностьФронтаСпадаПрОборотов.IndexParameter, mДлительностьФронтаСпадаПрОборотов.Astop),
                        ArrowType.Horizontal,
                        $"{parameter}:dT={Round(mДлительностьФронтаСпадаПрОборотов.Tstop - .Tstop, 2)} сек.")
                    Protocol(7, 2) = Round(mДлительностьФронтаСпадаПрОборотов.Tstop - .Tstop, 2) & " сек."
                End With
#End Region

#Region "2.	Оценить начало роста Т4, которое должно быть не ниже окончания броска."
                parameter = conT4
                ' нахождение максимального значения параметра T4
                Dim mМаксимальноеЗначениеT4 = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
                With mМаксимальноеЗначениеT4
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
                            $"{parameter}:максимум={Round(.MaxValue, 2)} град.")
                        Protocol(8, 2) = Round(.MaxValue, 2) & " град."
                    End If
                End With

                ' параметр = "N2>53%" тест
                Dim mЗначениеПараметраТ4ВИндексе = CType(mFiguresManager(EnumFigures.ЗначениеПараметраВИндексе, parameter), ЗначениеПараметраВИндексе)
                With mЗначениеПараметраТ4ВИндексе
                    .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IndexTstop
                    .Calculation()
                End With
                If mЗначениеПараметраТ4ВИндексе.IsErrors Then
                    ' анализ для последующих построений, накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += mЗначениеПараметраТ4ВИндексе.ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                End If

                Dim mДлительностьФронтаСпадаОтИндексаДоУровняТ4 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
                With mДлительностьФронтаСпадаОтИндексаДоУровняТ4
                    .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IndexTstop
                    .Astop = mЗначениеПараметраТ4ВИндексе.ParameterValue + 5 ' начало стабильного роста Т4
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                    End If
                End With

                ' вычисление длительность первого спада после максимума
                parameter = conРтОК1К
                Dim mДлительностьЗабросаПровалаРтОК1К = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
                With mДлительностьЗабросаПровалаРтОК1К
                    .Astart = 12
                    .Astop = 11.9
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                    End If

                    If mДлительностьФронтаСпадаОтИндексаДоУровняТ4.Tstop > .Tstart Then
                        ' плохо температура растет позже первого спада Рт ОК1К рисуем стрелку
                        ' отрисовать стрелки
                        Parent.TracingDecodingArrow(
                            .Tstart,
                            CastToAxesStandard(.IndexParameter, .Astart),
                            mДлительностьФронтаСпадаОтИндексаДоУровняТ4.Tstop,
                            CastToAxesStandard(mДлительностьФронтаСпадаОтИндексаДоУровняТ4.IndexParameter, mДлительностьФронтаСпадаОтИндексаДоУровняТ4.Astop),
                            ArrowType.Horizontal,
                            $"{conT4}:dTзадержки={Round(mДлительностьФронтаСпадаОтИндексаДоУровняТ4.Tstop - .Tstart, 2)} сек.")
                    End If
                End With
#End Region

#Region "длительность первого броска РтОК1К"
                ' вычисление длительность первого максимума в промежутке от включения кнопки "Запуск" до T4 максимальное(бросков может быть 2, но первый самый большой)
                Dim mМаксимальноеЗначениеРтОК1К = CType(mFiguresManager(EnumFigures.МинимальноеМаксимальноеЗначениеПараметра, parameter), МинимальноеМаксимальноеЗначениеПараметра)
                With mМаксимальноеЗначениеРтОК1К
                    .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IndexTstop
                    .GraphMaximum = mМаксимальноеЗначениеT4.mIndexMaxValue
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                    End If
                End With
                ' вычисление времени от перехода значения от полки на величину увеличения полка + 0.3
                Dim mДлительностьОтИндексаДоСтабильногоРостаРтОК1К = CType(mFiguresManager(EnumFigures.ДлительностьОтИндексаДоСтабильногоРоста, parameter), ДлительностьОтИндексаДоСтабильногоРоста)
                With mДлительностьОтИндексаДоСтабильногоРостаРтОК1К
                    .IndexTstart = mДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IndexTstop
                    .GraphMaximum = mМаксимальноеЗначениеРтОК1К.IndexMaxValue
                    .ThresholdGrowthFromMinimal = 0.3
                    .Calculation()
                    If .IsErrors Then
                        ' анализ для последующих построений, накапливаем ошибку
                        IsTotalErrors = True
                        totalErrorsMessage += .ErrorsMessage & vbCrLf
                    Else
                        ' отрисовать стрелки
                        Dim TMaxРтОК1К As Double = mМаксимальноеЗначениеРтОК1К.IndexMaxValue * .ClockPeriod
                        Parent.TracingDecodingArrow(
                            .Tstop,
                            CastToAxesStandard(.IndexParameter, .Astop),
                            TMaxРтОК1К,
                            CastToAxesStandard(.IndexParameter, mМаксимальноеЗначениеРтОК1К.MaxValue),
                            ArrowType.Horizontal,
                            $"{parameter}:dTброска={Round(TMaxРтОК1К - .Tstop, 2)} сек.")
                        Protocol(9, 2) = Round(TMaxРтОК1К - .Tstop, 2) & " сек."
                    End If
                End With
#End Region
            End If
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class