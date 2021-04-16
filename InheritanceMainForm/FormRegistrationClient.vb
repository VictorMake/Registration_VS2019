Imports System.Threading
Imports MathematicalLibrary

Friend Class FormRegistrationClient
    ''' <summary>
    ''' Сервер изменил структуру каналов в передаваемой строке
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsNewImitatorSnapshot As Boolean
    ''' <summary>
    ''' естьМодулиСбораКТ
    ''' </summary>
    Private isModuleSolveKT As Boolean
    ''' <summary>
    ''' Проверка зависания поступившие данные
    ''' </summary>
    Private newReceiveData(2) As Double
    ''' <summary>
    ''' Проверка зависания предыдущие данные
    ''' </summary>
    Private oldReceiveData(2) As Double
    ''' <summary>
    ''' Счетчик повторов
    ''' </summary>
    Private counterRepetition As Short

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, NameOf(FormRegistrationClient))
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
        IsServer = False
        CheckModuleSolveKT()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
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
        TimerTimeOutClient.Enabled = False
        MenuNewWindowClient.Enabled = True
        MainMDIFormParent.MenuNewWindowClient.Enabled = True
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
        MenuQuestioningParameter.Enabled = False
        MenuServerEnable.Enabled = False
        MenuReceiveFromServer.Enabled = True
        MenuModeOfOperation.Enabled = False
        MenuConfigurationChannels.Enabled = False
        ButtonContinuously.Enabled = False
        ButtonRecord.Enabled = False
    End Sub
    ''' <summary>
    ''' вызывается при образовании формы
    ''' </summary>
    Friend Overrides Sub StartMeasurement()
        MyBase.StartMeasurement()

        ShowFormClientDataSocket()
        ButtonContinuously.Checked = True
        TimerTimeOutClient.Enabled = True
    End Sub
    ''' <summary>
    ''' Настройка конфигурации режима
    ''' </summary>
    Protected Overrides Sub TuneConditionsRegime()
        InstallQuestionForm(WhoIsExamine.Examination)
        frmTuningRegime = New FormTuningRegime
        frmTuningRegime.ShowDialog()
    End Sub
    ''' <summary>
    ''' Полностью переопределяет метод
    ''' </summary>
    Protected Overrides Sub SettingsOptionProgram()
        'MyBase.SettingsOptionProgram()

        InstallQuestionForm(WhoIsExamine.Setting)
        SettingForm.ShowDialog() ' обязательно модально
        StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyBackground) & "Гц"
    End Sub
    ''' <summary>
    ''' Нет реализации
    ''' </summary>
    Protected Overrides Sub StartStopSocket(start As Boolean)
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

        If IsNewImitatorSnapshot = False Then ReadConfigurationRegime()

        UnpackStringConfigurationWithEmpty(ConfigurationString)
        Re.Dim(IndexParameters, 0)
        ' выгрузить индикаторы аварийных значений
        UnloadAlarmButton()
        left = AlarmPulseButton(keyFirstButtonAlarm).Left
        width = AlarmPulseButton(keyFirstButtonAlarm).Width
        height = AlarmPulseButton(keyFirstButtonAlarm).Height
        ' для снимка уже установленные другой формой глобальные переменные не менять
        AdditionalParameterCount = 0 : DigitalParametersCount = 0 : CountSolveParameters = 0

        ' Массив IndexParameters() состоит только из параметров подлежащих измерению
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

        If IsFrmDigitalOutputPortStart Then DigitalPortForm.PopulateListParametersFromServer()
        If gFormsPanelManager.FormPanelMotoristCount > 0 Then gFormsPanelManager.PopulateListParametersFromServer()

        If isModuleSolveKT AndAlso IsNewImitatorSnapshot Then ' для клиента
            MainMDIFormParent.ModuleAcquisitionKTManager.PopulateListParametersFromServer()
        End If

        RunRegistrationForm()
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
        TimerTimeOutClient.Enabled = False
        FormClientDataSocket.ButtonDisconnect_Click(FormClientDataSocket.ButtonDisconnect, New EventArgs)
    End Sub
    ''' <summary>
    ''' Определить удленение шкалы
    ''' Для клиента в связи с пропусками пакетов время сбора растягивается
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides Function GetLengtheningScale() As Double
        Return LinearInterpolation(FrequencyBackground, 1, 1, 100, 2)
    End Function
    ''' <summary>
    ''' Нет реализации
    ''' Обновление таблицы Режимы при проверке подключаемых расчётных модулей при работе TcpClient
    ''' </summary>
    Protected Overrides Sub UpdateTableRegimeForTcpClient()
        ' ничего
    End Sub
    ''' <summary>
    ''' Нет реализации
    ''' Обновить компенсацию ХС для регистратора SCXI
    ''' </summary>
    Protected Overrides Sub UpdateCompensationXC()
        ' ничего
    End Sub

    ''' <summary>
    ''' Обновление данных на экране из события DataSocket клиентского получения данных
    ''' </summary>
    Friend Overrides Sub AcquiredData()
        'Public Sub ReceiveDataFromServer(ByRef dataFromServer() As Double)
        Dim J, N, IndexPlot As Integer
        Dim average As Double ' осреднение
        Dim name As String
        Dim vKey As Integer
        Dim indexDiscret As Integer ' индекс Цифровые
        Dim isHeightLevel As Boolean
        Dim lengthArrayIndexParamMinus1 As Integer = UBound(IndexParameters) - 1 ' uBound Список Параметров Минус 1
        Dim isVisible As Boolean ' для аварийных индикаторов

        If IsNewImitatorSnapshot Then Exit Sub

        countUpdateScreen1 += 1S

        If countUpdateScreen1 >= RefreshScreen Then
            isFireUpdateScreen1 = True
            isNeedToCountForUpdateScreen2 = True
        End If

        If isNeedToCountForUpdateScreen2 Then countUpdateScreen2 += 1S
        If countUpdateScreen2 >= RefreshScreen2 Then isFireUpdateScreen2 = True

        xNewPos = Math.Round((lastx + 1), 2) Mod (arraysize + 1) ' запись в массив среднего, код такой-же как SweepChart
        indexTimeVsRow = CInt(lastx)

        For J = 0 To CountMeasurand
            vKey = J + 1 ' ищем этот номер
            average = DataValuesFromServer(vKey)
            N = IndexParameters(vKey)
            name = ParametersType(N).NameParameter

            '*---------начало  вывестиИзображения ------------*
            If isFireUpdateScreen1 Then
                ' проверка на зависание
                If J < 2 Then
                    newReceiveData(J) = average
                End If

                Select Case name
                    Case NameTBox
                        TemperatureOfBox = average
                        coefficientBringingTBoxing = Math.Sqrt(Const288 / (TemperatureOfBox + Kelvin))
                        Exit Select
                    Case conN1
                        TextBoxIndicatorN1physics.Text = $"N1ф={Format(average, FormatString)} %"
                        newReceiveData(0) = average
                        TextBoxIndicatorN1reduce.Text = $"N1п={Format(average * coefficientBringingTBoxing, FormatString)} %"
                        Exit Select
                    Case conN2
                        TextBoxIndicatorN2physics.Text = $"N2ф={Format(average, FormatString)} %"
                        newReceiveData(1) = average
                        N2reduced = average * coefficientBringingTBoxing
                        TextBoxIndicatorN2reduce.Text = $"N2п={Format(N2reduced, FormatString)} %"
                        Exit Select
                    Case conаРУД
                        ARUD = average
                        TextBoxIndicatorRud.Text = $"Руд={Format(average, FormatString)}" ' & "дел"
                        newReceiveData(2) = average
                        Exit Select
                    Case Else
                        'здесь проверка дискретных по номеру формулы
                        If ParametersType(N).NumberFormula = 3 Then
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
                    ElseIf average < ParametersType(N).AlarmValueMin Then
                        isVisible = True
                        AlarmPulseButton(N).Text = $"{name}<{CStr(ParametersType(N).AlarmValueMin)}"
                    End If

                    If AlarmPulseButton(N).Visible <> isVisible Then AlarmPulseButton(N).Visible = isVisible
                End If
            End If ' вывести изображение
            '*---------конец  Вывода Изображения ------------*

            If isShowDiagramFromParameter Then ParameterTwoGraph(N) = average

            ' накопление для монирота Поля и накопление для подсчета К.Т.
            If IsSubscriptionOnEventsAcquisitionEnabled OrElse isModuleSolveKT OrElse (IsMonitorDigitalOutputPort AndAlso IsFrmDigitalOutputPortStart) Then
                ParameterAccumulate(N) = average
            End If

            ' запись в массив среднего, код такой-же как SweepChart
            MeasuredValues(J, indexTimeVsRow) = average
            If isDetailedSheet Then
                If ParametersType(IndexParameters(vKey)).IsVisible Then
                    average = CastToAxesStandard(NumberParameterAxes, vKey, average) ' массив приведен к какой-то шкале
                    MeasuredValuesToRange(J, 0) = average

                    If IndexPlot <= countVisibleTrendRegistrator Then dataAllTrends(IndexPlot, indexTimeVsRow) = average

                    IndexPlot += 1
                End If
            Else
                If SnapshotSmallParameters(vKey).IsVisible Then
                    average = CastToAxesStandard(NumberParameterAxes, vKey, average) ' массив приведен к какой-то шкале
                    MeasuredValuesToRange(J, 0) = average ' массив приведен к какой-то шкале

                    If IndexPlot <= countVisibleTrendRegistrator Then dataAllTrends(IndexPlot, indexTimeVsRow) = average

                    IndexPlot += 1
                End If
            End If

            average = 0
        Next J ' по измеренным каналам

        SweepChart()

        If isShowDiagramFromParameter Then OutParametersGraph()

        '*---------начало второго Вывода Изображения ------------*
        If IsSubscriptionOnEventsAcquisitionEnabled OrElse isModuleSolveKT Then
            'Dim fireAcquiredDataEventArgs As New AcquiredDataEventArgs(ParameterAccumulate)
            ''  Теперь вызов события с помощью вызова делегата.
            ''  Передать object которое инициирует событие (Me) такое же как FireEventArgs. 
            ''  Вызов обязан соответствовать сигнатуре FireEventHandler.
            'RaiseEvent AcquiredDataFormMain(Me, fireAcquiredDataEventArgs)

            OnAcquiredData(New AcquiredDataEventArgs(ParameterAccumulate))
        End If

        If isFireUpdateScreen2 Then
            ' пользовательская настройка частоты обновления значений параметров
            If countTextDisplayRate >= TextDisplayRate Then
                countTextDisplayRate = 0

                ListViewAcquisition.BeginUpdate()
                With ListViewAcquisition ' обновить лист
                    If isDetailedSheet Then
                        For J = 0 To lengthArrayIndexParamMinus1 'UBound(arrIndexParameters) - 1
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

                'If mblnТарировкаТяги Then frmТарировкаТяги.Замерить(Round(arrСреднее(frmТарировкаТяги.indexЭталон, indexTimeVsRow)), Round(arrСреднее(frmТарировкаТяги.indexИзмерительный, indexTimeVsRow)))
                If IsShowTextControl Then frmTextControl.UpdateValuesTextControls(MeasuredValues, indexTimeVsRow) 'If mblnТекстКонтроль Then frmТекстКонтроль.Вывести(arrСреднее, xNewPos)
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
        End If
        '*---------конец второго Вывода Изображения ------------*

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
    ''' Новый снимок имитатора.
    ''' При работе в режиме клиента в новых данных изменился список каналов,
    ''' поэтому требуется перенастройка.
    ''' </summary>
    Friend Sub NewChannelsInImitatorSnapshot()
        IsNewImitatorSnapshot = True
        If IsShowTextControl Then frmTextControl.Close()
        If IsShowGraphControl Then frmGraphControl.Close()
        ' СтрокаКонфигурации изменилась поэтому перегрузка
        ApplyRegimeRegistrator() ' идет первым
        ButtonContinuously.Checked = True
        Re.Dim(ParameterTwoGraph, UBound(ParametersType))
        RestoreDiagramFromParameter()

        If isShowDiagramFromParameter Then TuneRepresentationParameter()

        If gFormsPanelManager.FormPanelMotoristCount > 0 Then
            ' запуск процедуры проверки корректности
            gFormsPanelManager.СonfigureAllMotoristPanels(False)
            gFormsPanelManager.RestartAllTask()
        End If

        IsNewImitatorSnapshot = False
    End Sub

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
                MainMDIFormParent.ModuleAcquisitionKTManager.ParentFormMain = Me
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

    Private Sub MenuReceiveFromServer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuReceiveFromServer.Click
        FormClientDataSocket.IsFormOpenedFromMenu = True
        ShowFormClientDataSocket()
    End Sub
    ''' <summary>
    ''' Показать форму получения данных по сети
    ''' </summary>
    Private Sub ShowFormClientDataSocket()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ShowFormClientDataSocket)}> Показать форму получения данных по сети")

        ' запуск нового сервера или изменение адреса у старого если он уже запущен
        If IsFormClientStart Then
            FormClientDataSocket.TextBoxURL.Text = AddressURLServer
            FormClientDataSocket.ButtonConnect_Click(FormClientDataSocket.ButtonConnect, New EventArgs)
            FormClientDataSocket.TimerReceive.Enabled = True
            IsFocusToClient = True

            WindowState = FormWindowState.Normal
            FormClientDataSocket.WindowState = FormWindowState.Normal ' Maximized 
            FormClientDataSocket.Show()
            FormClientDataSocket.Activate()
        Else ' или если не запущен
            If CBool(InStr(1, ServerWorkingFolder, "\\")) Then ' другой компьютер
                AddressURLServer = Mid$(ServerWorkingFolder, 3, InStr(3, ServerWorkingFolder, Separator) - 3)
                AddressURLServer = $"dstp://{AddressURLServer}/wave"
            Else ' локальный компьютер
                AddressURLServer = "dstp://localhost/wave"
            End If

            FormClientDataSocket = New FormClient(Me)
            FormClientDataSocket.Show()
            FormClientDataSocket.Activate()
            IsFormClientStart = True
            IsFocusToClient = True
        End If
    End Sub
    ''' <summary>
    ''' Проверка зависания Клиента
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TimerTimeOutClient_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerTimeOutClient.Tick
        ' Проверка на зависание 3 пришедших по сети данных
        ' значит обновления не было тогда перезагрузка
        If oldReceiveData(0) = newReceiveData(0) AndAlso oldReceiveData(1) = newReceiveData(1) AndAlso oldReceiveData(2) = newReceiveData(2) Then
            counterRepetition += 1S

            If counterRepetition >= 3 Then
                FormClientDataSocket.DataSocketReceive.Disconnect()
                Application.DoEvents()
                Thread.Sleep(500)
                FormClientDataSocket.DataSocketReceive.Connect(FormClientDataSocket.TextBoxURL.Text, NationalInstruments.Net.AccessMode.ReadAutoUpdate)
                Application.DoEvents()
                Thread.Sleep(500)
                counterRepetition = 0
            End If
            Exit Sub
        End If

        ' запомнить
        oldReceiveData(0) = newReceiveData(0)
        oldReceiveData(1) = newReceiveData(1)
        oldReceiveData(2) = newReceiveData(2)
        counterRepetition = 0
    End Sub
End Class