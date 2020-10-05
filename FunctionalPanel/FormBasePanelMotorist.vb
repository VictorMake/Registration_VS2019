Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Soap
Imports System.Security.Permissions
Imports System.Text
Imports NationalInstruments.DAQmx
Imports NationalInstruments.UI.WindowsForms
Friend Class FormBasePanelMotorist
    Private Const КОНТРОЛ_ИМЕЕТ_ПУСТОЙ_TAG As String = "Контрол {0} имеет пустой Tag"
    Private Const ДЛЯ_КОНТРОЛА_ЗАДАН_НЕВЕРНЫЙ_ПАРАМЕТР As String = "Для контрола с именем <{0}> и типа <{1}> задан параметр: {2}{3}В серверных каналах опроса такого имени нет."
    Private Const AXES_X As String = "AxesX"
    Private Const AXES_Y As String = "AxesY"

    '--- Коллекции элементов -------------------------------------------------
    ' индикаторы
    Private tankList As New List(Of Tank)
    Private gaugeList As New List(Of Gauge)
    Private thermometerList As New List(Of Thermometer)
    Private meterList As New List(Of Meter)
    ' графики
    Private waveformGraphList As New List(Of WaveformGraph)
    Private scatterGraphList As New List(Of ScatterGraph)
    ' задатчики
    Private switchList As New List(Of WindowsForms.Switch)
    Public ledList As New List(Of Led)
    Private numericEditList As New List(Of NumericEdit)
    Private knobList As New List(Of Knob)
    Public slideList As New List(Of Slide)
    '--- Коллекции свойств для элементов -------------------------------------
    ' индикаторы
    Private tankPropertiesDic As New Dictionary(Of String, TankProperties)
    Private gaugePropertiesDic As New Dictionary(Of String, GaugeProperties)
    Private thermometerPropertiesDic As New Dictionary(Of String, ThermometerProperties)
    Private meterPropertiesDic As New Dictionary(Of String, MeterProperties)
    ' графики
    Private waveformGraphPropertiesDic As New Dictionary(Of String, WaveformGraphProperties)
    Private scatterGraphPropertiesDic As New Dictionary(Of String, ScatterGraphProperties)
    ' задатчики
    Private switchPropertiesDic As New Dictionary(Of String, SwitchProperties)
    Private ledPropertiesDic As New Dictionary(Of String, LedProperties)
    Private numericEditPropertiesDic As New Dictionary(Of String, NumericEditProperties)
    Private knobPropertiesDic As New Dictionary(Of String, KnobProperties)
    Private slidePropertiesDic As New Dictionary(Of String, SlideProperties)

    ' организация связи с серверной переменной для связанного контрола
    Private numericEditMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private tankMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private gaugeMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private thermometerMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private meterMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private waveformGraphMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)
    Private scatterGraphMyTypeDic As New Dictionary(Of String, TypeBaseParameterTCP)

    Private captionAddedControls As New List(Of String) ' список контролов к которым добавлено описание
    Private funcsActionPutToListDictionary As New Dictionary(Of String, Action(Of Control, List(Of String)))() ' указатели на функции обработки
    Private funcsActionInitControlDictionary As New Dictionary(Of String, Action(Of Control, List(Of String)))() ' указатели на функции обработки
    Private funcsActionAddLabelDictionary As New Dictionary(Of String, Action(Of Control))() ' указатели на функции обработки
    'Private funcsActionModifyControlDictionary As New Dictionary(Of String, Action(Of Control, List(Of String)))() ' указатели на функции обработки

    ''' <summary>
    ''' нет Добавлений Надписей
    ''' </summary>
    Private isNoAddedCaption As Boolean = True ' флаг остановки рекурсии по контролам формы
    ''' <summary>
    ''' модификация Контролов Проведена
    ''' </summary>
    Private isModifyControlSsuccess As Boolean = False ' для предотвращения повторного перемещения контролов в уже открытой формы

    '--- свойства -------------------------------------------------------------
    ''' <summary>
    ''' Имя Панели Моториста
    ''' </summary>
    ''' <returns></returns>
    Public Property NameMotoristPanel() As String
    Public Property IsTestSuccess() As Boolean

    Private WithEvents mMainMDIFormReference As FormRegistrationBase
    Public Property MainMDIFormReference() As FormRegistrationBase
        Get
            Return mMainMDIFormReference
        End Get
        Set(ByVal Value As FormRegistrationBase)
            mMainMDIFormReference = Value
        End Set
    End Property

    ' Свойство для лёгкого доступа к контролам формы
    Public ReadOnly Property PanelBaseFormControls() As Control.ControlCollection
        Get
            Return Controls
        End Get
    End Property

    Private mPanelName As String
    Public ReadOnly Property PanelName() As String
        Get
            Return mPanelName
        End Get
    End Property

    Private mFileName As String
    Public Property FileName() As String
        Get
            Return mFileName
        End Get
        Set(ByVal Value As String)
            mFileName = Value
            mPanelName = Path.GetFileNameWithoutExtension(mFileName)
            Text = PanelName
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        'This is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        MDITabPanelMotoristForm.AddChildWindow(Me)
    End Sub

    Private Sub FormBasePanelMotorist_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)

        FindFormRegistration()
        'If (Not strФормаРегистратор Is Nothing) AndAlso strФормаРегистратор.IndexOf("Регистратор") <> -1 Then
        '    ЗаполнитьСпискиПараметровОтСервера()
        'End If
        'Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        'в дизайнере формы по другому
        WindowState = FormWindowState.Normal
        FormBorderStyle = FormBorderStyle.Sizable
        MaximizeBox = True
        MinimizeBox = True

        'Me.DoubleBuffered
        If MainMDIFormReference IsNot Nothing Then MainMDIFormReference.Refresh()

        MdiParent.Refresh()
    End Sub

    ''' <summary>
    ''' Найти Форму Регистратора
    ''' </summary>
    Private Sub FindFormRegistration()
        If RegistrationFormName <> "" Then
            For Each itemForm In CollectionForms.Forms.Values
                If itemForm.Text = RegistrationFormName Then
                    MainMDIFormReference = CType(itemForm, FormRegistrationBase)
                    Exit For
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Закрыть При Неудаче Конфигурирования
    ''' </summary>
    Public Sub CloseOnFailureConfiguration()
        FormBasePanelMotorist_FormClosing(Me, Nothing)
    End Sub

    Private Sub FormBasePanelMotorist_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)

        Try
            MainMDIFormReference = Nothing
            ' сбросить все порты
            If switchList.Count > 0 Then
                For Each SwitchItem In switchList
                    ' обнулить перед выгрузкой
                    If IsTestSuccess Then SwitchItem.Value = False ' обнулить значение (должно сработать SwitchItem.StateChanged)

                    RemoveHandler SwitchItem.StateChanged, AddressOf Switch_StateChanged
                Next
            End If

            If knobList.Count > 0 Then
                For Each KnobItem In knobList
                    RemoveHandler KnobItem.AfterChangeValue, AddressOf Knob_AfterChangeValue
                Next
            End If

            If slideList.Count > 0 Then
                For Each SlideItem In slideList
                    ' обнулить перед выгрузкой
                    If IsTestSuccess Then SlideItem.Value = SlideItem.Range.Minimum

                    RemoveHandler SlideItem.AfterChangeValue, AddressOf Slide_AfterChangeValue
                Next
            End If

            ' установить свойство ClosingComplete в true,
            ' чтобы позволить классу формы знать, что она может быть удалена из коллекции
            'closingCompleteValue = True

            'If Me.dirty Then
            '    ' Спросить пользователя Сохранить (Yes), Не сохранять (No), или прервать закрытие (Cancel)
            '    Dim strDocTitle As String
            '    Dim strMsg As String = String.Format("Хотите сохранить {0}?", strDocTitle)

            '    Select Case MessageBox.Show(strMsg, "Закрытие", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            '        Case Windows.Forms.DialogResult.Yes                       
            '            ' Записать документ 
            '            Me.SaveDocument()
            '        Case Windows.Forms.DialogResult.No
            '            ' не сохранять документ, просто выйти
            '            ' вставить необходимый код здесь
            '        Case Windows.Forms.DialogResult.Cancel
            '            ' Остановить закрытие формы
            '            e.Cancel = True
            '            ' Если закрытие прервано,необходимо сохранить
            '            ' форму в основной коллекции форм
            '            closingCompleteValue = False
            '            ' Вызвать событие остановки приложения
            '            ' при закрытии любых открытых документов
            '            RaiseEvent SaveWhileClosingCancelled(Me, Nothing)
            '    End Select
            'End If
        Catch exp As Exception
            Dim caption As String = $"Процедура {NameOf(FormBasePanelMotorist_FormClosing)}"
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Очистка коллекций во перед конфигурированием формы
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearAllCollection()
        tankList.Clear()
        gaugeList.Clear()
        thermometerList.Clear()
        meterList.Clear()
        waveformGraphList.Clear()
        scatterGraphList.Clear()

        switchList.Clear()
        ledList.Clear()
        numericEditList.Clear()
        knobList.Clear()
        slideList.Clear()

        tankPropertiesDic.Clear()
        gaugePropertiesDic.Clear()
        thermometerPropertiesDic.Clear()
        meterPropertiesDic.Clear()

        waveformGraphPropertiesDic.Clear()
        scatterGraphPropertiesDic.Clear()

        switchPropertiesDic.Clear()
        ledPropertiesDic.Clear()
        numericEditPropertiesDic.Clear()
        knobPropertiesDic.Clear()
        slidePropertiesDic.Clear()

        numericEditMyTypeDic.Clear()
        tankMyTypeDic.Clear()
        gaugeMyTypeDic.Clear()
        thermometerMyTypeDic.Clear()
        meterMyTypeDic.Clear()
        waveformGraphMyTypeDic.Clear()
        scatterGraphMyTypeDic.Clear()

        funcsActionPutToListDictionary.Clear()
        funcsActionInitControlDictionary.Clear()
        funcsActionAddLabelDictionary.Clear()
        'funcsActionModifyControlDictionary.Clear()
    End Sub

    ''' <summary>
    ''' Конфигурировать Контролы Панели
    ''' Проверка корректности настроек контролов на форме
    ''' и создание коллекций контролов и свойтсв по их типам.
    ''' Очистка коллекций во перед конфигурированием формы
    ''' </summary>
    ''' <remarks></remarks>
    Public Function PopulatePanelByControls() As Boolean
        Dim errors As New List(Of String)

        IsTestSuccess = False

        If Not isModifyControlSsuccess Then
            PrepareListParentControls(errors)

            If errors.Count = 0 Then
                ' должна идти первой перед ПроверкаДискретныхВходов(errors) для проверки назначения порта на OUTPUT или INPUT
                ' проверку делать имеет смысл делать, если ошибки на предыдущем уровне не было
                If errors.Count = 0 Then CheckDiscreteOutput(errors)
                If errors.Count = 0 Then CheckDiscreteInput(errors)
                If errors.Count = 0 Then CheckAnalogOutputGenerator(errors)
                If errors.Count = 0 Then CheckAnalogOutputVolt(errors)
                If errors.Count = 0 Then CheckAnalogInput(errors)

                ClearAllCollection()

                If errors.Count = 0 Then
                    InitializeParentControls(errors)

                    Do
                        isNoAddedCaption = True
                        AddCaptionParentControls()
                    Loop Until isNoAddedCaption

                    isModifyControlSsuccess = True
                End If
            End If
        End If

        If errors.Count > 0 Then
            Dim I As Integer
            Dim result As New StringBuilder("Были обнаружены следующие проблемы:" & Environment.NewLine) ' )

            For Each strError As String In errors
                result.AppendLine(strError)
                I += 1
                If I > 20 Then
                    result.AppendLine("и далее..")
                    Exit For
                End If
            Next

            Const caption As String = "Загрузка панели"
            Dim text As String = result.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Return False
        Else
            Return True
        End If
    End Function

#Region "Рекурсия контролов"
    'Private Sub AddIn(dictionary As Dictionary(Of String, Action(Of Object, Object)), имяКонтрола As String)
    '    dictionary.Add(имяКонтрола, (Sub(parametrs, data) Console.WriteLine("Контрол:{0}, parametrs->{1}, data->{2}", имяКонтрола, parametrs, data)))
    'End Sub

    ''' <summary>
    ''' Составить Список Контролы Верхнего Уровня
    ''' Рекурсивный проход по контролам формы
    ''' </summary>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub PrepareListParentControls(ByVal errors As List(Of String))
        'Private ТипыКонтролов As List(Of [String]) = New List(Of String)() From {"Тип1",
        '                                                              "Тип2",
        '                                                              "Тип3",
        '                                                              "Тип4",
        '                                                              "Тип5"}

        'For Each имяКонтрола In ТипыКонтролов
        '    AddIn(funcsDictionary, имяКонтрола)
        'Next

        'funcsDictionary = New Dictionary(Of String, Action(Of Object, Object))() From {{GetType(WindowsForms.Switch).ToString, New Action(Of Object, Object)(Sub(aControl As System.Windows.Forms.Control, errors As List(Of String))
        '                                                                                                                                                         If SwitchList.Contains(aControl) Then
        '                                                                                                                                                              MessageControl(aControl, errors)
        '                                                                                                                                                         Else
        '                                                                                                                                                             Dim mSwitchProperties As SwitchProperties = GetControlBaseFromTag(aControl, errors)
        '                                                                                                                                                             If mSwitchProperties IsNot Nothing Then
        '                                                                                                                                                                 SwitchPropertiesDic.Add(aControl.Name, mSwitchProperties)
        '                                                                                                                                                                 SwitchList.Add(aControl)
        '                                                                                                                                                             End If
        '                                                                                                                                                         End If
        '                                                                                                                                                     End Sub)},
        '                                                                               {GetType(WindowsForms.Led).ToString, New Action(Of Object, Object)(Sub(aControl As System.Windows.Forms.Control, errors As List(Of String))
        '                                                                                                                                                      If LedList.Contains(aControl) Then
        '                                                                                                                                                           MessageControl(aControl, errors)
        '                                                                                                                                                      Else
        '                                                                                                                                                          Dim mLedProperties As LedProperties = GetControlBaseFromTag(aControl, errors)
        '                                                                                                                                                          If mLedProperties IsNot Nothing Then
        '                                                                                                                                                              LedPropertiesDic.Add(aControl.Name, mLedProperties)
        '                                                                                                                                                              LedList.Add(aControl)
        '                                                                                                                                                          End If
        '                                                                                                                                                      End If
        '                                                                                                                                                  End Sub)}}

        ' составить коллекцию указателей обработчиков функций для контролов
        ' так как обработчики работают с различающимися типами, то применить обобщённую или простую процедуру нельзя
        funcsActionPutToListDictionary = New Dictionary(Of String, Action(Of Control, List(Of String)))() From {
            {GetType(WindowsForms.Switch).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListSwitch)},
            {GetType(Led).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListLed)},
            {GetType(Knob).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListKnob)},
            {GetType(Slide).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListSlide)},
            {GetType(NumericEdit).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListNumericEdit)},
            {GetType(Tank).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListTank)},
            {GetType(Gauge).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListGauge)},
            {GetType(Thermometer).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListThermometer)},
            {GetType(Meter).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListMeter)},
            {GetType(WaveformGraph).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListWaveformGraph)},
            {GetType(ScatterGraph).ToString, New Action(Of Control, List(Of String))(AddressOf ActionPutToListScatterGraph)}}

        For Each aControl As Control In Me.Controls
            PrepareListChildControls(aControl, errors)
        Next
    End Sub

    ''' <summary>
    ''' Инициализировать Контролы Верхнего Уровня
    ''' Рекурсивный проход по контролам формы
    ''' </summary>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub InitializeParentControls(ByVal errors As List(Of String))
        'data = New DataManager
        'data.IsVertical = True

        funcsActionInitControlDictionary = New Dictionary(Of String, Action(Of Control, List(Of String)))() From {{GetType(WindowsForms.Switch).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlSwitch)},
                                                                               {GetType(Led).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlLed)},
                                                                               {GetType(Knob).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlKnob)},
                                                                               {GetType(Slide).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlSlide)},
                                                                               {GetType(NumericEdit).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlNumericEdit)},
                                                                               {GetType(Tank).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlTank)},
                                                                               {GetType(Gauge).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlGauge)},
                                                                               {GetType(Thermometer).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlThermometer)},
                                                                               {GetType(Meter).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlMeter)},
                                                                               {GetType(WaveformGraph).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlWaveformGraph)},
                                                                               {GetType(ScatterGraph).ToString, New Action(Of Control, List(Of String))(AddressOf ActionInitControlScatterGraph)}}

        For Each aControl As Control In Me.Controls
            InitializeChildControls(aControl, errors)
        Next
    End Sub

    ''' <summary>
    ''' Добавить Надписи Контролы Верхнего Уровня
    ''' </summary>
    Private Sub AddCaptionParentControls()
        funcsActionAddLabelDictionary = New Dictionary(Of String, Action(Of Control))() From {{GetType(WindowsForms.Switch).ToString, New Action(Of Control)(AddressOf ActionAddLabelSwitch)},
                                                                       {GetType(Led).ToString, New Action(Of Control)(AddressOf ActionAddLabelLed)},
                                                                       {GetType(Knob).ToString, New Action(Of Control)(AddressOf ActionAddLabelKnob)},
                                                                       {GetType(Slide).ToString, New Action(Of Control)(AddressOf ActionAddLabelSlide)},
                                                                       {GetType(NumericEdit).ToString, New Action(Of Control)(AddressOf ActionAddLabelNumericEdit)},
                                                                       {GetType(Tank).ToString, New Action(Of Control)(AddressOf ActionAddLabelTank)},
                                                                       {GetType(Gauge).ToString, New Action(Of Control)(AddressOf ActionAddLabelGauge)},
                                                                       {GetType(Thermometer).ToString, New Action(Of Control)(AddressOf ActionAddLabelThermometer)},
                                                                       {GetType(Meter).ToString, New Action(Of Control)(AddressOf ActionAddLabelMeter)}}

        For Each aControl As Control In Me.Controls
            AddCaptionChildControls(aControl)
        Next
    End Sub


#Region "ActionPutToList"
    ''' <summary>
    ''' Обработать Action для контрола из словаря указателей на функции.
    ''' Данный подход плозволяет исключить большие блоки Case и легко добавлять новые обработчики
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub ExecFuncPutToList(ByVal aControl As Control, ByVal errors As List(Of String)) 'Optional parametrs As Object = Nothing)
        If funcsActionPutToListDictionary.Keys.Contains(aControl.GetType.ToString) Then
            funcsActionPutToListDictionary(aControl.GetType.ToString).Invoke(aControl, errors)
        End If
    End Sub

    Private Sub ActionPutToListSwitch(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mSwitch = CType(aControl, WindowsForms.Switch)
        If switchList.Contains(mSwitch) Then
            MessageControl(aControl, errors)
        Else
            Dim mSwitchProperties As SwitchProperties = CType(GetControlBaseFromTag(mSwitch, errors), SwitchProperties)

            If mSwitchProperties IsNot Nothing Then
                switchPropertiesDic.Add(mSwitch.Name, mSwitchProperties)
                switchList.Add(mSwitch)
            End If
        End If
    End Sub

    Private Sub ActionPutToListLed(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mLed = CType(aControl, Led)
        If ledList.Contains(mLed) Then
            MessageControl(aControl, errors)
        Else
            Dim mLedProperties As LedProperties = CType(GetControlBaseFromTag(mLed, errors), LedProperties)

            If mLedProperties IsNot Nothing Then
                ledPropertiesDic.Add(mLed.Name, mLedProperties)
                ledList.Add(mLed)
            End If
        End If
    End Sub

    Private Sub ActionPutToListKnob(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mKnob = CType(aControl, Knob)
        If knobList.Contains(mKnob) Then
            MessageControl(aControl, errors)
        Else
            Dim mKnobProperties As KnobProperties = CType(GetControlBaseFromTag(mKnob, errors), KnobProperties)

            If mKnobProperties IsNot Nothing Then
                knobPropertiesDic.Add(mKnob.Name, mKnobProperties)
                knobList.Add(mKnob)
            End If
        End If
    End Sub

    Private Sub ActionPutToListSlide(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mSlide = CType(aControl, Slide)
        If slideList.Contains(mSlide) Then
            MessageControl(aControl, errors)
        Else
            Dim mSlideProperties As SlideProperties = CType(GetControlBaseFromTag(mSlide, errors), SlideProperties)

            If mSlideProperties IsNot Nothing Then
                slidePropertiesDic.Add(aControl.Name, mSlideProperties)
                slideList.Add(mSlide)
            End If
        End If
    End Sub

    Private Sub ActionPutToListNumericEdit(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mNumericEdit = CType(aControl, NumericEdit)
        If numericEditList.Contains(mNumericEdit) Then
            MessageControl(aControl, errors)
        Else
            Dim mNumericEditProperties As NumericEditProperties = CType(GetControlBaseFromTag(mNumericEdit, errors), NumericEditProperties)

            If mNumericEditProperties IsNot Nothing Then
                numericEditPropertiesDic.Add(mNumericEdit.Name, mNumericEditProperties)
                numericEditList.Add(mNumericEdit)
            End If
        End If
    End Sub

    Private Sub ActionPutToListTank(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mTank = CType(aControl, Tank)
        If tankList.Contains(mTank) Then
            MessageControl(aControl, errors)
        Else
            Dim mTankProperties As TankProperties = CType(GetControlBaseFromTag(mTank, errors), TankProperties)

            If mTankProperties IsNot Nothing Then
                tankPropertiesDic.Add(mTank.Name, mTankProperties)
                tankList.Add(mTank)
            End If
        End If
    End Sub

    Private Sub ActionPutToListGauge(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mGauge = CType(aControl, Gauge)
        If gaugeList.Contains(mGauge) Then
            MessageControl(aControl, errors)
        Else
            Dim mGaugeProperties As GaugeProperties = CType(GetControlBaseFromTag(mGauge, errors), GaugeProperties)

            If mGaugeProperties IsNot Nothing Then
                gaugePropertiesDic.Add(mGauge.Name, mGaugeProperties)
                gaugeList.Add(mGauge)
            End If
        End If
    End Sub

    Private Sub ActionPutToListThermometer(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mThermometer = CType(aControl, Thermometer)
        If thermometerList.Contains(mThermometer) Then
            MessageControl(aControl, errors)
        Else
            Dim mThermometerProperties As ThermometerProperties = CType(GetControlBaseFromTag(mThermometer, errors), ThermometerProperties)

            If mThermometerProperties IsNot Nothing Then
                thermometerPropertiesDic.Add(mThermometer.Name, mThermometerProperties)
                thermometerList.Add(mThermometer)
            End If
        End If
    End Sub

    Private Sub ActionPutToListMeter(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mMeter = CType(aControl, Meter)
        If meterList.Contains(mMeter) Then
            MessageControl(aControl, errors)
        Else
            Dim mMeterProperties As MeterProperties = CType(GetControlBaseFromTag(mMeter, errors), MeterProperties)

            If mMeterProperties IsNot Nothing Then
                meterPropertiesDic.Add(mMeter.Name, mMeterProperties)
                meterList.Add(mMeter)
            End If
        End If
    End Sub

    Private Sub ActionPutToListWaveformGraph(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mWaveformGraph = CType(aControl, WaveformGraph)
        If waveformGraphList.Contains(mWaveformGraph) Then
            MessageControl(aControl, errors)
        Else
            Dim mWaveformGraphProperties As WaveformGraphProperties = CType(GetControlBaseFromTag(mWaveformGraph, errors), WaveformGraphProperties)

            If mWaveformGraphProperties IsNot Nothing Then
                waveformGraphPropertiesDic.Add(mWaveformGraph.Name, mWaveformGraphProperties)
                waveformGraphList.Add(mWaveformGraph)
            End If
        End If
    End Sub

    Private Sub ActionPutToListScatterGraph(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mScatterGraph = CType(aControl, ScatterGraph)
        If scatterGraphList.Contains(mScatterGraph) Then
            MessageControl(aControl, errors)
        Else
            Dim mScatterGraphProperties As ScatterGraphProperties = CType(GetControlBaseFromTag(mScatterGraph, errors), ScatterGraphProperties)

            If mScatterGraphProperties IsNot Nothing Then
                scatterGraphPropertiesDic.Add(mScatterGraph.Name, mScatterGraphProperties)
                scatterGraphList.Add(mScatterGraph)
            End If
        End If
    End Sub

#End Region

#Region "ActionInitControl"
    ''' <summary>
    ''' Обработать Action для контрола из словаря указателей на функции.
    ''' Данный подход плозволяет исключить большие блоки Case и легко добавлять новые обработчики
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub ExecFuncInitControl(ByRef aControl As Control, ByVal errors As List(Of String)) 'Optional parametrs As Object = Nothing)
        If funcsActionInitControlDictionary.Keys.Contains(aControl.GetType.ToString) Then
            funcsActionInitControlDictionary(aControl.GetType.ToString).Invoke(aControl, errors)
        End If
    End Sub

    Private Sub ActionInitControlSwitch(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mSwitch = CType(aControl, WindowsForms.Switch)
        If switchList.Contains(mSwitch) Then
            MessageControl(aControl, errors)
        Else
            Dim mSwitchProperties As SwitchProperties = CType(GetControlBaseFromTag(mSwitch, errors), SwitchProperties)

            If mSwitchProperties IsNot Nothing Then
                switchPropertiesDic.Add(aControl.Name, mSwitchProperties)
                switchList.Add(mSwitch)
                AddHandler CType(mSwitch, WindowsForms.Switch).StateChanged, AddressOf Switch_StateChanged
            End If
        End If
    End Sub

    Private Sub ActionInitControlLed(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mLed = CType(aControl, Led)
        If ledList.Contains(mLed) Then
            MessageControl(aControl, errors)
        Else
            Dim mLedProperties As LedProperties = CType(GetControlBaseFromTag(mLed, errors), LedProperties)

            If mLedProperties IsNot Nothing Then
                ledPropertiesDic.Add(mLed.Name, mLedProperties)
                ledList.Add(mLed)
            End If
        End If
    End Sub

    Private Sub ActionInitControlKnob(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mKnob = CType(aControl, Knob)
        If knobList.Contains(mKnob) Then
            MessageControl(aControl, errors)
        Else
            Dim mKnobProperties As KnobProperties = CType(GetControlBaseFromTag(aControl, errors), KnobProperties)

            If mKnobProperties IsNot Nothing Then
                knobPropertiesDic.Add(mKnob.Name, mKnobProperties)
                InitializeKnob(mKnob, mKnobProperties)
                knobList.Add(mKnob)
            End If
        End If
    End Sub

    Private Sub ActionInitControlSlide(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mSlide = CType(aControl, Slide)
        If slideList.Contains(mSlide) Then
            MessageControl(aControl, errors)
        Else
            Dim mSlideProperties As SlideProperties = CType(GetControlBaseFromTag(mSlide, errors), SlideProperties)

            If mSlideProperties IsNot Nothing Then
                slidePropertiesDic.Add(mSlide.Name, mSlideProperties)
                InitializeSlide(mSlide, mSlideProperties)
                slideList.Add(mSlide)
            End If
        End If
    End Sub

    Private Sub ActionInitControlNumericEdit(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mNumericEdit = CType(aControl, NumericEdit)
        If numericEditList.Contains(mNumericEdit) Then
            MessageControl(aControl, errors)
        Else
            Dim mNumericEditProperties As NumericEditProperties = CType(GetControlBaseFromTag(mNumericEdit, errors), NumericEditProperties)

            If mNumericEditProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mNumericEditProperties, PropertiesControlChannelBase), errors) Then
                numericEditPropertiesDic.Add(mNumericEdit.Name, mNumericEditProperties)
                numericEditMyTypeDic.Add(mNumericEdit.Name, GetMyTypeFromArrParameters(mNumericEditProperties.ИмяКанала))
                'CType(aControl, WindowsForms.NumericEdit).OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange ' исключить ошибку переполнения
                numericEditList.Add(mNumericEdit)
            End If
        End If
    End Sub

    Private Sub ActionInitControlTank(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mTank = CType(aControl, Tank)
        If tankList.Contains(mTank) Then
            MessageControl(aControl, errors)
        Else
            Dim mTankProperties As TankProperties = CType(GetControlBaseFromTag(aControl, errors), TankProperties)

            If mTankProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mTankProperties, PropertiesControlChannelBase), errors) Then
                tankPropertiesDic.Add(mTank.Name, mTankProperties)
                tankMyTypeDic.Add(mTank.Name, GetMyTypeFromArrParameters(mTankProperties.ИмяКанала))
                InitializeTank(mTank, mTankProperties)
                tankList.Add(mTank)
            End If
        End If
    End Sub

    Private Sub ActionInitControlGauge(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mGauge = CType(aControl, Gauge)
        If gaugeList.Contains(mGauge) Then
            MessageControl(aControl, errors)
        Else
            Dim mGaugeProperties As GaugeProperties = CType(GetControlBaseFromTag(mGauge, errors), GaugeProperties)

            If mGaugeProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mGaugeProperties, PropertiesControlChannelBase), errors) Then
                gaugePropertiesDic.Add(mGauge.Name, mGaugeProperties)
                gaugeMyTypeDic.Add(mGauge.Name, GetMyTypeFromArrParameters(mGaugeProperties.ИмяКанала))
                InitializeGauge(mGauge, mGaugeProperties)
                gaugeList.Add(mGauge)
            End If
        End If
    End Sub

    Private Sub ActionInitControlThermometer(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mThermometer = CType(aControl, Thermometer)
        If thermometerList.Contains(mThermometer) Then
            MessageControl(aControl, errors)
        Else
            Dim mThermometerProperties As ThermometerProperties = CType(GetControlBaseFromTag(mThermometer, errors), ThermometerProperties)

            If mThermometerProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mThermometerProperties, PropertiesControlChannelBase), errors) Then
                thermometerPropertiesDic.Add(aControl.Name, mThermometerProperties)
                thermometerMyTypeDic.Add(mThermometer.Name, GetMyTypeFromArrParameters(mThermometerProperties.ИмяКанала))
                InitializeThermometer(mThermometer, mThermometerProperties)
                thermometerList.Add(mThermometer)
            End If
        End If
    End Sub

    Private Sub ActionInitControlMeter(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mMeter = CType(aControl, Meter)
        If meterList.Contains(mMeter) Then
            MessageControl(aControl, errors)
        Else
            Dim mMeterProperties As MeterProperties = CType(GetControlBaseFromTag(mMeter, errors), MeterProperties)

            If mMeterProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mMeterProperties, PropertiesControlChannelBase), errors) Then
                meterPropertiesDic.Add(mMeter.Name, mMeterProperties)
                meterMyTypeDic.Add(mMeter.Name, GetMyTypeFromArrParameters(mMeterProperties.ИмяКанала))
                InitializeMeter(mMeter, mMeterProperties)
                meterList.Add(mMeter)
            End If
        End If
    End Sub

    Private Sub ActionInitControlWaveformGraph(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mWaveformGraph = CType(aControl, WaveformGraph)
        If waveformGraphList.Contains(mWaveformGraph) Then
            MessageControl(aControl, errors)
        Else
            Dim mWaveformGraphProperties As WaveformGraphProperties = CType(GetControlBaseFromTag(mWaveformGraph, errors), WaveformGraphProperties)

            If mWaveformGraphProperties IsNot Nothing AndAlso FindIndexInArrayParameters(CType(mWaveformGraphProperties, PropertiesControlChannelBase), errors) Then
                waveformGraphPropertiesDic.Add(mWaveformGraph.Name, mWaveformGraphProperties)
                waveformGraphMyTypeDic.Add(mWaveformGraph.Name, GetMyTypeFromArrParameters(mWaveformGraphProperties.ИмяКанала))
                InitializeWaveformGraph(mWaveformGraph, mWaveformGraphProperties)
                waveformGraphList.Add(mWaveformGraph)
            End If
        End If
    End Sub

    Private Sub ActionInitControlScatterGraph(ByVal aControl As Control, ByVal errors As List(Of String))
        Dim mScatterGraph = CType(aControl, ScatterGraph)
        If scatterGraphList.Contains(mScatterGraph) Then
            MessageControl(aControl, errors)
        Else
            Dim mScatterGraphProperties As ScatterGraphProperties = CType(GetControlBaseFromTag(aControl, errors), ScatterGraphProperties)

            If mScatterGraphProperties IsNot Nothing AndAlso FindIndexInArrayParameters(mScatterGraphProperties, errors) Then
                scatterGraphPropertiesDic.Add(mScatterGraph.Name, mScatterGraphProperties)
                scatterGraphMyTypeDic.Add(mScatterGraph.Name & AXES_X, GetMyTypeFromArrParameters(mScatterGraphProperties.ИмяКаналаОсиХ))
                scatterGraphMyTypeDic.Add(mScatterGraph.Name & AXES_Y, GetMyTypeFromArrParameters(mScatterGraphProperties.ИмяКаналаОсиУ))
                InitializeScatterGraph(mScatterGraph, mScatterGraphProperties)
                scatterGraphList.Add(mScatterGraph)
            End If
        End If
    End Sub

#End Region

#Region "ActionAddLabel"
    ''' <summary>
    ''' Добавить метку к контролу связанному с переменной
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub AddLabelName(ByVal aControl As Control, text As String, color As Color)
        'LabelName.Size = New System.Drawing.Size(194, 13)
        Dim labelName As New Label With {
            .BackColor = color, 'System.Drawing.SystemColors.Control 'System.Drawing.Color.Transparent
            .ForeColor = Color.Blue,
            .Location = New Point(aControl.Location.X, aControl.Location.Y - 13), ' LabelName.Size.Height)
            .Name = "LabelName" & aControl.Name,
            .AutoSize = True,
            .Text = text
        }

        aControl.Parent.Controls.Add(labelName)
        labelName.BringToFront()
    End Sub

    ''' <summary>
    ''' Обработать Action для контрола из словаря указателей на функции.
    ''' Данный подход плозволяет исключить большие блоки Case и легко добавлять новые обработчики
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <remarks></remarks>
    Private Sub ExecFuncAddLabel(ByVal aControl As Control) 'Optional parametrs As Object = Nothing)
        If funcsActionAddLabelDictionary.Keys.Contains(aControl.GetType.ToString) Then
            funcsActionAddLabelDictionary(aControl.GetType.ToString).Invoke(aControl)
        End If
    End Sub

    Private Sub ActionAddLabelSwitch(ByVal aControl As Control)
        Dim defaultColor As Color = SystemColors.Control

        If Not captionAddedControls.Contains(aControl.Name) Then
            If switchPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mSwitchProperties As SwitchProperties = switchPropertiesDic(aControl.Name)
                Dim lines As String

                If mSwitchProperties.НомерМодуляКорзины = "" Then
                    ' плата
                    lines = $"Dev{mSwitchProperties.НомерУстройства}/port{mSwitchProperties.НомерПорта}/line{mSwitchProperties.НомерБита}"
                Else ' модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    lines = $"SC{mSwitchProperties.НомерУстройства}Mod{mSwitchProperties.НомерМодуляКорзины}/port{mSwitchProperties.НомерПорта}/line{mSwitchProperties.НомерБита}"
                End If

                AddLabelName(aControl, lines, defaultColor)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelLed(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If ledPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mLedProperties As LedProperties = ledPropertiesDic(aControl.Name)
                Dim namePort As String

                If mLedProperties.НомерМодуляКорзины = "" Then
                    ' плата
                    namePort = $"Dev{mLedProperties.НомерУстройства}/port{mLedProperties.НомерПорта}/line{mLedProperties.НомерБита}"
                    'DeviceName = "Dev" & mLedProperties.НомерУстройства
                Else ' модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    namePort = $"SC{mLedProperties.НомерУстройства}Mod{mLedProperties.НомерМодуляКорзины}/port{mLedProperties.НомерПорта}/line{mLedProperties.НомерБита}"
                    'DeviceName = "SC" & mLedProperties.НомерУстройства & "Mod" & mLedProperties.НомерМодуляКорзины
                End If

                AddLabelName(aControl, namePort, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelKnob(ByVal aControl As Control)
        Dim defaultColor As Color = SystemColors.Control

        If Not captionAddedControls.Contains(aControl.Name) Then
            If knobPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mKnobProperties As KnobProperties = knobPropertiesDic(aControl.Name)
                AddLabelName(aControl, $"Dev{mKnobProperties.НомерУстройства}/ao{mKnobProperties.НомерКанала}", defaultColor)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelSlide(ByVal aControl As Control)
        Dim defaultColor As Color = SystemColors.Control

        If Not captionAddedControls.Contains(aControl.Name) Then
            If slidePropertiesDic.ContainsKey(aControl.Name) Then
                Dim mSlideProperties As SlideProperties = slidePropertiesDic(aControl.Name)
                AddLabelName(aControl, $"Dev{mSlideProperties.НомерУстройства}/ao{mSlideProperties.НомерКанала}", defaultColor)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelNumericEdit(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If numericEditPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mNumericEditProperties As NumericEditProperties = numericEditPropertiesDic(aControl.Name)
                AddLabelName(aControl, mNumericEditProperties.ИмяКанала, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelTank(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If tankPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mTankProperties As TankProperties = tankPropertiesDic(aControl.Name)
                AddLabelName(aControl, mTankProperties.ИмяКанала, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelGauge(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If gaugePropertiesDic.ContainsKey(aControl.Name) Then
                Dim mGaugeProperties As GaugeProperties = gaugePropertiesDic(aControl.Name)
                AddLabelName(aControl, mGaugeProperties.ИмяКанала, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelThermometer(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If thermometerPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mThermometerProperties As ThermometerProperties = thermometerPropertiesDic(aControl.Name)
                AddLabelName(aControl, mThermometerProperties.ИмяКанала, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

    Private Sub ActionAddLabelMeter(ByVal aControl As Control)
        If Not captionAddedControls.Contains(aControl.Name) Then
            If meterPropertiesDic.ContainsKey(aControl.Name) Then
                Dim mMeterProperties As MeterProperties = meterPropertiesDic(aControl.Name)
                AddLabelName(aControl, mMeterProperties.ИмяКанала, SystemColors.Control)
                captionAddedControls.Add(aControl.Name)
                isNoAddedCaption = False
            End If
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Составить Список Вложенный Контрол
    ''' Нахождение только связанных с управлением контролов.
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub PrepareListChildControls(ByVal aControl As Control, ByVal errors As List(Of String))
        Try
            ExecFuncPutToList(aControl, errors)
        Catch ex As Exception
            errors.Add($"Процедура <{NameOf(PrepareListChildControls)}>:{Environment.NewLine}
                        Контрол типа: {aControl.GetType}{Environment.NewLine}
                        вызвал исключение: {ex}")
        End Try

        ' проверить содержит ли контрол внутри себя
        If aControl.Controls.Count <> 0 Then
            For Each ctlLoop As Control In aControl.Controls
                PrepareListChildControls(ctlLoop, errors)
            Next ctlLoop
        End If
    End Sub

    ''' <summary>
    ''' Инициализировать Вложенный Контрол
    ''' aControl передаётся по ссылке, а значит ему может быть присвоен новый тип.
    ''' Нахождение только связанных с управлением контролов.
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub InitializeChildControls(ByRef aControl As Control, ByVal errors As List(Of String))
        Try
            ExecFuncInitControl(aControl, errors)

            ' другие контролы для настройки
            'ElseIf typeControl Is GetType(System.Windows.Forms.TextBox) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.Button) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.CheckBox) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.RadioButton) Then

            'ElseIf typeControl Is GetType(System.Windows.Forms.Panel) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.GroupBox) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.TabControl) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.TabPage) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.TableLayoutPanel) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.FlowLayoutPanel) Then
            'ElseIf typeControl Is GetType(System.Windows.Forms.PictureBox) Then

            ' не доступные для настройки
            'ElseIf typeControl Is GetType(WaveformPlot) Then
            'ElseIf typeControl Is GetType(ScatterPlot) Then
            'ElseIf typeControl Is GetType(XAxis) Then
            'ElseIf typeControl Is GetType(YAxis) Then

            'End If

            ' создать контрол который используется во всех последующих циклах
            'Dim aControl As Control
            'Dim aGroupControl As Control
            'Dim anObject As Object

            '' сбросить строковое описание
            'Me.surveyResponseValue = ""

            ' Цикл по каждому контролу ы выдать пользователю строку описания
            ' SurveyResponse. (Строка может лекго быть размещена в сортируемой коллекции.)

            ' определить выходной базовый тип контрола
            '    Select Case TypeName(aControl)
            '        Case "ComboBox"
            '            ' сгруппировать информацию в строку
            '            surveyResponseValue += aControl.Name + " - "
            '            surveyResponseValue += aControl.Text
            '            surveyResponseValue += vbCrLf
            '        Case "TextBox"
            '            ' сгруппировать информацию в строку
            '            surveyResponseValue += aControl.Name + " - "
            '            surveyResponseValue += aControl.Text
            '            surveyResponseValue += vbCrLf
            '        Case "GroupBox"
            '            ' Необходимо пройти внутрь GroupBox и установить настройки 
            '            '   RadioButtons
            '            For Each aGroupControl In CType(aControl, GroupBox).Controls
            '                If TypeOf aGroupControl Is RadioButton Then
            '                    If CType(aGroupControl, RadioButton).Checked Then
            '                        ' сгруппировать информацию в строку
            '                        surveyResponseValue += aControl.Name + " - "
            '                        surveyResponseValue += aGroupControl.Text
            '                        surveyResponseValue += vbCrLf
            '                    End If
            '                End If
            '            Next
            '        Case "ListBox"
            '            ' Здесь получить каждую выделенную линию, и вернуть их
            '            surveyResponseValue += aControl.Name + " - "
            '            For Each anObject In CType(aControl, ListBox).SelectedItems
            '                If TypeOf anObject Is String Then
            '                    ' сгруппировать информацию в строку
            '                    surveyResponseValue += vbTab + CStr(anObject)
            '                    surveyResponseValue += vbCrLf
            '                End If
            '            Next
            '    End Select

        Catch ex As Exception
            errors.Add($"Процедура <{NameOf(InitializeChildControls)}>:{Environment.NewLine}Контрол типа: {aControl.GetType}{Environment.NewLine}вызвал исключение: {ex}")
        End Try

        ' проверить содержит ли контрол внутри себя
        If aControl.Controls.Count <> 0 Then
            For Each ctlLoop As Control In aControl.Controls
                InitializeChildControls(ctlLoop, errors)
            Next ctlLoop
        End If
    End Sub

    ''' <summary>
    ''' Добавить Надписи Вложенный Контрол
    ''' Добавить метку к контролу и поместить его в список обработанных элементов
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <remarks></remarks>
    Private Sub AddCaptionChildControls(ByVal aControl As Control)
        ExecFuncAddLabel(aControl)

        ' проверить содержит ли контрол внутри себя
        If aControl.Controls.Count <> 0 Then
            For Each ctlLoop As Control In aControl.Controls
                AddCaptionChildControls(ctlLoop)
            Next ctlLoop
        End If
    End Sub

    ''' <summary>
    ''' Добавить строку с ошибкой
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub MessageControl(ByRef aControl As Control, ByVal errors As List(Of String))
        'MessageBox.Show("Контрол " & aControl.Name & " уже содержится в коллекции!", "Проверка коллекции", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        errors.Add($"Контрол {aControl.Name} уже содержится в коллекции!")
    End Sub

#End Region

#Region "InitializeControl"

    Private Sub CheckRange(nameControl As String,
                           workRangeMinimum As Double,
                           workRangeMaximum As Double,
                           alertLevel As Double,
                           alarmLevel As Double)

        Dim messageError As String = $"Контрол <{nameControl}> неправильно настроен.{Environment.NewLine}"
        Dim isError As Boolean

        If workRangeMinimum >= workRangeMaximum Then
            ' Control.Range
            isError = True
            messageError &= String.Format("рабочий диапазон минимум {0} должен быть меньше рабочего диапазона максимум {1}", workRangeMinimum, workRangeMaximum) & Environment.NewLine
        ElseIf workRangeMinimum >= alertLevel Then
            ' ScaleRangeFill1
            isError = True
            messageError &= String.Format("рабочий диапазон минимум {0} должен быть меньше нижнего аварийного уровня {1}", workRangeMinimum, alertLevel) & Environment.NewLine
        ElseIf alertLevel >= alarmLevel Then
            ' ScaleRangeFill2
            isError = True
            messageError &= String.Format("нижний аварийный уровнь {0} должен быть меньше верхнего аварийного уровня {1}", alertLevel, alarmLevel) & Environment.NewLine
        ElseIf alarmLevel >= workRangeMaximum Then
            ' ScaleRangeFill3
            isError = True
            messageError &= String.Format("верхний аварийный уровнь {0} должен быть меньше рабочего диапазона максимум {1}", alarmLevel, workRangeMaximum) & Environment.NewLine
        End If

        If isError Then Throw New MyException(messageError)
    End Sub

    Private Sub CheckRangeGraph(nameGraph As String,
                                workRangeMinimum As Double,
                                workRangeMaximum As Double)

        Dim messageError As String = $"График <{nameGraph}> неправильно настроен.{Environment.NewLine}"
        Dim isError As Boolean

        If workRangeMinimum >= workRangeMaximum Then
            isError = True
            messageError &= String.Format("рабочий диапазон минимум {0} должен быть меньше рабочего диапазона максимум {1}", workRangeMinimum, workRangeMaximum) & Environment.NewLine
        End If

        If isError Then Throw New MyException(messageError)
    End Sub

    Private Sub InitializeSlide(ByRef SlideControl As Slide, mSlideProperties As SlideProperties)
        Dim nameControl As String = mSlideProperties.Name
        Dim alertLevel As Double = mSlideProperties.УправлениеМин + 1
        Dim alarmLevel As Double = mSlideProperties.УправлениеМакс - 1

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(nameControl, mSlideProperties.УправлениеМин, mSlideProperties.УправлениеМакс, alertLevel, alarmLevel)
        Dim рабочийДиапазон As Range = New Range(mSlideProperties.УправлениеМин, mSlideProperties.УправлениеМакс)
        CType(SlideControl, ComponentModel.ISupportInitialize).BeginInit()

        'SlideControl.Border = NationalInstruments.UI.Border.Raised
        'SlideControl.Dock = System.Windows.Forms.DockStyle.Fill
        SlideControl.FillColor = Color.Lime
        SlideControl.FillMode = NumericFillMode.ToBaseValue
        'SlideControl.Location = New System.Drawing.Point(416, 3)
        'SlideControl.Name = "SlideControl"
        SlideControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange ' исключить ошибку переполнения

        'SlideControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        SlideControl.Value = 0
        SlideControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        SlideControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'SlideControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'SlideControl.Size = New System.Drawing.Size(244, 98)
        'SlideControl.TabIndex = 3
        'SlideControl.SlideStyle = NationalInstruments.UI.SlideStyle.Raised3D
        'ToolTip1.SetToolTip(SlideControl, "Индикатор значения переменной")
        'SlideControl.Value = 50.0R

        CType(SlideControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeKnob(ByRef KnobControl As Knob, mKnobProperties As KnobProperties)
        Dim nameControl As String = mKnobProperties.Name
        Dim alertLevel As Double = mKnobProperties.ЧастотаМин + 1
        Dim alarmLevel As Double = mKnobProperties.ЧастотаМакс - 1

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(nameControl, mKnobProperties.ЧастотаМин, mKnobProperties.ЧастотаМакс, alertLevel, alarmLevel)
        Dim рабочийДиапазон As Range = New Range(mKnobProperties.ЧастотаМин, mKnobProperties.ЧастотаМакс)

        CType(KnobControl, ComponentModel.ISupportInitialize).BeginInit()

        'KnobControl.Border = NationalInstruments.UI.Border.Raised
        'KnobControl.Dock = System.Windows.Forms.DockStyle.Fill
        'KnobControl.FillColor = System.Drawing.Color.Lime
        'KnobControl.FillMode = NationalInstruments.UI.NumericFillMode.ToBaseValue
        'KnobControl.Location = New System.Drawing.Point(416, 3)
        'KnobControl.Name = "KnobControl"
        KnobControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

        'KnobControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        KnobControl.Value = 0 'netVar.ValueDouble
        KnobControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        KnobControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'KnobControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'KnobControl.Size = New System.Drawing.Size(244, 98)
        'KnobControl.TabIndex = 3
        'KnobControl.KnobStyle = NationalInstruments.UI.KnobStyle.Raised3D
        'ToolTip1.SetToolTip(KnobControl, "Индикатор значения переменной")
        'KnobControl.Value = 50.0R

        CType(KnobControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeTank(ByRef TankControl As Tank, mTankProperties As TankProperties)
        Dim параметр As TypeBaseParameterTCP = tankMyTypeDic(TankControl.Name)
        Dim alertLevel As Double = параметр.AlarmValueMin
        Dim alarmLevel As Double = параметр.AlarmValueMax

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(mTankProperties.Name, параметр.LowerLimit, параметр.UpperLimit, alertLevel, alarmLevel)
        Dim рабочийДиапазон As New Range(параметр.LowerLimit, параметр.UpperLimit)

        CType(TankControl, ComponentModel.ISupportInitialize).BeginInit()

        'TankControl.Border = NationalInstruments.UI.Border.Raised
        'TankControl.Dock = System.Windows.Forms.DockStyle.Fill
        TankControl.FillColor = Color.Lime
        TankControl.FillMode = NumericFillMode.ToBaseValue
        'TankControl.Location = New System.Drawing.Point(416, 3)
        'TankControl.Name = "TankControl"
        TankControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

        'TankControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        TankControl.Value = 0
        TankControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        TankControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'TankControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'TankControl.Size = New System.Drawing.Size(244, 98)
        'TankControl.TabIndex = 3
        'TankControl.TankStyle = NationalInstruments.UI.TankStyle.Raised3D
        'ToolTip1.SetToolTip(TankControl, "Индикатор значения переменной")
        'TankControl.Value = 50.0R

        CType(TankControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeGauge(ByRef GaugeControl As Gauge, mGaugeProperties As GaugeProperties)
        Dim параметр As TypeBaseParameterTCP = gaugeMyTypeDic(GaugeControl.Name)
        Dim alertLevel As Double = параметр.AlarmValueMin
        Dim alarmLevel As Double = параметр.AlarmValueMax

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(mGaugeProperties.Name, параметр.LowerLimit, параметр.UpperLimit, alertLevel, alarmLevel)
        Dim рабочийДиапазон As New Range(параметр.LowerLimit, параметр.UpperLimit)

        CType(GaugeControl, ComponentModel.ISupportInitialize).BeginInit()

        'GaugeControl.Border = NationalInstruments.UI.Border.Raised
        'GaugeControl.Dock = System.Windows.Forms.DockStyle.Fill
        'GaugeControl.FillColor = System.Drawing.Color.Lime
        'GaugeControl.FillMode = NationalInstruments.UI.NumericFillMode.ToBaseValue
        'GaugeControl.Location = New System.Drawing.Point(416, 3)
        'GaugeControl.Name = "GaugeControl"
        GaugeControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

        'GaugeControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        GaugeControl.Value = 0
        GaugeControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        GaugeControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'GaugeControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'GaugeControl.Size = New System.Drawing.Size(244, 98)
        'GaugeControl.TabIndex = 3
        'GaugeControl.TankStyle = NationalInstruments.UI.TankStyle.Raised3D
        'ToolTip1.SetToolTip(GaugeControl, "Индикатор значения переменной")
        'GaugeControl.Value = 50.0R

        CType(GaugeControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeThermometer(ByRef ThermometerControl As Thermometer, mThermometerProperties As ThermometerProperties)
        Dim параметр As TypeBaseParameterTCP = thermometerMyTypeDic(ThermometerControl.Name)
        Dim alertLevel As Double = параметр.AlarmValueMin
        Dim alarmLevel As Double = параметр.AlarmValueMax

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(mThermometerProperties.Name, параметр.LowerLimit, параметр.UpperLimit, alertLevel, alarmLevel)
        Dim рабочийДиапазон As New Range(параметр.LowerLimit, параметр.UpperLimit)

        CType(ThermometerControl, ComponentModel.ISupportInitialize).BeginInit()

        'ThermometerControl.Border = NationalInstruments.UI.Border.Raised
        'ThermometerControl.Dock = System.Windows.Forms.DockStyle.Fill
        ThermometerControl.FillColor = Color.Lime
        ThermometerControl.FillMode = NumericFillMode.ToBaseValue
        'ThermometerControl.Location = New System.Drawing.Point(416, 3)
        'ThermometerControl.Name = "ThermometerControl"
        ThermometerControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

        'ThermometerControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        ThermometerControl.Value = 0
        ThermometerControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        ThermometerControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'ThermometerControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'ThermometerControl.Size = New System.Drawing.Size(244, 98)
        'ThermometerControl.TabIndex = 3
        'ThermometerControl.TankStyle = NationalInstruments.UI.TankStyle.Raised3D
        'ToolTip1.SetToolTip(ThermometerControl, "Индикатор значения переменной")
        'ThermometerControl.Value = 50.0R

        CType(ThermometerControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeMeter(ByRef MeterControl As Meter, mMeterProperties As MeterProperties)
        Dim параметр As TypeBaseParameterTCP = meterMyTypeDic(MeterControl.Name)
        Dim alertLevel As Double = параметр.AlarmValueMin
        Dim alarmLevel As Double = параметр.AlarmValueMax

        ' настраивается после присваивания других свойств от которых зависит РабочийДиапазон
        CheckRange(mMeterProperties.Name, параметр.LowerLimit, параметр.UpperLimit, alertLevel, alarmLevel)
        Dim рабочийДиапазон As New Range(параметр.LowerLimit, параметр.UpperLimit)

        CType(MeterControl, ComponentModel.ISupportInitialize).BeginInit()

        'MeterControl.Border = NationalInstruments.UI.Border.Raised
        'MeterControl.Dock = System.Windows.Forms.DockStyle.Fill
        'MeterControl.FillColor = System.Drawing.Color.Lime
        'MeterControl.FillMode = NationalInstruments.UI.NumericFillMode.ToBaseValue
        'MeterControl.Location = New System.Drawing.Point(416, 3)
        'MeterControl.Name = "MeterControl"
        MeterControl.OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

        'MeterControl.Range = New NationalInstruments.UI.Range(0.0R, 100.0R)

        MeterControl.Value = 0
        MeterControl.Range = New Range(рабочийДиапазон.Minimum, рабочийДиапазон.Maximum)

        Dim ScaleRangeFill1 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill2 As ScaleRangeFill = New ScaleRangeFill()
        Dim ScaleRangeFill3 As ScaleRangeFill = New ScaleRangeFill()

        ScaleRangeFill1.Range = New Range(рабочийДиапазон.Minimum, alertLevel)
        ScaleRangeFill1.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Orange) '(Color.DarkGreen)

        ScaleRangeFill2.Range = New Range(alertLevel, alarmLevel)
        ScaleRangeFill2.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.DarkGreen) '(Color.Yellow)

        ScaleRangeFill3.Range = New Range(alarmLevel, рабочийДиапазон.Maximum)
        ScaleRangeFill3.Style = ScaleRangeFillStyle.CreateSolidStyle(Color.Red)

        MeterControl.RangeFills.AddRange(New ScaleRangeFill() {ScaleRangeFill1, ScaleRangeFill2, ScaleRangeFill3})

        'MeterControl.ScalePosition = NationalInstruments.UI.NumericScalePosition.Bottom
        'MeterControl.Size = New System.Drawing.Size(244, 98)
        'MeterControl.TabIndex = 3
        'MeterControl.TankStyle = NationalInstruments.UI.TankStyle.Raised3D
        'ToolTip1.SetToolTip(MeterControl, "Индикатор значения переменной")
        'MeterControl.Value = 50.0R

        CType(MeterControl, ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub InitializeWaveformGraph(ByRef WaveformGraphControl As WaveformGraph, mWaveformGraphProperties As WaveformGraphProperties)
        Dim mWaveformGraph As WaveformGraph = CType(WaveformGraphControl, WaveformGraph)
        Dim XAxis1 As XAxis = mWaveformGraph.XAxes(0)
        Dim YAxis1 As YAxis = mWaveformGraph.YAxes(0)
        Dim historyPlot As WaveformPlot = mWaveformGraph.Plots(0)
        Dim dataCursor As New XYCursor

        mWaveformGraph.InteractionMode = CType((((((GraphInteractionModes.ZoomX Or GraphInteractionModes.ZoomY) _
            Or GraphInteractionModes.ZoomAroundPoint) _
            Or GraphInteractionModes.PanX) _
            Or GraphInteractionModes.PanY) _
            Or GraphInteractionModes.DragAnnotationCaption), GraphInteractionModes)

        mWaveformGraph.Cursors.Clear()
        '
        'dataCursor
        '
        dataCursor.Color = Color.Gold
        If mWaveformGraphProperties.ТипГрафика Then ' горизонтально
            dataCursor.LabelDisplay = XYCursorLabelDisplay.ShowY
        Else
            dataCursor.LabelDisplay = XYCursorLabelDisplay.ShowX
        End If

        dataCursor.LabelVisible = True
        dataCursor.LineStyle = LineStyle.None
        dataCursor.Plot = historyPlot
        dataCursor.PointStyle = PointStyle.SolidCircle
        'dataCursor.SnapMode = CursorSnapMode.ToPlot
        'dataCursor.LabelAlignment = PointAlignment.MiddleLeft

        mWaveformGraph.Cursors.AddRange(New XYCursor() {dataCursor})
        '
        'historyPlot
        '
        historyPlot.HistoryCapacity = 101
        'historyPlot.LineColor = System.Drawing.Color.White
        'historyPlot.LineWidth = 2.0!
        historyPlot.ToolTipsEnabled = True
        historyPlot.XAxis = XAxis1
        historyPlot.YAxis = YAxis1
        '
        'XAxis1
        '
        XAxis1.AutoMinorDivisionFrequency = 4
        XAxis1.MajorDivisions.GridVisible = True
        XAxis1.MinorDivisions.GridVisible = True
        '
        'YAxis1
        '
        YAxis1.AutoMinorDivisionFrequency = 4
        YAxis1.MajorDivisions.GridVisible = True
        YAxis1.MinorDivisions.GridVisible = True

        If mWaveformGraphProperties.ТипГрафика Then ' горизонтально
            If mWaveformGraphProperties.Прокрутка Then
                XAxis1.Mode = AxisMode.StripChart
            Else
                XAxis1.Mode = AxisMode.ScopeChart
            End If

            XAxis1.Range = New Range(0, 100)
            YAxis1.Caption = mWaveformGraphProperties.ИмяКанала
            'YAxis1.Mode = AxisMode.AutoScaleLoose
            YAxis1.Mode = AxisMode.Fixed

            CheckRangeGraph(mWaveformGraphProperties.Name, waveformGraphMyTypeDic(WaveformGraphControl.Name).LowerLimit, waveformGraphMyTypeDic(WaveformGraphControl.Name).UpperLimit)
            'YAxis1.Range = New Range(mWaveformGraphProperties.ЗначениеОсиYМин, mWaveformGraphProperties.ЗначениеОсиYМакс)
            ' надо брать из свойств arrПараметры
            YAxis1.Range = New Range(waveformGraphMyTypeDic(WaveformGraphControl.Name).LowerLimit, waveformGraphMyTypeDic(WaveformGraphControl.Name).UpperLimit)
        Else ' вертикально
            If mWaveformGraphProperties.Прокрутка Then
                YAxis1.Mode = AxisMode.StripChart
            Else
                YAxis1.Mode = AxisMode.ScopeChart
            End If

            XAxis1.Mode = AxisMode.Fixed

            CheckRangeGraph(mWaveformGraphProperties.Name, waveformGraphMyTypeDic(WaveformGraphControl.Name).LowerLimit, waveformGraphMyTypeDic(WaveformGraphControl.Name).UpperLimit)
            'XAxis1.Range = New Range(mWaveformGraphProperties.ЗначениеОсиYМин, mWaveformGraphProperties.ЗначениеОсиYМакс)
            ' надо брать из свойств arrПараметры
            XAxis1.Range = New Range(waveformGraphMyTypeDic(WaveformGraphControl.Name).LowerLimit, waveformGraphMyTypeDic(WaveformGraphControl.Name).UpperLimit)

            XAxis1.Caption = mWaveformGraphProperties.ИмяКанала
            YAxis1.Range = New Range(0, 100)
        End If
    End Sub

    Private Sub InitializeScatterGraph(ByRef ScatterGraphControl As ScatterGraph, mScatterGraphProperties As ScatterGraphProperties)
        Dim mScatterGraph As ScatterGraph = CType(ScatterGraphControl, ScatterGraph)
        Dim XAxis1 As XAxis = mScatterGraph.XAxes(0)
        Dim YAxis1 As YAxis = mScatterGraph.YAxes(0)
        Dim ScatterPlot1 As ScatterPlot = mScatterGraph.Plots(0)
        Dim dataCursor As New XYCursor

        mScatterGraph.InteractionMode = CType((((((GraphInteractionModes.ZoomX Or GraphInteractionModes.ZoomY) _
            Or GraphInteractionModes.ZoomAroundPoint) _
            Or GraphInteractionModes.PanX) _
            Or GraphInteractionModes.PanY) _
            Or GraphInteractionModes.DragAnnotationCaption), GraphInteractionModes)

        mScatterGraph.Cursors.Clear()
        '
        'dataCursor
        '
        dataCursor.Color = Color.Gold
        'dataCursor.LabelDisplay = XYCursorLabelDisplay.ShowY
        dataCursor.LabelVisible = True
        dataCursor.LineStyle = LineStyle.None
        dataCursor.Plot = ScatterPlot1
        dataCursor.PointStyle = PointStyle.SolidCircle
        'dataCursor.SnapMode = CursorSnapMode.ToPlot

        mScatterGraph.Cursors.AddRange(New XYCursor() {dataCursor})

        ScatterPlot1.XAxis = XAxis1
        ScatterPlot1.YAxis = YAxis1

        ScatterPlot1.LineColor = Color.White
        ScatterPlot1.PointColor = Color.Red
        ScatterPlot1.PointStyle = PointStyle.SolidDiamond
        '
        'XAxis1
        '
        XAxis1.Caption = mScatterGraphProperties.ИмяКаналаОсиХ '"Параметр " &
        XAxis1.MajorDivisions.GridVisible = True
        XAxis1.MinorDivisions.GridVisible = True
        XAxis1.Mode = AxisMode.Fixed 'AutoScaleLoose 

        CheckRangeGraph(mScatterGraphProperties.Name, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_X).LowerLimit, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_X).UpperLimit)

        'XAxis1.Range = New Range(mScatterGraphProperties.ЗначениеОсиХМин, mScatterGraphProperties.ЗначениеОсиХМакс)
        ' надо брать из свойств arrПараметры
        XAxis1.Range = New Range(scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_X).LowerLimit, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_X).UpperLimit)
        '
        'YAxis1
        '
        YAxis1.Caption = mScatterGraphProperties.ИмяКаналаОсиУ '"Параметр " & 
        YAxis1.MajorDivisions.GridVisible = True
        YAxis1.MinorDivisions.GridVisible = True
        YAxis1.Mode = AxisMode.Fixed 'AutoScaleLoose 

        CheckRangeGraph(mScatterGraphProperties.Name, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_Y).LowerLimit, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_Y).UpperLimit)
        'YAxis1.Range = New Range(mScatterGraphProperties.ЗначениеОсиYМин, mScatterGraphProperties.ЗначениеОсиYМакс)
        ' надо брать из свойств arrПараметры
        YAxis1.Range = New Range(scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_Y).LowerLimit, scatterGraphMyTypeDic(ScatterGraphControl.Name & AXES_Y).UpperLimit)
    End Sub

#End Region

#Region "soap serializer"
    ''' <summary>
    ''' Записать текстовое представление объекта PropertiesControlBase из свойства aControl.Tag созданного контрола.
    ''' Это базовый класс для всех индивидуальных настроек контролов отображаемых в PropertyGrid.
    ''' Затем десериализовать его в Hashtable и преобразовать в базовый класс настроек.
    ''' </summary>
    ''' <param name="aControl"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetControlBaseFromTag(ByVal aControl As Control, ByVal errors As List(Of String)) As PropertiesControlBase
        Dim propControlBase As PropertiesControlBase = Nothing

        If aControl.Tag Is Nothing OrElse aControl.Tag.ToString = "" Then
            errors.Add(String.Format(КОНТРОЛ_ИМЕЕТ_ПУСТОЙ_TAG, aControl.Name))
            Return Nothing
        Else
            Try
                ' записать строку в файл
                Using sw As StreamWriter = New StreamWriter(MainModule.PathTempFileSoap)
                    sw.Write(aControl.Tag)
                    'sw.Close() FxCop
                End Using

                ' считать с преобразованием
                Dim hashtableControlBase As Object = DeserializePropertiesControlBaseFromXML(MainModule.PathTempFileSoap, errors)
                If hashtableControlBase IsNot Nothing Then
                    propControlBase = DirectCast(hashtableControlBase, PropertiesControlBase)
                End If
            Catch ex As Exception
                errors.Add(ex.Message)
                Return Nothing
            End Try

            Return propControlBase
        End If
    End Function

    ''' <summary>
    ''' Десериализовать XML представление базового класса PropertiesControlBase
    ''' </summary>
    ''' <param name="fileXML"></param>
    ''' <param name="errors"></param>
    ''' <returns>базовый класс PropertiesControlBase</returns>
    ''' <remarks></remarks>
    Private Function DeserializePropertiesControlBaseFromXML(ByVal fileXML As String, ByVal errors As List(Of String)) As Object
        Dim sf As New SoapFormatter() ' Создать soap сериализатор.
        Dim objPropertiesControlBase As Object

        ' перезагрузить файл содержимого, используя тот же SoapFormatter объект.
        Try
            Using fs As New FileStream(fileXML, FileMode.Open)
                objPropertiesControlBase = sf.Deserialize(fs) ' десериализовать в PropertiesControlBase
            End Using
        Catch ex As Exception
            errors.Add(ex.Message)
            Return Nothing
        End Try

        Return objPropertiesControlBase
    End Function

#End Region

#Region "НайтиИндексВМассивеПараметров"
    ''' <summary>
    ''' Найти Индекс В Массиве Параметров
    ''' перегруженная версии для контрола
    ''' </summary>
    ''' <param name="mPropertiesControlBase"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    Private Function FindIndexInArrayParameters(ByRef mPropertiesControlBase As PropertiesControlChannelBase, ByVal errors As List(Of String)) As Boolean
        Dim indexParam As Integer = Array.IndexOf(gFormsPanelManager.NamesParametersForControl, mPropertiesControlBase.ИмяКанала)
        'strИменаПараметровДляКонтрола(0) = conПараметрОтсутствует
        If indexParam = -1 OrElse indexParam = 0 Then
            ' [Enum].Parse(GetType(EnumControl), mPropertiesControlBase.Тип)
            errors.Add(String.Format(ДЛЯ_КОНТРОЛА_ЗАДАН_НЕВЕРНЫЙ_ПАРАМЕТР,
                                     mPropertiesControlBase.Name,
                                     mPropertiesControlBase.Тип,
                                     mPropertiesControlBase.ИмяКанала,
                                     Environment.NewLine))
            Return False
        Else
            ' индекс в массиве arrСписокПараметровКонтроля собранных значение находится не в strИменаПараметровДляКонтрола
            mPropertiesControlBase.ИндексВМассивеПараметров = IndexParametersForControl(indexParam)
            Return True
        End If
    End Function

    ''' <summary>
    ''' Найти Индекс В Массиве Параметров
    ''' перегруженная версия для двух параметров ScatterGraph
    ''' </summary>
    ''' <param name="mPropertiesControlBase"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FindIndexInArrayParameters(ByVal mPropertiesControlBase As ScatterGraphProperties, ByVal errors As List(Of String)) As Boolean
        Dim isParameterAxisXFound As Boolean = False ' параметр Оси Х Найден
        Dim isParameterAxisYFound As Boolean = False ' параметр Оси У Найден

        Dim indexParamX As Integer = Array.IndexOf(gFormsPanelManager.NamesParametersForControl, mPropertiesControlBase.ИмяКаналаОсиХ)
        If indexParamX = -1 OrElse indexParamX = 0 Then
            errors.Add(String.Format(ДЛЯ_КОНТРОЛА_ЗАДАН_НЕВЕРНЫЙ_ПАРАМЕТР,
                                         mPropertiesControlBase.Name,
                                         mPropertiesControlBase.Тип,
                                         mPropertiesControlBase.ИмяКаналаОсиХ,
                                         vbCrLf))
            'Me._tableLP.GrowStyle = DirectCast([Enum].Parse(GetType(TableLayoutPanelGrowStyle), Me._combGrowStyle.SelectedItem.ToString()), TableLayoutPanelGrowStyle)
            '[Enum].Parse(GetType(EnumControl), mPropertiesControlBase.Тип)
        Else
            mPropertiesControlBase.ИндексКаналаОсиХВМассивеПараметров = IndexParametersForControl(indexParamX) 'ИмяКаналаОсиХ
            isParameterAxisXFound = True
        End If

        Dim indexParamY As Integer = Array.IndexOf(gFormsPanelManager.NamesParametersForControl, mPropertiesControlBase.ИмяКаналаОсиУ)
        If indexParamY = -1 OrElse indexParamY = 0 Then
            errors.Add(String.Format(ДЛЯ_КОНТРОЛА_ЗАДАН_НЕВЕРНЫЙ_ПАРАМЕТР,
                                     mPropertiesControlBase.Name,
                                     mPropertiesControlBase.Тип,
                                     mPropertiesControlBase.ИмяКаналаОсиУ,
                                     vbCrLf))
        Else
            mPropertiesControlBase.ИндексКаналаОсиУВМассивеПараметров = IndexParametersForControl(indexParamY) 'ИмяКаналаОсиУ
            isParameterAxisYFound = True
        End If

        Return isParameterAxisXFound AndAlso isParameterAxisYFound
    End Function
#End Region

#Region "События Контролов"
    'подписаться на событие
    Private Sub mMainMDIFormReference_frmMainAcquiredData(ByVal sender As Object, ByVal e As FormRegistrationBase.AcquiredDataEventArgs) Handles mMainMDIFormReference.AcquiredDataEvent
        Dim newValue As Double

        If IsTestSuccess Then
            Dim arrAcquiredData As Double() = e.ArrПарамНакопленные ' можно вызвать и другие свойства

            If numericEditList.Count > 0 Then
                Dim result As Double
                Dim isConditionFire As Boolean ' условие Сработало
                Dim numProperty As NumericEditProperties

                For Each NumericEditItem In numericEditList
                    numProperty = numericEditPropertiesDic(NumericEditItem.Name)
                    result = arrAcquiredData(numProperty.ИндексВМассивеПараметров)
                    NumericEditItem.Value = result
                    isConditionFire = True

                    Select Case numProperty.ОперацияСравнения
                        Case "="
                            isConditionFire = isConditionFire AndAlso (result = numProperty.ВеличинаУсловия)
                            Exit Select
                        Case "<>"
                            isConditionFire = isConditionFire AndAlso (result <> numProperty.ВеличинаУсловия)
                            Exit Select
                        Case "<"
                            isConditionFire = isConditionFire AndAlso (result < numProperty.ВеличинаУсловия)
                            Exit Select
                        Case ">"
                            isConditionFire = isConditionFire AndAlso (result > numProperty.ВеличинаУсловия)
                            Exit Select
                        Case "между"
                            isConditionFire = isConditionFire AndAlso (result > numProperty.ВеличинаУсловия AndAlso result < numProperty.ВеличинаУсловия2)
                            Exit Select
                        Case "вне"
                            isConditionFire = isConditionFire AndAlso (result < numProperty.ВеличинаУсловия OrElse result > numProperty.ВеличинаУсловия2)
                            Exit Select
                    End Select

                    If isConditionFire Then
                        If NumericEditItem.BackColor <> Color.Orange Then NumericEditItem.BackColor = Color.Orange
                    Else
                        If NumericEditItem.BackColor <> SystemColors.Window Then NumericEditItem.BackColor = SystemColors.Window
                    End If
                    'NumericEditItem.Refresh()
                Next
            End If

            'ВНИМАНИЕ для SwitchList KnobItem SlideItem надо регистрировать обработчик события в котором устанавливается значение порта
            'If SwitchList.Count > 0 Then
            '    For Each SwitchItem In SwitchList
            '        If SwitchItem.Value Then
            '            SwitchItem.CaptionBackColor = Color.Red
            '        Else
            '            SwitchItem.CaptionBackColor = Color.Transparent
            '        End If
            '        SwitchItem.Caption = SwitchItem.Value.ToString
            '    Next
            'End If
            '**********************************
            'If KnobList.Count > 0 Then
            '    For Each KnobItem In KnobList
            '        KnobItem.Caption = KnobItem.Value.ToString
            '    Next
            'End If

            'If SlideList.Count > 0 Then
            '    For Each SlideItem In SlideList
            '        SlideItem.Caption = SlideItem.Value.ToString
            '    Next
            'End If
            '**********************************

            'If LedList.Count > 0 Then
            '    For Each LedItem In LedList
            '        LedItem.Value = Not LedItem.Value
            '    Next
            'End If

            If tankList.Count > 0 Then
                Dim tankProperty As TankProperties

                For Each itemTank In tankList
                    tankProperty = tankPropertiesDic(itemTank.Name)

                    'TankItem.Value = GetNewValue(TankItem.Range, TankItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
                    newValue = arrAcquiredData(tankProperty.ИндексВМассивеПараметров)
                    'TankItem.Refresh()

                    'If newValue >= tankMyTypeDic(itemTank.Name).АварийноеЗначениеМин AndAlso newValue < tankMyTypeDic(itemTank.Name).АварийноеЗначениеМакс Then
                    '    itemTank.FillColor = Color.Orange
                    'ElseIf newValue >= tankMyTypeDic(itemTank.Name).АварийноеЗначениеМин Then
                    '    itemTank.FillColor = Color.Red
                    'Else
                    '    itemTank.FillColor = Color.Lime
                    'End If

                    If newValue < tankMyTypeDic(itemTank.Name).AlarmValueMin Then
                        itemTank.FillColor = Color.Orange
                    ElseIf newValue > tankMyTypeDic(itemTank.Name).AlarmValueMax Then
                        itemTank.FillColor = Color.Red
                    Else
                        itemTank.FillColor = Color.Lime
                    End If

                    itemTank.Value = newValue
                    itemTank.Caption = newValue.ToString("F2") 'tankProperty.ИмяКанала & "=" & 
                Next
            End If

            If gaugeList.Count > 0 Then
                Dim gaugeProperty As GaugeProperties

                For Each itemGauge In gaugeList
                    gaugeProperty = gaugePropertiesDic(itemGauge.Name)

                    'GaugeItem.Value = GetNewValue(GaugeItem.Range, GaugeItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
                    newValue = arrAcquiredData(gaugeProperty.ИндексВМассивеПараметров)
                    'GaugeItem.Refresh()

                    'If newValue >= gaugeMyTypeDic(itemGauge.Name).АварийноеЗначениеМин AndAlso newValue < gaugeMyTypeDic(itemGauge.Name).АварийноеЗначениеМакс Then
                    '    itemGauge.PointerColor = Color.Olive
                    'ElseIf newValue >= gaugeMyTypeDic(itemGauge.Name).АварийноеЗначениеМин Then
                    '    itemGauge.PointerColor = Color.Maroon
                    'Else
                    '    itemGauge.PointerColor = Color.Black
                    'End If

                    If newValue < gaugeMyTypeDic(itemGauge.Name).AlarmValueMin Then
                        itemGauge.PointerColor = Color.Olive
                    ElseIf newValue > gaugeMyTypeDic(itemGauge.Name).AlarmValueMax Then
                        itemGauge.PointerColor = Color.Maroon
                    Else
                        itemGauge.PointerColor = Color.Black
                    End If

                    itemGauge.Value = newValue
                    itemGauge.Caption = newValue.ToString("F2") 'gaugeProperty.ИмяКанала & "=" & 
                Next
            End If

            If thermometerList.Count > 0 Then
                Dim thermoProperty As ThermometerProperties

                For Each itemThermometer In thermometerList
                    thermoProperty = thermometerPropertiesDic(itemThermometer.Name)

                    'ThermometerItem.Value = GetNewValue(ThermometerItem.Range, ThermometerItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
                    newValue = arrAcquiredData(thermoProperty.ИндексВМассивеПараметров)
                    'ThermometerItem.Refresh()

                    'If newValue >= thermometerMyTypeDic(itemThermometer.Name).АварийноеЗначениеМин AndAlso newValue < thermometerMyTypeDic(itemThermometer.Name).АварийноеЗначениеМакс Then
                    '    itemThermometer.FillColor = Color.Orange
                    'ElseIf newValue >= thermometerMyTypeDic(itemThermometer.Name).АварийноеЗначениеМин Then
                    '    itemThermometer.FillColor = Color.Red
                    'Else
                    '    itemThermometer.FillColor = Color.Lime
                    'End If

                    If newValue < thermometerMyTypeDic(itemThermometer.Name).AlarmValueMin Then
                        itemThermometer.FillColor = Color.Orange
                    ElseIf newValue > thermometerMyTypeDic(itemThermometer.Name).AlarmValueMax Then
                        itemThermometer.FillColor = Color.Red
                    Else
                        itemThermometer.FillColor = Color.Lime
                    End If

                    itemThermometer.Value = newValue
                    itemThermometer.Caption = newValue.ToString("F2") ' thermoProperty.ИмяКанала & "=" & 
                Next
            End If

            If meterList.Count > 0 Then
                Dim meterProperty As MeterProperties

                For Each itemMeter In meterList
                    meterProperty = meterPropertiesDic(itemMeter.Name)

                    'MeterItem.Value = GetNewValue(MeterItem.Range, MeterItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
                    newValue = arrAcquiredData(meterProperty.ИндексВМассивеПараметров)
                    'MeterItem.Refresh()

                    'If newValue >= meterMyTypeDic(itemMeter.Name).АварийноеЗначениеМин AndAlso newValue < meterMyTypeDic(itemMeter.Name).АварийноеЗначениеМакс Then
                    '    itemMeter.PointerColor = Color.Olive
                    'ElseIf newValue >= meterMyTypeDic(itemMeter.Name).АварийноеЗначениеМин Then
                    '    itemMeter.PointerColor = Color.Maroon
                    'Else
                    '    itemMeter.PointerColor = Color.Black
                    'End If

                    If newValue < meterMyTypeDic(itemMeter.Name).AlarmValueMin Then
                        itemMeter.PointerColor = Color.Olive
                    ElseIf newValue > meterMyTypeDic(itemMeter.Name).AlarmValueMax Then
                        itemMeter.PointerColor = Color.Maroon
                    Else
                        itemMeter.PointerColor = Color.Black
                    End If

                    itemMeter.Value = newValue
                    itemMeter.Caption = newValue.ToString("F2") ' meterProperty.ИмяКанала & "=" & 
                Next
            End If

            If waveformGraphList.Count > 0 Then
                'Dim x As Double
                'Dim y As Double
                'data.GetNextPoint(x, y)
                Dim waveGraphProperty As WaveformGraphProperties
                Dim valData As Double

                For Each itemWaveformGraph As WaveformGraph In waveformGraphList
                    waveGraphProperty = waveformGraphPropertiesDic(itemWaveformGraph.Name)
                    valData = arrAcquiredData(waveGraphProperty.ИндексВМассивеПараметров)

                    If waveGraphProperty.ТипГрафика Then ' горизонтально 
                        itemWaveformGraph.PlotYAppend(valData)
                        Dim arrData As Double() = itemWaveformGraph.Plots(0).GetXData ' получить накопленный буфер из Plots
                        If arrData.Count > 1 Then itemWaveformGraph.Cursors(0).XPosition = arrData(arrData.Count - 1) ' переместить курсор
                    Else
                        itemWaveformGraph.PlotXAppend(valData)
                        Dim arrData As Double() = itemWaveformGraph.Plots(0).GetYData ' получить накопленный буфер из Plots
                        If arrData.Count > 1 Then itemWaveformGraph.Cursors(0).YPosition = arrData(arrData.Count - 1) ' переместить курсор
                    End If
                    'WaveformGraphItem.Refresh()
                Next
            End If

            If scatterGraphList.Count > 0 Then
                'Dim x As Double
                'Dim y As Double
                'data.GetNextPoint(x, y)
                'Const numberOfPoints As Integer = 360

                'Dim dataR(numberOfPoints) As Double
                'Dim dataP(numberOfPoints) As Double
                'Dim dataX(numberOfPoints) As Double
                'Dim dataY(numberOfPoints) As Double

                '' вычислить полярные координаты.
                'For i As Integer = 0 To numberOfPoints
                '    dataP(i) = i * 10
                '    dataR(i) = Math.Pow(i, 2) / 70000
                'Next

                '' Конвертировать полярные координаты в XY координаты.
                'For i As Integer = 0 To numberOfPoints
                '    Dim current As Double = (dataP(i) / numberOfPoints) * TwoPi
                '    dataX(i) = Math.Cos(current) * dataR(i) * y
                '    dataY(i) = Math.Sin(current) * dataR(i) * y
                'Next

                Dim x As Double
                Dim y As Double
                Dim scatterGraphProperty As ScatterGraphProperties

                For Each itemScatterGraph In scatterGraphList
                    scatterGraphProperty = scatterGraphPropertiesDic(itemScatterGraph.Name)
                    x = arrAcquiredData(scatterGraphProperty.ИндексКаналаОсиХВМассивеПараметров)
                    y = arrAcquiredData(scatterGraphProperty.ИндексКаналаОсиУВМассивеПараметров)
                    scatterGraphProperty.РегистрацияГрафики(x, y)
                    'ScatterGraphItem.Plots(0).PlotXY(dataX, dataY)
                    itemScatterGraph.Plots(0).PlotXY(scatterGraphProperty.GetDataX, scatterGraphProperty.GetDataY)
                    'ScatterGraphItem.Cursors(0).MoveCursor(dataX.Last, dataY.Last)
                    itemScatterGraph.Cursors(0).MoveCursor(x, y)
                    'ScatterGraphItem.Refresh()
                Next
            End If

            'Me.Refresh() 'не помогло всё сдвигается
        End If
    End Sub

    Private Sub Switch_StateChanged(ByVal sender As Object, ByVal e As ActionEventArgs)
        Dim mSwitch As WindowsForms.Switch = CType(sender, WindowsForms.Switch)
        If mSwitch.Caption <> "" Then
            If mSwitch.Value Then
                mSwitch.CaptionBackColor = Color.Red
            Else
                mSwitch.CaptionBackColor = SystemColors.ActiveCaption 'Color.Transparent
            End If
            'sender.Caption = sender.Value.ToString
        End If

        Dim mSwitchProperties As SwitchProperties = switchPropertiesDic(mSwitch.Name)
        Dim lines As String

        If mSwitchProperties.НомерМодуляКорзины = "" Then
            ' плата
            lines = $"Dev{mSwitchProperties.НомерУстройства}/port{mSwitchProperties.НомерПорта}/line{mSwitchProperties.НомерБита}"
        Else ' модуль SCXI'SC1Mod<slot#>/port0/lineN" 
            lines = $"SC{mSwitchProperties.НомерУстройства}Mod{mSwitchProperties.НомерМодуляКорзины}/port{mSwitchProperties.НомерПорта}/line{mSwitchProperties.НомерБита}"
        End If

        ' в теге содержится имя линии цифрового порта
        Dim digitalWriteTask As Task = New Task("digital")
        mSwitch.Refresh()

        Try
            digitalWriteTask.DOChannels.CreateChannel(lines, "", ChannelLineGrouping.OneChannelForAllLines) ' работает
            ' Запись данных в цифровой порт.  WriteDigitalSingChanSingSampPort записывает простой набор цифровых данных
            ' по требованию, поэтому необходим timeout.
            Dim writer As DigitalSingleChannelWriter = New DigitalSingleChannelWriter(digitalWriteTask.Stream)
            digitalWriteTask.Control(TaskAction.Verify)
            Dim bits As Boolean() = New Boolean() {mSwitch.Value}
            writer.WriteSingleSampleMultiLine(True, bits)
        Catch ex As Exception
            Const caption As String = "Switch_StateChanged"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            digitalWriteTask.Dispose()
        End Try
    End Sub

    Private Sub Knob_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs)
        Dim tempKnob As Knob = CType(sender, Knob)
        Dim tempGenerator As FormsPanelManager.Generator = gFormsPanelManager.AnalogOutputsDictionary(knobPropertiesDic(tempKnob.Name).ToString).Generator

        tempKnob.Refresh()

        If tempGenerator.TypeFrequencyControl = "Частота" Then
            tempGenerator.FixFrequency = tempKnob.Value
        Else
            tempGenerator.AmplitudeFix = tempKnob.Value
        End If

        gFormsPanelManager.Restart()
    End Sub

    Public Sub Slide_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs)
        'sender.Caption = sender.Value.ToString
        Dim mSlide As Slide = CType(sender, Slide)
        Dim mSlideProperties As SlideProperties = slidePropertiesDic(mSlide.Name)
        mSlide.Refresh()
        ' плата
        Dim lines As String = $"Dev{mSlideProperties.НомерУстройства}/ao{mSlideProperties.НомерКанала}"
        Dim myAOutputTask As Task = Nothing

        Try
            myAOutputTask = New Task()
            myAOutputTask.AOChannels.CreateVoltageChannel(
                lines,
                "aoChannel",
                mSlideProperties.ЗначениеЦапМин,
                mSlideProperties.ЗначениеЦапМакс,
                AOVoltageUnits.Volts)

            Dim writer As AnalogSingleChannelWriter = New AnalogSingleChannelWriter(myAOutputTask.Stream)
            myAOutputTask.Control(TaskAction.Verify)
            ' здесь функция преобразования mSlide.Value в вольт
            With mSlideProperties
                writer.WriteSingleSample(True, LinearInterpolation(mSlide.Value, .УправлениеМин, .ЗначениеЦапМин, .УправлениеМакс, .ЗначениеЦапМакс))
            End With
        Catch ex As DaqException
            Dim caption As String = NameOf(Slide_AfterChangeValue)
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            myAOutputTask.Dispose()
        End Try
    End Sub

#End Region

#Region "Проверка"
    ''' <summary>
    ''' Проверка Дискретных Выходов
    ''' </summary>
    ''' <param name="errors"></param>
    Private Sub CheckDiscreteOutput(ByVal errors As List(Of String))
        If switchList.Count > 0 Then
            For Each itemSwitch As WindowsForms.Switch In switchList
                ' получить свойство
                Dim mSwitchProperties As SwitchProperties = switchPropertiesDic(itemSwitch.Name)

                'For Each mSwitchProperties As Порт In mДействие.ListПорт
                Dim namePort As String

                If mSwitchProperties.НомерМодуляКорзины = "" Then
                    'плата
                    namePort = $"Dev{mSwitchProperties.НомерУстройства}/port{mSwitchProperties.НомерПорта}" '& "/line" & mБит.НомерБита
                    'DeviceName = "Dev" & mSwitchProperties.НомерУстройства
                Else 'модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    namePort = $"SC{mSwitchProperties.НомерУстройства}Mod{mSwitchProperties.НомерМодуляКорзины}/port{mSwitchProperties.НомерПорта}" '& "/line" & mБит.НомерБита
                    'DeviceName = "SC" & mSwitchProperties.НомерУстройства & "Mod" & mSwitchProperties.НомерМодуляКорзины
                End If

                If gFormsPanelManager.DigitalPortsDictionary IsNot Nothing Then
                    ' проверка существования в системе данного порта
                    For Each itemPort As FormsPanelManager.DigitalPort In gFormsPanelManager.DigitalPortsDictionary.Values 'PortCollection.Values
                        If itemPort.NameDigitalPort = namePort Then
                            If itemPort.IsPortOutput Then ' не был использован как Input предыдущей загруженной панелью
                                'For Each mБит As Бит In mSwitchProperties.ListБит 'Dev0/port1/line0
                                ' Строковая заготовка
                                Dim textMessage As String
                                If mSwitchProperties.НомерМодуляКорзины = "" Then
                                    textMessage = String.Format("Элемент:{0}{1}Плата:{2}{1}Порт:{3}{1}Бит:{4}{1}", itemSwitch.Name, vbCrLf, mSwitchProperties.НомерУстройства, mSwitchProperties.НомерПорта, mSwitchProperties.НомерБита)
                                Else
                                    textMessage = String.Format("Элемент:{0}{1}Корзина:{2}{1}Модуль:{3}{1}Порт:{4}{1}Бит:{5}{1}", itemSwitch.Name, vbCrLf, mSwitchProperties.НомерУстройства, mSwitchProperties.НомерМодуляКорзины, mSwitchProperties.НомерПорта, mSwitchProperties.НомерБита)
                                End If
                                'PortLineDic
                                If itemPort.LineCount >= mSwitchProperties.НомерБита AndAlso itemPort.AllPortLineDic.ContainsValue($"{namePort}/line{mSwitchProperties.НомерБита}") Then
                                    If itemPort.IsLineOutput(mSwitchProperties.НомерБита) Then
                                        textMessage &= "Бит с данным номером уже используется в загруженной панели"
                                        errors.Add(textMessage)
                                    Else 'OK
                                        itemPort.IsLineOutput(mSwitchProperties.НомерБита) = True
                                        ' так как биты дважды не используются 

                                        ' проверка если девайса еще не было то создать коллекцию и добавить в словарь New List(Of String)
                                        'If Not FormsPanelManager.strDigitOutLineCollDic.ContainsKey(DeviceName) Then
                                        '    FormsPanelManager.strDigitOutLineCollDic.Add(DeviceName, New List(Of String))
                                        'End If
                                        'FormsPanelManager.strDigitOutLineCollDic(DeviceName).Add(NamePort & "/line" & mSwitchProperties.НомерБита)
                                    End If
                                Else
                                    If mSwitchProperties.НомерМодуляКорзины = "" Then
                                        textMessage &= "Нет соответствующего устройства в компьютере."
                                    Else
                                        textMessage &= "Нет соответствующего модуля в корзине."
                                    End If

                                    errors.Add(textMessage)
                                End If
                            Else
                                errors.Add($"Порт {namePort} уже использован для входных сигналов в загруженной панели")
                            End If
                        End If
                    Next
                Else
                    errors.Add($"{namePort} в системе не существует {vbCrLf}")
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Проверка Дискретных Входов
    ''' </summary>
    ''' <param name="errors"></param>
    Private Sub CheckDiscreteInput(ByVal errors As List(Of String))
        Dim indexLed As Integer ' индекс Элемента В Коллекции LedList

        If ledList.Count > 0 Then
            For Each itemLed As Led In ledList
                ' получить свойство
                Dim mLedProperties As LedProperties = ledPropertiesDic(itemLed.Name)

                Dim namePort As String
                If mLedProperties.НомерМодуляКорзины = "" Then
                    ' плата
                    namePort = $"Dev{mLedProperties.НомерУстройства}/port{mLedProperties.НомерПорта}" '& "/line" & mБит.НомерБита
                    'DeviceName = "Dev" & mLedProperties.НомерУстройства
                Else ' модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    namePort = $"SC{mLedProperties.НомерУстройства}Mod{mLedProperties.НомерМодуляКорзины}/port{mLedProperties.НомерПорта}" '& "/line" & mБит.НомерБита
                    'DeviceName = "SC" & mLedProperties.НомерУстройства & "Mod" & mLedProperties.НомерМодуляКорзины
                End If
                If gFormsPanelManager.DigitalPortsDictionary IsNot Nothing Then
                    ' проверка существования в системе данного порта
                    For Each itemPort As FormsPanelManager.DigitalPort In gFormsPanelManager.DigitalPortsDictionary.Values
                        If itemPort.NameDigitalPort = namePort Then
                            ' Строковая заготовка
                            ' проверить не используется ли данный порт для вывода, если нет, то назначить его как INPUT
                            'TempPort.ЛинияИспользуется(mSwitchProperties.НомерБита) = True
                            Dim isPortFree As Boolean = True ' порт Свободен
                            For i As Integer = 0 To itemPort.IsLineOutput.Count - 1
                                If itemPort.IsLineOutput(i) = True Then ' используется тумблером
                                    isPortFree = False
                                    Exit For
                                End If
                            Next

                            If isPortFree Then
                                itemPort.IsPortOutput = False ' назначить его как INPUT

                                Dim textMessage As String
                                If mLedProperties.НомерМодуляКорзины = "" Then
                                    textMessage = String.Format("Элемент:{0}{1}Плата:{2}{1}Порт:{3}{1}Бит:{4}{1}", itemLed.Name, vbCrLf, mLedProperties.НомерУстройства, mLedProperties.НомерПорта, mLedProperties.НомерБита)
                                Else
                                    textMessage = String.Format("Элемент:{0}{1}Корзина:{2}{1}Модуль:{3}{1}Порт:{4}{1}Бит:{5}{1}", itemLed.Name, vbCrLf, mLedProperties.НомерУстройства, mLedProperties.НомерМодуляКорзины, mLedProperties.НомерПорта, mLedProperties.НомерБита)
                                End If

                                ' PortLineDic
                                If itemPort.LineCount >= mLedProperties.НомерБита AndAlso itemPort.AllPortLineDic.ContainsValue($"{namePort}/line{mLedProperties.НомерБита}") Then
                                    ' здесь на одну линию порта ввода можно назначить много потребителей индикаторов
                                    'If TempPort.ЛинияИспользуетсяДляВывода(mLedProperties.НомерБита) Then
                                    '    TextMessage &= "Бит с данным номером уже используется в загруженной панели"
                                    '    errors.Add(TextMessage)
                                    'Else 'OK
                                    'TempPort.ЛинияИспользуетсяДляВывода(mLedProperties.НомерБита) = True

                                    gFormsPanelManager.MyDigitalInputsMotoristPanel.Add($"{namePort}/line{mLedProperties.НомерБита}", NameMotoristPanel, itemLed.Name, indexLed)

                                    ' проверка если девайса еще не было то создать коллекцию и добавить в словарь New List(Of String)
                                    'If Not FormsPanelManager.strDigitOutLineCollDic.ContainsKey(DeviceName) Then
                                    '    FormsPanelManager.strDigitOutLineCollDic.Add(DeviceName, New List(Of String))
                                    'End If
                                    'FormsPanelManager.strDigitOutLineCollDic(DeviceName).Add(NamePort & "/line" & mLedProperties.НомерБита)
                                    'End If
                                Else
                                    If mLedProperties.НомерМодуляКорзины = "" Then
                                        textMessage &= "Нет соответствующего устройства в компьютере."
                                    Else
                                        textMessage &= "Нет соответствующего модуля в корзине."
                                    End If

                                    errors.Add(textMessage)
                                End If
                            Else
                                errors.Add($"Порт {namePort} уже использован для выходных сигналов в загруженной панели")
                            End If
                        End If '= NamePort
                    Next 'Порт
                    indexLed += 1
                Else
                    errors.Add($"{namePort} в системе не существует {vbCrLf}")
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Проверка Аналоговых Выводов Генератора
    ''' </summary>
    ''' <param name="errors"></param>
    Private Sub CheckAnalogOutputGenerator(ByVal errors As List(Of String))
        Dim isLineFound As Boolean ' линия Найдена
        Dim indexKnob As Integer ' индекс Элемента В Коллекции Knob List

        Try
            If knobList.Count > 0 Then
                For Each itemKnob As Knob In knobList
                    ' получить свойство
                    Dim mKnobProperties As KnobProperties = knobPropertiesDic(itemKnob.Name)
                    ' плата
                    Dim namePort As String = $"Dev{mKnobProperties.НомерУстройства}/ao{mKnobProperties.НомерКанала}"
                    ' проверка существования в системе данного порта
                    isLineFound = False

                    If gFormsPanelManager.AnalogOutputsDictionary IsNot Nothing Then
                        For Each itemPort As FormsPanelManager.AnalogOutput In gFormsPanelManager.AnalogOutputsDictionary.Values
                            If itemPort.NameAnalogOutputPort = namePort Then
                                If itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.Nobody Then ' не был использован как output предыдущей загруженной панелью
                                    itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.Generator
                                    itemPort.Subscriber.NameMotoristPanel = NameMotoristPanel
                                    itemPort.Subscriber.NameIndicator = itemKnob.Name
                                    itemPort.Subscriber.IndexItem = indexKnob

                                    'FormsPanelManager.AnalogOutputGeneratorDic.Add(NamePort, New FormsPanelManager.Generator(NamePort))
                                    ' получить только-что добавленный экземпляр
                                    Dim tempGenerator As FormsPanelManager.Generator = gFormsPanelManager.AnalogOutputsDictionary(namePort).Generator
                                    tempGenerator.TypeWaveform = mKnobProperties.ТипГенератора
                                    tempGenerator.TypeFrequencyControl = mKnobProperties.ТипРегулировки
                                    tempGenerator.FrequencyMin = mKnobProperties.ЧастотаМин
                                    tempGenerator.FrequencyMax = mKnobProperties.ЧастотаМакс
                                    tempGenerator.AmplitudeFix = mKnobProperties.ЗначениеАмплитудыФикс
                                    tempGenerator.RangeDacMin = mKnobProperties.ЗначениеЦапМин
                                    tempGenerator.RangeDacMax = mKnobProperties.ЗначениеЦапМакс
                                    tempGenerator.FixFrequency = mKnobProperties.ЧастотаФикс
                                    tempGenerator.PointsSamplePerBuffer = mKnobProperties.ТочекГрафикаВБуфере
                                    tempGenerator.CyclesPerBuffer = mKnobProperties.ЦикловВБуфере
                                    tempGenerator.СоответствиеДиапазону = mKnobProperties.СоответствиеДиапазону

                                    If tempGenerator.TypeFrequencyControl = "Частота" Then
                                        itemKnob.Range = New Range(tempGenerator.FrequencyMin, tempGenerator.FrequencyMax)
                                        tempGenerator.FixFrequency = itemKnob.Value ' начать диапазон с этого значения
                                    Else
                                        'KnobItem.Range = New NationalInstruments.UI.Range(TempGenerator.ЗначениеЦапМин, TempGenerator.ЗначениеЦапМакс)
                                        itemKnob.Range = New Range(0.1, tempGenerator.RangeDacMax)
                                        tempGenerator.AmplitudeFix = itemKnob.Value 'начать диапазон с этого значения
                                    End If

                                    'AddHandler CType(aControl, NationalInstruments.UI.WindowsForms.Knob).AfterChangeValue, AddressOf Knob_AfterChangeValue
                                    AddHandler itemKnob.AfterChangeValue, AddressOf Knob_AfterChangeValue
                                Else
                                    errors.Add($"Линия {namePort} уже использован для выходных сигналов в загруженной панели")
                                End If
                                isLineFound = True
                                Exit For
                            End If
                        Next 'Порт

                        If Not isLineFound Then errors.Add(namePort & " Нет соответствующего устройства в компьютере.")

                        indexKnob += 1
                    Else
                        errors.Add($"{namePort} в системе не существует {vbCrLf}")
                    End If
                Next
                If gFormsPanelManager.AnalogOutputsDictionary IsNot Nothing Then
                    If errors.Count <> 0 Then
                        'For Each TempGenerator As FormsPanelManager.Generator In FormsPanelManager.AnalogOutputGeneratorDic.Values
                        '    FormsPanelManager.AOutputPortSystemCollection(TempGenerator.PhysicalChannel).ЛинияИспользуется = False
                        'Next
                        'FormsPanelManager.AnalogOutputGeneratorDic.Clear()

                        For Each itemPort As FormsPanelManager.AnalogOutput In gFormsPanelManager.AnalogOutputsDictionary.Values
                            If itemPort.Subscriber.NameMotoristPanel = NameMotoristPanel Then
                                itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.Nobody
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(CheckAnalogOutputGenerator)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Проверка Аналоговых Выводов Напряжения
    ''' </summary>
    ''' <param name="errors"></param>
    Private Sub CheckAnalogOutputVolt(ByVal errors As List(Of String))
        Dim isLineFound As Boolean ' линия Найдена
        Dim indexSlide As Integer ' индекс Элемента В Коллекции SlideList

        Try
            If slideList.Count > 0 Then
                For Each itemSlide As Slide In slideList
                    ' получить свойство
                    Dim mSlideProperties As SlideProperties = slidePropertiesDic(itemSlide.Name)
                    ' плата
                    Dim namePort As String = $"Dev{mSlideProperties.НомерУстройства}/ao{mSlideProperties.НомерКанала}"
                    ' проверка существования в системе данного порта
                    isLineFound = False

                    If gFormsPanelManager.AnalogOutputsDictionary IsNot Nothing Then
                        For Each itemPort As FormsPanelManager.AnalogOutput In gFormsPanelManager.AnalogOutputsDictionary.Values
                            If itemPort.NameAnalogOutputPort = namePort Then
                                If itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.Nobody Then ' не был использован как output предыдущей загруженной панелью
                                    itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.VoltOut
                                    itemPort.Subscriber.NameMotoristPanel = NameMotoristPanel
                                    itemPort.Subscriber.NameIndicator = itemSlide.Name
                                    itemPort.Subscriber.IndexItem = indexSlide

                                    itemSlide.Range = New Range(mSlideProperties.УправлениеМин, mSlideProperties.УправлениеМакс)
                                    AddHandler itemSlide.AfterChangeValue, AddressOf Slide_AfterChangeValue
                                Else
                                    errors.Add($"Линия {namePort} уже использован для выходных сигналов в загруженной панели")
                                End If

                                isLineFound = True
                                Exit For
                            End If
                        Next ' Порт

                        If Not isLineFound Then errors.Add(namePort & " Нет соответствующего устройства в компьютере.")

                        indexSlide += 1
                    Else
                        errors.Add($"{namePort} в системе не существует {vbCrLf}")
                    End If
                Next
                If gFormsPanelManager.AnalogOutputsDictionary IsNot Nothing Then
                    If errors.Count <> 0 Then
                        For Each itemPort As FormsPanelManager.AnalogOutput In gFormsPanelManager.AnalogOutputsDictionary.Values
                            If itemPort.Subscriber.NameMotoristPanel = NameMotoristPanel Then
                                itemPort.WhoUseLine = FormsPanelManager.WhoUseLineAOLine.Nobody
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(CheckAnalogOutputVolt)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub


    ''' <summary>
    ''' Проверка Аналоговых Входов
    ''' Проверка аналоговых выходов на корректную идентификацию в существующем наборе железа
    ''' NumericEdit, Tank, Gauge, Thermomete,r Meter, WaveformGraph, ScatterGraph
    ''' </summary>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub CheckAnalogInput(ByVal errors As List(Of String))
        If numericEditList.Count > 0 Then
            For Each itemNumericEdit In numericEditList
                CheckConcrete(itemNumericEdit.Name, numericEditPropertiesDic(itemNumericEdit.Name).ИмяКанала, CType(numericEditPropertiesDic(itemNumericEdit.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If tankList.Count > 0 Then
            For Each itemTank In tankList
                CheckConcrete(itemTank.Name, tankPropertiesDic(itemTank.Name).ИмяКанала, CType(tankPropertiesDic(itemTank.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If gaugeList.Count > 0 Then
            For Each itemGauge In gaugeList
                CheckConcrete(itemGauge.Name, gaugePropertiesDic(itemGauge.Name).ИмяКанала, CType(gaugePropertiesDic(itemGauge.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If thermometerList.Count > 0 Then
            For Each itemThermometer In thermometerList
                CheckConcrete(itemThermometer.Name, thermometerPropertiesDic(itemThermometer.Name).ИмяКанала, CType(thermometerPropertiesDic(itemThermometer.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If meterList.Count > 0 Then
            For Each MeterItem In meterList
                CheckConcrete(MeterItem.Name, meterPropertiesDic(MeterItem.Name).ИмяКанала, CType(meterPropertiesDic(MeterItem.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If waveformGraphList.Count > 0 Then
            For Each itemWaveformGraph In waveformGraphList
                CheckConcrete(itemWaveformGraph.Name, waveformGraphPropertiesDic(itemWaveformGraph.Name).ИмяКанала, CType(waveformGraphPropertiesDic(itemWaveformGraph.Name), PropertiesControlChannelBase), errors)
            Next
        End If

        If scatterGraphList.Count > 0 Then
            For Each itemScatterGraph In scatterGraphList
                Dim mScatterGraphProperties As ScatterGraphProperties = scatterGraphPropertiesDic(itemScatterGraph.Name)
                Dim nameControl As String = itemScatterGraph.Name

                If FindIndexInArrayParameters(mScatterGraphProperties, errors) Then
                    'If mScatterGraphProperties.AxesXBindingWithNetVar Then
                    'CheckConcrete(nameControl, mScatterGraphProperties.ИмяКаналаОсиХ, errors)
                    'End If

                    'If mScatterGraphProperties.AxesYBindingWithNetVar Then
                    'CheckConcrete(nameControl, mScatterGraphProperties.ИмяКаналаОсиУ, errors)
                    'End If
                End If
            Next
        End If
    End Sub

    Private Sub CheckConcrete(nameControl As String, nameNetVar As String, ByRef mPropertiesControlBase As PropertiesControlChannelBase, ByVal errors As List(Of String))
        'Dim nameNetVar As String = mPropertiesControlBase.ИмяКанала
        If Not FindIndexInArrayParameters(mPropertiesControlBase, errors) Then
            'CheckConcrete(nameControl, nameNetVar, errors)
        End If
    End Sub

#End Region

#Region "Вывод ошибки errorsText"
    Private pictureBox1 As PictureBox
    Private errorsText As String

    ''' <summary>
    ''' Обработчик события перерисовки рисунка с текстом 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pictureBox1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
        ' создать локальную версию graphics объекта PictureBox.
        Dim g As Graphics = e.Graphics

        ' рисовать строку на PictureBox.
        g.DrawString(errorsText,
            New Font("Arial", 10), Brushes.Red, New PointF(30.0F, 30.0F))
        '' рисовать линию на PictureBox.
        'g.DrawLine(System.Drawing.Pens.Red,
        '           pictureBox1.Left, pictureBox1.Top,
        '           pictureBox1.Right, pictureBox1.Bottom)
    End Sub

    ''' <summary>
    ''' Вывод на поверхности формы ошибку загрузки сериализации из файла
    ''' </summary>
    ''' <param name="errorsText"></param>
    ''' <remarks></remarks>
    Public Sub ShowLosdErrors(errorsText As String)
        Me.errorsText = errorsText
        pictureBox1 = New PictureBox() With {.Dock = DockStyle.Fill, .BackColor = Color.White}
        AddHandler pictureBox1.Paint, AddressOf pictureBox1_Paint
        ' добавить PictureBox control на форму.
        Controls.Add(pictureBox1)
    End Sub

#End Region

    ''' <summary>
    ''' Выдать элемент массива MyType из arrПараметры
    ''' </summary>
    ''' <param name="nameChannel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMyTypeFromArrParameters(nameChannel As String) As TypeBaseParameterTCP
        If IsUseTCPClient Then
            Dim parametersSet = From parameter In ParametersTCP
                                Where parameter.NameParameter = nameChannel
                                Select parameter

            If parametersSet.Count > 0 Then
                Return parametersSet(0)
            Else
                Dim TypeBaseParameterSet = From parameter In ParametersType
                                           Where parameter.NameParameter = nameChannel
                                           Select parameter

                If TypeBaseParameterSet.Count > 0 Then
                    Return GetTypeBaseParameter(TypeBaseParameterSet(0))
                Else
                    Return Nothing ' проблема в будущем, надо обработать ошибку
                End If
            End If
        Else
            Dim TypeBaseParameterSet = From parameter In ParametersType
                                       Where parameter.NameParameter = nameChannel
                                       Select parameter

            If TypeBaseParameterSet.Count > 0 Then
                Return GetTypeBaseParameter(TypeBaseParameterSet(0))
            Else
                Return Nothing ' проблема в будущем, надо обработать ошибку
            End If
        End If
    End Function

    Public Function GetTypeBaseParameter(inTypeBaseParameter As TypeBaseParameter) As TypeBaseParameterTCP
        Dim tempBaseType As TypeBaseParameterTCP = New TypeBaseParameterTCP

        With inTypeBaseParameter
            tempBaseType.NumberParameter = .NumberParameter ' НомерПараметра
            tempBaseType.NameParameter = .NameParameter ' НаименованиеПараметра
            tempBaseType.UnitOfMeasure = .UnitOfMeasure ' ЕдиницаИзмерения
            tempBaseType.LowerLimit = .LowerLimit ' ДопускМинимум
            tempBaseType.UpperLimit = .UpperLimit  'ДопускМаксимум
            tempBaseType.RangeYmin = .RangeYmin ' РазносУмин
            tempBaseType.RangeYmax = .RangeYmax ' РазносУмакс
            tempBaseType.AlarmValueMin = .AlarmValueMin ' АварийноеЗначениеМин
            tempBaseType.AlarmValueMax = .AlarmValueMax ' АварийноеЗначениеМакс
            tempBaseType.IsVisibleRegistration = .IsVisibleRegistration ' ВидимостьРегистратор
            tempBaseType.Description = .Description ' Примечания
            tempBaseType.LevelWarning = LevelWarningWhat.Normal
        End With

        Return tempBaseType
    End Function

#Region "Вспомогательные для удаления"
    'Private PanelNameValue As String
    'Private fileNameValue As String
    'Private closingCompleteValue As Boolean = False
    'Private Scrolling As Boolean
    'Private raisedWithThickNeedleMeterValueIncreasing As Boolean = True

    ''--- события -------------------------------------------------------------
    ' ''' <summary>
    ' ''' Это пользовательские события используются для извещения Forms class
    ' ''' когда пользователь прервал сохранение несохраненного документа в событии закрытия формы
    ' ''' или вызвал Exit из File menu.
    ' ''' </summary>
    'Public Event SaveWhileClosingCancelled As System.EventHandler
    'Public Event ExitApplication As System.EventHandler


    ' Свойство для лёгкого доступа к высоте формы.
    'Public Property SurveyHeight() As Integer
    '    Get
    '        Return Me.Height
    '    End Get
    '    Set(ByVal Value As Integer)
    '        Me.Height = Value
    '    End Set
    'End Property

    ' Свойство для лёгкого доступа к описанию контрола
    'Public ReadOnly Property SurveyResponse() As String
    '    Get
    '        Return surveyResponseValue
    '    End Get
    'End Property

    ' Свойство для лёгкого доступа к заголовку формы
    'Public Property SurveyTitle() As String
    '    Get
    '        Return surveyTitleValue
    '    End Get
    '    Set(ByVal Value As String)
    '        surveyTitleValue = Value
    '        Me.Text = surveyTitleValue
    '    End Set
    'End Property

    ' Свойство для лёгкого доступа к ширине формы.
    'Public Property SurveyWidth() As Integer
    '    Get
    '        Return Me.Width
    '    End Get
    '    Set(ByVal Value As Integer)
    '        Me.Width = Value
    '    End Set
    'End Property

    'Public ReadOnly Property ClosingComplete() As Boolean
    '    Get
    '        Return closingCompleteValue
    '    End Get
    'End Property

    'Public ReadOnly Property PanelName() As String
    '    Get
    '        Return PanelNameValue
    '    End Get
    'End Property

    ' ''' <summary>
    ' ''' XML файл дизайнера для сериализации 
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property FileName() As String
    '    Get
    '        Return fileNameValue
    '    End Get
    '    Set(ByVal Value As String)
    '        fileNameValue = Value
    '        PanelNameValue = System.IO.Path.GetFileNameWithoutExtension(fileNameValue)
    '        Me.Text = Me.PanelName
    '    End Set
    'End Property

    'Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
    '    ' выйти из приложения
    '    RaiseEvent ExitApplication(Me, Nothing)
    'End Sub

    'Private Sub mnuNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNew.Click
    '    ' создать новый экземпляр документа
    '    Forms.NewForm()
    'End Sub

    'Private Sub mnuClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClose.Click
    '    Me.Close()
    'End Sub

    'Private Sub SaveDocument()
    '    ' этот код не производит какой либо файл I/O. 
    '    Try
    '        ' проверить, что документ не сохранён
    '        If Me.Dirty Then
    '            ' убедиться, что имя файла (документа) уже создано
    '            If Not Me.FileName Is Nothing Then
    '                ' сохранить текущий документ в файле
    '            Else
    '                ' Имя файла не присвоено - спросить.
    '                ' можно использовать Common Dialog 

    '                ' Или имя файла основано на имени документа и текущей директории приложения
    '                Me.FileName = AppDomain.CurrentDomain.BaseDirectory & "Saved" & Me.Text
    '            End If

    '            ' Как только документ сохранён, сбросить флаг сохранения
    '            Me.Dirty = False
    '        End If
    '    Catch exp As Exception
    '        MessageBox.Show(exp.Message, exp.Source, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '' Закрыть форму.
    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    ' сбросить строку SurveyResponse подсказки.
    '    'surveyResponseValue = "Survey Not Completed"
    '    Me.Close()
    'End Sub

    'Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
    '    sampleTimer.Enabled = True
    'End Sub
    'Private Shared Function GetNewValue(ByVal range As Range, ByVal currentValue As Double, ByVal numberOfIntervals As Integer, ByRef increasing As Boolean) As Double
    '    'определить новые значения контрола.
    '    Dim controlRange As Double = range.Maximum - range.Minimum
    '    Dim newValue As Double = 0
    '    If (increasing) Then
    '        newValue = currentValue + controlRange / numberOfIntervals
    '        If (newValue > range.Maximum) Then
    '            newValue = range.Maximum
    '            increasing = False
    '        End If
    '    Else
    '        newValue = currentValue - controlRange / numberOfIntervals
    '        If (newValue < range.Minimum) Then
    '            newValue = range.Minimum
    '            increasing = True
    '        End If
    '    End If

    '    Return newValue
    'End Function

    'Private Enum ChartingModes
    '    Strip
    '    Scope
    'End Enum

    'Private data As DataManager

    'Private Sub SetAxisModes(ByRef aControl As WindowsForms.WaveformGraph)
    '    'If optionStripChartRadioButton.Checked Then
    '    SetAxisModes(ChartingModes.Strip, aControl)
    '    'ElseIf optionScopeChartRadioButton.Checked Then
    '    'SetAxisModes(ChartingModes.Scope)
    '    'End If
    '    'StripChart
    'End Sub

    'Private Sub SetAxisModes(ByVal mode As ChartingModes, ByRef aControl As WindowsForms.WaveformGraph)
    '    Dim chartingAxis As Axis
    '    Dim scaleAxis As Axis

    '    If data.IsVertical Then
    '        chartingAxis = aControl.YAxes(0) 'yAxis
    '        scaleAxis = aControl.XAxes(0) 'xAxis
    '    Else
    '        chartingAxis = aControl.XAxes(0) 'xAxis
    '        scaleAxis = aControl.YAxes(0) 'yAxis
    '    End If

    '    scaleAxis.Mode = AxisMode.AutoScaleLoose
    '    If mode = ChartingModes.Scope Then
    '        chartingAxis.Mode = AxisMode.ScopeChart
    '    Else
    '        chartingAxis.Mode = AxisMode.StripChart
    '    End If

    '    aControl.ClearData()
    '    data.Reset()
    'End Sub

    'Private Class DataManager
    '    Private Const NumberOfPoints As Integer = 100
    '    Private Const YRange As Integer = 10

    '    Private data As Double()
    '    Private index As Integer
    '    Private currentX As Double
    '    Private vertical As Boolean
    '    Private Const TwoPi As Double = Math.PI * 2

    '    Public Sub New()
    '        data = GenerateSineWave(NumberOfPoints, YRange)
    '        Reset()
    '    End Sub

    '    Property IsVertical() As Boolean
    '        Get
    '            Return vertical
    '        End Get

    '        Set(ByVal Value As Boolean)
    '            vertical = Value
    '            Reset()
    '        End Set
    '    End Property

    '    Public Sub Reset()
    '        index = -1
    '        currentX = 0
    '    End Sub

    '    Public Sub GetNextPoint(ByRef x As Double, ByRef y As Double)
    '        index += 1
    '        If index = NumberOfPoints Then
    '            index = 1
    '        End If

    '        If Not vertical Then
    '            x = currentX
    '            y = data(index)
    '        Else
    '            x = data(index)
    '            y = currentX
    '        End If

    '        currentX += 1
    '    End Sub

    '    Private Shared Function GenerateSineWave(ByVal xRange As Integer, ByVal yRange As Integer) As Double()
    '        If xRange < 0 Then
    '            Throw New ArgumentOutOfRangeException("xRange")
    '        End If

    '        If yRange < 0 Then
    '            Throw New ArgumentOutOfRangeException("yRange")
    '        End If

    '        Dim wave(xRange) As Double
    '        Dim i As Integer
    '        For i = 0 To xRange
    '            wave(i) = yRange / 2 * (1 - CType(Math.Sin(i * 2 * Math.PI / (xRange - 1)), Single))
    '        Next

    '        Return wave
    '    End Function
    'End Class


    'Private Sub sampleTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If NumericEditList.Count > 0 Then
    '        For Each NumericEditItem In NumericEditList
    '            NumericEditItem.Value = GetNewValue(NumericEditItem.Range, NumericEditItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
    '        Next
    '    End If

    '    If SwitchList.Count > 0 Then
    '        'ВНИМАНИЕ для SwitchList надо регистрировать обработчик события в котором устанавливается значение порта
    '        For Each SwitchItem In SwitchList
    '            If SwitchItem.Value Then
    '                SwitchItem.CaptionBackColor = Color.Red
    '            Else
    '                SwitchItem.CaptionBackColor = Color.Transparent
    '            End If
    '            SwitchItem.Caption = SwitchItem.Value.ToString
    '        Next
    '    End If

    '    If LedList.Count > 0 Then
    '        For Each LedItem In LedList
    '            LedItem.Value = Not LedItem.Value
    '        Next
    '    End If

    '    '**********************************
    '    If KnobList.Count > 0 Then
    '        For Each KnobItem In KnobList
    '            KnobItem.Caption = KnobItem.Value.ToString
    '        Next
    '    End If

    '    If SlideList.Count > 0 Then
    '        For Each SlideItem In SlideList
    '            SlideItem.Caption = SlideItem.Value.ToString
    '        Next
    '    End If
    '    '**********************************

    '    If TankList.Count > 0 Then
    '        For Each TankItem In TankList
    '            TankItem.Value = GetNewValue(TankItem.Range, TankItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
    '        Next
    '    End If

    '    If GaugeList.Count > 0 Then
    '        For Each GaugeItem In GaugeList
    '            GaugeItem.Value = GetNewValue(GaugeItem.Range, GaugeItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
    '        Next
    '    End If

    '    If ThermometerList.Count > 0 Then
    '        For Each ThermometerItem In ThermometerList
    '            ThermometerItem.Value = GetNewValue(ThermometerItem.Range, ThermometerItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
    '        Next
    '    End If

    '    If MeterList.Count > 0 Then
    '        For Each MeterItem In MeterList
    '            MeterItem.Value = GetNewValue(MeterItem.Range, MeterItem.Value, 10, raisedWithThickNeedleMeterValueIncreasing)
    '        Next
    '    End If

    '    If WaveformGraphList.Count > 0 Then
    '        Dim x As Double
    '        Dim y As Double
    '        data.GetNextPoint(x, y)
    '        For Each WaveformGraphItem In WaveformGraphList
    '            'If optionVerticalCheckBox.Checked Then
    '            '    WaveformGraphItem.PlotXAppend(x)
    '            'Else
    '            WaveformGraphItem.PlotYAppend(y)
    '            'End If
    '        Next
    '    End If

    '    If ScatterGraphList.Count > 0 Then
    '        Dim x As Double
    '        Dim y As Double
    '        data.GetNextPoint(x, y)


    '        Const numberOfPoints As Integer = 360

    '        Dim dataR(numberOfPoints) As Double
    '        Dim dataP(numberOfPoints) As Double
    '        Dim dataX(numberOfPoints) As Double
    '        Dim dataY(numberOfPoints) As Double

    '        ' вычислить полярные координаты.
    '        For i As Integer = 0 To numberOfPoints
    '            dataP(i) = i * 10
    '            dataR(i) = Math.Pow(i, 2) / 70000
    '        Next

    '        ' Конвертировать полярные координаты в XY координаты.
    '        For i As Integer = 0 To numberOfPoints
    '            Dim current As Double = (dataP(i) / numberOfPoints) * TwoPi
    '            dataX(i) = Math.Cos(current) * dataR(i) * y
    '            dataY(i) = Math.Sin(current) * dataR(i) * y
    '        Next

    '        For Each ScatterGraphItem In ScatterGraphList
    '            ScatterGraphItem.Plots(0).PlotXY(dataX, dataY)
    '        Next
    '    End If

    'End Sub
#End Region

End Class


''' <summary>
''' пользовательскле исключение на все случаи применения
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class MyException
    Inherits Exception
    Implements ISerializable

    Private ReadOnly _exceptionData As Double = 0.0 ' дополнительные данные для исключения
    Public ReadOnly Property ExceptionData() As Double
        Get
            Return _exceptionData
        End Get
    End Property

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(exceptionData As Double, message As String)
        MyBase.New(message)
        _exceptionData = exceptionData
    End Sub

    Public Sub New(exceptionData As Double, message As String, innerException As Exception)
        MyBase.New(message, innerException)
        _exceptionData = exceptionData
    End Sub

    ' serialization handlers
    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
        ' deserialize
        _exceptionData = info.GetDouble("MyExceptionData")
    End Sub

    <SecurityPermission(SecurityAction.Demand, SerializationFormatter:=True)>
    Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        ' serialize
        MyBase.GetObjectData(info, context)
        info.AddValue("MyExceptionData", _exceptionData)
    End Sub
End Class

'''' <summary>
'''' Записать текстовое представление объекта PropertiesControlBase из свойства aControl.Tag созданного контрола.
'''' Это базовый класс для всех индивидуальных настроек контролов отображаемых в PropertyGrid.
'''' Затем десериализовать его в Hashtable и преобразовать в базовый класс настроек.
'''' </summary>
'''' <param name="aControl"></param>
'''' <param name="errors"></param>
'''' <returns></returns>
'''' <remarks></remarks>
'Private Function GetControlBaseFromTag(ByVal aControl As Object, ByVal errors As List(Of String)) As PropertiesControlBase
'    Dim propControlBase As PropertiesControlBase = Nothing

'    If aControl.Tag Is Nothing OrElse aControl.Tag = "" Then
'        errors.Add("Контрол " & aControl.Name & " имеет пустой Tag")
'        Return Nothing
'    Else
'        Try
'            ' записать строку в файл
'            Using sw As StreamWriter = New StreamWriter(strTempFileSoap)
'                sw.Write(aControl.Tag)
'                sw.Close()
'            End Using

'            ' считать с преобразованием
'            Dim HashtableControlBase As Object = DeserializePropertiesControlBaseFromXML(strTempFileSoap, errors)
'            If HashtableControlBase IsNot Nothing Then
'                propControlBase = DirectCast(HashtableControlBase, PropertiesControlBase)
'            End If
'        Catch ex As Exception
'            errors.Add(ex.Message)
'            Return Nothing
'        End Try

'        Return propControlBase
'    End If
'End Function


''перегруженные версии
'Private Function НайтиИндексВМассивеПараметров(ByVal mPropertiesControlBase As ScatterGraphProperties, ByVal errors As List(Of String)) As Boolean
'    Dim indexParam As Integer = Array.IndexOf(FormsPanelManager.именаПараметровДляКонтрола, mPropertiesControlBase.ИмяКаналаОсиХ)

'    If indexParam = -1 OrElse indexParam = 0 Then
'        errors.Add("Для контрола с именем <" & mPropertiesControlBase.Name & "> и типа <" & mPropertiesControlBase.Тип.ToString & "> задан параметр: " & mPropertiesControlBase.ИмяКаналаОсиХ & vbCrLf & "В каналах опроса такого имени нет.")
'        'Me._tableLP.GrowStyle = DirectCast([Enum].Parse(GetType(TableLayoutPanelGrowStyle), Me._combGrowStyle.SelectedItem.ToString()), TableLayoutPanelGrowStyle)
'        '[Enum].Parse(GetType(EnumControl), mPropertiesControlBase.Тип)
'        Return False
'    Else
'        mPropertiesControlBase.ИндексКаналаОсиХВМассивеПараметров = ArrСписокПараметровКонтрол(indexParam) 'ИмяКаналаОсиХ

'        indexParam = Array.IndexOf(FormsPanelManager.именаПараметровДляКонтрола, mPropertiesControlBase.ИмяКаналаОсиУ)
'        If indexParam = -1 OrElse indexParam = 0 Then
'            errors.Add("Для контрола с именем <" & mPropertiesControlBase.Name & "> и типа <" & mPropertiesControlBase.Тип.ToString & "> задан параметр: " & mPropertiesControlBase.ИмяКаналаОсиУ & vbCrLf & "В каналах опроса такого имени нет.")
'            Return False
'        Else
'            mPropertiesControlBase.ИндексКаналаОсиУВМассивеПараметров = ArrСписокПараметровКонтрол(indexParam) 'ИмяКаналаОсиУ
'        End If
'        Return True
'    End If
'End Function