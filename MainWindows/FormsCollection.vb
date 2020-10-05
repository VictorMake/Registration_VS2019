Public Class FormsCollection
    Inherits CollectionBase

    Private formsDictionary As Generic.Dictionary(Of String, Form) 

    Public Overloads Sub Add(ByVal keyCaption As String, ByVal valueForm As Form)
        If formsDictionary.ContainsKey(keyCaption) Then
            Const caption As String = "Добавление ссылки в коллекцию форм"
            Dim text As String = keyCaption + " есть в Hashtable"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        Else
            'MessageBox.Show(strKeyCaption + " нет в Hashtable")
            formsDictionary.Add(keyCaption, valueForm)
        End If
    End Sub

    Public Overloads Sub Remove(ByVal strKeyCaption As String)
        formsDictionary.Remove(strKeyCaption)
    End Sub

    Public ReadOnly Property Forms() As Generic.Dictionary(Of String, Form)
        Get
            Return formsDictionary
        End Get
    End Property

    Public Overloads ReadOnly Property Count() As Integer
        Get
            Return formsDictionary.Count
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        formsDictionary = New Generic.Dictionary(Of String, Form)
    End Sub
End Class
