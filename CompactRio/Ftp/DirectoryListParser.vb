Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Public Structure FileStruct
    Public Flags As String
    Public Owner As String
    Public IsDirectory As Boolean
    Public CreateTime As String
    Public Name As String
End Structure

Public Enum FileListStyle
    UnixStyle
    WindowsStyle
    Unknown
End Enum

''' <summary>
''' Парсер строки с содержанием структуры каталога полученным от RT по FTP протоколу.
''' </summary>
''' <remarks></remarks>
Public Class DirectoryListParser

    Private ReadOnly structFiles As List(Of FileStruct)

    ''' <summary>
    ''' Перевод коллекции всей файловой структуры в массив
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FullListing() As FileStruct()
        Get
            Return structFiles.ToArray()
        End Get
    End Property

    ''' <summary>
    ''' Перевод только файлов из коллекции файловой структуры в массив
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Files() As FileStruct()
        Get
            Dim filesStr As New List(Of FileStruct)()

            For Each itemFile As FileStruct In structFiles
                If Not itemFile.IsDirectory Then filesStr.Add(itemFile)
            Next

            Return filesStr.ToArray()
        End Get
    End Property

    ''' <summary>
    ''' Перевод только папок из коллекции файловой структуры в массив
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Directories() As FileStruct()
        Get
            Dim directoriesStr As New List(Of FileStruct)()

            For Each itemFile As FileStruct In structFiles
                If itemFile.IsDirectory Then directoriesStr.Add(itemFile)
            Next

            Return directoriesStr.ToArray()
        End Get
    End Property

    Public Sub New(responseString As String)
        structFiles = GetList(responseString)
    End Sub

    ''' <summary>
    ''' Разбор потока ответа от запроса FTP клиента о составе файловой системы по указанному сетевому адресу
    ''' </summary>
    ''' <param name="dataString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetList(dataString As String) As List(Of FileStruct)
        Dim directories As New List(Of FileStruct)()
        Dim dataRecords As String() = dataString.Split(ControlChars.Lf) ' разделить с переводом строк
        Dim directoryListStyle As FileListStyle = GuessFileListStyle(dataRecords)

        For Each s As String In dataRecords
            If directoryListStyle <> FileListStyle.Unknown AndAlso s <> "" Then
                Dim f As New FileStruct() With {.Name = ".."} ' временный

                Select Case directoryListStyle
                    Case FileListStyle.UnixStyle
                        f = ParseFileStructFromUnixStyleRecord(s)
                        Exit Select
                    Case FileListStyle.WindowsStyle
                        f = ParseFileStructFromWindowsStyleRecord(s)
                        Exit Select
                End Select

                If f.Name <> "" AndAlso f.Name <> "." AndAlso f.Name <> ".." Then
                    directories.Add(f)
                End If
            End If
        Next

        Return directories
    End Function

    ''' <summary>
    ''' По вхождению символов в ответе определить файловую систему запрашиваемого ресурса
    ''' </summary>
    ''' <param name="recordList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GuessFileListStyle(recordList As String()) As FileListStyle
        ' догадаться только при достаточно длинной строке
        For Each s As String In recordList
            If s.Length > 10 AndAlso Regex.IsMatch(s.Substring(0, 10), "(-|d)((-|r)(-|w)(-|x)){3}") Then
                Return FileListStyle.UnixStyle
            ElseIf s.Length > 8 AndAlso Regex.IsMatch(s.Substring(0, 8), "[0-9]{2}-[0-9]{2}-[0-9]{2}") Then
                Return FileListStyle.WindowsStyle
            End If
        Next

        Return FileListStyle.Unknown
    End Function

    ''' <summary>
    ''' Подразумевается запись в стиле как
    ''' 02-03-04  07:46PM       {DIR}          Append
    ''' </summary>
    ''' <param name="record"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ParseFileStructFromWindowsStyleRecord(record As String) As FileStruct
        Dim f As New FileStruct()
        Dim processstr As String = record.Trim()
        Dim dateStr As String = processstr.Substring(0, 8)
        processstr = (processstr.Substring(8, processstr.Length - 8)).Trim()
        Dim timeStr As String = processstr.Substring(0, 7)

        processstr = (processstr.Substring(7, processstr.Length - 7)).Trim()
        f.CreateTime = $"{dateStr} {timeStr}" ' получили в формате 02-03-04  07:46

        If processstr.Substring(0, 5) = "<DIR>" Then
            f.IsDirectory = True
            processstr = (processstr.Substring(5, processstr.Length - 5)).Trim()
        Else
            ' разбить на подстроки и взять имя по индексу 1
            Dim strs As String() = processstr.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            processstr = strs(1)
            f.IsDirectory = False
        End If

        f.Name = processstr ' "<DIR>" или имя
        ' остаться на части имени    
        Return f
    End Function

    ''' <summary>
    ''' Подразумевается запись в стиле как
    ''' dr-xr-xr-x   1 owner    group               0 Nov 25  2022 bussys
    ''' </summary>
    ''' <param name="record"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ParseFileStructFromUnixStyleRecord(record As String) As FileStruct
        Dim f As New FileStruct()

        If record(0) = "-"c OrElse record(0) = "d"c Then
            ' правильная запись файла
            Dim processstr As String = record.Trim()
            f.Flags = processstr.Substring(0, 9)
            f.IsDirectory = (f.Flags(0) = "d"c)
            processstr = (processstr.Substring(11)).Trim()
            CutSubstringFromStringWithTrim(processstr, " "c, 0)
            ' пропустить одну часть
            f.Owner = CutSubstringFromStringWithTrim(processstr, " "c, 0)
            f.CreateTime = GetCreateTimeString(record)
            Dim fileNameIndex As Integer = record.IndexOf(f.CreateTime) + f.CreateTime.Length
            ' остаться на части имени                
            f.Name = record.Substring(fileNameIndex).Trim()
        Else
            f.Name = ""
        End If

        Return f
    End Function

    ''' <summary>
    ''' Делает простую проверку строки даты по шаблону с не точной проверкой даты и времени файла
    ''' </summary>
    ''' <param name="record"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCreateTimeString(record As String) As String
        Const month As String = "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)"
        Const space As String = "(\040)+"
        Const day As String = "([0-9]|[1-3][0-9])"
        Const year As String = "[1-2][0-9]{3}"
        Const time As String = "[0-9]{1,2}:[0-9]{2}"

        Dim dateTimeRegex As New Regex($"{month}{space}{day}{space}({year}|{time})", RegexOptions.IgnoreCase)
        Dim match As Match = dateTimeRegex.Match(record)

        Return match.Value
    End Function

    ''' <summary>
    ''' Выдать вырезанную часть с начала до первого вхождения символа(c As Char).
    ''' Исходная строка модифицируется.
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="c"></param>
    ''' <param name="startIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CutSubstringFromStringWithTrim(ByRef s As String, c As Char, startIndex As Integer) As String
        Dim pos1 As Integer = s.IndexOf(c, startIndex)
        Dim retString As String = s.Substring(0, pos1)

        s = (s.Substring(pos1)).Trim()

        Return retString
    End Function
End Class