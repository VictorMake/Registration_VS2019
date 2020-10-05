Imports System.Collections.Generic

''' <summary>
'''  связывает два контрола ToolStripButton и ToolStripMenuItem
'''  для ссылочной манипуляции друг с другом
''' </summary>
''' <remarks></remarks>
Public Class UtilityHelper
    'Private helpIndex As Integer
    Private menuHelper As Dictionary(Of ToolStripItem, String)
    'Private helperStrings As String()
    'Private toolTips As String()
    Private ListPairs As List(Of Pair)


    Public Sub New()
        menuHelper = New Dictionary(Of ToolStripItem, String)
        'helpIndex = 0
        ListPairs = New List(Of Pair)
        ''можно так
        ''Dim _assembly As Assembly = Assembly.GetExecutingAssembly
        ''Dim _manager As ResourceManager = New ResourceManager("NationalInstruments.Examples.SimpleGraph.Strings", _assembly)

        'Dim manager As ResourceManager = New ResourceManager("NationalInstruments.Examples.SimpleGraph.Strings", GetType(MainForm).Assembly)

        ''If _manager.Equals(manager) Then
        ''    Debug.Print("_manager.Equals(manager) is True")
        ''End If
        ''If _manager.GetHashCode = manager.GetHashCode Then
        ''    Debug.Print("_manager.GetHashCode = manager.GetHashCode) is True")
        ''End If
        ''If _manager.GetType Is manager.GetType Then
        ''    Debug.Print("_manager.GetHashCode = manager.GetHashCode) is True")
        ''End If

        ''helperStrings = ParseResource(manager.GetString("helperStrings"))
        ''toolTips = ParseResource(manager.GetString("toolTips"))
        '' так как создан строго типизированный ресурс в свойстве Custom Tool -> ResXFileCodeGenerator, то можно обращаться к к типизированному
        '' свойству напрямую
        'helperStrings = ParseResource(Strings.helperStrings)
        'toolTips = ParseResource(Strings.toolTips)
    End Sub

    'Private Function ParseResource(ByVal temp As String) As String()
    '    Dim regex As Regex = New Regex("(\t| ){2,}")
    '    temp = regex.Replace(temp, "")
    '    Return System.Text.RegularExpressions.Regex.Split(temp, Environment.NewLine)
    'End Function

    Public Sub AddMenuString(ByVal key As ToolStripItem)
        'Debug.Assert(helpIndex >= 0 And helpIndex < helperStrings.Length, "Ни одна строка меню помощи не найдена help index")
        'menuHelper.Add(key, helperStrings(helpIndex))
        'helpIndex += 1
        menuHelper.Add(key, key.ToolTipText)
    End Sub

    'Public Function GetMenuString(ByVal key As ToolStripItem) As String
    '    Return menuHelper.Item(key)
    'End Function

    'Public Function GetToolTip(ByVal index As Integer) As String
    '    Debug.Assert(index >= 0 And index < toolTips.Length, "Введенный index неправильный для tooltip(index)")
    '    Return toolTips(index)
    'End Function

    Public Sub MapMenuAndToolBar(ByVal button As ToolStripButton, ByVal item As ToolStripMenuItem)
        ListPairs.Add(New Pair(button, item))
    End Sub

    Public Function FromToolBarButton(ByVal toolBarButton As ToolStripButton) As ToolStripMenuItem
        For Each itemPair As Pair In ListPairs
            If itemPair.Button.Equals(toolBarButton) Then
                Return itemPair.Item
            End If
        Next

        'Debug.Fail("Невозможно найти ToolStripMenuItem from the ToolStripButton переданного в параметре")
        Return Nothing
    End Function

    Public Function FromMenuItem(ByVal item As ToolStripMenuItem) As ToolStripButton
        For Each itemPair As Pair In ListPairs
            If itemPair.Item.Equals(item) Then
                Return itemPair.Button
            End If
        Next

        'Debug.Fail("Невозможно найти ToolStripButton для ToolStripMenuItem переданного в параметре")
        Return Nothing
    End Function

    Private Class Pair

        Public Sub New(ByVal buttonVal As ToolStripButton, ByVal itemVal As ToolStripMenuItem)
            Button = buttonVal
            Item = itemVal
        End Sub

        Public Property Button() As ToolStripButton

        Public Property Item() As ToolStripMenuItem
    End Class
End Class
