Namespace Factory
    Public Class ProductA1
        Inherits AbstractProductA

        Public Overrides Function Clone() As Object
            Return New ProductA1()
        End Function
    End Class

    Public Class ProductA2
        Inherits AbstractProductA

        Public Overrides Function Clone() As Object
            Return New ProductA2()
        End Function
    End Class

    ' Я добавил ICloneable и здесь реализация
    'public class StyledEdit : Edit
    '{
    '    public override object Clone()
    '    {
    '        return new StyledEdit();
    '    }
    '}

    ' Было
    Public Class ProductB1
        Inherits AbstractProductB

        Private mResult As String

        Public Overrides Sub Interact(ByVal a As AbstractProductA)
            mResult = Me.GetType().Name & " взаимодействует с " & a.GetType().Name
        End Sub

        Public Overrides Function Result() As String
            Return mResult
        End Function
    End Class

    Public Class ProductB2
        Inherits AbstractProductB

        Private mResult As String
        Public Overrides Sub Interact(ByVal a As AbstractProductA)
            mResult = Me.GetType().Name & " взаимодействует с " & a.GetType().Name
        End Sub

        Public Overrides Function Result() As String
            Return mResult
        End Function
    End Class
End Namespace
