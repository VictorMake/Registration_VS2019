Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

Public Class FormSplash

    Private Sub frmSplash_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        Me.Opacity = 0
        lblVersion.Text = $"Версия: {My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}.{My.Application.Info.Version.Build}"
        'lblVersion.Text = $"Версия {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly.Location).FileMajorPart}.{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly.Location).FileMinorPart} {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly.Location).ProductVersion()}"
        lblProductName.Text = Assembly.GetExecutingAssembly.GetName.Name
        ' Информация об авторских правах
        lblLicenseTo.Text = My.Application.Info.Copyright
        lblCompanyProduct.Text = My.Application.Info.CompanyName

        'Dim phkРезультат As Integer
        'Dim slValue As String = "Keys"
        'Dim slAllow As String = CStr(1)
        'Dim slTimesRemaining As String
        'Dim rc As Boolean

        '' Основной раздел реестра
        'NativeMethods.hKey = NativeMethods.HKEY_CURRENT_USER

        '' Подраздел для хранения данных
        'NativeMethods.SubKey = "SOFTWARE\Prosoft\" & "Iconics\Genesis32"

        'If NativeMethods.RegOpenKeyEx(NativeMethods.hKey, NativeMethods.SubKey, 0, 1, phkРезультат) = NativeMethods.ERROR_SUCCESS Then

        '    '' Проверить, существует ли отмеченный номер
        '    'glРегистрационныйНомер = GetRegValue(hKey, SubKey, "Number", "Not Found")
        '    'If Len(glРегистрационныйНомер) > 0 Then

        '    '    'Копия отмечена, завершить проверку
        '    '    '            lblWarning = "Отмеченная копия"
        '    '    glЗарегистрирована = True
        '    '    strНадпись = conЗАРЕГИСТРИРОВАНА
        '    '    Exit Sub
        '    'End If

        '    ' занести случайное число
        '    Randomize()
        '    ' генерировать случайное число между 1 и 1000000
        '    If Not NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, "Number", CStr(CInt(Int((1000000 * Rnd()) + 1)))) Then
        '        MessageBox.Show("Не удалось задать значение параметра.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Else
        '        CryptoSample = New CheckCrypto("TripleDES") With {.SourceFileName = PathCryptoSource, .SaltIVFile = PathCryptoCurrentKeyFile}
        '        Dim strTemp As String = CryptoSample.CryptoOutput
        '        CryptoSample.EncryptDecrypt(False) ' расшифровать
        '        'TextBox1.Text = strTemp

        '        CryptoSample.EncryptDecrypt(True) ' и сразу зашифровать
        '        'txtCrypto.Text = crpSample.CryptoOutput

        '        If CryptoSample.Current = strTemp Then
        '            'MessageBox.Show("OK", "Проверка текущего и записанного", MessageBoxButtons.OK)
        '            NativeMethods.glRgstrdOk = True
        '            NativeMethods.Notice = NativeMethods.conRgstrd
        '            Exit Sub
        '            'Else
        '            '    MessageBox.Show("NO", "Проверка текущего и записанного", MessageBoxButtons.OK)
        '        End If
        '    End If

        '    ' Раздел существует - следовательно, программа запускается
        '    ' не в первый раз
        '    ' проверка количества оставшихся запусков
        '    slTimesRemaining = NativeMethods.GetRegValue(NativeMethods.hKey, NativeMethods.SubKey, "Keys", "Not Found")
        '    ' это создание не нужно здесь но была какая-то хрень с потерей раздела "Keys"
        '    If slTimesRemaining <> "Not Found" Then
        '        rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, slValue, slAllow)
        '    End If

        '    If Val(CStr(CDbl(slTimesRemaining) - 1)) <= 0 Then
        '        NativeMethods.Notice = NativeMethods.conUnRgstrd
        '        NativeMethods.glRgstrdOk = False
        '    Else
        '        ' разрешенные запуски еще остались, вычесть 1
        '        rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, slValue, CStr(CDbl(slTimesRemaining) - 1))
        '        NativeMethods.glRgstrdOk = True
        '        NativeMethods.Notice = NativeMethods.conRgstrdTmp
        '    End If
        'Else
        '    ' Раздела не существует - программа запускается впервые.
        '    ' создать новый раздел
        '    NativeMethods.glRgstrdOk = False

        '    If NativeMethods.CreateRegKey(NativeMethods.SubKey) Then
        '        ' если раздел создан, создать параметр для количества запусков
        '        rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, slValue, slAllow)
        '        'rc = CBool(vbNullString)
        '        rc = NativeMethods.SetRegValue(NativeMethods.hKey, NativeMethods.SubKey, "Number", CStr(CInt(Int((1000000 * Rnd()) + 1))))
        '        NativeMethods.Notice = NativeMethods.conUnRgstrd
        '    Else
        '        MessageBox.Show("Не удалось создать раздел.", "Регистрация при первом запуске", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    End If

        '    Exit Sub
        'End If
    End Sub

    ''' <summary>
    ''' синхронный вызов метода заставки
    ''' </summary>
    Public Sub UpdateStatus()
        ProgressBar1.Value = 0
        For I As Integer = 1 To 100
            UpdateProgressBar(I)
            Text = $"{I}%"
            Opacity = I / 100
            Refresh()
            Thread.Sleep(100)
        Next
        ProgressBar1.Value = 0
        Thread.Sleep(200)
    End Sub

    Public Sub UpdateStatusRevers()
        For I As Integer = 100 To 0 Step -1
            Opacity = I / 100
            Refresh()
            Thread.Sleep(20)
        Next
    End Sub

    ''' <summary>
    ''' асинхронный вызов метода заставки
    ''' </summary>
    Public Async Sub UpdateStatusAsync()
        Await UpdateStatusTask() '.ConfigureAwait(True)
        ProgressBar1.Value = 0
        Thread.Sleep(200)
        'Me.Close()
    End Sub

    ''' <summary>
    ''' возврат задачи с анонимной процедурой
    ''' </summary>
    ''' <returns></returns>
    Private Function UpdateStatusTask() As Tasks.Task
        Return Task.Run(Sub()
                            For I As Integer = 1 To 100
                                Dim progress As Integer = I
                                Me.Invoke(New System.Action(Sub()
                                                                UpdateProgressBar(progress)
                                                                Text = $"{progress}%"
                                                                Opacity = progress / 100
                                                                Refresh()
                                                                Thread.Sleep(100)
                                                                'Await Task.Delay(1000).ConfigureAwait(False)
                                                            End Sub))
                            Next
                        End Sub)
        'Return Task.Run(Sub() UpdateStatus())
    End Function

    '''' <summary>
    '''' процедура для задачи
    '''' </summary>
    '''' <returns></returns>
    'Private Sub UpdateStatus()
    '    For I As Integer = 1 To 100
    '        Dim progress As Integer = I
    '        Me.Invoke(New System.Action(Sub()
    '                                        UpdateProgressBar(progress)
    '                                        Text = $"{progress}%"
    '                                        Opacity = progress / 100
    '                                        Refresh()
    '                                        Thread.Sleep(100)
    '                                        'Await Task.Delay(1000).ConfigureAwait(False)
    '                                    End Sub))
    '        'System.Threading.Thread.Sleep(20)
    '    Next
    'End Sub

    ''' <summary>
    ''' устранить некорректную работу ProgressBar
    ''' </summary>
    ''' <param name="i"></param>
    Private Sub UpdateProgressBar(ByVal i As Integer)
        If i = ProgressBar1.Maximum Then
            ProgressBar1.Maximum = i + 1
            ProgressBar1.Value = i + 1
            ProgressBar1.Maximum = i
        Else
            ProgressBar1.Value = i + 1
        End If
        ProgressBar1.Value = i
    End Sub
End Class