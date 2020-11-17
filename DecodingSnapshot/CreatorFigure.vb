''' <summary>
''' Класс Создатель объявляет фабричный метод, который должен возвращать
''' объект класса Figure. Подклассы Создателя обычно предоставляют
''' реализацию этого метода.
''' </summary>
MustInherit Class CreatorFigure
    ''' <summary>
    ''' Почти так же, как фабрика, просто дополнительная экспозиция, чтобы сделать что-то с созданным методом
    ''' </summary>
    ''' <param name="parameter"></param>
    ''' <param name="inFormMain"></param>
    ''' <returns></returns>
    Protected MustOverride Function CreateFigure(parameter As String, inFormMain As FormSnapshotViewingDiagram) As Figure

    ' Создатель может также обеспечить реализацию
    ' фабричного метода по умолчанию.
    ''' <summary>
    ''' FactoryMethod
    ''' </summary>
    ''' <returns></returns>      
    Friend Function GetFigure(parameter As String, inFormMain As FormSnapshotViewingDiagram) As Figure
        Return Me.CreateFigure(parameter, inFormMain)
    End Function
End Class
