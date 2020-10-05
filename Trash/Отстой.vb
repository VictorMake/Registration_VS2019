Module Отстой

#Region "События пользователя"

    'Private WithEvents mText As TimerState
    'Private Sub Form1_Load(ByVal sender As Object, _
    '                   ByVal e As System.EventArgs) _
    '                   Handles MyBase.Load
    '    Button1.Text = "Start"
    '    mText = New TimerState
    'End Sub
    'Private Sub Button1_Click(ByVal sender As System.Object, _
    '                          ByVal e As System.EventArgs) _
    '                          Handles Button1.Click
    '    mText.StartCountdown(10.0, 0.1)
    'End Sub

    'Private Sub mText_ChangeText() Handles mText.Finished
    '    TextBox1.Text = "Done"
    'End Sub

    'Private Sub mText_UpdateTime(ByVal Countdown As Double) _
    '                             Handles mText.UpdateTime
    '    TextBox1.Text = Format(Countdown, "##0.0")
    '    ' Use DoEvents to allow the display to refresh.
    '    My.Application.DoEvents()
    'End Sub

    'Class TimerState
    '    Public Event UpdateTime(ByVal Countdown As Double)
    '    Public Event Finished()
    '    Public Sub StartCountdown(ByVal Duration As Double, _
    '                              ByVal Increment As Double)
    '        Dim Start As Double = DateAndTime.Timer
    '        Dim ElapsedTime As Double = 0

    '        Dim SoFar As Double = 0
    '        Do While ElapsedTime < Duration
    '            If ElapsedTime > SoFar + Increment Then
    '                SoFar += Increment
    '                RaiseEvent UpdateTime(Duration - SoFar)
    '            End If
    '            ElapsedTime = DateAndTime.Timer - Start
    '        Loop
    '        RaiseEvent Finished()
    '    End Sub
    'End Class

    '    Class Raiser
    '        Public Event E1(ByVal Count As Integer)

    '        Public Sub Raise()
    '            Static RaiseCount As Integer = 0

    '            RaiseCount += 1
    '            RaiseEvent E1(RaiseCount)
    '        End Sub
    '    End Class

    'Module Test
    '        Private WithEvents x As Raiser

    '        Private Sub E1Handler(ByVal Count As Integer) Handles x.E1
    '            Console.WriteLine("Raise #" & Count)
    '        End Sub

    '        Public Sub Main()
    '            x = New Raiser
    '            x.Raise()        ' Prints "Raise #1".
    '            x.Raise()        ' Prints "Raise #2".
    '            x.Raise()        ' Prints "Raise #3".
    '        End Sub
    '    End Module
#End Region


#Region "Отстой"

#Region "Пример добавления пунктов в меню"


#Region "Event Handlers"
    ''пример показа справочной информации в статусной строке 
    'Private Sub MenuItem_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles FileMenu.MouseEnter, NewMenuItem.MouseEnter, Option1.MouseEnter, MoreOptions.MouseEnter, MoreOptions1.MouseEnter, MoreOptions2.MouseEnter, MoreOptions3.MouseEnter, Option2.MouseEnter, Option3.MouseEnter, OpenMenuItem.MouseEnter, ViewToolStripMenuItem1.MouseEnter, StatusStripOption.MouseEnter, CheckedListMenu.MouseEnter, AddOptionMenuItem.MouseEnter, RemoveOptionMenuItem.MouseEnter
    '    Dim selected As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
    '    Status.Text = selected.Text
    'End Sub

    'Private Sub MenuItem_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles FileMenu.MouseLeave, NewMenuItem.MouseLeave, Option1.MouseLeave, MoreOptions.MouseLeave, MoreOptions1.MouseLeave, MoreOptions2.MouseLeave, MoreOptions3.MouseLeave, Option2.MouseLeave, Option3.MouseLeave, OpenMenuItem.MouseLeave, ViewToolStripMenuItem1.MouseLeave, StatusStripOption.MouseLeave, CheckedListMenu.MouseLeave, AddOptionMenuItem.MouseLeave, RemoveOptionMenuItem.MouseLeave
    '    Status.Text = ""
    'End Sub

    'Private Sub MenuOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    'сначало все пункты выключим
    '    For Each item As Object In CheckedListMenu.DropDownItems
    '        If (TypeOf item Is ToolStripMenuItem) Then
    '            Dim itemObject As ToolStripMenuItem = CType(item, ToolStripMenuItem)
    '            itemObject.Checked = False
    '        End If
    '    Next
    '    'затем включим нужный
    '    Dim selectedItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
    '    selectedItem.Checked = True
    'End Sub

    'Private Sub RemoveOptionMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveOptionMenuItem.Click
    '    Dim itemToRemove As ToolStripMenuItem = Nothing
    '    '3 пункта надо зарезервировать для добавить, удалить и разделитель
    '    If CheckedListMenu.DropDownItems.Count > 3 Then
    '        itemToRemove = CType(CheckedListMenu.DropDownItems(CheckedListMenu.DropDownItems.Count - 1), ToolStripMenuItem)
    '        Dim removeAt As Integer = CheckedListMenu.DropDownItems.Count - 1
    '        If itemToRemove.Checked And CheckedListMenu.DropDownItems.Count > 4 Then
    '            Dim itemToCheck As ToolStripMenuItem = CType(CheckedListMenu.DropDownItems(CheckedListMenu.DropDownItems.Count - 2), ToolStripMenuItem)
    '            itemToCheck.Checked = True
    '        End If
    '        CheckedListMenu.DropDownItems.RemoveAt(removeAt)
    '    End If
    'End Sub

#End Region

    'Private Sub AddOptionMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddOptionMenuItem.Click
    '    AddOption()
    'End Sub

    'Private Sub AddOption()
    '    Dim newOption As New ToolStripMenuItem()
    '    newOption.Checked = False
    '    newOption.CheckOnClick = True
    '    newOption.Text = "Option " & (CheckedListMenu.DropDownItems.Count - 2).ToString()

    '    If CheckedListMenu.DropDownItems.Count = 3 Then
    '        newOption.Checked = True
    '    End If

    '    ' Add the event handlers for the Click and MouseEnter events of the new option.
    '    AddHandler newOption.Click, AddressOf Me.MenuOption_Click
    '    AddHandler newOption.MouseEnter, AddressOf Me.MenuItem_MouseEnter
    '    AddHandler newOption.MouseLeave, AddressOf Me.MenuItem_MouseLeave

    '    CheckedListMenu.DropDownItems.Add(newOption)
    'End Sub


    'Private Sub DropDownColorItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
    '    Select Case menuItem.Name
    '        Case "Red"
    '            'Me.BackColor = Color.Red
    '            ЦветФонаИндикаторов(Color.Maroon)
    '        Case "Yellow"
    '            'Me.BackColor = Color.Yellow
    '            ЦветФонаИндикаторов(Color.Olive)
    '        Case "Green"
    '            'Me.BackColor = Color.Green
    '            ЦветФонаИндикаторов(Color.DarkGreen)
    '    End Select
    '    MenuStrip1.Items("colorSplitButton").Text = menuItem.Name
    'End Sub

    'Private Sub ColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim button As ToolStripButton = CType(sender, ToolStripButton)
    '    Select Case button.Name
    '        Case "Red"
    '            'Me.BackColor = Color.Red
    '            ЦветИндикаторов(Color.Red)
    '        Case "Yellow"
    '            'Me.BackColor = Color.Yellow
    '            ЦветИндикаторов(Color.Yellow)
    '        Case "Green"
    '            'Me.BackColor = Color.Green
    '            ЦветИндикаторов(Color.Lime)
    '    End Select
    '    MenuStrip1.Items("colorSplitButton").Text = button.Name
    'End Sub

    'Private Sub SplitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    'Me.BackColor = Color.Red
    '    ЦветИндикаторов(Color.Red)
    'End Sub

    'Private Sub ЦветИндикаторов(ByVal OnColor As System.Drawing.Color)
    '    mOnColor = OnColor
    '    For НомерПорта As Integer = 0 To 7
    '        For НомерБита As Integer = 0 To 7
    '            CType(TableLayoutPanelPort.Controls.Item("TableLayoutPanelPort" & НомерПорта.ToString).Controls.Item("LedLedPort" & НомерПорта.ToString & "Line" & НомерБита.ToString), NationalInstruments.UI.WindowsForms.Led).OnColor = OnColor
    '        Next НомерБита
    '    Next НомерПорта
    'End Sub

    'Private Sub ЦветФонаИндикаторов(ByVal OffColor As System.Drawing.Color)
    '    mOffColor = OffColor
    '    For НомерПорта As Integer = 0 To 7
    '        For НомерБита As Integer = 0 To 7
    '            If CType(TableLayoutPanelPort.Controls.Item("TableLayoutPanelPort" & НомерПорта.ToString).Controls.Item("LabelPort" & НомерПорта.ToString), Label).Text <> "P" & НомерПорта.ToString Then
    '                CType(TableLayoutPanelPort.Controls.Item("TableLayoutPanelPort" & НомерПорта.ToString).Controls.Item("LedLedPort" & НомерПорта.ToString & "Line" & НомерБита.ToString), NationalInstruments.UI.WindowsForms.Led).OffColor = OffColor
    '            Else
    '                CType(TableLayoutPanelPort.Controls.Item("TableLayoutPanelPort" & НомерПорта.ToString).Controls.Item("LedLedPort" & НомерПорта.ToString & "Line" & НомерБита.ToString), NationalInstruments.UI.WindowsForms.Led).OffColor = Color.Silver
    '            End If
    '        Next НомерБита
    '    Next НомерПорта
    'End Sub
#End Region

#End Region '"Отстой"

#Region "Создание формы из XML файла"

    ' This subroutine handles the btnCreateSurvey.Click event and creates
    ' a new frmPanelBaseForm. The controls that are generated are added to the
    ' created survey form. There are no event handlers associated with the 
    ' created controls.
    ' The created form is fairly general, and creates a survey with questions
    ' that are based on information provided by the Questions.xml document.
    ' By changing, adding, or removing nodes in the XML document, you can 
    ' change the structure and form of the survey.
    'Private Sub btnCreateSurvey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateSurvey.Click

    '    ' Create a new Survey Form to display to the User.
    '    Dim survey As New PanelBaseForm

    '    ' Get the controls collection of the Survey form.
    '    Dim surveyControls As Control.ControlCollection = survey.PanelBaseFormControls

    '    ' Set a location for the first control.
    '    Dim location As Point
    '    location = New Point(10, 10)

    '    ' Create an XML document to read in the survey questions.
    '    Dim xr As New Xml.XmlDocument
    '    xr.LoadXml(My.Resources.Questions)

    '    ' Get the Tag used for each of the Controls we'll create. This may
    '    ' be useful later, if the example was extended to break apart
    '    ' different types of questions/responses for analysis.
    '    Dim myTag As String = xr.SelectSingleNode("//survey").Attributes("name").Value

    '    ' Set the Title on the survey form to the Display Name of the Survey.
    '    survey.SurveyTitle = xr.SelectSingleNode("//survey").Attributes("displayName").Value

    '    ' Create an XmlNodeList to contain each of the questions. Fill it.
    '    Dim nodeList As Xml.XmlNodeList
    '    nodeList = xr.GetElementsByTagName("question")

    '    ' Create a temporary XML Node to use when retrieving information
    '    ' about the nodes in the nodeList just created.
    '    Dim myNode As XmlNode
    '    For Each myNode In nodeList
    '        If Not myNode.Attributes Is Nothing Then
    '            ' Determine what type of control should be created. Pass
    '            ' in the required information, including the Controls collection
    '            ' from the frmPanelBaseForm form.
    '            Select Case myNode.Attributes("type").Value
    '                Case "dropdown"
    '                    location = Survey_AddComboBox(myNode, surveyControls, _
    '                        location, myTag)
    '                Case "multilist"
    '                    location = Survey_AddListBox(myNode, surveyControls, _
    '                        location, myTag, True)
    '                Case "text"
    '                    location = Survey_AddTextBox(myNode, surveyControls, _
    '                        location, myTag)
    '                Case "radio"
    '                    location = Survey_AddRadioButtons(myNode, surveyControls, _
    '                        location, myTag)
    '            End Select
    '        End If
    '    Next

    '    ' Set the size of the form, based off of how many controls
    '    ' have been placed on the form, and their dimensions.
    '    survey.Width = location.X + controlWidth + 30
    '    ' Add a bit extra to leave room for the OK and Cancel buttons.
    '    survey.Height = location.Y + 75

    '    ' Show the form.  You can also use the Show() method if you like.
    '    survey.ShowDialog()

    '    ' Show the response to the user.
    '    MsgBox(survey.SurveyResponse, MsgBoxStyle.OKOnly, Me.Text)
    'End Sub
#End Region


#Region "SerializeObject"
    'SerializeObject(strTempFileSoap)
    'DeserializeObject(strTempFileSoap)

    'Private Sub SerializeObject(ByVal filename As String)
    '    Console.WriteLine("Writing With TextWriter")

    '    Dim serializer As New XmlSerializer(GetType(OrderedItem))

    '    'SoapFormatter
    '    Dim i As New OrderedItem

    '    With i
    '        .ItemName = "Widget"
    '        .Description = "Regular Widget"
    '        .Quantity = 10
    '        .UnitPrice = CDec(2.3)
    '        .Calculate()
    '    End With

    '    ' Create a StreamWriter to write with. First create a FileStream
    '    ' object, and create the StreamWriter specifying an Encoding to use. 
    '    Dim fs As New FileStream(filename, FileMode.Create)
    '    Dim writer As New StreamWriter(fs, New UTF8Encoding())
    '    ' Serialize using the XmlTextWriter.
    '    serializer.Serialize(writer, i)
    '    writer.Close()
    'End Sub


    'Private Sub DeserializeObject(ByVal filename As String)
    '    Console.WriteLine("Reading with Stream")
    '    ' Create an instance of the XmlSerializer.
    '    Dim serializer As New XmlSerializer(GetType(OrderedItem))
    '    ' Reading the XML document requires a FileStream.
    '    Dim reader As New FileStream(filename, FileMode.Open)

    '    ' Declare an object variable of the type to be deserialized.
    '    Dim i As OrderedItem

    '    ' the Deserialize method to restore the object's state.
    '    i = CType(serializer.Deserialize(reader), OrderedItem)

    '    ' Write out the properties of the object.
    '    Console.Write(i.ItemName & ControlChars.Tab & _
    '                  i.Description & ControlChars.Tab & _
    '                  i.UnitPrice & ControlChars.Tab & _
    '                  i.Quantity & ControlChars.Tab & _
    '                  i.LineTotal)
    'End Sub

    '' This is the class that will be serialized.
    'Public Class OrderedItem
    '    Public ItemName As String
    '    Public Description As String
    '    Public UnitPrice As Decimal
    '    Public Quantity As Integer
    '    Public LineTotal As Decimal

    '    'A custom method used to calculate price per item.
    '    Public Sub Calculate()
    '        LineTotal = UnitPrice * Quantity
    '    End Sub
    'End Class

    'Private Shared _serializer As System.Xml.Serialization.XmlSerializer 'XmlSerializer

    'Private Shared ReadOnly Property Serializer() As System.Xml.Serialization.XmlSerializer
    '    Get
    '        If _serializer Is Nothing Then
    '            _serializer = New System.Xml.Serialization.XmlSerializer(GetType(Switch))
    '        End If
    '        Return _serializer
    '    End Get
    'End Property

    ' глубокое клонирование экземпляра
    ' метод медленный, но в данном случае это неважно
    'Dim newInstance As Config
    'Dim o As Switch = New Switch("Проба")
    'Using ms As New MemoryStream()
    '    Serializer.Serialize(ms, o)
    '    ms.Flush()
    '    ms.Position = 0
    '    propSwitch = DirectCast(Serializer.Deserialize(ms), Switch)
    '    'newInstance.ConfigChanged = _instance.ConfigChanged
    'End Using
    ''Return newInstance
#End Region

#Region "AddDirectories"
    'Private Sub AddDirectories(ByVal strDirectories As String) '(ByVal tn As TreeNode)
    '    Dim strPath As String = strDirectories 'tn.FullPath
    '    Dim diDirectory As New DirectoryInfo(strPath)
    '    Dim adiDirectories() As DirectoryInfo
    '    Try
    '        ' Get an array of all sub-directories as DirectoryInfo objects.
    '        adiDirectories = diDirectory.GetDirectories()
    '    Catch exp As Exception
    '        Exit Sub
    '    End Try

    '    Dim di As DirectoryInfo
    '    For Each di In adiDirectories
    '        ' Create a child node for every sub-directory, passing in the directory
    '        ' name and the images its node will use.
    '        'Dim tnDir As New TreeNode(di.Name, 1, 2)
    '        ' Add the new child node to the parent node.
    '        'tn.Nodes.Add(tnDir)

    '        ' We could now fill up the whole tree by recursively calling 
    '        ' AddDirectories():
    '        '
    '        '   AddDirectories(tnDir)
    '        '
    '        ' This is way too slow, however. Give it a try!
    '        AddOptionMenuPanel(di.Name)
    '    Next
    'End Sub


    'Private Sub ShowFiles(ByVal strDirectory As String)
    '    'Items.Clear()
    '    Dim diDirectories As New DirectoryInfo(strDirectory)
    '    Dim afiFiles() As FileInfo

    '    Try
    '        ' вызвать метод GetFiles чтобы получить массив файлов в директории
    '        afiFiles = diDirectories.GetFiles()
    '    Catch
    '        Return
    '    End Try

    '    Dim fi As FileInfo
    '    For Each fi In afiFiles
    '        ' Create ListViewItem.
    '        'Dim lvi As New ListViewItem(fi.Name)
    '        ' определить ImageIndex базовый от расширения.
    '        Select Case Path.GetExtension(fi.Name).ToUpper()
    '            Case ".XML"
    '                AddOptionMenuPanel(fi.Name.Replace(".xml", ""))
    '                'lvi.ImageIndex = 1
    '                'Case Else
    '                'lvi.ImageIndex = 0
    '        End Select
    '        'добавить длину и время последней модификации
    '        ' Add file length and last modified time sub-items.
    '        'lvi.SubItems.Add(fi.Length.ToString("N0"))
    '        'lvi.SubItems.Add(fi.LastWriteTime.ToString())
    '        'добавить аттрибут
    '        'Dim strAttr As String = ""
    '        'If (fi.Attributes And FileAttributes.Archive) <> 0 Then
    '        '    strAttr += "A"
    '        'End If
    '        'If (fi.Attributes And FileAttributes.Hidden) <> 0 Then
    '        '    strAttr += "H"
    '        'End If
    '        'If (fi.Attributes And FileAttributes.ReadOnly) <> 0 Then
    '        '    strAttr += "R"
    '        'End If
    '        'If (fi.Attributes And FileAttributes.System) <> 0 Then
    '        '    strAttr += "S"
    '        'End If
    '        'lvi.SubItems.Add(strAttr)
    '        ' добавить укомплектованный ListViewItem в FileListView.
    '        'Items.Add(lvi)
    '    Next fi
    'End Sub
#End Region

    '    Q. Как при переборе коллекции удалить один из элементов, но при этом продолжить цикл?

    'A. Можно создать промежуточную коллекцию ключей, которую и перебирать.

    'foreach (string key in new List<string>(_coll.Keys))
    '{
    '    bool result = method(_coll[key]); // Здесь вызываем метод.
    '    if (result == false)
    '        _coll.Remove(key);
    '}


    'Public Sub Linq5()
    '    Dim digits = New String() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}

    '    Dim shortDigits = digits.Where(Function(digit, index) digit.Length < index)

    '    Console.WriteLine("Short digits:")
    '    For Each d In shortDigits
    '        Console.WriteLine("The word " & d & " is shorter than its value.")
    '    Next
    'End Sub

    'Public Sub Linq9()
    '    'Dim words = New String() {"aPPLE", "BlUeBeRrY", "cHeRry"}

    '    'Dim upperLowerWords = From word In words _
    '    '              Select Upper = word.ToUpper(), Lower = word.ToLower()

    '    Dim upperLowerWords = From word In words _
    '                         Select n = "", Dev = word.Replace("Dev", "")

    '    ' Dev = word.Substring(1, "dev".Length)

    '    'Alternate syntax
    '    'Dim upperLowerWords = From w In words _
    '    '                      Select New With {.Upper = w.ToUpper(), .Lower = w.ToLower()}

    '    For Each ul In upperLowerWords
    '        'Console.WriteLine("Uppercase: " & ul.Upper & ", Lowercase: " & ul.Lower)
    '        'Console.WriteLine("Uppercase: " & ul.Upper & ", Lowercase: " & ul.Lower)
    '        physicalChannelComboBox.Items.Add(ul.Dev) ' & ul.dsd)
    '    Next
    'End Sub

    'Public Sub Linq13()
    '    'Dim numbers As Integer() = {5, 4, 1, 3, 9, 8, 6, 7, 2, 0}
    '    'Dim digits As String() = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}

    '    Dim lowNums = From word In words _
    '                  Where word.IndexOf("Dev1") _
    '                  Select word

    '    For Each num In lowNums
    '        'Console.WriteLine(num)
    '        physicalChannelComboBox.Items.Add(num)

    '    Next
    'End Sub

    'Public Sub Linq59()

    '    'Dim strings = New String() {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}

    '    Dim FirstO = Aggregate str In words _
    '                 Into First(str.IndexOf("Dev1"))

    '    'Alternative Syntax
    '    'Dim FirstO = strings.First(Function(s) s(0) = "o"c)

    '    'Console.WriteLine("A string starting with 'o': " & FirstO)
    '    For Each ul In FirstO
    '        physicalChannelComboBox.Items.Add(ul) ' & ul.dsd)
    '    Next
    'End Sub

    'Public Sub Linq41()
    '    'Dim words = New String() {"blueberry", "chimpanzee", "abacus", "banana", "apple", "cheese"}

    '    'Dim wordGroups = From word In words _
    '    '                 Group word By Key = "2" Into Group _
    '    '                 Select FirstLetter = Key, WordGroup = Group

    '    Dim I As Integer
    '    If (physicalPortComboBox.Items.Count > 0) Then
    '        physicalPortComboBox.SelectedIndex = 0
    '        For I = 0 To physicalPortComboBox.Items.Count - 1


    '            'Dim wordGroups = From word In words _
    '            '                    Group word By Key = physicalPortComboBox.Items(2).ToString Into Group _
    '            '                    Select FirstLetter = Key, WordGroup = Group
    '            'Dim categoryCounts = From prod In products _
    '            '         Group prod By prod.Category Into Count() _
    '            '         Select Category, ProductCount = Count


    '            Dim wordGroups = From word In words _
    '                      Where word.IndexOf(physicalPortComboBox.Items(I).ToString) <> -1 _
    '                      Group word By Key = "line" Into Count() _
    '                      Select ProductCount = Count 'FirstLetter = Key,

    '            'Group word By word.Contains("line") Into Count() _

    '            For Each w In wordGroups
    '                physicalChannelComboBox.Items.Add(w) ' & ul.dsd)
    '            Next
    '            'For Each g In wordGroups
    '            '    Console.WriteLine("Words that start with the letter '" & g.FirstLetter & "':")
    '            '    For Each w In g.WordGroup
    '            '        physicalChannelComboBox.Items.Add(w)
    '            '    Next
    '            'Next

    '            'For Each g In wordGroups
    '            '    Console.WriteLine("Words that start with the letter '" & g.FirstLetter & "':")
    '            '    For Each w In g.WordGroup
    '            '        physicalChannelComboBox.Items.Add(w)
    '            '    Next
    '            'Next

    '        Next
    '    End If

    'End Sub



#Region "Friend Class FormsPanelManager"

    'Private Shared Sub PanelBaseForm_SaveWhileClosingCancelled(ByVal sender As Object, ByVal e As System.EventArgs)
    'Это событие будет обработано если пользователь нажал cancel
    'при вопросе сохранения несохраненного документа
    '    If m_ShutdownInProgress Then
    '        ' Only change our internal value if
    '        ' we're actually in the process of shutting down.
    '        Forms.m_CancelExit = True
    '    End If
    'End Sub

    'Private Shared Sub PanelBaseForm_ExitApplication(ByVal sender As Object, ByVal e As System.EventArgs)
    'Это событие будет обработано если пользователь нажал Exit menu command.
    '    Forms.ExitApp()
    'End Sub


    'Public Shared Sub ExitApp()
    '    Try
    '        'm_ShutdownInProgress = True

    '        'выгрузить за раз все формы которые должны быть закрыты
    '        Dim frm As frmBasePanelMotorist

    '        'Dim i As Integer

    '        'цикл сквозь коллекцию шагая по одной за раз
    '        'спрашивая каждый раз закрывая саму себя.
    '        'Только спрашивая формы которые имеют несохраненные данные
    '        'чтобы проверить если пользователь нажал Cancel, если он не хочет закрытие формы

    '        'было
    '        ''For i = m_КоллекцияПанелейМоториста.Count To 1 Step -1
    '        ''    frm = CType(m_КоллекцияПанелейМоториста(i), PanelBaseForm)
    '        ''    If frm.Dirty Then
    '        ''        frm.Close()
    '        ''    End If

    '        ''    ' проверка внутреннего флага в случае
    '        ''    ' если пользователь хочет остановить выгрузку
    '        ''    If m_CancelExit = True Then
    '        ''        m_CancelExit = False
    '        ''        Exit Sub
    '        ''    End If
    '        ''Next
    '        'стало
    '        'Dim srtFormsName As Dictionary(Of String, PanelBaseForm).KeyCollection = m_КоллекцияПанелейМоториста.Keys.ToString
    '        Dim srtFormsName As New List(Of String)
    '        For Each strName As String In m_КоллекцияПанелейМоториста.Keys
    '            srtFormsName.Add(strName)
    '        Next

    '        For Each strName As String In srtFormsName
    '            frm = m_КоллекцияПанелейМоториста.Item(strName)
    '            'If frm.Dirty Then
    '            frm.Close()
    '            'End If

    '            ' проверка внутреннего флага в случае
    '            ' если пользователь хочет остановить выгрузку
    '            'закоментировал
    '            'If m_CancelExit = True Then
    '            '    m_CancelExit = False
    '            '    Exit Sub
    '            'End If
    '        Next
    '        'сейчас закрытие любого документа который сохранен
    '        'в этой точке нет других окон с прерванной выгрузкой
    '        'было
    '        ''If m_КоллекцияПанелейМоториста.Count > 0 Then
    '        ''    For i = m_КоллекцияПанелейМоториста.Count To 1 Step -1
    '        ''        frm = CType(m_КоллекцияПанелейМоториста(i), PanelBaseForm)
    '        ''        frm.Close()
    '        ''    Next
    '        ''End If
    '        'стало
    '        'srtFormsName = m_КоллекцияПанелейМоториста.Keys
    '        srtFormsName.Clear()
    '        For Each strName As String In m_КоллекцияПанелейМоториста.Keys
    '            srtFormsName.Add(strName)
    '        Next

    '        If m_КоллекцияПанелейМоториста.Count > 0 Then
    '            For Each strName As String In srtFormsName
    '                frm = m_КоллекцияПанелейМоториста.Item(strName)
    '                frm.Close()
    '            Next
    '        End If


    '    Catch exp As Exception
    '        MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        'выйти из ошибки, если она получена
    '        Application.Exit()
    '    Finally
    '        'm_ShutdownInProgress = False
    '    End Try
    'End Sub
#End Region

#Region "BindControls"
    'Protected Sub BindControls()
    '    ' Create two Binding objects for the first two TextBox 
    '    ' controls. The data-bound property for both controls 
    '    ' is the Text property. The data source is a DataSet 
    '    ' (ds). The data member is specified by a navigation 
    '    ' path in the form : TableName.ColumnName. 
    '    CheckedListBoxModule.DataBindings.Add _
    '       (New Binding("Text", ds, "customers.custName"))
    '    textBox2.DataBindings.Add _
    '       (New Binding("Text", ds, "customers.custID"))

    '    ' Bind the DateTimePicker control by adding a new Binding. 
    '    ' The data member of the DateTimePicker is specified by a 
    '    ' navigation path in the form: TableName.RelationName.ColumnName. 
    '    DateTimePicker1.DataBindings.Add _
    '       (New Binding("Value", ds, "customers.CustToOrders.OrderDate"))

    '    ' Create a new Binding using the DataSet and a 
    '    ' navigation path(TableName.RelationName.ColumnName).
    '    ' Add event delegates for the Parse and Format events to 
    '    ' the Binding object, and add the object to the third 
    '    ' TextBox control's BindingsCollection. The delegates 
    '    ' must be added before adding the Binding to the 
    '    ' collection; otherwise, no formatting occurs until 
    '    ' the Current object of the BindingManagerBase for
    '    ' the data source changes. 
    '    Dim b As New Binding("Text", ds, "customers.custToOrders.OrderAmount")
    '    AddHandler b.Parse, AddressOf CurrencyStringToDecimal
    '    AddHandler b.Format, AddressOf DecimalToCurrencyString
    '    textBox3.DataBindings.Add(b)

    '    ' Bind the fourth TextBox to the Value of the 
    '    ' DateTimePicker control. This demonstrates how one control
    '    ' can be bound to another.
    '    textBox4.DataBindings.Add("Text", DateTimePicker1, "Value")
    '    Dim bmText As BindingManagerBase = Me.BindingContext(DateTimePicker1)

    '    ' Print the Type of the BindingManagerBase, which is 
    '    ' a PropertyManager because the data source
    '    ' returns only a single property value. 
    '    Console.WriteLine(bmText.GetType().ToString())
    '    ' Print the count of managed objects, which is 1.
    '    Console.WriteLine(bmText.Count)

    '    ' Get the BindingManagerBase for the Customers table. 
    '    bmCustomers = Me.BindingContext(ds, "Customers")
    '    ' Print the Type and count of the BindingManagerBase.
    '    ' Because the data source inherits from IBindingList,
    '    ' it is a RelatedCurrencyManager (derived from CurrencyManager). 
    '    Console.WriteLine(bmCustomers.GetType().ToString())
    '    Console.WriteLine(bmCustomers.Count)

    '    ' Get the BindingManagerBase for the Orders of the current
    '    ' customer using a navigation path: TableName.RelationName. 
    '    bmOrders = Me.BindingContext(ds, "customers.CustToOrders")
    'End Sub



    'Private fruits() As String = {"Spruce", "Ash", "Koa", "Elm", "Oak", "Cherry", "Ironwood", "Cedar", "Sequoia", "Walnut", "Maple", "Balsa", "Pine"}

    'Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdd.Click
    '    If CheckedListBoxModule.Items.Count < fruits.Length Then
    '        Dim stopLoop As Boolean = False
    '        Dim found As Boolean = False
    '        Dim i As Integer = 0
    '        ' If we still have some fruit that have not been
    '        ' added to the CheckedListBoxModule, run through the list
    '        ' and add the first fruit that has not been added.
    '        While stopLoop = False
    '            found = False
    '            Dim j As Integer
    '            For j = 0 To CheckedListBoxModule.Items.Count - 1
    '                If fruits(i).Equals(CStr(CheckedListBoxModule.Items(j))) Then
    '                    found = True
    '                End If
    '            Next j
    '            If found = False Then
    '                stopLoop = True
    '            Else
    '                i += 1
    '            End If
    '        End While
    '        CheckedListBoxModule.Items.Add(fruits(i), False)
    '    End If
    '    If CheckedListBoxModule.Items.Count = fruits.Length Then

    '        ' Make sure that the user can't attemp to add fruits
    '        ' that don't exist.
    '        cmdAdd.Enabled = False

    '    End If
    '    If CheckedListBoxModule.Items.Count > 0 Then
    '        cmdRemove.Enabled = True
    '    End If

    'End Sub


    'Private Sub cmdRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRemove.Click
    '    If CheckedListBoxModule.SelectedIndex >= 0 Then
    '        Dim index As Integer = CheckedListBoxModule.SelectedIndex
    '        CheckedListBoxModule.Items.RemoveAt(index)

    '        If index > 0 Then
    '            CheckedListBoxModule.SelectedIndex = index - 1
    '        Else
    '            If CheckedListBoxModule.Items.Count <> 0 Then
    '                CheckedListBoxModule.SelectedIndex = 0
    '            End If
    '        End If
    '    End If

    '    If CheckedListBoxModule.Items.Count = 0 Then
    '        cmdRemove.Enabled = False
    '    End If

    '    If CheckedListBoxModule.Items.Count < fruits.Length Then
    '        cmdAdd.Enabled = True
    '    End If

    'End Sub


    'Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReset.Click
    '    Dim nListItems As Integer = CheckedListBoxModule.Items.Count
    '    Dim new_checked(fruits.Length) As Boolean
    '    Dim item As String = ""
    '    Dim k As Integer

    '    For k = 0 To fruits.Length - 1
    '        new_checked(k) = False
    '    Next k

    '    Dim l As Integer = 0
    '    Dim m As Integer = 0
    '    For l = 0 To nListItems - 1
    '        If CheckedListBoxModule.GetItemChecked(l) Then
    '            item = CStr(CheckedListBoxModule.Items(l))
    '            For m = 0 To fruits.Length - 1
    '                If fruits(m).Equals(item) Then
    '                    new_checked(m) = True
    '                End If
    '            Next m
    '        End If
    '    Next l

    '    CheckedListBoxModule.Items.Clear()

    '    Dim j As Integer
    '    For j = 0 To nListItems - 1
    '        CheckedListBoxModule.Items.Add(fruits(j), False)
    '        If new_checked(j) = True Then
    '            CheckedListBoxModule.SetItemChecked(j, True)
    '        End If
    '    Next j

    '    cmdReset.Enabled = False
    'End Sub
#End Region


    '#Region "List Data Structure Methods"
    ''Dim genericDataType As Type

    ''Dim stringList As List(Of String)
    ''Dim longList As List(Of LongClass)
    ''Dim customerList As List(Of Customer)

    ''Private Sub ListAdd(Of ItemType)(ByVal list As List(Of ItemType))
    ''    ' Populate the List with the data from the source list box.
    ''    For i As Integer = 0 To listSourceData.Items.Count - 1
    ''        list.Add(CType(listSourceData.Items(i), ItemType))
    ''    Next
    ''End Sub

    ''Private Sub ListDisplay(Of ItemType)(ByVal list As List(Of ItemType))
    ''    For i As Integer = 0 To list.Count - 1
    ''        listTargetData.Items.Add(list(i))
    ''    Next
    ''End Sub

    ''Private Sub ListReverse(Of ItemType)(ByVal list As List(Of ItemType))
    ''    list.Reverse()
    ''End Sub

    ''Private Sub ListSort(Of ItemType)(ByVal list As List(Of ItemType))
    ''    list.Sort()
    ''End Sub

    ''Private Sub CreateList()
    ''    Select Case genericDataType.Name
    ''        Case "String"
    ''            stringList = New System.Collections.Generic.List(Of String)
    ''            ListAdd(stringList)
    ''        Case "LongClass"
    ''            longList = New System.Collections.Generic.List(Of LongClass)
    ''            ListAdd(longList)
    ''        Case "Customer"
    ''            customerList = New System.Collections.Generic.List(Of Customer)
    ''            ListAdd(customerList)
    ''    End Select
    ''End Sub

    ''Private Sub LoadList(Optional ByVal recreate As Boolean = True)
    ''    ' We'll re-create the List here just in case it was emptied.
    ''    If recreate Then
    ''        CreateList()
    ''    End If

    ''    Select Case genericDataType.Name
    ''        Case "String"
    ''            ListDisplay(stringList)
    ''        Case "LongClass"
    ''            ListDisplay(longList)
    ''        Case "Customer"
    ''            ListDisplay(customerList)
    ''    End Select
    ''End Sub

    ''Private Sub EmptyList()
    ''    Select Case genericDataType.Name
    ''        Case "String"
    ''            stringList.Clear()
    ''        Case "LongClass"
    ''            longList.Clear()
    ''        Case "Customer"
    ''            customerList.Clear()
    ''    End Select
    ''End Sub

    ''Private Sub ReverseList()
    ''    Select Case genericDataType.Name
    ''        Case "String"
    ''            ListReverse(stringList)
    ''        Case "LongClass"
    ''            ListReverse(longList)
    ''        Case "Customer"
    ''            ListReverse(customerList)
    ''    End Select

    ''    ' Load items from the queue to the target list box using the existing methods.
    ''    LoadList(False)
    ''End Sub

    ''Private Sub SortList()
    ''    Select Case genericDataType.Name
    ''        Case "String"
    ''            ListSort(stringList)
    ''        Case "LongClass"
    ''            ListSort(longList)
    ''        Case "Customer"
    ''            ListSort(customerList)
    ''    End Select

    ''    ' Load items from the queue to the target list box using the existing methods.
    ''    LoadList(False)
    ''End Sub
    '#End Region


    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    'Dim linesPerPage As Single = 0
    '    'Dim yPos As Single = 0
    '    'Dim count As Integer = 0
    '    'Dim leftMargin As Single = ev.MarginBounds.Left
    '    'Dim topMargin As Single = ev.MarginBounds.Top
    '    'Dim line As String = Nothing

    '    '' Calculate the number of lines per page.
    '    'linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics)

    '    '' Iterate over the file, printing each line.
    '    'While count < linesPerPage
    '    '    line = streamToPrint.ReadLine()
    '    '    If line Is Nothing Then
    '    '        Exit While
    '    '    End If
    '    '    yPos = topMargin + count * printFont.GetHeight(ev.Graphics)
    '    '    ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, _
    '    '        yPos, New StringFormat())
    '    '    count += 1
    '    'End While

    '    '' If more lines exist, print another page.
    '    'If Not (line Is Nothing) Then
    '    '    ev.HasMorePages = True
    '    'Else
    '    '    ev.HasMorePages = False
    '    'End If
    '    e.Graphics.DrawImage(Me.BackgroundImage, 0, 0)
    '    'Me.BackgroundImage()
    'End Sub

#Region "MDIParent1"
    'Public Class MDIParent1
    '    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click, NewWindowToolStripMenuItem.Click
    '        ' Create a new instance of the child form.
    '        Dim ChildForm As New System.Windows.Forms.Form
    '        ' Make it a child of this MDI form before showing it.
    '        ChildForm.MdiParent = Me

    '        m_ChildFormNumber += 1
    '        ChildForm.Text = "Window " & m_ChildFormNumber

    '        ChildForm.Show()
    '    End Sub

    '    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
    '        Dim OpenFileDialog As New OpenFileDialog
    '        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
    '        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
    '        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
    '            Dim FileName As String = OpenFileDialog.FileName
    '            ' TODO: Add code here to open the file.
    '        End If
    '    End Sub

    '    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
    '        Dim SaveFileDialog As New SaveFileDialog
    '        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
    '        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

    '        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
    '            Dim FileName As String = SaveFileDialog.FileName
    '            ' TODO: Add code here to save the current contents of the form to a file.
    '        End If
    '    End Sub


    '    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
    '        Global.System.Windows.Forms.Application.Exit()
    '    End Sub

    '    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
    '        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    '    End Sub

    '    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
    '        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    '    End Sub

    '    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
    '        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    '    End Sub

    '    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
    '        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    '    End Sub

    '    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
    '        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    '    End Sub

    '    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
    '        Me.LayoutMdi(MdiLayout.Cascade)
    '    End Sub

    '    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
    '        Me.LayoutMdi(MdiLayout.TileVertical)
    '    End Sub

    '    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
    '        Me.LayoutMdi(MdiLayout.TileHorizontal)
    '    End Sub

    '    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
    '        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    '    End Sub

    '    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
    '        ' Close all child forms of the parent.
    '        For Each ChildForm As Form In Me.MdiChildren
    '            ChildForm.Close()
    '        Next
    '    End Sub

    '    Private m_ChildFormNumber As Integer = 0

    'End Class

#End Region

#Region "BindControls"
    'Protected Sub BindControls()
    '    ' Create two Binding objects for the first two TextBox 
    '    ' controls. The data-bound property for both controls 
    '    ' is the Text property. The data source is a DataSet 
    '    ' (ds). The data member is specified by a navigation 
    '    ' path in the form : TableName.ColumnName. 
    '    CheckedListBoxModule.DataBindings.Add _
    '       (New Binding("Text", ds, "customers.custName"))
    '    textBox2.DataBindings.Add _
    '       (New Binding("Text", ds, "customers.custID"))

    '    ' Bind the DateTimePicker control by adding a new Binding. 
    '    ' The data member of the DateTimePicker is specified by a 
    '    ' navigation path in the form: TableName.RelationName.ColumnName. 
    '    DateTimePicker1.DataBindings.Add _
    '       (New Binding("Value", ds, "customers.CustToOrders.OrderDate"))

    '    ' Create a new Binding using the DataSet and a 
    '    ' navigation path(TableName.RelationName.ColumnName).
    '    ' Add event delegates for the Parse and Format events to 
    '    ' the Binding object, and add the object to the third 
    '    ' TextBox control's BindingsCollection. The delegates 
    '    ' must be added before adding the Binding to the 
    '    ' collection; otherwise, no formatting occurs until 
    '    ' the Current object of the BindingManagerBase for
    '    ' the data source changes. 
    '    Dim b As New Binding("Text", ds, "customers.custToOrders.OrderAmount")
    '    AddHandler b.Parse, AddressOf CurrencyStringToDecimal
    '    AddHandler b.Format, AddressOf DecimalToCurrencyString
    '    textBox3.DataBindings.Add(b)

    '    ' Bind the fourth TextBox to the Value of the 
    '    ' DateTimePicker control. This demonstrates how one control
    '    ' can be bound to another.
    '    textBox4.DataBindings.Add("Text", DateTimePicker1, "Value")
    '    Dim bmText As BindingManagerBase = Me.BindingContext(DateTimePicker1)

    '    ' Print the Type of the BindingManagerBase, which is 
    '    ' a PropertyManager because the data source
    '    ' returns only a single property value. 
    '    Console.WriteLine(bmText.GetType().ToString())
    '    ' Print the count of managed objects, which is 1.
    '    Console.WriteLine(bmText.Count)

    '    ' Get the BindingManagerBase for the Customers table. 
    '    bmCustomers = Me.BindingContext(ds, "Customers")
    '    ' Print the Type and count of the BindingManagerBase.
    '    ' Because the data source inherits from IBindingList,
    '    ' it is a RelatedCurrencyManager (derived from CurrencyManager). 
    '    Console.WriteLine(bmCustomers.GetType().ToString())
    '    Console.WriteLine(bmCustomers.Count)

    '    ' Get the BindingManagerBase for the Orders of the current
    '    ' customer using a navigation path: TableName.RelationName. 
    '    bmOrders = Me.BindingContext(ds, "customers.CustToOrders")
    'End Sub



    'Private fruits() As String = {"Spruce", "Ash", "Koa", "Elm", "Oak", "Cherry", "Ironwood", "Cedar", "Sequoia", "Walnut", "Maple", "Balsa", "Pine"}

    'Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdd.Click
    '    If CheckedListBoxModule.Items.Count < fruits.Length Then
    '        Dim stopLoop As Boolean = False
    '        Dim found As Boolean = False
    '        Dim i As Integer = 0
    '        ' If we still have some fruit that have not been
    '        ' added to the CheckedListBoxModule, run through the list
    '        ' and add the first fruit that has not been added.
    '        While stopLoop = False
    '            found = False
    '            Dim j As Integer
    '            For j = 0 To CheckedListBoxModule.Items.Count - 1
    '                If fruits(i).Equals(CStr(CheckedListBoxModule.Items(j))) Then
    '                    found = True
    '                End If
    '            Next j
    '            If found = False Then
    '                stopLoop = True
    '            Else
    '                i += 1
    '            End If
    '        End While
    '        CheckedListBoxModule.Items.Add(fruits(i), False)
    '    End If
    '    If CheckedListBoxModule.Items.Count = fruits.Length Then

    '        ' Make sure that the user can't attemp to add fruits
    '        ' that don't exist.
    '        cmdAdd.Enabled = False

    '    End If
    '    If CheckedListBoxModule.Items.Count > 0 Then
    '        cmdRemove.Enabled = True
    '    End If

    'End Sub


    'Private Sub cmdRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRemove.Click
    '    If CheckedListBoxModule.SelectedIndex >= 0 Then
    '        Dim index As Integer = CheckedListBoxModule.SelectedIndex
    '        CheckedListBoxModule.Items.RemoveAt(index)

    '        If index > 0 Then
    '            CheckedListBoxModule.SelectedIndex = index - 1
    '        Else
    '            If CheckedListBoxModule.Items.Count <> 0 Then
    '                CheckedListBoxModule.SelectedIndex = 0
    '            End If
    '        End If
    '    End If

    '    If CheckedListBoxModule.Items.Count = 0 Then
    '        cmdRemove.Enabled = False
    '    End If

    '    If CheckedListBoxModule.Items.Count < fruits.Length Then
    '        cmdAdd.Enabled = True
    '    End If

    'End Sub


    'Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReset.Click
    '    Dim nListItems As Integer = CheckedListBoxModule.Items.Count
    '    Dim new_checked(fruits.Length) As Boolean
    '    Dim item As String = ""
    '    Dim k As Integer

    '    For k = 0 To fruits.Length - 1
    '        new_checked(k) = False
    '    Next k

    '    Dim l As Integer = 0
    '    Dim m As Integer = 0
    '    For l = 0 To nListItems - 1
    '        If CheckedListBoxModule.GetItemChecked(l) Then
    '            item = CStr(CheckedListBoxModule.Items(l))
    '            For m = 0 To fruits.Length - 1
    '                If fruits(m).Equals(item) Then
    '                    new_checked(m) = True
    '                End If
    '            Next m
    '        End If
    '    Next l

    '    CheckedListBoxModule.Items.Clear()

    '    Dim j As Integer
    '    For j = 0 To nListItems - 1
    '        CheckedListBoxModule.Items.Add(fruits(j), False)
    '        If new_checked(j) = True Then
    '            CheckedListBoxModule.SetItemChecked(j, True)
    '        End If
    '    Next j

    '    cmdReset.Enabled = False
    'End Sub

#End Region

#Region "Module1"
    'Public Function funПроверкаСоответствия(ByRef cn As OleDbConnection) As Boolean
    '    Dim blnНадоПерезаписать As Boolean
    '    Dim I, J As Short
    '    Dim lngКоличество As Integer
    '    Dim cmd As OleDbCommand = cn.CreateCommand
    '    Dim strSQL As String
    '    Dim blnНайден As Boolean

    '    strSQL = "SELECT COUNT(*) FROM СвойстваПараметров WHERE ДляЧегоПараметр='Измерение';"
    '    cmd.CommandType = CommandType.Text
    '    cmd.CommandText = strSQL
    '    lngКоличество = CInt(cmd.ExecuteScalar)
    '    strSQL = "SELECT СоответствиеПараметров.*, СвойстваПараметров.ОписаниеПараметра, СвойстваПараметров.РазмерностьВходная, СвойстваПараметров.РазмерностьВыходная, СвойстваПараметров.ДляЧегоПараметр " & "FROM СвойстваПараметров RIGHT JOIN СоответствиеПараметров ON СвойстваПараметров.keyИмяПараметра = СоответствиеПараметров.keyИмяПараметра " & "WHERE (((СвойстваПараметров.ДляЧегоПараметр)='Измерение')) ORDER BY СоответствиеПараметров.НомерПараметраРасчета;"
    '    If lngКоличество > 0 Then
    '        ReDim_arrСоответствие(lngКоличество)
    '        'считываем из базы и заносим из recordset в массив
    '        cmd.CommandType = CommandType.Text
    '        cmd.CommandText = strSQL
    '        Dim rdr As OleDbDataReader = cmd.ExecuteReader
    '        I = 1
    '        While rdr.Read
    '            'Do Until .EOF
    '            arrСоответствие(I).strИмяРасчета = rdr("ИмяПараметраРасчета")
    '            arrСоответствие(I).NРасчета = rdr("НомерПараметраРасчета")
    '            arrСоответствие(I).strИмяБазы = rdr("ИмяПараметраИзмерения")
    '            arrСоответствие(I).NБазы = rdr("НомерПараметраИзмерения")
    '            If IsDBNull(rdr("ИмяБазовогоПараметра")) Then
    '                arrСоответствие(I).strИмяБазовогоПараметра = "" 'vbNullString
    '            Else
    '                arrСоответствие(I).strИмяБазовогоПараметра = rdr("ИмяБазовогоПараметра")
    '            End If
    '            If IsDBNull(rdr("ОписаниеПараметра")) Then
    '                arrСоответствие(I).strОписание = vbNullString
    '            Else
    '                arrСоответствие(I).strОписание = rdr("ОписаниеПараметра")
    '            End If

    '            If IsDBNull(rdr("РазмерностьВходная")) Then
    '                arrСоответствие(I).strРазмерностьВходная = vbNullString
    '            Else
    '                arrСоответствие(I).strРазмерностьВходная = rdr("РазмерностьВходная")
    '            End If
    '            If IsDBNull(rdr("РазмерностьВыходная")) Then
    '                arrСоответствие(I).strРазмерностьВыходная = vbNullString
    '            Else
    '                arrСоответствие(I).strРазмерностьВыходная = rdr("РазмерностьВыходная")
    '            End If
    '            If IsDBNull(rdr("ТипДавления")) Then
    '                'доработка была почему-то эта строка и даже работала
    '                'arrСоответствие(I).strРазмерностьВыходная = " "
    '                arrСоответствие(I).strТипДавления = "" 'думаю что это правильно
    '            Else
    '                If rdr("ТипДавления") = vbNullString Then
    '                    arrСоответствие(I).strТипДавления = ""
    '                Else
    '                    arrСоответствие(I).strТипДавления = rdr("ТипДавления")
    '                End If
    '            End If
    '            I = I + 1
    '        End While
    '        rdr.Close()
    '    End If
    '    'запись погрешностей
    '    'strSQL = "SELECT * FROM СоответствиеПараметров"
    '    'Dim odaDataAdapter As New OleDbDataAdapter(strSQL, cn)

    '    'Dim ds As New DataSet
    '    'odaDataAdapter.Fill(ds)
    '    'Dim tlb As DataTable = ds.Tables(0)
    '    'Dim aRows As DataRow()
    '    'For I = 1 To UBound(arrСоответствие)
    '    '    For J = 1 To UBound(arrПараметры)
    '    '        If arrПараметры(J).strНаименованиеПараметра = arrСоответствие(I).strИмяБазы Then
    '    '            arrСоответствие(I).sngПогрешностьТарировкиКанала = arrПараметры(J).sngПогрешность
    '    '            aRows = tlb.Select("ИмяПараметраРасчета = '" & arrСоответствие(I).strИмяРасчета & "'")
    '    '            If aRows.Length > 0 Then
    '    '                aRows(0)("ПогрешностьТарировкиКанала") = arrПараметры(J).sngПогрешность
    '    '            End If
    '    '            Exit For
    '    '        End If
    '    '    Next J
    '    'Next I
    '    'Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(odaDataAdapter)
    '    'odaDataAdapter.UpdateCommand = myDataRowsCommandBuilder.GetUpdateCommand
    '    'odaDataAdapter.Update(ds)
    '    'ds.Tables(0).AcceptChanges()
    '    'odaDataAdapter.UpdateCommand.Connection.Close()

    '    'проверка соответствия
    '    'если номера изменились, то перепишутся новые, если параметр отсутствует, то ему присвоится conПараметрОтсутствует
    '    blnНадоПерезаписать = False
    '    For I = 1 To UBound(arrСоответствие)
    '        blnНайден = False
    '        If arrСоответствие(I).strИмяБазы <> conПараметрОтсутствует Then
    '            ' проверить есть ли такой в списке замеряемых
    '            For J = 1 To UBound(arrСписПарамКопия)
    '                'проверить на совпадение имен и номеров
    '                If arrСоответствие(I).strИмяБазы = arrПараметры(arrСписПарамКопия(J)).strНаименованиеПараметра Then
    '                    arrСоответствие(I).NБазы = arrСписПарамКопия(J) 'присвоить номер т.к. параметр может быть, номер не совпадать
    '                    blnНайден = True
    '                    Exit For
    '                End If
    '            Next J
    '            If Not blnНайден Then
    '                arrСоответствие(I).NБазы = 0
    '                arrСоответствие(I).strИмяБазы = conПараметрОтсутствует
    '                blnНадоПерезаписать = True
    '            End If
    '        End If
    '    Next I
    '    Return blnНадоПерезаписать
    'End Function

    'Public Sub УстановкаСвойств(ByRef cn As OleDbConnection)
    '    Dim I As Short
    '    Dim strSQL As String
    '    Dim cmd As OleDbCommand = cn.CreateCommand
    '    strSQL = "SELECT COUNT(*) FROM [СвойстваПараметров] WHERE СвойстваПараметров.ДляЧегоПараметр <> 'Измерение'"
    '    cmd.CommandType = CommandType.Text
    '    cmd.CommandText = strSQL
    '    ReDim_arrСвойства(CInt(cmd.ExecuteScalar))

    '    'считываем из базы и заносим из recordset в массив
    '    strSQL = "SELECT * from [СвойстваПараметров] WHERE СвойстваПараметров.ДляЧегоПараметр <> 'Измерение'"
    '    cmd.CommandType = CommandType.Text
    '    cmd.CommandText = strSQL
    '    Dim rdr As OleDbDataReader = cmd.ExecuteReader

    '    I = 1
    '    Do While rdr.Read
    '        arrСвойства(I).strИмяПараметра = rdr("ИмяПараметра")
    '        If IsDBNull(rdr("ОписаниеПараметра")) Then
    '            arrСвойства(I).strОписание = vbNullString
    '        Else
    '            arrСвойства(I).strОписание = rdr("ОписаниеПараметра")
    '        End If
    '        arrСвойства(I).strДляЧегоПараметр = rdr("ДляЧегоПараметр")

    '        If IsDBNull(rdr("РазмерностьВходная")) Then
    '            arrСвойства(I).strРазмерностьВходная = vbNullString
    '        Else
    '            arrСвойства(I).strРазмерностьВходная = rdr("РазмерностьВходная")
    '        End If
    '        If IsDBNull(rdr("РазмерностьВыходная")) Then
    '            arrСвойства(I).strРазмерностьВыходная = vbNullString
    '        Else
    '            arrСвойства(I).strРазмерностьВыходная = rdr("РазмерностьВыходная")
    '        End If
    '        I = I + 1
    '    Loop
    '    rdr.Close()
    'End Sub



    'Public Sub ПроверкаПравильностиПути()
    '    'проверяет правильность путей к текстовым данным
    '    Dim I, intЧислоЗаписей, J As Short
    '    Dim strSQL As String
    '    Dim strПуть, strСимвол, strФайл As String
    '    Dim strКаталогПриемник As String
    '    Dim cnnChannels As New ADODB.Connection()
    '    Dim NewRec As New ADODB.Recordset()
    '    cnnChannels.Open(strProviderJet & "Data Source=" & strПутьChannels & ";")
    '    strSQL = "SELECT БазаСнимков.ПутьНаДиске From БазаСнимков"
    '    NewRec.Open(strSQL, cnnChannels, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    '    intЧислоЗаписей = NewRec.RecordCount
    '    If intЧислоЗаписей <> 0 Then
    '        NewRec.MoveFirst()
    '        'считывает первую запись поля ПутьНаДиске
    '        strПуть = NewRec.Fields("ПутьНаДиске").Value
    '        I = InStr(1, strПуть, "База снимков")
    '        strПуть = Left(strПуть, I - 1)
    '        For J = Len(strПутьChannels) To 1 Step -1
    '            strСимвол = Mid(strПутьChannels, J, 1)
    '            If strСимвол = сРазделитель Then Exit For
    '        Next J
    '        strФайл = Left(strПутьChannels, J)

    '        If strПуть <> strФайл Then
    '            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    '            With NewRec
    '                .MoveFirst()
    '                Do Until .EOF
    '                    'переименовать путь к текстовым данным
    '                    strПуть = .Fields("ПутьНаДиске").Value
    '                    For J = Len(strПуть) To 1 Step -1
    '                        strСимвол = Mid(strПуть, J, 1)
    '                        If strСимвол = сРазделитель Then Exit For
    '                    Next J
    '                    strКаталогПриемник = strФайл & "База снимков\" & Right(strПуть, Len(strПуть) - J)
    '                    .Fields("ПутьНаДиске").Value = strКаталогПриемник
    '                    .Update()
    '                    .MoveNext() 'Переместить на следующую запись
    '                Loop
    '            End With
    '            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    '        End If
    '    End If
    '    NewRec.Close()
    '    cnnChannels.Close()
    'End Sub


    ' если имееется многодокументная форма и каждая форма использует какой-то общий ресурс
    ' то при ее закрытии делается проверка , если она последняя то общий ресурс также закрывается
    ' cn - какйо общий ресурс присваемый свойству формы
    'Private Sub QueryForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    RemoveOldSettings()
    '    'если форма последняя соединение закрыть
    '    If IsLastFormForConnection(Me) Then cn.Close()
    'End Sub

    'Public Function IsLastFormForConnection(ByVal FormToCheck As QueryForm) As Boolean
    '    Dim blnFound, blnIsLastForm As Boolean

    '    Dim frm As QueryForm

    '    blnIsLastForm = True
    '    For Each frm In FormToCheck.MdiParent.MdiChildren
    '        If frm.ConnectionObject Is FormToCheck.ConnectionObject Then
    '            If Not blnFound Then
    '                blnFound = True
    '            Else
    '                blnIsLastForm = False
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    Return blnIsLastForm
    'End Function
#End Region

#Region "Дерево"
    '' Handles the Click event for the "Scan | All Directories" menu item.
    'Private Sub mnuScanAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScanAll.Click
    '    ' Get an array of all logical drives.
    '    Dim Drives As String() = Directory.GetLogicalDrives()
    '    Dim drive As String

    '    tvwDirectories.Nodes.Clear()
    '    'lvwDirectories.Items.Clear()

    '    For Each drive In Drives
    '        Dim dnDrive As ChannelNode

    '        Try
    '            ' Create a ChannelNode that represents each logical drive and add
    '            ' it to the TreeView.
    '            dnDrive = New ChannelNode
    '            dnDrive.Text = drive.Remove(Len(drive) - 1, 1)
    '            tvwDirectories.Nodes.Add(dnDrive)

    '            ' Calculate the size of the drive by adding up the size of all its
    '            ' sub-directories.
    '            dnDrive.Size += GetDirectorySize(drive, dnDrive)
    '        Catch exc As Exception
    '            ' Do nothing. Simply skip any directories that can't be read. Control
    '            ' passes to the first line after End Try.
    '        End Try
    '    Next
    'End Sub

    '' Handles the AfterExpand event for the TreeView, which does not occur after 
    '' the TreeView is selected, but after the application decides that the user's 
    '' attempt to expand the node should be allowed. The corresponding BeforeExpand 
    '' event handler is used for this decision making, if desired. All Before______
    '' events pass a TreeViewCancelEventArgs object that contains a Cancel property.
    '' This property can be used for vetoing the user's action. Thus, the "AfterExpand"
    '' event could rightly be named "AfterExpandApproval".
    'Private Sub TreeView_AfterExpand(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvwDirectories.AfterExpand
    '    e.Node.Expand()
    '    ShowSubDirectories(CType(e.Node, ChannelNode))
    'End Sub

    '' Handles the AfterSelect event for the TreeView, which does not occur after 
    '' the TreeView is selected, but after the application decides that the user's 
    '' attempt to select the node should be allowed. The corresponding BeforeSelect 
    '' event handler is used for this decision making, if desired. All Before______
    '' events pass a TreeViewCancelEventArgs object that contains a Cancel property.
    '' This property can be used for vetoing the user's action. Thus, the "AfterSelect"
    '' event could rightly be named "AfterSelectApproval".
    'Private Sub TreeView_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvwDirectories.AfterSelect
    '    Dim strSubDirectory As ChannelNode = CType(e.Node, ChannelNode)

    '    'lvwDirectories.Items.Clear()

    '    AddToListView(Format(strSubDirectory.Size / (1024 * 1024), "F") + "MB", _
    '        strSubDirectory.Text)
    'End Sub

    '' When a directory node is expanded, add its subdirectories to the ListView.
    'Public Sub ShowSubDirectories(ByVal dnDrive As ChannelNode)
    '    Dim strSubDirectory As ChannelNode

    '    'lvwDirectories.Items.Clear()

    '    For Each strSubDirectory In dnDrive.Nodes
    '        AddToListView(Format(strSubDirectory.Size / MB, "F") + "MB", _
    '            strSubDirectory.Text)
    '    Next
    'End Sub


    '' <doc>
    '' <desc>
    ''     For a given root directory (or drive), add the directories to the
    ''     directoryTree.
    '' </desc>
    '' </doc>
    ''
    'Private Sub AddDirectories(ByVal node As TreeNode)
    '    Try
    '        Dim dir As New DirectoryInfo(GetPathFromNode(node))
    '        Dim e As DirectoryInfo() = dir.GetDirectories()
    '        Dim i As Integer

    '        For i = 0 To e.Length - 1
    '            Dim name As String = e(i).Name
    '            If Not name.Equals(".") And Not name.Equals("..") Then
    '                node.Nodes.Add(New ChannelNode(name))
    '            End If
    '        Next i
    '    Catch e As Exception
    '        MessageBox.Show(e.Message)
    '    End Try
    'End Sub


    '' <doc>
    '' <desc>
    ''     For a given node, add the sub-directories for node's children in the
    ''     directoryTree.
    '' </desc>
    '' </doc>
    ''
    'Private Sub AddSubDirectories(ByVal node As ChannelNode)
    '    Dim i As Integer
    '    For i = 0 To node.Nodes.Count - 1
    '        AddDirectories(node.Nodes(i))
    '    Next i
    '    node.SubDirectoriesAdded = True
    'End Sub


    '' <doc>
    '' <desc>
    ''     Event handler for the afterSelect event on the directoryTree. Change the
    ''     title bar to show the path of the selected ChannelNode.
    '' </desc>
    '' </doc>
    ''
    'Private Sub directoryTree_AfterSelect(ByVal [source] As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles directoryTree.AfterSelect
    '    [Text] = "Windows.Forms File Explorer - " + e.Node.Text
    'End Sub


    '' <doc>
    '' <desc>
    ''     Event handler for the beforeExpand event on the directoryTree. If the
    ''     node is not already expanded, expand it.
    '' </desc>
    '' </doc>
    ''
    'Private Sub directoryTree_BeforeExpand(ByVal [source] As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles directoryTree.BeforeExpand
    '    Dim nodeExpanding As ChannelNode = CType(e.Node, ChannelNode)
    '    If Not nodeExpanding.SubDirectoriesAdded Then
    '        AddSubDirectories(nodeExpanding)
    '    End If
    'End Sub

    '' <doc>
    '' <desc>
    ''      For initializing the directoryTree upon creation of the TreeViewCtl form.
    '' </desc>
    '' </doc>
    ''
    'Private Sub FillDirectoryTree()
    '    Dim i As Integer
    '    Dim drives As String() = Environment.GetLogicalDrives()
    '    For i = 0 To drives.Length - 1
    '        If PlatformInvokeKernel32.GetDriveType(drives(i)) = PlatformInvokeKernel32.DRIVE_FIXED Then
    '            Dim cRoot As New ChannelNode(drives(i))
    '            directoryTree.Nodes.Add(cRoot)
    '            AddDirectories(cRoot)
    '        End If
    '    Next i
    'End Sub


    '' <doc>
    '' <desc>
    ''        Returns the directory path of the node.
    '' </desc>
    '' <retvalue>
    ''        Directory path of node.
    '' </retvalue>
    '' </doc>
    ''
    'Private Function GetPathFromNode(ByVal node As TreeNode) As String
    '    If node.Parent Is Nothing Then
    '        Return node.Text
    '    End If
    '    Return Path.Combine(GetPathFromNode(node.Parent), node.Text)
    'End Function


    '' <doc>
    '' <desc>
    ''        Refresh helper functions to get all expanded nodes under the given
    ''        node.
    '' </desc>
    '' <param term='expandedNodes'>
    ''        Reference to an array of paths containing all nodes which were in the
    ''        expanded state when Refresh was requested.
    '' </param>
    '' <param term='startIndex'>
    ''        Array index of ExpandedNodes to start adding entries to.
    '' </param>
    '' <retvalue>
    ''        New StartIndex, i.e. given value of StartIndex + number of entries
    ''        added to ExpandedNodes.
    '' </retvalue>
    '' </doc>
    ''
    'Private Function Refresh_GetExpanded(ByVal Node As TreeNode, ByVal ExpandedNodes() As String, ByVal StartIndex As Integer) As Integer
    '    If StartIndex < ExpandedNodes.Length Then
    '        If Node.IsExpanded Then

    '            ExpandedNodes(StartIndex) = Node.Text
    '            StartIndex += 1
    '            Dim i As Integer
    '            For i = 0 To Node.Nodes.Count - 1
    '                StartIndex = Refresh_GetExpanded(Node.Nodes(i), ExpandedNodes, StartIndex)
    '            Next i
    '        End If
    '        Return StartIndex
    '    End If
    '    Return -1
    'End Function

    '' <doc>
    '' <desc>
    ''        Refresh helper function to expand all nodes whose paths are in parameter
    ''        ExpandedNodes.
    '' </desc>
    '' <param term='node'>
    ''        Node from which to start expanding.
    '' </param>
    '' <param term='expandedNodes'>
    ''        Array of strings with the path names of all nodes to expand.
    '' </param>
    '' </doc>
    ''
    'Private Sub Refresh_Expand(ByVal Node As TreeNode, ByVal ExpandedNodes() As String)
    '    Dim i As Integer

    '    For i = ExpandedNodes.Length - 1 To 0 Step -1
    '        If ExpandedNodes(i) = Node.Text Then
    '            ' For the expand button to show properly, one level of
    '            ' invisible children have to be added to the tree.
    '            AddSubDirectories(CType(Node, ChannelNode))
    '            Node.Expand()
    '            Dim j As Integer

    '            ' If the node is expanded, expand any children that were
    '            ' expanded before the refresh.
    '            For j = 0 To Node.Nodes.Count - 1
    '                Refresh_Expand(Node.Nodes(j), ExpandedNodes)
    '            Next j

    '            Return
    '        End If
    '    Next i
    'End Sub


    '' <doc>
    '' <desc>
    ''     Refreshes the view by deleting all the nodes and restoring them by
    ''     reading the disk(s). Any expanded nodes in the directoryView will be
    ''     expanded after the refresh.
    '' </desc>
    '' <param term='node'>
    ''     - Node from which the refresh begins. Generally, this is
    ''     the root.
    '' </param>
    '' </doc>
    ''
    'Private Overloads Sub Refresh(ByVal node As TreeNode)
    '    If node.Nodes.Count > 0 Then
    '        If node.IsExpanded Then
    '            Dim tooBigExpandedNodes(node.GetNodeCount(True)) As String
    '            Dim iExpandedNodes As Integer = Refresh_GetExpanded(node, tooBigExpandedNodes, 0)
    '            Dim expandedNodes(iExpandedNodes) As String
    '            ' Update the directoryTree
    '            ' Save all expanded nodes rooted at node, even those that are
    '            ' indirectly rooted.
    '            Array.Copy(tooBigExpandedNodes, 0, expandedNodes, 0, iExpandedNodes)

    '            node.Nodes.Clear()
    '            AddDirectories(node)

    '            ' so children with subdirectories show up with expand/collapse
    '            ' button.
    '            AddSubDirectories(CType(node, ChannelNode))
    '            node.Expand()
    '            Dim j As Integer

    '            ' check all children. Some might have had sub-directories added
    '            ' from an external application so previous childless nodes
    '            ' might now have children.
    '            For j = 0 To node.Nodes.Count - 1
    '                If node.Nodes(j).Nodes.Count > 0 Then
    '                    ' If the child has subdirectories. If it was expanded
    '                    ' before the refresh, then expand after the refresh.
    '                    Refresh_Expand(node.Nodes(j), expandedNodes)
    '                End If
    '            Next j
    '        Else
    '            ' If the node is not expanded, then there is no need to check
    '            ' if any of the children were expanded. However, we should
    '            ' update the tree by reading the drive in case an external
    '            ' application add/removed any directories.
    '            node.Nodes.Clear()
    '            AddDirectories(node)
    '        End If
    '    Else
    '        ' Again, if there are no children, then there is no need to
    '        ' worry about expanded nodes but if an external application
    '        ' add/removed any directories we should reflect that.
    '        node.Nodes.Clear()
    '        AddDirectories(node)
    '    End If
    'End Sub

    'Private Sub checkBox1_Click(ByVal [source] As System.Object, ByVal e As System.EventArgs) Handles checkBox1.Click
    '    Me.directoryTree.Sorted = checkBox1.Checked
    '    Dim i As Integer
    '    For i = 0 To directoryTree.Nodes.Count - 1
    '        Refresh(directoryTree.Nodes(i))
    '    Next i

    'End Sub

    'Private Sub imageListComboBox_SelectedIndexChanged(ByVal [source] As System.Object, ByVal e As System.EventArgs) Handles imageListComboBox.SelectedIndexChanged
    '    Dim index As Integer = imageListComboBox.SelectedIndex
    '    If index = 0 Then
    '        directoryTree.ImageList = Nothing
    '    Else
    '        If index = 1 Then
    '            directoryTree.ImageList = imageList1
    '        Else
    '            directoryTree.ImageList = imageList2
    '        End If

    '    End If
    'End Sub


    'Dim strPath As String = tn.FullPath
    'Dim diDirectory As New DirectoryInfo(strPath)
    'Dim adiDirectories() As DirectoryInfo

    'Try
    '    ' Get an array of all sub-directories as DirectoryInfo objects.
    '    adiDirectories = diDirectory.GetDirectories()
    'Catch exp As Exception
    '    Exit Sub
    'End Try

    'Dim di As DirectoryInfo
    'For Each di In adiDirectories
    '    ' Create a child node for every sub-directory, passing in the directory
    '    ' name and the images its node will use.
    '    Dim tnDir As New TreeNode(di.Name, 1, 2)
    '    ' Add the new child node to the parent node.
    '    tn.Nodes.Add(tnDir)

    '    ' We could now fill up the whole tree by recursively calling 
    '    ' AddDirectories():
    '    '
    '    '   AddDirectories(tnDir)
    '    '
    '    ' This is way too slow, however. Give it a try!
    'Next
#End Region

End Module
