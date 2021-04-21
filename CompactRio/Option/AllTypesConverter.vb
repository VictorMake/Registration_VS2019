Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports Registration.My

Module AllTypesConverter

#Region "Конверторы типа BooleanConverter"
#Region "ТипРаботыBooleanTypeConverter"
    ''' <summary>
    ''' TypeConverter для Property СКонтроллером() As Boolean
    ''' </summary>
    Class ТипРаботыBooleanTypeConverter
        Inherits BooleanConverter

        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
            Return IIf(CBool(value), "С контроллером", "Автономно")
        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            Return DirectCast(value, String) = "С контроллером"
        End Function
    End Class
#End Region

#Region "BooleanTypeConverter"
    ''' <summary>
    ''' TypeConverter для bool
    ''' </summary>
    Class BooleanTypeConverter
        Inherits BooleanConverter

        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
            Return IIf(CBool(value), "Есть", "Нет")
        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            Return DirectCast(value, String) = "Есть"
        End Function
    End Class

    ''' <summary>
    ''' TypeConverter для bool
    ''' </summary>
    Class BooleanTypeConverter2
        Inherits BooleanConverter
        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
            Return IIf(CBool(value), "Да", "Нет")
        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            Return DirectCast(value, String) = "Да"
        End Function
    End Class
#End Region
#End Region

#Region "Конверторы типа StringConverter"
#Region "ЧастотаСбораTypeConverter"
    ''' <summary>
    ''' TypeConverter для свойств списка:
    ''' ЧастотаСбора() As String
    ''' СтепеньПередискретизации() As String
    ''' ФактическаяЧастотаОпросаКаналаГц() As String
    ''' </summary>
    Class ЧастотаСбораTypeConverter
        Inherits StringConverter

        ''' <summary>
        ''' Будем предоставлять выбор из списка
        ''' </summary>
        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' ... и только из списка
        ''' </summary>
        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
            ' false - можно вводить вручную
            ' true - только выбор из списка
            Return True
        End Function

        ''' <summary>
        ''' А вот и список
        ''' </summary>
        Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
            ' возвращаем список строк из настроек программы
            ' (базы данных, интернет и т.д.)
            Dim frequencies As Integer() = {1, 2, 5, 10, 20, 50, 100}

            Return New StandardValuesCollection(frequencies)
        End Function
    End Class
#End Region

#Region "StendServerTypeConverter"
    ''' <summary>
    ''' TypeConverter для списка Property StendServer() As String
    ''' </summary>
    Class StendServerTypeConverter
        Inherits StringConverter

        ''' <summary>
        ''' Будем предоставлять выбор из списка
        ''' </summary>
        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' ... и только из списка
        ''' </summary>
        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
            ' false - можно вводить вручную
            ' true - только выбор из списка
            Return True
        End Function

        ''' <summary>
        ''' А вот и список
        ''' </summary>
        Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
            ' возвращаем список строк из настроек программы
            Return New StandardValuesCollection(MySettings.[Default].NamberSSDList)
        End Function
    End Class
#End Region
#End Region

#Region "EnumTypeConverter"
    ''' <summary>
    ''' TypeConverter для Enum, преобразовывающий Enum к строке с
    ''' учетом атрибута Description
    ''' </summary>
    Class EnumTypeConverter
        Inherits EnumConverter

        Private ReadOnly _EnumType As Type
        ''' <summary>
        ''' Инициализирует экземпляр
        ''' </summary>
        ''' <param name="type">тип Enum</param>
        Public Sub New(ByVal type As Type)
            MyBase.New(type)
            _EnumType = type
        End Sub

        Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destType As Type) As Boolean
            Return destType Is GetType(String)
        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
            Dim fi As FieldInfo = _EnumType.GetField([Enum].GetName(_EnumType, value)) ' Поиск открытого поля с заданным именем.
            ' Извлекает настраиваемый атрибут, примененный к члену типа. Параметры определяют член и тип настраиваемого атрибута для поиска.
            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

            If dna IsNot Nothing Then
                Return dna.Description ' Вернуть описание, хранящееся в данном атрибуте.
            Else
                Return value.ToString() ' Вернуть строку, представляющую текущий объект.
            End If
        End Function

        Public Overloads Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
            'Значение true, если данный преобразователь может выполнить преобразование; в противном случае — значение false.
            Return sourceType Is GetType(String)
        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            For Each fi As FieldInfo In _EnumType.GetFields() ' Поиск открытого поля с заданным именем.
                ' Извлекает настраиваемый атрибут, примененный к члену типа. Параметры определяют член и тип настраиваемого атрибута для поиска.
                Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)
                ' если описание присутствует и его описание совпадает, то
                If (dna IsNot Nothing) AndAlso (DirectCast(value, String) = dna.Description) Then
                    Return [Enum].Parse(_EnumType, fi.Name) ' Преобразовать строковое представление имени или числового значения одной или нескольких перечислимых констант в эквивалентный перечислимый объект.
                End If
            Next

            ' иначе преобразовать строковое представление преобразуемого объекта в эквивалентный перечислимый объект.
            Return [Enum].Parse(_EnumType, DirectCast(value, String))
        End Function
    End Class
#End Region

#Region "CollectionTypeConverter"
    ''' <summary>
    ''' TypeConverter для редактируемых коллекций
    ''' </summary>
    Class CollectionTypeConverter
        Inherits TypeConverter

        ''' <summary>
        ''' Только в строку
        ''' </summary>
        Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destType As Type) As Boolean
            Return destType Is GetType(String)
        End Function

        ''' <summary>
        ''' И только так
        ''' </summary>
        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
            Return "< Список... >"
        End Function
    End Class
#End Region
End Module