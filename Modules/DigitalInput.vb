Imports System.Threading
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports MathematicalLibrary

''' <summary>
''' ЦифровыеВходы
''' </summary>
Friend Class DigitalInput
    Private mDigitalInputs As Dictionary(Of String, ItemDigitalInput)
    Public ReadOnly Property DigitalInputs() As Dictionary(Of String, ItemDigitalInput)
        Get
            Return mDigitalInputs
        End Get
    End Property

    Private mDigitalInputsCount As Integer
    Public ReadOnly Property DigitalInputsCount() As Integer
        Get
            Return mDigitalInputsCount
        End Get
    End Property

    Private arrDigitalBaseParameter() As TypeBaseParameter
    Friend ReadOnly Property Item(ByVal strKey As String) As TypeBaseParameter
        Get
            Dim indexKey As Integer

            For I As Integer = 0 To UBound(arrDigitalBaseParameter)
                If strKey = arrDigitalBaseParameter(I).NameParameter Then
                    indexKey = I
                    Exit For
                End If
            Next

            Return arrDigitalBaseParameter(indexKey)
        End Get
    End Property

    Private Structure MyTypeChannel
        Dim NameParameter As String ' Наименование Параметра
        Dim RangeYmin As Short ' Разнос Умин
        Dim RangeYmax As Short ' Разнос Умакс
        Dim AlarmValueMin As Single ' Аварийное Значение Мин
        Dim AlarmValueMax As Single ' Аварийное Значение Макс
        Dim Blocking As Boolean ' Блокировка
        Dim IsVisible As Boolean ' Видимость
        Dim IsVisibleRegistration As Boolean ' Видимость Регистратор
    End Structure
    Private arrTypeChannel() As MyTypeChannel

    Public Sub New()
        MyBase.New()

        mDigitalInputs = New Dictionary(Of String, ItemDigitalInput)
    End Sub

    ''' <summary>
    ''' СчитатьПараметры
    ''' </summary>
    Public Sub LoadParameters()
        Dim I, J, countChannels, indexTag As Integer
        Dim nameParameter As String ' наименование Параметра
        Dim unit As String ' единица Измерения
        Dim strSQL As String = $"SELECT COUNT(*) FROM {ChannelLast} WHERE Погрешность={IndexDiscreteInput}"
        Dim cmd As OleDbCommand
        Dim rdr As OleDbDataReader
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim cb As OleDbCommandBuilder
        Dim cn As OleDbConnection = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))

        cn.Open()
        cmd = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        countChannels = CInt(cmd.ExecuteScalar)

        ' считать из базы Channels параметры Расчет и если есть копировать в массив признаки
        If countChannels > 0 Then
            'ReDim_arrTypeChannel(countChannels - 1)
            Re.Dim(arrTypeChannel, countChannels - 1)
            I = 0
            strSQL = "SELECT НаименованиеПараметра, Погрешность, РазносУмин, РазносУмакс, АварийноеЗначениеМин, АварийноеЗначениеМакс, Блокировка, Видимость, ВидимостьРегистратор " &
                    "FROM " & ChannelLast &
                    " WHERE Погрешность=" & IndexDiscreteInput.ToString
            ' 1000 признак цифровых входных каналов
            cmd.CommandText = strSQL
            rdr = cmd.ExecuteReader

            Do While (rdr.Read)
                arrTypeChannel(I).NameParameter = CStr(rdr("НаименованиеПараметра"))
                arrTypeChannel(I).RangeYmin = CShort(rdr("РазносУмин"))
                arrTypeChannel(I).RangeYmax = CShort(rdr("РазносУмакс"))
                arrTypeChannel(I).AlarmValueMin = CSng(rdr("АварийноеЗначениеМин"))
                arrTypeChannel(I).AlarmValueMax = CSng(rdr("АварийноеЗначениеМакс"))
                arrTypeChannel(I).Blocking = CBool(rdr("Блокировка"))
                arrTypeChannel(I).IsVisible = CBool(rdr("Видимость"))
                arrTypeChannel(I).IsVisibleRegistration = CBool(rdr("ВидимостьРегистратор"))
                I += 1
            Loop
            rdr.Close()

            ' 1 Выполнить команду на удаление каналов ChannelDigitalInput
            strSQL = $"DELETE * FROM {ChannelLast} WHERE Погрешность={IndexDiscreteInput}"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            Thread.Sleep(500)
            Application.DoEvents()

            ' считать и поиск по именам и обновить поля РазносУмин РазносУмакс АварийноеЗначениеМин АварийноеЗначениеМакс Блокировка Видимость ВидимостьРегистратор
            ' обновить базу
            strSQL = "SELECT * FROM ChannelDigitalInput;"
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            odaDataAdapter.Fill(dtDataTable)

            If dtDataTable.Rows.Count > 0 Then
                ' должны быть только по 1 на данном запуске
                For Each drDataRow As DataRow In dtDataTable.Rows
                    nameParameter = CStr(drDataRow("НаименованиеПараметра"))
                    For I = 0 To countChannels - 1
                        If arrTypeChannel(I).NameParameter = nameParameter Then
                            'drDataRow("НаименованиеПараметра") = arrПараметрыChannel(I).strНаименованиеПараметра
                            drDataRow("РазносУмин") = arrTypeChannel(I).RangeYmin
                            drDataRow("РазносУмакс") = arrTypeChannel(I).RangeYmax
                            drDataRow("АварийноеЗначениеМин") = arrTypeChannel(I).AlarmValueMin
                            drDataRow("АварийноеЗначениеМакс") = arrTypeChannel(I).AlarmValueMax
                            drDataRow("Блокировка") = arrTypeChannel(I).Blocking
                            drDataRow("Видимость") = arrTypeChannel(I).IsVisible
                            drDataRow("ВидимостьРегистратор") = arrTypeChannel(I).IsVisibleRegistration
                            Exit For
                        End If
                    Next I
                Next

                cb = New OleDbCommandBuilder(odaDataAdapter)
                odaDataAdapter.Update(dtDataTable)
                dtDataTable.AcceptChanges()
            End If
        End If 'countChannels > 0

        Thread.Sleep(500)
        Application.DoEvents()

        ' 4 В цикле пор тегам и по массиву arrКаналыДискретногоСлова заполняем массив arrПараметрыКаналовЦифровыхВходов и включаем видимость с ранее считанного
        mDigitalInputs.Clear()

        strSQL = "SELECT Count(*) FROM ChannelDigitalInput;"
        cmd = cn.CreateCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strSQL
        countChannels = CInt(cmd.ExecuteScalar)

        ' intCount = intRecordCount 'для данного случая где нет дискретных слов в теге
        'If countChannels > 0 Then ReDimarrDigitalBaseParameter(countChannels - 1)
        If countChannels > 0 Then Re.Dim(arrDigitalBaseParameter, countChannels - 1)

        strSQL = "SELECT * FROM ChannelDigitalInput;"
        I = 0
        cmd.CommandText = strSQL
        rdr = cmd.ExecuteReader

        If countChannels > 0 Then ' And intRecordCount > 0
            'ReDim_NameDigitalInputChannels(countChannels - 1)
            Re.Dim(NameDigitalInputChannels, countChannels - 1)

            Do While (rdr.Read)
                nameParameter = CStr(rdr("НаименованиеПараметра"))
                'sTemp = StrConv(LoadResData(sResName, sResType), vbUnicode)
                NameDigitalInputChannels(indexTag) = nameParameter ' идет от 0
                mDigitalInputs.Add(nameParameter, New ItemDigitalInput)
                indexTag += 1 ' увеличить номер тега

                With mDigitalInputs.Item(nameParameter)
                    .ParameterName = CStr(rdr("НаименованиеПараметра"))
                    .DeviseNumber = CShort(rdr("НомерУстройства"))

                    If Not IsDBNull(rdr("НомерМодуляКорзины")) Then
                        .ModuleNumderInChassis = CStr(rdr("НомерМодуляКорзины"))
                    Else
                        .ModuleNumderInChassis = ""
                    End If

                    .PortNumber = CShort(rdr("НомерПорта"))
                    .LineNumber = CShort(rdr("НомерЛинии"))

                    If Not IsDBNull(rdr("Примечание")) Then
                        arrDigitalBaseParameter(I).Description = CStr(rdr("Примечание"))
                    Else
                        arrDigitalBaseParameter(I).Description = "нет"
                    End If

                    unit = rdr("ЕдиницаИзмерения").ToString
                    .Unit = unit
                    .RangeMin = CSng(rdr("ДиапазонИзмененияMin"))
                    .RangeMax = CSng(rdr("ДиапазонИзмененияMax"))
                    .IndexInArray = I

                    arrDigitalBaseParameter(I).NumberChannel = CShort(I) ' для того чтобы знать индекс для этого канала в масииве типа
                    arrDigitalBaseParameter(I).NumberDevice = CShort(rdr("НомерУстройства")) ' indexTag

                    arrDigitalBaseParameter(I).NameParameter = CStr(rdr("НаименованиеПараметра"))
                    arrDigitalBaseParameter(I).UnitOfMeasure = unit
                    arrDigitalBaseParameter(I).LowerLimit = CSng(rdr("ДиапазонИзмененияMin"))
                    arrDigitalBaseParameter(I).UpperLimit = CSng(rdr("ДиапазонИзмененияMax"))
                    arrDigitalBaseParameter(I).RangeYmin = CShort(rdr("РазносУмин"))
                    arrDigitalBaseParameter(I).RangeYmax = CShort(rdr("РазносУмакс"))
                    arrDigitalBaseParameter(I).AlarmValueMin = CSng(rdr("АварийноеЗначениеМин"))
                    arrDigitalBaseParameter(I).AlarmValueMax = CSng(rdr("АварийноеЗначениеМакс"))
                    arrDigitalBaseParameter(I).Blocking = CBool(rdr("Блокировка"))
                    arrDigitalBaseParameter(I).IsVisible = CBool(rdr("Видимость"))
                    arrDigitalBaseParameter(I).IsVisibleRegistration = CBool(rdr("ВидимостьРегистратор"))

                    I += 1
                End With
            Loop
        End If
        rdr.Close()

        ' 5 делаем запрос на выборку(вставку) и добавляем параметры в базу ChannelNNN
        ' 1000 признак цифровых входных каналов
        strSQL = $"SELECT * FROM {ChannelLast} ORDER BY НомерПараметра" ' & " WHERE Погрешность=" & indexДискрВход.tostring
        odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
        dtDataTable = New DataTable
        odaDataAdapter.Fill(dtDataTable)
        J = CInt(dtDataTable.Rows(dtDataTable.Rows.Count - 1).Item("НомерПараметра")) + 1

        For I = 0 To countChannels - 1
            Dim drDataRow As DataRow = dtDataTable.NewRow
            drDataRow.BeginEdit()
            drDataRow("НомерПараметра") = J
            drDataRow("НаименованиеПараметра") = arrDigitalBaseParameter(I).NameParameter
            drDataRow("НомерКанала") = arrDigitalBaseParameter(I).NumberChannel
            drDataRow("НомерУстройства") = arrDigitalBaseParameter(I).NumberDevice
            drDataRow("НомерМодуляКорзины") = 0
            drDataRow("НомерКаналаМодуля") = 0
            drDataRow("ТипПодключения") = "DIFF"
            drDataRow("НижнийПредел") = 0
            drDataRow("ВерхнийПредел") = 0
            drDataRow("ТипСигнала") = "DC"
            drDataRow("НомерФормулы") = 3 '1
            drDataRow("СтепеньАппроксимации") = 1
            drDataRow("A0") = 0
            drDataRow("A1") = 0
            drDataRow("A2") = 0
            drDataRow("A3") = 0
            drDataRow("A4") = 0
            drDataRow("A5") = 0
            drDataRow("Смещение") = 0

            drDataRow("КомпенсацияХС") = False
            drDataRow("ЕдиницаИзмерения") = arrDigitalBaseParameter(I).UnitOfMeasure
            drDataRow("ДопускМинимум") = arrDigitalBaseParameter(I).LowerLimit
            drDataRow("ДопускМаксимум") = arrDigitalBaseParameter(I).UpperLimit
            drDataRow("РазносУмин") = arrDigitalBaseParameter(I).RangeYmin
            drDataRow("РазносУмакс") = arrDigitalBaseParameter(I).RangeYmax
            drDataRow("АварийноеЗначениеМин") = arrDigitalBaseParameter(I).AlarmValueMin
            drDataRow("АварийноеЗначениеМакс") = arrDigitalBaseParameter(I).AlarmValueMax
            drDataRow("Блокировка") = arrDigitalBaseParameter(I).Blocking
            drDataRow("Дата") = Date.Today.ToShortDateString

            drDataRow("Видимость") = arrDigitalBaseParameter(I).IsVisible
            drDataRow("ВидимостьРегистратор") = arrDigitalBaseParameter(I).IsVisibleRegistration
            drDataRow("Погрешность") = IndexDiscreteInput
            drDataRow("Примечания") = Left(arrDigitalBaseParameter(I).Description, 99) '100)
            drDataRow.EndEdit()
            dtDataTable.Rows.Add(drDataRow)
            J += 1
        Next

        cb = New OleDbCommandBuilder(odaDataAdapter)
        odaDataAdapter.Update(dtDataTable)
        cn.Close()

        Thread.Sleep(500)
        Application.DoEvents()

        ' 6 организуем массив с для DigitalInputValue() as double
        'ReDim_DigitalInputValue(countChannels - 1)
        Re.Dim(DigitalInputValue, countChannels - 1)

        mDigitalInputsCount = countChannels

        If mDigitalInputsCount > 0 Then IsDigitalInput = True

        ' 7 при открытии формы в цикле по числу тегов  загружаем массив OPCDataSocket настрамваем по тегам
        ' 8 при пуске сбора запускаем таймер в событии которого по индексу считываем значение а по тегу ищем
        ' в mvarКоллекцияItemЦифровойВход и
        ' если  РазвернутьДискретноеСлово=False по индексу ИндексВМассивеЗначений записать вычисленное значени в DigitalInputValue
        ' если свойство тега РазвернутьДискретноеСлово=True, то через Item(I).ИндексВМассивеЗначений
        ' производится по маске считывание бита и значение 0 или 1 записать в DigitalInputValue

        '9 в событии измерения если работа с Расчет данные из DigitalInputValue дополняют массив

        '                If arrПараметры(arrСписокПараметров(vKey)).blnВидимость Or myTypeList(vKey).blnВидимость Then
        '                    arrСреднееПересчитанный(J, 0) = ПриведениеКОсиЭталона(номерПараметраОсиЭталона, vKey, dblСреднее) 'массив приведен к какой-то шкале
        '                End If
        '
        '                'запись в массив среднего, код такой-же как SweepChart
        '                arrСреднее(J, x) = dblСреднее

        ' 10 При конфигурации CWAI и определении ЧислоВсехИзмеряемыхПараметров убрать каналы Расчет
    End Sub
End Class