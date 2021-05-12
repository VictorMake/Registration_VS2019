Option Strict Off

Imports System.Data.OleDb
Imports NationalInstruments.Analysis.Math
Imports NationalInstruments.DAQmx
Imports Microsoft.Office.Interop
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports MathematicalLibrary
'Imports System.Threading.Tasks

Friend Class FormTarir
    ''' <summary>
    ''' Режим Опроса
    ''' </summary>
    Private Enum TypeAcquisition
        NotHysteresis = 0 ' Гистерезиса Нет
        WithHysteresis = 1 ' Гистерезис Есть
        Table = 2 ' Таблица
    End Enum
    Private mTypeAcquisition As TypeAcquisition

    ''' <summary>
    ''' Запуск
    ''' </summary>
    ''' <returns></returns>
    Public Property IsRunning() As Boolean

    Private mIsGroupTarir As Boolean
    Public Property IsGroupTarir() As Boolean
        Get
            IsGroupTarir = mIsGroupTarir
        End Get
        Set(ByVal Value As Boolean)
            mIsGroupTarir = Value
            If mIsGroupTarir = True Then
                ButtonGrup.Enabled = False
            Else
                ButtonGrup.Enabled = True
            End If
        End Set
    End Property

    Private ReadOnly MainMDIFormParent As FormMainMDI

    Private Class ArrayMeasurement
        Public Property First As Double()
        Public Property Back As Double()
    End Class

    Private mListMeasurement As List(Of ArrayMeasurement) ' сохранение опросов точек
    Private Const PointsCount As Integer = 100      ' кол. Точек Графика
    Private Const maxCountTarir As Integer = 14     ' максимальное число точек тарировки
    Private indexChannel As Integer
    Private acquisitionCount As Integer             ' количество опросов одной точки
    Private waitSleep As Integer                    ' задержка
    Private currentPoint As Integer = 1             'текущая Точка
    Private currentDirection As Integer             ' текущий Ход
    Private acquisitionVolt(,,) As Double           ' измеренные (вольты АЦП) значения точек по всем отсчетам
    Private polynomialPhysical(,,) As Double        ' пересчитанные (физика) значения точек по всем отсчетам после полинома

    '--- переменные для метрологической обработки -----------------
    Private среднееФизикаПолином As Double(,)       ' средние значения (физика) прямого и обратного  в контрольной точке
    Private оценкаСКО As Double()                   ' оценка среднеквадратического отклонения случайной величины
    Private систематическаяПогрешности As Double()  ' систематическая составляющая погрешности
    Private вариация As Double()                    ' вариация
    Private доверительныйИнтервал As Double(,)      ' доверительный интервал в котором с заданной вероятностью Р находится погрешность
    Private quadraticMeanAllRange As Double                    ' СКО во всем диапазоне
    Private интервалПогрешностиКанала(2) As Double  ' во всем диапазоне
    Private maxСистематическая As Double            ' максимальное значение по всем сечениям
    Private maxВариация As Double                   ' максимальное значение по всем сечениям
    Private погрешность As Double                   ' относительная погрешность измерительного канала
    Private погрешностьПроцент As Double            ' относительная погрешность измерительного канала
    Private Const T_0_95 As Double = 2.0#           ' для заданной доверительной вероятности Р=0.95
    Private polynomialOrder As Integer              ' cтепеньПолинома 

    '--- переменные для ListView ----------------------------------------------
    Private isSortOrder As Boolean ' порядок Сортировки
    Private sampleX() As Double

    '--- для форматирования ---------------------------------------
    Private polinomialTextBox(5) As TextBox
    Private ButtonPoints() As RadioButton
    Private TextEtalons, TextBoxForwards, TextBacks As TextBox()

    '--- Для протокола --------------------------------------------------------
    Private numberParameter, nameParameter, numberChannel, minVal, maxVal, unitChannel As String

    Private mFormBrowser As FormWebBrowser
    Private mFormChannel As FormChannel
    Private frmMetrologyGroup As FormMetrologyGroup

    Private whiteNoisePsysical, whiteNoiseAD As Double
    Private physicalMin, physicalMax, ADmin, ADmax As Double
    Private isImitatorMetrology As Boolean ' имитатор Метрологии

    '--- Групповая тарировка --------------------------------------
    Private acquisitionGroupVolt(,,,) As Double ' замерАЦП_Группа
    Private selectedIndexParameterGroup As Integer
    Private currentIndexParameterGroup As Integer

    Private selectChassislName As String = String.Empty
    Private selectChannelName As String = String.Empty
    Private isFormLoaded As Boolean

#Region "FormTarir"
    ''' <summary>
    ''' Матобработка Выделенного Параметра Из Группы
    ''' </summary>
    ''' <param name="selectedIndex"></param>
    Public Sub MathematicalTreatmentSampleSelectedParameterFromGroup(ByVal selectedIndex As Integer)
        Dim I, J, K As Integer

        Re.Dim(sampleX, acquisitionCount - 1)

        For I = 0 To acquisitionCount - 1
            sampleX(I) = acquisitionGroupVolt(selectedIndex, currentPoint, I + 1, currentDirection)
        Next

        selectedIndexParameterGroup = MetrologyGroup(selectedIndex)
        ComboBoxParameters.SelectedIndex = selectedIndexParameterGroup - 1

        MathematicalTreatmentSample()

        For I = 1 To PointTarirCount
            For J = 1 To acquisitionCount
                For K = 1 To 2
                    acquisitionVolt(I, J, K) = acquisitionGroupVolt(selectedIndex, I, J, K)
                Next
            Next
        Next

        RewriteListViewMathMeasurement()
        MathCoefficientPolynomial()
    End Sub

    ''' <summary>
    ''' Инициализация Переменных Группового Опроса
    ''' </summary>
    Public Sub InitializeVariablesGroupAcquire()
        Re.Dim(acquisitionGroupVolt, UBound(MetrologyGroup), PointTarirCount, acquisitionCount, 2)
    End Sub

    Public Sub New(ByVal frmParent As FormMainMDI)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        MdiParent = frmParent
        MainMDIFormParent = frmParent
        InstallQuestionForm(WhoIsExamine.Taring)
    End Sub

    Private Sub FormTarir_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна: " & Text)

        Dim I As Integer

        MainMdiForm.MenuNewWindowTarir.Enabled = False
        MenuNewWindowRegistration.Enabled = MainMdiForm.MenuNewWindowRegistration.Enabled
        MenuNewWindowSnapshot.Enabled = MainMdiForm.MenuNewWindowSnapshot.Enabled
        MenuNewWindowTarir.Enabled = MainMdiForm.MenuNewWindowTarir.Enabled
        MenuNewWindowClient.Enabled = MainMdiForm.MenuNewWindowClient.Enabled
        MenuNewWindowDBaseChannels.Enabled = MainMdiForm.MenuNewWindowDBaseChannels.Enabled
        MenuDebugExamineParameter.Enabled = Not IsCompactRio

        ' по умолчанию начальное присваивание
        indexChannel = 1
        PointTarirCount = 2 ' начальное
        If IsCompactRio Then NEditCountSamples.Value = 100
        acquisitionCount = CInt(NEditCountSamples.Value)
        waitSleep = 0
        currentDirection = 1
        LoadTableMetrology(PointTarirCount)
        PopulateComboBoxParameters()
        InitializeVariables()
        CheckHysteresis.CheckState = CheckState.Unchecked

        LabelStend.Text = "Стенд №" & StandNumber

        polinomialTextBox(0) = TextPolynomial0
        polinomialTextBox(1) = TextPolynomial1
        polinomialTextBox(2) = TextPolynomial2
        polinomialTextBox(3) = TextPolynomial3
        polinomialTextBox(4) = TextPolynomial4
        polinomialTextBox(5) = TextPolynomial5

        mTypeAcquisition = TypeAcquisition.NotHysteresis

        ButtonPoints = {ButtonPoint1, ButtonPoint2, ButtonPoint3, ButtonPoint4, ButtonPoint5, ButtonPoint6, ButtonPoint7, ButtonPoint8, ButtonPoint9, ButtonPoint10, ButtonPoint11, ButtonPoint12, ButtonPoint13, ButtonPoint14, ButtonPoint15}
        TextEtalons = {TextEtalon1, TextEtalon2, TextEtalon3, TextEtalon4, TextEtalon5, TextEtalon6, TextEtalon7, TextEtalon8, TextEtalon9, TextEtalon10, TextEtalon11, TextEtalon12, TextEtalon13, TextEtalon14, TextEtalon15}
        TextBoxForwards = {TextForward1, TextForward2, TextForward3, TextForward4, TextForward5, TextForward6, TextForward7, TextForward8, TextForward9, TextForward10, TextForward11, TextForward12, TextForward13, TextForward14, TextForward15}
        TextBacks = {TextBack1, TextBack2, TextBack3, TextBack4, TextBack5, TextBack6, TextBack7, TextBack8, TextBack9, TextBack10, TextBack11, TextBack12, TextBack13, TextBack14, TextBack15}

        For I = 0 To maxCountTarir
            ButtonPoints(I).Tag = I.ToString
            TextEtalons(I).Tag = I.ToString
            TextBoxForwards(I).Tag = I.ToString
            TextBacks(I).Tag = I.ToString
            AddHandler ButtonPoints(I).CheckedChanged, AddressOf ButtonPoints_CheckedChanged
            AddHandler TextEtalons(I).Leave, AddressOf TextEtalons_Leave
            AddHandler TextEtalons(I).Enter, AddressOf TextEtalons_Enter
            AddHandler TextEtalons(I).KeyDown, AddressOf TextEtalons_KeyDown
            AddHandler TextEtalons(I).KeyPress, AddressOf TextEtalons_KeyPress

            AddHandler TextBoxForwards(I).Leave, AddressOf TextBoxForwards_Leave
            AddHandler TextBoxForwards(I).Enter, AddressOf TextBoxForwards_Enter
            AddHandler TextBoxForwards(I).KeyDown, AddressOf TextBoxForwards_KeyDown
            AddHandler TextBoxForwards(I).KeyPress, AddressOf TextBoxForwards_KeyPress
        Next

        InitializeListViews()

        isFormLoaded = True
        SelectTypeAcquisition()
        ComboBoxParameters.SelectedIndex = 0 ' по умолчанию первый злемент активный
        'ВклВыклПанелиФормы(False)
        mListMeasurement = New List(Of ArrayMeasurement)
        ' по умолчанию в начале 2 элемента
        PopulateListMeasurement()

        CollectionForms.Add(Text, Me)
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub FormTarir_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна: " & Text)

        For I As Integer = 0 To maxCountTarir
            ButtonPoints(I).Checked = False
        Next

        MainMdiForm.MenuNewWindowTarir.Enabled = True
        Erase ButtonPoints
        Erase TextEtalons
        Erase TextBoxForwards
        Erase TextBacks

        For Each tempForm In CollectionForms.Forms.Values
            If CStr(tempForm.Tag) = TagFormDaughter Then CType(tempForm, FormMain).MenuNewWindowTarir.Enabled = MainMdiForm.MenuNewWindowTarir.Enabled
        Next

        If IsRunning Then InstallQuestionForm(WhoIsExamine.Nobody)

        If IsGroupTarir Then
            frmMetrologyGroup.Close()
            frmMetrologyGroup = Nothing
        End If

        CollectionForms.Remove(Text)
        RunRegistrationForm()
        Dispose()
    End Sub

    Private Sub InitializeVariables()
        Re.Dim(PhisicalEtalon, PointTarirCount)
        Re.Dim(acquisitionVolt, PointTarirCount, acquisitionCount, 2)
        Re.Dim(polynomialPhysical, PointTarirCount, acquisitionCount, 2)
        Re.Dim(AverageInput, PointTarirCount)
        Re.Dim(среднееФизикаПолином, PointTarirCount, 2)
        Re.Dim(оценкаСКО, PointTarirCount)
        Re.Dim(систематическаяПогрешности, PointTarirCount)
        Re.Dim(вариация, PointTarirCount)
        Re.Dim(доверительныйИнтервал, PointTarirCount, 2)

        If mIsGroupTarir Then InitializeVariablesGroupAcquire()
    End Sub

    ''' <summary>
    ''' Загрузка Таблицы Метрология
    ''' </summary>
    ''' <param name="pointsCount"></param>
    Private Sub LoadTableMetrology(ByVal pointsCount As Integer)
        Dim tableRecord(8) As String

        ListMetrology.Items.Clear()

        ' Создать переменную, чтобы добавлять объекты ListItem.
        For I As Integer = 1 To pointsCount
            tableRecord(0) = I.ToString
            ListMetrology.Items.Add(New ListViewItem(tableRecord))
        Next
    End Sub

    Private Sub PopulateListMeasurement()
        mListMeasurement.Clear()
        For I As Integer = 0 To CInt(NEditCountPoints.Value - 1)
            mListMeasurement.Add(New ArrayMeasurement)
        Next
    End Sub

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

    Private Sub InitializeListViews()
        'ListViewMathMeasurement.Columns.Clear()
        ListViewMathMeasurement.Columns.Add("№ опроса", "№ опроса", ListViewMathMeasurement.Width \ 3, HorizontalAlignment.Left, 0)
        ListViewMathMeasurement.Columns.Add("Значение", "Значение", (ListViewMathMeasurement.Width * 2 \ 3) - 8, HorizontalAlignment.Right, 0)

        'ListViewМетрология.Columns.Clear()
        'ListViewМетрология.Items.Clear()
        ListMetrology.Columns.Add("№", "№", (ListMetrology.Width * 1 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("АЦП", "АЦП", (ListMetrology.Width * 3 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("Эталон", "Эталон", (ListMetrology.Width * 2 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("Прямой", "Прямой", (ListMetrology.Width * 2 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("Обратный", "Обратный", (ListMetrology.Width * 2 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("Вариация", "Вариация", (ListMetrology.Width * 1 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("ССП", "ССП", (ListMetrology.Width * 2 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("СКО", "СКО", (ListMetrology.Width * 2 \ 18) - 1, HorizontalAlignment.Left, 0)
        ListMetrology.Columns.Add("Погрешность", "Погрешность", (ListMetrology.Width * 3 \ 18) - 3, HorizontalAlignment.Left, 0)
    End Sub

    Private Sub FormTarir_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        If Me.IsHandleCreated AndAlso isFormLoaded Then FormTarirResize()
    End Sub

    Public Sub FormTarirResize()
        ListMetrology.Columns("№").Width = (ListMetrology.Width * 1 \ 18) - 1
        ListMetrology.Columns("АЦП").Width = (ListMetrology.Width * 3 \ 18) - 1
        ListMetrology.Columns("Эталон").Width = (ListMetrology.Width * 2 \ 18) - 1
        ListMetrology.Columns("Прямой").Width = (ListMetrology.Width * 2 \ 18) - 1
        ListMetrology.Columns("Обратный").Width = (ListMetrology.Width * 2 \ 18) - 1
        ListMetrology.Columns("Вариация").Width = (ListMetrology.Width * 1 \ 18) - 1
        ListMetrology.Columns("ССП").Width = (ListMetrology.Width * 2 \ 18) - 1
        ListMetrology.Columns("СКО").Width = (ListMetrology.Width * 2 \ 18) - 1
        ListMetrology.Columns("Погрешность").Width = (ListMetrology.Width * 3 \ 18) - 3
    End Sub

    Private Sub ShowMessageToStatusBar(ByVal strMessage As String)
        LabelStatus.Text = strMessage
    End Sub
#End Region

#Region "Опрос канала"
#Region "AcquisitionCompactRioTask"
    Public TokenSource As CancellationTokenSource
    Private taskLoopWhileEndAcquisitionCompactRio As Tasks.Task
    Private isTaskAcquisitionCompactRioRunning As Boolean
    Private acquisitionCompactRio As Double()
    Private counterAcquiredDataCompactRio As Integer
    'Private counterLoopTask As Integer
    'Dim average As Double

    ' Запуск сбора в событии таймера от формы работы с шасси с ожиданием флага окончания в отдельной Task.
    ' По окончанию накопления останавливается таймер и отдаётся массив собранных данных.
    ''' <summary>
    ''' тест замера
    ''' </summary>
    ''' <returns></returns>
    Private Function MeasurementCompactRio() As Double()
        If RegistrationMain IsNot Nothing AndAlso RegistrationMain.IsRun Then InstallQuestionForm(WhoIsExamine.Nobody)
        If MainMDIFormParent.GFormCompactRio IsNot Nothing AndAlso MainMDIFormParent.GFormCompactRio.IsStartAcquisition Then MainMDIFormParent.GFormCompactRio.StopAcquisition()

        MainMDIFormParent.GFormCompactRio.Initialize()
        counterAcquiredDataCompactRio = 0
        'counterLoopTask = 0
        'average = 0
        Re.Dim(acquisitionCompactRio, acquisitionCount - 1)
        ShowHideControlAcquired(True)
        ProgressBar.Maximum = acquisitionCount
        ' здесь надо awaite вызов
        MainMDIFormParent.GFormCompactRio.StartAcquisitionTimer(AddressOf MainMDIFormParent.GFormCompactRio.TarirTimerTick)
        RunStopAcquisitionCompactRio(True)

        Do
            ' создаётся задача и запускается с длительным ожиданием
            ' внутри сбора вывести слайдер процесса выполнения
            ' в задаче запускается цикл проверки, что таймер остановлен (условие If CounterAcquiredData > 100 Then)
            ' Wait  ждать окончания этой задачи
            Thread.Sleep(100) 'MainMDIFormParent.GFormTestCompactRio.TimerIntervalWait)
            Application.DoEvents()
        Loop While isTaskAcquisitionCompactRioRunning

        UpdateProgressStatus(0)
        ShowHideControlAcquired(False)
        'MessageBox.Show($"CounterAcquiredData = {counterAcquiredData}; CounterLoopTask = {counterLoopTask}")
        Return acquisitionCompactRio
    End Function

    ''' <summary>
    ''' Запуск задачи ожидания флага окончания сбора.
    ''' </summary>
    ''' <param name="isStart"></param>
    Private Sub RunStopAcquisitionCompactRio(isStart As Boolean)
        If isStart Then
            TokenSource = New CancellationTokenSource
            taskLoopWhileEndAcquisitionCompactRio = Tasks.Task.Factory.StartNew(Sub() LoopWhileEndAcquisitionCompactRio(TokenSource.Token), TokenSource.Token, Tasks.TaskCreationOptions.LongRunning)
        Else
            If TokenSource IsNot Nothing Then TokenSource.Cancel() ' прервать задачу 
            taskLoopWhileEndAcquisitionCompactRio = Nothing
        End If
        isTaskAcquisitionCompactRioRunning = isStart
    End Sub

    ''' <summary>
    ''' Задачи ожидания флага окончания сбора.
    ''' Тупой цикл с задержкой.
    ''' </summary>
    ''' <param name="ct"></param>
    Private Sub LoopWhileEndAcquisitionCompactRio(ByVal ct As CancellationToken)
        ' Прерывание уже было запрошено?
        If ct.IsCancellationRequested Then ct.ThrowIfCancellationRequested()

        Dim timerIntervalWait As Integer = MainMDIFormParent.GFormCompactRio.TimerIntervalWait
        Do
            If ct.IsCancellationRequested Then
                'ct.ThrowIfCancellationRequested() ' выйти по исключению
                Exit Do ' завершить задачу
            End If

            If taskLoopWhileEndAcquisitionCompactRio Is Nothing Then
                ' скорее всего остановлено
                Exit Sub
            Else
                ' Завершить цикл избегая напрасную трату времени CPU
                taskLoopWhileEndAcquisitionCompactRio.Wait(timerIntervalWait)
            End If

            'counterLoopTask += 1
        Loop While isTaskAcquisitionCompactRioRunning ' True
    End Sub

    ''' <summary>
    ''' В событии обновления значения канала производится вывод значения канала и процент исполнения.
    ''' </summary>
    ''' <param name="voltChannel"></param>
    ''' <remarks></remarks>
    Public Sub UpdateReadValue(voltChannel As Double)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() UpdateReadValue(voltChannel)))
        Else
            'tsChannelVolt.Text = voltChannel.ToString
            UpdateProgressStatus(counterAcquiredDataCompactRio)
        End If
    End Sub

    ''' <summary>
    ''' Обновление прогресса исполнения.
    ''' </summary>
    ''' <param name="percent"></param>
    Public Sub UpdateProgressStatus(percent As Integer)
        If InvokeRequired Then
            ' Выполнить делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления. 
            Invoke(New MethodInvoker(Sub() UpdateProgressStatus(percent)))
        Else
            ProgressBar.Value = percent
            LabelProgressBar.Text = percent.ToString
            ProgressBar.Invalidate() '.Refresh()
            LabelProgressBar.Invalidate() '.Refresh()
        End If
    End Sub

    ''' <summary>
    ''' Скрыть или показать контролы при опросе канала CompactRio
    ''' </summary>
    Private Sub ShowHideControlAcquired(isVisibleProgress As Boolean)
        ProgressBar.Visible = isVisibleProgress
        LabelProgressBar.Visible = isVisibleProgress
        ButtonRunSample.Visible = Not isVisibleProgress

        If isVisibleProgress Then
            RadioButtonBack.Visible = False
            RadioButtonForward.Visible = False
        Else
            If CheckHand.CheckState = CheckState.Checked Then
                RadioButtonBack.Visible = False
            Else
                If CheckHysteresis.CheckState = CheckState.Checked Then RadioButtonBack.Visible = True
            End If
            RadioButtonForward.Visible = Not (CheckHand.CheckState = CheckState.Checked)
        End If
        Me.Refresh()
    End Sub

    ''' <summary>
    ''' Обновление данных на экране из события сбора
    ''' DataValuesFromServer As Double() - Данные значений каналов, полученные по сети от шасси CompactRio
    ''' </summary>
    Friend Sub AcquiredData(DataValuesFromServer As Double())
        'average += DataValuesFromServer(indexChannel)
        Dim voltChannel As Double = DataValuesFromServer(indexChannel) 'average 
        acquisitionCompactRio(counterAcquiredDataCompactRio) = voltChannel
        UpdateReadValue(voltChannel)
        counterAcquiredDataCompactRio += 1

        If counterAcquiredDataCompactRio >= acquisitionCount Then
            If MainMDIFormParent.GFormCompactRio IsNot Nothing AndAlso MainMDIFormParent.GFormCompactRio.IsStartAcquisition Then
                MainMDIFormParent.GFormCompactRio.StopAcquisition()
                RunStopAcquisitionCompactRio(False)
            End If
        End If
    End Sub

    'Private Function TestUpdateReadValue() As Double()
    '    Dim rnd As New Random()
    '    Dim voltChannel As Double
    '    'Dim acquisitionVolts(acquisitionCount - 1) As Double
    '    Re.Dim(acquisitionVolts, acquisitionCount - 1)

    '    ShowHideControlAcquired(True)
    '    ProgressBar.Maximum = acquisitionCount
    '    taskRunning = True
    '    'currentSample = 0

    '    For I As Integer = 1 To acquisitionCount - 1
    '        voltChannel = rnd.[Next](10)
    '        acquisitionVolts(I) = acquisitionVolts(I - 1) + voltChannel
    '        UpdateReadValue(voltChannel)
    '        Thread.Sleep(50)
    '    Next

    '    ShowHideControlAcquired(False)

    '    Return acquisitionVolts
    'End Function
#End Region

    ''' <summary>
    ''' Замер Одного Параметра
    ''' </summary>
    Private Sub AcquisitionOneParameter()
        ButtonRunSample.Enabled = False

        If IsRunning = False Then
            InstallQuestionForm(WhoIsExamine.Taring)
            ComboBoxParameters_SelectedIndexChanged(ComboBoxParameters, New EventArgs)
        End If

        Re.Dim(sampleX, acquisitionCount - 1)

        If IsTaskRunning = False Then
            Cursor = Cursors.WaitCursor

            Try
                Dim volts() As Double

                IsTaskRunning = True
                If IsWorkWithDaqController Then
                    volts = AcquisitionDAQmxTask()
                ElseIf IsCompactRio Then
                    volts = MeasurementCompactRio()
                End If
                IsTaskRunning = False

                For I As Integer = 0 To acquisitionCount - 1
                    sampleX(I) = volts(I)
                    acquisitionVolt(currentPoint, I + 1, currentDirection) = volts(I)

                    If CheckHysteresis.CheckState = CheckState.Unchecked Then acquisitionVolt(currentPoint, I + 1, 2) = volts(I) ' если нет гистерезиса

                    If mIsGroupTarir Then
                        acquisitionGroupVolt(currentIndexParameterGroup, currentPoint, I + 1, currentDirection) = volts(I)
                        If CheckHysteresis.CheckState = CheckState.Unchecked Then acquisitionGroupVolt(currentIndexParameterGroup, currentPoint, I + 1, 2) = volts(I) ' если нет гистерезиса
                    End If
                Next

                isSortOrder = False
                RewriteListViewMathMeasurement()
                MathematicalTreatmentSample()

                ' запись в первую таблицу
                If RadioButtonForward.Checked = True Then
                    TextBoxForwards(currentPoint - 1).Text = TextAverage.Text

                    If CheckHysteresis.CheckState = CheckState.Unchecked Then TextBacks(currentPoint - 1).Text = TextAverage.Text
                Else
                    TextBacks(currentPoint - 1).Text = TextAverage.Text
                End If
            Catch ex As DaqException
                Const caption As String = NameOf(AcquisitionOneParameter)
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                IsTaskRunning = False
                DAQmxTask.Dispose()
            Finally
                Cursor = Cursors.Default
                ButtonRunSample.Enabled = True
                ShowMessageToStatusBar("Произведён опрос канала: " & selectChannelName)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Создать задачу и произвести опрос канала.
    ''' </summary>
    ''' <returns></returns>
    Private Function AcquisitionDAQmxTask() As Double()
        Dim samplingRate As Double
        ' инициализация
        If Val(NIEditWait.Value) = 0 Then
            samplingRate = 1000 ' 10000 очень быстро
        Else
            samplingRate = 1 / (Val(NIEditWait.Value) / 1000)
        End If

        ' создать задачу
        If DAQmxTask IsNot Nothing Then
            DAQmxTask.Dispose()
            DAQmxTask = Nothing
            'Else
            '    myTask = New Task("aiTask")
        End If

        DAQmxTask = New Task("aiTask")
        ' создать виртуальный канал
        CreateAiChannel(ParametersType(indexChannel).NumberChannel.ToString,
                        ParametersType(indexChannel).NumberDevice.ToString,
                        ParametersType(indexChannel).NumberModuleChassis.ToString,
                        ParametersType(indexChannel).NumberChannelModule.ToString,
                        Convert.ToDouble(ParametersType(indexChannel).LowerMeasure),
                        Convert.ToDouble(ParametersType(indexChannel).UpperMeasure),
                        ParametersType(indexChannel).TypeConnection,
                        ParametersType(indexChannel).SignalType)

        DAQmxTask.Timing.ConfigureSampleClock("", samplingRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples)
        ' проверить корректность задачи
        DAQmxTask.Control(TaskAction.Verify)
        'reader.SynchronizingObject = Me
        Dim analogReader As AnalogSingleChannelReader = New AnalogSingleChannelReader(DAQmxTask.Stream) With {.SynchronizeCallbacks = True}
        Return analogReader.ReadMultiSample(acquisitionCount)
    End Function

    ''' <summary>
    ''' Переписать Лист
    ''' </summary>
    Private Sub RewriteListViewMathMeasurement()
        ListViewMathMeasurement.BeginUpdate()
        ListViewMathMeasurement.Items.Clear()

        ' Создать переменную, чтобы добавлять объекты ListItem.
        For I As Integer = 0 To acquisitionCount - 1
            Dim newListViewItem As New ListViewItem(CStr(I + 1))
            newListViewItem.SubItems.Add(Format(sampleX(I), "##0.0#####"))
            ListViewMathMeasurement.Items.Add(newListViewItem)
        Next

        ListViewMathMeasurement.EndUpdate()

        Select Case mTypeAcquisition
            Case TypeAcquisition.NotHysteresis
                mListMeasurement(currentPoint - 1).First = CType(sampleX.Clone, Double())
                mListMeasurement(currentPoint - 1).Back = CType(sampleX.Clone, Double())
                Exit Select
            Case TypeAcquisition.WithHysteresis
                If RadioButtonForward.Checked Then mListMeasurement(currentPoint - 1).First = CType(sampleX.Clone, Double())
                If RadioButtonBack.Checked Then mListMeasurement(currentPoint - 1).Back = CType(sampleX.Clone, Double())
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Замер Группы
    ''' </summary>
    Private Sub AcquisitionGroupParameter()
        For indexRow As Integer = 1 To PointTarirCount
            If TextEtalons(indexRow - 1).Text = vbNullString Then
                MessageBox.Show("Не введены поля эталона!", "Расчет", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not IsDigitCheck("Эталон", TextEtalons(indexRow - 1).Text) Then Exit Sub

            PhisicalEtalon(indexRow) = CDbl(TextEtalons(indexRow - 1).Text)
        Next

        For I As Integer = 1 To UBound(MetrologyGroup)
            selectedIndexParameterGroup = MetrologyGroup(I)
            currentIndexParameterGroup = I
            ComboBoxParameters.SelectedIndex = selectedIndexParameterGroup - 1
            ComboBoxParameters_SelectedIndexChanged(ComboBoxParameters, New EventArgs)
            AcquisitionOneParameter()
            Application.DoEvents()
        Next
    End Sub

    ''' <summary>
    ''' Переписать Лист При Навигации
    ''' </summary>
    Private Sub RewriteNavigationList()
        Dim tempVolts As Double() = Nothing

        If mListMeasurement.Count = PointTarirCount AndAlso mListMeasurement(currentPoint - 1).First IsNot Nothing Then
            ListViewMathMeasurement.BeginUpdate()
            ListViewMathMeasurement.Items.Clear()

            Select Case mTypeAcquisition
                Case TypeAcquisition.NotHysteresis
                    tempVolts = CType(mListMeasurement(currentPoint - 1).First.Clone, Double())
                    Exit Select
                Case TypeAcquisition.WithHysteresis
                    If RadioButtonForward.Checked Then
                        tempVolts = CType(mListMeasurement(currentPoint - 1).First.Clone, Double())
                    End If
                    If RadioButtonBack.Checked Then
                        If mListMeasurement(currentPoint - 1).Back Is Nothing Then
                            ListViewMathMeasurement.EndUpdate()
                            Exit Sub
                        End If
                        tempVolts = CType(mListMeasurement(currentPoint - 1).Back.Clone, Double())
                    End If
                    Exit Select
            End Select

            ' Создать переменную, чтобы добавлять объекты ListItem.
            For I As Integer = 0 To tempVolts.Length - 1
                Dim mListViewItem As New ListViewItem(CStr(I + 1))
                mListViewItem.SubItems.Add(Format(tempVolts(I), "##0.0#####"))
                ListViewMathMeasurement.Items.Add(mListViewItem)
            Next

            ListViewMathMeasurement.EndUpdate()

            sampleX = tempVolts
            isSortOrder = False
            ShowMessageToStatusBar($"Выбрана точка {currentPoint} для канала <{selectChannelName}>")
            MathematicalTreatmentSample()
        End If
    End Sub
#End Region

#Region "Обработчики элементов управления"
    ''' <summary>
    ''' Очищение полей
    ''' </summary>
    ''' <param name="isSelected"></param>
    ''' <remarks></remarks>
    Private Sub ShowSelectChannel(isSelected As Boolean)
        If isSelected Then
            TextChassislName.Text = selectChassislName
            TextChannelName.Text = selectChannelName
        Else
            TextChassislName.Text = "Шасси не выбрано"
            TextChannelName.Text = "Канал не выбран"
            selectChassislName = String.Empty
            selectChannelName = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Выбор канала из всех шасси в системе из модальной формы
    ''' для создания нового подписчика для приёма данных от канала AI
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ButtonSelectChannel_Click(sender As Object, e As EventArgs) Handles ButtonSelectChannel.Click
        Dim dlg As New SelectChannelDialogForm
        ' загружается модальная форма с деревом шасси и каналов AI.
        If dlg.ShowDialog() = DialogResult.OK Then
            selectChassislName = dlg.ChassisName
            selectChannelName = dlg.ChannelName
            ShowSelectChannel(True)
        End If
    End Sub

    Private Sub MenuFileExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFileExit.Click
        Close()
    End Sub

    Private Sub MenuNewWindowRegistration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowRegistration.Click
        MainMdiForm.MenuNewWindowRegistration_Click(sender, e)
        MenuNewWindowRegistration.Enabled = MainMdiForm.MenuNewWindowRegistration.Enabled
    End Sub

    Private Sub MenuWindowCascade_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowCascade.Click
        MainMdiForm.MenuWindowCascade_Click(sender, e)
    End Sub

    Private Sub MenuWindowTileHorizontal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowTileHorizontal.Click
        MainMdiForm.MenuWindowTileHorizontal_Click(sender, e)
    End Sub

    Private Sub MenuWindowTileVertical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWindowTileVertical.Click
        MainMdiForm.MenuWindowTileVertical_Click(sender, e)
    End Sub

    Private Sub MenuNewWindowSnapshot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowSnapshot.Click
        MainMdiForm.MenuNewWindowSnapshot_Click(sender, e)
        MenuNewWindowSnapshot.Enabled = MainMdiForm.MenuNewWindowSnapshot.Enabled
    End Sub

    Private Sub MenuNewWindowClient_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowClient.Click
        MainMdiForm.MenuNewWindowClient_Click(sender, e)
        MenuNewWindowClient.Enabled = MainMdiForm.MenuNewWindowClient.Enabled
    End Sub

    Private Sub MenuNewWindowDBaseChannels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuNewWindowDBaseChannels.Click
        MainMdiForm.MenuNewWindowDBaseChannels_Click(sender, e)
        MenuNewWindowDBaseChannels.Enabled = MainMdiForm.MenuNewWindowDBaseChannels.Enabled
    End Sub

    Private Sub ButtonCalculateCoefficient_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCalculateCoefficient.Click
        MathCoefficientPolynomial()
    End Sub

    Private Sub RadioButtonBack_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonBack.CheckedChanged
        If CType(sender, RadioButton).Checked Then currentDirection = 2
        If IsHandleCreated Then SelectActivePoint()

        ShowMessageToStatusBar("Обратный ход нагружения")
    End Sub

    Private Sub RadioButtonForward_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonForward.CheckedChanged
        If CType(sender, RadioButton).Checked Then currentDirection = 1
        If IsHandleCreated Then SelectActivePoint()

        ShowMessageToStatusBar("Прямой ход нагружения")
    End Sub

    ''' <summary>
    ''' Выделить Поле Активной Точки
    ''' </summary>
    Private Sub SelectActivePoint()
        For I As Integer = 0 To maxCountTarir
            If ButtonPoints(I).Checked Then ButtonPoints_CheckedChanged(ButtonPoints(I), New EventArgs)
        Next
    End Sub

    Private Sub ButtonRunSample_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRunSample.Click
        If mIsGroupTarir Then
            AcquisitionGroupParameter()
        Else
            AcquisitionOneParameter()
        End If
    End Sub

    Private Sub MenuDebugExamineParameter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuDebugExamineParameter.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuDebugExamineParameter_Click)}> загрузка формы: <{NameOf(FormTestChannel)}> опроса канала")
        InstallQuestionForm(WhoIsExamine.Examination)
        TestChannelForm.ParentFormBase = Me
        TestChannelForm.Show()
    End Sub

    Private Sub MenuStendView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuStendView.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuStendView_Click)}> загрузка формы: <{NameOf(FormChannel)}> состава каналов")
        mFormChannel = New FormChannel
        mFormChannel.Show()
        mFormChannel.Activate()
    End Sub

    Private Sub MenuHelpAbout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuHelpAbout.Click
        AboutForm = New FormAbout
        AboutForm.ShowDialog()
    End Sub

    Private Sub MenuHelpProgramm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuHelpProgramm.Click
        mFormBrowser = New FormWebBrowser
        mFormBrowser.Show()
        mFormBrowser.brwWebBrowser.Navigate(PathHelps)
        mFormBrowser.Activate()
    End Sub

    Private Async Sub FilePrint_ClickAsync(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFilePrint.Click, ButtonPrint.Click
        MenuFile.Enabled = False
        Await SavePrintProtocolCalibrationAsync(MenuFile, True)
        ShowMessageToStatusBar("Произведена запись и печать протокола для канала: " & selectChannelName)
    End Sub

    Private Async Sub FileSaveProtocol_ClickAsync(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFileSaveProtocol.Click, ButtonSaveProtocol.Click
        MenuFile.Enabled = False
        Await SavePrintProtocolCalibrationAsync(MenuFile, False)
        ShowMessageToStatusBar("Произведена запись протокола для канала: " & selectChannelName)
    End Sub

    Private Sub FileSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuFileSave.Click, ButtonSave.Click
        SavePolynomial()
        ShowMessageToStatusBar("Произведена запись коэффициентов тарировки для канала: " & selectChannelName)
    End Sub

    ''' <summary>
    ''' Записать Коэффициенты
    ''' </summary>
    Public Sub SavePolynomial()
        Dim strSQL As String = $"SELECT НомерПараметра,НаименованиеПараметра,СтепеньАппроксимации,A0,A1,A2,A3,A4,A5,Смещение,КомпенсацияХС,Погрешность,Дата FROM {ChannelLast} WHERE [НаименованиеПараметра]='{selectChannelName}'"
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter(strSQL, cn)

        Try
            odaDataAdapter.Fill(dtDataTable)

            If dtDataTable.Rows.Count > 0 Then
                drDataRow = dtDataTable.Rows(0)
                drDataRow.BeginEdit()

                drDataRow("СтепеньАппроксимации") = polynomialOrder
                ParametersType(indexChannel).LevelOfApproximation = CShort(polynomialOrder)
                drDataRow("A0") = Convert.ToDouble(TextPolynomial0.Text)
                ParametersType(indexChannel).Coefficient(0) = Convert.ToDouble(TextPolynomial0.Text)
                drDataRow("A1") = Convert.ToDouble(TextPolynomial1.Text)
                ParametersType(indexChannel).Coefficient(1) = Convert.ToDouble(TextPolynomial1.Text)
                drDataRow("A2") = Convert.ToDouble(TextPolynomial2.Text)
                ParametersType(indexChannel).Coefficient(2) = Convert.ToDouble(TextPolynomial2.Text)
                drDataRow("A3") = Convert.ToDouble(TextPolynomial3.Text)
                ParametersType(indexChannel).Coefficient(3) = Convert.ToDouble(TextPolynomial3.Text)
                drDataRow("A4") = Convert.ToDouble(TextPolynomial4.Text)
                ParametersType(indexChannel).Coefficient(4) = Convert.ToDouble(TextPolynomial4.Text)
                drDataRow("A5") = Convert.ToDouble(TextPolynomial5.Text)
                ParametersType(indexChannel).Coefficient(5) = Convert.ToDouble(TextPolynomial5.Text)
                CoefficientPolynomial2D(indexChannel, 0) = ParametersType(indexChannel).Coefficient(0)
                CoefficientPolynomial2D(indexChannel, 1) = ParametersType(indexChannel).Coefficient(1)
                CoefficientPolynomial2D(indexChannel, 2) = ParametersType(indexChannel).Coefficient(2)
                CoefficientPolynomial2D(indexChannel, 3) = ParametersType(indexChannel).Coefficient(3)
                CoefficientPolynomial2D(indexChannel, 4) = ParametersType(indexChannel).Coefficient(4)
                CoefficientPolynomial2D(indexChannel, 5) = ParametersType(indexChannel).Coefficient(5)

                drDataRow("Погрешность") = CSng(погрешностьПроцент)

                If CBool(drDataRow("КомпенсацияХС")) <> True Then
                    drDataRow("Смещение") = 0
                    ParametersType(indexChannel).Offset = 0
                End If

                drDataRow("Дата") = Date.Now
                drDataRow.EndEdit()
            End If

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
        Catch ex As Exception
            Dim caption As String = $"Ошибка обновления коэффициентов тарировки в процедуре: {NameOf(SavePolynomial)}."
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_DB_UPDATE_FAILED($"<{caption}> {text}")
        Finally
            If (cn.State = ConnectionState.Open) Then cn.Close()
        End Try
    End Sub

    Private Sub ButtonCheck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCheck.Click
        Try
            ' проверка качества вычисленных коэффициентов
            Dim inputData As Double() = {CDbl(Val(TextValueVolt.Text)), CDbl(Val(TextValueVolt.Text))}
            Dim mathData As Double() = ArrayOperation.PolynomialEvaluation1D(inputData, CoefficientsPolynomial)

            ' вычислить для проверки ' вычислить для построения графика
            TextResult.Text = CStr(mathData(0))
            ShowMessageToStatusBar($"Произведена проверка качества вычисления коэффициентов тарировки для канала: <{selectChannelName}> со степенью полинома {polynomialOrder}")
        Catch ex As Exception
            Const caption As String = NameOf(ButtonCheck_Click)
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub ListViewMathMeasurement_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) Handles ListViewMathMeasurement.ColumnClick
        isSortOrder = Not isSortOrder

        If isSortOrder Then
            Array.Sort(sampleX)
        Else
            Array.Reverse(sampleX)
        End If

        RewriteListViewMathMeasurement()
        GraphMathematician.PlotY(sampleX)
        ShowMessageToStatusBar($"Сортировка замеров в {If(isSortOrder, "прямом", "обратном")} направлении")
    End Sub

    Private Sub MenuCreatePronocol_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuCreatePronocol.Click
        Dim averageForward As Double
        Dim averageBack As Double
        Dim rnd As New Random(DateTime.Now.Millisecond)
        Dim sign As Double = 1.0#
        Dim percentage As Double ' процент 

        CheckHand.Checked = False
        CheckHysteresis.Checked = True
        NEditCountPoints.Value = 10
        NEditCountSamples.Value = 10

        Dim mfrmНастройкаПротоколаМетрологии As New FormSettingProtocolMetrological
        mfrmНастройкаПротоколаМетрологии.ShowDialog()

        physicalMin = mfrmНастройкаПротоколаМетрологии.PhysicalMin
        physicalMax = mfrmНастройкаПротоколаМетрологии.PhysicalMax
        ADmin = mfrmНастройкаПротоколаМетрологии.ADCmin
        ADmax = mfrmНастройкаПротоколаМетрологии.ADCmax
        whiteNoisePsysical = Math.Round((physicalMax - physicalMin) / PointTarirCount, 2)
        whiteNoiseAD = (ADmax - ADmin) / PointTarirCount

        For intIndex As Integer = 1 To PointTarirCount
            averageForward = 0
            averageBack = 0
            percentage = (10000 - 1000 + 1) * rnd.NextDouble + 1000

            For I As Integer = 0 To acquisitionCount - 1
                acquisitionVolt(intIndex, I + 1, 1) = ADmin - sign * ADmin * rnd.NextDouble / percentage
                acquisitionVolt(intIndex, I + 1, 2) = ADmin + sign * ADmin * rnd.NextDouble / percentage
                averageForward += acquisitionVolt(intIndex, I + 1, 1)
                averageBack += acquisitionVolt(intIndex, I + 1, 2)
            Next

            TextBoxForwards(intIndex - 1).Text = Format(averageForward / acquisitionCount, "##0.0#####")
            TextBacks(intIndex - 1).Text = Format(averageBack / acquisitionCount, "##0.0#####")
            TextEtalons(intIndex - 1).Text = physicalMin.ToString

            ADmin += whiteNoiseAD
            physicalMin += whiteNoisePsysical
            sign *= (-1)
        Next intIndex

        MathCoefficientPolynomial()
        isImitatorMetrology = True
    End Sub

    Private Sub ButtonGrup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonGrup.Click
        If IsGroupTarir = False Then
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonGrup_Click)}> групповая тарировка")
            frmMetrologyGroup = New FormMetrologyGroup() With {.FormParent = Me}
            frmMetrologyGroup.Show()
            IsGroupTarir = True
        End If
    End Sub

    Private Sub NEditCountSamples_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NEditCountSamples.AfterChangeValue
        acquisitionCount = CInt(e.NewValue)
        InitializeVariables()
        ShowMessageToStatusBar("Установлено число опросов одной точки: " & acquisitionCount)
    End Sub

    Private Sub NIEditWait_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NIEditWait.AfterChangeValue
        'If e.NewValue < 0 Then e.NewValue = 0
        waitSleep = CInt(e.NewValue)
        ShowMessageToStatusBar($"Установлена задержка между опросами: {waitSleep} млсек.")
    End Sub

    Private Sub NEditCountPoints_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NEditCountPoints.AfterChangeValue
        If IsHandleCreated Then
            LoadTableMetrology(CInt(e.NewValue))
            PointTarirCount = CInt(e.NewValue)
            InitializeVariables()
            SelectTypeAcquisition()
            PopulateListMeasurement()
            ShowMessageToStatusBar("Установлено число ступеней нагружения в колличестве: " & PointTarirCount)
        End If
    End Sub

    Private Sub ListViewMathMeasurement_Resize(sender As Object, e As EventArgs) Handles ListViewMathMeasurement.Resize
        If Me.IsHandleCreated AndAlso isFormLoaded Then
            ListViewMathMeasurement.Columns("№ опроса").Width = ListViewMathMeasurement.Width \ 3
            ListViewMathMeasurement.Columns("Значение").Width = (ListViewMathMeasurement.Width * 2 \ 3) - 16
        End If
    End Sub
#End Region

#Region "Обработчики табличных элементов"

    Private Sub TextEtalons_Leave(ByVal sender As Object, ByVal e As EventArgs)
        Dim I As Integer = Convert.ToInt32(CType(sender, TextBox).Tag)
        Try
            TextEtalons(I).BackColor = Color.White
            TextEtalons(I).ForeColor = Color.Black
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TextEtalons_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim I As Integer = Convert.ToInt32(CType(sender, TextBox).Tag)
        TextEtalons(I).BackColor = Color.Red
        TextEtalons(I).ForeColor = Color.White
    End Sub

    Private Sub TextBoxForwards_Leave(ByVal sender As Object, ByVal e As EventArgs)
        Dim I As Integer = Convert.ToInt32(CType(sender, TextBox).Tag)
        Try
            If mTypeAcquisition = TypeAcquisition.Table Then
                TextBoxForwards(I).BackColor = Color.White
                TextBoxForwards(I).ForeColor = Color.Black
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TextBoxForwards_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim I As Integer = Convert.ToInt32(CType(sender, TextBox).Tag)
        If mTypeAcquisition = TypeAcquisition.Table Then
            TextBoxForwards(I).BackColor = Color.DarkBlue
            TextBoxForwards(I).ForeColor = Color.White
        End If
    End Sub

    Private Sub ButtonPoints_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim I As Integer = Convert.ToInt32(CType(sender, RadioButton).Tag)
        currentPoint = I + 1

        Select Case mTypeAcquisition
            Case TypeAcquisition.NotHysteresis
                If ButtonPoints(I).Checked Then
                    ButtonPoints(I).BackColor = Color.Orange
                    TextBoxForwards(I).BackColor = Color.DarkBlue
                    TextBoxForwards(I).ForeColor = Color.White
                    TextBacks(I).BackColor = Color.DarkBlue
                    TextBacks(I).ForeColor = Color.White
                    RewriteNavigationList()

                    Exit Select
                Else
                    ButtonPoints(I).BackColor = Color.LightGray
                    TextBoxForwards(I).BackColor = Color.White
                    TextBoxForwards(I).ForeColor = Color.Black
                    TextBacks(I).BackColor = Color.White
                    TextBacks(I).ForeColor = Color.Black
                End If
                Exit Select
            Case TypeAcquisition.WithHysteresis
                If ButtonPoints(I).Checked Then
                    ButtonPoints(I).BackColor = Color.Orange
                    If RadioButtonForward.Checked Then
                        TextBoxForwards(I).BackColor = Color.DarkBlue
                        TextBoxForwards(I).ForeColor = Color.White
                        TextBacks(I).BackColor = Color.White
                        TextBacks(I).ForeColor = Color.Black

                        RewriteNavigationList()
                    End If

                    If RadioButtonBack.Checked Then
                        TextBoxForwards(I).BackColor = Color.White
                        TextBoxForwards(I).ForeColor = Color.Black
                        TextBacks(I).BackColor = Color.DarkBlue
                        TextBacks(I).ForeColor = Color.White

                        RewriteNavigationList()
                    End If
                Else
                    ButtonPoints(I).BackColor = Color.LightGray
                    TextBoxForwards(I).BackColor = Color.White
                    TextBoxForwards(I).ForeColor = Color.Black
                    TextBacks(I).BackColor = Color.White
                    TextBacks(I).ForeColor = Color.Black
                End If
                Exit Select
            Case TypeAcquisition.Table
                If ButtonPoints(I).Checked Then
                    ButtonPoints(I).BackColor = Color.Orange
                    TextBoxForwards(I).BackColor = Color.DarkBlue
                    TextBoxForwards(I).ForeColor = Color.White
                    TextBacks(I).Visible = False
                Else
                    ButtonPoints(I).BackColor = Color.LightGray
                    TextBoxForwards(I).BackColor = Color.White
                    TextBoxForwards(I).ForeColor = Color.Black
                    TextBacks(I).Visible = False
                End If
                Exit Select
        End Select
    End Sub

    Private Sub TextEtalons_KeyDown(ByVal sender As Object, ByVal eventArgs As KeyEventArgs)
        Dim I As Integer = Convert.ToInt16(CType(sender, TextBox).Tag)
        Dim keyCode As Short = CShort(eventArgs.KeyCode)
        Dim shift As Short = CShort(eventArgs.KeyData \ &H10000)
        EditKeyCodeTextEtalon(keyCode, shift, I)
    End Sub

    Private Sub TextEtalons_KeyPress(ByVal sender As Object, ByVal eventArgs As KeyPressEventArgs)
        Dim keyAscii As Short = CShort(Asc(eventArgs.KeyChar))
        ' Ничего не возвращать, чтобы избавиться от гудка.
        If keyAscii = Asc(vbCr) Then keyAscii = 0
        If keyAscii = 0 Then eventArgs.Handled = True
    End Sub

    Private Sub EditKeyCodeTextEtalon(ByVal keyCode As Short, ByVal shift As Short, ByVal index As Integer)
        ' Стандартное редактирование.
        Select Case keyCode
            Case 27 ' ESC: спрятать, вернуть фокус 
                TextEtalons_Leave(TextEtalons(index), New EventArgs)
                Exit Select
            Case 13 ' ENTER вернуть фокус 
                Application.DoEvents()
                If index < PointTarirCount - 1 Then
                    TextEtalons(index + 1).Focus()
                End If
                Exit Select
            Case 38 ' Клавиша стрелка вверх.
                Application.DoEvents()
                If index > 0 Then
                    TextEtalons(index - 1).Focus()
                End If
                Exit Select
            Case 40 ' Клавиша стрелка Вниз.
                Application.DoEvents()
                If index < PointTarirCount - 1 Then
                    TextEtalons(index + 1).Focus()
                End If
                Exit Select
        End Select
    End Sub

    Private Sub TextBoxForwards_KeyDown(ByVal sender As Object, ByVal eventArgs As KeyEventArgs)
        Dim I As Short = Convert.ToInt16(CType(sender, TextBox).Tag)
        Dim keyCode As Short = CShort(eventArgs.KeyCode)
        Dim shift As Short = CShort(eventArgs.KeyData \ &H10000)
        EditKeyCodeTextBoxForward(keyCode, shift, I)
    End Sub

    Private Sub TextBoxForwards_KeyPress(ByVal sender As Object, ByVal eventArgs As KeyPressEventArgs)
        Dim keyAscii As Short = CShort(Asc(eventArgs.KeyChar))
        ' Ничего не возвращать, чтобы избавиться от гудка.
        If keyAscii = Asc(vbCr) Then keyAscii = 0
        If keyAscii = 0 Then eventArgs.Handled = True
    End Sub

    Private Sub EditKeyCodeTextBoxForward(ByRef KeyCode As Short, ByRef Shift As Short, ByRef I As Short)
        ' Стандартное редактирование.
        Select Case KeyCode
            Case 27 ' ESC: спрятать, вернуть
                TextBoxForwards_Leave(TextBoxForwards(I), New EventArgs)
                Exit Select
            Case 13 ' ENTER вернуть фокус 
                Application.DoEvents()
                If I < PointTarirCount - 1 Then
                    TextBoxForwards(I + 1).Focus()
                End If
                Exit Select
            Case 38 ' Клавиша стрелка вверх.
                Application.DoEvents()
                If I > 0 Then
                    TextBoxForwards(I - 1).Focus()
                End If
                Exit Select
            Case 40 'Клавиша стрелка Вниз.
                Application.DoEvents()
                If I < PointTarirCount - 1 Then
                    TextBoxForwards(I + 1).Focus()
                End If
                Exit Select
        End Select
    End Sub

#End Region

#Region "СТАТИСТИКА"
    '--- СТАТИСТИКА -----------------------------------------------------------
    ''' <summary>
    ''' Математическая обработка замера одной ступени нагружения
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MathematicalTreatmentSample()
        GraphMathematician.PlotY(sampleX)
        ' среднее
        Dim average As Double = Statistics.Mean(sampleX)
        ' средне Квадратическое
        Dim quadraticMean As Double = Statistics.StandardDeviation(sampleX)

        Dim indexOfMinimum, indexOfMaximum As Integer
        Dim minimum, maximum As Double
        ArrayOperation.MaxMin1D(sampleX, maximum, indexOfMaximum, minimum, indexOfMinimum)
        TextAverage.Text = Format(average, "##0.0#####")
        TextMeanSquare.Text = Format(quadraticMean, "##0.0#####")
        TextMax.Text = Format(maximum, "##0.0#####")
        TextMin.Text = Format(minimum, "##0.0#####")

        If average <> 0 Then TextReducedError.Text = Format((2.0# * quadraticMean * 100) / average, "##0.0###")

        ' ГИСТОГРАММА
        Dim xArray() As Double = Nothing
        Dim yArray() As Integer = Nothing
        Dim max, min As Double
        Dim iMax, iMin As Integer
        Const nIntervals As Integer = 19

        ArrayOperation.MaxMin1D(sampleX, max, iMax, min, iMin)

        If min <> max Then yArray = Statistics.Histogram(sampleX, min, max, nIntervals, xArray) ' чтобы не было ошибки когда все равны

        ScatterGraphHistogram.PlotXY(xArray, Array.ConvertAll(yArray, New Converter(Of Integer, Double)(AddressOf PointFToPoint)))
    End Sub

    ''' <summary>
    ''' Конвертер из одного типа в другой
    ''' </summary>
    ''' <param name="pf"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PointFToPoint(ByVal pf As Integer) As Double
        'Return New Point(CInt(pf.X), CInt(pf.Y))
        Return CDbl(pf)
    End Function

    ''' <summary>
    ''' Вычислить Коэффициенты Полиномов
    ''' </summary>
    Private Sub MathCoefficientPolynomial()
        Dim K, I, J As Integer

        polynomialOrder = CInt(NumericEditPolynomialOrder.Value)

        If polynomialOrder >= PointTarirCount Then
            MessageBox.Show("Степень полинома должна быть меньше количества точек градуировки!", "Расчет", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' копирование с 1 таблицы эталонные значения
        For indexRow As Integer = 1 To PointTarirCount
            If TextEtalons(indexRow - 1).Text = vbNullString Then
                MessageBox.Show("Не введены поля эталона!", "Расчет", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If Not IsDigitCheck("Эталон", TextEtalons(indexRow - 1).Text) Then Exit Sub

            PhisicalEtalon(indexRow) = CDbl(TextEtalons(indexRow - 1).Text)
        Next

        If CheckHand.CheckState = CheckState.Unchecked Then
            Dim summ As Double ' сумма Замеров
            ' расчет центров группировки
            For I = 1 To PointTarirCount

                For J = 1 To acquisitionCount
                    For K = 1 To 2
                        summ += acquisitionVolt(I, J, K)
                    Next K
                Next J

                AverageInput(I) = summ / (2 * acquisitionCount)
                summ = 0
            Next I
        Else ' считывание с таблицы
            For indexRow As Integer = 1 To PointTarirCount
                If TextBoxForwards(indexRow - 1).Text = vbNullString Then
                    MessageBox.Show("Не введены поля величины!", "Расчет", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                If Not IsDigitCheck("Прямой", TextBoxForwards(indexRow - 1).Text) Then Exit Sub

                AverageInput(indexRow) = CDbl(TextBoxForwards(indexRow - 1).Text)
            Next
        End If

        ' подготовка для расчета полинома
        Re.Dim(XPhisical, PointTarirCount - 1)
        Re.Dim(YOutput, PointTarirCount - 1)


        For I = 0 To PointTarirCount - 1
            XPhisical(I) = PhisicalEtalon(I + 1)
            YOutput(I) = AverageInput(I + 1)
        Next

        Dim mathPoints As Double()
        Dim CoEff(polynomialOrder) As Double ' коэффициенты аппроксимации

        ' вычислить коэффициенты
        Dim mse As Double
        mathPoints = CurveFit.PolynomialFit(XPhisical, YOutput, polynomialOrder, PolynomialFitAlgorithm.Svd, CoEff, mse) ' для графика
        mathPoints = CurveFit.PolynomialFit(YOutput, XPhisical, polynomialOrder, PolynomialFitAlgorithm.Svd, CoefficientsPolynomial, mse) ' для расчета
        ' могу использовать результат arrCoefficientsPolynomial() в программе

        ' очистка и заполнение текстовых полей
        For I = 0 To UBound(polinomialTextBox)
            polinomialTextBox(I).Text = CStr(0)
        Next

        For I = 0 To UBound(CoEff)
            polinomialTextBox(I).Text = CStr(CoefficientsPolynomial(I))
        Next

        PlotGraphs()

        If CheckHand.CheckState = CheckState.Unchecked Then ProcessingTableTarir()

        ShowMessageToStatusBar($"Произведено вычисление коэффициентов тарировки для канала <{selectChannelName}> со степенью полинома {polynomialOrder}")
        MenuFileSaveProtocol.Enabled = True
        MenuFilePrint.Enabled = True
        ButtonSaveProtocol.Enabled = True
        ButtonPrint.Enabled = True
    End Sub

    ''' <summary>
    ''' Обработка
    ''' </summary>
    Private Sub ProcessingTableTarir()
        Dim K, I, J As Integer
        Dim summ As Double
        Dim временныйЗамерАЦП(acquisitionCount - 1) As Double
        Dim average As Double ' cреднее
        Dim коэфQ As Double
        Dim dataColumns(8) As String

        quadraticMeanAllRange = 0
        maxСистематическая = 0
        maxВариация = 0
        погрешность = 0
        погрешностьПроцент = 0

        ' обратный расчет выходной физической величины
        For K = 1 To 2
            For I = 1 To PointTarirCount
                ' переписать в массив который засылается
                For J = 1 To acquisitionCount
                    временныйЗамерАЦП(J - 1) = acquisitionVolt(I, J, K)
                Next J

                ' считать в массив вычисленные значения
                Dim временныйПриемПолином As Double() = ArrayOperation.PolynomialEvaluation1D(временныйЗамерАЦП, CoefficientsPolynomial)
                average = Statistics.Mean(временныйПриемПолином)
                среднееФизикаПолином(I, K) = average

                ' считать в рабочий массив вычисленные значения
                For J = 1 To acquisitionCount
                    polynomialPhysical(I, J, K) = временныйПриемПолином(J - 1)
                Next J
            Next I
        Next K

        ' определение оценки СКО
        коэфQ = SolveKofQ(2 * acquisitionCount - 2)

        For I = 1 To PointTarirCount
            For J = 1 To acquisitionCount
                summ += (polynomialPhysical(I, J, 1) - среднееФизикаПолином(I, 1)) * (polynomialPhysical(I, J, 1) - среднееФизикаПолином(I, 1)) + (polynomialPhysical(I, J, 2) - среднееФизикаПолином(I, 2)) * (polynomialPhysical(I, J, 2) - среднееФизикаПолином(I, 2))
            Next J

            оценкаСКО(I) = коэфQ * Math.Sqrt(summ / (2 * acquisitionCount - 2))
            summ = 0
        Next I

        ' определяется систематическая составляющая погрешности
        For I = 1 To PointTarirCount
            систематическаяПогрешности(I) = (Math.Abs(PhisicalEtalon(I) - среднееФизикаПолином(I, 1)) + Math.Abs(PhisicalEtalon(I) - среднееФизикаПолином(I, 2))) / 2
            If maxСистематическая < систематическаяПогрешности(I) Then maxСистематическая = систематическаяПогрешности(I)
        Next

        ' определяется вариация
        For I = 1 To PointTarirCount
            вариация(I) = Math.Abs(среднееФизикаПолином(I, 1) - среднееФизикаПолином(I, 2))
            If maxВариация < вариация(I) Then maxВариация = вариация(I)
        Next

        ' определяется доверительный интервал в котором с заданной вероятностью Р находится погрешность
        For I = 1 To PointTarirCount
            доверительныйИнтервал(I, 1) = систематическаяПогрешности(I) - T_0_95 * Math.Sqrt(вариация(I) * вариация(I) / 12 + оценкаСКО(I) * оценкаСКО(I))
            доверительныйИнтервал(I, 2) = систематическаяПогрешности(I) + T_0_95 * Math.Sqrt(вариация(I) * вариация(I) / 12 + оценкаСКО(I) * оценкаСКО(I))
        Next

        ' определяется СКО во всем диапазоне
        summ = 0
        For I = 1 To PointTarirCount
            summ += оценкаСКО(I) * оценкаСКО(I)
        Next

        quadraticMeanAllRange = Math.Sqrt(summ / PointTarirCount)
        ' определяется интервал погрешности во всем диапазоне
        интервалПогрешностиКанала(1) = maxСистематическая - T_0_95 * Math.Sqrt(maxВариация * maxВариация / 12 + quadraticMeanAllRange * quadraticMeanAllRange)
        интервалПогрешностиКанала(2) = maxСистематическая + T_0_95 * Math.Sqrt(maxВариация * maxВариация / 12 + quadraticMeanAllRange * quadraticMeanAllRange)

        погрешность = интервалПогрешностиКанала(2)
        погрешностьПроцент = погрешность * 100 / Math.Abs(PhisicalEtalon(PointTarirCount) - PhisicalEtalon(1))

        ListMetrology.Items.Clear()

        For I = 1 To PointTarirCount
            dataColumns(0) = I.ToString
            dataColumns(1) = Format(AverageInput(I), "##0.0###")
            dataColumns(2) = Format(PhisicalEtalon(I), "###0.0#")
            dataColumns(3) = Format(среднееФизикаПолином(I, 1), "###0.0#")
            dataColumns(4) = Format(среднееФизикаПолином(I, 2), "###0.0#")
            dataColumns(5) = Format(вариация(I), "#0.0####")
            dataColumns(6) = Format(систематическаяПогрешности(I), "#0.0####")
            dataColumns(7) = Format(оценкаСКО(I), "#0.0####")
            dataColumns(8) = Format(доверительныйИнтервал(I, 2), "#0.0####")
            ListMetrology.Items.Add(New ListViewItem(dataColumns))
        Next

        TextMaxReducedError.Text = Format(погрешность, "#0.0####")
        TextPercent.Text = Format(погрешностьПроцент, "##0.0##")
    End Sub

    Private Sub PlotGraphs()
        Dim I As Integer
        ' вычисления для графика
        ScatterGraphADPhysical.ClearData()

        '--- переменные графика ---------------------------------------
        Dim pointsPolynomialCurveX((PointTarirCount - 1) * PointsCount) As Double ' точки Полином График для построения графика полиноминальной кривой
        Dim physicistAverageForGraph(PointTarirCount - 1) As Double ' физика Среднее Для Графика
        Dim xData(PointTarirCount - 1) As Double ' точки Графика X
        Dim yData(PointTarirCount - 1) As Double ' точки Графика Y 

        For I = 0 To PointTarirCount - 1
            physicistAverageForGraph(I) = AverageInput(I + 1)
            xData(I) = YOutput(I)
            yData(I) = XPhisical(I)
        Next

        Dim maximumPhysicist, minimumPhysicist As Double
        Dim indexOfMinimum, indexOfMaximum As Integer
        ArrayOperation.MaxMin1D(physicistAverageForGraph, maximumPhysicist, indexOfMaximum, minimumPhysicist, indexOfMinimum)
        ' для графика
        Dim range As Double = Math.Abs(maximumPhysicist - minimumPhysicist)
        Dim hundredRange As Integer = (PointTarirCount - 1) * PointsCount 'Граница

        For I = 0 To hundredRange
            pointsPolynomialCurveX(I) = I * range / hundredRange + AverageInput(1)
        Next

        ' показать точки на графике
        ' вычислить для построения графика
        ScatterPlotPolinomusDot1.PlotXY(pointsPolynomialCurveX, ArrayOperation.PolynomialEvaluation1D(pointsPolynomialCurveX, CoefficientsPolynomial))
        ScatterPlotMeasurement1.PlotXY(xData, yData)
        UpdateGraph2()
    End Sub

    Private Sub UpdateGraph2()
        Dim PolynomialCurve((PointTarirCount - 1) * PointsCount) As Double
        Dim pointsPolynomialGraph As Double() ' точкиПолиномГрафик
        Dim xData(PointTarirCount - 1), yData(PointTarirCount - 1) As Double
        Dim CoEff(polynomialOrder) As Double ' коэффициенты аппроксимации

        ScatterGraphPhysicalAD.ClearData()

        Dim range As Double = Math.Abs(PhisicalEtalon(PointTarirCount) - PhisicalEtalon(1))
        Dim hundredRange As Integer = (PointTarirCount - 1) * PointsCount

        For I = 0 To hundredRange
            PolynomialCurve(I) = I * range / hundredRange + PhisicalEtalon(1)
        Next

        pointsPolynomialGraph = ArrayOperation.PolynomialEvaluation1D(PolynomialCurve, CoEff) 'вычислить для построения графика
        ' показать точки на графике
        'ScatterPlotDot.PlotXY(arrТочкиПолиномГрафик, dblПолинКривая) ' arrФизикаэталон(1), (dblДиапазон / (lngЧислоТочекТарировки - 1)) / intКолТочекГрафика)
        ScatterPlotPolinomusDot2.PlotXY(PolynomialCurve, pointsPolynomialGraph)

        For I As Integer = 0 To PointTarirCount - 1
            xData(I) = XPhisical(I)
            yData(I) = YOutput(I)
        Next

        ScatterPlotMeasurement2.PlotXY(xData, yData)
    End Sub

    Private Sub CheckHand_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckHand.CheckStateChanged
        If CheckHand.CheckState = CheckState.Checked Then
            RadioButtonBack.Visible = False
            SetControlVisible(False)
        Else
            SetControlVisible(True)
            If CheckHysteresis.CheckState = CheckState.Checked Then RadioButtonBack.Visible = True
        End If

        SelectTypeAcquisition()
        ShowMessageToStatusBar("Тарировка производится посредством " &
                          If(CheckHand.CheckState = CheckState.Checked, "заполнения таблицы для косвенной тарировки", "последовательной загрузки датчика и опроса канала"))

    End Sub

    Private Sub SetControlVisible(inVisible As Boolean)
        NEditCountSamples.Visible = inVisible
        NIEditWait.Visible = inVisible
        RadioButtonForward.Visible = inVisible
        ButtonRunSample.Visible = inVisible
        CheckHysteresis.Visible = inVisible
        LabelCoutMeasurement.Visible = inVisible
        LabelPause.Visible = inVisible
    End Sub

    Private Sub CheckHysteresis_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckHysteresis.CheckStateChanged
        If CheckHysteresis.CheckState = CheckState.Checked Then
            RadioButtonBack.Visible = True
        Else
            RadioButtonForward.Checked = True
            RadioButtonBack.Visible = False
        End If

        SelectTypeAcquisition()
        ShowMessageToStatusBar("Тарировка производится последовательной загрузкой датчика " &
                  If(CheckHysteresis.CheckState = CheckState.Checked, "в прямом и обратном направлении", "только в прямом направлении"))
    End Sub

    ''' <summary>
    ''' Определить Режим Опроса
    ''' </summary>
    Private Sub SelectTypeAcquisition()
        mTypeAcquisition = TypeAcquisition.NotHysteresis ' по умолчанию

        If CheckHand.Checked Then
            mTypeAcquisition = TypeAcquisition.Table
        Else
            If CheckHysteresis.Checked Then
                mTypeAcquisition = TypeAcquisition.WithHysteresis
            Else
                mTypeAcquisition = TypeAcquisition.NotHysteresis
            End If
        End If

        Select Case mTypeAcquisition
            Case TypeAcquisition.NotHysteresis, TypeAcquisition.WithHysteresis
                'устанавливает видимость и доступность полей
                For I As Integer = 0 To maxCountTarir
                    ButtonPoints(I).Checked = False
                    TextEtalons(I).Text = ""
                    TextBoxForwards(I).Text = ""
                    TextBacks(I).Text = ""
                    TextBoxForwards(I).BackColor = Color.White
                    TextBoxForwards(I).ForeColor = Color.Black
                    TextBoxForwards(I).ReadOnly = True
                    TextBacks(I).BackColor = Color.White
                    TextBacks(I).ForeColor = Color.Black
                    TextBacks(I).Visible = True
                    ButtonPoints(I).Visible = I <= PointTarirCount - 1
                    TextEtalons(I).Visible = I <= PointTarirCount - 1
                    TextBoxForwards(I).Visible = I <= PointTarirCount - 1
                    TextBacks(I).Visible = I <= PointTarirCount - 1
                Next
                Exit Select
            Case TypeAcquisition.Table
                For I As Integer = 0 To maxCountTarir
                    ButtonPoints(I).Checked = False
                    ButtonPoints(I).Visible = I <= PointTarirCount - 1
                    TextEtalons(I).Text = ""
                    TextEtalons(I).Visible = I <= PointTarirCount - 1
                    TextBoxForwards(I).Text = ""
                    TextBoxForwards(I).BackColor = Color.White
                    TextBoxForwards(I).ForeColor = Color.Black
                    TextBoxForwards(I).ReadOnly = False
                    TextBoxForwards(I).Visible = I <= PointTarirCount - 1
                    TextBacks(I).Text = ""
                    TextBacks(I).BackColor = Color.White
                    TextBacks(I).ForeColor = Color.Black
                    TextBacks(I).Visible = False
                Next
                Exit Select
        End Select
    End Sub

    Private Sub ComboBoxParameters_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxParameters.SelectedIndexChanged
        ' смена параметра в комбинированном окне
        ' определение по массиву
        indexChannel = ComboBoxParameters.SelectedIndex + 1
        ' зто действенно при четком соответствии позиции в комбинированном списке с номером канала-1
        ' массив arrПараметры должен быть в возрастающем порядке, что и есть
        If CheckMeasurementChannels(indexChannel) Then
            ButtonRunSample.Enabled = True
        Else
            ButtonRunSample.Enabled = False
        End If

        If mIsGroupTarir = False Then SelectTypeAcquisition()

        selectChannelName = ComboBoxParameters.Text
        TextChannelName.Text = selectChannelName
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxParameters)
        mSearchChannel.SelectChannel()
    End Sub
#End Region

#Region "ПечатьПротокола"
    ''' <summary>
    ''' Запись, печать Протокола Тарировки.
    ''' Подготовка и обертка.
    ''' </summary>
    ''' <param name="inMenuItem"></param>
    ''' <param name="isPrint"></param>
    Public Async Function SavePrintProtocolCalibrationAsync(inMenuItem As ToolStripMenuItem, isPrint As Boolean) As Tasks.Task 'As Tasks.Task(Of Task)
        Dim mFormMessageBox As FormMessageBox

        If isPrint = False Then
            mFormMessageBox = New FormMessageBox("Подождите, идет запись", "Запись")
        Else
            mFormMessageBox = New FormMessageBox
        End If

        mFormMessageBox.Show()
        mFormMessageBox.TopMost = True
        mFormMessageBox.Activate()
        mFormMessageBox.Refresh()
        Refresh()

        Await SavePrintProtocolCalibrationTaskAsync(isPrint)
        mFormMessageBox.Close()
        inMenuItem.Enabled = True
    End Function

    ''' <summary>
    ''' Запись, печать Протокола Тарировки.
    ''' Запуск задачи асинхронно.
    ''' </summary>
    ''' <param name="isPrint"></param>
    ''' <returns></returns>
    Private Function SavePrintProtocolCalibrationTaskAsync(isPrint As Boolean) As Tasks.Task
        Return Tasks.Task.Run(Sub()
                                  SavePrintProtocolCalibrationTask(isPrint)
                              End Sub)
    End Function

    ''' <summary>
    ''' Запись, печать Протокола Тарировки.
    ''' Задача в собственном потоке.
    ''' </summary>
    ''' <param name="isPrint"></param>
    Private Sub SavePrintProtocolCalibrationTask(isPrint As Boolean)
        Dim strSQL As String = $"SELECT * FROM {ChannelLast} WHERE [НаименованиеПараметра]= '{selectChannelName}'"
        Dim cmd As OleDbCommand

        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cmd = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            ' или можно так: cmd = New OleDbCommand(strSQL, cn)
            cn.Open()

            Using drDataReader As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If drDataReader.Read() = True Then
                    numberParameter = Convert.ToString(drDataReader("НомерПараметра"))
                    nameParameter = Convert.ToString(drDataReader("НаименованиеПараметра"))
                    numberChannel = Convert.ToString(drDataReader("НомерКанала"))

                    minVal = Math.Round(CDbl(drDataReader("НижнийПредел")), 3).ToString
                    maxVal = Math.Round(CDbl(drDataReader("ВерхнийПредел")), 3).ToString
                    unitChannel = Convert.ToString(drDataReader("ЕдиницаИзмерения"))
                End If
            End Using
        End Using

        ' установить ссылку открыть книгу
        Dim reportExcel As New Excel.Application
        'ExcelПротокол = CreateObject("Excel.Application") ' в старом стиле
        reportExcel.Workbooks.Open(Filename:=PathReportTaring)
        reportExcel.Visible = False

        Try
            PrintReport(reportExcel, isPrint)
        Catch ex As Exception
            Const caption As String = NameOf(SavePrintProtocolCalibrationTask)
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            reportExcel.ActiveWorkbook.Saved = True
            reportExcel.ActiveWindow.Close(SaveChanges:=False)
            reportExcel.Quit()
            reportExcel = Nothing
            GC.Collect()
        End Try
    End Sub

    ''' <summary>
    ''' Печать Протокола
    ''' </summary>
    ''' <param name="reportExcel"></param>
    ''' <param name="isPrint"></param>
    Private Sub PrintReport(ByVal reportExcel As Excel.Application, isPrint As Boolean)
        RegistrationEventLog.EventLog_MSG_USER_ACTION("Печать Протокола Тарировки")

        Dim I As Integer
        Dim averageX() As Double

        With reportExcel
            '--- очистка ----------------------------------------------------
            .Rows._Default("23:60").Select()
            .Selection.Clear()
            .Selection.RowHeight = 12
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlCenter
                .VerticalAlignment = Excel.Constants.xlTop
                .WrapText = True
                .Orientation = 0
                .AddIndent = False
                .ShrinkToFit = False
                .MergeCells = False
            End With
            With .Selection.Font
                .Name = "Arial Cyr"
                .Size = 8
                .Strikethrough = False
                .Superscript = False
                .Subscript = False
                .OutlineFont = False
                .Shadow = False
                .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Range("A23").Select()

            '--- основная таблица -------------------------------------------
            For I = 1 To PointTarirCount
                .Range("A" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = I ' номер контрольной точки
                .Range("B" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = AverageInput(I) ' значение АЦП
                .Range("C" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = PhisicalEtalon(I) ' Эталон
                .Range("D" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = среднееФизикаПолином(I, 1) ' Прямой
                .Range("E" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = среднееФизикаПолином(I, 2) ' Обратный
                .Range("F" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = вариация(I) ' Вариация
                .Range("G" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = систематическаяПогрешности(I) ' Системат. составл. погрешности
                .Range("H" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = оценкаСКО(I) ' Оценка СКО
                .Range("I" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = доверительныйИнтервал(I, 1) ' Доверительный интервал нижний
                .Range("J" & 22 + I).Select()
                .ActiveCell.FormulaR1C1 = доверительныйИнтервал(I, 2) ' Доверительный интервал верхний
            Next

            .Range("A23:J" & 22 + PointTarirCount).Select()
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlRight
                .VerticalAlignment = Excel.Constants.xlTop
                .WrapText = True
                .Orientation = 0
                .AddIndent = False
                .ShrinkToFit = False
                .MergeCells = False
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With

            '--- выводится результирующая таблица ----------------------------
            .Range("C" & 24 + PointTarirCount & ":" & "H" & 24 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("C" & 24 + PointTarirCount & ":" & "H" & 24 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Результат поверки для доверительной вероятности Р=0.95"
            With .ActiveCell.Characters(Start:=1, Length:=54).Font
                .Size = 9
                .Bold = True
            End With
            .Range("C" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Вариация max"
            .Range("D" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Систем. сост. погрешн. max"
            .Range("E" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Среднее среднекв. Отклон. "
            .Range("E" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Среднее среднекв. отклон. во всем диапазоне  "
            .Range("F" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = vbNullString
            .Range("F" & 25 + PointTarirCount & ":" & "G" & 25 + PointTarirCount).Select()
            .Selection.Merge() ' объединить две ячейки
            .Range("F" & 25 + PointTarirCount & ":" & "G" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Интервал погрешности канала во всем диапазоне"
            .Range("H" & 25 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Относительная погрешность %"
            .Range("H" & 26 + PointTarirCount).Select()
            .Rows._Default(25 + PointTarirCount & ":" & 25 + PointTarirCount).RowHeight = 60
            .Range("C" & 24 + PointTarirCount & ":" & "H" & 26 + PointTarirCount).Select()
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With

            '***************************
            .Range("D" & 28 + PointTarirCount & ":" & "E" & 28 + PointTarirCount).Select()
            .Selection.Merge()
            .ActiveCell.FormulaR1C1 = "Полином степени:"
            .Range("D" & 29 + PointTarirCount & ":" & "F" & 29 + PointTarirCount).Select()
            .Selection.Merge()
            .ActiveCell.FormulaR1C1 = "Коэффициенты аппроксимации"
            .Range("D" & 30 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a0"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With
            .Range("D" & 31 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a1"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With
            .Range("D" & 32 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a2"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With
            .Range("D" & 33 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a3"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With
            .Range("D" & 34 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a4"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With
            .Range("D" & 35 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "a5"
            With .ActiveCell.Characters(Start:=2, Length:=1).Font
                .Subscript = True
            End With

            .Range("E" & 30 + PointTarirCount & ":" & "F" & 30 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("E" & 31 + PointTarirCount & ":" & "F" & 31 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("E" & 32 + PointTarirCount & ":" & "F" & 32 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("E" & 33 + PointTarirCount & ":" & "F" & 33 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("E" & 34 + PointTarirCount & ":" & "F" & 34 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("E" & 35 + PointTarirCount & ":" & "F" & 35 + PointTarirCount).Select()
            .Selection.Merge()
            .Range("D" & 28 + PointTarirCount & ":" & "F" & 35 + PointTarirCount).Select()
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Range("D" & 37 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = "Исполнитель:"
            .Range("D" & 37 + PointTarirCount & ":" & "E" & 37 + PointTarirCount).Select()
            .Selection.Merge()
            With .Selection.Font
                .Size = 10
            End With
            .Range("C" & 37 + PointTarirCount).Select()

            '--- заполнить шапку ---------------------------------------------
            .Range("B3").Select()
            .ActiveCell.FormulaR1C1 = Today ' Дата
            .Range("E3").Select()
            .ActiveCell.FormulaR1C1 = numberParameter ' номер параметра
            .Range("F3").Select()
            .ActiveCell.FormulaR1C1 = nameParameter ' наименование параметра
            .Range("I3").Select()
            .ActiveCell.FormulaR1C1 = numberChannel ' номер канала
            .Range("C4").Select()
            .ActiveCell.FormulaR1C1 = $"{minVal} : {maxVal}" ' диапазон АЦП нижний-верхний

            .Range("H4").Select()
            .ActiveCell.FormulaR1C1 = $"{Math.Round(PhisicalEtalon(1), 3)} : {Math.Round(PhisicalEtalon(PointTarirCount), 3)} {unitChannel}" ' диапазон параметра  нижний-верхний-единица
            '--- заполнить результат ------------------------------------------
            .Range("C" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = maxВариация ' вариация max
            .Range("D" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = maxСистематическая ' систем. составляющая погрешности max
            .Range("E" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = quadraticMeanAllRange ' среднее СКО
            .Range("F" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = интервалПогрешностиКанала(1) ' нижний интервал погрешности
            .Range("G" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = интервалПогрешностиКанала(2) ' верхний интервал погрешности
            .Range("H" & 26 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = погрешностьПроцент ' относительная погрешность
            .Range("F" & 28 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = polynomialOrder
            .Range("E" & 30 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial0.Text ' A0
            .Range("E" & 31 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial1.Text ' A1
            .Range("E" & 32 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial2.Text ' A2
            .Range("E" & 33 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial3.Text ' A3
            .Range("E" & 34 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial4.Text ' A4
            .Range("E" & 35 + PointTarirCount).Select()
            .ActiveCell.FormulaR1C1 = TextPolynomial5.Text ' A5

            '--- Graph -------------------------------------------------------
            .Sheets("Лист2").Select()
            Re.Dim(averageX, PointTarirCount - 1)
            For I = 0 To PointTarirCount - 1
                averageX(I) = AverageInput(I + 1)
                .Range("C" & I + 1).Select()
                .ActiveCell.FormulaR1C1 = YOutput(I)
                .Range("D" & I + 1).Select()
                .ActiveCell.FormulaR1C1 = XPhisical(I)
            Next

            Dim minimum, maximum As Double
            Dim indexOfMinimum, indexOfMaximum As Integer

            ArrayOperation.MaxMin1D(averageX, maximum, indexOfMaximum, minimum, indexOfMinimum)

            Dim polynomialCurveX((PointTarirCount - 1) * PointsCount) As Double

            Dim range As Double = Math.Abs(maximum - minimum)
            Dim hundredRange As Integer = (PointTarirCount - 1) * PointsCount

            For I = 0 To hundredRange
                polynomialCurveX(I) = I * range / hundredRange + AverageInput(1)
            Next

            Dim pointsPolynomialCurveX() As Double = ArrayOperation.PolynomialEvaluation1D(polynomialCurveX, CoefficientsPolynomial) ' вычислить для построения графика
            Dim increment As Double = (range / (PointTarirCount - 1)) / PointsCount

            For I = 0 To hundredRange
                .Range("A" & I + 1).Select()
                .ActiveCell.FormulaR1C1 = minimum
                .Range("B" & I + 1).Select()
                .ActiveCell.FormulaR1C1 = pointsPolynomialCurveX(I)
                minimum += increment
            Next

            .Sheets("Лист1").Select()
            .Range("A1:J1").Select()
            .Charts.Add()
            .ActiveChart.ChartTitle.Characters.Text = "График тарировочной характеристики"
            .ActiveChart.ChartType = Excel.XlChartType.xlXYScatterSmoothNoMarkers
            .ActiveChart.Location(Where:=Excel.XlChartLocation.xlLocationAsObject, Name:="Лист1")
            .ActiveChart.SetSourceData(Source:= .Sheets("Лист2").Range("A1:B3" & hundredRange + 1), PlotBy:=Excel.XlRowCol.xlColumns)
            .ActiveChart.SeriesCollection.NewSeries()
            .ActiveChart.SeriesCollection(2).XValues = $"=Лист2!R1C3:R{PointTarirCount}C3"
            .ActiveChart.SeriesCollection(2).Values = $"=Лист2!R1C4:R{PointTarirCount}C4"
            With .ActiveChart
                .HasTitle = True
                ' в этом месте новый Excel вызывает ошибку .ChartTitle.Characters.Text = "График тарировочной характеристики"
                .Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).HasTitle = True
                .Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Characters.Text = "Вольт"
                .Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).HasTitle = True
                .Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).AxisTitle.Characters.Text = nameParameter
            End With
            With .ActiveChart.Axes(Excel.XlAxisType.xlCategory)
                .HasMajorGridlines = True
                .HasMinorGridlines = False
            End With
            With .ActiveChart.Axes(Excel.XlAxisType.xlValue)
                .HasMajorGridlines = True
                .HasMinorGridlines = False
            End With
            .ActiveSheet.Shapes(1).IncrementLeft(-500)
            .ActiveSheet.Shapes(1).IncrementTop(-100)
            'If ЧислоТочекТарировки > 5 Then .ActiveSheet.Shapes(1).IncrementTop(-357.0#)
            ' подобрать коэффициенты масштабирования Chart
            .ActiveSheet.Shapes(1).ScaleWidth(1.3, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoScaleFrom.msoScaleFromTopLeft)
            .ActiveSheet.Shapes(1).ScaleHeight(1.3, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoScaleFrom.msoScaleFromTopLeft)
            .ActiveChart.HasLegend = True
            .ActiveChart.Legend.Select()
            .Selection.Position = Excel.Constants.xlBottom
            .ActiveChart.ApplyDataLabels(Type:=Excel.XlDataLabelsType.xlDataLabelsShowNone, LegendKey:=False)
            .ActiveChart.PlotArea.Select()
            With .Selection.Border
                .ColorIndex = 16
                .Weight = Excel.XlBorderWeight.xlThin
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Interior
                .ColorIndex = 2
                .PatternColorIndex = 1
                .Pattern = Excel.Constants.xlSolid
            End With
            .ActiveChart.SeriesCollection(1).Select()
            With .Selection.Border
                .ColorIndex = 1
                .Weight = Excel.XlBorderWeight.xlThin
                .LineStyle = Excel.XlLineStyle.xlDot
            End With
            With .Selection
                .MarkerBackgroundColorIndex = Excel.Constants.xlNone
                .MarkerForegroundColorIndex = Excel.Constants.xlNone
                .MarkerStyle = Excel.Constants.xlNone
                .Smooth = True
                .MarkerSize = 3
                .Shadow = False
            End With
            .ActiveChart.SeriesCollection(2).Select()
            With .Selection.Border
                .ColorIndex = 3
                .Weight = Excel.XlBorderWeight.xlMedium
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection
                .MarkerBackgroundColorIndex = 5
                .MarkerForegroundColorIndex = 5
                .MarkerStyle = Excel.Constants.xlDiamond
                .Smooth = False
                .MarkerSize = 3
                .Shadow = False
            End With
            .ActiveChart.ChartArea.Select()
            .ActiveChart.SeriesCollection(1).Name = "=""Аппроксимация"""
            .ActiveChart.SeriesCollection(2).Name = "=""Исходные"""

            .ActiveChart.Axes(Excel.XlAxisType.xlCategory).Select()
            With .ActiveChart.Axes(Excel.XlAxisType.xlCategory)
                .MinimumScale = Math.Floor(YOutput(0))
                .MaximumScale = Math.Ceiling(YOutput(PointTarirCount - 1))
                .MinorUnitIsAuto = True
                .MajorUnitIsAuto = True
                .Crosses = Excel.Constants.xlAutomatic
                .ReversePlotOrder = False
                .ScaleType = Excel.XlTrendlineType.xlLinear
                .DisplayUnit = Excel.Constants.xlNone
            End With
            .ActiveChart.Axes(Excel.XlAxisType.xlValue).Select()
            With .ActiveChart.Axes(Excel.XlAxisType.xlValue)
                .MinimumScale = Math.Floor(XPhisical(0))
                .MaximumScale = Math.Ceiling(XPhisical(PointTarirCount - 1))
                .MinorUnitIsAuto = True
                .MajorUnitIsAuto = True
                .Crosses = Excel.Constants.xlAutomatic
                .ReversePlotOrder = False
                .ScaleType = Excel.XlTrendlineType.xlLinear
                .DisplayUnit = Excel.Constants.xlNone
            End With

            If isImitatorMetrology Then
                Dim indexOfset As Integer
                .Sheets("Лист3").Select()

                For I = 1 To PointTarirCount
                    indexOfset = I + 7

                    .Range("B" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = PhisicalEtalon(I)

                    .Range("C" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 1, 1)
                    .Range("D" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 1, 2)

                    .Range("E" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 2, 1)
                    .Range("F" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 2, 2)

                    .Range("G" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 3, 1)
                    .Range("H" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 3, 2)

                    .Range("I" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 4, 1)
                    .Range("J" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 4, 2)

                    .Range("K" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 5, 1)
                    .Range("L" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 5, 2)

                    indexOfset = I + 20

                    .Range("A" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 6, 1)
                    .Range("B" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 6, 2)

                    .Range("C" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 7, 1)
                    .Range("D" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 7, 2)

                    .Range("E" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 8, 1)
                    .Range("F" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 8, 2)

                    .Range("G" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 9, 1)
                    .Range("H" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 9, 2)

                    .Range("I" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 10, 1)
                    .Range("J" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = acquisitionVolt(I, 10, 2)

                    .Range("K" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = AverageInput(I)

                    .Range("L" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = среднееФизикаПолином(I, 1)
                    .Range("M" & indexOfset).Select()
                    .ActiveCell.FormulaR1C1 = среднееФизикаПолином(I, 2)
                Next
            End If

            .Sheets("Лист1").Select()
            .Range("A1").Select()

            .ActiveWorkbook.SaveCopyAs(Path.Combine(PathResourses, "Протоколы тарировки", nameParameter) & ".xlsx")

            '--- печать ------------------------------------------------------
            If isPrint Then
                Dim strPrinterName As String
                Dim dlg As New PrintDialog
                Dim pd As Printing.PrintDocument = New Printing.PrintDocument
                dlg.Document = pd

                If dlg.ShowDialog() = DialogResult.OK Then
                    strPrinterName = dlg.PrinterSettings.PrinterName ' "\\PENTIUM4\HP DeskJet 1220C (Ne01:)"

                    If dlg.PrinterSettings.IsValid Then
                        Try
                            .ActiveWindow.SelectedSheets.PrintOut(Copies:=1, ActivePrinter:=strPrinterName, Collate:=True)
                            '1 System.Threading.Thread.Sleep(20000) 'Sleep(10000)
                            'Application.DoEvents()
                        Catch ex As Exception
                            Const caption As String = NameOf(PrintReport)
                            Dim text As String = ex.ToString
                            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        End Try
                    Else
                        Const caption As String = NameOf(PrintReport)
                        Const text As String = "Принтер не установлен."
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    End If
                End If
            End If
        End With
    End Sub
#End Region

#Region "DrawItem"
    Private Sub ComboBoxParameters_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ComboBoxParameters.DrawItem
        If CollectionForms.Forms.ContainsKey(Me.Text) Then ' был случай обработки события, когда форма уже закрывается
            DrawItemComboBox(sender, die, Nothing, ImageList2, False)
        End If
    End Sub

    Private Sub ComboBoxParameters_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ComboBoxParameters.MeasureItem
        Dim cb As ComboBox = CType(sender, ComboBox)
        mie.ItemHeight = cb.ItemHeight - 2
    End Sub
#End Region
End Class