''' <summary>
''' строго-типизированная коллекция ToolboxItem объектов
''' </summary>
''' <remarks></remarks>
<Serializable()> 
Public Class ToolboxItemCollection
    Inherits CollectionBase

    '''  <summary>
    ''' инициализация нового экземпляра cref="ToolboxLibrary.ToolboxItemCollection"
    '''  </summary>
    '''  <remarks></remarks>
    Public Sub New()
    End Sub

    '''  <summary>
    ''' инициализация нового экземпляра cref="ToolboxLibrary.ToolboxItemCollection" базового другого cref="ToolboxLibrary.ToolboxItemCollection"
    '''  </summary>
    '''  <param name="value">
    '''       cref="ToolboxLibrary.ToolboxItemCollection" с которым содержимое скопировано
    '''  </param>
    '''  <remarks></remarks>
    Public Sub New(ByVal value As ToolboxItemCollection)
        AddRange(value)
    End Sub

    '''  <summary>
    ''' инициализация нового экземпляра cref="ToolboxLibrary.ToolboxItemCollection" содержащего массив cref="ToolboxLibrary.ToolboxItemCollection" объектов
    '''  </summary>
    '''  <param name="value">
    '''  массив cref="ToolboxLibrary.ToolboxItem" объектов с которым инициализированна коллекция
    '''  </param>
    '''  <remarks></remarks>
    Public Sub New(ByVal value As ToolboxItem())
        AddRange(value)
    End Sub

    '''  <summary>
    ''' Представление входа специфицированного индекса cref="ToolboxLibrary.ToolboxItem"
    '''  </summary>
    '''  <param name="index">Начинающаяся с нуля база индекса входа местоположение в коллекции.</param>
    '''  <value>
    '''  Вход специфицированного индекса в коллекцию.
    '''  </value>
    '''  <remarks><exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> выходной правильный индекс для коллекции.</exception></remarks>
    Default Public Property Item(ByVal index As Integer) As ToolboxItem
        Get
            Return DirectCast((List(index)), ToolboxItem)
        End Get
        Set(ByVal value As ToolboxItem)
            List(index) = value
        End Set
    End Property

    '''  <summary>
    '''    Добавить cref="ToolboxLibrary.ToolboxItem"/> специфицированное значение в коллекцию
    '''    [see cref="ToolboxLibrary.ToolboxItemCollection"/> .
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxItem"/> to add.</param>
    '''  <returns>
    '''    Индекс с каким новым элементом был вставлен.
    '''  </returns>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxItemCollection.AddRange"/></remarks>
    Public Function Add(ByVal value As ToolboxItem) As Integer
        Return List.Add(value)
    End Function

    '''  <summary>
    '''  Копирование элементов из массива в конец cref="ToolboxLibrary.ToolboxItemCollection"/>.
    '''  </summary>
    '''  <param name="value">
    '''    Массив типа  cref="ToolboxLibrary.ToolboxItem"/> содержит объекты добавленные в коллекцию.
    '''  </param>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
    Public Sub AddRange(ByVal value As ToolboxItem())
        Dim i As Integer = 0
        While (i < value.Length)
            Add(value(i))
            i += 1
        End While
    End Sub

    '''  <summary>
    '''      Добавление содержимого другого [see cref="ToolboxLibrary.ToolboxItemCollection"/> в конец коллекции.
    '''  </summary>
    '''  <param name="value">
    '''    Элемент cref="ToolboxLibrary.ToolboxItemCollection" содержит объекты для добавления в коллекцию.
    '''  </param>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
    Public Sub AddRange(ByVal value As ToolboxItemCollection)
        Dim i As Integer = 0
        While (i < value.Count)
            Add(value(i))
            i += 1
        End While
    End Sub

    '''  <summary>
    '''  Получить значение индицирующее 
    '''    [see cref="ToolboxLibrary.ToolboxItemCollection"/> содержится-ли специфичный cref="ToolboxLibrary.ToolboxItem"/>.
    '''  </summary>
    '''  <param name="value">Экземпляр cref="ToolboxLibrary.ToolboxItem"/> местоположения.</param>
    '''  <returns>
    '''  langword="true" в cref="ToolboxLibrary.ToolboxItem"/> содержится в коллекции; 
    '''   otherwise, [see langword="false"/>.
    '''  </returns>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxItemCollection.IndexOf"/></remarks>
    Public Function Contains(ByVal value As ToolboxItem) As Boolean
        Return List.Contains(value)
    End Function

    '''  <summary>
    '''  Копировать cref="ToolboxLibrary.ToolboxItemCollection" значение в одномерный массив cref="System.Array"/> экземпяр 
    '''    специфицированный индексом.
    '''  </summary>
    '''  <param name="array">Одномерный массив cref="System.Array"/> который  предназначен для копирования из [see cref="ToolboxLibrary.ToolboxItemCollection"/> .</param>
    '''  <param name="index">Индекс массива <paramref name="array"/> где копирование начинактся.</param>
    '''  <remarks><exception cref="System.ArgumentException"><paramref name="array"/> многомерный. <para>-or-</para> <para>Число элементов в cref="ToolboxLibrary.ToolboxItemCollection"/> есть больше чем доступное пространство между <paramref name="index"/> и концом <paramref name="array"/>.</para></exception>
    '''  <exception cref="System.ArgumentNullException"><paramref name="array"/> есть langword="null"/>. </exception>
    '''  <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> меньше чем<paramref name="array"/>" нижняя граница. </exception>
    '''  cref="System.Array"/>
    '''  </remarks>
    Public Sub CopyTo(ByVal array As ToolboxItem(), ByVal index As Integer)
        List.CopyTo(array, index)
    End Sub

    '''  <summary>
    '''   Возвращает индекс [see cref="ToolboxLibrary.ToolboxItem"/> в 
    '''        [see cref="ToolboxLibrary.ToolboxItemCollection"/> .
    '''  </summary>
    '''  <param name="value">The [see cref="ToolboxLibrary.ToolboxItem"/> to locate.</param>
    '''  <returns>
    '''  Индекс [see cref="ToolboxLibrary.ToolboxItem"/> of <paramref name="value"/> в
    '''  [see cref="ToolboxLibrary.ToolboxItemCollection"/>, если найден; иначе, -1.
    '''  </returns>
    '''  <remarks>[seealso cref="ToolboxLibrary.ToolboxItemCollection.Contains"/></remarks>
    Public Function IndexOf(ByVal value As ToolboxItem) As Integer
        Return List.IndexOf(value)
    End Function

    '''  <summary>
    '''  Включить cref="ToolboxLibrary.ToolboxItem"/> внутрь cref="ToolboxLibrary.ToolboxItemCollection"/> по индексу.
    '''  </summary>
    '''  <param name="index">С нуля начинающийся индекс массива <paramref name="value"/> должен быть вставлен.</param>
    '''  <param name=" value">Значение cref="ToolboxLibrary.ToolboxItem"/> для вставки.</param>
    '''  <remarks> cref="ToolboxLibrary.ToolboxItemCollection.Add"/></remarks>
    Public Sub Insert(ByVal index As Integer, ByVal value As ToolboxItem)
        List.Insert(index, value)
    End Sub

    '''  <summary>
    '''    Возвращает перечислитель для иттерации сквозь 
    '''       cref="ToolboxLibrary.ToolboxItemCollection"/> .
    '''  </summary>
    '''  <returns>Перечеслитель коллекции</returns>
    '''  <remarks> cref="System.Collections.IEnumerator"/></remarks>
    Public Shadows Function GetEnumerator() As ToolboxItemEnumerator
        Return New ToolboxItemEnumerator(Me)
    End Function

    '''  <summary>
    '''     Возвращение специфицированного cref="ToolboxLibrary.ToolboxItem"/> из 
    '''    [see cref="ToolboxLibrary.ToolboxItemCollection"] .
    '''  </summary>
    '''  <param name="value">Экземпляр cref="ToolboxLibrary.ToolboxItem"/> для удаления cref="ToolboxLibrary.ToolboxItemCollection"/> .</param>
    '''  <remarks><exception cref="System.ArgumentException"><paramref name="value"/> не найден в коллекции. </exception></remarks>
    Public Sub Remove(ByVal value As ToolboxItem)
        List.Remove(value)
    End Sub

    Public Class ToolboxItemEnumerator
        Inherits Object
        Implements IEnumerator

        Private baseEnumerator As IEnumerator

        Private temp As IEnumerable

        Public Sub New(ByVal mappings As ToolboxItemCollection)
            temp = DirectCast((mappings), IEnumerable)
            baseEnumerator = temp.GetEnumerator()
        End Sub

        Public ReadOnly Property Current() As ToolboxItem
            Get
                Return DirectCast((baseEnumerator.Current), ToolboxItem)
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
''' ToolboxItem.
''' </summary>
Public Class ToolboxItem

    Public Sub New()
    End Sub

    Public Property Name() As String = Nothing

    Public Property Type() As Type = Nothing

End Class
