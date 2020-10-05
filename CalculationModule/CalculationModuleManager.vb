Imports System.Collections.Generic
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Xml.Linq
Imports System.Data.OleDb
Imports BaseForm
Imports MathematicalLibrary

''' <summary>
''' Менеджер управления расчётными модулями - скомпилированными как DLL программами, соответствующими интерфейсу BaseForm.IClassCalculation
''' </summary>
''' <remarks></remarks>
Friend Class CalculationModuleManager
    Implements IEnumerable

    ''' <summary>
    ''' Список имен модулей в конфигурации
    ''' </summary>
    Public NameModulesInConfiguration As New List(Of String)
    ''' <summary>
    ''' выражает сущность модуля
    ''' </summary>
    Public PassportModuleDictionary As Dictionary(Of String, PassportModule)
    ''' <summary>
    ''' Список Расчетных Параметров
    ''' </summary>
    Public CalculationParameters As New List(Of String)
    ''' <summary>
    ''' Значения Параметров Расчета
    ''' </summary>
    Public ValueCalculationParameters() As Double
    ''' <summary>
    ''' Внутренняя коллекция для управления формами
    ''' </summary>
    Private mCalculationModuleDictionary As Dictionary(Of String, FrmBase)
    ''' <summary>
    ''' список Имен Модулей В Каталоге
    ''' </summary>
    Private ReadOnly NameModulesInCatalog As New List(Of String)
    ''' <summary>
    ''' список Описаний Модулей В Каталоге
    ''' </summary>
    Private ReadOnly DescriptionModulesInCatalog As New List(Of String)
    ''' <summary>
    ''' имена Параметров Для Грида
    ''' </summary>
    Private nameParametersForGrid() As String
    ''' <summary>
    ''' внутренний счетчик для подсчета созданных форм можно использовать в заголовке
    ''' </summary>
    Private mCountFormsCreated As Integer = 0
    Private ReadOnly pathConnectionModulsXml As String
    ''' <summary>
    ''' путь Каталога С Модулями
    ''' </summary>
    Private ReadOnly pathCatalogModules As String

    Public Sub New(ByVal inPathCatalogModules As String)
        pathCatalogModules = inPathCatalogModules
        pathConnectionModulsXml = Path.Combine(pathCatalogModules, "ConnectionModuls.xml")

        If Not File.Exists(pathConnectionModulsXml) Then
            Dim documentSettings As XDocument = New XDocument(New XElement("CalculationModuls"))
            documentSettings.Save(pathConnectionModulsXml)
        End If
    End Sub

    ''' <summary>
    ''' Составить Список Модулей В Каталоге
    ''' Проверка всех DLL на предмет соответствия интерфейсу и в случает успеха добавить имя в список
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MakeListModulesInCatalogue() As Boolean
        Dim namesDLL As New List(Of String)
        Dim diDirectories As New DirectoryInfo(pathCatalogModules)
        Dim fiFiles() As FileInfo
        Dim success As Boolean = False

        Try
            ' вызвать метод GetFiles чтобы получить массив файлов в директории
            fiFiles = diDirectories.GetFiles()

            For Each fi As FileInfo In fiFiles
                Select Case Path.GetExtension(fi.Name).ToUpper()
                    Case ".DLL"
                        namesDLL.Add(fi.Name.Replace(".dll", ""))
                End Select
            Next

            ' проверить что DLL реализует наследование
            For Each itemName As String In namesDLL
                Dim assemblyName As String = Path.Combine(pathCatalogModules, itemName & ".dll")
                Dim inheritsBaseFormAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(assemblyName)
                ' получить из имени файла DLL строку имени класса и имени визуально наследуемой формы
                Dim className As String = itemName & ".FrmMain"

                ' создание экземпляра класса
                Dim tempBaseForm_frmBase As FrmBase = CType(inheritsBaseFormAssembly.CreateInstance(className), FrmBase)
                If tempBaseForm_frmBase IsNot Nothing Then
                    ' все в порядке, добавить имя в коллекцию
                    NameModulesInCatalog.Add(itemName)
                End If
            Next

            For Each fi As FileInfo In fiFiles
                Select Case Path.GetExtension(fi.Name).ToUpper()
                    Case ".XML"
                        If NameModulesInCatalog.Contains(fi.Name.Replace(".xml", "")) Then
                            ' открыть XML файл и прочитать описание
                            Dim documentSettings = New XDocument
                            documentSettings = XDocument.Load(fi.FullName)
                            DescriptionModulesInCatalog.Add(documentSettings...<Description>.Value)
                        End If
                    Case Else
                        Exit Select
                End Select
            Next

            ' Заполнить Коллекцию
            PassportModuleDictionary = PassportModule.LoadPassportModule(NameModulesInCatalog, DescriptionModulesInCatalog)
            LoadNameDllsFromConfigurationXML()

            success = NameModulesInCatalog.Count > 0
        Catch ex As Exception
            Dim caption As String = $"Ошибка при запуске процедуры {NameOf(MakeListModulesInCatalogue)} " & pathCatalogModules
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try

        Return success
    End Function

    ''' <summary>
    ''' Загрузить формы расчётных модулей, включённых в список для загрузки
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadInheritanceForms()
        Try
            mCalculationModuleDictionary = New Dictionary(Of String, FrmBase)

            For Each itemPassport As PassportModule In PassportModuleDictionary.Values
                If itemPassport.Enable Then
                    Dim assemblyName As String = Path.Combine(pathCatalogModules, itemPassport.NameModule & ".dll")
                    Dim tempBaseForm_frmBase As FrmBase
                    Dim inheritsBaseFormAssembly As Reflection.Assembly = System.Reflection.Assembly.LoadFrom(assemblyName)

                    ' получить из имени файла DLL строку имени класса и имени визуально наследуемой формы
                    Dim className As String = itemPassport.NameModule & ".FrmMain"
                    ' frmMain - эту входную форму должна содержать любая DLL расчета
                    ' создание экземпляра класса
                    tempBaseForm_frmBase = CType(inheritsBaseFormAssembly.CreateInstance(className), FrmBase)

                    If tempBaseForm_frmBase IsNot Nothing Then
                        ' все в порядке, добавить имя в коллекцию
                        ' при создании автоматом добавляется в коллекцию
                        If NewForm(tempBaseForm_frmBase) Then ' там проверка на корректность
                            If tempBaseForm_frmBase.IsDllVisible Then
                                CType(MainMdiForm.MenuWindowModuleCalculation.DropDownItems(tempBaseForm_frmBase.Tag.ToString), ToolStripMenuItem).CheckState = CheckState.Checked
                            End If
                        End If
                    End If
                End If
            Next

            ' составить результирующий список параметров расчета, которые будут добавлены как каналы в базу
            CalculationParameters.Clear()

            Dim I As Integer
            For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
                For Each itemRow As BaseFormDataSet.РасчетныеПараметрыRow In itemBaseForm.Manager.CalculatedDataTable.Rows
                    itemRow.ИндексКаналаИзмерения = CShort(I) ' на самом деле индекс с 0 элемента в массива arrЗначенияПараметровРасчета
                    CalculationParameters.Add(itemRow.ИмяПараметра)
                    I += 1
                Next
            Next
            ' там уже могут быть добавлены каналы цифровых входов и АИ222 поэтому добавлять только в конец
            НастройкаКаналов()
            'ReDim_ValueCalculationParameters(CalculationParameters.Count - 1)
            Re.Dim(ValueCalculationParameters, CalculationParameters.Count - 1)
        Catch ex As Exception
            Dim caption As String = $"Ошибка при запуске процедуры {NameOf(LoadInheritanceForms)}"
            Dim text As String = ex.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Считать Имена Модулей Из Конфигурационного Файла
    ''' Вызывается один раз при запуске для заполнения СписокИменМодулейВКонф
    ''' который служит для хранения списка при работе
    ''' </summary>
    Private Sub LoadNameDllsFromConfigurationXML()
        NameModulesInConfiguration.Clear()
        ' открыть XML файл и прочитать описание
        Dim documentConnectionModuls = XDocument.Load(pathConnectionModulsXml)

        Dim elemets As IEnumerable(Of XElement) = documentConnectionModuls.Element("CalculationModuls").Elements("Module")
        For Each element As XElement In elemets
            NameModulesInConfiguration.Add(element.Value)
        Next

        For Each nameModule As String In NameModulesInConfiguration
            PassportModuleDictionary(nameModule).Enable = NameModulesInCatalog.Contains(nameModule)
        Next
    End Sub

    ''' <summary>
    ''' Записать Имена Модулей В Конфигурационный Файл
    ''' Сохранить конфигурацию включённых в данном запуске расчётных модулей
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveNameDllsToConfigurationXML()
        NameModulesInConfiguration.Clear()

        Try
            Dim documentConnectionModuls = New XDocument() ' создать новый пустой документ
            Dim calculationModules = New XElement("CalculationModuls")

            documentConnectionModuls.Add(calculationModules)

            For Each itemPasspor As PassportModule In PassportModuleDictionary.Values
                If itemPasspor.Enable Then
                    NameModulesInConfiguration.Add(itemPasspor.NameModule)
                    calculationModules.Add(New XElement("Module", itemPasspor.NameModule))
                End If
            Next

            documentConnectionModuls.Save(pathConnectionModulsXml)
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(SaveNameDllsToConfigurationXML)}>"
            Dim text As String = $"Невозможно сохранить настройки в конфигурационном файле.{vbCr}{vbLf}Error: {ex}"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Сбросить Подключение
    ''' </summary>
    Public Sub DisenableModules()
        For Each itemPasspor As PassportModule In PassportModuleDictionary.Values
            itemPasspor.Enable = False
        Next
    End Sub

    ''' <summary>
    ''' Есть Подключенные Модули
    ''' </summary>
    ''' <returns></returns>
    Public Function IsEnableModules() As Boolean
        For Each itemPasspor As PassportModule In PassportModuleDictionary.Values
            If itemPasspor.Enable Then Return True
        Next

        Return False
    End Function

    ''' <summary>
    ''' Доступность меню и доступность таблиц в базовой форме модуля.
    ''' </summary>
    ''' <param name="enable"></param>
    Public Sub EnableMenu(ByVal enable As Boolean)
        For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
            itemBaseForm.Manager.IsEnabledTuningForms = enable
            If Not enable Then
                ' скрыть невидимые модули и снять пункты меню для них
                If itemBaseForm.IsDllVisible = False Then
                    itemBaseForm.Hide()
                    CType(MainMdiForm.MenuWindowModuleCalculation.DropDownItems(itemBaseForm.Tag.ToString), ToolStripMenuItem).CheckState = CheckState.Unchecked
                End If
            End If
        Next

        ' сделать соответствующий доступ
        For I As Integer = MainMdiForm.MenuWindowModuleCalculation.DropDownItems.Count - 1 To 2 Step -1
            If MainMdiForm.MenuWindowModuleCalculation.DropDownItems.Count > 2 Then
                MainMdiForm.MenuWindowModuleCalculation.DropDownItems(I).Enabled = enable
            End If
        Next
    End Sub

    ''' <summary>
    ''' Заполнить Списки Параметров От Сервера
    ''' вызывается из CharacteristicForRegime
    ''' </summary>
    Public Sub PopulateListParametersFromServer()
        'ReDim_nameParametersForGrid(UBound(IndexParametersForControl))
        Re.Dim(nameParametersForGrid, UBound(IndexParametersForControl))
        nameParametersForGrid(0) = MissingParameter

        For I As Integer = 1 To UBound(IndexParametersForControl)
            nameParametersForGrid(I) = ParametersType(IndexParametersForControl(I)).NameParameter
        Next

        'ReDim_ParameterAccumulate(UBound(ParametersType)) ' обнулить массив
        Re.Dim(ParameterAccumulate, UBound(ParametersType)) ' обнулить массив

        ' запустить проверку соответствия параметров каналам сбора
        For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
            itemBaseForm.Manager.NameRegistrationParameters(nameParametersForGrid) ' New String() {"Отсутствует", "One", "Two", "N1", "Вк", "Вб"}
            itemBaseForm.Manager.SetIndexRegistrationParameters(IndexParametersForControl) 'New Integer() {1, 2, 3, 4, 5}
            itemBaseForm.Manager.FillCombo()
        Next
    End Sub

#Region "Class МодульРасчетаManager"
    ''' <summary>
    ''' число текущих загруженных форм
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Count() As Integer
        Get
            Return mCalculationModuleDictionary.Count
        End Get
    End Property

    Public ReadOnly Property CalculationModuleDictionary() As Dictionary(Of String, FrmBase)
        Get
            Return mCalculationModuleDictionary
        End Get
    End Property

    Public ReadOnly Property Item(ByVal vntIndexKey As String) As FrmBase
        Get
            Return mCalculationModuleDictionary.Item(vntIndexKey)
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mCalculationModuleDictionary.GetEnumerator
    End Function

    Public Sub Remove(ByRef sIndexKey As String) ' Shared убрал так как надо удалять закрывая саму форму, а она уже удаляет из коллекции
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mCalculationModuleDictionary.Remove(sIndexKey)
        mCountFormsCreated -= 1
    End Sub

    Public Sub Clear()
        mCalculationModuleDictionary.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        mCalculationModuleDictionary = Nothing
        MyBase.Finalize()
    End Sub

    Public Function NewForm(ByVal tempBaseForm_frmBase As FrmBase) As Boolean
        Try
            If mCalculationModuleDictionary.ContainsKey(tempBaseForm_frmBase.Tag.ToString) Then
                MessageBox.Show($"Модуль с именем {tempBaseForm_frmBase.Tag} уже загружен!",
                                "Загрузка нового модуля", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            tempBaseForm_frmBase.Manager.PathSettingMdb = Path.Combine(pathCatalogModules, tempBaseForm_frmBase.Tag.ToString & ".mdb")
            ' далее передать параметры-это пустышка для хоть какого-то заполнения
            tempBaseForm_frmBase.Manager.NameRegistrationParameters(New String() {"Отсутствует", "One", "Two", "N1", "Вк", "Вб"})
            tempBaseForm_frmBase.Manager.SetIndexRegistrationParameters(New Integer() {1, 2, 3, 4, 5})

            tempBaseForm_frmBase.Show() ' здесь происходит загрузка и заполнение таблиц

            If tempBaseForm_frmBase.IsDllVisible Then
                'Dim КаталогРасчетногоМодуля As String = tempBaseForm_frmBase.OwnCatalogue
            Else
                tempBaseForm_frmBase.Hide()
            End If

            ' проверить что имена СписокРасчетныхПараметров не повторяется
            Dim errors As New List(Of String)
            Dim parametersList As New List(Of String) ' временный лист

            For I As Integer = 1 To UBound(ParametersType)
                If ParametersType(I).Mistake <> indexCalculated Then
                    ' не добавить для анализа расчетные параметры, оставшиеся с прежнего запуска
                    ' они будут удалены после этой проверки в НастройкаКаналов
                    parametersList.Add(ParametersType(I).NameParameter)
                End If
            Next

            For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In tempBaseForm_frmBase.Manager.CalculatedDataTable.Rows
                'теперь по всем расчетным параметрам всех модулей включенных в коллекцию
                For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
                    For Each itemРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In itemBaseForm.Manager.CalculatedDataTable.Rows
                        If rowРасчетныйПараметр.ИмяПараметра = itemРасчетныйПараметр.ИмяПараметра Then
                            'совпадение недопустимо
                            errors.Add($"Расчётный параметр модуля {tempBaseForm_frmBase.Tag} с именем {rowРасчетныйПараметр.ИмяПараметра} уже использован в загруженном модуле {itemBaseForm.Name}")
                        End If
                    Next
                Next

                'проверка на конфликтность с именами в arrПараметры - там уже чисто статичные параметры
                '1 АИ222 - базы
                '2 Дискретные входные - из дополнительной таблицы в базе
                If parametersList.Contains(rowРасчетныйПараметр.ИмяПараметра) Then
                    errors.Add($"Расчётный параметр модуля {tempBaseForm_frmBase.Tag} с именем {rowРасчетныйПараметр.ИмяПараметра} уже использован в базе измерительных каналов.")
                End If
            Next

            'есть такая проверка
            'Temp_BaseForm_frmBase.Manager.ПроверкаСоответствияПройдена
            If errors.Count = 0 Then
                ' можно добавить в коллекцию
                mCalculationModuleDictionary.Add(tempBaseForm_frmBase.Tag.ToString, tempBaseForm_frmBase)
                mCountFormsCreated += 1
            Else
                tempBaseForm_frmBase.IsWindowClosed = True
                tempBaseForm_frmBase.Close()
                Dim I As Integer
                Dim result As New StringBuilder("Были обнаружены следующие конфликты имён:" & Environment.NewLine)

                For Each itemError As String In errors
                    result.AppendLine(itemError)
                    I += 1

                    If I > 20 Then
                        result.AppendLine("и далее...")
                        Exit For
                    End If
                Next

                Const caption As String = "Загрузка расчётных модулей"
                Dim text As String = result.ToString
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            End If

            Return mCalculationModuleDictionary.ContainsKey(tempBaseForm_frmBase.Tag.ToString)
        Catch exp As Exception
            Dim caption As String = exp.Source
            Dim text As String = exp.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")

            If Count = 0 Then
                'Throw exp ' передать снова ошибку в Main где возможно выгрузить процесс
                Throw
            End If

            Return False
        End Try
    End Function

    ''' <summary>
    ''' Выгрузить Все Расчетные Модули
    ''' </summary>
    Public Sub RemoveAllCalculationModule()
        Dim nameKeys As String()

        If CalculationModuleDictionary IsNot Nothing Then
            Dim keyColl As Dictionary(Of String, FrmBase).KeyCollection = mCalculationModuleDictionary.Keys
            'ReDim_nameKeys(keyColl.Count - 1)
            Re.Dim(nameKeys, keyColl.Count - 1)
            keyColl.CopyTo(nameKeys, 0)

            For Each itemKey As String In nameKeys
                mCalculationModuleDictionary.Item(itemKey).IsWindowClosed = True
                ' надо послать сообщении о закрытии
                mCalculationModuleDictionary.Item(itemKey).Close()
                mCalculationModuleDictionary.Remove(itemKey)
                mCountFormsCreated -= 1
            Next

            mCalculationModuleDictionary.Clear() ' по идее она уже чистая
            mCountFormsCreated = 0
        End If
    End Sub
#End Region

#Region "Сохранение и очистка параметров в базе"
    Private Structure MyTypeChannel
        Dim strНаименованиеПараметра As String
        Dim ДопускМинимум As Single
        Dim ДопускМаксимум As Single
        Dim РазносУмин As Short
        Dim РазносУмакс As Short
        Dim АварийноеЗначениеМин As Single
        Dim АварийноеЗначениеМакс As Single
        Dim Видимость As Boolean
        Dim ВидимостьРегистратор As Boolean
    End Structure
    Private aParametersChannel() As MyTypeChannel

    Public Property ДобавкаКонфигурацииТсрКлиента As String

    Public Sub НастройкаКаналов()
        Dim strSQL As String
        Dim nameParameter As String
        Dim I, сount, J As Integer
        Dim cn As OleDbConnection
        Dim cmd As OleDbCommand
        Dim rdr As OleDbDataReader
        Dim odaDataAdapter As OleDbDataAdapter
        Dim dtDataTable As New DataTable
        Dim newDataRow As DataRow
        Dim cb As OleDbCommandBuilder

        ДобавкаКонфигурацииТсрКлиента = Nothing ' для добавления к строке конфигурации ТСР клиенту

        Try
            cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            cmd = cn.CreateCommand
            strSQL = $"SELECT COUNT(*) FROM {ChannelLast} WHERE Погрешность={indexCalculated}"
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            сount = CInt(cmd.ExecuteScalar)

            ' считать из базы Channels параметры Расчет и если есть копировать в массив признаки
            If сount > 0 Then
                'ReDim_aParametersChannel(сount - 1)
                Re.Dim(aParametersChannel, сount - 1)
                I = 0
                strSQL = $"SELECT НаименованиеПараметра, ДопускМинимум, ДопускМаксимум, РазносУмин, РазносУмакс, АварийноеЗначениеМин, АварийноеЗначениеМакс, Видимость, ВидимостьРегистратор FROM {ChannelLast} WHERE Погрешность={indexCalculated}"
                ' 10000 признак расчётных каналов
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader

                Do While (rdr.Read)
                    aParametersChannel(I).strНаименованиеПараметра = CStr(rdr("НаименованиеПараметра"))
                    aParametersChannel(I).ДопускМинимум = CSng(rdr("ДопускМинимум"))
                    aParametersChannel(I).ДопускМаксимум = CSng(rdr("ДопускМаксимум"))
                    aParametersChannel(I).РазносУмин = CShort(rdr("РазносУмин"))
                    aParametersChannel(I).РазносУмакс = CShort(rdr("РазносУмакс"))
                    aParametersChannel(I).АварийноеЗначениеМин = CSng(rdr("АварийноеЗначениеМин"))
                    aParametersChannel(I).АварийноеЗначениеМакс = CSng(rdr("АварийноеЗначениеМакс"))
                    aParametersChannel(I).Видимость = CBool(rdr("Видимость"))
                    aParametersChannel(I).ВидимостьРегистратор = CBool(rdr("ВидимостьРегистратор"))
                    I += 1
                Loop
                rdr.Close()

                ' 1 Выполнить команду на удаление каналов 
                strSQL = $"DELETE * FROM {ChannelLast} WHERE Погрешность={indexCalculated}"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

                Thread.Sleep(500)
                Application.DoEvents()

                ' обновить базу
                For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
                    For Each itemRow As BaseFormDataSet.РасчетныеПараметрыRow In itemBaseForm.Manager.CalculatedDataTable.Rows
                        ' должны быть только по 1 на данном запуске
                        nameParameter = itemRow.ИмяПараметра
                        For I = 0 To сount - 1
                            If aParametersChannel(I).strНаименованиеПараметра = nameParameter Then
                                itemRow.ДопускМинимум = aParametersChannel(I).ДопускМинимум
                                itemRow.ДопускМаксимум = aParametersChannel(I).ДопускМаксимум
                                itemRow.РазносУмин = aParametersChannel(I).РазносУмин
                                itemRow.РазносУмакс = aParametersChannel(I).РазносУмакс
                                itemRow.АварийноеЗначениеМин = aParametersChannel(I).АварийноеЗначениеМин
                                itemRow.АварийноеЗначениеМакс = aParametersChannel(I).АварийноеЗначениеМакс
                                itemRow.Видимость = aParametersChannel(I).Видимость
                                itemRow.ВидимостьРегистратор = aParametersChannel(I).ВидимостьРегистратор
                                Exit For
                            End If
                        Next I
                    Next
                    'записать изменения
                    itemBaseForm.Manager.SaveTable()
                Next
            End If

            ' 5 делаем запрос на выборку(вставку) и добавляем параметры в базу ChannelNNN
            '10000 признак цифровых входных каналов
            strSQL = $"SELECT * FROM {ChannelLast} ORDER BY НомерПараметра" ' & " WHERE Погрешность=" & indexРасчетный.tostring
            odaDataAdapter = New OleDbDataAdapter(strSQL, cn)
            dtDataTable = New DataTable
            odaDataAdapter.Fill(dtDataTable)
            J = CInt(dtDataTable.Rows(dtDataTable.Rows.Count - 1).Item("НомерПараметра")) + 1

            For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
                For Each rowРасчетныйПараметр As BaseFormDataSet.РасчетныеПараметрыRow In itemBaseForm.Manager.CalculatedDataTable.Rows

                    newDataRow = dtDataTable.NewRow
                    newDataRow.BeginEdit()
                    newDataRow("НомерПараметра") = J
                    newDataRow("НаименованиеПараметра") = rowРасчетныйПараметр.ИмяПараметра
                    newDataRow("НомерКанала") = rowРасчетныйПараметр.ИндексКаналаИзмерения ' I для того чтобы знать индекс для этого канала в массиве равен ЦиклРасчетныйПараметр.ИндексКаналаИзмерения
                    newDataRow("НомерУстройства") = 0 ' rowРасчетныйПараметр.intНомерУстройства
                    newDataRow("НомерМодуляКорзины") = 0
                    newDataRow("НомерКаналаМодуля") = rowРасчетныйПараметр.ИндексКаналаИзмерения
                    newDataRow("ТипПодключения") = "DIFF"
                    newDataRow("НижнийПредел") = 0
                    newDataRow("ВерхнийПредел") = 10
                    newDataRow("ТипСигнала") = "DC"
                    newDataRow("НомерФормулы") = 1
                    newDataRow("СтепеньАппроксимации") = 1
                    newDataRow("A0") = 0
                    newDataRow("A1") = 0
                    newDataRow("A2") = 0
                    newDataRow("A3") = 0
                    newDataRow("A4") = 0
                    newDataRow("A5") = 0
                    newDataRow("Смещение") = 0

                    newDataRow("КомпенсацияХС") = False
                    newDataRow("ЕдиницаИзмерения") = ConvertUnitOfMeasureToBase(rowРасчетныйПараметр.РазмерностьВыходная)
                    newDataRow("ДопускМинимум") = rowРасчетныйПараметр.ДопускМинимум
                    newDataRow("ДопускМаксимум") = rowРасчетныйПараметр.ДопускМаксимум
                    newDataRow("РазносУмин") = rowРасчетныйПараметр.РазносУмин
                    newDataRow("РазносУмакс") = rowРасчетныйПараметр.РазносУмакс
                    newDataRow("АварийноеЗначениеМин") = rowРасчетныйПараметр.АварийноеЗначениеМин
                    newDataRow("АварийноеЗначениеМакс") = rowРасчетныйПараметр.АварийноеЗначениеМакс
                    newDataRow("Дата") = Date.Today.ToShortDateString

                    newDataRow("Видимость") = rowРасчетныйПараметр.Видимость
                    newDataRow("ВидимостьРегистратор") = rowРасчетныйПараметр.ВидимостьРегистратор
                    newDataRow("Погрешность") = indexCalculated
                    newDataRow("Примечания") = Left(rowРасчетныйПараметр.ОписаниеПараметра, 99) '100)
                    newDataRow.EndEdit()
                    dtDataTable.Rows.Add(newDataRow)
                    J += 1

                    If ДобавкаКонфигурацииТсрКлиента = vbNullString Then
                        ДобавкаКонфигурацииТсрКлиента = rowРасчетныйПараметр.ИмяПараметра
                    Else
                        ДобавкаКонфигурацииТсрКлиента &= "\" & rowРасчетныйПараметр.ИмяПараметра
                    End If
                Next
            Next
            ДобавкаКонфигурацииТсрКлиента &= "\"
            cb = New OleDbCommandBuilder(odaDataAdapter)
            odaDataAdapter.Update(dtDataTable)
            cn.Close()

            Thread.Sleep(500)
            Application.DoEvents()
        Catch exp As Exception
            Dim caption As String = $"Процедура <{NameOf(НастройкаКаналов)}> - " & exp.Source
            Dim text As String = exp.Message
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    ''' <summary>
    ''' Перевод Ед Подсчета В Базовые
    ''' Перевод единиц измерения расчёта в используемые для графического контроля
    ''' </summary>
    ''' <param name="inUnitOfMeasure"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertUnitOfMeasureToBase(inUnitOfMeasure As String) As String
        '"%";"K";"атм";"бар";"Вольт";"град (рад)";"град С";"Деления";"дин/см^2";"кг/кгс*час";"кг/с";"кг/час";"кгс";"кгс/м^2";"кгс/см^2";"кПа";"мм";"мм.вод.ст";"мм.рт.ст";"Мпа";"Н/см^2";"нет";"Па";

        Dim outUnitOfMeasure As String = "дел"
        ' не используется "мм/с"
        Select Case inUnitOfMeasure
            Case "град (рад)", "кг/с", "кг/час"
                ' КлассДавление
                outUnitOfMeasure = "Кгсм"
                Exit Select
            Case "%"
                ' КлассОбороты
                outUnitOfMeasure = "%"
                Exit Select
            Case "кгс"
                ' КлассОбороты
                outUnitOfMeasure = "кгс"
                Exit Select
            Case "нет"
                ' КлассВибрация
                outUnitOfMeasure = "кг/ч"
                Exit Select
            Case "Вольт"
                outUnitOfMeasure = "мкА"
                ' КлассТок
                Exit Select
            Case "K", "град С"
                ' КлассТемпература
                outUnitOfMeasure = "градус"
                Exit Select
            Case "мм", "атм", "бар", "кПа", "Мпа", "Па", "мм.вод.ст", "мм.рт.ст"
                ' КлассСтолбы
                outUnitOfMeasure = "мм"
                Exit Select
            Case "Деления", "дин/см^2", "кг/кгс*час", "кгс/м^2", "кгс/см^2", "Н/см^2"
                ' КлассРасход
                outUnitOfMeasure = "дел"
                Exit Select
            Case Else
                outUnitOfMeasure = "дел"
                Exit Select
        End Select

        Return outUnitOfMeasure
    End Function

    ''' <summary>
    ''' Очистить Базу Каналов От Расчетных Параметров
    ''' </summary>
    Public Sub ClearBaseChannelsFromCalculationParameters()
        Dim cn As OleDbConnection = Nothing
        Dim cmd As OleDbCommand
        Dim strSQL As String

        Try
            cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cmd = cn.CreateCommand
            cmd.CommandType = CommandType.Text

            strSQL = $"DELETE * FROM {ChannelLast} WHERE Погрешность={indexCalculated}"
            cmd.CommandText = strSQL
            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            Thread.Sleep(500)
            Application.DoEvents()
        Catch ex As Exception
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(ClearBaseChannelsFromCalculationParameters)}> {ex}")
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub
#End Region

#Region "подписаться на событие"
    Private WithEvents WithEventsParentFormMain As FormRegistrationBase

    Public Property ParentFormMain() As FormRegistrationBase
        Get
            Return WithEventsParentFormMain
        End Get
        Set(ByVal Value As FormRegistrationBase)
            WithEventsParentFormMain = Value
        End Set
    End Property

    Private Sub WithEventsParentFormMain_AcquiredDataEvent(ByVal sender As Object, ByVal e As FormRegistrationBase.AcquiredDataEventArgs) Handles WithEventsParentFormMain.AcquiredDataEvent
        ' arrAcquiredData=arrПарамНакопленные=arrСреднее=arrПарамНакопленные(N) = dblСреднее
        Dim arrAcquiredData As Double() = e.ArrПарамНакопленные ' можно вызвать и другие свойства

        For Each itemBaseForm As FrmBase In mCalculationModuleDictionary.Values
            For Each itemMeasureRow As BaseFormDataSet.ИзмеренныеПараметрыRow In itemBaseForm.Manager.MeasurementDataTable.Rows
                If itemMeasureRow.ИмяКаналаИзмерения <> MissingParameter Then
                    itemMeasureRow.ИзмеренноеЗначение = arrAcquiredData(itemMeasureRow.ИндексКаналаИзмерения)
                Else
                    If itemMeasureRow.ИспользоватьКонстанту Then
                        ' для реальной камеры
                        itemMeasureRow.ИзмеренноеЗначение = itemMeasureRow.ЗначениеКонстанты
                        ' тест
                        'itemRow.ИзмеренноеЗначение = TestCalculate(ЦиклИзмеренныйПараметр)
                    Else
                        itemMeasureRow.ИзмеренноеЗначение = con9999999
                    End If
                End If
            Next

            ' далее вызвать расчет
            itemBaseForm.ClassCalculation.Calculate()
            ' далее выдать подсчитанные значения в массив средних значений массив 
            For Each itemCalculateRow As BaseFormDataSet.РасчетныеПараметрыRow In itemBaseForm.Manager.CalculatedDataTable.Rows
                ValueCalculationParameters(itemCalculateRow.ИндексКаналаИзмерения) = itemCalculateRow.ВычисленноеПереведенноеЗначение
            Next
        Next
    End Sub

    'Private random As Random = New Random
    'Private Function TestCalculate(ByVal ЦиклИзмеренныйПараметр As BaseFormDataSet.ИзмеренныеПараметрыRow) As Double
    '    'Return ЦиклИзмеренныйПараметр.ДопускМаксимум * 0.6 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.025 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '    'Return 0
    '    Select Case ЦиклИзмеренныйПараметр.ИмяПараметра
    '        Case "T309_1"
    '            Return 490 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.025 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "T309_12"
    '            Return 490 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.025 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "T340_1"
    '            Return 1200 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.08 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "T340_10"
    '            Return 1200 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.08 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "T3мерн_участка"
    '            Return 300 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.01 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "Tбокса"
    '            Return 20 + random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.025 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '        Case "Барометр"
    '            Return 760 '+ random.NextDouble * (ЦиклИзмеренныйПараметр.ДопускМаксимум - ЦиклИзмеренныйПараметр.ДопускМинимум) * 0.025 'arrAcquiredData(ЦиклИзмеренныйПараметр.ИндексКаналаИзмерения)
    '            Exit Select
    '    End Select
    'End Function

#End Region

    ''' <summary>
    ''' Представление в виде строки
    ''' </summary>
    Public Overloads Overrides Function ToString() As String
        ' пара первых букв каждой работы через точку с запятой
        Dim descriptionModules As String = ""
        For Each tempПаспортМодуля As PassportModule In PassportModuleDictionary.Values
            If tempПаспортМодуля.Enable Then
                descriptionModules += tempПаспортМодуля.DescriptionModule + ";" 'tempПаспортМодуля.ОписаниеМодуля.Substring(0, 6) + "...;"
            End If
        Next
        Return descriptionModules
    End Function
End Class
