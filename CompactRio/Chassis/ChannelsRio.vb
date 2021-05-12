Imports System.Collections.Generic
''' <summary>
''' Инкапсуляция коллекции каналов посредством словаря.
''' </summary>
''' <remarks></remarks>
Public Class ChannelsRio
    Implements IEnumerable
    Implements IEnumerable(Of ChannelRio)

    Private ReadOnly mChannels As Dictionary(Of String, ChannelRio)

    Public Sub New()
        mChannels = New Dictionary(Of String, ChannelRio)
    End Sub

    Default Public ReadOnly Property Item(ByVal Key As String) As ChannelRio
        Get
            Return mChannels.Item(Key)
        End Get
    End Property

    Public Iterator Function GetEnumerator() As IEnumerator(Of ChannelRio) Implements IEnumerable(Of ChannelRio).GetEnumerator
        For Each key As String In mChannels.Keys.ToArray
            Yield mChannels(key)
        Next
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

    Public ReadOnly Property Count() As Integer
        Get
            Return mChannels.Count
        End Get
    End Property

    Public ReadOnly Property Channels() As Dictionary(Of String, ChannelRio)
        Get
            Return mChannels
        End Get
    End Property

    Public Sub Remove(Key As String)
        ' удаление по номеру или имени или объекту?
        ' если целый тип то по плавающему индексу, а если строковый то по ключу
        mChannels.Remove(Key)
    End Sub

    Public Sub Clear()
        mChannels.Clear()
    End Sub

    Public Sub Add(ByVal nameChannel As String, ByVal varChannel As ChannelRio)
        If mChannels.ContainsKey(nameChannel) Then
            MessageBox.Show($"Имя канала: <{nameChannel}> уже существует!", "Ошибка добавления канала", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        mChannels.Add(nameChannel, varChannel)
    End Sub
End Class