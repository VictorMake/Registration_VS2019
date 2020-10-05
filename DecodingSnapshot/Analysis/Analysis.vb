Friend MustInherit Class Analysis
    Public Property Name() As String
    Public Parent As FormSnapshotViewingDiagram
    Public Property Protocol As String(,) = New String(,) {}
    'Dim Protocol As String(,) = New String(11, 3) {}
    Private CurrentEngine As Engine

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        Me.Name = name
        Me.Parent = inFormMain
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
End Class