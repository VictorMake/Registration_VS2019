Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports Registration.FormCompactRio

''' <summary>
''' Управляющий класс коллекцией сконфигурированных шасси для ИВК
''' </summary>
''' <remarks></remarks>
Friend Class TargetsChassis
    Implements IEnumerable
    Implements IEnumerable(Of Chassis)

    ' внутренняя коллекция для управления шасси
    Private ReadOnly mChassis As New Dictionary(Of String, Chassis)

#Region "Class ManagerChassis"
    Default Public ReadOnly Property Item(ByVal Key As String) As Chassis
        Get
            Return mChassis.Item(Key)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of Chassis) Implements IEnumerable(Of Chassis).GetEnumerator
        For Each key As String In mChassis.Keys.ToArray
            Yield mChassis(key)
        Next
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator() ' mChassis.GetEnumerator
    End Function

    ' число текущих загруженных форм
    Public ReadOnly Property Count() As Integer
        Get
            Return mChassis.Count
        End Get
    End Property

    Public ReadOnly Property Chassis() As Dictionary(Of String, Chassis)
        Get
            Return mChassis
        End Get
    End Property

    Public Sub Remove(ByRef Key As String) ' Shared убрал 
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mChassis.Remove(Key)
    End Sub

    Public Sub Clear()
        mChassis.Clear()
    End Sub

    ''' <summary>
    ''' Добавление нового шасси из конфигурации целевых устройств
    ''' после проверки корректного содержимого файлов для копирования.
    ''' </summary>
    ''' <param name="newChassis"></param>
    ''' <returns></returns>
    Public Function IsCheckAddNewChassis(ByRef newChassis As Chassis) As Boolean
        Dim success As Boolean = False

        If mChassis.ContainsKey(newChassis.HostName) Then
            MessageBox.Show($"Шасси с именем {newChassis.HostName} уже загружен!", "Загрузка нового шасси", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return success
        End If

        Try
#If DEBUG_ClientTest = True Then
            mChassis.Add(newChassis.HostName, newChassis)
            success = mChassis.ContainsKey(newChassis.HostName)
#Else
            If IsCheckFolderToCopy(newChassis) Then
                mChassis.Add(newChassis.HostName, newChassis)
                success = mChassis.ContainsKey(newChassis.HostName)
            End If
#End If
        Catch exp As Exception
            Dim caption As String = exp.Source
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function
#End Region
    Private ReadOnly mCompactRioForm As FormCompactRio
    Private ReadOnly mOptionData As OptionDataCRIO

    Public Sub New(inCompactRioForm As FormCompactRio, inOptionData As OptionDataCRIO)
        mCompactRioForm = inCompactRioForm
        mOptionData = inOptionData
    End Sub

    ''' <summary>
    ''' Проверка наличия и корректного содержимого папки с программой для копирования на целевое устройство.
    ''' Проверить Состав Папки Для Копирования
    ''' </summary>
    ''' <param name="newChassis"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsCheckFolderToCopy(ByRef newChassis As Chassis) As Boolean
        Dim caption As String = $"Проверить состав папки для копирования в шасси: <{newChassis.HostName}>"
        Dim text As String
        Dim success As Boolean = False
        Dim targetCRIOFolderName As String = newChassis.FolderName

        If Directory.Exists(targetCRIOFolderName) = False Then
            text = $"Папка с программой для шасси: <{newChassis.HostName}> по указанному пути: <{newChassis.FolderName}> отсутствует!{Environment.NewLine}Шасси не будет запущено!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return success
        End If

        Dim chassisFolderName = mChassis.Values.Where(Function(chassis) chassis.FolderName = targetCRIOFolderName)

        If chassisFolderName.Any() Then ' папки Совпадают
            For Each itemChassis As Chassis In chassisFolderName
                text = $"Папка с программой для шасси: <{newChassis.HostName}> совпадает с папкой с программой для шасси: <{itemChassis.HostName}>{Environment.NewLine}Шасси не будет запущено из-за конфликта каналов из двух повторных конфигураций!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Next

            Return success
        End If

        Try
            ' должны присутствовать 2 папки 1)c не обязательно 2)FPGA Bitfiles
            ' в них обязательно должны быть файлы:
            ' каталог FPGA Bitfiles файл ххх.lvbitx для ПЛИС
            ' каталог c\ni-rt\startup  файл startup.rtexe

            ' вызвать метод GetFiles чтобы получить массив файлов в директории
            Dim dirInfo As New DirectoryInfo(newChassis.FolderName)
            Dim isError_FPGA_Bitfiles As Boolean = False

            If Not dirInfo.GetDirectories(NI_RT, SearchOption.AllDirectories).Where(Function(dri) dri.Name = NI_RT).Any() Then ' отсутствует
                text = $"Папка с программой в каталоге: <{NI_RT}> по указанному пути: <{newChassis.FolderName}> отсутствует!{Environment.NewLine}Шасси не будет запущено!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return success
            End If

            For Each dri As DirectoryInfo In dirInfo.GetDirectories(FPGA_BITFILES, SearchOption.AllDirectories)
                If dri.Name = FPGA_BITFILES Then
                    ' вызвать метод GetFiles чтобы получить массив файлов в директории
                    Dim filesCount As Integer = dri.GetFiles(SEARCH_PATTERN_LVBITX, SearchOption.AllDirectories).Count()

                    If filesCount = 0 Then
                        text = $"Хотя каталог: <{FPGA_BITFILES}> присутствует, но в нём не содержится ни одного файла <{SEARCH_PATTERN_LVBITX}>{Environment.NewLine}Шасси не будет запущено до исправления папки!"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        isError_FPGA_Bitfiles = True
                    ElseIf filesCount > 1 Then
                        text = $"В каталоге: <{FPGA_BITFILES}> присутствует более одного файла <{SEARCH_PATTERN_LVBITX}>{Environment.NewLine}Шасси не будет запущено до исправления папки!"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        isError_FPGA_Bitfiles = True
                    Else ' один файл
                        newChassis.IsContainsFPGABitfiles = True
                    End If
                    Exit For
                End If
            Next

            If isError_FPGA_Bitfiles Then Return success
            success = True
        Catch ex As Exception
            text = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Заполнить коллекцию шасси с контролем корректности
    ''' создания вспомогательных файлов.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsLoadChassisSuccess() As Boolean
        Dim success As Boolean

        Try
            For Each itemTargetCRIO As TargetCRIO In mOptionData.CollectionTargetCRIO
                ' при создании автоматом добавляется в коллекцию
                If IsCheckAddNewChassis(New Chassis(itemTargetCRIO.HostName,
                                               itemTargetCRIO.IPAddressRTtarget,
                                               itemTargetCRIO.ModeWork,
                                               itemTargetCRIO.FolderName,
                                               itemTargetCRIO.CopyFolder)) Then ' там проверка на корректность
                    mChassis(itemTargetCRIO.HostName).IndexRow = itemTargetCRIO.IndexRow ' индекс в таблице которая уже создана
                    FillManagerChannels(itemTargetCRIO.HostName)
                End If
            Next

            success = True
        Catch ex As Exception
            Const caption As String = NameOf(IsLoadChassisSuccess)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Заполнение коллекции менеждера каналов в шасси
    ''' </summary>
    ''' <param name="hostName"></param>
    ''' <remarks></remarks>
    Private Sub FillManagerChannels(hostName As String)
        ' разбивка на группы происходит по первым 4 буквам атрибута ChId
        ' Mod1/ai0 to Mod1/ai15
        ' Mod1/ao0 to Mod1/ao15
        ' Mod1/port0/line0 to Mod1/port0/line31
        ' Mod5/ctr0, Mod5/ctr1
        ' Netw - группа для сетевых переменных управления
        ' Comm - группа для сетевой переменной команды внешнего управления 
        ' Имя сетевой переменной в атрибуте Name каналов Сервера и состоит из имени Шасси и имени самой переменной
        ' идентификатор сетевого имени каналов управления типа ИмяШасси:СетевоеИмяКаналаУправления наприммер cRIO-9012-Victor:СетеваяУправления
        ' также сетевой канал несущий признак, что управлением шасси взято извне ИмяШасси:Command наприммер cRIO-9012-Victor:СетеваяКомандная
        ' Пины состоят из (NИВК)(P,T,I,V,D,N,C)(Nшасси в ИВК)(0-100)
        ' NИВК номер ИВК
        ' P давление
        ' T температура
        ' I ток
        ' V ЦАП output
        ' D цифровой
        ' N сетевая управления
        ' C команда


        ' Пример сетевых адресов
        ' \\cRIO-9012\RT Network Communication\activate
        ' \\cRIO-9012\RT Network Communication\loopperiod
        ' \\localhost\my_process\my_variable 
        ' \\test_machine\my_process\my_folder\my_variable 
        ' \\192.168.1.100\my_process\my_variable
        ' Если в системе несколько начальников, команда должна уточнить свое происхождение с помощь уникального идентификатора, чтобы квитанция могла быть направлена соответствующему Начальнику.
        ' Команда создаётся как кластер, содержащий Имя команды(нужно если конанда не именована, а создаётся как обобщённая, для последующего разбора)
        ' Peply подтверждение, перечисление из 
        ' Received: По доставке (Upon Delivery): Цикл-Исполнитель прочитал команду из сети. Команда считается полученной.
        ' Acceted: По приему (Upon Acceptance): Исполнитель оценил команду и определил, принять ее к исполнению или отказаться выполнять.
        ' Completed: По завершению (Upon Completion): Исполнитель отвечает только, когда команда выполнена, или сообщает Начальнику об ошибке при выполнении команды.
        ' Далее 
        ' Parameters - значение параметра, 
        ' логическое OK - потверждение квитирования, 
        ' номер команды - Command#, 
        ' идентификатор начальника Commander ID (может имя компьютера или IP адрес компьютера)
        '
        ' ***************Physical Channel Syntax*****************
        ' Use this syntax to refer to physical channels and groups of physical channels in NI-DAQmx.
        ' Physical Channel Names
        ' Physical channel names consist of a device identifier and a slash (/) followed by a channel identifier. For example, if the physical channel is Dev1/ai1, the device identifier is Dev1, and the channel identifier is ai1. MAX assigns device identifiers to devices in the order they are installed in the system, such as Dev0 and Dev1. You also can assign arbitrary device identifiers with MAX.

        ' For analog I/O and counter I/O, channel identifiers combine the type of the channel, such as analog input (ai), analog output (ao), and counter (ctr), with a channel number such as the following:
        '             ai1
        '             ctr0

        ' For digital I/O, channel identifiers specify a port, which includes all lines within a port:
        '                 port0()
        ' Or, the channel identifier can specify a line within a port:
        ' port0/line1

        ' All lines have a unique identifier. Therefore, you can use lines without specifying which port they belong to. For example, line31—is equivalent to port3/line7 on a device with four 8-bit ports.
        ' Physical Channel Ranges
        ' To specify a range of physical channels, use a colon between two channel numbers or two physical channel names:
        ' Dev1/ai0:4
        ' Dev1/ai0:Dev1/ai4

        ' For digital I/O, you can specify a range of ports with a colon between two port numbers:
        ' Dev1/port0:1

        ' You also can specify a range of lines:
        ' Dev1/port0/line0:4
        ' Dev1/line0:31

        ' You can specify channel ranges in reverse order:
        ' Dev1/ai4:0
        ' Dev1/ai4:Dev1/ai0
        ' Dev1/port1/line3:0

        ' Physical Channel Lists
        ' Use commas to separate physical ch annel names and ranges in a list as follows:
        ' Dev1/ai0, Dev1/ai3:6 
        ' Dev1/port0, Dev1/port1/line0:2
        ' *****************************

        ' сделать выборку  из SSDXXX по шасси по атрибуту Chassis=HostName
        ' записать 3 файла: 1.заголовки полей, 
        ' 2.массив каналов m_Channels разделённых запятыми
        ' 3.файл с описанием типа работы контроллера шасси и адрес системного блока ИВКXXX

        ' Проверка наличие корреспонденции между каналами ИВК и Общими каналами Сервера
        ' Проверка числа каналов для шасси
        ' атрибут Chassis не доложен быть пустым!!!
        ' надо сделать верификацию между атрибутом State с каналах ИВК и каналах Сервера 
        ' (скорее всего  State с каналах ИВК отключает опрос в шасси, а  State с каналах Сервера отключает передачу его клиентам)

        Dim itemChassis As Chassis = mChassis(hostName)
        Dim index As Integer = 1
        Dim channelGroupIndex As Integer = 1
        Dim missingChannels As New List(Of String) ' отсутствующие Каналы
        Dim repeatedChannels As New List(Of String) ' повторяющиеся Каналы

        ' взять коллекцию каналов для "SSD" & myOptionData.НомерИВК
        ' сделать запрос для выборки из списка каналов только для шасси selectHwCh.Chassis = HostName
        ' и группировка этих каналов по модулям <ModX> (Group selectHwCh By Key = selectHwCh.ChId.Substring(0, 4))
        Dim ModuleGroups = From selectParameter In mCompactRioForm.ChannelsType.AsParallel.AsOrdered
                           Where selectParameter.Chassis = hostName
                           Group selectParameter By Key = $"Mod{selectParameter.NumberModuleChassis}/ai{selectParameter.NumberChannelModule}".Substring(0, 4) Into Group
                           Select FirstLetter = Key, ModuleGroup = Group

        For Each group In ModuleGroups
            Dim channelGroupName As String = group.FirstLetter

            If channelGroupName = String.Empty Then ' если атрибут пустой, то имя группы по порядковому номеру
                channelGroupName = $"Group {channelGroupIndex}"
            End If

            channelGroupIndex += 1

            For Each itemParameter In group.ModuleGroup
                Dim numberParameter As String = CStr(itemParameter.NumberParameter)

                ' сделать запрос к общим каналам Сервера для поиска дополнительных атрибутов
                Dim Parameters As IEnumerable(Of TypeBaseChannel) = From selectParameter In mCompactRioForm.ChannelsType
                                                                    Where selectParameter.NumberParameter = CShort(numberParameter)
                                                                    Select selectParameter

                If Parameters.Count = 0 Then
                    ' здесь накопить Pin не найденные каналы в лист сообщений для ошибки
                    missingChannels.Add($"Номер: <{numberParameter}> отсутствует в базе данных Сервера")
                ElseIf Parameters.Count > 1 Then ' проверять необходимо т.к. для _Config.clsCHANNELS.Ch_Dictionary парсер делает проверку по имени канала
                    ' здесь накопить что itemCh.Pin повторяется 2 раза в каналах Сервера
                    repeatedChannels.Add($"Номер: <{numberParameter}> повторяется более одого раза в базе данных Сервера")
                Else
                    For Each itemChannel As TypeBaseChannel In Parameters
                        Dim mChannel As New ChannelRio With {
                                                        .NumberParameter = numberParameter,
                                                        .IndexOnChassis = index,
                                                        .Group = channelGroupName,
                                                        .HwBoardType = itemParameter.BoardType,
                                                        .ChId = $"Mod{itemParameter.NumberModuleChassis}/ai{itemParameter.NumberChannelModule}",
                                                        .MinVal = itemParameter.LowerMeasure,
                                                        .MaxVal = itemParameter.UpperMeasure,
                                                        .UnitOfMeasure = itemParameter.UnitOfMeasure,
                                                        .TerminalMode = itemParameter.TypeConnection,
                                                        .Name = itemChannel.NameParameter,
                                                        .MinLim = itemChannel.LowerLimit,
                                                        .MaxLim = itemChannel.UpperLimit,
                                                        .CoefficientsPolynomial = itemChannel.CoefficientsPolynomial,
                                                        .LevelOfApproximation = itemChannel.LevelOfApproximation
                                                        }

                        itemChassis.GManagerChannels.Add(mChannel.Name, mChannel)
                        index += 1
                    Next
                End If
            Next
        Next

        If missingChannels.Count > 0 OrElse repeatedChannels.Count > 0 Then
            ShowErrors(missingChannels, repeatedChannels)
        Else
            SaveConfiguration(itemChassis)
        End If
    End Sub

    ''' <summary>
    ''' Выыод и логирование ошибок.
    ''' </summary>
    ''' <param name="missingChannels"></param>
    ''' <param name="repeatedChannels"></param>
    Private Sub ShowErrors(missingChannels As List(Of String), repeatedChannels As List(Of String))
        Dim allErrors As String = Nothing
        Dim I As Integer

        For Each itemError As String In missingChannels
            allErrors &= itemError & Environment.NewLine
            I += 1
            If I > 10 Then
                allErrors &= "и еще более..." & Environment.NewLine
                Exit For
            End If
        Next

        I = 0
        For Each itemError As String In repeatedChannels
            allErrors &= itemError & Environment.NewLine
            I += 1
            If I > 10 Then
                allErrors &= "и еще более..." & Environment.NewLine
                Exit For
            End If
        Next

        Const caption As String = NameOf(FillManagerChannels)
        Dim text As String = $"Список каналов содержит следующие ошибки:{Environment.NewLine}{allErrors}{Environment.NewLine}Загрузка будет прекращена."
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
    End Sub

    ''' <summary>
    ''' Запись простого текстового файла CSV с данными каналов из списков и
    ''' конфигурации с параметрами запуска шасси.
    ''' Запись Конфигурации
    ''' </summary>
    ''' <param name="itemChassis"></param>
    ''' <remarks></remarks>
    Private Sub SaveConfiguration(ByRef itemChassis As Chassis)
        Dim pathChannelsFileCSV As String = Path.Combine(itemChassis.FolderName, C_NI_RT_STARTUP, CHANNELS_FILE_CSV)
        Dim pathAttributeChannelsFileCSV As String = Path.Combine(itemChassis.FolderName, C_NI_RT_STARTUP, ATTRIBUTE_CHANNELS_FILE_CSV)
        Dim pathChassisOptionIni As String = Path.Combine(itemChassis.FolderName, C_NI_RT_STARTUP, CHASSIS_OPTION_INI)

        If File.Exists(pathChannelsFileCSV) Then My.Computer.FileSystem.DeleteFile(pathChannelsFileCSV)

        Try
            Dim strBuilder As New StringBuilder
            Dim listChannels As New List(Of String)

            For Each itemChannel In itemChassis.GManagerChannels.Channels.Values
                AddStringBuilder(strBuilder, itemChannel)
                listChannels.Add(itemChannel.Name)
                itemChassis.ChannelsCount += 1 ' добавить в число измерительных каналов (размерность буфера)
            Next

            itemChassis.ListChannelsToAcquire = listChannels.ToArray

            My.Computer.FileSystem.WriteAllText(pathAttributeChannelsFileCSV, ChannelRio.GetPropertyString, False)
            File.WriteAllBytes(pathChannelsFileCSV, ConvertUnicodeToWindows1251Byte(strBuilder.ToString()))

            WriteINI(pathChassisOptionIni, "Chassis", "ModeWork", itemChassis.ModeWork.ToString)
            WriteINI(pathChassisOptionIni, "SSD", "IPAddressSSD", mOptionData.IPaddressProperty.IP)
            WriteINI(pathChassisOptionIni, "SSD", "Port", mOptionData.Порт)
        Catch ex As Exception 'Catch ex As IOException
            Const caption As String = NameOf(SaveConfiguration)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Дополнить таблицу записей каналов новой строкой.
    ''' </summary>
    ''' <param name="refStrBuilder"></param>
    ''' <param name="itemChannel"></param>
    ''' <remarks></remarks>
    Private Sub AddStringBuilder(ByRef refStrBuilder As StringBuilder, itemChannel As ChannelRio)
        Dim separate As String = ","c 'vbTab

        With refStrBuilder
            .Append(itemChannel.IndexOnChassis & separate)
            .Append(itemChannel.Group & separate)
            ' "ClassHwCh" каналы ИВК
            .Append(itemChannel.NumberParameter & separate)
            .Append(itemChannel.HwBoardType & separate)
            .Append(itemChannel.ChId & separate)
            .Append("ON" & separate)
            .Append(itemChannel.MinVal & separate)
            .Append(itemChannel.MaxVal & separate)
            .Append(itemChannel.UnitOfMeasure & separate)
            .Append(itemChannel.TerminalMode & separate)
            ' "ClassCh" каналы Сервера
            .Append(itemChannel.Name & separate)
            .Append(itemChannel.Name & separate)
            .Append("ON" & separate)
            .Append(itemChannel.MinLim & separate)
            .Append(itemChannel.MaxLim & separate)
            .Append(itemChannel.UnitOfMeasure & separate) ' Scr_EdIzm
            .Append(itemChannel.UnitOfMeasure & separate) ' Ch_Unit
            .Append(itemChannel.CoefficientsPolynomial(0) & separate) ' K1
            .Append(itemChannel.CoefficientsPolynomial(1) & separate) ' K2
            .Append(itemChannel.CoefficientsPolynomial(2) & separate) ' K3
            .Append(itemChannel.CoefficientsPolynomial(3) & separate) ' K4
            .Append(itemChannel.CoefficientsPolynomial(4) & separate) ' K5
            .Append(itemChannel.CoefficientsPolynomial(5) & Environment.NewLine) ' последний Environment.NewLine
        End With
    End Sub

    ''' <summary>
    ''' В менеджере шасси произвести поиск шасси по требуемому IPAddress
    ''' </summary>
    ''' <param name="remoteEndPointIPAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindChassisInManager(remoteEndPointIPAddress As String) As Chassis
        Dim chassisForConnection As Chassis = Nothing

        For Each itemChassis As Chassis In mChassis.Values
            If itemChassis.IPAddressRTtarget.IP = remoteEndPointIPAddress Then
                chassisForConnection = itemChassis
                Exit For
            End If
        Next

        If chassisForConnection Is Nothing Then
            Const caption As String = NameOf(FindChassisInManager)
            Dim text As String = $"Для сокета с IP адрессом: <{remoteEndPointIPAddress}> не найдено соответствующее шасси в коллекции."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End If

        Return chassisForConnection
    End Function

#Region "Регистрация сообщений команд от шасси"
    ''' <summary>
    ''' Шасси отсоединилось от ИВК.
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="IsConnected"></param>
    ''' <remarks></remarks>
    Public Sub ChangeChassisConnection(infoHostName As String, IsConnected As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).IsConnected = IsConnected ' равно False

            Dim text As String = $"Шасси: <{infoHostName}> - отсоединилось от ИВК."
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{NameOf(ChangeChassisConnection)}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Запрос списка разрешённых команд прошёл успешно
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="GetMetaSuccess"></param>
    ''' <remarks></remarks>
    Public Sub GetMetaChassisSuccess(infoHostName As String, GetMetaSuccess As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).GetMetaSuccess = GetMetaSuccess

            Dim text As String = $"Шасси: <{infoHostName}> - запрос списка разрешённых команд прошёл успешно"
            RegistrationEventLog.EventLog_MSG_CONNECT($"<{NameOf(GetMetaChassisSuccess)}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Инициализация модулей прошла успешно
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="InitSuccess"></param>
    ''' <remarks></remarks>
    Public Sub InitializeChassisSuccess(infoHostName As String, InitSuccess As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).IsInitSuccess = InitSuccess

            Const caption As String = NameOf(InitializeChassisSuccess)
            Dim text As String = $"Шасси: <{infoHostName}> - инициализация модулей прошла успешно"
            RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Инициализация модулей не была произведена
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="InitBad"></param>
    ''' <remarks></remarks>
    Public Sub InitializeChassisBad(infoHostName As String, InitBad As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).IsInitBad = InitBad

            Const caption As String = NameOf(InitializeChassisBad)
            Dim text As String = $"Шасси: <{infoHostName}> - инициализация модулей не была произведена"
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Первый запуск опроса каналов произведён успешно
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="LaunchSuccess"></param>
    ''' <remarks></remarks>
    Public Sub LaunchChassisSuccess(infoHostName As String, LaunchSuccess As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).IsLaunchSuccess = LaunchSuccess

            Const caption As String = NameOf(LaunchChassisSuccess)
            Dim text As String = $"Шасси: <{infoHostName}> - первый запуск опроса каналов произведён успешно"
            RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Первый запуск опроса каналов произведён с ошибкой
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="LaunchBad"></param>
    ''' <remarks></remarks>
    Public Sub LaunchChassisBad(infoHostName As String, LaunchBad As Boolean)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).IsLaunchBad = LaunchBad

            Const caption As String = NameOf(LaunchChassisBad)
            Dim text As String = $"Шасси: <{infoHostName}> - первый запуск опроса каналов произведён с ошибкой"
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Во время работы шасси произошла критическая ошибка 
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <param name="CriticalError"></param>
    ''' <remarks></remarks>
    Public Sub CriticalErrorChassis(infoHostName As String, CriticalError As String)
        If mChassis.ContainsKey(infoHostName) Then
            mChassis(infoHostName).CriticalError = CriticalError

            Const caption As String = NameOf(CriticalErrorChassis)
            Dim text As String = $"Шасси: <{infoHostName}> - во время работы произошла критическая ошибка {CriticalError}"
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End If
    End Sub
#End Region
End Class