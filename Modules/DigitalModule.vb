Imports NationalInstruments.DAQmx
Imports NationalInstruments.Analysis.Math
Imports System.Threading.Thread
Imports System.Collections.Generic
Imports System.ComponentModel
Imports MathematicalLibrary

Module DigitalModule
    ' базовые формы отличаются внешним видом:
    ' RegistrationBase - панель аварийных значений и лампы аварий
    Public Const c_RegistrationBase As String = "Базовая Регистратор"
    Public Const c_RegistrationSCXI As String = "Регистратор"
    Public Const c_RegistrationClient As String = "Клиент"
    Public Const c_RegistrationTCP As String = "Клиент TCP"
    ' SnapshotBase - панелью работы с графиками
    Public Const c_SnapshotBase As String = "Базовая Снимок"
    Public Const c_SnapshotViewingDiagram As String = "Расшифровка Снимков"
    Public Const c_SnapshotPhotograph As String = "Снятие и просмотр Снимков"

    ''' <summary>
    ''' Тип Испытания
    ''' </summary>
    ''' <remarks></remarks>
    <Flags()> Public Enum FormExamination
        'None = 0'нельзя с таким окном
        <Description(c_RegistrationSCXI)>
        RegistrationSCXI = 1
        <Description(c_RegistrationClient)>
        RegistrationClient = 2
        <Description(c_RegistrationTCP)>
        RegistrationTCP = 4
        <Description(c_SnapshotViewingDiagram)>
        SnapshotViewingDiagram = 8
        <Description(c_SnapshotPhotograph)>
        SnapshotPhotograph = 16

        <Description(c_RegistrationBase)>
        RegistrationBase = FormExamination.RegistrationSCXI Or FormExamination.RegistrationClient Or FormExamination.RegistrationTCP
        <Description(c_SnapshotBase)>
        SnapshotBase = FormExamination.SnapshotViewingDiagram Or FormExamination.SnapshotPhotograph
    End Enum

    ''' <summary>
    ''' Для определения групп форм, создаваемых по связанным пунктам меню 
    ''' </summary>
    Public Enum TypeExamination
        Registration = 1
        Client = 2
        Snapshot = 8
    End Enum

    ''' <summary>
    ''' Какая форма опрашивает железо
    ''' </summary>
    Public Enum WhoIsExamine
        Oscilloscope = 1 'Осциллограф
        Snapshot = 2 'Снимок
        Taring = 3 'Тарировка
        Examination = 4 'Опрос
        Setting = 5 'Шапка
        Nobody = 6 'Никто
    End Enum
    Public activeForm As WhoIsExamine

    Public RegistrationMain As FormRegistrationBase
    Public SnaphotMain As FormSnapshotBase
    Public TarirForm As FormTarir
    Public ServiceBasesForm As FormServiceBases
    Public DigitalPortForm As FormDigitalPort
    Public MDITabPanelMotoristForm As FormMDITabPanel
    Public IsAcquisitionOk As Boolean = False
    Public DAQmxTask As Task

    Public IsTaskRunning As Boolean
    Public DataAnalogChannelReader As Double(,)
    Public AnalogInReader As AnalogMultiChannelReader
    Public MultiAnalogCallback As AsyncCallback
    Public CountsBuffers, CurrentCountBuffers As Integer
    ''' <summary>
    ''' Буфер Снимка
    ''' </summary>
    Public BuffersSnapshot As Dictionary(Of Integer, Double(,))
    Public parametersCount, countAcquisition As Integer

    Private inputCoupling As AICoupling
    Private terminalConfiguration As AITerminalConfiguration

    Private taskRunningDigitalInput As Boolean
    Private mTaskDigitalInput As Task
    Private waveformDigitalInput As DigitalWaveform()
    Private mDigitalReader As DigitalMultiChannelReader

    Public Sub CreateAiChannel(ByVal inNumberChannel As String,
                               ByVal inNumberDevice As String,
                               ByVal inNumberModuleChassis As String,
                               ByVal inNumberChannelModule As String,
                               ByVal inLowerMeasure As Double,
                               ByVal inUpperMeasure As Double,
                               ByVal inTypeConnection As String,
                               ByVal inSignalType As String)

        Try
            Dim aiChannel As AIChannel
            ' получить Terminal Configuration ="DIFF" Or ="RSE" Or ="NRSE" Or ="4WRITE"
            Select Case inTypeConnection
                Case "RSE" ' в DAQmx "Rse"
                    terminalConfiguration = AITerminalConfiguration.Rse
                    Exit Select
                Case "NRSE" ' в DAQmx "Nrse"
                    terminalConfiguration = AITerminalConfiguration.Nrse
                    Exit Select
                Case "Pseudodifferential" ' у меня нет
                    terminalConfiguration = AITerminalConfiguration.Pseudodifferential
                    Exit Select
                Case "Let NI-DAQ Choose" ' у меня нет
                    terminalConfiguration = CType(-1, AITerminalConfiguration)
                    Exit Select
                Case Else ' "DIFF"
                    terminalConfiguration = AITerminalConfiguration.Differential
                    Exit Select
            End Select

            ' получить Input Coupling ="DC" Or ="AC"
            Select Case inSignalType
                Case "DC"
                    inputCoupling = AICoupling.DC
                    Exit Select
                Case "Gnd" ' у меня нет
                    inputCoupling = AICoupling.Ground
                    Exit Select
                Case Else '"AC"
                    inputCoupling = AICoupling.AC
                    Exit Select
            End Select

            Dim deviseString As String

            If inNumberModuleChassis <> "-1" Then
                deviseString = $"SC{inNumberDevice}Mod{inNumberModuleChassis}/ai{inNumberChannelModule}"
            Else
                deviseString = $"Dev{inNumberDevice}/ai{inNumberChannel}"
            End If

            aiChannel = DAQmxTask.AIChannels.CreateVoltageChannel(deviseString, "", terminalConfiguration,
                inLowerMeasure, inUpperMeasure, AIVoltageUnits.Volts)
            ' конфигурировать входные настройки
            aiChannel.Coupling = inputCoupling
        Catch ex As DaqException
            Const caption As String = "CreateAiChannel"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            DAQmxTask.Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' Запустить Форму Регистратора
    ''' </summary>
    Public Sub RunRegistrationForm()
        If RegistrationFormName <> "" Then
            For Each itemForm In CollectionForms.Forms.Values
                If itemForm.Text = RegistrationFormName Then
                    Application.DoEvents()
                    CType(itemForm, FormRegistrationBase).StartContinuous() ' там же IsRun=true
                    Exit For
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Установить Опрашивающую Форму
    ''' </summary>
    ''' <param name="inActiveForm"></param>
    Public Sub InstallQuestionForm(ByVal inActiveForm As WhoIsExamine)
        ' strForm используется для определения, где останавливать сбор
        ' "Осциллограф" или "Тарировка"
        ' железо не может работать с двумя задачами
        StopAcquisition()

        activeForm = inActiveForm

        For Each itemForm In CollectionForms.Forms.Values
            Select Case inActiveForm
                Case WhoIsExamine.Oscilloscope
                    If CStr(itemForm.Tag) = TagFormDaughter Then
                        'If CType(itemForm, FormRegistrationBase).ТипИспытания And FormExamination.RegistrationBase Then
                        Dim tmpFormRegistrationBase As FormRegistrationBase = TryCast(itemForm, FormRegistrationBase)
                        If tmpFormRegistrationBase IsNot Nothing Then
                            tmpFormRegistrationBase.IsFormRunning = True
                            Continue For
                        End If
                    End If
                    Exit Select
                Case WhoIsExamine.Snapshot
                    If CStr(itemForm.Tag) = TagFormDaughter Then
                        'If CType(itemForm, FormSnapshotPhotograph).ТипИспытания And FormExamination.SnapshotBase Then
                        Dim tmpFormSnapshotPhotograph As FormSnapshotPhotograph = TryCast(itemForm, FormSnapshotPhotograph)
                        If tmpFormSnapshotPhotograph IsNot Nothing Then
                            tmpFormSnapshotPhotograph.IsFormRunning = True
                            Continue For
                        End If
                    End If
                    Exit Select
                Case WhoIsExamine.Taring
                    If CStr(itemForm.Tag) = TagFormTarir Then
                        CType(itemForm, FormTarir).IsRunning = True
                    End If
                    Exit Select
            End Select
        Next
    End Sub

    ''' <summary>
    ''' ОстановитьСбор
    ''' </summary>
    Public Sub StopAcquisition()
        If IsWorkWithController Then
            If activeForm = WhoIsExamine.Examination AndAlso TestChannelForm.Running Then
                Try
                    If IsTaskRunning = True Then
                        IsTaskRunning = False

                        If Not DAQmxTask Is Nothing Then
                            DAQmxTask.Stop()
                            DAQmxTask.Dispose()
                            DAQmxTask = Nothing
                        End If
                    End If
                Catch ex As Exception
                    Const caption As String = "Остановка сбора"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                End Try
            End If
        End If

        For Each tempForm In CollectionForms.Forms.Values
            If CStr(tempForm.Tag) = TagFormDaughter Then
                Dim tmpFormRegistrationSCXI As FormRegistrationSCXI = TryCast(tempForm, FormRegistrationSCXI)
                If tmpFormRegistrationSCXI IsNot Nothing Then
                    If tmpFormRegistrationSCXI.IsFormRunning Then
                        MainMdiForm.TimerAwait.Enabled = False

                        Try
                            If IsTaskRunning = True Then
                                IsTaskRunning = False

                                If Not DAQmxTask Is Nothing Then
                                    DAQmxTask.Stop()
                                    Sleep(100)
                                    DAQmxTask.Dispose()
                                    DAQmxTask = Nothing
                                End If
                            End If

                            If IsDigitalInput Then StopTaskDigitalInput()
                        Catch ex As Exception
                            Const caption As String = "Остановка сбора"
                            Dim text As String = ex.ToString
                            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        End Try

                        tmpFormRegistrationSCXI.StopRecord()
                        tmpFormRegistrationSCXI.IsFormRunning = False
                    End If

                    Continue For
                End If

                Dim tmpFormSnapshotPhotograph As FormSnapshotPhotograph = TryCast(tempForm, FormSnapshotPhotograph)
                If tmpFormSnapshotPhotograph IsNot Nothing Then
                    If tmpFormSnapshotPhotograph.IsFormRunning Then
                        tmpFormSnapshotPhotograph.IsFormRunning = False
                    End If

                    Continue For
                End If
            End If

            If CStr(tempForm.Tag) = TagFormTarir Then
                If CType(tempForm, FormTarir).IsRunning Then
                    CType(tempForm, FormTarir).IsRunning = False
                End If
            End If
        Next

        Application.DoEvents()
        activeForm = WhoIsExamine.Nobody
    End Sub

    ''' <summary>
    ''' Делегат ассинхронной задачи окончания сбора
    ''' </summary>
    ''' <param name="ar"></param>
    Public Sub MultiAnalogInCallback(ByVal ar As IAsyncResult)
        Try
            If IsTaskRunning = True Then
                DataAnalogChannelReader = AnalogInReader.EndReadMultiSample(ar)
                Select Case activeForm
                    Case WhoIsExamine.Oscilloscope
                        For I As Integer = 1 To ADCAcquisitionParametersCount
                            ' так как стало измерять только помеченные для записи без пропуска GarrСписокПараметров(J) - 1)
                            RegistrationMain.DataValuesFromServer(I) = Statistics.Mean(ArrayOperation.CopyRow(DataAnalogChannelReader, I - 1))
                        Next ' по измеренным каналам

                        IsAcquisitionOk = True
                        RegistrationMain.AcquiredData()
                        ' вставил сюда
                        AnalogInReader.BeginReadMultiSample(LevelOversampling, MultiAnalogCallback, Nothing)

                        Exit Select
                    Case WhoIsExamine.Snapshot
                        CurrentCountBuffers += 1

                        If CurrentCountBuffers < CountsBuffers Then
                            AnalogInReader.BeginReadMultiSample(CountAcquisitionSCXI, MultiAnalogCallback, Nothing)
                            BuffersSnapshot(CurrentCountBuffers) = DataAnalogChannelReader
                            SnaphotMain.ProgressBarExport.Value = CurrentCountBuffers
                        Else
                            IsTaskRunning = False

                            If Not DAQmxTask Is Nothing Then
                                DAQmxTask.Stop()
                                Sleep(100)
                                DAQmxTask.Dispose()
                                DAQmxTask = Nothing
                            End If

                            If CurrentCountBuffers > 1 Then
                                BuffersSnapshot(CurrentCountBuffers) = DataAnalogChannelReader ' записать последний буфер
                                SnaphotMain.ProgressBarExport.Value = CurrentCountBuffers

                                Dim key As Integer
                                Dim oneBuffer As Double(,)
                                Dim rowBuffer As Double() = Nothing
                                Dim I, J, K As Integer

                                'ReDim_DataAnalogChannelReader(parametersCount, countAcquisition)
                                Re.Dim(DataAnalogChannelReader, parametersCount, countAcquisition)
                                'For Each Key In БуферСнимка.Keys нельзя использовать, т.к. идет с конца
                                For key = 1 To CurrentCountBuffers
                                    oneBuffer = BuffersSnapshot(key)

                                    For I = 0 To UBound(oneBuffer) ' по строкам
                                        rowBuffer = ArrayOperation.CopyRow(oneBuffer, I)
                                        For J = 0 To UBound(rowBuffer)
                                            DataAnalogChannelReader(I, K + J) = rowBuffer(J)
                                        Next J
                                    Next I

                                    K = K + UBound(rowBuffer) + 1
                                Next key
                            End If

                            SnaphotMain.ProcesSnapshot()
                            BuffersSnapshot.Clear()
                        End If

                        'Case КтоБудетОпрашивать.Опрос
                        '    mfrmОпросКанала.CWAI1_AcquiredData(ArrayOperation.CopyRow(data, 0))

                        Exit Select
                    Case Else
                        MessageBox.Show("Непонятно, кто опрашивает?", "MultiAnalogInCallback", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Select
                End Select
            End If
        Catch ex As DaqException
            Const caption As String = "MultiAnalogInCallback"
            Dim text As String = ex.ToString

            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            IsTaskRunning = False

            If Not DAQmxTask Is Nothing Then
                DAQmxTask.Stop()
                DAQmxTask.Dispose()
                DAQmxTask = Nothing
            End If
        End Try
    End Sub

    ''' <summary>
    ''' КонфигурироватьБуфер
    ''' </summary>
    ''' <param name="inCountsBuffers"></param>
    ''' <param name="inCountParameters"></param>
    ''' <param name="inCountAcquisition"></param>
    Public Sub ConfigureBuffer(ByVal inCountsBuffers As Integer, ByVal inCountParameters As Integer, ByVal inCountAcquisition As Integer)
        BuffersSnapshot = New Dictionary(Of Integer, Double(,))(inCountsBuffers)
        DigitalModule.parametersCount = inCountParameters
        DigitalModule.countAcquisition = inCountAcquisition
    End Sub

    Public Sub StartTaskDigitalInput()
        Try
            ' создать задачу 
            If taskRunningDigitalInput Then
                taskRunningDigitalInput = False
                If mTaskDigitalInput IsNot Nothing Then
                    mTaskDigitalInput.Stop()
                    mTaskDigitalInput.Dispose()
                    mTaskDigitalInput = Nothing
                End If
            End If
            mTaskDigitalInput = New Task("diTask")

            Dim deviseString As String
            For Each mItemDigitalInput As ItemDigitalInput In gDigitalInput.DigitalInputs.Values
                ' подключить только измеряемые каналы
                ' создать виртуальный канал
                ' добавлять только те каналы, которые помечены на сбор не сделал - 
                ' добавляются все цифровые каналы, значит сбор по всем каналам
                'If Array.IndexOf(arrНаименование2, mItemDigitalInput.НаименованиеПараметра) <> -1 Then
                'SC1Mod1/port0/line31 
                'Dev1/port0/line0, 
                If mItemDigitalInput.ModuleNumderInChassis = "" Then
                    deviseString = $"Dev{mItemDigitalInput.DeviseNumber}/port{mItemDigitalInput.PortNumber}/line{mItemDigitalInput.LineNumber}"
                Else
                    deviseString = $"SC{mItemDigitalInput.DeviseNumber}Mod{mItemDigitalInput.ModuleNumderInChassis}/port{mItemDigitalInput.PortNumber}/line{mItemDigitalInput.LineNumber}"
                End If
                mTaskDigitalInput.DIChannels.CreateChannel(deviseString, "", ChannelLineGrouping.OneChannelForEachLine)
            Next
            ' создать канал
            'myTaskDigitalInput.DIChannels.CreateChannel(strСтрока, "", ChannelLineGrouping.OneChannelForEachLine)
            'myTaskDigitalInput.Timing.ConfigureSampleClock("", CType(samplesClockRate, Double), _
            '                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, CType(samplesClockRate, Integer))

            'фактическая частота сбора
            'myTaskDigitalInput.Timing.ConfigureSampleClock(String.Empty, CType(intЧастотаФонового, Double), _
            '    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, 1)

            mTaskDigitalInput.Timing.ConfigureSampleClock(String.Empty, FrequencyBackground,
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples)
            mTaskDigitalInput.Timing.SampleTimingType = SampleTimingType.OnDemand
            ' On Demand нужен наверно для платы, а для корзины наверно не нужен
            ' проверить корректность задачи
            mTaskDigitalInput.Control(TaskAction.Verify)

            mDigitalReader = New DigitalMultiChannelReader(mTaskDigitalInput.Stream) With {.SynchronizeCallbacks = True}
            mDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), mTaskDigitalInput)
            taskRunningDigitalInput = True
        Catch ex As DaqException
            Dim text As String = ex.ToString
            MessageBox.Show(text, NameOf(StartTaskDigitalInput), MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(StartTaskDigitalInput)}> {text}")
            ' ликвидировать task
            StopTaskDigitalInput()
        End Try
    End Sub

    Private Sub StopTaskDigitalInput()
        taskRunningDigitalInput = False
        mDigitalReader = Nothing
        If mTaskDigitalInput IsNot Nothing Then
            'myTaskDigitalInput.Stop()
            mTaskDigitalInput.Dispose()
        End If
    End Sub

    Private Sub DisplayData(ByVal waveform As DigitalWaveform()) ')
        ' тест расшифровки полученных данных readData() As Boolean
        'Dim val As Long = 0
        'Dim i As Integer
        ''line0CheckBox.Checked = readData(0)
        ''line1CheckBox.Checked = readData(1)
        ''line2CheckBox.Checked = readData(2)
        ''line3CheckBox.Checked = readData(3)
        ''line4CheckBox.Checked = readData(4)
        ''line5CheckBox.Checked = readData(5)
        ''line6CheckBox.Checked = readData(6)
        ''line7CheckBox.Checked = readData(7)
        'For i = 0 To readData.Length - 1
        '    If readData(i) Then
        '        ' если бит установлен
        '        ' добавить добавить цифровое значение бита
        '        val += 1L << i
        '    End If
        'Next i
        '' показать данные в шестнадцатичеричном виде (hex)
        ''hexTextBox.Text = String.Format("0x{0:X}", val)

        ' иттерация по каналам
        Dim currentLineIndex As Integer = 0
        For Each signal As DigitalWaveform In waveform
            For sample As Integer = 0 To (signal.Signals(0).States.Count - 1)
                If (signal.Signals(0).States(sample) = DigitalState.ForceUp) Then
                    'DataTable.Rows(sample)(currentLineIndex) = 1
                    DigitalInputValue(currentLineIndex) = 5
                Else
                    DigitalInputValue(currentLineIndex) = 0
                    'DataTable.Rows(sample)(currentLineIndex) = 0
                End If
            Next
            currentLineIndex += 1
        Next
    End Sub

    Private Sub OnCallbackDataReadyDigitalInput(ByVal result As IAsyncResult)
        Try
            If taskRunningDigitalInput Then
                If mTaskDigitalInput Is result.AsyncState Then
                    'Dim data As Boolean() = myDigitalReader.EndReadSingleSampleMultiLine(result)
                    'DisplayData(data)
                    ' читать 1 значение
                    waveformDigitalInput = mDigitalReader.ReadWaveform(1)
                    DisplayData(waveformDigitalInput)
                    ' заново запустить
                    mDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), mTaskDigitalInput)
                End If
            End If
        Catch ex As DaqException
            Dim text As String = ex.ToString
            MessageBox.Show(text, NameOf(OnCallbackDataReadyDigitalInput), MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(OnCallbackDataReadyDigitalInput)}> {text}")
            StopTaskDigitalInput()
        End Try
    End Sub

#Region "LinqTest"
    Public Sub LinqLineCount(ByVal wordsPhysicalChannel() As String, ByVal physicalPortComboBox As ComboBox, ByRef comboBoxLineCount As ComboBox)
        If (physicalPortComboBox.Items.Count > 0) Then
            physicalPortComboBox.SelectedIndex = 0
            For I As Integer = 0 To physicalPortComboBox.Items.Count - 1
                Dim index As Integer = I ' чтобы отладчик не ругался
                Dim wordGroups = From word In wordsPhysicalChannel
                                 Where word.IndexOf(physicalPortComboBox.Items(index).ToString) <> -1
                                 Group word By Key = "line" Into Count()
                                 Select ProductCount = Count

                For Each w In wordGroups
                    comboBoxLineCount.Items.Add(w) ' & ul.dsd)
                Next
                'LineCount.Items.AddRange(wordGroups)
            Next
            If comboBoxLineCount.Items.Count > 0 Then comboBoxLineCount.SelectedIndex = 0
        End If
    End Sub

    Public Sub LinqLineCount(ByVal wordsPhysicalChannel() As String, ByVal physicalPortComboBox As List(Of String), ByRef lineCount As List(Of Integer))
        If (physicalPortComboBox.Count > 0) Then
            'physicalPortComboBox.SelectedIndex = 0
            For I As Integer = 0 To physicalPortComboBox.Count - 1
                Dim index As Integer = I ' чтобы отладчик не ругался
                Dim wordGroups = From word In wordsPhysicalChannel
                                 Where word.IndexOf(physicalPortComboBox(index)) <> -1
                                 Group word By Key = "line" Into Count()
                                 Select ProductCount = Count

                For Each w In wordGroups
                    lineCount.Add(w) ' & ul.dsd)
                Next
                'LineCount.Items.AddRange(wordGroups)
            Next
            'If LineCount.Count > 0 Then LineCount.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Определить Число Возможных DigitalWriteTask
    ''' </summary>
    ''' <param name="deviceComboBox"></param>
    ''' <param name="physicalPortComboBox"></param>
    ''' <param name="digitalWriteTaskComboBox"></param>
    Public Sub CheckCountsDigitalWriteTask(ByRef deviceComboBox As ComboBox, ByRef physicalPortComboBox As ComboBox, ByRef digitalWriteTaskComboBox As ComboBox)
        Dim I, J As Integer
        If deviceComboBox.Items.Count > 0 AndAlso physicalPortComboBox.Items.Count > 0 Then
            For I = 0 To deviceComboBox.Items.Count - 1
                If deviceComboBox.Items(I).ToString.IndexOf("Dev") <> -1 Then
                    For J = 0 To physicalPortComboBox.Items.Count - 1
                        If physicalPortComboBox.Items(J).ToString.IndexOf(deviceComboBox.Items(I).ToString) <> -1 Then
                            digitalWriteTaskComboBox.Items.Add(deviceComboBox.Items(I).ToString)
                            Exit For
                        End If
                    Next
                ElseIf deviceComboBox.Items(I).ToString.IndexOf("SC") <> -1 Then
                    For J = 0 To physicalPortComboBox.Items.Count - 1
                        If physicalPortComboBox.Items(J).ToString.IndexOf(deviceComboBox.Items(I).ToString) <> -1 Then
                            digitalWriteTaskComboBox.Items.Add(deviceComboBox.Items(I).ToString)
                            Exit For
                        End If
                    Next
                End If
            Next

            If digitalWriteTaskComboBox.Items.Count > 0 Then digitalWriteTaskComboBox.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Определить Число Возможных DigitalWriteTask
    ''' </summary>
    ''' <param name="deviceList"></param>
    ''' <param name="physicalPortList"></param>
    ''' <param name="DigitalWriteTaskList"></param>
    Public Sub CheckCountsDigitalWriteTask(ByRef deviceList As List(Of String), ByRef physicalPortList As List(Of String), ByRef digitalWriteTaskList As List(Of String))
        Dim I, J As Integer

        If deviceList.Count > 0 And physicalPortList.Count > 0 Then
            For I = 0 To deviceList.Count - 1
                If deviceList(I).IndexOf("Dev") <> -1 Then
                    For J = 0 To physicalPortList.Count - 1
                        If physicalPortList(J).IndexOf(deviceList(I)) <> -1 Then
                            digitalWriteTaskList.Add(deviceList(I))
                            Exit For
                        End If
                    Next
                ElseIf deviceList(I).IndexOf("SC") <> -1 Then
                    For J = 0 To physicalPortList.Count - 1
                        If physicalPortList(J).IndexOf(deviceList(I)) <> -1 Then
                            digitalWriteTaskList.Add(deviceList(I))
                            Exit For
                        End If
                    Next
                End If
            Next
        End If
    End Sub
#End Region


    'Private Sub ИнициализацияЦифровыхВыходов()
    '    'узнать конфигурацию платы
    '    'deviceComboBox.Items.AddRange(DaqSystem.Local.Devices)

    '    'physicalPortComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
    '    'If (physicalPortComboBox.Items.Count > 0) Then
    '    '    physicalPortComboBox.SelectedIndex = 0
    '    'End If

    '    'physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External))
    '    'If physicalChannelComboBox.Items.Count > 0 Then
    '    '    physicalChannelComboBox.SelectedIndex = 0
    '    'End If

    '    DigitalWriteTaskPort0 = New Task()

    '    Try
    '        '  создать Digital Output channel и назвать его.
    '        DigitalWriteTaskPort0.DOChannels.CreateChannel( _
    '            DeviceName & "/port0", _
    '            "port0", _
    '            ChannelLineGrouping.OneChannelForAllLines)

    '        '  записать данные в цифровой порт. WriteDigitalSingChanSingSampPort записывает простой набор
    '        '  цифровых данных по требованию, таким образом timeout не нужен.
    '        WriterPort0 = New DigitalSingleChannelWriter(DigitalWriteTaskPort0.Stream)

    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.ToString)
    '        If Not DigitalWriteTaskPort0 Is Nothing Then
    '            DigitalWriteTaskPort0.Dispose()
    '            DigitalWriteTaskPort0 = Nothing
    '        End If
    '    End Try
    'End Sub

    'Private Sub Закрытие()
    '    'обнулить перед выгрузкой
    '    If blnЦиклЗагрузкиЗагружен AndAlso dblСуммарноеВремяСдобавкой <> 0 Then
    '        If ПроверкаЗагрузкиПроведена Then
    '            ИсполнитьЗагрузки(True) 'нулевые загрузки
    '        End If
    '    End If

    '    Try
    '        If Not DigitalWriteTaskPort0 Is Nothing Then
    '            DigitalWriteTaskPort0.Dispose()
    '            DigitalWriteTaskPort0 = Nothing
    '        End If

    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.ToString)
    '    End Try
    'End Sub

    'Private Sub ИсполнитьЗагрузки(ByVal blnУстановитьВсеУстройстваВНуль As Boolean)
    '    'в цикле по устройствам
    '    'по функции найти значение желательно без округления и задать UpdateOutput
    '    'в порт значение
    '    valPort0 = 0
    '    UsePort0 = False

    '    For Each УстройствоВЦикле As Device In УстройстваВЦиклеДляИсполнения.Values
    '        If blnУстановитьВсеУстройстваВНуль Then
    '            ЗагрузкаПромежуточное = 0
    '        Else
    '            ЗагрузкаПромежуточное = УстройствоВЦикле.funПромежуточноеЗначениеЗагрузки(CSng(ВремяПромежуточное))
    '        End If
    '        ИсполнитьВеличинуЗагрузкиОдногоУстройства(УстройствоВЦикле)
    '    Next
    'End Sub

    'Private Sub ИсполнитьВеличинуЗагрузкиОдногоУстройства(ByRef УстройствоВЦикле As Device)
    '    'отобразить на индикаторе величину ЗагрузкаПромежуточное
    '    'получить величину загрузки по индексу из коллекции
    '    Dim cВеличинаЗагрузки As ВеличинаЗагрузки = УстройствоВЦикле.ВсеВеличиныЗагрузок(ЗагрузкаПромежуточное)

    '    For Each сБитПорта As БитПорта In cВеличинаЗагрузки.ВсеБитыПорта.Values
    '        'если есть определение включения бита по условию (срабатывание по логике = False)
    '        Select Case УстройствоВЦикле.НомерПорта
    '            Case 0 To 3
    '                'СуммироватьБиты(valPort0, УстройствоВЦикле.НомерПорта, сБитПорта.НомерБита, УстройствоВЦикле.ВысокийУровеньПорта, сБитПорта.ЗначениеЛогики)
    '                СуммироватьБиты(valPort1, сБитПорта.НомерБита, УстройствоВЦикле.ВысокийУровеньПорта, сБитПорта.ЗначениеЛогики)

    '                UsePort0 = True
    '                Exit Select
    '                'Case 4
    '                '    If сБитПорта.СрабатываниеПоЛогике Then
    '                '        СуммироватьБиты(valPort1, сБитПорта.НомерБита, УстройствоВЦикле.ВысокийУровеньПорта, сБитПорта.ЗначениеЛогики)
    '                '    Else 'бит срабатывает от значения параметра
    '                '        СуммироватьБиты(valPort1, сБитПорта.НомерБита, УстройствоВЦикле.ВысокийУровеньПорта, сБитПорта.ЗначениеПараметраПоСети, сБитПорта.ИндексКаналаВМассиве, сБитПорта.НижнийПределВключения, сБитПорта.ВерхнийПределВыключения)
    '                '    End If ' сБитПорта.СрабатываниеПоЛогике
    '                '    UsePort1 = True
    '                '    Exit Select
    '                'Case 6 To 7
    '                'для платы 6224 6229 порт 
    '                'Dev1/port0/line0 through Dev1/port0/line31
    '                'Dev1/port1/line0 through Dev1/port1/line7
    '                'Dev1/port2/line0 through Dev1/port2/line7
    '        End Select
    '    Next 'сБитПорта
    '    Dim TempLed As NationalInstruments.UI.WindowsForms.Led
    '    Try
    '        WriterPort0.WriteSingleSamplePort(True, CType(valPort0, UInt32)) '  Long.ToUInt32(valPort0))
    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.ToString)
    '        If Not DigitalWriteTaskPort0 Is Nothing Then
    '            DigitalWriteTaskPort0.Dispose()
    '            DigitalWriteTaskPort0 = Nothing
    '        End If
    '    End Try
    '    'For НомерПорта As Integer = 0 To 3
    '    '    For НомерБита As Integer = 0 To 7
    '    '        TempLed = CType(TableLayoutPanelPort.Controls.Item("TableLayoutPanelPort" & НомерПорта.ToString).Controls.Item("LedLedPort" & НомерПорта.ToString & "Line" & НомерБита.ToString), NationalInstruments.UI.WindowsForms.Led)
    '    '        If (valPort0 And arrСтепениДвойки(НомерБита + НомерПорта * 8)) <> 0 Then
    '    '            If TempLed.Value <> True Then TempLed.Value = True
    '    '        Else
    '    '            If TempLed.Value <> False Then TempLed.Value = False
    '    '        End If
    '    '    Next НомерБита
    '    'Next НомерПорта
    'End Sub


    'Private Sub СуммироватьБиты(ByRef valPort As Long, ByVal НомерПорта As Integer, ByVal НомерБита As Integer, ByVal ВысокийУровеньПорта As Integer, ByVal ЗначениеЛогики As Boolean)
    '    'здесь добавка УстройствоВЦикле.НомерПорта * 8 добавляет сдвиг на 8 позиций влево
    '    'If ВысокийУровеньПорта = 1 Then
    '    If ЗначениеЛогики = True Then
    '        valPort += 1L << НомерБита + НомерПорта * 8
    '    End If
    '    'Else
    '    'If ЗначениеЛогики = False Then
    '    '    valPort += 1L << НомерБита + НомерПорта * 8
    '    'End If
    '    'End If
    'End Sub

    'Private Sub СуммироватьБиты(ByRef valPort As Long, ByVal НомерБита As Integer, ByVal ВысокийУровеньПорта As Integer, ByVal ЗначениеЛогики As Boolean)
    '    'If ВысокийУровеньПорта = 1 Then
    '    If ЗначениеЛогики = True Then
    '        valPort += 1L << НомерБита
    '    End If
    '    'Else
    '    'If ЗначениеЛогики = False Then
    '    '    valPort += 1L << НомерБита
    '    'End If
    '    'End If
    'End Sub


    'Private DigitalWriteTaskPort0 As Task
    'Private WriterPort0 As DigitalSingleChannelWriter
    'Private УстройстваВЦиклеДляИсполнения As Dictionary(Of String, PortDevice)


    'SCXI and SCC Physical Channels 
    'SC1Mod1 is the default device name for an SCXI module, where SC1 is the default chassis ID, and Mod1 refers to the slot number. 
    'These names can be changed in MAX.

    'Analog Input
    'An SCXI module usually has eight or 32 analog input channels; refer to your device documentation to be sure. 
    'These physical channel names are of the form SC1Mod<slot#>/ai0 to SC1Mod<slot#>/aiN, where <slot#> is the chassis slot number of the module, 
    'and N equals the number of analog input channels on the module minus one. For example, SCI1Mod1/ai31 is the highest numbered physical channel for a module with 32 analog input channels.

    'An SCC module has either one or two physical channels named SCC1Mod<J connector#>aiN, 
    'where <J connector#> is the number of the J connector where the SCC module resides, and N is the channel number. 
    'SCC1 is the SCC connector block ID (for example, SCC1Mod1/ai0).

    'NI PXI-4224 Only—You cannot scan channel ai7 and the CJC channel simultaneously in a task, since the CJC channel is multiplexed to channel 7. However, when you make a thermocouple measurement on ai0:7 with internal CJC, NI-DAQmx automatically reads the CJC channel at the beginning of the measurement and then scans the rest of the channels correctly.

    'Analog Output
    'An SCXI module has some number of output channels for voltage or current. These physical channel names are of the form SC1Mod<slot#>/ao0 to SC1Mod<slot#>/aoN, where <slot#> is the chassis slot number of the module, and N equals the number of analog output channels on the module minus one. For example, SC1Mod1/ao5 is the highest numbered physical channel on a module with six channels.

    'Digital Input and Output
    'An SCXI digital module has eight, 16, or 32 lines named SC1Mod<slot#>/port0/line0 through SC1Mod<slot#>/port0/lineN, 
    'where <slot#> is the chassis slot number of the module, and N is the number of digital lines minus one. 
    'For example, SC1Mod1/port0/line31 is the highest numbered line for a module with 32 lines. 
    'These lines belong to a single port and the physical channel named SC1Mod<slot#>/port0 refers to all the lines at once.

    'An SCC module has either one digital input line or one digital output line with names of the form SCC1Mod<J connector#>diN or SCC1Mod<J connector#>diN, 
    'where <J connector#> is the number of the J connector where the SCC module resides, and N is the channel number. 
    'SCC1 is the SCC connector block ID (for example, SCC1Mod1/di0).

    'Public analogCallback As AsyncCallback

    'Public Sub AnalogInCallback(ByVal ar As IAsyncResult)
    '    Try
    '        If taskRunning = True Then
    '            'закоментировал
    '            'data = analogInReader.EndReadMultiSample(ar)

    '            'Plot your data here
    '            'закоментировал
    '            'dataToDataTable(data, DataTable)

    '            'Label1.Text = data(31, 0)
    '            fMainForm.Label2.Text = analogInReader.EndReadMultiSample(ar).GetValue(31, 0)

    '            'analogInReader.BeginReadMultiSample(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, Nothing)
    '            analogInReader.BeginReadMultiSample(4, analogCallback, Nothing)
    '            'System.Threading.Thread.Sleep(10)
    '        End If

    '    Catch ex As DaqException
    '        MessageBox.Show(ex.ToString)
    '        taskRunning = False
    '        myTask.Dispose()
    '        fMainForm.stopButton.Enabled = False
    '        fMainForm.startButton.Enabled = True
    '    End Try
    'End Sub

    'Private Function dataToDataTable(ByVal sourceArray As Double(,), ByRef dataTable As DataTable)
    '    Try
    '        Dim channelCount = sourceArray.GetLength(0)
    '        Dim dataCount As Integer

    '        If sourceArray.GetLength(1) < 10 Then
    '            dataCount = sourceArray.GetLength(1)
    '        Else
    '            dataCount = 10
    '        End If

    '        Dim currentDataIndex As Int16
    '        Dim currentChannelIndex As Int16

    '        For currentDataIndex = 0 To (dataCount - 1)
    '            For currentChannelIndex = 0 To (channelCount - 1)
    '                dataTable.Rows(currentDataIndex)(currentChannelIndex) = sourceArray.GetValue(currentChannelIndex, currentDataIndex)
    '            Next
    '        Next
    '    Catch e As System.Exception
    '        MessageBox.Show(e.TargetSite.ToString())
    '        taskRunning = False
    '        myTask.Dispose()
    '        stopButton.Enabled = False
    '        startButton.Enabled = True
    '    End Try
    'End Function

    'Public Function InitializeDataTable(ByVal channelCollection As AIChannelCollection, ByRef data As DataTable)
    '    Dim numOfChannels As Int16 = channelCollection.Count
    '    data.Rows.Clear()
    '    data.Columns.Clear()
    '    dataColumn = New DataColumn(numOfChannels) {}
    '    Dim numOfRows As Int16 = 10
    '    Dim currentChannelIndex As Int16 = 0
    '    Dim currentDataIndex As Int16 = 0

    '    For currentChannelIndex = 0 To (numOfChannels - 1)
    '        dataColumn(currentChannelIndex) = New DataColumn
    '        dataColumn(currentChannelIndex).DataType = System.Type.GetType("System.Double")
    '        dataColumn(currentChannelIndex).ColumnName = channelCollection(currentChannelIndex).PhysicalName
    '    Next

    '    data.Columns.AddRange(dataColumn)

    '    For currentDataIndex = 0 To (numOfRows - 1)
    '        Dim rowArr As Object() = New Object(numOfChannels - 1) {}
    '        data.Rows.Add(rowArr)
    '    Next
    'End Function


    'Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If taskRunning = False Then
    '        Try
    '            stopButton.Enabled = True
    '            startButton.Enabled = False
    '            taskRunning = True

    '            'Create a new task
    '            myTask = New Task("aiTask")

    '            'Create a virtual channel
    '            'myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", _
    '            '    CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
    '            '    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)

    '            'myTask.AIChannels.CreateVoltageChannel("Dev1/ai0:7", "", _
    '            '    CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
    '            '    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)
    '            'SC1Mod1/ai31
    '            'SC1Mod<slot#>/ao0 to SC1Mod<slot#>/aoN,
    '            '                Dev0/ai0:Dev0/ai4
    '            'Dev0/ai0:Dev0/ai4
    '            '"SC1Mod2/ai0:SC1Mod2/ai31"
    '            Dim I, J As Integer
    '            For I = 1 To 12
    '                'myTask.AIChannels.CreateVoltageChannel("SC1Mod" & I.ToString & "/ai0:31", "", _
    '                '    CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
    '                '    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)
    '                'myTask.AIChannels.CreateVoltageChannel("SC1Mod" & I.ToString & "/ai0:31", "", _
    '                '    CType(-1, AITerminalConfiguration), -10, _
    '                '    10, AIVoltageUnits.Volts)
    '                For J = 0 To 31
    '                    myTask.AIChannels.CreateVoltageChannel("SC1Mod" & I.ToString & "/ai" & J.ToString, "", _
    '                    CType(-1, AITerminalConfiguration), 0, 5, AIVoltageUnits.Volts)
    '                Next
    '            Next

    '            'myTask.AIChannels.CreateVoltageChannel("SC1Mod2/ai0:31", "", _
    '            '    CType(-1, AITerminalConfiguration), Convert.ToDouble(minimumValueNumeric.Value), _
    '            '    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts)


    '            'myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value), _
    '            '    SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples)
    '            myTask.Timing.ConfigureSampleClock("", 80, _
    '                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples)

    '            'Verify the Task
    '            myTask.Control(TaskAction.Verify)

    '            analogInReader = New AnalogMultiChannelReader(myTask.Stream)
    '            analogInReader.SynchronizingObject = Me
    '            analogCallback = New AsyncCallback(AddressOf AnalogInCallback)

    '            'Prepare the table for Data
    '            'InitializeDataTable(myTask.AIChannels, dataTable)
    '            'acquisitionDataGrid.DataSource = dataTable

    '            'analogInReader.BeginReadMultiSample(Convert.ToInt32(samplesPerChannelNumeric.Value), analogCallback, Nothing)
    '            analogInReader.BeginReadMultiSample(4, analogCallback, Nothing)

    '        Catch ex As DaqException
    '            MessageBox.Show(exception.Message)
    '            taskRunning = False
    '            stopButton.Enabled = False
    '            startButton.Enabled = True
    '            myTask.Dispose()
    '        End Try

    '    End If
    'End Sub

End Module