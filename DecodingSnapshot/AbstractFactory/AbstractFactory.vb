Namespace Factory
	''' <summary>
	''' Базовый класс для наследования.
	''' Реализует методы создания и потверждения интерфейса.
	''' Конкретный экземпляр фабрики будет так же реализовывать другие интерфейсы - IFactoryMethod.
	''' Конкретный экземпляр фабрики приводится к IFactoryMethod, и вызывается метод создания конкретного продукта (return factoryMethod.Create())
	''' </summary>
	Public MustInherit Class AbstractFactory
		Implements IAbstractFactory

		Public Function Create(Of T)() As T Implements IAbstractFactory.Create
			' здесь приведение к одному из реализованных GUIToolkit (наследник) интерфейсов ->  class GUIToolkit : CloneFactory, IFactoryMethod<Edit>, ICloneMethod<Button>
			Dim factoryMethod As IFactoryMethod(Of T) = TryCast(Me, IFactoryMethod(Of T))
			If factoryMethod IsNot Nothing Then
				Return factoryMethod.Create()
			End If
			' Здесь можно вызвать Exception, так как
			' интерфейс IFactoryMethod<T> не реализован 
			Return CType(Nothing, T)
		End Function

		Public Function IsProduct(Of T)() As Boolean Implements IAbstractFactory.IsProduct
			Return TypeOf Me Is IFactoryMethod(Of T)
		End Function
	End Class
End Namespace
