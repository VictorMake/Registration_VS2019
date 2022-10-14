Imports System.Collections.Generic
Imports System.Xml.Linq

''' <summary>
''' Хранение настроек приложения в конфигурационном файле XML.
''' </summary>
''' <remarks></remarks>
Public Class ReadWriteIni
    Public Property PathXmlFile() As String

    Public Sub New(inPathXmlFile As String)
        PathXmlFile = inPathXmlFile
    End Sub

    ''' <summary>
    ''' Получить значение ключа.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="inPartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIni(inPartition As String, inSection As String, inKey As String, ByRef sDefault As String) As String
        ' создать документ
        Dim xmlDoc As XElement = XElement.Load(PathXmlFile)
        Return GetIni(xmlDoc, inPartition, inSection, inKey, sDefault)
    End Function

    ''' <summary>
    ''' Получить значение ключа.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="xmlDoc"></param>
    ''' <param name="inPartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIni(ByRef xmlDoc As XElement, inPartition As String, inSection As String, inKey As String, sDefault As String) As String
        Try
            Dim nodeKey As XElement = xmlDoc.Elements(inPartition).Elements(inSection).Elements(inKey).FirstOrDefault()

            If nodeKey Is Nothing Then
                ' проверка существования раздела
                Dim nodePartition As XElement = xmlDoc.Elements(inPartition).FirstOrDefault()

                If nodePartition Is Nothing Then Return CreatePartition(xmlDoc, inPartition, inSection, inKey, sDefault)

                ' проверка существования секции
                Dim nodeSection As XElement = xmlDoc.Elements(inPartition).Elements(inSection).FirstOrDefault()
                If (nodeSection Is Nothing) Then Return CreateSection(xmlDoc, nodePartition, inSection, inKey, sDefault)

                ' проверка существования ключа
                nodeKey = xmlDoc.Elements(inPartition).Elements(inSection).Elements(inKey).FirstOrDefault()
                If (nodeKey Is Nothing) Then Return CreateKey(xmlDoc, nodeSection, inKey, sDefault)
            Else
                ' как правило ключи уже созданы
                Return nodeKey.Value
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, Convert.ToString("Считывание значения ключа: ") & inKey, MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Создать Раздел.
    ''' </summary>
    ''' <param name="xmlDoc"></param>
    ''' <param name="inPartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    Private Function CreatePartition(ByRef xmlDoc As XElement, inPartition As String, inSection As String, inKey As String, sDefault As String) As String
        Dim newNodePartition As New XElement(inPartition)
        xmlDoc.Add(newNodePartition)
        Return CreateSection(xmlDoc, newNodePartition, inSection, inKey, sDefault)
    End Function

    ''' <summary>
    ''' Создать Секцию.
    ''' </summary>
    ''' <param name="xmlDoc"></param>
    ''' <param name="refNodePartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    Private Function CreateSection(ByRef xmlDoc As XElement, ByRef refNodePartition As XElement, inSection As String, inKey As String, sDefault As String) As String
        Dim newNodeSection As New XElement(inSection)
        refNodePartition.Add(newNodeSection)
        Return CreateKey(xmlDoc, newNodeSection, inKey, sDefault)
    End Function

    ''' <summary>
    ''' Создать Ключ.
    ''' </summary>
    ''' <param name="xmlDoc"></param>
    ''' <param name="refNodeSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    Private Function CreateKey(ByRef xmlDoc As XElement, ByRef refNodeSection As XElement, inKey As String, sDefault As String) As String
        Dim newNodeKey As New XElement(inKey, sDefault)
        refNodeSection.Add(newNodeKey)
        ' сохранение документа только при создании нового ключа
        xmlDoc.Save(PathXmlFile)
        Return sDefault
    End Function

    ''' <summary>
    ''' Сохранить значение ключа.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="inPartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sValue"></param>
    ''' <remarks></remarks>
    Public Sub WriteINI(inPartition As String, inSection As String, inKey As String, sValue As Object)
        'создать документ
        Dim xmlDoc As XElement = XElement.Load(PathXmlFile)
        WriteINI(xmlDoc, inPartition, inSection, inKey, sValue)
    End Sub

    ''' <summary>
    ''' Сохранить значение ключа.
    ''' Перегруженная версия.
    ''' </summary>
    ''' <param name="xmlDoc"></param>
    ''' <param name="inPartition"></param>
    ''' <param name="inSection"></param>
    ''' <param name="inKey"></param>
    ''' <param name="sValue"></param>
    ''' <remarks></remarks>
    Public Sub WriteINI(ByRef xmlDoc As XElement, inPartition As String, inSection As String, inKey As String, sValue As Object)
        Dim convValue As String = Convert.ToString(sValue)

        Try
            Dim nodeKey As XElement = xmlDoc.Elements(inPartition).Elements(inSection).Elements(inKey).FirstOrDefault()

            If nodeKey Is Nothing Then
                ' ключа нет значит создать новый в методе GetIni
                Dim sFindValue As String = GetIni(xmlDoc, inPartition, inSection, inKey, convValue)
            Else
                ' как правило ключи уже созданы
                nodeKey.Value = convValue
                xmlDoc.Save(PathXmlFile)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, $"Редактирование значения ключа: {inKey }={sValue }", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
End Class