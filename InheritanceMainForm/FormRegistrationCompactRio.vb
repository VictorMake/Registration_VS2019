Imports System.Threading
Imports MathematicalLibrary

Friend Class FormRegistrationCompactRio
    ''' <summary>
    ''' Счетчик цикла для передачи данных клиентам
    ''' </summary>
    Private counterDataToNetwork As Integer
    ''' <summary>
    ''' Накопленные данные передать по сети клиенту
    ''' </summary>
    Private isSendDataToClient As Boolean

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationCompactRio, NameOf(FormRegistrationCompactRio))
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
        MenuQuestioningParameter.Enabled = False
        ButtonSnapshot.Visible = False
        'MenuModeOfOperation.Enabled = False
        'MenuServerEnable.Enabled = False
        LoadClientServerForm()
        StopTimerCompactRio()
        'AddHandler MainMDIFormParent.EventMainAcquiredDataForChannelsForm, AddressOf MainMDIFormReference_EventMainAcquiredDataForChannelsForm
        CheckOnModificationNameChannel()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
        StopTimerCompactRio()
        'RemoveHandler MainMDIFormParent.EventMainAcquiredDataForChannelsForm, AddressOf MainMDIFormReference_EventMainAcquiredDataForChannelsForm
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
    ''' Запуск-Останов таймера передачи собранных данных на шасси CompactRio.
    ''' </summary>
    ''' <param name="start"></param>
    Protected Overrides Sub StartStopSocket(start As Boolean)
        If start Then StartTimerCompactRio() Else StopTimerCompactRio()
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
            Re.Dim(CopyListOfParameter, IndexParameters.Length - 1)
            Array.Copy(IndexParameters, CopyListOfParameter, IndexParameters.Length)
        End If

        ComboBoxSelectAxis.SelectedIndex = 0 ' по умолчанию первый элемент активный
        SlidePlot.Value = ComboBoxSelectAxis.SelectedIndex + 1
        TuneDiagramUnderSelectedParameterAxesY()
        YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))

        If isUsePens Then TuneAnnotation()

        Cursor = Cursors.Default
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
        ' остановить сбор, чтобы мерцание не мешало combobox
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
    ''' Заглушка: Обновление таблицы Режимы при проверке подключаемых расчётных модулей при работе TcpClient
    ''' </summary>
    Protected Overrides Sub UpdateTableRegimeForTcpClient()
        ' ничего
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' Заглушка: Обновить компенсацию ХС для регистратора SCXI
    ''' </summary>
    Protected Overrides Sub UpdateCompensationXC()
        ' ничего
    End Sub

    'TODO: тест повторов 1
    'Dim TestCount As Integer
    ''' <summary>
    ''' Обновление данных на экране из события сбора
    ''' </summary>
    Friend Overrides Sub AcquiredData()
        'TestCount += 1

        Dim J, N, IndexPlot As Integer
        Dim average As Double ' осреднение
        Dim name As String
        Dim vKey As Integer
        Dim indexDiscret As Integer ' индекс Цифровые
        Dim isHeightLevel As Boolean
        Dim strData As String
        Dim lengthArrayIndexParamMinus1 As Integer = UBound(IndexParameters) - 1 ' uBound Список Параметров Минус 1
        Dim startAddedChannels As Integer = CountMeasurand - AdditionalParameterCount ' Начало добавленных каналов
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
                    'ElseIf IsDigitalInput Then
                    '    average = DigitalInputValue(ParametersType(N).NumberChannel) ' на самом деле индекс этого массива
                End If
            Else
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
                        If IsQuestioningRevolution = False AndAlso average < ValueStopWrite AndAlso TimerButtonRun.Enabled = False Then 'N1<1% ->  выключить запись и задержка кнопки Запуск>10 секунд
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
                        'Case NameTx
                        '    TemperatureTxc = average
                        '    Exit Select
                    Case Else
                        ' здесь проверка дискретных по номеру формулы
                        If ParametersType(N).NumberFormula = 3 Then
                            If name = NameVarStartWrite AndAlso IsQuestioningRevolution AndAlso average = 5 Then ' ->  включить запись
                                If (Not IsRecordEnable) AndAlso (Not isRecordingSnapshot) AndAlso ButtonRecord.Enabled Then
                                    WorkTaskRecordStartStop(False, True, IsFormRunning)
                                    TimerButtonRun.Interval = WaitStartWrite ' 60000
                                    TimerButtonRun.Enabled = True ' задержка на мертвую зону пока не раскрутился N1
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

            ' запись в массив среднего, код такой-же как SweepChart
            ' TODO: тест повторов 2
            MeasuredValues(J, indexTimeVsRow) = average
            'MeasuredValues(J, indexTimeVsRow) = TestCount

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
            Dim index As Integer = 1
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
                        Next
                    Else
                        For J = 1 To UBound(SnapshotSmallParameters)
                            If SnapshotSmallParameters(J).IsVisible Then
                                .Items(SnapshotSmallParameters(J).NumberInList - 1).SubItems(1).Text = CStr(Math.Round(MeasuredValues(J - 1, indexTimeVsRow), Precision))
                            End If
                        Next
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
                IsMonitorDigitalOutputPort = False ' на случай если в подсчете ошибка, если ошибки не будет, то там заново TRUE
                DigitalPortForm.Monitor() ' там вызов ОбработатьНакопление() ' там blnМониторDigitalOutputPort = True
                IsMonitorDigitalOutputPort = DigitalPortForm.TSButtonStart.Checked ' должен быть включен после монитора или сброшен после протокола
            End If

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

    '''' <summary>
    '''' Обновление данных на экране из события сбора клиента (разнесено по времени с обработкой аварийных уровненй)
    '''' вызывается из события клиента к Серверу _GFormTestCompactRio.SyncSocketClientAcquiredData
    '''' Заменил напрямой вызов метода  RegistrationMain.AcquiredData() в события таймера OnTimed_mmTimer.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e">массив собранных клиентом данных</param>
    '''' <remarks></remarks>
    'Private Sub MainMDIFormReference_EventMainAcquiredDataForChannelsForm(sender As Object, e As FormMainMDI.AcquiredDataEventArgs) ' Handles MainMDIFormParentReference.EventMainAcquiredDataForChannelsForm
    '    DataValuesFromServer = e.ParametersAcquiredData
    '    AcquiredData()
    'End Sub

    ''' <summary>
    ''' Запуск таймера передачи данных собранными шасси CompactRio.
    ''' Сбор на шасси и сетевой обмен с Registration продолжается.
    ''' </summary>
    Private Sub StartTimerCompactRio()
        ' 1.сформировать  m_OutputChannelsBindingList(используется в mSyncSocketClient для формирования списка каналов опроса) на основании записанной конфигурации каналов
        ' 2.т.к. используются экранные имена clsChannelOutput.НаименованиеПараметра (не исключён повтор имён) то сделать поиск clsChannelOutput.Name
        ' 3.arrПараметры - arrНаименование2 - arrIndexParameters(номера)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StartTimerCompactRio)}> Запуск таймера передачи данных собранными шасси CompactRio.")

        LabelMessageTcpClient.Text = "Запуск сбора"

        ' не объединять два условия
        If MainMDIFormParent.GFormTestCompactRio IsNot Nothing Then
            MainMDIFormParent.GFormTestCompactRio.Initialize()
            'MainMDIFormParent.GFormTestCompactRio.StartAcquisitionTimer(New Action(Of Object, EventArgs)(AddressOf MainMDIFormParent.GFormTestCompactRio.RegistrationTimerTick)) ' идёт последним - там настройка размерностей
            MainMDIFormParent.GFormTestCompactRio.StartAcquisitionTimer(AddressOf MainMDIFormParent.GFormTestCompactRio.RegistrationTimerTick)
        End If
    End Sub

    ''' <summary>
    ''' Останов таймера передачи данных собранными шасси CompactRio.
    ''' Сбор на шасси и сетевой обмен с Registration продолжается.
    ''' </summary>
    Private Sub StopTimerCompactRio()
        If MainMDIFormParent.GFormTestCompactRio IsNot Nothing Then
            If MainMDIFormParent.GFormTestCompactRio.IsStartAcquisition Then
                ' вначале остановить таймер формы
                MainMDIFormParent.GFormTestCompactRio.StopAcquisition()
                Thread.Sleep(50)
            End If
        End If

        LabelMessageTcpClient.Text = "Остановка сбора"
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StopTimerCompactRio)}> Останов таймера передачи данных собранными шасси CompactRio.")
    End Sub
End Class
