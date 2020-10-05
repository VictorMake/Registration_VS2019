Imports System.Reflection

Public Class FileListView
    Inherits ListView

    Public Sub New()
        ' Задать по умолчанию View перечеслитель в Details
        'Me.View = System.Windows.Forms.View.Details

        ' Получить картинку для некоторых общих типов
        'Dim img As New ImageList()
        'With img.Images
        '    .Add(My.Resources.DOC)
        '    .Add(My.Resources.EXE)
        'End With

        ' Small и Large картинки листа для ListView используя тотже набор картинок
        SmallImageList = DigitalPortForm.ImageListTree
        LargeImageList = DigitalPortForm.ImageListTree

        ' Создать колонку.
        'With Columns
        '    .Add("Name", 100, HorizontalAlignment.Left)
        '    .Add("Size", 100, HorizontalAlignment.Right)
        '    .Add("Modified", 100, HorizontalAlignment.Left)
        '    .Add("Attribute", 100, HorizontalAlignment.Left)
        'End With
        Scrollable = True
    End Sub

    ''' <summary>
    ''' Перезагрузка события базового класса OnItemActivate.
    ''' Расширяет реализацию базового класса для запуска любых .exe или ассоциированных файлов.
    ''' </summary>
    Protected Overrides Sub OnItemActivate(ByVal ea As EventArgs)
        MyBase.OnItemActivate(ea)

        'Dim lvi As ListViewItem
        'For Each lvi In SelectedItems
        '    Process.Start(Path.Combine(strDirectory, lvi.Text))
        'Next lvi
    End Sub

    ''' <summary>
    ''' Заполнить Колонки
    ''' </summary>
    ''' <typeparam name="V"></typeparam>
    ''' <param name="item"></param>
    Private Sub PopulateColumns(Of V)(ByVal item As V)
        Columns.Clear()
        Columns.Add("Name", 100, HorizontalAlignment.Left)

        'Console.WriteLine()
        'Console.WriteLine("TYPE = " & item.GetType.Name)

        'For Each mi As MethodInfo In GetType(V).GetMethods()
        'Dim pi As System.Reflection.PropertyInfo =  GetType(объект).GetProperty(имя свойства) '
        '' извлечь текущее значение свойства
        'Dim value As Object = pi.GetValue(объект, Nothing)

        For Each pi As PropertyInfo In GetType(V).GetProperties() 'GetType(V).GetMethods()
            ' через интерфейс это метод - Sub Visit(ByVal context As C, ByVal node As RootNode)
            ' для класса PrintFuncVisitor 

            'Console.WriteLine()
            'Console.WriteLine(pi.Name)
            'Dim Myattributes As PropertyAttributes = pi.Attributes
            'Console.Write("PropertyAttributes - " & Myattributes.ToString())

            Columns.Add(pi.Name, 100, HorizontalAlignment.Left)

            'Dim pis As ParameterInfo() = mi.GetParameters()

            'If pis.Length <> 2 Then
            '    Throw New ArgumentException(String.Format("Метод '{0}' должен иметь 2 параметра", mi.Name))
            'End If
            'pis(1) это в методе Sub Visit(ByVal context As C, ByVal node As RootNode) есть узел node
            ' будут добавлены RootNode, Type1Node, Type2Node, Type3Node
        Next
    End Sub

    ''' <summary>
    ''' Заполнить Колонки Действия
    ''' </summary>
    ''' <typeparam name="V"></typeparam>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Private Function PopulateActionColumns(Of V)(ByVal item As V) As Integer
        'Columns.Clear()
        Dim columsCount As Integer

        For Each pi As PropertyInfo In GetType(V).GetProperties() 'GetType(V).GetMethods()
            Columns.Add(pi.Name, 100, HorizontalAlignment.Left)
            columsCount += 1
        Next

        Return columsCount
    End Function

    ''' <summary>
    ''' Добавить Узел из дерева как ListViewItem в ListView
    ''' </summary>
    ''' <typeparam name="V"></typeparam>
    ''' <param name="item"></param>
    ''' <param name="text"></param>
    ''' <param name="imageIndex"></param>
    Private Sub AddListViewItem(Of V)(ByVal item As V, text As String, imageIndex As Integer)
        Dim lvi As New ListViewItem(text) With {.ImageIndex = imageIndex}
        AddSubItems(item, lvi, 0)
        Items.Add(lvi)
    End Sub

    ''' <summary>
    ''' Добавить свойства Узла из дерева как дополнительные колонки(SubItems) в ListView
    ''' </summary>
    ''' <typeparam name="V"></typeparam>
    ''' <param name="item"></param>
    ''' <param name="lvi"></param>
    ''' <param name="columsCount"></param>
    Private Sub AddSubItems(Of V)(ByVal item As V, ByRef lvi As ListViewItem, ByVal columsCount As Integer)
        If columsCount <> 0 Then
            For I As Integer = 1 To columsCount
                lvi.SubItems.Add("")
            Next
        End If

        Try
            For Each pi As PropertyInfo In GetType(V).GetProperties()
                Dim obj As Object = pi.GetValue(item, Nothing)
                If obj IsNot Nothing Then
                    lvi.SubItems.Add(pi.GetValue(item, Nothing).ToString)
                Else
                    lvi.SubItems.Add("")
                End If
            Next
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(AddSubItems)}>"
            Dim text As String = "Ошибка получения информации свойств " & item.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    'Private strDirectory As String

    ''' <summary>
    ''' Показать лист всех узлов в директории
    ''' выбранные пользователем в TreeView контроле.
    ''' </summary>
    Public Sub ShowNodes(ByVal TreeViewEventArgs_Node As TreeNode) '(ByVal strDirectory As String)
        ' Сохранить имя директории как поля
        'Me.strDirectory = strDirectory
        Dim isColumnFill As Boolean ' колонки узла Заполнены
        Items.Clear()

        'Dim diDirectories As New DirectoryInfo(strDirectory)
        'Dim afiFiles() As FileInfo

        'Try
        '    ' Вызвать подходящий GetFiles метод для получения массива файлов в директории.
        '    afiFiles = diDirectories.GetFiles()
        'Catch
        '    Return
        'End Try

        'Dim fi As FileInfo
        'For Each fi In afiFiles
        '    ' Создать ListViewItem.
        '    Dim lvi As New ListViewItem(fi.Name)

        '    ' Назначить ImageIndex основываясь на расширении файла.
        '    Select Case Path.GetExtension(fi.Name).ToUpper()
        '        Case ".EXE"
        '            lvi.ImageIndex = 1
        '        Case Else
        '            lvi.ImageIndex = 0
        '    End Select

        '    ' Добавить длину файла и время последней модификации.
        '    lvi.SubItems.Add(fi.Length.ToString("N0"))
        '    lvi.SubItems.Add(fi.LastWriteTime.ToString())

        '    ' Добавить аттрибут подуровня.
        '    Dim strAttr As String = ""

        '    If (fi.Attributes And FileAttributes.Archive) <> 0 Then
        '        strAttr += "A"
        '    End If
        '    If (fi.Attributes And FileAttributes.Hidden) <> 0 Then
        '        strAttr += "H"
        '    End If
        '    If (fi.Attributes And FileAttributes.ReadOnly) <> 0 Then
        '        strAttr += "R"
        '    End If
        '    If (fi.Attributes And FileAttributes.System) <> 0 Then
        '        strAttr += "S"
        '    End If

        '    lvi.SubItems.Add(strAttr)

        '    ' Добавить созданный ListViewItem в FileListView.
        '    Items.Add(lvi)
        'Next fi

        'If TreeViewEventArgs_Node.Tag.GetType Is GetType(Конфигурация) Then
        Select Case TreeViewEventArgs_Node.Tag.GetType.Name
            ' по типу в иерархии базы данных создать узлы
            Case GetType(ВсеКонфигурации).Name
                Dim allConfigurations As ВсеКонфигурации = CType(TreeViewEventArgs_Node.Tag, ВсеКонфигурации)
                ' если TreeNodeBase, а значит и mВсеКонфигурации имеет аттрибут раскрытия
                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, allConfigurations)

                ' PropertyGrid – удобный компонент для визуального редактирования свойств объектов. 
                ' Объект для редактирования задается в дизайнере WinForms, либо непосредственно в коде
                ' устанавливаем редактируемый объект
                'frmDigitalOutputPort.propertyGrid1.SelectedObject = mВсеКонфигурации

                'For Each nodeКонфигурация As Конфигурация In mВсеКонфигурации.DictionaryВсеКонфигурации.Values
                If allConfigurations.ВсеКонфигурации IsNot Nothing Then
                    For Each nodeConfiguration As Конфигурация In allConfigurations.ВсеКонфигурации
                        'nodeДействие.AcceptVisitor(context, Me)
                        '    ' Create ListViewItem.
                        If Not isColumnFill Then
                            PopulateColumns(nodeConfiguration)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodeConfiguration, nodeConfiguration.Name, 1)
                    Next
                End If

                Exit Select
            Case GetType(Конфигурация).Name
                Dim Configuration As Конфигурация = CType(TreeViewEventArgs_Node.Tag, Конфигурация)
                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Configuration)

                If Configuration.Действия IsNot Nothing Then
                    For Each nodeAction As Действие In Configuration.Действия
                        If Not isColumnFill Then
                            PopulateColumns(nodeAction)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodeAction, nodeAction.Name, 2)
                    Next
                End If

                Exit Select
            Case GetType(Действие).Name
                Dim triggerColumnsCount As Integer
                Dim formulaColumnsCount As Integer
                Dim portColumnsCount As Integer
                Dim Action As Действие = CType(TreeViewEventArgs_Node.Tag, Действие)

                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Action)
                Columns.Clear()
                Columns.Add("Name", 100, HorizontalAlignment.Left)

                If Action.ТриггерыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                    For Each nodeTrigger As ТриггерСрабатыванияЦифровогоВыхода In Action.ТриггерыСрабатыванияЦифровогоВыхода
                        If Not isColumnFill Then
                            triggerColumnsCount += PopulateActionColumns(nodeTrigger)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodeTrigger, nodeTrigger.Name, 3)
                    Next
                End If

                isColumnFill = False

                If Action.ФормулыСрабатыванияЦифровогоВыхода IsNot Nothing Then
                    For Each nodeFormula As ФормулаСрабатыванияЦифровогоВыхода In Action.ФормулыСрабатыванияЦифровогоВыхода
                        If Not isColumnFill Then
                            formulaColumnsCount += PopulateActionColumns(nodeFormula)
                            isColumnFill = True
                        End If

                        Dim lvi As New ListViewItem(nodeFormula.Name) With {.ImageIndex = 4}
                        If triggerColumnsCount = 0 Then
                            AddSubItems(nodeFormula, lvi, 0)
                        Else
                            AddSubItems(nodeFormula, lvi, triggerColumnsCount)
                        End If
                        Items.Add(lvi)
                    Next
                End If

                isColumnFill = False

                If Action.Порты IsNot Nothing Then
                    For Each nodePort As Порт In Action.Порты
                        If Not isColumnFill Then
                            portColumnsCount += PopulateActionColumns(nodePort)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodePort, nodePort.Name, 6)
                    Next
                End If

                Exit Select
            Case GetType(ТриггерСрабатыванияЦифровогоВыхода).Name
                Dim Trigger As ТриггерСрабатыванияЦифровогоВыхода = CType(TreeViewEventArgs_Node.Tag, ТриггерСрабатыванияЦифровогоВыхода)

                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Trigger)
                PopulateColumns(Trigger)
                AddListViewItem(Trigger, Trigger.Name, 3)

                Exit Select
            Case GetType(ФормулаСрабатыванияЦифровогоВыхода).Name
                Dim Formula As ФормулаСрабатыванияЦифровогоВыхода = CType(TreeViewEventArgs_Node.Tag, ФормулаСрабатыванияЦифровогоВыхода)

                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Formula)

                If Formula.АргументыДляФормулы IsNot Nothing Then
                    For Each nodeArgument As АргументДляФормулы In Formula.АргументыДляФормулы
                        If Not isColumnFill Then
                            PopulateColumns(nodeArgument)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodeArgument, nodeArgument.Name, 5)
                    Next
                End If

                Exit Select
            Case GetType(АргументДляФормулы).Name
                Dim Argument As АргументДляФормулы = CType(TreeViewEventArgs_Node.Tag, АргументДляФормулы)

                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Argument)
                PopulateColumns(Argument)
                AddListViewItem(Argument, Argument.Name, 5)

                Exit Select
            Case GetType(Порт).Name
                Dim Port As Порт = CType(TreeViewEventArgs_Node.Tag, Порт)

                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Port)

                If Port.Биты IsNot Nothing Then
                    For Each nodeBit As Бит In Port.Биты

                        If Not isColumnFill Then
                            PopulateColumns(nodeBit)
                            isColumnFill = True
                        End If

                        AddListViewItem(nodeBit, nodeBit.Name, 7)
                    Next
                End If

                Exit Select
            Case GetType(Бит).Name
                Dim Bit As Бит = CType(TreeViewEventArgs_Node.Tag, Бит)
                DigitalPortForm.CreatePropertyEditorSource(TreeViewEventArgs_Node.Tag.GetType.Name, Bit)

                PopulateColumns(Bit)
                AddListViewItem(Bit, Bit.Name, 7)
                Exit Select
        End Select
    End Sub
End Class