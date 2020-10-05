Imports System.Drawing.Design
Imports System.ComponentModel.Design
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Soap
Imports NationalInstruments.DAQmx
Imports System.Xml.Linq
Imports System.ComponentModel
Imports NationalInstruments.UI.WindowsForms
' при смене версии контролов NI поменять в XML следующие атрибуты: 
' 1.Version=13.0.40.242 => 13.0.45.242
' 2.PublicKeyToken=dc6ad606294fc298 => 4febd62461bf11a4
' 3.Registration/Registration на RedactorFunctionPanels/RedactorFunctionPanels

'Version=9.0.40.292, Culture=neutral, PublicKeyToken=dc6ad606294fc298
'Version=13.0.45.242, Culture=neutral, PublicKeyToken=4febd62461bf11a4
' в файлах LoadNIforInit.xml и во всех файлах папки ПанелиМоториста

' значить надо добавить функционал для сохранения всего каталога с файлами XML в каком-то каталоге с добавлением даты и
' во всех файлах XML каталога пакетно поменять Version (новое на старое) и может быть не один раз

''' <summary>
''' Редактор функциональных панелей.
''' Позволяет связывать контролы пользовательского управления с сетевыми переменными
''' </summary>
''' <remarks></remarks>
Friend Class FormEditorPanelMotorist
    Public Enum EnumChannelType
        <Description("ЦифровойЧтение")>
        AnalogInput
        <Description("ЦифровойЗапись")>
        AnalogOutput
        <Description("ЛогическийЧтение")>
        DigitalInput
        <Description("ЛогическийЗапись")>
        DigitalOutput
        <Description("Элемент не для связывания")>
        None
    End Enum

    Public Enum EnumControl
        <Description("Label")>
        LabelControl
        <Description("Switch")>
        SwitchControl
        <Description("Led")>
        LedControl
        <Description("NumericEdit")>
        NumericEditControl

        '<Description("TextBox")> _
        'TextBoxControl
        '<Description("Button")> _
        'ButtonControl
        '<Description("CheckBox")> _
        'CheckBoxControl
        '<Description("RadioButton")> _
        'RadioButtonControl
        <Description("Knob")>
        KnobControl
        <Description("Slide")>
        SlideControl

        <Description("Panel")>
        PanelControl
        <Description("GroupBox")>
        GroupBoxControl
        <Description("TabControl")>
        TabControl
        <Description("TableLayoutPanel")>
        TableLayoutPanelControl
        <Description("FlowLayoutPanel")>
        FlowLayoutPanelControl
        <Description("PictureBox")>
        PictureBoxControl

        <Description("Tank")>
        TankControl
        <Description("Gauge")>
        GaugeControl
        <Description("Thermometer")>
        ThermometerControl
        <Description("Meter")>
        MeterControl

        <Description("WaveformGraph")>
        WaveformGraphControl
        <Description("ScatterGraph")>
        ScatterGraphControl

        <Description("Неизв.")>
        Unknown
    End Enum

    Public Property PropertyGridChannelType As EnumChannelType = EnumChannelType.None
    Public HostSurfaceManagerFuncPanel As HostSurfaceManager = Nothing
    Private formCount As Integer = 0
    'Private _userControlCount As Integer = 0
    'Private _componentCount As Integer = 0
    'Private _grapherCount As Integer = 0
    'Private _hostSurfaceManager As HostSurfaceManager = Nothing

    Private prevIndex As Integer = 0
    Private curIndex As Integer = 0
    Private ReadOnly tempFileSoap As String = PathResourses & "\SoapFormatterControl.xml"
    Private ReadOnly pathLoadNIforInit As String = PathResourses & "\LoadNIforInit.xml"
    Private isFormClosed As Boolean ' 2 раза не показывать при выходе по кнопке крест
    Private isFormClosing As Boolean

    Public Sub New()
        MyBase.New()

        If FileNotExists(tempFileSoap) Then
            Const caption As String = "Запуск редактора"
            Const text As String = "В каталоге нет файла SoapFormatterControl.xml!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If
        If FileNotExists(pathLoadNIforInit) Then
            Const caption As String = "Запуск редактора"
            Const text As String = "В каталоге нет файла LoadNIforInit.xml!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CustomInitialize()
        'Me.РодительскаяФорма = РодительскаяФорма 'доработка
        MdiParent = MainMdiForm
        'Me.MdiParent = Registration.fMainForm 'сделано так потому что родительская форма может и не быть т.к. будет вызванно из frmMainMDI

        ' Add any initialization after the InitializeComponent() call.
        ToFindOutStructureEquipmentChannels()

        'работа с компонентом свойств National
        'plotLinePropertyEditor.Source = New PlotColorPropertyEditorSource(defaultWaveformPlot, "LineColor")
        'xAxisRangePropertyEditor.Source = New RangePropertyEditorSource(defaultXAxis, "Range")
        ''''''''''''''''''''''''''
        'EnumControl
        For Each value As Object In [Enum].GetValues(GetType(EnumControl))
            ComboBoxTypeControl.Items.Add(value)
            'If DefaultPropertyEditor.BorderStyle.Equals(value) Then
            '    propertyEditorBorderStyleComboBox.SelectedItem = value
            'End If
        Next

        'For Each value As Object In [Enum].GetValues(GetType(BorderStyle))
        '    propertyEditorBorderStyleComboBox.Items.Add(value)
        '    If DefaultPropertyEditor.BorderStyle.Equals(value) Then
        '        propertyEditorBorderStyleComboBox.SelectedItem = value
        '    End If
        'Next
        'xAxisLabelFormatPropertyEditor.Source = New PropertyEditorSource(xAxis1.MajorDivisions, "LabelFormat")
    End Sub

    ''' <summary>
    ''' Узнать Состав Каналов Оборудования
    ''' </summary>
    Private Sub ToFindOutStructureEquipmentChannels()
        ' узнать конфигурацию платы
        '(Led.Tag = "SC1Mod<slot#>/port" & mНомерПорта.ToString & "/line" & i.ToString) '"SC" & НомерУстройства & "Mod<slot#>/port0/lineN" 'Dev0/port1/line0:2
        '"SC" & НомерУстройства может ссылаться на на "Dev" & НомерУстройства 
        ' поэтому если есть ссылка "SC" & НомерУстройства = "Dev" & НомерУстройства , то порты с "Dev" не показывать
        deviceComboBox.Items.AddRange(DaqSystem.Local.Devices)

        If (deviceComboBox.Items.Count > 0) Then deviceComboBox.SelectedIndex = 0

        'physicalPortComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
        physicalPortComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External).Where(Function(nameDev) Not nameDev.StartsWith("Sim")).ToArray)

        physicalPortAnalogOutputComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))

        'physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
        'physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External))
        Dim wordsPhysicalChannel As String() = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DILine, PhysicalChannelAccess.External).Where(Function(nameDev) Not nameDev.StartsWith("Sim")).ToArray
        physicalChannelComboBox.Items.AddRange(wordsPhysicalChannel)

        If physicalChannelComboBox.Items.Count > 0 Then physicalChannelComboBox.SelectedIndex = 0
        If physicalPortAnalogOutputComboBox.Items.Count > 0 Then physicalPortAnalogOutputComboBox.SelectedIndex = 0

        LinqLineCount(wordsPhysicalChannel, physicalPortComboBox, ComboBoxLineCount)

        If (physicalPortComboBox.Items.Count > 0) Then
            physicalPortComboBox.SelectedIndex = 0
            'InitializeComponentDigital()
        End If

        'OnScaleModeChanged()
        'booleanComboBox.SelectedIndex = 0

        CheckCountsDigitalWriteTask(deviceComboBox, physicalPortComboBox, ComboDigitalWriteTask)
    End Sub

    Private Sub FormEditorPanelMotorist_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' заглушка для загрузки класса HostControl с элементом NI
        ' без этого первоначальной загрузки другие формы не грузятся BasicHostLoaderRun
        'Dim hc As HostControl = HostSurfaceManagerFuncPanel.GetNewHost(предварительнаяЗагрузка) 'fileName)
        'hc.Dispose()
        'hc = Nothing
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub FormEditorPanelMotorist_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If tabControlDesign.TabCount = 0 Then Exit Sub
        If isFormClosing = True Then Exit Sub

        Const Caption As String = "Внимание!"
        ' Проверить выключение производит пользователь или Панель Управления
        If e.CloseReason = CloseReason.UserClosing Then
            Const Message As String = "Вы действительно хотите закрыть Окно Редактора?"
            Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim Result As DialogResult = MessageBox.Show(Message, Caption, Buttons, MessageBoxIcon.Question)

            If Result = DialogResult.No Then
                'Close()
                e.Cancel = True
            End If
        ElseIf e.CloseReason = CloseReason.WindowsShutDown Then
        End If

        If e.Cancel = False Then
            Const Message As String = "Произвести сохранение произведённых изменений?"
            Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim Result As DialogResult = MessageBox.Show(Message, Caption, Buttons, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then
                ' закрыть все вкладки
                For indexTab As Integer = tabControlDesign.TabCount - 1 To 0 Step -1
                    tabControlDesign.SelectedIndex = indexTab
                    saveMenuItem_Click(saveMenuItem, Nothing) 'записать
                    DeleteCurrentTabPage()
                Next
            End If
            isFormClosing = True
        End If
    End Sub
    Private Sub FormEditorPanelMotorist_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        If isFormClosed = False Then
            isFormClosed = True
            RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
            MainMdiForm.MenuEditorPanel.Enabled = True
            MainMdiForm.MenuShowPanel.Enabled = True
        End If
    End Sub

    Private Sub exitMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitMenuItem.Click
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        Close()
    End Sub

    ''' <summary>
    ''' Добавить пользовательские сервисы в HostManager подобно TGoolbox, PropertyGrid, SolutionExplorer.
    ''' OutputWindow добавлен как сервис. Он добавлен посредством HostSurfaceManager
    ''' для записи из OutputWindow. Можно добавить любые сервисы по желанию.
    ''' </summary>
    Private Sub CustomInitialize()
        HostSurfaceManagerFuncPanel = New HostSurfaceManager()
        HostSurfaceManagerFuncPanel.AddService(GetType(IToolboxService), Toolbox)

        ' установить SetToolTip для кнопок раскрытия вкладок
        For I = 0 To Toolbox.Controls.Count - 1 - 2 '2 служебных элемента
            ToolTip1.SetToolTip(Toolbox.Controls(I), Toolbox.HelpToolboxItemDictionary(Toolbox.Controls(I).Text))
        Next

        ' изменил
        '_hostSurfaceManager.AddService(GetType(ToolWindows.SolutionExplorer), Me.solutionExplorer1)
        '_hostSurfaceManager.AddService(GetType(ToolWindows.OutputWindow), Me.OutputWindow)
        HostSurfaceManagerFuncPanel.AddService(GetType(ToolStripStatusLabel), tsStatusLabel)
        HostSurfaceManagerFuncPanel.AddService(GetType(PropertyGrid), propertyGridDesign)

        'codeDomDesignerLoaderMenuItem_Click(Nothing, Nothing) ' установить данный пункт меню
        AddHandler tabControlDesign.SelectedIndexChanged, New EventHandler(AddressOf tabControl1_SelectedIndexChanged)
        'Me.noLoaderMenuItem_Click(Nothing, Nothing) ' установить данный пункт меню
        basicDesignerLoaderMenuItem_Click(Nothing, Nothing) ' установить данный пункт меню
    End Sub

    ''' <summary>
    ''' Текущий индекс вкладки холста дизайнера
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentDocumentsDesignIndex() As Integer
        Get
            Dim codeText As String
            Dim designText As String
            Dim index As Integer = 0

            If CurrentDocumentView = StringsConst.Design Then
                Return tabControlDesign.SelectedIndex ' выдать индекс закладки с дизайнером
            Else
                ' Это замена лексемы StringsConst.Code в заголовке вкладки. Это позволит найти Design View
                codeText = tabControlDesign.TabPages(tabControlDesign.SelectedIndex).Text.Trim()
                designText = codeText.Replace(StringsConst.Code, StringsConst.Design)
                For Each tab As TabPage In tabControlDesign.TabPages
                    If tab.Text = designText Then
                        Return index
                    End If
                    index += 1
                Next
            End If

            Return -1
        End Get
    End Property

    ''' <summary>
    ''' Текущий индекс вкладки кода дизайнера
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentDocumentsCodeIndex() As Integer
        Get
            ' текущая уже выделена
            If CurrentDocumentView = StringsConst.Code Then
                Return tabControlDesign.SelectedIndex
            End If

            Dim index As Integer = 0

            'HostControl currentHostControl = CurrentDocumentsHostControl;
            ' Узнать есть ли Code Tab уже существует
            Dim designText As String = tabControlDesign.TabPages(tabControlDesign.SelectedIndex).Text.Trim()
            Dim codeText As String = designText.Replace(StringsConst.Design, StringsConst.Code)

            For Each tab As TabPage In tabControlDesign.TabPages
                If tab.Text = codeText Then
                    Return index
                End If

                index += 1
            Next

            ' вкладки нет, необходимо создать нужную
            Dim tabPage As New TabPage() With {.Text = codeText, .Tag = CurrentActiveDocumentLoaderType}
            tabControlDesign.Controls.Add(tabPage)

            ' Создать новый RichTextBox для codeEditor
            Dim codeEditor As RichTextBox = New RichTextBox With {
                .BackColor = SystemColors.Desktop,
                .ForeColor = Color.White,
                .Dock = DockStyle.Fill,
                .Font = New Font("Verdana", 9.25F, FontStyle.Regular, GraphicsUnit.Point, CByte((0))),
                .Location = New Point(0, 0),
                .ScrollBars = RichTextBoxScrollBars.Both,
                .WordWrap = False,
                .Size = New Size(284, 247),
                .TabIndex = 0,
                .[ReadOnly] = True,
                .Text = ""
            }
            tabPage.Controls.Add(codeEditor)

            Return tabControlDesign.TabPages.Count - 1
        End Get
    End Property

    ''' <summary>
    ''' Вернуть текущий редактор кода
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentDocumentsHostControl() As HostControl
        Get
            Return DirectCast(tabControlDesign.TabPages(CurrentDocumentsDesignIndex).Controls(0), HostControl)
        End Get
    End Property

    ''' <summary>
    ''' Вернуть текущий редактор кода
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentDocumentsCodeEditor() As RichTextBox
        Get
            Return DirectCast(tabControlDesign.TabPages(CurrentDocumentsCodeIndex).Controls(0), RichTextBox)
        End Get
    End Property

    ''' <summary>
    ''' Вернуть тип загрузчика дизайнера в соответствии с отмеченным пунктом меню
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentMenuSelectionLoaderType() As LoaderType
        Get
            If basicDesignerLoaderMenuItem.Checked Then
                Return LoaderType.BasicDesignerLoader
            ElseIf codeDomDesignerLoaderMenuItem.Checked Then
                Return LoaderType.CodeDomDesignerLoader
            Else
                Return LoaderType.NoLoader
            End If
        End Get
    End Property

    ''' <summary>
    ''' Вернуть тип загрузчика дизайнера в соответствии с выделенной вкладкой холста дизайнера
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentActiveDocumentLoaderType() As LoaderType
        Get
            Dim tabPage As TabPage = tabControlDesign.TabPages(tabControlDesign.SelectedIndex)
            Return DirectCast(tabPage.Tag, LoaderType)
        End Get
    End Property

    ''' <summary>
    ''' Строковое представление типа выделенной вкладкой холста дизайнера
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property CurrentDocumentView() As String
        Get
            Dim tabPage As TabPage = tabControlDesign.TabPages(tabControlDesign.SelectedIndex)

            If tabPage.Text.Contains(StringsConst.Design) Then
                Return StringsConst.Design
            Else
                Return StringsConst.Code
            End If
        End Get
    End Property

    Private Class StringsConst
        Public Const Design As String = "Design"
        Public Const Code As String = "Code"
        Public Const Xml As String = "Xml"
        Public Const CS As String = "C#"
        'Public Const JS As String = "J#"
        Public Const VB As String = "VB"
    End Class

#Region "дополнительные свойства контрола"

    ''' <summary>
    ''' Преобразовать таблицу дополнительных свойств контрола управления в сериализуемый XML формат 
    ''' </summary>
    ''' <param name="propControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetTag(ByVal propControl As PropertiesControlBase) As String
        ' записать
        SerializePropertiesControlBaseToXML(tempFileSoap, propControl)
        ' затем считать как XDocument
        Dim doc As XDocument = XDocument.Load(tempFileSoap)
        ' Затем преобразовать документ с исключением форматирования для отображения
        PropertyGrid2.SelectedObject = propControl
        Return doc.ToString(Xml.Linq.SaveOptions.DisableFormatting)
    End Function

    ''' <summary>
    ''' Из сериализованной строки восстановить propControlBase и отобразить в PropertyGrid
    ''' </summary>
    ''' <param name="strTag"></param>
    ''' <remarks></remarks>
    Private Sub GetTag(ByVal strTag As String)
        ' записать строку в файл
        Using sw As StreamWriter = New StreamWriter(tempFileSoap)
            sw.Write(strTag)
            'sw.Close() ' FxCop
        End Using

        ' считать с преобразованием
        Dim propControlBase As PropertiesControlBase = DirectCast(DeserializePropertiesControlBaseFromXML(tempFileSoap), PropertiesControlBase)

        ' отобразить
        PropertyGrid2.SelectedObject = propControlBase

        'For Each value As Object In [Enum].GetValues(GetType(EnumControl))
        '    ComboBoxТипКонтролов.Items.Add(value)
        '    'If DefaultPropertyEditor.BorderStyle.Equals(value) Then
        '    '    propertyEditorBorderStyleComboBox.SelectedItem = value
        '    'End If
        'Next
        ComboBoxTypeControl.Text = propControlBase.Тип.ToString
    End Sub

    ''' <summary>
    ''' Отобразить дополнительные свойства контрола при его выделении в дизайнере
    ''' Вывести Свойства Контрола
    ''' </summary>
    ''' <param name="inControl"></param>
    ''' <remarks></remarks>
    Public Sub ShowPropertyControl(ByVal inControl As Object)
        PropertyGrid2.SelectedObject = Nothing
        Const cEditing As String = "Выбран элемент: "
        PropertyGridChannelType = EnumChannelType.None

        Try
            'If inControl.Tag Is Nothing OrElse myControl.Tag = "" Then
            '--- Вывести надпись редактируемый тип контрола ---------------
            '--- Дискретные вход/выход ------------------------------------
            Dim typeControl As Type = inControl.GetType

            If typeControl Is GetType(WindowsForms.Switch) Then
                'Dim cSwitch As NationalInstruments.UI.WindowsForms.Switch = CType(inControl, NationalInstruments.UI.WindowsForms.Switch)
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.SwitchControl)
                PropertyGridChannelType = EnumChannelType.DigitalOutput
                CType(inControl, WindowsForms.Switch).Caption = "0"
            ElseIf typeControl Is GetType(Led) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.LedControl)
                PropertyGridChannelType = EnumChannelType.DigitalInput
                CType(inControl, Led).Caption = "0"
                '--- Аналоговый выход -------------------------------------
            ElseIf typeControl Is GetType(Knob) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.KnobControl)
                PropertyGridChannelType = EnumChannelType.AnalogOutput
                CType(inControl, Knob).Caption = "0"
            ElseIf typeControl Is GetType(Slide) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.SlideControl)
                PropertyGridChannelType = EnumChannelType.AnalogOutput
                CType(inControl, Slide).Caption = "0"

                '--- Аналоговый вход --------------------------------------
            ElseIf typeControl Is GetType(NumericEdit) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.NumericEditControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
            ElseIf typeControl Is GetType(Tank) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.TankControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
                CType(inControl, Tank).Caption = "0"
            ElseIf typeControl Is GetType(Gauge) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.GaugeControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
                CType(inControl, Gauge).Caption = "0"
            ElseIf typeControl Is GetType(Thermometer) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.ThermometerControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
                CType(inControl, Thermometer).Caption = "0"
            ElseIf typeControl Is GetType(Meter) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.MeterControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
                CType(inControl, Meter).Caption = "0"

            ElseIf typeControl Is GetType(WaveformGraph) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.WaveformGraphControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput
                'ElseIf type__1 Is GetType(NationalInstruments.UI.WaveformPlot) Then
            ElseIf typeControl Is GetType(ScatterGraph) Then
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.ScatterGraphControl)
                PropertyGridChannelType = EnumChannelType.AnalogInput

                'ElseIf type__1 Is GetType(NationalInstruments.UI.ScatterPlot) Then
                'ElseIf type__1 Is GetType(NationalInstruments.UI.XAxis) Then
                'ElseIf type__1 Is GetType(NationalInstruments.UI.YAxis) Then

                '--- Стандартные .Net -------------------------------------
                'If inControl.GetType Is GetType(System.Windows.Forms.Label) Then
            ElseIf typeControl Is GetType(Label) Then
                ComboBoxTypeControl.Text = EnumControl.LabelControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.LabelControl)
                'ElseIf type__1 Is GetType(TextBox) Then
                'ElseIf type__1 Is GetType(Button) Then
                'ElseIf type__1 Is GetType(CheckBox) Then
                'ElseIf type__1 Is GetType(RadioButton) Then
                'ElseIf type__1 Is GetType(System.Windows.Forms.TabPage) Then
            ElseIf typeControl Is GetType(Panel) Then
                ComboBoxTypeControl.Text = EnumControl.PanelControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.PanelControl)
            ElseIf typeControl Is GetType(GroupBox) Then
                ComboBoxTypeControl.Text = EnumControl.GroupBoxControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.GroupBoxControl)
            ElseIf typeControl Is GetType(TabControl) Then
                ComboBoxTypeControl.Text = EnumControl.TabControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.TabControl)
            ElseIf typeControl Is GetType(TableLayoutPanel) Then
                ComboBoxTypeControl.Text = EnumControl.TableLayoutPanelControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.TableLayoutPanelControl)
            ElseIf typeControl Is GetType(FlowLayoutPanel) Then
                ComboBoxTypeControl.Text = EnumControl.FlowLayoutPanelControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.FlowLayoutPanelControl)
            ElseIf typeControl Is GetType(PictureBox) Then
                ComboBoxTypeControl.Text = EnumControl.PictureBoxControl.ToString
                LabelSelectedControl.Text = cEditing & ConvertTo(GetType(EnumControl), EnumControl.PictureBoxControl)

                '--- Стандартные .Net не представленные в ComboBoxTypeControl -
            ElseIf typeControl Is GetType(TabPage) Then
                ComboBoxTypeControl.Text = EnumControl.Unknown.ToString
                LabelSelectedControl.Text = cEditing & "TabPage"
            ElseIf typeControl Is GetType(Form) Then
                ComboBoxTypeControl.Text = EnumControl.Unknown.ToString
                LabelSelectedControl.Text = cEditing & "Form"
            Else
                LabelSelectedControl.Text = ""
            End If

            Dim myControl As Control = CType(inControl, Control)
            '--- Нужно ли обновить TAG ----------------------------------------
            'If String.IsNullOrEmpty(myControl.Tag.ToString) Then сбоит
            If myControl.Tag Is Nothing OrElse myControl.Tag.ToString = "" Then
                'Dim type__1 As Type = myControl.GetType
                '--- Дискретные вход/выход ------------------------------------
                If typeControl Is GetType(WindowsForms.Switch) Then
                    'Dim cSwitch As NationalInstruments.UI.WindowsForms.Switch = CType(myControl, NationalInstruments.UI.WindowsForms.Switch)
                    myControl.Tag = SetTag(New SwitchProperties(myControl.Name))
                ElseIf typeControl Is GetType(Led) Then
                    myControl.Tag = SetTag(New LedProperties(myControl.Name))

                    '--- Аналоговый выход -------------------------------------
                ElseIf typeControl Is GetType(Knob) Then
                    myControl.Tag = SetTag(New KnobProperties(myControl.Name))
                ElseIf typeControl Is GetType(Slide) Then
                    myControl.Tag = SetTag(New SlideProperties(myControl.Name))

                    '--- Аналоговый вход --------------------------------------
                ElseIf typeControl Is GetType(NumericEdit) Then
                    myControl.Tag = SetTag(New NumericEditProperties(myControl.Name))
                ElseIf typeControl Is GetType(Tank) Then
                    myControl.Tag = SetTag(New TankProperties(myControl.Name))
                ElseIf typeControl Is GetType(Gauge) Then
                    myControl.Tag = SetTag(New GaugeProperties(myControl.Name))
                ElseIf typeControl Is GetType(Thermometer) Then
                    myControl.Tag = SetTag(New ThermometerProperties(myControl.Name))
                ElseIf typeControl Is GetType(Meter) Then
                    myControl.Tag = SetTag(New MeterProperties(myControl.Name))

                ElseIf typeControl Is GetType(WaveformGraph) Then
                    myControl.Tag = SetTag(New WaveformGraphProperties(myControl.Name))

                    'ElseIf type__1 Is GetType(NationalInstruments.UI.WaveformPlot) Then
                ElseIf typeControl Is GetType(ScatterGraph) Then
                    myControl.Tag = SetTag(New ScatterGraphProperties(myControl.Name))

                    'ElseIf type__1 Is GetType(NationalInstruments.UI.ScatterPlot) Then
                    'ElseIf type__1 Is GetType(NationalInstruments.UI.XAxis) Then
                    'ElseIf type__1 Is GetType(NationalInstruments.UI.YAxis) Then
                End If

                Flush()
            Else
                ' для всех одинаково
                GetTag(myControl.Tag.ToString)
            End If 'myControl.Tag Is Nothing 
        Catch ex As Exception
            Dim caption As String = $"Элемент {inControl} имеет несовместимую версию свойства тега!"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Получить описание (Description) для перечисления
    ''' </summary>
    ''' <param name="EnumType"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertTo(ByVal EnumType As Type, ByVal value As Object) As String
        Dim fi As Reflection.FieldInfo = EnumType.GetField([Enum].GetName(EnumType, value))
        Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

        If dna IsNot Nothing Then
            Return dna.Description
        Else
            Return value.ToString()
        End If
    End Function

    ''' <summary>
    ''' Восстановить дерево состояния объекта
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Flush()
        ''перезапись xmlDocument
        'можно получить HostSurface из HostControl из текущей tabControl1, но метод Flush не всегда переписывает xmlDocument
        'CType(tabControl1.TabPages(tabControl1.SelectedIndex).Controls(0), HostControl).HostSurface.Loader.Flush()
        'этот вызов надёжнее
        Dim currentHostControl As HostControl = CurrentDocumentsHostControl
        If CurrentActiveDocumentLoaderType = LoaderType.BasicDesignerLoader Then
            DirectCast(currentHostControl.HostSurface.Loader, BasicHostLoader).PerformFlushWorker()
        End If
    End Sub

    ''' <summary>
    ''' Получить Hashtable Из Файла
    ''' Десериализовать XML представление базового класса настроек в Hashtable
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeserializePropertiesControlBaseFromXML(ByVal strFileXML As String) As Object
        ' по пути к файлу десериализовать в Hashtable
        Dim sf As New SoapFormatter() ' Создать soap сериализатор.
        Dim ht As Object 'Hashtable

        ' перезагрузить файл содержимого, используя тот же SoapFormatter объект.
        Using fs As New FileStream(strFileXML, FileMode.Open)
            ht = sf.Deserialize(fs) 'DirectCast(sf.Deserialize(fs), Hashtable)
            '        instance = CType(sf.Deserialize(fs), SerializableClass)
        End Using
        ' удостовериться, что объект был десеарилизован корректно
        'For Each de As DictionaryEntry In ht2
        '    Console.WriteLine("Key={0}  Value={1}", de.Key, de.Value)
        'Next

        Return ht
    End Function

    ''' <summary>
    ''' Сериализовать объект в файл формата XML
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Private Sub SerializePropertiesControlBaseToXML(ByVal strFileXML As String, ByVal instance As Object) ' ByVal ArrayListObject As ArrayList)
        ' на входе ArrayList переводим его в Hashtable со строковым ключом индекса
        ' Создать  Hashtable объект и заполнить какими-то данными
        'Dim ht As New Hashtable()
        'Dim I As Integer
        'For Each obj As Object In ArrayListObject
        '    ht.Add(I.ToString, obj)
        '    I += 1
        'Next

        ' Создать soap сериализатор.
        Dim sf As New SoapFormatter()
        ' Записать Hashtable на диск в SOAP формате.
        Using fs As New FileStream(strFileXML, FileMode.Create)
            sf.Serialize(fs, instance) 'ht)
        End Using
    End Sub
#End Region

#Region "обработчики событий"
#Region "Тип загрузчика"
    Private Sub basicDesignerLoaderMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles basicDesignerLoaderMenuItem.Click
        noLoaderMenuItem.Checked = False
        codeDomDesignerLoaderMenuItem.Checked = False
        basicDesignerLoaderMenuItem.Checked = True

        ' выключить все пункты типов исключая Form
        FileMenuItem.Enabled = True
        ' изменил
        'Me.userControlMenuItem.Enabled = False
        'Me.componentMenuItem.Enabled = False
        'Me.grapherMenuItem.Enabled = False
    End Sub

    Private Sub noLoaderMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles noLoaderMenuItem.Click
        noLoaderMenuItem.Checked = True
        codeDomDesignerLoaderMenuItem.Checked = False
        basicDesignerLoaderMenuItem.Checked = False

        ' включить все пункты типов
        FileMenuItem.Enabled = True
        ' изменил
        'Me.userControlMenuItem.Enabled = True
        'Me.componentMenuItem.Enabled = True
        'Me.grapherMenuItem.Enabled = True
    End Sub

    Private Sub codeDomDesignerLoaderMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles codeDomDesignerLoaderMenuItem.Click
        noLoaderMenuItem.Checked = False
        codeDomDesignerLoaderMenuItem.Checked = True
        basicDesignerLoaderMenuItem.Checked = False

        ' выключить все пункты типов исключая Form
        FileMenuItem.Enabled = True
        ' изменил
        'Me.userControlMenuItem.Enabled = False
        'Me.componentMenuItem.Enabled = False
        'Me.grapherMenuItem.Enabled = False
    End Sub
#End Region

#Region "Язык редактора кода"

    'Private Sub xmlMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles xmlMenuItem.Click
    '    SwitchToCode(Strings.Xml)
    'End Sub

    'Private Sub cMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cMenuItem1.Click
    '    SwitchToCode(Strings.CS)
    'End Sub

    'Private Sub vBMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles vBMenuItem.Click
    '    SwitchToCode(Strings.VB)
    'End Sub

    'Private Sub jMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SwitchToCode(Strings.JS)
    'End Sub

    Private Sub SwitchToCode(ByVal context As String)
        Dim caption As String = String.Empty
        Dim text As String = NameOf(SwitchToCode)

        If CurrentActiveDocumentLoaderType = LoaderType.NoLoader Then
            text = "Просмоторщик кода не поддерживается для документа загруженного без Loaders"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        If context = StringsConst.Xml AndAlso CurrentActiveDocumentLoaderType <> LoaderType.BasicDesignerLoader Then
            text = "Просмоторщик Xml кода поддерживается только для базового загрузчика дизайнера"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        ' изменил
        'If (context = Strings.CS OrElse context = Strings.VB OrElse context = Strings.JS) AndAlso CurrentActiveDocumentLoaderType <> LoaderType.CodeDomDesignerLoader Then
        '    MessageBox.Show(context & " просмоторщик кода поддерживается только для CodeDom загрузчика дизайнера")
        '    Return
        'End If

        If CurrentActiveDocumentLoaderType <> LoaderType.CodeDomDesignerLoader Then
            text = context & " просмоторщик кода поддерживается только для CodeDom загрузчика дизайнера"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        Dim currentHostControl As HostControl = CurrentDocumentsHostControl
        Dim codeEditor As RichTextBox = CurrentDocumentsCodeEditor

        If CurrentActiveDocumentLoaderType = LoaderType.BasicDesignerLoader Then
            codeEditor.Text = DirectCast(currentHostControl.HostSurface.Loader, BasicHostLoader).GetCode()
        ElseIf CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
            codeEditor.Text = DirectCast(currentHostControl.HostSurface.Loader, CodeDomHostLoader).GetCode(context)
        End If

        Dim index As Integer = CurrentDocumentsCodeIndex

        If tabControlDesign.SelectedIndex <> index Then
            prevIndex = tabControlDesign.SelectedIndex
            tabControlDesign.SelectedIndex = index
            curIndex = index
        End If
    End Sub
#End Region

    Private Sub tabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If tabControlDesign.SelectedIndex = -1 Then Exit Sub

        If CurrentDocumentView = StringsConst.Design Then
            ' изменил
            'If CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
            '    Me.eMenuItem.Enabled = True
            'Else
            '    Me.eMenuItem.Enabled = False
            'End If
        Else
            If CurrentActiveDocumentLoaderType = LoaderType.BasicDesignerLoader Then
                SwitchToCode(StringsConst.Xml)
                ' изменил
                'ElseIf CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
                '    SwitchToCode(Strings.CS)
            End If
        End If
    End Sub

#Region "Открыть, записать проект"

    ''' <summary>
    ''' Сохранить проект, если хост загружен используя BasicDesignerLoader
    ''' </summary>
    Private Sub saveMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveMenuItem.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(saveMenuItem_Click)}> Продолжить загрузку используя BasicDesignerLoader")
        If tabControlDesign.TabPages.Count = 0 Then Exit Sub

        Dim caption As String = String.Empty
        Dim text As String = "saveMenuItem_Click"

        If CurrentActiveDocumentLoaderType = LoaderType.NoLoader Then
            text = "Невозможно сохранить документ созданный без загрузчика"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        If CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
            text = "Невозможно сохранить документ созданный с использованием CodeDom загрузчика дизайнера"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        ' привести загрузчик текущего дизайнера к пока единственно поддерживаемому BasicHostLoader и записать в файл
        Dim currentHostControl As HostControl = CurrentDocumentsHostControl

        If CurrentActiveDocumentLoaderType = LoaderType.BasicDesignerLoader Then
            DirectCast(currentHostControl.HostSurface.Loader, BasicHostLoader).PerformFlushWorker() ' или то же Call Flush()
            DirectCast(currentHostControl.HostSurface.Loader, BasicHostLoader).Save()
        End If

        ' изменил
        'Me.OutputWindow.RichTextBox.Text += "Сохранен host." & vbLf
        ShowMessageOnStatusPanel("Файл проекта сохранён")
    End Sub

    ''' <summary>
    ''' Открыть xml файл проекта сохраненный ранее в gПутьПанелиМоториста
    ''' </summary>
    Private Sub openMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles openMenuItem.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(openMenuItem_Click)}> Открыть xml файл сохраненный ранее")

        Try
            Dim fileName As String

            ' Открыть диалог
            Dim dlg As New OpenFileDialog() With {.DefaultExt = "xml",
                                                  .Filter = "Xml Files|*.xml",
                                                  .InitialDirectory = PathPanelMotorist}
            If dlg.ShowDialog() = DialogResult.OK Then
                fileName = dlg.FileName
            Else
                fileName = Nothing
            End If

            If fileName Is Nothing Then
                Return
            End If

            ' создать Form
            formCount += 1
            Dim hc As HostControl = HostSurfaceManagerFuncPanel.GetNewHost(fileName)
            Toolbox.DesignerHost = hc.DesignerHost
            Dim tabpage As New TabPage(String.Format("Form{0} - {1}", formCount, StringsConst.Design))

            If fileName.EndsWith("xml") Then
                tabpage.Tag = LoaderType.BasicDesignerLoader
                'ElseIf fileName.EndsWith("cs") OrElse fileName.EndsWith("vb") Then
                '    tabpage.Tag = LoaderType.CodeDomDesignerLoader
            End If

            hc.Parent = tabpage
            hc.Dock = DockStyle.Fill
            tabControlDesign.TabPages.Add(tabpage)
            tabControlDesign.SelectedIndex = tabControlDesign.TabPages.Count - 1

            ' изменил
            'Me.OutputWindow.RichTextBox.Text += "Открыт новый host." & vbLf
            ShowMessageOnStatusPanel("Открыт новый файл проекта")
            EnabledToolAndMenu()
        Catch
            Const caption As String = "openMenuItem_Click Сообщение"
            Const text As String = "Ошибка в создании нового хоста"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    ''' <summary>
    '''  Сохранить проект в новом файле, если хост загружен используя BasicDesignerLoader
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub saveAsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveAsMenuItem.Click
        If tabControlDesign.SelectedIndex = -1 Then Exit Sub

        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(saveAsMenuItem_Click)}> Сохранить xml файл")

        Dim caption As String = String.Empty
        Dim text As String = "saveAsMenuItem_Click"

        If CurrentActiveDocumentLoaderType = LoaderType.NoLoader Then
            text = "Невозможно сохранить документ созданный без загрузчика"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        If CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
            text = "Невозможно сохранить документ созданный с использованием CodeDom загрузчика дизайнера"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, text))
            Return
        End If

        Dim currentHostControl As HostControl = CurrentDocumentsHostControl
        DirectCast(currentHostControl.HostSurface.Loader, BasicHostLoader).Save(True)
    End Sub

    'Private Sub OpenRunMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenRunMenuItem.Click
    '    Try
    '        Dim fileName As String = Nothing

    '        ' Open File Dialog
    '        Dim dlg As New OpenFileDialog()
    '        dlg.DefaultExt = "xml"
    '        dlg.Filter = "Xml Files|*.xml"
    '        If dlg.ShowDialog() = DialogResult.OK Then
    '            fileName = dlg.FileName
    '        End If

    '        If fileName Is Nothing Then
    '            Return
    '        End If

    '        'If myFormCopy IsNot Nothing Then
    '        '    myFormCopy.Close()
    '        '    myFormCopy.Dispose()
    '        'End If
    '        'myFormCopy = New PanelBaseForm ' System.Windows.Forms.Form
    '        'Dim myBasicHostLoaderRun As New BasicHostLoaderRun(fileName)
    '        'myBasicHostLoaderRun.PerformLoad2()
    '        'myFormCopy.Show()

    '        FormsPanelManager.NewForm(fileName)
    '    Catch
    '        MessageBox.Show("Ошибка в создании нового хоста", "Shell Сообщение", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '    End Try
    'End Sub
#End Region

    'изменил
    '''' <summary>
    '''' Если host был загружен используя CodeDomDesignerLoader значит можго выполнить его
    '''' </summary>
    'Private Sub runMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles runMenuItem.Click
    '    If CurrentActiveDocumentLoaderType = LoaderType.NoLoader OrElse CurrentActiveDocumentLoaderType = LoaderType.BasicDesignerLoader Then
    '        MessageBox.Show("Невозможно запустить документ созданный без загрузки или создания используя Базовый Загрузчик Дизайнера. Для запуска документа используйте CodeDom загрузчик дизайнера")
    '        Return
    '    End If

    '    Dim currentHostControl As HostControl = CurrentDocumentsHostControl
    '    DirectCast(currentHostControl.HostSurface.Loader, CodeDomHostLoader).Run()
    'End Sub

#Region "Добавить новый проект"
    Private Sub AddTabForNewHost(ByVal tabText As String, ByVal hc As HostControl)
        Toolbox.DesignerHost = hc.DesignerHost
        Dim tabpage As New TabPage(tabText) With {.Tag = CurrentMenuSelectionLoaderType}
        hc.Parent = tabpage
        hc.Dock = DockStyle.Fill
        tabControlDesign.TabPages.Add(tabpage)
        tabControlDesign.SelectedIndex = tabControlDesign.TabPages.Count - 1
        HostSurfaceManagerFuncPanel.ActiveDesignSurface = hc.HostSurface

        ' изменил
        'If CurrentActiveDocumentLoaderType = LoaderType.CodeDomDesignerLoader Then
        '    Me.eMenuItem.Enabled = True
        'Else
        '    Me.eMenuItem.Enabled = False
        'End If
        'Me.solutionExplorer1.AddFileNode(tabText)
    End Sub

    Private Sub formMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles formMenuItem.Click
        Try
            formCount += 1
            Dim hc As HostControl = HostSurfaceManagerFuncPanel.GetNewHost(GetType(Form), CurrentMenuSelectionLoaderType)
            AddTabForNewHost(String.Format("Form{0} - {1}", formCount, StringsConst.Design), hc)

            PerformAction("&Select All") ' блуд для того, чтобы при щелчке по пустой форме отразились её свойства в сетке
            EnabledToolAndMenu()
        Catch
            Const caption As String = "formMenuItem_Click Сообщение"
            Const text As String = "Ошибка в создании нового хоста"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    'изменил
    'Private Sub userControlMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles userControlMenuItem.Click
    '    Try
    '        _userControlCount += 1
    '        Dim hc As HostControl = _hostSurfaceManager.GetNewHost(GetType(UserControl), CurrentMenuSelectionLoaderType)
    '        AddTabForNewHost("UserControl" & _userControlCount.ToString() & " - " & Strings.Design, hc)
    '    Catch
    '        MessageBox.Show("Ошибка в создании нового хоста", "Shell Сообщение", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '    End Try
    'End Sub

    'Private Sub componentMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles componentMenuItem.Click
    '    Try
    '        _componentCount += 1
    '        Dim hc As HostControl = _hostSurfaceManager.GetNewHost(GetType(Component), CurrentMenuSelectionLoaderType)
    '        AddTabForNewHost("Component" & _componentCount.ToString() & " - " & Strings.Design, hc)
    '    Catch
    '        MessageBox.Show("Ошибка в создании нового хоста", "Shell Сообщение", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '    End Try
    'End Sub

    'Private Sub grapherMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles grapherMenuItem.Click
    '    Try
    '        _grapherCount += 1
    '        Dim hc As HostControl = _hostSurfaceManager.GetNewHost(GetType(MyTopLevelComponent), CurrentMenuSelectionLoaderType)
    '        AddTabForNewHost("CustomDesigner" & _grapherCount.ToString() & " - " & Strings.Design, hc)
    '    Catch
    '        MessageBox.Show("Ошибка в создании нового хоста", "Shell Сообщение", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '    End Try
    'End Sub
#End Region

#Region "Обработка действий пользователя при редактировании"

    ''' <summary>
    ''' Выполнить все опции в меню Edit используя MenuCommandService
    ''' </summary>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub PerformAction(ByVal text As String)
        If CurrentDocumentView = StringsConst.Code Then
            Const caption As String = "PerformAction"
            Const _text As String = "Это не поддерживается в просмоторщике кода."
            MessageBox.Show(_text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", caption, _text))
            Return
        End If

        If CurrentDocumentsHostControl Is Nothing Then
            Return
        End If

        Dim ims As IMenuCommandService = TryCast(CurrentDocumentsHostControl.HostSurface.GetService(GetType(IMenuCommandService)), IMenuCommandService)
        '--- не работают ---
        'AlignToGrid
        'ArrangeBottom
        'ArrangeIcons
        'ArrangeRight
        'BringForward
        'DocumentOutline
        'F1Help
        'Group
        'LineupIcons
        'LockControls
        'MultiLevelRedo
        'MultiLevelUndo
        'Properties
        'PropertiesWindow
        'Replace
        'SendBackward
        'ShowGrid
        'ShowLargeIcons
        'SizeToFit
        'SizeToGrid
        'SnapToGrid
        'Ungroup
        'VerbFirst
        'VerbLast
        'ViewCode
        'ViewGrid

        Try
            Select Case text
                '--- не работают ---
                'Case "&Cut" 
                '    ims.GlobalInvoke(StandardCommands.Cut)
                '    Exit Select
                'Case "C&opy"
                '    ims.GlobalInvoke(StandardCommands.Copy)
                '    Exit Select
                'Case "&Paste" 
                '    ims.GlobalInvoke(StandardCommands.Paste)
                '    Exit Select
                'Case "Undo" 
                '    ims.GlobalInvoke(StandardCommands.Undo)
                '    Exit Select
                'Case "Redo" 
                '    ims.GlobalInvoke(StandardCommands.Redo)
                '    Exit Select

                Case "Lefts"
                    ims.GlobalInvoke(StandardCommands.AlignLeft)
                    Exit Select
                Case "Centers"
                    ims.GlobalInvoke(StandardCommands.AlignHorizontalCenters)
                    Exit Select
                Case "Rights"
                    ims.GlobalInvoke(StandardCommands.AlignRight)
                    Exit Select
                Case "Tops"
                    ims.GlobalInvoke(StandardCommands.AlignTop)
                    Exit Select
                Case "Middles"
                    ims.GlobalInvoke(StandardCommands.AlignVerticalCenters)
                    Exit Select
                Case "Bottoms"
                    ims.GlobalInvoke(StandardCommands.AlignBottom)
                    Exit Select

                Case "SizeToControl"
                    ims.GlobalInvoke(StandardCommands.SizeToControl)
                    Exit Select
                Case "SizeToControlHeight"
                    ims.GlobalInvoke(StandardCommands.SizeToControlHeight)
                    Exit Select
                Case "SizeToControlWidth"
                    ims.GlobalInvoke(StandardCommands.SizeToControlWidth)
                    Exit Select

                Case "HorizSpaceConcatenate"
                    ims.GlobalInvoke(StandardCommands.HorizSpaceConcatenate)
                    Exit Select
                Case "HorizSpaceDecrease"
                    ims.GlobalInvoke(StandardCommands.HorizSpaceDecrease)
                    Exit Select
                Case "HorizSpaceIncrease"
                    ims.GlobalInvoke(StandardCommands.HorizSpaceIncrease)
                    Exit Select
                Case "HorizSpaceMakeEqual"
                    ims.GlobalInvoke(StandardCommands.HorizSpaceMakeEqual)
                    Exit Select

                Case "VertSpaceConcatenate"
                    ims.GlobalInvoke(StandardCommands.VertSpaceConcatenate)
                    Exit Select
                Case "VertSpaceDecrease"
                    ims.GlobalInvoke(StandardCommands.VertSpaceDecrease)
                    Exit Select
                Case "VertSpaceIncrease"
                    ims.GlobalInvoke(StandardCommands.VertSpaceIncrease)
                    Exit Select
                Case "VertSpaceMakeEqual"
                    ims.GlobalInvoke(StandardCommands.VertSpaceMakeEqual)
                    Exit Select

                Case "CenterHorizontally"
                    ims.GlobalInvoke(StandardCommands.CenterHorizontally)
                    Exit Select
                Case "CenterVertically"
                    ims.GlobalInvoke(StandardCommands.CenterVertically)
                    Exit Select

                Case "BringToFront"
                    ims.GlobalInvoke(StandardCommands.BringToFront)
                    Exit Select
                Case "SendToBack"
                    ims.GlobalInvoke(StandardCommands.SendToBack)
                    Exit Select

                Case "TabOrder"
                    ims.GlobalInvoke(StandardCommands.TabOrder)
                    Exit Select

                Case "Select All"
                    ims.GlobalInvoke(StandardCommands.SelectAll)
                    Exit Select
                Case "Delete"
                    ims.GlobalInvoke(StandardCommands.Delete)
                    Exit Select

                Case Else
                    Exit Select
            End Select
        Catch
            Const caption As String = "formMenuItem_Click Сообщение"
            Dim Msgtext As String = "Ошибка в выполнении действия: " & text.Replace("&", "")
            MessageBox.Show(Msgtext, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
            ' изменил
            'Me.OutputWindow.RichTextBox.Text += "Ошибка в выполнении действия: " & text.Replace("&", "")
            ShowMessageOnStatusPanel(Msgtext)
        End Try
    End Sub

    ''' <summary>
    ''' Включить или выключить последовательность переходов
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabOrderToolStripButton_Click(sender As Object, e As EventArgs) Handles TabOrderToolStripButton.Click
        ToggleTabOrder()
    End Sub

    Private Sub TabOrderMenuItem_Click(sender As Object, e As EventArgs) Handles TabOrderMenuItem.Click
        ToggleTabOrder()
    End Sub

    Private Sub ToggleTabOrder()
        'Сначала переключить состояние отметки для соответствующего пункта меню
        TabOrderMenuItem.Checked = Not TabOrderMenuItem.Checked

        'Синхронизировать кнопку "Последовательность переходов" на панели инструментов
        TabOrderToolStripButton.Checked = TabOrderMenuItem.Checked
        ActionClick(TabOrderMenuItem, Nothing)
    End Sub

    Private Sub ActionClick(ByVal sender As Object, ByVal e As EventArgs) Handles LeftsMenuItem.Click,
            MiddlesMenuItem.Click,
            RightsMenuItem.Click,
            TopsMenuItem.Click,
            CentersMenuItem.Click,
            BottomsMenuItem.Click,
            SizeToControlWidthMenuItem.Click,
            SizeToControlHeightMenuItem.Click,
            SizeToControlMenuItem.Click,
            HorizSpaceMakeEqualMenuItem.Click,
            HorizSpaceIncreaseMenuItem.Click,
            HorizSpaceDecreaseMenuItem.Click,
            HorizSpaceConcatenateMenuItem.Click,
            VertSpaceMakeEqualMenuItem.Click,
            VertSpaceIncreaseMenuItem.Click,
            VertSpaceDecreaseMenuItem.Click,
            VertSpaceConcatenateMenuItem.Click,
            CenterHorizontallyMenuItem.Click,
            CenterVerticallyMenuItem.Click,
            BringToFrontMenuItem.Click,
            SendToBackMenuItem.Click,
            SelectAllMenuItem.Click,
            DeletSelectMenuItem.Click,
            LeftsToolStripButton.Click,
            MiddlesToolStripButton.Click,
            RightsToolStripButton.Click,
            TopsToolStripButton.Click,
            CentersToolStripButton.Click,
            BottomsToolStripButton.Click,
            SizeToControlWidthToolStripButton.Click,
            SizeToControlHeightToolStripButton.Click,
            SizeToControlToolStripButton.Click,
            HorizSpaceMakeEqualToolStripButton.Click,
            HorizSpaceConcatenateToolStripButton.Click,
            VertSpaceMakeEqualToolStripButton.Click,
            VertSpaceConcatenateToolStripButton.Click,
            BringToFrontToolStripButton.Click,
            SendToBackToolStripButton.Click

        If tabControlDesign.SelectedIndex <> -1 Then
            'ToolStripButton-> Inherits ToolStripItem
            'ToolStripMenuItem -> Inherits ToolStripDropDownItem ->Inherits ToolStripItem
            'PerformAction(TryCast(sender, ToolStripMenuItem).Tag)
            PerformAction(TryCast(sender, ToolStripItem).Tag.ToString)
        End If
    End Sub
#End Region

    Private Sub DeleteCurrentTabPage_Click(sender As Object, e As EventArgs) Handles TSMenuItemContexУдалитьТекущуюВкладку.Click, TSMenuItemУдалитьТекущуюВкладку.Click
        Const CAPTION As String = "Внимание!"
        Const MESSAGE As String = "Произвести сохранение произведённых изменений?"
        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult = MessageBox.Show(MESSAGE, CAPTION, Buttons, MessageBoxIcon.Question)

        If Result = DialogResult.Yes Then saveMenuItem_Click(saveMenuItem, Nothing) 'записать

        DeleteCurrentTabPage()
    End Sub

    ''' <summary>
    ''' Удалить Текущуу Вкладку
    ''' </summary>
    Private Sub DeleteCurrentTabPage()
        If tabControlDesign.SelectedIndex <> -1 Then tabControlDesign.TabPages.RemoveAt(tabControlDesign.SelectedIndex)

        EnabledToolAndMenu()
    End Sub

    Private Sub physicalPortComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles physicalPortComboBox.SelectedIndexChanged
        If IsHandleCreated AndAlso physicalPortComboBox.SelectedIndex <> -1 Then
            ComboBoxLineCount.SelectedIndex = physicalPortComboBox.SelectedIndex
        End If
    End Sub

    ''' <summary>
    ''' Нужно добавить обработчик события PropertyGrid – PropertyValueChanged для свойства, 
    ''' видимость которого зависит от другого свойства 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PropertyGrid2_PropertyValueChanged(ByVal s As Object, ByVal e As PropertyValueChangedEventArgs) Handles PropertyGrid2.PropertyValueChanged
        PropertyGrid2.Refresh()
    End Sub

#End Region

    Public Sub ShowMessageOnStatusPanel(ByVal strMessage As String)
        tsStatusLabel.Text = strMessage
    End Sub

    'Private Sub ОчиститьПанель()
    '    ToolStripStatusLabel.Text = ""
    'End Sub

    Private Sub ToolStripMenuItemRegExp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemRegExp.Click
        Dim frmRegularExpressions As New frmRegularExpressionsReplace()

        frmRegularExpressions.Init("", Nothing)
        frmRegularExpressions.ShowDialog(Me)
    End Sub

    Private Sub FileMenuItem_DropDownOpening(sender As Object, e As EventArgs) Handles FileMenuItem.DropDownOpening
        Dim tabNoSelectedIndex As Boolean = tabControlDesign.SelectedIndex = -1

        ToolStripMenuItemRegExp.Enabled = tabNoSelectedIndex
        TSMenuItemУдалитьТекущуюВкладку.Enabled = Not tabNoSelectedIndex
        saveMenuItem.Enabled = Not tabNoSelectedIndex
        saveAsMenuItem.Enabled = Not tabNoSelectedIndex
    End Sub

    ''' <summary>
    ''' Доступность редактирования зависит от выделенной вкладки
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnabledToolAndMenu()
        Dim mEnabled As Boolean = Not tabControlDesign.SelectedIndex = -1
        ToolStripEditor.Enabled = mEnabled
        FormatMenuItem.Enabled = mEnabled
        EditMenuItem.Enabled = mEnabled
    End Sub

End Class
