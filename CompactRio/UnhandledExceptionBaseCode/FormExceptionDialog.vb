Imports System.Text.RegularExpressions

''' <summary>
''' Пользовательское диалоговое окно с текстом исключения
''' </summary>
''' <remarks></remarks>
Public Class FormExceptionDialog

    ''' <summary>
    ''' Куда записано сообщение
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Message() As String

    ''' <summary>
    ''' Заголовок окна
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Header() As String

    ''' <summary>
    ''' Текст исключения
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InBoundException() As Exception

    Public Sub New()
        InitializeComponent()

        Try
            If My.Application.ExceptionDialogIcon IsNot Nothing Then
                Icon = My.Application.ExceptionDialogIcon
            End If
        Catch ex As Exception
            MessageBox.Show($"Глюка с Icon в тесте:{System.Environment.NewLine}{ex.ToString}", $"{NameOf(FormExceptionDialog)}", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub frmExceptionDialog_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim timeStamp As String = String.Format(Date.Now.ToString, "MM/dd/yyyy hh:mm:ss")

        ' сформировать XML текст сообщения разбив строку стека исключения
        Dim exceptionMessage = _
           <html>
               <style>
                  p {margin-bottom:10px;padding-bottom:10px;text-align:left;}
                  H3 {text-align:center;font-weight: bold;}
                  table, th, td {border: 1px solid #D4E0EE;border-collapse: collapse;color: #555;width:100%;padding-left:10px;}
                  caption {font-size: 100%;font-weight: bold;margin: 5px;caption-side:top;text-align:left;}
                  td, th {padding: 4px;}
                  thead th {text-align: center;background: #E6EDF5;color: #4F76A3;font-size: 100% !important;}
                  tbody th {font-weight: bold;}
                  tfoot th, tfoot td {font-size: 85%;}
                </style>
               <body style='margin: 0px;padding: 4px;background: #E6EDF5;'>
                   <H3><%= Header %></H3>
                   <p><%= Message %></p>

                   <table>
                       <caption>Stack Trace</caption>
                       <%= _
                           From T In Regex.Split(StackTracer.EnhancedStackTrace(InBoundException), "{sep}") _
                           Select <tr><td><%= T %></td></tr> _
                       %>
                   </table>
                   <p>Дата/Время исключения: <%= timeStamp %><br/>Пожалуйста обратитесь за помощью: Бригада автоматизации ЭИО-21 тел. (495)552-92-21 </p>
               </body>
           </html>

        WebBrowser1.DocumentText = exceptionMessage.ToString
    End Sub

    Private Sub CmdClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Close()
    End Sub

    Private Sub CmdCopyTextToClipboard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCopyTextToClipboard.Click
        Clipboard.SetText(WebBrowser1.Document.Body.InnerText)
    End Sub
End Class