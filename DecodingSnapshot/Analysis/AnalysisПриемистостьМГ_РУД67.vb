Imports System.Math
Imports MathematicalLibrary

Friend Class AnalysisПриемистостьМГ_РУД67
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDi_ Protocol(11, 3)
        Re.Dim(Protocol, 11, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Автоматика изделия и НА сработали"
        Protocol(5, 1) = "Сигналы прошли"
        Protocol(6, 1) = "t руд"
        Protocol(7, 1) = "t приём."
        Protocol(8, 1) = "Заброс N1"
        Protocol(9, 1) = "Провал N1"
        Protocol(10, 1) = "Заброс N2"
        Protocol(11, 1) = "Заброс Ттвг"
        'Protocol(13, 1) = "DРС на 67 град."

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "По ТУ"
        Protocol(6, 2) = "сек в ТУ"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "Нет"
        Protocol(9, 2) = "% в ТУ"
        Protocol(10, 2) = "Нет"
        Protocol(11, 2) = "Нет"
        'Protocol(13, 2) = "град."

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = GetEngineNormTUParameter(32)
        Protocol(7, 3) = GetEngineNormTUParameter(28)
        Protocol(8, 3) = GetEngineNormTUParameter(12)
        Protocol(9, 3) = GetEngineNormTUParameter(14)
        Protocol(10, 3) = GetEngineNormTUParameter(13)
        Protocol(11, 3) = GetEngineNormTUParameter(36)
        'Protocol(13, 3) = НормаТУПараметра(35)
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
            .Аконечное = 60 '67
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
                Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
            End With
            '************************************************
            'время восстановления по N1 минус 2%
            parameter = conN1
            Dim clsДлительностьФронтаОтИндексаДоN1Уст_2 As New ДлительностьФронтаОтИндексаДоN1Уст_2(parameter,
                                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                                    Parent.MeasuredValues,
                                                                                                    Parent.SnapshotSmallParameters,
                                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                                    Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаОтИндексаДоN1Уст_2
                .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Расчет()
            End With
            If clsДлительностьФронтаОтИндексаДоN1Уст_2.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьФронтаОтИндексаДоN1Уст_2.ErrorsMessage & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаОтИндексаДоN1Уст_2
                    Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                ArrowType.Horizontal,
                parameter & "уст-2%:dT=" & Round(.Тдлительность, 2) & " сек.")
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
                Protocol(8, 2) = parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
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
                Protocol(10, 2) = parameter & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " %"
            End With
        End If

        'вычисляем заброс Т4
        parameter = conТ4
        Dim clsДлительностьЗабросаПровалаТ4 As New ДлительностьЗабросаПровала(parameter,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.MeasuredValues,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum, Parent.XAxisTime.Range.Maximum)
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
                Protocol(11, 2) = parameter & ":заброс=" & dblЗабросT4 & " гр."
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
                Protocol(9, 2) = parameter & ":провал=" & Round(.DeltaA, 2) & " %"
            End With
        End If
        '************************************************
        'нахождение минимального значения параметра Рт за НП-96М
        parameter = conРтзаНП96М
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
            .ТМинимальногоЗначения,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение - 2),
            .ТМинимальногоЗначения,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МинимальноеЗначение + 2),
            ArrowType.Inclined,
            parameter & ":минимум=" & Round(.МинимальноеЗначение, 2) & " кг")
                'Protocol(5, 2) = "минимум=" & Round(.МинимальноеЗначение, 2) & " кг"
            End With
        End If

        'находим значение ДиаметрРС в конце
        parameter = conДиаметрРС
        Dim clsЗначениеПараметраДиаметрРСВИндексе As New ЗначениеПараметраВИндексе(parameter,
                                                                                   Parent.FrequencyBackgroundSnapshot,
                                                                                   Parent.MeasuredValues,
                                                                                   Parent.SnapshotSmallParameters,
                                                                                   Parent.XAxisTime.Range.Minimum,
                                                                                   Parent.XAxisTime.Range.Maximum)
        With clsЗначениеПараметраДиаметрРСВИндексе
            .ИндексТначальное = CInt((Parent.XAxisTime.Range.Maximum - 1))
            .Расчет()
        End With
        If clsЗначениеПараметраДиаметрРСВИндексе.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsЗначениеПараметраДиаметрРСВИндексе.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsЗначениеПараметраДиаметрРСВИндексе
                Parent.TracingDecodingArrow(
            .Тначальное,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра - 2),
            .Тначальное,
            Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .ЗначениеПараметра + 2),
            ArrowType.Inclined,
            parameter & "=" & Round(.ЗначениеПараметра, 2) & " дел.")
                'Protocol(7, 2) = Round(.ЗначениеПараметра, 2) & " дел."
            End With
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub

End Class

