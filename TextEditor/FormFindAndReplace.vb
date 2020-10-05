Public Class FormFindAndReplace

    Friend IsOKtoClose As Boolean = False

    Private Sub Close_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close_Button.Click
        Hide()
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnFindNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNext.Click
        Dim target As String = txtTarget.Text
        Dim find As String = txtFind.Text
        Dim nextIndex As Integer = txtTarget.SelectionStart

        If Not chkMatchCase.Checked Then
            target = target.ToUpper
            find = find.ToUpper
        End If

        If txtTarget.SelectedText.ToUpper = find.ToUpper Then
            If Not chkSearchUp.Checked Then
                nextIndex += 1
            End If
        End If

        nextIndex = MyFind(target, find, nextIndex, chkMatchCase.Checked, chkMatchWholeWord.Checked, Not chkSearchUp.Checked)

        If nextIndex > -1 Then
            txtTarget.Select(nextIndex, find.Length)
            txtTarget.ScrollToCaret()
        Else
            Const caption As String = "Поиск и замена"
            Const text As String = "Текст не найден!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If

        txtTarget.Focus()
    End Sub

    Friend Sub SetTarget(ByVal target As String)
        txtTarget.Text = target
    End Sub

    Private Sub frmFindAndReplace_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsOKtoClose Then
            Hide()
            ' после e.Cancel = True обработка If FindAndReplace.ShowDialog = Windows.Forms.DialogResult.OK Then не работает
            e.Cancel = True
        End If
    End Sub

    Private Sub btnReplaceAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReplaceAll.Click
        Dim target As String = txtTarget.Text
        Dim find As String = txtFind.Text
        Dim replace As String = txtReplace.Text
        Dim nextIndex As Integer = 0
        Dim numReplacements As Integer

        If Not chkMatchCase.Checked Then
            target = target.ToUpper
            find = find.ToUpper
        End If

        If txtTarget.SelectedText.ToUpper = find.ToUpper Then
            If Not chkSearchUp.Checked Then
                nextIndex += 1
            End If
        End If

        nextIndex = MyFind(target, find, nextIndex, chkMatchCase.Checked, chkMatchWholeWord.Checked)

        Do While nextIndex <> -1
            With txtTarget
                .SelectionStart = nextIndex
                .SelectionLength = find.Length
                .SelectedText = replace
            End With

            target = target.Substring(0, nextIndex) & replace & target.Substring(nextIndex + find.Length)
            nextIndex += replace.Length
            numReplacements += 1
            nextIndex = MyFind(target, find, nextIndex, chkMatchCase.Checked, chkMatchWholeWord.Checked)
        Loop

        Const caption As String = "Поиск и замена"
        Dim text As String = numReplacements & " замен сделано"
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
        RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        txtTarget.Focus()
    End Sub

    Private Sub btnReplace_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReplace.Click
        If txtTarget.SelectionLength > 0 Then
            txtTarget.SelectedText = txtReplace.Text
            txtTarget.Focus()
        Else
            Const caption As String = "Поиск и замена"
            Const text As String = "Нет текста в текущем выделении!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub

    Private Function MyFind(ByVal target As String, _
                            ByVal find As String, _
                            Optional ByVal startingPoint As Integer = 0, _
                            Optional ByVal isMatchCase As Boolean = False, _
                            Optional ByVal isMatchWholeWord As Boolean = False, _
                            Optional ByVal isSearchForward As Boolean = True) As Integer
        Dim nextIndex As Integer = startingPoint
        Dim isExitLoop As Boolean = False
        Dim isFoundMatch As Boolean = False

        If Not isMatchCase Then
            target = target.ToUpper
            find = find.ToUpper
        End If

        Do
            If isSearchForward Then
                nextIndex = target.IndexOf(find, nextIndex)
            Else
                nextIndex = target.LastIndexOf(find, nextIndex)
            End If

            If nextIndex = -1 Then
                isExitLoop = True
                isFoundMatch = False
            Else
                isFoundMatch = True
            End If

            If isFoundMatch AndAlso isMatchWholeWord Then
                Dim intCharacterToCheck As Integer = nextIndex - 1
                Dim chCharacterToCheck As Char

                If intCharacterToCheck >= 0 Then
                    chCharacterToCheck = target.Chars(intCharacterToCheck)

                    If Char.IsLetterOrDigit(chCharacterToCheck) Then
                        isFoundMatch = False

                        If Not chkSearchUp.Checked Then
                            nextIndex += find.Length
                        End If
                    End If
                End If

                intCharacterToCheck = nextIndex + find.Length
                If isFoundMatch AndAlso intCharacterToCheck < target.Length Then
                    chCharacterToCheck = target.Chars(intCharacterToCheck)

                    If Char.IsLetterOrDigit(chCharacterToCheck) Then
                        isFoundMatch = False

                        If Not chkSearchUp.Checked Then
                            nextIndex += find.Length
                        End If
                    End If
                End If
            End If
        Loop Until isFoundMatch Or isExitLoop

        Return nextIndex
    End Function
End Class
