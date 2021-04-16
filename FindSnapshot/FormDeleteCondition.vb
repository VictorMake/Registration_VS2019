Imports MathematicalLibrary

Public Class FormDeleteCondition
    Private IsConditionsToDelete As Boolean()

    Public Function GetConditionsToDelete() As Boolean()
        Return IsConditionsToDelete
    End Function

    Private arrCheckBox() As CheckBox
    Private countConditions As Integer

    Public Sub New(ByVal inCountConditions As Integer)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавbnm все инициализирующие действия после вызова InitializeComponent().
        Dim pointLocation As New Point(8, 24)
        Re.Dim(arrCheckBox, inCountConditions - 1)
        Re.Dim(IsConditionsToDelete, inCountConditions - 1)

        countConditions = inCountConditions

        For I As Integer = 1 To inCountConditions
            Dim CheckBox As CheckBox = New CheckBox() With {
                .Name = "CheckBox" & I.ToString,
                .Text = "Условие №" & I.ToString,
                .Location = New Point(pointLocation.X, pointLocation.Y),
                .Size = New Size(102, 24)
            }

            pointLocation.Y += CheckBox.Height + 1
            Panel2.Controls.Add(CheckBox)
            arrCheckBox(I - 1) = CheckBox
        Next

        ClientSize = New Size(230, pointLocation.Y - 24 + Panel2.Height)
    End Sub

    Private Sub CheckDelete()
        Dim count As Integer

        For I As Integer = 0 To countConditions - 1
            If arrCheckBox(I).Checked Then
                IsConditionsToDelete(I) = arrCheckBox(I).Checked
                count += 1
            End If
        Next

        If count = countConditions Then
            ' оставить хотя бы одно
            IsConditionsToDelete(0) = False
        End If
    End Sub

    Private Sub ButtonApply_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonApply.Click
        CheckDelete()
        Close()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCancel.Click
        Close()
    End Sub
End Class