Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 10 '"Быстродействие РС при включении СУНА по кнопке БК"
''' </summary>
Friend Class AnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК
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
        Protocol(5, 1) = "N2привед."
        Protocol(6, 1) = "t РС"
        Protocol(7, 1) = "tБК"
        Protocol(8, 1) = "tСУНА"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "%"
        Protocol(6, 2) = "сек"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "сек в ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = GetEngineNormTUParameter(46)
        Protocol(7, 3) = GetEngineNormTUParameter(52)
        Protocol(8, 3) = GetEngineNormTUParameter(41)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
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
                Protocol(7, 2) = Round(.Тдлительность, 2) & " сек."
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
                    Protocol(8, 2) = Round(.Тдлительность, 2) & " сек."
                End With
            End If

            параметр = conДиаметрРС
            Dim clsПостроениеНаклонной As New ПостроениеНаклонной(параметр,
                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                  Parent.MeasuredValues,
                                                                  Parent.SnapshotSmallParameters,
                                                                  Parent.XAxisTime.Range.Minimum,
                                                                  Parent.XAxisTime.Range.Maximum)
            With clsПостроениеНаклонной
                .ПревышениеНадСредним = 0.5
                .Уровень2Линии = 5 '20 '
                .Расчет()
            End With
            If clsПостроениеНаклонной.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsПостроениеНаклонной.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsПостроениеНаклонной
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                    Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                End With
                'наклонная 1
                With clsПостроениеНаклонной
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .ТBx,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    ArrowType.Inclined,
                    "")
                End With
                'наклонная 2 параллельно
                With clsПостроениеНаклонной
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .АначальноеПлюс5),
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .АначальноеПлюс5),
                    ArrowType.Inclined,
                    "")
                End With
            End If
        End If
        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

