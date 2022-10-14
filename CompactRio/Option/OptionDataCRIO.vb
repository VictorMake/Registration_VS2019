Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Timers
Imports System.Xml.Linq

''' <summary>
''' Данные для редактирования настроек с шасси CompactRio в PropertyGrid.
''' Класс управляемый видимостью своих свойств в зависимости от значения другого свойства.
''' Стандартный атрибут Browsable позволяет задавать видимость свойства в PropertyGrid только на этапе написания кода.
''' Чтобы управлять видимостью свойства в зависимости от значения другого свойства настраиваемого объекта, 
''' понадобятся новый атрибут – DynamicPropertyFilter и базовый класс – FilterablePropertyBase:
''' </summary>
<TypeConverter(GetType(PropertySorter))>
Friend Class OptionDataCRIO
    Inherits FilterablePropertyBase ' Указываем базовый класс для класса настраиваемого объекта.
    ' Задаем атрибут TypeConverter с параметром PropertySorter для всего класса с настраиваемыми свойствами.
    Private Shared aTimer As System.Timers.Timer

    Public Sub New()
        ' Код содержит объявления переменной таймера на уровне класса и внутри Main. 
        ' Чтобы увидеть, как агрессивная сборка мусора может повлиять на таймер, 
        ' объявленный внутри длительно выполняемого метода, можно закомментировать декларацию уровня класса 
        ' и раскомментировать локальную переменную. 
        ' Чтобы таймер не был удален при сборке мусора, раскомментировать метод GC.KeepAlive в конце процедуры.
        'Dim aTimer As System.Timers.Timer

        ' Создать таймер с секундным интервалом.
        aTimer = New Timer(1000)
        ' добавить обработчик события Elapsed для таймера.
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent

        ' Для установки интервала в 2 секунды (2000 milliseconds).
        'aTimer.Interval = 2000
        aTimer.AutoReset = False

        ' Если таймер декларирован длительно выполняющимся методом
        ' использовать KeepAlive для предупреждения сборщика мусора от вызова
        ' прежде чем метод завершится.
        'GC.KeepAlive(aTimer)
    End Sub

    ''' <summary>
    ''' Обработчик события
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OnTimedEvent(ByVal source As Object, ByVal e As ElapsedEventArgs)
        'Console.WriteLine("Событие таймера вызвано в {0}", e.SignalTime)
        aTimer.Enabled = False
        mIsWritindPath = False
    End Sub

    '   Чтобы заменить имя переменной в левой колонке “человеческим” именем свойства: для этого предназначен атрибут [DisplayName("Тип работы")].
    '   Чтобы отобразить расширенную подсказку по свойству в нижнем окне: для этого предназначен атрибут [Description("")].
    '   Чтобы сгруппировать свойства по категориям:  использовать атрибут [Category("1.***")].
    '   Чтобы отобразить свойство, недоступное для редактирования: для этого можно либо само свойство сделать read-only (оставив только Get), либо использовать атрибут ReadOnly.

    '--- 1. Общие -------------------------------------------------------------
#Region "1. Общие"
    ' сделать от этого свойства видимость других

    ''' <summary>
    ''' Запуск.
    ''' </summary>
    <DisplayName("Тип работы")>
    <Description("С контроллером: получение значений измерений от шасси и передача их далее Серверу" & vbCrLf & "Автономно: иммитация измерений от шасси и передача их далее Серверу")>
    <Category("1. Общие")>
    <PropertyOrder(10)>
    <TypeConverter(GetType(ТипРаботыBooleanTypeConverter))>
    Public Property СКонтроллером() As Boolean = True
#End Region

    '--- 2. Частота сбора -----------------------------------------------------
#Region "2. Частота сбора"
    Private mFrequency As String = "100"

    ''' <summary>
    ''' ЧастотаСбора.
    ''' </summary>
    <DisplayName("Частота передачи данных в Registration (Гц)")>
    <Description("Частота передачи данных в Registration с учетом осреднения при передискретизации (Гц)")>
    <Category("2. Частота передачи")>
    <PropertyOrder(100)>
    <TypeConverter(GetType(ЧастотаСбораTypeConverter))>
    <DynamicPropertyFilter("СКонтроллером", "True")>
    Public Property Frequency() As String
        Get
            Return mFrequency
        End Get
        Set(ByVal value As String)
            mFrequency = value
        End Set
    End Property
#End Region

    '    Чтобы организовать выбор значения из выпадающего списка, формируемого программно, необходимо реализовать [TypeConverter(TypeOf(***TypeConverter))],
    '    предоставляющий список, из которого можно будет делать выбор: 
    ' смотри class StendServerTypeConverter  StringConverter
    ' В данном случае возвращается список строк из настроек программы. Затем нужно задать этот класс в качестве параметра атрибута TypeConverter для редактируемого свойства

    '--- 3. Стенд сервера -----------------------------------------------------
#Region "3. Стенд сервера"
    Private mStendServer As String = "1"

    ''' <summary>
    ''' Стенд сервера.
    ''' </summary>
    <DisplayName("Стенд сервера")>
    <Description("Стенд сервера принимающий данные")>
    <Category("3. Дополнительно")>
    <PropertyOrder(180)>
    <TypeConverter(GetType(StendServerTypeConverter))>
    <DynamicPropertyFilter("СКонтроллером", "True")>
    Public Property StendServer() As String
        Get
            Return mStendServer
        End Get
        Set(ByVal value As String)
            mStendServer = value
        End Set
    End Property
#End Region

    '--- 4. Дополнительно -----------------------------------------------------
#Region "4. Дополнительно"
    '    ??? организовать выбор файла с заданным расширением?
    ' Задайте атрибут Editor:
    ' Собственно фильтр расширений задается в DocFileEditor:

    ' А для свойства, видимость которого зависит от другого свойства – атрибут DynamicPropertyFilter:
    ' здесь "StendServer" атрибут от которого зависит видимость со списком, где видимость включается
    ' Также нужно добавить обработчик события PropertyGrid – PropertyValueChanged:

    Private mPathStendServer As String = "Неправильный путь"

    ' TODO: "StendServer", "11,13,15"
    ''' <summary>
    ''' Путь к базе каналов сервера.
    ''' </summary>
    <DisplayName("Путь к MDB файлу базы данных каналов")>
    <Description("Путь к базе данных каналов")>
    <Category("4. Дополнительно")>
    <PropertyOrder(190)>
    <Editor(GetType(ChannelsFileEditor), GetType(UITypeEditor))>
    <DynamicPropertyFilter("StendServer", "11,13,15")>
    Public Property PathDataBaseStendServer() As String
        Get
            Return mPathStendServer
        End Get
        Set(ByVal value As String)
            If Not mIsReadingPath Then
                ' свойство надо записать сразу, затем идет обновление в событии propertyGrid1_PropertyValueChanged
                mIsWritindPath = True
                aTimer.Enabled = True ' запуск таймера для сброса ИдетЗаписьПути_ = False

                Dim cReadWriteIni As New ReadWriteIni(PathOptions_xml)
                Dim xmlDoc As XElement = XElement.Load(PathOptions_xml)
                With cReadWriteIni
                    ' в зависимости от данного параметра меняется путь
                    .WriteINI(xmlDoc, "MobileComplex", "Дополнительно", Path_Server_Data_Base & mStendServer, value)
                End With
            End If
            mPathStendServer = value
        End Set
    End Property

    ' перекрестные свойства, чтобы изменение пути не вызывало повторную запись
    ' ИдетЗаписьПути_ = False устанавливается по таймеру, который срабатывает, а затем отключается
    Private mIsWritindPath As Boolean

    <Browsable(False)>
    Public Property ИдетЗаписьПути() As Boolean
        Get
            Return mIsWritindPath
        End Get
        Set(ByVal value As Boolean)
            mIsWritindPath = value
        End Set
    End Property

    Private mIsReadingPath As Boolean

    ''' <summary>
    ''' Флаг для запрета считывания файла конфигурации при программном переопределении.
    ''' При изменении из сетки свойств будет происходить считывание нового файла конфигурации, проверка и перенастройка
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property ИдетСчитываниеПути() As Boolean
        Get
            Return mIsReadingPath
        End Get
        Set(ByVal value As Boolean)
            mIsReadingPath = value
        End Set
    End Property

    '    ??? избавиться от стандартного “(Collection)” в правой колонке для свойств-коллекций?
    ' использовать атрибут TypeConverter:
    ' При переходе к редактированию коллекции отобразится стандартное окно Collection Editor с данными редактируемой коллекции:
    Private mCollectionTargetCRIO As New List(Of TargetCRIO)()

    ''' <summary>
    ''' Список ограничений.
    ''' </summary>
    <DisplayName("Шасси")>
    <Description("Список шасси cRio в ИВК")>
    <Category("4. Дополнительно")>
    <PropertyOrder(200)>
    <TypeConverter(GetType(CollectionTypeConverter))>
    <Editor(GetType(ChassisCollectionEditor), GetType(UITypeEditor))>
    <DynamicPropertyFilter("СКонтроллером", "True")>
    Public Property CollectionTargetCRIO() As List(Of TargetCRIO)
        Get
            Return mCollectionTargetCRIO
        End Get
        Set(ByVal value As List(Of TargetCRIO))
            mCollectionTargetCRIO = value
        End Set
    End Property

    '    Чтобы организовать редактирование свойства в собственной форме: необходимо реализовать наследника UITypeEditor,
    '    обеспечивающего вызов нужной формы (в данном случае IPAddressEditorForm), 
    ' передачу ей редактируемого значения и получение результата:
    ' смотри public class IPAddressEditor : UITypeEditor 
    ' а затем привязать его к редактируемому свойству при помощи атрибута Editor:
    Private mIPaddress As New IPAddressTargetCRIO("192.168.1.1")

    ''' <summary>
    ''' IP адрес компьютера рабочего места.
    ''' </summary>
    <DisplayName("IP адрес ИВК")>
    <Description("IP адрес компьютера с программой управляющего ИВК")>
    <Category("4. Дополнительно")>
    <PropertyOrder(210)>
    <Editor(GetType(IPAddressEditor), GetType(UITypeEditor))>
    <DynamicPropertyFilter("СКонтроллером", "True")>
    Public Property IPaddressProperty() As IPAddressTargetCRIO
        Get
            Return mIPaddress
        End Get
        Set(ByVal value As IPAddressTargetCRIO)
            mIPaddress = value
        End Set
    End Property

    Private mPort As String = "5555"

    ''' <summary>
    ''' Номер порта компьютера рабочего места.
    ''' </summary>
    <DisplayName("Порт ИВК")>
    <Description("Номер порта состоит из цифр  между 1 и 65535")>
    <Category("4. Дополнительно")>
    <PropertyOrder(220)>
    <DynamicPropertyFilter("СКонтроллером", "True")>
    Public Property Порт() As String
        Get
            Return mPort
        End Get
        Set(ByVal value As String)
            Dim deltaPort As Integer
            If Not Integer.TryParse(value, deltaPort) OrElse deltaPort < 1 OrElse deltaPort > 65535 Then
                MessageBox.Show("Порт обязан быть числом между 1 и 65535.", "Неправильный номер порта", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                mPort = value
            End If
        End Set
    End Property
#End Region
End Class