Imports System.ComponentModel
Imports System.Xml
Imports NationalInstruments.UI.WindowsForms

''' <summary>
''' Вспомогательный класс настройки панели инструментов.
''' для контрола Toolbox при его настройки в режиме дизайнера
''' </summary>
''' <remarks></remarks>
Friend Class ToolboxUIManagerVS
    Private m_toolbox As Toolbox
    Private pointer As System.Drawing.Design.ToolboxItem

    Public Sub New(ByVal toolbox As Toolbox)
        m_toolbox = toolbox
        pointer = New System.Drawing.Design.ToolboxItem() With {.DisplayName = "<Pointer>",
                                                                .Bitmap = New Bitmap(16, 16)}
    End Sub

    ''' <summary>
    ''' Панель элементов, которую настраивают
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property Toolbox() As Toolbox
        Get
            Return m_toolbox
        End Get
    End Property

    ''' <summary>
    ''' Создать и заполнить панель инструментов
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillToolbox()
        CreateControls()
        ConfigureControls()
        UpdateToolboxItems(Toolbox.Tabs.Count - 1)
    End Sub

    ''' <summary>
    ''' Создать панель
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateControls()
        Toolbox.Controls.Clear()
        Toolbox.ToolsListBox = New ListBox()
        Toolbox.TabPageArray = New Button(Toolbox.Tabs.Count - 1) {}
    End Sub

    ''' <summary>
    ''' Добавить кнопки на пенель и в массив
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigureControls()
        Toolbox.SuspendLayout()
        For i As Integer = Toolbox.Tabs.Count - 1 To 0 Step -1
            ' 
            ' Tab Button
            ' 
            Dim button As New Button()

            button.Dock = DockStyle.Top

            button.FlatStyle = FlatStyle.Popup ' Standard
            button.BackColor = Color.LightSteelBlue ' System.Drawing.SystemColors.InactiveCaptionText
            'button.ForeColor = Color.White

            button.Location = New Point(0, (i + 1) * 20)
            button.Name = Toolbox.Tabs(i).Name
            button.Size = New Size(Toolbox.Width, 20)
            button.TabIndex = i + 1
            button.Text = Toolbox.Tabs(i).Name
            button.UseVisualStyleBackColor = True 'добавил

            button.TextAlign = ContentAlignment.MiddleLeft
            button.Tag = i
            AddHandler button.Click, New EventHandler(AddressOf button_Click)

            Toolbox.Controls.Add(button)
            Toolbox.TabPageArray(i) = button
        Next
        ' 
        ' toolboxTitleButton
        ' 
        Dim toolboxTitleButton As New Button()

        toolboxTitleButton.BackColor = SystemColors.ActiveCaption
        toolboxTitleButton.Dock = DockStyle.Top
        toolboxTitleButton.FlatStyle = FlatStyle.Popup
        toolboxTitleButton.ForeColor = SystemColors.ActiveCaptionText
        toolboxTitleButton.Location = New Point(0, 0)
        toolboxTitleButton.Name = "toolboxTitleButton"
        toolboxTitleButton.Size = New Size(Toolbox.Width, 20)
        toolboxTitleButton.TabIndex = 0
        toolboxTitleButton.Text = "Инструменты"
        toolboxTitleButton.TextAlign = ContentAlignment.MiddleLeft
        Toolbox.Controls.Add(toolboxTitleButton)
        ' 
        ' listBox
        ' 
        Dim listBox As New ListBox()

        listBox.BackColor = SystemColors.ControlLight
        listBox.DrawMode = DrawMode.OwnerDrawFixed
        listBox.ItemHeight = 18
        listBox.Location = New Point(0, (Toolbox.Tabs.Count + 1) * 20)
        listBox.Name = "ToolsListBox"
        listBox.Size = New Size(Toolbox.Width, Toolbox.Height - (Toolbox.Tabs.Count + 1) * 20)
        listBox.TabIndex = Toolbox.Tabs.Count + 1

        Toolbox.Controls.Add(listBox)
        UpdateToolboxItems(Toolbox.Tabs.Count - 1)

        Toolbox.ResumeLayout()
        Toolbox.ToolsListBox = listBox
        AddHandler Toolbox.SizeChanged, New EventHandler(AddressOf Toolbox_SizeChanged)
    End Sub

    ''' <summary>
    ''' Заполнить вспомогательные коллекции ToolsListBox
    ''' для кнопок выбора элементов на холст 
    ''' в зависимости от вкладки.
    ''' </summary>
    ''' <param name="tabIndex"></param>
    ''' <remarks></remarks>
    Private Sub UpdateToolboxItems(ByVal tabIndex As Integer)
        ' при звдержки мыши связать лист с вкладкой
        Toolbox.ToolsListBox.Tag = Toolbox.Tabs(tabIndex).Name

        Toolbox.ToolsListBox.Items.Clear()
        Toolbox.ToolsListBox.Items.Add(pointer)
        If Toolbox.Tabs.Count <= 0 Then
            Return
        End If

        Dim toolboxTab As ToolboxTab = Toolbox.Tabs(tabIndex)
        Dim toolboxItems As ToolboxItemCollection = toolboxTab.ToolboxItems

        For Each toolboxItem As ToolboxItem In toolboxItems
            Dim type As Type = toolboxItem.Type
            Dim tbi As New System.Drawing.Design.ToolboxItem(type)
            Dim tba As ToolboxBitmapAttribute = TryCast(TypeDescriptor.GetAttributes(type)(GetType(ToolboxBitmapAttribute)), ToolboxBitmapAttribute)

            If tba IsNot Nothing Then
                tbi.Bitmap = DirectCast(tba.GetImage(type), Bitmap)
            End If

            Toolbox.ToolsListBox.Items.Add(tbi)
        Next
    End Sub

    ''' <summary>
    ''' Обработчик события раскрытия панели листа с пунктами выбора элементов
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub button_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim button As Button = TryCast(sender, Button)

        If button Is Nothing Then
            Return
        End If

        Dim index As Integer = CInt(button.Tag)

        If button.Dock = DockStyle.Top Then
            ' всё, что идёт вслед index сдвигаем вниз
            For i As Integer = index + 1 To Toolbox.TabPageArray.Length - 1
                Toolbox.TabPageArray(i).Dock = DockStyle.Bottom
            Next
        Else
            ' всё, что до index сдвигаем вверх
            For i As Integer = 0 To index
                Toolbox.TabPageArray(i).Dock = DockStyle.Top
            Next
        End If

        Toolbox.ToolsListBox.Location = New Point(0, (index + 2) * 20)
        UpdateToolboxItems(index)
    End Sub

    Private Sub Toolbox_SizeChanged(ByVal sender As Object, ByVal e As EventArgs)
        Toolbox.ToolsListBox.Size = New Size(Toolbox.Width, Toolbox.Height - (Toolbox.Tabs.Count + 1) * 20)
    End Sub
End Class


''' <summary>
''' Наполнение toolbox из массивов или при считывание XML файла
''' </summary>
''' <remarks></remarks>
Friend Class ToolboxXmlManager
    Private m_toolbox As Toolbox = Nothing
    'Private ActiveControlslTypes As Type() = New Type() {GetType(NationalInstruments.UI.WindowsForms.WaveformGraph), GetType(System.Windows.Forms.PropertyGrid), GetType(System.Windows.Forms.Label), GetType(System.Windows.Forms.LinkLabel), GetType(System.Windows.Forms.Button), GetType(System.Windows.Forms.TextBox), _
    ' GetType(System.Windows.Forms.CheckBox), GetType(System.Windows.Forms.RadioButton), GetType(System.Windows.Forms.GroupBox), GetType(System.Windows.Forms.PictureBox), GetType(System.Windows.Forms.Panel), GetType(System.Windows.Forms.DataGrid), _
    ' GetType(System.Windows.Forms.ListBox), GetType(System.Windows.Forms.CheckedListBox), GetType(System.Windows.Forms.ComboBox), GetType(System.Windows.Forms.ListView), GetType(System.Windows.Forms.TreeView), GetType(System.Windows.Forms.TabControl), _
    ' GetType(System.Windows.Forms.DateTimePicker), GetType(System.Windows.Forms.MonthCalendar), GetType(System.Windows.Forms.HScrollBar), GetType(System.Windows.Forms.VScrollBar), GetType(System.Windows.Forms.Timer), GetType(System.Windows.Forms.Splitter), _
    ' GetType(System.Windows.Forms.DomainUpDown), GetType(System.Windows.Forms.NumericUpDown), GetType(System.Windows.Forms.TrackBar), GetType(System.Windows.Forms.ProgressBar), GetType(System.Windows.Forms.RichTextBox), GetType(System.Windows.Forms.ImageList), _
    ' GetType(System.Windows.Forms.HelpProvider), GetType(System.Windows.Forms.ToolTip), GetType(System.Windows.Forms.ToolBar), GetType(System.Windows.Forms.StatusBar), GetType(System.Windows.Forms.UserControl), GetType(System.Windows.Forms.NotifyIcon), _
    ' GetType(System.Windows.Forms.OpenFileDialog), GetType(System.Windows.Forms.SaveFileDialog), GetType(System.Windows.Forms.FontDialog), GetType(System.Windows.Forms.ColorDialog), GetType(System.Windows.Forms.PrintDialog), GetType(System.Windows.Forms.PrintPreviewDialog), _
    ' GetType(System.Windows.Forms.PrintPreviewControl), GetType(System.Windows.Forms.ErrorProvider), GetType(System.Drawing.Printing.PrintDocument), GetType(System.Windows.Forms.PageSetupDialog)}
    'Private WindowsFormsPanelsTypes As Type() = New Type() {GetType(System.IO.FileSystemWatcher), GetType(System.Diagnostics.Process), GetType(System.Timers.Timer)}
    'Private ControlsSlideTypes As Type() = New Type() {GetType(System.Data.OleDb.OleDbCommandBuilder), GetType(System.Data.OleDb.OleDbConnection), GetType(System.Data.SqlClient.SqlCommandBuilder), GetType(System.Data.SqlClient.SqlConnection)}
    'Private userControlsToolTypes As Type() = New Type() {GetType(System.Windows.Forms.UserControl)}
    'GetType(System.Windows.Forms.SplitContainer),

    'NationalInstruments.UI.WaveformPlot
    'NationalInstruments.UI.XAxis
    'NationalInstruments.UI.YAxis
    'NationalInstruments.UI.ScatterPlot
    'NationalInstruments.UI.XAxis
    'NationalInstruments.UI.YAxis

    Private ActiveControlslTypes As Type() = New Type() {GetType(Label),
                                                        GetType(NumericEdit),
                                                        GetType(NationalInstruments.UI.WindowsForms.Switch),
                                                        GetType(Led)} ', GetType(System.Windows.Forms.NumericUpDown)}

    'Private LinkingControlTypes As Type() = New Type() {GetType(TextBox),
    '                                            GetType(Button),
    '                                            GetType(CheckBox),
    '                                            GetType(RadioButton)} ', GetType(System.Windows.Forms.NumericUpDown)}

    Private WindowsFormsPanelsTypes As Type() = New Type() {GetType(Panel),
                                                            GetType(GroupBox),
                                                            GetType(TabControl),
                                                            GetType(TableLayoutPanel),
                                                            GetType(FlowLayoutPanel),
                                                            GetType(PictureBox)} ', GetType(Microsoft.VisualBasic.PowerPacks.ShapeContainer), GetType(Microsoft.VisualBasic.PowerPacks.RectangleShape), GetType(Microsoft.VisualBasic.PowerPacks.OvalShape), GetType(Microsoft.VisualBasic.PowerPacks.LineShape)}

    Private ControlsSlideTypes As Type() = New Type() {GetType(Tank),
                                                        GetType(Gauge),
                                                        GetType(Thermometer),
                                                        GetType(Meter)}

    Private AnalogOutputTypes As Type() = New Type() {GetType(Slide)}

    Private FrequencyTypes As Type() = New Type() {GetType(Knob)}

    Private ControlsGraphTypes As Type() = New Type() {GetType(WaveformGraph),
                                                        GetType(ScatterGraph)}

    Private ReadOnly Property Toolbox() As Toolbox
        Get
            Return m_toolbox
        End Get
    End Property

    Public Sub New(ByVal toolbox As Toolbox)
        m_toolbox = toolbox
    End Sub

    ''' <summary>
    ''' Заполнить панель инструментов
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulateToolboxInfo() As ToolboxTabCollection
        Try
            If Toolbox.FilePath Is Nothing OrElse Toolbox.FilePath = "" OrElse Toolbox.FilePath = String.Empty Then
                Return PopulateToolboxTabs()
            End If

            Dim xmlDocument As New XmlDocument()
            xmlDocument.Load(Toolbox.FilePath)
            Return PopulateToolboxTabs(xmlDocument)
        Catch ex As Exception
            Const caption As String = "PopulateToolboxInfo"
            Dim text As String = String.Format("Ошибка вызвана в Toolbox.xml файле.{0}{1}", vbLf, ex)
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Заполнить панель инструментов вкладками в RunTime
    ''' Перегруженный метод.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateToolboxTabs() As ToolboxTabCollection
        Dim toolboxTabs As New ToolboxTabCollection()
        Dim tabNames As String() = {ToolboxControlStrings.сControlsGraphTypes,
                                    ToolboxControlStrings.сWindowsFormsPanelsTypes,
                                    ToolboxControlStrings.сControlsSlideTypes,
                                    ToolboxControlStrings.сFrequencyTypes,
                                    ToolboxControlStrings.сAnalogOutputTypes,
                                    ToolboxControlStrings.сActiveControlslTypes}

        For i As Integer = 0 To tabNames.Length - 1
            Dim toolboxTab As New ToolboxTab() With {.Name = tabNames(i)}

            PopulateToolboxItems(toolboxTab)
            toolboxTabs.Add(toolboxTab)
        Next

        Return toolboxTabs
    End Function

    ''' <summary>
    ''' Заполнить вкладки панели инструментов элементами ToolboxItem
    ''' Перегруженный метод.
    ''' </summary>
    ''' <param name="toolboxTab"></param>
    ''' <remarks></remarks>
    Private Sub PopulateToolboxItems(ByVal toolboxTab As ToolboxTab) 'перегруженная
        If toolboxTab Is Nothing Then
            Return
        End If

        Dim stringHelpAll As String = String.Empty ' строка подсказки
        Dim typeArray As Type() = Nothing

        Select Case toolboxTab.Name
            Case ToolboxControlStrings.сActiveControlslTypes
                typeArray = ActiveControlslTypes
                Exit Select
            'Case Strings.сLinkingControlTypes
            '    typeArray = LinkingControlTypes
            '    Exit Select
            Case ToolboxControlStrings.сWindowsFormsPanelsTypes
                typeArray = WindowsFormsPanelsTypes
                Exit Select
            Case ToolboxControlStrings.сControlsSlideTypes
                typeArray = ControlsSlideTypes
                Exit Select
            Case ToolboxControlStrings.сAnalogOutputTypes
                typeArray = AnalogOutputTypes
                Exit Select
            Case ToolboxControlStrings.сFrequencyTypes
                typeArray = FrequencyTypes
                Exit Select
            Case ToolboxControlStrings.сControlsGraphTypes
                typeArray = ControlsGraphTypes
                Exit Select
            Case Else
                Exit Select
        End Select

        Dim toolboxItems As New ToolboxItemCollection()

        For i As Integer = 0 To typeArray.Length - 1
            Dim toolboxItem As New ToolboxItem() With {.Type = typeArray(i), .Name = typeArray(i).Name}
            toolboxItems.Add(toolboxItem)

            stringHelpAll += GetStringHelpByType(typeArray(i)) & " "
        Next
        Toolbox.HelpToolboxItemDictionary.Add(toolboxTab.Name, stringHelpAll)

        toolboxTab.ToolboxItems = toolboxItems
    End Sub

#Region "Заполнить панель инструментов в из XmlDocument"
    Private Function PopulateToolboxTabs(ByVal xmlDocument As XmlDocument) As ToolboxTabCollection 'перегруженная
        If xmlDocument Is Nothing Then
            Return Nothing
        End If

        Dim toolboxNode As XmlNode = xmlDocument.FirstChild
        If toolboxNode Is Nothing Then
            Return Nothing
        End If

        Dim tabCollectionNode As XmlNode = toolboxNode.FirstChild
        If tabCollectionNode Is Nothing Then
            Return Nothing
        End If

        Dim tabsNodeList As XmlNodeList = tabCollectionNode.ChildNodes
        If tabsNodeList Is Nothing Then
            Return Nothing
        End If

        Dim toolboxTabs As New ToolboxTabCollection()

        For Each tabNode As XmlNode In tabsNodeList
            If tabNode Is Nothing Then
                Continue For
            End If

            Dim propertiesNode As XmlNode = tabNode.FirstChild
            If propertiesNode Is Nothing Then
                Continue For
            End If

            Dim nameNode As XmlNode = propertiesNode(ToolboxControlStrings.Name)
            If nameNode Is Nothing Then
                Continue For
            End If

            Dim toolboxTab As New ToolboxTab() With {.Name = nameNode.InnerXml.ToString()}
            ' заполнить вкладку элементами
            PopulateToolboxItems(tabNode, toolboxTab)
            ' в конце добавить вкладку
            toolboxTabs.Add(toolboxTab)
        Next
        If toolboxTabs.Count = 0 Then
            Return Nothing
        End If

        Return toolboxTabs
    End Function

    ''' <summary>
    ''' Заполнить вкладки панели инструментов элементами ToolboxItem.
    ''' Перегруженный метод.
    ''' </summary>
    ''' <param name="tabNode"></param>
    ''' <param name="toolboxTab"></param>
    ''' <remarks></remarks>
    Private Sub PopulateToolboxItems(ByVal tabNode As XmlNode, ByVal toolboxTab As ToolboxTab) 'перегруженная
        If tabNode Is Nothing Then
            Return
        End If

        Dim toolboxItemCollectionNode As XmlNode = tabNode(ToolboxControlStrings.ToolboxItemCollection)
        If toolboxItemCollectionNode Is Nothing Then
            Return
        End If

        Dim toolboxItemNodeList As XmlNodeList = toolboxItemCollectionNode.ChildNodes
        If toolboxItemNodeList Is Nothing Then
            Return
        End If

        Dim toolboxItems As New ToolboxItemCollection()

        For Each toolboxItemNode As XmlNode In toolboxItemNodeList
            If toolboxItemNode Is Nothing Then
                Continue For
            End If

            Dim typeNode As XmlNode = toolboxItemNode(ToolboxControlStrings.Type)
            If typeNode Is Nothing Then
                Continue For
            End If

            Dim found As Boolean = False
            Dim loadedAssemblies As System.Reflection.Assembly() = AppDomain.CurrentDomain.GetAssemblies()
            Dim i As Integer = 0

            While i < loadedAssemblies.Length AndAlso Not found
                Dim assembly As System.Reflection.Assembly = loadedAssemblies(i)
                Dim types As Type() = assembly.GetTypes()
                Dim j As Integer = 0

                While j < types.Length AndAlso Not found
                    Dim type As Type = types(j)

                    If type.FullName = typeNode.InnerXml.ToString() Then
                        Dim toolboxItem As New ToolboxItem() With {.Type = type}

                        toolboxItems.Add(toolboxItem)
                        found = True
                    End If
                    j += 1
                End While

                i += 1
            End While
        Next

        toolboxTab.ToolboxItems = toolboxItems

        Return
    End Sub
#End Region

    ''' <summary>
    ''' Для типа элемента управления выдать описание его назначение в функциональной панели
    ''' </summary>
    ''' <param name="mType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetStringHelpByType(mType As Type) As String
        Dim stringHelp As String = String.Format("<{0}>:", mType.Name) 'String.Empty
        '' GetType(System.Windows.Forms.NumericUpDown), GetType(Microsoft.VisualBasic.PowerPacks.ShapeContainer), GetType(Microsoft.VisualBasic.PowerPacks.RectangleShape), GetType(Microsoft.VisualBasic.PowerPacks.OvalShape), GetType(Microsoft.VisualBasic.PowerPacks.LineShape)

        Select Case mType
            Case GetType(Label)
                stringHelp += "Надпись"
                Exit Select
            Case GetType(NumericEdit)
                stringHelp += "Цифровое значение параметра"
                Exit Select
            Case GetType(NationalInstruments.UI.WindowsForms.Switch)
                stringHelp += "Переключатель дискретного выхода"
                Exit Select
            Case GetType(Led)
                stringHelp += "Состояние дискретного входа"
                Exit Select
            Case GetType(TextBox)
                stringHelp += "Текстовое поле"
                Exit Select
            Case GetType(Button)
                stringHelp += "Кнопка"
                Exit Select
            Case GetType(CheckBox)
                stringHelp += "Флажок"
                Exit Select
            Case GetType(RadioButton)
                stringHelp += "Кнопка выбора"
                Exit Select
            Case GetType(Panel)
                stringHelp += "Панель элементов"
                Exit Select
            Case GetType(GroupBox)
                stringHelp += "Групповая панель"
                Exit Select
            Case GetType(TabControl)
                stringHelp += "Панель вкладок"
                Exit Select
            Case GetType(TableLayoutPanel)
                stringHelp += "Табличная панель"
                Exit Select
            Case GetType(FlowLayoutPanel)
                stringHelp += "Динамическая панель"
                Exit Select
            Case GetType(PictureBox)
                stringHelp += "Фоновый рисунок панели"
                Exit Select
            Case GetType(Tank)
                stringHelp += "Значение параметра"
                Exit Select
            Case GetType(Gauge)
                stringHelp += "Значение параметра"
                Exit Select
            Case GetType(Thermometer)
                stringHelp += "Значение параметра"
                Exit Select
            Case GetType(Meter)
                stringHelp += "Значение параметра"
                Exit Select
            Case GetType(Slide)
                stringHelp += "Установить значение параметра"
                Exit Select
            Case GetType(Knob)
                stringHelp += "Установить значение параметра"
                Exit Select
            Case GetType(WaveformGraph)
                stringHelp += "График параметра по времени"
                Exit Select
            Case GetType(ScatterGraph)
                stringHelp += "График одного параметра от значений другого"
                Exit Select
        End Select

        Return stringHelp
    End Function

    ''' <summary>
    ''' Вспомогательный класс обслуживания строковых констант
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ToolboxControlStrings
        Public Const Toolbox As String = "Toolbox"
        Public Const TabCollection As String = "TabCollection"
        Public Const Tab As String = "Tab"
        Public Const Properties As String = "Properties"
        Public Const Name As String = "Name"
        Public Const ToolboxItemCollection As String = "ToolboxItemCollection"
        Public Const ToolboxItem As String = "ToolboxItem"
        Public Const Type As String = "Type"

        Public Const сActiveControlslTypes As String = "Основные элементы"
        'Public Const сLinkingControlTypes As String = "Связанные элементы"
        Public Const сWindowsFormsPanelsTypes As String = "Группировка элементов"
        Public Const сControlsSlideTypes As String = "Индикаторы параметров"
        Public Const сAnalogOutputTypes As String = "Источник напряжения"
        Public Const сFrequencyTypes As String = "Функциональный генератор"
        Public Const сControlsGraphTypes As String = "Графики параметров"
    End Class
End Class