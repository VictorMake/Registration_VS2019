Imports System.Collections.Generic
Imports System.Xml.Linq
Imports Registration.FormEditiorGraphByParameter

'корневой для коллекции
''' <summary>
''' Коллекция графиков от параметров.
''' Используется для подготовки создания формы на основе шаблона.
''' </summary>
Friend Class GraphsOfParameters
    Implements IEnumerable
    Private GraphsOfParams As Dictionary(Of String, GraphOfParameter)
    Private ReadOnly ParentForm As FormMain

    Public Sub New(inParentForm As FormMain)
        ParentForm = inParentForm
        GraphsOfParams = New Dictionary(Of String, GraphOfParameter)
    End Sub

    Public ReadOnly Property DictionaryGraphOfParam() As Dictionary(Of String, GraphOfParameter)
        Get
            Return GraphsOfParams
        End Get
    End Property

    Public ReadOnly Property Item(ByVal key As String) As GraphOfParameter
        Get
            Return GraphsOfParams.Item(key)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return GraphsOfParams.Count()
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GraphsOfParams.GetEnumerator
    End Function

    Public Sub Remove(ByVal key As String)
        ' если целый тип, то по плавающему индексу, а если строковый то по ключу
        GraphsOfParams.Remove(key)
    End Sub

    Public Sub Clear()
        GraphsOfParams.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        GraphsOfParams = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Add(ByVal nameGraph As String) As Boolean
        If Not CheckForm(nameGraph) Then Return Nothing

        Dim newGrafOfParam = New GraphOfParameter(nameGraph, ParentForm)

        Dim success As Boolean = newGrafOfParam.CheckCorrectnessGrafOfParam
        If success Then
            GraphsOfParams.Add(nameGraph, newGrafOfParam)

            Dim newGraphByParameter As FormPatternGraphByParameter = New FormPatternGraphByParameter(ParentForm, nameGraph, newGrafOfParam)
            ' при создании автоматом добавляется в коллекцию
            newGraphByParameter.Show()
        End If

        Return success
    End Function

    ''' <summary>
    ''' Проверка Формы
    ''' </summary>
    ''' <param name="nameGraph"></param>
    ''' <returns></returns>
    Private Function CheckForm(ByVal nameGraph As String) As Boolean
        If GraphsOfParams.ContainsKey(nameGraph) Then
            Const caption As String = "Ошибка добавления нового графика"
            Dim text As String = $"График {nameGraph} в коллекции уже существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Класс графика от параметров используется как хранилище для создания, записи и восстановления настроек.
    ''' </summary>
    Friend Class GraphOfParameter
        Implements IEnumerable

        Friend Class Axis
            Implements IEnumerable
            Private mParameters As Dictionary(Of String, Parameter)

            Public Sub New(ByVal inNameAxis As String, ByVal inIsUseFormula As Boolean, ByVal min As Double, ByVal max As Double)
                IsUseFormula = inIsUseFormula

                If inIsUseFormula = True Then
                    mParameters = New Dictionary(Of String, Parameter)
                Else
                    ' по умолчанию. далее необходимо настроить
                    ParameterForAxis = New Parameter("Ввести", "Ввести", 1, False)
                End If

                NameAxis = inNameAxis
                Me.Min = min
                Me.Max = max
            End Sub

            Public ReadOnly Property Parameters() As Dictionary(Of String, Parameter)
                Get
                    Return mParameters
                End Get
            End Property

            Public Property IsUseFormula() As Boolean

            Public Property NameAxis() As String

            Public Property Min() As Double

            Public Property Max() As Double

            ''' <summary>
            ''' Параметр Для Оси
            ''' </summary>
            ''' <returns></returns>
            Public Property ParameterForAxis() As Parameter

            Public Property Formula() As String

            Default Public ReadOnly Property Item(ByVal key As String) As Parameter
                Get
                    Return mParameters.Item(key)
                End Get
            End Property

            Public ReadOnly Property Count() As Integer
                Get
                    Return mParameters.Count()
                End Get
            End Property

            Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return mParameters.GetEnumerator
            End Function

            Public Sub Remove(ByVal key As String)
                ' если целый тип то, по плавающему индексу, а если строковый то по ключу
                mParameters.Remove(key)
            End Sub

            Public Sub Clear()
                mParameters.Clear()
            End Sub

            Protected Overrides Sub Finalize()
                mParameters = Nothing
                MyBase.Finalize()
            End Sub

            Public Sub Add(ByVal inName As String, ByVal inNameChannel As String, ByVal inIndexInArrayParameters As Integer, ByVal inReduction As Boolean)
                Dim newParameter As Parameter = New Parameter(inName, inNameChannel, inIndexInArrayParameters, inReduction)

                If Not IsCheckParameter(newParameter) Then Exit Sub

                mParameters.Add(inName, newParameter)
            End Sub

            Public Function IsCheckParameter(ByRef newParameter As Parameter) As Boolean
                If mParameters.ContainsValue(newParameter) Then
                    Const caption As String = "Ошибка добавления параметра"
                    Dim text As String = $"Параметр ссылающийся на канал {newParameter.NameChannel} уже существует!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Return False
                End If

                Return True
            End Function

            Friend Class Parameter
                ' <Параметр Имя="ARG1" ИмяКанала="Bo" ИндексВМассиве="25" Приведение="False" ></Параметр>
                Public Sub New(ByVal inName As String, ByVal inNameChannel As String, ByVal inIndexInArrayParameters As Integer, ByVal inReduction As Boolean)
                    Name = inName
                    NameChannel = inNameChannel
                    IndexInArrayParameters = inIndexInArrayParameters
                    Reduction = inReduction
                End Sub

                Public Property Name() As String

                Public Property NameChannel() As String

                ''' <summary>
                ''' Индекс ВМассиве Параметров
                ''' </summary>
                ''' <returns></returns>
                Public Property IndexInArrayParameters() As Integer

                ''' <summary>
                ''' Приведение
                ''' </summary>
                ''' <returns></returns>
                Public Property Reduction() As Boolean
            End Class
        End Class

        ''' <summary>
        ''' График Границ
        ''' </summary>
        Friend Class LimitationGraph
            Implements IEnumerable
            Private mPointGraph As Dictionary(Of Integer, PointGraph)

            Public Sub New()
                mPointGraph = New Dictionary(Of Integer, PointGraph)
            End Sub

            Public Sub New(ByVal inNameLimitGraph As String)
                NameLimitGraph = inNameLimitGraph
                mPointGraph = New Dictionary(Of Integer, PointGraph)
            End Sub

            Public ReadOnly Property PointsGraphDictionary() As Dictionary(Of Integer, PointGraph)
                Get
                    Return mPointGraph
                End Get
            End Property

            Private ReadOnly Property Item(ByVal indexKey As Integer) As PointGraph
                Get
                    Return mPointGraph.Item(indexKey)
                End Get
            End Property

            Private ReadOnly Property Count() As Integer
                Get
                    Return mPointGraph.Count()
                End Get
            End Property

            Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return mPointGraph.GetEnumerator
            End Function

            Private Sub Remove(ByVal indexKey As Integer)
                'если целый тип, то по плавающему индексу, а если строковый то по ключу
                mPointGraph.Remove(indexKey)
            End Sub

            Private Sub Clear()
                mPointGraph.Clear()
            End Sub

            Protected Overrides Sub Finalize()
                mPointGraph = Nothing
                MyBase.Finalize()
            End Sub

            ''' <summary>
            ''' Имя Графика Границ
            ''' </summary>
            ''' <returns></returns>
            Public Property NameLimitGraph() As String
            Public Property Color() As String
            Public Property LineStyle() As String

            Public Sub Add(ByVal index As Integer, ByVal X As Double, ByVal Y As Double)
                If Not CheckPoints(index) Then Exit Sub

                mPointGraph.Add(index, New PointGraph(index, X, Y))
            End Sub

            Private Function CheckPoints(ByVal index As Integer) As Boolean
                If mPointGraph.ContainsKey(index) Then
                    Const caption As String = "Ошибка добавления точки"
                    Dim text As String = $"Точка с индексом {index} уже существует!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Return False
                End If

                If index < 0 Then
                    Const caption As String = "Ошибка добавления точки"
                    Const text As String = "Индекс точки быть в больше 0!"
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                    Return False
                End If

                Return True
            End Function

            Friend Class PointGraph
                Public Sub New(ByVal inindex As Integer, ByVal X As Double, ByVal Y As Double)
                    Index = inindex
                    Me.X = X
                    Me.Y = Y
                End Sub

                ''' <summary>
                ''' НомерИндекса
                ''' </summary>
                ''' <returns></returns>
                Public Property Index() As Integer

                Public Property X() As Double

                Public Property Y() As Double
            End Class
        End Class

        Private axesGraph As Dictionary(Of String, Axis)
        Private limitationGraphs As Dictionary(Of String, LimitationGraph)
        Private ReadOnly ParentForm As FormMain

        Public Sub New(inParentForm As FormMain)
            axesGraph = New Dictionary(Of String, Axis)
            limitationGraphs = New Dictionary(Of String, LimitationGraph)
            ParentForm = inParentForm
        End Sub

        Public Sub New(ByVal inNameGraph As String, inParentForm As FormMain)
            NameGraph = inNameGraph
            axesGraph = New Dictionary(Of String, Axis)
            limitationGraphs = New Dictionary(Of String, LimitationGraph)
            ParentForm = inParentForm
        End Sub

        Public ReadOnly Property AxisGraph() As Dictionary(Of String, Axis)
            Get
                Return axesGraph
            End Get
        End Property

        ''' <summary>
        ''' Все Графики Границ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AllLimitationGraphs() As Dictionary(Of String, LimitationGraph)
            Get
                Return limitationGraphs
            End Get
        End Property

        Private ReadOnly Property ItemAxis(ByVal key As String) As Axis
            Get
                Return axesGraph.Item(key)
            End Get
        End Property

        ''' <summary>
        ''' Item График Границ
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        Private ReadOnly Property ItemLimitationGraph(ByVal key As String) As LimitationGraph
            Get
                Return limitationGraphs.Item(key)
            End Get
        End Property

        ''' <summary>
        ''' Count Ось
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property CountAxis() As Integer
            Get
                Return axesGraph.Count()
            End Get
        End Property

        ''' <summary>
        ''' Число График Границ
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property CountLimitGraph() As Integer
            Get
                Return limitationGraphs.Count()
            End Get
        End Property

        Public Function GetEnumeratorAxis() As IEnumerator Implements IEnumerable.GetEnumerator
            Return axesGraph.GetEnumerator
        End Function

        ''' <summary>
        ''' Удалить Ось
        ''' </summary>
        ''' <param name="key"></param>
        Private Sub RemoveAxis(ByVal key As String)
            'если целый тип то, по плавающему индексу, а если строковый то по ключу
            axesGraph.Remove(key)
        End Sub

        Private Sub RemoveGraph(ByVal key As String)
            'если целый тип то, по плавающему индексу, а если строковый то по ключу
            limitationGraphs.Remove(key)
        End Sub

        Private Sub ClearAxis()
            ''здесь удаление по ключу, а он строковый
            'не работает
            'Dim oneInst As Условие
            'For Each oneInst In mCol
            '    mCol.Remove(oneInst.ID.ToString)
            'Next
            'Dim I As Integer
            'With mCol
            '    For I = .Count To 1 Step -1
            '        .Remove(I)
            '    Next
            'End With
            axesGraph.Clear()
        End Sub

        Public Sub ClearGraph()
            limitationGraphs.Clear()
        End Sub

        Protected Overrides Sub Finalize()
            limitationGraphs = Nothing
            axesGraph = Nothing
            MyBase.Finalize()
        End Sub

        Public Property NameGraph() As String

        Public Property IsTestPass() As Boolean

        Private Sub AddAxis(ByVal nameAxis As String, ByVal useFormula As Boolean, ByVal min As Double, ByVal max As Double)
            If Not IsCheckAxis(nameAxis) Then Exit Sub

            axesGraph.Add(nameAxis, New Axis(nameAxis, useFormula, min, max))
        End Sub

        ''' <summary>
        ''' Проверка Ось
        ''' </summary>
        ''' <param name="nameAxis"></param>
        ''' <returns></returns>
        Private Function IsCheckAxis(ByVal nameAxis As String) As Boolean
            If axesGraph.ContainsKey(nameAxis) Then
                Const caption As String = "Ошибка добавления оси"
                Dim text As String = $"Ось с именем {nameAxis} уже существует!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return False
            End If

            If Not (nameAxis = XmlAttributeX OrElse nameAxis = XmlAttributeY) Then
                Const caption As String = "Ошибка добавления оси"
                Const text As String = "Имя оси должно быть равно X или Y!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                'Exit Function
                Return False
            End If

            Return True
        End Function

        Private Sub AddGraph(ByVal nameLimitationGraph As String)
            If Not IsCheckGraph(nameLimitationGraph) Then Exit Sub

            limitationGraphs.Add(nameLimitationGraph, New LimitationGraph(nameLimitationGraph))
        End Sub

        ''' <summary>
        ''' Проверка График
        ''' </summary>
        ''' <param name="nameLimitationGraph"></param>
        ''' <returns></returns>
        Private Function IsCheckGraph(ByVal nameLimitationGraph As String) As Boolean
            If limitationGraphs.ContainsKey(nameLimitationGraph) Then
                Const caption As String = "Ошибка добавления графика границ"
                Dim text As String = $"График границ с именем {nameLimitationGraph} уже существует!"
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Конфигурировать ClassGrafOfParam и отдать флаг результата
        ''' </summary>
        ''' <returns></returns>
        Friend Function CheckCorrectnessGrafOfParam() As Boolean
            Dim findingGraphsList As IEnumerable(Of XElement) = GetFoundedGraphsList()

            If findingGraphsList.Any Then
                If Not PopulateAxes(findingGraphsList(0)) Then Return False
                PopulationLimitationGraph(findingGraphsList(0))
                IsTestPass = True
            End If

            Return True
        End Function

        ''' <summary>
        ''' Выдать список Xml узлов с именем как в текстовом поле.
        ''' </summary>
        ''' <returns></returns>
        Private Function GetFoundedGraphsList() As IEnumerable(Of XElement)
            Return From el In ParentForm.XDoc.Root...<ИмяГрафика>
                   Where el.@Наименование = NameGraph
                   Select el
        End Function

        ''' <summary>
        ''' Конфигурировать Оси X и Y
        ''' </summary>
        ''' <param name="foundGraph"></param>
        Private Function PopulateAxes(foundGraph As XElement) As Boolean
            Dim axesXElementList As IEnumerable(Of XElement) = From axis In foundGraph.<Ось>
                                                               Select axis
            For Each itemAxisXElement As XElement In axesXElementList
                ' анализ значения атрибута 
                Dim isUseFormula As Boolean = Convert.ToBoolean(itemAxisXElement.Attribute(XmlAttributeIsUseFormula).Value)
                Dim nameAxis As String = itemAxisXElement.Attribute(XmlAttributeAxisName).Value

                AddAxis(nameAxis, isUseFormula,
                        CDbl(itemAxisXElement.Attribute(XmlAttributeMin).Value),
                        CDbl(itemAxisXElement.Attribute(XmlAttributeMax).Value))

                If Not PopulateAxis(itemAxisXElement, nameAxis, isUseFormula) Then Return False
            Next

            Return True
        End Function

        ''' <summary>
        ''' Параметризованная процедура Конфигурировавания Оси
        ''' </summary>
        ''' <param name="axisXElement"></param>
        ''' <param name="isUseFormula"></param>
        Private Function PopulateAxis(axisXElement As XElement, nameAxis As String, isUseFormula As Boolean) As Boolean
            If isUseFormula Then
                For Each itemParameter As XElement In axisXElement.Elements(XmlNodeParameter)
                    'If itemParameter.Name = XmlNodeParameter Then
                    Dim name As String = itemParameter.Attribute(XmlAttributeName).Value
                    Dim nameChannel As String = itemParameter.Attribute(XmlAttributeNameChannel).Value
                    ' эдесь приведение не используется
                    Dim reduction As String = CStr(itemParameter.Attribute(XmlAttributeReduce))
                    ' сделать проверку на присутствие канала
                    Dim index As Integer ' Индекс в массиве параметров

                    If IsSearchIndexFromName(nameChannel, index) Then
                        AxisGraph(nameAxis).Add(name, nameChannel, index, CBool(reduction))
                    Else
                        MessageChannelIsNotFound(nameChannel)
                        Return False
                    End If
                Next

                ' на том же уровне
                If GetTestFormulaFunction(axisXElement.Elements(XmlNodeFormula).First.Value) Then
                    AxisGraph(nameAxis).Formula = axisXElement.Elements(XmlNodeFormula).First.Value
                Else
                    Return False
                End If
            Else ' только один параметр
                Dim parameterXElement As XElement = axisXElement.Elements(XmlNodeParameter).First
                ' эдесь имя не используется
                Dim nameChannel As String = parameterXElement.Attribute(XmlAttributeNameChannel).Value
                Dim index As Integer ' Индекс в массиве параметров

                If IsSearchIndexFromName(nameChannel, index) Then
                    AxisGraph(nameAxis).ParameterForAxis.Name = CStr(parameterXElement.Attribute(XmlAttributeName))
                    AxisGraph(nameAxis).ParameterForAxis.NameChannel = nameChannel
                    AxisGraph(nameAxis).ParameterForAxis.IndexInArrayParameters = index
                    ' эдесь приведение может быть использовано
                    AxisGraph(nameAxis).ParameterForAxis.Reduction = Convert.ToBoolean(parameterXElement.Attribute(XmlAttributeReduce).Value)
                Else
                    MessageChannelIsNotFound(nameChannel)
                    Return False
                End If
            End If

            Return True
        End Function

        ''' <summary>
        ''' Конфигурирование линий ограничений
        ''' </summary>
        ''' <param name="foundGraph"></param>
        Private Sub PopulationLimitationGraph(foundGraph As XElement)
            Dim lineTUXElementList As IEnumerable(Of XElement) = From lineTU In foundGraph.<ГрафикТУ>
                                                                 Select lineTU
            For Each itemlineTU As XElement In lineTUXElementList
                Dim nameTU As String = itemlineTU.Attribute(XmlAttributeNameTU).Value

                AddGraph(nameTU)
                AllLimitationGraphs(nameTU).Color = itemlineTU.Attribute(XmlAttributeColor).Value
                AllLimitationGraphs(nameTU).LineStyle = itemlineTU.Attribute(XmlAttributeLineStyle).Value

                For Each itemPointXElement As XElement In itemlineTU.Elements(XmlNodePoint)
                    AllLimitationGraphs(nameTU).Add(CInt(itemPointXElement.Attribute(XmlAttributeIndex).Value),
                                                    CDbl(itemPointXElement.Attribute(XmlAttributeX).Value),
                                                    CDbl(itemPointXElement.Attribute(XmlAttributeY).Value))
                Next
            Next
        End Sub

        ''' <summary>
        ''' Поиск индекса в массиве параметров по имени канала
        ''' и выдача результата.
        ''' </summary>
        ''' <param name="nameChannel"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Private Function IsSearchIndexFromName(ByVal nameChannel As String, ByRef index As Integer) As Boolean
            With ParentForm
                If .IsBeforeThatHappenLoadDbase Then
                    For I As Integer = 1 To UBound(.IndexParameters)
                        If ParametersShaphotType(.IndexParameters(I)).NameParameter = nameChannel Then
                            index = I - 1
                            Return True
                        End If
                    Next
                Else
                    For I As Integer = 1 To UBound(.IndexParameters)
                        If ParametersType(.IndexParameters(I)).NameParameter = nameChannel Then
                            index = I - 1
                            Return True
                        End If
                    Next
                End If
            End With

            Return False
        End Function

        ''' <summary>
        ''' Сообщение канал для графика от параметра не найден
        ''' </summary>
        ''' <param name="nameChannel"></param>
        Private Sub MessageChannelIsNotFound(ByVal nameChannel As String)
            Const caption As String = "Новый график ограничения"
            Dim text As String = $"Канал с именем {nameChannel} не найден!{vbCrLf}Загрузка графика будет прекращена.{vbCrLf}Необходимо отредактировать график в редакторе."
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Sub

        ''' <summary>
        ''' Выдать результат проверки функции
        ''' </summary>
        ''' <param name="expression"></param>
        ''' <returns></returns>
        Private Function GetTestFormulaFunction(ByVal expression As String) As Boolean
            Dim eval As New JScriptUtil.ExpressionEvaluator()
            Dim result As Double

            expression = expression.Replace("ARG", "1")
            Try
                result = CDbl(eval.Evaluate(expression))
            Catch ex As Exception
                Const caption As String = "Ошибка в формуле"
                Dim text As String = $"{ex}{vbCrLf}{vbCrLf}Математическое выражение содержит ошибку.{vbCrLf}Загрузка графика будет прекращена.{vbCrLf}Необходимо отредактировать формулу в редакторе."
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
                Return False
            End Try

            Return True
        End Function
    End Class
End Class
