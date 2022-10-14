Imports System.IO

''' <summary>
''' Регистрация сообщений о системных и программных событиях Windows посредством EventLogRegistration.dll
''' </summary>
Public Class RegistrationEventLog
    Private Enum eventID As Integer
        AUDIT_SUCCESS_ID_3000 = 3000
        MSG_CONNECT_ID_3001 = 3001
        MSG_CONNECT_FAILED_ID_3002 = 3002
        MSG_DB_UPDATE_ID_3003 = 3003
        MSG_DB_UPDATE_FAILED_ID_3004 = 3004
        MSG_EXCEPTION_ID_3005 = 3005
        MSG_FILE_IO_UPDATE_ID_3006 = 3006
        MSG_FILE_IO_FAILED_ID_3007 = 3007
        MSG_USER_ACTION_ID_3008 = 3008
        MSG_APPLICATION_MESSAGE_ID_3009 = 3009
        MSG_RELEVANT_QUESTION_ID_3010 = 3010
        EVENT_LOG_DISPLAY_NAME_MSGID = 5001
        EVENT_LOG_SERVICE_NAME_MSGID = 5002
    End Enum

    Private Enum category As Short
        AUDIT_SUCCESS = 1
        MSG_CONNECT = 2
        MSG_CONNECT_FAILED = 3
        MSG_DB_UPDATE = 4
        MSG_DB_UPDATE_FAILED = 5
    End Enum

    Public Shared evLog As EventLog
    Private Shared logName As String = "Registration_Log"
    Private Shared sourceName As String = "Registration_EventLog"
    Private Shared Property pathResourceFileDLL As String

    ''' <summary>
    ''' Инициализирует экземпляр класса System.Diagnostics.EventSourceCreationData
    ''' с заданным именем источника событий и журнала событий.
    ''' </summary>
    ''' <param name="pathOptions"></param>
    Public Shared Sub CreateRegistrationEventLog(pathOptions As String)
        pathResourceFileDLL = Path.Combine(PathResourses, "EventLogRegistration.dll")
        If FileNotExists(pathResourceFileDLL) Then
            MessageBox.Show("В каталоге нет файла <EventLogRegistration.dll>!", "Запуск приложения", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Environment.Exit(0) 'End
        End If

        If CBool(GetIni(pathOptions, "Options", "CreateEventSourceDeleteFirst", "False")) Then
            CreateEventSource(True) ' чтобы удалить созданный до этого
            ' вернуть True для последующего запуска новая регистрация уже будет проведена
            WriteINI(pathOptions, "Options", "CreateEventSourceDeleteFirst", "False")
        Else
            CreateEventSource(False) ' обычное первое обращение к классу
        End If
    End Sub

    Private Shared Sub CreateEventSource(isDeleteFirst As Boolean)
        If isDeleteFirst Then
            EventLog.DeleteEventSource(sourceName)
            EventLog.Delete(logName)
        End If

        If Not EventLog.SourceExists(sourceName) Then ' определить, если источник существут на компьютере
            If Not File.Exists(pathResourceFileDLL) Then
                MessageBox.Show("Ресурсный файл сообщений не существует", NameOf(CreateEventSource), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            ' Источник событий с локализованными ресурсами можно зарегистрировать для категории событий и строк сообщения.
            ' Для внесения записей в журнал событий в приложении вместо указания фактических строк используются идентификаторы ресурсов.
            ' В приложении "Просмотр событий" для поиска и отображения соответствующей строки из локализованного файла ресурсов,
            ' зависящего от текущих языковых параметров, используется идентификатор ресурса.
            ' Для категорий событий, сообщений и строк вставки параметров можно зарегистрировать отдельный файл, или зарегистрировать такой же файл ресурса для всех трех типов строк.
            ' Для настройки источника внесения локализованных записей в журнал событий используйте свойства CategoryCount, CategoryResourceFile, MessageResourceFile и ParameterResourceFile.
            ' Если приложение записывает значения строк напрямую в журнал событий, нет необходимости устанавливать эти свойства.
            ' Свойства CategoryCount и CategoryResourceFile используются для записи событий с локализованными строками категорий.
            ' Приложение "Просмотр событий" отображает категорию для записи события, если указать категорию при написании события.
            ' Категории журнала событий — это определенные приложением строки, помогающие проводить фильтрацию событий или предоставляющие информацию о событии.
            ' Например, приложение может определить отдельные категории для различных компонентов или операций.

            ' Данный метод создает источник событий, заданный в свойствах Source, регистрирует его в журнале событий, указанном в LogName
            Dim eventSource As New EventSourceCreationData(sourceName, logName) With {
                .CategoryResourceFile = pathResourceFileDLL, ' Возвращает или задает путь к файлу ресурсов, содержащему строки категорий источника.
                .CategoryCount = [Enum].GetNames(GetType(category)).Count, '5 ' Возвращает или задает число категорий в файле ресурсов категорий.
                .MessageResourceFile = pathResourceFileDLL, ' Возвращает или задает путь к файлу ресурсов сообщения, содержащему сообщение о форматировании строк источника.
                .ParameterResourceFile = pathResourceFileDLL ' Возвращает или задает путь к файлу ресурсов, содержащему строки параметров сообщения источника.
                }
            ' Создать новый источник событий при установке приложения.
            ' Благодаря этому операционная система успевает обновить свой список зарегистрированных источников событий и их конфигурации.
            ' Если список источников событий в системе не обновился и предпринимается попытка записать событие с новым источником, возникнет ошибка выполнения операции записи.
            ' Новый источник можно настроить, используя EventLogInstaller или метод CreateEventSource.
            ' Для создания нового источника событий необходимо иметь на компьютере права администратора.
            ' Можно создать источник событий для существующего журнала событий или для нового журнала событий.
            ' При создании нового источника для нового журнала событий система регистрирует источник для этого журнала, но журнал не будет создан, пока в него не будет внесена первая запись.
            EventLog.CreateEventSource(eventSource) ' Задает приложение как источник записей для определенного журнала событий в системе
        Else
            logName = EventLog.LogNameFromSourceName(sourceName, ".")
        End If
        ' Регистрировать локализованное имя журнала событий
        'Dim evLog As New EventLog(logName, ".", sourceName)
        evLog = New EventLog(logName, ".", sourceName)
        evLog.RegisterDisplayName(pathResourceFileDLL, eventID.EVENT_LOG_DISPLAY_NAME_MSGID)

        ''Использовать методы WriteEvent и WriteEntry, чтобы записать события в журнал событий. Для записи событий необходимо задать источник события. 
        'Using log = New EventLog(logName, ".", sourceName)
        '    'Метод WriteEvent используется для записи событий с использованием локализованного файла ресурсов сообщений. 

        '    Dim info As New EventInstance(1000, 4, EventLogEntryType.Information)
        '    'log.WriteEvent(info)
        '    log.WriteEvent(info, "Сообщение 1000")

        '    Dim info2 As New EventInstance(1001, 4, EventLogEntryType.[Error])
        '    'log.WriteEvent(info2, "Плохо")
        '    log.WriteEvent(info2, "Сообщение 1001")

        '    Dim info3 As New EventInstance(1002, 3, EventLogEntryType.[Error])
        '    Dim addionalInfo As Byte() = {1, 2, 3}
        '    log.WriteEvent(info3, addionalInfo)


        '    'Метод WriteEntry записывает заданную строку прямо в журнал событий; он не использует файл ресурсов локализуемых сообщений.
        '    log.WriteEntry("Сообщение 1")
        '    log.WriteEntry("Message 2", EventLogEntryType.Warning, 1001)
        '    log.WriteEntry("Message 3", EventLogEntryType.Information, 33)
        'End Using

        'EventLog.WriteEntry("Registration_EventLog", "Message 1");
        'EventLog.WriteEntry("Registration_EventLog", "Message 2", EventLogEntryType.Warning);
        'EventLog.WriteEntry("Registration_EventLog", "Message 3", EventLogEntryType.Error, 37);
    End Sub

#Region "EventLog"
    '     Инициализирует новый экземпляр класса System.Diagnostics.EventLog. Связывает
    '     экземпляр с журналом на указанном компьютере и создает или присваивает заданный
    '     источник классу System.Diagnostics.EventLog.

    ''' <summary>
    ''' Сообщение об успешном действии входа пользователя.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_AUDIT_SUCCESS(ParamArray values() As Object)
        ' какое-то сообщение об ошибке
        ' Используйте методы WriteEvent и WriteEntry, чтобы записать события в журнал событий. Для записи событий необходимо задать источник события. 
        Using log = New EventLog(logName, ".", sourceName)
            ' Метод WriteEvent используется для записи событий с использованием локализованного файла ресурсов сообщений. 
            'Dim info As New EventInstance(eventID.AUDIT_SUCCESS_ID_3000, category.AUDIT_SUCCESS, EventLogEntryType.SuccessAudit)
            'log.WriteEvent(info, values)
            ' Метод WriteEntry записывает заданную строку прямо в журнал событий; он не использует файл ресурсов локализуемых сообщений.
            log.WriteEntry(CStr(values(0)), EventLogEntryType.SuccessAudit, eventID.AUDIT_SUCCESS_ID_3000, category.AUDIT_SUCCESS)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение об успешной работе по протоколу TCP/IP.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_CONNECT(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_CONNECT_ID_3001, category.MSG_CONNECT, EventLogEntryType.Information)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Information, eventID.MSG_CONNECT_ID_3001, category.MSG_CONNECT)
        End Using
    End Sub

    ''' <summary>
    '''  Сообщение о проблеме при работе по протоколу TCP/IP.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_CONNECT_FAILED(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_CONNECT_FAILED_ID_3002, category.MSG_CONNECT_FAILED, EventLogEntryType.Warning)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Warning, eventID.MSG_CONNECT_FAILED_ID_3002, category.MSG_CONNECT_FAILED)
        End Using
    End Sub

    ''' <summary>
    '''  Сообщение об успешном обновлении базы данных.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_DB_UPDATE(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_DB_UPDATE_ID_3003, category.MSG_DB_UPDATE, EventLogEntryType.Information)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Information, eventID.MSG_DB_UPDATE_ID_3003, category.MSG_DB_UPDATE)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение о проблемах при работе с базой данных.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_DB_UPDATE_FAILED(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_DB_UPDATE_FAILED_ID_3004, category.MSG_DB_UPDATE_FAILED, EventLogEntryType.Warning)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Warning, eventID.MSG_DB_UPDATE_FAILED_ID_3004, category.MSG_DB_UPDATE_FAILED)
        End Using
    End Sub

    ''' <summary>
    '''  Сообщение при исключительной ситуации.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_EXCEPTION(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_EXCEPTION_ID_3005, category.AUDIT_SUCCESS, EventLogEntryType.Error)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Error, eventID.MSG_EXCEPTION_ID_3005, category.AUDIT_SUCCESS)
        End Using
    End Sub

    ''' <summary>
    '''  Сообщение об успешном записи файла.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_FILE_IO_UPDATE(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_FILE_IO_UPDATE_ID_3006, category.MSG_DB_UPDATE, EventLogEntryType.Information)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Information, eventID.MSG_FILE_IO_UPDATE_ID_3006, category.MSG_DB_UPDATE)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение о проблемах при работе с файлом.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_FILE_IO_FAILED(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_FILE_IO_FAILED_ID_3007, category.MSG_DB_UPDATE_FAILED, EventLogEntryType.Warning)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Warning, eventID.MSG_FILE_IO_FAILED_ID_3007, category.MSG_DB_UPDATE_FAILED)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение об действиях пользователя.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_USER_ACTION(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_USER_ACTION_ID_3008, category.AUDIT_SUCCESS, EventLogEntryType.SuccessAudit)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.SuccessAudit, eventID.MSG_USER_ACTION_ID_3008, category.AUDIT_SUCCESS)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение о том, что приложение что-то сообщает пользователю.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_APPLICATION_MESSAGE(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_APPLICATION_MESSAGE_ID_3009, category.AUDIT_SUCCESS, EventLogEntryType.Information)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Information, eventID.MSG_APPLICATION_MESSAGE_ID_3009, category.AUDIT_SUCCESS)
        End Using
    End Sub

    ''' <summary>
    ''' Сообщение о том, что приложение запрашивает действия от пользователя.
    ''' </summary>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Public Shared Sub EventLog_MSG_RELEVANT_QUESTION(ParamArray values() As Object)
        Using log = New EventLog(logName, ".", sourceName)
            'Dim info As New EventInstance(eventID.MSG_RELEVANT_QUESTION_ID_3010, category.AUDIT_SUCCESS, EventLogEntryType.Warning)
            'log.WriteEvent(info, values)
            log.WriteEntry(CStr(values(0)), EventLogEntryType.Warning, eventID.MSG_RELEVANT_QUESTION_ID_3010, category.AUDIT_SUCCESS)
        End Using
    End Sub
#End Region
End Class
