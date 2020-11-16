Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 11 '"Включение КО на N2=90"
''' </summary>
Friend Class AnalysisВключениеКОнаN290
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
        PopulateProtocol(4, {"Автоматика изделия и НА сработали", "По ТУ", ""})
        PopulateProtocol(5, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(6, {"t восстановления режима", "сек в ТУ", GetEngineNormTUParameter(43)})
        PopulateProtocol(7, {"t выдачи сигнала <Помпаж>", "сек в ТУ", GetEngineNormTUParameter(45)})
        PopulateProtocol(8, {"t стендовой блокировки", "сек в ТУ", GetEngineNormTUParameter(53)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время Помпаж
        ' параметр = conКлапанСУНА для теста
        parameter = conПомпаж
        Dim mДлительностьЗабросаПровалаПомпаж = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаПомпаж
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
                Protocol(7, 2) = Round(.TimeDuration, 2) & " сек."
            End If
        End With

        ' прохождение сигнала КО
        parameter = conКлапанКО
        Dim mДлительностьЗабросаПровалаКлапанКО = CType(mFiguresManager(EnumFigures.ДлительностьЗабросаПровала, parameter), ДлительностьЗабросаПровала)
        With mДлительностьЗабросаПровалаКлапанКО
            .Astart = 1
            .Astop = 4.99
            .Calculation()
        End With

        If mДлительностьЗабросаПровалаКлапанКО.IsErrors Then
            ' анализ для последующих построений, накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += mДлительностьЗабросаПровалаКлапанКО.ErrorsMessage & vbCrLf
        Else
            ' отрисовать стрелки
            With mДлительностьЗабросаПровалаКлапанКО
                Parent.TracingDecodingArrow(
                .Tstart,
                CastToAxesStandard(.IndexParameter, .Astop),
                .Tstop,
                CastToAxesStandard(.IndexParameter, .Astop),
                ArrowType.Horizontal,
                $"{parameter}:dT={Round(.TimeDuration, 2)} сек.")
            End With

            ' время восстановления по N1 минус 2%
            parameter = conN1
            Dim mДлительностьФронтаОтИндексаДоN1Уст_2 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаОтИндексаДоN1Уст_2, parameter), ДлительностьФронтаОтИндексаДоN1Уст_2)
            With mДлительностьФронтаОтИндексаДоN1Уст_2
                .IndexTstart = mДлительностьЗабросаПровалаКлапанКО.IndexTstop 'ИндексТначальное
                .Calculation()
                If .IsErrors Then
                    ' анализ для последующих построений
                    ' накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += .ErrorsMessage & vbCrLf
                Else
                    ' отрисовать стрелки
                    Parent.TracingDecodingArrow(
                        mДлительностьЗабросаПровалаКлапанКО.Tstart,
                        CastToAxesStandard(.IndexParameter, .Astart),
                        .Tstop,
                        CastToAxesStandard(.IndexParameter, .Astop),
                        ArrowType.Horizontal,
                        $"{parameter}уст-2%:dT={Round(.Tstop - mДлительностьЗабросаПровалаКлапанКО.Tstart, 2)} сек.")
                    Protocol(6, 2) = Round(.Tstop - mДлительностьЗабросаПровалаКлапанКО.Tstart, 2) & " сек."
                End If
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class