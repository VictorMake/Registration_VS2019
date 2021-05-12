Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Threading.Thread
Imports System.Xml.Linq
Imports MathematicalLibrary

Public Class FormMainMDI
    ''' <summary>
    ''' Имя Субформы
    ''' </summary>
    ''' <returns></returns>
    Public Property CaptionForm() As String

    Friend ModuleSolveManager As CalculationModuleManager
    Friend ModuleAcquisitionKTManager As KTCalculationModuleManager
    ''' <summary>
    ''' Счетчик повторов
    ''' </summary>
    Private counterRepetition As Integer
    Private Property FormManager As FactoryFormExaminationManager ' менеджер и фабрика форм загрузок

    Private Sub FormMainMDI_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        Left = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainLeft", CStr(0)))
        Top = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainTop", CStr(0)))
        Width = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainWidth", CStr(640)))
        Height = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainHeight", CStr(480)))

        If IsWorkWithDaqController Then TestChannelForm = New FormTestChannel
        If Directory.Exists(PathPanelMotorist) Then MenuShowPanel.Enabled = True
        If (IsWorkWithDaqController OrElse IsCompactRio OrElse IsTcpClient) AndAlso (PathSolveModule IsNot Nothing) Then AnalysisCalculationModule()
        ' если и клиент и работа со снимками
        If (Not (IsWorkWithDaqController OrElse IsCompactRio OrElse IsTcpClient)) AndAlso (PathModuleSolveKT IsNot Nothing) Then AnalysisAcquisitionModuleKT()
        If IsRunAutoBooting Then LoadNewInheritsForm(TypeExamination.Registration) ' регистратор

        If IsTcpClient Then
            DelegateAddListItem = New AddListItem(AddressOf AddListItemMethod) ' делегат обновления текста лога из другого потока
            LoadMainFormAgain()
        Else
            TableLayoutPanelConnection.Visible = False
            SplitterConnectionPanel.Visible = False
        End If

        If IsCompactRio Then
            LoadFormTestCompactRio()
        End If

        ShowConnectPanel()
        gFormsPanelManager = New FormsPanelManager
        FormManager = New FactoryFormExaminationManager(Me)
    End Sub

    Private Sub FormMainMDI_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If gFormsPanelManager.CollectionFormPanelMotorist IsNot Nothing Then
            If gFormsPanelManager.CollectionFormPanelMotorist.Count > 0 Then
                gFormsPanelManager.IsClosingApplication = True
                MDITabPanelMotoristForm.ClearListMenuByFunctionPanels() ' здесь выгружаются все панели моториста
            End If
        End If

        If MDITabPanelMotoristForm IsNot Nothing Then ' форма может быть загружена пустой без панелей
            MDITabPanelMotoristForm.Close()
        End If

        If ModuleSolveManager IsNot Nothing Then
            ' что-то сделать с модулями расчета
            ModuleSolveManager = Nothing
        End If

        If IsTcpClient Then
            DelegateClientSendData(False) ' остановить передачи пакетов Серверу
            Application.DoEvents()
            If TokenSource IsNot Nothing Then MainMdiForm.TokenSource.Cancel() ' прервать задачу проверки сетевого подключения с сервером
            'fMainForm.ConnectClientStartStop(False)
            StartStopConnectionWithServer(False) ' остановить связь с сервером
            ShowReceiveData(False)
        End If

        If IsCompactRio Then CloseFormTestCompactRio()
    End Sub

    Private Sub FormMainMDI_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        Dim nameForms As New List(Of String)

        If IsUseTdms Then
            If gTdmsFileProcessor IsNot Nothing Then
                gTdmsFileProcessor.Dispose()
                gTdmsFileProcessor = Nothing
            End If
        End If

        For Each tempForm In CollectionForms.Forms.Values
            nameForms.Add(tempForm.Text)
            tempForm.Close()
        Next

        If nameForms.Count > 0 Then
            For I As Integer = nameForms.Count - 1 To 0
                CollectionForms.Remove(nameForms(I))
            Next
        End If

        If WindowState <> FormWindowState.Minimized Then
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainLeft", CStr(Left))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainTop", CStr(Top))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainWidth", CStr(Width))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "MainHeight", CStr(Height))
        End If

        If IsWorkWithDaqController Then ' наверно повтор
            Try
                If IsTaskRunning = True Then
                    IsTaskRunning = False
                    If DAQmxTask IsNot Nothing Then
                        DAQmxTask.Stop()
                        Sleep(100)
                        DAQmxTask.Dispose()
                        DAQmxTask = Nothing
                    End If
                End If
            Catch ex As Exception
                Const caption As String = "Остановка DAQ"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
        End If

        SettingForm.Close()

        If TestChannelForm IsNot Nothing Then
            Try
                TestChannelForm.Close()
            Catch ex As Exception
                Const caption As String = "Выгрузка FormTestChannel"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
        End If

        TarirForm = Nothing
        RegistrationMain = Nothing
        SnaphotMain = Nothing
        ServiceBasesForm = Nothing
        CollectionForms.Clear()
        ' закрыть все формы
        'For i = My.Application.OpenForms.Count - 1 To 1 Step -1
        '    'Unload(My.Application.OpenForms(i))
        '    My.Application.OpenForms(i).Close()
        'Next

        Dispose()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{NameOf(FormMainMDI_FormClosed)}> Закрытие основного родительского окна")
    End Sub

    ''' <summary>
    ''' Создать дочернее окно для MDI основной формы в зависимости от 
    ''' типа испытания и от логических флагов работы с конроллером, TCP, клиент или расшифровка снимка.
    ''' </summary>
    ''' <param name="inTypeExamination"></param>
    Friend Sub LoadNewInheritsForm(ByVal inTypeExamination As TypeExamination)
        StopAcquisition()
        If IsFrmServiceBasesLoaded Then ServiceBasesForm.Close() ' здесь вопрос о записи изменений

        Static lDocumentCount As Integer
        lDocumentCount += 1

        ' на вход FormManager подать FormExamination и получить нужную форму
        Select Case inTypeExamination
            Case TypeExamination.Registration
                CloseTestBarometer()
                CheckForCopyingBase(TypeWorkAutomaticBackup.RegistrationOnStart)
                MenuNewWindowRegistration.Enabled = False
                CaptionForm = "Регистратор " & lDocumentCount
                RegistrationFormName = CaptionForm

                If IsWorkWithDaqController Then
                    RegistrationMain = CType(FormManager.CreateFormMain(FormExamination.RegistrationSCXI, CaptionForm), FormRegistrationBase)
                ElseIf IsCompactRio Then
                    RegistrationMain = CType(FormManager.CreateFormMain(FormExamination.RegistrationCompactRio, CaptionForm), FormRegistrationBase)
                ElseIf IsTcpClient Then
                    RegistrationMain = CType(FormManager.CreateFormMain(FormExamination.RegistrationTCP, CaptionForm), FormRegistrationBase)
                End If

                ShowDaughterForm(RegistrationMain)
                Exit Select
            Case TypeExamination.Snapshot
                MenuNewWindowSnapshot.Enabled = False
                CaptionForm = "Снимок " & lDocumentCount

                If IsWorkWithDaqController Then
                    SnaphotMain = CType(FormManager.CreateFormMain(FormExamination.SnapshotPhotograph, CaptionForm), FormSnapshotBase)
                Else
                    SnaphotMain = CType(FormManager.CreateFormMain(FormExamination.SnapshotViewingDiagram, CaptionForm), FormSnapshotBase)
                End If

                ShowDaughterForm(SnaphotMain)
                Exit Select
            Case TypeExamination.Client
                MenuNewWindowClient.Enabled = False
                CaptionForm = "Клиент " & lDocumentCount
                RegistrationFormName = CaptionForm
                RegistrationMain = CType(FormManager.CreateFormMain(FormExamination.RegistrationClient, CaptionForm), FormRegistrationBase)
                ShowDaughterForm(RegistrationMain)
                Exit Select
        End Select

        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(LoadNewInheritsForm)}> Была произведена эагрузка нового окна из родительской формы = {CaptionForm}")
    End Sub

    Private Sub ShowDaughterForm(daughterForm As FormMain)
        If daughterForm Is Nothing Then Exit Sub

        daughterForm.Show()
        CollectionForms.Add(daughterForm.Text, daughterForm)
        daughterForm.StartMeasurement()
    End Sub

    'Private arrInputVolt(2) As Double
    'Private arrInputПредыдущийСбор(2) As Double
    Private Sub TimerAwait_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerAwait.Tick
        If RegistrationMain Is Nothing Then
            TimerAwait.Stop()
            Exit Sub
        End If

        If RegistrationMain.IsFormRunning Then
            'If arrInputПредыдущийСбор(0) = arrInputVolt(0) AndAlso arrInputПредыдущийСбор(1) = arrInputVolt(1) AndAlso arrInputПредыдущийСбор(2) = arrInputVolt(2) Then
            If IsAcquisitionOk = False Then
                counterRepetition += 1
                If counterRepetition >= 3 Then
                    If IsTaskRunning = True Then
                        IsTaskRunning = False
                        If DAQmxTask IsNot Nothing Then
                            DAQmxTask.Stop()
                            Sleep(100)
                            DAQmxTask.Dispose()
                            DAQmxTask = Nothing
                        End If
                    End If
                    RegistrationMain.StartContinuous()
                    counterRepetition = 0
                End If
                Exit Sub
            End If
            'запомнить
            'arrInputПредыдущийСбор(0) = arrInputVolt(0)
            'arrInputПредыдущийСбор(1) = arrInputVolt(1)
            'arrInputПредыдущийСбор(2) = arrInputVolt(2)
            IsAcquisitionOk = False
            counterRepetition = 0
        End If
    End Sub

#Region "MenuEvents"
    Private Sub MenuApplicationExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuApplicationExit.Click
        Try
            Close()
        Catch ex As Exception
            Const caption As String = "Выгрузка FormMainMDI"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' регистратор
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowRegistration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowRegistration.Click
        LoadNewInheritsForm(TypeExamination.Registration)
    End Sub

    ''' <summary>
    ''' снимок
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowSnapshot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowSnapshot.Click
        LoadNewInheritsForm(TypeExamination.Snapshot)
    End Sub

    ''' <summary>
    ''' Тарировка
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowTarir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowTarir.Click
        StopAcquisition()

        If IsFrmServiceBasesLoaded Then ServiceBasesForm.Close()

        TarirForm = New FormTarir(Me)
        TarirForm.Show()
        TarirForm.Activate()
        TarirForm.FormTarirResize()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuNewWindowTarir_Click)}> Загрузка окна Тарировки")
    End Sub

    ''' <summary>
    ''' Клиент
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowClient_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowClient.Click
        LoadNewInheritsForm(TypeExamination.Client)
    End Sub

    ''' <summary>
    ''' Редактора Каналов
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowDBaseChannels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowDBaseChannels.Click
        ServiceBasesForm = New FormServiceBases(Me)
        ServiceBasesForm.Show()
        ServiceBasesForm.Activate()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuNewWindowDBaseChannels_Click)}> Загрузка окна Редактора Каналов")
    End Sub

    ''' <summary>
    ''' Управления по Событиям
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub MenuNewWindowEvents_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowEvents.Click
        If CheckExistChannelsDigitalOutput() Then
            MenuNewWindowEvents.Enabled = False
            DigitalPortForm = New FormDigitalPort
            DigitalPortForm.Show()
            DigitalPortForm.Activate()
            IsFrmDigitalOutputPortStart = True
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuNewWindowEvents_Click)}> Загрузка окна Управления По Событиям")
        End If
    End Sub

    Private Function CheckExistChannelsDigitalOutput() As Boolean
        PathChannelsDigitalOutput = PathResourses & "\ChannelsDigitalOutput.mdb"
        If FileNotExists(PathChannelsDigitalOutput) Then
            Const caption As String = "Зазгузка окна"
            Dim text As String = $"В каталоге нет файла ChannelsDigitalOutput.mdb!{vbCrLf}Зазгузка будет прекращена."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub MenuWindowCascade_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowCascade.Click
        LayoutMdi(MdiLayout.Cascade)
    End Sub

    Public Sub MenuWindowTileHorizontal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowTileHorizontal.Click
        LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Public Sub MenuWindowTileVertical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowTileVertical.Click
        LayoutMdi(MdiLayout.TileVertical)
    End Sub
#End Region

#Region "Работа с панелями моториста"
    Private Sub MenuEditorPanel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuEditorPanel.Click
        If IndexParametersForControl IsNot Nothing Then
            LoadEditorPanelMotoristForm()
        Else
            Const caption As String = "Запуск редактора панелей"
            Const text As String = "Перед загрузкой редактора панелей необходимо хотя-бы раз загрузить окно регистратора или клиента"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub
    ''' <summary>
    ''' Загрузить редактор панелей
    ''' </summary>
    Private Sub LoadEditorPanelMotoristForm()
        If MDITabPanelMotoristForm IsNot Nothing Then
            MDITabPanelMotoristForm.Close()
            'там MenuItemРедакторПанелей.Enabled = True
            ' и  MenuItemПоказатьПанели.Enabled = True
        End If

        MenuEditorPanel.Enabled = False
        MenuShowPanel.Enabled = False

        EditorPanelMotoristForm = New FormEditorPanelMotorist()
        EditorPanelMotoristForm.Show()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuEditorPanel_Click)}> Загрузка окна Редактора Визуальных Панелей")
    End Sub

    Private Sub MenuShowPanel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuShowPanel.Click
        MDITabPanelMotoristForm = New FormMDITabPanel()
        MDITabPanelMotoristForm.Show()
        MenuEditorPanel.Enabled = False
        MenuShowPanel.Enabled = False
    End Sub
#End Region

#Region "Работа с модулями расчета"
    Private Const nameBarometersBRS1M As String = "BarometersBRS1M.dll"

    ' 1. изначально mnuWindowМодулиРасчета и MenuItemКонфигураторМодулейРасчета недоступны
    ' если тип загрузки 1 с железом
    ' составить список модулей и их описания DLL в колекцию класса Менеджер и проверить их тип  Inherits BaseForm.frmBase
    ' если каталог не создан или список DLL пуст, то меню выключить mnuWindowМодулиРасчета класса Менеджер уничтожить
    ' 2. считатать имена модулей из конфигурационного файла 
    ' меню mnuWindowМодулиРасчета и MenuItemКонфигураторМодулейРасчета включить
    ' если имеется совпадающее имя модуля из конфигурационного файла и из списка модулей в каталоге - то имя добавляется в список подлежащих подключению
    ' и отображается в пунктах меню (видимые отображаютя как помеченные здесь невозможно так как нужна загрузка модуля - отображение значить 
    ' это отметить меню чеком при загрузке модулей в коллекцию конфигуратора)
    ' 3. после этого пользователь может вызвать конфигуратор и отметить или снять новые модули которые добавляется в список подлежащих подключению
    ' если после этого стартует регистратор то если MenuItemКонфигураторМодулейРасчета.enable=true
    ' MenuItemКонфигураторМодулейРасчета.enable=false выключить
    ' 4. если при АнализРасчетныхМодулей список подлежащих подключению модулей пуст - то меню mnuWindowМодулиРасчета выключить класса Менеджер уничтожить
    ' в противном случае с меню добавляются пункт с описанием модуля и с отмеченным чеком если модуль видим
    ' в последующем при повторных запусках регистратора пункт MenuItemКонфигураторМодулейРасчета.enable=false всегла будет выключен
    ' и повторной проверки и заполнения не надо.
    ' 5. при закрытии приложения Менеджер уничтожить

    ''' <summary>
    ''' Анализ расчетных модулей.
    ''' Вызвать процедуру можно, если каталог создан.
    ''' </summary>
    Private Sub AnalysisCalculationModule()
        ModuleSolveManager = New CalculationModuleManager(PathSolveModule)

        Try
            RemoveAllMenuForCalculationModule()
            If ModuleSolveManager.MakeListModulesInCatalogue Then
                ' там уже были считаны имена модулей из конфигурационного файла 
                MenuWindowModuleCalculation.Enabled = True
                MenuConfigurationCalculationModule.Enabled = True

                ' заполнить меню
                For Each itemPassportModule As PassportModule In ModuleSolveManager.PassportModuleDictionary.Values
                    If itemPassportModule.Enable Then AddMenuItemCalculationModule(itemPassportModule)
                Next
            Else
                ModuleSolveManager = Nothing
            End If

            MenuSetComPort.Enabled = File.Exists(Path.Combine(PathSolveModule, nameBarometersBRS1M))
            UpdateListSolveModules()
        Catch ex As Exception
            Dim caption As String = "АнализРасчетныхМодулей " & PathSolveModule
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub MenuConfigurationCalculationModule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuConfigurationCalculationModule.Click
        MenuConfigurationCalculationModule.Enabled = False

        Dim fКонфигураторРасчетныхМодулей As New FormConfigurationSolveModule(ModuleSolveManager)
        If fКонфигураторРасчетныхМодулей.ShowDialog = DialogResult.OK Then
            ' в цикле добавить меню и пометить видимые
            RemoveAllMenuForCalculationModule()
            ' при загрузке регистратора через менежер запустить на анализ, в котором не прошедшие выкидываются
            For Each tempПаспортМодуля As PassportModule In ModuleSolveManager.PassportModuleDictionary.Values
                If tempПаспортМодуля.Enable Then AddMenuItemCalculationModule(tempПаспортМодуля)
            Next

            fКонфигураторРасчетныхМодулей.Dispose()
        End If

        MenuConfigurationCalculationModule.Enabled = True
        CheckIsNeedArchive()
    End Sub

    ''' <summary>
    ''' Список подключенных модулей в конфигурации
    ''' </summary>
    Private listCalculationModule As List(Of String)

    ''' <summary>
    ''' Обновить список расчетных модулей
    ''' </summary>
    Private Sub UpdateListSolveModules()
        Dim modules = From m In ModuleSolveManager.PassportModuleDictionary.Values
                      Where m.Enable
                      Select Name = m.NameModule

        listCalculationModule = modules.ToList
    End Sub

    ''' <summary>
    ''' Проверить нужно архивировать
    ''' </summary>
    Private Sub CheckIsNeedArchive()
        Dim NewListCalculationModule As List(Of String)
        Dim modules = From m In ModuleSolveManager.PassportModuleDictionary.Values
                      Where m.Enable
                      Select Name = m.NameModule

        NewListCalculationModule = modules.ToList

        Dim isNotContains As Boolean

        ' вначале по старому листу
        For Each name As String In NewListCalculationModule
            If Not listCalculationModule.Contains(name) Then isNotContains = True
        Next

        ' теперь по новому листу
        For Each name As String In listCalculationModule
            If Not NewListCalculationModule.Contains(name) Then isNotContains = True
        Next

        listCalculationModule = NewListCalculationModule

        If isNotContains Then ArchivingByOpeningWindow()
    End Sub

    ''' <summary>
    ''' Удалить все меню для модули расчета
    ''' </summary>
    Private Sub RemoveAllMenuForCalculationModule()
        ' далее удалить все пункты меню
        Const itemsCountSafe As Integer = 3 ' 3 пункта надо зарезервировать для меню добавить, меню настройки БРС и разделитель (удалить нету)
        For i As Integer = MenuWindowModuleCalculation.DropDownItems.Count - 1 To itemsCountSafe Step -1
            If MenuWindowModuleCalculation.DropDownItems.Count > itemsCountSafe Then
                Dim removeAt As Integer = MenuWindowModuleCalculation.DropDownItems.Count - 1
                MenuWindowModuleCalculation.DropDownItems.RemoveAt(removeAt)
            End If
        Next
    End Sub

    Private Sub AddMenuItemCalculationModule(ByVal inPassportModule As PassportModule)
        Dim newMenuPanel As New ToolStripMenuItem() With {.Name = inPassportModule.NameModule,
                                                        .Checked = False,
                                                        .CheckOnClick = True,
                                                        .Text = inPassportModule.DescriptionModule,
                                                        .ToolTipText = "Для настойки модуля расчета отметить данный пункт",
                                                        .Enabled = False}

        ' Добавить обработчик запуска окна нанели.
        AddHandler newMenuPanel.Click, AddressOf MenuItemCalculationModule_Click
        MenuWindowModuleCalculation.DropDownItems.Add(newMenuPanel)
    End Sub

    Private Sub MenuItemCalculationModule_Click(ByVal sender As Object, ByVal e As EventArgs)
        If RegistrationFormName = "" Then Exit Sub

        ' если модуль видим(можно узнать из свойств модуля в уже загруженной коллекции), то ни чего не делать
        ' если невидим, то сделать видимым для настройки или наоборот скрыть  и изменить чек меню
        Dim selectedItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        If ModuleSolveManager.CalculationModuleDictionary.ContainsKey(selectedItem.Name) Then
            If ModuleSolveManager.CalculationModuleDictionary(selectedItem.Name).IsDllVisible = False Then
                ' значит модуль изначально невидим
                ' логика Checked по щелчку меняется поэтому условие наоборот
                If selectedItem.CheckState = CheckState.Unchecked Then
                    ModuleSolveManager.CalculationModuleDictionary(selectedItem.Name).Hide()
                    selectedItem.CheckState = CheckState.Unchecked
                Else
                    ' он был показан по другому щелчку форма модуля уже на экране, а значит скрыть
                    ModuleSolveManager.CalculationModuleDictionary(selectedItem.Name).Show()
                    selectedItem.CheckState = CheckState.Checked
                End If
            Else
                ' если модуль видим, то пункт всегда отмечен
                selectedItem.CheckState = CheckState.Checked
            End If
        Else
            Const caption As String = "Настройка расчётного модуля"
            Dim text As String = $"Расчётный модуль с именем {selectedItem.Name} не был загружен из-за конфликта имён расчётных параметров!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub

    ' В отличие от правильного тестирования типа как BaseForm.FrmBase тестируется конкретный тип BarometersBRS1M.FrmMain
    ' поэтому в проекте нужно сделать ссылку на этот конкретный тип
    Private WithEvents TestBarometer As BarometersBRS1M.FrmMain
    Private Sub MenuSetComPort_Click(sender As Object, e As EventArgs) Handles MenuSetComPort.Click
        If MenuSetComPort.CheckState = CheckState.Checked Then
            CloseTestBarometer()
        Else
            LoadBarometersBRS1M()
            MenuSetComPort.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub CloseTestBarometer()
        If TestBarometer IsNot Nothing Then
            CType(TestBarometer, BaseForm.FrmBase).IsWindowClosed = True
            TestBarometer.Close()
            TestBarometer = Nothing
        End If
        MenuSetComPort.CheckState = CheckState.Unchecked
    End Sub

    Private Sub LoadBarometersBRS1M()
        'отладка в последствии закоментировать а в наследующих классах в методе LOAD разкоментировать
        TestBarometer = New BarometersBRS1M.FrmMain
        TestBarometer.Manager.PathSettingMdb = Path.Combine(PathSolveModule, "BarometersBRS1M.mdb")
        TestBarometer.Manager.NameRegistrationParameters(New String() {"Отсутствует", "One", "Two", "N1", "Вк", "Вб"})
        TestBarometer.Manager.SetIndexRegistrationParameters(New Integer() {1, 2, 3, 4, 5})
        TestBarometer.Show()

        TestBarometer.Manager.IsEnabledTuningForms = False ' видимость сеток
        TestBarometer.EnableNewDeleteButton()
    End Sub

    Friend WithEvents GFormCompactRio As FormCompactRio

    Private Sub LoadFormTestCompactRio()
        'отладка в последствии закоментировать а в наследующих классах в методе LOAD разкоментировать
        GFormCompactRio = New FormCompactRio(Me) With {.PathSettingMdb = PathChannels}
        GFormCompactRio.Show()
    End Sub

    Private Sub CloseFormTestCompactRio()
        If GFormCompactRio IsNot Nothing Then
            GFormCompactRio.IsWindowClosed = True
            GFormCompactRio.Close()
            GFormCompactRio = Nothing
        End If
    End Sub
#End Region

#Region "Работа с модулями сбора КТ"
    'сделать обработку видимости таблиц базы данных по кнопке настройки графиков varМодульСбораКТManager.ДоступМеню(True)
    ' 1. изначально mnuWindowМодулиСбораКТ и MenuItemКонфигураторМодулейСбораКт недоступны
    ' если тип загрузки 4 - клиент
    ' составить список модулей и их описания DLL в колекцию класса Менеджер и проверить их тип  Inherits BaseForm.frmBase
    ' если каталог не создан или список DLL пуст, то меню выключить mnuWindowМодулиСбораКТ, класс Менеджер уничтожить
    ' 2. считатать имена модулей из конфигурационного файла 
    ' меню mnuWindowМодулиСбораКТ и MenuItemКонфигураторМодулейСбораКт включить
    ' если имеется совпадающее имя модуля из конфигурационного файла и из списка модулей в каталоге то имя добавляется в список подлежащих подключению
    ' и отображается в пунктах меню(видимые отображаютя как помеченные здесь невозможно так как нужна загрузка модуля - отображение значить 
    ' это отметить меню чеком при загрузке модулей в коллекцию конфигуратора)
    ' 3. после этого пользователь может вызвать конфигуратор и отметить или снять новые модули которые добавляется в список подлежащих подключению
    ' если после этого стартует регистратор то если MenuItemКонфигураторМодулейСбораКт.enable=true
    ' MenuItemКонфигураторМодулейСбораКт.enable=false выключить
    ' 4. если при АнализРасчетныхМодулей список подлежащих подключению модулей пуст  то меню mnuWindowМодулиСбораКТ выключить класс Менеджер уничтожить
    ' в противном случае в меню добавляются пункт с описанием модуля и с отмеченным чеком если модуль видим
    ' в последующем при повторных запусках регистратора пункт MenuItemКонфигураторМодулейСбораКт.enable=false всегла будет выключен
    ' и повторной проверки и заполнения не надо.
    ' 5. при закрытии приложения Менеджер уничтожить

    ''' <summary>
    ''' Анализ модулей сбора КТ.
    ''' Вызвать процедуру можно, если каталог создан.
    ''' </summary>
    Private Sub AnalysisAcquisitionModuleKT()
        ModuleAcquisitionKTManager = New KTCalculationModuleManager(PathModuleSolveKT)

        Try
            RemoveAllMenuAcquisitionKT()
            If ModuleAcquisitionKTManager.MakeListModulesInCatalogue Then
                ' там уже считатать имена модулей из конфигурационного файла 
                MenuWindowModuleCalculationKT.Enabled = True
                MenuConfigurationCalculationModuleKT.Enabled = True
                ' заполнить меню
                For Each tempПаспортМодуля As PassportModule In ModuleAcquisitionKTManager.PassportModuleDictionary.Values
                    If tempПаспортМодуля.Enable Then AddMenuItemAcquisitionKT(tempПаспортМодуля)
                Next
            Else
                ModuleAcquisitionKTManager = Nothing
            End If
        Catch ex As Exception
            Dim caption As String = $"{NameOf(AnalysisAcquisitionModuleKT)} {PathModuleSolveKT}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub MenuConfigurationCalculationModuleKT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuConfigurationCalculationModuleKT.Click
        MenuConfigurationCalculationModuleKT.Enabled = False

        Dim frmConfigurationModuleAcquisitionKT As New FormConfigurationModuleAcquisitionKT(ModuleAcquisitionKTManager)
        If frmConfigurationModuleAcquisitionKT.ShowDialog = DialogResult.OK Then
            ' в цикле добавить меню и пометить видимые
            RemoveAllMenuAcquisitionKT()
            ' при загрузке регистратора через менежер запустить на анализ, в котором не прошедшие выкидываются
            For Each tempПаспортМодуля As PassportModule In ModuleAcquisitionKTManager.PassportModuleDictionary.Values
                If tempПаспортМодуля.Enable Then AddMenuItemAcquisitionKT(tempПаспортМодуля)
            Next

            frmConfigurationModuleAcquisitionKT.Dispose()
        End If

        MenuConfigurationCalculationModuleKT.Enabled = True
    End Sub
    ''' <summary>
    ''' Удалить все меню для модули сбора КТ
    ''' </summary>
    Private Sub RemoveAllMenuAcquisitionKT()
        ' далее удалить все пункты меню
        ' 2 пункта надо зарезервировать для добавить и разделитель (удалить нету)
        For i As Integer = MenuWindowModuleCalculationKT.DropDownItems.Count - 1 To 2 Step -1
            If MenuWindowModuleCalculationKT.DropDownItems.Count > 2 Then
                Dim removeAt As Integer = MenuWindowModuleCalculationKT.DropDownItems.Count - 1
                MenuWindowModuleCalculationKT.DropDownItems.RemoveAt(removeAt)
            End If
        Next
    End Sub

    Private Sub AddMenuItemAcquisitionKT(ByVal tempПаспортМодуля As PassportModule)
        Dim newMenuPanel As New ToolStripMenuItem() With {.Name = tempПаспортМодуля.NameModule,
                                                        .Checked = False,
                                                        .CheckOnClick = True,
                                                        .Text = tempПаспортМодуля.DescriptionModule,
                                                        .ToolTipText = "Для настойки модуля сбора КТ отметить данный пункт",
                                                        .Enabled = False}

        MenuWindowModuleCalculationKT.DropDownItems.Add(newMenuPanel)
    End Sub
#End Region

#Region "Tcp_Client"
    Private showMorePanel As Boolean
    Private splitterConnectionPanelWidth As Integer
    Private Sub ShowConnectPanel()
        If showMorePanel Then
            ButtonShowConnectPanel.Image = My.Resources.forward
            Me.ToolTip.SetToolTip(ButtonShowConnectPanel, "Скрыть дополнительную панель >>")
            showMorePanel = False
            SplitterConnectionPanel.SplitPosition = splitterConnectionPanelWidth
        Else
            ButtonShowConnectPanel.Image = My.Resources.back
            Me.ToolTip.SetToolTip(ButtonShowConnectPanel, "<< Показать дополнительную панель")
            showMorePanel = True
            splitterConnectionPanelWidth = SplitterConnectionPanel.SplitPosition
            TableLayoutPanelConnection.Width = CInt(TableLayoutPanelConnection.ColumnStyles.Item(0).Width + 4)
        End If
    End Sub
    Private Sub ButtonShowConnectPanel_Click(sender As Object, e As EventArgs) Handles ButtonShowConnectPanel.Click
        ShowConnectPanel()
    End Sub

    Private Property ServerAddress As IPAddress
    Private Property TokenSource As CancellationTokenSource
    Private gASCII_Encoding As New System.Text.ASCIIEncoding
    Public Event EventMainAcquiredDataForChannelsForm(ByVal sender As Object, ByVal e As AcquiredDataEventArgs)
    Public WithEvents ConnectionClient As ConnectionInfoClient
    Private Const THRESHOLD_COUNT As Integer = 100 ' число полученных пакетов, чтобы обновить экран (при 100 Гц обновление раз в сек)
    Private Const KEY_RICH_TEXT_SERVER As Integer = -1

    Private command As Integer = 0 ' команда переданная Панелью управления через Сервер
    'Private Shared ReadOnly разобратьПакетОтСервераHandlerLockObject As New Object() ' объект синхронизации для блокирования доступа
    Private ReadOnly timeNumberStyle As NumberStyles = NumberStyles.AllowDecimalPoint Or NumberStyles.AllowThousands
    Private ReadOnly culture As CultureInfo = CultureInfo.CreateSpecificCulture("es-ES")
    Private message As String = String.Empty

    ' Один токен отмены должен относиться к одной отменяемой операции, однако эта операция может быть реализована в программе.
    ' После того как свойство IsCancellationRequested токена примет значение true, для него невозможно будет восстановить значение false. 
    ' Таким образом, токены отмены невозможно использовать повторно после выполнения отмены. Dim token As CancellationToken = tokenSource.Token
    Private mTaskDoMonitorCheckDataFromServer As Task
    Private mTaskCheckConnection As Task
    Private syncPointDoMonitor As Integer = 0 ' для синхронизации обработки пакетов от Сервера
    Private ConnectButtonChecked As Boolean

    ''' <summary>
    ''' Загрузка интерфейса вызывается при загрузке приложения и при перезагрузке
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadMainFormAgain()
        SSDRun()
    End Sub

    ''' <summary>
    ''' Основной поток ССД. Функция производит инициализацию и запуск ССД. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SSDRun()
        ' Установить размерности массивов сбора и Хеш
        ' надо заранее подготовить hash для всех каналов

        gPacketArray.Capacity = 100 '+ gArrayLengthAllChassis * 18 установить приблизительную ёмкость массива 18 байт размер пакета с типом Double + время + сторожевой таймер + канала статуса
        gPacketArraySmallString.Capacity = 11 + 256 ' command 1, Byte hash 4 Byte,valid 1 Byte,Time 4 Byte,Length 1 Byte

        ActivateTarget() 'ActivateTargetButton.Checked)
        StartStopConnectionWithServer(True)
        Sleep(100)

        InvokeClientSendData(True) ' включить индикатор

        ' запустить в фоне проверку соединения с Сервером
        ' В случае разрыва соединения, поток будет производить попытки его восстановить. 
        TokenSource = New CancellationTokenSource
        mTaskCheckConnection = Task.Factory.StartNew(Sub() WorkCheckConnection(TokenSource.Token), TokenSource.Token, TaskCreationOptions.LongRunning)
    End Sub

    ''' <summary>
    ''' Остановка и закрытие всех потоков подключений.
    ''' Вызывается при закрытии приложения и при перезагрузке.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StopAllConnections()
        If TokenSource IsNot Nothing Then TokenSource.Cancel() ' прервать задачу проверки сетевого подключения с сервером
        ConnectClientStartStop(False) ' остановить связь с сервером
    End Sub

    Private Sub InvokeConnectClientStartStop(isConnect As Boolean)
        Dim doConnectStop As New Action(Of Boolean)(AddressOf ConnectClientStartStop)
        Invoke(doConnectStop, isConnect)
    End Sub

    Private Sub ConnectClientStartStop(isConnect As Boolean)
        StartStopConnectionWithServer(isConnect)
        ShowReceiveData(isConnect)
    End Sub

#Region "Сокет"
    Private Sub ButtonConnectRefresh_Click(sender As Object, e As EventArgs) Handles ButtonConnectRefresh.Click
        StartStopConnectionWithServer(True)
    End Sub

    ''' <summary>
    ''' Поток соединения с сервером
    ''' </summary>
    ''' <param name="isConnectStart"></param>
    ''' <remarks></remarks>
    Private Sub StartStopConnectionWithServer(isConnectStart As Boolean)
        Const CAPTION As String = NameOf(StartStopConnectionWithServer)
        Dim text As String

        ConnectButtonChecked = isConnectStart
        If isConnectStart Then
            If ServerAddress IsNot Nothing Then
                If CheckPingClient(HostName) = True Then
                    Try
                        If ConnectionClient IsNot Nothing Then
                            ' чтобы не было 2 открытых соединений с Сервером
                            CType(mTaskDoMonitorCheckDataFromServer.AsyncState, ConnectionInfoClient).IsCancel = True
                            ConnectionClient.Close()
                            Sleep(100)
                        End If

                        ' Проба с таймером в форме
                        'If _ConnectionClient.ЗапускатьТаймерМожно Then  StartAcquisition()
                        ConnectionClient = New ConnectionInfoClient(ServerAddress, Convert.ToInt32(TextBoxServerPort.Text),
                                                                     AddressOf InvokeAppendOutput) With {.StopMethod = AddressOf InvokeConnectClientStartStop}

                        LabelStatusClient_adapter.Image = My.Resources.ledCornerGreen
                        LabelStatusClient_receive.Image = My.Resources.ledCornerOrange
                        PictureBoxConnectionServer.BackgroundImage = My.Resources.ledCornerGreen
                        ' Отправка пакета с описанием на сервер
                        Dim smallStringPakcetByte As Byte() = SendSSDDescription(gPacketArraySmallString, "Registration", "АРМ сбора и обработки информации")
                        ConnectionClient.Stream.Write(smallStringPakcetByte, 0, smallStringPakcetByte.Length)

                        Sleep(100)
                        ConnectionClient.StartAcquisitionTCP()
                        mTaskDoMonitorCheckDataFromServer = Task.Factory.StartNew(AddressOf DoMonitorCheckDataFromServer, ConnectionClient, TaskCreationOptions.LongRunning)
                        ConnectionClient.AwaitData()

                        text = "Соединение с Сервером установлено..."
                        ShowMsgText(text, MessageBoxIcon.Information)
                        RegistrationEventLog.EventLog_MSG_CONNECT($"<{CAPTION}> {text}")
                        LabelНепр.Text = "Приём каналов от Сервера включён" ' Приём каналов от Сервера отсутствует
                        LabelНепр.Image = My.Resources.Connect

                        CheckConnection(ConnectionClient.Tcp_Client.Connected) ' индикатор
                        IsConnectedWithServer = True
                        gWatchdogChannelValue = 0
                    Catch ex As Exception
                        text = "Ошибка подключения к Серверу:" & Environment.NewLine & ex.ToString
                        DelegateAppendOutput(text, MessageBoxIcon.Error)
                        'MessageBox.Show(ex.ToString, "Ошибка подключения к Серверу", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")
                        ConnectButtonChecked = False
                    End Try
                End If
            Else
                text = "Неправильное имя Сервера или адрес."
                DelegateAppendOutput(text, MessageBoxIcon.Error)
                'MessageBox.Show("Неправильное имя Сервера или адрес.", "Невозможно подключиться к Серверу", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")
                ConnectButtonChecked = False
            End If
        Else
            text = "Разрыв соединения с Сервером."
            ShowMsgText(text, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")

            LabelStatusClient_adapter.Image = My.Resources.ledCornerGray
            LabelStatusClient_receive.Image = My.Resources.ledCornerGray
            PictureBoxConnectionServer.BackgroundImage = My.Resources.ledCornerGray
            'StatusLabelClient_send.Image = My.Resources.ledCornerGray

            'tbНепр.Checked = False
            If ConnectionClient IsNot Nothing Then
                'Application.DoEvents()
                CType(mTaskDoMonitorCheckDataFromServer.AsyncState, ConnectionInfoClient).IsCancel = True
                ConnectionClient.Close()

                Application.DoEvents()
                Sleep(100)
                Application.DoEvents()
            End If

            mTaskDoMonitorCheckDataFromServer = Nothing
            ConnectionClient = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Выполняемая в потоке задача обработки сообщений Сервера.
    ''' </summary>
    ''' <param name="obj">пустой параметр (без него ругается)</param>
    Private Sub DoMonitorCheckDataFromServer(obj As Object)
        'Const CAPTION As String = "DoMonitorConnections"
        ' Создать делегат для обновления выходного дисплея
        Dim doAppendOutput As New Action(Of String, Integer, MessageBoxIcon)(AddressOf AppendOutput)
        '' Создать делегат для обновления метки числа соединений
        'Dim doUpdateConnectionCountLabel As New Action(Of Integer)(AddressOf UpdateConnectionCountLabel)

        If mTaskDoMonitorCheckDataFromServer IsNot Nothing Then
            ' вместо _ConnectionClient использую _ConnectionMontiorWithServer.AsyncState, что в принципе одно и тоже
            Dim connInfo As ConnectionInfoClient = CType(mTaskDoMonitorCheckDataFromServer.AsyncState, ConnectionInfoClient)

            Do
                If connInfo IsNot Nothing AndAlso connInfo.Tcp_Client.Connected Then
                    Dim sync As Integer = Interlocked.CompareExchange(syncPointDoMonitor, 1, 0)

                    If sync = 0 Then
                        If connInfo.DataQueue.Count > 0 Then
                            'SyncLock OnTimedEventLock
                            While connInfo.DataQueue.Count > 0
                                ParsePacketFromServerAndRespond(connInfo, connInfo.DataQueue.Dequeue)
                            End While
                            'End SyncLock
                        End If
                        syncPointDoMonitor = 0 ' освободить
                    End If
                End If

                If connInfo Is Nothing OrElse mTaskDoMonitorCheckDataFromServer Is Nothing Then
                    ' скорее всего обрыв соединения
                    Exit Sub
                End If
                ' Завершить цикл избегая напрасную трату времени CPU
                mTaskDoMonitorCheckDataFromServer.Wait(1) '(1)
            Loop While Not connInfo.IsCancel

            Try
                ' при закрытии приложения может не сработать
                RegistrationEventLog.EventLog_MSG_CONNECT($"<{NameOf(DoMonitorCheckDataFromServer)}> Монитор Остановлен.")
                ' обновить метку числа соединений и вывести в отчёт статус 
                'Invoke(doUpdateConnectionCountLabel, -1)
                Invoke(doAppendOutput, "Монитор Остановлен.", KEY_RICH_TEXT_SERVER)
            Catch ex As Exception
            End Try
        End If
    End Sub
#End Region

    Private Const CommandOff As String = "Off"
    Private Const CommandOn As String = "On"

    ''' <summary>
    ''' Разобрать пакет от Сервера и ответить
    ''' </summary>
    ''' <param name="info"></param>
    ''' <param name="arrMsgFromClientBytes"></param>
    Private Sub ParsePacketFromServerAndRespond(info As ConnectionInfoClient, arrMsgFromClientBytes As Byte())
        ' Надо разобрать ответ от запросов двух видов:
        ' 1 - запрос на получение значений каналов от Сервера
        ' 2 - запрос на значение управляющей команды (он отсылается во время отправки Серверу значений собранных сетевых переменных)
        ' разбор пакета учитывает эти различия
        ' частота 1 запроса 100 Гц, 2 запроса с частотой отсылки значений сетевых переменных

        Dim I As Integer ' счётчик разобранных каналов
        'Dim CMD As Integer ' номер команды
        Dim commandServer As Integer
        Dim valueOut As Double
        Dim valueTime As String = Nothing
        Dim bytesToRead As Integer ' сколько байт прочитать для разбора
        Dim arrBytes As Byte() ' временный буфер
        'Dim CmdBodyLength As Integer ' длина тела данных
        Dim offset, offsetSuccess As Integer ' смещение позиции чтения
        'Dim InputMsgText As String ' имя команды или текст сообщения
        Dim countItterationДанные As Integer ' countItterationУправления,
        Dim msgFromClientLength As Integer ' = MsgFromClientBytes.Length
        Dim hash As UInt32
        Dim isCommandResponseServer As Boolean ' пришёл ответ от Сервера на запрос управляющей команды выставляемой панелью управления

        If arrMsgFromClientBytes Is Nothing Then Exit Sub
        msgFromClientLength = arrMsgFromClientBytes.Length

        ' проверить не было ли переноса остатка с предыдущей обработки
        If info.LostBytes.Length > 0 Then
            arrMsgFromClientBytes = (info.LostBytes.Concat(arrMsgFromClientBytes)).ToArray
            msgFromClientLength = arrMsgFromClientBytes.Length
            info.LostBytes = New Byte() {} ' буфер с разорванной командой очистить
        End If

        Try
            '_lock.EnterWriteLock()

            'If MsgFromClientLength > conLengthInfo Then 
            Do
                'проверить возможность считать 10 байт
                'BytesToRead = 10
                'Array.Copy(MsgFromClientBytes, Offset, Bytes, 0, BytesToRead)
                'Offset += BytesToRead
                bytesToRead = 1
                Re.Dim(arrBytes, bytesToRead - 1)
                Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                offset += bytesToRead
                commandServer = arrBytes(0)

                If commandServer = CommandSetServer.Double_3 OrElse
                    commandServer = CommandSetServer.SmallString_51 OrElse
                    commandServer = CommandSetServer.Byte_1 OrElse
                    commandServer = CommandSetServer.Integer_2 OrElse
                    commandServer = CommandSetServer.Anything_4 OrElse
                    commandServer = CommandSetServer.LongString_52 OrElse
                    commandServer = CommandSetServer.ChannelProperty_53 Then

                    'считать hash
                    bytesToRead = 4
                    Re.Dim(arrBytes, bytesToRead - 1)
                    Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                    hash = CUInt(BitConverter.ToInt32(arrBytes, 0))
                    offset += (bytesToRead + 5) '1 валидность 4 - время

                    Select Case commandServer
                        Case CommandSetServer.Byte_1 ' Пакет byte  1 byte
                            bytesToRead = 1
                            arrBytes = New Byte(bytesToRead - 1) {}
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            ' или command = MsgFromClientBytes(Offset + 1)
                            offset += bytesToRead

                            If hash = gHashСommandCh Then
                                command = arrBytes(0)
                                isCommandResponseServer = True
                            End If
                            Exit Select
                        Case CommandSetServer.Integer_2 ' Пакет integer  4 byte 
                            bytesToRead = 4
                            arrBytes = New Byte(bytesToRead - 1) {}
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead

                            If hash = gHashСommandCh Then
                                command = BitConverter.ToInt32(arrBytes, 0)
                                isCommandResponseServer = True
                            End If
                            Exit Select
                        Case CommandSetServer.Double_3 ' Пакет double 8 byte
                            bytesToRead = 8
                            arrBytes = New Byte(bytesToRead - 1) {}
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead

                            If hash = gHashСommandCh Then
                                command = Convert.ToInt32(BitConverter.ToDouble(arrBytes, 0))
                                isCommandResponseServer = True
                            End If
                            Exit Select
                        Case CommandSetServer.Anything_4
                            bytesToRead = 4
                            arrBytes = New Byte(bytesToRead - 1) {}
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead
                            Exit Select
                        Case CommandSetServer.SmallString_51 ' Пакет Small string 
                            ' считать поле Length 
                            bytesToRead = 1
                            Re.Dim(arrBytes, bytesToRead - 1)
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead
                            bytesToRead = arrBytes(0)

                            arrBytes = New Byte(bytesToRead - 1) {} 'Small string 
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead
                            ' хрень несусветная
                            If hash = gHashСommandCh Then
                                If gASCII_Encoding.GetString(arrBytes) = CommandOn Then
                                    command = 1
                                Else
                                    command = 0
                                End If
                                'command = Integer.Parse(gASCII_Encoding.GetString(arrBytes))' приходит Off и будет ошибка
                                isCommandResponseServer = True
                            End If
                            ' вернуть назад размерность 
                            Exit Select
                        Case CommandSetServer.LongString_52 ' Пакет Description 
                            ' считать поле Length 
                            bytesToRead = 2
                            Re.Dim(arrBytes, bytesToRead - 1)
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead
                            bytesToRead = BitConverter.ToInt16(arrBytes, 0)

                            'Bytes = New Byte(BytesToRead - 1) {} ' Long string 
                            'Array.Copy(MsgFromClientBytes, Offset, Bytes, 0, BytesToRead)
                            offset += bytesToRead

                            ' вернуть назад размерность 
                            Exit Select
                        Case CommandSetServer.ChannelProperty_53
                            ' сместиться на 4 байта
                            offset += 4

                            ' считать поле Length 
                            bytesToRead = 2
                            Re.Dim(arrBytes, bytesToRead - 1)
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead
                            bytesToRead = BitConverter.ToInt16(arrBytes, 0)

                            arrBytes = New Byte(bytesToRead - 1) {} ' Property value  
                            Array.Copy(arrMsgFromClientBytes, offset, arrBytes, 0, bytesToRead)
                            offset += bytesToRead

                            ' вернуть назад размерность 
                            Exit Select
                        Case Else
                            Exit Select
                    End Select

                    If isCommandResponseServer Then
                        '' не обрабатывается для АРМ
                        '' для обновления счётчиков
                        'info.PacketsThresholdSendCount += 1
                        'If info.PacketsThresholdSendCount >= THRESHOLD_COUNT Then
                        '    'обновить сообщение на экране в случае фоновой задачи
                        '    info.PacketsSend += THRESHOLD_COUNT
                        '    Dim doUpdateStatusLabelClient_send As New Action(Of Long)(AddressOf UpdateStatusLabelClient_send)
                        '    Invoke(doUpdateStatusLabelClient_send, info.PacketsSend)
                        '    info.PacketsThresholdSendCount = 0
                        'End If
                    Else
                        ' разбор значений запрошенных каналов
                        Select Case commandServer
                            'Case CommandSetServer.Default_0
                            '    ' ни чего не делать
                            '    Exit Select
                            Case CommandSetServer.Byte_1
                                valueOut = arrBytes(0)
                                ' накопить буфер Arr_DoubleValue
                                ConnectionClient.AcquisitionValueOfDouble(I) = valueOut ' I + info.PacketsThresholdCount / 10 + (info.PacketsThresholdCount + 1) / 100 + (info.PacketsThresholdCount + 2) / 10000 'ValueOut
                                Exit Select
                            Case CommandSetServer.Integer_2
                                valueOut = BitConverter.ToInt32(arrBytes, 0)
                                ' накопить буфер Arr_DoubleValue
                                ConnectionClient.AcquisitionValueOfDouble(I) = valueOut
                                Exit Select
                            Case CommandSetServer.Double_3
                                valueOut = BitConverter.ToDouble(arrBytes, 0)
                                ' накопить буфер Arr_DoubleValue
                                ConnectionClient.AcquisitionValueOfDouble(I) = valueOut ' I + info.PacketsThresholdCount / 10 + (info.PacketsThresholdCount + 1) / 100 + (info.PacketsThresholdCount + 2) / 10000
                                'Case Command.Anything_4
                                Exit Select
                            Case CommandSetServer.SmallString_51, CommandSetServer.ChannelProperty_53
                                'Dim Encoding As New System.Text.ASCIIEncoding 'UTF8Encoding
                                valueTime = gASCII_Encoding.GetString(arrBytes)

                                If Not Double.TryParse(valueTime, valueOut) Then
                                    If valueTime <> CommandOff Then
                                        If Not Double.TryParse(valueTime, timeNumberStyle, culture, valueOut) Then
                                            valueOut = GetTimeFromString(valueTime)
                                        End If
                                    End If
                                End If

                                ' накопить буфер Arr_DoubleValue
                                ConnectionClient.AcquisitionValueOfDouble(I) = valueOut
                                Exit Select
                            Case Else
                                Exit Select
                        End Select

                        If commandServer <> CommandSetServer.Anything_4 Then ' при CMD=4 проглатывается инкремент индекса чтобы не возникало смещение значений каналов
                            I += 1

                            If I = ConnectionClient.CountSelectedNameChannels Then
                                ' для обновления счётчиков
                                info.PacketsThresholdCount += 1

                                If info.PacketsThresholdCount = THRESHOLD_COUNT Then
                                    ' обновить сообщение на экране в случае с таймером
                                    info.PacketsReceive += THRESHOLD_COUNT
                                    Dim doUpdatePacketsReceiveLabel As New Action(Of Long)(AddressOf UpdatePacketsReceiveLabel)
                                    Invoke(doUpdatePacketsReceiveLabel, info.PacketsReceive)
                                    info.PacketsThresholdCount = 0
                                End If

                                I = 0
                                offsetSuccess = offset
                                countItterationДанные += 1
                                'Exit Do ' за не имением более толкового решения сцепки пакетов
                            End If
                        End If
                    End If
                    'Else
                    '    Exit Sub
                End If
            Loop While offset < msgFromClientLength ' обработать следующую команду накопленную в очереди команд 
            'End If

            '_lock.ExitWriteLock()
        Catch ex As ArgumentException
            ' тело команды разорвано между пакета
            'Dim StatrComm As Integer = Offset
            ' запомнить остаток
            Dim Lenght As Integer = msgFromClientLength - offsetSuccess
            Re.Dim(info.LostBytes, Lenght - 1)
            Array.Copy(arrMsgFromClientBytes, offsetSuccess, info.LostBytes, 0, Lenght)
            'info.LostBytes = MsgFromClientBytes
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Один раз в секунду обновить метку полученных пакетов.
    ''' </summary>
    ''' <param name="PacketsReceive"></param>
    ''' <remarks></remarks>
    Private Sub UpdatePacketsReceiveLabel(PacketsReceive As Long)
        LabelStatusClient_receive.Text = $"Получено {PacketsReceive.ToString}"
    End Sub

    '''' <summary>
    '''' Один раз в секунду обновить метку полученных пакетов.
    '''' </summary>
    '''' <param name="PacketsSend"></param>
    '''' <remarks></remarks>
    'Private Sub UpdateStatusLabelClient_send(PacketsSend As Long)
    '    StatusLabelClient_send.Text = $"Отправлено {PacketsSend.ToString}"
    'End Sub

    ''' <summary>
    ''' пользовательское событие наследуется от EventArgs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AcquiredDataEventArgs
        Inherits EventArgs

        ' сюда можно напихать другие свойства
        Public Sub New()
        End Sub

        ' получить массив собранных значений
        Public Sub New(ByRef parametersAcquiredData As Double())
            Me.ParametersAcquiredData = parametersAcquiredData
        End Sub

        ' можно передать все накопленные
        ' arrСреднее(TempПараметр.ИндексВМассивеПараметров, x)
        ' а можно и конкретно осредненные или собранные
        ' arrПарамНакопленные(N) = dblСреднее

        Public Property ParametersAcquiredData() As Double()
    End Class

    Private Sub ConnectionClient_SyncSocketClientAcquiredData(sender As Object, e As ConnectionInfoClient.AcquiredDataEventArgs) Handles ConnectionClient.SyncSocketClientAcquiredData
        ' arrДанныеСервераЗначение = e.arrDataTCP
        ' на самом деле e.arrDataTCP это arrДанныеСервераЗначение

        If RegistrationMain IsNot Nothing AndAlso RegistrationMain.IsRun Then
            ' для обновления формы varViewServerChannelsForm
            RaiseEvent EventMainAcquiredDataForChannelsForm(Me, New AcquiredDataEventArgs(e.ArrDataTCP))
            'RegistrationMain.TCP_AcquiredData(e.arrDataTCP) так вызывать напрямую нельзя
        End If
    End Sub

    '''' <summary>
    '''' Обработчик события получения данных от CompactRio и дальнейшая передача данных посредством перевызова в базовой форме.
    '''' Вызов события из события таймера OnTimed_mmTimer формы CjmpactRio.
    '''' Заменил напрямой вызов метода  RegistrationMain.AcquiredData() в события таймера OnTimed_mmTimer.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub GFormTestCompactRio_CompactRioAcquiredData(sender As Object, e As FormTestCompactRio.AcquiredDataEventArgs) Handles GFormTestCompactRio.CompactRioAcquiredData
    '    If RegistrationMain IsNot Nothing AndAlso RegistrationMain.IsRun Then
    '        RaiseEvent EventMainAcquiredDataForChannelsForm(Me, New AcquiredDataEventArgs(e.CompactRioChannelsData))
    '    End If
    'End Sub

#Region "Запуск Сбора на шасси"
    'Private Sub ActivateTargetButton_Click(sender As Object, e As EventArgs) Handles ActivateTargetButton.Click
    '    ActivateTarget(ActivateTargetButton.Checked)
    'End Sub

    Private Sub ActivateTarget()
        IsSuccessLoadSettingsClientTCP()

        If Not CheckExistServerCfglmzXml(PathServerCfglmzXml) Then
            Throw New FileNotFoundException("Неправильный путь к конфигурационному файлу стенда №:" & StandNumber, PathServerCfglmzXml)
        End If

        Dim xdoc As XDocument = XDocument.Load(PathServerCfglmzXml)
        HostName = xdoc.<Cell>.<Config>.<Net>.<Server>.@HostIP
        PortTCP = CInt(xdoc.<Cell>.<Config>.<Net>.<Server>.@TcpPort)
        Dim ServerHost As String = xdoc.<Cell>.<Config>.<Net>.<Server>.@Host

        TextBoxServerIP.Text = HostName
        TextBoxServerPort.Text = PortTCP.ToString
        ValidateChildren() ' Значение true, если все дочерние объекты были успешно проверены; в противном случае — false (там установка значения _ServerAddress)
    End Sub

    'Private Sub InvokeServeActivate(start As Boolean)
    '    Dim doInvokeActivate As New Action(Of Boolean)(AddressOf delegateServeActivate)
    '    Invoke(doInvokeActivate, start)
    'End Sub

    'Private Sub delegateServeActivate(start As Boolean)
    '    ActivateTarget(start)
    'End Sub
#End Region

    ''' <summary>
    ''' Попытка заново переопределить путь к конфигурационному xml файлу
    ''' </summary>
    Private Function ReloadCfgPath() As Boolean
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ReloadCfgPath)}> Переопределить путь к конфигурационному xml файлу")
        Dim success As Boolean = False

        Try
            Dim fileName As String = Nothing

            ' Открыть диалог
            Dim dlg As New OpenFileDialog() With {.DefaultExt = "xml",
                                                  .Filter = "Xml Files|*.xml",
                                                  .InitialDirectory = "Сеть",
                                                  .Title = "Переопределить путь к конфигурационному xml файлу",
                                                  .RestoreDirectory = True}
            If dlg.ShowDialog() = DialogResult.OK Then
                fileName = dlg.FileName
            End If

            If fileName IsNot Nothing AndAlso fileName.EndsWith("xml") Then
                PathServerCfglmzXml = fileName
                success = True
            End If
        Catch ex As Exception
            Const caption As String = "Ошибка при переопределении пути к конфигурационному xml файлу"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return success
    End Function

    Private keyConfig As Integer ' последняя загруженная конфигурация LastTCPkeyConfig

    ''' <summary>
    ''' Считать настройки TCP Клиента.
    ''' Считывание настроек из файла Опции.ini
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsSuccessLoadSettingsClientTCP() As Boolean
        Dim success As Boolean = False
        Dim I As Integer
        Dim numbersStand As New List(Of String) ' номера стендов
        Dim pathsServerCfg_xml As String() ' массив путей к конфигурационным файлам серверов

        numbersStand.AddRange(GetIni(PathOptions, "Stend", "Stends", "1,2,3,4,5").Split(CType(",", Char())))
        For I = numbersStand.Count - 1 To 0 Step -1
            If numbersStand(I) = "Клиент" Then
                numbersStand.RemoveAt(I)
                Exit For
            End If
        Next

        Re.Dim(pathsServerCfg_xml, numbersStand.Count - 1)
        ' считать пути ServerCfg_xml
        For I = 0 To numbersStand.Count - 1
            pathsServerCfg_xml(I) = GetIni(PathOptions, "ServerCfg_xml", "Stend" & numbersStand(I), "\\Stend_1\c\Нужно ввести путь.xml") 'по умолчанию
        Next

        Try
            ' узнать путь конфигурации XML Сервера для выбранного стенда
            Dim iniStandNumber As String = GetIni(PathOptions, "Stend", "Stend", "1")

            For I = 0 To numbersStand.Count - 1
                If numbersStand(I) = iniStandNumber Then
                    PathServerCfglmzXml = pathsServerCfg_xml(I)
                    Exit For
                End If
            Next

            If CheckExistServerCfglmzXml(PathServerCfglmzXml) = False Then
                'Throw New FileNotFoundException("Неправильный путь к конфигурационному файлу стенда №:" & mstrНомерСтенда, pathServerCfglmzXml)
                Do While ReloadCfgPath() = False
                    MessageBox.Show("Необходимо переопределить путь к конфигурационному xml файлу", "Определить путь к Cfg файлу", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Loop
                WriteINI(PathOptions, "ServerCfg_xml", "Stend" & numbersStand(I), PathServerCfglmzXml)
            End If

            ' 2)номер изделия
            'strМодификация = sGetIni(strПутьОпции, "Product", "Modifikacia", cEngine39)
            'txtМодификацияИзделия.Text = strМодификация
            'Dim mkeyConfig As Integer = CInt(sGetIni(strПутьОпции, "Options", "LastTCPkeyConfig", "0"))
            keyConfig = CInt(GetIni(PathOptions, "Options", "LastTCPkeyConfig", "0"))

            If iniStandNumber <> StandNumber Then
                Const caption As String = "Изменён номер стенда"
                Dim text As String = $"Программа первоначально была загружена под номером стенда {StandNumber}{vbCrLf}В конфигураторе каналов был выбран стенд {iniStandNumber}{vbCrLf}Необходимо перезапустить программу!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Environment.Exit(0)
                success = False
            Else
                success = True
            End If
        Catch ex As Exception
            Const caption As String = "Ошибка при считывании настроек из файла <Опции.ini>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Попытка извлечения времени из строки неопределённой длины
    ''' </summary>
    ''' <param name="strValueTime"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTimeFromString(strValueTime As String) As Double
        Dim timeToDouble As Double
        Dim dateTimeString As String = String.Empty
        Const con21 As Integer = 21
        Const con22 As Integer = 22
        Const con26 As Integer = 26

        If strValueTime.Length = con22 Then ' ValueTime.Length > 11  andalso ValueTime.Length <= 22
            'Try
            'ValueTime = "26.07.2011 09:00:32,93"'для теста1
            'ValueTime = "26.07.2011 09:00:32.93"'для теста2
            'ValueTime = "26.07.2011 09:00:32:93"'для теста3

            'Dim v As DateTime = DateTime.Parse(ValueTime.Replace(".", "/").Replace(",", ".")) брыкается на тест для теста2
            'Dim v As DateTime = DateTime.Parse(ValueTime.Substring(0, 10).Replace(".", "/") & ValueTime.Substring(10, 12).Replace(",", "."))

            dateTimeString = $"{strValueTime.Substring(0, 10).Replace(".", "/")}{strValueTime.Substring(10, 9)}.{strValueTime.Substring(20, 2)}"
            'Dim v As DateTime = DateTime.Parse(DateTimeString)
            Dim varDateTime As DateTime
            If DateTime.TryParse(dateTimeString, varDateTime) Then
                timeToDouble = varDateTime.Hour * 3600.0 + varDateTime.Minute * 60.0 + varDateTime.Second + varDateTime.Millisecond / 1000
            End If

            'Return v.Hour * 3600.0 + v.Minute * 60.0 + v.Second + v.Millisecond / 1000
            'TimeToDouble = v.Hour * 3600.0 + v.Minute * 60.0 + v.Second + v.Millisecond / 1000
            'Catch ex As SocketException
            '    '    listener.Close()
            '    'Console.WriteLine("Socket exception: " + ex.SocketErrorCode)
            '    message = "Socket exception: " + ex.SocketErrorCode.ToString
            '    'Const caption As String = "GetMeasurementDataFromServer_LabView"
            '    'RegistrationEventLog.EventLog_CONNECT_FAILED(" <" & caption & "> " & message)
            '    If ParentIsTcpClientForm Then
            '        myThread = New Thread(AddressOf ParentTcpClientForm.ThreadFunction)
            '    Else
            '        myThread = New Thread(AddressOf ParentfrmMain.ThreadFunction)
            '    End If
            '    myThread.Start(message)
            'Catch ex As Exception
            '    'listener.Close()
            '    'Console.WriteLine("Exception: " & Convert.ToString(ex))
            '    message = "Exception: " & Convert.ToString(ex)
            '    'Const caption As String = "GetMeasurementDataFromServer_LabView"
            '    'RegistrationEventLog.EventLog_CONNECT_FAILED(" <" & caption & "> " & message)
            '    If ParentIsTcpClientForm Then
            '        myThread = New Thread(AddressOf ParentTcpClientForm.ThreadFunction)
            '    Else
            '        myThread = New Thread(AddressOf ParentfrmMain.ThreadFunction)
            '    End If
            '    myThread.Start(message)
            '    'Finally
            'End Try
            Return timeToDouble
        ElseIf strValueTime.Length = con21 Then
            'ValueTime = "01.01.2012 00:00:0.00" 'для теста4
            dateTimeString = $"{strValueTime.Substring(0, 10).Replace(".", "/")}{strValueTime.Substring(10, 8)}.{strValueTime.Substring(19, 2)}"
            Dim varDateTime As DateTime

            If DateTime.TryParse(dateTimeString, varDateTime) Then
                timeToDouble = varDateTime.Hour * 3600.0 + varDateTime.Minute * 60.0 + varDateTime.Second + varDateTime.Millisecond / 1000
            End If

            Return timeToDouble
        ElseIf strValueTime.Length = con26 Then
            'ValueTime = "20.01.2012 16:49:32.400000"'для теста5
            dateTimeString = strValueTime.Substring(0, 10).Replace(".", "/") & strValueTime.Substring(10, 12) '& "." & ValueTime.Substring(19, 2)
            Dim varDateTime As DateTime

            If DateTime.TryParse(dateTimeString, varDateTime) Then
                timeToDouble = varDateTime.Hour * 3600.0 + varDateTime.Minute * 60.0 + varDateTime.Second + varDateTime.Millisecond / 1000
            End If

            Return timeToDouble
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Проверка существования Сервера путём использования класса PingClient
    ''' </summary>
    ''' <param name="ServerHostIP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckPingClient(ServerHostIP As String) As Boolean
        Dim mPingClient As New PingClient(ServerHostIP) '"127.0.0.1")
        Dim regularThread As New Thread(AddressOf mPingClient.SendPing)
        ' запустить пинг в другом потоке, чтобы там запустить Методы Send и SendAsync 
        ' - выполняют асинхронную отправку сообщения запроса проверки связи ICMP на удаленный компьютер 
        ' и ожидают от него соответствующее сообщение ответа проверки связи ICMP. 
        regularThread.Start()

        ' ожидание окончания работы потока
        regularThread.Join()

        If mPingClient.SuccessPing = False Then
            Const caption As String = "Ping error."
            Dim text As String = $"Проверка связи с сервером по адресу <{ServerHostIP}>{vbCrLf}завершилась ошибкой: {vbCrLf}{mPingClient.PingDetails}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        Return mPingClient.SuccessPing
    End Function

#Region "WorkCheckConnection"
    ''' <summary>
    ''' В случае успеха запуска всех задач запустить задачу отслеживания соединения с Сервером.
    ''' </summary>
    ''' <param name="ct"></param>
    ''' <remarks></remarks>
    Private Sub WorkCheckConnection(ByVal ct As CancellationToken)
        ' Прерывание уже было запрошено?
        If ct.IsCancellationRequested = True Then
            'Console.WriteLine("Прерывание уже было запрошено до запуска.")
            'Console.WriteLine("Press Enter to quit.")
            ct.ThrowIfCancellationRequested()
        End If
        'Dim PacketsReceiveOld As Long
        'Dim СчётчикПовторов As Integer
        ' Внимание!!! Ошибка "OperationCanceledException was unhandled by user code"
        ' было вызвано здесь если "Just My Code"
        ' был включён и не может быть выключен. Исключение случилось
        ' Просто нажать F5 для продолжения выполнения кода

        Do
            If ct.IsCancellationRequested Then
                'ct.ThrowIfCancellationRequested() ' выйти по исключению
                Exit Do ' завершить задачу
            End If

            If ConnectionClient Is Nothing Then
                'TestSend = "WorkCheckConnection"
                Connect(ct)
            ElseIf ConnectionClient.Tcp_Client Is Nothing OrElse ConnectionClient.Tcp_Client.Connected = False Then
                Connect(ct)
            ElseIf ConnectionClient.Tcp_Client.ReceiveTimeout > 5000 Then
                Connect(ct)
            End If

            Sleep(5000)
        Loop While True
    End Sub

    Private Sub Connect(ByVal ct As CancellationToken)
        gClientSendData = False
        message = String.Empty
        Const CAPTION As String = "Connect"

        IsConnectedWithServer = False
        ' Создать a TCP/IP socket.
        'Dim listener As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        'listener = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            Do
                If ct.IsCancellationRequested Then
                    'ct.ThrowIfCancellationRequested() ' выйти по исключению
                    Exit Do ' завершить задачу
                End If

                ' Соединить сокет по адресу (remote endpoint).
                'listener.Connect(remoteEP)
                InvokeConnectClientStartStop(True)
                Sleep(5000)

                'If listener.Connected Then
                If ConnectionClient IsNot Nothing AndAlso ConnectionClient.Tcp_Client IsNot Nothing AndAlso ConnectionClient.Tcp_Client.Connected Then
                    'If _ConnectionClient Is Nothing Then 'AndAlso _ConnectionClient.Client.Connected

                    ' Запуск передачи данных на Сервер
                    InvokeClientSendData(True)
                    If ConnectionClient IsNot Nothing AndAlso ConnectionClient.Tcp_Client IsNot Nothing AndAlso ConnectionClient.Tcp_Client.Connected Then
                        IsConnectedWithServer = True
                    End If
                    'Exit Do
                Else
                    message = "Невозможно соединиться с Сервером" ' & Environment.NewLine
                End If

                ' Используя SelectWrite перечислитель получить Socket статус.
                'If listener.Poll(-1, SelectMode.SelectWrite) Then
                If ConnectionClient IsNot Nothing Then
                    If ConnectionClient.Tcp_Client.Client.Poll(-1, SelectMode.SelectWrite) Then
                        message = "Этот сокет предназначен для записи."
                    Else
                        If ConnectionClient.Tcp_Client.Client.Poll(-1, SelectMode.SelectRead) Then
                            message = "Этот сокет предназначен для чтения. "
                        Else
                            If ConnectionClient.Tcp_Client.Client.Poll(-1, SelectMode.SelectError) Then
                                message = "Сокет содержит ошибку."
                            End If
                        End If
                    End If
                End If

                myThread = New Thread(AddressOf ThreadFunction)
                myThread.Start(message)
            Loop Until IsConnectedWithServer

            ' код избыточен
            'If _ConnectionClient Is Nothing Then
            '    СоединениеУстановлено = False
            '    Exit Sub
            'End If

            message = $"Сокет соединился с {ConnectionClient.Tcp_Client.Client.RemoteEndPoint.ToString()}"
            myThread = New Thread(AddressOf ThreadFunction)
            myThread.Start(message)
            RegistrationEventLog.EventLog_MSG_CONNECT($"<{CAPTION}> {message}")
        Catch ex As SocketException
            'CloseConnection(connection)
            'Console.WriteLine("Socket exception: " + e.SocketErrorCode)
            message = "Ошибка исключения сокета: " & ex.SocketErrorCode.ToString
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {message}")

            myThread = New Thread(AddressOf ThreadFunction)
            myThread.Start(message)
        Catch ex As Exception
            'Console.WriteLine(e.ToString())
            message = ex.ToString()
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {message}")
            myThread = New Thread(AddressOf ThreadFunction)
            myThread.Start(message)
        End Try
    End Sub

#Region "включить индикатор"
    Private Sub InvokeClientSendData(Activate As Boolean)
        Dim doInvokeClientSendData As New Action(Of Boolean)(AddressOf DelegateClientSendData)
        Invoke(doInvokeClientSendData, Activate)
    End Sub
    Private Sub DelegateClientSendData(Activate As Boolean)
        If ConnectionClient IsNot Nothing AndAlso ConnectButtonChecked Then
            gClientSendData = Activate
        End If
        ShowReceiveData(True)
        'ShowSendDataClient(Activate)
    End Sub

    '''' <summary>
    '''' Обновление индикаторов и SubItems(3) при успешном соединении сокета
    '''' </summary>
    '''' <param name="sendStatus"></param>
    '''' <remarks></remarks>
    'Private Sub ShowSendDataClient(ByVal sendStatus As Boolean)
    '    If sendStatus Then
    '        StatusLabelClient_send.Image = My.Resources.ledCornerGreen
    '    Else
    '        StatusLabelClient_send.Image = My.Resources.ledCornerRed
    '    End If
    'End Sub

#End Region

#Region "Протоколирование"
    Private Sub InvokeAppendOutput(message As String, selectionModeIcon As MessageBoxIcon)
        Dim doAppendOutput As New Action(Of String, MessageBoxIcon)(AddressOf DelegateAppendOutput)
        Invoke(doAppendOutput, message, selectionModeIcon)
    End Sub

    Private Sub AppendOutput(message As String, RichTextBoxKey As Integer, selectionModeIcon As MessageBoxIcon)
        Dim tempRichTextBox As RichTextBox = RichTextBoxClient

        If tempRichTextBox.TextLength > 0 Then
            tempRichTextBox.AppendText(ControlChars.NewLine)
        End If

        tempRichTextBox.AppendText($"{DateTime.Now.ToLongTimeString} {message}")
        WriteTextToRichTextBox(tempRichTextBox, message, selectionModeIcon)
        tempRichTextBox.ScrollToCaret()
    End Sub

    Private Sub DelegateAppendOutput(message As String, selectionModeIcon As MessageBoxIcon)
        If RichTextBoxClient.TextLength > 0 Then
            RichTextBoxClient.AppendText(ControlChars.NewLine)
        End If

        RichTextBoxClient.AppendText($"{DateTime.Now.ToLongTimeString} {message}")
        WriteTextToRichTextBox(RichTextBoxClient, message, selectionModeIcon)
        RichTextBoxClient.ScrollToCaret()
        ShowReceiveData(True)
    End Sub

    Private Shared Sub WriteTextToRichTextBox(mRichTextBox As RichTextBox, message As String, selectionModeIcon As MessageBoxIcon)
        'RichTextBox.SelectedText = message + ControlChars.Cr
        mRichTextBox.SelectionStart = If(mRichTextBox.TextLength - message.Length < 0, 0, mRichTextBox.TextLength - message.Length)
        mRichTextBox.SelectionLength = message.Length

        Select Case selectionModeIcon
            Case MessageBoxIcon.Information 'i
                mRichTextBox.SelectionFont = New Font(FontFamily.GenericSansSerif, 10.0F)
                mRichTextBox.SelectionColor = Color.Blue
            Case MessageBoxIcon.Error
                mRichTextBox.SelectionFont = New Font("Arial", 10, FontStyle.Bold)
                mRichTextBox.SelectionColor = Color.Red
            Case MessageBoxIcon.Exclamation '!
                mRichTextBox.SelectionFont = New Font("Verdana", 10)
                mRichTextBox.SelectionColor = Color.Magenta
            Case MessageBoxIcon.Question '?
                mRichTextBox.SelectionFont = New Font("Arial", 10)
                mRichTextBox.SelectionColor = Color.Green
            Case Else
                mRichTextBox.SelectionFont = New Font("Arial", 10)
                mRichTextBox.SelectionColor = Color.Green
        End Select
    End Sub

    Private Sub ShowMsgText(inputMsgText As String, selectionModeIcon As MessageBoxIcon)
        If ConnectionClient IsNot Nothing AndAlso ConnectionClient.Tcp_Client IsNot Nothing AndAlso ConnectionClient.Tcp_Client.Connected Then
            Dim smallStringPakcetByte As Byte() = SendStatus(gPacketArraySmallString, gHashStatusPin, inputMsgText)
            ConnectionClient.Stream.Write(smallStringPakcetByte, 0, smallStringPakcetByte.Length)
        End If

        Dim doAppendOutput As New Action(Of String, MessageBoxIcon)(AddressOf DelegateAppendOutput)
        Invoke(doAppendOutput, inputMsgText, selectionModeIcon)
    End Sub
#End Region

    Private Sub ServerTextBox_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxServerIP.Validating
        ServerAddress = Nothing
        Dim remoteHost As IPHostEntry = Dns.GetHostEntry(TextBoxServerIP.Text)

        If remoteHost IsNot Nothing AndAlso remoteHost.AddressList.Length > 0 Then
            For Each deltaAddress As IPAddress In remoteHost.AddressList
                If deltaAddress.AddressFamily = AddressFamily.InterNetwork Then
                    ServerAddress = deltaAddress
                    DelegateAppendOutput($"host: {TextBoxServerIP.Text} IP: {deltaAddress.ToString}", MessageBoxIcon.Error)
                    Exit For
                End If
            Next
        End If

        If ServerAddress Is Nothing Then
            Const CAPTION As String = "ServerTextBox_Validating"
            Const text As String = "Невозможно разрешить адрес Сервера."
            RegistrationEventLog.EventLog_CONNECT_FAILED($"<{CAPTION}> {text}")
            MessageBox.Show(text, "Проверка адреса Сервера", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            TextBoxServerPort.SelectAll()
            e.Cancel = True
        End If
    End Sub

    Private Sub ServerPortTextBox_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxServerPort.Validating
        Dim deltaPort As Integer

        If Not Integer.TryParse(TextBoxServerPort.Text, deltaPort) OrElse deltaPort < 1 OrElse deltaPort > 65535 Then
            MessageBox.Show("Порт обязан быть числом между 1 и 65535.", "Неправильный номер порта", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            TextBoxServerPort.SelectAll()
            e.Cancel = True
        End If
    End Sub

    ''' <summary>
    ''' Установка индикатора связи с Сервером
    ''' </summary>
    ''' <param name="соединениеУстановлено"></param>
    ''' <remarks></remarks>
    Private Sub CheckConnection(соединениеУстановлено As Boolean)
        TextBoxConnectOk.Visible = соединениеУстановлено
        TextBoxConnectBad.Visible = Not соединениеУстановлено
        ButtonConnectRefresh.Visible = Not соединениеУстановлено
    End Sub

    ''' <summary>
    ''' Обновление индикаторов при успешном соединении сокета
    ''' </summary>
    ''' <param name="receiveStatus"></param>
    ''' <remarks></remarks>
    Private Sub ShowReceiveData(ByVal receiveStatus As Boolean)
        If receiveStatus Then
            If ConnectionClient IsNot Nothing Then
                LabelStatusClient_adapter.Text = $"local:{ConnectionClient.Local} remote:{ConnectionClient.Remote}"
                LabelStatusClient_receive.Image = My.Resources.ledCornerGreen
                PictureBoxConnectionServer.BackgroundImage = My.Resources.ledCornerGreen
            Else
                LabelStatusClient_receive.Image = My.Resources.ledCornerRed
                PictureBoxConnectionServer.BackgroundImage = My.Resources.ledCornerRed
            End If
        Else
            LabelStatusClient_receive.Image = My.Resources.ledCornerRed
            PictureBoxConnectionServer.BackgroundImage = My.Resources.ledCornerRed
        End If
    End Sub

#Region "Функция обратного вызова"
    ''' <summary>
    ''' Соединение установлено
    ''' </summary>
    ''' <returns></returns>
    Private Property IsConnectedWithServer As Boolean = False
    Private myThread As Thread

    Delegate Sub AddListItem(message As String)
    Private DelegateAddListItem As AddListItem

    ''' <summary>
    ''' функция обратного вызова из SyncSocketClient для отображения сообщений
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Private Sub AddListItemMethod(message As String)
        DelegateAppendOutput(message, MessageBoxIcon.Error)

        If ConnectionClient IsNot Nothing AndAlso ConnectionClient.Tcp_Client IsNot Nothing AndAlso ConnectionClient.Tcp_Client.Connected Then
            CheckConnection(ConnectionClient.Tcp_Client.Connected)
        Else
            CheckConnection(False)
        End If

        RegistrationEventLog.EventLog_MSG_CONNECT(message)
    End Sub

    Private Sub ThreadFunction(ByVal message As Object)
        'If _ConnectionClient IsNot Nothing Then ' событие может вызываться от таймера из другого потока, а основной поток уже завершен
        Dim myThreadClassObject As New MyThreadClass(Me, CStr(message))
        myThreadClassObject.Run()
        'End If
    End Sub

    Public Class MyThreadClass
        Implements IDisposable

        Private ReadOnly myFormControl1 As FormMainMDI

        'Public Sub New()
        'End Sub

        Public Sub New(myForm As FormMainMDI, message As String)
            myFormControl1 = myForm
            Me.message = message
        End Sub

        Private ReadOnly message As String
        Public Sub Run()
            If myFormControl1.IsHandleCreated Then
                myFormControl1.Invoke(myFormControl1.DelegateAddListItem, New Object() {message})
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Чтобы обнаружить избыточные вызовы

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' освободить управляемое состояние (управляемые объекты).
                    If myFormControl1 IsNot Nothing Then
                        myFormControl1.Dispose()
                    End If
                End If

                ' освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже Finalize().
                ' задать большие поля как null.
            End If

            disposedValue = True
        End Sub

        ' переопределить Finalize(), только если Dispose(ByVal disposing As Boolean) выше имеет код для освобождения неуправляемых ресурсов.
        Protected Overrides Sub Finalize()
            ' Не изменяйте этот код.  Поместите код очистки в расположенную выше команду Удалить(ByVal удаление как булево).
            Dispose(False)
            'MyBase.Finalize()
        End Sub

        ' Этот код добавлен редактором Visual Basic для правильной реализации шаблона высвобождаемого класса.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Не изменяйте этот код. Разместите код очистки выше в методе Dispose(disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class

#End Region
#End Region
#End Region
#End Region
End Class
