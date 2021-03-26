Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.Threading
Imports NationalInstruments.DAQmx
Imports Registration.ChannelsNDataSet

''' <summary>
''' Редактор каналов базы данных
''' </summary>
Friend Class FormServiceBases
    Private Class ChannelNode
        Inherits TreeNode

        Friend Property NodeType() As NodeType
        Friend Property KeyId() As Integer
        Friend Property IsSubDirectoriesAdded() As Boolean

        Friend Sub New(ByVal inText As String, ByVal NodeType As NodeType, ByVal KeyId As Integer)
            MyBase.New(inText)

            Me.NodeType = NodeType
            Me.KeyId = KeyId
        End Sub
    End Class

#Region "Enum"
    Private Enum NodeType
        Chassis1 = 1 ' Корзина
        Module2 = 2 ' Модуль
        Channel3 = 3 ' Канал
        DAQBoard4 = 4 ' ПлатаАЦП
        TypeParameter5 = 5 ' ТипПараметра
    End Enum

    Private Enum UnitType
        Rotation0 = 0 ' "%", "кгс"
        Discrete1 = 1 ' "дел", "мм/с"
        Temperature2 = 2 ' "градус"
        Current3 = 3 ' "мкА"
        Presure4 = 4 ' "Кгсм"
        Petrol5 = 5 ' "кг/ч"
        Vibration6 = 6 ' "мм"
    End Enum

    ''' <summary>
    ''' Как и в Public UnitOfMeasureArray As String() = {"%", "дел", "мм", "градус", "Кгсм", "мм/с", "мкА", "кг/ч", "кгс"}
    ''' </summary>
    Private Enum NodeImage
        Rotation0 = 0 ' "%" "ОБОРОТЫ"
        Discrete1 = 1 ' "дел" "ДИСКРЕТНЫЕ"
        Evacuation2 = 2 ' "мм" "РАЗРЕЖЕНИЯ"
        Temperature3 = 3 ' "градус" "ТЕМПЕРАТУРЫ"
        Petrol4 = 4 ' "кг/ч" "РАСХОДЫ"
        Vibration5 = 5 ' "мм/с" "ВИБРАЦИЯ"
        Current6 = 6 ' "мкА" "ТОКИ"
        Presure7 = 7 ' "Кгсм" "ДАВЛЕНИЯ"
        Traction8 = 8 ' "кгс" "ТЯГА"
        SliderVertical9 = 9
        SliderHorizontal10 = 10
        SliderVertical11 = 11
        Close12 = 12
        Chassis13 = 13 ' Корзина
        ChassisSelected14 = 14 ' Корзина открыта
        Module15 = 15 ' Модуль
        ModuleSelected16 = 16 ' Модуль открыт
        DAQBoard17 = 17 ' Плата АЦП
        DAQBoardSelected18 = 18 ' Плата АЦП открыта
        Selected19 = 19 ' выделенный узел
    End Enum

    ''' <summary>
    ''' Уровни подузлов в которых происходит поиск
    ''' </summary>
    Private Enum LevelNode
        Unit1 = 1 'ТипПараметра
        Chassis2 = 2 ' Корзина
        Module3 = 3 ' Модуль
        Channel4 = 4 ' Канал
    End Enum
#End Region

    Private Const maxDevice As Integer = 16
    Private Const conSortASC As String = "НомерПараметра ASC"
    Private Const conSortDESC As String = "НомерПараметра DESC"

    Private isCanEditName As Boolean ' разрешено изменять имена если в базе нет снимков = true
    Private isNeedAfterSelect As Boolean = True
    Private isNeedBeforeExpand As Boolean = True
    Private isNeedSelectedIndexChanged As Boolean = True
    Private isCleaningTree As Boolean ' очистка дерева
    Private isCalledFromFinding As Boolean ' вызван из поиска
    Private isUpdateRowDBase As Boolean ' вызван из обновить строку базы
    Private isChannelsTreeBeforeExpand As Boolean ' вызван из раскрытия узла
    Private isFindingChannel As Boolean ' вызвана из поиска канала
    Private isFormLoaded As Boolean
    Private isNeedSaveDbase As Boolean ' необходимо вернуть изменения из временной базы в текущую рабочую

    Private mfrmBrowser As FormWebBrowser
    Private tabRowSelectPosition As Integer
    Private cn As OleDbConnection
    Private WithEvents ObjCurrencyManager As CurrencyManager
    Private dt As DataTable
    Private rowCurrentChannel As ChannelNRow
    Private rowChannel As ChannelNRow
    Private dv As DataView
    Private drvChannel As DataRowView

    Private ReadOnly random As Random = New Random
    Private sortTransaction As String = ""
    Private unit1, chassis2, module3, nameChannel4 As String
    Private numberChannel, numberChannelModule As String
    Private ReadOnly ParentFormMainMDI As FormMainMDI

#Region "Запуск формы"
    Public Sub New(inMainMdiForm As FormMainMDI)
        MyBase.New()
        ' This is required by the Windows Form Designer.
        InitializeComponent()
        Text = FormServiceBasesText
        ' Add any initialization after the InitializeComponent() call.
        MdiParent = inMainMdiForm
        ParentFormMainMDI = inMainMdiForm
        'Add any initialization after the InitializeComponent() call
        ComboBoxPhysicalChannel.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External))

        If (ComboBoxPhysicalChannel.Items.Count > 0) Then
            ComboBoxPhysicalChannel.SelectedIndex = 0
        End If
    End Sub

    Private Sub FormServiceBases_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'TODO: данная строка кода позволяет загрузить данные в таблицу "ChannelsNDataSet1.ChannelN". При необходимости она может быть перемещена или удалена.
        'Me.ChannelNTableAdapter.Fill(Me.ChannelsNDataSet1.ChannelN)

        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        UploadChildrenForms()

        With ParentFormMainMDI
            .MenuNewWindowDBaseChannels.Enabled = False
            MenuNewWindowRegistration.Enabled = .MenuNewWindowRegistration.Enabled
            MenuNewWindowSnapshot.Enabled = .MenuNewWindowSnapshot.Enabled
            MenuNewWindowTarir.Enabled = .MenuNewWindowTarir.Enabled
            MenuNewWindowClient.Enabled = .MenuNewWindowClient.Enabled
            MenuNewWindowDBaseChannels.Enabled = .MenuNewWindowDBaseChannels.Enabled
        End With

        LabelAggregate.Text = "Стенд №" & StandNumber
        CollectionForms.Add(Text, Me)

        ' заполнить размерностями
        For I As Integer = 0 To UBound(UnitOfMeasureArray)
            ListBoxUnit.Items.Add(New StringIntObject(UnitOfMeasureArray(I), I))
        Next

        PopulateChannelsNBaseDataSet(StandNumber)
        DataGridChannels.AutoGenerateColumns = False
        LoadTableChannels()
        IndicationUpdateDBase(False)

        If ChannelsNDataSet1.ChannelN.Rows.Count > 0 Then
            tabRowSelectPosition = 0
            rowCurrentChannel = CType(ChannelsNDataSet1.ChannelN.Rows(0), ChannelNRow)
            ShowCurrentRowOnTable()
        End If

        AddHandlerOnControls()
        FillChassis()

        cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        cn.Open()
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT COUNT(*) FROM БазаСнимков;"

        If CInt(cmd.ExecuteScalar) > 0 Then
            cn.Close()
            Const caption As String = "Проверка наличия записей"
            Const text As String = "В базе существуют записи испытаний!" & vbCrLf &
                                "Имена параметров редактировать невозможно." & vbCrLf &
                                "Для возможности редактирования имен необходимо заархивировать все снимки испытаний." & vbCrLf & vbCrLf &
                                "Произвести автоматический разбор записей в архив?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                ArchivingByOpeningWindow()
                TextBoxName.ReadOnly = False
                isCanEditName = True
            End If
        Else
            TextBoxName.ReadOnly = False
            isCanEditName = True
        End If

        CheckUseTCPClient()
        WindowState = FormWindowState.Maximized
        isFormLoaded = True
        IsFrmServiceBasesLoaded = True
        ReInitializeAnnotationAlarm()
    End Sub

    Private Sub FormServiceBasess_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        CheckToWriteUpdateDBase()
        If isNeedSaveDbase Then CopyToWorkChannelFromChannelN(StandNumber)
        If cn.State = ConnectionState.Open Then cn.Close()
    End Sub

    ''' <summary>
    ''' Проверка Изменения Базы для её Записи 
    ''' </summary>
    Private Sub CheckToWriteUpdateDBase()
        If BindingNavigatorSaveDBase.Enabled AndAlso isFormLoaded Then
            Const caption As String = "Проверка изменений"
            Const text As String = "Были внесены изменения в текущую базу." & vbCrLf & "Сохранить изменения?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")
            If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                SaveDBase()
            End If
        End If
    End Sub

    Private Sub FormServiceBasess_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        LoadChannels()
        ParentFormMainMDI.MenuNewWindowDBaseChannels.Enabled = True

        For Each itemForm In CollectionForms.Forms.Values
            If CStr(itemForm.Tag) = TagFormDaughter Then
                CType(itemForm, FormServiceBases).MenuNewWindowDBaseChannels.Enabled = ParentFormMainMDI.MenuNewWindowDBaseChannels.Enabled
            End If
            If CStr(itemForm.Tag) = TagFormTarir Then
                CType(itemForm, FormTarir).MenuNewWindowDBaseChannels.Enabled = ParentFormMainMDI.MenuNewWindowDBaseChannels.Enabled
            End If
        Next

        CollectionForms.Remove(Text)
        IsFrmServiceBasesLoaded = False

        Try
            Dispose()
        Catch ex As Exception
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(FormServiceBasess_FormClosed)}> {ex}")
        End Try

        'fMainForm.MenuStrip1.Visible = Not fMainForm.MdiChildren.Length > 1
    End Sub

    Private Sub MenuFileExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFileExit.Click
        Close()
    End Sub

    ''' <summary>
    ''' Связать события контролов с обработчиками
    ''' </summary>
    Private Sub AddHandlerOnControls()
        AddHandler TextBoxA0.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxA1.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxA2.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxA3.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxA4.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxA5.Validating, AddressOf ValidatingCheck
        AddHandler TextBoxOffset.Validating, AddressOf ValidatingCheck

        AddHandler TextBoxName.TextChanged, AddressOf ControlChanged
        AddHandler RadioButtonRSE.CheckedChanged, AddressOf ControlChanged
        AddHandler RadioButtonNRSE.CheckedChanged, AddressOf ControlChanged
        AddHandler RadioButton4WRITE.CheckedChanged, AddressOf ControlChanged
        AddHandler RadioButtonDIFF.CheckedChanged, AddressOf ControlChanged

        AddHandler RadioButtonDC.CheckedChanged, AddressOf ControlChanged
        AddHandler RadioButtonGround.CheckedChanged, AddressOf ControlChanged
        AddHandler RadioButtonAC.CheckedChanged, AddressOf ControlChanged

        AddHandler CheckBoxXC.CheckedChanged, AddressOf ControlChanged
        AddHandler CheckBoxEnableAlarm.CheckedChanged, AddressOf ControlChanged
        AddHandler CheckBoxVisiblePhoto.CheckedChanged, AddressOf ControlChanged
        AddHandler CheckBoxVisibleRegister.CheckedChanged, AddressOf ControlChanged

        AddHandler ComboBoxLowLimit.SelectedIndexChanged, AddressOf ControlChanged
        AddHandler ComboBoxUpperLimit.SelectedIndexChanged, AddressOf ControlChanged
        AddHandler ComboBoxLevelOfApproximation.SelectedIndexChanged, AddressOf ControlChanged

        AddHandler TextBoxA0.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxA1.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxA2.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxA3.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxA4.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxA5.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxOffset.TextChanged, AddressOf ControlChanged
        AddHandler TextBoxNote.TextChanged, AddressOf ControlChanged

        AddHandler ListBoxFormula.SelectedIndexChanged, AddressOf ControlChanged
        AddHandler ListBoxUnit.SelectedIndexChanged, AddressOf ControlChanged

        AddHandler NumericLevelMin.ValueChanged, AddressOf ControlChanged
        AddHandler NumericLevelMax.ValueChanged, AddressOf ControlChanged
        AddHandler NumericSlidepercentMin.ValueChanged, AddressOf ControlChanged
        AddHandler NumericSlidepercentMax.ValueChanged, AddressOf ControlChanged
        AddHandler NumericEditAlarmMin.ValueChanged, AddressOf ControlChanged
        AddHandler NumericEditAlarmMax.ValueChanged, AddressOf ControlChanged
    End Sub

    ''' <summary>
    ''' Функция обратного вызова для проверки измениния настроек канала
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ControlChanged(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, Control).ForeColor = Color.Red
        BindingNavigatorUpdateRowDBase.Enabled = True
        LabelPanelRegistration.Text = "Изменилась настройка канала"
    End Sub

    ''' <summary>
    ''' Отключить доступность контролов в случае работы как клиента TCP
    ''' </summary>
    Private Sub CheckUseTCPClient()
        If IsUseTCPClient Then
            TextBoxName.ReadOnly = True
            TextBoxNDaq.ReadOnly = True
            TextBoxNDevice.ReadOnly = True
            TextBoxModule.ReadOnly = True
            TextBoxChannel.ReadOnly = True
            GroupBoxTypeConnection.Enabled = False
            GroupBoxTypeSignal.Enabled = False
            ComboBoxLowLimit.Enabled = False
            ComboBoxUpperLimit.Enabled = False

            ComboBoxLevelOfApproximation.Enabled = False
            TextBoxA0.ReadOnly = True
            TextBoxA1.ReadOnly = True
            TextBoxA2.ReadOnly = True
            TextBoxA3.ReadOnly = True
            TextBoxA4.ReadOnly = True
            TextBoxA5.ReadOnly = True
            TextBoxOffset.ReadOnly = True
            CheckBoxXC.Enabled = False
            TextBoxFault.ReadOnly = True
        End If
    End Sub

    ''' <summary>
    ''' Выгрузить дочерние формы
    ''' </summary>
    Private Sub UploadChildrenForms()
        Dim itemForm As Form
        Dim KeysCopy As String() = CollectionForms.Forms.Keys.ToArray

        For I As Integer = KeysCopy.GetLength(0) - 1 To 0 Step -1
            itemForm = CollectionForms.Forms(KeysCopy(I))

            If CStr(itemForm.Tag) = TagFormDaughter OrElse CStr(itemForm.Tag) = TagFormTarir Then
                itemForm.Close()
            End If

            If CStr(itemForm.Tag) = TagFormSnapshot Then itemForm.Close()
        Next
    End Sub

    ''' <summary>
    ''' Загрузить Таблицу Данными
    ''' </summary>
    Private Sub LoadTableChannels()
        If dv IsNot Nothing Then RemoveHandler ToolStripContainer1.BindingContext(dv).PositionChanged, AddressOf Dv_PositionChanged
        dv = Nothing
        ChannelNTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        ' сделать типизированный TableAdapter, который заполняется по номеру стенда
        ' и имеет поиск канала по имени
        ' данная строка кода позволяет загрузить данные в таблицу "ChannelsNBaseDataSet.ChannelN".
        ChannelNTableAdapter.Fill(ChannelsNDataSet1.ChannelN)

        dt = ChannelsNDataSet1.Tables(CHANNEL_N)
        dv = dt.DefaultView
        dv.Sort = "НомерПараметра"
        ApplyFilterBindingSource()
        AddHandler ToolStripContainer1.BindingContext(dv).PositionChanged, AddressOf Dv_PositionChanged

        DataGridChannels.DataSource = dv
        ObjCurrencyManager = CType(DataGridChannels.BindingContext(dv), CurrencyManager)

        If dt.Rows.Count = 0 Then EnabledMenuAddChassisOrCard()
    End Sub

    ''' <summary>
    ''' Заполнить Отсоединённый Набор BaseChannels
    ''' Вначале копируется база Channel(НомерСтенда) на место ChannelN
    ''' затем создаётся подключение
    ''' </summary>
    ''' <param name="standNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateChannelsNBaseDataSet(standNumber As String)
        Dim tadleFrom As String = "Channel" & standNumber

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            If CheckExistTable(cn, tadleFrom) AndAlso CheckExistTable(cn, CHANNEL_N) Then
                Try
                    Using cmd As OleDbCommand = cn.CreateCommand
                        cmd.CommandType = CommandType.Text
                        'strSQL = "DROP TABLE ChannelN;"
                        cmd.CommandText = $"DELETE * FROM {CHANNEL_N};"
                        cmd.ExecuteNonQuery()
                        'strSQL = "SELECT " & tadleFrom & ".* INTO ChannelN FROM " & tadleFrom
                        cmd.CommandText = $"INSERT INTO {CHANNEL_N} SELECT * FROM {tadleFrom}" '& " IN " & """" & strПутьChannels & """" & ";"
                        cmd.ExecuteNonQuery()
                    End Using
                    Thread.Sleep(500)
                    Application.DoEvents()
                Catch ex As Exception
                    Dim caption As String = $"Ошибка копирования данных в процедуре <{NameOf(PopulateChannelsNBaseDataSet)}>."
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
                End Try
            Else
                Dim caption As String = $"Проверка наличия таблицы в процедуре <{NameOf(PopulateChannelsNBaseDataSet)}>."
                Dim text As String = $"Таблицы {tadleFrom} или {CHANNEL_N} не существует!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Возврат назад внесённых изменений из ChannelN 
    ''' в рабочую базу Channel(НомерСтенда).
    ''' </summary>
    ''' <param name="standNumber"></param>
    ''' <remarks></remarks>
    Private Sub CopyToWorkChannelFromChannelN(standNumber As String)
        Dim tadleInto As String = "Channel" & standNumber

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            Try
                Using cmd As OleDbCommand = cn.CreateCommand
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = $"DELETE * FROM {tadleInto};"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = $"INSERT INTO {tadleInto} SELECT * FROM {CHANNEL_N}"
                    cmd.ExecuteNonQuery()
                End Using
                Thread.Sleep(500)
                Application.DoEvents()
            Catch ex As Exception
                Dim caption As String = $"Ошибка копирования данных в процедуре <{NameOf(CopyToWorkChannelFromChannelN)}>."
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
            End Try
        End Using
    End Sub
#End Region

#Region "Обработчики событий меню"
    Public Sub MenuNewWindowRegistration_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowRegistration.Click
        ParentFormMainMDI.MenuNewWindowRegistration_Click(eventSender, eventArgs)
    End Sub

    Public Sub MenuNewWindowSnapshot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowSnapshot.Click
        ParentFormMainMDI.MenuNewWindowSnapshot_Click(eventSender, eventArgs)
    End Sub

    Public Sub MenuNewWindowTarir_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowTarir.Click
        ParentFormMainMDI.MenuNewWindowTarir_Click(eventSender, eventArgs)
    End Sub

    Public Sub MenuNewWindowClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowClient.Click
        ParentFormMainMDI.MenuNewWindowClient_Click(eventSender, eventArgs)
    End Sub

    Public Sub MenuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuHelpAbout.Click
        AboutForm = New FormAbout
        AboutForm.ShowDialog()
    End Sub

    Public Sub MenuHelpApplication_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuHelpApplication.Click
        mfrmBrowser = New FormWebBrowser
        mfrmBrowser.Show()
        mfrmBrowser.brwWebBrowser.Navigate(PathHelps)
        mfrmBrowser.Activate()
    End Sub

    Public Function GetImageFromUnit(unit As String) As Image
        Dim outUnit As UnitType = UnitType.Rotation0

        Select Case unit
            Case "%", "кгс"
                outUnit = UnitType.Rotation0
            Case "дел", "мм/с"
                outUnit = UnitType.Discrete1
            Case "градус"
                outUnit = UnitType.Temperature2
            Case "мкА"
                outUnit = UnitType.Current3
            Case "Кгсм"
                outUnit = UnitType.Presure4
            Case "кг/ч"
                outUnit = UnitType.Petrol5
            Case "мм"
                outUnit = UnitType.Vibration6
                'Case Else
        End Select

        Return ImageList2.Images(outUnit)
    End Function

    Private Sub ListBoxUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxUnit.SelectedIndexChanged
        PictureBoxControl.Image = GetImageFromUnit(ListBoxUnit.SelectedItem.ToString)
        LabelPhysicsMin.Text = ListBoxUnit.SelectedItem.ToString
        LabelPhysicsMax.Text = ListBoxUnit.SelectedItem.ToString
    End Sub

    Private Sub ListBoxUnit_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ListBoxUnit.DrawItem
        DrawItemListBox(sender, die, ImageListTree)
    End Sub

    Private Sub ListBoxUnit_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ListBoxUnit.MeasureItem
        mie.ItemHeight = ListBoxUnit.ItemHeight - 2
    End Sub

    Private Sub RadioButtonViewModule_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonViewModule.CheckedChanged
        CheckToWriteUpdateString()
        If isFormLoaded Then
            If RadioButtonViewModule.Checked Then
                RadioButtonViewModule.BackColor = Color.Blue
                RadioButtonViewModule.ForeColor = Color.Yellow
                RadioButtonViewType.BackColor = Color.Silver
                RadioButtonViewType.ForeColor = Color.Black
            Else
                RadioButtonViewModule.BackColor = Color.Silver
                RadioButtonViewModule.ForeColor = Color.Black
                RadioButtonViewType.BackColor = Color.Blue
                RadioButtonViewType.ForeColor = Color.Yellow
            End If
            FillChassis()
        End If
    End Sub

    ''' <summary>
    ''' Поиск канала
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonFindChannel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFindChannel.Click
        Using frmSearchChannel As New FormSearchChannel(dv, dt)
            If frmSearchChannel.ShowDialog = DialogResult.OK Then
                numberChannel = frmSearchChannel.NumberChannel
                chassis2 = frmSearchChannel.Chassis
                module3 = frmSearchChannel.ModuleInChassis
                numberChannelModule = frmSearchChannel.NumberChannelModule
                nameChannel4 = frmSearchChannel.ChannelName
                unit1 = frmSearchChannel.Unit
                isFindingChannel = True
                SelectChannelFromFinding()
            End If
        End Using
        isFindingChannel = False
    End Sub
#End Region

#Region "Проводник и таблица"
    ''' <summary>
    ''' Заполнить контролы данными выделенной строки
    ''' </summary>
    Private Sub ShowCurrentRowOnTable()
        BindingNavigatorMoveFirstItem.Enabled = True
        BindingNavigatorMovePreviousItem.Enabled = True
        BindingNavigatorMoveNextItem.Enabled = True
        BindingNavigatorMoveLastItem.Enabled = True

        If rowCurrentChannel IsNot Nothing AndAlso tabRowSelectPosition <> -1 Then
            LabelDescriptionStage.Text = $"Выбрать канал для редактирование в дереве или в таблице (запись {tabRowSelectPosition + 1} из {dv.Count})" ' или {DataGridChannels.Rows.Count}
            If tabRowSelectPosition = 0 Then
                BindingNavigatorMoveFirstItem.Enabled = False
                BindingNavigatorMovePreviousItem.Enabled = False
            ElseIf tabRowSelectPosition = dv.Count - 1 Then
                BindingNavigatorMoveNextItem.Enabled = False
                BindingNavigatorMoveLastItem.Enabled = False
            End If

            FillFieldsRecord()
            SelectedNodeInTree()
            DataGridChannels.Rows(tabRowSelectPosition).Selected = True

            'DataGridChannels.Rows(ToolStripContainer1.BindingContext(dv).Position).Selected = True
            'DataGridChannels.Rows(DataGridChannels.CurrentRow.Index).Selected = True
            'DataGridChannels.CurrentRow.Selected = True

            'BindingSourceChannelsN.Position = tabRowSelectPosition
            'DataGridChannels.Rows(BindingSourceChannelsN.Position).Selected = True

            For Each itemPanel As Panel In New Panel() {Panel1, Panel2, Panel3, Panel4}
                For Each itemControl As Control In itemPanel.Controls
                    SetForeColorConcreteControl(itemControl)
                Next
            Next

            BindingNavigatorUpdateRowDBase.Enabled = False ' при изменении значения какого либо поля сделать доступным
        Else
            rowCurrentChannel = Nothing
            LabelDescriptionStage.Text = "Нет записей"
            BindingNavigatorMoveFirstItem.Enabled = False
            BindingNavigatorMovePreviousItem.Enabled = False
            BindingNavigatorMoveNextItem.Enabled = False
            BindingNavigatorMoveLastItem.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Задать цвет контрола по умолчанию в рекурсивном обходе
    ''' </summary>
    ''' <param name="ctl"></param>
    Private Sub SetForeColorConcreteControl(ByVal ctl As Control)
        ' проверить содержит ли контрол внутри себя
        If ctl.Controls.Count <> 0 Then
            For Each itemControl As Control In ctl.Controls
                itemControl.ForeColor = SystemColors.WindowText
                SetForeColorConcreteControl(itemControl)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Обработчик изменения позиции.
    ''' Происходит после изменения значения свойства System.Windows.Forms.BindingManagerBase.Position.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Dv_PositionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If dv Is Nothing Then Exit Sub

        LabelDescriptionStage.Text = ""
        If isFormLoaded AndAlso isChannelsTreeBeforeExpand = False AndAlso isFindingChannel = False Then
            Try
                CheckToWriteUpdateString()
                tabRowSelectPosition = ToolStripContainer1.BindingContext(dv).Position
                ShowCurrentRowOnTable()
                BindingSourceChannelsN.Position = ToolStripContainer1.BindingContext(dv).Position
            Catch ex As Exception
                Const caption As String = "Изменение активной строки таблицы"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
        End If
    End Sub
    Private Sub BindingSourceChannelsN_PositionChanged(sender As Object, e As EventArgs) Handles BindingSourceChannelsN.PositionChanged
        'Dv_PositionChanged(dv, New EventArgs)
        ToolStripContainer1.BindingContext(dv).Position = BindingSourceChannelsN.Position
    End Sub

    ''' <summary>
    ''' Заполнить Корзины
    ''' Выделить Канал Из Поиска
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TimerRefresh_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerRefresh.Tick
        TimerRefresh.Enabled = False
        FillChassis()
        SelectChannelFromFinding()
    End Sub

    ''' <summary>
    ''' Заполнить Поля Записи
    ''' </summary>
    Private Sub FillFieldsRecord()
        ' при первоначальной инициализации и сразу задать поиск, то может быть DataGridChannels.CurrentRow Is Nothing
        If DataGridChannels.CurrentRow Is Nothing Then TreeChannels.SelectedNode = TreeChannels.SelectedNode.Nodes(0).Nodes(0)
        ' из пустой таблице вывод аттрибутов выделеной записи невозможен
        If DataGridChannels.CurrentRow.Index = -1 Then Exit Sub

        Try
            drvChannel = dv(ToolStripContainer1.BindingContext(dv).Position)
            rowChannel = CType(drvChannel.Row, ChannelNRow)
            LabelNumber.Text = CStr(rowChannel.НомерПараметра)
            TextBoxName.Text = rowChannel.НаименованиеПараметра
            TextBoxNDaq.Text = CStr(rowChannel.НомерКанала)
            TextBoxNDevice.Text = CStr(rowChannel.НомерУстройства)

            ' здесь ошибка дизайнера
            'If IsDBNull(rowChannel.НомерМодуляКорзины) Then
            'If IsDBNull(rowChannel.НомерКаналаМодуля) Then

            If IsDBNull(CType(rowChannel, DataRow)("НомерМодуляКорзины")) Then
                TextBoxModule.Text = vbNullString
            Else
                TextBoxModule.Text = rowChannel.НомерМодуляКорзины
            End If

            If IsDBNull(CType(rowChannel, DataRow)("НомерКаналаМодуля")) Then
                TextBoxChannel.Text = vbNullString
            Else
                TextBoxChannel.Text = rowChannel.НомерКаналаМодуля
            End If

            ' ТипПодключения '="DIFF" Or ="RSE" Or ="NRSE" Or ="4WRITE"
            Select Case rowChannel.ТипПодключения
                Case "RSE" ' в DAQmx "Rse"
                    RadioButtonRSE.Checked = True
                    Exit Select
                Case "NRSE" ' в DAQmx "Nrse"
                    RadioButtonNRSE.Checked = True
                    'Case "Pseudodifferential" ' у меня нет
                    Exit Select
                Case "4WRITE"
                    RadioButton4WRITE.Checked = True
                    'Case "Let NI-DAQ Choose" ' у меня нет
                    Exit Select
                Case Else '"DIFF"
                    RadioButtonDIFF.Checked = True
                    Exit Select
            End Select

            ' ТипСигнала '="DC" Or ="AC"
            Select Case rowChannel.ТипСигнала
                Case "DC"
                    RadioButtonDC.Checked = True
                    Exit Select
                Case "Gnd" 'у меня нет
                    RadioButtonGround.Checked = True
                    Exit Select
                Case Else '"AC"
                    RadioButtonAC.Checked = True
                    Exit Select
            End Select

            ComboBoxLowLimit.SelectedIndex = ComboBoxLowLimit.Items.IndexOf(rowChannel.НижнийПредел.ToString)
            ComboBoxUpperLimit.SelectedIndex = ComboBoxUpperLimit.Items.IndexOf(rowChannel.ВерхнийПредел.ToString)

            ' НомерФормулы '=1 Or =2 Or =3
            ListBoxFormula.SelectedIndex = Array.IndexOf({1, 2, 3}, rowChannel.НомерФормулы)

            'myIntArray = New Integer() {0, 1, 2, 3, 4, 5}
            ' СтепеньАппроксимации '=0 Or =1 Or =2 Or =3 Or =4 Or =5
            ComboBoxLevelOfApproximation.SelectedIndex = ComboBoxLevelOfApproximation.Items.IndexOf(rowChannel.СтепеньАппроксимации.ToString)

            TextBoxA0.Text = CStr(rowChannel.A0)
            TextBoxA1.Text = CStr(rowChannel.A1)
            TextBoxA2.Text = CStr(rowChannel.A2)
            TextBoxA3.Text = CStr(rowChannel.A3)
            TextBoxA4.Text = CStr(rowChannel.A4)
            TextBoxA5.Text = CStr(rowChannel.A5)
            TextBoxOffset.Text = CStr(rowChannel.Смещение)
            CheckBoxXC.Checked = CBool(rowChannel.КомпенсацияХС)

            ' здесь ошибка дизайнера
            'If IsDBNull(rowChannel.Погрешность) Then
            If IsDBNull(CType(rowChannel, DataRow)("Погрешность")) Then
                TextBoxFault.Text = "0"
            Else
                TextBoxFault.Text = CStr(rowChannel.Погрешность) 'не может измениться
            End If

            Dim I As Integer = Array.IndexOf(UnitOfMeasureArray, rowChannel.ЕдиницаИзмерения)
            If I = -1 Then I = 4 '"Кгсм"
            ListBoxUnit.SelectedIndex = I

            TextBoxNote.Text = rowChannel.Примечания
            NumericLevelMin.Value = rowChannel.ДопускМинимум
            NumericLevelMax.Value = rowChannel.ДопускМаксимум
            NumericSlidepercentMin.Value = rowChannel.РазносУмин
            NumericSlidepercentMax.Value = rowChannel.РазносУмакс
            NumericEditAlarmMin.Value = rowChannel.АварийноеЗначениеМин
            NumericEditAlarmMax.Value = rowChannel.АварийноеЗначениеМакс
            CheckBoxEnableAlarm.Checked = CBool(rowChannel.Блокировка)
            CheckBoxVisiblePhoto.Checked = CBool(rowChannel.Видимость)
            CheckBoxVisibleRegister.Checked = CBool(rowChannel.ВидимостьРегистратор)
            XyPointAnnotationMin.Caption = "Min=" & NumericLevelMin.Value.ToString("F")
            XyPointAnnotationMax.Caption = "Max=" & NumericLevelMax.Value.ToString("F")
        Catch ex As Exception
            Const caption As String = "Заполнение полей из активной строки таблицы"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            BindingNavigatorUpdateRowDBase.Enabled = False ' при изменении значения какого либо поля сделать доступным
            LabelPanelRegistration.Text = "Готово"
        End Try
    End Sub

    ''' <summary>
    ''' Выделить Узел в дереве
    ''' </summary>
    Private Sub SelectedNodeInTree()
        If dv.Count > 0 AndAlso isFormLoaded Then
            Dim nameChannel As String

            If isCalledFromFinding Then
                nameChannel = nameChannel4
            Else
                nameChannel = rowChannel.НаименованиеПараметра
            End If

            If TreeChannels.SelectedNode IsNot Nothing AndAlso isNeedSelectedIndexChanged Then
                Dim toSelectedChannelNode As ChannelNode = Nothing ' Для Выделения
                Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)
                Dim parentNode As TreeNode = Nothing

                If nodeSelect.NodeType = NodeType.Channel3 Then
                    parentNode = nodeSelect.Parent
                ElseIf nodeSelect.NodeType = NodeType.Module2 OrElse nodeSelect.NodeType = NodeType.DAQBoard4 Then
                    parentNode = nodeSelect
                ElseIf nodeSelect.NodeType = NodeType.Chassis1 OrElse nodeSelect.NodeType = NodeType.TypeParameter5 Then
                    parentNode = Nothing
                    FindExpandedNode(nodeSelect, CType(parentNode, ChannelNode))
                End If

                If parentNode IsNot Nothing Then
                    For I As Integer = 0 To parentNode.Nodes.Count - 1
                        If parentNode.Nodes(I).Tag.ToString = nameChannel Then
                            toSelectedChannelNode = CType(parentNode.Nodes(I), ChannelNode)
                            Exit For
                        End If
                    Next
                End If

                If toSelectedChannelNode IsNot Nothing Then
                    TreeChannels.SelectedNode = toSelectedChannelNode
                End If
            End If

            If tabRowSelectPosition <> 0 Then
                DataGridChannels.CurrentRow.Selected = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Рекурсивное нахождение раскрытого родительского узла Module2 или DAQBoard4
    ''' для выделенного узла Chassis1 или TypeParameter5
    ''' </summary>
    ''' <param name="nodeSelect"></param>
    Private Sub FindExpandedNode(ByVal nodeSelect As ChannelNode, ByRef expandedParentNode As ChannelNode)
        If expandedParentNode Is Nothing Then
            For Each itemNode As ChannelNode In nodeSelect.Nodes
                If itemNode.IsExpanded Then
                    If itemNode.NodeType = NodeType.Module2 OrElse itemNode.NodeType = NodeType.DAQBoard4 Then
                        expandedParentNode = itemNode
                        Exit Sub
                    End If
                    FindExpandedNode(itemNode, expandedParentNode)
                End If
            Next
        End If
    End Sub

#Region "Навигация по таблице"
    Private Sub ShowCurrentRow()
        ' UnSelect
        DataGridChannels.CurrentRow.Selected = False
        rowCurrentChannel = CType(dv(tabRowSelectPosition).Row, ChannelNRow)
        ToolStripContainer1.BindingContext(dv).Position = tabRowSelectPosition
        ShowCurrentRowOnTable()
    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        'If BindingSourceChannelsN.Count > 0 Then BindingSourceChannelsN.MoveFirst()
        CheckToWriteUpdateString()
        FirstRecord()
    End Sub

    Private Sub FirstRecord()
        If dv.Count > 0 AndAlso tabRowSelectPosition <> 0 AndAlso DataGridChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = 0
            ShowCurrentRow()
        End If
    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        'If BindingSourceChannelsN.Count > 0 AndAlso BindingSourceChannelsN.Position > 0 Then BindingSourceChannelsN.MovePrevious()
        CheckToWriteUpdateString()
        PreviousRecord()
    End Sub

    Private Sub PreviousRecord()
        If tabRowSelectPosition > 0 AndAlso DataGridChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition -= 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        'If BindingSourceChannelsN.Count > 0 AndAlso BindingSourceChannelsN.Position + 1 < BindingSourceChannelsN.Count Then BindingSourceChannelsN.MoveNext()
        CheckToWriteUpdateString()
        NextRecord()
    End Sub

    Private Sub NextRecord()
        If dv.Count > tabRowSelectPosition + 1 AndAlso DataGridChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition += 1
            ShowCurrentRow()
        End If
    End Sub

    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        'If BindingSourceChannelsN.Count > 0 Then BindingSourceChannelsN.MoveLast()
        CheckToWriteUpdateString()
        LastRecord()
    End Sub

    Private Sub LastRecord()
        If dv.Count > 0 AndAlso dv.Count <> tabRowSelectPosition + 1 AndAlso DataGridChannels.CurrentRow.Index <> -1 Then
            tabRowSelectPosition = dv.Count - 1
            ShowCurrentRow()
        End If
    End Sub

    ''' <summary>
    ''' Проверка Изменения Строки для их сохранения
    ''' </summary>
    Private Sub CheckToWriteUpdateString()
        If BindingNavigatorUpdateRowDBase.Enabled AndAlso isFormLoaded AndAlso isUpdateRowDBase = False Then
            If MessageBox.Show("Были внесены изменения в текущую строку." & vbCrLf & "Сохранить изменения?",
                               "Проверка изменений", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                UpdateRowDBase()
            Else
                BindingNavigatorUpdateRowDBase.Enabled = False
                LabelPanelRegistration.Text = "Готово"
            End If
        End If
    End Sub

    'Private previousPosition As Integer
    'Private Sub ButtonRowAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRowAdd.Click
    '    Dim newDataRowView As DataRowView = dv.AddNew
    '    rowCurrentChannel = newDataRowView.Row
    '    previousPosition = intTabPosition
    '    intTabPosition = dv.Count - 1
    '    ShowCurrentRowTable()
    '    SetTarRowEditMode(True)
    '    If rowCurrentChannel Is Nothing Then
    '        MessageBox.Show("Нет текущей записи!", "Добавление записи", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Exit Sub
    '    End If
    '    Dim frmEditDetail As New frmEditDetail(Me)
    '    frmEditDetail.EditDetail(newDataRowView)
    '    frmEditDetail.Dispose()
    'End Sub

    'Private Sub ButtonRowDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRowDelete.Click
    '    If dv.Count > 0 Then
    '        dv.Delete(intTabPosition)
    '        If dv.Count > 0 Then
    '            If intTabPosition = dv.Count Then
    '                intTabPosition -= 1
    '            End If
    '            rowCurrentChannel = dv(intTabPosition).Row
    '        Else
    '            rowCurrentChannel = Nothing
    '        End If
    '        ShowCurrentRowTable()
    '        SetTarRowEditMode(False)
    '    Else
    '        MessageBox.Show("Нет текущих записей для удаления!", "Удаление записи", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End If
    'End Sub

    'Private Sub ButtonRowEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRowEdit.Click
    '    If dv.Count > 0 Then
    '        SetTarRowEditMode(True)
    '        If rowCurrentChannel Is Nothing Then
    '            MessageBox.Show("Нет текущей записи!", "Добавление записи", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End If
    '        Dim frmEditDetail As New frmEditDetail(Me)
    '        frmEditDetail.EditDetail(dv(DataGridChannels.CurrentRowIndex))
    '        frmEditDetail.Dispose()
    '    Else
    '        MessageBox.Show("Нет текущих записей для редактирования!", "Редактирование записи", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End If
    'End Sub

    'Public Sub SetTarRowCancel()
    '    intTabPosition = previousPosition
    '    If dv.Count > 0 Then
    '        rowCurrentChannel = dv(intTabPosition).Row
    '    Else
    '        rowCurrentChannel = Nothing
    '    End If

    '    ShowCurrentRowTable()
    '    SetTarRowEditMode(False)
    'End Sub
#End Region

#Region "SaveDBase"
    Private Sub BindingNavigatorSaveDBase_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorSaveDBase.Click, ButtonSave.Click, MenuFileSave.Click
        SaveDBase()
    End Sub

    Private Sub SaveDBase()
        Validate()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Записать В Базу")
        CheckToWriteUpdateString()

        BindingSourceChannelsN.EndEdit()
        If ChannelsNDataSet1.HasChanges Then

            Dim deletedChildRecords = CType(ChannelsNDataSet1.ChannelN.GetChanges(DataRowState.Deleted), ChannelsNDataSet.ChannelNDataTable)
            Dim newChildRecords = CType(ChannelsNDataSet1.ChannelN.GetChanges(DataRowState.Added), ChannelsNDataSet.ChannelNDataTable)
            Dim modifiedChildRecords = CType(ChannelsNDataSet1.ChannelN.GetChanges(DataRowState.Modified), ChannelsNDataSet.ChannelNDataTable)

            Try
                Dim countRowModified As Integer
                If deletedChildRecords IsNot Nothing Then
                    ChannelNTableAdapter.Update(deletedChildRecords)
                    countRowModified += deletedChildRecords.Rows.Count
                End If
                If modifiedChildRecords IsNot Nothing Then
                    ChannelNTableAdapter.Update(modifiedChildRecords)
                    countRowModified += modifiedChildRecords.Rows.Count
                End If
                If newChildRecords IsNot Nothing Then
                    ChannelNTableAdapter.Update(newChildRecords)
                    countRowModified += newChildRecords.Rows.Count
                End If

                ChannelsNDataSet1.ChannelN.AcceptChanges()
                UpdateRegimeStend()

                Const caption As String = "Обновление успешно!"
                Dim text As String = $"Изменено {countRowModified} записей"
                If isNeedSaveDbase = False AndAlso countRowModified <> 0 Then isNeedSaveDbase = True
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Catch ex As Exception
                Const caption As String = "Обновление не успешно!"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Finally
                If deletedChildRecords IsNot Nothing Then deletedChildRecords.Dispose()
                If modifiedChildRecords IsNot Nothing Then modifiedChildRecords.Dispose()
                If newChildRecords IsNot Nothing Then newChildRecords.Dispose()
            End Try
        Else
            MessageBox.Show("Нет изменений для записи!", "Запись изменений", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        IndicationUpdateDBase(False)
    End Sub

    '''' <summary>
    '''' Записать в базу
    '''' </summary>
    'Private Sub SaveDBaseOld()
    '    RegistrationEventLog.EventLog_MSG_USER_ACTION("Записать В Базу")
    '    CheckToWriteUpdateString()

    '    If ChannelsNDataSet1.HasChanges Then
    '        Dim txn As OleDbTransaction = Nothing

    '        Try
    '            If (cn.State = ConnectionState.Closed) Then cn.Open()

    '            txn = StartTransaction()
    '            'Me.BindingContext(dt).EndCurrentEdit()
    '            'ToolStripContainer1.BindingContext(dt).EndCurrentEdit()
    '            BindingSourceChannelsN.EndEdit()

    '            ' работает при удалении
    '            Dim countRowModified As Integer = ChannelNTableAdapter.Update(ChannelsNDataSet1.ChannelN.Select("", "", DataViewRowState.Deleted))
    '            countRowModified += ChannelNTableAdapter.Update(ChannelsNDataSet1.ChannelN.Select("", sortTransaction, DataViewRowState.ModifiedCurrent))
    '            'работает при добавлении
    '            countRowModified += ChannelNTableAdapter.Update(ChannelsNDataSet1.ChannelN.Select("", "", DataViewRowState.Added))
    '            sortTransaction = ""
    '            CommitTransaction(txn)
    '            Const caption As String = "Обновление успешно!"
    '            Dim text As String = $"Изменено {countRowModified} записей"
    '            MessageBox.Show(Text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {Text}")
    '        Catch ex As Exception
    '            RollbackTransaction(txn)
    '            Const caption As String = "Обновление не успешно!"
    '            Dim text As String = ex.ToString
    '            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
    '        End Try
    '    Else
    '        MessageBox.Show("Нет изменений для записи!", "Запись изменений", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End If

    '    IndicationUpdateDBase(False)
    'End Sub

    'Private Function StartTransaction() As OleDbTransaction
    '    Dim myDataRowsCommandBuilder As OleDbCommandBuilder = New OleDbCommandBuilder(ChannelNTableAdapter.Adapter)

    '    ChannelNTableAdapter.Adapter.UpdateCommand = myDataRowsCommandBuilder.GetUpdateCommand
    '    ChannelNTableAdapter.Adapter.InsertCommand = myDataRowsCommandBuilder.GetInsertCommand
    '    ChannelNTableAdapter.Adapter.DeleteCommand = myDataRowsCommandBuilder.GetDeleteCommand

    '    Dim txn As OleDbTransaction = cn.BeginTransaction
    '    ChannelNTableAdapter.Adapter.UpdateCommand.Transaction = txn
    '    ChannelNTableAdapter.Adapter.InsertCommand.Transaction = txn
    '    ChannelNTableAdapter.Adapter.DeleteCommand.Transaction = txn
    '    'ChannelNTableAdapter.Transaction = txn

    '    Return txn
    'End Function

    'Private Sub RollbackTransaction(ByVal txn As OleDbTransaction)
    '    txn.Rollback()
    '    cn.Close()
    'End Sub

    'Private Sub CommitTransaction(ByVal txn As OleDbTransaction)
    '    txn.Commit()
    '    UpdateRegimeStend()
    '    cn.Close()
    'End Sub

    ''' <summary>
    ''' Изменить режимы стенда
    ''' </summary>
    Private Sub UpdateRegimeStend()
        If (cn.State = ConnectionState.Closed) Then cn.Open()

        If dt.Rows.Count > 0 Then
            UpdateModeRecord("Отладочный режим")
            UpdateModeRecord("Регистратор")
        End If
        cn.Close()
    End Sub

    ''' <summary>
    ''' Обновить в таблице "РежимыNN" записи "Отладочный режим" и "Регистратор",
    ''' если ни один параметр в таблице формы не найден в перечне параметров этих таблиц
    ''' </summary>
    ''' <param name="inMode"></param>
    Private Sub UpdateModeRecord(ByVal inMode As String)
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        cmd.CommandText = $"SELECT ПереченьПараметров FROM Режимы{StandNumber} WHERE ([Наименование]= '{inMode}')"

        Dim lsitParams As String = CStr(cmd.ExecuteScalar)
        Dim isAllNotFound As Boolean = True ' предположение, что ни один не найден

        For I As Integer = 0 To dt.Rows.Count - 1
            If lsitParams.IndexOf(CStr(dt.Rows(I).Item("НаименованиеПараметра"))) <> -1 Then
                ' найден какой-то параметр, значит таблица просто модифицирована, а не абсолютно новая
                isAllNotFound = False
                Exit For
            End If
        Next

        If isAllNotFound Then
            ' если ни один параметр из таблицы формы не найден в строке базы, 
            ' то скорее всего было удаление всех каналов, значит перезапись перечня заново            
            cmd.CommandText = $"Update Режимы{StandNumber} SET ПереченьПараметров = '{CStr(dt.Rows(0).Item("НаименованиеПараметра")) & "\"}' WHERE ([Наименование]= '{inMode}')"
            cmd.ExecuteNonQuery()
        End If
    End Sub
#End Region

    Private Sub BindingNavigatorUpdateRowDBase_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BindingNavigatorUpdateRowDBase.Click
        UpdateRowDBase()
    End Sub
    ''' <summary>
    ''' Обновить строку базы
    ''' </summary>
    Private Sub UpdateRowDBase()
        If CheckLimits() Then
            If isCanEditName AndAlso rowChannel.НаименованиеПараметра <> TextBoxName.Text AndAlso CheckIdentityName(TextBoxName.Text) Then
                MessageBox.Show($"Наименование канала: {TextBoxName.Text} уже существует!{vbCrLf}Введите другое имя.",
                                "Обновить строку базы", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            isUpdateRowDBase = True ' заблокировать повторный вызов
            rowChannel.BeginEdit()
            'rowChannel.НомерПараметра= CInt(LabelNumber.Text) ' Длинное целое

            If isCanEditName AndAlso rowChannel.НаименованиеПараметра <> TextBoxName.Text Then
                ' сначало изменить имя в дереве
                For Each itemNode As ChannelNode In TreeChannels.Nodes
                    FindNodeNameThenChangeName(itemNode, CStr(rowChannel.НаименованиеПараметра), TextBoxName.Text, False)
                Next

                rowChannel.НаименованиеПараметра = TextBoxName.Text
            End If

            'rowChannel.НомерКанала= CInt(TextBoxNDaq.Text) ' Длинное целое
            'rowChannel.НомерУстройства= CInt(TextBoxNDevice.Text) ' Длинное целое
            'If Not TextBoxModule.Text = "" Then
            '    rowChannel.НомерМодуляКорзины= TextBoxModule.Text
            'Else
            '    rowChannel.НомерМодуляКорзины= vbNullString
            'End If

            'If Not TextBoxChannel.Text = "" Then
            '    rowChannel.НомерКаналаМодуля= TextBoxChannel.Text
            'Else
            '    rowChannel.НомерКаналаМодуля= vbNullString
            'End If

            'ТипПодключения '="DIFF" Or ="RSE" Or ="NRSE" Or ="4WRITE"
            If RadioButtonRSE.Checked = True Then
                rowChannel.ТипПодключения = "RSE"
            ElseIf RadioButtonNRSE.Checked = True Then
                rowChannel.ТипПодключения = "NRSE"
            ElseIf RadioButton4WRITE.Checked = True Then
                rowChannel.ТипПодключения = "4WRITE"
            ElseIf RadioButtonDIFF.Checked = True Then
                rowChannel.ТипПодключения = "DIFF"
            End If

            'ТипСигнала '="DC" Or ="AC"
            If RadioButtonDC.Checked = True Then
                rowChannel.ТипСигнала = "DC"
            ElseIf RadioButtonGround.Checked = True Then
                rowChannel.ТипСигнала = "Gnd"
            ElseIf RadioButtonAC.Checked = True Then
                rowChannel.ТипСигнала = "AC"
            End If

            rowChannel.НижнийПредел = CSng(ComboBoxLowLimit.Text)
            rowChannel.ВерхнийПредел = CSng(ComboBoxUpperLimit.Text)

            'НомерФормулы '=1 Or =2 Or =3
            rowChannel.НомерФормулы = ListBoxFormula.SelectedIndex + 1

            'СтепеньАппроксимации '=0 Or =1 Or =2 Or =3 Or =4 Or =5
            rowChannel.СтепеньАппроксимации = CInt(ComboBoxLevelOfApproximation.Text)

            rowChannel.A0 = CDbl(TextBoxA0.Text)
            rowChannel.A1 = CDbl(TextBoxA1.Text)
            rowChannel.A2 = CDbl(TextBoxA2.Text)
            rowChannel.A3 = CDbl(TextBoxA3.Text)
            rowChannel.A4 = CDbl(TextBoxA4.Text)
            rowChannel.A5 = CDbl(TextBoxA5.Text)
            rowChannel.Смещение = CDbl(TextBoxOffset.Text)

            rowChannel.КомпенсацияХС = CheckBoxXC.Checked
            'rowChannel.Погрешность= CSng(TextBoxFault.Text) 'не может измениться
            rowChannel.Примечания = TextBoxNote.Text

            rowChannel.ДопускМинимум = CSng(NumericLevelMin.Value)
            rowChannel.ДопускМаксимум = CSng(NumericLevelMax.Value)
            rowChannel.РазносУмин = CInt(NumericSlidepercentMin.Value)
            rowChannel.РазносУмакс = CInt(NumericSlidepercentMax.Value)
            rowChannel.АварийноеЗначениеМин = CSng(NumericEditAlarmMin.Value)
            rowChannel.АварийноеЗначениеМакс = CSng(NumericEditAlarmMax.Value)

            rowChannel.Блокировка = CheckBoxEnableAlarm.Checked
            rowChannel.Видимость = CheckBoxVisiblePhoto.Checked
            rowChannel.ВидимостьРегистратор = CheckBoxVisibleRegister.Checked
            rowChannel.EndEdit()

            BindingNavigatorUpdateRowDBase.Enabled = False
            LabelPanelRegistration.Text = "Готово"
            IndicationUpdateDBase(True)

            If rowChannel.ЕдиницаИзмерения <> ListBoxUnit.Text Then ' изменена Единица Измерения
                nameChannel4 = rowChannel.НаименованиеПараметра
                unit1 = ListBoxUnit.Text

                If RadioButtonViewModule.Checked Then
                    ' здесь без  dv.RowFilter = "" чтобы не вызывать событие смены dv
                    rowChannel.ЕдиницаИзмерения = ListBoxUnit.Text
                    ' изменить ЕдиницаИзмерения в узле дерева
                    For Each itemNode As ChannelNode In TreeChannels.Nodes
                        FindNode(itemNode, False)
                    Next
                Else
                    dv.RowFilter = ""
                    rowChannel.ЕдиницаИзмерения = ListBoxUnit.Text
                    numberChannel = CStr(rowChannel.НомерКанала)
                    chassis2 = CStr(rowChannel.НомерУстройства)

                    If Not IsDBNull(rowChannel.НомерМодуляКорзины) Then
                        module3 = rowChannel.НомерМодуляКорзины
                    Else
                        module3 = "" ' vbNullString
                    End If

                    If Not IsDBNull(rowChannel.НомерКаналаМодуля) Then
                        numberChannelModule = rowChannel.НомерКаналаМодуля
                    Else
                        numberChannelModule = "" ' vbNullString
                    End If

                    ' запустить таймер для заполнения корзины
                    TimerRefresh.Enabled = True
                End If
            End If

            ApplyFilterBindingSource()
            isUpdateRowDBase = False
        End If
    End Sub

    ''' <summary>
    ''' Проверка пределов
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckLimits() As Boolean
        Dim success As Boolean = True ' ошибка Заполнения

        If Val(ComboBoxLowLimit.Text) >= Val(ComboBoxUpperLimit.Text) Then
            MessageBox.Show($"Значение <Нижний предел={ComboBoxLowLimit.Text}> больше <Верхний предел={ComboBoxUpperLimit.Text}>!", "Ошибка при проверке", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        End If

        If NumericLevelMin.Value >= NumericLevelMax.Value Then
            MessageBox.Show($"Значение <Минимальное значение сигнала={NumericLevelMin.Value}> больше <Максимальное значение сигнала={NumericLevelMax.Value}>!", "Ошибка при проверке", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        End If

        If NumericSlidepercentMin.Value >= NumericSlidepercentMax.Value Then
            MessageBox.Show($"Значение <Разнос оси Y min={NumericSlidepercentMin.Value}> больше <Разнос оси Y max={NumericSlidepercentMax.Value}>!", "Ошибка при проверке", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        End If

        If NumericEditAlarmMin.Value > NumericEditAlarmMax.Value Then
            MessageBox.Show($"Значение <Аварийное значение min={NumericEditAlarmMin.Value}> больше <Аварийное значение max={NumericEditAlarmMax.Value}>!", "Ошибка при проверке", MessageBoxButtons.OK, MessageBoxIcon.Error)
            success = False
        End If

        Return success
    End Function

    ''' <summary>
    ''' Рекурсивный поиск узла с тегом=nameChannel4 во всём дереве
    ''' </summary>
    ''' <param name="treeNode"></param>
    ''' <param name="isNodeFounded"></param>
    Private Sub FindNode(ByVal treeNode As ChannelNode, ByVal isNodeFounded As Boolean)
        If isNodeFounded Then Exit Sub

        For Each itemNode As ChannelNode In treeNode.Nodes
            If itemNode.Tag.ToString = nameChannel4 Then
                If CBool(InStr(1, UnitOfMeasureString, unit1)) Then
                    itemNode.ImageIndex = Array.IndexOf(UnitOfMeasureArray, unit1)
                Else
                    itemNode.ImageIndex = NodeImage.Discrete1
                End If

                isNodeFounded = True
                Exit For
            End If

            FindNode(itemNode, isNodeFounded)
        Next
    End Sub

    ''' <summary>
    ''' Рекурсивный поиск узла с тегом=oldName во всём дереве и замена его на новое имя newName
    ''' </summary>
    ''' <param name="treeNode"></param>
    ''' <param name="oldName"></param>
    ''' <param name="newName"></param>
    ''' <param name="isNodeFounded"></param>
    Private Sub FindNodeNameThenChangeName(ByVal treeNode As ChannelNode, ByVal oldName As String, ByVal newName As String, ByVal isNodeFounded As Boolean)
        Dim numberChannel As String
        If isNodeFounded Then Exit Sub

        For Each itemNode As ChannelNode In treeNode.Nodes
            If itemNode.Tag.ToString = oldName Then
                If CType(itemNode.Parent, ChannelNode).NodeType = NodeType.Module2 Then
                    numberChannel = TextBoxChannel.Text '"НомерКаналаМодуля"
                Else 'ТипУзла.ПлатаАЦП4
                    numberChannel = TextBoxNDaq.Text '"НомерКанала"
                End If

                itemNode.Tag = newName
                itemNode.Text = $"{newName} {{Канал - {numberChannel}}}"
                isNodeFounded = True
                Exit For
            End If

            FindNodeNameThenChangeName(itemNode, oldName, newName, isNodeFounded)
        Next
    End Sub

    ''' <summary>
    ''' Проверка отсутствия совпадений имен
    ''' </summary>
    ''' <param name="nameParameter"></param>
    ''' <returns></returns>
    Private Function CheckIdentityName(ByVal nameParameter As String) As Boolean
        For Each itemRow As ChannelNRow In dt.Rows
            If itemRow.НаименованиеПараметра = nameParameter Then
                Return True
            End If
        Next itemRow

        Return False
    End Function

    ''' <summary>
    ''' Функция обратного вызова для проверки действительности цифровых значений.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ValidatingCheck(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim mTextBox As TextBox = CType(sender, TextBox)

        If Not IsNumeric(mTextBox.Text) Then
            Const caption As String = "Ошибка проверки"
            Const text As String = "Должна быть цифра!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            mTextBox.Undo()
            e.Cancel = True
        End If
    End Sub
#End Region

#Region "UpdateGraph"
    Private Sub SlideValuePrcentMax_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValuePrcentMax.AfterChangeValue
        If SlideValuePrcentMax.Value < 1 Then SlideValuePrcentMax.Value = 1

        UpdateGraphSignal()
    End Sub

    Private Sub SlideValuePrcentMin_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideValuePrcentMin.AfterChangeValue
        If SlideValuePrcentMin.Value > 99 Then SlideValuePrcentMin.Value = 99

        UpdateGraphSignal()
    End Sub

    Private Sub UpdateGraphSignal()
        CheckMinLessMax()
        PlotGraphSignalSinusOnRange()
        ReInitializeAnnotationAlarm()
    End Sub

    Private Sub NumericLevelMin_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NumericLevelMin.AfterChangeValue
        XyPointAnnotationMin.Caption = "Min=" & NumericLevelMin.Value.ToString("F")
        ReInitializeAnnotationAlarm()
    End Sub

    Private Sub NumericLevelMax_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NumericLevelMax.AfterChangeValue
        XyPointAnnotationMax.Caption = "Max=" & NumericLevelMax.Value.ToString("F")
        ReInitializeAnnotationAlarm()
    End Sub

    Private Sub NumericEditAlarmMin_AfterChangeValue(sender As Object, e As AfterChangeNumericValueEventArgs) Handles NumericEditAlarmMin.AfterChangeValue
        ReInitializeAnnotationAlarm()
    End Sub

    Private Sub NumericEditAlarmMax_AfterChangeValue(sender As Object, e As AfterChangeNumericValueEventArgs) Handles NumericEditAlarmMax.AfterChangeValue
        ReInitializeAnnotationAlarm()
    End Sub

    ''' <summary>
    ''' Проверка минимум меньше максимум 
    ''' </summary>
    Private Sub CheckMinLessMax()
        If SlideValuePrcentMax.Value < SlideValuePrcentMin.Value Then
            If SlideValuePrcentMax.Value - 1 > 0 Then
                SlideValuePrcentMin.Value = CInt(SlideValuePrcentMax.Value) - 1
            Else
                SlideValuePrcentMin.Value = 0
            End If
        End If
    End Sub

    ''' <summary>
    ''' Рисовать Синусоиду Размахов
    ''' </summary>
    Private Sub PlotGraphSignalSinusOnRange()
        Dim data() As Double = New Double(50) {}

        For i As Integer = 0 To 50
            data(i) = SlideValuePrcentMin.Value + (SlideValuePrcentMax.Value - SlideValuePrcentMin.Value) * random.NextDouble()
        Next

        Dim maxIndex, minIndex As Integer
        Dim max As Double = Double.MinValue
        Dim min As Double = Double.MaxValue

        For i As Integer = 0 To 50
            If max < data(i) Then
                max = data(i)
                maxIndex = i
            End If

            If min > data(i) Then
                min = data(i)
                minIndex = i
            End If
        Next

        GraphSignal1.PlotY(data)
        XyPointAnnotationMax.SetPosition(maxIndex, max)
        XyPointAnnotationMin.SetPosition(minIndex, min)

        XYCursorDown.MoveCursor(minIndex, min)
        XYCursorUp.MoveCursor(maxIndex, max)
    End Sub

    ''' <summary>
    ''' Обозначить области на графике, при превышении пределов диапазона или аврийных пределов 
    ''' будет изменяться цвет индикаторов.
    ''' </summary>
    Private Sub ReInitializeAnnotationAlarm()
        If Not IsFrmServiceBasesLoaded Then Exit Sub
        Dim ledColor As Color = Color.Yellow

        ' для дискретного сигнала аварийные цвета не обозначать
        If ListBoxFormula.SelectedIndex = 2 Then ledColor = Color.White

        If NumericEditAlarmMin.Value = 0.0 AndAlso NumericEditAlarmMax.Value = 0.0 Then
            ' простая привязка к процентам оси Y
            SetAnnotationAlarmMin(AlarmMinAnnotation, 0.0, If(SlideValuePrcentMin.Value = 0.0, 1.0, SlideValuePrcentMin.Value), ledColor)
            SetAnnotationAlarmMax(AlarmMaxAnnotation, If(SlideValuePrcentMax.Value = 100.0, 99.0, SlideValuePrcentMax.Value), 100.0, ledColor)

            LabelLedMin.Visible = False
            LabelLedMax.Visible = False
        Else
            Dim minimum As Double ' 0.0
            Dim maximum As Double ' 100.0
            Dim levelMin As Double = NumericLevelMin.Value
            Dim levelMax As Double = NumericLevelMax.Value
            Dim alarmMin As Double = NumericEditAlarmMin.Value
            Dim alarmMax As Double = NumericEditAlarmMax.Value
            Dim koffReduction As Double ' коэф. приведение разносов мин-макс физики к мин-макс процентов

            If levelMax <> levelMin Then koffReduction = (SlideValuePrcentMax.Value - SlideValuePrcentMin.Value) / (levelMax - levelMin)

            If levelMin > alarmMin Then
                minimum = SlideValuePrcentMin.Value - (levelMin - alarmMin) * koffReduction
            Else
                minimum = SlideValuePrcentMin.Value + (alarmMin - levelMin) * koffReduction
            End If

            If levelMax > alarmMax Then
                maximum = SlideValuePrcentMax.Value - (levelMax - alarmMax) * koffReduction
            Else
                maximum = SlideValuePrcentMax.Value + (alarmMax - levelMax) * koffReduction
            End If

            SetAnnotationAlarmMin(AlarmMinAnnotation, 0.0, If(minimum <= 0.0, 1.0, minimum), Color.Orange)
            SetAnnotationAlarmMax(AlarmMaxAnnotation, If(maximum >= 100.0, 99.0, maximum), 100.0, Color.Red)

            LabelLedMin.Visible = True
            LabelLedMax.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Заполнить конктретную аннотацию цветом
    ''' </summary>
    ''' <param name="refAlarmAnnotation"></param>
    ''' <param name="minimum"></param>
    ''' <param name="maximum"></param>
    ''' <param name="inFillColor"></param>
    Private Sub SetAnnotationAlarmMin(ByRef refAlarmAnnotation As XYRangeAnnotation, minimum As Double, maximum As Double, inFillColor As Color)
        refAlarmAnnotation.YRange = New Range(minimum, maximum)
        refAlarmAnnotation.RangeFillColor = Color.FromArgb(100, inFillColor)
        refAlarmAnnotation.RangeFillStyle = FillStyle.CreateVerticalGradient(refAlarmAnnotation.RangeFillColor, inFillColor)
    End Sub

    ''' <summary>
    ''' Заполнить конктретную аннотации цветом
    ''' </summary>
    ''' <param name="refAlarmAnnotation"></param>
    ''' <param name="minimum"></param>
    ''' <param name="maximum"></param>
    ''' <param name="inFillColor"></param>
    Private Sub SetAnnotationAlarmMax(ByRef refAlarmAnnotation As XYRangeAnnotation, minimum As Double, maximum As Double, inFillColor As Color)
        refAlarmAnnotation.YRange = New Range(minimum, maximum)
        refAlarmAnnotation.RangeFillColor = Color.FromArgb(100, inFillColor)
        refAlarmAnnotation.RangeFillStyle = FillStyle.CreateVerticalGradient(inFillColor, refAlarmAnnotation.RangeFillColor)
    End Sub
#End Region

#Region "Заполнение дерева"
    ''' <summary>
    ''' Заполнить Корзины
    ''' </summary>
    Private Sub FillChassis()
        Dim devices As New List(Of String)

        Try
            TreeChannels.Nodes.Clear()
            isCleaningTree = True

            If dt.Rows.Count > 0 Then
                isCleaningTree = False

                If RadioButtonViewModule.Checked Then
                    For Each itemDataRow As ChannelNRow In dt.Rows
                        ' здесь ошибка дизайнера
                        'If IsDBNull(itemDataRow.НомерМодуляКорзины) AndAlso IsDBNull(itemDataRow.НомерКаналаМодуля) Then
                        If IsDBNull(CType(itemDataRow, DataRow)("НомерМодуляКорзины")) AndAlso IsDBNull(CType(itemDataRow, DataRow)("НомерКаналаМодуля")) Then
                            ' корзины нет, значит плата
                            Dim sNewDAQboard As String = CStr(itemDataRow.НомерУстройства)

                            If Not devices.Contains(sNewDAQboard) Then
                                devices.Add(sNewDAQboard)
                                AddDAQboardToTree(sNewDAQboard)
                            End If
                        Else
                            Dim sNewChassis As String = CStr(itemDataRow.НомерУстройства)

                            If Not devices.Contains(sNewChassis) Then
                                devices.Add(sNewChassis)
                                AddChassisToTree(sNewChassis)
                            End If
                        End If
                    Next
                Else
                    ' по размерностям
                    For I As Integer = 0 To UBound(UnitOfMeasureArray)
                        AddUnitParameterToTree(UnitOfMeasureArray(I))
                    Next
                End If

                TreeChannels.Nodes(0).Expand()
            End If
        Catch ex As Exception
            Dim caption As String = $"Заполнить проводник по корзинам, модулям и каналам в процедуре <{NameOf(FillChassis)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Добавить в дерево Корзину
    ''' </summary>
    ''' <param name="numberChassis"></param>
    Private Sub AddChassisToTree(ByVal numberChassis As String)
        Dim cRoot As New ChannelNode("Корзина - " & numberChassis, NodeType.Chassis1, CInt(numberChassis)) With {
            .ImageIndex = NodeImage.Chassis13,
            .SelectedImageIndex = NodeImage.ChassisSelected14,
            .Tag = numberChassis
        }
        TreeChannels.Nodes.Add(cRoot)
        AddDirectories(cRoot)
    End Sub

    ''' <summary>
    ''' Добавить в дерево Модуль
    ''' </summary>
    ''' <param name="nodeParent"></param>
    ''' <param name="numberModule"></param>
    Private Sub AddModuleToTree(ByRef nodeParent As ChannelNode, ByVal numberModule As String)
        Dim cRoot As New ChannelNode("Модуль - " & numberModule, NodeType.Module2, CInt(numberModule)) With {
            .ImageIndex = NodeImage.Module15,
            .SelectedImageIndex = NodeImage.ModuleSelected16,
            .Tag = numberModule
        }
        nodeParent.Nodes.Add(cRoot)
        AddDirectories(cRoot) 'после больше потомков не добавляем 
    End Sub

    ''' <summary>
    ''' Добавить в дерево Канал
    ''' </summary>
    ''' <param name="nodeParent"></param>
    ''' <param name="refDataRow"></param>
    Private Sub AddChannelToTree(ByRef nodeParent As ChannelNode, ByRef refDataRow As ChannelNRow)
        Dim numberChannel As String
        Dim nameChannel As String = refDataRow.НаименованиеПараметра
        Dim unit As String = refDataRow.ЕдиницаИзмерения

        If nodeParent.NodeType = NodeType.Module2 Then
            numberChannel = refDataRow.НомерКаналаМодуля
        Else 'ТипУзла.ПлатаАЦП4
            numberChannel = CStr(refDataRow.НомерКанала)
        End If

        Dim cRoot As New ChannelNode($"{nameChannel} {{Канал - {numberChannel}}}", NodeType.Channel3, CInt(numberChannel))
        If CBool(InStr(1, UnitOfMeasureString, unit)) Then
            cRoot.ImageIndex = Array.IndexOf(UnitOfMeasureArray, unit)
        Else
            cRoot.ImageIndex = NodeImage.Discrete1
        End If
        cRoot.SelectedImageIndex = NodeImage.Selected19
        cRoot.Tag = nameChannel ' НомерКанала
        nodeParent.Nodes.Add(cRoot)
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Плату
    ''' </summary>
    ''' <param name="numberDAQboard"></param>
    Private Sub AddDAQboardToTree(ByVal numberDAQboard As String)
        Dim cRoot As New ChannelNode("Плата АЦП - " & numberDAQboard, NodeType.DAQBoard4, CInt(numberDAQboard)) With {
            .ImageIndex = NodeImage.DAQBoard17,
            .SelectedImageIndex = NodeImage.DAQBoardSelected18,
            .Tag = numberDAQboard
        }
        TreeChannels.Nodes.Add(cRoot)
        AddDirectories(cRoot)
    End Sub

    ''' <summary>
    ''' Добавить В Дерево Тип Параметра
    ''' </summary>
    ''' <param name="unit"></param>
    Private Sub AddUnitParameterToTree(ByVal unit As String)
        Dim typeParameter As String ' тип параметра

        Select Case unit
            Case "дел"
                typeParameter = "ДИСКРЕТНЫЕ"
                Exit Select
            Case "мм"
                typeParameter = "РАЗРЕЖЕНИЯ"
                Exit Select
            Case "градус"
                typeParameter = "ТЕМПЕРАТУРЫ"
                Exit Select
            Case "Кгсм"
                typeParameter = "ДАВЛЕНИЯ"
                Exit Select
            Case "мм/с"
                typeParameter = "ВИБРАЦИЯ"
                Exit Select
            Case "мкА"
                typeParameter = "ТОКИ"
                Exit Select
            Case "кг/ч"
                typeParameter = "РАСХОДЫ"
                Exit Select
            Case "кгс"
                typeParameter = "ТЯГА"
                Exit Select
            Case Else
                typeParameter = "ОБОРОТЫ"
                'Case "%"
                '    sТипПараметра = "ОБОРОТЫ"
                Exit Select
        End Select

        Dim cRoot As New ChannelNode(typeParameter, NodeType.TypeParameter5, Array.IndexOf(UnitOfMeasureArray, unit)) With {
            .ImageIndex = Array.IndexOf(UnitOfMeasureArray, unit),
            .SelectedImageIndex = NodeImage.Selected19,
            .Tag = unit
        }
        TreeChannels.Nodes.Add(cRoot)
        AddDirectories(cRoot)
    End Sub

    ''' <summary>
    ''' Рекурсивное добавление дочерних узлов.
    ''' Чтобы узнать, есть ли предки и изменить плюс в узле для раскрытия.
    ''' </summary>
    ''' <param name="nodeParent"></param>
    Private Sub AddDirectories(ByVal nodeParent As ChannelNode)
        Dim aRows As ChannelNRow()
        Dim listDevice As New List(Of String)

        Select Case nodeParent.NodeType
            Case NodeType.Chassis1 ' родитель Корзина1
                aRows = CType(dt.Select("НомерУстройства = " & nodeParent.Tag.ToString, conSortASC), ChannelNRow())
                Dim numberModule As String

                If aRows.Length > 0 Then
                    For Each itemDataRow As ChannelNRow In aRows
                        If Not IsDBNull(itemDataRow.НомерМодуляКорзины) Then
                            numberModule = itemDataRow.НомерМодуляКорзины
                        Else
                            numberModule = vbNullString
                        End If

                        If Not listDevice.Contains(numberModule) Then
                            listDevice.Add(numberModule)
                            AddModuleToTree(nodeParent, numberModule)
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If

                Exit Select
            Case NodeType.Module2 ' родитель Модуль2
                If RadioButtonViewModule.Checked Then
                    aRows = CType(dt.Select($"НомерУстройства = {nodeParent.Parent.Tag} And НомерМодуляКорзины = '{nodeParent.Tag}'", conSortASC), ChannelNRow())
                Else
                    aRows = CType(dt.Select($"НомерУстройства = {nodeParent.Parent.Tag} AND НомерМодуляКорзины = '{nodeParent.Tag}' AND ЕдиницаИзмерения = '{nodeParent.Parent.Parent.Tag}'", conSortASC), ChannelNRow())
                End If

                Dim numberChannel As String
                If aRows.Length > 0 Then
                    For Each itemDataRow As ChannelNRow In aRows
                        If Not IsDBNull(itemDataRow.НомерКаналаМодуля) Then
                            numberChannel = itemDataRow.НомерКаналаМодуля
                        Else
                            numberChannel = vbNullString
                        End If

                        If Not listDevice.Contains(numberChannel) Then
                            listDevice.Add(numberChannel)
                            AddChannelToTree(nodeParent, itemDataRow)
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If
                Exit Select
            Case NodeType.DAQBoard4 ' родитель ПлатаАЦП4
                If RadioButtonViewModule.Checked Then
                    aRows = CType(dt.Select($"НомерУстройства = {nodeParent.Tag}", conSortASC), ChannelNRow())
                Else
                    aRows = CType(dt.Select($"НомерУстройства = {nodeParent.Tag} AND ЕдиницаИзмерения = '{nodeParent.Parent.Tag}'", conSortASC), ChannelNRow())
                End If

                Dim numberChannel As String
                If aRows.Length > 0 Then
                    For Each itemDataRow As ChannelNRow In aRows
                        numberChannel = CStr(itemDataRow.НомерКанала)

                        If Not listDevice.Contains(numberChannel) Then
                            listDevice.Add(numberChannel)
                            AddChannelToTree(nodeParent, itemDataRow)
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If
                Exit Select
            Case NodeType.TypeParameter5 ' родитель ТипПараметра5 почти как родитель Корзина1
                aRows = CType(dt.Select($"ЕдиницаИзмерения = '{nodeParent.Tag}'", conSortASC), ChannelNRow())

                If aRows.Length > 0 Then
                    For Each itemDataRow In aRows
                        If IsDBNull(itemDataRow.НомерМодуляКорзины) AndAlso IsDBNull(itemDataRow.НомерКаналаМодуля) Then
                            ' корзины нет, значит плата
                            Dim numberDAQBoard As String = CStr(itemDataRow.НомерУстройства)

                            If Not listDevice.Contains(numberDAQBoard) Then
                                listDevice.Add(numberDAQBoard)
                                Dim cRoot As New ChannelNode("Плата АЦП - " & numberDAQBoard, NodeType.DAQBoard4, CInt(numberDAQBoard)) With {
                                    .ImageIndex = NodeImage.DAQBoard17,
                                    .SelectedImageIndex = NodeImage.DAQBoardSelected18,
                                    .Tag = numberDAQBoard
                                }
                                nodeParent.Nodes.Add(cRoot)
                                AddDirectories(cRoot)
                            End If
                        Else
                            Dim numberChassis As String = itemDataRow("НомерУстройства").ToString

                            If Not listDevice.Contains(numberChassis) Then
                                listDevice.Add(numberChassis)
                                Dim cRoot As New ChannelNode("Корзина - " & numberChassis, NodeType.Chassis1, CInt(numberChassis)) With {
                                    .ImageIndex = NodeImage.Chassis13,
                                    .SelectedImageIndex = NodeImage.ChassisSelected14,
                                    .Tag = numberChassis
                                }
                                nodeParent.Nodes.Add(cRoot)
                                AddDirectories(cRoot)
                            End If
                        End If
                    Next
                Else
                    nodeParent.Remove()
                End If
                Exit Select
        End Select
    End Sub

    Private Sub TreeChannels_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeChannels.AfterSelect
        PopulateFormAfterTreeSelect(e.Node)
    End Sub

    Private Sub ChannelsTree_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeChannels.BeforeExpand
        PopulateFormBeforeTreeExpand(e.Node)
    End Sub

    ''' <summary>
    ''' Обновить контролы формы после выбора в дереве узла
    ''' </summary>
    Private Sub PopulateFormAfterTreeSelect(node As TreeNode)
        Try
            TreeChannels.SelectedNode = node

            If Not isCalledFromFinding Then isNeedSelectedIndexChanged = False

            isNeedAfterSelect = False
            If isNeedBeforeExpand Then node.Expand()
            Delay(0.1)
            isNeedAfterSelect = True

            Dim nodeSelect As ChannelNode = CType(node, ChannelNode)

            If nodeSelect.NodeType = NodeType.Channel3 Then
                Dim counter As Integer
                'Dim vals(0) As Object
                ''dv.Sort = "НомерПараметра ASC"
                'vals(0) = nodeSelect.Tag 'ИмяКанала
                ''vals(1) = "что-то"
                'intTabPosition = dv.Find(vals)

                For Each rowChannel As DataRowView In dv
                    If nodeSelect.Tag.ToString = CStr(rowChannel("НаименованиеПараметра")) Then
                        tabRowSelectPosition = counter ' rowChannel.
                        Exit For
                    End If
                    counter += 1
                Next

                If ChannelsNDataSet1.ChannelN.Rows.Count > 0 Then
                    ' UnSelect
                    If DataGridChannels.CurrentRow.Index <> -1 Then
                        DataGridChannels.CurrentRow.Selected = False
                    End If

                    rowCurrentChannel = CType(dv(tabRowSelectPosition).Row, ChannelNRow)
                    ToolStripContainer1.BindingContext(dv).Position = tabRowSelectPosition
                    ShowCurrentRowOnTable()
                End If
            End If

            EnabledDisadleMenu(nodeSelect)
        Catch ex As Exception
            Dim caption As String = $"Смена отображения {NameOf(PopulateFormAfterTreeSelect)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            isNeedSelectedIndexChanged = True
        End Try
    End Sub

    ''' <summary>
    ''' Обновить контролы формы после развёртывания в дереве узла
    ''' </summary>
    ''' <param name="node"></param>
    Private Sub PopulateFormBeforeTreeExpand(node As TreeNode)
        Delay(0.1)

        If isCleaningTree Then Exit Sub
        CheckToWriteUpdateString()

        Dim nodeExpanding As ChannelNode = CType(node, ChannelNode)
        Try
            If node.Parent IsNot Nothing Then
                For Each itemNode As TreeNode In node.Parent.Nodes
                    If itemNode.IsExpanded Then
                        itemNode.Collapse()
                    End If
                Next
            Else ' по самому первому уровню
                For Each itemNode As TreeNode In TreeChannels.Nodes
                    If itemNode.IsExpanded Then
                        itemNode.Collapse()
                    End If
                Next
            End If
            ' закоментировал
            'If Not nodeExpanding.SubDirectoriesAdded Then
            '    AddSubDirectories(nodeExpanding)
            'End If

            node.EnsureVisible()
            node.BackColor = Color.Gold

            If nodeExpanding.NodeType = NodeType.Module2 Then
                isChannelsTreeBeforeExpand = True

                If RadioButtonViewModule.Checked Then
                    dv.RowFilter = $"НомерУстройства = {nodeExpanding.Parent.Tag} AND НомерМодуляКорзины = '{nodeExpanding.Tag}'"
                    dv.Sort = conSortASC
                Else
                    dv.RowFilter = $"НомерУстройства = {nodeExpanding.Parent.Tag} AND НомерМодуляКорзины = '{nodeExpanding.Tag}' AND ЕдиницаИзмерения = '{nodeExpanding.Parent.Parent.Tag}'"
                    dv.Sort = conSortASC
                End If

                isChannelsTreeBeforeExpand = False
                tabRowSelectPosition = -1
                FirstRecord()
            ElseIf nodeExpanding.NodeType = NodeType.DAQBoard4 Then
                isChannelsTreeBeforeExpand = True

                If RadioButtonViewModule.Checked Then
                    dv.RowFilter = $"НомерУстройства = {nodeExpanding.Tag}"
                    dv.Sort = conSortASC
                Else
                    If nodeExpanding.Parent Is Nothing Then
                        dv.RowFilter = $"НомерУстройства = {nodeExpanding.Tag}"
                    Else
                        dv.RowFilter = $"НомерУстройства = {nodeExpanding.Tag} AND ЕдиницаИзмерения = '{nodeExpanding.Parent.Tag}'"
                    End If

                    dv.Sort = conSortASC
                End If

                isChannelsTreeBeforeExpand = False
                tabRowSelectPosition = -1
                FirstRecord()
            Else
                isChannelsTreeBeforeExpand = True
                dv.RowFilter = "НомерУстройства = 128" ' какую нибудь чушь
                isChannelsTreeBeforeExpand = False
            End If

            ApplyFilterBindingSource()

            isNeedBeforeExpand = False
            If isNeedAfterSelect Then PopulateFormAfterTreeSelect(node)
            isNeedBeforeExpand = True

        Catch ex As Exception
            Dim caption As String = $"Смена отображения {NameOf(PopulateFormBeforeTreeExpand)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub ApplyFilterBindingSource()
        BindingSourceChannelsN.Filter = dv.RowFilter
        BindingSourceChannelsN.Sort = dv.Sort
    End Sub

    ' дополнять еще не раскрытый узел динамически
    'Private Sub AddSubDirectories(ByVal node As ChannelNode)
    '    Dim i As Integer
    '    For i = 0 To node.Nodes.Count - 1
    '        AddDirectories(node.Nodes(i))
    '    Next i
    '    node.SubDirectoriesAdded = True
    'End Sub

    Private Sub ChannelsTree_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeChannels.AfterCollapse
        Delay(0.1)
        e.Node.BackColor = Color.White
        EnabledDisadleMenu(CType(e.Node, ChannelNode))
    End Sub

    Private Sub DisadleMenu()
        MenuAddNewChassis.Enabled = False
        MenuRemoveSelectedChassis.Enabled = False
        MenuAddNewModule.Enabled = False
        MenuRemoveSelectedModule.Enabled = False
        MenuAddNewCard.Enabled = False
        MenuRemoveSelectedCard.Enabled = False

        ButtonAddChassis.Enabled = False
        ButtonRemoveChassis.Enabled = False
        ButtonAddModule.Enabled = False
        ButtonRemoveModule.Enabled = False
        ButtonAddCard.Enabled = False
        ButtonRemoveCard.Enabled = False
        ButtonRemoveAll.Enabled = False
    End Sub

    Private Sub EnabledMenuAddChassisOrCard()
        MenuAddNewChassis.Enabled = True
        ButtonAddChassis.Enabled = True
        MenuAddNewCard.Enabled = True
        ButtonAddCard.Enabled = True
    End Sub

    ''' <summary>
    ''' Вкл Выкл Меню
    ''' </summary>
    ''' <param name="nodeSelect"></param>
    Private Sub EnabledDisadleMenu(nodeSelect As ChannelNode)
        DisadleMenu()

        If Not nodeSelect.IsExpanded Then Exit Sub

        If RadioButtonViewModule.Checked Then
            Select Case nodeSelect.NodeType
                Case NodeType.Chassis1
                    MenuAddNewChassis.Enabled = True
                    MenuAddNewModule.Enabled = True
                    ButtonAddChassis.Enabled = True
                    ButtonAddModule.Enabled = True
                    If isCanEditName Then
                        MenuRemoveSelectedChassis.Enabled = True
                        ButtonRemoveChassis.Enabled = True
                        ButtonRemoveAll.Enabled = True
                    End If
                    Exit Select
                Case NodeType.Module2
                    MenuAddNewModule.Enabled = True
                    ButtonAddModule.Enabled = True
                    If isCanEditName Then
                        MenuRemoveSelectedModule.Enabled = True
                        ButtonRemoveModule.Enabled = True
                    End If
                    Exit Select
                Case NodeType.DAQBoard4
                    MenuAddNewCard.Enabled = True
                    ButtonAddCard.Enabled = True
                    If isCanEditName Then
                        MenuRemoveSelectedCard.Enabled = True
                        ButtonRemoveCard.Enabled = True
                        ButtonRemoveAll.Enabled = True
                    End If
                    Exit Select
            End Select
        End If
    End Sub

    'Public Sub Delay(secondsDelay As Double)
    '    Dim t = Tasks.Task.Run(Async Function()
    '                               Await Tasks.Task.Delay(TimeSpan.FromSeconds(secondsDelay)).ConfigureAwait(False)
    '                           End Function)
    '    t.Wait()
    'End Sub

    ''' <summary>
    ''' Выделить Канал Из Поиска
    ''' </summary>
    Private Sub SelectChannelFromFinding()
        'это для корзины
        'НомерКанала=0
        'НомерУстройства=1,2,3...'НомерКорзины
        'НомерМодуляКорзины=1,2,3...'НомерМодуляКорзины
        'НомерКаналаМодуля-1,2,3...НомерКаналаМодуля

        'numberChannelModule = "0" 'неважно (всегда=0)'НомерКанала
        'chassis2 = "2" 'НомерУстройства
        'module3 = "1" 'НомерМодуляКорзины
        'numberChannelModule = "0" 'неважно 0-31'НомерКаналаМодуля
        'nameChannel4 = "R изм-2"
        'unit1 = "кгс"

        'это для платы
        'НомерКанала=0-31 номерКанала
        'НомерУстройства=1,2,3...'НомерПлаты
        'НомерМодуляКорзины=Null
        'НомерКаналаМодуля=Null

        'numberChannelModule = "0" 'неважно 'НомерКанала            'numberChannelModule = mfrmFindChannel.НомерКанала
        'chassis2 = "2" 'НомерУстройства                            'chassis2 = mfrmFindChannel.Корзина
        'module3 = "" 'НомерМодуляКорзины                           'module3 = mfrmFindChannel.Модуль
        'numberChannelModule = "" 'НомерКаналаМодуля                'numberChannelModule = mfrmFindChannel.НомерКаналаМодуля
        'nameChannel4 = "R изм-2"                                   'nameChannel4 = mfrmFindChannel.ИмяКанала
        'unit1 = "кгс"                                              'unit1 = mfrmFindChannel.ЕдИзмерения

        TreeChannels.CollapseAll()

        If module3 = "" AndAlso numberChannelModule = "" Then
            ' корзины нет, значит плата
            For Each itemNode As TreeNode In TreeChannels.Nodes
                If RadioButtonViewModule.Checked Then
                    If CStr(itemNode.Tag) = chassis2 Then
                        TreeChannels.SelectedNode = itemNode
                        itemNode.Expand()
                        ExpandNodeDAQboard(itemNode, LevelNode.Channel4, chassis2, nameChannel4)
                        Exit Sub
                    End If
                Else
                    If CStr(itemNode.Tag) = unit1 Then
                        TreeChannels.SelectedNode = itemNode
                        itemNode.Expand()
                        ExpandNodeDAQboard(itemNode, LevelNode.Chassis2, chassis2, nameChannel4)
                        Exit Sub
                    End If
                End If
            Next
        Else
            For Each itemNode As TreeNode In TreeChannels.Nodes
                If RadioButtonViewModule.Checked Then
                    If itemNode.Tag.ToString = chassis2 Then
                        TreeChannels.SelectedNode = itemNode
                        itemNode.Expand()
                        ExpandNode(itemNode, LevelNode.Module3, chassis2, module3, nameChannel4)
                        Exit Sub
                    End If
                Else
                    If itemNode.Tag.ToString = unit1 Then
                        TreeChannels.SelectedNode = itemNode
                        itemNode.Expand()
                        ExpandNode(itemNode, LevelNode.Chassis2, chassis2, module3, nameChannel4)
                        Exit Sub
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Рекурсивно Раскрыть Узел в дереве для Платы
    ''' </summary>
    ''' <param name="parentNode"></param>
    ''' <param name="level"></param>
    ''' <param name="chassis2"></param>
    ''' <param name="nameChannel4"></param>
    Private Sub ExpandNodeDAQboard(ByVal parentNode As TreeNode, ByVal level As LevelNode, ByVal chassis2 As String, ByVal nameChannel4 As String)
        parentNode.Expand()

        For Each itemNode As TreeNode In parentNode.Nodes
            Select Case level
                Case LevelNode.Chassis2
                    If itemNode.Tag.ToString = chassis2 Then
                        ExpandNodeDAQboard(itemNode, LevelNode.Channel4, chassis2, nameChannel4)
                        Exit Sub
                    End If
                    Exit Select
                Case LevelNode.Channel4
                    If itemNode.Tag.ToString = nameChannel4 Then
                        ' самый нижний уровень надо внести корректировку в поиске канала
                        isCalledFromFinding = True
                        PopulateFormAfterTreeSelect(itemNode)
                        isCalledFromFinding = False
                        Exit Sub
                    End If
                    Exit Select
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Рекурсивно Раскрыть Узел в дереве
    ''' </summary>
    ''' <param name="parentNode"></param>
    ''' <param name="level"></param>
    ''' <param name="chassis2"></param>
    ''' <param name="module3"></param>
    ''' <param name="nameChannel4"></param>
    Private Sub ExpandNode(ByVal parentNode As TreeNode, ByVal level As LevelNode, ByVal chassis2 As String, ByVal module3 As String, ByVal nameChannel4 As String)
        Try
            parentNode.Expand()
            For Each itemNode As TreeNode In parentNode.Nodes
                Select Case level
                    Case LevelNode.Unit1
                        If itemNode.Tag.ToString = unit1 Then
                            ExpandNode(itemNode, LevelNode.Chassis2, chassis2, module3, nameChannel4)
                            Exit Sub
                        End If
                        Exit Select
                    Case LevelNode.Chassis2
                        If itemNode.Tag.ToString = chassis2 Then
                            ExpandNode(itemNode, LevelNode.Module3, chassis2, module3, nameChannel4)
                            Exit Sub
                        End If
                        Exit Select
                    Case LevelNode.Module3
                        If itemNode.Tag.ToString = module3 Then
                            ExpandNode(itemNode, LevelNode.Channel4, chassis2, module3, nameChannel4)
                            Exit Sub
                        End If
                        Exit Select
                    Case LevelNode.Channel4
                        If itemNode.Tag.ToString = nameChannel4 Then
                            ' самый нижний уровень надо внести корректировку в поиске канала
                            isCalledFromFinding = True
                            PopulateFormAfterTreeSelect(itemNode)
                            isCalledFromFinding = False
                            Exit Sub
                        End If
                        Exit Select
                End Select
            Next
        Catch ex As Exception
            Const caption As String = "Раскрыть узел"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub
#End Region

#Region "Добавить/удалить шасси, модуль, плату"
    Private Function IsConfirmRemove(device As String) As Boolean
        Dim text As String = "Вы точно уверены, что хотите удалить" & vbCrLf &
                            device & vbCrLf &
                            $"из существующей конфигурации каналов стенда № {StandNumber}?"
        Return MessageBox.Show(text, "Подтвердите удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes
    End Function

    Private Sub MenuRemoveSelectedChassis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuRemoveSelectedChassis.Click, ButtonRemoveChassis.Click
        If TreeChannels.SelectedNode Is Nothing Then Exit Sub
        If IsConfirmRemove($"Корзина №: {CType(TreeChannels.SelectedNode, ChannelNode).Tag}") Then RemoveChassis()
    End Sub

    Private Sub MenuRemoveSelectedModule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuRemoveSelectedModule.Click, ButtonRemoveModule.Click
        Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)
        Dim nodeSelectParent As ChannelNode = CType(nodeSelect.Parent, ChannelNode)
        Dim device As String = $"Корзина №: {nodeSelectParent.Tag} Модуль №: {nodeSelect.Tag}"

        If IsConfirmRemove(device) Then RemoveModule()
    End Sub

    Private Sub MenuRemoveSelectedCard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuRemoveSelectedCard.Click, ButtonRemoveCard.Click
        If TreeChannels.SelectedNode Is Nothing Then Exit Sub
        If IsConfirmRemove($"Плата №: {CType(TreeChannels.SelectedNode, ChannelNode).Tag}") Then RemoveDAQBoard()
    End Sub

    Private Sub ButtonRemoveAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRemoveAll.Click
        If IsConfirmRemove("все устойства") Then RemoveAll()
    End Sub

    Private Sub MenuAddNewChassis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuAddNewChassis.Click, ButtonAddChassis.Click
        AddChassis()
    End Sub

    Private Sub MenuAddNewModule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuAddNewModule.Click, ButtonAddModule.Click
        AddModules()
    End Sub

    Private Sub MenuAddNewCard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuAddNewCard.Click, ButtonAddCard.Click
        AddDAQBoards()
    End Sub

    ''' <summary>
    ''' Обновить Форму
    ''' </summary>
    Private Sub UpdateForm()
        SaveDBase()
        FillChassis()
        IndicationUpdateDBase(False)
    End Sub

    ''' <summary>
    ''' Удалить Корзину
    ''' </summary>
    Private Sub RemoveChassis()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить Выделенную Корзину")
        Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)
        Dim numberChassis As String ' удаляемый номер корзины

        sortTransaction = ""
        If nodeSelect.NodeType = NodeType.Chassis1 Then
            numberChassis = nodeSelect.Tag.ToString
            For Each itemDataRow As ChannelNRow In dt.Rows
                If itemDataRow.НомерУстройства = CInt(numberChassis) Then
                    itemDataRow.Delete()
                End If
            Next
            CheckSequenceNumber(NodeType.Chassis1)
        End If
    End Sub

    ''' <summary>
    ''' Удалить Модуль
    ''' </summary>
    Private Sub RemoveModule()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить Выделенный Модуль")
        Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)
        Dim nodeSelectParent As ChannelNode = CType(nodeSelect.Parent, ChannelNode)

        sortTransaction = ""
        If nodeSelect.NodeType = NodeType.Module2 Then
            If nodeSelectParent.NodeType = NodeType.Chassis1 Then
                For Each itemDataRow As ChannelNRow In dt.Rows
                    If itemDataRow.НомерУстройства = CInt(nodeSelectParent.Tag) AndAlso itemDataRow.НомерМодуляКорзины = nodeSelect.Tag.ToString Then
                        itemDataRow.Delete()
                    End If
                Next
                CheckSequenceNumber(NodeType.Module2)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Удалить Плату
    ''' </summary>
    Private Sub RemoveDAQBoard()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить Выделенную Плату")
        Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)

        sortTransaction = ""
        If nodeSelect.NodeType = NodeType.DAQBoard4 Then
            For Each drDataRow As ChannelNRow In dt.Rows
                If drDataRow.НомерУстройства = CInt(nodeSelect.Tag) Then
                    drDataRow.Delete()
                End If
            Next
            CheckSequenceNumber(NodeType.DAQBoard4)
        End If
    End Sub

    ''' <summary>
    ''' Удалить Все
    ''' </summary>
    Private Sub RemoveAll()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Удалить Всё")

        'dt.Clear() не помечает записи на удаление
        For Each itemDataRow As ChannelNRow In dt.Rows
            itemDataRow.Delete()
        Next

        UpdateForm()
        DisadleMenu()
        EnabledMenuAddChassisOrCard()
    End Sub

    ''' <summary>
    ''' Добавить Корзину
    ''' </summary>
    Private Sub AddChassis()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавить Новую Корзину")
        Dim msg As String = $"Введите номер вновь добавляемой корзины.{ControlChars.CrLf}Нажмите Cancel для выхода."
        Dim numberChassis As String
        Const caption As String = "Проверка номера корзины"
        sortTransaction = conSortDESC

        Do
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Question)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{NameOf(AddChassis)}> {msg}")

            numberChassis = InputBox(msg, "Добавление корзины")

            ' проверка на существование
            If numberChassis <> "" Then
                If Not IsNumeric(numberChassis) Then
                    Const textNotNumber As String = "Должна быть цифра!"
                    MessageBox.Show(textNotNumber, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {textNotNumber}")
                    Exit Sub
                End If

                If CInt(numberChassis) < 1 OrElse CInt(numberChassis) > maxDevice Then
                    Dim textNumberGreaterNull As String = "Номер должен быть больше 0 и меньше " & maxDevice.ToString
                    MessageBox.Show(textNumberGreaterNull, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {textNumberGreaterNull}")
                    Exit Sub
                End If

                If dt.Select("НомерУстройства = " & numberChassis).Length = 0 Then
                    'Dim aFindValue(0) As Object
                    'aFindValue(0) = CInt(NumberChassis)
                    'drDataRow = dt.Rows.Find(aFindValue)
                    'If drDataRow Is Nothing Then
                    ' все нормально
                    AddNewModule(numberChassis, 1.ToString)
                Else
                    Dim text As String = $"Корзина с номером: {numberChassis} уже существует!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{NameOf(AddChassis)}> {text}")
                End If
            End If
        Loop Until numberChassis = ""
    End Sub

    ''' <summary>
    ''' Добавить Модули
    ''' </summary>
    Private Sub AddModules()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавить Новый Модуль")
        Const msg As String = "Введите номер вновь добавляемого модуля." & ControlChars.CrLf &
                            "Нажмите Cancel для выхода."
        Dim numberModule As String
        ' проверка на существование модуля
        Dim nodeSelect As ChannelNode = CType(TreeChannels.SelectedNode, ChannelNode)
        Dim numberShassisForModule As String ' Номер Корзины Для Добавления Модуля
        Const caption As String = "Добавление модуля"

        If nodeSelect.NodeType = NodeType.Chassis1 Then
            numberShassisForModule = nodeSelect.Tag.ToString
        ElseIf nodeSelect.NodeType = NodeType.Module2 Then
            numberShassisForModule = nodeSelect.Parent.Tag.ToString
        Else
            Exit Sub
        End If

        sortTransaction = conSortDESC

        Do
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {msg}")
            numberModule = InputBox(msg, "Добавление модуля")
            'проверка на существование
            If numberModule <> "" Then
                If Not IsNumeric(numberModule) Then
                    Const _caption As String = "Проверка номера модуля"
                    Const _text As String = "Должна быть цифра!"
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                    Exit Sub
                End If

                If CInt(numberModule) < 1 OrElse CInt(numberModule) > maxDevice Then
                    Const _caption As String = "Проверка номера модуля"
                    Dim _text As String = "Номер должен быть больше 0 и меньше " & maxDevice.ToString
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                    Exit Sub
                End If

                If dt.Select($"НомерУстройства = {numberShassisForModule} And НомерМодуляКорзины = {numberModule}").Length = 0 Then
                    'все нормально
                    AddNewModule(numberShassisForModule, numberModule)
                Else
                    Const _caption As String = "Добавление модуля"
                    Dim _text As String = $"Модуль с номером: {numberModule} уже существует!"
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                End If
            End If
        Loop Until numberModule = ""
    End Sub

    ''' <summary>
    ''' Добавить Новый Модуль
    ''' </summary>
    ''' <param name="numberChassis"></param>
    ''' <param name="numberModule"></param>
    Private Sub AddNewModule(ByVal numberChassis As String, ByVal numberModule As String)
        Dim mFormTuningChannel As FormTuningChannel = New FormTuningChannel(ServiceBasesForm) With {.IsAddingBoard = False}
        mFormTuningChannel.ShowDialog() ' обязательно модально
        LoopAddChannels(mFormTuningChannel, False, numberChassis, numberModule, "")
    End Sub

    ''' <summary>
    ''' Добавить Новую Плату
    ''' </summary>
    ''' <param name="numberBoard"></param>
    Private Sub AddNewDAQBoard(ByVal numberBoard As String)
        Dim mFormTuningChannel As FormTuningChannel = New FormTuningChannel(ServiceBasesForm) With {.IsAddingBoard = True}
        mFormTuningChannel.ShowDialog() ' обязательно модально
        LoopAddChannels(mFormTuningChannel, True, "", "", numberBoard)
    End Sub

    ''' <summary>
    ''' Цикл Добавления Каналов
    ''' </summary>
    ''' <param name="frmTuningChannel"></param>
    ''' <param name="isAddingDAQBoard"></param>
    ''' <param name="numberChassis"></param>
    ''' <param name="numberModule"></param>
    ''' <param name="numberBoard"></param>
    Private Sub LoopAddChannels(ByRef frmTuningChannel As FormTuningChannel,
                                ByVal isAddingDAQBoard As Boolean,
                                ByVal numberChassis As String,
                                ByVal numberModule As String,
                                ByVal numberBoard As String)

        If Val(frmTuningChannel.ComboBoxLowLimit.Text) >= Val(frmTuningChannel.ComboBoxUpperLimit.Text) Then
            Const caption As String = "Ошибка при проверке"
            Const text As String = "Значение <Нижний предел> больше <Верхний предел>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        If frmTuningChannel.NumericLevelMin.Value >= frmTuningChannel.NumericLevelMax.Value Then
            Const caption As String = "Ошибка при проверке"
            Const text As String = "Значение <Минимальное значение сигнала> больше <Максимальное значение сигнала>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        Dim countAddChannels As Integer ' кол Добавляемых Каналов
        Dim newName As String
        Dim newNameBase As String = GetBaseNameChannelFromUnit(frmTuningChannel.ListBoxUnit.SelectedItem.ToString)

        If frmTuningChannel.IsAddingBoard Then
            countAddChannels = CInt(frmTuningChannel.ComboBoxCountOfChannels.Text)
            newNameBase &= $"-{numberBoard}-"
        Else
            countAddChannels = 32
            newNameBase &= $"-{numberChassis}-{numberModule}-"
        End If

        ' В цикле по всем каналам проверить на отсутствие совпадений
        For I = 0 To countAddChannels - 1
            newName = newNameBase & I.ToString
            If CheckIdentityName(newName) Then
                Const caption As String = "Обновить строку базы"
                Dim text As String = $"Канала с именем: {newName} уже существует!{vbCrLf}Этот параметр необходимо переименовать."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If
        Next

        Dim newNumberBase As Integer
        If dt.Rows.Count > 0 Then
            newNumberBase = CInt(dt.Rows(dt.Rows.Count - 1).Item("НомерПараметра"))
        Else
            newNumberBase = 0
        End If

        Dim counterChannels As Integer
        ' В цикле по всем каналам добавляем записи
        For I As Integer = 0 To countAddChannels - 1
            newName = newNameBase & I.ToString ' инкремент имени
            newNumberBase += 1 ' инкремент Номер Параметра
            Dim newDataRow As ChannelNRow = ChannelsNDataSet1.ChannelN.NewChannelNRow 'dt.NewRow

            newDataRow.BeginEdit()
            newDataRow.НомерПараметра = newNumberBase
            newDataRow.НаименованиеПараметра = newName

            If frmTuningChannel.IsAddingBoard Then
                If frmTuningChannel.RadioButtonDIFF.Checked Then
                    ' добавлять пачками через 8
                    newDataRow.НомерКанала = counterChannels

                    If (I + 1) Mod 8 = 0 Then
                        counterChannels += 8
                    End If
                    counterChannels += 1
                Else
                    ' последовательное увеличение номеров
                    newDataRow.НомерКанала = I
                End If
                newDataRow.НомерУстройства = CInt(numberBoard) ' целое
            Else
                ' последовательное увеличение номеров
                ' все каналы для модулей ссылаются на 0 канал платы АЦП
                newDataRow.НомерКанала = 0  ' Длинное целое
                newDataRow.НомерУстройства = CInt(numberChassis) ' целое
                newDataRow.НомерМодуляКорзины = numberModule
                newDataRow.НомерКаналаМодуля = I.ToString
            End If

            ' ТипПодключения '="DIFF" Or ="RSE" Or ="NRSE" Or ="4WRITE"
            If frmTuningChannel.RadioButtonRSE.Checked = True Then
                newDataRow.ТипПодключения = "RSE"
            ElseIf frmTuningChannel.RadioButtonNRSE.Checked = True Then
                newDataRow.ТипПодключения = "NRSE"
            ElseIf frmTuningChannel.RadioButton4WRITE.Checked = True Then
                newDataRow.ТипПодключения = "4WRITE"
            ElseIf frmTuningChannel.RadioButtonDIFF.Checked = True Then
                newDataRow.ТипПодключения = "DIFF"
            End If

            ' ТипСигнала '="DC" Or ="AC"
            If frmTuningChannel.RadioButtonDC.Checked = True Then
                newDataRow.ТипСигнала = "DC"
            ElseIf frmTuningChannel.RadioButtonGround.Checked = True Then
                newDataRow.ТипСигнала = "Gnd"
            ElseIf frmTuningChannel.RadioButtonAC.Checked = True Then
                newDataRow.ТипСигнала = "AC"
            End If

            newDataRow.НижнийПредел = CSng(frmTuningChannel.ComboBoxLowLimit.Text)
            newDataRow.ВерхнийПредел = CSng(frmTuningChannel.ComboBoxUpperLimit.Text)
            newDataRow.ЕдиницаИзмерения = frmTuningChannel.ListBoxUnit.Text
            ' НомерФормулы '=1 Or =2 Or =3
            newDataRow.НомерФормулы = frmTuningChannel.ListBoxFormula.SelectedIndex + 1
            ' СтепеньАппроксимации '=0 Or =1 Or =2 Or =3 Or =4 Or =5
            newDataRow.СтепеньАппроксимации = 1
            newDataRow.A0 = 0
            newDataRow.A1 = 1
            newDataRow.A2 = 0
            newDataRow.A3 = 0
            newDataRow.A4 = 0
            newDataRow.A5 = 0
            newDataRow.Смещение = 0
            newDataRow.КомпенсацияХС = False
            newDataRow.Погрешность = 0
            newDataRow.Примечания = newName
            newDataRow.ДопускМинимум = CSng(frmTuningChannel.NumericLevelMin.Value)
            newDataRow.ДопускМаксимум = CSng(frmTuningChannel.NumericLevelMax.Value)
            ' сделать инкремент
            newDataRow.РазносУмин = I

            If frmTuningChannel.ListBoxUnit.Text = "дел" Then
                newDataRow.РазносУмакс = 10 + I
            Else
                newDataRow.РазносУмакс = 100 - (countAddChannels - 1) + I
            End If

            newDataRow.АварийноеЗначениеМин = 0
            newDataRow.АварийноеЗначениеМакс = 0
            newDataRow.Дата = Today ' при удалении конкуренция комманд
            newDataRow.Блокировка = False
            newDataRow.Видимость = False
            newDataRow.ВидимостьРегистратор = False
            newDataRow.EndEdit()

            dt.Rows.Add(newDataRow)
        Next

        If isAddingDAQBoard Then
            CheckSequenceNumber(NodeType.DAQBoard4)
        Else
            CheckSequenceNumber(NodeType.Module2)
        End If

        LoadTableChannels()
        FillChassis()

        If isAddingDAQBoard Then
            numberChannel = "1"         'НомерКанала
            chassis2 = numberBoard      'Корзина
            module3 = ""                'Модуль
            numberChannelModule = ""    'НомерКаналаМодуля
            nameChannel4 = ""           'ИмяКанала
            'unit1 =                    'ЕдИзмерения
        Else
            numberChannel = ""          'НомерКанала
            chassis2 = numberChassis    'Корзина
            module3 = numberModule      'Модуль
            numberChannelModule = "1"   'НомерКаналаМодуля
            nameChannel4 = ""           'ИмяКанала
            'unit1 =                    'ЕдИзмерения
        End If

        SelectChannelFromFinding()
    End Sub

    ''' <summary>
    ''' В зависимости от единицы измерения выдать имя метки
    ''' </summary>
    ''' <param name="inUnit"></param>
    ''' <returns></returns>
    Private Shared Function GetBaseNameChannelFromUnit(inUnit As String) As String
        Dim baseName As String
        Select Case inUnit
            Case "дел"
                baseName = "Дискретный"
            Case "мм"
                baseName = "Разрежение"
            Case "градус"
                baseName = "Температура"
            Case "Кгсм"
                baseName = "Давление"
            Case "мм/с"
                baseName = "Вибрация"
            Case "мкА"
                baseName = "Ток"
            Case "кг/ч"
                baseName = "Расход"
            Case "кгс"
                baseName = "Тяга"
            Case Else
                baseName = "Оборот"
                'Case "%"
                '    NewNameBase = "Оборот"
        End Select

        Return baseName
    End Function

    ''' <summary>
    ''' Добавить Платы
    ''' </summary>
    Private Sub AddDAQBoards()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Добавить Новую Плату")
        Const msg As String = "Введите номер вновь добавляемой платы." & ControlChars.CrLf &
                            "Нажмите Cancel для выхода."
        Dim numberBoard As String

        sortTransaction = conSortDESC

        Do
            Const caption As String = "Добавление платы"
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Question)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {msg}")

            numberBoard = InputBox(msg, "Добавление платы")
            ' проверка на существование
            If numberBoard <> "" Then
                If Not IsNumeric(numberBoard) Then
                    Const _caption As String = "Проверка номера платы"
                    Const _text As String = "Должна быть цифра!"
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                    Exit Sub
                End If

                If CInt(numberBoard) < 1 OrElse CInt(numberBoard) > maxDevice Then
                    Const _caption As String = "Проверка номера платы"
                    Dim _text As String = "Номер должен быть больше 0 и меньше " & maxDevice.ToString
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                    Exit Sub
                End If

                If dt.Select("НомерУстройства = " & numberBoard).Length = 0 Then
                    ' все нормально
                    AddNewDAQBoard(numberBoard)
                Else
                    Const _caption As String = "Добавление платы"
                    Dim _text As String = $"Плата с номером:  {numberBoard} уже существует!"
                    MessageBox.Show(_text, _caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{_caption}> {_text}")
                End If
            End If
        Loop Until numberBoard = ""
    End Sub

    ''' <summary>
    ''' Индикация кнопок для обновления базы
    ''' </summary>
    ''' <param name="isNeedUpdate"></param>
    Private Sub IndicationUpdateDBase(ByVal isNeedUpdate As Boolean)
        BindingNavigatorSaveDBase.Enabled = isNeedUpdate
        ButtonSave.Enabled = isNeedUpdate
        MenuFileSave.Enabled = isNeedUpdate

        If isNeedUpdate Then
            LabelPanelRegistration.Text = "Требуется обновление базы каналов"
        Else
            LabelPanelRegistration.Text = "Готово"
        End If
    End Sub

    ''' <summary>
    ''' Проверка Последовательности Номеров
    ''' </summary>
    ''' <param name="inNodeType"></param>
    Private Sub CheckSequenceNumber(ByVal inNodeType As NodeType)
        Dim numberParameter As Integer = 1 ' счётчик номеров по возрастанию

        If TreeChannels.Nodes.Count > 0 Then TreeChannels.CollapseAll() ' свернуть корневой узел
        ' на всякий случай заново переименуем номера, сначало заведомо нереальными, затем настоящими
        Dim I As Integer = 10000
        For Each itemDataRow As ChannelNRow In ChannelsNDataSet1.ChannelN.Rows ' заменил dt.Rows
            If itemDataRow.RowState <> DataRowState.Deleted Then
                itemDataRow.BeginEdit()
                itemDataRow.НомерПараметра = I
                itemDataRow.EndEdit()
            End If
            I += 1
        Next

        UpdateForm()
        LoadTableChannels()

        If inNodeType = NodeType.DAQBoard4 Then
            For indexBoard As Integer = 1 To maxDevice
                ' для платы
                ' заменил CheckSequenceArrayRows(dt.Select(...
                CheckSequenceArrayRows(CType(ChannelsNDataSet1.ChannelN.Select("НомерУстройства = " & indexBoard.ToString, conSortASC), ChannelNRow()), numberParameter)
            Next
        Else
            For indexDevice As Integer = 1 To maxDevice
                For indexModule As Integer = 1 To maxDevice
                    ' заменил CheckSequenceArrayRows(dt.Select(...
                    CheckSequenceArrayRows(CType(ChannelsNDataSet1.ChannelN.Select($"НомерУстройства = {indexDevice} AND НомерМодуляКорзины = '{indexModule}'", conSortASC), ChannelNRow()), numberParameter)
                Next
            Next
        End If

        UpdateForm()
    End Sub

    ''' <summary>
    ''' Проверка Массива
    ''' </summary>
    ''' <param name="inRows"></param>
    Private Sub CheckSequenceArrayRows(ByRef inRows As ChannelNRow(), ByRef numberParameter As Integer)
        If inRows.Length > 0 Then
            For Each itemDataRow As ChannelNRow In inRows
                If itemDataRow.RowState <> DataRowState.Deleted Then
                    If itemDataRow.НомерПараметра <> numberParameter Then
                        itemDataRow.НомерПараметра = numberParameter
                    End If
                    numberParameter += 1
                End If
            Next itemDataRow
        End If
    End Sub
#End Region

End Class