Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 13 '"Приемистость МГ->N290"
''' </summary>
Friend Class AnalysisПриемистостьМГ_N290
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(6, 3)
        Re.Dim(Protocol, 6, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"
        Protocol(3, 1) = "Температура бокса"
        Protocol(4, 1) = "Сигналы прошли"
        Protocol(5, 1) = "t руд"
        Protocol(6, 1) = "tПомпаж"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "Нет"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = GetEngineNormTUParameter(32)
        Protocol(6, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim totalErrorsMessage As String = Nothing
        Dim IsTotalErrors As Boolean
        Dim parameter As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'находим время приемистости
        parameter = conаРУД
        '************************************************
        'время восстановления по Руд минус 2%
        Dim clsДлительностьФронтаОтИндексаДоРУДУст_2 As New ДлительностьФронтаОтИндексаДоN1Уст_2(parameter,
                                                                                                 Parent.FrequencyBackgroundSnapshot,
                                                                                                 Parent.MeasuredValues,
                                                                                                 Parent.SnapshotSmallParameters,
                                                                                                 Parent.XAxisTime.Range.Minimum,
                                                                                                 Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаОтИндексаДоРУДУст_2
            .Процент = 2
            .Расчет()
        End With

        Dim clsДлительностьФронтаСпада As New ДлительностьФронтаСпада(parameter,
                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                      Parent.MeasuredValues,
                                                                      Parent.SnapshotSmallParameters,
                                                                      Parent.XAxisTime.Range.Minimum,
                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпада
            .Аначальное = 17
            .Аконечное = clsДлительностьФронтаОтИндексаДоРУДУст_2.Аконечное 'До установившегося режима
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
        End If

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

        ShowTotalErrorsMessage.ShowMessage(IsTotalErrors, totalErrorsMessage)
    End Sub
End Class

