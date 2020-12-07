Namespace Factory
    ''' <summary>
    ''' Требует регистрации
    ''' </summary>
    Public MustInherit Class AbstractProductA
        Implements ICloneable

        Public MustOverride Function Clone() As Object Implements ICloneable.Clone
    End Class

    ' Я добавил ICloneable
    'public abstract class Edit : ICloneable
    '{
    '    public abstract object Clone();
    '}

    ' Было
    Public MustInherit Class AbstractProductB
        Public MustOverride Sub Interact(ByVal a As AbstractProductA)
        Public MustOverride Function Result() As String
    End Class
End Namespace
