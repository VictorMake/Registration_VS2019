Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Xml.Linq
Imports System.Xml.XPath

Module ModuleXmlLinq
#Region "Extension"
    ''' <summary>
    ''' получить неглубокое значение элемента
    ''' </summary>
    ''' <param name="xe"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ShallowValue(ByVal xe As XElement) As Object
        Return xe _
               .Nodes() _
               .OfType(Of XText)() _
               .Aggregate(New StringBuilder(),
                              Function(s, c) s.Append(c),
                              Function(s) s.ToString())
    End Function

    ''' <summary>
    ''' новая последовательность элементов с разделителем
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="separator"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function StrCat(Of T)(ByVal source As IEnumerable(Of T), ByVal separator As String) As String
        Return source.Aggregate(New StringBuilder,
                                Function(sb, i) sb.
                                    Append(i.ToString()).
                                    Append(separator),
                                    Function(s) s.ToString())
    End Function

    ''' <summary>
    ''' Метод расширения, Find, применяется к XElement. 
    ''' Возвращает коллекцию объектов XAttribute и XElement, содержащих некоторый указанный текст.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Find(ByVal source As XElement, ByVal value As String) As IEnumerable(Of XObject)
        Dim results = From att In source.Attributes()
                      Where att.Value.Contains(value)
                      Let a As XObject = att
                      Select a

        If source.Elements().Any Then
            For Each result In From child In source.Elements() Select Find(child, value)
                results = If(results Is Nothing, result, results.Union(result))
            Next
        Else
            If source.Value.Contains(value) Then
                results = If(results Is Nothing,
                             New List(Of XObject) From {source},
                             results.Union(New List(Of XObject) From {source}))
            End If
        End If

        Return results
    End Function
#End Region

    ''' <summary>
    ''' Имя элемента
    ''' </summary>
    ''' <param name="xe"></param>
    ''' <returns></returns>
    Private Function GetQName(ByVal xe As XElement) As String
        Dim prefix = xe.GetPrefixOfNamespace(xe.Name.Namespace)
        If xe.Name.Namespace = XNamespace.None OrElse prefix Is Nothing Then
            Return xe.Name.LocalName
        Else
            Return prefix & ":" & xe.Name.LocalName
        End If
    End Function

    ''' <summary>
    ''' Имя аттрибута
    ''' </summary>
    ''' <param name="xa"></param>
    ''' <returns></returns>
    Private Function GetQName(ByVal xa As XAttribute) As String
        Dim prefix = xa.Parent.GetPrefixOfNamespace(xa.Name.Namespace)
        If xa.Name.Namespace = XNamespace.None OrElse prefix Is Nothing Then
            Return xa.Name.LocalName
        Else
            Return prefix & ":" & xa.Name.LocalName
        End If
    End Function

    ''' <summary>
    ''' Действует применительно к XObject и возвращает выражение XPath, которое после его вычисления возвращает узел или атрибут
    ''' </summary>
    ''' <param name="xobj"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function GetXPath(ByVal xobj As XObject) As String
        If xobj.Parent Is Nothing Then
            Dim doc = TryCast(xobj, XDocument)
            If doc IsNot Nothing Then Return "."

            Dim el = TryCast(xobj, XElement)
            If el IsNot Nothing Then Return "/" + NameWithPredicate(el)

            ' the XPath data model does not include white space text nodes  
            ' that are children of a document, so this method returns null.  

            Dim xt = TryCast(xobj, XText)
            If xt IsNot Nothing Then Return Nothing

            Dim com = TryCast(xobj, XComment)
            If com IsNot Nothing Then
                Return "/" &
                    If(com.Document.Nodes().OfType(Of XComment)().Count() <> 1,
                       "comment()[" & (com.NodesBeforeSelf().OfType(Of XComment)().Count() + 1) & "]",
                       "comment()")
            End If

            Dim pi = TryCast(xobj, XProcessingInstruction)
            If pi IsNot Nothing Then
                Return "/" &
                    If(pi.Document.Nodes().OfType(Of XProcessingInstruction)().Count() <> 1,
                       "processing-instruction()[" &
                           (pi.NodesBeforeSelf().OfType(Of XProcessingInstruction)().Count() + 1) & "]",
                       "processing-instruction()")
            End If
            Return Nothing
        Else
            Dim el = TryCast(xobj, XElement)
            If el IsNot Nothing Then
                Return "/" &
                    el.Ancestors().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") & NameWithPredicate(el)
            End If
            Dim at = TryCast(xobj, XAttribute)
            If at IsNot Nothing Then
                Return "/" &
                    at.Parent.
                    AncestorsAndSelf().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") & "@" & GetQName(at)
            End If
            Dim com = TryCast(xobj, XComment)
            If com IsNot Nothing Then
                Return "/" &
                    com.Parent.
                    AncestorsAndSelf().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") &
                        If(com.Parent.Nodes().OfType(Of XComment)().Count() <> 1,
                           "comment()[" & (com.NodesBeforeSelf().OfType(Of XComment)().Count() + 1) & "]",
                           "comment()")
            End If

            Dim cd = TryCast(xobj, XCData)
            If cd IsNot Nothing Then
                Return "/" &
                    cd.Parent.
                    AncestorsAndSelf().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") &
                        If(cd.Parent.Nodes().OfType(Of XText)().Count() <> 1,
                           "text()[" & (cd.NodesBeforeSelf().OfType(Of XText)().Count() + 1) & "]",
                           "text()")
            End If
            Dim tx = TryCast(xobj, XText)
            If tx IsNot Nothing Then
                Return "/" &
                    tx.Parent.
                    AncestorsAndSelf().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") &
                        If(tx.Parent.Nodes().OfType(Of XText)().Count() <> 1,
                           "text()[" & (tx.NodesBeforeSelf().OfType(Of XText)().Count() + 1) & "]",
                           "text()")
            End If
            Dim pi As XProcessingInstruction = TryCast(xobj, XProcessingInstruction)
            If pi IsNot Nothing Then
                Return "/" &
                    pi.Parent.
                    AncestorsAndSelf().
                    InDocumentOrder().
                    Select(Function(e) NameWithPredicate(e)).StrCat("/") &
                        If(pi.Parent.Nodes().OfType(Of XProcessingInstruction)().Count() <> 1,
                           "processing-instruction()[" &
                               (pi.NodesBeforeSelf().OfType(Of XProcessingInstruction)().Count() + 1) & "]",
                           "processing-instruction()")
            End If
            Return Nothing
        End If
    End Function

    Private Function NameWithPredicate(ByVal el As XElement) As String
        If el.Parent IsNot Nothing AndAlso
           el.Parent.Elements(el.Name).Count() <> 1 Then
            Return GetQName(el) & "[" &
                (el.ElementsBeforeSelf(el.Name).Count() + 1) & "]"
        Else
            Return GetQName(el)
        End If
    End Function

#Region "Поиск"
    ''' <summary>
    ''' Выдаёт значение элемента узла.
    ''' </summary>
    ''' <param name="root"></param>
    ''' <param name="childElement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetValueOfNode(root As XElement, childElement As String) As String
        Dim nodeText As String = String.Empty
        Dim c1 As IEnumerable(Of XElement) = From el In root.Elements(childElement)
                                             Select el

        For Each el As XElement In c1
            nodeText &= el.Value
        Next

        Return nodeText
    End Function

    ''' <summary>
    ''' Поиск аттрибута AttributeName в элементе root.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="root"></param>
    ''' <param name="childElement"></param>
    ''' <param name="AttributeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetValueOfAttribute(root As XElement, childElement As String, AttributeName As String) As String
        Dim nodeText As String = String.Empty
        Dim c1 As IEnumerable(Of XElement) = From el In root.Elements(childElement)
                                             Select el

        For Each el As XElement In c1
            If el.HasAttributes Then
                For Each e2 In el.Attributes
                    If e2.Name.LocalName = AttributeName Then
                        nodeText &= e2.Value
                    End If
                Next
            End If
        Next
        Return nodeText
    End Function

    ''' <summary>
    ''' Поиск аттрибута AttributeName в элементе root.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="root"></param>
    ''' <param name="attributeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetValueOfAttribute(root As XElement, attributeName As String) As String
        Dim NodeText As String = String.Empty

        If root.HasAttributes Then
            If root.Attributes(attributeName).Count > 0 Then
                Return root.Attributes(attributeName).First.Value
            End If
        End If
        Return NodeText
    End Function

    Private Sub ПолучитьНеглубокоеЗначениеЭлемента()
        '--- получить неглубокое значение элемента. Неглубокое значение - это значение только конкретного элемента, в отличие от глубокого значения, которое содержит значения всех элементов-потомков, объединенные в одной строке.
        Dim root As XElement = XElement.Load("Report.xml")

        Dim query As IEnumerable(Of XElement) =
            From el In root.Descendants()
            Where CStr(el.ShallowValue()).StartsWith("=")
            Select el

        For Each q As XElement In query
            Console.WriteLine("{0}{1}{2}", q.Name.ToString().PadRight(8), q.Attribute("Name").ToString().PadRight(20), q.ShallowValue())
        Next
    End Sub

    Private Sub НайтиЭлементСУказаннымАтрибутом()
        ' выполнить поиск элемента Address, имеющего атрибут Type со значением «Billing»
        Dim root As XElement = XElement.Load("PurchaseOrder.xml")
        Dim address As IEnumerable(Of XElement) =
            From el In root.<Address>
            Where el.@Type = "Billing"
            Select el
        For Each el As XElement In address
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub НайтиЭлементСУказаннымДочернимЭлементом()
        ' В этом примере осуществляется поиск элемента Test, имеющего дочерний элемент CommandLine со значением «Examp2.EXE».
        Dim root As XElement = XElement.Load("TestConfig.xml")
        Dim tests As IEnumerable(Of XElement) =
            From el In root.<Test>
            Where el.<CommandLine>.Value = "Examp2.EXE"
            Select el
        For Each el As XElement In tests
            Console.WriteLine(el.@TestId)
        Next
    End Sub

    Private Sub ПоискОдногоПотомкаСПомощьюМетодаDescendants()
        ' Метод оси Descendants можно использовать для быстрого написания кода с целью поиска одного уникально именованного элемента.
        ' Этот способ особенно полезен, если нужно найти конкретного потомка с заданным именем.
        ' Можно написать собственный код для перехода к нужному элементу, но часто быстрей и легче написать такой код с помощью оси Descendants.
        Dim root As XElement =
    <Root>
        <Child1>
            <GrandChild1>GC1 Value</GrandChild1>
        </Child1>
        <Child2>
            <GrandChild2>GC2 Value</GrandChild2>
        </Child2>
        <Child3>
            <GrandChild3>GC3 Value</GrandChild3>
        </Child3>
        <Child4>
            <GrandChild4>GC4 Value</GrandChild4>
        </Child4>
    </Root>
        Dim grandChild3 As String = CStr((From el In root...<GrandChild3>
                                          Select el).First())
        Console.WriteLine(grandChild3)
        'Этот код выводит следующие результаты: GC3 Value  
    End Sub

    Private Sub ПоискДочернегоЭлемента()
        Dim cpo As XDocument = XDocument.Load("PurchaseOrders.xml")
        Dim po As XElement = cpo.Root.<PurchaseOrder>.FirstOrDefault
        'Dim po As XElement = cpo.Root.Element("PurchaseOrder")
        ' LINQ to XML query  
        'Dim el1 As XElement = po.Element("DeliveryNotes")
        Dim el1 As XElement = po.<DeliveryNotes>.FirstOrDefault
        ' XPath expression
        Dim el2 As XElement = po.XPathSelectElement("DeliveryNotes")
        ' same as "child::DeliveryNotes"
        ' same as "./DeliveryNotes"

        'If el1.Equals(el2) Then
        If el1 Is el2 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        Console.WriteLine(el1)

        ' Узлы - это более глубокий уровень гранулярности, чем элементы и атрибуты.
        '--- конкретный элемент или атрибут какого-то элемента ----------------
        ' Возвращает первый дочерний элемент объекта XElement, имеющий указанный атрибут XName.
        ' если известно имя дочернего элемента, а также то, что есть только один элемент с таким именем, удобнее получить один элемент, а не целую коллекцию.
        ' Получает первый (в порядке следования документа) дочерний элемент с заданным Name
        Dim Root2 As XElement = XDocument.Load(PathXmlFileCondition).Root
        Dim el4 As XElement = Root2.Element("ИмяФормаПараметр")
        Dim e As XElement = Root2.<ИмяФормаПараметр>(0)

        '--- получения одного атрибута элемента при условии, что название атрибута известно. 
        'Это полезно для составления выражений запросов, при которых требуется найти элемент с определенным атрибутом.
        Dim atr As XAttribute = el4.Attribute("ИмяУсловия")
        Dim str As String = el4.@ИмяУсловия ' Предоставляет доступ к значению атрибута для XElement объекта или к первому элементу в коллекции 

        ' используется синтаксис угловой скобки для получения значения XML-атрибута с именем number-type, который не является допустимым идентификатором в Visual Basic.
        Dim phone3 As XElement = <phone number-type=" work">425-555-0145</phone>
        Console.WriteLine("Phone type: " & phone3.@<number-type>)
    End Sub

    Private Sub ПоискСпискаДочернихЭлементов()
        ' осуществляется поиск всех дочерних элементов элемента Address.
        Dim cpo As XDocument = XDocument.Load("PurchaseOrders.xml")
        'Dim po As XElement = cpo.Root.Element("PurchaseOrder").Element("Address")
        Dim po As XElement = cpo.Root.<PurchaseOrder>.<Address>.FirstOrDefault
        ' LINQ to XML query
        Dim list1 As IEnumerable(Of XElement) = po.Elements()
        ' XPath expression 
        Dim list2 As IEnumerable(Of XElement) = po.XPathSelectElements("./*")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        For Each el In list1
            Console.WriteLine(el)
        Next

        '--- поиск элемента --------------------------------------------------
        Dim Root As XElement = XDocument.Load(PathXmlFileCondition).Root

        Dim childElements As IEnumerable(Of XElement) =
                From el In Root.<ДочернееУсловиеN>
                Select el
        '--- Result set follows -----------------------------------------------
        For Each el As XElement In childElements
            'Console.WriteLine(el.Value)
        Next

        '--- как просматривать коллекцию атрибутов элемента. ---
        Dim listOfAttributes As IEnumerable(Of XAttribute) =
            From att In Root.Attributes()
            Select att
        For Each att As XAttribute In listOfAttributes
            Console.WriteLine(att)
        Next

        '--- СвойствоДочернейОсиXML -----------------------------------
        ' Предоставляет доступ к дочерним элементам одного из следующих: объекта XElement, объекта XDocument, коллекции объектов XElement или коллекции объектов XDocument.
        ' получить доступ к значению первого узла-потомка с именем name и значения всех узлов-потомков с именем phone из объекта contacts
        Dim contact As XElement =
            <contact>
                <name>Patrick Hines</name>
                <phone type="home">206-555-0144</phone>
                <phone type="work">425-555-0145</phone>
            </contact>

        Dim homePhone = From hp In contact.<phone>
                        Where contact.<phone>.@type = "home"
                        Select hp

        Console.WriteLine("Home Phone = {0}", homePhone(0).Value)
        ' Этот пример кода отображает следующий текст
        'Home Phone = 206 - 555 - 144
        ' Свойство-индексатор расширения
        ' Объект из указанного расположения в коллекции или Nothing, если индекс выходит за пределы допустимого диапазона.
        ' использовать индексатор расширений для доступа ко второму дочернему узлу в коллекции объектов XElement. 
        Console.WriteLine("Second phone number: " & contact.<phone>(1).Value)
        ' как получить значение атрибута XML из коллекции объектов XAttribute. В примере свойство оси атрибута используется для вывода значения атрибута type для всех элементов phone.
        Dim types = contact.<phone>.Attributes("type")

        For Each attr In types
            Console.WriteLine(attr.Value)
        Next
        'Этот пример кода отображает следующий текст:
        'home
        'work

        ' как получить доступ к дочерним узлам с именем phone из коллекции, возвращенной свойством дочерней оси contact объекта contacts
        Dim contacts As XElement =
            <contacts>
                <contact>
                    <name>Patrick Hines</name>
                    <phone type="home"> 206 - 555 - 144</phone>
                </contact>
                <contact>
                    <name>Lance Tucker</name>
                    <phone type="work"> 425 - 555 - 145</phone>
                </contact>
            </contacts>

        Dim homePhone2 = From contact2 In contacts.<contact>
                         Where contact2.<phone>.@type = "home"
                         Select contact2.<phone>

        Console.WriteLine("Home Phone = {0}", homePhone2(0).Value)
        ' Этот пример кода отображает следующий текст
        ' Home Phone = 206-555-0144


        '' Create a simple document and  write it to a file  
        'File.WriteAllText("Test.xml", "<Root>" + Environment.NewLine +
        '    "    <Child1>1</Child1>" + Environment.NewLine +
        '    "    <Child2>2</Child2>" + Environment.NewLine +
        '    "    <Child3>3</Child3>" + Environment.NewLine +
        '    "</Root>")

        'Console.WriteLine("Querying tree loaded with XDocument.Load")
        'Console.WriteLine("----")
        Dim doc As XDocument = XDocument.Load("Test.xml")
        Dim childList As IEnumerable(Of XElement) =
                From el In doc.Root.Elements()
                Select el
        For Each e As XElement In childList
            Console.WriteLine(e)
        Next

    End Sub

    Private Sub ПоискКорневогоЭлемента()
        ' поиск корневого элемента с помощью XPath и LINQ to XML.
        Dim po As XDocument = XDocument.Load("PurchaseOrders.xml")
        Dim el1 As XElement = po.Root
        Dim el2 As XElement = po.XPathSelectElement("/PurchaseOrders")

        If el1 Is el2 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        Console.WriteLine(el1.Name)
    End Sub

    Private Sub ПоискЭлементовПотомков()
        ' как возвращать элементы-потомки с определенным именем.
        Dim po As XDocument = XDocument.Load("PurchaseOrders.xml")

        ' LINQ to XML query  
        'Dim list1 As IEnumerable(Of XElement) = po.Root.Descendants("Name")
        Dim list1 As IEnumerable(Of XElement) = po...<Name>
        ' XPath expression  
        Dim list2 As IEnumerable(Of XElement) = po.XPathSelectElements("//Name")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        For Each el As XElement In list1
            Console.WriteLine(el)
        Next


        Dim Root2 As XElement = XDocument.Load(PathXmlFileCondition).Root
        Dim el4 As XElement = Root2.Element("ИмяФормаПараметр")
        Dim e As XElement = Root2.<ИмяФормаПараметр>(0)
        '--- поиск всех потомков в дереве с названием ДочернееУсловиеN, затем атрибута с названием ИмяУсловия.
        Dim elList = From el In el4...<ДочернееУсловиеN>
        For Each ep As XElement In elList
            Console.WriteLine(ep.@ИмяУсловия)
        Next

        '--- получить значение атрибута, можно его задать точно так же, как при работе с объектами XElement.
        Dim elList2 As IEnumerable(Of XElement) =
            From el In el4...<ДочернееУсловиеN>
            Select el
        For Each el As XElement In elList2
            Console.WriteLine(el.@ИмяУсловия)
        Next

        Dim c2 As Nullable(Of Integer) = CType(Root2.Element("Child2"), Nullable(Of Integer))
        Console.WriteLine("c2:{0}", IIf(Not (c2.HasValue), "element does not exist", c2.ToString()))
        '--- получение отфильтрованной коллекции потомков, в которой содержатся только потомки с указанным именем
        Dim items As IEnumerable(Of XElement) =
            From el In Root2...<ЧтоЗаУсловие>
            Select el
        For Each prdName As XElement In items
            Console.WriteLine(prdName.Name.ToString & ":" & prdName.Value)
        Next

        'с помощью методовXContainer.Elements и Extensions.Elements выполняется поиск всех элементов Name во всех элементах Address во всех элементах PurchaseOrder.
        Dim names As IEnumerable(Of XElement) =
            From el In Root2.<PurchaseOrder>.<Address>.<Name>
            Select el
        For Each cp As XElement In names
            Console.WriteLine(cp)
        Next
        '--- использовать ось Extensions.Elements следующим образом.
        Dim configParameters As IEnumerable(Of XElement) =
            Root2.<Customer>.<Config>.<ConfigParameter>
        For Each cp As XElement In configParameters
            Console.WriteLine(cp)
        Next
    End Sub

    Private Sub ПоискСвязанныхЭлементов()
        ' как возвращать элемент, выбирая атрибут, обращение к которому осуществляется с помощью значения другого элемента.
        ' Выражение XPath
        './/Customer[@CustomerID=/ Root / Orders / Order[12]/CustomerID]
        ' В этом примере обнаруживается 12-й элемент Order, а затем определяется клиент, сделавший этот заказ.
        ' Обратите внимание, что индексирование в списках .NET начинается с нуля. Индексирование в коллекции узлов в предикате XPath начинается с единицы. 
        ' Данное различие находит отражение в следующем примере.
        Dim co As XDocument = XDocument.Load("CustomersOrders.xml")
        '' LINQ to XML query
        'Dim customer1 As XElement = (From el In co.Descendants("Customer")
        '                             Where CStr(el.Attribute("CustomerID")) = CStr((co.Element("Root").Element("Orders").Elements("Order").ToList()(11).Element("CustomerID")))
        '                             Select el).First()
        '' An alternate way to write the query that avoids creation  
        '' of a System.Collections.Generic.List 
        'Dim customer2 As XElement = (From el In co.Descendants("Customer")
        '                             Where CStr(el.Attribute("CustomerID")) = CStr((co.Element("Root").Element("Orders").Elements("Order").Skip(11).First().Element("CustomerID")))
        '                             Select el).First()
        ' LINQ to XML query  
        Dim customer1 As XElement = (From el In co...<Customer>
                                     Where el.@CustomerID = co.<Root>.<Orders>.<Order>.
        ToList()(11).<CustomerID>(0).Value
                                     Select el).First()

        ' An alternate way to write the query that avoids creation  
        ' of a System.Collections.Generic.List:  
        Dim customer2 As XElement = (From el In co...<Customer>
                                     Where el.@CustomerID = co.<Root>.<Orders>.<Order>.
        Skip(11).First().<CustomerID>(0).Value
                                     Select el).First()
        ' XPath expression 
        Dim customer3 As XElement = co.XPathSelectElement(".//Customer[@CustomerID=/Root/Orders/Order[12]/CustomerID]")

        If customer1 Is customer2 AndAlso customer1 Is customer3 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        Console.WriteLine(customer1)
    End Sub

    Private Sub ПоискПредшествующихЭлементовТогоЖеУровня()
        ' сравнивается ось XPath preceding-sibling с дочерней для LINQ to XML осью XNode.ElementsBeforeSelf.
        ' Выражение XPath
        ' preceding-sibling: *
        ' Обратите внимание, что результаты как XPathSelectElements, так и XNode.ElementsBeforeSelf соответствуют структуре документа.
        ' В следующем примере находится элемент FullAddress, после чего при помощи оси preceding-sibling получаются предыдущие элементы.
        Dim co As XElement = XElement.Load("CustomersOrders.xml")
        'Dim add As XElement = co.Element("Customers").Element("Customer").Element("FullAddress")
        Dim add As XElement = co.<Customers>.<Customer>.<FullAddress>.FirstOrDefault
        'LINQ to XML query 
        Dim list1 As IEnumerable(Of XElement) = add.ElementsBeforeSelf()
        ' XPath expression  
        Dim list2 As IEnumerable(Of XElement) = add.XPathSelectElements("preceding-sibling::*")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")

        End If
        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub ПоискПотомковДочернегоЭлемента()
        ' как возвращать элементы-потомки дочерних элементов с определенным именем.
        ' Выражение XPath
        './Paragraph//Text/text()
        ' В этом примере имитируются проблемы извлечения текста из XML-представления документа текстового редактора.
        ' В нем сначала выделяются все элементы Paragraph, а затем в нем выделяются все элементы-потомки Text каждого элемента Paragraph.
        ' В этом примере элементы-потомки Text элемента Comment не выделяются.
        Dim root As XElement = XElement.Parse("
                                                <Root>
                                                  <Paragraph>  
                                                    <Text>This is the start of</Text>  
                                                  </Paragraph>  
                                                  <Comment>  
                                                    <Text>This comment is not part of the paragraph text.</Text>  
                                                  </Comment>  
                                                  <Paragraph>  
                                                    <Annotation Emphasis='true'>  
                                                      <Text> a sentence.</Text>  
                                                    </Annotation>  
                                                  </Paragraph>  
                                                  <Paragraph>  
                                                    <Text>  This is a second sentence.</Text>  
                                                  </Paragraph>  
                                                </Root>")
        ' LINQ to XML query
        'Dim str1 As String = root.Elements("Paragraph").Descendants("Text").Select(Function(s) s.Value).Aggregate(New StringBuilder(),
        '                                                                                                          Function(s, i) s.Append(i),
        '                                                                                                          Function(s) s.ToString())
        Dim str1 As String = root.<Paragraph>...<Text>.Select(Function(ByVal s) s.Value).Aggregate(New StringBuilder(),
                                                                                                   Function(ByVal s, ByVal i) s.Append(i),
                                                                                                   Function(ByVal s) s.ToString())
        ' XPath expression
        Dim str2 As String = (CType(root.XPathEvaluate("./Paragraph//Text/text()"), IEnumerable)).Cast(Of XText)().Select(Function(s) s.Value).Aggregate(New StringBuilder(),
                                                                                                                                                         Function(s, i) s.Append(i),
                                                                                                                                                         Function(s) s.ToString())

        If str1 = str2 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        Console.WriteLine(str2)
    End Sub

    Private Sub ПоискОдноуровневыхУзлов()
        ' требуется найти все одноуровневые элементы с указанным именем. 
        ' Полученная в результате коллекция может содержать контекстный узел, если этот контекстный узел также имеет указанное имя.
        ' Выражение XPath
        '../Book
        ' В этом примере вначале осуществляется поиск элемента Book, а затем всех одноуровневых элементов с именем Book. Полученная в результате коллекция содержит контекстный узел.
        Dim books As XDocument = XDocument.Load("Books.xml")
        'Dim book As XElement = books.Root.Elements("Book").Skip(1).First()
        Dim book As XElement = books.Root.<Book>.Skip(1).First()
        'LINQ to XML quer
        'Dim list1 As IEnumerable(Of XElement) = book.Parent.Elements("Book")
        Dim list1 As IEnumerable(Of XElement) = book.Parent.<Book>
        'XPath expression
        Dim list2 As IEnumerable(Of XElement) = book.XPathSelectElements("../Book")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")

        End If
        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

    Public Sub ПоискАтрибутаРодительскогоЭлемента()
        ' способ перехода к родительскому элементу и нахождения его атрибута.
        ' Выражение XPath
        '../@id
        ' Пример
        ' Сначала в данном примере осуществляется поиск элемента Author. Затем в нем осуществляется поиск атрибута id родительского элемента.
        Dim books As XDocument = XDocument.Load("Books.xml")
        'Dim author As XElement = books.Root.Element("Book").Element("Author")
        Dim author As XElement = books.Root.<Book>.<Author>.FirstOrDefault()
        ' LINQ to XML query
        Dim att1 As XAttribute = author.Parent.Attribute("id")
        ' XPath expression
        Dim att2 As XAttribute = (CType(author.XPathEvaluate("../@id"), IEnumerable)).Cast(Of XAttribute)().First()

        If att1 Is att2 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If
        Console.WriteLine(att1)
    End Sub

    Private Sub ПоискАтрибутовЭлементовТогоЖеУровняСОпределеннымИменем()
        ' как найти все атрибуты одноуровневых элементов контекстного узла. В коллекции возвращаются только атрибуты с заданным именем.
        ' Выражение XPath
        '../Book/@id
        ' В этом примере вначале происходит поиск элемента Book, затем всех одноуровневых элементов с именем Book, а после этого всех атрибутов с именем id. Результатом становится коллекция атрибутов.
        Dim books As XDocument = XDocument.Load("Books.xml")
        'Dim book As XElement = books.Root.Element("Book")
        Dim book As XElement = books.Root.<Book>(0)
        'LINQ to XML query 
        'Dim list1 As IEnumerable(Of XAttribute) =
        '    From el In book.Parent.Elements("Book")
        '    Select el.Attribute("id")
        Dim list1 As IEnumerable(Of XAttribute) =
            From el In book.Parent.<Book>
            Select el.Attribute("id")
        ' XPath expression
        Dim list2 As IEnumerable(Of XAttribute) = (CType(book.XPathEvaluate("../Book/@id"), IEnumerable)).Cast(Of XAttribute)

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If
        For Each el As XAttribute In list1
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub ПоискЭлементовСОпределеннымАтрибутом()
        Dim doc As XElement = XElement.Parse("<Root>  
    <Child1>1</Child1>  
    <Child2 Select='true'>2</Child2>  
    <Child3>3</Child3>  
    <Child4 Select='true'>4</Child4>  
    <Child5>5</Child5>  
</Root>")
        'LINQ to XML query
        'Dim list1 As IEnumerable(Of XElement) = From el In doc.Elements() Where el.Attribute("Select") IsNot Nothing Select el
        Dim list1 As IEnumerable(Of XElement) = From el In doc.Elements()
                                                Where el.@Select <> Nothing
                                                Select el
        'XPath expression 
        Dim list2 As IEnumerable(Of XElement) = (CType(doc.XPathEvaluate("./*[@Select]"), IEnumerable)).Cast(Of XElement)

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub ПоискДочернихЭлементовПоПоложению()
        ' требуется найти элементы на основании их позиции. Может понадобиться найти второй элемент или найти третий элемент через пятый.
        ' Выражение XPath
        'Test[position() >= 2 And position() <= 4]
        ' Существует два простых подхода к написанию этого запроса LINQ To XML. Можно использовать операторы Skip и Take или перегруженный оператор Where, который потребляет индекс. При использовании перегрузки оператора Where необходимо использовать лямбда-выражение, которое имеет два аргумента. Следующий пример показывает обе возможности выбора на основе позиции.
        ' В данном примере производится поиск второго элемента через четвертый элемент Test. Результатом является коллекция элементов.
        Dim testCfg As XElement = XElement.Load("TestConfig.xml")
        'LINQ to XML query 
        Dim list1 As IEnumerable(Of XElement) = testCfg.Elements("Test").Skip(1).Take(3)
        'LINQ to XML query
        Dim list2 As IEnumerable(Of XElement) = testCfg.Elements("Test").Where(Function(el, idx) idx >= 1 AndAlso idx <= 3)
        'XPath expression
        Dim list3 As IEnumerable(Of XElement) = testCfg.XPathSelectElements("Test[position() >= 2 and position() <= 4]")

        If list1.Count = list2.Count AndAlso list1.Count = list3.Count AndAlso list1.Intersect(list2).Count = list1.Count AndAlso list1.Intersect(list3).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If

        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub ПоискБлижайшегоПредшествующегоЭлементаТогоЖеУровня()
        ' требуется найти ближайший предшествующий одноуровневый элемент узла.
        ' Из - за разности в семантике позиционных предикатов для осей предшествующих одноуровневых элементов в XPath и в LINQ to XML это сравнение является одним из наиболее интересных.
        ' Пример
        ' В этом примере в запросе LINQ To XML оператор Last используется для обнаружения последнего узла в коллекции, возвращенной методом ElementsBeforeSelf.
        ' В отличие от этого, в выражении XPath для обнаружения ближайшего предшествующего элемента используется предикат со значением 1.
        Dim root As XElement = XElement.Parse("<Root>
                                                <Child1/>  
                                                <Child2/>  
                                                <Child3/>  
                                                <Child4/>  
                                            </Root>")
        Dim child4 As XElement = root.Element("Child4")
        ' LINQ to XML query 
        Dim el1 As XElement = child4.ElementsBeforeSelf().Last()
        'XPath expression
        Dim el2 As XElement = (CType(child4.XPathEvaluate("preceding-sibling::*[1]"), IEnumerable)).Cast(Of XElement)().First()

        If el1 Is el2 Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")
        End If
        Console.WriteLine(el1)
    End Sub

    Private Sub ПолучатьЭлементыПотомкиСУказаннымИменемИАтрибутСЗаданнымЗначением()
        ' эквивалентное выражение XPath.
        ' //Address[@Type='Shipping']

        Dim po = XDocument.Load("PurchaseOrders.xml")

        Dim list1 = From el In po.Descendants("Address")
                    Where el.@Type = "Shipping"

        For Each el In list1
            Console.WriteLine(el)
        Next
        ' Компилятор переписывает выражение запроса из этого примера с использованием синтаксиса запросов, основанных на методах.
        ' Код в следующем примере имеет синтаксис запросов, основанных на методах и приводит к получению таких же результатов, что и предыдущий код.
        Dim list2 As IEnumerable(Of XElement) = po.Descendants("Address").Where(Function(el) el.@Type = "Shipping")
        'Метод Where является методом расширения. Дополнительные сведения см. в статье Методы расширения.
        ' Поскольку используется метод расширения Where, представленный выше запрос компилируется так, как показано далее.
        Dim list3 = Enumerable.Where(po.Descendants("Address"), Function(el) el.@Type = "Shipping")
        ' Этот пример приводит к получению в точности таких же результатов, что и два предыдущих примера.
        ' Таким образом, демонстрируется возможность эффективной компиляции запросов в вызовы методов со статическими ссылками.
        ' В сочетании с семантикой отложенного выполнения итераторов это позволяет повысить производительность.
    End Sub

    Private Sub ПоискОбъединенияДвухПутейРасположения()
        ' XPath позволяет найти объединение результатов определения двух путей доступа XPath.
        ' Выражение XPath: 
        '//Category|//Price
        ' Те же результаты можно получить с помощью стандартного оператора запроса Concat.
        ' Пример
        ' В этом примере осуществляется поиск всех элементов Category и всех элементов Price и объединение их в одну коллекцию. 
        ' Обратите внимание, что в запросе LINQ To XML вызывается метод InDocumentOrder для упорядочения результатов.
        ' Эти результаты вычисления выражения XPath также представлены в той же последовательности, что и в документе.
        Dim data As XDocument = XDocument.Load("Data.xml")
        'LINQ to XML query
        'Dim list1 As IEnumerable(Of XElement) = data.Descendants("Category").Concat(data.Descendants("Price")).InDocumentOrder()
        Dim list1 As IEnumerable(Of XElement) = data...<Category>.Concat(data...<Price>).InDocumentOrder()
        'XPath expression
        Dim list2 As IEnumerable(Of XElement) = data.XPathSelectElements("//Category|//Price")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")

        End If
        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

    Private Sub МетодОси_LINQ_to_XML()
        ' Можно написать свои собственные методы оси для получения коллекций из XML-дерева. Один из лучших способов выполнения этого состоит в написании метода расширения, 
        ' возвращающего коллекцию элементов или атрибутов. 
        ' Метод расширения можно написать для возврата указанных поднаборов элементов или атрибутов с учетом требований приложения.
        ' В следующем примере используются два метода расширения. 
        ' Первый метод расширения, GetXPath, действует применительно к XObject и возвращает выражение XPath, которое после его вычисления возвращает узел или атрибут. 
        ' Второй метод расширения, Find, применяется к XElement. Он возвращает коллекцию объектов XAttribute и XElement, содержащих некоторый указанный текст.
        ' В этом примере используется следующий XML-документ: Пример Xml - файла.Несколько заказов на покупку (LINQ to XML).

        Dim purchaseOrders = XElement.Load("..\..\PurchaseOrders.xml")

        Dim subset = From xobj In purchaseOrders.Find("1999")

        For Each obj In subset
            Console.WriteLine(obj.GetXPath())
            If obj.GetType() = GetType(XElement) Then
                Console.WriteLine(CType(obj, XElement).Value)
            ElseIf obj.GetType() = GetType(XAttribute) Then
                Console.WriteLine(CType(obj, XAttribute).Value)
            End If
        Next
        ' Этот код выводит следующие результаты:
        '/PurchaseOrders/PurchaseOrder[1]/@OrderDate  
        '1999-10-20  
        '/PurchaseOrders/PurchaseOrder[1]/Items/Item[2]/ShipDate  
        '1999-05-21  
        '/PurchaseOrders/PurchaseOrder[2]/@OrderDate  
        '1999-10-22  
        '/PurchaseOrders/PurchaseOrder[3]/@OrderDate  
        '1999-10-22  
    End Sub

#End Region

#Region "Фильтрация"
    Private Sub ЗапросыСоСложнойФильтрацией()
        ' Иногда возникает необходимость в написании запросов LINQ to XML с комплексной фильтрацией.
        ' Например, может потребоваться найти все элементы, имеющие дочерние элементы с определенным именем и значением.
        ' В этом разделе приводится пример написания запроса с комплексной фильтрацией.

        'В этом примере показано, как найти все элементы PurchaseOrder, имеющие дочерний элемент Address с атрибутом Type, равным «Доставка»,
        ' и дочерним элементом State, равным «NY».
        ' В нем используется вложенный запрос в предложении Where, а оператор Any возвращает значение True, если коллекция содержит элементы.
        Dim root As XElement = XElement.Load("PurchaseOrders.xml")
        Dim purchaseOrders As IEnumerable(Of XElement) =
            From el In root.<PurchaseOrder>
            Where
                (From add In el.<Address>
                 Where add.@Type = "Shipping" AndAlso add.<State>.Value = "NY"
                 Select add).Any()
            Select el
        For Each el As XElement In purchaseOrders
            Console.WriteLine(el.@PurchaseOrderNumber)
        Next
    End Sub

    Private Sub ФильтрацияПоНеобязательномуЭлементу()
        ' Иногда необходимо выполнить фильтрацию элемента, даже если неизвестно, существует ли он в документе XML.
        ' Поиск должен быть выполнен, чтобы, если конкретный элемент не имеет дочернего узла,
        ' при фильтрации по этому элементу не возникло исключение null reference.
        ' В следующем примере элемент Child5 не имеет дочернего узла Type, тем не менее запрос выполняется правильно.
        ' Этот пример использует метод расширений Elements.
        Dim root As XElement =
            <Root>
                <Child1>
                    <Text>Child One Text</Text>
                    <Type Value="Yes"/>
                </Child1>
                <Child2>
                    <Text>Child Two Text</Text>
                    <Type Value="Yes"/>
                </Child2>
                <Child3>
                    <Text>Child Three Text</Text>
                    <Type Value="No"/>
                </Child3>
                <Child4>
                    <Text>Child Four Text</Text>
                    <Type Value="Yes"/>
                </Child4>
                <Child5>
                    <Text>Child Five Text</Text>
                </Child5>
            </Root>
        Dim cList As IEnumerable(Of String) =
            From typeElement In root.Elements().<Type>
            Where typeElement.@Value = "Yes"
            Select typeElement.Parent.<Text>.Value
        Dim str As String
        For Each str In cList
            Console.WriteLine(str)
        Next
        'Этот код выводит следующие результаты:
        '        Child One Text  
        'Child Two Text  
        'Child Four Text 
    End Sub

    Private Sub НаходитЭлементыНаОсновеКонтекста()
        ' Иногда требуется написать запрос, который выбирает элементы, исходя из их контекста.
        ' Может потребоваться использовать фильтрацию с учетом предыдущих или следующих одноуровневых элементов.
        ' Может потребоваться использовать фильтрацию с учетом дочерних или родительских элементов.
        ' Это можно сделать, написав запрос и используя результаты запроса в предложении where.
        ' Если требуется сначала провести проверку на наличие значения null, а затем проверить само значение,
        ' более удобным будет выполнить запрос в предложении Let, а затем использовать результаты в предложении where.
        ' В следующем примере выбираются все элементы p, сразу за которыми следует элемент ul.
        Dim doc As XElement =
    <Root>
        <p id='1'/>
        <ul>abc</ul>
        <Child>
            <p id='2'/>
            <notul/>
            <p id='3'/>
            <ul>def</ul>
            <p id='4'/>
        </Child>
        <Child>
            <p id='5'/>
            <notul/>
            <p id='6'/>
            <ul>abc</ul>
            <p id='7'/>
        </Child>
    </Root>

        Dim items As IEnumerable(Of XElement) =
            From e In doc...<p>
            Let z = e.ElementsAfterSelf().FirstOrDefault()
            Where z IsNot Nothing AndAlso z.Name.LocalName = "ul"
            Select e

        For Each e As XElement In items
            Console.WriteLine("id = {0}", e.@<id>)
        Next
        ' Этот код выводит следующие результаты:
        'id = 1
        'id = 3
        'id = 6
    End Sub

    Private Sub ФильтрацияПоАтрибуту()
        'как получать элементы-потомки с указанным именем и атрибут с заданным значением.
        'Выражение XPath
        './/Address[@Type='Shipping']
        'В этом примере обнаруживаются все элементы-потомки с именем Address и с атрибутом Type, имеющим значение «Доставка».
        Dim po As XDocument = XDocument.Load("PurchaseOrders.xml")
        'Dim list1 As IEnumerable(Of XElement) =
        '    From el In po.Descendants("Address")
        '    Where CStr(el.Attribute("Type")) = "Shipping"
        '    Select el
        Dim list1 As IEnumerable(Of XElement) =
            From el In po...<Address>
            Where el.@Type = "Shipping"
            Select el
        Dim list2 As IEnumerable(Of XElement) = po.XPathSelectElements(".//Address[@Type='Shipping']")

        If list1.Count = list2.Count AndAlso list1.Intersect(list2).Count = list1.Count Then
            Console.WriteLine("Results are identical")
        Else
            Console.WriteLine("Results differ")

        End If
        For Each el As XElement In list1
            Console.WriteLine(el)
        Next
    End Sub

#End Region

#Region "Проецирование на объект"
    Private Class NameQty
        Public name As String
        Public qty As Integer
        Public Sub New(ByVal n As String, ByVal q As Integer)
            name = n
            qty = q
        End Sub
    End Class
    Private Sub ПроецированиеНовогоТипа()
        ' запросы, возвращающие результаты в виде значений IEnumerable<T> типа XElement,
        ' значений IEnumerable<T> типа String и значений IEnumerable<T> типа int. Это наиболее распространенные типы результатов, но подходят не для всех сценариев.
        ' Во многих случаях требуется, чтобы запросы возвращали IEnumerable<T> какого-то другого типа.
        ' В данном примере показано, как создавать экземпляры объектов в предложении Select. Сначала в коде определяется новый класс с помощью конструктора,
        ' а затем модифицируется инструкция Select, чтобы это выражение представляло новый экземпляр нового класса.
        Dim po As XElement = XElement.Load("PurchaseOrder.xml")

        Dim nqList As IEnumerable(Of NameQty) =
            From n In po...<Item>
            Select New NameQty(n.<ProductName>.Value, CInt(n.<Quantity>.Value))

        For Each n As NameQty In nqList
            Console.WriteLine(n.name & ":" & n.qty)
        Next
        ' В этом примере выводятся следующие данные:
        'Lawnmower:1  
        'Baby Monitor: 2  
    End Sub

    Class Address
        Public Enum AddressUse
            Shipping
            Billing
        End Enum

        Public Property AddressType() As AddressUse
        Public Property Name() As String
        Public Property Street() As String
        Public Property City() As String
        Public Property State() As String
        Public Property Zip() As String
        Public Property Country() As String

        Public Overrides Function ToString() As String
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append(String.Format("Type: {0}" + vbNewLine,
            IIf(AddressType = AddressUse.Shipping, "Shipping", "Billing")))
            sb.Append(String.Format("Name: {0}" + vbNewLine, Name))
            sb.Append(String.Format("Street: {0}" + vbNewLine, Street))
            sb.Append(String.Format("City: {0}" + vbNewLine, City))
            sb.Append(String.Format("State: {0}" + vbNewLine, State))
            sb.Append(String.Format("Zip: {0}" + vbNewLine, Zip))
            sb.Append(String.Format("Country: {0}" + vbNewLine, Country))
            Return sb.ToString()
        End Function
    End Class
    Class PurchaseOrderItem
        Public Property PartNumber() As String
        Public Property ProductName() As String
        Public Property Quantity() As Integer
        Public Property USPrice() As Decimal
        Public Property Comment() As String
        Public Property ShipDate() As DateTime

        Public Overrides Function ToString() As String
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append(String.Format("PartNumber: {0}" + vbNewLine, PartNumber))
            sb.Append(String.Format("ProductName: {0}" + vbNewLine, ProductName))
            sb.Append(String.Format("Quantity: {0}" + vbNewLine, Quantity))
            sb.Append(String.Format("USPrice: {0}" + vbNewLine, USPrice))
            If (Comment <> Nothing) Then
                sb.Append(String.Format("Comment: {0}" + vbNewLine, Comment))
            End If
            If (ShipDate <> DateTime.MinValue) Then
                sb.Append(String.Format("ShipDate: {0:d}" + vbNewLine, ShipDate))
            End If
            Return sb.ToString()
        End Function
    End Class
    Class PurchaseOrder
        Public Property PurchaseOrderNumber() As String
        Public Property OrderDate() As DateTime
        Public Property Comment() As String
        Public Property Addresses() As List(Of Address)
        Public Property Items() As List(Of PurchaseOrderItem)

        Public Overrides Function ToString() As String
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append(String.Format("PurchaseOrderNumber: {0}" _
                    + vbNewLine, PurchaseOrderNumber))
            sb.Append(String.Format("OrderDate: {0:d}" + vbNewLine, OrderDate))
            sb.Append(vbNewLine)
            sb.Append("Addresses" + vbNewLine)
            sb.Append("=====" + vbNewLine)
            For Each address As Address In Addresses
                sb.Append(address)
                sb.Append(vbNewLine)
            Next
            sb.Append("Items" + vbNewLine)
            sb.Append("=====" + vbNewLine)
            For Each item As PurchaseOrderItem In Items
                sb.Append(item)
                sb.Append(vbNewLine)
            Next
            Return sb.ToString()
        End Function
    End Class

    Private Sub ПроецированиеГрафаОбъектов()
        ' способ проецирования, или наполнения, из XML графа объектов.
        ' В следующем коде происходит заполнение графа объектов классами Address, PurchaseOrder и PurchaseOrderItem 
        ' из XML-документа Пример XML-файла. Стандартный заказ на покупку (LINQ to XML).

        Dim po As XElement = XElement.Load("PurchaseOrder.xml")
        Dim purchOrder = New PurchaseOrder With {
            .PurchaseOrderNumber = po.@<PurchaseOrderNumber>,
            .OrderDate = Convert.ToDateTime(po.@<OrderDate>),
            .Addresses = (
                From a In po.<Address>
                Select New Address With {
                    .AddressType = CType(IIf(a.@<Type> = "Shipping",
                                       Address.AddressUse.Shipping,
                                       Address.AddressUse.Billing), Address.AddressUse),
                    .Name = a.<Name>.Value,
                    .Street = a.<Street>.Value,
                    .City = a.<City>.Value,
                    .State = a.<State>.Value,
                    .Zip = a.<Zip>.Value,
                    .Country = a.<Country>.Value
                    }
                ).ToList(),
            .Items = (
                From i In po.<Items>.<Item>
                Select New PurchaseOrderItem With {
                    .PartNumber = i.@<PartNumber>,
                    .ProductName = i.<ProductName>.Value,
                    .Quantity = CInt(i.<Quantity>.Value),
                    .USPrice = CDec(i.<USPrice>.Value),
                    .Comment = i.<Comment>.Value,
                    .ShipDate = CDate(IIf(i.<ShipDate>.Value <> Nothing,
                                Convert.ToDateTime(i.<ShipDate>.Value),
                                DateTime.MinValue))
                    }
                ).ToList()
        }
        Console.WriteLine(purchOrder)
    End Sub

    Private Sub ПроецированиеАнонимногоТипа()
        ' В некоторых случаях может потребоваться проецировать запрос на новый тип, даже если известно, что этот тип будет использоваться недолго. 
        ' Создание нового типа для использования в проекции требует много дополнительной работы. Гораздо более эффективный подход заключается в проецировании на анонимный тип.
        ' Анонимные типы позволяют определять класс и инициализировать его объект, не присваивая имя этому классу.
        ' Анонимные типы являются реализацией в языке C# математического понятия кортежа.
        ' Математический термин «кортеж» обозначает одноэлементные, двухэлементные, трехэлементные, четырехэлементные, пятиэлементные и n-элементные последовательности.
        ' Он относится к конечной последовательности объектов, каждый из которых имеет конкретный тип. Иногда такую последовательность называют списком пар «имя/значение».
        ' Например, содержимое адреса в XML-документе Пример XML-файла. Стандартный заказ на покупку (LINQ to XML) можно представить следующим образом.

        'Name:   Ellen Adams  
        'Street: 123 Maple Street  
        'City:   Mill Valley  
        'State:  CA
        'Zip: 90952  

        'Создание экземпляра анонимного типа удобно трактовать как создание n-элементного кортежа.
        ' При написании запроса, создающего кортеж в предложении Select, запрос возвращает объект IEnumerable кортежа.
        'В этом примере предложение Select проецирует анонимный тип. Затем в этом примере используется тип Dim для создания объекта IEnumerable.
        ' В цикле For Each переменная итерации становится экземпляром анонимного типа, созданного в выражении запроса.
        Dim custOrd As XElement = XElement.Load("CustomersOrders.xml")
        Dim custList =
            From el In custOrd.<Customers>.<Customer>
            Select New With {
                .CustomerID = el.@<CustomerID>,
                .CompanyName = el.<CompanyName>.Value,
                .ContactName = el.<ContactName>.Value
            }
        For Each cust In custList
            Console.WriteLine("{0}:{1}:{2}", cust.CustomerID, cust.CompanyName, cust.ContactName)
        Next

        ' Этот код выводит следующие результаты:
        'GREAL:  Great Lakes Food Market: Howard Snyder  
        'HUNGC:  Hungry Coyote Import Store: Yoshi Latimer  
        'LAZYK:  Lazy K Kountry Store: John Steel  
        'LETSS:Let's Stop N Shop:Jaime Yorres 
    End Sub
#End Region

#Region "Иерархии"
    Private Sub СозданиеИерархииСПомощьюГруппирования()
        ' как группировать данные и затем создавать код XML на основе группирования.
        ' В этом примере сначала данные группируются по категориям, а затем создается новый XML-файл, в котором XML-иерархия отражает группирование.

        Dim doc As XElement = XElement.Load("Data.xml")
        Dim newData As XElement =
            <Root>
                <%=
                    From data In doc.<Data>
                    Group By category = data.<Category>(0).Value
                    Into groupedData = Group
                    Select <Group ID=<%= category %>>
                               <%=
                                   From g In groupedData
                                   Select
                                   <Data>
                                       <%= g.<Quantity>(0) %>
                                       <%= g.<Price>(0) %>
                                   </Data>
                               %>
                           </Group>
                %>
            </Root>
        Console.WriteLine(newData)
        ' В этом примере выводятся следующие данные:
        '<Root>  
        '  <Group ID = "A" >
        '    <Data>
        '        <Quantity>3</Quantity>
        '        <Price>24.50</Price>
        '    </Data>  
        '    <Data>  
        '      <Quantity>5</Quantity>  
        '      <Price>4.95</Price>  
        '    </Data>  
        '    <Data>  
        '      <Quantity>3</Quantity>  
        '      <Price>66.00</Price>  
        '    </Data>  
        '    <Data>  
        '      <Quantity>15</Quantity>  
        '      <Price>29.00</Price>  
        '    </Data>  
        '  </Group>  
        '  <Group ID = "B" >
        '    <Data>
        '        <Quantity>1</Quantity>
        '        <Price>89.99</Price>
        '    </Data>  
        '    <Data>  
        '      <Quantity>10</Quantity>  
        '      <Price>.99</Price>  
        '    </Data>  
        '    <Data>  
        '      <Quantity>8</Quantity>  
        '      <Price>6.99</Price>  
        '    </Data>  
        '  </Group>  
        '</Root>  
    End Sub

    Private Sub ВывестиСписокВсехУзловВДереве()
        ' Иногда нужно вывести список всех узлов дерева. Это полезно, если требуется узнать, как метод или свойство повлияли на дерево. 
        ' Одним из подходов для вывода списка всех узлов в текстовом формате является создание выражения XPath, которое точно определяет каждый конкретный узел дерева.
        ' Не слишком удобно выполнять выражения XPath с помощью LINQ To XML. Выражения XPath имеют более низкую производительность, 
        ' чем запросы LINQ To XML, к тому же запросы LINQ to XML являются более мощными. Однако для определения узлов в XML-дереве выражения XPath подходят хорошо.

        ' В этом примере показана функция с именем GetXPath, создающая конкретное выражение XPath для каждого узла XML-дерева.
        ' Она формирует соответствующие выражения XPath, даже если узлы находятся в пространстве имен. Выражения XPath создаются с помощью префиксов пространства имен.
        ' Затем пример создает небольшое XML-дерево, содержащее несколько типов узлов. После этого он проходит по узлам-потомкам и выводит выражение XPath для каждого узла.
        ' Обратите внимание, что XML-декларация не является узлом дерева.
        ' Следующий Xml - файл содержит несколько типов узлов.
        '<?xml version="1.0" encoding="utf-8" standalone="yes"?>  
        '<?target data?>  
        '<Root AttName = "An Attribute" xmlns:aw = "http://www.adventure-works.com" >
        '        <!--This is a comment-->  
        '  <Child>Text</Child>  
        '  <Child>Other Text</Child>  
        '  <ChildWithMixedContent>text<b>BoldText</b>otherText</ChildWithMixedContent>  
        '  <aw:ElementInNamespace>
        '    <aw:ChildInNamespace/>  
        '  </aw:ElementInNamespace>
        '</Root>  

        ' Ниже приводится список узлов в XML-дереве, представленном в виде выражений XPath.
        '/processing-instruction
        '/Root  
        '/Root/@AttName  
        '/Root/@xmlns:aw
        '/Root/comment()  
        '/Root/Child[1]  
        '/Root/Child[1]/text()  
        '/Root/Child[2]  
        '/Root/Child[2]/text()  
        '/Root/ChildWithMixedContent  
        '/Root/ChildWithMixedContent/text()[1]  
        '/Root/ChildWithMixedContent/b  
        '/Root/ChildWithMixedContent/b/text()  
        '/Root/ChildWithMixedContent/text()[2]  
        '/Root/aw:ElementInNamespace
        '/Root/aw:ElementInNamespace/ aw: ChildInNamespace

        Dim aw As XNamespace = "http://www.adventure-works.com"
        Dim doc As XDocument =
            <?xml version='1.0' encoding="utf-8" standalone='yes'?>
            <?target data?>
            <Root AttName='An Attribute' xmlns:aw='http://www.adventure-works.com'>
                <!--This is a comment-->
                <Child>Text</Child>
                <Child>Other Text</Child>
                <ChildWithMixedContent>text<b>BoldText</b>otherText</ChildWithMixedContent>
                <aw:ElementInNamespace>
                    <aw:ChildInNamespace/>
                </aw:ElementInNamespace>
            </Root>
        doc.Save("Test.xml")
        Console.WriteLine(File.ReadAllText("Test.xml"))
        Console.WriteLine("------")

        For Each obj As XObject In doc.DescendantNodes()
            Console.WriteLine(obj.GetXPath())
            Dim el As XElement = TryCast(obj, XElement)
            If el IsNot Nothing Then
                For Each at As XAttribute In el.Attributes()
                    Console.WriteLine(at.GetXPath())
                Next
            End If
        Next
    End Sub

    Private Sub ЗаполнитьДеревоXMLИзФайловойСистемы()
        ' Распространенным и полезным применением XML-деревьев является использование их в качестве иерархической структуры для хранения данных с именем и значением. 
        ' Можно заполнить дерево XML-данными, распределенными внутри иерархии, после чего выполнять по нему запросы, преобразования и, если необходимо, сериализацию.
        ' В следующем сценарии многие виды семантических конструкций, присущих XML, например пространства имен и обработка пробельных символов, неважны.
        ' Вместо этого XML-дерево используется как небольшая иерархическая база данных для одного пользователя, которая находится в памяти.
        ' В следующем примере происходит заполнение XML-дерева из локальной файловой системы при помощи рекурсии.
        ' Затем выполняется запрос по дереву и вычисляется общий размер всех файлов в дереве.
        Dim fileSystemTree As XElement = CreateFileSystemXmlTree("C:/Tmp")
        Console.WriteLine(fileSystemTree)
        Console.WriteLine("------")
        Dim totalFileSize As Long =
            (From f In fileSystemTree...<File>
             Select CLng(f.<Length>(0))
            ).Sum()
        Console.WriteLine("Total File Size:{0}", totalFileSize)
    End Sub

    Function CreateFileSystemXmlTree(ByVal source As String) As XElement
        Dim di As DirectoryInfo = New DirectoryInfo(source)
        Return <Dir Name=<%= di.Name %>>
                   <%= From d In Directory.GetDirectories(source)
                       Select CreateFileSystemXmlTree(d) %>
                   <%= From fi In di.GetFiles()
                       Select <File>
                                  <Name><%= fi.Name %></Name>
                                  <Length><%= fi.Length %></Length>
                              </File> %>
               </Dir>
    End Function
#End Region

#Region "Изменение"
    Private Sub УдалитьСуществующийЭлемент()
        Dim doc As XDocument = XDocument.Load(PathXmlFileCondition)
        '--- удалить существующий элемент -------------------------------------
        Dim Root2 As XElement = doc.Root
        Root2.Elements.Where(Function(el) CStr(el.FirstAttribute) = "dsdsd").First.Remove()
    End Sub

    Private Sub УдалениеЭлементов()
        Dim root As XElement =
    <Root>
        <A>1</A>
        <B>2</B>
        <C>3</C>
    </Root>
        For Each e As XElement In root.Elements().ToList()
            e.Remove()
        Next
        Console.WriteLine(root)
        ' Кроме того, можно совсем исключить итерацию за счет вызова RemoveAll на родительском элементе.
        root.RemoveAll()
    End Sub

    Private Sub ИзменениеXMLДереваПроцедурныйКод()
        ' Преобразование атрибутов в элементы
        ' Для этого примера предположим, что требуется изменить следующий образец XML-документа, чтобы атрибуты стали элементами.
        ' Сначала в этом разделе представлен обычный способ изменения на месте. Затем показан способ функционального построения.
        '<?xml version="1.0" encoding="utf-8" ?>  
        '<Root Data1 = "123" Data2="456">  
        '  <Child1>Content</Child1>  
        '</Root>  
        Dim root As XElement = XElement.Load("Data.xml")
        For Each att As XAttribute In root.Attributes()
            root.Add(New XElement(att.Name, att.Value))
        Next
        root.Attributes().Remove()
        Console.WriteLine(root)
        'Этот код выводит следующие результаты:
        '<Root>  
        '  <Child1>Content</Child1>  
        '  <Data1>123</Data1>  
        '  <Data2>456</Data2>  
        '</Root> 
    End Sub

    Private Sub ИзменениеXMLДереваФункциональныйКод()
        ' В отличие от этого, функциональный подход основан на применении кода, предназначенного для формирования нового дерева, 
        ' выбора и извлечения элементов и атрибутов из исходного дерева и их соответствующего преобразования по мере добавления в новое дерево. 
        ' Пример функционального подхода выглядит примерно так:
        Dim root As XElement = XElement.Load("Data.xml")
        Dim newTree As XElement =
            <Root>
                <%= root.<Child1> %>
                <%= From att In root.Attributes()
                    Select New XElement(att.Name, att.Value) %>
            </Root>
        Console.WriteLine(newTree)
        'В этом примере формируется такой же итоговый XML-документ, как и в первом примере.
        ' Но обратите внимание, что при функциональном подходе можно видеть итоговую структуру нового XML-документа.
        ' Можно видеть создание элемента Root, код, который получает по запросу элемент Child1 из исходного дерева,
        ' а также код, который преобразует атрибуты из исходного дерева в элементы в новом дереве.

        '--- Использование внедренных выражений в содержимом ------------------
        Dim nameParameter As String = "Параметр отсутствует"
        Dim ИмяФормаПараметр As XElement = <ИмяФормаПараметр><%= nameParameter %></ИмяФормаПараметр>

        '--- внедренное выражение для вставки элементов в дерево --------------
        Dim xmlTree1 As XElement = <Root>
                                       <Child>Contents</Child>
                                   </Root>
        Dim xmlTree2 As XElement = <Root>
                                       <%= xmlTree1.<Child> %>
                                   </Root>

        '--- Использование запросов LINQ во внедренном выражении --------------
        Dim arr As Integer() = {1, 2, 3}

        Dim n As XElement =
            <Root>
                <%= From i In arr
                    Select <Child><%= i %></Child> %>
            </Root>

        '--- Использование внедренных выражений для создания имен узлов -------
        Dim eleName As String = "ele"
        Dim attName As String = "att"
        Dim attValue As String = "aValue"
        Dim eleValue As String = "eValue"
        Dim n2 As XElement =
            <Root <%= attName %>=<%= attValue %>>
                <<%= eleName %>>
                    <%= eleValue %>
                </>
            </Root>

        '<Root att = "aValue">
        '  <ele>eValue</ele>
        '</Root> 

        ' для установки значения атрибута можно использовать встроенное свойство атрибута. 
        ' Далее в случае использования встроенного свойства атрибута для установки значения несуществующего атрибута этот атрибут будет создан.
        XDocument.Load(PathXmlFileCondition).Root.<ИмяФормаПараметр>.<ЧтоЗаУсловие>.@Примечание = "new content"

        ' создать новый XElement с необходимыми эелементами из старого
        Dim phoneTypes As XElement =
          <phoneTypes>
              <%= From phone In root.<УсловияПоиска>
                  Select <ЧтоЗаУсловие><%= phone.@ИмяУсловия %></ЧтоЗаУсловие>
              %>
          </phoneTypes>

        ' создать атрибуты для XML-элемента декларативно, как часть XML, и динамически, добавив атрибут к экземпляру объекта XElement.
        ' Атрибут Type создается декларативно, а атрибут owner создается динамически.
        Dim phone2 As XElement = <phone type="home">206-555-0144</phone>
        phone2.@owner = "Harris, Phyllis"
    End Sub

    Private Sub ДобавлениеЭлементовАтрибутовИУзлов()
        ' Можно добавлять содержимое (элементы, атрибуты, комментарии, инструкции по обработке, текст и CDATA) к существующему XML-дереву.
        ' В следующем примере создаются два XML-дерева, затем выполняется изменение одного из них.
        Dim srcTree As XElement =
            <Root>
                <Element1>1</Element1>
                <Element2>2</Element2>
                <Element3>3</Element3>
                <Element4>4</Element4>
                <Element5>5</Element5>
            </Root>
        Dim xmlTree As XElement =
            <Root>
                <Child1>1</Child1>
                <Child2>2</Child2>
                <Child3>3</Child3>
                <Child4>4</Child4>
                <Child5>5</Child5>
            </Root>

        xmlTree.Add(<NewChild>new content</NewChild>)
        xmlTree.Add(
            From el In srcTree.Elements()
            Where CInt(el) > 3
            Select el)

        ' Even though Child9 does not exist in srcTree, the following statement  
        ' will not throw an exception, and nothing will be added to xmlTree.  
        xmlTree.Add(srcTree.Element("Child9"))
        Console.WriteLine(xmlTree)

        ' Этот код выводит следующие результаты:
        '<Root>  
        '  <Child1>1</Child1>  
        '  <Child2>2</Child2>  
        '  <Child3>3</Child3>  
        '  <Child4>4</Child4>  
        '  <Child5>5</Child5>  
        '  <NewChild>new content</NewChild>  
        '  <Element4>4</Element4>  
        '  <Element5>5</Element5>  
        '</Root> 
    End Sub

    Private Sub УдалениеЭлементовАтрибутовИУзловИзXMLдерева()
        ' Можно вносить изменения в XML-дерево, удаляя элементы, атрибуты и узлы других типов.
        ' Удаление одного элемента или атрибута из XML-документа является простой операцией. 
        ' Но при удалении коллекций элементов или атрибутов необходимо вначале материализовать коллекцию в список, а затем удалить элементы или атрибуты из списка. 
        ' Наилучшим подходом является использование метода расширения Remove, позволяющего выполнить эту задачу.
        ' Основной причиной этого является то, что большинство коллекций, получаемых из XML-дерева, формируется с помощью отложенного выполнения.
        ' Если не проводится их предварительная материализация в список или не используются методы расширения, то становится возможным возникновение определенного класса ошибок.

        'В этом примере показано три подхода к удалению элементов. Сначала удаляется одиночный элемент. 
        'Затем он возвращает коллекцию элементов, материализует их с помощью оператора Enumerable.ToList и удаляет коллекцию. 
        'Наконец, он получает коллекцию элементов и удаляет их с помощью метода расширения Remove.
        Dim root As XElement =
            <Root>
                <Child1>
                    <GrandChild1/>
                    <GrandChild2/>
                    <GrandChild3/>
                </Child1>
                <Child2>
                    <GrandChild4/>
                    <GrandChild5/>
                    <GrandChild6/>
                </Child2>
                <Child3>
                    <GrandChild7/>
                    <GrandChild8/>
                    <GrandChild9/>
                </Child3>
            </Root>

        root.<Child1>.<GrandChild1>.Remove()
        root.<Child2>.Elements().ToList().Remove()
        root.<Child3>.Elements().Remove()
        Console.WriteLine(root)
        'Этот код выводит следующие результаты:
        '<Root>  
        '  <Child1>  
        '    <GrandChild2 />  
        '    <GrandChild3 />  
        '  </Child1>  
        '  <Child2 />  
        '  <Child3 />  
        '</Root>  
    End Sub

    Private Function MakeXElement(listCmd As List(Of String)) As XElement
        Dim xmlArray As XElement = New XElement("Array")

        xmlArray.Add(New XElement("Name", "Meta data"))
        xmlArray.Add(New XElement("Dimsize", listCmd.Count))

        For Each strCmd As String In listCmd
            Dim xmlString As XElement = New XElement("String")
            xmlString.Add(New XElement("Name", "Name"))
            xmlString.Add(New XElement("Val", strCmd))

            Dim xmlCluster As XElement = New XElement("Cluster")
            xmlCluster.Add(New XElement("Name", "Meta Element"))
            xmlCluster.Add(New XElement("NumElts", 1))
            xmlCluster.Add(xmlString)

            xmlArray.Add(xmlCluster)
        Next

        Return xmlArray
    End Function

    Private Sub SaveMyApplication_UnhandledException()
        Dim xDoc As New XDocument
        ' если файл уже создан, то загрузить его, в противном случае создать заново

        ' создать новый документ из строки
        xDoc = XDocument.Parse("<?xml version=""1.0"" encoding=""utf-8""?><УсловияПоиска></УсловияПоиска>")

        Dim content = xDoc.<УсловияПоиска>(0) ' найти первый элемент под именем УсловияПоиска
        Dim Message As String = "сообщение"
        Dim stackTraceText As String = "текст стека исключения взять из события"
        ' добавить новый элемент исключения
        content.Add(
            <Exception>
                <Date_Time><%= Now.ToString("MM/dd/yyyy HH:mm") %></Date_Time>
                <Message><%= Message %></Message>
                <StackTrace><%= Environment.NewLine %>
                    <%= stackTraceText %><%= Environment.NewLine %>
                </StackTrace>
            </Exception>)

        ' сохранить файл
        content.Save(PathXmlFileCondition)
    End Sub

    Private Sub ЗагрузкаXMLдокументаИзФайла()
        '--- загрузка XML-документа из файла ----------------------------------
        Dim booksFromFile As XElement = XElement.Load(PathXmlFileCondition)

        Dim doc As XDocument = XDocument.Load(PathXmlFileCondition)
        Try
            doc.Parse(booksFromFile.ToString)
        Catch ex As System.Xml.XmlException
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub SaveConditionsXmlDoc()
        Dim XmlDoc As XDocument = <?xml version="1.0" encoding="utf-8"?>
                                  <УсловияПоиска>
                                      <ЧтоЗаУсловие Примечание="1">
                                          <ДочернееУсловиеN ИмяУсловия="Условие1" ТипЗакладки="Шаблон">
                                              <ИмяФормаПараметр>Параметр отсутствует</ИмяФормаПараметр>
                                              <МаксимальныйПределПараметра>100</МаксимальныйПределПараметра>
                                              <СтартВерхнее>100.00</СтартВерхнее>
                                              <СтартНижнее>90.91</СтартНижнее>
                                              <ПерегибВерхнее>100.00</ПерегибВерхнее>
                                              <ПерегибНижнее>90.91</ПерегибНижнее>
                                              <ФинишВерхнее>100.00</ФинишВерхнее>
                                              <ФинишНижнее>90.91</ФинишНижнее>
                                              <ВремяПерегиба>0.03</ВремяПерегиба>
                                              <ШиринаВременногоОкна>1.00</ШиринаВременногоОкна>
                                          </ДочернееУсловиеN>
                                          <ДочернееУсловиеN ИмяУсловия="Условие15" ТипЗакладки="Значение">
                                              <ИмяЗначениеПараметр>аРУД</ИмяЗначениеПараметр>
                                              <УсловиеОтбора>Между</УсловиеОтбора>
                                              <НижнееЗначениеУсловия>0</НижнееЗначениеУсловия>
                                              <ВерхнееЗначениеУсловия>100</ВерхнееЗначениеУсловия>
                                          </ДочернееУсловиеN>
                                      </ЧтоЗаУсловие>
                                      <ЧтоЗаУсловие Примечание="МахN1">
                                          <ДочернееУсловиеN ИмяУсловия="Условие1" ТипЗакладки="Значение">
                                              <ИмяЗначениеПараметр>N1</ИмяЗначениеПараметр>
                                              <УсловиеОтбора>НайтиМаксимум</УсловиеОтбора>
                                          </ДочернееУсловиеN>
                                      </ЧтоЗаУсловие>
                                  </УсловияПоиска>

        XmlDoc.Save(PathXmlFileCondition)
    End Sub
#End Region

#Region "ПоддержкаПарИмяЗначение"
    Private Sub ПоддержкаПарИмяЗначение_SetAttributeValue()
        'Множеству  приложений приходится сохранять данные, которые лучше всего хранить в виде пар «имя-значение». 
        'Эти данные могут представлять сведения о конфигурации или глобальные параметры. 
        'LINQ To XML содержит несколько методов, которые облегчают хранение пар «имя-значение». 
        'Можно либо оставить информацию в виде атрибутов, либо в виде набора дочерних элементов.
        'Одно из отличий между хранением информации в виде атрибутов и в виде дочерних элементов состоит в том, что атрибуты имеют ограничение в том,
        ' что для элемента может быть только один атрибут с данным именем. Это ограничение не относится к дочерним элементам.

        '        SetAttributeValue может добавлять, изменять или удалять атрибуты элемента.
        'При вызове метода SetAttributeValue с несуществующим именем атрибута этот метод создаст новый атрибут и добавит его в указанный элемент.
        'При вызове метода SetAttributeValue с существующим именем атрибута и с указанным содержимым содержимое данного атрибута замещается указанным содержимым.
        'При вызове метода SetAttributeValue с существующим именем атрибута и с указанием значения NULL в качестве содержимого атрибут удаляется из своего родителя.

        'Следующий пример показывает, как создать элемент без атрибутов. Затем используется метод SetAttributeValue для создания и поддержания списка пар «имя-значение».
        ' Create an element with no content.  
        Dim root As XElement = <Root/>

        ' Add a number of name/value pairs as attributes.  
        root.SetAttributeValue("Top", 22)
        root.SetAttributeValue("Left", 20)
        root.SetAttributeValue("Bottom", 122)
        root.SetAttributeValue("Right", 300)
        root.SetAttributeValue("DefaultColor", "Color.Red")
        Console.WriteLine(root)

        ' Replace the value of Top.  
        root.SetAttributeValue("Top", 10)
        Console.WriteLine(root)

        ' Remove DefaultColor.  
        root.SetAttributeValue("DefaultColor", Nothing)
        Console.WriteLine(root)
        'В этом примере выводятся следующие данные
        '<Root Top = "22" Left="20" Bottom="122" Right="300" DefaultColor="Color.Red" />  
        '<Root Top = "10" Left="20" Bottom="122" Right="300" DefaultColor="Color.Red" />  
        '<Root Top = "10" Left="20" Bottom="122" Right="300" />  
    End Sub

    Private Sub ПоддержкаПарИмяЗначение_SetElementValue()
        '        SetElementValue может добавлять, изменять или удалять дочерние элементы элемента.
        'При вызове метода SetElementValue с несуществующим именем дочернего элемента этот метод создаст новый элемент и добавит его к указанному элементу.
        'При вызове метода SetElementValue с существующим именем элемента и с указанным содержимым содержимое данного элемента замещается указанным содержимым.
        'При вызове метода SetElementValue с существующим именем атрибута и с указанием значения NULL в качестве содержимого атрибут удаляется из своего родителя.

        'Следующий пример показывает, как создать элемент без дочерних элементов. Затем используется метод SetElementValue для создания и поддержания списка пар «имя-значение».
        ' Create an element with no content.  
        Dim root As XElement = <Root/>

        ' Add a number of name/value pairs as elements.  
        root.SetElementValue("Top", 22)
        root.SetElementValue("Left", 20)
        root.SetElementValue("Bottom", 122)
        root.SetElementValue("Right", 300)
        root.SetElementValue("DefaultColor", "Color.Red")
        Console.WriteLine(root)
        Console.WriteLine("----")

        ' Replace the value of Top.  
        root.SetElementValue("Top", 10)
        Console.WriteLine(root)
        Console.WriteLine("----")

        ' Remove DefaultColor.  
        root.SetElementValue("DefaultColor", Nothing)
        Console.WriteLine(root)
        'В этом примере выводятся следующие данные
        '<Root>  
        '  <Top>22</Top>  
        '  <Left>20</Left>  
        '  <Bottom>122</Bottom>  
        '  <Right>300</Right>  
        '  <DefaultColor>Color.Red</DefaultColor>  
        '</Root>  
        '----  
        '<Root>  
        '  <Top>10</Top>  
        '  <Left>20</Left>  
        '  <Bottom>122</Bottom>  
        '  <Right>300</Right>  
        '  <DefaultColor>Color.Red</DefaultColor>  
        '</Root>  
        '----  
        '<Root>  
        '  <Top>10</Top>  
        '  <Left>20</Left>  
        '  <Bottom>122</Bottom>  
        '  <Right>300</Right>  
        '</Root> 
    End Sub

    Private Sub РаботаСоСловарямиСПомощьюLINQ_to_XML()
        ' Часто бывает удобно преобразовать структуры данных в XML, а затем преобразовать XML в другие структуры данных.
        ' В этом разделе показана конкретная реализация этого общего подхода на примере преобразования Dictionary<TKey,TValue> в XML и обратно.
        '  этом примере используются литералы XML и запрос во внедренном выражении.
        ' Запрос проецирует новые XElement объекты, которые затем становятся новым содержимым для объекта Root XElement.
        Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {"Child1", "Value1"},
            {"Child2", "Value2"},
            {"Child3", "Value3"},
            {"Child4", "Value4"}
        }
        Dim root As XElement =
            <Root>
                <%= From keyValue In dict
                    Select New XElement(keyValue.Key, keyValue.Value) %>
            </Root>
        Console.WriteLine(root)

        ' Этот код выводит следующие результаты:
        '<Root>  
        '  <Child1>Value1</Child1>  
        '  <Child2>Value2</Child2>  
        '  <Child3>Value3</Child3>  
        '  <Child4>Value4</Child4>  
        '</Root>

        ' Следующий код создает словарь на основе XML.
        Dim root2 As XElement =
        <Root>
            <Child1>Value1</Child1>
            <Child2>Value2</Child2>
            <Child3>Value3</Child3>
            <Child4>Value4</Child4>
        </Root>

        Dim dict2 As Dictionary(Of String, String) = New Dictionary(Of String, String)
        For Each el As XElement In root2.Elements
            dict2.Add(el.Name.LocalName, el.Value)
        Next
        For Each str As String In dict2.Keys
            Console.WriteLine("{0}:{1}", str, dict2(str))
        Next
        ' Этот код выводит следующие результаты:
        'Child1: Value1
        'Child2: Value2
        'Child3: Value3
        'Child4: Value4
    End Sub
#End Region

#Region "Сериализация"
    Private Sub СериализацияСПомощью_DataContractSerializer()
        ' В следующем примере создается некоторое количество объектов, содержащих объекты XElement. Затем они сериализуются в текстовые файлы и десериализуются из текстовых файлов.
        Test(Of XElement)(CreateXElement())
        Test(Of XElementContainer)(New XElementContainer())
        Test(Of XElementNullContainer)(New XElementNullContainer())
    End Sub

    Public Sub Test(Of T)(ByRef obj As Object)
        Dim s As DataContractSerializer = New DataContractSerializer(GetType(T))
        Using fs As FileStream = File.Open("test" & GetType(T).Name & ".xml", FileMode.Create)
            Console.WriteLine("Testing for type: {0}", GetType(T))
            s.WriteObject(fs, obj)
        End Using

        Using fs As FileStream = File.Open("test" & GetType(T).Name & ".xml", FileMode.Open)
            Dim s2 As Object = s.ReadObject(fs)
            If s2 Is Nothing Then
                Console.WriteLine("  Deserialized object is null (Nothing in VB)")
            Else
                Console.WriteLine("  Deserialized type: {0}", s2.GetType())
            End If
        End Using
    End Sub

    Public Function CreateXElement() As XElement
        Return New XElement(XName.Get("NameInNamespace", "http://www.adventure-works.org"))
    End Function

    <DataContract()>
    Public Class XElementContainer
        <DataMember()>
        Public member As XElement

        Public Sub XElementContainer()
            member = CreateXElement()
        End Sub
    End Class

    <DataContract()>
    Public Class XElementNullContainer
        <DataMember()>
        Public member As XElement

        Public Sub XElementNullContainer()
            member = Nothing
        End Sub
    End Class
#End Region

#Region "Файл CSV <-> XML"
    Private Sub СоздаватьТекстовыеФайлыИзXML()
        'как создавать файл с разделителями-запятыми (csv) из XML-файла.
        'Версия Visual Basic использует процедурный код для статистической обработки коллекции строк в одной строке.
        Dim custOrd As XElement = XElement.Load("CustomersOrders.xml")
        Dim strCollection As IEnumerable(Of String) =
            From el In custOrd.<Customers>.<Customer>
            Select
                String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}{10}",
                    el.@CustomerID,
                    el.<CompanyName>.Value,
                    el.<ContactName>.Value,
                    el.<ContactTitle>.Value,
                    el.<Phone>.Value,
                    el.<FullAddress>.<Address>.Value,
                    el.<FullAddress>.<City>.Value,
                    el.<FullAddress>.<Region>.Value,
                    el.<FullAddress>.<PostalCode>.Value,
                    el.<FullAddress>.<Country>.Value,
                    Environment.NewLine
                )
        Dim sb As StringBuilder = New StringBuilder()
        For Each str As String In strCollection
            sb.Append(str)
        Next
        Console.WriteLine(sb.ToString())
        ' Этот код выводит следующие результаты:
        'GREAL,Great Lakes Food Market,Howard Snyder,Marketing Manager,(503) 555-7555,2732 Baker Blvd.,Eugene,OR,97403,USA  
    End Sub

    Private Sub СоздаватьXMLФайлыИзCSVфайлов()
        ' как использовать LINQ и LINQ to XML для создания XML-файла из файла значений с разделителями-запятыми (CSV).
        ' Следующий код выполняет запрос LINQ к массиву строк.

        ' Create the text file.  
        Dim csvString As String = "GREAL,Great Lakes Food Market,Howard Snyder,Marketing Manager,(503) 555-7555,2732 Baker Blvd.,Eugene,OR,97403,USA" & vbCrLf &
            "HUNGC,Hungry Coyote Import Store,Yoshi Latimer,Sales Representative,(503) 555-6874,City Center Plaza 516 Main St.,Elgin,OR,97827,USA" & vbCrLf &
            "LAZYK,Lazy K Kountry Store,John Steel,Marketing Manager,(509) 555-7969,12 Orchestra Terrace,Walla Walla,WA,99362,USA" & vbCrLf &
            "LETSS,Let's Stop N Shop,Jaime Yorres,Owner,(415) 555-5938,87 Polk St. Suite 5,San Francisco,CA,94117,USA"
        File.WriteAllText("cust.csv", csvString)

        ' Read into an array of strings.  
        Dim source As String() = File.ReadAllLines("cust.csv")
        Dim cust As XElement =
            <Root>
                <%= From strs In source
                    Let fields = Split(strs, ",")
                    Select
                    <Customer CustomerID=<%= fields(0) %>>
                        <CompanyName><%= fields(1) %></CompanyName>
                        <ContactName><%= fields(2) %></ContactName>
                        <ContactTitle><%= fields(3) %></ContactTitle>
                        <Phone><%= fields(4) %></Phone>
                        <FullAddress>
                            <Address><%= fields(5) %></Address>
                            <City><%= fields(6) %></City>
                            <Region><%= fields(7) %></Region>
                            <PostalCode><%= fields(8) %></PostalCode>
                            <Country><%= fields(9) %></Country>
                        </FullAddress>
                    </Customer>
                %>
            </Root>
        Console.WriteLine(cust)
    End Sub
#End Region

#Region "События"
    Private Sub СобытияLINQtoXML()
        ' События Linq To XML позволяют получать уведомления, когда изменяется дерево XML.
        ' Можно добавить события в экземпляр любого объекта XObject.
        ' Обработчик события будет получать уведомления об изменениях объекта XObject и всех его потомков.
        ' Например, можно добавить обработчик события в корень дерева и обрабатывать все изменения дерева в этом обработчике события.
        ' События полезны, когда нужно поддержать какие-либо статистические данные в дереве XML.
        ' Например, нужно рассчитать сумму элементов строки. Следующий пример использует события для подсчета суммы всех дочерних элементов сложного элемента Items.
        total = root.<Total>(0)
        ItemXElement = root.<Items>(0)
        ItemXElement.SetElementValue("Item1", 25)
        ItemXElement.SetElementValue("Item2", 50)
        ItemXElement.SetElementValue("Item2", 75)
        ItemXElement.SetElementValue("Item3", 133)
        ItemXElement.SetElementValue("Item1", Nothing)
        ItemXElement.SetElementValue("Item4", 100)
        Console.WriteLine("Total:{0}", CInt(total))
        Console.WriteLine(root)
        'Этот код выводит следующие результаты
        'Changed System.Xml.Linq.XElement Add  
        'Changed System.Xml.Linq.XElement Add  
        'Changed System.Xml.Linq.XText Remove  
        'Changed System.Xml.Linq.XText Add  
        'Changed System.Xml.Linq.XElement Add  
        'Changed System.Xml.Linq.XElement Remove  
        'Changed System.Xml.Linq.XElement Add  
        'Total:308  
        '<Root>  
        '  <Total>308</Total>  
        '  <Items>  
        '    <Item2>75</Item2>  
        '    <Item3>133</Item3>  
        '    <Item4>100</Item4>  
        '  </Items>  
        '</Root>  
    End Sub

    Dim WithEvents ItemXElement As XElement = Nothing
    Dim total As XElement = Nothing
    ReadOnly root As XElement =
            <Root>
                <Total>0</Total>
                <Items></Items>
            </Root>

    Private Sub XObjectChanged(ByVal sender As Object, ByVal cea As XObjectChangeEventArgs) Handles ItemXElement.Changed
        Select Case cea.ObjectChange
            Case XObjectChange.Add
                If sender.GetType() Is GetType(XElement) Then
                    total.Value = CStr(CInt(total.Value) + CInt((DirectCast(sender, XElement)).Value))
                End If
                If sender.GetType() Is GetType(XText) Then
                    total.Value = CStr(CInt(total.Value) + CInt((DirectCast(sender, XText)).Value))
                End If
            Case XObjectChange.Remove
                If sender.GetType() Is GetType(XElement) Then
                    total.Value = CStr(CInt(total.Value) - CInt((DirectCast(sender, XElement)).Value))
                End If
                If sender.GetType() Is GetType(XText) Then
                    total.Value = CStr(CInt(total.Value) -
                            CInt((DirectCast(sender, XText)).Value))
                End If
        End Select

        Console.WriteLine($"Changed {sender.GetType()} {cea.ObjectChange}")
    End Sub
#End Region

    Private Sub ИзменитьПространствоИменДляВсегоДереваXML()
        ' Иногда необходимо программно изменить пространство имен для элемента или атрибута. В LINQ to XML это делается легко.
        ' Можно установить свойство XElement.Name. Свойство XAttribute.Name не может быть установлено,
        ' но можно легко скопировать атрибуты в объект System.Collections.Generic.List<T>, заменив существующие атрибуты,
        ' а затем добавить новые атрибуты из нового пространства имен.

        'Следующий код создает два XML-дерева без пространства имен. Затем он изменяет пространства имен каждого дерева и объединяет их в одно дерево.
        Dim tree1 As XElement =
            <Data>
                <Child MyAttr="content">content</Child>
            </Data>
        Dim tree2 As XElement =
            <Data>
                <Child MyAttr="content">content</Child>
            </Data>

        Dim aw As XNamespace = "http://www.adventure-works.com"
        Dim ad As XNamespace = "http://www.adatum.com"
        ' change the namespace of every element and attribute in the first tree  
        For Each el As XElement In tree1.DescendantsAndSelf
            el.Name = aw.GetName(el.Name.LocalName)
            Dim atList As List(Of XAttribute) = el.Attributes().ToList()
            el.Attributes().Remove()
            For Each at As XAttribute In atList
                el.Add(New XAttribute(aw.GetName(at.Name.LocalName), at.Value))
            Next
        Next

        ' change the namespace of every element and attribute in the second tree  
        For Each el As XElement In tree2.DescendantsAndSelf()
            el.Name = ad.GetName(el.Name.LocalName)
            Dim atList As List(Of XAttribute) = el.Attributes().ToList()
            el.Attributes().Remove()
            For Each at As XAttribute In atList
                el.Add(New XAttribute(ad.GetName(at.Name.LocalName), at.Value))
            Next
        Next

        ' add attribute namespaces so that the tree will be serialized with  
        ' the aw and ad namespace prefixes  
        tree1.Add(New XAttribute(XNamespace.Xmlns + "aw", "http://www.adventure-works.com"))
        tree2.Add(New XAttribute(XNamespace.Xmlns + "ad", "http://www.adatum.com"))
        ' create a new composite tree  
        Dim root As XElement =
            <Root>
                <%= tree1 %>
                <%= tree2 %>
            </Root>
        Console.WriteLine(root)

        'В этом примере выводятся следующие данные
        '<Root>  
        '  <aw:Data xmlns: aw = "http://www.adventure-works.com" >
        '    <aw:Child aw:MyAttr="content">content</aw:Child>
        '          </aw:Data>
        '          <ad:Data xmlns:ad="http://www.adatum.com">
        '              <ad:Child ad:MyAttr="content">content</ad:Child>
        '          </ad:Data>  
        '</Root> 
    End Sub

End Module
