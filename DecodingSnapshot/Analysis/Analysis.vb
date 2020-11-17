''' <summary>
''' Базовый класс рассшифровки режима
''' </summary>
Friend MustInherit Class Analysis
    Public Property Name() As String
    Public Property Protocol As String(,) = New String(,) {}

    Friend Property Parent As FormSnapshotViewingDiagram
    'Dim Protocol As String(,) = New String(11, 3) {}
    Private CurrentEngine As Engine
    Protected mFiguresManager As FiguresManager
    Protected totalErrorsMessage As String
    Protected IsTotalErrors As Boolean
    Protected parameter As String

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        Me.Name = name
        Me.Parent = inFormMain
        mFiguresManager = New FiguresManager(inFormMain)
        mFiguresManager.Initialize()
    End Sub

    ''' <summary>
    ''' Расшифровать
    ''' </summary>
    Public MustOverride Sub DecodingRegimeSnapshot()

    ''' <summary>
    ''' Делегирование определения нормы ТУ к конкретному типу изделия введенному при загрузке программы
    ''' </summary>
    Public Sub EngineDefineTU()
        Dim mEngineManager As New EngineManager

        ' определить текущий режим и текущий тип изделия
        ' если изделие удалось создать из EngineManager, то запустить DefineTU
        CurrentEngine = mEngineManager(ModificationEngine)

        If CurrentEngine IsNot Nothing Then
            CurrentEngine.DefineTU()
        End If
        Сleaning()
    End Sub

    Private Sub Сleaning()
        totalErrorsMessage = String.Empty
        IsTotalErrors = False
    End Sub

    ''' <summary>
    ''' Делегирование выдачи нормы ТУ к конкретному типу изделия введенному при загрузке программы
    ''' </summary>
    ''' <param name="numberArticle"></param>
    ''' <returns></returns>
    Public Function GetEngineNormTUParameter(ByVal numberArticle As Integer) As String
        If CurrentEngine IsNot Nothing Then
            Return CurrentEngine.GetNormTUParameter(numberArticle)
        Else
            Return "Не найдена"
        End If
    End Function

    Public Sub PopulateProtocol(index As Integer, line As String())
        Protocol(index, 1) = line(0) : Protocol(index, 2) = line(1) : Protocol(index, 3) = line(2)
    End Sub

    ''' <summary>
    ''' Привести к оси эталона аналогичная функции FormMain.
    ''' Упрощает доступ из классов унаследованных от Analysis.
    ''' </summary>
    ''' <param name="indexParameter"></param>
    ''' <param name="physicalValue"></param>
    ''' <returns></returns>
    Friend Function CastToAxesStandard(indexParameter As Integer, physicalValue As Double) As Double
        ' physicalValue - физическое значение после полинома
        Dim NStandard As Integer = Parent.NumberParameterAxes ' - номер выбранного параметра для эталонной оси
        Dim NCurrent As Integer = indexParameter + 1 ' - номер текущего параметра
        With Parent
            Return .RangesOfDeviation(NStandard, 1) + (.RangesOfDeviation(NStandard, 2) - .RangesOfDeviation(NStandard, 1)) * (physicalValue - .RangesOfDeviation(NCurrent, 1)) / (.RangesOfDeviation(NCurrent, 2) - .RangesOfDeviation(NCurrent, 1))
        End With
    End Function
End Class