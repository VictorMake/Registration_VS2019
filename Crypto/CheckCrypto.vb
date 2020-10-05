'Imports System.IO
'Imports System.Security.Cryptography
'Imports System.Threading
'Imports System.Runtime.InteropServices
'Imports System.Text.RegularExpressions
'Imports System.Text.Encoding

'Class CheckCrypto
'Private abytIV() As Byte
'Private abytKey() As Byte
'Private abytSalt() As Byte
'Private crpSym As SymmetricAlgorithm
'Private strPassword As String = ""
'Private mstrCryptoOutput As String = ""
'Private strSaltIVFile As String = ""
'Private strSourceFile As String = ""
'Private Const strRoot As String = "znbdznfr"

'<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> Private Structure OSVERSIONINFO
'    Dim dwOSVersionInfoSize As Integer
'    Dim dwMajorVersion As Integer
'    Dim dwMinorVersion As Integer
'    Dim dwBuildNumber As Integer
'    Dim dwPlatformId As Integer
'    <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public szCSDVersion As String
'End Structure

'Private Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (ByRef LpVersionInformation As OSVERSIONINFO) As Integer
''не пошел
''<DllImport("kernel32", EntryPoint:="GetVersionExA", CharSet:=CharSet.Ansi, ExactSpelling:=True, SetLastError:=True)> Private Function GetVersionEx(ByRef LpVersionInformation As OSVERSIONINFO) As Integer
''End Function

'Private Declare Function CopyStringA Lib "kernel32" Alias "lstrcpyA" (ByVal NewString As String, ByVal OldString As Integer) As Integer
'Private Declare Function lstrlenA Lib "kernel32" (ByVal lpString As Integer) As Integer
'Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
'Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
''Private Declare Function RegQueryValueEx Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Any, ByRef lpcbData As Integer) As Integer
'Private Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData() As Byte, ByRef lpcbData As Integer) As Integer
''Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef dest As Any, ByRef source As Any, ByVal numBytes As Integer)
'Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef dest As Integer, ByVal source() As Byte, ByVal numBytes As Integer)
'Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal dest As String, ByVal source() As Byte, ByVal numBytes As Integer)

'Private Const KEY_READ As Integer = &H20019

'Private Const REG_SZ As Short = 1
'Private Const REG_EXPAND_SZ As Short = 2
'Private Const REG_BINARY As Short = 3
'Private Const REG_DWORD As Short = 4
'Private Const REG_MULTI_SZ As Short = 7

'Private Const ERROR_MORE_DATA As Short = 234
''Private Const ERROR_SUCCESS As Short = 0

'Private Const MAX_SIZE As Short = 2048
'Private Const HKLM As Integer = &H80000002
'Private isNT As Boolean

'''' <summary>
'''' Создание нового симметричного алгоритма объекта выбранного типа.
'''' </summary>
'''' <param name="strCryptoName"></param>
'Public Sub New(ByVal strCryptoName As String)
'    '' Общий метод Create абстрактного симметричного алгоритма базового класса
'    '' реализует фабричный метод для создания конкретного класса.
'    'crpSym = SymmetricAlgorithm.Create(strCryptoName)
'    '' инициализация битового массива подходящей длины для crypto class.
'    'ReDimByteArrays()
'    'isNT = IsWindowsNT()
'End Sub

'Public Property CryptoOutput() As String
'    Get
'        Return mstrCryptoOutput
'    End Get
'    Set(ByVal Value As String)
'        mstrCryptoOutput = Value
'    End Set
'End Property

'Private Property Password() As String
'    Get
'        Return strPassword
'    End Get
'    Set(ByVal Value As String)
'        strPassword = Value
'    End Set
'End Property

'Public Property SaltIVFile() As String
'    Get
'        Return strSaltIVFile
'    End Get
'    Set(ByVal Value As String)
'        If File.Exists(Value) Then
'            strSaltIVFile = Value
'        Else
'            CreateKey(Value)

'            'Dim sw As StreamWriter = New StreamWriter(Value)
'            ' добавить какой-то текст в файл.
'            'sw.Write("")
'            'sw.Close()
'            'strSaltIVFile = Value

'            'Throw New FileNotFoundException("SaltIV .dat файл для " & _
'            '    "выбранного типа шифровки не найден. Перед шифрованием или " & _
'            '    "дешифрованием вы должны ввести этот файл.")
'        End If
'    End Set
'End Property

'''' <summary>
'''' Создать Salt / IV Key
'''' </summary>
'''' <param name="CurrentKeyFile"></param>
'Private Sub CreateKey(ByVal CurrentKeyFile As String)
'    Try
'        If PasswordIsValid() Then
'            Password = strRoot
'        Else
'            Exit Sub
'        End If

'        If CreateSaltIVFile(CurrentKeyFile) Then
'            MessageBox.Show("Salt и IV успешно сгенерирован и сохранен .dat " & vbCrLf &
'                "файле в Visual Studio .NET Solution корневой директории.", "CreateKey", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If
'    Catch exp As Exception
'        MessageBox.Show(exp.Message, "CreateKey", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try
'End Sub

'Public Property SourceFileName() As String
'    Get
'        Return strSourceFile
'    End Get
'    Set(ByVal Value As String)
'        If File.Exists(Value) Then
'            strSourceFile = Value
'        Else
'            Throw New FileNotFoundException(Value & " не существует.")
'        End If
'    End Set
'End Property

'''' <summary>
'''' Текущий
'''' </summary>
'''' <returns></returns>
'Public ReadOnly Property Current() As String
'    Get
'        Return SystemBiosDate & SystemBiosVersion & VideoBiosDate & VideoBiosVersion
'    End Get
'End Property

'''' <summary>
'''' Создание .dat файла содержащего salt и IV.
'''' </summary>
'''' <param name="strSaveToPath"></param>
'''' <returns></returns>
'Private Function CreateSaltIVFile(ByVal strSaveToPath As String) As Boolean
'    ' инициализировать байтового массива подходящего размера
'    ReDimByteArrays()

'    ' создать Filestream object для записи salt и IV в файл.
'    Dim fsKey As New FileStream(strSaveToPath, FileMode.OpenOrCreate,
'        FileAccess.Write)

'    ' Генерация случайного "salt" значения. Эти случайные биты были добавлены к
'    ' паролю перед key началу. Взлом "Dictionary Attack" намного более затруднительна. 
'    ' Концепция подобно использованию IV.
'    Dim rng As RandomNumberGenerator = RandomNumberGenerator.Create()
'    rng.GetBytes(abytSalt)

'    'Dim pdb As New PasswordDeriveBytes(strPassword, abytSalt)
'    Dim k2 As New Rfc2898DeriveBytes(strPassword, abytSalt)
'    ' Получить то же самое количество байтов, как поток abytKey длиной, как набор в
'    ' ReDimByteArrays().
'    'abytKey = pdb.GetBytes(abytKey.Length)
'    abytKey = k2.GetBytes(abytKey.Length)

'    ' Генерировать новый случайный IV.
'    crpSym.GenerateIV()
'    abytIV = crpSym.IV

'    Try
'        fsKey.Write(abytSalt, 0, abytSalt.Length)
'        fsKey.Write(abytIV, 0, abytIV.Length)
'        strSaltIVFile = strSaveToPath
'        Return True
'    Catch exp As Exception
'        Throw New Exception(exp.Message)
'    Finally
'        fsKey.Close()
'    End Try
'End Function

'''' <summary>
'''' Расшифровать файл.
'''' </summary>
'Private Sub DecryptFile()
'    ' Если пароль - пустая строка, предполагается, что пользователь не установил 
'    ' "Advanced" CheckBox или не ввел пароль и таким образом не использует
'    ' полученный паролем ключ. В таком случае симметричный объект алгоритма 
'    ' будет только использовать его значение по умолчанию.
'    If strPassword <> "" Then
'        OpenSaltIVFileAndSetKeyIV()
'    End If

'    ' Создать FileStream для чтения назад зашифрованного файла.
'    Dim fsCipherText As New FileStream(strSourceFile, FileMode.Open, FileAccess.Read)
'    ' Создать FileStream для записи временного файла.
'    Dim fsPlainText As New FileStream("temp.dat", FileMode.Create, FileAccess.Write)

'    ' Чтение зашифрованного файла и расшифровка.
'    Dim csDecrypted As New CryptoStream(fsCipherText, crpSym.CreateDecryptor(), CryptoStreamMode.Read)
'    ' Создать StreamWriter для записи временного файла.
'    Dim swWriter As New StreamWriter(fsPlainText)
'    ' Чтение расшифрованного потока в StreamReader.
'    Dim srReader As New StreamReader(csDecrypted)

'    Try
'        ' Запись расшифрованного потока.
'        swWriter.Write(srReader.ReadToEnd)
'    Catch expCrypto As CryptographicException
'        Throw New CryptographicException
'    Finally
'        ' Очистка и закрытие в конце.
'        swWriter.Close()
'        csDecrypted.Close()
'    End Try

'    SwapFiles(True)
'End Sub

'''' <summary>
'''' Зашифровка файла.
'''' </summary>
'Private Sub EncryptFile()
'    ' Если пароль - пустая строка, предполагают, что пользователь не установил 
'    ' "Advanced" CheckBox и таким образом не использует полученный паролем ключ. В таком
'    ' случай симметрический объект алгоритма будет только его значение по умолчанию.

'    If strPassword <> "" Then
'        OpenSaltIVFileAndSetKeyIV()
'    End If

'    ' Создать FileStream для чтения файла источника.
'    Dim fsInput As New FileStream(strSourceFile, FileMode.Open, FileAccess.Read)

'    ' Создать байтовый массив FileStream для чтения файла источника.
'    Dim abytInput(CInt(fsInput.Length - 1)) As Byte
'    fsInput.Read(abytInput, 0, CInt(fsInput.Length))
'    fsInput.Close()

'    ' Создать FileStream для записи временного файла.
'    Dim fsCipherText As New FileStream("temp.dat", FileMode.Create, FileAccess.Write)
'    fsCipherText.SetLength(0)

'    ' Создать Crypto Stream который трансформирует файловый поток используя выбранную шифровку 
'    ' и записать в выходной FileStream объект.
'    Dim csEncrypted As New CryptoStream(fsCipherText, crpSym.CreateEncryptor(),
'        CryptoStreamMode.Write)

'    ' Проход в незашифрованном исходном файле байтовый массив и записывает выход 
'    ' зашифрованные байты к temp.dat файлу. Таким образом, логика работы:
'    ' abytInput----> Encryption----> fsCipherText.

'    csEncrypted.Write(abytInput, 0, abytInput.Length)

'    ' Когда все байты записаны, очистить шифровщик, и таким образом заканчить обработку любых 
'    ' байтов, остающиеся в буфере, используемом потоком шифровщика. Обычно это 
'    ' вызывает дополнение последнего из многократных блоков потока шифровщика.
'    ' Размер блока объекта (для Rijndael это - 16 байтов, или 128 битов), 
'    ' шифровка его, и затем запись этого заключительного блока к потоку памяти.
'    csEncrypted.FlushFinalBlock()

'    ' Очистка. Нет никакой потребности вызвать fsCipherText.Close(), потому что закрытие
'    ' crypto потока автоматически закрывает поток, который туда передавался.
'    csEncrypted.Close()

'    SwapFiles(False)
'End Sub

'''' <summary>
'''' Открытие .dat файла, чтение salt и IV, а затем отправка шифровщику key и IV.
'''' </summary> 
'Private Sub OpenSaltIVFileAndSetKeyIV()
'    ' инициализировать байтового массива подходящего размера
'    ReDimByteArrays()

'    ' создать Filestream объект для чтения содержимого .dat файла содержащего salt и IV.
'    Dim fsKey As New FileStream(strSaltIVFile, FileMode.Open, FileAccess.Read)
'    fsKey.Read(abytSalt, 0, abytSalt.Length)
'    fsKey.Read(abytIV, 0, abytIV.Length)
'    fsKey.Close()

'    ' Получите ключ из salted пароля.
'    'Dim pdb As New PasswordDeriveBytes(strPassword, abytSalt)
'    Dim k2 As New Rfc2898DeriveBytes(strPassword, abytSalt)

'    ' Получите то же самое количество байтов, как поток abytKey, длиной как в наборе ReDimByteArrays ().
'    'abytKey = pdb.GetBytes(abytKey.Length)
'    abytKey = k2.GetBytes(abytKey.Length)

'    ' Если поток crypto класс - TripleDES, проверить, удостовериться что используемый ключ 
'    ' не внесен в список Слабых Ключей (то есть, известные ключи, для успешной атаки).
'    If crpSym.GetType Is GetType(TripleDESCryptoServiceProvider) Then
'        ' Чтобы получить доступ к IsWeakKey методу, привести SymmetricAlgorithm переменный 
'        ' к типу класса TripleDES или TripleDESCryptoServiceProvider.
'        'Dim tdes As TripleDES = CType(crpSym, TripleDES)
'        'if tdes.IsWeakKey(abytKey) Then
'        If TripleDES.IsWeakKey(abytKey) Then
'            Throw New Exception("Текущий ключ внесен в список как Слабый Ключ. " &
'                "Вы должны произвести отличный ключ перед переходом далее.")
'        End If
'    End If

'    ' Назначите множества байта на Key и IV свойств симметричный crypto класс. 
'    crpSym.Key = abytKey
'    crpSym.IV = abytIV
'End Sub

'''' <summary>
'''' Привести к надлежащей длине массивы шифровщика.
'''' </summary>
'Private Sub ReDimByteArrays()
'    ' Для отладки только вывести размеры ключей.
'    Debug.WriteLine(crpSym.GetType.ToString & " legal key sizes in bits:")
'    Dim myKeySizes As KeySizes

'    For Each myKeySizes In crpSym.LegalKeySizes
'        With myKeySizes
'            'Debug.WriteLine("Max=" & .MaxSize & " bits " & _
'            '    "(" & (.MaxSize / 8) & " bytes)")
'            'Debug.WriteLine("Min=" & .MinSize & " bits " & _
'            '   "(" & (.MinSize / 8) & " bytes)")
'            'Debug.WriteLine("Skip=" & .SkipSize & " bits " & _
'            '   "(" & (.SkipSize / 8) & " bytes)")
'        End With
'    Next

'    If crpSym.GetType Is GetType(System.Security.Cryptography.RijndaelManaged) Then
'        ' размер Key массива байта был восстановлен через LegalKeySizes свойство 
'        ' объекта шифровщика. См. Debug.WriteLine выводы на консоль. 
'        ' Принять, что размер массивов - всегда на один больше чем верхнего
'        ' предела, который используется, чтобы инициализировать массив. Так что ReDim_размеры 
'        ' 1 меньше чем key базовый размер, полученные выше.
'        ReDim_abytKey(31)
'        ' Обычное империческое правило salt 1/2 от длины key.
'        ReDim_abytSalt(15)
'        ' Нет никакой "LegalIVSizes" свойства подобно есть для ключевых размеров. 
'        ' Поэтому, чтобы выяснить действительную IVбайтовую длину множества можно сделать
'        ' следующее:
'        '       crpSym.GenerateIV()
'        '       abytIV = crpSym.IV
'        '       Debug.WriteLine("Правильный abytIV.Length=" & abytIV.Length.ToString)
'        ReDim_abytIV(15)
'    Else
'        ReDim_abytKey(23)
'        ReDim_abytSalt(11)
'        ReDim_abytIV(7)
'    End If
'End Sub

'''' <summary>
'''' Эта функция копирует временный файл взамен файла источника
'''' в течение шифрования и декодирования.
'''' </summary>
'''' <param name="UseFileAccessWait"></param>
'Private Sub SwapFiles(ByVal UseFileAccessWait As Boolean)
'    If UseFileAccessWait Then
'        WaitForExclusiveAccess(strSourceFile)
'    End If

'    ' Заменить файл источник временным файлом и удалить временный.
'    File.Copy("temp.dat", strSourceFile, True)
'    File.Delete("temp.dat")
'End Sub

'''' <summary>
'''' Эта функция необходима, чтобы получить доступ к файлу, который был недавно прочитан.
'''' Это используется только после декодирования.
'''' </summary>
'''' <param name="fullPath"></param>
'Private Sub WaitForExclusiveAccess(ByVal fullPath As String)
'    While (True)
'        Try
'            Dim fs As FileStream = New FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.None)
'            fs.Close()
'            Exit Sub
'        Catch e As Exception
'            Thread.Sleep(300)
'        End Try
'    End While
'End Sub

'Public Function CreateFile(ByVal strTemp As String) As Boolean
'    If strTemp = "" Then strTemp = SystemBiosDate & SystemBiosVersion & VideoBiosDate & VideoBiosVersion
'    ' создать StreamWriter объект для записи bios в файл.
'    Dim sw As StreamWriter = New StreamWriter(strSourceFile)

'    Try
'        ' добавить информацию в файл.
'        sw.Write(strTemp)
'        sw.Close()
'        EncryptDecrypt(True)
'        Return True
'    Catch exp As Exception
'        Throw New Exception(exp.Message)
'    Finally
'        sw.Close()
'    End Try
'End Function

'Public Sub EncryptDecrypt(ByVal blnEncrypt As Boolean)
'    Try
'        If IsValid() Then
'            Password = strRoot
'        Else
'            Exit Sub
'        End If

'        If blnEncrypt Then
'            EncryptFile()
'        Else
'            DecryptFile()
'        End If

'        mstrCryptoOutput = ReadFileAsString(strSourceFile)
'    Catch expCrypto As CryptographicException
'        'MsgBox("Файл не может быть правильно расшифрован. Удостоверьтесь, что Вы ввели " & _
'        '    "корректный пароль. " & vbCrLf & "Эта ошибка может также быть вызвана, при изменении " & _
'        '    "типа шифрования между шифрованием и декодированием.", _
'        '    MsgBoxStyle.Critical, "EncryptDecrypt")
'        MessageBox.Show("Некорректная установка программы." & vbCrLf & "Обратитесь к разработчику.", "EncryptDecrypt", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    Catch exp As Exception
'        MessageBox.Show(exp.Message, "EncryptDecrypt", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try
'End Sub

'''' <summary>
'''' Проверка пароля.
'''' Эта функция читает содержимое файла и преобразовывает его к строке.
'''' </summary>
'''' <param name="path"></param>
'''' <returns></returns>
'Private Function ReadFileAsString(ByVal path As String) As String
'    Dim fs As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
'    Dim abyt(CInt(fs.Length - 1)) As Byte

'    fs.Read(abyt, 0, abyt.Length)
'    fs.Close()

'    Return UTF8.GetString(abyt)
'End Function

'''' <summary>
'''' Подтверждение корректных данных.
'''' </summary>
'''' <returns></returns>
'Private Function IsValid() As Boolean
'    If Not PasswordIsValid() Then
'        Return False
'    End If

'    If strSourceFile = "" Then
'        MessageBox.Show("Вы должны сначала загрузить исходный файл!", "IsValid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'        Return False
'    End If

'    Return True
'End Function

'Private Function PasswordIsValid() As Boolean
'    If Not Regex.IsMatch(strRoot, "^\s*(\w){8}\s*$") Then
'        MessageBox.Show("Вы должны ввести 8-digit пароль состоящий из цифр " &
'            "и/или букв.", "PasswordIsValid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'        Return False
'    End If
'    Return True
'End Function

'Private Function IsWindowsNT() As Boolean
'    Dim verinfo As OSVERSIONINFO = Nothing
'    verinfo.dwOSVersionInfoSize = Len(verinfo)
'    If (GetVersionEx(verinfo)) = 0 Then Exit Function
'    If verinfo.dwPlatformId = 2 Then IsWindowsNT = True
'End Function

'Private Function StrFromPtrA(ByVal lpszA As Integer) As String
'    Dim s As String = New String(Chr(0), lstrlenA(lpszA))
'    CopyStringA(s, lpszA)
'    StrFromPtrA = TrimNULL(s)
'End Function

'Private Function TrimNULL(ByVal str_Renamed As String) As String
'    If InStr(str_Renamed, Chr(0)) > 0 Then
'        TrimNULL = Left(str_Renamed, InStr(str_Renamed, Chr(0)) - 1)
'    Else
'        TrimNULL = str_Renamed
'    End If
'End Function

'Private Function GetRegistryValue(ByVal hKey As Integer, ByVal KeyName As String, ByVal ValueName As String, Optional ByRef DefaultValue As String = Nothing) As String 'Object
'    Dim handle As Integer
'    Dim resLong As Integer
'    Dim resString As String
'    Dim resBinary() As Byte
'    Dim length As Integer = MAX_SIZE
'    Dim retVal As Integer
'    Dim valueType As Integer

'    Return IIf(IsNothing(DefaultValue), Nothing, DefaultValue)
'    If RegOpenKeyEx(hKey, KeyName, 0, KEY_READ, handle) Then
'        Exit Function
'    End If

'    ReDim_resBinary(length - 1)
'    'retVal = RegQueryValueEx(handle, ValueName, 0, valueType, resBinary(0), length)
'    retVal = RegQueryValueEx(handle, ValueName, 0, valueType, resBinary, length)

'    If retVal = ERROR_MORE_DATA Then
'        ReDim_resBinary(length - 1)
'        'retVal = RegQueryValueEx(handle, ValueName, 0, valueType, resBinary(0), length)
'        retVal = RegQueryValueEx(handle, ValueName, 0, valueType, resBinary, length)
'    End If

'    Select Case valueType
'        Case REG_DWORD
'            'CopyMemory(resLong, resBinary(0), 4)
'            CopyMemory(resLong, resBinary, 4)
'            Return resLong.ToString
'            Exit Select
'        Case REG_SZ, REG_EXPAND_SZ
'            resString = Space(length - 1)
'            'CopyMemory(resString, resBinary(0), length - 1)
'            CopyMemory(resString, resBinary, length - 1)
'            Return resString
'            Exit Select
'        Case REG_BINARY
'            If length <> UBound(resBinary) + 1 Then
'                ReDimPreserve resBinary(length - 1)
'            End If
'            'GetRegistryValue = VB6.CopyArray(resBinary).ToString 'VB6.CopyArray(resBinary)
'            Return resBinary.ToString
'            Exit Select
'        Case REG_MULTI_SZ
'            resString = Space(length - 2)
'            'CopyMemory(resString, resBinary(0), length - 2)
'            CopyMemory(resString, resBinary, length - 2)
'            Return resString
'            Exit Select
'    End Select

'    RegCloseKey(handle)
'End Function

'Private ReadOnly Property VideoBiosDate() As String
'    Get
'        If isNT Then
'            Return GetRegistryValue(HKLM, "Hardware\Description\System", "VideoBiosDate", "")
'        Else
'            '       VideoBiosDate = Mid(StrFromPtrA(&HC00A8), 1, 8) '-Date build
'            Return Mid(StrFromPtrA(&HC00A8), 9, 8) '-Date revision
'        End If
'    End Get
'End Property

'Private ReadOnly Property VideoBiosVersion() As String
'    Get
'        Dim s As String
'        If isNT Then
'            Dim strGetRegistryValue As String = GetRegistryValue(HKLM, "Hardware\Description\System", "VideoBiosVersion", "")

'            Dim split As String() = strGetRegistryValue.Split(New [Char]() {" "c, CChar(vbTab)}) ' {" "c, ","c, "."c, ":"c, CChar(vbTab)})

'            'For Each sTmp As String In split
'            '    If sTmp.Trim() <> "" Then
'            '        Console.WriteLine(sTmp)
'            '    End If
'            'Next sTmp

'            If split.Length > 1 Then
'                Return $"{split(0)} {split(1)}"
'            Else
'                Return ""
'            End If
'        Else
'            s = StrFromPtrA(&HC0048)
'            s = Left(s, InStr(1, s, vbCrLf) - 1)
'            Return $"{s}{vbCrLf}ChipType: {GetRegistryValue(HKLM, "System\CurrentControlSet\Services\Class\Display\0000\INFO", "ChipType", "")}"
'        End If
'    End Get
'End Property

'Private ReadOnly Property SystemBiosDate() As String
'    Get
'        If isNT Then
'            Return GetRegistryValue(HKLM, "Hardware\Description\System", "SystemBiosDate", "")
'        Else
'            Return StrFromPtrA(&HFFFF5)
'        End If
'    End Get
'End Property

'Private ReadOnly Property SystemBiosVersion() As String
'    Get
'        If isNT Then
'            Return GetRegistryValue(HKLM, "Hardware\Description\System", "SystemBiosVersion", "")
'        Else
'            Return StrFromPtrA(&HFE061)
'        End If
'    End Get
'End Property
'End Class
