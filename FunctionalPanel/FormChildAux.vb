''' <summary>
''' Класс управления дочерними окнами в менеджере окон
''' </summary>
''' <remarks></remarks>
Public Class FormChildAux

    ''' <summary>
    ''' Добавить имя окна в список
    ''' </summary>
    ''' <param name="noticeDescription"></param>
    ''' <remarks></remarks>
    Public Sub AddPanelListViewItems(ByVal noticeDescription As String, indexImageIndex As Integer)
        ListViewForm.Items.Add(noticeDescription, indexImageIndex)
    End Sub

    ''' <summary>
    ''' в цикле просмотреть дочерние для родителя  Me.MdiParent
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ListViewItemsRebuildChecked()
        ' здесь в цикле просмотреть дочерние для родителя  Me.MdiParent
        For Each frm As Form In MdiParent.MdiChildren
            If TypeOf frm Is FormBasePanelMotorist Then
                For Each itemListView As ListViewItem In ListViewForm.Items
                    If itemListView.Text = CType(frm, FormBasePanelMotorist).NameMotoristPanel Then
                        itemListView.Checked = True
                        Exit For
                    End If
                Next
            End If
        Next frm

        AddHandlerListViewItemChecked()
    End Sub

    Public Sub CloseAllCheckedWindows()
        'For Each frm As Form In MdiParent.MdiChildren
        '    If TypeOf frm Is frmBasePanelMotorist Then
        '        For Each TempListView As ListViewItem In ListView1.Items
        '            If TempListView.Text = CType(frm, frmBasePanelMotorist).ИмяПанелиМоториста Then
        '                ' если название не совпадает, то можно использовать TAG
        '                'If TempListView.Text = frm.Tag Then
        '                If TempListView.Checked = True Then
        '                    TempListView.Checked = False
        '                End If
        '            End If
        '        Next
        '    End If
        'Next frm

        ' лучше наверно такой вариант, когда закрываются все, даже выведенные из родительской панели окна 
        For Each itemListView As ListViewItem In ListViewForm.Items
            If itemListView.Checked Then itemListView.Checked = False
        Next
    End Sub

    ''' <summary>
    ''' Снять отметки с надписей
    ''' Снять Выделение С Меню Моториста
    ''' </summary>
    ''' <param name="strnNotice"></param>
    ''' <remarks></remarks>
    Public Sub UnCheckListMenuByFunctionPanels(ByVal strnNotice As String)
        For Each itemListView As ListViewItem In ListViewForm.Items
            If itemListView.Text = strnNotice Then itemListView.Checked = False
        Next
    End Sub

    ''' <summary>
    ''' Добавить обработчик ItemChecked
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AddHandlerListViewItemChecked()
        AddHandler ListViewForm.ItemChecked, AddressOf ListViewItemChecked
    End Sub

    ''' <summary>
    ''' Удалить обработчик ItemChecked
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RemoveHandlerListViewItemChecked()
        RemoveHandler ListViewForm.ItemChecked, AddressOf ListViewItemChecked
    End Sub

    ''' <summary>
    ''' Удалить все пункты меню
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearListMenuByFunctionPanels()
        ListViewForm.Items.Clear()
    End Sub

    'Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
    '    If Not ListView1.SelectedItems.Count = 0 Then
    '        Dim frm As New ChildForm2

    '        frm.Icon = System.Drawing.Icon.FromHandle(CType(Me.ImageList1.Images.Item(0), Bitmap).GetHicon)
    '        frm.Text = ListView1.SelectedItems.Item(0).Text
    '        frm.TextBox1.Text = "This is " + ListView1.SelectedItems.Item(0).Text

    '        frm.MdiParent = Me.MdiParent
    '        frm.Show()
    '    End If
    'End Sub

    ''' <summary>
    ''' Загрузка окна по выделенной строке в списке 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ListViewItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs)
        Dim selectedItem As ListViewItem = CType(e.Item, ListViewItem)

        If IndexParametersForControl Is Nothing Then
            Const caption As String = "Попытка загрузки панели"
            Const text As String = "Необходимо хотя-бы раз запустить загрузить регистратор."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            selectedItem.Checked = False ' = CheckState.Unchecked
            Exit Sub
        End If

        Try
            If selectedItem.Checked Then
                ' попытка считать файл
                Try
                    Dim fileName As String = $"{PathPanelMotorist}\{selectedItem.Text}.xml"

                    If FileNotExists(fileName) Then
                        Const caption As String = "Запуск панели"
                        Dim text As String = $"В каталоге нет файла <{PathPanelMotorist}> нет файла {selectedItem.Text}.xml !"
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                        Exit Sub
                    End If

                    ' при создании автоматом добавляется в коллекцию
                    If Not gFormsPanelManager.CreateNewFormPanelMotorist(fileName, selectedItem.Text) Then 'там проверка на корректность
                        'selectedItem.CheckState = CheckState.Unchecked
                        selectedItem.Checked = False
                    End If

                Catch
                    Const caption As String = "ChildAuxForm.ListViewItemChecked Сообщение"
                    Const text As String = "Ошибка в создании нового хоста"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                End Try
            Else
                'FormsPanelManager.КоллекцияПанелейМоториста.Remove(selectedItem.Text) 'убрал так как надо удалять закрывая саму форму, а она уже удаляет из коллекции
                If gFormsPanelManager.CollectionFormPanelMotorist IsNot Nothing Then
                    If gFormsPanelManager.CollectionFormPanelMotorist.ContainsKey(selectedItem.Text) Then
                        gFormsPanelManager.CollectionFormPanelMotorist.Item(selectedItem.Text).Close() '.ВыгрузитьФорму() 'там и удаляется
                    End If
                End If
            End If
        Catch ex As Exception
            Dim caption As String = selectedItem.Text
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        MDITabPanelMotoristForm.StatusStrip.Items("ToolStripStatusLabel").Text = If(selectedItem.Checked, "Загружена панель ", "Выгружена панель ") & selectedItem.Text
    End Sub
End Class