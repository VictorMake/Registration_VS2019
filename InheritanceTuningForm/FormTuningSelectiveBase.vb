Imports System.Collections.Generic
Imports System.Windows.Forms.ListView
Imports Registration.FormMain

''' <summary>
''' Диалоговое окно настроек выборочного списка параметров отображаемые
''' окнами FormMain, FormGraphControl и FormTextControl.
''' </summary>
Friend Class FormTuningSelectiveBase

    ' 1. Класс использует глобальные ParametersType и CopyListOfParameter
    '    отмечает цветом используемых при опросе
    ' 2. Входные: заголовок и примечание, ограничение по кол. отмеченных
    '    имя формы с интерфейсом интерактивно обновляющий текущие настройки
    ' 3. Считывание из конфигурации списка для наследуемого класса.
    ' 4. Считывание ключа последнего используемого списка, если не найден то индекс принимается за последний номер в коллекции
    ' 5. При каждом изменении списка производится отображение в фоме текущего положения контролов, при условии что связанная форма отображена при запуске этого класса
    ' 6. При закрытии произвести запись активной конфигурации в JSON хранилище
    ' 7. Так же запись производится по кнопке запись или удалении
    '    если имя новое, то новая перекладка добавляется
    '    если переписать то удаление из базы старых и запись новых
    '    если добавить то запись в базу, очистка ComboBoxListConfig и считывание в лист заново
    ' 8. При загрузки текстового, графического или выборочного контроля из JSON хранилища считывается последняя конфигурация для этого контроля
    '    и передаётся массив параметров (дессериализация не требуетя) производится стандартна работа по настройки выводимых параметров
    ' 9. JSON хранилище должен иметь интерфес для работы с классом настроек и формами отображения

    ''' <summary>
    ''' менеджер настроек
    ''' </summary>
    Private mSettingSelectedParameters As SettingSelectedParameters
    ''' <summary>
    ''' Тип окна отображающего список параметров
    ''' </summary>
    Private mTypeFormTuningSelective As TypeFormTuningSelective
    ''' <summary>
    ''' Окна отображающее список параметров для интерактивного взаимодействия
    ''' </summary>
    Private mForm As IUpdateSelectiveControls
    Private lvSelectedIndices, lvPreviousSelectedIndices As Integer ' позиции выделенных строк в ListView
    ''' <summary>
    ''' Ограничить число наблюдаемых параметров и соответсвенно контролов для них
    ''' </summary>
    Private limitSelection As Integer = 16
    Private img As Bitmap ' вспомогательный для DragDrop
    Private hotSpot As Point ' вспомогательный для DragDrop
    ''' <summary>
    ''' Локальная копия массива структуры настроек каналов
    ''' </summary>
    Private parameters As TypeBaseParameter()
    Private indexesOfParameter As Integer()

#Region "FormTuningSelectiveBase"
    Public Sub New(inTypeFormTuningSelective As TypeFormTuningSelective,
                   inForm As IUpdateSelectiveControls,
                   ByRef inParametersType As TypeBaseParameter(),
                   ByVal inCopyListOfParameter As Integer())
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        parameters = inParametersType
        indexesOfParameter = inCopyListOfParameter
        ' Добавить все инициализирующие действия после вызова InitializeComponent().
        mTypeFormTuningSelective = inTypeFormTuningSelective
        If inForm IsNot Nothing Then mForm = inForm
    End Sub

    Private Sub FormTuningSelectiveBase_Load(sender As Object, e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)

        InitializeForm()
        AllowDrop = True
        ListViewSource.AllowDrop = True
        ListViewReceiver.AllowDrop = True

        ' избавиться от мерцания
        DoubleBuffered = True
    End Sub

    Private Sub FormTuningSelectiveBase_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        ListViewResize()
    End Sub

    Private Sub FormTuningSelectiveBase_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        mSettingSelectedParameters.SaveControlParameters()

        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        mForm = Nothing
    End Sub

    Private Sub InitializeForm()
        InitializeTypeForm()
        InitializeListViews()
        PopulateComboBoxAndListView()
        ListViewResize()
    End Sub

    ''' <summary>
    ''' Внешний вид и ограничения
    ''' </summary>
    Private Sub InitializeTypeForm()
        Dim formCaption As String = String.Empty

        Select Case mTypeFormTuningSelective
            Case TypeFormTuningSelective.TextControl
                formCaption = "текстового контроля."
                LabelCaption.Text = "Отметьте в нужном порядке какие параметры выводить на форму " & formCaption
                limitSelection = 64
                Exit Select
            Case TypeFormTuningSelective.GraphControl
                formCaption = "графического контроля."
                LabelCaption.Text = "Отметьте в нужном порядке какие параметры выводить на форму " & formCaption
                limitSelection = 16
                Exit Select
            Case TypeFormTuningSelective.SelectiveControl
                formCaption = "выборочного списка."
                LabelCaption.Text = "Отметьте в нужном порядке какие параметры выводить в лист " & formCaption
                limitSelection = 128
                Exit Select
            Case Else
                Throw New InvalidProgramException("Недопустимый перечислитель для настройки формы выборочного контроля.")
        End Select

        Me.Text = "Настройка конфигураций выборочных параметров для " & formCaption
        LabelCaption.Text &= vbCrLf & "Можно производить настройку непосредственно наблюдая результат на форме " & formCaption
    End Sub

    Private Const columnWidth As Integer = 100
    Private Sub InitializeListViews()
        InitializeListViews(ListViewSource)
        InitializeListViews(ListViewReceiver)
    End Sub

    Private Sub InitializeListViews(inListWiew As ListView)
        inListWiew.Items.Clear()
        inListWiew.Columns.Clear()
        inListWiew.Columns.Add("Параметр", columnWidth, HorizontalAlignment.Left)
        inListWiew.Columns.Add("Назначение", inListWiew.Width - inListWiew.Columns(0).Width - 8, HorizontalAlignment.Left)
    End Sub

    Private Sub PopulateComboBoxAndListView()
        mSettingSelectedParameters = New SettingSelectedParameters(PathResourses, [Enum].GetName(GetType(TypeFormTuningSelective), mTypeFormTuningSelective))

        ComboBoxListConfig.Items.Clear()
        ComboBoxListConfig.Items.AddRange(mSettingSelectedParameters.CollectionLists)
        ComboBoxListConfig.SelectedItem = mSettingSelectedParameters.NameListParameter
        'ComboBoxListConfig.SelectedIndex = ComboBoxListConfig.FindStringExact(ComboBoxListConfig.SelectedItem)
        ButtonDeleteSelectedList.Enabled = ComboBoxListConfig.Items.Count > 1
    End Sub

    Private Sub ListViewResize()
        If Me.IsHandleCreated Then
            ListViewResize(ListViewSource)
            ListViewResize(ListViewReceiver)
        End If
    End Sub

    Private Sub ListViewResize(inListWiew As ListView)
        inListWiew.Columns(0).Width = columnWidth
        inListWiew.Columns(1).Width = inListWiew.Width - inListWiew.Columns(0).Width - 8
    End Sub
#End Region

#Region "Манипуляция строками параметров"
    Private Sub MessageInsertPanel()
        MessageBox.Show(Me, $"Число контролируемых параметров должно быть от 1 до {limitSelection} штук!{vbCrLf}
Для добавления нового необходимо удалить любой параметр из списка.",
                "Добавление параметра для контроля.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ButtonInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonInsert.Click
        Dim receiverListViewItem, sourceListViewItem As ListViewItem
        Dim lastIndex As Integer

        EnableButtons(False)

        ' цикл по листу1 в поисках выделенного
        For I As Integer = 0 To ListViewSource.Items.Count - 1
            If ListViewSource.Items(I).Selected AndAlso ListViewSource.Items(I).ForeColor <> Color.Red Then
                sourceListViewItem = ListViewSource.Items(I)
                receiverListViewItem = New ListViewItem(sourceListViewItem.Text) With {
                    .Name = sourceListViewItem.Text,
                    .ImageIndex = sourceListViewItem.ImageIndex}
                receiverListViewItem.SubItems.Add(sourceListViewItem.SubItems(1).Text)
                ListViewReceiver.Items.Add(receiverListViewItem)
                lastIndex = I
            End If
        Next

        ' удалить превышающие ограничения
        While ListViewReceiver.Items.Count > limitSelection
            ListViewReceiver.Items.RemoveAt(ListViewReceiver.Items.Count - 1)
        End While

        UpdateOnInsertOrRemoveListViewItem(ListViewSource, lastIndex)
        EnableButtons(True)

        If ListViewReceiver.Items.Count >= limitSelection Then MessageInsertPanel()
    End Sub

    Private Sub ButtonRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRemove.Click
        If ListViewReceiver.SelectedItems.Count > 0 Then
            Dim lastIndex As Integer

            EnableButtons(False)
            ListViewReceiver.Focus()

            '' цикл по листу2 в поисках выделенного
            'Dim deleteListViewItem As ListViewItem
            'For I As Integer = lvwReceiver.Items.Count - 1 To 0 Step -1
            '    If lvwReceiver.Items(I).Selected Then
            '        deleteListViewItem = lvwReceiver.Items(I)
            '        lvwReceiver.Items.Remove(deleteListViewItem) ' требует объект а не индекс
            '        lastIndex = I
            '    End If
            'Next
            'Dim SelectedIndices As SelectedIndexCollection

            Dim mSelectedItems As SelectedListViewItemCollection = ListViewReceiver.SelectedItems
            For Each item As ListViewItem In mSelectedItems
                lastIndex = item.Index
                ListViewReceiver.Items.Remove(item)
            Next

            UpdateOnInsertOrRemoveListViewItem(ListViewReceiver, lastIndex)
            EnableButtons(True)
        End If
    End Sub

    Private Sub ButtonErase_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonErase.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Очистить список")
        ' первый лист полностью заполнить, а второй очистить
        EnableButtons(False)
        ' первый лист полностью заполнить, а второй очистить
        mSettingSelectedParameters.SelectedParametersAsString = New String() {}
        ListViewReceiver.Items.Clear()

        EnableButtons(True)
        PopulateSourceListView()
    End Sub

    Private Sub ButtonDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDown.Click
        StartMoveRow(-1)
    End Sub

    Private Sub ButtonUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUp.Click
        StartMoveRow(1)
    End Sub

    ''' <summary>
    ''' Запустить перемещения выделенной строки
    ''' </summary>
    ''' <param name="direct"></param>
    Private Sub StartMoveRow(direct As Integer)
        EnableButtons(False)
        lvPreviousSelectedIndices = lvSelectedIndices
        lvSelectedIndices += direct
        MoveSelectedListViewItem()
        EnableButtons(True)

        mSettingSelectedParameters.SaveControlParameters()
        ' здесь вызов у интерфейсного параметра метода Update
        If mForm IsNot Nothing Then mForm.UpdateControls()
    End Sub

    ''' <summary>
    ''' Управление доступом к контролам
    ''' </summary>
    ''' <param name="inEnable"></param>
    Private Sub EnableButtons(inEnable As Boolean)
        ListViewSource.Enabled = inEnable
        ButtonInsert.Enabled = inEnable
        ButtonRemove.Enabled = inEnable
        ButtonUp.Enabled = inEnable
        ButtonDown.Enabled = inEnable
        ButtonErase.Enabled = inEnable
        ButtonSaveConfig.Enabled = inEnable

        ButtonInsert.Enabled = ListViewReceiver.Items.Count < limitSelection
    End Sub

    ''' <summary>
    ''' Переместить выделенную строку в листе
    ''' </summary>
    Private Sub MoveSelectedListViewItem()
        With ListViewReceiver
            .Focus()
            If .Items.Count > 0 AndAlso .SelectedIndices.Count <> 0 Then
                Dim selectedIndex As Integer = .SelectedIndices(.SelectedIndices.Count - 1)
                If lvPreviousSelectedIndices < lvSelectedIndices Then ' вверх
                    If selectedIndex <> 0 Then SwapListViewItem(selectedIndex, selectedIndex - 1)
                Else
                    If selectedIndex <> .Items.Count - 1 Then SwapListViewItem(selectedIndex, selectedIndex + 1)
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' Произвести обновления в панели наблюдения после
    ''' добавления или удаления записи в списке
    ''' </summary>
    ''' <param name="tempListView"></param>
    ''' <param name="last"></param>
    Private Sub UpdateOnInsertOrRemoveListViewItem(tempListView As ListView, last As Integer)
        Dim listViewItemSelect As ListViewItem

        MakeUpdate()
        tempListView.Focus()

        For I As Integer = 0 To tempListView.Items.Count - 1
            tempListView.Items(I).Selected = False
        Next

        If last > tempListView.Items.Count - 1 Then last = tempListView.Items.Count - 1

        If last > 0 OrElse (last = 0 AndAlso tempListView.Items.Count > 0) Then
            listViewItemSelect = tempListView.Items(last)
            listViewItemSelect.EnsureVisible()
            listViewItemSelect.Selected = True
        End If
    End Sub

    ''' <summary>
    ''' Произвести обновления в панели наблюдения
    ''' </summary>
    Private Sub MakeUpdate()
        ' создать новый список
        Dim inNames As New List(Of String)
        For Each itemLVW As ListViewItem In ListViewReceiver.Items
            inNames.Add(itemLVW.Text)
        Next

        If mTypeFormTuningSelective = TypeFormTuningSelective.GraphControl Then
            For I = 1 To limitSelection - ListViewReceiver.Items.Count
                inNames.Add("Пусто")
            Next
        End If

        ' если переписать то удаление из списка старых и запись новых
        mSettingSelectedParameters.UpdateListParameter(inNames)
        PopulateAndClearSourseListView()
    End Sub

    ''' <summary>
    ''' Обмен содержиммым между соседними строками
    ''' </summary>
    ''' <param name="inFrom"></param>
    ''' <param name="inTo"></param>
    Private Sub SwapListViewItem(ByVal inFrom As Integer, ByVal inTo As Integer)
        Dim text As String
        Dim description As String
        Dim indexIcon As Integer

        With ListViewReceiver
            Dim currentListViewItem As ListViewItem = .Items(inFrom)
            Dim targetListViewItem As ListViewItem = .Items(inTo)

            ' запомнить предыдущего
            text = targetListViewItem.Text
            description = targetListViewItem.SubItems(1).Text
            indexIcon = targetListViewItem.ImageIndex

            ' запись перемещаемого в предыдущий
            targetListViewItem.Text = currentListViewItem.Text
            targetListViewItem.Name = currentListViewItem.Text
            targetListViewItem.SubItems(1).Text = currentListViewItem.SubItems(1).Text
            targetListViewItem.ImageIndex = currentListViewItem.ImageIndex

            ' перезапись в перемещаемый сохраненные
            currentListViewItem.Text = text
            currentListViewItem.Name = text
            currentListViewItem.SubItems(1).Text = description
            currentListViewItem.ImageIndex = indexIcon

            MakeUpdate()

            ' выделить
            .Items(inFrom).Selected = False
            targetListViewItem.EnsureVisible()
            .Items(inTo).Selected = True
        End With
    End Sub
#End Region

#Region "Манипуляция списками"
    Private Sub ComboBoxListConfig_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxListConfig.TextChanged
        ButtonAddNewConfig.Enabled = ComboBoxListConfig.FindStringExact(ComboBoxListConfig.Text) = -1
        ButtonDeleteSelectedList.Enabled = Not ComboBoxListConfig.Text = ""
    End Sub

    Private Sub ComboBoxListConfig_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxListConfig.SelectedIndexChanged
        If ComboBoxListConfig.FindStringExact(ComboBoxListConfig.Text) = -1 Then Exit Sub

        mSettingSelectedParameters.SetWorkListParameter(CType(CType(sender, ComboBox).SelectedItem, String))
        PopulateAndClearSourseListView()
    End Sub

    ''' <summary>
    ''' Заполнить и очистить лист источник от элементов содержащихся в листе приёмнике
    ''' </summary>
    Private Sub PopulateAndClearSourseListView()
        PopulateSourceListView()
        ' при считывании отключается событие изменения Check, после загрузки включается и производится добавление
        If mSettingSelectedParameters.SelectedParametersCount > 0 Then ClearListViewSource()
        ButtonSaveConfig.Enabled = False
    End Sub

    ''' <summary>
    ''' Заполнить лист источник
    ''' </summary>
    Private Sub PopulateSourceListView()
        Dim ImageIndex As Integer
        Dim nameParameter As String

        ListViewSource.BeginUpdate()
        ListViewSource.Items.Clear()
        ListViewReceiver.Items.Clear()

        ' заполнить первый лист
        For I As Integer = 1 To UBound(parameters)
            nameParameter = parameters(I).NameParameter

            If CBool(InStr(1, UnitOfMeasureString, parameters(I).UnitOfMeasure)) Then
                ImageIndex = Array.IndexOf(UnitOfMeasureArray, parameters(I).UnitOfMeasure)
            Else
                ImageIndex = 6
            End If

            Dim newLVWItem As ListViewItem = New ListViewItem(nameParameter) With {.Name = nameParameter, .ImageIndex = ImageIndex, .ForeColor = Color.Red} ' красный как будто его нет
            newLVWItem.SubItems.Add(parameters(I).Description)
            ListViewSource.Items.Add(newLVWItem)

            For J As Integer = 1 To UBound(indexesOfParameter)
                ' проверить на совпадение номеров и пометить параметр который есть в конфигурации
                If indexesOfParameter(J) = I Then
                    ListViewSource.Items(I - 1).ForeColor = Color.Black ' черный
                    Exit For
                End If
            Next
        Next

        ListViewSource.EndUpdate()

        mSettingSelectedParameters.SaveControlParameters()
        ' здесь вызов у интерфейсного параметра метода Update
        If mForm IsNot Nothing Then mForm.UpdateControls()
    End Sub

    ''' <summary>
    ''' Очистить лист источник от элементов содержащихся в листе приёмнике
    ''' </summary>
    Private Sub ClearListViewSource()
        If mSettingSelectedParameters.SelectedParametersCount > 0 Then
            Dim nameParameter As String

            ListViewSource.BeginUpdate()
            ListViewReceiver.BeginUpdate()

            ' заполнить очищенный массив (параметра может не быть) по количеству, если найдены
            For I As Integer = 0 To UBound(mSettingSelectedParameters.SelectedParametersAsString)
                For J As Integer = 1 To parameters.GetUpperBound(0)
                    If parameters(J).NameParameter = mSettingSelectedParameters.SelectedParametersAsString(I) Then
                        Dim foundedListViews As ListViewItem() = ListViewSource.Items.Find(mSettingSelectedParameters.SelectedParametersAsString(I), False)

                        If foundedListViews.Length > 0 Then
                            ' заполнить по содержанию второй лист по содержимому из первого уже настроенного листа
                            nameParameter = foundedListViews(0).Text
                            Dim newListViewItem As New ListViewItem(nameParameter) With {.Name = nameParameter, .ImageIndex = foundedListViews(0).ImageIndex}
                            newListViewItem.SubItems.Add(foundedListViews(0).SubItems(1).Text)
                            ListViewReceiver.Items.Add(newListViewItem)

                            For Each itemSource As ListViewItem In foundedListViews
                                ListViewSource.Items.Remove(itemSource) ' требует объект, а не индекс
                            Next
                        End If

                        Exit For
                    End If
                Next
            Next

            ListViewSource.EndUpdate()
            ListViewReceiver.EndUpdate()
            LabelSelected.Text = ListViewReceiver.Items.Count.ToString
        End If
    End Sub

    Private Sub ButtonAddNewConfig_Click(sender As Object, e As EventArgs) Handles ButtonAddNewConfig.Click
        AddNewOrSaveConfig()
    End Sub

    Private Sub ButtonSaveConfig_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveConfig.Click
        AddNewOrSaveConfig()
    End Sub

    ''' <summary>
    ''' Добавить или записать конфигурацию в зависимости от наличия этого имени в составе списка ComboBoxListConfig
    ''' </summary>
    Private Sub AddNewOrSaveConfig()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись составленного списка каналов в конфигурацию")

        If ListViewReceiver.Items.Count > limitSelection OrElse ListViewReceiver.Items.Count = 0 Then
            MessageInsertPanel()
            Exit Sub
        Else
            If ComboBoxListConfig.Text = "" Then
                MessageBox.Show("Введите имя", "Список", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Try
                ' создать новый список
                Dim inNames As New List(Of String)
                For Each itemLVW As ListViewItem In ListViewReceiver.Items
                    inNames.Add(itemLVW.Text)
                Next

                If mTypeFormTuningSelective = TypeFormTuningSelective.GraphControl Then
                    For I = 1 To limitSelection - ListViewReceiver.Items.Count
                        inNames.Add("Пусто")
                    Next
                End If

                If ComboBoxListConfig.FindStringExact(ComboBoxListConfig.Text) = -1 Then
                    ' если имя новое, то новый список добавляется
                    mSettingSelectedParameters.InsertNewListParameter(ComboBoxListConfig.Text, inNames)
                Else
                    ' если переписать то удаление из списка старых и запись новых
                    mSettingSelectedParameters.UpdateListParameter(inNames)
                End If

                PopulateComboBoxAndListView()
            Catch ex As Exception
                Const caption As String = "Запись выборочного списка в настроечный файл"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            Finally
                ButtonSaveConfig.Enabled = False
            End Try
        End If
    End Sub

    Private Sub ButtonDeleteSelectedList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedList.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удаление составленного списка каналов из конфигурации")
        If ComboBoxListConfig.Items.Count = 1 Then
            MessageBox.Show(Me, "Последний элемент в списке удалять нельзя!", "Удаление списка", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ButtonDeleteSelectedList.Enabled = False
            Exit Sub
        End If

        If ComboBoxListConfig.Text = "" OrElse ComboBoxListConfig.SelectedIndex = -1 Then
            MessageBox.Show(Me, "Выделите список для удаления.", "Удаление списка", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ComboBoxListConfig.FindStringExact(ComboBoxListConfig.Text) = -1 Then
            MessageBox.Show(Me, $"Список с именем: <{ComboBoxListConfig.Text}> отсутствует в сохранённых конфигурациях!", "Удаление списка", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            ' имя в списке существует
            mSettingSelectedParameters.DeleteListParameter(ComboBoxListConfig.Text)
            PopulateComboBoxAndListView()
        Catch ex As Exception
            Const caption As String = "Удаление из базы"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            PopulateAndClearSourseListView()
        End Try
    End Sub
#End Region

#Region "DragDrop"
    ''' <summary>
    ''' Удостовериться что пользовательский курсор использован
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FormTuningSelectiveBase_GiveFeedback(ByVal sender As Object, ByVal e As GiveFeedbackEventArgs) Handles Me.GiveFeedback, ListViewSource.GiveFeedback, ListViewReceiver.GiveFeedback
        If img IsNot Nothing Then
            e.UseDefaultCursors = False
            Windows.Forms.Cursor.Current = CreaterCursor.CreateCursor(img, hotSpot.X, hotSpot.Y)
        End If
    End Sub

    Private Sub ListView_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles ListViewReceiver.DragDrop
        If ListViewReceiver.Items.Count >= limitSelection Then
            MessageInsertPanel()
            Exit Sub
        End If

        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
            If ListViewReceiver.dropIndex > -1 Then
                ListViewReceiver.Items.Insert(ListViewReceiver.dropIndex, DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem))
            Else
                ListViewReceiver.Items.Add(DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem))
            End If

            ListViewReceiver.Alignment = ListViewAlignment.Default
            ListViewReceiver.Alignment = ListViewAlignment.Top
            ListViewReceiver.Refresh()

            Dim newListViewItemIndex As Integer = DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem).Index
            UpdateOnInsertOrRemoveListViewItem(ListViewSource, newListViewItemIndex)
            ButtonSaveConfig.Enabled = True
        End If
    End Sub

    Private Sub ListViews_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles ListViewSource.DragOver, ListViewReceiver.DragOver
        If e.Data.GetDataPresent(GetType(ListViewItem)) Then e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub ListViewSource_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ListViewSource.MouseDown
        Dim itemListView As ListViewItem = ListViewSource.HitTest(e.Location).Item

        If itemListView IsNot Nothing AndAlso itemListView.ForeColor <> Color.Red Then
            Dim index As Integer = itemListView.Index

            ListViewSource.DoDragDrop(DirectCast(ListViewSource.Items(index).Clone, ListViewItem), DragDropEffects.Copy)

            Dim p As Point = e.Location
            p.Offset(0, -ListViewSource.Items(index).Bounds.Top)

            Dim EffectCursor As New Windows.Forms.Cursor(New IO.MemoryStream(My.Resources.move1))

            img = New Bitmap(ListViewSource.Width, ListViewSource.Items(index).Bounds.Height + EffectCursor.Size.Height)

            Dim gr As Graphics = Graphics.FromImage(img)
            gr.Clear(Color.White)

            For I As Integer = 0 To ListViewSource.Items(index).SubItems.Count - 1
                gr.DrawString(ListViewSource.Items(index).SubItems(I).Text, ListViewSource.Font, Brushes.Black, ListViewSource.Items(index).SubItems(I).Bounds.Left, 0)
            Next

            EffectCursor.Draw(gr, New Rectangle(p, EffectCursor.Size))
            img.MakeTransparent(Color.White)
            hotSpot = p
        End If
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ListViewSource, ListViewReceiver)
        mSearchChannel.SelectChannel()
    End Sub
#End Region

End Class