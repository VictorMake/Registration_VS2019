Imports MathematicalLibrary

Friend Class FormSnapshotViewingDiagram

#Region "Ссылки"
    ''' <summary>
    ''' форма Нормы ТУ
    ''' </summary>
    Private frmNormTU As FormNormTU
    ''' <summary>
    ''' форма Протокол
    ''' </summary>
    Private frmFormProtocol As FormProtocol
    ''' <summary>
    ''' Фома поиска по условиям
    ''' </summary>
    Private frmConditionFind As FormConditionFind
#End Region
    ''' <summary>
    ''' Форма условия поиска загружена
    ''' </summary>
    ''' <returns></returns>
    Public Property IsLoadedFormConditionFind() As Boolean
    ''' <summary>
    ''' Позиция курсора на оси времени
    ''' </summary>
    Private indexTimePosition As Integer
    ''' <summary>
    ''' Кнопка перемещения Курсоров нажата
    ''' </summary>
    Private isButtonCursorMouseDown As Boolean
    ''' <summary>
    ''' естьМодулиСбораКТ
    ''' </summary>
    Private isModuleSolveKT As Boolean

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, "FormSnapshotViewingDiagram")
        'InitializeComponent()
    End Sub

    Public Sub New(ByVal frmParent As FormMainMDI, ByVal kindExamination As FormExamination, ByVal captionText As String)
        MyBase.New(frmParent, kindExamination, captionText)

        ' Этот вызов является обязательным для конструктора.
        'InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
    End Sub

#Region "Реализация интерфейса"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected Overrides Sub InheritFormLoad()
        ' пока нет
        CheckModuleSolveKT()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
        isCancel = e.Cancel
        Dim Msg, TITLE As String

        If IsFormSereverStart Then
            Msg = "Сервер передает данные клиенту." & vbCrLf & "В случае закрытия окна связь с клиентом прервется." & vbCrLf & "Вы точно уверены что хотите закрыть окно?"
            TITLE = "Закрытие окна" ' по умолчанию
            ' пользователь нажал да. 
            If MessageBox.Show(Msg, TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                If ServerForm IsNot Nothing Then
                    ServerForm.Close()
                End If
                IsFormSereverStart = False
                isCancel = False
            Else
                isCancel = True
                e.Cancel = isCancel
                Exit Sub
            End If
        End If

        If IsLoadedFormConditionFind Then frmConditionFind.Close()

        If isModuleSolveKT Then
            isModuleSolveKT = False
            MainMDIFormParent.ModuleAcquisitionKTManager.RemoveAllCalculationModule()
            MainMDIFormParent.MenuConfigurationCalculationModuleKT.Enabled = True
            MainMDIFormParent.ModuleAcquisitionKTManager.ParentFormMain = Nothing
        End If
    End Sub
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosed()
        ' пока нет
    End Sub
    ''' <summary>
    ''' Настройка меню и кнопок продолжение
    ''' </summary>
    Protected Overrides Sub EnableDisableControlsInherit()
        ' может быть загружено второе окно
        MenuChangingBase.Enabled = True
        MenuFindSnapshot.Enabled = True
        MenuConfigurationChannels.Enabled = False
        MenuNewWindowEvents.Enabled = False
        MenuQuestioningParameter.Enabled = False
        MenuNewWindowTarir.Enabled = False
        MenuConfigurationChannels.Enabled = False
        MenuNormTU.Enabled = True
        ComboBoxSelectAxis.Enabled = False
        ButtonSnapshot.Enabled = False
        ComboBoxTimeMeasurement.Visible = False
        LabelSec.Visible = False
        IsServer = True
        ' окно может быть вызвано из TCP клиента, а из TCP клиента ClientServerForm не загружается.
        LoadClientServerForm()

        AddMenuItemAdditionalRegime()
    End Sub
    ''' <summary>
    ''' Нет реализации
    ''' </summary>
    Friend Overrides Sub ProcesSnapshot()
    End Sub
    ''' <summary>
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub SettingsOptionProgram()
        'MyBase.SettingsOptionProgram()
        InstallQuestionForm(WhoIsExamine.Setting)
        SettingForm.ShowDialog() ' обязательно модально
    End Sub

    ''' <summary>
    ''' Перестройка числа параметров для выбранного режима
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub CharacteristicForRegime()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(CharacteristicForRegime)}> Настройка параметров расшифровки для выбранного режима режима")

        'If (ТипИспытания And FormExamination.SnapshotBase) AndAlso Снимок AndAlso Not IsTcpClient Then
        If IsSnapshot Then
            '    ' может быть:
            '    ' 1 Снимок с контроллером
            '    ' 2 Регистратор и Снимок для просмотра (Снимок=True)
            '    ' а нужен чисто Снимок для просмотра
            'If IsWorkWithController = False OrElse CollectionForms.CollectionForm.Keys.Where(Function(Key) Key.StartsWith("Регистратор")).Count = 0 Then
            If CollectionForms.Forms.Keys.Where(Function(Key) Key.StartsWith("Регистратор")).Count = 0 Then
                isRegimeChangeForDecoding = True
                ReadConfigurationRegime()
                TuneVisibilityAndSelectiveList()
                ChangeRegimeForDecoding() ' там очистка графиков и коллекции 
                'ОчисткаМассиваСечений()
                MenuPens.Checked = False
                DecodingRegimeSnapshot()
                ButtonDetailSelective.Checked = True
                Exit Sub
            Else
                Exit Sub
            End If
        End If

        ' сделать загрузку только тех параметров которые участвуют в данном испытании
        ' по номеру параметра из базы извлекается строка расшифровывается, по именам
        ' из arrПараметры комплектуется по номерам массив arrIndexParameters
        ' если режим регистрации запущен то вначале его нужно остановить
        If RegimeType = cРегистратор Then
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.SINEWAVE
        Else
            numberParameterAxesChanged = 1 ' заглушка
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.GRAPH04
        End If

        Cursor = Cursors.WaitCursor
        SlidePlot.Visible = False
        ComboBoxSelectAxis.Enabled = False

        ReadConfigurationRegime()
        UnpackStringConfigurationWithEmpty(ConfigurationString)
        'ReDim_IndexParameters(0)
        Re.Dim(IndexParameters, 0)

        Dim I, J As Integer
        ' Массив arrIndexParameters() состоит только из параметров подлежащих измерению
        For I = 1 To UBound(NamesParameterRegime)
            For J = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = NamesParameterRegime(I) Then
                    'ReDimPreserve IndexParameters(UBound(IndexParameters) + 1)
                    Re.DimPreserve(IndexParameters, UBound(IndexParameters) + 1)
                    IndexParameters(UBound(IndexParameters)) = J
                    Exit For
                End If
            Next
        Next

        ' массив IndexParameters содержит перечень параметров по номерам
        ApplyScaleRangeAxisY(ParametersType)

        RewriteListViewAcquisition(ParametersType)
        CleaningDiagram(UBound(IndexParameters), ParametersType)
        ComboBoxSelectAxis.Items.Clear() ' очистка списка

        For I = 1 To UBound(IndexParameters)
            ComboBoxSelectAxis.Items.Add(ParametersType(IndexParameters(I)).NameParameter)
        Next

        If Not IsNothing(IndexParameters) Then
            'ReDim_CopyListOfParameter(IndexParameters.Length - 1)
            Re.Dim(CopyListOfParameter, IndexParameters.Length - 1)
            Array.Copy(IndexParameters, CopyListOfParameter, IndexParameters.Length)
        End If

        ComboBoxSelectAxis.SelectedIndex = 0 ' по умолчанию первый элемент активный
        SlidePlot.Value = ComboBoxSelectAxis.SelectedIndex + 1
        TuneDiagramUnderSelectedParameterAxesY()
        YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))

        If isUsePens Then TuneAnnotation()

        Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' Настроить выборочный список
    ''' </summary>
    Protected Overrides Sub TuneSelectiveList()
        ' ничего
        If RegimeType = cРегистратор OrElse RegimeType = cОтладочныйРежим Then
            ShowFormTuningList()
        Else
            MessageBox.Show(Me, "В выбранном режиме расшифровки выборочный список каналов уже предопределён." & vbCrLf &
                            "Для возможности запуска редактора смените режим на <Отладочный>" & vbCrLf &
                            "и повторите вызов этого меню. ", "Редактор запустить нельзя.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Установить видимость MenuSimulator для включения иммитатора
    ''' </summary>
    Friend Overrides Sub SetEnabledMenuSimulator()
        MenuSimulator.Enabled = True
    End Sub
#End Region

    ''' <summary>
    ''' Проверка подключаемых модулей расчёта КТ
    ''' </summary>
    Private Sub CheckModuleSolveKT()
        isModuleSolveKT = False

        If MainMDIFormParent.ModuleAcquisitionKTManager IsNot Nothing Then
            ' каталог для работы с модулями существует
            If MainMDIFormParent.ModuleAcquisitionKTManager.IsEnableModules Then
                MainMDIFormParent.ModuleAcquisitionKTManager.LoadInheritanceForms()
                MainMDIFormParent.MenuConfigurationCalculationModuleKT.Enabled = False
                ' известны все имена СписокРасчетныхПараметров которые необходимо добавить в список каналов
                ' там уже могут быть добавлены каналы цифровых входов и АИ222 поэтому добавлять только в конец
                ' fMainForm.varМодульСбораКТManager.НастройкаКаналов()
                'MainMDIFormParent.ModuleAcquisitionKTManager.ParentFormMain = Me
                isModuleSolveKT = True
                'Else 'в сборе КТ нет
                '    'может модули в каталоге есть, а в конфигуратор не включены
                '    fMainForm.varМодульСбораКТManager.ОчиститьБазуКаналовОтРасчетныхПараметров()
            End If
            ' в любом случае надо обновить базу т.к. для клиентов важны все каналы
            ' но там уже могут быть добавлены каналы цифровых входов и АИ222
            ' в сборе КТ нет ЗагрузкаКаналов()
        End If
    End Sub

    ''' <summary>
    ''' Создание меню для дополнительных режимов.
    ''' В зависимости от наличия таблицы с именем "Изделие" в новых версиях базы Channel
    ''' добавляются меню выбора режима в подменю изделия или же просто в корень главного меню.
    ''' </summary>
    Private Sub AddMenuItemAdditionalRegime()
        managerAnalysis = New AnalysisManager(Me)
        managerAnalysis.Initialize()
    End Sub

    ''' <summary>
    ''' Создание меню режима
    ''' </summary>
    ''' <param name="inName"></param>
    ''' <param name="inText"></param>
    ''' <param name="inTag"></param>
    ''' <param name="inToolTipText"></param>
    ''' <returns></returns>
    Public Function NewToolStripMenuItem(inName As String, inText As String, inTag As String, inToolTipText As String) As ToolStripMenuItem
        Dim mnuNewRegime As New ToolStripMenuItem() With {
            .Image = Global.Registration.My.Resources.Resources.analysis,'= CType(Resources.GetObject("mnuРегистратор.Image"), System.Drawing.Image)
            .Name = inName,
            .Text = inText,
            .Tag = inTag,
            .ToolTipText = inToolTipText
        }
        AddHandler mnuNewRegime.Click, AddressOf MenuNewRegime_Click

        Return mnuNewRegime
    End Function

    ''' <summary>
    ''' Обработчик событи щелчка по меню
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuNewRegime_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        NumberRegime = Short.Parse(selectedItem.Tag.ToString)
        RegimeType = selectedItem.Text
        StatusStripMain.Items(NameOf(LabelRegistration)).Text = selectedItem.Text
        CharacteristicForRegime()
    End Sub

    ''' <summary>
    ''' Расшифровать Снимок
    ''' </summary>
    Private Sub DecodingRegimeSnapshot()
        'сечениеПостроено = False
        XyCursorStart.Visible = False
        XyCursorEnd.Visible = False

        Try
            'managerAnalysis.КоллекцияФормЗагрузок.Item(режим).Расшифровать()
            managerAnalysis(RegimeType).DecodingRegimeSnapshot()
        Catch ex As Exception
            Dim CAPTION As String = NameOf(DecodingRegimeSnapshot)
            Const TEXT As String = "Ошибка расшифровки снимка: " & vbCrLf
            MessageBox.Show(TEXT & ex.ToString, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", CAPTION, TEXT))
        End Try
    End Sub

#Region "События кнопок управления курсоров"
    Private Enum MoveCursorTo
        MoveFirstCursorToBackward = 1
        MoveFirstCursorToForward = 2
        MoveSecondCursorToBackward = 3
        MoveSecondCursorToForward = 4
    End Enum
    Private whereMoveCursor As MoveCursorTo

    ''' <summary>
    ''' Запуск имитатороа испытаний по загруженному снимку
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuSimulator_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MenuSimulator.CheckedChanged
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuSimulator_CheckedChanged)}> Запуск имитатороа испытаний по загруженному снимку")

        If MenuSimulator.Checked Then
            ' включить имитатор
            TimerCursor.Enabled = False
            TimerCursor.Interval = 1000 \ FrequencyBackgroundSnapshot

            GraphModeValue = MyGraphMode.OneCursor
            SetGraphMode(GraphModeValue)

            XyCursorStart.XPosition = 0
            indexTimePosition = 0
            TimerCursor.Enabled = True
        Else
            ' выключить имитатор
            TimerCursor.Enabled = False
        End If
    End Sub

    Private Sub TimerCursor_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TimerCursor.Tick
        If MenuSimulator.Checked Then
            With WaveformGraphTime
                indexTimePosition += 1

                If indexTimePosition >= XAxisTime.Range.Maximum Then
                    indexTimePosition = 0
                    XyCursorStart.XPosition = 0
                End If

                XyCursorStart.XPosition = Convert.ToDouble(indexTimePosition)
            End With
        Else ' управление курсора кнопками
            If isButtonCursorMouseDown Then
                Select Case whereMoveCursor
                    Case MoveCursorTo.MoveFirstCursorToBackward ' уменьшить
                        MyBase.MoveCursor(MyBase.XyCursorStart, False)
                        Exit Select
                    Case MoveCursorTo.MoveFirstCursorToForward ' увеличить
                        MyBase.MoveCursor(MyBase.XyCursorStart, True)
                        Exit Select
                    Case MoveCursorTo.MoveSecondCursorToBackward ' уменьшить
                        MyBase.MoveCursor(MyBase.XyCursorEnd, False)
                        Exit Select
                    Case MoveCursorTo.MoveSecondCursorToForward ' увеличить
                        MyBase.MoveCursor(MyBase.XyCursorEnd, True)
                        Exit Select
                End Select
            Else
                TimerCursor.Enabled = False
            End If
        End If
    End Sub

    Private Sub ButtonСursorEndMoveBack_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorEndMoveBack.MouseDown
        isButtonCursorMouseDown = True
        TimerCursor.Interval = 100
        whereMoveCursor = MoveCursorTo.MoveSecondCursorToBackward
        ButtonСursorEndMoveBack.BackColor = Color.Red
        TimerCursor.Enabled = True
    End Sub

    Private Sub ButtonСursorEndMoveForward_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorEndMoveForward.MouseDown
        isButtonCursorMouseDown = True
        TimerCursor.Interval = 100
        whereMoveCursor = MoveCursorTo.MoveSecondCursorToForward
        ButtonСursorEndMoveForward.BackColor = Color.Red
        TimerCursor.Enabled = True
    End Sub

    Private Sub ButtonСursorEndMoveBack_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorEndMoveBack.MouseUp
        isButtonCursorMouseDown = False
        ButtonСursorEndMoveBack.BackColor = Color.Cyan
    End Sub

    Private Sub ButtonСursorEndMoveForward_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorEndMoveForward.MouseUp
        isButtonCursorMouseDown = False
        ButtonСursorEndMoveForward.BackColor = Color.Cyan
    End Sub

    Private Sub ButtonСursorStartMoveForward_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorStartMoveForward.MouseDown
        isButtonCursorMouseDown = True
        TimerCursor.Interval = 100
        whereMoveCursor = MoveCursorTo.MoveFirstCursorToForward
        ButtonСursorStartMoveForward.BackColor = Color.Red
        TimerCursor.Enabled = True
    End Sub

    Private Sub ButtonСursorStartMoveForward_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorStartMoveForward.MouseUp
        isButtonCursorMouseDown = False
        ButtonСursorStartMoveForward.BackColor = Color.Yellow
    End Sub

    Private Sub ButtonСursorStartMoveBack_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorStartMoveBack.MouseDown
        ' здесь по логической переменной пока не произошло события вверх
        ' по таймеру происходят события клик
        ' когда событие вверх переменная сбрасывается
        ' таймер выключается
        isButtonCursorMouseDown = True
        TimerCursor.Interval = 100
        whereMoveCursor = MoveCursorTo.MoveFirstCursorToBackward
        ButtonСursorStartMoveBack.BackColor = Color.Red
        TimerCursor.Enabled = True
    End Sub

    Private Sub ButtonСursorStartMoveBack_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ButtonСursorStartMoveBack.MouseUp
        isButtonCursorMouseDown = False
        ButtonСursorStartMoveBack.BackColor = Color.Yellow
    End Sub
#End Region

    ''' <summary>
    ''' Показать форму протокола расшифровки
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuProtocol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuProtocol.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuProtocol_Click)}> Показать форму протокола расшифровки")
        frmFormProtocol = New FormProtocol(managerAnalysis(RegimeType).Protocol)
        frmFormProtocol.Show()
        frmFormProtocol.Activate()
    End Sub
    ''' <summary>
    ''' Смена текущей рабочей базы на базу из каталога архивов
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuChangingBase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuChangingBase.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuChangingBase_Click)}> Смена текущей рабочей базы на базу из каталога архивов")

        ' доступно только при автономном режиме
        With MainMDIFormParent.OpenFileDialog1
            .FileName = vbNullString
            .Title = "Текущий каталог базы-> " & PathChannels
            .DefaultExt = "mdb"
            ' установить флаг атрибутов
            .Filter = "База испытаний (*.mdb)|*.mdb"
            .RestoreDirectory = True
            If .ShowDialog() = DialogResult.OK AndAlso Len(.FileName) <> 0 Then
                PathChannels = .FileName
                ' записать путь к рабочей базе
                WriteINI(PathOptions, "TheCurrentBase", "Base", PathChannels)
                IsDataBaseChanged = True
                MenuOpenBaseSnapshot.PerformClick() ' сразу же вызвать пункт меню открытия
            End If
        End With
    End Sub
    ''' <summary>
    ''' Показать форму поиска кадров удовлетворяющих условию
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuFindSnapshot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFindSnapshot.Click
        If Not IsLoadedFormConditionFind Then
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuFindSnapshot_Click)}> Показать форму поиска кадров удовлетворяющих условию")
            frmConditionFind = New FormConditionFind(Me)
            frmConditionFind.Show()
        End If

        frmConditionFind.Activate()
    End Sub
    ''' <summary>
    ''' Показать таблицу норм ТУ
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuNormTU_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNormTU.Click
        frmNormTU = New FormNormTU(Me.ParentForm)
        frmNormTU.Show()
        frmNormTU.Activate()
    End Sub

    ''' <summary>
    ''' Поставить Метку Кадра Условия
    ''' Вставляется метка в кадр при поиске условий для обозначения места, где условие сработало.
    ''' </summary>
    ''' <param name="description"></param>
    Public Sub AddMarkingFrameFoundCondition(ByVal description As String)
        GraphModeValue = MyGraphMode.Scaling
        SetGraphMode(GraphModeValue)
        XyCursorStart.XPosition = frmConditionFind.CurrentPosition
        XyCursorEnd.XPosition = frmConditionFind.CurrentPosition
        AddMarking(description)

        XAxisTime.Range = New Range(CInt(XAxisTime.Range.Minimum), CInt(XAxisTime.Range.Maximum))
    End Sub
End Class