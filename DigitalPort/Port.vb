Imports System.Collections.Generic
Imports System.ComponentModel
Imports NationalInstruments.UI.WindowsForms

Friend Class Port
    Implements IEnumerable

    Private ReadOnly columnWidth As Single
    Private LedLedPortLine As Dictionary(Of String, Led)
    Private WithEvents TableLayoutPanelPort As TableLayoutPanel
    Private WithEvents LabelPort As Label
    Private WithEvents PanelLabelNumeric As Panel
    Private WithEvents LabelNamePort As Label
    Private WithEvents NumericEditPort As NumericEdit

    Public Sub New(ByVal inDeviceName As String, ByVal inLineCount As Integer)
        DeviceName = inDeviceName
        LineCount = inLineCount
        LedLedPortLine = New Dictionary(Of String, Led)

        TableLayoutPanelPort = New TableLayoutPanel
        LabelPort = New Label
        PanelLabelNumeric = New Panel
        LabelNamePort = New Label
        NumericEditPort = New NumericEdit

        TableLayoutPanelPort.SuspendLayout()
        TableLayoutPanelPort.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble

        If LineCount >= 31 Then
            TableLayoutPanelPort.ColumnCount = LineCount + 3
            columnWidth = 3.125!
        ElseIf LineCount = 7 Then
            TableLayoutPanelPort.ColumnCount = 10
            columnWidth = 12.5!
        End If

        CType(NumericEditPort, ISupportInitialize).BeginInit()
        PanelLabelNumeric.SuspendLayout()

        '--- LabelPort
        LabelPort.AutoSize = True
        LabelPort.Dock = DockStyle.Fill
        LabelPort.Margin = New Padding(0)
        LabelPort.Name = "LabelPort" & DeviceName
        LabelPort.Text = DeviceName
        LabelPort.TextAlign = ContentAlignment.MiddleCenter

        '--- PanelLabelNumeric
        PanelLabelNumeric.Controls.Add(LabelNamePort)
        PanelLabelNumeric.Controls.Add(NumericEditPort)
        PanelLabelNumeric.Dock = DockStyle.Fill
        PanelLabelNumeric.Name = "Panel" & DeviceName

        '--- LabelNamePort
        LabelNamePort.BorderStyle = BorderStyle.Fixed3D
        LabelNamePort.Dock = DockStyle.Fill
        LabelNamePort.Location = New Point(0, 0)
        LabelNamePort.Name = "LabelNamePort" & DeviceName
        LabelNamePort.Text = "Десятичный код" '"мощность" '"P" & mНомерПорта.ToString
        LabelNamePort.Size = New Size(39, 102)

        '--- NumericEditPort
        NumericEditPort.BackColor = Color.Black
        NumericEditPort.Dock = DockStyle.Bottom
        NumericEditPort.ForeColor = Color.Lime
        'NumericEditPort.InteractionMode = NationalInstruments.UI.NumericEditInteractionModes.Text
        NumericEditPort.InteractionMode = NumericEditInteractionModes.Indicator
        NumericEditPort.FormatMode = NumericFormatMode.CreateSimpleDoubleMode(0)
        NumericEditPort.TextAlign = HorizontalAlignment.Right
        NumericEditPort.Name = "NumericEditPort" & DeviceName

        PanelLabelNumeric.ResumeLayout(False)
        CType(NumericEditPort, ISupportInitialize).EndInit()

        'TableLayoutPanelPort.Controls.Add(LabelPort, column, row)
        TableLayoutPanelPort.Controls.Add(LabelPort, LineCount + 2, 0)
        TableLayoutPanelPort.Controls.Add(PanelLabelNumeric, LineCount + 1, 0)

        TableLayoutPanelPort.Dock = DockStyle.Fill
        TableLayoutPanelPort.GrowStyle = TableLayoutPanelGrowStyle.FixedSize

        TableLayoutPanelPort.Margin = New Padding(0)
        TableLayoutPanelPort.Name = "TableLayoutPanelPort" & DeviceName
        TableLayoutPanelPort.RowCount = 1
        TableLayoutPanelPort.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))

        AddPorLine()
    End Sub

    Private Sub AddPorLine()
        For I As Integer = 0 To LineCount
            LoadLedLedPorLine(I)
        Next

        ' коллекция стилей добавляется 2 раза для 2 последних столбцов
        TableLayoutPanelPort.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 45.0!))
        TableLayoutPanelPort.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 45.0!))
        TableLayoutPanelPort.ResumeLayout(False)
        TableLayoutPanelPort.PerformLayout()
    End Sub

    Private Sub LoadLedLedPorLine(ByVal indexLine As Integer)
        Dim Led As New Led

        CType(Led, ISupportInitialize).BeginInit()
        TableLayoutPanelPort.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, columnWidth))

        Led.Caption = indexLine.ToString
        Led.Dock = DockStyle.Fill
        Led.LedStyle = LedStyle.Square3D
        Led.Margin = New Padding(0)

        Led.InteractionMode = BooleanInteractionMode.Indicator '.SwitchWhenPressed
        Led.Name = "LedPortLine" & indexLine.ToString
        Led.Tag = $"{DeviceName}/line{indexLine}" '"SC" & НомерУстройства & "Mod<slot#>/port0/lineN" 'Dev0/port1/line0:2
        Led.OffColor = Color.Silver
        Led.OnColor = Color.Red

        TableLayoutPanelPort.Controls.Add(Led, LineCount - indexLine, 0)
        CType(Led, ISupportInitialize).EndInit()

        'AddHandler Led.StateChanged, AddressOf Led_StateChanged

        ' добавить в коллекцию
        LedLedPortLine.Add(indexLine.ToString, Led)
    End Sub

    Public ReadOnly Property AllLedPortLine() As Dictionary(Of String, Led)
        Get
            Return LedLedPortLine
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return LedLedPortLine.GetEnumerator
    End Function

    'Public Sub Remove(ByRef vntIndexKey As Single)
    '    'удаление по номеру или имени или объекту?
    '    'если целый тип то по плавающему индексу, а если строковый то по ключу
    '    ВеличиныЗагрузок.Remove(vntIndexKey)
    'End Sub

    'Public Sub Clear()
    '    ''здесь удаление по ключу, а он строковый
    '    'не работает
    '    'Dim oneInst As Условие
    '    'For Each oneInst In mCol
    '    '    mCol.Remove(oneInst.ID.ToString)
    '    'Next
    '    'Dim I As Integer
    '    'With mCol
    '    '    For I = .Count To 1 Step -1
    '    '        .Remove(I)
    '    '    Next
    '    'End With
    '    ВеличиныЗагрузок.Clear()
    'End Sub

    'Public ReadOnly Property ToArray() As Object()
    '    Get
    '        Dim arrayObject(ВеличиныЗагрузок.Count - 1) As Object
    '        Dim I As Integer
    '        For Each tempИмяСобытия As ИмяСобытия In ВеличиныЗагрузок.Values
    '            arrayObject(I) = tempИмяСобытия
    '            I += 1
    '        Next
    '        Return arrayObject
    '    End Get
    'End Property

    'Protected Overrides Sub Finalize()
    '    ВеличиныЗагрузок = Nothing
    '    MyBase.Finalize()
    'End Sub

    Public Property DeviceName() As String

    Public Property LineCount() As Integer

    Shadows ReadOnly Property ToString() As String 'Protected
        Get
            Return DeviceName
        End Get
    End Property

    Shadows ReadOnly Property TableLayoutPanel() As TableLayoutPanel
        Get
            Return TableLayoutPanelPort
        End Get
    End Property
End Class