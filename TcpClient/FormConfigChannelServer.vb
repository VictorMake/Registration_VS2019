Option Infer On

Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.Threading
Imports System.Xml.Linq
Imports MathematicalLibrary

'   1.	В опциях флаг, использования ЛМЗ – по нему в шапке убираются всё лишнее
'   2.	В шапке выбор TCP клиента от ИВК – обычный выбор стенда (по нему из опций считывается путь к .XML файлу сервера)
'   3.	Сделать в перечислении типа испытания новый тип клиент ИВК
'   4.	Загрузка основной формы с модификацией меню и проверкой соединения сервера, создаётся Socket, подписаться на событие таймера. 
'   В параметрах аргумента события передаётся массив (не слишком ли это затормозит, как вариант массив будет глобальный, а по событию запуск 
'   аналога CWAI1_AcquiredData с убранными полиномами)
'   5.	При загрузке каналы уже сконфигурированы в таблице Channels (в случае отсутствия канала Сервер выдаст 0 и ни чего страшного не произойдёт)
'   6.	Пункт Меню Конфигурация загружает форму TcpClientForm (значит, если было установлено соединение и был запуск, то его остановить – или кнопка Стоп)
'   7.	В случае успешного создания Socket по кнопке Пуск запускается таймер сбора
'   8.	Предусмотреть вывод сообщений об ошибках на панель (может подумать о протоколировании в  журнале сообщений)
'   9.	Как быть с обязательными параметрами? Тбокса, Тхс
'   10.	Перечислитель назначенной формы запуска (DAQ стоп) 
'   11.	Для сохранения настроек разносов мин мах каналов при добавлении в TcpClientForm сделать сверку каналами сохранёнными до этого 
'   в Channels – Channel_Номер_стеда ( может сделать считывание в типизированную таблицу и быстрым поиском находить нужный канал а потом при выходе из формы 
'   с выбранной конфигурацией настройки каналов и сами каналы перезаписываются в Channels – Channel_Номер_стеда). Так наверно было в OPC АИ222.


'   *******1 таблица**********
'   id			        id
'   Номер			    NumberCh
'   Имя			        Name
'   Pin			        Pin
'   Экр. Имя		    Scr_Name
'   Вкл/Выкл		    State
'   Экр. Ед.Изм.	    Scr_EdIzm
'   Физ. Ед.Изм.	    Ch_Unit
'   Группа			    GroupCh

'   *******2 таблица**********
'   id			        id
'   keyConfig		    keyConfig
'   Номер			    НомерПараметра
'   Имя Канала		    Name
'   Экр. Имя		    НаименованиеПараметра
'   Ед. Измерения		ЕдиницаИзмерения
'   Допуск Мин		    ДопускМинимум
'   Допуск Макс		    ДопускМаксимум
'   Разнос Y мин		РазносУмин
'   Разнос Y макс		РазносУмакс
'   Авар. Значение Мин 	АварийноеЗначениеМин
'   Авар. Значение Макс	АварийноеЗначениеМакс
'   Группа			    GroupCh

' сделать обработки ошибок при приёме и проанализировать прервано соединение или ошибка произошла при передачи или приёме данных
' если соединение не разорвано то разорвать и попытаться заново установить соединение
' если Пинг не проходит при повторном соединении - значит компьютер выключен или связь прервана

Friend Class FormConfigChannelServer
    ''' <summary>
    ''' Модификация Изделия
    ''' </summary>
    ''' <returns></returns>
    Private Property EngineModification As String ' = "117" 'запись извне
    ''' <summary>
    ''' Номер Стенда Сменен
    ''' </summary>
    Private IsNumberStendchanged As Boolean

#Region "Таблицы и источники данных"
    ' это хранилища каналов
    Private mManagerChannelsInput As ManagerChannelsInput
    Private mManagerChannelsOutput As ManagerChannelsOutput
    Private mInputChannelsBindingList As New ChannelsInputBindingList
    Private keyConfig As Integer ' последняя загруженная конфигурация
    Private connection As OleDbConnection
    Private shadowDataTable As DataTable
    Private WithEvents ContexCurrencyManager As CurrencyManager
    Private tabPosition As Integer
    Const conNameTableChannel As String = "Channel" ' имя таблицы
    Private isDirty As Boolean
#End Region

    Private pathServerCfg_xml As String() ' массив путей к конфигурационным файлам серверов
    'Private mSyncSocketClient As ConnectionInfoClient
    ' делегат для функции обратного вызова из SyncSocketClient для отображения сообщений
    'Delegate Sub AddListItem(message As String)
    'Public myDelegate As AddListItem

    Private Units As String() ' ЕдИзмерения  для заполнения колонки таблицы
    Private isFormLoaded As Boolean = False ' для предотвращения срабатывания из события контрола
    Private isFormClosed As Boolean ' 2 раза не показывать при выходе
    Private lvSelectedIndices, lvPreviousSelectedIndices As Integer ' позиции для перестановки строк

    ' при установке фильтра с именами параметров с подчёркиванием или с каналами с атрибутом Off или с CCD6-8
    ' при повторном считывании их не показывать
    Private ReadOnly CCDNamesToExclud As String() = {"ССД5", "ССД6"}
    Private Const conEngineModification117 As String = "117" ' для сравненения не менять
    Private changeNameChannelList As List(Of String)

#Region "Drag_Drop"
    ''' <summary>
    ''' для проверки нахождения курсора в области формы во время перетаскивания
    ''' </summary>
    Private screenOffset As Point
    ''' <summary>
    ''' для определения области курсора мыши
    ''' </summary>
    Private dragBoxFromMouseDown As Rectangle
    ''' <summary>
    ''' отслеживание первоначальной позиции выделенной строки таблицы при перетаскивании
    ''' </summary>
    Private indexOfItemUnderMouseToDrag As Integer
    ''' <summary>
    ''' отслеживание прошлой позиции выделенной строки таблицы при перетаскивании
    ''' </summary>
    Private indexOfItemUnderMouseToDragTemp As Integer
#End Region

    Private Class Config
        Public ReadOnly Property KeyConfig() As Integer
        Public ReadOnly Property NameConfiguration() As String

        Public Sub New(ByVal inKeyConfig As Integer, ByVal inNameConfiguration As String)
            KeyConfig = inKeyConfig
            NameConfiguration = inNameConfiguration
        End Sub
    End Class

    Private configs As List(Of Config)

    Private dictModuleRegistration As Dictionary(Of String, ModuleRegistration)
    Private Class ModuleRegistration
        Public Property NameModule As String
        Public Property CountChannels As Integer

        Public Sub New(inNameModule As String)
            NameModule = inNameModule
            CountChannels += 1
        End Sub

        Public Sub AddChannels()
            CountChannels += 1
        End Sub
    End Class

    Private lastModuleNumber As Integer
    Private mParentForm As FormMain
    Private ReadOnly StendNumbers As New List(Of String)

    ''' <summary>
    ''' глобальная, для того чтобы при повторных загрузках окна не обращаться к Серверу
    ''' </summary>
    ''' <remarks></remarks>
    Private IsFirstStatTcpClient As Boolean = True

    ''' <summary>
    ''' Отсев Каналов
    ''' </summary>
    ''' <remarks></remarks>
    Private IsSiftingChannels As Boolean = False

    Public Sub New(ByVal formParent As FormMain)
        MyBase.New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        mParentForm = formParent
    End Sub

    Private Sub FormConfigChannelServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ''TODO: данная строка кода позволяет загрузить данные в таблицу "ChannelsDataSet1.Channel". При необходимости она может быть перемещена или удалена.
        'Me.ChannelTableAdapter.Fill(Me.ChannelsDataSet.Channel)

        LoadStandOptions()

        Dim unitsTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter =
            New Channels_cfg_lmzDataSetTableAdapters.ЕдиницаИзмеренияTableAdapter() With {.ClearBeforeFill = True,
                                                                                          .Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))}
        unitsTableAdapter.Fill(ChannelsDataSet.ЕдиницаИзмерения)

        Dim tableЕдиницаИзмерения As Channels_cfg_lmzDataSet.ЕдиницаИзмеренияDataTable = ChannelsDataSet.ЕдиницаИзмерения
        'ReDim_Units(tableЕдиницаИзмерения.Rows.Count)
        Re.Dim(Units, tableЕдиницаИзмерения.Rows.Count)
        Units(0) = ""

        Dim I As Integer = 1
        For Each row As Channels_cfg_lmzDataSet.ЕдиницаИзмеренияRow In tableЕдиницаИзмерения.Rows
            Units(I) = row.ЕдиницаИзмерения
            I += 1
        Next

        ' заполнить все ячейки типа ComboBoxColumn данного столбца даныыми
        Using tempComboBoxColumnЕдИзмВходная As DataGridViewComboBoxColumn = CType(DataGridViewOutputChannel.Columns("ЕдиницаИзмеренияDataGridViewTextBoxColumn"), DataGridViewComboBoxColumn)
            tempComboBoxColumnЕдИзмВходная.Items.AddRange(Units)
        End Using

        TSButtonRemoveChannels.Checked = IsSiftingChannels
        isFormLoaded = True
        ConfigureByStend()
        DataGridViewInputChannel_Resize(Me, Nothing)
    End Sub

    Private Sub FormConfigChannelServer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)

        Dim mfrmMessageBox As FormMessageBox = GetFormMessageBox("Подождите, идёт применение настроек", "Запись конфигурации")
        isFormClosed = True

        If isDirty Then ButtonSaveConfiguration_Click(ButtonSaveConfiguration, Nothing)

        UpdateTableRegime()
        SaveStandOptions()
        mfrmMessageBox.Close()

        ' произвести заново настройку каналов
        'UpdateAllWindowsWithServerChannels()
        mParentForm = Nothing
    End Sub

    'Private Sub OK_Button_Click(sender As System.Object, e As System.EventArgs) Handles OK_Button.Click
    '    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    '    Me.Close()
    'End Sub

    'Private Sub Cancel_ButtonDialog_Click(sender As System.Object, e As System.EventArgs) Handles Cancel_ButtonDialog.Click
    '    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    '    Me.Close()
    'End Sub

    Private Sub DataGridViewInputChannel_Resize(sender As Object, e As EventArgs) Handles DataGridViewInputChannel.Resize
        PanelDrdFindChannel.Bounds = DataGridViewInputChannel.Bounds
    End Sub

    ''' <summary>
    ''' Настроить В Зависимости От Стенда
    ''' Вызывается при смене номера стенда из списка
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigureByStend()
        If isFormLoaded = True Then
            EnableDisable(False)

            If CheckExistServerCfglmzXml(PathServerCfglmzXml) Then
                Dim mfrmMessageBox As FormMessageBox = GetFormMessageBox("Подождите, идёт загрузка настроек", "Применить настройки стенда")

                If IsFirstStatTcpClient OrElse IsNumberStendchanged Then
                    SaveTableChannels_cfg_lmz(GetTableServerChannelsConfigurationXML(PathServerCfglmzXml))
                    IsFirstStatTcpClient = False
                Else
                    PopulateTableChannels_cfg_lmz()
                End If

                PopulateChannelsNBaseDataSet(StandNumber)
                LoadManagerChannelsInput()
                LoadConfigurationList()
                RestoreSelectedConfiguration() ' там конфигурируется адаптер
                InitializeShadowDataTable()
                PopulateTimeChannelTable()
                EnableDisable(True)
                mfrmMessageBox.Close()
            Else
                GroupBoxСтендКонфигСервер.Enabled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Изменить доступ к элементам управления в зависимости от запуска или остановка сбора
    ''' </summary>
    ''' <param name="inEnabled"></param>
    ''' <remarks></remarks>
    Private Sub EnableDisable(ByVal inEnabled As Boolean)
        TSButtonRemoveChannels.Enabled = inEnabled
        GroupBoxСтендКонфигСервер.Enabled = inEnabled
        TableLayoutPanelBotton.Enabled = inEnabled
        TableLayoutPanelBottonCFG.Enabled = inEnabled
        PanelFindChannel.Enabled = inEnabled
    End Sub

    '''' <summary>
    '''' Установка типа значений получаемых от сервера
    '''' </summary>
    '''' <param name="View"></param>
    '''' <remarks></remarks>
    'Private Sub SetView(ByVal View As TypeValues)
    '    ' Определить, какой пункт меню должен быть отмечен
    '    Dim menuItemToCheck As ToolStripMenuItem = Nothing
    '    Select Case View
    '        Case TypeValues.СырыеДанныеСвойства
    '            menuItemToCheck = RawToolStripMenuItem
    '            ListViewToolStripButton.Text = "Тип значений - сырые"
    '        Case TypeValues.РасчетныеЗначения
    '            menuItemToCheck = ValueToolStripMenuItem
    '            ListViewToolStripButton.Text = "Тип значений - расчётные"
    '            'Case Else
    '            '    Debug.Fail("Unexpected View")
    '            '    View = View.Details
    '            '    MenuItemToCheck = DetailsToolStripMenuItem
    '    End Select

    '    ' В меню "Представления" выбрать нужный пункт меню и отменить выбор остальных пунктов
    '    For Each tsMenuItem As ToolStripMenuItem In ListViewToolStripButton.DropDownItems
    '        If tsMenuItem Is menuItemToCheck Then
    '            tsMenuItem.Checked = True
    '        Else
    '            tsMenuItem.Checked = False
    '        End If
    '    Next

    '    '' В конце установить запрошенное представление
    '    'If mSyncSocketClient IsNot Nothing Then
    '    '    If mSyncSocketClient.СоединениеУстановлено Then
    '    '        If mSyncSocketClient.IsStartAcquisition Then
    '    '            mSyncSocketClient.StopAcquisition()
    '    '            mSyncSocketClient.ТипЗапрашиваемыхДанных = View
    '    '            mSyncSocketClient.Настроить()
    '    '            mSyncSocketClient.StartAcquisition()
    '    '        Else
    '    '            mSyncSocketClient.ТипЗапрашиваемыхДанных = View
    '    '        End If
    '    '    End If
    '    'End If
    'End Sub

#Region "События элементов управления"
    Private Sub TSButtonRemoveChannels_CheckedChanged(sender As Object, e As EventArgs) Handles TSButtonRemoveChannels.CheckedChanged
        If TSButtonRemoveChannels.Checked Then
            TSButtonRemoveChannels.Text = "Отсев каналов включен"
        Else
            TSButtonRemoveChannels.Text = "Отсев каналов выключен"
        End If
    End Sub

    Private Sub TSButtonRemoveChannels_Click(sender As Object, e As EventArgs) Handles TSButtonRemoveChannels.Click
        If IsHandleCreated Then
            IsSiftingChannels = TSButtonRemoveChannels.Checked
            LoadManagerChannelsInput()
            ' ЗагрузитьСпискиКонфигурации()' здесь уже список загружен
            RestoreSelectedConfiguration() ' там конфигурируется адаптер
            ' здесь уже таблица инициализирована
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(TSButtonRemoveChannels_Click)}> {TSButtonRemoveChannels.Text}")
            ShowMessageOnStatusPanel(String.Format("Отсев каналов (с подчёркиванием) для отображения {0}", If(TSButtonRemoveChannels.Checked, "включён", "выключен")))
        End If
    End Sub

    'Private Sub StartTSButton_Click(sender As Object, e As EventArgs) Handles StartTSButton.Click
    '    Dim I As Integer = 1

    '    StopTSButton.Enabled = True
    '    StartTSButton.Enabled = False
    '    ToolStripStatusLabel.Text = "Запуск сбора " & If(ValueToolStripMenuItem.Checked, "расчетных значений", "сырых данных свойств") & " c " & If(TSButtonОтсевКаналов.Checked, "включённым", "выключённым") & " отсевом каналов"
    '    RegistrationEventLog.EventLog_MSG_USER_ACTION(ToolStripStatusLabel.Text)
    '    ВключитьВыключить(False)
    '    DoubleValueListG = New DoubleValueList
    '    StrTimeValueListG = New StrTimeValueList
    '    ' было
    '    'mSyncSocketClient.ЧастотаСбора = частотаФонового
    '    'mSyncSocketClient.Настроить()

    '    'If mSyncSocketClient.AcquisitionDoubleValue IsNot Nothing Then
    '    '    For Each itemDouble As Double In mSyncSocketClient.AcquisitionDoubleValue
    '    '        DoubleValueListG.Add(New DoubleValue(I, 0))
    '    '        I += 1
    '    '    Next
    '    'End If

    '    'I = 1
    '    'If mSyncSocketClient.AcquisitionStrTimeValue IsNot Nothing Then
    '    '    For Each ItemString As String In mSyncSocketClient.AcquisitionStrTimeValue
    '    '        StrTimeValueListG.Add(New StrTimeValue(I, 0))
    '    '        I += 1
    '    '    Next
    '    'End If

    '    DGVDoubleValue.DataSource = DoubleValueListG
    '    DGVStrTimeValue.DataSource = StrTimeValueListG
    '    DGVDoubleValue.Columns(0).Width = 50
    '    DGVStrTimeValue.Columns(0).Width = 50
    '    DGVStrTimeValue.Columns(1).Width = 150

    '    'было
    '    'mSyncSocketClient.StartAcquisition() ' идёт последним - там настройка размерностей

    '    fMainForm.StartStopConnectionWithServer(True)

    'End Sub

    'Private Sub StopTSButton_Click(sender As Object, e As EventArgs) Handles StopTSButton.Click
    '    ' было
    '    'If mSyncSocketClient.IsStartAcquisition Then
    '    '    mSyncSocketClient.StopAcquisition()
    '    'End If

    '    fMainForm.StartStopConnectionWithServer(False)

    '    StartTSButton.Enabled = True
    '    StopTSButton.Enabled = False
    '    ToolStripStatusLabel.Text = "Остановка сбора"
    '    RegistrationEventLog.EventLog_MSG_USER_ACTION(ToolStripStatusLabel.Text)
    '    ВключитьВыключить(True)
    'End Sub

    'Private Sub RawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawToolStripMenuItem.Click
    '    SetView(TypeValues.СырыеДанныеСвойства)
    'End Sub

    'Private Sub ValueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ValueToolStripMenuItem.Click
    '    SetView(TypeValues.РасчетныеЗначения)
    'End Sub

#End Region

#Region "Рокировка записей"
    ''' <summary>
    ''' По имени из входной таблицы выдать экземпляр ChannelOutput
    ''' </summary>
    ''' <param name="NameChannelsInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChannelsOutput(nameChannelsInput As String) As ChannelOutput
        Dim newChannelOutput As New ChannelOutput
        ' найти канал по Имени в первом отсоединённом наборе и вытянуть из него доступные данные, а остальные дополнить
        'For Each rowChannel As Channels_cfg_lmzDataSet.ChannelRow In Me.ChannelsDataSet.Channel.Rows
        'rowChannel = ChannelTableAdapter.FindByName(Name)

        'Dim rowChannel As Channels_cfg_lmzDataSet.ChannelRow = Me.ChannelsDataSet.Channel.Rows.Find(IdChannelsInput)
        Dim rowChannel As ChannelInput = mManagerChannelsInput.CollectionsChannels(nameChannelsInput)

        'clsChannelOutput.id = rowChannel.id
        newChannelOutput.KeyConfig = keyConfig
        newChannelOutput.NumberParameter = OutputChannelsBindingList.Count + 1 'rowChannel.НомерПараметра
        newChannelOutput.Name = rowChannel.Name
        'clsChannelOutput.НаименованиеПараметра = ReplaceNameSlesh(rowChannel.Scr_Name)
        Dim scr_Name As String = ReplaceNameSlesh(rowChannel.ScrName)
        CheckScreenName(scr_Name)
        newChannelOutput.NameParameter = scr_Name

        'If Not IsDBNull(rowChannel("ЕдиницаИзмерения")) Then rowChannel.ЕдиницаИзмерения()
        newChannelOutput.UnitOfMeasure = "Деления"

        newChannelOutput.LowerLimit = 0 'rowChannel.ДопускМинимум
        newChannelOutput.UpperLimit = 100 'rowChannel.ДопускМаксимум

        newChannelOutput.RangeYmin = 0 'rowChannel.РазносУмин
        newChannelOutput.RangeYmax = 100 'rowChannel.РазносУмакс

        newChannelOutput.AlarmValueMin = 0 ' rowChannel.АварийноеЗначениеМин
        newChannelOutput.AlarmValueMax = 0 'rowChannel.АварийноеЗначениеМакс

        newChannelOutput.GroupCh = rowChannel.GroupCh
        newChannelOutput.Pin = rowChannel.Pin
        newChannelOutput.NumberFormula = 2 ' забыл внести поле при создании базы, поэтому здесь заплатка
        RestoreOneChannelOutputMembersFromChannelsNBaseDataSet(newChannelOutput)

        Return newChannelOutput
    End Function

    ''' <summary>
    ''' Проверка Экранного Имени
    ''' Дублирующая процедура (наверно в ManagerChannelsInput та же функция всё уже сделает)
    ''' </summary>
    ''' <param name="varScr_Name"></param>
    ''' <remarks></remarks>
    Private Sub CheckScreenName(ByRef varScr_Name As String)
        For Each ch As ChannelOutput In OutputChannelsBindingList
            If ch.NameParameter.ToUpper = varScr_Name.ToUpper Then
                varScr_Name &= "1"
                CheckScreenName(varScr_Name)
                Exit Sub
            End If
        Next
    End Sub

    ''' <summary>
    ''' Заменить \ на символ _
    ''' </summary>
    ''' <param name="Scr_Name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReplaceNameSlesh(Scr_Name As String) As String
        If Scr_Name.Contains("\") Then
            Return Scr_Name.Replace("\", "_")
        Else
            Return Scr_Name
        End If
    End Function

    ''' <summary>
    ''' Найти канал по имени в первом отсоединённом наборе 
    ''' </summary>
    ''' <param name="NameChannelsInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChannelsInput(nameChannelsInput As String) As ChannelInput
        Return mManagerChannelsInput.CollectionsChannels(nameChannelsInput)
    End Function

    Private Sub ButtonAddChannel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddChannel.Click
        Dim selectRow As DataGridViewRow
        Dim selectRows As DataGridViewSelectedRowCollection
        Dim lastSelectedRows As Integer

        If PanelDrdFindChannel.Visible Then
            If DataGridViewFindChannel.SelectedRows.Count > 0 Then
                ButtonСhoiceConfirmed.PerformClick()
            Else
                Exit Sub
            End If
        End If

        ' цикл по листу1 в поисках выделенного
        selectRows = DataGridViewInputChannel.SelectedRows

        For I As Integer = selectRows.Count - 1 To 0 Step -1
            'For Each SelectRow In SelectRows
            selectRow = selectRows(I)

            ' здесь поиск в коллекции InputChannels  имени   и добавление в OutputChannels
            'Dim clsChannelInput As ChannelInput = m_InputChannelsBindingList.Item(SelectRow.Index)
            OutputChannelsBindingList.Add(GetChannelsOutput(mInputChannelsBindingList.Item(selectRow.Index).Name))

            lastSelectedRows = selectRow.Index
            'Next
        Next
        ' не работает
        'grdOutputChannel.Rows.Add(SelectRows)

        CastingInputOutputChannelsBindingList()
        DataGridViewInputChannel.Focus()

        For Each selectRow In DataGridViewInputChannel.SelectedRows
            selectRow.Selected = False
        Next

        If lastSelectedRows > DataGridViewInputChannel.Rows.Count - 1 Then lastSelectedRows = DataGridViewInputChannel.Rows.Count - 1

        If lastSelectedRows >= 0 Then
            DataGridViewInputChannel.Rows(lastSelectedRows).Selected = True
            DataGridViewInputChannel.FirstDisplayedScrollingRowIndex = lastSelectedRows
        End If

        If DataGridViewOutputChannel.Rows.Count > 0 Then
            DataGridViewOutputChannel.Rows(DataGridViewOutputChannel.Rows.Count - 1).Selected = True
            DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex = DataGridViewOutputChannel.Rows.Count - 1
        End If

        ButtonAddChannel.Enabled = DataGridViewInputChannel.Rows.Count > 0
        ButtonRmoveChannel.Enabled = DataGridViewOutputChannel.Rows.Count > 0
        isDirty = True
    End Sub

    Private Sub ButtonRmoveChannel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRmoveChannel.Click
        Dim lastSelectedRows As Integer
        ' цикл по листу2 в поисках выделенного
        DataGridViewOutputChannel.Focus()

        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridViewOutputChannel.SelectedRows
        Dim indexCollumName As Integer

        For Each collum As DataGridViewColumn In DataGridViewOutputChannel.Columns
            If collum.HeaderText = "Имя" Then
                indexCollumName = collum.Index
                Exit For
            End If
        Next

        ' вставить добавление в m_InputChannelsBindingList из m_ManagerChannelsInput
        ' затем из m_OutputChannelsBindingList удалить
        For Each selectRow As DataGridViewRow In selectedRows
            For I As Integer = OutputChannelsBindingList.Count - 1 To 0 Step -1
                If selectRow.Cells(indexCollumName).Value.ToString = OutputChannelsBindingList.Item(I).Name Then
                    mInputChannelsBindingList.Add(GetChannelsInput(OutputChannelsBindingList.Item(selectRow.Index).Name))
                    Exit For
                End If
            Next
        Next

        For Each selectRow As DataGridViewRow In selectedRows
            lastSelectedRows = selectRow.Index
            For I As Integer = OutputChannelsBindingList.Count - 1 To 0 Step -1
                'For Each itmXLoop In m_OutputChannelsBindingList
                'If SelectRow.Cells(IndCollum).Value = itmXLoop.Название Then
                If selectRow.Cells(indexCollumName).Value.ToString = OutputChannelsBindingList.Item(I).Name Then
                    'm_OutputChannelsBindingList.Remove(itmXLoop) 
                    OutputChannelsBindingList.RemoveAt(I)
                    'Exit For
                End If
                'Next
            Next
        Next

        CastingInputOutputChannelsBindingList()
        DataGridViewOutputChannel.Focus()

        For Each selectRow As DataGridViewRow In DataGridViewOutputChannel.SelectedRows
            selectRow.Selected = False
        Next

        If lastSelectedRows > DataGridViewOutputChannel.Rows.Count - 1 Then lastSelectedRows = DataGridViewOutputChannel.Rows.Count - 1

        If lastSelectedRows >= 0 Then
            DataGridViewOutputChannel.Rows(lastSelectedRows).Selected = True
            DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex = lastSelectedRows
        End If

        ButtonAddChannel.Enabled = DataGridViewInputChannel.Rows.Count > 0
        ButtonRmoveChannel.Enabled = DataGridViewOutputChannel.Rows.Count > 0
        isDirty = True
    End Sub

    Private Sub ButtonDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDown.Click
        lvPreviousSelectedIndices = lvSelectedIndices
        lvSelectedIndices -= 1
        LVSelectedIndicesValueChanged()
        isDirty = True
        ShowMessageOnStatusPanel(String.Format("Перемещён канал <{0}> в пользовательской конфигурации.",
                                        (OutputChannelsBindingList.Item(DataGridViewOutputChannel.SelectedRows(DataGridViewOutputChannel.SelectedRows.Count - 1).Index)).Name))
    End Sub

    Private Sub ButtonUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUp.Click
        lvPreviousSelectedIndices = lvSelectedIndices
        lvSelectedIndices += 1
        LVSelectedIndicesValueChanged()
        isDirty = True
        ShowMessageOnStatusPanel(String.Format("Перемещён канал <{0}> в пользовательской конфигурации.",
                                        (OutputChannelsBindingList.Item(DataGridViewOutputChannel.SelectedRows(DataGridViewOutputChannel.SelectedRows.Count - 1).Index)).Name))
    End Sub

    Private Sub ButtonClearChannels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClearChannels.Click
        ' первый лист полностью заполнить, а второй очистить
        BindingSourceInputChannels.DataSource = Nothing
        BindingSourceoOutputChannels.DataSource = Nothing

        mInputChannelsBindingList.Clear()
        OutputChannelsBindingList.Clear()

        For Each itemChannel As String In mManagerChannelsInput.CollectionsChannels.Keys
            mInputChannelsBindingList.Add(mManagerChannelsInput.Item(itemChannel))
        Next

        BindingSourceInputChannels.DataSource = mInputChannelsBindingList
        BindingSourceoOutputChannels.DataSource = OutputChannelsBindingList
        ButtonAddChannel.Enabled = DataGridViewInputChannel.Rows.Count > 0
        ButtonRmoveChannel.Enabled = DataGridViewOutputChannel.Rows.Count > 0
        isDirty = True
        ShowMessageOnStatusPanel("Произведена очистка выборочного списка пользователя.")
    End Sub

    Private Sub SwapChannels(ByVal current As Integer, ByVal target As Integer)
        Dim currentChannel As ChannelOutput = OutputChannelsBindingList(current)
        Dim givenChannel As ChannelOutput = OutputChannelsBindingList(target)

        OutputChannelsBindingList.RemoveAt(current)
        OutputChannelsBindingList.Insert(current, givenChannel)

        OutputChannelsBindingList.RemoveAt(target)
        OutputChannelsBindingList.Insert(target, currentChannel)

        For Each selectRow As DataGridViewRow In DataGridViewOutputChannel.SelectedRows
            selectRow.Selected = False
        Next

        DataGridViewOutputChannel.Rows(target).Selected = True
        BindingSourceoOutputChannels.Position = target
    End Sub

    ''' <summary>
    ''' Вставить Перед
    ''' </summary>
    ''' <param name="current"></param>
    ''' <param name="target"></param>
    Private Sub InsertBefore(ByVal current As Integer, ByVal target As Integer)
        Dim currentChannel As ChannelOutput = OutputChannelsBindingList(current)

        OutputChannelsBindingList.RemoveAt(current)
        OutputChannelsBindingList.Insert(target, currentChannel)

        For Each selectRow As DataGridViewRow In DataGridViewOutputChannel.SelectedRows
            selectRow.Selected = False
        Next

        DataGridViewOutputChannel.Rows(target).Selected = True
        BindingSourceoOutputChannels.Position = target
    End Sub

    Private Sub LVSelectedIndicesValueChanged()
        If DataGridViewOutputChannel.SelectedRows.Count = 0 Then Exit Sub

        DataGridViewOutputChannel.Focus()

        With DataGridViewOutputChannel
            If .Rows.Count > 0 Then
                If .SelectedRows.Count <> 0 Then
                    Dim indexSelected As Integer = .SelectedRows(.SelectedRows.Count - 1).Index

                    If lvPreviousSelectedIndices < lvSelectedIndices Then ' вверх
                        If indexSelected <> 0 Then
                            SwapChannels(indexSelected, indexSelected - 1)
                        End If
                    Else
                        If indexSelected <> .Rows.Count - 1 Then
                            SwapChannels(indexSelected, indexSelected + 1)
                        End If
                    End If
                End If
            End If
        End With

        SortOutputChannelsByNumberParameter()
    End Sub

    'Public m_TimeChannelsBindingList As New ChannelsInputBindingList

    'Private Sub cmdДобавитьКаналВремени_Click(sender As Object, e As EventArgs)
    '    Dim selectRow As DataGridViewRow
    '    Dim selectRows As DataGridViewSelectedRowCollection = grdInputChannel.SelectedRows
    '    Dim indexLast As Integer
    '    Dim success As Boolean

    '    ' цикл по листу1 в поисках выделенного
    '    For I As Integer = selectRows.Count - 1 To 0 Step -1
    '        selectRow = selectRows(I)

    '        ' здесь поиск в коллекции InputChannels  имени и добавление в OutputChannels
    '        Dim clsChannelInput As ChannelInput = mInputChannelsBindingList.Item(selectRow.Index)
    '        'm_OutputChannelsBindingList.Add(GetChannelsOutput(m_InputChannelsBindingList.Item(SelectRow.Index).Name))

    '        ' проверить на существование в таблице
    '        success = False
    '        For Each rowTime As Channels_cfg_lmzDataSet.TimeChannelRow In Me.ChannelsDataSet.TimeChannel.Rows
    '            If clsChannelInput.Name = rowTime.Name Then
    '                success = True
    '            End If
    '        Next

    '        If success = False Then
    '            Me.TimeChannelTableAdapter.Insert(clsChannelInput.NumberCh,
    '                                              clsChannelInput.Name,
    '                                              clsChannelInput.Pin,
    '                                              clsChannelInput.Scr_Name,
    '                                              clsChannelInput.State,
    '                                              clsChannelInput.Scr_EdIzm,
    '                                              clsChannelInput.Ch_Unit,
    '                                              clsChannelInput.GroupCh)
    '        End If
    '        indexLast = selectRow.Index
    '    Next

    '    'ChannelsDataSet.AcceptChanges()
    '    'Me.Validate()
    '    Me.TimeBindingSource.EndEdit()
    '    Me.TimeChannelTableAdapter.Update(Me.ChannelsDataSet.TimeChannel)
    '    Me.TimeChannelTableAdapter.Fill(Me.ChannelsDataSet.TimeChannel)

    '    'grdTimeChannel.Refresh()
    '    grdInputChannel.Focus()

    '    For Each selectRow In grdInputChannel.SelectedRows
    '        selectRow.Selected = False
    '    Next

    '    If indexLast > grdInputChannel.Rows.Count - 1 Then indexLast = grdInputChannel.Rows.Count - 1
    '    If indexLast >= 0 Then
    '        grdInputChannel.Rows(indexLast).Selected = True
    '        'grdInputChannel.FirstDisplayedScrollingRowIndex = indexLast
    '    End If

    '    'For Each selectRow In grdTimeChannel.SelectedRows
    '    '    selectRow.Selected = False
    '    'Next

    '    'If grdTimeChannel.Rows.Count > 0 Then
    '    '    grdTimeChannel.Rows(grdTimeChannel.Rows.Count - 1).Selected = True
    '    '    grdTimeChannel.FirstDisplayedScrollingRowIndex = grdTimeChannel.Rows.Count - 1
    '    'End If

    '    'cmdДобавитьКаналВремени.Enabled = grdInputChannel.Rows.Count > 0
    '    'cmdУдалитьКаналВремени.Enabled = grdTimeChannel.Rows.Count > 0
    '    Обновитьm_TimeChannelsBindingList()
    '    isDirty = True
    'End Sub

    'Private Sub cmdУдалитьКаналВремени_Click(sender As Object, e As EventArgs)
    '    'Dim SelectRow As DataGridViewRow '= grdInputChannel.Rows(intTabPosition)
    '    Dim selectRows As DataGridViewSelectedRowCollection = grdTimeChannel.SelectedRows

    '    Dim indexCollumName As Integer
    '    For Each collum As DataGridViewColumn In grdTimeChannel.Columns
    '        If collum.HeaderText = "Имя" Then
    '            indexCollumName = collum.Index
    '            Exit For
    '        End If
    '    Next

    '    ' не работает
    '    'For Each SelectRow As DataGridViewRow In SelectRows
    '    '    For I As Integer = Me.ChannelsDataSet.TimeChannel.Rows.Count - 1 To 0 Step -1
    '    '        If SelectRow.Cells(IndexCollumName).Value = Me.ChannelsDataSet.TimeChannel.Rows(I)(IndexCollumName) Then
    '    '            Me.ChannelsDataSet.TimeChannel.Rows(I).Delete()
    '    '            Exit For
    '    '        End If
    '    '    Next
    '    'Next

    '    Dim intIndex As Integer
    '    Dim success As Boolean

    '    For Each rowTime As Channels_cfg_lmzDataSet.TimeChannelRow In Me.ChannelsDataSet.TimeChannel.Rows
    '        success = False

    '        For Each selectRow As DataGridViewRow In selectRows
    '            If selectRow.Cells(indexCollumName).Value = rowTime.Name Then
    '                success = True
    '                Exit For
    '            End If
    '        Next

    '        If success = True Then
    '            Me.ChannelsDataSet.TimeChannel.Rows(intIndex).Delete()
    '        End If

    '        intIndex += 1
    '    Next

    '    '    'здесь ошибка SelectRow.Index
    '    'For I As Integer = SelectRows.Count - 1 To 0 Step -1
    '    '    SelectRow = SelectRows(I)
    '    '    Me.ChannelsDataSet.TimeChannel.Rows(SelectRow.Index).Delete()
    '    'Next

    '    Me.TimeBindingSource.EndEdit()
    '    Me.TimeChannelTableAdapter.Update(Me.ChannelsDataSet.TimeChannel)
    '    grdTimeChannel.Refresh()

    '    If grdTimeChannel.Rows.Count > 0 Then
    '        'grdTimeChannel.Rows(grdTimeChannel.Rows.Count - 1).Selected = True
    '        grdTimeChannel.FirstDisplayedScrollingRowIndex = grdTimeChannel.Rows.Count - 1
    '    End If

    '    cmdУдалитьКаналВремени.Enabled = grdTimeChannel.Rows.Count > 0
    '    Обновитьm_TimeChannelsBindingList()
    '    isDirty = True
    'End Sub

    'Private Sub Обновитьm_TimeChannelsBindingList()
    '    m_TimeChannelsBindingList.Clear()

    '    For Each rowTime As Channels_cfg_lmzDataSet.TimeChannelRow In Me.ChannelsDataSet.TimeChannel.Rows
    '        Dim clsChannelInput As New ChannelInput() With {.id = rowTime.id, .NumberCh = rowTime.NumberCh, .Name = rowTime.Name}

    '        If Not IsDBNull(rowTime("Pin")) Then clsChannelInput.Pin = rowTime.Pin
    '        'If rowChannel.Pin IsNot Nothing Then clsChannelInput.Pin = rowChannel.Pin'ругается

    '        clsChannelInput.Scr_Name = rowTime.Scr_Name
    '        clsChannelInput.State = rowTime.State

    '        If Not IsDBNull(rowTime("Scr_EdIzm")) Then clsChannelInput.Scr_EdIzm = rowTime.Scr_EdIzm
    '        If Not IsDBNull(rowTime("Ch_Unit")) Then clsChannelInput.Ch_Unit = rowTime.Ch_Unit

    '        clsChannelInput.GroupCh = rowTime.GroupCh
    '        m_TimeChannelsBindingList.Add(clsChannelInput)
    '    Next
    'End Sub
#End Region

#Region "Управление списком конфигураций"
    Private Sub ButtonRestoreConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRestoreConfiguration.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonRestoreConfiguration_Click)}> Загрузка Выделенной Конфигурации")
        RestoreSelectedConfiguration()
    End Sub

#Region "Запись Выделенной Конфигурации"
    Private Sub ButtonSaveConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveConfiguration.Click
        SaveConfiguration()
    End Sub

    ''' <summary>
    ''' Запись Выделенной Конфигурации
    ''' </summary>
    Private Sub SaveConfiguration()
        Dim strSQL As String
        Dim success As Boolean
        Dim keySelectedConfig As Integer
        Dim nameConfiguration As String = ComboBoxConfigurations.Text ' имя Конфигурации

        ' если имя пусто вопрос  "введите имя" и так как модально нельзя, то выход из данной процедуры
        If nameConfiguration = "" Then
            ' надо ввести имя
            MessageBox.Show("Введите имя", "Список", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim mfrmMessageBox As FormMessageBox = GetFormMessageBox("Подождите, идёт сохраненение настроек", "Запись конфигурации")
        ' если имя новое, то новая конфигурация добавляется
        ' если переписать то удаление из базы старых и запись новых
        ' если добавить то запись в базу, очистка comСписки и считывание в лист заново
        For Each itemConfig As Config In configs
            If itemConfig.NameConfiguration = nameConfiguration Then
                success = True
                keySelectedConfig = itemConfig.KeyConfig
                Exit For
            End If
        Next

        If success Then
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonSaveConfiguration_Click)}> Запись Выделенной Конфигурации")
            ' заново наполнить таблицу и сделать обновление
            'If keySelectedConfig = keyConfig Then ' выделенная конфигурация в списке соответствует текущей загруженной
            If keySelectedConfig <> keyConfig Then
                ' пользователь сменил выделение конфигурации в списке, значит перезаписать её из текущей
                Dim message As String = $"Конфигурация с таким именем уже существует!{vbCrLf}Подтвердите сохранение текущей конфигурации под именем {nameConfiguration} ?"
                Const caption As String = "Сохранение изменений"
                Dim result As DialogResult

                RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {message}")
                result = MessageBox.Show(Me, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = DialogResult.No Then Exit Sub

                keyConfig = keySelectedConfig
                ChannelConfigTableAdapter.FillBykeyConfig(ChannelsDataSet.ChannelConfig, keyConfig)
            End If

            ' отсоединённый набор очистить
            For Each rowChannel As Channels_cfg_lmzDataSet.ChannelConfigRow In ChannelsDataSet.ChannelConfig.Rows
                rowChannel.Delete()
            Next
            'Me.ChannelConfigTableAdapter.Delete()

            ' заполнить отредактированными значениями
            FillChannelConfigTableAdapter(keyConfig)
            SaveTableChannelConfig()
        Else
            ' добавить новую конфигурацию и узнать keyConfig по событию
            connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
            Dim odaDataAdapter As OleDbDataAdapter
            Dim dtDataTable As New DataTable
            Dim drDataRow As DataRow
            Dim cb As OleDbCommandBuilder

            Try
                strSQL = "SELECT * FROM Config"
                odaDataAdapter = New OleDbDataAdapter(strSQL, connection)
                odaDataAdapter.Fill(dtDataTable)

                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("ИмяКонфигурации") = nameConfiguration
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
                ' подключить событие обновления автонумерации Autonumber.
                AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)
                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.Update(dtDataTable)
                connection.Close()
            Catch ex As Exception
                Const caption As String = "Запись конфигурации в базу"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try

            configs.Add(New Config(keyConfig, nameConfiguration))
            ComboBoxConfigurations.Items.Add(configs(configs.Count - 1)) ' добавить в comСписки последний из коллекции
            ' в текущуй базе добавить каналы которые пойдут в новую конфигурацию с новым ключом т.е. DataRowState.Added
            FillChannelConfigTableAdapter(keyConfig)
        End If

        ComboBoxConfigurations.Text = nameConfiguration ' выделить для загрузки
        SaveChangeChannelStendAndChannelN(StandNumber)
        RestoreSelectedConfiguration() ' там конфигурируется адаптер
        mfrmMessageBox.Close()
        isDirty = False
    End Sub

    ''' <summary>
    ''' заполнить отредактированными значениями адаптер
    ''' </summary>
    ''' <param name="keyConfig"></param>
    ''' <remarks></remarks>
    Private Sub FillChannelConfigTableAdapter(keyConfig As Integer)
        For Each ch As ChannelOutput In OutputChannelsBindingList
            ChannelConfigTableAdapter.Insert(keyConfig,
                                                ch.NumberParameter,
                                                ch.Name,
                                                ch.NameParameter,
                                                ch.UnitOfMeasure,
                                                ch.LowerLimit,
                                                ch.UpperLimit,
                                                CInt(ch.RangeYmin),
                                                CInt(ch.RangeYmax),
                                                ch.AlarmValueMin,
                                                ch.AlarmValueMax,
                                                ch.GroupCh)
        Next
    End Sub

    ''' <summary>
    ''' Процедура обработки события OnRowUpdated при создании автоинкрементного поля 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As OleDbRowUpdatedEventArgs)
        ' Включить переменную и команду получения значения идентификатора из Access базы данных.
        'Dim newID As Integer = 0
        Dim idCMD As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", connection)

        If args.StatementType = StatementType.Insert Then
            ' перезаписать идентификатор и сохранить его в поле колонки keyConfig
            keyConfig = CInt(idCMD.ExecuteScalar())
            args.Row("keyConfig") = keyConfig
        End If
    End Sub

    ''' <summary>
    ''' Запись Таблицы ChannelConfig
    ''' Сохранение изменений используя команды типизированного отсоединённого набора
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveTableChannelConfig()
        Validate()
        ChannelConfigBindingSource.EndEdit()

        Dim deletedChildRecords As Channels_cfg_lmzDataSet.ChannelConfigDataTable = CType(ChannelsDataSet.ChannelConfig.GetChanges(DataRowState.Deleted), Channels_cfg_lmzDataSet.ChannelConfigDataTable)
        Dim newChildRecords As Channels_cfg_lmzDataSet.ChannelConfigDataTable = CType(ChannelsDataSet.ChannelConfig.GetChanges(DataRowState.Added), Channels_cfg_lmzDataSet.ChannelConfigDataTable)
        Dim modifiedChildRecords As Channels_cfg_lmzDataSet.ChannelConfigDataTable = CType(ChannelsDataSet.ChannelConfig.GetChanges(DataRowState.Modified), Channels_cfg_lmzDataSet.ChannelConfigDataTable)

        With Me
            Try
                If deletedChildRecords IsNot Nothing Then .ChannelConfigTableAdapter.Update(deletedChildRecords)
                If modifiedChildRecords IsNot Nothing Then .ChannelConfigTableAdapter.Update(modifiedChildRecords)
                If newChildRecords IsNot Nothing Then .ChannelConfigTableAdapter.Update(newChildRecords)

                .ChannelsDataSet.AcceptChanges()
            Catch ex As Exception
                Dim caption As String = $"Ошибка обновления в процедуре  {NameOf(SaveTableChannelConfig)} в {NameOf(FormConfigChannelServer)}."
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            Finally
                If deletedChildRecords IsNot Nothing Then deletedChildRecords.Dispose()
                If modifiedChildRecords IsNot Nothing Then modifiedChildRecords.Dispose()
                If newChildRecords IsNot Nothing Then newChildRecords.Dispose()
            End Try
        End With

        Application.DoEvents()
    End Sub
#End Region

    Private Sub ButtonRemoveSelectedConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRemoveSelectedConfiguration.Click
        Dim selectedIndex As Integer = ComboBoxConfigurations.SelectedIndex
        Dim nameConfiguration As String = ComboBoxConfigurations.Text

        If nameConfiguration = "" OrElse selectedIndex = -1 Then Exit Sub

        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonRemoveSelectedConfiguration_Click)}> Удаление выделенной конфигурации {nameConfiguration}")
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text

        ' удалить из листа
        ComboBoxConfigurations.Items.RemoveAt(selectedIndex)
        configs.RemoveAt(selectedIndex)

        Try
            ' Открыть подключение
            cmd.CommandText = $"DELETE * FROM COnfig WHERE ИмяКонфигурации = '{nameConfiguration}'"
            cn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim caption As String = $"Удаление конфигурации {nameConfiguration} из базы"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If (cn.State = ConnectionState.Open) Then
                cn.Close()
            End If
            RestoreSelectedConfiguration() ' там конфигурируется адаптер
        End Try
    End Sub
#End Region

#Region "Поиск канала"
    Private Sub Filter()
        If Not shadowDataTable.Columns.Contains(ComboBoxFilter.Text) Then Exit Sub

        shadowDataTable.Rows.Clear()

        For Each GridRow As DataGridViewRow In DataGridViewInputChannel.Rows
            Dim dataRow As DataRow = shadowDataTable.NewRow()
            Dim I As Integer = 0

            For Each itemCell As DataGridViewCell In GridRow.Cells
                dataRow(I) = itemCell.Value
                I += 1
            Next

            shadowDataTable.Rows.Add(dataRow)
        Next

        ' "Имя like '*" & txtFilter.Text & "%'"
        shadowDataTable.DefaultView.RowFilter = $"{ComboBoxFilter.Text} like '*{TextBoxFilter.Text}%'"

        If shadowDataTable.DefaultView.Count = 0 OrElse TextBoxFilter.Text = "" Then
            PanelDrdFindChannel.Visible = False
            LabelRowPosition.Visible = False
            SetButtonPanelEnabled(True)

            If TextBoxFilter.Text <> "" Then
                Const caption As String = "Фильтрация"
                Dim text As String = "Не найдено ни одной записи с вхождением " & TextBoxFilter.Text
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If
        Else
            PanelDrdFindChannel.Visible = True
            LabelRowPosition.Visible = True
            SetButtonPanelEnabled(False)
        End If
    End Sub

    Private Sub TextBoxFilter_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxFilter.TextChanged
        If TextBoxFilter.Text <> "" Then
            Filter()
        Else
            PanelDrdFindChannel.Visible = False
            LabelRowPosition.Visible = False
            SetButtonPanelEnabled(True)
        End If
    End Sub

    Protected Sub DataGridView_PositionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If IsHandleCreated Then
            Try
                tabPosition = ToolStripContainerForm.BindingContext(shadowDataTable).Position
                ShowCurrentRowTable()
            Catch ex As Exception
                Const caption As String = "Изменение активной строки таблицы"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            End Try
        End If
    End Sub

    Private Sub ShowCurrentRowTable()
        If shadowDataTable IsNot Nothing Then
            If tabPosition <> -1 Then
                LabelRowPosition.Text = "Запись " & tabPosition + 1 & " из " & DataGridViewFindChannel.Rows.Count
            Else
                LabelRowPosition.Text = "Нет записей"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Выбор Подтверждён
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonСhoiceConfirmed_Click(sender As Object, e As EventArgs) Handles ButtonСhoiceConfirmed.Click
        Dim selectedNumber As Integer
        TextBoxFilter.Text = ""

        If DataGridViewFindChannel.SelectedRows.Count > 0 Then
            'selsctedNumber = grdFindChannel.SelectedRows(0).Index
            selectedNumber = CInt(DataGridViewFindChannel.SelectedRows(0).Cells(0).Value)

            For Each tmpRow As DataGridViewRow In DataGridViewInputChannel.Rows
                If CInt(tmpRow.Cells(0).Value) = selectedNumber Then
                    selectedNumber = tmpRow.Index
                    Exit For
                End If
            Next

            DataGridViewInputChannel.Rows(selectedNumber).Selected = True
            BindingSourceInputChannels.Position = selectedNumber
            PanelDrdFindChannel.Visible = False
            LabelRowPosition.Visible = False
            SetButtonPanelEnabled(True)
        End If
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        TextBoxFilter.Text = ""
    End Sub

    Private Sub SetButtonPanelEnabled(visible As Boolean)
        For Each control As Control In PanelButtons.Controls
            control.Enabled = visible
        Next

        ButtonAddChannel.Enabled = True
    End Sub
#End Region

#Region "Работа с конфигурациями"
    ''' <summary>
    ''' Запись TableChannels_cfg_lmz
    ''' при старте программы при первом запуске загружается и записывается в локальную базу
    ''' из которой в последствии при повторных вызовах формы можно быстро считать
    ''' </summary>
    ''' <param name="dtChannels"></param>
    ''' <remarks></remarks>
    Private Sub SaveTableChannels_cfg_lmz(dtChannels As DataTable)
        Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim odaDataAdapter As OleDbDataAdapter
        Dim strSQL As String = "DELETE * FROM " & conNameTableChannel

        Try
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cn.Open()
            cmd.ExecuteNonQuery()
            Thread.Sleep(500)
            Application.DoEvents()

            strSQL = "SELECT * FROM " & conNameTableChannel & " ORDER BY id"
            'strSQL = "SELECT Channel.id, Channel.Number, Channel.Name, Channel.Pin, Channel.Scr_Name, Channel.Scr_EdIzm, Channel.Ch_Unit, Channel.Group FROM Channel;"
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)

            For Each rowChannel As DataRow In dtChannels.Rows
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                ' не использовать в базе имена для столбцов Number и Group из-за ошибки определения команд InsertCommand и UpdateCommand
                'drDataRow("id") = RowChannel("id")
                drDataRow("NumberCh") = rowChannel("NumberCh")
                drDataRow("Name") = rowChannel("Name")
                drDataRow("Pin") = rowChannel("Pin")
                drDataRow("Scr_Name") = rowChannel("Scr_Name")
                drDataRow("State") = rowChannel("State")
                drDataRow("Scr_EdIzm") = rowChannel("Scr_EdIzm")
                drDataRow("Ch_Unit") = rowChannel("Ch_Unit")
                'drDataRow("GroupCh") = RowChannel("GroupCh")

                If IsDBNull(rowChannel("Pin")) Then
                    drDataRow("GroupCh") = GetNameOfGroup("")
                Else
                    drDataRow("GroupCh") = GetNameOfGroup(CStr(rowChannel("Pin")))
                End If

                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            'odaDataAdapter.InsertCommand = cb.GetInsertCommand
            'odaDataAdapter.UpdateCommand = cb.GetUpdateCommand
            odaDataAdapter.Update(dtDataTable)
            cn.Close()
            Thread.Sleep(500)
            Application.DoEvents()
            ChannelTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
            ChannelTableAdapter.Fill(ChannelsDataSet.Channel)
        Catch ex As Exception
            cn.Close()
            Dim caption As String = $"Ошибка при записи в процедуре {NameOf(SaveTableChannels_cfg_lmz)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Создать Имя Группы
    ''' На основании имени Пина RowChannel("Pin") создать имя группы
    ''' </summary>
    ''' <param name="rowChannelPin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNameOfGroup(rowChannelPin As String) As String
        Dim number As String
        Const conTime As String = "TIME"
        Dim newGroupCh As String = Nothing

        Try
            If rowChannelPin <> "" Then
                number = CInt(Val(rowChannelPin.Substring(0, 2))).ToString

                If rowChannelPin.Substring(number.Length, conTime.Length).ToUpper = conTime Then
                    newGroupCh = number ' номер
                Else
                    newGroupCh = rowChannelPin ' взять Pin
                End If
            Else
                newGroupCh = ""
            End If

            If newGroupCh = "" Then
                newGroupCh = "ИВК"
            Else
                number = CInt(Val(newGroupCh.Substring(0, 1))).ToString

                If rowChannelPin.Substring(number.Length, 1).ToUpper = "T" Then
                    newGroupCh = "ССД1"
                Else
                    number = CInt(Val(newGroupCh.Substring(0, 2))).ToString
                    newGroupCh = "ССД" & number
                End If
            End If

        Catch ex As Exception
            Dim caption As String = $"Ошибка создания новой группы в процедуре <{NameOf(GetNameOfGroup)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return newGroupCh
    End Function

    ''' <summary>
    ''' Получить Каналы Конфигуратора Сервера
    ''' Запросы для XDocument поиска значений атрибутов 
    ''' </summary>
    ''' <param name="pathServerCfglmzXml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTableServerChannelsConfigurationXML(ByVal pathServerCfglmzXml As String) As DataTable
        Dim xdoc As XDocument = XDocument.Load(pathServerCfglmzXml)
        'Dim ServerHostIP As String
        HostName = xdoc.<Cell>.<Config>.<Net>.<Server>.@HostIP
        'Dim ServerTcpPort As String
        PortTCP = CInt(xdoc.<Cell>.<Config>.<Net>.<Server>.@TcpPort)
        Dim ServerHost As String = xdoc.<Cell>.<Config>.<Net>.<Server>.@Host

        ' одинаково
        'Dim ServerHostIP2 As String = xdoc...<Server>.@HostIP
        'Dim queryServerHostIP = From Server In xdoc.Descendants("Server") Select Server.@HostIP
        'For Each result In queryServerHostIP
        '    Console.WriteLine(result)
        'Next
        'Dim queryServerHostIP2 = From Server In xdoc.<Cell>.<Config>.<Net>.<Server> Select Server.@HostIP
        'For Each result In queryServerHostIP2
        '    Console.WriteLine(result)
        'Next

        '*************************
        'Ch_Unit это Raw_Unit        
        Dim query = From Channel As XElement In xdoc.Descendants("Ch") Select Channel.@Name, Channel.@Pin, Channel.@Scr_Name, Channel.@State, Channel.@Scr_EdIzm, Channel.@Ch_Unit
        ' Select Channel.Value
        'Console.WriteLine("{0} Найдено всего", query.Count())
        'Console.WriteLine()

        Dim table As DataTable = CreateTableServerChannels()
        Dim row As DataRow
        Dim id As Integer = 1

        For Each item In query
            'Console.WriteLine(item)
            'For Each e In item.Attributes
            '    Console.WriteLine("Element content: " & e.Value)
            'Next

            row = table.NewRow()
            row("id") = id
            'row("ParentItem") = "ParentItem " + i.ToString()
            row("NumberCh") = id
            row("Name") = item.Name
            row("Pin") = item.Pin
            row("Scr_Name") = item.Scr_Name
            row("State") = item.State
            row("Scr_EdIzm") = item.Scr_EdIzm
            row("Ch_Unit") = item.Ch_Unit
            'row("GroupCh") = ""

            table.Rows.Add(row)
            id += 1
            'Console.WriteLine("Name = {0}; Pin = {1}; Scr_Name = {2}; Scr_EdIzm = {3}; Ch_Unit = {4}; ", item.Name, item.Pin, item.Scr_Name, If(item.Scr_EdIzm Is Nothing, "", item.Scr_EdIzm), If(item.Ch_Unit Is Nothing, "", item.Ch_Unit))
        Next

        '--- Выбрать все элементы имен в документе ---------------------------
        'For Each result In xdoc...<Ch>
        '    Console.WriteLine(result)
        '    'содержимое элемента
        '    'For Each e In result.Nodes().OfType(Of XAttribute)()
        '    For Each e In result.Attributes
        '        Console.WriteLine("Element content: " & e.Value)
        '    Next
        'Next
        '---------------------------------------------------------------------
        ''--- Выбрать всех клиентов в xml-документе --------------------------
        'For Each result In doc.<Root>.<Customers>
        '    Console.WriteLine(result.ToString)
        'Next

        Return table
    End Function

    ''' <summary>
    ''' Создать таблицу с известным составом столбцов
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateTableServerChannels() As DataTable
        Dim table As DataTable = New DataTable("Channel") ' Создать новую DataTable.
        ' Создать new DataColumn, установить DataType, ColumnName и добавить к DataTable.    
        '--- 1"id"
        Dim column As DataColumn = New DataColumn() With {.DataType = Type.GetType("System.Int32"), .ColumnName = "id", .ReadOnly = True, .Unique = True}
        ' добавить Column к DataColumnCollection.
        table.Columns.Add(column)
        '--- 2"NumberCh"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.Int32"), .ColumnName = "NumberCh", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 3"Name"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "Name", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 4"Pin"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "Pin", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 5"Scr_Name"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "Scr_Name", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 6"State"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "State", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 7"Scr_EdIzm"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "Scr_EdIzm", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 8"Ch_Unit"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "Ch_Unit", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)
        '--- 9"GroupCh"
        'column.Caption = ""
        column = New DataColumn() With {.DataType = Type.GetType("System.String"), .ColumnName = "GroupCh", .AutoIncrement = False, .ReadOnly = False, .Unique = False}
        table.Columns.Add(column)

        ' создать ID column как primary key column.
        Dim PrimaryKeyColumns(0) As DataColumn
        PrimaryKeyColumns(0) = table.Columns("id")
        table.PrimaryKey = PrimaryKeyColumns

        '' это должно быть в блоке декларации.
        ''Private dataSet As DataSet = New DataSet()

        '' добавить новую DataTable в DataSet.
        'dataSet.Tables.Add(table)

        ' создать три новых DataRow объекта и добавить их затем в DataTable
        'Dim i As Integer
        'For i = 0 To 2
        '    row = table.NewRow()
        '    row("id") = i
        '    row("ParentItem") = "ParentItem " + i.ToString()
        '    table.Rows.Add(row)
        'Next i

        Return table
    End Function

    ''' <summary>
    ''' Заполнить Таблицу Конфигурации Из Базы
    ''' Заполнить Channel значениями из базы
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateTableChannels_cfg_lmz()
        ChannelTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
        ChannelTableAdapter.Fill(ChannelsDataSet.Channel)
        DataGridViewInputChannel.Refresh()
    End Sub

    ''' <summary>
    ''' Заполнить TimeChannel значениями из базы
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateTimeChannelTable()
        Dim success As Boolean
        Dim currentIndex As Integer

        TimeChannelTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
        TimeChannelTableAdapter.Fill(ChannelsDataSet.TimeChannel)

        ' удалить из таблицы каналов времени в случае отсутствия в 1 таблице
        'For Each OutputChannel In m_OutputChannelsBindingList
        For Each rowTime As Channels_cfg_lmzDataSet.TimeChannelRow In ChannelsDataSet.TimeChannel.Rows
            'For Each InputChannel In m_InputChannelsBindingList
            success = False

            For Each rowChannel As Channels_cfg_lmzDataSet.ChannelRow In ChannelsDataSet.Channel.Rows
                If rowChannel.Name = rowTime.Name Then
                    'm_InputChannelsBindingList.Remove(rowChannel)
                    'Me.ChannelsDataSet.Channel.Rows.Remove(rowChannel)' не удаляет
                    'Me.ChannelsDataSet.Channel.Rows(intIndex).Delete()

                    'ChannelTableAdapter.DeleteQuery(rowChannel.id) ' работает
                    success = True
                    Exit For
                End If
            Next

            If success = False Then
                ChannelsDataSet.TimeChannel.Rows(currentIndex).Delete()
            End If

            currentIndex += 1
        Next

        'ChannelsDataSet.AcceptChanges()
        'Me.Validate()
        TimeBindingSource.EndEdit()
        TimeChannelTableAdapter.Update(ChannelsDataSet.TimeChannel)
        'Обновитьm_TimeChannelsBindingList()
    End Sub

    ''' <summary>
    ''' Заполнение m_ManagerChannelsInput каналами из Channel.Rows
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadManagerChannelsInput()
        mManagerChannelsInput = New ManagerChannelsInput

        For Each rowChannel As Channels_cfg_lmzDataSet.ChannelRow In ChannelsDataSet.Channel.Rows
            Dim newChannelInput As New ChannelInput() With {.Id = rowChannel.id,
                                                            .NumberCh = rowChannel.NumberCh,
                                                            .Name = rowChannel.Name}

            If Not IsDBNull(rowChannel("Pin")) Then newChannelInput.Pin = rowChannel.Pin
            'If rowChannel.Pin IsNot Nothing Then clsChannelInput.Pin = rowChannel.Pin ' ругается

            newChannelInput.ScrName = rowChannel.Scr_Name
            newChannelInput.State = rowChannel.State

            If Not IsDBNull(rowChannel("Scr_EdIzm")) Then newChannelInput.ScrEdIzm = rowChannel.Scr_EdIzm
            If Not IsDBNull(rowChannel("Ch_Unit")) Then newChannelInput.ChUnit = rowChannel.Ch_Unit

            newChannelInput.GroupCh = rowChannel.GroupCh

            ' произвести отсев параметров
            If IsSiftingChannels Then
                Dim isChannelRemove As Boolean = False ' Канал Исключить

                If rowChannel.Scr_Name.StartsWith("_") Then isChannelRemove = True
                If Array.IndexOf(CCDNamesToExclud, rowChannel.GroupCh) <> -1 Then isChannelRemove = True

                Dim tempCCD As String
                If EngineModification = conEngineModification117 Then
                    tempCCD = "ССД8" ' будет отсеян
                Else
                    tempCCD = "ССД10" ' будет отсеян
                End If

                If rowChannel.GroupCh = tempCCD Then isChannelRemove = True
                If rowChannel.State = "Off" Then isChannelRemove = True

                If Not isChannelRemove Then
                    mManagerChannelsInput.AddChannel(newChannelInput.Name, newChannelInput)
                End If
            Else
                mManagerChannelsInput.AddChannel(newChannelInput.Name, newChannelInput)
            End If
        Next

        If mManagerChannelsInput.IsRepetitionChannelNames Then
            Dim text As String = $"Среди каналов в конфигурационном файле Сервера имеются каналы с одинаковыми экранными именами!{vbCrLf}Была произведена следующая замена экранных имён:{vbCrLf}{mManagerChannelsInput.RepetitionChannelNames}"
            Const caption As String = "Изменение экранного имени канала"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Заполнение m_ManagerChannelsOutput каналами из ChannelConfig.Rows
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadManagerChannelsOutput()
        Dim rowChannel As Channels_cfg_lmzDataSet.ChannelConfigRow

        mManagerChannelsOutput = New ManagerChannelsOutput

        'For Each rowChannel As Channels_cfg_lmzDataSet.ChannelConfigRow In Me.ChannelsDataSet.ChannelConfig.Rows
        For I As Integer = 0 To ChannelsDataSet.ChannelConfig.Rows.Count - 1
            rowChannel = CType(ChannelsDataSet.ChannelConfig.Rows(I), Channels_cfg_lmzDataSet.ChannelConfigRow)

            ' добавить в Output хранилище только те каналы, которые содержатся в Input хранилище 
            If mManagerChannelsInput.CollectionsChannels.ContainsKey(rowChannel.Name) Then

                Dim clsChannelOutput As New ChannelOutput() With {.Id = rowChannel.id,
                                                                  .KeyConfig = rowChannel.keyConfig,
                                                                  .NumberParameter = rowChannel.НомерПараметра,
                                                                  .Name = rowChannel.Name}

                If mManagerChannelsInput.CollectionsChannels(rowChannel.Name).ScrName <> rowChannel.НаименованиеПараметра Then
                    If changeNameChannelList Is Nothing Then
                        changeNameChannelList = New List(Of String)
                    End If

                    'Dim text As String = String.Format("{0,-30}  ->   {1,30}", Trim(rowChannel.НаименованиеПараметра), Trim(m_ManagerChannelsInput.CollectionsChannels(rowChannel.Name).Scr_Name))
                    'Dim text As String = String.Format("{0,-30}", Trim(rowChannel.НаименованиеПараметра)) & vbTab & " ->  " & String.Format("{0,30}", Trim(m_ManagerChannelsInput.CollectionsChannels(rowChannel.Name).Scr_Name))
                    'Dim text As String = Trim(rowChannel.НаименованиеПараметра) & vbTab & If(rowChannel.НаименованиеПараметра.Length < 9, vbTab, "") & If(rowChannel.НаименованиеПараметра.Length < 17, vbTab, "") & " ->  " & vbTab & Trim(ManagerChannelsInputG.CollectionsChannels(rowChannel.Name).Scr_Name)
                    Dim text As String = $"{Trim(rowChannel.НаименованиеПараметра)}{vbTab}{If(rowChannel.НаименованиеПараметра.Length < 9, vbTab, "")}{If(rowChannel.НаименованиеПараметра.Length < 17, vbTab, "")} ->  {vbTab}{Trim(mManagerChannelsInput.CollectionsChannels(rowChannel.Name).ScrName)}"
                    changeNameChannelList.Add(text)
                End If

                'clsChannelOutput.НаименованиеПараметра = rowChannel.НаименованиеПараметра
                'rowChannel.НаименованиеПараметра = m_ManagerChannelsInput.CollectionsChannels(rowChannel.Name).Scr_Name
                ChannelsDataSet.ChannelConfig.Rows(I).Item("НаименованиеПараметра") = mManagerChannelsInput.CollectionsChannels(rowChannel.Name).ScrName
                clsChannelOutput.NameParameter = mManagerChannelsInput.CollectionsChannels(rowChannel.Name).ScrName

                If Not IsDBNull(rowChannel("ЕдиницаИзмерения")) Then clsChannelOutput.UnitOfMeasure = rowChannel.ЕдиницаИзмерения
                'If rowChannel.Pin IsNot Nothing Then clsChannelOutput.Pin = rowChannel.Pin

                clsChannelOutput.LowerLimit = rowChannel.ДопускМинимум
                clsChannelOutput.UpperLimit = rowChannel.ДопускМаксимум

                clsChannelOutput.RangeYmin = rowChannel.РазносУмин
                clsChannelOutput.RangeYmax = rowChannel.РазносУмакс

                clsChannelOutput.AlarmValueMin = rowChannel.АварийноеЗначениеМин
                clsChannelOutput.AlarmValueMax = rowChannel.АварийноеЗначениеМакс

                clsChannelOutput.GroupCh = rowChannel.GroupCh
                clsChannelOutput.NumberFormula = 2 ' забыл внести поле при создании базы, поэтому здесь заплатка
                mManagerChannelsOutput.AddChannel(clsChannelOutput.Name, clsChannelOutput)
            End If
        Next

        If changeNameChannelList IsNot Nothing AndAlso Not isFormClosed Then
            Dim repetitionChannels As String = String.Empty ' повторы Каналов
            Dim I As Integer

            For Each ChangedName As String In changeNameChannelList
                repetitionChannels &= ChangedName & vbCrLf
                I += 1
                If I > 30 Then
                    repetitionChannels &= "и ещё более ..." & vbCrLf
                    Exit For
                End If
            Next

            Dim text As String = $"Из-за несогласованности экранных имён каналов в конфигурации Сервера и загружаемой конфигурацией было произведено их приведение!{vbCrLf}Была произведена следующая замена экранных имён:{vbCrLf}{repetitionChannels}"
            Const caption As String = "Изменение экранного имени канала"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub

    ''' <summary>
    ''' Загрузить Списки Конфигурации
    ''' Заполнить список именами созданных конфигураций
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadConfigurationList()
        Const strSQL As String = "SELECT * FROM Config ORDER BY keyConfig"
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable

        Try
            Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))
                cn.Open()
                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                odaDataAdapter.Fill(dtDataTable)
            End Using

            configs = New List(Of Config)

            For Each itemDataRow As DataRow In dtDataTable.Rows
                configs.Add(New Config(CInt(itemDataRow("keyConfig")), CStr(itemDataRow("ИмяКонфигурации"))))
            Next

            ComboBoxConfigurations.Items.Clear()
            ComboBoxConfigurations.Items.AddRange(configs.ToArray)
            ComboBoxConfigurations.DisplayMember = "ИмяКонфигурации"
            ComboBoxConfigurations.ValueMember = "keyConfig"
        Catch ex As Exception
            Const caption As String = "Загрузка конфигурации"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            Dim mkeyConfig As Integer = CInt(GetIni(PathOptions, "Options", "LastTCPkeyConfig", "0"))
            Dim selectedIndex As Integer
            Dim success As Boolean

            For Each itemConfig As Config In ComboBoxConfigurations.Items
                If itemConfig.KeyConfig = mkeyConfig Then
                    success = True
                    Exit For
                End If
                selectedIndex += 1
            Next

            ComboBoxConfigurations.Focus()

            If success Then
                ComboBoxConfigurations.SelectedIndex = selectedIndex
            Else
                ComboBoxConfigurations.SelectedIndex = ComboBoxConfigurations.Items.Count - 1
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Восстановить Выбранную Конфигурацию
    ''' Загрузить выделенную в списке конфигурацию  
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RestoreSelectedConfiguration()
        Dim nameConfiguration As String = Nothing

        If ComboBoxConfigurations.SelectedIndex <> -1 Then
            nameConfiguration = ComboBoxConfigurations.Text
            keyConfig = CType(ComboBoxConfigurations.SelectedItem, Config).KeyConfig
        End If

        If nameConfiguration <> "" Then
            ChannelConfigTableAdapter_FillBykeyConfig(keyConfig)
            SetInputOutputChannelsBindingList()
            CastingInputOutputChannelsBindingList()
            RestoreAllChannelOutputMembersFromChannelsNBaseDataSet()
        End If

        ButtonAddChannel.Enabled = DataGridViewInputChannel.Rows.Count > 0
        ButtonRmoveChannel.Enabled = DataGridViewOutputChannel.Rows.Count > 0
        isDirty = True
    End Sub

    Private Sub ChannelConfigTableAdapter_FillBykeyConfig(keyConfig As Integer)
        ' строка подключения сделал сам т.к. в дизайнере ссылка на строку созданную при создании дизайнера
        ChannelConfigTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))

        ' This line of code loads data into the 'BaseFormDataSet.ИзмеренныеПараметры' table. You can move, or remove it, as needed.
        'Me.ChannelConfigTableAdapter.Fill(Me.ChannelsDataSet.ChannelConfig)
        If keyConfig > 0 Then
            ChannelConfigTableAdapter.FillBykeyConfig(ChannelsDataSet.ChannelConfig, keyConfig)
            LoadManagerChannelsOutput()
        End If
    End Sub

    ''' <summary>
    ''' Заполнить источники DataSource для таблиц из уже отсеянных списков в m_ManagerChannelsInput и m_ManagerChannelsOutput
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetInputOutputChannelsBindingList()
        BindingSourceInputChannels.DataSource = Nothing
        BindingSourceoOutputChannels.DataSource = Nothing

        mInputChannelsBindingList.Clear()
        OutputChannelsBindingList.Clear()

        For Each ItemChannel As String In mManagerChannelsInput.CollectionsChannels.Keys
            mInputChannelsBindingList.Add(mManagerChannelsInput.Item(ItemChannel))
        Next

        For Each ItemChannel As String In mManagerChannelsOutput.CollectionsChannels.Keys
            OutputChannelsBindingList.Add(mManagerChannelsOutput.Item(ItemChannel))
        Next

        BindingSourceInputChannels.DataSource = mInputChannelsBindingList
        BindingSourceoOutputChannels.DataSource = OutputChannelsBindingList
    End Sub

    ''' <summary>
    ''' Очистить повторы каналов в первой таблице при их наличии во второй
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CastingInputOutputChannelsBindingList()
        BindingSourceInputChannels.DataSource = Nothing
        BindingSourceoOutputChannels.DataSource = Nothing

        ' удалить из первого листа повторы
        For Each outputChannel As ChannelOutput In OutputChannelsBindingList
            For Each inputChannel As ChannelInput In mInputChannelsBindingList
                If inputChannel.Name = outputChannel.Name Then
                    outputChannel.Pin = inputChannel.Pin ' скопировать Пин (столбец не  был добавлен в таблицу на этапе создания)
                    mInputChannelsBindingList.Remove(inputChannel)
                    Exit For
                End If
            Next
        Next

        ' чтобы коллекция была отсортирована
        Dim tempChannelsInputBindingList As New ChannelsInputBindingList

        For Each itemChannel As String In mManagerChannelsInput.CollectionsChannels.Keys
            For Each itemChannelList As ChannelInput In mInputChannelsBindingList
                If itemChannel = itemChannelList.Name Then
                    tempChannelsInputBindingList.Add(mManagerChannelsInput.Item(itemChannelList.Name))
                    Exit For
                End If
            Next
        Next

        SortOutputChannelsByNumberParameter()
        mInputChannelsBindingList = tempChannelsInputBindingList
        BindingSourceInputChannels.DataSource = mInputChannelsBindingList
        BindingSourceoOutputChannels.DataSource = OutputChannelsBindingList
    End Sub

    ''' <summary>
    ''' Отсортировать Номера ChannelOutput
    ''' Присвоить номера новые имена строк
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SortOutputChannelsByNumberParameter()
        Dim I As Integer = 1
        For Each itemChannelList As ChannelOutput In OutputChannelsBindingList
            itemChannelList.NumberParameter = I
            I += 1
        Next
    End Sub

    ''' <summary>
    ''' Инициализировать таблицу поиска
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeShadowDataTable()
        PopulateShadowDataTable()

        If shadowDataTable.Rows.Count > 0 Then
            tabPosition = 0
            ShowCurrentRowTable()
        End If
    End Sub

    ''' <summary>
    ''' Заполнить данными срытую таблицу из таблицы grdInputChannel
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateShadowDataTable()
        Try
            shadowDataTable = New DataTable

            For Each collName As DataGridViewColumn In DataGridViewInputChannel.Columns
                shadowDataTable.Columns.Add(collName.HeaderText)
            Next

            AddHandler ToolStripContainerForm.BindingContext(shadowDataTable).PositionChanged, AddressOf DataGridView_PositionChanged

            DataGridViewFindChannel.DataSource = shadowDataTable
            DataGridViewFindChannel.Columns(0).Width = 0
            DataGridViewFindChannel.Columns(0).Visible = False
            DataGridViewFindChannel.Columns(1).FillWeight = 50
            DataGridViewFindChannel.Columns(1).Width = 50
            DataGridViewFindChannel.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            DataGridViewFindChannel.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewFindChannel.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewFindChannel.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            ContexCurrencyManager = CType(DataGridViewFindChannel.BindingContext(shadowDataTable), CurrencyManager)
        Catch ex As Exception
            Dim caption As String = $"Загрузка таблицы поиска в <{NameOf(PopulateShadowDataTable)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        End Try
    End Sub

#End Region

#Region "Read Save options"
    Private Sub ButtonSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonSave.Click
        SaveStandOptions()
        ReloadStandOptions()
    End Sub

    ''' <summary>
    ''' Запись настроек в INI файл
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveStandOptions()
        WriteINI(PathOptions, "Options", "LastTCPkeyConfig", CStr(keyConfig))
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(SaveStandOptions)}> стенд: {StandNumber}, LastTCPkeyConfig: {keyConfig}")
    End Sub

    ''' <summary>
    ''' Перезагрузить Настройки Стенда
    ''' </summary>
    Private Sub ReloadStandOptions()
        IsNumberStendchanged = True
        ConfigureByStend()
        IsNumberStendchanged = False
    End Sub

    ''' <summary>
    ''' Заполнить Список
    ''' Считать пути к файлам конфигураций серверов
    ''' </summary>
    ''' <param name="inComboBox"></param>
    ''' <remarks></remarks>
    Private Sub PopulateComboBox(ByVal inComboBox As ComboBox)
        For I As Integer = 0 To StendNumbers.Count - 1
            inComboBox.Items.Add(StendNumbers(I))

            If inComboBox Is ComboBoxPathServerConfigXML Then
                pathServerCfg_xml(I) = GetIni(PathOptions, "ServerCfg_xml", "Stend" & StendNumbers(I), "\\Stend_1\c\Нужно ввести путь.xml")
            End If
        Next
    End Sub

    ''' <summary>
    ''' Считать Настройки
    ''' </summary>
    Private Sub LoadStandOptions()
        StendNumbers.AddRange(GetIni(PathOptions, "Stend", "Stends", "1, 2, 3, 4").Split(CType(", ", Char())).ToArray)
        'ReDim_pathServerCfg_xml(StendNumbers.Count - 1)
        Re.Dim(pathServerCfg_xml, StendNumbers.Count - 1)
        PopulateComboBox(ComboBoxPathServerConfigXML)

        Try
            ' 1)номер стенда
            For I As Integer = 0 To StendNumbers.Count - 1
                If StendNumbers(I) = StandNumber Then
                    ComboBoxPathServerConfigXML.SelectedIndex = I
                    TextBoxPathServerConfigXML.Text = pathServerCfg_xml(I)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Const caption As String = "Ошибка при считывании настроек из файла <Опции.ini>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Поиск Пути Конфигурации Server
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonExplorerServerConfigXML_Click(sender As Object, e As EventArgs) Handles ButtonExplorerServerConfigXML.Click
        With OpenFileDialogPathDB
            .FileName = vbNullString
            .Title = "Поиск конфигурационного файла Сервера для стенда №" & ComboBoxPathServerConfigXML.Text
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonExplorerServerConfigXML_Click)}> { .Title}")
            .InitialDirectory = "c:  \"
            .CheckFileExists = True
            .DefaultExt = "xml"
            .RestoreDirectory = True
            ' установить флаг атрибутов
            .Filter = "cfg_lmz* (*.xml)|*.xml"

            If .ShowDialog = DialogResult.OK Then
                If Len(.FileName) = 0 Then
                    Exit Sub
                End If

                PathServerCfglmzXml = .FileName
                TextBoxPathServerConfigXML.Text = PathServerCfglmzXml
                pathServerCfg_xml(ComboBoxPathServerConfigXML.SelectedIndex) = PathServerCfglmzXml
                WriteINI(PathOptions, "ServerCfg_xml", "Stend" & ComboBoxPathServerConfigXML.Text, PathServerCfglmzXml)
            End If
        End With
    End Sub

    Private Sub ComboBoxPathServerConfigXML_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxPathServerConfigXML.SelectedIndexChanged
        TextBoxPathServerConfigXML.Text = pathServerCfg_xml(ComboBoxPathServerConfigXML.SelectedIndex)
        PathServerCfglmzXml = TextBoxPathServerConfigXML.Text
        ReloadStandOptions()
    End Sub
#End Region

#Region "DragDrop"
    Private Sub DataGridViewOutputChannel_DragEnter(sender As Object, e As DragEventArgs) Handles DataGridViewOutputChannel.DragEnter
        e.Effect = DragDropEffects.Copy
        ' сбросить текст метки.
        'DropLocationLabel.Text = "None"
    End Sub

    'Private Sub grdOutputChannel_DragLeave(sender As Object, e As System.EventArgs) Handles grdOutputChannel.DragLeave
    '    ' сбросить текст метки.
    '    'DropLocationLabel.Text = "None"
    'End Sub

    Private Sub DataGridViewOutputChannel_DragOver(sender As Object, e As DragEventArgs) Handles DataGridViewOutputChannel.DragOver
        ' Определить соответствует ли тип ChannelOutput из drop data. Если нет drop effect чувствует что drop не может случиться.
        If Not (e.Data.GetDataPresent(GetType(ChannelOutput))) Then
            e.Effect = DragDropEffects.None
            'DropLocationLabel.Text = "None - no ChannelOutput data."
            Return
        End If

        ' установить эффект на основании KeyState.
        If ((e.KeyState And (8 + 32)) = (8 + 32) And (e.AllowedEffect And DragDropEffects.Link) = DragDropEffects.Link) Then
            ' KeyState 8 + 32 = CTL + ALT

            ' связать drag-and-drop эффект.
            e.Effect = DragDropEffects.Link
        ElseIf ((e.KeyState And 32) = 32 And (e.AllowedEffect And DragDropEffects.Link) = DragDropEffects.Link) Then

            ' ALT KeyState для связи.
            e.Effect = DragDropEffects.Link
        ElseIf ((e.KeyState And 4) = 4 And (e.AllowedEffect And DragDropEffects.Move) = DragDropEffects.Move) Then

            ' SHIFT KeyState для перемещения.
            e.Effect = DragDropEffects.Move
        ElseIf ((e.KeyState And 8) = 8 And (e.AllowedEffect And DragDropEffects.Copy) = DragDropEffects.Copy) Then

            ' CTL KeyState для копирования.
            e.Effect = DragDropEffects.Copy
        ElseIf ((e.AllowedEffect And DragDropEffects.Move) = DragDropEffects.Move) Then

            ' по умолчанию, drop действие должно быть перемещение, если разрешено.
            'e.Effect = DragDropEffects.Move
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

        ' Позиция мыши зависит от экрана, так что надо конвертировать клиентские координаты
        ' indexOfItemUnderMouseToDrop = ListDragTarget.IndexFromPoint(ListDragTarget.PointToClient(New Point(e.X, e.Y)))

        Dim clientPoint As Point = DataGridViewOutputChannel.PointToClient(New Point(e.X, e.Y))
        Dim info As DataGridView.HitTestInfo = DataGridViewOutputChannel.HitTest(clientPoint.X, clientPoint.Y)

        If info.RowIndex >= 0 Then
            Const horizontalScrollBarHeight As Integer = 17 ' вычислил эксперементально
            Dim selectedCellHeight As Integer

            If DataGridViewOutputChannel.SelectedRows.Count > 0 Then
                selectedCellHeight = DataGridViewOutputChannel.SelectedCells(0).ContentBounds.Height ' высота ячейки (шрифт)
            End If

            ' снять выделение
            For Each SelectRow As DataGridViewRow In DataGridViewOutputChannel.SelectedRows
                SelectRow.Selected = False
            Next

            Dim isDown As Boolean = indexOfItemUnderMouseToDragTemp < info.RowIndex ' определить в каком направлении тянется строка

            If isDown Then ' вниз - надо бы вычислить нижний край
                If info.RowIndex + 1 >= CInt(DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex + (DataGridViewOutputChannel.Height - DataGridViewOutputChannel.ColumnHeadersHeight - 4 - horizontalScrollBarHeight) / (selectedCellHeight + 8)) Then
                    DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex = DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex + 1
                End If
            Else ' вверх
                If DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex = info.RowIndex Then ' подошли к верхнему краю - надо прокрутить
                    ' прокрутить невидимый
                    If info.RowIndex <> 0 Then
                        DataGridViewOutputChannel.FirstDisplayedScrollingRowIndex = info.RowIndex - 1
                    End If
                End If
            End If

            DataGridViewOutputChannel.Rows(info.RowIndex).Selected = True
            indexOfItemUnderMouseToDragTemp = info.RowIndex
        End If

        ' обновить текст метки.
        'If (indexOfItemUnderMouseToDrop <> ListBox.NoMatches) Then
        '   DropLocationLabel.Text = "Drops before item #" & (indexOfItemUnderMouseToDrop + 1)
        'Else
        '    DropLocationLabel.Text = "Drops at the end."
        'End If
    End Sub

    Private Sub DataGridViewOutputChannel_DragDrop(sender As Object, e As DragEventArgs) Handles DataGridViewOutputChannel.DragDrop
        'Dim clientPoint As Point = grdOutputChannel.PointToClient(New Point(e.X, e.Y))
        'Dim hit As DataGridView.HitTestInfo = grdOutputChannel.HitTest(clientPoint.X, clientPoint.Y)
        'Dim myType As Type = originGrid.Rows.GetType
        'If hit.RowIndex <> -1 Then
        '    grdOutputChannel.Rows.Insert(hit.RowIndex, e.Data.GetData(myType))
        'Else
        '    grdOutputChannel.Rows.Add(e.Data.GetData(myType))
        'End If

        If e.Data.GetDataPresent(GetType(ChannelOutput)) Then
            ' Определить какой категории содержимое было брошено
            Dim clientPoint As Point = DataGridViewOutputChannel.PointToClient(New Point(e.X, e.Y))
            ' надо перевести координаты e As System.Windows.Forms.DragEventArgs в e As System.Windows.Forms.MouseEventArgs
            Dim info As DataGridView.HitTestInfo = DataGridViewOutputChannel.HitTest(clientPoint.X, clientPoint.Y)
            'Dim index As Integer = lstCategories.IndexFromPoint(p)
            'If index >= 0 Then
            If info.RowIndex >= 0 Then
                ' получить ссылку к строке

                'Dim category As DataRowView = DirectCast(lstCategories.Items(index), DataRowView)
                'Dim category As DataRowView = DirectCast(grdOutputChannel.Rows(info.RowIndex).DataBoundItem, DataRowView)
                Dim RowTarget As ChannelOutput = DirectCast(DataGridViewOutputChannel.Rows(info.RowIndex).DataBoundItem, ChannelOutput)
                'Dim RowSource As DataRowView = DirectCast(e.Data.GetData(GetType(DataRowView)), DataRowView)'у меня типизированный
                Dim RowSource As ChannelOutput = DirectCast(e.Data.GetData(GetType(ChannelOutput)), ChannelOutput)
                ' здесь произвести обмен данными как и перемещением с помощью кнопок
                InsertBefore(indexOfItemUnderMouseToDrag, info.RowIndex)
                SortOutputChannelsByNumberParameter()

                ' получить старые и новые ids
                'Dim newID As Integer = CInt(category(0))
                'Dim oldID As Integer = CInt(campaign(1))
                '' удостовериться, что второй не такой же
                'If oldID <> newID Then
                '    campaign.BeginEdit()
                '    campaign(1) = newID
                '    campaign.EndEdit()
                'End If

                isDirty = True
            End If
        End If
    End Sub

    Private Sub DataGridViewOutputChannel_MouseDown(sender As Object, e As MouseEventArgs) Handles DataGridViewOutputChannel.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim info As DataGridView.HitTestInfo = DataGridViewOutputChannel.HitTest(e.X, e.Y)
            If info.RowIndex >= 0 Then
                Dim dragSize As Size = SystemInformation.DragSize

                ' создать прямоугольник используя DragSize, в позиции мыши в центре квадрата
                dragBoxFromMouseDown = New Rectangle(New Point(e.X - (dragSize.Width \ 2), e.Y - (dragSize.Height \ 2)), dragSize)
                indexOfItemUnderMouseToDrag = info.RowIndex
                indexOfItemUnderMouseToDragTemp = indexOfItemUnderMouseToDrag
                ' закоментировал т.к. DoDragDrop начинается в перетаскивании
                'Dim view As DataRowView = DirectCast(grdOutputChannel.Rows(info.RowIndex).DataBoundItem, DataRowView) не работает
                'Dim view As ChannelOutput = DirectCast(grdOutputChannel.Rows(info.RowIndex).DataBoundItem, ChannelOutput)
                'If view IsNot Nothing Then
                '    grdOutputChannel.DoDragDrop(view, DragDropEffects.Copy)
                'End If

                'if hit.Type is DataGridViewHitTestType.Cell then
                '    dataGridView1.DoDragDrop(dataGridView1.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value.ToString(), DragDropEffects.Copy)

                For Each SelectRow As DataGridViewRow In DataGridViewOutputChannel.SelectedRows
                    SelectRow.Selected = False
                Next
                DataGridViewOutputChannel.Rows(indexOfItemUnderMouseToDrag).Selected = True
            Else
                ' сбросить прямоугольник, если мышь не над поверхностью элемента ListBox
                dragBoxFromMouseDown = Rectangle.Empty
            End If
        End If
    End Sub

    Private Sub DataGridViewOutputChannel_MouseUp(sender As Object, e As MouseEventArgs) Handles DataGridViewOutputChannel.MouseUp
        dragBoxFromMouseDown = Rectangle.Empty
    End Sub

    'Private MyNoDropCursor As Cursor
    'Private MyNormalCursor As Cursor
    'Private UseCustomCursorsCheckChecked As Boolean = True
    Private Sub DataGridViewOutputChannel_MouseMove(sender As Object, e As MouseEventArgs) Handles DataGridViewOutputChannel.MouseMove
        'If ((e.Button And MouseButtons.Left) = MouseButtons.Left) Then
        If ((e.Button And MouseButtons.Right) = MouseButtons.Right) Then

            ' если мышь премещается за пределами квадрата, начать drag
            If (Rectangle.op_Inequality(dragBoxFromMouseDown, Rectangle.Empty) And
                Not dragBoxFromMouseDown.Contains(e.X, e.Y)) Then
                ' создать пользовательский курсор для операции drag-and-drop
                'Try
                '    'MyNormalCursor = New Cursor("expl_linetop_blue.ico") 
                '    'MyNoDropCursor = New Cursor("aero_unavail.cur")
                'Catch
                '    ' если произошла ошибка при попытки, то загрузить курсор по умолчанию
                '    'UseCustomCursorsCheckChecked = False
                'Finally

                ' screenOffset используется для подсчёта границ рабочего экрана
                ' который может быть выше или левее экрана, тогда определяется прерывание операции  drag drop
                screenOffset = SystemInformation.WorkingArea.Location

                ' Продолжить drag-and-drop, проходя пункт меню.  
                'Dim dropEffect As DragDropEffects = grdOutputChannel.DoDragDrop(ListDragSource.Items(indexOfItemUnderMouseToDrag), DragDropEffects.All Or DragDropEffects.Link)
                Dim view As ChannelOutput = DirectCast(DataGridViewOutputChannel.Rows(indexOfItemUnderMouseToDrag).DataBoundItem, ChannelOutput)
                Dim dropEffect As DragDropEffects = DataGridViewOutputChannel.DoDragDrop(view, DragDropEffects.All Or DragDropEffects.Link)

                ' если drag операция была перемещением, то удалить элемент.
                'If (dropEffect = DragDropEffects.Move) Then
                '    ListDragSource.Items.RemoveAt(indexOfItemUnderMouseToDrag)

                '    ' выделить предыдущий элемент в листе в той же позиции как текущий элемент
                '    If (indexOfItemUnderMouseToDrag > 0) Then
                '        ListDragSource.SelectedIndex = indexOfItemUnderMouseToDrag - 1

                '    ElseIf (ListDragSource.Items.Count > 0) Then
                '        ' Selects the first item.
                '        ListDragSource.SelectedIndex = 0
                '    End If
                'End If

                ' Очистить курсор сразу же как необходимо.
                'If (Not MyNormalCursor Is Nothing) Then MyNormalCursor.Dispose()
                'If (Not MyNoDropCursor Is Nothing) Then MyNoDropCursor.Dispose()
                'End Try
            End If
        End If
    End Sub

    'Private Sub grdOutputChannel_GiveFeedback(sender As Object, e As System.Windows.Forms.GiveFeedbackEventArgs) Handles grdOutputChannel.GiveFeedback
    '    'Use custom cursors if the check box is checked.
    '    If (UseCustomCursorsCheckChecked) Then
    '        ' установить пользовательский курсор на основании эффекта.
    '        e.UseDefaultCursors = False
    '        If ((e.Effect And DragDropEffects.Move) = DragDropEffects.Move) Then
    '            Windows.Forms.Cursor.Current = MyNormalCursor
    '        Else
    '            Windows.Forms.Cursor.Current = MyNoDropCursor
    '        End If
    '    End If
    'End Sub

    Private Sub DataGridViewOutputChannel_QueryContinueDrag(sender As Object, e As QueryContinueDragEventArgs) Handles DataGridViewOutputChannel.QueryContinueDrag
        ' Прервать операцию drag, если мышь переместилась вне формы
        Dim dgView As DataGridView = CType(sender, DataGridView)
        If (dgView IsNot Nothing) Then
            Dim f As Form = dgView.FindForm()
            ' Прервать операцию drag, если мышь переместилась вне формы
            ' screenOffset даёт размеры рабочего стола, что выше и левее от стороны экрана
            If (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) OrElse
                ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) OrElse
                ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) OrElse
                ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom)) Then

                e.Action = DragAction.Cancel
            End If
        End If
    End Sub

#End Region

#Region "Channel(НомерСтенда) <=> ChannelN"
    ''' <summary>
    ''' Заполнить Отсоединённый Набор BaseChannels
    ''' Вначале копируется база Channel(НомерСтенда) на место ChannelN
    ''' затем создаётся подключение
    ''' </summary>
    ''' <param name="standNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateChannelsNBaseDataSet(standNumber As String)
        ' сделать типизированный TableAdapter, который заполняется по номеру стенда
        ' и имеет поиск канала по имени
        Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim tadleFrom As String = "Channel" & standNumber
        Dim strSQL As String

        cn.Open()
        If CheckExistTable(cn, tadleFrom) Then
            Try
                cmd.CommandType = CommandType.Text
                strSQL = $"DELETE * FROM {CHANNEL_N};"
                'strSQL = "DROP TABLE ChannelN;"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
                strSQL = $"INSERT INTO {CHANNEL_N} SELECT * FROM " & tadleFrom '& " IN " & """" & strПутьChannels & """" & ";"
                'strSQL = "SELECT " & tadleFrom & ".* INTO ChannelN FROM " & tadleFrom
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
                cn.Close()
                Thread.Sleep(500)
                Application.DoEvents()
            Catch ex As Exception
                cn.Close()
                Const caption As String = "Ошибка копирования данных."
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                Exit Sub
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
            End Try
        Else
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            Const caption As String = "Ошибка копирования данных."
            Dim text As String = $"Таблицы {tadleFrom} или {CHANNEL_N} не существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        ChannelNTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        ' данная строка кода позволяет загрузить данные в таблицу "ChannelsNBaseDataSet.ChannelN". При необходимости она может быть перемещена или удалена.
        ChannelNTableAdapter.Fill(ChannelsNBaseDataSet.ChannelN)
    End Sub

    ''' <summary>
    ''' Восстановить Настройки Всех Каналов Из BaseChannels
    ''' Типизированный Me.ChannelsNBaseDataSet.ChannelN от Channels(номер стенда) уже загружен в память
    ''' из него считываются настройки каналов и последовательно ищутся каналы в адаптере и корректируются  настройки допусков, ед. измерений и т.д.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RestoreAllChannelOutputMembersFromChannelsNBaseDataSet()
        For Each rowChannelN As ChannelsNDataSet.ChannelNRow In ChannelsNBaseDataSet.ChannelN.Rows
            For Each itemChannelOutput As ChannelOutput In OutputChannelsBindingList
                If rowChannelN.НаименованиеПараметра = itemChannelOutput.NameParameter Then
                    itemChannelOutput.UnitOfMeasure = ConvertUnitRegistrationToLMZ(rowChannelN.ЕдиницаИзмерения)
                    itemChannelOutput.LowerLimit = rowChannelN.ДопускМинимум
                    itemChannelOutput.UpperLimit = rowChannelN.ДопускМаксимум
                    itemChannelOutput.RangeYmin = rowChannelN.РазносУмин
                    itemChannelOutput.RangeYmax = rowChannelN.РазносУмакс
                    itemChannelOutput.AlarmValueMin = rowChannelN.АварийноеЗначениеМин
                    itemChannelOutput.AlarmValueMax = rowChannelN.АварийноеЗначениеМакс
                    itemChannelOutput.IsVisible = rowChannelN.Видимость
                    itemChannelOutput.IsVisibleRegistration = rowChannelN.ВидимостьРегистратор
                    itemChannelOutput.NumberFormula = rowChannelN.НомерФормулы
                    Exit For
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Восстановить Настройки Одного Канала Из BaseChannels
    ''' по имени clsChannelOutput.НаименованиеПараметра из Channels(номер стенда) считывается настройка канала
    ''' и производится модификация величин clsChannelOutput
    ''' сделана отсоединённая таблица из запроса чтобы операция происходила быстрее, а не обращалась каждый раз к базе
    ''' </summary>
    ''' <param name="clsChannelOutput"></param>
    ''' <remarks></remarks>
    Private Sub RestoreOneChannelOutputMembersFromChannelsNBaseDataSet(ByRef clsChannelOutput As ChannelOutput)
        Dim ChannelNTable As ChannelsNDataSet.ChannelNDataTable = ChannelNTableAdapter.GetDataByName(clsChannelOutput.NameParameter)

        If ChannelNTable.Rows.Count > 0 Then
            Dim rowChannelN As ChannelsNDataSet.ChannelNRow = CType(ChannelNTable.Rows(0), ChannelsNDataSet.ChannelNRow)
            clsChannelOutput.UnitOfMeasure = ConvertUnitRegistrationToLMZ(rowChannelN.ЕдиницаИзмерения)
            clsChannelOutput.LowerLimit = rowChannelN.ДопускМинимум
            clsChannelOutput.UpperLimit = rowChannelN.ДопускМаксимум
            clsChannelOutput.RangeYmin = rowChannelN.РазносУмин
            clsChannelOutput.RangeYmax = rowChannelN.РазносУмакс
            clsChannelOutput.AlarmValueMin = rowChannelN.АварийноеЗначениеМин
            clsChannelOutput.AlarmValueMax = rowChannelN.АварийноеЗначениеМакс
            clsChannelOutput.IsVisible = rowChannelN.Видимость
            clsChannelOutput.IsVisibleRegistration = rowChannelN.ВидимостьРегистратор
            clsChannelOutput.NumberFormula = rowChannelN.НомерФормулы
        End If
    End Sub

    ''' <summary>
    ''' Номер устройства можно положить из номера Группы
    ''' </summary>
    ''' <param name="nameGroupCh"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNumberDeviceFromSSDN(nameGroupCh As String) As String
        Dim numberDevice As String

        If nameGroupCh = "ИВК" Then
            numberDevice = "0"
        Else
            numberDevice = nameGroupCh.Substring("ССД".Length, nameGroupCh.Length - "ССД".Length)
        End If

        If Not dictModuleRegistration.ContainsKey(numberDevice) Then
            dictModuleRegistration.Add(numberDevice, New ModuleRegistration(numberDevice))
        Else
            dictModuleRegistration(numberDevice).AddChannels()
        End If

        lastModuleNumber = dictModuleRegistration(numberDevice).CountChannels

        Return numberDevice
    End Function

    ''' <summary>
    ''' Записать Изменения Конфигурации Каналов Channels
    ''' Запись конфигурации каналов из m_OutputChannelsBindingList в ChannelsN, а затем копирования её в таблицу Channels(номер стенда)
    ''' </summary>
    ''' <param name="standNumber"></param>
    ''' <remarks></remarks>
    Private Sub SaveChangeChannelStendAndChannelN(standNumber As String)
        ' с последней загруженной и редактируемой в памяти конфигурации берутся настройки допусков, ед. измерений и т.д.
        ' и сохраняются в отсоединённом наборе базы Me.ChannelsNBaseDataSet.ChannelN
        ' после прохождения по всем строкам и внесения изменений он сохраняется сам и так же как Channels(номер стенда)
        ' НаименованиеПараметра это Scr_Name
        ' НомерМодуляКорзины это номер GroupCh (ССД)
        ' Примечания это GroupCh
        Dim N As Integer = 1
        Const numberChannel As String = "0"
        'Dim НомерУстройства As String = "2" 'корзина - это номер ССД
        'Dim НомерМодуляКорзины As String = "1" 'Из пин взять номер
        'Dim НомерКаналаМодуля As String = "1" 'порядковый номер в данном модуле
        'Dim НомерФормулы As String = "2"
        'Dim ЕдиницаИзмерения As String 'сделать перевод 

        ' вначале очистить
        'Me.ChannelsNBaseDataSet.ChannelN.Rows.Clear() не работает DataRowState.Deleted
        For Each rowChannelN As ChannelsNDataSet.ChannelNRow In ChannelsNBaseDataSet.ChannelN.Rows
            rowChannelN.Delete()
        Next

        'Запись ТаблицыChannelN()
        dictModuleRegistration = New Dictionary(Of String, ModuleRegistration)

        For Each clsChannelOutput As ChannelOutput In OutputChannelsBindingList
            'Me._adapter.InsertCommand.CommandText = "INSERT INTO `ChannelN` (`НомерПараметра`, `НаименованиеПараметра`, `НомерКанала`," & _
            '   " `НомерУстройства`, `НомерМодуляКорзины`, `НомерКаналаМодуля`, `ТипПодключения`," & _
            '   " `НижнийПредел`, `ВерхнийПредел`, `ТипСигнала`, `НомерФормулы`, `СтепеньАппрокси" & _
            '   "мации`, `A0`, `A1`, `A2`, `A3`, `A4`, `A5`, `Смещение`, `КомпенсацияХС`, `Единиц" & _
            '   "аИзмерения`, `ДопускМинимум`, `ДопускМаксимум`, `РазносУмин`, `РазносУмакс`, `Ав" & _
            '   "арийноеЗначениеМин`, `АварийноеЗначениеМакс`, `Блокировка`, `Дата`, `Видимость`," & _
            '   " `ВидимостьРегистратор`, `Погрешность`, `Примечания`) VALUES (?, ?, ?, ?, ?, ?, " & _
            '   "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)" & _
            '   ""

            ChannelsNBaseDataSet.ChannelN.AddChannelNRow(НомерПараметра:=N,
                                                            НаименованиеПараметра:=If(clsChannelOutput.NameParameter.Length > 50, clsChannelOutput.NameParameter.Substring(0, 50), clsChannelOutput.NameParameter),
                                                            НомерКанала:=CInt(numberChannel),
                                                            НомерУстройства:=CInt(GetNumberDeviceFromSSDN(clsChannelOutput.GroupCh)),
                                                            НомерМодуляКорзины:="1",
                                                            НомерКаналаМодуля:=lastModuleNumber.ToString,
                                                            ТипПодключения:="DIFF",
                                                            НижнийПредел:=0,
                                                            ВерхнийПредел:=10,
                                                            ТипСигнала:="DC",
                                                            НомерФормулы:=clsChannelOutput.NumberFormula,
                                                            СтепеньАппроксимации:=1,
                                                            A0:=0,
                                                            A1:=1,
                                                            A2:=0,
                                                            A3:=0,
                                                            A4:=0,
                                                            A5:=0,
                                                            Смещение:=0,
                                                            КомпенсацияХС:=False,
                                                            ЕдиницаИзмерения:=ConvertUnitLMZtoRegistration(clsChannelOutput.UnitOfMeasure),
                                                            ДопускМинимум:=clsChannelOutput.LowerLimit,
                                                            ДопускМаксимум:=clsChannelOutput.UpperLimit,
                                                            РазносУмин:=CInt(clsChannelOutput.RangeYmin),
                                                            РазносУмакс:=CInt(clsChannelOutput.RangeYmax),
                                                            АварийноеЗначениеМин:=clsChannelOutput.AlarmValueMin,
                                                            АварийноеЗначениеМакс:=clsChannelOutput.AlarmValueMax,
                                                            Блокировка:=False,
                                                            Дата:=CDate("12.12.2012"),
                                                            Видимость:=clsChannelOutput.IsVisible,
                                                            ВидимостьРегистратор:=clsChannelOutput.IsVisibleRegistration,
                                                            Погрешность:=0,
                                                            Примечания:="<Канал: " & clsChannelOutput.Name & "> <Пин: " & clsChannelOutput.Pin & "> <Группа: " & clsChannelOutput.GroupCh & ">")
            N += 1
        Next

        SaveChangeChannelN()
        ChannelNTableAdapter.Fill(ChannelsNBaseDataSet.ChannelN)

        Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim tadleTo As String = "Channel" & standNumber
        Dim strSQL As String

        cn.Open()

        If CheckExistTable(cn, tadleTo) Then
            Try
                cmd.CommandType = CommandType.Text
                strSQL = "DELETE * FROM " & tadleTo 'не работает
                'strSQL = "DROP TABLE " & tadleTo
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

                strSQL = $"INSERT INTO {tadleTo} SELECT * FROM {CHANNEL_N}"   '& " IN " & """" & strПутьChannels & """" & ";"
                'strSQL = "SELECT ChannelN.* INTO " & tadleTo & " FROM ChannelN"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
                cn.Close()
                Thread.Sleep(500)
                Application.DoEvents()
            Catch ex As Exception
                cn.Close()
                Const caption As String = "Ошибка копирования данных."
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                Exit Sub
            Finally
                If cn.State = ConnectionState.Open Then
                    cn.Close()
                End If
            End Try
        Else
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If

            Const caption As String = "Ошибка копирования данных."
            Dim text As String = $"Таблицы {tadleTo} или {CHANNEL_N} не существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' Запись обновлений таблицы ChannelN через ChannelNTableAdapter
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveChangeChannelN()
        Validate()
        BindingSourceChannelNBase.EndEdit()

        Dim deletedRecords As ChannelsNDataSet.ChannelNDataTable = CType(ChannelsNBaseDataSet.ChannelN.GetChanges(DataRowState.Deleted), ChannelsNDataSet.ChannelNDataTable)
        Dim modifiedRecords As ChannelsNDataSet.ChannelNDataTable = CType(ChannelsNBaseDataSet.ChannelN.GetChanges(DataRowState.Modified), ChannelsNDataSet.ChannelNDataTable)
        Dim newRecords As ChannelsNDataSet.ChannelNDataTable = CType(ChannelsNBaseDataSet.ChannelN.GetChanges(DataRowState.Added), ChannelsNDataSet.ChannelNDataTable)

        With Me
            Try
                If deletedRecords IsNot Nothing Then .ChannelNTableAdapter.Update(deletedRecords)
                If modifiedRecords IsNot Nothing Then .ChannelNTableAdapter.Update(modifiedRecords)
                If newRecords IsNot Nothing Then .ChannelNTableAdapter.Update(newRecords)

                .ChannelsNBaseDataSet.AcceptChanges()
            Catch ex As Exception
                Dim caption As String = $"Процедура <{NameOf(SaveChangeChannelN)}> в {NameOf(FormConfigChannelServer)}"
                Dim text As String = $"Ошибка обновления таблицы ChannelN{vbCrLf}{ex}"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            Finally
                If deletedRecords IsNot Nothing Then deletedRecords.Dispose()
                If modifiedRecords IsNot Nothing Then modifiedRecords.Dispose()
                If newRecords IsNot Nothing Then newRecords.Dispose()
            End Try
        End With

        Thread.Sleep(200)
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Запись Конфигурации Режима
    ''' В таблицу Режимы записать конфигурационную строку с каналами, подлежащими сбору
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateTableRegime()
        Dim configuration As String = Nothing

        For Each rowChannelN As ChannelsNDataSet.ChannelNRow In ChannelsNBaseDataSet.ChannelN.Rows
            If configuration = vbNullString Then
                configuration = rowChannelN.НаименованиеПараметра
            Else
                configuration &= "\" & rowChannelN.НаименованиеПараметра
            End If
        Next

        configuration &= "\"

        If mParentForm.IsUseCalculationModule Then
            MainMdiForm.ModuleSolveManager.НастройкаКаналов()
            ' добавить все расчётные каналы в строку конфигурации
            If MainMdiForm.ModuleSolveManager.ДобавкаКонфигурацииТсрКлиента IsNot Nothing Then
                configuration &= MainMdiForm.ModuleSolveManager.ДобавкаКонфигурацииТсрКлиента
            End If
        End If

        ' теперь запись
        ' Номер Режима	Наименование	Перечень параметров	Текст справки
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = $"Update Режимы{StandNumber} SET ПереченьПараметров = '{configuration}' WHERE ([НомерРежима]= 1)"
            cn.Open()
            cmd.ExecuteNonQuery()
        End Using

        Thread.Sleep(200)
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Вывести окно сообщения по аналогии с окном MessageBox но без модальности
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFormMessageBox(message As String, caption As String) As FormMessageBox
        Dim mfrmMessageBox As FormMessageBox = New FormMessageBox(message, caption)

        mfrmMessageBox.Show()
        mfrmMessageBox.Activate()
        mfrmMessageBox.Refresh()

        Return mfrmMessageBox
    End Function

    ''' <summary>
    ''' Перевод Ед ЛМЗ в Ед Регистратора
    ''' Перевод более расширенные единиц измерения е единицы Регистратора "%";"дел";"мм";"градус";"Кгсм";"мм/с";"мкА";"кг/ч";"кгс" 
    ''' </summary>
    ''' <param name="inputUnit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertUnitLMZtoRegistration(inputUnit As String) As String
        Dim outUnit As String = "дел"
        ' не используется "мм/с"
        Select Case inputUnit
            Case ""
                outUnit = "дел"
                Exit Select
            Case "нет"
                outUnit = "дел"
                Exit Select
            Case "%"
                outUnit = "%"
                Exit Select
            Case "Деления"
                outUnit = "дел"
                Exit Select
            '******************
            Case "бар"
                outUnit = "мм"
                Exit Select
            Case "атм"
                outUnit = "мм"
                Exit Select
            Case "кПа"
                outUnit = "мм"
                Exit Select
            Case "мм"
                outUnit = "мм"
                Exit Select
            Case "мм.вод.ст"
                outUnit = "мм"
                Exit Select
            Case "мм.рт.ст"
                outUnit = "мм"
                Exit Select
            Case "Мпа"
                outUnit = "мм"
                Exit Select
            Case "Па"
                outUnit = "мм"
                Exit Select
            '******************
            Case "K"
                outUnit = "градус"
                Exit Select
            Case "град (рад)"
                outUnit = "градус"
                Exit Select
            Case "град С"
                outUnit = "градус"
                Exit Select
            '******************
            Case "дин/см^2"
                outUnit = "Кгсм"
                Exit Select
            Case "кгс/м^2"
                outUnit = "Кгсм"
                Exit Select
            Case "кгс/см^2"
                outUnit = "Кгсм"
                Exit Select
            Case "Н/см^2"
                outUnit = "Кгсм"
                Exit Select
            '******************
            Case "Вольт"
                outUnit = "мкА"
                Exit Select
            '******************
            Case "кг/кгс*час"
                outUnit = "кг/ч"
                Exit Select
            Case "кг/час"
                outUnit = "кг/ч"
                Exit Select
            '******************
            Case "кг/с"
                outUnit = "кгс"
                Exit Select
            Case "кгс"
                outUnit = "кгс"
                Exit Select
        End Select
        Return outUnit
    End Function

    ''' <summary>
    ''' Перевод Ед Регистратора в Ед ЛМЗ
    ''' Перевод единиц измерения Регистратора "%";"дел";"мм";"градус";"Кгсм";"мм/с";"мкА";"кг/ч";"кгс" в более расширенные
    ''' </summary>
    ''' <param name="inputUnit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertUnitRegistrationToLMZ(inputUnit As String) As String
        'Public arrРазмерности() As String = {"%", "дел", "мм", "градус", "Кгсм", "мм/с", "мкА", "кг/ч", "кгс"}
        '"%";"дел";"мм";"градус";"Кгсм";"мм/с";"мкА";"кг/ч";"кгс"
        Dim outUnit As String = "дел"
        Select Case inputUnit
            Case "%"
                outUnit = "%"
                Exit Select
            Case "дел"
                outUnit = "Деления"
                Exit Select
            Case "мм"
                outUnit = "мм"
                Exit Select
            Case "градус"
                outUnit = "град С"
                Exit Select
            Case "Кгсм"
                outUnit = "кгс/см^2"
                Exit Select
            Case "мм/с"
                outUnit = "мм"
                Exit Select
            Case "мкА"
                outUnit = "Вольт"
                Exit Select
            Case "кг/ч"
                outUnit = "кг/час"
                Exit Select
            Case "кгс"
                outUnit = "кгс"
                Exit Select
        End Select
        Return outUnit
    End Function
#End Region

    Private Sub ButtonUdateTCP_Click(sender As Object, e As EventArgs) Handles ButtonUdateTCP.Click
        ButtonSave.PerformClick()
        'UpdateAllWindowsWithServerChannels()
        ShowMessageOnStatusPanel("Произведено обновление списка кналов от Сервера в конфигурации программы.")
    End Sub

    'Private Shared Sub UpdateAllWindowsWithServerChannels()
    '    ' остановить и возобновить связь с сервером
    '    fMainForm.StartStopConnectionWithServer(False)
    '    Application.DoEvents()
    '    Thread.Sleep(2000)
    '    Application.DoEvents()
    '    ' пройти по всем окнам использующих каналы Сервера и в них произвести настройку или проверку каналов
    '    fMainForm.StartStopConnectionWithServer(True)
    'End Sub

    Private Sub ShowMessageOnStatusPanel(ByVal message As String)
        ToolStripStatusLabel.Text = message
        Const caption As String = "Сообщение в Окне Конфигуратор каналов Сервера"
        RegistrationEventLog.EventLog_MSG_USER_ACTION(String.Format("<{0}> {1}", caption, message))
    End Sub
End Class

#Region "Функция обратного вызова"

'''' <summary>
'''' функция обратного вызова из SyncSocketClient для отображения сообщений
'''' </summary>
'''' <param name="message"></param>
'''' <remarks></remarks>
'Public Sub AddListItemMethod(message As String)
'    If message = "" Then
'        LabelПериодЦикла.Text = fMainForm.mConnectionClient.Цикл_мсек
'        LabelПериодТаймера.Text = fMainForm.mConnectionClient.ЦиклТаймера_мсек
'        LabelЦикловОпроса.Text = fMainForm.mConnectionClient.ВсегоЦиклов
'        LabelЧастотаГц.Text = fMainForm.mConnectionClient.РеальнаяЧастотаГерц

'        '_DoubleValueList.Clear()
'        '_DoubleValueList.AddRange(Arr_DoubleValue)
'        Dim I As Integer
'        'For Each ItemDouble As Double In mSyncSocketClient.Arr_DoubleValue
'        '    I += 1
'        'Next
'        If fMainForm.mConnectionClient.AcquisitionDoubleValue IsNot Nothing Then
'            For I = 0 To fMainForm.mConnectionClient.AcquisitionDoubleValue.Length - 1
'                DoubleValueListG(I).ValueCh = fMainForm.mConnectionClient.AcquisitionDoubleValue(I)
'            Next
'        End If

'        If fMainForm.mConnectionClient.AcquisitionStrTimeValue IsNot Nothing Then
'            For I = 0 To fMainForm.mConnectionClient.AcquisitionStrTimeValue.Length - 1
'                StrTimeValueListG(I).ValueCh = fMainForm.mConnectionClient.AcquisitionStrTimeValue(I)
'            Next
'        End If

'        DGVDoubleValue.Refresh()
'        DGVStrTimeValue.Refresh()
'    Else
'        ListBoxConsole.Items.Add(message)
'        CheckConnection(fMainForm.mConnectionClient.СоединениеУстановлено)
'        RegistrationEventLog.EventLog_MSG_CONNECT(message)
'    End If
'    'Console.WriteLine(message)
'    'MessageBox.Show(message, "SyncSocketClient", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'End Sub

'Public Sub ThreadFunction(ByVal message As Object)
'    If mSyncSocketClient IsNot Nothing Then 'событие может вызываться от таймера из другого потока, а основной поток уже завершен
'        Dim myThreadClassObject As New MyThreadClass(Me, message) ' "Тест")
'        myThreadClassObject.Run()
'    End If
'End Sub

'Public Class MyThreadClass
'    Private mFormTcpClient As FormTcpClient
'    Private message As String

'    Public Sub New(myForm As FormTcpClient, message As String)
'        mFormTcpClient = myForm
'        Me.message = message
'    End Sub

'    Public Sub Run()
'        'Dim i As Integer
'        'For i = 1 To 5
'        '    message = "Step number " + i.ToString() + " executed"
'        '    Thread.Sleep(400)
'        '    ' выполнить специализированный делегат в потоке окна содержащего контрол
'        '    ' с специализированными аргументами
'        mFormTcpClient.Invoke(mFormTcpClient.myDelegate, New Object() {message})
'        'Next i
'    End Sub
'End Class

#End Region

#Region "Тесты"
'Private DataSetChannel As DataSet
'Private ChannelTableAdapter2 As OleDbDataAdapter

''Private Sub ButtonLoad_Click(sender As System.Object, e As System.EventArgs) Handles ButtonLoad.Click
''    'ЗаполнитьТаблицуИзБазы()
''    ЗаполнитьТаблицуВремени()
''End Sub

'Private Sub ЗаполнитьТаблицуВремени()
'    'здесь не типизированный датасет
'    Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(strProviderJet, strПутьChannels_cfg_lmz))
'    Dim dtChannels As DataTable
'    Dim strSQL As String

'    dtChannels = New DataTable
'    Try
'        strSQL = "SELECT * FROM TimeChannel ORDER BY id"
'        cn.Open()
'        DataSetChannel = New DataSet

'        ChannelTableAdapter2 = New OleDbDataAdapter(strSQL, cn)
'        ChannelTableAdapter2.Fill(DataSetChannel, "TimeChannel")
'        dtChannels = DataSetChannel.Tables("TimeChannel")

'        'If dtИзмеренные.Rows.Count > 0 AndAlso dtПриведенные.Rows.Count > 0 AndAlso dtПриведенные.Rows.Count > 0 Then
'        '    ОчиститьРасчетныеТаблицы()
'        'End If

'        'связывание
'        'ChannelBindingSource.DataSource = dtChannels
'        'grdInputChannel.DataSource = ChannelBindingSource

'        grdTimeChannel.AutoGenerateColumns = True
'        grdTimeChannel.DataSource = DataSetChannel 'dtChannels
'        grdTimeChannel.DataMember = "TimeChannel"

'        'DataGridViewChannels.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
'        'DataGridViewChannels.Columns(0).FillWeight = 10
'        'DataGridViewChannels.Columns(0).Width = 10
'        'DataGridViewChannels.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

'        grdTimeChannel.Refresh()
'        'AllowSelectionChanged = True
'        'If dtИзмеренные.Rows.Count > 0 Then 'AndAlso TableStyles(ИзмеренныеПараметры) = False
'        '    ДобавитьСтиль(cn, DataGridViewChannels, strSQLИзмеренные)
'        '    'TableStyles(ИзмеренныеПараметры) = True
'        'End If

'        cn.Close()
'    Catch ex As Exception
'        MessageBox.Show(ex.ToString, "Процедура DataGrid5НомерЗапуска_CurrentRowChanged", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    Finally
'        If cn.State = ConnectionState.Open Then
'            cn.Close()
'        End If
'        'AllowSelectionChanged = True
'    End Try
'End Sub

'Private Sub ЗаполнитьТаблицуИзБазы()
'    'здесь не типизированный датасет
'    Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(strProviderJet, strПутьChannels_cfg_lmz))
'    Dim dtChannels As DataTable
'    Dim strSQL As String

'    dtChannels = New DataTable
'    Try
'        strSQL = "SELECT * FROM " & conNameTableChannel & " ORDER BY id"

'        cn.Open()
'        DataSetChannel = New DataSet

'        ChannelTableAdapter2 = New OleDbDataAdapter(strSQL, cn)
'        ChannelTableAdapter2.Fill(DataSetChannel, conNameTableChannel)
'        dtChannels = DataSetChannel.Tables(conNameTableChannel)
'        'If dtИзмеренные.Rows.Count > 0 AndAlso dtПриведенные.Rows.Count > 0 AndAlso dtПриведенные.Rows.Count > 0 Then
'        '    ОчиститьРасчетныеТаблицы()
'        'End If
'        'связывание
'        ChannelBindingSource.DataSource = dtChannels
'        grdInputChannel.DataSource = ChannelBindingSource

'        'DataGridViewChannels.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
'        'DataGridViewChannels.Columns(0).FillWeight = 10
'        'DataGridViewChannels.Columns(0).Width = 10
'        'DataGridViewChannels.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

'        grdInputChannel.Refresh()

'        'AllowSelectionChanged = True
'        'If dtИзмеренные.Rows.Count > 0 Then 'AndAlso TableStyles(ИзмеренныеПараметры) = False
'        '    ДобавитьСтиль(cn, DataGridViewChannels, strSQLИзмеренные)
'        '    'TableStyles(ИзмеренныеПараметры) = True
'        'End If
'        cn.Close()
'    Catch ex As Exception
'        MessageBox.Show(ex.ToString, "Процедура DataGrid5НомерЗапуска_CurrentRowChanged", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    Finally
'        If cn.State = ConnectionState.Open Then
'            cn.Close()
'        End If
'        'AllowSelectionChanged = True
'    End Try
'End Sub

'Private Sub ЗаполнитьТаблицуИзXML()
'    'здесь таблица никак не связана с базой
'    Dim dtChannels As DataTable = ПолучитьКаналыКонфигуратораСервера(PathServerCfglmzXml)
'    'связывание
'    ChannelBindingSource.DataSource = dtChannels
'    grdInputChannel.DataSource = ChannelBindingSource

'    grdInputChannel.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
'    grdInputChannel.Columns(0).FillWeight = 10
'    grdInputChannel.Columns(0).Width = 10
'    grdInputChannel.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

'    grdInputChannel.Refresh()
'End Sub

''Private Sub СоздатьГруппы()
''    'dim varИзмеренныеПараметры As ChannelsDataSet.
''    Dim _Table As Channels_cfg_lmzDataSet.ChannelDataTable = Me.ChannelsDataSet.Channel
''    Dim I As Integer
''    Dim Number As Integer
''    Dim strNumber As String
''    Const conTime As String = "TIME"
''    Dim strArr(_Table.Rows.Count - 1) As String

''    'Try

''    I = 0
''    For Each rowChannel As Channels_cfg_lmzDataSet.ChannelRow In _Table.Rows

''        If Not IsDBNull(rowChannel("Pin")) Then
''            'If rowChannel.Pin IsNot Nothing Then
''            Number = CInt(Val(rowChannel.Pin.Substring(0, 2))) 'первые 2 символа
''            strNumber = Number.ToString
''            If rowChannel.Pin.Substring(strNumber.Length, conTime.Length).ToUpper = conTime Then
''                'номер
''                strArr(I) = strNumber
''            Else
''                'взять Pin
''                strArr(I) = rowChannel.Pin
''            End If
''        Else
''            strArr(I) = ""
''        End If

''        If strArr(I) = "" Then
''            strArr(I) = "ИВК"
''        Else
''            Number = CInt(Val(strArr(I).Substring(0, 1)))
''            strNumber = Number.ToString
''            If rowChannel.Pin.Substring(strNumber.Length, 1).ToUpper = "T" Then
''                strArr(I) = "ССД1"
''            Else
''                Number = CInt(Val(strArr(I).Substring(0, 2)))
''                strNumber = Number.ToString
''                strArr(I) = "ССД" & strNumber
''            End If
''        End If
''        rowChannel("Pin") = strArr(I)
''        I += 1
''    Next

''    'не срабатывает
''    _Table.AcceptChanges()
''    Me.ChannelsDataSet.AcceptChanges()
''    grdInputChannel.Refresh()

''    'Catch ex As Exception
''    '    MessageBox.Show(ex.ToString, "Ошибка при СоздатьГруппы", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    'Finally
''    'End Try
''    Stop

''End Sub
#End Region

'Private _clientSocket As Socket
'Public Property _Explorer As TcpClientForm

'Public Sub ЗаполнитьДействиеTableAdapter(ByVal keyКонфигурацияДействияИзОбновления As Integer)
'    Me.BindingSourceДействие.DataSource = ChannelsDigitalOutputDataSet
'    Me.BindingSourceДействие.DataMember = ChannelsDigitalOutputDataSet.Действие.ToString
'    If keyКонфигурацияДействияИзОбновления > 0 Then
'        ДействиеTableAdapter.FillBykeyКонфигурацияДействия(ChannelsDigitalOutputDataSet.Действие, keyКонфигурацияДействияИзОбновления)
'    End If
'End Sub


'Private Sub Explorer1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    'Настройте интерфейс пользователя
'    SetUpListViewColumns()
'    LoadTree()
'End Sub

'Private Sub LoadTree()
'    ' TODO: написать код добавления элементов в дерево

'    Dim tvRoot As TreeNode
'    Dim tvNode As TreeNode

'    tvRoot = Me.TreeView.Nodes.Add("Root")
'    tvNode = tvRoot.Nodes.Add("TreeItem1")
'    tvNode = tvRoot.Nodes.Add("TreeItem2")
'    tvNode = tvRoot.Nodes.Add("TreeItem3")
'End Sub

'Private Sub LoadListView()
'    ' TODO: Добавить код для добавления элементов в список в соответствии с выбранным узлом дерева

'    Dim lvItem As ListViewItem
'    ListView.Items.Clear()

'    lvItem = ListView.Items.Add("ListViewItem1")
'    lvItem.ImageKey = "Graph1"
'    lvItem.SubItems.AddRange(New String() {"Столбец2", "Столбец3"})

'    lvItem = ListView.Items.Add("ListViewItem2")
'    lvItem.ImageKey = "Graph2"
'    lvItem.SubItems.AddRange(New String() {"Столбец2", "Столбец3"})

'    lvItem = ListView.Items.Add("ListViewItem3")
'    lvItem.ImageKey = "Graph3"
'    lvItem.SubItems.AddRange(New String() {"Столбец2", "Столбец3"})
'End Sub

'Private Sub SetUpListViewColumns()

'    ' TODO: добавить код для настройки столбцов списка
'    Dim lvColumnHeader As ColumnHeader

'    ' Установка ширины столбцов действует только для текущего представления, поэтому в этой строке
'    '  список явным образом переключается в режим "Маленькие значки"
'    '  перед установкой ширины столбца
'    SetView(View.SmallIcon)

'    lvColumnHeader = ListView.Columns.Add("Столбец1")
'    ' Установить достаточную ширину столбцов в режиме "Маленькие значки", чтобы элементы
'    '  не накладывались друг на друга
'    ' Обратите внимание, что второй и третий столбцы не видны в режиме "Маленькие значки",
'    '  поэтому настраивать их необязательно
'    lvColumnHeader.Width = 90

'    ' Переключить отображение в режим "Таблица" и установить соответствующую
'    '  ширину столбцов, отличную от ширины столбцов в режиме "Маленькие значки"
'    SetView(View.Details)

'    ' В режиме "Таблица" первый столбец должен быть несколько шире, чем
'    '  в режиме "Маленькие значки", а для Столбец2 и Столбец3 в этом режиме
'    '  размеры надо указать явно
'    lvColumnHeader.Width = 100

'    lvColumnHeader = ListView.Columns.Add("Столбец2")
'    lvColumnHeader.Width = 70

'    lvColumnHeader = ListView.Columns.Add("Столбец3")
'    lvColumnHeader.Width = 70

'End Sub

'Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
'    'Закрыть эту форму
'    Me.Close()
'End Sub

'Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolBarToolStripMenuItem.Click
'    'Переключить отображение полоски инструментов и отметку на соответствующем пункте меню
'    ToolBarToolStripMenuItem.Checked = Not ToolBarToolStripMenuItem.Checked
'    ToolStrip.Visible = ToolBarToolStripMenuItem.Checked
'End Sub

'Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBarToolStripMenuItem.Click
'    'Переключить отображение полоски состояния и отметку на соответствующем пункте меню
'    StatusBarToolStripMenuItem.Checked = Not StatusBarToolStripMenuItem.Checked
'    StatusStrip.Visible = StatusBarToolStripMenuItem.Checked
'End Sub

''Включить или выключить отображение области папок
'Private Sub ToggleFoldersVisible()
'    'Сначала переключите состояние отметки для соответствующего пункта меню
'    FoldersToolStripMenuItem.Checked = Not FoldersToolStripMenuItem.Checked

'    'Синхронизировать кнопку "Папки" на панели инструментов
'    FoldersToolStripButton.Checked = FoldersToolStripMenuItem.Checked

'    ' Свернуть панель, на которой содержится дерево.
'    Me.SplitContainer.Panel1Collapsed = Not FoldersToolStripMenuItem.Checked
'End Sub

'Private Sub FoldersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FoldersToolStripMenuItem.Click
'    ToggleFoldersVisible()
'End Sub

'Private Sub FoldersToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FoldersToolStripButton.Click
'    ToggleFoldersVisible()
'End Sub

'Private Sub SetView(ByVal View As System.Windows.Forms.View)
'    'Определить, какой пункт меню должен быть отмечен
'    Dim MenuItemToCheck As ToolStripMenuItem = Nothing
'    Select Case View
'        Case View.Details
'            MenuItemToCheck = DetailsToolStripMenuItem
'        Case View.LargeIcon
'            MenuItemToCheck = LargeIconsToolStripMenuItem
'        Case View.List
'            MenuItemToCheck = ListToolStripMenuItem
'        Case View.SmallIcon
'            MenuItemToCheck = SmallIconsToolStripMenuItem
'        Case View.Tile
'            MenuItemToCheck = TileToolStripMenuItem
'        Case Else
'            Debug.Fail("Unexpected View")
'            View = View.Details
'            MenuItemToCheck = DetailsToolStripMenuItem
'    End Select

'    'В меню "Представления" выбрать нужный пункт меню и отменить выбор остальных пунктов
'    For Each MenuItem As ToolStripMenuItem In ListViewToolStripButton.DropDownItems
'        If MenuItem Is MenuItemToCheck Then
'            MenuItem.Checked = True
'        Else
'            MenuItem.Checked = False
'        End If
'    Next

'    'В конце установить запрошенное представление
'    ListView.View = View
'End Sub

'Private Sub ListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListToolStripMenuItem.Click
'    SetView(View.List)
'End Sub

'Private Sub DetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailsToolStripMenuItem.Click
'    SetView(View.Details)
'End Sub

'Private Sub LargeIconsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LargeIconsToolStripMenuItem.Click
'    SetView(View.LargeIcon)
'End Sub

'Private Sub SmallIconsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmallIconsToolStripMenuItem.Click
'    SetView(View.SmallIcon)
'End Sub

'Private Sub TileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TileToolStripMenuItem.Click
'    SetView(View.Tile)
'End Sub

'Private Sub OpenToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
'    Dim OpenFileDialog As New OpenFileDialog
'    OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
'    OpenFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt"
'    OpenFileDialog.ShowDialog(Me)

'    Dim FileName As String = OpenFileDialog.FileName
'    ' TODO: добавить код открытия файла
'End Sub

'Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
'    Dim SaveFileDialog As New SaveFileDialog
'    SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
'    SaveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt"
'    SaveFileDialog.ShowDialog(Me)

'    Dim FileName As String = SaveFileDialog.FileName
'    ' TODO: добавить код для сохранения содержимого формы в файл.
'End Sub

'Private Sub TreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView.AfterSelect
'    ' TODO: добавить код для изменения содержимого списка в соответствии с выбранным узлом дерева
'    LoadListView()
'End Sub


''алгоритма приема данных от сервера (описанние программиста ИНСИС) 
'Private Sub GetParList_Insys_sbk_kdg1(Число_каналов_данных As Integer, Число_каналов_времени As Integer, Индекс1каналаВремени As Integer)
'    'входы:
'    'Индекс 1-го канала времени
'    'Число_каналов_данных
'    'Число_каналов_времени
'    '
'    'выход:
'    'ArrStrВремя
'    'Arr_DoubleValue
'    '
'    '
'    '
'    '
'    '
'    Dim ВсегоКаналов As Integer = Число_каналов_данных + Число_каналов_времени
'    Dim Индекс1каналаВремениPlus2 = Индекс1каналаВремени + 2
'    Dim I As Integer
'    Dim Buffer() As Byte
'    Dim CMD As Integer
'    Dim BytesToRead As Integer
'    Dim Value As Double
'    Dim ValueTime As String

'    Try

'        Do While I < ВсегоКаналов
'            Buffer = New Byte(9) {} '-заголовок микро-пакета (постоянная часть, 10 байт)
'            _clientSocket.ReceiveBufferSize = Buffer.Length
'            _clientSocket.Receive(Buffer) ',0, Buffer.Length, SocketFlags.None,new AsyncCallback(ReceiveCallback),connection)
'            'Поле Command (1 байт) содержит информацию о типе передаваемых данных в микро-пакете. 
'            CMD = Buffer(0) ' BitConverter.ToInt32(Buffer, 0)
'            Select Case CMD
'                Case Command._Default_0
'                    BytesToRead = 0 ' будет ошибка создания и чтения
'                Case Command.Byte_1
'                    BytesToRead = 1
'                Case Command.Integer_2
'                    BytesToRead = 4
'                Case Command.Double_3
'                    BytesToRead = 8
'                Case Command.Anything_4
'                    BytesToRead = 4
'                Case Command.SmallString_51
'                    Buffer = New Byte(0) {}
'                    _clientSocket.ReceiveBufferSize = Buffer.Length
'                    _clientSocket.Receive(Buffer)
'                    BytesToRead = Buffer(0) 'Поле Length (1 байт) содержит информацию о длине стоки, передаваемой в микро-пакете.
'                Case Command.ChannelProperty_53
'                    Buffer = New Byte(3) {} 'Property hash (4 байта) содержит код наименования свойства канала. Тип поля данных Property hash – Unsigned Integer 
'                    _clientSocket.ReceiveBufferSize = Buffer.Length
'                    _clientSocket.Receive(Buffer)

'                    Buffer = New Byte(1) {} 'Поле Length (2 байта) содержит информацию о длине значения свойства канала (строки), передаваемого в микро-пакете.
'                    _clientSocket.ReceiveBufferSize = Buffer.Length
'                    _clientSocket.Receive(Buffer)
'                    BytesToRead = BitConverter.ToInt16(Buffer, 0)
'            End Select

'            Buffer = New Byte(BytesToRead - 1) {}
'            _clientSocket.ReceiveBufferSize = Buffer.Length
'            _clientSocket.Receive(Buffer)

'            Select Case CMD
'                Case Command._Default_0
'                    'ни чего не делать
'                Case Command.Byte_1
'                    Value = Buffer(0)
'                    'накопить буфер Arr_DoubleValue
'                Case Command.Integer_2
'                    Value = BitConverter.ToInt32(Buffer, 0)
'                    'накопить буфер Arr_DoubleValue

'                Case Command.Double_3
'                    Value = BitConverter.ToDouble(Buffer, 0)
'                    'накопить буфер Arr_DoubleValue

'                    'Case Command.Anything_4
'                Case Command.SmallString_51, Command.ChannelProperty_53
'                    Dim Encoding As New System.Text.ASCIIEncoding 'UTF8Encoding
'                    ValueTime = Encoding.GetString(Buffer)
'                    If I < Число_каналов_времени Then
'                        'обработать канал времени

'                        'накопить буфер ArrStrВремя
'                    Else
'                        Dim ОбработатьКакВремя As Boolean = I >= Индекс1каналаВремени AndAlso I <= Индекс1каналаВремениPlus2
'                        If ОбработатьКакВремя Then
'                            ValueTime = ValueTime.Substring(11, 11)
'                            Dim v As DateTime = DateTime.Parse(ValueTime)
'                            Value = v.Hour * 3600.0 + v.Minute * 60.0 + v.Second
'                        Else
'                            Value = Double.Parse(ValueTime)
'                        End If
'                        'накопить буфер Arr_DoubleValue
'                    End If
'            End Select

'            If CMD <> Command.Anything_4 Then
'                I += 1
'            End If
'        Loop


'    Catch exc As SocketException
'        _clientSocket.Close()
'        Console.WriteLine("Socket exception: " + exc.SocketErrorCode)
'    Catch exc As Exception
'        _clientSocket.Close()
'        Console.WriteLine("Exception: " & Convert.ToString(exc))
'    End Try

'End Sub



''алгоритма приема данных от сервера (одиночный запрос для сырых данных) 
'Private Function GetPropStr() As String
'    'входы:
'    'Это чистый получатель запрошенных свойств
'    '
'    'выход:
'    'ValueOut -свойство

'    Dim Buffer() As Byte
'    Dim CMD As Integer
'    Dim BytesToRead As Integer
'    Dim ValueOut As String = ""


'    Try

'        Buffer = New Byte(9) {} '-заголовок микро-пакета (постоянная часть, 10 байт)
'        _clientSocket.ReceiveBufferSize = Buffer.Length
'        _clientSocket.Receive(Buffer) ',0, Buffer.Length, SocketFlags.None,new AsyncCallback(ReceiveCallback),connection)
'        'Поле Command (1 байт) содержит информацию о типе передаваемых данных в микро-пакете. 
'        CMD = Buffer(0) ' BitConverter.ToInt32(Buffer, 0)
'        Select Case CMD
'            Case Command._Default_0
'                BytesToRead = 0 ' будет ошибка создания и чтения
'            Case Command.Byte_1
'                BytesToRead = 1
'            Case Command.Integer_2
'                BytesToRead = 4
'            Case Command.Double_3
'                BytesToRead = 8
'            Case Command.Anything_4
'                BytesToRead = 4
'            Case Command.SmallString_51
'                Buffer = New Byte(0) {}
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                BytesToRead = Buffer(0) 'Поле Length (1 байт) содержит информацию о длине стоки, передаваемой в микро-пакете.
'            Case Command.ChannelProperty_53
'                Buffer = New Byte(3) {} 'Property hash (4 байта) содержит код наименования свойства канала. Тип поля данных Property hash – Unsigned Integer 
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                'Получить Хэш PropHash= BitConverter.ToInt32(Buffer, 0)


'                Buffer = New Byte(1) {} 'Поле Length (2 байта) содержит информацию о длине значения свойства канала (строки), передаваемого в микро-пакете.
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                BytesToRead = BitConverter.ToInt16(Buffer, 0)
'        End Select

'        Buffer = New Byte(BytesToRead - 1) {}
'        _clientSocket.ReceiveBufferSize = Buffer.Length
'        _clientSocket.Receive(Buffer)

'        Select Case CMD
'            Case Command._Default_0
'                ValueOut = "error"
'                'Case Command.Byte_1
'                '    Value = Buffer(0)
'                'Case Command.Integer_2
'                '    Value = BitConverter.ToInt32(Buffer, 0)
'                'Case Command.Double_3
'                '    Value = BitConverter.ToDouble(Buffer, 0)

'                'Case Command.Anything_4
'            Case Command.SmallString_51, Command.ChannelProperty_53
'                Dim Encoding As New System.Text.ASCIIEncoding 'System.Text.UTF8Encoding
'                ValueOut = Encoding.GetString(Buffer)
'                'ValueOut = System.Text.Encoding.ASCII.GetString(Buffer, 0, Buffer.Length)

'        End Select


'    Catch exc As SocketException
'        _clientSocket.Close()
'        Console.WriteLine("Socket exception: " + exc.SocketErrorCode)
'        'установить GlobTCP_Insys.Mode=Write
'    Catch exc As Exception
'        _clientSocket.Close()
'        Console.WriteLine("Exception: " & Convert.ToString(exc))
'        'установить GlobTCP_Insys.Mode=Write

'        'Finally
'    End Try

'    Return ValueOut

'End Function



''алгоритма приема данных от сервера (одиночный запрос для сырых данных) 
'Private Function GetProp() As Double
'    'входы:
'    'Это чистый получатель запрошенных свойств
'    '
'    'выход:
'    'ValueOut -свойство

'    Dim Buffer() As Byte
'    Dim CMD As Integer
'    Dim BytesToRead As Integer
'    Dim ValueOut As Double = Double.NaN


'    Try

'        Buffer = New Byte(9) {} '-заголовок микро-пакета (постоянная часть, 10 байт)
'        _clientSocket.ReceiveBufferSize = Buffer.Length
'        _clientSocket.Receive(Buffer) ',0, Buffer.Length, SocketFlags.None,new AsyncCallback(ReceiveCallback),connection)
'        'Поле Command (1 байт) содержит информацию о типе передаваемых данных в микро-пакете. 
'        CMD = Buffer(0) ' BitConverter.ToInt32(Buffer, 0)
'        Select Case CMD
'            Case Command._Default_0
'                BytesToRead = 0 ' будет ошибка создания и чтения
'            Case Command.Byte_1
'                BytesToRead = 1
'            Case Command.Integer_2
'                BytesToRead = 4
'            Case Command.Double_3
'                BytesToRead = 8
'            Case Command.Anything_4
'                BytesToRead = 4
'            Case Command.SmallString_51
'                Buffer = New Byte(0) {}
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                BytesToRead = Buffer(0) 'Поле Length (1 байт) содержит информацию о длине стоки, передаваемой в микро-пакете.
'            Case Command.ChannelProperty_53
'                Buffer = New Byte(3) {} 'Property hash (4 байта) содержит код наименования свойства канала. Тип поля данных Property hash – Unsigned Integer 
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                'Получить Хэш PropHash= BitConverter.ToInt32(Buffer, 0)


'                Buffer = New Byte(1) {} 'Поле Length (2 байта) содержит информацию о длине значения свойства канала (строки), передаваемого в микро-пакете.
'                _clientSocket.ReceiveBufferSize = Buffer.Length
'                _clientSocket.Receive(Buffer)
'                BytesToRead = BitConverter.ToInt16(Buffer, 0)
'        End Select

'        Buffer = New Byte(BytesToRead - 1) {}
'        _clientSocket.ReceiveBufferSize = Buffer.Length
'        _clientSocket.Receive(Buffer)

'        Select Case CMD
'            Case Command._Default_0
'                ValueOut = Double.NaN
'            Case Command.Byte_1
'                ValueOut = Buffer(0)
'            Case Command.Integer_2
'                ValueOut = BitConverter.ToInt32(Buffer, 0)
'            Case Command.Double_3
'                ValueOut = BitConverter.ToDouble(Buffer, 0)
'            Case Command.Anything_4 'пакет 4
'                ValueOut = Double.NaN

'            Case Command.SmallString_51, Command.ChannelProperty_53
'                Dim Encoding As New System.Text.ASCIIEncoding 'System.Text.UTF8Encoding
'                Dim strValueBuffer = Encoding.GetString(Buffer)
'                'ValueOut = System.Text.Encoding.ASCII.GetString(Buffer, 0, Buffer.Length)

'                If Not Double.TryParse(strValueBuffer, ValueOut) Then
'                    ValueOut = Double.NaN
'                End If
'        End Select
'    Catch exc As SocketException
'        _clientSocket.Close()
'        Console.WriteLine("Socket exception: " + exc.SocketErrorCode)
'        'установить GlobTCP_Insys.Mode=Write
'    Catch exc As Exception
'        _clientSocket.Close()
'        Console.WriteLine("Exception: " & Convert.ToString(exc))
'        'установить GlobTCP_Insys.Mode=Write

'        'Finally
'    End Try
'    Return ValueOut
'End Function

'Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
'    'If _Explorer Is Nothing Then
'    '    _Explorer = New Explorer1(Me)
'    '    _Explorer.Show()
'    'End If
'    tect()
'End Sub

'Private Sub tect()
'    Dim _Byte As Byte = 51
'    Dim Buffer() As Byte
'    Dim НаименованиеКоманды As Integer
'    Dim Value As Double
'    Dim BytesToRead As Integer
'    НаименованиеКоманды = _Byte


'    BytesToRead = 4
'    'Buffer = New Byte(BytesToRead - 1) {}
'    'Buffer(0) = 51
'    'Buffer(1) = 51
'    'Buffer(2) = 51
'    'Buffer(3) = 51
'    'Value = BitConverter.ToDouble(Buffer, 0)

'    Buffer = BitConverter.GetBytes(12345.55)
'    'Array.Reverse(arrBytes)
'    'преобразовать массив байт в UInt32
'    Value = BitConverter.ToDouble(Buffer, 0)

'    'BytesToDoubleDemo.Main()

'    Dim CMD As Integer = 3 ' 1 '2 3
'    Select Case CMD
'        Case Command._Default_0
'        Case Command.Byte_1
'            BytesToRead = 1
'            Buffer = New Byte(BytesToRead - 1) {}
'            Buffer(0) = 1
'            Value = Buffer(0)
'        Case Command.Integer_2
'            BytesToRead = 4
'            Buffer = New Byte(BytesToRead - 1) {}
'            Buffer(0) = 2
'            Value = BitConverter.ToInt32(Buffer, 0)
'        Case Command.Double_3
'            BytesToRead = 8
'            Buffer = New Byte(BytesToRead - 1) {}
'            Buffer(0) = 3
'            Value = BitConverter.ToDouble(Buffer, 0)
'        Case Command.SmallString_51, Command.ChannelProperty_53

'    End Select

'    Console.WriteLine(Value)

'End Sub



''Для инициализации серверного сокета нужно создать сокет, привязать его к адресу и настроить очередь прослушивания запросов.
'Private Sub SetupServerSocket()


'    ' Получаем информацию о локальном компьютере
'    'Метод Dns.GetHostEntry возвращает объект IPHostEntry, содержащий соответствующую информацию о компьютере и позволяющий узнать все IP-адреса компьютера, которые известны DNS-серверу. Для упрощения в этой статье я буду использовать первый адрес из списка.
'    Dim localMachineInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
'    'Объект IPEndPoint является логическим представлением конечной точки при сетевой коммуникации и включает данные об IP-адресе и порте. 
'    Dim myEndpoint As New IPEndPoint(localMachineInfo.AddressList(0), port)

'    ' Создаем сокет, привязываем его к адресу
'    ' и начинаем прослушивание
'    'Чтобы настроить серверный TCP-сокет в управляемом коде, прежде всего нужно создать экземпляр класса Socket. Его конструктор принимает три параметра: AddressFamily, SocketType и ProtocolType. 
'    _clientSocket = New Socket(myEndpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
'    'Как только сокет создан, его можно привязать к адресу. Привязка клиентского сокета к адресу не обязательна, но в случае серверных сокетов она необходима. Чтобы привязать сокет к адресу, вызовите метод Bind объекта Socket. Этому методу нужен адрес и порт, которые будут сопоставлены с сокетом, поэтому в качестве параметра он принимает экземпляр класса, производного от EndPoint. 
'    _clientSocket.Bind(myEndpoint)
'    'После вызова метода Bind метод Socket.Listen конфигурирует для сокета внутренние очереди. Когда клиент пытается подключиться к серверу, в очередь помещается запрос соединения. Метод Listen принимает один аргумент — максимальное число запросов соединений, которые могут находиться в очереди. 
'    _clientSocket.Listen(CInt(SocketOptionName.MaxConnections))
'End Sub



'считать в текстовое поле (Путь файла конфигурации ИВК) из глобальных настроек 
'в цикле с подсчетом перезапуска
'если панель не закрывается, то работает перезапуск TCP соединения 
'проверка значения кластера "Сведения" об выходе
'инициализировать индикатор приема
'инициализировать листбокс расчётные значения (против сырые значения)
'кнопка Стоп=False
'обнулить массив с данными
'проверить наличие конфигурационного файла на диске
'если файл не найден то выдать сообщение и завершить работу
'*********************************************************
'блок ReadCFG
'если нет ошибок то
'выделить путь из строки конфигурации CFG path 
'из DLL выход кластера его разделить на 2 строки, затем вторую строку преобразовать в число, и сново соединить 
'- на индикаторе Server Out будет IP сервера и его порт
'на другом индикаторе (Channels 2) будут все каналы (проверить)
'если нет ошибок то в цикле разбор этого массива кластера для выделения из строк массива полиномов  и т.д.
'Parser.dll вызов void  readCFG(const LStrHandle path, void *server, void *Channels, void *error);
'подготовить запрос с массивом(Channels) имен атрибутов
'1 параметр
'CFG path
'2 параметр
'String
'String 2
'3 параметр
'1 Name
'2 Scr_Name
'3 Scr_EdIzm
'4 Cal_Polynome - развернуть в массив
'5 Ch_Unit
'6 Pin
'7 Raw_Unit
'8 Tare_Polynome - развернуть в массив
'9 Polynome - развернуть в массив
'
'выход
'1 параметр(server out) String IP адрес сервера, String преобразовать в число - номер порта
'2 параметр кластер
'число каналов такое же как на левом списке

'******************************************
'        '0 Sequence****************************************

'если конфигурация считалась успешно
'идёт открытие TCP соединения в Connection_arm_Insys_sbk
'на входе Server host(String IP) и port
'GlobTCP_Insys.vi - подготовка соединения (на запись)
'постоянно крутится цикл с частотой 1 сек попытка установки связи TCP контрола
'в цикле производится чтение параметров из GlobTCP_Insys.vi и если связь устанолена ни чего
'если связи нет то заново открывается соединение и если нет ошибки то в GlobTCP_Insys.vi производится запись
'если ошибка соединение закрывается
'после установки связи выход
'отметить индикатор установления связи (ошибка приема)
'отметить поле Обновлений буфера
'инициализировать листбокс расчётные значения (против сырые значения)
'сделать доступным кнопку "Стоп"
'вызывается модуль инициализации констант (№ стенда, № изделия, № двигателя настройки расч. части и положения кнопок)
'инициализируется индикатор времени
'обнуляются таблицы выбранных каналов и каналов времени и табл. данных

'********************************************************
'        '1 Sequence****************************************
'Функция "Читать файл network_start"
'считываются переменные Автостарт, Предупр.Автостарт, Пред.Перезапуск, Загруж.Все каналы, Выход вместе с ПО Арм, Дополнение

'кластер расщепляется в массив и заполняется таблица (Каналы конфигурации ИВК и Выбранные каналы)и заполняется число элементов в таблице(Каналов ИВК)
'1 Name
'2 Scr_Name
'3 Scr_EdIzm
'7 Raw_Unit
'(Имя канала, Имя параметра)
'(Имя канала, Имя параметра, Ед.изм.физ., Ед.изм.сыр. )
'Из Свойств PIN создаётся вспомогательный массив(ArrPinNime)
'В нём делается замена 1TIME001 на -> 1  11TIME001 на -> 11 и т.д. т.е. берутся номера
'из строки с первых позиций извлекается номер и вычисляется длина этого числа
'затем из этой строки со смещением на эту длину и длиной 4 символа проверяется на равенство "TIME"
'если равно "TIME" берется номер иначе всё имя PIN
'во втором проходе если число из строки не излекается(=0) то сразу присваивается "ИВК"
'иначе если первые символы число и сместиться в строке за числом с первый символ после числа равеи "Т" то имя  присваивается "ССД1" 
'иначе число извлеклось (первые символы PIN) то составляется имя из "ССД" + число
'должно
'установить кнопку старт в активное положение

'если установлен Автостарт, то через 5 сек будет или обычный запуск клиента со всеми каналами
'Обычный запуск клиента.
'или
'если Загруж.все каналы = True, то
'Загружены все каналы и 3 канала для времени. 
'Ищутся индексы для каналов  Time1 Time2 Time3 и заполняется таблица
'(Time1 Время ССД1)(Time2 Время ССД2)(Time3 Время ССД3)
'
'если отсев не производится то в табл выбранные каналы идут все загруженные каналы
'если нужен отсев то в цикле по столбцу имени канала
'в имени канала проверяется первый символ подчеркивания
'если (_) или ArrPinNime равен ССД5 или ССД6 или (если Изделие =117 ССД8 иначе ССД10) то эти каналы и соответствующий элемент из ArrPinNime пропускается
'иначе пропустить все элементы 
'они накапливаются в новых массивах для отображения в таблицах Выбранные каналы и таблица Группа
'иначе
'Каналы загружены из файла.
'если Загруж.все каналы = False, то
'то в табл выбранные каналы таблица каналов времени загружается из файла
' 2 Sequence****************************************
'создаётся событи для Старт и Стоп



''Ошибка приёма выключить
''0 Sequence****************************************
''берётся первый столбец времени из списка (Каналы времени) и переменная число каналов времени идёт во 2 Sequence
''берётся список(Имя канала) из первого столбца таблицы (выбранные каналы) и объединяется с первым списком каналов времени
''этот массив подаётся в функции ReqParList_Insys_sbk и ReqRawList - там подготовка строк для запроса к серверу
''на выход также число элементов в этом массиве

''Ищется в список(Имя канала) имя с параметром "Time1"
''(если не найден то -1000 иначе его номер) прибавить к числу элементов в массиве списка (Каналы времени) и подаётся на выход

''вычисляется путь к \\CONFIG\InSys.cfg и выводится в окне
''выделяется массив из 1 столбца таблицы (выбранные каналы) -это имена параметров
''Замена_одинаковых_имен_в_массиве_строк()
''выделяется массив из 2 столбца таблицы (выбранные каналы) -это Ед.изм.физ.
''и массив групп
''вызывается Создать_файл_с_К1_1
''если в этой процедуре ошибок не было то сообщение об успешной записи CFG файла
''или сообщение об ошибке
''
''1 Sequence****************************************
''устанавливается TCP на чтение
'If TCPClientIsConnected Then
'    'если ошибки приёма нет то
'    'подготавливается цикл опроса сервера
'    'Значение периода опроса считывается с поля ввода окна программы
'    'считывается с контрола значение 1-й запуск
'    'устанавливается индикатор (ошибка приёма)
'    'устанавливается размерность таблицы Данные такой же как и размерность Выбранные каналы

'    'считывается значение переменной "!!!"
'    Do
'        'запускается цикл
'        'вычисляется длительность цикла как разница во времени в начале и в конце цикла
'        If ОшибкаПриёма Then
'            'установить GlobTCP_Insys.Mode=Read
'            'установить индикатор ошибки
'            If TCPClientIsConnected Then
'                'ветка case=false
'                If True Then 'первый запуск или ОПА!!!!
'                    'ОПА!!!!=false
'                    'NewEvent=True
'                    'генерация события и запись лога
'                    '-TCP клиент- Установлена TCP cвязь с сервером
'                Else
'                    'ничего
'                End If
'                'задержки wait нет
'            Else
'                'ветка case=true
'                If True Then 'ОПА!!!!
'                    'ничего
'                Else
'                    'ОПА!!!!=true
'                    'NewEvent=True
'                    'генерация события и запись лога
'                    '-TCP клиент- Потеряна TCP связь с сервером
'                End If
'                '
'                If MsgПерезап Then 'MSGПерезапуск
'                    'Ошибка приема TCP клиента.
'                    'Клиент будет перезапущен.
'                    переменная_выхода = Msg_Автостарт(5)
'                Else
'                    'установить переменную_выхода из цикла или проще Exit Do
'                    переменная_выхода = True
'                    Exit Do
'                End If

'                'задержки wait 10000 мсек
'            End If 'ОшибкаПриёма
'            'коментарий=""
'        Else
'            If ТипЗначений Then
'                'если Тип Значений - Расчётные значения (1) то
'                'выполняется запрос TCP Write с запросом строки от ReqParList_Insys_sbk, TimeOut=2000, connectionID из установленной связи
'                'вызывается GetParList_Insys_sbk_kdg1 который заполняет ArrStrВремя и Arr_DoubleValue
'            Else
'                'если Тип Значений - Сырые данные (0) то
'                '0 Sequence
'                'в цикле от 0 до ЧислаКаналовВремени-1
'                'в пакете для отправки из ReqRawList.vi(это огромный список запросов сырых данных- его лучше преобразовать в массив)
'                'последовательно считываются строки (длина 14 байт)
'                'на TCP настроенного на запись подаётся эта строка
'                'далее в процедуре GetPropStr получают запрошенные значения времени
'                'значения накапливаются
'                'значение счётчика накопленных каналов времен идёт на 1 Sequence
'                '1 Sequence
'                'в цикле от (счётчика накопленных времен) до ЧислаВыбранныхКаналов-1
'                'в пакете для отправки из ReqRawList.vi
'                'последовательно считываются строки (длина 14 байт)
'                'на TCP настроенного на запись подаётся эта строка
'                'далее в процедуре GetProp получают запрошенные значения канала
'                'значения накапливаются
'            End If
'            'если "!!!" или первый запуск то вызов Генерация_события
'            'если на предыдущих не было ошибок вызывается Заполнить_буфер_и_записать_в_BIN_файл туда же идёт счётчик циклов опроса
'            'заполняется таблица с данными
'        End If 'ОшибкаПриёма
'    Loop 'стоп или ошибка приема
'    'если нажата кнопка Стоп то генерируется сбыти Стоп(проверить)
'Else
'    'если ошибка приёма то перезапрос
'End If


''формирование запроса выходных данных каналов
'Private Sub ReqParList_Insys_sbk(Arr_Chs, Optional Time = 0)
'    'переменная Time может не использоваться
'    'выход Packet out - пакет запроса с серверу
'    '(можно оптимизировать и сделать суммирование бит)
'End Sub

''формирование запроса «сырых» данных с сервера
'Private Sub ReqRawList(Arr_Chs, Optional Time = 0)
'    'переменная Time может не использоваться
'    'выход Packet out - пакет запроса с серверу
'    'здесь выделяется массив число элементов*14 в цикле по каналам 
'    'этот массив заполняется битами
'    '(можно оптимизировать и сделать суммирование бит)
'End Sub

'Private Function Замена_одинаковых_имен_в_массиве_строк(Имена As String()) As String()
'    Dim Имена_без_повторов As String() = Имена
'    Return Имена_без_повторов
'    'В конфигурации физических параметров были найдены совпадающие имена каналов.
'    'Последние имена повторяющихся каналов остались без изменений, остальные были изменены.
'    'Повторяющиеся имена каналов:
'    'Измененные имена повторяющихся каналов:
'    'создаётся массив с Имена_без_повторов
'    'в цикле по именам по первому массиву
'    'в цикле по второму массиву
'    'проверяется наличие из первого во втором и если нет то сразу добавляется во второй
'    'иначе это имя заносится в спикок каналов с повторами
'    'к концу имени добавляется растущий индекс
'    'это имя заносится в спикок каналов с новыми именами
'    'затем  добавляется во второй список 
'    'второй список преобразуется в выходной массив и выводитс сообщение
'End Function

'Sub Создать_файл_с_К1_1(Путь_CFG_файла As String, Имена As String(), Ед_измер As String(), Группы As String())
'    'если размерность Ед_измер =0 то создаётся новый с размерностью Путь_CFG_файла инициализированный (?)
'    'в цикле по массиву имена
'    'имя очистить от пробелов
'    'Имена(0) = Trim(Имена(0))
'    'затем имя очищается от 00 регулрным выражением []*$
'    'макс. Уровень, Макс. Авар, Макс. Граф. =100
'    'Контроль = False   
'    'GПримечание =""
'    'Тип опроса =0
'    'Имя Daq = a + i.tostring - неправильно
'    'Группа = "Группа" + (i MOD 32).tostring + 
'    '*********************************************
'    'Файл перезаписывается
'End Sub

'Private Sub Запись_в_файл_Глоб(Путь_файла As String)
'    'Сохраняет в файл конфигурации ФП (*.cfg) глобальную переменную Физические параметры, при этом все коэффициенты полинома заменяются на 0;1;0;0;0;0.
'    '%6.2f\t%6.2f\t%6.2f\t%6.2f\t%6.2f\t%6.2f\t
'    'структура состоит из имён
'    'Аналог.ВХОД
'    'Аналог.ВЫХОД
'    'ОБОРОТЫ
'    'Дискрет.ВХОД
'    'Дискрет.ВХОД
'    'ВРЕМЯ

'    'другая структура
'    'Вибрации
'    'Тензо
'    'Медленный

'    'строится StringBuilder
'End Sub


'Private Sub TimeOut_Event()
'    'событие 0
'    ' в индикаторе (Выбрано каналов) отобразить число число каналов из таблицы Выбранные каналы 
'End Sub

'Private Sub КнопкаЗапрос_Event()
'    'событие 1 нажатие кнопки
'    'создаётся событе Запуск и оно генерируется
'End Sub

'Private Sub ЗакрытиеПанели_Event()
'    'событие 2 
'    'закрывается TCPсоединение и панель
'End Sub

'Private Sub КнопкаУдалитВсе2_Event()
'    'событие 4 нажатие кнопки
'    'очистка таблицы ВыбранныеКаналы
'End Sub

'Private Sub КнопкаУдалитьВсе2_Event()
'    'событие 5 нажатие кнопки
'    'очистка таблицы КаналыВремени
'End Sub

'Private Sub КнопкаСохранить_Event()
'    'событие 6 нажатие кнопки
'    'Запись_выбранных_каналов_в_файл
'End Sub

'Private Sub КнопкаЗагрузить_Event()
'    'событие 7 нажатие кнопки
'    'Чтение_выбранных_каналов_из_файла
'End Sub

'Private Sub ТаблицаКаналы_Event_DoubleClick()
'    'строка из таблицы Каналы добавляется в таблицу ВыбранныеКаналы
'    'событие 8 нажатие кнопки
'    'Проверка что Row меньше размерности числа строк таблицы
'    'взять первый элемент выделенной строки
'    'проверяется что имя из столбца 0 Каналы не содержится в таблице ВыбранныеКаналы
'    'если не содержится то в таблицу ВыбранныеКаналы
'    'добавляются столбцы 0,1,2
'End Sub

'Private Sub ДобавитьКаналыВремени_Event_Click()
'    'строка из таблицы Каналы добавляется в таблицу КаналыВремени
'    'событие 9 нажатие кнопки
'    'Проверка что Item(или Index) меньше размерности числа строк таблицы
'    'взять первый элемент выделенной строки
'    'проверяется что имя из столбца 0 Каналы не содержится в таблице КаналыВремени
'    'если не содержится то в таблицу КаналыВремени
'    'добавляются столбцы 0,1,2
'End Sub

'Private Sub ВыбранныеКаналы_УдалитьКанал_Event_DoubleClick()
'    'строка из таблицы Каналы добавляется в таблицу КаналыВремени
'    'событие 10 нажатие кнопки
'    'Взять индекс канала таблицы и удалить его из неё
'End Sub

'Private Sub Генерация_события()
'    'запись в журнал потеряна связь или установлена
'    'и генерируется соответствующее событие
'End Sub

'Private Sub Заполнить_буфер_и_записать_в_BIN_файл()
'    '1 Sequence****************************************
'    'вычисляется остаток от деления на 2 для определения чётного цикла
'    'если чётный то идёт накопление буфера
'    'иначе
'    'накопление буфера
'    'вызов Замена_данных
'    'вызов Обработка_и_сохранение
'    '2 Sequence****************************************
'    'если идёт замер
'    'замещаются? или накапливаются данные из Замера и Данных
'End Sub

'Private Sub Замена_данных()
'    'если EditInputDATA =false то буфер передаётся на выход
'    'иначе переписывается
'End Sub

'Private Sub Обработка_и_сохранение()
'    'запись файла (была модификация)
'End Sub

'Private Function Msg_Автостарт(TimeOut As Double) As Boolean
'    'Ошибка приема TCP клиента.
'    'Клиент будет перезапущен.
'    Dim Cancel As Boolean = False
'    Do
'        'если OK  выход
'        'если  Cancel то  Cancel  = True выход
'        'если время вышло то выход
'        'запись лога
'    Loop 'прошло 5 сек
'    Return Cancel
'End Function

'Private Sub Запись_выбранных_каналов_в_файл()
'    'формат записи таблицы Выбранные каналы и Каналы времени
'    'Trm138_ch1	ТРМ-138 Канал 1	Град С	Град.
'End Sub

'Private Sub Чтение_выбранных_каналов_из_файла()
'    'чтение и загрузка таблицы Выбранные каналы и Каналы времени
'    'чтение строк VbCrLF
'    'разбор строки по символу табуляции
'End Sub


'Private FileNameBIN As String
' ''' <summary>
' ''' запись простого текстового файла с данными каналов из списков
' ''' </summary>
' ''' <param name="ВсеПараметры"></param>
' ''' <remarks></remarks>
'Private Sub ЗаписьКонфигурации(ВсеПараметры As Boolean)
'    'Определение имени и пути к файлу
'    Dim FileInfoBinFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(FileNameBIN)
'    Dim folderPath As String = FileInfoBinFile.DirectoryName
'    'Dim fileName As String = Path.GetFileNameWithoutExtension(testFile.Name)
'    'Объединение имени файла с именем каталога для образования полного пути к файлу
'    Dim FileCfg As String = My.Computer.FileSystem.CombinePath(folderPath, "InSys.cfg") ' fileName & ".cfg")
'    If File.Exists(FileCfg) Then My.Computer.FileSystem.DeleteFile(FileCfg)
'    Try
'        Dim txtStrBuilder As New System.Text.StringBuilder
'        If ВсеПараметры Then
'            For Each ItemChannel In m_ManagerChannels.CollectionsChannels.Values
'                AddStringBuilder(txtStrBuilder, ItemChannel)
'            Next
'            'Else
'            '    Dim ItemChannel As Channel
'            '    For Each VisibleChannels In IndexVisibleChannels
'            '        ItemChannel = m_ManagerChannels.Item(VisibleChannels.Value)
'            '        AddStringBuilder(txtStrBuilder, ItemChannel)
'            '    Next
'        End If
'        'My.Computer.FileSystem.WriteAllText(FileCfg, txtStrBuilder.ToString(), False) ' True)'запись в unicode не считывает LabView
'        'System.IO.File.WriteAllText(FileCfg, ConvertUnicodeToSCIIByte(txtStrBuilder.ToString)) 'не работает
'        System.IO.File.WriteAllBytes(FileCfg, ConvertUnicodeToSCIIByte(txtStrBuilder.ToString()))
'    Catch ex As Exception 'Catch ex As IOException
'        MessageBox.Show(ex.ToString, "Запись файла конфигурации " & FileCfg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'    End Try
'End Sub

'Private Sub AddStringBuilder(ByRef _StrBuilder As StringBuilder, _ItemChannel As Channel)
'    _StrBuilder.Append(_ItemChannel.Название & vbTab)
'    _StrBuilder.Append(IIf(_ItemChannel.Включение = True, "ВКЛ", "ВЫКЛ") & vbTab)
'    _StrBuilder.Append(_ItemChannel.ЕдИзмер & vbTab)
'    _StrBuilder.Append(_ItemChannel.Тип & vbTab)

'    _StrBuilder.Append("0.0" & vbTab) 'K1
'    _StrBuilder.Append("1.0" & vbTab) 'K2
'    _StrBuilder.Append("0.0" & vbTab) 'K3
'    _StrBuilder.Append("0.0" & vbTab) 'K4
'    _StrBuilder.Append("0.0" & vbTab) 'K5
'    _StrBuilder.Append("0.0" & vbTab) 'K6

'    _StrBuilder.Append(_ItemChannel.МинУров & vbTab)
'    _StrBuilder.Append(_ItemChannel.МаксУров & vbTab)
'    _StrBuilder.Append(_ItemChannel.МинАвар & vbTab)
'    _StrBuilder.Append(_ItemChannel.МаксАвар & vbTab)
'    _StrBuilder.Append(_ItemChannel.МинГраф & vbTab)
'    _StrBuilder.Append(_ItemChannel.МаксГраф & vbTab)

'    _StrBuilder.Append(IIf(_ItemChannel.Контроль = True, "ВКЛ", "ВЫКЛ") & vbTab)
'    _StrBuilder.Append(_ItemChannel.Примечание & vbTab)
'    _StrBuilder.Append(_ItemChannel.ТипОпроса & vbTab)
'    _StrBuilder.Append(_ItemChannel.ИмяDAQ & vbTab)
'    _StrBuilder.Append(_ItemChannel.Группа & vbCrLf)
'End Sub


'Private Function ConvertUnicodeToSCIIByte(unicodeString As String) As Byte() 'As String
'    'Все строчки в .net хранятся в юникоде. 
'    'Надо взять строку, и с помощью метода GetBytes нужной кодировки получить представление строки в байтах для данной кодировки. 
'    'Ну а дальше берется этот массив байт и преобразуется в строку с помощью Encoding.GetString.
'    'Должны вызывать метод той кодировки, к которой хотите преобразовать строку.
'    'Создать 2 различные кодировки.
'    'Encoding.GetEncoding("cp866")'Encoding.UTF7 ' Encoding.UTF8 'Encoding.ASCII 'Encoding.GetEncoding("koi8-r")'As New UTF8Encoding
'    Dim ascii As Encoding = Encoding.GetEncoding("windows-1251")
'    Dim [unicode] As Encoding = Encoding.Unicode 'As New UnicodeEncoding()

'    'Конвертировать строку в массив byte[].
'    Dim unicodeBytes As Byte() = [unicode].GetBytes(unicodeString)

'    'Произвести конвертацию из одной кодировки в другую.
'    Dim asciiBytes As Byte() = Encoding.Convert([unicode], ascii, unicodeBytes)

'    'Конвертировать новый byte[] в char[] а затем в строку.
'    'Немного другой подход конвертации  GetCharCount/GetChars.
'    Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length) - 1) As Char
'    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
'    Dim asciiString As New String(asciiChars)

'    'Тест показа строк до и после конвертации, чтобы показать, что обратная конвертация правильна.
'    'Console.WriteLine("Original string: {0}", unicodeString)
'    'Console.WriteLine("Ascii converted string: {0}", asciiString)

'    'Записывать надо байты, а не строку
'    'System.IO.File.WriteAllBytes(FileCfg, asciiBytes)

'    'System.IO.File.WriteAllText(FileCfg, asciiString)
'    'Dim sNewText As String = StrConv(asciiString, VbStrConv.Lowercase)
'    'Return asciiString
'    Return asciiBytes
'End Function
