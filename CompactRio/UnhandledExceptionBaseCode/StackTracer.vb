''' <summary>
''' Раскрутка стека программы
''' </summary>
''' <remarks></remarks>
Public Class StackTracer
    ''' <summary>
    ''' Получить из фрейма используя рефлексию текстовое представление, содержащие сведения о выполняемом методе
    ''' </summary>
    ''' <param name="ApplicationStack"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function StackFrameToString(ByVal applicationStack As StackFrame) As String
        Const LINE_BREAK As String = "{sep}"
        Dim sb As New Text.StringBuilder
        Dim paramIterator As Integer

        Dim discard As Boolean ' сбросить
        Dim mi As Reflection.MemberInfo = applicationStack.GetMethod ' метод, в котором выполняется кадр.

        With sb
            .Append(mi.DeclaringType.Namespace.StackSpacer) ' пространство имён
            .Append(".")
            .Append(mi.DeclaringType.Name) ' класс
            .Append(".")
            .Append(mi.Name) ' метод в фрейме

            ' определить параметры
            Dim parameters As Reflection.ParameterInfo() = applicationStack.GetMethod.GetParameters()
            Dim param As Reflection.ParameterInfo

            .Append("(")

            paramIterator = 0
            ' по каждому параметру определить имя и тип
            For Each param In parameters
                paramIterator += 1
                If paramIterator > 1 Then .Append(", ")
                .Append(param.Name) ' имя параметра для метода
                .Append(" как ")
                .Append(param.ParameterType.Name) ' какого типа параметр
            Next

            .Append(")")
            .Append(LINE_BREAK)

            ' узнать линию и колонку при наличии символов отладки в файле исполняемого кода
            If applicationStack.GetFileName Is Nothing OrElse applicationStack.GetFileName.Length = 0 Then
                ' получить данные из вспомогательного класса
                .Append(System.IO.Path.GetFileName(Helper.ParentAssembly.CodeBase.StackSpacer))
                .Append(": N ")
                .Append(String.Format("{0:#00000}", applicationStack.GetNativeOffset))
                discard = True
            Else
                ' имя файла, содержащего выполняемый код. Эти сведения обычно извлекаются из символов отладки для исполняемого файла.
                .Append(System.IO.Path.GetFileName(applicationStack.GetFileName).StackSpacer)
                .Append(": line ")
                .Append(String.Format("{0:#0000}", applicationStack.GetFileLineNumber))
                .Append(", col ")
                .Append(String.Format("{0:#00}", applicationStack.GetFileColumnNumber))
                discard = False
            End If

            .Append(LINE_BREAK)
        End With

        If discard Then
            ' проверить корректность сброса?
            Return ""
        Else
            mDetails = sb.ToString
            Return sb.ToString
        End If
    End Function

    ''' <summary>
    ''' Получить текстовое представление из всех фреймов трассировки стека
    ''' </summary>
    ''' <param name="ST"></param>
    ''' <param name="SkipClassName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function EnhancedStackTrace(ByVal ST As StackTrace, Optional ByVal SkipClassName As String = "") As String
        Dim sb As New System.Text.StringBuilder
        sb.AppendLine("")

        ' пройти по всем фреймам в стеке
        For Frame As Integer = 0 To ST.FrameCount - 1
            Dim sf As StackFrame = ST.GetFrame(Frame)
            Dim mi As Reflection.MemberInfo = sf.GetMethod ' метод, в котором выполняется кадр.
            Dim Results As String

            ' если класс объявивший метод не известен ("UnExceptionManager"), то пропустить
            If SkipClassName <> "" AndAlso mi.DeclaringType.Name.IndexOf(SkipClassName) > -1 Then
            Else
                Results = StackFrameToString(sf)
                If Results.Length > 0 Then
                    sb.Append(Results) ' извлекаются из символов отладки для исполняемого файла
                End If
            End If
        Next

        Return sb.ToString
    End Function

    ''' <summary>
    ''' Подробная раскрутка стека
    ''' </summary>
    ''' <param name="Ex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function EnhancedStackTrace(ByVal Ex As Exception) As String
        Dim ST As New StackTrace(Ex, True)
        Return EnhancedStackTrace(ST, String.Empty)
    End Function

    ''' <summary>
    ''' Подробная раскрутка стека
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function EnhancedStackTrace() As String
        Dim ST As New StackTrace(True)
        Return EnhancedStackTrace(ST, "UnExceptionManager")
    End Function

    Private Shared mDetails As String
    ''' <summary>
    ''' Детали содержимого стека программы
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Details() As String
        Get
            Return mDetails
        End Get
    End Property
End Class

''' <summary>
''' Для необработанного исключения общие вспомогательные методы
''' </summary>
''' <remarks></remarks>
Module Helper
    Private mParentAssembly As Reflection.Assembly = Nothing
    ''' <summary>
    ''' Модуль сборки или процесс
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ParentAssembly() As Reflection.Assembly
        If mParentAssembly Is Nothing Then
            If System.Reflection.Assembly.GetEntryAssembly Is Nothing Then
                ' объект System.Reflection.Assembly метода, который вызывает текущий выполняемый метод.
                mParentAssembly = System.Reflection.Assembly.GetCallingAssembly ' метод вызвавший текущее исключение
            Else
                ' процесс, исполняемый в домене приложения по умолчанию. 
                ' В других доменах приложений это первый исполняемый процесс, который был выполнен методом System.AppDomain.ExecuteAssembly(System.String). 
                ' Возвращаемые значения: Сборка, представляющая собой исполняемый файл процесса в домене приложения по умолчанию 
                ' или первый исполняемый файл, выполненный методом System.AppDomain.ExecuteAssembly(System.String).
                mParentAssembly = System.Reflection.Assembly.GetEntryAssembly ' процесс в домене приложения
            End If
        End If

        Return mParentAssembly
    End Function
End Module

''' <summary>
''' Вспомогательный расширяющий метод
''' </summary>
''' <remarks></remarks>
Module SpaceExts
    ''' <summary>
    ''' Определяет отступ линий в классе трассировке стека 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Diagnostics.DebuggerStepThrough()> _
    <System.Runtime.CompilerServices.Extension()> _
    Public Function StackSpacer(ByVal sender As String) As String
        Return String.Concat("        ", sender)
    End Function
End Module