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
        'ReDim_Protocol(8, 3)
        Re.Dim(Protocol, 8, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "N2физ."
        Protocol(6, 1) = "Ртбр"
        Protocol(7, 1) = "РтОК"
        Protocol(8, 1) = "t бр"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "%"
        Protocol(6, 2) = "кг/см2"
        Protocol(7, 2) = "кг/см2"
        Protocol(8, 2) = "сек в ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(50)
        Protocol(6, 3) = GetEngineNormTUParameter(39)
        Protocol(7, 3) = GetEngineNormTUParameter(20)
        Protocol(8, 3) = GetEngineNormTUParameter(40)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'вычисляем длительность заброса
        parameter = conРтОК1К
        Dim clsДлительностьЗабросаПровала As New ДлительностьЗабросаПровала(parameter,
                                                                            Parent.FrequencyBackgroundSnapshot,
                                                                            Parent.MeasuredValues,
                                                                            Parent.SnapshotSmallParameters,
                                                                            Parent.XAxisTime.Range.Minimum,
                                                                            Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровала
            .Аначальное = 5
            .Апорога = 7
            .Расчет()
        End With

        If clsДлительностьЗабросаПровала.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsДлительностьЗабросаПровала.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровала
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(8, 2) = Round(.Тдлительность, 2) & " сек."
            End With
            'рисуем риску максимального значения
            With clsДлительностьЗабросаПровала
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение - 2),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение + 2),
                ArrowType.Inclined,
                parameter & ":максимум=" & Round(.МаксимальноеЗначение, 2) & " кг")
                Protocol(6, 2) = "максимум=" & Round(.МаксимальноеЗначение, 2) & " кг"
            End With

            '************************************************
            'вычисление время первого спада ниже уровня 1
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровня As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
                                                                                                            Parent.FrequencyBackgroundSnapshot,
                                                                                                            Parent.MeasuredValues,
                                                                                                            Parent.SnapshotSmallParameters,
                                                                                                            Parent.XAxisTime.Range.Minimum,
                                                                                                            Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровня
                .ИндексТначальное = clsДлительностьЗабросаПровала.ИндексТконечное
                .Аконечное = 1
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровня.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровня.ErrorsMessage & vbCrLf
            Else
                Dim clsЗначениеПараметраВИндексе As New ЗначениеПараметраВИндексе(parameter,
                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                  Parent.MeasuredValues,
                                                                                  Parent.SnapshotSmallParameters,
                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                  Parent.XAxisTime.Range.Maximum)
                With clsЗначениеПараметраВИндексе
                    .ИндексТначальное = clsДлительностьФронтаСпадаОтИндексаДоУровня.ИндексТконечное - 5 * Parent.FrequencyBackgroundSnapshot
                    .Расчет()
                End With
                If clsЗначениеПараметраВИндексе.IsErrors Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += clsЗначениеПараметраВИндексе.ErrorsMessage & vbCrLf
                Else
                    'строим стрелки
                    With clsЗначениеПараметраВИндексе
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра - 2),
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра + 2),
                        ArrowType.Inclined,
                        parameter & "=" & Round(.ЗначениеПараметра, 2) & " кг")
                        Protocol(7, 2) = Round(.ЗначениеПараметра, 2) & " кг"
                    End With
                End If
            End If
        End If
        '************************************************
        'нахождение минимального и максимального значения параметра N2
        parameter = conN2
        Dim clsМинимальноеМаксимальноеЗначениеПараметра As New МинимальноеМаксимальноеЗначениеПараметра(parameter,
                                                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                                                        Parent.MeasuredValues,
                                                                                                        Parent.SnapshotSmallParameters,
                                                                                                        Parent.XAxisTime.Range.Minimum,
                                                                                                        Parent.XAxisTime.Range.Maximum)
        With clsМинимальноеМаксимальноеЗначениеПараметра
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
            .Расчет()
        End With
        If clsМинимальноеМаксимальноеЗначениеПараметра.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsМинимальноеМаксимальноеЗначениеПараметра.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsМинимальноеМаксимальноеЗначениеПараметра
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение - 2),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение + 2),
                ArrowType.Inclined,
                parameter & ":максимум=" & Round(.МаксимальноеЗначение, 2) & " %")
                Protocol(5, 2) = "максимум=" & Round(.МаксимальноеЗначение, 2) & " %"
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class

