Imports System.ComponentModel ' для сереализации

''' <summary>
''' Содержит классы необходимые для OptionData
''' </summary>
''' <remarks></remarks>
Module AllProperties

#Region "PropertyOrder Attribute"
    ''' <summary>
    ''' Пользовательский Аттрибут для задания сортировки свойств по пунктам
    ''' </summary>
    <AttributeUsage(AttributeTargets.[Property])>
    Public Class PropertyOrderAttribute
        Inherits Attribute

        Private ReadOnly mOrder As Integer
        Public Sub New(ByVal order As Integer)
            mOrder = order
        End Sub

        Public ReadOnly Property Order() As Integer
            Get
                Return mOrder
            End Get
        End Property
    End Class
#End Region

#Region "PropertySorter"
    ''' <summary>
    ''' Сортировка по аттрибуту PropertyOrder для OptionData
    ''' Наследует преобразователь типа для преобразования расширяемых объектов в прочие представления и обратно.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PropertySorter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function GetPropertiesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Возвращает упорядоченный список свойств
        ''' </summary>
        Public Overloads Overrides Function GetProperties(ByVal context As ITypeDescriptorContext, ByVal value As Object, ByVal attributes As Attribute()) As PropertyDescriptorCollection
            Dim pdc As PropertyDescriptorCollection = TypeDescriptor.GetProperties(value, attributes)
            Dim orderedProperties As New ArrayList() ' отфильтрованные свойства

            ' пройти по всем свойствам в поисках аттрибута PropertyOrder
            For Each pd As PropertyDescriptor In pdc
                Dim attribute As Attribute = pd.Attributes(GetType(PropertyOrderAttribute))

                If attribute IsNot Nothing Then
                    ' атрибут есть - используем номер п/п из него
                    Dim poa As PropertyOrderAttribute = DirectCast(attribute, PropertyOrderAttribute)
                    orderedProperties.Add(New PropertyOrderPair(pd.Name, poa.Order)) ' добавить новый элемент наследующий IComparable
                Else
                    ' атрибута нет - считаем что 0
                    orderedProperties.Add(New PropertyOrderPair(pd.Name, 0))
                End If
            Next

            ' сортируем по Order-у
            orderedProperties.Sort()

            ' формируем список имен свойств по которому в итоге будет производится сортировка
            Dim propertyNames As New ArrayList()
            For Each pop As PropertyOrderPair In orderedProperties
                propertyNames.Add(pop.Name)
            Next

            ' возвращаем
            ' Новая коллекция System.ComponentModel.PropertyDescriptorCollection, содержащая отсортированные объекты System.ComponentModel.PropertyDescriptor.
            Return pdc.Sort(DirectCast(propertyNames.ToArray(GetType(String)), String()))
        End Function
    End Class

#Region "PropertyOrderPair"
    ''' <summary>
    ''' Пара имя/номер п/п с сортировкой по номеру
    ''' </summary>
    Public Class PropertyOrderPair
        Implements IComparable

        Private ReadOnly mOrder As Integer
        Private ReadOnly mName As String
        Public ReadOnly Property Name() As String
            Get
                Return mName
            End Get
        End Property

        Public Sub New(name As String, order As Integer)
            mOrder = order
            mName = name
        End Sub

        ''' <summary>
        ''' Собственно метод сравнения
        ''' </summary>
        Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherOrder As Integer = DirectCast(obj, PropertyOrderPair).mOrder

            If otherOrder = mOrder Then
                ' если Order одинаковый - сортируем по именам
                Dim otherName As String = DirectCast(obj, PropertyOrderPair).mName
                Return String.Compare(mName, otherName)
            ElseIf otherOrder > mOrder Then
                Return -1
            End If

            Return 1
        End Function
    End Class
#End Region
#End Region
End Module