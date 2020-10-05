'Imports System.Drawing.Drawing2D
'Imports System.ComponentModel
'Imports System.Drawing.Imaging
'Imports System.Drawing.Text


'Namespace MsdnMag

'#Region "GraphicMenu Class"
'' ***************************************************************
''  GraphicMenu Class
''  Subclasses a WinForms Menu object making all items ownerdraw
'Public Class GraphicMenu : Inherits Component
'Public conMenuItemBackColorStart As Color = Color.White
'Public conMenuItemBackColorEnd As Color = Color.LightSkyBlue

'#Region "Private members"
'    ' ***********************************************************************
'    ' Private members
'    Private MenuItemWidth As Integer = 60
'    Private MenuItemHeight As Integer = 22
'    Private BitmapWidth As Integer = 20
'    Private BitmapHeight As Integer = MenuItemHeight
'    Private VerticalTextOffset As Integer = 0
'    Private HorizontalTextOffset As Integer = 6
'    Private SeparatorHeight As Integer = 6
'    Private RightOffset As Integer = 15
'    Private menuItemIconCollection As Hashtable
'    Private ItemFont As Font
'    Private BitmapBounds As RectangleF
'    Private MenuItemBounds As RectangleF
'    Private ItemBounds As RectangleF
'    Private ItemTextBounds As RectangleF

'    Private __BitmapBackColor As Color = Color.Gainsboro
'    Private __MenuItemBackColorStart As Color = Color.Snow
'    Private __MenuItemBackColorEnd As Color = Color.Gainsboro
'    Private __MenuItemForeColor As Color = Color.Navy
'    Private __MenuItemForeColorDisabled As Color = Color.Gray
'    Private __MenuItemBackColorSelected As Color = Color.FromArgb(182, 189, 210)
'    Private __MenuItemBorderSelected As Color = Color.Indigo
'    Private __FontName As String = "Tahoma"
'    Private __FontSize As Integer = 8
'    Private __MenuItemDithered As Boolean = True

'    Private __AutoBind As Boolean = True

'    Private WithEvents mimageListBit As System.Windows.Forms.ImageList 'victor
'    'Private mimageListBit As System.Windows.Forms.ImageList  'victor
'#End Region

'#Region "Appearance Properties"

'    Public Property imageListBit() As System.Windows.Forms.ImageList 'victor
'        Get
'            Return mimageListBit
'        End Get
'        Set(ByVal Value As System.Windows.Forms.ImageList)
'            mimageListBit = Value
'        End Set
'    End Property


'    ' ***********************************************************************
'    ' Public properties defining the appearance of the menu

'    ' ***********************************************************************
'    ' PROPERTY: BitmapBackColor
'    ' NOTES   : Background color of the bitmap (if Color.Empty, use the  
'    '           menu item background color)
'    Public Property BitmapBackColor() As Color
'        Get
'            Return __BitmapBackColor
'        End Get
'        Set(ByVal Value As Color)
'            __BitmapBackColor = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemBackColorStart
'    ' NOTES   : Start color of a menu item dithered background. If not 
'    '           dithered, this is the menu item background color
'    Public Property MenuItemBackColorStart() As Color
'        Get
'            Return __MenuItemBackColorStart
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemBackColorStart = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemBackColorEnd
'    ' NOTES   : End color of a menu item dithered background. Not used if 
'    '           the background is not dithered
'    Public Property MenuItemBackColorEnd() As Color
'        Get
'            Return __MenuItemBackColorEnd
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemBackColorEnd = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemForeColor
'    ' NOTES   : Color of the menu item text
'    Public Property MenuItemForeColor() As Color
'        Get
'            Return __MenuItemForeColor
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemForeColor = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemForeColorDisabled
'    ' NOTES   : Color of the menu item text when the item is disabled
'    Public Property MenuItemForeColorDisabled() As Color
'        Get
'            Return __MenuItemForeColorDisabled
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemForeColorDisabled = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemBackColorSelected
'    ' NOTES   : Background color when a menu item is selected
'    Public Property MenuItemBackColorSelected() As Color
'        Get
'            Return __MenuItemBackColorSelected
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemBackColorSelected = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemBorderSelected
'    ' NOTES   : Border color when a menu item is selected
'    Public Property MenuItemBorderSelected() As Color
'        Get
'            Return __MenuItemBorderSelected
'        End Get
'        Set(ByVal Value As Color)
'            __MenuItemBorderSelected = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: Font 
'    ' NOTES   : Font object to use for menu items
'    Public Property Font() As Font
'        Get
'            Return ItemFont
'        End Get
'        Set(ByVal Value As Font)
'            ItemFont = Value
'        End Set
'    End Property

'    ' ***********************************************************************
'    ' PROPERTY: MenuItemDithered
'    ' NOTES   : Gets and sets whether the background of the menu item must be
'    '           painted with a gradient brush
'    Public Property MenuItemDithered() As Boolean
'        Get
'            Return __MenuItemDithered
'        End Get
'        Set(ByVal Value As Boolean)
'            __MenuItemDithered = Value
'        End Set
'    End Property

'#End Region

'#Region "Behavior Properties"
'    ' ***********************************************************************
'    ' Public properties defining the behavior of the component

'    ' ***********************************************************************
'    ' PROPERTY: AutoBind
'    'ПРИМЕЧАНИЯ: Указывает, должен ли компонент автоматически 
'    '         подклассифицируйте главное меню формы и меню контекста. 
'    '          Другие меню должны быть вручную связаны. (Используя отражение,
'    '          Вы могли расширить это, чтобы автоматически воздействовать на любого
'    '          Объект меню, явно определенный в пределах формы
'    Public Property AutoBind() As Boolean
'        Get
'            Return __AutoBind
'        End Get
'        Set(ByVal Value As Boolean)
'            __AutoBind = Value
'        End Set
'    End Property

'#End Region

'    ' ***********************************************************************
'    ' METHOD: Init
'    ' INPUT : menu object to subclass
'    ' NOTES : Entry point method, subclasses the menu and iteratively marks 
'    '         child items as ownerdraw. Popup menus are not modified.
'    Public Sub Init(ByVal menu As Menu)

'        ' Return if the menu is null
'        If menu Is Nothing Then
'            Return
'        End If

'        ' Initialize the font object used to render the menu items
'        If (ItemFont Is Nothing) Then
'            ItemFont = New Font("tahoma", 8)
'        End If

'        ' Initialize the hashtable used to hold bitmap/item bindings
'        If menuItemIconCollection Is Nothing Then
'            menuItemIconCollection = New Hashtable
'        End If

'        ' Context menu requires a different treatment
'        If TypeOf menu Is ContextMenu Then
'            HandleChildMenuItems(menu)
'            Return
'        End If

'        ' Iterate on all top-level menus and handle their items
'        For Each popup As MenuItem In menu.MenuItems
'            '                MakeItemOwnerDraw(popup)
'            HandleChildMenuItems(popup)
'        Next

'        'Me.imageListBit = New System.Windows.Forms.ImageList '(Me.components) 'victor

'    End Sub


'    ' ***********************************************************************
'    ' METHOD: AddIcon
'    ' INPUT : menu item, icon file
'    ' ПРИМЕЧАНИЯ: Связывает данный пункт меню с указанным названием{именем} изображения.
'    ' Изображением может быть любой файл, поддержавший GDI + (18x18)
'    'Public Sub AddIcon(ByVal item As MenuItem, ByVal iconName As String)'victor
'    Public Sub AddIcon(ByVal item As MenuItem, ByVal iconName As Integer)

'        ' Add the image to the collection. The menu item object is used 
'        ' as the key of the hash table. The collection is not null by design
'        menuItemIconCollection.Add(item, iconName)

'    End Sub


'    ' ***********************************************************************
'    ' METHOD: Close
'    ' INPUT : void
'    ' NOTES : Frees any resource held by the object
'    Public Sub Close()

'        ' Free the font
'        If Not (ItemFont Is Nothing) Then
'            ItemFont.Dispose()
'        End If

'        ' Clear the icon collection
'        menuItemIconCollection.Clear()
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: HandleChildMenuItems
'    ' INPUT : popup menu 
'    ' ПРИМЕЧАНИЯ: Отметьте текущий пункт, поскольку владелец тянет{рисует}, и рекурсивно обрабатывает
'    ' все дети
'    Private Sub HandleChildMenuItems(ByVal popupMenu As Menu)

'        ' Mark as ownerdraw the current item and iterate on its children
'        For Each item As MenuItem In popupMenu.MenuItems
'            MakeItemOwnerDraw(item)
'            HandleChildMenuItems(item)
'        Next
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: MakeItemOwnerDraw
'    ' INPUT : menu item
'    ' NOTES : Turns the ownerdraw mode on for the specified item
'    Private Sub MakeItemOwnerDraw(ByVal item As MenuItem)
'        item.OwnerDraw = True

'        AddHandler item.DrawItem, AddressOf StdDrawItem
'        AddHandler item.MeasureItem, AddressOf StdMeasureItem
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: StdMeasureItem
'    ' INPUT : menu item, ad hoc structure for measurement
'    ' NOTES : Event handler for the MeasureItem event typical of 
'    '         ownerdraw objects
'    Private Sub StdMeasureItem(ByVal sender As Object, ByVal e As MeasureItemEventArgs)

'        ' Grab a reference to the menu item being measured
'        Dim item As MenuItem = CType(sender, MenuItem)

'        ' If it is a separator, handle differently
'        If (item.Text = "-") Then
'            e.ItemHeight = SeparatorHeight
'            Return
'        End If

'        ' Measure the item text with the current font. The text to 
'        ' measure includes keyboard shortcuts
'        Dim stringSize As SizeF
'        stringSize = e.Graphics.MeasureString(GetEffectiveText(item), ItemFont)

'        ' Set the height and width of the item
'        e.ItemHeight = MenuItemHeight
'        e.ItemWidth = BitmapWidth + HorizontalTextOffset + CInt(stringSize.Width) + RightOffset
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: StdDrawItem
'    ' INPUT : menu item, ad hoc structure for custom drawing
'    ' NOTES : Event handler for the DrawItem event typical of 
'    '         ownerdraw objects
'    Private Sub StdDrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)

'        ' Grab a reference to the item being drawn
'        Dim item As MenuItem = CType(sender, MenuItem)

'        ' Saves helper objects for easier reference
'        Dim g As Graphics = e.Graphics
'        Dim bounds As RectangleF = MakeRectangleF(e.Bounds)
'        Dim itemText As String = item.Text
'        Dim itemState As DrawItemState = e.State

'        ' Define bounding rectangles to use later
'        CreateLayout(bounds)

'        ' Draw the menu item background and text
'        DrawBackground(g, itemState)

'        ' Draw the bitmap leftmost area
'        DrawBitmap(g, item, itemState)

'        ' Draw the text
'        DrawText(g, item, itemState)
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: MakeRectangleF
'    ' INPUT : rectangle expressed with integer coordinates
'    ' OUT   : A RectangleF object
'    ' NOTES : Converts coordinates of the rectangle to Single values
'    Private Function MakeRectangleF(ByVal rect As Rectangle) As RectangleF
'        Dim rectF As RectangleF
'        rectF.X = rect.X
'        rectF.Y = rect.Y
'        rectF.Width = rect.Width
'        rectF.Height = rect.Height
'        Return rectF
'    End Function


'    ' ***********************************************************************
'    ' HELPER: CreateLayout
'    ' INPUT : Base item rectangle (expressed with Single values)
'    ' ПРИМЕЧАНИЯ: Создайте дополнительные прямоугольники, чтобы разграничить полезные области подобно
'    ' область текста, bitmap область, и еще немногие
'    Private Sub CreateLayout(ByVal bounds As RectangleF)

'        ' Define the overall menu item area
'        MenuItemBounds = bounds

'        ' Define the Bitmap area
'        BitmapBounds = MenuItemBounds
'        BitmapBounds.Width = BitmapWidth + 2

'        ' Define the Client area (everything right of the bitmap)
'        ItemBounds = bounds
'        ItemBounds.X = BitmapWidth

'        ' Define the Text area (including text offset)
'        ItemTextBounds.X = CType(BitmapWidth + HorizontalTextOffset, Single)
'        ItemTextBounds.Y = CType(bounds.Y + VerticalTextOffset, Single)
'        ItemTextBounds.Width = CType(bounds.Width, Single)
'        ItemTextBounds.Height = CType(bounds.Height, Single)
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: DrawBitmap
'    ' INPUT : Graphics, menu item, state of the item
'    ' ПРИМЕЧАНИЯ: Отдает bitmap за текущий пункт, принимающий во внимание 
'    ' текущее состояние пункта
'    Private Sub DrawBitmap(ByVal g As Graphics, ByVal item As MenuItem, ByVal itemState As DrawItemState)

'        ' Grab the current state of the menu item 
'        Dim selected, disabled, checked As Boolean
'        selected = (itemState And DrawItemState.Selected) = DrawItemState.Selected
'        disabled = (itemState And DrawItemState.Disabled) = DrawItemState.Disabled
'        checked = (itemState And DrawItemState.Checked) = DrawItemState.Checked

'        ' Declare the bitmap object to use
'        Dim bmp As Bitmap

'        ' Determine the bitmap to use if checked, radio-checked, normal 
'        If (checked) Then
'            If item.RadioCheck Then
'                bmp = ToolboxBitmapAttribute.GetImageFromResource(Me.GetType(), "Bullet.bmp", False)
'            Else
'                bmp = ToolboxBitmapAttribute.GetImageFromResource(Me.GetType(), "Checkmark.bmp", False)
'            End If
'        Else
'            If item.RadioCheck Then
'                bmp = ToolboxBitmapAttribute.GetImageFromResource(Me.GetType(), "BulletEmpty.bmp", False)
'            Else
'                If Not menuItemIconCollection.Contains(item) Then
'                    Return
'                End If
'                'victor
'                'Dim iconName As String = menuItemIconCollection(item).ToString()
'                'If File.Exists(iconName) Then
'                '    bmp = Bitmap.FromFile(iconName)
'                'End If


'                'bmp = mimageListBit.Images(CInt(iconName))
'                bmp = mimageListBit.Images(menuItemIconCollection(item))

'            End If
'        End If


'        ' If no valid bitmap is found, exits
'        If bmp Is Nothing Then
'            Return
'        End If


'        ' Make the bitmap transparent
'        bmp.MakeTransparent()

'        ' Render the bitmap (the bitmap is grayed out if the 
'        ' item is disabled)
'        If (disabled) Then
'            Dim imageAttr As New ImageAttributes
'            imageAttr.SetGamma(0.2F)
'            Dim tmpRect As Rectangle = New Rectangle(BitmapBounds.X + 2, BitmapBounds.Y + 2, BitmapBounds.Width - 2, BitmapBounds.Right - 2)
'            g.DrawImage(bmp, tmpRect, 0, 0, bmp.Width, bmp.Height,
'                GraphicsUnit.Pixel, imageAttr)
'            imageAttr.ClearGamma()
'        Else
'            g.DrawImage(bmp, BitmapBounds.X + 2, BitmapBounds.Y + 2)
'        End If

'        ' Free the resource
'        bmp.Dispose()
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: DrawBackground
'    ' ВХОД{ВКЛАД}: Графика, состояние пункта
'    ' ПРИМЕЧАНИЯ: Заполняет фон пункта, включая bitmap область
'    Private Sub DrawBackground(ByVal g As Graphics, ByVal itemState As DrawItemState)
'        ' Declare some helper variables
'        Dim backBrush As Brush = Brushes.Gold
'        Dim bitmapBrush As Brush = Brushes.Gold
'        Dim borderPen As Pen = Pens.Gold
'        Dim selected, disabled, paintBitmapArea As Boolean
'        Dim rectToPaint As Rectangle

'        ' Determine the state of the item
'        selected = (itemState And DrawItemState.Selected) = DrawItemState.Selected
'        disabled = (itemState And DrawItemState.Disabled) = DrawItemState.Disabled

'        ' Determine whether the bitmap vertical strip must be created
'        paintBitmapArea = Not BitmapBackColor.Equals(Color.Empty)
'        If paintBitmapArea Then
'            rectToPaint = Rectangle.Round(ItemBounds)
'        Else
'            rectToPaint = Rectangle.Round(MenuItemBounds)
'        End If

'        ' Determine the brushes to use based on the state
'        If selected And Not disabled Then
'            backBrush = New SolidBrush(MenuItemBackColorSelected)
'            borderPen = New Pen(MenuItemBorderSelected)
'        Else
'            If MenuItemDithered Then
'                backBrush = New LinearGradientBrush(rectToPaint,
'                    MenuItemBackColorStart,
'                    MenuItemBackColorEnd,
'                    LinearGradientMode.Horizontal)
'                borderPen = Nothing
'            Else
'                backBrush = New SolidBrush(MenuItemBackColorStart)
'            End If
'        End If

'        ' Fill the area
'        ' ОБРАТИТЕ ВНИМАНИЕ:
'        ' Когда Вы заполняете область больший чем линейный градиент,
'        ' цвет конца используется, чтобы заполнить это. Это гарантирует, что мы также имеем 
'        ' bitmap область, окрашенная с цветом конца градиента.
'        ' Это - для свободного

'        If (selected And Not disabled) Then
'            rectToPaint = Rectangle.Round(MenuItemBounds)
'            g.FillRectangle(backBrush, rectToPaint)

'            ' Draw border
'            rectToPaint.Width -= 1
'            rectToPaint.Height -= 1
'            g.DrawRectangle(borderPen, rectToPaint)
'        Else
'            g.FillRectangle(backBrush, rectToPaint)
'            If paintBitmapArea Then
'                bitmapBrush = New SolidBrush(BitmapBackColor)
'                g.FillRectangle(bitmapBrush, BitmapBounds)
'            End If
'        End If

'        ' Cleanup objects
'        If Not (bitmapBrush Is Nothing) Then
'            bitmapBrush.Dispose()
'        End If
'        backBrush.Dispose()
'        If Not (borderPen Is Nothing) Then
'            borderPen.Dispose()
'        End If
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: DrawText
'    ' INPUT : Graphics, menu item 
'    ' NOTES : Paints the text of the menu item 
'    Private Sub DrawText(ByVal g As Graphics, ByVal item As MenuItem, ByVal itemState As DrawItemState)
'        ' Declare the foreground brush
'        Dim foreBrush As Brush

'        ' Handle the separator as a special case; then return
'        If item.Text = "-" Then
'            DrawSeparator(g)
'            Return
'        End If

'        ' Determine the foreground brush to use based on the state
'        ' NOTE: you could use gradients too
'        If Not item.Enabled Then
'            foreBrush = New SolidBrush(MenuItemForeColorDisabled)
'        Else
'            foreBrush = New SolidBrush(MenuItemForeColor)
'        End If

'        ' If default item, use bold
'        Dim tmpFont As Font
'        Dim defaultItem As Boolean
'        defaultItem = (itemState And DrawItemState.Default) = DrawItemState.Default
'        If (defaultItem) Then
'            tmpFont = New Font(ItemFont, FontStyle.Bold)
'        Else
'            tmpFont = ItemFont
'        End If

'        ' Get text and keyboard shortcut info to paint
'        Dim textToPaint As String = GetEffectiveText(item)

'        ' Text and shortcut are null-separated. Split the string in two parts
'        Dim parts() As String = textToPaint.Split(Chr(0))

'        ' Format the string(s) to render
'        Dim strFormat As New StringFormat
'        strFormat.HotkeyPrefix = HotkeyPrefix.Show
'        strFormat.LineAlignment = StringAlignment.Center

'        ' Paint text
'        If parts.Length = 1 Then
'            ' Paint when no shortcut info is found
'            g.DrawString(textToPaint, tmpFont, foreBrush, ItemTextBounds, strFormat)
'        Else
'            ' Paint text when shortcut info is found
'            g.DrawString(parts(0), tmpFont, foreBrush, ItemTextBounds, strFormat)

'            ' Paint right-aligned shortcut info
'            Dim rect As New RectangleF(ItemBounds.X, ItemBounds.Y, ItemBounds.Width, ItemBounds.Height)
'            rect.Width -= BitmapWidth + HorizontalTextOffset + RightOffset
'            strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
'            g.DrawString(parts(1), tmpFont, foreBrush, rect, strFormat)
'        End If

'        ' Cleanup resources
'        foreBrush.Dispose()
'    End Sub


'    ' ***********************************************************************
'    ' HELPER: GetEffectiveText
'    ' INPUT : menu item 
'    ' OUT   : text + expanded shortcut info
'    ' ПРИМЕЧАНИЯ: Добавляет сокращенную информацию к тексту пункта меню. Сокращенная информация
'    ' не содержит + сепараторы. Функция добавляет их все
'    ' и отделяет эти две части с пустым указателем [Chr (0)]

'    Private Function GetEffectiveText(ByVal item As MenuItem) As String
'        Dim finalText As String = item.Text
'        Dim tmp As String

'        ' Separates each component in the shortcut string with a +
'        ' A typical shortcut is CtrlO. We insert a + after each upper 
'        ' case character 
'        If item.ShowShortcut And item.Shortcut <> Shortcut.None Then
'            Dim buf As String = item.Shortcut.ToString()
'            tmp = buf
'            For index As Integer = 0 To buf.Length - 1
'                If Char.IsUpper(buf.Chars(index)) Then
'                    If index > 0 Then
'                        tmp = tmp.Insert(index, "+")
'                    End If
'                End If
'            Next
'            finalText += Chr(0) + tmp
'        End If

'        Return finalText
'    End Function


'    ' ***********************************************************************
'    ' HELPER: DrawSeparator
'    ' INPUT : Graphics
'    ' NOTES : Paints a separator line 1 pixel thin
'    Private Sub DrawSeparator(ByVal g As Graphics)
'        Dim sepPen As Pen = New Pen(MenuItemForeColorDisabled, 1)
'        g.DrawLine(sepPen, ItemTextBounds.X, ItemTextBounds.Y,
'            ItemTextBounds.X + ItemTextBounds.Right,
'            ItemTextBounds.Y)
'        sepPen.Dispose()
'    End Sub


'End Class
'#End Region

'#Region "TextBoxContextMenu Class"
'Public Class TextBoxContextMenu : Inherits ContextMenu
'    Public MenuItemUndo As MenuItem
'    Public MenuItemCut As MenuItem
'    Public MenuItemCopy As MenuItem
'    Public MenuItemDelete As MenuItem
'    Public MenuItemPaste As MenuItem
'    Public MenuItemSelectAll As MenuItem


'    Public Sub New()
'        MenuItemUndo = Me.MenuItems.Add("Undo", AddressOf TextBox_Undo)
'        Me.MenuItems.Add("-")
'        MenuItemCut = Me.MenuItems.Add("Cut", AddressOf TextBox_Cut)
'        MenuItemCopy = Me.MenuItems.Add("Copy", AddressOf TextBox_Copy)
'        MenuItemPaste = Me.MenuItems.Add("Paste", AddressOf TextBox_Paste)
'        MenuItemDelete = Me.MenuItems.Add("Delete", AddressOf TextBox_Delete)
'        Me.MenuItems.Add("-")
'        MenuItemSelectAll = Me.MenuItems.Add("Select All", AddressOf TextBox_SelectAll)
'    End Sub

'    Private Sub ConfigureContextMenu(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Popup
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        MenuItems(0).Enabled = ctl.CanUndo
'        MenuItems(7).Enabled = (ctl.SelectedText <> ctl.Text)
'        MenuItems(2).Enabled = (ctl.SelectionLength > 0) ' cut
'        MenuItems(3).Enabled = (ctl.SelectionLength > 0) ' copy
'        MenuItems(4).Enabled = Clipboard.GetDataObject().GetDataPresent(GetType(String))  ' paste
'        MenuItems(5).Enabled = (ctl.SelectionLength > 0) ' delete
'    End Sub

'    Private Sub TextBox_Undo(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        ctl.Undo()
'        ctl.ClearUndo()
'    End Sub
'    Private Sub TextBox_Cut(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        ctl.Cut()
'    End Sub
'    Private Sub TextBox_Copy(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        ctl.Copy()
'    End Sub
'    Private Sub TextBox_Paste(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        ctl.Paste()
'    End Sub
'    Private Sub TextBox_Delete(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        Dim selText As String = ctl.SelectedText
'        Dim selStart As Integer = ctl.SelectionStart
'        Dim buf As String = ctl.Text
'        buf = buf.Replace(selText, "")
'        ctl.Text = buf
'        ctl.SelectionStart = selStart
'    End Sub
'    Private Sub TextBox_SelectAll(ByVal sender As Object, ByVal e As EventArgs)
'        Dim ctl As TextBox = CType(Me.SourceControl, TextBox)
'        ctl.SelectAll()
'    End Sub
'End Class
'#End Region

'End Namespace

'Private gMenu As GraphicMenu
'Private gMenu, gMenuTree As GraphicMenu


'gMenu = New GraphicMenu
'gMenu.Init(Me.Menu)
'gMenu.BitmapBackColor = conBitmapBackColor
'gMenu.MenuItemBackColorStart = conMenuItemBackColorStart
'gMenu.MenuItemBackColorEnd = conMenuItemBackColorEnd
'gMenu.imageListBit = ImageListMenu

'gMenu.AddIcon(mnuFileNew, 0)
'gMenu.AddIcon(mnuFileOpen, 1)
'gMenu.AddIcon(mnuFileSave, 2)
''gMenu.AddIcon(mnuFileSaveAs()
'gMenu.AddIcon(mnuFileExit, 3)
''gMenu.AddIcon(mnuRecentFile1()
''gMenu.AddIcon(mnuRecentFile2()
''gMenu.AddIcon(mnuRecentFile3()
''gMenu.AddIcon(mnuRecentFile4()
''gMenu.AddIcon(mnuRecentFile5()

'gMenu.AddIcon(mnuEditCut, 4)
'gMenu.AddIcon(mnuEditCopy, 5)
'gMenu.AddIcon(mnuEditPaste, 6)
'gMenu.AddIcon(mnuEditDelete, 7)
''gMenu.AddIcon(mnuEditSelectAll()
'gMenu.AddIcon(mnuEditTime, 8)

'gMenu.AddIcon(mnuSearchFind, 9)
'gMenu.AddIcon(mnuSearchFindNext, 10)

'gMenu.AddIcon(mnuOptionsToolbar, 11)
'gMenu.AddIcon(mnuOptionsLaunch, 12)

'gMenu.AddIcon(mnuWindowCascade, 13)
'gMenu.AddIcon(mnuWindowTileHorizontal, 14)
'gMenu.AddIcon(mnuWindowTileVertical, 15)

'Private Sub frmMainMDI_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.FormClosing
'    '********
'    'gMenu.Close()
'gMenu.Close()
'gMenuTree.Close()
'End Sub