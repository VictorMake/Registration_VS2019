Imports System.Text.RegularExpressions

Friend Class IPAddressEditorForm
    ''' <summary>
    ''' Форма для редактирования ip-адреса
    ''' </summary>
    Public Sub New(ByVal ipMemo As IPAddressCls)
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        IP = ipMemo
    End Sub

    ''' <summary>
    ''' Преобразование представления ip в MaskedTextBox
    ''' и в нормальном виде
    ''' </summary>
    Public Property IP() As IPAddressCls
        ' MaskedTextBox добивает короткие части ip пробелами до 3-х знаков
        ' эти пробелы нужно удалять
        Get
            Return New IPAddressCls(IPmaskedTextBox.Text.Replace(" ", ""))
        End Get

        ' а здесь наоборот - надо выровнять пробелами
        Set(ByVal value As IPAddressCls)
            ' если части IP короче 3 знаков, MaskedTextBox их слепляет :(
            ' надо их при необходимости подравнять пробелами
            ' чтоб легли в маску
            Dim r As New Regex("^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$")
            Dim m As Match = r.Match(value.IP)
            Dim ip4maskedit As String = ""
            Dim firstgroup As Boolean = True

            For Each g As Group In m.Groups
                If firstgroup Then
                    firstgroup = False
                    Continue For
                End If

                Dim s As String = g.ToString()
                While s.Length < 3
                    s &= " "
                End While

                ip4maskedit &= s
            Next

            IPmaskedTextBox.Text = ip4maskedit
        End Set
    End Property
End Class