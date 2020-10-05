Public Class DirectoryTreeView
    Inherits TreeView
    Public Property CurrentTreeNode() As TreeNode

    Public Sub New()
        ' Увеличить размер для длнных путей к директориям.
        Width *= 2

        ' установить картинки для изображений.
        'Me.ImageList = New ImageList()
        'With Me.ImageList.Images
        '    .Add(My.Resources.FLOPPY)
        '    .Add(My.Resources.CLSDFOLD)
        '    .Add(My.Resources.OPENFOLD)
        'End With

        ImageList = DigitalPortForm.ImageListTree
        ' создать дерево.
        RefreshTree()
    End Sub

    Protected Overrides Sub OnAfterSelect(ByVal e As TreeViewEventArgs)
        MyBase.OnAfterSelect(e)
        CurrentTreeNode = SelectedNode
    End Sub

    Public Sub SelectCureentNode()
        SelectedNode = CurrentTreeNode
        'Me.SelectedNode = mCurrentTreeNode.Parent
        If CurrentTreeNode Is Nothing Then DigitalPortForm.FindLastNode(TypeNode)
        If CurrentTreeNode Is Nothing Then Exit Sub ' не найден
        DigitalPortForm.FlvFiles.ShowNodes(CurrentTreeNode)
        'flvFiles.ShowFiles(tvea.Node)
    End Sub

    ' Указатель на событие BeforeExpand для поддиректорий TreeView. Коментарий 
    ' в парных событиях Before_____ и After_______ TreeView  /DirectoryScanner/DirectoryScanner.vb.
    Protected Overrides Sub OnBeforeExpand(ByVal tvcea As TreeViewCancelEventArgs)
        MyBase.OnBeforeExpand(tvcea)
        ' Для увеличения проиизводительности и избегания TreeView "мерцания" в течении длительного обновления, 
        ' лучше обернуть код в конструкции  BeginUpdate()... EndUpdate.
        BeginUpdate()
        'Dim tn As TreeNode
        ' Добавить дочерний узел для каждого дочернего узла только по клику пользователя.
        ' Для увеличения производительности каждый узел DirectoryTreeView водержащий только следующий уровень показан знаком +
        ' Для добавления в этот узел дочерних узлов их необходимо раскрыть.

        'For Each tn In tvcea.Node.Nodes
        '    AddDirectories(tn)
        'Next tn

        EndUpdate()
    End Sub

    ''' <summary>
    ''' Добавить дочерние узлы для каждой директории родительского узла, передаваемого как аргумент.
    ''' Дополнительно описание в событии OnBeforeExpand.
    ''' </summary>
    ''' <param name="tn"></param>
    Private Sub AddDirectories(ByVal tn As TreeNode)
        tn.Nodes.Clear()

        'Dim strPath As String = tn.FullPath
        'Dim diDirectory As New DirectoryInfo(strPath)
        'Dim adiDirectories() As DirectoryInfo

        'Try
        ' Получить массив всех поддиректорий как DirectoryInfo объекты.
        '    adiDirectories = diDirectory.GetDirectories()
        'Catch exp As Exception
        '    Exit Sub
        'End Try

        'Dim di As DirectoryInfo
        'For Each di In adiDirectories
        ' Создать дочерний узел для каждой директории, передавая имя директории и картинку узла который будет использоваться.
        '    Dim tnDir As New TreeNode(di.Name, 1, 2)
        '    ' Добавить новый дочерний узел в родительский.
        '    tn.Nodes.Add(tnDir)

        '    ' Заполнить директорию рекурсивным вызовом
        '    ' AddDirectories():
        '    '   AddDirectories(tnDir)
        '    ' Это слишком медленный путь!
        'Next

        'CType(tn.Tag, TreeNodeBase).AcceptVisitor(New MyContextClass(0), New PrintContextVisitor())

        'For Each node As TreeNodeBase In CType(tn.Tag, TreeNodeBase).AcceptVisitor(Me, New PrintContextVisitor())

        '    'tree.AcceptVisitor(New MyContextClass(0), New PrintContextVisitor(writer))
        ' Создать дочерний узел для каждой директории, передавая имя директории и картинку узла который будет использоваться.
        '    Dim tnDir As New TreeNode(di.Name, 1, 2)
        '    ' Добавить новый дочерний узел в родительский.
        '    tn.Nodes.Add(tnDir)

        '    ' Заполнить директорию рекурсивным вызовом
        '    ' AddDirectories():
        '    '   AddDirectories(tnDir)
        '    ' Это слишком медленный путь!
        'Next
    End Sub

    ''' <summary>
    ''' Очистка существующего TreeNode и восстановление DirectoryTreeView
    ''' </summary>
    Public Sub RefreshTree()
        ' Для увеличения проиизводительности и избегания TreeView "мерцания" в течении длительного обновления, 
        ' лучше обернуть код в конструкции  BeginUpdate()... EndUpdate.
        Dim tempTypeNode As TypeGridDigitalNode = TypeNode
        DigitalPortForm.FlvFiles.Items.Clear()

        BeginUpdate()
        Nodes.Clear()

        ' Сделать драйвер диска как корневой узел. 
        'Dim astrDrives As String() = Directory.GetLogicalDrives()
        'Dim strDrive As String
        'For Each strDrive In astrDrives
        '    Dim tnDrive As New TreeNode(strDrive, 0, 0)
        '    Nodes.Add(tnDrive)
        '    AddDirectories(tnDrive)

        '    ' Set the C drive as the default selection.
        '    If strDrive = "C:\" Then
        '        Me.SelectedNode = tnDrive
        '    End If
        'Next

        'Dim astrDrives As String() = Directory.GetLogicalDrives()
        'Dim strDrive As String
        'PrintContextVisitor.Print(treeRoot, Me)
        PrintContextVisitor.Print(GlobalAllConfigurations, Me)

        'For Each node As TreeNodeBase In mВсеКонфигурации.DictionaryВсеКонфигурации.Values
        '    Dim tnDrive As New TreeNode(node.Name, 0, 0)
        '    tnDrive.Tag = node
        '    Nodes.Add(tnDrive)
        '    AddDirectories(tnDrive)

        '    ' Set the C drive as the default selection.
        '    'If strDrive = "C:\" Then
        '    '    Me.SelectedNode = tnDrive
        '    'End If
        'Next
        EndUpdate()
        DigitalPortForm.ClearInstrumentControlStrip()
        TypeNode = tempTypeNode
    End Sub
End Class
