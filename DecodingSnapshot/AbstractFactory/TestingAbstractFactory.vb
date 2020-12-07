Namespace Factory
    ' Совместное использование
    ' Несложно заметить, что интерфейс фабрики, основанной на прототипах, отличается от интерфейса рассмотренной ранее generic-фабрики только методами SetPrototype<>() и GetPrototype<>(). 
    ' Отсюда следует, что можно выделить наиболее общий интерфейс для всех фабрик, а именно, содержащий только методы Create<>() и IsProduct<>(). 
    ' Этот интерфейс назовем IAbstractFactory. Определим интерфейс фабрики на базе прототипов, назовем его ICloneFactory. 
    ' Он будет производным от IAbstractFactory, и будет содержать методы, специально предназначенные для работы с прототипами.
    ' Аналогично, выделим общий интерфейс для фабричных методов – IFactoryMethod<>, содержащий только метод Create(). 
    ' Производным от него будет являться ICloneMethod<>, главная задача которого – обеспечение доступа к прототипу через соответствующее свойство.

    ' Далее создаем абстрактный класс AbstractFactory, реализующий интерфейс IAbstractFactory, код которого рассматривался выше. 
    ' Частный случаем является фабрика, основанная на использовании прототипов, поэтому класс CloneFactory одновременно наследуется от AbstractFactory и реализует интерфейс ICloneFactory.
    ' Таким образом, благодаря иерархии интерфейсов фабричных методов и фабрик получаем механизм совместного использования продуктов, 
    ' создаваемых как посредством клонирования, так и посредством вызова фабричного метода.

    ' Теперь, чтобы создать фабрику с такими свойствами, необходимо сделать ее наследницей CloneFactory и реализовать интерфейсы IFactoryMethod<> и ICloneMethod<>. 
    ' В качестве примера приведем фабрику, создающую элементы управления.
    ' При передаче типа Edit в Create<>() с помощью фабричного метода будет создан объект StyledEdit. 
    ' При передаче Button будет клонирован объект buttonPrototype. Важно, чтобы класс Button реализовывал интерфейс IClonable.

    Friend Class TestingAbstractFactory
        Public Shared Sub Test()
            '// установить прототип Button в ICloneFactory фабрику
            'ICloneFactory cloneFactory = new ConcreteFactory1();
            'cloneFactory.SetPrototype<AbstractProductA>(new ProductA1());

            '// привели к более общему типу (IAbstractFactory)
            'IAbstractFactory factory = cloneFactory;

            '// создаёт новый экземпляр
            'AbstractProductB edit = factory.Create<AbstractProductB>();
            '// клонирует из ранее созданного прототипа
            'AbstractProductA button = factory.Create<AbstractProductA>();

            ' Вызов абстрактной фабрики № 1
            Dim cloneFactory1 As ICloneFactory = New ConcreteFactory1()
            cloneFactory1.SetPrototype(Of AbstractProductA)(New ProductA1())
            ' привели к более общему типу (IAbstractFactory)
            Dim factory1 As IAbstractFactory = cloneFactory1
            Dim client1 As New Client(factory1)
            client1.Run()

            ' Вызов абстрактной фабрики № 2
            Dim cloneFactory2 As ICloneFactory = New ConcreteFactory2()
            cloneFactory2.SetPrototype(Of AbstractProductA)(New ProductA2())
            ' привели к более общему типу (IAbstractFactory)
            Dim factory2 As IAbstractFactory = cloneFactory2
            Dim client2 As New Client(factory2)
            client2.Run()

            MessageBox.Show($"{client1.Result}{vbCrLf}{client2.Result}", $"{NameOf(TestingAbstractFactory)}:{NameOf(Test)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
    End Class
End Namespace