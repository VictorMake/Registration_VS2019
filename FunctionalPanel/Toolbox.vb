Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design

''' <summary>
''' Toolbox панель элементов - это реализация IToolboxService, включающая
''' перетаскивание инструментов на host
''' Представляет панельки с кнопками и листами с элементами
''' Имеет режим работы в дизайнере VS
''' </summary>
''' <remarks></remarks>
Public Class Toolbox
    Inherits UserControl
    Implements IToolboxService ' Обеспечивает методы и свойства для управления панелью инструментов в среде разработки и выполнения запросов к этой панели.
    Private m_filePath As String = Nothing
    Private selectedIndex As Integer = 0
    'Public Property HelpToolboxItemDictionary As Dictionary(Of String, String) ' подсказки назначения элементов управления
    ' подсказки назначения элементов управления

    Public Sub New()
        ' This call is required by the Windows.Forms Form Designer.
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Заполняет Toolbox элементами
    ''' Вызывается в конструкторе frmРедакторПанелейМоториста (Me.Toolbox.FilePath = Nothing)
    ''' при задании свойства FilePath
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeToolbox()
        HelpToolboxItemDictionary = New Dictionary(Of String, String)

        Dim toolboxXmlManager As New ToolboxXmlManager(Me)
        Tabs = toolboxXmlManager.PopulateToolboxInfo()

        Dim toolboxUIManagerVS As New ToolboxUIManagerVS(Me)
        toolboxUIManagerVS.FillToolbox()

        AddEventHandlers()
        'изменил
        'PrintToolbox()
    End Sub

    'изменил
    'Private Sub PrintToolbox()
    '    Try
    '        For i As Integer = 0 To Tabs.Count - 1
    '            Console.WriteLine(Tabs(i).Name)
    '            For j As Integer = 0 To Tabs(i).ToolboxItems.Count - 1
    '                Console.WriteLine(vbTab + Tabs(i).ToolboxItems(j).Type.ToString())
    '            Next
    '            Console.WriteLine(" ")
    '        Next
    '    Catch ex As Exception
    '        MessageBox.Show(ex.ToString())
    '    End Try
    'End Sub

    ''' <summary>
    ''' Предоставляет интерфейс для управления транзакциями и компонентами конструктора.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DesignerHost() As IDesignerHost = Nothing

    ''' <summary>
    ''' Реализация строго типизированной коллекции вкладок.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Tabs() As ToolboxTabCollection = Nothing

    ''' <summary>
    ''' Реализация строго типизированной коллекции подсказок.
    ''' Идиотское свойство меняется только когда в редакторе для frmРедакторПанелейМоториста меняем что-либо 
    ''' в свойстве HelpToolboxItemDictionary PropertyGrid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property HelpToolboxItemDictionary As Dictionary(Of String, String) = Nothing

    ''' <summary>
    ''' Me.Toolbox.FilePath = Nothing в конструкторе frmРедакторПанелейМоториста
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FilePath() As String
        Get
            Return m_filePath
        End Get
        Set(ByVal value As String)
            m_filePath = value
            InitializeToolbox()
        End Set
    End Property

    ''' <summary>
    ''' Массив кнопок на вкладке
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property TabPageArray() As Button() = Nothing

    ''' <summary>
    ''' ListBox с кнопками на вкладке
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property ToolsListBox() As ListBox = Nothing

    ''' <summary>
    ''' Обработчики событий мыши над листом с кнопками
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddEventHandlers()
        AddHandler ToolsListBox.KeyDown, New KeyEventHandler(AddressOf list_KeyDown)
        AddHandler ToolsListBox.MouseDown, New MouseEventHandler(AddressOf list_MouseDown)
        AddHandler ToolsListBox.DrawItem, New DrawItemEventHandler(AddressOf list_DrawItem)
        ' не получается сделать выделение над элементом листа при перемещении мыши над элементом
        'AddHandler ToolsListBox.MouseHover, New System.Windows.Forms.MouseEventHandler(AddressOf Me.list_MouseHover)
        AddHandler ToolsListBox.MouseHover, New EventHandler(AddressOf List_MouseHover)
        'AddHandler ToolsListBox.MouseMove, New System.Windows.Forms.MouseEventHandler(AddressOf Me.list_MouseHover)
    End Sub

    ''' <summary>
    ''' Происходит при изменении вида элемента System.Windows.Forms.ListBox, рисуемого владельцем.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub list_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
        Try
            Dim lbSender As ListBox = TryCast(sender, ListBox)
            If lbSender Is Nothing Then
                Return
            End If

            ' Если этот инструмент есть текущий выделенный инструмент, переместить его на свободный участок 
            If selectedIndex = e.Index Then
                e.Graphics.FillRectangle(Brushes.LightSlateGray, e.Bounds)
            End If

            Dim tbi As System.Drawing.Design.ToolboxItem = TryCast(lbSender.Items(e.Index), System.Drawing.Design.ToolboxItem)
            Dim BitmapBounds As New Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y + e.Bounds.Height \ 2 - tbi.Bitmap.Height \ 2, tbi.Bitmap.Width, tbi.Bitmap.Height)
            Dim StringBounds As New Rectangle(e.Bounds.Location.X + BitmapBounds.Width + 5, e.Bounds.Location.Y, e.Bounds.Width - BitmapBounds.Width, e.Bounds.Height)

            Dim format As New StringFormat() With {
                .LineAlignment = StringAlignment.Center,
                .Alignment = StringAlignment.Near}

            e.Graphics.DrawImage(tbi.Bitmap, BitmapBounds)
            e.Graphics.DrawString(tbi.DisplayName, New Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.World), Brushes.Black, StringBounds, format)
        Catch ex As Exception
            Const caption As String = "list_DrawItem"
            Dim text As String = ex.ToString
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    ''' <summary>
    ''' Происходит при нажатии кнопки мыши, если указатель мыши находится на элементе управления.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub list_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim caption As String = "list_MouseDown"
        Dim text As String = String.Empty

        Try
            Dim lbSender As ListBox = TryCast(sender, ListBox)
            Dim lastSelectedBounds As Rectangle = lbSender.GetItemRectangle(0)

            Try
                lastSelectedBounds = lbSender.GetItemRectangle(selectedIndex)
            Catch ex As Exception
                'ex.ToString()
                text = ex.ToString
                'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
            End Try

            selectedIndex = lbSender.IndexFromPoint(e.X, e.Y)
            ' изменить выделение
            lbSender.SelectedIndex = selectedIndex
            lbSender.Invalidate(lastSelectedBounds)
            ' очистить место из последнего выделения
            lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex))
            ' на передний план
            If selectedIndex <> 0 Then
                If e.Clicks = 2 Then
                    Dim idh As IDesignerHost = DirectCast(DesignerHost.GetService(GetType(IDesignerHost)), IDesignerHost)
                    Dim tbu As IToolboxUser = TryCast(idh.GetDesigner(TryCast(idh.RootComponent, IComponent)), IToolboxUser)

                    If tbu IsNot Nothing Then
                        tbu.ToolPicked(DirectCast((lbSender.Items(selectedIndex)), System.Drawing.Design.ToolboxItem))
                    End If
                ElseIf e.Clicks < 2 Then
                    Dim tbi As System.Drawing.Design.ToolboxItem = TryCast(lbSender.Items(selectedIndex), System.Drawing.Design.ToolboxItem)
                    Dim tbs As IToolboxService = Me

                    ' IToolboxService сериализован ToolboxItems посредством упаковки их в DataObjects
                    Dim d As DataObject = TryCast(tbs.SerializeToolboxItem(tbi), DataObject)

                    Try
                        lbSender.DoDragDrop(d, DragDropEffects.Copy)
                    Catch ex As Exception
                        caption = "list_MouseDown Error"
                        text = ex.ToString
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.[Error])
                        RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
                    End Try
                End If
            End If
        Catch ex As Exception
            caption = "list_MouseDown"
            text = ex.ToString
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    ''' <summary>
    ''' Происходит при нажатии клавиши, если элемент управления имеет фокус.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub list_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        Try
            Dim lbSender As ListBox = TryCast(sender, ListBox)
            Dim lastSelectedBounds As Rectangle = lbSender.GetItemRectangle(0)

            Try
                lastSelectedBounds = lbSender.GetItemRectangle(selectedIndex)
            Catch ex As Exception
                'ex.ToString()
                Const caption As String = "list_KeyDown"
                Dim text As String = ex.ToString
                'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
            End Try

            Select Case e.KeyCode
                Case Keys.Up
                    If selectedIndex > 0 Then
                        selectedIndex -= 1
                        ' изменить выделение
                        lbSender.SelectedIndex = selectedIndex
                        lbSender.Invalidate(lastSelectedBounds)
                        ' очистить старое выделение 
                        ' добавить новое
                        lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex))
                    End If
                    Exit Select

                Case Keys.Down
                    If selectedIndex + 1 < lbSender.Items.Count Then
                        selectedIndex += 1
                        ' изменить выделение
                        lbSender.SelectedIndex = selectedIndex
                        lbSender.Invalidate(lastSelectedBounds)
                        ' очистить старое выделение
                        ' добавить новое
                        lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex))
                    End If
                    Exit Select

                Case Keys.Enter
                    If DesignerHost Is Nothing Then
                        MessageBox.Show("Отсутствует хост дизайнера")
                    End If

                    Dim tbu As IToolboxUser = TryCast(DesignerHost.GetDesigner(TryCast(DesignerHost.RootComponent, IComponent)), IToolboxUser)

                    If tbu IsNot Nothing Then
                        ' Ввести в возможное место инструмент с расположением и размером по умолчанию
                        tbu.ToolPicked(DirectCast((lbSender.Items(selectedIndex)), System.Drawing.Design.ToolboxItem))
                        lbSender.Invalidate(lastSelectedBounds)
                        ' очистить старое выделение
                        ' добавить новое
                        lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex))
                    End If

                    Exit Select
                Case Else

                    'If True Then
                    Console.WriteLine("Ошибка: Невозможно добавить")
                    MessageBox.Show("Ошибка: Невозможно добавить", "Работа в панели инструментов с клавиатурой", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Select
                    'End If
                    ' switch
            End Select
        Catch ex As Exception
            Const caption As String = "list_KeyDown"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    ''' <summary>
    ''' Происходит, когда указатель мыши задерживается на элементе управления.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub List_MouseHover(sender As Object, e As EventArgs)
        Dim lbSender As ListBox = TryCast(sender, ListBox)

        Try
            'Dim frm As EditingFunctionPanels = CType(lbSender.TopLevelControl, EditingFunctionPanels)
            'For i = 0 To lbSender.Items.Count - 1
            Dim msg As String = String.Empty
            'Dim list As ListBox = Me.ToolsListBox

            'For I = 0 To list.Items.Count - 1
            '    Dim tbi As System.Drawing.Design.ToolboxItem = DirectCast(list.Items(I), System.Drawing.Design.ToolboxItem)
            '    If tbi.DisplayName <> "<Pointer>" Then
            '        msg += tbi.DisplayName & " "
            '        'Else
            '    End If
            'Next

            If HelpToolboxItemDictionary.ContainsKey(lbSender.Tag.ToString) Then
                msg = HelpToolboxItemDictionary(lbSender.Tag.ToString)
            End If
            'frm.СообщениеНаПанель(msg)
            EditorPanelMotoristForm.ShowMessageOnStatusPanel(msg)
        Catch ex As Exception
            Const caption As String = NameOf(List_MouseHover)
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
        End Try
    End Sub

    'Private Sub list_MouseHover(sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    Dim caption As String = "list_MouseHover"
    '    Dim text As String = String.Empty

    '    Try
    '        Dim lbSender As ListBox = TryCast(sender, ListBox)

    '        For i = 0 To lbSender.Items.Count - 1
    '            'For Each itm In lbSender.Items
    '            'CType(CType(itm, ToolboxItem), Button) = Color.Aqua

    '            Dim lastSelectedBounds2 As Rectangle = lbSender.GetItemRectangle(i) '(0)
    '            Try
    '                lastSelectedBounds2 = lbSender.GetItemRectangle(i) 'selectedIndex)
    '            Catch ex As Exception
    '                'ex.ToString()
    '                text = ex.ToString
    '                'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
    '            End Try


    '            'selectedIndex = lbSender.IndexFromPoint(e.X, e.Y)
    '            'изменить выделение
    '            lbSender.SelectedIndex = i 'selectedIndex
    '            lbSender.Invalidate(lastSelectedBounds2)
    '            'очистить место из последнего выделения
    '            lbSender.Invalidate(lbSender.GetItemRectangle(i)) 'selectedIndex))

    '        Next


    '        'Dim lastSelectedBounds As Rectangle = lbSender.GetItemRectangle(0)
    '        'Try
    '        '    lastSelectedBounds = lbSender.GetItemRectangle(selectedIndex)
    '        'Catch ex As Exception
    '        '    'ex.ToString()
    '        '    text = ex.ToString
    '        '    'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        '    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
    '        'End Try


    '        'selectedIndex = lbSender.IndexFromPoint(e.X, e.Y)
    '        ''изменить выделение
    '        'lbSender.SelectedIndex = selectedIndex
    '        'lbSender.Invalidate(lastSelectedBounds)
    '        ''очистить место из последнего выделения
    '        'lbSender.Invalidate(lbSender.GetItemRectangle(selectedIndex))
    '        'на передний план
    '        'If selectedIndex <> 0 Then
    '        '    If e.Clicks = 2 Then
    '        '        Dim idh As IDesignerHost = DirectCast(Me.DesignerHost.GetService(GetType(IDesignerHost)), IDesignerHost)
    '        '        Dim tbu As IToolboxUser = TryCast(idh.GetDesigner(TryCast(idh.RootComponent, IComponent)), IToolboxUser)

    '        '        If tbu IsNot Nothing Then
    '        '            tbu.ToolPicked(DirectCast((lbSender.Items(selectedIndex)), System.Drawing.Design.ToolboxItem))
    '        '        End If
    '        '    ElseIf e.Clicks < 2 Then
    '        '        Dim tbi As System.Drawing.Design.ToolboxItem = TryCast(lbSender.Items(selectedIndex), System.Drawing.Design.ToolboxItem)
    '        '        Dim tbs As IToolboxService = Me

    '        '        'IToolboxService сериализован ToolboxItems посредством упаковки их в DataObjects
    '        '        Dim d As DataObject = TryCast(tbs.SerializeToolboxItem(tbi), DataObject)

    '        '        Try
    '        '            lbSender.DoDragDrop(d, DragDropEffects.Copy)
    '        '        Catch ex As Exception
    '        '            caption = "list_MouseHover Error"
    '        '            text = ex.ToString
    '        '            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.[Error])
    '        '            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
    '        '        End Try
    '        '    End If
    '        'End If
    '    Catch ex As Exception
    '        'ex.ToString()
    '        caption = "list_MouseHover"
    '        text = ex.ToString
    '        'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
    '    End Try

    'End Sub

#Region "IToolboxService Members"
    ' Реализация только действительно важных членов ToolboxService

    ''' <summary>
    ''' Получает выбранный в данный момент элемент панели инструментов, 
    ''' если это доступно для всех конструкторов или поддерживает определенный конструктор.
    ''' </summary>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSelectedToolboxItem(ByVal host As IDesignerHost) As System.Drawing.Design.ToolboxItem Implements IToolboxService.GetSelectedToolboxItem
        Dim list As ListBox = ToolsListBox
        Dim tbi As System.Drawing.Design.ToolboxItem = DirectCast(list.Items(selectedIndex), System.Drawing.Design.ToolboxItem)
        If tbi.DisplayName <> "<Pointer>" Then
            Return tbi
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Получает выбранный в данный момент элемент панели инструментов,
    ''' если это доступно для всех конструкторов или поддерживает определенный конструктор.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSelectedToolboxItem() As System.Drawing.Design.ToolboxItem Implements IToolboxService.GetSelectedToolboxItem
        Return Me.GetSelectedToolboxItem(Nothing)
    End Function

    ''' <summary>
    ''' Добавляет указанный элемент заданной категории на панель инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <param name="category"></param>
    ''' <remarks></remarks>
    Public Sub AddToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem, ByVal category As String) Implements IToolboxService.AddToolboxItem
    End Sub

    ''' <summary>
    ''' Добавляет указанный элемент заданной категории на панель инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <remarks></remarks>
    Public Sub AddToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem) Implements IToolboxService.AddToolboxItem
    End Sub

    ''' <summary>
    ''' Получает значение, указывающее, является ли заданный объект сериализованным элементом панели инструментов, 
    ''' используя указанный хост-узел конструктора.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsToolboxItem(ByVal serializedObject As Object, ByVal host As IDesignerHost) As Boolean Implements IToolboxService.IsToolboxItem
        Return False
    End Function

    ''' <summary>
    ''' Получает значение, указывающее, является ли заданный объект сериализованным элементом панели инструментов, 
    ''' используя указанный хост-узел конструктора.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsToolboxItem(ByVal serializedObject As Object) As Boolean Implements IToolboxService.IsToolboxItem
        Return False
    End Function

    ''' <summary>
    ''' Выбирает указанный элемент панели инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <remarks></remarks>
    Public Sub SetSelectedToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem) Implements IToolboxService.SetSelectedToolboxItem
    End Sub

    ''' <summary>
    ''' Уведомляет службу панели инструментов, что выбранное средство было использовано.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SelectedToolboxItemUsed() Implements IToolboxService.SelectedToolboxItemUsed
        Dim list As ListBox = ToolsListBox

        list.Invalidate(list.GetItemRectangle(selectedIndex))
        selectedIndex = 0
        list.SelectedIndex = 0
        list.Invalidate(list.GetItemRectangle(selectedIndex))
    End Sub

    ''' <summary>
    ''' Получает имена всех категорий средств имеющихся в данный момент на панели инструментов.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CategoryNames() As CategoryNameCollection Implements IToolboxService.CategoryNames
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Обновляет состояние элементов панели инструментов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IToolboxService_Refresh() Implements IToolboxService.Refresh
    End Sub

    ''' <summary>
    ''' Добавляет на панель инструментов заданный связанный с проектом элемент определенной категории.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <param name="category"></param>
    ''' <param name="host"></param>
    ''' <remarks></remarks>
    Public Sub AddLinkedToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem, ByVal category As String, ByVal host As IDesignerHost) Implements IToolboxService.AddLinkedToolboxItem
    End Sub

    ''' <summary>
    ''' Добавляет заданный связанный с проектом элемент на панель инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <param name="host"></param>
    ''' <remarks></remarks>
    Public Sub AddLinkedToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem, ByVal host As IDesignerHost) Implements IToolboxService.AddLinkedToolboxItem
    End Sub

    ''' <summary>
    ''' Получает значение, определяющее, соответствует ли указанный объект, 
    ''' который представляет сериализованный элемент панели инструментов, заданным атрибутам.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <param name="filterAttributes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsSupported(ByVal serializedObject As Object, ByVal filterAttributes As ICollection) As Boolean Implements IToolboxService.IsSupported
        Return False
    End Function

    ''' <summary>
    ''' Получает значение, определяющее, может ли указанный объект, 
    ''' который представляет сериализованный элемент панели инструментов, 
    ''' быть использован указанным хост-узелом конструктора.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsSupported(ByVal serializedObject As Object, ByVal host As IDesignerHost) As Boolean Implements IToolboxService.IsSupported
        Return False
    End Function

    ''' <summary>
    ''' Получает или задает имя выбранной в данный момент категории средств с панели инструментов.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectedCategory() As String Implements IToolboxService.SelectedCategory
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    ''' <summary>
    ''' Получает элемент панели инструментов из указанного объекта, 
    ''' который представляет элемент панели в сериализованной форме, 
    ''' с помощью указанного узла конструктора.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeserializeToolboxItem(ByVal serializedObject As Object, ByVal host As IDesignerHost) As System.Drawing.Design.ToolboxItem Implements IToolboxService.DeserializeToolboxItem
        Return DirectCast(DirectCast(serializedObject, DataObject).GetData(GetType(System.Drawing.Design.ToolboxItem)), System.Drawing.Design.ToolboxItem)
    End Function

    ''' <summary>
    ''' Получает элемент панели инструментов из указанного объекта, 
    ''' который представляет элемент панели в сериализованной форме.
    ''' </summary>
    ''' <param name="serializedObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeserializeToolboxItem(ByVal serializedObject As Object) As System.Drawing.Design.ToolboxItem Implements IToolboxService.DeserializeToolboxItem
        Return Me.DeserializeToolboxItem(serializedObject, DesignerHost)
    End Function

    ''' <summary>
    ''' Получает из панели инструментов коллекцию элементов панели инструментов, 
    ''' которые сопоставлены указанным хост-узлу конструктора и категории.
    ''' </summary>
    ''' <param name="category"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolboxItems(ByVal category As String, ByVal host As IDesignerHost) As System.Drawing.Design.ToolboxItemCollection Implements IToolboxService.GetToolboxItems
        Return Nothing
    End Function

    ''' <summary>
    ''' Получает из панели инструментов коллекцию элементов определенной категории.
    ''' </summary>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolboxItems(ByVal category As String) As System.Drawing.Design.ToolboxItemCollection Implements IToolboxService.GetToolboxItems
        Return Nothing
    End Function

    ''' <summary>
    ''' Получает коллекцию элементов панели элементов, 
    ''' связанных с указанным узлом конструктора из панели элементов.
    ''' </summary>
    ''' <param name="host"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolboxItems(ByVal host As IDesignerHost) As System.Drawing.Design.ToolboxItemCollection Implements IToolboxService.GetToolboxItems
        Return Nothing
    End Function

    ''' <summary>
    ''' Получает полную коллекцию элементов панели инструментов из панели инструментов.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolboxItems() As System.Drawing.Design.ToolboxItemCollection Implements IToolboxService.GetToolboxItems
        Return Nothing
    End Function

    ''' <summary>
    ''' Добавляет создатель нового элемента панели инструментов для заданных формата данных и хост-узла конструктора.
    ''' </summary>
    ''' <param name="creator"></param>
    ''' <param name="format"></param>
    ''' <param name="host"></param>
    ''' <remarks></remarks>
    Public Sub AddCreator(ByVal creator As ToolboxItemCreatorCallback, ByVal format As String, ByVal host As IDesignerHost) Implements IToolboxService.AddCreator
    End Sub

    ''' <summary>
    ''' Добавляет создатель нового элемента панели инструментов для заданного формата данных.
    ''' </summary>
    ''' <param name="creator"></param>
    ''' <param name="format"></param>
    ''' <remarks></remarks>
    Public Sub AddCreator(ByVal creator As ToolboxItemCreatorCallback, ByVal format As String) Implements IToolboxService.AddCreator
    End Sub

    ''' <summary>
    ''' Значение true, если курсор установлен с помощью выбранного в настоящий момент средства, 
    ''' значение false, если ни одно средство не выбрано и курсор является стандартным курсором Windows.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetCursor() As Boolean Implements IToolboxService.SetCursor
        Return False
    End Function

    ''' <summary>
    ''' Удаляет заданный элемент панели инструментов из панели инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <param name="category"></param>
    ''' <remarks></remarks>
    Public Sub RemoveToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem, ByVal category As String) Implements IToolboxService.RemoveToolboxItem
    End Sub

    ''' <summary>
    ''' Удаляет заданный элемент панели инструментов из панели инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <remarks></remarks>
    Public Sub RemoveToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem) Implements IToolboxService.RemoveToolboxItem
    End Sub

    ''' <summary>
    ''' Получает сериализованный объект, который представляет выбранный элемент панели инструментов.
    ''' </summary>
    ''' <param name="toolboxItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SerializeToolboxItem(ByVal toolboxItem As System.Drawing.Design.ToolboxItem) As Object Implements IToolboxService.SerializeToolboxItem
        Return New DataObject(toolboxItem)
    End Function

    ''' <summary>
    ''' Удаляет ранее добавленный разработчик, который связан с указанным форматированием данных и определенным узелом конструктора.
    ''' </summary>
    ''' <param name="format"></param>
    ''' <param name="host"></param>
    ''' <remarks></remarks>
    Public Sub RemoveCreator(ByVal format As String, ByVal host As IDesignerHost) Implements IToolboxService.RemoveCreator
    End Sub

    ''' <summary>
    ''' Удаляет ранее добавленный разработчик элемента панели инструментов для форматирования указанных данных.
    ''' </summary>
    ''' <param name="format"></param>
    ''' <remarks></remarks>
    Public Sub RemoveCreator(ByVal format As String) Implements IToolboxService.RemoveCreator
    End Sub

#End Region

End Class
