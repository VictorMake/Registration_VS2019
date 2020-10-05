''' <summary>
''' Класс Создатель объявляет фабричный метод, который должен возвращать
''' объект класса MdiChildrenWindow. Подклассы Создателя обычно предоставляют
''' реализацию этого метода.
''' </summary>
MustInherit Class Creator
    ''' <summary>
    ''' Почти так же, как фабрика, просто дополнительная экспозиция, чтобы сделать что-то с созданным методом
    ''' </summary>
    ''' <param name="inMainForm"></param>
    ''' <returns></returns>
    Protected MustOverride Function CreateWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow

    ' Создатель может также обеспечить реализацию
    ' фабричного метода по умолчанию.
    ''' <summary>
    ''' FactoryMethod
    ''' </summary>
    ''' <returns></returns>      
    Public Function GetWindow(inMainForm As FormMainMDI, ByVal captionText As String) As IMdiChildrenWindow
        Return Me.CreateWindow(inMainForm, captionText)
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