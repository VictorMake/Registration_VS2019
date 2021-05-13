Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Threading.Thread
Imports System.Timers
Imports System.Xml.Linq
Imports MathematicalLibrary

Public Class FormCompactRio
    Public Class AcquireDataEventArgs
        Inherits EventArgs
        Public Property AcquisitionData() As Double()
    End Class

    Public Event AcquireData As EventHandler(Of AcquireDataEventArgs)
    Public Structure TypeBaseChannel
        Public NumberParameter As Short            ' НомерПараметра
        Public NameParameter As String             ' НаименованиеПараметра
        Public NumberModuleChassis As Short        ' Номер модуля в корзине
        Public NumberChannelModule As Short        ' Номер канала модуля
        Public TypeConnection As String            ' ТипПодключения
        Public LowerMeasure As Single              ' НижнийПредел
        Public UpperMeasure As Single              ' ВерхнийПредел
        Public LevelOfApproximation As Short       ' СтепеньАппроксимации
        <VBFixedArray(5)> Public CoefficientsPolynomial As Double()
        Public UnitOfMeasure As String             ' ЕдиницаИзмерения
        Public LowerLimit As Single                ' ДопускМинимум 
        Public UpperLimit As Single                ' ДопускМаксимум 
        Public Description As String               ' Примечания
        Public Chassis As String
        Public BoardType As String

        Public Sub Initialize()
            Re.Dim(CoefficientsPolynomial, 5)
        End Sub
    End Structure

    Public ChannelsType As TypeBaseChannel()

#Region "Properties"
    Private mPathSettingMdb As String
    ''' <summary>
    ''' Путь к базе Access с настройками.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathSettingMdb() As String
        Get
            Return mPathSettingMdb
        End Get
        Set(ByVal value As String)
            If Not File.Exists(value) Then
                MessageBox.Show($"В каталоге нет файла <{value}>!", $"Запуск модуля {NameOf(FormCompactRio)}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                mPathSettingMdb = value
                PathCatalog = Path.GetDirectoryName(mPathSettingMdb)
                PathSettingXml = Path.Combine(PathCatalog, "SettingFormCompactRio.xml")
            End If
        End Set
    End Property

    Private mPathSettingXml As String = "Определить путь к файлу настроек .xml" ' "...\Ресурсы\***.xml"
    ''' <summary>
    ''' Путь к файлу настроек .xml
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property PathSettingXml() As String
        Get
            Return mPathSettingXml
        End Get
        Set(ByVal value As String)
            mPathSettingXml = value

            If Not File.Exists(mPathSettingXml) Then
                CreateDocumentSettings(mPathSettingXml)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Свойство для управления родителем закрытия окон плагина.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsWindowClosed() As Boolean

    Private mFrequencyAcquisition As Integer
    Private Property FrequencyAcquisition() As Integer
        Get
            Return mFrequencyAcquisition
        End Get
        Set(ByVal value As Integer)
            mFrequencyAcquisition = value
            timerIntervalMainFormAcquisition = 1000 \ value
            TimerIntervalWait = timerIntervalMainFormAcquisition
        End Set
    End Property

    Public Property TimerIntervalWait As Integer = timerIntervalMainFormAcquisition
    Public Property IsStartAcquisition As Boolean ' флаг запуска сбора
    ''' <summary>
    ''' Все шасси cRio в конфигурации
    ''' </summary>
    ''' <returns></returns>
    Friend Property ManagerChassis As TargetsChassis
#End Region

    ''' <summary>
    ''' Список идентификаторов разрешённых команд обмена ИВК с шасси.
    ''' Список должен быть расширен.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CommandSet As UShort
        GetListCommand_0 = 0        ' обязательно 0 первая команда отсылаемая Клиентом (шасси cRio)
        GetMeta_1 = 1               ' обязательно 1 первая команда отсылаемая Сервером, содержит XML список команд, разрешённых для шасси
        Activate_2 = 2              ' команда отсылаемая Сервером для запуска-остановки цикла сбора
        Stop_3 = 3                  ' команда отсылаемая Сервером прекращение работы
        Acquisition_4 = 4           ' обязательно 4 команда отсылаемая Клиентом с буфером сбора
        Pause_5 = 5                 ' команда отсылаемая Сервером для запуска-остановки передачи по сети результатов замера
        Loop_Period_6 = 6           ' команда отсылаемая Сервером (Обратная от частоты сбора) перед командой Activate_2 для настройки клиента на частоту сбора (можно в процессе работы изменять)
        InitSuccess_7 = 7           ' команда отсылаемая Клиентом об успехе инициализации контроллера после разбора конфигурационного файла. В теле содержится текст "InitSuccess".
        InitBad_8 = 8               ' команда отсылаемая Клиентом ошибка инициализации контроллера (может быть нет такого модуля). В теле содержится текст ошибки.
        LaunchSuccess_9 = 9         ' команда отсылаемая Клиентом после первого цикла опроса железа (перед первой отправкой буфера). Выполняется только 1 раз, как свидетельство работы контроллера. В теле содержится текст "LaunchSuccess".
        LaunchBad_10 = 10           ' команда отсылаемая Клиентом в случае сбоя первого сбора буфера. В теле содержится текст ошибки.
        CriticalError_11 = 11       ' команда отсылаемая Клиентом в случае ошибки на терминале. В теле содержится текст ошибки.
        Function_12 = 12            ' Резерв - команда отсылаемая Сервером для настройки функции сигнала для теста (0 - случайная, 1 - синус, 2 - косинус)
        Lost_Samples_13 = 13        ' Резерв - команда отсылаемая Клиентом о пропущенных буферах при сборе между детерминированным циклом опроса железа и циклом передачи данных в сеть (ошибка -2221 буфера Acquisition). В теле содержится I32 - число
        CheckConnection_14 = 14     ' Пустая команда, отправляется клиенту 1 раз в секунду  и служит как маркер проверки связи. В случае отсоединения клиента произойдёт разрыв соединения
        Unknown = UShort.MaxValue
    End Enum

    Private mOptionDataCRIO As OptionDataCRIO ' редактируемые данные

    ''' <summary>
    ''' Универсальная коллекция, поддерживающая привязку данных.
    ''' Для отображения шасси.
    ''' </summary>
    Private mChassisValueList As BindingList(Of ChassisValue)
    ''' <summary>
    ''' Количество всех каналов ИВК (сумма по всем шасси)
    ''' </summary>
    Private LengthArrChannelsAllChassis As Integer

#Region "ConstCommand"
    Private Const GET_LIST_COMMAND_0 As String = "GetListCommand"
    Private Const GET_META_1 As String = "GetMeta"
    Private Const ACTIVATE_2 As String = "Activate"
    Private Const STOP_3 As String = "Stop"
    Private Const ACQUISITION_4 As String = "Acquisition"
    Private Const PAUSE_5 As String = "Pause"
    Private Const LOOP_PERIOD_6 As String = "Loop Period"
    Private Const INIT_SUCCESS_7 As String = "InitSuccess"
    Private Const INIT_BAD_8 As String = "InitBad"
    Private Const LAUNCH_SUCCESS_9 As String = "LaunchSuccess"
    Private Const LAUNCH_BAD_10 As String = "LaunchBad"
    Private Const CRITICAL_ERROR_11 As String = "CriticalError"
    Private Const FUNCTION_12 As String = "Function"
    Private Const LOST_SAMPLES_13 As String = "Lost Samples"
    Private Const CHECK_CONNECTION_14 As String = "CheckConnection"

    Private ReadOnly ArrCmdForMeasurement As String() = {
                                            GET_LIST_COMMAND_0,
                                            GET_META_1,
                                            ACTIVATE_2,
                                            STOP_3,
                                            ACQUISITION_4,
                                            PAUSE_5,
                                            LOOP_PERIOD_6,
                                            INIT_SUCCESS_7,
                                            INIT_BAD_8,
                                            LAUNCH_SUCCESS_9,
                                            LAUNCH_BAD_10,
                                            CRITICAL_ERROR_11,
                                            FUNCTION_12,
                                            LOST_SAMPLES_13,
                                            CHECK_CONNECTION_14
                                            }

    Private ReadOnly ArrEnumForMeasurement As CommandSet() = {
                                            CommandSet.GetListCommand_0,
                                            CommandSet.GetMeta_1,
                                            CommandSet.Activate_2,
                                            CommandSet.Stop_3,
                                            CommandSet.Acquisition_4,
                                            CommandSet.Pause_5,
                                            CommandSet.Loop_Period_6,
                                            CommandSet.InitSuccess_7,
                                            CommandSet.InitBad_8,
                                            CommandSet.LaunchSuccess_9,
                                            CommandSet.LaunchBad_10,
                                            CommandSet.CriticalError_11,
                                            CommandSet.Function_12,
                                            CommandSet.Lost_Samples_13,
                                            CommandSet.CheckConnection_14
                                            }

    Private Const BodyPacketLength As Integer = 4   ' 4 байт - длина тела данных
    Private Const CommandLength As Integer = 2      ' 2 байта номера команды
    Private Const ArraySizeLength As Integer = 2    ' 2 байта - размерность массива
    Private Const InfoLength As Integer = BodyPacketLength + CommandLength
    Private Const DoubleBytes As Integer = 8
    Private thresholdPacketsCount As Integer = 100  ' число полученных пакетов, чтобы обновить экран (при 100 Гц обновление раз в сек)
    Private Const MaxChannelsOnShassis As Integer = 270 ' буфер сконфигурирован на 270 каналов, принимаемый пакет надо добрать на величину пустышек
    ' Ошибочно было: Private Const MaxMsgFromClientLength As Integer = MaxChannelsOnShassis * DoubleBytes * 8 + 10 
    Private Const MaxMsgFromClientLength As Integer = MaxChannelsOnShassis * DoubleBytes + BodyPacketLength + CommandLength + ArraySizeLength + 10 ' = 2168 '(270 канала)*8 байт + (8 заголовок) + (10 запас не знаю для чего) (= 20488 '((10 шасси) * (8 модулей) * (32 канала))*8 байт + (8 заголовок))
    Private Const TimerIntervalCheckFreeSpace As Integer = 15   ' проверка каждые 15 минут(чаще не надо)
    Private Const WaiteStartStopAcquisition As Integer = 20000  ' подождать до запуска монитора для подключения клиентов
#End Region

#Region "ConstTree"
    Private Enum DataGridColumnEnum As Integer
        <Description("NameChassisDataGridViewTextBoxColumn")>
        NameChassis = 0
        <Description("StatusAdapterDataGridViewTextBoxColumn")>
        StatusAdapterData = 1
        <Description("StatusAdapterImageDataGridViewImageColumn")>
        StatusAdapterImage = 2
        <Description("StatusSendImageDataGridViewImageColumn")>
        StatusSendImage = 3
        <Description("StatusReceiveImageDataGridViewImageColumn")>
        StatusReceiveImage = 4
        <Description("PacketsReceiveDataGridViewTextBoxColumn")>
        PacketsReceiveData = 5
    End Enum

    ''' <summary>
    ''' Иконка для параметра в дереве
    ''' </summary>
    Private Enum NodeImage
        Rotation0 = 0 ' "%" "ОБОРОТЫ"
        Discrete1 = 1 ' "дел" "ДИСКРЕТНЫЕ"
        Evacuation2 = 2 ' "мм" "РАЗРЕЖЕНИЯ"
        Temperature3 = 3 ' "градус" "ТЕМПЕРАТУРЫ"
        Petrol4 = 4 ' "кг/ч" "РАСХОДЫ"
        Vibration5 = 5 ' "мм/с" "ВИБРАЦИЯ"
        Current6 = 6 ' "мкА" "ТОКИ"
        Presure7 = 7 ' "Кгсм" "ДАВЛЕНИЯ"
        Traction8 = 8 ' "кгс" "ТЯГА"
        SliderVertical9 = 9
        SliderHorizontal10 = 10
        SliderVertical11 = 11
        Close12 = 12
        Chassis13 = 13 ' Корзина
        ChassisSelected14 = 14 ' Корзина открыта
        Module15 = 15 ' Модуль
        ModuleSelected16 = 16 ' Модуль открыт
        DAQBoard17 = 17 ' Плата АЦП
        DAQBoardSelected18 = 18 ' Плата АЦП открыта
        Selected19 = 19 ' выделенный узел
    End Enum

    ''' <summary>
    ''' Тип узла в дереве
    ''' </summary>
    Friend Enum NodeTypeEnum
        ''' <summary>
        ''' ССД0
        ''' </summary>
        SSD0 = 0
        ''' <summary>
        ''' Шасси1
        ''' </summary>
        Chassis1 = 1
        ''' <summary>
        ''' Модуль2
        ''' </summary>
        Module2 = 2
        ''' <summary>
        ''' Канал3
        ''' </summary>
        Channel3 = 3
        ''' <summary>
        ''' ТипПараметра5
        ''' </summary>
        ParameterType5 = 5
    End Enum

    ''' <summary>
    ''' Иконка столбца для списка шасси
    ''' </summary>
    Private Enum UnitSubItem As Integer
        Module_ = 0
        IPAddress = 1
        Mode = 2
        ChannelsCount = 3
        Revolution = 4
        Discrete = 5
        Evacuation = 6
        Temperature = 7
        Presure = 8
        Vibration = 9
        Current = 10
        Petrol = 11
        Traction = 12
    End Enum

    Private Const cModule As String = "Шасси"
    Private Const cIPAddress As String = "IP Address"
    Private Const cMode As String = "Режим"
    Private Const cChannelsCount As String = "Всего каналов"
    Private Const cRevolution As String = "Обороты"
    Private Const cDiscrete As String = "Дискретные"
    Private Const cEvacuation As String = "Разрежения"
    Private Const cTemperature As String = "Температуры"
    Private Const cPresure As String = "Давления"
    Private Const cVibration As String = "Вибрация"
    Private Const cCurrent As String = "Токи"
    Private Const cPetrol As String = "Расходы"
    Private Const cTraction As String = "Тяга"

    Private Const unitRevolution As String = "%" ' "ОБОРОТЫ" - 0
    Private Const unitDiscrete As String = "дел" ' "ДИСКРЕТНЫЕ" - 1
    Private Const unitEvacuation As String = "мм" ' "РАЗРЕЖЕНИЯ" -2
    Private Const unitTemperature As String = "градус" ' "ТЕМПЕРАТУРЫ" - 3
    Private Const unitPresure As String = "Кгсм" ' "ДАВЛЕНИЯ" - 4
    Private Const unitVibration As String = "мм/с" ' "ВИБРАЦИЯ" - 5
    Private Const unitCurrent As String = "мкА" ' "ТОКИ" - 6
    Private Const unitPetrol As String = "кг/ч" ' "РАСХОДЫ" - 7
    Private Const unitTraction As String = "кгс" ' "ТЯГА" - 8

    ' {"%", "дел", "мм", "градус", "Кгсм", "мм/с", "мкА", "кг/ч", "кгс"}' Размерности
    Private ReadOnly UnitOfMeasureArray As String() = {unitRevolution, unitDiscrete, unitEvacuation, unitTemperature, unitPresure, unitVibration, unitCurrent, unitPetrol, unitTraction}
    ' "%-дел-мм-градус-Кгсм-мм/с-мкА-кг/ч-кгс"' ВХОЖДЕНИЯ_РАЗМЕРНОСТЕЙ
    Private ReadOnly Pattern_Units As String = $"{unitRevolution}-{unitDiscrete}-{unitEvacuation}-{unitTemperature}-{unitPresure}-{unitVibration}-{unitCurrent}-{unitPetrol}-{unitTraction}"
    Private ReadOnly pattern As String = $"{unitRevolution}{unitDiscrete}{unitEvacuation}{unitTemperature}{unitPresure}{unitVibration}{unitCurrent}{unitPetrol}{unitTraction}"

    Private Const ColumnChassis As String = "ColumnChassis"
    Private Const ColumnModule As String = "ColumnModule"
    Private Const ColumnIPAddress As String = "ColumnIPAddress"
    Private Const ColumnMode As String = "ColumnMode"
    Private Const ColumnAllChannels As String = "ColumnAllChannels"
    Private Const ColumnRevolution As String = "ColumnRevolution" ' Обороты
    Private Const ColumnDiscrete As String = "ColumnDiscrete" ' "ДИСКРЕТНЫЕ" 
    Private Const ColumnEvacuation As String = "ColumnEvacuation" ' Разрежения
    Private Const ColumnTemperature As String = "ColumnTemperature" ' "ТЕМПЕРАТУРЫ"
    Private Const ColumnPresure As String = "ColumnPresure" ' "ДАВЛЕНИЯ" 
    Private Const ColumnVibration As String = "ColumnVibration" ' Вибрация
    Private Const ColumnCurrent As String = "ColumnCurrent" ' "ТОКИ" 
    Private Const ColumnPetrol As String = "ColumnPetrol" ' Расходы
    Private Const ColumnTraction As String = "ColumnTraction" ' Тяга
#End Region

    Private WithEvents TabPageChassis As TabPage
    Private WithEvents RichTextBoxChassis As RichTextBox
    Private ReadOnly RichTextBoxes As New Dictionary(Of Integer, RichTextBox)

    Private ssdListener As TcpListener              ' server (ИВК) слушающий клиентов (шасси) для подключения
    Private ReadOnly dictConnInfoChassis As New Dictionary(Of String, ConnectionInfoServer)
    Private TaskConnectionMontior As Task           ' задача ожидания подключения шасси
    Private TaskCheckConnectionWithChassis As Task  ' задача проверки соединений с клиентами

    Private isInitializeSuccess As Boolean          ' настройка Формы Выполнена
    Private isHandleFormCreated As Boolean          ' форма Загружена
    Private AcquisitionMemo As Double()             ' осреднённые буферизованные по событию таймера значения каналов всех шасси, среднее значение вычисляется в индексе с номером counterBuffer
    Private AcquisitionAverageChassis As Double()   ' осреднённые значения отсеянных каналов включёнными в конфигурацию опроса - буферизованные по событию таймера значения каналов всех шасси
    Private counterBuffer As Integer                ' счётчик добавлений в массив сбора AcquisitionMemo дла подсчёта среднего значения и вызова отправки в в Registration

    ''' <summary>
    ''' Делегата для функции AppendMsgToRichTextBoxByKey в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)

#Region "Переменные Клиент"
    Private WithEvents mmTimerAcquisitionMainForm As Multimedia.Timer       ' точный системный таймер
    Private WithEvents mmTimerChassisAcquisition100Hz As Multimedia.Timer   ' точный системный таймер
    Private aTimerUpdataTree As System.Timers.Timer                         ' серверный таймер работает в другом потоке
    Private Const timerIntervalChassisAcquisition100Hz As Integer = 10      ' (миллисекунд) интервал таймера и интервал частоты шасси (100 Гц)
    Private timerIntervalMainFormAcquisition As Integer = 10                ' (миллисекунд) интервал таймера регистрации данных
    Private syncTimerAcquisition100Hz As Integer = 0                        ' Блокировка потока от двойного события таймера
    Private syncTimerUpdataTree As Integer = 0                              ' Блокировка потока от двойного события таймера
    Private syncTimerAcquisitionMainForm As Integer = 0                     ' Блокировка потока от двойного события таймера
    Private Shared ReadOnly HandlerLockObjectMonitorConn As New Object      ' объект синхронизации для блокирования доступа

    Private eventHandlerTimerTick As EventHandler
    Private ReadOnly mFormMainMDI As FormMainMDI

    Private Const CRITICAL_CMD_ERROR As String = "Неизвестная команда от шасси"
    Private Const ACQUISITION_START As String = "Запустить передачу замеров на Registration"
    Private Const ACQUISITION_STOP As String = "Остановить передачу замеров на Registration"
#End Region

#Region "Сетевой обмен TCP/IP (клиентские функции)"
    Private ReadOnly ASCII_Encoding As New Text.ASCIIEncoding
    ''' <summary>
    ''' задаётся интервалом таймера (частота сбора 100 Гц или 10 милисекунд)
    ''' </summary>
    ''' <remarks></remarks>
    Private LoopPeriod As UInt32 = 10
    ''' <summary>
    ''' Смещение индекса вкладки сообщений Серверной части
    ''' </summary>
    ''' <remarks></remarks>
    Public Const KeyRichTexServer As Integer = -1
    ''' <summary>
    ''' Смещение индекса вкладки сообщений Клиентской части
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeyRichTexClient As Integer = -2
    ''' <summary>
    ''' флаг успешной операции проверки
    ''' </summary>
    ''' <remarks></remarks>
    Private IsRestartAllChassisSuccess As Boolean
    ''' <summary>
    ''' Ёмкость буфера собранных данных определяется частотой сбора данных от шасси cRio.
    ''' </summary>
    Private RefreshScreen As Integer
#End Region

    ''' <summary>
    ''' Путь к каталогу с дополнительными файлами к программе.
    ''' </summary>
    Private PathCatalog As String
    ''' <summary>
    ''' Описание для окна уточняется из файла конфигурации.
    ''' </summary>
    Private Description As String = "Это Тестовая форма CompactRio"
    ' Один токен отмены должен относиться к одной отменяемой операции, однако эта операция может быть реализована в программе.
    ' После того как свойство IsCancellationRequested токена примет значение true, для него невозможно будет восстановить значение false. 
    ' Таким образом, токены отмены невозможно использовать повторно после выполнения отмены.
    Private tokenSource As CancellationTokenSource
    Private message As String = String.Empty    ' тексты для журналирования сообщений
    Private parentNodeExpand As ChannelNode     ' родительский узел раскрытой ветки в дереве 
    Private nodeExpand As ChannelNode           ' узел раскрытой ветки в дереве

    Public Sub New(inFormMainMDI As FormMainMDI)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
        mFormMainMDI = inFormMainMDI
    End Sub

    ''' <summary>
    ''' Начальная инициализация массивов используемых в программе в зависимости от каналов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        FrequencyAcquisition = FrequencyBackground
        'Re.Dim(AcquisitionValueOfDouble, RegistrationMain.CountMeasurand + 1)
        ' Инициализация  размерности массива по числу каналов для подключенных шасси, т.к. некоторые могут быть отключены
        ' значить и ParametersType должен учитывать подключенные шасси (а в базе этих полей может не быть).
        ' При запуске это отслеживается и выводится сообщение.
        Re.Dim(AcquisitionAverageChassis, ParametersType.Length - 1)

        'TestInitialize()
    End Sub

    Private Sub CompactRioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' если в настройках приложения включена исполняющая среда, 
        ' то возможна обработка необработанных исключений Handles Me.UnhandledException
        ' поэтому первой должна запускаться форма
        mOptionDataCRIO = GetOptionData()
        SetRefreshScreen()
        Me.Text += $" (частота сбора на шасси: {1000 / timerIntervalChassisAcquisition100Hz} Гц , приёма данных: {FrequencyBackground} Гц)"
        LoopPeriod = timerIntervalChassisAcquisition100Hz ' частота сбора шасси должна быть согласована с ИВК
        LoadConfiguration()
        myDelegate = New AddListItem(AddressOf AddListItemMethod) ' делегат обновления текста лога из другого потока
        Me.TopMost = True
        InitializeFormAgain()
    End Sub

    ''' <summary>
    ''' Загрузка окна настроек и передача класса настроек работы с шасси CompactRio.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetOptionData() As OptionDataCRIO
        Using mFormCompactRioSetting As New FormCompactRioSetting
            mFormCompactRioSetting.LoadForm()
            Return mFormCompactRioSetting.GetOptionData
        End Using
    End Function

    ''' <summary>
    ''' Инициализация интерфейса вызывается при загрузке приложения и при перезагрузке
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeFormAgain()
        ' Создать таймер с заданным интервалом.
        aTimerUpdataTree = New System.Timers.Timer(1000)
        ' назначить делегата обработки события таймера.
        AddHandler aTimerUpdataTree.Elapsed, AddressOf OnTimedUpdataTreeEvent
        aTimerUpdataTree.Enabled = ChannelsToolStripMenuItem.Checked
        IsRestartAllChassisSuccess = False
        ' Настроить интерфейс пользователя
        isInitializeSuccess = False
        isHandleFormCreated = False

        BindingDataGridChassis()   ' связывание таблицы со списком
        InitializeListViewColumns() ' настройка колонок
        InitializeTabPageChassis()  ' настройка вкладок
        InitializeListView()        ' настройка изображений
        PopulateDataGridViewRowsOnHostName() ' прописка HostName в строках шасси
        SSDPortTextBox.Text = mOptionDataCRIO.Порт

        InitializePanelServerForm() ' дополнительные настройки
        ' запуск 
        Dim tsk As Task = Task.Factory.StartNew(Sub() SSDRun()) ' не ждём завершения задачи, пользовательский интерфейс разблокирован
        Application.DoEvents()

        isHandleFormCreated = True
    End Sub

    'Private Sub CompactRioForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    '    ' Проверить выключение производит пользователь или Панель Управления
    '    If e.CloseReason = CloseReason.UserClosing Then
    '        If isSSDShutDown = False Then
    '            If MessageBox.Show("Вы действительно хотите завершить работу всех шасси?",
    '                               "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
    '                e.Cancel = True
    '            End If
    '        End If
    '    End If

    '    If Not e.Cancel Then StopAllConnections()
    'End Sub

    Private Sub FormCompactRio_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If IsWindowClosed Then
            SavePathSettinngXml()
        Else
            e.Cancel = True
            Return
        End If

        StopAllConnections()
    End Sub

    ''' <summary>
    ''' Функция ассинхронного выполнения для задачи фонового потока инициализации и запуска ИВК. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SSDRun()
        ' Основной поток программы ИВК – данный поток запускается самым первым и производит инициализацию основных структур данных, 
        ' инициализацию шасси и запуск служебных потоков ИВК. 
        ' Так же, поток производит управление служебными потоками и контроль над свободным местом локального жёсткого диска. 
        ' Информацию о необходимости перезагрузить или остановить программу, основной поток получает из потока приёма данных от сервера.

        If IsLoadCfgAndReboot() = False Then
            ShowErrorSSDRun()
            Exit Sub
        End If

        ' Запуск мониторинга подключения клиентов для слушающего сокета
        InvokeStartAcquisitionTargetCRIO()

        ' проверку делать 2 раза, т.к. иногда время в какой либо проверке требуется больше
        If IsRunChassisTasks() = False AndAlso IsRunChassisTasks() = False Then
            ShowErrorSSDRun()
            Exit Sub
        End If

        ' Установить размерности массивов сбора и Хеш
        LengthArrChannelsAllChassis = 0
        For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
            LengthArrChannelsAllChassis += itemChassis.ChannelsCount ' число каналов включённых каналов подлежащих измерению
        Next

        Re.Dim(AcquisitionMemo, LengthArrChannelsAllChassis - 1) ' в последнем индексе вычисляется среднее значение
        Re.Dim(AcquisitionAverageChassis, LengthArrChannelsAllChassis - 1) ' инициализация должна быть выполнена после настройки конфигурации каналов

        ' Запуск сбора на Шасси
        InvokeActivateTarget(True)
        InvokePopulateChannels()
        ' запустить в фоне проверку соединения с Сервером
        ' В случае разрыва соединения, поток будет производить попытки его восстановить. 
        tokenSource = New CancellationTokenSource
        ' проверка соединений с шасси
        TaskCheckConnectionWithChassis = Task.Factory.StartNew(Sub() WorkCheckConnectionWithClients(tokenSource.Token), tokenSource.Token, TaskCreationOptions.LongRunning)
        InvokeStartAcquisition(True)
        DownSwithTopMost()
    End Sub

    Private Sub ShowErrorSSDRun()
        Const caption As String = NameOf(SSDRun)
        Const text As String = "Работа приложения невозможна из-за проблем с корректным запуском шасси"

        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, MessageBoxIcon.Error)
    End Sub

    ''' <summary>
    '''  Возвращает значение, показывающее, необходимо ли отображать форму как форму переднего плана.
    ''' </summary>
    Private Sub DownSwithTopMost()
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() DownSwithTopMost()))
        Else
            Me.TopMost = False
        End If
    End Sub

    ''' <summary>
    ''' Остановка и закрытие всех потоков подключений.
    ''' Вызывается при закрытии приложения и при перезагрузке.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StopAllConnections()
        ' освободить управляемое состояние (управляемые объекты).
        If aTimerUpdataTree IsNot Nothing Then
            If aTimerUpdataTree.Enabled Then aTimerUpdataTree.Stop()
            aTimerUpdataTree.Dispose()
            aTimerUpdataTree = Nothing
        End If

        StopAcquisition()
        StopAcquisitionTimer()

        If tokenSource IsNot Nothing Then tokenSource.Cancel() ' прервать задачу проверки сетевого подключения с сервером

        Application.DoEvents()
        ActivateTarget(False) ' остановить сбора на Шасси
        Application.DoEvents()
        StartStopAcquisitionTargetCRIO(False) ' остановить Серверную часть и выключить все шасси послав команду Stop
        tokenSource.Dispose()
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Обнулить массив значений каналов при перезапуске таймера
    ''' </summary>
    Private Sub ClearAcquisitionTimerChassis()
        Array.Clear(AcquisitionMemo, 0, AcquisitionMemo.Length)
        Array.Clear(AcquisitionAverageChassis, 0, AcquisitionAverageChassis.Length)
    End Sub

    ''' <summary>
    ''' По частоте сбора настроить значение ёмкости буфера собранных данных.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetRefreshScreen()
        'RefreshScreen = CInt(100 / Integer.Parse(mOptionDataCRIO.Frequency))
        RefreshScreen = 100 \ FrequencyBackground
        'Select Case Frequency
        '    Case 1
        '        RefreshScreen = 100
        '    Case 100
        '        RefreshScreen = 1
        'End Select
    End Sub

#Region "Интерфейс пользователя"
    ''' <summary>
    ''' Вкладка Серверной части панели.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializePanelServerForm()
        ' Команды Клиента
        CmbBoxCommand.Items.Clear()
        CmbBoxCommand.Items.AddRange({ACQUISITION_START, ACQUISITION_STOP})
        CmbBoxCommand.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Прописать адрес HostName в каждую строку по шасси.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateDataGridViewRowsOnHostName()
        Dim strHostName As String = Dns.GetHostName()
        ' Получить Ip address локальной машины
        ' Сперва получить host name локальной машины
        Dim ipEntry As IPHostEntry = Dns.GetHostEntry(strHostName)
        Dim addr As System.Net.IPAddress() = ipEntry.AddressList

        For I As Integer = 0 To addr.Length - 1
            If addr(I).AddressFamily = AddressFamily.InterNetwork Then
                For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
                    grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusAdapterData).Value = $"host: {strHostName} IP: {addr(I)}"
                Next
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Настойка таблицы посредством связывания со списком.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindingDataGridChassis()
        Dim ledGrayImage As Image = My.Resources.ledCornerGray
        Dim indexRow As Integer

        BindingSourceChassis.DataSource = Nothing
        mChassisValueList = New BindingList(Of ChassisValue)

        For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
            mChassisValueList.Add(New ChassisValue(itemTargetCRIO.HostName, "StatusAdapter", ledGrayImage, ledGrayImage, ledGrayImage, "PacketsReceive"))
            itemTargetCRIO.IndexRow = indexRow
            indexRow += 1
        Next

        BindingSourceChassis.DataSource = mChassisValueList
        grdChassis.Refresh()
    End Sub

    ''' <summary>
    ''' Настройка колонок в ListView.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeListViewColumns()
        ' Назначить ImageList для ListView.
        ListView.LargeImageList = ImageListTree
        ListView.SmallImageList = ImageListTree

        ListView.Columns.Clear()
        ' Установка ширины столбцов действует только для текущего представления, поэтому в этой строке
        ' список явным образом переключается в режим "Маленькие значки"
        ' перед установкой ширины столбца
        'SetView(View.SmallIcon)

        'lvColumnHeader = ListView.Columns.Add("Шасси")
        ' Установить достаточную ширину столбцов в режиме "Маленькие значки", чтобы элементы
        ' не накладывались друг на друга
        ' внимание, второй и третий столбцы не видны в режиме "Маленькие значки",
        ' поэтому настраивать их необязательно

        ' В режиме "Таблица" первый столбец должен быть несколько шире, чем
        ' в режиме "Маленькие значки", а для Столбец2 и Столбец3 в этом режиме
        ' размеры надо указать явно

        ' Разрешить пользователю редактировать текст.
        'ListView.LabelEdit = True
        ' Разрешить пользователю реорганизовывать колонки.
        ListView.AllowColumnReorder = True
        ' Показать check boxes.
        'ListView.CheckBoxes = True
        ' Выделять item и subitems в режиме выделения.
        ListView.FullRowSelect = True
        ' Показать сетку линий.
        'ListView.GridLines = True
        ' Сортировать элементы листа по возрастанию.
        'ListView.Sorting = SortOrder.Ascending

        ' Создать колонки для элементов и подэлементов.
        ' При установке в -2 индикация в auto-size.
        ListView.Columns.Add(ColumnModule, cModule, 150, HorizontalAlignment.Center, ColumnModule) ' 0
        ListView.Columns.Add(ColumnIPAddress, cIPAddress, 100, HorizontalAlignment.Center, ColumnIPAddress) ' 1
        ListView.Columns.Add(ColumnMode, cMode, 100, HorizontalAlignment.Center, ColumnMode) ' 2
        ListView.Columns.Add(ColumnAllChannels, cChannelsCount, 100, HorizontalAlignment.Center, ColumnAllChannels) ' 3
        ListView.Columns.Add(ColumnRevolution, cRevolution, 100, HorizontalAlignment.Center, ColumnRevolution) ' "ОБОРОТЫ" - 4
        ListView.Columns.Add(ColumnDiscrete, cDiscrete, 100, HorizontalAlignment.Center, ColumnDiscrete) ' "ДИСКРЕТНЫЕ" - 5
        ListView.Columns.Add(ColumnEvacuation, cEvacuation, 100, HorizontalAlignment.Center, ColumnEvacuation) ' "РАЗРЕЖЕНИЯ" - 6
        ListView.Columns.Add(ColumnTemperature, cTemperature, 100, HorizontalAlignment.Center, ColumnTemperature) ' "ТЕМПЕРАТУРЫ" - 7
        ListView.Columns.Add(ColumnPresure, cPresure, 100, HorizontalAlignment.Center, ColumnPresure) ' "ДАВЛЕНИЯ" - 8
        ListView.Columns.Add(ColumnVibration, cVibration, 100, HorizontalAlignment.Center, ColumnVibration) ' "ВИБРАЦИЯ" - 9
        ListView.Columns.Add(ColumnCurrent, cCurrent, 100, HorizontalAlignment.Center, ColumnCurrent) ' "ТОКИ" - 10
        ListView.Columns.Add(ColumnPetrol, cPetrol, 100, HorizontalAlignment.Center, ColumnPetrol) ' "РАСХОДЫ" - 11
        ListView.Columns.Add(ColumnTraction, cTraction, 100, HorizontalAlignment.Center, ColumnTraction) ' "ТЯГА" - 12

        ' Переключить отображение в режим "Таблица" и установить соответствующую
        ' ширину столбцов, отличную от ширины столбцов в режиме "Маленькие значки"
        SetView(View.Details)
    End Sub

    ''' <summary>
    ''' Настройка отображения шасси в ListView только сконфигурированных в настроечной форме.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeListView()
        Dim lvItem As ListViewItem

        ListView.Items.Clear()

        For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
            lvItem = ListView.Items.Add(itemTargetCRIO.HostName)
            lvItem.Tag = itemTargetCRIO.HostName

            If itemTargetCRIO.ModeWork = EnumModeWork.Measurement Then
                lvItem.ImageKey = ColumnChassis
            Else
                lvItem.ImageKey = ColumnChassis
            End If

            lvItem.SubItems.AddRange({itemTargetCRIO.IPAddressRTtarget.IP,
                                     itemTargetCRIO.ModeWork.ToString,
                                     "-", "-", "-", "-", "-", "-", "-", "-", "-", "-"})
        Next
    End Sub

    ''' <summary>
    ''' Занесение в ListView по шасси колличеством параметров сгруппированным то типам.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateListViewItems()
        For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
            For indexLVItem As Integer = 0 To ListView.Items.Count - 1
                If ListView.Items(indexLVItem).Text = itemChassis.HostName Then
                    ' узнали шасси -> пройти по размерностям для него и для каждой узнать количество каналов
                    For Each unit As String In UnitOfMeasureArray
                        Dim countUnit As Integer = (From selectChannel In itemChassis.GManagerChannels.Channels.Values
                                                    Where selectChannel.UnitOfMeasure = unit
                                                    Select selectChannel).Count
                        If countUnit > 0 Then
                            Select Case unit
                                Case unitRevolution ' "%"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Revolution).Text = CStr(countUnit) ' "ОБОРОТЫ" - 4
                                Case unitDiscrete ' "дел"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Discrete).Text = CStr(countUnit) ' "ДИСКРЕТНЫЕ" - 5
                                Case unitEvacuation ' "мм"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Evacuation).Text = CStr(countUnit) ' "РАЗРЕЖЕНИЯ" - 6
                                Case unitTemperature ' "градус"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Temperature).Text = CStr(countUnit) ' "ТЕМПЕРАТУРЫ" - 7
                                Case unitPresure ' "Кгсм"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Presure).Text = CStr(countUnit) ' "ДАВЛЕНИЯ" - 8
                                Case unitVibration ' "мм/с"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Vibration).Text = CStr(countUnit) ' "ВИБРАЦИЯ" - 9
                                Case unitCurrent ' "мкА"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Current).Text = CStr(countUnit) ' "ТОКИ" - 10
                                Case unitPetrol ' "кг/ч"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Petrol).Text = CStr(countUnit) ' "РАСХОДЫ" - 11
                                Case unitTraction ' "кгс"
                                    ListView.Items(indexLVItem).SubItems(UnitSubItem.Traction).Text = CStr(countUnit) ' "ТЯГА" - 12
                            End Select
                        End If
                    Next

                    ListView.Items(indexLVItem).SubItems(UnitSubItem.ChannelsCount).Text = CStr(itemChassis.GManagerChannels.Channels.Count)

                    Exit For
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Настройка панели вкладок и динамическое создание RichTextBox для логирования сообщений.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeTabPageChassis()
        RichTextBoxes.Clear()
        RichTextBoxes.Add(KeyRichTexServer, RichTextBoxServer)
        RichTextBoxes.Add(KeyRichTexClient, RichTextBoxClient)

        For I As Integer = TabLogControl.Controls.Count - 1 To 2 Step -1
            TabLogControl.Controls.RemoveAt(I)
        Next

        Dim pagesCount As Integer

        For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
            RichTextBoxChassis = New RichTextBox()
            TabPageChassis = New TabPage()

            TabPageChassis.SuspendLayout()
            TabLogControl.Controls.Add(TabPageChassis)

            '--- TabPageChassis -----------------------------------------------
            TabPageChassis.BackColor = SystemColors.Control
            TabPageChassis.BorderStyle = BorderStyle.Fixed3D
            TabPageChassis.Controls.Add(RichTextBoxChassis)
            TabPageChassis.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
            TabPageChassis.ImageIndex = 2
            TabPageChassis.Location = New Point(4, 23)
            TabPageChassis.Name = "TabPageChassis" & pagesCount
            TabPageChassis.Tag = pagesCount
            TabPageChassis.Size = New Size(333, 597)
            TabPageChassis.Text = itemTargetCRIO.HostName

            '--- RichTextBoxChassis -------------------------------------------
            RichTextBoxChassis.Dock = DockStyle.Fill
            RichTextBoxChassis.Location = New Point(0, 0)
            RichTextBoxChassis.Name = "RichTextBoxChassis" & pagesCount
            RichTextBoxChassis.ReadOnly = True
            RichTextBoxChassis.Size = New Size(329, 593)
            RichTextBoxChassis.Text = ""

            TabPageChassis.ResumeLayout(False)
            RichTextBoxes.Add(pagesCount, RichTextBoxChassis)
            pagesCount += 1
        Next

        BackToolStripButton.Enabled = mOptionDataCRIO.CollectionTargetCRIO.Count > 1
        ForwardToolStripButton.Enabled = mOptionDataCRIO.CollectionTargetCRIO.Count > 1
        selectedRowIndex = 0
    End Sub
#End Region

#Region "Оброботчики событий пользователя"
    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        ' Переключить отображение ленты инструментов и отметку на соответствующем пункте меню
        ToolBarToolStripMenuItem.Checked = Not ToolBarToolStripMenuItem.Checked
        ToolStrip.Visible = ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        ' Переключить отображение ленты состояния и отметку на соответствующем пункте меню
        StatusBarToolStripMenuItem.Checked = Not StatusBarToolStripMenuItem.Checked
        StatusStrip.Visible = StatusBarToolStripMenuItem.Checked
    End Sub

    ''' <summary>
    ''' Включить или выключить отображение области проводника каналов. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ToggleFoldersVisible()
        ' Сначала переключите состояние отметки для соответствующего пункта меню
        ChannelsToolStripMenuItem.Checked = Not ChannelsToolStripMenuItem.Checked
        ' Синхронизировать кнопку "Папки" на панели инструментов
        ChannelsToolStripButton.Checked = ChannelsToolStripMenuItem.Checked
        ' Свернуть панель, на которой содержится дерево.
        SplitContainerTree.Panel1Collapsed = Not ChannelsToolStripMenuItem.Checked
        ' включить таймер обновления значений
        aTimerUpdataTree.Enabled = ChannelsToolStripMenuItem.Checked
    End Sub

    Private Sub FoldersToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ChannelsToolStripMenuItem.Click
        ToggleFoldersVisible()
    End Sub

    Private Sub FoldersToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ChannelsToolStripButton.Click
        ToggleFoldersVisible()
    End Sub

    ''' <summary>
    ''' Настройка вида (иконок) листа
    ''' </summary>
    ''' <param name="inView"></param>
    ''' <remarks></remarks>
    Private Sub SetView(inView As View)
        ' Определить, какой пункт меню должен быть отмечен
        Dim menuItemToCheck As ToolStripMenuItem

        Select Case inView
            Case View.Details
                menuItemToCheck = DetailsToolStripMenuItem
            Case View.LargeIcon
                menuItemToCheck = LargeIconsToolStripMenuItem
            Case View.List
                menuItemToCheck = ListToolStripMenuItem
            Case View.SmallIcon
                menuItemToCheck = SmallIconsToolStripMenuItem
            Case View.Tile
                menuItemToCheck = TileToolStripMenuItem
            Case Else
                Debug.Fail("Unexpected View")
                inView = View.Details
                menuItemToCheck = DetailsToolStripMenuItem
        End Select

        ' В меню "Представления" выбрать нужный пункт меню и отменить выбор остальных пунктов
        For Each menuItem As ToolStripMenuItem In ListViewToolStripButton.DropDownItems
            If menuItem Is menuItemToCheck Then
                menuItem.Checked = True
            Else
                menuItem.Checked = False
            End If
        Next

        ' В конце установить запрошенное представление
        ListView.View = inView
    End Sub

    Private Sub ListToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ListToolStripMenuItem.Click
        SetView(View.List)
    End Sub

    Private Sub DetailsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DetailsToolStripMenuItem.Click
        SetView(View.Details)
    End Sub

    Private Sub LargeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LargeIconsToolStripMenuItem.Click
        SetView(View.LargeIcon)
    End Sub

    Private Sub SmallIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SmallIconsToolStripMenuItem.Click
        SetView(View.SmallIcon)
    End Sub

    Private Sub TileToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileToolStripMenuItem.Click
        SetView(View.Tile)
    End Sub

    Private Sub ListContextMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ListContextMenu.Opening
        mnuRebootChassis.Enabled = ListView.SelectedItems.Count > 0
    End Sub

    Private Sub mnuRebootChassis_Click(sender As Object, e As EventArgs) Handles mnuRebootChassis.Click
        If ListView.SelectedItems.Count > 0 Then
            Dim lvItem As ListViewItem = ListView.SelectedItems(0)
            Dim KeyChassis As String = CStr(lvItem.Tag)
            Dim message As String = $"Вы действительно хотите перезагрузить {KeyChassis} шасси?"
            Dim caption As String = "Внимание!"
            Dim msgButtons As MessageBoxButtons = MessageBoxButtons.YesNo

            If MessageBox.Show(message, caption, msgButtons, MessageBoxIcon.Question) = DialogResult.Yes Then
                caption = NameOf(mnuRebootChassis_Click)
                Dim text As String = $"Перезагрузка {KeyChassis} шасси"
                RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{caption}> {text}")
                Dim tsk As Task = Task.Factory.StartNew(Sub() RebootChassis(KeyChassis))
                ' не ждём завершения задачи
            End If
        End If
    End Sub

    Private Sub mnuRebootAllChassis_Click(sender As Object, e As EventArgs) Handles mnuRebootAllChassis.Click
        InvokeRebootSSD()
    End Sub

    ''' <summary>
    ''' Просмотреть журнал исключений
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdViewLog_Click(sender As Object, e As EventArgs) Handles cmdViewLog.Click
        Dim frmExLogViewer As New FormExceptionLogViewer With {.ExceptionLogFile = ExceptionLogFile}

        Try
            frmExLogViewer.ShowDialog()
        Finally
            frmExLogViewer.Dispose()
        End Try
    End Sub
#End Region

#Region "Server"
#Region "Ручная посылка команд"
    Private Sub StartStopButton_Click(sender As Object, e As EventArgs) Handles StartStopButton.Click
        StartStopAcquisitionTargetCRIO(StartStopButton.Checked)
    End Sub

    ''' <summary>
    ''' Создание серверного слушающего сокета ИВК и связанного с ним потока.
    ''' При остановке - посылка шасси команды CommandSet.Stop_3.
    ''' </summary>
    ''' <param name="isStart"></param>
    ''' <remarks></remarks>
    Private Sub StartStopAcquisitionTargetCRIO(isStart As Boolean)
        StartStopButton.Checked = isStart

        If isStart Then
            StartStopButton.Text = "Стоп"
            StartStopButton.Image = My.Resources.Resources.Disconnect
            ' Ожидает подключения от TCP-клиентов сети.
            ssdListener = New TcpListener(System.Net.IPAddress.Any, CInt(SSDPortTextBox.Text))
            ssdListener.Server.NoDelay = True
            ' Использовать метод Start, чтобы начать ожидание входящих запросов подключения. 
            ' Start будет ставить входящие подключения в очередь до вызова метода Stop 
            ' или пока в очередь не будет поставлено MaxConnections подключений.
            ssdListener.Start()
            ListenForClient()

            For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
                grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusAdapterImage).Value = My.Resources.ledCornerGreen
                grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusReceiveImage).Value = My.Resources.ledCornerOrange
            Next

            ' запустить задачу обработки сообщений Клиентов
            TaskConnectionMontior = Task.Factory.StartNew(CType(AddressOf DoMonitorConnections, Action(Of Object)),
                                                          New MonitorInfo(ssdListener, dictConnInfoChassis),
                                                          TaskCreationOptions.LongRunning)
        Else
            SendCommandStopToClients()
            StartStopButton.Text = "Пуск"
            StartStopButton.Image = My.Resources.Resources.Connect

            If TaskConnectionMontior IsNot Nothing Then
                CType(TaskConnectionMontior.AsyncState, MonitorInfo).Cancel = True
            End If

            If ssdListener IsNot Nothing Then ssdListener.Stop()
            ssdListener = Nothing

            Dim ledGrayImage As Image = My.Resources.ledCornerGray
            For Each itemTargetCRIO As TargetCRIO In mOptionDataCRIO.CollectionTargetCRIO
                grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusAdapterImage).Value = ledGrayImage
                grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusSendImage).Value = ledGrayImage
                grdChassis.Rows(itemTargetCRIO.IndexRow).Cells(DataGridColumnEnum.StatusReceiveImage).Value = ledGrayImage
            Next
        End If
    End Sub
#End Region

#Region "Запуск Сбора на шасси"
    Private Sub ActivateTargetButton_Click(sender As Object, e As EventArgs) Handles ActivateTargetButton.Click
        ActivateTarget(ActivateTargetButton.Checked)
    End Sub

    ''' <summary>
    ''' Запуск/остановка сбора для всех шасси посылкой команды CommandSet.Activate_2
    ''' </summary>
    ''' <param name="inStart"></param>
    ''' <remarks></remarks>
    Private Sub ActivateTarget(inStart As Boolean)
        Me.ValidateChildren() ' Значение true, если все дочерние объекты были успешно проверены; в противном случае — false (там установка значения _ServerAddress)
        ActivateTargetButton.Checked = inStart

        If inStart Then
            ActivateTargetButton.Text = "Остановить Сбор"
            ActivateTargetButton.Image = My.Resources.Resources.Disconnect
            SendCommandLoopPeriodToClients(LoopPeriod)
        Else
            ActivateTargetButton.Text = "Запустить Сбор"
            ActivateTargetButton.Image = My.Resources.Resources.Connect
        End If

        SendCommandActivateToClients(inStart)
    End Sub

    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции ActivateTarget в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <param name="Start"></param>
    ''' <remarks></remarks>
    Private Sub InvokeActivateTarget(start As Boolean)
        Dim doInvokeActivate As New Action(Of Boolean)(AddressOf ActivateTarget)
        Invoke(doInvokeActivate, start)
    End Sub
#End Region

#Region "Асинхронное ожидание подключения клиента"
    ''' <summary>
    '''  Запуск асинхронного ожидания следующего подключения клиента.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ListenForClient()
        Dim infoConnWithServer As New ConnectionInfoServer(ssdListener, Me)
        ' Начинает асинхронную операцию, чтобы принять попытку входящего подключения.
        ssdListener.BeginAcceptTcpClient(AddressOf DoAcceptClient, infoConnWithServer)
    End Sub

    ''' <summary>
    ''' Обработать новое подключение клиента.
    ''' </summary>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Private Sub DoAcceptClient(result As IAsyncResult)
        Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)

        If monitorInfo.Listener IsNot Nothing AndAlso Not monitorInfo.Cancel Then
            Dim infoConnWithServer As ConnectionInfoServer = CType(result.AsyncState, ConnectionInfoServer)

            infoConnWithServer.AcceptClient(result) ' завершить асинхронную операцию
            ListenForClient() ' ждать следующего подключения клиента

            '--- TODO: Для отладки локального Клиента -------------------------
#If DEBUG_ClientTest = True Then
            infoConnWithServer.RemoteEndPointIPAddress = "127.0.0.1"
#Else
            infoConnWithServer.RemoteEndPointIPAddress = System.Net.IPAddress.Parse(CType(infoConnWithServer.Client.Client.RemoteEndPoint, IPEndPoint).Address.ToString()).ToString
#End If
            '------------------------------------------------------------------

            If infoConnWithServer.NameChassis Is Nothing Then
                infoConnWithServer.Client.Close() ' связанного шасси не обнаружено
            Else
                SyncLock HandlerLockObjectMonitorConn
                    If monitorInfo.Connections.ContainsKey(infoConnWithServer.NameChassis) Then
                        monitorInfo.Connections.Remove(infoConnWithServer.NameChassis)
                    End If
                    monitorInfo.Connections.Add(infoConnWithServer.NameChassis, infoConnWithServer) ' добавить клиента в список
                End SyncLock

                infoConnWithServer.AwaitData() ' а для нового клиента запустить чтение из входного потока

                ' Обновление GUI вынесено в отдельную задачу, иначе в потоке задачи SSDRUN происходит игнорирование подключения клиентов
                Dim tsk As Task = Task.Factory.StartNew(Sub() ShowNewClient(infoConnWithServer))
                tsk.Wait() ' без ожидания не работает
            End If
        End If
    End Sub

    ''' <summary>
    ''' Процедура ассинхронного выполнения для задачи обновления GUI при подклюении нового клиента (сокета шасси)
    ''' </summary>
    ''' <param name="infoConnWithServer"></param>
    ''' <remarks></remarks>
    Private Sub ShowNewClient(infoConnWithServer As ConnectionInfoServer)
        ' обновить сообщение на экране
        Dim doUpdateLabelConnectionCount As New Action(Of Integer)(AddressOf UpdateLabelConnectionCount)
        Invoke(doUpdateLabelConnectionCount, infoConnWithServer.IndexRow)

        ' Создать делегат для обновления пользоателького интерфейса в GUI
        Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
        Dim message As String = String.Format("Установлено соединение с {0}{1}Порт номер {2}",
                                                 System.Net.IPAddress.Parse(CType(infoConnWithServer.Client.Client.RemoteEndPoint, IPEndPoint).Address.ToString()),
                                                 Environment.NewLine, CType(infoConnWithServer.Client.Client.RemoteEndPoint, IPEndPoint).Port)
        Invoke(doAppendOutput, message, KeyRichTexServer, MessageBoxIcon.Information)
        Invoke(doAppendOutput, message, infoConnWithServer.IndexRow, MessageBoxIcon.Information)
        ShowMessageOnRichTextBoxClient(message, MessageBoxIcon.Information)

        Dim doUpdateLed As New Action(Of String, String, Integer)(AddressOf UpdateRowLedStatusReceive)
        Invoke(doUpdateLed, infoConnWithServer.Local.ToString, infoConnWithServer.Remote.ToString, infoConnWithServer.IndexRow)

        Dim caption As String = $"{NameOf(DoAcceptClient)}->{NameOf(ShowNewClient)}"
        Dim text As String = $"Установлено соединение с шасси: <{infoConnWithServer.NameChassis}> с IP адресом: {infoConnWithServer.RemoteEndPointIPAddress}"
        RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")
    End Sub

    ''' <summary>
    ''' Обновить метку числа соединений.
    ''' </summary>
    ''' <param name="IndexRow"></param>
    ''' <remarks></remarks>
    Private Sub UpdateLabelConnectionCount(indexRow As Integer)
        ToolStripStatusLabel.Text = $"{dictConnInfoChassis.Count} Подключено"
        If indexRow <> -1 Then grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusReceiveImage).Value = My.Resources.ledCornerRed
    End Sub

    ''' <summary>
    ''' Обновить таблицу клиентов при успешном новом соединении.
    ''' Не вызывать из другого потока, а из потока формы вызывать из Invoke().
    ''' </summary>
    ''' <param name="clientSocketClientRemoteEndPoint"></param>
    ''' <param name="clientSocketClientLocalEndPoint"></param>
    ''' <param name="IndexRow"></param>
    ''' <remarks></remarks>
    Private Sub UpdateRowLedStatusReceive(clientSocketClientRemoteEndPoint As String, clientSocketClientLocalEndPoint As String, indexRow As Integer)
        grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusReceiveImage).Value = My.Resources.ledCornerGreen
        grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusAdapterData).Value = $"local: {clientSocketClientRemoteEndPoint} remote: {clientSocketClientLocalEndPoint}"
    End Sub
#End Region

#Region "Обработки сообщений Клиентов"
    ''' <summary>
    ''' Выполняемая в потоке задача обработки сообщений Клиентов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoMonitorConnections()
        Const caption As String = NameOf(DoMonitorConnections)
        ' Создать делегат для обновления выходного дисплея
        Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
        ' Создать делегат для обновления метки числа соединений
        Dim doUpdateLabelLostConnectionCount As New Action(Of Integer)(AddressOf UpdateLabelLostConnectionCount)
        ' Получить MonitorInfo экземпляр из потока-хранителя Task экземпляра 
        Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)

        ' Протоколирование действий
        RegistrationEventLog.EventLog_MSG_CONNECT($"<{NameOf(DoMonitorConnections)} Монитор запущен...")
        Invoke(doAppendOutput, "Монитор запущен...", KeyRichTexServer, MessageBoxIcon.Information)

        Dim lostConnList As New List(Of String)

        ' осуществить соединение клиента в цикле обработки
        Do
            SyncLock HandlerLockObjectMonitorConn
                ' создать временный лист для записи закрытых соединений
                ' Проверить каждое соединение в обработке
                For Each infoConnWithServer As ConnectionInfoServer In monitorInfo.Connections.Values
                    If infoConnWithServer.Client.Connected Then
                        ' обработать соединение клиента
                        If infoConnWithServer.DataQueue.Count > 0 Then
                            ' в этом потоке обрабатываются накопленные в очереди сообщения по каждому клиенту, до тех пор пока очередь не опустеет
                            While infoConnWithServer.DataQueue.Count > 0
                                DisassemblePackFromChassisAndAnswer(infoConnWithServer, infoConnWithServer.DataQueue.Dequeue)
                            End While

                            ' TODO: Закоментировал, так как infoConnWithServer.AcquisitionQueue не используется для передачи внешнему Серверу
                            'Try
                            '    ' что принято сразу отдаём Серверу
                            '    If infoConnWithServer.AcquisitionQueue.Count > 0 Then
                            '        While infoConnWithServer.AcquisitionQueue.Count > 0
                            '            infoConnWithServer.AcquisitionQueue.Dequeue() ' TODO: массив из очереди куда-то добавляется, наверно в общий массив полученных данных
                            '        End While
                            '    End If
                            'Catch ex As SocketException
                            '    message = "Ошибка исключения сокета: " & ex.SocketErrorCode.ToString
                            '    RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {message}")
                            '    myThread = New Thread(AddressOf Me.ThreadFunction)
                            '    myThread.Start(message)
                            'Catch ex As Exception
                            '    message = ex.ToString()
                            '    RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {message}")
                            '    myThread = New Thread(AddressOf Me.ThreadFunction)
                            '    myThread.Start(message)
                            'End Try
                        End If
                    Else
                        ' Записать клиентов долго не соединявшихся или отсоединённых
                        lostConnList.Add(infoConnWithServer.NameChassis)
                    End If
                Next

                ' очистить любые закрытые соединения клиентов и удалить их из коллекции
                If lostConnList.Count > 0 Then
                    While lostConnList.Count > 0
                        ' обновления метки числа соединений
                        Invoke(doUpdateLabelLostConnectionCount, monitorInfo.Connections(lostConnList(0)).IndexRow)
                        monitorInfo.Connections.Remove(lostConnList(0))
                        lostConnList.RemoveAt(0)
                    End While
                End If
            End SyncLock

            ' Завершить цикл избегая напрасную трату времени CPU
            TaskConnectionMontior.Wait(1)
        Loop While Not monitorInfo.Cancel

        ' Закрыть все соединения прежде выхода из монитора
        For Each infoConnWithServer As ConnectionInfoServer In monitorInfo.Connections.Values
            infoConnWithServer.Client.Close()
        Next
        monitorInfo.Connections.Clear()

        Try
            ' при закрытии приложения может не сработать
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{NameOf(DoMonitorConnections)}> Монитор Остановлен.")
            ' обновить метку числа соединений и вывести в отчёт статус 
            Invoke(doUpdateLabelLostConnectionCount, -1)
            Invoke(doAppendOutput, "Монитор Остановлен.", KeyRichTexServer)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Обновить метку числа соединений в случае потери связи.
    ''' </summary>
    ''' <param name="IndexRow"></param>
    ''' <remarks></remarks>
    Private Sub UpdateLabelLostConnectionCount(indexRow As Integer)
        ToolStripStatusLabel.Text = $"{dictConnInfoChassis.Count - 1} Подключено"

        If indexRow <> -1 Then
            grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusReceiveImage).Value = My.Resources.ledCornerRed

            Dim message As String = "Разорвано соединение с шасси: " & ListView.Items(indexRow).Text ' grdChassis.Rows(IndexRow).Cells(0).Value.ToString()
            doAppendOutput(message, indexRow, MessageBoxIcon.Error) 'вывести сообщение
            doAppendOutput(message, KeyRichTexServer, MessageBoxIcon.Error)
            ShowMessageOnRichTextBoxClient(message, MessageBoxIcon.Information)

            Dim caption As String = $"{NameOf(DoMonitorConnections)}->{NameOf(UpdateLabelLostConnectionCount)}"
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {message}")
        End If
    End Sub

    ''' <summary>
    ''' Один раз в секунду обновить метку полученных пакетов.
    ''' </summary>
    ''' <param name="packetsReceive"></param>
    ''' <param name="indexRow"></param>
    ''' <remarks></remarks>
    Private Sub UpdateLabelPacketsReceive(packetsReceive As Long, indexRow As Integer)
        grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.PacketsReceiveData).Value = $"Получено: {packetsReceive} пакетов"
    End Sub

    ''' <summary>
    ''' Разбор пакета от клиентского сокета шасси.
    ''' Разобрать Пакет От Шасси И Ответить
    ''' </summary>
    ''' <param name="infoConnWithServer"></param>
    ''' <param name="MsgFromClientBytes"></param>
    ''' <remarks></remarks>
    Private Sub DisassemblePackFromChassisAndAnswer(infoConnWithServer As ConnectionInfoServer, msgFromClientBytes As Byte())
        Dim iCMD As Integer         ' номер команды
        Dim cmdSet As CommandSet    ' тип команды
        Dim bytesToRead As Integer  ' сколько байт прочитать для разбора
        Dim tempBytes As Byte()     ' временный буфер
        Dim CmdBodyLength As Integer ' длина тела данных
        Dim arrayLength As Integer  ' размерность массива полученного буфера собранных данных из заголовка для контроля с размерностью буфера, при конфигурировании шасси
        Dim offset As Integer       ' смещение позиции чтения
        Dim inputMsgText As String  ' имя команды или текст сообщения
        Dim countItteration As Integer ' для разорванного пакета
        Dim msgFromClientLength As Integer

        If msgFromClientBytes Is Nothing Then Exit Sub
        msgFromClientLength = msgFromClientBytes.Length

        ' проверить не было ли переноса остатка с предыдущей обработки
        If infoConnWithServer.LostBytes.Length > 0 Then
            ' объединить сохранённый остаток с поступившими данными
            msgFromClientBytes = (infoConnWithServer.LostBytes.Concat(msgFromClientBytes)).ToArray
            msgFromClientLength = msgFromClientBytes.Length
            infoConnWithServer.LostBytes = New Byte() {} ' буфер с разорванной командой очистить
        End If

        If msgFromClientLength > InfoLength Then ' как минимум 4 байта длина и 2 байта на номер команды
            Do
                ' считать первые 4 байт - длина тела данных
                bytesToRead = BodyPacketLength
                Re.Dim(tempBytes, bytesToRead - 1)

                If offset + bytesToRead > msgFromClientLength Then Exit Sub ' в случае приёма хрени от cRio(остатка от буфера) или конец цикла
                Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)

                offset += bytesToRead
                CmdBodyLength = BitConverter.ToInt32(tempBytes, 0) ' длина тела команды

                If CmdBodyLength <= 0 OrElse CmdBodyLength > MaxMsgFromClientLength Then Exit Sub ' в случае приёма хрени от cRio

                ' прочитать 2 байта номера команды
                bytesToRead = CommandLength
                Re.Dim(tempBytes, bytesToRead - 1)

                If offset + bytesToRead > msgFromClientLength Then Exit Sub ' в случае приёма хрени от cRio(остатка от буфера) или конец цикла

                Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                offset += bytesToRead
                iCMD = BitConverter.ToUInt16(tempBytes, 0) ' номер команды
                cmdSet = TypeCastCmd(iCMD, infoConnWithServer.ModeWork)
                ' выполнить действия в зависимости от типа команды или подготовить ответ

                Select Case cmdSet
                    Case CommandSet.GetListCommand_0
                        ' прочитать тело сообщения
                        bytesToRead = CmdBodyLength - CommandLength ' 2 без номера команды

                        ' условие для этой команды не выполнено, скорее всего ошибка по сети
                        If bytesToRead <> 14 AndAlso CmdBodyLength <> 16 Then Exit Sub ' в случае приёма хрени от cRio

                        tempBytes = New Byte(bytesToRead - 1) {}

                        Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                        offset += bytesToRead
                        inputMsgText = ASCII_Encoding.GetString(tempBytes)

                        ' подготовить и отослать клиенту список разрешенных команд
                        SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.GetMeta_1, ResponseMetaXML(infoConnWithServer.ModeWork))

                        ManagerChassis.GetMetaChassisSuccess(infoConnWithServer.NameChassis, True)
                        ' Протоколирование действий закоментировать в релизе
                        Invoke(doAppendOutput, inputMsgText, infoConnWithServer.IndexRow, MessageBoxIcon.Information)

                        Exit Select
                    Case CommandSet.Acquisition_4
                        If IsSuccessRequestMeta(infoConnWithServer.NameChassis) = False Then Exit Sub

                        ' если длина (msgFromClientLength) поступившего сообщения не кратна (меньше) длине пакета (CmdBodyLength + BodyPacketLength),
                        ' значит пакет разорван. Сохранить эту разорванную часть в info.LostBytes для последующего соединения с новым пакетом.
                        If msgFromClientLength < (CmdBodyLength + BodyPacketLength) * (countItteration + 1) Then ' + conLengthCommand 
                            Dim statrComm As Integer = offset - BodyPacketLength - CommandLength
                            ' тело команды разорвано между пакета
                            ' запомнить остаток в infoConnWithServer.LostBytes
                            Dim lenght As Integer = msgFromClientLength - statrComm
                            Re.Dim(infoConnWithServer.LostBytes, lenght - 1)
                            Array.Copy(msgFromClientBytes, statrComm, infoConnWithServer.LostBytes, 0, lenght)
                            Exit Sub
                        End If

                        ' прочитать 2 байта - размерность массива
                        bytesToRead = ArraySizeLength
                        Re.Dim(tempBytes, bytesToRead - 1)
                        Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                        offset += bytesToRead
                        arrayLength = BitConverter.ToUInt16(tempBytes, 0) ' размерность массива

                        If arrayLength <> infoConnWithServer.ArrayLength Then ' выдать предупреждение
                            ' Протоколирование действий
                            ' вывести предупреждение 10 раз
                            If infoConnWithServer.CountWarning < 10 Then
                                Dim text As String = $"Число переданных каналов от шасси: <{infoConnWithServer.RemoteEndPointIPAddress}> равно: {arrayLength} и не соответствует конфигурации"

                                If infoConnWithServer.CountWarning = 0 Then
                                    Const caption As String = NameOf(DisassemblePackFromChassisAndAnswer)
                                    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                                End If

                                Invoke(doAppendOutput, text, infoConnWithServer.IndexRow, MessageBoxIcon.Error)
                                infoConnWithServer.CountWarning += 1
                            End If

                            Exit Sub
                        Else
                            ' заполнить массив
                            Re.Dim(tempBytes, DoubleBytes - 1)
                            ' прочитать 8 байта - тип Double;  4 байта - тип Single
                            bytesToRead = DoubleBytes

                            For I = 0 To arrayLength - 1
                                If offset + bytesToRead > msgFromClientLength Then Exit For ' в случае приёма хрени от cRio (остатка от буфера) или конец цикла

                                Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                                infoConnWithServer.AcquisitionData(I) = BitConverter.ToDouble(tempBytes, 0)
                                offset += bytesToRead
                            Next

                            If arrayLength < MaxChannelsOnShassis Then ' если массив меньше 270 элементов добрать остаток
                                offset += (MaxChannelsOnShassis - arrayLength) * bytesToRead
                            End If

                            ' добавить массив значений каналов в очередь для ConnectionInfoServer данного шасси
                            ' возможно надо синхронизироваться с цикл сбора первым шасси 
                            ' TODO: Закоментировал, так как infoConnWithServer.AcquisitionQueue не используется для передачи внешнему Серверу
                            'infoConnWithServer.AcquisitionQueue.Enqueue(infoConnWithServer.AcquisitionData) 'TODO: проверить необходимость AcquisitionQueue и всё что с ним связано

                            ' подсчёт количества принятых пакетов от шасси
                            infoConnWithServer.PacketsThresholdCount += 1
                            If infoConnWithServer.PacketsThresholdCount >= thresholdPacketsCount Then
                                infoConnWithServer.PacketsReceive += thresholdPacketsCount
                                ' обновить сообщение на экране
                                Dim doUpdateLabelPacketsReceive As New Action(Of Long, Integer)(AddressOf UpdateLabelPacketsReceive)
                                Invoke(doUpdateLabelPacketsReceive, infoConnWithServer.PacketsReceive, infoConnWithServer.IndexRow)
                                infoConnWithServer.PacketsThresholdCount = 0
                            End If
                        End If

                        Exit Select
                    Case CommandSet.InitSuccess_7, CommandSet.InitBad_8, CommandSet.LaunchSuccess_9, CommandSet.LaunchBad_10, CommandSet.CriticalError_11
                        If IsSuccessRequestMeta(infoConnWithServer.NameChassis) = False Then Exit Sub

                        ' прочитать тело сообщения
                        bytesToRead = CmdBodyLength - CommandLength ' 2 без номера команды
                        tempBytes = New Byte(bytesToRead - 1) {}

                        Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                        offset += bytesToRead

                        If IsRestartAllChassisSuccess Then ' чтобы не принимать команды оставшиеся после ручной перезагрузки шасси
                            inputMsgText = ASCII_Encoding.GetString(tempBytes)
                            Select Case cmdSet
                                Case CommandSet.InitSuccess_7
                                    ManagerChassis.InitializeChassisSuccess(infoConnWithServer.NameChassis, True)
                                    Exit Select
                                Case CommandSet.InitBad_8
                                    ManagerChassis.InitializeChassisBad(infoConnWithServer.NameChassis, True)
                                    Exit Select
                                Case CommandSet.LaunchSuccess_9
                                    ManagerChassis.LaunchChassisSuccess(infoConnWithServer.NameChassis, True)
                                    Exit Select
                                Case CommandSet.LaunchBad_10
                                    ManagerChassis.LaunchChassisBad(infoConnWithServer.NameChassis, True)
                                    Exit Select
                                Case CommandSet.CriticalError_11
                                    ManagerChassis.CriticalErrorChassis(infoConnWithServer.NameChassis, inputMsgText)
                                    Exit Select
                            End Select
                            ' вывести после
                            Invoke(doAppendOutput, inputMsgText, infoConnWithServer.IndexRow, MessageBoxIcon.Information)
                        End If

                        Exit Select
                    Case CommandSet.Lost_Samples_13
                        ' Резер - команда отсылаемая Клиентом о пропущенных буферах при сборе между детерминированным циклом опроса железа
                        ' и циклом передачи данных в сеть (ошибка -2221 буфера Acquisition). В теле содержится I32 - число
                        If IsSuccessRequestMeta(infoConnWithServer.NameChassis) = False Then Exit Sub

                        Dim lostSamples As Integer
                        ' прочитать тело сообщения
                        bytesToRead = CmdBodyLength - CommandLength ' 4 - Int32 - Lost_Samples
                        tempBytes = New Byte(bytesToRead - 1) {}

                        Array.Copy(msgFromClientBytes, offset, tempBytes, 0, bytesToRead)
                        offset += bytesToRead
                        lostSamples = BitConverter.ToInt32(tempBytes, 0)

                        Invoke(doAppendOutput, $"Для шасси: <{infoConnWithServer.RemoteEndPointIPAddress}> число потерянных циклов составило: {lostSamples}  ", infoConnWithServer.IndexRow, MessageBoxIcon.Exclamation)
                        Exit Select
                    Case CommandSet.Unknown
                        offset = msgFromClientLength ' чтобы выйти из цикла DO...Loop While

                        ' вывести предупреждение 10 раз
                        If infoConnWithServer.CountWarning < 10 Then
                            Dim text As String = $"{CRITICAL_CMD_ERROR}: {infoConnWithServer.RemoteEndPointIPAddress} "

                            If infoConnWithServer.CountWarning = 0 Then
                                Const caption As String = NameOf(DisassemblePackFromChassisAndAnswer)
                                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                            End If

                            Invoke(doAppendOutput, text, infoConnWithServer.IndexRow, MessageBoxIcon.Error)
                            infoConnWithServer.CountWarning += 1
                        End If

                        Exit Select
                End Select

                countItteration += 1
            Loop While offset < msgFromClientLength ' обработать следующую команду накопленную в очереди команд 
        End If
    End Sub

    ''' <summary>
    ''' Это первая команда после успешного запуска шасси.
    ''' Не разрешать обрабатывать команды пришедшие в другой последовательности.
    ''' Для блокировки приёма от шасси некорректных запросов
    ''' или отсечения запросов от исключённых из кофигурации шасси,
    ''' но питание которых не снято.
    ''' Проверка Выполнения Запроса Списка Команд
    ''' </summary>
    ''' <param name="infoHostName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsSuccessRequestMeta(infoHostName As String) As Boolean
        If ManagerChassis.Chassis.ContainsKey(infoHostName) Then
            Return ManagerChassis.Chassis(infoHostName).GetMetaSuccess
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' В зависимости от режима работы шасси по индексу команды выдать перечислитель данной команды.
    ''' </summary>
    ''' <param name="iCMD"></param>
    ''' <param name="infoModeWork"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TypeCastCmd(iCMD As Integer, infoModeWork As EnumModeWork) As CommandSet
        Dim cmdSetUnknown As CommandSet = CommandSet.Unknown

        Select Case infoModeWork
            Case EnumModeWork.Measurement
                If iCMD <= ArrEnumForMeasurement.Length - 1 Then
                    Return ArrEnumForMeasurement(iCMD)
                Else
                    Return cmdSetUnknown
                End If
            'Case EnumModeWork.Control
            '    If iCMD <= ArrEnumForControl.Length - 1 Then
            '        Return ArrEnumForControl(iCMD)
            '    Else
            '        Return cmdSetUnknown
            '    End If
            Case EnumModeWork.Unknown
                Return cmdSetUnknown
            Case Else
                Return cmdSetUnknown
        End Select
    End Function
#End Region

#Region "Команды отсылаемые Сервером"
    ' Когда Visual Basic преобразует значения числовых типов данных в Boolean, 0 становится False, а все остальные значения — True. 
    ' Когда Visual Basic преобразует значения Boolean в числовые типы, False становится 0, а True становится -1. 
    ''' <summary>
    ''' Послать Клиентам Команду Стоп
    ''' </summary>
    Private Sub SendCommandStopToClients()
        If TaskConnectionMontior IsNot Nothing AndAlso CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim YesNo As Short = CShort(vbTrue) ' TriState.True
            Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' тело сообщения
            Dim listKeysConnInfo As List(Of String) = monitorInfo.Connections.Keys.ToList

            For Each key As String In listKeysConnInfo
                Dim infoConnWithServer As ConnectionInfoServer = monitorInfo.Connections(key)

                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Stop_3, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда остановиться",
                           infoConnWithServer.IndexRow,
                           MessageBoxIcon.Exclamation)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Послать Клиентам Команду LoopPeriod
    ''' </summary>
    ''' <param name="loopPeriod"></param>
    Private Sub SendCommandLoopPeriodToClients(loopPeriod As UInt32)
        If CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim messageData As Byte() = BitConverter.GetBytes(loopPeriod) ' тело сообщения

            For Each infoConnWithServer As ConnectionInfoServer In monitorInfo.Connections.Values
                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Loop_Period_6, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда установить период сбора {loopPeriod} ms",
                           infoConnWithServer.IndexRow,
                           MessageBoxIcon.Information)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Послать Клиентам Команду Activate
    ''' </summary>
    ''' <param name="isActivate"></param>
    Private Sub SendCommandActivateToClients(isActivate As Boolean)
        If TaskConnectionMontior IsNot Nothing AndAlso CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim YesNo As Short = CShort(If(isActivate, vbTrue, vbFalse))
            Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' тело сообщения

            For Each infoConnWithServer As ConnectionInfoServer In monitorInfo.Connections.Values
                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Activate_2, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда {If(isActivate, "запуска", "остановки")} сбора",
                           infoConnWithServer.IndexRow,
                           If(isActivate, MessageBoxIcon.Information, MessageBoxIcon.Exclamation))
                End If
                UpdateRowLedSendDataServer(isActivate, infoConnWithServer.IndexRow)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Индикация отправки команды Activate.
    ''' </summary>
    ''' <param name="isActivate"></param>
    ''' <param name="IndexRow"></param>
    ''' <remarks></remarks>
    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    Private Sub UpdateRowLedSendDataServer(isActivate As Boolean, indexRow As Integer)
        If isActivate Then
            grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusSendImage).Value = My.Resources.ledCornerGreen
        Else
            grdChassis.Rows(indexRow).Cells(DataGridColumnEnum.StatusSendImage).Value = My.Resources.ledCornerRed
        End If
    End Sub

    ''' <summary>
    ''' Послать Одному Клиенту Команду Стоп
    ''' </summary>
    ''' <param name="keyChassis"></param>
    Public Sub SendCommandStopToConcreteClient(keyChassis As String)
        If CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim YesNo As Short = CShort(vbTrue) ' TriState.True
            Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' тело сообщения

            If monitorInfo.Connections.ContainsKey(keyChassis) Then
                Dim infoConnWithServer As ConnectionInfoServer = monitorInfo.Connections(keyChassis)

                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Stop_3, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда остановиться",
                           infoConnWithServer.IndexRow,
                           MessageBoxIcon.Exclamation)
                    infoConnWithServer.Client.Close()
                    monitorInfo.Connections.Remove(keyChassis)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Послать Одному Клиенту Команду LoopPeriod
    ''' </summary>
    ''' <param name="keyChassis"></param>
    ''' <param name="loopPeriod"></param>
    Public Sub SendCommandLoopPeriodToConcreteClient(keyChassis As String, loopPeriod As UInt32)
        If CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim messageData As Byte() = BitConverter.GetBytes(loopPeriod) ' тело сообщения

            If monitorInfo.Connections.ContainsKey(keyChassis) Then
                Dim infoConnWithServer As ConnectionInfoServer = monitorInfo.Connections(keyChassis)
                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Loop_Period_6, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда установить период сбора {loopPeriod} ms",
                           infoConnWithServer.IndexRow,
                           MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Послать Одному Клиенту Команду Activate
    ''' </summary>
    ''' <param name="keyChassis"></param>
    ''' <param name="isActivate"></param>
    Public Sub SendCommandActivateToConcreteClient(keyChassis As String, isActivate As Boolean)
        If CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)
            Dim YesNo As Short = CShort(If(isActivate, vbTrue, vbFalse))
            Dim messageData As Byte() = BitConverter.GetBytes(YesNo) ' тело сообщения

            If monitorInfo.Connections.ContainsKey(keyChassis) Then
                Dim infoConnWithServer As ConnectionInfoServer = monitorInfo.Connections(keyChassis)
                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.Activate_2, messageData)
                    Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
                    Invoke(doAppendOutput,
                           $"Шасси {infoConnWithServer.RemoteEndPointIPAddress} послана команда {If(isActivate, "запуска", "остановки")} сбора",
                           infoConnWithServer.IndexRow,
                           MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

#Region "WorkCheckConnectionWithClients"
    Private messageTestBytes As Byte() = ASCII_Encoding.GetBytes("Test") ' тело сообщения

    ''' <summary>
    ''' Посылка необрабатываемой команды на шасси, для проверки соединения с клиентом.
    ''' Послать Клиентам Команду CheckConnection
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendCommandCheckConnectionToClients()
        ' Здесь возможно будет ошибка, может быть надо вставить обработчик ошибки.
        If CType(TaskConnectionMontior.AsyncState, MonitorInfo).Listener IsNot Nothing Then
            Dim monitorInfo As MonitorInfo = CType(TaskConnectionMontior.AsyncState, MonitorInfo)

            For Each infoConnWithServer As ConnectionInfoServer In monitorInfo.Connections.Values
                If infoConnWithServer.Client.Connected Then
                    SendAnyCommandToConcreteClient(infoConnWithServer, CommandSet.CheckConnection_14, messageTestBytes)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Задача проверка соединений с клиентами.
    ''' Пустая команда, отправляется клиенту 1 раз в секунду  и служит как маркер проверки связи. 
    ''' В случае отсоединения клиента произойдёт разрыв соединения
    ''' </summary>
    ''' <param name="ct"></param>
    ''' <remarks></remarks>
    Public Sub WorkCheckConnectionWithClients(ct As CancellationToken)
        ' Прерывание было запрошено до запуска?
        If ct.IsCancellationRequested Then ct.ThrowIfCancellationRequested()

        Do
            If ct.IsCancellationRequested Then
                Exit Do ' завершить задачу
            End If

            SendCommandCheckConnectionToClients()
            Sleep(1000)
        Loop While True
    End Sub
#End Region

    ''' <summary>
    ''' Упаковка и отправка команд.
    ''' Послать Клиенту Команду
    ''' </summary>
    ''' <param name="infoConnWithServer"></param>
    ''' <param name="CmdSet"></param>
    ''' <param name="BodyMessageData"></param>
    ''' <remarks></remarks>
    Private Sub SendAnyCommandToConcreteClient(infoConnWithServer As ConnectionInfoServer, cmdSet As CommandSet, bodyMessageData() As Byte)
        Dim listPacketOut As New List(Of Byte)
        Dim pakcetOutByte As Byte() = BitConverter.GetBytes(cmdSet)             ' номер команды

        pakcetOutByte = (pakcetOutByte.Concat(bodyMessageData)).ToArray()       ' соединить тело сообщения
        listPacketOut.AddRange(BitConverter.GetBytes(pakcetOutByte.Length))     ' пакет начинается с длины команды
        listPacketOut.AddRange(pakcetOutByte)                                   ' длина команды + номер команды + тело сообщения
        pakcetOutByte = listPacketOut.ToArray
        infoConnWithServer.Stream.Write(pakcetOutByte, 0, pakcetOutByte.Length) ' отправить клиенту
    End Sub

    ''' <summary>
    ''' Подготовить пакет Клиенту на список каналов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ResponseMetaXML(modeWork As EnumModeWork) As Byte()
        Dim commandsMetaXML As XElement = Nothing

        Select Case modeWork
            Case EnumModeWork.Measurement
                commandsMetaXML = MakeXElement(New List(Of String)(ArrCmdForMeasurement))
                'Case EnumModeWork.Control
                '    commandsMetaXML = MakeXElement(New List(Of String)(ArrCmdForControl))
        End Select

        Return ASCII_Encoding.GetBytes(commandsMetaXML.ToString)
    End Function

    ''' <summary>
    ''' Создать XML элемент с составом специфичных для режима работы шасси команд
    ''' </summary>
    ''' <param name="listCmd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MakeXElement(listCmd As List(Of String)) As XElement
        Dim xmlArray As XElement = New XElement("Array")

        xmlArray.Add(New XElement("Name", "Meta data"))
        xmlArray.Add(New XElement("Dimsize", listCmd.Count))

        For Each strCmd As String In listCmd
            Dim xmlString As XElement = New XElement("String")
            xmlString.Add(New XElement("Name", "Name"))
            xmlString.Add(New XElement("Val", strCmd))

            Dim xmlCluster As XElement = New XElement("Cluster")
            xmlCluster.Add(New XElement("Name", "Meta Element"))
            xmlCluster.Add(New XElement("NumElts", 1))
            xmlCluster.Add(xmlString)

            xmlArray.Add(xmlCluster)
        Next

        Return xmlArray
    End Function
#End Region
#End Region

#Region "Клиент к Registration"
    Private Sub SSDPortTextBox_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SSDPortTextBox.Validating
        Dim deltaPort As Integer
        If Not Integer.TryParse(SSDPortTextBox.Text, deltaPort) OrElse deltaPort < 1 OrElse deltaPort > 65535 Then
            MessageBox.Show("Порт обязан быть числом между 1 и 65535.", "Неправильный номер порта", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            SSDPortTextBox.SelectAll()
            e.Cancel = True
        End If
    End Sub

#Region "Функции обратного вызова из потока Клиента или Task"
    ' InvokeAppendOutput метод может легко быть замещён лямбда методом 
    ' подходящем  к ConnectionInfo конструктору в обработчике события ConnectButton_CheckedChanged

    ''' <summary>
    ''' Вызвать запуск серверной (слушающей) части ИВК из потока метода SSDRun
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InvokeStartAcquisitionTargetCRIO()
        Sleep(WaiteStartStopAcquisition)
        Dim doStartAcquisitionTargetCRIO As New Action(Of Boolean)(AddressOf StartStopAcquisitionTargetCRIO)
        Invoke(doStartAcquisitionTargetCRIO, True)
    End Sub

#Region "Протоколирование"
    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции AppendMsgToRichTextBoxByKey в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="RichTextBoxKey"></param>
    ''' <param name="SelectionMode"></param>
    ''' <remarks></remarks>
    Public Sub InvokeAppendMsgToRichTextBoxByKey(message As String, richTextBoxKey As Integer, selectionMode As MessageBoxIcon)
        Dim doAppendOutputServer As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxByKey)
        Invoke(doAppendOutputServer, message, richTextBoxKey, selectionMode)
    End Sub

    ''' <summary>
    ''' Добавить сообщение в закладку клиента шасси
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="RichTextBoxKey"></param>
    ''' <param name="SelectionMode"></param>
    ''' <remarks></remarks>
    Private Sub AppendMsgToRichTextBoxByKey(message As String, richTextBoxKey As Integer, selectionMode As MessageBoxIcon)
        Dim tempRichTextBox As RichTextBox = RichTextBoxes(richTextBoxKey)

        If tempRichTextBox.TextLength > 0 Then tempRichTextBox.AppendText(ControlChars.NewLine)

        tempRichTextBox.AppendText($"{DateTime.Now.ToLongTimeString} {message}")
        WriteTextToRichTextBox(tempRichTextBox, message, selectionMode)
        tempRichTextBox.ScrollToCaret()
    End Sub

    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции AppendMsgToRichTextBoxClient в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="SelectionMode"></param>
    ''' <remarks></remarks>
    Public Sub InvokeAppendMsgToRichTextBoxClient(message As String, selectionMode As MessageBoxIcon)
        Dim doAppendOutput As New Action(Of String, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxClient)
        Invoke(doAppendOutput, message, selectionMode)
    End Sub

    ''' <summary>
    ''' Добавить сообщение в закладку клиента Сервера
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="SelectionMode"></param>
    ''' <remarks></remarks>
    Private Sub AppendMsgToRichTextBoxClient(message As String, selectionMode As MessageBoxIcon)
        If RichTextBoxClient.TextLength > 0 Then RichTextBoxClient.AppendText(ControlChars.NewLine)

        RichTextBoxClient.AppendText($"{DateTime.Now.ToLongTimeString} {message}")
        WriteTextToRichTextBox(RichTextBoxClient, message, selectionMode)
        RichTextBoxClient.ScrollToCaret()
    End Sub

    Private Shared Sub WriteTextToRichTextBox(richTextBox As RichTextBox, message As String, selectionMode As MessageBoxIcon)
        'RichTextBox.SelectedText = message + ControlChars.Cr
        richTextBox.SelectionStart = If(richTextBox.TextLength - message.Length < 0, 0, richTextBox.TextLength - message.Length)
        richTextBox.SelectionLength = message.Length

        Select Case selectionMode
            Case MessageBoxIcon.Information 'i
                richTextBox.SelectionFont = New Font(FontFamily.GenericSansSerif, 10.0F)
                richTextBox.SelectionColor = Color.Blue
            Case MessageBoxIcon.Error
                richTextBox.SelectionFont = New Font("Arial", 10, FontStyle.Bold)
                richTextBox.SelectionColor = Color.Red
            Case MessageBoxIcon.Exclamation '!
                richTextBox.SelectionFont = New Font("Verdana", 10)
                richTextBox.SelectionColor = Color.Magenta
            Case MessageBoxIcon.Question '?
                richTextBox.SelectionFont = New Font("Arial", 10)
                richTextBox.SelectionColor = Color.Green
            Case Else
                richTextBox.SelectionFont = New Font("Arial", 10)
                richTextBox.SelectionColor = Color.Green
        End Select
    End Sub

    ''' <summary>
    ''' Сообщение на закладку сообщений и на панель управления
    ''' Инкапсулирует вызова делегата для функции AppendMsgToRichTextBoxClient в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <param name="inputMsgText"></param>
    ''' <param name="selectionMode"></param>
    ''' <remarks></remarks>
    Private Sub ShowMessageOnRichTextBoxClient(inputMsgText As String, selectionMode As MessageBoxIcon)
        Dim doAppendOutput As New Action(Of String, MessageBoxIcon)(AddressOf AppendMsgToRichTextBoxClient)
        Invoke(doAppendOutput, inputMsgText, selectionMode)
    End Sub
#End Region

#Region "Обработчики обратных вызовов Клиента"
    Private isSSDShutDown As Boolean = False
    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции CloseSSDdelegate в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InvokeCloseSSD()
        Dim doCloseSSD As New System.Action(AddressOf CloseSSDdelegate)
        Invoke(doCloseSSD)
    End Sub

    ''' <summary>
    ''' Закрыть ИВК (приложение)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CloseSSDdelegate()
        isSSDShutDown = True
        Close()
    End Sub

    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции RebootSSDdelegate в том потоке,
    ''' которому принадлежит основной дескриптор окна элемента управления,
    ''' с указанным списком аргументов. 
    ''' </summary>
    Private Sub InvokeRebootSSD()
        If IsStartAcquisition Then InvokeStartAcquisition(False) ' остановить запись
        Dim doRebootSSD As New System.Action(AddressOf RebootSSDdelegate)
        Invoke(doRebootSSD)
    End Sub

    ''' <summary>
    ''' Перезагрузка ИВК
    ''' В случае если поступила команда от сервера ИВК о необходимости перезагрузки ИВК, то производится выход из цикла без сбрасывания глобального флага работы ИВК. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RebootSSDdelegate()
        StopAllConnections()
        InitializeFormAgain()
    End Sub

    Private Sub ApplyCommandButton_Click(sender As Object, e As EventArgs) Handles ApplyCommandButton.Click
        If CmbBoxCommand.Text = ACQUISITION_START OrElse CmbBoxCommand.Text = ACQUISITION_STOP Then
            SetStartStopRecord()
        End If
    End Sub

    Private Sub SetStartStopRecord()
        Select Case CmbBoxCommand.Text
            Case ACQUISITION_START
                StartAcquisitionActivateDelegate(True)
            Case ACQUISITION_STOP
                StartAcquisitionActivateDelegate(False)
        End Select
    End Sub

    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции StartAcquisitionActivateDelegate в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <param name="isActivate"></param>
    ''' <remarks></remarks>
    Private Sub InvokeStartAcquisition(isActivate As Boolean)
        Dim doStartAcquisition As New Action(Of Boolean)(AddressOf StartAcquisitionActivateDelegate)
        Invoke(doStartAcquisition, isActivate)
    End Sub

    ''' <summary>
    ''' Включение/Выключение записи на ИВК
    ''' </summary>
    ''' <param name="isActivate"></param>
    ''' <remarks></remarks>
    Private Sub StartAcquisitionActivateDelegate(isActivate As Boolean)
        Dim text As String
        Dim mImage As Image

        If isActivate Then
            text = "Запущена передача замеров на Registration"
            mImage = My.Resources.AcquisitionStart
            StartAcquisitionTimer()
        Else
            StopAcquisitionTimer()
            text = "Остановлена передача замеров на Registration"
            mImage = My.Resources.AcquisitionStop
        End If

        AppendMsgToRichTextBoxClient(text, MessageBoxIcon.Information)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StartAcquisitionActivateDelegate)}> {text}")
        TSStatusLabelRecord.Text = text
        TSStatusLabelRecord.Image = mImage
    End Sub
#End Region
#End Region

    ''' <summary>
    ''' Инкапсулирует вызова делегата для функции PopulateChannelsDelegate в том потоке, 
    ''' которому принадлежит основной дескриптор окна элемента управления, 
    ''' с указанным списком аргументов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InvokePopulateChannels()
        Dim doPopulateChannelsDelegate As New System.Action(AddressOf PopulateChannelsDelegate)
        Invoke(doPopulateChannelsDelegate)
    End Sub

    ''' <summary>
    ''' Обновление столбцов ListView числом параметров сгруппироваными по типу сигнала
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateChannelsDelegate()
        If isInitializeSuccess = False Then
            PopulateListViewItems()
            PopulateChannelsNodes()
            ApplyCommandButton.Enabled = True
            isInitializeSuccess = True
        End If
    End Sub

    ''' <summary>
    ''' Выбрать собранные данные в общий массив с каждого шасси.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mmTimerChassisAcquisition100Hz_Tick(sender As Object, e As EventArgs) Handles mmTimerChassisAcquisition100Hz.Tick
        Dim sync As Integer = Interlocked.CompareExchange(syncTimerAcquisition100Hz, 1, 0)

        If sync = 0 Then
            Try
                If IsStartAcquisition Then
                    Dim indexChannel As Integer = 0

                    If counterBuffer >= RefreshScreen Then ' на 1 больше, чем RefreshScreen
                        For I As Integer = 0 To LengthArrChannelsAllChassis - 1
                            If RefreshScreen = 1 Then
                                AcquisitionAverageChassis(I) = AcquisitionMemo(I)
                            Else
                                AcquisitionAverageChassis(I) = AcquisitionMemo(I) / RefreshScreen
                            End If

                            AcquisitionMemo(I) = 0
                        Next

                        counterBuffer = 0
                    End If

                    ' выбрать собранные данные в общий массив с каждого шасси
                    ' порядок следования сокетов ConnectionInfoServer в _Connections не соответствует порядку следования
                    ' шасси itemChassis в varManagerChassis.CollectionsChassisClass.Values
                    For Each itemChassis As Chassis In ManagerChassis.Chassis.Values

                        'Dim keyChassis As String = itemChassis.HostName
                        '' есть ли сокет в подключениях
                        'If dictConnInfoChassis.ContainsKey(keyChassis) Then
                        '    Dim infoConnWithServer As ConnectionInfoServer = dictConnInfoChassis(keyChassis)
                        '    For I As Integer = 0 To infoConnWithServer.ArrayLength - 1
                        '        'If isSolvePolinomForRecord Then
                        '        '    AcquisitionMemo(IndexChannelTemp) += PhysicalApproximation(infoConnWithServer.AcquisitionData(I),
                        '        '                                     GManagerChassis.Chassis(infoConnWithServer.NameChassis).GManagerChannels.Channels(infoConnWithServer.ListChannelsToAcquire(I)).LevelOfApproximation,
                        '        '                                     GManagerChassis.Chassis(infoConnWithServer.NameChassis).GManagerChannels.Channels(infoConnWithServer.ListChannelsToAcquire(I)).Polynome)
                        '        'Else
                        '        AcquisitionMemo(indexChannel) += infoConnWithServer.AcquisitionData(I)
                        '        'End If
                        '        indexChannel += 1
                        '    Next
                        'Else
                        '    ' если шасси отключилось то индекс элемента в AcquisitionDataAllChassis должен перепрыгнуть на число каналов в отключившемся шасси
                        '    indexChannel += itemChassis.ChannelsCount()
                        'End If

                        '---------------------
                        ' TODO: Здесь вместо infoConnWithServer.AcquisitionData(I) генерируется RandomNextDouble(indexChannel)
                        For I As Integer = 0 To itemChassis.ChannelsCount - 1
                            AcquisitionMemo(indexChannel) += RandomNextDouble(indexChannel) ' rnd.NextDouble ' counterBuffer + 1
                            indexChannel += 1
                        Next
                        '---------------------
                    Next
                    counterTimer += 1 ' TODO: удалить
                    counterBuffer += 1
                Else
                    ' если во время сбора была ошибка, то стоп
                    If IsStartAcquisition Then
                        mmTimerChassisAcquisition100Hz.Stop()
                        ClearAcquisitionTimerChassis()
                        IsStartAcquisition = False
                    End If
                End If
            Catch ex As Exception
                mmTimerChassisAcquisition100Hz.Stop()
                ClearAcquisitionTimerChassis()
                MessageBox.Show(ex.Message, $"Error <{NameOf(mmTimerChassisAcquisition100Hz_Tick)}>", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Finally
                syncTimerAcquisition100Hz = 0  ' освободить
            End Try
        End If
    End Sub

    '''' <summary>
    '''' Временный флаг для вывода значений на экран.
    '''' TODO: Убрать
    '''' </summary>
    'Private IsShowAsquiredData As Boolean

    '''' <summary>
    '''' Вызов события передачи данных в Registration
    '''' </summary>
    'Private Sub SendAcquisitionDataToRegistration()
    '    ' TODO: вызвать событие с отсеянными каналами не включёнными в конфигурацию опроса
    '    'If IsShowAsquiredData Then
    '    '    OnAcquireData(New AcquireDataEventArgs With {.AcquisitionData = AcquisitionAverage})
    '    'End If
    '    'IsShowAsquiredData = False
    'End Sub

    'Protected Overridable Sub OnAcquireData(e As AcquireDataEventArgs)
    '    RaiseEvent AcquireData(Me, e)
    'End Sub

    '''' <summary>
    '''' Подписка на событие передачи данных в Registration
    '''' Тест, на самом деле нужна ручная подписка.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub FormCompactRio_AcquireData(sender As Object, e As AcquireDataEventArgs) Handles Me.AcquireData
    '    Dim I As Integer
    '    Dim AsquiredDataStringBuilder As New StringBuilder()

    '    For Each itemChassis As Chassis In GManagerChassis.Chassis.Values
    '        For Each channel As Channel In itemChassis.GManagerChannels.Channels.Values
    '            AsquiredDataStringBuilder.AppendLine($"{channel.NumberParameter}{vbTab}{channel.IndexOnChassis}{vbTab}{channel.Name}{vbTab}{vbTab}{e.AcquisitionData(I).ToString("F6", CultureInfo.InvariantCulture)}")
    '            I += 1
    '        Next
    '    Next

    '    TextBoxAsquiredData.Text = AsquiredDataStringBuilder.ToString
    '    IsShowAsquiredData = False
    'End Sub

    ''' <summary>
    ''' Запуск таймера накопления и записи
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StartAcquisitionTimer()
        mmTimerChassisAcquisition100Hz = New Multimedia.Timer With {
            .Mode = Multimedia.TimerMode.Periodic,
            .Period = timerIntervalChassisAcquisition100Hz,
            .Resolution = 1,
            .SynchronizingObject = Me
        }
        Try
            mmTimerChassisAcquisition100Hz.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, $"Error <{NameOf(StartAcquisitionTimer)}>", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        IsStartAcquisition = mmTimerChassisAcquisition100Hz.IsRunning
    End Sub

    ''' <summary>
    ''' Остановить таймер накопления и записи
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopAcquisitionTimer()
        If mmTimerChassisAcquisition100Hz IsNot Nothing Then mmTimerChassisAcquisition100Hz.Stop()
        ClearAcquisitionTimerChassis()
        IsStartAcquisition = False
    End Sub
#End Region

#Region "Функция обратного вызова фонового потока вызова AddListItemMethod"
    Private myThread As Thread ' поток

    Delegate Sub AddListItem(message As String) ' шаблон указателя на функцию
    Private myDelegate As AddListItem ' экземпляр делегата указываемый на функцию AddListItemMethod

    ''' <summary>
    ''' Функция обратного вызова из SyncSocketClient для отображения сообщений в вкладке журнала Клиента ИВК.
    ''' Используется для экземпляра делегата myDelegate типа AddListItem.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Sub AddListItemMethod(message As String)
        AppendMsgToRichTextBoxClient(message, MessageBoxIcon.Error)
        RegistrationEventLog.EventLog_MSG_CONNECT(message)
    End Sub

    ''' <summary>
    ''' Функция запускаемая в потоке myThread с одним параметром Object типа MyThreadClass,
    ''' в конструктор которого передаётся необходимая информация.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Private Sub ThreadFunction(message As Object)
        Dim myThreadClassObject As New MyThreadClass(Me, CStr(message))
        myThreadClassObject.Run()
    End Sub

    ''' <summary>
    ''' Выполняемая в фоновом потоке работа по добавлению сообщений в главную форму
    ''' посредством вызова в ней AddListItemMethod.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class MyThreadClass
        Private ReadOnly myFormControl As FormCompactRio

        Public Sub New(myForm As FormCompactRio, message As String)
            myFormControl = myForm
            Me.message = message
        End Sub

        Private ReadOnly message As String
        Public Sub Run()
            If myFormControl.IsHandleCreated Then
                myFormControl.Invoke(myFormControl.myDelegate, New Object() {message})
            End If
        End Sub
    End Class
#End Region

#Region "ChannelsTree"
    ''' <summary>
    ''' Расширение TreeNode для отображения канала в дереве
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ChannelNode
        Inherits TreeNode

        Friend Sub New(inText As String, nodeType As NodeTypeEnum, keyId As Integer)
            MyBase.New(inText)
            Me.NodeType = nodeType
            Me.KeyId = keyId
        End Sub

        ''' <summary>
        ''' Индекс шасси, номер модуля, номер канала, индекс размерности.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Property KeyId() As Integer
        Friend Property NodeType() As NodeTypeEnum

        ''' <summary>
        ''' Серверное Имя канала в конигурации.
        ''' Используется для поиска канала в коллекции в шасси 
        ''' и нахождении полиномов при вычислении физического значения.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Property ChName() As String
    End Class

    ''' <summary>
    ''' Изменить представление каналов в проводнике по шасси или по единицам измерения
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RadioButtonViewModule_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonViewModule.CheckedChanged
        If isHandleFormCreated Then 'Me.IsHandleCreated
            If RadioButtonViewModule.Checked Then
                RadioButtonViewModule.BackColor = Color.Blue
                RadioButtonViewModule.ForeColor = Color.Yellow
                RadioButtonViewType.BackColor = Color.Silver
                RadioButtonViewType.ForeColor = Color.Black
            Else
                RadioButtonViewModule.BackColor = Color.Silver
                RadioButtonViewModule.ForeColor = Color.Black
                RadioButtonViewType.BackColor = Color.Blue
                RadioButtonViewType.ForeColor = Color.Yellow
            End If

            PopulateChannelsNodes()
        End If
    End Sub

    ''' <summary>
    ''' Заполнить Корзины
    ''' </summary>
    Private Sub PopulateChannelsNodes()
        Dim chassisHostName As String
        Dim listChassisHostName As New List(Of String)

        ChannelsTree.Nodes.Clear()

        If RadioButtonViewModule.Checked Then ' по шасси
            Dim tvRoot As New ChannelNode("Registration", NodeTypeEnum.SSD0, 0) With {.ImageIndex = NodeImage.DAQBoard17, .SelectedImageIndex = NodeImage.DAQBoardSelected18, .Tag = "Registration"}

            ChannelsTree.Nodes.Add(tvRoot) ' добавить корневой

            For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
                chassisHostName = itemChassis.HostName

                If Not listChassisHostName.Contains(chassisHostName) Then
                    listChassisHostName.Add(chassisHostName)
                    AddChassisToTree(tvRoot, chassisHostName, itemChassis)
                End If
            Next
        Else ' по размерностям
            For I As Integer = 0 To UBound(UnitOfMeasureArray)
                AddParameterToTree(UnitOfMeasureArray(I))
            Next
        End If

        ChannelsTree.Nodes(0).Expand()
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Шасси
    ''' </summary>
    ''' <param name="tvRoot"></param>
    ''' <param name="hostName"></param>
    ''' <param name="itemChassis"></param>
    Private Sub AddChassisToTree(ByVal tvRoot As ChannelNode, ByVal hostName As String, itemChassis As Chassis)
        Dim cRoot As New ChannelNode("Шасси - " & hostName, NodeTypeEnum.Chassis1, itemChassis.IndexRow) With {
            .ImageIndex = NodeImage.Chassis13,
            .SelectedImageIndex = NodeImage.ChassisSelected14,
            .Tag = hostName
        }
        tvRoot.Nodes.Add(cRoot)
        AddDirectories(cRoot, itemChassis) ' чтобы узнать, есть ли предки и изменить плюс
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Модуль
    ''' </summary>
    ''' <param name="nodeParent"></param>
    ''' <param name="numberModule"></param>
    ''' <param name="itemChassis"></param>
    Private Sub AddModuleToTree(ByRef nodeParent As ChannelNode, numberModule As String, itemChassis As Chassis)
        Dim cRoot As New ChannelNode("Модуль - " & numberModule, NodeTypeEnum.Module2, CInt(Val(numberModule.Substring(3, 1)))) With {
            .ImageIndex = NodeImage.Module15,
            .SelectedImageIndex = NodeImage.ModuleSelected16,
            .Tag = numberModule
        }
        nodeParent.Nodes.Add(cRoot)
        AddDirectories(cRoot, itemChassis) ' чтобы узнать, есть ли предки и изменить плюс
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Канал
    ''' </summary>
    ''' <param name="nodeParent"></param>
    ''' <param name="itemChannel"></param>
    Private Sub AddChannelToTree(ByRef nodeParent As ChannelNode, itemChannel As ChannelRio)
        Dim nameChannel As String = $"[{itemChannel.IndexOnChassis}] {itemChannel.Name} ({itemChannel.UnitOfMeasure})"
        Dim unit As String = itemChannel.UnitOfMeasure
        Dim cRoot As New ChannelNode(nameChannel, NodeTypeEnum.Channel3, itemChannel.IndexOnChassis) With {
            .ImageIndex = If(CBool(InStr(1, Pattern_Units, unit)), Array.IndexOf(UnitOfMeasureArray, unit), NodeImage.Discrete1),
            .SelectedImageIndex = NodeImage.Selected19,
            .Tag = nameChannel,
            .ChName = itemChannel.Name
        }
        nodeParent.Nodes.Add(cRoot)
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Тип Параметра
    ''' </summary>
    ''' <param name="inUnit" ></param>
    Private Sub AddParameterToTree(inUnit As String)
        Dim typeParameter As String ' Тип Параметра

        Select Case inUnit
            Case unitDiscrete ' "дел"
                typeParameter = cDiscrete ' "ДИСКРЕТНЫЕ"
            Case unitEvacuation ' "мм"
                typeParameter = cEvacuation' "РАЗРЕЖЕНИЯ"
            Case unitTemperature ' "градус"
                typeParameter = cTemperature' "ТЕМПЕРАТУРЫ"
            Case unitPresure ' "Кгсм"
                typeParameter = cPresure' "ДАВЛЕНИЯ"
            Case unitVibration ' "мм/с"
                typeParameter = cVibration' "ВИБРАЦИЯ"
            Case unitCurrent ' "мкА"
                typeParameter = cCurrent' "ТОКИ"
            Case unitPetrol ' "кг/ч"
                typeParameter = cPetrol' "РАСХОДЫ"
            Case unitTraction ' "кгс"
                typeParameter = cTraction ' "ТЯГА"
            Case Else ' unitRevolution' "%"
                typeParameter = cRevolution ' "ОБОРОТЫ"
        End Select

        Dim cRoot As New ChannelNode(typeParameter, NodeTypeEnum.ParameterType5, Array.IndexOf(UnitOfMeasureArray, inUnit)) With {
            .ImageIndex = Array.IndexOf(UnitOfMeasureArray, inUnit),
            .SelectedImageIndex = NodeImage.Selected19,
            .Tag = inUnit
        }
        ChannelsTree.Nodes.Add(cRoot)

        For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
            AddDirectories(cRoot, itemChassis) ' чтобы узнать, есть ли предки и изменить плюс
        Next

        If cRoot.Nodes.Count = 0 Then cRoot.Remove()
    End Sub

    ''' <summary>
    ''' Добавить дочерний узел в родительский
    ''' </summary>
    ''' <param name="nodeParent"></param>
    ''' <param name="itemChassis"></param>
    ''' <remarks></remarks>
    Private Sub AddDirectories(nodeParent As ChannelNode, itemChassis As Chassis)
        Dim listDevice As New List(Of String)

        ' по типу родителя определяется тип добавляемых дочерних узлов
        Select Case nodeParent.NodeType
            Case NodeTypeEnum.Chassis1 ' родитель Шасси1 -> найти все модули в шасси
                Dim HwChGroups = From selectChannel In itemChassis.GManagerChannels.Channels.Values
                                 Group selectChannel By Key = selectChannel.ChId.Substring(0, 4) Into Group
                                 Select FirstLetterModule = Key, ModuleGroup = Group

                If HwChGroups.Count > 0 Then
                    For Each g In HwChGroups
                        If Not listDevice.Contains(g.FirstLetterModule) Then
                            listDevice.Add(g.FirstLetterModule)
                            AddModuleToTree(nodeParent, g.FirstLetterModule, itemChassis)
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If

                Exit Select
            Case NodeTypeEnum.Module2 ' родитель Модуль2 -> найти все каналы в модуле
                Dim channels As IEnumerable(Of ChannelRio) = Nothing

                If RadioButtonViewModule.Checked Then ' по шасси
                    channels = From selectChannel In itemChassis.GManagerChannels.Channels.Values
                               Where selectChannel.ChId.Substring(0, 4) = CStr(nodeParent.Tag)
                               Select selectChannel
                Else ' по размерностям
                    channels = From selectChannel In itemChassis.GManagerChannels.Channels.Values
                               Where selectChannel.ChId.Substring(0, 4) = CStr(nodeParent.Tag) AndAlso selectChannel.UnitOfMeasure = CStr(nodeParent.Parent.Parent.Tag)
                               Select selectChannel
                End If

                If channels.Count > 0 Then
                    For Each itemChannel As ChannelRio In channels
                        If Not listDevice.Contains(CStr(itemChannel.IndexOnChassis)) Then
                            listDevice.Add(CStr(itemChannel.IndexOnChassis))
                            AddChannelToTree(nodeParent, itemChannel)
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If

                Exit Select
            Case NodeTypeEnum.ParameterType5 ' родитель ТипПараметра5 почти как родитель Шасси1 -> найти все каналы с размерностью в Tag родительского узла
                Dim channel_Dict As IEnumerable(Of ChannelRio) = From selectChannel In itemChassis.GManagerChannels.Channels.Values
                                                                 Where selectChannel.UnitOfMeasure = CStr(nodeParent.Tag)
                                                                 Select selectChannel
                Dim chassisHostName As String

                If channel_Dict.Count > 0 Then
                    For Each itemChannel As ChannelRio In channel_Dict
                        chassisHostName = itemChassis.HostName

                        If Not listDevice.Contains(chassisHostName) Then
                            listDevice.Add(chassisHostName)
                            Dim cRoot As New ChannelNode("Шасси - " & chassisHostName, NodeTypeEnum.Chassis1, itemChassis.IndexRow) With {
                                .ImageIndex = NodeImage.Chassis13,
                                .SelectedImageIndex = NodeImage.ChassisSelected14,
                                .Tag = chassisHostName
                            }
                            nodeParent.Nodes.Add(cRoot)
                            AddDirectories(cRoot, itemChassis) ' чтобы узнать, есть ли предки и изменить плюс
                        End If
                    Next
                End If

                Exit Select
        End Select
    End Sub

    Private Sub ChannelsTree_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles ChannelsTree.AfterCollapse
        e.Node.BackColor = Color.White
    End Sub

    ''' <summary>
    ''' Свернуть другие ветки
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChannelsTree_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles ChannelsTree.BeforeExpand
        parentNodeExpand = Nothing
        nodeExpand = Nothing

        Try
            ' вначале свернуть все дочерние
            If e.Node.Parent Is Nothing Then ' по самому первому уровню
                For Each loopNode As TreeNode In ChannelsTree.Nodes
                    If loopNode.IsExpanded Then loopNode.Collapse()
                Next
            Else
                For Each loopNode As TreeNode In e.Node.Parent.Nodes
                    If loopNode.IsExpanded Then loopNode.Collapse()
                Next
            End If

            nodeExpand = CType(e.Node, ChannelNode) ' текущий узел

            If nodeExpand.NodeType = NodeTypeEnum.Module2 Then
                parentNodeExpand = CType(nodeExpand.Parent, ChannelNode)
            End If

            e.Node.EnsureVisible() ' прокрутить для отображения
            e.Node.BackColor = Color.Gold
        Catch ex As Exception
            Dim caption As String = $"Смена отображения <{NameOf(ChannelsTree_BeforeExpand)}>"
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Вывести значения каналов в раскрытой ветке проводника
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    Private Sub OnTimedUpdataTreeEvent(source As Object, e As ElapsedEventArgs)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() OnTimedUpdataTreeEvent(source, e)))
        Else
            ' 1. определить раскрытую ветку в дереве или является ли ветка уровня N раскрытой
            ' 2. из этой ветки считать узлы и по номеру KeyID определить номер в индексе массива измеренных данных сокета клиента.
            ' 3. имя сокет клиента для раскрытого узла в дереве взять из parentNodeExpand.Tag
            ' 4. индекс значения канала loopNode.KeyId равен Channel.Number
            If parentNodeExpand IsNot Nothing AndAlso nodeExpand IsNot Nothing Then
                Dim sync As Integer = Interlocked.CompareExchange(syncTimerUpdataTree, 1, 0)

                If sync = 0 Then
                    'Dim hostNameChassis As String = parentNodeExpand.Tag ' содержит HostName
                    'If dictConnInfoChassis.ContainsKey(hostNameChassis) Then
                    '    Dim infoConnWithServer As ConnectionInfoServer = dictConnInfoChassis(hostNameChassis)
                    '    ChannelsTree.BeginUpdate()
                    '    For Each loopNode As ChannelNode In nodeExpand.Nodes
                    '        'If isSolvePolinomForGrid Then
                    '        '    loopNode.Text = String.Format("{0}  {1}", loopNode.Tag,
                    '        '                                  PhysicalApproximation(AcquisitionAverageChassis(loopNode.KeyId - 1),
                    '        '                                                     GManagerChassis.Chassis(infoConnWithServer.NameChassis).GManagerChannels.Channels(loopNode.ChName).LevelOfApproximation,
                    '        '                                                     GManagerChassis.Chassis(infoConnWithServer.NameChassis).GManagerChannels.Channels(loopNode.ChName).Polynome).ToString("F6", CultureInfo.InvariantCulture))
                    '        'Else
                    '        loopNode.Text = $"{loopNode.Tag}  {AcquisitionAverageChassis(loopNode.KeyId - 1).ToString("F6", CultureInfo.InvariantCulture)}"
                    '        'End If
                    '    Next
                    '    ChannelsTree.EndUpdate()
                    'End If

                    ChannelsTree.BeginUpdate()
                    For Each loopNode As ChannelNode In nodeExpand.Nodes
                        loopNode.Text = $"{loopNode.Tag}  {AcquisitionAverageChassis(loopNode.KeyId - 1).ToString("F6", CultureInfo.InvariantCulture)}"
                    Next
                    ChannelsTree.EndUpdate()
                    syncTimerUpdataTree = 0  ' освободить
                End If
            End If
        End If
    End Sub
#End Region

#Region "Навигация по вкладкам"
    Dim isGridSelectionChanged, isListViewSelectedChanged, isTabSelectedChanged As Boolean
    Dim selectedRowIndex As Integer
    ' событие идёт -> 
    ' от кнопок         -> grdChassis
    ' от grdChassis     -> ListView     + TabLogControl -> ButtonEnabled
    ' от ListView       -> grdChassis   + TabLogControl -> ButtonEnabled
    ' от TabLogControl  -> ListView     + grdChassis    -> ButtonEnabled

    Private Sub BackToolStripButton_Click(sender As Object, e As EventArgs) Handles BackToolStripButton.Click
        ForwardToolStripButton.Enabled = True
        selectedRowIndex -= 1

        If selectedRowIndex < 0 Then selectedRowIndex = 0

        For I = 0 To grdChassis.Rows.Count - 1
            grdChassis.Rows(I).Selected = False
        Next

        grdChassis.Rows(selectedRowIndex).Selected = True
        BackToolStripButton.Enabled = selectedRowIndex <> 0
    End Sub

    Private Sub ForwardToolStripButton_Click(sender As Object, e As EventArgs) Handles ForwardToolStripButton.Click
        BackToolStripButton.Enabled = True
        selectedRowIndex += 1

        If selectedRowIndex > grdChassis.Rows.Count - 1 Then selectedRowIndex = grdChassis.Rows.Count - 1

        For I = 0 To grdChassis.Rows.Count - 1
            grdChassis.Rows(I).Selected = False
        Next

        grdChassis.Rows(selectedRowIndex).Selected = True
        ForwardToolStripButton.Enabled = selectedRowIndex <> grdChassis.Rows.Count - 1
    End Sub

    Private Sub grdChassis_SelectionChanged(sender As Object, e As EventArgs) Handles grdChassis.SelectionChanged
        If isHandleFormCreated AndAlso grdChassis.SelectedRows.Count > 0 AndAlso isListViewSelectedChanged = False AndAlso isTabSelectedChanged = False Then
            Dim hostNameSelectedRow As String = grdChassis.SelectedRows(0).Cells(0).Value.ToString()

            isGridSelectionChanged = True

            For I = 0 To ListView.Items.Count - 1
                ListView.Items(I).Selected = False
            Next

            For I = 0 To ListView.Items.Count - 1
                If ListView.Items(I).Text = hostNameSelectedRow Then
                    ListView.Items(I).Selected = True
                    TabLogControl.SelectedIndex = I + 2 ' (строки Клиента и Сервера в grdChassis нет)
                    Exit For
                End If
            Next

            ButtonEnabled()
            isGridSelectionChanged = False
        End If
    End Sub

    Private Sub ListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView.SelectedIndexChanged
        If isHandleFormCreated AndAlso ListView.SelectedItems.Count > 0 AndAlso isGridSelectionChanged = False AndAlso isTabSelectedChanged = False Then
            Dim hostNameSelectedList As String = ListView.SelectedItems(0).Text

            isListViewSelectedChanged = True

            For I = 0 To grdChassis.Rows.Count - 1
                grdChassis.Rows(I).Selected = False
            Next

            For I = 0 To grdChassis.Rows.Count - 1
                If grdChassis.Rows(I).Cells(0).Value.ToString() = hostNameSelectedList Then
                    grdChassis.Rows(I).Selected = True
                    TabLogControl.SelectedIndex = I + 2 ' (строки Клиента и Сервера в ListView нет)
                    Exit For
                End If
            Next

            ButtonEnabled()
            isListViewSelectedChanged = False
        End If
    End Sub

    Private Sub TabLogControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabLogControl.SelectedIndexChanged
        If isHandleFormCreated AndAlso grdChassis.SelectedRows.Count > 0 AndAlso isGridSelectionChanged = False AndAlso isListViewSelectedChanged = False Then
            Dim tempTabControl As TabControl = CType(sender, TabControl)
            Dim tempTabPage As TabPage = tempTabControl.TabPages(tempTabControl.SelectedIndex)

            If tempTabPage.Tag IsNot Nothing Then
                isTabSelectedChanged = True

                For I = 0 To ListView.Items.Count - 1
                    ListView.Items(I).Selected = False
                Next

                For I = 0 To grdChassis.Rows.Count - 1
                    grdChassis.Rows(I).Selected = False
                Next

                ListView.Items(CInt(tempTabPage.Tag)).Selected = True
                grdChassis.Rows(CInt(tempTabPage.Tag)).Selected = True
                ButtonEnabled()
                isTabSelectedChanged = False
            End If
        End If
    End Sub

    Private Sub ButtonEnabled()
        BackToolStripButton.Enabled = grdChassis.SelectedRows(0).Index <> 0
        ForwardToolStripButton.Enabled = grdChassis.SelectedRows(0).Index <> grdChassis.Rows.Count - 1
    End Sub
#End Region

#Region "WorkTask"
    'Модуль содержит реализации длительно выполняемых задач в выделенных рабочих потоках.
    ''' <summary>
    ''' Пользовательское исключение.
    ''' </summary>
    ''' <remarks></remarks>
    <SerializableAttribute>
    Private Class WorkTask_ERROR
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, inner As Exception)
            MyBase.New(message, inner)

            HelpLink = "http://home.VictorMake.com/~Victorinstructor/ProgExp/Connect.html"
            ' Заполнить пользовательские данные касательно ошибки
            Data.Add("Временная метка", $"Ошибка произошла {DateTime.Now}")
            Data.Add("Причина", "Описание причины")
        End Sub
    End Class

    ''' <summary>
    ''' Простой класс результата выполнения задачи
    ''' </summary>
    ''' <remarks></remarks>
    Private Class TasksResult
        Public Property Result As Boolean
        Public Property SourseError As String
    End Class

    ''' <summary>
    ''' Инкапсуляция работы проверки прохождения этапа инициализации шасси.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CheckAllChassisStep
        Implements IDisposable

        Private Property TimerInterval As Integer = 50      ' миллисекунд
        Private Property ThreadSleep As Integer = 100       ' миллисекунд
        Private Property WaitExecutionCheck As Integer = 60 ' Ждать Выполнения Проверки сек
        Private Property StepCompleted As ChassisStepCompleted = ChassisStepCompleted.IsConnected
        ReadOnly strOutChassisError As New StringBuilder()

        Private aTimer As System.Timers.Timer ' серверный таймер работает в другом потоке
        Private startTime As DateTime
        Private syncPointCheck As Integer = 0
        Private checkAllChassisCompleted As Boolean
        Private ReadOnly mCompactRioForm As FormCompactRio

        Public Sub New(timerInterval As Integer, timerSleep As Integer, timerWaitCheck As Integer, stepCompleted As ChassisStepCompleted, inCompactRioForm As FormCompactRio)
            Me.TimerInterval = timerInterval
            Me.ThreadSleep = timerSleep
            Me.WaitExecutionCheck = timerWaitCheck
            Me.StepCompleted = stepCompleted
            mCompactRioForm = inCompactRioForm
        End Sub

        ''' <summary>
        ''' Метод для задачи, который ждём в отдельном потоке
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function WaitAllChassisStep() As TasksResult
            aTimer = New System.Timers.Timer(TimerInterval)
            AddHandler aTimer.Elapsed, AddressOf OnTimedEvent

            startTime = Date.Now
            aTimer.Interval = TimerInterval
            aTimer.Enabled = True

            ' здесь ожидание срабатывания
            Do While aTimer.Enabled
                Thread.Sleep(ThreadSleep)
                Application.DoEvents()
            Loop

            aTimer = Nothing
            Return New TasksResult With {.Result = checkAllChassisCompleted, .SourseError = strOutChassisError.ToString}
        End Function

        Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
            Dim sync As Integer = Interlocked.CompareExchange(syncPointCheck, 1, 0)

            Application.DoEvents()
            If sync = 0 Then
                Dim passTime As TimeSpan = DateTime.Now - startTime

                checkAllChassisCompleted = True ' предположение, что для всех шасси требование выполнено
                strOutChassisError.Clear()

                ' если не выполнено необходимое требование хотя бы на одном шасси, то повторить проверку в следующем событии
                For Each itemChassis As Chassis In mCompactRioForm.ManagerChassis.Chassis.Values
                    Select Case StepCompleted
                        Case ChassisStepCompleted.IsConnected
                            If itemChassis.IsConnected = False Then
                                checkAllChassisCompleted = False
                                strOutChassisError.AppendLine(itemChassis.HostName)
                            End If
                            Exit Select
                        Case ChassisStepCompleted.GetMetaSuccess
                            If itemChassis.GetMetaSuccess = False Then
                                checkAllChassisCompleted = False
                                strOutChassisError.AppendLine(itemChassis.HostName)
                            End If
                            Exit Select
                        Case ChassisStepCompleted.InitSuccess
                            If itemChassis.IsInitSuccess = False Then
                                checkAllChassisCompleted = False
                                strOutChassisError.AppendLine(itemChassis.HostName)
                            End If
                            Exit Select
                        Case ChassisStepCompleted.LaunchSuccess
                            If itemChassis.IsLaunchSuccess = False Then
                                checkAllChassisCompleted = False
                                strOutChassisError.AppendLine(itemChassis.HostName)
                            End If
                            Exit Select
                    End Select
                Next

                If checkAllChassisCompleted Then ' для всех шасси требование выполнено
                    syncPointCheck = 0  ' освободить
                    aTimer.Stop()
                    Exit Sub
                End If

                If passTime.TotalSeconds > WaitExecutionCheck Then ' превышено время ожидания
                    aTimer.Stop()
                End If

                syncPointCheck = 0
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Чтобы обнаружить избыточные вызовы

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If aTimer IsNot Nothing Then
                        If aTimer.Enabled Then aTimer.Stop()
                        aTimer.Dispose()
                        aTimer = Nothing
                    End If
                End If

            End If
            Me.disposedValue = True
        End Sub

        ' Этот код добавлен редактором Visual Basic для правильной реализации шаблона высвобождаемого класса.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Не изменяйте этот код. Разместите код очистки выше в методе Dispose(disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    ''' <summary>
    ''' Перечислитель команд, характеризующих успешный запуск шасси.
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ChassisStepCompleted
        IsConnected
        GetMetaSuccess
        InitSuccess
        LaunchSuccess
    End Enum

    ''' <summary>
    ''' Загрузка конфигурационного файла Сервера, разбор, создание манеджера шасси и их перезагрузка.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsLoadCfgAndReboot() As Boolean
        Dim success As Boolean = False ' результат возвращаемый функцией, по умолчанию  = False

        Try
            ' Чтение XML конфигурационного файла и парсинг конфигурационного xml
            If Not IsParseDataBase() Then
                success = False
                Throw New WorkTask_ERROR($"Ошибка разбора конфигурационного файла в {NameOf(IsParseDataBase)}")
            End If

            If mOptionDataCRIO.CollectionTargetCRIO.Count = 0 Then
                success = False
                Throw New WorkTask_ERROR("Шасси cRio не включены в конфигурацию")
            End If

            ' 1. Создаётся менеджер классов ИВК и производится создание коллекции ИВК.
            ' 2. Определяется из gOptionData число шасси cRio, адрес каждого шасси, тип решаемой задачи 
            ' (только сбор или сбор с управлением, 
            ' определяется каталог содержащий отлаженные файлы программы, для скачивания на диск в шасси).
            ' 3. В менеджере произвести проверку каталога для скачивания и наличие файлов в нём. 
            ' 4. Разбор ветки HwChannels каналов и групировка по шасси и далее по модулям.
            ' 5. Поиск для измерительных каналов в ветке Channels канала по ПИН и нахождение полиномов.
            ' 6. Подготовить соответствующий INI файл с каналами для каждого шасси и разместить его в каталоге с программами для копирования.
            ' Каждая ИВК может содержать каналы от разных cRio, поэтому в атрибутах каналов
            ' (скорее всего в ATTR_HWCH_Chassis) в ветках ИВК HwCh указан идентификатор шасси cRio). 
            ' 7. Xml конфигурационный файл Сервера разбирается парсером, на основании идентификаторов  cRio ( по ATTR_HWCH_Chassis)
            ' подготавливается список каналов со всеми атрибутами (в том числе полиномов) для каждого контроллера cRio 
            ' и этот список вместе со всеми другими файлами пересылается по FTP в директории контроллера  (/ni-rt/startup/Channels),
            ' чтобы программа запускаемая на контроллере знала с какими каналами она работает. 
            ' ПРОВЕРКА СТАТУСА КАНАЛА (ВКЛЮЧЁН ИЛИ ВЫКЛЮЧЕН)
            ' Таким образом программа на cRio должна настроить задачи сбора или задачи управления,
            ' и определить массив для пересылки измерений в соответствии с числом каналов по всем задачам. 
            ' Не используется: Для cRio работающих с управлением устройств дополнительно добавляются управляющие каналы
            ' (они так же находятся в ветках HwCh ИВК).

            ' 8. Исполняемые файлы LabView и скомпилированные FPGA копируются в папку /ni-rt/startup/ каждой cRio
            ' (попутно проверяется соединение). 
            ' Если конфигурация ИВК не менялась, то файлы для каждого контроллера cRio уже были записаны на диск контроллера 
            ' и при его перезапуске будет произведена автозагрузка)
            ' 9. подключение ко всем cRio и перекачку файлов по FTP
            ' rio://192.168.0.98/RIO0

            If Not IsCreateManagerChassis() Then Return False ' создать менеджер шасси

#If DEBUG_ClientTest = False Then ' для отладки без копирования и перезапуска
            If Not CheckPingAllFtpClient() Then Return False ' проверить сетевое соединение шасси
            If Not RunTasksCopyFileFromAllChassis() Then Return False ' скопировать файлы на шасси по FTP
            If Not RunRestartAllChassis() Then Return False ' перезапустить все шасси
#End If
            IsRestartAllChassisSuccess = True
            ' если дошли сюда, то OK
            success = True
        Catch ex As WorkTask_ERROR
            Const caption As String = NameOf(IsLoadCfgAndReboot)
            MessageBox.Show("Ошибка при загрузке модуля", caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> Ошибка при загрузке модуля")
        Catch ex As Exception
            ' Возникла ошибка при работе функции. Производится освобождение всех ресурсов которые были выделены.
            Const caption As String = NameOf(IsLoadCfgAndReboot)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Создание менеджера класса Шасси на основании конфигурации СписокШассиCRio.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCreateManagerChassis() As Boolean
        Dim success As Boolean = False ' результат возвращаемый функцией, по умолчанию  = False

        Try
            ManagerChassis = New TargetsChassis(Me, mOptionDataCRIO)
            If ManagerChassis.IsLoadChassisSuccess() Then
                If ManagerChassis.Count > 0 Then
                    ' наладить основное окно
                    success = True
                Else
                    ManagerChassis = Nothing ' сбросить
                    success = False
                End If
            End If
        Catch ex As Exception
            Const caption As String = NameOf(IsCreateManagerChassis)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Создать с помощью фабрики задач массив задач, проверяющих по Ping сетевое подключение .
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckPingAllFtpClient() As Boolean
        Dim success As Boolean  ' результат возвращаемый функцией, по умолчанию  = False
        Dim tokenSource As New CancellationTokenSource()
        Dim token As CancellationToken = tokenSource.Token
        ' Массив ссылок задач таким образом можно ожидать их просматривать их статус после прерывания
        Dim arrTask(ManagerChassis.Chassis.Count - 1) As Task(Of Boolean)
        Dim I As Integer

        For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
            arrTask(I) = Task(Of Boolean).Factory.StartNew(Function() IsCheckPingClient(itemChassis.IPAddressRTtarget.IP), token, token)
            I += 1
        Next

        Try
            Task.WaitAll(arrTask)
        Catch ae As AggregateException
            ' Показать OperationCanceledExceptions сообщение.
            ' Предполагается, что известно, что делать с особенным исключением при
            ' переброске его наверх. AggregateException.Handle предоставляет
            ' другой путь отобразить это.

            For Each ex In ae.InnerExceptions
                Const caption As String = NameOf(IsCheckPingAllFtpClient)
                Dim text As String = ex.Message
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")

                If TypeOf (ex) Is WorkTask_ERROR Then
                    'Console.WriteLine(ex.Message)
                Else
                    ' В этом исключение System.AggregateException перехватывается, 
                    ' но не предпринимается попытка обработать какие-либо его внутренние исключения. 
                    ' Вместо этого метод Flatten используется для извлечения внутренних исключений из любых вложенных экземпляров AggregateException и 
                    ' повторного создания одного исключения AggregateException, 
                    ' которое будет непосредственно содержать все внутренние необработанные исключения. 
                    ' Объединение исключения делает его обработку клиентским кодом более удобной.
                    Throw ae.Flatten()
                End If
            Next

            success = False
        End Try

        ' общий результат зависит от успешности каждой операции
        Dim results(arrTask.Length - 1) As Boolean
        For I = 0 To arrTask.Length - 1
            results(I) = arrTask(I).Result ' Свойство Result блокирует вызывающий поток до завершения задачи. но уже используется  WaitAll
            Dim text As String = $"Результат пинга шасси: <{ManagerChassis.Chassis.ToArray(I).Value.HostName}> равен: {results(I)}"
            InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(results(I), MessageBoxIcon.Question, MessageBoxIcon.Error))
            RegistrationEventLog.EventLog_MSG_CONNECT($"<CheckPingAllFtpClient> {text}")
        Next

        success = True
        For I = 0 To arrTask.Length - 1
            If results(I) = False Then
                ' вызвать исключение или сообщение
                success = False
            End If
        Next

        Return success
    End Function

    ''' <summary>
    ''' Создать с помощью фабрики задач массив задач, ссылающихся на класс FTP и запускающий из него метод пересылки.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function IsRunTasksCopyFileFromAllChassis() As Boolean
        Const caption As String = NameOf(IsRunTasksCopyFileFromAllChassis)
        Dim message As String
        ' Каждая задача запускается при создании и возвращает результат о успехе (по идеи основной поток должен ждать окончания завершения каждой задачи),    
        ' но всё равно используется WaitAll(массив задач).
        Dim success As Boolean = False ' результат возвращаемый функцией, по умолчанию  = False
        ' B случае отмены или ошибки предусмотрена отмена (CancellationToken) задачи и обработка исключения AggregateException.
        ' Запрос прерывания простой задачи, когда token source прерван.
        ' Передать token пользовательскому делегату и также задаче, и таким образом управлять исключением корректно.
        Dim tokenSource As New CancellationTokenSource()
        Dim token As CancellationToken = tokenSource.Token
        ' Массив ссылок задач, таким образом можно ожидать их просматривать их статус после прерывания.
        Dim arrTask(ManagerChassis.Chassis.Count - 1) As Task(Of Boolean)
        Dim I As Integer

        For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
            arrTask(I) = Task(Of Boolean).Factory.StartNew(Function() IsCopyFileFromChassis(itemChassis.HostName, token), token, token)
            I += 1
        Next

        Try
            Task.WaitAll(arrTask) ' ожидание завершения всех задач
        Catch ae As AggregateException
            ' для демонстрации возможностей показать OperationCanceledExceptions сообщение
            ' предполагается что известно что делать с особенным исключением
            ' перебросить это куда-то ещё. AggregateException.Handle предоставляет
            ' другой путь отобразить это.
            For Each ex In ae.InnerExceptions
                message = ex.Message
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {message}")

                If TypeOf (ex) Is WorkTask_ERROR Then
                    'Console.WriteLine(ex.Message)
                Else
                    ' В этом исключение System.AggregateException перехватывается, но не предпринимается попытка обработать какие-либо его внутренние исключения. 
                    ' Вместо этого метод Flatten используется для извлечения внутренних исключений из любых вложенных экземпляров AggregateException и повторного создания одного исключения AggregateException, 
                    ' которое будет непосредственно содержать все внутренние необработанные исключения. Выравнивание исключения делает обработку клиентским кодом более удобной.
                    Throw ae.Flatten()
                End If
            Next

            success = False
        End Try

        Dim results(arrTask.Length - 1) As Boolean
        For I = 0 To arrTask.Length - 1
            results(I) = arrTask(I).Result ' Свойство Result блокирует вызывающий поток до завершения задачи. но уже используется  WaitAll
            message = $"Результат копирования файлов на шасси: <{ManagerChassis.Chassis.ToArray(I).Value.HostName}> равен: {results(I)}"
            InvokeAppendMsgToRichTextBoxByKey(message, KeyRichTexServer, If(results(I), MessageBoxIcon.Question, MessageBoxIcon.Error))
            RegistrationEventLog.EventLog_MSG_FILE_IO_UPDATE($"<{caption}> {message}")
        Next

        success = True
        For I = 0 To arrTask.Length - 1
            If results(I) = False Then
                ' вызвать исключение или сообщение
                success = False
            End If
        Next

        Return success
    End Function

    ''' <summary>
    ''' Перезапустить каждое шасси в простом цикле.
    ''' Замена более сложной задачи RunTasksRestartAllChassis.
    ''' </summary>
    ''' <returns></returns>
    Private Function IsRunRestartAllChassis() As Boolean
        Const caption As String = NameOf(IsRunRestartAllChassis)
        Dim success As Boolean
        Dim message As String
        Dim tokenSource As New CancellationTokenSource()
        Dim token As CancellationToken = tokenSource.Token
        ' Запрос прерывания простой задачи когда token source прерван.
        ' Передать token пользовательскому делегату и также задаче, и таким образом управлять исключением корректно
        Dim results(ManagerChassis.Chassis.Count - 1) As TasksResult

        Try
            Dim I As Integer
            For Each itemChassis As Chassis In ManagerChassis.Chassis.Values
                results(I) = CallTryRestartChassis(itemChassis.IPAddressRTtarget.IP, token)
                I += 1
            Next
        Catch ae As AggregateException
            ' Показать OperationCanceledExceptions сообщение.
            ' Предполагается, что известно, что делать с особенным исключением при
            ' переброске его наверх. AggregateException.Handle предоставляет
            ' другой путь отобразить это.

            For Each ex In ae.InnerExceptions
                message = ex.Message
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {message}")

                If TypeOf (ex) Is WorkTask_ERROR Then
                    'Console.WriteLine(ex.Message)
                Else
                    ' В этом исключение System.AggregateException перехватывается, 
                    ' но не предпринимается попытка обработать какие-либо его внутренние исключения. 
                    ' Вместо этого метод Flatten используется для извлечения внутренних исключений из любых вложенных экземпляров AggregateException и 
                    ' повторного создания одного исключения AggregateException, 
                    ' которое будет непосредственно содержать все внутренние необработанные исключения. 
                    ' Объединение исключения делает его обработку клиентским кодом более удобной.
                    Throw ae.Flatten()
                End If
            Next
        End Try

        success = True
        ' общий результат зависит от успешности каждой операции
        For I As Integer = 0 To results.Length - 1
            message = $"Результат перезагрузки шасси: <{ManagerChassis.Chassis.ToArray(I).Value.HostName}> равен: {results(I).Result}"
            InvokeAppendMsgToRichTextBoxByKey(message, KeyRichTexServer, If(results(I).Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
            RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {message}")

            If results(I).Result = False Then
                success = False
            End If
        Next

        Return success
    End Function

    ''' <summary>
    ''' Задача рабочего потока связанного с шасси по копированию на него исполняемых файлов.
    ''' </summary>
    ''' <param name="HostName"></param>
    ''' <param name="ct"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCopyFileFromChassis(HostName As String, ct As CancellationToken) As Boolean
        Dim itemChassis As Chassis = ManagerChassis.Chassis(HostName)
        Dim mFtpClient As New FtpClient(itemChassis.IPAddressRTtarget, itemChassis.HostName, itemChassis.IsCopyFolder, Me)

        ' Прерывание было запрошено до запуска?
        If ct.IsCancellationRequested = True Then
            ct.ThrowIfCancellationRequested()
        End If

        Return mFtpClient.CopyFilesToDevice()
    End Function

    ''' <summary>
    ''' Запуск в фоне TryRestartChassis.DLL перезапускающей шасси.
    ''' </summary>
    ''' <param name="ChassisIPAdress"></param>
    ''' <param name="ct"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallTryRestartChassis(chassisIPAdress As String, ct As CancellationToken) As TasksResult
        Const bufferSize As Integer = 256 ' число знаков, которое может храниться в памяти, выделенной текущим экземпляром
        Dim sbOutBufResult As New StringBuilder(bufferSize)
        Dim sbOutBufSourseError As New StringBuilder(bufferSize)

        ' Прерывание было запрошено до запуска?
        If ct.IsCancellationRequested Then
            ct.ThrowIfCancellationRequested()
        End If

        Try
            ' фоновая задача:
            TryRestartChassis(chassisIPAdress, sbOutBufResult, bufferSize, sbOutBufSourseError, bufferSize)
        Catch ex As Exception
            ' Возникла ошибка при работе функции. Производится освобождение всех ресурсов которые были выделены.
            Const caption As String = NameOf(CallTryRestartChassis)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        ' выдать результат операции на основании модифицированных 
        Return New TasksResult With {.Result = sbOutBufResult.ToString.ToUpper = "OK", .SourseError = sbOutBufSourseError.ToString}
    End Function

    ''' <summary>
    ''' Процедура ассинхронного выполнения для задачи обработки команды от пользователя по перзагрузке шасси.
    ''' </summary>
    ''' <param name="keyChassis"></param>
    ''' <remarks></remarks>
    Private Sub RebootChassis(keyChassis As String)
        Dim success As Boolean  ' результат возвращаемый функцией, по умолчанию  = False
        Dim itemChassis As Chassis = ManagerChassis.Chassis(keyChassis)
        Dim chassisIPAdress As String = itemChassis.IPAddressRTtarget.IP

        ' остановить сбор
        SendCommandActivateToConcreteClient(keyChassis, False)
        SendCommandStopToConcreteClient(keyChassis)
        itemChassis.IsConnected = False
        itemChassis.GetMetaSuccess = False
        itemChassis.IsInitSuccess = False
        itemChassis.IsLaunchSuccess = False

        IsRestartAllChassisSuccess = True

#If DEBUG_ClientTest = True Then
        success = True
#Else
        Dim tokenSource As New CancellationTokenSource()
        Dim token As CancellationToken = tokenSource.Token
        Dim taskReboot As Task(Of TasksResult) = Task(Of TasksResult).Factory.StartNew(Function() CallTryRestartChassis(chassisIPAdress, token), token, token)
        taskReboot.Wait()

        Dim result As TasksResult = taskReboot.Result ' Свойство Result блокирует вызывающий поток до завершения задачи. но уже используется  Wait
        Const caption As String = NameOf(RebootChassis)
        Dim text As String = $"Результат перезагрузки шасси: <{itemChassis.HostName}> равен: {result.Result} ошибка: {If(result.Result, "отсутствует", result.SourseError)}"
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(result.Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
        RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {text}")
        success = result.Result
#End If
        If success AndAlso IsCheckAllConditionReboot() Then
            ' запустить сбор
            SendCommandLoopPeriodToConcreteClient(keyChassis, LoopPeriod)
            SendCommandActivateToConcreteClient(keyChassis, True)
            ' Stop
        End If
    End Sub

    ''' <summary>
    ''' Результат перезагрузки - проверка всех этапов при старте всех шасси
    ''' </summary>
    ''' <returns></returns>
    Private Function IsCheckAllConditionReboot() As Boolean
        If IsCheckAllConnection() AndAlso IsCheckAllGetMetaInfo() AndAlso IsCheckAllInitialize() AndAlso IsCheckAllLaunch() Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Ожидание подключения шасси после перезагрузки.
    ''' Запускает фоновую задачу и ожидает её завершения с результатом.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckAllConnection() As Boolean
        Dim text As String
        Const caption As String = NameOf(IsCheckAllConnection)
        Dim mCheckAllChassisConnection As New CheckAllChassisStep(50, 50, 300, ChassisStepCompleted.IsConnected, Me)
        Dim tsk As Task(Of TasksResult) = Task(Of TasksResult).Factory.StartNew(Function() mCheckAllChassisConnection.WaitAllChassisStep())

        tsk.Wait()

        If tsk.Result.Result = False Then
            text = $"Cледующие шасси не смогли успешно стартовать после перезагрузки: {Environment.NewLine}{tsk.Result.SourseError}"
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
        End If

        text = $"Результат проверки соединения шасси равен: {tsk.Result.Result}"
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(tsk.Result.Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
        RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")

        Return tsk.Result.Result
    End Function

    ''' <summary>
    ''' Ожидание инициализации модулей шасси после перезагрузки.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckAllInitialize() As Boolean
        Dim text As String
        Const caption As String = NameOf(IsCheckAllInitialize)
        Dim mCheckAllChassisConnection As New CheckAllChassisStep(50, 50, 120, ChassisStepCompleted.InitSuccess, Me)
        Dim tsk As Task(Of TasksResult) = Task(Of TasksResult).Factory.StartNew(Function() mCheckAllChassisConnection.WaitAllChassisStep())

        tsk.Wait()

        If tsk.Result.Result = False Then
            text = $"Cледующие шасси не смогли успешно инициализировать модули: {Environment.NewLine}{tsk.Result.SourseError}"
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
        End If

        text = $"Результат проверки инициализации шасси равен: {tsk.Result.Result}"
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(tsk.Result.Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
        RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")

        Return tsk.Result.Result
    End Function

    ''' <summary>
    ''' Ожидание запроса списка разрешённых команд от модулей шасси после перезагрузки.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckAllGetMetaInfo() As Boolean
        Dim text As String
        Const caption As String = NameOf(IsCheckAllGetMetaInfo)
        Dim mCheckAllChassisGetMetaInfo As New CheckAllChassisStep(50, 50, 120, ChassisStepCompleted.GetMetaSuccess, Me)
        Dim tsk As Task(Of TasksResult) = Task(Of TasksResult).Factory.StartNew(Function() mCheckAllChassisGetMetaInfo.WaitAllChassisStep())

        tsk.Wait()

        If tsk.Result.Result = False Then
            text = $"Cледующие шасси не смогли запросить список разрешённых команд: {Environment.NewLine}{tsk.Result.SourseError}"
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{caption}> {text}")
        End If

        text = $"Результат проверки запроса списка разрешённых команд шасси равен: {tsk.Result.Result}"
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(tsk.Result.Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
        RegistrationEventLog.EventLog_MSG_CONNECT($"<{caption}> {text}")

        Return tsk.Result.Result
    End Function

    ''' <summary>
    ''' Ожидание опроса каналов модулей шасси после перезагрузки.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckAllLaunch() As Boolean
        Dim text As String
        Const caption As String = NameOf(IsCheckAllLaunch)
        Dim mCheckAllChassisConnection As New CheckAllChassisStep(50, 50, 120, ChassisStepCompleted.LaunchSuccess, Me)
        Dim tsk As Task(Of TasksResult) = Task(Of TasksResult).Factory.StartNew(Function() mCheckAllChassisConnection.WaitAllChassisStep())

        tsk.Wait()

        If tsk.Result.Result = False Then
            text = $"Cледующие шасси не смогли успешно произвести опрос каналов: {Environment.NewLine}{tsk.Result.SourseError}"
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        text = $"Результат проверки запуска шасси равен: {tsk.Result.Result}"
        InvokeAppendMsgToRichTextBoxByKey(text, KeyRichTexServer, If(tsk.Result.Result, MessageBoxIcon.Question, MessageBoxIcon.Error))
        RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {text}")

        Return tsk.Result.Result
    End Function

    ''' <summary>
    ''' Результат первого запуска системы - проверка следующих этапов при старте всех шасси:
    ''' - ожидать подключение всех шасси;
    ''' - список разрешённых команд считан всеми шасси;
    ''' - первый опрос каналов модулей для всех шасси произведен успешно.
    ''' </summary>
    ''' <returns></returns>
    Private Function IsCheckConditionFirstRun() As Boolean
        If IsCheckAllConnection() AndAlso IsCheckAllGetMetaInfo() AndAlso IsCheckAllLaunch() Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Ожидание подключения всех шасси после их перезагрузки.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsRunChassisTasks() As Boolean
        Dim success As Boolean = False ' результат возвращаемый функцией, по умолчанию  = False

        Try
#If DEBUG_ClientTest = True Then
            'TODO: Для отладки с локальным Клиентом
            'Return True
            ' По всем шасси cRio производится запуск программы (загрузочный файл startup.rtexe). Этот запуск производит небольшая программа на LabView на компьютере управляющего ИВК. 
            ' Она получает список адресов шасси cRio и по этому списку перезапускает контроллеры для автозапуска на них программы или сразу производит подключение и запуск программы. Далее эта программа завершается.

            ' ИВК ожидает подключения всех шасси cRio из списка конфигурации ( в цикле по списку с задержкой до тех пор пока TCP соединение не установится (должен прийти запрос от каждого шасси на состав команд используемых при общении)) или если время TimeOut вышло завершить. 
            ' эдесь запуск не проходит из-за конкуренции потоков
            ' Здесь производится создание сетевых потоков подключений к шасси для получения данных с каждой cRio (функция сбора данных обычного ИВК стенда). Программа ИВК (потоки сетевых подключений к шасси) по отношению с шасси cRio работает как Сервер.
            'success = IsCheckAllConditionReboot() ' TODO: закоментировал
            success = True
#Else
            'If Not CheckAllConnection() Then Return False ' ожидать подключение всех шасси
            'Application.DoEvents()
            'If Not CheckAllGetMetaInfo() Then Return False ' список разрешённых команд считан всеми шасси
            'Application.DoEvents()
            'If Not CheckAllLaunch() Then Return False ' первый опрос каналов модулей для всех шасси произведен успешно
            'Application.DoEvents()
            ' если дошли сюда, то OK
            success = IsCheckConditionFirstRun()
#End If
        Catch ex As WorkTask_ERROR
            Const caption As String = NameOf(IsRunChassisTasks)
            Const text As String = "Ошибка при загрузке модуля"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Catch ex As Exception
            ' Возникла ошибка при работе функции. Производится освобождение всех ресурсов которые были выделены.
            Const caption As String = NameOf(IsRunChassisTasks)
            Dim text As String = ex.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Проверка существования шасси или Сервера путём использования класса PingClient
    ''' </summary>
    ''' <param name="targetIP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckPingClient(targetIP As String) As Boolean
        Dim mPingClient As New PingClient(targetIP, 200, 20) ' "127.0.0.1")
        Dim regularThread As New Thread(AddressOf mPingClient.SendPing)

        ' запустить пинг в другом потоке, чтобы там запустить Методы Send и 
        ' SendAsync - выполняют асинхронную отправку сообщения запроса проверки связи ICMP на удаленный компьютер и 
        ' ожидают от него соответствующее сообщение ответа проверки связи ICMP. 
        regularThread.Start()
        ' ожидание окончания работы потока
        regularThread.Join()

        If mPingClient.SuccessPing = False Then
            Const caption As String = "Ping error."
            Dim text As String = $"Проверка связи с целевым устройством по адресу <{targetIP}>{vbCrLf}завершилась ошибкой: {vbCrLf}{mPingClient.PingDetails}"
            InvokeAppendMsgToRichTextBoxClient(text, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        Return mPingClient.SuccessPing
    End Function
#End Region

#Region "ParseDataBase"
    ' 1. вставить работу с базой данных простой запрос (не типизированный) как при загрузке в настроечной форме
    ' 2. проверить наличие полей Chassis и BoardType

    ''' <summary>
    ''' Разбор конфигурационного файла и заполнение класса Config.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsParseDataBase() As Boolean
        Dim success As Boolean

        Try
            LoadChannels()
            success = True
        Catch e As PrppertyChassisIsNullOrEmpty
            Dim text As String = $"{e.Message}{vbCrLf}Приложение будет закрыто."
            Dim caption As String = "Ошибка загрузки и разбора базы данных: " & PathServerDataBase
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            success = False
        Catch e As Exception
            Dim text As String = $"{e.Message}{vbCrLf}Приложение будет закрыто."
            Dim caption As String = "Ошибка загрузки и разбора базы данных: " & PathServerDataBase
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Загрузка Каналов
    ''' </summary>
    Private Sub LoadChannels()
        Dim rowsCount As Integer
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim aFindValue(0) As Object
        Dim dcDataColumn(1) As DataColumn
        Dim ChannelLast As String = "Channel" & mOptionDataCRIO.StendServer
        Dim PathChannels As String = PathServerDataBase

        odaDataAdapter = New OleDbDataAdapter($"SELECT * FROM {ChannelLast} Order By НомерПараметра",
                                              BuildCnnStr(ProviderJet, PathChannels))
        odaDataAdapter.Fill(dtDataTable)
        rowsCount = dtDataTable.Rows.Count

        If rowsCount = 0 Then
            Dim caption As String = "Ошибка загрузки и разбора базы данных: " & PathServerDataBase
            Dim text As String = $"В базе каналов {ChannelLast} нет записей!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        dcDataColumn(0) = dtDataTable.Columns("НомерПараметра")
        dtDataTable.PrimaryKey = dcDataColumn

        Re.Dim(ChannelsType, rowsCount - 1)

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        For I = 0 To rowsCount - 1
            ChannelsType(I).Initialize()
            aFindValue(0) = I + 1
            drDataRow = dtDataTable.Rows.Find(aFindValue)

            If drDataRow IsNot Nothing Then
                With drDataRow
                    ChannelsType(I).NumberParameter = CShort(.Item("НомерПараметра"))
                    ChannelsType(I).NameParameter = CStr(.Item("НаименованиеПараметра"))

                    If Not IsDBNull(.Item("НомерМодуляКорзины")) Then
                        ChannelsType(I).NumberModuleChassis = CShort(.Item("НомерМодуляКорзины"))
                    Else
                        ChannelsType(I).NumberModuleChassis = -1
                    End If

                    If Not IsDBNull(.Item("НомерКаналаМодуля")) Then
                        ChannelsType(I).NumberChannelModule = CShort(.Item("НомерКаналаМодуля"))
                    Else
                        ChannelsType(I).NumberChannelModule = -1
                    End If

                    ChannelsType(I).TypeConnection = CStr(.Item("ТипПодключения"))
                    ChannelsType(I).LowerMeasure = CSng(.Item("НижнийПредел"))
                    ChannelsType(I).UpperMeasure = CSng(.Item("ВерхнийПредел"))
                    ChannelsType(I).LevelOfApproximation = CShort(.Item("СтепеньАппроксимации"))
                    ChannelsType(I).CoefficientsPolynomial(0) = CDbl(.Item("A0"))
                    ChannelsType(I).CoefficientsPolynomial(1) = CDbl(.Item("A1"))
                    ChannelsType(I).CoefficientsPolynomial(2) = CDbl(.Item("A2"))
                    ChannelsType(I).CoefficientsPolynomial(3) = CDbl(.Item("A3"))
                    ChannelsType(I).CoefficientsPolynomial(4) = CDbl(.Item("A4"))
                    ChannelsType(I).CoefficientsPolynomial(5) = CDbl(.Item("A5"))

                    ChannelsType(I).UnitOfMeasure = CStr(.Item("ЕдиницаИзмерения"))
                    ChannelsType(I).LowerLimit = CSng(.Item("ДопускМинимум"))
                    ChannelsType(I).UpperLimit = CSng(.Item("ДопускМаксимум"))

                    If Not IsDBNull(.Item("Примечания")) Then
                        ChannelsType(I).Description = CStr(.Item("Примечания"))
                    Else
                        ChannelsType(I).Description = CStr(0)
                    End If

                    If Not IsDBNull(.Item("Chassis")) Then
                        ChannelsType(I).Chassis = CStr(.Item("Chassis"))
                    Else
                        Throw New PrppertyChassisIsNullOrEmpty($"Поле: <Chassis> для записи с номером: <{ChannelsType(I).NumberParameter}> таблицы: <{ChannelLast}> пустое.")
                    End If

                    If Not IsDBNull(.Item("BoardType")) Then
                        ChannelsType(I).BoardType = CStr(.Item("BoardType"))
                    Else
                        Throw New PrppertyChassisIsNullOrEmpty($"Поле: <BoardType> для записи с номером: <{ChannelsType(I).NumberParameter}> таблицы: <{ChannelLast}> пустое.")
                    End If
                End With
            End If
        Next
    End Sub

    <SerializableAttribute>
    Private Class PrppertyChassisIsNullOrEmpty
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
#End Region

#Region "Xml файл настроек"
    ''' <summary>
    ''' Сохранить положение окна в файле настроек
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SavePathSettinngXml()
        Try
            ' создать документ
            Dim xmlDocumentSettings = New XDocument()
            ' создать xml описание и установить в документе
            'document.Declaration = New XDeclaration("1.0", Nothing, Nothing)

            ' создать Settings element и добавить в документ
            Dim xmlSettings = New XElement("Settings")
            xmlDocumentSettings.Add(xmlSettings)

            ' создать order инструкцию добавить перед предыдущим элементом
            'Dim pi = New XProcessingInstruction("order", "alpha ascending")
            'Settings.AddBeforeSelf(pi)

            ' создать Location element и добавить в Settings element
            Dim xmlLocation = New XElement("Location")
            xmlSettings.Add(xmlLocation)
            Dim xmlSize = New XElement("Size")
            xmlSettings.Add(xmlSize)

            If WindowState <> FormWindowState.Minimized Then
                ' добавить аттрибуты размерности в Location и Size element 
                xmlLocation.SetAttributeValue("Left", Left)
                xmlLocation.SetAttributeValue("Top", Top)

                xmlSize.SetAttributeValue("Width", Width)
                xmlSize.SetAttributeValue("Height", Height)
                Dim xmlWindowState = New XElement("WindowState", [Enum].GetName(GetType(FormWindowState), WindowState))
                xmlSettings.Add(xmlWindowState)
            Else
                ' добавить аттрибуты размерности в Location и Size element 
                xmlLocation.SetAttributeValue("Left", 0)
                xmlLocation.SetAttributeValue("Top", 0)

                xmlSize.SetAttributeValue("Width", 1024)
                xmlSize.SetAttributeValue("Height", 768)
                Dim WindowState = New XElement("WindowState", [Enum].GetName(GetType(FormWindowState), FormWindowState.Normal))
                xmlSettings.Add(WindowState)
            End If

            Dim xmlDescription = New XElement("Description", Description)
            xmlSettings.Add(xmlDescription)
            xmlDocumentSettings.Save(PathSettingXml)
        Catch ex As Exception
            MessageBox.Show(Me,
                            $"Невозможно сохранить настройки в конфигурационном файле.{Environment.NewLine}Error: {ex.Message}",
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Создание по умолчанию Xml файл настроек.
    ''' </summary>
    ''' <param name="pathSettinngXml"></param>
    ''' <remarks></remarks>
    Private Shared Sub CreateDocumentSettings(ByVal pathSettinngXml As String)
        ' создать документ
        Dim DocumentSettings As XDocument = New XDocument(
                                            New XElement("Settings",
                                                         New XElement("Location", New XAttribute("Left", 0), New XAttribute("Top", 0)),
                                                         New XElement("Size", New XAttribute("Width", 1024), New XAttribute("Height", 768)),
                                                         New XElement("WindowState", "Normal"),
                                                         New XElement("Description", "Ввести описание модуля расчета")))

        DocumentSettings.Save(pathSettinngXml)
    End Sub

    ''' <summary>
    ''' Считать положение окна из файла настроек
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadConfiguration()
        Try
            ' использовать Attribute в полном пути Elements 
            'Dim result = document.<art>.<period>.@name
            ' использовать Attribute для потомка для получения первого аттрибута
            'Dim result = document.<Settings>.<Location>.@Width
            'Dim result = document...<Location>.@Width
            ' значени узла
            'result = document...<WindowState>.Value
            ' поиск узла по порядку
            'Dim periodElement = document...<period>(0)
            'result = periodElement.@name

            Dim DocumentSettings = New XDocument
            DocumentSettings = XDocument.Load(PathSettingXml)

            Me.Left = Convert.ToInt32(DocumentSettings...<Location>.@Left)
            Me.Top = Convert.ToInt32(DocumentSettings...<Location>.@Top)

            Me.Width = Convert.ToInt32(DocumentSettings...<Size>.@Width)
            Me.Height = Convert.ToInt32(DocumentSettings...<Size>.@Height)

            'Dim name As String = _
            '    System.Enum.GetName(GetType(System.Windows.Forms.FormWindowState), System.Windows.Forms.FormWindowState.Normal)

            Dim strWindowState As String = DocumentSettings...<WindowState>.Value
            Dim valuesFormWindowState As Array = [Enum].GetValues(GetType(FormWindowState))
            Dim tempFormWindowState As FormWindowState = FormWindowState.Normal ' по умолчанию

            For I As Integer = 0 To valuesFormWindowState.Length - 1
                If valuesFormWindowState.GetValue(I).ToString = strWindowState Then
                    tempFormWindowState = CType(valuesFormWindowState.GetValue(I), FormWindowState)
                    Exit For
                End If
            Next

            ' восстановить из сохранённых значений
            Me.WindowState = CType(tempFormWindowState, FormWindowState)
            Me.Description = DocumentSettings...<Description>.Value
            'Me.Text = Me.Description 'TODO: закоентировал
        Catch ex As Exception
            MessageBox.Show(CType(Me, IWin32Window),
                            $"Ошибка в процедуре {NameOf(LoadConfiguration)}.{Environment.NewLine}Error: {ex.Message}",
                            Me.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "AcquisitionMainForm"
    ''' <summary>
    ''' Запуск таймера накопления и записи
    ''' </summary>
    ''' <param name="inEventHandlerTimerTick"></param>
    Public Sub StartAcquisitionTimer(inEventHandlerTimerTick As EventHandler)
        'Public Sub StartAcquisitionTimer(inEventHandlerTimerTick As Action(Of Object, EventArgs))
        mmTimerAcquisitionMainForm = New Multimedia.Timer() With {
            .Mode = Multimedia.TimerMode.Periodic,
            .Period = timerIntervalMainFormAcquisition,
            .Resolution = 1
        }

        Thread.CurrentThread.Priority = ThreadPriority.Normal
        ' для отслеживания события в форме назначить объект синхронизации (должен быть компонент)
        ' если таймер работает самостоятельно, то форму назначать не надо
        mmTimerAcquisitionMainForm.SynchronizingObject = mFormMainMDI ' необходимо для отслеживания вызова событий

        eventHandlerTimerTick = inEventHandlerTimerTick

        Try
            AddHandler mmTimerAcquisitionMainForm.Tick, eventHandlerTimerTick
            mmTimerAcquisitionMainForm.Start()
        Catch ex As Exception
            Dim CAPTION As String = $"Error {NameOf(StartAcquisitionTimer)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")
        End Try

        IsStartAcquisition = mmTimerAcquisitionMainForm.IsRunning
    End Sub

    ''' <summary>
    ''' Остановить таймер
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopAcquisition()
        If mmTimerAcquisitionMainForm IsNot Nothing Then
            mmTimerAcquisitionMainForm.Stop() ' может быть не создан. если было нарушение соотвествия конфигурации каналов Сервера
            RemoveHandler mmTimerAcquisitionMainForm.Tick, eventHandlerTimerTick
        End If
        IsStartAcquisition = False
        If RegistrationMain IsNot Nothing Then RegistrationMain.IsFormRunning = False
    End Sub

    ''' <summary>
    ''' Handles mmTimer.Tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub RegistrationTimerTick(sender As Object, e As EventArgs)
        If IsStartAcquisition Then 'TODO: добавить проверку сети AndAlso _Tcp_Client IsNot Nothing AndAlso _Tcp_Client.Connected Then
            OnTimedRegistrationTimer() ' асинхронное чтение
        Else
            mmTimerAcquisitionMainForm.Stop()
            RemoveHandler mmTimerAcquisitionMainForm.Tick, eventHandlerTimerTick
        End If
    End Sub

    ''' <summary>
    ''' Handles mmTimer.Tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub TarirTimerTick(sender As Object, e As EventArgs)
        If IsStartAcquisition Then 'TODO: добавить проверку сети AndAlso _Tcp_Client IsNot Nothing AndAlso _Tcp_Client.Connected Then
            OnTimedTarirTimer() ' асинхронное чтение
        Else
            mmTimerAcquisitionMainForm.Stop()
            RemoveHandler mmTimerAcquisitionMainForm.Tick, eventHandlerTimerTick
        End If
    End Sub

    Private random As Random = New Random
    'Private Sub TestInitialize()
    '    'TODO: убрать тест
    '    For I As Integer = 0 To AcquisitionValueOfDouble.Length - 1
    '        AcquisitionValueOfDouble(I) = I
    '    Next
    'End Sub

    Dim counterTimer As Integer 'TODO: убрать

    Private Function RandomNextDouble(index As Integer) As Double
        Dim amplidude As Double = random.NextDouble * 5.0
        Return Math.Sin(counterTimer / 500.0 * Math.PI) * amplidude + amplidude / 10.0
    End Function

    ''' <summary>
    ''' Общая функция для всех таймеров
    ''' Осуществляет посылку, приём и разборку по массивам полученных данных на синхронном блокирующем сокете
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnTimedRegistrationTimer()
        Dim sync As Integer = Interlocked.CompareExchange(syncTimerAcquisitionMainForm, 1, 0)

        If sync = 0 Then
            'If _Tcp_Client.Connected Then
            ' CompactRioAcquiredData: при получении из событий первый буфер немного быстрее (пропуск 3 такта затем по 2)
            ' но если включена запись и вызвать передачу данных в сеть клиентам, то происходит зависание.
            ' RegistrationMain.DataValuesFromServer = AcquisitionValueOfDouble: при премом вызове пропуск 4 такта затем так же,
            ' восстановление записи происходит нормально.
            'RaiseEvent CompactRioAcquiredData(Me, New AcquiredDataEventArgs(AcquisitionValueOfDouble))

            RegistrationMain.DataValuesFromServer = AcquisitionAverageChassis
            RegistrationMain.AcquiredData()
            syncTimerAcquisitionMainForm = 0 ' освободить

            'Else
            '    syncPoint = 0  ' освободить
            '    StopAcquisition()
            '    'End If
        End If
    End Sub

    ''' <summary>
    ''' Общая функция для всех таймеров
    ''' Осуществляет посылку, приём и разборку по массивам полученных данных на синхронном блокирующем сокете
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnTimedTarirTimer()
        Dim sync As Integer = Interlocked.CompareExchange(syncTimerAcquisitionMainForm, 1, 0)

        If sync = 0 Then
            TarirForm.AcquiredData(AcquisitionAverageChassis)
            syncTimerAcquisitionMainForm = 0 ' освободить
        End If
    End Sub

    'Private TokenSource As CancellationTokenSource
    'Private taskSendReciveWithServer As Task

    'Public Sub RunSendRecive(sendStart As Boolean)
    '    If sendStart Then
    '        TokenSource = New CancellationTokenSource
    '        taskSendReciveWithServer = Task.Factory.StartNew(Sub() LoopSendReciveWithServer(TokenSource.Token), TokenSource.Token, TaskCreationOptions.LongRunning)
    '    Else
    '        If TokenSource IsNot Nothing Then TokenSource.Cancel() ' прервать задачу 
    '        taskSendReciveWithServer = Nothing
    '    End If
    'End Sub

    'Private Sub LoopSendReciveWithServer(ByVal ct As CancellationToken)
    '    ' Прерывание уже было запрошено?
    '    If ct.IsCancellationRequested = True Then
    '        'Console.WriteLine("Прерывание уже было запрошено до запуска.")
    '        'Console.WriteLine("Press Enter to quit.")
    '        ct.ThrowIfCancellationRequested()
    '    End If

    '    ' Внимание!!! Ошибка "OperationCanceledException was unhandled by user code"
    '    ' было вызвано здесь если "Just My Code"
    '    ' был включён и не может быть выключен. Исключение случилось
    '    ' Просто нажать F5 для продолжения выполнения кода

    '    'If TaskSendReciveWithServer IsNot Nothing Then
    '    'TestSend = "DoMonitorConnectionsWithServer TaskSendReciveWithServer=True"
    '    Do
    '        If ct.IsCancellationRequested Then
    '            'ct.ThrowIfCancellationRequested() ' выйти по исключению
    '            Exit Do ' завершить задачу
    '        End If

    '        If taskSendReciveWithServer Is Nothing Then
    '            ' скорее всего обрыв соединения
    '            Exit Sub
    '        End If

    '        StreamWritePacketToServer()

    '        ' Завершить цикл избегая напрасную трату времени CPU
    '        If taskSendReciveWithServer IsNot Nothing Then taskSendReciveWithServer.Wait(timerIntervalSend)
    '    Loop While True
    '    'Else
    '    ' TestSend = "DoMonitorConnectionsWithServer TaskSendReciveWithServer=False"
    '    'End If
    'End Sub

    'Private Sub StreamWritePacketToServer()
    '    ' Асинхронная запись запроса для получения значений сконфигурированных кналов
    'End Sub

    'Public Event CompactRioAcquiredData(ByVal sender As Object, ByVal e As AcquiredDataEventArgs)

    '''' <summary>
    '''' AcquiredDataEventArgs: пользовательское событие наследуется от EventArgs.
    '''' </summary>
    '''' <remarks></remarks>
    'Public Class AcquiredDataEventArgs
    '    Inherits EventArgs

    '    ' получить массив собранных значений
    '    Public Sub New(ByRef arrData As Double())
    '        CompactRioChannelsData = arrData
    '    End Sub

    '    ' сюда можно напихать другие свойства
    '    ' можно передать все накопленные
    '    ' arrСреднее(TempПараметр.ИндексВМассивеПараметров, x)
    '    ' а можно и конкретно осредненные или собранные
    '    'arrПарамНакопленные(N) = dblСреднее
    '    Public Property CompactRioChannelsData() As Double()
    'End Class
#End Region
End Class