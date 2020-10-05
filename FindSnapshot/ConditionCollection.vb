Imports System.Collections.Generic
Imports System.Xml.Linq
Imports Registration.FormConditionFind

''' <summary>
''' Коллекция условий поиска
''' </summary>
Friend Class ConditionCollection
    Implements IEnumerable

    Default Public ReadOnly Property Item(ByVal indexKey As Integer) As Condition
        Get
            Return mConditions.Item(indexKey)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Count = mConditions.Count()
        End Get
    End Property

    'Public ReadOnly Property NewEnum() As stdole.IUnknown
    '    Get
    '        'this property allows you to enumerate
    '        'this collection with the For...Each syntax
    '        NewEnum = mCol.GetEnumerator() 'доработка mCol._NewEnum()
    '    End Get
    'End Property

    Public ReadOnly Property GetDictionaryConditions() As Dictionary(Of Integer, Condition)
        Get
            Return mConditions
        End Get
    End Property

    Private ReadOnly ParentForm As FormConditionFind
    Private mConditions As Dictionary(Of Integer, Condition)

    Public Sub New(inParentForm As FormConditionFind)
        MyBase.New()
        ParentForm = inParentForm
        mConditions = New Dictionary(Of Integer, Condition)
    End Sub

    Public Function Add(number As Integer) As Condition
        Dim name As String = cCondition & number

        Dim newCondition As Condition = New Condition(name, number, ParentForm) With {.Name = name} ' Создать новый объект
        mConditions.Add(number, newCondition)

        Return newCondition
    End Function

    ''' <summary>
    ''' Даёт ошибку
    ''' </summary>
    ''' <returns></returns>
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mConditions.GetEnumerator
    End Function

    Public Sub Remove(ByRef indexKey As Integer)
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mConditions.Remove(indexKey)
    End Sub

    Public Sub Clear()
        'не работает
        'For Each oneInst In mCol
        '    mCol.Remove(oneInst.ID.ToString)
        'Next

        'For I As Integer = mCol.Count To 1 Step -1
        '    mCol.Remove(I)
        'Next

        mConditions.Clear()
    End Sub

    ''' <summary>
    ''' Создать элемент XmlElement {nameConfiguration}
    ''' </summary>
    ''' <param name="nameConfiguration"></param>
    ''' <returns></returns>
    Public Function GetXElement(nameConfiguration As String) As XElement
        ' создать элемент XmlElement nameConfiguration
        '- <ЧтоЗаУсловие Примечание="Имя общего условия">
        '  </ЧтоЗаУсловие>
        Dim newConditionXmlElement As New XElement(cWhatIsCondition)
        newConditionXmlElement.SetAttributeValue("Примечание", nameConfiguration)

        ' в цикле по коллекции добавляем условия в узел txtИмяПерекладки.Text
        For Each itemCondition As Condition In mConditions.Values
            ' в элемент TextBoxNameConfiguration.Text добавили условие из коллекции Conditions
            '  </ДочернееУсловиеN>
            newConditionXmlElement.Add(itemCondition.CreateChildConditionXmlElement())
        Next

        Return newConditionXmlElement
    End Function

    ''' <summary>
    ''' Проверить слайдеры на равенство.
    ''' Проверить условия в диапазоне на равенство.
    ''' </summary>
    Public Function CheckCorrectConditions() As Boolean
        Dim message As String = String.Empty

        For Each itemCondition As Condition In mConditions.Values
            itemCondition.CheckCorrectCondition(message)
        Next

        If message <> String.Empty Then
            ' СтартВерхнееРавноСтартНижнее OrElse ПерегибВерхнееРавноПерегибНижнее OrElse ФинишВерхнееРавноФинишНижнее OrElse ВерхнееЗначениеУсловияРавноНижнееЗначениеУсловия
            Const Caption As String = "Некорректные условия"
            message &= $"{vbCrLf}{vbCrLf}Хотите исправить условия?"
            RegistrationEventLog.EventLog_MSG_RELEVANT_QUESTION($"<{Caption}> {message}")
            If MessageBox.Show(ParentForm, message, Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                Return False
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' Очистка для в коллеции условий формы что для данного кадра условие найдено
    ''' </summary>
    Public Sub ResetAllIsFounded()
        For Each itemCondition As Condition In mConditions.Values
            itemCondition.ResetIsFounded()
        Next
    End Sub

    Protected Overrides Sub Finalize()
        mConditions = Nothing
        MyBase.Finalize()
    End Sub
End Class