Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 5 '"Сброс РУД115 (106) -> МГ"
''' </summary>
Friend Class AnalysisСбросРУД115_МГ
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
        Protocol(6, 1) = "t сброса"

        Protocol(1, 2) = CStr(Parent.NumberProductionSnapshot)
        Protocol(2, 2) = "п/заказчика"
        Protocol(3, 2) = TemperatureOfBox & "град."
        Protocol(4, 2) = "По ТУ"
        Protocol(5, 2) = "сек в ТУ"
        Protocol(6, 2) = "сек"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
        Protocol(3, 3) = ""
        Protocol(4, 3) = ""
        Protocol(5, 3) = ""
        Protocol(6, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        Dim общийТекстОшибок As String = Nothing
        Dim общаяОшибка As Boolean
        Dim параметр As String

        Protocol(3, 2) = CStr(Round(TemperatureBoxInSnaphot, 2)) & "град."
        'находим время приемистости
        параметр = conаРУД
        Dim clsДлительностьФронтаСпада As New ДлительностьФронтаСпада(параметр,
                                                                      Parent.FrequencyBackgroundSnapshot,
                                                                      Parent.MeasuredValues,
                                                                      Parent.SnapshotSmallParameters,
                                                                      Parent.XAxisTime.Range.Minimum,
                                                                      Parent.XAxisTime.Range.Maximum)
        With clsДлительностьФронтаСпада
            .Аначальное = ВводЗначенияРуда() '111
            .Аконечное = 17
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
            параметр = conN1
            Dim clsДлительностьФронтаСпадаОтИндексаДоУровня As New ДлительностьФронтаСпадаОтИндексаДоУровня(параметр,
                                                                                                            Parent.FrequencyBackgroundSnapshot,
                                                                                                            Parent.MeasuredValues,
                                                                                                            Parent.SnapshotSmallParameters,
                                                                                                            Parent.XAxisTime.Range.Minimum,
                                                                                                            Parent.XAxisTime.Range.Maximum)
            With clsДлительностьФронтаСпадаОтИндексаДоУровня
                .ИндексТначальное = clsДлительностьФронтаСпада.ИндексТначальное
                .Аконечное = 43
                .Расчет()
            End With
            If clsДлительностьФронтаСпадаОтИндексаДоУровня.Ошибка = True Then
                'анализируем для последующих построений
                'накапливаем ошибку
                общаяОшибка = True
                общийТекстОшибок += clsДлительностьФронтаСпадаОтИндексаДоУровня.ТекстОшибки & vbCrLf
            Else
                'строим стрелки
                With clsДлительностьФронтаСпадаОтИндексаДоУровня
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
        End If

        'если накопленная ошибка во всех классах
        If общаяОшибка = True Then
            MessageBox.Show(общийТекстОшибок, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class