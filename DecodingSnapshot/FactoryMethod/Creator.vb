Namespace FactoryMethod
	Public MustInherit Class Creator
		Public MustOverride Function FactoryMethod() As TAnalysis
	End Class

	Public Class ConcreteCreatorA
		Inherits Creator

		Public Overrides Function FactoryMethod() As TAnalysis
			Return New ConcreteAnalysisA()
		End Function
	End Class

	Public Class ConcreteCreatorB
		Inherits Creator

		Public Overrides Function FactoryMethod() As TAnalysis
			Return New ConcreteAnalysisB()
		End Function
	End Class

	Friend Class ConcreteCreatorC
		Inherits Creator

		Public Overrides Function FactoryMethod() As TAnalysis
			Return New ConcreteAnalysisB()
		End Function
	End Class
End Namespace