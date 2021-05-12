Imports System.Xml.Linq

Public Class FormExceptionLogViewer
    Public ExceptionLogFile As String = ""
    WithEvents bsExceptions As New BindingSource
    Private Document As New XDocument

    ''' <summary>
    ''' Документ XDocument по пользовательскому расширению возвращается как DataTable
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' DataGridView1 columns особенность
    ''' Колонка свойств данных для обоих колонок сделана элементом на форме
    ''' </remarks>
    Private Sub FormExceptionLogViewer_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If IO.File.Exists(ExceptionLogFile) Then
            Document = XDocument.Load(ExceptionLogFile)

            ' получить таблицу из перечислителя используя метод расширени
            ' перечислитель создан из документа как коллекция анонимного типа с полями <Stamp> <Text> <Trace>
            ' пройти по узлам <Exception> и на основании узлов Date_Time, Message, StackTrace создать анонимный тип
            Dim dtExceptions = (From X In Document...<Exception>
                                Select New With
                                       {
                                           .Stamp = X.<Date_Time>.Value,
                                           .Text = X.<Message>.Value,
                                           .Trace = X.<StackTrace>.Value
                                       }
                                   ).ToDataTable

            If dtExceptions.Rows.Count > 0 Then
                DataGridView1.AutoGenerateColumns = False
                bsExceptions.DataSource = dtExceptions
                DataGridView1.DataSource = bsExceptions ' для таблицы источник данных назначить из BindingSource
                DataGridView1.Columns("ExceptionColumn").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

                Text = "Просмотр необработанных исключений в " & ExceptionLogFile
                bsExceptions_PositionChanged(bsExceptions, Nothing)
                ActiveControl = DataGridView1
            Else
                ActiveControl = cmdClose
            End If
        Else
            ActiveControl = cmdClose
        End If
    End Sub

    Private Sub bsExceptions_PositionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles bsExceptions.PositionChanged
        ' изменить символ окончания строки символом новой строки для красивого форматирования
        txtStackTrace.Text = bsExceptions.CurrentRowTraceText.Replace(Chr(10), Environment.NewLine)
    End Sub

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Close()
    End Sub
End Class