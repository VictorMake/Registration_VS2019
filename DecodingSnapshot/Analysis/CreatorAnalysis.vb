''' <summary>
''' Класс Создатель объявляет фабричный метод, который должен возвращать
''' объект класса Analysis. Подклассы Создателя обычно предоставляют
''' реализацию этого метода.
''' </summary>
MustInherit Class CreatorAnalysis
    ''' <summary>
    ''' Почти так же, как фабрика, просто дополнительная экспозиция, чтобы сделать что-то с созданным методом
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="inFormMain"></param>
    ''' <returns></returns>
    Protected MustOverride Function CreateAnalysis(name As String, inFormMain As FormSnapshotViewingDiagram) As Analysis

    ' Создатель может также обеспечить реализацию
    ' фабричного метода по умолчанию.
    ''' <summary>
    ''' FactoryMethod
    ''' </summary>
    ''' <returns></returns>      
    Public Function GetAnalysis(name As String, inFormMain As FormSnapshotViewingDiagram) As Analysis
        Return Me.CreateAnalysis(name, inFormMain)
    End Function

    '' Также несмотря на название, основная обязанность
    '' Создателя не заключается в создании форм. Обычно он содержит
    '' некоторую базовую бизнес-логику, которая основана  на объектах
    '' MdiChildrenWindow, возвращаемых фабричным методом.  Подклассы могут косвенно
    '' изменять эту бизнес-логику, переопределяя фабричный метод и возвращая
    '' из него другой тип продукта.
    'Public Function SomeOperation(inMainForm As FrmMain) As String
    '    ' Вызываем фабричный метод, чтобы получить объект-MdiChildrenWindow.
    '    Dim product = CreateWindow(inMainForm)
    '    ' Далее, работаем с этим MdiChildrenWindow.
    '    Dim result = "Creator: The same creator's code has just worked with " & product.Name

    '    Return result
    'End Function
End Class