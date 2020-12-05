Namespace FactoryMethod
    ' В фабричный метод передается ключ, на основании которого конкретный создатель решает, какой именно конкретный продукт создать и вернуть. 
    ' Большой набор значений ключа часто приводит к загроможденному коду и, как следствие, к трудностям сопровождения приложения. 
    ' Данную проблему можно устранить при помощи обобщений (generics) и анонимных методов (anonymous methods) языка C# 2.0. 
    ' Для этого предлагается ввести дополнительный класс KeyedFactory, на который возлагается выбор метода создания объекта и его вызов. 
    ' Конкретный создатель при инициализации регистрирует в KeyedFactory пары <значение, делегат создания> и непосредственно в фабричном методе делегирует создание объекта KeyedFactory. 
    ' По сути своей KeyedFactory является словарем <ключ, фабричный метод>.
    '
    ' Если перенести KeyedFactory в Creator и создать Public-метод для регистрации продуктов по ключу, то необходимость в порождении ConcreteCreator отпадает, 
    ' т.е. клиент сможет через метод Register изменить создаваемый тип продуктов непосредственно в создателе.
    '
    ' Реализация класса KeyedFactory. В основе реализации лежит обобщенный делегат TProduct FactoryMethod<TProduct>(), 
    ' экземпляры которого хранятся в качестве значений словаря <TKey, FactoryMethod<TProduct>> в Private-поле _factoryMethods. 
    ' В методе KeyedFactory.Create(TKey key) из словаря достается делегат, и с его помощью создается экземпляр продукта. 

    Public Delegate Function FactoryMethod(Of TAnalysis)() As TAnalysis

    Public Class AnalysisFactory
        'Public Shared Sub Main()
        '    'ClassicFactoryMethod()
        '    KeyedFactoryMethod()
        'End Sub

        'Private Shared Sub ClassicFactoryMethod()
        '	' an array of creators
        '	Dim creators() As Creator = {
        '		New ConcreteCreatorA(),
        '		New ConcreteCreatorB()
        '	}
        '	' iterate over creators and create products
        '	For Each creator As Creator In creators
        '		Dim analysis As TAnalysis = creator.FactoryMethod()
        '		Console.WriteLine("Created {0}", analysis.GetTypeToString())
        '	Next

        '	' Wait for user
        '	Console.ReadKey()
        'End Sub

        'Private Shared Sub KeyedFactoryMethod()
        '    Dim factory As New KeyedFactory()
        '    Dim someCreator As New ConcreteCreatorC()

        '    Dim keyValue As String = "ConcreteAnalysisB"
        '    ' В общем случае в KeyedFactory при помощи метода RegisterMethod регистрируются фабричные методы(через делегаты),         
        '    ' в C# регистрация выглядит следующим образом:
        '    'factory.RegisterMethod(keyValue, new FactoryMethod<Tanalysis>(someCreator.FactoryMethod));

        '    ' Компилятор C# 2.0 понимает более компактную запись, без явного создания делегата:
        '    'factory.RegisterMethod(keyValue, someCreator.FactoryMethod);

        '    ' Также возможно использование анонимных методов:
        '    'factory.RegisterMethod(keyValue, delegate { return new ConcreteAnalysisB(); });

        '    ' При помощи лямбда-выражений C# 3.0 эту запись можно сократить:
        '    factory.RegisterMethod(keyValue, Function() New ConcreteAnalysisB())
        '    Dim analysisB As TAnalysis = factory.Create(keyValue)
        '    Console.WriteLine("Created {0}", analysisB.GetTypeToString())

        '    ' Если конкретный продукт имеет public-конструктор без параметров, то создание делегата для удобства можно перенести в саму фабрику, 
        '    ' используя ограничитель (constrain) обобщения new() следующим образом:
        '    factory.RegisterType(Of ConcreteAnalysisA)("ConcreteAnalysisA")
        '    Dim analysisA As TAnalysis = factory.Create("ConcreteAnalysisA")
        '    Console.WriteLine("Created {0}", analysisA.GetTypeToString())

        '    ' Более того, функциональность KeyedFactory можно расширить поддержкой прототипов.
        '    ' Для этого используется способность анонимных методов захватывать внешние переменные – замыкание(closure).
        '    ' До тех пор, пока где - то имеется «живая» ссылка на делегат, который, в свою очередь ссылается на анонимный метод, 
        '    ' будут живы все переменные, на которые замкнут этот анонимный метод.
        '    ' Физически в текущих версиях компилятора C# от Microsoft для этого используется генерирование скрытого класса, 
        '    ' но это уже детали реализации. Этой возможностью предлагается воспользоваться для реализации поддержки прототипов в KeyedFactory.
        '    'ConcreteAnalysisC typePoduct = new ConcreteAnalysisC();
        '    factory.RegisterPrototype(Of ConcreteAnalysisC)("ConcreteAnalysisC", New ConcreteAnalysisC())
        '    Dim analysisC As TAnalysis = factory.Create("ConcreteAnalysisC")
        '    Console.WriteLine("Created {0}", analysisC.GetTypeToString())

        '    ' Wait for user
        '    Console.ReadKey()
        '    ' Описанный подход к реализации Parameterized Factory Method делает код более простым и понятным.
        '    ' KeyedFactory представляет удобные методы регистрации способов создания объектов: 
        '    ' RegisterType, RegisterPrototype, RegisterImmutable, RegisterLazyPrototype, RegisterLazyImmutable.
        '    ' Однако, как видно, способ создания объектов сильно влияет на производительность и разработчик должен это учитывать при его использовании. 
        '    ' Существенным недостатком такой реализации является необходимость наличия конструктора без параметров в случае регистрации типов(не прототипов и immutable),
        '    ' но это можно обойти при помощи инициализирующих методов.
        'End Sub

        Private factory As New KeyedFactory()
        Private Const cAnalysisA As String = "ConcreteAnalysisA"
        Private Const cAnalysisB As String = "ConcreteAnalysisB"
        Private Const cAnalysisC As String = "ConcreteAnalysisC"

        Public Sub New()
            factory.RegisterType(Of ConcreteAnalysisA)(cAnalysisA)
            factory.RegisterType(Of ConcreteAnalysisB)(cAnalysisB)
            factory.RegisterType(Of ConcreteAnalysisC)(cAnalysisC)
        End Sub

        ' Тестирование данного класса:
        'Dim mAnalysisFactory As New FactoryMethod.AnalysisFactory
        'mAnalysisFactory.Test()

        Public Sub Test()
            Dim analysisA As TAnalysis = GetAnalysis(cAnalysisA)
            Dim analysisB As TAnalysis = GetAnalysis(cAnalysisB)
            Dim analysisC As TAnalysis = GetAnalysis(cAnalysisC)

            MessageBox.Show($"Created:{vbCrLf}{analysisA.GetTypeToString()}{vbCrLf}{analysisB.GetTypeToString()}{vbCrLf}{analysisC.GetTypeToString()}",
                            $"{NameOf(Test)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ''' <summary>
        ''' Выдать класс расшифровки прямо из коллекции если он был создан хотя-бы один раз,
        ''' в противном случае создать его и добавить в коллекцию.
        ''' Ленивая инициализвция.
        ''' </summary>
        ''' <param name="nameAnalysis"></param>
        ''' <returns></returns>
        Public Function GetAnalysis(ByVal nameAnalysis As String) As TAnalysis
            If Not factory.ContainsKey(nameAnalysis) Then
                ' Если конкретный продукт имеет public-конструктор без параметров, то создание делегата для удобства можно перенести в саму фабрику, 
                ' используя ограничитель (constrain) обобщения new() следующим образом:
                Throw New ArgumentException($"Тип {nameAnalysis} не зарегистрирован в <KeyedFactory>.")
            End If
            Return factory.Create(nameAnalysis)
        End Function

        'Public Function GetAnalysis(ByVal nameAnalysis As String) As TAnalysis
        '    If Not factory.ContainsKey(nameAnalysis) Then
        '        ' Если конкретный продукт имеет public-конструктор без параметров, то создание делегата для удобства можно перенести в саму фабрику, 
        '        ' используя ограничитель (constrain) обобщения new() следующим образом:
        '        factory.RegisterType(Of ConcreteAnalysisA)(nameAnalysis)
        '    End If
        '    Return factory.Create(nameAnalysis)
        'End Function
    End Class
End Namespace