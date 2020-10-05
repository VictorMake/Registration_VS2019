''' <summary>
''' Диалоговое окно Find для поиска текста.
''' Созданный для SDI Записной книжки производит поиск предложения
''' Использование: 
''' глобальная переменные gFindCase (переключает регистр выбора);
''' gFindString (текст который ищут);
''' gFindDirection (переключатель поиска направление); 
''' gFirstTime (переключает продолжить поиск от начало текста) 
''' </summary>
''' <remarks></remarks>
Public Class FormFind
    Private optDirection(1) As RadioButton

    Private Sub frmFind_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' Не отключите находящуюся кнопку - никакой текст, чтобы найти все же.
        cmdFind.Enabled = False
        optDirection(0) = optDirection0
        optDirection(1) = optDirection1
        ' Чтение глобальной переменной, и установка переключателя.
        optDirection(FindDirection).Checked = True
    End Sub

    Private Sub chkCase_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCase.CheckStateChanged
        ' Назначить значение на глобальную переменную.
        FindCase = chkCase.Checked
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdcancel.Click
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' Сохранить значения в глобальных переменных.
        FindString = txtFind.Text
        FindCase = chkCase.Checked
        ' Выгрузка диалога.
        Close()
    End Sub

    Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click
        ' Назначает текстовую строку в глобальную переменную.
        FindString = txtFind.Text
        FindIt()
    End Sub

    Private Sub txtFind_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFind.TextChanged
        ' Установить глобальную переменную.
        IsFirstTime = True
        ', если textbox пуст, отключить находящуюся кнопку.
        If txtFind.Text = vbNullString Then
            cmdFind.Enabled = False
        Else
            cmdFind.Enabled = True
        End If
    End Sub

    Private Sub optDirection_CheckedChanged(ByVal Index As Short) '(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        FindDirection = Index
    End Sub

    Private Sub optDirection0_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles optDirection0.CheckedChanged
        optDirection_CheckedChanged(0)
    End Sub

    Private Sub optDirection1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles optDirection1.CheckedChanged
        optDirection_CheckedChanged(1)
    End Sub
End Class