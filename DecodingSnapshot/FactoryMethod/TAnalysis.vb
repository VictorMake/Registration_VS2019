Namespace FactoryMethod
	Public MustInherit Class TAnalysis
		Public MustOverride Function GetTypeToString() As String
	End Class

	Public Class ConcreteAnalysisA
		Inherits TAnalysis

		Public Overrides Function GetTypeToString() As String
			Return "ConcreteAnalysisA"
		End Function
	End Class

	Public Class ConcreteAnalysisB
		Inherits TAnalysis

		Public Overrides Function GetTypeToString() As String
			Return "ConcreteAnalysisB"
		End Function
	End Class

	Public Class ConcreteAnalysisC
		Inherits TAnalysis
		Implements ICloneable

		Public Function Clone() As Object Implements ICloneable.Clone
			Return Me
		End Function

		Public Overrides Function GetTypeToString() As String
			Return "ConcreteAnalysisC"
		End Function
	End Class
End Namespace

