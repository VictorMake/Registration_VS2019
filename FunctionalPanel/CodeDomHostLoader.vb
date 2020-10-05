Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.IO
Imports System.Reflection
Imports System.CodeDom.Compiler
Imports System.CodeDom
Imports Microsoft.CSharp


''' <summary>
''' Наследуется от CodeDomDesignerLoader. Он генерирует C# или VB код для HostSurface.
''' Этот загрузчик не поддерживает парсинг C# или VB файлов.
''' </summary>
Public Class CodeDomHostLoader
    Inherits CodeDomDesignerLoader

    Private _csCodeProvider As New CSharpCodeProvider()
    Private codeCompileUnit As CodeCompileUnit = Nothing
    Private cg As CodeGen = Nothing
    Private _trs As TypeResolutionService = Nothing
    Private executable As String
    Private runProcess As Process

    Public Sub New()
        _trs = New TypeResolutionService()
    End Sub

    Protected Overloads Overrides ReadOnly Property TypeResolutionService() As ITypeResolutionService
        Get
            Return _trs
        End Get
    End Property

    Protected Overloads Overrides ReadOnly Property CodeDomProvider() As CodeDomProvider
        Get
            Return _csCodeProvider
        End Get
    End Property

    ''' <summary>
    ''' Загрузочный метод - загрузка бланк Form
    ''' </summary>
    ''' <returns></returns>
    Protected Overloads Overrides Function Parse() As CodeCompileUnit
        Dim ccu As CodeCompileUnit = Nothing

        Dim ds As New DesignSurface()
        ds.BeginLoad(GetType(Form))
        Dim idh As IDesignerHost = DirectCast(ds.GetService(GetType(IDesignerHost)), IDesignerHost)
        idh.RootComponent.Site.Name = "Form1"

        cg = New CodeGen()
        ccu = cg.GetCodeCompileUnit(idh)

        Dim names As AssemblyName() = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
        For i As Integer = 0 To names.Length - 1
            Dim assembly__1 As Assembly = Assembly.Load(names(i))
            ccu.ReferencedAssemblies.Add(assembly__1.Location)
        Next

        codeCompileUnit = ccu
        Return ccu
    End Function

    ''' <summary>
    ''' Этот метод вызывается когда Загрузчик создан в Потоке. 
    ''' Базовый класс (CodeDomDesignerLoader) создаёт программный граф CodeCompileUnit.
    ''' Метод просто кеширует CodeCompileUnit и использует его, когда необходимо генерировать код из него.
    ''' Для генерирования кода используется CodeProvider.
    ''' </summary>
    Protected Overloads Overrides Sub Write(ByVal unit As CodeCompileUnit)
        codeCompileUnit = unit
    End Sub

    ''' <summary>
    ''' Проверить ошибки компиляции и вывести в окно диагностики.
    ''' </summary>
    ''' <param name="successful"></param>
    ''' <param name="errors"></param>
    ''' <remarks></remarks>
    Protected Overloads Overrides Sub OnEndLoad(ByVal successful As Boolean, ByVal errors As ICollection)
        MyBase.OnEndLoad(successful, errors)
        If errors IsNot Nothing Then
            Dim ie As IEnumerator = errors.GetEnumerator()
            While ie.MoveNext()
                Trace.WriteLine(ie.Current.ToString())
            End While
        End If
    End Sub

#Region "Public methods"
    ''' <summary>
    ''' Сбросить host и вернуть обновлённый CodeCompileUnit
    ''' </summary>
    ''' <returns></returns>
    Public Function GetCodeCompileUnit() As CodeCompileUnit
        Flush()
        Return codeCompileUnit
    End Function

    ''' <summary>
    ''' Этот метод записывает содержимое дизайнера в C# и VB.
    ''' Он генерирует программный граф кода из codeCompileUnit используя CodeRpovider
    ''' </summary>
    Public Function GetCode(ByVal context As String) As String
        Flush()

        Dim codGenOpt As New CodeGeneratorOptions() With {.BlankLinesBetweenMembers = True, .BracingStyle = "C", .ElseOnClosing = False, .IndentString = "    "}

        If context = "C#" Then
            Dim swCS As New StringWriter()
            Dim cs As New CSharpCodeProvider()

            cs.GenerateCodeFromCompileUnit(codeCompileUnit, swCS, codGenOpt)
            Dim code As String = swCS.ToString()
            swCS.Close()

            Return code
        ElseIf context = "VB" Then
            Dim swVB As New StringWriter()
            Dim vb As New VBCodeProvider()

            vb.GenerateCodeFromCompileUnit(codeCompileUnit, swVB, codGenOpt)
            Dim code As String = swVB.ToString()
            swVB.Close()

            Return code
        End If

        Return [String].Empty
    End Function
#End Region

#Region "Build and Run"

    ''' <summary>
    ''' Вызывается когда необходимо создать исполнительный файл. Возвращает true если успешно.
    ''' </summary>
    Public Function Build() As Boolean
        Flush()

        ' Если ещё не выбрана точка записи исполняемого файла, то выполнить именно сейчас.
        If executable Is Nothing Then
            Dim dlg As New SaveFileDialog() With {.DefaultExt = "exe",
                                                  .Filter = "Executables|*.exe"}
            If dlg.ShowDialog() = DialogResult.OK Then
                executable = dlg.FileName
            End If
        End If

        If executable IsNot Nothing Then
            ' Необходимо собрать параметры, которые компилятор будет использовать.
            Dim cp As New CompilerParameters()
            Dim assemblyNames As AssemblyName() = Assembly.GetEntryAssembly().GetReferencedAssemblies()

            For Each an As AssemblyName In assemblyNames
                Dim assembly__1 As Assembly = Assembly.Load(an)
                cp.ReferencedAssemblies.Add(assembly__1.Location)
            Next

            cp.GenerateExecutable = True
            cp.OutputAssembly = executable

            ' Запомнить основной класс не Form, а Form1 (или как-либо пользователь его назвал)
            cp.MainClass = "DesignerHostSample." & Convert.ToString(LoaderHost.RootComponent.Site.Name)

            Dim cc As New CSharpCodeProvider()
            Dim cr As CompilerResults = cc.CompileAssemblyFromDom(cp, codeCompileUnit)

            If cr.Errors.HasErrors Then
                Dim errors As String = ""

                For Each [error] As CompilerError In cr.Errors
                    errors += [error].ErrorText & vbLf
                Next

                Const caption As String = "Ошибка в процессе компиляции."
                Dim text As String = errors
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If

            Return Not cr.Errors.HasErrors
        End If

        Return False
    End Function

    ''' <summary>
    ''' Создать исполняемый файл и запустить его.
    ''' Делается проверка, что не запущены дважды одни и те-же процессы.
    ''' </summary>
    Public Sub Run()
        If (runProcess Is Nothing) OrElse (runProcess.HasExited) Then
            If Build() Then
                runProcess = New Process()
                runProcess.StartInfo.FileName = executable
                runProcess.Start()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Просто в случае выбора кнопки закрытия (красный крест Х с правого верха) не корректно.
    ''' Необходимо здесь убить процесс.
    ''' </summary>
    Public Sub [Stop]()
        If (runProcess IsNot Nothing) AndAlso (Not runProcess.HasExited) Then
            runProcess.Kill()
        End If
    End Sub

#End Region
End Class

''' <summary>
''' Класс использует CodeDomHostLoader для генерации CodeCompleUnit
''' </summary>
Friend Class CodeGen
    Private codeCompileUnit As CodeCompileUnit
    Private ns As CodeNamespace
    Private myDesignerClass As New CodeTypeDeclaration()
    Private initializeComponent As New CodeMemberMethod()
    Private host As IDesignerHost
    Private root As IComponent

    'Private Shared ReadOnly propertyAttributes As Attribute() = New Attribute() {DesignOnlyAttribute.No}

    ''' <summary>
    ''' Это функция генерирует по умолчанию CodeCompileUnit шаблон
    ''' </summary>
    Public Function GetCodeCompileUnit(ByVal host As IDesignerHost) As CodeCompileUnit
        Me.host = host
        Dim idh As IDesignerHost = DirectCast(Me.host.GetService(GetType(IDesignerHost)), IDesignerHost)
        root = idh.RootComponent
        'Dim nametable As New Hashtable(idh.Container.Components.Count)

        ns = New CodeNamespace("DesignerHostSample")
        myDesignerClass = New CodeTypeDeclaration()
        initializeComponent = New CodeMemberMethod()

        Dim code As New CodeCompileUnit()

        ' Imports
        ns.[Imports].Add(New CodeNamespaceImport("System"))
        ns.[Imports].Add(New CodeNamespaceImport("System.ComponentModel"))
        ns.[Imports].Add(New CodeNamespaceImport("System.Windows.Forms"))
        code.Namespaces.Add(ns)
        myDesignerClass = New CodeTypeDeclaration(root.Site.Name)
        myDesignerClass.BaseTypes.Add(GetType(Form).FullName)

        ' не отключать
        Dim manager As IDesignerSerializationManager = TryCast(host.GetService(GetType(IDesignerSerializationManager)), IDesignerSerializationManager)

        ns.Types.Add(myDesignerClass)

        ' Constructor
        Dim con As New CodeConstructor() With {.Attributes = MemberAttributes.[Public]}
        con.Statements.Add(New CodeMethodInvokeExpression(New CodeMethodReferenceExpression(New CodeThisReferenceExpression(), "InitializeComponent")))
        myDesignerClass.Members.Add(con)

        ' Main
        Dim main As New CodeEntryPointMethod() With {.Name = "Main", .Attributes = MemberAttributes.[Public] Or MemberAttributes.[Static]}
        main.CustomAttributes.Add(New CodeAttributeDeclaration("System.STAThreadAttribute"))
        main.Statements.Add(New CodeMethodInvokeExpression(New CodeMethodReferenceExpression(New CodeTypeReferenceExpression(GetType(Application)), "Run"), New CodeExpression() {New CodeObjectCreateExpression(New CodeTypeReference(root.Site.Name))}))
        myDesignerClass.Members.Add(main)

        ' InitializeComponent
        initializeComponent.Name = "InitializeComponent"
        initializeComponent.Attributes = MemberAttributes.[Private]
        initializeComponent.ReturnType = New CodeTypeReference(GetType(Void))
        myDesignerClass.Members.Add(initializeComponent)
        codeCompileUnit = code
        Return codeCompileUnit
    End Function
End Class

''' <summary>
''' Этот сервис разрешает типы и требуется когда используется CodeDomHostLoader
''' </summary>
Public Class TypeResolutionService
    Implements ITypeResolutionService

    Private ht As New Hashtable()

    Public Sub New()
    End Sub

    Public Function GetAssembly(ByVal name As AssemblyName) As Assembly Implements ITypeResolutionService.GetAssembly
        Return GetAssembly(name, True)
    End Function

    Public Function GetAssembly(ByVal name As AssemblyName, ByVal throwOnErrors As Boolean) As Assembly Implements ITypeResolutionService.GetAssembly
        Return Assembly.GetAssembly(GetType(Form))
    End Function

    Public Function GetPathOfAssembly(ByVal name As AssemblyName) As String Implements ITypeResolutionService.GetPathOfAssembly
        Return Nothing
    End Function

    Public Overloads Function [GetType](ByVal name As String) As Type Implements ITypeResolutionService.GetType
        Return Me.GetType(name, True)
    End Function

    Public Overloads Function [GetType](ByVal name As String, ByVal throwOnError As Boolean) As Type Implements ITypeResolutionService.GetType
        Return Me.GetType(name, throwOnError, False)
    End Function

    ''' <summary>
    ''' Этот метод вызывается когда бросается контрол из панели инструментов на хост,
    ''' что загружен используя CodeDomHostLoader. 
    ''' Для упрощения просто идти сквозь System.Windows.Forms assembly.
    ''' </summary>
    Public Overloads Function [GetType](ByVal name As String, ByVal throwOnError As Boolean, ByVal ignoreCase As Boolean) As Type Implements ITypeResolutionService.GetType
        If ht.ContainsKey(name) Then
            Return DirectCast(ht(name), Type)
        End If

        Dim winForms As Assembly = Assembly.GetAssembly(GetType(Button))
        Dim types As Type() = winForms.GetTypes()
        Dim typeName As String = [String].Empty

        For Each type__1 As Type In types
            typeName = "system.windows.forms." & type__1.Name.ToLower()
            If typeName = name.ToLower() Then
                ht(name) = type__1
                Return type__1
            End If
        Next
        Return Type.GetType(name)
    End Function

    Public Sub ReferenceAssembly(ByVal name As AssemblyName) Implements ITypeResolutionService.ReferenceAssembly
    End Sub
End Class


