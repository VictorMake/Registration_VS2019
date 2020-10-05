Imports NationalInstruments.UI.WindowsForms
Public Class ConvertColorForGraph
    Private scatterXYWaveformGraph As XYGraph ' родительский класс для ScatterGraph и WaveformGraph
    Private isGraphParamByParam As Boolean

    ''' <summary>
    ''' Принимает ScatterGraph.
    ''' Перегруженный.
    ''' </summary>
    ''' <param name="scGraph"></param>
    ''' <param name="inIsGraphParamByParam"></param>
    Public Sub New(ByVal scGraph As ScatterGraph, ByVal inIsGraphParamByParam As Boolean)
        If scGraph Is Nothing Then
            Throw New ArgumentNullException("Ошибка изменения цвета ScatterGraph")
        End If

        scatterXYWaveformGraph = scGraph
        isGraphParamByParam = inIsGraphParamByParam
    End Sub

    ''' <summary>
    ''' Принимает WaveformGraph.
    ''' Перегруженный.
    ''' </summary>
    ''' <param name="wfGraph"></param>
    ''' <param name="inIsGraphParamByParam"></param>
    Public Sub New(ByVal wfGraph As WaveformGraph, ByVal inIsGraphParamByParam As Boolean)
        If wfGraph Is Nothing Then
            Throw New ArgumentNullException("Ошибка изменения цвета WaveformGraph")
        End If

        scatterXYWaveformGraph = wfGraph
        isGraphParamByParam = inIsGraphParamByParam
    End Sub

    ''' <summary>
    ''' Изменить Цвет На Графике
    ''' </summary>
    ''' <param name="startColor"></param>
    ''' <param name="endColor"></param>
    Public Sub ChangeColorOnGraph(ByRef startColor As Color, ByRef endColor As Color)
        Dim I As Integer

        With scatterXYWaveformGraph
            .PlotAreaColor = startColor

            If startColor = Color.White Then
                .BackColor = Color.White
            Else
                If isGraphParamByParam Then
                    .BackColor = Color.DimGray 'Silver
                Else
                    .BackColor = SystemColors.Control
                End If
            End If

            For I = 0 To .YAxes.Count - 1
                If .YAxes(I).CaptionForeColor.Equals(startColor) Then
                    .YAxes(I).CaptionForeColor = endColor
                    .YAxes(I).MajorDivisions.LabelForeColor = endColor
                    .YAxes(I).MajorDivisions.TickColor = endColor
                    .YAxes(I).MinorDivisions.TickColor = endColor
                End If
            Next

            For I = 0 To .XAxes.Count - 1
                If .XAxes(I).CaptionForeColor.Equals(startColor) Then
                    .XAxes(I).CaptionForeColor = endColor
                    .XAxes(I).MajorDivisions.LabelForeColor = endColor
                    .XAxes(I).MajorDivisions.TickColor = endColor
                    .XAxes(I).MinorDivisions.TickColor = endColor
                End If
            Next

            For I = 0 To .Annotations.Count - 1
                .Annotations(I).ArrowColor = endColor
                .Annotations(I).CaptionForeColor = endColor
            Next

            If TypeOf scatterXYWaveformGraph Is ScatterGraph Then
                For Each plot As ScatterPlot In CType(scatterXYWaveformGraph, ScatterGraph).Plots
                    If plot.LineColor.Equals(startColor) Then plot.LineColor = endColor
                Next
            ElseIf TypeOf scatterXYWaveformGraph Is WaveformGraph Then
                For Each plot As WaveformPlot In CType(scatterXYWaveformGraph, WaveformGraph).Plots
                    If plot.LineColor.Equals(startColor) Then plot.LineColor = endColor
                Next
            End If
        End With
    End Sub
End Class