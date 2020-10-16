Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.IO
Imports System.Math
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports MathematicalLibrary

Friend MustInherit Class FormRegistrationBase
    ''' <summary>
    ''' Данные значений каналов, собранные DAQmx, или полученные по сети от Сервера
    ''' </summary>
    ''' <returns></returns>
    Friend Property DataValuesFromServer As Double()
    ''' <summary>
    ''' Выведена форма показа значений выбранных каналов
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsShowTextControl() As Boolean
    ''' <summary>
    ''' Графический контроль параметров на дополнительной форме
    ''' </summary>
    ''' <remarks></remarks>
    Friend Property IsShowGraphControl As Boolean
#Region "Boolean"
    ''' <summary>
    ''' Идет запись на диск кадра регистратора
    ''' </summary>
    Protected isRecordingSnapshot As Boolean
    ''' <summary>
    ''' Нажата кнопка Запись вручную
    ''' </summary>
    Protected isButtonRecordPressHandle As Boolean
    ''' <summary>
    ''' Вызвать событие AcquiredData
    ''' </summary>
    Protected isFireEventAcquiredData As Boolean
    ' После загрузки окна Сервера произойдёт заново запуск опроса
    ' и если была включена запись то нужно её возобновить. Флаги запоминают текущие состояния.
    Private isButtonRecordPressHandleMemo, isRecordEnableMemo, isFormRunningMemo As Boolean
    ''' <summary>
    ''' Не останавливать запись когда происходит настройка видимости шлейфов
    ''' </summary>
    Private isSkipStopRecord As Boolean
#End Region

#Region "Ссылки"
    Protected frmTextControl As FormTextControl
    Protected frmGraphControl As FormGraphControl
    Protected frmParameterInRange As FormParameterInRange
#End Region
    ''' <summary>
    ''' Список параметров в диапазоне
    ''' </summary>
    Protected listParameterInRange As String
    ''' <summary>
    ''' Число видимых шлейфов Регистратора
    ''' </summary>
    Protected countVisibleTrendRegistrator As Integer
    ''' <summary>
    ''' const Ограничение видимых шлефов
    ''' </summary>
    Private Const conLimitVisibleTrend As Integer = 16
    ''' <summary>
    ''' Массив значений всех собранных параметров по времени
    ''' </summary>
    Protected dataAllTrends As Double(,)
    ''' <summary>
    ''' Через сколько опросов обновлять цифры при регистрации
    ''' </summary>
    Protected countUpdateScreen1 As Short
    ''' <summary>
    ''' Через сколько опросов обновлять цифры при регистрации
    ''' </summary>
    Protected countUpdateScreen2 As Short
    ''' <summary>
    ''' Разрешить обновить изображение для каких-то элементов 1-го этапа
    ''' </summary>
    Protected isFireUpdateScreen1 As Boolean ' для обновления изображений
    ''' <summary>
    ''' Разрешить обновить изображение для каких-то элементов 2-го этапа
    ''' </summary>
    Protected isFireUpdateScreen2 As Boolean
    ''' <summary>
    ''' Начать подсчёт для обновления изображение для каких-то элементов 2-го этапа
    ''' </summary>
    Protected isNeedToCountForUpdateScreen2 As Boolean
    ''' <summary>
    ''' Текущая позиция курсора
    ''' </summary>
    Protected xNewPos As Double
    ''' <summary>
    ''' Сохранение текущей позиции курсора для вычисления нового значения xNewPos
    ''' </summary>
    Protected lastx As Double
    ''' <summary>
    ''' Счетчик кадров регистратора
    ''' </summary>
    Private countFrameRegistrator As Integer
    ''' <summary>
    ''' Число фактических произведённых замеров внутри кадра снимка
    ''' равен текущему положению курсора.
    ''' </summary>
    Protected countRow As Integer
    ''' <summary>
    ''' Коллекция ламп индикаторов аврийных сообщений
    ''' </summary>
    Protected AlarmPulseButton As New Dictionary(Of Integer, PulseButton.PulseButton)
    ''' <summary>
    ''' Коллекция ламп дискретных индикаторов
    ''' </summary>
    Protected listDiscreteLed As New List(Of WindowsForms.Led)
    ''' <summary>
    ''' Индекс первого дискретного индикатора
    ''' </summary>
    Protected keyFirstButtonAlarm As Integer
    ''' <summary>
    ''' Индекс последнего дискретного индикатора
    ''' </summary>
    Protected keyLastButtonAlarm As Integer
    ''' <summary>
    ''' Событие не может наследоваться, поэтому его вызывают через переопределяемый метод
    ''' все унаследованные формы вместо
    ''' Friend Event AcquiredDataFormMain(ByVal sender As Object, ByVal e As AcquiredDataEventArgs)
    ''' </summary>
    Friend Event AcquiredDataEvent As EventHandler(Of AcquiredDataEventArgs)
    ''' <summary>
    ''' Через сколько пропусков обновлять замеры относительно величины TextDisplayRate
    ''' </summary>
    Protected countTextDisplayRate As Integer
    ''' <summary>
    ''' Параметр РУД
    ''' </summary>
    Protected ARUD As Double
    ''' <summary>
    ''' Параметр обороты N2 приведенные
    ''' </summary>
    Protected N2reduced As Double
    ''' <summary>
    ''' Формат для отображения на панели инструментов
    ''' </summary>
    Protected Const FormatString As String = "##0.0"

#Region "DateTime"
    ''' <summary>
    ''' Системное время
    ''' </summary>
    Private systemTime As Date
    ''' <summary>
    ''' Временной инкремент шкалы графика
    ''' </summary>
    Private defaultInterval As PrecisionTimeSpan ' = WaveformPlot.DefaultWaveformPrecisionTiming.SampleInterval
    ''' <summary>
    ''' Время начала шкалы графика
    ''' </summary>
    Private precisionTimeStart As PrecisionDateTime
#End Region

    Friend WithEvents TableLayoutPanelDiscrete As TableLayoutPanel

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, "FormRegistrationBase")
        'InitializeComponent()
    End Sub
    Public Sub New(ByVal frmParent As FormMainMDI, ByVal kindExamination As FormExamination, ByVal captionText As String)
        MyBase.New(frmParent, kindExamination, captionText)

        ' Этот вызов является обязательным для конструктора.
        'InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
        PopulateAlarmButton(0)
        AlarmPulseButton(0).BringToFront()
    End Sub

#Region "Реализовать интерфейс"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected MustOverride Sub InheritFormLoad()
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub InheritFormClosed()
    ''' <summary>
    ''' Настройка меню и кнопок продолжение
    ''' </summary>
    Protected MustOverride Sub EnableDisableControlsInherit()
    ''' <summary>
    ''' Настройка конфигурации режима
    ''' </summary>
    Protected MustOverride Sub TuneConditionsRegime()
    ''' <summary>
    ''' Реализуется в TCP Client
    ''' </summary>
    ''' <param name="start"></param>
    Protected MustOverride Sub StartStopSocket(start As Boolean)
    ''' <summary>
    ''' Реализуется в FormRegistrationSCXI
    ''' </summary>
    Protected MustOverride Sub InitializeContinuousDAQmxTask()
    ''' <summary>
    ''' Определить удленение шкалы
    ''' Для клиента в связи с пропусками пакетов время сбора растягивается
    ''' </summary>
    ''' <returns></returns>
    Protected MustOverride Function GetLengtheningScale() As Double
    ''' <summary>
    ''' Обновление таблицы Режимы при проверке подключаемых расчётных модулей при работе TcpClient
    ''' </summary>
    Protected MustOverride Sub UpdateTableRegimeForTcpClient()
    ''' <summary>
    ''' Обновить компенсацию ХС для регистратора SCXI
    ''' </summary>
    Protected MustOverride Sub UpdateCompensationXC()
    ''' <summary>
    ''' Вызов событие базового класса посредством переопредеяемого метода
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overridable Sub OnAcquiredData(e As AcquiredDataEventArgs)
        'Dim handler As EventHandler(Of AcquiredDataEventArgs) = RaiseCustomEvent
        '' Event will be null if there are no subscribers
        'If handler IsNot Nothing Then
        '    ' Format the string to send inside the CustomEventArgs parameter
        '    e.Message += [String].Format(" at {0}", DateTime.Now.ToString())

        '    ' Use the () operator to raise the event.
        '    handler(Me, e)
        'End If

        RaiseEvent AcquiredDataEvent(Me, e)
    End Sub
    ''' <summary>
    ''' Обновление данных на экране из события сбора
    ''' </summary>
    Friend MustOverride Sub AcquiredData()
#End Region

#Region "Реализация интерфейса"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected Overrides Sub BaseFormLoad()
        CheckSolveModule()

        If Not IsUseTdms Then dataMeasuredValuesString = New StringBuilder(1000000)

        If IsFrmDigitalOutputPortStart Then
            DigitalPortForm.TSButtonStart.Enabled = True
            DigitalPortForm.FormMainReference = Me
        End If

        If gFormsPanelManager.FormPanelMotoristCount > 0 Then
            For Each itemBasePanelMotorist As FormBasePanelMotorist In gFormsPanelManager.CollectionFormPanelMotorist.Values
                itemBasePanelMotorist.MainMDIFormReference = Me
            Next
        End If

        ' обязательно в конце
        InheritFormLoad()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub BaseFormClosing(ByRef e As FormClosingEventArgs)
        ' Обязательно вначале этого метода
        InheritFormClosing(e)
        If isCancel = True Then Exit Sub ' пользователь не хочет выходить

        ' реализация далее...
        If IsFormSereverStart Then
            If ServerForm IsNot Nothing Then
                ServerForm.Close()
            End If
            IsFormSereverStart = False
        End If

        If IsFormRunning Then ' ТипИспытания <> FormExamination.RegistrationClient AndAlso
            InstallQuestionForm(WhoIsExamine.Nobody)
            If IsRecordEnable Then
                TextBoxRecordToDisc.Visible = False
                ButtonRecord.Image = My.Resources.StopSave
                ' признак закрытия
                RecordSnapshotFrameRegistrationToDisk(True)
                IsRecordEnable = False
                'If UseTdms Then
                '    'вообще-то событие отлавливается даже при закрытии окна и есть шанс записать в базу даже при закрытии окна
                '    If varTdmsFileProcessor IsNot Nothing Then
                '        varTdmsFileProcessor.CloseTDMSFile()
                '    End If
                'End If
                'If (IsWorkWithController OrElse IsTcpClient) AndAlso UseTdms Then
                'вообще-то событие отлавливается даже при закрытии окна и есть шанс записать в базу даже при закрытии окна
                If gTdmsFileProcessor IsNot Nothing Then
                    RemoveHandler gTdmsFileProcessor.ClosedTDMSFileEvent, AddressOf TdmsFilePr_ClosedTDMSFileEvent
                    RemoveHandler gTdmsFileProcessor.ContinueAccumulation, AddressOf TdmsFilePr_ContinueAccumulation
                End If
                'End If
            End If
        End If

        If IsShowFormParametersInRange Then frmParameterInRange.Close()
        If IsShowTextControl Then frmTextControl.Close()
        If IsShowGraphControl Then frmGraphControl.Close()
        'If mblnТарировкаТяги Then frmТарировкаТяги.Close()

        If IsUseCalculationModule Then
            IsUseCalculationModule = False
            MainMDIFormParent.ModuleSolveManager.RemoveAllCalculationModule()
            MainMDIFormParent.MenuConfigurationCalculationModule.Enabled = True
            MainMDIFormParent.MenuSetComPort.Enabled = True
            MainMDIFormParent.ModuleSolveManager.ParentFormMain = Nothing
        End If
    End Sub
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub BaseFormClosed()
        ' Обязательно вначале этого метода
        InheritFormClosed()
        ' реализация далее...

        If IsFormClientStart Then FormClientDataSocket.Close()

        RegistrationFormName = ""
        UnloadAlarmButton()
        Controls.Remove(CType(AlarmPulseButton(0), PulseButton.PulseButton))
        AlarmPulseButton = Nothing

        If IsFrmDigitalOutputPortStart Then
            ' вызвать процедуру сброса монитора после чего blnМониторDigitalOutputPort=false
            ' если кнопка пуск запущена то выключить
            If DigitalPortForm.TSButtonStart.Checked Then DigitalPortForm.TSButtonStart.Checked = False
            DigitalPortForm.TSButtonStart.Enabled = False
            DigitalPortForm.FormMainReference = Nothing
        End If

        RegistrationMain = Nothing
    End Sub
    ''' <summary>
    ''' Настроить график под выбранный параметр Оси У
    ''' </summary>
    Protected Overrides Sub TuneDiagramUnderSelectedParameterAxesY()
        If isFormLoaded Then
            ' выбор оси
            Dim I As Integer
            Dim indexPlot As Integer

            If isDetailedSheet Then
                For I = 1 To UBound(IndexParameters)
                    If ParametersType(IndexParameters(I)).IsVisible Then
                        indexPlot += 1

                        If SlidePlot.Value = indexPlot Then
                            'cmbВыборОси.Text = arrПараметры(arrIndexParameters(I)).strНаименованиеПараметра
                            Exit For
                        End If
                    End If
                Next
            Else
                For I = 1 To UBound(IndexParameters)
                    If SnapshotSmallParameters(I).IsVisible Then
                        indexPlot += 1

                        If SlidePlot.Value = indexPlot Then
                            'cmbВыборОси.Text = myTypeList(I).strНаименованиеПараметра
                            Exit For
                        End If
                    End If
                Next
            End If

            NumberParameterAxes = I
            numberParameterAxesОriginal = indexPlot

            If ComboBoxSelectAxis.Items.Count = 1 OrElse ComboBoxSelectAxis.Items.Count < NumberParameterAxes Then
                ComboBoxSelectAxis.SelectedIndex = 0
                numberParameterAxesОriginal = 1
                NumberParameterAxes = 1
            Else
                ComboBoxSelectAxis.SelectedIndex = NumberParameterAxes - 1
            End If

            SlidePlot.PointerColor = ColorsNet((I - 1) Mod 7)

            If IsFormRunning Then
                YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))
            End If

            LabelIndicator.Text = "Ось Y по " & ComboBoxSelectAxis.Text
            ' выделить выбранный график
            WaveformGraphTime.Plots.Item(numberParameterAxesОriginal - 1).LineWidth = 2

            ' numberParameterAxesChanged может и не быть в данном снимке
            Try
                WaveformGraphTime.Plots.Item(numberParameterAxesChanged - 1).LineWidth = 1
            Catch ex As Exception
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(TuneDiagramUnderSelectedParameterAxesY)}> {ex}")
            End Try

            numberParameterAxesChanged = numberParameterAxesОriginal
        End If
    End Sub
    ''' <summary>
    ''' Очистка графиков
    ''' </summary>
    ''' <param name="N"></param>
    ''' <param name="arrParameters"></param>
    Protected Overrides Sub CleaningDiagram(ByRef N As Integer, ByRef arrParameters() As TypeBaseParameter)
        If N = 0 Then Exit Sub ' массив еще не инициализирован
        Dim I As Integer
        Dim plot As WaveformPlot

        ClearArrowCollection()
        WaveformGraphTime.Annotations.Clear()
        WaveformGraphTime.Plots.Clear()
        SlidePlot.CustomDivisions.Clear()

        For I = 1 To N
            If isDetailedSheet Then
                If arrParameters(IndexParameters(I)).IsVisible AndAlso WaveformGraphTime.Plots.Count < conLimitVisibleTrend Then
                    plot = GetLineLoopTrand(I)
                    plot.Tag = arrParameters(IndexParameters(I)).NameParameter
                    WaveformGraphTime.Plots.Add(plot)
                    AddCustomDivision(plot.Tag.ToString)
                End If
            Else
                If SnapshotSmallParameters(I).IsVisible AndAlso WaveformGraphTime.Plots.Count < conLimitVisibleTrend Then
                    plot = GetLineLoopTrand(I)
                    plot.Tag = SnapshotSmallParameters(I).NameParameter
                    WaveformGraphTime.Plots.Add(plot)
                    AddCustomDivision(plot.Tag.ToString)
                End If
            End If
        Next

        ' если ни чего не добавилось то
        If WaveformGraphTime.Plots.Count = 0 Then
            'Const caption As String ="ОчисткаГрафиковРегистратор"
            'Dim text As String = "Отсутствуют шлейфы в графике по времени!" & vbCrLf & "Возможны ошибки при перемещении курсора." & vbCrLf & "Произведите заново настройку видимости шлейфов."
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ' принудительно включим видимость
            plot = GetLineLoopTrand(1)
            If isDetailedSheet Then
                arrParameters(IndexParameters(1)).IsVisible = True
                plot.Tag = arrParameters(IndexParameters(1)).NameParameter
            Else
                SnapshotSmallParameters(1).IsVisible = True
                plot.Tag = SnapshotSmallParameters(1).NameParameter
            End If
            WaveformGraphTime.Plots.Add(plot)
            AddCustomDivision(plot.Tag.ToString)
        End If

        ' подстроить ширину панели для вмещения всех надписей
        Dim maxTextLenght As Integer = 5 ' символов по умолчанию
        Const ONE_SIMVOL_LENGHT As Single = 7.64 ' приблизительная ширина символа
        Const SLIDE_PLOT_LENGHT As Single = 41.25 ' приблизительная ширина ползунка
        For Each itemScaleCustomDivision As ScaleCustomDivision In SlidePlot.CustomDivisions
            If itemScaleCustomDivision.Text.Length > maxTextLenght Then
                maxTextLenght = itemScaleCustomDivision.Text.Length
            End If
        Next
        SplitContainer2.SplitterDistance = CInt(maxTextLenght * ONE_SIMVOL_LENGHT + SLIDE_PLOT_LENGHT)

        For Each Cursor As XYCursor In WaveformGraphTime.Cursors
            Cursor.Plot = WaveformGraphTime.Plots(0)
        Next

        SlidePlot.Enabled = True

        If WaveformGraphTime.Plots.Count >= 2 Then
            SlidePlot.Range = New Range(1, WaveformGraphTime.Plots.Count) 'иначе ошибка равества диапазонов
            SlidePlot.Visible = True
        Else
            SlidePlot.Range = New Range(1, 2) ' иначе ошибка равества диапазонов
            SlidePlot.Visible = False
        End If

        countVisibleTrendRegistrator = WaveformGraphTime.Plots.Count - 1
        'ReDim_dataAllTrends(countVisibleTrendRegistrator, arraysize)
        Re.Dim(dataAllTrends, countVisibleTrendRegistrator, arraysize)
    End Sub

    ''' <summary>
    ''' Настройка меню и кнопок
    ''' </summary>
    Protected Overrides Sub EnableDisableControlsBase()
        XyCursorTime.Visible = True
        TableLayoutPanelDiscrete = New TableLayoutPanel()
        SplitContainerForm.Panel2.Controls.Add(TableLayoutPanelDiscrete)
        TableLayoutPanelDiscrete.Dock = DockStyle.Top
        TableLayoutPanelDiscrete.Visible = True
        TableLayoutPanelУправленияДляСнимка.Visible = False
        SplitContainer2.SplitterDistance = SlidePlot.Width
        SlideAxis.Visible = False
        PanelSlide.Visible = False
        PanelKey.Visible = False
        LabelIndicator.Visible = False
        GroupBoxAxis.Visible = False
        ButtonSnapshot.Enabled = False
        ComboBoxSelectAxis.Enabled = False
        TextBoxRecordToDisc.Visible = False
        ComboBoxTimeMeasurement.Visible = False
        LabelSec.Visible = False
        MenuFile.Text = cРегистратор
        MenuOpenBaseSnapshot.Enabled = False
        MenuWriteDecodingSnapshotToDBase.Enabled = False
        MenuPrintDecodingSnapshot.Enabled = False
        MenuDecoding.Enabled = False
        MenuExportSnapshotInExcel.Enabled = False
        MenuRecordGraph.Enabled = False
        MenuPrintGraph.Enabled = False
        MenuShowGraphControl.Enabled = True
        MenuShowTextControl.Enabled = True
        IsQuestioningRevolution = True
        StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyBackground) & "Гц"
        GraphModeValue = MyGraphMode.DoNothing
        SetGraphMode(GraphModeValue)
        PopulateCboxDisplayRate()

        EnableDisableControlsInherit()
    End Sub
    ''' <summary>
    ''' вызывается при образовании формы
    ''' </summary>
    Friend Overrides Sub StartMeasurement()
        ApplyRegimeRegistrator() ' идет первым
    End Sub

    ''' <summary>
    ''' Применить настройку видимости шлейфов
    ''' Применение после формы FormVisibleTail
    ''' </summary>
    Protected Overrides Sub ApplyTuningVisibilityMeasuringStub()
        isSkipStopRecord = True
        ApplyRegimeRegistrator()
        isSkipStopRecord = False
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' </summary>
    Protected Overrides Sub ReloadDiagramParameterFromParameterForSnapshot(ByRef inClassGrafOfParam As GraphsOfParameters.GraphOfParameter)
    End Sub

    'Protected Overrides Sub SettingsOptionProgram()
    '    ' реализуется далее
    'End Sub

    'Protected Overrides Sub CharacteristicForRegime()
    '    ' реализуется далее
    'End Sub
    ''' <summary>
    ''' Уточнение Включение подробного/выборочного листа
    ''' </summary>
    Protected Overrides Sub DetailSelectiveBase()
        CleaningDiagram(UBound(IndexParameters), ParametersType)

        Try
            If isUsePens Then
                TuneAnnotation()
            End If

            If GraphModeValue = MyGraphMode.OneCursor Then
                MoveCursor(XyCursorStart, False)
            ElseIf GraphModeValue = MyGraphMode.TwoCursors Then
                MoveCursor(XyCursorStart, False)
                MoveCursor(XyCursorEnd, False)
            End If

        Catch ex As Exception
            Dim caption As String = $"<{NameOf(DetailSelectiveBase)}>"
            Dim text As String = $"{ex}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(DetailSelectiveBase)}> {ButtonDetailSelective.Text}")
        End Try
    End Sub

    ''' <summary>
    ''' Настроить аннотации.
    ''' Метод может быть полностью синхронизирован с помощью атрибута MethodlmplAttribute.
    ''' Такой подход может стать альтернативой оператору lock в тех случаях,
    ''' когда метод требуется заблокировать полностью. 
    ''' </summary>
    <MethodImpl(MethodImplOptions.Synchronized)>
    Protected Overrides Sub TuneAnnotation()
        Dim I, indexPlot As Integer
        Dim isSldPlotSelected As Boolean
        Dim tempMaxVisibleTrends As Integer ' Ограничение видимых щлефов

        Try
            WaveformGraphTime.Annotations.Clear()

            'If GraphModeValue = MyGraphMode.ДваКурсора Then
            '    tempОграничениеВидимыхШлефов = WaveformGraphTime.Plots.Count * 2
            'Else
            tempMaxVisibleTrends = WaveformGraphTime.Plots.Count
            'End If

            Dim mTypeParameter As TypeBaseParameter()
            If IsBeforeThatHappenLoadDbase Then
                mTypeParameter = ParametersShaphotType
            Else
                mTypeParameter = ParametersType
            End If

            If isDetailedSheet Then
                For I = 1 To UBound(IndexParameters)
                    If mTypeParameter(IndexParameters(I)).IsVisible AndAlso WaveformGraphTime.Annotations.Count < tempMaxVisibleTrends Then
                        WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, mTypeParameter(IndexParameters(I)).NameParameter))

                        'If GraphModeValue = MyGraphMode.ДваКурсора Then
                        '    WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, mTypeParameter(IndexParameters(I)).NameParameter))
                        'End If
                        indexPlot += 1

                        If isSldPlotSelected = False Then
                            ' слайдер будет первым видимым параметром
                            SlidePlot.Value = indexPlot
                            SlidePlot.PointerColor = ColorsNet((I - 1) Mod 7)
                            ComboBoxSelectAxis.SelectedIndex = I - 1
                            isSldPlotSelected = True
                        End If
                    End If
                Next I
            Else
                For I = 1 To UBound(SnapshotSmallParameters)
                    If SnapshotSmallParameters(I).IsVisible AndAlso WaveformGraphTime.Annotations.Count < tempMaxVisibleTrends Then
                        WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, SnapshotSmallParameters(I).NameParameter))

                        'If GraphModeValue = MyGraphMode.ДваКурсора Then
                        '    WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, arrTypeSmallParameter(I).NameParameter))
                        'End If
                        indexPlot += 1

                        If isSldPlotSelected = False Then
                            ' слайдер будет первым видимым параметром
                            SlidePlot.Value = indexPlot
                            SlidePlot.PointerColor = ColorsNet((I - 1) Mod 7)
                            ComboBoxSelectAxis.SelectedIndex = I - 1
                            isSldPlotSelected = True
                        End If
                    End If
                Next I
            End If
        Catch ex As Exception
            Dim caption As String = NameOf(TuneAnnotation)
            Dim text As String = $"{ex}{vbCrLf}Для исправления перещёлкните кнопку <Выборочно<->Подробно>"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub
    ''' <summary>
    ''' Перезаписывает строку запроса SQL и выставляет флаг если база данных сменена
    ''' </summary>
    ''' <param name="outSQL"></param>
    ''' <param name="IsChannelShaphot"></param>
    Friend Overrides Sub GetSqlForDbase(ByRef outSQL As String, ByRef IsChannelShaphot As Boolean)
        IsChannelShaphot = False
        outSQL = "SELECT * FROM " & ChannelLast
    End Sub
#End Region

    ''' <summary>
    ''' Добавить пользовательские деление к слайдеру
    ''' </summary>
    ''' <param name="nameParameter"></param>
    Private Sub AddCustomDivision(nameParameter As String)
        ' пока убрал
        'If nameParameter.Length > 5 Then
        '    nameParameter = nameParameter.Substring(0, 5)
        'End If
        SlidePlot.CustomDivisions.Add(New ScaleCustomDivision With {
            .Value = WaveformGraphTime.Plots.Count,
            .Text = nameParameter
        })
    End Sub

    ''' <summary>
    ''' Заполнить список примечаниями к перечислимому типу {DisplayRate} частоты обновления измеренных параметров
    ''' и выделить считанный из настроек.
    ''' </summary>
    Private Sub PopulateCboxDisplayRate()
        'Dim dicRate As New Dictionary(Of String, DisplayRate)
        Dim dicRate As New List(Of String)

        ' получить все аттрибуты перечислителя для создания списка возможных частот обновления текстовых полей
        For Each value In [Enum].GetValues(GetType(DisplayRate))
            Dim fi As FieldInfo = GetType(DisplayRate).GetField([Enum].GetName(GetType(DisplayRate), value))
            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

            If dna IsNot Nothing Then
                dicRate.Add(dna.Description) ', value)
            Else
                dicRate.Add("Нет описания") ', value)
            End If
        Next

        ComboBoxDisplayRate.Items.AddRange(dicRate.ToArray) 'dicRate.Keys.ToArray)
        ' выделить текст значения считанного из настроек TextDisplayRate
        Dim fiTextDisplayRate As FieldInfo = GetType(DisplayRate).GetField([Enum].GetName(GetType(DisplayRate), TextDisplayRate))
        Dim dnaTextDisplayRate As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fiTextDisplayRate, GetType(DescriptionAttribute)), DescriptionAttribute)
        If dnaTextDisplayRate IsNot Nothing Then
            ComboBoxDisplayRate.Text = dnaTextDisplayRate.Description
        End If
    End Sub

#Region "Старт-Стоп сбор и запись"
    Private workerRecordStartStop As TaskRecordStartStop
    Private tokenSouce As CancellationTokenSource ' маркер отмены
    Private isExitEvent As Boolean ' предотвращение повторного срабатывания события
    Private Const millisecondsTimeout As Integer = 1000 ' задержка
    Private Const twoSecondsTimeout As Double = 2.0 ' задержка
    ''' <summary>
    ''' Ассинхроный запуск приостановки исполнения команды,
    ''' чтобы IsRecordEnable не схватывало 2 раза.
    ''' Включение непрерывной записи
    ''' </summary>
    ''' <param name="isHandle"></param>
    ''' <param name="inIsRecordEnable"></param>
    Protected Async Sub WorkTaskRecordStartStop(isHandle As Boolean, inIsRecordEnable As Boolean, inIsFormRunning As Boolean)
        ButtonRecord.Enabled = False
        isExitEvent = True
        IsRecordEnable = inIsRecordEnable
        workerRecordStartStop = New TaskRecordStartStop()
        '_worker.ProcessChanged += _worker_ProcessChanged
        tokenSouce = New CancellationTokenSource()
        Dim token As CancellationToken = tokenSouce.Token
        Dim mTask As Task(Of Boolean) = Nothing
        'Dim isError As Boolean = False
        'Dim message As String = ""

        Try
            ' просто приостановить на время (можно использовать аналогичный метод Delay в MainModule)
            mTask = Task(Of Boolean).Factory.StartNew(Function() workerRecordStartStop.Work(token, millisecondsTimeout), token)
            Await mTask
            'Catch ex As OperationCanceledException
        Catch ex As Exception
            'isError = True
            Dim caption As String = $"<{NameOf(WorkTaskRecordStartStop)}>"
            Dim text As String = $"Произошла ошибка: {ex}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        'If Not isError Then message = If(mTask.IsCanceled, "Процесс отменен", "Процесс завершен!")
        'MessageBox.Show(message)

        If inIsFormRunning Then
            If IsRecordEnable Then
                ButtonRecord.ToolTipText = "Выключить запись"
                ButtonRecord.Image = My.Resources.RunSave
            Else
                ButtonRecord.ToolTipText = "Включить запись испытания"
                ButtonRecord.Image = My.Resources.StopSave
            End If

            TextBoxRecordToDisc.Visible = inIsRecordEnable
            StartStopWriting(IsRecordEnable)
        Else
            If ButtonRecord.Checked Then ButtonRecord.Checked = False
        End If

        isButtonRecordPressHandle = isHandle
        ButtonRecord.Checked = IsRecordEnable
        isExitEvent = False ' после ButtonRecord.Checked = IsRecordEnable для вылета
        ButtonRecord.Enabled = True
    End Sub

    ''' <summary>
    ''' Запуск или остановка записи, конфигурирование TdmsFileProcessor.
    ''' </summary>
    Private Sub StartStopWriting(inIsRecordEnable As Boolean)
        IsQuestioningRevolution = Not inIsRecordEnable
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(StartStopWriting)}> Запуск или остановка записи ={inIsRecordEnable}")

        If inIsRecordEnable Then
            UpdateCompensationXC() ' записать Тхс, для ТСРКлиент не нужно

            If IsUseTdms Then
                Do While gTdmsFileProcessor.IsBusy OrElse isRecordingSnapshot
                    'Thread.Sleep(10)
                    Delay(0.01)
                    Application.DoEvents()
                Loop

                gTdmsFileProcessor.Configure(PathResourses, NumberEngine.ToString, "Фоновая запись", IndexParameters, ParametersType)
            End If
        Else ' здесь надо сделать запись если прервано в середине графика
            RecordSnapshotFrameRegistrationToDisk(True)
            ' используется как признак сброса из памяти на диск varTdmsFileProcessor.CloseTDMSFile()
            countFrameRegistrator = 0 ' обнуляем, кнопка записи выключена 
        End If
    End Sub

    ''' <summary>
    ''' Включение непрерывной записи
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonRecord_CheckedChanged(sender As Object, e As EventArgs) Handles ButtonRecord.CheckedChanged
        If isExitEvent Then Exit Sub

        If ButtonRecord.Checked Then
            WorkTaskRecordStartStop(True, True, IsFormRunning)
        Else
            WorkTaskRecordStartStop(True, False, IsFormRunning)
        End If
        isExitEvent = False
    End Sub

    ''' <summary>
    ''' Остановить звпись
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub TimerSwitchOffRecord_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TimerSwitchOffRecord.Tick
        TimerSwitchOffRecord.Enabled = False

        If IsRecordEnable Then WorkTaskRecordStartStop(False, False, IsFormRunning)
    End Sub

    ''' <summary>
    ''' Запустить/Остановить непрерывный сбор
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonContinuously_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonContinuously.CheckedChanged
        If ButtonContinuously.Checked AndAlso IsFormRunning Then Exit Sub
        If IsMemoForStartRecord Then
            ' После загрузки окна Сервера произойдёт заново запуск опроса
            ' и если была включена запись то нужно её возобновить.
            isButtonRecordPressHandleMemo = isButtonRecordPressHandle
            isRecordEnableMemo = IsRecordEnable
            isFormRunningMemo = IsFormRunning
        End If
        ContinuousTelemetry(ButtonContinuously.Checked)
    End Sub

    ''' <summary>
    ''' Запуск или остановка сбора
    ''' </summary>
    ''' <param name="start"></param>
    Private Sub ContinuousTelemetry(start As Boolean)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ContinuousTelemetry)}> Запуск или остановка сбора ={start}")

        If IsRecordEnable AndAlso isSkipStopRecord = False Then
            ' остановить запись когда происходит конфигурация
            ' и не останавливать когда происходит настройка видимости шлейфов
            WorkTaskRecordStartStop(True, False, IsFormRunning)
            'Thread.Sleep(millisecondsTimeout * 2)
            Delay(twoSecondsTimeout)
        End If

        If start Then
            StartContinuous()

            If isUsePens Then TuneAnnotation()

            If Not ButtonTuneTrand.Checked Then
                TuneTrand(False)
                isShowDiagramFromParameter = ButtonGraphParameter.Checked
            End If

            If IsUseCalculationModule Then
                MainMDIFormParent.ModuleSolveManager.EnableMenu(False)
            End If

            ButtonContinuously.Text = "Стоп"
            ButtonContinuously.ToolTipText = "Остановить сбор"
            ButtonContinuously.Image = My.Resources.Registration_pause
        Else
            InstallQuestionForm(WhoIsExamine.Nobody)
            StartStopSocket(start)

            If IsUseCalculationModule Then
                MainMDIFormParent.ModuleSolveManager.EnableMenu(True)
            End If

            ButtonContinuously.Text = "Пуск"
            ButtonContinuously.ToolTipText = "Начать сбор"
            ButtonContinuously.Image = My.Resources.Registrationr_play
        End If

        Application.DoEvents()
        IsRun = start
    End Sub

    ''' <summary>
    ''' Выключить запись
    ''' </summary>
    Friend Sub StopRecord()
        If IsRecordEnable AndAlso isSkipStopRecord = False Then
            WorkTaskRecordStartStop(True, False, IsFormRunning)
            'Thread.Sleep(millisecondsTimeout * 2)
            Delay(twoSecondsTimeout)
        End If
    End Sub

    ''' <summary>
    ''' Запустить Непрерывный сбор с контроллером DAQmx
    ''' </summary>
    Friend Sub StartContinuous()
        ' здесь проверка не была ли включена кнопка записи
        If IsMemoForStartRecord Then
            IsMemoForStartRecord = False
            WorkTaskRecordStartStop(isButtonRecordPressHandleMemo, isRecordEnableMemo, isFormRunningMemo)
            'Thread.Sleep(millisecondsTimeout * 2)
            Delay(twoSecondsTimeout)
        Else
            If IsRecordEnable AndAlso isSkipStopRecord = False Then
                ' остановить запись когда происходит конфигурация
                ' и не останавливать когда происходит настройка видимости шлейфов
                WorkTaskRecordStartStop(True, False, IsFormRunning)
                'Thread.Sleep(millisecondsTimeout * 2)
                Delay(twoSecondsTimeout)
            End If
        End If

        'NativeMethods.RegProgramm()
        countRow = 0

        If Not IsUseTdms Then dataMeasuredValuesString.Remove(0, dataMeasuredValuesString.Length)

        ClearArrowCollection()
        IsSnapshot = False
        lastx = 0
        MainModule.CountAcquisition = CInt(DeltaX * (FrequencyBackground * LevelOversampling))

        'ReDim_dataAllTrends(countVisibleTrendRegistrator, arraysize)
        'ReDim_MeasuredValuesToRange(UBound(IndexParameters) - 1, MainModule.CountAcquisition \ LevelOversampling) ' частота сбора/передискретизацию
        'ReDim_MeasuredValues(UBound(IndexParameters) - 1, arraysize)
        'ReDim_PackOfParametersToRecord(UBound(IndexParameters) - 1)
        Re.Dim(dataAllTrends, countVisibleTrendRegistrator, arraysize)
        Re.Dim(MeasuredValuesToRange, UBound(IndexParameters) - 1, MainModule.CountAcquisition \ LevelOversampling) ' частота сбора/передискретизацию
        Re.Dim(MeasuredValues, UBound(IndexParameters) - 1, arraysize)
        Re.Dim(PackOfParametersToRecord, UBound(IndexParameters) - 1)

        systemTime = TimeOfDay

        If Not IsFormRunning Then InstallQuestionForm(WhoIsExamine.Oscilloscope)

        CountMeasurand = UBound(IndexParameters) - 1
        ADCAcquisitionParametersCount = CountMeasurand + 1 ' обычный сбор

        If IsUseCalculationModule OrElse IsDigitalInput Then
            ADCAcquisitionParametersCount = CountMeasurand - AdditionalParameterCount + 1
        End If

        'ReDim_DataValuesFromServer(CountMeasurand + 1)
        Re.Dim(DataValuesFromServer, CountMeasurand + 1)
        ConfigureWaveformGraphScale(arraysize)
        YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))
        SlidePlot.Visible = WaveformGraphTime.Plots.Count >= 2

        If isUsePens Then TuneAnnotation()

        countUpdateScreen1 = 0
        InitializeContinuousDAQmxTask()
        StartStopSocket(True)

        ButtonContinuously.Checked = True
    End Sub
#End Region

    ''' <summary>
    ''' Отобразить значения на графике от параметров
    ''' </summary>
    Protected Sub OutParametersGraph()
        Dim average As Double ' среднее
        Dim numberTrand As Integer ' Номер шлейфа

        ' работа с графиком от параметра
        If isShowDiagramFromTime OrElse (Not IsNothing(isClearDataDiagramFromParameter)) Then
            Dim upperLoop As Integer = CounterLightParametersGraph - 1

            If AllGraphParametersByParameter Is Nothing Then TuneTrand(False) ' т.е. запуск сбора cmdНастроитьГрафик_ValueChanged(cmdНастроитьГрафик, New AxCWUIControlsLib._DCWButtonEvents_ValueChangedEvent(True))

            If counterDiagramFromParameter >= CounterParametersGraph Then
                Dim tempPointAnnotation As XYPointAnnotation

                For J = 1 To UBound(AllGraphParametersByParameter)
                    average = MeasuredValues(J - 1, indexTimeVsRow)

                    If isShowDiagramFromTime = True Then
                        If AllGraphParametersByParameter(J).NumberTail <> -1 Then ' значит ему был привязан шлейф
                            dataDiagramFromParameter(AllGraphParametersByParameter(J).NumberTail, positionCursorDiagramFromParameter) = average
                            Name = AllGraphParametersByParameter(J).NameParameter
                            With ScatterGraphParameter
                                tempPointAnnotation = CType(.Annotations(AllGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                                tempPointAnnotation.SetPosition(positionCursorDiagramFromParameter, average)
                                tempPointAnnotation.Caption = $"{Name}: {Round(average, Precision)}"
                                tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                            End With
                        End If
                    Else
                        If nameParameterAxesX = AllGraphParametersByParameter(J).NameParameter Then
                            If isClearDiagramFromParameterAxesX = True Then
                                For N As Integer = 0 To upperLoop
                                    axesXDiagramFromParameter(N) = average
                                Next
                                isClearDiagramFromParameterAxesX = False
                            End If

                            ' сдвиг массива
                            For N As Integer = 0 To upperLoop - 1
                                axesXDiagramFromParameter(N) = axesXDiagramFromParameter(N + 1)
                            Next

                            ' добавить к последнему элементу в массиве
                            If average = 0 Then
                                ' если значение равно нулю, скорее всего в SweepChart обнулён массив arrСреднее, поэтому скопировать предыдущее значение
                                axesXDiagramFromParameter(upperLoop) = axesXDiagramFromParameter(upperLoop - 1)
                            Else
                                axesXDiagramFromParameter(upperLoop) = average '=график который на ось х
                            End If
                        End If

                        If AllGraphParametersByParameter(J).NumberTail <> -1 Then ' значит ему был привязан шлейф
                            ' сдвиг массива
                            numberTrand = AllGraphParametersByParameter(J).NumberTail
                            average = MeasuredValues(J - 1, indexTimeVsRow)

                            If isClearDataDiagramFromParameter(numberTrand) = False Then ' был очищен массив
                                For N As Integer = 0 To upperLoop
                                    dataDiagramFromParameter(numberTrand, N) = average
                                Next
                                isClearDataDiagramFromParameter(numberTrand) = True
                            End If

                            For N As Integer = 0 To upperLoop - 1
                                dataDiagramFromParameter(numberTrand, N) = dataDiagramFromParameter(numberTrand, N + 1)
                            Next

                            ' добавить к последнему элементу в массиве
                            If average = 0 Then
                                ' если значение равно нулю, скорее всего в SweepChart обнулён массив arrСреднее, поэтому скопировать предыдущее значение
                                dataDiagramFromParameter(numberTrand, upperLoop) = dataDiagramFromParameter(numberTrand, upperLoop - 1)
                            Else
                                dataDiagramFromParameter(numberTrand, upperLoop) = average
                            End If

                        End If
                    End If
                Next

                If isShowDiagramFromTime Then
                    XyCursor1.XPosition = positionCursorDiagramFromParameter
                    ScatterGraphParameter.PlotXYMultiple(axesXDiagramFromParameter, dataDiagramFromParameter)
                    positionCursorDiagramFromParameter += 1
                    If positionCursorDiagramFromParameter > arraysizeDiagramFromParameter Then positionCursorDiagramFromParameter = 0
                Else
                    If dataDiagramFromParameter.Length > 0 Then
                        For J = 1 To UBound(AllGraphParametersByParameter)
                            numberTrand = AllGraphParametersByParameter(J).NumberTail

                            If numberTrand <> -1 Then
                                average = dataDiagramFromParameter(numberTrand, upperLoop)
                                ' переписали Name
                                Name = AllGraphParametersByParameter(J).NameParameter
                                With ScatterGraphParameter
                                    tempPointAnnotation = CType(.Annotations(AllGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                                    tempPointAnnotation.SetPosition(axesXDiagramFromParameter(upperLoop), average)
                                    tempPointAnnotation.Caption = $"{Name}: {Round(average, Precision)}"
                                    tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                                End With
                            End If
                        Next

                        ScatterGraphParameter.PlotXYMultiple(axesXDiagramFromParameter, dataDiagramFromParameter)
                    End If
                End If

                counterDiagramFromParameter = 0
                CopyFromListViewAcquisitionToListViewParametr()
            End If
            counterDiagramFromParameter += 1
        End If
    End Sub

    ''' <summary>
    ''' Перестроить шкалу графика
    ''' capacity = arraysize
    ''' </summary>
    ''' <param name="capacity"></param>
    Protected Sub ConfigureWaveformGraphScale(ByVal capacity As Integer)
        precisionTimeStart = New PrecisionDateTime(DateTime.Now)
        defaultInterval = New PrecisionTimeSpan((1 / FrequencyBackground) * GetLengtheningScale()) ' в секундах

        XAxisTime.MajorDivisions.LabelVisible = True
        XAxisTime.MajorDivisions.TickVisible = True
        XAxisTime.CustomDivisions.Clear()
        XAxisTime.MajorDivisions.LabelFormat = New FormatString(FormatStringMode.DateTime, "h:mm:ss tt") '"HH:mm:ss tt", "MMM d, yyyy"), "m:ss.fff")
        XAxisTime.MajorDivisions.Base = CDbl(DataConverter.Convert(precisionTimeStart, Type.GetType("System.Double")))
        'XAxisTime.AutoMinorDivisionFrequency = 1
        Dim precisionTimeEnd As PrecisionDateTime = precisionTimeStart.AddMilliseconds(capacity * (1000 / FrequencyBackground) * GetLengtheningScale())
        XAxisTime.Range = New Range(CDbl(DataConverter.Convert(precisionTimeStart, Type.GetType("System.Double"))), CDbl(DataConverter.Convert(precisionTimeEnd, Type.GetType("System.Double"))))
        SlideTime.Range = New Range(0, capacity / FrequencyBackground)
    End Sub

    ''' <summary>
    ''' Обновить развёртку графика
    ''' </summary>
    Protected Sub SweepChart()
        If Not isRecordingSnapshot Then
            If xNewPos = 0 Then
                ' RegProgramm()
                ' здесь запись полного снимка регистратора по аналогии с осциллографом
                ' в зависимости от того включена или нет кнопка записи
                If IsRecordEnable Then
                    RecordSnapshotFrameRegistrationToDisk(False)
                    ' здесь обновить информацию о свободном дисковом пространстве текущего диска где установлена программа
                    If FrequencyBackground < 20 Then StatusStripMain.Items(NameOf(LabelDiscValue)).Text = FreeBytes(PathResourses) ' чтобы не тормозила
                    ' обновить значение компенсации ХС
                    If TemperatureTxc > -30 AndAlso TemperatureTxc < 40 Then
                        For I As Integer = 1 To UBound(ParametersType)
                            If ParametersType(I).CompensationXC Then ParametersType(I).Offset = TemperatureTxc
                        Next
                    End If
                End If

                If Not IsUseTdms Then dataMeasuredValuesString.Remove(0, dataMeasuredValuesString.Length)

                ' очистка массива чтобы не остались прежние значения
                'ReDi_ MeasuredValues(UBound(IndexParameters) - 1, arraysize)
                Re.Dim(MeasuredValues, UBound(IndexParameters) - 1, arraysize)
                systemTime = TimeOfDay
                ConfigureWaveformGraphScale(arraysize)
                countRow = 0
                'Application.DoEvents() '(нельзя т.к. она тормозит текущий поток в ижидании других) вставил только в эту процедуру обработки сбора чтобы более отзывчивое приложение было
            End If

            If isFireUpdateScreen1 AndAlso xNewPos <> 0 Then
                MoveSlideTime(indexTimeVsRow * DeltaX)
                XyCursorTime.MoveCursor(CDbl(DataConverter.Convert(precisionTimeStart.AddMilliseconds(indexTimeVsRow * defaultInterval.TotalMilliseconds).ToDateTime, Type.GetType("System.Double"))), YAxisTime.Range.Minimum)

                If isUsePens Then MoveAnnotations(XyCursorTime.XPosition, indexTimeVsRow, True, True)

                WaveformGraphTime.PlotYMultiple(dataAllTrends, DataOrientation.DataInRows, precisionTimeStart.ToDateTime, defaultInterval.ToTimeSpan)
            End If

            lastx = xNewPos
        End If
    End Sub

    ''' <summary>
    ''' Режим двигателя на индикатор для оператора
    ''' </summary>
    ''' <param name="inARUD"></param>
    ''' <param name="inN2reduced"></param>
    ''' <returns></returns>
    Protected Function GetRegimeEngineString(inARUD As Double, inN2reduced As Double) As String
        If inARUD <= 8 Then
            Return vbNullString
        ElseIf (inARUD > 8 AndAlso inARUD < 14) AndAlso inN2reduced < 80 Then
            Return "МГ" ' Малый газ
        ElseIf (inARUD >= 14 AndAlso inARUD < 65) AndAlso inN2reduced < 86 Then
            Return "КРпрод" ' Крейсерский продолжительный
        ElseIf inARUD < 65 AndAlso (inN2reduced >= 86 AndAlso inN2reduced < 90) Then
            Return "КР" ' Крейсерский
        ElseIf inARUD < 65 AndAlso (inN2reduced >= 90 AndAlso inN2reduced < 93.5) Then
            Return "92%" ' N2прив 92%
        ElseIf inARUD < 65 AndAlso (inN2reduced >= 93.5 AndAlso inN2reduced < 97) Then
            Return "95%" ' N2прив 95%
        ElseIf (inARUD >= 65 AndAlso inARUD < 69) AndAlso inN2reduced >= 97 Then
            Return "M" ' Максимальный режим
        ElseIf (inARUD > 74 AndAlso inARUD < 78) AndAlso inN2reduced >= 97 Then
            Return "Фмин" ' Форсаж минимальный
        ElseIf (inARUD > 104 AndAlso inARUD < 113) AndAlso inN2reduced >= 97 Then
            Return "Фполн" ' Форсаж полный
        ElseIf inARUD > 113 AndAlso inN2reduced >= 97 Then
            Return "ОР" ' Особый режим
        Else
            Return vbNullString
        End If
    End Function

#Region "Запись кадров"
    ''' <summary>
    ''' Запись кадра регистратора на диск
    ''' </summary>
    ''' <param name="isCloseTDMSFile"></param>
    Protected Sub RecordSnapshotFrameRegistrationToDisk(isCloseTDMSFile As Boolean)
        If countRow < 2 Then
            Exit Sub
        End If

        If IsUseTdms Then
            isRecordingSnapshot = True

            Do While gTdmsFileProcessor.IsBusy
                'Thread.Sleep(10)
                Delay(0.01)
                Application.DoEvents()
            Loop

            gTdmsFileProcessor.AppendData(MeasuredValues)

            If isCloseTDMSFile Then ' прекращение записи по кнопке "Запись", закрытие окна
                ' прерванный снимок - закрыть файл
                gTdmsFileProcessor.IsCloseTDMSFile = True
                gTdmsFileProcessor.CloseTDMSFile()
                ' там вызов TdmsFilePr_ClosedTDMSFileEven в котором isRecordingSnapshot = False
            End If
        Else
            isRecordingSnapshot = True

            Dim maximum As Double
            Dim strTime As String = $"{Replace(Trim(Now.ToLongTimeString), ":", "-")}-{Now.Millisecond}"
            Dim descriptionFrame As String = "Фоновая запись - прерванный снимок"

            If xNewPos = 0 Then
                countFrameRegistrator += 1
                descriptionFrame = "Фоновая запись " & Str(countFrameRegistrator)
            End If

            Dim pathTextDataStream As String = $"{PathResourses}\База снимков\{ModificationEngine}-{NumberEngine} ({Today.ToShortDateString} {strTime}) {descriptionFrame}.txt"

            If IsRecordEnable = False Then 'значит запись прервана в середине
                maximum = XyCursorTime.XPosition
            Else 'запись не прервана и курсор дошел до конца
                maximum = XAxisTime.Range.Maximum
            End If

            If countRow <> arraysize Then countRow -= 1
            'strSQL = "INSERT INTO БазаСнимков (НомерИзделия, Дата, ВремяНачалаСбора, Тбокса, ТипКРД, Режим, СтрокаКонфигурации, КолСтрок, КолСтолбцов, ЧастотаОпроса, НачалоОсиХ, КонецОсиХ, ПутьНаДиске, ТаблицаКаналов, Примечание) VALUES (" &
            '    lngНомерИзделия.ToString & ",'" & Today.ToString & "'," & [Время].ToOADate & "," & TemperatureOfBox.ToString & ",'" & strТипКрд & "','" & режим & "','" & strСтрокаКонфигурации & "'," & chКолСтрок.ToString & "," & UBound(arrСреднее).ToString & "," & intЧастотаФонового.ToString & "," & CStr(XAxisTime.Range.Minimum) & "," & dblMaximum.ToString & ",'" & pathTextDataStream & "','" & strChannelПоследняя & "','" & strПримечаниеСнимка & "')"
            RecordSnapshotFrameToDBase("INSERT INTO БазаСнимков (НомерИзделия, Дата, ВремяНачалаСбора, Тбокса, ТипКРД, Режим, СтрокаКонфигурации, КолСтрок, КолСтолбцов, ЧастотаОпроса, НачалоОсиХ, КонецОсиХ, ПутьНаДиске, ТаблицаКаналов, Примечание) VALUES (" &
                NumberEngine.ToString & ",'" &
                Today.ToString & "'," &
                systemTime.ToOADate & "," &
                TemperatureOfBox.ToString & ",'" &
                TypeKRD & "','" &
                RegimeType & "','" &
                ConfigurationString & "'," &
                countRow.ToString & "," &
                UBound(MeasuredValues).ToString & "," &
                FrequencyBackground.ToString & "," &
                CStr(XAxisTime.Range.Minimum) & "," &
                maximum.ToString & ",'" &
                pathTextDataStream & "','" &
                ChannelLast & "','" &
                descriptionFrame & "')")

            ' делаем ссылку и открываем поток
            Using FS As New FileStream(pathTextDataStream, FileMode.Create, FileAccess.Write)
                Using SW As New StreamWriter(FS)
                    SW.Write(dataMeasuredValuesString.ToString)
                End Using
            End Using

            'Application.DoEvents()
            isRecordingSnapshot = False
        End If

        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Запись строки с данными Кадра Сбора в базу данных
    ''' </summary>
    ''' <param name="strSQL"></param>
    Private Sub RecordSnapshotFrameToDBase(ByVal strSQL As String)
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        cn.Open()
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cmd.ExecuteNonQuery()

        If Arrows.Count > 0 Then
            ' придётся дурить так как обработчик чтобы узнать lngKeyID связан с odaDataAdapter, а здесь strSQL = "INSERT INTO
            cmd = New OleDbCommand("SELECT @@IDENTITY", cn)

            ' Записать идентификатор автоинкремена ключевого поля в колонку KeyID
            GKeyID = CInt(cmd.ExecuteScalar())

            strSQL = "SELECT * FROM [Стрелки]  WHERE KeyID = " & GKeyID.ToString
            Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter(strSQL, cn)
            Dim dtDataTable As New DataTable
            Dim drDataRow As DataRow
            Dim cb As OleDbCommandBuilder

            ' запись стрелок
            odaDataAdapter.Fill(dtDataTable)

            ' запрос должен быть пустым и в него добавляется запись нового поля
            For I As Integer = 0 To Arrows.Count - 1
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("KeyID") = GKeyID
                drDataRow("ПризнакВидаСтрелки") = Arrows(I).ViewArrow
                drDataRow("X1") = Arrows(I).X1
                drDataRow("Y1") = Arrows(I).Y1
                drDataRow("X2") = Arrows(I).X2
                drDataRow("Y2") = Arrows(I).Y2
                drDataRow("Надпись") = Arrows(I).Legend
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            ClearArrowCollection()
        End If

        cn.Close()
    End Sub

    ''' <summary>
    ''' Метод делегата обработки события сброса из памяти и закрытие TDMS файла
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub TdmsFilePr_ClosedTDMSFileEvent(ByVal sender As Object, ByVal e As TdmsFileProcessor.ClosedTDMSFileEventArgs)
        ' присвоить Cancel = True, установить флаг что в событии _ClosedTDMSFileEvent после записи надо закрыть форму
        ' далее дождаться _ClosedTDMSFileEvent и если флаг установлен на закрытие окна, то после записи в базу заново инициировать закрытие окна,
        ' но наверно будет проблема, если закрывается приложение - наверно в родительском окне надо тоже что-то отслеживать
        Dim descriptionFrame As String = "Фоновая запись - прерванный снимок"

        If e.IsSnapshotComplete Then
            countFrameRegistrator += 1
            If IsRecordEnable Then descriptionFrame = "Фоновая запись " & Str(countFrameRegistrator)
        End If

            'If e.RowsCount = 0 Then
            '    'Stop
            '    ' значит сработал лишнее событие и был создан пустой файл, который нужно удалить
            '    DeleteTextFile(e.PathFile)
            '    DeleteTextFile(e.PathFile & "_index")
            'Else
            RecordSnapshotFrameToDBase("INSERT INTO БазаСнимков (НомерИзделия, Дата, ВремяНачалаСбора, Тбокса, ТипКРД, Режим, СтрокаКонфигурации, КолСтрок, КолСтолбцов, ЧастотаОпроса, НачалоОсиХ, КонецОсиХ, ПутьНаДиске, ТаблицаКаналов, Примечание) VALUES (" &
                NumberEngine.ToString & ",'" &
                e.DateTimeToday & "'," &
                e.TimeStartCollect & "," &
                TemperatureOfBox.ToString & ",'" &
                TypeKRD & "','" &
                cРегистратор & "','" &
                e.ConfigurationString & "'," &
                e.RowsCount & "," &
                e.ColumnsCount & "," &
                FrequencyBackground.ToString & "," &
                "0" & "," &
                arraysize.ToString & ",'" &
                e.PathFile & "','" &
                ChannelLast & "','" &
                descriptionFrame & "')")
        'End If

        isRecordingSnapshot = False ' (начало в RecordSnapshotFrameRegistrationToDisk, а здесь завершающий этап)
    End Sub

    ''' <summary>
    ''' Метод делегата обработки события сброса из памяти в TDMS файл
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub TdmsFilePr_ContinueAccumulation(sender As Object, e As EventArgs)
        isRecordingSnapshot = False ' (начало в RecordSnapshotFrameRegistrationToDisk, а здесь завершающий этап)
    End Sub

    ''' <summary>
    ''' Автоматический разбор записей
    ''' </summary>
    ''' <param name="strSQL"></param>
    Protected Sub AutomaticParsingRecording(strSQL As String)
        ' strSQL = "SELECT COUNT(*) FROM " & strChannelПоследняя & " WHERE Погрешность=" & indexРасчетный.ToString
        ' strSQL = "SELECT COUNT(*) FROM [БазаСнимков]"
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim countRows As Integer ' Число записей

        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cn.Open()
        countRows = CInt(cmd.ExecuteScalar)
        cn.Close()

        If countRows <> 0 Then
            Const caption As String = "Проверка соответствия каналов"
            Const text As String = "Будет произведён автоматический разбор записей в архив, немного подождите."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ArchivingByOpeningWindow()
        End If
    End Sub
#End Region

#Region "ButtonАвария и индикаторы дискретных каналов"
    ''' <summary>
    ''' Число дискретных индикаторов
    ''' </summary>
    Protected countDiscreteLed As Integer
    Dim counterLed, counterTLPanel As Integer
    Private Const TableLayoutPanelColumnCount As Integer = 16

    ''' <summary>
    ''' Инициализация дискретных индикаторов
    ''' </summary>
    Friend Sub InitializeDiscreteLed()
        countDiscreteLed = 0

        For I = 1 To UBound(IndexParameters)
            If ParametersType(IndexParameters(I)).NumberFormula = 3 Then countDiscreteLed += 1
        Next

        listDiscreteLed.Clear()

        If countDiscreteLed = 0 Then
            ' спрятать панель
            SplitContainerForm.Panel2Collapsed = True
            TableLayoutPanelDiscrete.Controls.Clear()
            Exit Sub
        End If

        SplitContainerForm.Panel2Collapsed = False
        InitializeTableLayoutPanelDiscrete(countDiscreteLed)

        Dim visibleLeds As Integer = 0
        For I = 1 To UBound(IndexParameters)
            If ParametersType(IndexParameters(I)).NumberFormula = 3 Then
                listDiscreteLed(visibleLeds).Caption = ParametersType(IndexParameters(I)).NameParameter
                ToolTip1.SetToolTip(listDiscreteLed(visibleLeds), ParametersType(IndexParameters(I)).NameParameter)
                listDiscreteLed(visibleLeds).Visible = True
                visibleLeds += 1
            End If
        Next
    End Sub

    ''' <summary>
    ''' Инициализация дискретных индикаторов
    ''' </summary>
    ''' <param name="countDiscreteLed"></param>
    Private Sub InitializeTableLayoutPanelDiscrete(countDiscreteLed As Integer)
        Dim countPanel As Integer = countDiscreteLed \ TableLayoutPanelColumnCount + If(countDiscreteLed Mod TableLayoutPanelColumnCount = 0, 0, 1)
        Dim rowHeight As Integer = 60
        Dim rowDecorative As Integer = 10

        counterLed = 1
        counterTLPanel = 1

        TableLayoutPanelDiscrete.SuspendLayout()
        With TableLayoutPanelDiscrete
            .Controls.Clear()
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
            .ColumnCount = 1
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

            For row As Integer = 1 To countPanel
                .Controls.Add(CreateTableLayoutPanel(row), 0, row - 1)
            Next

            .GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            .Location = New Point(0, 0)
            .Margin = New Padding(0)
            .Name = "TableLayoutPanelDiscrete"
            .RowCount = countPanel

            For row As Integer = 1 To countPanel
                .RowStyles.Add(New RowStyle(SizeType.Percent, 12.5!))
            Next

            .RowStyles.Add(New RowStyle(SizeType.Absolute, 20.0!))
            .Size = New Size(SplitContainerForm.Panel2.Width, countPanel * rowHeight)
        End With
        TableLayoutPanelDiscrete.ResumeLayout(False)

        ' перерисовать панель, т.к. сама делает плохо
        TableLayoutPanelDiscrete.Refresh()

        If countPanel = 1 Then
            ' только одна строка видима
            SplitContainerForm.SplitterDistance = SplitContainerForm.Height - rowHeight - rowDecorative
        Else
            ' только две строки видимы, остальное прокрутка
            SplitContainerForm.SplitterDistance = SplitContainerForm.Height - 2 * rowHeight - rowDecorative
        End If

        ' дурка с ресайзом для перерисовки
        Dim oldSplitterDistance As Integer = SplitContainerForm.SplitterDistance
        SplitContainerForm.SplitterDistance = SplitContainerForm.Height
        SplitContainerForm.SplitterDistance = SplitContainerForm.Height - 200
        SplitContainerForm.SplitterDistance = oldSplitterDistance
    End Sub

    ''' <summary>
    ''' Создать строку для таблицы с дискретными индикаторами
    ''' </summary>
    ''' <param name="indexPanel"></param>
    ''' <returns></returns>
    Private Function CreateTableLayoutPanel(indexPanel As Integer) As TableLayoutPanel
        Dim newTableLayoutPanel As TableLayoutPanel = New TableLayoutPanel()

        newTableLayoutPanel.SuspendLayout()
        With newTableLayoutPanel
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
            .ColumnCount = TableLayoutPanelColumnCount
            Dim ledWidht As Single = 100 / TableLayoutPanelColumnCount

            For column As Integer = 1 To TableLayoutPanelColumnCount
                .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, ledWidht))
                .Controls.Add(CreateLed(), column - 1, 0)
            Next

            .Dock = DockStyle.Fill
            .GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            .Location = New Point(3, 3)
            .Margin = New Padding(0)
            .Name = "TableLayoutPanel" & indexPanel
            .RowCount = 1
            .RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
            .Size = New Size(984, 57)
        End With
        newTableLayoutPanel.ResumeLayout(False)

        newTableLayoutPanel.Refresh()
        Return newTableLayoutPanel
    End Function

    ''' <summary>
    ''' Создать дискретный индикатор
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateLed() As WindowsForms.Led
        Dim newLed As WindowsForms.Led = New WindowsForms.Led()

        CType(newLed, ISupportInitialize).BeginInit()
        With newLed
            .Dock = DockStyle.Fill
            .LedStyle = LedStyle.Square3D
            .Location = New Point(3, 3)
            .Margin = New Padding(0)
            .Name = "Led" & counterLed
            .OffColor = Color.Silver
            .OnColor = Color.Red
            .Size = New Size(58, 51)
            .Visible = False
        End With
        CType(newLed, ISupportInitialize).EndInit()

        counterLed += 1
        listDiscreteLed.Add(newLed)

        Return newLed
    End Function

    ''' <summary>
    ''' Выгрузить аварийные индикаторы
    ''' </summary>
    Protected Sub UnloadAlarmButton()
        Dim I As Integer
        Dim KeysCopy(AlarmPulseButton.Count - 1) As Integer

        For Each key As Integer In AlarmPulseButton.Keys
            KeysCopy(I) = key
            I += 1
        Next

        For I = KeysCopy.GetUpperBound(0) To 0 Step -1
            If KeysCopy(I) <> 0 Then
                Controls.Remove(CType(AlarmPulseButton(KeysCopy(I)), PulseButton.PulseButton))
                AlarmPulseButton.Remove(KeysCopy(I))
            End If
        Next

        keyFirstButtonAlarm = 0
        keyLastButtonAlarm = 0
    End Sub
    ''' <summary>
    ''' Загрузить аварийные индикаторы
    ''' </summary>
    ''' <param name="index"></param>
    Protected Sub PopulateAlarmButton(ByVal index As Integer)
        Dim alarmButton As New PulseButton.PulseButton
        Dim buttonAlarmLeft, buttonAlarmTop As Integer

        keyFirstButtonAlarm = 0
        keyLastButtonAlarm = index
        alarmButton.Name = index.ToString
        'CType(alarmButton, System.ComponentModel.ISupportInitialize).BeginInit()
        Controls.Add(alarmButton)
        'CType(alarmButton, System.ComponentModel.ISupportInitialize).EndInit()

        With alarmButton
            .Size = New Size(250, 57)

            ' правый угол Х
            buttonAlarmLeft = SlidePlot.Width + SplitContainerGraph1.Panel1.ClientRectangle.Width - .Width - 60
            ' правый угол У
            buttonAlarmTop = Height - (ToolStripContainer1.ContentPanel.ClientRectangle.Height + StatusStripMain.Height) - 90

            .Location = New Point(buttonAlarmLeft, buttonAlarmTop)
            .Visible = False

            .ButtonColorBottom = Color.Maroon
            .ButtonColorTop = Color.Red
            .CornerRadius = 5
            .Font = New Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte)) ' ((Byte)(204)))
            .ForeColor = Color.Yellow
            .NumberOfPulses = 2
            .PulseColor = Color.Maroon
            .PulseSpeed = 0.6F
            .PulseWidth = 20
            .ShapeType = PulseButton.PulseButton.Shape.Rectangle
            .AutoSize = True
        End With

        AddHandler alarmButton.Click, AddressOf AlarmButton_ClickEvent

        AlarmPulseButton.Add(index, alarmButton)
    End Sub
    ''' <summary>
    ''' Зажечь индикатор аварийного сигнала
    ''' </summary>
    Protected Sub GenerateAlarm()
        PulseButtonTakeOffAlarm.BringToFront()
        'Try
        '    If Not writerAOPoint1 Is Nothing Then
        '        writerAOPoint1.WriteSingleSample(True, 5)
        '    End If
        PulseButtonTakeOffAlarm.Visible = True
        'Catch ex As DaqException
        '    MessageBox.Show(ex.ToString)
        '    myTaskCWAOPoint1.Dispose()
        '    myTaskCWAOPoint1 = Nothing
        'End Try
    End Sub
    ''' <summary>
    ''' Погасить индикатор аварийного сигнала
    ''' </summary>
    Protected Sub GenerateNull() ' перенос в frmDigital
        PulseButtonTakeOffAlarm.Visible = False
    End Sub
    ''' <summary>
    ''' Погасить индикатор аварийного сигнала
    ''' </summary>
    Private Sub PulseButtonTakeOffAlarm_Click(sender As Object, e As EventArgs) Handles PulseButtonTakeOffAlarm.Click
        GenerateNull()
    End Sub
    ''' <summary>
    ''' Погасить индикатор аварийного сигнала
    ''' </summary>
    Private Sub AlarmButton_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        AlarmPulseButton(CInt(CType(eventSender, PulseButton.PulseButton).Name)).Visible = False
    End Sub

    Private Sub SplitContainerGraf1_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles SplitContainerGraph1.Resize, SplitContainerGraph2.Resize, SplitContainerGraph1.Panel1.Resize
        ResizeAlarmBotton()
    End Sub
    ''' <summary>
    ''' Подстроить размеры индикаторов аварийного сигнала
    ''' </summary>
    Private Sub ResizeAlarmBotton()
        If isFormLoaded AndAlso AlarmPulseButton.Count > 1 Then
            Dim top As Integer
            Dim left As Integer = SlidePlot.Width + SplitContainerGraph1.Panel1.ClientRectangle.Width - AlarmPulseButton(keyFirstButtonAlarm).Width - 60
            Dim width As Integer = AlarmPulseButton(keyFirstButtonAlarm).Width
            Dim height As Integer = AlarmPulseButton(keyFirstButtonAlarm).Height
            Dim keySecond As Integer = keyFirstButtonAlarm

            For Each key As Integer In AlarmPulseButton.Keys
                If key = keyFirstButtonAlarm Then
                    top = AlarmPulseButton(keyFirstButtonAlarm).Top
                Else
                    top = AlarmPulseButton(keySecond).Top + AlarmPulseButton(keySecond).Height + 1
                End If

                keySecond = key
                AlarmPulseButton(key).SetBounds(left, top, width, height)
            Next
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Проверка подключаемых расчётных модулей
    ''' </summary>
    Private Sub CheckSolveModule()
        IsUseCalculationModule = False

        If MainMDIFormParent.ModuleSolveManager IsNot Nothing Then
            ' каталог для работы с модулями существует
            If MainMDIFormParent.ModuleSolveManager.IsEnableModules Then
                MainMDIFormParent.ModuleSolveManager.LoadInheritanceForms()
                MainMDIFormParent.MenuConfigurationCalculationModule.Enabled = False
                MainMDIFormParent.MenuSetComPort.Enabled = False
                ' известны все имена СписокРасчетныхПараметров которые необходимо добавить в список каналов
                ' там уже могут быть добавлены каналы цифровых входов поэтому добавлять только в конец
                MainMDIFormParent.ModuleSolveManager.ParentFormMain = Me
                IsUseCalculationModule = True
            Else
                ' может модули в каталоге есть, а в конфигуратор не включены
                ' перед очисткой надо проверить снимки не содержат ли они каналы расчётных параметров
                AutomaticParsingRecording($"SELECT COUNT(*) FROM { ChannelLast} WHERE Погрешность=" & indexCalculated.ToString)
                MainMDIFormParent.ModuleSolveManager.ClearBaseChannelsFromCalculationParameters()
            End If
            ' в любом случае надо обновить базу т.к. для клиентов важны все каналы
            ' но там уже могут быть добавлены каналы цифровых входов
            LoadChannels()
            UpdateTableRegimeForTcpClient()
        End If
    End Sub

    ''' <summary>
    ''' Проверка на изменение имен каналов
    ''' Считать последнюю запись из базы данных БазаСнимков и определить каналы в снимке.
    ''' При обнаружении отсутствия (значит канал был переименован) выполнить принудительную архивацию.
    ''' Хотя если каналы были переименованы вручную от порчи снимков это не спасёт, но новые снимки будут корректными.
    ''' Сравнить их с именами каналов в базе каналов.
    ''' </summary>
    Protected Sub CheckOnModificationNameChannel()
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim cmd As OleDbCommand = cn.CreateCommand
        Dim rdr As OleDbDataReader
        Dim strSQL As String = "SELECT KeyID FROM [БазаСнимков]"
        Dim KeyID As Integer
        Dim configurationString As String = Nothing

        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cn.Open()
        rdr = cmd.ExecuteReader

        Do While rdr.Read
            KeyID = CInt(rdr("KeyID"))
        Loop

        rdr.Close()

        If KeyID = 0 Then ' наверно нет записей
            cn.Close()
            Exit Sub
        End If

        strSQL = "SELECT СтрокаКонфигурации FROM [БазаСнимков] WHERE [KeyID]=" & KeyID
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() = True Then
            configurationString = CStr(rdr("СтрокаКонфигурации"))
        End If

        rdr.Close()
        cn.Close()

        If configurationString IsNot Nothing Then
            Dim lenghtConfig As Integer = Len(configurationString)
            Dim lostChannels As New List(Of String) 'отсутствуетКанал
            Dim strTemp As String
            Dim success As Boolean
            Dim start As Integer = 1
            Dim J As Integer
            Dim I As Integer = 1

            Do
                strTemp = Mid(configurationString, start, InStr(start, configurationString, Separator) - start)
                success = False

                For J = 1 To UBound(ParametersType)
                    If ParametersType(J).NameParameter = strTemp Then
                        success = True
                        Exit For
                    End If
                Next

                If success = False Then
                    lostChannels.Add(strTemp)
                End If

                start = InStr(start, configurationString, Separator) + 1
                I += 1
            Loop While start < lenghtConfig

            If lostChannels.Count > 0 Then
                Dim message As String = Nothing
                I = 0

                For Each StrChannel As String In lostChannels
                    message += StrChannel & vbCrLf
                    I += 1
                    If I > 10 Then
                        message += "и еще более..." & vbCrLf
                        Exit For
                    End If
                Next

                Const caption As String = "Проверка соответствия каналов"
                Dim text As String = $"Следующие каналы были переименованы или отключены расчётные модули:{vbCrLf}{message}В базе существуют записи испытаний c этими каналами!{vbCrLf}{vbCrLf}Будет произведён автоматический разбор записей в архив, немного подождите."
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                If MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                    ArchivingByOpeningWindow()
                End If
            End If
        End If
    End Sub

#Region "Обработчики событий контролов"
    ''' <summary>
    ''' ' Задержка на мертвую зону пока не раскрутился N1
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub TimerButtonRun_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TimerButtonRun.Tick
        TimerButtonRun.Enabled = False
    End Sub

    ''' <summary>
    ''' Настройка конфигурации режима
    ''' </summary>
    Private Sub MenuConfigurationChannels_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuConfigurationChannels.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuConfigurationChannels_Click)}> Показать форму настройки конфигурации каналов для режима")
        TuneConditionsRegime()
        ApplyRegimeRegistrator()
        AllGraphParametersByParameter = Nothing ' чтобы вызвать повторную инициализацию
    End Sub

    Private Sub MenuSelectiveTextControl_Click(sender As Object, e As EventArgs) Handles MenuSelectiveTextControl.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuSelectiveGaugeControl_Click)}> Загрузка формы настройки выборочного текстового контроля")
        ' настройка вывода параметров
        Dim frmFormTuningSelectiveBase As New FormTuningSelectiveBase(TypeFormTuningSelective.TextControl,
                                                                      If(IsShowTextControl, frmTextControl, Nothing),
                                                                      ParametersType,
                                                                      CopyListOfParameter)
        frmFormTuningSelectiveBase.ShowDialog()
    End Sub

    ''' <summary>
    ''' Отображение окна текстового контроля
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuShowTextControl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuShowTextControl.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuShowTextControl_Click)}> Отображение окна текстового контроля")
        frmTextControl = New FormTextControl(Me)
        frmTextControl.Show()
        frmTextControl.Activate()
        MenuShowTextControl.Enabled = False
    End Sub

    ''' <summary>
    ''' Загрузка формы настройки выборочного графического контроля
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuSelectiveGaugeControl_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuSelectiveGraphControl.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuSelectiveGaugeControl_Click)}> Загрузка формы настройки выборочного графического контроля")
        ' настройка вывода параметров
        Dim frmFormTuningSelectiveBase As New FormTuningSelectiveBase(TypeFormTuningSelective.GraphControl,
                                                                      If(IsShowGraphControl, frmGraphControl, Nothing),
                                                                      ParametersType,
                                                                      CopyListOfParameter)
        frmFormTuningSelectiveBase.ShowDialog()
    End Sub
    ''' <summary>
    ''' Загрузка окна с выборочными графическими параметрами
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuShowGraphControl_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuShowGraphControl.Click
        ' отобразить параметры - если форма регистратор то
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuShowGraphControl_Click)}> Загрузка окна с выборочными графическими параметрами")
        frmGraphControl = New FormGraphControl(Me)
        frmGraphControl.Show()
        frmGraphControl.Activate()
        MenuShowGraphControl.Enabled = False
    End Sub

    ''' <summary>
    ''' Показать окно наблюдения параметров в диапазоне
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuParameterInRange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuParameterInRange.Click
        If Not IsSnapshot Then
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuParameterInRange_Click)}> Показать окно наблюдения параметров в диапазоне")
            frmParameterInRange = New FormParameterInRange(Me)
            frmParameterInRange.Show()
            frmParameterInRange.Activate()
        End If
    End Sub
#End Region

    ''' <summary>
    ''' пользовательское событие наследуется от EventArgs
    ''' </summary>
    Public Class AcquiredDataEventArgs
        Inherits EventArgs

        'получить массив собранных значений
        Public Sub New(ByRef inArrПарамНакопленные As Double())
            ArrПарамНакопленные = inArrПарамНакопленные
        End Sub 'New
        'сюда можно напихать другие свойства

        'можно передать все накопленные
        'arrСреднее(TempПараметр.ИндексВМассивеПараметров, x)
        'а можно и конкретно осредненные или собранные
        'arrПарамНакопленные(N) = среднее

        Public Property ArrПарамНакопленные() As Double()
    End Class

End Class