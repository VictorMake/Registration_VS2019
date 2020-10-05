Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.IO

Public Enum LoaderType
    BasicDesignerLoader = 1
    CodeDomDesignerLoader = 2
    NoLoader = 3
End Enum

''' <summary>
''' Унаследованный класс, управляет коллекцией объектов System.ComponentModel.Design.DesignSurface.
''' Любые сервисы добавленные к HostSurfaceManager будут доступны во всех HostSurfaces
''' </summary>
''' <remarks></remarks>
Friend Class HostSurfaceManager
    Inherits DesignSurfaceManager

    Public Sub New()
        MyBase.New()

        AddService(GetType(INameCreationService), New NameCreationService())
        ' Происходит при изменении активного в текущий момент конструктора.
        AddHandler ActiveDesignSurfaceChanged, New ActiveDesignSurfaceChangedEventHandler(AddressOf HostSurfaceManager_ActiveDesignSurfaceChanged)
    End Sub

    Protected Overloads Overrides Function CreateDesignSurfaceCore(ByVal parentProvider As IServiceProvider) As DesignSurface
        Return New HostSurface(parentProvider)
    End Function

    ''' <summary>
    ''' Получить новый HostSurface и загрузить его с подходящим типом корневого компонента
    ''' </summary>
    Private Function GetNewHost(ByVal rootComponentType As Type) As HostControl
        Dim hostSurface As HostSurface = DirectCast(CreateDesignSurface(ServiceContainer), HostSurface)

        If rootComponentType Is GetType(Form) Then
            hostSurface.BeginLoad(GetType(Form))
            'изменил
            'ElseIf rootComponentType Is GetType(UserControl) Then
            '    hostSurface.BeginLoad(GetType(UserControl))
            'ElseIf rootComponentType Is GetType(Component) Then
            '    hostSurface.BeginLoad(GetType(Component))
            'ElseIf rootComponentType Is GetType(MyTopLevelComponent) Then
            '    hostSurface.BeginLoad(GetType(MyTopLevelComponent))
        Else
            Throw New Exception("Неопределенный Host Типа: " & rootComponentType.ToString())
        End If

        hostSurface.Initialize()
        ActiveDesignSurface = hostSurface

        Return New HostControl(hostSurface)
    End Function

    ''' <summary>
    ''' Получить новый HostSurface и загрузить его с подходящим типом корневого компонента
    ''' Использовать подходящий Загрузчик для загрузки HostSurface.
    ''' </summary>
    Public Function GetNewHost(ByVal rootComponentType As Type, ByVal loaderType__1 As LoaderType) As HostControl
        If loaderType__1 = LoaderType.NoLoader Then
            Return GetNewHost(rootComponentType)
        End If

        Dim hostSurface As HostSurface = DirectCast(CreateDesignSurface(ServiceContainer), HostSurface)
        Dim host As IDesignerHost = DirectCast(hostSurface.GetService(GetType(IDesignerHost)), IDesignerHost)

        Select Case loaderType__1
            Case LoaderType.BasicDesignerLoader
                Dim basicHostLoader As New BasicHostLoader(rootComponentType)
                hostSurface.BeginLoad(basicHostLoader)
                hostSurface.Loader = basicHostLoader
                Exit Select
            Case LoaderType.CodeDomDesignerLoader
                Dim codeDomHostLoader As New CodeDomHostLoader
                hostSurface.BeginLoad(codeDomHostLoader)
                hostSurface.Loader = codeDomHostLoader
                Exit Select
            Case Else
                Throw New Exception("Загрузчик не определен: " & loaderType__1.ToString())
        End Select

        hostSurface.Initialize()

        Return New HostControl(hostSurface)
    End Function

    ''' <summary>
    ''' Открыть Xml файл и загрузить его используя BasicHostLoader (унаследованного от BasicDesignerLoader)
    ''' </summary>
    Public Function GetNewHost(ByVal fileName As String) As HostControl
        If fileName Is Nothing OrElse Not File.Exists(fileName) Then
            Const caption As String = "GetNewHost"
            Dim text As String = "Файл не корректен: " & fileName
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        Dim loaderType__1 As LoaderType

        If fileName.EndsWith("xml") Then
            loaderType__1 = LoaderType.BasicDesignerLoader
        Else
            loaderType__1 = LoaderType.NoLoader
        End If

        If loaderType__1 = LoaderType.NoLoader OrElse loaderType__1 = LoaderType.CodeDomDesignerLoader Then
            Throw New Exception("Файл не может быть открыт. Проверьте тип расширения файла. Поддерживаемый формат - Xml")
        End If

        Dim hostSurface As HostSurface = DirectCast(CreateDesignSurface(ServiceContainer), HostSurface)
        Dim host As IDesignerHost = DirectCast(hostSurface.GetService(GetType(IDesignerHost)), IDesignerHost)

        Dim basicHostLoader As New BasicHostLoader(fileName)
        hostSurface.BeginLoad(basicHostLoader)
        hostSurface.Loader = basicHostLoader
        hostSurface.Initialize()

        Return New HostControl(hostSurface)
    End Function

    ''' <summary>
    ''' Добавляет заданную службу в контейнер служб.
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="serviceInstance"></param>
    ''' <remarks></remarks>
    Public Sub AddService(ByVal type As Type, ByVal serviceInstance As Object)
        ServiceContainer.AddService(type, serviceInstance)
    End Sub

    ''' <summary>
    ''' Использовать OutputWindow сервис и записать его
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HostSurfaceManager_ActiveDesignSurfaceChanged(ByVal sender As Object, ByVal e As ActiveDesignSurfaceChangedEventArgs)
        'Dim o As ToolWindows.OutputWindow = TryCast(Me.GetService(GetType(ToolWindows.OutputWindow)), ToolWindows.OutputWindow)
        'o.RichTextBox.Text += "Новый host добавлен." & vbLf

        ' изменил
        Dim tsLabel As ToolStripStatusLabel = TryCast(GetService(GetType(ToolStripStatusLabel)), ToolStripStatusLabel)
        tsLabel.Text = "Добавлен новый Хост дизайнера."
    End Sub
End Class


''' <summary>
''' Наследование из DesignSurface и хост RootComponent
''' и всех остальныйх дизайнеров. Это также использует загрузчик (BasicDesignerLoader
''' или CodeDomDesignerLoader) когда необходимо. Также представлены переменные
''' сервиса дизайнера. Добавлен MenuCommandService который используется для Cut, Copy, Paste, и т.д.
''' </summary>
Friend Class HostSurface
    Inherits DesignSurface

    Private _loader As BasicDesignerLoader
    Private _selectionService As ISelectionService

    Public Sub New()
        MyBase.New()
        AddService(GetType(IMenuCommandService), New MenuCommandService(Me))
    End Sub

    Public Sub New(ByVal parentProvider As IServiceProvider)
        MyBase.New(parentProvider)
        AddService(GetType(IMenuCommandService), New MenuCommandService(Me))
    End Sub

    Friend Sub Initialize()
        Dim control As Control = Nothing
        Dim host As IDesignerHost = DirectCast(GetService(GetType(IDesignerHost)), IDesignerHost)

        If host Is Nothing Then
            Return
        End If

        Try
            ' Установить backcolor
            Dim hostType As Type = host.RootComponent.[GetType]()
            If hostType Is GetType(Form) Then
                control = TryCast(View, Control)
                control.BackColor = Color.White
                'ElseIf hostType Is GetType(UserControl) Then
                '    control = TryCast(Me.View, Control)
                '    control.BackColor = Color.White
                'ElseIf hostType Is GetType(Component) Then
                '    control = TryCast(Me.View, Control)
                '    control.BackColor = Color.FloralWhite
            Else
                Throw New Exception("Неопределенный Host Типа: " & hostType.ToString())
            End If

            ' Установка SelectionService - SelectionChanged указатель события
            _selectionService = DirectCast((ServiceContainer.GetService(GetType(ISelectionService))), ISelectionService)
            AddHandler _selectionService.SelectionChanged, New EventHandler(AddressOf SelectionService_SelectionChanged)
        Catch ex As Exception
            'Trace.WriteLine(ex.ToString())
            Const caption As String = "InitializeHost"
            Dim text As String = ex.ToString
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Предоставляет базовый интерфейс System.ComponentModel.Design.Serialization.IDesignerLoaderService.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Loader() As BasicDesignerLoader
        Get
            Return _loader
        End Get
        Set(ByVal value As BasicDesignerLoader)
            _loader = value
        End Set
    End Property

    ''' <summary>
    ''' Когда выделение изменено этот набор PropertyGrid's выделяет компонент
    ''' </summary>
    Private Sub SelectionService_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        If _selectionService IsNot Nothing Then
            Dim selectedComponents As ICollection = _selectionService.GetSelectedComponents()
            Dim propertyGrid As PropertyGrid = DirectCast(GetService(GetType(PropertyGrid)), PropertyGrid)
            Dim comps As Object() = New Object(selectedComponents.Count - 1) {}
            Dim i As Integer = 0

            For Each o As Object In selectedComponents
                comps(i) = o
                i += 1
            Next

            propertyGrid.SelectedObjects = comps
            If comps.Count = 1 Then
                EditorPanelMotoristForm.ShowPropertyControl(comps(0))
            End If
        End If
    End Sub

    Public Sub AddService(ByVal type As Type, ByVal serviceInstance As Object)
        ServiceContainer.AddService(type, serviceInstance)
    End Sub
End Class