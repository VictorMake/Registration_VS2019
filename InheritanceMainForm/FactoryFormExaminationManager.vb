Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection

''' <summary>
''' Менеджер управления дочерними основными окнами.
''' Абстрактная фабрика осведомлена о всех конкретных класссах и способна порождать их экземпляры. 
''' Метод CreateFormMain создает класс заданного типа и назначает ему медиатор. 
''' </summary>
''' <remarks></remarks>
Friend Class FactoryFormExaminationManager
    Implements IEnumerable

#Region "Interface"
    ''' <summary>
    ''' Оболочка коллекции окон
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property AllLoadedFormsMain() As Dictionary(Of String, FormMain)
        Get
            Return loadedFormsMain
        End Get
    End Property

    ''' <summary>
    ''' элемент коллекции
    ''' </summary>
    ''' <param name="vntIndexKey"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Item(ByVal vntIndexKey As String) As FormMain
        Get
            If Not loadedFormsMain.Keys.Contains(vntIndexKey) Then
                Throw New ArgumentOutOfRangeException(vntIndexKey, "Окно с таким именем не может быть загружено.")
            End If
            Return loadedFormsMain.Item(vntIndexKey)
        End Get
    End Property

    ''' <summary>
    ''' число текущих загруженных форм
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count() As Integer
        Get
            Return loadedFormsMain.Count
        End Get
    End Property

    ''' <summary>
    ''' перечислитель
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return loadedFormsMain.GetEnumerator
    End Function

    ''' <summary>
    ''' удаление по номеру или имени или объекту?
    ''' </summary>
    ''' <param name="vntIndexKey"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByRef vntIndexKey As String)
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        loadedFormsMain.Remove(vntIndexKey)
        Me.mFormMainCreated -= 1
    End Sub

    Public Sub Clear()
        loadedFormsMain.Clear()
    End Sub

    'Protected Overrides Sub Finalize()
    '    loadedFormsMain = Nothing
    '    MyBase.Finalize()
    'End Sub
#End Region

    Private Property Mediator As FormMainMDI
    'Private formMainToLoadList As List(Of String)      ' список подлежащих загрузке классов Основного Окна
    Private ReadOnly loadedFormsMain As New Dictionary(Of String, FormMain) ' внутренняя коллекция для управления расшифровками
    Private descriptionsFormExamination As List(Of String)    ' список описаний из типа перечислителя
    Private namesFormExamination As List(Of String)           ' список значений элементов из типа перечислителя
    Private mFormMainCreated As Integer ' внутренний счетчик для подсчета созданных форм можно использовать в заголовке
    Private Shared ReadOnly ChildWindows As Dictionary(Of FormExamination, Creator) = New Dictionary(Of FormExamination, Creator) From {
        {FormExamination.RegistrationSCXI, New CreatorRegistrationSCXI},
        {FormExamination.RegistrationClient, New CreatorRegistrationClient},
        {FormExamination.RegistrationTCP, New CreatorRegistrationTCP},
        {FormExamination.SnapshotViewingDiagram, New CreatorSnapshotViewingDiagram},
        {FormExamination.SnapshotPhotograph, New CreatorSnapshotPhotograph}
    }

    Public Sub New(mediator As FormMainMDI) ', inFormMainsToLoadList As List(Of String))
        Me.Mediator = mediator
        'formMainToLoadList = inFormMainsToLoadList
        'InitializemaFormsManager()
        PopulateListEnumNamesAndDescriptions()
    End Sub

    'Private Sub InitializemaFormsManager()
    '    Try
    '        'Паттерн        Абстрактная фабрика
    '        'Имя в проекте  FactoryFormExaminationManager()
    '        'Задача         Создавать конкретные модули проекта. Скрыть от главной формы все конкретные классы модулей проекта. 
    '        'Решение        Скрыть знание о конкретных классах 
    '        'Результат      Главная форма не знает конкретных классов модулей, и ее код остается неизменен при добавлении новых модулей в проект. 

    '        ' В качестве <Главного модуля> (TAppConsole) выступает основная форма. Она создает множество модулей через абстрактную фабрику AnalysisManager. 
    '        ' (Кроме этого TAppConsole создает экземпляр TAppConsoleForm – главного окна программы.)
    '        ' Для перечисления всех модулей программы используется перечисление FormExamination. 
    '        ' TAppConsole пробегается по значениям этого типа (через mEnumDescriptionsList) и вызывает фабрику для создания конкретной формы. 

    '        PopulateListEnumNamesAndDescriptions()

    '        ' по описанию перечислителя расшифровок, фабрика создаёт экземпляр класса расшифровки и добавляется в словарь менеджера
    '        For Each itemDescription As String In enumDescriptionsList
    '            If formMainToLoadList.Contains(itemDescription) Then
    '                Try
    '                    ' при создании автоматом добавляется в коллекцию
    '                    If Not CreateFormMain(itemDescription) Then ' там проверка на корректность
    '                        Dim caption As String = NameOf(InitializemaFormsManager)
    '                        Dim text As String = String.Format("Ошибка при создании класса Основного Окна: <{0}>{1}", itemDescription, vbCrLf)
    '                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                    End If
    '                Catch ex As Exception
    '                    Dim caption As String = NameOf(InitializemaFormsManager)
    '                    Const text As String = "Ошибка при создании класса Основного Окна: " & vbCrLf
    '                    MessageBox.Show(text & ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                    RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))
    '                End Try
    '            End If
    '        Next

    '    Catch ex As Exception
    '        MessageBox.Show($"Ошибка загрузки классов Основного Окна в {NameOf(InitializemaFormsManager)}",
    '                        $"Error: {ex.ToString}", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Заполнение списков описаний и
    ''' списков значений элементов из типа перечислителя.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateListEnumNamesAndDescriptions()
        descriptionsFormExamination = New List(Of String)
        namesFormExamination = New List(Of String)

        ' получить все аттрибуты перечислителя для создания списка возможных окон в системе
        For Each value In [Enum].GetValues(GetType(FormExamination))
            Dim fi As FieldInfo = GetType(FormExamination).GetField([Enum].GetName(GetType(FormExamination), value))
            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)

            If dna IsNot Nothing Then
                descriptionsFormExamination.Add(dna.Description)
            Else
                descriptionsFormExamination.Add("Нет описания")
            End If
        Next

        ' то же самое по другому
        'For Each c In TypeDescriptor.GetConverter(GetType(WindowsForms)).GetStandardValues
        '    Dim dna As DescriptionAttribute = GetType(WindowsForms).GetField([Enum].GetName(GetType(WindowsForms), c)).GetCustomAttributes(GetType(DescriptionAttribute), True)(0)
        '    If dna IsNot Nothing Then
        '        mListEnumDescriptions.Add(dna.Description)
        '    Else
        '        mListEnumDescriptions.Add(WindowsForms.РедакторПерекладок.ToString())
        '    End If
        'Next     

        namesFormExamination.AddRange([Enum].GetNames(GetType(FormExamination)).ToArray)
    End Sub

    ''' <summary>
    ''' Добавление класса расшифровки в коллекцию
    ''' </summary>
    ''' <param name="kindExamination"></param>
    ''' <param name="captionText"></param>
    ''' <returns></returns>
    Public Function CreateFormMain(ByVal kindExamination As FormExamination, ByVal captionText As String) As FormMain
        Dim clsFormExamination As FormMain = Nothing

        Try
            If ChildWindows.Keys.Contains(kindExamination) Then clsFormExamination = CType(ChildWindows(kindExamination).GetWindow(Mediator, captionText), FormMain)

            If clsFormExamination Is Nothing Then
                Dim CAPTION As String = NameOf(CreateFormMain)
                Dim text As String = $"Имя класса Основного Окна {captionText} не найдено."
                MessageBox.Show(text, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE(String.Format("<{0}> {1}", CAPTION, text))
                Return Nothing
            End If

            loadedFormsMain.Add(captionText, clsFormExamination)
            Me.mFormMainCreated += 1

            ' здесь провести проверку на корректность и только если прошло продолжить
            If loadedFormsMain.ContainsKey(captionText) Then
                ' добавить обработчик события Closed новой формы, которое используется здесь 
                ' чтобы знать когда она закрывается и обработать из централизованного места
                'AddHandler frm.Closed, AddressOf FormWindowsManager.PanelBaseForm_Closed

                AddHandler clsFormExamination.Closed, AddressOf Me.PanelBaseForm_Closed

                ' добавить обработчик события SaveWhileClosingCancelled такой чтобы знать
                ' использование прерывания Cancel button когда было напоминание сохранения несохраненных данных
                'AddHandler frm.SaveWhileClosingCancelled, AddressOf Forms.PanelBaseForm_SaveWhileClosingCancelled
                ' добавить обработчик события ExitApplicaiton чтобы знать когда необходимо выгрузить
                ' приложение выбрав Exit menu из формы
                'AddHandler frm.ExitApplication, AddressOf Forms.PanelBaseForm_ExitApplication
                'frm.Show() ' проверка успешна, значит показать форму 

                Return clsFormExamination
            Else
                Return Nothing
            End If

        Catch exp As Exception
            Dim caption As String = NameOf(CreateFormMain)
            Dim text As String = exp.Message
            MessageBox.Show(text, $"Процедура {NameOf(CreateFormMain)}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", caption, text))

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Это событие получено когда форма была признана корректной и закрывается
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PanelBaseForm_Closed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim frm As FormMain = CType(sender, FormMain)

            ' Удалить обработчики событий, добавленных при создании формы
            'RemoveHandler frm.Closed, AddressOf FormWindowsManager.PanelBaseForm_Closed
            RemoveHandler frm.Closed, AddressOf Me.PanelBaseForm_Closed

            'RemoveHandler frm.SaveWhileClosingCancelled, AddressOf Forms.PanelBaseForm_SaveWhileClosingCancelled
            'RemoveHandler frm.ExitApplication, AddressOf Forms.PanelBaseForm_ExitApplication

            ' вызвать функцию очистки 
            'FormsPanelManager.FormClosed(frm)
            ' удалить форму которая закрывается из внутренней коллекции
            'm_КоллекцияПанелейМоториста.Remove(frm.GetHashCode.ToString())
            'gMainForm.СнятьВыделениеСМенюМоториста(frm.Tag)
            'loadedFormsMain.Remove(frm.Tag)
            loadedFormsMain.Remove(CStr(frm.KindFormExamination))
            Me.mFormMainCreated -= 1

            ' если не имеется более форм, то выгрузить процесс
            ' это вызывается только из добавленных в коллекцию форм
            ' корневая форма это событие не вызывает
            'If m_КоллекцияПанелейМоториста.Count = 0 Then
            '    Application.Exit()
            ''End If

        Catch exp As Exception
            Dim text As String = exp.Message
            MessageBox.Show(text, $"Процедура {NameOf(PanelBaseForm_Closed)}", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION(String.Format("<{0}> {1}", "Закрытие Основной Формы", text))
        End Try
    End Sub
End Class
