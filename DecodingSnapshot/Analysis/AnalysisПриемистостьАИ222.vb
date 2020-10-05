Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 28 '"Приемистость АИ-222"
''' </summary>
Friend Class AnalysisПриемистостьАИ222
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
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "t nпр. ЗМГ или ПНГ" 'меняется в программе
        Protocol(7, 1) = "Заброс nндпр от устан. значения"
        Protocol(8, 1) = "Заброс ntТНД от устан. значения"
        Protocol(9, 1) = "Превышение tТНД (790гр.)" ' более 1.5 сек"
        Protocol(10, 1) = "Автоматика изделия и НА сработали"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "%"
        Protocol(7, 2) = "%"
        Protocol(8, 2) = "%"
        Protocol(9, 2) = "Нет" ' или "Есть"'меняется в программе
        Protocol(10, 2) = "По ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = "не более 1 сек"
        Protocol(6, 3) = ""
        Protocol(7, 3) = "<=0.01%"
        Protocol(8, 3) = ""
        Protocol(9, 3) = "не более 1.5 сек"
        Protocol(10, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Dim strДобавка As String = "ПМГ"
        Dim clsЗначениеПараметраЗемляАИ222ВИндексе As ЗначениеПараметраВИндексе = Nothing

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
            .Аначальное = 20
            .Аконечное = 65
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
                    If .ЗначениеПараметра >= 1 Then
                        strДобавка = "ЗМГ"
                    Else
                        strДобавка = "ПМГ"
                    End If
                End With
            End If

            'время восстановления по conNндпрАИ222 минус 5%
            параметр = conNндпрАИ222
            Dim clsДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5 As New ДлительностьФронтаОтИндексаДоN1Уст_2(параметр,
                                                                                                            Parent.FrequencyBackgroundSnapshot,
                                                                                                            Parent.MeasuredValues,
                                                                                                            Parent.SnapshotSmallParameters,
                                                                                                            Parent.XAxisTime.Range.Minimum,
                                                                                                            Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5
                .Процент = 5
                .ИндексТначальное = clsДлительностьФронтаСпадаРУДАИ222.ИндексТначальное
                .Расчет()
            End With
            If clsДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаОтИндексаДоNндпрАИ222Уст_5
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    параметр & strДобавка & "уст-5%:dT=" & Round(.Тдлительность, 2) & " сек.")
                    Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
                End With
                If clsЗначениеПараметраЗемляАИ222ВИндексе.ЗначениеПараметра >= 1 Then
                    'strДобавка = "ЗМГ"
                    Protocol(6, 3) = "<=6 сек."
                Else
                    'strДобавка = "ПМГ"
                    Protocol(6, 3) = "<=4 сек."
                End If
                Protocol(6, 1) = параметр & strДобавка
            End If
        End If

        'находим заброс NндпрАИ222 относительно установившегося
        параметр = conNндпрАИ222
        Dim clsЗабросNндпрАИ222ОтносительноУстановившегося As New ЗабросN1ОтносительноУстановившегося(параметр,
                                                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                                                      Parent.MeasuredValues,
                                                                                                      Parent.SnapshotSmallParameters,
                                                                                                      Parent.XAxisTime.Range.Minimum,
                                                                                                      Parent.XAxisTime.Range.Maximum)
        With clsЗабросNндпрАИ222ОтносительноУстановившегося
            .ИндексТначальное = clsДлительностьФронтаСпадаРУДАИ222.ИндексТконечное 'отсчет от этой точки минус 5 секунды
            .Расчет()
        End With

        If clsЗабросNндпрАИ222ОтносительноУстановившегося.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsЗабросNндпрАИ222ОтносительноУстановившегося.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsЗабросNндпрАИ222ОтносительноУстановившегося
                If .DeltaA > 0 Then
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Vertical,
                    параметр & strДобавка & ":уст. заброс=" & Round(.DeltaA, 2) & " %")
                    Protocol(7, 2) = параметр & strДобавка & ":уст. заброс=" & Round(.DeltaA, 2) & " %"
                    'If clsЗначениеПараметраЗемляАИ222ВИндексе IsNot Nothing Then
                    '    If clsЗначениеПараметраЗемляАИ222ВИндексе.ЗначениеПараметра >= 1 Then
                    '        'strДобавка = "ЗМГ"
                    '        Protocol(7, 3) = "ниже 57.5 %"
                    '    Else
                    '        'strДобавка = "ПМГ"
                    '        Protocol(7, 3) = "ниже 67.5 %"
                    '    End If
                    'End If
                End If
            End With
        End If

        Dim ТндАИ222_790 As Double = 790
        't*тнд
        'риски ТндАИ222 для величины 790 гр.
        параметр = conТндАИ222
        Dim clsРискиНастроекПараметровТндАИ222 As New РискиНастроекПараметров(параметр,
                                                                              Parent.FrequencyBackgroundSnapshot,
                                                                              Parent.SnapshotSmallParameters,
                                                                              Parent.XAxisTime.Range.Minimum,
                                                                              Parent.XAxisTime.Range.Maximum)
        clsРискиНастроекПараметровТндАИ222.Расчет()
        If clsРискиНастроекПараметровТндАИ222.Ошибка = False Then
            'строим стрелки
            With clsРискиНастроекПараметровТндАИ222
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, ТндАИ222_790),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, ТндАИ222_790),
                ArrowType.Inclined,
                параметр & "=" & CStr(ТндАИ222_790) & " гр.")
            End With
        End If

        'вычисляем заброс conТндАИ222 от величины 790 гр
        параметр = conТндАИ222
        Dim clsДлительностьЗабросаПровалаТндАИ222 As New ДлительностьЗабросаПровала(параметр,
                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                    Parent.MeasuredValues,
                                                                                    Parent.SnapshotSmallParameters,
                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                    Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаТндАИ222
            .Аначальное = 600
            .Апорога = ТндАИ222_790
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаТндАИ222.Ошибка = True Then
            'анализируем для последующих построений
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаТндАИ222
                Parent.TracingDecodingArrow(
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .ТМаксимальногоЗначения,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .МаксимальноеЗначение),
                ArrowType.Vertical,
                параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " гр.")
                Protocol(8, 2) = параметр & ":заброс=" & Round(.МаксимальноеЗначение - .Апорога, 2) & " гр."
                If .МаксимальноеЗначение > ТндАИ222_790 Then
                    'строим стрелки
                    Parent.TracingDecodingArrow(
                        .Тначальное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                        .Тконечное,
                        Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                        ArrowType.Horizontal,
                        параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                    Protocol(9, 2) = Round(.Тдлительность, 2) & " сек."
                End If
            End With
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

