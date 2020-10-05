Imports System.Collections.Generic
Imports System.IO
Imports System.Xml.Linq
Imports BaseFormKT
Imports MathematicalLibrary

''' <summary>
''' Менеджер управления модулями подсчёта Контрольных Точек - скомпилированными как DLL программами, соответствующими интерфейсу BaseForm.IClassCalculation
''' </summary>
''' <remarks></remarks>
Friend Class KTCalculationModuleManager
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
    ''' Внутренняя коллекция для управления формами
    ''' </summary>
    Private mCalculationModuleDictionary As Dictionary(Of String, frmBaseKT)
    ''' <summary>
    ''' список Имен Модулей В Каталоге
    ''' </summary>
    Private NameModulesInCatalog As New List(Of String)
    ''' <summary>
    ''' список Описаний Модулей В Каталоге
    ''' </summary>
    Private ReadOnly DescriptionModulesInCatalog As New List(Of String)
    ''' <summary>
    ''' имена Параметров Для Грида
    ''' </summary>
    Private nameParametersForGrid As String()
    ''' <summary>
    ''' внутренний счетчик для подсчета созданных форм можно использовать в заголовке
    ''' </summary>
    Private countFormsCreated As Integer = 0
    Private ReadOnly pathConnectionModulsXml As String
    ''' <summary>
    ''' путь Каталога С Модулями
    ''' </summary>
    Private ReadOnly pathCatalogModules As String

    Public Sub New(ByVal inPathCatalogModules As String)
        pathCatalogModules = inPathCatalogModules
        pathConnectionModulsXml = Path.Combine(pathCatalogModules, "ConnectionModuls.xml")

        If Not File.Exists(pathConnectionModulsXml) Then
            Dim DocumentSettings As XDocument = New XDocument(New XElement("CalculationModuls"))
            DocumentSettings.Save(pathConnectionModulsXml)
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
        Dim fiFiles As FileInfo()
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

                'Dim Types() As Type = inheritsBaseFormAssembly.GetTypes
                'For Each T In Types
                '    Console.WriteLine(T.FullName)
                'Next
                '' пример создания обьекта по имени типа
                'Dim obj As Object = Activator.CreateInstance(varName, ClassName)
                'Dim model As ICarModel = CType(Activator.CreateInstance(Type), ICarModel) ' создать экемпляр указанного типа

                ' получить из имени файла DLL строку имени класса и имени визуально наследуемой формы
                Dim className As String = itemName & ".frmMain"

                ' создание экземпляра класса
                Dim tempBaseFormKT_frmBaseKT As frmBaseKT = CType(inheritsBaseFormAssembly.CreateInstance(className), frmBaseKT)
                If tempBaseFormKT_frmBaseKT IsNot Nothing Then
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
            mCalculationModuleDictionary = New Dictionary(Of String, frmBaseKT)

            For Each itemPassport As PassportModule In PassportModuleDictionary.Values
                If itemPassport.Enable Then
                    Dim assemblyName As String = Path.Combine(pathCatalogModules, itemPassport.NameModule & ".dll")
                    Dim tempBaseFormKT_frmBaseKT As frmBaseKT
                    Dim inheritsBaseFormAssembly As Reflection.Assembly = System.Reflection.Assembly.LoadFrom(assemblyName)

                    ' получить из имени файла DLL строку имени класса и имени визуально наследуемой формы
                    Dim className As String = itemPassport.NameModule & ".frmMain"
                    ' frmMain - эту входную форму должна содержать любая DLL расчета
                    ' создание экземпляра класса
                    tempBaseFormKT_frmBaseKT = CType(inheritsBaseFormAssembly.CreateInstance(className), frmBaseKT)

                    If tempBaseFormKT_frmBaseKT IsNot Nothing Then
                        ' все в порядке, добавить имя в коллекцию
                        ' при создании автоматом добавляется в коллекцию
                        If NewForm(tempBaseFormKT_frmBaseKT) Then ' там проверка на корректность
                            If tempBaseFormKT_frmBaseKT.IsDLLVisible Then
                                CType(MainMdiForm.MenuWindowModuleCalculationKT.DropDownItems(tempBaseFormKT_frmBaseKT.Tag.ToString), ToolStripMenuItem).CheckState = CheckState.Checked
                                ' selectedItem.CheckState = CheckState.Unchecked
                            End If
                        End If
                    End If
                End If
            Next
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
            Dim CalculationModules = New XElement("CalculationModuls")

            documentConnectionModuls.Add(CalculationModules)

            For Each itemPasspor As PassportModule In PassportModuleDictionary.Values
                If itemPasspor.Enable Then
                    NameModulesInConfiguration.Add(itemPasspor.NameModule)
                    CalculationModules.Add(New XElement("Module", itemPasspor.NameModule))
                End If
            Next

            documentConnectionModuls.Save(pathConnectionModulsXml)
        Catch ex As Exception
            Dim caption As String = $"Процедура <{NameOf(SaveNameDllsToConfigurationXML)}>"
            Dim text As String = $"Невозможно сохранить настройки в конфигурационном файле.{vbCrLf}Error: {ex}"
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

    ' в отличии от расчетных модулей настройку соответствия можно выполнить и без остановки сбора
    'fMainForm.varМодульРасчетаManager.ДоступМеню(False)
    'Public Sub EnableMenu(ByVal Доступ As Boolean)
    'End Sub

    ''' <summary>
    ''' Заполнить Списки Параметров От Сервера
    ''' вызывается из CharacteristicForRegime
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PopulateListParametersFromServer()
        If IndexParametersForControl Is Nothing Then Exit Sub

        'ReDim_nameParametersForGrid(UBound(IndexParametersForControl))
        Re.Dim(nameParametersForGrid, UBound(IndexParametersForControl))
        nameParametersForGrid(0) = MissingParameter

        For I As Integer = 1 To UBound(IndexParametersForControl)
            nameParametersForGrid(I) = ParametersType(IndexParametersForControl(I)).NameParameter
        Next

        'ReDim_ParameterAccumulate(UBound(ParametersType)) ' обнулить массив
        Re.Dim(ParameterAccumulate, UBound(ParametersType)) ' обнулить массив

        ' запустить проверку соответствия параметров каналам сбора
        For Each itemBaseForm As frmBaseKT In mCalculationModuleDictionary.Values
            itemBaseForm.Manager.NameRegistrationParameters = nameParametersForGrid
            itemBaseForm.Manager.SetIndexRegistrationParameters(IndexParametersForControl)
            itemBaseForm.Manager.FillCombo()
            itemBaseForm.Manager.FrequencyBackground = FrequencyBackground
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

    Public ReadOnly Property CalculationModuleDictionary() As Dictionary(Of String, frmBaseKT)
        Get
            Return mCalculationModuleDictionary
        End Get
    End Property

    Public ReadOnly Property Item(ByVal vntIndexKey As String) As frmBaseKT
        Get
            Return mCalculationModuleDictionary.Item(vntIndexKey)
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mCalculationModuleDictionary.GetEnumerator
    End Function

    Public Sub Remove(ByRef sKey As String) 'Shared убрал так как надо удалять закрывая саму форму, а она уже удаляет из коллекции
        ' удаление по строковому ключу
        mCalculationModuleDictionary.Remove(sKey)
        countFormsCreated -= 1
    End Sub

    Public Sub Clear()
        mCalculationModuleDictionary.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        mCalculationModuleDictionary = Nothing
        MyBase.Finalize()
    End Sub

    Public Function NewForm(ByVal tempBaseFormKT_frmBaseKT As frmBaseKT) As Boolean
        If mCalculationModuleDictionary.ContainsKey(tempBaseFormKT_frmBaseKT.Tag.ToString) Then
            MessageBox.Show($"Модуль с именем {tempBaseFormKT_frmBaseKT.Tag} уже загружен!",
                            "Загрузка нового модуля", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Try
            tempBaseFormKT_frmBaseKT.Manager.PathSettingMdb = Path.Combine(pathCatalogModules, tempBaseFormKT_frmBaseKT.Tag.ToString & ".mdb")
            ' для отключения или включения меню и окон
            tempBaseFormKT_frmBaseKT.Manager.IsSwohSnapshot = Not IsClient
            tempBaseFormKT_frmBaseKT.Show() ' здесь происходит загрузка и заполнение таблиц

            If tempBaseFormKT_frmBaseKT.IsDLLVisible Then
                'Dim КаталогРасчетногоМодуля As String = temp_BaseFormKT_frmBaseKT.OwnCatalogue
            Else
                tempBaseFormKT_frmBaseKT.Hide()
            End If

            ' можно добавить в коллекцию
            tempBaseFormKT_frmBaseKT.Manager.FrequencyBackground = FrequencyBackground
            mCalculationModuleDictionary.Add(tempBaseFormKT_frmBaseKT.Tag.ToString, tempBaseFormKT_frmBaseKT)
            countFormsCreated += 1

            Return mCalculationModuleDictionary.ContainsKey(tempBaseFormKT_frmBaseKT.Tag.ToString)
        Catch exp As Exception
            Dim caption As String = exp.Source
            Dim text As String = exp.ToString
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            Dim keyColl As Dictionary(Of String, frmBaseKT).KeyCollection = mCalculationModuleDictionary.Keys
            'ReDim_nameKeys(keyColl.Count - 1)
            Re.Dim(nameKeys, keyColl.Count - 1)
            keyColl.CopyTo(nameKeys, 0)

            For Each itemKey As String In nameKeys
                mCalculationModuleDictionary.Item(itemKey).IsWindowClosed = True
                ' надо послать сообщении о закрытии
                mCalculationModuleDictionary.Item(itemKey).Close()
                mCalculationModuleDictionary.Remove(itemKey)
                countFormsCreated -= 1
            Next

            mCalculationModuleDictionary.Clear() ' по идее она уже чистая
            countFormsCreated = 0
        End If
    End Sub
#End Region

#Region "подписаться на событие"
    Private WithEvents WithEventsParentFormMain As FormRegistrationClient

    Public Property ParentFormMain() As FormRegistrationClient
        Get
            Return WithEventsParentFormMain
        End Get
        Set(ByVal Value As FormRegistrationClient)
            WithEventsParentFormMain = Value
        End Set
    End Property

    Private Sub WithEventsParentFormMain_AcquiredDataEvent(ByVal sender As Object, ByVal e As FormRegistrationBase.AcquiredDataEventArgs) Handles WithEventsParentFormMain.AcquiredDataEvent
        'arrAcquiredData=arrПарамНакопленные=arrСреднее=arrПарамНакопленные(N) = dblСреднее
        Dim arrAcquiredData As Double() = e.ArrПарамНакопленные ' можно вызвать и другие свойства

        For Each itemBaseForm As frmBaseKT In mCalculationModuleDictionary.Values
            If itemBaseForm.Manager.IsEnabledTuningForms = False Then ' значить закладки редактирования настроек не активны
                For Each itemMeasureRow As BaseFormDataSet.ИзмеренныеПараметрыRow In itemBaseForm.Manager.MeasurementDataTable.Rows
                    If itemMeasureRow.ИмяКаналаИзмерения <> MissingParameter Then
                        itemMeasureRow.ИзмеренноеЗначение = arrAcquiredData(itemMeasureRow.ИндексКаналаИзмерения)
                    Else
                        If itemMeasureRow.ИспользоватьКонстанту Then
                            itemMeasureRow.ИзмеренноеЗначение = itemMeasureRow.ЗначениеКонстанты
                            ' тест
                            ' itemRow.ИзмеренноеЗначение = TestCalculate(ЦиклИзмеренныйПараметр)
                        Else
                            itemMeasureRow.ИзмеренноеЗначение = con9999999
                        End If
                    End If
                Next

                ' далее вызвать расчет
                itemBaseForm.ClassCalculation.Calculate()
                ' далее выдать подсчитанные значения в массив средних значений массив 
                ' в переменной If itemBaseForm.Manager.ЗапросЗначенийВсехКаналов
                ' передать в свойство itemBaseForm.Manager.ЗначенияВсехКаналов массив Dim arrAcquiredData As Double()
                If itemBaseForm.Manager.IsQueryChannelsValue Then
                    Dim channelsValue(WithEventsParentFormMain.CountMeasurand + 1) As Double
                    ' нулевой индекс не заполняется
                    For J = 0 To WithEventsParentFormMain.CountMeasurand
                        'UBound(strЗначения, 2) - 1 - 3 'ЧислоВсехИзмеряемыхПараметров'3 параметра дополнены и не мерятся

                        'vKey = J + 1 'ищем этот номер
                        'dblСреднее = arrДанныеСервераЗначение(J + 1) '(vKey) ' N))
                        'N = arrIndexParameters(vKey)
                        'в ДанныеОтСервера arrПарамНакопленные(N) = dblСреднее 
                        channelsValue(J + 1) = arrAcquiredData(WithEventsParentFormMain.IndexParameters(J + 1))
                    Next

                    itemBaseForm.Manager.ChannelsValue = channelsValue
                End If
            End If
        Next
    End Sub
#End Region

End Class