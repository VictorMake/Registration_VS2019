Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Text

''' <summary>
''' Работа с файловой системой удалённого клиента по FTP протоколу
''' </summary>
''' <remarks></remarks>
Friend Class FtpClient
    Public Enum MethodUpload
        ''' <summary>
        ''' По Частям
        ''' </summary>
        ByPartial
        ''' <summary>
        ''' Целиком
        ''' </summary>
        Fully
    End Enum

    Public Enum MethodRefresh
        ''' <summary>
        ''' Очистить
        ''' </summary>
        Clear
        ''' <summary>
        ''' Снимок
        ''' </summary>
        Snapshot
    End Enum

    Public Enum FTPClientManagerStatus
        Idle ' простой
        Uploading ' закачка
    End Enum

    <SerializableAttribute>
    Public Class FtpClientException
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, inner As Exception)
            MyBase.New(message, inner)
        End Sub
    End Class

    ''' <summary>
    ''' ID  для формата "ftp://192.168.0.98/"
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property HostId() As String
        Get
            Return mIPAddress.IP.ToLower()
        End Get
    End Property

    Private ReadOnly mCompactRioForm As FormCompactRio
    Public Const UserName As String = "Admin"
    Public Const Password As String = ""

    Private ReadOnly mIPAddress As IPAddressTargetCRIO
    Private ReadOnly mHostName As String
    Private ReadOnly isCopyWorkFolder As Boolean
    'Private mUsername As String
    Private ReadOnly mCredential As NetworkCredential
    Private folderTreeView As TreeView

    Public Sub New(IP As IPAddressTargetCRIO, hostName As String, isCopyWorkFolder As Boolean, inCompactRioForm As FormCompactRio) ', credential As NetworkCredential) ', isSecured As Boolean)
        mIPAddress = IP
        mHostName = hostName
        Me.isCopyWorkFolder = isCopyWorkFolder
        mCredential = New NetworkCredential(UserName, Password)
        mCompactRioForm = inCompactRioForm
        'If credential IsNot Nothing Then
        '    mUsername = credential.UserName
        'Else
        '    mUsername = "anonymous"
        'End If
    End Sub

    ''' <summary>
    ''' Создать запрос к Серверу
    ''' </summary>
    ''' <param name="uri"></param>
    ''' <param name="method"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateFtpWebRequest(uri As Uri, method As String) As FtpWebRequest
        Dim ftpclientRequest As FtpWebRequest = DirectCast(WebRequest.Create(uri), FtpWebRequest)
        ftpclientRequest.Proxy = Nothing

        If mCredential IsNot Nothing Then
            ftpclientRequest.Credentials = mCredential
        End If

        ftpclientRequest.Method = method
        ftpclientRequest.KeepAlive = True
        ftpclientRequest.UsePassive = True

        Return ftpclientRequest
    End Function

    Private mStatus As FTPClientManagerStatus

    ''' <summary>
    ''' Получить или установить статус данного FTPClient.
    ''' </summary>
    Public Property Status() As FTPClientManagerStatus
        Get
            Return mStatus
        End Get

        Private Set(ByVal value As FTPClientManagerStatus)
            If mStatus <> value Then
                mStatus = value
                ' вызвать событие OnStatusChanged
                OnStatusChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event ErrorOccurred As EventHandler(Of ErrorEventArgs)
    Public Event StatusChanged As EventHandler

#Region "Очистка целевой папки и копирование папки с проэктом из Хоста на Шасси"
    ''' <summary>
    ''' Этот класс просто расширяет класс TreeNode, добавляя поле FileStruct для поддержки.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DirectoryNode
        Inherits TreeNode

        Public FileStruct As FileStruct
        'Public Property Size As Long

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(text As String)
            MyBase.New(text)
        End Sub
    End Class

    ''' <summary>
    ''' Очищает рабочую директорию на целевом шасси.
    ''' </summary>
    ''' <param name="ClearFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearStartupFolder(clearFile As MethodRefresh) As Boolean
        folderTreeView = New TreeView() With {.Name = "FolderListView", .PathSeparator = "/"}
        Dim isFolderFindOut As Boolean = False
        Dim success As Boolean

        Try
            ' Вначале проверим что папка есть
            Dim uri As New Uri($"ftp://{ mIPAddress.IP}/ni-rt/")
            Dim DirList As FileStruct() = GetDirectoryList(uri.AbsolutePath)

            If DirList IsNot Nothing Then
                For Each fs As FileStruct In DirList
                    If fs.Name = STARTUP Then
                        isFolderFindOut = True
                        Exit For
                    End If
                Next
            End If

            If Not isFolderFindOut Then MakeDirectory(NI_RT_STARTUP) ' папки нет, надо создать

            Dim tn As New DirectoryNode($"{mIPAddress.IP}/{NI_RT_STARTUP}")
            'addDummyNode(tn)????
            folderTreeView.Nodes.Add(tn)
            'Dim CurrentNode As TreeNode = FolderListView.Nodes(FolderListView.Nodes.Count - 1)'last node	

            RefreshFolderNode(tn, clearFile)
            'Console.WriteLine("Очистка или снимок папки [ni-rt/startup] на шасси " & Me._HostName & " произведена успешно")
            success = True
        Catch ex As Exception
            Const caption As String = NameOf(ClearStartupFolder)
            Dim text As String = $"Очистка или снимок папки <{NI_RT_STARTUP}> на шасси: <{mHostName}>"
            MessageBox.Show(ex.Message, text)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Копирование рабочих проектов в папку на целевом устройстве
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyFilesToDevice() As Boolean
        Dim success As Boolean

        Try
            If isCopyWorkFolder Then
                If ClearStartupFolder(MethodRefresh.Clear) Then
                    Dim itemChassis As Chassis = mCompactRioForm.ManagerChassis.Chassis(mHostName)
                    Dim diSource As DirectoryInfo() = New DirectoryInfo(itemChassis.FolderName).GetDirectories(C_NI_RT_STARTUP) '"c\ni-rt\startup"

                    If diSource.Count > 0 Then
                        CopyAll(diSource(0), NI_RT_STARTUP)
                    Else
                        Throw New FtpClientException($"Не найдена директория <{C_NI_RT_STARTUP}> в каталоге для скачивания на шасси")
                    End If

                    ' если есть файлы для заливки на FPGA
                    If itemChassis.IsContainsFPGABitfiles Then
                        diSource = New DirectoryInfo(itemChassis.FolderName).GetDirectories(FPGA_BITFILES)
                        If diSource.Count > 0 Then
                            ' приблуда для создания каталога (он копируется из другой папки [FPGA Bitfiles] а не из [c\ni-rt\]
                            MakeDirectory(NI_RT_STARTUP & "/" & FPGA_BITFILES)
                            CopyAll(diSource(0), NI_RT_STARTUP & "/" & FPGA_BITFILES)
                        Else
                            Throw New FtpClientException("Не найдена директория [c\FPGA Bitfiles] в каталоге для скачивания на шасси")
                        End If
                    End If
                End If
                ' если потребуется снимок скопированного каталога то разкоментировать
                ' ClearStartupFolder(MethodRefresh.Snapshot)
            Else
                ' копировать только файл AttributeChannels.csv, Channels.csv, ChassisOption.ini
                CopySelectFile(New List(Of String) From {ATTRIBUTE_CHANNELS_FILE_CSV, CHANNELS_FILE_CSV, CHASSIS_OPTION_INI})
            End If

            'CopyNi_Rt_ini() 'TODO: убрал копирование "c\ni-rt\startup\ni-rt.ini"  копируется всегда
            success = True
        Catch ex As Exception
            Const caption As String = NameOf(CopyFilesToDevice)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    '' Копирует все файлы из директории source в директорию target обычная версия
    'Sub CopyAll(ByVal source As DirectoryInfo, ByVal target As DirectoryInfo)
    '    'Проверить что текущая директория создана, и если нет, то создать
    '    If Directory.Exists(target.FullName) = False Then
    '        Directory.CreateDirectory(target.FullName)
    '    End If

    '    'Копировать каждый файл в новую директория
    '    For Each fi As FileInfo In source.GetFiles()
    '        Console.WriteLine("Copying {0}\{1}", target.FullName, fi.Name)
    '        fi.CopyTo(Path.Combine(target.ToString(), fi.Name), True)
    '    Next

    '    'для каждой поддиректории использовать рекурсию
    '    For Each diSourceSubDir As DirectoryInfo In source.GetDirectories()
    '        Dim nextTargetSubDir As DirectoryInfo = target.CreateSubdirectory(diSourceSubDir.Name)
    '        CopyAll(diSourceSubDir, nextTargetSubDir)
    '    Next
    'End Sub

    ''' <summary>
    ''' Копирует все файлы из директории source в директорию targetDirectoryFTP на cRio по FTP.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="targetDirectoryFTP"></param>
    ''' <remarks></remarks>
    Public Sub CopyAll(source As DirectoryInfo, targetDirectoryFTP As String)
        targetDirectoryFTP = targetDirectoryFTP.Replace("\", "/")

        ' Проверить что текущая директория создана, и если нет, то создать
        'Dim uri As New Uri("ftp://" & _IPAddress.IP & "/" & targetDirectoryFTP)
        'Dim DirList As FileStruct() = GetDirectoryList(uri.AbsolutePath)
        'If DirList Is Nothing Then
        '    MakeDirectory(targetDirectoryFTP)
        'End If

        ' Копировать каждый файл в новую директория
        For Each fi As FileInfo In source.GetFiles()
            'Console.WriteLine("Копирование {0}/{1} на шасси {2}", targetDirectoryFTP, fi.Name, Me._HostName)
            Upload(fi.FullName, New Uri($"ftp://{mIPAddress.IP}/{Path.Combine(targetDirectoryFTP, fi.Name)}"), MethodUpload.ByPartial)
        Next

        ' для каждой поддиректории использовать рекурсию
        For Each diSourceSubDir As DirectoryInfo In source.GetDirectories()
            MakeDirectory(Path.Combine(targetDirectoryFTP, diSourceSubDir.Name))
            CopyAll(diSourceSubDir, Path.Combine(targetDirectoryFTP, diSourceSubDir.Name))
        Next
    End Sub

    ''' <summary>
    ''' Копировать талько необходимые файлы инициализации и конфигурации.
    ''' </summary>
    ''' <param name="listFile"></param>
    ''' <remarks></remarks>
    Private Sub CopySelectFile(listFile As List(Of String))
        Dim itemChassis As Chassis = mCompactRioForm.ManagerChassis.Chassis(mHostName)

        For Each strFile In listFile
            Dim fi As New FileInfo(Path.Combine($"{itemChassis.FolderName}\{ C_NI_RT_STARTUP}", strFile))
            Upload(fi.FullName, New Uri($"ftp://{mIPAddress.IP}/{Path.Combine(NI_RT_STARTUP, fi.Name)}"), MethodUpload.ByPartial)
        Next
    End Sub

    ''' <summary>
    ''' Копировать файл инициализации с шасси в директорию для скачивания
    ''' модифицировать и скопировать назад.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CopyNi_Rt_ini()
        ' Для автозапуска программ в файле ni-rt.ini в каталоге c cRio в группе [LVRT] должен быть аттрибут RTTarget.LaunchAppAtBoot=True
        Dim itemChassis As Chassis = mCompactRioForm.ManagerChassis.Chassis(mHostName)
        Dim fi As New FileInfo(Path.Combine(itemChassis.FolderName & "\", NI_RT_INI))

        Download(NI_RT_INI, fi.FullName)
        If fi.Exists Then
            WriteINI(fi.FullName, "LVRT", "RTTarget.LaunchAppAtBoot", "True")
            Upload(fi.FullName, New Uri($"ftp://{mIPAddress.IP}/{ fi.Name}"), MethodUpload.ByPartial)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Эта функция возвращает размер каталога и все его подкаталоги.
    ' ''' обычная версия
    ' ''' </summary>
    'Public Function GetDirectorySize(ByVal strDirPath As String, ByVal dnDriveOrDirectory As DirectoryNode) As Long
    '    Try
    '        Dim astrSubDirectories As String() = Directory.GetDirectories(strDirPath)
    '        Dim strSubDirectory As String

    '        ' Размер текущего каталога зависит от размера 
    '        ' подкаталогов в массиве astrSubDirectories. Поэтому просмотреть
    '        ' все элементы массива и использовать рекурсию, чтобы получить общий
    '        ' размер текущего каталога и всех его подкаталогов.
    '        For Each strSubDirectory In astrSubDirectories
    '            Dim dnSubDirectoryNode As DirectoryNode
    '            dnSubDirectoryNode = New DirectoryNode()

    '            ' Задайте как текст узла только последнюю часть полного пути.
    '            dnSubDirectoryNode.Text = strSubDirectory.Remove(0, strSubDirectory.LastIndexOf("\") + 1)

    '            ' следующая строка является рекурсивной.
    '            dnDriveOrDirectory.Size += GetDirectorySize(strSubDirectory, dnSubDirectoryNode)
    '            dnDriveOrDirectory.Nodes.Add(dnSubDirectoryNode)
    '        Next

    '        ' Добавить в приведенное выше вычисление размера все файлы текущего 
    '        ' каталога.
    '        Dim astrFiles As String() = Directory.GetFiles(strDirPath)
    '        Dim strFileName As String
    '        Dim Size As Long = 0

    '        For Each strFileName In astrFiles
    '            dnDriveOrDirectory.Size += New FileInfo(strFileName).Length
    '        Next
    '        ' Задать цвет узла дерева в зависимости от общего вычисленного размера.
    '        'dnDriveOrDirectory.ForeColor = GetSizeColor(dnDriveOrDirectory.Size)
    '    Catch exc As Exception
    '        ' Не нужно ничего делать. Просто пропустить все каталоги, которые не читаются. Управление
    '        ' передается первой строке после End Try.
    '    End Try

    '    ' Вернуть общий размер для этого каталога.
    '    Return dnDriveOrDirectory.Size
    'End Function

    ''' <summary>
    ''' Рекурсивно удаляет всё содержимое или делает снимок состава каталога 
    ''' после его копирования из Хоста на шасси в переменной FolderTreeView
    ''' в зависимости от типа переменной ClearFile.
    ''' </summary>
    ''' <param name="baseNode"></param>
    ''' <param name="ClearFile"></param>
    ''' <remarks></remarks>
    Private Sub RefreshFolderNode(baseNode As DirectoryNode, clearFile As MethodRefresh) ' TreeNode)
        Dim uri As New Uri($"ftp://{baseNode.FullPath}/")
        Dim dirList As FileStruct() = GetDirectoryList(uri.AbsolutePath)

        'SyncLock Me.FolderListView
        baseNode.Nodes.Clear()

        If dirList IsNot Nothing Then
            For Each fs As FileStruct In dirList
                Dim tn As New DirectoryNode(fs.Name) 'TreeNode(fs.Name)

                tn.FileStruct.Flags = fs.Flags
                tn.FileStruct.Owner = fs.Owner
                tn.FileStruct.IsDirectory = True
                tn.FileStruct.CreateTime = fs.CreateTime
                tn.FileStruct.Name = fs.Name
                'addDummyNode(tn)

                baseNode.Nodes.Add(tn)
                RefreshFolderNode(tn, clearFile)
                ' удалить папку
                If clearFile = MethodRefresh.Clear Then RemoveDirectory(New Uri($"ftp://{baseNode.FullPath}/{fs.Name}"))
            Next
        End If
        'End SyncLock

        Dim fileList As FileStruct() = GetFileList(uri.AbsolutePath)

        If fileList IsNot Nothing Then
            For Each fs As FileStruct In fileList
                Dim tn As New DirectoryNode(fs.Name) ' TreeNode(fs.Name)

                tn.FileStruct.Flags = fs.Flags
                tn.FileStruct.Owner = fs.Owner
                tn.FileStruct.IsDirectory = False
                tn.FileStruct.CreateTime = fs.CreateTime
                tn.FileStruct.Name = fs.Name

                baseNode.Nodes.Add(tn)
                ' удалить файл
                If clearFile = MethodRefresh.Clear Then DeleteFile(New Uri($"ftp://{baseNode.FullPath}/{fs.Name}"))
            Next
        End If
        'End SyncLock
    End Sub

    'Private Sub addDummyNode(baseNode As DirectoryNode)
    '    Dim dummy As New TreeNode()
    '    baseNode.Nodes.Add(dummy)
    'End Sub
#End Region

#Region "Get Directory File"
    ''' <summary>
    ''' Запрос состава папок в директории.
    ''' </summary>
    ''' <param name="directoryPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDirectoryList(directoryPath As String) As FileStruct()
        Return GetDirectoryParser(directoryPath).Directories
    End Function

    ''' <summary>
    ''' Запрос состава файлов в папке.
    ''' </summary>
    ''' <param name="directoryPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileList(directoryPath As String) As FileStruct()
        Return GetDirectoryParser(directoryPath).Files
    End Function

    Private Function GetDirectoryParser(directoryPath As String) As DirectoryListParser
        ' для корневой директории передать directoryPath = ""
        Dim request As FtpWebRequest = CreateFtpWebRequest(New Uri($"ftp://{mIPAddress.IP}/{directoryPath}"), WebRequestMethods.Ftp.ListDirectoryDetails)
        Dim response As WebResponse = request.GetResponse()
        Dim sr As New StreamReader(response.GetResponseStream(), Encoding.ASCII)
        Dim dataString As String = sr.ReadToEnd()
        response.Close()
        Dim parser As New DirectoryListParser(dataString)
        Return parser
    End Function

    ''' <summary>
    ''' Просмотр содержимого каталога по протоколу FTP.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GetListDirectoryDetails(directoryPath As String)
        Const caption As String = NameOf(GetListDirectoryDetails)
        Dim text As String
        ' Получить объект, используемый для связи с сервером
        Dim reader As StreamReader = Nothing

        Try
            Dim listRequest As FtpWebRequest = DirectCast(WebRequest.Create(New Uri($"ftp://{mIPAddress.IP}/{directoryPath}")), FtpWebRequest)
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails
            ' This assumes the FTP site uses anonymous logon.
            listRequest.Credentials = New NetworkCredential(UserName, Password) '("anonymous", "janeDoe@contoso.com")

            Dim listResponse As FtpWebResponse = DirectCast(listRequest.GetResponse(), FtpWebResponse)

            'Dim responseStream As Stream = listResponse.GetResponseStream()
            'reader = New StreamReader(responseStream)
            reader = New StreamReader(listResponse.GetResponseStream())

            'Console.WriteLine(reader.ReadToEnd())
            'Console.WriteLine("Лист состава директории получен, status {0} на шасси {1}", listResponse.StatusDescription, Me._HostName)
            listResponse.Close()
        Catch ex As UriFormatException
            text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
        Catch ex As WebException
            text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            If reader IsNot Nothing Then reader.Close()
        End Try
    End Sub
#End Region

#Region "Download"
    ''' <summary>
    ''' Загрузить файл с FTP-сервера.
    ''' </summary>
    ''' <param name="sourceUri"></param>
    ''' <param name="destinationFile"></param>
    ''' <remarks></remarks>
    Public Sub Download(sourceUri As String, destinationFile As String)
        Const caption As String = NameOf(Download)
        Dim text As String

        Dim responseStream As Stream = Nothing
        Dim fileStream As FileStream = Nothing
        'Dim reader As StreamReader = Nothing
        Dim mFile As New FileInfo(destinationFile)

        If mFile.Directory.Exists Then
            Try
                ' получить объект используя соединение с Сервером
                'Dim downloadUrl As String = FtpChassisAdress & SenderPathFile
                'Dim downloadRequest As FtpWebRequest = DirectCast(WebRequest.Create(downloadUrl), FtpWebRequest)
                'downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile
                '' подразумевается FTP анонимный вход пользователя.
                'downloadRequest.Credentials = New NetworkCredential(UserName, Password) '("anonymous", "myName@contoso.com")

                Dim downloadUrl As New Uri($"ftp://{mIPAddress.IP}/{sourceUri}")
                Dim downloadRequest As FtpWebRequest = CreateFtpWebRequest(downloadUrl, WebRequestMethods.Ftp.DownloadFile)
                Dim downloadResponse As FtpWebResponse = DirectCast(downloadRequest.GetResponse(), FtpWebResponse)

                responseStream = downloadResponse.GetResponseStream()
                fileStream = New FileStream(destinationFile, FileMode.Create, FileAccess.Write)
                CopyDataToDestination(responseStream, fileStream)

                ' далее закоментировал для разбора

                'Dim fileName As String = Path.GetFileName(downloadRequest.RequestUri.AbsolutePath)
                'If fileName.Length = 0 Then
                '    reader = New StreamReader(responseStream, Encoding.GetEncoding("windows-1251")) ' System.Text.Encoding.ASCII)
                '    Dim strText As String = reader.ReadToEnd()
                '    'Console.WriteLine(reader.ReadToEnd())
                '    Console.WriteLine(strText)
                '    Console.WriteLine("Чтение завершено, status {0}", downloadResponse.StatusDescription)
                'Else
                '    fileStream = File.Create(fileName)
                '    Dim buffer(1024) As Byte
                '    Dim bytesRead As Integer
                '    While True
                '        bytesRead = responseStream.Read(buffer, 0, buffer.Length)
                '        If bytesRead = 0 Then
                '            Exit While
                '        End If
                '        fileStream.Write(buffer, 0, bytesRead)
                '    End While
                'End If
                'Console.WriteLine("Загрузить файл {0} с FTP-сервера {1} завершена", destinationFile, Me._HostName)

            Catch ex As UriFormatException
                text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
                mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
            Catch ex As WebException
                text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
                mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
            Catch ex As IOException
                text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
                mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
            Finally
                'If reader IsNot Nothing Then reader.Close()
                If responseStream IsNot Nothing Then
                    responseStream.Close()
                    'downloadResponse.Close()
                End If

                If fileStream IsNot Nothing Then fileStream.Close()
            End Try
        Else
            text = $"Директория <{mFile.DirectoryName}> не существует!"
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End If
    End Sub

    'Public Sub DownloadFile(sourceUri As Uri, destinationFile As String)
    '    Dim file As New FileInfo(destinationFile)
    '    If file.Directory.Exists Then
    '        Dim request As FtpWebRequest = CreateFtpWebRequest(sourceUri, WebRequestMethods.Ftp.DownloadFile)
    '        Dim response As WebResponse = request.GetResponse()
    '        Dim responseStream As Stream = response.GetResponseStream()
    '        Dim fileStream As New FileStream(destinationFile, FileMode.Create, FileAccess.Write)
    '        copyDataToDestination(responseStream, fileStream)
    '    Else
    '        MessageBox.Show("Directory " + file.DirectoryName & " doesn't exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '    End If
    'End Sub
#End Region

#Region "Upload"
    ''' <summary>
    ''' Отправить файл на FTP-сервер.
    ''' </summary>
    ''' <param name="sourceFile"></param>
    ''' <param name="destinationPath"></param>
    ''' <param name="method"></param>
    ''' <remarks></remarks>
    Public Sub Upload(sourceFile As String, destinationPath As Uri, method As MethodUpload)
        Const caption As String = NameOf(Upload)
        Dim text As String
        Dim requestStream As Stream = Nothing
        Dim fileStream As FileStream = Nothing
        Dim uploadResponse As FtpWebResponse = Nothing

        Try
            'Dim _file As New FileInfo(sourceFile)
            ' получить объект используя соединение с Сервером
            ' Чтобы получить экземпляр FtpWebRequest, использовать метод Create.
            ' Можно также использовать класс WebClient для загрузки сведений с FTP-сервера и выгрузки сведений на него.
            'Dim uploadUrl As New Uri("ftp://" & _hostName & DestinationPathFile)
            'Dim uploadUrl As New Uri((destinationPath.AbsoluteUri & "/") + _file.Name)

            'Dim uploadRequest As FtpWebRequest = DirectCast(WebRequest.Create(uploadUrl), FtpWebRequest)
            'uploadRequest.Method = WebRequestMethods.Ftp.UploadFile
            '' UploadFile не поддерживается через Http proxy
            '' таким образом отключить proxy для этого запроса.
            'uploadRequest.Proxy = Nothing
            '' подразумевается FTP анонимный вход пользователя.
            'uploadRequest.Credentials = New NetworkCredential(UserName, Password) '("anonymous", "janeDoe@contoso.com")

            Dim uploadRequest As FtpWebRequest = CreateFtpWebRequest(destinationPath, WebRequestMethods.Ftp.UploadFile)

            Select Case method
                Case MethodUpload.ByPartial
                    requestStream = uploadRequest.GetRequestStream()
                    fileStream = File.Open(sourceFile, FileMode.Open, FileAccess.Read)

                    Dim buffer(1024) As Byte
                    Dim bytesRead As Integer

                    While True
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length)
                        If bytesRead = 0 Then
                            Exit While
                        End If
                        requestStream.Write(buffer, 0, bytesRead)
                    End While
                    Exit Select
                Case MethodUpload.Fully
                    ' Копирование содержимого файла требует создание потока.
                    'Dim sourceStream As New StreamReader(SenderPathFile) 
                    'Dim fileContents As Byte() = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd()) 'перевести в ASCII
                    'sourceStream.Close()

                    'Using sr As StreamReader = New StreamReader(SenderPathFile)
                    '    Text = sr.ReadToEnd
                    'End Using

                    '*******************************
                    ' это работает
                    'Dim Text As String = "Тест Test"
                    'Dim fileContents As Byte() = ConvertUnicodeToSCIIByte(Text) 
                    '*******************************

                    ' если раскоментировать и посмотреть файл в неправильной кодировке
                    'Dim Text As String = My.Computer.FileSystem.ReadAllText(SenderPathFile)

                    ' считать файл в правильной кодировке и перевести в байты
                    'Dim encoding__1 As Encoding = Encoding.GetEncoding("windows-1251")
                    Dim fileContents As Byte() = File.ReadAllBytes(sourceFile)

                    '' Некоторое содержимое закодировано как UTF-16
                    '' чтобы востановить строку в unicode
                    'Dim unicodeValues As String = encoding__1.GetString(fileContents)
                    '' перевести в байты: fileContents равен fileContents2
                    'Dim fileContents2 As Byte() = ConvertUnicodeToSCIIByte(unicodeValues)

                    uploadRequest.ContentLength = fileContents.Length
                    requestStream.Write(fileContents, 0, fileContents.Length)

                    Exit Select
            End Select
            ' Поток запроса обязан быть закрыт прежде получения ответа
            requestStream.Close()

            ' При использовании объекта FtpWebRequest для выгрузки файла на сервер, необходимо записать содержимое файла в поток запроса, 
            ' полученный путем вызова метода GetRequestStream или его асинхронных аналогов BeginGetRequestStream и EndGetRequestStream. 
            ' Необходимо произвести запись в этот поток и закрыть поток перед отправкой запроса.

            ' Запросы отправляются на сервер посредством вызова метода GetResponse или его асинхронных аналогов BeginGetResponse и EndGetResponse. 
            ' После завершения запрошенной операции возвращается объект FtpWebResponse. Объект FtpWebResponse предоставляет сведения о состоянии операции, а также любые данные, загруженные с сервера. 
            uploadResponse = DirectCast(uploadRequest.GetResponse(), FtpWebResponse)
            'Console.WriteLine("Передача файла {0} по FTP на шасси {1} завершена, status {2}", destinationPath, Me._HostName, uploadResponse.StatusDescription)
        Catch ex As UriFormatException
            text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
        Catch ex As IOException
            text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Catch ex As WebException
            text = $"На шасси: <{mHostName}> ошибка: {ex.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        Finally
            If uploadResponse IsNot Nothing Then uploadResponse.Close()
            If fileStream IsNot Nothing Then fileStream.Close()
            If requestStream IsNot Nothing Then requestStream.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Копирование из одного бинарного потока в другой.
    ''' </summary>
    ''' <param name="sourceStream"></param>
    ''' <param name="destinationStream"></param>
    ''' <remarks></remarks>
    Private Sub CopyDataToDestination(sourceStream As Stream, destinationStream As Stream)
        Dim buffer As Byte() = New Byte(1023) {}
        Dim bytesRead As Integer = sourceStream.Read(buffer, 0, buffer.Length)
        While bytesRead <> 0
            destinationStream.Write(buffer, 0, bytesRead)
            bytesRead = sourceStream.Read(buffer, 0, buffer.Length)
        End While
        destinationStream.Close()
        sourceStream.Close()
    End Sub

    'Public Sub UploadFile(sourceFile As String, destinationPath As Uri)
    '    Try
    '        Dim file As New FileInfo(sourceFile)
    '        Dim uri As New Uri((destinationPath.AbsoluteUri & "/") + file.Name)
    '        Dim request As FtpWebRequest = CreateFtpWebRequest(uri, WebRequestMethods.Ftp.UploadFile)
    '        Dim requestStream As Stream = request.GetRequestStream()
    '        Dim fileStream As New FileStream(sourceFile, FileMode.Open, FileAccess.Read)
    '        copyDataToDestination(fileStream, requestStream)
    '        Dim response As WebResponse = request.GetResponse()
    '        response.Close()
    '    Catch e As WebException
    '        displayErrorDialog("Fileuploade Error", e.Message)
    '    Catch e As IOException
    '        displayErrorDialog("Fileuploade Error", e.Message)
    '    End Try
    'End Sub

    '' Получает самый последний код состояния, отправленный с FTP-сервера.
    '' Значение, возвращенное свойством StatusCode, включается в свойство StatusDescription. При загрузке данных значение свойства StatusCode меняется вместе с кодами состояния, возвращаемыми FTP-сервером. 
    '' После вызова метода GetResponse свойство StatusCode содержит промежуточный код состояния. При вызове метода Close свойство StatusCode содержит окончательное состояние. 
    'Public Function UploadFileToServer(fileName As String) As Boolean
    '    Dim serverUri As New Uri("ftp://" & _IPAddress.IP & "/" & fileName)

    '    'Dim SenderFullPathFile As String = FtpAdressSSD & fileName

    '    'URI описывает серверный serverUri должен быть в шаблоне ftp:// scheme
    '    'fileName параметр идентифицирует файл содержащий данные для передачи
    '    If serverUri.Scheme <> Uri.UriSchemeFtp Then
    '        Return False
    '    End If
    '    'Получить объект используя соединение с Сервером

    '    Dim request As FtpWebRequest = DirectCast(WebRequest.Create(serverUri), FtpWebRequest)
    '    request.Method = WebRequestMethods.Ftp.UploadFile
    '    'Не устанавливать лимит для окончания операции

    '    request.Timeout = System.Threading.Timeout.Infinite

    '    'Копировать содержимое файла  через поток запроса
    '    Const bufferLength As Integer = 2048
    '    Dim buffer As Byte() = New Byte(bufferLength - 1) {}

    '    Dim count As Integer = 0
    '    Dim readBytes As Integer = 0
    '    Dim stream As FileStream = File.OpenRead(fileName)
    '    Dim requestStream As Stream = request.GetRequestStream()
    '    Do
    '        readBytes = stream.Read(buffer, 0, bufferLength)
    '        requestStream.Write(buffer, 0, bufferLength)
    '        count += readBytes
    '    Loop While readBytes <> 0

    '    'Console.WriteLine("Writing {0} bytes to the stream.", count)
    '    'Необходимо закрыть поток request прежде отправки
    '    requestStream.Close()

    '    Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
    '    'Console.WriteLine("Upload status: {0}, {1}", response.StatusCode, response.StatusDescription)

    '    response.Close()
    '    Return True
    'End Function
#End Region

#Region "MakeDirectory"
    ''' <summary>
    ''' Создание директории на Сервере.
    ''' </summary>
    ''' <param name="fullDirectoryPath"></param>
    ''' <remarks></remarks>
    Public Sub MakeDirectory(fullDirectoryPath As String)
        Dim serverUri As New Uri($"ftp://{mIPAddress.IP}/{fullDirectoryPath}")
        Try
            ' serverUri должен быть начинаться с ftp:// scheme.
            fullDirectoryPath = fullDirectoryPath.Replace("\", "/")

            If serverUri.Scheme = Uri.UriSchemeFtp Then
                ' получить объект используя коммуникацию с Сервером
                ' Dim request As FtpWebRequest = DirectCast(WebRequest.Create(serverUri), FtpWebRequest)
                ' request.KeepAlive = True ' закрывать подключение не надо
                ' request.Method = WebRequestMethods.Ftp.MakeDirectory
                ' request.Credentials = New NetworkCredential(UserName, Password)
                Dim request As FtpWebRequest = CreateFtpWebRequest(serverUri, WebRequestMethods.Ftp.MakeDirectory)
                Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
                ' Console.WriteLine("Директория {0} на шасси {1}  создана. Status: {2}", fullDirectoryPath, Me._HostName, response.StatusDescription)
                response.Close()
            End If
        Catch webEx As WebException
            Const caption As String = NameOf(MakeDirectory)
            Dim text As String = $"Ошибка при создании директории {serverUri}{vbCrLf}{webEx.Message}"
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            ' (550) Файл недоступен (например, не найден или к нему нет доступа). Каталог уже создан, ни чего не делать.
            Dim ftpResponse As FtpWebResponse = TryCast(webEx.Response, FtpWebResponse)
            Dim msg As String = $"Эта ошибка произошла во время создания директории {serverUri.ToString()}. StatusCode: {ftpResponse.StatusCode.ToString()}  StatusDescription: { ftpResponse.StatusDescription}"
            Dim errorException As New ApplicationException(msg, webEx)

            ' вызвать событие ErrorOccurred с описанием ошибки.
            Dim e As ErrorEventArgs = New ErrorEventArgs With {.ErrorException = errorException}
            OnErrorOccurred(e)
        End Try
    End Sub

    'Public Function MakeDirectoryOnServer(serverUri As Uri) As Boolean
    '    'serverUri должен быть начинаться с ftp:// scheme.

    '    If serverUri.Scheme <> Uri.UriSchemeFtp Then
    '        Return False
    '    End If

    '    ' получить объект используя коммуникацию с Сервером
    '    Dim request As FtpWebRequest = DirectCast(WebRequest.Create(serverUri), FtpWebRequest)
    '    request.KeepAlive = True 'закрывать подключение не надо
    '    request.Method = WebRequestMethods.Ftp.MakeDirectory
    '    Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
    '    Console.WriteLine("Status: {0}", response.StatusDescription)
    '    Return True
    'End Function

    'Public Sub MakeDirectory(fullDirectoryPath As String)
    '    Try
    '        Dim uri As New Uri("ftp://" & _hostName & fullDirectoryPath)
    '        Dim request As FtpWebRequest = CreateFtpWebRequest(uri, WebRequestMethods.Ftp.MakeDirectory)
    '        Dim response As WebResponse = request.GetResponse()
    '        response.Close()
    '    Catch e As WebException
    '        displayErrorDialog("MakeDirectory Error", e.Message)
    '    End Try
    'End Sub
#End Region

#Region "DeleteFile RemoveDirectory"
    ''' <summary>
    ''' Удаление директории на Сервере.
    ''' </summary>
    ''' <param name="fullFileUri"></param>
    ''' <remarks></remarks>
    Public Sub DeleteFile(fullFileUri As Uri)
        Try
            Dim request As FtpWebRequest = CreateFtpWebRequest(fullFileUri, WebRequestMethods.Ftp.DeleteFile)
            Dim response As WebResponse = request.GetResponse()
            response.Close()
        Catch webEx As WebException
            Const caption As String = NameOf(DeleteFile)
            Dim text As String = $"Ошибка при удалении директории {fullFileUri}{vbCrLf}{webEx.Message}"
            'MessageBox.Show(text, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            Dim ftpResponse As FtpWebResponse = TryCast(webEx.Response, FtpWebResponse)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            Dim msg As String = $"Эта ошибка произошла во время удаления файла {fullFileUri.ToString()}. StatusCode: {ftpResponse.StatusCode.ToString()} StatusDescription: {ftpResponse.StatusDescription}"
            Dim errorException As New ApplicationException(msg, webEx)
            ' вызвать событие ErrorOccurred с описанием ошибки.
            Dim e As ErrorEventArgs = New ErrorEventArgs With {.ErrorException = errorException}
            OnErrorOccurred(e)
        End Try
    End Sub

    '' Получает текст, описывающий код состояния, отправленный с FTP-сервера.
    '' Текст, возвращенный свойством StatusDescription, включает значение свойства StatusCode, состоящее из трех цифр.
    '' При загрузке данных значение свойства StatusDescription меняется вместе с кодами состояния, возвращаемыми FTP-сервером. 
    '' После вызова метода GetResponse свойство StatusDescription содержит промежуточный код состояния. 
    '' При вызове метода Close свойство StatusDescription содержит окончательное состояние. 
    '' отправляется запрос на удаление файла на FTP-сервере и отображается состояние сообщения из ответа сервера на запрос.
    'Public Function DeleteFileOnServer(serverUri As Uri) As Boolean
    '    If serverUri.Scheme <> Uri.UriSchemeFtp Then
    '        Return False
    '    End If
    '    ' Get the object used to communicate with the server.

    '    Dim request As FtpWebRequest = DirectCast(WebRequest.Create(serverUri), FtpWebRequest)
    '    request.Method = WebRequestMethods.Ftp.DeleteFile

    '    Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
    '    'Console.WriteLine("На шасси {0} файл {1} удалён. Delete status: {2}", Me._HostName, serverUri, response.StatusDescription)
    '    response.Close()
    '    Return True
    'End Function

    Public Sub RemoveDirectory(fullDirectoryPath As Uri)
        Try
            'Dim uri As New Uri("ftp://" & _IPAddress.IP & "/" & fullDirectoryPath)
            Dim request As FtpWebRequest = CreateFtpWebRequest(fullDirectoryPath, WebRequestMethods.Ftp.RemoveDirectory)
            Dim response As WebResponse = request.GetResponse()
            response.Close()
        Catch webEx As WebException
            Const caption As String = NameOf(RemoveDirectory)
            Dim text As String = $"На шасси: <{mHostName}> ошибка: {webEx.Message} во время удаления директории <{fullDirectoryPath.ToString()}>"
            'MessageBox.Show(ex.Message, "RemoveDirectory Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            mCompactRioForm.InvokeAppendMsgToRichTextBoxByKey(text, FormCompactRio.KeyRichTexServer, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

            Dim ftpResponse As FtpWebResponse = TryCast(webEx.Response, FtpWebResponse)
            Dim msg As String = $"Эта ошибка произошла во время удаления директории {fullDirectoryPath.ToString()}. StatusCode: {ftpResponse.StatusCode.ToString()} StatusDescription: {ftpResponse.StatusDescription}"
            Dim errorException As New ApplicationException(msg, webEx)
            ' вызвать событие ErrorOccurred с описанием ошибки.
            Dim e As ErrorEventArgs = New ErrorEventArgs With {.ErrorException = errorException}
            OnErrorOccurred(e)
        End Try
    End Sub
#End Region

    ''' <summary>
    ''' Запись строки в указанный файл.
    ''' </summary>
    ''' <param name="senderString"></param>
    ''' <param name="destinationPathFile"></param>
    ''' <remarks></remarks>
    Public Sub WriteFtpStringStream(senderString As String, destinationPathFile As String)
        ' Получить объект используя коммуникацию с сервером
        Dim destinationFullPathFile As New Uri($"ftp://{mIPAddress.IP}/{destinationPathFile}")
        Dim request As FtpWebRequest = DirectCast(WebRequest.Create(destinationFullPathFile), FtpWebRequest)

        request.Method = WebRequestMethods.Ftp.UploadFile

        ' Это подразумевает что FTP просмотр использует анонимный вход
        request.Credentials = New NetworkCredential(UserName, Password) '("anonymous", "myName@contoso.com")

        Dim fileContents As Byte() = ConvertUnicodeToSCIIByte(senderString)
        request.ContentLength = fileContents.Length

        Dim requestStream As Stream = request.GetRequestStream()
        requestStream.Write(fileContents, 0, fileContents.Length)
        requestStream.Close()

        Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
        response.Close()
    End Sub

    Public Function ConvertUnicodeToSCIIByte(unicodeString As String) As Byte()
        ' Все строчки в .net хранятся в юникоде. 
        ' Надо взять строку, и с помощью метода GetBytes нужной кодировки получить представление строки в байтах для данной кодировки. 
        ' Ну а дальше берется этот массив байт и преобразуется в строку с помощью Encoding.GetString.
        ' Должен быть вызван метод той кодировки, к которой необходимо преобразовать строку.
        ' Создать 2 различные кодировки.
        Dim ascii As Encoding = Encoding.GetEncoding("windows-1251")
        Dim [unicode] As Encoding = Encoding.Unicode

        ' Конвертировать строку в массив byte[].
        Dim unicodeBytes As Byte() = [unicode].GetBytes(unicodeString)

        ' Произвести конвертацию из одной кодировки в другую.
        Dim asciiBytes As Byte() = Encoding.Convert([unicode], ascii, unicodeBytes)

        ' Конвертировать новый byte[] в char[] а затем в строку.
        ' Немного другой подход конвертации  GetCharCount/GetChars.
        Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length) - 1) As Char
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)

        Return asciiBytes
    End Function

    Protected Overridable Sub OnErrorOccurred(ByVal e As ErrorEventArgs)
        Status = FTPClientManagerStatus.Idle

        RaiseEvent ErrorOccurred(Me, e)
    End Sub

    Protected Overridable Sub OnStatusChanged(ByVal e As EventArgs)
        RaiseEvent StatusChanged(Me, e)
    End Sub
End Class

Public Class ErrorEventArgs
    Inherits EventArgs
    Public Property ErrorException() As Exception
End Class