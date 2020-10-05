''' <summary>
''' Строго типизированные ToolboxTab объекты
''' </summary>
''' <remarks></remarks>
<Serializable()> 
Public Class ToolboxTabCollection
    Inherits CollectionBase

    '''  <summary>
    '''  Инициализация нового экземпляра cref="ToolboxLibrary.ToolboxTabCollection"/>.
    '''  </summary>
    '''  <remarks></remarks>
    Public Sub New()
    End Sub

    '''  <summary>
    '''  Инициализировать новый cref="ToolboxLibrary.ToolboxTabCollection"/> базовый для другого cref="ToolboxLibrary.ToolboxTabCollection"/>.
    '''  </summary>
    '''  <param name="value">
    '''      Экземпляр cref="ToolboxLibrary.ToolboxTabCollection"/> с помощью которого содержимое копируется
    '''  </param>
    '''  <remarks></remarks>
    Public Sub New(ByVal value As ToolboxTabCollection)
        AddRange(value)
    End Sub

    '''  <summary>
    '''  Инициализировать новый экземпляр cref="ToolboxLibrary.ToolboxTabCollection"/> содержащий любой массив cref="ToolboxLibrary.ToolboxTab"/> объектов.
    '''  </summary>
    '''  <param name="value">
    '''       Масссив из cref="ToolboxLibrary.ToolboxTab"/> объектов с которыми инициализируется коллекция
    '''  </param>
    '''  <remarks></remarks>
    Public Sub New(ByVal value As ToolboxTab())
        AddRange(value)
    End Sub

    '''  <summary>
    '''  Представление мходного специфицированного индекса cref="ToolboxLibrary.ToolboxTab"/>.
    '''  </summary>
    '''  <param name="index">Начинающаяся с нуля база индекса входа местоположение в коллекции.</param>
    '''  <value>
    '''  Вход специфицированного индекса в коллекцию.
    '''  </value>
    '''  <remarks><exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> выходной правильный индекс для коллекции.</exception></remarks>
    Default Public Property Item(ByVal index As Integer) As ToolboxTab
        Get
            'Return DirectCast((List(index)), ToolboxTab)
            Return CType((List(index)), ToolboxTab)
        End Get
        Set(ByVal value As ToolboxTab)
            List(index) = value
        End Set
    End Property

    '''  <summary>
    '''    Добавить cref="ToolboxLibrary.ToolboxTab"/> специфицированное значение в коллекцию
    '''    cref="ToolboxLibrary.ToolboxTabCollection"/> .
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxTab"/> to add.</param>
    '''  <returns>
    '''    Индекс с каким новым элементом был вставлен.
    '''  </returns>
    '''  <remarks>cref="ToolboxLibrary.ToolboxTabCollection.AddRange"/></remarks>
    Public Function Add(ByVal value As ToolboxTab) As Integer
        Return List.Add(value)
    End Function

    '''  <summary>
    '''  Копирование элементов из массива в конец cref="ToolboxLibrary.ToolboxTabCollection"/>.
    '''  </summary>
    '''  <param name="value">
    '''    An array of type [see cref="ToolboxLibrary.ToolboxTab"/> содержит объекты добавленные в коллекцию.
    '''  </param>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxTabCollection.Add"/></remarks>
    Public Sub AddRange(ByVal value As ToolboxTab())
        Dim i As Integer = 0
        While (i < value.Length)
            Add(value(i))
            i += 1
        End While
    End Sub

    '''  <summary>
    '''  Добавление содержимого другого  [see cref="ToolboxLibrary.ToolboxTabCollection"/> в конец коллекции.
    '''  </summary>
    '''  <param name="value">
    '''    Элемент[see cref="ToolboxLibrary.ToolboxTabCollection"/> содержит объекты для добавления в коллекцию.
    '''  </param>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxTabCollection.Add"/></remarks>
    Public Sub AddRange(ByVal value As ToolboxTabCollection)
        Dim i As Integer = 0
        While (i < value.Count)
            Add(value(i))
            i += 1
        End While
    End Sub

    '''  <summary>
    '''  Получить значение индицирующее 
    '''    [see cref="ToolboxLibrary.ToolboxTabCollection"/> содержится-ли специфичный [see cref="ToolboxLibrary.ToolboxTab"/>.
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxTab"/> to locate.</param>
    '''  <returns>
    '''  [see langword="true"/> Если [see cref="ToolboxLibrary.ToolboxTab"/> содержится в коллекции; 
    '''   иначе, [see langword="false"/>.
    '''  </returns>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxTabCollection.IndexOf"/></remarks>
    Public Function Contains(ByVal value As ToolboxTab) As Boolean
        Return List.Contains(value)
    End Function

    '''  <summary>
    '''  Копировать[see cref="ToolboxLibrary.ToolboxTabCollection"/> значение в одномерный массив  [see cref="System.Array"/> экземпяр
    '''    специфицированный индексом.
    '''  </summary>
    '''  <param name="array">Одномерный массив [see cref="System.Array"/> который  предназначен для копирования из [see cref="ToolboxLibrary.ToolboxTabCollection"/> .</param>
    '''  <param name="index">Индекс массива<paramref name="array"/> где копирование начинактся.</param>
    '''  <remarks><exception cref="System.ArgumentException"><paramref name="array"/> многомерный. <para>-or-</para> <para>Число элементов в [see cref="ToolboxLibrary.ToolboxTabCollection"/> есть больше чем доступное пространство между <paramref name="index"/> и концом <paramref name="array"/>.</para></exception>
    '''  <exception cref="System.ArgumentNullException"><paramref name="array"/> есть [see langword="null"/>. </exception>
    '''  <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than <paramref name="array"/>"s lowbound. </exception>
    '''  [seealso cref="System.Array"/>
    '''  </remarks>
    Public Sub CopyTo(ByVal array As ToolboxTab(), ByVal index As Integer)
        List.CopyTo(array, index)
    End Sub

    '''  <summary>
    '''    Возвращает индекс [see cref="ToolboxLibrary.ToolboxTab"/> в 
    '''        [see cref="ToolboxLibrary.ToolboxTabCollection"/> .
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxTab"/> to locate.</param>
    '''  <returns>
    '''  Индекс [see cref="ToolboxLibrary.ToolboxTab"/> of <paramref name="value"/> в
    '''  [see cref="ToolboxLibrary.ToolboxTabCollection"/>, если найден; иначе, -1.
    '''  </returns>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxTabCollection.Contains"/></remarks>
    Public Function IndexOf(ByVal value As ToolboxTab) As Integer
        Return List.IndexOf(value)
    End Function

    '''  <summary>
    '''  Включить [see cref="ToolboxLibrary.ToolboxTab"/> внутрь [see cref="ToolboxLibrary.ToolboxTabCollection"/> по индексу.
    '''  </summary>
    '''  <param name="index">С нуля начинающийся индекс массива <paramref name="value"/>должен быть вставлен.</param>
    '''  <param name=" value">The [see cref="ToolboxLibrary.ToolboxTab"/> to insert.</param>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxTabCollection.Add"/></remarks>
    Public Sub Insert(ByVal index As Integer, ByVal value As ToolboxTab)
        List.Insert(index, value)
    End Sub

    '''  <summary>
    '''    Возвращает перечислитель для иттерации сквозь  
    '''        [see cref="ToolboxLibrary.ToolboxTabCollection"/> .
    '''  </summary>
    '''  <returns>Перечеслитель коллекции</returns>
    '''  <remarks>[seealso cref="System.Collections.IEnumerator"/></remarks>
    Public Shadows Function GetEnumerator() As ToolboxTabEnumerator
        Return New ToolboxTabEnumerator(Me)
    End Function

    '''  <summary>
    '''  Возвращение специфицированного[see cref="ToolboxLibrary.ToolboxTab"/> из 
    '''    [see cref="ToolboxLibrary.ToolboxTabCollection"/> .
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxTab"/> для удаления [see cref="ToolboxLibrary.ToolboxTabCollection"/> .</param>
    '''  <remarks><exception cref="System.ArgumentException"><paramref name="value"/> не найден в коллекции </exception></remarks>
    Public Sub Remove(ByVal value As ToolboxTab)
        List.Remove(value)
    End Sub

    Public Class ToolboxTabEnumerator
        Inherits Object
        Implements IEnumerator

        Private baseEnumerator As IEnumerator

        Private temp As IEnumerable

        Public Sub New(ByVal mappings As ToolboxTabCollection)
            temp = DirectCast((mappings), IEnumerable)
            baseEnumerator = temp.GetEnumerator()
        End Sub

        Public ReadOnly Property Current() As ToolboxTab
            Get
                Return DirectCast((baseEnumerator.Current), ToolboxTab)
            End Get
        End Property

        Private ReadOnly Property IEnumerator_Current() As Object Implements IEnumerator.Current
            Get
                Return baseEnumerator.Current
            End Get
        End Property

        Public Function MoveNext() As Boolean
            Return baseEnumerator.MoveNext()
        End Function

        Private Function IEnumerator_MoveNext() As Boolean Implements IEnumerator.MoveNext
            Return baseEnumerator.MoveNext()
        End Function

        Public Sub Reset()
            baseEnumerator.Reset()
        End Sub

        Private Sub IEnumerator_Reset() Implements IEnumerator.Reset
            baseEnumerator.Reset()
        End Sub
    End Class
End Class


''' <summary>
''' ToolboxTabs.
''' </summary>
Public Class ToolboxTab
    Private mName As String = Nothing

    Public Sub New()
    End Sub

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property ToolboxItems() As ToolboxItemCollection = Nothing
End Class
