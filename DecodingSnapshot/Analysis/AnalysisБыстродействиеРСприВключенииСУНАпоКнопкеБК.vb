Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 10 '"Быстродействие РС при включении СУНА по кнопке БК"
''' </summary>
Friend Class AnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК
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
        PopulateProtocol(5, {"N2привед.", "%", ""})
        PopulateProtocol(6, {"t РС", "сек", GetEngineNormTUParameter(46)})
        PopulateProtocol(7, {"tБК", "сек в ТУ", GetEngineNormTUParameter(52)})
        PopulateProtocol(8, {"tСУНА", "сек в ТУ", GetEngineNormTUParameter(41)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
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
                Protocol(7, 2) = Round(.TimeDuration, 2) & " сек."
            End With

            ' вычисление длительность КлапанСУНА
            parameter = conКлапанСУНА
            Dim mДлительностьЗабросаПровалаКлапанСУНА = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
            With mДлительностьЗабросаПровалаКлапанСУНА
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
                    Protocol(8, 2) = Round(.TimeDuration, 2) & " сек."
                End If
            End With

            parameter = conДиаметрРС
            Dim mПостроениеНаклонной = CType(mFiguresManager(EnumFigures.ПостроениеНаклонной, parameter), ПостроениеНаклонной)
            With mПостроениеНаклонной
                .ExcessOverAverage = 0.5
                .LevelLine2 = 5
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
                    Protocol(6, 2) = Round(.TimeDuration, 2) & " сек."
                    ' наклонная 1
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .Astart),
                        .ТBx,
                        CastToAxesStandard(.IndexParameter, .MaxValue),
                        ArrowType.Inclined,
                        "")
                    ' наклонная 2 параллельно
                    Parent.TracingDecodingArrow(
                        .Tstart,
                        CastToAxesStandard(.IndexParameter, .AstartPlus5),
                        .TimeMaxValue,
                        CastToAxesStandard(.IndexParameter, .AstartPlus5),
                        ArrowType.Inclined,
                        "")
                End If
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class