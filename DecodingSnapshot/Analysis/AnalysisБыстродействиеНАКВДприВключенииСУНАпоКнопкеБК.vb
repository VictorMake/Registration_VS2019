Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 9 '"Быстродействие НА КВД при включении СУНА по кнопке БК"
''' </summary>
Friend Class AnalysisБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(5, 3)
        Re.Dim(Protocol, 5, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Сигналы прошли"
        Protocol(4, 1) = "tБК"
        Protocol(5, 1) = "tСУНА"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = "По ТУ"
        Protocol(4, 2) = "сек в ТУ"
        Protocol(5, 2) = "сек в ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = GetEngineNormTUParameter(52)
        Protocol(5, 3) = GetEngineNormTUParameter(41)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        'Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'вычисляем длительность КнопкаБК
        параметр = conКнопкаБК
        Dim clsДлительностьЗабросаПровалаБК As New ДлительностьЗабросаПровала(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаБК
            .Аначальное = 1
            .Апорога = 4.99
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаБК.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьЗабросаПровалаБК.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаБК
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(4, 2) = Round(.Тдлительность, 2) & " сек."
            End With

            'вычисляем длительность КлапанСУНА
            параметр = conКлапанСУНА
            Dim clsДлительностьЗабросаПровала As New ДлительностьЗабросаПровала(параметр,
                                                                                Parent.FrequencyBackgroundSnapshot,
                                                                                Parent.MeasuredValues,
                                                                                Parent.SnapshotSmallParameters,
                                                                                Parent.XAxisTime.Range.Minimum,
                                                                                Parent.XAxisTime.Range.Maximum)
            With clsДлительностьЗабросаПровала
                .Аначальное = 1
                .Апорога = 4.99
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
                    Protocol(5, 2) = Round(.Тдлительность, 2) & " сек."
                End With

                'находим значение a2 в точке начала БК
                параметр = cona2
                Dim clsЗначениеПараметраА2ВИндексе As New ЗначениеПараметраВИндексе(параметр,
                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                    Parent.MeasuredValues,
                                                                                    Parent.SnapshotSmallParameters,
                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                    Parent.XAxisTime.Range.Maximum)
                With clsЗначениеПараметраА2ВИндексе
                    .ИндексТначальное = CShort(clsДлительностьЗабросаПровалаБК.ИндексТначальное - 1)
                    .Расчет()
                End With
                If clsЗначениеПараметраА2ВИндексе.Ошибка = True Then
                    'анализируем для последующих построений
                    'накапливаем ошибку
                    общаяОшибка = True
                    общийТекстОшибок += clsЗначениеПараметраА2ВИндексе.ТекстОшибки & vbCrLf
                Else
                    'строим стрелки
                    With clsЗначениеПараметраА2ВИндексе
                        Parent.TracingDecodingArrow(
                        clsДлительностьЗабросаПровалаБК.Тначальное - 1 / Parent.FrequencyBackgroundSnapshot,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра - 2),
                        clsДлительностьЗабросаПровалаБК.Тначальное - 1 / Parent.FrequencyBackgroundSnapshot,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра + 2),
                        ArrowType.Inclined,
                        параметр & ":=" & Round(clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра, 2) & " дел.")
                        'Protocol(9, 2) = Round(clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра, 2) & " дел."
                    End With
                End If
                'нахождение максимального значения параметра a2 за время СУНА 
                Dim clsМинимальноеМаксимальноеЗначениеПараметра As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                                Parent.FrequencyBackgroundSnapshot,
                                                                                                                Parent.MeasuredValues,
                                                                                                                Parent.SnapshotSmallParameters,
                                                                                                                Parent.XAxisTime.Range.Minimum,
                                                                                                                Parent.XAxisTime.Range.Maximum)
                With clsМинимальноеМаксимальноеЗначениеПараметра
                    .ИндексТначальное = clsДлительностьЗабросаПровала.ИндексТначальное
                    .ИндексТконечное = clsДлительностьЗабросаПровала.ИндексТконечное
                    .Расчет()
                    'Protocol(8, 2) = Round(.МаксимальноеЗначение, 2) & " дел."
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
                        clsДлительностьЗабросаПровалаБК.Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра),
                        .ТМинимальногоЗначения,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                        ArrowType.Horizontal,
                        параметр & ":мин. dТ=" & Round(.ТМинимальногоЗначения - clsЗначениеПараметраА2ВИндексе.Тначальное, 2) & " сек.")
                    End With
                End If
            End If
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

