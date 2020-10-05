Imports System.Collections.Generic
Imports System.Text

Public Class ChannelInput
    ' необходимы следующие атрибуты для предполагаемых видов сигналов

    ' 1 таблица
    ' id            id
    ' Номер			NumberCh
    ' Имя			Name
    ' Pin			Pin
    ' Экр. Имя		Scr_Name
    ' Вкл/Выкл		State
    ' Экр. Ед.Изм.	Scr_EdIzm
    ' Физ. Ед.Изм.	Ch_Unit
    ' Группа		GroupCh

    Public Sub New()
    End Sub

    Public Property Id As Integer
    Public Property NumberCh As Integer
    Public Property Name As String
    Public Property Pin As String
    Public Property ScrName As String
    Public Property State As String
    Public Property ScrEdIzm As String
    Public Property ChUnit As String
    Public Property GroupCh As String
End Class

Public Class ChannelOutput
    ' 2 таблица
    '   id			        id
    '   keyConfig		    keyConfig
    '   Номер			    НомерПараметра
    '   Имя Канала		    Name
    '   Экр. Имя		    НаименованиеПараметра
    '   Ед. Измерения	    ЕдиницаИзмерения
    '   Допуск Мин		    ДопускМинимум
    '   Допуск Макс		    ДопускМаксимум
    '   Разнос Y мин		РазносУмин
    '   Разнос Y макс		РазносУмакс
    '   Авар. Значение Мин 	АварийноеЗначениеМин
    '   Авар. Значение Макс	АварийноеЗначениеМакс
    '   Группа			    GroupCh

    Public Property Id As Integer
    Public Property KeyConfig As Integer
    Public Property NumberParameter As Integer ' НомерПараметра
    Public Property Name As String
    Public Property NameParameter As String ' НаименованиеПараметра
    Public Property UnitOfMeasure As String ' ЕдиницаИзмерения
    Public Property LowerLimit As Single ' ДопускМинимум 
    Public Property UpperLimit As Single ' ДопускМаксимум 
    Public Property RangeYmin As Single ' РазносУмин
    Public Property RangeYmax As Single ' РазносУмакс
    Public Property AlarmValueMin As Single ' АварийноеЗначениеМин
    Public Property AlarmValueMax As Single ' АварийноеЗначениеМакс
    Public Property GroupCh As String
    Public Property IsVisible As Boolean ' Видимость
    Public Property IsVisibleRegistration As Boolean ' ВидимостьРегистратор
    Public Property NumberFormula As Integer ' НомерФормулы
    Public Property Pin As String

    Public Sub New()
    End Sub
End Class

Public Class ManagerChannelsInput
    Implements IEnumerable
    '    Inherits System.ComponentModel.BindingList(Of Channel)

    Public ReadOnly Property Count() As Integer
        Get
            Return mChannels.Count
        End Get
    End Property

    Public ReadOnly Property CollectionsChannels() As Dictionary(Of String, ChannelInput)
        Get
            Return mChannels
        End Get
    End Property

    Public ReadOnly Property Item(ByVal indexKey As String) As ChannelInput
        Get
            Return mChannels.Item(indexKey)
        End Get
    End Property

    Private mChannels As Dictionary(Of String, ChannelInput)

    Public Sub New()
        mChannels = New Dictionary(Of String, ChannelInput)
    End Sub

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mChannels.GetEnumerator
    End Function

    Public Sub Remove(ByRef indexKey As String)
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mChannels.Remove(indexKey)
    End Sub

    Public Sub Clear()
        mChannels.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        mChannels = Nothing
        MyBase.Finalize()
    End Sub

    Private ReadOnly mRepetitionChannelNames As New StringBuilder
    Public ReadOnly Property RepetitionChannelNames As String
        Get
            If ModifyChannelNames IsNot Nothing Then
                Dim count As Integer
                For Each ChangedName As String In ModifyChannelNames
                    mRepetitionChannelNames.AppendLine(ChangedName)
                    count += 1
                    If count > 30 Then
                        mRepetitionChannelNames.AppendLine("и так далее ...")
                        Exit For
                    End If
                Next
            End If

            Return mRepetitionChannelNames.ToString
        End Get
    End Property

    Private mIsRepetitionChannelNames As Boolean
    ''' <summary>
    ''' Есть Повторы Каналов
    ''' </summary>
    Public ReadOnly Property IsRepetitionChannelNames As Boolean
        Get
            If ModifyChannelNames IsNot Nothing Then
                mIsRepetitionChannelNames = True
            End If
            Return mIsRepetitionChannelNames
        End Get
    End Property

    ''' <summary>
    ''' Изменённые Имена Каналов
    ''' </summary>
    Private ModifyChannelNames As List(Of String)

    Public Sub AddChannel(ByVal nameChannel As String, ByVal channelIn As ChannelInput)
        If Not CheckChannel(nameChannel) Then Exit Sub

        channelIn.ScrName = ReplaceNameSlesh(channelIn.ScrName)

        Dim OldName As String = channelIn.ScrName

        CheckWindowsName(channelIn)

        If OldName <> channelIn.ScrName Then
            If ModifyChannelNames Is Nothing Then
                ModifyChannelNames = New List(Of String)
            End If

            Dim text As String = String.Format("{0,-30}  ->   {1,30}", OldName, channelIn.ScrName)
            ModifyChannelNames.Add(text)
        End If

        mChannels.Add(nameChannel, channelIn)
    End Sub

    Private Function CheckChannel(ByVal nameChannel As String) As Boolean
        If mChannels.ContainsKey(nameChannel) Then
            Const caption As String = "Ошибка добавления канала"
            Dim text As String = $"Имя канала {nameChannel} уже существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Проверка Экранного Имени
    ''' </summary>
    ''' <param name="channelIn"></param>
    Private Sub CheckWindowsName(ByRef channelIn As ChannelInput)
        For Each itemChannelInput As ChannelInput In mChannels.Values
            If channelIn.ScrName.ToUpper = itemChannelInput.ScrName.ToUpper Then
                channelIn.ScrName &= "_1"
                CheckWindowsName(channelIn)
                Exit Sub
            End If
        Next
    End Sub

    ''' <summary>
    ''' Заменить \ на символ _
    ''' </summary>
    ''' <param name="Scr_Name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReplaceNameSlesh(Scr_Name As String) As String
        If Scr_Name.Contains("\") Then
            Return Scr_Name.Replace("\", "_")
        Else
            Return Scr_Name
        End If
    End Function
End Class

''' <summary>
''' для заполнения таблиц 
''' </summary>
Public Class ChannelsInputBindingList
    Inherits System.ComponentModel.BindingList(Of ChannelInput)
End Class

Public Class ManagerChannelsOutput
    Implements IEnumerable
    '    Inherits System.ComponentModel.BindingList(Of Channel)
    Private mChannels As Dictionary(Of String, ChannelOutput)

    Public ReadOnly Property Count() As Integer
        Get
            Return mChannels.Count
        End Get
    End Property

    Public ReadOnly Property CollectionsChannels() As Dictionary(Of String, ChannelOutput)
        Get
            Return mChannels
        End Get
    End Property

    Public Sub New()
        mChannels = New Dictionary(Of String, ChannelOutput)
    End Sub

    Public ReadOnly Property Item(ByVal indexKey As String) As ChannelOutput
        Get
            Return mChannels.Item(indexKey)
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mChannels.GetEnumerator
    End Function

    Public Sub Remove(ByRef indexKey As String)
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mChannels.Remove(indexKey)
    End Sub

    Public Sub Clear()
        mChannels.Clear()
    End Sub

    Protected Overrides Sub Finalize()
        mChannels = Nothing
        MyBase.Finalize()
    End Sub

    Public Sub AddChannel(ByVal nameChannel As String, ByVal channelOut As ChannelOutput)
        If Not CheckChannel(nameChannel) Then Exit Sub

        mChannels.Add(nameChannel, channelOut)
    End Sub

    ''' <summary>
    ''' Проверка Канала
    ''' </summary>
    ''' <param name="nameChannel"></param>
    ''' <returns></returns>
    Private Function CheckChannel(ByVal nameChannel As String) As Boolean
        If mChannels.ContainsKey(nameChannel) Then
            Const caption As String = "Ошибка добавления канала"
            Dim text As String = $"Имя канала {nameChannel} уже существует!"
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RegistrationEventLog.EventLog_MSG_APPLICATION_MESSAGE($"<{caption}> {text}")
            Return False
        End If

        Return True
    End Function
End Class

''' <summary>
''' для заполнения таблиц 
''' </summary>
Public Class ChannelsOutputBindingList
    Inherits System.ComponentModel.BindingList(Of ChannelOutput)
End Class


Public Class DoubleValue
    Public Sub New(inNumberCh As Integer, inValueCh As Double)
        NumberCh = inNumberCh
        ValueCh = inValueCh
    End Sub

    Public Property NumberCh As Integer
    Public Property ValueCh As Double
End Class

Public Class DoubleValueList
    Inherits List(Of DoubleValue)
End Class

Public Class StrTimeValue
    Public Sub New(inNumberCh As Integer, inValueCh As String)
        NumberCh = inNumberCh
        ValueCh = inValueCh
    End Sub

    Public Property NumberCh As Integer
    Public Property ValueCh As String
End Class

Public Class StrTimeValueList
    Inherits List(Of StrTimeValue)
End Class