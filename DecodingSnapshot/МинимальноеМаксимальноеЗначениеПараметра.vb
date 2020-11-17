Friend Class МинимальноеМаксимальноеЗначениеПараметра
    Inherits Figure

    '''' <summary>
    '''' Индекс Т начальное
    '''' </summary>
    '''' <returns></returns>
    Public Overloads Property IndexTstart() As Integer
        Get
            Return mIndexTstart
        End Get
        Set(ByVal Value As Integer)
            mIndexTstart = Value
        End Set
    End Property

    ''' <summary>
    ''' Индекс Т конечное
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property IndexTstop() As Integer
        Get
            Return mIndexTstop
        End Get
        Set(ByVal Value As Integer)
            mIndexTstop = Value
        End Set
    End Property

    Public Sub New(inNameParameter As String,
                    frequency As Integer,
                    inMeasuredValues(,) As Double,
                    inTypeSmallParameter() As TypeSmallParameter,
                    minimum As Double, ByVal maximum As Double)

        MyBase.New(inNameParameter, frequency, inMeasuredValues, inTypeSmallParameter, minimum, maximum)
        IndexTstart = GraphMinimum
    End Sub

    Public Overrides Sub Calculation()
        Dim startGraph, stopGraph As Integer

        If ShowTotalErrorsMessage.IsParameterNotCorrect(nameParameter, mErrorsMessage, Parameters, mIndexParameter) Then
            mIsErrors = True
            Exit Sub
        End If

        startGraph = mIndexTstart : stopGraph = mIndexTstop

        If mIndexTstart = mIndexTstop Then startGraph = mGraphMinimum : stopGraph = mGraphMaximum
        If mIndexTstart = mGraphMinimum AndAlso mIndexTstop <> mGraphMinimum Then startGraph = mGraphMinimum : stopGraph = mIndexTstop
        If mIndexTstart <> mGraphMinimum AndAlso mIndexTstop = mGraphMinimum Then startGraph = mIndexTstart : stopGraph = mGraphMaximum
        If mIndexTstart <> 0 AndAlso mIndexTstop = 0 Then startGraph = mIndexTstart : stopGraph = mGraphMaximum

        For I As Integer = startGraph To stopGraph
            ' поиск максимального
            If MeasuredValues(mIndexParameter, I) > mMaxValue Then
                mMaxValue = MeasuredValues(mIndexParameter, I)
                mIndexMaxValue = I
            End If

            ' поиск минимального
            If MeasuredValues(mIndexParameter, I) < mMinValue Then
                mMinValue = MeasuredValues(mIndexParameter, I)
                mIndexMinValue = I
            End If
        Next
    End Sub
End Class
