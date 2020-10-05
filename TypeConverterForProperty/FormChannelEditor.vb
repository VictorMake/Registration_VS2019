Public Class FormChannelEditor
    Private imagesCross As Integer

    Public Sub New(ByVal mChannel As String)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        imagesCross = ImageListChannel.Images.Count - 1 ' крест
        PopulateComboBox(mChannel)
        Channel = mChannel
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
    End Sub

    ''' <summary>
    ''' Имя канала выбранное в списке
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Channel() As String
        Get
            Return ComboBoxParameters.Text
        End Get
        Set(ByVal value As String)
            ComboBoxParameters.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Из предварительно считанного из базы в массив всех установок параметров
    ''' считывание и занесение в cmbПараметры.
    ''' </summary>
    ''' <param name="mChannel"></param>
    Private Sub PopulateComboBox(ByVal mChannel As String)
        If EditorPanelMotoristForm IsNot Nothing Then
            If IndexParametersForControl IsNot Nothing Then 'If РедакторПанелейМоториста.РодительскаяФорма IsNot Nothing Then
                AddItems()
            Else
                ComboBoxParameters.Items.Add(New StringIntObject(mChannel, imagesCross))
            End If
        End If

        If DigitalPortForm IsNot Nothing Then
            If DigitalPortForm.FormMainReference IsNot Nothing Then
                AddItems()
            Else
                ComboBoxParameters.Items.Add(New StringIntObject(mChannel, imagesCross))
            End If
        End If
    End Sub

    Private Sub AddItems()
        ComboBoxParameters.Items.Clear()

        ComboBoxParameters.Items.Add(New StringIntObject(MissingParameter, imagesCross))
        For I As Integer = 1 To UBound(IndexParametersForControl)
            If CBool(InStr(1, UnitOfMeasureString, ParametersType(IndexParametersForControl(I)).UnitOfMeasure)) Then
                ComboBoxParameters.Items.Add(New StringIntObject(ParametersType(IndexParametersForControl(I)).NameParameter, Array.IndexOf(UnitOfMeasureArray, ParametersType(IndexParametersForControl(I)).UnitOfMeasure)))
            Else
                ComboBoxParameters.Items.Add(New StringIntObject(ParametersType(IndexParametersForControl(I)).NameParameter, 1))
            End If
        Next
    End Sub

#Region "DrawItem"
    Private Sub ComboBoxParameters_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ComboBoxParameters.DrawItem
        DrawItemComboBox(sender, e, Nothing, ImageListChannel, False)
    End Sub
    Private Sub ComboBoxParameters_MeasureItem(sender As Object, e As MeasureItemEventArgs) Handles ComboBoxParameters.MeasureItem
        Dim cb As ComboBox = CType(sender, ComboBox)
        e.ItemHeight = cb.ItemHeight - 2
    End Sub

    Private Sub ButtonFindChannel_Click(sender As Object, e As EventArgs) Handles ButtonFindChannel.Click
        Dim mSearchChannel As New SearchChannel(ComboBoxParameters)
        mSearchChannel.SelectChannel()
    End Sub

    'Private Sub cmbПараметры_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles cmbПараметры.DrawItem
    '    If e.Index = -1 Then Return
    '    If sender Is Nothing Then Return

    '    Dim cmb As ComboBox = CType(sender, ComboBox)
    '    Dim g As Graphics = e.Graphics

    '    ' Если элемент выделен, он будет перерисован в корректный background color
    '    e.DrawBackground()
    '    e.DrawFocusRectangle()
    '    Dim rectPreviewBox As Rectangle = e.Bounds

    '    ' Рисовать цветом квадрат обрамления
    '    ' при данном шрифте Font("Arial", 22) высота rectPreviewBox.Height=36
    '    'ImageList2 размером 32, значит надо квадрат 32*32
    '    rectPreviewBox.Offset(2, 2)
    '    rectPreviewBox.Height -= 4
    '    rectPreviewBox.Width = rectPreviewBox.Height 'rectPreviewBox.Height - 4 '32 '20
    '    g.DrawRectangle(New Pen(e.ForeColor), rectPreviewBox)

    '    'g.DrawImage(ImageList2.Images(0), rectPreviewBox)

    '    ' Получить подходящую кисть Brush object для выделенного цвета
    '    ' и залить квадрат обрамления.
    '    rectPreviewBox.Offset(1, 1)
    '    rectPreviewBox.Width -= 2
    '    rectPreviewBox.Height -= 2
    '    'g.FillRectangle(selectedBrush, rectPreviewBox)
    '    'g.DrawImage(ImageList2.Images(die.Index), rectPreviewBox)

    '    'If Array.IndexOf(arrИндексыНенайденныхПараметров, die.Index) = -1 Then 'не найден, значит иконка по настройке
    '    g.DrawImage(ImageListChannel.Images(CType(cmb.Items(e.Index), StringIntObject).ImageIndex), rectPreviewBox)
    '    'Else ' индекс найден, иконка с крестиком
    '    '    g.DrawImage(ImageList2.Images(ImagesCross), rectPreviewBox)
    '    'End If

    '    ' Рисовать имя выделенным цветом
    '    'Dim drawFont As New Font("Arial", 16, FontStyle.Bold)
    '    'drawFont. = True
    '    Dim drawFont As Font = New System.Drawing.Font(cmb.Font,
    '       cmb.Font.Style) ' Or FontStyle.Bold)

    '    'g.DrawString(cmb.Items(die.Index).ToString, New Font("Arial", ImagesCross, FontStyle.Bold), New SolidBrush(die.ForeColor), die.Bounds.X + 30, die.Bounds.Y + 1)
    '    g.DrawString(cmb.Items(e.Index).ToString, drawFont, New SolidBrush(e.ForeColor), e.Bounds.X + 30, e.Bounds.Y + 1)
    '    drawFont = Nothing
    'End Sub

    '''' <summary>
    '''' Вспомогательный клас для заполнения списка
    '''' </summary>
    '''' <remarks></remarks>
    'Friend Class StringIntObject
    '    Private mNameChannel As String
    '    Private mImageIndex As Integer

    '    Public Sub New(ByVal nameChannel As String, ByVal imageIndex As Integer)
    '        mNameChannel = nameChannel
    '        mImageIndex = imageIndex
    '    End Sub

    '    Public Property ImageIndex As Integer
    '        Get
    '            Return mImageIndex
    '        End Get
    '        Set(ByVal value As Integer)
    '            mImageIndex = value
    '        End Set
    '    End Property

    '    Public Property NameChannel As String
    '        Get
    '            Return mNameChannel
    '        End Get
    '        Set(ByVal value As String)
    '            mNameChannel = value
    '        End Set
    '    End Property

    '    Public Overrides Function ToString() As String
    '        Return mNameChannel
    '    End Function
    'End Class

#End Region

End Class