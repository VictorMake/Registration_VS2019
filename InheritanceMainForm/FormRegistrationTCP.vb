Imports System.Data.OleDb
Imports System.Threading
Imports MathematicalLibrary

Friend Class FormRegistrationTCP
    ''' <summary>
    ''' Счетчик цикла для передачи данных клиентам
    ''' </summary>
    Private counterDataToNetwork As Integer
    ''' <summary>
    ''' Накопленные данные передать по сети клиенту
    ''' </summary>
    Private isSendDataToClient As Boolean
    Private frmTcpClient As FormConfigChannelServer

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, "FormRegistrationTCP")
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
        MenuQuestioningParameter.Enabled = False
        ButtonSnapshot.Visible = False
        MenuModeOfOperation.Enabled = False
        MenuServerEnable.Enabled = False
        LoadClientServerForm()

        StopSocket()
        AddHandler MainMDIFormParent.EventMainAcquiredDataForChannelsForm, AddressOf MainMDIFormReference_EventMainAcquiredDataForChannelsForm
        CheckOnModificationNameChannel()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
        StopSocket()
        RemoveHandler MainMDIFormParent.EventMainAcquiredDataForChannelsForm, AddressOf MainMDIFormReference_EventMainAcquiredDataForChannelsForm
    End Sub
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosed()
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
        'CheckIsSocketCreate()
        If ButtonContinuously.Checked Then ButtonContinuously.Checked = False

        AutomaticParsingRecording("SELECT COUNT(*) FROM [БазаСнимков]")
        frmTcpClient = New FormConfigChannelServer(Me)
        frmTcpClient.ShowDialog()
    End Sub
    ''' <summary>
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub SettingsOptionProgram()
        'MyBase.SettingsOptionProgram()
        Dim isNeedRestart As Boolean = IsFormRunning ' Надо презапустить
        If ButtonContinuously.Checked Then ButtonContinuously.Checked = False

        InstallQuestionForm(WhoIsExamine.Setting)
        ButtonContinuously.Checked = False
        SettingForm.ShowDialog() ' обязательно модально

        If isNeedRestart Then CharacteristicForRegime() ' там запуск

        StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyBackground) & "Гц"
    End Sub
    ''' <summary>
    ''' Запуск-Останов связи с Сервером при помощи сетевого подключения по сокету TCP
    ''' </summary>
    ''' <param name="start"></param>
    Protected Overrides Sub StartStopSocket(start As Boolean)
        If start Then StartSocket() Else StopSocket()
    End Sub
    ''' <summary>
    ''' Нет реализации
    ''' </summary>
    Protected Overrides Sub InitializeContinuousDAQmxTask()
    End Sub

    ''' <summary>
    ''' Перестройка числа параметров для выбранного режима
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub CharacteristicForRegime()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(CharacteristicForRegime)}> Настройка параметров сбора для сконфигурированного режима")

        Dim I, J As Integer
        Dim sngLeft, sngHeight, sngWidth As Integer
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
        ' т.к. массив структуры arrПараметры при работе с контроллером в процессе испытаний не менняется, а с ТСРКлиент меняется, необходимо загрузить
        LoadChannels()
        UnpackStringConfigurationWithEmpty(ConfigurationString)
        'ReDim_IndexParameters(0)
        Re.Dim(IndexParameters, 0)
        ' выгрузить индикаторы аварийных значений
        UnloadAlarmButton()
        sngLeft = AlarmPulseButton(keyFirstButtonAlarm).Left
        sngWidth = AlarmPulseButton(keyFirstButtonAlarm).Width
        sngHeight = AlarmPulseButton(keyFirstButtonAlarm).Height
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
                        AlarmPulseButton(J).SetBounds(sngLeft,
                                                             AlarmPulseButton(keyLastButtonAlarm).Top + AlarmPulseButton(keyLastButtonAlarm).Height + 1,
                                                             sngWidth,
                                                             sngHeight)
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
        ShowFormTuningList()
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
    ''' Обновление таблицы Режимы при проверке подключаемых расчётных модулей при работе TcpClient
    ''' </summary>
    Protected Overrides Sub UpdateTableRegimeForTcpClient()
        Thread.CurrentThread.Priority = ThreadPriority.BelowNormal

        Dim configurationString As String = Nothing
        For J As Integer = 1 To UBound(ParametersType)
            If configurationString = vbNullString Then
                configurationString = ParametersType(J).NameParameter
            Else
                configurationString &= "\" & ParametersType(J).NameParameter
            End If
        Next
        configurationString &= "\"

        'If fMainForm.varМодульРасчетаManager.ДобавкаКонфигурацииТсрКлиента IsNot Nothing Then
        '    sСтрокаКонфигурации = sСтрокаКонфигурации & fMainForm.varМодульРасчетаManager.ДобавкаКонфигурацииТсрКлиента
        'End If

        Dim strSQL As String = $"Update Режимы{StandNumber} SET ПереченьПараметров = '{configurationString}' WHERE ([НомерРежима]= 1)"
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            Dim cmd As OleDbCommand = cn.CreateCommand

            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cn.Open()
            cmd.ExecuteNonQuery()
        End Using

        RegistrationEventLog.EventLog_MSG_DB_UPDATE($"Обновление таблицы Режимы при загрузке {Text}")
        'uiTaskScheduler = Tasks.TaskScheduler.FromCurrentSynchronizationContext
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' Обновить компенсацию ХС для регистратора SCXI
    ''' </summary>
    Protected Overrides Sub UpdateCompensationXC()
        ' ничего
    End Sub
    ''' <summary>
    ''' Обновление данных на экране из события сбора
    ''' </summary>
    Friend Overrides Sub AcquiredData()
        'Public Sub TCP_AcquiredData(ByRef arrDataTCP() As Double)
        Dim J, N, IndexPlot As Integer
        Dim average As Double ' осреднение
        Dim name As String
        Dim vKey As Integer
        Dim indexDiscret As Integer ' индекс Цифровые
        Dim isHeightLevel As Boolean
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

        xNewPos = Math.Round((lastx + 1), 2) Mod (arraysize + 1) 'от 0 до 1800 в цикле запись в массив среднего, код такой-же как SweepChart
        ' в SweepChart производится lastx = xNewPos (lastx постоянно растёт от 0 до 1800 )
        indexTimeVsRow = CInt(lastx)

        ''ТЕСТ ни чего не обнаружил
        'Dim summTest As Double
        'For I = 0 To UBound(arrДанныеСервераЗначение)
        '    summTest += arrДанныеСервераЗначение(I)
        'Next
        'If summTest = 0 Then
        '    Stop
        'End If

        For J = 0 To CountMeasurand
            vKey = J + 1 ' ищем этот номер

            If J <= startAddedChannels Then
                average = DataValuesFromServer(J)
            End If

            N = IndexParameters(vKey)

            If IsShowFormParametersInRange AndAlso isFireUpdateScreen1 Then
                If average >= MinLimitParameter AndAlso average <= MaxLimitParameter Then
                    listParameterInRange &= $"{ParametersType(N).NameParameter}  № модуля:{ParametersType(N).NumberModuleChassis}  № канала:{ParametersType(N).NumberChannelModule}{vbCrLf}"
                End If
            End If

            If J > startAddedChannels Then
                ' одновременно расчетный и дискретный вход измерять нельзя
                If IsUseCalculationModule AndAlso J > (CountMeasurand - CountSolveParameters) Then
                    ' уже цифры занесены с предыдущего расчета, поэтому отставание на время вывести изображение
                    average = MainMDIFormParent.ModuleSolveManager.ValueCalculationParameters(ParametersType(N).NumberChannel)
                    ' на самом деле индекс этого массива arrПараметры(N).intНомерКанала = ЦиклРасчетныйПараметр.ИндексКаналаИзмерения
                ElseIf IsDigitalInput Then
                    average = DigitalInputValue(ParametersType(N).NumberChannel) ' на самом деле индекс этого массива
                End If
            Else
                '*---------Вычисление физич. значений ------------*
                ' перенесена из Модуль1()
                ' Function ФизическиеЗначения(v As Integer, ByVal w As Double) As Double
                ' v- номер параметра
                ' w- значение аргумента - напряжения или кода
                ' fy = 0
                Select Case ParametersType(N).NumberFormula
                    Case 3
                        If average <= 1 Then average = 0 Else average = 5
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
                    Case NameVarStopWrite
                        If IsQuestioningRevolution = False AndAlso average < ValueStopWrite AndAlso TimerButtonRun.Enabled = False Then 'N1<1% ->  выключить запись и задержка кнопки Запуск>10 секунд
                            If Not isButtonRecordPressHandle Then
                                IsQuestioningRevolution = True
                                TimerSwitchOffRecord.Interval = WaitStopWrite '40000 '40 секунд ждать до остановки
                                TimerSwitchOffRecord.Enabled = True
                            End If
                        End If
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

            ' накопление для монирота Поля и накопление для подсчета К.Т.
            If IsSubscriptionOnEventsAcquisitionEnabled OrElse IsUseCalculationModule OrElse (IsMonitorDigitalOutputPort AndAlso IsFrmDigitalOutputPortStart) Then
                ' накапливать нельзя т.к. blnМониторDigitalOutputPort работает асинхронно
                ParameterAccumulate(N) = average
            End If

            ' запись в массив среднего, код такой-же как SweepChart
            MeasuredValues(J, indexTimeVsRow) = average

            If Not IsUseTdms Then PackOfParametersToRecord(J) = CStr(Math.Round(average, Precision))

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

        If Not IsUseTdms Then
            strData = Join(PackOfParametersToRecord, vbTab)
            dataMeasuredValuesString.Append(strData & vbCrLf)
        End If

        countRow += 1
        SweepChart()

        If isShowDiagramFromParameter Then OutParametersGraph()

        '*---------начало второго Вывода Изображения ------------*
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

                If IsShowGraphControl Then frmGraphControl.UpdateValueIndicatorControls(MeasuredValues, indexTimeVsRow) ' вызов метода формы обновления контроля
                If IsShowTextControl Then frmTextControl.UpdateValuesTextControls(MeasuredValues, indexTimeVsRow)
            End If

            countTextDisplayRate += 1

            If IsShowFormParametersInRange Then
                frmParameterInRange.txtКаналВДиапвзоне.Text = listParameterInRange
                listParameterInRange = ""
            End If

            If isUseWindowsDiagramFromParameter Then UpdateGraphByParameter(coefficientBringingTBoxing)

            If IsMonitorDigitalOutputPort Then
                IsMonitorDigitalOutputPort = False ' на случай если в подсчете ошибка, если ошибки не будет, то там заново TRUE
                DigitalPortForm.Monitor() ' там вызов ОбработатьНакопление() ' там blnМониторDigitalOutputPort = True
                IsMonitorDigitalOutputPort = DigitalPortForm.TSButtonStart.Checked ' должен быть включен после монитора или сброшен после протокола
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
    ''' Обновление данных на экране из события сбора клиента (разнесено по времени с обработкой аварийных уровненй)
    ''' вызывается из события клиента к Серверу _ConnectionClient.SyncSocketClientAcquiredData
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">массив собранных клиентом данных</param>
    ''' <remarks></remarks>
    Private Sub MainMDIFormReference_EventMainAcquiredDataForChannelsForm(sender As Object, e As FormMainMDI.AcquiredDataEventArgs) ' Handles MainMDIFormParentReference.EventMainAcquiredDataForChannelsForm
        DataValuesFromServer = e.ParametersAcquiredData
        AcquiredData()
    End Sub

    ''' <summary>
    ''' Запуск связи с Сервером при помощи сетевого подключения по сокету TCP
    ''' </summary>
    Private Sub StartSocket()
        ' 1.сформировать  m_OutputChannelsBindingList(используется в mSyncSocketClient для формирования списка каналов опроса) на основании записанной конфигурации каналов
        ' 2.т.к. используются экранные имена clsChannelOutput.НаименованиеПараметра (не исключён повтор имён) то сделать поиск clsChannelOutput.Name
        ' 3.arrПараметры - arrНаименование2 - arrIndexParameters(номера)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StartSocket)}> Попытка установки связи c Сервером")

        LabelMessageTcpClient.Text = "Запуск сбора расчётных значений"

        ' не объединять два условия
        If MainMDIFormParent.ConnectionClient Is Nothing Then
            MainMDIFormParent.LoadMainFormAgain()
        End If
        If MainMDIFormParent.ConnectionClient IsNot Nothing Then
            MainMDIFormParent.ConnectionClient.Initialize()
            MainMDIFormParent.ConnectionClient.StartAcquisitionTimer() ' идёт последним - там настройка размерностей
        End If
    End Sub

    ''' <summary>
    ''' Останов связи с Сервером при помощи сетевого подключения по сокету TCP
    ''' </summary>
    Private Sub StopSocket()
        If MainMDIFormParent.ConnectionClient IsNot Nothing Then
            If MainMDIFormParent.ConnectionClient.IsStartAcquisition Then
                ' вначале остановить таймер формы
                MainMDIFormParent.ConnectionClient.StopAcquisition()
                Thread.Sleep(50)
            End If
        End If

        LabelMessageTcpClient.Text = "Остановка сбора"
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StopSocket)}> Остановка сбора от Сервером")
    End Sub
End Class

'#Region "ПробаЗаменыТаймера"

'Private myTaskСокета As Task
'Private taskRunningСокета As Boolean
'Private analogInReaderСокета As AnalogMultiChannelReader
'Private MultiAnalogCallbackСокета As AsyncCallback
'Private dataWaveform As AnalogWaveform(Of Double)()
'Private CountMultiAnalogInCallback As Integer

'Private Sub ИнициализацияССобытиемДляСокета()
'    Dim terminalConfigurationСокета As AITerminalConfiguration
'    Dim inputCouplingСокета As AICoupling
'    Dim НомерУстройства As String = "1"
'    Dim НомерКанала As String = "0"
'    CountMultiAnalogInCallback = 0

'    If taskRunningСокета = False Then
'        Try
'            taskRunningСокета = True
'            'Create a new task
'            If Not myTaskСокета Is Nothing Then
'                'myTaskСокета.Stop()
'                myTaskСокета.Dispose()
'                myTaskСокета = Nothing
'            End If
'            myTaskСокета = New Task("aiTask")

'            Try
'                Dim aiChannel As AIChannel
'                terminalConfigurationСокета = AITerminalConfiguration.Differential
'                inputCouplingСокета = AICoupling.DC
'                strСтрока = "Dev" & НомерУстройства & "/ai" & НомерКанала
'                aiChannel = myTaskСокета.AIChannels.CreateVoltageChannel(strСтрока, "", terminalConfigurationСокета, _
'                    -10, 10, AIVoltageUnits.Volts)
'                aiChannel.Coupling = inputCouplingСокета
'            Catch ex As DaqException
'                Const caption As String ="CreateAiChannel"
'                Dim text As String = exception.Message
'                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
'                myTaskСокета.Dispose()
'            End Try

'            'фактическая частота сбора
'            myTaskСокета.Timing.ConfigureSampleClock("", intЧастотаФонового, _
'                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1)

'            'Verify the Task
'            myTaskСокета.Control(TaskAction.Verify)

'            analogInReaderСокета = New AnalogMultiChannelReader(myTaskСокета.Stream)
'            analogInReaderСокета.SynchronizeCallbacks = True
'            MultiAnalogCallbackСокета = New AsyncCallback(AddressOf MultiAnalogInCallbackСокета) 'MultiAnalogInCallback - процедура, MultiAnalogCallback - делегат
'            'analogInReaderСокета.BeginReadMultiSample(1, MultiAnalogCallbackСокета, Nothing)
'            'analogInReaderСокета.BeginReadMultiSample(1, MultiAnalogCallbackСокета, myTaskСокета)
'            analogInReaderСокета.BeginReadWaveform(1, MultiAnalogCallbackСокета, myTaskСокета)
'        Catch ex As DaqException
'            Const caption As String ="Инициализация с событием для сокета"
'            taskRunningСокета = False
'            myTaskСокета.Dispose()
'            MessageBox.Show(exception.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {exception.Message}")
'        End Try
'    End If
'End Sub

'Private Sub ОстановитьСборСокета()
'    Try
'        If taskRunningСокета = True Then
'            taskRunningСокета = False
'            If Not myTaskСокета Is Nothing Then
'                myTaskСокета.Stop()
'                myTaskСокета.Dispose()
'                myTaskСокета = Nothing
'            End If
'        End If
'    Catch ex As Exception
'        Const caption As String ="Остановка сбора для сокета"
'        Dim text As String = ex.ToString
'        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
'        'Finally
'    End Try
'End Sub

'Private Sub MultiAnalogInCallbackСокета(ByVal ar As IAsyncResult)
'    Try
'        If taskRunningСокета = True Then
'            'data = analogInReaderСокета.EndReadMultiSample(ar)
'            dataWaveform = analogInReaderСокета.EndReadWaveform(ar)
'            'frmРегистратор.CWAI1_AcquiredData()
'            'analogInReaderСокета.BeginReadMultiSample(1, MultiAnalogCallbackСокета, Nothing)

'            CountMultiAnalogInCallback += 1
'            'Dim Value As Double
'            'For Each waveform As AnalogWaveform(Of Double) In dataWaveform
'            '    Dim dataCount As Integer = 0
'            '    dataCount = waveform.Samples.Count

'            '    For sample As Integer = 0 To (dataCount - 1)
'            '        Value = waveform.Samples(sample).Value
'            '    Next
'            'Next

'            mSyncSocketClient._OnTimedEvent()

'            'arrДанныеСервераЗначение(0) = CountMultiAnalogInCallback 'для теста
'            'TCP_AcquiredData()
'            'TCP_AcquiredData(mSyncSocketClient.AcquisitionDoubleValue)
'            analogInReaderСокета.BeginMemoryOptimizedReadWaveform(1, MultiAnalogCallbackСокета, myTaskСокета, dataWaveform)
'        End If
'    Catch ex As DaqException
'        Const caption As String ="MultiAnalogInCallbackСокета"
'        Dim text As String = ex.ToString
'        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
'        taskRunningСокета = False
'        If Not myTaskСокета Is Nothing Then
'            myTaskСокета.Stop()
'            myTaskСокета.Dispose()
'            myTaskСокета = Nothing
'        End If
'    End Try
'End Sub
'#End Region

'''' <summary>
'''' Вызывается при смене номера стенда из списка
'''' </summary>
'''' <remarks></remarks>
'Private Function НастроитьВЗависимостиОтСтендаИКонфигурации() As Boolean
'    RegistrationEventLog.EventLog_MSG_USER_ACTION("<" & "НастроитьВЗависимостиОтСтендаИКонфигурации" & "> " & "Считывание последней конфигурации")
'    Dim success As Boolean = False

'    ' ВключитьВыключить(False)
'    If CheckExistServerCfglmzXml(pathServerCfglmzXml) Then
'        Dim channelConfigTableAdapter As Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter = Nothing
'        Dim channelsDataSet As Channels_cfg_lmzDataSet

'        Try
'            Dim xdoc As XDocument = XDocument.Load(pathServerCfglmzXml)
'            HostName = xdoc.<Cell>.<Config>.<Net>.<Server>.@HostIP
'            PortTCP = xdoc.<Cell>.<Config>.<Net>.<Server>.@TcpPort

'            channelConfigTableAdapter = New Registration.Channels_cfg_lmzDataSetTableAdapters.ChannelConfigTableAdapter()
'            channelConfigTableAdapter.ClearBeforeFill = True ' взят с дизайнера

'            channelsDataSet = New Channels_cfg_lmzDataSet()
'            channelsDataSet.DataSetName = "Channels_cfg_lmzDataSet"
'            channelsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema ' взят с дизайнера

'            ' строка подключения сделал сам т.к. в дизайнере ссылка на строку созданную при создании дизайнера
'            channelConfigTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels_cfg_lmz))

'            channelConfigTableAdapter.FillBykeyConfig(channelsDataSet.ChannelConfig, keyConfig)
'            gOutputChannelsBindingList.Clear()
'            m_TimeChannelsBindingList.Clear()

'            If UBound(arrIndexParameters) - AdditionalParameterCount <> channelsDataSet.ChannelConfig.Rows.Count Then
'                'If arrIndexParameters.Length - ЧислоДополнительныхПараметров <> ChannelsDataSet.ChannelConfig.Rows.Count Then
'                Const caption As String = "Загрузка выбранной конфигурации"
'                Dim text As String = "Нарушено соответствие списка каналов в базе данных выбранного стенда " &
'                                "и списка выбранных для опроса каналов в последней рабочей конфигурации!" & vbCrLf &
'                                "Повторно произведите составление списка каналов по пункту меню:" & vbCrLf &
'                                "Настроить -> Конфигурация режимов."
'                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
'                tbНепр.Checked = False

'                Return False
'            End If

'            Dim списокОтсутствующихКаналов As New List(Of String)
'            For Each rowChannel As Channels_cfg_lmzDataSet.ChannelConfigRow In channelsDataSet.ChannelConfig.Rows
'                If Not arrНаименование2.Contains(rowChannel.НаименованиеПараметра) Then
'                    'MessageBox.Show("Канала с именем <" & rowChannel.НаименованиеПараметра & "> не существует в данной конфигурации!" & vbCrLf & "Обновите список каналов в конфигурации," & vbCrLf & "или измените экранное имя канала, чтобы его длина не превышала 50 символов.", "Загрузка выбранной конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'                    списокОтсутствующихКаналов.Add(rowChannel.НаименованиеПараметра)
'                End If

'                Dim clsChannelOutput As New ChannelOutput() With {.id = rowChannel.id,
'                                                                .keyConfig = rowChannel.keyConfig,
'                                                                .НомерПараметра = rowChannel.НомерПараметра,
'                                                                .Name = rowChannel.Name,
'                                                                .НаименованиеПараметра = rowChannel.НаименованиеПараметра}

'                If Not IsDBNull(rowChannel("ЕдиницаИзмерения")) Then clsChannelOutput.ЕдиницаИзмерения = rowChannel.ЕдиницаИзмерения
'                'If clsChannelOutput.Pin IsNot Nothing Then clsChannelOutput.Pin = rowChannel.Pin

'                clsChannelOutput.ДопускМинимум = rowChannel.ДопускМинимум
'                clsChannelOutput.ДопускМаксимум = rowChannel.ДопускМаксимум
'                clsChannelOutput.РазносУмин = rowChannel.РазносУмин
'                clsChannelOutput.РазносУмакс = rowChannel.РазносУмакс
'                clsChannelOutput.АварийноеЗначениеМин = rowChannel.АварийноеЗначениеМин
'                clsChannelOutput.АварийноеЗначениеМакс = rowChannel.АварийноеЗначениеМакс
'                clsChannelOutput.GroupCh = rowChannel.GroupCh

'                gOutputChannelsBindingList.Add(clsChannelOutput)
'                'End If
'                'Else
'                '    MessageBox.Show("Канала с именем <" & rowChannel.НаименованиеПараметра & "> не существует в данной конфигурации!" & vbCrLf & "Обновите список каналов в конфигурации," & vbCrLf & "или измените экранное имя канала, чтобы его длина не превышала 50 символов.", "Загрузка выбранной конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
'                'End If
'            Next

'            channelConfigTableAdapter.Connection.Close()

'            If списокОтсутствующихКаналов.Count > 0 Then
'                Dim message As String = Nothing
'                Dim I As Integer

'                For Each channelName As String In списокОтсутствующихКаналов
'                    message += channelName & vbCrLf
'                    I += 1
'                    If I > 10 Then
'                        message += "и еще более..." & vbCrLf
'                        Exit For
'                    End If
'                Next

'                Const caption As String = "Загрузка выбранной конфигурации"
'                Dim text As String = "Следующие каналы:" & vbCrLf & message &
'                                "не существуют в данной конфигурации или были переименованы!" & vbCrLf &
'                                "1) Обновите список каналов в конфигурации, или" & vbCrLf &
'                                "2) измените экранное имя канала, чтобы его длина не превышала 50 символов."
'                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
'            End If
'        Catch ex As Exception
'            Const caption As String = "Настройка каналов сокета в зависимости от стенда и конфигурации"
'            Dim text As String = ex.ToString
'            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
'        Finally
'            If channelConfigTableAdapter.Connection.State = ConnectionState.Open Then
'                channelConfigTableAdapter.Connection.Close()
'            End If
'        End Try

'        success = СоздатьСокетсСервером()

'        'Else
'        'StartTSButton.Enabled = False
'        'GroupBoxStend.Enabled = True
'        'GroupBoxСтендКонфигСервер.Enabled = True
'    End If

'    Return success
'End Function

'Private Sub TSButtonConnectRefresh_Click(sender As Object, e As EventArgs) Handles TSButtonConnectRefresh.Click
'    СоздатьСокетсСервером()
'End Sub

'Private Function СоздатьСокетсСервером() As Boolean
'    Dim success As Boolean = False

'    If CheckPingClient(HostName) = True Then ' True - успех
'        CheckIsSocketCreate()
'        mSyncSocketClient = New ConnectionInfoClientOld(Me)
'        mSyncSocketClient.Connect()

'        ' скорее всего загрузка прошла нормально
'        'StartTSButton.Enabled = mSyncSocketClient.СоединениеУстановлено
'        tbНепр.Checked = mSyncSocketClient.СоединениеУстановлено
'        CheckConnection(mSyncSocketClient.СоединениеУстановлено)
'        success = mSyncSocketClient.СоединениеУстановлено
'        'If success Then
'        '    AddHandler mSyncSocketClient.SyncSocketClientAcquiredData, AddressOf SyncSocketClientAcquiredData
'        'End If
'    End If

'    Return success
'End Function

'#Region "Функция обратного вызова"

'Delegate Sub AddListItem(message As String)
'Public myDelegate As AddListItem

'''' <summary>
'''' функция обратного вызова из SyncSocketClient для отображения сообщений
'''' </summary>
'''' <param name="message"></param>
'''' <remarks></remarks>
'Public Sub AddListItemMethod(message As String)
'    'Dim I As Integer

'    'If mSyncSocketClient.AcquisitionDoubleValue IsNot Nothing Then
'    '    For I = 0 To mSyncSocketClient.AcquisitionDoubleValue.Length - 1
'    '        gDoubleValueList(I).ValueCh = mSyncSocketClient.AcquisitionDoubleValue(I)
'    '    Next
'    'End If

'    'ListBoxConsole.Items.Add(message)
'    TSLabelMessageTcpClient.Text = message
'    CheckConnection(mSyncSocketClient.СоединениеУстановлено)
'    RegistrationEventLog.EventLog_MSG_CONNECT(message)
'End Sub

'Public Sub ThreadFunction(ByVal message As Object)
'    If mSyncSocketClient IsNot Nothing Then ' событие может вызываться от таймера из другого потока, а основной поток уже завершен
'        Dim myThreadClassObject As New MyThreadClass(Me, message) ' "Тест")
'        myThreadClassObject.Run()
'    End If
'End Sub


'Public Class MyThreadClass
'    Private myFormControl1 As FormMain

'    Public Sub New(myForm As FormMain, message As String)
'        myFormControl1 = myForm
'        Me.message = message
'    End Sub

'    Private message As String

'    Public Sub Run()
'        If myFormControl1.IsHandleCreated Then
'            'Dim i As Integer
'            'For i = 1 To 5
'            '    message = "Step number " + i.ToString() + " executed"
'            '    Thread.Sleep(400)
'            '    ' выполнить специальный делегатв потоке родителя
'            '    ' 'myFormControl1' control's содержащий указатель окна
'            '    ' с листом аргументов.
'            myFormControl1.Invoke(myFormControl1.myDelegate, New Object() {message})
'            'Next i
'        End If
'    End Sub
'End Class

'***************************
'Delegate Sub AcquisitionTCP(AcquisitionDouble() As Double)
'Public myDelegateAcquisitionTCP As AcquisitionTCP

' ''' <summary>
' ''' функция обратного вызова из SyncSocketClient для отображения полученных данных
' ''' </summary>
' ''' <param name="AcquisitionDouble"></param>
' ''' <remarks></remarks>
'Public Sub CallBackAcquisitionTCP(AcquisitionDouble() As Double)
'    TCP_AcquiredData()
'End Sub

'Public Sub ThreadFunctionAcquisition(AcquisitionDouble() As Double) '(ByVal message As Object)
'    If mSyncSocketClient IsNot Nothing Then ' событие может вызываться от таймера из другого потока, а основной поток уже завершен
'        Dim myThreadClassObject As New MyThreadClassAcquisitionTCP(Me, AcquisitionDouble) ' "Тест")
'        myThreadClassObject.Run()
'    End If
'End Sub

'Public Class MyThreadClassAcquisitionTCP
'    Private myFormControl1 As frmMain
'    Private AcquisitionDouble() As Double

'    Public Sub New(myForm As frmMain, AcquisitionDouble() As Double)
'        myFormControl1 = myForm
'        Me.AcquisitionDouble = AcquisitionDouble
'    End Sub

'    Public Sub Run()
'        myFormControl1.Invoke(myFormControl1.myDelegateAcquisitionTCP, New Object() {AcquisitionDouble})
'    End Sub
'End Class


'Imports System
'  Imports System.Threading

'  Namespace InterlockedExchange_Example
'        Class MyInterlockedExchangeExampleClass
'            '0 для false, 1 для true.
'            Private Shared usingResource As Integer = 0

'            Private Const numThreadIterations As Integer = 5
'            Private Const numThreads As Integer = 10

'            <MTAThread()> _
'            Shared Sub Main()
'                Dim myThread As Thread
'                Dim rnd As New Random()

'                Dim i As Integer
'                For i = 0 To numThreads - 1
'                    myThread = New Thread(AddressOf MyThreadProc)
'                    myThread.Name = String.Format("Thread{0}", i + 1)

'                    ' подождать случайное время прежде запуска следующего потока.
'                    Thread.Sleep(rnd.Next(0, 1000))
'                    myThread.Start()
'                Next i
'            End Sub 'Main

'            Private Shared Sub MyThreadProc()
'                Dim i As Integer
'                For i = 0 To numThreadIterations - 1
'                    UseResource()

'                    ' подождать 1 секунду перед следующей попыткой.
'                    Thread.Sleep(1000)
'                Next i
'            End Sub

'            ' простая блокировка повторного вызова.
'            Shared Function UseResource() As Boolean
'                '0 показывает что метод ещё не используется.
'                If 0 = Interlocked.Exchange(usingResource, 1) Then
'                    Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name)

'                    ' код доступа к ресурсу  is not thread safe would go here.
'                    ' симулировать некоторую работу
'                    Thread.Sleep(500)

'                    Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name)

'                    ' освободить блокировку
'                    Interlocked.Exchange(usingResource, 0)
'                    Return True
'                Else
'                    Console.WriteLine("   {0} was denied the lock", Thread.CurrentThread.Name)
'                    Return False
'                End If
'            End Function
'        End Class
'    End Namespace

'Private syncPoint As Integer = 0 ' для синхронизации
'' работа с 2-мя таймерами позволяет сделать приложение отзывчивым, но общие массивы блокируются
'Private Sub mmTimerMain_Tick(sender As Object, e As System.EventArgs) Handles mmTimerMain.Tick
'    Dim sync As Integer = Interlocked.CompareExchange(syncPoint, 1, 0)
'    If sync = 0 Then
'        If mSyncSocketClient.IsStartAcquisition Then
'            TCP_AcquiredData()
'            'StopTimeCount2 += 1
'        Else
'            mmTimerMain.Stop()
'        End If
'        syncPoint = 0 'освободить
'        'Else
'        '    Console.WriteLine("   {0} was denied the lock", Thread.CurrentThread.Name)
'    End If
'End Sub

' в качестве альтернативы использовать события
'Private Sub SyncSocketClientAcquiredData(sender As Object, e As SyncSocketClient.AcquiredDataEventArgs)
'    TCP_AcquiredData()
'End Sub

''''Private messagesLock As New Object
'Private Sub mSyncSocketClient_SyncSocketClientAcquiredData(sender As Object, e As ConnectionInfoClientOld.AcquiredDataEventArgs) Handles mSyncSocketClient.SyncSocketClientAcquiredData
'    'arrДанныеСервераЗначение = e.arrDataTCP
'    ' на самом деле e.arrDataTCP это arrДанныеСервераЗначение
'    'SyncLock messagesLock

'    TCP_AcquiredData(e.arrDataTCP)

'    'e.arrDataTCP.CopyTo(arrДанныеСервераЗначение, 0)
'    'TCP_AcquiredData(arrДанныеСервераЗначение)

'    'Invoke пропускает
'    'Me.Invoke(Sub() TCP_AcquiredData(), New Object() {e.arrDataTCP})
'    'Me.Invoke(Sub() TCP_AcquiredData(e.arrDataTCP)) ', New Object() {arrДанныеСервераЗначение})
'    'Application.DoEvents()
'    'End SyncLock
'End Sub



'''' <summary>
'''' Обновление данных на экране из события сбора клиента (разнесено по времени с обработкой аварийных уровненй)
'''' вызывается из события клиента к Серверу _ConnectionClient.SyncSocketClientAcquiredData
'''' </summary>
'''' <param name="sender"></param>
'''' <param name="e">массив собранных клиентом данных</param>
'''' <remarks></remarks>
'Private Sub MainMDIFormParentReference_EventMainAcquiredDataForChannelsForm(sender As Object, e As FormMainMDI.AcquiredDataEventArgs) Handles MainMDIFormParentReference.EventMainAcquiredDataForChannelsForm
'    TCP_AcquiredData(e.arrПарамНакопленные)


'    'If формаЗагружена = False Then Exit Sub

'    'Dim arrDataTCP As Double() = e.arrПарамНакопленные ' можно вызвать и другие свойства
'    'Dim J, N, indexPlot As Integer
'    'Dim среднее As Double
'    'Dim indexParameter As Integer
'    'Dim countAlarmLed As Integer
'    'Dim intUBoundСписокПараметровМинус1 As Integer = UBound(garrIndexParameters) - 1

'    'countОбновлениеЭкрана += 1

'    'If countОбновлениеЭкрана >= gОбновлениеЭкрана Then
'    '    вывестиИзображения = True
'    '    считатьОбнЭкрана2 = True
'    'End If

'    'If считатьОбнЭкрана2 Then countОбновлениеЭкрана2 += 1

'    'If countОбновлениеЭкрана2 >= gОбновлениеЭкрана2 Then
'    '    вывестиИзображения2 = True
'    'End If

'    'xNewPos = Math.Round((lastx + 1), 2) Mod (arraysize + 1) ' от 0 до 1800 в цикле запись в массив среднего, код такой-же как SweepChart
'    '' в SweepChart производится lastx = xNewPos (lastx постоянно растёт от 0 до 1800 )
'    'indexTimeVsRow = lastx

'    'For J = 0 To ЧислоВсехИзмеряемыхПараметров
'    '    indexParameter = J + 1 ' ищем этот номер

'    '    ' обычный сбор
'    '    ' отличается от других модулей событтий сбора тем начало массива что не с 1 с с 0
'    '    среднее = arrDataTCP(J) ' mSyncSocketClient.AcquisitionDoubleValue(J)
'    '    N = garrIndexParameters(indexParameter)

'    '    '--- начало Вывода Изображения ---------------------------------
'    '    If вывестиИзображения AndAlso xNewPos <> 0 Then
'    '        If countAlarmLed < ЧИСЛО_ЦИФРОВЫХ_ИНДИКАТОРОВ Then
'    '            countAlarmLed += 1
'    '            CheckValueInRange(listAlarmButton(countAlarmLed - 1), gArrПараметры(N), среднее)
'    '        End If
'    '    End If
'    '    '--- конец Вывода Изображения ---------------------------------

'    '    ' запись в массив среднего, код такой-же как SweepChart
'    '    arrСреднее(J, indexTimeVsRow) = среднее

'    '    If подробныйЛист Then
'    '        If gArrПараметры(garrIndexParameters(indexParameter)).ВидимостьРегистратор Then
'    '            среднее = ПриведениеКОсиЭталона(indexПараметраОсиЭталона, indexParameter, среднее) ' массив приведен к какой-то шкале
'    '            arrСреднееПересчитанный(J, 0) = среднее

'    '            If indexPlot <= числоВидимыхШлейфовРегистратора Then
'    '                dataГрафик(indexPlot, indexTimeVsRow) = среднее
'    '            End If
'    '            indexPlot += 1
'    '        End If
'    '    Else
'    '        If inTypeSmallParameter(indexParameter).Видимость Then
'    '            среднее = ПриведениеКОсиЭталона(indexПараметраОсиЭталона, indexParameter, среднее) ' массив приведен к какой-то шкале
'    '            arrСреднееПересчитанный(J, 0) = среднее ' массив приведен к какой-то шкале

'    '            If indexPlot <= числоВидимыхШлейфовРегистратора Then
'    '                dataГрафик(indexPlot, indexTimeVsRow) = среднее
'    '            End If

'    '            indexPlot += 1
'    '        End If
'    '    End If

'    '    среднее = 0
'    'Next J ' по измеренным каналам

'    'SweepChart()

'    ''*---------начало второго Вывода Изображения ------------*
'    '' вызов frmMainAcquiredData происходит в следующем цикле сбора - это попытка разнести вызовы обновлений экрана

'    'If вывестиИзображения2 AndAlso xNewPos <> 0 Then
'    '    If блокироватьListView = False Then
'    '        With ListViewChannels ' обновить лист
'    '            .BeginUpdate()

'    '            If подробныйЛист Then
'    '                For J = 0 To intUBoundСписокПараметровМинус1
'    '                    .Items(J).SubItems(1).Text = CStr(Math.Round(arrСреднее(J, indexTimeVsRow), Precision))
'    '                Next J
'    '            Else
'    '                For J = 1 To UBound(inTypeSmallParameter)
'    '                    If inTypeSmallParameter(J).Видимость Then
'    '                        .Items(inTypeSmallParameter(J).НомерВЛисте - 1).SubItems(1).Text = CStr(Math.Round(arrСреднее(J - 1, indexTimeVsRow), Precision))
'    '                    End If
'    '                Next J
'    '            End If

'    '            .EndUpdate()
'    '        End With
'    '    End If

'    '    'Application.DoEvents() '(нельзя т.к. она тормозит текущий поток в ижидании других) вставил только в эту процедуру обработки сбора чтобы более отзывчивое приложение было
'    'End If
'    ''*---------конец второго Вывода Изображения ------------*

'    'If вывестиИзображения Then
'    '    countОбновлениеЭкрана = 0
'    '    вывестиИзображения = False
'    'End If

'    'If вывестиИзображения2 Then
'    '    считатьОбнЭкрана2 = False
'    '    countОбновлениеЭкрана2 = 0
'    '    вывестиИзображения2 = False
'    'End If
'End Sub


''delegate****************************
'' делегат обязан иметь ту же самую сигнатуру, что и метод
'' этот вызов асинхронный.
''Public Delegate Function AsyncMethodCaller(ByVal callDuration As Integer, <Runtime.InteropServices.Out()> ByRef threadId As Integer) As String
'Public Delegate Sub AsyncMethodCaller(arrDataTCP() As Double) 'ByVal arrДанные() As Double)

'Private Sub mSyncSocketClient_SyncSocketClientAcquiredData(sender As Object, e As SyncSocketClient.AcquiredDataEventArgs) Handles mSyncSocketClient.SyncSocketClientAcquiredData
'    ' создать делегат.
'    Dim caller As New AsyncMethodCaller(AddressOf Me.TCP_AcquiredData)

'    'Dim result As IAsyncResult = caller.BeginInvoke(e.arrDataTCP, _
'    '        dummy, _
'    '        AddressOf CallbackMethod, _
'    '        "The call executed on thread {0}, with return value ""{1}"".")

'    'Он имеет те же параметры, что и метод, который нужно выполнить асинхронно, а также два дополнительных параметра. 
' Первый параметр является делегатом AsyncCallback, который ссылается на метод, вызываемый при завершении асинхронного вызова. 
' Второй параметр — это пользовательский объект, который передает данные в метод обратного вызова
'    Dim result As IAsyncResult = caller.BeginInvoke(e.arrDataTCP, AddressOf CallbackMethod, Nothing)
'End Sub

'' делегат обязан иметь ту же самую сигнатуру, что и метод
'' AsyncCallback delegate.
'Shared Sub CallbackMethod(ByVal ar As IAsyncResult)
'    ' Retrieve the delegate.
'    Dim result As AsyncResult = CType(ar, AsyncResult)
'    Dim caller As AsyncMethodCaller = CType(result.AsyncDelegate, AsyncMethodCaller)

'    ' вернуть строку. что была передана в информации состояния 
'    'Dim formatString As String = CType(ar.AsyncState, String)

'    ' определить переменную полученную из выходного параметра.
'    ' парамет передаётся по ссылке ByRef <Out> и должен быть плолем класса для передачи в BeginInvoke.
'    'Dim threadId As Integer = 0

'    ' EndInvoke получает результат.
'    'Dim returnValue As String = caller.EndInvoke(threadId, ar)
'    caller.EndInvoke(ar)

'    ' использовать строковый формат для вывода сообщения.
'    'Console.WriteLine(formatString, threadId, returnValue)
'End Sub

''delegate****************************

'Private Sub mSyncSocketClient_SyncSocketClientAcquiredData() Handles mSyncSocketClient.SyncSocketClientAcquiredData
'    'arrДанныеСервераЗначение = e.arrDataTCP
'    'на самом деле e.arrDataTCP это arrДанныеСервераЗначение
'    TCP_AcquiredData()
'End Sub

'Private Sub mSyncSocketClient_SyncSocketClientAcquiredData() Handles mSyncSocketClient.SyncSocketClientAcquiredData
'    TCP_AcquiredData()
'    'Application.DoEvents()
'    'My.Application.DoEvents()
'End Sub

'''' <summary>
'''' проверить не был ли сокет уже создан
'''' </summary>
'''' <remarks></remarks>
'Private Sub CheckIsSocketCreate()
'    If mSyncSocketClient IsNot Nothing Then
'        If mSyncSocketClient.СоединениеУстановлено Then
'            If mSyncSocketClient.IsStartAcquisition Then
'                mSyncSocketClient.StopAcquisition()
'                'ОстановитьСборСокета()
'            End If

'            Thread.Sleep(200)
'            Application.DoEvents()
'            mSyncSocketClient.Shutdown()
'            Application.DoEvents()
'        End If
'        mSyncSocketClient = Nothing
'    End If
'End Sub

'''' <summary>
'''' Запуск сокета (с его таймером сбора) и таймера формы для обновления собранных данных
'''' </summary>
'''' <remarks></remarks>
'Private Sub StartSocket()
'    ' 1.сформировать  m_OutputChannelsBindingList(используется в mSyncSocketClient для формирования списка каналов опроса) на основании записанной конфигурации каналов
'    ' 2.т.к. используются экранные имена clsChannelOutput.НаименованиеПараметра (не исключён повтор имён) то сделать поиск clsChannelOutput.Name
'    ' 3.arrПараметры - arrНаименование2 - arrIndexParameters(номера)
'    RegistrationEventLog.EventLog_MSG_USER_ACTION("<" & "StartSocket" & "> " & "Попытка установки связи c Сервером")

'    If СчитатьНастройкиТсрКлиента() Then
'        If НастроитьВЗависимостиОтСтендаИКонфигурации() Then
'            TSLabelMessageTcpClient.Text = "Запуск сбора расчётных значений"
'            mSyncSocketClient.ЧастотаСбора = FrequencyBackground
'            mSyncSocketClient.Настроить()

'            mSyncSocketClient.StartAcquisition() ' идёт последним - там настройка размерностей
'            'mmTimerMainStart() ' затем запуск таймера формы
'            ' ИнициализацияССобытиемДляСокета()
'        End If
'    End If
'End Sub
