Imports System.ComponentModel
Imports System.Globalization
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel.Design
Imports Registration.My
Imports System.Reflection ' для сереализации

#Region "TypeConverter"
''' <summary>
''' TypeConverter для списка операций сравнения
''' </summary>
Public Class OperationTypeConverter
    Inherits StringConverter
    ''' <summary>
    ''' предоставляет выбор из списка
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
    ''' список
    ''' </summary>
    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        ' возвращаем список строк операций
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        '        '="=" Or ="<>" Or ="<" Or =">" Or ="между"  Or ="вне"
        Dim operation() As String = {"=", "<>", "<", ">", "между", "вне"}
        Return New StandardValuesCollection(operation)
    End Function
End Class

''' <summary>
''' TypeConverter для списка операций сравнения
''' </summary>
Public Class OperationTypeConverterFormula
    Inherits StringConverter
    ''' <summary>
    ''' TypeConverter для списка операций сравнения
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
    ''' список
    ''' </summary>
    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        ' возвращаем список строк операций
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        '        '="=" Or ="<>" Or ="<" Or =">" Or ="между"  Or ="вне"

        Dim operation() As String = {">", "<"}
        Return New StandardValuesCollection(operation)
    End Function
End Class

''' <summary>
''' TypeConverter для списка стендов
''' </summary>
Public Class NumberDeviceTypeConverter
    Inherits Int32Converter
    'Inherits StringConverter

    ''' <summary>
    ''' выбор из списка
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
    ''' список
    ''' </summary>
    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        ' возвращаем список строк операций
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        Dim arrStend() As Integer = {1, 2, 3, 4, 5} ' должен определяться динамически в зависимости от опроса железа
        'Dim arrStend() As Integer = {11, 13, 15, 17, 19, 21, 22, 25, 28, 34, 37, 41}
        Return New StandardValuesCollection(arrStend)
    End Function
End Class

''' <summary>
''' TypeConverter для списка модулей
''' </summary>
Public Class NumberModuleTypeConverter
    Inherits StringConverter
    ''' <summary>
    ''' выбор из списка
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
        ' возвращаем список модулей
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        Dim modules() As String = {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"} ' должен определяться динамически в зависимости от опроса железа
        Return New StandardValuesCollection(modules)
    End Function
End Class

''' <summary>
''' TypeConverter для списка портов
''' </summary>
Public Class NumberPortTypeConverter
    Inherits Int32Converter
    'Inherits StringConverter

    ''' <summary>
    ''' выбор из списка
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
    ''' список
    ''' </summary>
    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        ' возвращаем список строк портов
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        Dim ports() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11} ' должен определяться динамически в зависимости от опроса железа
        Return New StandardValuesCollection(ports)
    End Function
End Class

''' <summary>
''' TypeConverter для списка Bit
''' </summary>
Public Class NumberBitTypeConverter
    Inherits Int32Converter
    'Inherits StringConverter

    ''' <summary>
    ''' выбор из списка
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
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        ' должен определяться динамически в зависимости от опроса железа
        Dim bits() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31} 'должен определяться динамически в зависимости от опроса железа
        Return New StandardValuesCollection(bits)
    End Function
End Class

#Region "Конверторы типа BooleanConverter"
#Region "ТипРаботыBooleanTypeConverter"
''' <summary>
''' TypeConverter для Property СКонтроллером() As Boolean
''' </summary>
Public Class ТипРаботыBooleanTypeConverter
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
Public Class BooleanTypeConverter
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
Public Class BooleanTypeConverter2
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

''' <summary>
''' TypeConverter для редактируемых коллекций
''' </summary>
Public Class CollectionTypeConverter
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

''' <summary>
''' TypeConverter для списка 
''' </summary>
Public Class OperationTypeConverterFrequencyOrAmplitude
    Inherits StringConverter
    ''' <summary>
    ''' предоставить выбор из списка
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
        'Return New StandardValuesCollection(MySettings.[Default].PostList)

        Dim FrequencyOrAmplitude() As String = {"Частота", "Амплитуда"}
        Return New StandardValuesCollection(FrequencyOrAmplitude)

    End Function
End Class

''' <summary>
''' TypeConverter для списка 
''' </summary>
Public Class OperationTypeConverterСоответствиеДиапазону
    Inherits StringConverter
    ''' <summary>
    ''' выбор из списка
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
        'Return New StandardValuesCollection(MySettings.[Default].PostList)

        Dim Диапазон() As String = {"10.00", "5.00", "2.00", "1.00"}
        Return New StandardValuesCollection(Диапазон)
    End Function
End Class

#Region "EnumTypeConverter"
''' <summary>
''' TypeConverter для Enum, преобразовывающий Enum к строке с
''' учетом атрибута Description
''' </summary>
Public Class EnumTypeConverter
    Inherits EnumConverter

    Private _EnumType As Type
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

#Region "Конверторы типа StringConverter"
''' <summary>
''' TypeConverter для списка частот сбора
''' </summary>
Public Class ЧастотаСбораTypeConverter
    Inherits StringConverter
    ''' <summary>
    ''' выбор из списка
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
    ''' список
    ''' </summary>
    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        ' возвращаем список строк 
        'Return New StandardValuesCollection(MySettings.[Default].PostList)
        Dim frequencies() As Integer = {100} '{1, 2, 5, 10, 20, 50, 100}

        Return New StandardValuesCollection(frequencies)
    End Function
End Class

#Region "StendServerTypeConverter"
''' <summary>
''' TypeConverter для списка Property StendServer() As String
''' </summary>
Public Class StendServerTypeConverter
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

''' <summary>
''' TypeConverter для bool
''' </summary>
Class ТипГрафикаBooleanTypeConverter
    Inherits BooleanConverter

    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
        Return IIf(CBool(value), "Горизонтально", "Вертикально")
    End Function

    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
        Return DirectCast(value, String) = "Горизонтально"
    End Function
End Class

''' <summary>
''' TypeConverter для bool
''' </summary>
Class ПрокруткаBooleanTypeConverter
    Inherits BooleanConverter

    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
        Return IIf(CBool(value), "Рулон", "Осциллограф")
    End Function

    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
        Return DirectCast(value, String) = "Рулон"
    End Function
End Class

#End Region

#Region "TypeEditor"
''' <summary>
''' ChannelAndFormulaEditor
''' </summary>
Public Class ChannelAndFormulaEditor
    Inherits UITypeEditor

    ''' <summary>
    ''' Реализация метода редактирования
    ''' </summary>
    Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        If (context IsNot Nothing) AndAlso (provider IsNot Nothing) Then
            Dim svc As IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

            If svc IsNot Nothing Then
                'Using chfrm As New FormChannelEditor((DirectCast(value, String)))
                '    If svc.ShowDialog(chfrm) = DialogResult.OK Then
                '        value = chfrm.Channel
                '    End If
                'End Using

                Dim chfrm As New FormChannelEditor((DirectCast(value, String)))
                If svc.ShowDialog(chfrm) = DialogResult.OK Then
                    value = chfrm.Channel
                End If
            End If
        End If

        Return MyBase.EditValue(context, provider, value)
    End Function

    ''' <summary>
    ''' Возвращаем стиль редактора - модальное окно
    ''' </summary>
    Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        If context IsNot Nothing Then
            Return UITypeEditorEditStyle.Modal
        Else
            Return MyBase.GetEditStyle(context)
        End If
    End Function
End Class

''' <summary>
''' Модальный редактор FormulaEditor
''' </summary>
Public Class FormulaEditor
    Inherits UITypeEditor

    ''' <summary>
    ''' Реализация метода редактирования
    ''' </summary>
    Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        If (context IsNot Nothing) AndAlso (provider IsNot Nothing) Then
            Dim svc As IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

            If svc IsNot Nothing Then
                Using chfrm As New FormFormulaEditor((DirectCast(value, String)))
                    If svc.ShowDialog(chfrm) = DialogResult.OK Then value = chfrm.Formula
                End Using
            End If
        End If

        Return MyBase.EditValue(context, provider, value)
    End Function

    ''' <summary>
    ''' Возвращаем стиль редактора - модальное окно
    ''' </summary>
    Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        If context IsNot Nothing Then
            Return UITypeEditorEditStyle.Modal
        Else
            Return MyBase.GetEditStyle(context)
        End If
    End Function

End Class

'Delegate Sub ADelegateConvertedAnonymousMethod3(ByVal sndr As Object, ByVal ea As EventArgs)

' реализация CollectionEditor, ***CollectionEditor, умеет запоминать положение и размеры своего окна, 
' меняет стандартные подписи на соответствующие редактируемым данным и добавляет окно с расширенной подсказкой по свойствам:

''' <summary>
''' Свой CollectionEditor для редактирования списков  -
''' для задания заголовка и запоминания положения окна
''' </summary>
Class ListCollectionEditor
    Inherits CollectionEditor

    Dim collform As CollectionForm ' = MyBase.CreateCollectionForm()
    Dim labelDescription As New Label()
    Dim isAddRemove As Boolean
    Dim isControlChanged As Boolean
    'Private Shared aTimer As System.Timers.Timer

    ''' <summary>
    ''' Конструктор
    ''' </summary>
    Public Sub New(ByVal t As Type)
        MyBase.New(t)
        Trace.WriteLine("ListCollectionEditor() ctor")
        'aTimer = New System.Timers.Timer(10)
        'AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
        ''aTimer.Interval = 2000
        'aTimer.AutoReset = False
    End Sub

    ''' <summary>
    ''' Перекрытый метод создания формы редактора - для сохранения/восстановления
    ''' размеров и положения окна
    ''' </summary>
    Protected Overloads Overrides Function CreateCollectionForm() As CollectionForm
        'Dim collform As CollectionForm = MyBase.CreateCollectionForm()
        collform = MyBase.CreateCollectionForm()

        ' подключаем свой обработчик открытия и в нем 
        ' восстанавливаем положение и размер окна
        AddHandler collform.Load, AddressOf ConvertedAnonymousMethodLoad
        ' подключаем свой обработчик закрытия и в нем сохраняем положение и размер окна
        AddHandler collform.FormClosing, AddressOf ConvertedAnonymousMethodClosing

        Return collform
    End Function

    Private Sub ConvertedAnonymousMethodLoad(ByVal sender As Object, ByVal e As EventArgs)
        Trace.WriteLine("collform.Load()")
        collform.HelpButton = False
        ' пока это положение первый раз не сохранено, при 
        ' попытке чтения будет кирдык
        '--- сохранение/восстановление размеров и положения окна--------------
        Try
            collform.Size = MySettings.[Default].GridCollectFormSize
            Dim mPoint As Point = MySettings.[Default].GridCollectFormLocation
            mPoint.X += TypeNode * 10
            mPoint.Y += TypeNode * 10
            collform.Location = mPoint
            collform.WindowState = MySettings.[Default].GridCollectFormState
        Catch
            Trace.WriteLine("CollectionEditor::CreateCollectionForm() exception")
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<CollectionEditor::CreateCollectionForm() exception> {New StackTrace(0)}")
        End Try

        collform.Text = "Установить свойства для уровня " & GetPropertiesText()

        '--- Русификация и кастомизация надписей на форме --------------------
        ' перебираем все контролы на форме и заменяем
        ' неправильные надписи
        For Each ctrl As Control In collform.Controls
            'Trace.WriteLine("1  " & ctrl.[GetType]().ToString())
            'Trace.WriteLine("1  " & ctrl.Name.ToString())

            For Each ctrl1 As Control In ctrl.Controls
                If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Label" AndAlso (ctrl1.Text = "&Members:" OrElse ctrl1.Text = "&Члены:") Then
                    'ctrl1.Text = MembersText
                    Select Case TypeNode
                        Case TypeGridDigitalNode.AllConfigurations
                            ctrl1.Text = "&Конфигурации:"

                        Case TypeGridDigitalNode.Configuration
                            ctrl1.Text = "&Все Конфигурации:"
                        Case TypeGridDigitalNode.AllActions
                            ctrl1.Text = "&Конфигурации:"
                        Case TypeGridDigitalNode.Action
                            ctrl1.Text = "&Все Действия:"

                        Case TypeGridDigitalNode.AllTriggers
                            ctrl1.Text = "&Действия:"
                        Case TypeGridDigitalNode.Trigger
                            ctrl1.Text = "&Все определённые Триггеры:"

                        Case TypeGridDigitalNode.AllFormulas
                            ctrl1.Text = "&Действия:"
                        Case TypeGridDigitalNode.Formula
                            ctrl1.Text = "&Все введенные Формулы:"
                        Case TypeGridDigitalNode.AllArguments
                            ctrl1.Text = "&Формулы:"
                        Case TypeGridDigitalNode.Argument
                            ctrl1.Text = "&Установленные Аргументы:"

                        Case TypeGridDigitalNode.AllPorts
                            ctrl1.Text = "&Действия:"
                        Case TypeGridDigitalNode.Port
                            ctrl1.Text = "&Все используемые Порты:"
                        Case TypeGridDigitalNode.AllEnableBits
                            ctrl1.Text = "&Порты:"
                        Case TypeGridDigitalNode.Bit
                            ctrl1.Text = "&Установленные Биты:"

                        Case Else
                            ctrl1.Text = "&Неизвестен:"
                    End Select
                End If

                If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Label" AndAlso (ctrl1.Text.Contains("&properties") OrElse ctrl1.Text.Contains("&Cвойства")) Then
                    labelDescription = DirectCast(ctrl1, Label)
                    labelDescription.Text = GetPropertiesText()
                End If
                If ctrl1.[GetType]().ToString() = "System.ComponentModel.Design.CollectionEditor+FilterListBox" Then
                    ' это самый правильный обработчик, но после него
                    ' срабатывает обработчик формы и меняет надпись на свою
                    '((ListBox)ctrl1).SelectedValueChanged +=
                    '   delegate(object sndr, EventArgs ea)
                    '   {
                    '      Trace.WriteLine("SelectedValueChanged()");
                    '      BlaBlaProperties.Text = PropertiesText ;
                    '   };

                    ' вместо одного правильного - два обходных - 
                    ' на смену индекса в листвоксе

                    AddHandler DirectCast(ctrl1, ListBox).SelectedIndexChanged, AddressOf ConvertedAnonymousMethodSelectedIndexChanged
                    'Dim anonymousMethod3 As ADelegateConvertedAnonymousMethod3 = Function(sndr As Object, ea As EventArgs) BlaBlaProperties.Text = PropertiesText
                    'AddHandler DirectCast(ctrl1, ListBox).SelectedIndexChanged, AddressOf anonymousMethod3
                    ' событие на переход к другой строке
                    'AddHandler DirectCast(ctrl1, ListBox).SelectedValueChanged, AddressOf SelectedValueChanged
                    'AddHandler DirectCast(ctrl1, ListBox).ControlAdded, AddressOf ControlAddedRemoved
                    'AddHandler DirectCast(ctrl1, ListBox).ControlRemoved, AddressOf ControlAddedRemoved
                End If

                If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Design.VsPropertyGrid" Then
                    ' и на редактирование в PropertyGrid
                    AddHandler DirectCast(ctrl1, PropertyGrid).SelectedGridItemChanged, AddressOf ConvertedAnonymousMethodSelectedGridItemChanged
                    ' также сделать доступным окно с подсказками по параметрам в нижней части 
                    DirectCast(ctrl1, PropertyGrid).HelpVisible = True
                    DirectCast(ctrl1, PropertyGrid).HelpBackColor = SystemColors.Info
                    ' если какое-то свойство изменяется то обновить дерево
                    AddHandler DirectCast(ctrl1, PropertyGrid).PropertyValueChanged, AddressOf ControlValueChanged
                End If

                'Trace.WriteLine("2           " & ctrl1.[GetType]().ToString())
                'Trace.WriteLine("2           " & ctrl1.Name.ToString())

                If ctrl1.Name = "addRemoveTableLayoutPanel" Then

                    For Each ctrl2 As Control In ctrl1.Controls
                        'Trace.WriteLine("2           " & ctrl2.[GetType]().ToString())
                        'Trace.WriteLine("2           " & ctrl2.Name.ToString())
                        '2:                      System.ComponentModel.Design.CollectionEditor(+SplitButton)
                        '2:                      addButton()
                        '2:                      System.Windows.Forms.Button()
                        '2:                      removeButton()
                        'If ctrl2.[GetType]().ToString() = "System.Windows.Forms.Button" Then ' AndAlso (ctrl1.Text = "&Add" OrElse ctrl1.Text = "&Cancel") Then
                        If ctrl2.Name = "addButton" OrElse ctrl2.Name = "removeButton" Then
                            AddHandler DirectCast(ctrl2, Button).Click, AddressOf AddRemove
                        End If
                    Next
                End If

                If ctrl1.Name = "okCancelTableLayoutPanel" Then
                    For Each ctrl2 As Control In ctrl1.Controls
                        If ctrl2.Name = "okButton" OrElse ctrl2.Name = "cancelButton" Then
                            AddHandler DirectCast(ctrl2, Button).Click, AddressOf OkCancel
                        End If
                    Next
                End If
            Next
        Next

        'Dim obj As Object = MemberwiseClone()
        'Me.CollectionItemType.ToString()

        'Dim mList As System.Collections.ArrayList = Me.GetItems(Me)
        'Dim mList() As Object = Me.GetItems(Me)
    End Sub

    Private Sub ConvertedAnonymousMethodClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        Trace.WriteLine("collform.FormClosing()")

        If TypeNode = TypeGridDigitalNode.AllConfigurations Then
            MySettings.[Default].GridCollectFormState = collform.WindowState

            If collform.WindowState = FormWindowState.Normal Then
                MySettings.[Default].GridCollectFormSize = collform.Size
                MySettings.[Default].GridCollectFormLocation = collform.Location
            Else
                MySettings.[Default].GridCollectFormSize = collform.RestoreBounds.Size
                MySettings.[Default].GridCollectFormLocation = collform.RestoreBounds.Location
            End If

            ' сохраним возможно измененные значения параметров
            MySettings.[Default].Save()
        End If
    End Sub

    Private Sub ConvertedAnonymousMethodSelectedIndexChanged(ByVal sndr As Object, ByVal ea As EventArgs)
        labelDescription.Text = GetPropertiesText()
    End Sub
    Private Sub ConvertedAnonymousMethodSelectedGridItemChanged(ByVal sndr As Object, ByVal segichd As SelectedGridItemChangedEventArgs)
        labelDescription.Text = GetPropertiesText()
    End Sub

    Private Sub ControlValueChanged(ByVal sndr As Object, ByVal segichd As EventArgs)
        isControlChanged = True
    End Sub
    Private Sub AddRemove(ByVal sndr As Object, ByVal segichd As EventArgs)
        isAddRemove = True
    End Sub

    Private Sub OkCancel(ByVal sndr As Object, ByVal segichd As EventArgs)
        If isAddRemove OrElse isControlChanged Then
            UpdateDirectoryTreeViewAdListView()
        End If
        isAddRemove = False
        isControlChanged = False
    End Sub

    Private Sub UpdateDirectoryTreeViewAdListView()
        DigitalPortForm.FlvFiles.Columns.Clear()
        DigitalPortForm.FlvFiles.Items.Clear()
        DigitalPortForm.DtvwDirectory.SelectCureentNode()
        DigitalPortForm.DtvwDirectory.RefreshTree()
        DigitalPortForm.FindLastNode(TypeNode)
    End Sub

    'Private Sub SelectedValueChanged(ByVal sndr As Object, ByVal segichd As EventArgs)
    '    aTimer.Enabled = True ' запуск таймера для сброса ИдетЗаписьПути_ = False
    'End Sub

    '' событие таймера
    ''Private Shared Sub OnTimedEvent(ByVal source As Object, ByVal e As ElapsedEventArgs)
    'Private Sub OnTimedEvent(ByVal source As Object, ByVal e As ElapsedEventArgs)
    '    'Console.WriteLine("событие таймера произошло в {0}", e.SignalTime)
    '    aTimer.Enabled = False

    '    frmDigitalOutputPort.flvFiles.Columns.Clear()
    '    frmDigitalOutputPort.flvFiles.Items.Clear()

    '    frmDigitalOutputPort.dtvwDirectory.RefreshTree()
    'End Sub

    Private Function GetPropertiesText() As String
        Select Case TypeNode
            Case TypeGridDigitalNode.AllConfigurations
                Return "Все конфигурации:"
            Case TypeGridDigitalNode.Configuration
                Return "Конфигурация:"
            Case TypeGridDigitalNode.AllActions
                Return "Все действия:"
            Case TypeGridDigitalNode.Action
                Return "Действие:"
            Case TypeGridDigitalNode.AllTriggers
                Return "Все триггеры:"
            Case TypeGridDigitalNode.Trigger
                Return "Триггер:"
            Case TypeGridDigitalNode.AllFormulas
                Return "Все формулы:"
            Case TypeGridDigitalNode.Formula
                Return "Формула:"
            Case TypeGridDigitalNode.AllArguments
                Return "Все аргументы:"
            Case TypeGridDigitalNode.Argument
                Return "Аргумент:"
            Case TypeGridDigitalNode.AllPorts
                Return "Все порты:"
            Case TypeGridDigitalNode.Port
                Return "Порт:"
            Case TypeGridDigitalNode.AllEnableBits
                Return "Все установленные биты:"
            Case TypeGridDigitalNode.Bit
                Return "Бит:"
            Case Else
                Return "&Неизвестен:"
        End Select
    End Function
End Class

'Delegate Function ADelegate(ByVal value As Integer) As Boolean
'Class AClass
'    Dim anonymousMethod1 As ADelegate = Function(value As Integer) value > 5
'    Shared anonymousMethod2 As ADelegate = Function(value As Integer) value > 5

'    Sub Method1()
'        anonymousMethod1(10)
'        anonymousMethod2(10)
'    End Sub
'End Class

'Namespace PropertyGridUtils
''' <summary>
''' Класс для установки фильтра по расширению при выборе базы каналов Channels.mdb 
''' </summary>
Class ChannelsFileEditor
    Inherits FileNameEditor

    ''' <summary>
    ''' Настройка фильтра расширений 
    ''' </summary>
    Protected Overloads Overrides Sub InitializeDialog(ByVal ofd As OpenFileDialog)
        ofd.CheckFileExists = False
        ofd.Filter = "База каналов Channels.mdb (*.mdb)|*.mdb"
    End Sub
End Class

''' <summary>
''' Добавляет картинки, соответствующие каждому члену перечисления
''' </summary>
Public Class ГенераторEditor
    Inherits UITypeEditor

    Public Overloads Overrides Function GetPaintValueSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Sub PaintValue(ByVal e As PaintValueEventArgs)
        ' картинки хранятся в ресурсах с именами, соответствующими
        ' именам каждого члена перечисления EnumDigitalNode
        Dim resourcename As String = DirectCast(e.Value, WaveformType).ToString()

        ' достаем картинку из ресурсов
        Dim sexImage As Bitmap = DirectCast(Resources.ResourceManager.GetObject(resourcename), Bitmap)
        Dim destRect As Rectangle = e.Bounds
        sexImage.MakeTransparent()

        ' и отрисовываем
        e.Graphics.DrawImage(sexImage, destRect)
    End Sub
End Class

#End Region

#Region "Property Order Sorter"

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
        Dim orderedProperties As New ArrayList()

        For Each pd As PropertyDescriptor In pdc
            Dim attribute As Attribute = pd.Attributes(GetType(PropertyOrderAttribute))

            If attribute IsNot Nothing Then
                ' атрибут есть - используем номер п/п из него
                Dim poa As PropertyOrderAttribute = DirectCast(attribute, PropertyOrderAttribute)
                orderedProperties.Add(New PropertyOrderPair(pd.Name, poa.Order))
            Else
                ' атрибута нет - считаем что 0
                orderedProperties.Add(New PropertyOrderPair(pd.Name, 0))
            End If
        Next

        ' сортируем по Order-у
        orderedProperties.Sort()

        ' формируем список имен свойств
        Dim propertyNames As New ArrayList()
        For Each pop As PropertyOrderPair In orderedProperties
            propertyNames.Add(pop.Name)
        Next

        ' возвращаем
        Return pdc.Sort(DirectCast(propertyNames.ToArray(GetType(String)), String()))
    End Function
End Class

''' <summary>
''' Атрибут для задания сортировки
''' </summary>
<AttributeUsage(AttributeTargets.[Property])> _
Public Class PropertyOrderAttribute
    Inherits Attribute

    Public Sub New(ByVal order As Integer)
        Me.Order = order
    End Sub

    Public ReadOnly Property Order() As Integer
End Class

''' <summary>
''' Пара имя/номер п/п с сортировкой по номеру
''' </summary>
Public Class PropertyOrderPair
    Implements IComparable

    Private _order As Integer
    Public ReadOnly Property Name() As String

    Public Sub New(ByVal name As String, ByVal order As Integer)
        _order = order
        Me.Name = name
    End Sub

    ''' <summary>
    ''' Собственно метод сравнения
    ''' </summary>
    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim otherOrder As Integer = DirectCast(obj, PropertyOrderPair)._order
        If otherOrder = _order Then
            ' если Order одинаковый - сортируем по именам
            Dim otherName As String = DirectCast(obj, PropertyOrderPair).Name
            Return String.Compare(Name, otherName)
        ElseIf otherOrder > _order Then
            Return -1
        End If
        Return 1
    End Function
End Class
#End Region

'''' <summary>
'''' Данные, входящие в цель ипытаний
'''' </summary>
'<TypeConverter(GetType(ExpandableObjectConverter))> _
'<Serializable()> _
'Class TestingData
'    Implements ISerializationSurrogate 'для сереализации

'    'для сереализации
'    Public Sub GetObjectData(ByVal obj As Object, ByVal info As SerializationInfo, _
'   ByVal context As StreamingContext) Implements ISerializationSurrogate.GetObjectData
'        Dim flags As BindingFlags = BindingFlags.Instance Or _
'           BindingFlags.Public Or BindingFlags.Public
'        For Each fi As FieldInfo In obj.GetType().GetFields(flags)
'            info.AddValue(fi.Name, fi.GetValue(obj))
'        Next
'    End Sub

'    Public Function SetObjectData(ByVal obj As Object, ByVal info As SerializationInfo, _
'       ByVal context As StreamingContext, ByVal selector As ISurrogateSelector) _
'       As Object Implements ISerializationSurrogate.SetObjectData
'        Dim flags As BindingFlags = BindingFlags.Instance Or _
'           BindingFlags.Public Or BindingFlags.Public
'        For Each fi As FieldInfo In obj.GetType().GetFields(flags)
'            fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType))
'        Next
'        Return obj
'    End Function

'    ''' <summary>
'    ''' Конструктор
'    ''' </summary>
'    Public Sub New(ByVal сборка As String, ByVal постановка As String, ByVal запуск As UInteger)
'        Сборка_ = сборка
'        Постановка_ = постановка
'        Запуск_ = запуск
'    End Sub

'    Private Сборка_ As String

'    ''' <summary>
'    ''' Сборка
'    ''' </summary>
'    <DisplayName("Сборка")> _
'    <Description("Назначение сборки")> _
'    Public Property Сборка() As String
'        Get
'            Return Сборка_
'        End Get
'        Set(ByVal value As String)
'            Сборка_ = value
'        End Set
'    End Property

'    Private Постановка_ As String

'    ''' <summary>
'    ''' Цель постановки
'    ''' </summary>
'    <DisplayName("Постановка")> _
'    <Description("Цель постановки")> _
'    Public Property Постановка() As String
'        Get
'            Return Постановка_
'        End Get
'        Set(ByVal value As String)
'            Постановка_ = value
'        End Set
'    End Property

'    Private Запуск_ As UInteger

'    ''' <summary>
'    ''' Номер запуска
'    ''' </summary>
'    <DisplayName("Запуск")> _
'    <Description("Номер запуска")> _
'    Public Property Запуск() As UInteger
'        Get
'            Return Запуск_
'        End Get
'        Set(ByVal value As UInteger)
'            Запуск_ = value
'        End Set
'    End Property

'    ''' <summary>
'    ''' Представление в виде строки
'    ''' </summary>
'    Public Overloads Overrides Function ToString() As String
'        Return Сборка_ + ", " + Постановка_ + " - " + CStr(Запуск_)
'    End Function
'End Class

'''' <summary>
'''' TypeConverter для bool
'''' </summary>
'Class ТипРаботыBooleanTypeConverter
'    Inherits BooleanConverter
'    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
'        Return IIf(CBool(value), "С контроллером", "Автономно")
'    End Function

'    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
'        Return DirectCast(value, String) = "С контроллером"
'    End Function
'End Class