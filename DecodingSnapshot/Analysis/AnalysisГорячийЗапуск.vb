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
        'ReDim_Protocol(7, 3)
        Re.Dim(Protocol, 7, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Запуск прошел"
        Protocol(5, 1) = "t2 МГ->откл. стартера"
        Protocol(6, 1) = "Запальное устройство"
        Protocol(7, 1) = "Время запуска"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "лев/прав/оба"
        Protocol(7, 2) = ""

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = "По графику"
        Protocol(6, 3) = ""
        Protocol(7, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String
        Dim arrТзапускаВерх(3), arrТзапускаНиз(3) As PointF
        Dim strТзапускаВерх As String = Nothing
        Dim strТзапускаНиз As String = Nothing
        Dim sngТзапуска As Double

        arrТзапускаВерх(1).X = -38
        arrТзапускаВерх(1).Y = 3.8
        arrТзапускаВерх(2).X = 26
        arrТзапускаВерх(2).Y = 6
        arrТзапускаВерх(3).X = 40
        arrТзапускаВерх(3).Y = 6

        arrТзапускаНиз(1).X = -40
        arrТзапускаНиз(1).Y = 3
        arrТзапускаНиз(2).X = 0
        arrТзапускаНиз(2).Y = 3
        arrТзапускаНиз(3).X = 30
        arrТзапускаНиз(3).Y = 4
        'по верху
        If TemperatureBoxInSnaphot < 26 Then
            strТзапускаВерх = CStr(Round(LinearInterpolation(TemperatureBoxInSnaphot, arrТзапускаВерх(1).X, arrТзапускаВерх(1).Y, arrТзапускаВерх(2).X, arrТзапускаВерх(2).Y), 1))
        ElseIf TemperatureBoxInSnaphot >= 26 Then
            strТзапускаВерх = CStr(arrТзапускаВерх(3).Y)
        End If
        'по низу
        If TemperatureBoxInSnaphot < 0 Then
            strТзапускаНиз = CStr(arrТзапускаНиз(1).Y)
        ElseIf TemperatureBoxInSnaphot >= 0 Then
            strТзапускаНиз = CStr(Round(LinearInterpolation(TemperatureBoxInSnaphot, arrТзапускаНиз(2).X, arrТзапускаНиз(2).Y, arrТзапускаНиз(3).X, arrТзапускаНиз(3).Y), 1))
        End If
        'для изделия 99 время запуска от кнопки запуск до 67 %
        sngТзапуска = Round(LinearInterpolation(TemperatureBoxInSnaphot, -40, 34, 40, 50.8), 1)
        Protocol(7, 3) = sngТзапуска.ToString & " сек."

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        parameter = conN2
        Dim clsДлительностьФронтаСпадаПрОборотов As New ДлительностьФронтаСпадаПрОборотов(parameter,
                                                                                          Parent.FrequencyBackgroundSnapshot,
                                                                                          Parent.MeasuredValues,
                                                                                          Parent.SnapshotSmallParameters,
                                                                                          Parent.XAxisTime.Range.Minimum,
                                                                                          Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпадаПрОборотов
            .Температура = TemperatureBoxInSnaphot
            .Аначальное = 53
            .Аконечное = 67
            .Расчет()
        End With

        If clsДлительностьФронтаСпадаПрОборотов.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsДлительностьФронтаСпадаПрОборотов.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьФронтаСпадаПрОборотов
                Parent.TracingDecodingArrow(
            .Тначальное,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
            .Тконечное,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
            ArrowType.Horizontal,
            parameter & ":53физ-67пр dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(5, 2) = Round(.Тдлительность, 2) & " сек."
                Protocol(5, 3) = "От " & strТзапускаНиз & " до " & strТзапускаВерх & " сек."
            End With

            parameter = conЗапуск
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
                                                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                                                  Parent.MeasuredValues,
                                                                                                                  Parent.SnapshotSmallParameters,
                                                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                                                  Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск
                .ИндексТначальное = 1 'clsДлительностьФронтаСпада.ИндексТначальное
                .Аконечное = 4
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.ErrorsMessage & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск
                    Parent.TracingDecodingArrow(
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    clsДлительностьФронтаСпадаПрОборотов.Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, clsДлительностьФронтаСпадаПрОборотов.ИндексПараметра + 1, clsДлительностьФронтаСпадаПрОборотов.Аконечное),
                    ArrowType.Horizontal,
                    parameter & ":dT=" & Round(clsДлительностьФронтаСпадаПрОборотов.Тконечное - .Тконечное, 2) & " сек.")
                    Protocol(7, 2) = Round(clsДлительностьФронтаСпадаПрОборотов.Тконечное - .Тконечное, 2) & " сек."
                End With

                parameter = conТ4
                'параметр = "N2>53%" 'проба
                Dim clsЗначениеПараметраТ4ВИндексе As New ЗначениеПараметраВИндексе(parameter,
                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                    Parent.MeasuredValues,
                                                                                    Parent.SnapshotSmallParameters,
                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                    Parent.XAxisTime.Range.Maximum)
                With clsЗначениеПараметраТ4ВИндексе
                    .ИндексТначальное = clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.ИндексТконечное
                    .Расчет()
                End With
                If clsЗначениеПараметраТ4ВИндексе.IsErrors Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += clsЗначениеПараметраТ4ВИндексе.ErrorsMessage & vbCrLf
                Else
                    ''строим стрелки
                End If

                Dim clsДлительностьФронтаСпадаОтИндексаДоУровняТ4 As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
                                                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                                                  Parent.MeasuredValues,
                                                                                                                  Parent.SnapshotSmallParameters,
                                                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                                                  Parent.XAxisTime.Range.Maximum)
                With clsДлительностьФронтаСпадаОтИндексаДоУровняТ4
                    .ИндексТначальное = clsДлительностьФронтаСпадаОтИндексаДоУровняЗапуск.ИндексТконечное
                    .Аконечное = clsЗначениеПараметраТ4ВИндексе.ЗначениеПараметра + 5 'начало стабильного роста Т4
                    .Расчет()
                End With
                If clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.IsErrors Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.ErrorsMessage & vbCrLf
                Else
                    ''строим стрелки
                End If

                'вычисляем длительность заброса первого спада после максимума
                parameter = conРтОК1К
                Dim clsДлительностьЗабросаПровалаОК1К As New ДлительностьЗабросаПровала(parameter,
                                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                                        Parent.MeasuredValues,
                                                                                        Parent.SnapshotSmallParameters,
                                                                                        Parent.XAxisTime.Range.Minimum,
                                                                                        Parent.XAxisTime.Range.Maximum)
                With clsДлительностьЗабросаПровалаОК1К
                    .Аначальное = 12
                    .Апорога = 11.9
                    .Расчет()
                End With

                If clsДлительностьЗабросаПровалаОК1К.IsErrors Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    IsTotalErrors = True
                    totalErrorsMessage += clsДлительностьЗабросаПровалаОК1К.ErrorsMessage & vbCrLf
                Else
                    ''строим стрелки
                End If

                If clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.Тконечное > clsДлительностьЗабросаПровалаОК1К.Тначальное Then
                    'плохо температура растет позже первого спада Рт ОК1К риуем стрелку
                    ''строим стрелки
                    With clsДлительностьЗабросаПровалаОК1К
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                        clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.ИндексПараметра + 1, clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.Аконечное),
                        ArrowType.Horizontal,
                        conТ4 & ":dTзадержки=" & Round(clsДлительностьФронтаСпадаОтИндексаДоУровняТ4.Тконечное - .Тначальное, 2) & " сек.")
                        'Protocol(7, 2) = Round(.Тконечное - clsДлительностьЗабросаПровалаЗапуск.Тначальное, 2) & " сек."
                    End With
                End If
            End If
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class

