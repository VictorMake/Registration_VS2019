'Friend Class Symbol
'    Private msKeyPhrase As String = "123456789"

'    WriteOnly Property Char_Renamed() As Short
'        Set(ByVal Value As Short)

'            Select Case Value
'                Case Keys.V : msKeyPhrase = Mid(msKeyPhrase, 2) & "V"
'                    Exit Select
'                Case Keys.I : msKeyPhrase = Mid(msKeyPhrase, 2) & "I"
'                    Exit Select
'                Case Keys.C : msKeyPhrase = Mid(msKeyPhrase, 2) & "C"
'                    Exit Select
'                Case Keys.T : msKeyPhrase = Mid(msKeyPhrase, 2) & "T"
'                    Exit Select
'                Case Keys.O : msKeyPhrase = Mid(msKeyPhrase, 2) & "O"
'                    Exit Select
'                Case Keys.R : msKeyPhrase = Mid(msKeyPhrase, 2) & "R"
'                    Exit Select
'                Case Keys.I : msKeyPhrase = Mid(msKeyPhrase, 2) & "I"
'                    Exit Select
'                Case Keys.N : msKeyPhrase = Mid(msKeyPhrase, 2) & "N"
'                    Exit Select
'                Case Keys.A : msKeyPhrase = Mid(msKeyPhrase, 2) & "A"
'                    Exit Select
'                Case Else : msKeyPhrase = "123456789"
'                    Exit Select
'            End Select
'            If msKeyPhrase = "VICTORINA" Then EasterEgg()
'        End Set
'    End Property

'    Private Sub EasterEgg()
'        MessageBox.Show("Правильно!!!", "EasterEgg", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        AboutForm.TryRgstrtn()
'    End Sub
'End Class