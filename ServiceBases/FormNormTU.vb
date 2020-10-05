Imports System.Data.OleDb

Friend Class FormNormTU
    Public Sub New(inMdiParent As Form)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        Me.MdiParent = inMdiParent
    End Sub

    Private Sub FormChannel_Load(sender As Object, e As EventArgs) Handles Me.Load
        DataGridChannels.AutoGenerateColumns = False
        LoadTableChannels()
    End Sub

    Private Sub FormChannel_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Close()
    End Sub

    ''' <summary>
    ''' Загрузить Таблицу Данными
    ''' </summary>
    Private Sub LoadTableChannels()
        NormTuTableAdapter.Connection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        ' сделать типизированный TableAdapter, который заполняется по номеру стенда
        ' и имеет поиск канала по имени
        ' данная строка кода позволяет загрузить данные в таблицу "NormTuDataSet.ТехническиеУсловия". При необходимости она может быть перемещена или удалена.
        NormTuTableAdapter.Fill(NormTuDataSet.ТехническиеУсловия)
    End Sub
End Class