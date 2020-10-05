Imports System.Data.OleDb
Imports System.Collections.Generic

''' <summary>
''' Навигатор по фоновым снимкам
''' </summary>
Friend Class FormNavigatorSnapshot

    Public SerialShots As New Dictionary(Of integer, String)
    Public Property ParentFormSnapshot() As FormSnapshotBase

    Private isForwardClick As Boolean
    Private isBackClick As Boolean
    Private isLockSelectedIndexChanged As Boolean
    Private keyID As Integer
    Private listBoxColors As Color() = {Color.Lime, Color.Orange, Color.LightGoldenrodYellow, Color.Yellow, Color.Red}

    Public Sub New(ByVal parentForm As FormSnapshotBase)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        Me.ParentFormSnapshot = parentForm
    End Sub

    Private Sub ListBoxSnapshots_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxSnapshots.DrawItem
        If e.Index = -1 Then Exit Sub
        e.DrawBackground()
        Dim mode As DrawMode = ListBoxSnapshots.DrawMode
        Dim strItems As String = CStr(ListBoxSnapshots.Items(e.Index))

        If mode = DrawMode.OwnerDrawFixed Then
            Dim b As Brush = New SolidBrush(listBoxColors(2))

            If strItems.IndexOf("Фоновая запись - прерванный снимок") > 0 Then
                b = New SolidBrush(listBoxColors(1))
            ElseIf strItems.IndexOf("Фоновая запись  1") = strItems.Length - 17 Then
                'ElseIf strItems.Length = 16 Then
                b = New SolidBrush(listBoxColors(0))
            ElseIf strItems.IndexOf("Объединённая") > 0 Then
                b = New SolidBrush(listBoxColors(4))
            End If

            Dim bSel As Brush = New SolidBrush(Color.Blue)
            e.Graphics.FillRectangle(b, e.Bounds)
            e.Graphics.DrawRectangle(SystemPens.WindowText, e.Bounds)

            If ((e.State And DrawItemState.Selected) = DrawItemState.Selected) Then
                e.Graphics.FillRectangle(bSel, e.Bounds)
                e.Graphics.DrawRectangle(SystemPens.WindowText, e.Bounds)
                e.Graphics.DrawString($"{ListBoxSnapshots.Items(e.Index)}  загружен", Font, New SolidBrush(Color.White), e.Bounds.X + 1, e.Bounds.Y + 1)
            Else
                e.Graphics.DrawString(ListBoxSnapshots.Items(e.Index).ToString & " ", Font, New SolidBrush(Color.Black), e.Bounds.X + 1, e.Bounds.Y + 1)
            End If

            e.DrawFocusRectangle()
        End If
    End Sub

    Private Sub FormNavigatorSnapshot_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ParentFormSnapshot.IsLoadFormNavigatorSnapshot = True
    End Sub

    Private Sub FormNavigatorSnapshot_Closed(ByVal eventSender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        ParentFormSnapshot.IsLoadFormNavigatorSnapshot = False
        ParentFormSnapshot = Nothing
        CollectionForms.Remove(Text)
    End Sub

    Private Sub ListBoxSnapshots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBoxSnapshots.SelectedIndexChanged
        If Not (isBackClick OrElse isForwardClick OrElse isLockSelectedIndexChanged) Then
            ButtonForward.Enabled = True
            ButtonBack.Enabled = True
            LoadFrameBackgroundSnapshot()
        End If
    End Sub

    Private Sub ButtonBack_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonBack.Click
        Dim selectedIndex As Integer = ListBoxSnapshots.SelectedIndex - 1

        isBackClick = True

        If selectedIndex < 0 Then
            MessageBox.Show("Конец списка!", "Навигация по списку", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ButtonBack.Enabled = False
        Else
            ListBoxSnapshots.SelectedIndex = selectedIndex
            ButtonBack.Enabled = True
            LoadFrameBackgroundSnapshot()
        End If

        ButtonForward.Enabled = True
        isBackClick = False
    End Sub

    Private Sub ButtonForward_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonForward.Click
        Dim selectedIndex As Integer = ListBoxSnapshots.SelectedIndex + 1

        isForwardClick = True

        If selectedIndex > ListBoxSnapshots.Items.Count - 1 Then
            MessageBox.Show("Конец списка!", "Навигация по списку", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ButtonForward.Enabled = False
        Else
            ListBoxSnapshots.SelectedIndex = selectedIndex
            ButtonForward.Enabled = True
            LoadFrameBackgroundSnapshot()
        End If

        ButtonBack.Enabled = True
        isForwardClick = False
    End Sub

    ''' <summary>
    ''' Загрузить Фоновые Снимки
    ''' </summary>
    ''' <param name="inNumberProductionSnapshot"></param>
    ''' <param name="onlyBackgroundSnapshot"></param>
    Public Sub LoadBackgroundSnapshot(inNumberProductionSnapshot As Integer, onlyBackgroundSnapshot As Boolean)
        ' после загрузки из проводника любого снимка определяется номер двигателя
        ' создается запрос с шаблоном "Фоновая запись" 
        Dim keyId As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim strSQL As String = "Select БазаСнимков.KeyID, БазаСнимков.НомерИзделия, БазаСнимков.Дата, БазаСнимков.ВремяНачалаСбора, БазаСнимков.Примечание From БазаСнимков Where БазаСнимков.НомерИзделия = " & inNumberProductionSnapshot
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        ListBoxSnapshots.BeginUpdate()
        ListBoxSnapshots.Items.Clear()
        isLockSelectedIndexChanged = True

        If onlyBackgroundSnapshot Then
            strSQL &= " And БазаСнимков.Примечание Like '%запись%' ORDER BY БазаСнимков.KeyID"
            Text = "Фоновая запись изделия №" & Str(inNumberProductionSnapshot)
        Else
            strSQL &= " And БазаСнимков.Режим <> 'Регистратор' ORDER BY БазаСнимков.KeyID"
            Text = "Переменные режимы изделия №" & Str(inNumberProductionSnapshot)
        End If

        CollectionForms.Add(Text, Me) ' добавляем после определения заголовка окна
        cn.Open()
        cmd.CommandText = strSQL
        ' Создание читателя
        rdr = cmd.ExecuteReader

        Dim I As Integer = 1
        ' затем добавим по порядку
        Do While rdr.Read
            keyId = CInt(rdr("KeyID"))
            SerialShots.Add(keyId, $"{I} - Дата:{Convert.ToDateTime(rdr("Дата")).ToShortDateString} Время:{Convert.ToDateTime(rdr("ВремяНачалаСбора")).ToShortTimeString} {rdr("Примечание")}")
            ListBoxSnapshots.Items.Add(SerialShots.Item(keyId))
            I += 1
        Loop

        rdr.Close()
        cn.Close()

        For Each key As Integer In SerialShots.Keys
            If key = GKeyID Then
                ListBoxSnapshots.SelectedItem = SerialShots.Item(key)
                Exit For
            End If
        Next

        ListBoxSnapshots.EndUpdate()
        isLockSelectedIndexChanged = False
    End Sub

    ''' <summary>
    ''' Загрузить ГрафикФонового Снимка
    ''' </summary>
    Private Sub LoadFrameBackgroundSnapshot()
        For Each key As Integer In SerialShots.Keys
            If SerialShots.Item(key).Equals(ListBoxSnapshots.SelectedItem) Then
                keyID = key
                Exit For
            End If
        Next

        ParentFormSnapshot.ShowFrameSnapshotFromDBase(keyID)
    End Sub

    ''' <summary>
    ''' Обновить Индекс Фонового Снимка
    ''' </summary>
    ''' <param name="lngKeyID"></param>
    Public Sub UpdateIndexBackgroundSnapshot(ByVal lngKeyID As Integer)
        For Each key As Integer In SerialShots.Keys
            If SerialShots.Item(key).Equals(ListBoxSnapshots.SelectedItem) Then
                SerialShots.Add(lngKeyID, ListBoxSnapshots.SelectedItem.ToString)
                SerialShots.Remove(key)
                Exit For
            End If
        Next
    End Sub
End Class