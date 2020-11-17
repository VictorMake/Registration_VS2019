Imports System.Collections.Generic
Imports MathematicalLibrary

Friend Class FormPatternGraphByParameter
    Private queueDataX As New Queue(Of Double)(Dynamics)
    Private queueDataY As New Queue(Of Double)(Dynamics)
    Private xData(), yData() As Double
    Private ReadOnly mGrafOfParam As GraphsOfParameters.GraphOfParameter

    Private title As String = ""
    Private captionGraf As String = ""
    Private FrequencySampleRateSnapshot As Integer ' частота Фонового Снимка
    Private isModeSnapshot As Boolean ' режим Снимок
    Private isMoveTimeSlide As Boolean
    Private isMoveXyCursor As Boolean
    Public Property ParentFormMain() As FormMain
    Public Property NameGraphByParameter() As String ' Имя Графика От Параметра 

    Sub New(ByVal inParentForm As FormMain, ByVal inNameGraphByParameter As String, ByVal tempGrafOfParam As GraphsOfParameters.GraphOfParameter)
        MyBase.New()
        ParentFormMain = inParentForm
        Me.NameGraphByParameter = inNameGraphByParameter
        mGrafOfParam = tempGrafOfParam

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MdiParent = MainMdiForm
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        Name = NameGraphByParameter
        Text = NameGraphByParameter
        Tag = NameGraphByParameter

        If ParentFormMain.GraphsByParameters Is Nothing Then
            ParentFormMain.GraphsByParameters = New Dictionary(Of String, FormPatternGraphByParameter)
        End If

        ParentFormMain.GraphsByParameters.Add(NameGraphByParameter, Me)
    End Sub

    Private Sub FormPatternGraphByParameter_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' работаем с уже сконфигурированным экземпляром mGrafOfParam()
        For Each itemAxis As GraphsOfParameters.GraphOfParameter.Axis In mGrafOfParam.AxisGraph.Values
            'Dim ИмяОси As String = TempОсь.NameAxis
            ' анализ значения атрибута 
            Dim min As Double = itemAxis.Min
            Dim max As Double = itemAxis.Max
            Dim isUseFormula As Boolean = itemAxis.IsUseFormula
            Dim range As Range = New Range(min, max)
            Dim captionAxis As String

            If isUseFormula Then
                captionAxis = itemAxis.Formula

                For Each TempПараметр As GraphsOfParameters.GraphOfParameter.Axis.Parameter In itemAxis.Parameters.Values
                    captionAxis = captionAxis.Replace(TempПараметр.Name, TempПараметр.NameChannel)
                Next
            Else
                captionAxis = itemAxis.ParameterForAxis.NameChannel

                If itemAxis.ParameterForAxis.Reduction Then
                    captionAxis += " приведенный"
                End If
            End If

            If captionAxis.Length > 40 Then captionAxis = captionAxis.Substring(0, 40) & "..."

            If itemAxis.NameAxis = "X" Then
                XAxis1.Range = range
                XAxis1.Caption = captionAxis
            Else
                YAxis1.Range = range
                YAxis1.Caption = captionAxis
            End If
        Next

        For Each itemLimitationGraph As GraphsOfParameters.GraphOfParameter.LimitationGraph In mGrafOfParam.AllLimitationGraphs.Values
            Dim nameLimitGraph As String = itemLimitationGraph.NameLimitGraph 'iteratorГрафикТУ.Current.GetAttribute("ИмяТУ", String.Empty)
            Dim sColor As String = itemLimitationGraph.Color 'iteratorГрафикТУ.Current.GetAttribute("Color", String.Empty)
            Dim sLineStyle As String = itemLimitationGraph.LineStyle 'iteratorГрафикТУ.Current.GetAttribute("LineStyle", String.Empty)
            Dim scatterPlotTemp As ScatterPlot = New ScatterPlot With {
                .LineColor = Color.FromName(sColor),
                .PointColor = Color.Red,
                .PointStyle = PointStyle.Cross 'SolidDiamond
                }
            'ScatterPlotTemp.LineColor = System.Drawing.Color.White

            Dim values As Array = EnumObject.GetValues(ScatterPlot1.LineStyle.UnderlyingType)
            Dim valueTemp As LineStyle = LineStyle.Dot ' по умолчанию

            For I As Integer = 0 To values.Length - 1
                If values.GetValue(I).ToString = sLineStyle Then
                    valueTemp = CType(values.GetValue(I), LineStyle)
                    Exit For
                End If
            Next

            scatterPlotTemp.LineStyle = CType(valueTemp, LineStyle)
            scatterPlotTemp.XAxis = XAxis1
            scatterPlotTemp.YAxis = YAxis1
            scatterPlotTemp.Tag = nameLimitGraph

            For Each itemPoint As GraphsOfParameters.GraphOfParameter.LimitationGraph.PointGraph In itemLimitationGraph.PointsGraphDictionary.Values
                scatterPlotTemp.PlotXYAppend(itemPoint.X, itemPoint.Y)
            Next

            CWGraphA1.Plots.AddRange(New ScatterPlot() {scatterPlotTemp})
        Next

        CWGraphA1.Caption = $"График значения параметра {YAxis1.Caption} от параметра {XAxis1.Caption}"
        lblX.Text = XAxis1.Caption
        lblY.Text = YAxis1.Caption

        'If ParentForm.ТипИспытания = FormExamination.RegistrationSCXI OrElse ParentForm.ТипИспытания = FormExamination.RegistrationClient Then
        If CBool(ParentFormMain.KindFormExamination And FormExamination.RegistrationBase) Then
            XyCursor1.Visible = False
            SlideTime.Visible = False
        ElseIf ParentFormMain.KindFormExamination = FormExamination.SnapshotViewingDiagram Then
            isModeSnapshot = True
            XyCursor1.MoveCursor((XAxis1.Range.Minimum + XAxis1.Range.Maximum) / 2, (YAxis1.Range.Minimum + YAxis1.Range.Maximum) / 2)
            XyCursor1.Visible = True

            SlideTime.Visible = True
            FrequencySampleRateSnapshot = ParentFormMain.SampleRate
            SlideTime.Range = New Range(Convert.ToDouble(ParentFormMain.WaveformGraphTime.XAxes(0).Range.Minimum / FrequencySampleRateSnapshot),
                                           Convert.ToDouble(ParentFormMain.WaveformGraphTime.XAxes(0).Range.Maximum / FrequencySampleRateSnapshot))
        End If
    End Sub

    Public Sub CloseForm()
        Close()
    End Sub

    Private Sub TSMenuItemExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemExit.Click
        Close()
    End Sub

    Private Sub FormPatternGraphByParameter_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormMain.ClearMarkFromMenu(NameGraphByParameter)
        ParentFormMain.GraphsByParameters.Remove(NameGraphByParameter) ' ссылка на Родительскую Форму
        ParentFormMain = Nothing
    End Sub

    ''' <summary>
    ''' Регистрация Графики
    ''' </summary>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    Friend Sub UpdateAsquredGraphByParameter(ByVal X As Double, ByVal Y As Double)
        LabelIndicatorX.Text = Format(X, "##0.0")
        LabelIndicatorY.Text = Format(Y, "##0.0")

        If queueDataX.Count = Dynamics Then
            queueDataX.Dequeue()
            queueDataY.Dequeue()
        End If

        queueDataX.Enqueue(X)
        queueDataY.Enqueue(Y)
        ScatterPlot1.PlotXY(queueDataX.ToArray, queueDataY.ToArray)
    End Sub

    ''' <summary>
    ''' Снимок Графики
    ''' </summary>
    ''' <param name="xDataSourse"></param>
    ''' <param name="yDataSourse"></param>
    ''' <param name="inFrequencySampleRateSnapshot"></param>
    Friend Sub UpdateSnapshotGraphByParameter(ByVal xDataSourse() As Double, ByVal yDataSourse() As Double, ByVal inFrequencySampleRateSnapshot As Integer)
        'ReDim_xData(xDataSourse.Length - 1)
        'ReDim_yData(yDataSourse.Length - 1)
        Re.Dim(xData, xDataSourse.Length - 1)
        Re.Dim(yData, yDataSourse.Length - 1)


        Array.Copy(xDataSourse, xData, xDataSourse.Length)
        Array.Copy(yDataSourse, yData, yDataSourse.Length)

        If Me.FrequencySampleRateSnapshot <> inFrequencySampleRateSnapshot Then
            SlideTime.Range = New Range(Convert.ToDouble(ParentFormMain.WaveformGraphTime.XAxes(0).Range.Minimum / inFrequencySampleRateSnapshot),
                                                                  Convert.ToDouble(ParentFormMain.WaveformGraphTime.XAxes(0).Range.Maximum / inFrequencySampleRateSnapshot))
            Me.FrequencySampleRateSnapshot = inFrequencySampleRateSnapshot
        End If

        ScatterPlot1.PlotXY(xData, yData)
    End Sub

    Public Sub XyCursor1_AfterMove(ByVal sender As Object, ByVal e As AfterMoveXYCursorEventArgs) Handles XyCursor1.AfterMove
        If XyCursor1.Visible Then
            isMoveXyCursor = True

            If Not isMoveTimeSlide AndAlso xData IsNot Nothing Then
                Dim индексXDataOтКурсора As Integer = Array.IndexOf(xData, e.XPosition)
                If индексXDataOтКурсора <> -1 AndAlso Array.IndexOf(yData, e.YPosition) <> -1 Then
                    SlideTime.Value = SlideTime.Range.Minimum + индексXDataOтКурсора / FrequencySampleRateSnapshot
                End If
            End If

            LabelIndicatorX.Text = Format(e.XPosition, "##0.0")
            LabelIndicatorY.Text = Format(e.YPosition, "##0.0")
            isMoveXyCursor = False
        End If
    End Sub

    Private Sub SlideTime_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideTime.AfterChangeValue
        isMoveTimeSlide = True
        SlideTime.CustomDivisions(0).Text = Format(SlideTime.Value, "0.00")
        SlideTime.CustomDivisions(0).Value = SlideTime.Value

        If Not isMoveXyCursor Then
            If xData IsNot Nothing Then ' чтобы не было ошибки из frmШаблонГрафикаОтПараметра_Load при установке CWSlideВремя.Range
                ' на случай если был масштабирован график
                Dim индексОтСлайдера As Integer = CInt((SlideTime.Value - SlideTime.Range.Minimum) * FrequencySampleRateSnapshot)
                If индексОтСлайдера > 0 AndAlso индексОтСлайдера < xData.Length Then
                    XyCursor1.XPosition = xData(индексОтСлайдера)
                End If
            End If
        End If

        ParentFormMain.XyCursorStart.XPosition = SlideTime.Value * FrequencySampleRateSnapshot
        isMoveTimeSlide = False
    End Sub

    Private Sub TSMenuItemSaveGraf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemSaveGraf.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(TSMenuItemSaveGraf_Click)}> сохранение графика от параметра")
        ShowCaption()
        Dim convertColor As ConvertColorForGraph = New ConvertColorForGraph(CWGraphA1, False)
        CWGraphA1.BackColor = Color.White
        CWGraphA1.Caption = captionGraf
        XyCursor1.Visible = False

        Try
            convertColor.ChangeColorOnGraph(Color.White, Color.Black)
            Dim graphSave As GraphSave = New GraphSave(CWGraphA1)
            graphSave.Save()
        Catch ex As Exception
            Const caption As String = "Ошибка записи графика"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            convertColor.ChangeColorOnGraph(Color.Black, Color.White)
            CWGraphA1.Caption = ""

            If isModeSnapshot Then XyCursor1.Visible = True
        End Try
    End Sub

    Private Const vgaWidht As Integer = 1024
    Private Const vgaHeight As Integer = 768

    Private Sub TSMenuItemPrintGraf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemPrintGraf.Click
        ' пользователь нажал да.
        If MessageBox.Show("Печать на весь лист?", "Печать графика", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            WindowState = FormWindowState.Normal
            If Screen.PrimaryScreen.Bounds.Width = vgaWidht OrElse Screen.PrimaryScreen.Bounds.Height = vgaHeight Then
                Width = 1036
                Height = 780
                MessageBox.Show("Установите альбомную ориентацию!", "Печать графика", MessageBoxButtons.OK, MessageBoxIcon.Information)
                PrintGraph()
                WindowState = FormWindowState.Maximized
            ElseIf Screen.PrimaryScreen.Bounds.Width > vgaWidht OrElse Screen.PrimaryScreen.Bounds.Height > vgaHeight Then
                Width = 762
                Height = 1036
                PrintGraph()
                WindowState = FormWindowState.Maximized
            Else
                PrintGraph()
            End If
        Else
            PrintGraph()
        End If
    End Sub

    Private Sub PrintGraph()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Печатать графика от параметра")
        ShowCaption()
        Dim convertColor As ConvertColorForGraph = New ConvertColorForGraph(CWGraphA1, False)
        CWGraphA1.BackColor = Color.White
        CWGraphA1.Caption = captionGraf
        XyCursor1.Visible = False

        Try
            convertColor.ChangeColorOnGraph(Color.White, Color.Black)
            Dim printer As GraphPrinter = New GraphPrinter(CWGraphA1)
            printer.Print()
        Catch ex As Exception
            Const caption As String = "Ошибка печати графика"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            convertColor.ChangeColorOnGraph(Color.Black, Color.White)
            CWGraphA1.Caption = ""

            If isModeSnapshot Then XyCursor1.Visible = True
        End Try
    End Sub

    ''' <summary>
    ''' Ввести Примечание
    ''' </summary>
    Private Sub ShowCaption()
        title = InputBox("Введите текст примечания к снимку (не обязательно)", captionGraf, title)
        If title <> "" AndAlso captionGraf.LastIndexOf(title) = -1 Then
            captionGraf = $"{captionGraf} {title}"
        Else
            captionGraf = title
        End If
    End Sub
End Class