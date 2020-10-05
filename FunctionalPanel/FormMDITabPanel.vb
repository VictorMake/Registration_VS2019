Imports System.Collections.Generic
Imports Registration.My
Imports System.IO
Imports MathematicalLibrary

''' <summary>
''' Табличный интерфейс Окна Функциональных панелей
''' </summary>
''' <remarks></remarks>
Friend Class FormMDITabPanel

    Private Enum SampleViewMode
        Simple
        AdvancedBottom
        AdvancedRight
        AdvancedLeft
    End Enum
    Private m_viewMode As SampleViewMode

    Private m_useWindowManager As Boolean = True

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
    End Sub

    Private Sub FormMDITabPanel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)

        Try
            Size = MySettings.[Default].TabPanelFormSize
            Location = MySettings.[Default].TabPanelFormLocation
            WindowState = MySettings.[Default].TabPanelFormState
        Catch ex As Exception
            Trace.WriteLine("FormMDITabPanel_Load exception")
            'Const caption As String = "frmMDITabPanel_Load"
            Dim text As String = ex.ToString
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(FormMDITabPanel_Load)}> {text}")
        End Try

        ToolBoxPanel.Visible = False
        ToolStrip.Visible = False
        StatusStrip.Visible = False
        InitializeSampleView()
    End Sub

    Private Sub FormMDITabPanel_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If gFormsPanelManager.CollectionFormPanelMotorist IsNot Nothing Then
            If gFormsPanelManager.CollectionFormPanelMotorist.Count > 0 Then
                ' Проверить выключение производит пользователь или Панель Управления
                If e.CloseReason = CloseReason.UserClosing Then
                    Const message As String = "Вы действительно хотите закрыть все Окна функциональных панелей?"
                    Const caption As String = "Внимание!"
                    Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
                    Dim Result As DialogResult = MessageBox.Show(message, caption, Buttons, MessageBoxIcon.Question)

                    If Result = DialogResult.No Then
                        'Close()
                        e.Cancel = True
                    End If
                ElseIf e.CloseReason = CloseReason.WindowsShutDown Then
                End If
            End If
        End If

        If e.Cancel = False Then
            ' закрыть все окна через WindowManagerPanel1
            If m_useWindowManager Then
                CType(WindowManagerPanel1.AuxiliaryWindow, FormChildAux).CloseAllCheckedWindows()
            Else
                Dim _ChildAuxForm As FormChildAux = TryGetChildAuxForm()
                If _ChildAuxForm IsNot Nothing Then
                    _ChildAuxForm.CloseAllCheckedWindows()
                End If
            End If

            ' сохранить возможно изменённые значения параметров
            MySettings.[Default].TabPanelFormState = WindowState

            If WindowState = FormWindowState.Normal Then
                MySettings.[Default].TabPanelFormSize = Size
                MySettings.[Default].TabPanelFormLocation = Location
            Else
                MySettings.[Default].TabPanelFormSize = RestoreBounds.Size
                MySettings.[Default].TabPanelFormLocation = RestoreBounds.Location
            End If

            MySettings.[Default].Save()
        End If
    End Sub

    Private Sub FormMDITabPanel_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        MainMdiForm.MenuEditorPanel.Enabled = True
        MainMdiForm.MenuShowPanel.Enabled = True
        'РодительскаяФорма = Nothing
    End Sub

#Region "MDI интерфейс"
    Private Sub InitializeSampleView()
        If m_useWindowManager Then
            InitializeUsingWindowManager()
        Else
            InitializeUsingClassicMDI()
        End If
    End Sub

    ''' <summary>
    ''' Инициализация табличного вида
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeUsingWindowManager()
        ' показать WindowManagerPanel
        WindowManagerPanel1.AutoDetectMdiChildWindows = True
        WindowManagerPanel1.Visible = True

        ' показать Window Menu (смотри WindowMenuItem_Popup для большего кол. деталей)
        WindowMenuItem.Visible = True
        ' скрыть классическое MDI Window Menu если оно видимо
        ClassicMdiWindowMenuItem.Visible = False

        ' настроить внешний вид
        SetupWindowManagerProperties(SampleViewMode.AdvancedRight)

        'LoadSampleWindows()

        ' установить фокус на первое окно
        ' можно также просто использовать Form.Show/Form.BringToFront, но здесь меньше моргание
        'If Me.MdiChildren.Length > 2 Then
        '    Me.WindowManagerPanel1.SetActiveWindow(0)
        'End If

        ' установить фокус на первое дочернее окно mdi
        'Me.MdiChildren(0).BringToFront()
        ' Подобный метод: 
        'WindowManagerPanel1.SetActiveWindow(0)
        ' лучше использовать методы WindowManager
    End Sub

    ''' <summary>
    ''' Инициализация классического MDI
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeUsingClassicMDI()
        ' выключить WindowManagerPanel
        WindowManagerPanel1.AutoDetectMdiChildWindows = False
        WindowManagerPanel1.Visible = False

        ' скрыть специальное Window Menu
        WindowMenuItem.Visible = False
        ' показать Classic MDI Window Menu
        ClassicMdiWindowMenuItem.Visible = True

        ' настроить менеджер
        SetupWindowManagerProperties(SampleViewMode.Simple)

        'LoadSampleWindows()
        LoadAuxWindow()
        'Me.LayoutMdi(MdiLayout.Cascade)
        CreateListMenuByFunctionPanels()

        ' установить фокус на первое окно
        MdiChildren(0).BringToFront()
    End Sub

    ''' <summary>
    ''' Настройка свойств менеджера окон
    ''' </summary>
    ''' <param name="viewMode"></param>
    ''' <remarks></remarks>
    Private Sub SetupWindowManagerProperties(ByVal viewMode As SampleViewMode)
        Select Case viewMode
            Case SampleViewMode.Simple
                ToolBoxPanel.Width = 100

                ' освободить пристыковку вспомогательного окна 
                If WindowManagerPanel1.AuxiliaryWindow IsNot Nothing Then
                    WindowManagerPanel1.AuxiliaryWindow.Close()
                    WindowManagerPanel1.AuxiliaryWindow = Nothing
                End If

                With WindowManagerPanel1
                    .Orientation = MDIWindowManager.WindowManagerOrientation.Top
                    ' следующие свойства просто обычные настройки в дезайн режиме, но можно настроить и программным путём
                    .AllowUserVerticalRepositioning = False
                    .Top = .GetMDIClientAreaBounds.Top
                    .ShowTitle = False
                    .Height = 26
                    .AutoHide = True
                End With
            Case SampleViewMode.AdvancedBottom, SampleViewMode.AdvancedRight, SampleViewMode.AdvancedLeft
                ToolBoxPanel.Width = 100

                With WindowManagerPanel1
                    ' следующие свойства просто обычные настройки в дезайн режиме, но можно настроить и программным путём
                    .ShowTitle = True
                    .Height = 55
                    .AutoHide = False

                    Select Case viewMode
                        Case SampleViewMode.AdvancedBottom
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Bottom
                            .AllowUserVerticalRepositioning = True
                            .Top = 400
                        Case SampleViewMode.AdvancedRight
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Right
                            .AllowUserVerticalRepositioning = False
                            .Top = .GetMDIClientAreaBounds.Top
                            .Width = Width - 200
                        Case SampleViewMode.AdvancedLeft
                            .Orientation = MDIWindowManager.WindowManagerOrientation.Left
                            .AllowUserVerticalRepositioning = False
                            .Top = .GetMDIClientAreaBounds.Top
                            .Width = Width - 200
                    End Select

                    ' настройка вспомогательного окна будут управляться WindowManagerPanel но не показывают их в табличных закладках
                    ' взамен это пристыковка - как сорт оперирования порядком окон достигается 2 или 3 панельным приложением
                    If WindowManagerPanel1.AuxiliaryWindow Is Nothing Then
                        Dim frm As New FormChildAux() With {.FormBorderStyle = FormBorderStyle.None}
                        WindowManagerPanel1.AuxiliaryWindow = frm
                        frm.Show()
                        'If Not Me.HasChildren Then
                        ' загружены подставная WindowManager.DummyForm и ChildAuxForm формы
                        If Not MdiChildren.Length > 2 Then 'ни какая панель не была до этого загружена
                            CreateListMenuByFunctionPanels()
                        Else
                            ShowFiles(PathPanelMotorist) 'создать список
                            GetChildAuxForm.ListViewItemsRebuildChecked() 'если панель уже загружена, то пометка её в списке
                        End If
                    End If

                    If viewMode = SampleViewMode.AdvancedBottom Then
                        .AutoHide = True
                    Else
                        .AutoHide = False
                    End If
                End With
        End Select

        m_viewMode = viewMode
    End Sub

    Private Sub CloseAllChildren()
        If m_useWindowManager Then
            WindowManagerPanel1.CloseAllWindows()
        Else
            For Each frm As Form In MdiChildren
                If Not (TypeOf frm Is FormChildAux) Then frm.Close()
            Next
            ' впомогательную панель ChildAuxForm закрыть в последнюю очередь, т.к. она нужна для пометок панелей в листе
            For Each frm As Form In MdiChildren
                frm.Close()
            Next
        End If
    End Sub

    Public Sub AddChildWindow(ByVal frm As Form)
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub LoadAuxWindow()
        Dim frm As New FormChildAux
        AddChildWindow(frm)
    End Sub

#End Region

#Region "Event"

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim dlg As AboutBoxFunctionPanel = New AboutBoxFunctionPanel() With {.Owner = Me}
        dlg.Show(Me)
    End Sub

    'Private Sub ContentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Process.Start("http://www.cflashsoft.com/progs/mdiwinman/")
    'End Sub
    Private Sub ContentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContentsToolStripMenuItem.Click
        Dim tempFormMain As FormMain = GetFormMain()

        If tempFormMain IsNot Nothing Then
            Help.ShowHelp(Me, tempFormMain.HelpProviderAdvancedCHM.HelpNamespace)
        End If
    End Sub

    Private Sub IndexToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IndexToolStripMenuItem.Click
        Dim tempFormMain As FormMain = GetFormMain()

        If tempFormMain IsNot Nothing Then
            Help.ShowHelpIndex(Me, tempFormMain.HelpProviderAdvancedCHM.HelpNamespace)
        End If
    End Sub

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click
        Dim tempFormMain As FormMain = GetFormMain()

        If tempFormMain IsNot Nothing Then
            Help.ShowHelp(Me, tempFormMain.HelpProviderAdvancedCHM.HelpNamespace, HelpNavigator.Find, "")
        End If
    End Sub

    Public Function GetFormMain() As FormMain
        If RegistrationFormName <> "" Then
            For Each tempForm In CollectionForms.Forms.Values
                If tempForm.Text = RegistrationFormName Then
                    Return CType(tempForm, FormMain)
                End If
            Next
        End If
        Return Nothing
    End Function

#Region "Шаблонные"
    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    'Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Использовать My.Computer.Clipboard для помещения выбранного текста или изображений в буфер обмена
    'End Sub

    'Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Использовать My.Computer.Clipboard для помещения выбранного текста или изображений в буфер обмена
    'End Sub

    'Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    'Использовать My.Computer.Clipboard.GetText() или My.Computer.Clipboard.GetData для получения информации из буфера обмена.
    'End Sub

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        ToolStrip.Visible = ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        StatusStrip.Visible = StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Закрыть все дочерние формы указанного родителя.
        For Each ChildForm As Form In MdiChildren
            ChildForm.Close()
        Next
    End Sub
#End Region

    Private Sub WindowMenuItem_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles WindowMenuItem.MouseDown 'WindowMenuItem.Paint
        ' Использовать расширение WindowManager для загрузки и показа листа всех окон
        ' удалить все старые пункты меню
        'For index As Integer = Me.WindowMenuItem.MenuItems.Count - 1 To 0 Step -1
        '    Dim mnu As MenuItem = Me.WindowMenuItem.MenuItems.Item(index)
        '    If TypeOf mnu Is MDIWindowManager.WrappedWindowMenuItem Then
        '        Me.WindowMenuItem.MenuItems.Remove(mnu)'MenuItem старый тип и в DropDownItems не работает
        '    End If
        'Next index
        Dim listTSMenuItem As New List(Of ToolStripMenuItem)

        For Each menuItem As ToolStripItem In WindowMenuItem.DropDownItems
            If TypeOf menuItem Is ToolStripMenuItem Then
                ' то же самое
                'If (item.GetType() Is GetType(ToolStripMenuItem)) Then
                If CStr(menuItem.Tag) = "WrappedWindowMenuItem" Then
                    listTSMenuItem.Add(CType(menuItem, ToolStripMenuItem))
                    ' удалять в цикле по коллекции нельзя
                    'Me.WindowMenuItem.DropDownItems.Remove(menuItem)
                End If
            End If
        Next

        For Each menuItem As ToolStripMenuItem In listTSMenuItem
            WindowMenuItem.DropDownItems.Remove(menuItem)
        Next

        Dim CopyWindowMoreWindowsMenuItem As ToolStripMenuItem = WindowMoreWindowsMenuItem
        WindowMenuItem.DropDownItems.Remove(WindowMoreWindowsMenuItem)

        ' задать первые 9 оконных пунктов и добавить их в меню
        Dim menuItems As MenuItem() = WindowManagerPanel1.GetAllWindowsMenu(9, True)

        If menuItems IsNot Nothing AndAlso menuItems.Length > 0 Then
            'Me.WindowMenuItem.MenuItems.AddRange(menuItems)
            For Each mnu As MenuItem In menuItems
                ' создадим копию с поведением MenuItem, но типа ToolStripMenuItem
                Dim CopyTSMenuItem As New ToolStripMenuItem
                If mnu.Checked Then
                    CopyTSMenuItem.Checked = True
                    CopyTSMenuItem.CheckState = CheckState.Checked
                End If
                CopyTSMenuItem.Name = mnu.Name
                CopyTSMenuItem.Size = New Size(204, 22)
                CopyTSMenuItem.Text = mnu.Text
                CopyTSMenuItem.ToolTipText = mnu.Text
                CopyTSMenuItem.Tag = "WrappedWindowMenuItem"
                AddHandler CopyTSMenuItem.Click, AddressOf OnTSMenuItemMouseClick
                WindowMenuItem.DropDownItems.Add(CopyTSMenuItem)
            Next
        End If

        ' переместить "more windows" пункт меню в конец
        'Me.WindowMoreWindowsMenuItem.Index = Me.WindowMenuItem.MenuItems.Count - 1
        WindowMenuItem.DropDownItems.Add(CopyWindowMoreWindowsMenuItem)
    End Sub

    Private Sub OnTSMenuItemMouseClick(ByVal sender As Object, ByVal e As EventArgs)
        'перенаправить обработку из ToolStripMenuItem в MenuItem для WindowManagerPanel1
        'соответствие находится по .Text
        Dim TSMenuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim menuItems As MenuItem() = WindowManagerPanel1.GetAllWindowsMenu(9, True)
        For Each mnu As MenuItem In menuItems
            If mnu.Text = TSMenuItem.Text Then
                mnu.PerformClick()
                Exit For
            End If
        Next
    End Sub

#Region "Управление одиночным окном"
    Private Sub WindowHTileMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowHTileMenuItem.Click
        WindowManagerPanel1.HTileWrappedWindow(WindowManagerPanel1.GetActiveWindow())
    End Sub

    Private Sub WindowTileMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowTileMenuItem.Click
        WindowManagerPanel1.TileOrUntileWrappedWindow(WindowManagerPanel1.GetActiveWindow())
    End Sub

    Private Sub WindowPopOutMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowPopOutMenuItem.Click
        WindowManagerPanel1.PopOutWrappedWindow(WindowManagerPanel1.GetActiveWindow())
    End Sub

    Private Sub WindowCloseAllMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowCloseAllMenuItem.Click
        CloseAllChildren()
    End Sub
#End Region

    Private Sub WindowMoreWindowsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles WindowMoreWindowsMenuItem.Click
        WindowManagerPanel1.ShowAllWindowsDialog()
    End Sub

#Region "Дополнительная панель"
    Private Sub ViewSimpleMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewSimpleMenuItem.Click
        SetupWindowManagerProperties(SampleViewMode.Simple)
    End Sub

    Private Sub ViewAdvRightMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewAdvRightMenuItem.Click
        SetupWindowManagerProperties(SampleViewMode.AdvancedRight)
    End Sub

    Private Sub ViewAdvBottomMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewAdvBottomMenuItem.Click
        SetupWindowManagerProperties(SampleViewMode.AdvancedBottom)
    End Sub

    Private Sub ViewAdvLeftMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewAdvLeftMenuItem.Click
        SetupWindowManagerProperties(SampleViewMode.AdvancedLeft)
    End Sub
#End Region

    Private Sub ViewShowTitleMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewShowTitleMenuItem.Click
        WindowManagerPanel1.ShowTitle = Not WindowManagerPanel1.ShowTitle

        If WindowManagerPanel1.ShowTitle Then
            WindowManagerPanel1.Height = 42
        Else
            WindowManagerPanel1.Height = 26
        End If
    End Sub

#Region "TabStile"
    Private Sub ViewShowIconsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewShowIconsMenuItem.Click
        WindowManagerPanel1.ShowIcons = Not WindowManagerPanel1.ShowIcons
    End Sub

    Private Sub ViewShowLayoutButtonsMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewShowLayoutButtonsMenuItem.Click
        WindowManagerPanel1.ShowLayoutButtons = Not WindowManagerPanel1.ShowLayoutButtons
    End Sub

    Private Sub ViewShowCloseButtonMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewShowCloseButtonMenuItem.Click
        WindowManagerPanel1.ShowCloseButton = Not WindowManagerPanel1.ShowCloseButton
    End Sub

    Private Sub ViewTabStylesClassicMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewTabStylesClassicMenuItem.Click
        WindowManagerPanel1.Style = MDIWindowManager.TabStyle.ClassicTabs
    End Sub

    Private Sub ViewTabStylesModernMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewTabStylesModernMenuItem.Click
        WindowManagerPanel1.Style = MDIWindowManager.TabStyle.ModernTabs
    End Sub

    Private Sub ViewTabStylesFlatHiliteMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewTabStylesFlatHiliteMenuItem.Click
        WindowManagerPanel1.Style = MDIWindowManager.TabStyle.FlatHiliteTabs
    End Sub

    Private Sub ViewTabStylesAngledHiliteMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewTabStylesAngledHiliteMenuItem.Click
        WindowManagerPanel1.Style = MDIWindowManager.TabStyle.AngledHiliteTabs
    End Sub

    'Private Sub ViewTabStylesMoreInfoMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    MsgBox("These are the default tab styles provided by WindowManagerPanel. You can customize or completely handle how tabs are drawn via the <<TabPaint>> event (see Custom Paint Sample)." & vbCrLf & vbCrLf _
    '& "Additionally, you can also create totally custom 'TabProviders.' MDIWindowManager currently contains an additional TabProvider called SystemTabsProvider, which uses the standard .NET TabControl to present the tabs (see Alternate Tabs Sample). " _
    '& "For convenience, you can accomplish the same shown by the Alternate Tabs Sample by using TabRenderMode property in the designer." & vbCrLf & vbCrLf _
    ', MsgBoxStyle.Information)
    'End Sub

    Private Sub ViewButtonStylesStandardMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewButtonStylesStandardMenuItem.Click
        WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Standard
    End Sub

    Private Sub ViewButtonStylesSystemMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewButtonStylesSystemMenuItem.Click
        WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.System
    End Sub

    Private Sub ViewButtonStylesProMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewButtonStylesProMenuItem.Click
        WindowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Professional
    End Sub
#End Region

    Private Sub ShowToolboxMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowToolboxMenuItem.Click
        ToolBoxPanel.Visible = Not ToolBoxPanel.Visible
        ToolBoxSplitter.Visible = ToolBoxPanel.Visible
    End Sub

    Private Sub SwitchToClassicMdiMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SwitchToClassicMdiMenuItem.Click
        CloseAllChildren()
        m_useWindowManager = Not m_useWindowManager
        InitializeSampleView()
    End Sub

    Private Sub ViewMenuItem_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles ViewMenuItem.Paint
        ' установить Checked свойства только во время перерисовки
        ViewSimpleMenuItem.Enabled = m_useWindowManager
        ViewAdvRightMenuItem.Enabled = m_useWindowManager
        ViewAdvBottomMenuItem.Enabled = m_useWindowManager
        ViewAdvLeftMenuItem.Enabled = m_useWindowManager
        ViewAppearanceMenuItem.Enabled = m_useWindowManager

        ViewSimpleMenuItem.Checked = False
        ViewAdvRightMenuItem.Checked = False
        ViewAdvBottomMenuItem.Checked = False
        ViewAdvLeftMenuItem.Checked = False

        Select Case m_viewMode
            Case SampleViewMode.Simple
                ViewSimpleMenuItem.Checked = True
            Case SampleViewMode.AdvancedRight
                ViewAdvRightMenuItem.Checked = True
            Case SampleViewMode.AdvancedBottom
                ViewAdvBottomMenuItem.Checked = True
            Case SampleViewMode.AdvancedLeft
                ViewAdvLeftMenuItem.Checked = True
        End Select

        ViewShowTitleMenuItem.Checked = WindowManagerPanel1.ShowTitle
        ViewShowIconsMenuItem.Checked = WindowManagerPanel1.ShowIcons
        ViewShowLayoutButtonsMenuItem.Checked = WindowManagerPanel1.ShowLayoutButtons
        ViewShowCloseButtonMenuItem.Checked = WindowManagerPanel1.ShowCloseButton
        ShowToolboxMenuItem.Checked = ToolBoxPanel.Visible

        ViewTabStylesClassicMenuItem.Checked = False
        ViewTabStylesModernMenuItem.Checked = False
        ViewTabStylesFlatHiliteMenuItem.Checked = False
        ViewTabStylesAngledHiliteMenuItem.Checked = False

        Select Case WindowManagerPanel1.Style
            Case MDIWindowManager.TabStyle.ClassicTabs
                ViewTabStylesClassicMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.ModernTabs
                ViewTabStylesModernMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.FlatHiliteTabs
                ViewTabStylesFlatHiliteMenuItem.Checked = True
            Case MDIWindowManager.TabStyle.AngledHiliteTabs
                ViewTabStylesAngledHiliteMenuItem.Checked = True
        End Select

        ViewButtonStylesStandardMenuItem.Checked = False
        ViewButtonStylesSystemMenuItem.Checked = False
        ViewButtonStylesProMenuItem.Checked = False

        Select Case WindowManagerPanel1.ButtonRenderMode
            Case MDIWindowManager.ButtonRenderMode.Standard
                ViewButtonStylesStandardMenuItem.Checked = True
            Case MDIWindowManager.ButtonRenderMode.System
                ViewButtonStylesSystemMenuItem.Checked = True
            Case MDIWindowManager.ButtonRenderMode.Professional
                ViewButtonStylesProMenuItem.Checked = True
        End Select

        If m_useWindowManager Then
            SwitchToClassicMdiMenuItem.Text = "Переключить В Классический Мультидокумент"
        Else
            SwitchToClassicMdiMenuItem.Text = "Переключить В Табличный Мультидокумент"
        End If
    End Sub

#End Region

#Region "Работа с панелями моториста"
    Public Function TryGetChildAuxForm() As FormChildAux
        Dim _ChildAuxForm As FormChildAux = Nothing
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FormChildAux Then
                _ChildAuxForm = CType(frm, FormChildAux)
            End If
        Next
        Return _ChildAuxForm
    End Function

    ''' <summary>
    ''' получить ChildAuxForm форму или создать в случае отсутствия
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChildAuxForm() As FormChildAux
        Dim _ChildAuxForm As FormChildAux = Nothing

        If m_useWindowManager Then
            If WindowManagerPanel1.AuxiliaryWindow IsNot Nothing Then
                _ChildAuxForm = CType(WindowManagerPanel1.AuxiliaryWindow, FormChildAux)
            Else
                _ChildAuxForm = New FormChildAux With {.FormBorderStyle = FormBorderStyle.None}

                If WindowManagerPanel1.Orientation = MDIWindowManager.WindowManagerOrientation.Top Then
                    WindowManagerPanel1.Orientation = MDIWindowManager.WindowManagerOrientation.Right
                End If

                WindowManagerPanel1.AuxiliaryWindow = _ChildAuxForm
                _ChildAuxForm.Show()
                CreateListMenuByFunctionPanels()
            End If
        Else
            For Each frm As Form In MdiChildren
                If TypeOf frm Is FormChildAux Then
                    _ChildAuxForm = CType(frm, FormChildAux)
                End If
            Next

            If _ChildAuxForm Is Nothing Then
                _ChildAuxForm = New FormChildAux
                AddChildWindow(_ChildAuxForm)
                CreateListMenuByFunctionPanels()
            End If

            ' не работает т.к. ChildAuxForm может быть по любым индексом
            'If TypeOf Me.MdiChildren(0) Is ChildAuxForm Then
            '    _ChildAuxForm = CType(Me.MdiChildren(0), ChildAuxForm)
            'Else
            '    _ChildAuxForm = New ChildAuxForm
            '    AddChildWindow(_ChildAuxForm)
            'End If
        End If

        Return _ChildAuxForm
    End Function

    ''' <summary>
    ''' Создание Меню Для Панелей Моториста
    ''' </summary>
    Public Sub CreateListMenuByFunctionPanels()
        GetChildAuxForm.RemoveHandlerListViewItemChecked()

        Try
            ClearListMenuByFunctionPanels()
            ShowFiles(PathPanelMotorist)
        Catch ex As Exception
            Dim caption As String = "Считывание каталога " & PathPanelMotorist
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        GetChildAuxForm.AddHandlerListViewItemChecked()
    End Sub

    ''' <summary>
    ''' Снять Выделение С Меню Моториста
    ''' </summary>
    ''' <param name="strnNotice"></param>
    Public Sub UnCheckListMenuByMotoristPanels(ByVal strnNotice As String)
        GetChildAuxForm.UnCheckListMenuByFunctionPanels(strnNotice)
    End Sub

    ''' <summary>
    ''' Удалить Все Меню Панелей Моториста
    ''' </summary>
    Public Sub ClearListMenuByFunctionPanels()
        Dim arrName As String()

        If gFormsPanelManager.CollectionFormPanelMotorist IsNot Nothing Then
            Dim keyColl As Dictionary(Of String, FormBasePanelMotorist).KeyCollection = gFormsPanelManager.CollectionFormPanelMotorist.Keys
            'ReDim_arrName(keyColl.Count - 1)
            Re.Dim(arrName, keyColl.Count - 1)
            keyColl.CopyTo(arrName, 0)

            For Each TempKeyPanel As String In arrName
                gFormsPanelManager.CollectionFormPanelMotorist.Item(TempKeyPanel).Close()
            Next
            gFormsPanelManager.CollectionFormPanelMotorist.Clear() 'по идеи она уже чистая
            ' затем очистка коллекции настроек
        End If

        ' далее удалить все пункты меню
        GetChildAuxForm.ClearListMenuByFunctionPanels()
    End Sub

    Private Sub ShowFiles(ByVal strDirectory As String)
        Dim diDirectories As New DirectoryInfo(strDirectory)
        Dim afiFiles As FileInfo()

        Try
            ' вызвать метод GetFiles чтобы получить массив файлов в директории
            afiFiles = diDirectories.GetFiles()
        Catch
            Return
        End Try

        Dim fi As FileInfo
        For Each fi In afiFiles
            ' Create ListViewItem.
            'Dim lvi As New ListViewItem(fi.Name)
            ' определить ImageIndex базовый от расширения.
            Select Case Path.GetExtension(fi.Name).ToUpper()
                Case ".XML"
                    AddPanelListViewItems(fi.Name.Replace(".xml", ""))
                    'lvi.ImageIndex = 1
                    'Case Else
                    'lvi.ImageIndex = 0
            End Select
            'добавить длину и время последней модификации
            ' Add file length and last modified time sub-items.
            'lvi.SubItems.Add(fi.Length.ToString("N0"))
            'lvi.SubItems.Add(fi.LastWriteTime.ToString())
            'добавить аттрибут
            'Dim strAttr As String = ""
            'If (fi.Attributes And FileAttributes.Archive) <> 0 Then
            '    strAttr += "A"
            'End If
            'If (fi.Attributes And FileAttributes.Hidden) <> 0 Then
            '    strAttr += "H"
            'End If
            'If (fi.Attributes And FileAttributes.ReadOnly) <> 0 Then
            '    strAttr += "R"
            'End If
            'If (fi.Attributes And FileAttributes.System) <> 0 Then
            '    strAttr += "S"
            'End If
            'lvi.SubItems.Add(strAttr)
            ' добавить укомплектованный ListViewItem в FileListView.
            'Items.Add(lvi)
        Next fi
    End Sub

    Private Sub AddPanelListViewItems(ByVal strnNotice As String)
        GetChildAuxForm.AddPanelListViewItems(strnNotice, 0)
    End Sub

#End Region

End Class

'Private Sub LoadSampleWindows()
'    For count As Integer = 1 To 10
'        Dim frm As New ChildForm1

'        frm.Text = "Window " + CStr(count)
'        frm.TextBox1.Text = "I am Form " + CStr(count)
'        'If AutoDetectMdiChildren property were False this would be
'        'the only line of code that is different than regular old mdi.
'        'WindowManagerPanel1.AddWindow(frm)

'        AddChildWindow(frm)
'    Next count
'End Sub


'Private Sub DoFileNew()
'    AddChildWindow(New ChildForm2)
'End Sub

'Private Sub DoFileOpen()
'    Dim dlg As New OpenFileDialog
'    dlg.Filter = "Text Files|*.txt|All Files|*.*"

'    If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
'        Dim frm As New ChildForm2
'        AddChildWindow(frm)
'        Dim sr As New System.IO.StreamReader(dlg.FileName, System.Text.Encoding.Default)
'        frm.TextBox1.Text = sr.ReadToEnd()
'        sr.Close()
'    End If
'End Sub

'Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click
'    '' Создать новый экземпляр дочерней формы.
'    'Dim ChildForm As New System.Windows.Forms.Form
'    '' Сделать ее дочерней для данной формы MDI перед отображением.
'    'ChildForm.MdiParent = Me
'    'm_ChildFormNumber += 1
'    'ChildForm.Text = "Окно " & m_ChildFormNumber
'    'ChildForm.Show()
'    DoFileNew()
'End Sub

'Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
'    'Dim OpenFileDialog As New OpenFileDialog
'    'OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
'    'OpenFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
'    'If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
'    '    Dim FileName As String = OpenFileDialog.FileName
'    '    '  добавьте здесь код открытия файла.
'    'End If
'    DoFileOpen()
'End Sub