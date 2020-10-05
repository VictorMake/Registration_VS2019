Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text

' Функциональная часть RegExRep вынесена в два класса:
' AscRegExpParser – инкапсулирует работу с регулярными выражениями.
' AscSearchEngine - инкапсулирует операции по поиску файлов.

''' <summary>
''' Выполняет операции по работе с файлами и регулярными выражениями
''' (замена содержимого файла и поиск вхождений, соответствующих регулярному выражению).
''' </summary>
Public Class AscRegExpParser
    Protected m_rx As Regex
    Protected m_sReplaceWith As String

    ''' <summary>
    ''' Инициализирует регулярные выражения.
    ''' </summary>
    ''' <param name="sPatern">Строка для поиска.</param>
    ''' <param name="sReplaceWith">Строка для заметы.</param>
    ''' <param name="Options">Опции.</param>
    Public Sub New(ByVal sPatern As String, ByVal sReplaceWith As String, ByVal Options As RegexOptions)
        ' Конструктор Regex принимает два параметра: строку с регулярным выражением и опции (в виде битовых флагов). 
        ' Интересно, что RegexOptions является перечислением (имеет тип enum).
        ' Но C# запрещает логические операции над константами перечислений, а параметр Options как раз должен содержать результат таких логических операций. Как же быть?
        ' В Паскале для решения подобных задач был введен специальный тип данных (Set). В C/C++ приходилось объявлять такие переменные как целочисленные (например, int).
        ' Решение C/C++ является нетипобезопасным, но, видимо, пойти на поводу у Паскаля разработчики C# тоже не могли, и у них родилось альтернативное решение.
        ' Они ввели атрибут [Flags]. Если при описании перечисления (enum-а) указать атрибут [Flags], то над переменными, имеющими тип этого перечисления,
        ' можно будет производить булевы операции, как будто это не перечисление, а обычный int. Получился красивый, хотя и спорный, паллиативный вариант.
        m_rx = New Regex(sPatern, Options)
        m_sReplaceWith = sReplaceWith
    End Sub

    ''' <summary>
    ''' Читает содержимое файла в строку.
    ''' </summary>
    ''' <param name="sFilePath">Имя файла.</param>
    ''' <returns>Содержимое файла.</returns>
    Public Function ReadFile(ByVal sFilePath As String) As String
        ' StreamReader позволяет прочесть Stream (объект, обеспечивающий потоковые чтение и запись) или файл в виде текстовой строки.
        Using sr As New StreamReader(sFilePath, Encoding.[Default], False, 3000000)
            Return sr.ReadToEnd()
            ' После окончания чтения данных нужно закрыть файл. Для этого нужно вызывать метод Close или метод Dispose интерфейса IDisposable.
            ' Задача усложняется тем, что в любой момент после открытия файла может произойти исключение, и файл не будет закрыт до следующей сборки мусора 
            ' (а она может быть отложена на очень большой срок, вплоть до закрытия приложения). 
            ' Поэтому работу с файлом нужно помещать в блок try, а вызов метода Close помещать в блок finally.
            ' Это довольно неудобно (много кода). Упростить код можно с помощью конструкции using() {...}.
            ' Нужно просто объявить переменную, класс которой реализует интерфейс IDisposable, а все действия с файлом поместить в фигурные скобки 
            ' (если это отдельный оператор, как в нашем случае, то фигурные скобки можно опустить).
            ' Отсутствие в стопроцентно CLR-совместимых (в MC++ это возможно, хотя и с значительными ограничениями) языках автоматически вызываемых при выходе из области видимости деструкторов
            ' делает using единственным надежным способом контроля за системными ресурсами (файлами, подключениями к БД и т.п.).
        End Using
    End Function

    ''' <summary>
    ''' Разбирает содержимое файла с помощью регулярных выражений, ища
    ''' в нем соответствующие вхождения. (замена не производится).
    ''' </summary>
    ''' <param name="sText">Текст в котором нужно производить поиск.</param>
    ''' <returns>Коллекция вхождений найденных в тектсе.</returns>
    ''' <remarks>Все недостающие значения задаются в параметрах
    ''' конструктора.</remarks>
    Public Function Parse(ByVal sText As String) As MatchCollection
        Return m_rx.Matches(sText)
        ' Элемент коллекции MatchCollection содержит информацию о вхождении в тексте регулярного выражения, 
        ' например: смещение вхождения относительно начала текста – Index, длину текста – Length и сам текст. 
        ' Этой информации достаточно, чтобы найти вхождение внутри основного текста (что, собственно, и нужно сделать на стадии предварительного просмотра – «Preview»).
    End Function

    ''' <summary>
    ''' Считывает файл с в память и разбирает его.
    ''' </summary>
    ''' <param name="sFilePath">Имя файла.</param>
    ''' <returns>Коллекция вхождений найденных в ектсе.</returns>
    ''' <remarks>Все недостающие значения задаются в параметрах
    ''' конструктора.</remarks>
    Public Function ReadAndParsFile(ByVal sFilePath As String) As MatchCollection
        ' ReadAndParsFile – объединяет функциональность двух предыдущих методов.
        Return Parse(ReadFile(sFilePath))
    End Function

    ''' <summary>
    ''' Считывает файл и производит замену его содержимого.
    ''' </summary>
    ''' <param name="sFilePath">Полный путь к файлу.</param>
    ''' <param name="iReplaceCount">Сколько замен нужно сделать.</param>
    ''' <remarks>Все недостающие значения задаются в параметрах конструктора.</remarks>
    Public Sub Replace(ByVal sFilePath As String, ByVal iReplaceCount As Integer)
        ' Параметр iReplaceCount говорит о том, сколько замен нужно сделать. Если он меньше или равен нулю, значит, нужно заменять все найденные вхождения.
        ' В зависимости от значения iReplaceCount вызывается тот или иной конструктор Regex.
        ' Далее происходит запись измененного содержимого обратно в файл. Это делается с помощью экземпляра класса StreamWriter (антипода класса StreamReader).
        ' Как и в других местах (например, функции ReadFile этого же класса), закрытие файла производится автоматически (с помощью директивы using).
        Dim sText As String = Nothing

        If iReplaceCount > 0 Then
            sText = m_rx.Replace(ReadFile(sFilePath), m_sReplaceWith, iReplaceCount)
        Else
            sText = m_rx.Replace(ReadFile(sFilePath), m_sReplaceWith)
        End If

        Using sw As New StreamWriter(sFilePath, False, Encoding.[Default])
            sw.Write(sText)
        End Using
    End Sub
End Class

''' <summary>
''' Выполняет операции по поиску файлов.
''' </summary>
Public Class AscSearchEngine
    Implements IEnumerable

    ''' <summary>
    ''' Константы, определяющие индексы картинок в дереве.
    ''' </summary>
    Public Const ciDir As Integer = 0, ciDirSel As Integer = 1, ciFile As Integer = 2, ciFileSel As Integer = 2

    ' У класса AscSearchEngine есть два свойства DirsCount и FilesCount.
    ' В данном случае свойства позволяют только читать информацию, защищая тем самым данные от случайной порчи.
    ' Свойства также позволяют получить доступ к скрытым данным. Можно было бы оформить этот код и в виде методов, но свойства более легко читаются. 
    ' Чтобы отличать свойства от переменных-членов класса, желательно давать переменным определенный префикс m_ (сокращение от английского member).

    ''' <summary>
    ''' Количество найденных каталогов.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DirsCount() As Integer
        Get
            Return m_Root.m_iDirs
        End Get
    End Property

    ''' <summary>
    ''' Количество найденных файлов.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FilesCount() As Integer
        Get
            Return m_Root.m_iFiles
        End Get
    End Property

    ' Метод FileCallback, позволяет перебрать все найденные файлы (он используется для организации пакетной замены).
    ' Он переадресует это почетное занятие объекту m_Root (классу AscDir), который хранит найденные каталоги и файлы.
    Public Delegate Sub FileCallback(ByVal sFileName As String)

    ''' <summary>
    ''' Перебирает последовательно каждый найденный файл.
    ''' </summary>
    ''' <param name="Callback"></param>
    ''' <remarks></remarks>
    Public Sub IterateNodes(ByVal Callback As FileCallback)
        ' InternalIterateNode перебирает последовательно каждый найденный файл, и вызывать callback-метод (Public Sub ProcessFile(ByVal sFileName As String)), 
        ' ссылка на который передается в так называемом делегате.
        m_Root.InternalIterateNode(Callback, "")
    End Sub

    ''' <summary>
    ''' Сканирует каталог (и все подкаталоги), путь к которому передается в параметре sPath, и ищет все файлы, удовлетворяющие маске sPattern.
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <param name="sPattern"></param>
    ''' <remarks></remarks>
    Public Sub ScanDir(ByVal sPath As String, ByVal sPattern As String)
        GC.Collect()
        m_Root = New AscDir(Nothing, sPath, sPattern)
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Рекурсиваная функция  заполняющая TreeView.
    ''' </summary>
    ''' <param name="tv"></param>
    ''' <remarks></remarks>
    Public Sub FillTreeView(ByVal tv As TreeView)
        GC.Collect()
        tv.Nodes.Clear()
        FillNode(tv.Nodes, m_Root)
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Внутренняя рекурсиваная функция  заполняющая TreeView.
    ''' </summary>
    Private Sub FillNode(ByVal tnc As TreeNodeCollection, ByVal dir As AscDir)
        ' Константы используются для улучшения читаемости текста. Это особенно важно, 
        ' поскольку при выделении ветки в TreeView именно по типу картинки (свойству ImageIndex) определяется, файл это или каталог.
        Dim tn As TreeNode

        For Each ad As AscDir In dir.m_SubDir
            ' Добавляем директорию, только если в ней найдены файлы.
            If ad.m_iFiles > 0 Then
                tn = New TreeNode(ad.m_sName)
                tn.SelectedImageIndex = ciDirSel
                tn.ImageIndex = ciDir
                tnc.Add(tn)
                FillNode(tn.Nodes, ad)
            End If
        Next

        If dir.m_Files IsNot Nothing Then
            For Each s As String In dir.m_Files
                tn = New TreeNode(s)
                tn.SelectedImageIndex = ciFileSel
                tn.ImageIndex = ciFile
                tnc.Add(tn)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Реализация интерфейса IEnumerable
    ''' Нужно реализовать только один метод GetEnumerator
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shadows Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        ' Создаем итератор и запрашиваем у него IEnumerator
        Return DirectCast(New AscDir.Iterator(m_Root), IEnumerator)
    End Function

    ' Обычно интерфейс IEnumerable реализуется в отдельном классе. 
    ' Сам класс, который должен поддерживать перечисление, должен реализовать интерфейс IEnumerable, состоящий из одного метода: IEnumerable.GetEnumerator
    ' Принцип действия этого итератора прост, он перебирает все подкаталоги, заходя в каждый из них, и перебирает файлы в этих каталогах. 
    ' Но, реализация этого итератора намного сложнее, чем функции IterateNodes (точнее, InternalIterateNode), которая была описана выше. 
    ' Дело в том, что в итераторе приходится изворачиваться и уходить от рекурсии. Разворот рекурсии всегда приводит к увеличению и усложнению кода.
    ' Зачастую такой разворот может сопровождаться ускорением работы алгоритма (но в нашем случае это не так). 
    ' В принципе, процедура остается рекурсивной, но введение дополнительных переменных и стека позволяет перейти к итеративному поиску следующего элемента. 
    ' Пока ищется следующий элемент, используется рекурсия, но как только элемент найден, текущее состояние запоминается в переменных итератора, 
    ' а управление возвращается основной программе.
    Private m_Root As AscDir

    ''' <summary>
    ''' Внутрення реализация поиска файлов (скрытый вложенный класс).
    ''' Осуществляет поиск, хранение и обработку файлов и каталогов.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class AscDir
        ' Ссылка на объект (AscDir) описывающий родительский каталог.
        ' Для первого объекта в иерархии эта ссылка устанавливается в NULL.
        Protected m_ParentDir As AscDir
        ' Имя каталога описываемого объектом AscDir (без пути).
        Friend m_sName As String
        ' Список описаний поддиректорий (таких же объектов AscDir).
        Friend m_SubDir As AscDir()
        ' Список файлов лежащих непосредственно в этом каталоге 
        ' и удовлетворяющих маске. Не включает файлов найденных в подкаталогах.
        Friend m_Files As String()
        ' Количество подкаталогов. Всех, а не только текущего уровня.
        ' Вычисляется рекурсивно.
        Friend m_iDirs As Integer = 0
        ' Количество файлов во всех подкаталогах всех уровней.
        ' Вычисляется рекурсивно.
        Friend m_iFiles As Integer = 0

        ''' <summary>
        ''' Итератор последовательно просматривающий все ветки дерева
        ''' объектов AscDir и позволяет перебрать находящиеся в нем файлы.
        ''' </summary>
        Public Class Iterator
            Implements IEnumerator

            Private m_ad As AscDir
            ' Текущая ветка.
            Private m_adRoot As AscDir
            Private m_iCurrDir As Integer
            Private m_iCurrFile As Integer
            Private aryDirStak As New Stack(20)

            Public Sub New(ByVal ad As AscDir)
                m_adRoot = ad
                Reset()
            End Sub

            ' IEnumerator
            Public Sub Reset() Implements IEnumerator.Reset
                m_iCurrDir = 0
                m_iCurrFile = -1
                m_ad = m_adRoot
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If m_ad.m_Files Is Nothing OrElse m_iCurrFile + 1 >= m_ad.m_Files.Length Then
                    If m_iCurrDir < m_ad.m_SubDir.Length Then
                        aryDirStak.Push(m_iCurrDir)
                        m_ad = m_ad.m_SubDir(m_iCurrDir)
                        m_iCurrDir = 0
                        m_iCurrFile = -1
                        Return MoveNext()
                    Else
                        If aryDirStak.Count > 0 Then
                            m_iCurrDir = CInt(aryDirStak.Pop()) + 1
                            m_ad = m_ad.m_ParentDir
                            m_iCurrFile = If(m_ad.m_Files Is Nothing, 0, m_ad.m_Files.Length)
                            Return MoveNext()
                        End If
                        Return False
                    End If
                Else
                    m_iCurrFile += 1
                    If m_iCurrFile >= m_ad.m_Files.Length Then
                        Return MoveNext()
                    End If
                    Return True
                End If
            End Function

            Public ReadOnly Property Current() As Object Implements IEnumerator.Current
                Get
                    Return m_ad.m_Files(m_iCurrFile)
                End Get
            End Property
        End Class

        ''' <summary>
        ''' Разбирает переданный каталог. Если находит подкаталоги
        ''' вызывает себя рекурсивно. 
        ''' </summary>
        Friend Sub New(ByVal ParentDir As AscDir, ByVal sPath As String, ByVal sPattern As String)
            m_ParentDir = ParentDir
            ' Запоминаем родительский каталог.
            ' Класс DirectoryInfo позволяет получить исчерпывающую информацию
            ' о одном каталоге. Путь к каталогу задается в параметре конструктора.
            Dim di As New DirectoryInfo(sPath)
            ' Запоминаем имя каталога. В принципе можно было бы просто хранить
            ' сам объект DirectoryInfo, но это привело бы к неоправданному 
            ' расходу ресурсов.
            m_sName = di.Name
            ' Получаем список (массив) подкаталогов (следующего уровня вложенности).
            Dim adi As DirectoryInfo() = di.GetDirectories()
            ' Создаем аналогичный по длине массив объектов AscDir.
            m_SubDir = New AscDir(adi.Length - 1) {}
            ' Перебираем подкаталоги и для каждого из них создаем свой объект AscDir.
            ' Это приводит к рекурсивному вызову, так что, в конце концов, строится
            ' иерархия каталогов.
            For i As Integer = 0 To m_SubDir.Length - 1
                ' В новый объект AscDir передается путь, получаемый как текущий плюс
                ' имя обрабатываемого каталога, и маска поиска файлов.
                m_SubDir(i) = New AscDir(Me, $"{sPath}\{adi(i).Name}", sPattern)
            Next

            ' Теперь остается найти файлы находящиеся в текущем каталоге
            ' и удовлетворяющие маске поиска.

            ' Для этого разбираем маску поиска. Маска задается как строка где
            ' отдельные маски разделены знаком «;». Например, маска может быть 
            ' такой «*.txt;*.htm*;*.asp».
            ' Разбор делается методом Split класса string.
            Dim asPatterns As String() = sPattern.Split(";"c)
            ' Теперь нужно попытаться найти файлы соответствующие каждой маске.
            ' Для этого перебираем маски...
            For Each s As String In asPatterns
                ' и считываем список файлов удовлетворяющих этой маске.
                ' Список получается методам GetFiles класса DirectoryInfo.
                ' Для того, чтобы не загромождать конструктор, считывание вынесено
                ' В отдельный внутренний метод.
                AppendFileNames(di.GetFiles(s))
            Next

            ' И наконец подсчитываем общее количество файлов (в этой директории
            ' и всех поддиректориях).

            ' Для начала определяем количество файлов найденных непосредственно
            ' в текущем каталоге. Это не сложно, так как оно равно количеству 
            ' элементов массива m_Files в который они помещаются функцией 
            ' AppendFileNames. Но есть одна проблема. В случае если файлов в этом
            ' каталоге не найдено, массив вообще не будет создан и переменная m_iFiles
            ' будет содержать NULL. Следующий код является стандартным обходом 
            ' подобных проблем.
            m_iFiles = If(m_Files Is Nothing, 0, m_Files.Length)
            ' Делаем тоже самое для директорий.
            m_iDirs = m_SubDir.Length

            ' Теперь нужно подсчитать количество файлов в поддиректориях.
            ' Для этого просто перебираем объекты AscDir, которые к этому времени
            ' уже полностью проинициализированы. Так как конструктор вызывает себя
            ' рекурсивно, после выхода из него поле m_iFiles будет содержать количество
            ' файлов во всех его подкаталога, а значит нам достаточно считать
            ' значение m_iFiles только у следующего уровня подкаталогов.
            For Each SubDir As AscDir In m_SubDir
                m_iFiles += SubDir.m_iFiles
                m_iDirs += SubDir.m_iDirs
            Next
        End Sub

        ''' <summary>
        ''' Внутренняя функция копирующая имена фалов из FileInfo в 
        ''' список файлов (m_Files). Чтобы кто ни будь не попытался вызвать ее 
        ''' извне, объявляем ее как private.
        ''' </summary>
        Private Sub AppendFileNames(ByVal afi As FileInfo())
            If afi IsNot Nothing AndAlso afi.Length > 0 Then
                ' Если массив не пуст.
                ' Узнаем сколько имен файлов уже помещено в массив m_Files.
                Dim iCurLen As Integer = (If(m_Files Is Nothing, 0, m_Files.Length))
                ' Создаем новый массив размер которого достаточен, чтобы вместить
                ' дополнительные имена файлов. В принципе здесь можно было бы 
                ' воспользоваться ArrayList, для упрощения алгоритма, но и так,
                ' так тоже неплохо.
                Dim sTmp As String() = New String(iCurLen + (afi.Length - 1)) {}
                ' Если старый массив не пуст...
                If m_Files IsNot Nothing Then
                    ' Копируем элементы из старого массива в новый.
                    Array.Copy(m_Files, sTmp, m_Files.Length)
                End If
                ' Теперь iCurLen указывает на первый новый элемент.
                ' Но использовать переменную с таим именем будет нехорошо, по
                ' отношению к тем, кто будет читать этот код в будущем. Так что 
                ' заводим новую переменную для перебора оставшийся элементов массива.
                ' Оптимизация должна ее выкинуть.
                Dim i As Integer = iCurLen
                ' Перебираем список найденых файлов и копируем их имена в хвост массива.
                For Each fi As FileInfo In afi
                    sTmp(i) = fi.Name
                    i += 1
                Next
                ' Присваиваем в m_Files ссылку на новый массив.
                m_Files = sTmp
            End If
        End Sub

        ''' <summary>
        ''' Перебирает последовательно каждый найденный файл.
        ''' </summary>
        ''' <param name="Callback"></param>
        ''' <param name="sPath"></param>
        ''' <remarks></remarks>
        Friend Sub InternalIterateNode(ByVal Callback As FileCallback, ByVal sPath As String)
            ' Если это корневой объект...
            '(первый вызов приходится на корневой)
            If m_ParentDir IsNot Nothing Then
                sPath += m_sName & "\"c
            End If
            ' Добавляем имя корневого каталога.
            ' Перебираем все подкаталоги, вызывая этот же метод (рекурсивно).
            For Each Dir As AscDir In m_SubDir
                Dir.InternalIterateNode(Callback, DirectCast(sPath.Clone(), String))
            Next
            ' Если список имен файлов не пуст...
            If m_Files IsNot Nothing Then
                For Each File As String In m_Files
                    ' перебираем их..
                    Callback(sPath & File)
                Next
            End If
            ' и вызываем для каждого callback-метод. (в данном случае frmRegularExpressionsReplace.ProcessFile)
        End Sub
    End Class

End Class
