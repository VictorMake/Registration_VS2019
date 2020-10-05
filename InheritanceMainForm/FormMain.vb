Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.IO
Imports System.Math
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.Linq
Imports MathematicalLibrary
Imports Registration.FormEditiorGraphByParameter
Imports TaskClientServerLibrary
Imports TaskClientServerLibrary.Clobal

Friend MustInherit Class FormMain
    Implements IUpdateSelectiveControls, IMdiChildrenWindow

#Region "Property"
    ''' <summary>
    ''' Сбор запущен по кнопке
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsRun As Boolean
    ''' <summary>
    ''' Форма является опрашивающей железо
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsFormRunning() As Boolean
    ''' <summary>
    ''' Родительская форма
    ''' </summary>
    ''' <returns></returns>
    Protected ReadOnly Property MainMDIFormParent As FormMainMDI Implements IMdiChildrenWindow.MainMDIFormParent
    ''' <summary>
    ''' Коллекция форм графиков парамет от параметра
    ''' </summary>
    ''' <returns></returns>
    Friend Property GraphsByParameters() As Dictionary(Of String, FormPatternGraphByParameter)
    ''' <summary>
    ''' Массив каналов на втором графике параметр от параметра
    ''' </summary>
    ''' <returns></returns>
    Friend Property AllGraphParametersByParameter() As GraphParametersByParameter()
    ''' <summary>
    ''' Выведена форма показа каналов в определенном диапазоне
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsShowFormParametersInRange() As Boolean

    ''' <summary>
    ''' Включена запись Регистратора
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsRecordEnable() As Boolean
    ''' <summary>
    ''' Перед этим была загрузка базы
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsBeforeThatHappenLoadDbase() As Boolean
    ''' <summary>
    ''' Режим Снимка
    ''' </summary>
    ''' <returns></returns>
    Protected Property IsSnapshot() As Boolean
    ''' <summary>
    ''' Загружено окно навигации по фоновым снимкам
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsLoadFormNavigatorSnapshot() As Boolean
    ''' <summary>
    ''' Тип Формы Испытания
    ''' </summary>
    ''' <returns></returns>
    Friend Property KindFormExamination() As FormExamination
    ''' <summary>
    ''' Строка Конфигурации
    ''' </summary>
    ''' <returns></returns>
    Friend Property ConfigurationString() As String

    Private mKeyConfiguration As Integer
    ''' <summary>
    ''' Индекс новой строки конфигурации в базе
    ''' </summary>
    ''' <returns></returns>
    Friend Property KeyConfiguration() As Integer
        Get
            Return mKeyConfiguration
        End Get
        Set(ByVal Value As Integer)
            mKeyConfiguration = Value
            '29) Конфигурация графиков от параметра
            WriteINI(PathOptions, "Stend", "keyКонфигурация", mKeyConfiguration.ToString)
        End Set
    End Property
    ''' <summary>
    ''' Каналы сбора и расшифровки сконфигурированы для режима с именем "Регистратор"
    ''' или режим изменен для расшифровки
    ''' </summary>
    ''' <returns></returns>
    Protected ReadOnly Property IsRegimeIsRegistrator As Boolean
        Get
            If RegimeType = cРегистратор OrElse isRegimeChangeForDecoding Then
                Return True
            Else 'если осциллограф то
                Return False
            End If
        End Get
    End Property
    ''' <summary>
    ''' Текущий режим сконфигурированных каналов
    ''' </summary>
    ''' <returns></returns>
    Friend Property RegimeType As String = cРегистратор ' текст кнопки меню выбранного режима расшифровки
    ''' <summary>
    ''' Частота опроса регистратора или снимка
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property SampleRate() As Integer
        Get
            If IsRegimeIsRegistrator Then
                Return FrequencyBackgroundSnapshot
            Else 'если осциллограф то
                Return FrequencyHandQuery
            End If
        End Get
    End Property
    ''' <summary>
    ''' Частота опроса с которой был произведен снимка
    ''' </summary>
    ''' <returns></returns>
    Friend Property FrequencyBackgroundSnapshot As Short
    ''' <summary>
    ''' Форма клиента для получения данных от Сервера
    ''' </summary>
    ''' <returns></returns>
    Friend Property FormClientDataSocket As FormClient
    ''' <summary>
    ''' Массив индексов выбранных каналов для связи со структурой TypeBaseParameter
    ''' </summary>
    ''' <returns></returns>
    Friend Property IndexParameters() As Integer()
    ''' <summary>
    ''' Число всех измеряемых параметров
    ''' </summary>
    ''' <returns></returns>
    Friend Property CountMeasurand As Integer
    ''' <summary>
    ''' Используются Расчетные Модули
    ''' </summary>
    ''' <returns></returns>
    Friend Property IsUseCalculationModule As Boolean
    ''' <summary>
    ''' Тип Крд в снимке
    ''' </summary>
    ''' <returns></returns>
    Friend Property TypeKRDinSnapshot As String
    ''' <summary>
    ''' Средние значения всех каналов за время сбора в кадре
    ''' </summary>
    ''' <returns></returns>
    Friend Property MeasuredValues As Double(,)
    ''' <summary>
    ''' Cписок всех параметров которые снимаются
    ''' </summary>
    ''' <returns></returns>
    Friend Property SnapshotSmallParameters As TypeSmallParameter()
    ''' <summary>
    ''' Номер параметра Оси Эталона
    ''' </summary>
    ''' <returns></returns>
    Friend Property NumberParameterAxes As Integer
    ''' <summary>
    ''' Номер изделия снимка
    ''' </summary>
    ''' <returns></returns>
    Friend Property NumberProductionSnapshot As Integer
#End Region

#Region "Enum"
    ''' <summary>
    ''' Режим работы с холстом графика
    ''' </summary>
    Protected Enum MyGraphMode
        'Панорамирование = 0
        Scaling = 1 ' Масштабирование
        OneCursor = 2 ' Один Курсор
        TwoCursors = 3 ' Два Курсора
        DoNothing = 4 ' Ничего нельзя делать
    End Enum
    ''' <summary>
    ''' Режим работы с холстом графика
    ''' </summary>
    Protected GraphModeValue As MyGraphMode = MyGraphMode.Scaling

    ''' <summary>
    ''' Для настройки формы {FormTuningSelectiveBase} выборочного списка параметров
    ''' </summary>
    Public Enum TypeFormTuningSelective As Integer
        <Description(TextControlFormTuning)>
        TextControl = 1
        <Description(GraphControlFormTuning)>
        GraphControl = 2
        <Description(SelectiveControlFormTuning)>
        SelectiveControl = 3
    End Enum
#End Region

#Region "Интерфейс для реализации"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected MustOverride Sub BaseFormLoad()
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub BaseFormClosing(ByRef e As FormClosingEventArgs)
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub BaseFormClosed()
    ''' <summary>
    ''' Настроить график под выбранный параметр Оси У
    ''' </summary>
    Protected MustOverride Sub TuneDiagramUnderSelectedParameterAxesY()
    ''' <summary>
    ''' Очистка графиков
    ''' </summary>
    ''' <param name="N"></param>
    ''' <param name="arrParameters"></param>
    Protected MustOverride Sub CleaningDiagram(ByRef N As Integer, ByRef arrParameters() As TypeBaseParameter)
    ''' <summary>
    ''' Настройка меню и кнопок
    ''' </summary>
    Protected MustOverride Sub EnableDisableControlsBase()
    ''' <summary>
    ''' Запуск сбора вызывается при образовании формы
    ''' </summary>
    Friend MustOverride Sub StartMeasurement()
    ''' <summary>
    ''' Применить настройку видимости шлейфов
    ''' Применение после формы FormVisibleTail
    ''' </summary>
    Protected MustOverride Sub ApplyTuningVisibilityMeasuringStub()
    ''' <summary>
    ''' Обновить графики от параметров для снимка
    ''' </summary>
    ''' <param name="inGraphOfParam"></param>
    Protected MustOverride Sub ReloadDiagramParameterFromParameterForSnapshot(ByRef inGraphOfParam As GraphsOfParameters.GraphOfParameter)
    ''' <summary>
    ''' Настройки опций программы
    ''' </summary>
    Protected MustOverride Sub SettingsOptionProgram()
    ''' <summary>
    ''' Перестройка числа параметров для выбранного режима
    ''' </summary>
    Protected MustOverride Sub CharacteristicForRegime()
    ''' <summary>
    ''' Уточнение Включение подробного/выборочного листа
    ''' </summary>
    Protected MustOverride Sub DetailSelectiveBase()
    ''' <summary>
    ''' Настроить аннотации.
    ''' Метод может быть полностью синхронизирован с помощью атрибута MethodlmplAttribute.
    ''' Такой подход может стать альтернативой оператору lock в тех случаях,
    ''' когда метод требуется заблокировать полностью. 
    ''' </summary>
    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    Protected MustOverride Sub TuneAnnotation()
    ''' <summary>
    ''' Настроить выборочный список
    ''' </summary>
    Protected MustOverride Sub TuneSelectiveList()
    ''' <summary>
    ''' Остановить сбор или приём данных
    ''' </summary>
    Protected MustOverride Sub StopAcquisition()
    ''' <summary>
    ''' Перезаписывает строку запроса SQL и выставляет флаг если база данных сменена
    ''' </summary>
    ''' <param name="outSQL"></param>
    ''' <param name="IsChannelShaphot"></param>
    Friend MustOverride Sub GetSqlForDbase(ByRef outSQL As String, ByRef IsChannelShaphot As Boolean)
#End Region

#Region "Ссылки на формы"
    Protected frmTuningRegime As FormTuningRegime
    Private frmBrowser As FormWebBrowser
    Private WithEvents mReaderWriterCommander As ReaderWriterCommand
#End Region

    ''' <summary>
    ''' Диапазон по оси Х и число замеров
    ''' 1800*0.05=90 сек=1.5 минуты за это время полностью пройдет курсор при частоте сбора 20 герц
    ''' </summary>
    Protected Const arraysize As Short = 1800
    ''' <summary>
    ''' Шаг между рисками разметки шкалы графика
    ''' </summary>
    Protected stepTic As Integer
    ''' <summary>
    ''' Индекс времени или строки для записи последних собранных данных в массив
    ''' </summary>
    Protected indexTimeVsRow As Integer
    ''' <summary>
    ''' Массив размахов для приведения значения параметра в нормированный диапазон на графике
    ''' </summary>
    Protected RangesOfDeviation As Double(,)
    ''' <summary>
    ''' Средние значения всех каналов за время сбора в кадре приведенных в нормированный диапазон
    ''' arrСреднееПересчитанный
    ''' </summary>
    Protected MeasuredValuesToRange As Double(,)
    ''' <summary>
    ''' Массив для упаковки данных для последующей их передачи клиенту
    ''' </summary>
    Protected PackOfParameters As String()
    ''' <summary>
    ''' Массив для упаковки данных для записи
    ''' </summary>
    Protected PackOfParametersToRecord As String()
    ''' <summary>
    ''' Менеджер и фабрика классов расшифровок режимов
    ''' </summary>
    Protected managerAnalysis As AnalysisManager
    ''' <summary>
    ''' Массив имен параметров составленный из конфигурационной строки для выбранного режима
    ''' </summary>
    Protected NamesParameterRegime As String()
    'Private arrСечения(,) As Double
    ''' <summary>
    ''' Массив стрелок расшифровки
    ''' </summary>
    Protected Arrows As New List(Of Arrow)
    ''' <summary>
    ''' Номер параметра оси смененного
    ''' </summary>
    Protected numberParameterAxesChanged As Integer
    ''' <summary>
    ''' Номер параметра оси первоначальное
    ''' </summary>
    Protected numberParameterAxesОriginal As Integer = 1
    ''' <summary>
    ''' Средние значения всех каналов за время сбора в кадре
    ''' для записи при использовании текстового формата.
    ''' </summary>
    Protected dataMeasuredValuesString As StringBuilder ' Память выделяется при инициализации далее.

#Region "Линии и стрелки"
    ''' <summary>
    ''' Координаты стрелки X
    ''' </summary>
    Protected xData(1) As Double
    ''' <summary>
    ''' Координаты стрелки Y
    ''' </summary>
    Protected yData(1) As Double
    ''' <summary>
    ''' Временная копия для восстановления диапазона
    ''' </summary>
    Protected XAxisTimeRange As Range
    ''' <summary>
    ''' Для приведения физического значения к шкале построения
    ''' </summary>
    Protected YAxisTimeRange As Range
    ''' <summary>
    ''' Есть наклонная линия
    ''' </summary>
    Protected isSlantingLine As Boolean
    ''' <summary>
    ''' Графический контекст для рисования в контролах
    ''' </summary>
    Private g As Graphics
#End Region

#Region "ГрафикОтПараметров"
    ''' <summary>
    ''' Отображать график от параметра
    ''' </summary>
    Protected isShowDiagramFromParameter As Boolean
    ''' <summary>
    ''' Отображать график параметров от времени
    ''' </summary>
    Protected isShowDiagramFromTime As Boolean
    ''' <summary>
    ''' Счётчик отображения шлейфов графиков от параметров
    ''' </summary>
    Protected counterDiagramFromParameter As Integer
    ''' <summary>
    ''' Размерность массива для отображения графиков от параметров
    ''' когда график по времени
    ''' </summary>
    Protected arraysizeDiagramFromParameter As Integer
    ''' <summary>
    ''' Координаты значений параметров по оси X при отображении графика параметр от параметра
    ''' </summary>
    Protected axesXDiagramFromParameter As Double()
    ''' <summary>
    ''' Позиция курсора для значений при отображении графика от параметров
    ''' </summary>
    Protected positionCursorDiagramFromParameter As Integer
    ''' <summary>
    ''' Массив значений параметров при отображении графика от параметров
    ''' </summary>
    Protected dataDiagramFromParameter As Double(,)
    ''' <summary>
    ''' Вспомогательный логический массив для определения очистки шлейфа при инициализации
    ''' </summary>
    Protected isClearDataDiagramFromParameter As Boolean()
    ''' <summary>
    ''' Очистки шлейфа для параметра оси Х при инициализации
    ''' </summary>
    Protected isClearDiagramFromParameterAxesX As Boolean
    ''' <summary>
    ''' Имя параметра оси Х по которому производится построение графика от параметров
    ''' </summary>
    Protected nameParameterAxesX As String
    ''' <summary>
    ''' Коллекция окон графиков от параметров.
    ''' </summary>
    Protected mGraphsOfParams As GraphsOfParameters
    ''' <summary>
    ''' Используются окона графики от параметров.
    ''' </summary>
    Protected isUseWindowsDiagramFromParameter As Boolean
    ''' <summary>
    ''' Коэффициент приведения Т бокса
    ''' </summary>
    Protected coefficientBringingTBoxing As Double
    ''' <summary>
    ''' математическое строковое выражение
    ''' </summary>
    Protected expressionMath As String
    ''' <summary>
    ''' Количество графиков от параметров
    ''' </summary>
    Private countDiagramFromParameter As Integer
    ''' <summary>
    ''' Конфигурационный файл коллекции окон графиков от параметров
    ''' </summary>
    Friend XDoc As XDocument
    ''' <summary>
    ''' Настроечная форма конфигурирования графиков параметр от параметра
    ''' </summary>
    Private mAxesAdvanced As FormAxesAdvanced
#End Region

#Region "Boolean"
    '''' <summary>
    '''' Не останавливать запись когда вызывается запуск Сервера
    '''' </summary>
    Friend IsMemoForStartRecord As Boolean
    ''' <summary>
    ''' Использовать перья курсоров
    ''' </summary>
    Protected isUsePens As Boolean ' для меток курсоров
    ''' <summary>
    ''' Форма загружена
    ''' </summary>
    Protected isFormLoaded As Boolean = False
    ''' <summary>
    ''' Режим изменен для расшифровки
    ''' </summary>
    Protected isRegimeChangeForDecoding As Boolean
    ''' <summary>
    ''' Подробный лист
    ''' </summary>
    Protected isDetailedSheet As Boolean
    ''' <summary>
    ''' Пользователь хочет прерывать закрытие окна
    ''' </summary>
    Protected isCancel As Boolean
    'Private сечениеПостроено As Boolean
#End Region

#Region "FormMain"
    Public Sub New()
        'Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal frmParent As FormMainMDI, ByVal kindExamination As FormExamination, ByVal captionText As String)
        MyBase.New()
        Me.KindFormExamination = kindExamination
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MdiParent = frmParent
        MainMDIFormParent = frmParent
        Text = captionText
        mKeyConfiguration = SettingKeyConfiguration ' Конфигурация графиков от параметра
    End Sub

    Private Sub FormMain_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'CheckVersionOs()
        'ReDim_IndexParameters(0)
        Re.Dim(IndexParameters, 0)
        coefficientBringingTBoxing = Math.Sqrt(Const288 / (TemperatureOfBox + Kelvin))
        isSlantingLine = False
        isUsePens = False
        IsDataBaseChanged = False ' при повторном открытии окна сбросить по умолчанию
        NumberParameterAxes = 1
        numberParameterAxesChanged = 2

        InitializeControlsMain()

        StatusStripMain.Items(NameOf(LabelProduct)).Text = "Установка " & StandNumber
        HelpProviderAdvancedCHM.HelpNamespace = Path.Combine(PathResourses, "HelpRegistrationTCP.chm")
        'LedStyleДискретныхИндикаторов()' проба с шаманством
        isFormLoaded = True
        ButtonGraphParameter.Checked = False
        TimerRealize3D.Enabled = True

        ' обязательно в конце этого метода
        BaseFormLoad()

        Dim mIsServer As Boolean = False ' клиент по умолчанию

        If Not IsWorkWithController Then ' просмотр снимка
            mIsServer = False
        ElseIf IsServer Then
            mIsServer = True
        End If

        If Not IsTcpClient Then mReaderWriterCommander = New ReaderWriterCommand(Me, PathResourses, mIsServer, CountClientOrNumberClient, ServerWorkingFolder, ClientWorkingFolder, AddressOf AppendOutput)
    End Sub

    ''' <summary>
    ''' Logging командного обмена между компьютерами
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="RichTextBoxKey"></param>
    ''' <param name="selectionModeIcon"></param>
    Private Sub AppendOutput(message As String, RichTextBoxKey As Integer, selectionModeIcon As MessageBoxIcon)
        ' TODO: в случае небходимости доделать
        'Dim tempRichTextBox As RichTextBox = RichTextBoxClient 'richTextBoxDictionary(RichTextBoxKey)

        'If tempRichTextBox.TextLength > 0 Then
        '    tempRichTextBox.AppendText(ControlChars.NewLine)
        'End If

        'tempRichTextBox.AppendText(String.Format("{0} {1}", DateTime.Now.ToLongTimeString, message))
        'WriteTextToRichTextBox(tempRichTextBox, message, selectionModeIcon)
        'tempRichTextBox.ScrollToCaret()
    End Sub

    ''' <summary>
    ''' Настройка контролов для всех форм
    ''' </summary>
    Private Sub InitializeControlsMain()
        DetailSelective(False)
        XAxisTime.Range = New Range(0, 10)
        YAxisTime.Range = New Range(0, 100)
        SlideTime.Range = New Range(0, XAxisTime.Range.Maximum)
        NumericUpDownPrecisionScreen.Value = Precision

        EnableDisableControlsBase()
        InitializeListViews()

        StatusStripMain.Items(NameOf(LabelDiscValue)).Text = FreeBytes(PathResourses)
        MenuNewWindowRegistration.Enabled = MainMDIFormParent.MenuNewWindowRegistration.Enabled
        MenuNewWindowSnapshot.Enabled = MainMDIFormParent.MenuNewWindowSnapshot.Enabled
        MenuNewWindowTarir.Enabled = MainMDIFormParent.MenuNewWindowTarir.Enabled
        MenuNewWindowClient.Enabled = MainMDIFormParent.MenuNewWindowClient.Enabled
        MenuNewWindowDBaseChannels.Enabled = MainMDIFormParent.MenuNewWindowDBaseChannels.Enabled
        MenuNewWindowEvents.Enabled = MainMDIFormParent.MenuNewWindowEvents.Enabled

        ' получить графический контекст
        Dim hwnd As New IntPtr
        hwnd = MyBase.Handle
        g = Graphics.FromHwnd(hwnd)
        FillItemsComboBoxSelectiveList(ComboBoxSelectiveList)

        'Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        SplitContainerGraph.Panel1Collapsed = False
        SplitContainerGraph.Panel2Collapsed = False

        MenuPens.Checked = True

        If File.Exists(PathXmlFileGraphByParameters) Then PopulateAllMenuPatternGraphByParameter()

        WindowState = FormWindowState.Maximized

        TableLayoutPanelMode.RowStyles(1).Height = 0.0!
        PanelMode.Height = Convert.ToInt32(TableLayoutPanelMode.RowStyles(0).Height + TableLayoutPanelMode.RowStyles(1).Height + 2)
        TableLayoutPanelMode.Height = TableLayoutPanelMode.Height
    End Sub

    Private Sub FormMain_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        BaseFormClosing(e) ' Обязательно вначале этого метода должны идти обработчики унаследованных форм
        e.Cancel = isCancel
        ReaderWriterCommandCloseAsync()
    End Sub

    Private Async Sub ReaderWriterCommandCloseAsync()
        If mReaderWriterCommander IsNot Nothing Then
            'TODO: закоментировал Послать команду остановки контроллера "Stop"
            'For Each itemtarget As targetClass In ReaderWriterCommand.gManagertarget.Collectionstarget.Values
            '    itemtarget.CommandWriterQueue.Enqueue(New ProgramTotargetTask(COMMAND_STOP))
            'Next
            'ReaderWriterCommand.ОтправитьКомандыНаВсеШасси()

            'Await ReaderWriterCommand.CloseAsync()
            mReaderWriterCommander.Close()
            Await Task.Delay(1000)
            mReaderWriterCommander = Nothing
        End If
    End Sub

    Private Sub FormMain_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        If isCancel Then Exit Sub ' пользователь не хочет прерывать

        BaseFormClosed() ' Обязательно вначале этого метода
        isFormLoaded = False
        g.Dispose()

        CoordinateMenuForm()
        DeleteAllMenuPatternGraphByParameter()
        WaveformGraphTime.Annotations.Clear()

        If gFormsPanelManager.FormPanelMotoristCount > 0 Then
            For Each mfrmBasePanelMotorist As FormBasePanelMotorist In gFormsPanelManager.CollectionFormPanelMotorist.Values
                mfrmBasePanelMotorist.MainMDIFormReference = Nothing
            Next
        End If

        CollectionForms.Remove(Text)

        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        'Me.Dispose()' остаётся ссылка окна в куче и не уничтожает до конца окно
        'fMainForm.MenuStrip1.Visible = Not fMainForm.MdiChildren.Length > 1

        ' Шаманство, чтобы не выскакивала ошибка:
        ' System.ObjectDisposedException : Доступ к ликвидированному объекту невозможен.
        ' Имя объекта:  "NationalInstruments.UI.WindowsForms.PropertyEditor".
        InstrumentControlStrip1.Items.Clear()
    End Sub

    ''' <summary>
    ''' Согласовать видимость MDI меню в главной и дочерних формах
    ''' </summary>
    Private Sub CoordinateMenuForm()
        Dim itemForm As Form
        Dim KeysCopy As String() = CollectionForms.Forms.Keys.ToArray

        For I As Integer = KeysCopy.GetLength(0) - 1 To 0 Step -1
            itemForm = CollectionForms.Forms(KeysCopy(I))

            If CStr(itemForm.Tag) = TagFormDaughter Then
                CType(itemForm, FormMain).MenuNewWindowRegistration.Enabled = MainMDIFormParent.MenuNewWindowRegistration.Enabled
                CType(itemForm, FormMain).MenuNewWindowSnapshot.Enabled = MainMDIFormParent.MenuNewWindowSnapshot.Enabled
                CType(itemForm, FormMain).MenuNewWindowTarir.Enabled = MainMDIFormParent.MenuNewWindowTarir.Enabled
                CType(itemForm, FormMain).MenuNewWindowClient.Enabled = MainMDIFormParent.MenuNewWindowClient.Enabled
                CType(itemForm, FormMain).MenuNewWindowDBaseChannels.Enabled = MainMDIFormParent.MenuNewWindowDBaseChannels.Enabled
            ElseIf CStr(itemForm.Tag) = TagFormTarir Then
                CType(itemForm, FormTarir).MenuNewWindowRegistration.Enabled = MainMDIFormParent.MenuNewWindowRegistration.Enabled
                CType(itemForm, FormTarir).MenuNewWindowSnapshot.Enabled = MainMDIFormParent.MenuNewWindowSnapshot.Enabled
                CType(itemForm, FormTarir).MenuNewWindowTarir.Enabled = MainMDIFormParent.MenuNewWindowTarir.Enabled
                CType(itemForm, FormTarir).MenuNewWindowClient.Enabled = MainMDIFormParent.MenuNewWindowClient.Enabled
                CType(itemForm, FormTarir).MenuNewWindowDBaseChannels.Enabled = MainMDIFormParent.MenuNewWindowDBaseChannels.Enabled
            End If
        Next

        Erase KeysCopy
    End Sub

    ''в надежде что перерисует
    'Private Sub LedStyleДискретныхИндикаторов()
    '    For Each ItemTableLayoutPanel As TableLayoutPanel In РамкаДискретных.Controls
    '        For Each Led As NationalInstruments.UI.WindowsForms.Led In ItemTableLayoutPanel.Controls
    '            Led.LedStyle = NationalInstruments.UI.LedStyle.Square3D
    '            Led.Value = True
    '            Led.Refresh()
    '            'Led.Value = False
    '        Next
    '        System.Windows.Forms.Application.DoEvents()
    '    Next
    'End Sub
#End Region

#Region "Циклы сбора и приёма"
    ''' <summary>
    ''' Привести к оси эталона
    ''' </summary>
    ''' <param name="NStandard"></param>
    ''' <param name="NCurrent"></param>
    ''' <param name="physicalValue"></param>
    ''' <returns></returns>
    Friend Function CastToAxesStandard(ByRef NStandard As Integer, ByRef NCurrent As Integer, ByRef physicalValue As Double) As Double
        ' NStandard - номер выбранного параметра для эталонной оси
        ' NCurrent - номер текущего параметра
        ' physicalValue - физическое значение после полинома
        Return RangesOfDeviation(NStandard, 1) + (RangesOfDeviation(NStandard, 2) - RangesOfDeviation(NStandard, 1)) * (physicalValue - RangesOfDeviation(NCurrent, 1)) / (RangesOfDeviation(NCurrent, 2) - RangesOfDeviation(NCurrent, 1))
    End Function

    ''' <summary>
    ''' Переместить слайдер времени
    ''' </summary>
    ''' <param name="valueCursor"></param>
    Protected Sub MoveSlideTime(ByVal valueCursor As Double)
        SlideTime.CustomDivisions(0).Text = Format(valueCursor, "0")
        SlideTime.CustomDivisions(0).Value = valueCursor
        SlideTime.Value = valueCursor
    End Sub
#End Region

#Region "График от параметра"
    ''' <summary>
    ''' Настоить График параметр от параметра
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonTuneTrand_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTuneTrand.CheckedChanged
        If IsHandleCreated Then
            If AllGraphParametersByParameter IsNot Nothing Then
                TuneTrand(ButtonTuneTrand.Checked)
            Else
                ButtonTuneTrand.Checked = False
            End If
        End If
    End Sub
    ''' <summary>
    ''' Настоить шлейфы График параметр от параметра
    ''' </summary>
    ''' <param name="isTuning"></param>
    Protected Sub TuneTrand(ByVal isTuning As Boolean)
        If isTuning Then
            isShowDiagramFromParameter = False
            TuneDiagramFromParameter() ' загружается форма
        Else ' сбор
            'ReDim_ParameterTwoGraph(UBound(ParametersType))
            Re.Dim(ParameterTwoGraph, UBound(ParametersType))
            RestoreDiagramFromParameter()
            If isShowDiagramFromParameter Then TuneRepresentationParameter()
        End If
    End Sub
    ''' <summary>
    ''' Настоить График параметр от параметра
    ''' </summary>
    Private Sub TuneDiagramFromParameter()
        Try
            mAxesAdvanced = New FormAxesAdvanced(Me)
            mAxesAdvanced.Show()
        Catch ex As Exception
            Const caption As String = "Ошибка загрузки формы <Настройка графика от параметра>"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        Finally
            mAxesAdvanced = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' Восстановить Режим Графика От Параметра
    ''' </summary>
    Protected Sub RestoreDiagramFromParameter()
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Const таблицаКонфигурации As String = "КонфигурацииОтображения"
        Dim newScatterPlot As ScatterPlot
        Dim newPointAnnotation As XYPointAnnotation
        Dim nameParameter, caption As String
        Dim I, indexPosition, numberAxis, countTrand As Integer

        cn.Open()
        ' Здесь проверка существования базы
        If CheckExistTable(cn, таблицаКонфигурации) = False Then CreateTableConfigurationDiagram(cn)

        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text

        ' проверка существования конфигурации
        Dim sSQL As String = "SELECT КонфигурацииОтображения.keyКонфигурацияОтображения " &
                            "FROM КонфигурацииОтображения " &
                            "WHERE КонфигурацииОтображения.keyКонфигурацияОтображения = " & mKeyConfiguration
        cmd.CommandText = sSQL
        rdr = cmd.ExecuteReader
        If Not rdr.Read() Then mKeyConfiguration = 0
        rdr.Close()

        If mKeyConfiguration = 0 Then ' конкретной конфигурации нет
            ' считывания, настройки и записи не было значит считываем последнюю конфигурацию
            sSQL = "SELECT КонфигурацииОтображения.keyКонфигурацияОтображения FROM КонфигурацииОтображения ORDER BY keyКонфигурацияОтображения"
            ' Создание читателя
            cmd.CommandText = sSQL
            rdr = cmd.ExecuteReader

            ' keyКонфигурация в последней строке последняя сохраненная конфигурация
            Do While rdr.Read
                mKeyConfiguration = Convert.ToInt32(rdr("keyКонфигурацияОтображения"))
            Loop

            rdr.Close()
        End If

        ' считываем конкретную конфигурацию
        sSQL = "SELECT КонфигурацииОтображения.* " &
                "FROM КонфигурацииОтображения " &
                "WHERE КонфигурацииОтображения.keyКонфигурацияОтображения = " & mKeyConfiguration
        cmd.CommandText = sSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() = True Then
            isShowDiagramFromTime = Convert.ToBoolean(rdr("ВремяИлиПараметр"))
            CounterParametersGraph = CInt(FrequencyBackground / Convert.ToInt32(rdr("ЧастотаПостроения")))
            CounterLightParametersGraph = Convert.ToInt32(rdr("ЧастотаПостроения")) * Convert.ToInt32(rdr("ВремяСвечения"))

            If isShowDiagramFromTime Then
                XAxis1.Caption = vbNullString
                XAxis1.Visible = False
            Else
                nameParameterAxesX = Convert.ToString(rdr("ИмяПараметраОсиХ"))
                XAxis1.Caption = nameParameterAxesX
                XAxis1.Range = New Range(CDbl(rdr("МинОсь")), CDbl(rdr("МахОсь")))
                XAxis1.Visible = True
            End If

            rdr.Close()
            sSQL = "SELECT КонфигурацииОтображения.ИмяКонфигурации, Ось.* " &
                    "FROM КонфигурацииОтображения RIGHT JOIN Ось ON КонфигурацииОтображения.keyКонфигурацияОтображения = Ось.keyКонфигурацияОтображения " &
                    "WHERE(((КонфигурацииОтображения.keyКонфигурацияОтображения) = " & mKeyConfiguration & ")) " &
                    "ORDER BY Ось.НомерОси;"

            ' Создание читателя
            cmd.CommandText = sSQL
            rdr = cmd.ExecuteReader

            ' сначало удалим оси
            For I = ScatterGraphParameter.YAxes.Count - 1 To 1 Step -1
                ScatterGraphParameter.YAxes.RemoveAt(I)
            Next

            ' затем добавим по порядку
            Do While rdr.Read
                numberAxis = Convert.ToInt32(rdr("НомерОси"))

                If numberAxis <> 0 Then ScatterGraphParameter.YAxes.Add(New YAxis())
                If numberAxis > ScatterGraphParameter.YAxes.Count - 1 Then numberAxis = 0

                ScatterGraphParameter.YAxes(numberAxis).Tag = CStr(numberAxis)
                indexPosition = Convert.ToInt32(rdr("РасположениеМетки"))

                If indexPosition = Disposition.Left Then
                    ScatterGraphParameter.YAxes(numberAxis).Position = YAxisPosition.Left
                ElseIf indexPosition = Disposition.Right Then
                    ScatterGraphParameter.YAxes(numberAxis).Position = YAxisPosition.Right
                    'Else 'intРасположение = 3
                End If

                indexPosition = Convert.ToInt32(rdr("РасположениеЧисла"))

                If indexPosition = Disposition.Left Then
                    ScatterGraphParameter.YAxes(numberAxis).CaptionPosition = YAxisPosition.Left
                ElseIf indexPosition = Disposition.Right Then
                    ScatterGraphParameter.YAxes(numberAxis).CaptionPosition = YAxisPosition.Right
                    'Else 'intРасположение = 3
                End If

                ScatterGraphParameter.YAxes(numberAxis).Mode = AxisMode.Fixed
                ScatterGraphParameter.YAxes(numberAxis).Range = New Range(CDbl(rdr("НижнееЗначение")), CDbl(rdr("ВерхнееЗначение")))
                ScatterGraphParameter.YAxes(numberAxis).CaptionForeColor = ColorsNet(Convert.ToInt32(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MajorDivisions.TickColor = ColorsNet(Convert.ToInt32(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MajorDivisions.LabelForeColor = ColorsNet(Convert.ToInt32(rdr("НомерЦвета")))
                ScatterGraphParameter.YAxes(numberAxis).MinorDivisions.TickColor = ColorsNet(Convert.ToInt32(rdr("НомерЦвета")))
            Loop

            rdr.Close()
            'ReDim_AllGraphParametersByParameter(UBound(IndexParameters))
            Re.Dim(AllGraphParametersByParameter, UBound(IndexParameters))

            For I = 1 To UBound(IndexParameters)
                If IsDataBaseChanged Then
                    AllGraphParametersByParameter(I).NameParameter = ParametersShaphotType(IndexParameters(I)).NameParameter
                    AllGraphParametersByParameter(I).UnitOfMeasure = ParametersShaphotType(IndexParameters(I)).UnitOfMeasure
                Else
                    AllGraphParametersByParameter(I).NameParameter = ParametersType(IndexParameters(I)).NameParameter
                    AllGraphParametersByParameter(I).UnitOfMeasure = ParametersType(IndexParameters(I)).UnitOfMeasure
                End If

                AllGraphParametersByParameter(I).IndexColor = 7
                AllGraphParametersByParameter(I).NumberAxis = -1
                AllGraphParametersByParameter(I).NumberTail = -1
            Next

            ' теперь с графиками
            sSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " &
                "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " &
                "WHERE ((([КонфигурацииОтображения].keyКонфигурацияОтображения)= " & mKeyConfiguration & "));"
            ' Создание читателя
            cmd.CommandText = sSQL
            rdr = cmd.ExecuteReader
            countDiagramFromParameter = 0
            ' сначало удалим графики
            ScatterGraphParameter.Annotations.Clear()
            ScatterGraphParameter.Plots.Clear()

            ' затем добавим по порядку
            Do While rdr.Read
                countDiagramFromParameter += 1
                newScatterPlot = New ScatterPlot
                ScatterGraphParameter.Plots.Add(newScatterPlot)
                newScatterPlot.XAxis = XAxis1
                countTrand = ScatterGraphParameter.Plots.Count
                nameParameter = Convert.ToString(rdr("ИмяПараметра"))

                For I = 1 To UBound(AllGraphParametersByParameter)
                    If AllGraphParametersByParameter(I).NameParameter = nameParameter Then
                        AllGraphParametersByParameter(I).IndexColor = CInt(rdr("НомерЦвета"))
                        AllGraphParametersByParameter(I).NumberTail = countTrand - 1
                        AllGraphParametersByParameter(I).NumberAxis = Convert.ToInt32(rdr("НомерОси"))
                        AllGraphParametersByParameter(I).IsTailVisible = True
                        Exit For
                    End If
                Next

                newScatterPlot.LineColor = ColorsNet(Convert.ToInt32(rdr("НомерЦвета")))
                newScatterPlot.LineStyle = LineStyle.Solid

                If isShowDiagramFromTime Then
                    newScatterPlot.PointStyle = PointStyle.None
                Else
                    newScatterPlot.PointStyle = PointStyle.SolidDiamond
                    newScatterPlot.PointColor = ScatterGraphParameter.YAxes(CInt(rdr("НомерОси"))).CaptionForeColor
                End If

                newScatterPlot.YAxis = ScatterGraphParameter.YAxes(CInt(rdr("НомерОси")))

                newPointAnnotation = New XYPointAnnotation
                ScatterGraphParameter.Annotations.Add(newPointAnnotation)
                newPointAnnotation.ArrowColor = newScatterPlot.LineColor
                newPointAnnotation.ArrowHeadStyle = ArrowStyle.SolidStealth
                newPointAnnotation.ArrowLineWidth = 1.0!
                newPointAnnotation.ArrowTailSize = New Size(20, 15)
                newPointAnnotation.Caption = nameParameter
                ' смещение по моему в пикселях
                'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.Auto, CSng(Math.Abs(maximumX - minimumX) * 0.01!), -CSng(Math.Abs(maximumY - minimumY) * 0.1!))
                'dblСмещениеХ = (XAxis1.Range.Maximum - XAxis1.Range.Minimum) / 50
                'TempPointAnnotation.CaptionAlignment=New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None,(xData(xData.Length - 1) + dblСмещениеХ, yData(yData.Length - 1) + (CWGraphParametr.YAxes(NAxes).Range.Maximum - CWGraphParametr.YAxes(NAxes).Range.Minimum) / 100))
                'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None, xoffset, yoffset)
                newPointAnnotation.CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                newPointAnnotation.CaptionForeColor = newScatterPlot.LineColor

                newPointAnnotation.ShapeFillColor = Color.Red
                newPointAnnotation.ShapeSize = New Size(5, 5)
                newPointAnnotation.ShapeStyle = ShapeStyle.Oval
                newPointAnnotation.ShapeZOrder = AnnotationZOrder.AbovePlot
                newPointAnnotation.XAxis = XAxis1
                'TempPointAnnotation.XPosition = arrТочкиX(I)
                newPointAnnotation.YAxis = newScatterPlot.YAxis
                'TempPointAnnotation.YPosition = arrТочкиY(I)
                'TempPointAnnotation.SetPosition(xData(xData.Length - 1), yData(yData.Length - 1))
            Loop
            rdr.Close()

            If countDiagramFromParameter = 0 Then
                Dim text As String = "Отсутствуют шлейфы параметров для прикрепленния к осям!" & vbCrLf & "Графики от параметров отображаться не будут."
                MessageBox.Show(text, NameOf(RestoreDiagramFromParameter), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"Считывание конфигурации графиков <{NameOf(RestoreDiagramFromParameter)}> {text}")
                isShowDiagramFromParameter = False
                Exit Sub
            Else
                XyCursor1.Plot = ScatterGraphParameter.Plots(0)
                XyCursor1.YPosition = XyCursor1.Plot.YAxis.Range.Minimum
            End If

            ' вернулись к графиками
            sSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " &
                "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " &
                "WHERE ((([КонфигурацииОтображения].keyКонфигурацияОтображения)= " & mKeyConfiguration & "));"
            Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter(sSQL, cn)
            Dim dtDataTable As New DataTable
            Dim countTable As Integer

            odaDataAdapter.Fill(dtDataTable)
            countTable = dtDataTable.Rows.Count

            For I = 0 To ScatterGraphParameter.YAxes.Count - 1
                ' просмотр всех графиков принадлежащих данной оси
                caption = vbNullString

                For J As Integer = 0 To countTable - 1
                    nameParameter = Convert.ToString(dtDataTable.Rows(J)("ИмяПараметра"))
                    If ScatterGraphParameter.YAxes(I).Tag.ToString = CStr(dtDataTable.Rows(J)("НомерОси")) Then
                        caption = $"{caption}{nameParameter}  "
                    End If
                Next

                ScatterGraphParameter.YAxes(I).Caption = caption
            Next

            isShowDiagramFromParameter = True
        Else
            Dim captionMsg As String = $"Процедура <{NameOf(RestoreDiagramFromParameter)}>"
            Dim text As String = $"Отсутствует запись с индексом № {mKeyConfiguration} в таблице КонфигурацииОтображения.{vbCrLf}Индекс записан в файле настроек Опции.ini в ключе <[Stend]:keyКонфигурация>"
            MessageBox.Show(text, captionMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"{captionMsg} {text}")
        End If

        cn.Close()
    End Sub
    ''' <summary>
    ''' Настроить отображение параметров
    ''' </summary>
    Protected Sub TuneRepresentationParameter()
        MarkSheetParametersByColour()

        If isShowDiagramFromTime Then
            arraysizeDiagramFromParameter = CInt(arraysize / CounterParametersGraph)
            'ReDim_dataDiagramFromParameter(countDiagramFromParameter - 1, arraysizeDiagramFromParameter)
            'ReDim_axesXDiagramFromParameter(arraysizeDiagramFromParameter)
            Re.Dim(dataDiagramFromParameter, countDiagramFromParameter - 1, arraysizeDiagramFromParameter)
            Re.Dim(axesXDiagramFromParameter, arraysizeDiagramFromParameter)

            For I As Integer = 0 To arraysizeDiagramFromParameter
                axesXDiagramFromParameter(I) = I
            Next

            XyCursor1.Visible = True
            XAxis1.Range = New Range(0, arraysizeDiagramFromParameter)
            YAxis1.AutoMinorDivisionFrequency = 4
            YAxis1.CaptionForeColor = Color.White
            YAxis1.MajorDivisions.GridVisible = True
            YAxis1.MajorDivisions.LabelForeColor = Color.White
            YAxis1.MajorDivisions.TickColor = Color.White
            YAxis1.MinorDivisions.GridVisible = True
            YAxis1.MinorDivisions.TickColor = Color.White
            positionCursorDiagramFromParameter = 0
        Else
            'ReDim_isClearDataDiagramFromParameter(countDiagramFromParameter - 1)
            'ReDim_dataDiagramFromParameter(countDiagramFromParameter - 1, CounterLightParametersGraph - 1)
            'ReDim_axesXDiagramFromParameter(CounterLightParametersGraph - 1)
            Re.Dim(isClearDataDiagramFromParameter, countDiagramFromParameter - 1)
            Re.Dim(dataDiagramFromParameter, countDiagramFromParameter - 1, CounterLightParametersGraph - 1)
            Re.Dim(axesXDiagramFromParameter, CounterLightParametersGraph - 1)
            isClearDiagramFromParameterAxesX = True
            XyCursor1.Visible = False
            YAxis1.AutoMinorDivisionFrequency = 4
            'Me.YAxis1.CaptionForeColor = System.Drawing.Color.White
            YAxis1.MajorDivisions.GridVisible = True
            'Me.YAxis1.MajorDivisions.LabelForeColor = System.Drawing.Color.White
            'Me.YAxis1.MajorDivisions.TickColor = System.Drawing.Color.White
            YAxis1.MinorDivisions.GridVisible = True
            'Me.YAxis1.MinorDivisions.TickColor = System.Drawing.Color.White
        End If
    End Sub
    ''' <summary>
    ''' Пометить цветом лист параметров
    ''' </summary>
    Private Sub MarkSheetParametersByColour()
        Dim itmX As ListViewItem

        If AllGraphParametersByParameter IsNot Nothing Then
            ListViewParametr.BeginUpdate()

            For Each itmX In ListViewParametr.Items
                For I As Integer = 1 To UBound(AllGraphParametersByParameter)
                    If AllGraphParametersByParameter(I).NameParameter = itmX.Text Then
                        If AllGraphParametersByParameter(I).IndexColor <> -1 Then
                            'If arrГрафикОтПараметров(I).intЦвет > ColorsNet.Length - 1 Then arrГрафикОтПараметров(I).intЦвет = 0
                            itmX.ForeColor = ColorsNet(AllGraphParametersByParameter(I).IndexColor)
                            'Else
                            '    itmX.ForeColor = Color.Silver
                        End If
                        Exit For
                    End If
                Next I
            Next

            ListViewParametr.EndUpdate()
        End If
    End Sub

    'новая версия
    'Private Sub НарисоватьГраницы(ByRef cn As System.Data.OleDb.OleDbConnection)
    '    Dim sSQL As String
    '    Dim rdr As OleDbDataReader
    '    Dim cmd As OleDbCommand = cn.CreateCommand
    '    cmd.CommandType = CommandType.Text
    '    Dim I, intНомерОси, keyИмяПараметра As Integer
    '    Dim strИмяПараметра, NamePlot As String
    '    Dim Color As String = Nothing
    '    Dim LineStyle As String = Nothing

    '    For I = 0 To UBound(arrГрафикОтПараметров)
    '        If arrГрафикОтПараметров(I).ОтображатьШлейф Then
    '            strИмяПараметра = arrГрафикОтПараметров(I).strНаименованиеПараметра
    '            intНомерОси = arrГрафикОтПараметров(I).intНомерОси
    '            'узнать все имена границ
    '            sSQL = "SELECT ПараметрДляГраниц.keyИмяПараметра, Plots.NamePlot " & _
    '            "FROM ПараметрДляГраниц RIGHT JOIN Plots ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " & _
    '            "WHERE (((ПараметрДляГраниц.ИмяПараметра)='" & strИмяПараметра & "'));"
    '            cmd.CommandText = sSQL
    '            rdr = cmd.ExecuteReader

    '            If Not rdr.HasRows Then
    '                rdr.Close()
    '            Else
    '                'добавить имена Plots в коллекцию
    '                Dim NamePlots As New List(Of String)
    '                Do While rdr.Read
    '                    NamePlots.Add(rdr("NamePlot"))
    '                    keyИмяПараметра = CInt(rdr("keyИмяПараметра"))
    '                Loop
    '                rdr.Close()

    '                If NamePlots.Count <> 0 Then
    '                    'по каждой границе считать точки
    '                    For Each NamePlot In NamePlots
    '                        sSQL = "SELECT Plots.Color, Plots.LineStyle, Точки.НомерТочки, Точки.X, Точки.Y " & _
    '                        "FROM ПараметрДляГраниц RIGHT JOIN (Plots RIGHT JOIN Точки ON Plots.keyPlot = Точки.keyPlot) ON ПараметрДляГраниц.keyИмяПараметра = Plots.keyИмяПараметра " & _
    '                        "WHERE(((ПараметрДляГраниц.keyИмяПараметра) = " & keyИмяПараметра.ToString & ") And ((Plots.NamePlot) = '" & NamePlot & "')) " & _
    '                        "ORDER BY Точки.НомерТочки;"
    '                        cmd.CommandText = sSQL
    '                        rdr = cmd.ExecuteReader
    '                        If rdr.HasRows Then
    '                            Dim arrXData As New List(Of Double)
    '                            Dim arrYData As New List(Of Double)

    '                            Do While rdr.Read
    '                                Color = rdr("Color")
    '                                LineStyle = rdr("LineStyle")
    '                                arrXData.Add(rdr("X"))
    '                                arrYData.Add(rdr("Y"))
    '                            Loop
    '                            ДобавитьPlotДляГраниц(NamePlot, intНомерОси, arrXData.ToArray, arrYData.ToArray, Color, LineStyle) ' intНомерОси Mod 7)
    '                        End If
    '                        rdr.Close()
    '                    Next 'Each NamePlot
    '                End If 'count
    '            End If 'Not rdr.HasRows
    '        End If 'arrГрафикКБПР(I).blnГрафик
    '    Next I 'UBound(arrГрафикКБПР)
    'End Sub

    'Private Sub ДобавитьPlotДляГраниц(ByVal NamePlot As String, ByVal NAxes As Integer, ByRef xData() As Double, ByRef yData() As Double, ByVal Color As String, ByVal LineStyle As String) 'ByVal NColors As Integer)
    '    Dim plot As NationalInstruments.UI.ScatterPlot
    '    Dim xoffset As Single = 10
    '    Dim yoffset As Single = -20

    '    With CWGraphParametr
    '        plot = New NationalInstruments.UI.ScatterPlot
    '        CWGraphParametr.Plots.Add(plot)
    '        'plot.LineColor = ColorsNet(NColors)
    '        plot.LineColor = System.Drawing.Color.FromName(Color)

    '        'plot.LineStyle = NationalInstruments.UI.LineStyle.Solid
    '        Dim values As Array = NationalInstruments.UI.LineStyle.GetValues(plot.LineStyle.UnderlyingType)
    '        'по умолчанию
    '        Dim valueTemp As NationalInstruments.UI.LineStyle = NationalInstruments.UI.LineStyle.Dot
    '        For I = 0 To values.Length - 1
    '            If values.GetValue(I).ToString = LineStyle Then
    '                valueTemp = values.GetValue(I)
    '                Exit For
    '            End If
    '        Next I
    '        plot.LineStyle = CType(valueTemp, NationalInstruments.UI.LineStyle)

    '        plot.PointColor = .YAxes(NAxes).CaptionForeColor
    '        plot.PointStyle = NationalInstruments.UI.PointStyle.EmptyDiamond
    '        plot.XAxis = Me.XAxis1
    '        plot.YAxis = .YAxes(NAxes)

    '        plot.PlotXY(xData, yData)

    '        TempPointAnnotation = New NationalInstruments.UI.XYPointAnnotation
    '        .Annotations.Add(TempPointAnnotation)
    '        TempPointAnnotation.ArrowColor = plot.LineColor
    '        TempPointAnnotation.ArrowHeadStyle = NationalInstruments.UI.ArrowStyle.SolidStealth
    '        TempPointAnnotation.ArrowLineWidth = 1.0!
    '        TempPointAnnotation.ArrowTailSize = New System.Drawing.Size(20, 15)

    '        TempPointAnnotation.Caption = NamePlot
    '        'смещение по моему в пикселях
    '        'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.Auto, CSng(Math.Abs(maximumX - minimumX) * 0.01!), -CSng(Math.Abs(maximumY - minimumY) * 0.1!))
    '        'dblСмещениеХ = (XAxis1.Range.Maximum - XAxis1.Range.Minimum) / 50
    '        'TempPointAnnotation.CaptionAlignment=New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None,(xData(xData.Length - 1) + dblСмещениеХ, yData(yData.Length - 1) + (CWGraphParametr.YAxes(NAxes).Range.Maximum - CWGraphParametr.YAxes(NAxes).Range.Minimum) / 100))

    '        TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None, xoffset, yoffset)
    '        TempPointAnnotation.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    '        TempPointAnnotation.CaptionForeColor = plot.LineColor

    '        TempPointAnnotation.ShapeFillColor = System.Drawing.Color.Red
    '        TempPointAnnotation.ShapeSize = New System.Drawing.Size(5, 5)
    '        TempPointAnnotation.ShapeStyle = NationalInstruments.UI.ShapeStyle.Oval
    '        TempPointAnnotation.ShapeZOrder = NationalInstruments.UI.AnnotationZOrder.AbovePlot
    '        TempPointAnnotation.XAxis = Me.XAxis1
    '        'TempPointAnnotation.XPosition = arrТочкиX(I)
    '        TempPointAnnotation.YAxis = .YAxes(NAxes)
    '        'TempPointAnnotation.YPosition = arrТочкиY(I)
    '        'стрелку на последнюю точку
    '        TempPointAnnotation.SetPosition(xData(xData.Length - 1), yData(yData.Length - 1))
    '    End With
    'End Sub

#Region "CREATE TABLE"
    ''' <summary>
    ''' Создать таблицу КонфигурацииОтображения
    ''' </summary>
    ''' <param name="cn"></param>
    Private Sub CreateTableConfigurationDiagram(ByRef cn As OleDbConnection)
        Dim cmd As OleDbCommand = cn.CreateCommand

        Try
            cmd.CommandText = "CREATE TABLE КонфигурацииОтображения (keyКонфигурацияОтображения INTEGER identity NOT NULL PRIMARY KEY, ИмяКонфигурации VARCHAR(50) NOT NULL, ЧастотаПостроения REAL, ВремяИлиПараметр BIT, ИмяПараметраОсиХ VARCHAR(50), МинОсь SMALLINT, МахОсь SMALLINT, ВремяСвечения SMALLINT, Описание TEXT)" 'NCHAR VARYING (255))"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE ПараметрОтображения (keyИмяПараметра INTEGER identity NOT NULL PRIMARY KEY, keyКонфигурацияОтображения INTEGER, ИмяПараметра VARCHAR(50), НомерЦвета SMALLINT, НомерОси SMALLINT, Примечание TEXT, CONSTRAINT КонфигурацииПараметр FOREIGN KEY (keyКонфигурацияОтображения) REFERENCES КонфигурацииОтображения ON UPDATE CASCADE ON DELETE CASCADE)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE Ось (keyОсь INTEGER identity NOT NULL PRIMARY KEY, keyКонфигурацияОтображения INTEGER, НомерОси SMALLINT, НижнееЗначение REAL, ВерхнееЗначение REAL, НомерЦвета SMALLINT, РасположениеМетки SMALLINT, РасположениеЧисла SMALLINT, CONSTRAINT КонфигурацииОсь FOREIGN KEY (keyКонфигурацияОтображения) REFERENCES КонфигурацииОтображения ON UPDATE CASCADE ON DELETE CASCADE)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "INSERT INTO КонфигурацииОтображения (ИмяКонфигурации, ЧастотаПостроения, ВремяИлиПараметр, ИмяПараметраОсиХ, МинОсь, МахОсь, ВремяСвечения, Описание) VALUES ('Проба', 1, 0, 'аРУД', 0, 115, 10, 'Пробный');"
            cmd.ExecuteNonQuery()
            ' ключевое поле равно 1
            ' параметры
            cmd.CommandText = "INSERT INTO ПараметрОтображения (keyКонфигурацияОтображения, ИмяПараметра, НомерЦвета, НомерОси, Примечание) VALUES (1, 'N1', 0, 0, 'N1');"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO ПараметрОтображения (keyКонфигурацияОтображения, ИмяПараметра, НомерЦвета, НомерОси, Примечание) VALUES (1, 'N2', 1, 1, 'N2');"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO ПараметрОтображения (keyКонфигурацияОтображения, ИмяПараметра, НомерЦвета, НомерОси, Примечание) VALUES (1, 'ДиаметрРС', 2, 2, 'ДиаметрРС');"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO ПараметрОтображения (keyКонфигурацияОтображения, ИмяПараметра, НомерЦвета, НомерОси, Примечание) VALUES (1, 'R изм', 3, 3, 'R изм');"
            cmd.ExecuteNonQuery()
            'оси
            cmd.CommandText = "INSERT INTO Ось (keyКонфигурацияОтображения, НомерОси, НижнееЗначение, ВерхнееЗначение, НомерЦвета, РасположениеМетки, РасположениеЧисла) VALUES (1, 0, 0, 100, 0, 1, 1);"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO Ось (keyКонфигурацияОтображения, НомерОси, НижнееЗначение, ВерхнееЗначение, НомерЦвета, РасположениеМетки, РасположениеЧисла) VALUES (1, 1, 0, 110, 1, 2, 2);"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO Ось (keyКонфигурацияОтображения, НомерОси, НижнееЗначение, ВерхнееЗначение, НомерЦвета, РасположениеМетки, РасположениеЧисла) VALUES (1, 2, 0, 120, 2, 1, 1);"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO Ось (keyКонфигурацияОтображения, НомерОси, НижнееЗначение, ВерхнееЗначение, НомерЦвета, РасположениеМетки, РасположениеЧисла) VALUES (1, 3, 0, 1000, 3, 2, 2);"
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim caption As String = $"Невозможно создание отсутствующей таблицы <{NameOf(CreateTableConfigurationDiagram)}>!"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    'Private Sub СоздатьТаблицуТарировкиТяги(ByRef cn As System.Data.OleDb.OleDbConnection)
    '    Dim cmd As OleDbCommand = cn.CreateCommand
    '    Try
    '        'здесь как кдюч
    '        cmd.CommandText = "CREATE TABLE ТарировкаТяги (Nточки SMALLINT CONSTRAINT Ключ1 PRIMARY KEY , ПрямойХодКонтрольный FLOAT , ПрямойХодИзмерительный FLOAT, ОбратныйХодКонтрольный FLOAT, ОбратныйХодИзмерительный FLOAT)" 'NCHAR VARYING (255))"
    '        cmd.ExecuteNonQuery()
    '        'здесь как счетчик
    '        'cmd.CommandText = "CREATE TABLE ТарировкаТяги (Nточки SMALLINT identity NOT NULL PRIMARY KEY, ПрямойХодКонтрольный FLOAT , ПрямойХодИзмерительный FLOAT, ОбратныйХодКонтрольный FLOAT, ОбратныйХодИзмерительный FLOAT)" 'NCHAR VARYING (255))"
    '        'cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (1,0,0,0,0);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (2,1000,1000,1000,1000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (3,2000,2000,2000,2000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (4,3000,3000,3000,3000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (5,5000,5000,5000,5000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (6,7000,7000,7000,7000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (7,9000,9000,9000,9000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (8,11000,11000,11000,11000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (9,12000,12000,12000,12000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (10,13000,13000,13000,13000);"
    '        cmd.ExecuteNonQuery()
    '        cmd.CommandText = "INSERT INTO ТарировкаТяги (Nточки, ПрямойХодКонтрольный, ПрямойХодИзмерительный, ОбратныйХодКонтрольный, ОбратныйХодИзмерительный) VALUES (11,14000,14000,14000,14000);"
    '        cmd.ExecuteNonQuery()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.ToString, "Невозможно создание отсутствующей таблицы <ТарировкаТяги>!", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        'Finally
    '    End Try
    'End Sub
#End Region

#End Region

#Region "ListView"
    ''' <summary>
    ''' Включение подробного/выборочного листа
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonDetailSelective_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDetailSelective.CheckedChanged
        DetailSelective(ButtonDetailSelective.Checked)
    End Sub

    ''' <summary>
    ''' Включение подробного/выборочного листа
    ''' </summary>
    ''' <param name="selected"></param>
    Private Sub DetailSelective(selected As Boolean)
        If selected Then
            ButtonDetailSelective.Text = "Выборочно"
            TableLayoutPanelMode.RowStyles(1).Height = 30.0!
        Else
            ButtonDetailSelective.Text = "Подробно"
            TableLayoutPanelMode.RowStyles(1).Height = 0.0!
        End If

        ComboBoxSelectiveList.Visible = selected
        isDetailedSheet = Not selected
        CompleteListView()

        PanelMode.Height = CInt(TableLayoutPanelMode.RowStyles(0).Height + TableLayoutPanelMode.RowStyles(1).Height + 2)
        TableLayoutPanelMode.Height = TableLayoutPanelMode.Height

        DetailSelectiveBase()
    End Sub

    ''' <summary>
    ''' Переместить курсор
    ''' </summary>
    ''' <param name="inXyCursor"></param>
    ''' <param name="toForward"></param>
    Protected Sub MoveCursor(inXyCursor As XYCursor, toForward As Boolean)
        Dim positionХ As Double

        If toForward Then ' увеличить
            positionХ = inXyCursor.XPosition + 1.0
            If positionХ > XAxisTime.Range.Maximum Then positionХ = XAxisTime.Range.Maximum
            inXyCursor.XPosition = positionХ
        Else ' уменьшить
            positionХ = inXyCursor.XPosition - 1.0
            If positionХ < XAxisTime.Range.Minimum Then positionХ = XAxisTime.Range.Minimum
            inXyCursor.XPosition = positionХ
        End If
    End Sub

    ''' <summary>
    ''' Настроить стиль списка цифровых значений параметров в обоих листах
    ''' </summary>
    Private Sub CompleteListView()
        RewriteListViewAcquisition(ParametersShaphotType)

        If isDetailedSheet Then
            ListViewAcquisition.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel)
            ListViewParametr.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel)
        Else
            ListViewAcquisition.Font = New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Pixel)
            ListViewParametr.Font = New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Pixel)
        End If

        CloneFromListViewAcquisitionToListViewParametr()

        If isShowDiagramFromParameter Then MarkSheetParametersByColour()
    End Sub

    ''' <summary>
    ''' Копировать значения с листа сбора на лист параметров
    ''' </summary>
    Protected Sub CopyFromListViewAcquisitionToListViewParametr()
        ListViewParametr.BeginUpdate()
        For J As Integer = 0 To ListViewAcquisition.Items.Count - 1
            ListViewParametr.Items(J).SubItems(1).Text() = ListViewAcquisition.Items(J).SubItems(1).Text()
        Next
        ListViewParametr.EndUpdate()
    End Sub
    ''' <summary>
    ''' Клонировать настройки с листа сбора на лист параметров
    ''' </summary>
    Private Sub CloneFromListViewAcquisitionToListViewParametr()
        ListViewParametr.BeginUpdate()
        ListViewParametr.Items.Clear()

        For Each itmX As ListViewItem In ListViewAcquisition.Items
            Dim itmXClone As ListViewItem = CType(itmX.Clone(), ListViewItem)
            itmXClone.ForeColor = Color.WhiteSmoke
            itmXClone.SubItems(0).ForeColor = Color.WhiteSmoke
            ListViewParametr.Items.Add(itmXClone)
        Next

        ListViewParametr.EndUpdate()
    End Sub

    ''' <summary>
    ''' Переписать новыми значениями из замера или снимка
    ''' </summary>
    ''' <param name="refParametersCurentOrSnapshot"></param>
    Protected Sub RewriteListViewAcquisition(ByRef refParametersCurentOrSnapshot() As TypeBaseParameter)
        If UBound(IndexParameters) = 0 Then Exit Sub
        If isRegimeChangeForDecoding = False Then TuneStructureAndArray()

        Dim itmX As ListViewItem
        Dim I As Integer
        Dim J As Integer

        ListViewAcquisition.BeginUpdate()
        ListViewAcquisition.Items.Clear()

        If isRegimeChangeForDecoding Then
            If isDetailedSheet Then
                For I = 1 To UBound(IndexParameters)
                    If IsDataBaseChanged Then ' OrElse снимокСДиска
                        itmX = New ListViewItem(refParametersCurentOrSnapshot(IndexParameters(I)).NameParameter)
                    Else
                        itmX = New ListViewItem(ParametersType(IndexParameters(I)).NameParameter)
                    End If

                    itmX.ForeColor = ColorsNet((I - 1) Mod 7)
                    itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Microsoft Sans Serif", 12, FontStyle.Bold))
                    ListViewAcquisition.Items.Add(itmX)
                Next
            Else ' выборочно'заполнить по количеству
                For I = 1 To UBound(IndexParameters)
                    If SnapshotSmallParameters(I).IsVisible Then
                        itmX = New ListViewItem
                        itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Tahoma", 18, FontStyle.Bold))
                        ListViewAcquisition.Items.Add(itmX)
                    End If
                Next
                ' заполнить по содержанию
                For I = 0 To ListViewAcquisition.Items.Count - 1
                    For J = 1 To UBound(IndexParameters)
                        If I + 1 = SnapshotSmallParameters(J).NumberInList Then
                            itmX = ListViewAcquisition.Items(I)
                            itmX.Text = SnapshotSmallParameters(J).NameParameter
                            itmX.ForeColor = ColorsNet((J - 1) Mod 7)
                            Exit For
                        End If
                    Next
                Next
            End If
        Else
            If isDetailedSheet OrElse RegimeType <> cРегистратор Then
                ' подробно
                For I = 1 To UBound(IndexParameters)
                    If IsDataBaseChanged Then ' OrElse снимокСДиска
                        itmX = New ListViewItem(refParametersCurentOrSnapshot(IndexParameters(I)).NameParameter)
                    Else
                        itmX = New ListViewItem(ParametersType(IndexParameters(I)).NameParameter)
                    End If

                    itmX.ForeColor = ColorsNet((I - 1) Mod 7)
                    itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Microsoft Sans Serif", 12, FontStyle.Bold))
                    ListViewAcquisition.Items.Add(itmX)
                Next
            ElseIf Not (isDetailedSheet) AndAlso RegimeType = cРегистратор Then
                ' выборочно - заполнить по количеству
                For I = 1 To UBound(IndexParameters)
                    If SnapshotSmallParameters(I).IsVisible Then
                        itmX = New ListViewItem
                        itmX.SubItems.Add(CStr(0), Color.White, Color.Black, New Font("Tahoma", 18, FontStyle.Bold))
                        ListViewAcquisition.Items.Add(itmX)
                    End If
                Next
                ' заполнить по содержанию
                For I = 0 To ListViewAcquisition.Items.Count - 1
                    For J = 1 To UBound(IndexParameters)
                        If I + 1 = SnapshotSmallParameters(J).NumberInList Then
                            itmX = ListViewAcquisition.Items(I)
                            itmX.Text = SnapshotSmallParameters(J).NameParameter
                            itmX.ForeColor = ColorsNet((J - 1) Mod 7)
                            Exit For
                        End If
                    Next
                Next
            End If
        End If

        ListViewAcquisition.EndUpdate()
        CloneFromListViewAcquisitionToListViewParametr()
    End Sub
    ''' <summary>
    ''' Инициализировать лист сбора и лист парамет от параметра
    ''' </summary>
    Private Sub InitializeListViews()
        ListViewAcquisition.Columns.Clear()
        ListViewAcquisition.BorderStyle = BorderStyle.Fixed3D
        ListViewAcquisition.View = View.Details
        ListViewAcquisition.LabelEdit = False
        ListViewAcquisition.AllowColumnReorder = False
        ListViewAcquisition.CheckBoxes = False
        ListViewAcquisition.GridLines = True

        ListViewAcquisition.Columns.Add("Параметр", ListViewAcquisition.Width * 4 \ 7, HorizontalAlignment.Left)
        ListViewAcquisition.Columns.Add("Значение", ListViewAcquisition.Width * 3 \ 7, HorizontalAlignment.Left)

        ListViewParametr.Columns.Clear()
        ListViewParametr.BorderStyle = BorderStyle.Fixed3D
        ListViewParametr.View = View.Details
        ListViewParametr.LabelEdit = False
        ListViewParametr.AllowColumnReorder = False
        ListViewParametr.CheckBoxes = False
        ListViewParametr.GridLines = True

        ListViewParametr.Columns.Add("Параметр", ListViewAcquisition.Width * 4 \ 7, HorizontalAlignment.Left)
        ListViewParametr.Columns.Add("Значение", ListViewAcquisition.Width * 3 \ 7, HorizontalAlignment.Left)
    End Sub
    ''' <summary>
    ''' инициировать drag and drop операцию
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ListViewAcquisition_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles ListViewAcquisition.ItemDrag
        If e.Button = MouseButtons.Left Then
            DoDragDrop(e.Item, DragDropEffects.Move Or DragDropEffects.Copy)
        End If
    End Sub

    Private Sub ListViewAcquisition_Resize(sender As Object, e As EventArgs) Handles ListViewAcquisition.Resize
        If ListViewAcquisition.Columns.Count > 1 Then
            ListViewAcquisition.Columns(0).Width = CInt(ListViewAcquisition.Width * 4 / 7)
            ListViewAcquisition.Columns(1).Width = CInt(ListViewAcquisition.Width * 3 / 7)
        End If
    End Sub

    Private Sub ListViewParametr_Resize(sender As Object, e As EventArgs) Handles ListViewParametr.Resize
        If ListViewParametr.Columns.Count > 1 Then
            ListViewParametr.Columns(0).Width = CInt(ListViewParametr.Width * 4 / 7)
            ListViewParametr.Columns(1).Width = CInt(ListViewParametr.Width * 3 / 7)
        End If
    End Sub
#End Region

#Region "Обработчики Меню"
    Private Sub NumericUpDownPrecisionScreen_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownPrecisionScreen.ValueChanged
        Precision = Convert.ToInt32(NumericUpDownPrecisionScreen.Value)
        WriteINI(PathOptions, "Options", "Discredit", CStr(Precision))
        'RegistrationEventLog.EventLog_MSG_USER_ACTION("<Записать Настройки>" & " стенд: " & myOptionData.StendServer & ", LastTCPkeyConfig: " & CStr(keyConfig))
    End Sub

    Private Sub MenuNewWindowRegistration_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowRegistration.Click
        MainMDIFormParent.MenuNewWindowRegistration_Click(eventSender, eventArgs)
        MenuNewWindowRegistration.Enabled = MainMDIFormParent.MenuNewWindowRegistration.Enabled
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowRegistration_Click))
    End Sub

    Private Sub MenuNewWindowSnapshot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowSnapshot.Click
        MainMDIFormParent.MenuNewWindowSnapshot_Click(eventSender, eventArgs)
        MenuNewWindowSnapshot.Enabled = MainMDIFormParent.MenuNewWindowSnapshot.Enabled
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowSnapshot_Click))
    End Sub

    Private Sub MenuNewWindowClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowClient.Click
        MainMDIFormParent.MenuNewWindowClient_Click(eventSender, eventArgs)
        MenuNewWindowClient.Enabled = MainMDIFormParent.MenuNewWindowClient.Enabled
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowClient_Click))
    End Sub

    Private Sub MenuNewWindowTarir_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowTarir.Click
        MainMDIFormParent.MenuNewWindowTarir_Click(eventSender, eventArgs)
        MenuNewWindowTarir.Enabled = False
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowTarir_Click))
    End Sub

    Private Sub MenuNewWindowDBaseChannels_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowDBaseChannels.Click
        MainMDIFormParent.MenuNewWindowDBaseChannels_Click(eventSender, eventArgs)
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowDBaseChannels_Click))
    End Sub

    Private Sub MenuNewWindowEvents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuNewWindowEvents.Click
        MainMDIFormParent.MenuNewWindowEvents_Click(eventSender, eventArgs)
        MenuNewWindowEvents.Enabled = MainMDIFormParent.MenuNewWindowEvents.Enabled
        RegistrationEventLog.EventLog_MSG_USER_ACTION(NameOf(MenuNewWindowEvents_Click))
    End Sub

    Private Sub MenuWindowCascade_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuWindowCascade.Click
        MainMDIFormParent.MenuWindowCascade_Click(eventSender, eventArgs)
    End Sub

    Private Sub MenuWindowTileHorizontal_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuWindowTileHorizontal.Click
        MainMDIFormParent.MenuWindowTileHorizontal_Click(eventSender, eventArgs)
    End Sub

    Private Sub MenuWindowTileVertical_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuWindowTileVertical.Click
        MainMDIFormParent.MenuWindowTileVertical_Click(eventSender, eventArgs)
    End Sub

    Private Sub MenuVisibilityTrend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuVisibilityTrend.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuVisibilityTrend_Click)}> Загрузка формы настройки видимости шлейфов")
        Dim frmFormTuningVisibleTail As New FormTuningVisibleTail(Me, ParametersType, ParametersShaphotType, CopyListOfParameter)
        frmFormTuningVisibleTail.ShowDialog() ' обязательно модально
        ApplyTuningVisibilityMeasuringStub()
    End Sub

    Private Sub MenuSelectiveList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuSelectiveList.Click
        TuneSelectiveList()
    End Sub

    ''' <summary>
    ''' Показать форму настройки и применить выбранную конфигурацию
    ''' </summary>
    Protected Sub ShowFormTuningList()
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(TuneSelectiveList)}> Загрузка формы настройки выборочного списка")
        ' после загрузки опции Запуск сбрасывается в False
        ' настройка вывода параметров
        Dim frmFormTuningSelectiveBase As New FormTuningSelectiveBase(TypeFormTuningSelective.SelectiveControl,
                                                                      Me,
                                                                      ParametersType,
                                                                      CopyListOfParameter)
        frmFormTuningSelectiveBase.ShowDialog() 'обязательно модально
        UpdatAfterTuningList()
    End Sub

    ''' <summary>
    ''' Обновление выборочного списка после изменения
    ''' </summary>
    Private Sub UpdatAfterTuningList()
        TuneStructureAndArray()
        CompleteListView()

        If isUsePens Then TuneAnnotation()
    End Sub

    ''' <summary>
    ''' Настройка структур и массивов
    ''' </summary>
    Protected Sub TuneStructureAndArray()
        Dim selectivelyNames As String()
        Dim J, I As Integer
        Dim clearNames As String()

        'ReDim_clearNames(0)
        'ReDim_SnapshotSmallParameters(UBound(IndexParameters))
        Re.Dim(clearNames, 0)
        Re.Dim(SnapshotSmallParameters, UBound(IndexParameters))

        ' заполнить список всех параметров которые снимаются
        If IsDataBaseChanged Then 'OrElse снимокСДиска и считан снимок с другой конфигурацией - этого условия здесь нет
            For I = 1 To UBound(IndexParameters)
                SnapshotSmallParameters(I).NumberParameter = CShort(IndexParameters(I))
                SnapshotSmallParameters(I).NameParameter = ParametersShaphotType(IndexParameters(I)).NameParameter
                SnapshotSmallParameters(I).UnitOfMeasure = ParametersShaphotType(IndexParameters(I)).UnitOfMeasure
                SnapshotSmallParameters(I).IsVisible = False
            Next
        Else
            For I = 1 To UBound(IndexParameters)
                SnapshotSmallParameters(I).NumberParameter = CShort(IndexParameters(I))
                SnapshotSmallParameters(I).NameParameter = ParametersType(IndexParameters(I)).NameParameter
                SnapshotSmallParameters(I).UnitOfMeasure = ParametersType(IndexParameters(I)).UnitOfMeasure
                SnapshotSmallParameters(I).IsVisible = False
            Next
        End If

        ' настройка вывода параметров в списке 
        Select Case ComboBoxSelectiveList.SelectedIndex
            Case 0   '"СПИСОК" 0
                ' считать из файла строку с параметрами контроля и расшифровать ее в массив
                selectivelyNames = New SettingSelectedParameters(PathResourses, [Enum].GetName(GetType(TypeFormTuningSelective), TypeFormTuningSelective.SelectiveControl)).SelectedParametersAsString
            Case Else
                'Case 1  '"ОБОРОТЫ" 1"%"
                'Case 2  '"ДИСКРЕТНЫЕ" 2"дел"
                'Case 3  '"РАЗРЕЖЕНИЯ" 3"мм"
                'Case 4  '"ТЕМПЕРАТУРЫ" 4"градус"
                'Case 5  '"ДАВЛЕНИЯ" 5"Кгсм"
                'Case 6  '"ВИБРАЦИЯ" 6"мм/с"
                'Case 7  '"ТОКИ" 7"мкА"
                'Case 8  '"РАСХОДЫ" 8"кг/ч"
                'Case 9  '"ТЯГА" 9"кгс"                
                ' шаблон поиска
                Dim patternSearch As String = UnitOfMeasureArray(ComboBoxSelectiveList.SelectedIndex - 1)
                ' создать новый список
                Dim patternNames As New List(Of String)

                For I = 1 To UBound(SnapshotSmallParameters)
                    If SnapshotSmallParameters(I).UnitOfMeasure = patternSearch Then
                        patternNames.Add(SnapshotSmallParameters(I).NameParameter)
                    End If
                Next

                selectivelyNames = patternNames.ToArray
        End Select

        If selectivelyNames.Count = 0 Then selectivelyNames = {ParametersType(IndexParameters(1)).NameParameter}

        ' очистить массив arrНаименование от параметров которых может не быть
        For I = 0 To UBound(selectivelyNames)
            For J = 1 To UBound(SnapshotSmallParameters)
                If SnapshotSmallParameters(J).NameParameter = selectivelyNames(I) Then
                    'ReDimPreserve clearNames(UBound(clearNames) + 1)
                    Re.DimPreserve(clearNames, UBound(clearNames) + 1)
                    clearNames(UBound(clearNames)) = selectivelyNames(I)
                    Exit For
                End If
            Next
        Next

        Dim isVisibleTrends As Boolean
        ' отметить, что данный параметр выводится и на каком месте в листе
        For I = 1 To UBound(SnapshotSmallParameters)
            For J = 1 To UBound(clearNames)
                If SnapshotSmallParameters(I).NameParameter = clearNames(J) Then
                    SnapshotSmallParameters(I).IsVisible = True
                    ' номер в листе не по счетчику а по положению
                    SnapshotSmallParameters(I).NumberInList = CShort(J)
                    isVisibleTrends = True
                    Exit For
                End If
            Next
        Next

        If Not isVisibleTrends Then ' выделить хотя бы один шлейф
            SnapshotSmallParameters(1).IsVisible = True
            'ReDimPreserve clearNames(UBound(clearNames) + 1)
            Re.DimPreserve(clearNames, UBound(clearNames) + 1)
            clearNames(UBound(clearNames)) = ParametersType(IndexParameters(1)).NameParameter
            ParametersType(IndexParameters(1)).IsVisibleRegistration = True
            ParametersType(IndexParameters(1)).IsVisible = True
            SnapshotSmallParameters(1).NumberInList = 1
        End If
    End Sub
    '''' <summary>
    '''' Загрузить последнюю конфигурации выборочного списка
    '''' </summary>
    '''' <returns></returns>
    'Private Function LoadLastConfigurationSelectiveList() As String
    '    Const strSQL As String = "SELECT * FROM Списки ORDER BY Id"
    '    Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
    '    Dim odaDataAdapter As OleDbDataAdapter
    '    Dim dtDataTable As New DataTable
    '    Dim drDataRow As DataRow
    '    Dim lastIdСписки As Integer
    '    Dim lastConfiguration As String = "N1\"

    '    Try
    '        lastIdСписки = CInt(GetIni(PathOptions, "ConfigList", "LastIdСписки", "0"))
    '        cn.Open()
    '        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
    '        odaDataAdapter.Fill(dtDataTable)
    '        cn.Close()

    '        If dtDataTable.Rows.Count > 0 Then
    '            ' в случае, если не будет найдена по LastIdСписки
    '            lastConfiguration = dtDataTable.Rows(dtDataTable.Rows.Count - 1)("Text")

    '            For Each drDataRow In dtDataTable.Rows
    '                If drDataRow("Id") = lastIdСписки Then
    '                    lastConfiguration = drDataRow("Text")
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Const caption As String = "Загрузка конфигурации"
    '        MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex.ToString}")
    '    Finally
    '        If cn.State = ConnectionState.Open Then
    '            cn.Close()
    '        End If
    '    End Try

    '    Return lastConfiguration
    'End Function

    Private Sub MenuCommandClientServer_Click(sender As Object, e As EventArgs) Handles MenuCommandClientServer.Click
        OnMenuCommandClientServerClick(MenuCommandClientServer.Checked)
    End Sub

    Private Sub OnMenuCommandClientServerClick(isChecked As Boolean)
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(OnMenuCommandClientServerClick)}> Показать форму обмена задачами по сети")

        If isChecked Then
            mReaderWriterCommander.ShowFormCommand()
        Else
            mReaderWriterCommander.HideFormCommand()
        End If

        UpdateMenuCommandClientServer(isChecked)
    End Sub

    Private Sub UpdateMenuCommandClientServer(isChecked As Boolean)
        If isChecked Then
            MenuCommandClientServer.Text = "Скрыть сетевой обмен"
        Else
            MenuCommandClientServer.Text = "Показать сетевой обмен"
        End If

        MenuCommandClientServer.Checked = isChecked
    End Sub

    Private Sub ReaderWriterCommand_FormCommandClosed(sender As Object, e As FormCommandVisibleClosedEventArg) Handles mReaderWriterCommander.FormCommandClosed
        UpdateMenuCommandClientServer(e.IsVisible)
    End Sub

    Private Sub MenuShowSetting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuShowSetting.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuShowSetting_Click)}> Показать форму настройки опций программы")
        SettingsOptionProgram()
    End Sub

    Private Sub MenuPens_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MenuPens.CheckedChanged
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuPens_CheckedChanged)}> {MenuPens.Checked}")

        If MenuPens.Checked = False Then
            isUsePens = False
            If isRegimeChangeForDecoding Then
                RemoveAnnotationNotAarrow()
            Else
                WaveformGraphTime.Annotations.Clear()
            End If
        Else
            isUsePens = True
            TuneAnnotation()
        End If
    End Sub

    ''' <summary>
    ''' Удалить аннотации  не связанные со стрелками
    ''' </summary>
    Protected Sub RemoveAnnotationNotAarrow()
        If WaveformGraphTime.Annotations.Count = 0 OrElse Arrows.Count = 0 Then Exit Sub

        For I As Integer = WaveformGraphTime.Annotations.Count - 1 To 0 Step -1
            If WaveformGraphTime.Annotations(I).Tag Is Nothing Then
                WaveformGraphTime.Annotations.RemoveAt(I)
            End If
        Next
    End Sub
    ''' <summary>
    ''' Показать окно справки
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuHelpRegime_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuHelpRegime.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuHelpRegime_Click)}> Показать окно справки")
        TextEditorForm = New FormTextEditor
        TextEditorForm.Show()
        TextEditorForm.Activate()
    End Sub

    Private Sub MenuExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuExit.Click
        Close()
    End Sub

    ''' <summary>
    ''' Отображение панели график по времени или по параметру
    ''' </summary>
    Private Sub ShowOrCollapsePanels()
        isShowDiagramFromParameter = ButtonGraphParameter.Checked
        ButtonTuneTrand.Visible = isShowDiagramFromParameter

        SplitContainerGraph1.Visible = ButtonGraphTime.Checked
        SplitContainerGraph2.Visible = ButtonGraphParameter.Checked
        SplitContainerGraph.Panel1Collapsed = Not ButtonGraphTime.Checked
        SplitContainerGraph.Panel2Collapsed = Not ButtonGraphParameter.Checked

        MenuGraphAlongTime.Checked = ButtonGraphTime.Checked
        MenuGraphAlongParameters.Checked = ButtonGraphParameter.Checked

        If ButtonGraphTime.Checked AndAlso ButtonGraphParameter.Checked Then
            SplitContainerGraph.Panel1Collapsed = False
            SplitContainerGraph.Panel2Collapsed = False
            SplitContainerGraph.SplitterDistance = SplitContainerGraph.Width \ 2
        ElseIf ButtonGraphTime.Checked AndAlso ButtonGraphParameter.Checked = False Then
            SplitContainerGraph.SplitterDistance = SplitContainerGraph.Width
            SplitContainerGraph.Panel1Collapsed = False
            SplitContainerGraph.Panel2Collapsed = True
        ElseIf ButtonGraphTime.Checked = False AndAlso ButtonGraphParameter.Checked Then
            SplitContainerGraph.SplitterDistance = 0
            SplitContainerGraph.Panel1Collapsed = True
            SplitContainerGraph.Panel2Collapsed = False
        ElseIf ButtonGraphTime.Checked = False AndAlso ButtonGraphParameter.Checked = False Then
            SplitContainerGraph.Panel1Collapsed = True
            SplitContainerGraph.Panel2Collapsed = True
        End If
    End Sub
    ''' <summary>
    ''' Отображение панели график по времени
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuGraphAlongTime_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MenuGraphAlongTime.CheckedChanged
        ButtonGraphTime.Checked = MenuGraphAlongTime.Checked
    End Sub
    ''' <summary>
    ''' Отображение панели график по параметру
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuGraphAlongParameters_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MenuGraphAlongParameters.CheckedChanged
        ButtonGraphParameter.Checked = MenuGraphAlongParameters.Checked
    End Sub

    Private Sub MenuAboutProgramm_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuAboutProgramm.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuAboutProgramm_Click)}> Отображение справки")
        AboutForm = New FormAbout
        AboutForm.ShowDialog()
    End Sub
    ''' <summary>
    ''' Отображение справки
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuExplorerHelpApplication_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuExplorerHelpApplication.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuExplorerHelpApplication_Click)}> Отображение справки")
        If frmBrowser IsNot Nothing Then
            frmBrowser.Close()
            frmBrowser = Nothing
        End If
        frmBrowser = New FormWebBrowser
        frmBrowser.Show()
        frmBrowser.brwWebBrowser.Navigate(PathHelps)
        frmBrowser.Activate()
    End Sub
    ''' <summary>
    ''' Отображение панели график по времени
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonGraphTime_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonGraphTime.CheckedChanged
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonGraphTime_CheckedChanged)}> Отображение панели График По Времени ={ButtonGraphTime.Checked}")
        If isFormLoaded Then ShowOrCollapsePanels()
    End Sub
    ''' <summary>
    ''' Отображение панели график по параметру
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonGraphParameter_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonGraphParameter.CheckedChanged
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ButtonGraphParameter_CheckedChanged)}> Отображение панели График По Параметру ={ButtonGraphParameter.Checked}")
        If isFormLoaded Then ShowOrCollapsePanels()
    End Sub

    'Private Sub mnuПостроитьСечения_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuПостроитьСечения.Click
    '    If режим <> sРегистратор Then
    '        Dim mfrmWizard As frmWizard = New frmWizard
    '        mfrmWizard.РодительскаяФорма = Me
    '        mfrmWizard.Show()
    '        mfrmWizard.Activate()
    '    End If
    'End Sub

#End Region

#Region "Обработчики контролов формы"
    ''' <summary>
    ''' Заполнить ComboBoxSelectiveList списком групп по единицам измерения
    ''' </summary>
    ''' <param name="cmb"></param>
    Private Sub FillItemsComboBoxSelectiveList(ByVal cmb As ComboBox)
        'Public arrРазмерности() As String = {"%", "дел", "мм", "градус", "Кгсм", "мм/с", "мкА", "кг/ч", "кгс"}
        'cmb.Items.Add("ВСЕ") '0
        cmb.Items.Add("СПИСОК") '0
        cmb.Items.Add("ОБОРОТЫ") '1"%"
        cmb.Items.Add("ДИСКРЕТНЫЕ") '2"дел"
        cmb.Items.Add("РАЗРЕЖЕНИЯ") '3"мм"
        cmb.Items.Add("ТЕМПЕРАТУРЫ") '4"градус"
        cmb.Items.Add("ДАВЛЕНИЯ") '5"Кгсм"
        cmb.Items.Add("ВИБРАЦИЯ") '6"мм/с"
        cmb.Items.Add("ТОКИ") '7"мкА"
        cmb.Items.Add("РАСХОДЫ") '8"кг/ч"
        cmb.Items.Add("ТЯГА") '9"кгс"
        cmb.SelectedIndex = 0
    End Sub

    Private Sub ComboBoxSelectiveList_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ComboBoxSelectiveList.DrawItem
        DrawItemComboBox(sender, die, Nothing, ImageList2, True)
    End Sub

    Private Sub ComboBoxSelectiveList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxSelectiveList.SelectedIndexChanged
        If ComboBoxSelectiveList.SelectedIndex >= 0 Then DetailSelective(ButtonDetailSelective.Checked)
    End Sub

    Private Sub ComboBoxSelectiveList_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ComboBoxSelectiveList.MeasureItem
        mie.ItemHeight = CType(sender, ComboBox).ItemHeight - 2
    End Sub

    Private Sub ComboBoxSelectAxis_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBoxSelectAxis.SelectedIndexChanged
        If IsSnapshot Then SlidePlot.Value = ComboBoxSelectAxis.SelectedIndex + 1
    End Sub

    Private Sub FormMain_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        FormMainResize()
    End Sub
    Private Sub FormMainResize()
        If isFormLoaded Then
            If isDetailedSheet Then
                ListViewAcquisition.Font = New Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel)
            Else
                ListViewAcquisition.Font = New Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Pixel)
            End If

            If ListViewAcquisition.Columns.Count > 1 Then
                ListViewAcquisition.Columns(0).Width = CInt(ListViewAcquisition.Width * 4 / 7)
                ListViewAcquisition.Columns(1).Width = CInt(ListViewAcquisition.Width * 3 / 7)
            End If

            If ListViewParametr.Columns.Count > 1 Then
                ListViewParametr.Columns(0).Width = CInt(ListViewParametr.Width * 4 / 7)
                ListViewParametr.Columns(1).Width = CInt(ListViewParametr.Width * 3 / 7)
            End If

            SplitContainerGraph.SplitterDistance = SplitContainerGraph.Width \ 2
        End If
    End Sub
    ''' <summary>
    ''' Шаманство для включения 3D стилей контролов
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TimerRealize3D_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerRealize3D.Tick
        TimerRealize3D.Enabled = False
        PulseButtonTakeOffAlarm.Visible = False
        If IsRun Then
            ' вообще-то этот запуск должен быть последним в процедуре Load 
            IsRun = False
            ButtonContinuously.Checked = True
        End If
    End Sub
    ''' <summary>
    ''' Выбор скорости отображения цифровых значений
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBoxDisplayRate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDisplayRate.SelectedIndexChanged
        If isFormLoaded Then
            For Each value As DisplayRate In [Enum].GetValues(GetType(DisplayRate))
                Dim fi As FieldInfo = GetType(DisplayRate).GetField([Enum].GetName(GetType(DisplayRate), value))
                Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

                If dna IsNot Nothing Then
                    If ComboBoxDisplayRate.Text = dna.Description Then
                        TextDisplayRate = DirectCast(value, DisplayRate)
                        WriteINI(PathOptions, "Options", "DisplayRate", TextDisplayRate.ToString)
                        Exit Sub
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Запуск нового сервера или изменение адреса у старого, если он уже запущен
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuServerEnable_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuServerEnable.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuServerEnable_Click)}> Показать форму нового сервера или изменение адреса у старого если он уже запущени")

        If IsFormRunning Then
            ' После загрузки окна Сервера произойдёт заново запуск опроса
            ' и если была включена запись то нужно её возобновить.
            ' Выставить флаг запоминания состояния режима записи.
            IsMemoForStartRecord = True
            StopAcquisition()
        End If

        If IsFormSereverStart Then
            IsServerOn = True
            ServerForm.edURL.Text = "dstp://localhost/wave"
            ServerForm.cmdConnect_Click(ServerForm.cmdConnect, New EventArgs)
            ServerForm.TimerConnect.Enabled = True
        Else ' или если не запущен
            AddressURL = "dstp://localhost/wave"
            ServerForm = New FormServer
            ServerForm.Show()
            ServerForm.Activate()
            IsFormSereverStart = True
        End If
    End Sub
#End Region

#Region "Записи и считывания кадров"
    ''' <summary>
    ''' Размахи по оси Y
    ''' </summary>
    ''' <param name="parametersCurrentOrSnapshot"></param>
    Protected Sub ApplyScaleRangeAxisY(ByRef parametersCurrentOrSnapshot() As TypeBaseParameter)
        Dim I, indexParam As Integer
        Dim разносМин, физМин, физМакс, разносМакс As Double
        Dim разносОсь, размахФиз, размах1пр As Double

        'ReDim_RangesOfDeviation(UBound(IndexParameters), 2)
        Re.Dim(RangesOfDeviation, UBound(IndexParameters), 2)

        For I = 1 To UBound(IndexParameters)
            indexParam = IndexParameters(I)
            физМин = parametersCurrentOrSnapshot(indexParam).LowerLimit
            физМакс = parametersCurrentOrSnapshot(indexParam).UpperLimit
            разносМин = parametersCurrentOrSnapshot(indexParam).RangeYmin
            разносМакс = parametersCurrentOrSnapshot(indexParam).RangeYmax
            размахФиз = физМакс - физМин

            If размахФиз = 0 Then
                физМакс = физМин * 2
                размахФиз = физМакс - физМин
            End If

            разносОсь = разносМакс - разносМин

            If разносОсь = 0 Then
                разносМакс = разносМин * 2
                разносОсь = разносМакс - разносМин
            End If

            размах1пр = размахФиз / разносОсь
            ' ОсьУмин
            RangesOfDeviation(I, 1) = физМин - разносМин * размах1пр
            ' ОсьУмакс
            RangesOfDeviation(I, 2) = физМакс + (100 - разносМакс) * размах1пр
        Next
    End Sub
#End Region

#Region "Выборочный и отладочный режим"
    Private Sub MenuRegistration_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuRegistration.Click
        ApplyRegimeRegistrator()
    End Sub
    Private Sub MenuDebugging_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuDebugging.Click
        ApplyRegimeDebugging()
    End Sub
    ''' <summary>
    ''' Применить режим Регистратор
    ''' </summary>
    Protected Sub ApplyRegimeRegistrator()
        NumberRegime = 1
        RegimeType = cРегистратор
        StatusStripMain.Items(NameOf(LabelRegistration)).Text = cРегистратор
        CharacteristicForRegime()
    End Sub
    ''' <summary>
    ''' Применить Отладочный режим
    ''' </summary>
    Protected Sub ApplyRegimeDebugging()
        NumberRegime = 17
        RegimeType = cОтладочныйРежим
        StatusStripMain.Items(NameOf(LabelRegistration)).Text = cОтладочныйРежим
        CharacteristicForRegime()
    End Sub
#End Region

#Region "РасшифроватьСнимок"
    ''' <summary>
    ''' Наклонная линия
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetInclinedLine() As WaveformPlot
        'plot.PointStyle = PointStyle.SolidDiamond
        Return New WaveformPlot() With {.LineColor = Color.White,
                                        .LineColorPrecedence = ColorPrecedence.UserDefinedColor,
                                        .LineStyle = LineStyle.Dot,
                                        .XAxis = XAxisTime,
                                        .YAxis = YAxisTime}
    End Function
    ''' <summary>
    ''' Относительные координаты для стрелки
    ''' </summary>
    ''' <param name="inArrow"></param>
    Protected Sub ApplyDifferentialCoordinates(ByRef inArrow As Arrow)
        ' ось Х остается неизмененный, ось У приводится
        xData(0) = XyCursorStart.XPosition
        yData(0) = XyCursorStart.YPosition
        xData(1) = XyCursorEnd.XPosition
        yData(1) = XyCursorEnd.YPosition

        inArrow.X1 = xData(0)
        inArrow.Y1 = (yData(0) - YAxisTimeRange.Minimum) / (YAxisTimeRange.Maximum - YAxisTimeRange.Minimum)
        inArrow.X2 = xData(1)
        inArrow.Y2 = (yData(1) - YAxisTimeRange.Minimum) / (YAxisTimeRange.Maximum - YAxisTimeRange.Minimum)
    End Sub
    ''' <summary>
    ''' Добавить примечание к стрелке
    ''' </summary>
    ''' <param name="inCaption"></param>
    ''' <param name="xPosition"></param>
    ''' <param name="yPosition"></param>
    ''' <returns></returns>
    Protected Function AddAnnotationToArrow(ByVal inCaption As String, ByVal xPosition As Double, ByVal yPosition As Double) As XYPointAnnotation
        'смещение по моему в пикселях
        Dim newPointAnnotation As New XYPointAnnotation With {
            .Tag = inCaption,' признак добавленной стрелки
            .ArrowVisible = True,
            .ArrowColor = Color.White,
            .ArrowHeadStyle = ArrowStyle.SolidStealth,
            .ArrowLineWidth = 1.0!,
            .ArrowTailSize = New Size(20, 15),
            .Caption = inCaption,
            .CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte)),
            .CaptionForeColor = Color.White, '_ScatterPlot.LineColor
            .ShapeFillColor = Color.Red,
            .ShapeSize = New Size(5, 5),
            .ShapeStyle = ShapeStyle.Oval,
            .ShapeZOrder = AnnotationZOrder.AbovePlot,
            .InteractionMode = AnnotationInteractionMode.DragCaption,
            .XAxis = XAxisTime,
            .YAxis = YAxisTime, ' GraphTime.Plots.Item(I - 1).YAxis '_ScatterPlot.YAxis
            .CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 0, 20)
        }
        WaveformGraphTime.Annotations.Add(newPointAnnotation)
        newPointAnnotation.SetPosition(xPosition, yPosition)

        Return newPointAnnotation
    End Function
    ''' <summary>
    ''' Вставляется метка с линией в кадр
    ''' </summary>
    ''' <param name="description"></param>
    Protected Sub AddMarking(ByVal description As String)
        XyCursorStart.YPosition = YAxisTime.Range.Minimum
        XyCursorEnd.YPosition = YAxisTime.Range.Maximum

        Dim newArrow As New Arrow With {
            .Legend = description,
            .ViewArrow = ArrowType.Inclined
        }
        ApplyDifferentialCoordinates(newArrow)

        Dim plot As WaveformPlot = GetInclinedLine()
        WaveformGraphTime.Plots.Add(plot)
        plot.PlotY(yData, xData(0), xData(1) - xData(0))
        newArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
        newArrow.PointAnnotation = AddAnnotationToArrow(description, xData(0), yData(0) + (yData(1) - yData(0)) / 2)

        Arrows.Add(newArrow)
        UpdateListDescription()
        ComboBoxDescriptionPointer.SelectedIndex = ComboBoxDescriptionPointer.Items.Count - 1
    End Sub

    ''' <summary>
    ''' Вызывается из подключаемх модулей расчёта контрольных точек
    ''' для обозначения места начала их сбора при просмотре снимков
    ''' </summary>
    ''' <param name="captionKT"></param>
    Private Sub AddMarkingKT(ByVal captionKT As String)
        XAxisTimeRange = New Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
        YAxisTimeRange = New Range(YAxisTime.Range.Minimum, YAxisTime.Range.Maximum)
        XyCursorStart.XPosition = XyCursorTime.XPosition ' CDbl(DataConverter.Convert(precisionTimeStart.AddMilliseconds(indexTimeVsRow * defaultInterval.TotalMilliseconds).ToDateTime, Type.GetType("System.Double"))) 'indexTimeVsRow
        XyCursorEnd.XPosition = XyCursorTime.XPosition ' indexTimeVsRow
        AddMarking(captionKT)
        ' значит вызван из ДобавитьМеткуКТ и надо координаты индекса а не времени
        Arrows.Last.X1 = indexTimeVsRow
        Arrows.Last.X2 = indexTimeVsRow
    End Sub

    ''' <summary>
    ''' Считать конфигурацию
    ''' </summary>
    Protected Sub ReadConfigurationRegime()
        CheckTableNameRegime()

        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        cmd.CommandText = $"Select * FROM [Режимы{StandNumber}] WHERE [номерРежима]= {NumberRegime}"
        cn.Open()
        rdr = cmd.ExecuteReader

        If rdr.Read() = True Then
            ConfigurationString = CStr(rdr("ПереченьПараметров"))
            If Not IsDBNull(rdr("ТекстСправки")) Then HelpString = CStr(rdr("ТекстСправки")) Else HelpString = "Описание отсутствует"
        End If

        rdr.Close()
        cn.Close()
    End Sub

    ''' <summary>
    ''' Распаковка строки конфигурации в массив имён
    ''' с модификацией исходной строки
    ''' </summary>
    ''' <param name="configurationString"></param>
    Protected Sub UnpackStringConfigurationWithEmpty(ByRef configurationString As String)
        Dim start As Integer = 1
        Dim temp As String
        Dim lenghtString As Integer = Len(configurationString)
        Dim names As New List(Of String) From {String.Empty} ' с пустым элементом

        Do
            temp = Mid(configurationString, start, InStr(start, configurationString, Separator) - start)
            For J As Integer = 1 To UBound(ParametersType)
                If ParametersType(J).NameParameter = temp Then
                    names.Add(temp)
                    Exit For
                End If
            Next

            start = InStr(start, configurationString, Separator) + 1
        Loop While start < lenghtString

        ' заново сформировать строку - она передана ссылке
        configurationString = vbNullString ' String.Empty

        If names.Count = 1 Then ' значит есть только String.Empty
            'ReDim_NamesParameterRegime(1)
            Re.Dim(NamesParameterRegime, 1)
            NamesParameterRegime(0) = String.Empty
            NamesParameterRegime(1) = ParametersType(1).NameParameter
        Else
            NamesParameterRegime = names.ToArray()
        End If

        For I As Integer = 1 To UBound(NamesParameterRegime)
            configurationString &= NamesParameterRegime(I) & "\"
        Next
    End Sub
#End Region

#Region "ReaderWriterCommand"
    ''' <summary>
    ''' Загрузить форму обмена командами
    ''' </summary>
    Protected Sub LoadClientServerForm()
        MenuCommandClientServer.Enabled = True
    End Sub

#Region "Скажи_текущее_время"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="parameters"></param>
    Public Sub Скажи_текущее_время(ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)

        ' отправится из очереди ReaderWriterCommandClassReaderWriterCommand.SendRequestProgrammed("Установи текущее время", Parameters)
        ' varConfigPidAndChannelstargetForm.gTabPageCollection.Item("Идетификатор компьютера").ПослатьЗапросПрограммно("Установи текущее время", Parameters)
        ' передать Исходящую задачу- в ответе индекс передать тот же
        mReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(УстановиТекущееВремя, {Date.Now.ToLongTimeString}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Исходящая задача.
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="TimeNow"></param>
    Public Sub Установи_текущее_время(TimeNow As DateTime, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show("Текущее время: " & TimeNow.ToShortTimeString, УстановиТекущееВремя, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Set_Polynomial_Channel"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' Обновить коэффициенты полинома заданного канала.
    ''' </summary>
    ''' <param name="НомерКанала"></param>
    ''' <param name="K0"></param>
    ''' <param name="K1"></param>
    ''' <param name="K2"></param>
    ''' <param name="K3"></param>
    ''' <param name="K4"></param>
    ''' <param name="K5"></param>
    ''' <param name="parameters"></param>
    Public Sub Set_Polynomial_Channel(НомерКанала As Integer,
                                      K0 As Double,
                                      K1 As Double,
                                      K2 As Double,
                                      K3 As Double,
                                      K4 As Double,
                                      K5 As Double,
                                      ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        ' передать Исходящую задачу- в ответе индекс передать тот же
        mReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OkSetPolynomialChannel, {"обновление коэффициенов полинома заданного канала произведен"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Ответ - обновление коэффициенов полинома заданного канала произведено.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Set_Polynomial_Channel(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Поставить_метку_КТ"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' </summary>
    ''' <param name="captionKT"></param>
    ''' <param name="parameters"></param>
    Public Sub Поставить_метку_КТ(captionKT As String, ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        AddMarkingKT(captionKT)
        ' передать Исходящую задачу- в ответе индекс передать тот же
        mReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(ОтветПоставитьМеткуКТ, {"Отметка КТ произведена"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub

    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ответ_Поставить_метку_КТ(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "Stop_Client"
    ''' <summary>
    ''' Входящая задача с ответом подтверждения.
    ''' </summary>
    ''' <param name="parameters"></param>
    Public Sub Stop_Client(ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)
        ' чего-то делаем
        ' передать Исходящую задачу- в ответе индекс передать тот же
        mReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OkStopClient, {$"Остановка клиента {inHostName} произведена"}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub
    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Stop_Client(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' чего-то делаем
        'MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

    ''' <summary>
    ''' не удалять, вызывается косвенно
    ''' </summary>
    Public Sub Очистка_линии(ByVal ParamArray parameters() As String)
        ' ни чего не делаем
    End Sub

#Region "Send_Message"
    ''' <summary>
    ''' Тестовый метод для проверки прохождения команд.
    ''' </summary>
    ''' <param name="textMessage"></param>
    Public Sub Send_Message(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        Dim inHostName As String = parameters(0)
        Dim indexResponse As String = parameters(1)

        ' чего-то делаем
        MessageBox.Show(textMessage, $"Пришло сообщение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ' послать что принято к сведению
        mReaderWriterCommander.ManagerAllTargets.Targets(inHostName).CommandWriterQueue.Enqueue(New NetCommandForTask(OKSendMessage, {$"Сообщение от {parameters(0)} принято к исполнению."}) With {.IsResponse = True, .IndexResponse = indexResponse})
    End Sub
    ''' <summary>
    ''' Ответ.
    ''' Исходящая задача.
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="parameters"></param>
    Public Sub Ok_Send_Message(ByVal textMessage As String, ByVal ParamArray parameters() As String)
        ' послать что клиент принял к сведению
        MessageBox.Show(textMessage, $"Подтверждение от {parameters(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#End Region

#Region "Работа с графиками от параметров"
    ''' <summary>
    ''' Делегат обработчика события меню графика от параметра
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuItemGraphOfParam_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        Try
            If selectedItem.CheckState = CheckState.Checked Then
                ' загрузить 
                If mGraphsOfParams Is Nothing Then
                    mGraphsOfParams = New GraphsOfParameters(Me)
                End If

                If mGraphsOfParams.Add(selectedItem.Text) Then
                    isUseWindowsDiagramFromParameter = True
                    ReloadDiagramParameterFromParameterForSnapshot(mGraphsOfParams.Item(selectedItem.Text))
                Else
                    isUseWindowsDiagramFromParameter = mGraphsOfParams.Count > 0
                End If
            Else ' выгрузить 
                ' вначале выгрузить из mGraphsOfParams
                mGraphsOfParams.Remove(selectedItem.Text)
                If GraphsByParameters IsNot Nothing Then
                    If GraphsByParameters.ContainsKey(selectedItem.Text) Then
                        GraphsByParameters.Item(selectedItem.Text).CloseForm() ' там и удаляется
                    End If
                End If

                isUseWindowsDiagramFromParameter = mGraphsOfParams.Count > 0
            End If
        Catch ex As Exception
            Dim caption As String = selectedItem.Text
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Остановить сбор.
    ''' Отобразить редактор параметр от параметра.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuEditGraphOfParameter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuEditGraphOfParameter.Click
        StopAcquisition()
        ShowFormEditiorGraphByGraph()
    End Sub

    ''' <summary>
    ''' Отобразить редактор параметр от параметра и применить настройки
    ''' </summary>
    Private Sub ShowFormEditiorGraphByGraph()
        DeleteAllMenuPatternGraphByParameter()
        MenuEditGraphOfParameter.Enabled = False
        Dim frmEditiorGraphByParameter As New FormEditiorGraphByParameter(Me)
        frmEditiorGraphByParameter.Show()
    End Sub

    ''' <summary>
    ''' Снять выделение с меню
    ''' </summary>
    ''' <param name="namePatternGraphByParameter"></param>
    Friend Sub ClearMarkFromMenu(ByVal namePatternGraphByParameter As String)
        For I As Integer = 2 To MenuGraphCheckedList.DropDownItems.Count - 1
            Dim itemToRemove As ToolStripMenuItem = CType(MenuGraphCheckedList.DropDownItems(I), ToolStripMenuItem)
            If itemToRemove.Text = namePatternGraphByParameter Then
                mGraphsOfParams.Remove(namePatternGraphByParameter)
                itemToRemove.Checked = False
            End If
        Next

        isUseWindowsDiagramFromParameter = mGraphsOfParams.Count > 0
    End Sub

    ''' <summary>
    ''' Удалить Все Меню Графиков Границ
    ''' </summary>
    Private Sub DeleteAllMenuPatternGraphByParameter()
        If mGraphsOfParams IsNot Nothing Then
            mGraphsOfParams.Clear()
        End If

        If GraphsByParameters IsNot Nothing Then
            Dim keyColl As Dictionary(Of String, FormPatternGraphByParameter).KeyCollection = GraphsByParameters.Keys
            Dim namesGraphs(keyColl.Count - 1) As String
            keyColl.CopyTo(namesGraphs, 0)

            For Each itemKeyGraph As String In namesGraphs
                GraphsByParameters.Item(itemKeyGraph).CloseForm()
            Next

            ' это не работает из-за модификации Dictionary 
            'For Each itemKeyGraph As FormPatternGraphByParameter In GraphsByParameters.Values
            '    GraphsByParameters.Item(itemKeyGraph.Name).ВыгрузитьФорму()
            'Next
            'For i As Integer = GraphsByParameters.Count - 1 To 0 Step -1
            '    GraphsByParameters.Item((i).ВыгрузитьФорму()
            'Next
            GraphsByParameters.Clear() ' по идее она уже чистая
            ' затем очистка коллекции настроек
        End If

        ' далее удалить все пункты меню
        'Dim itemToRemove As ToolStripMenuItem = Nothing
        ' 2 пункта надо зарезервировать для добавить и разделитель (удалить нету)
        For i As Integer = MenuGraphCheckedList.DropDownItems.Count - 1 To 2 Step -1
            If MenuGraphCheckedList.DropDownItems.Count > 2 Then
                'itemToRemove = CType(CheckedListMenu.DropDownItems(CheckedListMenu.DropDownItems.Count - 1), ToolStripMenuItem)
                Dim removeAt As Integer = MenuGraphCheckedList.DropDownItems.Count - 1
                'If itemToRemove.Checked And CheckedListMenu.DropDownItems.Count > 4 Then
                '    Dim itemToCheck As ToolStripMenuItem = CType(CheckedListMenu.DropDownItems(CheckedListMenu.DropDownItems.Count - 2), ToolStripMenuItem)
                '    itemToCheck.Checked = True
                'End If
                MenuGraphCheckedList.DropDownItems.RemoveAt(removeAt)
            End If
        Next

        isUseWindowsDiagramFromParameter = False
    End Sub

    ''' <summary>
    ''' Создание меню для графиков от параметров
    ''' </summary>
    Friend Sub PopulateAllMenuPatternGraphByParameter()
        Try
            DeleteAllMenuPatternGraphByParameter()
            XDoc = XDocument.Load(PathXmlFileGraphByParameters)
            AddNodeAndChildren(XDoc.Root)
        Catch ex As Exception
            Const caption As String = "Считывание ГрафикиОтПараметров.xml"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Private Sub AddNodeAndChildren(ByVal xnode As XElement)
        If xnode.Name = XmlNodeNameGraph Then
            Dim newTSMenuItemGraphOfParam As New ToolStripMenuItem() With {.Checked = False, .CheckOnClick = True, .Text = xnode.Attribute(XmlAttributeDescription).Value} ' имя графика от параметра
            ' добавить обработчик события выбора меню.
            AddHandler newTSMenuItemGraphOfParam.Click, AddressOf MenuItemGraphOfParam_Click
            MenuGraphCheckedList.DropDownItems.Add(newTSMenuItemGraphOfParam)
        End If
        ' дальше наверно здесь не будут узлы "ИмяГрафика"
        If xnode.HasElements Then
            For Each itemXElement As XElement In xnode.Elements
                AddNodeAndChildren(itemXElement)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Обновить графики от параметров
    ''' </summary>
    ''' <param name="coefficientBringingTBoxing"></param>
    Protected Sub UpdateGraphByParameter(ByVal coefficientBringingTBoxing As Double)
        For Each itemGraphOfParam As GraphsOfParameters.GraphOfParameter In mGraphsOfParams.DictionaryGraphOfParam.Values
            If itemGraphOfParam.IsTestPass Then
                Dim valX, valY As Double
                Dim result As Double = con9999999

                For Each itemAxis As GraphsOfParameters.GraphOfParameter.Axis In itemGraphOfParam.AxisGraph.Values
                    If itemAxis.IsUseFormula = True Then
                        expressionMath = itemAxis.Formula

                        For Each itemParameter As GraphsOfParameters.GraphOfParameter.Axis.Parameter In itemAxis.Parameters.Values
                            expressionMath = expressionMath.Replace(itemParameter.Name, MeasuredValues(itemParameter.IndexInArrayParameters, indexTimeVsRow).ToString)
                        Next

                        Try
                            Dim Eval As New JScriptUtil.ExpressionEvaluator
                            result = CDbl(Eval.Evaluate(expressionMath))
                        Catch ex As Exception
                            Dim caption As String = expressionMath
                            Dim text As String = ex.ToString
                            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                        End Try
                    Else
                        Dim itemParameter As GraphsOfParameters.GraphOfParameter.Axis.Parameter = itemAxis.ParameterForAxis

                        If itemParameter.Reduction Then
                            result = MeasuredValues(itemParameter.IndexInArrayParameters, indexTimeVsRow) * coefficientBringingTBoxing
                        Else
                            result = MeasuredValues(itemParameter.IndexInArrayParameters, indexTimeVsRow)
                        End If
                    End If

                    If itemAxis.NameAxis = XmlAttributeX Then
                        valX = result
                    Else
                        valY = result
                    End If
                Next

                GraphsByParameters.Item(itemGraphOfParam.NameGraph).UpdateAsquredGraphByParameter(valX, valY)
            End If
        Next
    End Sub
#End Region

#Region "help"
    Private Sub ContentsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuContents.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ContentsToolStripMenuItem_Click)}> Загрузка справки")
        ' показать содержание файла помощи
        Help.ShowHelp(Me, HelpProviderAdvancedCHM.HelpNamespace)
    End Sub

    Private Sub IndexToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuIndex.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(IndexToolStripMenuItem_Click)}> Загрузка справки")
        ' показать индексную вкладку файла помощи
        Help.ShowHelpIndex(Me, HelpProviderAdvancedCHM.HelpNamespace)
    End Sub

    Private Sub SearchToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuSearch.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(SearchToolStripMenuItem_Click)}> Загрузка справки")
        ' показать вкладку поиска файла помощи
        Help.ShowHelp(Me, HelpProviderAdvancedCHM.HelpNamespace, HelpNavigator.Find, "")
    End Sub
#End Region

#Region "График и курсоры"
    ''' <summary>
    ''' Выдать график с настройками по умолчанию
    ''' </summary>
    ''' <param name="I"></param>
    ''' <returns></returns>
    Protected Function GetLineLoopTrand(ByVal I As Integer) As WaveformPlot
        'plot.PointStyle = PointStyle.SolidDiamond
        Return New WaveformPlot() With {.LineColor = ColorsNet((I - 1) Mod 7),
                                        .LineColorPrecedence = ColorPrecedence.UserDefinedColor,
                                        .LineStyle = LineStyle.Solid,
                                        .XAxis = XAxisTime,
                                        .YAxis = YAxisTime}
    End Function

    Private Sub SlidePlot_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SlidePlot.ValueChanged
        TuneDiagramUnderSelectedParameterAxesY()
    End Sub

    ''' <summary>
    ''' Настроить режим графика с курсорами или без них.
    ''' </summary>
    ''' <param name="GraphModeValue"></param>
    Protected Sub SetGraphMode(ByVal GraphModeValue As MyGraphMode)
        GroupBoxAxis.Visible = False

        Select Case GraphModeValue
            Case MyGraphMode.Scaling
                XyCursorStart.Visible = False
                XyCursorEnd.Visible = False
                GroupeBoxCursorStart.Visible = False
                GroupeBoxCursorEnd.Visible = False
                TableLayoutPanelFrameCursor.ColumnStyles(0).Width = 0
                TableLayoutPanelFrameCursor.ColumnStyles(1).Width = 0
                TableLayoutPanelFrameCursor.ColumnStyles(2).Width = 0

                If isRegimeChangeForDecoding Then
                    RemoveAnnotationNotAarrow()
                Else
                    WaveformGraphTime.Annotations.Clear()
                End If

                Exit Select
            Case MyGraphMode.OneCursor
                XyCursorStart.Visible = True
                XyCursorEnd.Visible = False
                XyCursorStart.MoveCursor((XAxisTime.Range.Minimum + XAxisTime.Range.Maximum) / 2, (YAxisTime.Range.Minimum + YAxisTime.Range.Maximum) / 2)
                GroupeBoxCursorStart.Visible = True
                GroupeBoxCursorEnd.Visible = False
                TableLayoutPanelFrameCursor.ColumnStyles(0).Width = 127
                TableLayoutPanelFrameCursor.ColumnStyles(1).Width = 0
                TableLayoutPanelFrameCursor.ColumnStyles(2).Width = 0

                Exit Select
            Case MyGraphMode.TwoCursors
                XyCursorStart.Visible = True
                XyCursorEnd.Visible = True
                XyCursorStart.MoveCursor((XAxisTime.Range.Minimum + XAxisTime.Range.Maximum) / 2, (YAxisTime.Range.Minimum + YAxisTime.Range.Maximum) / 2)
                XyCursorEnd.MoveCursor(XyCursorStart.XPosition * 1.01, XyCursorStart.YPosition * 1.01)
                GroupBoxAxis.Visible = True
                GroupeBoxCursorStart.Visible = True
                GroupeBoxCursorEnd.Visible = True
                TableLayoutPanelFrameCursor.ColumnStyles(0).Width = 127
                TableLayoutPanelFrameCursor.ColumnStyles(1).Width = 127
                TableLayoutPanelFrameCursor.ColumnStyles(2).Width = 237

                Exit Select
            Case MyGraphMode.DoNothing
                XyCursorStart.Visible = False
                XyCursorEnd.Visible = False
                GroupeBoxCursorStart.Visible = False
                GroupeBoxCursorEnd.Visible = False
                WaveformGraphTime.InteractionMode = GraphInteractionModes.None ' запрет на любое взаимодействие
                TableLayoutPanelFrameCursor.ColumnStyles(0).Width = 0
                TableLayoutPanelFrameCursor.ColumnStyles(1).Width = 0
                TableLayoutPanelFrameCursor.ColumnStyles(2).Width = 0
        End Select

        If isUsePens Then
            TuneAnnotation()
        End If
    End Sub
    ''' <summary>
    ''' Очистка коллекции стрелок
    ''' </summary>
    Protected Sub ClearArrowCollection()
        For I As Integer = Arrows.Count - 1 To 0 Step -1
            RemoveSelectedArrow()
        Next
    End Sub

    ''' <summary>
    ''' Удаление веделенной в списке стрелки
    ''' </summary>
    Protected Sub RemoveSelectedArrow()
        If ComboBoxDescriptionPointer.SelectedIndex <> -1 Then
            Dim removeArrow As Arrow = CType(ComboBoxDescriptionPointer.SelectedItem, Arrow)

            WaveformGraphTime.Annotations.Remove(removeArrow.PointAnnotation)
            ' удалить 1 Plot
            XAxisTimeRange = New Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
            WaveformGraphTime.Plots.Remove(removeArrow.Plot1)
            ' удалить 2 Plot
            If removeArrow.ViewArrow <> ArrowType.Inclined Then WaveformGraphTime.Plots.Remove(removeArrow.Plot2)

            XAxisTime.Range = XAxisTimeRange
            ' удалить из коллекции
            Arrows.Remove(removeArrow)
            UpdateListDescription()
            isSlantingLine = False
        End If
    End Sub
    ''' <summary>
    ''' Обновление списка меток в ComboBoxDescriptionPointer
    ''' </summary>
    Protected Sub UpdateListDescription()
        ComboBoxDescriptionPointer.Items.Clear()

        If Arrows.Count = 0 Then
            ButtonDelete.Visible = False
        Else
            For Each itemArrow As Arrow In Arrows
                ComboBoxDescriptionPointer.Items.Add(itemArrow)
            Next

            ComboBoxDescriptionPointer.SelectedIndex = 0
            ButtonDelete.Visible = True
        End If

        TextBoxCount.Text = CStr(Arrows.Count)
    End Sub

    'Private Function XPixel() As Double
    '    'сколько пиксел на единицу измерения
    '    If Me.ТипИспытания = enumТипИспытания.Регистратор Then
    '        Return GraphTime.PlotAreaBounds.Width / (arraysize * conДельтаХ) ' / cntСекунда
    '    Else
    '        Return GraphTime.PlotAreaBounds.Width / (XAxisTimeRange.Maximum - XAxisTimeRange.Minimum)
    '    End If
    'End Function

    'Private Function YPixel() As Double
    '    'сколько пиксел на единицу измерения
    '    Return GraphTime.PlotAreaBounds.Height / (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
    'End Function

    ''' <summary>
    ''' Переместить аннотации вслед за курсором
    ''' </summary>
    ''' <param name="xPosition">текущая позиция курсора</param>
    ''' <param name="inIndexTimeVsRow">текущий индекс графика</param>
    ''' <param name="isRegistration">является текущий режим Регистратор</param>
    ''' <param name="isMovingCursorNumberOne">двигает курсор с номером один</param>
    Protected Sub MoveAnnotations(xPosition As Double, inIndexTimeVsRow As Integer, isRegistration As Boolean, isMovingCursorNumberOne As Boolean)
        Dim I, J As Integer
        Dim tempPointAnnotation As XYPointAnnotation

        Dim increment As Integer ' когда 2 курсора, то первый нечётный, второй - чётный
        If GraphModeValue = MyGraphMode.TwoCursors Then
            increment = 2
        Else
            increment = 1
        End If

        ' аннотации к второму курсору на 1 сдвинуты
        If isMovingCursorNumberOne Then
            J = Arrows.Count
        Else
            J = 1 + Arrows.Count
        End If

        Dim indexInArr As Integer ' индекс в массиве. Для режима регистратор индекс равен 0, для снимка индек равен позиции курсора
        If isRegistration Then
            indexInArr = 0
        Else
            indexInArr = inIndexTimeVsRow
        End If

        ' для всех курсоров
        With WaveformGraphTime
            Dim mTypeParameter As TypeBaseParameter()
            If IsBeforeThatHappenLoadDbase Then
                mTypeParameter = ParametersShaphotType
            Else
                mTypeParameter = ParametersType
            End If

            Dim annotationsCount As Integer = .Annotations.Count - 1

            If isDetailedSheet Then
                For I = 1 To UBound(IndexParameters)
                    If mTypeParameter(IndexParameters(I)).IsVisible Then
                        If J <= annotationsCount Then
                            tempPointAnnotation = CType(.Annotations(J), XYPointAnnotation)
                            tempPointAnnotation.SetPosition(xPosition, MeasuredValuesToRange(I - 1, indexInArr))
                            tempPointAnnotation.Caption = $"{mTypeParameter(IndexParameters(I)).NameParameter}: {Round(MeasuredValues(I - 1, inIndexTimeVsRow), Precision)}"
                            tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                        End If

                        J += increment
                    End If
                Next
            Else
                For I = 1 To UBound(SnapshotSmallParameters)
                    If SnapshotSmallParameters(I).IsVisible Then
                        If J <= annotationsCount Then
                            tempPointAnnotation = CType(.Annotations(J), XYPointAnnotation)
                            tempPointAnnotation.SetPosition(xPosition, MeasuredValuesToRange(I - 1, indexInArr))
                            tempPointAnnotation.Caption = $"{mTypeParameter(IndexParameters(I)).NameParameter}: {Round(MeasuredValues(I - 1, inIndexTimeVsRow), Precision)}"
                            tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                        End If

                        J += increment
                    End If
                Next
            End If
        End With
    End Sub

    ''' <summary>
    ''' Создание аннотации соответствующий какому-то шлейфу шлейфу
    ''' </summary>
    ''' <param name="indexPlot"></param>
    ''' <param name="inCaption"></param>
    ''' <returns></returns>
    Protected Function NewXYPointAnnotation(indexPlot As Integer, inCaption As String) As XYPointAnnotation
        ' смещение по моему в пикселях
        Return New XYPointAnnotation With {
                            .ArrowVisible = True,
                            .ArrowColor = WaveformGraphTime.Plots.Item(indexPlot).LineColor,
                            .ArrowHeadStyle = ArrowStyle.SolidStealth,
                            .ArrowLineWidth = 1.0!,
                            .ArrowTailSize = New Size(20, 15),
                            .Caption = inCaption,'.Caption = ParametersType(arrIndexParameters(I)).NameParameter
                            .CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte)),
                            .CaptionForeColor = Color.White, '_ScatterPlot.LineColor
                            .ShapeFillColor = Color.Red,
                            .ShapeSize = New Size(5, 5),
                            .ShapeStyle = ShapeStyle.Oval,
                            .ShapeZOrder = AnnotationZOrder.AbovePlot,
                            .InteractionMode = AnnotationInteractionMode.None,
                            .XAxis = XAxisTime,
                            .YAxis = YAxisTime ' GraphTime.Plots.Item(I - 1).YAxis '_ScatterPlot.YAxis
                            }
        'TempPointAnnotation.CaptionAlignment = New NationalInstruments.UI.AnnotationCaptionAlignment(NationalInstruments.UI.BoundsAlignment.None, xoffset, yoffset)
        'TempPointAnnotation.XPosition = arrТочкиX(I)
        'TempPointAnnotation.YPosition = arrТочкиY(I)
        'TempPointAnnotation.SetPosition(xData(xData.Length - 1), yData(yData.Length - 1))
    End Function

    Public Sub UpdateControls() Implements IUpdateSelectiveControls.UpdateControls
        ' если установлен показ выборочного списка и в нём установлено именно его отображение 
        If ComboBoxSelectiveList.SelectedIndex = 0 Then UpdatAfterTuningList()
    End Sub

#End Region

    'Private Sub CheckVersionOs()
    '    ' для регистрации проверить используемую операционную систему Запрос функции GetVersion.
    '    Dim version As Integer = NativeMethods.GetVersion()

    '    If ((version And &H80000000) = 0) Then
    '        NativeMethods.glngWhichWindows32 = NativeMethods.mlngWindowsNT
    '        ' MsgBox "Выполняется Windows NT"
    '    Else
    '        NativeMethods.glngWhichWindows32 = NativeMethods.mlngWindows95
    '        ' MsgBox "Выполняется Windows 95 or 98"
    '    End If
    'End Sub
End Class