Friend Class ItemDigitalInput
    Public Property Value() As Double
    ''' <summary>
    ''' НомерУстройства
    ''' </summary>
    ''' <returns></returns>
    Public Property DeviseNumber() As Short
    ''' <summary>
    ''' Номер Модуля в Корзине
    ''' </summary>
    ''' <returns></returns>
    Public Property ModuleNumderInChassis() As String
    ''' <summary>
    ''' Номер Порта
    ''' </summary>
    ''' <returns></returns>
    Public Property PortNumber() As Short
    ''' <summary>
    ''' Номер Линии
    ''' </summary>
    ''' <returns></returns>
    Public Property LineNumber() As Short
    ''' <summary>
    ''' Индекс В Массиве Знаений
    ''' </summary>
    ''' <returns></returns>
    Public Property IndexInArray() As Integer
    ''' <summary>
    ''' Диапазон Изменения Max
    ''' </summary>
    ''' <returns></returns>
    Public Property RangeMax() As Single
    ''' <summary>
    ''' Диапазон Изменения Min
    ''' </summary>
    ''' <returns></returns>
    Public Property RangeMin() As Single
    ''' <summary>
    ''' Единица Измерения
    ''' </summary>
    ''' <returns></returns>
    Public Property Unit() As String
    ''' <summary>
    ''' Наименование Параметра
    ''' </summary>
    ''' <returns></returns>
    Public Property ParameterName() As String
End Class