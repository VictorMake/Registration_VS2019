Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 11 '"Включение КО на N2=90"
''' </summary>
Friend Class AnalysisВключениеКОнаN290
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
        Protocol(4, 1) = "Автоматика изделия и НА сработали"
        Protocol(5, 1) = "Сигналы прошли"
        Protocol(6, 1) = "t восстановления режима"
        Protocol(7, 1) = "t выдачи сигнала <Помпаж>"
        Protocol(8, 1) = "t стендовой блокировки"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "По ТУ"
        Protocol(6, 2) = "сек в ТУ"
        Protocol(7, 2) = "сек в ТУ"
        Protocol(8, 2) = "сек в ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = GetEngineNormTUParameter(43)
        Protocol(7, 3) = GetEngineNormTUParameter(45)
        Protocol(8, 3) = GetEngineNormTUParameter(53)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'находим время Помпаж
        параметр = conПомпаж
        'параметр = conКлапанСУНА 'для пробы
        Dim clsДлительностьЗабросаПровалаПомпаж As New ДлительностьЗабросаПровала(параметр,
                                                                                  Parent.FrequencyBackgroundSnapshot,
                                                                                  Parent.MeasuredValues,
                                                                                  Parent.SnapshotSmallParameters,
                                                                                  Parent.XAxisTime.Range.Minimum,
                                                                                  Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаПомпаж
            .Аначальное = 1
            .Апорога = 4.99
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаПомпаж.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьЗабросаПровалаПомпаж.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаПомпаж
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(7, 2) = Round(.Тдлительность, 2) & " сек."
            End With
        End If

        'находим прохождение сигнала КО
        параметр = conКлапанКО
        Dim clsДлительностьЗабросаПровалаКлапанКО As New ДлительностьЗабросаПровала(параметр,
                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                    Parent.MeasuredValues,
                                                                                    Parent.SnapshotSmallParameters,
                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                    Parent.XAxisTime.Range.Maximum)
        With clsДлительностьЗабросаПровалаКлапанКО
            .Аначальное = 1
            .Апорога = 4.99
            .Расчет()
        End With

        If clsДлительностьЗабросаПровалаКлапанКО.Ошибка = True Then
            'анализируем для последующих построений
            'накапливаем ошибку
            общаяОшибка = True
            общийТекстОшибок += clsДлительностьЗабросаПровалаКлапанКО.ТекстОшибки & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаКлапанКО
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                параметр & ":dT=" & Round(.Тдлительность, 2) & " сек.")
            End With

            '************************************************
            'время восстановления по N1 минус 2%
            параметр = conN1
            Dim clsДлительностьФронтаОтИндексаДоN1Уст_2 As New ДлительностьФронтаОтИндексаДоN1Уст_2(параметр,
                                                                                                    Parent.FrequencyBackgroundSnapshot,
                                                                                                    Parent.MeasuredValues,
                                                                                                    Parent.SnapshotSmallParameters,
                                                                                                    Parent.XAxisTime.Range.Minimum,
                                                                                                    Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаОтИндексаДоN1Уст_2
                .ИндексТначальное = clsДлительностьЗабросаПровалаКлапанКО.ИндексТконечное 'ИндексТначальное
                .Расчет()
            End With
            If clsДлительностьФронтаОтИндексаДоN1Уст_2.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаОтИндексаДоN1Уст_2.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаОтИндексаДоN1Уст_2
                    Parent.TracingDecodingArrow(
                    clsДлительностьЗабросаПровалаКлапанКО.Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    параметр & "уст-2%:dT=" & Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек.")
                    Protocol(6, 2) = Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек."
                End With
            End If
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class

