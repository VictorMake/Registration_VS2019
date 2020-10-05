Imports System.Drawing.Design
Imports System.Reflection 'для сереализации
Imports System.Runtime.Serialization
Imports System.ComponentModel

Module PropertiesControls
    Public Enum EnumControl
        <Description("Label")>
        wcontrolLabel
        <Description("Switch")>
        wcontrolSwitch
        <Description("Led")>
        wcontrolLed
        <Description("NumericEdit")>
        wcontrolNumericEdit

        <Description("TextBox")>
        wcontrolTextBox
        <Description("Button")>
        wcontrolButton
        <Description("CheckBox")>
        wcontrolCheckBox
        <Description("RadioButton")>
        wcontrolRadioButton
        <Description("Knob")>
        wcontrolKnob
        <Description("Slide")>
        wcontrolSlide

        <Description("Panel")>
        wcontrolPanel
        <Description("GroupBox")>
        wcontrolGroupBox
        <Description("TabControl")>
        wcontrolTabControl
        <Description("TableLayoutPanel")>
        wcontrolTableLayoutPanel
        <Description("FlowLayoutPanel")>
        wcontrolFlowLayoutPanel
        <Description("PictureBox")>
        wcontrolPictureBox

        <Description("Tank")>
        wcontrolTank
        <Description("Gauge")>
        wcontrolGauge
        <Description("Thermometer")>
        wcontrolThermometer
        <Description("Meter")>
        wcontrolMeter

        <Description("WaveformGraph")>
        wcontrolWaveformGraph
        <Description("ScatterGraph")>
        wcontrolScatterGraph

        <Description("Неизв.")>
        Unknown
    End Enum

    Public Enum WaveformType
        <Description("Синус")> _
        SineWave = 0
        <Description("Пила")> _
        TriangleWave = 1
        <Description("Меандр")> _
        SquareWave = 2
        '<Description("Неизв.")> _
        'Unknown
    End Enum

    ' Основная идея этого паттерна состоит в том, что каждый элемент объектной структуры содержит метод Accept (AcceptVisitorRefresh), 
    ' который принимает на вход в качестве аргумента специальный объект, Посетитель, реализующий заранее известный интерфейс. 
    ' Этот интерфейс содержит по одному методу Visit (UpdateTag) для каждого типа узла. Метод Accept в каждом узле должен 
    ' вызывать методы Visit для осуществления навигации по структуре.

    ''' <summary>
    ''' Через контекст вызова context передается дополнительный параметр, нужный для программы.
    ''' Каждый экземпляр унаследованного от PropertiesControlBase реализует IRefreshContextVisitor с методом UpdateTag.
    ''' Перегруженные реализации UpdateTag реализует RefreshContextVisitor
    ''' </summary>
    ''' <typeparam name="C"></typeparam>
    ''' <remarks></remarks>
    Public Interface IRefreshContextVisitor(Of C)
        'Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As LabelProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As SwitchProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As LedProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As NumericEditProperties)

        'Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As TextBoxProperties)
        'Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As ButtonProperties)
        'Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As CheckBoxProperties)
        'Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As RadioButtonProperties)

        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As KnobProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As SlideProperties)

        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As TankProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As GaugeProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As ThermometerProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As MeterProperties)

        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As WaveformGraphProperties)
        Sub UpdateTag(ByVal context As C, ByVal CurrentProrertiesControl As ScatterGraphProperties)
    End Interface

    ''' <summary>
    ''' Общий класс хранения чего-то общего
    ''' Значение переменной используется во всем приложении, когда все экземпляры имеют доступ к одному месту хранения, 
    ''' и если один экземпляр изменяет значение переменной, то все экземпляры получают доступ к обновленному значению.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MyContextClass
        Public Shared InstanceContextClass As MyContextClass
        Public Sub New()
            InstanceContextClass = Me
        End Sub

        ''' <summary>
        ''' Установить Тэг Выделенного Контрола
        ''' Это нечто имеет метод экземпляра
        ''' </summary>
        ''' <param name="strValue"></param>
        ''' <remarks></remarks>
        Public Sub SetValueTagSelectedControl(ByVal strValue As String)
            ' получить выделенный объект из PropertyGrid, который назначен в _hostSurfaceManager как сервис
            ' наверно выделенный объект можно получить непосредственно сразу как Dim selectedComponents As ICollection = _selectionService.GetSelectedComponents()
            ' DirectCast(MainShel._hostSurfaceManager.GetService(GetType(PropertyGrid)), PropertyGrid).SelectedObject.tag = MainShel.SetTag(rootNode)
            ' через службу конструктора получить грид свойств выделенного редактируемого (SelectedObject) объекта и 
            ' присвоить этому контролу в теге строку сериализованного объекта настройки сетевой переменной 
            Dim propertyGridSelObject As PropertyGrid = DirectCast(EditorPanelMotoristForm.HostSurfaceManagerFuncPanel.GetService(GetType(PropertyGrid)), PropertyGrid)
            CType(propertyGridSelObject.SelectedObject, Control).Tag = strValue

            ' было: DirectCast(EditorPanelMotoristForm.HostSurfaceManagerFuncPanel.GetService(GetType(PropertyGrid)), PropertyGrid).SelectedObject.tag = strValue
            EditorPanelMotoristForm.Flush() ' перезапись xmlDocument
        End Sub
    End Class

    ''' <summary>
    ''' При любом изменении в гриде отображающем PropertiesControlBase через вызов метода Обновить
    ''' производится запись строки (сериализующей этот PropertiesControlBase) в тег контрола.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RefreshContextVisitor
        Implements IRefreshContextVisitor(Of MyContextClass)

        ' общий метод вызывается из каждого экземпляра унаследованного от PropertiesControlBase
        Public Shared Sub Update(ByVal CurrentPropCntr As PropertiesControlBase) ', ByVal clPropertyGrid As System.Windows.Forms.PropertyGrid)
            If EditorPanelMotoristForm.propertyGridDesign.SelectedObject Is Nothing Then Exit Sub ' если ещё не создан при начальной загрузки окна

            ' в первый раз надо вызвать через конструктор
            If MyContextClass.InstanceContextClass Is Nothing Then
                CurrentPropCntr.AcceptVisitorRefresh(New MyContextClass, New RefreshContextVisitor()) 'там вызов visitor.UpdateTag(context, Me)
            Else
                'MyContextClass уже создан
                CurrentPropCntr.AcceptVisitorRefresh(MyContextClass.InstanceContextClass, New RefreshContextVisitor()) 'там вызов visitor.UpdateTag(context, Me)
            End If
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As SwitchProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As LedProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As NumericEditProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As KnobProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As SlideProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As TankProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As GaugeProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As ThermometerProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As MeterProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As ScatterGraphProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub

        Private Sub UpdateTag(ByVal context As MyContextClass, ByVal CurrentProrertiesControl As WaveformGraphProperties) Implements IRefreshContextVisitor(Of MyContextClass).UpdateTag
            context.SetValueTagSelectedControl(EditorPanelMotoristForm.SetTag(CurrentProrertiesControl))
        End Sub
    End Class


#Region "PropertiesControlBase"
    ' Используется перегрузка для упрощения реализации метода Accept.
    ' Для использования этой техники необходимо, чтобы все методы посетителя назывались одинаково и различались 
    ' лишь типом параметра. В этом случае код метода Accept будет одинаковым. Компилятор при компиляции сам выберет необходимый метод.

    ''' <summary>
    ''' Указываем базовый класс для класса настраиваемого объекта паттерна:
    ''' Задаем атрибут TypeConverter с параметром PropertySorter для всего класса с настраиваемыми свойствами:
    ''' Базовый класс для данных настроек подключения каналов 
    ''' для редактирования в PropertyGrid.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    <TypeConverter(GetType(PropertySorter))>
    Public MustInherit Class PropertiesControlBase
        Inherits FilterablePropertyBase
        Implements ISerializationSurrogate ' для сереализации

        ' для сереализации
        ''' <summary>
        ''' Заполняет предоставленный System.Runtime.Serialization.SerializationInfo данными, необходимыми для сериализации объекта.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Public Sub GetObjectData(ByVal obj As Object,
                                 ByVal info As SerializationInfo,
                                 ByVal context As StreamingContext) Implements ISerializationSurrogate.GetObjectData

            Dim flags As BindingFlags = BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.Public

            For Each fi As FieldInfo In obj.GetType().GetFields(flags)
                info.AddValue(fi.Name, fi.GetValue(obj))
            Next
        End Sub

        ''' <summary>
        ''' Заполняет объект с помощью сведений в System.Runtime.Serialization.SerializationInfo.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <param name="selector"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetObjectData(ByVal obj As Object,
                                      ByVal info As SerializationInfo,
                                      ByVal context As StreamingContext,
                                      ByVal selector As ISurrogateSelector) As Object Implements ISerializationSurrogate.SetObjectData

            Dim flags As BindingFlags = BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.Public

            For Each fi As FieldInfo In obj.GetType().GetFields(flags)
                fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType))
            Next
            Return obj
        End Function

        'Private ReadOnly _name As String
        Private _name As String

        Protected Sub New(ByVal name As String)
            If name Is Nothing Then
                Throw New ArgumentNullException("Имя ???")
            End If
            _name = name
        End Sub

        '<Browsable(False)> 
        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Property Name() As String
            Get
                'ТипУровня = Me.Тип
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
                'RefreshContextVisitor.Обновить(Me, frmDigitalOutputPort.dtvwDirectory)
            End Set
        End Property

        <Browsable(False)>
        Public Property Тип() As EnumControl = EnumControl.Unknown

        Public Overloads Overrides Function ToString() As String
            Return Name
        End Function

        ''' <summary>
        ''' Обязан быть переопределён в каждом наследующем классе
        ''' </summary>
        ''' <typeparam name="C"></typeparam>
        ''' <param name="context">ByVal context As C - это в методы посетителя требуется передать параметр, этот контекст можно сделать статически типизированным</param>
        ''' <param name="visitor">ByVal visitor As IRefreshContextVisitor(Of C)) - это посетитель</param>
        ''' <remarks></remarks>
        Public MustOverride Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
    End Class

    <Serializable()>
    <TypeConverter(GetType(PropertySorter))>
    Public MustInherit Class PropertiesControlChannelBase
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate ' для сереализации

        Protected Sub New(name As String)
            MyBase.New(name)
        End Sub

        Public MustOverride Property ИмяКанала() As String
        <Browsable(False)>
        Public MustOverride Property ИндексВМассивеПараметров() As Integer
    End Class


#End Region

#Region "SwitchProperties"
    <Serializable()>
    <DisplayName("Switch")>
    <Description("Название созданного Тумблера")>
    Class SwitchProperties
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate ' для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolSwitch
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        Private mНомерУстройства As Integer = 1
        ''' <summary>
        ''' Номер Устройства
        ''' </summary>
        <DisplayName("Номер Устройства")>
        <Description("Номер Устройства платы сбора или корзины сбора")>
        <Category("2. Железо")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberDeviceTypeConverter))>
        Public Property НомерУстройства() As Integer
            Get
                Return mНомерУстройства
            End Get
            Set(ByVal value As Integer)
                mНомерУстройства = value
                RefreshContextVisitor.Update(Me) ', frmDigitalOutputPort.dtvwDirectory)
            End Set
        End Property

        Private mНомерМодуляКорзины As String = ""
        ''' <summary>
        ''' Номер Модуля Корзины
        ''' </summary>
        <DisplayName("Номер Модуля Корзины")>
        <Description("Номер Модуля SCXI в корзине сбора")>
        <Category("2. Железо")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(NumberModuleTypeConverter))>
        Public Property НомерМодуляКорзины() As String
            Get
                Return mНомерМодуляКорзины
            End Get
            Set(ByVal value As String)
                mНомерМодуляКорзины = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерПорта As Integer = 0
        ''' <summary>
        ''' Номер Порта
        ''' </summary>
        <DisplayName("Номер Порта")>
        <Description("Номер Порта в плате сбора или в корзине сбора SCXI")>
        <Category("2. Железо")>
        <PropertyOrder(120)>
        <TypeConverter(GetType(NumberPortTypeConverter))>
        Public Property НомерПорта() As Integer
            Get
                Return mНомерПорта
            End Get
            Set(ByVal value As Integer)
                mНомерПорта = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерБита As Integer = 0
        ''' <summary>
        ''' Номер Бита
        ''' </summary>
        <DisplayName("Номер Бита")>
        <Description("Номер бита является номером цифровой линии порта, значение которого установится в высокий уровень при срабатывании тумблера")>
        <Category("3. Номер Бита")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberBitTypeConverter))>
        Public Property НомерБита() As Integer
            Get
                Return mНомерБита
            End Get
            Set(ByVal value As Integer)
                mНомерБита = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ''' <summary>
        ''' physicalChannelName
        ''' </summary>
        <DisplayName("physicalChannelName")>
        <Description("Полное обозначение канала")>
        <Category("4. ChannelName")>
        <PropertyOrder(100)>
        Shadows ReadOnly Property ToString() As String 'Protected
            Get
                If НомерМодуляКорзины = "" Then
                    'плата
                    Return $"Dev{НомерУстройства}/port{НомерПорта}/line{НомерБита}"
                Else 'модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    Return $"SC{НомерУстройства}Mod{НомерМодуляКорзины}/port{НомерПорта}/line{НомерБита}"
                End If
            End Get
        End Property

        'Public Overloads Overrides Sub AcceptVisitor(Of C)(ByVal context As C, ByVal visitor As IContextVisitor(Of C))
        '    visitor.Visit(context, Me)
        'End Sub

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "LedProperties"
    <Serializable()>
    <DisplayName("Led")>
    <Description("Название созданного Индикатора")>
    Class LedProperties
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolLed
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property


        'НомерУстройства
        'НомерМодуляКорзины'необязат текст
        'НомерПорта

        Private mНомерУстройства As Integer = 1
        ''' <summary>
        ''' Номер Устройства
        ''' </summary>
        <DisplayName("Номер Устройства")>
        <Description("Номер Устройства платы сбора или корзины сбора")>
        <Category("2. Железо")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberDeviceTypeConverter))>
        Public Property НомерУстройства() As Integer
            Get
                Return mНомерУстройства
            End Get
            Set(ByVal value As Integer)
                mНомерУстройства = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерМодуляКорзины As String = ""
        ''' <summary>
        ''' Номер Модуля Корзины
        ''' </summary>
        <DisplayName("Номер Модуля Корзины")>
        <Description("Номер Модуля SCXI в корзине сбора")>
        <Category("2. Железо")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(NumberModuleTypeConverter))>
        Public Property НомерМодуляКорзины() As String
            Get
                Return mНомерМодуляКорзины
            End Get
            Set(ByVal value As String)
                mНомерМодуляКорзины = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерПорта As Integer = 0
        ''' <summary>
        ''' Номер Порта
        ''' </summary>
        <DisplayName("Номер Порта")>
        <Description("Номер Порта в плате сбора или в корзине сбора SCXI")>
        <Category("2. Железо")>
        <PropertyOrder(120)>
        <TypeConverter(GetType(NumberPortTypeConverter))>
        Public Property НомерПорта() As Integer
            Get
                Return mНомерПорта
            End Get
            Set(ByVal value As Integer)
                mНомерПорта = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерБита As Integer = 0
        ''' <summary>
        ''' Номер Бита
        ''' </summary>
        <DisplayName("Номер Бита")>
        <Description("Номер бита является номером цифровой линии порта, значение высокого уровня которого устанавливает индикатор во включённое состояние")>
        <Category("3. Номер Бита")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberBitTypeConverter))>
        Public Property НомерБита() As Integer
            Get
                Return mНомерБита
            End Get
            Set(ByVal value As Integer)
                mНомерБита = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ''' <summary>
        ''' physicalChannelName
        ''' </summary>
        <DisplayName("physicalChannelName")>
        <Description("Полное обозначение канала")>
        <Category("4. ChannelName")>
        <PropertyOrder(100)>
        Shadows ReadOnly Property ToString() As String 'Protected
            Get
                If НомерМодуляКорзины = "" Then
                    'плата
                    Return $"Dev{НомерУстройства}/port{НомерПорта}/line{НомерБита}"
                Else 'модуль SCXI'SC1Mod<slot#>/port0/lineN" 
                    Return $"SC{НомерУстройства}Mod{НомерМодуляКорзины}/port{НомерПорта}/line{НомерБита}"
                End If
            End Get
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "NumericEditProperties"
    <Serializable()>
    <DisplayName("NumericEdit")>
    <Description("Название созданного Индикатора")>
    Class NumericEditProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolNumericEdit
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property
        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого отображается в индикаторе")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        '="=" Or ="<>" Or ="<" Or =">" Or ="между"  Or ="вне"
        Private mОперацияСравнения As String = "="

        ''' <summary>
        ''' Операция Сравнения
        ''' </summary>
        <DisplayName("Операция Сравнения")>
        <Description("Операция сравнения значения канала при определении условия изменения цвета")>
        <Category("3. Условие изменения цвета")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(OperationTypeConverter))>
        Public Property ОперацияСравнения() As String
            Get
                Return mОперацияСравнения
            End Get
            Set(ByVal value As String)
                mОперацияСравнения = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mВеличинаУсловия As Double

        ''' <summary>
        ''' Величина Условия 1
        ''' </summary>
        <DisplayName("Значение")>
        <Description("Значение величины в условии сравнения со значением канала для изменения цвета")>
        <Category("3. Условие изменения цвета")>
        <PropertyOrder(120)>
        Public Property ВеличинаУсловия() As Double
            Get
                Return mВеличинаУсловия
            End Get
            Set(ByVal value As Double)
                'If value <> "" Then
                '    If Not IsNumeric(value) Then
                '        Dim тextMessage As String = "Должна быть цифра!"
                '        MessageBox.Show(тextMessage, "Проверка величины загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '        'СообщениеНаПанель(тextMessage)
                '        Exit Property
                '    End If
                'End If
                mВеличинаУсловия = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        '    Как организовать выбор файла с заданным расширением?
        'Задайте атрибут Editor:
        'Собственно фильтр расширений задается в DocFileEditor:

        'А для свойства, видимость которого зависит от другого свойства – атрибут DynamicPropertyFilter:
        'здесь "Post" атрибут от которого зависит видимость со списком, где видимость включается
        'Также нужно добавить обработчик события PropertyGrid – PropertyValueChanged:

        Private mВеличинаУсловия2 As Double

        ''' <summary>
        ''' Величина Условия 2
        ''' </summary>
        <DisplayName("Значение 2")>
        <Description("Значение величины в условии сравнения со значением канала для изменения цвета")>
        <Category("3. Условие изменения цвета")>
        <PropertyOrder(130)>
        <DynamicPropertyFilter("ОперацияСравнения", "между,вне")>
        Public Property ВеличинаУсловия2() As Double
            Get
                Return mВеличинаУсловия2
            End Get
            Set(ByVal value As Double)
                mВеличинаУсловия2 = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "KnobProperties"
    <Serializable()>
    <DisplayName("Knob")>
    <Description("Название созданного Регулятора")>
    Class KnobProperties
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolKnob
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        Private mНомерУстройства As Integer = 1
        ''' <summary>
        ''' Номер Устройства
        ''' </summary>
        <DisplayName("Номер Устройства")>
        <Description("Номер Устройства платы сбора сбора")>
        <Category("2. Железо")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberDeviceTypeConverter))>
        Public Property НомерУстройства() As Integer
            Get
                Return mНомерУстройства
            End Get
            Set(ByVal value As Integer)
                mНомерУстройства = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерКанала As Integer = 0
        ''' <summary>
        ''' Номер Канала
        ''' </summary>
        <DisplayName("Номер Канала")>
        <Description("Номер канала аналогового вывода ЦАП, который управляет внешним устройством")>
        <Category("2. Железо")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(NumberBitTypeConverter))>
        Public Property НомерКанала() As Integer
            Get
                Return mНомерКанала
            End Get
            Set(ByVal value As Integer)
                mНомерКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ' Как показать свою картинку для каждого значения из перечисления?
        ' Необходимо реализвать своего наследника от UITypeEditor с кодом отрисовки (в данном случае изображения хранятся в ресурсах с именами, соответствующими именам членов перечисления):
        ' смотри public class SexEditor
        ' и привязать его с помощью атрибута Editor к редактируемому свойству:
        Private typeWaveformType As WaveformType = WaveformType.SineWave
        ''' <summary>
        ''' Тип Генератора
        ''' </summary>
        <DisplayName("Тип Генератора")>
        <Description("Тип Генератора определяет фому генерируемого сигнала")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(EnumTypeConverter))>
        <Editor(GetType(ГенераторEditor), GetType(UITypeEditor))>
        Public Property ТипГенератора() As WaveformType
            Get
                Return typeWaveformType
            End Get
            Set(ByVal value As WaveformType)
                typeWaveformType = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ' Как организовать выбор значения из выпадающего списка, формируемого программно?
        ' Необходимо реализовать TypeConverter, предоставляющий список, из которого можно будет делать выбор: 
        ' смотри class PostTypeConverter : StringConverter
        ' В данном случае возвращается список строк из настроек программы. Затем нужно задать этот класс в качестве параметра атрибута TypeConverter для редактируемого свойства:
        Private mТипРегулировки As String = "Частота"
        ''' <summary>
        ''' Тип Регулировки
        ''' </summary>
        <DisplayName("Тип Регулировки")>
        <Description("Регулировка осуществляется по частоте или по амплитуде")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(OperationTypeConverterFrequencyOrAmplitude))>
        Public Property ТипРегулировки() As String
            Get
                Return mТипРегулировки
            End Get
            Set(ByVal value As String)
                mТипРегулировки = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЧастотаМин As Double = 0.1
        ''' <summary>
        ''' Минимальная частота
        ''' </summary>
        <DisplayName("Минимальная частота")>
        <Description("Минимальное значение частоты при фиксированной амплитуде")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(120)>
        <DynamicPropertyFilter("ТипРегулировки", "Частота")>
        Public Property ЧастотаМин() As Double
            Get
                Return mЧастотаМин
            End Get
            Set(ByVal value As Double)
                If value >= mЧастотаМакс Then
                    Const caption As String = "Проверка минимального значения частоты"
                    Const text As String = "Минимальное значение частоты должно быть меньше Максимального значения частоты!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mЧастотаМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЧастотаМакс As Double = 1000
        ''' <summary>
        ''' Максимальная частота
        ''' </summary>
        <DisplayName("Максимальная частота")>
        <Description("Максимальное значение частоты при фиксированной амплитуде")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(130)>
        <DynamicPropertyFilter("ТипРегулировки", "Частота")>
        Public Property ЧастотаМакс() As Double
            Get
                Return mЧастотаМакс
            End Get
            Set(ByVal value As Double)
                If value <= mЧастотаМин Then
                    Const caption As String = "Проверка максимального значения частоты"
                    Const text As String = "Максимальное значение частоты должно быть больше Минимального значения частоты!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mЧастотаМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеАмплитудыФикс As Double = 0.1
        ''' <summary>
        ''' Фиксированная амплитуда генератора
        ''' </summary>
        <DisplayName("Амплитуда")>
        <Description("Фиксированная амплитуда генератора сигнала (Вольт)")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(140)>
        <DynamicPropertyFilter("ТипРегулировки", "Частота")>
        Public Property ЗначениеАмплитудыФикс() As Double
            Get
                Return mЗначениеАмплитудыФикс
            End Get
            Set(ByVal value As Double)
                If value < 0.1 OrElse value > 10 Then
                    Const caption As String = "Проверка фиксированного значения амплитуды"
                    Const text As String = "Значение Амплитуды не может быть меньше 0.1 или больше 10 вольт!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Exit Property
                End If
                mЗначениеАмплитудыФикс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property


        Dim blnВызванИзЦапМакс As Boolean
        Dim blnВызванИзЦапМин As Boolean

        Private mЗначениеЦапМин As Double = -10
        ''' <summary>
        ''' Амплитуда Мининум
        ''' </summary>
        <DisplayName("Амплитуда Мининум")>
        <Description("Минимальное значение выходного сигнала генератора(Вольт)")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(150)>
        <DynamicPropertyFilter("ТипРегулировки", "Амплитуда")>
        Public Property ЗначениеЦапМин() As Double
            Get
                Return mЗначениеЦапМин
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value >= mЗначениеЦапМакс Then
                    Dim тextMessage As String = String.Empty
                    If value < -10 OrElse value > 10 Then
                        тextMessage = "Минимальное значение Амплитуды не может быть меньше -10 или больше 10 вольт!"
                    End If
                    If value >= mЗначениеЦапМакс Then
                        тextMessage = "Минимальное значение амплитуды должно быть меньше Максимального значения амплитуды!"
                    End If
                    Const caption As String = "Проверка минимального значения Амплитуды"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеЦапМин = value

                If blnВызванИзЦапМакс Then Exit Property
                blnВызванИзЦапМин = True
                ЗначениеЦапМакс = -value 'зеркально
                blnВызванИзЦапМин = False

                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеЦапМакс As Double = 10
        ''' <summary>
        ''' Амплитуда Максимум
        ''' </summary>
        <DisplayName("Амплитуда Максимум")>
        <Description("Максимальное значение выходного сигнала генератора(Вольт)")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(160)>
        <DynamicPropertyFilter("ТипРегулировки", "Амплитуда")>
        Public Property ЗначениеЦапМакс() As Double
            Get
                Return mЗначениеЦапМакс
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value <= mЗначениеЦапМин Then
                    Dim тextMessage As String = String.Empty
                    If value < -10 OrElse value > 10 Then
                        тextMessage = "Максимальное значение Амплитуды не может быть меньше -10 или больше 10 вольт!"
                    End If
                    If value <= mЗначениеЦапМин Then
                        тextMessage = "Максимальное значение амплитуды должно быть больше Минимального значения амплитуды!"
                    End If
                    Const caption As String = "Проверка максимального значения Амплитуды"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеЦапМакс = value

                If blnВызванИзЦапМин Then Exit Property
                blnВызванИзЦапМакс = True
                ЗначениеЦапМин = -value 'зеркально
                blnВызванИзЦапМакс = False

                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЧастотаФикс As Double = 0.1
        ''' <summary>
        ''' Фиксированная частота генератора
        ''' </summary>
        <DisplayName("Частота")>
        <Description("Фиксированная частота генератора сигнала (Герц)")>
        <Category("3. Тип Генератора")>
        <PropertyOrder(170)>
        <DynamicPropertyFilter("ТипРегулировки", "Амплитуда")>
        Public Property ЧастотаФикс() As Double
            Get
                Return mЧастотаФикс
            End Get
            Set(ByVal value As Double)
                If value < 0.1 Then
                    Const тextMessage As String = "Слишком низкая частота!"
                    Const caption As String = "Проверка значения частоты"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЧастотаФикс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private samplesPerBuffer As Integer = 10
        ''' <summary>
        ''' Точек Графика В Буфере
        ''' </summary>
        <DisplayName("Точки Графика")>
        <Description("Количество точек построения кривой выходного сигнала генератора")>
        <Category("4. Общие")>
        <PropertyOrder(100)>
        Public Property ТочекГрафикаВБуфере() As Integer
            Get
                Return samplesPerBuffer
            End Get
            Set(ByVal value As Integer)
                If value < 10 OrElse value > 1000 Then
                    Const тextMessage As String = "Значение точек построения не может быть меньше 10 или больше 1000 !"
                    Const caption As String = "Проверка точек в буфере графика"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                samplesPerBuffer = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private cyclesPerBuffer As Integer = 1
        ''' <summary>
        ''' Циклов графика в буфере
        ''' </summary>
        <DisplayName("Циклы в буфере")>
        <Description("Количество периодов изменения сигнала графика в выходном буфера")>
        <Category("4. Общие")>
        <PropertyOrder(110)>
        Public Property ЦикловВБуфере() As Integer
            Get
                Return cyclesPerBuffer
            End Get
            Set(ByVal value As Integer)
                If value < 1 OrElse value > 10 Then
                    Const тextMessage As String = "Значение Циклов в буфере не может быть меньше 1 или больше 10 !"
                    Const caption As String = "Проверка числа Циклов в буфере"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                cyclesPerBuffer = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mСоответствиеДиапазону As Double = 10
        ''' <summary>
        ''' Соответствие Диапазону
        ''' </summary>
        <DisplayName("Диапазон ЦАП")>
        <Description("Подстройка разрешение ЦАП под амплитуду сигнала (не обязательно)")>
        <Category("4. Общие")>
        <PropertyOrder(120)>
        <TypeConverter(GetType(OperationTypeConverterСоответствиеДиапазону))>
        Public Property СоответствиеДиапазону() As Double
            Get
                Return mСоответствиеДиапазону
            End Get
            Set(ByVal value As Double)
                mСоответствиеДиапазону = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ''' <summary>
        ''' physicalChannelName
        ''' </summary>
        <DisplayName("physicalChannelName")>
        <Description("Полное обозначение канала")>
        <Category("5. ChannelName")>
        <PropertyOrder(100)>
        Shadows ReadOnly Property ToString() As String 'Protected
            Get
                Return $"Dev{mНомерУстройства}/ao{mНомерКанала}"
            End Get
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "SlideProperties"
    <Serializable()>
    <DisplayName("Slide")>
    <Description("Название созданного Источника Напряжения")>
    Class SlideProperties
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolSlide
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        Private mНомерУстройства As Integer = 1
        ''' <summary>
        ''' Номер Устройства
        ''' </summary>
        <DisplayName("Номер Устройства")>
        <Description("Номер Устройства платы сбора сбора")>
        <Category("2. Железо")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(NumberDeviceTypeConverter))>
        Public Property НомерУстройства() As Integer
            Get
                Return mНомерУстройства
            End Get
            Set(ByVal value As Integer)
                mНомерУстройства = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mНомерКанала As Integer = 0
        ''' <summary>
        ''' Номер Канала
        ''' </summary>
        <DisplayName("Номер Канала")>
        <Description("Номер канала аналогового вывода ЦАП, который управляет внешним устройством")>
        <Category("2. Железо")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(NumberBitTypeConverter))>
        Public Property НомерКанала() As Integer
            Get
                Return mНомерКанала
            End Get
            Set(ByVal value As Integer)
                mНомерКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеЦапМин As Double = 0
        ''' <summary>
        ''' Цап Мининум
        ''' </summary>
        <DisplayName("Цап Мининум")>
        <Description("Минимальное значение выходного аналогового сигнала (Вольт), соответствующее величине Управление Мининум")>
        <Category("2. Железо")>
        <PropertyOrder(120)>
        Public Property ЗначениеЦапМин() As Double
            Get
                Return mЗначениеЦапМин
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value >= mЗначениеЦапМакс Then
                    Dim тextMessage As String = String.Empty
                    If value < -10 OrElse value > 10 Then
                        тextMessage = "Минимальное значение ЦАП не может быть меньше -10 или больше 10 вольт!"
                    End If
                    If value >= mЗначениеЦапМакс Then
                        тextMessage = "Минимальное значение ЦАП должно быть меньше Максимального значения ЦАП!"
                    End If
                    Const caption As String = "Проверка минимального значения ЦАП"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеЦапМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеЦапМакс As Double = 10
        ''' <summary>
        ''' Цап Максимум
        ''' </summary>
        <DisplayName("Цап Максимум")>
        <Description("Максимальное значение выходного аналогового сигнала (Вольт), соответствующее величине Управление Максимум")>
        <Category("2. Железо")>
        <PropertyOrder(130)>
        Public Property ЗначениеЦапМакс() As Double
            Get
                Return mЗначениеЦапМакс
            End Get
            Set(ByVal value As Double)
                If value < -10 OrElse value > 10 OrElse value <= mЗначениеЦапМин Then
                    Dim тextMessage As String = String.Empty
                    If value < -10 OrElse value > 10 Then
                        тextMessage = "Максимальное значение ЦАП не может быть меньше -10 или больше 10 вольт!"
                    End If
                    If value <= mЗначениеЦапМин Then
                        тextMessage = "Максимальное значение ЦАП должно быть больше Минимального значения ЦАП!"
                    End If
                    Const caption As String = "Проверка максимального значения ЦАП"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеЦапМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mУправлениеМин As Double = 0
        ''' <summary>
        ''' Управление Мининум
        ''' </summary>
        <DisplayName("Управление Мининум")>
        <Description("Минимальное значение Управления, соответствующее минимальному выходному напряжению ЦАП (Вольт)")>
        <Category("3. Управление")>
        <PropertyOrder(110)>
        Public Property УправлениеМин() As Double
            Get
                Return mУправлениеМин
            End Get
            Set(ByVal value As Double)
                If value >= mУправлениеМакс Then
                    Const тextMessage As String = "Минимальное значение должно быть меньше Максимального значения!"
                    Const caption As String = "Проверка минимального значения Управления"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mУправлениеМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mУправлениеМакс As Double = 10
        ''' <summary>
        ''' Управление Максимум
        ''' </summary>
        <DisplayName("Управление Максимум")>
        <Description("Максимальное значение Управления, соответствующее максимальному выходному напряжению ЦАП (Вольт)")>
        <Category("3. Управление")>
        <PropertyOrder(120)>
        Public Property УправлениеМакс() As Double
            Get
                Return mУправлениеМакс
            End Get
            Set(ByVal value As Double)
                If value <= mУправлениеМин Then
                    Const тextMessage As String = "Максимальное значение должно быть больше Минимальное значения!"
                    Const caption As String = "Проверка максимального значения Управления"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mУправлениеМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        ''' <summary>
        ''' physicalChannelName
        ''' </summary>
        <DisplayName("physicalChannelName")>
        <Description("Полное обозначение канала")>
        <Category("4. ChannelName")>
        <PropertyOrder(100)>
        Shadows ReadOnly Property ToString() As String 'Protected
            Get
                Return $"Dev{mНомерУстройства}/ao{mНомерКанала}"
            End Get
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "TankProperties"
    <Serializable()>
    <DisplayName("Tank")>
    <Description("Название созданного Индикатора")>
    Class TankProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate ' для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolTank
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property
        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого отображается в индикаторе")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "GaugeProperties"
    <Serializable()>
    <DisplayName("Gauge")>
    <Description("Название созданного Индикатора")>
    Class GaugeProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolGauge
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого отображается в индикаторе")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "ThermometerProperties"
    <Serializable()>
    <DisplayName("Thermometer")>
    <Description("Название созданного Индикатора")>
    Class ThermometerProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolThermometer
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого отображается в индикаторе")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "MeterProperties"
    <Serializable()>
    <DisplayName("Meter")>
    <Description("Название созданного Индикатора")>
    Class MeterProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolMeter
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        '    Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого отображается в индикаторе")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "ScatterGraphProperties"
    <Serializable()>
    <DisplayName("ScatterGraph")>
    <Description("Название созданного Графика по параметру")>
    Class ScatterGraphProperties
        Inherits PropertiesControlBase
        Implements ISerializationSurrogate 'для сереализации


        '<NonSerialized()> _
        Private arrDataX As Queue '(Of Double)
        '<NonSerialized()> _
        Private arrDataY As Queue '(Of Double)

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolScatterGraph

            'arrDataX = New Queue(Of Double)(intДинамика)
            'arrDataY = New Queue(Of Double)(intДинамика)
            arrDataX = New Queue(Dynamics)
            arrDataY = New Queue(Dynamics)
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property
        <Browsable(False)>
        Public Property ИндексКаналаОсиХВМассивеПараметров() As Integer
        <Browsable(False)>
        Public Property ИндексКаналаОсиУВМассивеПараметров() As Integer

        'Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКаналаОсиХ As String = MissingParameter ' New IPAddress("192.168.1.1")
        ''' <summary>
        ''' Имя Канала X
        ''' </summary>
        <DisplayName("Имя Канала X")>
        <Description("Имя Канала для оси X, значение которого используется при построении графика")>
        <Category("2. Ось X")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Property ИмяКаналаОсиХ() As String
            Get
                Return mИмяКаналаОсиХ
            End Get
            Set(ByVal value As String)
                mИмяКаналаОсиХ = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиХМин As Double = 0
        ''' <summary>
        ''' X Мининум
        ''' </summary>
        <DisplayName("X Мининум")>
        <Description("Минимальное значение оси Х")>
        <Category("2. Ось X")>
        <PropertyOrder(110)>
        Public Property ЗначениеОсиХМин() As Double
            Get
                Return mЗначениеОсиХМин
            End Get
            Set(ByVal value As Double)
                If value >= mЗначениеОсиХМакс Then
                    Const тextMessage As String = "Минимальное значение должно быть меньше Максимального значения!"
                    Const caption As String = "Проверка минимального значения оси Х"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиХМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиХМакс As Double = 100
        ''' <summary>
        ''' X Максимум
        ''' </summary>
        <DisplayName("X Максимум")>
        <Description("Максимальное значение оси Х")>
        <Category("2. Ось X")>
        <PropertyOrder(120)>
        Public Property ЗначениеОсиХМакс() As Double
            Get
                Return mЗначениеОсиХМакс
            End Get
            Set(ByVal value As Double)
                If value <= mЗначениеОсиХМин Then
                    Const тextMessage As String = "Максимальное значение должно быть больше Минимальное значения!"
                    Const caption As String = "Проверка максимального значения оси Х"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиХМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mИмяКаналаОсиУ As String = MissingParameter
        ''' <summary>
        ''' Имя Канала Y
        ''' </summary>
        <DisplayName("Имя Канала Y")>
        <Description("Имя Канала для оси Y, значение которого используется при построении графика")>
        <Category("3. Ось Y")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Property ИмяКаналаОсиУ() As String
            Get
                Return mИмяКаналаОсиУ
            End Get
            Set(ByVal value As String)
                mИмяКаналаОсиУ = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиYМин As Double = 0
        ''' <summary>
        ''' Y Мининум
        ''' </summary>
        <DisplayName("Y Мининум")>
        <Description("Минимальное значение оси Y")>
        <Category("3. Ось Y")>
        <PropertyOrder(110)>
        Public Property ЗначениеОсиYМин() As Double
            Get
                Return mЗначениеОсиYМин
            End Get
            Set(ByVal value As Double)
                If value >= mЗначениеОсиYМакс Then
                    Const тextMessage As String = "Минимальное значение должно быть меньше Максимального значения!"
                    Const caption As String = "Проверка минимального значения оси Y"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиYМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиYМакс As Double = 100
        ''' <summary>
        ''' Y Максимум
        ''' </summary>
        <DisplayName("Y Максимум")>
        <Description("Максимальное значение оси Y")>
        <Category("3. Ось Y")>
        <PropertyOrder(120)>
        Public Property ЗначениеОсиYМакс() As Double
            Get
                Return mЗначениеОсиYМакс
            End Get
            Set(ByVal value As Double)
                If value <= mЗначениеОсиYМин Then
                    Const тextMessage As String = "Максимальное значение должно быть больше Минимальное значения!"
                    Const caption As String = "Проверка максимального значения оси Y"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиYМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        '<NonSerialized()> _
        'Private arrDataX As New Queue(Of Double)(intДинамика)
        '<NonSerialized()> _
        'Private arrDataY As New Queue(Of Double)(intДинамика)

        Public Sub РегистрацияГрафики(ByVal X As Double, ByVal Y As Double)
            If arrDataX.Count = Dynamics Then
                arrDataX.Dequeue()
                arrDataY.Dequeue()
            End If
            arrDataX.Enqueue(X)
            arrDataY.Enqueue(Y)
            'ScatterPlot1.PlotXY(arrDataX.ToArray, arrDataY.ToArray)
        End Sub

        <Browsable(False)>
        Public ReadOnly Property GetDataX() As Double()
            Get
                'Return arrDataX.ToArray
                Return arrDataX.Cast(Of Double).ToArray
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property GetDataY() As Double()
            Get
                'Return arrDataY.ToArray
                Return arrDataY.Cast(Of Double).ToArray
            End Get
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

#Region "WaveformGraphProperties"
    <Serializable()>
    <DisplayName("WaveformGraph")>
    <Description("Название созданного Графика по времени")>
    Class WaveformGraphProperties
        Inherits PropertiesControlChannelBase
        Implements ISerializationSurrogate 'для сереализации

        Public Sub New(ByVal name As String)
            MyBase.New(name)
            Тип = EnumControl.wcontrolWaveformGraph
        End Sub

        <DisplayName("Название")>
        <Description("Название подуровня")>
        <Category("1. Имя")>
        Public Overloads Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal value As String)
                MyBase.Name = value
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property ИндексВМассивеПараметров() As Integer

        'Как организовать редактирование свойства в собственной форме?
        'Необходимо реализовать наследника UITypeEditor, обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
        'передачу ей редактируемого значения и получение результата:
        'смотри public class IPAddressEditor : UITypeEditor 
        'а затем привязать его к редактируемому свойству при помощи атрибута Editor:
        Private mИмяКанала As String = MissingParameter '
        ''' <summary>
        ''' ИмяКанала
        ''' </summary>
        <DisplayName("Имя Канала")>
        <Description("Имя Канала, значение которого используется при построении графика")>
        <Category("2. Канал")>
        <PropertyOrder(100)>
        <Editor(GetType(ChannelAndFormulaEditor), GetType(UITypeEditor))>
        Public Overrides Property ИмяКанала() As String
            Get
                Return mИмяКанала
            End Get
            Set(ByVal value As String)
                mИмяКанала = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиYМин As Double = 0
        ''' <summary>
        ''' Y Мининум
        ''' </summary>
        <DisplayName("Y Мининум")>
        <Description("Минимальное значение оси Y")>
        <Category("3. Ось Y")>
        <PropertyOrder(110)>
        Public Property ЗначениеОсиYМин() As Double
            Get
                Return mЗначениеОсиYМин
            End Get
            Set(ByVal value As Double)
                If value >= mЗначениеОсиYМакс Then
                    Const тextMessage As String = "Минимальное значение должно быть меньше Максимального значения!"
                    Const caption As String = "Проверка минимального значения оси Y"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиYМин = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mЗначениеОсиYМакс As Double = 100
        ''' <summary>
        ''' Y Максимум
        ''' </summary>
        <DisplayName("Y Максимум")>
        <Description("Максимальное значение оси Y")>
        <Category("3. Ось Y")>
        <PropertyOrder(120)>
        Public Property ЗначениеОсиYМакс() As Double
            Get
                Return mЗначениеОсиYМакс
            End Get
            Set(ByVal value As Double)
                If value <= mЗначениеОсиYМин Then
                    Const тextMessage As String = "Максимальное значение должно быть больше Минимальное значения!"
                    Const caption As String = "Проверка максимального значения оси Y"
                    MessageBox.Show(тextMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {тextMessage}")
                    Exit Property
                End If
                mЗначениеОсиYМакс = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property


        'Как заменить стандартные True/False в отображении свойств типа bool?
        'Используйте атрибут TypeConverter:
        Private mТипГрафика As Boolean = True
        ''' <summary>
        ''' Тип графика
        ''' </summary>
        <DisplayName("Тип графика")>
        <Description("Выбор типа рисования шлейфа графика")>
        <Category("4. Тип графика")>
        <PropertyOrder(100)>
        <TypeConverter(GetType(ТипГрафикаBooleanTypeConverter))>
        Public Property ТипГрафика() As Boolean
            Get
                Return mТипГрафика
            End Get
            Set(ByVal value As Boolean)
                mТипГрафика = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Private mПрокрутка As Boolean = True
        ''' <summary>
        ''' Тип прокрутки
        ''' </summary>
        <DisplayName("Тип прокрутки")>
        <Description("Выбор типа прокрутки шлейфа графика")>
        <Category("4. Тип графика")>
        <PropertyOrder(110)>
        <TypeConverter(GetType(ПрокруткаBooleanTypeConverter))>
        Public Property Прокрутка() As Boolean
            Get
                Return mПрокрутка
            End Get
            Set(ByVal value As Boolean)
                mПрокрутка = value
                RefreshContextVisitor.Update(Me)
            End Set
        End Property

        Public Overloads Overrides Sub AcceptVisitorRefresh(Of C)(ByVal context As C, ByVal visitor As IRefreshContextVisitor(Of C))
            visitor.UpdateTag(context, Me)
        End Sub
    End Class
#End Region

End Module
