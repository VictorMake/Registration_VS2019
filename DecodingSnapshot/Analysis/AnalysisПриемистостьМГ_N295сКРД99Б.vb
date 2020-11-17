﻿Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 6 '"Приемистость МГ -> N2=95 с КРД99Б"
''' </summary>
Friend Class AnalysisПриемистостьМГ_N295сКРД99Б
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
        PopulateProtocol(6, {"t РтНп", "сек в ТУ", GetEngineNormTUParameter(37)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' вначале нахожу конец(начало) спада РУД
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
                Protocol(5, 2) = $"{Round(.TimeDuration, 2)} сек."
            End With

            parameter = conРтзаНП96М
            Dim mПровалЗаНП96 = CType(mFiguresManager(EnumFigures.ПровалЗаНП96, parameter), ПровалЗаНП96)
            With mПровалЗаНП96
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
                .Astop = 120
                .DecreaseAverage = 5
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
                    Protocol(6, 2) = $"{Round(.TimeDuration, 2)} сек."
                End If
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class