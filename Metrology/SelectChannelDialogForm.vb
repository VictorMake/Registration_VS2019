Imports DataTreeViewDLL.Chaliy.Windows.Forms.DataTreeView

''' <summary>
''' Модальная форма выбора аналогового канала ввода AI для тарировки из дерева по шасси и модулям.
''' </summary>
''' <remarks></remarks>
Public Class SelectChannelDialogForm
    Public Property ChassisName As String
    Public Property ChannelName As String

    Private Sub SelectChannelDialogForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' Тест
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("Шасси 1", "Шасси 1", "", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 1>", "Модуль 1", "Шасси 1", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 1><Канал 1>", "Канал 1", "<Шасси 1><Модуль 1>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 1><Канал 2>", "Канал 2", "<Шасси 1><Модуль 1>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 1><Канал 3>", "Канал 3", "<Шасси 1><Модуль 1>", True)

        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 2>", "Модуль 2", "Шасси 1", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 2><Канал 1>", "Канал 1", "<Шасси 1><Модуль 2>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 2><Канал 2>", "Канал 2", "<Шасси 1><Модуль 2>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 1><Модуль 2><Канал 3>", "Канал 3", "<Шасси 1><Модуль 2>", True)

        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("Шасси 2", "Шасси 2", "", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2>Модуль 1>", "Модуль 1", "Шасси 2", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 1><Канал 1>", "Канал 1", "<Шасси 2>Модуль 1>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 1><Канал 2>", "Канал 2", "<Шасси 2>Модуль 1>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 1><Канал 3>", "Канал 3", "<Шасси 2>Модуль 1>", True)

        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 2>", "Модуль 2", "Шасси 2", False)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 2><Канал 1>", "Канал 1", "<Шасси 2><Модуль 2>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 2><Канал 2>", "Канал 2", "<Шасси 2><Модуль 2>", True)
        'Me.ChannelsChassisDataSetFrm.Test.AddTestRow("<Шасси 2><Модуль 2><Канал 3>", "Канал 3", "<Шасси 2><Модуль 2>", True)

        Const MODULE_3 As String = "Модуль 3" ' пока используется фиксированное значение положения и количество модулей AI

        ' пройти по всем шасси в конфигурации и добавить в дерево по принципу Узел(Ключ, Имя, РодительскийКлюч, ЭтоКанал)
        'For Each itemChassis As ChassisClass In gManagerChassis.CollectionsChassis.Values
        '    Dim hostName As String = itemChassis.HostName
        '    Dim idModule As String = "<" & itemChassis.HostName & "><" & MODULE_3 & ">"
        '    Me.ChannelsChassisDataSetFrm.Test.AddTestRow(hostName, hostName, "", False)
        '    Me.ChannelsChassisDataSetFrm.Test.AddTestRow(idModule, MODULE_3, hostName, False)
        '    GetSettingAnalogInput(itemChassis.HostName, idModule)
        'Next

        Dim hostName As String = "SCXI"
        Dim idModule As String = "<" & hostName & "><" & MODULE_3 & ">"
        Me.ChannelsChassisDataSetFrm.Test.AddTestRow(hostName, hostName, "", False)
        Me.ChannelsChassisDataSetFrm.Test.AddTestRow(idModule, MODULE_3, hostName, False)

        For I As Integer = 1 To UBound(ParametersType)
            'If InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure) Then
            '    cmbПараметры.Items.Add(New StringIntObject(ParametersType(I).NameParameter, Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)))
            'Else
            '    cmbПараметры.Items.Add(New StringIntObject(ParametersType(I).NameParameter, 1))
            'End If

            Dim nameChannel As String = ParametersType(I).NameParameter
            Me.ChannelsChassisDataSetFrm.Test.AddTestRow(idModule & "<" & nameChannel & ">", nameChannel, idModule, True)
        Next

    End Sub

    '''' <summary>
    '''' Добавить в дерево каналы конкретного шасси
    '''' </summary>
    '''' <param name="ИмяШасси"></param>
    '''' <param name="idModule"></param>
    '''' <remarks></remarks>
    'Private Sub GetSettingAnalogInput(ИмяШасси As String, idModule As String)
    '    Dim cmmDbCommand As OleDbCommand
    '    ' По подключенным в проект шасси производится заполнение дерева каналами из базы данных только используемых и отмеченных USE
    '    Dim sSQL As String = "SELECT Count(*) AS CountChannels " &
    '     "FROM Шасси RIGHT JOIN SettingAnalogInput ON Шасси.keyШасси = SettingAnalogInput.keyШасси " &
    '     "WHERE (((Шасси.ИмяШасси)= '" & ИмяШасси & "') AND ((SettingAnalogInput.Use)=True));"

    '    Using cnnOleDbConnection As New OleDbConnection(BuildCnnStr(ProviderJet, gPathChannels_cfg_lmz))
    '        cmmDbCommand = New OleDbCommand(sSQL, cnnOleDbConnection)
    '        cmmDbCommand.CommandType = CommandType.Text
    '        cnnOleDbConnection.Open()

    '        cmmDbCommand.CommandText = sSQL
    '        Dim Dimsize As Integer = Convert.ToInt32(cmmDbCommand.ExecuteScalar)

    '        If Dimsize > 0 Then
    '            '--- Запрос по аналоговым каналам AI---------------------------
    '            sSQL = "SELECT Шасси.keyШасси, Шасси.ИмяШасси, SettingAnalogInput.* " &
    '                "FROM Шасси RIGHT JOIN SettingAnalogInput ON Шасси.keyШасси = SettingAnalogInput.keyШасси " &
    '                "WHERE (((Шасси.ИмяШасси)= '" & ИмяШасси & "') AND ((SettingAnalogInput.Use)=True));"
    '            cmmDbCommand.CommandText = sSQL

    '            Using drDataReader As OleDbDataReader = cmmDbCommand.ExecuteReader(CommandBehavior.CloseConnection)
    '                Do While drDataReader.Read
    '                    If Convert.ToBoolean(drDataReader("Use")) Then
    '                        'Convert.ToInt16(drDataReader("Number"))
    '                        Dim nameChannel As String = Convert.ToString(drDataReader("NameNetVar"))
    '                        Me.ChannelsChassisDataSetFrm.Test.AddTestRow(idModule & "<" & nameChannel & ">", nameChannel, idModule, True)
    '                    End If
    '                Loop
    '            End Using
    '        End If
    '    End Using
    'End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ButtonOK.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ButtonCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SelectChannelDialogForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text & vbCrLf &
                                                    " с выбором пользователем канала: <" & ChannelName & ">" & vbCrLf &
                                                    " для шасси: <" & ChassisName & ">")
    End Sub

    Private Sub DataTreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles DataTreeView1.AfterSelect
        ButtonOK.Enabled = False

        TextBoxShassis.Visible = False
        TextBoxChannelName.BackColor = Color.Red
        Dim selNode As DataTreeViewNode = TryCast(DataTreeView1.SelectedNode, DataTreeViewNode)
        If selNode IsNot Nothing Then

            If selNode.IsChannel Then
                'selNode.ImageKey = "Selected"
                TextBoxShassis.Visible = True
                ButtonOK.Enabled = True

                TextBoxShassis.Text = selNode.Parent.Parent.Text 'TryCast(selNode.Parent.Parent, DataTreeViewNode).Text
                TextBoxShassis.BackColor = Color.Lime
                TextBoxChannelName.BackColor = Color.Lime
            End If
            'LabelTag.Text = selNode.Tag
            'RefreshTreeView()
            'Timer1.Enabled = True
        End If
    End Sub

    Private Sub TextBoxChannelName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxChannelName.TextChanged
        ChannelName = TextBoxChannelName.Text
    End Sub

    Private Sub TextBoxShassis_TextChanged(sender As Object, e As EventArgs) Handles TextBoxShassis.TextChanged
        ChassisName = TextBoxShassis.Text
    End Sub

    ' Не работают назначения ImageKey
    'Private Sub RefreshTreeView()
    '    For Each rootNode As TreeNode In DataTreeView1.Nodes
    '        rootNode.ImageKey = "NotSelected"
    '        If rootNode.Nodes.Count > 0 Then
    '            RefreshNode(rootNode)
    '        End If
    '    Next
    'End Sub

    'Private Sub RefreshNode(UpNode As TreeNode)
    '    UpNode.ImageKey = "NotSelected"
    '    For Each mNode As TreeNode In UpNode.Nodes
    '        mNode.ImageKey = "NotSelected"
    '        If mNode.Nodes.Count > 0 Then
    '            RefreshNode(mNode)
    '        End If
    '    Next
    'End Sub

    'Private Sub Timer1_Tick(sender As Object, e As EventArgs)
    '    Timer1.Enabled = False
    '    RefreshTreeView()
    '    Dim selNode As DataTreeViewNode = TryCast(DataTreeView1.SelectedNode, DataTreeViewNode)
    '    If selNode IsNot Nothing Then
    '        If selNode.IsSelected Then
    '            selNode.ImageKey = "Selected"
    '        End If
    '    End If
    'End Sub

End Class
