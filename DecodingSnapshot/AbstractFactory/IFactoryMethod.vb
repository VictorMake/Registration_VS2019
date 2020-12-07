Namespace Factory
	''' <summary>
	''' Интерфейс фабричного метода создания продукта
	''' </summary>
	''' <typeparam name="T"></typeparam>
	Public Interface IFactoryMethod(Of T)
		Function Create() As T
	End Interface
End Namespace

