Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 12 '"Включение КО на аРУД=115 (106)"
''' </summary>
Friend Class AnalysisВключениеКОнаРУД115
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(7, 3)
        Re.Dim(Protocol, 7, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "t восстановления режима"
        Protocol(6, 1) = "t выдачи сигнала <Помпаж>"
        Protocol(7, 1) = "t гидроблокировки"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "сек в ТУ"
        Protocol(7, 2) = "сек в ТУ"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(44)
        Protocol(6, 3) = GetEngineNormTUParameter(45)
        Protocol(7, 3) = GetEngineNormTUParameter(42)
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'находим время Помпаж
        parameter = conПомпаж
        Dim clsДлительностьЗабросаПровалаПомпаж As New ДлительностьЗабросаПровала(parameter,
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

        If clsДлительностьЗабросаПровалаПомпаж.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsДлительностьЗабросаПровалаПомпаж.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаПомпаж
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
                Protocol(6, 2) = Round(.Тдлительность, 2) & " сек."
            End With
        End If

        'находим прохождение сигнала КО
        parameter = conКлапанКО
        Dim clsДлительностьЗабросаПровалаКлапанКО As New ДлительностьЗабросаПровала(parameter,
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

        If clsДлительностьЗабросаПровалаКлапанКО.IsErrors Then
            'анализируем для последующих построений
            'накапливаем ошибку
            IsTotalErrors = True
            totalErrorsMessage += clsДлительностьЗабросаПровалаКлапанКО.ErrorsMessage & vbCrLf
        Else
            'строим стрелки
            With clsДлительностьЗабросаПровалаКлапанКО
                Parent.TracingDecodingArrow(
                .Тначальное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                .Тконечное,
                Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Апорога),
                ArrowType.Horizontal,
                parameter & ":dT=" & Round(.Тдлительность, 2) & " сек.")
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
                .ИндексТначальное = clsДлительностьЗабросаПровалаКлапанКО.ИндексТконечное 'ИндексТначальное
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
                    clsДлительностьЗабросаПровалаКлапанКО.Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    parameter & "уст-2%:dT=" & Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек.")
                    Protocol(5, 2) = Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек."
                End With
            End If

            'гидроблокировка  Рт ОК1К на помпаже
            parameter = conРтОК1К
            Dim clsДлительностьОтИндексаДоСтабильногоРоста As New ДлительностьОтИндексаДоСтабильногоРоста(parameter,
                                                                                                          Parent.FrequencyBackgroundSnapshot,
                                                                                                          Parent.MeasuredValues,
                                                                                                          Parent.SnapshotSmallParameters,
                                                                                                          Parent.XAxisTime.Range.Minimum,
                                                                                                          Parent.XAxisTime.Range.Maximum)
            With clsДлительностьОтИндексаДоСтабильногоРоста
                .ИндексТначальное = clsДлительностьЗабросаПровалаКлапанКО.ИндексТначальное
                .ПорогРостаОтМинимального = 0.3  '1
                .Расчет()
            End With
            If clsДлительностьОтИндексаДоСтабильногоРоста.IsErrors Then
                'анализируем для последующих построений
                'накапливаем ошибку
                IsTotalErrors = True
                totalErrorsMessage += clsДлительностьОтИндексаДоСтабильногоРоста.ErrorsMessage & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьОтИндексаДоСтабильногоРоста
                    Parent.TracingDecodingArrow(
                    .Тначальное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аначальное),
                    .Тконечное,
                    Parent.CastToAxesStandard(Parent.NumberParameterAxes, .ИндексПараметра + 1, .Аконечное),
                    ArrowType.Horizontal,
                    parameter & ":dTстаб=" & Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек.")
                    Protocol(7, 2) = Round(.Тконечное - clsДлительностьЗабросаПровалаКлапанКО.Тначальное, 2) & " сек."
                End With
            End If
        End If

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class

