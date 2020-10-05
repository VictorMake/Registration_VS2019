''' <summary>
''' Класс заменен в проекте FormTuningSelectiveBase
''' </summary>
Friend Class FormTuningGraphControl
    'Private isFormLoaded As Boolean = False

    'Public Sub New()
    '    ' Этот вызов является обязательным для конструктора.
    '    InitializeComponent()
    '    ' Добавьте все инициализирующие действия после вызова InitializeComponent().
    '    InitializeForm()
    'End Sub

    'Private Sub InitializeForm()
    '    RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
    '    InitializeListView()
    '    PopulateSelectiveControl()
    '    isFormLoaded = True
    '    FormTuningControl_Resize(Me, New EventArgs())
    'End Sub

    'Private Sub InitializeListView()
    '    ListViewName.Items.Clear()
    '    ListViewName.Columns.Clear()
    '    ListViewName.Columns.Add("Параметр", ListViewName.Width * 2 \ 5 - 8, HorizontalAlignment.Left)
    '    ListViewName.Columns.Add("Назначение", ListViewName.Width * 3 \ 5 - 4, HorizontalAlignment.Left)
    'End Sub

    'Private Sub PopulateSelectiveControl()
    '    Dim itemListView As ListViewItem
    '    Dim I, J As Integer
    '    Dim selectiveControl As String = GetIni(PathOptions, "Control", "Selective", "N1\")

    '    ListViewName.Items.Clear()
    '    ' Создать переменную, чтобы добавлять объекты ListItem.
    '    ' заполнить без пометки
    '    For I = 1 To UBound(ParametersType)
    '        itemListView = New ListViewItem(ParametersType(I).NameParameter)
    '        itemListView.SubItems.Add(ParametersType(I).Description)

    '        If InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure) Then
    '            itemListView.ImageIndex = Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)
    '        Else
    '            itemListView.ImageIndex = 6
    '        End If

    '        itemListView.ForeColor = Color.Red ' красный как будто его нет
    '        ListViewName.Items.Add(itemListView)

    '        For J = 1 To UBound(CopyListOfParameter)
    '            ' проверить на совпадение номеров и пометить параметр который есть в конфигурации
    '            If CopyListOfParameter(J) = I Then
    '                ListViewName.Items(I - 1).ForeColor = Color.Black ' черный
    '                Exit For
    '            End If
    '        Next
    '    Next

    '    ' считывание строки из файла
    '    ' считать из файла строку с параметрами контроля и расшифровать ее в массив
    '    ' из массива на лист
    '    DecryptionString(selectiveControl)

    '    ' пометить какие параметры на контроль
    '    For I = 1 To UBound(ParametersType)
    '        For J = 1 To UBound(NameParam)
    '            If ParametersType(I).NameParameter = NameParam(J) Then
    '                ListViewName.Items(I - 1).Checked = True
    '                Exit For
    '            End If
    '        Next
    '    Next
    'End Sub

    'Private Sub FormTuningControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
    '    If isFormLoaded Then
    '        ListViewName.Columns(0).Width = ListViewName.Width * 2 / 5 - 8
    '        ListViewName.Columns(1).Width = ListViewName.Width * 3 / 5 - 4
    '    End If
    'End Sub

    'Private Sub ButtonSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonSave.Click
    '    RegistrationEventLog.EventLog_MSG_USER_ACTION("Запись списка каналов для выборочного контроля")
    '    Dim I As Integer
    '    Const limitSelection As Integer = 16
    '    Dim selectiveControl As String = vbNullString
    '    Dim count As Integer

    '    ' сначало делается запись а затем считывание по новой
    '    ' формирование строки
    '    ' проверка на предел в Ограничение
    '    For I = 1 To UBound(ParametersType)
    '        If ListViewName.Items(I - 1).Checked = True Then
    '            count += 1
    '        End If
    '    Next

    '    If count > limitSelection OrElse count = 0 Then
    '        Const caption As String = "Настройка"
    '        Dim text As String = "Число отмеченных параметров должно быть от 1 до " & limitSelection
    '        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
    '        Exit Sub
    '    End If

    '    count = 0

    '    For I = 1 To UBound(ParametersType)
    '        If ListViewName.Items(I - 1).Checked = True Then
    '            selectiveControl &= ParametersType(I).NameParameter & "\"
    '            count += 1
    '        End If
    '    Next

    '    For I = 1 To limitSelection - count
    '        selectiveControl &= "Пусто\"
    '    Next

    '    ' теперь запись
    '    WriteINI(PathOptions, "Control", "Selective", selectiveControl)
    'End Sub

    'Private Sub ListViewName_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListViewName.ItemChecked
    '    LabelSelected.Text = ListViewName.CheckedItems.Count
    'End Sub
End Class