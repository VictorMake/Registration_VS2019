Public Class FormWebBrowser

    Public property StartingAddress As String
    Private dontNavigateNow As Boolean

    Private Sub FormWebBrowser_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        'tbToolBar.CtlRefresh()
        If Len(StartingAddress) > 0 Then
            cboAddress.Text = StartingAddress
            cboAddress.Items.Add(cboAddress.Text)
            ' попробовать перейти по стартовому адресу
            timTimer.Enabled = True
            brwWebBrowser.Navigate(StartingAddress)
        End If

        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub brwWebBrowser_DownloadComplete(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles brwWebBrowser.DownloadComplete
        Try
            Text = brwWebBrowser.LocationName
        Catch ex As Exception
        End Try
    End Sub

    Private Sub brwWebBrowser_NavigateComplete2(ByVal eventSender As Object, ByVal eventArgs As AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event) Handles brwWebBrowser.NavigateComplete2
        Try
            Dim found As Boolean
            Dim I As Integer
            Text = brwWebBrowser.LocationName

            For I = 0 To cboAddress.Items.Count - 1
                If cboAddress.Items(I).ToString() = brwWebBrowser.LocationURL Then
                    found = True
                    Exit For
                End If
            Next

            dontNavigateNow = True

            If found Then
                cboAddress.Items.RemoveAt(I)
            End If

            cboAddress.Items.Insert(0, brwWebBrowser.LocationURL)
            cboAddress.SelectedIndex = 0
            dontNavigateNow = False
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cboAddress_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddress.SelectedIndexChanged
        If dontNavigateNow Then Exit Sub
        timTimer.Enabled = True
        brwWebBrowser.Navigate(cboAddress.Text)
    End Sub

    Private Sub cboAddress_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboAddress.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)

        Try
            If KeyAscii = Keys.Return Then
                cboAddress_SelectedIndexChanged(cboAddress, New EventArgs())
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormWebBrowser_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        brwWebBrowser.SetBounds(4, tbToolBar.Height + Panel1.Height + 2, ClientRectangle.Width - 4, ClientRectangle.Height - tbToolBar.Height - Panel1.Height - 6)
    End Sub

    Private Sub timTimer_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles timTimer.Tick
        If brwWebBrowser.Busy = False Then
            timTimer.Enabled = False
            Text = brwWebBrowser.LocationName
        Else
            Text = "Обработка..."
        End If
    End Sub

    Private Sub ToolBar1_ButtonClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs) Handles tbToolBar.ButtonClick
        Try
            timTimer.Enabled = True
            Select Case e.Button.Tag.ToString
                Case "Back"
                    brwWebBrowser.GoBack()
                    Exit Select
                Case "Forward"
                    brwWebBrowser.GoForward()
                    Exit Select
                Case "Refresh"
                    brwWebBrowser.Refresh() 'CtlRefresh()
                    Exit Select
                Case "Home"
                    brwWebBrowser.GoHome()
                    Exit Select
                Case "Search"
                    brwWebBrowser.GoSearch()
                    Exit Select
                Case "Stop"
                    timTimer.Enabled = False
                    brwWebBrowser.Stop()
                    Text = brwWebBrowser.LocationName
                    Exit Select
            End Select
        Catch ex As Exception
        End Try
    End Sub
End Class