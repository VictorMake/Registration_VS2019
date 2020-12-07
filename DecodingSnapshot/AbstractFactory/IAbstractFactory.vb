Namespace Factory
	''' <summary>
	''' Фабрика должна реализовать методы создания и подтверждения
	''' </summary>
	Public Interface IAbstractFactory
		Function Create(Of T)() As T

		Function IsProduct(Of T)() As Boolean
	End Interface
End Namespace
