Imports System.Data.OleDb
Imports System.Threading
Imports MathematicalLibrary

Friend Class FormAxesAdvanced
    Private connection As OleDbConnection
    Private countAxes As Integer
    Private countPlots As Integer

    Const cMaxAxes As Integer = 6 ' ограничение добавляемых осей
    Private constShade As Color = Color.DarkGray
    Private constFace As Color = SystemColors.Control

    Private FrequencyGraph() As Single
    Private isNeedToSave As Boolean ' надо Записать
    Private isProcessDeletePlot As Boolean ' идет Удаление Шлейфа
    Private isNeedToUpdateCaption As Boolean ' надо Обновлять Надписи

#Region "Form"
    Sub New(ByVal formParent As FormMain)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.ParentForm = formParent
        mParentForm = formParent
    End Sub

    Private mParentForm As FormMain
    'Private WriteOnly Property ParentForm() As FormMain
    '    Set(ByVal Value As FormMain)
    '        mParentForm = Value
    '    End Set
    'End Property

    Private Sub FormAxesAdvanced_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{NameOf(FormAxesAdvanced_Load)}> Загрузка окна настройки осей")
        InitializeForm()
        mParentForm.ButtonTuneTrand.Enabled = False
    End Sub

    Private Sub FormAxesAdvanced_Closed(ByVal eventSender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS($"<{NameOf(FormAxesAdvanced_Closed)}> Закрытие окна настройки осей")

        If isNeedToSave Then SaveConfiguration()

        mParentForm.ButtonTuneTrand.Enabled = True
        mParentForm.ButtonTuneTrand.Checked = False
        mParentForm = Nothing
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonClose.Click
        Close()
    End Sub

    Public Sub InitializeForm()
        Dim index, I As Integer

        countAxes = mParentForm.ScatterGraphParameter.YAxes.Count
        SlideSelectAxis.Value = 0
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        LabelGrafColorNext.BackColor = ColorsNet(0)

        'For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In mФормаРодителя.FormParent.Manager.РасчетныеПараметры.Rows
        '    cmbПараметры.Items.Add(rowРасчетныйПараметр.ИмяПараметра)
        '    cmbПараметрыОси.Items.Add(rowРасчетныйПараметр.ИмяПараметра)
        'Next
        'For Each rowИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow In mФормаРодителя.FormParent.Manager.ИзмеренныеПараметры.Rows
        '    cmbПараметры.Items.Add(rowИзмеренныйПараметр.ИмяПараметра)
        '    cmbПараметрыОси.Items.Add(rowИзмеренныйПараметр.ИмяПараметра)
        'Next

        'cmbПараметрыОси.SelectedIndex = 0
        'cmbПараметры.SelectedIndex = 0

        For I = 1 To mParentForm.AllGraphParametersByParameter.GetLength(0) - 1
            ' для оси
            Dim nodeParameter As New TreeNode(mParentForm.AllGraphParametersByParameter(I).NameParameter) With {
                .Tag = mParentForm.AllGraphParametersByParameter(I).NameParameter
            }
            index = Array.IndexOf(UnitOfMeasureArray, mParentForm.AllGraphParametersByParameter(I).UnitOfMeasure)
            If index = -1 Then index = 6
            nodeParameter.SelectedImageIndex = 14
            nodeParameter.ImageIndex = index
            TreeViewParametersAxis.Nodes.Item(index).Nodes.Add(nodeParameter)
            TreeViewParametersAxis.Nodes.Item(index).Tag = "Root"
            ' для параметров
            TreeViewParameters.Nodes.Item(index).Nodes.Add(CType(nodeParameter.Clone, TreeNode))
            TreeViewParameters.Nodes.Item(index).Tag = "Root"
        Next

        SetFrequencyGraph(FrequencyBackground)

        For I = 0 To UBound(FrequencyGraph)
            ComboBoxFrequency.Items.Add(CStr(FrequencyGraph(I)))
        Next

        ComboBoxTimeTail.Items.Add("10")
        ComboBoxTimeTail.Items.Add("20")
        ComboBoxTimeTail.Items.Add("30")
        ComboBoxTimeTail.Items.Add("60")

        ComboBoxFrequency.SelectedIndex = 0
        ComboBoxTimeTail.SelectedIndex = 0

        ' здесь восстанавливаются все настройки, если такие параметры есть
        LoadConfigurationFromDBase()
        RestoreConfiguration($"keyКонфигурацияОтображения)= {mParentForm.KeyConfiguration}))")
    End Sub

    ''' <summary>
    ''' Загрузить Конфигурации Графиков
    ''' </summary>
    Private Sub LoadConfigurationFromDBase()
        Dim indexRow, lastSelectedIndex As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim strSQL As String = "SELECT DISTINCTROW КонфигурацииОтображения.keyКонфигурацияОтображения, КонфигурацииОтображения.ИмяКонфигурации " &
                                "FROM(КонфигурацииОтображения) " &
                                "ORDER BY КонфигурацииОтображения.keyКонфигурацияОтображения;"

        ' Открыть подключение
        cn.Open()
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        odaDataAdapter.Fill(dtDataTable)
        cn.Close()

        If dtDataTable.Rows.Count <> 0 Then
            indexRow = 0

            For Each drDataRow In dtDataTable.Rows
                ComboBoxListConfigurations.Items.Add(drDataRow("ИмяКонфигурации"))

                If CInt(drDataRow("keyКонфигурацияОтображения")) = mParentForm.KeyConfiguration Then lastSelectedIndex = indexRow

                indexRow += 1
            Next
        End If

        ComboBoxListConfigurations.Focus()
        ComboBoxListConfigurations.SelectedIndex = lastSelectedIndex
    End Sub

    ''' <summary>
    ''' Восстановить конфигурацию
    ''' </summary>
    ''' <param name="whereCondition"></param>
    Private Sub RestoreConfiguration(ByVal whereCondition As String)
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader
        Dim cmd As OleDbCommand = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        Dim strSQL As String
        Dim I As Integer
        Dim success As Boolean

        isProcessDeletePlot = True
        SlidePlots.CustomDivisions.Clear()
        mParentForm.ScatterGraphParameter.Annotations.Clear()
        isProcessDeletePlot = False
        mParentForm.ScatterGraphParameter.Plots.Clear()

        ' удалить оси оставить 1 последние
        For I = mParentForm.ScatterGraphParameter.YAxes.Count - 1 To 1 Step -1
            mParentForm.ScatterGraphParameter.YAxes.RemoveAt(I)
        Next

        mParentForm.ScatterGraphParameter.XAxes(0).Caption = ""
        ' по умолчанию
        SlideSelectAxis.Value = 0
        SlideSelectAxis.Range = New Range(0, 1)
        SlideAssignAxis.Range = New Range(0, 1)
        ButtonAddAxis.Enabled = True

        ' на этом этапе все очищено
        ' Открыть подключение
        strSQL = "SELECT КонфигурацииОтображения.* " &
                "FROM КонфигурацииОтображения " &
                "WHERE (((КонфигурацииОтображения." & whereCondition
        cn.Open()
        cmd.CommandText = strSQL
        ' Создание recordset
        rdr = cmd.ExecuteReader

        If rdr.Read Then
            mParentForm.KeyConfiguration = CInt(rdr("keyКонфигурацияОтображения"))
            RadioButtonTypeAxisX.Checked = CBool(rdr("ВремяИлиПараметр"))
            RadioButtonTypeAxisY.Checked = Not RadioButtonTypeAxisX.Checked
            success = False

            For I = 0 To ComboBoxFrequency.Items.Count - 1
                ' проверка на существование
                If ComboBoxFrequency.Items(I).ToString = CStr(rdr("ЧастотаПостроения")) Then
                    success = True
                    Exit For
                End If
            Next

            If success Then
                ComboBoxFrequency.SelectedIndex = I
            Else
                ComboBoxFrequency.SelectedIndex = 0
            End If

            SetCounterLightByFrequencyBackground()

            success = False
            For I = 0 To ComboBoxTimeTail.Items.Count - 1
                ' проверка на существование
                If ComboBoxTimeTail.Items(I).ToString = CStr(rdr("ВремяСвечения")) Then
                    success = True
                    Exit For
                End If
            Next

            If success Then
                ComboBoxTimeTail.SelectedIndex = I
            Else
                ComboBoxTimeTail.SelectedIndex = 0
            End If

            SetCounterLightByUser()
            success = False
            'For I = 0 To cmbПараметрыОси.Items.Count - 1
            '    'проверка на существование
            '    If cmbПараметрыОси.Items(I).ToString = rdr("ИмяПараметраОсиХ") Then
            '        blnНайдено = True
            '        Exit For
            '    End If
            'Next 

            For I = 1 To mParentForm.AllGraphParametersByParameter.GetLength(0) - 1
                If mParentForm.AllGraphParametersByParameter(I).NameParameter = CStr(rdr("ИмяПараметраОсиХ")) Then
                    success = True
                    LabelSelectedParameter.Text = CStr(rdr("ИмяПараметраОсиХ"))
                    Exit For
                End If
            Next

            If success Then
                'cmbПараметрыОси.SelectedIndex = I
                NumericEditParamMin.Value = CDbl(rdr("МинОсь"))
                NumericEditParamMax.Value = CDbl(rdr("МахОсь"))
            Else
                'cmbПараметрыОси.SelectedIndex = 0
                NumericEditParamMin.Value = 0
                NumericEditParamMax.Value = 100
            End If
        End If

        rdr.Close()
        strSQL = "SELECT КонфигурацииОтображения.keyКонфигурацияОтображения, КонфигурацииОтображения.ИмяКонфигурации, Ось.* " &
                "FROM КонфигурацииОтображения RIGHT JOIN Ось ON КонфигурацииОтображения.keyКонфигурацияОтображения = Ось.keyКонфигурацияОтображения " &
                "WHERE (((КонфигурацииОтображения." & whereCondition &
                " ORDER BY Ось.НомерОси;"

        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' затем добавим по порядку
        Do While rdr.Read
            NumericEditAxisYMin.Value = CDbl(rdr("НижнееЗначение"))
            NumericEditAxisYMax.Value = CDbl(rdr("ВерхнееЗначение"))
            SlidePositionTicks.Value = CDbl(rdr("РасположениеМетки"))
            SlidePositionNumeric.Value = CDbl(rdr("РасположениеЧисла"))

            ' по умолчанию 0 ось уже есть и оси в возрастающем порядке и не повторяются
            If CInt(rdr("НомерОси")) > 0 Then mParentForm.ScatterGraphParameter.YAxes.Add(New YAxis())

            SetAttributeAxisY()
        Loop

        rdr.Close()

        isNeedToUpdateCaption = False
        ' теперь с шлейфами, они уже очищены
        strSQL = "SELECT [КонфигурацииОтображения].[keyКонфигурацияОтображения], [КонфигурацииОтображения].[ИмяКонфигурации], ПараметрОтображения.* " &
                "FROM КонфигурацииОтображения RIGHT JOIN ПараметрОтображения ON [КонфигурацииОтображения].[keyКонфигурацияОтображения]=[ПараметрОтображения].[keyКонфигурацияОтображения] " &
                "WHERE ((([КонфигурацииОтображения]." & whereCondition
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        ' добавим по порядку
        Do While rdr.Read
            'Dim index As Integer = cmbПараметры.Items.IndexOf(rdr("ИмяПараметра"))
            'If index <> -1 Then
            '    cmbПараметры.SelectedIndex = index 'выделим имя
            '    ДобавитьШлейфВСлайдер(CInt(rdr("НомерОси")))
            'End If
            For I = 1 To mParentForm.AllGraphParametersByParameter.GetLength(0) - 1
                'проверка на существование
                If mParentForm.AllGraphParametersByParameter(I).NameParameter = CStr(rdr("ИмяПараметра")) Then
                    LabelSelectedParameterGraph.Text = CStr(rdr("ИмяПараметра"))
                    AddPlotToAxis(CInt(rdr("НомерОси")))
                    Exit For
                End If
            Next
        Loop

        rdr.Close()
        cn.Close()
        isNeedToUpdateCaption = True

        UpdateCaptionAxes()
        isNeedToSave = False
    End Sub

    ''' <summary>
    ''' Добавить Шлейф В Слайдер
    ''' </summary>
    ''' <param name="numberAxis"></param>
    Private Sub AddPlotToAxis(ByVal numberAxis As Integer)
        Dim plot As ScatterPlot = New ScatterPlot
        Dim I As Integer
        Dim arrX(), arrY() As Double

        For I = 0 To SlidePlots.CustomDivisions.Count - 1
            If SlidePlots.CustomDivisions(I).Text = LabelSelectedParameterGraph.Text Then
                Const caption As String = "Ошибка добавление графика"
                Const text As String = "Для этого параметра шлейф уже создан!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If
        Next

        ' добавить шлейф в график и получить номер
        mParentForm.ScatterGraphParameter.Plots.Add(plot)
        countPlots = mParentForm.ScatterGraphParameter.Plots.Count
        plot.PointStyle = PointStyle.SolidDiamond

        ' цвет точек как цвет оси
        If mParentForm.ScatterGraphParameter.YAxes.Count = 1 Then numberAxis = 0

        plot.PointColor = mParentForm.ScatterGraphParameter.YAxes(numberAxis).CaptionForeColor
        Dim NewScaleCustomDivision As New ScaleCustomDivision() With {.Text = LabelSelectedParameterGraph.Text}
        SlidePlots.CustomDivisions.Add(NewScaleCustomDivision)
        SetRangeForSlidePlots(countPlots)
        NewScaleCustomDivision.Value = countPlots - 1
        ' цвет линии назначается автоматически
        plot.LineColor = ColorsNet((countPlots - 1) Mod 7)
        Dim maxRange As Double = mParentForm.ScatterGraphParameter.XAxes(0).Range.Maximum
        Dim divider As Double

        If maxRange < 1 Then
            divider = 0.01
        ElseIf maxRange >= 1 AndAlso maxRange < 10 Then
            divider = 0.1
        ElseIf maxRange >= 10 AndAlso maxRange < 100 Then
            divider = 1
        ElseIf maxRange >= 100 AndAlso maxRange < 1000 Then
            divider = 10
        ElseIf maxRange >= 1000 AndAlso maxRange < 10000 Then
            divider = 100
        ElseIf maxRange >= 10000 Then
            divider = 1000
        End If

        Dim size As Integer = CInt(maxRange / divider)
        Re.Dim(arrX, size)
        Re.Dim(arrY, size)

        For I = 0 To size
            arrX(I) = I * divider
        Next

        Dim amplidude As Double = mParentForm.ScatterGraphParameter.YAxes(CInt(numberAxis)).Range.Maximum / 2 * Rnd()

        For I = 0 To size
            arrY(I) = Math.Sin(I / 20.0# * Math.PI * 2) * amplidude + amplidude
        Next

        mParentForm.ScatterGraphParameter.Plots(countPlots - 1).PlotXY(arrX, arrY)
        ButtonRemovePlot.Enabled = True
        ButtonAssignPlotToAxis.Enabled = True
        mParentForm.ScatterGraphParameter.Plots(countPlots - 1).YAxis = mParentForm.ScatterGraphParameter.YAxes(numberAxis)

        LabelGrafColorNext.BackColor = ColorsNet(countPlots Mod 7)
        SlidePlots.Value = countPlots - 1

        UpdateCaptionAxes()
    End Sub

    ''' <summary>
    ''' Назначить Диапазон Слайдера Шлейфов
    ''' </summary>
    ''' <param name="countPlots"></param>
    Private Sub SetRangeForSlidePlots(ByVal countPlots As Integer)
        If countPlots > 1 Then ' больше чем один шлейф
            SlidePlots.Enabled = True
            SlidePlots.Range = New Range(0, countPlots - 1)
        Else ' если один шлейф или меньше диапазон от 0 до 1, иначе ошибка
            SlidePlots.Enabled = False
            SlidePlots.Range = New Range(0, 1)
        End If
    End Sub

    ''' <summary>
    ''' Установить Аттрибуты Оси
    ''' </summary>
    Private Sub SetAttributeAxisY()
        With mParentForm.ScatterGraphParameter.YAxes
            countAxes = .Count
            .Item(countAxes - 1).CaptionForeColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MajorDivisions.TickColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MajorDivisions.LabelForeColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).MinorDivisions.TickColor = ColorsNet((countAxes - 1) Mod 7)
            .Item(countAxes - 1).Tag = CStr(countAxes - 1)
            .Item(countAxes - 1).Mode = AxisMode.Fixed
            SetRangeForSlideAssignAxis(countAxes)
            ButtonRemoveAxis.Enabled = True

            If SlidePositionTicks.Value = Disposition.Left Then
                .Item(countAxes - 1).Position = YAxisPosition.Left
            ElseIf SlidePositionTicks.Value = Disposition.Right Then
                .Item(countAxes - 1).Position = YAxisPosition.Right
            End If

            If SlidePositionNumeric.Value = Disposition.Left Then
                .Item(countAxes - 1).CaptionPosition = YAxisPosition.Left
            ElseIf SlidePositionNumeric.Value = Disposition.Right Then
                .Item(countAxes - 1).CaptionPosition = YAxisPosition.Right
            End If

            .Item(countAxes - 1).Range = New Range(NumericEditAxisYMin.Value, NumericEditAxisYMax.Value)
        End With

        SlideSelectAxis.Value = countAxes - 1
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        ButtonAddAxis.Enabled = countAxes < cMaxAxes
        NumericEditAxisYMin.Value = 0 '4 - (NumAxes * 2)
        NumericEditAxisYMax.Value = 4 + (countAxes * 5) '14 - (NumAxes * 2)
    End Sub

    ''' <summary>
    ''' Назначить Диапазон Слайдеров Осей
    ''' </summary>
    ''' <param name="countAxis"></param>
    Private Sub SetRangeForSlideAssignAxis(ByVal countAxis As Integer)
        If countAxis > 1 Then
            SlideAssignAxis.Enabled = True
            SlideSelectAxis.Range = New Range(0, countAxis - 1)
            SlideAssignAxis.Range = New Range(0, countAxis - 1)
        Else
            SlideSelectAxis.Range = New Range(0, 1)
            SlideAssignAxis.Range = New Range(0, 1)
            SlideAssignAxis.Value = 1
            SlideAssignAxis.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Обновить Надписи Осей
    ''' </summary>
    Private Sub UpdateCaptionAxes()
        If isNeedToUpdateCaption Then
            With mParentForm.ScatterGraphParameter
                For I As Integer = 0 To .YAxes.Count - 1
                    ' просмотр всех графиков принадлежащих данной оси
                    Dim axisCaption As String = String.Empty

                    For J As Integer = 0 To .Plots.Count - 1
                        If .Plots(J).YAxis.Tag.ToString = .YAxes(I).Tag.ToString Then
                            If SlidePlots.CustomDivisions.Count > 0 Then
                                axisCaption &= SlidePlots.CustomDivisions(J).Text & "  "
                            End If
                        End If
                    Next

                    .YAxes(I).Caption = axisCaption
                    '.YAxes(I).CaptionForeColor = .YAxes(I).CaptionForeColor
                Next
            End With
        End If
    End Sub

    ''' <summary>
    ''' Установить Счетчик Свечения Графика по фоновой частоте
    ''' </summary>
    Private Sub SetCounterLightByFrequencyBackground()
        CounterLightParametersGraph = CInt(FrequencyBackground / CSng(ComboBoxFrequency.Text))
    End Sub

    ''' <summary>
    ''' Установить Счетчик Свечения Графика по выбору пользователя
    ''' </summary>
    Private Sub SetCounterLightByUser()
        CounterLightParametersGraph = 10

        Try
            CounterLightParametersGraph = CInt(CSng(ComboBoxFrequency.Text) * CInt(ComboBoxTimeTail.Text))
        Catch ex As Exception
            Dim text As String = ex.ToString
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(SetCounterLightByUser)}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Частоты Построенния Графика
    ''' </summary>
    ''' <param name="inFrequencyBackground"></param>
    Private Sub SetFrequencyGraph(ByVal inFrequencyBackground As Integer)
        Select Case inFrequencyBackground
            Case 1
                Re.Dim(FrequencyGraph, 1)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                Exit Select
            Case 2
                Re.Dim(FrequencyGraph, 2)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                Exit Select
            Case 5
                Re.Dim(FrequencyGraph, 3)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2.5
                FrequencyGraph(3) = 5
                Exit Select
            Case 10
                Re.Dim(FrequencyGraph, 4)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 5
                FrequencyGraph(4) = 10
                Exit Select
            Case 20
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
                Exit Select
            Case 50
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
                Exit Select
            Case 100
                Re.Dim(FrequencyGraph, 5)
                FrequencyGraph(0) = 0.5
                FrequencyGraph(1) = 1
                FrequencyGraph(2) = 2
                FrequencyGraph(3) = 4
                FrequencyGraph(4) = 10
                FrequencyGraph(5) = 20
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Обработчик события добавления строки.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As OleDbRowUpdatedEventArgs)
        ' команда получения нового идентификатора для новой добавленной строки
        'Dim newID As Integer = 0
        Dim idCMD As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", connection)

        If args.StatementType = StatementType.Insert Then
            ' записать этот полученный идентификатор в только что добавленную строку
            mParentForm.KeyConfiguration = CInt(idCMD.ExecuteScalar())
            args.Row("keyКонфигурацияОтображения") = mParentForm.KeyConfiguration
        End If
    End Sub

    ''' <summary>
    ''' Записать Список В Базу
    ''' </summary>
    Private Sub SaveConfiguration()
        Dim I, position As Integer
        Dim strSQL As String
        Dim cn As OleDbConnection
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim isConfigurationContain As Boolean
        Dim nameConfiguration As String = ComboBoxListConfigurations.Text

        ' если имя пусто, то вопрос: "введите имя" и так как модально нельзя, то выход из данной процедуры
        If nameConfiguration <> "" Then
            ' проверка число осей должно быть больше 1, число параметров больше 0
            If SlidePlots.CustomDivisions.Count = 0 Then
                Const caption As String = "Запись конфигурации"
                Dim text As String = "Отсутствуют шлейфы параметров для прикрепленния к осям!" & vbCrLf & "Изменения не будут записаны."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Exit Sub
            End If

            ' если имя новое, то новая перекладка добавляется
            ' если переписать, то удаление из базы старых и запись новых
            ' если добавить, то запись в базу, очистка comСписки и считывание в лист заново
            For I = 0 To ComboBoxListConfigurations.Items.Count - 1
                If ComboBoxListConfigurations.Items(I).ToString = nameConfiguration Then
                    isConfigurationContain = True
                    Exit For
                End If
            Next

            ' Открыть подключение
            cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            If isConfigurationContain Then
                strSQL = "SELECT КонфигурацииОтображения.* " &
                        "FROM КонфигурацииОтображения " &
                        "WHERE ИмяКонфигурации = '" & nameConfiguration & "'"
            Else
                strSQL = "SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения"
            End If

            ' Создание recordset
            cn.Open()
            connection = cn ' для события вставки новой строки
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)

            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text

            If isConfigurationContain Then ' удаляется старая группа
                If dtDataTable.Rows.Count > 0 Then dtDataTable.Rows(0).Delete()
            Else ' добавить в комбо и в массив
                ComboBoxListConfigurations.Items.Add(nameConfiguration)
            End If

            ' добавляется новая конфигурация или перезаписывается старая
            drDataRow = dtDataTable.NewRow
            drDataRow.BeginEdit()
            drDataRow("ИмяКонфигурации") = nameConfiguration 'имя
            drDataRow("ЧастотаПостроения") = CSng(ComboBoxFrequency.Text)
            drDataRow("ВремяИлиПараметр") = RadioButtonTypeAxisX.Checked
            drDataRow("ИмяПараметраОсиХ") = LabelSelectedParameter.Text 'cmbПараметрыОси.Text
            drDataRow("ВремяСвечения") = CInt(ComboBoxTimeTail.Text)
            If NumericEditParamMax.Value <= NumericEditParamMin.Value Then NumericEditParamMax.Value = NumericEditParamMin.Value + 1
            drDataRow("МинОсь") = NumericEditParamMin.Value
            drDataRow("МахОсь") = NumericEditParamMax.Value
            drDataRow.EndEdit()
            dtDataTable.Rows.Add(drDataRow)

            ' подключить обработчик события генерации автоинкремента для добавляемой строки
            AddHandler odaDataAdapter.RowUpdated, New OleDbRowUpdatedEventHandler(AddressOf OnRowUpdated)
            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)

            'Dim rdr As OleDbDataReader
            'strSQL = "SELECT keyКонфигурацияОтображения FROM КонфигурацииОтображения WHERE ИмяКонфигурации = '" & strИмяСписки & "'"
            'cmd.CommandText = strSQL
            'rdr = cmd.ExecuteReader
            'If rdr.Read() = True Then
            '    lngkeyКонфигурация = rdr("keyКонфигурацияОтображения")
            'End If
            'rdr.Close()

            ' очистить
            connection = Nothing

            ' запись осей
            strSQL = "SELECT Ось.* FROM Ось WHERE keyКонфигурацияОтображения = " & mParentForm.KeyConfiguration
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)

            For I = 0 To mParentForm.ScatterGraphParameter.YAxes.Count - 1
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyКонфигурацияОтображения") = mParentForm.KeyConfiguration
                drDataRow("НомерОси") = I
                drDataRow("НижнееЗначение") = mParentForm.ScatterGraphParameter.YAxes(I).Range.Minimum
                drDataRow("ВерхнееЗначение") = mParentForm.ScatterGraphParameter.YAxes(I).Range.Maximum
                drDataRow("НомерЦвета") = I Mod 7

                If mParentForm.ScatterGraphParameter.YAxes(I).Position = YAxisPosition.Left Then
                    position = Disposition.Left
                ElseIf mParentForm.ScatterGraphParameter.YAxes(I).Position = YAxisPosition.Right Then
                    position = Disposition.Right
                Else
                    position = Disposition.None
                End If

                drDataRow("РасположениеМетки") = position

                If mParentForm.ScatterGraphParameter.YAxes(I).CaptionPosition = YAxisPosition.Left Then
                    position = Disposition.Left
                ElseIf mParentForm.ScatterGraphParameter.YAxes(I).CaptionPosition = YAxisPosition.Right Then
                    position = Disposition.Right
                Else
                    position = Disposition.None
                End If

                drDataRow("РасположениеЧисла") = position
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)

            strSQL = "SELECT ПараметрОтображения.* " &
                    "FROM ПараметрОтображения " &
                    "WHERE keyКонфигурацияОтображения = " & mParentForm.KeyConfiguration
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)

            For I = 0 To SlidePlots.CustomDivisions.Count - 1
                SlidePlots.Value = I  ' начинается с 0
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("keyКонфигурацияОтображения") = mParentForm.KeyConfiguration
                drDataRow("ИмяПараметра") = SlidePlots.CustomDivisions(I).Text
                ' к какой оси приписан шлейф
                drDataRow("НомерОси") = Val(mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)
                drDataRow("НомерЦвета") = (CInt(SlidePlots.Value)) Mod 7
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next

            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            Thread.Sleep(1000)
            cn.Close()
            isNeedToSave = False
            Application.DoEvents()
        Else ' надо ввести имя
            Const caption As String = "Запись конфигурации"
            Const text As String = "Введите имя"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
        End If
    End Sub
#End Region

#Region "ButtonConfiguration Event"
    Private Sub ButtonRestoreConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRestoreConfiguration.Click
        If ComboBoxListConfigurations.Text <> "" Then
            RestoreConfiguration($"ИмяКонфигурации)= '{ComboBoxListConfigurations.Text}'))")
            isNeedToSave = False
        End If
    End Sub

    Private Sub ButtonSaveConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveConfiguration.Click
        ButtonClose.Enabled = False
        Application.DoEvents()
        SaveConfiguration()
        ButtonClose.Enabled = True
        Application.DoEvents()
        isNeedToSave = False
    End Sub

    Private Sub ButtonDeleteConfiguration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteConfiguration.Click
        Dim strSQL As String
        Dim nameConfiguration As String = ComboBoxListConfigurations.Text
        Dim isSelectedIndexLast As Boolean ' удаляется Последняя По Списку
        Dim cn As OleDbConnection
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim cb As OleDbCommandBuilder

        If nameConfiguration <> "" AndAlso ComboBoxListConfigurations.SelectedIndex <> -1 Then
            If ComboBoxListConfigurations.Items.Count > 1 Then

                If ComboBoxListConfigurations.SelectedIndex = ComboBoxListConfigurations.Items.Count - 1 Then isSelectedIndexLast = True

                ' удалить из листа
                ComboBoxListConfigurations.Items.RemoveAt(ComboBoxListConfigurations.SelectedIndex)
                cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
                ' Открыть подключение
                strSQL = $"SELECT КонфигурацииОтображения.* FROM КонфигурацииОтображения WHERE ИмяКонфигурации = '{nameConfiguration}'"
                ' Создание recordset
                cn.Open()
                odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
                odaDataAdapter.Fill(dtDataTable)

                If dtDataTable.Rows.Count > 0 Then dtDataTable.Rows(0).Delete()

                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.Update(dtDataTable)
                cn.Close()
                Thread.Sleep(500)

                If isSelectedIndexLast = True Then
                    ComboBoxListConfigurations.SelectedIndex = ComboBoxListConfigurations.Items.Count - 1
                    ButtonRestoreConfiguration.PerformClick()
                End If
            Else
                Const caption As String = "Удаление Списка"
                Const text As String = "Последнюю запись удалять нельзя!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If
        End If
    End Sub
#End Region

#Region "RadioButtonTypeAxis Events"
    Private Sub RadioButtonTypeAxis_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonTypeAxisX.CheckedChanged, RadioButtonTypeAxisY.CheckedChanged
        SetControlsColorAndEnabled()
        isNeedToSave = True
    End Sub

    Private Sub SetControlsColorAndEnabled()
        SetControlsEnabled(Not RadioButtonTypeAxisX.Checked)

        If RadioButtonTypeAxisX.Checked Then
            SetControlsColor(constShade)
        Else
            SetControlsColor(constFace)
        End If
    End Sub

    Private Sub SetControlsEnabled(inEnabled As Boolean)
        ComboBoxTimeTail.Enabled = inEnabled
        NumericEditParamMin.Enabled = inEnabled
        NumericEditParamMax.Enabled = inEnabled
        TreeViewParametersAxis.Enabled = inEnabled
    End Sub

    Private Sub SetControlsColor(inColor As Color)
        RadioButtonTypeAxisY.BackColor = inColor
        LabelSelectParameterAxisX.BackColor = inColor
        LabelTimeTail.BackColor = inColor
        ShapeParameter.BackColor = inColor
        LabelMinAxisX.BackColor = inColor
        LabelMaxAxisX.BackColor = inColor
        LabelSelectedParameter.BackColor = inColor
    End Sub
#End Region

#Region "TreeView Events"
    Private Sub TreeViewParameter_BeforeExpand(ByVal [source] As Object, ByVal e As TreeViewCancelEventArgs) Handles TreeViewParametersAxis.BeforeExpand,
                                                                                                                    TreeViewParameters.BeforeExpand
        If Not e.Node.Parent Is Nothing Then
            For Each loopNode As TreeNode In e.Node.Parent.Nodes
                If loopNode.IsExpanded Then loopNode.Collapse()
            Next
        Else ' по самому первому уровню
            Dim mTreeViewParameters As TreeView = CType([source], TreeView)
            For Each loopNode As TreeNode In mTreeViewParameters.Nodes
                If loopNode.IsExpanded Then loopNode.Collapse()
            Next
        End If

        e.Node.EnsureVisible()
        e.Node.BackColor = Color.Gold

        If e.Node.Tag.ToString = "Root" Then e.Node.ImageIndex = 13
    End Sub

    Private Sub TreeViewParameters_AfterCollapse(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewParametersAxis.AfterCollapse,
                                                                                                                TreeViewParameters.AfterCollapse
        e.Node.BackColor = Color.White

        If e.Node.Tag.ToString = "Root" Then e.Node.ImageIndex = 12
    End Sub

    Private Sub TreeViewParametersAxis_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewParametersAxis.AfterSelect
        If e.Node.Tag.ToString <> "Root" Then
            LabelSelectedParameter.Text = e.Node.Text
            isNeedToSave = True
        End If
    End Sub

    Private Sub TreeViewParameters_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles TreeViewParameters.AfterSelect
        If e.Node.Tag.ToString <> "Root" Then
            LabelSelectedParameterGraph.Text = e.Node.Text
            ButtonAddPlot.Text = "Добавить шлейф " & e.Node.Text
            isNeedToSave = True
        End If
    End Sub
#End Region

    Private Sub Controls_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles NumericEditParamMin.AfterChangeValue,
                                                                                                                    NumericEditParamMax.AfterChangeValue,
                                                                                                                    NumericEditAxisYMin.AfterChangeValue,
                                                                                                                    NumericEditAxisYMax.AfterChangeValue,
                                                                                                                    SlidePositionTicks.AfterChangeValue,
                                                                                                                    SlidePositionNumeric.AfterChangeValue
        isNeedToSave = True
    End Sub

#Region "Plots"
    Private Sub ButtonAddPlot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAddPlot.Click
        AddPlotToAxis(CInt(SlideAssignAxis.Value))
        isNeedToSave = True
    End Sub

    Private Sub ButtonRemovePlot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonRemovePlot.Click
        RemoveAtPlot(CInt(SlidePlots.Value))
        isNeedToSave = True
    End Sub

    ''' <summary>
    ''' Удалить Шлейф
    ''' </summary>
    ''' <param name="numberRemovedPlot"></param>
    Private Sub RemoveAtPlot(ByVal numberRemovedPlot As Integer)
        Dim I As Integer

        isProcessDeletePlot = True
        SlidePlots.CustomDivisions.RemoveAt(numberRemovedPlot)
        isProcessDeletePlot = False

        ' переопределить значения
        ' начинается с 0
        For I = 0 To SlidePlots.CustomDivisions.Count - 1
            SlidePlots.CustomDivisions(I).Value = I
        Next

        If SlidePlots.CustomDivisions.Count > 0 Then
            If SlidePlots.CustomDivisions.Count = 1 Then
                ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(0).Text
            Else
                ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(CInt(SlidePlots.Value - 1)).Text
            End If
        End If

        mParentForm.ScatterGraphParameter.Plots.RemoveAt(numberRemovedPlot)

        countPlots = mParentForm.ScatterGraphParameter.Plots.Count
        SetRangeForSlidePlots(countPlots)

        If mParentForm.ScatterGraphParameter.Plots.Count > 0 Then
            If numberRemovedPlot > mParentForm.ScatterGraphParameter.Plots.Count - 1 Then ' если удалялся последний график
                SlidePlots.Value = mParentForm.ScatterGraphParameter.Plots.Count - 1
            Else ' если удалялся не последний график
                SlidePlots.Value = numberRemovedPlot
            End If

            For I = 0 To mParentForm.ScatterGraphParameter.Plots.Count - 1
                mParentForm.ScatterGraphParameter.Plots(I).LineColor = ColorsNet(I Mod 7)
            Next

            SlideAssignAxis.Value = Val(mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)
        End If

        'If PlotsCount > 0 Then sldPlot.Value = PlotsCount - 1
        SlidePlots.Enabled = countPlots > 1

        If countPlots = 0 Then
            ButtonRemovePlot.Enabled = False
            ButtonAssignPlotToAxis.Enabled = False
        End If

        LabelGrafColorNext.BackColor = ColorsNet(countPlots Mod 7)
        UpdateCaptionAxes()
    End Sub

    Private Sub SlidePlots_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlidePlots.AfterChangeValue
        If isProcessDeletePlot Then Exit Sub

        If IsHandleCreated Then
            SlidePlots.PointerColor = ColorsNet(CInt(SlidePlots.Value Mod 7))
            ' sldPlot переместился, значит присвоить новое значение sldAssignAxis.Value
            SlideAssignAxis.Value = Val(mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag)

            For I As Integer = 0 To mParentForm.ScatterGraphParameter.Plots.Count - 1
                mParentForm.ScatterGraphParameter.Plots(I).LineWidth = 1
                mParentForm.ScatterGraphParameter.Plots(I).PointStyle = PointStyle.SolidDiamond
            Next

            mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).LineWidth = 2
            mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).PointStyle = PointStyle.SolidSquare
            ButtonRemovePlot.Text = "Удалить шлейф " & SlidePlots.CustomDivisions(CInt(e.NewValue)).Text
            isNeedToSave = True
        End If
    End Sub

    Private Sub SlideAssignAxis_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideAssignAxis.AfterChangeValue
        SlideAssignAxis.PointerColor = ColorsNet(CInt(e.NewValue Mod 7))
        ButtonAssignPlotToAxis.Text = "Прикрепить шлейф к оси " & CStr(e.NewValue)
        isNeedToSave = True
    End Sub

    ''' <summary>
    ''' Прикрепить График
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub ButtonAssignPlotToAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAssignPlotToAxis.Click
        mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis = mParentForm.ScatterGraphParameter.YAxes(CInt(SlideAssignAxis.Value))
        mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).PointColor = mParentForm.ScatterGraphParameter.YAxes(CInt(SlideAssignAxis.Value)).CaptionForeColor
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub
#End Region

#Region "Axis"
    Private Sub ButtonAddAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonAddAxis.Click
        mParentForm.ScatterGraphParameter.YAxes.Add(New YAxis())
        SetAttributeAxisY()
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub

    Private Sub ButtonRemoveAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonRemoveAxis.Click
        Dim I, J As Integer
        Dim numberRemovedAxis As Integer = CInt(SlideSelectAxis.Value)

        If numberRemovedAxis = 0 Then Exit Sub

        ' удалить те шлейфы, где встречаются номера удаляемой оси
        For I = mParentForm.ScatterGraphParameter.Plots.Count - 1 To 0 Step -1 ' сверху вниз
            For J = SlidePlots.CustomDivisions.Count - 1 To 0 Step -1
                SlidePlots.Value = SlidePlots.CustomDivisions(J).Value
                If Val(mParentForm.ScatterGraphParameter.Plots(CInt(SlidePlots.Value)).YAxis.Tag) = numberRemovedAxis Then
                    RemoveAtPlot(CInt(SlidePlots.Value))
                End If
            Next
        Next

        ' а затем удалить саму дополнительную ось
        If mParentForm.ScatterGraphParameter.YAxes.Count > 0 Then mParentForm.ScatterGraphParameter.YAxes.RemoveAt(numberRemovedAxis)

        ' дать новые имена осям
        For I = 0 To mParentForm.ScatterGraphParameter.YAxes.Count - 1
            mParentForm.ScatterGraphParameter.YAxes(I).Tag = CStr(I)
        Next

        ' переместиь ползунок
        If numberRemovedAxis > mParentForm.ScatterGraphParameter.YAxes.Count - 1 Then
            SlideSelectAxis.Value = mParentForm.ScatterGraphParameter.YAxes.Count - 1
        Else
            SlideSelectAxis.Value = numberRemovedAxis
        End If

        For I = 0 To mParentForm.ScatterGraphParameter.YAxes.Count - 1
            ' присвоить цвета по умолчанию
            mParentForm.ScatterGraphParameter.YAxes(I).CaptionForeColor = ColorsNet(I Mod 7)
            mParentForm.ScatterGraphParameter.YAxes(I).MajorDivisions.TickColor = ColorsNet(I Mod 7)
            mParentForm.ScatterGraphParameter.YAxes(I).MajorDivisions.LabelForeColor = ColorsNet(I Mod 7)
            mParentForm.ScatterGraphParameter.YAxes(I).MinorDivisions.TickColor = ColorsNet(I Mod 7)

            ' просмотр всех графиков принадлежащих данной оси и установка цвета
            For J = SlidePlots.CustomDivisions.Count - 1 To 0 Step -1
                If mParentForm.ScatterGraphParameter.Plots(J).YAxis.Tag.ToString = mParentForm.ScatterGraphParameter.YAxes(I).Tag.ToString Then
                    mParentForm.ScatterGraphParameter.Plots(J).PointColor = mParentForm.ScatterGraphParameter.YAxes(I).CaptionForeColor
                End If
            Next
        Next

        countAxes = mParentForm.ScatterGraphParameter.YAxes.Count

        If SlideAssignAxis.Value > countAxes - 1 Then SlideAssignAxis.Value = countAxes - 1
        If SlideSelectAxis.Value > countAxes - 1 Then SlideSelectAxis.Value = countAxes - 1

        SetRangeForSlideAssignAxis(countAxes)

        ButtonRemoveAxis.Enabled = countAxes > 1
        ButtonAddAxis.Enabled = countAxes < cMaxAxes
        LabelAxisColorNext.BackColor = ColorsNet((countAxes) Mod 7)
        UpdateCaptionAxes()
        isNeedToSave = True
    End Sub

    Private Sub ButtonUpdateAxis_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonUpdateAxis.Click
        If mParentForm.ScatterGraphParameter.YAxes.Count = 1 AndAlso SlideSelectAxis.Value > 0 Then Exit Sub

        If SlidePositionTicks.Value = Disposition.Left Then
            mParentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Position = YAxisPosition.Left
        ElseIf SlidePositionTicks.Value = Disposition.Right Then
            mParentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Position = YAxisPosition.Right
        End If

        If SlidePositionNumeric.Value = Disposition.Left Then
            mParentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).CaptionPosition = YAxisPosition.Left
        ElseIf SlidePositionNumeric.Value = Disposition.Right Then
            mParentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).CaptionPosition = YAxisPosition.Right
        End If

        mParentForm.ScatterGraphParameter.YAxes(CInt(SlideSelectAxis.Value)).Range = New Range(NumericEditAxisYMin.Value, NumericEditAxisYMax.Value)
        isNeedToSave = True
    End Sub

    Private Sub SlideSelectAxis_AfterChangeValue(ByVal sender As Object, ByVal e As AfterChangeNumericValueEventArgs) Handles SlideSelectAxis.AfterChangeValue
        If IsHandleCreated Then
            SlideSelectAxis.PointerColor = ColorsNet(CInt(e.NewValue Mod 7))

            If mParentForm.ScatterGraphParameter.YAxes.Count = 1 AndAlso e.NewValue > 0 Then Exit Sub

            If mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Position = YAxisPosition.Left Then
                SlidePositionTicks.Value = Disposition.Left
            ElseIf mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Position = YAxisPosition.Right Then
                SlidePositionTicks.Value = Disposition.Right
            Else
                SlidePositionTicks.Value = Disposition.None
            End If

            If mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).CaptionPosition = YAxisPosition.Left Then
                SlidePositionNumeric.Value = Disposition.Left
            ElseIf mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).CaptionPosition = YAxisPosition.Right Then
                SlidePositionNumeric.Value = Disposition.Right
            Else
                SlidePositionNumeric.Value = Disposition.None
            End If

            NumericEditAxisYMin.Value = mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Range.Minimum
            NumericEditAxisYMax.Value = mParentForm.ScatterGraphParameter.YAxes(CInt(e.NewValue)).Range.Maximum

            If e.NewValue = 0 Then
                ButtonRemoveAxis.Enabled = False
                ButtonRemoveAxis.Text = "Удалить нельзя"
            Else
                ButtonRemoveAxis.Enabled = True
                ButtonRemoveAxis.Text = "Удалить ось " & CStr(e.NewValue)
            End If

            isNeedToSave = True
        End If
    End Sub
#End Region

#Region "Frequency"
    Private Sub ComboBoxTimeTail_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxTimeTail.SelectedIndexChanged
        If IsHandleCreated Then
            SetCounterLightByUser()
            isNeedToSave = True
        End If
    End Sub

    Private Sub ComboBoxFrequency_SelectedIndexChanged(ByVal ByVeventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxFrequency.SelectedIndexChanged
        If IsHandleCreated Then
            If ComboBoxTimeTail.SelectedIndex = -1 Then Exit Sub

            SetCounterLightByUser()
            isNeedToSave = True
        End If
    End Sub
#End Region

End Class
