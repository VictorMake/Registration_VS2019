Imports MathematicalLibrary

Friend Class FormMetrologyGroup
    Public WriteOnly Property FormParent() As FormTarir
        Set(ByVal Value As FormTarir)
            mFormParent = Value
        End Set
    End Property
    Private mFormParent As FormTarir

    Private isFormLoaded As Boolean = False
    Private isHidePanel As Boolean

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        InitializeListViewParameters()
        PopulateListViewParameters()
        Re.Dim(MetrologyGroup, 0)
        isFormLoaded = True
        FormMetrologyGroup_Resize(Me, New EventArgs)
    End Sub

    Private Sub FormMetrologyGroup_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        If Not mFormParent Is Nothing Then mFormParent.IsGroupTarir = False
        mFormParent = Nothing
    End Sub

    Private Sub InitializeListViewParameters()
        ListViewSource.Items.Clear()
        ListViewSource.Columns.Clear()
        ListViewSource.Columns.Add("Параметр", ListViewSource.Width \ 2 - 4, HorizontalAlignment.Left)
        ListViewSource.Columns.Add("Назначение", ListViewSource.Width \ 2 - 4, HorizontalAlignment.Left)
        ListViewReceiver.Items.Clear()
        ListViewReceiver.Columns.Clear()
        ListViewReceiver.Columns.Add("Параметр", ListViewReceiver.Width \ 2 - 4, HorizontalAlignment.Left)
        ListViewReceiver.Columns.Add("Назначение", ListViewReceiver.Width \ 2 - 4, HorizontalAlignment.Left)
    End Sub

    Private Sub PopulateListViewParameters()
        ListViewSource.BeginUpdate()
        ListViewSource.Items.Clear()
        ListViewReceiver.Items.Clear()

        For I As Integer = 1 To UBound(ParametersType)
            Dim lvItem As New ListViewItem(ParametersType(I).NameParameter)
            lvItem.SubItems.Add(ParametersType(I).Description)

            If CBool(InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure)) Then
                lvItem.ImageIndex = Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)
            Else
                lvItem.ImageIndex = 6
            End If

            lvItem.ForeColor = Color.Black
            ListViewSource.Items.Add(lvItem)
        Next

        ListViewSource.EndUpdate()
    End Sub

    Private Sub ClearSendList(ByVal inConfiguration As String)
        Dim I, J As Integer
        Dim arrCleareNames() As String
        Re.Dim(arrCleareNames, 0)

        ' список параметров в упаковке
        Dim names As String() = DecryptionString(inConfiguration)
        ListViewSource.BeginUpdate()
        ListViewReceiver.BeginUpdate()

        ' заполнить очищенный массив (параметра может не быть) по количеству если найдены
        For I = 1 To UBound(names)
            For J = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = names(I) Then
                    Dim lvItem As New ListViewItem("NULL")
                    lvItem.SubItems.Add("NULL")
                    ListViewReceiver.Items.Add(lvItem)
                    Re.DimPreserve(arrCleareNames, UBound(arrCleareNames) + 1)
                    arrCleareNames(UBound(arrCleareNames)) = names(I)
                    Exit For
                End If
            Next
        Next

        ' заполнить по содержанию второй лист
        Dim countItemsList1, countItemsList2 As Integer ' почему-то  вылетает из for

        For Each itemListView As ListViewItem In ListViewSource.Items
            countItemsList1 += 1
            If countItemsList1 > ListViewSource.Items.Count Then Exit For

            'System.Diagnostics.Debug.WriteLine(itmXLoop.Text & "  " & List1ItemsCount.ToString) 
            For J = 1 To UBound(arrCleareNames)
                If itemListView.Text = arrCleareNames(J) Then
                    Dim lvItem As ListViewItem = ListViewReceiver.Items(J - 1)
                    lvItem.Text = itemListView.Text
                    lvItem.SubItems(1).Text = itemListView.SubItems(1).Text
                    lvItem.ImageIndex = itemListView.ImageIndex
                    Exit For
                End If
            Next
        Next

        countItemsList1 = 0
        ' удалить из первого листа повторы
        For Each itemListView As ListViewItem In ListViewReceiver.Items
            countItemsList2 += 1

            If countItemsList2 > ListViewReceiver.Items.Count Then Exit For

            For Each lvItem As ListViewItem In ListViewSource.Items
                countItemsList1 += 1

                If countItemsList1 > ListViewSource.Items.Count Then Exit For

                If lvItem.Text = itemListView.Text Then
                    ListViewSource.Items.Remove(lvItem)
                    Exit For
                End If
            Next

            countItemsList1 = 0
        Next

        ListViewSource.EndUpdate()
        ListViewReceiver.EndUpdate()
        ButtonApply.Visible = ListViewReceiver.Items.Count > 0
    End Sub

    Private Sub FormMetrologyGroup_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If isFormLoaded Then
            If isHidePanel Then
                PanelReceiver.Width = ClientSize.Width - PanelButtons.Width
                PanelButtons.Height = PanelSource.Height
            Else
                PanelSource.Width = (ClientSize.Width - PanelButtons.Width) \ 2
                PanelReceiver.Width = PanelSource.Width
                PanelButtons.Height = PanelSource.Height
            End If

            ListViewSource.Columns(0).Width = ListViewSource.Width \ 2 - 4
            ListViewSource.Columns(1).Width = ListViewSource.Width \ 2 - 4
            ListViewReceiver.Columns(0).Width = ListViewReceiver.Width \ 2 - 4
            ListViewReceiver.Columns(1).Width = ListViewReceiver.Width \ 2 - 4
        End If
    End Sub

    Private Sub ListViewReceiver_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListViewReceiver.SelectedIndexChanged
        If ButtonClearAll.Visible = False AndAlso ListViewReceiver.SelectedIndices.Count > 0 Then
            mFormParent.MathematicalTreatmentSampleSelectedParameterFromGroup(ListViewReceiver.SelectedIndices(0) + 1)
        End If
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAdd.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавить канал в список опроса")

        Dim lvSource As ListViewItem
        Dim I, lastIndex As Integer
        Dim configuration As String = vbNullString

        ' цикл по листу1 в поисках выделенного
        For I = 0 To ListViewSource.Items.Count - 1
            If ListViewSource.Items(I).Selected Then
                lvSource = ListViewSource.Items(I)
                Dim lvReceiver As New ListViewItem(lvSource.Text)
                lvReceiver.SubItems.Add(lvSource.SubItems(1).Text)
                lvReceiver.ImageIndex = lvSource.ImageIndex
                ListViewReceiver.Items.Add(lvReceiver)
                lastIndex = I
            End If
        Next

        For I = 0 To ListViewReceiver.Items.Count - 1
            configuration &= ListViewReceiver.Items(I).Text & "\"
        Next

        PopulateListViewParameters()
        ClearSendList(configuration)
        ListViewSource.Focus()

        For I = 0 To ListViewSource.Items.Count - 1
            ListViewSource.Items(I).Selected = False
        Next

        If lastIndex > ListViewSource.Items.Count - 1 Then lastIndex = ListViewSource.Items.Count - 1

        If lastIndex > 0 Then
            lvSource = ListViewSource.Items(lastIndex)
            lvSource.EnsureVisible()
            lvSource.Selected = True
        End If
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDelete.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить канал из списка опроса")

        Dim I, lastIndex As Integer
        Dim configuration As String = vbNullString

        ' цикл по листу2 в поисках выделенного
        With ListViewReceiver
            .Focus()

            For I = .Items.Count - 1 To 0 Step -1
                If .Items(I).Selected Then
                    .Items.Remove(.Items(I))
                    lastIndex = I
                End If
            Next

            For I = 0 To .Items.Count - 1
                configuration &= .Items(I).Text & "\"
            Next

            PopulateListViewParameters()
            ClearSendList(configuration)
            .Focus()

            For I = 0 To .Items.Count - 1
                .Items(I).Selected = False
            Next

            If lastIndex > .Items.Count - 1 Then lastIndex = .Items.Count - 1

            If lastIndex > 0 Then
                .Items(lastIndex).EnsureVisible()
                .Items(lastIndex).Selected = True
            End If
        End With
    End Sub

    Private Sub ButtonClearAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClearAll.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Очистка списка каналов из опроса")
        ' первый лист полностью заполнить, а второй очистить
        PopulateListViewParameters()
        ButtonApply.Visible = False
    End Sub

    Private Sub ButtonApply_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonApply.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Приступить к опросу группы каналов")

        PanelSource.Visible = False
        ButtonAdd.Visible = False
        ButtonDelete.Visible = False
        ButtonClearAll.Visible = False
        ButtonApply.Visible = False
        ButtonSave.Visible = True
        ButtonPrint.Visible = True
        isHidePanel = True
        LabelCaption.Text = "Щелчок по параметру обновит его замер"

        Re.Dim(MetrologyGroup, ListViewReceiver.Items.Count)

        For I As Integer = 0 To ListViewReceiver.Items.Count - 1
            For J As Integer = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = ListViewReceiver.Items(I).Text Then
                    If IsWorkWithDaqController Then
                        MetrologyGroup(I + 1) = ParametersType(J).NumberParameter
                    Else
                        MetrologyGroup(I + 1) = CShort(J)
                    End If
                    Exit For
                End If
            Next J
        Next

        mFormParent.InitializeVariablesGroupAcquire()
        Width \= 2
        FormMetrologyGroup_Resize(Me, New EventArgs)
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSave.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Записать коэффициенты аппроксимации в базу")

        For I As Integer = 0 To ListViewReceiver.Items.Count - 1
            mFormParent.MathematicalTreatmentSampleSelectedParameterFromGroup(I + 1)
            Application.DoEvents()
            mFormParent.SavePolynomial()
            Application.DoEvents()
        Next
    End Sub

    Private Async Sub ButtonPrint_ClickAsync(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPrint.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Печать протокола групповой тарировки")
        PanelButtons.Enabled = False
        mFormParent.ToolStripToolBar.Enabled = False

        For I As Integer = 0 To ListViewReceiver.Items.Count - 1
            mFormParent.MenuFile.Enabled = False
            mFormParent.MathematicalTreatmentSampleSelectedParameterFromGroup(I + 1)
            Application.DoEvents()
            mFormParent.SavePolynomial()
            Application.DoEvents()
            Await mFormParent.SavePrintProtocolCalibrationAsync(mFormParent.MenuFile, False)
            Application.DoEvents()
        Next

        mFormParent.ToolStripToolBar.Enabled = True
        PanelButtons.Enabled = True
    End Sub
End Class