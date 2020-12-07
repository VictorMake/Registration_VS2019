Namespace Factory
	''' <summary>
	''' Класс клиента, в котором происходит взаимодействие между объектами
	''' </summary>
	Friend Class Client
		Private ReadOnly mAbstractProductA As AbstractProductA
		Private ReadOnly mAbstractProductB As AbstractProductB

		' Конструктор
		Public Sub New(ByVal factory As IAbstractFactory)
			mAbstractProductA = factory.Create(Of AbstractProductA)()
			mAbstractProductB = factory.Create(Of AbstractProductB)()
		End Sub

		Public Sub Run()
			mAbstractProductB.Interact(mAbstractProductA)
		End Sub

		Public Function Result() As String
			Return mAbstractProductB.Result
		End Function
	End Class
End Namespace

