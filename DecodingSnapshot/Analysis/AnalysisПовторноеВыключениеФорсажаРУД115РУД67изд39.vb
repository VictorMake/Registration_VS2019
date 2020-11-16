Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 27 '"Повторное выключение Форсажа РУД115 (106) -> РУД67 изделия 39"
''' </summary>
Friend Class AnalysisПовторноеВыключениеФорсажаРУД115РУД67изд39
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        Re.Dim(Protocol, 7, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(Parent.NumberProductionSnapshot), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
        PopulateProtocol(3, {"Температура бокса", TemperatureOfBox & "град.", ""})
        PopulateProtocol(4, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(5, {"t руд", "сек в ТУ", GetEngineNormTUParameter(32)})
        PopulateProtocol(6, {"Провал N1 от устан. значения", "%", ""})
        PopulateProtocol(7, {"Провал N2 от устан. значения", "%", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время спада
        parameter = conаРУД
        Dim mДлительностьФронтаСпадаРУД = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУД
            .Astart = GetRud()
            .Astop = 73
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
                Protocol(5, 2) = Round(.TimeDuration, 2) & " сек."
            End With
        End If

        ' провал N1 относительно установившегося
        parameter = conN1
        Dim mПровалN1ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ПровалN1ОтносительноУстановившегося, parameter), ПровалN1ОтносительноУстановившегося)
        With mПровалN1ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
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
                    ArrowType.Vertical,
                    $"{parameter}:провал={Round(.DeltaA, 2)} %")
                Protocol(6, 2) = $"{Round(.DeltaA, 2)} %"
            End If
        End With

        ' провал N2 относительно установившегося
        parameter = conN2
        Dim mПровалN2ОтносительноУстановившегося = CType(mFiguresManager(EnumFigures.ПровалN1ОтносительноУстановившегося, parameter), ПровалN1ОтносительноУстановившегося)
        With mПровалN2ОтносительноУстановившегося
            .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstop
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
                    ArrowType.Vertical,
                    $"{parameter}:уст. провал={Round(.DeltaA, 2)} %")
                Protocol(7, 2) = $"{Round(.DeltaA, 2)} %"
            End If
        End With

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class