Namespace Factory
	''' <summary>
	''' Наследница CloneFactory и реализует интерфейсы IFactoryMethod<> и ICloneMethod<>.
	''' Конкретный экземпляр фабрики реализует фабричные интерфейсы по созданию конкретных продуктов.
	''' Обобщённый интерфейс базового класса {public T Create<T>()} приводится к интерфейсу фабричного метода
	''' и вызывает метод создания конкретного продукта.
	''' Фабрика клонирует объекты-прототипы, которые содержит в себе.
	''' </summary>
	Public Class ConcreteFactory1
		Inherits CloneFactory
		Implements ICloneMethod(Of AbstractProductA), IFactoryMethod(Of AbstractProductB)

		' При передаче типа Edit в Create<>() с помощью фабричного метода будет создан объект StyledEdit. 
		' При передаче Button будет клонирован объект buttonPrototype. Важно, чтобы класс Button реализовывал интерфейс IClonable.

		' Моя версия
		' реализовать 1 интерфейс
		'private Edit editPrototype;

		'Edit IFactoryMethod<Edit>.Create()
		'{ return (Edit)editPrototype.Clone(); }

		' реализовать 2 интерфейса
		Private buttonPrototype As AbstractProductA

		Private Property ICloneMethodGeneric_Prototype() As AbstractProductA Implements ICloneMethod(Of AbstractProductA).Prototype
			Get
				Return buttonPrototype
			End Get
			Set(ByVal value As AbstractProductA)
				buttonPrototype = value
			End Set
		End Property

		Private Function CreateProductA() As AbstractProductA Implements IFactoryMethod(Of AbstractProductA).Create
			Return DirectCast(buttonPrototype.Clone(), AbstractProductA)
		End Function

		' Было без моего добавления ICloneable
		Private Function CreateProductB() As AbstractProductB Implements IFactoryMethod(Of AbstractProductB).Create
			Return New ProductB1()
		End Function
	End Class

	''' <summary>
	''' Наследница CloneFactory и реализует интерфейсы IFactoryMethod<> и ICloneMethod<>.
	''' Конкретный экземпляр фабрики реализует фабричные интерфейсы по созданию конкретных продуктов.
	''' Обобщённый интерфейс базового класса {public T Create<T>()} приводится к интерфейсу фабричного метода
	''' и вызывает метод создания конкретного продукта.
	''' Фабрика клонирует объекты-прототипы, которые содержит в себе.
	''' </summary>
	Public Class ConcreteFactory2
		Inherits CloneFactory
		Implements ICloneMethod(Of AbstractProductA), IFactoryMethod(Of AbstractProductB)

		' При передаче типа Edit в Create<>() с помощью фабричного метода будет создан объект StyledEdit. 
		' При передаче Button будет клонирован объект buttonPrototype. Важно, чтобы класс Button реализовывал интерфейс IClonable.

		' Моя версия
		' реализовать 1 интерфейс
		'private Edit editPrototype;

		'Edit IFactoryMethod<Edit>.Create()
		'{ return (Edit)editPrototype.Clone(); }

		' реализовать 2 интерфейса
		Private buttonPrototype As AbstractProductA

		Private Property ICloneMethodGeneric_Prototype() As AbstractProductA Implements ICloneMethod(Of AbstractProductA).Prototype
			Get
				Return buttonPrototype
			End Get
			Set(ByVal value As AbstractProductA)
				buttonPrototype = value
			End Set
		End Property

		Private Function CreateProductA() As AbstractProductA Implements IFactoryMethod(Of AbstractProductA).Create
			Return DirectCast(buttonPrototype.Clone(), AbstractProductA)
		End Function

		' Было без моего добавления ICloneable
		Private Function CreateProductB() As AbstractProductB Implements IFactoryMethod(Of AbstractProductB).Create
			Return New ProductB2()
		End Function
	End Class
End Namespace

