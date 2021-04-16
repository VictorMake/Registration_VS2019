Imports MathematicalLibrary
Imports NationalInstruments.DAQmx

Friend Class FormSnapshotPhotograph
    Protected Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, NameOf(FormSnapshotPhotograph))
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
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
        ' пока нет
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
        MenuProtocol.Enabled = False
        MenuComeBackToBeginning.Enabled = False
        MenuWriteDecodingSnapshotToDBase.Enabled = False
        MenuWriteDecodingSnapshotToExcel.Enabled = False
        MenuPrintDecodingSnapshot.Enabled = False
    End Sub
    ''' <summary>
    ''' Обработать Снимок
    ''' </summary>
    Friend Overrides Sub ProcesSnapshot()
        Dim K, M, L As Integer
        Dim average As Double
        Dim N As Integer = UBound(IndexParameters) - 1

        FrequencyBackgroundSnapshot = FrequencyHandQuery
        GKeyID = 0
        Cursor = Cursors.No

        If N > SnapshotLimit - 1 Then N = SnapshotLimit - 1

        For J As Integer = 0 To N
            ' здесь поиск по массиву arrIndexParameters положения (индекс) в массиве Volts (в нем все каналы из базы без промежутков по 4 модулям)
            ' в массиве arrIndexParameters содержатся индексы позиции параметра в базе
            M = 0
            L = J + 1 ' стало сейчас порядок параметров без пропусков

            For I As Integer = 0 To UBound(DataAnalogChannelReader, 2)
                If L <= SnapshotLimit Then ' было не более 64 параметров
                    average += DataAnalogChannelReader(L - 1, I)
                    K += 1

                    If K = DegreeDiscreditPhoto Then
                        MeasuredValues(J, M) = PhysicalValue(IndexParameters(J + 1), average / K)
                        M += 1
                        K = 0
                        average = 0
                    End If
                End If
            Next
        Next

        ConfigureWaveformGraphScale(CInt(ComboBoxTimeMeasurement.Text) * FrequencyHandQuery, FrequencyHandQuery)
        TuneDiagramUnderSelectedParameterAxesY()

        Cursor = Cursors.Default
        QuestionToWritingOnDisk()
        SlidePlot.Visible = True
        ComboBoxSelectAxis.Enabled = True
        ProgressBarExport.Visible = False

        ' если был запущен регистратор, то запустить его
        RunRegistrationForm()
    End Sub

    '''' <summary>
    '''' Настройка меню и кнопок
    '''' </summary>
    'Protected Overrides Sub EnableDisableControlsDerived()
    '    MyBase.EnableDisableControlsDerived()
    'End Sub

    '''' <summary>
    '''' Настройка контролов в зависимости от типа окон
    '''' продолжение реализации
    '''' </summary>
    'Protected Overrides Sub InitializeControlsBase()
    '    MyBase.InitializeControlsBase()

    '    ЗаполнитьСписокВремяСбора()
    'End Sub
    ''' <summary>
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub SettingsOptionProgram()
        'MyBase.SettingsOptionProgram()

        InstallQuestionForm(WhoIsExamine.Setting)
        SettingForm.ShowDialog() ' обязательно модально
        InstallQuestionForm(WhoIsExamine.Snapshot)
    End Sub

    ''' <summary>
    ''' Перестройка числа параметров для выбранного режима
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub CharacteristicForRegime()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(CharacteristicForRegime)}> Настройка параметров сбора для сконфигурированного режима")

        'If IsFormRunning Then StopAcquisition()

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
                'DecodingRegimeSnapshot()
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
            ButtonRecord.Enabled = False
            TextBoxRecordToDisc.Visible = False
            ButtonRecord.Image = My.Resources.StopSave
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.GRAPH04
        End If

        Cursor = Cursors.WaitCursor
        SlidePlot.Visible = False
        ComboBoxSelectAxis.Enabled = False

        ReadConfigurationRegime()
        UnpackStringConfigurationWithEmpty(ConfigurationString)
        Re.Dim(IndexParameters, 0)

        Dim I, J As Integer
        ' Массив IndexParameters() состоит только из параметров подлежащих измерению
        For I = 1 To UBound(NamesParameterRegime)
            For J = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = NamesParameterRegime(I) Then
                    Re.DimPreserve(IndexParameters, UBound(IndexParameters) + 1)
                    IndexParameters(UBound(IndexParameters)) = J
                    Exit For
                End If
            Next
        Next

        ' массив IndexParameters содержит перечень параметров по номерам
        ApplyScaleRangeAxisY(ParametersType)

        If UBound(IndexParameters) > SnapshotLimit Then
            ' ограничить для снимка SnapshotLimit
            Re.DimPreserve(IndexParameters, SnapshotLimit)
        End If

        RewriteListViewAcquisition(ParametersType)
        CleaningDiagram(UBound(IndexParameters), ParametersType)
        ComboBoxSelectAxis.Items.Clear() ' очистка списка

        For I = 1 To UBound(IndexParameters)
            ComboBoxSelectAxis.Items.Add(ParametersType(IndexParameters(I)).NameParameter)
        Next

        If Not IsNothing(IndexParameters) Then
            Re.Dim(CopyListOfParameter, IndexParameters.Length - 1)
            Array.Copy(IndexParameters, CopyListOfParameter, IndexParameters.Length)
        End If

        ' снимок значит нужна конфигурация буфера
        ButtonSnapshot.Enabled = True

        ComboBoxSelectAxis.SelectedIndex = 0 ' по умолчанию первый элемент активный
        SlidePlot.Value = ComboBoxSelectAxis.SelectedIndex + 1
        TuneDiagramUnderSelectedParameterAxesY()
        YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))

        If isUsePens Then TuneAnnotation()

        Cursor = Cursors.Default
    End Sub

    ''' <summary>
    ''' Настроить выборочный список
    ''' </summary>
    Protected Overrides Sub TuneSelectiveList()
        If RegimeType = cРегистратор Then ShowFormTuningList()
    End Sub
    ''' <summary>
    ''' Установить видимость MenuSimulator для включения иммитатора
    ''' </summary>
    Friend Overrides Sub SetEnabledMenuSimulator()
        ' ничего
    End Sub
#End Region

    ''' <summary>
    ''' Запуск сбора для кратковременного снимка
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonSnapshot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSnapshot.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonSnapshot_Click)}> Запуск сбора для кратковременного снимка")
        TakeSnapshot()
    End Sub
    ''' <summary>
    ''' Запуск сбора для кратковременного снимка
    ''' </summary>
    Private Sub TakeSnapshot()
        descriptionSnapshot = vbNullString

        If Not IsFormRunning Then InstallQuestionForm(WhoIsExamine.Snapshot)

        ClearArrowCollection()
        IsBeforeThatHappenLoadDbase = False
        IsSnapshot = True
        'сечениеПостроено = False
        timeStartRecord = TimeOfDay
        Cursor = Cursors.WaitCursor
        InitializeSnapshotDAQmxTask()
        ProgressBarExport.Maximum = CountsBuffers
        ProgressBarExport.Value = 0
        ProgressBarExport.Visible = True
        ' позицирование в конце, чтобы в обработчике не было ошибки на пустой массив arrСреднее
        XyCursorStart.XPosition = 0
        XyCursorEnd.XPosition = 0
        xCursorStart = 0
        xCursorEnd = 0
    End Sub

    ''' <summary>
    ''' Показать форму настройки конфигурации каналов для режима
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuConfigurationChannels_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuConfigurationChannels.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuConfigurationChannels_Click)}> Показать форму настройки конфигурации каналов для режима")
        InstallQuestionForm(WhoIsExamine.Snapshot)
        frmTuningRegime = New FormTuningRegime
        frmTuningRegime.ShowDialog()
        ApplyRegimeDebugging()
        AllGraphParametersByParameter = Nothing ' чтобы вызвать повторную инициализацию
    End Sub

    ''' <summary>
    ''' Вопрос запись на диск
    ''' </summary>
    Private Sub QuestionToWritingOnDisk()
        ' пользователь нажал да.
        Const caption As String = "Сохранение"
        Const text As String = "Записать измерения ?"
        RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

        If MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            'ОчисткаМассиваСечений()
            MenuWriteDecodingSnapshotToDBase_Click(MenuWriteDecodingSnapshotToDBase, New EventArgs)
        Else ' пользователь нажал нет.
            Exit Sub
        End If
    End Sub

#Region "Инициализаторы Device для сбора"
    ''' <summary>
    ''' Инициализаторы Device для сбора снимка
    ''' </summary>
    Private Sub InitializeSnapshotDAQmxTask()
        ' передается список параметров
        Dim J, limit As Integer

        If IsTaskRunning = False Then
            Try
                IsTaskRunning = True
                ' создать новую задачу
                If DAQmxTask IsNot Nothing Then
                    'myTask.Stop()
                    DAQmxTask.Dispose()
                    DAQmxTask = Nothing
                    'Else
                    '    myTask = New Task("aiTask")
                End If
                DAQmxTask = New Task("aiTask")

                If SnapshotLimit <= UBound(ParametersType) Then limit = SnapshotLimit Else limit = UBound(ParametersType)
                ' неправильно параметр может быть в модуле который больше

                For J = 1 To limit
                    If CheckMeasurementChannels(J) Then
                        ' подключить только измеряемые каналы
                        ' Create a virtual channel
                        ' добавлять только те каналы, которые помечены на сбор
                        If Array.IndexOf(IndexParameters, J) <> -1 Then
                            CreateAiChannel(ParametersType(J).NumberChannel.ToString, ParametersType(J).NumberDevice.ToString, ParametersType(J).NumberModuleChassis.ToString, ParametersType(J).NumberChannelModule.ToString,
                            Convert.ToDouble(ParametersType(J).LowerMeasure), Convert.ToDouble(ParametersType(J).UpperMeasure), ParametersType(J).TypeConnection, ParametersType(J).SignalType)
                        End If
                    End If
                Next

                MainModule.CountAcquisition = CInt(MyBase.ComboBoxTimeMeasurement.Text) * FrequencyHandQuery * DegreeDiscreditPhoto
                CountAcquisitionSCXI = 10 * (FrequencyHandQuery * DegreeDiscreditPhoto)
                ' фактическая частота сбора intЧастотаОпроса * intСтепеньПередискСнимка
                'myTask.Timing.ConfigureSampleClock("", intЧастотаОпроса * intСтепеньПередискСнимка, _
                '    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples)
                DAQmxTask.Timing.ConfigureSampleClock("", FrequencyHandQuery * DegreeDiscreditPhoto,
                    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, CountAcquisitionSCXI)

                ' доработка перенес из cmbВремяСбора_SelectedIndexChanged
                J = MainModule.CountAcquisition \ DegreeDiscreditPhoto

                Re.Dim(MeasuredValues, UBound(IndexParameters) - 1, J)
                Re.Dim(MeasuredValuesToRange, UBound(IndexParameters) - 1, J)
                Re.Dim(PackOfParametersToRecord, UBound(IndexParameters) - 2)
                CurrentCountBuffers = 0
                CountsBuffers = CInt(Val(ComboBoxTimeMeasurement.Text)) \ 10
                ConfigureBuffer(CountsBuffers, UBound(IndexParameters) - 1, MainModule.CountAcquisition)

                'myTask.Stream.ConfigureInputBuffer(intКолОпросов)

                ' проверить сконфигурированную задачу
                DAQmxTask.Control(TaskAction.Verify)

                AnalogInReader = New AnalogMultiChannelReader(DAQmxTask.Stream) With {.SynchronizeCallbacks = True}
                MultiAnalogCallback = New AsyncCallback(AddressOf MultiAnalogInCallback) 'MultiAnalogInCallback - процедура, MultiAnalogCallback - делегат
                AnalogInReader.BeginReadMultiSample(CountAcquisitionSCXI, MultiAnalogCallback, Nothing)
            Catch ex As DaqException
                Const caption As String = "Инициализация для снимка"
                IsTaskRunning = False
                DAQmxTask.Dispose()
                MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
            End Try
        End If
    End Sub
#End Region
End Class