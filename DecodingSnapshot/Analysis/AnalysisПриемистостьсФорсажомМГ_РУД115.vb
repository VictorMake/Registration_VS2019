Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 3 "Приемистость с Форсажом МГ -> РУД115 (106)"
''' </summary>
Friend Class AnalysisПриемистостьсФорсажомМГ_РУД115
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(12, 3)
        Re.Dim(Protocol, 12, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли "
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "t приём.Ф1"
        Protocol(7, 1) = "t приём.Ф2"
        Protocol(8, 1) = "J ионизации"
        Protocol(9, 1) = "Заброс N1"
        Protocol(10, 1) = "Провал N1"
        Protocol(11, 1) = "Заброс N2"
        Protocol(12, 1) = "Забросов Ттвг"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "сек в ТУ"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "100 мкА"
        Protocol(9, 2) = "Нет"
        Protocol(10, 2) = "% в ТУ"
        Protocol(11, 2) = "Нет"
        Protocol(12, 2) = "Нет"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(32)
        Protocol(6, 3) = GetEngineNormTUParameter(29)
        Protocol(7, 3) = GetEngineNormTUParameter(30)
        Protocol(8, 3) = GetEngineNormTUParameter(38)
        Protocol(9, 3) = GetEngineNormTUParameter(12)
        Protocol(10, 3) = GetEngineNormTUParameter(15)
        Protocol(11, 3) = GetEngineNormTUParameter(13)
        Protocol(12, 3) = GetEngineNormTUParameter(36)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String

        Dim mfrmРегулировка As New FormAdjustment(Parent.TypeKRDinSnapshot)
        mfrmРегулировка.ShowDialog()
        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."

        'риски настройки КРД параметра N1
        parameter = conN1
        Dim clsРискиНастроекПараметровN1 As New РискиНастроекПараметров(parameter,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровN1.Расчет()
        If clsРискиНастроекПараметровN1.IsErrors = False Then
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
        parameter = conN2
        Dim clsРискиНастроекПараметровN2 As New РискиНастроекПараметров(parameter,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровN2.Расчет()
        If clsРискиНастроекПараметровN2.IsErrors = False Then
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
        parameter = conТ4
        Dim clsРискиНастроекПараметровТ4 As New РискиНастроекПараметров(parameter,
                                                                        Parent.FrequencyBackgroundSnapshot,
                                                                        Parent.SnapshotSmallParameters,
                                                                        Parent.XAxisTime.Range.Minimum,
                                                                        Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровТ4.Расчет()
        If clsРискиНастроекПараметровТ4.IsErrors = False Then
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
        parameter = conаРУД
        Dim clsДлительностьФронтаСпада As New ДлительностьФронтаСпада(parameter,
                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                      Parent.MeasuredValues,
                                                                      Parent.SnapshotSmallParameters,
                                                                      Parent.XAxisTime.Range.Minimum,
                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпада
            .Аначальное = 17
            .Аконечное = ВводЗначенияРуда() '111
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
            '************************************************
            'вычисление время первой форсажной приемистости
            parameter = conТокJправый
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
                                                                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                                                                      Parent.MeasuredValues,
                                                                                                                      Parent.SnapshotSmallParameters,
                                                                                                                      Parent.XAxisTime.Range.Minimum,
                                                                                                                      Parent.XAxisTime.Range.Maximum)
            Dim clsМинимальноеМаксимальноеЗначениеПараметраТокJправый As New МинимальноеМаксимальноеЗначениеПараметра(parameter,
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
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.ErrorsMessage & vbCrLf
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
            parameter = conТокJлевый
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
                                                                                                                     Parent.FrequencyBackgroundSnapshot,
                                                                                                                     Parent.MeasuredValues,
                                                                                                                     Parent.SnapshotSmallParameters,
                                                                                                                     Parent.XAxisTime.Range.Minimum,
                                                                                                                     Parent.XAxisTime.Range.Maximum)
            Dim clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый As New МинимальноеМаксимальноеЗначениеПараметра(parameter,
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
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.ErrorsMessage & vbCrLf
            Else
                '************************************************
                'нахождение минимального и максимального значения параметра ТокJправый
                With clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый
                    .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                    .Расчет()
                End With
            End If
            If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.IsErrors = False And clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.IsErrors = False Then
                If clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый.Тдлительность <= clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый.Тдлительность Then
                    'строим(стрелки)
                    With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJправый
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                        ArrowType.Horizontal,
                        parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                        Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                    End With
                    Protocol(8, 2) = Round(clsМинимальноеМаксимальноеЗначениеПараметраТокJправый.МаксимальноеЗначение, 2) & " мка."
                Else
                    'строим стрелки
                    With clsДлительностьФронтаСпадаОтИндексаДоУровняТокJлевый
                        Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                        ArrowType.Horizontal,
                        parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                        Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                    End With
                    Protocol(8, 2) = Round(clsМинимальноеМаксимальноеЗначениеПараметраТокJлевый.МаксимальноеЗначение, 2) & " мка."
                End If
            End If
            '************************************************
            'вычисление время второй форсажной приемистости
            parameter = conПолныйФорсаж
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ As New ДлительностьФронтаСпадаОтИндексаДоУровня(parameter,
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
            If clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ.ErrorsMessage & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаСпадаОтИндексаДоУровняМСТ
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                    Protocol(7, 2) = Round(.Тдлительность, 2) & " сек."
                End With
            End If
        End If

        'вычисляем заброс N1
        parameter = conN1
        Dim clsДлительностьЗабросаПровалаN1 As New ДлительностьЗабросаПровала(parameter,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаN1
            .Аначальное = 50
            .Апорога = mfrmРегулировка.N1НастройкаКРД
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаN1.IsErrors Then
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
                parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %")
                Protocol(9, 2) = parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
            End With
        End If

        'вычисляем заброс N2
        parameter = conN2
        Dim clsДлительностьЗабросаПровалаN2 As New ДлительностьЗабросаПровала(parameter,
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

        If clsДлительностьЗабросаПровалаN2.IsErrors Then
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
                parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %")
                Protocol(11, 2) = parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
            End With
        End If

        'вычисляем заброс Т4
        parameter = conТ4
        Dim clsДлительностьЗабросаПровалаТ4 As New ДлительностьЗабросаПровала(parameter,
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

        If clsДлительностьЗабросаПровалаТ4.IsErrors Then
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
                    parameter & ":заброс=" & dblЗабросT4 & " гр.")
                End If
                Protocol(12, 2) = parameter & ":заброс=" & dblЗабросT4 & " гр."
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
                Protocol(10, 2) = parameter & ":провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class
