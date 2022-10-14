Imports System.IO
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Soap
Imports System.Xml.Linq
Imports Registration.My

''' <summary>
''' Считывание и сохранение настроек программы
''' </summary>
''' <remarks></remarks>
Public Class FormCompactRioSetting
    Private ReadOnly mOptionData As OptionDataCRIO

    ''' <summary>
    ''' Главное окно приложения
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        mOptionData = New OptionDataCRIO()
    End Sub

    Friend Function GetOptionData() As OptionDataCRIO
        Return mOptionData
    End Function

#Region "Load/Close"
    Private Sub FormCompactRioSetting_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        LoadForm()
    End Sub

    Public Sub LoadForm()
        LoadSettings()
        ' PropertyGrid – удобный компонент для визуального редактирования свойств объектов.
        ' Объект для редактирования задается в дизайнере WinForms, либо непосредственно в коде.

        ' устанавливаем редактируемый объект
        propertyGridChassis.SelectedObject = mOptionData
        ' восстановить положение окна
        RestorePosition()
        ' восстановить положение разделителя колонок в гриде
        RestoreGridSplitterPosition()
    End Sub

    Private Sub FormCompactRioSetting_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True ' нужно не закрыть форму настроек, а свернуть окно

        'Trace.WriteLine("MainForm_FormClosing()")
        ' запоминаем положение окна
        SavePosition()
        ' и разделителя в гриде
        SaveGridSplitterPosition()
        ' сохраним возможно измененные значения параметров
        MySettings.[Default].Save()
        PathServerDataBase = mOptionData.PathDataBaseStendServer

        If Not File.Exists(PathServerDataBase) Then
            Dim caption As String = NameOf(FormCompactRioSetting_FormClosing)
            Dim text As String
            caption = "Проверка наличия файла"
            text = $"По указанному пути нет файла: {PathServerDataBase} !{vbCrLf}Проверьте сетевое окружение."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0)
        End If

        WriteSettings()
        Hide()
        DialogResult = DialogResult.OK
    End Sub

#Region "Запись и считывание настроек"
    ''' <summary>
    ''' Считать Настройки.
    ''' </summary>
    Private Sub LoadSettings()
        PathOptions_xml = Path.Combine(PathResourses, "OptionsCRIO.xml")

        If Not File.Exists(PathOptions_xml) Then
            Dim caption As String = NameOf(LoadSettings)
            Dim text As String
            text = $"В каталоге нет файла <{PathOptions_xml}>!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Environment.Exit(0) ' End
        End If

        ' При смене имени проекта прежние сериализованные файлы открываться не будут 
        ' т.к. при сериализации класса включается имя проекта.
        ' Надо удалить ключи и файлы, чтобы они вновь создались с настройками по умолчанию.
        ' Классы предназначенные для сериализации должны содержать атрибуты.
        Dim partition, section As String
        Dim cReadWriteIni As New ReadWriteIni(PathOptions_xml)
        Dim xmlDoc As XElement = XElement.Load(PathOptions_xml)

        ' 2 способ для многократного вызова ключа
        With cReadWriteIni
            partition = "MobileComplex"
            '--- 1. Общие -------------------------------------------------------------
            section = "Общие"
            ' "Тип работы"
            mOptionData.СКонтроллером = CBool(.GetIni(xmlDoc, partition, section, "Тип_работы", "True"))

            '--- 2. Частота сбора -----------------------------------------------------
            section = "Frequency"
            ' Частота сбора Гц"
            mOptionData.Frequency = Integer.Parse(.GetIni(xmlDoc, partition, section, "Frequency", "10")).ToString

            '--- 3. Стенд сервера -----------------------------------------------------
            '--- 4. Дополнительно -----------------------------------------------------
            section = "Дополнительно"
            ' Стенд сервера" считать простые строки
            mOptionData.StendServer = .GetIni(xmlDoc, partition, section, "Стенд_сервера", "1")
            ' считать простые строки
            ' Путь к базе сервера"в зависимости от данного параметра меняется путь
            mOptionData.ИдетСчитываниеПути = True
            mOptionData.PathDataBaseStendServer = .GetIni(xmlDoc, partition, section, Path_Server_Data_Base & mOptionData.StendServer, "Неправильный путь")
            mOptionData.ИдетСчитываниеПути = False
            PathServerDataBase = mOptionData.PathDataBaseStendServer

            ' --- Файл "СписокШассиCRio" -------------------------------------
            ' в ключе в опциях содержится путь к файлу .XML содержащий сериализованный Hashtable с коллекцией элементов
            ' типа Object содержащих экземпляры классов, являющихся свойствами persondata
            ' --- Файл "Цель испытаний" --------------------------------------
            ' в ключе в опциях содержится путь к файлу .XML содержащий сериализованный Hashtable с коллекцией элементов
            ' типа Object содержащих экземпляры классов, являющихся свойствами persondata
            Dim xmlFile As String = Path.Combine(PathResourses, CRIOChassisList & ".xml")
            If Not File.Exists(xmlFile) Then
                ' раздела нет значит нет и файра сериализации, надо создать
                ' для примера коллекция из одного элемента экземпляра TargetCRIO
                Dim ArrayListObject As New ArrayList From {New TargetCRIO("New Target", New IPAddressTargetCRIO("192.168.1.1"), EnumModeWork.Measurement, "Папка не указана")}
                SerializeHashtableToXML(xmlFile, ArrayListObject)
                ' создать раздел
                .GetIni(xmlDoc, partition, section, CRIOChassisList, xmlFile)
            End If

            ' извлекаем из Hashtable элементы (они экземпляры классов свойств TargetCRIO уже известного типа - привести к типу TargetCRIO)
            For Each de As DictionaryEntry In DeserializeHashtableFromXML(xmlFile)
                'Console.WriteLine("Key={0}  Value={1}", de.Key, de.Value)
                mOptionData.CollectionTargetCRIO.Add(CType(de.Value, TargetCRIO))
            Next

            ' "IP адрес"
            ' считать простые строки и вставить в конструктор IPAddress
            mOptionData.IPaddressProperty = New IPAddressTargetCRIO(.GetIni(xmlDoc, partition, section, "IP_адрес", "192.168.1.1"))
            mOptionData.Порт = .GetIni(xmlDoc, partition, section, "Port", "5555")
        End With
    End Sub

    ''' <summary>
    ''' Записать Настройки.
    ''' </summary>
    Private Sub WriteSettings()
        Dim partition, section As String
        Dim cReadWriteIni As New ReadWriteIni(PathOptions_xml)

        ' 2 способ для многократного вызова ключа
        Dim xmlDoc As XElement = XElement.Load(PathOptions_xml)
        '.writeINI(xmlDoc, partition, section, strКлюч, strValue)
        With cReadWriteIni
            partition = "MobileComplex"
            '--- 1. Общие -------------------------------------------------------------
            section = "Общие"

            '--- 2. Частота сбора -----------------------------------------------------
            section = "Frequency"
            ' "Частота сбора Гц"
            .WriteINI(xmlDoc, partition, section, "Frequency", mOptionData.Frequency)

            '--- 3. Стенд сервера -----------------------------------------------------
            '--- 4. Дополнительно -----------------------------------------------------
            section = "Дополнительно"
            ' "Стенд сервера"
            ' записать простые строки
            .WriteINI(xmlDoc, partition, section, "Стенд_сервера", mOptionData.StendServer)

            ' "Путь к базе сервера"
            ' в зависимости от данного параметра меняется путь
            .WriteINI(xmlDoc, partition, section, Path_Server_Data_Base & mOptionData.StendServer, mOptionData.PathDataBaseStendServer)

            ' --- Файл "СписокШассиCRio" -------------------------------------
            ' в ключе в опциях содержится путь к файлу .XML содержащий сериализованный Hashtable с коллекцией элементов
            ' типа Object содержащих экземпляры классов, являющихся свойствами шасси
            ' в ключе в опциях содержится путь к файлу .XML содержащий сериализованный Hashtable с коллекцией элементов
            ' типа Object содержащих экземпляры классов, являющихся свойствами TestingData
            Dim xmlFile As String = Path.Combine(PathResourses, CRIOChassisList & ".xml")
            Dim ArrayListObject As New ArrayList
            ' подготовить ArrayList
            For Each itemTargetCRIO As TargetCRIO In mOptionData.CollectionTargetCRIO
                ArrayListObject.Add(itemTargetCRIO.DeepCopy)
            Next

            SerializeHashtableToXML(xmlFile, ArrayListObject)
            ' обновить путь файла
            .WriteINI(xmlDoc, partition, section, CRIOChassisList, xmlFile)
            ' "IP адрес"записать простые строки
            .WriteINI(xmlDoc, partition, section, "IP_адрес", mOptionData.IPaddressProperty.IP)
            .WriteINI(xmlDoc, partition, section, "Port", mOptionData.Порт)
        End With
    End Sub
#End Region

#Region "Серелизация и десериализация ArrayList в Hashtable"
    ''' <summary>
    ''' Получить Hashtable из Файла.
    ''' По пути к XML файлу десериализовать его в Hashtable.
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeserializeHashtableFromXML(ByVal strFileXML As String) As Hashtable
        ' Создать soap сериализатор.
        Dim sf As New SoapFormatter()

        ' Загрузить содержимое файла, используя этот же SoapFormatter объект
        Dim ht2 As Hashtable
        Using fs As New FileStream(strFileXML, FileMode.Open)
            ht2 = DirectCast(sf.Deserialize(fs), Hashtable)
        End Using
        ' Проверить, что объект был дессериализован корректно
        'For Each de As DictionaryEntry In ht2
        '    Console.WriteLine("Key={0}  Value={1}", de.Key, de.Value)
        'Next
        Return ht2
    End Function

    ''' <summary>
    ''' Записать Hashtable в файл.
    ''' Серелизация коллекции представленной ArrayList и запись в XML файл.
    ''' </summary>
    ''' <param name="strFileXML"></param>
    ''' <param name="ArrayListObject"></param>
    ''' <remarks></remarks>
    Private Sub SerializeHashtableToXML(ByVal strFileXML As String, ByVal ArrayListObject As ArrayList)
        ' на входе ArrayList переводим его в Hashtable со строковым ключом индекса
        ' Создать Hashtable объект, и заполнить его данными из коллекции.
        Dim I As Integer
        Dim ht As New Hashtable()

        For Each obj As Object In ArrayListObject
            ht.Add(I.ToString, obj)
            I += 1
        Next

        ' Создать a soap сериализатор.
        Dim sf As New SoapFormatter()
        ' Записать Hashtable на диск в SOAP формате.
        Using fs As New FileStream(strFileXML, FileMode.Create)
            sf.Serialize(fs, ht)
        End Using
    End Sub
#End Region

#End Region

#Region "Сохранение/восстановление положения окна"
    ' Сохранение положения окна.
    Private Sub SavePosition()
        MySettings.[Default].CompactRioFormState = WindowState
        If WindowState = FormWindowState.Normal Then
            MySettings.[Default].CompactRioFormSize = Size
            MySettings.[Default].CompactRioFormLocation = Location
        Else
            MySettings.[Default].CompactRioFormSize = RestoreBounds.Size
            MySettings.[Default].CompactRioFormLocation = RestoreBounds.Location
        End If

        'Trace.WriteLine($"{NameOf(FormCompactRioSetting)}::{NameOf(SavePosition)}: Height={MySettings.[Default].CompactRioFormSize.Height}, Width={MySettings.[Default].CompactRioFormSize.Width}, X={MySettings.[Default].CompactRioFormLocation.X}, Y={MySettings.[Default].CompactRioFormLocation.Y}, CompactRioFormState={MySettings.[Default].CompactRioFormState}")
    End Sub

    ' Восстановление положения окна.
    Private Sub RestorePosition()
        ' пока это положение первый раз не сохранено, при 
        ' попытке чтения будет кирдык
        Try
            Size = MySettings.[Default].CompactRioFormSize
            Location = MySettings.[Default].CompactRioFormLocation
            WindowState = MySettings.[Default].CompactRioFormState
        Catch
            Trace.WriteLine($"{NameOf(FormCompactRioSetting)}::{NameOf(RestorePosition)}: exception")
        End Try

        'Trace.WriteLine($"{NameOf(FormCompactRioSetting)}::{NameOf(RestorePosition)}: Height={MySettings.[Default].CompactRioFormSize.Height}, Width={MySettings.[Default].CompactRioFormSize.Width}, X={MySettings.[Default].CompactRioFormLocation.X}, Y={MySettings.[Default].CompactRioFormLocation.Y}, CompactRioFormState={MySettings.[Default].CompactRioFormState}")
    End Sub

    ' Как запомнить и восстановить положение разделителя колонок в PropertyGrid?
    ' Это можно сделать при помощи следующих функций:
    ' Вызываются они соответственно перед закрытием и перед загрузкой окна формы, содержащей PropertyGrid:

    ''' <summary>
    ''' Сохранение положения разделителя в гриде.
    ''' </summary>
    Private Sub SaveGridSplitterPosition()
        Dim type As Type = propertyGridChassis.[GetType]() ' получить тип объекта
        Dim field As FieldInfo = type.GetField("gridView", BindingFlags.NonPublic Or BindingFlags.Instance) ' получить доступ к метаданным сетки

        Dim valGrid As Object = field.GetValue(propertyGridChassis) ' хотя объект сетка недоступна напрямую она извлекается рефлексией 
        Dim gridType As Type = valGrid.[GetType]() ' получить тип объекта
        ' в итоге вызывает член типа для получения ширины метки
        MySettings.[Default].GridSplitterPos = CInt(gridType.InvokeMember("GetLabelWidth", BindingFlags.[Public] Or BindingFlags.InvokeMethod Or BindingFlags.Instance, Nothing, valGrid, {}))

        'Trace.WriteLine($"{NameOf(SaveGridSplitterPosition)}: {Convert.ToString(MySettings.[Default].GridSplitterPos)}")
    End Sub

    ''' <summary>
    ''' Восстановление положения разделителя в гриде.
    ''' </summary>
    Private Sub RestoreGridSplitterPosition()
        Try
            Dim type As Type = propertyGridChassis.[GetType]() ' получить тип объекта
            Dim field As FieldInfo = type.GetField("gridView", BindingFlags.NonPublic Or BindingFlags.Instance) ' получить доступ к метаданным сетки

            Dim valGrid As Object = field.GetValue(propertyGridChassis) ' хотя объект сетка недоступна напрямую она извлекается рефлексией 
            Dim gridType As Type = valGrid.[GetType]() ' получить тип объекта
            ' в итоге вызывает член типа для перемещения разделителя на позицию ширины метки
            'Trace.WriteLine($"{NameOf(RestoreGridSplitterPosition)}: {MySettings.[Default].GridSplitterPos}")
            gridType.InvokeMember("MoveSplitterTo", BindingFlags.NonPublic Or BindingFlags.InvokeMethod Or BindingFlags.Instance, Nothing, valGrid, {MySettings.[Default].GridSplitterPos})
        Catch ex As MissingMethodException
            Trace.WriteLine($"{NameOf(FormCompactRioSetting)}::{NameOf(RestoreGridSplitterPosition)} exception: {ex.ToString}")
        End Try
    End Sub
#End Region

    ' закоментировал для предотвращения повторного вызова
    'Private Sub BtnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
    '    Close()
    'End Sub

    ''' <summary>
    ''' Обработчик события PropertyGrid – PropertyValueChanged для свойства, видимость которого зависит от другого свойства 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PropertyGridChassis_PropertyValueChanged(ByVal s As Object, ByVal e As PropertyValueChangedEventArgs) Handles propertyGridChassis.PropertyValueChanged
        '' По содержимому поля Label определить модифицируемое свойство OptionData
        '' --- 1. "Частота сбора Гц" ------------------------------------------
        'If e.ChangedItem.Label = "Частота сбора Гц" Then
        '    SetRefreshScreen() '(myOptionData.ЧастотаСбора)
        'End If

        ' --- 3. XmlПутьОпции ------------------------------------------------
        mOptionData.ИдетСчитываниеПути = True
        If Not mOptionData.ИдетЗаписьПути Then
            Dim partition, section As String
            Dim cReadWriteIni As New ReadWriteIni(PathOptions_xml)
            Dim xmlDoc As XElement = XElement.Load(PathOptions_xml)
            ' 2 способ для многократного вызова ключа
            With cReadWriteIni
                partition = "MobileComplex"
                section = "Дополнительно"
                ' в зависимости от данного параметра меняется путь
                mOptionData.PathDataBaseStendServer = .GetIni(xmlDoc, partition, section, Path_Server_Data_Base & mOptionData.StendServer, "Неправильный путь")
            End With
        End If
        mOptionData.ИдетСчитываниеПути = False

        propertyGridChassis.Refresh()
    End Sub

    ''' <summary>
    ''' Проверить числовое значение Поля.
    ''' </summary>
    ''' <param name="inTextBox"></param>
    ''' <param name="propertyDescriptor"></param>
    ''' <returns></returns>
    Private Function ValidatingTextBoxEmpty(ByVal inTextBox As String, ByVal propertyDescriptor As String) As Boolean
        'значениеПоля = Trim(значениеПоля)
        Dim success As Boolean = False

        ' Если поле не заполнено, сообщить пользователю
        If inTextBox = vbNullString Then
            MessageBox.Show($"Поле <{propertyDescriptor}> не может быть пустым!", "Ввод шапки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return success
        End If

        Try
            Dim dblTemp As Double = Double.Parse(inTextBox)
            success = True
        Catch ex As Exception
            MessageBox.Show($"Поле <{propertyDescriptor}> должно быть числом!", "Ввод шапки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            success = False
        End Try

        Return success
    End Function
End Class