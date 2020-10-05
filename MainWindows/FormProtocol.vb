Friend Class FormProtocol
    Dim protocol As String(,)

    Sub New(inProtocol As String(,))
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        protocol = inProtocol
    End Sub

    Private Sub FormProtocol_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        Dim rows As Integer = UBound(protocol)
        Dim columns As Integer = UBound(protocol, 2)
        ' установить форму по размеру строк
        DGVProtocol.Rows.Add(rows)
        Dim totalWidth As Integer = 0
        Dim totalHeight As Integer = DGVProtocol.ColumnHeadersHeight

        For Each col As DataGridViewColumn In DGVProtocol.Columns
            totalWidth += (col.Width + col.DividerWidth)
        Next

        For Each row As DataGridViewRow In DGVProtocol.Rows
            totalHeight += (DGVProtocol.Rows(row.Index).Height + row.DividerHeight)
        Next

        ClientSize = New Size(totalWidth + 5, totalHeight + 5) ' Me.ClientSize.Height) '+ Me.DGVПротокол.RowHeadersWidth он не видим

        For I As Integer = 1 To rows
            For J As Integer = 1 To columns
                DGVProtocol.Rows(I - 1).Cells(J - 1).Value = CType(protocol(I, J), Object)
            Next
        Next
        Me.Text = $"Протокол расшифровки. Нормы ТУ для - Изделие:{ModificationEngine} Режим:{ModeRegime}"
    End Sub

    Private Sub FormProtocol_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        DGVProtocol.Rows(0).Cells(0).Selected = True

        Dim rowValue As String
        For I As Integer = 1 To UBound(protocol) ' Row
            For J As Integer = 1 To UBound(protocol, 2) ' Column
                rowValue = CStr(DGVProtocol.Rows(I - 1).Cells(J - 1).Value)

                If rowValue.Length > 0 Then
                    protocol(I, J) = rowValue
                Else
                    protocol(I, J) = "-"
                End If
            Next
        Next
    End Sub
End Class