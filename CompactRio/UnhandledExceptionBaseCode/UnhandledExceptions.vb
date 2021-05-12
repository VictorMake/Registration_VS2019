Imports System.Xml.Linq
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ''' <summary>
    ''' Дополнение к классу MyApplication
    ''' Расширяет его дополнительными свойствами
    ''' </summary>
    ''' <remarks></remarks>
    Partial Friend Class MyApplication

        ''' <summary>
        ''' Иникатор запущено под IDE или нет
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RunningUnderDebugger() As Boolean
            Get
                Return Debugger.IsAttached
            End Get
        End Property

        ''' <summary>
        ''' Путь к XML файлу с необработанными исключениями находится в корневой папке программы и устанавлиивается при запуске
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnhandledExceptionsFileName() As String

        ''' <summary>
        ''' Специфицирует использование иконки для диалога исключения как копию основного окна приложения
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExceptionDialogIcon() As Icon

        Private mContinueAfterException As Boolean
        Public Event UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)

        ''' <summary>
        ''' Определяет может ли приложение оставаться открытым после того,
        ''' как необработанное исключение было вызванно
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Не практично в большинстве обстоятельств, когда не известно,
        ''' как руководить необработанными исключениями и покидать программу не зная причины исключения
        ''' </remarks>
        Public Property ContinueAfterException() As Boolean
            Get
                Return Not mContinueAfterException
            End Get
            Set(ByVal value As Boolean)
                mContinueAfterException = value
            End Set
        End Property

        ''' <summary>
        ''' Вывести дружественное сообщение относительно необработанного исключения в приложении
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' Так же было бы благоразумно записать лог файл.
        ''' Если использовать код склонный к ошибкам, неразумно проводить долгое тестирование,
        ''' пока не будет найдено исправление.
        ''' </remarks>
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ' если имя файла не назначено, применить по умолчанию
            If String.IsNullOrEmpty(UnhandledExceptionsFileName) Then
                UnhandledExceptionsFileName = "unHandledExceptions.xml"
            End If

            Dim xDoc As New XDocument
            ' если файл уже создан, то загрузить его, в противном случае создать заново
            If IO.File.Exists(My.Application.UnhandledExceptionsFileName) Then
                xDoc = XDocument.Load(My.Application.UnhandledExceptionsFileName)
            Else
                ' создать новый документ из строки
                xDoc = XDocument.Parse("<?xml version=""1.0"" encoding=""utf-8""?><Exceptions></Exceptions>")
            End If

            Dim content = xDoc.<Exceptions>(0) ' найти первый элемент под именем Exceptions
            Dim stackTraceText As String = e.Exception.StackTrace.ToString ' текст стека исключения взять из события
            ' добавить новый элемент исключения
            content.Add( _
            <Exception>
                <Date_Time><%= Now.ToString("MM/dd/yyyy HH:mm") %></Date_Time>
                <Message><%= e.Exception.Message %></Message>
                <StackTrace><%= Environment.NewLine %>
                    <%= stackTraceText %><%= Environment.NewLine %>
                </StackTrace>
            </Exception>)

            ' сохранить файл
            content.Save(My.Application.UnhandledExceptionsFileName)

            ' заполнить свойства окна
            Dim f As New FormExceptionDialog With {
                .InBoundException = e.Exception,
                .Message = $"Исключение было записано в [{My.Application.UnhandledExceptionsFileName}]",
                .Header = "Необработанное исключение"
            }

            Try
                f.ShowDialog()
            Finally
                f.Dispose()
            End Try

            ' должно ли приложение завершать работу при выходе из обработчика исключения.
            e.ExitApplication = ContinueAfterException

            If ContinueAfterException Then
                ' прекратить обрабатывать исключения (до исправления первого по стеку)
                ContinueAfterException = False
            End If
        End Sub
    End Class
End Namespace