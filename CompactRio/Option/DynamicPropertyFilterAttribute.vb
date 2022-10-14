Imports System.ComponentModel

''' <summary>
''' Атрибут для поддержки динамически показываемых свойств (список).
''' </summary>
<AttributeUsage(AttributeTargets.[Property], Inherited:=True)>
Friend Class DynamicPropertyFilterAttribute
    Inherits Attribute

    Private ReadOnly mPropertyName As String

    ''' <summary>
    ''' Название свойства, от которого будет зависить видимость  
    ''' </summary>
    Public ReadOnly Property PropertyName() As String
        Get
            Return mPropertyName
        End Get
    End Property

    Private ReadOnly mShowOn As String

    ''' <summary>
    ''' Значения свойства от которого зависит видимость 
    ''' (через запятую, если несколько), при котором свойство, к
    ''' которому применен атрибут, будет видимо. 
    ''' </summary>
    Public ReadOnly Property ShowOn() As String
        Get
            Return mShowOn
        End Get
    End Property

    ''' <summary>
    ''' Конструктор  
    ''' </summary>
    ''' <param name="propName">Название свойства, от которого будет зависить видимость</param>
    ''' <param name="value">Значения свойства, через запятую, если несколько, при котором свойство, к
    ''' которому применен атрибут, будет видимо.</param>
    Public Sub New(ByVal propName As String, ByVal value As String)
        mPropertyName = propName
        mShowOn = value
    End Sub
End Class

''' <summary>
''' Базовый класс для объектов, поддерживающих динамическое 
''' отображение свойств в PropertyGrid.
''' Реализует интерфейс, с помощью которого предоставляются динамические сведения о пользовательских типах объектов.
''' </summary>
<Serializable()>
Public Class FilterablePropertyBase
    Implements ICustomTypeDescriptor

    ''' <summary>
    ''' Возвращает свойства для этого экземпляра компонента, используя массив атрибутов в качестве фильтра.
    ''' Используется для фильтрации свойств по аттрибуту ТипРаботыBooleanTypeConverter
    ''' </summary>
    ''' <param name="attributes">Массив типа System.Attribute, используемый в качестве фильтра.</param>
    ''' <returns>Коллекция System.ComponentModel.PropertyDescriptorCollection, представляющая фильтрованные свойства для этого экземпляра компонента.</returns>
    ''' <remarks></remarks>
    Protected Function GetFilteredProperties(ByVal attributes As Attribute()) As PropertyDescriptorCollection
        Dim pdc As PropertyDescriptorCollection = TypeDescriptor.GetProperties(Me, attributes, True) '  коллекция свойств для указанного компонента
        Dim finalProps As New PropertyDescriptorCollection(New PropertyDescriptor(-1) {}) ' краткое описание свойства класса.

        ' пройти по всем свойствам в поиске содержащих аттрибут типа DynamicPropertyFilterAttribute
        For Each pd As PropertyDescriptor In pdc
            Dim include As Boolean = False
            Dim propertyIsDynamic As Boolean = False

            For Each a As Attribute In pd.Attributes
                If TypeOf a Is DynamicPropertyFilterAttribute Then
                    propertyIsDynamic = True

                    Dim dpf As DynamicPropertyFilterAttribute = DirectCast(a, DynamicPropertyFilterAttribute)
                    Dim temp As PropertyDescriptor = pdc(dpf.PropertyName) ' найти свойство, от которого зависит видимость  

                    ' если в перечислении есть значение равное значению свойства, от которого зависит видимость, то:
                    If dpf.ShowOn.IndexOf(temp.GetValue(Me).ToString()) > -1 Then
                        include = True
                    End If
                End If
            Next

            ' нединамические выдать как есть, а динамические выдать только с включённой видимостью
            If Not propertyIsDynamic OrElse include Then
                finalProps.Add(pd)
            End If
        Next

        Return finalProps
    End Function

#Region "ICustomTypeDescriptor Members"
    Public Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
        Return TypeDescriptor.GetConverter(Me, True)
    End Function

    Public Function GetEvents(ByVal attributes As Attribute()) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, attributes, True)
    End Function

    Private Function GetEvents() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, True)
    End Function

    Public Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
        Return TypeDescriptor.GetComponentName(Me, True)
    End Function

    Public Function GetPropertyOwner(ByVal pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
        Return Me
    End Function

    Public Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
        Return TypeDescriptor.GetAttributes(Me, True)
    End Function

    Public Function GetProperties(ByVal attributes As Attribute()) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Return GetFilteredProperties(attributes)
    End Function

    Private Function GetProperties() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Return GetFilteredProperties(New Attribute(-1) {})
    End Function

    Public Function GetEditor(ByVal editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
        Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
    End Function

    Public Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
        Return TypeDescriptor.GetDefaultProperty(Me, True)
    End Function

    Public Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
        Return TypeDescriptor.GetDefaultEvent(Me, True)
    End Function

    Public Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
        Return TypeDescriptor.GetClassName(Me, True)
    End Function
#End Region
End Class