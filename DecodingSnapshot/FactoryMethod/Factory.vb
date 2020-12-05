Imports System.Collections.Generic

Namespace FactoryMethod
    Public Class KeyedFactory
        Private factoryMethods As New Dictionary(Of String, FactoryMethod(Of TAnalysis))

        ''' <summary>
        ''' Из словаря достается делегат, и с его помощью создается экземпляр продукта.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        Public Function Create(ByVal key As String) As TAnalysis
            ' Объект делегата вызывается при помощи имени объекта делегата
            'FactoryMethod<TAnalysis> delegateSomeMethod = factoryMethods[key];
            'return delegateSomeMethod();

            ' просто вызов делегата
            Return factoryMethods(key).Invoke()
            ' Activator.CreateInstance(Type)
        End Function

        ''' <summary>
        '''  Регистрируются фабричный метод через делегат.
        ''' </summary>
        ''' <param name="keyValue"></param>
        ''' <param name="someMethod"></param>
        Public Sub RegisterMethod(ByVal keyValue As String, ByVal someMethod As FactoryMethod(Of TAnalysis))
            factoryMethods.Add(keyValue, someMethod)
        End Sub

        Public Sub RegisterType(Of TConcreteAnalysis As {TAnalysis, New})(ByVal key As String)
            RegisterMethod(key, Function()
                                    Return New TConcreteAnalysis()
                                End Function)
        End Sub

        Public Sub RegisterPrototype(Of TConcreteAnalysis As {TAnalysis, ICloneable})(ByVal key As String, ByVal prototype As TConcreteAnalysis)
            RegisterMethod(key, Function()
                                    Return DirectCast(prototype.Clone(), TAnalysis)
                                End Function)
        End Sub

        Friend Function ContainsKey(nameAnalysis As String) As Boolean
            Return factoryMethods.ContainsKey(nameAnalysis)
        End Function
    End Class
End Namespace