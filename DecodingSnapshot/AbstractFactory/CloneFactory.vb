Namespace Factory
    ''' <summary>
    ''' Частный случаем является фабрика, основанная на использовании прототипов, 
    ''' поэтому класс CloneFactory одновременно наследуется от AbstractFactory и реализует интерфейс ICloneFactory.
    ''' Базовый класс для наследования.
    ''' Реализует методы создания и потверждения интерфейса.
    ''' Конкретный экземпляр фабрики будет так же реализовывать другие интерфейсы - IFactoryMethod.
    ''' Конкретный экземпляр фабрики приводится к IFactoryMethod, и вызывается метод 
    ''' клонирования объекта-прототипа, которые содержит в себе.
    ''' </summary>
    Public MustInherit Class CloneFactory
        Inherits AbstractFactory
        Implements ICloneFactory

        Public Sub SetPrototype(Of T As ICloneable)(ByVal prototype As T) Implements ICloneFactory.SetPrototype
            Dim cloneMethod As ICloneMethod(Of T) = TryCast(Me, ICloneMethod(Of T))
            If cloneMethod IsNot Nothing Then
                cloneMethod.Prototype = prototype
            End If

            ' throw new ArgumentException();
        End Sub

        Public Function GetPrototype(Of T As ICloneable)() As T Implements ICloneFactory.GetPrototype
            Dim cloneMethod As ICloneMethod(Of T) = TryCast(Me, ICloneMethod(Of T))
            If cloneMethod IsNot Nothing Then
                Return cloneMethod.Prototype
            End If

            ' throw new ArgumentException();
            Return CType(Nothing, T)
        End Function
    End Class
End Namespace
