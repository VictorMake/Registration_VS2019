Option Strict Off

Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.Globalization
Imports System.IO
Imports System.Math
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports MathematicalLibrary
Imports Microsoft.Office.Interop
Imports NationalInstruments.DAQmx
Imports VB = Microsoft.VisualBasic

Friend MustInherit Class FormSnapshotBase
    ''' <summary>
    ''' Сохранение расшифровки в Excel
    ''' </summary>
    Private isSavedDecodingToExcel As Boolean = False
    ' при смене масштаба оси У пересчет  координат линии производить в единицы вольт
    ' также перед записью в свойства линии для редактирования из списка коллекции вибирается редактируемый и уничтожается
    ' печать
    Private axisXMinimum, axisYMinimum, axisXMaximum, axisYMaximum As Double
    Private frameX1 As Single = 29.61
    Private frameX2 As Single = 631.37
    Private frameY1 As Single = 424
    Private frameY2 As Single = 8
    Private chartLeft As Single = 4
    Private chartTop As Single = 1
    Private chartWidth As Single = 639
    Private chartHeight As Single = 441
    ''' <summary>
    ''' Индекс строк таблицы протокола
    ''' </summary>
    Private offsetRow As Short
    ''' <summary>
    ''' Связывает два контрола ToolStripButton и ToolStripMenuItem
    ''' </summary>
    Private utilityHelper As UtilityHelper
    ''' <summary>
    ''' Отображение текста в статусе
    ''' </summary>
    Private lastStatus As String
    ''' <summary>
    ''' Для хранения даты
    ''' </summary>
    Private objDate As Object
    ''' <summary>
    ''' Количество строк в снимке
    ''' </summary>
    Protected arraysizeSnapshot As Integer
    ''' <summary>
    ''' Примечание снимка
    ''' </summary>
    Protected descriptionSnapshot As String
    ''' <summary>
    ''' Приведенное значение оси Х.
    ''' Если ось Х по параметру, а он приведенный, то его расчитать.
    ''' </summary>
    Private correctedValueAxisX As Double
    ''' <summary>
    ''' Файл снимка в формате Tdms
    ''' </summary>
    Private isReadFileSnapshotTdms As Boolean
    ''' <summary>
    ''' Время начала сбора
    ''' </summary>
    Protected timeStartRecord As Object

#Region "Линии и стрелки"
    ''' <summary>
    ''' Где вывести контестное меню
    ''' </summary>
    Private pointMouse As Point
    ''' <summary>
    ''' Для вывода контекстного меню на курсоре
    ''' </summary>
    Private contextCursor As XYCursor
    ''' <summary>
    ''' Для вывода контекстного меню на стрелке
    ''' </summary>
    Private contextAnnotation As XYAnnotation
    ''' <summary>
    ''' Для показа имени шлейфа
    ''' </summary>
    Private cursorAnnotation As XYPointAnnotation
#End Region

#Region "курсоры"
    ''' <summary>
    ''' Для запоминания текущей позиции 1 курсора
    ''' </summary>
    Protected xCursorStart As Double  ' 
    ''' <summary>
    ''' Для запоминания текущей позиции 2 курсора
    ''' </summary>
    Protected xCursorEnd As Double    ' 
#End Region

#Region "Ссылки"
    ''' <summary>
    ''' Фоновый Список
    ''' </summary>
    ''' <remarks></remarks>
    Private navigatorBackgroundSnapshot As FormNavigatorSnapshot
#End Region

    Protected Sub New()
        'Public Sub New()
        Me.New(New FormMainMDI, FormExamination.RegistrationSCXI, "FormSnapshotBase")
        'InitializeComponent()
    End Sub

    Public Sub New(ByVal frmParent As FormMainMDI, ByVal kindExamination As FormExamination, ByVal captionText As String)
        MyBase.New(frmParent, kindExamination, captionText)

        ' Этот вызов является обязательным для конструктора.
        'InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
    End Sub

#Region "Реализовать интерфейс"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected MustOverride Sub InheritFormLoad()
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub InheritFormClosing(ByRef e As FormClosingEventArgs)
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected MustOverride Sub InheritFormClosed()
    ''' <summary>
    ''' Настройка меню и кнопок продолжение
    ''' </summary>
    Protected MustOverride Sub EnableDisableControlsInherit()
    ''' <summary>
    ''' Обработать Снимок
    ''' </summary>
    Friend MustOverride Sub ProcesSnapshot()
    ''' <summary>
    ''' Установить видимость MenuSimulator для включения иммитатора
    ''' </summary>
    Friend MustOverride Sub SetEnabledMenuSimulator()
#End Region

#Region "Реализация интерфейса"
    ''' <summary>
    ''' Происходит до первоначального отображения формы.
    ''' Main->Base->Inherit
    ''' </summary>
    Protected Overrides Sub BaseFormLoad()
        objDate = Today

        ' обязательно в конце
        InheritFormLoad()
    End Sub
    ''' <summary>
    ''' Происходит перед закрытием формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub BaseFormClosing(ByRef e As FormClosingEventArgs)
        ' Обязательно вначале этого метода
        InheritFormClosing(e)
        If isCancel = True Then Exit Sub ' пользователь не хочет выходить
        ' реализация далее...

    End Sub
    ''' <summary>
    ''' Происходит после закрытия формы.
    ''' Inherit->Base->Main
    ''' </summary>
    Protected Overrides Sub BaseFormClosed()
        ' Обязательно вначале этого метода
        InheritFormClosed()
        ' реализация далее...
        MenuNewWindowSnapshot.Enabled = True
        MainMDIFormParent.MenuNewWindowSnapshot.Enabled = True

        If navigatorBackgroundSnapshot IsNot Nothing Then navigatorBackgroundSnapshot.Close()
    End Sub

    ''' <summary>
    ''' Настроить график под выбранный параметр Оси У
    ''' </summary>
    Protected Overrides Sub TuneDiagramUnderSelectedParameterAxesY()
        If isFormLoaded Then
            ' выбор оси
            Dim positionCursor As Double
            Dim I, J As Integer

            If IsSnapshot Then ' если регистратор запущен то перестройки оси нет
                SlidePlot.PointerColor = ColorsNet(CInt((SlidePlot.Value - 1) Mod 7))
                NumberParameterAxes = CInt(SlidePlot.Value)

                ' здесь надо массив пересчитать
                For I = 0 To UBound(MeasuredValues)
                    For J = 0 To UBound(MeasuredValues, 2)
                        MeasuredValuesToRange(I, J) = CastToAxesStandard(NumberParameterAxes, I + 1, MeasuredValues(I, J))
                    Next
                Next

                ' обновить график с новым массивом
                Dim startAxisX As Integer = CInt(XAxisTime.Range.Minimum)
                Dim endAxisX As Integer = CInt(XAxisTime.Range.Maximum)
                If startAxisX < 0 Then startAxisX = 0
                If endAxisX > UBound(MeasuredValues, 2) Then endAxisX = UBound(MeasuredValues, 2)

                XAxisTimeRange = New Range(startAxisX, endAxisX)

                If Arrows.Count <> 0 Then
                    ' для того, чтобы обычные аннотации стёрлись
                    MenuPens.Checked = False
                End If

                RemoveArrows()
                WaveformGraphTime.ClearData()

                For Each axis As XAxis In WaveformGraphTime.XAxes
                    ' чтобы Автомасштабировать
                    axis.Mode = AxisMode.AutoScaleLoose
                Next

                ' временно как будто полный экран
                YAxisTime.Range = New Range(RangesOfDeviation(NumberParameterAxes, 1), RangesOfDeviation(NumberParameterAxes, 2))

                ' если регистратор и если снимок с частотой сбора гц то
                If (RegimeType = cРегистратор AndAlso FrequencyBackgroundSnapshot < FrequencyHandQuery) OrElse isRegimeChangeForDecoding Then
                    XAxisTime.Range = New Range(0, arraysizeSnapshot)
                Else ' если осциллограф то
                    XAxisTime.Range = New Range(0, CDbl(ComboBoxTimeMeasurement.Text) * FrequencyHandQuery)
                End If

                WaveformGraphTime.PlotYMultiple(MeasuredValuesToRange)
                RestoreArrows()
                ' а теперь возвращаем записанные мин мах
                XAxisTime.Range = XAxisTimeRange
                SlideAxis.Range = New Range(YAxisTime.Range.Minimum, YAxisTime.Range.Maximum)
                ' текущее начало отсчета перевести в точку массива
                SlideAxis.Value = MeasuredValuesToRange(CInt(SlidePlot.Value - 1), CInt(Int(XAxisTime.Range.Minimum) + 5)) ' отступить 5 отсчётов т.к. для расчётных несколько первых значений нулевые
                RestoreAnnotations()
                ' если курсоры были выставлены то сохранить их позиции
                positionCursor = YAxisTime.Range.Minimum + (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum) / 2
                ' восстановить курсоры
                XyCursorStart.Plot = WaveformGraphTime.Plots(0)
                XyCursorEnd.Plot = WaveformGraphTime.Plots(0)

                If GraphModeValue = MyGraphMode.OneCursor Then
                    XyCursorStart.MoveCursor(xCursorStart, positionCursor)
                ElseIf GraphModeValue = MyGraphMode.TwoCursors Then
                    XyCursorStart.MoveCursor(xCursorStart, positionCursor)
                    XyCursorEnd.MoveCursor(xCursorEnd, positionCursor)
                End If

                ComboBoxSelectAxis.SelectedIndex = CInt(SlidePlot.Value - 1)
                LabelIndicator.Text = "Ось Y по " & ComboBoxSelectAxis.Text
                ' выделить выбранный график
                WaveformGraphTime.Plots.Item(NumberParameterAxes - 1).LineWidth = 2
                ' intNПараметраОсиСмененного может и не быть в данном снимке

                Try
                    WaveformGraphTime.Plots.Item(numberParameterAxesChanged - 1).LineWidth = 1
                Catch ex As ArgumentOutOfRangeException
                    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(TuneDiagramUnderSelectedParameterAxesY)}> {ex}")
                End Try

                numberParameterAxesChanged = NumberParameterAxes
            End If
        End If
    End Sub

    Protected Overrides Sub CleaningDiagram(ByRef N As Integer, ByRef arrParametersCurrentOrSnapshot() As TypeBaseParameter)
        Dim plot As WaveformPlot

        ClearArrowCollection()
        WaveformGraphTime.Annotations.Clear()
        WaveformGraphTime.Plots.Clear()

        For I As Integer = 1 To N
            plot = GetLineLoopTrand(I)
            WaveformGraphTime.Plots.Add(plot)

            If IsRegimeIsRegistrator Then ' только для режима регистратор
                If isDetailedSheet Then
                    plot.Tag = arrParametersCurrentOrSnapshot(IndexParameters(I)).NameParameter
                    plot.Visible = arrParametersCurrentOrSnapshot(IndexParameters(I)).IsVisible
                Else
                    plot.Tag = SnapshotSmallParameters(I).NameParameter
                    plot.Visible = SnapshotSmallParameters(I).IsVisible
                End If
            End If
        Next

        If WaveformGraphTime.Plots.Count = 0 Then
            'Const caption As String ="ОчисткаГрафиков"
            'Dim text As String = "Отсутствуют шлейфы в графике по времени!" & vbCrLf & "Возможны ошибки при перемещении курсора." & vbCrLf & "Произведите заново настройку видимости шлейфов."
            'MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            ' принудительно включим видимость
            If IsRegimeIsRegistrator Then
                plot = GetLineLoopTrand(1)

                If isDetailedSheet Then
                    arrParametersCurrentOrSnapshot(IndexParameters(1)).IsVisible = True
                    plot.Tag = ParametersType(IndexParameters(1)).NameParameter
                Else
                    SnapshotSmallParameters(1).IsVisible = True
                    plot.Tag = SnapshotSmallParameters(1).NameParameter
                End If

                WaveformGraphTime.Plots.Add(plot)
            End If
        End If

        For Each Cursor As XYCursor In WaveformGraphTime.Cursors
            Cursor.Plot = WaveformGraphTime.Plots(0)
        Next

        SlidePlot.Enabled = True
        If N > 1 Then SlidePlot.Range = New Range(1, N) ' иначе ошибка равенства диапазонов
    End Sub

    ''' <summary>
    ''' Настройка меню и кнопок
    ''' </summary>
    Protected Overrides Sub EnableDisableControlsBase()
        ComboBoxPointers.Items.Add("Время")
        ComboBoxPointers.Items.Add("Амплитуда")
        ComboBoxPointers.Items.Add("Линия")
        ComboBoxPointers.SelectedIndex = 0

        TableLayoutPanelFrameCursor.ColumnStyles(0).Width = 0
        TableLayoutPanelFrameCursor.ColumnStyles(1).Width = 0
        TableLayoutPanelFrameCursor.ColumnStyles(2).Width = 0

        SplitContainerForm.SplitterDistance = 456
        SplitContainer2.SplitterDistance = 100
        XyCursorTime.Visible = False
        TableLayoutPanelУправленияДляСнимка.Visible = True
        InstrumentControlStrip1.Visible = False
        TableLayoutPanelУправленияДляСнимка.RowStyles(1).Height = 0
        TableLayoutPanelУправленияДляСнимка.Dock = DockStyle.Fill
        PanelKey.Visible = True
        LabelIndicator.Visible = True
        SlideAxis.Visible = True
        PanelSlide.Visible = True
        ButtonSnapshot.Enabled = True
        ButtonContinuously.Enabled = False
        ButtonRecord.Enabled = False

        TextBoxRecordToDisc.Visible = False
        ComboBoxTimeMeasurement.Visible = True
        LabelSec.Visible = True
        ComboBoxSelectAxis.Enabled = True
        MenuOpenBaseSnapshot.Enabled = True
        MenuWriteDecodingSnapshotToDBase.Enabled = True
        MenuPrintDecodingSnapshot.Enabled = True
        MenuDecoding.Enabled = True
        MenuRegistration.Enabled = False
        MenuExportSnapshotInExcel.Enabled = True
        MenuShowGraphControl.Enabled = False
        TextBoxIndicatorN1physics.Visible = False
        TextBoxIndicatorN1reduce.Visible = False
        TextBoxIndicatorN2physics.Visible = False
        TextBoxIndicatorN2reduce.Visible = False
        TextBoxIndicatorRud.Visible = False
        ComboBoxDisplayRate.Visible = False
        MenuSelectiveGraphControl.Enabled = False
        MenuSelectiveTextControl.Enabled = False

        If IsTcpClient Then
            MenuModeOfOperation.Enabled = False
            MenuServerEnable.Enabled = False
        End If

        StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyHandQuery) & "Гц"
        utilityHelper = New UtilityHelper

        InitializeMenuHelperStrings(MenuDecoding.DropDownItems)
        MapToolBarAndMenuItems()

        GraphModeValue = MyGraphMode.Scaling
        SetGraphMode(GraphModeValue)

        InitializeInteractionMenu()
        InitializeContextMenu()
        FillListTimeCollection()

        'cmbТексты.DataSource = mcolСтрелок
        ComboBoxDescriptionPointer.DisplayMember = "ToString"
        'cmbТексты.ValueMember = "Key"

        EnableDisableControlsInherit()
    End Sub

    ''' <summary>
    ''' вызывается при образовании формы
    ''' </summary>
    Friend Overrides Sub StartMeasurement()
        ApplyRegimeDebugging()
    End Sub

    'Protected Overrides Sub SettingsOptionProgram()
    '    ' реализуется далее
    'End Sub

    Protected Overrides Sub ApplyTuningVisibilityMeasuringStub()
        If IsBeforeThatHappenLoadDbase Then
            SetEnableTrande(UBound(IndexParameters), ParametersShaphotType)
        Else
            SetEnableTrande(UBound(IndexParameters), ParametersType)
        End If

        If isUsePens Then TuneAnnotation()
    End Sub

    ''' <summary>
    ''' Обновить графики от параметров для снимка
    ''' </summary>
    ''' <param name="inClassGrafOfParam"></param>
    Protected Overrides Sub ReloadDiagramParameterFromParameterForSnapshot(ByRef inClassGrafOfParam As GraphsOfParameters.GraphOfParameter)
        Dim coefficientBringingTBoxingInSnapshot As Double = Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin))
        Dim xData, yData, result As Double()
        Dim J, indexResult As Integer
        Dim rangeMin As Integer = CInt(Int(XAxisTime.Range.Minimum))
        Dim rangeMax As Integer = CInt(Int(XAxisTime.Range.Maximum))

        'ReDim_xData(rangeMax - rangeMin)
        'ReDim_yData(rangeMax - rangeMin)
        'ReDim_result(rangeMax - rangeMin + 1)
        Re.Dim(xData, rangeMax - rangeMin)
        Re.Dim(yData, rangeMax - rangeMin)
        Re.Dim(result, rangeMax - rangeMin + 1)

        If inClassGrafOfParam.IsTestPass Then
            For Each itemAxis As GraphsOfParameters.GraphOfParameter.Axis In inClassGrafOfParam.AxisGraph.Values
                If itemAxis.IsUseFormula = True Then
                    indexResult = 1

                    For J = rangeMin To rangeMax
                        expressionMath = itemAxis.Formula

                        For Each itemParameter As GraphsOfParameters.GraphOfParameter.Axis.Parameter In itemAxis.Parameters.Values
                            expressionMath = expressionMath.Replace(itemParameter.Name, MeasuredValues(itemParameter.IndexInArrayParameters, J).ToString)
                        Next

                        Try
                            Dim eval As New JScriptUtil.ExpressionEvaluator
                            result(indexResult) = CDbl(eval.Evaluate(expressionMath))
                        Catch ex As Exception
                            Dim caption As String = expressionMath
                            Dim text As String = ex.ToString
                            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                            Exit Sub
                        End Try

                        indexResult += 1
                    Next
                Else
                    Dim parameterForAxis As GraphsOfParameters.GraphOfParameter.Axis.Parameter = itemAxis.ParameterForAxis
                    indexResult = 1

                    For J = rangeMin To rangeMax
                        If parameterForAxis.Reduction Then
                            result(indexResult) = MeasuredValues(parameterForAxis.IndexInArrayParameters, J) * coefficientBringingTBoxingInSnapshot
                        Else
                            result(indexResult) = MeasuredValues(parameterForAxis.IndexInArrayParameters, J)
                        End If

                        indexResult += 1
                    Next
                End If

                If itemAxis.NameAxis = "X" Then
                    Array.Copy(result, 1, xData, 0, result.Length - 1)
                Else
                    Array.Copy(result, 1, yData, 0, result.Length - 1)
                End If
            Next

            GraphsByParameters.Item(inClassGrafOfParam.NameGraph).UpdateSnapshotGraphByParameter(xData, yData, FrequencyBackgroundSnapshot)
        End If
    End Sub

    'Protected Overrides Sub CharacteristicForRegime()
    '    ' реализуется далее
    'End Sub

    Protected Overrides Sub DetailSelectiveBase()
        Try
            If isDetailedSheet Then
                ' в режиме снимок, когда не был произведен ни один замер, ParametersShaphotType пуст
                If ParametersShaphotType Is Nothing Then Exit Sub

                For I = 1 To UBound(IndexParameters)
                    WaveformGraphTime.Plots.Item(I - 1).Visible = ParametersShaphotType(IndexParameters(I)).IsVisible
                Next
            Else
                For I = 1 To UBound(SnapshotSmallParameters)
                    WaveformGraphTime.Plots.Item(I - 1).Visible = SnapshotSmallParameters(I).IsVisible
                Next
            End If

            XAxisTime.Range = New Range(CInt(XAxisTime.Range.Minimum), CInt(XAxisTime.Range.Maximum))

            If isUsePens Then
                TuneAnnotation()
            End If

            If GraphModeValue = MyGraphMode.OneCursor Then
                MoveCursor(XyCursorStart, False)
            ElseIf GraphModeValue = MyGraphMode.TwoCursors Then
                MoveCursor(XyCursorStart, False)
                MoveCursor(XyCursorEnd, False)
            End If

        Catch ex As Exception
            Dim caption As String = $"<{NameOf(DetailSelectiveBase)}>"
            Dim text As String = $"{ex}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Finally
            RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(DetailSelectiveBase)}> {ButtonDetailSelective.Text}")
        End Try
    End Sub

    ''' <summary>
    ''' Настроить аннотации.
    ''' Метод может быть полностью синхронизирован с помощью атрибута MethodlmplAttribute.
    ''' Такой подход может стать альтернативой оператору lock в тех случаях,
    ''' когда метод требуется заблокировать полностью. 
    ''' </summary>
    <MethodImplAttribute(MethodImplOptions.Synchronized)>
    Protected Overrides Sub TuneAnnotation()
        If GraphModeValue = MyGraphMode.Scaling OrElse GraphModeValue = MyGraphMode.DoNothing Then
            Exit Sub
        End If

        Dim I, indexPlot As Integer
        Dim isSldPlotSelected As Boolean
        Dim limitationVisibleTrend As Integer ' Ограничение Видимых Шлефов 

        Try
            If isRegimeChangeForDecoding Then
                RemoveAnnotationNotAarrow()
            Else
                WaveformGraphTime.Annotations.Clear()
            End If

            If GraphModeValue = MyGraphMode.TwoCursors Then
                limitationVisibleTrend = WaveformGraphTime.Plots.Count * 2
            Else
                limitationVisibleTrend = WaveformGraphTime.Plots.Count
            End If

            Dim mTypeParameter As TypeBaseParameter()
            If IsBeforeThatHappenLoadDbase Then
                mTypeParameter = ParametersShaphotType
            Else
                mTypeParameter = ParametersType
            End If

            ' слайдер остаётся прежний
            SlidePlot.PointerColor = ColorsNet(CInt((SlidePlot.Value - 1) Mod 7))
            ComboBoxSelectAxis.SelectedIndex = CInt(SlidePlot.Value - 1)
            isSldPlotSelected = True

            If isDetailedSheet Then
                For I = 1 To UBound(IndexParameters)
                    If mTypeParameter(IndexParameters(I)).IsVisible AndAlso WaveformGraphTime.Annotations.Count < limitationVisibleTrend Then
                        WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, mTypeParameter(IndexParameters(I)).NameParameter))
                        If GraphModeValue = MyGraphMode.TwoCursors Then
                            WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, mTypeParameter(IndexParameters(I)).NameParameter))
                        End If
                        indexPlot += 1
                    End If
                Next
            Else
                For I = 1 To UBound(SnapshotSmallParameters)
                    If SnapshotSmallParameters(I).IsVisible AndAlso WaveformGraphTime.Annotations.Count < limitationVisibleTrend Then
                        WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, SnapshotSmallParameters(I).NameParameter))
                        If GraphModeValue = MyGraphMode.TwoCursors Then
                            WaveformGraphTime.Annotations.Add(NewXYPointAnnotation(indexPlot, SnapshotSmallParameters(I).NameParameter))
                        End If
                        indexPlot += 1
                    End If
                Next
            End If
        Catch ex As Exception
            Dim caption As String = NameOf(TuneAnnotation)
            Dim text As String = $"{ex}{vbCrLf}Для исправления перещёлкните кнопку <Выборочно<->Подробно>"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Нет реализации
    ''' Остановить сбор или приём данных
    ''' </summary>
    Protected Overrides Sub StopAcquisition()
    End Sub
    ''' <summary>
    ''' Перезаписывает строку запроса SQL и выставляет флаг если база данных сменена
    ''' </summary>
    ''' <param name="outSQL"></param>
    ''' <param name="IsChannelShaphot"></param>
    Friend Overrides Sub GetSqlForDbase(ByRef outSQL As String, ByRef IsChannelShaphot As Boolean)
        If IsBeforeThatHappenLoadDbase Then
            IsChannelShaphot = True
            outSQL = "SELECT * FROM " & ChannelShaphot
        Else
            IsChannelShaphot = False
            outSQL = "SELECT * FROM " & ChannelLast
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Отобразить кадр испытания
    ''' </summary>
    ''' <param name="keyID"></param>
    Friend Sub ShowFrameSnapshotFromDBase(ByRef keyID As Integer)
        IsBeforeThatHappenLoadDbase = True
        MenuDecoding.Enabled = True
        LabelIndicator.Visible = True
        GKeyID = keyID
        isRegimeChangeForDecoding = False
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(ShowFrameSnapshotFromDBase)}> {NameOf(keyID)} = {keyID}")

        If GKeyID <> 0 Then
            LoadFrameSnapshotFromDBase()
            TimerCursor.Interval = 1000 \ FrequencyBackgroundSnapshot
            SlidePlot.Visible = True
            ComboBoxSelectAxis.Enabled = True

            ' для передачи в режиме имитатор
            If isUseWindowsDiagramFromParameter Then
                For Each TempClassGrafOfParam As GraphsOfParameters.GraphOfParameter In mGraphsOfParams.DictionaryGraphOfParam.Values
                    If TempClassGrafOfParam.IsTestPass Then
                        ReloadDiagramParameterFromParameterForSnapshot(TempClassGrafOfParam)
                    End If
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' Считать данные кадра испытания
    ''' </summary>
    Private Sub LoadFrameSnapshotFromDBase()
        Dim recordCount As Integer
        Dim pathTextDataStream As String
        Dim arrTransposition As Double(,)
        Dim I, J As Integer
        Dim N As Integer
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader

        Try
            ClearArrowCollection()
            Dim strSQL As String = "SELECT * FROM [БазаСнимков] WHERE [KeyID]=" & GKeyID
            Dim cmd As OleDbCommand = cn.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            cn.Open()
            rdr = cmd.ExecuteReader

            If rdr.Read() = False Then
                rdr.Close()
                cn.Close()
                Exit Sub
            End If

            NumberProductionSnapshot = CInt(rdr("НомерИзделия"))
            objDate = rdr("Дата")
            timeStartRecord = rdr("ВремяНачалаСбора")
            TemperatureBoxInSnaphot = CDbl(rdr("Тбокса"))
            TypeKRDinSnapshot = CStr(rdr("ТипКРД"))
            RegimeType = CStr(rdr("Режим"))
            StatusStripMain.Items(NameOf(LabelRegistration)).Text = RegimeType

            If RegimeType <> cРегистратор AndAlso CShort(rdr("ЧастотаОпроса")) <> FrequencyHandQuery Then isRegimeChangeForDecoding = True

            ConfigurationString = CStr(rdr("СтрокаКонфигурации"))
            ' кол. строк и стобцов востановилось
            I = CInt(rdr("КолСтолбцов"))
            J = CInt(rdr("КолСтрок"))
            arraysizeSnapshot = J
            FrequencyBackgroundSnapshot = CShort(rdr("ЧастотаОпроса"))

            ' необходимо обновить поле время сбора
            If FrequencyBackgroundSnapshot = FrequencyHandQuery Then
                ComboBoxTimeMeasurement.SelectedIndex = CInt(rdr("КолСтрок")) \ CInt(rdr("ЧастотаОпроса")) \ 10 - 1
            Else
                If Not isRegimeChangeForDecoding Then
                    ButtonSnapshot.Enabled = False ' здесь выключить кнопку снимка
                End If
            End If
            ' при движении прокрутки снимков использовать такой перевод начала и конца снимка
            ' format(0.032,"long time")

            If Not IsDBNull(rdr("Примечание")) Then descriptionSnapshot = CStr(rdr("Примечание"))

            pathTextDataStream = CStr(rdr("ПутьНаДиске"))
            ChannelShaphot = CStr(rdr("ТаблицаКаналов"))
            CaptionGraf = $"Изделие №{NumberProductionSnapshot} Дата:{CType(objDate, Date).ToShortDateString} {CType(timeStartRecord, Date).ToShortTimeString} {RegimeType} {descriptionSnapshot}"
            rdr.Close()

            ' загрузка каналов из таблицы strChannelСнимка
            pathTextDataStream = ProduceRightPath(pathTextDataStream)
            IsDataBaseChanged = True
            LoadChannelsSnapshot() ' здесь база закрывается

            ' установить ссылку заново
            ' считать файл с физикой
            If FileNotExists(pathTextDataStream) Then
                cn.Close()
                MessageBox.Show("Нет файла " & pathTextDataStream, "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If arraysizeSnapshot = 0 Then
                cn.Close()
                MessageBox.Show($"Количество записей с снимке равно 0.{vbCrLf}Вероятно до этого был прерванный снимок.{vbCrLf}Загрузите следующий снимок.", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' чтобы очистить память:
            'ReDim_MeasuredValuesToRange(0, 0)
            Re.Dim(MeasuredValuesToRange, 0, 0)
            WaveformGraphTime.Annotations.Clear()
            WaveformGraphTime.Plots.Clear()

            Dim fi As FileInfo = New FileInfo(pathTextDataStream)

            If Path.GetExtension(fi.Name).ToUpper(CultureInfo.CurrentCulture) = ".TDMS" Then
                Dim mfrmMessageBox As New FormMessageBox("Подождите, идёт загрузка файла", "Загрузка")
                mfrmMessageBox.Show()
                mfrmMessageBox.Activate()
                mfrmMessageBox.Refresh()
                GC.Collect()

                Using readTdmsFileProcessor As New TdmsFileProcessor
                    'Dim waveform As AnalogWaveform(Of Double)() = AnalogWaveform(Of Double).FromArray2D(varTdmsFileProcessor.LoadTDMSFile(pathTextDataStream))
                    MeasuredValues = readTdmsFileProcessor.LoadTDMSFile(pathTextDataStream)
                    isReadFileSnapshotTdms = True
                End Using

                mfrmMessageBox.Close()
            Else
                isReadFileSnapshotTdms = False
                Dim arrLine As String()
                Dim arrWord As String()
                Dim delimiterCrLf As Char() = vbCr.ToCharArray
                Dim delimiterTab As Char() = vbTab.ToCharArray

                Using FS As New FileStream(pathTextDataStream, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Using SR As New StreamReader(FS)
                        arrLine = SR.ReadToEnd.Split(delimiterCrLf)
                    End Using
                End Using

                N = UBound(arrLine) - 1
                'ReDim_arrTransposition(N, I)
                Re.Dim(arrTransposition, N, I)

                For I = 0 To N
                    arrWord = arrLine(I).Split(delimiterTab)
                    For J = 0 To UBound(arrWord)
                        arrTransposition(I, J) = CDbl(arrWord(J))
                    Next
                Next

                ' транспонируем массив физики назад
                MeasuredValues = NationalInstruments.Analysis.Math.LinearAlgebra.Transpose(arrTransposition)
                'ReDim_arrTransposition(0, 0)
                Re.Dim(arrTransposition, 0, 0)
            End If

            'скопированная из процедуры CharacteristicForRegime
            UnpackStringConfiguration(ConfigurationString)
            'ReDim_IndexParameters(0)
            Re.Dim(IndexParameters, 0)
            N = UBound(NamesParameterRegime)

            ' если снимок ограничить количество параметров 
            ' если регистратор и если снимок с частотой 200 гц то
            If RegimeType = cРегистратор AndAlso FrequencyBackgroundSnapshot = FrequencyHandQuery Then
                If N > SnapshotLimit Then N = SnapshotLimit
            End If

            For I = 1 To N
                For J = 1 To UBound(ParametersShaphotType)
                    If ParametersShaphotType(J).NameParameter = NamesParameterRegime(I) Then
                        'ReDimPreserve IndexParameters(UBound(IndexParameters) + 1)
                        Re.DimPreserve(IndexParameters, UBound(IndexParameters) + 1)
                        IndexParameters(UBound(IndexParameters)) = J
                        Exit For
                    End If
                Next
            Next

            If Not IsNothing(IndexParameters) Then
                'ReDim_CopyListOfParameter(IndexParameters.Length - 1)
                Re.Dim(CopyListOfParameter, IndexParameters.Length - 1)
                Array.Copy(IndexParameters, CopyListOfParameter, IndexParameters.Length)
            End If

            'ReDim_PackOfParameters(UBound(IndexParameters) * 3)
            Re.Dim(PackOfParameters, UBound(IndexParameters) * 3)
            ' массив arrIndexParameters содержит перечень параметров по номерам
            ApplyScaleRangeAxisY(ParametersShaphotType)
            IsSnapshot = True
            stepTic = 20
            ConfigureWaveformGraphScale(arraysizeSnapshot, FrequencyBackgroundSnapshot)
            ButtonAddTiks.Visible = True
            ButtonDeleteTiks.Visible = True

            ' очистка списка
            ComboBoxSelectAxis.Items.Clear()
            For I = 1 To UBound(IndexParameters)
                ComboBoxSelectAxis.Items.Add(ParametersShaphotType(IndexParameters(I)).NameParameter)
            Next

            'ReDim_MeasuredValuesToRange(UBound(MeasuredValues), UBound(MeasuredValues, 2))
            Re.Dim(MeasuredValuesToRange, UBound(MeasuredValues), UBound(MeasuredValues, 2))

            If isRegimeChangeForDecoding Then
                TuneVisibilityAndSelectiveList()
                ChangeRegimeForDecoding() ' там очистка графиков и коллекции 
            Else
                RewriteListViewAcquisition(ParametersShaphotType)
                CleaningDiagram(UBound(IndexParameters), ParametersShaphotType)
                Dim parameterNotFound As Boolean
                Dim needExitSub As Boolean ' выйти из процедуры
                For I = 1 To UBound(NamesParameterRegime)
                    parameterNotFound = False
                    For J = 1 To UBound(ParametersShaphotType)
                        If ParametersShaphotType(J).NameParameter = NamesParameterRegime(I) Then
                            parameterNotFound = True
                            Exit For
                        End If
                    Next J

                    If Not parameterNotFound Then
                        needExitSub = True
                        '" номер в базе " & arrПараметрыСнимка(arrIndexParameters(I)).intНомерПараметра.ToString & vbCrLf &
                        Const caption As String = "Ошибка снимка"
                        Dim text As String = $"Параметр {NamesParameterRegime(I)} с которым была произведена запись, отсутствует в базе {PathChannels}{vbCrLf}Скорее всего параметр был переименован."
                        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                    End If
                Next I

                If needExitSub Then
                    cn.Close()
                    Exit Sub
                End If

                TuneDiagramUnderSelectedParameterAxesY()
            End If

            ' здесь загрузка коллекции
            strSQL = "SELECT * FROM [Стрелки] WHERE [KeyID]=" & Str(GKeyID)
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader
            'recordCount = 0

            Do While rdr.Read
                Dim стрелка As New Arrow With {
                    .ViewArrow = DirectCast([Enum].Parse(GetType(ArrowType), Convert.ToString(rdr("ПризнакВидаСтрелки"))), ArrowType),
                    .X1 = CDbl(rdr("X1")),
                    .Y1 = CDbl(rdr("Y1")),
                    .X2 = CDbl(rdr("X2")),
                    .Y2 = CDbl(rdr("Y2"))}

                If Not IsDBNull(rdr("Надпись")) Then
                    стрелка.Legend = CStr(rdr("Надпись"))
                Else
                    стрелка.Legend = String.Empty
                End If

                Arrows.Add(стрелка)
                'recordCount += 1
            Loop

            rdr.Close()
            ' здесь загрузка протокола
            strSQL = "SELECT COUNT(*) FROM [Протокол] WHERE [KeyID]=" & Str(GKeyID)
            cmd.CommandText = strSQL
            recordCount = CInt(cmd.ExecuteScalar)

            If managerAnalysis IsNot Nothing Then
                If recordCount = 0 Then
                    With managerAnalysis(RegimeType)
                        'ReDim_.Protocol(2, 3)
                        Re.Dim(.Protocol, 2, 3)
                        .Protocol(1, 1) = "Контрольный лист №"
                        .Protocol(2, 1) = "Кадр предъявляется"

                        .Protocol(1, 2) = CStr(NumberEngine)
                        .Protocol(2, 2) = "п/заказчика"

                        .Protocol(1, 3) = ""
                        .Protocol(2, 3) = ""
                    End With
                Else
                    With managerAnalysis(RegimeType)
                        'ReDim_.Protocol(recordCount, 3)
                        Re.Dim(.Protocol, recordCount, 3)
                        strSQL = "SELECT * FROM [Протокол] WHERE [KeyID]=" & Str(GKeyID)
                        cmd.CommandText = strSQL
                        rdr = cmd.ExecuteReader
                        recordCount = 1

                        Do While rdr.Read
                            .Protocol(recordCount, 1) = CStr(rdr("Заголовок"))
                            .Protocol(recordCount, 2) = CStr(rdr("Колонка1"))
                            .Protocol(recordCount, 3) = CStr(rdr("Колонка2"))
                            recordCount += 1
                        Loop

                        rdr.Close()
                    End With
                End If
            End If

            ''здесь загрузка сечений
            'strSQL = "SELECT COUNT(*) FROM [Сечения] WHERE [KeyID]=" & Str(GKeyID)
            'cmd.CommandText = strSQL
            'числоЗаписей = CShort(cmd.ExecuteScalar)
            'If числоЗаписей <> 0 Then
            '    ReDim_arrСечения(7, числоЗаписей)
            '    strSQL = "SELECT * FROM [Сечения] WHERE [KeyID]=" & Str(GKeyID)
            '    cmd.CommandText = strSQL
            '    rdr = cmd.ExecuteReader
            '    числоЗаписей = 1
            '    Do While rdr.Read
            '        arrСечения(1, числоЗаписей) = CDbl(rdr("Время"))
            '        arrСечения(2, числоЗаписей) = CDbl(rdr("N1физ"))
            '        arrСечения(3, числоЗаписей) = CDbl(rdr("N1прив"))
            '        arrСечения(4, числоЗаписей) = CDbl(rdr("A1"))
            '        arrСечения(5, числоЗаписей) = CDbl(rdr("N2физ"))
            '        arrСечения(6, числоЗаписей) = CDbl(rdr("N2прив"))
            '        arrСечения(7, числоЗаписей) = CDbl(rdr("A2"))
            '        числоЗаписей += 1
            '    Loop
            '    rdr.Close()
            '    сечениеПостроено = True
            'Else
            '    ОчисткаМассиваСечений()
            'End If

            ' определить по надписи номер режима
            strSQL = $"SELECT * FROM [Режимы{Mid(ChannelShaphot, Len("Channel") + 1, Len("strChannelСнимка"))}] WHERE [Наименование]= '{RegimeType}'"
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader

            If rdr.Read() = True Then
                NumberRegime = CShort(rdr("НомерРежима"))
                HelpString = CStr(rdr("ТекстСправки"))
            End If

            rdr.Close()
            cn.Close()

            RestoreArrows() ' на графике
            ' восстановление текстовых полей
            XAxisTimeRange = New Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
            YAxisTimeRange = New Range(YAxisTime.Range.Minimum, YAxisTime.Range.Maximum)
            RestoreAnnotations()
            UpdateListDescription()
            StatusStripMain.Items(NameOf(LabelSampleRate)).Text = CStr(FrequencyBackgroundSnapshot) & "Гц"

            If isUsePens Then
                TuneAnnotation()
                If GraphModeValue = MyGraphMode.OneCursor Then
                    MoveCursor(XyCursorStart, False)
                ElseIf GraphModeValue = MyGraphMode.TwoCursors Then
                    MoveCursor(XyCursorStart, False)
                    MoveCursor(XyCursorEnd, False)
                End If
            End If
        Catch ex As OutOfMemoryException
            Const caption As String = "Загрузка файла"
            Dim text As String = $"{ex}{vbCrLf}Не хватает памяти.{vbCrLf}Попробуйте конвертировать файл для уменьшения количества параметров или увеличить прореживание."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Catch ex As Exception
            Const caption As String = "Загрузка файла"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Загрузить Каналы Снимка
    ''' </summary>
    Private Sub LoadChannelsSnapshot()
        Dim rowsCount As Integer
        Dim strSQL As String = "SELECT * FROM " & ChannelShaphot
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim aFindValue(0) As Object
        Dim dcDataColumn(1) As DataColumn
        Dim odaDataAdapter As OleDbDataAdapter = New OleDbDataAdapter(strSQL, BuildCnnStr(ProviderJet, PathChannels))

        odaDataAdapter.Fill(dtDataTable)
        rowsCount = dtDataTable.Rows.Count
        dcDataColumn(0) = dtDataTable.Columns("НомерПараметра")
        dtDataTable.PrimaryKey = dcDataColumn

        'ReDim_ParametersShaphotType(rowsCount)
        Re.Dim(ParametersShaphotType, rowsCount)
        ' загрузка коэффициентов по параметрам с базы с помощью запроса
        ' при добавлении полей надо модифицировать запрос в базе
        For I As Integer = 1 To rowsCount
            ParametersShaphotType(I).Initialize()
            aFindValue(0) = I
            drDataRow = dtDataTable.Rows.Find(aFindValue)

            If drDataRow IsNot Nothing Then
                With drDataRow
                    ParametersShaphotType(I).NumberParameter = CShort(.Item("НомерПараметра"))
                    ParametersShaphotType(I).NameParameter = CStr(.Item("НаименованиеПараметра"))
                    ParametersShaphotType(I).NumberChannel = CShort(.Item("НомерКанала"))
                    ParametersShaphotType(I).NumberDevice = CShort(.Item("НомерУстройства"))

                    If Not IsDBNull(.Item("НомерМодуляКорзины")) Then
                        ParametersShaphotType(I).NumberModuleChassis = CShort(.Item("НомерМодуляКорзины"))
                    Else
                        ParametersShaphotType(I).NumberModuleChassis = -1
                    End If

                    If Not IsDBNull(.Item("НомерКаналаМодуля")) Then
                        ParametersShaphotType(I).NumberChannelModule = CShort(.Item("НомерКаналаМодуля"))
                    Else
                        ParametersShaphotType(I).NumberChannelModule = -1
                    End If

                    ParametersShaphotType(I).TypeConnection = CStr(.Item("ТипПодключения"))
                    ParametersShaphotType(I).LowerMeasure = CSng(.Item("НижнийПредел"))
                    ParametersShaphotType(I).UpperMeasure = CSng(.Item("ВерхнийПредел"))
                    ParametersShaphotType(I).SignalType = CStr(.Item("ТипСигнала"))
                    ParametersShaphotType(I).NumberFormula = CShort(.Item("НомерФормулы"))
                    ParametersShaphotType(I).LevelOfApproximation = CShort(.Item("СтепеньАппроксимации"))
                    ParametersShaphotType(I).Coefficient(0) = CDbl(.Item("A0"))
                    ParametersShaphotType(I).Coefficient(1) = CDbl(.Item("A1"))
                    ParametersShaphotType(I).Coefficient(2) = CDbl(.Item("A2"))
                    ParametersShaphotType(I).Coefficient(3) = CDbl(.Item("A3"))
                    ParametersShaphotType(I).Coefficient(4) = CDbl(.Item("A4"))
                    ParametersShaphotType(I).Coefficient(5) = CDbl(.Item("A5"))
                    ParametersShaphotType(I).Offset = CDbl(.Item("Смещение"))
                    ParametersShaphotType(I).UnitOfMeasure = CStr(.Item("ЕдиницаИзмерения"))
                    ParametersShaphotType(I).LowerLimit = CSng(.Item("ДопускМинимум"))
                    ParametersShaphotType(I).UpperLimit = CSng(.Item("ДопускМаксимум"))
                    ParametersShaphotType(I).RangeYmin = CShort(.Item("РазносУмин"))
                    ParametersShaphotType(I).RangeYmax = CShort(.Item("РазносУмакс"))
                    ParametersShaphotType(I).IsVisible = CBool(.Item("Видимость"))
                    If Not IsDBNull(.Item("Примечания")) Then
                        ParametersShaphotType(I).Description = CStr(.Item("Примечания"))
                    Else
                        ParametersShaphotType(I).Description = CStr(0)
                    End If
                End With
            End If
        Next
    End Sub

    ''' <summary>
    ''' Изменение режима для расшифровки
    ''' </summary>
    Protected Sub ChangeRegimeForDecoding()
        If RegimeType = cРегистратор Then
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.SINEWAVE
        Else
            numberParameterAxesChanged = 1 'заглушка
            StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.GRAPH04
        End If

        CaptionGraf = $"Изделие №{NumberProductionSnapshot} Дата:{(Convert.ToDateTime(objDate)).ToShortDateString} {(Convert.ToDateTime(timeStartRecord)).ToShortTimeString} {RegimeType}" ' & " " & strПримечаниеСнимка
        CleaningDiagram(UBound(IndexParameters), ParametersShaphotType)
        TuneDiagramUnderSelectedParameterAxesY()
    End Sub

#Region "Записи и считывания кадров"
    ''' <summary>
    ''' Запись протокола произведённых расшифровок
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Friend Sub MenuWriteDecodingSnapshotToDBase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuWriteDecodingSnapshotToDBase.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuWriteDecodingSnapshotToDBase_Click)}> Запись протокола произведённых расшифровок")
        Dim I As Integer
        SaveSnapshotDecodingToDisk()

        ' здесь запись всех коллекций элементов стрелок
        Dim strSQL As String = "SELECT * FROM [Стрелки]"
        ' до этого каскадно были удалены стрелки, протокол и сечения
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder

        cn.Open()
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        odaDataAdapter.Fill(dtDataTable)

        ' запрос создан как пустой и в него добавляется запись нового поля
        For I = 0 To Arrows.Count - 1
            drDataRow = dtDataTable.NewRow
            drDataRow.BeginEdit()
            drDataRow("KeyID") = GKeyID
            drDataRow("ПризнакВидаСтрелки") = Arrows(I).ViewArrow
            drDataRow("X1") = Arrows(I).X1
            drDataRow("Y1") = Arrows(I).Y1
            drDataRow("X2") = Arrows(I).X2
            drDataRow("Y2") = Arrows(I).Y2
            drDataRow("Надпись") = Arrows(I).Legend
            drDataRow.EndEdit()
            dtDataTable.Rows.Add(drDataRow)
        Next

        cb = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.Update(dtDataTable)

        ' запись таблицы протокола
        strSQL = "SELECT * FROM [Протокол]"
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        dtDataTable = New DataTable
        odaDataAdapter.Fill(dtDataTable)

        With managerAnalysis(RegimeType)
            For I = 1 To UBound(.Protocol)
                drDataRow = dtDataTable.NewRow
                drDataRow.BeginEdit()
                drDataRow("KeyID") = GKeyID
                drDataRow("Заголовок") = .Protocol(I, 1)
                drDataRow("Колонка1") = .Protocol(I, 2)
                drDataRow("Колонка2") = .Protocol(I, 3)
                drDataRow.EndEdit()
                dtDataTable.Rows.Add(drDataRow)
            Next
        End With

        cb = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.Update(dtDataTable)

        'If Not arrСечения Is Nothing Then
        '    'запись таблицы сеченний
        '    strSQL = "SELECT * FROM [Сечения]"
        '    odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        '    dtDataTable = New System.Data.DataTable
        '    odaDataAdapter.Fill(dtDataTable)
        '    For I = 1 To UBound(arrСечения, 2)
        '        drDataRow = dtDataTable.NewRow
        '        drDataRow.BeginEdit()
        '        drDataRow("KeyID") = lngKeyID
        '        drDataRow("Время") = arrСечения(1, I)
        '        drDataRow("N1физ") = arrСечения(2, I)
        '        drDataRow("N1прив") = arrСечения(3, I)
        '        drDataRow("A1") = arrСечения(4, I)
        '        drDataRow("N2физ") = arrСечения(5, I)
        '        drDataRow("N2прив") = arrСечения(6, I)
        '        drDataRow("A2") = arrСечения(7, I)
        '        drDataRow.EndEdit()
        '        dtDataTable.Rows.Add(drDataRow)
        '    Next I
        '    cb = New OleDbCommandBuilder(odaDataAdapter)
        '    odaDataAdapter.Update(dtDataTable)
        'End If

        cn.Close()
    End Sub

    ''' <summary>
    ''' Запись снимка или расшифровки снимка и протокола произведённых расшифровок
    ''' </summary>
    Private Sub SaveSnapshotDecodingToDisk()
        ' здесь запись в базу снимков и текстового файла
        ' сразу после снимка и по меню Записать (после редактирования)
        Dim countRows As Integer ' число записей
        Dim pathTextDataStream As String
        Dim I, J, K, L, indexName, indexParameter As Integer
        Dim pathFile As String = VB.Left(PathChannels, InStrRev(PathChannels, Separator))
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim drDataRow As DataRow
        Dim cb As OleDbCommandBuilder
        Dim descriptionSnapshot As String = String.Empty
        Dim tempNumberEngine As String

        cn.Open()
        ' если нужно создать новый снимок, делаем проверку на его отсутствие
        Dim strSQL As String = "SELECT * FROM [БазаСнимков] WHERE [KeyID] = " & Str(GKeyID)
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        odaDataAdapter.Fill(dtDataTable)
        countRows = dtDataTable.Rows.Count

        Dim cmd As OleDbCommand = cn.CreateCommand

        If countRows <> 0 Then
            ' надо удалить данную запись, а вслед за ней каскадно удаляются записи стрелок
            ' также удалить текстовый файл так как KeyID будет новый
            drDataRow = dtDataTable.Rows(0)
            pathTextDataStream = CType(drDataRow("ПутьНаДиске"), String)
            pathTextDataStream = $"{pathFile}База снимков\{pathTextDataStream.Substring(InStrRev(pathTextDataStream, Separator))}"
            DeleteDataFile(pathTextDataStream)

            If IsUseTdms Then DeleteDataFile(pathTextDataStream & "_index")

            Try
                cmd.CommandType = CommandType.Text
                strSQL = "DELETE FROM БазаСнимков WHERE [KeyID] = " & Str(GKeyID)
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Catch ex As InvalidOperationException
                Const caption As String = "Ошибка при удалении строк"
                MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
                'Finally
                '    cn.Close()
            End Try
            'drDataRow.Delete()
            'cb = New OleDbCommandBuilder(odaDataAdapter)
            'odaDataAdapter.Update(dtDataTable)
        End If

        ' запрос уже создан, он должен быть пустым и в него добавляется запись нового поля
        ' здесь форма для ввода примечания  ПримечаниеСнимка
        descriptionSnapshot = $"Изделие №:{NumberEngine} {Today.ToShortDateString} {Convert.ToDateTime(timeStartRecord).ToShortTimeString} {RegimeType}"
        Dim descriprtionInput As String = InputBox("Введите текст примечания к снимку (не обязательно)", descriptionSnapshot, descriptionSnapshot)
        If descriprtionInput <> "" Then descriptionSnapshot = descriprtionInput

        drDataRow = dtDataTable.NewRow
        drDataRow.BeginEdit()

        ' если запись уже считанного файла то
        If countRows <> 0 Then
            drDataRow("Дата") = objDate ' было удаление значит это редактируемая запись
            drDataRow("Режим") = RegimeType
            drDataRow("НомерИзделия") = NumberProductionSnapshot
            tempNumberEngine = NumberProductionSnapshot.ToString
            drDataRow("Тбокса") = TemperatureBoxInSnaphot
            drDataRow("ТипКРД") = TypeKRDinSnapshot
            drDataRow("ЧастотаОпроса") = FrequencyBackgroundSnapshot
            drDataRow("ТаблицаКаналов") = ChannelShaphot
        Else ' если запись в первый раз то
            drDataRow("Дата") = Today ' новая запись
            drDataRow("Режим") = RegimeType
            drDataRow("НомерИзделия") = NumberEngine
            tempNumberEngine = NumberEngine.ToString
            drDataRow("Тбокса") = TemperatureOfBox
            drDataRow("ТипКРД") = TypeKRD
            drDataRow("ЧастотаОпроса") = FrequencyHandQuery
            drDataRow("ТаблицаКаналов") = ChannelLast
        End If

        drDataRow("ВремяНачалаСбора") = timeStartRecord
        drDataRow("СтрокаКонфигурации") = ConfigurationString
        drDataRow("КолСтрок") = UBound(MeasuredValues, 2) ' кол. строк и стобцов поменялось местами

        If isRegimeChangeForDecoding Then
            drDataRow("КолСтолбцов") = UBound(NamesParameterRegime) - 1
        Else ' снимок
            drDataRow("КолСтолбцов") = UBound(MeasuredValues)
        End If

        drDataRow("НачалоОсиХ") = XAxisTime.Range.Minimum
        drDataRow("КонецОсиХ") = XAxisTime.Range.Maximum
        drDataRow("Примечание") = descriptionSnapshot

        Dim timeString As String = Replace(Trim(Now.ToLongTimeString), ":", "-")

        ' эти примечания корректируются для записи расшифрованных снимков
        If descriptionSnapshot = Nothing Then descriptionSnapshot = $"Снимок {FrequencyHandQuery} Гц"
        descriptionSnapshot = Replace(descriptionSnapshot, ":", "-")
        descriptionSnapshot = Replace(descriptionSnapshot, ">", "-")

        If IsUseTdms Then
            Dim snapshotTdmsFileProcessor As TdmsFileProcessor = New TdmsFileProcessor()

            If isRegimeChangeForDecoding Then
                Dim decodingMeasuredValues(UBound(NamesParameterRegime) - 1, UBound(MeasuredValues, 2)) As Double ' сделать новый массив decodingMeasuredValues
                K = UBound(NamesParameterRegime)
                L = UBound(SnapshotSmallParameters)

                For J = 0 To UBound(MeasuredValues, 2)
                    For indexName = 1 To K
                        For indexParameter = 1 To L
                            If SnapshotSmallParameters(indexParameter).NameParameter = NamesParameterRegime(indexName) Then
                                decodingMeasuredValues(indexName - 1, J) = MeasuredValues(indexParameter - 1, J)
                                Exit For
                            End If
                        Next indexParameter
                    Next indexName
                Next J

                snapshotTdmsFileProcessor.Configure(pathFile, tempNumberEngine, descriptionSnapshot, NamesParameterRegime, SnapshotSmallParameters)
                snapshotTdmsFileProcessor.AppendData(decodingMeasuredValues)
            Else ' снимок последние значения здесь будут нулями
                snapshotTdmsFileProcessor.Configure(pathFile, tempNumberEngine, descriptionSnapshot, IndexParameters, ParametersType)
                snapshotTdmsFileProcessor.AppendData(MeasuredValues)
            End If

            pathTextDataStream = snapshotTdmsFileProcessor.TdmsFileName
            snapshotTdmsFileProcessor.CloseTDMSFile()
            snapshotTdmsFileProcessor = Nothing
        Else
            pathTextDataStream = $"{pathFile}База снимков\{tempNumberEngine}_{Today.ToShortDateString}_{timeString}_{descriptionSnapshot}.txt"
        End If

        drDataRow("ПутьНаДиске") = pathTextDataStream
        drDataRow.EndEdit()

        dtDataTable.Rows.Add(drDataRow)
        cb = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.Update(dtDataTable)

        Dim rdr As OleDbDataReader
        cmd.CommandType = CommandType.Text
        strSQL = $"SELECT KeyID FROM [БазаСнимков] WHERE [ПутьНаДиске] = '{pathTextDataStream}'"
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        If rdr.Read() = True Then
            GKeyID = CInt(rdr("KeyID"))
        End If

        rdr.Close()
        cn.Close()

        If Not IsUseTdms Then
            ' делаем ссылку и открываем поток
            Using FS As New FileStream(pathTextDataStream, FileMode.Create, FileAccess.Write)
                Using SW As New StreamWriter(FS)

                    If dataMeasuredValuesString Is Nothing Then
                        dataMeasuredValuesString = New StringBuilder(1000000)
                    Else
                        dataMeasuredValuesString.Remove(0, dataMeasuredValuesString.Length)
                    End If

                    ' запись
                    If isRegimeChangeForDecoding Then
                        K = UBound(NamesParameterRegime)
                        L = UBound(SnapshotSmallParameters)

                        For J = 0 To UBound(MeasuredValues, 2)
                            For indexName = 1 To K
                                For indexParameter = 1 To L
                                    If SnapshotSmallParameters(indexParameter).NameParameter = NamesParameterRegime(indexName) Then
                                        dataMeasuredValuesString.Append(CStr(Round(MeasuredValues(indexParameter - 1, J), Precision)))

                                        If indexName < K Then dataMeasuredValuesString.Append(vbTab)

                                        Exit For
                                    End If
                                Next indexParameter
                            Next indexName

                            dataMeasuredValuesString.Append(vbCrLf)
                        Next
                    Else ' снимок
                        K = UBound(MeasuredValues)

                        For J = 0 To UBound(MeasuredValues, 2) - 1
                            For I = 0 To K - 1 'параметры
                                dataMeasuredValuesString.Append(CStr(Round(MeasuredValues(I, J), Precision)) & vbTab)
                            Next
                            dataMeasuredValuesString.Append(CStr(Round(MeasuredValues(K, J), Precision)) & vbCrLf)
                        Next

                        ' запись последнего столбца с нулевыми значениями на предпоследний
                        For I = 0 To K - 1
                            dataMeasuredValuesString.Append(CStr(Round(MeasuredValues(I, UBound(MeasuredValues, 2) - 1), Precision)) & vbTab)
                        Next

                        dataMeasuredValuesString.Append(CStr(Round(MeasuredValues(K, UBound(MeasuredValues, 2) - 1), Precision)) & vbCrLf)
                    End If

                    SW.Write(dataMeasuredValuesString.ToString)
                End Using
            End Using
        End If

        If IsLoadFormNavigatorSnapshot = True Then navigatorBackgroundSnapshot.UpdateIndexBackgroundSnapshot(GKeyID)
    End Sub
#End Region

    ''' <summary>
    ''' Восстановить аннотации
    ''' </summary>
    Private Sub RestoreAnnotations()
        If Arrows.Count > 0 Then
            For Each itemArrow As Arrow In Arrows
                ' приведение относительного положения в абсолютное
                ' и зависимости от вида стрелки
                Select Case itemArrow.ViewArrow
                    Case ArrowType.Horizontal
                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        Exit Select
                    Case ArrowType.Vertical
                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X1
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        Exit Select
                    Case ArrowType.Inclined
                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        Exit Select
                End Select

                itemArrow.PointAnnotation = AddAnnotationToArrow(itemArrow.Legend, xData(0), yData(0) + (yData(1) - yData(0)) / 2)
            Next
        End If
    End Sub
    ''' <summary>
    ''' Удалить Стрелки
    ''' </summary>
    Private Sub RemoveArrows()
        For Each itemArrow As Arrow In Arrows
            WaveformGraphTime.Annotations.Remove(itemArrow.PointAnnotation)
            WaveformGraphTime.Plots.Remove(itemArrow.Plot1)

            ' удалить 2 Plot
            If itemArrow.ViewArrow <> ArrowType.Inclined Then
                WaveformGraphTime.Plots.Remove(itemArrow.Plot2)
            End If
        Next
    End Sub
    ''' <summary>
    ''' Восстановить Стрелки
    ''' </summary>
    Private Sub RestoreArrows()
        Dim plot As WaveformPlot

        If Arrows.Count > 0 Then
            For Each itemArrow As Arrow In Arrows
                Select Case itemArrow.ViewArrow
                    Case ArrowType.Horizontal ' горизонтальная
                        plot = GetHorizontalLine()
                        WaveformGraphTime.Plots.Add(plot)

                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        plot.PlotY(yData, xData(0), xData(1) - xData(0))
                        itemArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                        ' вертикальная без стрелки
                        plot = GetVerticalLineWithoutArrow()
                        WaveformGraphTime.Plots.Add(plot)

                        xData(0) = itemArrow.X2
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        plot.PlotY(yData, xData(0), xData(1) - xData(0))
                        itemArrow.Plot2 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                        Exit Select
                    Case ArrowType.Vertical ' вертикальная
                        plot = GetHorizontalLine()
                        WaveformGraphTime.Plots.Add(plot)

                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X1
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        plot.PlotY(yData, xData(0), xData(1) - xData(0))
                        itemArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)

                        ' вертикальная без стрелки
                        plot = GetVerticalLineWithoutArrow()
                        WaveformGraphTime.Plots.Add(plot)
                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        plot.PlotY(yData, xData(0), xData(1) - xData(0))
                        itemArrow.Plot2 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                        Exit Select
                    Case ArrowType.Inclined ' наклонная
                        xData(0) = itemArrow.X1
                        yData(0) = YAxisTime.Range.Minimum + itemArrow.Y1 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)
                        xData(1) = itemArrow.X2
                        yData(1) = YAxisTime.Range.Minimum + itemArrow.Y2 * (YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)

                        plot = GetInclinedLine()
                        WaveformGraphTime.Plots.Add(plot)
                        plot.PlotY(yData, xData(0), xData(1) - xData(0))
                        itemArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                        Exit Select
                End Select
            Next
        End If
    End Sub

    ''' <summary>
    ''' Настроить видимость и выборочный список
    ''' </summary>
    Protected Sub TuneVisibilityAndSelectiveList()
        Dim I, J As Integer

        UnpackStringConfiguration(ConfigurationString)

        For J = 1 To UBound(ParametersShaphotType)
            ParametersShaphotType(J).IsVisible = False

            For I = 1 To UBound(NamesParameterRegime)
                If ParametersShaphotType(J).NameParameter = NamesParameterRegime(I) Then
                    ParametersShaphotType(J).IsVisible = True
                    Exit For
                End If
            Next
        Next

        Dim clearNames As String()
        'ReDim_clearNames(0)
        'ReDim_SnapshotSmallParameters(UBound(IndexParameters))
        Re.Dim(clearNames, 0)
        Re.Dim(SnapshotSmallParameters, UBound(IndexParameters))

        ' заполнить список всех параметров которые снимаются
        For I = 1 To UBound(IndexParameters)
            SnapshotSmallParameters(I).NumberParameter = CShort(IndexParameters(I))
            SnapshotSmallParameters(I).NameParameter = ParametersShaphotType(IndexParameters(I)).NameParameter
            SnapshotSmallParameters(I).UnitOfMeasure = ParametersShaphotType(IndexParameters(I)).UnitOfMeasure
            SnapshotSmallParameters(I).IsVisible = False
        Next

        ' очистить массив NamesParameterRegime от параметров которых может не быть
        For I = 1 To UBound(NamesParameterRegime)
            For J = 1 To UBound(SnapshotSmallParameters)
                If SnapshotSmallParameters(J).NameParameter = NamesParameterRegime(I) Then
                    'ReDimPreserve clearNames(UBound(clearNames) + 1)
                    Re.DimPreserve(clearNames, UBound(clearNames) + 1)
                    clearNames(UBound(clearNames)) = NamesParameterRegime(I)
                    Exit For
                End If
            Next
        Next

        ' отметить, что данный параметр выводится и на каком месте в листе
        For I = 1 To UBound(SnapshotSmallParameters)
            For J = 1 To UBound(clearNames)
                If SnapshotSmallParameters(I).NameParameter = clearNames(J) Then
                    SnapshotSmallParameters(I).IsVisible = True
                    ' номер в листе по положению
                    SnapshotSmallParameters(I).NumberInList = CShort(J)
                    Exit For
                End If
            Next
        Next

        ConfigurationString = vbNullString

        For I = 1 To UBound(clearNames)
            ConfigurationString &= clearNames(I) & "\"
        Next

        If ConfigurationString Is Nothing Then
            ConfigurationString = ParametersShaphotType(IndexParameters(1)).NameParameter & "\"
        End If

        UnpackStringConfiguration(ConfigurationString)
        RewriteListViewAcquisition(ParametersShaphotType)
    End Sub

    ''' <summary>
    ''' Распаковка строки конфигурации в массив имён
    ''' </summary>
    Private Sub UnpackStringConfiguration(ByVal configurationString As String)
        Dim count As Integer = 1
        Dim start As Integer = 1
        Dim lenghtString As Integer = Len(configurationString)

        ' вначале холостой проход для определения числа каналов
        Do
            start = InStr(start, configurationString, Separator) + 1
            count += 1
        Loop While start < lenghtString

        'ReDim_NamesParameterRegime(count - 1)
        Re.Dim(NamesParameterRegime, count - 1)
        start = 1
        count = 1
        Do
            NamesParameterRegime(count) = Mid(configurationString, start, InStr(start, configurationString, Separator) - start)
            start = InStr(start, configurationString, Separator) + 1
            count += 1
        Loop While start < lenghtString
    End Sub

    ''' <summary>
    ''' Открыть проводник по базе
    ''' </summary>
    Private Async Sub OpenExplorerToDBaseAsync()
        'NativeMethods.RegProgramm()
        IsBeforeThatHappenLoadDbase = True
        MenuDecoding.Enabled = True
        LabelIndicator.Visible = True
        IsGroupExportSnapshot = False
        GKeyID = 0
        ' проводник по базе данных снимков
        Dim mFormSelectSnapshotDBase As New FormSelectSnapshotDBase() With {.ParentFormMain = Me}
        mFormSelectSnapshotDBase.ShowDialog() ' обязательно модально

        If GKeyID <> 0 Then ' запрос и загрузка снимка и тегов
            ComboBoxSelectAxis.SelectedIndex = 0 ' по умолчанию первый злемент активный
            LoadFrameSnapshotFromDBase()
            SlidePlot.Visible = True

            If Not IsLoadFormNavigatorSnapshot Then
                navigatorBackgroundSnapshot = New FormNavigatorSnapshot(Me)
                navigatorBackgroundSnapshot.Show()
            End If

            navigatorBackgroundSnapshot.Activate()

            ' по первому считанному снимку узнаем и номер изделия и фоновая запись или снимок 200Гц
            If InStr(1, descriptionSnapshot, "Фоновая запись", CompareMethod.Text) = 1 OrElse InStr(1, descriptionSnapshot, "Объединённая", CompareMethod.Text) = 1 Then
                navigatorBackgroundSnapshot.LoadBackgroundSnapshot(NumberProductionSnapshot, True)
            Else ' скрыть форму
                navigatorBackgroundSnapshot.LoadBackgroundSnapshot(NumberProductionSnapshot, False)
            End If

            ComboBoxSelectAxis.Enabled = True
            ComboBoxSelectAxis.Focus()

            If RegimeType = cРегистратор Then
                StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.SINEWAVE
            Else
                StatusStripMain.Items(NameOf(LabelRegistration)).Image = ProjectResources.GRAPH04
                'ToolStripButtonСкрыть.Image = My.Resources.IndicatorOpen
            End If

            isShowDiagramFromParameter = ButtonGraphParameter.Checked

            If IsGroupExportSnapshot Then Await GroupExportSnapshot()

            If IsUniteDetachedSnapshot Then UniteDetachedSnapshot()
        End If
    End Sub

    ''' <summary>
    ''' Экспорт снимков в Excel
    ''' </summary>
    ''' <returns></returns>
    Private Async Function GroupExportSnapshot() As Tasks.Task
        navigatorBackgroundSnapshot.WindowState = FormWindowState.Minimized
        ProgressBarExport.Maximum = IndexForGroupExport.Length
        ProgressBarExport.Visible = True

        MenuFile.Enabled = False
        Dim mfrmMessageBox As New FormMessageBox("Подождите, идет экспорт данных", "Экспорт в Excel")
        mfrmMessageBox.Show()
        mfrmMessageBox.TopMost = True
        mfrmMessageBox.Activate()
        mfrmMessageBox.Refresh()
        Refresh()

        ' в цикле перебор коллекции и загрузка снимков
        For I As Integer = 0 To UBound(IndexForGroupExport)
            isSavedDecodingToExcel = True
            ProgressBarExport.Value = I + 1
            ShowFrameSnapshotFromDBase(IndexForGroupExport(I))
            PathExcel = Path.Combine(PathExportFolderXLS, Replace(navigatorBackgroundSnapshot.SerialShots.Item(IndexForGroupExport(I)).ToString, ":", "-")) + ".xlsx"
            Await ExportSnapshotToExcelTaskAsync()
        Next

        mfrmMessageBox.Close()
        MenuFile.Enabled = True
        ProgressBarExport.Visible = False
        ' вернуться к первому
        ShowFrameSnapshotFromDBase(IndexForGroupExport(0))
        IsGroupExportSnapshot = False
        navigatorBackgroundSnapshot.WindowState = FormWindowState.Normal
    End Function

    ''' <summary>
    ''' Объединение выделенных снимков в один
    ''' </summary>
    Private Sub UniteDetachedSnapshot()
        Dim pathTextDataStream As String = Nothing
        Dim pathDataFile As String
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))

        navigatorBackgroundSnapshot.WindowState = FormWindowState.Minimized
        ProgressBarExport.Maximum = IndexForMergerSnapshot.Length
        ProgressBarExport.Visible = True

        Try
            cn.Open()
            ' в цикле перебор коллекции и загрузка снимков
            ' считыаем arrИндексыДляСлиянияСнимков(0)  пути с данным по полю ПутьНаДиске таблицы БазаСнимков
            Dim cmd As OleDbCommand = cn.CreateCommand
            Dim rdr As OleDbDataReader
            Dim sizesUnitedArray As Integer = UBound(IndexForMergerSnapshot) ' размерность Массива Слияния
            'Dim fsFrom, fsTo As FileStream
            'Dim srFrom As StreamReader
            'Dim swTo As StreamWriter
            Dim sizeRecords As Integer ' количество строк

            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT КолСтрок, ПутьНаДиске FROM [БазаСнимков] WHERE KeyID = " & CStr(IndexForMergerSnapshot(0))
            rdr = cmd.ExecuteReader

            If rdr.Read() = True Then
                pathTextDataStream = ProduceRightPath(CStr(rdr("ПутьНаДиске")))
                sizeRecords = CInt(rdr("КолСтрок"))
            End If
            rdr.Close()

            If File.Exists(pathTextDataStream) = True Then
                ' делаем ссылку и открываем поток
                Using fsTo As New FileStream(pathTextDataStream, FileMode.Append, FileAccess.Write)
                    Using swTo As New StreamWriter(fsTo)

                        ' в этот файл сливаются все данные с последующих файлов
                        For I As Integer = 1 To sizesUnitedArray
                            ProgressBarExport.Value = I + 1
                            cmd.CommandText = "SELECT КолСтрок, ПутьНаДиске FROM [БазаСнимков] WHERE KeyID = " & CStr(IndexForMergerSnapshot(I))
                            rdr = cmd.ExecuteReader

                            If rdr.Read() = True Then
                                pathDataFile = ProduceRightPath(CStr(rdr("ПутьНаДиске")))
                                sizeRecords += CInt(rdr("КолСтрок"))

                                If File.Exists(pathDataFile) = True Then
                                    Using fsFrom As New FileStream(pathDataFile, FileMode.Open, FileAccess.Read, FileShare.Read)
                                        Using srFrom As New StreamReader(fsFrom)
                                            swTo.Write(srFrom.ReadToEnd)
                                        End Using
                                    End Using
                                End If
                            End If
                            rdr.Close()
                        Next

                        swTo.Flush()
                    End Using
                End Using
            End If

            Dim newDescription As String = $"Объединённая запись из {CStr(sizesUnitedArray + 1)} снимков"
            cmd.CommandText = "Update БазаСнимков " &
                            "SET КолСтрок = " & CStr(sizeRecords + sizesUnitedArray) & " , Примечание = '" & newDescription & "' " &
                            "WHERE KeyID = " & CStr(IndexForMergerSnapshot(0))
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Const caption As String = "Объединение снимков"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        Finally
            cn.Close()
            ProgressBarExport.Visible = False
            ' вернуться к первому
            ShowFrameSnapshotFromDBase(IndexForMergerSnapshot(0))
            IsUniteDetachedSnapshot = False
            navigatorBackgroundSnapshot.WindowState = FormWindowState.Normal
        End Try
    End Sub

#Region "Excel"
    ''' <summary>
    ''' Ввод имени файла Excel
    ''' </summary>
    Private Sub InputNameExcelFile()
        With MainMDIFormParent.SaveFileDialog1
            .FileName = vbNullString

            If isSavedDecodingToExcel Then
                .Title = "Сохранение расшифровки в Excel"
            Else
                .Title = "Экспорт снимка в Excel"
            End If

            .InitialDirectory = "D:\"
            .DefaultExt = ".xlsx"
            ' установить флаг атрибутов
            .Filter = "Книга Excel (*.xlsx)|*.xlsx"
            .RestoreDirectory = True

            If .ShowDialog() = DialogResult.OK AndAlso Len(.FileName) <> 0 Then
                PathExcel = .FileName
            End If
        End With
    End Sub

    ''' <summary>
    ''' Экспорт загруженного кадра в файл формата Excel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuExportSnapshotInExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuExportSnapshotInExcel.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"{NameOf(MenuExportSnapshotInExcel_Click)}> Экспорт загруженного кадра в файл формата Excel")
        PathExcel = vbNullString
        InputNameExcelFile()

        If PathExcel IsNot Nothing Then
            MenuExportSnapshotInExcel.Enabled = False
            ExportSnapshotToExcelAsync(MenuExportSnapshotInExcel)
        End If
    End Sub

    ''' <summary>
    ''' Запись расшифровки кадра в файл формата Excel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MenuWriteDecodingSnapshotToExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuWriteDecodingSnapshotToExcel.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuWriteDecodingSnapshotToExcel_Click)}> Запись расшифровки кадра в файл формата Excel")
        isSavedDecodingToExcel = True
        PathExcel = vbNullString
        InputNameExcelFile()

        If PathExcel Is Nothing Then Exit Sub

        If PathExcel.Length <> 0 Then
            MenuWriteDecodingSnapshotToExcel.Enabled = False
            PrintOrSaveDecodedSnapshotToExcelAsync(MenuWriteDecodingSnapshotToExcel)
        End If
    End Sub

    ''' <summary>
    ''' Печать расшифровки кадра формата Excel
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuPrintDecodingSnapshot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuPrintDecodingSnapshot.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuPrintDecodingSnapshot_Click)}> Печать расшифровки кадра формата Excel")
        MenuPrintDecodingSnapshot.Enabled = False
        PrintOrSaveDecodedSnapshotToExcelAsync(MenuPrintDecodingSnapshot)
    End Sub

    ''' <summary>
    ''' Печать или запись расшифровки кадра формата Excel.
    ''' Подготовка и обертка.
    ''' </summary>
    ''' <param name="inMenuItem"></param>
    Private Async Sub PrintOrSaveDecodedSnapshotToExcelAsync(inMenuItem As ToolStripMenuItem)
        ' запись таблицы сеченний
        ''If blnАИ222 Then'в снимке он False использовать нельзя
        Dim IntNндпрАИ222 As Integer = ComboBoxSelectAxis.Items.IndexOf(conNндпрАИ222)
        If IntNндпрАИ222 <> -1 Then 'АИ222
            SlidePlot.Value = IntNндпрАИ222 + 1
        Else 'повтор
            TuneDiagramUnderSelectedParameterAxesY()
        End If

        Dim mfrmMessageBox As FormMessageBox
        Dim description As String = InputBox("Введите текст примечания к снимку (не обязательно)",
                                             $"Изделие №:{NumberEngine} {Today.ToShortDateString} {Convert.ToDateTime(timeStartRecord).ToShortTimeString} {RegimeType}")
        If description <> "" Then descriptionSnapshot = $"{descriptionSnapshot} {description}"

        If isSavedDecodingToExcel Then
            mfrmMessageBox = New FormMessageBox("Подождите, идет запись", "Запись")
        Else
            mfrmMessageBox = New FormMessageBox
        End If

        mfrmMessageBox.Show()
        mfrmMessageBox.TopMost = True
        mfrmMessageBox.Activate()
        mfrmMessageBox.Refresh()
        Refresh()

        Await PrintOrSaveDecodedSnapshotToExcelAsyncTask()

        mfrmMessageBox.Close()
        inMenuItem.Enabled = True
    End Sub

    ''' <summary>
    ''' Печать или запись расшифровки кадра формата Excel.
    ''' Запуск задачи асинхронно.
    ''' </summary>
    ''' <returns></returns>
    Private Function PrintOrSaveDecodedSnapshotToExcelAsyncTask() As Tasks.Task
        Return Tasks.Task.Run(Sub()
                                  PrintOrSaveDecodedSnapshotToExcelTask()
                              End Sub)
    End Function

    ''' <summary>
    ''' Печать или запись расшифровки кадра формата Excel.
    ''' Задача в собственном потоке.
    ''' </summary>
    Private Sub PrintOrSaveDecodedSnapshotToExcelTask()
        Dim pathTextDataStream As String = PathResourses & "\База снимков\ПередачаExcel.txt"
        ' индексы в массиве по концам
        Dim tempData As Double(,)
        Dim K, I, J, L As Integer
        Dim N, M As Integer
        Dim y As Double(,)
        Dim z As Double(,)
        Dim isMemoryOk As Boolean = False

        GC.Collect()
        'запись в файл приведенных значений в выделенном промежутке для загрузки в Excel
        'делаем ссылку и открываем поток
        Dim startAxisX As Integer = CInt(XAxisTime.Range.Minimum)
        Dim endAxisX As Integer = CInt(XAxisTime.Range.Maximum)
        If startAxisX < 0 Then startAxisX = 0
        If endAxisX > UBound(MeasuredValues, 2) Then endAxisX = UBound(MeasuredValues, 2)
        L = endAxisX - startAxisX + 1

        Try
            ' копируем часть arrСреднееПересчитанный во временный
            ' для режима Регистратор только видимые шлейфы
            If IsRegimeIsRegistrator Then
                'ReDim_tempData(UBound(MeasuredValuesToRange), L - 1)
                Re.Dim(tempData, UBound(MeasuredValuesToRange), L - 1)

                For I = 0 To UBound(MeasuredValuesToRange)
                    K = 0
                    For J = startAxisX To endAxisX
                        tempData(I, K) = MeasuredValuesToRange(I, J)
                        K += 1
                    Next
                Next

                N = UBound(IndexParameters)
                K = 0
                z = NationalInstruments.Analysis.Math.LinearAlgebra.Transpose(tempData)

                'ReDim_y(L - 1, 0)
                Re.Dim(y, L - 1, 0)

                For M = 1 To N
                    If IsBeforeThatHappenLoadDbase Then
                        If ParametersShaphotType(IndexParameters(M)).IsVisible Then
                            'ReDimPreserve y(L - 1, K)
                            Re.DimPreserve(y, L - 1, K)
                            For I = 0 To L - 1
                                y(I, K) = z(I, M - 1)
                            Next
                            K += 1
                        End If
                    Else
                        If ParametersType(IndexParameters(M)).IsVisible Then
                            'ReDimPreserve y(L - 1, K)
                            Re.DimPreserve(y, L - 1, K)
                            For I = 0 To L - 1
                                y(I, K) = z(I, M - 1)
                            Next
                            K += 1
                        End If
                    End If
                Next M
            Else ' нормальный снимок
                'ReDim_tempData(UBound(MeasuredValuesToRange), L - 1)
                Re.Dim(tempData, UBound(MeasuredValuesToRange), L - 1)
                For I = 0 To UBound(MeasuredValuesToRange)
                    K = 0
                    For J = startAxisX To endAxisX
                        tempData(I, K) = MeasuredValuesToRange(I, J)
                        K += 1
                    Next
                Next
                ' транспонируем данные
                y = NationalInstruments.Analysis.Math.LinearAlgebra.Transpose(tempData)
            End If

            ' запись только выделенного диапазона
            Using FS As New FileStream(pathTextDataStream, FileMode.Create, FileAccess.Write)
                Using SW As New StreamWriter(FS)
                    If dataMeasuredValuesString Is Nothing Then
                        dataMeasuredValuesString = New StringBuilder(1000000)
                    Else
                        dataMeasuredValuesString.Remove(0, dataMeasuredValuesString.Length)
                    End If

                    For I = 0 To UBound(y)
                        For J = 0 To UBound(y, 2)
                            dataMeasuredValuesString.Append(Str(Round(y(I, J), 3)))
                            If J <> UBound(y, 2) Then dataMeasuredValuesString.Append(vbTab)
                        Next
                        dataMeasuredValuesString.Append(vbCrLf)
                    Next

                    SW.Write(dataMeasuredValuesString.ToString)
                End Using
            End Using
            isMemoryOk = True
        Catch ex As OutOfMemoryException
            Const caption As String = "Печать или запись расшифрованного снимка"
            Dim text As String = $"{ex}{vbCrLf}Не хватает памяти.{vbCrLf}Попробуйте конвертировать файл для уменьшения количества параметров или увеличить прореживание."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Catch ex As Exception
            Const caption As String = "Печать или запись расшифрованного снимка"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        If isMemoryOk = False Then Exit Sub

        '--- Excel -----------------------------------------------------------
        Dim ProtocolExcel As New Excel.Application
        Try
            ' установить ссылку открыть книгу
            ProtocolExcel.Visible = False 'было False
            ProtocolExcel.Workbooks.Open(Filename:=PathProtocol)
            WorkWithExcelSheet(ProtocolExcel)

            If isSavedDecodingToExcel Then ProtocolExcel.ActiveWorkbook.SaveAs(Filename:=PathExcel, FileFormat:=Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)

            '1. Thread.Sleep(10000) ' без задержки может отвалиться
            ' запрос на изменения не надо
            ProtocolExcel.ActiveWorkbook.Saved = True 'было True
            ' изменения не сохранять
            ProtocolExcel.ActiveWindow.Close(SaveChanges:=False) 'было False
            '2.  Thread.Sleep(1000) ' без задержки может отвалиться
            ProtocolExcel.Quit()
            '3.  Thread.Sleep(1000) ' без задержки может отвалиться
        Catch ex As Exception
            Const caption As String = "Работа с листом Excel"
            Dim text As String = ex.ToString
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        isSavedDecodingToExcel = False
        ProtocolExcel = Nothing
        GC.Collect()
        ' Если вызов GC.Collect() не помогает, попробовать очистку памяти этим способом
        ' Вызываем сборщик мусора для немедленной очистки памяти
        ' GC.GetTotalMemory(True)
        ' Если параметр forceFullCollection равен true, то функция возвращает занимаемую хипом память после вызова сборщика мусора. 
        ' Как известго, GC не гарантирует освобождение абсолютно всей возможной памяти. 
        ' Данная функция вызывает сборщик мусора несколько (до 20) раз до тех пор пока разница в занимаемой памяти до и после сборки не будет составлять 5%.

        'ExcelGlobal_definst = Nothing' sheets
    End Sub

    'Public Async Function WaitAsynchronouslyAsync(inDelay As Integer) As Task(Of String)
    '    Await Tasks.Task.Delay(inDelay).ConfigureAwait(False)
    '    Return "Finished"
    'End Function

    'Dim inDelay As Integer = 1000
    '        Await Tasks.Task.Delay(inDelay)

    '            Await Tasks.Task.Run(Async Function()
    '                                 Await Tasks.Task.Delay(inDelay)
    '                                 Return 123
    '                                 End Function)

    '           или Await WaitAsynchronouslyAsync(inDelay)

    'Sub TestExcel()
    '    Dim xlApp As Microsoft.Office.Interop.Excel.Application
    '    Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
    '    Dim xlSheet As Microsoft.Office.Interop.Excel.Worksheet

    '    xlApp = CType(CreateObject("Excel.Application"), _
    '                Microsoft.Office.Interop.Excel.Application)
    '    xlBook = CType(xlApp.Workbooks.Add, _
    '                Microsoft.Office.Interop.Excel.Workbook)
    '    xlSheet = CType(xlBook.Worksheets(1), _
    '                Microsoft.Office.Interop.Excel.Worksheet)

    '    ' следующее выражение вставляет текст во вторую строку листа
    '    xlSheet.Cells(2, 2) = "This Is column B row 2"
    '    ' вывести лист.
    '    xlSheet.Application.Visible = True
    '    ' сохранить лист в C:\Test.xls директории.
    '    xlSheet.SaveAs("C:\Test.xls")
    '    ' опционно можно xlApp.Quit закрыть книгу.
    'End Sub

    ''' <summary>
    ''' Построение стрелок
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    Private Sub TracingArrows(ByRef refProtocolExcel As Excel.Application)
        ' ось в твипах
        ReadSettingFrameForPrint()

        With refProtocolExcel
            .Sheets("Диаграмма1").Select()
            .ActiveChart.ChartArea.Select()
            .ActiveChart.SetSourceData(Source:= .Sheets("Лист1").Range("A1").CurrentRegion, PlotBy:=Excel.XlRowCol.xlColumns)
            ' фактическая ось записывается на основании мин мах осей графика
            axisXMinimum = Round(XAxisTime.Range.Minimum, 1) / FrequencyBackgroundSnapshot
            axisXMaximum = Round(XAxisTime.Range.Maximum, 1) / FrequencyBackgroundSnapshot
            axisYMinimum = RangesOfDeviation(NumberParameterAxes, 1)
            axisYMaximum = RangesOfDeviation(NumberParameterAxes, 2) ' относительная в 100%

            .Sheets("Диаграмма1").Select()
            ' разметка оси Х
            .ActiveChart.Axes(Excel.XlAxisType.xlCategory).Select()
            With .ActiveChart.Axes(Excel.XlAxisType.xlCategory)
                .MinimumScale = axisXMinimum
                .MaximumScale = axisXMaximum
                .MinorUnit = (axisXMaximum - axisXMinimum) / 20
                .MajorUnit = (axisXMaximum - axisXMinimum) / 20
                .Crosses = Excel.Constants.xlCustom
                .CrossesAt = axisXMinimum
                .ReversePlotOrder = False
                .ScaleType = Excel.XlTrendlineType.xlLinear
                .DisplayUnit = Excel.Constants.xlNone
            End With

            ' разметка оси У пока убрана
            .ActiveChart.Axes(Excel.XlAxisType.xlValue).Select()
            With .ActiveChart.Axes(Excel.XlAxisType.xlValue)
                .MinimumScale = axisYMinimum
                .MaximumScale = axisYMaximum
                .MinorUnit = (axisYMaximum - axisYMinimum) / 20
                .MajorUnit = (axisYMaximum - axisYMinimum) / 20
                .Crosses = Excel.Constants.xlCustom
                .CrossesAt = axisYMinimum
                .ReversePlotOrder = False
                .ScaleType = Excel.XlTrendlineType.xlLinear
                .DisplayUnit = Excel.Constants.xlNone
            End With

            .ActiveChart.PlotArea.Select()
            .Selection.Left = chartLeft
            .Selection.Top = chartTop
            .Selection.Width = chartWidth
            .Selection.Height = chartHeight

            For I As Integer = 0 To Arrows.Count - 1
                xData(0) = Arrows(I).X1 / FrequencyBackgroundSnapshot
                yData(0) = RangesOfDeviation(NumberParameterAxes, 1) + Arrows(I).Y1 * (RangesOfDeviation(NumberParameterAxes, 2) - RangesOfDeviation(NumberParameterAxes, 1))
                xData(1) = Arrows(I).X2 / FrequencyBackgroundSnapshot
                yData(1) = RangesOfDeviation(NumberParameterAxes, 1) + Arrows(I).Y2 * (RangesOfDeviation(NumberParameterAxes, 2) - RangesOfDeviation(NumberParameterAxes, 1))
                TracingLine(xData(0), yData(0), xData(1), yData(1), Arrows(I).Legend, Arrows(I).ViewArrow, refProtocolExcel)
            Next

            'If UBound(arrСечения, 2) > 1 Then ' чтобы не выводить сечение по умолчанию
            '    For I = 1 To UBound(arrСечения, 2) - 1 ' чтобы не выводить последнее сечение
            '        xData(0) = arrСечения(1, I) / частотаФоновогоСнимка
            '        yData(0) = 0
            '        xData(1) = arrСечения(1, I) / частотаФоновогоСнимка
            '        yData(1) = arrРазмахи(номерПараметраОсиЭталона, 2)
            '        ПостроитьЛинию(xData(0), yData(0), xData(1), yData(1), "", 3, refProtocolExcel)
            '    Next I
            'End If
        End With
    End Sub

    ''' <summary>
    ''' Построить линию
    ''' </summary>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <param name="caption"></param>
    ''' <param name="lineType"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub TracingLine(ByVal X1 As Double,
                              ByVal Y1 As Double,
                              ByVal X2 As Double,
                              ByVal Y2 As Double,
                              ByRef caption As String,
                              ByVal lineType As ArrowType,
                              ByRef refProtocolExcel As Excel.Application)

        Dim twipX1 As Single = Convert.ToSingle(frameX1 + (frameX2 - frameX1) * (X1 - axisXMinimum) / (axisXMaximum - axisXMinimum))
        Dim twipX2 As Single = Convert.ToSingle(frameX1 + (frameX2 - frameX1) * (X2 - axisXMinimum) / (axisXMaximum - axisXMinimum))
        Dim twipY1 As Single = Convert.ToSingle(frameY2 + (frameY1 - frameY2) * (axisYMaximum - Y1) / (axisYMaximum - axisYMinimum))
        Dim twipY2 As Single = Convert.ToSingle(frameY2 + (frameY1 - frameY2) * (axisYMaximum - Y2) / (axisYMaximum - axisYMinimum))

        ' при выходе за пределы построения рамки принять ограничения, чтобы не было ошибки Excel
        ' но разметка тогда становится некорректной
        If twipX1 < frameX1 Then twipX1 = frameX1
        If twipY2 > frameY1 Then twipY2 = frameY1

        With refProtocolExcel
            Select Case lineType
                Case ArrowType.Horizontal
                    TracingRedArrow(twipX1, twipY1, twipX2, twipY1, refProtocolExcel)
                    TracingDottedLine(twipX2, twipY1, twipX2, twipY2, refProtocolExcel)
                    .ActiveChart.Shapes.AddLabel(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, (twipX1 + twipX2) / 2, twipY1, 0.0#, 0.0#).Select()
                    Exit Select
                Case ArrowType.Vertical
                    TracingRedArrow(twipX1, twipY1, twipX1, twipY2, refProtocolExcel)
                    TracingDottedLine(twipX1, twipY2, twipX2, twipY2, refProtocolExcel)
                    .ActiveChart.Shapes.AddLabel(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, twipX1, (twipY1 + twipY2) / 2, 0.0#, 0.0#).Select()

                    With .Selection
                        .HorizontalAlignment = Excel.Constants.xlLeft
                        .VerticalAlignment = Excel.Constants.xlTop
                        .Orientation = Excel.XlOrientation.xlUpward
                        .AutoSize = True
                    End With
                    Exit Select
                Case ArrowType.Inclined
                    TracingDottedLine(twipX1, twipY1, twipX2, twipY2, refProtocolExcel)
                    .ActiveChart.Shapes.AddLabel(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, (twipX1 + twipX2) / 2, (twipY1 + twipY2) / 2, 0.0#, 0.0#).Select()
                    Exit Select
            End Select

            ' формат текста
            .Selection.ShapeRange(1).TextFrame.AutoSize = Microsoft.Office.Core.MsoTriState.msoTrue

            If caption = "" Then caption = " "

            .Selection.Characters.Text = caption
            .Selection.AutoScaleFont = False
            With .Selection.Characters(Start:=1, Length:=Len(caption)).Font
                .Size = 8
            End With
        End With
    End Sub
    ''' <summary>
    ''' Включить Ось Y
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    Private Sub EnableAxisY(ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveChart.Axes(Excel.XlAxisType.xlValue).Select()
            .Selection.TickLabels.AutoScaleFont = True

            With .Selection.TickLabels.Font
                .ColorIndex = 1
            End With
        End With
    End Sub
    ''' <summary>
    ''' Выключить Ось Y
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    Private Sub DisableAxisY(ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveChart.Axes(Excel.XlAxisType.xlValue).Select()
            .Selection.TickLabels.AutoScaleFont = True

            With .Selection.TickLabels.Font
                .ColorIndex = 2
            End With
        End With
    End Sub

    ''' <summary>
    ''' Построить красную стрелку
    ''' </summary>
    ''' <param name="twipX1"></param>
    ''' <param name="twipY1"></param>
    ''' <param name="twipX2"></param>
    ''' <param name="twipY2"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub TracingRedArrow(ByVal twipX1 As Single, ByVal twipY1 As Single, ByVal twipX2 As Single, ByVal twipY2 As Single, ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveChart.Shapes.AddLine(twipX1, twipY1, twipX2, twipY2).Select()
            With .Selection.ShapeRange.Line
                .BeginArrowheadStyle = Microsoft.Office.Core.MsoArrowheadStyle.msoArrowheadTriangle
                .BeginArrowheadLength = Microsoft.Office.Core.MsoArrowheadLength.msoArrowheadLengthMedium
                .BeginArrowheadWidth = Microsoft.Office.Core.MsoArrowheadWidth.msoArrowheadWidthMedium
                .EndArrowheadStyle = Microsoft.Office.Core.MsoArrowheadStyle.msoArrowheadTriangle
                .EndArrowheadLength = Microsoft.Office.Core.MsoArrowheadLength.msoArrowheadLengthMedium
                .EndArrowheadWidth = Microsoft.Office.Core.MsoArrowheadWidth.msoArrowheadWidthMedium
                .Weight = 0.75
                .DashStyle = Microsoft.Office.Core.MsoLineDashStyle.msoLineSolid
                .Style = Microsoft.Office.Core.MsoLineStyle.msoLineSingle
                .Transparency = 0.0#
                .Visible = Microsoft.Office.Core.MsoTriState.msoTrue
                .ForeColor.SchemeColor = 10
                .BackColor.RGB = RGB(255, 255, 255)
            End With
        End With
    End Sub
    ''' <summary>
    ''' Построить пунктирную линию
    ''' </summary>
    ''' <param name="twipX1"></param>
    ''' <param name="twipY1"></param>
    ''' <param name="twipX2"></param>
    ''' <param name="twipY2"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub TracingDottedLine(ByVal twipX1 As Single, ByVal twipY1 As Single, ByVal twipX2 As Single, ByVal twipY2 As Single, ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveChart.Shapes.AddLine(twipX1, twipY1, twipX2, twipY2).Select()
            With .Selection.ShapeRange.Line
                .ForeColor.SchemeColor = 10
                .Visible = Microsoft.Office.Core.MsoTriState.msoTrue
                .Weight = 1.0#
                .Style = Microsoft.Office.Core.MsoLineStyle.msoLineSingle
                .DashStyle = Microsoft.Office.Core.MsoLineDashStyle.msoLineDash
            End With
        End With
    End Sub
    ''' <summary>
    ''' Показать стрелку
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub ButtonShowAxes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonShowAxes.Click
        MenuPens.Checked = False
        TracingArrow()
    End Sub
    ''' <summary>
    ''' Показать стрелку
    ''' </summary>
    Private Sub TracingArrow()
        Dim newArrow As Arrow
        Dim plot As WaveformPlot

        XAxisTimeRange = New Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
        YAxisTimeRange = New Range(YAxisTime.Range.Minimum, YAxisTime.Range.Maximum)

        Select Case ComboBoxPointers.SelectedIndex + 1
            Case 1 ' горизонтальная
                plot = GetHorizontalLine()
                WaveformGraphTime.Plots.Add(plot)

                xData(0) = XyCursorStart.XPosition
                yData(0) = XyCursorStart.YPosition
                xData(1) = XyCursorEnd.XPosition
                yData(1) = XyCursorStart.YPosition
                plot.PlotY(yData, xData(0), xData(1) - xData(0))
                newArrow = AddNewArrow()
                newArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)

                ' вертикальная без стрелки
                plot = GetVerticalLineWithoutArrow()
                WaveformGraphTime.Plots.Add(plot)

                xData(0) = XyCursorEnd.XPosition
                yData(0) = XyCursorStart.YPosition
                xData(1) = XyCursorEnd.XPosition
                yData(1) = XyCursorEnd.YPosition
                plot.PlotY(yData, xData(0), xData(1) - xData(0))
                newArrow.Plot2 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                Exit Select
            Case 2 ' вертикальная
                plot = GetHorizontalLine()
                WaveformGraphTime.Plots.Add(plot)

                xData(0) = XyCursorStart.XPosition
                yData(0) = XyCursorStart.YPosition
                xData(1) = XyCursorStart.XPosition
                yData(1) = XyCursorEnd.YPosition
                plot.PlotY(yData, xData(0), xData(1) - xData(0))
                newArrow = AddNewArrow()
                newArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                ' вертикальная без стрелки
                plot = GetVerticalLineWithoutArrow()
                WaveformGraphTime.Plots.Add(plot)

                xData(0) = XyCursorStart.XPosition
                yData(0) = XyCursorEnd.YPosition
                xData(1) = XyCursorEnd.XPosition
                yData(1) = XyCursorEnd.YPosition
                plot.PlotY(yData, xData(0), xData(1) - xData(0))
                newArrow.Plot2 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                Exit Select
            Case 3 ' наклонная
                If ButtonFixLine.Visible = False Then
                    xData(0) = XyCursorStart.XPosition
                    yData(0) = XyCursorStart.YPosition
                    xData(1) = XyCursorEnd.XPosition
                    yData(1) = XyCursorEnd.YPosition

                    plot = GetInclinedLine()
                    WaveformGraphTime.Plots.Add(plot)
                    plot.PlotY(yData, xData(0), xData(1) - xData(0))
                    isSlantingLine = True
                    newArrow = AddNewArrow()
                    newArrow.Plot1 = WaveformGraphTime.Plots(WaveformGraphTime.Plots.Count - 1)
                    ButtonFixLine.Visible = True
                End If
        End Select

        XAxisTime.Range = XAxisTimeRange
        YAxisTime.Range = YAxisTimeRange
    End Sub
    ''' <summary>
    ''' Добавить cтрелку
    ''' </summary>
    ''' <returns></returns>
    Private Function AddNewArrow() As Arrow
        Dim newArrow As New Arrow

        ApplyDifferentialCoordinates(newArrow)
        newArrow.Legend = TextBoxDescriptionOnAxes.Text
        newArrow.ViewArrow = CType(ComboBoxPointers.SelectedIndex + 1, ArrowType)
        newArrow.PointAnnotation = AddAnnotationToArrow(newArrow.Legend, xData(0), yData(0))

        Arrows.Add(newArrow)
        UpdateListDescription()

        Return newArrow
    End Function
    ''' <summary>
    ''' Рисовать стрелку расшифровки
    ''' </summary>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <param name="lineType"></param>
    ''' <param name="caption"></param>
    Friend Sub TracingDecodingArrow(ByVal X1 As Double,
                                          ByVal Y1 As Double,
                                          ByVal X2 As Double,
                                          ByVal Y2 As Double,
                                          ByVal lineType As ArrowType,
                                          ByVal caption As String)

        XyCursorStart.MoveCursor(X1 * FrequencyBackgroundSnapshot, Y1)
        XyCursorEnd.MoveCursor(X2 * FrequencyBackgroundSnapshot, Y2)
        ComboBoxPointers.SelectedIndex = CInt(lineType) - 1
        TextBoxDescriptionOnAxes.Text = caption
        TracingArrow()

        Select Case lineType
            'Case 0 'горизонтальная
            'Case 1 'вертикальная
            Case ArrowType.Inclined 'произвольная
                isSlantingLine = False
                ButtonFixLine.Visible = False
        End Select
    End Sub

    ''' <summary>
    ''' Горизонтальная линия
    ''' </summary>
    ''' <returns></returns>
    Private Function GetHorizontalLine() As WaveformPlot
        Return New WaveformPlot() With {.LineColor = Color.White,
                                        .LineColorPrecedence = ColorPrecedence.UserDefinedColor,
                                        .PointColor = Color.Red,
                                        .PointStyle = PointStyle.SolidDiamond,
                                        .XAxis = XAxisTime,
                                        .YAxis = YAxisTime}
    End Function

    ''' <summary>
    ''' Вертикальная линия без стрелки 
    ''' </summary>
    ''' <returns></returns>
    Private Function GetVerticalLineWithoutArrow() As WaveformPlot
        'plot.PointStyle = PointStyle.SolidDiamond,EmptyTriangleLeft,SolidTriangleRight
        Return New WaveformPlot() With {.LineColor = Color.White,
                                        .LineColorPrecedence = ColorPrecedence.UserDefinedColor,
                                        .PointColor = Color.Red,
                                        .LineStyle = LineStyle.Dot,
                                        .XAxis = XAxisTime,
                                        .YAxis = YAxisTime}
    End Function

    'Private Function СечениеЛиния() As WaveformPlot
    '    Return New WaveformPlot() With {.LineColor = Color.White,
    '                                    .LineColorPrecedence = ColorPrecedence.UserDefinedColor,
    '                                    .LineStyle = LineStyle.DashDot,
    '                                    .XAxis = XAxisTime,
    '                                    .YAxis = YAxisTime}
    'End Function

    ''' <summary>
    ''' Работа на листе Excel
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    Private Sub WorkWithExcelSheet(ByRef refProtocolExcel As Excel.Application)
        Dim myDocument As Object ' не работает Excel.Sheets
        Dim I, J As Integer
        Dim time As Double
        Dim headline As String ' колонтитул
        Dim pathTextDataStream As String = PathResourses & "\База снимков\ПередачаExcel.txt"
        Dim startAxisX, endAxisX As Integer
        Dim lengthArray As Integer
        Dim column As Integer

        offsetRow = 0
        With refProtocolExcel
            myDocument = .Sheets("Диаграмма1")
            ' очистка всех листов
            .Worksheets("Лист1").Activate()
            .Cells.Select()
            .Selection.Clear()

            '**********************************************************
            ' считывается и вставляется на лист из файла
            ' вставка в существующий лист
            .Range("B2").Select()
            With .ActiveSheet.QueryTables.Add(Connection:="TEXT;" & pathTextDataStream, Destination:= .Range("B2"))
                .FieldNames = True
                .RowNumbers = False
                .FillAdjacentFormulas = False
                .PreserveFormatting = True
                .RefreshOnFileOpen = False
                .RefreshStyle = Excel.XlCellInsertionMode.xlInsertDeleteCells
                .SavePassword = False
                .SaveData = True
                .AdjustColumnWidth = True
                .RefreshPeriod = 0
                .TextFilePromptOnRefresh = False
                .TextFilePlatform = Excel.XlPlatform.xlWindows
                .TextFileStartRow = 1
                .TextFileParseType = Excel.XlTextParsingType.xlDelimited
                .TextFileTextQualifier = Excel.XlTextQualifier.xlTextQualifierDoubleQuote
                .TextFileConsecutiveDelimiter = False
                .TextFileTabDelimiter = True
                .TextFileSemicolonDelimiter = False
                .TextFileCommaDelimiter = False
                .TextFileSpaceDelimiter = False
                '.TextFileColumnDataTypes = Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
                .TextFileDecimalSeparator = "."
                .Refresh(BackgroundQuery:=False)
            End With

            '**********************************************************
            ' расшифровка стоки конфигурации и заполнение 1 строки листа
            ' ограничить длину заголовков столбцов 10 символами
            .Worksheets("Лист1").Cells(1, 1).Value = "Время"
            lengthArray = UBound(NamesParameterRegime)

            If IsRegimeIsRegistrator Then
                column = 2
                If IsBeforeThatHappenLoadDbase Then
                    ' поиск по имени и если данный параметр видим, то вписать его
                    For J = 1 To UBound(ParametersShaphotType)
                        For I = 1 To lengthArray
                            If ParametersShaphotType(J).NameParameter = NamesParameterRegime(I) AndAlso ParametersShaphotType(J).IsVisible Then
                                .Worksheets("Лист1").Cells(1, column).Value = Mid(NamesParameterRegime(I), 1, 10)
                                column += 1
                                Exit For
                            End If
                        Next I

                        If column = lengthArray + 2 Then Exit For
                    Next J
                Else
                    ' поиск по имени и если данный параметр видим, то вписать его
                    For J = 1 To UBound(ParametersType)
                        For I = 1 To lengthArray
                            If ParametersType(J).NameParameter = NamesParameterRegime(I) AndAlso ParametersType(J).IsVisible Then
                                .Worksheets("Лист1").Cells(1, column).Value = Mid(NamesParameterRegime(I), 1, 10)
                                column += 1
                                Exit For
                            End If
                        Next I

                        If column = lengthArray + 2 Then Exit For
                    Next J
                End If

                column -= 2
            Else ' осциллографический снимок
                For I = 1 To UBound(NamesParameterRegime)
                    .Worksheets("Лист1").Cells(1, I + 1).Value = Mid(NamesParameterRegime(I), 1, 10)
                Next
                column = UBound(NamesParameterRegime)
            End If

            ' определить по временному диапазону индексы в массивах
            time = XAxisTime.Range.Minimum / FrequencyBackgroundSnapshot
            startAxisX = CInt(XAxisTime.Range.Minimum)
            endAxisX = CInt(XAxisTime.Range.Maximum)
            lengthArray = (endAxisX - startAxisX + 1)
            ' переход на ячейку А1 и с нее по 1 столбцу пойдет разметка времени
            .Range("A1").Select()

            With .Worksheets("Лист1")
                For I = 0 To lengthArray - 1
                    .Cells(I + 2, 1).Value = time
                    time += 1 / FrequencyBackgroundSnapshot
                Next I
            End With

            .Range("A1").Select()

            ' удалить стрелки и надписи
            Do Until myDocument.Shapes.Count = 1
                myDocument.Shapes(myDocument.Shapes.Count).Delete()
            Loop

            ' переопределяется область построения диаграммы
            ' в ПостроениеСтрелок настраиваются оси
            TracingArrows(refProtocolExcel)
            ' в цикле по коллекции заносятся стрелки
            ' подпись под диаграммой
            headline = $"Изделие №:{NumberProductionSnapshot} Дата:{(Convert.ToDateTime(objDate)).ToShortDateString} Время:{(Convert.ToDateTime(timeStartRecord)).ToShortTimeString} Режим: {RegimeType} {descriptionSnapshot}"
            SetCaption(headline, refProtocolExcel)
            ' переносится сечения и протокол
            .Worksheets("Лист2").Activate()
            .Cells.Select()
            .Selection.Clear()
            Me.SetHeadline(headline, refProtocolExcel)
            .Range("A1").Select()

            'If сечениеПостроено Then
            '    ЗаголовокСечений(refProtocolExcel)
            '    With .Worksheets("Лист2")
            '        'вывод массива сечений
            '        For I = 1 To UBound(arrСечения, 2)
            '            .Cells(I + 1, 1).Value = arrСечения(1, I) / частотаФоновогоСнимка
            '            For J = 2 To 7
            '                .Cells(I + 1, J).Value = arrСечения(J, I)
            '            Next J
            '        Next I
            '    End With
            '    offsetRow = UBound(arrСечения, 2) + 2
            '    РамкаДляТаблицы("A1", refProtocolExcel)
            'End If

            ' протокол
            With .Worksheets("Лист2")
                ' вывод массива протокола
                For I = 1 To UBound(managerAnalysis(RegimeType).Protocol)
                    .Cells(I + offsetRow, 1).Value = managerAnalysis(RegimeType).Protocol(I, 1)
                    refProtocolExcel.Range($"A{I + offsetRow}:D{I + offsetRow}").Select()
                    refProtocolExcel.Selection.Merge()

                    With refProtocolExcel.Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .MergeCells = True
                    End With

                    .Cells(I + offsetRow, 5).Value = managerAnalysis(RegimeType).Protocol(I, 2)
                    refProtocolExcel.Range($"E{I + offsetRow}:F{I + offsetRow}").Select()
                    refProtocolExcel.Selection.Merge()

                    With refProtocolExcel.Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .MergeCells = True
                        '.NumberFormat = "0.00" 'Excel.XlRangeAutoFormat.'xlFixedValue '"0.00"
                    End With

                    '.Range("E28:F28").Select()
                    'Selection.NumberFormat = "0.00"
                    '.Cells(I + intСмещение, 5). = Protocol(I, 2)
                    'refProtocolExcel.Selection.NumberFormat = "0.00"

                    .Cells(I + offsetRow, 7).Value = managerAnalysis(RegimeType).Protocol(I, 3)
                    refProtocolExcel.Range($"G{I + offsetRow}:H{I + offsetRow}").Select()
                    refProtocolExcel.Selection.Merge()

                    With refProtocolExcel.Selection
                        .HorizontalAlignment = Excel.Constants.xlCenter
                        .VerticalAlignment = Excel.Constants.xlCenter
                        .MergeCells = True
                    End With
                Next I
            End With

            SetFrameForTable($"A{I + offsetRow}", refProtocolExcel)
            offsetRow = offsetRow + CShort(UBound(managerAnalysis(RegimeType).Protocol)) + 3S
            ' подписи под протоколом
            SetDescriptionOTK(refProtocolExcel)
            'ТаблицаПараметров(refProtocolExcel)
            '4. Thread.Sleep(1000)
            Me.SetHeadline(headline, refProtocolExcel)
            ' коррекция колонтитула
            'With ExcelGlobal_definst.ActiveSheet.PageSetup'Sheets
            With .ActiveSheet.PageSetup
                .LeftMargin = .Application.InchesToPoints(0.590551181102362)
                .RightMargin = .Application.InchesToPoints(0.590551181102362)
                .TopMargin = .Application.InchesToPoints(0.984251968503937)
                .BottomMargin = .Application.InchesToPoints(0.984251968503937)
                .HeaderMargin = .Application.InchesToPoints(0.511811023622047)
                .FooterMargin = .Application.InchesToPoints(0.511811023622047)
                .Orientation = Excel.XlPageOrientation.xlLandscape
            End With

            ' шлейфы пожирнее первые 3 шлейфа
            ' ExcelGlobal_definst.ActiveWorkbook.Sheets("Диаграмма1").Select()'Sheets
            '5.  Thread.Sleep(1000) ' без задержки может отвалиться
            .ActiveWorkbook.Sheets("Диаграмма1").Select()
            '6.  Thread.Sleep(10000) ' без задержки может отвалиться

            With .ActiveChart
                For I = 1 To column
                    .SeriesCollection(I).Select()
                    'Thread.Sleep(1000) ' без задержки может отвалиться
                    If I = 1 Then refProtocolExcel.Selection.Border.ColorIndex = 4

                    If I <= 3 Then
                        refProtocolExcel.Selection.Border.Weight = Excel.XlBorderWeight.xlMedium 'xlThick 
                    Else
                        refProtocolExcel.Selection.Border.Weight = Excel.XlBorderWeight.xlThin
                    End If
                    'If I = 3 Then Exit For
                Next
            End With

            ' печать
            If isSavedDecodingToExcel = False Then PrintSheetExcel(refProtocolExcel, column)
        End With
        myDocument = Nothing
    End Sub

    'Private Sub ТаблицаПараметров(ByRef refProtocolExcel As Excel.Application)
    '    Dim I, J As Integer 'K 
    '    Dim colIndex, rwIndex, intЧислоСтрок As Integer
    '    Dim tempОсьX As Double

    '    With refProtocolExcel
    '        .Worksheets("Лист3").Activate()
    '        rwIndex = 2
    '        tempОсьX = осьX1

    '        'intЧислоСтрок = UBound(arrСечения, 2)
    '        'If intЧислоСтрок > 21 Then intЧислоСтрок = 21

    '        'For colIndex = 1 To intЧислоСтрок
    '        '    .Worksheets("Лист3").Cells(rwIndex, colIndex + 1).Value = arrСечения(1, colIndex) / частотаФоновогоСнимка
    '        'Next colIndex

    '        rwIndex = 3
    '        If РежимРегистратор Then
    '            For I = 1 To UBound(arrНаименование2)
    '                If ПередЭтимБылаЗагрузкаБазы Then
    '                    'поиск по имени и если данный параметр видим то вписать его
    '                    For J = 1 To UBound(arrIndexParameters)
    '                        If ParametersShaphotType(arrIndexParameters(J)).NameParameter = arrНаименование2(I) AndAlso ParametersShaphotType(arrIndexParameters(J)).IsVisible Then
    '                            ПечатьСтрокиТаблицыПротокола(arrНаименование2(I), J, rwIndex, refProtocolExcel)
    '                            Exit For
    '                        End If
    '                    Next J
    '                Else
    '                    'поиск по имени и если данный параметр видим то вписать его
    '                    For J = 1 To UBound(arrIndexParameters)
    '                        If ParametersType(arrIndexParameters(J)).NameParameter = arrНаименование2(I) AndAlso ParametersType(arrIndexParameters(J)).IsVisible Then
    '                            ПечатьСтрокиТаблицыПротокола(arrНаименование2(I), J, rwIndex, refProtocolExcel)
    '                            Exit For
    '                        End If
    '                    Next J
    '                End If
    '            Next I
    '        Else 'осциллографический снимок
    '            For I = 1 To UBound(arrНаименование2)
    '                If ПередЭтимБылаЗагрузкаБазы Then
    '                    For J = 1 To UBound(ParametersShaphotType)
    '                        If ParametersShaphotType(J).NameParameter = arrНаименование2(I) Then
    '                            ПечатьСтрокиТаблицыПротокола(arrНаименование2(I), J, rwIndex, refProtocolExcel)
    '                            Exit For
    '                        End If
    '                    Next J
    '                Else
    '                    For J = 1 To UBound(ParametersType)
    '                        If ParametersType(J).NameParameter = arrНаименование2(I) Then
    '                            ПечатьСтрокиТаблицыПротокола(arrНаименование2(I), J, rwIndex, refProtocolExcel)
    '                            Exit For
    '                        End If
    '                    Next J
    '                End If
    '            Next I
    '        End If
    '        'РамкаДляТаблицы("A1", refProtocolExcel)
    '    End With
    'End Sub

    'Private Sub ПечатьСтрокиТаблицыПротокола(ByVal strНаименование As String, ByVal I As Integer, ByRef rwIndex As Integer, ByRef refProtocolExcel As Excel.Application)
    '    Dim intИНдекс, intЧислоСтолбцов As Integer
    '    Dim colIndex As Integer = 1

    '    intЧислоСтолбцов = UBound(arrСечения, 2)
    '    If intЧислоСтолбцов > 21 Then intЧислоСтолбцов = 21
    '    With refProtocolExcel
    '        .Worksheets("Лист3").Cells(rwIndex, colIndex).Value = strНаименование
    '        For colIndex = 1 To intЧислоСтолбцов
    '            intИНдекс = arrСечения(1, colIndex)
    '            If intИНдекс <> 0 Then .Worksheets("Лист3").Cells(rwIndex, colIndex + 1).Value = arrСреднее(I - 1, intИНдекс)
    '        Next colIndex
    '        rwIndex += 1
    '    End With
    'End Sub

    'Private Sub ЗаголовокСечений(ByRef refProtocolExcel As Excel.Application)
    '    With refProtocolExcel
    '        .Range("A1").Select()
    '        .ActiveCell.FormulaR1C1 = "Время"
    '        .Range("B1").Select()
    '        .ActiveCell.FormulaR1C1 = "N1 физ."
    '        .Range("C1").Select()
    '        .ActiveCell.FormulaR1C1 = "N1 прив."
    '        .Range("D1").Select()
    '        .ActiveCell.FormulaR1C1 = cona1
    '        .Range("E1").Select()
    '        .ActiveCell.FormulaR1C1 = "N2 физ."
    '        .Range("F1").Select()
    '        .ActiveCell.FormulaR1C1 = "N2 прив."
    '        .Range("G1").Select()
    '        .ActiveCell.FormulaR1C1 = "a2"
    '        .Range("A1: G1").Select()
    '        With .Selection
    '            .HorizontalAlignment = Excel.Constants.xlCenter
    '            .VerticalAlignment = Excel.Constants.xlCenter
    '            .WrapText = False
    '            .Orientation = 0
    '            .AddIndent = False
    '            .ShrinkToFit = False
    '            .MergeCells = False
    '        End With
    '        With .Selection.Font
    '            .Name = "Arial Cyr"
    '            .FontStyle = "полужирный"
    '            .Size = 10
    '            .Strikethrough = False
    '            .Superscript = False
    '            .Subscript = False
    '            .OutlineFont = False
    '            .Shadow = False
    '            .Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
    '        .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        .Range("A1:G1").Select()
    '        .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
    '        .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlMedium
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlMedium
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlMedium
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlMedium
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
    '            .LineStyle = Excel.XlLineStyle.xlContinuous
    '            .Weight = Excel.XlBorderWeight.xlThin
    '            .ColorIndex = Excel.Constants.xlAutomatic
    '        End With
    '        .Range("A2").Select()
    '    End With
    'End Sub
    ''' <summary>
    ''' Рамка для таблицы
    ''' </summary>
    ''' <param name="rangeStart"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub SetFrameForTable(ByRef rangeStart As String, ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveSheet.Range(rangeStart).CurrentRegion.Select()
            'Range("A1:G3").Select
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlThin
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Range("A1").Select()
        End With
    End Sub
    ''' <summary>
    ''' Колонтитул
    ''' </summary>
    ''' <param name="headline"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub SetHeadline(ByRef headline As String, ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            With .ActiveSheet.PageSetup
                .PrintTitleRows = vbNullString
                .PrintTitleColumns = vbNullString
            End With
            .ActiveSheet.PageSetup.PrintArea = vbNullString
            With .ActiveSheet.PageSetup
                .CenterHeader = headline
                .CenterFooter = "&8НПО ""Салют"""
                .RightFooter = "&8страница №&P"
                .LeftMargin = refProtocolExcel.Application.InchesToPoints(0.787401575)
                .RightMargin = refProtocolExcel.Application.InchesToPoints(0.787401575)
                .TopMargin = refProtocolExcel.Application.InchesToPoints(0.984251969)
                .BottomMargin = refProtocolExcel.Application.InchesToPoints(0.984251969)
                .HeaderMargin = refProtocolExcel.Application.InchesToPoints(0.5)
                .FooterMargin = refProtocolExcel.Application.InchesToPoints(0.5)

                .PrintHeadings = False
                .PrintGridLines = False
                .PrintComments = Excel.XlPrintLocation.xlPrintNoComments
                '.PrintQuality = Array(240, 144)
                .CenterHorizontally = False
                .CenterVertically = False
                .Orientation = Excel.XlPageOrientation.xlPortrait
                .Draft = False
                .PaperSize = Excel.XlPaperSize.xlPaperA4
                .FirstPageNumber = Excel.Constants.xlAutomatic
                .Order = Excel.XlOrder.xlDownThenOver
                .BlackAndWhite = False
                .Zoom = 100
            End With
        End With
    End Sub
    ''' <summary>
    ''' Подписи ОТК
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    Private Sub SetDescriptionOTK(ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .Range("E" & offsetRow & ":I" & offsetRow + 5).Select()
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.Constants.xlNone
            .ActiveCell.FormulaR1C1 = "Примечания:"
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
                .Weight = Excel.XlBorderWeight.xlMedium
                .ColorIndex = Excel.Constants.xlAutomatic
            End With
            .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.Constants.xlNone
            .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.Constants.xlNone
            .Selection.Merge()
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlCenter
                .VerticalAlignment = Excel.Constants.xlTop
                .WrapText = False
                .Orientation = 0
                .AddIndent = False
                .ShrinkToFit = False
                .MergeCells = True
            End With
            .Range("A" & offsetRow).Select()
            .ActiveCell.FormulaR1C1 = "Расшифровал:_____________"
            .Range("A" & offsetRow + 2).Select()
            .ActiveCell.FormulaR1C1 = "Кадр проверил ОТК:________"
            .Range("A" & offsetRow + 4).Select()
            .ActiveCell.FormulaR1C1 = "Кадр принял п/з:___________"
        End With
    End Sub

    ''' <summary>
    ''' Подпись
    ''' </summary>
    ''' <param name="подпись"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub SetCaption(ByRef подпись As String, ByRef refProtocolExcel As Excel.Application)
        With refProtocolExcel
            .ActiveChart.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, 184.05, 455.2, 422.51, 40.8).Select()
            .Selection.Characters.Text = подпись
            .Selection.AutoScaleFont = False
            With .Selection.Characters(Start:=1, Length:=15).Font
                .Name = "Arial Cyr"
                .FontStyle = "полужирный"
                .Size = 10
            End With
        End With
    End Sub

    ''' <summary>
    ''' Считать настройки рамки для печати из базы
    ''' </summary>
    Private Sub ReadSettingFrameForPrint()
        Dim strSQL As String = $"SELECT * FROM [РамкаДляПечати] WHERE ([Стенд]={StandNumber})"
        Dim cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        Dim rdr As OleDbDataReader = Nothing
        Dim cmd As OleDbCommand = cn.CreateCommand

        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        cn.Open()

        Try
            rdr = cmd.ExecuteReader

            If rdr.Read() = True Then
                frameX1 = CSng(rdr("РамкаX1"))
                frameX2 = CSng(rdr("РамкаX2"))
                frameY1 = CSng(rdr("РамкаY1"))
                frameY2 = CSng(rdr("РамкаY2"))
                chartLeft = CSng(rdr("РамкаLeft"))
                chartTop = CSng(rdr("РамкаTop"))
                chartWidth = CSng(rdr("РамкаWidth"))
                chartHeight = CSng(rdr("РамкаHeight"))
            End If
        Catch ex As Exception
            Const caption As String = "Считывание параметров рамки для графика"
            Dim text As String = ex.ToString
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            rdr.Close()
            cn.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Печать листов Excel
    ''' </summary>
    ''' <param name="refProtocolExcel"></param>
    ''' <param name="column"></param>
    Private Sub PrintSheetExcel(ByRef refProtocolExcel As Excel.Application, ByVal column As Integer)
        Dim printerName As String
        Dim dlg As New PrintDialog
        Dim pd As PrintDocument = New PrintDocument

        dlg.Document = pd

        If dlg.ShowDialog() = DialogResult.OK Then
            printerName = dlg.PrinterSettings.PrinterName ' "\\PENTIUM4\HP DeskJet 1220C (Ne01:)"

            If dlg.PrinterSettings.IsValid Then
                Try
                    With refProtocolExcel
                        .Sheets("Диаграмма1").Select()
                        .ActiveWindow.SelectedSheets.PrintOut(Copies:=1, ActivePrinter:=
                        printerName, Collate:=True)
                        Application.DoEvents()
                        '8. Thread.Sleep(30000)
                        Application.DoEvents()

                        If Not (RegimeType = cРегистратор OrElse RegimeType = cОтладочныйРежим) Then
                            If MessageBox.Show("Нужна печать протокола?", "Печать", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                                Exit Sub
                            Else
                                .Worksheets("Лист2").Activate()
                                .Sheets("Лист2").Select()
                                .ActiveWindow.SelectedSheets.PrintOut(Copies:=1, ActivePrinter:=
                                printerName, Collate:=True)
                                Application.DoEvents()
                                '9.  Thread.Sleep(10000)
                                Application.DoEvents()

                                'If MessageBox.Show("Нужна печать сечений?", "Печать", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                                '    Exit Sub
                                'Else
                                '    .Worksheets("Лист3").Activate()
                                '    .Sheets("Лист3").Select()
                                '    .Range("A1:V" & CStr(column + 2)).Select()
                                '    .ActiveWindow.SelectedSheets.PrintOut(Copies:=1, ActivePrinter:=
                                '    printerName, Collate:=True)
                                '    Thread.Sleep(10000)
                                '    Application.DoEvents()
                                'End If
                            End If
                        End If
                    End With
                Catch ex As Exception
                    Const caption As String = "Печать"
                    Dim text As String = ex.ToString
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                    If MessageBox.Show("Повторить?", "Печать", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                        Exit Sub
                    Else
                        PrintSheetExcel(refProtocolExcel, column)
                    End If
                End Try
            Else
                MessageBox.Show("Принтер не установлен.", "Печать", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Экспорт загруженного кадра в файл формата Excel.
    ''' Подготовка и обертка.
    ''' </summary>
    ''' <param name="inMenuItem"></param>
    Private Async Sub ExportSnapshotToExcelAsync(inMenuItem As ToolStripMenuItem)
        Dim mfrmMessageBox As New FormMessageBox("Подождите, идет экспорт данных", "Экспорт в Excel")
        mfrmMessageBox.Show()
        mfrmMessageBox.TopMost = True
        mfrmMessageBox.Activate()
        mfrmMessageBox.Refresh()
        Refresh()

        Await ExportSnapshotToExcelTaskAsync()
        mfrmMessageBox.Close()
        inMenuItem.Enabled = True
    End Sub

    ''' <summary>
    ''' Экспорт загруженного кадра в файл формата Excel.
    ''' Запуск задачи асинхронно.
    ''' </summary>
    ''' <returns></returns>
    Private Function ExportSnapshotToExcelTaskAsync() As Tasks.Task
        Return Tasks.Task.Run(Sub()
                                  ExportSnapshotToExcelTask()
                              End Sub)
    End Function

    ''' <summary>
    ''' Экспорт загруженного кадра в файл формата Excel.
    ''' Задача в собственном потоке.
    ''' </summary>
    Private Sub ExportSnapshotToExcelTask()
        Dim pathTextDataStream As String = PathResourses & "\База снимков\ЭкспортExcel.txt"
        Dim startAxisX, endAxisX As Integer ' индексы в массиве по концам
        Dim K, I, J, L As Integer
        Dim N, M As Integer
        Dim tempData As Double(,)
        Dim y(0, 0) As Double
        Dim transposeData As Double(,)
        Dim isParameterAdd As Boolean ' добавить этот параметр
        Dim memoryOk As Boolean = False
        Dim isSnapshotRegistration As Boolean ' снимок Регистратор
        Dim name As String = Nothing

        GC.Collect()
        '--- начало копирования в текстовый поток -----------------------------
        ' запись в файл приведенных значений в выделенном промежутке для загрузки в Excel
        ' делаем ссылку и открываем поток
        Try
            Using FS As New FileStream(pathTextDataStream, FileMode.Create, FileAccess.Write)
                Using SW As New StreamWriter(FS, New UnicodeEncoding)
                    startAxisX = CInt(XAxisTime.Range.Minimum)
                    endAxisX = CInt(XAxisTime.Range.Maximum)
                    If startAxisX < 0 Then startAxisX = 0
                    If endAxisX > UBound(MeasuredValues, 2) Then endAxisX = UBound(MeasuredValues, 2)
                    L = endAxisX - startAxisX + 1
                    ' копируем часть arrСреднее во временный
                    ' для режима Регистратор только видимые шлейфы
                    'ReDim_tempData(UBound(MeasuredValues), L - 1)
                    Re.Dim(tempData, UBound(MeasuredValues), L - 1)

                    For I = 0 To UBound(MeasuredValues)
                        K = 0
                        For J = startAxisX To endAxisX
                            tempData(I, K) = MeasuredValues(I, J)
                            K += 1
                        Next J
                    Next I

                    N = UBound(IndexParameters)
                    K = 0
                    transposeData = NationalInstruments.Analysis.Math.LinearAlgebra.Transpose(tempData)
                    'ReDim_y(L - 1, 0)
                    Re.Dim(y, L - 1, 0)
                    isSnapshotRegistration = (RegimeType = cРегистратор)
                    isParameterAdd = False

                    If isDetailedSheet Then
                        For M = 1 To N
                            If isSnapshotRegistration = True Then
                                If IsBeforeThatHappenLoadDbase Then
                                    If ParametersShaphotType(IndexParameters(M)).IsVisible Then isParameterAdd = True
                                Else
                                    If ParametersType(IndexParameters(M)).IsVisible Then isParameterAdd = True
                                End If
                            Else
                                isParameterAdd = True
                            End If

                            If isParameterAdd = True Then
                                'ReDimPreserve y(L - 1, K)
                                Re.DimPreserve(y, L - 1, K)
                                For I = 0 To L - 1
                                    y(I, K) = transposeData(I, M - 1)
                                Next I
                                K += 1
                            End If

                            isParameterAdd = False
                        Next M
                    Else ' выборочный лист 
                        For number As Integer = 1 To UBound(SnapshotSmallParameters) ' по возрастанию номеров
                            For J = 1 To UBound(SnapshotSmallParameters)
                                If SnapshotSmallParameters(J).NumberInList = number AndAlso SnapshotSmallParameters(J).IsVisible Then
                                    For M = 1 To N
                                        If IsBeforeThatHappenLoadDbase Then
                                            If ParametersShaphotType(IndexParameters(M)).NameParameter = SnapshotSmallParameters(J).NameParameter Then isParameterAdd = True
                                        Else
                                            If ParametersType(IndexParameters(M)).NameParameter = SnapshotSmallParameters(J).NameParameter Then isParameterAdd = True
                                        End If

                                        If isParameterAdd = True Then
                                            'ReDimPreserve y(L - 1, K)
                                            Re.DimPreserve(y, L - 1, K)
                                            For I = 0 To L - 1
                                                y(I, K) = transposeData(I, M - 1)
                                            Next I

                                            K += 1
                                            isParameterAdd = False
                                            Exit For
                                        End If
                                    Next M
                                End If
                            Next J
                        Next number
                    End If

                    isParameterAdd = False
                    If isDetailedSheet Then
                        ' запись только выделенного диапазона
                        ' заголовки столбцов
                        For M = 1 To N
                            If isSnapshotRegistration = True Then
                                If IsBeforeThatHappenLoadDbase Then
                                    If ParametersShaphotType(IndexParameters(M)).IsVisible Then
                                        name = ParametersShaphotType(IndexParameters(M)).NameParameter
                                        isParameterAdd = True
                                    End If
                                Else
                                    If ParametersType(IndexParameters(M)).IsVisible Then
                                        name = ParametersType(IndexParameters(M)).NameParameter
                                        isParameterAdd = True
                                    End If
                                End If
                            Else ' в заголовок все имена
                                isParameterAdd = True
                                If IsBeforeThatHappenLoadDbase Then
                                    name = ParametersShaphotType(IndexParameters(M)).NameParameter
                                Else
                                    name = ParametersType(IndexParameters(M)).NameParameter
                                End If
                            End If

                            If isParameterAdd = True Then
                                SW.Write(name)
                                SW.Write(vbTab)
                            End If
                            isParameterAdd = False
                        Next M
                    Else ' выборочный лист 
                        For number = 1 To UBound(SnapshotSmallParameters) ' по возрастанию номеров
                            For J = 1 To UBound(SnapshotSmallParameters)
                                If SnapshotSmallParameters(J).NumberInList = number AndAlso SnapshotSmallParameters(J).IsVisible Then
                                    For M = 1 To N
                                        If IsBeforeThatHappenLoadDbase Then
                                            If ParametersShaphotType(IndexParameters(M)).NameParameter = SnapshotSmallParameters(J).NameParameter Then
                                                name = ParametersShaphotType(IndexParameters(M)).NameParameter
                                                isParameterAdd = True
                                            End If
                                        Else
                                            If ParametersType(IndexParameters(M)).NameParameter = SnapshotSmallParameters(J).NameParameter Then
                                                name = ParametersType(IndexParameters(M)).NameParameter
                                                isParameterAdd = True
                                            End If
                                        End If

                                        If isParameterAdd = True Then
                                            SW.Write(name)
                                            SW.Write(vbTab)
                                            isParameterAdd = False
                                            Exit For
                                        End If
                                    Next M
                                End If
                            Next J
                        Next number
                    End If

                    SW.WriteLine()

                    If dataMeasuredValuesString Is Nothing Then
                        dataMeasuredValuesString = New StringBuilder(1000000)
                    Else
                        dataMeasuredValuesString.Remove(0, dataMeasuredValuesString.Length)
                    End If

                    For I = 0 To UBound(y)
                        For J = 0 To UBound(y, 2)
                            dataMeasuredValuesString.Append(Str(Round(y(I, J), Precision)))
                            If J <> UBound(y, 2) Then dataMeasuredValuesString.Append(vbTab)
                        Next J
                        dataMeasuredValuesString.Append(vbCrLf)
                    Next I

                    SW.Write(dataMeasuredValuesString.ToString)
                End Using
            End Using
            memoryOk = True
        Catch ex As OutOfMemoryException
            Const caption As String = "Экспорт файла в EXCEL"
            Dim text As String = $"{ex}{vbCrLf}Не хватает памяти.{vbCrLf}Попробуйте конвертировать файл для уменьшения количества параметров или увеличить прореживание."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        Catch ex As Exception
            Const caption As String = "Экспорт файла в EXCEL"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        If memoryOk = False Then Exit Sub
        '--- конец копирования в текстовый поток ----------------------------
        ' открыть книгу
        Dim ProtocolExcel As New Excel.Application With {
            .Visible = False ' было False
            }
        ProtocolExcel.Workbooks.Add()
        'ProtocolExcel.Workbooks.Open FileName:=strПутьExcel
        Try
            WorkWithExcelSheetExport(pathTextDataStream, K + 1, UBound(y), ProtocolExcel)
            ' запрос на изменения не надо
            'ProtocolExcel.ActiveWorkbook.Saved = False
            ProtocolExcel.ActiveWorkbook.SaveAs(Filename:=PathExcel, FileFormat:=Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)
            'ProtocolExcel.ActiveWorkbook.Saved = True 'было True
            ' изменения не сохранять
            ProtocolExcel.ActiveWorkbook.Close(SaveChanges:=False)
            'ProtocolExcel.ActiveWindow.Close(SaveChanges:=False)
            ProtocolExcel.Quit()
        Catch ex As Exception
            Const caption As String = "Экспорт в Excel"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        GC.Collect()
        ProtocolExcel = Nothing
    End Sub
    ''' <summary>
    ''' Работа на листе Excel Экспорта
    ''' </summary>
    ''' <param name="pathTextDataStreamExcel"></param>
    ''' <param name="index"></param>
    ''' <param name="counRows"></param>
    ''' <param name="refProtocolExcel"></param>
    Private Sub WorkWithExcelSheetExport(ByVal pathTextDataStreamExcel As String,
                                         ByVal index As Integer,
                                         ByVal counRows As Integer,
                                         ByRef refProtocolExcel As Excel.Application)
        Dim intString As Integer
        Dim rangeRow As String
        Dim I As Integer
        Dim integerPart As Integer ' целаяЧасть
        Dim remainder As Integer ' остаток
        Dim selectRange As String
        Dim columnName(26) As Char

        columnName(1) = CChar("A")
        columnName(2) = CChar("B")
        columnName(3) = CChar("C")
        columnName(4) = CChar("D")
        columnName(5) = CChar("E")
        columnName(6) = CChar("F")
        columnName(7) = CChar("G")
        columnName(8) = CChar("H")
        columnName(9) = CChar("I")
        columnName(10) = CChar("J")
        columnName(11) = CChar("K")
        columnName(12) = CChar("L")
        columnName(13) = CChar("M")
        columnName(14) = CChar("N")
        columnName(15) = CChar("O")
        columnName(16) = CChar("P")
        columnName(17) = CChar("Q")
        columnName(18) = CChar("R")
        columnName(19) = CChar("S")
        columnName(20) = CChar("T")
        columnName(21) = CChar("U")
        columnName(22) = CChar("V")
        columnName(23) = CChar("W")
        columnName(24) = CChar("X")
        columnName(25) = CChar("Y")
        columnName(26) = CChar("Z")

        With refProtocolExcel
            ' Разметка времени
            .Sheets("Лист1").Select()
            .Range("A2").Select()
            .ActiveCell.FormulaR1C1 = 0 ' "0"
            .Range("A3").Select()
            .ActiveCell.FormulaR1C1 = 1 / FrequencyBackgroundSnapshot
            .Range("A4").Select()
            .ActiveCell.FormulaR1C1 = 2 / FrequencyBackgroundSnapshot
            .Range("A2:A4").Select()
            .Selection.AutoFill(Destination:= .Range("A2:A" & CStr(counRows + 2)), Type:=Excel.XlAutoFillType.xlFillDefault)
            .Range("A2:A" & CStr(counRows + 2)).Select()
            .ActiveWindow.ScrollRow = 1
            'В желтый цвет
            .Range("A2").Select()
            .ActiveWindow.ScrollRow = 868
            .Range("A2:A" & CStr(counRows + 2)).Select()
            With .Selection.Interior
                .ColorIndex = 6
                .Pattern = Excel.Constants.xlSolid
            End With
            ' Импорт из текстового файла
            .Range("B1").Select()
            With .ActiveSheet.QueryTables.Add(Connection:="TEXT;" & pathTextDataStreamExcel, Destination:= .Range("B1"))
                .FieldNames = True
                .RowNumbers = False
                .FillAdjacentFormulas = False
                .PreserveFormatting = True
                .RefreshOnFileOpen = False
                .RefreshStyle = Excel.XlCellInsertionMode.xlInsertDeleteCells
                .SavePassword = False
                .SaveData = True
                .AdjustColumnWidth = True
                .RefreshPeriod = 0
                .TextFilePromptOnRefresh = False
                .TextFilePlatform = Excel.XlPlatform.xlWindows
                .TextFileStartRow = 1
                .TextFileParseType = Excel.XlTextParsingType.xlDelimited
                .TextFileTextQualifier = Excel.XlTextQualifier.xlTextQualifierDoubleQuote
                .TextFileConsecutiveDelimiter = False
                .TextFileTabDelimiter = True
                .TextFileSemicolonDelimiter = False
                .TextFileCommaDelimiter = False
                .TextFileSpaceDelimiter = False
                .TextFileColumnDataTypes = New Object() {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
                .Refresh(BackgroundQuery:=False)
            End With

            integerPart = index \ 26
            remainder = index Mod 26

            If index <= 26 Then
                selectRange = columnName(index)
            Else
                If remainder = 0 Then
                    selectRange = columnName(integerPart - 1) & columnName(26)
                Else
                    selectRange = columnName(integerPart) & columnName(remainder)
                End If
            End If

            selectRange &= CStr(counRows + 1)
            ' Построение диаграммы
            .Range("A1:" & selectRange).Select()
            .Charts.Add()
            .ActiveChart.ChartType = Excel.XlChartType.xlXYScatterSmoothNoMarkers
            .ActiveChart.SetSourceData(Source:= .Sheets("Лист1").Range("A1:" & selectRange), PlotBy:=Excel.XlRowCol.xlColumns)
            .ActiveChart.Location(Where:=Excel.XlChartLocation.xlLocationAsNewSheet)
            With .ActiveChart.Axes(Excel.XlAxisType.xlCategory)
                .HasMajorGridlines = True
                .HasMinorGridlines = False
            End With
            With .ActiveChart.Axes(Excel.XlAxisType.xlValue)
                .HasMajorGridlines = True
                .HasMinorGridlines = False
            End With
            .ActiveChart.PlotArea.Select()
            With .Selection.Border
                .ColorIndex = 16
                .Weight = Excel.XlBorderWeight.xlThin
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Interior
                .ColorIndex = 2
                .PatternColorIndex = 1
                .Pattern = Excel.Constants.xlSolid
            End With
            .ActiveChart.Axes(Excel.XlAxisType.xlValue).Select()
            .Selection.TickLabels.NumberFormat = "0"
            '.ActiveChart.Axes(xlCategory).Select
            With .ActiveChart.Axes(Excel.XlAxisType.xlCategory)
                .MinimumScaleIsAuto = True
                .MaximumScale = counRows / FrequencyBackgroundSnapshot
                .MinorUnitIsAuto = True
                .MajorUnit = (counRows / FrequencyBackgroundSnapshot) / 18
                .Crosses = Excel.Constants.xlAutomatic
                .ReversePlotOrder = False
                .ScaleType = Excel.XlTrendlineType.xlLinear
                .DisplayUnit = Excel.Constants.xlNone
            End With
            .Sheets("Лист1").Select()
            ' форматирование столбца и строки
            .Range("A1").Select()
            .ActiveCell.FormulaR1C1 = "Время сек."
            .Rows._Default("1:1").Select()
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlCenter
            End With
            With .Selection.Font
                .Name = "Arial CYR"
                .FontStyle = "полужирный"
                .Size = 11
                .ColorIndex = 3
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideVertical)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Interior
                .ColorIndex = 6
            End With
            .Columns._Default("A:A").Select()
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeLeft)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeTop)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlEdgeRight)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
                .LineStyle = Excel.XlLineStyle.xlContinuous
            End With
            With .Selection
                .HorizontalAlignment = Excel.Constants.xlCenter
            End With
            .Cells.Select()
            .Selection.Columns.AutoFit()
            .Range("B2").Select()
            ' зафиксировать область
            .ActiveWindow.FreezePanes = True

            .Range("A1").Select()
            ' метку КТ ставим в столбец после диапазона
            index += 1
            integerPart = index \ 26
            remainder = index Mod 26

            If index <= 26 Then
                selectRange = columnName(index)
            Else
                If remainder = 0 Then
                    selectRange = columnName(integerPart - 1) & columnName(26)
                Else
                    selectRange = columnName(integerPart) & columnName(remainder)
                End If
            End If

            For I = Arrows.Count - 1 To 0 Step -1
                If CBool(InStr(1, Arrows(I).Legend, "КТ")) Then
                    intString = CShort(Arrows(I).X1) + 1  ' начало с А2
                    rangeRow = CStr(intString)
                    .Rows._Default($"{rangeRow}:{rangeRow}").Select()
                    .Selection.Interior.ColorIndex = 3
                    .Range(selectRange & rangeRow).Select()
                    .ActiveCell.FormulaR1C1 = Arrows(I).Legend
                End If
            Next I
            .Range("A1").Select()
        End With
    End Sub
#End Region

#Region "График и курсоры"
    Private Sub OneCursorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonOneCursor.Click
        If MeasuredValues Is Nothing Then Exit Sub

        ButtonOneCursor.Checked = Not ButtonOneCursor.Checked

        If ButtonOneCursor.Checked = False Then
            ButtonTwoCursor.Checked = False
        End If

        If ButtonOneCursor.Checked Then
            GraphModeValue = MyGraphMode.OneCursor
        Else
            GraphModeValue = MyGraphMode.Scaling
        End If

        SetGraphMode(GraphModeValue)
    End Sub

    Private Sub TwoCursorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonTwoCursor.Click
        If MeasuredValues Is Nothing Then Exit Sub

        ButtonTwoCursor.Checked = Not ButtonTwoCursor.Checked

        If ButtonTwoCursor.Checked Then
            ButtonOneCursor.Checked = True
        End If

        If ButtonTwoCursor.Checked Then
            GraphModeValue = MyGraphMode.TwoCursors
        ElseIf ButtonOneCursor.Checked Then
            GraphModeValue = MyGraphMode.OneCursor
        Else
            GraphModeValue = MyGraphMode.Scaling
        End If

        SetGraphMode(GraphModeValue)
    End Sub

    ''' <summary>
    ''' если мышь над PlotArea, то Вначале идёт событие PlotAreaMouseDown затем MouseDown
    ''' если мышь не над PlotArea, но над другими областями графика, то идёт только MouseDown
    ''' событи KeyDown в любом случае идёт перед ними
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GraphTime_PlotAreaMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles WaveformGraphTime.PlotAreaMouseDown
        Dim I, J As Integer
        Dim difference, minDifference As Double ' разность

        ' проверить перед выводом контекстного меню
        If e.Button = MouseButtons.Right Then
            Dim hitCursor As XYCursor = WaveformGraphTime.GetCursorAt(e.X, e.Y)

            If hitCursor IsNot Nothing Then
                contextCursor = hitCursor
                ContextMenuCursor.Show(WaveformGraphTime, New Point(e.X, e.Y))
            Else
                Dim hitAnnotation As XYAnnotation = WaveformGraphTime.GetAnnotationAt(e.X, e.Y)

                If hitAnnotation IsNot Nothing Then
                    contextAnnotation = hitAnnotation
                    ContextMenuAnnotation.Show(WaveformGraphTime, New Point(e.X, e.Y))
                    pointMouse = New Point(WaveformGraphTime.Location.X + e.X, WaveformGraphTime.Location.Y + e.Y)
                Else
                    Dim _XPosition, _YPosition As Double
                    Dim index As Integer
                    Dim hitPlot As XYPlot = WaveformGraphTime.GetPlotAt(e.X, e.Y, _XPosition, _YPosition, index)

                    If hitPlot IsNot Nothing Then
                        XyCursorParametr.MoveCursor(_XPosition, _YPosition)
                        'LabelXreal.Text = ""
                        'LabelYreal.Text = ""
                    Else
                        ' еще раз проба с графиком
                        ' Координаты х и у относительно окна графика, а не окна приложения
                        Dim pix_X_В_Дел As Double = Abs((XAxisTime.Range.Maximum - XAxisTime.Range.Minimum)) / WaveformGraphTime.PlotAreaBounds.Width
                        Dim pix_Y_В_Дел As Double = Abs((YAxisTime.Range.Maximum - YAxisTime.Range.Minimum)) / WaveformGraphTime.PlotAreaBounds.Height

                        _XPosition = XAxisTime.Range.Minimum + (e.X - WaveformGraphTime.PlotAreaBounds.X) * pix_X_В_Дел
                        _YPosition = YAxisTime.Range.Maximum - (e.Y - WaveformGraphTime.PlotAreaBounds.Y) * pix_Y_В_Дел
                        'LabelXreal.Text = _XPosition ' Format(_XPosition, "##0.0")
                        'LabelYreal.Text = _YPosition
                        XyCursorParametr.MoveCursor(_XPosition, _YPosition)
                        'XyCursorParametr.XPosition = _XPosition
                        'XyCursorParametr.YPosition = _YPosition
                    End If

                    ' найти самый ближайший по значению
                    minDifference = Double.MaxValue

                    Dim iXPosition As Integer = CInt(_XPosition)
                    ' отобразить физические значения
                    For J = 1 To UBound(IndexParameters)
                        If IsRegimeIsRegistrator Then
                            ' только видимые
                            If isDetailedSheet Then
                                If ParametersShaphotType(IndexParameters(J)).IsVisible Then
                                    difference = Abs(MeasuredValuesToRange(J - 1, iXPosition) - _YPosition)
                                    If minDifference > difference Then
                                        minDifference = difference
                                        I = J
                                    End If
                                End If
                            Else
                                If SnapshotSmallParameters(J).IsVisible Then
                                    difference = Abs(MeasuredValuesToRange(J - 1, iXPosition) - _YPosition)
                                    If minDifference > difference Then
                                        minDifference = difference
                                        I = J
                                    End If
                                End If
                            End If
                        Else
                            ' по всем шлейфам
                            difference = Abs(MeasuredValuesToRange(J - 1, iXPosition) - _YPosition)
                            If minDifference > difference Then
                                minDifference = difference
                                I = J
                            End If
                        End If
                    Next J

                    XyCursorParametr.YPosition = MeasuredValuesToRange(I - 1, iXPosition)
                    cursorAnnotation = New XYPointAnnotation() With {
                        .Caption = "Annotation",
                        .ShapeFillColor = Color.Red,
                        .ShapeSize = New Size(6, 6),
                        .ShapeStyle = ShapeStyle.Oval,
                        .InteractionMode = AnnotationInteractionMode.None,
                        .XAxis = XAxisTime,
                        .YAxis = YAxisTime
                    }
                    WaveformGraphTime.Annotations.Add(cursorAnnotation)

                    ' переделать  на .Annotations.Item("Select").PointIndex = _XPosition
                    cursorAnnotation.SetPosition(_XPosition, XyCursorParametr.YPosition)
                    'CursorAnnotation.Caption = "X: " & Math.Round(_XPosition, 3).ToString & "  Y: " & Math.Round(_YPosition, 3).ToString
                    If ParametersShaphotType Is Nothing Then ' сразу после снимка arrПараметрыСнимка=Nothing, а после загрузки не равен Nothing
                        cursorAnnotation.Caption = ParametersType(IndexParameters(I)).NameParameter
                    Else
                        cursorAnnotation.Caption = ParametersShaphotType(IndexParameters(I)).NameParameter
                    End If

                    cursorAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 50.0!, -50.0!)
                    XyCursorParametr.Visible = True
                End If
            End If
        End If
    End Sub

    Private Sub GraphTime_PlotAreaMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles WaveformGraphTime.PlotAreaMouseUp
        If e.Button = MouseButtons.Right Then
            XyCursorParametr.Visible = False
            If cursorAnnotation IsNot Nothing Then
                For Each itemAnnotation As XYPointAnnotation In WaveformGraphTime.Annotations
                    If itemAnnotation Is cursorAnnotation Then
                        WaveformGraphTime.Annotations.Remove(cursorAnnotation)
                        cursorAnnotation = Nothing
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private showMorePanel As Boolean = True
    ''' <summary>
    ''' Показать/Скрыть дополнительную панель
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MoreButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonMore.Click
        If showMorePanel Then
            'MoreButton.Image = Registration.ProjectResources.SINEWAVE - можно и так
            ButtonMore.Image = My.Resources.up
            ButtonMore.ToolTipText = "<< Скрыть дополнительную панель"
            showMorePanel = False
            SplitContainerForm.SplitterDistance = SplitContainerForm.SplitterDistance - 24
            TableLayoutPanelУправленияДляСнимка.RowStyles(1).Height = 24
            InstrumentControlStrip1.Visible = True
        Else
            ButtonMore.Image = My.Resources.down
            ButtonMore.ToolTipText = "Показать дополнительную панель >>"
            showMorePanel = True
            SplitContainerForm.SplitterDistance = SplitContainerForm.SplitterDistance + 24
            TableLayoutPanelУправленияДляСнимка.RowStyles(1).Height = 0
            InstrumentControlStrip1.Visible = False
        End If
    End Sub

    Private Sub GraphTime_AfterMoveCursor(ByVal sender As Object, ByVal e As AfterMoveXYCursorEventArgs) Handles WaveformGraphTime.AfterMoveCursor
        ' никакие обработчики перемещения курсора при регистрации не нужны
        Dim J, N As Integer
        Dim amplitude As Double
        Dim period As Double
        Dim xCursorIndexPosition As Integer
        Dim average As Double

        ' отобразить в листе текущие величины параметров
        If e.Cursor Is XyCursorStart Then
            xCursorIndexPosition = CInt(XyCursorStart.XPosition)
        ElseIf e.Cursor Is XyCursorEnd Then
            xCursorIndexPosition = CInt(XyCursorEnd.XPosition)
        Else : Exit Sub
        End If

        ' отобразить физические значения
        If xCursorIndexPosition > MeasuredValues.GetUpperBound(1) OrElse xCursorIndexPosition < 0 Then Exit Sub

        ListViewAcquisition.BeginUpdate()
        If isDetailedSheet Then
            For J = 1 To UBound(IndexParameters)
                ListViewAcquisition.Items(J - 1).SubItems(1).Text = CStr(Round(MeasuredValues(J - 1, xCursorIndexPosition), Precision))
                ' Debug.Print(ListView1.Items(J - 1).SubItems(0).Text)
                ' если в программе Осциллографировыния не корректно переименовали каналы или не сделали обновление Режима
                ' то могут в Registration быть ошибки а в Осциллографировыния нет
                ' попробовать скопировать из таблицы "РежимыХХ" режим sРегистратор в "строку конфигурации" таблицй "База Снимков"
            Next
        Else
            If IsRegimeIsRegistrator Then
                For J = 1 To UBound(IndexParameters)
                    If SnapshotSmallParameters(J).IsVisible Then
                        ListViewAcquisition.Items(SnapshotSmallParameters(J).NumberInList - 1).SubItems(1).Text = CStr(Round(MeasuredValues(J - 1, xCursorIndexPosition), Precision))
                    End If
                Next
            Else
                For J = 1 To UBound(IndexParameters)
                    ListViewAcquisition.Items(J - 1).SubItems(1).Text = CStr(Round(MeasuredValues(J - 1, xCursorIndexPosition), Precision))
                Next
            End If
        End If

        ' включить обновление элемента
        ListViewAcquisition.EndUpdate()
        ' передать данные при включенном DataSokcet
        Dim index As Integer

        If IsServerOn Then ' AndAlso Not (IsWorkWithController OrElse IsTcpClient)
            ' передать  Клиенту
            index = 1

            For J = 1 To UBound(IndexParameters)
                PackOfParameters(index) = ParametersShaphotType(IndexParameters(J)).NameParameter
                PackOfParameters(index + 1) = CStr(Round(MeasuredValues(J - 1, xCursorIndexPosition), Precision))
                PackOfParameters(index + 2) = CStr(ParametersShaphotType(IndexParameters(J)).NumberParameter)
                index += 3
            Next

            ServerForm.DataSocketSend.Data.Value = CObj(Join(PackOfParameters, Separator).Substring(1) & Separator)
        End If

        If e.Cursor Is XyCursorStart Then
            XPosition.Text = Format(XyCursorStart.XPosition / SampleRate, "0.###") ' Данные от курсора
            YPosition.Text = Format(e.YPosition, "0.###") ' Данные от обработчика случая

            If isUseWindowsDiagramFromParameter Then
                For Each itemGraf As FormPatternGraphByParameter In GraphsByParameters.Values
                    itemGraf.SlideTime.Value = e.XPosition / SampleRate
                Next
            End If
        End If

        If (GraphModeValue = MyGraphMode.TwoCursors AndAlso e.Cursor Is XyCursorEnd) OrElse (GraphModeValue = MyGraphMode.TwoCursors AndAlso e.Cursor Is XyCursorStart) Then
            amplitude = CDbl(Format(Abs(XyCursorEnd.YPosition - XyCursorStart.YPosition), "0.##"))
            period = CDbl(Format(Abs(XyCursorEnd.XPosition - XyCursorStart.XPosition) / SampleRate, "0.##"))
            AmplitudeVal.Text = CStr(amplitude)
            PeriodVal.Text = CStr(period)
        End If

        ' коллекция какой текст отображать
        Select Case ComboBoxPointers.SelectedIndex
            Case 0
                TextBoxDescriptionOnAxes.Text = $"t= {period}сек"
                Exit Select
            Case 1
                TextBoxDescriptionOnAxes.Text = $"d{ComboBoxSelectAxis.Text} = {amplitude}"
                Exit Select
            Case 2
                TextBoxDescriptionOnAxes.Text = vbNullString
                Exit Select
        End Select

        If isSlantingLine AndAlso ComboBoxPointers.SelectedIndex = 2 Then
            XAxisTimeRange = New Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
            YAxisTimeRange = New Range(YAxisTime.Range.Minimum, YAxisTime.Range.Maximum)
            '--- Переместить Наклонную Метку ----------------------------------
            ApplyDifferentialCoordinates(Arrows(Arrows.Count - 1))
            CType(WaveformGraphTime.Annotations(WaveformGraphTime.Annotations.Count - 1), XYPointAnnotation).SetPosition(xData(0), yData(0))
            '------------------------------------------------------------------
            Arrows(Arrows.Count - 1).Plot1.PlotY(yData, xData(0), xData(1) - xData(0))
            XAxisTime.Range = XAxisTimeRange
            YAxisTime.Range = YAxisTimeRange
        End If

        ' запомнить позиции курсоров
        xCursorStart = XyCursorStart.XPosition
        xCursorEnd = XyCursorEnd.XPosition

        If isShowDiagramFromParameter Then  ' работа с графиком от параметра
            If AllGraphParametersByParameter Is Nothing Then TuneTrand(False) ' т.е. запуск сбора

            Dim shift As Integer = CounterLightParametersGraph - 1
            Dim numberTrande As Integer
            Dim name As String
            Dim tempPointAnnotation As XYPointAnnotation

            If counterDiagramFromParameter >= CounterParametersGraph Then
                ' узнать значение параметра по оси Х в срезе курсора первого графика
                If Not isShowDiagramFromTime Then
                    For J = 1 To UBound(AllGraphParametersByParameter)
                        If nameParameterAxesX = AllGraphParametersByParameter(J).NameParameter Then
                            correctedValueAxisX = MeasuredValues(J - 1, xCursorIndexPosition)

                            If isClearDiagramFromParameterAxesX = True Then
                                For N = 0 To shift
                                    axesXDiagramFromParameter(N) = correctedValueAxisX
                                Next N
                                isClearDiagramFromParameterAxesX = False
                            End If

                            ' сдвиг массива
                            For N = 0 To shift - 1
                                axesXDiagramFromParameter(N) = axesXDiagramFromParameter(N + 1)
                            Next N

                            ' добавить к последнему элементу в массиве
                            axesXDiagramFromParameter(shift) = correctedValueAxisX ' = график который на ось х
                            Exit For
                        End If
                    Next
                End If

                For J = 1 To UBound(AllGraphParametersByParameter)
                    If isShowDiagramFromTime = True Then
                        If AllGraphParametersByParameter(J).NumberTail <> -1 Then
                            average = MeasuredValues(J - 1, xCursorIndexPosition)
                            dataDiagramFromParameter(AllGraphParametersByParameter(J).NumberTail, positionCursorDiagramFromParameter) = average
                            name = AllGraphParametersByParameter(J).NameParameter
                            tempPointAnnotation = CType(ScatterGraphParameter.Annotations(AllGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                            tempPointAnnotation.SetPosition(positionCursorDiagramFromParameter, average)
                            tempPointAnnotation.Caption = $"{name}: {Round(average, Precision)}"
                            tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                        End If
                    Else
                        If AllGraphParametersByParameter(J).NumberTail <> -1 Then
                            ' сдвиг массива
                            numberTrande = AllGraphParametersByParameter(J).NumberTail
                            average = MeasuredValues(J - 1, xCursorIndexPosition)

                            If isClearDataDiagramFromParameter(numberTrande) = False Then ' был очищен массив
                                For N = 0 To shift
                                    dataDiagramFromParameter(numberTrande, N) = average
                                Next N
                                isClearDataDiagramFromParameter(numberTrande) = True
                            End If

                            For N = 0 To shift - 1
                                dataDiagramFromParameter(numberTrande, N) = dataDiagramFromParameter(numberTrande, N + 1)
                            Next N

                            'добавить к последнему элементу в массиве
                            dataDiagramFromParameter(numberTrande, shift) = average
                            'переписали strName
                            name = AllGraphParametersByParameter(J).NameParameter
                            tempPointAnnotation = CType(ScatterGraphParameter.Annotations(AllGraphParametersByParameter(J).NumberTail), XYPointAnnotation)
                            tempPointAnnotation.SetPosition(axesXDiagramFromParameter(shift), average)
                            tempPointAnnotation.Caption = $"{name}: {Round(average, Precision)}"
                            tempPointAnnotation.CaptionAlignment = New AnnotationCaptionAlignment(BoundsAlignment.None, 20, -5)
                        End If
                    End If
                Next J

                If isShowDiagramFromTime Then
                    XyCursor1.XPosition = positionCursorDiagramFromParameter
                    ScatterGraphParameter.PlotXYMultiple(axesXDiagramFromParameter, dataDiagramFromParameter)
                    positionCursorDiagramFromParameter += 1
                    If positionCursorDiagramFromParameter > arraysizeDiagramFromParameter Then positionCursorDiagramFromParameter = 0
                Else
                    ScatterGraphParameter.PlotXYMultiple(axesXDiagramFromParameter, dataDiagramFromParameter)
                End If

                counterDiagramFromParameter = 0
                CopyFromListViewAcquisitionToListViewParametr()
            End If

            counterDiagramFromParameter += 1
        End If

        ' когда ставится метка из клиента при сборе КТ lintЧастотаОпроса ему неизвестна и равна 0
        If SampleRate <> 0 Then MoveSlideTime(xCursorStart / SampleRate)

        If isUsePens Then
            If e.Cursor Is XyCursorStart Then
                MoveAnnotations(xCursorStart, xCursorIndexPosition, False, True)
            Else
                MoveAnnotations(xCursorEnd, xCursorIndexPosition, False, False)
            End If
        End If
    End Sub

    Private Sub ComboBoxPointers_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBoxPointers.SelectedIndexChanged
        If IsSnapshot Then GraphTime_AfterMoveCursor(WaveformGraphTime, New AfterMoveXYCursorEventArgs(XyCursorEnd, XyCursorEnd.XPosition, XyCursorEnd.YPosition, UI.Action.Programmatic))

        If ComboBoxPointers.SelectedIndex = 2 Then
            ButtonShowAxes.Enabled = Not (TextBoxDescriptionOnAxes.Text = "")
        Else
            ButtonFixLine.Visible = False
            ButtonShowAxes.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Заполнить список время сбора для снимков 200 гц
    ''' </summary>
    Private Sub FillListTimeCollection()
        Dim countTimeTicks As Integer = 30 ' для SCXI 5 минут максимум через 10 сек 

        'Select Case SnapshotLimit
        'Case 32
        '    countTimeTicks = 48
        '    Exit Select
        'Case 64
        '    countTimeTicks = 24
        '    Exit Select
        'Case 128
        '    countTimeTicks = 12
        '    Exit Select
        'Case 256
        '    countTimeTicks = 6
        '    Exit Select
        '    Case 512
        '        countTimeTicks = 3
        '        Exit Select
        'End Select

        For I As Integer = 1 To countTimeTicks
            ComboBoxTimeMeasurement.Items.Add(CStr(I * 10))
        Next

        ComboBoxTimeMeasurement.SelectedIndex = 0
    End Sub

#Region "Интерактивные Режимы графика"
    ''' <summary>
    ''' Назначение обработчиков события перемещения мыши для вывода подсказок
    ''' </summary>
    ''' <param name="menuItems"></param>
    Private Sub InitializeMenuHelperStrings(ByVal menuItems As ToolStripItemCollection)
        For Each itemTS As ToolStripItem In menuItems
            If TypeOf itemTS Is ToolStripMenuItem Then
                ' то же самое
                'If (item.GetType() Is GetType(ToolStripMenuItem)) Then
                If CStr(itemTS.Tag) = "Helper" Then ' приблуда только для конкретных пунктов
                    utilityHelper.AddMenuString(itemTS)
                    AddHandler itemTS.MouseMove, AddressOf OnMenuSelect
                    AddHandler itemTS.MouseEnter, AddressOf MenuItem_MouseEnter
                    AddHandler itemTS.MouseLeave, AddressOf MenuItem_MouseLeave

                    If CType(itemTS, ToolStripMenuItem).DropDownItems.Count > 0 Then
                        InitializeMenuHelperStrings(CType(itemTS, ToolStripMenuItem).DropDownItems)
                    End If
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Создание взаимосвязи между кнопками и пунктами меню
    ''' </summary>
    Private Sub MapToolBarAndMenuItems()
        utilityHelper.MapMenuAndToolBar(ButtonXRange, MenuXRange)
        utilityHelper.MapMenuAndToolBar(ButtonYRange, MenuYRange)
        utilityHelper.MapMenuAndToolBar(ButtonDragCursor, MenuDragCursor)
        utilityHelper.MapMenuAndToolBar(ButtonPanX, MenuPanX)
        utilityHelper.MapMenuAndToolBar(ButtonPanY, MenuPanY)
        utilityHelper.MapMenuAndToolBar(ButtonZoomPoint, MenuZoomPoint)
        utilityHelper.MapMenuAndToolBar(ButtonZoomX, MenuZoomX)
        utilityHelper.MapMenuAndToolBar(ButtonZoomY, MenuZoomY)
    End Sub

    ''' <summary>
    ''' Проверка интерактивного режима
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <returns></returns>
    Private Function IsInteractionModeSelected(ByVal mode As GraphInteractionModes) As Boolean
        Return ((WaveformGraphTime.InteractionMode And mode) = mode)
    End Function
    ''' <summary>
    ''' Установить режим работы с холстом в соответствии с установками в режиме дизайна
    ''' </summary>
    Private Sub InitializeInteractionMenu()
        ' по умолчанию следующие настройки
        'GraphTime.InteractionMode = CType((((((((NationalInstruments.UI.GraphInteractionModes.ZoomX Or NationalInstruments.UI.GraphInteractionModes.ZoomY) _
        '    Or NationalInstruments.UI.GraphInteractionModes.ZoomAroundPoint) _
        '    Or NationalInstruments.UI.GraphInteractionModes.PanX) _
        '    Or NationalInstruments.UI.GraphInteractionModes.PanY) _
        '    Or NationalInstruments.UI.GraphInteractionModes.DragCursor) _
        '    Or NationalInstruments.UI.GraphInteractionModes.DragAnnotationCaption) _
        '    Or NationalInstruments.UI.GraphInteractionModes.EditRange), NationalInstruments.UI.GraphInteractionModes)

        ' применить настройки в соответствии с установками в режиме дизайна
        If IsInteractionModeSelected(GraphInteractionModes.DragCursor) Then
            'dragCursorMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuDragCursor, GraphInteractionModes.DragCursor)
        End If

        If IsInteractionModeSelected(GraphInteractionModes.PanX) Then
            ' panXMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuPanX, GraphInteractionModes.PanX)
        End If

        If IsInteractionModeSelected(GraphInteractionModes.PanY) Then
            ' panYMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuPanY, GraphInteractionModes.PanY)
        End If

        If IsInteractionModeSelected(GraphInteractionModes.ZoomAroundPoint) Then
            ' zoomPointMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuZoomPoint, GraphInteractionModes.ZoomAroundPoint)
        End If

        If IsInteractionModeSelected(GraphInteractionModes.ZoomX) Then
            ' zoomXMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuZoomX, GraphInteractionModes.ZoomX)
        End If

        If IsInteractionModeSelected(GraphInteractionModes.ZoomY) Then
            ' zoomYMenuItem.PerformClick()
            UpdateGraphAndToolBar(MenuZoomY, GraphInteractionModes.ZoomY)
        End If

        'sampleWaveformGraph.InteractionMode =
        'ZoomX Or ZoomY Or ZoomAroundPoint Or PanX Or PanY Or DragCursor Or DragAnnotationCaption {127}
    End Sub

    ''' <summary>
    ''' Привести настройки кнопок в соответствии с пунктами меню
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="mode"></param>
    Private Sub UpdateGraphAndToolBar(ByVal item As ToolStripMenuItem, ByVal mode As GraphInteractionModes)
        item.Checked = Not item.Checked

        Dim button As ToolStripButton = utilityHelper.FromMenuItem(item)
        button.Checked = item.Checked

        If (item.Checked) Then
            WaveformGraphTime.InteractionMode = mode Or WaveformGraphTime.InteractionMode
        Else
            WaveformGraphTime.InteractionMode = (Not mode) And WaveformGraphTime.InteractionMode
        End If
    End Sub

    Private Sub MenuItem_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
        lastStatus = LabelRegistration.Text
    End Sub

    Private Sub MenuItem_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        LabelRegistration.Text = lastStatus
    End Sub

    Private Sub OnMenuSelect(ByVal sender As Object, ByVal e As EventArgs)
        LabelRegistration.Text = CType(sender, ToolStripMenuItem).ToolTipText ' utilityHelper.GetMenuString(sender)
    End Sub

    Private Sub ToolStripMain2_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs) Handles ToolStripMain2.ItemClicked
        If TypeOf e.ClickedItem Is ToolStripButton Then
            Dim item As ToolStripMenuItem = utilityHelper.FromToolBarButton(CType(e.ClickedItem, ToolStripButton))
            If item IsNot Nothing Then item.PerformClick()
        End If
    End Sub

    Private Sub OnDragCursor(ByVal sender As Object, ByVal e As EventArgs) Handles MenuDragCursor.Click
        UpdateGraphAndToolBar(MenuDragCursor, GraphInteractionModes.DragCursor)
    End Sub

    Private Sub OnPanX(ByVal sender As Object, ByVal e As EventArgs) Handles MenuPanX.Click
        UpdateGraphAndToolBar(MenuPanX, GraphInteractionModes.PanX)
    End Sub

    Private Sub OnPanY(ByVal sender As Object, ByVal e As EventArgs) Handles MenuPanY.Click
        UpdateGraphAndToolBar(MenuPanY, GraphInteractionModes.PanY)
    End Sub

    Private Sub OnZoomAroundPoint(ByVal sender As Object, ByVal e As EventArgs) Handles MenuZoomPoint.Click
        UpdateGraphAndToolBar(MenuZoomPoint, GraphInteractionModes.ZoomAroundPoint)
    End Sub

    Private Sub OnZoomX(ByVal sender As Object, ByVal e As EventArgs) Handles MenuZoomX.Click
        UpdateGraphAndToolBar(MenuZoomX, GraphInteractionModes.ZoomX)
    End Sub

    Private Sub OnZoomY(ByVal sender As Object, ByVal e As EventArgs) Handles MenuZoomY.Click
        UpdateGraphAndToolBar(MenuZoomY, GraphInteractionModes.ZoomY)
    End Sub

    Private Sub SetRange(ByVal scale As UI.Scale, ByVal caption As String)
        Dim dlg As FormDialogRangeEditor

        If scale Is XAxisTime Then
            dlg = New FormDialogRangeEditor(Round(scale.Range.Minimum / SampleRate, 0), Round(scale.Range.Maximum / SampleRate, 0))
        Else
            dlg = New FormDialogRangeEditor(scale.Range.Minimum, scale.Range.Maximum)
        End If

        dlg.Text = caption
        Dim result As DialogResult = dlg.ShowDialog()

        If Not result = DialogResult.Cancel Then
            Try
                If scale Is XAxisTime Then
                    scale.Range = New Range(dlg.Minimum * SampleRate, dlg.Maximum * SampleRate)
                Else
                    scale.Range = New Range(dlg.Minimum, dlg.Maximum)
                End If
            Catch ex As ArgumentException
                Const exCaption As String = "Ошибка диапазона"
                Const text As String = "Диапазон.Minimum больше чем Диапазон.Maximum"
                MessageBox.Show(text, exCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
            End Try
        End If
        dlg.Dispose()
    End Sub

    Private Sub OnSetXRange(ByVal sender As Object, ByVal e As EventArgs) Handles MenuXRange.Click
        SetRange(XAxisTime, "Установить X Диапазон")
    End Sub

    Private Sub OnSetYRange(ByVal sender As Object, ByVal e As EventArgs) Handles MenuYRange.Click
        SetRange(YAxisTime, "Установить Y Диапазон")
    End Sub

#End Region

#Region "Контекстные Меню"
    ''' <summary>
    ''' Настроить контекстные меню
    ''' </summary>
    Private Sub InitializeContextMenu()
        ' добавить пункты контекстного меню
        Dim bools As Boolean() = {True, False}
        Dim boolStrings As String() = Nothing

        If DataConverter.CanConvert(bools, GetType(String())) Then
            boolStrings = CType(DataConverter.Convert(bools, GetType(String())), String())
        End If

        Dim snapModes As String() = [Enum].GetNames(GetType(CursorSnapMode))
        AddMenuItem(ContextMenuCursor, "Режим Захвата", snapModes, AddressOf OnSnapModeMenuClick, AddressOf OnSnapModeMenuPopup)

        Dim lineStyles As String() = EnumObject.GetNames(GetType(LineStyle))
        AddMenuItem(ContextMenuCursor, "Стиль Линии", lineStyles, AddressOf OnLineStyleMenuClick, AddressOf OnLineStyleMenuPopup)

        Dim lineWidths As Single() = {1, 2, 3, 4, 5}
        If DataConverter.CanConvert(lineWidths, GetType(String())) Then
            Dim lineWidthStrings As String() = CType(DataConverter.Convert(lineWidths, GetType(String())), String())
            AddMenuItem(ContextMenuCursor, "Толщина Линии", lineWidthStrings, AddressOf OnLineWidthMenuClick, AddressOf OnLineWidthMenuPopup)
        End If

        Dim shapeZOrder As String() = [Enum].GetNames(GetType(AnnotationZOrder))
        AddMenuItem(ContextMenuAnnotation, "Формат Z-Порядка", shapeZOrder, AddressOf OnShapeZOrderMenuClick, AddressOf OnShapeZOrderMenuPopup)

        Dim arrowHeadStyle As String() = EnumObject.GetNames(GetType(ArrowStyle))
        AddMenuItem(ContextMenuAnnotation, "Стиль указателя Стрелки", arrowHeadStyle, AddressOf OnArrowHeadStyleMenuClick, AddressOf OnArrowHeadStyleMenuPopup)

        AddMenuItem(ContextMenuAnnotation, "Стиль текста ...", AddressOf OnCaptionFontMenuClick, Nothing)
    End Sub
    ''' <summary>
    ''' Добавить контекстное меню.
    ''' Перегруженный метод.
    ''' </summary>
    ''' <param name="destination"></param>
    ''' <param name="caption"></param>
    ''' <param name="onClick"></param>
    ''' <param name="onPopup"></param>
    Private Sub AddMenuItem(ByVal destination As ContextMenuStrip, ByVal caption As String, ByVal onClick As EventHandler, ByVal onPopup As PaintEventHandler)
        Dim menu As ToolStripMenuItem = New ToolStripMenuItem(caption, Nothing, onClick)
        AddHandler menu.Paint, onPopup
        destination.Items.Add(menu)
    End Sub
    ''' <summary>
    ''' Добавить контекстное меню.
    ''' Перегруженный метод.
    ''' </summary>
    ''' <param name="destination"></param>
    ''' <param name="caption"></param>
    ''' <param name="menuItemCaptions"></param>
    ''' <param name="onItemClick"></param>
    ''' <param name="onPopup"></param>
    Private Sub AddMenuItem(ByVal destination As ContextMenuStrip, ByVal caption As String, ByVal menuItemCaptions() As String, ByVal onItemClick As EventHandler, ByVal onPopup As PaintEventHandler)
        Dim menuItems(menuItemCaptions.Length - 1) As ToolStripItem
        Dim menuItemNew As ToolStripMenuItem
        Dim i As Integer

        While i < menuItemCaptions.Length
            menuItemNew = New ToolStripMenuItem(menuItemCaptions(i), Nothing, onItemClick)
            AddHandler menuItemNew.Paint, onPopup
            menuItems(i) = menuItemNew
            i += 1
        End While

        Dim menuUp As ToolStripMenuItem = New ToolStripMenuItem(caption, Nothing, menuItems)
        destination.Items.Add(menuUp)
    End Sub
    ''' <summary>
    ''' Обработчик щелчка на пункте меню
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnSnapModeMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim snapMode As CursorSnapMode = CType([Enum].Parse(GetType(CursorSnapMode), menuItem.Text), CursorSnapMode)
                contextCursor.SnapMode = snapMode
            End If

            contextCursor = Nothing
        End If
    End Sub

    Private Sub OnSnapModeMenuPopup(ByVal sender As Object, ByVal e As PaintEventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim snapMode As CursorSnapMode = CType([Enum].Parse(GetType(CursorSnapMode), menuItem.Text), CursorSnapMode)

                If contextCursor.SnapMode = snapMode Then
                    menuItem.Checked = True
                Else
                    menuItem.Checked = False
                End If
            End If
        End If
    End Sub

    Private Sub OnLineStyleMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim lineStyle As LineStyle = CType(EnumObject.Parse(GetType(LineStyle), menuItem.Text), LineStyle)

                If Not (lineStyle Is Nothing) Then
                    contextCursor.LineStyle = lineStyle
                End If
            End If

            contextCursor = Nothing
        End If
    End Sub

    Private Sub OnLineStyleMenuPopup(ByVal sender As Object, ByVal e As PaintEventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim lineStyle As LineStyle = CType(EnumObject.Parse(GetType(LineStyle), menuItem.Text), LineStyle)

                If contextCursor.LineStyle.Equals(lineStyle) Then
                    menuItem.Checked = True
                Else
                    menuItem.Checked = False
                End If
            End If
        End If
    End Sub

    Private Sub OnLineWidthMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                contextCursor.LineWidth = [Single].Parse(menuItem.Text)
            End If

            contextCursor = Nothing
        End If
    End Sub

    Private Sub OnLineWidthMenuPopup(ByVal sender As Object, ByVal e As PaintEventArgs)
        If Not (contextCursor Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim lineWidth As Double = [Double].Parse(menuItem.Text)

                If contextCursor.LineWidth = lineWidth Then
                    menuItem.Checked = True
                Else
                    menuItem.Checked = False
                End If
            End If
        End If
    End Sub

    Private Sub OnShapeZOrderMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextAnnotation Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim shapeZOrder As AnnotationZOrder = CType([Enum].Parse(GetType(AnnotationZOrder), menuItem.Text), AnnotationZOrder)

                If TypeOf contextAnnotation Is XYPointAnnotation Then
                    Dim contextPointAnnotation As XYPointAnnotation = CType(contextAnnotation, XYPointAnnotation)
                    contextPointAnnotation.ShapeZOrder = shapeZOrder
                End If
            End If

            contextAnnotation = Nothing
        End If
    End Sub

    Private Sub OnShapeZOrderMenuPopup(ByVal sender As Object, ByVal e As PaintEventArgs)
        If Not (contextAnnotation Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim shapeZOrder As AnnotationZOrder = CType([Enum].Parse(GetType(AnnotationZOrder), menuItem.Text), AnnotationZOrder)

                If TypeOf contextAnnotation Is XYPointAnnotation Then
                    Dim contextPointAnnotation As XYPointAnnotation = CType(contextAnnotation, XYPointAnnotation)

                    If contextPointAnnotation.ShapeZOrder = shapeZOrder Then
                        menuItem.Checked = True
                    Else
                        menuItem.Checked = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub OnArrowHeadStyleMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextAnnotation Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim arrowStyle As ArrowStyle = CType(EnumObject.Parse(GetType(ArrowStyle), menuItem.Text), ArrowStyle)
                contextAnnotation.ArrowHeadStyle = arrowStyle
            End If

            contextAnnotation = Nothing
        End If
    End Sub

    Private Sub OnArrowHeadStyleMenuPopup(ByVal sender As Object, ByVal e As PaintEventArgs)
        If Not (contextAnnotation Is Nothing) Then
            Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

            If Not (menuItem Is Nothing) Then
                Dim arrowStyle As ArrowStyle = CType(EnumObject.Parse(GetType(ArrowStyle), menuItem.Text), ArrowStyle)

                If contextAnnotation.ArrowHeadStyle.Equals(arrowStyle) Then
                    menuItem.Checked = True
                Else
                    menuItem.Checked = False
                End If
            End If
        End If
    End Sub

    Private Sub OnCaptionFontMenuClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not (contextAnnotation Is Nothing) Then
            Dim fontDialog As New FontDialog

            Try
                If fontDialog.ShowDialog() = DialogResult.OK Then
                    contextAnnotation.CaptionFont = fontDialog.Font
                End If
            Finally
                fontDialog.Dispose()
            End Try

            contextAnnotation = Nothing
        End If
    End Sub

    Private Sub MenuСhangeText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuСhangeText.Click
        TextBoxCaption.Location = pointMouse
        TextBoxCaption.Text = contextAnnotation.Caption

        If TextBoxCaption.Text.Length > 0 Then
            TextBoxCaption.Width = TextBoxCaption.Text.Length * 7
        Else
            TextBoxCaption.Width = 20
        End If

        TextBoxCaption.Visible = True
        TextBoxCaption.Focus()
    End Sub

    Private Sub TextBoxCaption_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxCaption.LostFocus
        TextBoxCaption.Visible = False
    End Sub

    Private Sub TextBoxCaption_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxCaption.TextChanged
        If IsHandleCreated Then
            If TextBoxCaption.Text.Length > 0 Then TextBoxCaption.Width = TextBoxCaption.Text.Length * 7
            contextAnnotation.Caption = TextBoxCaption.Text
            CType(contextAnnotation.Tag, Arrow).Legend = contextAnnotation.Caption
            UpdateListDescription()
        End If
    End Sub

#End Region
#End Region

    ''' <summary>
    ''' Отмена произведённых расшифровок снимка
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuComeBackToBeginning_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuComeBackToBeginning.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuComeBackToBeginning_Click)}> Отмена произведённых расшифровок снимка")
        Dim maximum As Double

        If (RegimeType = cРегистратор AndAlso FrequencyBackgroundSnapshot < FrequencyHandQuery) OrElse isRegimeChangeForDecoding Then
            maximum = arraysizeSnapshot
        Else ' если осциллограф то
            maximum = CDbl(ComboBoxTimeMeasurement.Text) * FrequencyHandQuery
        End If

        XAxisTime.Range = New Range(0, maximum)
        SlidePlot.Value = 1
        ClearArrowCollection()
    End Sub
    ''' <summary>
    ''' Показать форму проводника по базе произведённых записей
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuOpenBaseSnapshot_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuOpenBaseSnapshot.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuOpenBaseSnapshot_Click)}> Показать форму проводника по базе произведённых записей")
        GraphModeValue = MyGraphMode.Scaling
        SetGraphMode(GraphModeValue)
        CheckForCopyingBase(TypeWorkAutomaticBackup.Snapshot)
        OpenExplorerToDBaseAsync()
        SetEnabledMenuSimulator()
    End Sub
    ''' <summary>
    ''' Зафиксировать положение наклонной линии и принять ее координаты
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub ButtonFixLine_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonFixLine.Click
        If ComboBoxPointers.SelectedIndex = 2 Then
            isSlantingLine = False
            ButtonFixLine.Visible = False
        End If
    End Sub
    ''' <summary>
    ''' Удаление веделенной в списке стрелки
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub ButtonDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ButtonDelete.Click
        RemoveSelectedArrow()
    End Sub
    ''' <summary>
    ''' Изменить текст подписи расшифровки
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TextBoxDescriptionOnAxes_TextChanged(sender As Object, e As EventArgs) Handles TextBoxDescriptionOnAxes.TextChanged
        If ComboBoxPointers.SelectedIndex = 2 Then
            ButtonShowAxes.Enabled = Not (TextBoxDescriptionOnAxes.Text = "")
        End If
    End Sub
    ''' <summary>
    ''' Печать графика произведённых расшифровок
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuPrintGraph_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuPrintGraph.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuPrintGraph_Click)}> Печать графика произведённых расшифровок")
        Dim convertColor As ConvertColorForGraph = Nothing
        Dim printer As GraphPrinter

        Try
            If MenuGraphAlongTime.Checked = True Then
                convertColor = New ConvertColorForGraph(WaveformGraphTime, False)
                convertColor.ChangeColorOnGraph(Color.White, Color.Black)
                printer = New GraphPrinter(WaveformGraphTime)
            Else
                convertColor = New ConvertColorForGraph(ScatterGraphParameter, True)
                convertColor.ChangeColorOnGraph(Color.White, Color.Black)
                printer = New GraphPrinter(ScatterGraphParameter)
            End If

            printer.Print()
        Catch ex As Exception
            Const caption As String = "Ошибка печати графика"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        Finally
            convertColor.ChangeColorOnGraph(Color.Black, Color.White)
        End Try
    End Sub
    ''' <summary>
    ''' Запись графика произведённых расшифровок
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub MenuRecordGraph_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MenuRecordGraph.Click
        RegistrationEventLog.EventLog_MSG_USER_ACTION($"<{NameOf(MenuRecordGraph_Click)}> Запись графика произведённых расшифровок")
        Dim convertColor As ConvertColorForGraph = Nothing
        Dim graphSave As GraphSave

        Try
            If MenuGraphAlongTime.Checked = True Then
                convertColor = New ConvertColorForGraph(WaveformGraphTime, False)
                convertColor.ChangeColorOnGraph(Color.White, Color.Black)
                graphSave = New GraphSave(WaveformGraphTime)
            Else
                convertColor = New ConvertColorForGraph(ScatterGraphParameter, True)
                convertColor.ChangeColorOnGraph(Color.White, Color.Black)
                graphSave = New GraphSave(ScatterGraphParameter)
            End If

            graphSave.Save()
        Catch ex As Exception
            Const caption As String = "Ошибка записи графика"
            MessageBox.Show(ex.ToString, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {ex}")
        Finally
            convertColor.ChangeColorOnGraph(Color.Black, Color.White)
        End Try
    End Sub
    ''' <summary>
    ''' Добавить тики
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonAddTiks_Click(sender As Object, e As EventArgs) Handles ButtonAddTiks.Click
        ButtonAddTiks.Enabled = False
        ButtonDeleteTiks.Enabled = True

        If FrequencyBackgroundSnapshot / stepTic < 1 Then ' 2 Then ' ДобавкаНаШаге <2
            Exit Sub
        End If

        Dim tempRange As Range = XAxisTime.Range
        Dim countCustomDivisions As Integer = XAxisTime.CustomDivisions.Count
        Dim counter As Integer

        Do
            stepTic *= 2 ' 1.5
            ConfigureWaveformGraphScale(arraysizeSnapshot, FrequencyBackgroundSnapshot)
            counter += 1
            If counter > 3 Then Exit Do
        Loop While XAxisTime.CustomDivisions.Count <= countCustomDivisions

        XAxisTime.Range = tempRange
        ButtonAddTiks.Enabled = True
    End Sub
    ''' <summary>
    ''' Удалить тики
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonDeleteTiks_Click(sender As Object, e As EventArgs) Handles ButtonDeleteTiks.Click
        Dim tempRange As Range = XAxisTime.Range
        Dim countCustomDivisions As Integer = XAxisTime.CustomDivisions.Count
        Dim counter As Integer

        ButtonDeleteTiks.Enabled = False
        ButtonAddTiks.Enabled = True

        Do
            stepTic \= 2 '1.5
            If stepTic < 20 Then
                stepTic = 20
                Exit Sub
            End If

            ConfigureWaveformGraphScale(arraysizeSnapshot, FrequencyBackgroundSnapshot)
            counter += 1
            If counter > 15 Then Exit Do
        Loop While XAxisTime.CustomDivisions.Count >= countCustomDivisions

        XAxisTime.Range = tempRange
        ButtonDeleteTiks.Enabled = True
    End Sub

    ''' <summary>
    ''' Перестроить шкалу графика
    ''' capacity = arraysize
    ''' частотаОпроса = 100
    ''' </summary>
    ''' <param name="capacity"></param>
    ''' <param name="inFrequency"></param>
    Protected Sub ConfigureWaveformGraphScale(ByVal capacity As Integer, ByVal inFrequency As Single)
        'If axisХTickOrTime Then
        ' разделить диапазан на 20 равномерных частей
        Dim additionOnStep As Double = capacity / stepTic ' добавка на шаге
        Dim additionOnStepSec As Double ' добавка на шаге секунд
        Dim division As Double ' деления
        Dim endAxisSec As Double = capacity * (1 / inFrequency) ' конец оси сек

        XAxisTime.Range = New Range(0, capacity)
        XAxisTime.AutoMinorDivisionFrequency = 1
        XAxisTime.MajorDivisions.LabelFormat = New FormatString(FormatStringMode.Numeric, "G5")
        XAxisTime.MajorDivisions.LabelVisible = False
        XAxisTime.MajorDivisions.TickVisible = False

        If additionOnStep = 0 Then additionOnStep = 1

        ' округлить до целых
        If additionOnStep > 0 AndAlso additionOnStep <= 10 Then
            additionOnStep = 10
        ElseIf additionOnStep > 10 AndAlso additionOnStep <= 20 Then
            additionOnStep = 20
        ElseIf additionOnStep > 20 AndAlso additionOnStep <= 50 Then
            additionOnStep = 50
        ElseIf additionOnStep > 50 AndAlso additionOnStep <= 100 Then
            additionOnStep = 100

        ElseIf additionOnStep > 100 AndAlso additionOnStep <= 200 Then
            additionOnStep = 200
        ElseIf additionOnStep > 200 AndAlso additionOnStep <= 500 Then
            additionOnStep = 500
        ElseIf additionOnStep > 500 AndAlso additionOnStep <= 1000 Then
            additionOnStep = 1000

        ElseIf additionOnStep > 1000 AndAlso additionOnStep <= 2000 Then
            additionOnStep = 2000
        ElseIf additionOnStep > 2000 AndAlso additionOnStep <= 5000 Then
            additionOnStep = 5000
        ElseIf additionOnStep > 5000 AndAlso additionOnStep <= 10000 Then
            additionOnStep = 10000
        ElseIf additionOnStep > 10000 AndAlso additionOnStep <= 20000 Then
            additionOnStep = 20000
        ElseIf additionOnStep > 20000 AndAlso additionOnStep <= 100000 Then
            additionOnStep = 100000

        ElseIf additionOnStep > 100000 Then
            additionOnStep = 100000
        End If

        ' в каждой отсчёте содержится КоэфПрореж * (1 / FсбораГц) секунд поэтому
        additionOnStepSec = additionOnStep * (1 / inFrequency)
        ' округлим
        Dim roundDigit As Integer = 0

        If additionOnStepSec = 0 Then additionOnStepSec = 1

        If additionOnStepSec > 0 AndAlso additionOnStepSec <= 0.1 Then
            additionOnStepSec = Round(Round(additionOnStepSec * 100 + 0.5) / 100, 2)
            roundDigit = 2
        ElseIf additionOnStepSec > 0.1 AndAlso additionOnStepSec <= 1 Then
            additionOnStepSec = Round(Round(additionOnStepSec * 10 + 0.5) / 10, 1)
            roundDigit = 1
        ElseIf additionOnStepSec > 1 AndAlso additionOnStepSec <= 10 Then
            additionOnStepSec = CInt(Round(additionOnStepSec + 0.5))
        ElseIf additionOnStepSec > 10 AndAlso additionOnStepSec <= 100 Then
            additionOnStepSec = CInt((Round(additionOnStepSec / 10 + 0.5)) * 10)
        ElseIf additionOnStepSec > 100 AndAlso additionOnStepSec <= 1000 Then
            additionOnStepSec = CInt((Round(additionOnStepSec / 100 + 0.5)) * 100)
        ElseIf additionOnStepSec > 1000 AndAlso additionOnStepSec <= 10000 Then
            additionOnStepSec = CInt((Round(additionOnStepSec / 1000 + 0.5)) * 1000)
        ElseIf additionOnStepSec > 10000 Then
            additionOnStepSec = CInt((Round(additionOnStepSec / 10000 + 0.5)) * 10000)
        End If

        XAxisTime.CustomDivisions.Clear()
        'If endAxisSec < 20 Then
        '    additionOnStepSec = 1
        'ElseIf endAxisSec >= 10 AndAlso endAxisSec < 100 Then
        '    additionOnStepSec = 2
        'End If
        'Dim СчетчикПрогресса, DivisionCount As Integer
        'prgПрогресс.Value = 0
        'prgПрогресс.Visible = True
        'СчетчикПрогресса = 0
        'DivisionCount = endAxisSec / additionOnStepSec
        Do
            'AxisCustomDivision.TickColor = System.Drawing.Color.Red
            ' время с учетом коэф.
            Dim newAxisCustomDivision As AxisCustomDivision = New AxisCustomDivision() With {.TickColor = Color.Black,
                                                                                                .Text = division.ToString,
                                                                                                .Value = division * inFrequency}

            XAxisTime.CustomDivisions.Add(newAxisCustomDivision)
            division += additionOnStepSec
            division = Round(division, roundDigit)
            'I += additionOnStep
            'If СчетчикПрогресса Mod 100 = 0 Then
            '    prgПрогресс.Value = CInt((СчетчикПрогресса / DivisionCount) * 100)
            '    Application.DoEvents()
            'End If
            ' СчетчикПрогресса += 1
        Loop While division <= endAxisSec

        SlideTime.Range = New Range(0, XAxisTime.Range.Maximum / inFrequency)
    End Sub

    ''' <summary>
    ''' Видимость графиков
    ''' </summary>
    ''' <param name="N"></param>
    ''' <param name="inParameterType"></param>
    Private Sub SetEnableTrande(ByVal N As Integer, ByRef inParameterType() As TypeBaseParameter)
        ' только для режима регистратор
        Dim atartAxisX, endAxisX As Integer

        If IsRegimeIsRegistrator Then
            atartAxisX = CInt(XAxisTime.Range.Minimum)
            endAxisX = CInt(XAxisTime.Range.Maximum)

            For I As Integer = 1 To N
                WaveformGraphTime.Plots.Item(I - 1).Visible = inParameterType(IndexParameters(I)).IsVisible
            Next
            XAxisTime.Range = New Range(atartAxisX, endAxisX)
        End If
    End Sub
End Class

'Friend Sub ПостроитьСечения()
'    Dim числоСечений, I, J, N As Integer
'    Dim времяСечения As Double
'    Dim name As String
'    Dim временный As Double
'    Dim КоэПриведения As Double
'    Dim mItem As ListViewItem
'    Dim ЧастотаОпроса As Integer

'    ЧастотаОпроса = ЧастотаОпроса
'    If ПередЭтимБылаЗагрузкаБазы Then
'        КоэПриведения = System.Math.Sqrt(Const288 / (TemperatureOfBox + Kelvin))
'    Else
'        КоэПриведения = System.Math.Sqrt(Const288 / (TemperatureBoxInSnaphot + Kelvin))
'    End If

'    If IsNothing(arrСреднее) Then Exit Sub 'на случай если пустой

'    mFormVerticalSection = New FormVerticalSection
'    mFormVerticalSection.Show()
'    mFormVerticalSection.Activate()
'    mFormVerticalSection.ListViewGrid.Columns.Clear()
'    mFormVerticalSection.ListViewGrid.Items.Clear()

'    числоСечений = UBound(arrСечения, 2)
'    Dim strСтрокаФиз(числоСечений) As String
'    Dim strСтрокаПрив(числоСечений) As String

'    mFormVerticalSection.ListViewGrid.Columns.Add("Время", 50, HorizontalAlignment.Left)
'    For I = 1 To числоСечений
'        mFormVerticalSection.ListViewGrid.Columns.Add(arrСечения(1, I).ToString, 50, HorizontalAlignment.Right)
'    Next I
'    'здесь вычисляются физические, приведенные значения оборотов и а
'    For J = 1 To UBound(arrIndexParameters)
'        N = arrIndexParameters(J)
'        If ПередЭтимБылаЗагрузкаБазы Then
'            name = ParametersShaphotType(N).NameParameter
'        Else
'            name = ParametersType(N).NameParameter
'        End If
'        'поиск по номеру параметра соответствия имени
'        'N1физ
'        If name = conN1 Then
'            strСтрокаФиз(0) = "N1 физ."
'            strСтрокаПрив(0) = "N1 прив."
'            For I = 1 To числоСечений
'                времяСечения = arrСечения(1, I)
'                временный = arrСреднее(J - 1, времяСечения)
'                arrСечения(2, I) = временный
'                strСтрокаФиз(I) = Format(временный, "##0.0#")
'                'N1 приведенное
'                arrСечения(3, I) = временный * КоэПриведения
'                strСтрокаПрив(I) = Format(arrСечения(3, I), "##0.0#")
'            Next I
'            mItem = New ListViewItem(strСтрокаФиз)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'            mItem = New ListViewItem(strСтрокаПрив)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'        End If
'        'а1
'        If name = cona1 Then
'            strСтрокаФиз(0) = cona1
'            For I = 1 To числоСечений
'                времяСечения = arrСечения(1, I)
'                arrСечения(4, I) = arrСреднее(J - 1, времяСечения)
'                strСтрокаФиз(I) = Format(arrСечения(4, I), "##0.0#")
'            Next I
'            mItem = New ListViewItem(strСтрокаФиз)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'        End If
'        'N2физ
'        If name = conN2 Then
'            strСтрокаФиз(0) = "N2 физ."
'            strСтрокаПрив(0) = "N2 прив."
'            For I = 1 To числоСечений
'                времяСечения = arrСечения(1, I)
'                временный = arrСреднее(J - 1, времяСечения)
'                arrСечения(5, I) = временный
'                strСтрокаФиз(I) = Format(временный, "##0.0#")
'                'N2 приведенное
'                arrСечения(6, I) = временный * КоэПриведения
'                strСтрокаПрив(I) = Format(arrСечения(6, I), "##0.0#")
'            Next I
'            mItem = New ListViewItem(strСтрокаФиз)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'            mItem = New ListViewItem(strСтрокаПрив)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'        End If
'        'а2
'        If name = cona2 Then
'            strСтрокаФиз(0) = cona2
'            For I = 1 To числоСечений
'                времяСечения = arrСечения(1, I)
'                arrСечения(7, I) = arrСреднее(J - 1, времяСечения)
'                strСтрокаФиз(I) = Format(arrСечения(7, I), "##0.0#")
'            Next I
'            mItem = New ListViewItem(strСтрокаФиз)
'            mFormVerticalSection.ListViewGrid.Items.Add(mItem)
'        End If
'    Next J
'    'надо преобразовать абсолютное время в базовое от 0 и каждое значение минус декремент
'    'начало движения РУД принимается за 0
'    'временный = arrСечения(1, 1)'убрал нулевое значение начала построения сечения
'    временный = 0
'    For I = 1 To числоСечений
'        'arrСечения(1, I) = System.Math.Round((arrСечения(1, I) - временный) / ЧастотаОпроса, 3)
'        mFormVerticalSection.ListViewGrid.Columns(I).Text = System.Math.Round(arrСечения(1, I) / ЧастотаОпроса, 3).ToString
'    Next I
'    blnСечениеПостроено = True
'End Sub

'Friend Sub ПостроитьЛинииСечений()
'    Dim числоСечений, I As Integer
'    Dim времяСечения, sgnИнкремент As Single ' время для сечений
'    Dim plot As NationalInstruments.UI.WaveformPlot
'    XAxisTimeRange = New NationalInstruments.UI.Range(XAxisTime.Range.Minimum, XAxisTime.Range.Maximum)
'    ПромежутокСечения(sgnИнкремент, числоСечений, времяСечения)
'    ReDim_arrСечения(7, числоСечений)

'    For I = 1 To числоСечений
'        plot = СечениеЛиния()
'        GraphTime.Plots.Add(plot)

'        xData(0) = времяСечения
'        yData(0) = -1000
'        xData(1) = времяСечения
'        yData(1) = 1000
'        plot.PlotY(yData, xData(0), xData(1) - xData(0))
'        arrСечения(1, I) = времяСечения
'        времяСечения = времяСечения + sgnИнкремент
'    Next I
'    XAxisTime.Range = XAxisTimeRange
'End Sub

'Friend Sub ВставитьСечение(ByRef dblВремяСечения As Double)
'    Dim I, J As Integer

'    J = UBound(arrСечения, 2) + 1
'    ReDimPreserve arrСечения(7, J)
'    arrСечения(1, J) = dblВремяСечения
'    Dim x(J - 1) As Double
'    For I = 1 To J
'        x(I - 1) = arrСечения(1, I)
'    Next I
'    Array.Sort(x)
'    For I = 1 To J
'        arrСечения(1, I) = x(I - 1)
'    Next I
'End Sub

'Private Sub ПромежутокСечения(ByRef инкремент As Single, ByRef числоСечений As Integer, ByRef времяСечения As Single)
'    Dim промежуток As Double = XAxisTime.Range.Maximum - XyCursorStart.XPosition

'    времяСечения = XyCursorStart.XPosition

'    If NumberRegime >= 8 And NumberRegime <= 12 Then
'        инкремент = 0.2 * ЧастотаОпроса
'        числоСечений = Int(промежуток / инкремент) + 1
'    Else
'        инкремент = 0.5 * ЧастотаОпроса
'        числоСечений = Int(промежуток / инкремент) + 1
'    End If
'End Sub

'Private Sub ОчисткаМассиваСечений()
'    ReDim_arrСечения(7, 1)
'    arrСечения(1, 1) = 0 ' время
'    arrСечения(2, 1) = 0 '100 N1физ
'    arrСечения(3, 1) = 0 '100 'N1прив
'    arrСечения(4, 1) = 0 '100 'A1
'    arrСечения(5, 1) = 0 '100 'N2физ
'    arrСечения(6, 1) = 0 '100 'N2прив
'    arrСечения(7, 1) = 0 '100 'A2
'    сечениеПостроено = True
'End Sub