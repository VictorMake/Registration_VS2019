Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 24 '"Выключение Форсажа РУД115 (106) -> РУД67 изделия 39"
''' </summary>
Friend Class AnalysisВыключениеФорсажаРУД115РУД67изд39
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(7, 3)
        Re.Dim(Protocol, 7, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "Провал N1 от устан. значения"
        Protocol(7, 1) = "Провал N2 от устан. значения"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "%"
        Protocol(7, 2) = "%"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(32)
        Protocol(6, 3) = ""
        Protocol(7, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

        'находим время спада
        parameter = conаРУД
        Dim clsДлительностьФронтаСпада As New ДлительностьФронтаСпада(parameter,
                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                      Parent.MeasuredValues,
                                                                      Parent.SnapshotSmallParameters,
                                                                      Parent.XAxisTime.Range.Minimum,
                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпада
            .Аначальное = ВводЗначенияРуда() '110 '114
            .Аконечное = 73
            .Расчет()
        End With

        If clsДлительностьФронтаСпада.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsДлительностьФронтаСпада.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьФронтаСпада
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Horizontal,
                parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(5, 2) = Round(.Тдлительность, 2) & " сек."
            End With
        End If

        'находим провал N1относительно установившегося
        parameter = conN1
        Dim clsПровалN1ОтносительноУстановившегося As New ПровалN1ОтносительноУстановившегося(parameter,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsПровалN1ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 'отсчет от этой точки минус 2 секунды
            .Расчет()
        End With

        If clsПровалN1ОтносительноУстановившегося.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsПровалN1ОтносительноУстановившегося.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsПровалN1ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Vertical,
                parameter & ":провал=" & Round(.DeltaA, 2) & " %")
                Protocol(6, 2) = parameter & ":уст. провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If

        'находим провал N2относительно установившегося
        parameter = conN2
        Dim clsПровалN2ОтносительноУстановившегося As New ПровалN1ОтносительноУстановившегося(parameter,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsПровалN2ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 'отсчет от этой точки минус 2 секунды
            .Расчет()
        End With

        If clsПровалN2ОтносительноУстановившегося.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsПровалN2ОтносительноУстановившегося.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsПровалN2ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Vertical,
                parameter & ":уст. провал=" & Round(.DeltaA, 2) & " %")
                Protocol(7, 2) = parameter & ":уст. провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class

