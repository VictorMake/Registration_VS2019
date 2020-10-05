Imports System.Collections.Generic
Imports NationalInstruments.DAQmx
Imports System.Threading
Imports MathematicalLibrary

' Формы загружаются  как дочерние.
' Проверка каналов в зависимости в режиме регистратор или клиент была запущена программа (источник проверки каналов один КонфигурироватьВсеФормы, а там ЗаполнитьСпискиПараметровОтСервера)
' а клиент получил данные (возможно при изменении количества полученных каналов придется заново пройти процедуру конфигурации КонфигурироватьВсеФормы)
' Соответствие параметров к наличию железа (выполняется сразу), иначе сообщение и выгрузка формы,
' затем цикл проверки повторяется с оставшимися формами.
' Если при загрузке ошибок нет, то формируется задача на DAQ Input и Output (эти задачи формируются здесь).
' Логическая переменная о окончании конфигурировании устанавливается в истину (blnПодпискаНаСобытиеСбораВключена = True).
' В событиях сбора по коллекции форм панелей послать массив собраных значений каналов (имя, индекс, значение) 
' и ответ формы об значениях цифровых и аналоговых выходов.
' При выгрузки какого либо окна - КонфигурироватьВсеФормы переменная конфигурировании в False (blnПодпискаНаСобытиеСбораВключена = False),
' производится заново конфигурация, и в случае успеха переменная конфигурировании в True(blnПодпискаНаСобытиеСбораВключена = True).
' При выгрузке родительской формы frmMainMdi сбрасывается выходы портов.
' При выгрузке форме портов управления (DAQ Input и Output) также сбрасываются порты (а если на режиме кто-то закрыл форму? 
' с другой стороны и оставлять порты в прежнем состоянии нельзя т.к. при закрытии формы реле остануться включенными)
' поэтому лучше обнулить.
' В процедуре конфигурирования проверяются наличие каналов управления железа, наличие имен каналов в сборщике, иначе сброс.
' Сигналы на выходе порта устанавливаются в соответствии с включением тумблера.
' Не допускается применение порта платы в сборе как вход в базе ChannelDigitalInput и этот же порт, как выход.
' В плате подклюенной к корзине SCXI не все порты доступны для управления.

''' <summary>
''' Сервисный класс управления функциональными панелями.
''' В программе используется как статический класс 
''' (все экземпляры имеют доступ к одному месту хранения).
''' </summary>
''' <remarks></remarks>
Friend Class FormsPanelManager
    Implements IEnumerable

    Public IsClosingApplication As Boolean 'закрытие Всего Приложения устанавливается в frmMainMDI_FormClosed
    Public MyDigitalInputsMotoristPanel As New DigitalInputsMotoristPanel
    Public NamesParametersForControl() As String
    ' здесь 2 коллекции задачи управления вместе DI, DO, и AO
    Public DigitalPortsDictionary As Dictionary(Of String, DigitalPort)
    Public AnalogOutputsDictionary As Dictionary(Of String, AnalogOutput)

    Private mCollectionFormPanelMotorist As New Dictionary(Of String, FormBasePanelMotorist) ' внутренняя коллекция для управления формами
    Private deviceList As New List(Of String)
    Private physicalPortList As New List(Of String)
    Private ReadOnly physicalPortAnalogOutputList As New List(Of String)
    Private ReadOnly physicalChannelList As New List(Of String)
    Private digitalWriteTaskList As New List(Of String)
    Private lineCount As New List(Of Integer)

    ' внутренний счетчик для подсчета созданных форм можно использовать в заголовке
    Private mFormsCreated As Integer = 0
    ''' <summary>
    ''' проверка Железа Проведена
    ''' </summary>
    Private isCheckDeviceCompleted As Boolean
    Private isTaskRunningDigitalInputPanel As Boolean
    Private isDigitalInputPanel As Boolean
    Private myTaskDigitalInput As Task
    Private waveformDigitalInput As DigitalWaveform()
    Private myDigitalReader As DigitalMultiChannelReader

    Private myTaskAOutput As Task
    Private WithEvents StatusCheckTimer As Windows.Forms.Timer
    Private startAOTask As Boolean = False
    Private isAOutputPanel As Boolean

#Region "Form"
    ''' <summary>
    ''' число текущих загруженных форм
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>    
    Public ReadOnly Property FormPanelMotoristCount() As Integer
        Get
            Return mCollectionFormPanelMotorist.Count
        End Get
    End Property

    Public ReadOnly Property CollectionFormPanelMotorist() As Dictionary(Of String, FormBasePanelMotorist)
        Get
            Return mCollectionFormPanelMotorist
        End Get
    End Property

    Public ReadOnly Property Item(ByVal vntIndexKey As String) As FormBasePanelMotorist
        Get
            Return mCollectionFormPanelMotorist.Item(vntIndexKey)
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mCollectionFormPanelMotorist.GetEnumerator
    End Function

    'Public Sub Remove(ByRef vntIndexKey As String) 'Shared убрал так как надо удалять закрывая саму форму, а она уже удаляет из коллекции
    '    'удаление по номеру или имени или объекту?
    '    'если целый тип то по плавающему индексу, а если строковый то по ключу
    '    m_КоллекцияПанелейМоториста.Remove(vntIndexKey)
    '    FormsPanelManager.mFormsCreated -= 1
    'End Sub

    Public Sub ClearFormPanelMotorist()
        mCollectionFormPanelMotorist.Clear()
        gFormsPanelManager.mFormsCreated = 0
    End Sub

    Protected Overrides Sub Finalize()
        mCollectionFormPanelMotorist = Nothing
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Реализация сервиса (Add) добавления новой формы в коллекцию.
    ''' В случае успешного считывания, форма добавляется в коллекцию
    ''' и заново проверяются все формы в совокупе.
    ''' После чего перезапускаются задачи сбора железа.    
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="namePanelMotorist"></param>
    ''' <returns></returns>
    Public Function CreateNewFormPanelMotorist(ByVal fileName As String, ByVal namePanelMotorist As String) As Boolean ' Public Function Add
        If Not isCheckDeviceCompleted Then PopulateDeviceDictionary()

        If mCollectionFormPanelMotorist.ContainsKey(namePanelMotorist) Then
            Const caption As String = "Загрузка новой панели"
            Dim text As String = $"Панель с именем <{namePanelMotorist}> уже загружена!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False ' выйти из функции
        End If

        Try
            If FormPanelMotoristCount = 0 Then PopulateListParametersFromServer() ' возможно это в первый раз, поэтому обновить

            Dim errorsText As String = Nothing
            Dim frm As New FormBasePanelMotorist() With {.NameMotoristPanel = namePanelMotorist}
            ' загрузить хост
            Dim myBasicHostLoaderRun As New BasicHostLoaderRun(fileName, frm)
            myBasicHostLoaderRun.PerformLoad2(errorsText)

            mCollectionFormPanelMotorist.Add(frm.NameMotoristPanel, frm) 'стало
            gFormsPanelManager.mFormsCreated += 1

            If errorsText IsNot Nothing Then
                Const caption As String = "Загрузка панели"
                'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, errorsText))
                frm.ShowLosdErrors(errorsText)
            Else
                RegistrationEventLog.EventLog_AUDIT_SUCCESS("Добавление функциональной панели: " & namePanelMotorist)
            End If

            ' и только если форма успешно воссоздана,
            ' то далее провести проверку на корректность работы всех форм
            СonfigureAllMotoristPanels(True)

            If mCollectionFormPanelMotorist.ContainsKey(namePanelMotorist) Then
                ' добавить обработчик события Closed новой формы, которое используется здес,
                ' чтобы знать когда она закрывается и обработать из централизованного места
                AddHandler frm.Closed, AddressOf gFormsPanelManager.PanelBaseForm_Closed
                ' добавить обработчик события SaveWhileClosingCancelled, чтобы
                ' использовать прерывания Cancel button, когда было напоминание сохранения несохраненных данных
                ' AddHandler frm.SaveWhileClosingCancelled, AddressOf Forms.PanelBaseForm_SaveWhileClosingCancelled
                ' добавить обработчик события ExitApplicaiton чтобы знать, когда необходимо выгрузить приложение выбрав Exit menu из формы
                ' AddHandler frm.ExitApplication, AddressOf Forms.PanelBaseForm_ExitApplication

                frm.Show() ' проверка успешна, значит показать форму 
                RestartAllTask()
                Return True
            Else
                RestartAllTask()
                Return False
            End If

        Catch exp As Exception
            Dim caption As String = $"Процедура <{NameOf(CreateNewFormPanelMotorist)}>"
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            If gFormsPanelManager.FormPanelMotoristCount = 0 Then
                ' передать снова ошибку в Main где возможно выгрузить процесс
                Throw ' exp
            End If
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Конфигурировать Все Формы
    ''' Проверка новой панели на корректную работу совместно с другими уже загруженными в коллекцию.
    ''' </summary>
    ''' <param name="isCheckForNewAddedPanel"></param>
    ''' <remarks></remarks>
    Public Sub СonfigureAllMotoristPanels(ByVal isCheckForNewAddedPanel As Boolean)
        PopulateListParametersFromServer()
        IsSubscriptionOnEventsAcquisitionEnabled = False

        If isDigitalInputPanel Then StopTaskDigitalInputPanel()
        isDigitalInputPanel = False

        If isAOutputPanel Then StopTaskAOutputPanel()
        isAOutputPanel = False

        ' очистить логические пометки используемых выходных портов (для входных не надо)
        If DigitalPortsDictionary IsNot Nothing Then
            For Each itemPort As DigitalPort In DigitalPortsDictionary.Values
                itemPort.IsPortOutput = True

                For I As Integer = 0 To itemPort.IsLineOutput.Count - 1
                    itemPort.IsLineOutput(I) = False
                Next
            Next
            ' очистить коллекцию используемых линий портов Ввода
            MyDigitalInputsMotoristPanel.Clear()
        End If

        If AnalogOutputsDictionary IsNot Nothing Then
            ' очистить логические пометки используемых портов аналогового вывода генератора
            For Each itemPort As AnalogOutput In AnalogOutputsDictionary.Values
                itemPort.WhoUseLine = WhoUseLineAOLine.Nobody
                itemPort.Subscriber = New Subscriber("нет", "нет", 0)
            Next
        End If

        Try
            ' выгрузить за раз все формы которые некорректны
            Dim frm As FormBasePanelMotorist
            Dim nameForms As New List(Of String)

            For Each keyName As String In mCollectionFormPanelMotorist.Keys
                nameForms.Add(keyName)
            Next

            For Each itemName As String In nameForms
                frm = mCollectionFormPanelMotorist.Item(itemName)

                If frm.PopulatePanelByControls() Then
                    frm.IsTestSuccess = True
                Else
                    ' удалить возможно добавленные Подписчики для этой неудачной формы
                    If MyDigitalInputsMotoristPanel.Count > 0 Then
                        ' цифровой Вход Панели Для Удаления
                        Dim ItemsDigitalInputForDelete As New List(Of String)

                        For Each itemItemDigitalInput As ItemDigitalInput In MyDigitalInputsMotoristPanel.CollectionDigitalInput.Values
                            For Each itemSubscriber As Subscriber In itemItemDigitalInput.Subscribers
                                If itemSubscriber.NameMotoristPanel = itemName Then
                                    ItemsDigitalInputForDelete.Add(itemItemDigitalInput.ToString)
                                End If
                            Next
                        Next

                        If ItemsDigitalInputForDelete.Count > 0 Then
                            For Each tempName As String In ItemsDigitalInputForDelete
                                MyDigitalInputsMotoristPanel.CollectionDigitalInput.Remove(tempName)
                            Next
                        End If
                    End If

                    If isCheckForNewAddedPanel Then
                        ' если форма вновь добавляемая то,
                        ' пока форма не создана и нет подписки на событие закрытия 
                        ' уничтожать не в PanelBaseForm_Closed а руками
                        frm.CloseOnFailureConfiguration()
                        frm.Close()
                        mCollectionFormPanelMotorist.Remove(frm.NameMotoristPanel) 'стало
                        gFormsPanelManager.mFormsCreated -= 1
                    Else
                        ' frm.Close() должно вызваться событие PanelBaseForm_Closed а в нем КонфигурироватьВсеФормы, что вызовет рекурсию
                        ' поэтому чистим вручную
                        RemoveHandler frm.Closed, AddressOf gFormsPanelManager.PanelBaseForm_Closed
                        frm.Close()
                        MDITabPanelMotoristForm.UnCheckListMenuByMotoristPanels(frm.NameMotoristPanel)
                        mCollectionFormPanelMotorist.Remove(frm.NameMotoristPanel)
                        gFormsPanelManager.mFormsCreated -= 1
                    End If
                End If
            Next

            'srtFormsName.Clear()
            'For Each strName As String In m_КоллекцияПанелейМоториста.Keys
            '    srtFormsName.Add(strName)
            'Next

            'If m_КоллекцияПанелейМоториста.Count > 0 Then
            '    For Each strName As String In srtFormsName
            '        frm = m_КоллекцияПанелейМоториста.Item(strName)
            '        frm.Close()
            '    Next
            'End If
            'Return True

            ' Заново конфигурировать железо и запустить задачи
            If FormPanelMotoristCount > 0 Then IsSubscriptionOnEventsAcquisitionEnabled = True
            If MyDigitalInputsMotoristPanel.Count > 0 Then isDigitalInputPanel = True

            If AnalogOutputsDictionary IsNot Nothing Then
                For Each itemPort As AnalogOutput In AnalogOutputsDictionary.Values
                    If itemPort.WhoUseLine = WhoUseLineAOLine.Generator Then
                        isAOutputPanel = True
                        Exit For
                    End If
                Next
            End If

        Catch exp As Exception
            Dim caption As String = $"Процедура <{NameOf(СonfigureAllMotoristPanels)}>"
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Заполнить Списки Параметров От Сервера
    ''' вызывается из CharacteristicForRegime
    ''' </summary>
    Public Sub PopulateListParametersFromServer()
        'ReDim_NamesParametersForControl(UBound(IndexParametersForControl))
        Re.Dim(NamesParametersForControl, UBound(IndexParametersForControl))
        NamesParametersForControl(0) = MissingParameter

        For I = 1 To UBound(IndexParametersForControl)
            NamesParametersForControl(I) = ParametersType(IndexParametersForControl(I)).NameParameter
        Next

        'ReDim_ParameterAccumulate(UBound(ParametersType)) ' обнулить массив
        Re.Dim(ParameterAccumulate, UBound(ParametersType)) ' обнулить массив
    End Sub

    ''' <summary>
    ''' Это событие получено, когда форма была признана корректной и закрывается.
    ''' После этого надо заново проверить конфигурацию.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PanelBaseForm_Closed(ByVal sender As Object, ByVal e As EventArgs)
        IsSubscriptionOnEventsAcquisitionEnabled = False ' на всякий случай хотя в КонфигурироватьВсеФормы есть
        If isDigitalInputPanel Then StopTaskDigitalInputPanel() ' остановить перед удалением формы 
        If isAOutputPanel Then StopTaskAOutputPanel()

        Try
            Dim frm As FormBasePanelMotorist = CType(sender, FormBasePanelMotorist)

            ' Удалить обработчики событий, добавленных при создании формы
            RemoveHandler frm.Closed, AddressOf gFormsPanelManager.PanelBaseForm_Closed
            'RemoveHandler frm.SaveWhileClosingCancelled, AddressOf Forms.PanelBaseForm_SaveWhileClosingCancelled
            'RemoveHandler frm.ExitApplication, AddressOf Forms.PanelBaseForm_ExitApplication

            ' вызвать функцию очистки 
            ' FormsPanelManager.FormClosed(frm)
            ' удалить форму которая закрывается из внутренней коллекции
            ' m_КоллекцияПанелейМоториста.Remove(frm.GetHashCode.ToString())
            MDITabPanelMotoristForm.UnCheckListMenuByMotoristPanels(frm.NameMotoristPanel)

            mCollectionFormPanelMotorist.Remove(frm.NameMotoristPanel)
            gFormsPanelManager.mFormsCreated -= 1

            ' если не имеется более форм, то выгрузить процесс
            ' это вызывается только из добавленных в коллекцию форм
            ' корневая форма это событие не вызывает
            'If m_КоллекцияПанелейМоториста.Count = 0 Then
            '    Application.Exit()
            ''End If
            'If m_КоллекцияПанелейМоториста.Count = 0 Then
            '    blnПодпискаНаСобытиеСбораВключена = False
            'End If

            If Not IsClosingApplication Then
                СonfigureAllMotoristPanels(False)
                RestartAllTask()
            End If
        Catch exp As Exception
            Dim caption As String = $"Процедура <{NameOf(PanelBaseForm_Closed)}>"
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Перезапустить Все Задачи
    ''' Обновить события в индикаторах для управляющих контролов
    ''' </summary>
    Public Sub RestartAllTask()
        If isDigitalInputPanel Then StartTaskDigitalInputPanel()
        If isAOutputPanel Then StartTaskAOutputPanel()

        If AnalogOutputsDictionary IsNot Nothing Then
            'сгенерировать события для обновления выходного напряжения
            Dim AllChannels = From AOutputPort In AnalogOutputsDictionary.Values
                              Where AOutputPort.WhoUseLine = WhoUseLineAOLine.VoltOut
                              Select AOutputPort

            For Each itemAOutputPort As AnalogOutput In AllChannels
                Dim mSlide As WindowsForms.Slide = mCollectionFormPanelMotorist(itemAOutputPort.Subscriber.NameMotoristPanel).slideList(itemAOutputPort.Subscriber.IndexItem)
                'mSlide.Value = mSlide.Range.Minimum
                mCollectionFormPanelMotorist(itemAOutputPort.Subscriber.NameMotoristPanel).Slide_AfterChangeValue(mSlide, New AfterChangeNumericValueEventArgs(mSlide.Value, mSlide.Value, UI.Action.Programmatic))
            Next
        End If
    End Sub

    ''' <summary>
    ''' Узнать Наличие Железа
    ''' </summary>
    Private Sub PopulateDeviceDictionary()
        'узнать конфигурацию платы
        '(Led.Tag = "SC1Mod<slot#>/port" & mНомерПорта.ToString & "/line" & i.ToString) '"SC" & НомерУстройства & "Mod<slot#>/port0/lineN" 'Dev0/port1/line0:2
        '"SC" & НомерУстройства может ссылаться на на "Dev" & НомерУстройства 
        ' поэтому если есть ссылка "SC" & НомерУстройства = "Dev" & НомерУстройства , то порты с "Dev" не показывать
        deviceList.AddRange(DaqSystem.Local.Devices)
        physicalPortList.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort Or PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External))
        physicalPortAnalogOutputList.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External))

        Dim wordsPhysicalChannel As String() = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine Or PhysicalChannelTypes.DILine, PhysicalChannelAccess.External)

        physicalChannelList.AddRange(wordsPhysicalChannel)
        LinqLineCount(wordsPhysicalChannel, physicalPortList, lineCount)

        If physicalPortList.Count > 0 Then
            ' заполнить коллекцию портов
            DigitalPortsDictionary = New Dictionary(Of String, DigitalPort)

            For I = 0 To physicalPortList.Count - 1
                DigitalPortsDictionary.Add(physicalPortList(I).ToString, New DigitalPort(physicalPortList(I).ToString, lineCount(I) - 1))
            Next
        End If

        If physicalPortAnalogOutputList.Count > 0 Then
            ' заполнить коллекцию аналоговых каналов вывода
            AnalogOutputsDictionary = New Dictionary(Of String, AnalogOutput)

            For I = 0 To physicalPortAnalogOutputList.Count - 1
                AnalogOutputsDictionary.Add(physicalPortAnalogOutputList(I).ToString, New AnalogOutput(physicalPortAnalogOutputList(I).ToString))
            Next
        End If

        CheckCountsDigitalWriteTask(deviceList, physicalPortList, digitalWriteTaskList)
        isCheckDeviceCompleted = True
    End Sub

#End Region

#Region "TaskDigitalInputPanel"
    ''' <summary>
    ''' Используется для составления задачи
    ''' </summary>
    Public Class DigitalPort
        Implements IEnumerable
        ''' <summary>
        ''' Линия Используется Для Вывода
        ''' </summary>
        ''' <returns></returns>
        Public Property IsLineOutput() As Boolean()
        ''' <summary>
        ''' Порт Для Вывода
        ''' </summary>
        ''' <returns></returns>
        Public Property IsPortOutput() As Boolean

        Public ReadOnly Property AllPortLineDic() As Dictionary(Of String, String)
        Public Property NameDigitalPort() As String
        Public Property LineCount() As Integer

        Public Sub New(ByVal name As String, ByVal inLineCount As Integer)
            Me.NameDigitalPort = name
            Me.LineCount = inLineCount
            AllPortLineDic = New Dictionary(Of String, String)

            'ReDim_IsLineOutput(inLineCount)
            Re.Dim(IsLineOutput, inLineCount)

            For I As Integer = 0 To inLineCount
                AllPortLineDic.Add(I.ToString, $"{Me.NameDigitalPort}/line{I}")
            Next

            IsPortOutput = True
        End Sub

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            GetEnumerator = AllPortLineDic.GetEnumerator
        End Function

        Shadows ReadOnly Property ToString() As String 'Protected
            Get
                Return NameDigitalPort
            End Get
        End Property

        'Public Sub Remove(ByRef vntIndexKey As Single)
        '    'удаление по номеру или имени или объекту?
        '    'если целый тип то по плавающему индексу, а если строковый то по ключу
        '    ВеличиныЗагрузок.Remove(vntIndexKey)
        'End Sub

        'Public Sub Clear()
        '    ''здесь удаление по ключу, а он строковый
        '    'не работает
        '    'Dim oneInst As Условие
        '    'For Each oneInst In mCol
        '    '    mCol.Remove(oneInst.ID.ToString)
        '    'Next
        '    'Dim I As Integer
        '    'With mCol
        '    '    For I = .Count To 1 Step -1
        '    '        .Remove(I)
        '    '    Next
        '    'End With
        '    ВеличиныЗагрузок.Clear()
        'End Sub

        'Public ReadOnly Property ToArray() As Object()
        '    Get
        '        Dim arrayObject(ВеличиныЗагрузок.Count - 1) As Object
        '        Dim I As Integer
        '        For Each tempИмяСобытия As ИмяСобытия In ВеличиныЗагрузок.Values
        '            arrayObject(I) = tempИмяСобытия
        '            I += 1
        '        Next
        '        Return arrayObject
        '    End Get
        'End Property

        'Protected Overrides Sub Finalize()
        '    ВеличиныЗагрузок = Nothing
        '    MyBase.Finalize()
        'End Sub
    End Class

    Private dCallback As AsyncCallback
    Private Const samplesPerChannelNumeric As Integer = 1

    ''' <summary>
    ''' Запуск задачи сбора с дискретных входов: линий портов
    ''' </summary>
    Private Sub StartTaskDigitalInputPanel()
        Try
            ' создать задачу
            If isTaskRunningDigitalInputPanel Then
                isTaskRunningDigitalInputPanel = False
                If myTaskDigitalInput IsNot Nothing Then
                    myTaskDigitalInput.Stop()
                    myTaskDigitalInput.Dispose()
                    myTaskDigitalInput = Nothing
                End If
            End If

            myTaskDigitalInput = New Task("diTask")

            For Each loopItemDigitalInput As ItemDigitalInput In MyDigitalInputsMotoristPanel.CollectionDigitalInput.Values
                ' подключить только измеряемые каналы
                ' создать виртуальный канал
                ' добавлять только те каналы, которые помечены на сбор не сделал
                ' добавляются все цифровые каналы, значит сбор по всем каналам
                'If Array.IndexOf(arrНаименование2, myItemЦифровойВходПанели.НаименованиеПараметра) <> -1 Then
                'SC1Mod1/port0/line31 
                'Dev1/port0/line0, 
                'If myItemЦифровойВходПанели.НомерМодуляКорзины = "" Then
                '    strСтрока = "Dev" & myItemЦифровойВходПанели.НомерУстройства & "/port" & myItemЦифровойВходПанели.НомерПорта & "/line" & myItemЦифровойВходПанели.НомерЛинии
                'Else
                '    strСтрока = "SC" & myItemЦифровойВходПанели.НомерУстройства & "Mod" & myItemЦифровойВходПанели.НомерМодуляКорзины & "/port" & myItemЦифровойВходПанели.НомерПорта & "/line" & myItemЦифровойВходПанели.НомерЛинии
                'End If
                myTaskDigitalInput.DIChannels.CreateChannel(loopItemDigitalInput.ToString, "", ChannelLineGrouping.OneChannelForEachLine)
            Next

            ' создать канал
            'myTaskDigitalInput.DIChannels.CreateChannel(strСтрока, "", ChannelLineGrouping.OneChannelForEachLine)
            'myTaskDigitalInput.Timing.ConfigureSampleClock("", CType(samplesClockRate, Double), _
            '                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, CType(samplesClockRate, Integer))

            ' фактическая частота сбора
            'myTaskDigitalInput.Timing.ConfigureSampleClock(String.Empty, CType(intЧастотаФонового, Double), _
            '    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, 1)

            Dim clockSource As String = String.Empty 'To use the internal clock of the device, set this value to Empty. "/Dev1/PFI0"
            Dim sampleRate As Double = 2 ' было 5 герц сделал 2 герц

            myTaskDigitalInput.Timing.ConfigureSampleClock(clockSource, sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannelNumeric)
            myTaskDigitalInput.Timing.SampleTimingType = SampleTimingType.OnDemand
            'On Demand нужен наверно для платы, а для корзины наверно не нужен

            ' Set timeout to 10 s
            'myTaskDigitalInput.Stream.Timeout = 10000

            ' проверить корректность задачи
            myTaskDigitalInput.Control(TaskAction.Verify)

            'myDigitalReader = New DigitalSingleChannelReader(myTaskDigitalInput.Stream)
            myDigitalReader = New DigitalMultiChannelReader(myTaskDigitalInput.Stream) With {.SynchronizeCallbacks = False} 'True не даёт обновляться другим окнам

            'myDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), myTaskDigitalInput)
            ' читать одну выборку
            'myDigitalReader.BeginReadWaveform(1, New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), myTaskDigitalInput)

            dCallback = New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput)
            myDigitalReader.BeginReadWaveform(samplesPerChannelNumeric, dCallback, myTaskDigitalInput)
            isTaskRunningDigitalInputPanel = True
        Catch ex As DaqException
            Dim caption As String = $"Процедура <{NameOf(StartTaskDigitalInputPanel)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            ' очистить задачу
            StopTaskDigitalInputPanel()
        End Try
    End Sub

    ''' <summary>
    ''' Делегат обратного вызова при чтении сбора
    ''' </summary>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Private Sub OnCallbackDataReadyDigitalInput(ByVal result As IAsyncResult)
        Try
            If isTaskRunningDigitalInputPanel Then
                If myTaskDigitalInput IsNot Nothing AndAlso myDigitalReader IsNot Nothing Then
                    If myTaskDigitalInput Is result.AsyncState Then
                        'Dim data As Boolean(,) = myDigitalReader.EndReadSingleSampleMultiLine(result)
                        'DisplayData(data)
                        ' читать samplesPerChannel=1 значение
                        'WaveformDigitalInput = myDigitalReader.ReadWaveform(1)
                        ' так наверно правильно
                        waveformDigitalInput = myDigitalReader.EndReadWaveform(result)

                        DisplayData(waveformDigitalInput)
                        ' заново запустить
                        ' Thread.Sleep(20)'делает задержку в основной программе сбора
                        'Application.DoEvents() 'то же ерунда хотя и даёт возможность более отзывчивой формы, но от ошибок переполнения стека не спасёт
                        'If myDigitalReader IsNot Nothing Then myDigitalReader.BeginReadSingleSampleMultiLine(New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), myTaskDigitalInput)
                        'Application.DoEvents() ' то же ерунда хотя и даёт возможность более отзывчивой формы, но от ошибок переполнения стека не спасёт

                        'myDigitalReader.BeginReadWaveform(1, New AsyncCallback(AddressOf OnCallbackDataReadyDigitalInput), myTaskDigitalInput)
                        myDigitalReader.BeginReadWaveform(samplesPerChannelNumeric, dCallback, myTaskDigitalInput)
                    End If
                End If
            End If
        Catch ex As DaqException
            Dim caption As String = $"Процедура <{NameOf(OnCallbackDataReadyDigitalInput)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
            StopTaskDigitalInputPanel()
        End Try
    End Sub

    ''' <summary>
    ''' Остановить задачу сбора с дискретных входов: линий портов
    ''' </summary>
    Private Sub StopTaskDigitalInputPanel()
        isTaskRunningDigitalInputPanel = False
        myDigitalReader = Nothing

        If myTaskDigitalInput IsNot Nothing Then
            'myTaskDigitalInput.Stop()
            myTaskDigitalInput.Dispose()
        End If
    End Sub

    ''' <summary>
    ''' Включить индикаторные лампы в зависимости от включения линий портов
    ''' </summary>
    ''' <param name="waveform"></param>
    Private Sub DisplayData(ByVal waveform As DigitalWaveform())
        Dim isConditionFire As Boolean ' условие Сработало
        Dim currentLineIndex As Integer = 0
        Dim myLed As WindowsForms.Led

        For Each signal As DigitalWaveform In waveform
            For sample As Integer = 0 To (signal.Signals(0).States.Count - 1)
                isConditionFire = signal.Signals(0).States(sample) = DigitalState.ForceUp

                'For Each myItemЦифровойВходПанели As ItemЦифровойВходПанели In cЦифровыеВходыПанели.КоллекцияItemЦифровыхВходов.Values
                '    If myItemЦифровойВходПанели.НомерИндексаВКоллекции = currentLineIndex Then
                '        For Each myПодписчик As Подписчик In myItemЦифровойВходПанели.Подписчики
                '            'что-то делаем
                '            myLed = m_КоллекцияПанелейМоториста.Item(myПодписчик.ИмяПанели).LedList(myПодписчик.ИндексЭлементаВКоллекции)
                '            If myLed.Value <> УсловиеСработало Then
                '                myLed.Value = УсловиеСработало
                '            End If
                '        Next
                '        Exit For
                '    End If
                'Next

                Dim DigitalInputs = From ItemDigitalInput In MyDigitalInputsMotoristPanel.CollectionDigitalInput.Values
                                    Where ItemDigitalInput.IndexNumber = currentLineIndex
                                    Select ItemDigitalInput

                ' он должен быть один
                For Each ItemDigitalInput In DigitalInputs
                    For Each itemSubscriber As Subscriber In ItemDigitalInput.Subscribers
                        ' что-то делаем
                        myLed = mCollectionFormPanelMotorist.Item(itemSubscriber.NameMotoristPanel).ledList(itemSubscriber.IndexItem)

                        If myLed.Value <> isConditionFire Then myLed.Value = isConditionFire
                    Next
                Next
            Next

            currentLineIndex += 1
        Next
    End Sub

    ''' <summary>
    ''' Цифровые Входы Панели
    ''' Управление подписчиками (функциональными панелями) получающими информацию о состоянии значения какого-то дискретного входа
    ''' </summary>
    Friend Class DigitalInputsMotoristPanel
        Implements IEnumerable

        Private dictionaryItemDigitalInput As Dictionary(Of String, ItemDigitalInput)

        Public Sub New()
            MyBase.New()
            dictionaryItemDigitalInput = New Dictionary(Of String, ItemDigitalInput)
        End Sub

        Public Sub Add(ByVal nameDigitalInput As String, ByVal namePanel As String, ByVal nameLed As String, ByVal indexElement As Integer)
            If Not dictionaryItemDigitalInput.ContainsKey(nameDigitalInput) Then
                'Dim ИндексВКоллекцииВход As Integer = mvarКоллекцияItemЦифровойВходПанели.Count - 1
                'If ИндексВКоллекцииВход < 0 Then 'коллекция пуста
                '    ИндексВКоллекцииВход = 0
                'Else
                '    ИндексВКоллекцииВход += 1
                'End If
                Dim индексВКоллекцииВход As Integer = dictionaryItemDigitalInput.Count
                dictionaryItemDigitalInput.Add(nameDigitalInput, New ItemDigitalInput(nameDigitalInput, индексВКоллекцииВход))
            End If
            ' уже есть в коллекции, значит вызвать экземпляр и добавить в список потребителей
            dictionaryItemDigitalInput(nameDigitalInput).Subscribers.Add(New Subscriber(namePanel, nameLed, indexElement))
        End Sub

        Public ReadOnly Property Item(ByVal key As String) As ItemDigitalInput
            Get
                Return dictionaryItemDigitalInput.Item(key)
            End Get
        End Property

        Public ReadOnly Property CollectionDigitalInput() As Dictionary(Of String, ItemDigitalInput)
            Get
                Return dictionaryItemDigitalInput
            End Get
        End Property

        Public ReadOnly Property Count() As Integer
            Get
                Return dictionaryItemDigitalInput.Count()
            End Get
        End Property

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return dictionaryItemDigitalInput.GetEnumerator
        End Function

        Public Sub Remove(ByRef key As String)
            ' если целый тип то по индексу, а если строковый то по ключу
            dictionaryItemDigitalInput.Remove(key)
        End Sub

        Public Sub Clear()
            dictionaryItemDigitalInput.Clear()
        End Sub

        Protected Overrides Sub Finalize()
            dictionaryItemDigitalInput = Nothing
            MyBase.Finalize()
        End Sub
    End Class

    ''' <summary>
    ''' Item Цифровой Вход Панели
    ''' Дискренный вход в коллекции всех входов в загруженных панелях
    ''' </summary>
    Friend Class ItemDigitalInput
        Private ReadOnly mSubscribers As New List(Of Subscriber)
        Private ReadOnly nameItemDigitalInput As String = ""

        Public Sub New(ByVal nameDigitalInput As String, ByVal inIndexNumber As Integer)
            nameItemDigitalInput = nameDigitalInput
            IndexNumber = inIndexNumber
        End Sub

        ''' <summary>
        ''' Номер Индекса В Коллекции
        ''' </summary>
        ''' <returns></returns>
        Public Property IndexNumber() As Integer

        Shadows ReadOnly Property ToString() As String
            Get
                Return nameItemDigitalInput
            End Get
        End Property

        Friend ReadOnly Property Subscribers() As List(Of Subscriber)
            Get
                Return mSubscribers
            End Get
        End Property
    End Class
#End Region

#Region "TaskAOutputPanel"

#Region "AnalogOutputPort"
    ''' <summary>
    ''' Кто Использует AO Line
    ''' </summary>
    Public Enum WhoUseLineAOLine
        Generator ' Генератор
        VoltOut ' Напряжение
        Nobody ' Никто
    End Enum

    ''' <summary>
    ''' Используется для составления задачи
    ''' </summary>
    Public Class AnalogOutput

        Public Sub New(ByVal name As String)
            NameAnalogOutputPort = name
            Subscriber = New Subscriber("нет", "нет", 0)
            Generator = New Generator(name)
        End Sub
        ''' <summary>
        ''' Линия Используется
        ''' </summary>
        ''' <returns></returns>
        Public Property WhoUseLine() As WhoUseLineAOLine = WhoUseLineAOLine.Nobody

        Public Property NameAnalogOutputPort() As String

        Public Property Subscriber() As Subscriber

        Public Property Generator() As Generator

        Shadows ReadOnly Property ToString() As String
            Get
                Return NameAnalogOutputPort
            End Get
        End Property
    End Class
#End Region

    ''' <summary>
    ''' Запуск задачи выдачи сигнала на аналоговый выход
    ''' </summary>
    Private Sub StartTaskAOutputPanel()
        StatusCheckTimer = New Windows.Forms.Timer With {.Interval = 1000}

        Try
            ' создать задачу и каналы
            If startAOTask Then
                startAOTask = False
                If myTaskAOutput IsNot Nothing Then
                    myTaskAOutput.Stop()
                    myTaskAOutput.Dispose()
                    myTaskAOutput = Nothing
                End If
            End If

            'myTaskAOutput = New Task(PhysicalChannel.Replace("/", "")) '/" & PhysicalChannel)"AOTask"
            myTaskAOutput = New Task("AOTask")

            '        Dim numChannels As Integer = myTaskAOutput.AOChannels.Count
            ' узнать колличество используемых каналов
            Dim generatorChannels = From AOutputPort In AnalogOutputsDictionary.Values
                                    Where AOutputPort.WhoUseLine = WhoUseLineAOLine.Generator
                                    Select AOutputPort

            Dim countChannels As Integer = generatorChannels.Count
            ' число точек в буфере узнаем по первому элементу в коллекции (хотя они могут быть разлмчными в других генераторах)
            Dim numSamples As Integer = generatorChannels(0).Generator.PointsSamplePerBuffer
            Dim samplesPerSecond As Double

            Dim data As Double(,) = New Double(countChannels - 1, numSamples - 1) {}
            Dim I As Integer

            For Each AOutputPort As AnalogOutput In generatorChannels
                Dim tempGenerator As Generator = AOutputPort.Generator
                myTaskAOutput.AOChannels.CreateVoltageChannel(tempGenerator.PhysicalChannel,
                                                              tempGenerator.PhysicalChannel.Replace("/", ""),
                                                              tempGenerator.RangeDacMin,
                                                              tempGenerator.RangeDacMax,
                                                              AOVoltageUnits.Volts)

                myTaskAOutput.Control(TaskAction.Verify)

                ' Вычислить некоторые параметры формы и сгенерировать данные
                ' вместо ТочекГрафикаВБуфере конкретного генератора подставляем numSamples самого первого
                Dim fGen As New FunctionGenerator(myTaskAOutput.Timing,
                                                  tempGenerator.FixFrequency,
                                                  numSamples,
                                                  tempGenerator.CyclesPerBuffer,
                                                  tempGenerator.TypeWaveform,
                                                  tempGenerator.AmplitudeFix)

                samplesPerSecond = fGen.ResultingSampleClockRate
                Dim dataGenerator As Double() = fGen.Data

                For J As Integer = 0 To dataGenerator.Length - 1
                    data(I, J) = dataGenerator(J)
                Next
                I += 1
            Next

            ' сконфигурировать частоту выдачи каждой точки 
            ' вместо конкректного fGen.ResultingSampleClockRate подставляем самый последний  samplesPerSecond
            ' поэтому все частоты будут одинаковы что плохо
            myTaskAOutput.Timing.ConfigureSampleClock("",
                                                       samplesPerSecond,
                                                       SampleClockActiveEdge.Rising,
                                                       SampleQuantityMode.ContinuousSamples)

            ' Проверить задачу перед генерацией сигнала
            myTaskAOutput.Control(TaskAction.Verify)

            ' конфигурировать связь с измерительным устройством
            'сконфигурировать подстройку под амплитуду для улучшения формы сигнала
            myTaskAOutput.AOChannels.All.DacReferenceSource = AODacReferenceSource.Internal
            myTaskAOutput.AOChannels.All.DacReferenceValue = generatorChannels(0).Generator.СоответствиеДиапазону

            ' записать данные в буфер множественного канала
            'Dim writer As New AnalogSingleChannelWriter(myTaskAOutput.Stream)
            Dim writer As New AnalogMultiChannelWriter(myTaskAOutput.Stream)

            writer.WriteMultiSample(False, data) 'fGen.Data)

            ' запуск выдачи сигнала наружу
            myTaskAOutput.Start()
            startAOTask = True
            StatusCheckTimer.Enabled = True
        Catch err As DaqException
            'statusCheckTimer.Enabled = False
            Dim caption As String = $"Процедура <{NameOf(StartTaskAOutputPanel)}>"
            Dim text As String = err.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            'myTaskAOutput.Dispose()
            StopTaskAOutputPanel()
        End Try
    End Sub

    ''' <summary>
    ''' Остановка задачи аналогово выхода
    ''' </summary>
    Private Sub StopTaskAOutputPanel()
        StatusCheckTimer.Enabled = False

        If myTaskAOutput IsNot Nothing Then
            Try
                myTaskAOutput.Stop()
            Catch x As Exception
                Dim caption As String = NameOf(StopTaskAOutputPanel)
                Dim text As String = x.Message
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try

            myTaskAOutput.Dispose()
            myTaskAOutput = Nothing
            startAOTask = False
        End If

        ' теперь надо убрать все оставшиеся напряжения на выходе от генератора
        ' просто записать ноль вольт
        Dim AllChannels = From AOutputPort In AnalogOutputsDictionary.Values
                          Where AOutputPort.WhoUseLine = WhoUseLineAOLine.Generator
                          Select AOutputPort

        For Each AOutputPort As AnalogOutput In AllChannels
            Dim tempAOutputTask As Task = Nothing

            Try
                tempAOutputTask = New Task()
                tempAOutputTask.AOChannels.CreateVoltageChannel(AOutputPort.ToString,
                                                                 "aoChannel",
                                                                 -10,
                                                                 10,
                                                                 AOVoltageUnits.Volts)

                Dim writer As AnalogSingleChannelWriter = New AnalogSingleChannelWriter(tempAOutputTask.Stream)
                tempAOutputTask.Control(TaskAction.Verify)
                writer.WriteSingleSample(True, 0)
            Catch ex As DaqException
                Dim caption As String = $"Процедура <{NameOf(StopTaskAOutputPanel)}>"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            Finally
                tempAOutputTask.Dispose()
            End Try
        Next
    End Sub

    Private Sub StatusCheckTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles StatusCheckTimer.Tick
        Try
            ' Получение myTaskAOutput.IsDone и проверка ошибок.
            ' Также будет постоянно проверяться остановку непрерывной генерации.
            If (myTaskAOutput.IsDone) Then
                StatusCheckTimer.Enabled = False
                myTaskAOutput.Stop()
                myTaskAOutput.Dispose()
                startAOTask = False
            End If
        Catch ex As DaqException
            StatusCheckTimer.Enabled = False
            Dim caption As String = $"Процедура <{NameOf(StatusCheckTimer_Tick)}>"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            myTaskAOutput.Dispose()
            startAOTask = False
        End Try
    End Sub

    ''' <summary>
    ''' Перезапуск задачи аналогово выхода
    ''' </summary>
    Public Sub Restart()
        If startAOTask Then
            Thread.Sleep(50)
            StopTaskAOutputPanel()
            Thread.Sleep(50)
            StartTaskAOutputPanel()
        End If
    End Sub

    Public Class Generator
        Public Sub New(ByVal physicalChannel As String)
            If physicalChannel Is Nothing Then
                Throw New ArgumentNullException("PhysicalChannel ???")
            End If
            Me.PhysicalChannel = physicalChannel
        End Sub
        Public Property PhysicalChannel() As String
        ''' <summary>
        ''' Тип Генератора
        ''' </summary>
        Public Property TypeWaveform() As WaveformType = WaveformType.SineWave
        ''' <summary>
        ''' Тип Регулировки
        ''' </summary>
        Public Property TypeFrequencyControl() As String = "Частота"

        Private mFrequencyMin As Double = 0.1
        ''' <summary>
        ''' Минимальная частота
        ''' </summary>
        Public Property FrequencyMin() As Double
            Get
                Return mFrequencyMin
            End Get
            Set(ByVal value As Double)
                If value >= mFrequencyMax Then
                    Const caption As String = "Проверка минимального значения частоты"
                    Const text As String = "Минимальное значение частоты должно быть меньше Максимального значения частоты!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mFrequencyMin = value
            End Set
        End Property

        Private mFrequencyMax As Double = 1000
        ''' <summary>
        ''' Максимальная частота
        ''' </summary>
        Public Property FrequencyMax() As Double
            Get
                Return mFrequencyMax
            End Get
            Set(ByVal value As Double)
                If value <= mFrequencyMin Then
                    Const caption As String = "Проверка максимального значения частоты"
                    Const text As String = "Максимальное значение частоты должно быть больше Минимального значения частоты!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mFrequencyMax = value
            End Set
        End Property

        Private mAmplitudeFix As Double = 0.1
        ''' <summary>
        ''' Фиксированная амплитуда генератора
        ''' </summary>
        Public Property AmplitudeFix() As Double
            Get
                Return mAmplitudeFix
            End Get
            Set(ByVal value As Double)
                If value < 0.1 OrElse value > 10 Then
                    Const caption As String = "Проверка фиксированного значения амплитуды"
                    Const text As String = "Значение Амплитуды не может быть меньше 0.1 или больше 10 вольт!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mAmplitudeFix = value
            End Set
        End Property

        Private mRangeDacMin As Double = 0
        ''' <summary>
        ''' Амплитуда Мининум
        ''' </summary>
        Public Property RangeDacMin() As Double
            Get
                Return mRangeDacMin
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value >= mRangeDacMax Then
                    Dim textMessage As String

                    If value < -10 OrElse value > 10 Then
                        textMessage = "Минимальное значение Амплитуды не может быть меньше -10 или больше 10 вольт!"
                    Else
                        textMessage = Nothing
                    End If

                    If value >= mRangeDacMax Then
                        textMessage = "Минимальное значение амплитуды должно быть меньше Максимального значения амплитуды!"
                    End If

                    Const caption As String = "Проверка минимального значения Амплитуды"
                    MessageBox.Show(textMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {textMessage}")
                    Exit Property
                End If

                mRangeDacMin = value
            End Set
        End Property

        Private mRangeDacMax As Double = 10
        ''' <summary>
        ''' Амплитуда Максимум
        ''' </summary>
        Public Property RangeDacMax() As Double
            Get
                Return mRangeDacMax
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value <= mRangeDacMin Then
                    Dim textMessage As String

                    If value < -10 OrElse value > 10 Then
                        textMessage = "Максимальное значение Амплитуды не может быть меньше -10 или больше 10 вольт!"
                    Else
                        textMessage = Nothing
                    End If

                    If value <= mRangeDacMin Then
                        textMessage = "Максимальное значение амплитуды должно быть больше Минимального значения амплитуды!"
                    End If

                    Const caption As String = "Проверка максимального значения Амплитуды"
                    MessageBox.Show(textMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {textMessage}")
                    Exit Property
                End If

                mRangeDacMax = value
            End Set
        End Property

        Private mFixFrequency As Double = 0.1
        ''' <summary>
        ''' Фиксированная частота генератора
        ''' </summary>
        Public Property FixFrequency() As Double
            Get
                Return mFixFrequency
            End Get
            Set(ByVal value As Double)
                If value < 0.1 Then
                    Const TextMessage As String = "Слишком низкая частота!"
                    Const caption As String = "Проверка значения частоты"
                    MessageBox.Show(TextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {TextMessage}")
                    Exit Property
                End If

                mFixFrequency = value
            End Set
        End Property

        Private mPointsSamplePerBuffer As Integer = 10
        ''' <summary>
        ''' Точек Графика В Буфере
        ''' </summary>
        Public Property PointsSamplePerBuffer() As Integer
            Get
                Return mPointsSamplePerBuffer
            End Get
            Set(ByVal value As Integer)
                If value < 10 OrElse value > 1000 Then
                    Const TextMessage As String = "Значение точек построения не может быть меньше 10 или больше 1000 !"
                    Const caption As String = "Проверка точек в буфере графика"
                    MessageBox.Show(TextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {TextMessage}")
                    Exit Property
                End If

                mPointsSamplePerBuffer = value
            End Set
        End Property

        Private mCyclesPerBuffer As Integer = 1
        ''' <summary>
        ''' Циклов графика в буфере
        ''' </summary>
        Public Property CyclesPerBuffer() As Integer
            Get
                Return mCyclesPerBuffer
            End Get
            Set(ByVal value As Integer)
                If value < 1 OrElse value > 10 Then
                    Const TextMessage As String = "Значение Циклов в буфере не может быть меньше 1 или больше 10 !"
                    Const caption As String = "Проверка числа Циклов в буфере"
                    MessageBox.Show(TextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {TextMessage}")
                    Exit Property
                End If

                mCyclesPerBuffer = value
            End Set
        End Property
        ''' <summary>
        ''' Соответствие Диапазону
        ''' </summary>
        Public Property СоответствиеДиапазону() As Double = 10
    End Class

    Private Class FunctionGenerator
        Private _data As Double()
        Private _resultingSampleClockRate As Double
        Private _resultingFrequency As Double
        Private _desiredSampleClockRate As Double
        Private _samplesPerCycle As Integer

        Public Sub New(ByVal timingSubobject As Timing,
                       ByVal desiredFrequency As Double,
                       ByVal samplesPerBuffer As Integer,
                       ByVal cyclesPerBuffer As Integer,
                       ByVal type As String,
                       ByVal amplitude As Double)

            Dim t As WaveformType = WaveformType.SineWave

            If type = "Синус" Then
                t = WaveformType.SineWave
            ElseIf type = "Пила" Then
                t = WaveformType.TriangleWave
            ElseIf type = "Меандр" Then
                t = WaveformType.SquareWave
            Else
                Debug.Assert(False, "Неправильный Waveform тип")
            End If

            InitializeFunctionGenerator(timingSubobject,
                  desiredFrequency,
                  samplesPerBuffer,
                  cyclesPerBuffer,
                  t,
                  amplitude)
        End Sub

        Public Sub New(ByVal timingSubobject As Timing,
                       ByVal desiredFrequency As Double,
                       ByVal samplesPerBuffer As Integer,
                       ByVal cyclesPerBuffer As Integer,
                       ByVal type As WaveformType,
                       ByVal amplitude As Double)

            InitializeFunctionGenerator(timingSubobject,
                 desiredFrequency,
                 samplesPerBuffer,
                 cyclesPerBuffer,
                 type,
                 amplitude)
        End Sub

        Private Sub InitializeFunctionGenerator(ByVal timingSubobject As Timing,
                         ByVal desiredFrequency As Double,
                         ByVal samplesPerBuffer As Integer,
                         ByVal cyclesPerBuffer As Integer,
                         ByVal type As WaveformType,
                         ByVal amplitude As Double)

            If (desiredFrequency <= 0) Then
                Throw New ArgumentOutOfRangeException("желаемая Частота", desiredFrequency, "Этот параметр должен быть положительным числом")
            ElseIf (samplesPerBuffer <= 0) Then
                Throw New ArgumentOutOfRangeException("точек в Буфере", samplesPerBuffer, "Этот параметр должен быть положительным числом")
            ElseIf (cyclesPerBuffer <= 0) Then
                Throw New ArgumentOutOfRangeException("циклов в Буфере", cyclesPerBuffer, "Этот параметр должен быть положительным числом")
            End If

            ' Вначале конфигурировать Task timing параметр 
            If (timingSubobject.SampleTimingType = SampleTimingType.OnDemand) Then
                timingSubobject.SampleTimingType = SampleTimingType.SampleClock
            End If

            _desiredSampleClockRate = (desiredFrequency * samplesPerBuffer) / cyclesPerBuffer ' частота выдачи точки
            _samplesPerCycle = samplesPerBuffer \ cyclesPerBuffer ' точек в цикле
            ' определить частоту опроса
            timingSubobject.SampleClockRate = _desiredSampleClockRate
            _resultingSampleClockRate = timingSubobject.SampleClockRate ' результирующая может быть скорректирована
            _resultingFrequency = _resultingSampleClockRate / (samplesPerBuffer / cyclesPerBuffer)

            If (type = WaveformType.SineWave) Then
                _data = GenerateSineWave(_resultingFrequency, amplitude, _resultingSampleClockRate, samplesPerBuffer)
            ElseIf (type = WaveformType.TriangleWave) Then
                _data = GenerateTriangleWave(cyclesPerBuffer, samplesPerBuffer, amplitude)
            ElseIf (type = WaveformType.SquareWave) Then
                _data = GenerateSquareWave(cyclesPerBuffer, samplesPerBuffer, amplitude)
            End If
        End Sub

        Public ReadOnly Property Data() As Double()
            Get
                Return _data
            End Get
        End Property

        Public ReadOnly Property ResultingSampleClockRate() As Double
            Get
                Return _resultingSampleClockRate
            End Get
        End Property

        Public Function GenerateSineWave(ByVal frequency As Double,
                                                ByVal amplitude As Double,
                                                ByVal sampleClockRate As Double,
                                                ByVal samplesPerBuffer As Double) As Double()

            Dim deltaT As Double = 1 / sampleClockRate
            Dim intSamplesPerBuffer As Integer = CInt(samplesPerBuffer) - 1 ' sec./samp
            Dim rVal(intSamplesPerBuffer) As Double

            For I As Integer = 0 To intSamplesPerBuffer
                rVal(I) = amplitude * Math.Sin((2.0 * Math.PI) * frequency * (I * deltaT))
            Next

            Return rVal
        End Function

        'Public Shared Sub InitComboBox(ByVal box As ComboBox)
        '    Dim obj(2) As Object
        '    obj(0) = "Синус"
        '    obj(1) = "Пила"
        '    obj(2) = "Меандр"

        '    box.Items.Clear()
        '    box.Items.AddRange(obj)
        '    box.Sorted = False
        '    box.DropDownStyle = ComboBoxStyle.DropDownList
        '    box.Text = "Синус"
        'End Sub

        Private Function GenerateTriangleWave(ByVal numCycles As Integer, ByVal numSamplesPerCycle As Integer, ByVal amplitude As Double) As Double()
            Dim data As Double() = New Double(numCycles * numSamplesPerCycle - 1) {}
            Dim toggle As Boolean = True
            Dim k As Double = 0
            Dim tempAmplitude As Double

            For I As Integer = 0 To numCycles - 1
                For J As Integer = 0 To numSamplesPerCycle - 1

                    If k > 1 OrElse k < -1 Then toggle = Not toggle

                    If toggle Then
                        k += 1 / Convert.ToDouble(numSamplesPerCycle) * 4
                    Else
                        k -= 1 / Convert.ToDouble(numSamplesPerCycle) * 4
                    End If

                    tempAmplitude = k * amplitude
                    data(I * numSamplesPerCycle + J) = CDbl(IIf(Math.Abs(tempAmplitude) > 10, Math.Sign(tempAmplitude) * 10, tempAmplitude))
                Next
            Next

            Return data
        End Function

        Private Function GenerateSquareWave(ByVal numCycles As Integer, ByVal numSamplesPerCycle As Integer, ByVal amplitude As Double) As Double()
            Dim data As Double() = New Double(numCycles * numSamplesPerCycle - 1) {}
            Dim toggle As Boolean = False

            For i As Integer = 0 To numCycles - 1
                For j As Integer = 0 To numSamplesPerCycle - 1
                    If j Mod (numSamplesPerCycle \ 2) = 0 Then
                        toggle = Not toggle
                    End If

                    If toggle Then
                        data(i * numSamplesPerCycle + j) = -1 * amplitude
                    Else
                        data(i * numSamplesPerCycle + j) = 1 * amplitude
                    End If
                Next
            Next

            Return data
        End Function

        '        Private Function FunctionGenerator(ByVal selectedIndex As Integer, ByVal numCycles As Integer, ByVal numSamplesPerCycle As Integer, ByVal amplitude As Integer) As Double()
        '            Dim data As Double() = {}
        '            Select Case selectedIndex
        '                Case 0
        '                    data = GenerateSineWave(numCycles, numSamplesPerCycle, amplitude)
        '                    Exit Select
        '                Case 1
        '                    data = GenerateTriangleWave(numCycles, numSamplesPerCycle, amplitude)
        '                    Exit Select
        '                Case 2
        '                    data = GenerateSquareWave(numCycles, numSamplesPerCycle, amplitude)
        '                    Exit Select
        '                Case Else
        '                    MessageBox.Show("Please choose a waveform type.")
        '                    Exit Select
        '            End Select
        '            Return data
        '        End Function

        '        Private Function GenerateSineWave(ByVal numCycles As Integer, ByVal numSamplesPerCycle As Integer, ByVal amplitude As Integer) As Double()
        '            Dim data As Double() = New Double(numCycles * numSamplesPerCycle - 1) {}
        '            For i As Integer = 0 To numCycles - 1
        '                For j As Integer = 0 To numSamplesPerCycle - 1
        '                    data(i * numSamplesPerCycle + j) = Math.Sin(Math.PI / 180 * 360 * j / numSamplesPerCycle) * amplitude
        '                Next
        '            Next
        '            Return data
        '        End Function

    End Class

#End Region

    ''' <summary>
    ''' Подписчик (панель моториста) для дискретного входа
    ''' </summary>
    Friend Class Subscriber
        Public Sub New(ByVal inNameMotoristPanel As String, ByVal inNameIndicator As String, ByVal inIndexItem As Integer)
            Me.NameMotoristPanel = inNameMotoristPanel
            Me.NameIndicator = inNameIndicator
            Me.IndexItem = inIndexItem
        End Sub

        ''' <summary>
        ''' Имя Панели
        ''' </summary>
        ''' <returns></returns>
        Public Property NameMotoristPanel() As String
        ''' <summary>
        ''' Имя Индикатора
        ''' </summary>
        ''' <returns></returns>
        Public Property NameIndicator() As String
        ''' <summary>
        ''' Индекс Элемента В Коллекции
        ''' </summary>
        ''' <returns></returns>
        Public Property IndexItem() As Integer
    End Class
End Class
