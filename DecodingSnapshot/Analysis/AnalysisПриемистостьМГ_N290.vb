Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 13 '"Приемистость МГ->N290"
''' </summary>
Friend Class AnalysisПриемистостьМГ_N290
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 6, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"t руд", "сек в ТУ", GetEngineNormTUParameter(32)})
        PopulateProtocol(6, {"tПомпаж", "Нет", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время приемистости
        ' время восстановления по Руд минус 2%
        parameter = conаРУД
        Dim mДлительностьФронтаОтИндексаДоРУДУст_2 = CType(mFiguresManager(EnumFigures.ДлительностьФронтаОтИндексаДоN1Уст_2, parameter), ДлительностьФронтаОтИндексаДоN1Уст_2)
        With mДлительностьФронтаОтИндексаДоРУДУст_2
            .Percent = 2
            .Calculation()
        End With

        Dim mДлительностьФронтаСпадаРУД = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУД
            .Astart = 17
            .Astop = mДлительностьФронтаОтИндексаДоРУДУст_2.Astop ' До установившегося режима
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
                Protocol(5, 2) = $"{Round(.TimeDuration, 2)} сек."
            End If
        End With

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
                Protocol(6, 2) = $"{Round(.TimeDuration, 2)} сек."
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class