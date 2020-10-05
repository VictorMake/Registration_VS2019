Friend Class FormTuningChannel
    Public Sub New(ByVal inParentForm As FormServiceBases)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        ParentFormServiceBases = inParentForm
    End Sub

    Private mParentForm As FormServiceBases
    Public WriteOnly Property ParentFormServiceBases() As FormServiceBases
        Set(ByVal Value As FormServiceBases)
            mParentForm = Value
        End Set
    End Property

    ''' <summary>
    ''' Добавляется Плата
    ''' </summary>
    ''' <returns></returns>
    Public Property IsAddingBoard() As Boolean

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonOK.Click
        Close()
    End Sub

    Private Sub ListBoxUnit_DrawItem(ByVal sender As Object, ByVal die As DrawItemEventArgs) Handles ListBoxUnit.DrawItem
        DrawItemListBox(sender, die, mParentForm.ImageListTree)
    End Sub

    Private Sub ListBoxUnit_MeasureItem(ByVal sender As Object, ByVal mie As MeasureItemEventArgs) Handles ListBoxUnit.MeasureItem
        'Dim lstBox As ListBox = CType(sender, ListBox)
        'mie.ItemHeight = lstBox.ItemHeight - 2
        mie.ItemHeight = ListBoxUnit.ItemHeight - 2
    End Sub

    Private Sub FormTuningChannel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)

        For I As Integer = 0 To UBound(UnitOfMeasureArray)
            ListBoxUnit.Items.Add(New StringIntObject(UnitOfMeasureArray(I), I))
        Next

        ListBoxUnit.SelectedIndex = 0
        ComboBoxLowLimit.SelectedIndex = 10
        ComboBoxUpperLimit.SelectedIndex = 20
        ListBoxFormula.SelectedIndex = 1
        ComboBoxCountOfChannels.SelectedIndex = 3

        If IsAddingBoard Then
            Label1.Visible = True
            Label2.Visible = True
            ComboBoxCountOfChannels.Visible = True
        End If
    End Sub

    Private Sub ListBoxUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxUnit.SelectedIndexChanged
        PictureBoxControl.Image = mParentForm.GetImageFromUnit(ListBoxUnit.SelectedItem.ToString)
    End Sub
End Class