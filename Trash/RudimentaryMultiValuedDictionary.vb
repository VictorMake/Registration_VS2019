Imports System.Collections.Generic

Public Class RudimentaryMultiValuedDictionary(Of TKey, TValue)
    Implements IEnumerable(Of KeyValuePair(Of TKey, List(Of TValue)))

    Private internalDictionary As Dictionary(Of TKey, List(Of TValue)) = New Dictionary(Of TKey, List(Of TValue))()

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, List(Of TValue))) Implements IEnumerable(Of KeyValuePair(Of TKey, List(Of TValue))).GetEnumerator
        Return internalDictionary.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return internalDictionary.GetEnumerator()
    End Function

    Default Public Property Item(ByVal key As TKey) As List(Of TValue)
        Get
            Return internalDictionary(key)
        End Get
        Set(ByVal value As List(Of TValue))
            Add(key, value)
        End Set
    End Property

    Public Sub Add(ByVal key As TKey, ParamArray values As TValue())
        Add(key, CType(values, IEnumerable(Of TValue)))
    End Sub

    Public Sub Add(ByVal key As TKey, ByVal values As IEnumerable(Of TValue))
        Dim storedValues As List(Of TValue) = Nothing
        If Not internalDictionary.TryGetValue(key, storedValues) Then
            'internalDictionary.Add(key, CSharpImpl.__Assign(storedValues, New List(Of TValue)()))
            storedValues = New List(Of TValue)()
            internalDictionary.Add(key, storedValues)
        End If
        storedValues.AddRange(values)
    End Sub

    'Private Class CSharpImpl
    '    <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
    '    Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
    '        target = value
    '        Return value
    '    End Function
    'End Class

    ' --- Использование: ------------------------------------------------------
    'Public Sub Main()
    '    Dim rudimentaryMultiValuedDictionary1 As RudimentaryMultiValuedDictionary(Of String, String) = New RudimentaryMultiValuedDictionary(Of String, String)() From {
    '        {"Group1", "Bob", "John", "Mary"},
    '        {"Group2", "Eric", "Emily", "Debbie", "Jesse"}
    '    }

    '    Dim rudimentaryMultiValuedDictionary2 As RudimentaryMultiValuedDictionary(Of String, String) = New RudimentaryMultiValuedDictionary(Of String, String)() From {
    '        {"Group1", New List(Of String) From {"Bob", "John", "Mary"}},
    '        {"Group2", New List(Of String) From {"Eric", "Emily", "Debbie", "Jesse"}}
    '    }

    '    Dim rudimentaryMultiValuedDictionary3 As RudimentaryMultiValuedDictionary(Of String, String) = New RudimentaryMultiValuedDictionary(Of String, String)() From {
    '                {"Group1", New String() {"Bob", "John", "Mary"}},
    '                {"Group2", New String() {"Eric", "Emily", "Debbie", "Jesse"}}
    '    }

    '    Console.WriteLine("Using first multi-valued dictionary created with a collection initializer:")

    '    For Each group As KeyValuePair(Of String, List(Of String)) In rudimentaryMultiValuedDictionary1
    '        Console.WriteLine($"{System.Environment.NewLine}Members of group {group.Key}: ")

    '        For Each member As String In group.Value
    '            Console.WriteLine(member)
    '        Next
    '    Next

    '    Console.WriteLine(vbCrLf & "Using second multi-valued dictionary created with a collection initializer using indexing:")

    '    For Each group As KeyValuePair(Of String, List(Of String)) In rudimentaryMultiValuedDictionary2
    '        Console.WriteLine($"{System.Environment.NewLine}Members of group {group.Key}: ")

    '        For Each member As String In group.Value
    '            Console.WriteLine(member)
    '        Next
    '    Next

    '    Console.WriteLine(vbCrLf & "Using third multi-valued dictionary created with a collection initializer using indexing:")

    '    For Each group As KeyValuePair(Of String, List(Of String)) In rudimentaryMultiValuedDictionary3
    '        Console.WriteLine($"{System.Environment.NewLine}Members of group {group.Key}: ")

    '        For Each member As String In group.Value
    '            Console.WriteLine(member)
    '        Next
    '    Next
    'End Sub
End Class


