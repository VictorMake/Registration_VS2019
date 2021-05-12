Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports System.Xml.Linq
Imports MathematicalLibrary

Public Class FormTestCompactRio

#Region "Properties"
    Private mPathSettingMdb As String
    ''' <summary>
    ''' Путь к каталогу с дополнительными файлами к программе.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathCatalog() As String

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
            mPathSettingMdb = value

            If Not File.Exists(value) Then
                MessageBox.Show($"В каталоге нет файла <{value}>!", $"Запуск модуля {NameOf(FormTestCompactRio)}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
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
    Public Property PathSettingXml() As String
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
    ''' Описание предназначения плагина.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Description() As String = "Это Тестовая форма CompactRio"

    'Private mIsDllVisible As Boolean = False
    '''' <summary>
    '''' Видима DLL или нет, т.е. имеются вкладки с другими окнами или она только вычисляет.
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Overridable ReadOnly Property IsDllVisible() As Boolean
    '    Get
    '        Return mIsDllVisible
    '    End Get
    'End Property

    ''' <summary>
    ''' Свойство для управления родителем закрытия окон плагина.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsWindowClosed() As Boolean

    Private Sub FormTestCompactRio_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If IsWindowClosed Then
            SavePathSettinngXml()
        Else
            e.Cancel = True
            Return
        End If

        StopAcquisition()
    End Sub

    Private mFrequencyAcquisition As Integer
    Private Property FrequencyAcquisition() As Integer
        Get
            Return mFrequencyAcquisition
        End Get
        Set(ByVal value As Integer)
            mFrequencyAcquisition = value
            timerIntervalTest = 1000 \ value
            TimerIntervalWait = timerIntervalTest
        End Set
    End Property

    Public Property TimerIntervalWait As Integer = timerIntervalTest
    Public Property IsStartAcquisition() As Boolean
#End Region

    Private AcquisitionTimerChassis As Double() ' полученные данные каналов от Сервера получаются потребителями из основного окна, 
    Private eventHandlerTimerTick As EventHandler
    Private syncPoint As Integer = 0 ' для синхронизации
    Private WithEvents mmTimer As Multimedia.Timer
    Private timerIntervalTest As Integer = 10 ' миллисекунд
    Private ReadOnly mFormMainMDI As FormMainMDI

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
        ' TODO: изменить инициализацию  размерности массива по числу каналов для подключенных шасси, т.к. некоторые могут быть отключены
        ' значить и ParametersType должен учитывать подключенные шасси (а в базе этих полей может не быть)
        Re.Dim(AcquisitionTimerChassis, ParametersType.Length - 1)

        'TestInitialize()
    End Sub

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
    Public Sub LoadConfiguration()
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

    Private Sub FormTestCompactRio_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadConfiguration()
        ' TODO: снять Me.TopMost = False после того как все шасси законнектятся и будет запущена команда передачи данных в основное Окно Registration

    End Sub
#End Region

    ''' <summary>
    ''' Запуск таймера
    ''' </summary>
    ''' <param name="inEventHandlerTimerTick"></param>
    Public Sub StartAcquisitionTimer(inEventHandlerTimerTick As EventHandler)
        'Public Sub StartAcquisitionTimer(inEventHandlerTimerTick As Action(Of Object, EventArgs))
        syncPoint = 0
        mmTimer = New Multimedia.Timer() With {.Mode = Multimedia.TimerMode.Periodic,
                                               .Period = timerIntervalTest,
                                               .Resolution = 1}

        Thread.CurrentThread.Priority = ThreadPriority.Normal
        ' для отслеживания события в форме назначить объект синхронизации (должен быть компонент)
        ' если таймер работает самостоятельно, то форму назначать не надо
        mmTimer.SynchronizingObject = mFormMainMDI ' необходимо для отслеживания вызова событий

        eventHandlerTimerTick = inEventHandlerTimerTick

        Try
            'RunSendRecive(True)

            'AddHandler mmTimer.Tick, AddressOf RegistrationTimerTick
            AddHandler mmTimer.Tick, eventHandlerTimerTick
            mmTimer.Start()
        Catch ex As Exception
            Dim CAPTION As String = $"Error {NameOf(StartAcquisitionTimer)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")
        End Try

        IsStartAcquisition = True
    End Sub

    ''' <summary>
    ''' Остановить таймер
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopAcquisition()
        'RunSendRecive(False)
        If mmTimer IsNot Nothing Then
            mmTimer.Stop() ' может быть не создан. если было нарушение соотвествия конфигурации каналов Сервера
            'RemoveHandler mmTimer.Tick, AddressOf RegistrationTimerTick
            RemoveHandler mmTimer.Tick, eventHandlerTimerTick
        End If
        IsStartAcquisition = False
        If RegistrationMain IsNot Nothing Then RegistrationMain.IsFormRunning = False
    End Sub

    ''' <summary>
    ''' Handles mmTimer.Tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub RegistrationTimerTick(sender As Object, e As EventArgs) 'Handles mmTimer.Tick
        If IsStartAcquisition Then 'TODO: добавить проверку сети AndAlso _Tcp_Client IsNot Nothing AndAlso _Tcp_Client.Connected Then
            OnTimedRegistrationTimer() ' асинхронное чтение
        Else
            mmTimer.Stop()
            'RemoveHandler mmTimer.Tick, AddressOf RegistrationTimerTick
            RemoveHandler mmTimer.Tick, eventHandlerTimerTick
            'RunSendRecive(False)
        End If
    End Sub

    ''' <summary>
    ''' Handles mmTimer.Tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub TarirTimerTick(sender As Object, e As EventArgs) 'Handles mmTimer.Tick
        If IsStartAcquisition Then 'TODO: добавить проверку сети AndAlso _Tcp_Client IsNot Nothing AndAlso _Tcp_Client.Connected Then
            OnTimedTarirTimer() ' асинхронное чтение
        Else
            mmTimer.Stop()
            'RemoveHandler mmTimer.Tick, AddressOf RegistrationTimerTick
            RemoveHandler mmTimer.Tick, eventHandlerTimerTick
            'RunSendRecive(False)
        End If
    End Sub

    Private random As Random = New Random
    'Private Sub TestInitialize()
    '    'TODO: убрать тест
    '    For I As Integer = 0 To AcquisitionValueOfDouble.Length - 1
    '        AcquisitionValueOfDouble(I) = I
    '    Next
    'End Sub

    Dim counterTimer As Integer
    Private Sub TestInitializeRandom()
        counterTimer += 1
        'TODO: убрать тест
        For I As Integer = 0 To AcquisitionTimerChassis.Length - 1
            Dim amplidude As Double = random.NextDouble * 5.0
            AcquisitionTimerChassis(I) = Math.Sin(counterTimer / 500.0 * Math.PI) * amplidude + amplidude / 10.0
            'AcquisitionValueOfDouble(I) = random.NextDouble * 10.0
        Next
    End Sub

    ''' <summary>
    ''' Общая функция для всех таймеров
    ''' Осуществляет посылку, приём и разборку по массивам полученных данных на синхронном блокирующем сокете
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnTimedRegistrationTimer()
        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

        If sync = 0 Then
            'If _Tcp_Client.Connected Then
            ' CompactRioAcquiredData: при получении из событий первый буфер немного быстрее (пропуск 3 такта затем по 2)
            ' но если включена запись и вызвать передачу данных в сеть клиентам, то происходит зависание.
            ' RegistrationMain.DataValuesFromServer = AcquisitionValueOfDouble: при премом вызове пропуск 4 такта затем так же,
            ' восстановление записи происходит нормально.
            'RaiseEvent CompactRioAcquiredData(Me, New AcquiredDataEventArgs(AcquisitionValueOfDouble))

            TestInitializeRandom()
            RegistrationMain.DataValuesFromServer = AcquisitionTimerChassis
            RegistrationMain.AcquiredData()
            syncPoint = 0 ' освободить

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
        Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)

        If sync = 0 Then
            TestInitializeRandom()
            TarirForm.AcquiredData(AcquisitionTimerChassis)
            syncPoint = 0 ' освободить
        End If
    End Sub

    Private Sub ButtonStartAcquisitionTimer_Click(sender As Object, e As EventArgs) Handles ButtonStartAcquisitionTimer.Click
        'If IsEnableStartTimer Then StartAcquisitionTimer()
        Me.TopMost = False
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

End Class