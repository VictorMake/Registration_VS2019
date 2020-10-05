Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.Text
Imports MathematicalLibrary
Imports NationalInstruments.DAQmx
Imports NationalInstruments.UI.WindowsForms

' применить стиль, которые перечислены в комбобоксе как строки
'Me._tableLP.GrowStyle = DirectCast([Enum].Parse(GetType(TableLayoutPanelGrowStyle), Me._combGrowStyle.SelectedItem.ToString()), TableLayoutPanelGrowStyle)

' 1. при загрузке проверяется наличие родительской формы
' если родительская загружена позже, то проверка каналов запускается из родительской
' 2. при выгрузке родительской формы раньше, сбрасывается на форме портов управления кнопка "Пуск", а значит н выходы портов
' 3. при выгрузке формы портов управления также сбрасываются порты
' 4. при нажатии кнопки "Пуск" проверяются наличие каналов управления, наличие каналов в сборщике  и правильность формул - иначе сброс
' 5. кнопка пуск становится доступной, если из проводника загружена программа управления портами
' 6. сигналы на выходе порта устанавливаются в соответствии с включением индикаторов, а при ручном управлении каждая линия отдельно
' 7. при удалении и добавлении конфигураций отразить их в combo, а в листе выбранных, если надо удалить, и заново перезагрузить, так как ссылки указывают на разные объекты
' 8. Не допускается применение порта платы в сборе, как вход в базе ChannelDigitalInput и этот же порт как выход.
' 9. В плате подключенной к корзине SCXI не все порты доступны для управления!!!

Friend Class FormDigitalPort
    Public Property FormMainReference() As FormMain
    ' классы дерева и листа
    Public DtvwDirectory As DirectoryTreeView
    Public FlvFiles As FileListView
    Private mivChecked As MenuItemView
    Private WithEvents TableLayoutPanelAllPorts As TableLayoutPanel
    Private Ports As Dictionary(Of String, Port)
    ' перейти к обобщенным коллекциям по имени устройства (не порта т.к. портов в устройстве может быть несколько)
    Private DigitalWriteTasks As New Dictionary(Of String, Task)
    Private DigitalSingleChannelWriters As New Dictionary(Of String, DigitalSingleChannelWriter)
    Private DataDigitalOutArrayDictionary As New Dictionary(Of String, Boolean())
    Private DigitalLineToStringsDictionary As New Dictionary(Of String, List(Of String))
    Private OutIndexes As New Dictionary(Of String, Integer) ' индекс реле управляемое линией порта (0, 1, 2...N)
    Private ReadOnly mOffColor As Color = Color.Silver
    'Private mOnColor As Color = Color.Red

    Private expression As String
    Private NameParametersForControl() As String ' имена Параметров Для Контрола
    Private Const capacityQueue As Integer = 18
    Private AlarmsQueue As New Queue(Of String)(capacityQueue * 5) ' на всякий случай побольше

    Private isObservationOnOccurrenceStart As Boolean ' наблюдение За Событиями Запущено
    Private isCheckLoadSuccess As Boolean ' проверка Загрузки Проведена
    Private isTaskRunningDigitalOutput As Boolean
    Private isUpdatingDataGridViewConfigurationFromParent As Boolean ' идет Обновление DataGridViewКонфигурация Из Верхнего Уровня
    Private isUpdatingDataGridViewFormulaFromParent As Boolean ' идет Обновление DataGridViewФормула Из Верхнего Уровня
    Private isUpdatingDataGridViewPortFromParent As Boolean ' идет Обновление DataGridViewПорт Из Верхнего Уровня
    Private ReadOnly rnd As New Random(DateTime.Now.Millisecond)

#Region "Form"
    Public Sub New()
        MyBase.New()

        MdiParent = MainMdiForm

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        ' узнать конфигурацию платы
        ' (Led.Tag = "SC1Mod<slot#>/port" & mНомерПорта.ToString & "/line" & i.ToString) '"SC" & НомерУстройства & "Mod<slot#>/port0/lineN" 'Dev0/port1/line0:2
        ' "SC" & НомерУстройства может ссылаться на на "Dev" & НомерУстройства 
        ' поэтому если есть ссылка "SC" & НомерУстройства = "Dev" & НомерУстройства , то порты с "Dev" не показывать
        deviceComboBox.Items.AddRange(DaqSystem.Local.Devices)

        If (deviceComboBox.Items.Count > 0) Then deviceComboBox.SelectedIndex = 0

        ComboBoxPhysicalPort.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External).Where(Function(nameDev) Not nameDev.StartsWith("Sim")).ToArray)
        'physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
        'physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))

        Dim WordsPhysicalChannel As String() = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External).Where(Function(nameDev) Not nameDev.StartsWith("Sim")).ToArray
        physicalChannelComboBox.Items.AddRange(WordsPhysicalChannel)

        If physicalChannelComboBox.Items.Count > 0 Then physicalChannelComboBox.SelectedIndex = 0

        LinqLineCount(WordsPhysicalChannel, ComboBoxPhysicalPort, ComboBoxLineCount)
        'Linq9()'Linq13()'Linq59()'Linq41()

        If (ComboBoxPhysicalPort.Items.Count > 0) Then
            ComboBoxPhysicalPort.SelectedIndex = 0
            InitializeDigitalTableLayoutPanelPort()
        End If

        OnScaleModeChanged()
        booleanComboBox.SelectedIndex = 0

        CheckCountsDigitalWriteTask(deviceComboBox, ComboBoxPhysicalPort, ComboDigitalWriteTask)
        ' работа с компонентом свойств National
        'plotLinePropertyEditor.Source = New PlotColorPropertyEditorSource(defaultWaveformPlot, "LineColor")
        'xAxisRangePropertyEditor.Source = New RangePropertyEditorSource(defaultXAxis, "Range")
        ''''''''''''''''''''''''''
        'For Each value As Object In [Enum].GetValues(GetType(BorderStyle))
        '    propertyEditorBorderStyleComboBox.Items.Add(value)
        '    If DefaultPropertyEditor.BorderStyle.Equals(value) Then
        '        propertyEditorBorderStyleComboBox.SelectedItem = value
        '    End If
        'Next
        'xAxisLabelFormatPropertyEditor.Source = New PropertyEditorSource(xAxis1.MajorDivisions, "LabelFormat")
    End Sub

    Private Sub FormDigitalPort_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.БитПорта' table. You can move, or remove it, as needed.
        'Me.БитПортаTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.БитПорта)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.Порты' table. You can move, or remove it, as needed.
        'Me.ПортыTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.Порты)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.АргументыДляФормулы' table. You can move, or remove it, as needed.
        'Me.АргументыДляФормулыTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.АргументыДляФормулы)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода' table. You can move, or remove it, as needed.
        'Me.ФормулаСрабатыванияЦифровогоВыходаTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода' table. You can move, or remove it, as needed.
        'Me.ТриггерСрабатыванияЦифровогоВыходаTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.КонфигурацияДействий' table. You can move, or remove it, as needed.
        'Me.КонфигурацияTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.КонфигурацияДействий)
        ''TODO: This line of code loads data into the 'ChannelsDigitalOutputDataSet.Действие' table. You can move, or remove it, as needed.
        'Me.ДействиеTableAdapter.Fill(Me.ChannelsDigitalOutputDataSet.Действие)

        WindowState = FormWindowState.Maximized
        FindFormRegistration()

        If (Not RegistrationFormName Is Nothing) AndAlso RegistrationFormName.IndexOf("Регистратор") <> -1 Then
            PopulateListParametersFromServer()
        End If

        Dim mOleDbConnection As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannelsDigitalOutput))
        ConfigurationTableAdapter.Connection = mOleDbConnection
        ActionTableAdapter.Connection = mOleDbConnection
        TriggerFireDigitalOutputTableAdapter.Connection = mOleDbConnection
        FormulaFireDigitalOutputTableAdapter.Connection = mOleDbConnection
        ArgumentsOfFormulaTableAdapter.Connection = mOleDbConnection
        PortsTableAdapter.Connection = mOleDbConnection
        BitsOfPortTableAdapter.Connection = mOleDbConnection

        ' стандартные методы заполнения при дезайне формы в коде заменим на созданные
        FillAllAdapters()
        FlvFiles = New FileListView()
        SplitContainerExplorer.Panel2.Controls.Add(FlvFiles)
        FlvFiles.Dock = DockStyle.Fill
        CreateTree() ' Создать экземпляр DirectoryTreeView и добавить OnAfterSelect обработчик события.
        DtvwDirectory = New DirectoryTreeView With {.Dock = DockStyle.Fill} ' там заполнить дерево dtvwDirectory.RefreshTree()
        SplitContainerExplorer.Panel1.Controls.Add(DtvwDirectory)
        ' Динамически добавить обработчик события AfterSelect.
        AddHandler DtvwDirectory.AfterSelect, AddressOf DirectoryTreeViewOnAfterSelect

        ' добавить меню команд View к основному меню.
        Dim menuView As New ToolStripMenuItem("&Вид подробности")
        MenuStrip1.Items.Add(menuView)
        ' добавить 4 меню View. Начать с создания массивов для конфигурирования каждого пункта.
        Dim viewsString As String() = {"&Плитка", "&Значки", "&Список", "&Таблица"}
        Dim views As View() = {View.LargeIcon, View.SmallIcon, View.List, View.Details}
        ' создать обработчики события.
        Dim eh As New EventHandler(AddressOf MenuOnViewSelect)

        For I As Integer = 0 To 3
            ' использовать пользовательский класс MenuItemView, который рассширяет MenuItem для поддержки View свойства.
            Dim miv As New MenuItemView() With {.Text = viewsString(I), .View = views(I), .Checked = False}
            ' ассоциировать ранее созданный обработчик с событием Click.
            AddHandler miv.Click, eh

            ' установить вид по умолчанию
            If I = 3 Then
                mivChecked = miv
                mivChecked.Checked = True
                FlvFiles.View = mivChecked.View
            End If

            menuView.DropDownItems.Add(miv) ' добавить новый пункт меню View.
        Next

        '' тест с добавлением пользовтельского класса смотри Module ModuleExplorer
        'dynamicObject = New DynamicObject()
        'dynamicObject.AddProperty(Of ВсеКонфигурации)("MyClass1 Param", mВсеКонфигурации, "MyClass1 Param Description", "MyClass1 types", False)
        ''PropertyGrid – удобный компонент для визуального редактирования свойств объектов. Объект для редактирования задается в дизайнере WinForms, либо непосредственно в коде:
        '' устанавливаем редактируемый объект
        'PropertyGrid2.SelectedObject = dynamicObject

        'RestorePos()' восстанавливаем положение окна
        'RestoreGridSplitterPos()' и разделителя колонок в гриде
        DtvwDirectory.Select()
        IsFrmDigitalOutputPortStart = True
        MainStatusStrip.Items(1).Text = "Установка " & StandNumber
        'ReDim_ParameterAccumulate(UBound(ParametersType))
        Re.Dim(ParameterAccumulate, UBound(ParametersType))
        TimerResize.Enabled = True ' включить таймер для определения размеров панели индикаторов
    End Sub

    Private Sub FormDigitalPort_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        IsFrmDigitalOutputPortStart = False
        IsMonitorDigitalOutputPort = False
        isObservationOnOccurrenceStart = False
        MainMdiForm.MenuNewWindowEvents.Enabled = True
        FormMainReference = Nothing

        For Each itemForm As Form In CollectionForms.Forms.Values
            If itemForm.Tag.ToString <> "" AndAlso (itemForm.Tag.ToString = TagFormDaughter) Then
                If itemForm.Text <> FormServiceBasesText Then
                    CType(itemForm, FormMain).MenuNewWindowEvents.Enabled = MainMdiForm.MenuNewWindowEvents.Enabled
                End If
            End If
        Next

        ' обнулить перед выгрузкой
        ' нулевые загрузки
        If isCheckLoadSuccess Then ExecuteOccurrencesConfiguration(True)

        Try
            StopTaskDigitalOutput()
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(FormDigitalPort_FormClosed)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        If PropertiesChanging Then
            Const caption As String = "Сохранение изменений"
            Dim text As String = "Были изменения при редактировании конфигураций" & vbCrLf & "Произвести сохранение изменений?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")
            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                TSButtonSaveInDataSet_Click(TSButtonSaveInDataSet, New EventArgs)
            End If
        End If

        'SavePos()' запоминаем положение окна
        'SaveGridSplitterPos()' и разделителя в гриде
        My.MySettings.[Default].Save() ' сохраним возможно измененные значения параметров
        ' решил убрать т.к. varМодульСбораКТManager могут быть Nothing
        'If Not (blnПодпискаНаСобытиеСбораВключена OrElse fMainForm.varМодульСбораКТManager.ЕстьПодключенныеМодули OrElse fMainForm.varМодульРасчетаManager.ЕстьПодключенныеМодули) Then Erase arrПарамНакопленные
        Dispose()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    ''' <summary>
    ''' Заполнить Все Адаптеры
    ''' </summary>
    Public Sub FillAllAdapters()
        ConfigurationTableAdapter.Fill(ChannelsDigitalOutputDataSet.КонфигурацияДействий)
        ' стандартные методы заполнения, их в коде заменим на созданные
        ActionTableAdapter.Fill(ChannelsDigitalOutputDataSet.Действие)
        TriggerFireDigitalOutputTableAdapter.Fill(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода)
        FormulaFireDigitalOutputTableAdapter.Fill(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода)
        ArgumentsOfFormulaTableAdapter.Fill(ChannelsDigitalOutputDataSet.АргументыДляФормулы)
        PortsTableAdapter.Fill(ChannelsDigitalOutputDataSet.Порты)
        BitsOfPortTableAdapter.Fill(ChannelsDigitalOutputDataSet.БитПорта)
    End Sub

    ''' <summary>
    ''' Найти Форму Регистратора
    ''' </summary>
    Private Sub FindFormRegistration()
        TSButtonStart.Enabled = False

        If RegistrationFormName <> "" Then
            For Each itemForm As Form In CollectionForms.Forms.Values
                If itemForm.Text = RegistrationFormName Then
                    FormMainReference = CType(itemForm, FormMain)
                    Exit For
                End If
            Next
        End If
    End Sub

    ' Класс расширяет MenuItem для поддержки View свойства.
    Class MenuItemView
        Inherits ToolStripMenuItem
        Public View As View
    End Class

    ''' <summary>
    ''' Обработчик события AfterSelect для DirectoryTreeView, вызываемый
    ''' FileListView для показа содержимого выбранной директории.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="tvea"></param>
    Public Sub DirectoryTreeViewOnAfterSelect(ByVal obj As Object, ByVal tvea As TreeViewEventArgs)
        FlvFiles.ShowNodes(tvea.Node)
    End Sub

    ''' <summary>
    ''' Обработчик события OnViewSelect для View пункта меню.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="ea"></param>
    Sub MenuOnViewSelect(ByVal obj As Object, ByVal ea As EventArgs)
        mivChecked.Checked = False ' Снять чек выделенного элемента.
        mivChecked = CType(obj, MenuItemView) ' привести тип источника к MenuItemView, заново присвоить и отметить его.
        mivChecked.Checked = True
        FlvFiles.View = mivChecked.View ' Изменить вид просмотра файлов в FileListView контроле.
    End Sub
#End Region

#Region "Добавление конфигураций в лист и бокс"
    ''' <summary>
    ''' Добавить Конфигурации в ComboBox и ListBox
    ''' </summary>
    Public Sub PopulateComboBoxAndListBoxConfigurations()
        ComboBoxConfigurations.Items.Clear()

        If GlobalAllConfigurations.ВсеКонфигурации IsNot Nothing Then
            For Each mКонфигурация As Конфигурация In GlobalAllConfigurations.ВсеКонфигурации
                ComboBoxConfigurations.Items.Add(mКонфигурация)
            Next

            If ComboBoxConfigurations.Items.Count > 0 Then
                ComboBoxConfigurations.DisplayMember = "Name"
                ComboBoxConfigurations.ValueMember = "keyКонфигурацияДействия"
                ComboBoxConfigurations.SelectedIndex = 0
                ToolStripButtonNew.Enabled = True
            Else
                ToolStripButtonNew.Enabled = False
            End If

            ' не работает, так как после модификации в mВсеКонфигурации не все элементы оставшиеся в ListBoxГруппаКонфигураций различны
            'For Each mКонфигурация As Конфигурация In mВсеКонфигурации.ListВсеКонфигурации
            '    If Not ListBoxГруппаКонфигураций.Items.Contains(mКонфигурация) Then
            '        ListBoxГруппаКонфигураций.Items.Remove(mКонфигурация)
            '    Else
            '        ' вначале удалить, а затем добавить т.к. объекты могут не совсем совпадать 
            '        ListBoxГруппаКонфигураций.Items.Remove(mКонфигурация)
            '        ListBoxГруппаКонфигураций.Items.Add(mКонфигурация)
            '    End If
            'Next

            Dim success As Boolean
            For I As Integer = ListBoxGroupConfigurations.Items.Count - 1 To 0 Step -1
                Dim listBoxConfig As Конфигурация = CType(ListBoxGroupConfigurations.Items(I), Конфигурация)
                'For Each mКонфигурация2 As Конфигурация In ListBoxГруппаКонфигураций.Items' не работае при изменении в цикле
                success = False
                For Each itemConfiguration As Конфигурация In GlobalAllConfigurations.ВсеКонфигурации
                    If listBoxConfig.Name = itemConfiguration.Name Then
                        success = True
                        ' вначале удалить, а затем добавить т.к. объекты могут не совсем совпадать 
                        ListBoxGroupConfigurations.Items.Remove(listBoxConfig)
                        ListBoxGroupConfigurations.Items.Add(itemConfiguration)
                        Exit For
                    End If
                Next

                ' остальные удалить
                If Not success Then ListBoxGroupConfigurations.Items.Remove(listBoxConfig)
            Next
        End If
    End Sub
#End Region

#Region "Загрузить Все Конфигурации Испытания"
    ''' <summary>
    ''' Загрузить Все Конфигурации Испытания
    ''' </summary>
    Private Sub LoadAllSelectedConfigurations()
        Dim result As New StringBuilder("Были обнаружены следующие проблемы:") ' для накопления сообщений
        ' сделать все проверки только в первый запуск с формой сообщения о проверке
        ' в последующем, если проверка успешна и изменений в содержании и количестве конфигураций не было, то
        ' заново делать проверку при запуске не надо
        SetLedOffColor()
        ReloadListBoxGroupConfigurations()
        ' в коллекции содержатся все каналы по устройствам
        DigitalLineToStringsDictionary.Clear()
        isCheckLoadSuccess = True ' логическая переменная сообщает о проблеме

        If ListBoxGroupConfigurations.Items.Count = 0 Then
            result.AppendLine("Устройства в цикле отсутствуют.")
            isCheckLoadSuccess = False
        End If

        For Each itemConfiguration As Конфигурация In ListBoxGroupConfigurations.Items
            For Each itemAction As Действие In itemConfiguration.Действия
                ConfigurePort(itemConfiguration, itemAction, result)
                ConfigureTrigger(itemConfiguration, itemAction, result)
                ConfigureFormula(itemConfiguration, itemAction, result)
            Next 'Действие
        Next 'Конфигурация

        If isCheckLoadSuccess Then ' проба сконфигурировать железо
            ConfigureDigitalWriteTasks(result)
        End If

        If Not isCheckLoadSuccess Then ShowSendError(result)
    End Sub

    ''' <summary>
    ''' Конфигурация DigitLineToStringCollection
    ''' </summary>
    ''' <param name="itemConfiguration"></param>
    ''' <param name="itemAction"></param>
    ''' <param name="result"></param>
    Private Sub ConfigurePort(itemConfiguration As Конфигурация, itemAction As Действие, result As StringBuilder)
        For Each itemPort As Порт In itemAction.Порты
            Dim namePort As String = String.Empty
            Dim nameDevice As String = String.Empty
            GetPortName(itemPort, namePort, nameDevice)

            If Ports Is Nothing Then Exit For
            ' поиск по контролам
            For Each itemDicPort As Port In Ports.Values
                If itemDicPort.DeviceName = namePort Then
                    For Each itemBit As Бит In itemPort.Биты 'Dev0/port1/line0
                        ' Строковая заготовка
                        Dim textMessage As String
                        If itemPort.НомерМодуляКорзины = "" Then
                            textMessage = $"Конфигурация:{itemConfiguration.Name}{vbCrLf}Действие:{itemAction.Name}{vbCrLf}Плата:{itemPort.НомерУстройства}{vbCrLf}Порт:{itemPort.НомерПорта}{vbCrLf}Бит:{itemBit.НомерБита}{vbCrLf}"
                        Else
                            textMessage = $"Конфигурация:{itemConfiguration.Name}{vbCrLf}Действие:{itemAction.Name}{vbCrLf}Корзина:{itemPort.НомерУстройства}{vbCrLf}Модуль:{itemPort.НомерМодуляКорзины}{vbCrLf}Порт:{itemPort.НомерПорта} & {vbCrLf}Бит:{itemBit.НомерБита}{vbCrLf}"
                        End If

                        If itemDicPort.LineCount >= itemBit.НомерБита AndAlso itemDicPort.TableLayoutPanel.Controls.ContainsKey("LedPortLine" & itemBit.НомерБита.ToString) Then
                            Dim tempLed As Led = CType(itemDicPort.TableLayoutPanel.Controls.Item("LedPortLine" & itemBit.НомерБита.ToString), Led)
                            If tempLed.OffColor = Color.Olive Then
                                textMessage &= "Бит с данным номером уже используется в предыдущей конфигурации"
                                result.AppendLine(textMessage)
                                isCheckLoadSuccess = False
                            Else 'OK
                                tempLed.Caption = itemBit.Name
                                tempLed.OffColor = Color.Olive
                                ' так как биты дважды не используются 
                                'strDigitLineCollection.Add(NamePort & "/line" & mБит.НомерБита)

                                ' проверка если девайса еще не было то создать коллекцию и добавить в словарь New List(Of String)
                                If Not DigitalLineToStringsDictionary.ContainsKey(nameDevice) Then
                                    DigitalLineToStringsDictionary.Add(nameDevice, New List(Of String))
                                End If
                                DigitalLineToStringsDictionary(nameDevice).Add($"{namePort}/line{itemBit.НомерБита}")
                            End If
                        Else
                            If itemPort.НомерМодуляКорзины = "" Then
                                textMessage &= "Нет соответствующего устройства в компьютере."
                            Else
                                textMessage &= "Нет соответствующего модуля в корзине."
                            End If

                            result.AppendLine(textMessage)
                            isCheckLoadSuccess = False
                        End If
                    Next
                End If
            Next
        Next ' Порт
    End Sub

    ''' <summary>
    ''' Конфигурация Конфигурация.ТриггерСрабатыванияЦифровогоВыхода.ИндексВМассивеПараметров
    ''' </summary>
    ''' <param name="itemConfiguration"></param>
    ''' <param name="itemAction"></param>
    ''' <param name="result"></param>
    Private Sub ConfigureTrigger(itemConfiguration As Конфигурация, itemAction As Действие, result As StringBuilder)
        For Each itemTrigger As ТриггерСрабатыванияЦифровогоВыхода In itemAction.ТриггерыСрабатыванияЦифровогоВыхода
            Dim indexParam As Integer = Array.IndexOf(NameParametersForControl, itemTrigger.ИмяКанала)
            If indexParam = -1 Then
                result.AppendLine($"Конфигурация:{itemConfiguration.Name} Действие:{itemAction.Name} Триггер:{itemTrigger.Name} задан параметр {itemTrigger.ИмяКанала}")
                result.AppendLine("В каналах опроса такого имени нет.")
                isCheckLoadSuccess = False
            Else
                ' индекс в массиве arrСписокПараметровКонтроля собранных значение находится не в strИменаПараметровДляКонтрола
                itemTrigger.ИндексВМассивеПараметров = IndexParametersForControl(indexParam)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Конфигурация Действие.ФормулыСрабатыванияЦифровогоВыхода.АргументДляФормулы.ИндексВМассивеПараметров 
    ''' </summary>
    ''' <param name="itemConfiguration"></param>
    ''' <param name="itemAction"></param>
    ''' <param name="result"></param>
    Private Sub ConfigureFormula(itemConfiguration As Конфигурация, itemAction As Действие, result As StringBuilder)
        For Each itemFormula As ФормулаСрабатыванияЦифровогоВыхода In itemAction.ФормулыСрабатыванияЦифровогоВыхода
            For Each itemArg As АргументДляФормулы In itemFormula.АргументыДляФормулы
                Dim indexParam As Integer = Array.IndexOf(NameParametersForControl, itemArg.ИмяКанала)
                If indexParam = -1 Then
                    result.AppendLine($"Конфигурация:{itemConfiguration.Name} Действие:{itemAction.Name} Формула:{itemFormula.Формула} Аргумент:{itemArg.Name} задан канал {itemArg.ИмяКанала}")
                    result.AppendLine("В каналах опроса такого имени нет.")
                    isCheckLoadSuccess = False
                    ' проверить что имя аргумента начинается с ARG... наверно не надо т.к. проверка формулы
                    If itemArg.Name.IndexOf("ARG") = -1 Then
                        result.AppendLine($"Конфигурация:{itemConfiguration.Name} Действие:{itemAction.Name} Формула:{itemFormula.Формула} Аргумент:{itemArg.Name} имя аргумента должно иметь формат: ARGn - где n порядковый номер")
                        isCheckLoadSuccess = False
                    End If
                Else
                    ' индек в массиве arrСписокПараметровКонтроля собранных значение находится не в strИменаПараметровДляКонтрола
                    itemArg.ИндексВМассивеПараметров = IndexParametersForControl(indexParam)
                End If
            Next
            ' здесь проба вычисления формулы
            If TestFunction(itemFormula.Формула) = False Then
                result.AppendLine($"Конфигурация:{itemConfiguration.Name} Действие:{itemAction.Name} Формула:{itemFormula.Формула} содержит ошибку!")
                isCheckLoadSuccess = False
            End If
        Next
    End Sub

    ''' <summary>
    ''' Конфигурация:
    ''' DigitalWriteTaskDictionary
    ''' WriterDigitalLineDictionary
    ''' DataDigitalOutArrayDictionary
    ''' IndexDictionary
    ''' </summary>
    ''' <param name="result"></param>
    Private Sub ConfigureDigitalWriteTasks(result As StringBuilder)
        Dim tempTaskRunning As Boolean = isTaskRunningDigitalOutput ' в основной форме frmMain по этой переменной определяются запущенные задачи
        isTaskRunningDigitalOutput = False

        If DigitalWriteTasks.Count > 0 Then
            For Each DigitalWriteTask In DigitalWriteTasks.Values
                If tempTaskRunning Then DigitalWriteTask.Stop()

                DigitalWriteTask.Dispose()
                DigitalWriteTask = Nothing
            Next
        End If

        DigitalWriteTasks.Clear()
        DigitalSingleChannelWriters.Clear()
        DataDigitalOutArrayDictionary.Clear()
        OutIndexes.Clear()

        Try
            For Each keyTaskAsDeviceName In DigitalLineToStringsDictionary.Keys
                ' определить число каналов для конкретного устройства
                Dim dataDigitalOutArray(DigitalLineToStringsDictionary(keyTaskAsDeviceName).Count - 1) As Boolean
                DataDigitalOutArrayDictionary.Add(keyTaskAsDeviceName, dataDigitalOutArray)
                ' создать новую задачу для конкретного устройства
                DigitalWriteTasks.Add(keyTaskAsDeviceName, New Task(keyTaskAsDeviceName))
                ' суммировать каналы в строку
                Dim PhysicalChannels As String = Join(DigitalLineToStringsDictionary(keyTaskAsDeviceName).ToArray, ", ")
                ' добавить каналы в задачу
                DigitalWriteTasks(keyTaskAsDeviceName).DOChannels.CreateChannel(PhysicalChannels, "", ChannelLineGrouping.OneChannelForAllLines)

                'DigitalWriteTaskDictionary("doTask").Timing.SampleTimingType = SampleTimingType.OnDemand
                ' On Demand нужен наверно для платы, а для корзины наверно не нужен

                '  Write digital port data. WriteDigitalSingChanSingSampPort writes a single sample
                '  of digital data on demand, so no timeout is necessary.
                ' создать писателя для задачи и добавить в коллекцию
                DigitalSingleChannelWriters.Add(keyTaskAsDeviceName, New DigitalSingleChannelWriter(DigitalWriteTasks(keyTaskAsDeviceName).Stream))

                ' проверить корректность задачи
                DigitalWriteTasks(keyTaskAsDeviceName).Control(TaskAction.Verify)
                'DigitalWriteTaskDictionary("doTask").Start()
                'WriterDigitalLine.SynchronizeCallbacks = True
                'WriterDigitalLine.BeginWriteSingleSampleMultiLine(True, dataDigitalOutArray, New AsyncCallback(AddressOf OnCallbackDataWritten), DigitalWriteTask)
                OutIndexes.Add(keyTaskAsDeviceName, 0)
            Next

            isTaskRunningDigitalOutput = True
        Catch ex As Exception
            result.AppendLine(ex.ToString)
            isCheckLoadSuccess = False
            StopTaskDigitalOutput()
        End Try
    End Sub

    ''' <summary>
    ''' Заново занести в лист выбранные конфигурации
    ''' </summary>
    Private Sub ReloadListBoxGroupConfigurations()
        For Each itemConfiguration As Конфигурация In GlobalAllConfigurations.ВсеКонфигурации
            For Each listBoxConfig As Конфигурация In ListBoxGroupConfigurations.Items
                '    If ListBoxГруппаКонфигураций.Items.Contains(mКонфигурация) Then
                ' не работает при обновлении в mВсеКонфигурации т. к. объекты в ListBoxГруппаКонфигураций разные
                If itemConfiguration.Name = listBoxConfig.Name Then
                    ' вначале удалить а затем добавить т.к. объекты могут не совсем совпадать 
                    ListBoxGroupConfigurations.Items.Remove(listBoxConfig)
                    ListBoxGroupConfigurations.Items.Add(itemConfiguration)
                    Exit For
                End If
            Next
        Next
    End Sub

    Private Sub ShowSendError(ByVal inResult As StringBuilder)
        Const TextMessage As String = "Исполнение конфигураций наблюдений за событиями невозможно до устранения всех ошибок!"
        ShowMessageOnStatusPanel(TextMessage)
        inResult.AppendLine(TextMessage)
        Const caption As String = "Обнаружены следующие проблемы"
        Dim text As String = inResult.ToString
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
    End Sub

    'Private Const SleepTime As Integer = 20 '500
    'Private Sub OnCallbackDataWritten(ByVal result As IAsyncResult)
    '    Try
    '        If taskRunningDigitalOutput Then
    '            If DigitalWriteTask Is result.AsyncState Then
    '                WriterDigitalLine.EndWrite(result)
    '                System.Threading.Thread.Sleep(SleepTime)
    '                'заново запустить
    '                WriterDigitalLine.BeginWriteSingleSampleMultiLine(True, dataDigitalOutArray, New AsyncCallback(AddressOf OnCallbackDataWritten), DigitalWriteTask)
    '            End If
    '        End If
    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.ToString)
    '         StopTaskDigitalOutput()
    '    End Try
    'End Sub

#End Region

#Region "Исполнение загрузки"
    ''' <summary>
    ''' Исполнить События Конфигураций
    ''' </summary>
    ''' <param name="setAllDeviceToNull"></param>
    Private Sub ExecuteOccurrencesConfiguration(ByVal setAllDeviceToNull As Boolean)
        Dim isNeedUpdateListEvents As Boolean ' обновить Лист Событий

        ' в цикле по устройствам
        ' по функции найти значение желательно без округления и задать UpdateOutput
        ' в порт значение
        ' UsePort0 = False
        ' проверяется по условию AND срабатывания всех триггеров в Действие -> ВсеТриггерыВДействииСработали
        ' проверяется по условию AND срабатывания всех формул в Действие -> ВсеФормулыВДействииСработали
        ' если ВсеТриггерыВДействииСработали OR ВсеФормулыВДействииСработали, то установить все линии порта в истину
        ' (линии не пересекаются по условию проверки для всех конфигураций)
        ' обнулить счетчик-индекс линии для указателя текущей линии
        ' не работает
        'For Each Key As String In IndexDictionary.Keys '(DeviceName)
        '    IndexDictionary(Key) = 0
        'Next
        Dim key() As String = OutIndexes.Keys.ToArray
        For J As Integer = 0 To key.Count - 1
            OutIndexes(key(J)) = 0
        Next

        For Each itemConfig As Конфигурация In ListBoxGroupConfigurations.Items
            For Each itemAction As Действие In itemConfig.Действия
                isNeedUpdateListEvents = CheckTrigersAndFormulas(itemAction, setAllDeviceToNull)
            Next 'Действие
        Next 'Конфигурация

        WritePortsMultiLine()

        If setAllDeviceToNull Then
            ' установить все линии в ноль
            isNeedUpdateListEvents = True
            AlarmsQueue.Enqueue("Установить все линии в ноль")
        End If

        If isNeedUpdateListEvents Then
            ListViewAlarms.Items.Clear()
            For I As Integer = 0 To AlarmsQueue.Count - 1 - capacityQueue
                AlarmsQueue.Dequeue()
            Next
            ListViewAlarms.Items.AddRange(AlarmsQueue.ToArray)
            ListViewAlarms.SelectedIndex = ListViewAlarms.Items.Count - 1
        End If

        ' если ошибки не произошло то:
        IsMonitorDigitalOutputPort = True
    End Sub

    ''' <summary>
    ''' Проверить в действии срабатывание триггеров и формул.
    ''' Установить биты портов и индикаторы.
    ''' Занести сообщение в очередь для вывода логгирования.
    ''' </summary>
    ''' <param name="itemAction"></param>
    ''' <param name="setAllDeviceToNull"></param>
    ''' <returns></returns>
    Private Function CheckTrigersAndFormulas(ByRef itemAction As Действие, setAllDeviceToNull As Boolean) As Boolean
        Dim isNeedUpdateListEvents As Boolean ' обновить Лист Событий
        Dim conditionOccurrenceTriggers As String = "" ' условия Сработавших Триггеров или Формул
        Dim conditionOccurrenceFormulas As String = "" ' условия Сработавших Триггеров или Формул
        Dim isAllFormulasInActionFire, isAllTriggersInActionFire As Boolean ' все Формулы или Триггеры В Действии Сработали

        If setAllDeviceToNull = False Then
            isAllTriggersInActionFire = CheckTriggers(itemAction, conditionOccurrenceTriggers)
            isAllFormulasInActionFire = CheckFormulas(itemAction, conditionOccurrenceFormulas)
        End If ' УстановитьВсеУстройстваВНуль = False

        For Each itemPort As Порт In itemAction.Порты
            Dim namePort As String = String.Empty
            Dim nameDevice As String = String.Empty
            GetPortName(itemPort, namePort, nameDevice)

            ' поиск по контролам
            For Each dictionaryPort As Port In Ports.Values
                If dictionaryPort.DeviceName = namePort Then
                    Dim valPort As Long = 0
                    For Each itemBit As Бит In itemPort.Биты 'Dev0/port1/line0
                        Dim tempLed As Led = CType(dictionaryPort.TableLayoutPanel.Controls.Item("LedPortLine" & itemBit.НомерБита.ToString), Led)

                        If setAllDeviceToNull Then ' установить все линии в ноль
                            If tempLed.Value <> False Then tempLed.Value = False
                        Else
                            If isAllTriggersInActionFire OrElse isAllFormulasInActionFire Then ' установить линию в единицу
                                If tempLed.Value <> True Then
                                    tempLed.Value = True
                                    ' значение индикатора изменилось, значит вывести в лист событий
                                    isNeedUpdateListEvents = True
                                    AlarmsQueue.Enqueue($"{namePort}/line{itemBit.НомерБита} сработал по {IIf(isAllTriggersInActionFire, "триггеру " & conditionOccurrenceTriggers, "")}{IIf(isAllFormulasInActionFire, "формуле " & conditionOccurrenceFormulas, "")}")
                                End If
                            Else ' установить линиию в ноль
                                If tempLed.Value <> False Then
                                    tempLed.Value = False
                                    ' значение индикатора изменилось, значит вывести в лист событий
                                    isNeedUpdateListEvents = True
                                    AlarmsQueue.Enqueue($"{namePort}/line{itemBit.НомерБита} выключен")
                                End If
                            End If
                        End If

                        If tempLed.Value = True Then
                            valPort += 1L << itemBit.НомерБита 'НомерБита
                        End If

                        ' разные конфигурации могут включать линии одного порта
                        ' индикаторы это отображают
                        'If taskRunningDigitalOutput Then dataDigitalOutArray(J) = TempLed.Value
                        'If taskRunningDigitalOutput Then dataDigitalOutArrayDictionary(DeviceName)(J) = TempLed.Value
                        If isTaskRunningDigitalOutput Then DataDigitalOutArrayDictionary(nameDevice)(OutIndexes(nameDevice)) = tempLed.Value
                        OutIndexes(nameDevice) += 1
                    Next
                    ' вывести на индикаторы двоичное значение включенных бит
                    CType(dictionaryPort.TableLayoutPanel.Controls.Item("Panel" & namePort).Controls.Item("NumericEditPort" & namePort), NumericEdit).Value = valPort 'String.Format("0x{0:X}", valPort)
                    Exit For
                End If
            Next 'по контролам TempPort
        Next 'Порт

        Return isNeedUpdateListEvents
    End Function

    ''' <summary>
    ''' Проверить срабатывание всех формул в условии
    ''' </summary>
    ''' <param name="conditionOccurrenceFormulas"></param>
    ''' <param name="itemAction"></param>
    Private Function CheckFormulas(ByRef itemAction As Действие, ByRef conditionOccurrenceFormulas As String) As Boolean
        Dim result, resultFunction As Double
        Dim isAllFormulasInActionFire As Boolean

        If itemAction.ФормулыСрабатыванияЦифровогоВыхода.Count > 0 Then
            isAllFormulasInActionFire = True

            For Each itemFormula As ФормулаСрабатыванияЦифровогоВыхода In itemAction.ФормулыСрабатыванияЦифровогоВыхода
                expression = itemFormula.Формула
                Dim formulaToString As String = itemFormula.Формула

                For Each itemArgument As АргументДляФормулы In itemFormula.АргументыДляФормулы
                    result = con9999999

                    If itemArgument.Приведение Then
                        result = ParameterAccumulate(itemArgument.ИндексВМассивеПараметров) * Math.Sqrt(Const288 / (TemperatureOfBox + Kelvin))
                    Else
                        result = ParameterAccumulate(itemArgument.ИндексВМассивеПараметров)
                    End If

                    expression = expression.Replace(itemArgument.Name, result.ToString)
                    formulaToString = formulaToString.Replace(itemArgument.Name, itemArgument.ИмяКанала)
                Next

                ' здесь вычисления формулы
                Try
                    Dim Eval As New JScriptUtil.ExpressionEvaluator
                    resultFunction = CDbl(Eval.Evaluate(expression))
                Catch ex As Exception
                    MessageBox.Show(ex.ToString, expression, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'Finally
                End Try

                Select Case itemFormula.ОперацияСравнения
                    Case ">"
                        isAllFormulasInActionFire = isAllFormulasInActionFire And (resultFunction > itemFormula.ВеличинаУсловия)
                    Case "<"
                        isAllFormulasInActionFire = isAllFormulasInActionFire And (resultFunction < itemFormula.ВеличинаУсловия)
                End Select

                If isAllFormulasInActionFire Then ' можно накопить
                    conditionOccurrenceFormulas += $"{formulaToString}{itemFormula.ОперацияСравнения}{itemFormula.ВеличинаУсловия}; "
                Else
                    Exit For ' дальше проверку Формул в данном действии можно не проводить, т.к. хотя-бы одина формула уже не сработала
                End If
            Next
        End If

        Return isAllFormulasInActionFire
    End Function

    ''' <summary>
    ''' Проверить срабатывание всех триггеров в условии
    ''' </summary>
    ''' <param name="itemAction"></param>
    ''' <param name="conditionOccurrenceTriggers"></param>
    ''' <returns></returns>
    Private Function CheckTriggers(ByRef itemAction As Действие, ByRef conditionOccurrenceTriggers As String) As Boolean
        Dim isAllTriggersInActionFire As Boolean

        If itemAction.ТриггерыСрабатыванияЦифровогоВыхода.Count > 0 Then
            isAllTriggersInActionFire = True

            For Each itemTrigger As ТриггерСрабатыванияЦифровогоВыхода In itemAction.ТриггерыСрабатыванияЦифровогоВыхода
                isAllTriggersInActionFire = AnyTriggersInActionFire(itemTrigger, isAllTriggersInActionFire)

                If isAllTriggersInActionFire Then ' можно накопить
                    conditionOccurrenceTriggers = AccumulateMessageOccurrenceTriggers(conditionOccurrenceTriggers, itemTrigger)
                Else
                    Exit For ' дальше проверку триггеров в данном действии можно не проводить, т.к. хотя-бы один триггер уже не сработал
                End If
            Next
        End If

        Return isAllTriggersInActionFire
    End Function

    Private Sub GetPortName(itemPort As Порт, ByRef namePort As String, ByRef nameDevice As String)
        If itemPort.НомерМодуляКорзины = "" Then
            ' плата
            namePort = $"Dev{itemPort.НомерУстройства}/port{itemPort.НомерПорта}"            '& "/line" & mБит.НомерБита
            nameDevice = "Dev" & itemPort.НомерУстройства
        Else 'модуль SCXI'SC1Mod<slot#>/port0/lineN" 
            namePort = $"SC{itemPort.НомерУстройства}Mod{itemPort.НомерМодуляКорзины}/port{itemPort.НомерПорта}"        '& "/line" & mБит.НомерБита
            nameDevice = $"SC{itemPort.НомерУстройства}Mod{itemPort.НомерМодуляКорзины}"
        End If
    End Sub

    Private Function AccumulateMessageOccurrenceTriggers(ByRef conditionOccurrenceTriggers As String, itemTrigger As ТриггерСрабатыванияЦифровогоВыхода) As String
        Select Case itemTrigger.ОперацияСравнения
            Case "между", "вне"
                conditionOccurrenceTriggers += $"{itemTrigger.ИмяКанала} {itemTrigger.ОперацияСравнения} {itemTrigger.ВеличинаУсловия} и {itemTrigger.ВеличинаУсловия2}; "
            Case Else
                conditionOccurrenceTriggers += $"{itemTrigger.ИмяКанала}{itemTrigger.ОперацияСравнения}{itemTrigger.ВеличинаУсловия}; "
        End Select

        Return conditionOccurrenceTriggers
    End Function

    ''' <summary>
    ''' Проверить срабатывание триггера
    ''' </summary>
    ''' <param name="inTrigger"></param>
    ''' <returns></returns>
    Private Function AnyTriggersInActionFire(inTrigger As ТриггерСрабатыванияЦифровогоВыхода, isAllTriggersInActionFire As Boolean) As Boolean
        Dim valueParameter As Double = ParameterAccumulate(inTrigger.ИндексВМассивеПараметров)

        Select Case inTrigger.ОперацияСравнения
            Case "="
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter = inTrigger.ВеличинаУсловия)
            Case "<>"
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter <> inTrigger.ВеличинаУсловия)
            Case "<"
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter < inTrigger.ВеличинаУсловия)
            Case ">"
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter > inTrigger.ВеличинаУсловия)
            Case "между"
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter > inTrigger.ВеличинаУсловия And valueParameter < inTrigger.ВеличинаУсловия2)
            Case "вне"
                isAllTriggersInActionFire = isAllTriggersInActionFire And (valueParameter < inTrigger.ВеличинаУсловия Or valueParameter > inTrigger.ВеличинаУсловия2)
        End Select

        Return isAllTriggersInActionFire
    End Function

    Private Sub WritePortsMultiLine()
        If isTaskRunningDigitalOutput Then
            Try
                For Each TaskAsDeviceName In DigitalSingleChannelWriters.Keys
                    DigitalSingleChannelWriters(TaskAsDeviceName).WriteSingleSampleMultiLine(True, DataDigitalOutArrayDictionary(TaskAsDeviceName))
                Next
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
                StopTaskDigitalOutput()
            End Try
        End If
    End Sub

    Private Sub StopTaskDigitalOutput()
        isTaskRunningDigitalOutput = False
        isObservationOnOccurrenceStart = False
        IsMonitorDigitalOutputPort = False

        'If DigitalWriteTask IsNot Nothing Then
        If DigitalWriteTasks.Count > 0 Then
            'DigitalWriteTask.Stop()
            For Each DigitalWriteTask In DigitalWriteTasks.Values
                DigitalWriteTask.Dispose()
                DigitalWriteTask = Nothing
            Next

            DigitalWriteTasks.Clear()
            DigitalSingleChannelWriters.Clear()
            DataDigitalOutArrayDictionary.Clear()
            DigitalLineToStringsDictionary.Clear()
        End If
    End Sub
#End Region

#Region "Initialize DigitalTableLayoutPanelPort"
    Private Sub InitializeDigitalTableLayoutPanelPort()
        Dim I As Integer

        TableLayoutPanelAllPorts = New TableLayoutPanel
        Ports = New Dictionary(Of String, Port)

        For I = 0 To ComboBoxPhysicalPort.Items.Count - 1
            Ports.Add(ComboBoxPhysicalPort.Items(I).ToString, New Port(ComboBoxPhysicalPort.Items(I).ToString, Convert.ToInt32(ComboBoxLineCount.Items(I)) - 1))
        Next

        TableLayoutPanelAllPorts.SuspendLayout()
        SplitContainerGlobal.Panel1.SuspendLayout()
        SplitContainerGlobal.SuspendLayout()
        SuspendLayout()
        '
        'TableLayoutPanelPort
        '
        TableLayoutPanelAllPorts.CellBorderStyle = TableLayoutPanelCellBorderStyle.[Single]
        'TableLayoutPanelPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        TableLayoutPanelAllPorts.ColumnCount = 1
        TableLayoutPanelAllPorts.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

        I = 0
        For Each itemPort As Port In Ports.Values
            'TableLayoutPanelPort.Controls.Add(TableLayoutPanelPort0, column, row)
            TableLayoutPanelAllPorts.Controls.Add(itemPort.TableLayoutPanel, 0, I)
            I += 1
        Next

        TableLayoutPanelAllPorts.Location = New Point(0, 0)
        TableLayoutPanelAllPorts.GrowStyle = TableLayoutPanelGrowStyle.AddRows '.FixedSize
        TableLayoutPanelAllPorts.Margin = New Padding(0)
        TableLayoutPanelAllPorts.Name = "TableLayoutPanelPorts"
        TableLayoutPanelAllPorts.RowCount = Ports.Count

        For I = 0 To TableLayoutPanelAllPorts.RowCount - 1
            TableLayoutPanelAllPorts.RowStyles.Add(New RowStyle(SizeType.Percent, CSng(100 / (TableLayoutPanelAllPorts.RowCount - 1))))
        Next

        TableLayoutPanelAllPorts.GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        SplitContainerGlobal.Panel1.Controls.Add(TableLayoutPanelAllPorts)

        TableLayoutPanelAllPorts.ResumeLayout(False)
        SplitContainerGlobal.Panel1.ResumeLayout(False)
        SplitContainerGlobal.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    ''' <summary>
    ''' Определить Высоту
    ''' </summary>
    Private Sub DefineSizetableLayoutPanelPort()
        Const HeightOfPort As Integer = 48
        If Ports Is Nothing Then Exit Sub

        ' если портов мало, то полное заполнение, иначе размер потра 48 пикселей
        If Ports.Count * HeightOfPort < SplitContainerGlobal.Size.Height - (SplitContainerGlobal.Panel2.Height + SplitContainerGlobal.SplitterWidth) Then
            SplitContainerGlobal.Panel1.AutoScroll = False
            TableLayoutPanelAllPorts.Dock = DockStyle.Fill
        Else
            TableLayoutPanelAllPorts.Size = New Size(SplitContainerGlobal.Size.Width - SplitContainerGlobal.SplitterWidth, Ports.Count * HeightOfPort)

            TableLayoutPanelAllPorts.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles)
            SplitContainerGlobal.Panel1.AutoScroll = True
        End If
    End Sub

#End Region

#Region "Массив Элементов"
    Private Sub OnScaleModeChanged()
        UpdateValuesListBox()

        Select Case (ScalingSwitchArray.ScaleMode.Type)
            Case ControlArrayScaleModeType.Automatic
                automaticScaleModePanel.Enabled = True
            Case ControlArrayScaleModeType.Fixed
                automaticScaleModePanel.Enabled = False
        End Select
    End Sub

    Private Sub UpdateValuesListBox()
        valuesListBox.Items.Clear()
        Dim switchArrayValues() As Boolean = ScalingSwitchArray.GetValues()

        For Each value As Boolean In switchArrayValues
            valuesListBox.Items.Add(value)
        Next
    End Sub

    Private Sub UpdateScalingLedArrayValues()
        Dim switchArrayValues() As Boolean = ScalingSwitchArray.GetValues()
        scalingLedArray.SetValues(switchArrayValues)

        Dim switchArrayValuesDouble As Double() = Array.ConvertAll(switchArrayValues, New Converter(Of Boolean, Double)(AddressOf BooleanToDouble))

        NumericEditArray1.SetValues(switchArrayValuesDouble)
    End Sub

    Private Sub UpdateNumericEditArray2()
        Dim switchArrayNubberDouble(ScalingSwitchArray.GetValues.Count - 1) As Double

        For I As Integer = 0 To switchArrayNubberDouble.Length - 1
            switchArrayNubberDouble(I) = (switchArrayNubberDouble.Length - 1) - I
        Next

        NumericEditArray2.SetValues(switchArrayNubberDouble)
    End Sub

    Private Function BooleanToDouble(ByVal switchArrayValue As Boolean) As Double
        Return CDbl(IIf(switchArrayValue, 1, 0))
    End Function

    Private Sub ScaleModePropertyEditor_SourceValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ScaleModePropertyEditor.SourceValueChanged
        scalingLedArray.ScaleMode = CType(ScaleModePropertyEditor.SourceValue, ControlArrayScaleMode)

        NumericEditArray1.ScaleMode = CType(ScaleModePropertyEditor.SourceValue, ControlArrayScaleMode)
        NumericEditArray2.ScaleMode = CType(ScaleModePropertyEditor.SourceValue, ControlArrayScaleMode)

        OnScaleModeChanged()
    End Sub

    Private Sub ScalingSwitchArray_ValuesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ScalingSwitchArray.ValuesChanged
        If IsHandleCreated Then
            UpdateScalingLedArrayValues()
            UpdateValuesListBox()
            UpdateNumericEditArray2()
        End If
    End Sub

    Private Sub ScalingSwitchArray_ItemPropertyChanged(ByVal sender As Object, ByVal e As UI.ControlArrayPropertyChangedEventArgs(Of UI.WindowsForms.Switch)) Handles ScalingSwitchArray.ItemPropertyChanged
        If e.PropertyName.Equals("Value") Then
            UpdateScalingLedArrayValues()
            UpdateValuesListBox()
        End If
    End Sub

    Private Sub ButtonRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRemove.Click
        If (valuesListBox.SelectedItems.Count > 0) Then
            For Each index As Integer In valuesListBox.SelectedIndices
                valuesListBox.Items.RemoveAt(index)
            Next

            Dim updatedValues() As Boolean = New Boolean((valuesListBox.Items.Count) - 1) {}

            Dim i As Integer = 0
            Do While (i < updatedValues.Length)
                updatedValues(i) = Convert.ToBoolean(valuesListBox.Items(i))
                i += 1
            Loop

            ScalingSwitchArray.SetValues(updatedValues)
        End If
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAdd.Click
        Dim switchArrayValues() As Boolean = ScalingSwitchArray.GetValues
        If switchArrayValues.Count = 32 Then Exit Sub

        Dim updatedValues As List(Of Boolean) = New List(Of Boolean)(switchArrayValues) From {Convert.ToBoolean(booleanComboBox.SelectedItem)}
        ScalingSwitchArray.SetValues(updatedValues.ToArray())
    End Sub

#End Region

#Region "InstrumentControlStrip"

#Region "Пример National при работе с контролом свойст"
    'Public Class CustomRangeConverter
    '    Inherits TypeConverter
    '    Private _baseConverter As TypeConverter

    '    Private Const Separator As Char = "-"c

    '    Public Sub New(ByVal baseConverter As TypeConverter)
    '        _baseConverter = baseConverter
    '    End Sub

    '    Public Overloads Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
    '        Return _baseConverter.CanConvertFrom(context, sourceType)
    '    End Function

    '    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
    '        If (Not (value Is Nothing)) AndAlso (TypeOf value Is String) Then
    '            Dim valueText As String = CType(value, String)
    '            valueText = valueText.Trim
    '            Dim parts As String() = valueText.Split(Separator)
    '            If parts.Length <= 1 OrElse parts.Length > 2 Then
    '                Throw New FormatException
    '            End If
    '            Dim minimum As Double
    '            Dim maximum As Double
    '            Try
    '                minimum = Double.Parse(parts(0), CultureInfo.CurrentCulture)
    '            Catch ex As FormatException
    '                Throw New FormatException("minimum", ex)
    '            End Try
    '            Try
    '                maximum = Double.Parse(parts(1), CultureInfo.CurrentCulture)
    '            Catch ex As FormatException
    '                Throw New FormatException("maximum", ex)
    '            End Try
    '            Return New Range(minimum, maximum)
    '        End If
    '        Return _baseConverter.ConvertFrom(context, culture, value)
    '    End Function

    '    Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
    '        Return _baseConverter.CanConvertTo(context, destinationType)
    '    End Function

    '    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
    '        If (Not (value Is Nothing)) AndAlso (TypeOf value Is Range) Then
    '            Dim range As Range = CType(value, Range)
    '            Return String.Format(CultureInfo.InvariantCulture, "{0:R} {1} {2:R}", range.Minimum, Separator, range.Maximum)
    '        End If
    '        Return _baseConverter.ConvertTo(context, culture, value, destinationType)
    '    End Function
    'End Class

    'Public Class RangePropertyEditorSource
    '    Inherits PropertyEditorSource
    '    Private _customRangeConverter As CustomRangeConverter

    '    Public Sub New(ByVal obj As Object, ByVal propertyName As String)
    '        MyBase.New(obj, propertyName)
    '        Dim rangeConverter As TypeConverter = TypeDescriptor.GetConverter(GetType(Range))
    '        _customRangeConverter = New CustomRangeConverter(rangeConverter)
    '    End Sub

    '    Public Overloads Overrides ReadOnly Property Converter() As TypeConverter
    '        Get
    '            Return _customRangeConverter
    '        End Get
    '    End Property
    'End Class

    'Public Class CustomGraphColorEditor
    '    Inherits UITypeEditor

    '    Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
    '        Return UITypeEditorEditStyle.None
    '    End Function

    '    Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
    '        Return MyBase.EditValue(context, provider, value)
    '    End Function
    'End Class

    'Public Class PlotColorPropertyEditorSource
    '    Inherits PropertyEditorSource
    '    Private _editor As CustomGraphColorEditor

    '    Public Sub New(ByVal obj As Object, ByVal propertyName As String)
    '        MyBase.New(obj, propertyName)
    '        _editor = New CustomGraphColorEditor
    '    End Sub

    '    Public Overloads Overrides ReadOnly Property Editor() As UITypeEditor
    '        Get
    '            Return _editor
    '        End Get
    '    End Property
    'End Class

    '''''''''''''''''''''''''''

    'Private ReadOnly Property DefaultPropertyEditor() As PropertyEditor
    '    Get
    '        Return interactionModePropertyEditor
    '    End Get
    'End Property

    'Private Sub OnPropertyEditorBorderStyleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles propertyEditorBorderStyleComboBox.SelectedIndexChanged
    '    For Each control As Control In graphGroupBox.Controls
    '        If (control.GetType() Is GetType(PropertyEditor)) Then
    '            CType(control, PropertyEditor).BorderStyle = CType(propertyEditorBorderStyleComboBox.SelectedItem, BorderStyle)
    '        End If
    '    Next
    'End Sub

#End Region

#Region "Сохранение/восстановление положения окна и разделителя в гриде"

    ' Сохранение положения окна
    'Private Sub SavePos()
    '    My.MySettings.[Default].MainFormState = WindowState
    '    If WindowState = FormWindowState.Normal Then
    '        My.MySettings.[Default].MainFormSize = Size
    '        My.MySettings.[Default].MainFormLocation = Location
    '    Else
    '        My.MySettings.[Default].MainFormSize = RestoreBounds.Size
    '        My.MySettings.[Default].MainFormLocation = RestoreBounds.Location
    '    End If

    '    Trace.WriteLine("MainForm::SavePos(): " & My.MySettings.[Default].MainFormSize.Height & ", " & My.MySettings.[Default].MainFormSize.Width & ", " & My.MySettings.[Default].MainFormLocation.X & ", " & My.MySettings.[Default].MainFormLocation.Y & ", " & My.MySettings.[Default].MainFormState)
    'End Sub

    ' Восстановление положения окна
    'Private Sub RestorePos()
    '    ' пока это положение первый раз не сохранено, при 
    '    ' попытке чтения будет кирдык
    '    Try
    '        Size = My.MySettings.[Default].MainFormSize
    '        Location = My.MySettings.[Default].MainFormLocation
    '        WindowState = My.MySettings.[Default].MainFormState
    '    Catch
    '        Trace.WriteLine("MainForm::RestorePos() exception")
    '    End Try

    '    Trace.WriteLine("MainForm::RestorePos(): " & Size.Height & ", " & Size.Width & ", " & Location.X & ", " & Location.Y & ", " & WindowState)
    'End Sub

    '    Как запомнить и восстановить положение разделителя колонок в PropertyGrid?
    'Это можно сделать при помощи следующих функций:
    'Вызываются они соответственно перед закрытием и перед загрузкой окна формы, содержащей PropertyGrid:

    '''' <summary>
    '''' Сохранение положения разделителя в гриде
    '''' </summary>
    'Private Sub SaveGridSplitterPos()
    '    Dim type As Type = propertyGrid1.[GetType]()
    '    Dim field As FieldInfo = type.GetField("gridView", BindingFlags.NonPublic Or BindingFlags.Instance)

    '    Dim valGrid As Object = field.GetValue(propertyGrid1)
    '    Dim gridType As Type = valGrid.[GetType]()
    '    My.MySettings.[Default].GridSplitterPos = CInt(gridType.InvokeMember("GetLabelWidth", BindingFlags.[Public] Or BindingFlags.InvokeMethod Or BindingFlags.Instance, Nothing, valGrid, New Object() {}))

    '    Trace.WriteLine("SaveGridSplitterPos(): " + CStr(My.MySettings.[Default].GridSplitterPos))
    'End Sub

    '''' <summary>
    '''' Восстановление положения разделителя в гриде
    '''' </summary>
    'Private Sub RestoreGridSplitterPos()
    '    Try
    '        Dim type As Type = propertyGrid1.[GetType]()
    '        Dim field As FieldInfo = type.GetField("gridView", BindingFlags.NonPublic Or BindingFlags.Instance)

    '        Dim valGrid As Object = field.GetValue(propertyGrid1)
    '        Dim gridType As Type = valGrid.[GetType]()
    '        gridType.InvokeMember("MoveSplitterTo", BindingFlags.NonPublic Or BindingFlags.InvokeMethod Or BindingFlags.Instance, Nothing, valGrid, New Object() {My.MySettings.[Default].GridSplitterPos})

    '        Trace.WriteLine("RestoreGridSplitterPos(): " + My.MySettings.[Default].GridSplitterPos)
    '    Catch
    '        Trace.WriteLine("MainForm::RestoreGridSplitterPos() exception")
    '    End Try
    'End Sub

#End Region

    ' Также нужно добавить обработчик события PropertyGrid – PropertyValueChanged для свойства, видимость которого зависит от другого свойства 
    'Private Sub propertyGrid1_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles propertyGrid1.PropertyValueChanged
    '    'If e.ChangedItem.PropertyDescriptor.DisplayName = "Номер Бита" Then
    '    '    'If Not ПроверкаПройдена(ВсеКонфигурации.НомерБита, e.ChangedItem.PropertyDescriptor.DisplayName) Then
    '    '    If Not ПроверкаПройдена(e.ChangedItem.Value, e.ChangedItem.PropertyDescriptor.DisplayName) Then
    '    '        'dynamicObject.НомерИзделия = e.OldValue
    '    '        'e.ChangedItem.Value = e.OldValue
    '    '    End If
    '    'End If
    '    propertyGrid1.Refresh()
    'End Sub

    'Private Function ПроверкаПройдена(ByVal objПоле As String, ByVal ОписаниеПоля As String) As Boolean 'System.Windows.Forms.TextBox)
    '    Dim mstrТекст As String
    '    'Dim dblTemp As Double
    '    Dim intTemp As Double

    '    'mstrТекст = Trim(objПоле.Text)
    '    mstrТекст = objПоле

    '    ' Если поле не заполнено, сообщить пользователю
    '    If mstrТекст = vbNullString Then
    '        MessageBox.Show("Поле <" & ОписаниеПоля & "> не может быть пустым!", "Редактирование свойства", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        'objПоле.Focus()
    '        'Exit Sub
    '        Return False
    '    End If
    '    Try
    '        'If Not IsNumeric(txtПолнК1.Text)
    '        'dblTemp = CDbl(objПоле.Text)
    '        'dblTemp = CDbl(objПоле)
    '        intTemp = CInt(objПоле)
    '    Catch ex As Exception
    '        MessageBox.Show("Поле <" & ОписаниеПоля & "> должно быть числом!", "Редактирование свойства", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        'objПоле.Focus()
    '        Return False
    '    End Try
    '    Return True
    'End Function

    ' элементы управления динамически добавляемые в панель для редактирования свойств элементов конфигурации

    ''' <summary>
    ''' Добавить ToolStripLabel в панель показа свойств узла
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="mText"></param>
    Private Sub AddToolStripLabel(ByVal Name As String, ByVal mText As String) ' As System.Windows.Forms.ToolStripLabel
        '"ToolStripLabel1"
        '"состав..."
        Dim ToolStripLabel As ToolStripLabel = New ToolStripLabel() With {.Name = Name, .Size = New Size(80, 22), .Text = mText}

        InstrumentControlStrip1.SuspendLayout()
        InstrumentControlStrip1.Items.AddRange(New ToolStripItem() {ToolStripLabel})
        InstrumentControlStrip1.ResumeLayout(False)
    End Sub

    ''' <summary>
    ''' Добавить ToolStripPropertyEditor в панель показа свойств узла
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <returns></returns>
    Private Function AddToolStripPropertyEditor(ByVal Name As String) As ToolStripPropertyEditor
        Dim tsPropertyEditor As ToolStripPropertyEditor =
            New ToolStripPropertyEditor() With {.AutoSize = False,
                                                .Name = Name,
                                                .RenderMode = PropertyEditorRenderMode.Inherit,
                                                .Size = New Size(120, 16)}

        InstrumentControlStrip1.SuspendLayout()
        InstrumentControlStrip1.Items.AddRange(New ToolStripItem() {tsPropertyEditor})
        InstrumentControlStrip1.ResumeLayout(False)

        Return tsPropertyEditor
    End Function

    ''' <summary>
    ''' Очистить Редакторы Свойств для InstrumentControlStrip
    ''' </summary>
    Public Sub ClearInstrumentControlStrip()
        If InstrumentControlStrip1.Items.Count > 1 Then
            For I As Integer = InstrumentControlStrip1.Items.Count - 1 To 1 Step -1
                InstrumentControlStrip1.Items.RemoveAt(I)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Создать Редакторы Свойств
    ''' </summary>
    ''' <param name="TypeName"></param>
    ''' <param name="obj"></param>
    Public Sub CreatePropertyEditorSource(ByVal TypeName As String, ByVal obj As Object)
        If IsNotCreatePropertyEditorSource Then Exit Sub

        ClearInstrumentControlStrip()

        Select Case TypeName
            Case GetType(ВсеКонфигурации).Name
                AddToolStripLabel("ToolStripLabel1", "состав:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "ВсеКонфигурации")
                Exit Select
            Case GetType(Конфигурация).Name
                AddToolStripLabel("ToolStripLabel1", "состав Действий:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "Действия")
                Exit Select
            Case GetType(Действие).Name
                AddToolStripLabel("ToolStripLabel1", "состав Триггеров:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "ТриггерыСрабатыванияЦифровогоВыхода")

                AddToolStripLabel("ToolStripLabel2", "состав Формул:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor2").Source = New PropertyEditorSource(obj, "ФормулыСрабатыванияЦифровогоВыхода")

                AddToolStripLabel("ToolStripLabel3", "состав Портов:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor3").Source = New PropertyEditorSource(obj, "Порты")
                Exit Select
            Case GetType(ТриггерСрабатыванияЦифровогоВыхода).Name
                AddToolStripLabel("ToolStripLabel1", "Имя Канала:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "ИмяКанала")

                AddToolStripLabel("ToolStripLabel2", "Операция Сравнения:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor2").Source = New PropertyEditorSource(obj, "ОперацияСравнения")

                AddToolStripLabel("ToolStripLabel3", "Величина Условия:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor3").Source = New PropertyEditorSource(obj, "ВеличинаУсловия")

                If CType(obj, ТриггерСрабатыванияЦифровогоВыхода).ОперацияСравнения = "между" OrElse CType(obj, ТриггерСрабатыванияЦифровогоВыхода).ОперацияСравнения = "вне" Then
                    AddToolStripLabel("ToolStripLabel4", "Величина Условия2:")
                    AddToolStripPropertyEditor("ToolStripPropertyEditor4").Source = New PropertyEditorSource(obj, "ВеличинаУсловия2")
                End If
                Exit Select
            Case GetType(ФормулаСрабатыванияЦифровогоВыхода).Name
                AddToolStripLabel("ToolStripLabel1", "Формула:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "Формула")

                AddToolStripLabel("ToolStripLabel2", "Операция Сравнения:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor2").Source = New PropertyEditorSource(obj, "ОперацияСравнения")

                AddToolStripLabel("ToolStripLabel3", "Величина Условия:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor3").Source = New PropertyEditorSource(obj, "ВеличинаУсловия")

                AddToolStripLabel("ToolStripLabel4", "состав Аргументов:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor4").Source = New PropertyEditorSource(obj, "АргументыДляФормулы")
                Exit Select
            Case GetType(АргументДляФормулы).Name
                AddToolStripLabel("ToolStripLabel1", "Имя Аргумента (ARG...):")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "Name")

                AddToolStripLabel("ToolStripLabel2", "Имя Канала:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor2").Source = New PropertyEditorSource(obj, "ИмяКанала")

                AddToolStripLabel("ToolStripLabel3", "Использовать приведение к САУ:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor3").Source = New PropertyEditorSource(obj, "Приведение")
                Exit Select
            Case GetType(Порт).Name
                AddToolStripLabel("ToolStripLabel1", "Номер Устройства:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "НомерУстройства")

                AddToolStripLabel("ToolStripLabel2", "Номер Модуля Корзины:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor2").Source = New PropertyEditorSource(obj, "НомерМодуляКорзины")

                AddToolStripLabel("ToolStripLabel3", "Номер Порта:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor3").Source = New PropertyEditorSource(obj, "НомерПорта")

                AddToolStripLabel("ToolStripLabel4", "состав Бит:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor4").Source = New PropertyEditorSource(obj, "Биты")
                Exit Select
            Case GetType(Бит).Name
                AddToolStripLabel("ToolStripLabel1", "Номер Бита:")
                AddToolStripPropertyEditor("ToolStripPropertyEditor1").Source = New PropertyEditorSource(obj, "НомерБита")
                Exit Select
        End Select
    End Sub
#End Region

#Region "DataSet"
    Private Sub TimerUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerUpdate.Tick
        TimerUpdate.Enabled = False

        Select Case TypeNode
            Case TypeGridDigitalNode.Configuration
                SaveModificationConfiguration()
            Case TypeGridDigitalNode.Action
                SaveModificationAction()
            Case TypeGridDigitalNode.Trigger
                SaveModificationTrigger()
            Case TypeGridDigitalNode.Formula
                SaveModificationFormula()
            Case TypeGridDigitalNode.Argument
                SaveModificationArgument()
            Case TypeGridDigitalNode.Port
                SaveModificationPort()
            Case TypeGridDigitalNode.Bit
                SaveModificationBit()
        End Select
    End Sub

    ''' <summary>
    ''' Обновить Действия
    ''' </summary>
    Private Sub UpdateAction()
        If BindingSourceConfiguration.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourceBit.DataSource = Nothing
            BindingSourceАргумент.DataSource = Nothing
            BindingSourcePort.DataSource = Nothing
            BindingSourceFormula.DataSource = Nothing
            BindingSourceTrigger.DataSource = Nothing
            BindingSourceAction.DataSource = Nothing
            Exit Sub
        Else
            isUpdatingDataGridViewConfigurationFromParent = True
            isUpdatingDataGridViewFormulaFromParent = True
            isUpdatingDataGridViewPortFromParent = True

            If BindingSourceAction.DataSource Is Nothing Then
                BindingSourceAction.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceAction.DataMember = ChannelsDigitalOutputDataSet.Действие.ToString
            End If

            If BindingSourceTrigger.DataSource Is Nothing Then
                BindingSourceTrigger.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceTrigger.DataMember = ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.ToString '"ТриггерСрабатыванияЦифровогоВыхода" '
            End If

            If BindingSourceFormula.DataSource Is Nothing Then
                BindingSourceFormula.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceFormula.DataMember = ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.ToString
            End If

            'If Me.BindingSourceАргумент.DataSource Is Nothing Then
            '    Me.BindingSourceАргумент.DataSource = ChannelsDigitalOutputDataSet
            '    Me.BindingSourceАргумент.DataMember = ChannelsDigitalOutputDataSet.АргументыДляФормулы.ToString
            'End If

            If BindingSourcePort.DataSource Is Nothing Then
                BindingSourcePort.DataSource = ChannelsDigitalOutputDataSet
                BindingSourcePort.DataMember = ChannelsDigitalOutputDataSet.Порты.ToString
            End If

            'If Me.BindingSourceБит.DataSource Is Nothing Then
            '    Me.BindingSourceБит.DataSource = ChannelsDigitalOutputDataSet
            '    Me.BindingSourceБит.DataMember = ChannelsDigitalOutputDataSet.БитПорта.ToString
            'End If

            isUpdatingDataGridViewConfigurationFromParent = False
            isUpdatingDataGridViewFormulaFromParent = False
            isUpdatingDataGridViewPortFromParent = False
        End If

        Dim keyConfigurationAction As Integer
        Dim row As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow = CType(CType(BindingSourceConfiguration.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.КонфигурацияДействийRow)
        'My.Forms.CustomersForm.Show(row.CustomerID)

        keyConfigurationAction = row.keyКонфигурацияДействия
        'If Me.CustomerID.Trim().Length > 0 Then
        If keyConfigurationAction > 0 Then
            isUpdatingDataGridViewConfigurationFromParent = True
            ActionTableAdapter.FillBykeyКонфигурацияДействия(ChannelsDigitalOutputDataSet.Действие, keyConfigurationAction)
            isUpdatingDataGridViewConfigurationFromParent = False
            UpdateTrigger()
            UpdateFormula()
            UpdatePort()
        End If
    End Sub

    ''' <summary>
    ''' Обновить Триггер
    ''' </summary>
    Private Sub UpdateTrigger()
        If isUpdatingDataGridViewConfigurationFromParent Then Exit Sub

        If BindingSourceAction.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourceTrigger.DataSource = Nothing
            Exit Sub
        Else
            If BindingSourceTrigger.DataSource Is Nothing Then
                BindingSourceTrigger.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceTrigger.DataMember = ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.ToString '"ТриггерСрабатыванияЦифровогоВыхода" '
            End If
        End If

        Dim keyAction As Integer
        Dim row As ChannelsDigitalOutputDataSet.ДействиеRow = CType(CType(BindingSourceAction.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.ДействиеRow)

        'If DataGridViewТриггер.Rows.Count > 0 Then DataGridViewТриггер.Rows.Clear()
        'DataGridViewТриггер.Rows.RemoveAt(0)
        keyAction = row.keyДействие
        If keyAction > 0 Then
            TriggerFireDigitalOutputTableAdapter.FillBykeyДествие(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода, keyAction)
        End If
    End Sub

    ''' <summary>
    ''' Обновить Формула
    ''' </summary>
    Private Sub UpdateFormula()
        If isUpdatingDataGridViewConfigurationFromParent Then Exit Sub

        If BindingSourceAction.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourceFormula.DataSource = Nothing
            Exit Sub
        Else
            isUpdatingDataGridViewFormulaFromParent = True
            If BindingSourceFormula.DataSource Is Nothing Then
                BindingSourceFormula.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceFormula.DataMember = ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.ToString
            End If
            isUpdatingDataGridViewFormulaFromParent = False
        End If

        Dim keyAction As Integer
        Dim row As ChannelsDigitalOutputDataSet.ДействиеRow = CType(CType(BindingSourceAction.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.ДействиеRow)
        keyAction = row.keyДействие

        If keyAction > 0 Then
            isUpdatingDataGridViewFormulaFromParent = True
            FormulaFireDigitalOutputTableAdapter.FillBykeyДействие(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода, keyAction)
            isUpdatingDataGridViewFormulaFromParent = False
            UpdateArgument()
        End If
    End Sub

    ''' <summary>
    ''' Обновить Порт
    ''' </summary>
    Private Sub UpdatePort()
        If isUpdatingDataGridViewConfigurationFromParent Then Exit Sub

        If BindingSourceAction.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourcePort.DataSource = Nothing
            Exit Sub
        Else
            isUpdatingDataGridViewPortFromParent = True
            If BindingSourcePort.DataSource Is Nothing Then
                BindingSourcePort.DataSource = ChannelsDigitalOutputDataSet
                BindingSourcePort.DataMember = ChannelsDigitalOutputDataSet.Порты.ToString
            End If
            isUpdatingDataGridViewPortFromParent = False
        End If

        Dim keyAction As Integer
        Dim row As ChannelsDigitalOutputDataSet.ДействиеRow = CType(CType(BindingSourceAction.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.ДействиеRow)
        keyAction = row.keyДействие

        If keyAction > 0 Then
            isUpdatingDataGridViewPortFromParent = True
            PortsTableAdapter.FillBykeyДействие(ChannelsDigitalOutputDataSet.Порты, keyAction)
            isUpdatingDataGridViewPortFromParent = False
            UpdateBit()
        End If
    End Sub

    ''' <summary>
    ''' Обновить Аргумент
    ''' </summary>
    Private Sub UpdateArgument()
        If isUpdatingDataGridViewFormulaFromParent Then Exit Sub

        If BindingSourceFormula.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourceАргумент.DataSource = Nothing
            Exit Sub
        Else
            If BindingSourceАргумент.DataSource Is Nothing Then
                BindingSourceАргумент.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceАргумент.DataMember = ChannelsDigitalOutputDataSet.АргументыДляФормулы.ToString
            End If
        End If

        Dim row As ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow = CType(CType(BindingSourceFormula.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаRow)

        If row IsNot Nothing Then
            If row.KeyFormula > 0 Then
                ArgumentsOfFormulaTableAdapter.FillByKeyFormula(ChannelsDigitalOutputDataSet.АргументыДляФормулы, row.KeyFormula)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Обновить Бит
    ''' </summary>
    Private Sub UpdateBit()
        If isUpdatingDataGridViewPortFromParent Then Exit Sub

        If BindingSourcePort.Current Is Nothing Then ' если родитель равен нулю, то и дочерний приравнять нулю
            BindingSourceBit.DataSource = Nothing
            Exit Sub
        Else
            If BindingSourceBit.DataSource Is Nothing Then
                BindingSourceBit.DataSource = ChannelsDigitalOutputDataSet
                BindingSourceBit.DataMember = ChannelsDigitalOutputDataSet.БитПорта.ToString
            End If
        End If

        Dim row As ChannelsDigitalOutputDataSet.ПортыRow = CType(CType(BindingSourcePort.Current, DataRowView).Row, ChannelsDigitalOutputDataSet.ПортыRow)

        If row IsNot Nothing Then
            If row.KeyПорта > 0 Then
                BitsOfPortTableAdapter.FillByKeyПорта(ChannelsDigitalOutputDataSet.БитПорта, row.KeyПорта)
            End If
        End If
    End Sub

    Public Sub FillConfigurationTableAdapter()
        ConfigurationTableAdapter.Fill(ChannelsDigitalOutputDataSet.КонфигурацияДействий)
    End Sub

    Public Sub FillActionTableAdapter(ByVal keyConfigurationAction As Integer)
        BindingSourceAction.DataSource = ChannelsDigitalOutputDataSet
        BindingSourceAction.DataMember = ChannelsDigitalOutputDataSet.Действие.ToString

        If keyConfigurationAction > 0 Then
            ActionTableAdapter.FillBykeyКонфигурацияДействия(ChannelsDigitalOutputDataSet.Действие, keyConfigurationAction)
        End If
    End Sub

    Public Sub FillTriggerFireDigitalOutputTableAdapter(ByVal keyAction As Integer)
        BindingSourceTrigger.DataSource = ChannelsDigitalOutputDataSet
        BindingSourceTrigger.DataMember = ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.ToString

        If keyAction > 0 Then
            TriggerFireDigitalOutputTableAdapter.FillBykeyДествие(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода, keyAction)
        End If
    End Sub

    Public Sub FillFormulaFireDigitalOutputTableAdapter(ByVal keyAction As Integer)
        BindingSourceFormula.DataSource = ChannelsDigitalOutputDataSet
        BindingSourceFormula.DataMember = ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.ToString

        If keyAction > 0 Then
            FormulaFireDigitalOutputTableAdapter.FillBykeyДействие(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода, keyAction)
        End If
    End Sub

    Public Sub FillPortsTableAdapter(ByVal keyAction As Integer)
        BindingSourcePort.DataSource = ChannelsDigitalOutputDataSet
        BindingSourcePort.DataMember = ChannelsDigitalOutputDataSet.Порты.ToString

        If keyAction > 0 Then
            PortsTableAdapter.FillBykeyДействие(ChannelsDigitalOutputDataSet.Порты, keyAction)
        End If
    End Sub

    Public Sub FillArgumentsOfFormulaTableAdapter(ByVal keyFormula As Integer)
        BindingSourceАргумент.DataSource = ChannelsDigitalOutputDataSet
        BindingSourceАргумент.DataMember = ChannelsDigitalOutputDataSet.АргументыДляФормулы.ToString

        If keyFormula > 0 Then
            ArgumentsOfFormulaTableAdapter.FillByKeyFormula(ChannelsDigitalOutputDataSet.АргументыДляФормулы, keyFormula)
        End If
    End Sub

    Public Sub FillBitsOfPortTableAdapter(ByVal KeyPort As Integer)
        BindingSourceBit.DataSource = ChannelsDigitalOutputDataSet
        BindingSourceBit.DataMember = ChannelsDigitalOutputDataSet.БитПорта.ToString

        If KeyPort > 0 Then
            BitsOfPortTableAdapter.FillByKeyПорта(ChannelsDigitalOutputDataSet.БитПорта, KeyPort)
        End If
    End Sub

    Public Sub UpdateConfigurationTableAdapter()
        Validate()
        BindingSourceConfiguration.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.КонфигурацияДействий.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.КонфигурацияДействий.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.КонфигурацияДействий.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then ConfigurationTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then ConfigurationTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then ConfigurationTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateConfigurationTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try
        Application.DoEvents()
    End Sub

    Public Sub UpdateActionTableAdapter()
        Validate()
        BindingSourceAction.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.ДействиеDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.ДействиеDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.ДействиеDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then ActionTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then ActionTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then ActionTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateActionTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Public Sub UpdateTriggerFireDigitalOutputTableAdapter()
        Validate()
        BindingSourceTrigger.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыходаDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then TriggerFireDigitalOutputTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then TriggerFireDigitalOutputTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then TriggerFireDigitalOutputTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

            'Me.ТриггерСрабатыванияЦифровогоВыходаTableAdapter.Update(Me.ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateTriggerFireDigitalOutputTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Public Sub UpdateFormulaFireDigitalOutputTableAdapter()
        Validate()
        BindingSourceFormula.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыходаDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then FormulaFireDigitalOutputTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then FormulaFireDigitalOutputTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then FormulaFireDigitalOutputTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateFormulaFireDigitalOutputTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Public Sub UpdatePortsTableAdapter()
        Validate()
        BindingSourcePort.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.Порты.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.ПортыDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.Порты.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.ПортыDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.Порты.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.ПортыDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then PortsTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then PortsTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then PortsTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdatePortsTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Public Sub UpdateArgumentsOfFormulaTableAdapter()
        Validate()
        BindingSourceАргумент.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.АргументыДляФормулы.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.АргументыДляФормулыDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.АргументыДляФормулы.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.АргументыДляФормулыDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.АргументыДляФормулы.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.АргументыДляФормулыDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then ArgumentsOfFormulaTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then ArgumentsOfFormulaTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then ArgumentsOfFormulaTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateArgumentsOfFormulaTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Public Sub UpdateBitsOfPortTableAdapter()
        Validate()
        BindingSourceBit.EndEdit()

        Dim deletedChildRecords = CType(ChannelsDigitalOutputDataSet.БитПорта.GetChanges(DataRowState.Deleted), ChannelsDigitalOutputDataSet.БитПортаDataTable)
        Dim newChildRecords = CType(ChannelsDigitalOutputDataSet.БитПорта.GetChanges(DataRowState.Added), ChannelsDigitalOutputDataSet.БитПортаDataTable)
        Dim modifiedChildRecords = CType(ChannelsDigitalOutputDataSet.БитПорта.GetChanges(DataRowState.Modified), ChannelsDigitalOutputDataSet.БитПортаDataTable)

        Try
            If Not deletedChildRecords Is Nothing Then BitsOfPortTableAdapter.Update(deletedChildRecords)
            If Not modifiedChildRecords Is Nothing Then BitsOfPortTableAdapter.Update(modifiedChildRecords)
            If Not newChildRecords Is Nothing Then BitsOfPortTableAdapter.Update(newChildRecords)

            ChannelsDigitalOutputDataSet.AcceptChanges()

        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(UpdateBitsOfPortTableAdapter)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If Not deletedChildRecords Is Nothing Then deletedChildRecords.Dispose()
            If Not modifiedChildRecords Is Nothing Then modifiedChildRecords.Dispose()
            If Not newChildRecords Is Nothing Then newChildRecords.Dispose()
        End Try

        Application.DoEvents()
    End Sub

    Private Sub GridViewConfiguration_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridViewConfiguration.SelectionChanged
        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        BindingSourceAction.DataSource = Nothing
        UpdateAction()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(GridViewConfiguration_SelectionChanged)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub DataGridViewAction_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridViewAction.SelectionChanged
        If isUpdatingDataGridViewConfigurationFromParent Then Exit Sub

        ChannelsDigitalOutputDataSet.EnforceConstraints = False

        BindingSourceBit.DataSource = Nothing
        BindingSourceАргумент.DataSource = Nothing
        BindingSourceTrigger.DataSource = Nothing
        BindingSourceFormula.DataSource = Nothing
        BindingSourcePort.DataSource = Nothing

        UpdateTrigger()
        UpdateFormula()
        UpdatePort()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(DataGridViewAction_SelectionChanged)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub DataGridViewFormula_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridViewFormula.SelectionChanged
        If isUpdatingDataGridViewFormulaFromParent Then Exit Sub

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        BindingSourceАргумент.DataSource = Nothing
        UpdateArgument()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(DataGridViewFormula_SelectionChanged)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub DataGridViewPort_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridViewPort.SelectionChanged
        If isUpdatingDataGridViewPortFromParent Then Exit Sub

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        BindingSourceBit.DataSource = Nothing
        UpdateBit()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(DataGridViewPort_SelectionChanged)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub TSButtonConfigurationSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonConfigurationSave.Click
        SaveModificationConfiguration()
    End Sub
    Private Sub TSButtonConfigurationDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonConfigurationDelete.Click
        TypeNode = TypeGridDigitalNode.Configuration
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonActionSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonActionSave.Click
        SaveModificationAction()
        BindingSourceAction.MoveLast()
    End Sub
    Private Sub TSButtonActionDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonActionDelete.Click
        TypeNode = TypeGridDigitalNode.Action
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonTriggerSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonTriggerSave.Click
        SaveModificationTrigger()
        BindingSourceTrigger.MoveLast()
    End Sub
    Private Sub TSButtonTriggerDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonTriggerDelete.Click
        TypeNode = TypeGridDigitalNode.Trigger
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonFormulaSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonFormulaSave.Click
        SaveModificationFormula()
        BindingSourceFormula.MoveLast()
    End Sub
    Private Sub TSButtonFormulaDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonFormulaDelete.Click
        TypeNode = TypeGridDigitalNode.Formula
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonArgumentSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonArgumentSave.Click
        SaveModificationArgument()
        BindingSourceАргумент.MoveLast()
    End Sub
    Private Sub TSButtonArgumenDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonArgumenDelete.Click
        TypeNode = TypeGridDigitalNode.Argument
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonSave.Click
        SaveModificationPort()
        BindingSourcePort.MoveLast()
    End Sub
    Private Sub TSButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonDelete.Click
        TypeNode = TypeGridDigitalNode.Port
        TimerUpdate.Enabled = True
    End Sub

    Private Sub TSButtonBitSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonBitSave.Click
        SaveModificationBit()
        BindingSourceBit.MoveLast()
    End Sub
    Private Sub TSButtonBitDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonBitDelete.Click
        TypeNode = TypeGridDigitalNode.Bit
        TimerUpdate.Enabled = True
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Конфигурация
    ''' </summary>
    Public Sub SaveModificationConfiguration()
        Try
            Validate()
            BindingSourceConfiguration.EndEdit()
            ConfigurationTableAdapter.Update(ChannelsDigitalOutputDataSet.КонфигурацияДействий)
            Application.DoEvents()
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationConfiguration)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        If BindingSourceConfiguration.Current Is Nothing Then
            BindingSourceBit.DataSource = Nothing
            BindingSourceАргумент.DataSource = Nothing
            BindingSourcePort.DataSource = Nothing
            BindingSourceFormula.DataSource = Nothing
            BindingSourceTrigger.DataSource = Nothing
            BindingSourceAction.DataSource = Nothing
            Exit Sub
        End If
        'Me.ChannelsDigitalOutputDataSet.КонфигурацияДействий.AcceptChanges()

        ConfigurationTableAdapter.Fill(ChannelsDigitalOutputDataSet.КонфигурацияДействий)
        BindingSourceConfiguration.MoveLast()
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Действие
    ''' </summary>
    Private Sub SaveModificationAction()
        Try
            Validate()
            BindingSourceAction.EndEdit()

            ''This example shows how to send updates from a dataset that contains two related data tables.

            'Dim deletedChildRecords As DataTable = _
            '    ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Deleted)

            'Dim newChildRecords As DataTable = _
            '    ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Added)

            'Dim modifiedChildRecords As DataTable = _
            '    ChannelsDigitalOutputDataSet.Действие.GetChanges(DataRowState.Modified)

            'Try
            '    If Not deletedChildRecords Is Nothing Then
            '        ДействиеTableAdapter.Update(deletedChildRecords)
            '    End If

            '    'daCustomers.Update(dsNorthwind1, "Customers")
            '    Me.ТриггерСрабатыванияЦифровогоВыходаTableAdapter.Update(Me.ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода)
            '    Me.ФормулаСрабатыванияЦифровогоВыходаTableAdapter.Update(Me.ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода)
            '    Me.АргументыДляФормулыTableAdapter.Update(Me.ChannelsDigitalOutputDataSet.АргументыДляФормулы)

            '    If Not newChildRecords Is Nothing Then
            '        ДействиеTableAdapter.Update(newChildRecords)
            '    End If

            '    If Not modifiedChildRecords Is Nothing Then
            '        ДействиеTableAdapter.Update(modifiedChildRecords)
            '    End If

            '    ChannelsDigitalOutputDataSet.AcceptChanges()

            'Catch ex As Exception
            '    ' Update error, resolve and try again.
            '    MessageBox.Show("Ошибка в процедуре СохранитьИзмененияДействие")

            'Finally
            '    If Not deletedChildRecords Is Nothing Then
            '        deletedChildRecords.Dispose()
            '    End If

            '    If Not newChildRecords Is Nothing Then
            '        newChildRecords.Dispose()
            '    End If

            '    If Not modifiedChildRecords Is Nothing Then
            '        modifiedChildRecords.Dispose()
            '    End If

            'End Try

            ActionTableAdapter.Update(ChannelsDigitalOutputDataSet.Действие)
            Application.DoEvents()
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(SaveModificationAction)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdateAction()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationAction)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Триггер
    ''' </summary>
    Private Sub SaveModificationTrigger()
        Try
            Validate()
            BindingSourceTrigger.EndEdit()
            TriggerFireDigitalOutputTableAdapter.Update(ChannelsDigitalOutputDataSet.ТриггерСрабатыванияЦифровогоВыхода)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationTrigger)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
        Application.DoEvents()

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdateTrigger()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationTrigger)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Формула
    ''' </summary>
    Private Sub SaveModificationFormula()
        Try
            Validate()
            BindingSourceFormula.EndEdit()
            FormulaFireDigitalOutputTableAdapter.Update(ChannelsDigitalOutputDataSet.ФормулаСрабатыванияЦифровогоВыхода)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationFormula)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
        Application.DoEvents()

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdateFormula()
        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationFormula)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Аргумент
    ''' </summary>
    Private Sub SaveModificationArgument()
        Try
            Validate()
            BindingSourceАргумент.EndEdit()
            ArgumentsOfFormulaTableAdapter.Update(ChannelsDigitalOutputDataSet.АргументыДляФормулы)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationArgument)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
        Application.DoEvents()

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdateArgument()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationArgument)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Порт
    ''' </summary>
    Private Sub SaveModificationPort()
        Try
            Validate()
            BindingSourcePort.EndEdit()
            PortsTableAdapter.Update(ChannelsDigitalOutputDataSet.Порты)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationPort)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
        Application.DoEvents()

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdatePort()
        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationPort)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сохранить Изменения Бит
    ''' </summary>
    Private Sub SaveModificationBit()
        Try
            Validate()
            BindingSourceBit.EndEdit()
            BitsOfPortTableAdapter.Update(ChannelsDigitalOutputDataSet.БитПорта)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления в процедуре <{NameOf(SaveModificationBit)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
        Application.DoEvents()

        ChannelsDigitalOutputDataSet.EnforceConstraints = False
        UpdateBit()

        Try
            ChannelsDigitalOutputDataSet.EnforceConstraints = True
        Catch ex As ConstraintException
            Dim caption As String = $"Процедура <{NameOf(SaveModificationBit)}>"
            Dim text As String = ex.GetType().ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

#Region "Пробы работы со строками таблицам DataSet"
    'Проверка корректности введенных данных
    'Private Sub sickLeaveHoursTextBox_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sickLeaveHoursTextBox.Validating
    '    Me.HandleValidation(sender, e, Me.sickLeaveHoursTextBox.Text.Trim(), True)
    'End Sub

    'Private Sub HandleValidation(ByVal sender As Object, ByVal e As CancelEventArgs, ByVal text As String, ByVal requiresNumeric As Boolean)
    '    Dim err As String = Nothing
    '    Dim numericFailed As Boolean = False
    '    If requiresNumeric Then
    '        Dim output As Integer
    '        Dim isNumeric As Boolean = Integer.TryParse(text, output)
    '        numericFailed = Not isNumeric
    '    End If
    '    If ((text.Length = 0) OrElse numericFailed) Then
    '        err = IIf(requiresNumeric, "Required Numeric Field", "Required Field")
    '        ErrorProvider1.SetError(CType(sender, Control), err)
    '        e.Cancel = True
    '    Else
    '        ErrorProvider1.Clear()
    '    End If
    'End Sub

    'Dim NewkeyКонфигурацияДействия As Integer

    'Private Sub Добавить_в_таблицу_строку()
    '    'проба с программной модификацией
    '    'BindingSourceКонфигурация.DataSource = Nothing
    '    'BindingSourceКонфигурация.DataSource = ChannelsDigitalOutputDataSet
    '    'BindingSourceКонфигурация.DataMember = ChannelsDigitalOutputDataSet.КонфигурацияДействий.ToString

    '    'добавить в таблицу строку
    '    Dim tblКонфигурацияДействий As ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable = ChannelsDigitalOutputDataSet.КонфигурацияДействий
    '    Dim rowКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow
    '    rowКонфигурация = tblКонфигурацияДействий.NewКонфигурацияДействийRow 'новая строка
    '    rowКонфигурация.ИмяКонфигурации = "программа"
    '    rowКонфигурация.Описание = "программа"
    '    tblКонфигурацияДействий.AddКонфигурацияДействийRow(rowКонфигурация)

    '    'или
    '    'tblКонфигурацияДействий.AddКонфигурацияДействийRow("программа2", "программа2")

    '    'tblКонфигурацияДействий.AcceptChanges()
    '    'Me.КонфигурацияTableAdapter.Update(tblКонфигурацияДействий)
    '    'Me.КонфигурацияTableAdapter.Update(Me.ChannelsDigitalOutputDataSet.КонфигурацияДействий)
    '     СохранитьИзмененияКонфигурация()
    '    'получить key
    '    rowКонфигурация = tblКонфигурацияДействий.Rows(tblКонфигурацияДействий.Rows.Count - 1)
    '    NewkeyКонфигурацияДействия = rowКонфигурация.keyКонфигурацияДействия
    '    'или
    '    NewkeyКонфигурацияДействия = tblКонфигурацияДействий.Last.keyКонфигурацияДействия
    'End Sub

    'Dim keyДействиеDelete As Integer '= rowДействие.keyДействие

    'Private Sub Поиск_записи_добавить_дочернюю_запись()
    '    'поиск записи
    '    Dim rowFindКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow
    '    'rowFindКонфигурация = tblКонфигурацияДействий.FindBykeyКонфигурацияДействия(keyКонфигурацияДействия:=NewkeyКонфигурацияДействия)

    '    rowFindКонфигурация = ChannelsDigitalOutputDataSet.КонфигурацияДействий.FindBykeyКонфигурацияДействия(keyКонфигурацияДействия:=NewkeyКонфигурацияДействия)
    '    If rowFindКонфигурация Is Nothing Then
    '    Else
    '        'проверка со значением NULL
    '        If rowFindКонфигурация.IsОписаниеNull Then
    '            'что-то
    '        End If
    '    End If

    '    'добавить дочернюю запись
    '    Dim tblДействие As ChannelsDigitalOutputDataSet.ДействиеDataTable = ChannelsDigitalOutputDataSet.Действие
    '    '1 вариант
    '    tblДействие.AddДействиеRow(rowFindКонфигурация, "Новое Действие1")
    '    '2 вариант
    '    Dim rowДействие As ChannelsDigitalOutputDataSet.ДействиеRow
    '    rowДействие = tblДействие.NewДействиеRow 'новая строка
    '    rowДействие.keyКонфигурацияДействия = NewkeyКонфигурацияДействия
    '    rowДействие.ИмяДействия = "Новое Действие2"
    '    tblДействие.AddДействиеRow(rowДействие)

    '    'ТипУровня = EnumУровень.Действие
    '    '
    '    'Timer1.Enabled = True

    '    'необходимо активизировать последнюю строку в DataGridViewКонфигурация
    '     СохранитьИзмененияДействие()

    '    Dim tblКонфигурацияДействий As ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable = ChannelsDigitalOutputDataSet.КонфигурацияДействий
    '    DataGridViewКонфигурация.CurrentCell = DataGridViewКонфигурация.Rows(tblКонфигурацияДействий.Rows.Count - 1).Cells(0)
    '     DataGridViewКонфигурация_SelectionChanged(DataGridViewКонфигурация, New System.EventArgs)
    'End Sub

    'Private Sub Найти_родительскую_запись()
    '    Dim tblКонфигурацияДействий As ChannelsDigitalOutputDataSet.КонфигурацияДействийDataTable = ChannelsDigitalOutputDataSet.КонфигурацияДействий
    '    Dim rowFindКонфигурация As ChannelsDigitalOutputDataSet.КонфигурацияДействийRow

    '    'найти родительскую запись
    '    rowFindКонфигурация = tblКонфигурацияДействий.FindBykeyКонфигурацияДействия(keyКонфигурацияДействия:=NewkeyКонфигурацияДействия)
    '    'или
    '    'rowКонфигурация = tblКонфигурацияДействий.Rows(tblКонфигурацияДействий.Rows.Count - 1)

    '    'получить иерархические данные с верхнего уровня
    '    'rowFindКонфигурация.GetДействиеRows.Last
    '    For Each rowДействие As ChannelsDigitalOutputDataSet.ДействиеRow In rowFindКонфигурация.GetДействиеRows
    '        Console.WriteLine(rowДействие.ИмяДействия)
    '    Next

    '    '1 вариант
    '    'удалить запись с определенным индексом
    '    Dim rowДействиеDelete As ChannelsDigitalOutputDataSet.ДействиеRow = ChannelsDigitalOutputDataSet.Действие.Rows(ChannelsDigitalOutputDataSet.Действие.Rows.Count - 1)
    '    Dim keyДействиеDelete As Integer = rowДействиеDelete.keyДействие

    '    Dim intIndex As Integer
    '    For Each ItemДействие As ChannelsDigitalOutputDataSet.ДействиеRow In ChannelsDigitalOutputDataSet.Действие.Rows
    '        'If ItemДействие Is rowДействиеDelete Then
    '        'или
    '        If ItemДействие.keyДействие = keyДействиеDelete Then
    '            Exit For
    '        End If
    '        intIndex += 1
    '    Next
    '    'работает
    '    'Me.BindingSourceДействие.Position = intIndex
    '    'BindingSourceДействие.RemoveCurrent()
    '    'или
    '    'Me.BindingSourceДействие.RemoveAt(intIndex)

    '    'этот удаляет
    '    'ChannelsDigitalOutputDataSet.Действие.Rows(intIndex).Delete()
    '    '2 ещё вариант работает
    '    If ChannelsDigitalOutputDataSet.Действие.Rows.Count > 0 AndAlso keyДействиеDelete <> 0 Then
    '        ChannelsDigitalOutputDataSet.Действие.FindBykeyДействие(keyДействиеDelete).Delete()
    '    End If

    '    'эти 2 метода не удаляют
    '    'ChannelsDigitalOutputDataSet.Действие.RemoveДействиеRow(rowДействиеDelete)
    '    'ChannelsDigitalOutputDataSet.Действие.Rows.Remove(rowДействиеDelete)
    '    'не работает только чтение
    '    'Me.BindingSourceДействие.Current = rowДействиеDelete 

    '    ТипУровня = fEnumУровень.Действие
    '    TimerUpdate.Enabled = True
    'End Sub
#End Region

#End Region

#Region "Поиск узлов"
    ''' <summary>
    ''' Найти Последний Узел
    ''' </summary>
    ''' <param name="inTypeGridDigitalNode"></param>
    Public Sub FindLastNode(ByVal inTypeGridDigitalNode As TypeGridDigitalNode)
        TypeNode = inTypeGridDigitalNode
        SetParentForNode()
        SetMemoKeyLastNode(inTypeGridDigitalNode)

        If DtvwDirectory.Nodes.Count > 0 Then
            DtvwDirectory.Nodes(0).Expand()
        End If

        Select Case inTypeGridDigitalNode
            Case TypeGridDigitalNode.Unknown, TypeGridDigitalNode.AllConfigurations, TypeGridDigitalNode.Configuration
                ExpandConfigurationNode()
                Exit Select
            Case TypeGridDigitalNode.AllActions, TypeGridDigitalNode.Action
                ExpandActionNode()
                Exit Select
            Case TypeGridDigitalNode.AllTriggers, TypeGridDigitalNode.Trigger
                ExpandTriggerNode()
                Exit Select
            Case TypeGridDigitalNode.AllFormulas, TypeGridDigitalNode.Formula
                ExpandFormulaNode()
                Exit Select
            Case TypeGridDigitalNode.AllArguments, TypeGridDigitalNode.Argument
                ExpandArgumentNode()
                Exit Select
            Case TypeGridDigitalNode.AllPorts, TypeGridDigitalNode.Port
                ExpandPortNode()
                Exit Select
            Case TypeGridDigitalNode.AllEnableBits, TypeGridDigitalNode.Bit
                ExpandBitNode()
                Exit Select
        End Select
    End Sub

#Region "ExpandNode"
    Private Sub ExpandConfigurationNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            DtvwDirectory.CurrentTreeNode = configTreeNodeFound
            configTreeNodeFound.Expand()
            DtvwDirectory.SelectCureentNode()
        End If

        PopulateComboBoxAndListBoxConfigurations()
    End Sub

    Private Sub ExpandActionNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                DtvwDirectory.CurrentTreeNode = actionTreeNodeFound
                actionTreeNodeFound.Expand()
                DtvwDirectory.SelectCureentNode()
            End If
        End If
    End Sub

    Private Sub ExpandTriggerNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                actionTreeNodeFound.Expand()
                Dim triggerTreeNodeFound As TreeNode = FindTriggerNode(actionTreeNodeFound)

                If triggerTreeNodeFound IsNot Nothing Then
                    DtvwDirectory.CurrentTreeNode = triggerTreeNodeFound
                    triggerTreeNodeFound.Expand()
                    DtvwDirectory.SelectCureentNode()
                End If
            End If
        End If
    End Sub

    Private Sub ExpandFormulaNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                actionTreeNodeFound.Expand()
                Dim formulaTreeNodeFound As TreeNode = FindFormulaNode(actionTreeNodeFound)

                If formulaTreeNodeFound IsNot Nothing Then
                    DtvwDirectory.CurrentTreeNode = formulaTreeNodeFound
                    formulaTreeNodeFound.Expand()
                    DtvwDirectory.SelectCureentNode()
                End If
            End If
        End If
    End Sub

    Private Sub ExpandArgumentNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                actionTreeNodeFound.Expand()
                Dim formulaTreeNodeFound As TreeNode = FindFormulaNode(actionTreeNodeFound)

                If formulaTreeNodeFound IsNot Nothing Then
                    formulaTreeNodeFound.Expand()
                    Dim argumentTreeNodeFound As TreeNode = FindArgumentNode(formulaTreeNodeFound)

                    If argumentTreeNodeFound IsNot Nothing Then
                        DtvwDirectory.CurrentTreeNode = argumentTreeNodeFound
                        argumentTreeNodeFound.Expand()
                        DtvwDirectory.SelectCureentNode()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ExpandPortNode()
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                actionTreeNodeFound.Expand()
                Dim portTreeNodeFound As TreeNode = FindPortNode(actionTreeNodeFound)

                If portTreeNodeFound IsNot Nothing Then
                    DtvwDirectory.CurrentTreeNode = portTreeNodeFound
                    portTreeNodeFound.Expand()
                    DtvwDirectory.SelectCureentNode()
                End If
            End If
        End If
    End Sub

    Private Sub ExpandBitNode()
        'ищем gkeyБитПорта
        Dim configTreeNodeFound As TreeNode = FindConfigurationNode()

        If configTreeNodeFound IsNot Nothing Then
            configTreeNodeFound.Expand()
            Dim actionTreeNodeFound As TreeNode = FindActionNode(configTreeNodeFound)

            If actionTreeNodeFound IsNot Nothing Then
                actionTreeNodeFound.Expand()
                Dim portTreeNodeFound As TreeNode = FindPortNode(actionTreeNodeFound)

                If portTreeNodeFound IsNot Nothing Then
                    portTreeNodeFound.Expand()
                    Dim bitTreeNodeFound As TreeNode = FindBitNode(portTreeNodeFound)

                    If bitTreeNodeFound IsNot Nothing Then
                        DtvwDirectory.CurrentTreeNode = bitTreeNodeFound
                        bitTreeNodeFound.Expand()
                        DtvwDirectory.SelectCureentNode()
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Найти Конфигурация
    ''' </summary>
    ''' <returns></returns>
    Private Function FindConfigurationNode() As TreeNode
        Dim configTreeNodeFound As TreeNode = Nothing
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            If MemoKeyConfigurationAction = CType(itemConfigTreeNode.Tag, Конфигурация).KeyКонфигурацияДействия Then
                configTreeNodeFound = itemConfigTreeNode
                Exit For
            End If
        Next
        Return configTreeNodeFound
    End Function

    ''' <summary>
    ''' НайтиДействие
    ''' </summary>
    ''' <param name="configurationTreeNode"></param>
    ''' <returns></returns>
    Private Function FindActionNode(ByVal configurationTreeNode As TreeNode) As TreeNode
        Dim actionTreeNodeFound As TreeNode = Nothing
        For Each itemActionTreeNode As TreeNode In configurationTreeNode.Nodes
            If MemoKeyAction = CType(itemActionTreeNode.Tag, Действие).keyДействие Then
                actionTreeNodeFound = itemActionTreeNode
                Exit For
            End If
        Next
        Return actionTreeNodeFound
    End Function

    ''' <summary>
    ''' НайтиТриггер
    ''' </summary>
    ''' <param name="actionTreeNode"></param>
    ''' <returns></returns>
    Private Function FindTriggerNode(ByVal actionTreeNode As TreeNode) As TreeNode
        Dim triggerTreeNodeFound As TreeNode = Nothing
        For Each itemTriggerTreeNode As TreeNode In actionTreeNode.Nodes
            If CType(itemTriggerTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Trigger Then
                If MemoKeyTrigger = CType(itemTriggerTreeNode.Tag, ТриггерСрабатыванияЦифровогоВыхода).KeyТриггер Then
                    triggerTreeNodeFound = itemTriggerTreeNode
                    Exit For
                End If
            End If
        Next
        Return triggerTreeNodeFound
    End Function

    ''' <summary>
    ''' Найти Формулу
    ''' </summary>
    ''' <param name="actionTreeNode"></param>
    ''' <returns></returns>
    Private Function FindFormulaNode(ByVal actionTreeNode As TreeNode) As TreeNode
        Dim formulaTreeNodeFound As TreeNode = Nothing
        For Each itemFormulaTreeNode As TreeNode In actionTreeNode.Nodes
            If CType(itemFormulaTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Formula Then
                If MemoKeyFormula = CType(itemFormulaTreeNode.Tag, ФормулаСрабатыванияЦифровогоВыхода).KeyFormula Then
                    formulaTreeNodeFound = itemFormulaTreeNode
                    Exit For
                End If
            End If
        Next
        Return formulaTreeNodeFound
    End Function

    ''' <summary>
    ''' Найти Аргумент
    ''' </summary>
    ''' <param name="formulaTreeNode"></param>
    ''' <returns></returns>
    Private Function FindArgumentNode(ByVal formulaTreeNode As TreeNode) As TreeNode
        Dim argumentTreeNodeFound As TreeNode = Nothing
        For Each itemArgumentTreeNode As TreeNode In formulaTreeNode.Nodes
            If MemoКеуArgument = CType(itemArgumentTreeNode.Tag, АргументДляФормулы).КеуArgument Then
                argumentTreeNodeFound = itemArgumentTreeNode
                Exit For
            End If
        Next
        Return argumentTreeNodeFound
    End Function

    ''' <summary>
    ''' Найти Порт
    ''' </summary>
    ''' <param name="actionTreeNode"></param>
    ''' <returns></returns>
    Private Function FindPortNode(ByVal actionTreeNode As TreeNode) As TreeNode
        Dim portTreeNodeFound As TreeNode = Nothing
        For Each itemPortTreeNode As TreeNode In actionTreeNode.Nodes
            If CType(itemPortTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Port Then
                If MemoKeyPort = CType(itemPortTreeNode.Tag, Порт).KeyПорта Then
                    portTreeNodeFound = itemPortTreeNode
                    Exit For
                End If
            End If
        Next
        Return portTreeNodeFound
    End Function

    ''' <summary>
    ''' Найти Бит
    ''' </summary>
    ''' <param name="portTreeNode"></param>
    ''' <returns></returns>
    Private Function FindBitNode(ByVal portTreeNode As TreeNode) As TreeNode
        Dim bitTreeNodeFound As TreeNode = Nothing
        For Each itemBitTreeNode As TreeNode In portTreeNode.Nodes
            If MemoKeyBit = CType(itemBitTreeNode.Tag, Бит).keyБитПорта Then
                bitTreeNodeFound = itemBitTreeNode
                Exit For
            End If
        Next
        Return bitTreeNodeFound
    End Function
#End Region

#Region "Найти Ключи Последнего Узла"
    ''' <summary>
    ''' Найти Ключи Последнего Узла
    ''' </summary>
    ''' <param name="inTypeGridDigitalNode"></param>
    Public Sub SetMemoKeyLastNode(ByVal inTypeGridDigitalNode As TypeGridDigitalNode)
        Select Case inTypeGridDigitalNode
            Case TypeGridDigitalNode.Unknown, TypeGridDigitalNode.AllConfigurations, TypeGridDigitalNode.Configuration
                Exit Select
            Case TypeGridDigitalNode.AllActions, TypeGridDigitalNode.Action
                FindKeyAction()
                Exit Select
            Case TypeGridDigitalNode.AllTriggers, TypeGridDigitalNode.Trigger
                FindKeyTrigger()
                Exit Select
            Case TypeGridDigitalNode.AllFormulas, TypeGridDigitalNode.Formula
                FindKeyFormula()
                Exit Select
            Case TypeGridDigitalNode.AllArguments, TypeGridDigitalNode.Argument
                FindКеуArgument()
                Exit Select
            Case TypeGridDigitalNode.AllPorts, TypeGridDigitalNode.Port
                FindKeyPort()
                Exit Select
            Case TypeGridDigitalNode.AllEnableBits, TypeGridDigitalNode.Bit
                FindKeyBit()
                Exit Select
        End Select
    End Sub

    Private Sub FindKeyAction()
        For Each configTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each actionTreeNode As TreeNode In configTreeNode.Nodes
                If MemoKeyAction = CType(actionTreeNode.Tag, Действие).keyДействие Then
                    MemoKeyConfigurationAction = CType(actionTreeNode.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                    Exit Sub
                End If
            Next
        Next
    End Sub

    Private Sub FindKeyTrigger()
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each itemActionTreeNode As TreeNode In itemConfigTreeNode.Nodes
                For Each itemTriggerTreeNode As TreeNode In itemActionTreeNode.Nodes
                    If CType(itemTriggerTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Trigger Then
                        If MemoKeyTrigger = CType(itemTriggerTreeNode.Tag, ТриггерСрабатыванияЦифровогоВыхода).KeyТриггер Then
                            MemoKeyAction = CType(itemTriggerTreeNode.Parent.Tag, Действие).keyДействие
                            MemoKeyConfigurationAction = CType(itemTriggerTreeNode.Parent.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                            Exit Sub
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub FindKeyFormula()
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each itemActionTreeNode As TreeNode In itemConfigTreeNode.Nodes
                For Each itemFormulaTreeNode As TreeNode In itemActionTreeNode.Nodes
                    If CType(itemFormulaTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Formula Then
                        If MemoKeyFormula = CType(itemFormulaTreeNode.Tag, ФормулаСрабатыванияЦифровогоВыхода).KeyFormula Then
                            MemoKeyAction = CType(itemFormulaTreeNode.Parent.Tag, Действие).keyДействие
                            MemoKeyConfigurationAction = CType(itemFormulaTreeNode.Parent.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                            Exit Sub
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub FindКеуArgument()
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each itemActionTreeNode As TreeNode In itemConfigTreeNode.Nodes
                For Each itemFormulaTreeNode As TreeNode In itemActionTreeNode.Nodes
                    If CType(itemFormulaTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Formula Then
                        For Each itemArgumentTreeNode As TreeNode In itemFormulaTreeNode.Nodes
                            If MemoКеуArgument = CType(itemArgumentTreeNode.Tag, АргументДляФормулы).КеуArgument Then
                                MemoKeyFormula = CType(itemArgumentTreeNode.Parent.Tag, ФормулаСрабатыванияЦифровогоВыхода).KeyFormula
                                MemoKeyAction = CType(itemArgumentTreeNode.Parent.Parent.Tag, Действие).keyДействие
                                MemoKeyConfigurationAction = CType(itemArgumentTreeNode.Parent.Parent.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                                Exit Sub
                            End If
                        Next
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub FindKeyPort()
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each itemActionTreeNode As TreeNode In itemConfigTreeNode.Nodes
                For Each itemPortTreeNode As TreeNode In itemActionTreeNode.Nodes
                    If CType(itemPortTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Port Then
                        If MemoKeyPort = CType(itemPortTreeNode.Tag, Порт).KeyПорта Then
                            MemoKeyAction = CType(itemPortTreeNode.Parent.Tag, Действие).keyДействие
                            MemoKeyConfigurationAction = CType(itemPortTreeNode.Parent.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                            Exit Sub
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub FindKeyBit()
        For Each itemConfigTreeNode As TreeNode In DtvwDirectory.Nodes(0).Nodes
            For Each itemActionTreeNode As TreeNode In itemConfigTreeNode.Nodes
                For Each itemPortTreeNode As TreeNode In itemActionTreeNode.Nodes
                    If CType(itemPortTreeNode.Tag, TreeNodeBase).Тип = TypeGridDigitalNode.Port Then
                        For Each itemBitTreeNode As TreeNode In itemPortTreeNode.Nodes
                            If MemoKeyBit = CType(itemBitTreeNode.Tag, Бит).keyБитПорта Then
                                MemoKeyPort = CType(itemBitTreeNode.Parent.Tag, Порт).KeyПорта
                                MemoKeyAction = CType(itemBitTreeNode.Parent.Parent.Tag, Действие).keyДействие
                                MemoKeyConfigurationAction = CType(itemBitTreeNode.Parent.Parent.Parent.Tag, Конфигурация).KeyКонфигурацияДействия
                                Exit Sub
                            End If
                        Next
                    End If
                Next
            Next
        Next
    End Sub
#End Region

#Region "Прверить На Присутствие Родителя"
    ''' <summary>
    ''' Прверить На Присутствие Родителя
    ''' </summary>
    Private Sub SetParentForNode()
        For Each itemConfig As Конфигурация In GlobalAllConfigurations.ВсеКонфигурации
            SetParentForAction(itemConfig)
        Next
    End Sub

    Private Sub SetParentForAction(itemConfig As Конфигурация)
        For Each itemAction As Действие In itemConfig.Действия
            If itemAction.Parent Is Nothing Then
                itemAction.Parent = itemConfig
                itemAction.keyКонфигурацияДействия = itemConfig.KeyКонфигурацияДействия
            End If

            SetParentForTrigger(itemAction)
            SetParentForFormula(itemAction)
            SetParentForPort(itemAction)
        Next
    End Sub

    Private Sub SetParentForTrigger(itemAction As Действие)
        For Each itemTrigger As ТриггерСрабатыванияЦифровогоВыхода In itemAction.ТриггерыСрабатыванияЦифровогоВыхода
            If itemTrigger.Parent Is Nothing Then
                itemTrigger.Parent = itemAction
                itemTrigger.keyДействие = itemAction.keyДействие
            End If
        Next
    End Sub

    Private Sub SetParentForFormula(itemAction As Действие)
        For Each itemFormula As ФормулаСрабатыванияЦифровогоВыхода In itemAction.ФормулыСрабатыванияЦифровогоВыхода
            If itemFormula.Parent Is Nothing Then
                itemFormula.Parent = itemAction
                itemFormula.keyДействие = itemAction.keyДействие
            End If

            SetParentForArgument(itemFormula)
        Next
    End Sub

    Private Sub SetParentForArgument(itemFormula As ФормулаСрабатыванияЦифровогоВыхода)
        For Each itemArgument As АргументДляФормулы In itemFormula.АргументыДляФормулы
            If itemArgument.Parent Is Nothing Then
                itemArgument.Parent = itemFormula
                itemArgument.KeyFormula = itemFormula.KeyFormula
            End If
        Next
    End Sub

    Private Sub SetParentForPort(itemAction As Действие)
        For Each itemPort As Порт In itemAction.Порты
            If itemPort.Parent Is Nothing Then
                itemPort.Parent = itemAction
                itemPort.keyДействие = itemAction.keyДействие
            End If

            SetParentForBit(itemPort)
        Next
    End Sub

    Private Sub SetParentForBit(itemPort As Порт)
        For Each itemBit As Бит In itemPort.Биты
            If itemBit.Parent Is Nothing Then
                itemBit.Parent = itemPort
                itemBit.KeyПорта = itemPort.KeyПорта
            End If
        Next
    End Sub
#End Region

#End Region

#Region "События контролов"
    Private Sub TSButtonSaveInDataSet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonSaveInDataSet.Click
        SaveTree()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveToolStripMenuItem.Click
        SaveTree()
    End Sub

    Private Sub TSButtonHandCheck_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonHandCheck.CheckedChanged
        If Ports Is Nothing Then Exit Sub
        If TSButtonHandCheck.Checked Then
            For Each itemPort As Port In Ports.Values
                For Each itemLed As Led In itemPort.AllLedPortLine.Values
                    itemLed.InteractionMode = BooleanInteractionMode.SwitchWhenPressed
                    AddHandler itemLed.StateChanged, AddressOf Led_StateChanged
                    'AddHandler LedTemp.ValueChanged, AddressOf Led_ValueChanged
                    itemLed.Value = False
                Next
            Next
        Else
            For Each itemPort As Port In Ports.Values
                For Each itemLed As Led In itemPort.AllLedPortLine.Values
                    itemLed.Value = False
                    RemoveHandler itemLed.StateChanged, AddressOf Led_StateChanged
                    'RemoveHandler LedTemp.ValueChanged, AddressOf Led_ValueChanged
                    itemLed.InteractionMode = BooleanInteractionMode.Indicator
                Next
            Next
        End If
    End Sub

    Private Sub TSButtonStart_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonStart.CheckedChanged
        TSMenuItemStart.Checked = TSButtonStart.Checked
    End Sub

    Private Sub TSMenuItemStart_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSMenuItemStart.CheckedChanged
        StartObservationOnOccurrence(TSMenuItemStart)
    End Sub

    ''' <summary>
    ''' Пуск Цикла
    ''' </summary>
    ''' <param name="item"></param>
    Private Sub StartObservationOnOccurrence(ByVal item As ToolStripMenuItem)
        TSButtonStart.Checked = item.Checked
        If NameParametersForControl Is Nothing Then PopulateListParametersFromServer()

        If TSButtonStart.Checked Then
            LoadAllSelectedConfigurations()
            isObservationOnOccurrenceStart = isCheckLoadSuccess
            IsMonitorDigitalOutputPort = isCheckLoadSuccess
            TSMenuItemStart.Checked = isCheckLoadSuccess

            If isCheckLoadSuccess Then
                'ReDim_ParameterAccumulate(UBound(ParametersType)) ' обнулить массив
                Re.Dim(ParameterAccumulate, UBound(ParametersType)) ' обнулить массив

                TSButtonHandCheck.Checked = False
                TSButtonHandCheck.Enabled = False

                ToolStripButtonNew.Enabled = False
                ToolStripButtonDelete.Enabled = False
                'mnuOptionsMenuItem.Enabled = False
                TSButtonInterruptObservation.Enabled = True
                TabControlConfig.SelectedIndex = 0
                TabControlConfig.TabPages("TabPageEdit").Hide()
                ShowMessageOnStatusPanel("Пуск наблюдения за событиями включен")
            Else
                Exit Sub
            End If
        Else
            TSButtonHandCheck.Enabled = True
            ToolStripButtonNew.Enabled = True
            ToolStripButtonDelete.Enabled = True
            'mnuOptionsMenuItem.Enabled = True
            TabControlConfig.TabPages("TabPageEdit").Show()
            TSButtonInterruptObservation.Enabled = False

            ShowMessageOnStatusPanel("Пуск наблюдения за событиями выключен")
            TSMenuItemStart.Checked = False

            If isCheckLoadSuccess Then SetAllDeviceAtZero()
            StopTaskDigitalOutput()
            ' там blnНаблюдениеЗаСобытиямиЗапущено = False blnМониторDigitalOutputPort = False
            SetLedOffColor()
            ' GenerateNull()
        End If

        'If Not ПроверкаЗагрузкиПроведена OrElse blnPropertiesChanging Then  ЗагрузитьВсеКонфигурацииИспытания()
    End Sub

    Private Sub TSButtonStart_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonStart.EnabledChanged
        If ListBoxGroupConfigurations.Items.Count = 0 Then CType(sender, ToolStripButton).Enabled = False
        TSMenuItemStart.Enabled = TSButtonStart.Enabled
    End Sub

    Private Sub TSButtonInterruptObservation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TSButtonInterruptObservation.Click
        TSButtonStart.Checked = False
    End Sub

    Private Sub Led_StateChanged(ByVal sender As Object, ByVal e As ActionEventArgs)
        'Dim Index As String = CType(sender, NationalInstruments.UI.WindowsForms.Led).Tag
        'CWButtonАвария(Index).Visible = False

        ' в теге содержится имя линии цифрового порта
        Dim digitalWriteTask As Task = New Task("digital")

        Try
            digitalWriteTask.DOChannels.CreateChannel(CType(sender, Led).Tag.ToString, "", ChannelLineGrouping.OneChannelForAllLines) ' работает
            '  Запись в цифровой порт значение. WriteDigitalSingChanSingSampPort записывает набор данных
            '  или цифроваые данные по требованию, таким образом в timeout нет необходимости.
            Dim writer As DigitalSingleChannelWriter = New DigitalSingleChannelWriter(digitalWriteTask.Stream)
            digitalWriteTask.Control(TaskAction.Verify)
            Dim arrBitBoolean() As Boolean = New Boolean() {CType(sender, Led).Value}
            writer.WriteSingleSampleMultiLine(True, arrBitBoolean)
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(Led_StateChanged)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            digitalWriteTask.Dispose()
        End Try
    End Sub

    Private Sub TimerResize_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerResize.Tick
        TimerResize.Enabled = False
        DefineSizetableLayoutPanelPort()
    End Sub

    Private Sub TabControlConfig_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlConfig.SelectedIndexChanged
        ' при смене закладок - если запуск закладку погасить
        If TSButtonStart.Checked Then
            If TabControlConfig.TabPages(TabControlConfig.SelectedIndex) Is TabControlConfig.TabPages("TabPageEdit") Then
                TabControlConfig.TabPages("TabPageEdit").Hide()
            End If
        End If
    End Sub

    Private Sub ToolStripButtonNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButtonNew.Click
        CheckAddingNewAction()
    End Sub

    ''' <summary>
    ''' Поверить на непротиворечивость добавления нового действия к списку уже отобранных.
    ''' </summary>
    Private Sub CheckAddingNewAction()
        If ComboBoxConfigurations.SelectedIndex = -1 Then Exit Sub

        If ListBoxGroupConfigurations.Items.Contains(ComboBoxConfigurations.SelectedItem) Then
            Const TextMessage As String = "Конфигурация уже добавлена в список."
            ShowMessageOnStatusPanel(TextMessage)
            MessageBox.Show(TextMessage, "Вставка конфигурации в список", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' в цикле проверить чтобы не было дважды использования Битов
        Dim isPortContainNewBits As Boolean ' номер Порта Уже Есть

        For Each itemNewAction As Действие In CType(ComboBoxConfigurations.SelectedItem, Конфигурация).Действия
            For Each itemNewPort As Порт In itemNewAction.Порты
                For Each itemNewBit As Бит In itemNewPort.Биты
                    isPortContainNewBits = CheckRepetition(isPortContainNewBits, itemNewPort, itemNewBit)
                Next
            Next
        Next

        If Not isPortContainNewBits Then
            ListBoxGroupConfigurations.Items.Add(ComboBoxConfigurations.SelectedItem)
            ListBoxGroupConfigurations.SelectedItem = ComboBoxConfigurations.SelectedItem
            Const TextMessage As String = "Добавлена новая конфигурация в список"
            ShowMessageOnStatusPanel(TextMessage)
        End If

        TSButtonStart.Enabled = FormMainReference IsNot Nothing
        ToolStripButtonDelete.Enabled = True
        isCheckLoadSuccess = False
        SetLedOffColor()
    End Sub

    ''' <summary>
    ''' Поверить на присутствие битов из нового добавляемого действия к списку уже отобранных.
    ''' </summary>
    ''' <param name="isPortContainNewBits"></param>
    ''' <param name="itemNewPort"></param>
    ''' <param name="itemNewBit"></param>
    ''' <returns></returns>
    Private Function CheckRepetition(isPortContainNewBits As Boolean, itemNewPort As Порт, itemNewBit As Бит) As Boolean
        For Each itemConfig As Конфигурация In ListBoxGroupConfigurations.Items
            For Each itemAction As Действие In itemConfig.Действия
                For Each itemPort As Порт In itemAction.Порты
                    If itemPort.НомерМодуляКорзины = "" And itemNewPort.НомерМодуляКорзины = "" Then
                        ' плата
                        If itemPort.НомерУстройства = itemNewPort.НомерУстройства AndAlso itemPort.НомерПорта = itemNewPort.НомерПорта Then
                            For Each itemBit As Бит In itemPort.Биты
                                If itemBit.НомерБита = itemNewBit.НомерБита Then
                                    isPortContainNewBits = True
                                    Dim TextMessage As String = "Повторное использование линии порта!" & vbCrLf & vbCrLf _
                                    & "Используется в следующей конфигурации" & vbCrLf _
                                    & "Конфигурация:" & itemConfig.Name & vbCrLf _
                                    & "Действие:" & itemAction.Name & vbCrLf _
                                    & "Плата:" & itemPort.НомерУстройства & vbCrLf _
                                    & "Порт:" & itemPort.НомерПорта & vbCrLf _
                                    & "Бит:" & itemBit.НомерБита & vbCrLf _
                                    & "Добавление невозможно."

                                    ShowMessageOnStatusPanel(TextMessage)
                                    MessageBox.Show(TextMessage, "Вставка конфигурации в список", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        ' модуль SCXI
                        If (itemPort.НомерУстройства = itemNewPort.НомерУстройства) AndAlso (itemPort.НомерМодуляКорзины = itemNewPort.НомерМодуляКорзины) AndAlso (itemPort.НомерПорта = itemNewPort.НомерПорта) Then
                            For Each itemBit As Бит In itemPort.Биты
                                If itemBit.НомерБита = itemNewBit.НомерБита Then
                                    isPortContainNewBits = True
                                    Dim textMessage As String = "Повторное использование линии порта!" & vbCrLf & vbCrLf _
                                    & "Используется в следующей конфигурации" & vbCrLf _
                                    & "Конфигурация:" & itemConfig.Name & vbCrLf _
                                    & "Действие:" & itemAction.Name & vbCrLf _
                                    & "Корзина:" & itemPort.НомерУстройства & vbCrLf _
                                    & "Модуль:" & itemPort.НомерМодуляКорзины & vbCrLf _
                                    & "Порт:" & itemPort.НомерПорта & vbCrLf _
                                    & "Бит:" & itemBit.НомерБита & vbCrLf _
                                    & "Добавление невозможно."

                                    ShowMessageOnStatusPanel(textMessage)
                                    MessageBox.Show(textMessage, "Вставка конфигурации в список", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Next ' Порт
            Next ' Действие
        Next ' Конфигурация

        Return isPortContainNewBits
    End Function

    Private Sub ToolStripButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolStripButtonDelete.Click
        If ListBoxGroupConfigurations.SelectedIndex <> -1 Then
            Dim textMessage As String

            If ListBoxGroupConfigurations.Items.Count = 1 Then
                textMessage = "Последнюю конфигурацию удалять из списка нельзя!"
                ShowMessageOnStatusPanel(textMessage)
                MessageBox.Show(textMessage, "Удаление конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ListBoxGroupConfigurations.Items.Remove(ListBoxGroupConfigurations.SelectedItem)
            textMessage = "Конфигурация успешно удалена из списка"
            ShowMessageOnStatusPanel(textMessage)
            isCheckLoadSuccess = False
            SetLedOffColor()
        End If
    End Sub
#End Region

#Region "Функции"
    ''' <summary>
    ''' Монитор цикл по конфигурациям и исполнение портов
    ''' </summary>
    Public Sub Monitor()
        If isObservationOnOccurrenceStart Then ExecuteOccurrencesConfiguration(False)
    End Sub

    ''' <summary>
    ''' Заполнить Списки Параметров От Сервера
    ''' </summary>
    Public Sub PopulateListParametersFromServer() ' вызывается из CharacteristicForRegime
        Dim memoBoolean As Boolean = IsMonitorDigitalOutputPort

        IsMonitorDigitalOutputPort = False
        'ReDim_NameParametersForControl(UBound(IndexParametersForControl))
        Re.Dim(NameParametersForControl, UBound(IndexParametersForControl))
        NameParametersForControl(0) = MissingParameter

        For I As Integer = 1 To UBound(IndexParametersForControl)
            NameParametersForControl(I) = ParametersType(IndexParametersForControl(I)).NameParameter
        Next

        'ReDim_ParameterAccumulate(UBound(ParametersType)) ' обнулить массив
        Re.Dim(ParameterAccumulate, UBound(ParametersType)) ' обнулить массив
        IsMonitorDigitalOutputPort = memoBoolean
    End Sub

    Private Sub ShowMessageOnStatusPanel(ByVal message As String)
        Status.Text = message
    End Sub

    'Private Sub ОчиститьПанель()
    '    Status.Text = ""
    'End Sub

    Public Function GetKeyId() As Integer
        Return CInt(Int((1000000 * rnd.NextDouble) + 1))
    End Function

    ''' <summary>
    ''' Установить Все Устройства В Нуль
    ''' </summary>
    Private Sub SetAllDeviceAtZero()
        If Not isCheckLoadSuccess Then LoadAllSelectedConfigurations()
        ' в случае успеха
        If isCheckLoadSuccess Then ExecuteOccurrencesConfiguration(True)
    End Sub

    Private Function TestFunction(ByVal Expression As String) As Boolean
        Dim eval As New JScriptUtil.ExpressionEvaluator()
        Dim result As Double

        Expression = Expression.Replace("ARG", "1")

        Try
            result = CDbl(eval.Evaluate(Expression))
        Catch ex As Exception
            'MessageBox.Show(ex.ToString & vbCrLf & vbCrLf & "Математическое выражение содержит ошибку." & vbCrLf & _
            '"Загрузка графика будет прекращена." & vbCrLf & _
            '"Необходимо отредактировать формулу в редакторе.", "Ошибка в формуле", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Обновить Фон Индикаторов
    ''' </summary>
    Private Sub SetLedOffColor()
        'Dim arrayObject(УстройстваВЦикле.Count - 1) As Object
        'Dim I As Integer
        'For Each tempDevice As PortDevice In УстройстваВЦикле.Values
        '    arrayObject(I) = tempDevice
        '    I += 1
        'Next
        ''добавляем как массив объектов
        'ListBoxГруппаКонфигураций.Items.AddRange(arrayObject)
        'ListBoxГруппаКонфигураций.DisplayMember = "ИмяКонфигурации"
        'ListBoxГруппаКонфигураций.ValueMember = "keyКонфигурацияДействия"
        'If I > 0 Then
        '    ListBoxГруппаКонфигураций.SelectedIndex = 0
        '    IndexТекущегоРедактируемогоУстройства = 0 '-1
        '    blnЦиклЗагрузкиЗагружен = True
        '    ПроверкаЗагрузкиПроведена = False
        'End If

        If Ports IsNot Nothing Then
            For Each itemPort As Port In Ports.Values
                For I As Integer = 0 To itemPort.LineCount 'Mod<slot#>/port0/lineN" 'Dev0/port1/line0
                    Dim Led As Led = CType(itemPort.TableLayoutPanel.Controls.Item("LedPortLine" & I.ToString), Led)
                    Led.Value = False
                    Led.OffColor = mOffColor
                    Led.Caption = I.ToString
                Next
            Next
        End If
    End Sub

    Private Sub ComboBoxPhysicalPort_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxPhysicalPort.SelectedIndexChanged
        If IsHandleCreated AndAlso ComboBoxPhysicalPort.SelectedIndex <> -1 AndAlso ComboBoxLineCount.Items.Count > 0 Then
            ComboBoxLineCount.SelectedIndex = ComboBoxPhysicalPort.SelectedIndex
        End If
    End Sub
#End Region

End Class
