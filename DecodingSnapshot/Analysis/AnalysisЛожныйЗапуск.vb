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
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'вычисляем длительность заброса
        параметр = conРтОК1К
        Dim clsДлительностьЗабросаПровала As New ДлительностьЗабросаПровала(параметр,
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

        If clsДлительностьЗабросаПровала.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьЗабросаПровала.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровала
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
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
                параметр & ":максимум=" & Round(.МаксимальноеЗначение, 2) & " кг")
                Protocol(6, 2) = "максимум=" & Round(.МаксимальноеЗначение, 2) & " кг"
            End With

            '************************************************
            'вычисление время первого спада ниже уровня 1
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровня As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
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
            If clsДлительностьФронтаСпадаОтИндексаДоУровня.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаОтИндексаДоУровня.ТекстОшибки & vbCrLf
            Else
                Dim clsЗначениеПараметраВИндексе As New ЗначениеПараметраВИндексе(параметр,
                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                  Parent.MeasuredValues,
                                                                                  Parent.SnapshotSmallParameters,
                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                  Parent.XAxisTime.Range.Maximum)
                With clsЗначениеПараметраВИндексе
                    .ИндексТначальное = clsДлительностьФронтаСпадаОтИндексаДоУровня.ИндексТконечное - 5 * Parent.FrequencyBackgroundSnapshot
                    .Расчет()
                End With
                If clsЗначениеПараметраВИндексе.Ошибка = True Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    общаяОшибка = True
                    общийТекстОшибок += clsЗначениеПараметраВИндексе.ТекстОшибки & vbCrLf
                Else
                    'строим стрелки
                    With clsЗначениеПараметраВИндексе
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра - 2),
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра + 2),
                        ArrowType.Inclined,
                        параметр & "=" & Round(.ЗначениеПараметра, 2) & " кг")
                        Protocol(7, 2) = Round(.ЗначениеПараметра, 2) & " кг"
                    End With
                End If
            End If
        End If
        '************************************************
        'нахождение минимального и максимального значения параметра N2
        параметр = conN2
        Dim clsМинимальноеМаксимальноеЗначениеПараметра As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                                                        Parent.MeasuredValues,
                                                                                                        Parent.SnapshotSmallParameters,
                                                                                                        Parent.XAxisTime.Range.Minimum,
                                                                                                        Parent.XAxisTime.Range.Maximum)
        With clsМинимальноеМаксимальноеЗначениеПараметра
            '.ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
            .Расчет()
        End With
        If clsМинимальноеМаксимальноеЗначениеПараметра.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsМинимальноеМаксимальноеЗначениеПараметра.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsМинимальноеМаксимальноеЗначениеПараметра
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение - 2),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение + 2),
                ArrowType.Inclined,
                параметр & ":максимум=" & Round(.МаксимальноеЗначение, 2) & " %")
                Protocol(5, 2) = "максимум=" & Round(.МаксимальноеЗначение, 2) & " %"
            End With
        End If
        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

