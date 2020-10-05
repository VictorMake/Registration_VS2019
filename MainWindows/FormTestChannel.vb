Imports NationalInstruments.Analysis.SpectralMeasurements
Imports NationalInstruments.Analysis.Dsp
Imports NationalInstruments.Analysis.Math
Imports NationalInstruments.DAQmx
Imports System.Threading.Thread

Public Class FormTestChannel
    Private mParentForm As Form

    Public WriteOnly Property ParentFormBase() As Form
        Set(ByVal Value As Form)
            mParentForm = Value
            If TypeOf mParentForm Is FormMain Then
                ComboBoxParameters.SelectedIndex = CType(mParentForm, FormMain).ComboBoxSelectAxis.SelectedIndex + 1
            ElseIf TypeOf mParentForm Is FormTarir Then
                ComboBoxParameters.SelectedIndex = CType(mParentForm, FormTarir).ComboBoxParameters.SelectedIndex
            End If
        End Set
    End Property

    ''' <summary>
    ''' Запуск
    ''' </summary>
    ''' <returns></returns>
    Public Property Running() As Boolean

    ' переменные к осциллографу
    Private trigState As Integer
    Private isRunningOscill As Boolean
    Private trigCap(3) As String
    ' мои переменные
    Private selIndexChannel As Integer = 1
    Private isFormLoaded As Boolean = False
    ' спектр
    Private reader As AnalogSingleChannelReader
    Private samplingRate As Double = 1024
    Private samplesPerChannel As Integer = 1024
    Private autoPowerSpectrum() As Double
    Private searchFrequency As Double
    Private equivalentNoiseBandwidth As Double
    Private coherentGain As Double
    Private df As Double

    Private analogCallback As AsyncCallback
    Private isNeedEventxyCursor_AfterMove As Boolean = False
    Private isOccurringOnceOscillograph As Boolean = False ' однократный Сбор Осциллографа
    Private vScaleValue As Double = 0.1
    Private hScaleValue As Double = 100
    Private data() As Double

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().

        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' из предварительно считанного из базы в массив всех установок параметров
        ' считывание и занесение в ComboBoxParameters
        PopulateComboBoxParameters()
        ComboBoxParameters.SelectedIndex = 0 'по умолчанию первый злемент активный
        trigCap(0) = "Однокр"
        trigCap(1) = "Непрер"
        trigCap(2) = "Останов"
        isFormLoaded = True
    End Sub

    ''' <summary>
    ''' Занесение в ComboBox пары имя-индекс иконки.
    ''' </summary>
    Private Sub PopulateComboBoxParameters()
        ' из предварительно считанного из базы в массив всех установок параметров
        ' считывание и занесение
        ComboBoxParameters.Items.Clear()

        For I As Integer = 1 To UBound(ParametersType)
            If CBool(InStr(1, UnitOfMeasureString, ParametersType(I).UnitOfMeasure)) Then
                ComboBoxParameters.Items.Add(New StringIntObject(ParametersType(I).NameParameter, Array.IndexOf(UnitOfMeasureArray, ParametersType(I).UnitOfMeasure)))
            Else
                ComboBoxParameters.Items.Add(New StringIntObject(ParametersType(I).NameParameter, 1))
            End If
        Next
    End Sub

    Private Sub ComboBoxParameters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxParameters.SelectedIndexChanged
        If isOccurringOnceOscillograph AndAlso isFormLoaded = False Then StopOscillograph()
        If AcquisitionStateSwitch.Value = True Then AcquisitionStateSwitch.Value = False
        If isFormLoaded Then ConfigurationChannel()
    End Sub

#Region "DrawItem"
    Private Sub ComboBoxParameters_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ComboBoxParameters.DrawItem
        DrawItemComboBox(sender, e, Nothing, ImageListChannel, False)
    End Sub

    Private Sub ComboBoxParameters_MeasureItem(sender As Object, e As MeasureItemEventArgs) Handles ComboBoxParameters.MeasureItem
        Dim cb As ComboBox = CType(sender, ComboBox)
        e.ItemHeight = cb.ItemHeight - 2
    End Sub

#End Region

    Private Sub ButtonHide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonHide.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Закрытие окна опроса параметров")
        StopOscillograph()
        InstallQuestionForm(WhoIsExamine.Nobody)
        Hide()
        RunRegistrationForm()
    End Sub

    Private Sub FormTestChannel_KeyDown(ByVal eventSender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        Dim I As Integer
        ' Прибегнуть к ctrl+tab, чтобы двигаться к следующей закладке
        'If e.KeyCode = Keys.Tab AndAlso (e.Alt OrElse e.Control OrElse e.Shift) Then'работает
        If e.KeyCode = Keys.Tab AndAlso e.Modifiers = Keys.Control Then
            I = TabControlTools.SelectedIndex + 1 'SelectedItem.Index
            If I >= TabControlTools.TabPages.Count Then 'TabCount Then 'TabОпции.Tabs.Count
                ' Последняя закладка, так что надо перейти к закладке 1
                'tbsИнструмент.SelectedTab = tbsИнструмент.TabPages(0)
                'Else
                ' Увеличить закладку
                I = 0
            End If

            TabControlTools.SelectedIndex = I
            'tbsИнструмент.SelectedTab = tbsИнструмент.TabPages(I)
        End If

        '' определить нажатие F1 для показа помощи.
        'If e.KeyCode = Keys.F1 AndAlso (e.Alt OrElse e.Control OrElse e.Shift) Then
        '    ' показать помощь
        '    Help.ShowPopup(TextBox1, "Enter your name.", New Point(TextBox1.Bottom, TextBox1.Right))
        'ElseIf e.KeyCode = Keys.F2 AndAlso e.Modifiers = Keys.Alt Then
        '    Help.ShowPopup(TextBox1, "Enter your first name followed by your last name. Middle name is optional.", _
        '         New Point(TextBox1.Top, Me.TextBox1.Left))
        'End If
    End Sub

    ''' <summary>
    ''' какой был запущен прибор?
    ''' </summary>
    Private Sub ChangeTool()
        ' если прибор был включен, остановить и очистить буфера
        ' сделать инициализацию при переключении закладок
        ' Инициализировать переменные и форму
        ComboBoxParameters_SelectedIndexChanged(ComboBoxParameters, New EventArgs) ' вызвать искуственно смену параметра

        Select Case TabControlTools.SelectedIndex
            Case 0
                Exit Select
            Case 1
                If isOccurringOnceOscillograph = False Then StopOscillograph()

                trigState = 0
                'TriggerLevelText.Text = Format(TriggerLevel.Value, "#.0 V")
                HScaleText.Text = HScale.CustomDivisions.Item(CInt(HScale.Value)).Text
                VScaletext.Text = VScale.CustomDivisions.Item(CInt(VScale.Value)).Text
                SetVScale()

                Exit Select
            Case 2
                If AcquisitionStateSwitch.Value = True Then AcquisitionStateSwitch.Value = False
                Exit Select
        End Select

        Running = False
    End Sub

    Private Sub TabControlTools_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TabControlTools.Click
        ChangeTool()
    End Sub

    ''' <summary>
    ''' Конфигурация Канала
    ''' </summary>
    Public Sub ConfigurationChannel()
        ' смена конфигурации при измененнии параметра
        selIndexChannel = ComboBoxParameters.SelectedIndex + 1
        If selIndexChannel = 0 Then
            ComboBoxParameters.SelectedIndex = 0
            selIndexChannel = 1
        End If
    End Sub

#Region "вольтметр"
    Private Sub Acquire_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Acquire.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Опрос канала вольтметра " & selIndexChannel)

        If Not CheckMeasurementChannels(selIndexChannel) Then
            Const caption As String = "Опрос канала"
            Const text As String = "Параметр не подлежит опросу!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Exit Sub
        End If

        Const samplingRate As Double = 1000
        Const samples As Integer = 1000
        Dim volts() As Double
        Dim reader As AnalogSingleChannelReader
        Dim average As Double

        ' инициализация
        If IsTaskRunning = False Then
            Cursor = Cursors.WaitCursor
            Acquire.Enabled = False

            Try
                IsTaskRunning = True
                ' создать задачу
                If Not DAQmxTask Is Nothing Then
                    DAQmxTask.Dispose()
                    DAQmxTask = Nothing
                    'Else
                    '    myTask = New Task("aiTask")
                End If
                DAQmxTask = New Task("aiTask")
                ' создать виртуальный канал
                CreateAiChannel(ParametersType(selIndexChannel).NumberChannel.ToString,
                                ParametersType(selIndexChannel).NumberDevice.ToString,
                                ParametersType(selIndexChannel).NumberModuleChassis.ToString,
                                ParametersType(selIndexChannel).NumberChannelModule.ToString,
                                Convert.ToDouble(ParametersType(selIndexChannel).LowerMeasure),
                                Convert.ToDouble(ParametersType(selIndexChannel).UpperMeasure),
                                ParametersType(selIndexChannel).TypeConnection,
                                ParametersType(selIndexChannel).SignalType)
                DAQmxTask.Timing.ConfigureSampleClock("", samplingRate,
                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, samples)
                ' проверить задачу
                DAQmxTask.Control(TaskAction.Verify)
                'reader.SynchronizingObject = Me
                reader = New AnalogSingleChannelReader(DAQmxTask.Stream) With {
                    .SynchronizeCallbacks = True
                }
                volts = reader.ReadMultiSample(samples)
                IsTaskRunning = False
                average = Statistics.Mean(volts) 'здесь можно применить фильтр
                CWNumEdit1.Text = Format(average, "##0.0####")
                ' расчет физики
                CWNumEdit2.Text = Format(PhysicalValue(selIndexChannel, average), "##0.0####")
                CWGraph1.PlotY(volts)
            Catch ex As DaqException
                Const caption As String = "Acquire_Click"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                IsTaskRunning = False
                DAQmxTask.Dispose()
            Finally
                Cursor = Cursors.Default
                Acquire.Enabled = True
            End Try
        End If
    End Sub
#End Region

#Region "блок к осциллографу"
    Private Sub ResetOsc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ResetOsc.Click
        ' Перезапуска (обычно используемый только с единственным триггером)
        Start_Oscill()
    End Sub

    Private Sub Trigger_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Trigger.Click
        ' Выбор между тремя способами триггера
        ' TrigSatet переменна следит за текущим способом

        trigState += 1
        If trigState = 3 Then trigState = 0
        'Trigger.OffText = TrigCap(TrigState)
        'Trigger.OnText = TrigCap(TrigState)
        If trigState = 0 Then 'неактивен
            StopOscillograph()
        ElseIf trigState = 1 Then  ' непрерывный
            isOccurringOnceOscillograph = False
            Start_Oscill()
            Running = True
        Else
            StopOscillograph()
            isOccurringOnceOscillograph = True
            Start_Oscill()
            Running = False
        End If
    End Sub

    Private Sub HScale_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles HScale.AfterChangeValue
        ' При изменении разрешающей способности по вертикале, перепрограммирование и перезапуск
        HScaleText.Text = HScale.CustomDivisions.Item(CInt(HScale.Value)).Text
        'HScaleText.Text = HScale.Axis.ValuePairs.Item(HScale.ValuePairIndex).Name
        Select Case e.NewValue
            Case 0
                HScaleText.BackColor = Color.Lime
                hScaleValue = 100
                Exit Select
            Case 1
                HScaleText.BackColor = Color.Lime
                hScaleValue = 50
                Exit Select
            Case 2
                HScaleText.BackColor = Color.Lime
                hScaleValue = 20
                Exit Select
            Case 3
                HScaleText.BackColor = Color.Lime
                hScaleValue = 10
                Exit Select
            Case 4
                HScaleText.BackColor = Color.Yellow
                hScaleValue = 5
                Exit Select
            Case 5
                HScaleText.BackColor = Color.Yellow
                hScaleValue = 2
                Exit Select
            Case 6
                HScaleText.BackColor = Color.Red
                hScaleValue = 1
                Exit Select
        End Select

        If isRunningOscill = True Then
            Start_Oscill()
        End If
    End Sub

    Private Sub VOffset_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles VOffset.AfterChangeValue
        ' Изменение вертикального масштаба графика
        SetVScale()
    End Sub

    'Private Sub TOffset_AfterChangeValue(ByVal sender As System.Object, ByVal e As NationalInstruments.UI.AfterChangeNumericValueEventArgs) Handles TOffset.AfterChangeValue
    '    fMainForm.CWAI1.StopCondition.PreTriggerScans = TOffset.Value
    '    If blnФормаЗагружена Then Start_Oscill()
    'End Sub

    'Private Sub TriggerLevel_AfterChangeValue(ByVal sender As System.Object, ByVal e As NationalInstruments.UI.AfterChangeNumericValueEventArgs) Handles TriggerLevel.AfterChangeValue
    '    ' Если аналоговый триггер выбран, остановка, перепрограммирование, и перезапуск
    '    TriggerLevelText.Text = Format(TriggerLevel.Value, "#.0 V")
    '    If RunningOscill = True And TrigState = 2 Then
    '        Start_Oscill()
    '    End If
    'End Sub

    Private Sub VScale_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles VScale.AfterChangeValue
        ' Изменение вертикального масштаба графика
        'VScaletext.Text = VScale.CustomDivisions.Item(VScale.Value).Text
        VScaletext.Text = VScale.CustomDivisions.Item(CInt(e.NewValue)).Text

        Select Case e.NewValue
            Case 0
                VScaletext.BackColor = Color.Red
                vScaleValue = 0.1
                Exit Select
            Case 1
                VScaletext.BackColor = Color.Yellow
                vScaleValue = 0.2
                Exit Select
            Case 2
                VScaletext.BackColor = Color.Yellow
                vScaleValue = 0.5
                Exit Select
            Case 3
                VScaletext.BackColor = Color.Lime
                vScaleValue = 1
                Exit Select
            Case 4
                VScaletext.BackColor = Color.Lime
                vScaleValue = 2
                Exit Select
            Case 5
                VScaletext.BackColor = Color.Lime
                vScaleValue = 5
                Exit Select
        End Select

        SetVScale()
    End Sub

    Private Sub SetVScale()
        ' Установки реквизитов на вертикальной оси графика согласно установкам пользователя
        VOffsetText.Text = CStr(Math.Round(VOffset.Value * vScaleValue, 2)) & " V"
        'VScaletext.Text = VScale.Axis.ValuePairs.Item(VScale.ValuePairIndex).Name
        CWGraph2.YAxes(0).Range = New Range(CDbl(vScaleValue * -2 + VOffset.Value * vScaleValue), CDbl(vScaleValue * 2 + VOffset.Value * vScaleValue))
    End Sub

    Private Sub Start_Oscill()
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Опрос канала осциллографа " & selIndexChannel)

        If Not CheckMeasurementChannels(selIndexChannel) Then
            'MessageBox.Show("Параметр не подлежит опросу!", "Опрос канала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Try
            ' сбор включён
            'If (acquisitionStateSwitch.Value And Not (taskRunning)) Then
            If Not IsTaskRunning OrElse isRunningOscill = True Then

                IsTaskRunning = True
                'Me.blnЗапуск = True
                isRunningOscill = True

                ' создать новую задачу
                If Not DAQmxTask Is Nothing Then
                    'myTask.Stop()
                    'Sleep(100)
                    DAQmxTask.Dispose()
                    DAQmxTask = Nothing
                    'Else
                    '    myTask = New Task("aiTask")
                End If
                TriggerLED.Value = Not isOccurringOnceOscillograph

                DAQmxTask = New Task("aiTask")

                samplingRate = 20000 / (hScaleValue)
                samplesPerChannel = 200

                CreateAiChannel(ParametersType(selIndexChannel).NumberChannel.ToString, ParametersType(selIndexChannel).NumberDevice.ToString, ParametersType(selIndexChannel).NumberModuleChassis.ToString, ParametersType(selIndexChannel).NumberChannelModule.ToString,
                Convert.ToDouble(ParametersType(selIndexChannel).LowerMeasure), Convert.ToDouble(ParametersType(selIndexChannel).UpperMeasure), ParametersType(selIndexChannel).TypeConnection, ParametersType(selIndexChannel).SignalType)

                DAQmxTask.Timing.ConfigureSampleClock("", samplingRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannel)

                ' проверить задачу
                DAQmxTask.Control(TaskAction.Verify)

                IsTaskRunning = True
                'reader.SynchronizingObject = Me
                reader = New AnalogSingleChannelReader(DAQmxTask.Stream) With {.SynchronizeCallbacks = True}
                analogCallback = New AsyncCallback(AddressOf myCallbackOSC) 'my
                DAQmxTask.Start()
                reader.BeginReadMultiSample(samplesPerChannel, analogCallback, Nothing)
                ' сбор выключить
                'Else
                'If taskRunning Then
                '    taskRunning = False
                '    Me.blnЗапуск = False
                '    myTask.Dispose()
                '    СобытиеxyCursor_AfterMoveНужно = True
                'End If
            End If

        Catch ex As DaqException
            Const caption As String = "Конфигурация осциллографа"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            IsTaskRunning = False
            DAQmxTask.Dispose()
        End Try
    End Sub

    Private Sub myCallbackOSC(ByVal ar As IAsyncResult)
        Try
            If IsTaskRunning Then
                CWGraph2.PlotY(reader.EndReadMultiSample(ar))
                If isOccurringOnceOscillograph = False Then
                    reader.BeginReadMultiSample(samplesPerChannel, analogCallback, Nothing)
                Else
                    IsTaskRunning = False
                End If
            End If
        Catch ex As DaqException
            Const caption As String = "Сбор осциллографа"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            StopOscillograph()
            Start_Oscill()
            'taskRunning = False
            'myTask.Dispose()
        End Try
    End Sub

    Private Sub StopOscillograph()
        isRunningOscill = False
        Try
            If IsTaskRunning Then
                TriggerLED.Value = False
                IsTaskRunning = False
                Running = False
                DAQmxTask.Stop()
                Sleep(100)
                DAQmxTask.Dispose()
            End If

        Catch ex As DaqException
            Const caption As String = "Остановка осциллографа"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            IsTaskRunning = False
            DAQmxTask.Dispose()
        End Try

        Running = False
    End Sub
#End Region

#Region "спектроанализатор"
    Private Sub AcquisitionStateSwitch_StateChanged(ByVal sender As Object, ByVal e As ActionEventArgs) Handles AcquisitionStateSwitch.StateChanged
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Опрос канала спектроанализатора " & selIndexChannel)
        If Not CheckMeasurementChannels(selIndexChannel) AndAlso AcquisitionStateSwitch.Value Then
            Const caption As String = "Опрос канала"
            Const text As String = "Параметр не подлежит опросу!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            AcquisitionStateSwitch.Value = False
            Exit Sub
        End If

        Try
            ' сбор включён
            If (AcquisitionStateSwitch.Value AndAlso Not (IsTaskRunning)) Then
                IsTaskRunning = True
                Running = True
                isNeedEventxyCursor_AfterMove = False

                ' создать новую задачу
                If Not DAQmxTask Is Nothing Then
                    'myTask.Stop()
                    'Sleep(100)
                    DAQmxTask.Dispose()
                    DAQmxTask = Nothing
                    'Else
                    '    myTask = New Task("aiTask")
                End If

                DAQmxTask = New Task("aiTask")

                samplingRate = rateNumeric.Value
                samplesPerChannel = Convert.ToInt16(samplesNumeric.Value)
                WaveformGraph.XAxes.Item(0).Range = New Range(0, samplesPerChannel)
                powerSpectrumGraph.XAxes.Item(0).Range = New Range(0, samplingRate / 2)

                CreateAiChannel(ParametersType(selIndexChannel).NumberChannel.ToString, ParametersType(selIndexChannel).NumberDevice.ToString, ParametersType(selIndexChannel).NumberModuleChassis.ToString, ParametersType(selIndexChannel).NumberChannelModule.ToString,
                Convert.ToDouble(ParametersType(selIndexChannel).LowerMeasure), Convert.ToDouble(ParametersType(selIndexChannel).UpperMeasure), ParametersType(selIndexChannel).TypeConnection, ParametersType(selIndexChannel).SignalType)

                DAQmxTask.Timing.ConfigureSampleClock("", samplingRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannel)

                ' проверить задачу
                DAQmxTask.Control(TaskAction.Verify)

                IsTaskRunning = True
                'reader.SynchronizingObject = Me
                reader = New AnalogSingleChannelReader(DAQmxTask.Stream) With {.SynchronizeCallbacks = True}
                analogCallback = New AsyncCallback(AddressOf myCallback) 'my
                reader.BeginReadMultiSample(samplesPerChannel, analogCallback, Nothing)
                ' выключить сбор
            Else
                If IsTaskRunning Then
                    IsTaskRunning = False
                    Running = False
                    DAQmxTask.Stop()
                    Sleep(100)
                    DAQmxTask.Dispose()
                    isNeedEventxyCursor_AfterMove = True
                End If
            End If

        Catch ex As DaqException
            Const caption As String = "Конфигурация спектроанализатора"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            IsTaskRunning = False
            DAQmxTask.Dispose()
        End Try
    End Sub

    Private Sub myCallback(ByVal ar As IAsyncResult)
        Try
            If IsTaskRunning Then
                data = reader.EndReadMultiSample(ar)
                WaveformGraph.PlotY(data)
                GetUnitConvertedAutoPowerSpectrum(data)  ' получить график спектральной мощности сигнала. 
                ' запустить функцию вычисления powerPeak и frequencyPeak.
                CurrentPeakData()
                ' продолжить сбор данных до тех пор, пока задача запущена
                reader.BeginReadMultiSample(samplesPerChannel, analogCallback, Nothing)
            End If

        Catch ex As DaqException
            Const caption As String = "Сбор спектроанализатора"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            IsTaskRunning = False
            DAQmxTask.Dispose()
        End Try
    End Sub

    Private Sub GetUnitConvertedAutoPowerSpectrum(ByRef waveform() As Double)
        Dim unitConvertedSpectrum() As Double
        Dim subsetOfUnitConvertedSpectrum() As Double = New Double(samplesPerChannel \ 2 - 1) {}
        'Dim winType As ScaledWindowType = ScaledWindowType.FlatTop
        Dim scaleMode As ScalingMode = ScalingMode.Linear
        Dim unitOfdisplay As DisplayUnits = DisplayUnits.VoltsRms

        ' установить тип окна выбранный пользователем.
        Dim scaleWindow As ScaledWindow = Nothing

        Select Case window.SelectedIndex
            Case 0
                scaleWindow = ScaledWindow.CreateHanningWindow()
                Exit Select
            Case 1
                scaleWindow = ScaledWindow.CreateRectangularWindow()
                Exit Select
            Case 2
                scaleWindow = ScaledWindow.CreateHanningWindow()
                Exit Select
            Case 3
                scaleWindow = ScaledWindow.CreateBlackmanHarrisWindow()
                Exit Select
            Case 4
                scaleWindow = ScaledWindow.CreateExactBlackmanWindow()
                Exit Select
            Case 5
                scaleWindow = ScaledWindow.CreateBlackmanWindow()
                Exit Select
            Case 6
                scaleWindow = ScaledWindow.CreateFlatTopWindow()
                Exit Select
            Case 7
                scaleWindow = ScaledWindow.CreateBlackmanHarris4TermWindow()
                Exit Select
            Case 8
                scaleWindow = ScaledWindow.CreateBlackmanHarris7TermWindow()
                Exit Select
        End Select

        ' единица измерения выбранная пользователем для автоматического вывода спектра мощности
        Select Case units.SelectedIndex
            Case 0
                unitOfdisplay = DisplayUnits.VoltsRms
                Exit Select
            Case 1
                unitOfdisplay = DisplayUnits.VoltsRmsSquared
                Exit Select
            Case 2
                unitOfdisplay = DisplayUnits.VoltsRmsPerRootHZ
                Exit Select
            Case 3
                unitOfdisplay = DisplayUnits.VoltsPeakSquaredPerHZ
                Exit Select
            Case 4
                unitOfdisplay = DisplayUnits.VoltsPeak
                Exit Select
            Case 5
                unitOfdisplay = DisplayUnits.VoltsPeakSquared
                Exit Select
            Case 6
                unitOfdisplay = DisplayUnits.VoltsPeakPerRootHZ
                Exit Select
            Case 7
                unitOfdisplay = DisplayUnits.VoltsRmsSquaredPerHZ
                Exit Select
            Case Else
                unitOfdisplay = DisplayUnits.VoltsRms
                Exit Select
        End Select

        ' Выбранный масштаб: Linear, dB or dBm
        Select Case scalecomboBox.SelectedIndex
            Case 0
                scaleMode = ScalingMode.DB
                Exit Select
            Case 1
                scaleMode = ScalingMode.DBM
                Exit Select
            Case 2
                scaleMode = ScalingMode.Linear
                Exit Select
        End Select

        scaleWindow.Apply(waveform, equivalentNoiseBandwidth, coherentGain)
        ' Вычислить график спектра мощности
        autoPowerSpectrum = New Double(samplesPerChannel - 1) {}
        autoPowerSpectrum = Measurements.AutoPowerSpectrum(waveform, 1.0 / samplingRate, df)
        Dim unit As System.Text.StringBuilder = New System.Text.StringBuilder("V", 256)
        ' единица преобразования спектра мощности выбранная пользователем
        unitConvertedSpectrum = New Double(samplesPerChannel - 1) {}
        unitConvertedSpectrum = Measurements.SpectrumUnitConversion(autoPowerSpectrum, NationalInstruments.Analysis.SpectralMeasurements.SpectrumType.Power, scaleMode, unitOfdisplay, df, equivalentNoiseBandwidth, coherentGain, unit)
        ' установить примечание оси yAxis в соответствии с выбранной единицей дисплея.
        powerSpectrumYAxis.Caption = unit.ToString()

        'For i = 0 To samplesPerChannel / 2 - 1
        '    subsetOfUnitConvertedSpectrum(i) = unitConvertedSpectrum(i)
        'Next i

        Array.Copy(unitConvertedSpectrum, subsetOfUnitConvertedSpectrum, CLng(samplesPerChannel / 2))
        ' рисовать unitConvertedSpectrum.        
        powerSpectrumGraph.PlotY(subsetOfUnitConvertedSpectrum, 0, df)
    End Sub

    Private Sub CurrentPeakData()
        Dim frequencyPeak As Double
        Dim powerPeak As Double

        searchFrequency = XYCursor.XPosition    'задать текущую XPosition курсора.
        ' применить PowerFrequencyEstimate функцию.
        Measurements.PowerFrequencyEstimate(autoPowerSpectrum, searchFrequency, equivalentNoiseBandwidth, coherentGain, df, 7, frequencyPeak, powerPeak)
        peakFrequency.Value = frequencyPeak
        peakPower.Value = powerPeak
    End Sub
#End Region

    Private Sub XYCursor_AfterMove(ByVal sender As Object, ByVal e As AfterMoveXYCursorEventArgs) Handles XYCursor.AfterMove
        If isNeedEventxyCursor_AfterMove Then CurrentPeakData()
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxParameters)
        mSearchChannel.SelectChannel()
    End Sub
End Class