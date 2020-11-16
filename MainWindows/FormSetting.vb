Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.IO
Imports MathematicalLibrary
Imports NationalInstruments.Analysis.Math
Imports NationalInstruments.DAQmx
Imports Registration.SettingConstantChannels

Public Class FormSetting
    Private servers() As String
    Private clients() As String = {cClient}
    Private numberServerStend As String
    Private numberClientStend As String
    Private modeApplicationRun As String
    Private pathServerStend As String
    Private pathClientStend As String
    Private pathOptionServer As String
    Private pathWorkDataBase As String
    Private isCompensationReferenceJunction As Boolean ' запись компенсации ХС произведена
    Private recordingFrameTime As String ' длтельность кадра

    Private ReadOnly numberStend As String
    Private ReadOnly modification As String
    Private ReadOnly numberProduct As Integer
    Private ReadOnly mFrequencyBackground As Short ' Частота Фонового
    Private mEngineManager As EngineManager
    Private listKeyValuePair As New List(Of KeyValuePair(Of String, ComboBox))
    Private imagesCross As Integer

    Private Const cServer As String = "Сервер"
    Private Const cClient As String = "Клиент"
    Private Const cSnapshot As String = "Снимок"
    Private Const cClientТСР As String = "КлиентТСР"

    ''' <summary>
    ''' менеджер настроек списков константных каналов
    ''' </summary>
    Private mSettingSelectedParameters As SettingConstantChannels

    'Является обязательной для конструктора форм Windows Forms для связывания с DataGridView
    Public Property FormComponents As System.ComponentModel.IContainer
#Region "Form"
    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавить все инициализирующие действия после вызова InitializeComponent().

        If IsRunAutoBooting Then
            numberStend = StandNumber
            modification = ModificationEngine
            numberProduct = NumberEngine
            mFrequencyBackground = FrequencyBackground
        End If

        FormComponents = Me.components
        InitializeForm()
    End Sub

    Public Sub InitializeForm()
        ' сделать проверку был ли ввод формы чтобы записать прежде введенные данные, а не выводить пустые поля
        ComboFrequencyCollection.Items.AddRange({"1", "2", "5", "10", "20", "50", "100"})
        ComboNumberStend.Items.AddRange(GetIni(PathOptions, "Stend", "Stends", "11,13,15,16,17,19,21,25,34,37,39,41,Клиент").Split(CType(",", Char())))
        'ReDim_servers(ComboNumberStend.Items.Count - 2) ' без Клиент
        Re.Dim(servers, ComboNumberStend.Items.Count - 2) ' без Клиент
        PopulateComboBoxPath(ComboPathServer)
        PopulateComboBoxPath(ComboPathClient)

        ComboPathServer.SelectedIndex = 0
        ComboPathClient.SelectedIndex = 0
        TextPathServer.Text = servers(0)
        TextPathClient.Text = clients(0)
        DTPickerDate.Value = Today

        Dim mKRDsManager As New KRDsManager
        ComboTypeKrd.Items.AddRange(mKRDsManager.AllKRDKeysToArray)
        ComboTypeKrd.SelectedIndex = 1 ' по умолчанию активный

        ' по умолчанию
        modeApplicationRun = cSnapshot
        StandNumber = "25"
        numberServerStend = StandNumber
        numberClientStend = StandNumber
        NumberEngine = 0
        ModificationEngine = cEngine39
        TypeKRD = cKRD_B
        TemperatureOfBox = 20
        FrequencyBackground = 10
        Precision = 2
        TextDisplayRate = DisplayRate.VeryFast
        recordingFrameTime = "30"

        Try
            ' считывание записанных опций
            '0)Запуск
            modeApplicationRun = GetIni(PathOptions, "Stend", "Start", cSnapshot)
            Select Case modeApplicationRun
                Case cServer
                    ButtonWithController.Checked = True
                Case cClientТСР
                    ButtonWithClientТСР.Checked = True
                Case cSnapshot
                    ButtonWithSnapshop.Checked = True
                Case cClient
                    ButtonWithClient.Checked = True
                Case Else
                    ButtonWithSnapshop.Checked = True
            End Select

            ' 1)номер стенда
            StandNumber = GetIni(PathOptions, "Stend", "Stend", "25")

            For I As Integer = 0 To ComboNumberStend.Items.Count - 1
                If ComboNumberStend.Items(I).ToString = StandNumber Then
                    ComboNumberStend.SelectedIndex = I
                    Exit For
                End If
            Next

            ' 2)номер изделия
            NumberEngine = CInt(GetIni(PathOptions, "Product", "Nizdelia", "555"))
            TextNumberProduct.Text = CStr(NumberEngine)

            ' 8)модификация
            ModificationEngine = GetIni(PathOptions, "Product", "Modifikacia", cEngine39)
            mEngineManager = New EngineManager
            ComboEngine.Items.AddRange(mEngineManager.AllEnginesKeysToArray)
            ComboEngine.SelectedIndex = 1

            If mEngineManager.AllEnginesKeysToArray.Contains(ModificationEngine) Then
                ComboEngine.SelectedIndex = Array.IndexOf(mEngineManager.AllEnginesKeysToArray, ModificationEngine)
            End If

            ' 9)режим
            ComboRegime.Items.Clear()
            ComboRegime.Items.Add(B)
            ComboRegime.Items.Add(UB)

            If ComboEngine.Text = cEngine99A Then ComboRegime.Items.Add(O_R)

            Select Case GetIni(PathOptions, "Product", "Mode", B)
                Case B
                    ComboRegime.SelectedIndex = 0
                    Exit Select
                Case UB
                    ComboRegime.SelectedIndex = 1
                    Exit Select
                Case O_R
                    ComboRegime.SelectedIndex = 2
                    Exit Select
            End Select

            ' 11)тип КРД
            TypeKRD = GetIni(PathOptions, "KRD", "KRD", cKRD_B)
            If mKRDsManager.AllKRDKeysToArray.Contains(TypeKRD) Then
                ComboTypeKrd.SelectedIndex = Array.IndexOf(mKRDsManager.AllKRDKeysToArray, TypeKRD)
            End If

            ' 12)температура бокса
            TemperatureOfBox = CDbl(GetIni(PathOptions, "Weather", "Tboksa", "20"))
            TextTbox.Text = CStr(TemperatureOfBox)

            '17)Частота фонового сбора
            FrequencyBackground = CShort(GetIni(PathOptions, "Options", "Frequency", "10"))
            ComboFrequencyCollection.Text = CStr(FrequencyBackground)
            recordingFrameTime = GetIni(PathOptions, "Options", "TimeOfSnapshot", "30")
            SetIntervalSnapshot(recordingFrameTime)

            '18)Точность
            Precision = CShort(GetIni(PathOptions, "Options", "Discredit", "2"))
            NumPrecision.Value = Precision

            '19)Скорость текстового отображения
            TextDisplayRate = DirectCast([Enum].Parse(GetType(DisplayRate), GetIni(PathOptions, "Options", "DisplayRate", "VeryFast")), DisplayRate)
            'IsUseTdms = CBool(GetIni(PathOptions, "Options", "UseTdms", "False"))
            IsUseTdms = Not (GetIni(PathOptions, "Options", "UseTdms", "True") = "TXT")

            '20)номер стенда сервера
            numberServerStend = GetIni(PathOptions, "Stend", "StendServer", "25")
            For I As Integer = 0 To ComboPathServer.Items.Count - 1
                If CStr(ComboPathServer.Items(I)) = numberServerStend Then
                    ComboPathServer.SelectedIndex = I
                    Exit For
                End If
            Next

            '21)номер стенда клиента
            numberClientStend = GetIni(PathOptions, "Stend", "StendClient", "25")
            For I As Integer = 0 To ComboPathClient.Items.Count - 1
                If CStr(ComboPathClient.Items(I)) = numberClientStend Then
                    ComboPathClient.SelectedIndex = I
                    Exit For
                End If
            Next

            ' 29) Конфигурация графиков от параметра
            SettingKeyConfiguration = CInt(GetIni(PathOptions, "Stend", "keyКонфигурация", "0"))
            NameVarStartWrite = GetIni(PathOptions, "StartStopWrite", "NameVarStartWrite", conЗапуск)
            NameVarStopWrite = GetIni(PathOptions, "StartStopWrite", "NameVarStopWrite", conN1)

            WaitStartWrite = CInt(GetIni(PathOptions, "StartStopWrite", "WaitStartWrite", "1"))
            TextWaitStartWrite.Text = CStr(WaitStartWrite)

            ValueStopWrite = CDbl(GetIni(PathOptions, "StartStopWrite", "ValueStopWrite", "1"))
            TextValueStopWrite.Text = CStr(ValueStopWrite)

            WaitStopWrite = CInt(GetIni(PathOptions, "StartStopWrite", "WaitStopWrite", "1"))
            TextWaitStopWrite.Text = CStr(WaitStopWrite)

            ' параметры для приведения и компенсации холодного спая
            NameTBox = GetIni(PathOptions, "Const", "contбокса", contбокса)
            NameTx = GetIni(PathOptions, "Const", "conТхс", conТхс)
            PopulateComboBoxChannelsName()
            ' число клиентов или номер самого клиента
            CountClientOrNumberClient = CInt(GetIni(PathOptions, "Stend", "CountClientOrNumberClient", "1"))
            ComboBoxCountClientOrNumberClient.SelectedIndex = CountClientOrNumberClient - 1
        Catch ex As Exception
            Const caption As String = "Ошибка при считывании настроек из файла <Опции.ini>"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        End Try

        hpPlainHTML.HelpNamespace = Path.Combine(PathResourses, "help.htm")

        If IsUseTdms Then
            LabelIntervalSnapshot.Text = "Длительность кадра минут"
        Else
            LabelIntervalSnapshot.Text = "Длительность кадра секунд/минут"
        End If

        UpdateForm()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна Опции")
    End Sub

    Private Sub FormSetting_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsRunAutoBooting Then
            ButtonWithClient.Checked = False
            ButtonWithClientТСР.Checked = False
            ButtonWithController.Checked = False

            StandNumber = numberStend
            ComboNumberStend.Text = numberStend
            ModificationEngine = modification

            If mEngineManager.AllEnginesKeysToArray.Contains(modification) Then
                ComboEngine.SelectedIndex = Array.IndexOf(mEngineManager.AllEnginesKeysToArray, modification)
            End If

            NumberEngine = numberProduct
            TextNumberProduct.Text = numberProduct.ToString

            FrequencyBackground = mFrequencyBackground
            ComboFrequencyCollection.Text = mFrequencyBackground.ToString

            Close()
        Else
            PopulateConstantChannels()
        End If
    End Sub

    Private Sub FormSetting_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ButtonApply.PerformClick() ' не вызывать здесь DataEntry() т.к. будет 2 раза записываться при закрытии окна
    End Sub

    ''' <summary>
    ''' Заполнить список
    ''' </summary>
    ''' <param name="cmbBox"></param>
    Private Sub PopulateComboBoxPath(ByVal cmbBox As ComboBox)
        If cmbBox Is ComboPathServer Then
            For I As Integer = 0 To ComboNumberStend.Items.Count - 2
                cmbBox.Items.Add(ComboNumberStend.Items(I))
                servers(I) = GetIni(PathOptions, "Server", "Stend" & CStr(ComboNumberStend.Items(I)), "\\Stend_25\D\Registration\Store\Channels.mdb")
            Next
        End If

        If cmbBox Is ComboPathClient Then
            cmbBox.Items.Add(cClient)
            clients(0) = GetIni(PathOptions, "Client", "Stend" & cClient, "\\Stend_25\D\Registration\Store\Channels.mdb")
        End If
    End Sub
#End Region

#Region "Ввод и проверка данны"
    ''' <summary>
    ''' Проверка непротиворечивочти режимов и стендов.
    ''' </summary>
    Private Function CheckCorrectSetting() As Boolean
        Const TITLE As String = "Ошибка ввода данных"
        Dim message As String

        If ButtonWithClient.Checked = True Then
            If TextPathServer.Text = PathChannels Then
                message = "Измените путь к базе данных каналов Регистратора" & vbCrLf &
                    "для стенда выбранного как Сервер на вкладке <Сбор>." & vbCrLf &
                    "Приём данных с существующими параметрами невозможен."
                MessageBox.Show(message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{TITLE}> {message}")
                Return False
            End If

            'If Environment.MachineName.ToUpper.IndexOf("STEND") <> -1 Then
            '    'If Environment.MachineName.IndexOf("006-VICTOR") <> -1 Then
            '    ' номер рабочего стенда и номер стенда клиента равны чревато последствиями
            '    message = "Компьютер является стендовым регистратором и не может быть запущен как Клиент!" & vbCrLf &
            '        "Это чревато удалением текущей рабочей базы." & vbCrLf &
            '        "Операция с такими параметрами невозможна."
            '    MessageBox.Show(message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{TITLE}> {message}")
            '    Return False
            'End If
        Else
            If ComboNumberStend.Text = cClient Then 'ComboNumberStend.Items(ComboNumberStend.Items.Count - 1) Then
                ' не должен быть равен Клиент
                message = "Номер стенда не может быть равен Клиент!" & vbCrLf &
                    "Необходимо выбрать номер." & vbCrLf &
                    "Операция с такими параметрами невозможна."
                MessageBox.Show(message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{TITLE}> {message}")
                Return False
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' Ввод и проверка данных перед закрытием формы
    ''' </summary>
    Private Sub DataEntry()
        If Not CheckCorrectSetting() Then Exit Sub

        StandNumber = ComboNumberStend.Text
        NumberEngine = CInt(TextNumberProduct.Text)

        IsTcpClient = ButtonWithClientТСР.Checked
        IsWorkWithController = ButtonWithController.Checked
        IsClient = ButtonWithClient.Checked

        If IsClient Then
            ServerWorkingFolder = servers(ComboPathServer.SelectedIndex)
            ' обрезать строку channels
            ServerWorkingFolder = Mid(ServerWorkingFolder, 1, InStrRev(ServerWorkingFolder, "Channels") - 1)
            pathOptionServer = ServerWorkingFolder & "Опции.ini"

            If Not CheckFileServer(pathOptionServer) Then
                Environment.Exit(0)
            End If

            ' 1)номер стенда
            StandNumber = GetIni(pathOptionServer, "Stend", "Stend", "25")
            ' считать путь к рабочей базе которая может быть сменена с сервера (только для клиента)
            pathWorkDataBase = GetIni(pathOptionServer, "TheCurrentBase", "Base", "ОшибкаПутиТекущейБазеСевера")
            TakeDBaseFromServer()
        Else ' сервер
            ClientWorkingFolder = clients(ComboPathClient.SelectedIndex)
            ' обрезать строку channels
            ClientWorkingFolder = Mid(ClientWorkingFolder, 1, InStrRev(ClientWorkingFolder, "Channels") - 1)
            ' записать путь к рабочей базе только не для клиента
            WriteINI(PathOptions, "TheCurrentBase", "Base", PathChannels)
        End If

        SetLastChannelDBase()
        LoadChannels() ' в шапке выбирается номер стенда

        If Not isCompensationReferenceJunction Then SaveCoefficientBringingTBoxing()

        SaveINI()
        If IsWorkWithController Then mSettingSelectedParameters.SaveAndRestoreAfterSaving()
        Hide()
        DialogResult = DialogResult.OK
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<Применение настроек Опции> Клиент={IsClient} , ТСРКлиент={IsTcpClient} , Работа с контроллером={IsWorkWithController} , Номер стенда={StandNumber} , Номер изделия={NumberEngine}")
    End Sub

    ''' <summary>
    ''' Последняя таблица каналов стенда.
    ''' Найти последнюю по времени создания таблицу  данного стенда.
    ''' </summary>
    Private Sub SetLastChannelDBase()
        Dim dateCreated As Date
        Dim tableName As String = "Channel" & StandNumber
        Dim conn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))

        conn.Open()
        Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        conn.Close()
        dateCreated = CDate(schemaTable.Rows(0)("DATE_CREATED"))

        ' найти дату создания первой таблицы данного стенда
        For Each row As DataRow In schemaTable.Rows
            If CBool(InStr(1, row("TABLE_NAME").ToString, tableName)) Then
                dateCreated = CDate(row("DATE_CREATED"))
                Exit For
            End If
        Next

        ' проверить есть, ли еще таблицы данного стенда и найти последнюю по времени создания
        For Each row As DataRow In schemaTable.Rows
            If CBool(InStr(1, row("TABLE_NAME").ToString, tableName)) Then
                If CDate(row("DATE_CREATED")) >= dateCreated Then
                    dateCreated = CDate(row("DATE_CREATED"))
                    ChannelLast = row("TABLE_NAME").ToString
                End If
            End If
        Next

        If ChannelLast Is Nothing Then
            Dim caption As String = $"Отсутствует таблица с именем <{tableName}>"
            Dim text As String = $"В базе данных <{PathChannels}> отсутствует таблица с именем <{tableName}>.{vbCrLf}Продолжение работы невозможно!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) 'End
        End If

        CheckTableNameRegime()
    End Sub

    ''' <summary>
    ''' Проверка наличия файла
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    Private Function CheckFileServer(ByVal file As String) As Boolean
        If FileNotExists(file) Then
            Const caption As String = "Проверка наличия файла"
            Dim text As String = $"По указанному пути нет файла {file} !{vbCrLf}Проверьте сетевое окружение.{vbCrLf}Зазгузка будет прекращена."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub SaveINI()
        '0)Запуск
        WriteINI(PathOptions, "Stend", "Start", modeApplicationRun)
        '1)номер стенда
        WriteINI(PathOptions, "Stend", "Stend", ComboNumberStend.Text)
        '2)номер изделия
        WriteINI(PathOptions, "Product", "Nizdelia", CStr(NumberEngine))
        ''7)вид испытаний
        ''     "Контрольные"
        ''     "Длительные"
        ''     "Сдаточные"
        'strВидИспытаний = cmdВидИспытаний.Text
        'WriteINI(PathOptions, "Product", "VidIspitanij", strВидИспытаний)
        '8)модификация
        ModificationEngine = ComboEngine.Text
        WriteINI(PathOptions, "Product", "Modifikacia", ModificationEngine)
        ''9)режим
        ModeRegime = ComboRegime.Text
        WriteINI(PathOptions, "Product", "Mode", ModeRegime)
        '11)тип КРД
        TypeKRD = ComboTypeKrd.Text
        WriteINI(PathOptions, "KRD", "KRD", TypeKRD)
        '12)температура бокса
        TemperatureOfBox = CSng(TextTbox.Text)
        WriteINI(PathOptions, "Weather", "Tboksa", CStr(TemperatureOfBox))
        '17)Частота фонового сбора
        FrequencyBackground = CShort(ComboFrequencyCollection.Text)
        WriteINI(PathOptions, "Options", "Frequency", CStr(FrequencyBackground))
        WriteINI(PathOptions, "Options", "TimeOfSnapshot", ComboIntervalSnapshot.Text)
        '18)Точность
        Precision = CInt(NumPrecision.Value)
        WriteINI(PathOptions, "Options", "Discredit", CStr(Precision))
        '20)номер стенда сервера
        numberServerStend = ComboPathServer.Text
        WriteINI(PathOptions, "Stend", "StendServer", numberServerStend)
        '21)номер стенда клиента
        numberClientStend = ComboPathClient.Text
        WriteINI(PathOptions, "Stend", "StendClient", numberClientStend)

        If ComboBoxNameVarStopWrite.Text <> NameVarStopWrite AndAlso ComboBoxNameVarStopWrite.Text <> "" Then
            NameVarStopWrite = ComboBoxNameVarStopWrite.Text
            WriteINI(PathOptions, "StartStopWrite", "NameVarStopWrite", NameVarStopWrite)
        End If

        ValueStopWrite = CDbl(TextValueStopWrite.Text)
        WriteINI(PathOptions, "StartStopWrite", "ValueStopWrite", CStr(ValueStopWrite))

        WaitStopWrite = CInt(TextWaitStopWrite.Text)
        WriteINI(PathOptions, "StartStopWrite", "WaitStopWrite", CStr(WaitStopWrite))
        WaitStopWrite = CInt(WaitStopWrite * 1000)

        If ComboBoxNameVarStartWrite.Text <> NameVarStartWrite AndAlso ComboBoxNameVarStartWrite.Text <> "" Then
            NameVarStartWrite = ComboBoxNameVarStartWrite.Text
            WriteINI(PathOptions, "StartStopWrite", "NameVarStartWrite", NameVarStartWrite)
        End If

        WaitStartWrite = CInt(TextWaitStartWrite.Text)
        WriteINI(PathOptions, "StartStopWrite", "WaitStartWrite", CStr(WaitStartWrite))
        WaitStartWrite = CInt(WaitStartWrite * 1000)

        If ComboBoxChannelTbox.Text <> NameTBox AndAlso ComboBoxChannelTbox.Text <> "" Then
            NameTBox = ComboBoxChannelTbox.Text
            WriteINI(PathOptions, "Const", "contбокса", NameTBox)
        End If

        If ComboBoxTxc.Text <> NameTx AndAlso ComboBoxTxc.Text <> "" Then
            NameTx = ComboBoxTxc.Text
            WriteINI(PathOptions, "Const", "conТхс", NameTx)
        End If

        If IsClient Then TakeOptionIniFromServer()
        MainMdiForm.Text = "Изделие №" & Str(NumberEngine)
        ' число клиентов или номер самого клиента
        CountClientOrNumberClient = Convert.ToInt32(ComboBoxCountClientOrNumberClient.Text)
        WriteINI(PathOptions, "Stend", "CountClientOrNumberClient", CStr(CountClientOrNumberClient))
    End Sub

    Private Sub ButtonApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonApply.Click
        DataEntry()
    End Sub

    ''' <summary>
    ''' Взять с сервера базу
    ''' </summary>
    Private Sub TakeDBaseFromServer()
        ' копироаит таблицу каналов сервера по номеру стенда, если копмпьютер сервера не локальный
        Dim conn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = conn.CreateCommand
        cmd.CommandType = CommandType.Text

        conn.Open()
        Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing) 'New Object() {Nothing, Nothing, Nothing, "TABLE"})
        'Dim dc As DataColumn
        'For Each dc In schemaTable.Columns
        '    Console.WriteLine(dc.ColumnName)
        'Next dc
        'будет выведено
        'TABLE_CATALOG()
        'TABLE_SCHEMA()
        'TABLE_NAME()
        'TABLE_TYPE()
        'TABLE_GUID()
        'DESCRIPTION()
        'TABLE_PROPID()
        'DATE_CREATED()
        'DATE_MODIFIED()

        'For Each row In schemaTable.Rows
        '    Console.WriteLine(vbTab & row("TABLE_NAME").ToString)
        'Next row

        Dim tableName As String = "Channel" & StandNumber

        If CBool(InStr(1, ServerWorkingFolder, "\\")) Then 'другой компьютер
            pathWorkDataBase = Mid(ServerWorkingFolder, 1, InStr(3, ServerWorkingFolder, "\")) & Replace(pathWorkDataBase, ":\", "\")
        End If

        For Each row As DataRow In schemaTable.Rows
            If row("TABLE_NAME").ToString = tableName Then  'база есть
                cmd.CommandText = "DELETE * FROM " & tableName
                cmd.ExecuteNonQuery()
                cmd.CommandText = "INSERT INTO " & tableName & " SELECT * FROM " & tableName & " IN " & """" & pathWorkDataBase & """" & ";"
                cmd.ExecuteNonQuery()
                Exit For
            End If
        Next

        tableName = "Режимы" & StandNumber
        For Each row As DataRow In schemaTable.Rows
            If row("TABLE_NAME").ToString = tableName Then  'база есть
                cmd.CommandText = "DELETE * FROM " & tableName
                cmd.ExecuteNonQuery()
                cmd.CommandText = "INSERT INTO " & tableName & " SELECT * FROM " & tableName & " IN " & """" & pathWorkDataBase & """" & ";"
                cmd.ExecuteNonQuery()
                Exit For
            End If
        Next
        conn.Close()
    End Sub

    ''' <summary>
    ''' Взять с сервера опции
    ''' </summary>
    Private Sub TakeOptionIniFromServer()
        Try
            ' 2)номер изделия
            NumberEngine = CInt(GetIni(pathOptionServer, "Product", "Nizdelia", "555"))
            '' 7)вид испытаний
            'Dim strВидИспытаний As String
            'strВидИспытаний = sGetIni(PathOptionsСервера, "Product", "VidIspitanij", "Контрольные")
            ' 8)модификация
            ModificationEngine = GetIni(pathOptionServer, "Product", "Modifikacia", cEngine39)
            '' 9)режим
            'Dim strРежим As String
            'strРежим = sGetIni(PathOptionsСервера, "Product", "Mode", B)
            '' 11)тип КРД
            TypeKRD = GetIni(pathOptionServer, "KRD", "KRD", cKRD_B)
            ' 12)температура бокса
            TemperatureOfBox = CDbl(GetIni(pathOptionServer, "Weather", "Tboksa", "20"))
            ' 17)Частота фонового сбора
            FrequencyBackground = CShort(GetIni(pathOptionServer, "Options", "Frequency", "10"))
        Catch ex As Exception
            Const caption As String = "Ошибка при считывании настроек из файла <Опции.ini>"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        End Try

        SetFrequency(CStr(FrequencyBackground))
    End Sub

#End Region

#Region "Настойка Формы от типа приложения"
    Private Sub ButtonWork_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonWithController.CheckedChanged,
                                                                                                            ButtonWithClientТСР.CheckedChanged,
                                                                                                            ButtonWithSnapshop.CheckedChanged,
                                                                                                            ButtonWithClient.CheckedChanged
        CheckTypeWork()
    End Sub

    ''' <summary>
    ''' Поиск выделенного контрола с типом RadioButton
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedRadioButton() As RadioButton
        Dim tempRadioButton As RadioButton
        Dim checkedRadioButton As RadioButton = ButtonWithController ' по умолчанию

        ' Пройти по контролам верхнего уровня
        For Each cntrl As Control In FrameTypeWork.Controls
            If cntrl.GetType Is GetType(RadioButton) Then
                tempRadioButton = CType(cntrl, RadioButton)
                If tempRadioButton.Checked Then
                    tempRadioButton.BackColor = Color.LightSteelBlue
                    tempRadioButton.ForeColor = Color.Blue
                    checkedRadioButton = tempRadioButton
                Else
                    tempRadioButton.BackColor = Color.WhiteSmoke
                    tempRadioButton.ForeColor = SystemColors.ControlText
                End If
            End If
        Next

        Return checkedRadioButton
    End Function

    ''' <summary>
    ''' Определить тип запуска.
    ''' </summary>
    Private Sub CheckTypeWork()
        Dim checkedRadioButton As RadioButton = GetSelectedRadioButton()
        ComboNumberStend.Enabled = True

        If checkedRadioButton Is ButtonWithController Then
            modeApplicationRun = cServer
        ElseIf checkedRadioButton Is ButtonWithClientТСР Then
            modeApplicationRun = cClientТСР
        ElseIf checkedRadioButton Is ButtonWithSnapshop Then
            modeApplicationRun = cSnapshot
        ElseIf checkedRadioButton Is ButtonWithClient Then
            modeApplicationRun = cClient
            ComboNumberStend.SelectedIndex = ComboNumberStend.Items.Count - 1
            ComboNumberStend.Enabled = False
        End If

        UpdateForm()
    End Sub

    ''' <summary>
    ''' Настойка Формы
    ''' </summary>
    Private Sub UpdateForm()
        'If Me.IsHandleCreated Then
        IsUseTCPClient = False
        TuneFrameProduct(True)
        TuneCountClientOrNumberClient(False)

        Select Case modeApplicationRun
            Case cServer
                FraProduct.Visible = True
                ComboIntervalSnapshot.Visible = IsUseTdms
                LabelIntervalSnapshotFact.Visible = Not IsUseTdms
                SetControlsEnabled(True)
                SetFramesEnabledServerOrClient(False)
                TuneCountClientOrNumberClient(True)
                Exit Select
            Case cClientТСР
                IsUseTCPClient = True
                FraProduct.Visible = True
                ComboIntervalSnapshot.Visible = IsUseTdms
                LabelIntervalSnapshotFact.Visible = Not IsUseTdms
                SetControlsEnabled(True)
                SetFramesEnabledServerOrClient(False)

                LabelDiscredit.Visible = False
                LabelDiscreditFact.Visible = False
                LabelFrequencySamplingChannel.Visible = False
                TextFrequencySamplingChannel.Visible = False
                Exit Select
            Case cSnapshot
                FraProduct.Visible = True
                TuneFrameProduct(False)
                ComboIntervalSnapshot.Visible = False
                LabelIntervalSnapshotFact.Visible = False
                SetControlsEnabled(False)
                SetFramesEnabledServerOrClient(False)
                Exit Select
            Case cClient ', "Контрольная точка"
                FraProduct.Visible = False
                ComboIntervalSnapshot.Visible = False
                LabelIntervalSnapshotFact.Visible = False
                SetControlsEnabled(False)
                SetFramesEnabledServerOrClient(True)
                Exit Select
        End Select
    End Sub

    Private Sub SetControlsEnabled(isVisible As Boolean)
        TabPageStartStop.Enabled = isVisible
        TabPageChannels.Enabled = isVisible
        LabelDiscredit.Visible = isVisible
        LabelDiscreditFact.Visible = isVisible
        LabelFrequencySamplingChannel.Visible = isVisible
        TextFrequencySamplingChannel.Visible = isVisible
        LabelIntervalSnapshot.Visible = isVisible
        LabelPrecision.Visible = isVisible
        NumPrecision.Visible = isVisible
        LabelFrequencyCollection.Visible = isVisible
        ComboFrequencyCollection.Visible = isVisible
    End Sub

    Private Sub SetFramesEnabledServerOrClient(isVisible As Boolean)
        FrameStendServer.Enabled = isVisible
        FrameStendClient.Enabled = Not isVisible
    End Sub

    Private Sub TuneFrameProduct(visible As Boolean)
        LabelNumberProduct.Visible = visible
        TextNumberProduct.Visible = visible
        LabelTbox.Visible = visible
        TextTbox.Visible = visible
        LabelTypeKrd.Visible = visible
        ComboTypeKrd.Visible = visible
    End Sub

    Private Sub TuneCountClientOrNumberClient(isServer As Boolean)
        If isServer Then
            LabelComboBoxCountClientOrNumberClient.Text = "Число всех клиентов (для командного обмена):"
            ToolTip1.SetToolTip(Me.ComboBoxCountClientOrNumberClient, "Число всех клиентов (компьютеров получающих данные)")
        Else
            LabelComboBoxCountClientOrNumberClient.Text = "Номер текущего клиента (для командного обмена):"
            ToolTip1.SetToolTip(Me.ComboBoxCountClientOrNumberClient, "Порядковый номер клиента (из числа компьютеров получающих данные)")
        End If
    End Sub

    ''' <summary>
    ''' Считывание из выделенной рабочей базы имен каналов и занесение их в ComboBox.
    ''' Выделение ранее настроенных имен.
    ''' </summary>
    Private Sub PopulateComboBoxChannelsName()
        If ComboNumberStend.Text = cClient Then Exit Sub

        StandNumber = ComboNumberStend.Text
        imagesCross = ImageListChannel.Images.Count - 1 ' крест и признак проведения инициализации
        SetLastChannelDBase()
        LoadChannels() ' в шапке выбирается номер стенда
        ' можно было реализовать с помощью Dictionary
        listKeyValuePair = New List(Of KeyValuePair(Of String, ComboBox)) From {New KeyValuePair(Of String, ComboBox)(NameVarStartWrite, ComboBoxNameVarStartWrite),
                                                                                New KeyValuePair(Of String, ComboBox)(NameVarStopWrite, ComboBoxNameVarStopWrite),
                                                                                New KeyValuePair(Of String, ComboBox)(NameTBox, ComboBoxChannelTbox),
                                                                                New KeyValuePair(Of String, ComboBox)(NameTx, ComboBoxTxc)}

        For Each itemKeyValuePair As KeyValuePair(Of String, ComboBox) In listKeyValuePair
            AddItems(itemKeyValuePair.Value)
            AddHandler itemKeyValuePair.Value.DrawItem, AddressOf ComboBox_DrawItem
            AddHandler itemKeyValuePair.Value.MeasureItem, AddressOf ComboBox_MeasureItem
            SelectNameOnComboBox(itemKeyValuePair.Value, itemKeyValuePair.Key)
        Next
    End Sub

    ''' <summary>
    ''' Занесение в ComboBox пары имя-индекс иконки.
    ''' </summary>
    ''' <param name="refComboBox"></param>
    Private Sub AddItems(refComboBox As ComboBox)
        refComboBox.Items.Clear()
        refComboBox.Items.Add(New StringIntObject(MissingParameter, imagesCross))

        For I As Integer = 1 To UBound(ParametersType)
            If CBool(InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure)) Then
                refComboBox.Items.Add(New StringIntObject(ParametersType(I).NameParameter, Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)))
            Else
                refComboBox.Items.Add(New StringIntObject(ParametersType(I).NameParameter, 1))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Выделение ранее настроенных имен.
    ''' </summary>
    ''' <param name="refComboBox"></param>
    ''' <param name="inNameChannel"></param>
    Private Sub SelectNameOnComboBox(refComboBox As ComboBox, inNameChannel As String)
        If CheckNameChannel(inNameChannel) Then
            refComboBox.Text = inNameChannel
        Else
            refComboBox.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Проверка корректного имени.
    ''' </summary>
    ''' <param name="inNameChannel"></param>
    ''' <returns></returns>
    Private Function CheckNameChannel(ByVal inNameChannel As String) As Boolean
        For I As Integer = 0 To UBound(ParametersType)
            If ParametersType(I).NameParameter = inNameChannel Then Return True
        Next

        Return False
    End Function

    Private Sub ComboBox_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
        DrawItemComboBox(sender, e, Nothing, ImageListChannel, False)
    End Sub

    Private Sub ComboBox_MeasureItem(sender As Object, e As MeasureItemEventArgs)
        Dim cb As ComboBox = CType(sender, ComboBox)
        e.ItemHeight = cb.ItemHeight - 2
    End Sub
#End Region

    ''' <summary>
    ''' Настройка константантных каналов измерения
    ''' </summary>
    Private Sub PopulateConstantChannels()
        mSettingSelectedParameters = New SettingConstantChannels(PathResourses, Me)
    End Sub

#Region "Обработчики событий контролов"
    Private Sub ComboNumberStend_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboNumberStend.SelectedIndexChanged
        If ButtonWithClient.Checked Then
            pathClientStend = PathChannels
            TextPathClient.Text = pathClientStend
            clients(0) = pathClientStend
            WriteINI(PathOptions, "Client", "Stend" & ComboPathClient.Text, pathClientStend)
        Else
            pathServerStend = PathChannels
            TextPathServer.Text = pathServerStend
            servers(ComboNumberStend.SelectedIndex) = pathServerStend
            ComboPathServer.SelectedIndex = ComboNumberStend.SelectedIndex
            WriteINI(PathOptions, "Server", "Stend" & ComboPathServer.Text, pathServerStend)

            If imagesCross <> 0 Then PopulateComboBoxChannelsName()
        End If
    End Sub

    Private Sub ComboFrequencyCollection_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboFrequencyCollection.SelectedIndexChanged
        SetFrequency(ComboFrequencyCollection.Text)
    End Sub

    ''' <summary>
    ''' Определить частоту сбора
    ''' </summary>
    ''' <param name="inFrequency"></param>
    Private Sub SetFrequency(ByVal inFrequency As String)
        Dim limit As Integer = CInt(65536 / 1800) ' arraysize = 1800
        Dim stepDivide As Integer

        ComboIntervalSnapshot.Items.Clear()

        Select Case inFrequency
            Case "1"
                FrequencyBackground = 1
                DeltaX = 1
                LevelOversampling = 16
                LabelIntervalSnapshotFact.Text = "1800/30"
                TimeFrame = 1800
                RefreshScreen = 1
                RefreshDataToNetwork = 1
                stepDivide = 1
                Exit Select
            Case "2"
                FrequencyBackground = 2
                DeltaX = 0.5
                LevelOversampling = 8
                LabelIntervalSnapshotFact.Text = "900/15"
                TimeFrame = 900
                RefreshScreen = 1
                RefreshDataToNetwork = 1
                stepDivide = 2
                ComboIntervalSnapshot.Items.Add("15") ' при "15" LimitAddExel=1
                Exit Select
            Case "5"
                FrequencyBackground = 5
                DeltaX = 0.2
                LevelOversampling = 4
                LabelIntervalSnapshotFact.Text = "360/6"
                TimeFrame = 360
                RefreshScreen = 1
                RefreshDataToNetwork = 1
                stepDivide = 5
                ComboIntervalSnapshot.Items.AddRange({"6", "12"}) ' при "6" LimitAddExel=1
                Exit Select
            Case "10"
                FrequencyBackground = 10
                DeltaX = 0.1
                LevelOversampling = 4
                LabelIntervalSnapshotFact.Text = "180/3"
                TimeFrame = 180
                RefreshScreen = 1
                RefreshDataToNetwork = 1
                stepDivide = 10
                ComboIntervalSnapshot.Items.AddRange({"3", "6", "9", "15"}) ' при "3" LimitAddExel=1
                Exit Select
            Case "20"
                FrequencyBackground = 20
                DeltaX = 0.05
                LevelOversampling = 2
                LabelIntervalSnapshotFact.Text = "90/1.5"
                TimeFrame = 90
                RefreshScreen = 2
                RefreshDataToNetwork = 1
                stepDivide = 10
                ComboIntervalSnapshot.Items.AddRange({"3", "6", "9"})
                Exit Select
            Case "50"
                FrequencyBackground = 50
                DeltaX = 0.02
                LevelOversampling = 1 '4
                LabelIntervalSnapshotFact.Text = "36/0.6"
                TimeFrame = 36
                RefreshScreen = 5
                RefreshDataToNetwork = 1
                stepDivide = 15
                Exit Select
            Case "100"
                FrequencyBackground = 100
                DeltaX = 0.01
                LevelOversampling = 1
                LabelIntervalSnapshotFact.Text = "18/0.3"
                TimeFrame = 18
                RefreshScreen = 10
                RefreshDataToNetwork = 1
                stepDivide = 10
                Exit Select
        End Select

        'If FrequencyBackground = 100 Then
        '    RefreshScreen2 = RefreshScreen
        'Else
        '    RefreshScreen2 = Math.Round(RefreshScreen / 2 + 0.5)
        'End If
        RefreshScreen2 = RefreshScreen

        ' 10 секунд будет шлейф для углов
        Dynamics = CShort(10 / DeltaX / RefreshScreen)
        LabelDiscreditFact.Text = CStr(LevelOversampling)
        TextFrequencySamplingChannel.Text = CStr(FrequencyBackground * LevelOversampling)

        For I = 0 To limit Step stepDivide
            If I = 0 Then Continue For
            ComboIntervalSnapshot.Items.Add((TimeFrame * I) / 60)
        Next

        If inFrequency = "100" Then
            ComboIntervalSnapshot.SelectedIndex = 1 ' сделать 6 минут по умолчанию (3,6,9 минут при StepDivide = 10)
        Else
            ComboIntervalSnapshot.SelectedIndex = 0 ' cmbДлительностьКадра.Items.Count - 1
        End If

        Application.DoEvents()
        Beep()
    End Sub

    Private Sub ComboIntervalSnapshot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboIntervalSnapshot.SelectedIndexChanged
        LimitAddExel = CInt(Integer.Parse(ComboIntervalSnapshot.Text) * 60 / TimeFrame)
    End Sub

    ''' <summary>
    ''' Проверить длительность кадра
    ''' </summary>
    ''' <param name="inTimeOfSnapshot"></param>
    Private Sub SetIntervalSnapshot(inTimeOfSnapshot As String)
        For I As Integer = 0 To ComboIntervalSnapshot.Items.Count - 1
            If CStr(ComboIntervalSnapshot.Items(I)) = inTimeOfSnapshot Then
                ComboIntervalSnapshot.SelectedIndex = I
                Exit For
            End If
        Next
    End Sub

    Private Sub FormSetting_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        'If e.KeyCode = Keys.Right Then NextRecord()
        'If e.KeyCode = Keys.Left Then PreviousRecord()
        'If e.KeyCode = Keys.Home Then FirstRecord()
        'If e.KeyCode = Keys.End Then LastRecord()

        ' Прибегнуть к ctrl+tab, чтобы двигаться к следующей закладке
        If e.KeyCode = Keys.Tab AndAlso e.Modifiers = Keys.Control Then
            Dim selectedIndex As Integer = TabOption.SelectedIndex + 1

            If selectedIndex >= TabOption.TabCount Then
                ' Последняя закладка, так что надо перейти к закладке 1
                TabOption.SelectedTab = TabOption.TabPages(0)
            Else
                ' Увеличить закладку
                TabOption.SelectedTab = TabOption.TabPages(selectedIndex)
            End If
        End If
    End Sub

    Private Sub ComboEngine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboEngine.SelectedIndexChanged
        TabPageStartStop.Visible = True
        ComboRegime.Items.Clear()

        ComboRegime.Items.Add(B)
        ComboRegime.Items.Add(UB)
        If ComboEngine.Text = cEngine99A Then 'изделие 99А
            ComboRegime.Items.Add(O_R)
        End If

        ComboRegime.SelectedIndex = 0
    End Sub
#End Region

#Region "Validating"
    Private Sub TextNumberProduct_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextNumberProduct.Validating
        ValidatingTextBoxNumeral(TextNumberProduct)
    End Sub

    Private Sub TextTbox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextTbox.Validating
        ValidatingTextBoxNumeral(TextTbox)
    End Sub

    Private Sub TextBoxValueStopWrite_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextValueStopWrite.Validating
        ValidatingTextBoxNumeral(TextValueStopWrite)
    End Sub

    Private Sub TextBoxWaitStopWrite_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextWaitStopWrite.Validating
        ValidatingTextBoxNumeral(TextWaitStopWrite)
    End Sub

    Private Sub TextBoxWaitStartWrite_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextWaitStartWrite.Validating
        ValidatingTextBoxNumeral(TextWaitStartWrite)
    End Sub
    ''' <summary>
    ''' Вывести знак предупреждения рядом с контролом
    ''' </summary>
    ''' <param name="inTextBox"></param>
    Private Sub ValidatingTextBoxNumeral(ByRef inTextBox As TextBox)
        If Not IsNumeric(inTextBox.Text) Then
            ErrorProvider1.SetError(inTextBox, "Это поле должно быть числом!")
        Else
            ErrorProvider1.SetError(inTextBox, "")
        End If
    End Sub

    Private Sub TextNumberProduct_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles TextNumberProduct.Leave
        ValidatingTextBoxEmpty(TextNumberProduct)
    End Sub

    Private Sub TextTbox_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TextTbox.Leave
        ValidatingTextBoxEmpty(TextTbox)
    End Sub

    Private Sub TextBoxValueStopWrite_Leave(sender As Object, e As EventArgs) Handles TextValueStopWrite.Leave
        ValidatingTextBoxEmpty(TextValueStopWrite)
    End Sub

    Private Sub TextBoxWaitStopWrite_Leave(sender As Object, e As EventArgs) Handles TextWaitStopWrite.Leave
        ValidatingTextBoxEmpty(TextWaitStopWrite)
    End Sub

    Private Sub TextBoxWaitStartWrite_Leave(sender As Object, e As EventArgs) Handles TextWaitStartWrite.Leave
        ValidatingTextBoxEmpty(TextWaitStartWrite)
    End Sub

    ''' <summary>
    ''' Проверка контракта ввода.
    ''' Предупреждение выводится даже ранее чем проверка в событии Validating.
    ''' </summary>
    ''' <param name="inTextBox"></param>
    Private Sub ValidatingTextBoxEmpty(ByRef inTextBox As TextBox)
        Dim dblTemp As Double

        ' Если поле не заполнено, сообщить пользователю
        If Trim(inTextBox.Text) = vbNullString Then
            Const caption As String = "Ввод шапки"
            Const text As String = "Это поле не может быть пустым!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            inTextBox.Focus()
            Exit Sub
        End If

        Try
            'If Not IsNumeric(txtПолнК1.Text)
            dblTemp = CDbl(inTextBox.Text)
        Catch ex As Exception
            Const caption As String = "Ввод шапки"
            Const text As String = "Это поле должно быть числом!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{caption}> {text}")
            inTextBox.Focus()
        End Try
    End Sub

#End Region

#Region "Настройка Путей"
    Private Sub ButtonPathServerExplorer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonPathServerExplorer.Click
        With OpenFileDialogPuth
            .FileName = vbNullString
            .Title = "Поиск рабочей базы данных на компьютере-Сервере стенда №" & ComboPathServer.Text
            .InitialDirectory = "c:\"
            .CheckFileExists = True
            .DefaultExt = "mdb"
            .RestoreDirectory = True
            ' установить флаг атрибутов
            .Filter = "Channels (*.mdb)|*.mdb"

            If .ShowDialog = DialogResult.OK Then
                If Len(.FileName) = 0 Then Exit Sub

                pathServerStend = .FileName
                TextPathServer.Text = pathServerStend
                servers(ComboPathServer.SelectedIndex) = pathServerStend
                WriteINI(PathOptions, "Server", "Stend" & ComboPathServer.Text, pathServerStend)
            End If
        End With
    End Sub

    Private Sub ButtonPathClientExplorer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonPathClientExplorer.Click
        With OpenFileDialogPuth
            .FileName = vbNullString
            .Title = "Поиск рабочей базы данных на компьютере-Клиенте стенда №" & ComboPathClient.Text
            .InitialDirectory = "c:\"
            .CheckFileExists = True
            .DefaultExt = "mdb"
            .RestoreDirectory = True
            ' установить флаг атрибутов
            .Filter = "Channels (*.mdb)|*.mdb"

            If .ShowDialog = DialogResult.OK Then
                If Len(.FileName) = 0 Then Exit Sub

                pathClientStend = .FileName
                TextPathClient.Text = pathClientStend
                clients(ComboPathClient.SelectedIndex) = pathClientStend
                WriteINI(PathOptions, "Client", "Stend" & ComboPathClient.Text, pathClientStend)
            End If
        End With
    End Sub

    Private Sub ComboPathServer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboPathServer.SelectedIndexChanged
        TextPathServer.Text = servers(ComboPathServer.SelectedIndex)
        pathServerStend = TextPathServer.Text
    End Sub

    Private Sub ComboPathClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboPathClient.SelectedIndexChanged
        TextPathClient.Text = clients(ComboPathClient.SelectedIndex)
        pathClientStend = TextPathClient.Text
    End Sub
#End Region

#Region "Компенсация Холодного спая"
    ''' <summary>
    ''' Записать компенсацию ХС
    ''' </summary>
    Private Sub SaveCoefficientBringingTBoxing()
        Dim I As Integer
        Dim numberParameterXC As Integer ' номер Параметра ХС
        Dim numberParameterTbox As Integer ' номер Параметра Тбокса
        Dim acquireTempeatureXC As Double ' температура ХС

        If IsWorkWithController Then
            numberParameterXC = -1
            numberParameterTbox = -1
            ' здесь надо считать из базы параметр "Т хс" измерения карандаша компенсации холодного спая
            ' и измерить его значение, а затем записать в поле смещение для параметров с компенсацией Х.С.
            For I = 1 To UBound(ParametersType)
                If ParametersType(I).NameParameter = NameTx Then ' "Т хс"
                    numberParameterXC = I
                    Exit For
                End If
            Next

            For I = 1 To UBound(ParametersType)
                If ParametersType(I).NameParameter = NameTBox Then ' "tбокса"
                    numberParameterTbox = I
                    Exit For
                End If
            Next

            Try
                If numberParameterXC <> -1 Then acquireTempeatureXC = Acquire(numberParameterXC)
                If numberParameterTbox <> -1 Then TemperatureOfBox = CDbl(Acquire(numberParameterTbox))
                TextTbox.Text = CStr(TemperatureOfBox)
            Catch ex As Exception
                Const caption As String = "Опрос каналов Тхс и Тбокса"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try

            If numberParameterXC <> -1 Then
                If acquireTempeatureXC < -10 OrElse acquireTempeatureXC > 40 Then
                    Const caption As String = "Опрос температур"
                    Dim text As String = $"Температура холодного спая {acquireTempeatureXC:F} не в норме!{vbCrLf}Проверьте параметр <Т хс>."
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Else
                    TemperatureTxc = acquireTempeatureXC

                    Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
                        cn.Open()
                        Using odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter($"SELECT * FROM [{ChannelLast}] WHERE ([КомпенсацияХС] = True)", cn)
                            Using dtDataTable As New DataTable
                                odaDataAdapter.Fill(dtDataTable)

                                If dtDataTable.Rows.Count <> 0 Then
                                    For I = 0 To dtDataTable.Rows.Count - 1
                                        dtDataTable.Rows(I)("Смещение") = acquireTempeatureXC
                                    Next

                                    Dim cb As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
                                    odaDataAdapter.Update(dtDataTable)
                                End If
                            End Using
                        End Using
                    End Using
                End If
            End If

            LoadChannels() ' загрузка после записи ХС
            isCompensationReferenceJunction = True
        End If
    End Sub

    Friend Function Acquire(ByVal indxeChannel As Integer) As Double
        Dim samplesBuff() As Double = Nothing
        Dim average As Double
        Dim reader As AnalogSingleChannelReader

        If IsTaskRunning = False Then
            Try
                IsTaskRunning = True
                If Not DAQmxTask Is Nothing Then
                    DAQmxTask.Dispose()
                    DAQmxTask = Nothing
                    'Else
                    '    myTask = New Task("aiTask")
                End If
                DAQmxTask = New Task("aiTask")

                ' соэдать виртуальный канал
                CreateAiChannel(ParametersType(indxeChannel).NumberChannel.ToString, ParametersType(indxeChannel).NumberDevice.ToString, ParametersType(indxeChannel).NumberModuleChassis.ToString, ParametersType(indxeChannel).NumberChannelModule.ToString,
                Convert.ToDouble(ParametersType(indxeChannel).LowerMeasure), Convert.ToDouble(ParametersType(indxeChannel).UpperMeasure), ParametersType(indxeChannel).TypeConnection, ParametersType(indxeChannel).SignalType)

                DAQmxTask.Timing.ConfigureSampleClock("", 10000,
                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples)

                ' проверить задачу
                DAQmxTask.Control(TaskAction.Verify)

                'analogInReader = New AnalogMultiChannelReader(myTask.Stream)
                'analogInReader.SynchronizingObject = Me

                'reader.SynchronizingObject = Me
                reader = New AnalogSingleChannelReader(DAQmxTask.Stream) With {
                    .SynchronizeCallbacks = True
                }

                'SingleAnalogCallback = New AsyncCallback(AddressOf SingleAnalogInCallback)
                'analogInReader.BeginReadMultiSample(1000, SingleAnalogCallback, Nothing)

                samplesBuff = reader.ReadMultiSample(1000)
                'data = analogInReader.ReadMultiSample(1000)
                IsTaskRunning = False
            Catch ex As DaqException
                Const caption As String = "Acquire"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                IsTaskRunning = False
                DAQmxTask.Dispose()
            End Try
        End If

        average = Statistics.Mean(samplesBuff) ' здесь можно применить фильтр
        ' расчет физики
        Return PhysicalValue(CShort(indxeChannel), average)
    End Function

    Private Sub ButtonFindChannelStartWrite_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelStartWrite.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxNameVarStartWrite)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub ButtonFindChannelStopWrite_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelStopWrite.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxNameVarStopWrite)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub ButtonFindChannelTxc_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelTxc.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxTxc)
        mSearchChannel.SelectChannel()
    End Sub

    Private Sub ButtonFindChannelTbox_Click(sender As Object, e As EventArgs) Handles ButtonFindChannelTbox.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxChannelTbox)
        mSearchChannel.SelectChannel()
    End Sub
#End Region

    ''' <summary>
    ''' Обработчик события добавления нового константного параметра должен вызываться первым.
    ''' Далее вызывается событие DataGridViewConstantChannels_RowsAdded. 
    ''' При выносе подключения данного события в mSettingSelectedParameters нарушается последовательность 
    ''' вызова обработчика события DataGridViewConstantChannels_RowsAdded.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BindingNavigatorAddNewItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorAddNewItem.Click
        mSettingSelectedParameters.BindingNavigatorAddNewItem()
    End Sub
End Class