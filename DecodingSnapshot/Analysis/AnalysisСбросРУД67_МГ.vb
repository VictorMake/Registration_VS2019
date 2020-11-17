Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 4 '"Сброс РУД67 -> МГ"
''' </summary>
Friend Class AnalysisСбросРУД67_МГ
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
        PopulateProtocol(4, {"Автоматика изделия и НА сработали", "По ТУ", ""})
        PopulateProtocol(5, {"Сигналы прошли", "По ТУ", ""})
        PopulateProtocol(6, {"t руд", "сек в ТУ", GetEngineNormTUParameter(32)})
        PopulateProtocol(7, {"t сброс.", "сек в ТУ", GetEngineNormTUParameter(31)})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        ' время приемистости
        parameter = conаРУД
        Dim mДлительностьФронтаСпадаРУД = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпада, parameter), ДлительностьФронтаСпада)
        With mДлительностьФронтаСпадаРУД
            .Astart = 60
            .Astop = 17
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
                Protocol(6, 2) = $"{Round(.TimeDuration, 2)} сек."
            End With

            ' вычисление время первой форсажной приемистости
            parameter = conN1
            Dim mДлительностьФронтаСпадаОтИндексаДоУровня = CType(mFiguresManager(EnumFigures.ДлительностьФронтаСпадаОтИндексаДоУровня, parameter), ДлительностьФронтаСпадаОтИндексаДоУровня)
            With mДлительностьФронтаСпадаОтИндексаДоУровня
                .IndexTstart = mДлительностьФронтаСпадаРУД.IndexTstart
                .Astop = 43
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

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class