Imports System.IO
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Threading
Imports System.ComponentModel
Imports MathematicalLibrary
Imports System.Runtime.Serialization.Formatters.Soap
Imports System.Collections.Generic

Module MainModule

#Region "Глобальные Формы и объекты"
    Public MainMdiForm As FormMainMDI
    Public EditorPanelMotoristForm As FormEditorPanelMotorist
    Public AboutForm As FormAbout
    Public TextEditorForm As FormTextEditor
    ''' <summary>
    ''' Опрос Канала
    ''' </summary>
    ''' <remarks></remarks>
    Public TestChannelForm As FormTestChannel
    Public SettingForm As FormSetting
    Public ServerForm As FormServer
    Public CollectionForms As New FormsCollection
    Public gDigitalInput As DigitalInput
    Public gTdmsFileProcessor As TdmsFileProcessor
    Public gFormsPanelManager As FormsPanelManager
#End Region

#Region "Глобальные флаги"
    Public IsFrmServiceBasesLoaded As Boolean
    Public IsFrmDigitalOutputPortStart As Boolean
    Public IsMonitorDigitalOutputPort As Boolean
    ''' <summary>
    ''' Форма Сервера Запущена
    ''' </summary>
    ''' <remarks></remarks>
    Public IsFormSereverStart As Boolean
    ''' <summary>
    ''' Форма Клиента Запущена
    ''' </summary>
    ''' <remarks></remarks>
    Public IsFormClientStart As Boolean

    Public IsDigitalInput As Boolean
    ''' <summary>
    ''' Автоматический запуск из коммандной строки
    ''' </summary>
    ''' <remarks></remarks>
    Public IsRunAutoBooting As Boolean
    ''' <summary>
    ''' Сервер Включен для передачи данные
    ''' </summary>
    ''' <remarks></remarks>
    Public IsServerOn As Boolean
    ''' <summary>
    ''' Подписка На Событие Сбора Включена SubscriptionOnEventsAcquisitionEnabled
    ''' </summary>
    ''' <remarks></remarks>
    Public IsSubscriptionOnEventsAcquisitionEnabled As Boolean
    ''' <summary>
    ''' Фокус На Клиента
    ''' </summary>
    ''' <remarks></remarks>
    Public IsFocusToClient As Boolean
    ' Режимы запуска программы, установленные при запуске в настроечной форме
    Public IsServer, IsClient, IsWorkWithDaqController, IsTcpClient, IsCompactRio, IsSnapshot As Boolean
    ''' <summary>
    ''' Флаг того, что сообщение было выведено один раз
    ''' </summary>
    Public IsCheckEmptyChassis As Boolean
    ''' <summary>
    ''' Опрос Оборотов
    ''' </summary>
    ''' <remarks></remarks>
    Public IsQuestioningRevolution As Boolean
    ''' <summary>
    ''' База Сменена
    ''' </summary>
    ''' <remarks></remarks>
    Public IsDataBaseChanged As Boolean
    ''' <summary>
    ''' Групповой Экспорт Снимков
    ''' </summary>
    ''' <remarks></remarks>
    Public IsGroupExportSnapshot As Boolean
    ''' <summary>
    ''' Объединить Выделенные Снимки
    ''' </summary>
    ''' <remarks></remarks>
    Public IsUniteDetachedSnapshot As Boolean
    ''' <summary>
    ''' Регистратор Загружен
    ''' </summary>
    ''' <remarks></remarks>
    Public IsRegistrationLoaded As Boolean
    ''' <summary>
    ''' Проверка Свободного Места Проведена
    ''' </summary>
    Public IsCheckFreePlace As Boolean
    ''' <summary>
    ''' Использован формат записи Tdms
    ''' </summary>
    Public IsUseTdms As Boolean
#End Region

#Region "TcpClient"
    Public PathServerCfglmzXml As String ' путь к конфигурационному файлу сервера
    Public PathChannels_cfg_lmz As String
    Public Const CHANNEL_N As String = "ChannelN"
    Public HostName As String '= "127.0.0.1"
    Public PortTCP As Integer ' = 1701
    ' список измерительных каналов и каналов времени
    Public OutputChannelsBindingList As New ChannelsOutputBindingList
#End Region

#Region "Структуры"
    Public Structure TypeBaseParameter
        Dim NumberParameter As Short            ' НомерПараметра
        Dim NameParameter As String             ' НаименованиеПараметра
        Dim NumberChannel As Short              ' НомерКанала DAQ
        Dim NumberDevice As Short               ' НомерУстройства или корзины
        Dim NumberModuleChassis As Short        ' Номер модуля в корзине
        Dim NumberChannelModule As Short        ' Номер канала модуля
        Dim TypeConnection As String            ' ТипПодключения
        Dim LowerMeasure As Single              ' НижнийПредел
        Dim UpperMeasure As Single              ' ВерхнийПредел
        Dim SignalType As String                ' ТипСигнала
        Dim NumberFormula As Short              ' НомерФормулы
        Dim LevelOfApproximation As Short       ' СтепеньАппроксимации
        <VBFixedArray(5)> Dim Coefficient As Double()
        Dim CompensationXC As Boolean
        Dim Offset As Double                    ' Смещение
        Dim UnitOfMeasure As String             ' ЕдиницаИзмерения
        Dim LowerLimit As Single                ' ДопускМинимум 
        Dim UpperLimit As Single                ' ДопускМаксимум 
        Dim RangeYmin As Short                  ' РазносУмин
        Dim RangeYmax As Short                  ' РазносУмакс
        Dim AlarmValueMin As Single             ' АварийноеЗначениеМин
        Dim AlarmValueMax As Single             ' АварийноеЗначениеМакс
        Dim Blocking As Boolean                 ' Блокировка
        Dim IsVisible As Boolean                ' Видимость
        Dim IsVisibleRegistration As Boolean    ' ВидимостьРегистратор
        Dim Mistake As Single                   ' Погрешность
        Dim Description As String               ' Примечания

        Public Sub Initialize()
            Re.Dim(Coefficient, 5)
        End Sub
    End Structure

    Public Structure TypeSmallParameter
        Dim NameParameter As String     ' НаименованиеПараметра
        Dim NumberParameter As Short    ' НомерПараметра
        Dim IsVisible As Boolean
        Dim NumberInList As Short       ' НомерВЛисте
        Dim UnitOfMeasure As String     ' ЕдиницаИзмерения
    End Structure

    Public ParametersType As TypeBaseParameter()
    Public ParametersShaphotType As TypeBaseParameter()

    ''' <summary>
    ''' Координаты
    ''' </summary>
    Public Structure Coordinate
        Dim x As Double
        Dim y As Double
    End Structure

    Public Structure GraphParametersByParameter
        Dim NameParameter As String     ' Наименование Параметра
        Dim NumberTail As Integer       ' Номер Параметра
        Dim NumberAxis As Integer       ' Номер Оси 
        Dim IndexColor As Integer       ' Цвет
        Dim IsTailVisible As Boolean    ' Отображать Шлейф
        Dim UnitOfMeasure As String     ' Единица Измерения
    End Structure

    Public Structure TypeBaseParameterTCP
        Dim NumberParameter As Integer      ' НомерПараметра
        Dim NameParameter As String         ' НаименованиеПараметра
        Dim UnitOfMeasure As String         ' ЕдиницаИзмерения
        Dim LowerLimit As Single            ' ДопускМинимум
        Dim UpperLimit As Single            ' ДопускМаксимум 
        Dim RangeYmin As Integer            ' РазносУмин
        Dim RangeYmax As Integer            ' РазносУмакс
        Dim AlarmValueMin As Single         ' АварийноеЗначениеМин
        Dim AlarmValueMax As Single         ' АварийноеЗначениеМакс
        Dim IsVisibleRegistration As Boolean ' ВидимостьРегистратор
        Dim Description As String           ' Примечания
        Dim LevelWarning As LevelWarningWhat
        Dim MessageWarning As String        ' сообщение о причине аварии

        'Public Sub Initialize()
        '    'TypeNetVar = EnumTypeNetVar.Double
        'End Sub

        Public Overrides Function ToString() As String
            Return NameParameter
        End Function
    End Structure

    Public ParametersTCP As TypeBaseParameterTCP()

    Public Enum LevelWarningWhat
        Normal 'нормальный уровень
        Alert ' больше уровня предупреждения АварийноеЗначениеМин
        Alarm ' больше аварийного уровня АварийноеЗначениеМакс
    End Enum
#End Region

#Region "Enum"
    ''' <summary>
    ''' Расположение
    ''' </summary>
    Public Enum Disposition
        Left = 1
        Right = 2
        None = 3
    End Enum

    ''' <summary>
    ''' Тип Работы Авт Архивирования
    ''' </summary>
    Public Enum TypeWorkAutomaticBackup
        RegistrationOnStart = 1 'Регистратор При Запуске
        Snapshot = 2 'Снимок
    End Enum

    Public Const cVeryFast As String = "Очень быстро"
    Public Const cQuickly As String = "Быстро"
    Public Const cNormally As String = "Нормально"
    Public Const cSlowly As String = "Медленно"
    ''' <summary>
    ''' Скорость отображения текстовых полей
    ''' </summary>
    <Flags()>
    Public Enum DisplayRate
        <Description(cVeryFast)>
        VeryFast = 1
        <Description(cQuickly)>
        Quickly = 2
        <Description(cNormally)>
        Normally = 4
        <Description(cSlowly)>
        Slowly = 8
    End Enum

    ''' <summary>
    ''' Тип стрелки
    ''' </summary>
    Public Enum ArrowType As Integer
        <Description("горизонтальная")>
        Horizontal = 1
        <Description("вертикальная")>
        Vertical = 2
        <Description("наклонная")>
        Inclined = 3
    End Enum

    Public Const B As String = "Б"
    Public Const UB As String = "УБ"
    Public Const O_R As String = "ОР"

    Public Const cEngine99A As String = "99А серия 03 и 04"
    Public Const cEngine99B As String = "99Б"
    Public Const cEngine39 As String = "39"
    Public Const cEngineM1 As String = "M1"
    Public Const cEngineM1_25_1 As String = "M1 25.1"
    Public Const cEngine39_3 As String = "39 сер.3"
    Public Const cEngine222 As String = "АИ-222-25"

    ''' <summary>
    ''' Тип изделия
    ''' </summary>
    <Flags()>
    Public Enum EngineType
        <Description(cEngine99A)>
        Engine99A = 1
        <Description(cEngine99B)>
        Engine99B = 2
        <Description(cEngine39)>
        Engine39 = 4
        <Description(cEngineM1)>
        EngineM1 = 8
        <Description(cEngineM1_25_1)>
        EngineM1_25_1 = 16
        <Description(cEngine39_3)>
        Engine39_3 = 32
        <Description(cEngine222)>
        Engine222 = 64
    End Enum

    Public Const cKRD_A As String = "КРД-А"
    Public Const cKRD_B As String = "КРД-Б"
    Public Const cARD_39 As String = "АРД-39"
    Public Const cKRD_99_C As String = "КРД99Ц"
    Public Const cCRD_99 As String = "ЦРД99"
    Public Const cESU_222 As String = "ЭСУ-222"

    ''' <summary>
    ''' Тип КРД
    ''' </summary>
    <Flags()>
    Public Enum KRDType
        <Description(cKRD_A)>
        KRD_A = 1
        <Description(cKRD_B)>
        KRD_B = 2
        <Description(cARD_39)>
        ARD_39 = 4
        <Description(cKRD_99_C)>
        KRD_99_C = 8
        <Description(cCRD_99)>
        CRD_99 = 16
        <Description(cESU_222)>
        ESU_222 = 32
    End Enum

    ''' <summary>
    ''' Типы результатов окончившейся операции по архивированию
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BackupingResult
        BackupingIsSuccess ' Сжатие Произведено
        TimeOut ' Время Сжатия Истекло
    End Enum
#End Region

#Region "Const"
    Public Const FormServiceBasesText As String = "Редактирование базы каналов"
    Public Const TagFormDaughter As String = "FormIsDaughter"
    Public Const TagFormTarir As String = "FormIsTarir"
    Public Const TagFormSnapshot As String = "FormIsNavigatorSnapshot"
    Public Const csAppName As String = "Asc Search"
    Public Const LimitControl As Short = 16
    Public Const Kelvin As Double = 273.15
    Public Const Const288 As Double = 288.15
    ''' <summary>
    ''' Частота Ручного Опроса
    ''' </summary>
    ''' <remarks></remarks>
    Public Const FrequencyHandQuery As Short = 200
    ''' <summary>
    ''' Степень Передиск Снимка
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DegreeDiscreditPhoto As Short = 4
    ''' <summary>
    ''' 100 Мб на систему
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MinFreeSpace As Short = 100
    Public Const UnitOfMeasureString As String = "%-дел-мм-градус-Кгсм-мм/с-мкА-кг/ч-кгс"
    Public UnitOfMeasureArray As String() = {"%", "дел", "мм", "градус", "Кгсм", "мм/с", "мкА", "кг/ч", "кгс"}
    Public Const Separator As String = "\"
    Public Const MissingParameter As String = "Отсутствует"
    Public Const ProviderJet As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
    Public Const con9999999 As Integer = 9999999
    Public Const IndexDiscreteInput As Single = 1000
    Public Const indexCalculated As Single = 10000
    ''' <summary>
    ''' ДобавленийНеБолее 65536/1800=36 чтобы влезло в Excel примерно 54 для 20 герц
    ''' </summary>
    ''' <remarks></remarks>
    Public LimitAddExel As Integer = 36
    Public Const RootDirectory As String = "Store"
    ''' <summary>
    ''' Ограничение Снимка = 512 
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SnapshotLimit As Short = 512
    Public Const SOAP_FORMATTER_CONTROL_XML As String = "SoapFormatterControl" & ".xml"

    Public Const TextControlFormTuning As String = "TextControl"
    Public Const GraphControlFormTuning As String = "GraphControl"
    Public Const SelectiveControlFormTuning As String = "SelectiveControl"
    Public Const CRIOChassisList As String = "СписокШассиCRio"
    Private Const Chassis As String = "Chassis"
    Private Const BoardType As String = "BoardType"
    Public Const UseCompactRio As String = "UseCompactRio"
#End Region

#Region "Path Application"
    Public PathResourses As String
    Public PathChannels As String
    Public PathExcel As String
    Public PathXmlFileCondition As String
    ''' <summary>
    ''' XmlFile Графики От Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public PathXmlFileGraphByParameters As String
    ''' <summary>
    ''' Путь Протокол
    ''' </summary>
    ''' <remarks></remarks>
    Public PathProtocol As String
    Public PathOptions As String
    ''' <summary>
    ''' Путь Панели Моториста
    ''' </summary>
    ''' <remarks></remarks>
    Public PathPanelMotorist As String
    ''' <summary>
    ''' Путь Модули Расчета
    ''' </summary>
    ''' <remarks></remarks>
    Public PathSolveModule As String
    ''' <summary>
    ''' Путь Модули СбораКТ
    ''' </summary>
    ''' <remarks></remarks>
    Public PathModuleSolveKT As String
    Public PathChannelsDigitalOutput As String
    ''' <summary>
    ''' Server Рабочий Каталог
    ''' </summary>
    ''' <remarks></remarks>
    Public ServerWorkingFolder As String
    ''' <summary>
    ''' Клиент Рабочий Каталог
    ''' </summary>
    ''' <remarks></remarks>
    Public ClientWorkingFolder As String
    Public PathExportFolderXLS As String
    'Public PathCryptoSource As String
    'Public PathTripleDESSaltIVFile As String
    'Public PathCryptoCurrentKeyFile As String
    Public PathTempFileSoap As String
#End Region

#Region "Array"
    ''' <summary>
    ''' Массив индексов выбранных в конфигурации Параметров Контроля
    ''' </summary>
    ''' <remarks></remarks>
    Public IndexParametersForControl As Integer()
    ''' <summary>
    ''' Индексы Для Группового Экспорта
    ''' </summary>
    ''' <remarks></remarks>
    Public IndexForGroupExport As Integer()
    ''' <summary>
    ''' Индексы Для Слияния Снимков
    ''' </summary>
    ''' <remarks></remarks>
    Public IndexForMergerSnapshot As Integer()
    ''' <summary>
    ''' для копирования локального arrСписПарамКопия
    ''' </summary>
    ''' <remarks></remarks>
    Public CopyListOfParameter As Integer()
    ''' <summary>
    ''' Парам Накопленные
    ''' </summary>
    ''' <remarks></remarks>
    Public ParameterAccumulate As Double()
    ''' <summary>
    ''' Парам Второй График
    ''' </summary>
    ''' <remarks></remarks>
    Public ParameterTwoGraph As Double()
    ''' <summary>
    ''' Имена Параметров От Сервера
    ''' </summary>
    ''' <remarks></remarks>
    Public NameParametersFromServer As String()
    Public MetrologyGroup As Short()
    ''' <summary>
    ''' Значения Цифровых Входов
    ''' </summary>
    ''' <remarks></remarks>
    Public DigitalInputValue As Double()
    ''' <summary>
    ''' Имена Каналов Цифровых Входов
    ''' </summary>
    ''' <remarks></remarks>
    Public NameDigitalInputChannels As String()
    Public ColorsNet() As Color = {Color.White, Color.Lime, Color.Red, Color.Yellow, Color.DeepSkyBlue, Color.Cyan, Color.Magenta, Color.Silver}
#End Region

#Region "Text variable"
    ''' <summary>
    ''' Строка Справки
    ''' </summary>
    ''' <remarks></remarks>
    Public HelpString As String
    ''' <summary>
    ''' для навигации WebBrowser
    ''' </summary>
    ''' <remarks></remarks>
    Public PathHelps As String
    ''' <summary>
    ''' Форма Регистратор
    ''' </summary>
    ''' <remarks></remarks>
    Public RegistrationFormName As String
    Public CaptionGraf As String = ""
#End Region

#Region "Stand"
    ''' <summary>
    ''' для Сервера
    ''' </summary>
    ''' <remarks></remarks>
    Public AddressURL As String
    ''' <summary>
    ''' для Клиента
    ''' </summary>
    ''' <remarks></remarks>
    Public AddressURLServer As String
    ''' <summary>
    ''' имя последней таблицы каналов данного стенда
    ''' </summary>
    ''' <remarks></remarks>
    Public ChannelLast As String
    ''' <summary>
    ''' имя таблицы каналов при которой был записан снимок
    ''' </summary>
    ''' <remarks></remarks>
    Public ChannelShaphot As String
    Public StandNumber As String
    Public GKeyID As Integer
    ''' <summary>
    ''' количество знаков после точки
    ''' </summary>
    ''' <remarks></remarks>
    Public Precision As Integer
    ''' <summary>
    ''' Для Сервера - число клиентов и соответствующее число вкладок
    ''' Для Клиента - номер клиента и соответсвующая вкладка
    ''' </summary>
    Public CountClientOrNumberClient As Integer
#End Region

#Region "Engine"
    Public NumberEngine As Integer
    Public ModificationEngine As String
    Public ModeRegime As String = B ' "Б" "УБ" "ОР"
    Public TypeKRD As String
    ''' <summary>
    ''' Температура Бокса
    ''' </summary>
    ''' <remarks></remarks>
    Public TemperatureOfBox As Double
    ''' <summary>
    ''' Тбокса В Снимке
    ''' </summary>
    ''' <remarks></remarks>
    Public TemperatureBoxInSnaphot As Double
    Public TemperatureTxc As Double
    Public NameTBox As String = "tбокса"
    Public NameTx As String = "Т хс"
    Public NameVarStopWrite As String = "N1"
    Public NameVarStartWrite As String = "Запуск"
    Public ValueStopWrite As Double
    Public WaitStopWrite, WaitStartWrite As Integer
    ''' <summary>
    ''' Номер Режима
    ''' </summary>
    ''' <remarks></remarks>
    Public NumberRegime As Short
    ''' <summary>
    ''' Данные В Сеть
    ''' </summary>
    ''' <remarks></remarks>
    Public RefreshDataToNetwork As Integer
#End Region

#Region "Acquisition"
    ''' <summary>
    ''' частота фонового
    ''' </summary>
    ''' <remarks></remarks>
    Public FrequencyBackground As Short
    ''' <summary>
    ''' длительность периода
    ''' </summary>
    ''' <remarks></remarks>
    Public DeltaX As Single
    ''' <summary>
    ''' передискретизация
    ''' </summary>
    ''' <remarks></remarks>
    Public LevelOversampling As Integer
    ''' <summary>
    ''' 5 секунд будет шлейф на графике от а1 и а2
    ''' </summary>
    ''' <remarks></remarks>
    Public Dynamics As Short
    ''' <summary>
    ''' ОбновлениеЭкрана глобальная через сколько опросов обновлять цифры при регистрации
    ''' </summary>
    ''' <remarks></remarks>
    Public RefreshScreen As Integer
    ''' <summary>
    ''' ОбновлениеЭкрана глобальная через сколько опросов обновлять цифры при регистрации
    ''' </summary>
    ''' <remarks></remarks>
    Public RefreshScreen2 As Integer
    Public TextDisplayRate As DisplayRate
    ''' <summary>
    ''' Длительность Кадра секунд
    ''' </summary>
    ''' <remarks></remarks>
    Public TimeFrame As Short
    ''' <summary>
    ''' КолОпросов
    ''' </summary>
    ''' <remarks></remarks>
    Public CountAcquisition As Integer
    ''' <summary>
    ''' КолОпросовSCXI
    ''' </summary>
    ''' <remarks></remarks>
    Public CountAcquisitionSCXI As Integer
    Public CoefficientPolynomial2D As Double(,)
    Public ADCAcquisitionParametersCount, AdditionalParameterCount, DigitalParametersCount As Integer
    ''' <summary>
    ''' Кол Расчетных Параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public CountSolveParameters As Integer
    ''' <summary>
    ''' Счетчик для отображения графика от параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public CounterParametersGraph As Integer
    ''' <summary>
    ''' Счетчик свечения шлейфа графика от параметров
    ''' </summary>
    ''' <remarks></remarks>
    Public CounterLightParametersGraph As Integer
    ''' <summary>
    ''' последняя Конфигурация графиков от параметра
    ''' </summary>
    ''' <remarks></remarks>
    Public SettingKeyConfiguration As Integer
    ''' <summary>
    ''' Минимальный Предел Параметра
    ''' </summary>
    ''' <remarks></remarks>
    Public MinLimitParameter As Double
    ''' <summary>
    ''' Максимальный Предел Параметра
    ''' </summary>
    ''' <remarks></remarks>
    Public MaxLimitParameter As Double
#End Region

    'Public TagValueHashCodeNew As Integer
    'Public TagValueHashCodeOld As Integer
    'Public uiTaskScheduler As Tasks.TaskScheduler 'контекст синхронизации задачи, чтобы не было конфликта потоков
    'Public AssemblyName As String = System.Configuration.ConfigurationSettings.AppSettings("CalculationAssemblyFilename")
    'Public AssemblyName As String = System.Configuration.ConfigurationManager.AppSettings("CalculationAssemblyFilename") '.AppSettings("CalculationAssemblyFilename")
    'Dim config As System.Configuration.ConfigurationSettings = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

    Public Sub Main(ByVal cmdArgs() As String)
        Dim sTitle As String
        Dim IRetVal As Integer ' результат вызова Windows API функций
        Const caption As String = "Запуск приложения"

        AnalysisArguments(cmdArgs)

        If (UBound(Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)) > 0) = True Then
            Dim text As String = "Приложение уже запущено!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            sTitle = "Автоматизированная система регистрации"
            Dim hWnd As Integer = NativeMethods.FindWindow(Nothing, sTitle)
            If hWnd <> 0 Then
                IRetVal = NativeMethods.ShowWindow(hWnd, NativeMethods.SW_RESTORE)
                IRetVal = NativeMethods.SetForegroundWindow(CType(hWnd, IntPtr))
            End If
            'System.Environment.Exit(0) 'End
        End If

        Dim myCIintl As CultureInfo = CultureInfo.CurrentCulture
        Dim myNFI As NumberFormatInfo = myCIintl.NumberFormat
        Dim strSeparate As String = myNFI.PercentDecimalSeparator
        If strSeparate <> "." Then
            Dim text As String = $"Необходимо произвести следующие настройки в системе:{vbCrLf}Панель управления ->{vbCrLf}Язык ->{vbCrLf}Изменение форматов даты, времени и чисел ->{vbCrLf}Дополнительные параметры... ->{vbCrLf}Разделитель целой и дробной части ->{vbCrLf}Поставить точку (.)"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
            'наверно работает для текущего потока а в панели управления не меняет
            'Dim newCInfo As CultureInfo = CType(Thread.CurrentThread.CurrentCulture.Clone, CultureInfo)
            'newCInfo.NumberFormat.NumberDecimalSeparator = "."
            'newCInfo.NumberFormat.PercentDecimalSeparator = "."
            'Thread.CurrentThread.CurrentCulture = newCInfo
        End If

        'PathResourses = VB6.GetPath & "\Ресурсы" ' получить текущий путь к директории приложения (это работает).
        PathResourses = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), RootDirectory) ' заменил на это
        'PathResourses = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) & "\Ресурсы"
        'PathResourses = Path.GetDirectoryName(Directory.GetCurrentDirectory) & "\Ресурсы"

        'Dim strPathToSettingsFile As String = GetLocationOfSettingsFile()
        'Public Function GetLocationOfSettingsFile() As String
        '    Dim strPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        '    strPath &= "\какой-то путь"
        '    If Not IO.Directory.Exists(strPath) Then
        '        IO.Directory.CreateDirectory(strPath)
        '    End If
        '    strPath &= "\Settings.XML"
        '    Return strPath
        'End Function

        'Directory.SetCurrentDirectory(Application.StartupPath)
        '    strCurrentDirectory = Microsoft.VisualBasic.Left(Environment.CurrentDirectory, Len(Environment.CurrentDirectory) - 3)

        'Dim directoryInfo As System.IO.DirectoryInfo
        'Dim directoryFile As System.IO.DirectoryInfo
        '' получить путь запуска приложения
        'directoryInfo = System.IO.Directory.GetParent(Application.StartupPath)
        '' установить выходной путь
        'If directoryInfo.Name.ToString() = "bin" Then
        '    directoryFile = System.IO.Directory.GetParent(directoryInfo.FullName)
        '    strFile = directoryFile.FullName + "\Условия.xml"
        'Else
        '    strFile = directoryInfo.FullName + "\Условия.xml"
        'End If

        'PathCryptoSource = PathResourses & "\complete.dat"
        '' установить путь для обоих файлов ключей
        'PathTripleDESSaltIVFile = PathResourses & "\Data.dat"
        '' установить текущий файл ключа к файлу по умолчанию секретного ключа
        'PathCryptoCurrentKeyFile = PathTripleDESSaltIVFile

        'If FileExists(PathCryptoSource) Then
        '    text = "В каталоге нет файла <complete.dat>!"
        '    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        '    Environment.Exit(0) 'End
        'End If

        'If FileExists(PathTripleDESSaltIVFile) Then
        '    text = "В каталоге нет файла <Data.dat>!"
        '    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        '    Environment.Exit(0) 'End
        'End If

        Application.EnableVisualStyles() ' включить стиль XP
        'Thread.CurrentThread.Priority = ThreadPriority.AboveNormal

        ' заставка
        Dim mfrmSplash As FormSplash = New FormSplash
        mfrmSplash.Show()
        mfrmSplash.Refresh()
        'mfrmSplash.UpdateStatusAsync()
        'mfrmSplash.UpdateStatus()
        ConfigPath()

        MainMdiForm = New FormMainMDI
        ' загрузка шапки
        SettingForm = New FormSetting
        SettingForm.ShowDialog()
        'mfrmSplash.UpdateStatusRevers()
        mfrmSplash.Close()

        ' настройка пользовательского интерфейса в зависимости от режима работы
        With MainMdiForm
            .MenuNewWindowRegistration.Enabled = False
            .MenuNewWindowSnapshot.Enabled = False
            .MenuNewWindowTarir.Enabled = False
            .MenuNewWindowClient.Enabled = False

            If IsWorkWithDaqController Then
                .MenuNewWindowRegistration.Enabled = True
                .MenuNewWindowSnapshot.Enabled = True
                .MenuNewWindowTarir.Enabled = True
            End If

            If IsCompactRio Then
                .MenuNewWindowRegistration.Enabled = True
                .MenuNewWindowTarir.Enabled = True
                .MenuNewWindowEvents.Enabled = False
            End If

            If IsTcpClient Then
                If FileNotExists(PathChannels_cfg_lmz) Then
                    ' данная проверка после считывания конфигурационного файла
                    Dim text As String = "В каталоге нет файла Channels_cfg_lmz.mdb!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Environment.Exit(0) 'End'        'Global.System.Windows.Forms.Application.Exit()
                End If

                .MenuNewWindowRegistration.Enabled = True
                .MenuNewWindowSnapshot.Enabled = True
            End If

            If IsSnapshot Then
                .MenuNewWindowSnapshot.Enabled = True
            End If

            If IsClient Then
                .MenuNewWindowClient.Enabled = True
                .MenuNewWindowDBaseChannels.Enabled = False
            End If
        End With

        Application.Run(MainMdiForm)
    End Sub

    ''' <summary>
    ''' Парсинг ключей аргументов консольного запуска программы
    ''' </summary>
    ''' <param name="cmdArgs"></param>
    Private Sub AnalysisArguments(ByVal cmdArgs() As String)
        ' Список имен shr-файлов, переданных программе в командной строке.
        'Dim arysConfigs As List(Of String) = Nothing
        ' Цикл разбора аргументов. 
        Try
            'Registration.exe -r -SCppStr-2.shr -SCppStr.shr -LTestLog.txt -DC:\TEMP\Test
            '--- Назначение ключей для автоматического запуска программы Registration из автозагрузки по ярлыку или .BAT файлу:

            '-R - Автоматический запуск приложения
            '-S - Номер стенда {1, 2, 3, 4}
            '-T – Тип изделия {117, T117С} (где буква С русская)
            '-N - Номер изделия (например 1001)
            '-F – Частота опроса {1, 2, 5, 10, 20, 50, 100}
            '-H - Справка по ключам

            ' Пример:
            'D:\Registration.exe -r -S2 -T117С -N10 -F100
            'Dim cmdArgs() As String = New String() {"-r", "-S2", "-T117С", "-N10", "-F10"} ' {"-?"} '"-H", Тест

            StandNumber = "1"
            ModificationEngine = cEngine99B
            NumberEngine = 1
            FrequencyBackground = 100

            For Each arg As String In cmdArgs
                ' В эту строку будет выделен ключ параметра.
                Dim sKey As String = ""
                ' Проверяем параметр на наличие ключа. Допускается задавать ключ
                ' с помощью символов «/» и «-».
                If arg.Length >= 2 AndAlso (arg(0) = "/"c OrElse arg(0) = "-"c) Then
                    ' Считываем ключ параметра (символ идущий за '/' или '-'.
                    ' Ключ определяет, как будет интерпретироваться данный параметр.
                    sKey = arg(1).ToString()
                Else
                    ' Конвертация в строку (char тоже объект)
                    ' Если параметр не является ключом, возбуждаем исключение, которое
                    ' перехватывается оператором catch (см. ниже). 
                    Throw New Exception($"Неправильный аргумент ""{arg}"". Для большей информации произведите запуск программы с -h ключом.")
                End If

                ' Приложение должно понимать имена ключей, введенных в любом 
                ' регистре. Поэтому перед проверками приводим строку с ключем к
                ' верхнему регистру.
                sKey = sKey.ToUpper()

                ' Если параметр содержит кроме ключа еще и дополнительную строку 
                ' с именем файла или каталога, выделяем эту строку и помещаем
                ' в переменную sFile. Для этого достаточно отбросить два первых
                ' символа.
                Dim sValArg As String = If(arg.Length > 2, arg.Substring(2, arg.Length - 2), "")

                ' Теперь настала очередь определить тип параметра.
                Select Case sKey
                    ' Help
                    Case "H", "?"
                        ' По ключу "H" необходимо вывести описание других ключей.
                        ' Можно было бы просто встроить текст сообщения в тело программы.
                        ' Но, во-первых, это очень неудобно (текст довольно большой, а
                        ' во-вторых, это вообще не правильно, так как его будет трудно
                        ' править и, если возникнет задача перевода приложения на другой
                        ' язык, придется вносить изменения в исходный код программы.
                        ' Выходом может служить хранение текста в ресурсах приложения.
                        ' Следующий код считывает текст сообщения из ресурсов (точнее 
                        ' манифеста приложения).
                        ' Текстовый файл, содержащий сообщение, был добавлено в проект
                        ' и в его свойстве "Build Action" было установлено значение
                        ' "Embedded Resource". Открыть свойства файла можно выбрав 
                        ' соответствующий пункт из контекстного меню в окне 
                        ' «Solution Explorer».
                        ' Как и в случае с файлом StreamReader, позволяем читать данные 
                        ' в виде строки (значительно упрощая жизнь).
                        ' Считать данные из ресурсов можно с помощью следующего кода: 
                        ' Считывание ресурсов производится функцией
                        ' GetManifestResourceStream. Она вызывается у сборки (Assembly),
                        ' в которой расположен ресурс (в данном случае у exe-модуля).
                        ' Имя ресурса состоит из имени пространства имен "Registration"
                        ' и имени файла. Имя пространства имен добавляется VS.Net. Если
                        ' добавлять ресурсы в модуль вручную (с помощью утилит командной
                        ' строки, имена ресурсов можно будет задавать самостоятельно.
                        ' Чтобы получить корректные данные (на русском языке), нужно
                        ' указать кодировку. Если задать значение Encoding.Default,
                        ' будет браться текущие системные настройки. Но файл содержит
                        ' данные в кодировке 1251, а она может не совпадать с текущей
                        ' кодировкой. Поэтому лучше задать кодировку жестко.

                        'Using sr As New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Registration.Promt.txt"), System.Text.Encoding.GetEncoding(1251))
                        '    ' Читаем все данные в строку (sr.ReadToEnd()) 
                        '    MessageBox.Show(sr.ReadToEnd(), "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'End Using

                        MessageBox.Show(My.Resources.Promt, NameOf(Registration), MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Select
                    'Case "L"
                    '    ' Log (задает Log-файл)
                    '    ' Проверяем, не был ли задан Log-файл ранее.
                    '    If sLogFileName IsNot Nothing Then
                    '        ' Если был, возбуждаем исключение.
                    '        Throw New Exception("Допустимо определить только один Log-file как параиетр!")
                    '    End If
                    '    sLogFileName = sFile
                    '    ' Запоминаем Log-файл.
                    '    Exit Select
                    'Case "D"
                    '' Dir (каталог для пакетного режима)
                    '' Возбуждаем исключение, если каталог уже задан.
                    'If sPath IsNot Nothing Then
                    '    Throw New Exception("Допустимо определить только один каталог как параиетр!")
                    'End If
                    'sPath = sFile
                    '' Запоминаем путь.
                    'Exit Select
                    Case "R" ' Run автозапуск
                        IsRunAutoBooting = True
                        Exit Select
                    Case "S" ' Номер стенда
                        StandNumber = sValArg
                        Exit Select
                    Case "T" ' Types тип изделия
                        ModificationEngine = sValArg
                        Exit Select
                    Case "N" ' Номер изделия
                        NumberEngine = CInt(sValArg)
                        Exit Select
                    Case "F" ' Frequency сбора
                        FrequencyBackground = CShort(sValArg)
                        Exit Select

                        'Case "S"
                        '    ' Задает shr-файл настройки, может встречаться более
                        '    ' одного раза.
                        '    ' Если массив еще не создан (ключ S встретился впервые)... 
                        '    If arysConfigs Is Nothing Then
                        '        arysConfigs = New ArrayList()
                        '    End If
                        '    ' ...создаем его.
                        '    arysConfigs.Add(sFile)
                        '    ' Добавляем shr-файл в массив.
                        '    Exit Select
                    Case Else
                        ' Встретился неизвестный науке ключ...
                        Throw New Exception($"Неправильный ключ '{sKey}'")
                        ' если бы не было throw, пришлось бы вставить
                        ' сюда оператор break или return. Иначе компилятор будет
                        ' сильно ругаться... И это верно! Вдруг мы решим перенести код 
                        ' обработчика «default» выше?!
                End Select
            Next

            ' Выдаем сообщение об ошибке, если задана опция R, но не задано 
            'If bRun AndAlso arysConfigs Is Nothing Then
            '    Throw New Exception("Если программа запущена с -r опцией " & "должно быть указано номер изделия.")
            'End If
        Catch ex As Exception
            ' обрабатываем все необработанные исключения.
            MessageBox.Show(ex.ToString, NameOf(Registration), MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    ''' <summary>
    ''' Конфигурация путей
    ''' </summary>
    Private Sub ConfigPath()
        Const caption As String = "Конфигурация путей при запуске приложения"

        PathChannels = Path.Combine(PathResourses, "Channels.mdb")
        PathChannels_cfg_lmz = Path.Combine(PathResourses, "Channels_cfg_lmz.mdb")
        PathProtocol = Path.Combine(PathResourses, "Печать снимка.xlsx")
        PathReportTaring = Path.Combine(PathResourses, "Протокол.xlsx")
        PathHelps = Path.Combine(PathResourses, "Справка\default.htm") ' |        & "query?pg=q?what=web&fmt=.&q="
        PathOptions = Path.Combine(PathResourses, "Опции.ini")
        PathXmlFileCondition = Path.Combine(PathResourses, "Условия.xml")
        PathXmlFileGraphByParameters = Path.Combine(PathResourses, "ГрафикиОтПараметров.xml")
        PathTempFileSoap = Path.Combine(PathResourses, SOAP_FORMATTER_CONTROL_XML)
        PathPanelMotorist = Path.Combine(PathResourses, "ПанелиМоториста")
        PathSolveModule = Path.Combine(PathResourses, "МодулиРасчета")
        PathModuleSolveKT = Path.Combine(PathResourses, "МодулиСбораКТ")

        Dim text As String
        ' проверка файлов в каталоге
        If FileNotExists(PathChannels) Then
            text = $"В каталоге нет файла <{PathChannels}>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End'        'Global.System.Windows.Forms.Application.Exit()
        End If

        If FileNotExists(PathProtocol) Then
            text = $"В каталоге нет файла <{PathProtocol}>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        If FileNotExists(PathOptions) Then
            text = $"В каталоге нет файла <{PathOptions}>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        If FileNotExists(PathReportTaring) Then
            text = $"В каталоге нет файла <{PathReportTaring}!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        ' проверка существования папки, если нет - то создание создание
        If Not Directory.Exists(PathPanelMotorist) Then Directory.CreateDirectory(PathPanelMotorist)
        If Not Directory.Exists(PathSolveModule) Then PathSolveModule = Nothing
        If Not Directory.Exists(PathModuleSolveKT) Then PathModuleSolveKT = Nothing
        If Not Directory.Exists(Path.Combine(PathResourses, "Temp")) Then Directory.CreateDirectory(Path.Combine(PathResourses, "Temp"))
        If Not Directory.Exists(Path.Combine(PathResourses, "Архив")) Then Directory.CreateDirectory(Path.Combine(PathResourses, "Архив"))
        If Not Directory.Exists(Path.Combine(PathResourses, "База снимков")) Then Directory.CreateDirectory(Path.Combine(PathResourses, "База снимков"))
        If Not Directory.Exists(Path.Combine(PathResourses, "Протоколы тарировки")) Then Directory.CreateDirectory(Path.Combine(PathResourses, "Протоколы тарировки"))

        RegistrationEventLog.CreateRegistrationEventLog(PathOptions)
    End Sub

    ''' <summary>
    ''' Загрузка Каналов
    ''' </summary>
    Public Sub LoadChannels()
        Dim rowsCount As Integer
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim aFindValue(0) As Object
        Dim dcDataColumn(1) As DataColumn

        DeleteAddedDigitalInputChannels()
        CheckExisAndAddedColunmsUseCompactRio()

        odaDataAdapter = New OleDbDataAdapter($"SELECT * FROM {ChannelLast} Order By НомерПараметра",
                                              BuildCnnStr(ProviderJet, PathChannels))
        odaDataAdapter.Fill(dtDataTable)
        rowsCount = dtDataTable.Rows.Count

        If rowsCount = 0 Then
            Const caption As String = "Запуск приложения"
            Dim text As String = $"В базе каналов {ChannelLast} нет записей!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        ' на всякий случай заново переименуем номера, сначало заведомо нереальными, затем настоящими
        Dim I As Integer = 10000
        For Each drDataRow In dtDataTable.Rows
            drDataRow.Item("НомерПараметра") = I
            I += 1
        Next

        I = 1
        For Each drDataRow In dtDataTable.Rows
            drDataRow.Item("НомерПараметра") = I
            I += 1
        Next

        If IsCompactRio Then RemarkDataTableUseCompactRio(dtDataTable)

        ' внести изменения в базу данных
        Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.UpdateCommand = myDataRowsCommandBuilder.GetUpdateCommand
        odaDataAdapter.Update(dtDataTable)
        dtDataTable.AcceptChanges()

        If IsCompactRio Then
            ' если CompactRio, то заново считать с учётом подключённых шасси, ранее помеченных в процедуре RemarkDataTableUseCompactRio
            odaDataAdapter = New OleDbDataAdapter($"SELECT * FROM {ChannelLast} WHERE UseCompactRio <> 0 Order By НомерПараметра",
                                                 BuildCnnStr(ProviderJet, PathChannels))
            dtDataTable.Clear()
            odaDataAdapter.Fill(dtDataTable)
            rowsCount = dtDataTable.Rows.Count
        End If

        dcDataColumn(0) = dtDataTable.Columns("НомерПараметра")
        dtDataTable.PrimaryKey = dcDataColumn

        Re.Dim(ParametersType, rowsCount)
        Re.Dim(CoefficientPolynomial2D, rowsCount, 5)
        I = 1

        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        'For I = 1 To rowsCount
        For Each drDataRow In dtDataTable.Rows
            ParametersType(I).Initialize()
            ''aFindValue(0) = I
            ''drDataRow = dtDataTable.Rows.Find(aFindValue)

            'If drDataRow IsNot Nothing Then
            With drDataRow
                ParametersType(I).NumberParameter = CShort(.Item("НомерПараметра"))
                ParametersType(I).NameParameter = CStr(.Item("НаименованиеПараметра"))
                ParametersType(I).NumberChannel = CShort(.Item("НомерКанала"))
                ParametersType(I).NumberDevice = CShort(.Item("НомерУстройства"))

                If Not IsDBNull(.Item("НомерМодуляКорзины")) Then
                    ParametersType(I).NumberModuleChassis = CShort(.Item("НомерМодуляКорзины"))
                Else
                    ParametersType(I).NumberModuleChassis = -1
                End If

                If Not IsDBNull(.Item("НомерКаналаМодуля")) Then
                    ParametersType(I).NumberChannelModule = CShort(.Item("НомерКаналаМодуля"))
                Else
                    ParametersType(I).NumberChannelModule = -1
                End If

                ParametersType(I).TypeConnection = CStr(.Item("ТипПодключения"))
                ParametersType(I).LowerMeasure = CSng(.Item("НижнийПредел"))
                ParametersType(I).UpperMeasure = CSng(.Item("ВерхнийПредел"))
                ParametersType(I).SignalType = CStr(.Item("ТипСигнала"))
                ParametersType(I).NumberFormula = CShort(.Item("НомерФормулы"))
                ParametersType(I).LevelOfApproximation = CShort(.Item("СтепеньАппроксимации"))
                ParametersType(I).Coefficient(0) = CDbl(.Item("A0"))
                ParametersType(I).Coefficient(1) = CDbl(.Item("A1"))
                ParametersType(I).Coefficient(2) = CDbl(.Item("A2"))
                ParametersType(I).Coefficient(3) = CDbl(.Item("A3"))
                ParametersType(I).Coefficient(4) = CDbl(.Item("A4"))
                ParametersType(I).Coefficient(5) = CDbl(.Item("A5"))
                CoefficientPolynomial2D(I, 0) = ParametersType(I).Coefficient(0)
                CoefficientPolynomial2D(I, 1) = ParametersType(I).Coefficient(1)
                CoefficientPolynomial2D(I, 2) = ParametersType(I).Coefficient(2)
                CoefficientPolynomial2D(I, 3) = ParametersType(I).Coefficient(3)
                CoefficientPolynomial2D(I, 4) = ParametersType(I).Coefficient(4)
                CoefficientPolynomial2D(I, 5) = ParametersType(I).Coefficient(5)

                ParametersType(I).Offset = CDbl(.Item("Смещение"))
                ParametersType(I).UnitOfMeasure = CStr(.Item("ЕдиницаИзмерения"))
                ParametersType(I).LowerLimit = CSng(.Item("ДопускМинимум"))
                ParametersType(I).UpperLimit = CSng(.Item("ДопускМаксимум"))
                ParametersType(I).RangeYmin = CShort(.Item("РазносУмин"))
                ParametersType(I).RangeYmax = CShort(.Item("РазносУмакс"))
                ParametersType(I).AlarmValueMin = CSng(.Item("АварийноеЗначениеМин"))
                ParametersType(I).AlarmValueMax = CSng(.Item("АварийноеЗначениеМакс"))
                ParametersType(I).Blocking = CBool(.Item("Блокировка"))
                ParametersType(I).IsVisible = CBool(.Item("ВидимостьРегистратор"))
                ParametersType(I).CompensationXC = CBool(.Item("КомпенсацияХС"))

                If Not IsDBNull(.Item("Погрешность")) Then
                    ParametersType(I).Mistake = CSng(.Item("Погрешность"))
                Else
                    ParametersType(I).Mistake = 0
                End If

                If Not IsDBNull(.Item("Примечания")) Then
                    ParametersType(I).Description = CStr(.Item("Примечания"))
                Else
                    ParametersType(I).Description = CStr(0)
                End If
            End With
            'End If
            I += 1
        Next
    End Sub

    ''' <summary>
    ''' Вычисление физич. значений
    ''' </summary>
    ''' <param name="v"></param>
    ''' <param name="w"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PhysicalValue(ByVal v As Integer, ByVal w As Double) As Double
        ' v- номер параметра
        ' w- значение аргумента - напряжения или кода
        Dim fy As Double
        Dim intx As Short
        Select Case ParametersType(v).NumberFormula
            Case 2
                For intx = ParametersType(v).LevelOfApproximation To 0 Step -1
                    fy = fy * w + ParametersType(v).Coefficient(intx)
                Next intx

                Return fy + ParametersType(v).Offset
                Exit Select
            Case 1
                For intx = ParametersType(v).LevelOfApproximation To 0 Step -1
                    fy = fy * w + ParametersType(v).Coefficient(intx)
                Next intx
                'для расхода по формуле: Qлитр=коэф.из поверки*Fгц=А5*полином(АЦП(модульВ5(Fгц))
                '=arrПараметры(v).sgnКоэффициенты(5)* polin(v, w)
                'полином не может быть больше 4 степени, так как в А5 записывается коэффициент b1 из паспорта ТДР-7 или ТДР-10
                'ФизическиеЗначения = arrПараметры(v).sgnКоэффициенты(0) / w 'было

                Return ParametersType(v).Coefficient(5) * fy + ParametersType(v).Offset
                Exit Select
            Case 3
                If w <= 1 Then Return 0 Else Return 5
                '        Case 0
                '            ФизическиеЗначения = w
                Exit Select
        End Select
    End Function

    ''' <summary>
    ''' Вычисление физических значений.
    ''' Более быстрый метод.
    ''' </summary>
    ''' <param name="w">значение аргумента - напряжения или кода</param>
    ''' <param name="inLevelOfApproximation"></param>
    ''' <param name="Kpolinom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PolynomialEvaluation(ByVal w As Double, ByVal inLevelOfApproximation As Integer, ByVal Kpolinom() As Double) As Double
        Dim fy As Double

        Select Case inLevelOfApproximation
            Case 0
                'fy = Kpolinom(0) 'по правилам, но не наш случай (когда массив полиномов не определён, то СтепеньАппроксимации=0 и надо вернуть просто значение)
                Return w
                Exit Select
            Case 1
                fy = Kpolinom(0) + w * Kpolinom(1)
                Exit Select
            Case 2
                fy = Kpolinom(0) + w * Kpolinom(1) + w * w * Kpolinom(2)
                Exit Select
            Case 3
                fy = Kpolinom(0) + w * Kpolinom(1) + w * w * Kpolinom(2) + w * w * w * Kpolinom(3)
                Exit Select
            Case 4
                fy = Kpolinom(0) + w * Kpolinom(1) + w * w * Kpolinom(2) + w * w * w * Kpolinom(3) + w * w * w * w * Kpolinom(4)
                Exit Select
            Case 5
                fy = Kpolinom(0) + w * Kpolinom(1) + w * w * Kpolinom(2) + w * w * w * Kpolinom(3) + w * w * w * w * Kpolinom(4) + w * w * w * w * w * Kpolinom(5)
                Exit Select
        End Select

        Return fy
    End Function

    ''' <summary>
    ''' True - файла нет
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FileNotExists(ByVal FileName As String) As Boolean
        'FileExists = CBool(Dir(FileName) = vbNullString) 
        Return Not File.Exists(FileName)
    End Function

    ''' <summary>
    ''' Вернуть массив каналов из упакованной строки
    ''' </summary>
    ''' <param name="stringCongiguration"></param>
    Public Function DecryptionString(ByVal stringCongiguration As String) As String()
        Dim count As Integer = 1
        Dim start As Integer = 1
        Dim lenghtString As Integer = Len(stringCongiguration)

        If lenghtString = 0 Then Return New String() {}

        ' вначале холостой проход для определения числа каналов
        Do
            start = InStr(start, stringCongiguration, Separator) + 1
            count += 1
        Loop While start < lenghtString

        ' список параметров в упаковке
        Dim parameterNames(count - 1) As String
        start = 1
        count = 1
        Do
            parameterNames(count) = Mid(stringCongiguration, start, InStr(start, stringCongiguration, Separator) - start)
            start = InStr(start, stringCongiguration, Separator) + 1
            count += 1
        Loop While start < lenghtString

        Return parameterNames
    End Function

    ''' <summary>
    ''' Удаление файла записи испытаний
    ''' </summary>
    ''' <param name="pathFile"></param>
    Public Sub DeleteDataFile(ByRef pathFile As String)
        Try
            Dim infFile As New FileInfo(pathFile)
            If infFile.Exists Then infFile.Delete()
        Catch ex As Exception
            Dim caption As String = "Удаление файла " & pathFile
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End Try
    End Sub

    Public Function FreeBytes(ByVal directoryPath As String) As String
        Dim freeSpace As Long
        Dim isExit As Boolean = False
        ' Проверка наличия свободного места на локальном жёстком диске. В случае, если свободного места на жёстком диске мало, 
        ' то производится удаление последних записей испытаний.
        Dim dirInfo As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(directoryPath)
        Dim drive As DriveInfo = My.Computer.FileSystem.GetDriveInfo(dirInfo.Root.Name)
        'Me.nameLabel.Text = drive.Name
        'Me.rootDirectoryLabel.Text = drive.RootDirectory.Name

        ' MinFreeSpace1 > MinFreeSpace2
        ' Для этих свойств необходима готовность диска.
        If (drive.IsReady) Then
            '    'Me.sizeLabel.Text = CType(drive.TotalSize, String)
            '    'Me.freeSpaceLabel.Text = CType(drive.TotalFreeSpace, String)
            '    'Me.volumeLabelLabel.Text = drive.VolumeLabel

            'Dim d As DriveInfo = New DriveInfo(dirInfo.Root.Name)
            'Console.WriteLine("Drive {0}", d.Name)
            'Console.WriteLine("  File type: {0}", d.DriveType)
            'If d.IsReady = True Then
            '    Console.WriteLine("  Метка дискаl: {0}", d.VolumeLabel)
            '    Console.WriteLine("  Файловая система: {0}", d.DriveFormat)
            '    Console.WriteLine("  Доступное пространство для текущего пользователя: {0, 15} bytes", d.AvailableFreeSpace)
            '    Console.WriteLine("  Всего свободное место:                            {0, 15} bytes", d.TotalFreeSpace)
            '    Console.WriteLine("  Общая ёмкость диска:                              {0, 15} bytes", d.TotalSize)
            'End If

            freeSpace = Convert.ToInt64(CInt(drive.TotalFreeSpace / 1024) \ 1024) ' общий объем свободного места, доступного на диске, в Мбайтах

            If freeSpace < MinFreeSpace Then
                Const strSQL As String = "SELECT TOP 1 БазаСнимков.KeyID, БазаСнимков.Дата, БазаСнимков.ВремяНачалаСбора, БазаСнимков.ПутьНаДиске " &
                                        "From БазаСнимков " &
                                        "ORDER BY БазаСнимков.KeyID"
                Dim cnnChannels As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))

                Try
                    Dim odaDataAdapter As OleDbDataAdapter
                    Const таблица As String = "БазаСнимков"

                    cnnChannels.Open()

                    Do While freeSpace < MinFreeSpace AndAlso isExit = False ' удаление первых записей из база
                        ' делаем проверку на его отсутствие
                        odaDataAdapter = New OleDbDataAdapter(strSQL, cnnChannels)
                        Dim ds As New DataSet
                        odaDataAdapter.Fill(ds, таблица)
                        Dim tlb As DataTable = ds.Tables(0)

                        If tlb.Rows.Count > 0 Then
                            DeleteDataFile(CStr(tlb.Rows(0)("ПутьНаДиске")))
                            tlb.Rows(0).Delete()
                            Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
                            odaDataAdapter.DeleteCommand = myDataRowsCommandBuilder.GetDeleteCommand
                            odaDataAdapter.Update(ds, таблица)
                            ds.Tables(таблица).AcceptChanges()
                            odaDataAdapter.DeleteCommand.Connection.Close()
                        Else
                            isExit = True
                        End If

                        ' заново определим свободное место
                        freeSpace = Convert.ToInt64(CInt(drive.TotalFreeSpace / 1024) \ 1024) ' общий объем свободного места, доступного на диске, в Мбайтах
                    Loop
                Catch ex As Exception
                    Const caption As String = "Удаление первой записи из базы снимов"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                Finally
                    If (cnnChannels.State = ConnectionState.Open) Then
                        cnnChannels.Close()
                    End If
                End Try
            End If
        End If

        Return CStr(freeSpace) & " МБ"
    End Function

    ''' <summary>
    ''' Чтение данных из файла INI - с возможностью записи значения по умолчанию где аргументы:
    ''' </summary>
    ''' <param name="sINIFile"></param>
    ''' <param name="sSection">sSection  = Название раздела</param>
    ''' <param name="sKey">sKey  = Название параметра</param>
    ''' <param name="sDefault">sDefault = Значение по умолчанию (на случай его отсутствия)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIni(ByRef sINIFile As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef sDefault As String = "") As String
        ' Значение возвращаемое функцией GetPrivateProfileString если искомое значение параметра не найдено
        Const NO_VALUE As String = ""
        Dim sLength As Short ' Длина возвращаемой строки (функцией GetPrivateProfileString)
        Dim sTemp As String ' Возвращаемая строка

        Try
            ' Получаем значение из файла - если его нет будет возвращен 4й аргумент = strNoValue
            ' sTemp.Value = Space(256)
            sTemp = New String(Chr(0), 255)
            sLength = CShort(NativeMethods.GetPrivateProfileString(sSection, sKey, NO_VALUE, sTemp, 255, sINIFile))
            sTemp = Left(sTemp, sLength)

            ' Определяем было найдено значение или нет (если возвращено знач. константы strNoValue то = НЕТ)
            If sTemp = NO_VALUE Then ' Значение не было найдено
                If sDefault <> "" Then ' Если значение по умолчанию задано
                    WriteINI(sINIFile, sSection, sKey, sDefault) ' Записываем заданное аргументом sDefault значение по умолчанию
                    sTemp = sDefault ' и возвращаем его же
                End If
            End If

            ' Возвращаем найденное
            Return sTemp
        Catch ex As ApplicationException
            Const caption As String = "Ощибка чтения INI"
            Dim text As String = $"Функция sGetIni привела к ошибке:{vbCrLf}{ex.ToString}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED(String.Format("<{0}> {1}", caption, text))
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Запись данных в INI файл - аргументы:
    ''' </summary>
    ''' <param name="sINIFile"></param>
    ''' <param name="sSection">sSection = Название раздела</param>
    ''' <param name="sKey">sKey = Название параметра</param>
    ''' <param name="sValue">sValue = Значение параметра</param>
    ''' <remarks></remarks>
    Public Sub WriteINI(ByRef sINIFile As String, ByRef sSection As String, ByRef sKey As String, ByRef sValue As String)
        Dim N As Integer
        Dim sTemp As String = sValue

        ' Заменить символы CR/LF на пробелы
        For N = 1 To Len(sValue)
            If Mid(sValue, N, 1) = vbCr OrElse Mid(sValue, N, 1) = vbLf Then Mid(sValue, N) = " "
        Next

        Try
            ' Пишем значения
            N = CShort(NativeMethods.WritePrivateProfileString(sSection, sKey, sTemp, sINIFile))
            ' Проверка результата записи
            If N <> 1 Then ' Неудачное завершение
                MsgBox("Процедура WriteINI не смогла записать параметр INI Файла:" & vbCrLf & sINIFile & vbCrLf &
                       "-----------------------------------------------------------------" & vbCrLf &
                       "[" & sSection & "]" & vbCrLf &
                       sKey & "=" & sValue)
            End If
        Catch ex As ApplicationException
            Const caption As String = "Ощибка чтения INI"
            Dim text As String = $"Процедура WriteINI привела к ошибке:{vbCrLf}#{ex.ToString}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    ''' <summary>
    ''' Проверка Для Копирования Базы
    ''' </summary>
    ''' <param name="whoIsCall"></param>
    Public Sub CheckForCopyingBase(ByVal whoIsCall As TypeWorkAutomaticBackup)
        'Dim isTest As Boolean = True
        Dim difference, freeSpace, sizeVolumeDBase As Double
        Dim pathArchive As String
        Dim folderCopying As String ' папка Копирования
        Dim folderDBaseSnapshot As String ' папка База Cнимков
        Dim newPathChannels As String 'новый Путь Channels
        Dim indexIn As Integer = PathChannels.IndexOf("\\")
        Dim isFolderStory, isFolderOscill As Boolean ' Каталог Ресурсы или Каталог Осциллографирование
        Const мaxSizeBase As Integer = 5000 ' 5 гигабайт

        ' если обращение по сети, то проверку не делать, так как неизвестно, что там происходит
        If indexIn <> -1 Then Exit Sub ' другой компьютер

        ' проверка родительского каталога на имя "ресурсы" нужен только каталог "ресурсы" для определения, что попытка просмотра архивов
        indexIn = PathChannels.IndexOf(RootDirectory) '"Ресурсы"

        If indexIn <> -1 Then isFolderStory = True
        ' проверка родительского каталога на имя "Осциллографирование" нужен только каталог "Осциллографирование"
        indexIn = PathChannels.IndexOf("Осциллографирование")

        If indexIn <> -1 Then isFolderOscill = True

        If isFolderOscill = False AndAlso isFolderStory = False Then Exit Sub

        ' для режима регистратора при запуске
        ' для режима снимок только когда просмотр без записи (чтобы эта процедура не завелась во время испытаний)
        ' и только во время открытия проводника, потому что база может буть сменена
        If whoIsCall = TypeWorkAutomaticBackup.Snapshot AndAlso (IsWorkWithDaqController OrElse IsCompactRio OrElse IsTcpClient) Then
            Exit Sub ' просмотр архива
        End If

        ' проверки логической переменной присвоить проверено
        If IsCheckFreePlace = False Then
            ' находится свободное пространство на диске текущей базы
            freeSpace = FreeSpaceDisk(PathChannels)
            ' находится объем папки "База снимков" и самой база Channels Uбазы
            folderDBaseSnapshot = Path.Combine(Path.GetDirectoryName(PathChannels), "База снимков")
            sizeVolumeDBase = CalculationSizeFolder(folderDBaseSnapshot)
            sizeVolumeDBase += CalculationSizeFile(PathChannels)
            difference = freeSpace - sizeVolumeDBase ' МБ

            If difference < 100 OrElse sizeVolumeDBase > мaxSizeBase Then  'OrElse isTest Then ' перенос Свободного места достаточно для проведения архивирования
                If freeSpace > sizeVolumeDBase Then ' OrElse isTest Then
                    ' все нормально
                    ' сообщение о проводимой процедуре
                    '****************************
                    Dim messageBoxForm As New FormMessageBox("Подождите, идет архивирование", "Автоматическое архивирование")
                    messageBoxForm.Show()
                    messageBoxForm.Activate()
                    messageBoxForm.Refresh()

                    ' проверка существования папаки "Архивы" 
                    pathArchive = Path.Combine(Directory.GetDirectoryRoot(PathChannels), "Архивы")
                    If Not Directory.Exists(pathArchive) Then ' если нет то создание создание в папке "Архивы" новой папки
                        'MessageBox.Show("В корне нет папки Архивы!", "Проверка свободного места", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Directory.CreateDirectory(pathArchive)
                    End If

                    'создание папки с датой и временем
                    folderCopying = Path.Combine(pathArchive, $"Автоматическое архивирование {Today.ToShortDateString} ({Now.Hour}ч{Now.Minute}м{Now.Second}с)")
                    Directory.CreateDirectory(folderCopying) ' создать папку перед переносом

                    ' 1 перенос всей папки "База снимков" и копирование базы 
                    Try
                        Directory.Move(folderDBaseSnapshot, Path.Combine(folderCopying, "База снимков"))
                        newPathChannels = Path.Combine(folderCopying, "Channels.mdb")
                        File.Copy(PathChannels, newPathChannels)
                        ' создание папки "База снимков" в каталоге ресурсы, которая была перемещена
                        Directory.CreateDirectory(folderDBaseSnapshot)
                        ' удаление(очистка) и сжатие базы
                        DeleteAllRecordAfterCoping()
                        ' сжатие баз источника и приемника
                        ' заменил работу JRO в отдельном исполняемом файле
                        ' было
                        'ВосстанавлениеБазыПослеКопирования()
                        CompressionBaseAfterCopying(PathChannels)

                        ' присвоение нового пути к базе только если просмотр из другой программы
                        ' которая произвела архивирование
                        ' или из текущей загруженной как снимок
                        If whoIsCall = TypeWorkAutomaticBackup.Snapshot AndAlso Not (IsWorkWithDaqController OrElse IsCompactRio) Then
                            PathChannels = newPathChannels
                            IsDataBaseChanged = True
                        End If
                    Catch ex As Exception
                        Const caption As String = "Перенос папки или базы"
                        Dim text As String = ex.ToString
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
                        IsCheckFreePlace = True
                    Finally
                        messageBoxForm.Close()
                    End Try
                Else
                    ' сообщение о нехватке места и необходимости ручного переноса
                    Const caption As String = "Автоматическое архивирование базы"
                    Dim text As String = $"Свободного места на диске недостаточно для автоматического архивирования.{vbCrLf}Произведите архивирование вручную."
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                End If
            End If

            IsCheckFreePlace = True
        End If
    End Sub

    ''' <summary>
    ''' Восстанавливает базу данных и выполняет ее сжатие.
    ''' </summary>
    ''' <param name="pathDataBase"></param>
    ''' <returns></returns>
    Public Function CompressionBaseAfterCopying(pathDataBase As String) As BackupingResult
        Dim myBackupingResult As BackupingResult = BackupingResult.TimeOut ' по умолчанию

        Try
            Dim moreToDo As Boolean = True
            Dim sw = New Stopwatch
            Dim waitTimeSec As Integer = 20 ' время Ожидания сек
            Dim startInfo As New ProcessStartInfo(Path.Combine(PathResourses, "BackupJRO.exe")) With {
                .WindowStyle = ProcessWindowStyle.Minimized, 'Normal 
                .WorkingDirectory = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), RootDirectory)
            }
            'startInfo.UseShellExecute = True

            ' сжать базу 
            WriteINI(PathOptions, "BackupJRO", "PathTarget", pathDataBase)
            sw.Start()
            Process.Start(startInfo) ' выставление флага BackupingIsCompleted=False

            While moreToDo
                If sw.Elapsed.Seconds > waitTimeSec Then
                    moreToDo = False
                    myBackupingResult = BackupingResult.TimeOut
                ElseIf CBool(GetIni(PathOptions, "BackupJRO", "BackupingIsCompleted", "False")) Then
                    moreToDo = False
                    myBackupingResult = BackupingResult.BackupingIsSuccess
                End If

                If moreToDo Then
                    Thread.Sleep(500)
                Else
                    sw.Stop()
                End If
            End While

            If myBackupingResult = BackupingResult.TimeOut Then
                Const caption As String = "Ошибка сжатия базы"
                Dim text As String = $"При сжатии базы {pathDataBase} превышено время ожидания"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If

        Catch ex As Exception
            Const captionEx As String = "Ошибка запуска BackupJRO.exe"
            Dim textEx As String = $"Функция Process.Start привела к ошибке:{Environment.NewLine}{ex}"
            'Console.WriteLine(String.Format("{0}{1}{2}", captionEx, Environment.NewLine, textEx))
            'Console.ReadKey()
            MessageBox.Show(textEx, captionEx, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{captionEx}> {textEx}")
        End Try

        Return myBackupingResult
    End Function

    '''' <summary>
    '''' Восстанавливает базу данных и выполняет ее сжатие.
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub ВосстанавлениеБазыПослеКопирования()
    '    Dim strBackup As String = PathResourses & "\Temp\Copy.mdb"
    '    Dim je As New JRO.JetEngine
    '    Dim strFilename As String = PathChannels ' где будет копия 

    '    '--- Сделать рабочую копию базы данных --------------------------------
    '    Try
    '        File.Copy(strFilename, strBackup, True)
    '    Catch ex As Exception
    '        Const caption As String = "Ошибка копирования базы данных при сжатии"
    '        Dim text As String = ex.ToString
    '        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
    '    End Try

    '    Application.DoEvents()

    '    Try
    '        File.Delete(strFilename)
    '        Thread.Sleep(200)
    '        '--- Восстановленные базы данных должны быть сжаты. ---------------
    '        je.CompactDatabase(ProviderJet & "Data Source=" & strBackup & ";", ProviderJet & "Data Source=" & strFilename & ";Jet OLEDB:Encrypt Database=True")
    '        '--- В случае успеха удалить резервную копию. ---------------------
    '        File.Delete(strBackup)
    '        Application.DoEvents()
    '    Catch ex As Exception
    '        Const caption As String = "Ошибка восстановления базы данных при сжатии"
    '        Dim text As String = ex.ToString
    '        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
    '        ПроверкаСвободногоМестаПроведена = True
    '    Finally
    '        Beep()
    '    End Try
    'End Sub

    ''' <summary>
    ''' Удалить все записи после копирования
    ''' </summary>
    Private Sub DeleteAllRecordAfterCoping()
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Using cmd As OleDbCommand = cn.CreateCommand
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE [БазаСнимков].* FROM [БазаСнимков]"
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Const caption As String = "Ошибка удаления записей в базе после копирования"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                    IsCheckFreePlace = True
                End Try
            End Using
        End Using
        Thread.Sleep(200)
    End Sub

#Region "Свободное Место На Диске"
    ''' <summary>
    ''' Накопление размера папки
    ''' </summary>
    ''' <param name="fileTemp"></param>
    ''' <param name="allLength"></param>
    Private Sub AccumulationSizeFolder(ByVal fileTemp As FileSystemInfo, ByRef allLength As Long)
        If fileTemp.GetType Is GetType(FileInfo) Then
            allLength += CType(fileTemp, FileInfo).Length()
        Else
            Dim dirTemp As New DirectoryInfo(fileTemp.FullName)
            Dim fsiNew As FileSystemInfo

            For Each fsiNew In dirTemp.GetFileSystemInfos
                AccumulationSizeFolder(fsiNew, allLength)
            Next
        End If
    End Sub

    Private Function CalculationSizeFolder(ByVal pathFolder As String) As Double
        ' возвращает размер всех вложенных папок в мегабайтах
        Dim размер As Long
        Dim dirTemp As New DirectoryInfo(pathFolder)
        AccumulationSizeFolder(dirTemp, размер)
        Return размер / 1048576
    End Function

    ''' <summary>
    ''' Подсчет Размера Файла
    ''' </summary>
    ''' <param name="pathFile"></param>
    ''' <returns></returns>
    Private Function CalculationSizeFile(ByVal pathFile As String) As Double
        ' возвращает размер файла в мегабайтах
        Dim infFile As New FileInfo(pathFile)
        Return infFile.Length / 1048576
    End Function

    ''' <summary>
    ''' Свободное Место На Диске
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <returns></returns>
    Private Function FreeSpaceDisk(ByVal sPath As String) As Double
        Dim freeSpace As Long
        ' Проверка наличия свободного места на локальном жёстком диске. В случае если свободного места на жёстком диске мало, 
        ' то производится установка флагов, запрещающих потоку записи данных на жёсткий диск производить запись.
        Dim dirInfo As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(sPath)
        Dim drive As DriveInfo = My.Computer.FileSystem.GetDriveInfo(dirInfo.Root.Name)
        'Me.nameLabel.Text = drive.Name
        'Me.rootDirectoryLabel.Text = drive.RootDirectory.Name

        ' Для этих свойств необходима готовность диска.
        If (drive.IsReady) Then
            '    'Me.sizeLabel.Text = CType(drive.TotalSize, String)
            '    'Me.freeSpaceLabel.Text = CType(drive.TotalFreeSpace, String)
            '    'Me.volumeLabelLabel.Text = drive.VolumeLabel

            'Dim d As DriveInfo = New DriveInfo(dirInfo.Root.Name)
            'Console.WriteLine("Drive {0}", d.Name)
            'Console.WriteLine("  File type: {0}", d.DriveType)
            'If d.IsReady = True Then
            '    Console.WriteLine("  Метка дискаl: {0}", d.VolumeLabel)
            '    Console.WriteLine("  Файловая система: {0}", d.DriveFormat)
            '    Console.WriteLine("  Доступное пространство для текущего пользователя: {0, 15} bytes", d.AvailableFreeSpace)
            '    Console.WriteLine("  Всего свободное место:                            {0, 15} bytes", d.TotalFreeSpace)
            '    Console.WriteLine("  Общая ёмкость диска:                              {0, 15} bytes", d.TotalSize)
            'End If

            freeSpace = Convert.ToInt64(CInt(drive.TotalFreeSpace / 1024) \ 1024) ' общий объем свободного места, доступного на диске, в Мбайтах
        End If

        Return freeSpace
    End Function
#End Region

    ''' <summary>
    ''' График
    ''' </summary>
    ''' <param name="knownX"></param>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <returns></returns>
    Public Function LinearInterpolation(ByVal knownX As Double, ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Double
        If X2 = X1 Then ' dblX2 - dblX1 = 0
            Return Y1
        Else
            Return Y1 + (Y2 - Y1) * (knownX - X1) / (X2 - X1)
        End If
    End Function

    ''' <summary>
    ''' Выдать Правильный Путь
    ''' </summary>
    ''' <param name="pathRecord"></param>
    ''' <returns></returns>
    Public Function ProduceRightPath(ByVal pathRecord As String) As String
        Dim tempPath As String = pathRecord
        Dim rootPath As String
        Dim indexFolder As Integer = tempPath.IndexOf("База снимков")
        Dim lastIndexSeparator As Integer = PathChannels.LastIndexOf(Separator)

        If indexFolder > 0 Then tempPath = tempPath.Substring(0, indexFolder)
        If lastIndexSeparator > 0 Then
            rootPath = PathChannels.Substring(0, lastIndexSeparator + 1)
        Else
            rootPath = Nothing
        End If

        If tempPath <> rootPath Then
            tempPath = pathRecord
            ' переименовать путь к текстовым данным
            lastIndexSeparator = tempPath.LastIndexOf(Separator)
            Return $"{rootPath}База снимков\{tempPath.Substring(lastIndexSeparator + 1, tempPath.Length - lastIndexSeparator - 1)}"
        Else
            Return pathRecord
        End If
    End Function

#Region "Dbase & Table"
    Public Function BuildCnnStr(ByVal provider As String, ByVal dataBase As String) As String
        'Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=1;Data Source="D:\ПрограммыVBNET\RUD\RUD.NET\bin\Ресурсы\Channels.mdb";Jet OLEDB:Engine Type=5;Provider="Microsoft.Jet.OLEDB.4.0";Jet OLEDB:System database=;Jet OLEDB:SFP=False;persist security info=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;User ID=Admin;Jet OLEDB:Global Bulk Transactions=1
        'sConnect = strProviderJet & "Persist Security Info=False;User ID=Admin;Data Source=" & strПутьChannels & ";Mode=Share Deny None;Extended Properties=';COUNTRY=0;CP=1252;LANGID=0x0409';Locale Identifier=1033;Jet OLEDB:System database='';Jet OLEDB:Registry Path='';Jet OLEDB:Database Password='';Jet OLEDB:Global Partial Bulk Ops=2"
        Return $"{provider}Data Source={dataBase};"
    End Function

    ''' <summary>
    ''' Удалить каналы в которых погрешность является признаком
    ''' дискретных каналов с подключенными модулями.
    ''' </summary>
    Private Sub DeleteAddedDigitalInputChannels()
        If (MainMdiForm.ModuleSolveManager Is Nothing OrElse IsFrmServiceBasesLoaded) AndAlso IsWorkWithDaqController Then
            ' если MainMdiForm.ModuleSolveManager Is Nothing значит вызвана из шапки и полная проверка
            ' если IsFrmServiceBasesLoaded значит вызвана оттуда в событии закрытия
            Using cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
                cn.Open()
                ' проверить наличие таблицы с именем ChannelDigitalInput
                If CheckExistTable(cn, "ChannelDigitalInput") Then
                    If gDigitalInput Is Nothing Then
                        gDigitalInput = New DigitalInput
                        gDigitalInput.LoadParameters()
                    End If
                Else
                    Dim cmd As OleDbCommand = cn.CreateCommand
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = $"DELETE * FROM {ChannelLast} WHERE Погрешность={IndexDiscreteInput}"
                    cmd.ExecuteNonQuery()
                End If
            End Using
            Thread.Sleep(500)
            Application.DoEvents()
        End If
    End Sub

    ''' <summary>
    ''' Пометить в dtDataTable записи, чьи Chassis входят в коллекцию шасси подключённых к сбору.
    ''' </summary>
    ''' <param name="refDataTable"></param>
    Private Sub RemarkDataTableUseCompactRio(ByRef refDataTable As DataTable)
        ' если используется режим сбора с шасси CompactRio, то пройти по шасси подключённым для сбора и удалить из dtDataTable
        ' записи, чьи Chassis не входят в коллекцию шасси подключённых к сбору. Потребуется расшифровка конфигурации <СписокШассиCRio.xml>
        Dim xmlFile As String = Path.Combine(PathResourses, CRIOChassisList & ".xml")
        If Not File.Exists(xmlFile) Then
            ' Файла нет, значит нет и файла сериализации, надо создать,
            ' для примера коллекция из одного элемента экземпляра TargetCRIO.
            Dim ArrayListObject As New ArrayList From {New TargetCRIO("New Target", New IPAddressTargetCRIO("192.168.1.1"), EnumModeWork.Measurement, "Папка не указана")}
            SerializeHashtableToXML(xmlFile, ArrayListObject)
        End If

        Dim namesTargetCRIO As New List(Of String)
        ' извлекаем из Hashtable элементы (они экземпляры классов свойств TargetCRIO уже известного типа - приводит тип не надо)
        For Each de As DictionaryEntry In DeserializeHashtableFromXML(xmlFile)
            namesTargetCRIO.Add(CType(de.Value, TargetCRIO).HostName)
        Next

        'Dim dataRowsToDelete = From itemDataRow As DataRow In refDataTable
        '                       Where Not namesTargetCRIO.Contains(CStr(IIf(IsDBNull(itemDataRow(Chassis)), " ", itemDataRow(Chassis))))
        '                       Select itemDataRow

        'If dataRowsToDelete.Count > 0 Then
        '    'For Each itemDataRow As DataRow In dataRowsToDelete
        '    '    refDataTable.Rows.Remove(itemDataRow)
        '    'Next

        '    Debug.Print("Всего: " & dataRowsToDelete.Count.ToString)

        '    For I As Integer = refDataTable.Rows.Count - 1 To 0 Step -1
        '        If dataRowsToDelete.Contains(refDataTable.Rows(I)) Then
        '            refDataTable.Rows.RemoveAt(I)
        '            Debug.Print(I.ToString)
        '        End If
        '    Next
        'End If
        'refDataTable.AcceptChanges()

        ' выбрать записи, чьи поля <Chassis> таблицы каналов имеются в списке сконфигурированных шасси или записи параметров расчётных параметров
        Dim dataRowsToSelect = From itemDataRow As DataRow In refDataTable
                               Where namesTargetCRIO.Contains(CStr(IIf(IsDBNull(itemDataRow(Chassis)), " ", itemDataRow(Chassis)))) OrElse CSng(IIf(IsDBNull(itemDataRow("Погрешность")), 0, itemDataRow("Погрешность"))) = indexCalculated
                               Select itemDataRow
        If dataRowsToSelect.Count > 0 Then
            'Debug.Print("Всего: " & dataRowsToSelect.Count.ToString)
            For I As Integer = 0 To refDataTable.Rows.Count - 1
                refDataTable.Rows(I).Item(UseCompactRio) = False

                If dataRowsToSelect.Contains(refDataTable.Rows(I)) Then
                    refDataTable.Rows(I).Item(UseCompactRio) = True
                    'Debug.Print(I.ToString)
                End If
            Next
        End If

        If IsCheckEmptyChassis Then Exit Sub
        ' обратный фильтр
        Dim dataRowsMissingTargetCRIO = From itemDataRow As DataRow In refDataTable
                                        Where Not (namesTargetCRIO.Contains(CStr(IIf(IsDBNull(itemDataRow(Chassis)), " ", itemDataRow(Chassis)))) OrElse CSng(IIf(IsDBNull(itemDataRow("Погрешность")), 0, itemDataRow("Погрешность"))) = indexCalculated)
                                        Select itemDataRow
        Dim nameMissing As String
        Dim namesMissTargetCRIO As New List(Of String)
        For Each itemMissingDataRow As DataRow In dataRowsMissingTargetCRIO
            Dim nameChassis As String = CStr(IIf(IsDBNull(itemMissingDataRow(Chassis)), " ", itemMissingDataRow(Chassis)))
            If Not namesMissTargetCRIO.Contains(nameChassis) Then
                namesMissTargetCRIO.Add(nameChassis)
                nameMissing += $"<{nameChassis}> "
            End If
        Next

        If namesMissTargetCRIO.Count > 0 Then
            Const caption As String = NameOf(RemarkDataTableUseCompactRio)
            Dim text As String = $"В базе данных рабочего стенда для параметров назначены шасси с именами:{Environment.NewLine}" &
                $"{nameMissing}{Environment.NewLine}" &
                $"которые отсутствуют в конфигурационном файле: {xmlFile}{Environment.NewLine}" &
                $"Проверьте настройки каналов или настройки шасси.{Environment.NewLine}" &
                $"Параметры для этих шасси будут отсутствовать при регистрации!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        ' найти шасси, находящиеся в конфигурации, но отсутствующие в поле <Chassis> таблицы каналов. Будут ошибки регистрации.
        namesMissTargetCRIO = New List(Of String)
        nameMissing = String.Empty
        Dim success As Boolean

        For Each itemTargetCRIO As String In namesTargetCRIO
            success = False
            For I As Integer = 0 To refDataTable.Rows.Count - 1
                Dim itemDataRow As DataRow = refDataTable.Rows(I)
                If CSng(IIf(IsDBNull(itemDataRow("Погрешность")), 0, itemDataRow("Погрешность"))) = indexCalculated Then Continue For

                Dim rowNameChassis As String = CStr(IIf(IsDBNull(refDataTable.Rows(I).Item(Chassis)), "DBNull", refDataTable.Rows(I).Item(Chassis)))
                If rowNameChassis = "DBNull" Then Continue For

                If itemTargetCRIO = rowNameChassis Then
                    success = True ' присутствует в записях таблицы каналов
                    Exit For
                End If
            Next

            If Not success AndAlso Not namesMissTargetCRIO.Contains(itemTargetCRIO) Then
                namesMissTargetCRIO.Add(itemTargetCRIO)
                nameMissing += $"<{itemTargetCRIO}> "
            End If
        Next

        If namesMissTargetCRIO.Count > 0 Then
            Const caption As String = NameOf(RemarkDataTableUseCompactRio)
            Dim text As String = $"В конфигурационном файле: {xmlFile}{Environment.NewLine}" &
                $"находятся шасси с именами:{Environment.NewLine}" &
                $"{nameMissing}{Environment.NewLine}" &
                $"но эти шасси не назначены ни в одном поле <{Chassis}> таблицы каналов.{Environment.NewLine}" &
                $"Проверьте настройки каналов или настройки шасси.{Environment.NewLine}" &
                $"Запуск будет произведён с ошибкой!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        IsCheckEmptyChassis = True
    End Sub

    ''' <summary>
    ''' Проверка существования полей необходимых для работы с шасси CompactRio 
    ''' в таблице каналов выбранного стенда и добавление их в случае необходимости.
    ''' </summary>
    Private Sub CheckExisAndAddedColunmsUseCompactRio()
        ' проверить наличие таблицы с именем ChannelDigitalInput
        Using cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            CheckExisColunms(cn, ChannelLast)
            CheckExisColunms(cn, CHANNEL_N)
        End Using
        Thread.Sleep(500)
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Проверка наличия Колонок в Таблице.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="tableName"></param>
    Private Sub CheckExisColunms(ByRef cn As OleDbConnection, tableName As String)
        If Not CheckExisColunm(cn, tableName, Chassis) Then CreateTextColunmToDbase(cn, tableName, Chassis)
        If Not CheckExisColunm(cn, tableName, BoardType) Then CreateTextColunmToDbase(cn, tableName, BoardType)
        If Not CheckExisColunm(cn, tableName, UseCompactRio) Then CreateBitColunmToDbase(cn, tableName, UseCompactRio)
    End Sub

    ''' <summary>
    ''' Проверить есть, ли таблица режимов для данного стенда
    ''' </summary>
    Public Sub CheckTableNameRegime()
        Dim conn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim tableNameRegime As String = "Режимы" & StandNumber
        Dim isTableNameRegime As Boolean

        conn.Open()
        Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        conn.Close()

        For Each row As DataRow In schemaTable.Rows
            If CBool(InStr(1, row("TABLE_NAME").ToString, tableNameRegime)) Then isTableNameRegime = True
        Next

        If Not isTableNameRegime Then
            Dim caption As String = $"Отсутствует таблица с именем <{tableNameRegime}>"
            Dim text As String = $"В базе данных <{PathChannels}> отсутствует таблица с именем <{tableNameRegime}>.{vbCrLf}Продолжение работы невозможно!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
            'Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Возвращает сведения схемы из источника данных.
    ''' Проверка наличия Таблицы.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="tableName"></param>
    ''' <returns></returns>
    Public Function CheckExistTable(ByRef cn As OleDbConnection, ByVal tableName As String) As Boolean
        For Each row As DataRow In cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing).Rows
            If row("TABLE_NAME").ToString = tableName Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Возвращает сведения схемы из источника данных.
    ''' Проверка наличия Колонки в Таблице.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="tableName"></param>
    ''' <param name="colunmName"></param>
    ''' <returns></returns>
    Public Function CheckExisColunm(ByRef cn As OleDbConnection, tableName As String, colunmName As String) As Boolean
        For Each row As DataRow In cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, Nothing).Rows
            If row("TABLE_NAME").ToString = tableName AndAlso row("COLUMN_NAME").ToString = colunmName Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    '''  Изменения макета таблицы после того, как она была создана.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="tableName"></param>
    ''' <param name="colunmName"></param>
    Private Sub CreateTextColunmToDbase(cn As OleDbConnection, tableName As String, colunmName As String)
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {colunmName} TEXT(32)"
        cmd.ExecuteNonQuery()
    End Sub

    ''' <summary>
    '''  Изменения макета таблицы после того, как она была создана.
    ''' </summary>
    ''' <param name="cn"></param>
    ''' <param name="tableName"></param>
    ''' <param name="colunmName"></param>
    Private Sub CreateBitColunmToDbase(cn As OleDbConnection, tableName As String, colunmName As String)
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {colunmName} BIT"
        cmd.ExecuteNonQuery()
    End Sub
#End Region

    ''' <summary>
    ''' Архивация При Открытии Окна
    ''' </summary>
    Public Async Sub ArchivingByOpeningWindow()
        Dim mFormSelectSnapshotDBase As New FormSelectSnapshotDBase
        mFormSelectSnapshotDBase.PreparationForCallFromServiceBase()
        mFormSelectSnapshotDBase.Show()

        Dim mfrmMessageBox As FormMessageBox = New FormMessageBox("Производится автоматический разбор записей в архив", "Архивирование")
        mfrmMessageBox.Show()
        mfrmMessageBox.Activate()
        mfrmMessageBox.Refresh()
        Await mFormSelectSnapshotDBase.AutomaticArchives()
        mFormSelectSnapshotDBase.Close()
        mFormSelectSnapshotDBase = Nothing
        mfrmMessageBox.Close()
    End Sub

    ''' <summary>
    ''' Проверка На Измерительные Каналы
    ''' </summary>
    ''' <param name="indexChannel"></param>
    ''' <returns></returns>
    Public Function CheckMeasurementChannels(ByVal indexChannel As Integer) As Boolean
        If ParametersType.Length <= indexChannel Then Return False
        If ParametersType(indexChannel).Mistake <> indexCalculated AndAlso ParametersType(indexChannel).Mistake <> IndexDiscreteInput Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CheckExistServerCfglmzXml(ByVal file As String) As Boolean
        If FileNotExists(file) Then
            Const caption As String = "Проверка наличия файла"
            Dim text As String = $"По указанному пути нет файла {file} !{vbCrLf}Проверьте сетевое окружение."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Проверка: Цифра -> True
    ''' </summary>
    ''' <param name="mObject"></param>
    ''' <param name="text"></param>
    ''' <returns></returns>
    Public Function IsDigitCheck(ByVal mObject As String, ByVal text As String) As Boolean
        Dim success As Boolean = True

        Try
            Dim singleValue As Single = CSng(text)
        Catch ex As Exception
            Dim msg As String = $"В поле {mObject} введена не цифра!"
            Const title As String = "Ввод новых данных"
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{title}> {msg}")
            success = False
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Перерисовка иконки выпадающего списка в соответствии с физической размерностью канала 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="die"></param>
    ''' <param name="indexAbsentParameters">индексы Ненайденных Параметров</param>
    ''' <param name="inImageList"></param>
    ''' <param name="isFormMain"></param>
    Public Sub DrawItemComboBox(ByVal sender As Object,
                                ByVal die As DrawItemEventArgs,
                                indexAbsentParameters() As Integer,
                                inImageList As ImageList,
                                isFormMain As Boolean)

        If die.Index = -1 Then Return
        If sender Is Nothing Then Return

        Dim cmb As ComboBox = CType(sender, ComboBox)
        'Dim cmb As ListControl = CType(sender, ListControl) 'ListControl

        Dim g As Graphics = die.Graphics

        ' если элемент выделен, то отрисовать его цветом
        die.DrawBackground()
        die.DrawFocusRectangle()
        Dim rectPreviewBox As Rectangle = die.Bounds

        ' отрисовать цветной квадрат выделения
        ' при данном шрифте Font("Arial", 22) высота rectPreviewBox.Height=36
        ' ImageList2 размером 32, значит надо квадрат 32*32
        rectPreviewBox.Offset(2, 2)
        rectPreviewBox.Height -= 4
        rectPreviewBox.Width = rectPreviewBox.Height 'rectPreviewBox.Height - 4 '32 '20
        g.DrawRectangle(New Pen(die.ForeColor), rectPreviewBox)

        'g.DrawImage(ImageList2.Images(0), rectPreviewBox)

        ' получить подходящую кисть для выделения квадрата
        rectPreviewBox.Offset(1, 1)
        rectPreviewBox.Width -= 2
        rectPreviewBox.Height -= 2
        'g.FillRectangle(selectedBrush, rectPreviewBox)
        'g.DrawImage(ImageList2.Images(die.Index), rectPreviewBox)

        Dim drawFont As Font

        If indexAbsentParameters Is Nothing Then
            If isFormMain Then
                ' если элемент из основного окна приложения
                g.DrawImage(inImageList.Images(die.Index), rectPreviewBox)
            Else
                g.DrawImage(inImageList.Images(CType(cmb.Items(die.Index), StringIntObject).i), rectPreviewBox)
            End If
            drawFont = New Font(cmb.Font, cmb.Font.Style Or FontStyle.Bold)
        Else
            If Array.IndexOf(indexAbsentParameters, die.Index) = -1 Then ' не найден, значит иконка по настройке
                g.DrawImage(inImageList.Images(CType(cmb.Items(die.Index), StringIntObject).i), rectPreviewBox)
            Else ' индекс найден, иконка с крестиком
                g.DrawImage(inImageList.Images(12), rectPreviewBox)
            End If
            ' выделить имя цветом
            'Dim drawFont As New Font("Arial", 16, FontStyle.Bold)
            drawFont = New Font(cmb.Font, cmb.Font.Style) ' Or FontStyle.Bold)
        End If

        'g.DrawString(cmb.Items(die.Index).ToString, New Font("Arial", 12, FontStyle.Bold), New SolidBrush(die.ForeColor), die.Bounds.X + 30, die.Bounds.Y + 1)
        g.DrawString(cmb.Items(die.Index).ToString, drawFont, New SolidBrush(die.ForeColor), die.Bounds.X + 30, die.Bounds.Y + 1)
    End Sub

    ''' <summary>
    '''  Перерисовка иконки списка в соответствии с физической размерностью канала
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="die"></param>
    ''' <param name="inImageList"></param>
    Public Sub DrawItemListBox(ByVal sender As Object, ByVal die As DrawItemEventArgs, inImageList As ImageList)
        If die.Index = -1 Then Return
        If sender Is Nothing Then Return

        Dim cmb As ListBox = CType(sender, ListBox)
        Dim g As Graphics = die.Graphics

        ' если элемент выделен, то отрисовать его цветом
        die.DrawBackground()
        die.DrawFocusRectangle()
        Dim rectPreviewBox As Rectangle = die.Bounds

        ' отрисовать цветной квадрат выделения
        ' при данном шрифте Font("Arial", 22) высота rectPreviewBox.Height=36
        ' ImageList2 размером 32, значит надо квадрат 32*32
        rectPreviewBox.Offset(2, 2)
        rectPreviewBox.Height -= 4
        rectPreviewBox.Width = rectPreviewBox.Height 'rectPreviewBox.Height - 4 '32 '20
        g.DrawRectangle(New Pen(die.ForeColor), rectPreviewBox)

        'g.DrawImage(ImageListTree.Images(0), rectPreviewBox)

        ' получить подходящую кисть для выделения квадрата
        rectPreviewBox.Offset(1, 1)
        rectPreviewBox.Width -= 2
        rectPreviewBox.Height -= 2
        'g.FillRectangle(selectedBrush, rectPreviewBox)
        'g.DrawImage(ImageListTree.Images(die.Index), rectPreviewBox)

        'If Array.IndexOf(arrИндексыНенайденныхПараметров, die.Index) = -1 Then ' не найден, значит иконка по настройке
        g.DrawImage(inImageList.Images(CType(cmb.Items(die.Index), StringIntObject).i), rectPreviewBox)
        'Else ' индекс найден, иконка с крестиком
        '    g.DrawImage(ImageListTree.Images(12), rectPreviewBox)
        'End If

        ' выделить имя цветом
        'Dim drawFont As New Font("Arial", 16, FontStyle.Bold)
        Dim drawFont As Font = New Font(cmb.Font, cmb.Font.Style) ' Or FontStyle.Bold)

        'g.DrawString(ListBoxЕдИзмерения.Items(die.Index).ToString, New Font("Arial", 12, FontStyle.Bold), New SolidBrush(die.ForeColor), die.Bounds.X + 30, die.Bounds.Y + 1)
        g.DrawString(cmb.Items(die.Index).ToString, drawFont, New SolidBrush(die.ForeColor), die.Bounds.X + 30, die.Bounds.Y + 1)
    End Sub

    ''' <summary>
    ''' Асинхронная задержка за заданное время
    ''' </summary>
    ''' <param name="secondsDelay"></param>
    Public Sub Delay(secondsDelay As Double)
        Dim t = Tasks.Task.Run(Async Function()
                                   Await Tasks.Task.Delay(TimeSpan.FromSeconds(secondsDelay)).ConfigureAwait(False)
                               End Function)
        t.Wait()
    End Sub

#Region "Серелизация и десериализация ArrayList в Hashtable"
    ''' <summary>
    ''' Получить Hashtable Из Файла
    ''' По пути к XML файлу десериализовать его в Hashtable
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeserializeHashtableFromXML(ByVal strFileXML As String) As Hashtable
        ' Создать soap сериализатор.
        Dim sf As New SoapFormatter()

        ' Загрузить содержимое файла, используя этот же SoapFormatter объект
        Dim ht2 As Hashtable
        Using fs As New FileStream(strFileXML, FileMode.Open)
            ht2 = DirectCast(sf.Deserialize(fs), Hashtable)
        End Using
        ' Проверить, что объект был дессериализован корректно
        'For Each de As DictionaryEntry In ht2
        '    Console.WriteLine("Key={0}  Value={1}", de.Key, de.Value)
        'Next
        Return ht2
    End Function

    ''' <summary>
    ''' Записать Hashtable В Файл
    ''' Серелизация коллекции представленной ArrayList и запись в XML файл.
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <param name="ArrayListObject"></param>
    ''' <remarks></remarks>
    Private Sub SerializeHashtableToXML(ByVal strFileXML As String, ByVal ArrayListObject As ArrayList)
        ' на входе ArrayList переводим его в Hashtable со строковым ключом индекса
        ' Создать Hashtable объект, и заполнить его данными из коллекции.
        Dim I As Integer
        Dim ht As New Hashtable()

        For Each obj As Object In ArrayListObject
            ht.Add(I.ToString, obj)
            I += 1
        Next

        ' Создать a soap сериализатор.
        Dim sf As New SoapFormatter()
        ' Записать Hashtable на диск в SOAP формате.
        Using fs As New FileStream(strFileXML, FileMode.Create)
            sf.Serialize(fs, ht)
        End Using
    End Sub
#End Region

    'Public Class MyConnect
    '    ''' <summary>
    '    ''' Создать OleDbConnection
    '    ''' </summary>
    '    ''' <param name="dataPath"></param>
    '    ''' <returns></returns>
    '    Public Function OleDbConn(ByVal dataPath As String) As OleDbConnection
    '        ' Для того, чтобы создать OLE – соединение с локальной базой данных,
    '        ' необходимо указать 2 параметра – провайдера и путь к базе данных.
    '        ' Для наглядности я воспользовался построителем строк подключения – 
    '        ' OleDbConnectionStringBuilder.
    '        ' Указываем путь
    '        Dim bldr As New OleDbConnectionStringBuilder With {
    '            .DataSource = dataPath,
    '            .Provider = ProviderJet
    '        }
    '        ' Указываем провайдера
    '        ' Создаем подключение к источнику данных
    '        Return New OleDbConnection(bldr.ConnectionString)
    '    End Function

    '    ''' <summary>
    '    ''' Прочитать dbf-файл в DataTable
    '    ''' </summary>
    '    ''' <param name="oleDbConn"></param>
    '    ''' <param name="mySQL"></param>
    '    ''' <returns></returns>
    '    Public Function CopyDataTable(ByVal oleDbConn As OleDbConnection, ByVal mySQL As String) As DataTable
    '        ' Класс, необходимый для задания оператора SQL и источника данных
    '        ' Задаем оператор SQL
    '        Dim cmd As New OleDbCommand With {
    '                .CommandText = mySQL,
    '                .Connection = oleDbConn
    '            }
    '        ' Задаем источник данных
    '        ' Объект OleDbDataAdapter выполняет функцию моста 
    '        ' между DataTable и источником данных.
    '        Dim da As New OleDbDataAdapter With {
    '                .SelectCommand = cmd
    '            }

    '        Dim tbl As New DataTable()
    '        ' Обеспечивает он такой мост с помощью метода Fill.
    '        da.Fill(tbl)
    '        Return tbl
    '    End Function
    'End Class

End Module