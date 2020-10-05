''' <summary>
''' "Поиск канала по имени"
''' </summary>
Public Class SearchChannel
    Private Enum SearchEnum
        SearchOneListView
        SearchTwoListView
        SearchComboBox
    End Enum

    Private ReadOnly mySearchEnum As SearchEnum
    Private ReadOnly ListViewSource As ListView
    Private ReadOnly ListViewReceiver As ListView
    Private ReadOnly ComboBoxParameters As ComboBox

    Public Sub New(inListViewSource As ListView, inListViewReceiver As ListView)
        Me.ListViewSource = inListViewSource
        Me.ListViewReceiver = inListViewReceiver
        mySearchEnum = SearchEnum.SearchTwoListView
    End Sub

    Public Sub New(inListViewSource As ListView)
        Me.New(inListViewSource, New ListView)
        mySearchEnum = SearchEnum.SearchOneListView
    End Sub

    Public Sub New(inComboBoxParameters As ComboBox)
        Me.ComboBoxParameters = inComboBoxParameters
        mySearchEnum = SearchEnum.SearchComboBox
    End Sub

    ''' <summary>
    ''' Выделить строку найденного канала
    ''' </summary>
    Public Sub SelectChannel()
        Using frmSearchChannel As New FormSearchChannelTuning()
            If frmSearchChannel.ShowDialog = DialogResult.OK Then
                Dim nameChannel As String = frmSearchChannel.ChannelName
                Select Case mySearchEnum
                    Case SearchEnum.SearchOneListView
                        SelectChannelFromFinding(nameChannel, ListViewSource)
                    Case SearchEnum.SearchTwoListView
                        SelectChannelFromFinding(nameChannel, ListViewSource, ListViewReceiver)
                    Case SearchEnum.SearchComboBox
                        SelectChannelFromFinding(nameChannel, ComboBoxParameters)
                End Select
            End If
        End Using
    End Sub

    Private Sub SelectChannelFromFinding(inNameChannel As String, inListViewSource As ListView)
        Dim foundedListViews As ListViewItem() = inListViewSource.Items.Find(inNameChannel, False)
        If foundedListViews.Length > 0 Then
            ListViewsSelect(foundedListViews, inListViewSource)
        End If
    End Sub

    Private Sub SelectChannelFromFinding(inNameChannel As String, inListViewSource As ListView, inListViewReceiver As ListView)
        Dim foundedListViews As ListViewItem() = inListViewSource.Items.Find(inNameChannel, False)
        If foundedListViews.Length > 0 Then
            ListViewsSelect(foundedListViews, inListViewSource)
        Else
            foundedListViews = inListViewReceiver.Items.Find(inNameChannel, False)
            ListViewsSelect(foundedListViews, inListViewReceiver)
        End If
    End Sub

    Private Sub ListViewsSelect(infoundedListViews As ListViewItem(), inListView As ListView)
        If infoundedListViews.Length > 0 Then
            inListView.Focus()
            For I As Integer = 0 To inListView.Items.Count - 1
                inListView.Items(I).Selected = False
            Next
            'Dim nameParameter As String = foundedListViews(0).Text
            infoundedListViews(0).EnsureVisible()
            infoundedListViews(0).Selected = True
        End If
    End Sub

    Private Sub SelectChannelFromFinding(inNameChannel As String, inComboBoxParameters As ComboBox)
        Dim itemStringIntObject As StringIntObject = inComboBoxParameters.Items.Cast(Of StringIntObject).ToList.Find(Function(x) x.s = inNameChannel)
        If itemStringIntObject IsNot Nothing Then
            inComboBoxParameters.Focus()
            inComboBoxParameters.SelectedItem = itemStringIntObject
        End If
    End Sub
End Class
