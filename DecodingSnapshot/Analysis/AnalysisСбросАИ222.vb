﻿Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 29 '"Сброс АИ-222"
''' </summary>
Friend Class AnalysisСбросАИ222
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
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "Провал nвдпр ЗМГ или ПНГ" ' ЗМГ - ниже 57.5 или ПНГ - ниже 67.5
        Protocol(7, 1) = "t сброса" 'от начала Руд до Gt=360
        Protocol(8, 1) = "Автоматика изделия и НА сработали"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "Нет"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "По ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = "не более 1 сек"
        Protocol(6, 3) = ">= 57.5"
        Protocol(7, 3) = "не более 2 сек"
        Protocol(8, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Dim strДобавка As String = "ПМГ"
        Dim ЗначениеУровняNвдпр As Double = 67.5
        Dim clsЗначениеПараметраЗемляАИ222ВИндексе As ЗначениеПараметраВИндексе

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'находим время приемистости
        параметр = conаРУДАИ222
        Dim clsДлительностьФронтаСпадаРУДАИ222 As New ДлительностьФронтаСпада(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпадаРУДАИ222
            .Аначальное = 65 '65
            .Аконечное = 20
            .Расчет()
        End With

        If clsДлительностьФронтаСпадаРУДАИ222.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьФронтаСпадаРУДАИ222.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьФронтаСпадаРУДАИ222
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Horizontal,
                параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(5, 2) = Round(.Тдлительность, 2) & " сек."
            End With
            '************************************************
            'находим значение conЗемляАИ222 в начале движения Руд
            параметр = conЗемляАИ222
            clsЗначениеПараметраЗемляАИ222ВИндексе = New ЗначениеПараметраВИндексе(параметр,
                                                                                   Parent.FrequencyBackgroundSnapshot,
                                                                                   Parent.MeasuredValues,
                                                                                   Parent.SnapshotSmallParameters,
                                                                                   Parent.XAxisTime.Range.Minimum,
                                                                                   Parent.XAxisTime.Range.Maximum)
            With clsЗначениеПараметраЗемляАИ222ВИндексе
                .ИндексТначальное = clsДлительностьФронтаСпадаРУДАИ222.ИндексТначальное
                .Расчет()
            End With
            If clsЗначениеПараметраЗемляАИ222ВИндексе.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsЗначениеПараметраЗемляАИ222ВИндексе.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsЗначениеПараметраЗемляАИ222ВИндексе
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра - 0.5),
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра + 0.5),
                    ArrowType.Inclined,
                    параметр & "=" & Round(.ЗначениеПараметра, 2))
                    'параметр & "=" & Round(.ЗначениеПараметра, 2) & " дел.")
                    'Protocol(7, 2) = Round(.ЗначениеПараметра, 2) & " дел."
                    If .ЗначениеПараметра >= 1 Then
                        strДобавка = "ЗМГ"
                        ЗначениеУровняNвдпр = 57.5
                        Protocol(6, 3) = ">= 57.5 %"
                    Else
                        strДобавка = "ПМГ"
                        ЗначениеУровняNвдпр = 67.5
                        Protocol(6, 3) = ">= 67.5 %"
                    End If
                    'If clsЗначениеПараметраЗемляАИ222ВИндексе IsNot Nothing Then
                    '    If clsЗначениеПараметраЗемляАИ222ВИндексе.ЗначениеПараметра >= 1 Then
                    '        'strДобавка = "ЗМГ"
                    '        Protocol(7, 3) = "ниже 57.5 %"
                    '    Else
                    '        'strДобавка = "ПМГ"
                    '        Protocol(7, 3) = "ниже 67.5 %"
                    '    End If
                    'End If

                End With
            End If

            'вычисление время сброса от начала спада Руд до conGтопливаАИ222 = 360
            параметр = conGтопливаАИ222
            Dim clsДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
                                                                                                                          Parent.FrequencyBackgroundSnapshot,
                                                                                                                          Parent.MeasuredValues,
                                                                                                                          Parent.SnapshotSmallParameters,
                                                                                                                          Parent.XAxisTime.Range.Minimum,
                                                                                                                          Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня
                .ИндексТначальное = clsДлительностьФронтаСпадаРУДАИ222.ИндексТначальное
                .Аконечное = 360
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаСпадаGтопливаАИ2222ОтИндексаДоУровня
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                    Protocol(7, 2) = Round(.Тдлительность, 2) & " сек."
                End With
            End If
        End If

        'находим время провала conNвдпрАИ222 от уровня ЗначениеУровняNвдпр
        параметр = conNвдпрАИ222
        Dim clsДлительностьЗабросаПровалаNвдпрАИ222 As New ДлительностьЗабросаПровала(параметр,
                                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                                      Parent.MeasuredValues,
                                                                                      Parent.SnapshotSmallParameters,
                                                                                      Parent.XAxisTime.Range.Minimum,
                                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаNвдпрАИ222
            .Аначальное = 80
            .Апорога = ЗначениеУровняNвдпр
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаNвдпрАИ222.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьЗабросаПровалаNвдпрАИ222.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаNвдпрАИ222
                Parent.TracingDecodingArrow(
                .ТМинимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .ТМинимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение),
                ArrowType.Vertical,
                параметр & strДобавка & ":провал=" & Round(.Апорога - .МинимальноеЗначение, 2) & " %")
                Protocol(6, 2) = параметр & strДобавка & ":провал=" & Round(.Апорога - .МинимальноеЗначение, 2) & "% от уровня " & ЗначениеУровняNвдпр

                If .МинимальноеЗначение < ЗначениеУровняNвдпр Then
                    'строим стрелки
                    Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                        ArrowType.Horizontal,
                        параметр & strДобавка & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                    'Protocol(9, 2) = Round(.Тдлительность, 2) & " сек."
                End If

            End With
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

