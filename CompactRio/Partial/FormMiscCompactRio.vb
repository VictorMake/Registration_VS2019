''' <summary>
''' Разделённый класс основной формы.
''' Создан для отслеживания необработанных исключений при запуске программы.
''' </summary>
''' <remarks></remarks>
Partial Class FormCompactRio
    Private ReadOnly ExceptionLogFile As String = ""

    Public Sub New()
        InitializeComponent()
        My.Application.UnhandledExceptionsFileName = "unHandledExceptions.xml"
        ExceptionLogFile = My.Application.UnhandledExceptionsFileName
        cmdViewLog.Enabled = IO.File.Exists(ExceptionLogFile) ' меню просмотра файла исключения доступна только при наличии файла
    End Sub

    Private Sub CompactRioForm_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
        If My.Application.RunningUnderDebugger Then
            MessageBox.Show(Me, "Рабочая версия (.exe) должна запускаться посредством <Window's Explorer>", "Запуск под Visual Studio", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            My.Application.ExceptionDialogIcon = Icon
        End If
    End Sub
End Class