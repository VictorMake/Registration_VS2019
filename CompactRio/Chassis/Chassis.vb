''' <summary>
''' Описывает свойство шасси cRio
''' </summary>
''' <remarks></remarks>
Friend Class Chassis
    ''' <summary>
    ''' Инкапсуляция коллекции каналов посредством словаря.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GManagerChannels As ChannelsRio

    ''' <summary>
    ''' Имя шасси
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HostName As String

    ''' <summary>
    '''  IP Address шасси
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IPAddressRTtarget As IPAddressTargetCRIO

    ''' <summary>
    ''' Режим измерения или ещё и управление
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ModeWork As EnumModeWork

    ''' <summary>
    ''' Папка с файлами рабочих проектов для копирования на шасси
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FolderName As String

    ''' <summary>
    ''' Надо ли производить новое копирование проектов на шасси
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsCopyFolder As Boolean

    ''' <summary>
    ''' Есть ли файлы для заливки на FPGA
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsContainsFPGABitfiles As Boolean

    ''' <summary>
    ''' Число включённых каналов подлежащих измерению (плюс каналов управления для управляющих шасси)
    ''' ArrayLength
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChannelsCount As Integer

    ''' <summary>
    ''' Наличие соединения шасси с ИВК
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsConnected As Boolean

    ''' <summary>
    ''' Проверка выполнения запроса списка команд
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GetMetaSuccess As Boolean

    ''' <summary>
    ''' Успех инициализации модулей шасси 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsInitSuccess As Boolean

    ''' <summary>
    ''' Инициализация модулей не была произведена
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsInitBad As Boolean

    ''' <summary>
    ''' Первый запуск опроса каналов произведён успешно
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsLaunchSuccess As Boolean

    ''' <summary>
    ''' Первый запуск опроса каналов произведён с ошибкой
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsLaunchBad As Boolean

    ''' <summary>
    ''' Критическая ошибка произошедшая во время работы шасси
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CriticalError As String

    ''' <summary>
    ''' Индекс для связи со строкой таблицы
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IndexRow As Integer

    ''' <summary>
    ''' Список каналов подлежащих сбору
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListChannelsToAcquire() As String()

    ''' <summary>
    ''' почти полностью повторяет конструктор TargetCRIO
    ''' </summary>
    ''' <param name="inHostName"></param>
    ''' <param name="inIPAddressRTtarget"></param>
    ''' <param name="inModeWork"></param>
    ''' <param name="inFolderName"></param>
    ''' <param name="inCopyFolder"></param>
    ''' <remarks></remarks>
    Public Sub New(inHostName As String, inIPAddressRTtarget As IPAddressTargetCRIO, inModeWork As EnumModeWork, inFolderName As String, inCopyFolder As Boolean)
        HostName = inHostName
        IPAddressRTtarget = inIPAddressRTtarget
        ModeWork = inModeWork
        FolderName = inFolderName
        IsCopyFolder = inCopyFolder
        GManagerChannels = New ChannelsRio
    End Sub
End Class