Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 8 '"Включение СУНА на N2=90 по кнопке БК"
''' </summary>
Friend Class AnalysisВключениеСУНАНаN290ПоКнопкеБК
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(10, 3)
        Re.Dim(Protocol, 10, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "N2привед."
        Protocol(6, 1) = "t БК"
        Protocol(7, 1) = "t СУНА"
        Protocol(8, 1) = "dРС"
        Protocol(9, 1) = "a2"
        Protocol(10, 1) = "Автоматика а1=f(N2прив)"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "%"
        Protocol(6, 2) = "сек"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "дел. В ТУ"
        Protocol(9, 2) = "дел. В ТУ"
        Protocol(10, 2) = "По ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = GetEngineNormTUParameter(52)
        Protocol(7, 3) = GetEngineNormTUParameter(41)
        Protocol(8, 3) = GetEngineNormTUParameter(48)
        Protocol(9, 3) = GetEngineNormTUParameter(49)
        Protocol(10, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
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
                Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
            End With
        End If

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
                Protocol(7, 2) = Round(.Тдлительность, 2) & " сек."
            End With
            'находим провал ДиаметрРС относительно установившегося здесь нужно значение за последние 2 сек
            параметр = conДиаметрРС
            Dim clsПровалДиаметрРСОтносительноУстановившегося As New ПровалN1ОтносительноУстановившегося(параметр,
                                                                                                         Parent.FrequencyBackgroundSnapshot,
                                                                                                         Parent.MeasuredValues,
                                                                                                         Parent.SnapshotSmallParameters,
                                                                                                         Parent.XAxisTime.Range.Minimum,
                                                                                                         Parent.XAxisTime.Range.Maximum)
            With clsПровалДиаметрРСОтносительноУстановившегося
                .ИндексТначальное = clsДлительностьЗабросаПровала.ИндексТначальное
                .Расчет()
            End With

            If clsПровалДиаметрРСОтносительноУстановившегося.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsПровалДиаметрРСОтносительноУстановившегося.ТекстОшибки & vbCrLf
            Else
                'находим значение ДиаметрРС в точке окончания СУНА
                Dim clsЗначениеПараметраВИндексе As New ЗначениеПараметраВИндексе(параметр,
                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                  Parent.MeasuredValues,
                                                                                  Parent.SnapshotSmallParameters,
                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                  Parent.XAxisTime.Range.Maximum)
                With clsЗначениеПараметраВИндексе
                    .ИндексТначальное = clsДлительностьЗабросаПровала.ИндексТконечное - 1S
                    .Расчет()
                End With

                'строим стрелки
                With clsПровалДиаметрРСОтносительноУстановившегося
                    Parent.TracingDecodingArrow(
                    clsДлительностьЗабросаПровала.Тконечное - 1 / Parent.FrequencyBackgroundSnapshot,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраВИндексе.ЗначениеПараметра),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Vertical,
                    параметр & ":dA=" & Round(clsЗначениеПараметраВИндексе.ЗначениеПараметра - .Аконечное, 2) & " дел.")
                    Protocol(8, 2) = ":dA=" & Round(clsЗначениеПараметраВИндексе.ЗначениеПараметра - .Аконечное, 2) & " дел."
                End With
            End If

            'находим значение a2 в точке окончания СУНА
            параметр = cona2
            Dim clsЗначениеПараметраА2ВИндексе As New ЗначениеПараметраВИндексе(параметр,
                                                                                Parent.FrequencyBackgroundSnapshot,
                                                                                Parent.MeasuredValues,
                                                                                Parent.SnapshotSmallParameters,
                                                                                Parent.XAxisTime.Range.Minimum,
                                                                                Parent.XAxisTime.Range.Maximum)
            With clsЗначениеПараметраА2ВИндексе
                .ИндексТначальное = CShort(clsДлительностьЗабросаПровала.ИндексТконечное - 1)
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
                    clsДлительностьЗабросаПровала.Тконечное - 1 / Parent.FrequencyBackgroundSnapshot,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра - 2),
                    clsДлительностьЗабросаПровала.Тконечное - 1 / Parent.FrequencyBackgroundSnapshot,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра + 2),
                    ArrowType.Inclined,
                    параметр & ":=" & Round(clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра, 2) & " дел.")
                    Protocol(9, 2) = Round(clsЗначениеПараметраА2ВИндексе.ЗначениеПараметра, 2) & " дел."
                End With
            End If

            'находим значение N2приведенное в точке перед СУНА
            параметр = conN2
            Dim clsЗначениеПараметраN2ВИндексе As New ЗначениеПараметраВИндексе(параметр,
                                                                                Parent.FrequencyBackgroundSnapshot,
                                                                                Parent.MeasuredValues,
                                                                                Parent.SnapshotSmallParameters,
                                                                                Parent.XAxisTime.Range.Minimum,
                                                                                Parent.XAxisTime.Range.Maximum) '
            With clsЗначениеПараметраN2ВИндексе
                .ИндексТначальное = clsДлительностьЗабросаПровала.ИндексТначальное - 5S
                .Расчет()
            End With
            If clsЗначениеПараметраN2ВИндексе.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsЗначениеПараметраN2ВИндексе.ТекстОшибки & vbCrLf
            Else
                Dim КоэПриведения As Double = System.Math.Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin))
                'строим стрелки
                With clsЗначениеПараметраN2ВИндексе
                    Parent.TracingDecodingArrow(
                    clsДлительностьЗабросаПровала.Тначальное - 5 / Parent.FrequencyBackgroundSnapshot,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраN2ВИндексе.ЗначениеПараметра - 2),
                    clsДлительностьЗабросаПровала.Тначальное - 5 / Parent.FrequencyBackgroundSnapshot,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, clsЗначениеПараметраN2ВИндексе.ЗначениеПараметра + 2),
                    ArrowType.Inclined,
                    параметр & "привед:=" & Round(clsЗначениеПараметраN2ВИндексе.ЗначениеПараметра * КоэПриведения, 2) & " дел.")
                    Protocol(5, 2) = Round(clsЗначениеПараметраN2ВИндексе.ЗначениеПараметра * КоэПриведения, 2) & " дел."
                End With
            End If
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

