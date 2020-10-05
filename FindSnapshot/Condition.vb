Imports System.Xml.Linq
Imports Registration.FormConditionFind

''' <summary>
''' Условие поиска
''' </summary>
Friend Class Condition
    Public Property Name() As String
    Public Property ID() As Integer

    Public Overrides Function ToString() As String
        Return $"{ID}, {Name}"
    End Function

#Region "МинМах Properties"
    ''' <summary>
    ''' Максимум
    ''' </summary>
    ''' <returns></returns>
    Public Property Maximum() As Double
    ''' <summary>
    ''' Минимум
    ''' </summary>
    ''' <returns></returns>
    Public Property Minimum() As Double
    ''' <summary>
    ''' Кадр Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property Frame() As FrameCondition
#End Region

    ''' <summary>
    ''' Индекс Параметра
    ''' </summary>
    ''' <returns></returns>
    Public Property IndexParameter() As Integer ' индекс параметра в массиве имен
    ''' <summary>
    ''' Найден На Данном Кадре
    ''' </summary>
    ''' <returns></returns>
    Public Property IsFoundedOnThisFrame() As Boolean
    ''' <summary>
    ''' Номер Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property NumberCondition() As Integer
    ''' <summary>
    ''' Тип Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property ConditionTypeProperty() As ConditionType
    ''' <summary>
    ''' Закладка
    ''' </summary>
    ''' <returns></returns>
    Public Property MarkerProperty() As Marker

#Region "Значение"
    ''' <summary>
    ''' Имя Значение Параметр
    ''' </summary>
    ''' <returns></returns>
    Public Property NameValueParameter() As String
    ''' <summary>
    ''' Условие Отбора
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectiveConditionProperty() As SelectiveCondition
    ''' <summary>
    ''' Нижнее Значение Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property LowerValueCondition() As Double
    ''' <summary>
    ''' Верхнее Значение Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property UpperValueCondition() As Double
    ''' <summary>
    ''' Значение Условия
    ''' </summary>
    ''' <returns></returns>
    Public Property Condition() As Double
#End Region

#Region "Форма"
    ''' <summary>
    ''' Имя Форма Параметр
    ''' </summary>
    ''' <returns></returns>
    Public Property NameFormParameter() As String = "Параметр отсутствует"
    ''' <summary>
    ''' Максимальный Предел Параметра
    ''' </summary>
    ''' <returns></returns>
    Public Property MaxLimitParameter() As Double = 100.0
    ''' <summary>
    ''' Старт Верхнее
    ''' </summary>
    ''' <returns></returns>
    Public Property StartUpper() As Double = 100.0
    ''' <summary>
    ''' СтартНижнее
    ''' </summary>
    ''' <returns></returns>
    Public Property StartLover() As Double = 90.0
    ''' <summary>
    ''' Перегиб Верхнее
    ''' </summary>
    ''' <returns></returns>
    Public Property TwistTop() As Double = 100.0
    ''' <summary>
    ''' Перегиб Нижнее
    ''' </summary>
    ''' <returns></returns>
    Public Property TwistLower() As Double = 90.0
    ''' <summary>
    ''' Финиш Верхнее
    ''' </summary>
    ''' <returns></returns>
    Public Property FinishTop() As Double = 100.0
    ''' <summary>
    ''' Финиш Нижнее
    ''' </summary>
    ''' <returns></returns>
    Public Property FinishLower() As Double = 90.0
    ''' <summary>
    ''' Время Перегиба
    ''' </summary>
    ''' <returns></returns>
    Public Property TimeTwist() As Double = 5.0
    ''' <summary>
    ''' Ширина Временного Окна
    ''' </summary>
    ''' <returns></returns>
    Public Property WidthTemporaryWindow() As Double = 10.0
#End Region

    Private ReadOnly ParentForm As FormConditionFind

    Public Sub New(ByVal name As String, ByVal id As Integer, inParentForm As FormConditionFind)
        Me.Name = name
        Me.ID = id
        NumberCondition = id
        ParentForm = inParentForm
        SetConditionOnDefault()
    End Sub

    ''' <summary>
    ''' Выдать заголовок с описанием Условия
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAttributeCondition() As String
        If MarkerProperty = Marker.Trigger Then
            Return GetAttributeTriggerCondition()
        Else ' Шаблон
            Return $"шаблон {NameFormParameter}"
        End If
    End Function

    Public Function CreateChildConditionXmlElement() As XElement
        '- <ДочернееУсловиеN ИмяУсловия="УсловиеXXXX" ТипЗакладки=cTemplate>
        Dim conditionXElement As New XElement(cChildConditionN)
        conditionXElement.SetAttributeValue("ИмяУсловия", Name)

        If MarkerProperty = Marker.Trigger Then ' Триггер = 0
            PopulateChildConditionXmlElementTrigger(conditionXElement)
        Else ' Шаблон = 1
            PopulateChildConditionXmlElementTemplate(conditionXElement)
        End If

        Return conditionXElement
    End Function

    ''' <summary>
    ''' Копировать Условие
    ''' </summary>
    ''' <param name="sourse"></param>
    Public Sub CopyCondition(ByRef sourse As Condition)
        With sourse
            MarkerProperty = .MarkerProperty
            ConditionTypeProperty = .ConditionTypeProperty
            'Триггер
            NameValueParameter = .NameValueParameter
            SelectiveConditionProperty = .SelectiveConditionProperty
            LowerValueCondition = .LowerValueCondition
            UpperValueCondition = .UpperValueCondition
            Condition = .Condition
            ' Шаблон
            NameFormParameter = .NameFormParameter
            MaxLimitParameter = .MaxLimitParameter
            StartUpper = .StartUpper
            StartLover = .StartLover
            TwistTop = .TwistTop
            TwistLower = .TwistLower
            FinishTop = .FinishTop
            FinishLower = .FinishLower
            TimeTwist = .TimeTwist
            WidthTemporaryWindow = .WidthTemporaryWindow
        End With
    End Sub

    ''' <summary>
    ''' Установить свойства Условия по умолчанию
    ''' </summary>
    Private Sub SetConditionOnDefault()
        ' Триггер
        NameValueParameter = MissingParameter
        SelectiveConditionProperty = SelectiveCondition.Between
        LowerValueCondition = 0.0
        UpperValueCondition = 0.0
        Condition = 0.0
        ' Шаблн
        NameFormParameter = MissingParameter
        MaxLimitParameter = 100.0
        StartUpper = 100.0
        StartLover = 90.0
        TwistTop = 100.0
        TwistLower = 90.0
        FinishTop = 100.0
        FinishLower = 90.0
        TimeTwist = 10.0
        WidthTemporaryWindow = 20.0
    End Sub

    ''' <summary>
    ''' Установить свойства условия Триггер
    ''' </summary>
    ''' <param name="nameAttribute"></param>
    ''' <param name="value"></param>
    Public Sub SetTriggerConditionFromXmlAttribute(nameAttribute As String, value As String)
        Select Case nameAttribute
            Case cParameterTrigger
                NameValueParameter = value
            Case cConditionalTrigger
                If ParentForm.ConvertStringToEnumSelectiveCondition(value) = SelectiveCondition.FindMinimum OrElse ParentForm.ConvertStringToEnumSelectiveCondition(value) = SelectiveCondition.FindMaximum Then
                    ConditionTypeProperty = ConditionType.MinMax
                Else
                    ConditionTypeProperty = ConditionType.Mandatory
                End If
                SelectiveConditionProperty = ParentForm.ConvertStringToEnumSelectiveCondition(value)
            Case cEqualTrigger
                Condition = CDbl(value)
            Case cBetweenDownTrigger
                LowerValueCondition = CDbl(value)
            Case cBetweenUpTrigger
                UpperValueCondition = CDbl(value)
        End Select
    End Sub

    ''' <summary>
    ''' Установить свойства условия Шаблон
    ''' </summary>
    ''' <param name="nameAttribute"></param>
    ''' <param name="value"></param>
    Public Sub SetTemplateConditionFromXmlAttribute(nameAttribute As String, value As String)
        Select Case nameAttribute
            Case cParameterTemplate
                NameFormParameter = value
            Case cMaxLimit
                MaxLimitParameter = CDbl(value)
            Case cStartUp
                StartUpper = CDbl(value)
            Case cStartDown
                StartLover = CDbl(value)
            Case cBendUp
                TwistTop = CDbl(value)
            Case cBendDown
                TwistLower = CDbl(value)
            Case cFinishUp
                FinishTop = CDbl(value)
            Case cFinishDown
                FinishLower = CDbl(value)
            Case cTimeBend
                TimeTwist = CDbl(value)
            Case cWidthWindow
                WidthTemporaryWindow = CDbl(value)
        End Select
    End Sub

    ''' <summary>
    ''' Проверить слайдеры на равенство.
    ''' Проверить условия в диапазоне на равенство.
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub CheckCorrectCondition(ByRef message As String)
        If ConditionTypeProperty = ConditionType.Template Then
            ' Старт Верхнее Равно Старт Нижнее 
            If StartUpper = StartLover Then message &= $"{vbCrLf}В условии шаблона №:{ ID} [Старт Верхнее] = [Старт Нижнее]"
            'Перегиб Верхнее Равно Перегиб Нижнее  
            If TwistTop = TwistLower Then message &= $"{vbCrLf}В условии шаблона №:{ ID} [Перегиб Верхнее] = [Перегиб Нижнее]"
            ' Финиш Верхнее Равно Финиш Нижнее
            If FinishTop = FinishLower Then message &= $"{vbCrLf}В условии шаблона №:{ ID} [Финиш Верхнее] = [Финиш Нижнее]"
        Else ' Триггер
            If SelectiveConditionProperty = SelectiveCondition.OutOfRange OrElse SelectiveConditionProperty = SelectiveCondition.Between Then
                If LowerValueCondition = UpperValueCondition Then
                    ' Верхнее Значение Условия Равно Нижнее Значение Условия 
                    message &= $"{vbCrLf}В условии значения №:{ ID} [Верхнее Значение] = [Нижнее Значение]"
                ElseIf LowerValueCondition > UpperValueCondition Then
                    ' Верхнее Значение Условия Равно Нижнее Значение Условия
                    message &= $"{vbCrLf}В условии значения №:{ ID} [Нижнее Значение] больше [Верхнее Значение]"
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Очистка для в коллеции условий формы что для данного кадра условие найдено
    ''' </summary>
    Public Sub ResetIsFounded()
        If ConditionTypeProperty = ConditionType.Template Then IsFoundedOnThisFrame = False
    End Sub

    ''' <summary>
    ''' Добавить узлы спецификации для условия
    ''' </summary>
    ''' <param name="conditionXElement"></param>
    ''' <param name="name"></param>
    ''' <param name="innerText"></param>
    Private Sub AppendChildXmlElement(ByRef conditionXElement As XElement, name As String, innerText As String)
        Dim newXmlElement As New XElement(name, innerText)
        conditionXElement.Add(newXmlElement)
    End Sub

    ''' <summary>
    ''' Заполнить узлы спецификации для условия {Шаблон}
    ''' </summary>
    ''' <param name="conditionXElement"></param>
    Private Sub PopulateChildConditionXmlElementTemplate(ByRef conditionXElement As XElement)
        conditionXElement.SetAttributeValue("ТипЗакладки", cTemplate)

        '  <ИмяФормаПараметр>Вибрация</ИмяФормаПараметр> 
        AppendChildXmlElement(conditionXElement, cParameterTemplate, NameFormParameter)
        '  <МаксимальныйПределПараметра>100</МаксимальныйПределПараметра> 
        AppendChildXmlElement(conditionXElement, cMaxLimit, Format(MaxLimitParameter, "Fixed"))
        '  <СтартВерхнее>20</СтартВерхнее> 
        AppendChildXmlElement(conditionXElement, cStartUp, Format(StartUpper, "Fixed"))
        '  <СтартНижнее>10</СтартНижнее> 
        AppendChildXmlElement(conditionXElement, cStartDown, Format(StartLover, "Fixed"))
        '  <ПерегибВерхнее>100</ПерегибВерхнее> 
        AppendChildXmlElement(conditionXElement, cBendUp, Format(TwistTop, "Fixed"))
        '  <ПерегибНижнее>90</ПерегибНижнее> 
        AppendChildXmlElement(conditionXElement, cBendDown, Format(TwistLower, "Fixed"))
        '  <ФинишВерхнее>100</ФинишВерхнее> 
        AppendChildXmlElement(conditionXElement, cFinishUp, Format(FinishTop, "Fixed"))
        '  <ФинишНижнее>90</ФинишНижнее> 
        AppendChildXmlElement(conditionXElement, cFinishDown, Format(FinishLower, "Fixed"))
        '  <ВремяПерегиба>10</ВремяПерегиба> 
        AppendChildXmlElement(conditionXElement, cTimeBend, Format(TimeTwist, "Fixed"))
        '  <ШиринаВременногоОкна>20</ШиринаВременногоОкна> 
        AppendChildXmlElement(conditionXElement, cWidthWindow, Format(WidthTemporaryWindow, "Fixed"))
    End Sub

    ''' <summary>
    ''' Заполнить узлы спецификации для условия {Значение}
    ''' </summary>
    ''' <param name="conditionXElement"></param>
    Private Sub PopulateChildConditionXmlElementTrigger(ByRef conditionXElement As XElement)
        conditionXElement.SetAttributeValue("ТипЗакладки", cTriggerValue)
        ' Обязательное = 0
        ' MinMax = 1 'Обязательно если выполнено Обязательное при анализе
        ' Шаблон = 2 'если выполнено Обязательное и на данном снимке еще ни разу не было найдено
        '  <ИмяЗначениеПараметр>Обороты</ИмяЗначениеПараметр> 
        AppendChildXmlElement(conditionXElement, cParameterTrigger, NameValueParameter)
        '  <УсловиеОтбора>НайтиМинимум</УсловиеОтбора> 
        AppendChildXmlElement(conditionXElement, cConditionalTrigger, ConvertSelectiveConditionToString(SelectiveConditionProperty))

        If ConditionTypeProperty <> ConditionType.MinMax Then 'ТипУсловия = Обязательное
            If SelectiveConditionProperty = SelectiveCondition.Between OrElse SelectiveConditionProperty = SelectiveCondition.OutOfRange Then
                ' 3
                ' Между = 0' Вне = 1
                '  <НижнееЗначениеУсловия>12</НижнееЗначениеУсловия> 
                AppendChildXmlElement(conditionXElement, cBetweenDownTrigger, LowerValueCondition.ToString)
                '  <ВерхнееЗначениеУсловия>115</ВерхнееЗначениеУсловия> 
                AppendChildXmlElement(conditionXElement, cBetweenUpTrigger, UpperValueCondition.ToString)
            Else
                ' 2
                ' Равно = 2' НеРавно = 3' Больше = 4' Меньше = 5' БольшеИлиРавно = 6' МеньшеИлиРавно = 7
                '<ЗначениеУсловия>100</ЗначениеУсловия> 
                AppendChildXmlElement(conditionXElement, cEqualTrigger, Condition.ToString)
            End If
            'Else
            '    ' 1
            '    ' НайтиМаксимум = 8' НайтиМинимум = 9
        End If
    End Sub

    ''' <summary>
    ''' Аттрибуты Условия Trigger
    ''' </summary>
    ''' <returns></returns>
    Private Function GetAttributeTriggerCondition() As String
        'Dim headerText As String = $"{ NameValueParameter} {ConvertSelectiveConditionToString(SelectiveConditionProperty)} "

        'Select Case SelectiveConditionProperty
        '    Case SelectiveCondition.Between, SelectiveCondition.OutOfRange
        '        headerText += $"{LowerValueCondition} и {UpperValueCondition}"
        '    Case Else
        '        If SelectiveConditionProperty <> SelectiveCondition.FindMinimum AndAlso SelectiveConditionProperty <> SelectiveCondition.FindMaximum Then
        '            headerText += Condition.ToString
        '        End If
        'End Select

        Dim headerText As String = $"{NameValueParameter}"

        Select Case SelectiveConditionProperty
            Case SelectiveCondition.Between, SelectiveCondition.OutOfRange
                headerText += $" [{LowerValueCondition} {ConvertSelectiveConditionToString(SelectiveConditionProperty)} {UpperValueCondition}]"
            Case Else
                headerText += $" {ConvertSelectiveConditionToString(SelectiveConditionProperty)} "
                If SelectiveConditionProperty <> SelectiveCondition.FindMinimum AndAlso SelectiveConditionProperty <> SelectiveCondition.FindMaximum Then
                    headerText += Condition.ToString
                End If
        End Select

        Return headerText
    End Function
End Class
