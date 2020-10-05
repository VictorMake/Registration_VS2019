Imports System.Collections.Generic

Friend Class PassportModule
    '--- Property variables -----------------------------------------------
    ''' <summary>
    ''' Подлежит Подключению
    ''' </summary>
    ''' <returns></returns>
    Public Property Enable() As Boolean
    Public Property NameModule() As String
    Public Property DescriptionModule() As String

    Public Sub New()
        NameModule = "Введите ИмяМодуля."
        DescriptionModule = "Введите ОписаниеМодуля."
    End Sub

    Public Sub New(ByVal имяМодуля As String, ByVal описаниеМодуля As String) ', ByVal МодульВидим As Boolean)
        NameModule = имяМодуля
        DescriptionModule = описаниеМодуля
        'МодульВидимValue = МодульВидим
    End Sub

    Public Shared Function LoadPassportModule(ByVal списокИменМодулейВКаталоге As List(Of String), ByVal списокОписанийМодулейВКаталоге As List(Of String)) As Dictionary(Of String, PassportModule)
        Dim passports As New Dictionary(Of String, PassportModule)

        For i As Integer = 0 To списокИменМодулейВКаталоге.Count - 1
            Dim newEmployee As New PassportModule(списокИменМодулейВКаталоге.Item(i), списокОписанийМодулейВКаталоге.Item(i))
            passports.Add(списокИменМодулейВКаталоге.Item(i), newEmployee)
        Next

        Return passports
    End Function
End Class
