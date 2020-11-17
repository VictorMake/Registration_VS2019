''' <summary>
''' Вывести накопленную ошибку во всех классах
''' </summary>
Friend Class ShowTotalErrorsMessage
    Public Shared Sub ShowMessage(IsTotalErrors As Boolean, totalErrorsMessage As String)
        ' если накопленная ошибка во всех классах
        If IsTotalErrors Then
            MessageBox.Show(totalErrorsMessage, "Ошибка автоматической расшифровки", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Проверка на корректность введенныы параметров
    ''' </summary>
    ''' <param name="nameParameter"></param>
    ''' <param name="Astart"></param>
    ''' <param name="Astop"></param>
    ''' <param name="errorsMessage"></param>
    ''' <param name="parameters"></param>
    ''' <param name="indexParameter"></param>
    ''' <returns></returns>
    Public Shared Function IsParameterNotCorrect(nameParameter As String,
                                                 Astart As Double,
                                                 Astop As Double,
                                                 ByRef errorsMessage As String,
                                                 parameters() As TypeSmallParameter,
                                                 ByRef indexParameter As Integer) As Boolean
        Dim success As Boolean
        ' проверка на корректность введенныы параметров
        If Astart = Astop Then
            errorsMessage += "Не введены Аначальное или Аконечное" & vbCrLf
            Return True
        End If

        ' индекс параметра отличается на 1 от MeasuredValues(,), поэтому дважды производится коррекция, здесь и в CastToAxesStandard
        For J As Integer = 1 To UBound(parameters)
            If parameters(J).NameParameter = nameParameter AndAlso parameters(J).IsVisible Then
                indexParameter = J - 1
                success = True
                Exit For
            End If
        Next

        If Not success Then
            errorsMessage += "Параметр " & nameParameter & " не найден" & vbCrLf
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Проверка на корректность введенныы параметров
    ''' </summary>
    ''' <param name="nameParameter"></param>
    ''' <param name="errorsMessage"></param>
    ''' <param name="parameters"></param>
    ''' <param name="indexParameter"></param>
    ''' <returns></returns>
    Public Shared Function IsParameterNotCorrect(nameParameter As String,
                                             ByRef errorsMessage As String,
                                             parameters() As TypeSmallParameter,
                                             ByRef indexParameter As Integer) As Boolean
        Dim success As Boolean

        ' индекс параметра отличается на 1 от MeasuredValues(,), поэтому дважды производится коррекция, здесь и в CastToAxesStandard
        For J As Integer = 1 To UBound(parameters)
            If parameters(J).NameParameter = nameParameter AndAlso parameters(J).IsVisible Then
                indexParameter = J - 1
                success = True
                Exit For
            End If
        Next

        If Not success Then
            errorsMessage += "Параметр " & nameParameter & " не найден" & vbCrLf
            Return True
        End If

        Return False
    End Function

    Private Const TstartNotFound As String = "Тначальное не найдено" & vbCrLf
    Private Const TstopNotFound As String = "Тконечное не найдено" & vbCrLf
    Private Const TStartIsEqualStop As String = "Тначальное и Тконечное равны" & vbCrLf

    Public Shared Function IsTimeFound(isTstartFound As Boolean,
                                  isTstopFound As Boolean,
                                  indexTstart As Integer,
                                  indexTstop As Integer,
                                  graphMinimum As Integer,
                                  ByRef errorsMessage As String) As Boolean

        Dim IsErrors As Boolean

        If Not isTstartFound OrElse indexTstart = graphMinimum Then
            IsErrors = True
            errorsMessage += TstartNotFound
        End If

        If Not isTstopFound OrElse indexTstop = graphMinimum Then
            IsErrors = True
            errorsMessage += TstopNotFound
        End If

        If (indexTstart = indexTstop) AndAlso Not (indexTstart = graphMinimum AndAlso indexTstop = graphMinimum) Then
            IsErrors = True
            errorsMessage += TStartIsEqualStop
        End If

        Return IsErrors
    End Function

    Public Shared Function IsTimeFound(isTstopFound As Boolean,
                              indexTstart As Integer,
                              indexTstop As Integer,
                              graphMinimum As Integer,
                              ByRef errorsMessage As String) As Boolean

        Dim IsErrors As Boolean

        If indexTstart <= graphMinimum Then
            IsErrors = True
            errorsMessage += TstartNotFound
        End If

        If Not isTstopFound OrElse indexTstop = graphMinimum Then
            IsErrors = True
            errorsMessage += TstopNotFound
        End If

        If (indexTstart = indexTstop) AndAlso Not (indexTstart = graphMinimum AndAlso indexTstop = graphMinimum) Then
            IsErrors = True
            errorsMessage += TStartIsEqualStop
        End If

        Return IsErrors
    End Function

    Public Shared Function IsTimeFound(indexTstart As Integer,
                          indexTstop As Integer,
                          graphMinimum As Integer,
                          ByRef errorsMessage As String) As Boolean
        Dim IsErrors As Boolean

        If (indexTstart = indexTstop) AndAlso Not (indexTstart = graphMinimum AndAlso indexTstop = graphMinimum) Then
            IsErrors = True
            errorsMessage += TStartIsEqualStop
        End If

        Return IsErrors
    End Function
End Class