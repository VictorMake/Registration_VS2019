Imports System.Collections.Generic
Imports System.Data.OleDb
Imports MathematicalLibrary

Friend MustInherit Class Engine
    ''' <summary>
    ''' Тип изделия
    ''' </summary>
    ''' <returns></returns>
    Public Property Type As EngineType
    Private normsTU() As String
    Protected NameColumn As String

    ''' <summary>
    ''' Вернуть норму ТУ конкретного типа изделия по пункту в таблице
    ''' </summary>
    ''' <param name="numberArticle"></param>
    ''' <returns></returns>
    Public Function GetNormTUParameter(ByVal numberArticle As Integer) As String
        ' по пункту numberArticle массиве arrНормыТУ() ищется норма ТУ в котором при запуске
        ' основной формы загружены нормы
        If normsTU IsNot Nothing AndAlso numberArticle <= normsTU.Count Then
            Return normsTU(numberArticle)
        Else
            Return "Не найдена"
        End If
    End Function

    ''' <summary>
    ''' Определение норм ТУ по конкретному типу изделия и режиму испытания 
    ''' введенному при загрузке программы.
    ''' Общий метод базового класса.
    ''' </summary>
    Public Sub DefineTU()
        Dim strSQL As String
        Dim cmd As OleDbCommand
        Dim rowsCount As Integer
        Dim ColumnName As New List(Of String)

        ' конкретный класс определяет колонку для считывания из базы с учётом режима
        Using cn As New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
            cn.Open()
            cmd = cn.CreateCommand

            ' --- получить схему таблицы --------------------------------------
            Dim tblMetaData As DataTable
            Dim rdr As OleDbDataReader = Nothing

            'получить схему таблицы <ТехническиеУсловия> и составить список столбцов
            strSQL = "SELECT * FROM ТехническиеУсловия ORDER BY НомерПараметра"
            cmd.CommandText = strSQL
            Try
                rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo Or CommandBehavior.SchemaOnly)
            Catch ex As Exception
                Try
                    rdr = cmd.ExecuteReader(CommandBehavior.SchemaOnly)
                Catch ex2 As Exception
                    Const strStatus As String = "Невозможно собрать метаданные для результирующего запроса"
                    MessageBox.Show(ex2.Message, strStatus, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Try

            tblMetaData = rdr.GetSchemaTable
            For Each row As DataRow In tblMetaData.Rows
                ColumnName.Add(CStr(row("ColumnName")))
            Next
            rdr.Close()

            '--- сколько строк надо выделить длямассива -----------------------
            strSQL = "SELECT COUNT(*) FROM ТехническиеУсловия;"
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            rowsCount = CInt(cmd.ExecuteScalar)

            ' проверить наличие столбца в таблице <ТехническиеУсловия> 
            ' и заполнить массив
            If ColumnName.Contains(NameColumn) Then
                strSQL = "SELECT * FROM ТехническиеУсловия;"
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                rdr = cmd.ExecuteReader
                Re.Dim(normsTU, rowsCount)

                Do While (rdr.Read)
                    normsTU(CInt(rdr("НомерПараметра"))) = CStr(rdr(NameColumn))
                Loop

                rdr.Close()
            Else
                MessageBox.Show($"В базе данных для выбранного изделия: <{ModificationEngine}> в таблице <ТехническиеУсловия> отсутствует столбец с именем: <{NameColumn}> !" & Environment.NewLine &
                                "Смените тип изделия в конфигурационной форме <Опции> и попробуйте расшифровать снимок заново.",
                                "Отсутствуют нормы ТУ для выбранного изделия",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Using

        'Dim I As Integer
        ' загрузка норм ТУ
        ' при добавлении полей надо модифицировать запрос в базе
        'Извращения с OleDbCommand
        'Dim cn As OleDbConnection
        'cn = New OleDbConnection(BuildCnnStr(ProviderJet, PathChannels))
        'cn.Open()
        'cmd = cn.CreateCommand
        'strSQL = "SELECT COUNT(*) FROM ТехническиеУсловия;"
        'cmd.CommandType = CommandType.Text
        'cmd.CommandText = strSQL
        'rowsCount = CInt(cmd.ExecuteScalar)

        'strSQL = "SELECT * FROM ТехническиеУсловия;"
        'cmd.CommandType = CommandType.Text
        'cmd.CommandText = strSQL
        'Dim rdr As OleDbDataReader = cmd.ExecuteReader
        'ReDim_arrНормыТУ(rowsCount, 9)
        'Do While (rdr.Read)
        '    I = CInt(rdr("НомерПараметра"))
        '    arrНормыТУ(I, 1) = rdr("Изделие99А-Б")
        '    arrНормыТУ(I, 2) = rdr("Изделие99А-УБ")
        '    arrНормыТУ(I, 3) = rdr("Изделие99А-ОР")

        '    arrНормыТУ(I, 4) = rdr("Изделие99Б-Б")
        '    arrНормыТУ(I, 5) = rdr("Изделие99Б-УБ")

        '    arrНормыТУ(I, 6) = rdr("Изделие39-Б")
        '    arrНормыТУ(I, 7) = rdr("Изделие39-УБ")

        '    arrНормыТУ(I, 8) = rdr("ИзделиеМ1-Б")
        '    arrНормыТУ(I, 9) = rdr("ИзделиеМ1-УБ")
        'Loop
        'rdr.Close()
        'cn.Close()
    End Sub
End Class

''' <summary>
''' 99А серия 03 и 04
''' </summary>
Friend Class Engine99A
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "Изделие99А-Б"
        Const conRegimeUB As String = "Изделие99А-УБ"
        Const conRegimeO_R As String = "Изделие99А-ОР"

        Type = EngineType.Engine99A
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
            Case O_R
                NameColumn = conRegimeO_R
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' 99Б
''' </summary>
Friend Class Engine99B
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "Изделие99Б-Б"
        Const conRegimeUB As String = "Изделие99Б-УБ"

        Type = EngineType.Engine99B
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' 39
''' </summary>
Friend Class Engine39
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "Изделие39-Б"
        Const conRegimeUB As String = "Изделие39-УБ"

        Type = EngineType.Engine39
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' M1
''' </summary>
Friend Class EngineM1
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "ИзделиеМ1-Б"
        Const conRegimeUB As String = "ИзделиеМ1-УБ"

        Type = EngineType.EngineM1
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' M1 25.1
''' </summary>
Friend Class EngineM1_25_1
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "ИзделиеМ1_25_1-Б"
        Const conRegimeUB As String = "ИзделиеМ1_25_1-УБ"

        Type = EngineType.EngineM1_25_1
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' 39 сер.3
''' </summary>
Friend Class Engine39_3
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "Изделие39_сер_3-Б"
        Const conRegimeUB As String = "Изделие39_сер_3-УБ"

        Type = EngineType.Engine39_3
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class

''' <summary>
''' 222
''' </summary>
Friend Class Engine222
    Inherits Engine

    Public Sub New(inModeRegime As String)
        Const conRegimeB As String = "Изделие АИ-222-25-Б"
        Const conRegimeUB As String = "Изделие АИ-222-25-УБ"

        Type = EngineType.Engine222
        NameColumn = conRegimeB

        Select Case inModeRegime
            Case B
                NameColumn = conRegimeB
                Exit Select
            Case UB
                NameColumn = conRegimeUB
                Exit Select
        End Select
    End Sub
End Class