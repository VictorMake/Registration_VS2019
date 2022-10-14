Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection ' для сереализации
Imports System.Runtime.Serialization ' для сереализации

''' <summary>
''' Данные для идентификации целевого устройства compactRio
''' при сериализации и десериализации его как колекции представленной ArrayList.
''' </summary>
<Serializable()>
Friend Class TargetCRIO
    Implements ISerializationSurrogate

    ''' <summary>
    ''' Конструктор по умолчанию.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.New("New Target", New IPAddressTargetCRIO("192.168.1.1"), EnumModeWork.Measurement, "Папка не указана")
    End Sub

    ''' <summary>
    ''' Конструктор с определёнными параметрами.
    ''' </summary>
    ''' <param name="hostName"></param>
    ''' <param name="iPAddressRTtarget"></param>
    ''' <param name="modeWork"></param>
    ''' <param name="folderName"></param>
    Public Sub New(hostName As String, iPAddressRTtarget As IPAddressTargetCRIO, modeWork As EnumModeWork, folderName As String)
        mHostName = hostName
        mIPAddressRTtarget = iPAddressRTtarget
        mModeWork = modeWork
        mFolderName = folderName
    End Sub

    ''' <summary>
    ''' Заполняет предоставленный System.Runtime.Serialization.SerializationInfo данными,
    ''' необходимыми для сериализации объекта.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="info"></param>
    ''' <param name="context"></param>
    Public Sub GetObjectData(ByVal obj As Object,
                             ByVal info As SerializationInfo,
                             ByVal context As StreamingContext) Implements ISerializationSurrogate.GetObjectData

        Dim flags As BindingFlags = BindingFlags.Instance Or
                                    BindingFlags.Public Or
                                    BindingFlags.Public

        ' на типе для каждого открытого поля считать значение
        For Each fi As FieldInfo In obj.GetType().GetFields(flags)
            info.AddValue(fi.Name, fi.GetValue(obj))
        Next
    End Sub

    ''' <summary>
    ''' Заполняет объект с помощью сведений в System.Runtime.Serialization.SerializationInfo.
    ''' Возврат: Заполняет десериализованный объект.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="info"></param>
    ''' <param name="context"></param>
    ''' <param name="selector"></param>
    ''' <returns></returns>
    Public Function SetObjectData(ByVal obj As Object,
                                  ByVal info As SerializationInfo,
                                  ByVal context As StreamingContext,
                                  ByVal selector As ISurrogateSelector) As Object Implements ISerializationSurrogate.SetObjectData

        Dim flags As BindingFlags = BindingFlags.Instance Or
                                    BindingFlags.Public Or
                                    BindingFlags.Public

        ' на типе для каждого открытого поля установить значение
        For Each fi As FieldInfo In obj.GetType().GetFields(flags)
            fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType))
        Next

        Return obj
    End Function

    ''' <summary>
    ''' Для связи со строкой таблицы в основной форме.
    ''' </summary>
    ''' <remarks></remarks>
    <NonSerialized()> Public IndexRow As Integer

    Private mHostName As String

    ''' <summary>
    ''' Hostname (Имя шасси cRio).
    ''' </summary>
    <DisplayName("Hostname")>
    <Description("Имя шасси cRio")>
    <Category("1. Target")>
    <PropertyOrder(10)>
    Public Property HostName() As String
        Get
            Return mHostName
        End Get
        Set(ByVal value As String)
            mHostName = value
        End Set
    End Property

    Private mIPAddressRTtarget As New IPAddressTargetCRIO("192.168.1.1")

    ''' <summary>
    ''' IP адрес  шасси cRio.
    ''' </summary>
    <DisplayName("IP адрес")>
    <Description("IP адрес шасси cRio")>
    <Category("1. Target")>
    <PropertyOrder(20)>
    <Editor(GetType(IPAddressEditor), GetType(UITypeEditor))>
    Public Property IPAddressRTtarget() As IPAddressTargetCRIO
        Get
            Return mIPAddressRTtarget
        End Get
        Set(ByVal value As IPAddressTargetCRIO)
            mIPAddressRTtarget = value
        End Set
    End Property

    Private mModeWork As EnumModeWork = EnumModeWork.Measurement

    ''' <summary>
    ''' Тип работы шасси.
    ''' </summary>
    <DisplayName("Тип работы шасси")>
    <Description("Тип работы шасси сбор или управление")>
    <TypeConverter(GetType(EnumTypeConverter))>
    <Category("1. Target")>
    <PropertyOrder(30)>
    <Editor(GetType(EnumModeWorkEditor), GetType(UITypeEditor))>
    Public Property ModeWork() As EnumModeWork
        Get
            Return mModeWork
        End Get
        Set(ByVal value As EnumModeWork)
            mModeWork = value
        End Set
    End Property

    Private mFolderName As String = "Папка не указана"

    ''' <summary>
    ''' Путь к каталогу с файлами.
    ''' </summary>
    <DisplayName("Каталог с файлами")>
    <Description("Каталог с исполняемыми файлами для копирования на целевое устройство")>
    <Category("2. Исполняемые файлы cRio")>
    <PropertyOrder(40)>
    <Editor(GetType(CatalogEditor), GetType(UITypeEditor))>
    Public Property FolderName() As String
        Get
            Return mFolderName
        End Get
        Set(ByVal value As String)
            mFolderName = value
        End Set
    End Property

    ''' <summary>
    ''' mCopyFolder
    ''' </summary>
    Private _CopyFolder As Boolean = True

    ''' <summary>
    ''' Копировать файлы?
    ''' </summary>
    <DisplayName("Копировать файлы?")>
    <Description("Если копирование исполняемых файлов было произведено хотя бы раз, можно отключить для ускорения загрузки.")>
    <Category("2. Исполняемые файлы cRio")>
    <PropertyOrder(50)>
    <TypeConverter(GetType(BooleanTypeConverter2))>
    Public Property CopyFolder() As Boolean
        Get
            Return _CopyFolder
        End Get
        Set(ByVal value As Boolean)
            _CopyFolder = value
        End Set
    End Property

    ''' <summary>
    ''' Представление в виде строки, используется для показа списка вариантов 
    ''' при редактировании в PropertyGrid.
    ''' </summary>
    Public Overloads Overrides Function ToString() As String
        Return mHostName
    End Function

    ''' <summary>
    '''  Предоставить неполную копию текущего объекта и снять флаг копирования файлов проекта на Шасси
    '''  для исключения многократных копирований в случае забытия его снятия.
    ''' </summary>
    ''' <returns></returns>
    Public Function DeepCopy() As TargetCRIO
        Dim other As TargetCRIO = CType(Me.MemberwiseClone(), TargetCRIO)
        other.CopyFolder = False
        Return other
    End Function
End Class
