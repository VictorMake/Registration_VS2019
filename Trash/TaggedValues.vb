'Friend Class TaggedValues
'	Private mstrSeparator As String
'	Private mcolItems As Collection

'    Public Sub New()
'        MyBase.New()
'        ' Assume ";" as the separator.
'        mstrSeparator = ";"
'        mcolItems = New Collection()
'    End Sub

'    Protected Overrides Sub Finalize()
'        mcolItems = Nothing
'        MyBase.Finalize()
'    End Sub

'    Public ReadOnly Property Count() As Integer
'        Get
'            Count = mcolItems.Count()
'        End Get
'    End Property

'    Public Property Separator() As String
'        Get
'            Separator = mstrSeparator
'        End Get
'        Set(ByVal Value As String)
'            mstrSeparator = Value
'        End Set
'    End Property

'    Public ReadOnly Property Exists(ByVal Tag As String) As Boolean
'        Get
'            ' Эта специфическая отметка уже существует в
'            ' Эта коллекция отметок?
'            On Error Resume Next
'            Dim strValue As String
'            ', если Вы можете восстановить(отыскивать) значение, отметка уже
'            ' Существует.
'            strValue = mcolItems.Item(Tag).Value
'            Exists = (Err.Number = 0)
'            Err.Clear()
'        End Get
'    End Property

'    Public Property Text() As String
'        Get
'            Dim tv As TaggedValue
'            Dim strOut As String = Nothing
'            ' Цикл через каждый элемент(пункт) в коллекции.
'            ' Для любого, которые имеют не-пустые свойства Отметки
'            ' (И они все действительно должны), гвоздь(путь) на
'            ' Tag=Value пары.
'            For Each tv In mcolItems
'                If Len(tv.Tag) > 0 Then
'                    strOut = strOut & mstrSeparator & tv.Tag & "=" & tv.Value
'                End If
'            Next tv
'            ', если имеется что - нибудь в строке вывода,
'            ' Это будет иметь ведущий разделитель. Удалите это теперь.
'            If Len(strOut) > Len(mstrSeparator) Then
'                strOut = Mid(strOut, Len(mstrSeparator) + 1)
'            End If
'            Text = strOut
'        End Get
'        Set(ByVal Value As String)
'            ' Анализируйте все значения из в
'            ' Текст / значение пары.

'            ' Ищите mstrSeparator, и разбиться
'            ' Строка в пары =y. Тогда синтаксический анализ
'            ' Каждый из тех, ища знаки "=".
'            Dim varItems() As String ' As Variant
'            Dim varText() As String ' As Variant
'            Dim I As Short
'            Dim strTag As String
'            Dim strValue As String

'            varItems = Split(Value, mstrSeparator)
'            For I = LBound(varItems) To UBound(varItems)
'                varText = Split(varItems(I), "=")
'                ', если не имеется никаких элементов(пунктов), UBound будет -1.
'                If UBound(varText) > LBound(varText) Then
'                    strTag = varText(LBound(varText))
'                    strValue = varText(UBound(varText))
'                    Add(strTag, strValue)
'                End If
'            Next I
'        End Set
'    End Property

'    Private Sub ParseItems(ByRef strText As String)
'        ' Ищите mstrSeparator, и разбейте(прекратите) строку в
'        ' X=y пары. Тогда анализируйте каждый из тех, просмотра
'        ' Для знаков "=".
'        Dim varItems() As String ' As Variant
'        Dim varText() As String ' As Variant
'        Dim I As Short
'        Dim strTag As String
'        Dim strValue As String

'        varItems = Split(strText, mstrSeparator)
'        For I = LBound(varItems) To UBound(varItems)
'            varText = Split(varItems(I), "=")
'            strTag = varText(LBound(varText))
'            strValue = varText(UBound(varText))
'            Add(strTag, strValue)
'        Next I
'    End Sub

'    Public Function Item(ByRef Tag As String) As String
'        If Exists(Tag) Then
'            Return mcolItems.Item(Tag).Value
'        End If
'        Return String.Empty
'    End Function

'    Public Sub Remove(ByRef Tag As String)
'        ' Проверьте(отметьте) сначала, чтобы удостовериться, что  отметка существует.
'        ', если так, удалите это.
'        If Exists(Tag) Then
'            mcolItems.Remove(Tag)
'        End If
'    End Sub

'    Public Function Add(ByRef Tag As String, ByRef Value As String) As TaggedValue
'        Dim tv As TaggedValue
'        ', если отметка уже существует, удалить
'        ' Это так Вы может устанавливать новое значение.
'        Remove(Tag)

'        ' Создайте новый объект(цель) TaggedValue, установите его
'        ' Свойства, и добавляют это к коллекции.
'        tv = New TaggedValue()
'        tv.Tag = Tag
'        tv.Value = Value
'        mcolItems.Add(tv, Tag)
'        Add = tv
'    End Function
'End Class