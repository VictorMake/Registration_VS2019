Imports VB = Microsoft.VisualBasic

Friend NotInheritable Class FormAbout

    ' для регистрации
    'Private clsСимвол As New Symbol()
    Private X1, Y1 As Integer

    ' Reg Key параметры защиты...
    Const KEY_ALL_ACCESS As Integer = &H2003F

    ' Reg Key корневые типы...
    Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    Const ERROR_SUCCESS As Short = 0
    Const REG_SZ As Short = 1 ' Unicode пустой указатель законченной строки
    Const REG_SZ2 As Short = 2 ' добавил тип 2, которого ранее не было
    Const REG_DWORD As Short = 4 ' 32-bit номер

    Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
    Const gREGVALSYSINFOLOC As String = "MSINFO"
    Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    Const gREGVALSYSINFO As String = "PATH"

    Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer

    Const Xposition As Integer = 90
    Const Yposition As Integer = 25

    Private Sub frmAbout_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        KeyPreview = True
        lblDescription.Text = NativeMethods.Notice
        lblVersion.Text = $".net Версия {FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart}.{FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart & FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion()}"
        lblTitle.Text = Reflection.Assembly.GetExecutingAssembly.GetName.Name

        '' Установить заголовок формы.
        'Dim ApplicationTitle As String
        'If My.Application.Info.Title <> "" Then
        '    ApplicationTitle = My.Application.Info.Title
        'Else
        '    ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        'End If
        'Me.Text = String.Format("О программе {0}", ApplicationTitle)
        '' Инициализировать текст, отображаемый в окне "О программе".
        '' TODO: настроить сведения о сборке приложения в области "Приложение" диалогового окна 
        ''    свойств проекта (в меню "Проект").
        'Me.LabelProductName.Text = My.Application.Info.ProductName
        'Me.LabelVersion.Text = String.Format("Версия {0}", My.Application.Info.Version.ToString)
        'Me.LabelCopyright.Text = My.Application.Info.Copyright
        'Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        'Me.TextBoxDescription.Text = My.Application.Info.Description
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
    End Sub

    Private Sub cmdSysInfo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSysInfo.Click
        StartSysInfo()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Close()
    End Sub

    Public Sub StartSysInfo()
        Dim sysInfoPath As String = String.Empty

        Try
            ' Проверить, чтобы получить значение путь\имя информацию из системного реестра...
            If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, sysInfoPath) Then
                ' Проверить, чтобы получить значение программной информацию только из системного реестра...
            ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, sysInfoPath) Then
                ' Проверить правильность зарегистрированных 32 битных версий
                If (Dir(sysInfoPath & "\MSINFO32.EXE") <> "") Then
                    sysInfoPath &= "\MSINFO32.EXE"
                    ' Ошибка - файл не может быть найден...
                Else
                    MessageBox.Show("Системная информация сейчас не доступна", "Старт получения информации", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                ' Ошибка - запись системного реестра не может быть найдена...
            Else
                MessageBox.Show("Системная информация сейчас не доступна", "Старт получения информации", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Shell(sysInfoPath, AppWinStyle.NormalFocus)
        Catch ex As Exception
            Const caption As String = "Старт получения информации"
            Const text As String = "Системная информация сейчас не доступна"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
        Dim I As Integer ' Счетчик цикла
        Dim rc As Integer ' Код возврата
        Dim hKey As Integer ' дескриптор на открытый ключ системного реестра
        'Dim hDepth As Integer '
        Dim KeyValType As Integer ' Тип данных ключа системного реестра
        Dim tmpVal As String ' Временная для хранение значения ключа системного реестра
        Dim KeyValSize As Integer ' Размер переменной ключа системного реестра
        '------------------------------------------------------------
        ' Открыть RegKey ниже KeyRoot {HKEY_LOCAL_MACHINE...}
        '------------------------------------------------------------
        rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Открыть ключ системного реестра

        If (rc <> ERROR_SUCCESS) Then ' Ошибки дескриптора
            KeyVal = vbNullString ' Очистка
            rc = RegCloseKey(hKey) ' Закрытие ключа системного реестра
            Return False ' Возврат ошибки
        Else
            tmpVal = New String(Chr(0), 1024) ' Распределить временное место
            KeyValSize = 1024 ' Отметьте переменный Размер
            '------------------------------------------------------------
            ' отыскать зарегистрированное значение ключа...
            '------------------------------------------------------------
            rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' считывание/установка значение ключа

            If (rc <> ERROR_SUCCESS) Then ' Ошибки дескриптора
                KeyVal = vbNullString ' Очистка, чтобы Освободить Строку
                rc = RegCloseKey(hKey) ' Закрытие ключа системного реестра
                Return False ' Возврат ошибки
            Else
                tmpVal = VB.Left(tmpVal, InStr(tmpVal, Chr(0)) - 1)
                '------------------------------------------------------------
                ' Определить значения тип ключа  для преобразования...
                '------------------------------------------------------------
                Select Case KeyValType ' Анализ Типа данных ...
                    Case REG_SZ, REG_SZ2 ' Тип данных ключа системного реестра строки
                        KeyVal = tmpVal ' Копирование значение строки
                    Case REG_DWORD ' Двойной Тип данных ключа Системного реестра Слова
                        For I = Len(tmpVal) To 1 Step -1 ' Преобразование каждого бита
                            KeyVal &= Hex(Asc(Mid(tmpVal, I, 1))) ' Формирование .
                        Next
                        'KeyVal = VB6.Format("&h" & KeyVal) ' Преобразование двойного слова, чтобы привести
                        KeyVal = "&h" & KeyVal
                End Select

                rc = RegCloseKey(hKey) ' Закрыть ключ системного реестра
                Return True ' Возврат успех
            End If
        End If
    End Function

    '''' <summary>
    '''' для регистрации
    '''' </summary>
    'Public Sub TryRgstrtn()
    '    Dim slRegNumber As Object = "954 - XXL - 03" ' позволить пользователю отметиться
    '    ' пароль введен правильно проверка положения курсора
    '    Dim slValue As String
    '    Dim slAllow As String
    '    Dim rc As Object
    '    'Dim Reply As String
    '    Dim phkРезультат As Integer
    '    Dim SA As NativeMethods.SECURITY_ATTRIBUTES
    '    Dim slПодраздел As String

    '    If X1 > Xposition And Y1 > Yposition Then ' временная лицензия
    '        NativeMethods.glRgstrdOk = True
    '        ' Подраздел для хранения данных
    '        NativeMethods.SubKey = "SOFTWARE\Prosoft\" & "Iconics\Genesis32"
    '        slValue = "Keys"

    '        If NativeMethods.CreateRegKey(NativeMethods.SubKey) Then
    '            slAllow = "200"
    '            'если раздел создан, создать параметр для количества запусков
    '            rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, slValue, slAllow)
    '            rc = vbNullString
    '            rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, "Number", "")
    '            '            frmAbout.Label1 = "Количество оставщихся" & _
    '            ''            " запусков = " & slКолЗапусков
    '            lblDescription.Text = NativeMethods.conRgstrdTmp
    '            NativeMethods.Notice = NativeMethods.conRgstrdTmp
    '        Else
    '            MessageBox.Show("Не удалось создать раздел", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End If
    '    ElseIf Y1 < Yposition And X1 > Xposition Then  ' удаление лицензии
    '        '---------------------------------------------------
    '        ' Удалить регистрационный раздел
    '        '---------------------------------------------------
    '        NativeMethods.glRgstrdOk = False
    '        'открыть текущий раздел
    '        slПодраздел = "SOFTWARE\Prosoft\Iconics"
    '        NativeMethods.RegCreateKeyEx(NativeMethods.hKey, slПодраздел, 0, "", NativeMethods.REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, SA, phkРезультат, 0)

    '        ' удалить текущий раздел
    '        If NativeMethods.RegDeleteKey(phkРезультат, "Genesis32") = ERROR_SUCCESS Then
    '        Else
    '            ' закрыть текущий раздел
    '            RegCloseKey(phkРезультат)
    '            MessageBox.Show("Ошибка при удалении раздела", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        ' открыть текущий раздел
    '        slПодраздел = "SOFTWARE\Prosoft"
    '        NativeMethods.RegCreateKeyEx(NativeMethods.hKey, slПодраздел, 0, "", NativeMethods.REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, SA, phkРезультат, 0)

    '        ' удалить раздел DevHand
    '        If NativeMethods.RegDeleteKey(phkРезультат, "Iconics") = ERROR_SUCCESS Then
    '            '            MsgBox "Регистрационные данные удалены."
    '            NativeMethods.glRgstrdOk = False
    '            NativeMethods.glRgstrdNumber = vbNullString
    '            lblDescription.Text = NativeMethods.conUnRgstrd
    '            NativeMethods.Notice = NativeMethods.conUnRgstrd
    '        Else
    '            ' Закрыть текущий раздел
    '            RegCloseKey(phkРезультат)
    '            MessageBox.Show("Ошибка при удалении раздела", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End If

    '    ElseIf X1 < Xposition And Y1 < Yposition Then  ' постоянная лицензия
    '        ' Добавьть алгоритм, чтобы проверить правильность регистрационного
    '        ' Номер должен закончиться на 3
    '        'If VB.Right(slRegNumber, 1) = "3" Then
    '        ' Поместить регистрационный номер в системный реестр

    '        ' занести случайное число
    '        Randomize() ' Инициализация генератора случайного числа.
    '        ' генерация случайного значения между 1 и 1000000
    '        If Not NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, "Number", CStr(CInt(Int((1000000 * Rnd()) + 1)))) Then
    '            'If Not Модуль2.SetRegValue(hKey, SubKey, "Number", slRegNumber) Then
    '            MessageBox.Show("Не удалось задать значение параметра.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        Else
    '            If CryptoSample.CreateFile("") Then
    '                'txtCrypto.Text = crpSample.CryptoOutput
    '                MessageBox.Show("Успешно сгенерирован и сохранен .dat.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '                NativeMethods.glRgstrdOk = True
    '                NativeMethods.glRgstrdNumber = slRegNumber
    '                lblDescription.Text = NativeMethods.conRgstrd
    '                NativeMethods.Notice = NativeMethods.conRgstrd
    '            End If
    '            '                Me.Caption = "Отмеченная копия"
    '            '            Command1.Visible = False
    '            '                MsgBox "Отметка успешно завершена."
    '            'Unload Me
    '        End If
    '        'Else
    '        '    ' неправильный регистрационный номер
    '        '    MessageBox.Show("Неверный регистрационный номер.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        '    End
    '        'End If
    '    End If
    'End Sub

    'Private Sub frmAbout_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    '    Dim nKeyCode As Short = e.KeyCode
    '    'Dim nShift As Short = e.KeyData \ &H10000
    '    clsСимвол.Char_Renamed = nKeyCode
    'End Sub

    'Private Sub frmAbout_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
    '    'Dim Button As Short = e.Button \ &H100000
    '    'Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
    '    Dim x As Double = e.X ' VB6.PixelsToTwipsX(e.X)
    '    Dim y As Double = e.Y ' VB6.PixelsToTwipsY(e.Y)
    '    X1 = x
    '    Y1 = y
    '    LabelX.Text = "X= " & x
    '    LabelY.Text = "Y= " & y
    'End Sub
End Class
