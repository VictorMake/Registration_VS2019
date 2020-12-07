Imports System

Namespace Factory
	''' <summary>
	''' Фабрика должна реализовать методы создания и подтверждения
	''' </summary>
	Public Interface ICloneFactory
		Inherits IAbstractFactory

		Sub SetPrototype(Of T As ICloneable)(ByVal prototype As T)

		Function GetPrototype(Of T As ICloneable)() As T
	End Interface
End Namespace
