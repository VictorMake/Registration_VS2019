Imports System.Runtime.InteropServices

Friend NotInheritable Class NativeMethods
    Private Sub New()
    End Sub

    '<DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    'Friend Shared Function MessageBeep(ByVal uType As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    'End Function

    <DllImport("user32.dll", EntryPoint:="CreateIconIndirect")>
    Friend Shared Function CreateIconIndirect(ByVal iconInfo As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Friend Shared Function DestroyIcon(ByVal handle As IntPtr) As Boolean
    End Function

    <DllImport("gdi32.dll")>
    Friend Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
    End Function

    ' для записи в Опции.INI
    Friend Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String,
                                                                                                     ByVal lpKeyName As String,
                                                                                                     ByVal lpDefault As String,
                                                                                                     ByVal lpReturnedString As String,
                                                                                                     ByVal nSize As Integer,
                                                                                                     ByVal lpFileName As String) As Integer
    Friend Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String,
                                                                                                         ByVal lpKeyName As String,
                                                                                                         ByVal lpString As String,
                                                                                                         ByVal lpFileName As String) As Integer

    Friend Declare Function ShowWindow Lib "user32" (ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer
    Friend Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    'Friend Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Integer
    Friend Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As IntPtr) As Integer

    Friend Const SW_RESTORE As Short = 9
    'Friend Declare Function GetDiskFreeSpace Lib "kernel32" Alias "GetDiskFreeSpaceA" (ByVal lpRootPathNane As String, ByRef lpSectorsPerCluster As Integer, ByRef lpBytesPerSector As Integer, ByRef lpNumberOfFreeClusters As Integer, ByRef lpTotalNumberOfClusters As Integer) As Integer

    <DllImport("user32.dll")>
    Friend Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

#Region "Declare Function"

    'Private Declare Function OSWinHelp Lib "user32"  Alias "WinHelpA"(ByVal hwnd As Integer, ByVal HelpFile As String, ByVal wCommand As Short, ByRef dwData As Any) As Short
    'Private Const EWX_LogOff As Integer = 0
    Friend Const EWX_SHUTDOWN As Integer = 1
    'Private Const EWX_REBOOT As Integer = 2
    Friend Const EWX_FORCE As Integer = 4
    'Private Const EWX_POWEROFF As Integer = 8

    ' Функция ExitWindowsEx или выходит, завершает, или закрывает
    ' и вызывает перезагрузку система.
    Friend Declare Function ExitWindowsEx Lib "user32" (ByVal dwOptions As Integer, ByVal dwReserved As Integer) As Integer

    ' Функция GetLastError возвращает значение кода последней ошибку выполняемого потока.
    ' Код последней ошибки поддерживается на принципе " в нить ".
    ' Множественные потоки не записывают поверх друг друга последнюю ошибку кода.
    Friend Declare Function GetLastError Lib "kernel32" () As Integer

    Friend Const mlngWindows95 As Short = 0
    Friend Const mlngWindowsNT As Short = 1

    Friend Shared glngWhichWindows32 As Integer

    ' Функция GetVersion возвращает исполняющую операционную систему.
    Friend Declare Function GetVersion Lib "kernel32" () As Integer

    Friend Structure LUID
        Dim UsedPart As Integer
        Dim IgnoredForNowHigh32BitPart As Integer
    End Structure

    Friend Structure LUID_AND_ATTRIBUTES
        Dim TheLuid As LUID
        Dim Attributes As Integer
    End Structure

    Friend Structure TOKEN_PRIVILEGES
        Dim PrivilegeCount As Integer
        Dim TheLuid As LUID
        Dim Attributes As Integer
    End Structure

    ' Функция GetCurrentProcess возвращает псевдомаркер(псевдодескриптор) для текущего процесса.
    Friend Declare Function GetCurrentProcess Lib "kernel32" () As Integer

    ' Функция OpenProcessToken открывает лексему(маркер) доступа, связанную с процессом.
    Friend Declare Function OpenProcessToken Lib "advapi32" (ByVal ProcessHandle As Integer,
                                                             ByVal DesiredAccess As Integer,
                                                             ByRef TokenHandle As Integer) As Integer

    ' Функция LookupPrivilegeValue отыскивает уникальный идентификатор (LUID) используемый на указанной системе,
    ' чтобы в местном масштабе представить указанное имя привилегии.
    Friend Declare Function LookupPrivilegeValue Lib "advapi32" Alias "LookupPrivilegeValueA" (ByVal lpSystemName As String,
                                                                                               ByVal lpName As String,
                                                                                               ByRef lpLuid As LUID) As Integer

    ' Функция AdjustTokenPrivileges допускает или отключает привилегии
    ' в указанной лексеме(маркере) доступа. Предоставление или отключение привилегий
    ' в лексеме(маркере) доступа требует доступа TOKEN_ADJUST_PRIVILEGES.
    Friend Declare Function AdjustTokenPrivileges Lib "advapi32" (ByVal TokenHandle As Integer,
                                                                  ByVal DisableAllPrivileges As Integer,
                                                                  ByRef NewState As TOKEN_PRIVILEGES,
                                                                  ByVal BufferLength As Integer,
                                                                  ByRef PreviousState As TOKEN_PRIVILEGES,
                                                                  ByRef ReturnLength As Integer) As Integer

    Friend Declare Sub SetLastError Lib "kernel32" (ByVal dwErrCode As Integer)

    ' декларация API показать или скрыть курсор мыши
    'Private Declare Function ShowCursor Lib "user32" (ByVal bShow As Integer) As Integer

    'Десларация API копирования всего экрана
    '        Private Declare Function BitBlt _
    ''        Lib "gdi32" ( _
    ''            ByVal hDestDC As Long, _
    ''            ByVal x As Long, ByVal y As Long, _
    ''            ByVal nWidth As Long, _
    ''            ByVal nHeight As Long, _
    ''            ByVal hSrcDC As Long, _
    ''            ByVal xSrc As Long, ByVal ySrc As Long, _
    ''            ByVal dwRop As Long _
    ''        ) As Long

    'Declare API to get описатель рабочего стола
    'Private Declare Function GetDesktopWindow Lib "user32" () As Integer

    'Declare API to convert описатель в контекст
    'Private Declare Function GetDC Lib "user32" (ByVal hwnd As Integer) As Integer

    'Declare API to освобождение контекста устройства
    'Private Declare Function ReleaseDC Lib "user32" (ByVal hwnd As Integer, ByVal hdc As Integer) As Integer

    'Private Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal nSrcWidth As Integer, ByVal nSrcHeight As Integer, ByVal dwRop As Integer) As Integer
#End Region


#Region "для регистрации"
    'Private Sub AdjustToken()
    '    ' ********************************************************************
    '    ' * Процедура устанавливает надлежащие привилегии, чтобы позволить регистрацию
    '    ' * производимую под Windows NT.
    '    ' ********************************************************************
    '    Const TOKEN_ADJUST_PRIVILEGES As Short = &H20S
    '    Const TOKEN_QUERY As Short = &H8S
    '    Const SE_PRIVILEGE_ENABLED As Short = &H2S

    '    Dim hdlProcessHandle As Integer
    '    Dim hdlTokenHandle As Integer
    '    Dim tmpLuid As NativeMethods.LUID
    '    Dim tkp As NativeMethods.TOKEN_PRIVILEGES
    '    Dim tkpNewButIgnored As NativeMethods.TOKEN_PRIVILEGES
    '    Dim lBufferNeeded As Integer

    '    ' Установить код ошибки последнего потока, который обнулит использование
    '    ' SetLast  вероятности ошибки. Делать это так, чтобы GetLastError
    '    ' функция не возвращала другое значение, чем нуль для
    '    ' неочевидной причины.
    '    SetLastError(0)

    '    ' Использовать функцию GetCurrentProcess, чтобы установить hdlProcessHandle
    '    hdlProcessHandle = NativeMethods.GetCurrentProcess()

    '    If NativeMethods.GetLastError <> 0 Then
    '        '            MsgBox "Ошибка выдачи текущего процесса==" & GetLastError
    '    End If

    '    NativeMethods.OpenProcessToken(hdlProcessHandle, TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY, hdlTokenHandle)

    '    If NativeMethods.GetLastError <> 0 Then
    '        '            MsgBox "Ошибка лексемы открытия процесса==" & GetLastError
    '    End If

    '    ' Получить LUID для завершения привилегии 
    '    NativeMethods.LookupPrivilegeValue("", "SeShutdownPrivilege", tmpLuid)

    '    If NativeMethods.GetLastError <> 0 Then
    '        '            MsgBox "Ошибка выдачи значения из таблицы привелегий==" & GetLastError
    '    End If

    '    tkp.PrivilegeCount = 1 ' Установить привилегию в 1
    '    tkp.TheLuid = tmpLuid
    '    tkp.Attributes = SE_PRIVILEGE_ENABLED

    '    ' Допустить привилегию завершения в лексеме(маркере) доступа этого процесса
    '    NativeMethods.AdjustTokenPrivileges(hdlTokenHandle, False, tkp, Len(tkpNewButIgnored), tkpNewButIgnored, lBufferNeeded)

    '    If NativeMethods.GetLastError <> 0 Then
    '        '            MsgBox "Ошибка корректировки таблицы привелегии==" & GetLastError
    '    End If
    'End Sub

    'Private Sub cmdForceShutdown()
    '    If NativeMethods.glngWhichWindows32 = NativeMethods.mlngWindowsNT Then
    '        AdjustToken()
    '        '            MsgBox "Post-AdjustToken выдача последней ошибки " & GetLastError
    '    End If

    '    NativeMethods.ExitWindowsEx(NativeMethods.EWX_SHUTDOWN Or NativeMethods.EWX_FORCE, &HFFFFS)
    '    '      MsgBox "ExitWindowsEx's выдача последней ошибки " & GetLastError
    'End Sub

    'Friend Shared Sub RegProgramm()
    '    If ReleaseFull = False Then
    '        If Not NativeMethods.glRgstrdOk Then
    '            'cmdForceShutdown()
    '            MessageBox.Show("Программа требует полной комплектности." & vbCrLf &
    '                            "Обратитесь к разработчику.", "Проверка комплектности", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            End
    '        End If
    '    End If
    'End Sub
#End Region

#Region "Работа с системным реестром Windows"
    Friend Const SWP_NOSIZE As Short = &H1S
    Friend Const SWP_NOMOVE As Short = &H2S
    Friend Const SWP_NOZORDER As Short = &H4S
    Friend Const SWP_NOREDRAW As Short = &H8S
    Friend Const SWP_NOACTIVATE As Short = &H10S
    Friend Const SWP_FRAMECHANGED As Short = &H20S
    Friend Const SWP_SHOWWINDOW As Short = &H40S
    Friend Const SWP_HIDEWINDOW As Short = &H80S
    Friend Const SWP_NOCOPYBITS As Short = &H100S
    Friend Const SWP_NOOWNERZORDER As Short = &H200S
    Friend Const SWP_DRAWFRAME As Short = SWP_FRAMECHANGED
    Friend Const SWP_NOREPOSITION As Short = SWP_NOOWNERZORDER

    ' hwndInsertAfter - значения для функции SetWindowPos()
    Friend Const HWND_TOP As Short = 0
    Friend Const HWND_BOTTOM As Short = 1
    Friend Const HWND_TOPMOST As Short = -1
    Friend Const HWND_NOTOPMOST As Short = -2

    ' для регистрации
    ' Глобальные переменные
    Friend Shared glRgstrdOk As Boolean
    Friend Shared glRgstrdNumber As String
    Friend Shared SubKey As String
    Friend Shared hKey As Integer
    Friend Shared Create As Integer
    Friend Shared Notice As String
    Friend Const conRgstrd As String = "Full"
    Friend Const conRgstrdTmp As String = "Temporary"
    Friend Const conUnRgstrd As String = "Disallow"
    Friend Const ReleaseFull As Boolean = True 'False 'True для отладки
    ' Константы Системного реестра
    Friend Const HKEY_CURRENT_USER As Integer = &H80000001

    ' Системный реестр Определенные Права доступа
    Friend Const KEY_QUERY_VALUE As Short = &H1S
    Friend Const KEY_SET_VALUE As Short = &H2S
    Friend Const KEY_CREATE_SUB_KEY As Short = &H4S
    Friend Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
    Friend Const KEY_NOTIFY As Short = &H10S
    Friend Const KEY_CREATE_LINK As Short = &H20S
    Friend Const KEY_ALL_ACCESS As Short = &H3FS

    ' Открыть / создатьПараметры
    Friend Const REG_OPTION_NON_VOLATILE As Short = 0
    Friend Const REG_OPTION_VOLATILE As Short = &H1S

    ' ключи creation/open позиции
    Friend Const REG_CREATED_NEW_KEY As Short = &H1S
    Friend Const REG_OPENED_EXISTING_KEY As Short = &H2S

    ' Маски для предопределенных стандартных типов доступа
    Friend Const STANDARD_RIGHTS_ALL As Integer = &H1F0000
    Friend Const SPECIFIC_RIGHTS_ALL As Short = &HFFFFS

    ' Определите коды серьезности ошибки
    Friend Const ERROR_SUCCESS As Short = 0
    Friend Const ERROR_ACCESS_DENIED As Short = 5
    Friend Const ERROR_NO_MORE_ITEMS As Short = 259

    ' Предопределенные Типы Значения
    ' Структуры, необходимые Для Прототипов Системного реестра
    Structure SECURITY_ATTRIBUTES
        Dim nLength As Integer
        Dim lpSecurityDescriptor As Integer
        Dim bInheritHandle As Boolean
    End Structure

    Structure FILETIME
        Dim dwLowDateTime As Integer
        Dim dwHighDateTime As Integer
    End Structure

    ' Прототипы Функции Системного реестра
    Friend Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkРезультат As Integer) As Integer
    Friend Declare Function RegSetValueEx Lib "advapi32" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal szData As String, ByVal cbData As Integer) As Integer
    Friend Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer
    Friend Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal szData As String, ByRef lpcbData As Integer) As Integer
    Friend Declare Function RegCreateKeyEx Lib "advapi32" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByRef phkРезультат As Integer, ByRef lpdwDisposition As Integer) As Integer
    Friend Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
    Friend Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer

    'Friend Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    ' объявление функции выключающей обновление изображение элемента
    ' Public Declare Function SendMassageVal Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As String) As Integer 'доработка
    ' Public Const WM_SETREDRAW As Long = 11 
    ' выключает обновление окна или контроля
    'Friend Declare Function LockWindowUpdate Lib "user32" (ByVal hwndLock As Integer) As Integer

    ''' <summary>
    ''' Изменяет или устанавливает Значение
    ''' </summary>
    ''' <param name="hKey"></param>
    ''' <param name="lpszSubKey"></param>
    ''' <param name="sSetValue"></param>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function SetRegValue(ByRef hKey As Integer, ByRef lpszSubKey As String, ByVal sSetValue As String, ByVal sValue As String) As Boolean
        Try
            Dim phkРезультат As Integer
            Dim lРезультат As Integer
            Dim SA As SECURITY_ATTRIBUTES
            Dim REG_SZ As Object = Nothing

            ' Обратить внимание: функция This создаст раздел или
            ' Значение, если это не существует.
            ' Открытия или Создания раздел
            RegCreateKeyEx(hKey, lpszSubKey, 0, "", REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, SA, phkРезультат, Create)

            lРезультат = RegSetValueEx(phkРезультат, sSetValue, 0, CInt(REG_SZ), sValue, CInt(Len(sValue) + 1))

            ' Закрыть раздел
            RegCloseKey(phkРезультат)

            ' Возвратить Результат SetRegValue
            Return lРезультат = ERROR_SUCCESS
        Catch ex As Exception
            Const caption As String = "Ошибка в процедуре <SetRegValue>"
            Dim text As String = $"{ex}{Environment.NewLine}Перезапустите программу и попробуйте снова."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            Return False
        End Try
    End Function

    ''' <summary>
    ''' Получить значение раздел
    ''' </summary>
    ''' <param name="hKey"></param>
    ''' <param name="lpszSubKey"></param>
    ''' <param name="szKey"></param>
    ''' <param name="szDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetRegValue(ByRef hKey As Integer, ByRef lpszSubKey As String, ByRef szKey As String, ByRef szDefault As String) As String 'As Variant
        Try
            Dim phkРезультат As Integer
            Dim lРезультат As Integer
            Dim szБуфер As String = Space(255)
            ' Создать буфер
            Dim lРазмерБуфера As Integer = Len(szБуфер)

            ' Открыть раздел
            RegOpenKeyEx(hKey, lpszSubKey, 0, 1, phkРезультат)
            ' Сделать запрос значения
            lРезультат = RegQueryValueEx(phkРезультат, szKey, 0, 0, szБуфер, lРазмерБуфера)
            ' закрыть раздел
            RegCloseKey(phkРезультат)

            ' Возвращение полученного значение
            If lРезультат = ERROR_SUCCESS Then
                Return Left(szБуфер, lРазмерБуфера - 1)
            Else
                Return szDefault
            End If
        Catch ex As Exception
            Const caption As String = "Ошибка в процедуре <GetRegValue>"
            Dim text As String = $"{ex}{Environment.NewLine}Перезапустите программу и попробуйте снова."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            Return vbNullString
        End Try
    End Function

    ''' <summary>
    ''' Создание нового раздела
    ''' </summary>
    ''' <param name="NewSubKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function CreateRegKey(ByRef NewSubKey As String) As Boolean
        Try
            Dim phkРезультат As Integer
            Dim SA As SECURITY_ATTRIBUTES
            ' Создать раздел, если он не существует
            Dim blnРезультат As Boolean = (RegCreateKeyEx(hKey, NewSubKey, 0, "", REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, SA, phkРезультат, Create) = ERROR_SUCCESS)

            ' Закрыть раздел
            RegCloseKey(phkРезультат)
            Return blnРезультат
        Catch ex As Exception
            Const caption As String = "Ошибка в процедуре <CreateRegKey>"
            Dim text As String = $"{ex}{Environment.NewLine}Перезапустите программу и попробуйте снова."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            Return False
        End Try
    End Function
#End Region

End Class

