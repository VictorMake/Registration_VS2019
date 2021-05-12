Imports System.Collections.Generic

''' <summary>
''' Методы расширения для обработки исключений
''' </summary>
''' <remarks></remarks>
Module Methods
    ''' <summary>
    ''' На основании коллекции записей из элементов считанных из XML документа с необработанными исключениями создать таблицу
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToDataTable(Of T)(ByVal value As IEnumerable(Of T)) As DataTable
        Dim returnTable As New DataTable
        Dim firstRecord = value.First

        ' колонки таблицы создаются автоматически из первой записи
        ' свойства записи содержат имя и тип, что необходимо для конструктора колонки
        ' пройти по свойствам Типа первой записи и получить имя и тип свойства, а по ним добавить колонки с нужным типом
        For Each pi In firstRecord.GetType.GetProperties ' получить тип и все его свойства
            returnTable.Columns.Add(pi.Name, pi.GetValue(firstRecord, Nothing).GetType)
        Next

        ' по строкам коллекции
        For Each result In value
            Dim nr = returnTable.NewRow
            ' по свойствам как по колонкам 
            For Each pi In result.GetType.GetProperties
                nr(pi.Name) = pi.GetValue(result, Nothing)
            Next
            returnTable.Rows.Add(nr)
        Next

        Return returnTable
    End Function

    ''' <summary>
    ''' Когда имена колонок известны (из полей записи документа), то легко сделать сообщение из текущей записи.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Diagnostics.DebuggerStepThrough()>
    <System.Runtime.CompilerServices.Extension()>
    Public Function CurrentRowTraceText(ByVal sender As BindingSource) As String
        'Return CType((CType(sender.Current, DataRowView)).Row, DataRow).Item("Trace").ToString
        ' привести текущую запись BindingSource к DataRowView, а затем привести к DataRow. Далее извлечь из записи значение колонки по имени.
        Dim TraceText As String = String.Empty
        TraceText &= CType((CType(sender.Current, DataRowView)).Row, DataRow).Item("Stamp").ToString
        TraceText &= vbCrLf & CType((CType(sender.Current, DataRowView)).Row, DataRow).Item("Text").ToString
        TraceText &= vbCrLf & CType((CType(sender.Current, DataRowView)).Row, DataRow).Item("Trace").ToString

        Return TraceText
    End Function
End Module