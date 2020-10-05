﻿Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 23 '"Включение Форсажа РУД67 -> РУД115 (106) изделия 39"
''' </summary>
Friend Class AnalysisВключениеФорсажаРУД67РУД115изд39
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(15, 3)
        Re.Dim(Protocol, 15, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "t вкл Форсажа"
        Protocol(7, 1) = "t выхода на max Форсажа"
        Protocol(8, 1) = "J1 ионизации"
        Protocol(9, 1) = "J2 ионизации"
        Protocol(10, 1) = "Заброс N1 от устан. значения"
        Protocol(11, 1) = "Заброс N1 от КРД"
        Protocol(12, 1) = "Заброс N2 от КРД"
        Protocol(13, 1) = "Провал N2 от устан. значения"
        Protocol(14, 1) = "dN=dN1прев.+dN2пров."
        Protocol(15, 1) = "Заброс Ттвг"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "сек в ТУ"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "мкА"
        Protocol(9, 2) = "мкА"
        Protocol(10, 2) = "%"
        Protocol(11, 2) = "%"
        Protocol(12, 2) = "Нет"
        Protocol(13, 2) = "%"
        Protocol(14, 2) = "% в ТУ"
        Protocol(15, 2) = "Нет"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(32)
        Protocol(6, 3) = GetEngineNormTUParameter(26)
        Protocol(7, 3) = GetEngineNormTUParameter(27)
        Protocol(8, 3) = GetEngineNormTUParameter(38)
        Protocol(9, 3) = GetEngineNormTUParameter(38)
        Protocol(10, 3) = ""
        Protocol(11, 3) = GetEngineNormTUParameter(12)
        Protocol(12, 3) = GetEngineNormTUParameter(13)
        Protocol(13, 3) = ""
        Protocol(14, 3) = GetEngineNormTUParameter(16)
        Protocol(15, 3) = GetEngineNormTUParameter(36)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Dim mfrmРегулировка As New FormAdjustment(Parent.TypeKRDinSnapshot)
        mfrmРегулировка.ShowDialog()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

        'риски настройки КРД параметра N1
        параметр = conN1
        Dim clsРискиНастроекПараметровN1 As New РискиНастроекПараметров(параметр,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровN1.Расчет()
        If clsРискиНастроекПараметровN1.Ошибка = False Then
            'строим стрелки
            With clsРискиНастроекПараметровN1
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.N1НастройкаКРД),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.N1НастройкаКРД),
                ArrowType.Inclined,
                "настройка N1 КРД=" & CStr(mfrmРегулировка.N1НастройкаКРД) & " %")
            End With
        End If

        'риски настройки КРД параметра N2
        параметр = conN2
        Dim clsРискиНастроекПараметровN2 As New РискиНастроекПараметров(параметр,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровN2.Расчет()
        If clsРискиНастроекПараметровN2.Ошибка = False Then
            'строим стрелки
            With clsРискиНастроекПараметровN2
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.N2НастройкаКРД),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.N2НастройкаКРД),
                ArrowType.Inclined,
                "настройка N2 КРД=" & CStr(mfrmРегулировка.N2НастройкаКРД) & " %")
            End With
        End If

        'риски настройки КРД параметра Т4
        параметр = conТ4
        Dim clsРискиНастроекПараметровТ4 As New РискиНастроекПараметров(параметр,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровТ4.Расчет()
        If clsРискиНастроекПараметровТ4.Ошибка = False Then
            'строим стрелки
            With clsРискиНастроекПараметровТ4
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.Т4КРД),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, mfrmРегулировка.Т4КРД),
                ArrowType.Inclined,
                "настройка Т4 КРД=" & CStr(mfrmРегулировка.Т4КРД) & " гр.")
            End With
        End If

        'находим время приемистости
        параметр = conаРУД
        Dim clsДлительностьФронтаСпада As New ДлительностьФронтаСпада(параметр,
                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                      Parent.MeasuredValues,
                                                                      Parent.SnapshotSmallParameters,
                                                                      Parent.XAxisTime.Range.Minimum,
                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпада
            .Аначальное = 73
            .Аконечное = ВводЗначенияРуда() '111 '114
            .Расчет()
        End With

        If clsДлительностьФронтаСпада.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьФронтаСпада.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьФронтаСпада
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
            'вычисление время первой форсажной приемистости
            параметр = conТокJправый
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
                                                                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                                                                      Parent.MeasuredValues,
                                                                                                                      Parent.SnapshotSmallParameters,
                                                                                                                      Parent.XAxisTime.Range.Minimum,
                                                                                                                      Parent.XAxisTime.Range.Maximum)
            Dim clsМинимальноеМаксимальноеЗначениеПараметраТокJправый As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                                                                      Parent.MeasuredValues,
                                                                                                                      Parent.SnapshotSmallParameters,
                                                                                                                      Parent.XAxisTime.Range.Minimum,
                                                                                                                      Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый
                .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Аконечное = 60
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.ТекстОшибки & vbCrLf
            Else
                '************************************************
                'нахождение минимального и максимального значения параметра ТокJправый
                With clsМинимальноеМаксимальноеЗначениеПараметраТокJправый
                    .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                    .Расчет()
                End With
            End If
            '************************************************
            'находим наибольшее
            параметр = conТокJлевый
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
                                                                                                                     Parent.FrequencyBackgroundSnapshot,
                                                                                                                     Parent.MeasuredValues,
                                                                                                                     Parent.SnapshotSmallParameters,
                                                                                                                     Parent.XAxisTime.Range.Minimum,
                                                                                                                     Parent.XAxisTime.Range.Maximum)
            Dim clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый As New МинимальноеМаксимальноеЗначениеПараметра(параметр,
                                                                                                                     Parent.FrequencyBackgroundSnapshot,
                                                                                                                     Parent.MeasuredValues,
                                                                                                                     Parent.SnapshotSmallParameters,
                                                                                                                     Parent.XAxisTime.Range.Minimum,
                                                                                                                     Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый
                .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Аконечное = 60
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.ТекстОшибки & vbCrLf
            Else
                '************************************************
                'нахождение минимального и максимального значения параметра ТокJправый
                With clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый
                    .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                    .Расчет()
                End With
            End If
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.Ошибка = False And clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.Ошибка = False Then
                If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.Тдлительность <= clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.Тдлительность Then
                    'строим(стрелки)
                    With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                        ArrowType.Horizontal,
                        параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                        Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                    End With
                Else
                    'строим стрелки
                    With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                        ArrowType.Horizontal,
                        параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                        Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                    End With
                End If
                Protocol(8, 2) = Round(clsМинимальноеМаксимальноеЗначениеПараметраТокJправый.МаксимальноеЗначение, 2) & " мка."
                Protocol(9, 2) = Round(clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый.МаксимальноеЗначение, 2) & " мка."
            End If

            'вычисление время второй форсажной приемистости
            параметр = conПолныйФорсаж
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
                                                                                                               Parent.FrequencyBackgroundSnapshot,
                                                                                                               Parent.MeasuredValues,
                                                                                                               Parent.SnapshotSmallParameters,
                                                                                                               Parent.XAxisTime.Range.Minimum,
                                                                                                               Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ
                .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Аконечное = 4
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ
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

        'вычисляем заброс N1
        параметр = conN1
        Dim clsДлительностьЗабросаПровалаN1 As New ДлительностьЗабросаПровала(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаN1
            '.Аначальное = 50
            .СтартовыйИндекс = clsДлительностьФронтаСпада.ИндексТконечное
            .Апорога = mfrmРегулировка.N1НастройкаКРД
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаN1.Ошибка = True Then
            'анализируем для последующих построений
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаN1
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                ArrowType.Vertical,
                параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %")
                Protocol(11, 2) = параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
            End With
        End If

        'вычисляем заброс N2
        параметр = conN2
        Dim clsДлительностьЗабросаПровалаN2 As New ДлительностьЗабросаПровала(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаN2
            .Аначальное = 50
            .Апорога = mfrmРегулировка.N2НастройкаКРД
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаN2.Ошибка = True Then
            'анализируем для последующих построений
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаN2
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                ArrowType.Vertical,
                параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %")
                Protocol(12, 2) = параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
            End With
        End If

        'вычисляем заброс Т4
        параметр = conТ4
        Dim clsДлительностьЗабросаПровалаТ4 As New ДлительностьЗабросаПровала(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаТ4
            .Аначальное = 700
            .Апорога = mfrmРегулировка.Т4КРД
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаТ4.Ошибка = True Then
            'анализируем для последующих построений
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаТ4
                Dim dblЗабросT4 As Double = Round(.МаксимальноеЗначение - .Апорога, 2)
                If dblЗабросT4 > 0 AndAlso dblЗабросT4 < 3 Then
                    dblЗабросT4 = 0
                Else
                    Parent.TracingDecodingArrow(
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                    .ТМаксимальногоЗначения,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                    ArrowType.Vertical,
                    параметр & ":заброс=" & dblЗабросT4 & " гр.")
                End If
                Protocol(15, 2) = параметр & ":заброс=" & dblЗабросT4 & " гр."
            End With
        End If

        'находим провал N1относительно установившегося
        параметр = conN1
        Dim clsПровалN1ОтносительноУстановившегося As New ПровалN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsПровалN1ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 'отсчет от этой точки минус 2 секунды
            .Расчет()
        End With

        If clsПровалN1ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsПровалN1ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsПровалN1ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Vertical,
                параметр & ":провал=" & Round(.DeltaA, 2) & " %")
                'Protocol(10, 2) = параметр & ":провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If

        'находим провал N2относительно установившегося
        параметр = conN2
        Dim clsПровалN2ОтносительноУстановившегося As New ПровалN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsПровалN2ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 'отсчет от этой точки минус 2 секунды
            .Расчет()
        End With

        If clsПровалN2ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsПровалN2ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsПровалN2ОтносительноУстановившегося
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Vertical,
                параметр & ":уст. провал=" & Round(.DeltaA, 2) & " %")
                Protocol(13, 2) = параметр & ":уст. провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If

        'находим заброс N1 относительно установившегося
        параметр = conN1
        Dim clsЗабросN1ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                                              Parent.MeasuredValues,
                                                                                              Parent.SnapshotSmallParameters,
                                                                                              Parent.XAxisTime.Range.Minimum,
                                                                                              Parent.XAxisTime.Range.Maximum)
        With clsЗабросN1ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТконечное 'отсчет от этой точки минус 2 секунды
            .Расчет()
        End With

        If clsЗабросN1ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросN1ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросN1ОтносительноУстановившегося
                If .DeltaA > 0 Then
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Vertical,
                    параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %")
                    Protocol(10, 2) = параметр & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                End If
            End With
        End If

        Dim dN As Double
        If clsЗабросN1ОтносительноУстановившегося.DeltaA > 0 And clsПровалN2ОтносительноУстановившегося.DeltaA > 0 Then
            dN = clsЗабросN1ОтносительноУстановившегося.DeltaA + clsПровалN2ОтносительноУстановившегося.DeltaA
        ElseIf clsЗабросN1ОтносительноУстановившегося.DeltaA > 0 And clsПровалN2ОтносительноУстановившегося.DeltaA <= 0 Then
            dN = clsЗабросN1ОтносительноУстановившегося.DeltaA
        ElseIf clsЗабросN1ОтносительноУстановившегося.DeltaA <= 0 And clsПровалN2ОтносительноУстановившегося.DeltaA > 0 Then
            dN = clsПровалN2ОтносительноУстановившегося.DeltaA
        End If
        Protocol(14, 2) = "dN=" & Round(dN, 2) & " %"

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

