Imports System.Collections.Generic

''' <summary>
''' Вспомогательный класс для форм при редактировании математических функций
''' </summary>
Friend Class MathMethods
    Implements IEnumerable

    Private mMethods As Dictionary(Of String, Method)

    Public Sub New()
        mMethods = New Dictionary(Of String, Method)
    End Sub

    Public ReadOnly Property GetMethods() As Dictionary(Of String, Method)
        Get
            Return mMethods
        End Get
    End Property

    Public ReadOnly Property Item(ByVal indexKey As String) As Method
        Get
            Return mMethods.Item(indexKey)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return mMethods.Count()
        End Get
    End Property

    Private mTestResult As String
    Public ReadOnly Property TestResult() As String
        Get
            Return mTestResult
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mMethods.GetEnumerator
    End Function

    Public Sub Remove(ByRef indexKey As String)
        mMethods.Remove(indexKey)
    End Sub

    Public Sub Clear()
        mMethods.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        mMethods = Nothing
        MyBase.Finalize()
    End Sub

    Public Sub Add(ByVal nameMethod As String, ByVal textMethod As String)
        If Not CheckMethod(nameMethod) Then Exit Sub

        mMethods.Add(nameMethod, New Method(nameMethod, textMethod))
    End Sub

    Private Function CheckMethod(ByVal nameMethod As String) As Boolean
        If mMethods.ContainsKey(nameMethod) Then
            Const caption As String = "Ошибка добавления метода"
            Dim text As String = $"Method с именем {nameMethod} уже существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        Return True
    End Function

    Public Sub PopulateMath()
        '       Name(Description)
        '       Math.E
        '       Math.LN10
        '       Math.LN2
        '       Math.LOG10E
        '       Math.LOG2E
        '       Math.PI
        '       Math.SQRT1_2
        '       Math.SQRT2

        'Public Methods (see also Protected Methods ) 
        '       Math.abs()
        '       Math.acos()
        '       Math.asin()
        '       Math.atan()
        '       Math.atan2()
        '       Math.ceil()
        '       Math.cos()
        '       Math.exp()
        '       Math.floor()
        '       Math.log()
        '       Math.max()
        '       Math.min()
        '       Math.pow()
        '       Math.random()
        '       Math.round()
        '       Math.sin()
        '       Math.sqrt()
        '       Math.tan()
        Add("квадратный корень", "Math.sqrt()")
        Add("синус", "Math.sin()")
        Add("косинус", "Math.cos()")
        Add("тангенс", "Math.tan()")
        Add("арксинус", "Math.asin()")
        Add("арккосинус", "Math.acos()")
        Add("арктангенс", "Math.atan()")
        Add("экспонента", "Math.exp()")
        Add("возведение в степень", "Math.pow(ARG1, 2)") 'Math.pow(2,3) == 8
        Add("натуральный логарифм", "Math.log()")
        Add("десятичный логарифм", "Math.LOG10E")
        Add("логарифм по основанию 2", "Math.LOG2E")
        Add("округление", "Math.round()")
        Add("абсолютное значение", "Math.abs()")
        Add("число е", "Math.E")
        Add("натуральный логарифм числа 10", "Math.LN10")
        Add("натуральный логарифм числа 2", "Math.LN2")
        Add("число ПИ", "Math.PI")
        Add("корень числа 2", "Math.SQRT2")
        Add("корень числа 0.5", "Math.SQRT1_2")
    End Sub

    Public Function TestFunction(ByVal expression As String) As Boolean
        Dim eval As New JScriptUtil.ExpressionEvaluator()
        Dim result As Double

        expression = expression.Replace("ARG", "1")

        Try
            result = CDbl(eval.Evaluate(expression))
            Const caption As String = "Тест формулы"
            Const text As String = "Математическое выражение корректно."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            mTestResult = "Математическое выражение корректно"
            Return True
        Catch ex As Exception
            Const caption As String = "Ошибка в формуле"
            Dim text As String = $"{ex}{vbCrLf}{vbCrLf}Математическое выражение содержит ошибку.{vbCrLf}Исправьте и попробуйте протестировать снова."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            mTestResult = "Математическое выражение содержит ошибку"
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Элементарная математическая функция
    ''' </summary>
    Friend Class Method
        Public Property NameMethod() As String
        Public Property TextMethod() As String

        Public Sub New(ByVal nameMethod As String, ByVal textMethod As String)
            Me.NameMethod = nameMethod
            Me.TextMethod = textMethod
        End Sub
    End Class
End Class
