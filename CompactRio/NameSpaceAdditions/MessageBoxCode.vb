Namespace My
    ''' <summary>
    ''' Различные пользовательские диалоги для расширенного использования на будущее
    ''' </summary>
    ''' <remarks></remarks>
    <Global.System.ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)>
    Partial Friend Class _Dialogs
        Public Function Question(ByVal text As String) As Boolean
            Return (MessageBox.Show(text, My.Application.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes)
        End Function

        Public Function Question(ByVal text As String, ByVal caption As String) As Boolean
            Return (MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes)
        End Function

        Public Sub InformationDialog(ByVal text As String)
            MessageBox.Show(text, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Public Sub InformationDialog(ByVal text As String, ByVal caption As String)
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Public Sub ExceptionDialog(ByVal text As String)
            MessageBox.Show(text, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Public Sub ExceptionDialog(ByVal text As String, ByVal title As String)
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
    End Class

    <Global.Microsoft.VisualBasic.HideModuleName()>
    Friend Module KSG_Dialogs
        Private ReadOnly instance As New ThreadSafeObjectProvider(Of _Dialogs)
        ReadOnly Property Dialogs() As _Dialogs
            Get
                Return instance.GetInstance()
            End Get
        End Property
    End Module
End Namespace