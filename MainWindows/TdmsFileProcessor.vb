Imports System.ComponentModel
Imports NationalInstruments.Tdms
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports MathematicalLibrary

' если прерванный снимок, то из внешней программы вначале сделать AppendData, а затем CloseTDMSFile

Friend Class TdmsFileProcessor
    Implements IDisposable

    Public Event ContinueAccumulation(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ClosedTDMSFileEvent(ByVal sender As Object, ByVal e As ClosedTDMSFileEventArgs)
    ''' <summary>
    ''' Пользовательское событие наследуется от EventArgs.
    ''' </summary>
    Public Class ClosedTDMSFileEventArgs
        Inherits EventArgs

        ' получить массив собранных значений
        Public Sub New(ByVal today As String,
                       ByVal timeStartCollect As Double,
                       ByVal rowsCount As String,
                       ByVal columnsCount As String,
                       ByVal pathFile As String,
                       configurationString As String,
                       isSnapshotComplete As Boolean)
            Me.DateTimeToday = today
            Me.TimeStartCollect = timeStartCollect ' время Начала Сбора
            Me.RowsCount = rowsCount ' кол Строк
            Me.ColumnsCount = columnsCount ' кол Столбцов
            Me.PathFile = pathFile ' путь файла
            Me.ConfigurationString = configurationString ' конфигурация
            Me.IsSnapshotComplete = isSnapshotComplete ' кадр Полный
        End Sub

        ' открытые свойства для их записи в базу данных
        ''' <summary>
        ''' Дата начала сбора
        ''' </summary>
        ''' <returns></returns>
        Public Property DateTimeToday As String
        ''' <summary>
        ''' Время начала сбора
        ''' </summary>
        ''' <returns></returns>
        Public Property TimeStartCollect As Double
        ''' <summary>
        ''' Количество строк
        ''' </summary>
        ''' <returns></returns>
        Public Property RowsCount As String
        ''' <summary>
        ''' Количество столбцов
        ''' </summary>
        ''' <returns></returns>
        Public Property ColumnsCount As String
        ''' <summary>
        ''' Путь файла
        ''' </summary>
        ''' <returns></returns>
        Public Property PathFile As String
        ''' <summary>
        ''' Конфигурация
        ''' </summary>
        ''' <returns></returns>
        Public Property ConfigurationString As String
        ''' <summary>
        ''' Кадр полный
        ''' </summary>
        ''' <returns></returns>
        Public Property IsSnapshotComplete As Boolean
    End Class

    ''' <summary>
    ''' Имя файла требуется при записи снимка или расшифрованного снимка
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TdmsFileName() As String
        Get
            Return fileName
        End Get
    End Property

    Private mTdmsFile As TdmsFile = Nothing
    Private timing As WaveformTiming ' временная шкала в зависимости от частоты
    Private sampleIntervalMode As WaveformSampleIntervalMode = WaveformSampleIntervalMode.Regular ' тип времени - просто набор, фиксированная частота или каждому замеру своя временная отметка
    Private channelGroup As TdmsChannelGroup
    Private waveformChannels As TdmsChannel()
    Private fileOptions As TdmsFileOptions

    ' для снимков и фоновых
    ''' <summary>
    ''' индексы Параметров
    ''' </summary>
    Private IndexesParameters As Integer()
    Private arrTypeBaseParameter As TypeBaseParameter()
    ''' <summary>
    ''' для Изменённых Для Расшифровки
    ''' </summary>
    Private Names As String()
    Private arrTypeSmallParameter As TypeSmallParameter()
    Private configurationChannels As String

    Private WithEvents DataAppendBackgroundWorker As BackgroundWorker ' необходимо подписаться на событие по окончанию записи в данных файл, чтобы так-же сделать соответствующую запись в базе данных
    Private baseTime As DateTime = DateTime.Now
    ''' <summary>
    ''' Добавлений не более
    ''' </summary>
    Private limitAddedBlocks As Integer
    Private frequency As Double
    ''' <summary>
    ''' Счетчик добавлений в файл
    ''' </summary>
    Private countAddedBlocksToFile As Integer
    Private fileName As String = vbNullString
    Private pathFile As String = vbNullString

    Private title, description As String
    Private numberOfChannelsConfigure As Integer
    ''' <summary>
    ''' Для ЗаписьНаДискКадраРегистратора
    ''' </summary>
    Private todayString As String
    ''' <summary>
    ''' Время начала сбора
    ''' </summary>
    Private timeStartCollectNumeric As Double
    Private countRowsMeasuredValues As Integer ' в аргументе ClosedTDMSFileEventArgs
    Public countRowsAdded As Integer ' в аргументе ClosedTDMSFileEventArgs
    Public Property IsCloseTDMSFile As Boolean
    ''' <summary>
    ''' Проверка имен каналов при счтитывании файла
    ''' </summary>
    Private channelNames As List(Of String)
    ''' <summary>
    ''' Использовать событие записи
    ''' </summary>
    Private isRiseEventsSaveRecord As Boolean
    ''' <summary>
    ''' кадр Полный
    ''' </summary>
    Private isSnapshotFull As Boolean

    Sub New()
        Me.New(10, 20, WaveformSampleIntervalMode.Regular, False)
    End Sub

    Sub New(ByVal inLimitAddedBlocks As Integer, ByVal frequency As Double, ByVal intervalMode As WaveformSampleIntervalMode, ByVal isRiseEventsSave As Boolean)
        Me.DataAppendBackgroundWorker = New BackgroundWorker()
        Me.limitAddedBlocks = inLimitAddedBlocks
        Me.frequency = frequency
        Me.isRiseEventsSaveRecord = isRiseEventsSave
        sampleIntervalMode = intervalMode
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' флаг не более одного раза, чтобы обнаружить избыточные вызовы

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                'сборщик мусора не должен иметь сюда доступ
                ' освободить управляемое состояние (управляемые объекты).
            End If

            ' освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже Finalize().
            ' задать большие поля как null.
            CloseTDMSFile()
        End If
        Me.disposedValue = True
    End Sub

    'деструктор вызванный сборщиком мусора
    ' переопределить Finalize(), только если Dispose(ByVal disposing As Boolean) выше имеет код для освобождения неуправляемых ресурсов.
    'Protected Overrides Sub Finalize()
    '    ' Не изменяйте этот код.  Поместите код очистки в расположенную выше команду Удалить(ByVal удаление как булево).
    '    Dispose(False)
    '    MyBase.Finalize()

    'где-то сделано так:
    '    Try
    '    Dispose(False)
    'Finally
    '    MyBase.Finalize()
    'End Try
    'End Sub

    ' Этот код добавлен редактором Visual Basic для правильной реализации шаблона высвобождаемого класса.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Не изменяйте этот код. Разместите код очистки выше в Dispose(ByVal disposing As Boolean).
        Dispose(True) 'это вызывает пользователь, значит все управляемуе и неуправляемые ресурсы ассоциированные с данным объектом должны быть очищены
        GC.SuppressFinalize(Me) 'сообщает о том, что класс не нуждается в вызове деструктора(сборщик мусора вызывать ещё не нужно)
    End Sub
#End Region

    ''' <summary>
    ''' вызывается из ЗаписьНаДискКадраРегистратора и ЗаписьНаДиск
    ''' </summary>
    ''' <param name="measuredValues"></param>
    ''' <remarks></remarks>
    Public Sub AppendData(ByRef measuredValues(,) As Double)
        timing = GenerateTiming()
        countRowsMeasuredValues = UBound(measuredValues, 2)
        DataAppendBackgroundWorker.RunWorkerAsync(measuredValues)
    End Sub

    ''' <summary>
    ''' вызывается из 1) КнопкаЗапись (CharacteristicForRegime->Непрерывный - там только настройки входных массивов для Configure) 
    ''' 2) из Снимок-> ИнициализацияДляСнимка далее обработка собранных данных и в итоге ЗаписьНаДиск сдесь вызов Configure для 200 гц снимков и здесь сразу AppendData а затем ClosedTDMSFile
    ''' 3) СчитатьСДиска
    ''' </summary>
    ''' <param name="inPathFile"></param>
    ''' <param name="title"></param>'Title = lngНомерИзделия
    ''' <param name="description"></param>'Description = strМодификация или strПримечаниеСнимка
    ''' <param name="arrIndexParameters"></param>
    ''' <param name="arrTypeBaseParameter"></param>
    ''' <remarks></remarks>
    Public Sub Configure(ByVal inPathFile As String,
                         ByVal title As String,
                         ByVal description As String,
                         ByVal arrIndexParameters() As Integer,
                         ByVal arrTypeBaseParameter() As TypeBaseParameter)
        If mTdmsFile IsNot Nothing AndAlso mTdmsFile.IsOpen Then
            Exit Sub
        End If

        IndexesParameters = CType(arrIndexParameters.Clone, Integer()) ' номера записываемых каналов
        Me.arrTypeBaseParameter = CType(arrTypeBaseParameter.Clone, TypeBaseParameter()) ' для считывания атрибутов
        Me.title = title
        Me.description = description
        Me.pathFile = inPathFile
        numberOfChannelsConfigure = UBound(IndexesParameters) ' arrIndexParameters начинается с 0, а каналы с 1

        Dim ex As Exception = Nothing
        Try
            SetUpTDMSFile() ' нужно создать заранее для конфигурирования каналов
        Catch tdmsException As Exception
            ex = tdmsException
        Finally
            If ex IsNot Nothing Then
                Dim caption As String = $"Ошибка конфигурирования файла в процедуре {NameOf(Configure)}"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Режим изменен при расшифровке - открыт снимок из архива
    ''' 2) из Снимок-> ИнициализацияДляСнимка далее обработка собранных данных и в итоге ЗаписьНаДиск сдесь вызов Configure для 200 гц снимков и здесь сразу AppendData а затем ClosedTDMSFile
    ''' 3) СчитатьСДиска    ''' </summary>
    ''' <param name="inPathFile"></param>
    ''' <param name="title"></param>
    ''' <param name="description"></param>
    ''' <param name="namesParameterRegime"></param>
    ''' <param name="snapshotSmallParameters"></param>
    Public Sub Configure(ByVal inPathFile As String,
                         ByVal title As String,
                         ByVal description As String,
                         ByVal namesParameterRegime() As String,
                         ByVal snapshotSmallParameters() As TypeSmallParameter)
        Names = CType(namesParameterRegime.Clone, String()) ' номера записываемых каналов
        Me.arrTypeSmallParameter = CType(snapshotSmallParameters.Clone, TypeSmallParameter()) ' для считывания атрибутов
        Me.title = title
        Me.description = description
        Me.pathFile = inPathFile
        numberOfChannelsConfigure = UBound(Names) ' arrIndexParameters начинается с 0, а каналы с 1

        Dim ex As Exception = Nothing
        Try
            SetUpTDMSFileForDecoding() ' нужно создать заранее для конфигурирования каналов
        Catch tdmsException As Exception
            ex = tdmsException
        Finally
            If ex IsNot Nothing Then
                Dim caption As String = $"Ошибка конфигурирования файла в процедуре {NameOf(Configure)}"
                Dim text As String = ex.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Вызов при конфигурировании и после записи накопленного кадра для продолжения записи
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUpTDMSFile()
        ' обычно даже маленький остаточек будет в файле и должен быть записан при выключении кнопки запись, новой конфигурации, и закрытия окна и всего приложения
        Dim nowToLongTime As String = $"({Now.Hour}ч{Now.Minute}м{Now.Second}с{Now.Millisecond}мс)"
        fileName = $"{pathFile}\База снимков\{title}-{ModificationEngine} [{Today.ToShortDateString} {nowToLongTime}] {description}.tdms"
        todayString = Today.ToString
        timeStartCollectNumeric = TimeOfDay.ToOADate

        ' удалить если файл случайно существует 
        If File.Exists(fileName) Then TdmsFile.Delete(fileName)

        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Создание файла: " & fileName)

        ' настройть опции TDMS файла
        If fileOptions Is Nothing Then
            ' для повышения производительности
            fileOptions = New TdmsFileOptions() With {.FileFormat = TdmsFileFormat.Version20, .BufferingDisabled = True}
        End If

        mTdmsFile = CreateNewTDMSFile()
        channelGroup = mTdmsFile.GetChannelGroups(0) ' группа всегда одна
        Dim tdmsChannels As TdmsChannelCollection = channelGroup.GetChannels() ' сослаться на коллекцию всех каналов группы
        waveformChannels = New TdmsChannel(numberOfChannelsConfigure - 1) {} ' создать массив каналов

        'If sampleIntervalModeComboBox.SelectedItem.Equals(WaveformSampleIntervalMode.Irregular.ToString()) Then '"Irregular"
        If sampleIntervalMode = WaveformSampleIntervalMode.Irregular Then
            channelGroup.WaveformLayout = TdmsWaveformLayout.PairedTimeAndSampleChannels
        Else '"None"'"Regular"
            channelGroup.WaveformLayout = TdmsWaveformLayout.NoTimeChannel
        End If

        isSnapshotFull = False
        configurationChannels = vbNullString

        For I As Integer = 0 To numberOfChannelsConfigure - 1
            ' коллекция channels необходимая для записи waveforms. 
            ' Для waveforms с RegularTiming или NoTiming, использовать один channelдля каждого waveform. Канал хранит просто значения. 
            ' Для waveforms с IrregularTiming, использовать два канала для каждого waveform. первый канал хранит time values 
            ' второй канал хранит sample values. 
            If channelGroup.WaveformLayout = TdmsWaveformLayout.PairedTimeAndSampleChannels Then
                ' Настроить time channel. 
                ' каждый временной канал должен предшествовать соответствующему waveform sample channel
                Dim timeChannelName As String = "Time Channel " & I.ToString()
                Dim timeChannel As New TdmsChannel(timeChannelName, TdmsDataType.DateTime)
                tdmsChannels.Add(timeChannel)
            End If

            Dim channelName As String = arrTypeBaseParameter(IndexesParameters(I + 1)).NameParameter
            waveformChannels(I) = New TdmsChannel(channelName, TdmsDataType.Double) With {.Description = arrTypeBaseParameter(IndexesParameters(I + 1)).Description,
                                                                                            .UnitString = arrTypeBaseParameter(IndexesParameters(I + 1)).UnitOfMeasure}

            configurationChannels &= channelName & "\"

            ' если канал был создан можно проверить
            'If (tdmsChannels.Contains(channelName)) Then
            '    channel = tdmsChannels(channelName)
            'Else
            '    tdmsChannels.Add(channel)
            'End If

            tdmsChannels.Add(waveformChannels(I))
        Next

        countAddedBlocksToFile = 0
        countRowsAdded = 0
        IsCloseTDMSFile = False
    End Sub

    ''' <summary>
    ''' Вызов при конфигурировании для расшифровки при смененной базе данных
    ''' </summary>
    Private Sub SetUpTDMSFileForDecoding()
        ' обычно даже маленький остаточек будет в файле и должен быть записан при выключении кнопки запись, новой конфигурации, и закрытия окна и всего приложения
        Dim nowToLongTime As String = $"({Now.Hour}ч{Now.Minute}м{Now.Second}с{Now.Millisecond}мс)"
        fileName = $"{pathFile}\База снимков\{title} {Today.ToShortDateString} {nowToLongTime} {description}.tdms"

        'lngНомерИзделия.ToString & ",'" & Today.ToString & "'," & [Время].ToOADate & "," & sgnТемператураБокса.ToString & ",'" & strТипКрд & "','" & режим & "','" & strСтрокаКонфигурации & "'," & chКолСтрок.ToString & "," & UBound(arrСреднее).ToString & "," & intЧастотаФонового.ToString & "," & CStr(XAxisTime.Range.Minimum) & "," & dblMaximum.ToString & ",'" & strПутьТекстовогоПотока & "','" & strChannelПоследняя & "','" & strПримечаниеСнимка & "')"
        todayString = Today.ToString
        timeStartCollectNumeric = TimeOfDay.ToOADate

        ' удалить если файл случайно существует 
        If File.Exists(fileName) Then TdmsFile.Delete(fileName)

        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Создание файла: " & fileName)

        ' настройть опции TDMS файла
        If fileOptions Is Nothing Then
            ' для повышения производительности
            fileOptions = New TdmsFileOptions() With {.FileFormat = TdmsFileFormat.Version20, .BufferingDisabled = True}
        End If

        mTdmsFile = CreateNewTDMSFile()
        channelGroup = mTdmsFile.GetChannelGroups(0) ' группа всегда одна
        Dim tdmsChannels As TdmsChannelCollection = channelGroup.GetChannels() ' сослаться на коллекцию всех каналов группы
        waveformChannels = New TdmsChannel(numberOfChannelsConfigure - 1) {} ' создать массив каналов

        'If sampleIntervalModeComboBox.SelectedItem.Equals(WaveformSampleIntervalMode.Irregular.ToString()) Then '"Irregular"
        If sampleIntervalMode = WaveformSampleIntervalMode.Irregular Then
            channelGroup.WaveformLayout = TdmsWaveformLayout.PairedTimeAndSampleChannels
        Else '"None"'"Regular"
            channelGroup.WaveformLayout = TdmsWaveformLayout.NoTimeChannel
        End If

        For I As Integer = 0 To numberOfChannelsConfigure - 1
            For shПараметры As Integer = 1 To UBound(arrTypeSmallParameter)
                If arrTypeSmallParameter(shПараметры).NameParameter = Names(I + 1) Then
                    ' коллекция channels необходимая для записи waveforms. 
                    ' Для waveforms с RegularTiming или NoTiming,использовать один channelдля каждого waveform. Канал хранит просто значения. 
                    ' Для waveforms с IrregularTiming,использовать два канала для каждого waveform. первый канал хранит time values 
                    ' второй канал хранит sample values. 
                    If channelGroup.WaveformLayout = TdmsWaveformLayout.PairedTimeAndSampleChannels Then
                        ' Настроить time channel. 
                        ' каждый временной канал должен предшествовать соответствующему waveform sample channel
                        tdmsChannels.Add(New TdmsChannel("Time Channel " & I.ToString(), TdmsDataType.DateTime))
                    End If

                    Dim channelName As String = arrTypeSmallParameter(shПараметры).NameParameter
                    waveformChannels(I) = New TdmsChannel(channelName, TdmsDataType.Double) With {.Description = arrTypeSmallParameter(shПараметры).NameParameter,
                                                                                                .UnitString = arrTypeSmallParameter(shПараметры).UnitOfMeasure}

                    tdmsChannels.Add(waveformChannels(I))
                    Exit For
                End If
            Next shПараметры
        Next

        countAddedBlocksToFile = 0
        countRowsAdded = 0
        IsCloseTDMSFile = False
    End Sub

    ''' <summary>
    ''' Создание на диске и открыти в памяти file с специфицированными options
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateNewTDMSFile() As TdmsFile
        Dim file As TdmsFile = New TdmsFile(fileName, fileOptions) With {.AutoSave = True,
                                                                          .Name = fileName,
                                                                          .Author = "Victor",
                                                                          .Title = title,
                                                                          .Description = description}
        file.AddProperty("Frequency", TdmsPropertyDataType.Double, frequency)

        ' Установить единственную channel group. 
        Dim channelGroups As TdmsChannelGroupCollection = file.GetChannelGroups()
        Dim channelGroup As TdmsChannelGroup = New TdmsChannelGroup("Main Group") With {.Description = "Main Group"}
        'channelGroup.Name=""

        ' проверка не была ли группа создана
        'If (channelGroups.Contains(channelGroupName)) Then
        '    channelGroup = channelGroups(channelGroupName)
        'Else
        '    channelGroups.Add(channelGroup)
        'End If

        channelGroups.Add(channelGroup)

        Return file
    End Function

    ''' <summary>
    ''' Сброс из памяти и закрытие файла
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseTDMSFile()
        If mTdmsFile IsNot Nothing AndAlso mTdmsFile.IsOpen Then
            ' подождём пока поток исполнит работу
            Do While DataAppendBackgroundWorker.IsBusy
                Thread.Sleep(10)
                Application.DoEvents()
            Loop

            ' если второй раз вызывается из ожидающего потока то прервать
            If Not mTdmsFile.IsOpen Then Exit Sub

            mTdmsFile.Close()

            'If countRowsAdded = 0 Then
            '    ' значит сработал лишнее событие и был создан пустой файл, который нужно удалить
            '    DeleteTextFile(file.Path)
            '    DeleteTextFile(file.Path & "_index")
            'Else
            ' запись в базу снимков по событию
            '       strSQL = "INSERT INTO БазаСнимков (НомерИзделия, Дата, ВремяНачалаСбора, Тбокса, ТипКРД, Режим, СтрокаКонфигурации, КолСтрок, КолСтолбцов, ЧастотаОпроса, НачалоОсиХ, КонецОсиХ, ПутьНаДиске, ТаблицаКаналов, Примечание) VALUES (" &
            'lngНомерИзделия.ToString & ",'" & Today.ToString & "'," & [Время].ToOADate & "," & sgnТемператураБокса.ToString & ",'" & strТипКрд & "','" & режим & "','" & strСтрокаКонфигурации & "'," & chКолСтрок.ToString & "," & UBound(arrСреднее).ToString & "," & intЧастотаФонового.ToString & "," & CStr(XAxisTime.Range.Minimum) & "," & dblMaximum.ToString & ",'" & strПутьТекстовогоПотока & "','" & strChannelПоследняя & "','" & strПримечаниеСнимка & "')"
            If isRiseEventsSaveRecord Then
                Dim fireAcquiredDataEventArgs As New ClosedTDMSFileEventArgs(todayString, timeStartCollectNumeric, countRowsAdded.ToString, (numberOfChannelsConfigure - 1).ToString, fileName, configurationChannels, isSnapshotFull)
                ' Теперь вызов события с помощью вызова делегата. Проходя в
                ' object которое инициирует  событие (Me) с аргументом fireAcquiredDataEventArgs. 
                ' Вызов обязан соответствовать сигнатуре ClosedTDMSFileEvent.
                RaiseEvent ClosedTDMSFileEvent(Me, fireAcquiredDataEventArgs)
            End If
        End If
        'End If
    End Sub

    ''' <summary>
    ''' проверка завершения потока делегирует к DataAppendBackgroundWorker.IsBusy
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsBusy() As Boolean
        Get
            Return DataAppendBackgroundWorker.IsBusy
        End Get
    End Property

    ''' <summary>
    ''' считывание данных снимка
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadTDMSFile(ByVal path As String) As Double(,)
        Dim data As Double()
        Dim dataChannels As Double(,) = Nothing
        Dim channelsCount, channelDataCount As Integer

        If mTdmsFile IsNot Nothing Then
            mTdmsFile.Close()
        End If

        Try
            mTdmsFile = New TdmsFile(path, New TdmsFileOptions(TdmsFileFormat.Version20, TdmsFileAccess.Read))

            ' получить имя file.Name
            'Dim fileName As String = file.Name
            'If fileName = String.Empty Then
            '    Dim fullPath As String = file.Path
            '    Dim fileInfo As IO.FileInfo = New IO.FileInfo(fullPath)
            '    fileName = fileInfo.Name
            'End If

            ' получить channel group.
            Dim tdmsChannelGroups As TdmsChannelGroupCollection = mTdmsFile.GetChannelGroups()
            Dim tdmsChannelGroup As TdmsChannelGroup = tdmsChannelGroups(0)

            ' For Each tdmsChannelGroup As TdmsChannelGroup In tdmsChannelGroups
            Dim channelGroupName As String = tdmsChannelGroup.Name
            If channelGroupName = String.Empty Then
                channelGroupName = "Main Group" ' String.Format("Group {0}", channelGroupIndex.ToString())
            End If
            'channelGroupIndex = channelGroupIndex + 1

            'Dim channelGroupProperties As TdmsPropertyCollection = tdmsChannelGroup.GetProperties()
            'For Each channelGroupProperty As TdmsProperty In channelGroupProperties
            '    'tdmsPropertiesDataGridView.Rows.Add(channelGroupProperty.Name, channelGroupProperty.GetValue())
            'Next

            Dim channelIndex As Integer = 0
            Dim tdmsChannels As TdmsChannelCollection = tdmsChannelGroup.GetChannels()
            '' можно пройти по массиву AnalogWaveform или сразу передать массив на график
            'Dim waveform As AnalogWaveform(Of Double) = Nothing
            'Dim waveformArr As AnalogWaveform(Of Double)() = Nothing
            'waveformArr = channelGroup.GetAnalogWaveforms(Of Double)()
            'For Each tmpWaveform As AnalogWaveform(Of Double) In waveformArr
            '    Dim data() As Double = waveform.GetRawData
            'Next

            If tdmsChannels.Count > 0 Then
                channelsCount = tdmsChannels.Count - 1
                channelNames = New List(Of String)

                If tdmsChannels(0).DataCount > 0 Then
                    channelDataCount = CInt(tdmsChannels(0).DataCount - 1)
                    Re.Dim(dataChannels, channelsCount, channelDataCount)

                    For Each tdmsChannel As TdmsChannel In tdmsChannels
                        ' можно считать через следующие именованные свойства - name, description, unit_string
                        Dim channelName As String = tdmsChannel.Name
                        Dim channelProperties As TdmsPropertyCollection = tdmsChannel.GetProperties()
                        Dim dataType As TdmsDataType = tdmsChannel.TdmsDataType

                        If channelName = String.Empty Then
                            Dim channelNameProperty As TdmsProperty = channelProperties("NI_ChannelName")
                            If channelNameProperty IsNot Nothing Then
                                channelName = channelNameProperty.GetValue().ToString()
                            Else
                                channelName = String.Format("Channel {0}", channelIndex & 1.ToString())
                            End If
                            'For Each channelProperty As TdmsProperty In channelProperties
                            '    tdmsPropertiesDataGridView.Rows.Add(channelProperty.Name, channelProperty.GetValue())
                            'Next
                        End If

                        If channelProperties.Contains("wf_samples") Then ' wf_samples - кол элементов в порции
                            If dataType = TdmsDataType.Double Then
                                ' можно сразу получить AnalogWaveform для вывода на график
                                ' waveform = tdmsChannel.Parent.GetAnalogWaveform(Of Double)(tdmsChannel)

                                'If waveform.Timing.SampleIntervalMode = WaveformSampleIntervalMode.None Then
                                '    tdmsXAxis.MajorDivisions.LabelFormat = New FormatString(FormatStringMode.Numeric, "g")
                                '    Dim options As AnalogWaveformPlotOptions = New AnalogWaveformPlotOptions(AnalogWaveformPlotDisplayMode.Samples, AnalogWaveformPlotScaleMode.Raw)
                                '    tdmsWaveformGraph.PlotWaveform(waveform, options)
                                'Else
                                '    tdmsXAxis.MajorDivisions.LabelFormat = New FormatString(FormatStringMode.DateTime, "g")
                                '    Dim options As AnalogWaveformPlotOptions = New AnalogWaveformPlotOptions(AnalogWaveformPlotDisplayMode.Time, AnalogWaveformPlotScaleMode.Raw, AnalogWaveformPlotTimingMode.Auto)
                                '    tdmsWaveformGraph.PlotWaveform(waveform, options)
                                'End If

                                'data = waveform.GetRawData ' получить данные из waveform
                                data = tdmsChannel.GetData(Of Double)() ' получить данные из tdmsChannel
                                For J As Integer = 0 To channelDataCount ' накопить в массиве
                                    dataChannels(channelIndex, J) = data(J)
                                Next
                            End If
                        End If

                        'Dim data() As Object = tdmsChannel.GetData(dataIndex, count)
                        'If Not data Is Nothing Then
                        '    For i As Integer = 0 To data.Length - 1
                        '        tdmsDataGridView.Rows.Add(dataIndex + i, data(i))
                        '    Next i
                        'End If

                        'Dim data As Object() = tdmsChannel.GetData '(dataIndex, count)
                        'If DataConverter.CanConvert(Of Double())(data) Then
                        '    Dim doubleData As Double() = DataConverter.Convert(Of Double())(data)
                        'End If

                        channelIndex += 1
                        'Dim tdmsChannelNode As TreeNode = tdmsChannelGroupNode.Nodes.Add(channelName)
                        'tdmsChannelNode.Tag = tdmsChannel

                        channelNames.Add(channelName) 'добавить в List(Of String) для проверки
                    Next
                    'Next 'For Each tdmsChannelGroup
                End If
            End If
            mTdmsFile.Close()
        Catch ex As Exception
            Const caption As String = "Ошибка при чтении файла из LoadTDMSFile"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End Try

        Return dataChannels
    End Function

    ''' <summary>
    ''' вызывается для каждого нового файла снимка
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateTiming() As WaveformTiming
        Dim timing As WaveformTiming = Nothing

        If sampleIntervalMode = WaveformSampleIntervalMode.None Then
            timing = WaveformTiming.CreateWithNoInterval(baseTime)
        ElseIf sampleIntervalMode = WaveformSampleIntervalMode.Regular Then
            'timing = WaveformTiming.CreateWithRegularInterval(New TimeSpan(0, 0, 1), baseTime)
            baseTime = DateTime.Now ' мне добавлять не надо
            timing = WaveformTiming.CreateWithRegularInterval(New PrecisionTimeSpan(1 / frequency).ToTimeSpan, baseTime)
            'timing = WaveformTiming.CreateWithRegularInterval(New TimeSpan(0, 0, 0, 0, (1 / Frequency) * 1000), baseTime)


            'baseTime += New TimeSpan(0, 0, dataGenerator.NumberOfSamples) 'было 
            'Else 'times(j) должен был бы содержать парные значения времени к значению измерения
            '    Dim times As DateTime() = New DateTime(dataGenerator.NumberOfSamples - 1) {}
            '    times(0) = baseTime
            '    Dim r As New Random()
            '    For j As Integer = 1 To dataGenerator.NumberOfSamples - 1
            '        times(j) = times(j - 1) + New TimeSpan(0, 0, r.[Next](10))
            '    Next
            '    baseTime = times(dataGenerator.NumberOfSamples - 1) + New TimeSpan(0, 0, r.[Next](10)) ' пример для случайного задания времени
            '    timing = WaveformTiming.CreateWithIrregularInterval(times)
        End If

        Return timing
    End Function

    ''' <summary>
    ''' асинхронная работа при добавлении порции данных соответсвующей порции данных одного кадра
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataAppendBackgroundWorker_StartAppendData(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles DataAppendBackgroundWorker.DoWork
        Dim arrAcquiredacquiredData As Double(,) = CType(e.Argument, Double(,))
        Dim numberOfChannels As Integer = UBound(arrAcquiredacquiredData, 1) + 1

        If numberOfChannelsConfigure <> numberOfChannels Then
            If DataAppendBackgroundWorker.IsBusy Then Exit Sub
            Throw New Exception("Нет соответствия числа каналов при конфигурации с размером входного массива данных.")
        End If

        'Dim AnalogWaveData As AnalogWaveform(Of Double)() = New AnalogWaveform(Of Double)(numberOfChannels - 1) {}

        'For I As Integer = 0 To numberOfChannels - 1
        '    AnalogWaveData(I) = AnalogWaveform(Of Double).FromArray1D(ArrayOperation.CopyRow(_arrСреднее, I))
        '    AnalogWaveData(I).Timing = timing
        'Next

        ' так наверно эстетичнее
        Dim AnalogWaveData As AnalogWaveform(Of Double)() = AnalogWaveform(Of Double).FromArray2D(arrAcquiredacquiredData)
        For I As Integer = 0 To numberOfChannels - 1
            AnalogWaveData(I).Timing = timing
        Next

        'e.Result = AnalogWaveData

        'Try
        ' добавить waveforms в channels в TDMS file
        channelGroup.AppendAnalogWaveforms(Of Double)(waveformChannels, AnalogWaveData)
        'Catch ex As Exception
        '    Const caption As String = "Ошибка завершения работы DataAppendBackgroundWorker"
        '    Dim text As String = ex.ToString
        '    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")

        '    CloseTDMSFile()
        '    SetUpTDMSFile()
        'End Try
    End Sub

    ''' <summary>
    ''' Метод вызываемый после BackgroundWorker completed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataAppendBackgroundWorker_CompletedAppendData(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles DataAppendBackgroundWorker.RunWorkerCompleted
        Dim ex As Exception = e.[Error]

        'If ex Is Nothing Then
        '    ' получить переданный массив типа AnalogWaveform из e.Result
        '    Dim acquiredData As AnalogWaveform(Of Double)() = TryCast(e.Result, AnalogWaveform(Of Double)())

        '    Try
        '        ' добавить waveforms в channels в TDMS file
        '        channelGroup.AppendAnalogWaveforms(Of Double)(waveformChannels, acquiredData)
        '    Catch tdmsException As Exception
        '        ex = tdmsException
        '    End Try
        'End If

        ' обновить статус 
        If ex IsNot Nothing Then
            'MessageBox.Show(exception.Message, "Ошибка завершения работы DataAppendBackgroundWorker", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Else
            '    statusTextBox.Text = generateAndAppendMessage
            Const caption As String = "Ошибка завершения работы DataAppendBackgroundWorker"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_FILE_IO_FAILED($"<{caption}> {text}")
        End If

        If isRiseEventsSaveRecord Then
            countAddedBlocksToFile += 1
            countRowsAdded += countRowsMeasuredValues

            If countAddedBlocksToFile >= limitAddedBlocks Then
                isSnapshotFull = True
                CloseTDMSFile()
                If Not IsCloseTDMSFile Then SetUpTDMSFile() ' следующий файл
            Else
                ' просто уведомить вызывающий модуль о продолжениии накопления
                RaiseEvent ContinueAccumulation(Me, New EventArgs())
            End If
        End If
    End Sub

End Class
