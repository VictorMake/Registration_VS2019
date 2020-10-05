Imports MathematicalLibrary
Imports NationalInstruments.DAQmx

Friend Class FormRegistrationSCXI
    ''' <summary>
    ''' Счетчик цикла для передачи данных клиентам
    ''' </summary>
    Private counterDataToNetwork As Integer
    ''' <summary>
    ''' Данные в сеть
    ''' </summary>
    Private isSendDataToClient As Boolean

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, "FormRegistrationSCXI")
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
        IsServer = True
        LoadClientServerForm()
        CheckOnModificationNameChannel()
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
        GenerateNull() 'перенос в frmDigital
        'myTaskCWAOPoint1.Dispose()
        'myTaskCWAOPoint1 = Nothing
        'writerAOPoint1 = Nothing
        MenuNewWindowRegistration.Enabled = True
        MainMDIFormParent.MenuNewWindowRegistration.Enabled = True
    End Sub

    'Public Event AcquiredDataFormMain(ByVal sender As Object, ByVal e As AcquiredDataEventArgs)
    ''' <summary>
    ''' Вызов событие базового класса посредством переопредеяемого метода
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnAcquiredData(e As AcquiredDataEventArgs)
        MyBase.OnAcquiredData(e)
    End Sub
    ''' <summary>
    ''' Настройка меню и кнопок продолжение
    ''' </summary>
    Protected Overrides Sub EnableDisableControlsInherit()
        MenuFindSnapshot.Enabled = False
        MenuParameterInRange.Enabled = True
        ButtonContinuously.Enabled = True
        ButtonRecord.Enabled = True
        isButtonRecordPressHandle = False
        GenerateNull()

        If IsUseTdms Then
            ' если выбран формат сохранения как TDMS и производится выбор частоты и длительности кадра
            If gTdmsFileProcessor IsNot Nothing Then
                gTdmsFileProcessor.CloseTDMSFile()
                gTdmsFileProcessor.Dispose()
            End If

            gTdmsFileProcessor = New TdmsFileProcessor(LimitAddExel, FrequencyBackground, WaveformSampleIntervalMode.Regular, True)
            AddHandler gTdmsFileProcessor.ClosedTDMSFileEvent, AddressOf TdmsFilePr_ClosedTDMSFileEvent
            AddHandler gTdmsFileProcessor.ContinueAccumulation, AddressOf TdmsFilePr_ContinueAccumulation
        End If
    End Sub

    ''' <summary>
    ''' Настройка конфигурации режима
    ''' </summary>
    Protected Overrides Sub TuneConditionsRegime()
        If ButtonContinuously.Checked Then ButtonContinuously.Checked = False

        InstallQuestionForm(WhoIsExamine.Examination)
        frmTuningRegime = New FormTuningRegime
        frmTuningRegime.ShowDialog()

        ' чтобы измененния отразились
        InstallQuestionForm(WhoIsExamine.Oscilloscope)
    End Sub

    ''' <summary>
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub SettingsOptionProgram()
        'MyBase.SettingsOptionProgram()
        Dim isNeedRestart As Boolean = IsFormRunning ' Надо презапустить
        If ButtonContinuously.Checked Then ButtonContinuously.Checked = False

        InstallQuestionForm(WhoIsExamine.Setting)
        SettingForm.ShowDialog() ' обязательно модально
        InstallQuestionForm(WhoIsExamine.Oscilloscope)

        If isNeedRestart Then CharacteristicForRegime() ' там запуск

        StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyBackground) & "Гц"
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' </summary>
    Protected Overrides Sub StartStopSocket(start As Boolean)
    End Sub

    ''' <summary>
    ''' для сбора с событием список должен содержать каналы только по мультиплексорам
    ''' каналы в списке по модулям должны идти в возрастающем порядке без разрывов
    ''' </summary>
    Protected Overrides Sub InitializeContinuousDAQmxTask()
        MainMDIFormParent.TimerAwait.Enabled = True
        ' передается список параметров
        IsSnapshot = False

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

                'intКолОпросов  = intСтепеньПередиск
                '.ScanClock.Frequency = intЧастотаФонового * intСтепеньПередиск
                For J As Integer = 1 To UBound(ParametersType)
                    If CheckMeasurementChannels(J) Then
                        ' подключить только измеряемые каналы
                        ' создать виртуальный канал
                        ' добавлять только те каналы, которые помечены на сбор
                        If Array.IndexOf(IndexParameters, J) <> -1 Then
                            CreateAiChannel(ParametersType(J).NumberChannel.ToString,
                                            ParametersType(J).NumberDevice.ToString,
                                            ParametersType(J).NumberModuleChassis.ToString,
                                            ParametersType(J).NumberChannelModule.ToString,
                                            Convert.ToDouble(ParametersType(J).LowerMeasure),
                                            Convert.ToDouble(ParametersType(J).UpperMeasure),
                                            ParametersType(J).TypeConnection,
                                            ParametersType(J).SignalType)
                        End If
                    End If
                Next

                ' фактическая частота сбора
                DAQmxTask.Timing.ConfigureSampleClock("",
                                                   FrequencyBackground * LevelOversampling,
                                                   SampleClockActiveEdge.Rising,
                                                   SampleQuantityMode.ContinuousSamples)

                ' проверить сконфигурированную задачу
                DAQmxTask.Control(TaskAction.Verify)
                AnalogInReader = New AnalogMultiChannelReader(DAQmxTask.Stream) With {.SynchronizeCallbacks = True}
                MultiAnalogCallback = New AsyncCallback(AddressOf MultiAnalogInCallback) 'MultiAnalogInCallback - процедура, MultiAnalogCallback - делегат

                If IsDigitalInput Then StartTaskDigitalInput()

                AnalogInReader.BeginReadMultiSample(MainModule.CountAcquisition, MultiAnalogCallback, Nothing)
            Catch ex As DaqException
                Const caption As String = "Инициализация с событием"
                IsTaskRunning = False
                DAQmxTask.Dispose()
                MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Перестройка числа параметров для выбранного режима
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub CharacteristicForRegime()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(CharacteristicForRegime)}> Настройка параметров сбора для сконфигурированного режима")

        Dim I, J As Integer
        Dim left, height, width As Integer
        Dim memoIsFormRunning As Boolean = IsFormRunning ' запуск Был

        If IsFormRunning Then StopAcquisition()

        ' сделать загрузку только тех параметров которые участвуют в данном испытании
        ' по номеру параметра из базы извлекается строка расшифровывается, по именам
        ' из arrПараметры комплектуется по номерам массив arrIndexParameters
        ' если режим регистрации запущен то вначале его нужно остановить
        If RegimeType = cРегистратор Then
            ButtonRecord.Enabled = True
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.SINEWAVE
        Else
            numberParameterAxesChanged = 1 ' заглушка
            ButtonRecord.Enabled = False
            TextBoxRecordToDisc.Visible = False
            ButtonRecord.Image = My.Resources.StopSave
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.GRAPH04
            StopRecord()
        End If

        Cursor = Cursors.WaitCursor
        SlidePlot.Visible = False
        ComboBoxSelectAxis.Enabled = False

        ReadConfigurationRegime()
        UnpackStringConfigurationWithEmpty(ConfigurationString)
        'ReDim_IndexParameters(0)
        Re.Dim(IndexParameters, 0)
        ' выгрузить индикаторы аварийных значений
        UnloadAlarmButton()
        left = AlarmPulseButton(keyFirstButtonAlarm).Left
        width = AlarmPulseButton(keyFirstButtonAlarm).Width
        height = AlarmPulseButton(keyFirstButtonAlarm).Height
        ' для снимка уже установленные другой формой глобальные переменные не менять
        AdditionalParameterCount = 0 : DigitalParametersCount = 0 : CountSolveParameters = 0

        ' Массив arrIndexParameters() состоит только из параметров подлежащих измерению
        For I = 1 To UBound(NamesParameterRegime)
            For J = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = NamesParameterRegime(I) Then
                    'ReDimPreserve IndexParameters(UBound(IndexParameters) + 1)
                    Re.DimPreserve(IndexParameters, UBound(IndexParameters) + 1)
                    IndexParameters(UBound(IndexParameters)) = J

                    ' для снимка уже установленные другой формой глобальные переменные не менять
                    If Not CheckMeasurementChannels(J) Then
                        AdditionalParameterCount += 1
                        If ParametersType(J).Mistake = IndexDiscreteInput Then
                            DigitalParametersCount += 1
                        End If

                        If ParametersType(J).Mistake = indexCalculated Then
                            CountSolveParameters += 1
                        End If
                    End If

                    ' проверка на аварийное значение
                    If ParametersType(J).AlarmValueMin <> 0 OrElse ParametersType(J).AlarmValueMax <> 0 Then
                        PopulateAlarmButton(J)
                        AlarmPulseButton(J).SetBounds(left,
                                                      AlarmPulseButton(keyLastButtonAlarm).Top + AlarmPulseButton(keyLastButtonAlarm).Height + 1,
                                                      width,
                                                      height)
                        AlarmPulseButton(J).BringToFront()
                    End If

                    Exit For
                End If
            Next
        Next

        ' переписать для контроля 
        If IsShowTextControl Then frmTextControl.Close()
        If IsShowGraphControl Then frmGraphControl.Close()

        'ReDim_IndexParametersForControl(UBound(IndexParameters))
        Re.Dim(IndexParametersForControl, UBound(IndexParameters))

        For I = 1 To UBound(IndexParameters)
            IndexParametersForControl(I) = IndexParameters(I)
        Next

        ' массив IndexParameters содержит перечень параметров по номерам
        ApplyScaleRangeAxisY(ParametersType)
        RewriteListViewAcquisition(ParametersType)
        CleaningDiagram(UBound(IndexParameters), ParametersType)
        ComboBoxSelectAxis.Items.Clear() ' очистка списка

        For I = 1 To UBound(IndexParameters)
            ComboBoxSelectAxis.Items.Add(ParametersType(IndexParameters(I)).NameParameter)
        Next

        InitializeDiscreteLed()

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
        'ReDim_PackOfParameters(UBound(IndexParameters) * 3)
        Re.Dim(PackOfParameters, UBound(IndexParameters) * 3)

        If IsFrmDigitalOutputPortStart Then DigitalPortForm.PopulateListParametersFromServer()
        If gFormsPanelManager.FormPanelMotoristCount > 0 Then gFormsPanelManager.PopulateListParametersFromServer()

        If IsUseCalculationModule Then
            MainMDIFormParent.ModuleSolveManager.EnableMenu(True)
            MainMDIFormParent.ModuleSolveManager.PopulateListParametersFromServer()
        End If

        If memoIsFormRunning Then
            IsFormRunning = False
            StartContinuous()
        End If
    End Sub

    ''' <summary>
    ''' Настроить выборочный список
    ''' </summary>
    Protected Overrides Sub TuneSelectiveList()
        MainMDIFormParent.TimerAwait.Enabled = False
        ShowFormTuningList()
        MainMDIFormParent.TimerAwait.Enabled = True
    End Sub
    ''' <summary>
    ''' Остановить сбор или приём данных
    ''' </summary>
    Protected Overrides Sub StopAcquisition()
        ' остановить сбор чтобы мерцание не мешало combobox
        If ButtonContinuously.Checked Then ButtonContinuously.Checked = False
    End Sub
    ''' <summary>
    ''' Определить удленение шкалы
    ''' Для клиента в связи с пропусками пакетов время сбора растягивается
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides Function GetLengtheningScale() As Double
        Return 1.0
    End Function
    ''' <summary>
    ''' Нет реализации
    ''' Обновление таблицы Режимы при проверке подключаемых расчётных модулей при работе TcpClient
    ''' </summary>
    Protected Overrides Sub UpdateTableRegimeForTcpClient()
        ' ничего
    End Sub
    ''' <summary>
    ''' Обновить компенсацию ХС для регистратора SCXI
    ''' </summary>
    Protected Overrides Sub UpdateCompensationXC()
        If TemperatureTxc > -10 AndAlso TemperatureTxc < 40 Then
            For I As Integer = 1 To UBound(ParametersType)
                If ParametersType(I).CompensationXC Then ParametersType(I).Offset = TemperatureTxc
            Next I
        End If
    End Sub
    ''' <summary>
    ''' Обновление данных на экране из события сбора DAQmx
    ''' </summary>
    Friend Overrides Sub AcquiredData()
        Dim J, N, IndexPlot As Integer
        Dim average As Double ' осреднение
        Dim name As String
        Dim vKey As Integer
        Dim indexDiscret As Integer ' индекс Цифровые
        Dim isHeightLevel As Boolean
        Dim index As Integer
        Dim strData As String
        Dim lengthArrayIndexParamMinus1 As Integer = UBound(IndexParameters) - 1 ' uBound Список Параметров Минус 1
        Dim startAddedChannels As Integer = CountMeasurand - AdditionalParameterCount 'Начало добавленных каналов
        Dim isVisible As Boolean ' для аварийных индикаторов

        countUpdateScreen1 += 1S
        counterDataToNetwork += 1

        If countUpdateScreen1 >= RefreshScreen Then
            isFireUpdateScreen1 = True
            isNeedToCountForUpdateScreen2 = True
        End If

        If isNeedToCountForUpdateScreen2 Then countUpdateScreen2 += 1S
        If countUpdateScreen2 >= RefreshScreen2 Then isFireUpdateScreen2 = True
        If counterDataToNetwork >= RefreshDataToNetwork Then isSendDataToClient = True

        xNewPos = Math.Round((lastx + 1), 2) Mod (arraysize + 1) ' запись в массив среднего, код такой-же как SweepChart
        indexTimeVsRow = CInt(lastx)

        For J = 0 To CountMeasurand
            vKey = J + 1 ' ищем этот номер
            N = IndexParameters(vKey)

            If J > startAddedChannels Then
                ' расчётные
                If IsUseCalculationModule AndAlso J > (CountMeasurand - CountSolveParameters) Then
                    ' уже цифры занесены с предыдущего расчета, поэтому отставание на время вывести изображение
                    average = MainMDIFormParent.ModuleSolveManager.ValueCalculationParameters(ParametersType(N).NumberChannel)
                    ' на самом деле индекс этого массива arrПараметры(N).intНомерКанала = ЦиклРасчетныйПараметр.ИндексКаналаИзмерения
                ElseIf IsDigitalInput Then ' дискретные
                    average = DigitalInputValue(ParametersType(N).NumberChannel) ' на самом деле индекс этого массива
                End If
            Else
                average = DataValuesFromServer(vKey)

                ' накопить строку для отображения в форме параметры в диапазоне
                If IsShowFormParametersInRange AndAlso isFireUpdateScreen1 Then
                    If average >= MinLimitParameter AndAlso average <= MaxLimitParameter Then
                        listParameterInRange &= $"{ParametersType(N).NameParameter}  № модуля:{ParametersType(N).NumberModuleChassis}  № канала:{ParametersType(N).NumberChannelModule}{vbCrLf}"
                    End If
                End If

                '*---------Вычисление физич. значений ------------*
                ' перенесена из Модуль1()
                ' Function ФизическиеЗначения(v As Integer, ByVal w As Double) As Double
                ' v- номер параметра
                ' w- значение аргумента - напряжения или кода
                ' fy = 0
                Select Case ParametersType(N).NumberFormula
                    Case 2
                        'For intx = arrПараметры(N).intСтепеньАппроксимации To 0 Step -1
                        '    fy = fy * average + arrПараметры(N).sgnКоэффициенты(intx)
                        'Next intx
                        'average = fy + arrПараметры(N).sgnСмещение
                        'measureVolt(0) = average
                        'measureVolt = ArrayOperation.PolynomialEvaluation1D(measureVolt, ArrayOperation.CopyRow(CoefficientPolynomial2D, N))
                        'average = measureVolt(0) + ParametersType(N).Offset
                        average = PolynomialEvaluation(average, ParametersType(N).LevelOfApproximation,
                                                       {CoefficientPolynomial2D(N, 0),
                                                       CoefficientPolynomial2D(N, 1),
                                                       CoefficientPolynomial2D(N, 2),
                                                       CoefficientPolynomial2D(N, 3),
                                                       CoefficientPolynomial2D(N, 4),
                                                       CoefficientPolynomial2D(N, 5)}) + ParametersType(N).Offset
                        Exit Select
                    Case 3
                        If average <= 1 Then average = 0 Else average = 5
                        Exit Select
                    Case 1
                        'For intx = arrПараметры(N).intСтепеньАппроксимации To 0 Step -1
                        '    fy = fy * average + arrПараметры(N).sgnКоэффициенты(intx)
                        'Next intx
                        ''для расхода по формуле: Qлитр=коэф.из поверки*Fгц=А5*полином(АЦП(модульВ5(Fгц))
                        ''=arrПараметры(n).sgnКоэффициенты(5)* polin(n, average)
                        ''полином не может быть больше 4 степени, так как в А5 записывается коэффициент b1 из паспорта ТДР-7 или ТДР-10
                        'average = arrПараметры(N).sgnКоэффициенты(5) * fy + arrПараметры(N).sgnСмещение
                        'measureVolt(0) = average
                        'measureVolt = ArrayOperation.PolynomialEvaluation1D(measureVolt, ArrayOperation.CopyRow(CoefficientPolynomial2D, N))
                        'average = ParametersType(N).Coefficient(5) * measureVolt(0) + ParametersType(N).Offset
                        average = PolynomialEvaluation(average, ParametersType(N).LevelOfApproximation,
                                                       {CoefficientPolynomial2D(N, 0),
                                                       CoefficientPolynomial2D(N, 1),
                                                       CoefficientPolynomial2D(N, 2),
                                                       CoefficientPolynomial2D(N, 3),
                                                       CoefficientPolynomial2D(N, 4),
                                                       CoefficientPolynomial2D(N, 5)}) * ParametersType(N).Coefficient(5) + ParametersType(N).Offset
                        'Case 0
                        'average = average
                        Exit Select
                End Select
            End If
            '*---------Конец вычисление физич. значений ------------*

            name = ParametersType(N).NameParameter

            '*---------начало Вывода Изображения ------------*
            If isFireUpdateScreen1 AndAlso xNewPos <> 0 Then
                Select Case name
                    Case NameTBox
                        TemperatureOfBox = average
                        coefficientBringingTBoxing = Math.Sqrt(Const288 / (TemperatureOfBox + Kelvin))
                        Exit Select
                    Case conN1, NameVarStopWrite
                        If IsQuestioningRevolution = False AndAlso average < ValueStopWrite AndAlso TimerButtonRun.Enabled = False Then ' N1<1% ->  выключить запись и задержка кнопки Запуск>10 секунд
                            If Not isButtonRecordPressHandle Then
                                IsQuestioningRevolution = True
                                TimerSwitchOffRecord.Interval = WaitStopWrite '40000 '40 секунд ждать до остановки
                                TimerSwitchOffRecord.Enabled = True
                            End If
                        End If
                        TextBoxIndicatorN1physics.Text = $"N1ф={Format(average, FormatString)} %"
                        TextBoxIndicatorN1reduce.Text = $"N1п={Format(average * coefficientBringingTBoxing, FormatString)} %"
                        Exit Select
                    Case conN2
                        TextBoxIndicatorN2physics.Text = $"N2ф={Format(average, FormatString)} %"
                        N2reduced = average * coefficientBringingTBoxing
                        TextBoxIndicatorN2reduce.Text = $"N2п={Format(N2reduced, FormatString)} %"
                        Exit Select
                    Case conаРУД
                        TextBoxIndicatorRud.Text = $"Руд={Format(average, FormatString)}" ' & "дел"
                        ARUD = average
                        Exit Select
                    Case NameTx
                        TemperatureTxc = average
                        Exit Select
                    Case Else
                        ' здесь проверка дискретных по номеру формулы
                        If ParametersType(N).NumberFormula = 3 Then
                            If name = NameVarStartWrite Then
                                If IsQuestioningRevolution AndAlso average = 5 Then ' ->  включить запись
                                    If (Not IsRecordEnable) AndAlso (Not isRecordingSnapshot) AndAlso ButtonRecord.Enabled Then
                                        WorkTaskRecordStartStop(False, True, IsFormRunning)
                                        TimerButtonRun.Interval = WaitStartWrite ' 60000
                                        TimerButtonRun.Enabled = True ' задержка на мертвую зону пока не раскрутился N1
                                    End If
                                End If
                            End If

                            If countDiscreteLed <> 0 Then
                                indexDiscret += 1
                                isHeightLevel = average = 5

                                If listDiscreteLed(indexDiscret - 1).Value <> isHeightLevel Then listDiscreteLed(indexDiscret - 1).Value = isHeightLevel
                            End If
                        End If
                        Exit Select
                End Select

                ' Проверка нахождения значения параметра в диапазоне и изменение цвета цифрового индикатора.
                If ParametersType(N).AlarmValueMin <> 0 OrElse ParametersType(N).AlarmValueMax <> 0 Then
                    isVisible = False

                    If average > ParametersType(N).AlarmValueMax Then
                        isVisible = True
                        AlarmPulseButton(N).Text = $"{name}>{CStr(ParametersType(N).AlarmValueMax)}"
                        If ParametersType(N).Blocking Then GenerateAlarm()
                    ElseIf average < ParametersType(N).AlarmValueMin Then
                        isVisible = True
                        AlarmPulseButton(N).Text = $"{name}<{CStr(ParametersType(N).AlarmValueMin)}"
                        If ParametersType(N).Blocking Then GenerateAlarm()
                    End If

                    If AlarmPulseButton(N).Visible <> isVisible Then AlarmPulseButton(N).Visible = isVisible
                End If
            End If ' вывести изображение
            '*---------конец Вывода Изображения ------------*

            ' накопление для всех подписчиков измеренных каналов
            If IsSubscriptionOnEventsAcquisitionEnabled OrElse IsUseCalculationModule OrElse (IsMonitorDigitalOutputPort AndAlso IsFrmDigitalOutputPortStart) Then
                ' накапливать нельзя т.к. blnМониторDigitalOutputPort работает асинхронно
                ParameterAccumulate(N) = average
            End If

            ' запись в массив среднего, код такой-же, как SweepChart
            MeasuredValues(J, indexTimeVsRow) = average

            If Not IsUseTdms Then PackOfParametersToRecord(J) = Math.Round(average, Precision).ToString

            If isDetailedSheet Then
                If ParametersType(IndexParameters(vKey)).IsVisible Then
                    average = CastToAxesStandard(NumberParameterAxes, vKey, average) ' массив приведен к какой-то шкале
                    MeasuredValuesToRange(J, 0) = average

                    If IndexPlot <= countVisibleTrendRegistrator Then
                        dataAllTrends(IndexPlot, indexTimeVsRow) = average
                    End If
                    IndexPlot += 1
                End If
            Else
                If SnapshotSmallParameters(vKey).IsVisible Then
                    average = CastToAxesStandard(NumberParameterAxes, vKey, average) ' массив приведен к какой-то шкале
                    MeasuredValuesToRange(J, 0) = average ' массив приведен к какой-то шкале

                    If IndexPlot <= countVisibleTrendRegistrator Then
                        dataAllTrends(IndexPlot, indexTimeVsRow) = average
                    End If

                    IndexPlot += 1
                End If
            End If

            average = 0
        Next J ' по измеренным каналам

        ' передать непрерывно Клиенту
        If IsServerOn Then
            index = 1
            For J = 0 To CountMeasurand
                vKey = J + 1
                N = IndexParameters(vKey)
                PackOfParameters(index) = ParametersType(N).NameParameter
                PackOfParameters(index + 1) = CStr(Math.Round(MeasuredValues(J, indexTimeVsRow), Precision))
                PackOfParameters(index + 2) = CStr(ParametersType(N).NumberParameter)
                index += 3
            Next
        End If

        If Not IsUseTdms Then
            strData = Join(PackOfParametersToRecord, vbTab)
            dataMeasuredValuesString.Append(strData & vbCrLf)
        End If

        countRow += 1
        SweepChart()

        If IsServerOn AndAlso isSendDataToClient Then
            ServerForm.DataSocketSend.Data.Value = CObj(Join(PackOfParameters, Separator).Substring(1) & Separator)
        End If

        If isShowDiagramFromParameter Then OutParametersGraph()

        '*---------начало второго Вывода Изображения ------------*
        ' сделать передачу собранных параметров как аргумент к событию
        ' все желающие формы подписываются на получение этого события и оно вызывается не в момент Вывода Изображения вывода на экран, 
        ' а например чуть ранее, чтобы не загружать данный цикл
        ' вызов frmMainAcquiredData происходит в следующем цикле сбора - это попытка разнести вызовы обновлений экрана
        If isFireEventAcquiredData Then
            isFireEventAcquiredData = False
            If IsSubscriptionOnEventsAcquisitionEnabled OrElse IsUseCalculationModule Then
                'Dim fireAcquiredDataEventArgs As New AcquiredDataEventArgs(ParameterAccumulate)
                ''  Теперь вызов события с помощью вызова делегата.
                ''  Передать object которое инициирует событие (Me) такое же как FireEventArgs. 
                ''  Вызов обязан соответствовать сигнатуре FireEventHandler.
                'RaiseEvent AcquiredDataFormMain(Me, fireAcquiredDataEventArgs)
                OnAcquiredData(New AcquiredDataEventArgs(ParameterAccumulate))
            End If
        End If

        If isFireUpdateScreen2 AndAlso xNewPos <> 0 Then
            isFireEventAcquiredData = True

            ' пользовательская настройка частоты обновления значений параметров
            If countTextDisplayRate >= TextDisplayRate Then
                countTextDisplayRate = 0

                ListViewAcquisition.BeginUpdate()
                With ListViewAcquisition ' обновить лист
                    If isDetailedSheet Then
                        For J = 0 To lengthArrayIndexParamMinus1
                            .Items(J).SubItems(1).Text = CStr(Math.Round(MeasuredValues(J, indexTimeVsRow), Precision))
                        Next J
                    Else
                        For J = 1 To UBound(SnapshotSmallParameters)
                            If SnapshotSmallParameters(J).IsVisible Then
                                .Items(SnapshotSmallParameters(J).NumberInList - 1).SubItems(1).Text = CStr(Math.Round(MeasuredValues(J - 1, indexTimeVsRow), Precision))
                            End If
                        Next J
                    End If
                End With
                ListViewAcquisition.EndUpdate() ' включить обновление элемента

                If IsShowTextControl Then frmTextControl.UpdateValuesTextControls(MeasuredValues, indexTimeVsRow)
                If IsShowGraphControl Then frmGraphControl.UpdateValueIndicatorControls(MeasuredValues, indexTimeVsRow) ' вызов метода формы обновления контроля
                ' отобразить режим
                TextBoxRegime.Text = GetRegimeEngineString(ARUD, N2reduced)
            End If

            countTextDisplayRate += 1

            If isUseWindowsDiagramFromParameter Then UpdateGraphByParameter(coefficientBringingTBoxing)

            If IsMonitorDigitalOutputPort Then
                IsMonitorDigitalOutputPort = False ' на случай, если в подсчете ошибка, если ошибки не будет, то там заново TRUE
                DigitalPortForm.Monitor() ' там вызов ОбработатьНакопление() ' там blnМониторDigitalOutputPort = True
                IsMonitorDigitalOutputPort = DigitalPortForm.TSButtonStart.Checked ' должен быть включен после монитора или сброшен после протокола
            End If

            'If blnПодпискаНаСобытиеСбораВключена AndAlso frmMDITabPanelMotorist IsNot Nothing Then
            '    'frmMDITabPanelMotorist.Refresh() 'всё сдвигается сильно моргает
            '    'frmMDITabPanelMotorist.WindowManagerPanel1.Refresh() 'не помолгло
            '    'frmMDITabPanelMotorist.WindowManagerPanel1.GetActiveWindow.Window.Refresh() 'всё сдвигается изредка моргает
            'End If

            If IsShowFormParametersInRange Then
                frmParameterInRange.txtКаналВДиапвзоне.Text = listParameterInRange
                listParameterInRange = ""
            End If
        End If
        '*---------конец второго Вывода Изображения ------------*

        If isSendDataToClient Then
            counterDataToNetwork = 0
            isSendDataToClient = False
        End If

        If isFireUpdateScreen1 Then
            countUpdateScreen1 = 0
            isFireUpdateScreen1 = False
        End If

        If isFireUpdateScreen2 Then
            isNeedToCountForUpdateScreen2 = False
            countUpdateScreen2 = 0
            isFireUpdateScreen2 = False
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Показать окно опроса одиночного канала
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuQuestioningParameter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuQuestioningParameter.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuQuestioningParameter_Click)}> Показать окно опроса одиночного канала")

        StopRecord()
        InstallQuestionForm(WhoIsExamine.Examination)
        TestChannelForm.ParentFormBase = Me
        TestChannelForm.Show()
    End Sub

End Class

'Public Sub НайтиИндексыКаналовТяги(ByVal эталон As String, ByVal контроль As String, ByRef индексЭталон As Integer, ByRef индексКонтроль As Integer)
'    Dim vKey, N As Integer
'    Dim name As String

'    ' как в процедуре ДанныеОтСервера 
'    For J As Integer = 0 To ЧислоВсехИзмеряемыхПараметров
'        vKey = J + 1 'ищем этот номер
'        N = arrIndexParameters(vKey)
'        name = ParametersType(N).NameParameter
'        If name = эталон Then
'            индексЭталон = vKey - 1
'        End If
'        If name = контроль Then
'            индексКонтроль = vKey - 1
'        End If
'    Next J
'End Sub

'Private Sub mnuТарировкаТяги_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuТарировкаТяги.Click
'    Dim cn As New System.Data.OleDb.OleDbConnection(BuildCnnStr(strProviderJet, strПутьChannels))
'    Dim strТаблицаКонфигурации As String = "ТарировкаТяги"

'    cn.Open()
'    'Здесь проверка существования базы
'    If ПроверкаНаличияТаблицы(cn, strТаблицаКонфигурации) = False Then СоздатьТаблицуТарировкиТяги(cn)
'    cn.Close()

'    frmТарировкаТяги = New frmТяга(Me)
'    frmТарировкаТяги.Show()
'    frmТарировкаТяги.Activate()
'End Sub

'Public Sub ВыдатьИменаПараметровОтСервера(ByRef именаПараметровОтСервера As String())
'    Dim vKey, N As Integer
'    Dim name As String

'    Erase именаПараметровОтСервера

'    ' как в процедуре ДанныеОтСервера 
'    For J As Integer = 0 To ЧислоВсехИзмеряемыхПараметров
'        vKey = J + 1 ' ищем этот номер
'        N = arrIndexParameters(vKey)
'        name = ParametersType(N).NameParameter
'        ReDimPreserve именаПараметровОтСервера(vKey)
'        именаПараметровОтСервера(J) = name
'    Next J

'    ReDimPreserve именаПараметровОтСервера(UBound(именаПараметровОтСервера) - 1)
'End Sub

'Private Sub ЗаполнитьМассивУгловСнимка(ByRef arrПараметрыТекущиеИлиСнимка() As MyType)
'    Dim I As Integer
'    Dim intНачало, J, intКонец As Integer
'    Dim K As Integer
'    Dim N, L As Integer
'    Dim strName As String
'    If Me.ТипИспытания = enumТипИспытания.Регистратор OrElse Me.ТипИспытания = enumТипИспытания.Клиент Then
'        intНачало = 0
'        intКонец = arraysize
'    Else
'        intНачало = Int(XAxisTime.Range.Minimum)
'        intКонец = Int(XAxisTime.Range.Maximum)
'    End If
'    ReDim_arrГрафик(4, intКонец - intНачало + 1)
'    For I = 1 To UBound(arrIndexParameters)
'        N = arrIndexParameters(I) 'N  индекс для массива arrСреднее
'        strName = arrПараметрыТекущиеИлиСнимка(N).strНаименованиеПараметра
'        K = 1
'        Select Case strName
'            Case conN1
'                intN1Index = I - 1
'                L = 1
'                For J = intНачало To intКонец
'                    arrГрафик(L, K) = arrСреднее(I - 1, J)
'                    K = K + 1
'                Next J
'            Case cona1
'                inta1Index = I - 1
'                L = 2
'                For J = intНачало To intКонец
'                    arrГрафик(L, K) = arrСреднее(I - 1, J)
'                    K = K + 1
'                Next J
'            Case conN2
'                intN2Index = I - 1
'                L = 3
'                For J = intНачало To intКонец
'                    arrГрафик(L, K) = arrСреднее(I - 1, J)
'                    K = K + 1
'                Next J
'            Case cona2
'                inta2Index = I - 1
'                L = 4
'                For J = intНачало To intКонец
'                    arrГрафик(L, K) = arrСреднее(I - 1, J)
'                    K = K + 1
'                Next J
'        End Select
'    Next I
'End Sub

'Private Sub ОчиститьМассивДинамики()
'    Dim I, J As Integer
'    'очистить массив
'    For I = 1 To 5
'        For J = 1 To intДинамика
'            arrДинамика(I, J) = 0
'        Next J
'    Next I
'End Sub
