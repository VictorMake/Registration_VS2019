Namespace Factory
    ''' <summary>
    ''' Интерфейс IFactoryMethod(Of T) не имеет метода Create(), зато содержит свойство Prototype, 
    ''' которое и является прототипом продукта T, предназначенного для клонирования.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Interface ICloneMethod(Of T As ICloneable)
        Inherits IFactoryMethod(Of T)

        Property Prototype() As T
    End Interface
End Namespace

