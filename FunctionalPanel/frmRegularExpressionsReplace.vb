Imports Microsoft.Win32
Imports System.Xml
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.IO
Imports System.Collections.Generic

''' <summary>
''' Форма для работы с регулярными выражениями
''' </summary>
''' <remarks></remarks>
Public Class frmRegularExpressionsReplace
    ' tbFind – Искомый текст. Образец для поиска в файлах. 
    '   Может содержать регулярные выражения (подробнее о регулярных выражениях можно прочесть в документации к VS.Net 
    ' (ms-help://MS.VSCC/MS.MSDNVS/cpguide/html/cpconcomregularexpressions.htm – общая информация, 
    ' ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfSystemTextRegularExpressions.htm –программная модель). 
    ' tbReplaceTo – Текст для замены. 
    '       Может содержать так называемые вхождения регулярных выражений ($X. Где X – это порядковый номер вхождения (Mach)).
    ' lblFilesPatern – элемент управления Label с надписью «Шаблон файлов". 
    ' tbExtensions – Расширения файлов, используемые для фильтрации файлов. 
    ' lblPath – элемент управления Label с надписью «Путь». 
    ' tbPath – Путь. Директория в которой будет производиться поиск файлов. 
    ' pbBrowseFolder – Кнопка (с троеточием, находящаяся рядом с tbPath), позволяющая открыть диалог выбора пути. 
    ' cbReplAll и tbReplCount. Количество замен. 
    '       При замене файла может оказаться, что нужно делать только определенное количество замен. 
    '       Например, одну. Это может ускорить работу приложения, так как после замены первого вхождения повторный поиск производиться не будет. 
    '       Если cbReplAll включен, должны заменяться все найденные вхождения. 
    '       Иначе количество замен берется из tbReplCount. 
    ' gbRegExpOptions – GroupBox («Опции регулярного выражения»), содержащий флаги регулярных выражений. 
    '       Это флаги, используемые регулярными выражениями .Net для задания своего поведения. 
    '       Надо дать пользователю возможность настраивать хотя бы самые важные из них. 
    '       Что каждый из них означает: 
    ' cbIgnoreCase – Ignore Case. Игнорировать регистр букв при поиске. 
    ' cbMultiline – Multiline. Воспринимать текст, как набор строк. Если это свойство установлено в TRUE, 
    '       значение спецсимволов «^» и «$» интерпретируется как начало и конец строки, а не как начало и конец текста.
    '       Применяется, если нужно отслеживать и начало/конец текста, и начало/конец отдельных строк (что бывает очень редко). 
    '       Существуют специальные эскейп-последовательности которые не зависят от значения этого флага. 
    ' cbExplicitCapture – Explicit Capture. Не учитывать неименованные (или явно не нумерованные) группы. 
    '       В регулярных выражениях можно, так сказать, захватить группу (часть регулярного выражения), заключив ее в круглые скобки, 
    '       например "(некоторое регулярное выражение)". Захватываемые таким образом вхождения становятся доступными при замене как $x (где x порядковый номер группы). 
    '       Отдельную группу (выражение, заключенное в скобки) можно исключить из захвата, если указать перед открывающей скобкой «?:». 
    '       cbExplicitCapture автоматически исключает все не именованные и не нумерованные вхождения. 
    ' cbSingleline – Single line. Заставляет интерпретировать «.» (подстановочный символ точка), как любой символ, включая «\r\n». 
    '       Если этот флаг не будет указан, то «.» будет находить любой символ, кроме «\r\n». Так, в если этот флаг поднят (и при условии, что Multiline тоже включен,
    '       так как иначе $ не будет означать конца строки), выражение «".*$» будет находить подстроку, начиная с «"» и заканчивая концом файла. 
    '       Это происходит потому, что конструкция «.*» находит и концы строк. Если же этот флаг опустить, то находиться будет текст от символа «"» и до конца строки (включительно). 
    ' cbIgnorePatternWhitespace – Ignore Whitespace. Игнорировать пробелы, которым не предшествует символ «\». 
    '       При этом становятся доступными комментарии в стиле языка Perl (от знака «#» и до конца строки). 
    ' cbCompiled – Compiled. Компилировать выражение в MSIL перед выполнением. 
    '       Это должно давать выигрыш в производительности при осуществлении большого количества сложных замен. 

    ' tvFiles – TreeView, содержащий дерево каталогов и список файлов, соответствующих маске поиска (tbExtensions). 
    '       К tvFiles нужно подключить ImageList, содержащий иконки файлов и каталогов. 
    ' lbFileMatch – ListBox, содержащий список вхождений, найденных в текущем файле (имя которого в данный момент выделено в tvFiles). 
    ' tbFileBody – TextBox, в который загружается содержимое текущего файла. 
    ' tbReplaceResult – TextBox, в котором выводится результат замены вхождения, выделенного в списке lbFileMatch. 
    ' tbReplaceResult – TabControl, позволяющий переключаться между режимом просмотра только текста текущего файла 
    ' и режимом, в котором можно также просматривать результат замены вхождения, выделенного в lbFileMatch. 
    ' В дереве слева выводятся файлы, удовлетворяющие шаблону поиска, заданному в поле «File' s pattern». При этом сама замена не производится. 
    ' Вместо этого найденные вхождения выводятся в списке слева, а ниже, под этим списком, выводится содержимое файла и текст, 
    ' который получится в результате замены выделенного вхождения. Для удобства происходит прокрутка к найденному фрагменту, а сам он выделяется.
    ' Таким образом, пользователь может задавать параметры поиска/замены и интерактивно проверять их на закладке «Просмотр», не боясь испортить файлы.

    '--- константы -----------------------------------------------------------
    Private Const ciPanelMain As Integer = 0
    Private Const ciPanelFileCount As Integer = 1
    Private Const m_csRecentKey As String = "Software\LMZ.RU\AscSearch\Recent"

    '--- переменные -----------------------------------------------------------
    Private m_sCurrentTvPath As String = ""
    Private m_SearchEngine As New AscSearchEngine()
    Private m_sCurrentConfig As String = ""
    Private m_asRecent As String()
    Private m_bIsDirty As Boolean = False
    Private m_bLoading As Boolean = False
    Private m_Rep As AscRegExpParser = Nothing
    Private m_frmBatchRep As New frmBatchReplace() ' Форма в которой выводтится состояние процесса пакетной замены.

    'Const EM_SCROLLCARET As Integer = &HB7
    '<DllImport("User32.dll")> _
    'Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As UInt32, ByVal wParam As Int32, ByVal lParam As Int32) As Integer
    'End Function

    ''' <summary>
    ''' Для хранения информации о найденных вхождениях.
    ''' Класс является всего лишь контейнером для экземпляра класса Match, описывающего найденное вхождение.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class AscMatch
        Private m_m As Match

        Friend ReadOnly Property Match() As Match
            Get
                Return m_m
            End Get
        End Property

        Friend Sub New(ByVal m As Match)
            m_m = m
        End Sub

        ''' <summary>
        ''' Строка, содержащая в своем начале индекс и длину вхождения (в скобках), а в конце саму найденную строку.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function ToString() As String
            Return $"(Индекс={m_m.Index}, Длина={m_m.Length}){vbTab}{m_m}"
        End Function
    End Class

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        PopulateExceptControls()
    End Sub

    ''' <summary>
    ''' Инициализация перед загрузкой формы.
    ''' </summary>
    ''' <param name="sShrFileName"></param>
    ''' <param name="sPath"></param>
    ''' <remarks></remarks>
    Public Sub Init(ByVal sShrFileName As String, ByVal sPath As String)
        Visible = False

        If sShrFileName IsNot Nothing AndAlso sShrFileName.Length > 0 Then
            m_sCurrentConfig = sShrFileName
        Else
            ' Если строка не задана в качестве параметра, считываем ее из рееста.
            Try
                ' Перекрываем ненужные исключения...
                Using rk As RegistryKey = Registry.CurrentUser
                    Using rkAscSearch As RegistryKey = rk.CreateSubKey("Software\LMZ.RU\AscSearch")
                        m_sCurrentConfig = DirectCast(rkAscSearch.GetValue("m_sCurrentConfig"), String)
                    End Using
                End Using
            Catch ex As Exception ' Перекрываем ненужные исключения...
            End Try
        End If

        OpenConfig(m_sCurrentConfig)
        If sPath IsNot Nothing AndAlso sPath.Length > 0 Then
            tbPath.Text = sPath
        End If
    End Sub

    ''' <summary>
    ''' Считывание расположение и размеры главной формы
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try ' Перекрываем ненужные исключения...
            ' Считываем расположение и размеры главной формы.
            Using rk As RegistryKey = Registry.CurrentUser
                Using rkAscSearch As RegistryKey = rk.CreateSubKey("Software\LMZ.RU\AscSearch")
                    Top = CInt(rkAscSearch.GetValue("Top"))
                    Left = CInt(rkAscSearch.GetValue("Left"))
                    Height = CInt(rkAscSearch.GetValue("Height"))
                    Width = CInt(rkAscSearch.GetValue("Width"))
                    WindowState = CType(rkAscSearch.GetValue("WindowState"), FormWindowState)

                    SplitContainerPreviewHor.SplitterDistance = CInt(rkAscSearch.GetValue("splHorPrivew.SplitPosition"))
                    SplitContainerPrivewVert.SplitterDistance = CInt(rkAscSearch.GetValue("splVerPrivew.SplitPosition"))
                    SplitContainerSetting.SplitterDistance = CInt(rkAscSearch.GetValue("splHorSetings.SplitPosition"))
                End Using
            End Using
            ' Считываем список ранее открывавшихся файлов.
            RecentPersistInRegLoad()
        Catch ex As Exception ' Перекрываем ненужные исключения...
        End Try

        OpenConfig(m_sCurrentConfig)
        Visible = True
    End Sub

    ''' <summary>
    ''' Предложение пользователю сохранить изменения конфигурационного файла перед закрытием приложения.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not TrySave()
    End Sub

    ''' <summary>
    ''' Запись расположение и размеры главной формы
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        Using rk As RegistryKey = Registry.CurrentUser
            Using rkAscSearch As RegistryKey = rk.CreateSubKey("Software\LMZ.RU\AscSearch")
                If FormWindowState.Minimized <> WindowState Then
                    rkAscSearch.SetValue("WindowState", CInt(WindowState))
                End If
                If FormWindowState.Normal = WindowState Then
                    rkAscSearch.SetValue("Top", Top)
                    rkAscSearch.SetValue("Left", Left)
                    rkAscSearch.SetValue("Height", Height)
                    rkAscSearch.SetValue("Width", Width)
                End If
                rkAscSearch.SetValue("splHorSetings.SplitPosition", SplitContainerSetting.SplitterDistance)
                rkAscSearch.SetValue("splHorPrivew.SplitPosition", SplitContainerPreviewHor.SplitterDistance)
                rkAscSearch.SetValue("splVerPrivew.SplitPosition", SplitContainerPrivewVert.SplitterDistance)

                rkAscSearch.SetValue("m_sCurrentConfig", m_sCurrentConfig)
                RecentPersistInRegSave()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Позволяет прервать последовательно выполняемые пакетные операции.
    ''' </summary>
    ''' <remarks></remarks>
    Public m_bCancelBatch As Boolean = False

    ''' <summary>
    ''' Позволяет запусть пакетную операцию. При этом выводится только 
    ''' диалог с прогрессом.
    ''' </summary>
    ''' <param name="sShrFileName">Имя кнфигурационного фойла.</param>
    Public Sub Run(ByVal sShrFileName As String, ByVal sPath As String)
        If m_bCancelBatch Then
            Return
        End If
        Init(sShrFileName, sPath)
        ReplaceInFile()
    End Sub

#Region "Kод отвечающий за пакетную замену"
    ' Общая структура пакетной замены выглядит как 
    ' диалоговое окно индикации прогресса создается при инициализации главной формы (точнее, при инициализации ссылки).

    ''' <summary>
    ''' Спросить пользователя на проведение замены
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReplaceInFileWithPromt()
        If MessageBox.Show(Me, "Вы действительно хотите произвести замену?", csAppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            ' сделать копию каталога tbPath.Text
            CopyDirectory()
            ReplaceInFile()
        End If
    End Sub

    ''' <summary>
    ''' сделать копию каталога tbPath.Text
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CopyDirectory()
        Dim sourceDirectory As String = tbPath.Text
        Dim destinationDirectory As String = String.Format("{0}(Copy {1} {2})", sourceDirectory, Date.Now.ToLongDateString, Date.Now.ToLongTimeString.Replace(":", "-"))

        Try
            DirectoryCopy(sourceDirectory, destinationDirectory, True)
        Catch ex As IOException
            MessageBox.Show($"Ошибка при создании резервной копии директории {sourceDirectory} в новом месте {destinationDirectory}{Environment.NewLine}Описание: {ex}",
                            $"Процедура {NameOf(CopyDirectory)}",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)

        End Try
    End Sub

    ''' <summary>
    ''' Скопировать папку из одного места в другое.
    ''' Можно выбирать, следует ли также копировать подкаталоги.
    ''' </summary>
    ''' <param name="sourceDirName"></param>
    ''' <param name="destDirName"></param>
    ''' <param name="copySubDirs"></param>
    ''' <remarks></remarks>
    Private Shared Sub DirectoryCopy(ByVal sourceDirName As String,
                                     ByVal destDirName As String,
                                     ByVal copySubDirs As Boolean)

        Dim dir As DirectoryInfo = New DirectoryInfo(sourceDirName)
        Dim dirs As DirectoryInfo() = dir.GetDirectories()

        If Not dir.Exists Then
            Throw New DirectoryNotFoundException("Директория не существует или не найдена: " & sourceDirName)
        End If

        If Not Directory.Exists(destDirName) Then
            Directory.CreateDirectory(destDirName)
        End If

        Dim files As FileInfo() = dir.GetFiles()
        For Each file In files
            Dim temppath As String = Path.Combine(destDirName, file.Name)
            file.CopyTo(temppath, False)
        Next file

        If copySubDirs Then
            For Each subdir In dirs
                Dim temppath As String = Path.Combine(destDirName, subdir.Name)
                DirectoryCopy(subdir.FullName, temppath, copySubDirs)
            Next subdir
        End If
    End Sub

    ''' <summary>
    ''' Активизирует закладку с Log-ом и открывает окно индикации прогресса в модальном режиме.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReplaceInFile()
        tabControl1.SelectedTab = tpLog
        ' При открытии окна ему передается ссылка на родительское окно. 
        ' С одной стороны, это позволяет сделать новое окно плавающим (popup) над родительским, 
        ' а с другой - с помощью этой ссылки можно обращаться к данным родительского окна.

        'm_frmBatchRep.Родитель = Me
        m_frmBatchRep.ShowDialog(Me)
    End Sub

    Private Sub miRunGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miRunGo.Click
        ReplaceInFileWithPromt()
    End Sub

    ''' <summary>
    ''' Выводит сообщение в лог (закладка Result).
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub BatchReplaseLog(sText As String)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() BatchReplaseLog(sText)))
        Else
            tbLog.Text = (sText & vbCr & vbLf) & tbLog.Text
        End If
    End Sub

    ''' <summary>
    ''' Выводит сообщение в лог (закладка Result) и StatusBar.
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub BatchReplaseReport(ByVal sText As String)
        ' Вывод сообщение в Log (и в StatusBar) на закладке Result. 
        ' Чтобы последние сообщения всегда были видны, она выводит сообщения в обратном порядке, 
        ' т.е. новые сообщения оказываются сверху Log-а.
        SetStatusText(sText)
        BatchReplaseLog(sText)
    End Sub

    ''' <summary>Порядковый номер обрабатываетмого файла.</summary>
    Private m_iCurrFileIndex As Integer

    ''' <summary>
    ''' Количество замен. Если значение меньше или равно нулю,
    ''' производится замена всех вхождений в файле.
    ''' </summary>
    Private m_iReplaceCount As Integer

    ''' <summary>Вызывается для обработки одного (каждого) файла.</summary>
    ''' <param name="sFileName">Имя файла.</param>
    Public Sub ProcessFile(ByVal sFileName As String)
        ' Оповещаем окно прогресса о текущем состоянии работы.
        m_iCurrFileIndex += 1
        Dim sFilePath As String = MakePath(tbPath.Text, sFileName)
        SetStatusText(sFilePath)

        Try
            m_Rep.Replace(sFilePath, m_iReplaceCount)
        Catch ex As UnauthorizedAccessException
            BatchReplaseLog(ex.ToString)
        End Try

        m_frmBatchRep.UpdateInfo(m_iCurrFileIndex)
    End Sub

    ''' <summary>
    ''' Процедура в которой происходит пакетная замена 
    ''' содержимого файлов. Она вызывается из рабочего 
    ''' потока (отличного от основного потока приложения в
    ''' котором работает пользовательский интерфейс). 
    ''' </summary>
    Public Sub BatchReplase()
        ' Далее работа приложения раздваивается. 
        ' Основной поток показывает на экране диалог отображения прогресса и входит в модальный цикл, ожидая пользовательского ввода, 
        ' а рабочий поток приступает к поиску и замене файлов.
        ' Метод вызывается из m_frmBatchRep.ShowDialog(Me) далее Load
        Try
            ' Нужно для перехвата ошибок и завершения обработки методом Abort
            'tbLog.Clear();
            BatchReplaseReport(String.Format("Работа запущена в {0} (Лог пишется в реверсном режиме)." & Environment.NewLine, DateTime.Now))
            BatchReplaseReport("Конфигурационный файл процесса: " & m_sCurrentConfig)
            m_iReplaceCount = If(cbReplAll.Checked, -1, Integer.Parse(tbReplCount.Text)) ' кол. замен
            m_Rep = New AscRegExpParser(tbFind.Text, tbReplaceTo.Text, GetRegExOptions())
            m_iCurrFileIndex = 0
            'm_bStopBatchReplase = false;
            m_SearchEngine.ScanDir(tbPath.Text, tbExtensions.Text)

            SetStatusFileCountText(m_SearchEngine.FilesCount.ToString())
            m_frmBatchRep.SetInfo(m_SearchEngine.FilesCount)

            ' IterateNodes получает в делегате ссылку на метод ProcessFile этой же формы
            m_SearchEngine.IterateNodes(New AscSearchEngine.FileCallback(AddressOf ProcessFile))
            BatchReplaseReport(String.Format(Environment.NewLine & "{0} из {1} файл(ов) обработано.", m_iCurrFileIndex, m_SearchEngine.FilesCount))
        Catch generatedExceptionName As ThreadAbortException
            BatchReplaseReport(String.Format("Работа прервана пользователем. {0} из {1} файл(ов) обработано.", m_iCurrFileIndex, m_SearchEngine.FilesCount))
        Catch ex As Exception
            BatchReplaseReport(String.Format(Environment.NewLine & "Работа прервана в результате ошибки. {0} из {1} файл(ов) обработано.", m_iCurrFileIndex, m_SearchEngine.FilesCount))
            MessageBox.Show($"{Environment.NewLine}Ошибка при пакетной обработке.{Environment.NewLine}Описание: {ex}",
                            csAppName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw 'ex FxCop
        End Try
    End Sub

    ''' <summary>
    ''' Проверяет все CheckBox-ы и возвращает набор флагов RegexOptions, управляющий поведением парсера регулярных выражений.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRegExOptions() As RegexOptions
        Dim o As RegexOptions = RegexOptions.None ' Указывает на отсутствие заданных параметров.

        'if(cbGlobal.Checked) o |= RegexOptions.G
        If cbIgnoreCase.Checked Then
            ' Указывает соответствие, не учитывающее регистр.
            o = o Or RegexOptions.IgnoreCase
        End If
        If cbMultiline.Checked Then
            ' Многострочный режим. Изменяет значение символов "^" и "$" так, что они совпадают, соответственно, 
            ' в начале и конце любой строки, а не только в начале и конце целой строки.
            o = o Or RegexOptions.Multiline
        End If
        If cbExplicitCapture.Checked Then
            ' Указывает, что единственные допустимые записи являются явно поименованными или пронумерованными группами в форме (?<name>…).
            ' Это позволяет непоименованным круглым скобкам выступать в качестве незаписывающих групп без синтаксической неловкости выражения (?:…).
            o = o Or RegexOptions.ExplicitCapture
        End If
        If cbSingleline.Checked Then
            ' Указывает однострочный режим. Изменяет значение точки (.) так, что она соответствует любому символу (вместо любого символа, кроме "\n").
            o = o Or RegexOptions.Singleline
        End If
        If cbIgnorePatternWhitespace.Checked Then
            ' Устраняет из шаблона неизбежные пробелы и включает комментарии, помеченные "#". 
            ' Однако значение System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace не влияет или не устраняет пробелы в классе символов.
            o = o Or RegexOptions.IgnorePatternWhitespace
        End If
        If cbCompiled.Checked Then
            ' Указывает, что регулярное выражение скомпилировано в сборку. Это порождает более быстрое исполнение, но увеличивает время запуска.
            ' Это значение не должно назначаться свойству System.Text.RegularExpressions.RegexCompilationInfo.Options при обращении 
            ' к методу System.Text.RegularExpressions.Regex.CompileToAssembly(System.Text.RegularExpressions.RegexCompilationInfo[],System.Reflection.AssemblyName).
            o = o Or RegexOptions.Compiled
        End If
        Return o
    End Function

#End Region

#Region "Работа со списком недавно открытых файлов"

    ''' <summary>
    ''' Добавляет имя файла в начало Recent-списка. 
    ''' Если он там уже был, элемент перемещается наверх списка.
    ''' </summary>
    Private Sub RecentAdd(ByVal sConfigName As String)
        ' Перед окончанием работы RecentAdd в переменную m_asRecent присваивается ссылка на измененный массив. 
        ' Это может быть и ссылка на старый массив, но ввиду ссылочной природы массивов в .Net в этом нет никакой разницы, 
        ' так как любая переменная является только ссылкой на реальный массив, и ссылок может быть любое количество. 
        sConfigName = sConfigName.ToLower()
        ' Опускаем регистр.
        Dim asNew As String() = Nothing
        ' Если список ранее открываемых файлов пуст, ...
        If m_asRecent Is Nothing OrElse m_asRecent.Length = 0 Then
            asNew = New String(0) {}
        Else
            ' создаем массив с одним элементом.
            ' Иначе добавляем или перемещаем в верх новый путь.
            ' Макс. размер Recent-списка (ранее открываемых файлов).
            Const ciRecentMax As Integer = 30

            ' Ищем, не было ли такого элемента...
            Dim i As Integer = Array.IndexOf(m_asRecent, sConfigName)
            ' Если путь (sConfigName) не найден в массиве, i будет равна -1.
            ' Иначе i будет содержать индекс найденного элемента.
            ' В случае если путь уже имеется в массиве или
            ' размер массива равен максимально допустимому (ciRecentMax), нам не нужно расширять массив.
            ' В противном случае нужно добавить в массив дополнительный элемент. 
            ' (Для этого придется его перезанять.)

            ' Используем старый массив.
            ' Иначе создаем новый, шириной на единицу больше прежнего.
            asNew = If(i >= 0 OrElse m_asRecent.Length >= ciRecentMax, m_asRecent, New String(m_asRecent.Length) {})
            ' Если вставляется новый путь, делаем вид, что он найден 
            ' в последнем (новом) элементе массива.
            If i < 0 Then
                i = asNew.GetUpperBound(0)
            End If
            ' Копируем элементы, тем самым сдвигая их на элемент вниз.
            ' Задняя часть массива остается без изменений.
            Array.Copy(m_asRecent, 0, asNew, 1, i)
        End If
        ' Новый элемент всегда вставляется в начало списка.
        asNew(0) = sConfigName
        m_asRecent = asNew
        RecentUpdate()
    End Sub

    ''' <summary>
    ''' Сохраняет Recent-список в реестр. 
    ''' </summary>
    Private Sub RecentPersistInRegSave()
        Dim rk As RegistryKey = Registry.CurrentUser

        rk.OpenSubKey("Software\LMZ.RU", True)
        Try
            Try
                rk.DeleteSubKeyTree(m_csRecentKey)
            Catch ex As Exception
            End Try

            Dim rkRecentKey As RegistryKey = rk.CreateSubKey(m_csRecentKey)

            If m_asRecent Is Nothing Then
                rkRecentKey.SetValue("Count", 0)
            Else
                rkRecentKey.SetValue("Count", m_asRecent.Length)
                For i As Integer = 0 To m_asRecent.Length - 1
                    rkRecentKey.SetValue("Recent" & i, m_asRecent(i))
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Восстанавливает Recent-список из реестра.
    ''' </summary>
    Private Sub RecentPersistInRegLoad()
        Dim rk As RegistryKey = Registry.CurrentUser

        rk.OpenSubKey("Software\LMZ.RU", True)
        Try
            Dim rkRecentKey As RegistryKey = rk.OpenSubKey(m_csRecentKey, False)
            Dim iCount As Integer = CInt(rkRecentKey.GetValue("Count"))

            If iCount <= 0 Then
                Return
            End If

            If iCount > 31 Then
                Throw (New Exception("i > 31"))
            End If

            m_asRecent = New String(iCount - 1) {}

            For i As Integer = 0 To iCount - 1
                m_asRecent(i) = DirectCast(rkRecentKey.GetValue("Recent" & i), String)
            Next

            RecentUpdate()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Перебор элементы массива m_asRecent и добавление новые элементы меню.
    ''' В качестве текста для элементов меню используются строки из m_asRecent.
    ''' Здесь происходит подключение к обработчикам событий новых элементов меню.
    ''' Для всех элементов используется один метод-обработчик miRec1_Click.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RecentUpdate()
        miRecent.DropDownItems.Clear()
        tbbOpen.DropDownItems.Clear()

        Dim I As Integer

        For Each s As String In m_asRecent
            ' старое .Net
            'miRecent.MenuItems.Add(s, New System.EventHandler(AddressOf miRec1_Click))
            'miRecent.DropDownItems.Add(s, Nothing, New System.EventHandler(AddressOf miRec1_Click))

            Dim ToolStripMenuItem1 As ToolStripMenuItem = New ToolStripMenuItem() With {.Name = "ToolStripMenuItem" & I,
                                                                                        .Text = s}
            'Me.ToolStripMenuItem1.Size = New System.Drawing.Size(181, 22)
            AddHandler ToolStripMenuItem1.Click, AddressOf miRecent_Click
            miRecent.DropDownItems.Add(ToolStripMenuItem1)
            I += 1
        Next

        ' старое .Net
        'Dim arrI() As System.Windows.Forms.ToolStripItem ' = Nothing
        'arrI = New System.Windows.Forms.ToolStripItem() {New System.Windows.Forms.ToolStripItem(), New System.Windows.Forms.ToolStripItem()}
        'miRecent.DropDownItems.CopyTo(arrI, 0)
        'tbbOpen.DropDownItems.AddRange(arrI)

        For Each ItemTemp As ToolStripItem In miRecent.DropDownItems
            AddHandler tbbOpen.DropDownItems.Add(ItemTemp.Text).Click, AddressOf miRecent_Click
        Next
    End Sub

#Region "старое .Net"
    ' Чтобы добавить выпадающее меню к DropDownButton-кнопке ToolBar-а, нужно всего лишь создать это меню и выбрать его в свойстве DropDownMenu кнопки ToolBar-а 
    ' (или свойстве ContextMenu обычных элементов управления). Однако в старой версии контролов есть ограничения. Дело в том, что это меню должно обязательно быть типа ContextMenu. 
    ' Если уже имеется MenuItem (элемент меню) в MainMenu (главном меню формы), то использовать его как DropDownMenu или контекстное меню не удастся. 
    ' Надо обойти это ограничение.
    ' Например, есть меню (MenuItem) miRecent, который содержит (постоянно обновляемый) список ранее открывавшихся файлов. 
    ' Это меню имеет довольно глубокую вложенность, из-за чего доступ к нему с помощью мыши становится неудобным. 
    ' Можно присвоить кнопке «Open» (открывающей диалог выбора конфигурационного файла) стиль DropDownButton, и при нажатии на стрелку рядом с кнопкой 
    ' выводить меню со списком ранее открывавшихся файлов. Меню у нас уже есть, но, к сожалению, напрямую мы его использовать не можем.
    ' Зато мы можем динамически создать пустое контекстное меню, скопировать туда все элементы меню miRecent и вывести контекстное меню непосредственно под кнопкой tbbOpen. 
    ' Сделать это можно в обработчике события ButtonDropDown.
    '    Метод MergeMenu объединяет меню, для которого она вызывается, с получаемым в параметре, 
    ' но так как контекстное меню на этот момент еще пусто, он просто копирует элементы меню.
    ' Обратите внимание и на то, что ссылки на объекты можно проверять на равенство и неравенство. 
    ' Здесь эта проверка поставлена на всякий пожарный случай, так как DropDownButton в нашем ToolBar-е всего одна.
    ' Конечно, намного удобнее было бы просто выбрать miRecent в качестве контекстного меню еще во время разработки, но увы.

    'Private Sub tbbOpen_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbbOpen.DropDownOpening
    '    'End Sub
    '    'Private Sub toolBar1_ButtonDropDown(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
    '    'If e.Button IsNot tbbOpen Then
    '    '    Throw New Exception("Поддерживается только одна DropDownButton (tbbOpen)!")
    '    'End If
    '    If sender IsNot tbbOpen Then
    '        Throw New Exception("Поддерживается только одна DropDownButton (tbbOpen)!")
    '    End If

    '    ''Dim r As Rectangle = tbbOpen.Rectangle
    '    'Dim r As Rectangle = tbbOpen.ContentRectangle
    '    '' Получаем координаты кнопки
    '    ''Dim cm As New ContextMenu()
    '    ''Dim cm As New ContextMenuStrip

    '    ''cm.MergeMenu(miRecent)
    '    'ContextMenuStrip1.Items.Clear()

    '    'ContextMenuStrip1.Items.AddRange(miRecent.DropDownItems)
    '    '' Копируем, элемнты меню из miRecent.
    '    ''cm.Show(toolBar1, New Point(r.Left, r.Bottom))
    '    'ContextMenuStrip1.Show(toolBar1, New Point(r.Left, r.Bottom))
    '    '' Показываем меню.

    '    tbbOpen.DropDownItems.Clear()
    '    tbbOpen.DropDownItems.AddRange(miRecent.DropDownItems)
    'End Sub

    'Private Sub toolBar1_ButtonClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
    '    'Select Case DirectCast(e.Button.Tag, String)
    '    Select Case DirectCast(CType(sender, System.Windows.Forms.ToolStripButton).Tag, String)
    '        Case "New"
    '            FileNew()
    '            Exit Select
    '        Case "Open"
    '            FileOpenConfig()
    '            Exit Select
    '        Case "Save"
    '            SaveCurrentConfig()
    '            Exit Select
    '        Case "ReplaceInFile"
    '            ReplaceInFileWithPromt()
    '            Exit Select
    '        Case "Esc"
    '            EscepeSelection()
    '            Exit Select
    '        Case Else
    '            MessageBox.Show("Нет обработчика!")
    '            Exit Select
    '    End Select
    'End Sub
#End Region

#End Region

#Region "Работа с конфигурационными файлами"

    ''' <summary>
    ''' Предложение пользователю сохранить изменения конфигурационного файла.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileNew()
        If Not TrySave() Then
            Return
        End If
        m_sCurrentConfig = ""
        tbFind.Text = ""
        tbReplaceTo.Text = ""
        tbPath.Text = ""
        tbExtensions.Text = "*.xml" '"*.asp *.htm*"
        cbReplAll.Checked = True
        cbIgnoreCase.Checked = True
        cbMultiline.Checked = True
        cbExplicitCapture.Checked = False
        cbSingleline.Checked = True
        cbIgnorePatternWhitespace.Checked = False
        cbCompiled.Checked = False
        SetDirty(False)
    End Sub

    ''' <summary>
    ''' Выбор папки с файлами, в которых производится замена
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BroseForFolder()
        'Dim bfd As New BrowseFolderDialog() 'BrowseFolderDialog
        'bfd.InitialFolderName = tbPath.Text
        'bfd.Description = "Выбрать папку"
        'bfd.Flags = BrowseDialogFlags.ReturnOnlyFSDirs Or BrowseDialogFlags.NewDialogStyle Or BrowseDialogFlags.UseNewUI

        'If DialogResult.OK = bfd.ShowDialog(Me) Then
        '    tbPath.Text = bfd.FolderName
        'End If

        'BrowseFolderDialog.RootFolder = tbPath.Text
        ' по умолчанию установить папку
        BrowseFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer
        ' установить описание назначения окна FolderBrowserDialog.
        BrowseFolderDialog.Description = "Выбрать папку"
        BrowseFolderDialog.ShowNewFolderButton = False

        ' открыть диалог
        If DialogResult.OK = BrowseFolderDialog.ShowDialog(Me) Then
            tbPath.Text = BrowseFolderDialog.SelectedPath

            'folderName = BrowseFolderDialog.SelectedPath
            'If (Not fileOpened) Then
            '    ' ни один файл не открыт, вызвать openFileDialog в выбранном пути.
            '    OpenFile.InitialDirectory = folderName
            '    OpenFile.FileName = Nothing
            '    openMenuItem.PerformClick()
            'End If
        End If
    End Sub

    ''' <summary>
    ''' Запись настроек в XML. Записывает текст контрола в ветку соответствующую имени контрола.
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="ctrl"></param>
    ''' <remarks></remarks>
    Private Sub SaveToXmlControlText(ByVal doc As XmlDocument, ByVal ctrl As Control)
        ' Получаем корневую ветку XML-документа.
        Dim root As XmlNode = doc.DocumentElement
        ' Получаем имя контрола и удаляем у него префикс.
        Dim sName As String = ctrl.Name.Remove(0, 2)
        ' Создаем XML-элемент.
        Dim Elem As XmlNode = doc.CreateNode(XmlNodeType.Element, sName, "")
        ' Создаем XML-текст.
        Dim ElemTxt As XmlNode = doc.CreateNode(XmlNodeType.Text, "", "")
        ' Присваем текст контрола в XML-текст.
        ElemTxt.Value = ctrl.Text
        ' Добавляем XML-текс к XML-элементу.
        Elem.AppendChild(ElemTxt)
        ' Добавляем XML-элемент к корневому элементу документа.
        root.AppendChild(Elem)
    End Sub

    ''' <summary>
    ''' Запись настроек в XML. Записывает состояние Value CheckBox-а в ветку соответствующую имени контрола.
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="ctrl"></param>
    ''' <remarks></remarks>
    Private Sub SaveToXmlCheckBox(ByVal doc As XmlDocument, ByVal ctrl As CheckBox)
        Dim root As XmlNode = doc.DocumentElement
        Dim sName As String = ctrl.Name.Remove(0, 2)
        Dim Elem As XmlNode = doc.CreateNode(XmlNodeType.Element, sName, "")
        Dim ElemTxt As XmlNode = doc.CreateNode(XmlNodeType.Text, "", "")
        ElemTxt.Value = ctrl.Checked.ToString()
        Elem.AppendChild(ElemTxt)
        root.AppendChild(Elem)
    End Sub

    Private Shared listExceptControls As List(Of Control)
    ''' <summary>
    ''' Вспомогательный массив для элементов вкладки tpSetings не нуждающихся в сохранении настроек
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateExceptControls()
        listExceptControls = New List(Of Control)(2)
        listExceptControls.AddRange(New Control() {TextBoxCaptionToSearch, TextBoxCaptionToReplace})
    End Sub

    ''' <summary>
    ''' Сохранить конфигурационный файл
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveCurrentConfig()
        ' Методу SaveConfig требуется передать имя открываемого файла через переменную m_sCurrentConfig.
        ' Это строковая переменная, областью видимости которой является экземпляр формы.
        ' В ней хранится путь к файлу настроек, который открыт в данный момент. Поэтому для записи достаточно вызвать SaveConfig(m_sCurrentConfig);.
        ' Однако не все так просто. В первый раз путь к файлу будет пустым. 
        ' К тому же может случиться, что по каким-то причинам файл не может быть записан (например, файл или каталог защищен от записи).
        ' При этом неминуемо будет возбуждено исключение и, если его не перехватить, программа, повозмущавшись, закроется. 
        ' Можно конечно взять вызов функции в try/catch-блок и выдать сообщение пользователю. Но вряд ли от такой программы будет прок. 
        ' Правильным выходом будет в случае неудачи предложить пользователю записать файл под другим именем, выдав диалог «Save As». 
        ' К тому же диалог «Save As» сам по себе должен присутствовать в приличной программе.
        ' Еще одно соображение заключается в том, что реализовывать и простую запись, и «Save As» лучше не в обработчиках событий, а в отдельных методах.
        ' В программе из: меню, тулбара и командной строки.
        ' Сначала делается попытка записать файл под именем, указанным в переменной m_sCurrentConfig, и, 
        ' если это не удается, вызывается метод, предлагающий пользователю сохранить файл под другим именем.
        Try
            SaveConfig(m_sCurrentConfig)
        Catch ex As Exception
            SaveConfigAs()
        End Try
    End Sub

    ''' <summary>
    ''' Записывает настройки приложения в XML-файл.
    ''' sName: Имя файла.
    ''' </summary>
    ''' <param name="sName">Имя XML-файла.</param>
    Private Sub SaveConfig(ByVal sName As String)
        Dim doc As New XmlDocument()
        Dim root As XmlNode = doc.CreateNode(XmlNodeType.Element, "shr", "")
        'XmlNode cdata = doc.CreateNode(XmlNodeType.CDATA, "", "");
        doc.AppendChild(root)

        ' Перебераем элементы управления и записываем значение каждого 
        ' в конфигурационный файл. Комментарии см. в ф-и OpenConfig.
        For Each ctrl As Control In GetStatefullCtrls(tpSetings.Controls)
            If TypeOf ctrl Is CheckBox Then
                SaveToXmlCheckBox(doc, TryCast(ctrl, CheckBox))
            ElseIf TypeOf ctrl Is TextBox Then
                SaveToXmlControlText(doc, ctrl)
            Else
                Throw New Exception(String.Format("Неопознанный тип контрола.  Имя={0}, Info={1}", ctrl.Name, ctrl.ToString()))
            End If
        Next

        ' После записи настроек из элементов управления с панели tpSetings происходит запись пути в дереве каталогов с вкладки Preview.
        ' Свойство FullPath позволяет получить путь к выделенной ветке в виде строки.
        ' Сохраняем выделенную ветку в Preview-дереве.
        If tvFiles.SelectedNode IsNot Nothing Then
            Dim Elem As XmlNode = doc.CreateNode(XmlNodeType.Element, "m_sCurrentTvPath", "")
            Dim ElemTxt As XmlNode = doc.CreateNode(XmlNodeType.Text, "", "")
            ElemTxt.Value = tvFiles.SelectedNode.FullPath
            Elem.AppendChild(ElemTxt)
            root.AppendChild(Elem)
        End If

        doc.Save(sName)
        SetDirty(False)
    End Sub

    ''' <summary>
    ''' Предлагает пользователю ввести имя файла в диалоге "Save As"
    ''' и производит попытку записи в выбранный файл.
    ''' </summary>
    Private Sub SaveConfigAs()
        ' Если пользователь подтвердил переименование файла (нажал Save)...
        If DialogResult.OK = SaveFile.ShowDialog(Me) Then
            Try
                ' Попытка записи...
                SaveConfig(SaveFile.FileName)
                ' Если удается, запоминаем имя открывтого файла...
                m_sCurrentConfig = SaveFile.FileName
                ' и добавляем путь к Recent-списку.
                RecentAdd(m_sCurrentConfig)
            Catch e As Exception
                ' Показать сообщение об ошибке одновременно объясняя, где она возникла.
                ' MsgBox – это простенькая функция-обертка.
                MsgBox($"Невозможно сохранить настройки в конфигурационном файле.{Environment.NewLine}Ошибка: {e.Message}")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Предложение пользователю сохранить изменения конфигурационного файла
    ''' перед открытием нового.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileOpenConfig()
        If TrySave() Then
            ' Выводим диалог для ввода открываемого файла...
            If DialogResult.OK = OpenFile.ShowDialog() Then
                OpenConfigSafe(OpenFile.FileName)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Открывает XML-файл настроек. При возникновении ошибки показывает 
    ''' сообщение об ошибке и продолжает работу в нормальном режиме 
    ''' (не возбуждая исключения).
    ''' </summary>
    ''' <param name="sName">Имя XML-файла.</param>
    Private Sub OpenConfigSafe(ByVal sName As String)
        Try
            OpenConfig(sName)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, csAppName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Открывает XML-файл настроек.
    ''' </summary>
    ''' <param name="sName">Имя XML-файла. Null или пустая строка
    ''' приведет к созданию нового документа.</param>
    Private Sub OpenConfig(ByVal sName As String)
        ' Процесс считывания настроек из XML-файла аналогичен обратному, за исключением того, 
        ' что необходимо обновить «список ранее открываемых файлов». Этот список показывается в меню «File-> Recent». 
        ' Функцией RecentAdd вносит в этот список путь к файлу.
        tabControl1.SelectedIndex = 0

        If sName Is Nothing OrElse sName.Length = 0 Then
            SetDirty(False)
            FileNew()
            Return
        End If

        m_bLoading = True

        Try
            Dim doc As New XmlDocument()
            doc.Load(sName)
            Dim d As XmlElement = doc.DocumentElement

            ' Перебераем элементы управления и считываем для каждого из них
            ' значение из конфигурационного файла.
            For Each ctrl As Control In GetStatefullCtrls(tpSetings.Controls)
                ' Каждый контрол имеет двухбуквенный префикс (cb или tb)
                ' Поэтому имя тега можно получить отбросом первый двух
                ' символов имени контрола.
                Dim Name As String = ctrl.Name.Remove(0, 2)
                If TypeOf ctrl Is CheckBox Then
                    ' Для преобразования строки в bool нужно воспользоваться bool.Parse()
                    Try
                        TryCast(ctrl, CheckBox).Checked = Boolean.Parse(d.SelectSingleNode(Name).InnerText)
                    Catch ex As Exception
                    End Try
                ElseIf TypeOf ctrl Is TextBox Then
                    ' операции чтения взяты в try{ } catch{}, так как если значений нет 
                    ' в xml не нужно выдвать сообщений об ошибке, ведь это скорее всего новый элемент.
                    Try
                        TryCast(ctrl, TextBox).Text = d.SelectSingleNode(Name).InnerText
                    Catch ex As Exception
                    End Try
                Else
                    Throw New Exception(String.Format("Неопознанный тип контрола.  Имя={0}, Info={1}", ctrl.Name, ctrl.ToString()))
                End If
            Next

            Try
                m_sCurrentTvPath = d.SelectSingleNode("m_sCurrentTvPath").InnerText
            Catch ex As Exception
            End Try

            m_sCurrentConfig = sName
            ' Запоминаем имя конфигурационного файла
            ' Обновляем список открывавшихся файлов
            RecentAdd(sName)
        Catch ex As Exception
            Throw New Exception($"Невозможно открыть конфигурационный файл ""{sName}""{Environment.NewLine}Ошибка: {ex.ToString}")
        Finally
            m_bLoading = False
        End Try
        ' Если открытие файла настроек прошла удачно...
        SetDirty(False) ' ...помечаем состояние как "записанное".
    End Sub

    ''' <summary>
    ''' Рекурсивный обхода вложенных элементов управления.
    ''' Возвращает массив с элементами управления типа CheckBox или TextBox.
    ''' </summary>
    ''' <param name="ctrls">Коллекция контролов. Например, this.Controls</param>
    ''' <returns>Динамический массив элементов управления</returns>
    Private Shared Function GetStatefullCtrls(ByVal ctrls As Control.ControlCollection) As Control()
        ' Сохранение настроек в файле
        ' Чтобы не вводить каждый раз настройки вручную, их можно сохранять в специальных файлах и впоследствии загружать из них.
        ' Выбран в качестве базового формата XML. В принципе, прекрасно подошли бы и обычные ini-файлы.
        ' Xотя базовым форматом и является XML, сама структура файла целиком и полностью зависит от потребностей. 
        ' Исходя из этого, лучше всего давать этим файлам специальное расширение. Выбрано расширение «shr». 
        ' Если файл новый, нам необходимо дать возможность пользователю выбрать имя файла.
        ' Но записывать значение каждого элемента управления в файл, а потом извлекать его оттуда слишком утомительно. 
        ' К тому же в любой момент может появиться новая настойка и, чтобы ее учесть, придется менять код сразу в нескольких местах. 
        ' Чтобы избежать этого, воспользована объектная модель WinForms. 
        ' Каждый элемент управления в WinForms, а также формы, имеют свойство Controls. 
        ' Оно перечисляет все элементы управления, для которых данный элемент является родительским. 
        ' Таким образом, несложно создать код, который будет перебирать все элементы управления для заданного.
        ' Чтобы упростить процедуру перебора элементов управления, создана функция, которая принимает в качестве параметра коллекцию ControlCollection 
        ' и возвращает плоский массив, содержащий ссылки на входящие в нее элемент управления.
        ' Функция возвращает плоский массив элементов управления, содержащий только текстовые поля и переключатели. 
        ' Остальные элементы нам не нужны, так как сохраняются значения только для элементов управления этого типа.
        Dim aryCtrls As New List(Of Control)(ctrls.Count)

        For Each c As Control In ctrls
            If listExceptControls.Contains(c) Then
                Continue For
            End If

            If TypeOf c Is CheckBox OrElse TypeOf c Is TextBox Then
                aryCtrls.Add(c)
            Else
                aryCtrls.AddRange(GetStatefullCtrls(c.Controls))
            End If
        Next
        Dim cs As Control() = New Control(aryCtrls.Count - 1) {}
        aryCtrls.CopyTo(cs, 0)
        Return cs
    End Function

    ''' <summary>
    ''' Этот метод, во-первых, устанавливает переменную формы m_bIsDirty, говорящую, 
    ''' что конфигурационные данные изменены и их требуется сохранить (при m_bIsDirty = TRUE), 
    ''' или не требуется сохранять (при m_bIsDirty = FALSE). 
    ''' Во-вторых, он формирует заголовок окна.
    ''' </summary>
    ''' <param name="bIsDirty"></param>
    ''' <remarks></remarks>
    Private Sub SetDirty(ByVal bIsDirty As Boolean)
        ' Переменная-флаг m_bLoading поднимается при загрузке файла с настройками. 
        ' Пока загружается файл, поведение программы несколько отличается от обычного.
        ' В данном случае при загрузке не нужно модифицировать переменную m_bIsDirty и дописывать звездочку в заголовок.
        If Not m_bLoading Then
            m_bIsDirty = bIsDirty
        End If
        Text = csAppName & " - " &
            (If((m_sCurrentConfig IsNot Nothing AndAlso m_sCurrentConfig.Length > 0), m_sCurrentConfig, "<New search>")) &
            (If(m_bIsDirty AndAlso Not m_bLoading, " *", ""))
    End Sub

#End Region

    ''' <summary>
    ''' Окно сообщения
    ''' </summary>
    ''' <param name="sMsg"></param>
    ''' <remarks></remarks>
    Private Sub MsgBox(ByVal sMsg As String)
        MessageBox.Show(Me, sMsg, csAppName, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
    Private Sub miFileExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miFileExit.Click
        Close()
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        SplitterShamanism()
    End Sub

    ''' <summary>
    ''' Всего-навсего эмулирует модификацию свойства SplitPosition, 
    ''' что вызывает проверку минимальных размеров элементов управления (задаваемых свойствами MinExtra, MinSize). 
    ''' При этом происходит коррекция размеров элементов управления и положения сплитера. 
    ''' Если бы такой коррекции не происходило, сплитер вместе со вторым элементом управления периодически 
    ''' оказывались бы за пределами видимой области окна, (что сильно раздражает пользователей).
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SplitterShamanism()
        If WindowState = FormWindowState.Normal Then
            SplitContainerPrivewVert.SplitterDistance = SplitContainerPrivewVert.SplitterDistance
            SplitContainerPreviewHor.SplitterDistance = SplitContainerPreviewHor.SplitterDistance
            SplitContainerSetting.SplitterDistance = SplitContainerSetting.SplitterDistance
        End If
    End Sub

    ''' <summary>
    ''' Заполнять дерево и список вхождений.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreviewReadDirs()
        Try
            m_Rep = New AscRegExpParser(tbFind.Text, tbReplaceTo.Text, GetRegExOptions())
            Dim sPath As String = m_sCurrentTvPath
            Dim iSelFileMatch As Integer = lbFileMatch.SelectedIndex ' выбранный индекс найденного вхождения в тексте

            tvFiles.BeginUpdate()
            Try
                m_SearchEngine.ScanDir(tbPath.Text, tbExtensions.Text)
                m_SearchEngine.FillTreeView(tvFiles)

                tvFiles.SelectedNode = FindNode(tvFiles.Nodes, sPath)
                If iSelFileMatch >= 0 AndAlso iSelFileMatch < lbFileMatch.Items.Count Then
                    lbFileMatch.SelectedIndex = iSelFileMatch
                End If
                statusBar1.Items(ciPanelFileCount).Text = m_SearchEngine.FilesCount.ToString()
            Finally
                tvFiles.EndUpdate()
            End Try
        Catch ex As Exception
            MessageBox.Show(ex.ToString, csAppName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
            Return
        End Try
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Eсли открывается закладка «Preview», нужно обработать событие SelectedIndexChanged переключения вкладки, 
    ''' и заполнять дерево и список вхождений.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabControl1.SelectedIndexChanged
        SplitterShamanism()
        If tpPreview Is tabControl1.SelectedTab Then
            PreviewReadDirs()
        End If
    End Sub

    Private Sub miOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miOpen.Click
        FileOpenConfig()
    End Sub

    Private Sub miNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miNew.Click
        FileNew()
    End Sub

    Private Sub miBrowseForFolder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miBrowseForFolder.Click
        BroseForFolder()
    End Sub

    Private Sub pbBroseFolder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbBroseFolder.Click
        BroseForFolder()
    End Sub

    Private Sub miSaveAs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miSaveAs.Click
        SaveConfigAs()
    End Sub

    Private Sub miSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles miSave.Click
        SaveCurrentConfig()
    End Sub

    Private Sub AllTexBox_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbExtensions.TextChanged,
                                                                                                    tbFind.TextChanged,
                                                                                                    tbReplaceTo.TextChanged,
                                                                                                    tbReplCount.TextChanged,
                                                                                                    tbPath.TextChanged

        SetDirty(True)
    End Sub


    ''' <summary>
    ''' Предлагаем пользователю сохранить изменения конфигурационного файла.
    ''' Это событие вызывается при выборе одного из Recent-файлов
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub miRecent_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Метод TrySave проверяет, не был ли изменен текущий (открытый в данный момент) файл, 
        ' и если был, выдает предложение записать его. 
        ' При этом можно согласиться, отказаться от записи или отказаться от всей операции.
        ' В последнем случае TrySave возвращает FALSE и никаких действий не производится.
        If TrySave() Then
            ' в параметре sender передается ссылка на объект-возбудитель события.
            ' Так как точно исвестно, что это объект типа MenuItem, то можно сделать приведение типов и получить доступ к объекту-зачинщику события.
            ' При выборе одного из пунктов меню Recent всего лишь необходимо взять путь, являющийся текстом элемента меню (свойство Text)
            ' и открыть нужный конфигурационный файл (с помощью OpenConfigSafe).
            OpenConfigSafe(TryCast(sender, ToolStripMenuItem).Text)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Склеивает два пути в один.
    ' ''' </summary>
    ' ''' <param name="sPath1">Первый путь. Может оканчиваться на '/' или '\\',
    ' ''' а может не оканчиваться.</param>
    ' ''' <param name="sPath2">Не может начинаться с '/' или '\\'.</param>
    ' ''' <returns></returns>
    'Private Shared Function MakePath(ByVal sPath1 As String, ByVal sPath2 As String) As String
    '    ' MakePath – это простая функция позволяющая «склеить» два пути.
    '    ' Если строка не заканчивается на символ «\» или «/», то функция дописывает в конец символ «\». 
    '    ' В конце работы функция добавляет к первому пути второй. При этом второй путь не должен начинаться с перечисленных символов. 
    '    ' Это чисто вспомогательная функция, не имеющая прямого отношения к классу, поэтому она объявлена как статическая и скрытая (private static).
    '    ' Данный код вынесен в отдельную функцию для упрощения основной функции, к тому же он встречается в коде программы дважды.
    '    ' К символам StringBuilder-а можно обращаться с помощью квадратных скобок (так, как будто StringBuilder является массивом символов).
    '    Dim sb As New StringBuilder(sPath1, 1000)
    '    If sb.Length > 0 Then
    '        If sb(sb.Length - 1) <> "/"c OrElse sb(sb.Length - 1) <> "\"c Then
    '            sb.Append("\"c)
    '        End If
    '    End If
    '    sb.Append(sPath2)
    '    Return sb.ToString()
    'End Function

    ''' <summary>
    ''' Склеивает два пути в один.
    ''' </summary>
    ''' <param name="sPath1"></param>
    ''' <param name="sPath2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function MakePath(sPath1 As String, sPath2 As String) As String
        Dim combination As String = String.Empty
        Try
            combination = Path.Combine(sPath1, sPath2)
        Catch e As Exception
            If sPath1 = Nothing Then
                sPath1 = "Nothing"
            End If
            If sPath2 = Nothing Then
                sPath2 = "Nothing"
            End If
            Dim txt As String = String.Format("Невозможно скомбинировать пути '{0}' и '{1}' потому что: {2}{3}", sPath1, sPath2, Environment.NewLine, e.Message)
            MessageBox.Show(txt, "Функция MakePath", MessageBoxButtons.OK, MessageBoxIcon.Error)
            combination = "C:\"
        End Try
        Return combination
    End Function

    ''' <summary>
    ''' Событие вызываемое при выборе ветки в дереве файлов (закладка Preview).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tvFiles_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvFiles.AfterSelect
        ' После заполнения TreeView каталогами и файлами производится попытка найти и сделать активной ветку с путем, аналогичным ранее активной ветке.
        ' Такой ветки может и не найтись. В этом случае в свойство SelectedNode (свойство TreeView позволяющее узнать или установить активную ветку) попадает NULL.
        ' В результате TreeView не будет иметь ни одной активной ветки. При активации некоторой ветки TreeView генерирует событие AfterSelect.
        ' Это же событие генерируется, если ветка активизируется пользователем. Необходимо обработать это событие,
        ' загрузив на него список вхождений регулярных выражения в lbFileMatch (ListBox) и содержимое файла в tbFileBody (TextBox).
        ' Самым интересным моментом обработчика события tvFiles_AfterSelect является заполнение ListBox-а lbFileMatch. 
        ' Интересен он тем, что в качестве элемента списка методу Add передается экземпляр класса, а не строка, как это обычно бывает.
        ' Дело в том, что обычно ListBox хранит список в виде строк. Если нужно хранить дополнительную информацию,
        ' то с элементом списка можно ассоциировать некоторые пользовательские данные. 
        ' В WinForms в качестве элемента списка можно добавить любой объект. Более того, один ListBox может хранить объекты разных типов.
        ' При этом текст задавать вообще не нужно! Это самом деле очень удобно (к тому же оригинально).
        ' Дело в том, что любой объект (даже простые типы, такие как int или double) в CLR (и в том числе в C#) унаследован (хоть и мнимо)
        ' от класса object. Класс object реализует виртуальный метод ToString. Этот метод возвращает строку, приемлемую для чтения человеком.
        ' Новый класс наследует реализацию базового. Реализация System.Object (по умолчанию) выведет название класса и другую малоинтересную информацию.

        Dim tnSel As TreeNode = tvFiles.SelectedNode

        If tnSel Is Nothing Then
            Return
        End If
        m_sCurrentTvPath = tnSel.FullPath

        Dim sPath As String = MakePath(tbPath.Text, m_sCurrentTvPath)

        SetStatusText(sPath)

        If tnSel.ImageIndex <> AscSearchEngine.ciFileSel Then
            lbFileMatch.Items.Clear()
            tbFileBody.Clear()
            ' Если ветка не является файлом, выходим...
            Return
        End If

        GC.Collect()
        tbFileBody.Text = m_Rep.ReadFile(sPath)
        Dim mc As MatchCollection = m_Rep.Parse(tbFileBody.Text)

        lbFileMatch.BeginUpdate()
        Try
            lbFileMatch.Items.Clear()
            For Each m As Match In mc
                lbFileMatch.Items.Add(New AscMatch(m))
            Next
            If lbFileMatch.Items.Count > 0 Then
                lbFileMatch.SelectedIndex = 0
            End If
        Finally
            lbFileMatch.EndUpdate()
        End Try
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Выбор пункта в списке вхождений найденых в текущем файле (закладка Preview).
    ''' </summary>
    Private Sub lbFileMatch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbFileMatch.SelectedIndexChanged
        ' ListBox использует в качестве строки, идентифицирующей элемент списка, значение, возвращаемое методом ToString. 
        ' Это позволяет упростить и укоротить код. Так, в ListBox без каких бы то ни было преобразований можно добавлять экземпляры любых типов. 
        ' От базовых до структур и классов. Главное, чтобы их методы ToString предоставлял строку разумного содержания, 
        ' способную идентифицировать значение хранимого экземпляра. 
        ' Естественно, что в качестве такого экземпляра можно использовать и обычную строку (она тоже предоставляет реализацию метода ToString, которая, 
        ' впрочем, ничего не делает, просто возвращает ссылку на эту же строку). Но намного интереснее помещать в ListBox объекты, так сказать, собственного производства. 
        ' Естественно, для них нужно создать собственную реализацию метода ToString. Таким способом убиваются сразу два зайца. 
        ' С одной стороны, позволяет хранить необходимую информацию (экземпляр нужного объекта или простого типа). 
        ' С другой, тем же самым объектом мы задаем текст для элемента списка. В принципе, объект может быть добавлен к списку более одного раза, 
        ' поэтому идентификация элементов списка в основном осуществляется по индексу. 
        ' Так, у списка есть коллекция объектов (Items), доступ к элементам которой производится по индексу. 
        ' Но первый элемент коллекции можно узнать с помощью метода коллекции IndexOf. 
        ' Активный элемент списка можно узнать из свойств SelectedItem или SelectedIndex ListBox-а. 
        ' Первое свойство возвращает индекс активного элемента (или отрицательное значение, если такового нет). 
        ' Второе – сам объект (или null в отсутствии активной записи).
        ' Для Value-типов (структуры и простые типы (int, char, double...) перед добавлением производится так называемый боксинг 
        ' (реально он происходит автоматически при преобразовании Value-типов к типу object). 
        ' Для обычных объектов в список помещается ссылка. Причем в отличие от многих языков программирования список удерживает объекты от разрушения. 
        ' Можно создать объект и, не оставляя дополнительных ссылок, поместить ссылку на него в ListBox. 
        ' При этом объект будет жить ровно столько, сколько будет жить сам ListBox (т.е. пока в программе будут иметься ссылки на ListBox). 
        ' После уничтожения ListBox-а сборщик мусора автоматически уничтожит все объекты из списка, на которые нет ссылок из других мест программы.
        ' Код обработчика события tvFiles_AfterSelect создает экземпляр AscMatch, передавая в конструктор ссылку на Match, 
        ' и передает ссылку на новый объект методу Add коллекции Items ListBox-а. 
        ' Add добавляет ссылку на AscMatch в коллекции, тем самым удерживая сборщик мусора от уничтожения этого объекта. 
        ' Когда ListBox-у нужно отобразить строку, он получает ее путем вызова метода ToString объекта AscMatch.
        ' При установке свойства SelectedIndex (как в обработчике события tvFiles_AfterSelect) или при активизации элемента списка пользователем ListBox-е 
        ' lbFileMatch генерирует событие SelectedIndexChanged. 
        ' Обработчик этого события выделяет соответствующий фрагмент текста в текстовом окне tbFileBody (производя прокрутку этого окна, так, 
        ' чтобы выделенный текст стал виден на экране) и заполняет текстовое окно tbReplaceResult значением подстановки
        ' (которое получалось бы, если бы замена была произведена).
        ' TextBox в ранней .Net не умел осуществлять программного скролинга. 
        ' Но так как он создан на базе стандартного класса окна Windows (Edit), скроллинг можно осуществить с помощь посылки окну сообщения EM_SCROLLCARET. 
        ' Handle окна можно получить из одноименного свойства любого оконного объекта WinForms. 
        ' Чтобы послать текстовому окну сообщение, создана функцию SendMessage и сообщение EM_SCROLLCARET:
        ' Прямое использование Win32 API может создать непереносимый код и привнести в программу трудноуловимые ошибки. 
        ' Второе опасение нужно воспринимать очень серьезно. Прямые вызовы Windows API Windows лучше вообще не применять в приложениях напрямую. 
        ' Если без них не обойтись, лучше запаковать их в отдельную библиотеку, сделав безопасную обертку над ними. 

        Try
            Dim m As AscMatch = DirectCast(lbFileMatch.SelectedItem, AscMatch)
            ' Выделяем соотвествующий участок текста.
            tbFileBody.Select(m.Match.Index, m.Match.Length)
            ' И заставляем текстовое окно переместится к выделению.
            ' Эта функциональность не реализована в WinForms,
            ' По этому приходится прибегать к старому доброму WinAPI.
            'SendMessage(tbFileBody.Handle, EM_SCROLLCARET, 0, 0)
            '??? заменён на это   tbFileBody.ScrollToCaret()
            tbFileBody.ScrollToCaret()
            tbReplaceResult.Text = m.Match.Result(tbReplaceTo.Text)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, csAppName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    ''' <summary>
    ''' tbFileBody переносить на передний фронт путём смены Parent
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tabPreview_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabPreview.SelectedIndexChanged
        If tabPgFindeAndReplace Is tabPreview.SelectedTab Then
            tbFileBody.Parent = pnFindeAndReplace
        Else
            tbFileBody.Parent = tabPgFindeOnly
        End If
    End Sub

    ''' <summary>
    ''' Поиск ветки по заданному пути (в виде строки).
    ''' </summary>
    ''' <param name="tnc">Коллекция подветок ветки дерева.</param>
    ''' <param name="sPath">Путь в виде строки разделенной символами '\\'
    ''' или '/'.</param>
    ''' <returns>В случае успеха возвращается найденая ветка (TreeNode).
    ''' Иначе возвращается null.</returns>
    Private Function FindNode(ByVal tnc As TreeNodeCollection, ByVal sPath As String) As TreeNode
        ' Последовательно открывает найденные узлы в дереве по разделённому в массив пути к файлу
        Dim aryPathNode As String() = sPath.Split(New Char() {"\"c, "/"c})
        Dim tnCurr As TreeNode = Nothing

        For Each sPathNode As String In aryPathNode
            For Each tn As TreeNode In tnc
                If tn.Text = sPathNode Then
                    tnCurr = tn
                    tnCurr.EnsureVisible()
                    Exit For
                End If
            Next
            ' Если ветка не найдена, tnCurr равна null
            If tnCurr Is Nothing Then
                Return Nothing
            End If
            tnc = tnCurr.Nodes
        Next

        Return tnCurr
    End Function

    ''' <summary>
    ''' Выводит диалоговое окно с запросом на запись (Yes, No и Cancel).
    ''' </summary>
    ''' <returns>Если пользователь выбирает No, возвращается true.
    ''' Если пользователь выбирает Yes, производится попытка записать изменения.
    ''' При успехе возвращается true, при неудаче возбуждается исключение.
    ''' Если пользователь выбирает Cancel, возвращается false.</returns>
    Private Function TrySave() As Boolean
        If m_bIsDirty Then
            Select Case MessageBox.Show(Me, "Сохранить текущие настройки?", csAppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case DialogResult.Cancel
                    Return False
                Case DialogResult.Yes
                    SaveCurrentConfig()
                    Return True
            End Select
        End If
        Return True
    End Function


    'Private m_bStopBatchReplase As Boolean = False
    'Public Property StopBatchReplase() As Boolean
    '    Get
    '        Return m_bStopBatchReplase
    '    End Get
    '    Set(ByVal value As Boolean)
    '        m_bStopBatchReplase = value
    '    End Set
    'End Property

    ''' <summary>
    ''' Вывод информации колличества обработанных файлов на панели сообщений
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusFileCountText(sText As String)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() SetStatusFileCountText(sText)))
        Else
            statusBar1.Items(ciPanelFileCount).Text = sText
        End If
    End Sub

    ''' <summary>
    ''' Вывод информации текущего обрабатываемого файла на панели сообщений
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusText(sText As String)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() SetStatusText(sText)))
        Else
            statusBar1.Items(ciPanelMain).Text = sText
        End If
    End Sub

    Private Sub tbbClearLog_Click(sender As Object, e As EventArgs) Handles tbbClearLog.Click, tbbSaveLog.Click
        Dim s As String = DirectCast(CType(sender, ToolStripButton).Tag, String)
        Select Case s
            Case "ClearLog"
                tbLog.Clear()
                Exit Select
            Case "SaveLog"
                LogSave()
                Exit Select
            Case Else
                MessageBox.Show("Ошибочный контрол в событии tbbClearLog_Click.")
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Предлагает пользователю выбрать имя Log-файла и 
    ''' записывает Log файл с выбраным именем.
    ''' </summary>
    Private Sub LogSave()
        If SaveLog.ShowDialog(Me) = DialogResult.OK Then
            LogSave(SaveLog.FileName)
        End If
    End Sub

    ''' <summary>
    ''' Запись журнала работы приложения
    ''' </summary>
    ''' <param name="sLogFileName"></param>
    ''' <remarks></remarks>
    Public Sub LogSave(ByVal sLogFileName As String)
        Try
            Using sw As New StreamWriter(sLogFileName, False, Encoding.GetEncoding(1251))
                sw.Write("{0} журнал работы. Создан в {1}." & Environment.NewLine, csAppName, DateTime.Now)
                sw.Write(tbLog.Text)
            End Using
        Catch e As Exception
            MsgBox("Ошибка при записи журнала работы: " & e.Message)
        End Try
    End Sub

    Private Sub cbIgnoreCase_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbIgnoreCase.CheckedChanged
        tbReplCount.Visible = Not cbReplAll.Checked
    End Sub

    ''' <summary>
    ''' Преобразовать минимальный набор метасимволов 
    ''' (\, *, +, ?, |, {, [, (,), ^, $,., # и пробел), 
    ''' заменяя их escape-кодами.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EscapeSelection()
        If tbFind.Focused Then
            If tbFind.SelectedText.Length > 0 Then
                tbFind.SelectedText = Regex.Escape(tbFind.SelectedText)
            End If
        End If
    End Sub

    Private Sub EscapesSelectedTextToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EscapesSelectedTextToolStripMenuItem.Click
        EscapeSelection()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim frmAbout1 As New About
        frmAbout1.ShowDialog(Me)
    End Sub

    Private Sub tbReplCount_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles tbReplCount.Validating
        Dim i As UInteger
        Try
            i = UInteger.Parse(TryCast(sender, TextBox).Text)
            If i <= 0 OrElse i > 1000000 Then
                Throw New Exception()
            End If
        Catch generatedExceptionName As Exception
            e.Cancel = True
            MsgBox("Значение должно быть от 1 до 1000000!")
        End Try
    End Sub

    Private Sub AllFlags_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbReplAll.CheckStateChanged,
                                                                                                cbSingleline.CheckStateChanged,
                                                                                                cbMultiline.CheckStateChanged,
                                                                                                cbIgnorePatternWhitespace.CheckStateChanged,
                                                                                                cbIgnoreCase.CheckStateChanged,
                                                                                                cbExplicitCapture.CheckStateChanged,
                                                                                                cbCompiled.CheckStateChanged

        SetDirty(True)
    End Sub

    Private Sub cbReplAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbReplAll.CheckedChanged
        tbReplCount.Visible = Not cbReplAll.Checked
    End Sub

    ''' <summary>
    ''' Предложение пользователю сохранить изменения конфигурационного файла
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbbNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbbNew.Click
        FileNew()
    End Sub

    ''' <summary>
    ''' Сохранить конфигурационный файл
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbbSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbbSave.Click
        SaveCurrentConfig()
    End Sub

    ''' <summary>
    ''' Спросить пользователя на проведение замены
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbbReplaceInFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbbReplaceInFile.Click
        ReplaceInFileWithPromt()
    End Sub

    ''' <summary>
    ''' Преобразовать минимальный набор метасимволов 
    ''' (\, *, +, ?, |, {, [, (,), ^, $,., # и пробел), 
    ''' заменяя их escape-кодами.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbbEsc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbbEsc.Click
        EscapeSelection()
    End Sub

    ''' <summary>
    ''' Предложение пользователю сохранить изменения конфигурационного файла
    ''' перед открытием нового.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbbOpen_ButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles tbbOpen.ButtonClick
        FileOpenConfig()
    End Sub

    ''' <summary>
    ''' Вкл./Выкл. пункта Escapes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbFind_Enter(sender As Object, e As EventArgs) Handles tbFind.Enter
        EscapesSelectedTextToolStripMenuItem.Enabled = True
        tbbEsc.Enabled = True
    End Sub

    ''' <summary>
    ''' Вкл./Выкл. пункта Escapes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tbFind_Leave(sender As Object, e As EventArgs) Handles tbFind.Leave
        EscapesSelectedTextToolStripMenuItem.Enabled = False
        tbbEsc.Enabled = False
    End Sub

    '		public delegate void MyCallBack(int iPercett);обьявить делегат
    '		MyCallBack m_MyCallBack; локальная переменная делегата
    '		public void SetCallBack(MyCallBack CallBack) назначение локальному члену значение из общего метода класса
    '		{
    '			m_MyCallBack = CallBack;
    '		}

    'Private openFileName As String, folderName As String
    'Private fileOpened As Boolean = False
    'Private Sub openMenuItem_Click(sender As Object, e As System.EventArgs) _
    '   Handles openMenuItem.Click
    '    ' если файл не открыт, установить первоначальную директорию
    '    ' из значения FolderBrowserDialog.SelectedPath .
    '    If (Not fileOpened) Then
    '        OpenFile.InitialDirectory = BrowseFolderDialog.SelectedPath
    '        OpenFile.FileName = Nothing
    '    End If

    '    ' Вывести openFile диалог.
    '    Dim result As DialogResult = OpenFile.ShowDialog()

    '    ' OK кнопка нажата.
    '    If (result = DialogResult.OK) Then
    '        openFileName = OpenFile.FileName
    '        Try
    '            ' вывести запрашиваемый файл в richTextBox1.
    '            Dim s As Stream = OpenFile.OpenFile()
    '            richTextBox1.LoadFile(s, RichTextBoxStreamType.RichText)
    '            s.Close()

    '            fileOpened = True

    '        Catch exp As Exception
    '            MessageBox.Show("При попытке закгрузить файл произошла ошибка: " _
    '                            + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine)
    '            fileOpened = False
    '        End Try
    '        Invalidate()

    '        closeMenuItem.Enabled = fileOpened

    '        ' Cancel кнопка была нажата.
    '    ElseIf (result = DialogResult.Cancel) Then
    '        Return
    '    End If
    'End Sub

End Class