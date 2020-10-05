Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Xml

''' <summary>
''' Унаследован от BasicDesignerLoader. Он продолжает HostSurface
''' для Xml файла и может также разобирать Xml файл воссоздав RootComponent
''' и все компоненты, что он хостит.
''' </summary>
Public Class BasicHostLoader
    Inherits BasicDesignerLoader ' представляет реализацию System.ComponentModel.Design.Serialization.IDesignerLoaderService интерфейса.

    Private root As IComponent
    Private dirty As Boolean = True
    Private unsaved As Boolean
    Private fileName As String
    Private host As IDesignerLoaderHost
    Private xmlDocument As XmlDocument
    Private Shared ReadOnly propertyAttributes As Attribute() = New Attribute() {DesignOnlyAttribute.No}
    Private rootComponentType As Type

#Region "Constructors"

    ''' Пустой конструктор просто создает форму.
    Public Sub New(ByVal rootComponentType As Type)
        Me.rootComponentType = rootComponentType
        Modified = True
    End Sub

    ''' <summary>
    ''' Этот конструктор берёт имя файла. 
    ''' Этот файл должен существовать на диске и содержит XML,
    ''' и может быть прочитан как набор данных.
    ''' </summary>
    Public Sub New(ByVal fileName As String)
        If fileName Is Nothing Then
            Throw New ArgumentNullException("fileName")
        End If

        Me.fileName = fileName
    End Sub
#End Region

#Region "переопределяет метод BasicDesignerLoader"

    ' Вызов хоста, когда загружается документ.
    Protected Overloads Overrides Sub PerformLoad(ByVal designerSerializationManager As IDesignerSerializationManager)
        host = LoaderHost

        If host Is Nothing Then
            Throw New ArgumentNullException("BasicHostLoader.BeginLoad: Invalid designerLoaderHost.")
        End If

        ' Загрузчик будет бросать сообщения об ошибках здесь.
        Dim errors As New ArrayList()
        Dim successful As Boolean = True
        Dim baseClassName As String

        ' Если ни одно имя файла не было найдено, просто создать форму и делать без этого. Если файл имя было найдено, прочесть его.
        If fileName Is Nothing Then
            If rootComponentType Is GetType(Form) Then
                host.CreateComponent(GetType(Form))
                baseClassName = "Form1"
                'ElseIf rootComponentType Is GetType(UserControl) Then
                '    host.CreateComponent(GetType(UserControl))
                '    baseClassName = "UserControl1"
                'ElseIf rootComponentType Is GetType(Component) Then
                '    host.CreateComponent(GetType(Component))
                '    baseClassName = "Component1"
            Else
                Throw New Exception("Неопределённый тип хоста: " & rootComponentType.ToString())
            End If
        Else
            baseClassName = ReadFile(fileName, errors, xmlDocument)
        End If

        ' После корректной загрузки, необходимо начать слушать события.
        ' События слушателя извещают, есть сейчас дизайнер "Loader" может также быть использован для записи данных.
        ' Если необходимо интегрировать загрузчик с контролем источника кода
        ' необходимо слушать "ing" сообщения также, как и "ed" сообщения.
        Dim cs As IComponentChangeService = TryCast(host.GetService(GetType(IComponentChangeService)), IComponentChangeService)

        If cs IsNot Nothing Then
            AddHandler cs.ComponentChanged, New ComponentChangedEventHandler(AddressOf OnComponentChanged)
            AddHandler cs.ComponentAdded, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
            AddHandler cs.ComponentRemoved, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
        End If

        ' Позволить хосту знать, что загрузка сделана
        host.EndLoad(baseClassName, successful, errors) ' до этого был вызов hostSurface.BeginLoad(basicHostLoader)

        ' Документ просто загружен, таким образом мы нуждаемся в записи изменений
        dirty = True
        unsaved = False
    End Sub

    ''' <summary>
    ''' Этот метод вызывается дезайнером хоста всякий раз когда он хочет чтобы
    ''' загрузчик дизайнера заполнил неоконченные изменения. Поступление изменений
    ''' не означает то же самое, что сохранение на диске. Для примера в 
    ''' Visual Studio, поступление изменений следствие, что новоый код сгенерирован
    ''' и вставлен в окно текстового редактора. Пользователь может редактировать
    ''' новый код в редакторе окна, но ничего не будет сохранено на диске.
    ''' Этот разделяет поступление и сохранение для определения пунктов меню сохранения.
    ''' </summary>
    Protected Overloads Overrides Sub PerformFlush(ByVal designerSerializationManager As IDesignerSerializationManager)
        ' Ничего не произошло, если нет изменений
        If Not dirty Then
            Return
        End If

        PerformFlushWorker()
    End Sub
    Public Overloads Overrides Sub Dispose()
        ' Всегда удалять обработчики событий в методе Dispose.
        Dim cs As IComponentChangeService = TryCast(host.GetService(GetType(IComponentChangeService)), IComponentChangeService)

        If cs IsNot Nothing Then
            RemoveHandler cs.ComponentChanged, New ComponentChangedEventHandler(AddressOf OnComponentChanged)
            RemoveHandler cs.ComponentAdded, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
            RemoveHandler cs.ComponentRemoved, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
        End If
    End Sub

#End Region

#Region "Helper methods"

    ''' <summary>
    ''' Простой вспомогательный метод возвращает true, если данный тип конвертера поддерживает
    ''' двухстороннюю конверсию данного типа.
    ''' </summary>
    Private Function GetConversionSupported(ByVal converter As TypeConverter, ByVal conversionType As Type) As Boolean
        Return (converter.CanConvertFrom(conversionType) AndAlso converter.CanConvertTo(conversionType))
    End Function

    ''' <summary>
    ''' Как только что-то в дизайнере изменились - изменения пометить True, таким образом поток будет давать новый
    ''' xmlDocument и codeCompileUnit.
    ''' </summary>
    Private Sub OnComponentChanged(ByVal sender As Object, ByVal ce As ComponentChangedEventArgs)
        dirty = True
        unsaved = True
    End Sub

    ''' <summary>
    ''' Как только что-то в дизайнере изменились - изменения пометить True, таким образом поток будет давать новый
    ''' xmlDocument и codeCompileUnit.
    ''' </summary>
    Private Sub OnComponentAddedRemoved(ByVal sender As Object, ByVal ce As ComponentEventArgs)
        dirty = True
        unsaved = True
    End Sub

    ''' <summary>
    ''' Этот метод выводит диалог пользователю на сохранение изменений.
    ''' Напоминание случится, только, если пользователь сделал изменения.
    ''' </summary>
    Friend Function PromptDispose() As Boolean
        If dirty OrElse unsaved Then
            Const caption As String = "Сохранение изменений"
            Const text As String = "Сохранить изменения в существующем дизайнере?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{caption}> {text}")

            Select Case MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel)
                Case DialogResult.Yes
                    Save(False)
                    Exit Select

                Case DialogResult.Cancel
                    Return False
            End Select
        End If

        Return True
    End Function

#End Region

#Region "Serialize - Flush"
    ''' <summary>
    ''' Это будет рекурсивно идти сквозь все объекты в дереве и серилизовать их в Xml
    ''' </summary>
    Public Sub PerformFlushWorker()
        Dim document As New XmlDocument()
        document.AppendChild(document.CreateElement("DOCUMENT_ELEMENT"))

        Dim idh As IDesignerHost = DirectCast(host.GetService(GetType(IDesignerHost)), IDesignerHost)
        root = idh.RootComponent

        Dim nametable As New Hashtable(idh.Container.Components.Count)
        ' убрал*** Dim manager As IDesignerSerializationManager = TryCast(host.GetService(GetType(IDesignerSerializationManager)), IDesignerSerializationManager)

        document.DocumentElement.AppendChild(WriteObject(document, nametable, root))
        For Each comp As IComponent In idh.Container.Components
            If comp IsNot root AndAlso Not nametable.ContainsKey(comp) Then
                document.DocumentElement.AppendChild(WriteObject(document, nametable, comp))
            End If
        Next

        xmlDocument = document
    End Sub

    ''' <summary>
    ''' Рекурсия по узлам Object
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="nametable"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Function WriteObject(ByVal document As XmlDocument, ByVal nametable As IDictionary, ByVal value As Object) As XmlNode
        Dim idh As IDesignerHost = DirectCast(host.GetService(GetType(IDesignerHost)), IDesignerHost)
        Debug.Assert(value IsNot Nothing, "Не возможно вызвать WriteObject с нулевым значением")

        Dim node As XmlNode = document.CreateElement("Object")
        Dim typeAttr As XmlAttribute = document.CreateAttribute("type")

        typeAttr.Value = value.[GetType]().AssemblyQualifiedName
        node.Attributes.Append(typeAttr)

        Dim component As IComponent = TryCast(value, IComponent)

        If component IsNot Nothing AndAlso component.Site IsNot Nothing AndAlso component.Site.Name IsNot Nothing Then
            Dim nameAttr As XmlAttribute = document.CreateAttribute("name")

            nameAttr.Value = component.Site.Name
            node.Attributes.Append(nameAttr)
            Debug.Assert(nametable(component) Is Nothing, "WriteObject не может быть вызвана более чем однократно на том же самом объекте. Использовать WriteReference взамен")
            nametable(value) = component.Site.Name
        End If

        Dim isControl As Boolean = (TypeOf value Is Control)

        If isControl Then
            Dim childAttr As XmlAttribute = document.CreateAttribute("children")

            childAttr.Value = "Controls"
            node.Attributes.Append(childAttr)
        End If

        If component IsNot Nothing Then
            If isControl Then
                For Each child As Control In DirectCast(value, Control).Controls
                    If child.Site IsNot Nothing AndAlso child.Site.Container Is idh.Container Then
                        node.AppendChild(WriteObject(document, nametable, child))
                    End If
                Next
            End If
            ' если это Control
            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(value, propertyAttributes)

            If isControl Then
                Dim controlProp As PropertyDescriptor = properties("Controls")

                If controlProp IsNot Nothing Then
                    Dim propArray As PropertyDescriptor() = New PropertyDescriptor(properties.Count - 2) {}
                    Dim idx As Integer = 0

                    For Each p As PropertyDescriptor In properties
                        If p IsNot controlProp Then
                            'propArray[idx++] = p;
                            propArray(idx) = p
                            idx += 1
                            'propArray(System.Math.Max(System.Threading.Interlocked.Increment(idx), idx - 1)) = p
                        End If
                    Next

                    properties = New PropertyDescriptorCollection(propArray)
                End If
            End If

            WriteProperties(document, properties, value, node, "Property")

            Dim events As EventDescriptorCollection = TypeDescriptor.GetEvents(value, propertyAttributes)
            Dim bindings As IEventBindingService = TryCast(host.GetService(GetType(IEventBindingService)), IEventBindingService)

            If bindings IsNot Nothing Then
                properties = bindings.GetEventProperties(events)
                WriteProperties(document, properties, value, node, "Event")
            End If
        Else
            WriteValue(document, value, node)
        End If

        Return node
    End Function

    ''' <summary>
    ''' Рекурсия по узлам Properties
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="properties"></param>
    ''' <param name="value"></param>
    ''' <param name="parent"></param>
    ''' <param name="elementName"></param>
    Private Sub WriteProperties(ByVal document As XmlDocument, ByVal properties As PropertyDescriptorCollection, ByVal value As Object, ByVal parent As XmlNode, ByVal elementName As String)
        For Each prop As PropertyDescriptor In properties
            'удалил***
            'If prop.Name = "AutoScaleBaseSize" Then
            '    Dim _DEBUG_ As String = prop.Name
            'End If

            If prop.ShouldSerializeValue(value) Then
                'Dim compName As String = parent.Name
                Dim node As XmlNode = document.CreateElement(elementName)
                Dim attr As XmlAttribute = document.CreateAttribute("name")

                attr.Value = prop.Name
                node.Attributes.Append(attr)

                Dim visibility As DesignerSerializationVisibilityAttribute = DirectCast(prop.Attributes(GetType(DesignerSerializationVisibilityAttribute)), DesignerSerializationVisibilityAttribute)

                Select Case visibility.Visibility
                    Case DesignerSerializationVisibility.Visible
                        If Not prop.IsReadOnly AndAlso WriteValue(document, prop.GetValue(value), node) Then
                            parent.AppendChild(node)
                        End If

                        Exit Select

                    Case DesignerSerializationVisibility.Content
                        Dim propValue As Object = prop.GetValue(value)

                        If GetType(IList).IsAssignableFrom(prop.PropertyType) Then
                            WriteCollection(document, DirectCast(propValue, IList), node)
                        Else
                            Dim props As PropertyDescriptorCollection = TypeDescriptor.GetProperties(propValue, propertyAttributes)

                            WriteProperties(document, props, propValue, node, elementName)
                        End If

                        If node.ChildNodes.Count > 0 Then
                            parent.AppendChild(node)
                        End If

                        Exit Select
                    Case Else

                        Exit Select
                End Select
            End If
        Next
    End Sub

    ''' <summary>
    ''' Рекурсия по узлам ссылочного типа
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Function WriteReference(ByVal document As XmlDocument, ByVal value As IComponent) As XmlNode
        Dim idh As IDesignerHost = DirectCast(host.GetService(GetType(IDesignerHost)), IDesignerHost)

        Debug.Assert(value IsNot Nothing AndAlso value.Site IsNot Nothing AndAlso value.Site.Container Is idh.Container, "Invalid component passed to WriteReference")

        Dim node As XmlNode = document.CreateElement("Reference")
        Dim attr As XmlAttribute = document.CreateAttribute("name")

        attr.Value = value.Site.Name
        node.Attributes.Append(attr)

        Return node
    End Function

    ''' <summary>
    ''' Рекурсия по узлам значимого типа
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="value"></param>
    ''' <param name="parent"></param>
    ''' <returns></returns>
    Private Function WriteValue(ByVal document As XmlDocument, ByVal value As Object, ByVal parent As XmlNode) As Boolean
        Dim idh As IDesignerHost = DirectCast(host.GetService(GetType(IDesignerHost)), IDesignerHost)

        ' Для пустого значения мы просто возвращаем True. Это порождает пустой узел.
        If value Is Nothing Then
            Return True
        End If

        Dim converter As TypeConverter = TypeDescriptor.GetConverter(value)

        If GetConversionSupported(converter, GetType(String)) Then
            parent.InnerText = DirectCast(converter.ConvertTo(Nothing, CultureInfo.InvariantCulture, value, GetType(String)), String)
        ElseIf GetConversionSupported(converter, GetType(Byte())) Then
            Dim data As Byte() = DirectCast(converter.ConvertTo(Nothing, CultureInfo.InvariantCulture, value, GetType(Byte())), Byte())

            parent.AppendChild(WriteBinary(document, data))
        ElseIf GetConversionSupported(converter, GetType(InstanceDescriptor)) Then
            Dim id As InstanceDescriptor = DirectCast(converter.ConvertTo(Nothing, CultureInfo.InvariantCulture, value, GetType(InstanceDescriptor)), InstanceDescriptor)

            parent.AppendChild(WriteInstanceDescriptor(document, id, value))
        ElseIf TypeOf value Is IComponent AndAlso DirectCast(value, IComponent).Site IsNot Nothing AndAlso DirectCast(value, IComponent).Site.Container Is idh.Container Then
            parent.AppendChild(WriteReference(document, DirectCast(value, IComponent)))
        ElseIf value.[GetType]().IsSerializable Then
            Dim formatter As New BinaryFormatter()
            Dim stream As New MemoryStream()

            formatter.Serialize(stream, value)

            Dim binaryNode As XmlNode = WriteBinary(document, stream.ToArray())

            parent.AppendChild(binaryNode)
        Else
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Рекурсия по узлам коллекции
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="list"></param>
    ''' <param name="parent"></param>
    Private Sub WriteCollection(ByVal document As XmlDocument, ByVal list As IList, ByVal parent As XmlNode)
        For Each obj As Object In list
            Dim node As XmlNode = document.CreateElement("Item")
            Dim typeAttr As XmlAttribute = document.CreateAttribute("type")

            typeAttr.Value = obj.[GetType]().AssemblyQualifiedName
            node.Attributes.Append(typeAttr)
            WriteValue(document, obj, node)
            parent.AppendChild(node)
        Next
    End Sub

    ''' <summary>
    ''' Рекурсия по узлам 8-ми разрядных значений
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Function WriteBinary(ByVal document As XmlDocument, ByVal value As Byte()) As XmlNode
        Dim node As XmlNode = document.CreateElement("Binary")

        node.InnerText = Convert.ToBase64String(value)
        Return node
    End Function

    ''' <summary>
    ''' Рекурсия по узлам членов
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="desc"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Function WriteInstanceDescriptor(ByVal document As XmlDocument, ByVal desc As InstanceDescriptor, ByVal value As Object) As XmlNode
        Dim node As XmlNode = document.CreateElement("InstanceDescriptor")
        Dim formatter As New BinaryFormatter()
        Dim stream As New MemoryStream()

        formatter.Serialize(stream, desc.MemberInfo)

        Dim memberAttr As XmlAttribute = document.CreateAttribute("member")

        memberAttr.Value = Convert.ToBase64String(stream.ToArray())
        node.Attributes.Append(memberAttr)

        For Each arg As Object In desc.Arguments
            Dim argNode As XmlNode = document.CreateElement("Argument")

            If WriteValue(document, arg, argNode) Then
                node.AppendChild(argNode)
            End If
        Next

        If Not desc.IsComplete Then
            Dim props As PropertyDescriptorCollection = TypeDescriptor.GetProperties(value, propertyAttributes)

            WriteProperties(document, props, value, node, "Property")
        End If

        Return node
    End Function

#End Region

#Region "DeSerialize - Load"

    ''' <summary>
    ''' Этот метод используется парсером данного файла. Перед вызовом этого метода
    ''' переменные членов хоста обязаны быть установлены. Этот метод будет
    ''' создавать набор данных, читать набор данных из XML сохранненного файла
    ''' и затем проходить сквозь набор данных и создавать компоненты сохраненные в данных.
    ''' 
    ''' Этот метод никогда не вызывает исключения. Будет устанавливать в случае успеха
    ''' ссылочный параметр или Nothing, в случае катастрофической ошибки невозможности
    ''' разрешить (подобно невозможности парсить XML). Все ошибки исключения
    ''' добавляются в лист ошибок, включая второстепенные ошибки.
    ''' </summary>
    Private Function ReadFile(ByVal fileName As String, ByVal errors As ArrayList, ByRef document As XmlDocument) As String
        Dim baseClass As String = Nothing
        Dim sr As New StreamReader(fileName)

        ' Любые неожиданности есть фатальная ошибка.
        Try
            ' В основной форме и элементах (component) будет тот-же самый уровень,
            ' таким образом, необходимо создать высший super-root корневой узел в порядке
            ' конструирования данного XmlDocument.

            'Dim cleandown As String = sr.ReadToEnd()
            'cleandown = "<DOCUMENT_ELEMENT>" & cleandown & "</DOCUMENT_ELEMENT>"
            Dim cleandown As String = String.Format("<DOCUMENT_ELEMENT>{0}</DOCUMENT_ELEMENT>", sr.ReadToEnd())

            Dim doc As New XmlDocument()
            doc.LoadXml(cleandown)

            ' Сейчас пройти сквозь элементы документа
            For Each node As XmlNode In doc.DocumentElement.ChildNodes
                ' первый узел =Object type="System.Windows.Forms.Form все остальные вложены
                If baseClass Is Nothing Then
                    baseClass = node.Attributes("name").Value
                End If

                ' идти через все элементы Object
                If node.Name.Equals("Object") Then
                    ReadObject(node, errors)
                Else
                    errors.Add(String.Format("Узел типа {0} не разрешен здесь.", node.Name))
                End If
            Next

            document = doc
        Catch ex As Exception
            document = Nothing
            errors.Add(ex)
        Finally
            sr.Close()
        End Try

        Return baseClass
    End Function

    ''' <summary>
    ''' Чтение события
    ''' </summary>
    ''' <param name="childNode"></param>
    ''' <param name="instance"></param>
    ''' <param name="errors"></param>
    Private Sub ReadEvent(ByVal childNode As XmlNode, ByVal instance As Object, ByVal errors As ArrayList)
        Dim bindings As IEventBindingService = TryCast(host.GetService(GetType(IEventBindingService)), IEventBindingService)

        If bindings Is Nothing Then
            errors.Add("Невозможно присоединить сервис построителя события и таким образом невозможно создать какие-либо события")
            Return
        End If

        Dim nameAttr As XmlAttribute = childNode.Attributes("name")

        If nameAttr Is Nothing Then
            errors.Add("Нет имени события")
            Return
        End If

        Dim methodAttr As XmlAttribute = childNode.Attributes("method")

        If methodAttr Is Nothing OrElse methodAttr.Value Is Nothing OrElse methodAttr.Value.Length = 0 Then
            errors.Add(String.Format("Событие {0} не имеет методов привязки к нему.", nameAttr.Value))
            Return
        End If

        Dim evt As EventDescriptor = TypeDescriptor.GetEvents(instance)(nameAttr.Value)

        If evt Is Nothing Then
            errors.Add(String.Format("Событие {0} не существует на {1}", nameAttr.Value, instance.[GetType]().FullName))
            Return
        End If

        Dim prop As PropertyDescriptor = bindings.GetEventProperty(evt)

        Debug.Assert(prop IsNot Nothing, "Испорчено событие связывания")
        Try
            prop.SetValue(instance, methodAttr.Value)
        Catch ex As Exception
            errors.Add(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Чтение описание членов объекта
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    Private Function ReadInstanceDescriptor(ByVal node As XmlNode, ByVal errors As ArrayList) As Object
        ' в начале необходимо десериализовать членов
        Dim memberAttr As XmlAttribute = node.Attributes("member")

        If memberAttr Is Nothing Then
            errors.Add("Не член аттрибута экземпляра дескриптора")
            Return Nothing
        End If

        Dim data As Byte() = Convert.FromBase64String(memberAttr.Value)
        Dim formatter As New BinaryFormatter()
        Dim stream As New MemoryStream(data)
        Dim mi As MemberInfo = DirectCast(formatter.Deserialize(stream), MemberInfo)
        Dim args As Object() = Nothing


        ' Просмотреть, необходимы ли члену аргументы. Если так собрать их из XML. 
        If TypeOf mi Is MethodBase Then
            Dim paramInfos As ParameterInfo() = DirectCast(mi, MethodBase).GetParameters()
            Dim idx As Integer = 0

            args = New Object(paramInfos.Length - 1) {}

            For Each child As XmlNode In node.ChildNodes
                If child.Name.Equals("Argument") Then
                    Dim value As Object = Nothing

                    If Not ReadValue(child, TypeDescriptor.GetConverter(paramInfos(idx).ParameterType), errors, value, Nothing) Then
                        Return Nothing
                    End If
                    'args[idx++] = value;
                    'args(System.Math.Max(System.Threading.Interlocked.Increment(idx), idx - 1)) = value
                    args(idx) = value
                    idx += 1
                End If
            Next

            If idx <> paramInfos.Length Then
                errors.Add(String.Format("Член {0} требует {1} аргумент, не {2}.", mi.Name, args.Length, idx))
                Return Nothing
            End If
        End If

        Dim id As New InstanceDescriptor(mi, args)
        Dim instance As Object = id.Invoke()

        ' Ok, мы имеем объект. Сейчас просмотреть, есть ли любые свойства, и становлены ли они.
        For Each prop As XmlNode In node.ChildNodes
            If prop.Name.Equals("Property") Then
                ReadProperty(prop, instance, errors)
            End If
        Next

        Return instance
    End Function

    ''' <summary>
    ''' Чтение "Object" tags. Функция возвращает экземпляр последнего созданного объекта.
    ''' Возвратит null, если будет ошибка.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    Private Function ReadObject(ByVal node As XmlNode, ByVal errors As ArrayList) As Object
        'каждый узел имеет аттрибут type = с типом элемента
        Dim typeAttr As XmlAttribute = node.Attributes("type")

        If typeAttr Is Nothing Then
            errors.Add("<Object> tag отсутствует необходимый тип аттрибута")
            Return Nothing
        End If

        ' преобразовать текст значения узла в тип  type= "Тип Контрола"
        Dim typeControl As Type = Type.[GetType](typeAttr.Value)

        If typeControl Is Nothing Then
            errors.Add(String.Format("Тип {0} не может быть загружен.", typeAttr.Value))
            Return Nothing
        End If

        ' Эдесь может быть нуль, если нет имени для объекта
        Dim nameAttr As XmlAttribute = node.Attributes("name")
        Dim instance As Object

        ' Определить может ли экземпляр текущего System.Type быть назначен для экземпляра специализированного типа.
        If GetType(IComponent).IsAssignableFrom(typeControl) Then
            If nameAttr Is Nothing Then
                instance = host.CreateComponent(typeControl)
            Else
                'Dim instance2 As Object
                'If typeControl Is GetType(NationalInstruments.UI.WindowsForms.ScatterGraph) Then
                '    'instance2 = New NationalInstruments.UI.WindowsForms.ScatterGraph
                '    'instance = host.CreateComponent(typeControl, nameAttr.Value)
                '    'instance = instance2
                'ElseIf typeControl Is GetType(NationalInstruments.UI.ScatterPlot) Then
                '    'instance2 = New NationalInstruments.UI.ScatterPlot
                '    'instance = host.CreateComponent(typeControl, nameAttr.Value)
                '    'instance = instance2
                'ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.WaveformGraph) Then
                '    'instance2 = New NationalInstruments.UI.WindowsForms.WaveformGraph
                '    'instance = host.CreateComponent(typeControl, nameAttr.Value)
                '    ''instance = instance2
                'If typeControl Is GetType(NationalInstruments.UI.WaveformPlot) Then
                '    '    'instance2 = New NationalInstruments.UI.WaveformPlot
                '    '    'instance = host.CreateComponent(typeControl, nameAttr.Value)
                '    '    'instance = instance2
                '    'ElseIf typeControl Is GetType(NationalInstruments.UI.XAxis) Then
                '    '    'instance = New NationalInstruments.UI.XAxis
                '    'ElseIf typeControl Is GetType(NationalInstruments.UI.YAxis) Then
                '    '    'instance = New NationalInstruments.UI.YAxis

                'Else
                ' создать компонент хостом
                instance = host.CreateComponent(typeControl, nameAttr.Value)
                'End If
            End If
        Else
            instance = Activator.CreateInstance(typeControl)
        End If

        ' Получили объект, сейчас необходимо вызвать его. Просмотреть,
        ' содержит ли этот tag дочерние коллекции для добавления дочерних типов так-же.
        Dim childAttr As XmlAttribute = node.Attributes("children")
        Dim childList As IList = Nothing

        If childAttr IsNot Nothing Then
            ' значить это вложенный контрол - аттрибут с именем "Controls"
            Dim childProp As PropertyDescriptor = TypeDescriptor.GetProperties(instance)(childAttr.Value)

            If childProp Is Nothing Then
                If instance Is Nothing Then
                    errors.Add(String.Format("Дочерний лист аттрибутов {0} должен быть дочерняя коллекция, но этого нет в объекте {1}", childAttr.Value, typeControl.Name))
                Else
                    errors.Add(String.Format("Дочерний лист аттрибутов {0} должен быть дочерняя коллекция, но этого нет в свойстве {1}", childAttr.Value, instance.[GetType]().FullName))
                End If
            Else
                childList = TryCast(childProp.GetValue(instance), IList)

                If childList Is Nothing Then
                    errors.Add(String.Format("Свойство {0} найдено, но не возвращает корректный IList", childProp.Name))
                End If
            End If
        End If

        ' Сейчас, пройти по оставшейся части признаков в этом элементом
        For Each childNode As XmlNode In node.ChildNodes
            If childNode.Name.Equals("Object") Then
                ' Другой объект. В этом случае, создать объект,  
                ' как родителя, для использования его дочерних свойств. 

                ' Если здесь нет дочерних свойств просто выйти.
                If childAttr Is Nothing Then
                    errors.Add("Дочерний объект найден, но здесь нет дочерних аттрибутов.")
                    Continue For
                End If

                ' Ни какого смысла дальнейшего создания типа, если была ошибка получения свойства. Ошибка уже сообщена ниже.
                If childList IsNot Nothing Then
                    Dim childInstance As Object = ReadObject(childNode, errors)
                    childList.Add(childInstance)
                End If
            ElseIf childNode.Name.Equals("Property") Then
                ' Свойство. Попытаться свойство парсить само себя.
                ReadProperty(childNode, instance, errors)
            ElseIf childNode.Name.Equals("Event") Then
                ' Событие. Попытаться событие парсить само себя.
                ReadEvent(childNode, instance, errors)
            End If
        Next

        Return instance
    End Function

    ''' <summary>
    ''' Парсить XML узел и установить значение результирующего свойства.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="instance"></param>
    ''' <param name="errors"></param>
    Private Sub ReadProperty(ByVal node As XmlNode, ByVal instance As Object, ByVal errors As ArrayList)
        Dim nameAttr As XmlAttribute = node.Attributes("name")

        If nameAttr Is Nothing Then
            errors.Add("Свойство не имеет имя")
            Return
        End If

        Dim prop As PropertyDescriptor = TypeDescriptor.GetProperties(instance)(nameAttr.Value)

        If prop Is Nothing Then
            errors.Add(String.Format("Свойство {0} не существует в {1}", nameAttr.Value, instance.[GetType]().FullName))
            Return
        End If

        ' Выдать тип этого свойства. Имеется три опции:
        ' 1.  Нормальное read/write свойство.
        ' 2.  "Содержимое" свойство.
        ' 3.  Коллекция.
        '
        Dim isContent As Boolean = prop.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content)

        If isContent Then
            Dim value As Object = prop.GetValue(instance)

            ' Указатель свойства представляет коллекцию
            If TypeOf value Is IList Then
                For Each child As XmlNode In node.ChildNodes
                    If child.Name.Equals("Item") Then
                        Dim item As Object = Nothing
                        Dim typeAttr As XmlAttribute = child.Attributes("type")

                        If typeAttr Is Nothing Then
                            errors.Add("Элемент не имеет тип аттрибута")
                            Continue For
                        End If

                        Dim type__1 As Type = Type.[GetType](typeAttr.Value)

                        If type__1 Is Nothing Then
                            errors.Add(String.Format("Элемент типа {0} не может быть найден.", typeAttr.Value))
                            Continue For
                        End If

                        If ReadValue(child, TypeDescriptor.GetConverter(type__1), errors, item, type__1.Name) Then
                            Try
                                DirectCast(value, IList).Add(item)
                            Catch ex As Exception
                                errors.Add(ex.Message)
                            End Try
                        End If
                    Else
                        errors.Add(String.Format("Только элемент колекции разрешен в коллекции, не {0} эдемент.", child.Name))
                    End If
                Next
            Else
                ' Указатель свойства содержит дочерние свойства.
                For Each child As XmlNode In node.ChildNodes
                    If child.Name.Equals("Property") Then
                        ReadProperty(child, value, errors)
                    Else
                        errors.Add(String.Format("Только Property элементы разрешены в содержимом свойств, не {0} эдемент.", child.Name))
                    End If
                Next
            End If
        Else
            Dim value As Object = Nothing

            If ReadValue(node, prop.Converter, errors, value, Nothing) Then
                ' ReadValue успешно.  Заполнить в свойстве значение.
                Try
                    prop.SetValue(instance, value)
                Catch ex As Exception
                    errors.Add(ex.Message)
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Обобщенная функция чтения значения объекта. Взвращает true если чтение успешно.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="converter"></param>
    ''' <param name="errors"></param>
    ''' <param name="value"></param>
    ''' <param name="typeName"></param>
    ''' <returns></returns>
    Private Function ReadValue(ByVal node As XmlNode, ByVal converter As TypeConverter, ByVal errors As ArrayList, ByRef value As Object, ByVal typeName As String) As Boolean
        Try
            For Each child As XmlNode In node.ChildNodes
                If child.NodeType = XmlNodeType.Text Then
                    Try
                        If converter.ToString = "NationalInstruments.UI.Converters.RangeConverter" Then
                            'value = converter.ConvertFromInvariantString(node.InnerText.Replace(",", ";"))
                            value = converter.ConvertFromInvariantString(node.InnerText)
                        ElseIf converter.ToString = "NationalInstruments.UI.Converters.PlotConverter" Then
                            If typeName = "ScatterPlot" Then
                                value = New ScatterPlot
                            ElseIf typeName = "WaveformPlot" Then
                                value = New WaveformPlot()
                            End If
                        ElseIf converter.ToString = "NationalInstruments.UI.Converters.XAxisConverter" Then
                            value = New XAxis
                        ElseIf converter.ToString = "NationalInstruments.UI.Converters.YAxisConverter" Then
                            value = New YAxis
                        ElseIf converter.ToString = "NationalInstruments.UI.Converters.XYPlotYAxisConverter" Then
                            value = New YAxis
                        ElseIf converter.ToString = "NationalInstruments.UI.Converters.XYPlotXAxisConverter" Then
                            value = New XAxis
                        Else
                            value = converter.ConvertFromInvariantString(node.InnerText)
                        End If

                        Return True
                    Catch ex As Exception
                        value = Nothing
                        Return False
                    End Try
                ElseIf child.Name.Equals("Binary") Then
                    Dim data As Byte() = Convert.FromBase64String(child.InnerText)

                    ' Бинарный массив. Прсмотреть, может ли конвертер конвертировать его.
                    ' Если нет, то использовать сериализацию.
                    If GetConversionSupported(converter, GetType(Byte())) Then
                        value = converter.ConvertFrom(Nothing, CultureInfo.InvariantCulture, data)
                        Return True
                    Else
                        Dim formatter As New BinaryFormatter()
                        Dim stream As New MemoryStream(data)

                        value = formatter.Deserialize(stream)
                        Return True
                    End If
                ElseIf child.Name.Equals("InstanceDescriptor") Then
                    value = ReadInstanceDescriptor(child, errors)
                    Return (value IsNot Nothing)
                Else
                    errors.Add(String.Format("Неопределенный element type {0}", child.Name))
                    value = Nothing
                    Return False
                End If
            Next

            ' Если оказались здесь, значит нет узлов. Нет узлов и нет внутреннего текста в узле.
            ' Значит возвратить Nothing.
            value = Nothing
            Return True
        Catch ex As Exception
            errors.Add(ex.Message)
            value = Nothing
            Return False
        End Try
    End Function

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Этот метод очищает содержимое проекта в XML.
    ''' Затем записывает содержимое xmlDocument.    ''' </summary>
    ''' <returns></returns>
    Public Function GetCode() As String
        Flush()

        Dim sw As StringWriter = New StringWriter()
        Dim xtw As New XmlTextWriter(sw) With {.Formatting = Formatting.Indented}

        xmlDocument.WriteTo(xtw)

        Dim cleanup As String = sw.ToString().Replace("<DOCUMENT_ELEMENT>", "")

        cleanup = cleanup.Replace("</DOCUMENT_ELEMENT>", "")
        sw.Close()

        Return cleanup
    End Function

    Public Sub Save()
        Save(False)
    End Sub

    ''' <summary>
    ''' Записать текущее состояние загрузчика. Если пользователь загрузил файл
    ''' или когда-то записал его прежде, тогда он не нуждается в выборе файла снова.
    ''' Если запись вызвана из пукта "Save As..." то,
    ''' в этом случае forceFilePrompt будет true.
    ''' </summary>
    Public Sub Save(ByVal forceFilePrompt As Boolean)
        Try
            Flush()

            Dim filterIndex As Integer = 3

            If (fileName Is Nothing) OrElse forceFilePrompt Then
                Dim dlg As New SaveFileDialog() With {.DefaultExt = "xml", .Filter = "XML Files|*.xml"}
                If dlg.ShowDialog() = DialogResult.OK Then
                    fileName = dlg.FileName
                    filterIndex = dlg.FilterIndex
                End If
            End If

            If fileName IsNot Nothing Then
                Select Case filterIndex
                    Case 1, 3
                        If True Then
                            ' Записать xmlDocument в файл.
                            Dim sw As New StringWriter()
                            Dim xtw As New XmlTextWriter(sw) With {.Formatting = Formatting.Indented}
                            xmlDocument.WriteTo(xtw)

                            ' Избавится от искусственного корневого узла super-root прежде чем будет произведена запись XML.
                            Dim cleanup As String = sw.ToString().Replace("<DOCUMENT_ELEMENT>", "")

                            cleanup = cleanup.Replace("</DOCUMENT_ELEMENT>", "")
                            xtw.Close()

                            Dim file As New StreamWriter(fileName)

                            file.Write(cleanup)
                            file.Close()
                        End If
                        Exit Select
                End Select
                unsaved = False
            End If
        Catch ex As Exception
            Const caption As String = "BasicHostLoader:Save"
            Dim text As String = "Error during save: " & ex.ToString()
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

#End Region

End Class



''' <summary>
''' По аналогии BasicHostLoader. Он продолжает HostSurface
''' для Xml файла и может также разобрать Xml файл воссоздав RootComponent
''' и все компоненты, что он хостит.
''' </summary>
Friend Class BasicHostLoaderRun 'было public

    'Private root As IComponent
    'Private dirty As Boolean = True
    'Private unsaved As Boolean
    Private fileName As String
    'Private host As IDesignerLoaderHost
    Private xmlDocument As XmlDocument
    'Private Shared ReadOnly propertyAttributes As Attribute() = New Attribute() {DesignOnlyAttribute.No}
    'Private rootComponentType As Type

#Region "Constructors"

    '''' Пустой конструктор просто создает форму.
    'Public Sub New(ByVal rootComponentType As Type)
    '    Me.rootComponentType = rootComponentType
    '    'Me.Modified = True
    'End Sub

    ''' <summary>
    ''' Этот конструктор берёт имя файла. Этот файл
    ''' должен существовать на диске и содержит XML,
    ''' который может быть прочитан как набор данных.
    ''' </summary>
    Public Sub New(ByVal fileName As String, ByRef frm As FormBasePanelMotorist)
        If fileName Is Nothing Then
            Throw New ArgumentNullException("fileName")
        End If

        Me.fileName = fileName
        PanelBaseFormProperty = frm
    End Sub

    Public Property PanelBaseFormProperty() As FormBasePanelMotorist

#End Region

    ''' <summary>
    ''' Простой вспомогательный метод что возвращает true,
    ''' если данный тип конвертера поддерживает два способа конверсии данного типа.
    ''' </summary>
    Private Function GetConversionSupported(ByVal converter As TypeConverter, ByVal conversionType As Type) As Boolean
        Return (converter.CanConvertFrom(conversionType) AndAlso converter.CanConvertTo(conversionType))
    End Function

    ''' <summary>
    ''' Вызывается хостом, когда загружается форма из сереализованного файла
    ''' для заполнения всеми элементами
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PerformLoad2(ByRef errorsText As String) '(ByVal designerSerializationManager As IDesignerSerializationManager)
        'Me.host = Me.LoaderHost

        'If host Is Nothing Then
        '    Throw New ArgumentNullException("BasicHostLoader.BeginLoad: Invalid designerLoaderHost.")
        'End If

        ' Загрузчик будет передавать сюда все сообщения об ошибках.
        Dim errors As New List(Of String) 'ArrayList()
        'Dim successful As Boolean = True
        Dim baseClassName As String

        ' Если ни одно имя файла не было пройдено, просто создать форму и делать без данных. 
        ' Если файл имя было найдено, прочесть его.
        'If fileName Is Nothing Then
        '    If rootComponentType Is GetType(Form) Then
        '        'host.CreateComponent(GetType(Form))
        '        baseClassName = "Form1"

        '    Else
        '        Throw New Exception("Undefined Host Type: " & rootComponentType.ToString())
        '    End If
        'Else
        baseClassName = ReadFile2(fileName, errors, xmlDocument)
        'End If

        ' Загрузка прошла успешно, необходимо начать слушать события.
        ' События слушателя извещают, что дизайнер "Loader" и может также быть использован для записи данных.
        ' Если необходимо интегрировать загрузчик с контролем источника кода,
        ' необходимо слушать "-ing" сообщения также как и "-ed" сообщения.
        'Dim cs As IComponentChangeService = TryCast(host.GetService(GetType(IComponentChangeService)), IComponentChangeService)

        'If cs IsNot Nothing Then
        '    AddHandler cs.ComponentChanged, New ComponentChangedEventHandler(AddressOf OnComponentChanged)
        '    AddHandler cs.ComponentAdded, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
        '    AddHandler cs.ComponentRemoved, New ComponentEventHandler(AddressOf OnComponentAddedRemoved)
        'End If

        ' Позволить хосту знать, что загрузка сделана
        'host.EndLoad(baseClassName, successful, errors) 'до этого был вызов hostSurface.BeginLoad(basicHostLoader)

        ' Документ просто загружен, таким образом программа нуждается в потоке изменений.
        'dirty = True
        'unsaved = False

        If errors.Count > 0 Then
            Dim Result As New StringBuilder(String.Format("При загрузке функциональной панели <{0}> были обнаружены следующие проблемы:{1}",
                                                          PanelBaseFormProperty.NameMotoristPanel, Environment.NewLine))
            Dim I As Integer

            For Each strError As String In errors
                Result.AppendLine(strError)
                I += 1
                If I > 80 Then
                    Result.AppendLine("и далее..")
                    Exit For
                End If
            Next
            errorsText = Result.ToString
        End If

    End Sub

    ''' <summary>
    ''' Этот метод используется парсером данного файла. Перед вызовом этого метода
    ''' переменные членов хоста обязаны быть установлены. Этот метод будет
    ''' создавать набор данных, читать набор данных из XML сохранненного файла
    ''' и затем проходить сквозь набор данных и создавать компоненты сохраненные в данных.
    ''' 
    ''' Этот метод никогда не вызывает исключения. Будет устанавливать в случае успеха
    ''' ссылочный параметр или Nothing, в случае катастрофической ошибки невозможности
    ''' разрешить (подобно невозможности парсить XML). Все ошибки исключения
    ''' добавляются в лист ошибок, включая второстепенные ошибки.
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="errors"></param>
    ''' <param name="document"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadFile2(ByVal fileName As String, ByVal errors As List(Of String), ByRef document As XmlDocument) As String
        Dim baseClass As String = Nothing
        Dim sr As New StreamReader(fileName)

        'Любые неожиданности есть фатальная ошибка.
        Try
            ' В основной форме и элементах (component) будет тот-же самый уровень,
            ' таким образом, необходимо создать высший super-root корневой узел в порядке
            ' конструирования данного XmlDocument.

            Dim cleandown As String = sr.ReadToEnd()
            cleandown = String.Format("<DOCUMENT_ELEMENT>{0}</DOCUMENT_ELEMENT>", cleandown)

            Dim doc As New XmlDocument()

            doc.LoadXml(cleandown)

            ' Сейчас пройти сквозь элементы документа
            For Each node As XmlNode In doc.DocumentElement.ChildNodes
                ' первый узел = Object type = "System.Windows.Forms.Form. Все остальные вложены
                If baseClass Is Nothing Then
                    baseClass = node.Attributes("name").Value
                End If
                ' идти через все элементы Object
                If node.Name.Equals("Object") Then
                    'ReadObject(node, errors)
                    ' Здесь будет заполнен контролами класс формы верхнего уровня
                    If PanelBaseFormProperty IsNot Nothing Then
                        ReadObject2(node, errors)
                    End If

                Else
                    errors.Add(String.Format("Узел типа {0} не разрешен здесь.", node.Name))
                End If
            Next
            document = doc
        Catch ex As Exception
            document = Nothing
            errors.Add(ex.Message & Environment.NewLine & ex.StackTrace)
            'errors.Add(ex)
        Finally
            sr.Close()
        End Try

        Return baseClass
    End Function


    ''' <summary>
    ''' Чтение "Object" tags. Функция возвращает экземпляр последнего созданного объекта.
    ''' Возвратит null, если будет ошибка.
    ''' Для компоненты MeasurementStudio в данном проекте должн быть создан файл лицензии
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadObject2(ByVal node As XmlNode, ByVal errors As List(Of String)) As Object
        'Private Function ReadObject2(ByVal node As XmlNode, ByVal errors As ArrayList) As Object
        ' каждый узел имеет аттрибут type = с типом элемента
        Dim typeAttr As XmlAttribute = node.Attributes("type")

        If typeAttr Is Nothing Then
            errors.Add("Отсутствует tag <Object>, необходимый аттрибута для типа")
            Return Nothing
        End If

        ' преобразовать текст значения узла в тип  type= "Тип Контрола"
        Dim typeControl As Type = Type.[GetType](typeAttr.Value)

        If typeControl Is Nothing Then
            errors.Add(String.Format("Тип {0} не может быть загружен.", typeAttr.Value))
            Return Nothing
        End If

        ' Эдесь может быть нуль если нет имени для объекта name="Имя Контрола"
        Dim nameAttr As XmlAttribute = node.Attributes("name")
        Dim instance As Object = Nothing

        ' Определить может ли экземпляр текущего System.Type быть назначен для экземпляра специализированного типа.
        If GetType(IComponent).IsAssignableFrom(typeControl) Then
            If nameAttr Is Nothing Then
                'instance = host.CreateComponent(typeControl)
            Else
                ' создать компонент хостом
                'instance = host.CreateComponent(typeControl, nameAttr.Value)

                ' создать компонент в мою форму
                'If myPanelBaseForm IsNot Nothing Then
                If typeControl Is GetType(Form) Then
                    instance = PanelBaseFormProperty

                ElseIf typeControl Is GetType(Label) Then
                    instance = New Label
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.NumericEdit) Then
                    instance = New NationalInstruments.UI.WindowsForms.NumericEdit
                    ' чтобы не было ошибки
                    CType(instance, NationalInstruments.UI.WindowsForms.NumericEdit).OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Switch) Then
                    instance = New NationalInstruments.UI.WindowsForms.Switch
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Led) Then
                    instance = New NationalInstruments.UI.WindowsForms.Led

                ElseIf typeControl Is GetType(TextBox) Then
                    instance = New TextBox
                ElseIf typeControl Is GetType(Button) Then
                    instance = New Button
                ElseIf typeControl Is GetType(CheckBox) Then
                    instance = New CheckBox
                ElseIf typeControl Is GetType(RadioButton) Then
                    instance = New RadioButton
                ElseIf typeControl Is GetType(NumericUpDown) Then
                    instance = New NumericUpDown


                ElseIf typeControl Is GetType(Panel) Then
                    instance = New Panel
                ElseIf typeControl Is GetType(GroupBox) Then
                    instance = New GroupBox
                ElseIf typeControl Is GetType(TabControl) Then
                    instance = New TabControl
                ElseIf typeControl Is GetType(TabPage) Then
                    instance = New TabPage
                ElseIf typeControl Is GetType(TableLayoutPanel) Then
                    instance = New TableLayoutPanel
                ElseIf typeControl Is GetType(FlowLayoutPanel) Then
                    instance = New FlowLayoutPanel
                ElseIf typeControl Is GetType(PictureBox) Then
                    instance = New PictureBox


                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Tank) Then
                    instance = New NationalInstruments.UI.WindowsForms.Tank
                    CType(instance, NationalInstruments.UI.WindowsForms.Tank).OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Gauge) Then
                    instance = New NationalInstruments.UI.WindowsForms.Gauge
                    CType(instance, NationalInstruments.UI.WindowsForms.Gauge).OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Thermometer) Then
                    instance = New NationalInstruments.UI.WindowsForms.Thermometer
                    CType(instance, NationalInstruments.UI.WindowsForms.Thermometer).OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Meter) Then
                    instance = New NationalInstruments.UI.WindowsForms.Meter
                    CType(instance, NationalInstruments.UI.WindowsForms.Meter).OutOfRangeMode = NumericOutOfRangeMode.CoerceToRange

                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Knob) Then
                    instance = New NationalInstruments.UI.WindowsForms.Knob
                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.Slide) Then
                    instance = New NationalInstruments.UI.WindowsForms.Slide

                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.WaveformGraph) Then
                    instance = New NationalInstruments.UI.WindowsForms.WaveformGraph
                ElseIf typeControl Is GetType(WaveformPlot) Then
                    instance = New WaveformPlot

                ElseIf typeControl Is GetType(NationalInstruments.UI.WindowsForms.ScatterGraph) Then
                    instance = New NationalInstruments.UI.WindowsForms.ScatterGraph
                ElseIf typeControl Is GetType(ScatterPlot) Then
                    instance = New ScatterPlot

                ElseIf typeControl Is GetType(XAxis) Then
                    instance = New XAxis
                ElseIf typeControl Is GetType(YAxis) Then
                    instance = New YAxis

                End If
                'End If
                'If typeControl IsNot GetType(System.Windows.Forms.Form) Then
                '    myPanelBaseForm.Controls.Add(instance)
                'End If

            End If
        Else
            instance = Activator.CreateInstance(typeControl)
        End If

        ' Получили объект, сейчас необходимо вызвать его. Просмотреть если этот tag
        ' содержит дочернюю коллекцию для добавления дочерних так-же. 
        ' children="Controls"
        Dim childAttr As XmlAttribute = node.Attributes("children")
        Dim childList As IList = Nothing

        If childAttr IsNot Nothing Then
            ' значить это вложенный контрол - аттрибут с именем "Controls"
            Dim childProp As PropertyDescriptor = TypeDescriptor.GetProperties(instance)(childAttr.Value)

            If childProp Is Nothing Then
                errors.Add(String.Format("Дочерний лист аттрибутов {0} как дочерняя коллекция, но это не свойство {1}", childAttr.Value, instance.[GetType]().FullName))
            Else
                childList = TryCast(childProp.GetValue(instance), IList)
                If childList Is Nothing Then
                    errors.Add(String.Format("Свойство {0} найдено, но не возвращает корректный IList", childProp.Name))
                End If
            End If
        End If

        ' Сейчас, пройти по оставшейся части признаков в этом элементом
        For Each childNode As XmlNode In node.ChildNodes
            If childNode.Name.Equals("Object") Then
                ' Другой объект. В этом случае, создать объект,  
                ' как родителя, для использования его дочерних свойств. 

                ' Если здесь нет дочерних свойств просто выйти.
                ' здесь должно идти type= "Тип Контрола"
                If childAttr Is Nothing Then
                    errors.Add("Дочерний объект найден, но здесь нет дочерних аттрибутов.")
                    Continue For
                End If

                ' Ни какого смысла дальнейшего создания типа, если была ошибка получения свойства. Ошибка уже сообщена ниже.
                If childList IsNot Nothing Then
                    Dim childInstance As Object = ReadObject2(childNode, errors)
                    childList.Add(childInstance)
                End If
            ElseIf childNode.Name.Equals("Property") Then
                ' здесь типа такого <Property name="Size">272, 168</Property>
                ' Свойство. Попытаться свойство парсить само себя.
                ReadProperty2(childNode, instance, errors)
                'ElseIf childNode.Name.Equals("Event") Then
                '    'Событие. Попросить событие парсить само себя.
                '    Stop
                '    'ReadEvent2(childNode, instance, errors)
            End If
        Next

        Return instance
    End Function


    ''' <summary>
    ''' Парсить XML узел и установить значение результирующего свойства.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="instance"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Private Sub ReadProperty2(ByVal node As XmlNode, ByVal instance As Object, ByVal errors As List(Of String))
        Dim nameAttr As XmlAttribute = node.Attributes("name")

        If nameAttr Is Nothing Then
            errors.Add("Свойство не имеет имя")
            Return
        End If

        Dim prop As PropertyDescriptor = TypeDescriptor.GetProperties(instance)(nameAttr.Value)

        If prop Is Nothing Then
            errors.Add(String.Format("Свойство {0} не существует в {1}", nameAttr.Value, instance.[GetType]().FullName))
            Return
        End If

        ' Выдать тип этого свойства. Имеется три опции:
        ' 1.  Нормальное read/write свойство.
        ' 2.  "Содержимое" свойство.
        ' 3.  Коллекция.
        '
        Dim isContent As Boolean = prop.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content)

        If isContent Then
            Dim value As Object = prop.GetValue(instance)

            ' Указатель свойства представляет коллекцию
            If TypeOf value Is IList Then
                For Each child As XmlNode In node.ChildNodes
                    If child.Name.Equals("Item") Then
                        Dim item As Object = Nothing
                        Dim typeAttr As XmlAttribute = child.Attributes("type")

                        If typeAttr Is Nothing Then
                            errors.Add("Элемент не имеет тип аттрибута")
                            Continue For
                        End If

                        Dim type__1 As Type = Type.[GetType](typeAttr.Value)

                        If type__1 Is Nothing Then
                            errors.Add(String.Format("Элемент типа {0} не может быть найден.", typeAttr.Value))
                            Continue For
                        End If

                        If ReadValue2(child, TypeDescriptor.GetConverter(type__1), errors, item, type__1.Name) Then
                            Try
                                DirectCast(value, IList).Add(item)
                            Catch ex As Exception
                                errors.Add(ex.Message)
                            End Try
                        End If
                    Else
                        errors.Add(String.Format("Только элемент колекции разрешен в коллекции, не {0} эдемент.", child.Name))
                    End If
                Next
            Else
                ' Указатель свойства содержит дочерние свойства.
                For Each child As XmlNode In node.ChildNodes
                    If child.Name.Equals("Property") Then
                        ReadProperty2(child, value, errors)
                    Else
                        errors.Add(String.Format("Только Property элементы разрешены в содержимом свойств, не {0} эдемент.", child.Name))
                    End If
                Next
            End If
        Else
            Dim value As Object = Nothing

            If ReadValue2(node, prop.Converter, errors, value, Nothing) Then
                ' ReadValue успешно.  Заполнить в свойстве значение.
                '
                Try
                    prop.SetValue(instance, value)
                Catch ex As Exception
                    errors.Add(ex.Message)
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Обобщенная функция чтения значения объекта. Взвращает true если чтение успешно.
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="converter"></param>
    ''' <param name="errors"></param>
    ''' <param name="value"></param>
    ''' <param name="typeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>    
    Private Function ReadValue2(ByVal node As XmlNode, ByVal converter As TypeConverter, ByVal errors As List(Of String), ByRef value As Object, ByVal typeName As String) As Boolean
        Try
            For Each child As XmlNode In node.ChildNodes
                If child.NodeType = XmlNodeType.Text Then
                    If converter.ToString = "NationalInstruments.UI.Converters.RangeConverter" Then
                        'value = converter.ConvertFromInvariantString(node.InnerText.Replace(",", ";"))
                        value = converter.ConvertFromInvariantString(node.InnerText)
                    ElseIf converter.ToString = "NationalInstruments.UI.Converters.PlotConverter" Then
                        If typeName = "ScatterPlot" Then
                            value = New ScatterPlot
                        ElseIf typeName = "WaveformPlot" Then
                            value = New WaveformPlot()
                        End If
                    ElseIf converter.ToString = "NationalInstruments.UI.Converters.XAxisConverter" Then
                        value = New XAxis
                    ElseIf converter.ToString = "NationalInstruments.UI.Converters.YAxisConverter" Then
                        value = New YAxis
                    ElseIf converter.ToString = "NationalInstruments.UI.Converters.XYPlotYAxisConverter" Then
                        value = New YAxis
                    ElseIf converter.ToString = "NationalInstruments.UI.Converters.XYPlotXAxisConverter" Then
                        value = New XAxis
                    Else
                        value = converter.ConvertFromInvariantString(node.InnerText)
                    End If
                    Return True
                ElseIf child.Name.Equals("Binary") Then
                    Dim data As Byte() = Convert.FromBase64String(child.InnerText)

                    ' Бинарный массив. Посмотреть, может ли конвертер типа конвертировать его.
                    ' Если нет, то использовать сериализацию.
                    If GetConversionSupported(converter, GetType(Byte())) Then
                        value = converter.ConvertFrom(Nothing, CultureInfo.InvariantCulture, data)
                        Return True
                    Else
                        Dim formatter As New BinaryFormatter()
                        Dim stream As New MemoryStream(data)

                        value = formatter.Deserialize(stream)
                        Return True
                    End If
                ElseIf child.Name.Equals("InstanceDescriptor") Then
                    value = ReadInstanceDescriptor2(child, errors)
                    Return (value IsNot Nothing)
                Else
                    errors.Add(String.Format("Неопределенный element type {0}", child.Name))
                    value = Nothing
                    Return False
                End If
            Next

            ' Если оказались здесь, значит нет узлов. Нет узлов и нет внутреннего текста в узле.
            ' Значит возвратить Nothing.
            value = Nothing
            Return True
        Catch ex As Exception
            errors.Add(ex.Message)
            value = Nothing
            Return False
        End Try
    End Function

    'Private Sub ReadEvent2(ByVal childNode As XmlNode, ByVal instance As Object, ByVal errors As ArrayList)
    '    'Dim bindings As IEventBindingService = TryCast(host.GetService(GetType(IEventBindingService)), IEventBindingService)

    '    'If bindings Is Nothing Then
    '    '    errors.Add("Невозможно присоединить сервис построителя события таким образом мы не можем создать какие-либо события")
    '    '    Return
    '    'End If

    '    Dim nameAttr As XmlAttribute = childNode.Attributes("name")

    '    If nameAttr Is Nothing Then
    '        errors.Add("Нет имени события")
    '        Return
    '    End If

    '    Dim methodAttr As XmlAttribute = childNode.Attributes("method")

    '    If methodAttr Is Nothing OrElse methodAttr.Value Is Nothing OrElse methodAttr.Value.Length = 0 Then
    '        errors.Add(String.Format("Событие {0} не имеет методов привязки к нему."))
    '        Return
    '    End If

    '    Dim evt As EventDescriptor = TypeDescriptor.GetEvents(instance)(nameAttr.Value)

    '    If evt Is Nothing Then
    '        errors.Add(String.Format("Событие {0} не существует на {1}", nameAttr.Value, instance.[GetType]().FullName))
    '        Return
    '    End If

    '    'Dim prop As PropertyDescriptor = bindings.GetEventProperty(evt)

    '    Debug.Assert(prop IsNot Nothing, "Плохое событие связывания")
    '    Try
    '        prop.SetValue(instance, methodAttr.Value)
    '    Catch ex As Exception
    '        errors.Add(ex.Message)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Чтение метода и его параметров
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="errors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadInstanceDescriptor2(ByVal node As XmlNode, ByVal errors As List(Of String)) As Object
        ' В начале необходимо десериализовать члены
        Dim memberAttr As XmlAttribute = node.Attributes("member")

        If memberAttr Is Nothing Then
            errors.Add("Не член аттрибута экземпляра дескриптора")
            Return Nothing
        End If

        Dim data As Byte() = Convert.FromBase64String(memberAttr.Value)
        Dim formatter As New BinaryFormatter()
        Dim stream As New MemoryStream(data)
        Dim mi As MemberInfo = DirectCast(formatter.Deserialize(stream), MemberInfo)
        Dim args As Object() = Nothing


        ' Просмотреть необходимы ли члену аргументы. Если так собрать их из XML.  
        If TypeOf mi Is MethodBase Then
            Dim paramInfos As ParameterInfo() = DirectCast(mi, MethodBase).GetParameters()

            args = New Object(paramInfos.Length - 1) {}

            Dim idx As Integer = 0

            For Each child As XmlNode In node.ChildNodes
                If child.Name.Equals("Argument") Then
                    Dim value As Object = Nothing

                    If Not ReadValue2(child, TypeDescriptor.GetConverter(paramInfos(idx).ParameterType), errors, value, Nothing) Then
                        Return Nothing
                    End If
                    args(idx) = value
                    idx += 1
                End If
            Next

            If idx <> paramInfos.Length Then
                errors.Add(String.Format("Член {0} требует {1} аргумент, не {2}.", mi.Name, args.Length, idx))
                Return Nothing
            End If
        End If

        Dim id As New InstanceDescriptor(mi, args)
        Dim instance As Object = id.Invoke()

        ' Ok, мы имеем объект. Сейчас просмотреть, есть ли любые свойства, и установлены ли они.
        For Each prop As XmlNode In node.ChildNodes
            If prop.Name.Equals("Property") Then
                ReadProperty2(prop, instance, errors)
            End If
        Next

        Return instance
    End Function
End Class


'<System.Diagnostics.DebuggerStepThrough()> _
'Private Sub InitializeComponent()
'    Me.ScatterGraph1 = New NationalInstruments.UI.WindowsForms.ScatterGraph
'    Me.XAxis1 = New NationalInstruments.UI.XAxis
'    Me.YAxis1 = New NationalInstruments.UI.YAxis
'    Me.ScatterPlot1 = New NationalInstruments.UI.ScatterPlot
'    CType(Me.ScatterGraph1, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.SuspendLayout()
'    '
'    'ScatterGraph1
'    '
'    Me.ScatterGraph1.Location = New System.Drawing.Point(12, 22)
'    Me.ScatterGraph1.Name = "ScatterGraph1"
'    Me.ScatterGraph1.Plots.AddRange(New NationalInstruments.UI.ScatterPlot() {Me.ScatterPlot1})
'    Me.ScatterGraph1.Size = New System.Drawing.Size(272, 168)
'    Me.ScatterGraph1.TabIndex = 0
'    Me.ScatterGraph1.XAxes.AddRange(New NationalInstruments.UI.XAxis() {Me.XAxis1})
'    Me.ScatterGraph1.YAxes.AddRange(New NationalInstruments.UI.YAxis() {Me.YAxis1})
'    '
'    'ScatterPlot1
'    '
'    Me.ScatterPlot1.XAxis = Me.XAxis1
'    Me.ScatterPlot1.YAxis = Me.YAxis1
'    '
'    'Form1
'    '
'    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
'    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'    Me.ClientSize = New System.Drawing.Size(292, 266)
'    Me.Controls.Add(Me.ScatterGraph1)
'    Me.Name = "Form1"
'    Me.Text = "Form1"
'    CType(Me.ScatterGraph1, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.ResumeLayout(False)

'End Sub
'Friend WithEvents ScatterGraph1 As NationalInstruments.UI.WindowsForms.ScatterGraph
'Friend WithEvents ScatterPlot1 As NationalInstruments.UI.ScatterPlot
'Friend WithEvents XAxis1 As NationalInstruments.UI.XAxis
'Friend WithEvents YAxis1 As NationalInstruments.UI.YAxis
