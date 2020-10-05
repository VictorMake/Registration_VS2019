'Imports System.Collections.Generic

'Friend Class CollectionArrow
'    Implements IEnumerable
'    Implements IList


'    ' Локальная переменная содержащая коллекцию
'    Private listArrow As List(Of Arrow)

'    Public Sub New()
'        listArrow = New List(Of Arrow)
'    End Sub

'    Public ReadOnly Property Arrows() As List(Of Arrow)
'        Get
'            Return listArrow
'        End Get
'    End Property

'    ''' <summary>
'    ''' отсюда убрал Default и вставил в Item2 чтобы по индексу было возможно перечисление
'    ''' </summary>
'    ''' <param name="index"></param>
'    ''' <returns></returns>
'    Public Overloads Property Item2(ByVal index As Integer) As Object Implements IList.Item
'        Get
'            Return listArrow.Item(index)
'        End Get
'        Set(ByVal value As Object)
'            listArrow.Item(index) = value
'        End Set
'    End Property

'    ''' <summary>
'    ''' Расширенный доступ по индексу
'    ''' </summary>
'    ''' <param name="index"></param>
'    ''' <returns></returns>
'    Default Public Property Item(ByVal index As Integer) As Arrow
'        Get
'            Return listArrow.Item(index)
'        End Get
'        Set(ByVal value As Arrow)
'            listArrow.Item(index) = value
'        End Set
'    End Property

'    Public ReadOnly Property Count As Integer Implements ICollection.Count
'        Get
'            Return listArrow.Count()
'        End Get
'    End Property

'    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
'        Return listArrow.GetEnumerator
'    End Function

'    Public Sub RemoveAt(ByVal index As Integer) Implements IList.RemoveAt
'        listArrow.RemoveAt(index)
'    End Sub

'    Public Sub Remove(ByVal value As Object) Implements IList.Remove
'        Dim tempСтрелка As Arrow = CType(value, Arrow)
'        If listArrow.Contains(tempСтрелка) Then
'            listArrow.Remove(tempСтрелка)
'        End If
'    End Sub

'    Public Sub Clear() Implements IList.Clear
'        listArrow.Clear()
'    End Sub

'    Protected Overrides Sub Finalize()
'        listArrow = Nothing
'        MyBase.Finalize()
'    End Sub

'    Public Function Add1(ByVal value As Object) As Integer Implements IList.Add
'        Dim tempСтрелка As Arrow = CType(value, Arrow)
'        listArrow.Add(tempСтрелка)
'    End Function

'    Public Function Add() As Arrow
'        'If Not ПроверкаПовтора(Key) Then
'        '    Return Nothing
'        'End If
'        'Dim Key As Integer = mCollectionsСтрелок.Count
'        Dim tempСтрелка = New Arrow '(Key)
'        listArrow.Add(tempСтрелка)

'        Return tempСтрелка
'    End Function

'    'Private Function ПроверкаПовтора(ByVal Key As String) As Boolean
'    '    If mCollectionsСтрелок.ContainsKey(Key) Then
'    '        MessageBox.Show("Стрелка " & Key & " в коллекции уже существует!", "Ошибка добавления нового стрелки", MessageBoxButtons.OK, MessageBoxIcon.Information)
'    '        Return False
'    '    End If
'    '    Return True
'    'End Function

'    Public Sub CopyTo(ByVal array As Array, ByVal index As Integer) Implements ICollection.CopyTo
'        listArrow.CopyTo(array, index)
'    End Sub

'    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
'        Get
'            Return True
'        End Get
'    End Property

'    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
'        Get
'            Return Me ' может и не правильно
'        End Get
'    End Property

'    Public Function Contains(ByVal value As Object) As Boolean Implements IList.Contains
'        If listArrow.Contains(CType(value, Arrow)) Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function

'    Public Function IndexOf(ByVal value As Object) As Integer Implements IList.IndexOf
'        Return listArrow.IndexOf(value)
'    End Function

'    Public Sub Insert(ByVal index As Integer, ByVal value As Object) Implements IList.Insert
'        listArrow.Insert(index, CType(value, Arrow))
'    End Sub

'    Public ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
'        Get
'            Return False
'        End Get
'    End Property

'    Public ReadOnly Property IsReadOnly As Boolean Implements IList.IsReadOnly
'        Get
'            Return False
'        End Get
'    End Property
'End Class