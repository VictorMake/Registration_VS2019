''' <summary>
''' Вывести накопленную ошибку во всех классах
''' </summary>
Friend Class ShowTotalErrorsMessage
    Public Shared Sub ShowMessage(IsTotalErrors As Boolean, totalErrorsMessage As String)
        'если накопленная ошибка во всех классах
        If IsTotalErrors Then
            MessageBox.Show(totalErrorsMessage, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class
