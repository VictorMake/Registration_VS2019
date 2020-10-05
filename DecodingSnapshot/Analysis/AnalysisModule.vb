Imports System.ComponentModel

Module AnalysisModule
#Region "Const имён расшифровок"
    Public Const cРегистратор As String = "Регистратор" ' 1
    Public Const cПриемистостьМГ_РУД67 As String = "Приемистость МГ->РУД67" ' 2
    Public Const cПриемистостьсФорсажомМГ_РУД115 As String = "Приемистость с форсажом МГ -> РУД115 (106)" ' 3
    Public Const cСбросРУД67_МГ As String = "Сброс РУД67->МГ" ' 4
    Public Const cСбросРУД115_МГ As String = "Сброс РУД115 (106) -> МГ" '5
    Public Const cЛожныйЗапуск As String = "Ложный запуск" ' 6
    Public Const cВключениеФорсажаРУД67_РУД115 As String = "Включение форсажа РУД67 -> РУД115 (106)" ' 7
    Public Const cВключениеСУНАНаN290ПоКнопкеБК As String = "Включение СУНА на N2=90 по кнопке БК" ' 8
    Public Const cБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК As String = "Быстродействие НА КВД при включении СУНА покнопке БК" ' 9
    Public Const cБыстродействиеРСприВключенииСУНАпоКнопкеБК As String = "Быстродействие РС при включении СУНА по кнопке БК" ' 10
    Public Const cВключениеКОнаN290 As String = "Включение КО на N2=90" ' 11
    Public Const cВключениеКОнаРУД115 As String = "Включение КО на аРУД=115 (106)" ' 12
    Public Const cПриемистостьМГ_N290 As String = "Приемистость МГ->N2=90" ' 13
    Public Const cВождениеА1А2N1N2 As String = "Вождение a1,a2,N1,N2" ' 14
    Public Const cГорячийЗапуск As String = "Горячий запуск" ' 15
    Public Const cПриемистостьМГ_N295сКРД99Б As String = "Приемистость МГ -> N2=95 с КРД99Б по ДОП№3" ' 16
    Public Const cОтладочныйРежим As String = "Отладочный режим" ' 17
    Public Const cСбросN295_МГ As String = "Сброс N2=95 -> МГ" ' 22
    Public Const cВключениеФорсажаРУД67РУД115изд39 As String = "Включение Форсажа РУД67 -> РУД115 (106) изделия 39" ' 23
    Public Const cВыключениеФорсажаРУД115РУД67изд39 As String = "Выключение Форсажа РУД115 (106) -> РУД67 изделия 39" ' 24
    Public Const cПлавноеРезкоеДросселирование As String = "Плавное и резкое дросселирование форсажа изделия 39" ' 25
    Public Const cПовторноеВключениеФорсажаРУД67РУД115изд39 As String = "Повторное включение Форсажа РУД67 -> РУД115 (106) изделия 39" ' 26
    Public Const cПовторноеВыключениеФорсажаРУД115РУД67изд39 As String = "Повторное выключение Форсажа РУД115 (106) -> РУД67 изделия 39" ' 27
    Public Const cПриемистостьАИ222 As String = "Приемистость АИ222" ' 28
    Public Const cСбросАИ222 As String = "Сброс АИ222" ' 29

    ''' <summary>
    ''' Перечислитель режимов расшифпровок.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EnumAnalysis    '<Flags()>
        'None = 0'нельзя с таким окном
        <Description(cРегистратор)>
        Регистратор = 1
        <Description(cПриемистостьМГ_РУД67)>
        ПриемистостьМГ_РУД67 = 2
        <Description(cПриемистостьсФорсажомМГ_РУД115)>
        ПриемистостьсФорсажомМГ_РУД115 = 3
        <Description(cСбросРУД67_МГ)>
        СбросРУД67_МГ = 4
        <Description(cСбросРУД115_МГ)>
        СбросРУД115_МГ = 5
        <Description(cЛожныйЗапуск)>
        ЛожныйЗапуск = 6
        <Description(cВключениеФорсажаРУД67_РУД115)>
        ВключениеФорсажаРУД67_РУД115 = 7
        <Description(cВключениеСУНАНаN290ПоКнопкеБК)>
        ВключениеСУНАНаN290ПоКнопкеБК = 8
        <Description(cБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК)>
        БыстродействиеНАКВДприВключенииСУНАпоКнопкеБК = 9
        <Description(cБыстродействиеРСприВключенииСУНАпоКнопкеБК)>
        БыстродействиеРСприВключенииСУНАпоКнопкеБК = 10
        <Description(cВключениеКОнаN290)>
        ВключениеКОнаN290 = 11
        <Description(cВключениеКОнаРУД115)>
        ВключениеКОнаРУД115 = 12
        <Description(cПриемистостьМГ_N290)>
        ПриемистостьМГ_N290 = 13
        <Description(cВождениеА1А2N1N2)>
        ВождениеА1А2N1N2 = 14
        <Description(cГорячийЗапуск)>
        ГорячийЗапуск = 15
        <Description(cПриемистостьМГ_N295сКРД99Б)>
        ПриемистостьМГ_N295сКРД99Б = 16
        <Description(cОтладочныйРежим)>
        ОтладочныйРежим = 17
        <Description(cСбросN295_МГ)>
        СбросN295_МГ = 22
        <Description(cВключениеФорсажаРУД67РУД115изд39)>
        ВключениеФорсажаРУД67РУД115изд39 = 23
        <Description(cВыключениеФорсажаРУД115РУД67изд39)>
        ВыключениеФорсажаРУД115РУД67изд39 = 24
        <Description(cПлавноеРезкоеДросселирование)>
        ПлавноеРезкоеДросселирование = 25
        <Description(cПовторноеВключениеФорсажаРУД67РУД115изд39)>
        ПовторноеВключениеФорсажаРУД67РУД115изд39 = 26
        <Description(cПовторноеВыключениеФорсажаРУД115РУД67изд39)>
        ПовторноеВыключениеФорсажаРУД115РУД67изд39 = 27
        <Description(cПриемистостьАИ222)>
        ПриемистостьАИ222 = 28
        <Description(cСбросАИ222)>
        СбросАИ222 = 29
    End Enum
#End Region

#Region "Const имён параметров"
    'Public Const conNкомпрИГ As String = "nИГ-03"
    Public Const contбокса As String = "tбокса"
    Public Const conТхс As String = "Т хс"
    Public Const conN1 As String = "N1"
    Public Const conN2 As String = "N2"
    Public Const conаРУД As String = "аРУД"
    Public Const conЗапуск As String = "Запуск"

    Public Const cona1 As String = "a1"
    Public Const cona2 As String = "a2"
    Public Const conТ4 As String = "Т4"
    Public Const conРтзаНП96М As String = "Рт за НП-96М"
    Public Const conДиаметрРС As String = "ДиаметрРС"
    Public Const conТокJправый As String = "ТокJправый"
    Public Const conТокJлевый As String = "ТокJлевый"
    Public Const conПолныйФорсаж As String = "ПолныйФорсаж"
    Public Const conРтОК1К As String = "Рт ОК1К"
    Public Const conКнопкаБК As String = "КнопкаБК"
    Public Const conКлапанСУНА As String = "КлапанСУНА"
    Public Const conПомпаж As String = "Помпаж"
    Public Const conКлапанКО As String = "КлапанКО"
    Public Const conПирометр As String = "U447"

    ' параметры для АИ222
    Public Const conаРУДАИ222 As String = "aруд"
    Public Const conNвдпрАИ222 As String = "nвдПР"
    Public Const conNндпрАИ222 As String = "nндПР"
    Public Const conТндАИ222 As String = "t*тнд"
    Public Const conПомпажАИ222 As String = "Помпаж"
    Public Const conПомпажАИ222стенд As String = "ПомпажСт от ДОЛ"
    Public Const conGтопливаАИ222 As String = "Gтоплива"
    Public Const conЗемляАИ222 As String = "М:Земля"

#End Region

    Public Function ВводЗначенияРуда() As Double
        Dim success As Boolean
        Dim strTemp As String
        Dim Руд As Double

        Do
            strTemp = InputBox("Введите значение Руд")
            Try
                Руд = Double.Parse(strTemp, Globalization.CultureInfo.CurrentCulture)

                If Руд > 0 AndAlso Руд < 120 Then
                    success = True
                Else
                    Throw New FormatException("Значение Руд вне предела 0 - 120", New System.Exception)
                End If
            Catch ex As FormatException
                MessageBox.Show("Значение Руд= " & strTemp & " недопустимо", "Ввод данных", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        Loop Until success

        Return Руд
    End Function

    Public Structure КоординатыОборотыУглы
        Dim ОборотыПриведенные As Double
        Dim Угол As Double
    End Structure

    Public Function ПромежуточныеОборотыПриведАльфа(ByVal ОборотыПривед As Double, ByRef arrТочкиX() As КоординатыОборотыУглы) As Double
        If ОборотыПривед = 0 OrElse ОборотыПривед > arrТочкиX(arrТочкиX.GetUpperBound(0)).ОборотыПриведенные Then Exit Function
        Dim indexTime As Integer
        Dim ОборотыСравнения As New КоординатыОборотыУглы With {
            .ОборотыПриведенные = ОборотыПривед
        }

        indexTime = Array.BinarySearch(arrТочкиX, ОборотыСравнения, New ComparerОборотыСравнения)
        If indexTime < 0 Then  'индекс первого элемента больше искомого
            indexTime = -indexTime - 1
        End If

        Return LinearInterpolation(ОборотыПривед, arrТочкиX(indexTime - 1).ОборотыПриведенные, arrТочкиX(indexTime - 1).Угол, arrТочкиX(indexTime).ОборотыПриведенные, arrТочкиX(indexTime).Угол)
    End Function

    Private Class ComparerОборотыСравнения : Implements IComparer
        Dim mIndexKRD As Integer

        'Public Sub New(ByVal IndexKRD As Integer)
        '    mIndexKRD = IndexKRD
        'End Sub

        Public Function Compare(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements IComparer.Compare
            Dim Обороты1, Обороты2 As КоординатыОборотыУглы

            Try
                Обороты1 = CType(o1, КоординатыОборотыУглы)
                Обороты2 = CType(o2, КоординатыОборотыУглы)
            Catch ex As Exception
                Throw (ex)
                Exit Function
            End Try

            If Обороты1.ОборотыПриведенные < Обороты2.ОборотыПриведенные Then
                Return -1
            Else
                If Обороты1.ОборотыПриведенные > Обороты2.ОборотыПриведенные Then
                    Return 1
                Else
                    Return 0
                End If
            End If
        End Function
    End Class

#Region "CreateAnalysis"
    ''' <summary>
    ''' Регистратор - 1
    ''' </summary>
    Friend Class CreatorAnalysisРегистратор
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisРегистратор(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПриемистостьМГ_РУД67 - 2
    ''' </summary>
    Friend Class CreatorAnalysisПриемистостьМГ_РУД67
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПриемистостьМГ_РУД67(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ФорсажомМГ_РУД115 - 3
    ''' </summary>
    Friend Class CreatorAnalysisФорсажомМГ_РУД115
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПриемистостьсФорсажомМГ_РУД115(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' СбросРУД67_МГ - 4
    ''' </summary>
    Friend Class CreatorAnalysisСбросРУД67_МГ
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisСбросРУД67_МГ(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' СбросРУД115_МГ - 5
    ''' </summary>
    Friend Class CreatorAnalysisСбросРУД115_МГ
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisСбросРУД115_МГ(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ЛожныйЗапуск - 6
    ''' </summary>
    Friend Class CreatorAnalysisЛожныйЗапуск
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisЛожныйЗапуск(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВключениеФорсажаРУД67_РУД115 - 7
    ''' </summary>
    Friend Class CreatorAnalysisВключениеФорсажаРУД67_РУД115
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВключениеФорсажаРУД67_РУД115(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВключениеСУНАНаN290ПоКнопкеБК - 8
    ''' </summary>
    Friend Class CreatorAnalysisВключениеСУНАНаN290ПоКнопкеБК
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВключениеСУНАНаN290ПоКнопкеБК(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' БыстродействиеНАКВДприВключенииСУНАпоКнопкеБК - 9
    ''' </summary>
    Friend Class CreatorAnalysisБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisБыстродействиеНАКВДприВключенииСУНАпоКнопкеБК(name, Mediator)
        End Function
    End Class

    ''' <summary>
    '''  AnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК - 10
    ''' </summary>
    Friend Class CreatorAnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisБыстродействиеРСприВключенииСУНАпоКнопкеБК(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВключениеКОнаN290 - 11
    ''' </summary>
    Friend Class CreatorAnalysisВключениеКОнаN290
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВключениеКОнаN290(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВключениеКОнаРУД115 - 12
    ''' </summary>
    Friend Class CreatorAnalysisВключениеКОнаРУД115
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВключениеКОнаРУД115(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПриемистостьМГ_N290 - 13
    ''' </summary>
    Friend Class CreatorAnalysisПриемистостьМГ_N290
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПриемистостьМГ_N290(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВождениеА1А2N1N2 - 14
    ''' </summary>
    Friend Class CreatorAnalysisВождениеА1А2N1N2
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВождениеА1А2N1N2(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ГорячийЗапуск - 15
    ''' </summary>
    Friend Class CreatorAnalysisГорячийЗапуск
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisГорячийЗапуск(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПриемистостьМГ_N295сКРД99Б - 16
    ''' </summary>
    Friend Class CreatorAnalysisПриемистостьМГ_N295сКРД99Б
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПриемистостьМГ_N295сКРД99Б(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ОтладочныйРежим - 17
    ''' </summary>
    Friend Class CreatorAnalysisОтладочныйРежим
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisОтладочныйРежим(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' СбросN295_МГ - 22
    ''' </summary>
    Friend Class CreatorAnalysisСбросN295_МГ
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisСбросN295_МГ(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВключениеФорсажаРУД67РУД115изд39 - 23
    ''' </summary>
    Friend Class CreatorAnalysisВключениеФорсажаРУД67РУД115изд39
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВключениеФорсажаРУД67РУД115изд39(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ВыключениеФорсажаРУД115РУД67изд39 - 24
    ''' </summary>
    Friend Class CreatorAnalysisВыключениеФорсажаРУД115РУД67изд39
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisВыключениеФорсажаРУД115РУД67изд39(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПлавноеРезкоеДросселирование - 25
    ''' </summary>
    Friend Class CreatorAnalysisПлавноеРезкоеДросселирование
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПлавноеРезкоеДросселирование(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПовторноеВключениеФорсажаРУД67РУД115изд39 - 26
    ''' </summary>
    Friend Class CreatorAnalysisПовторноеВключениеФорсажаРУД67РУД115изд39
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПовторноеВключениеФорсажаРУД67РУД115изд39(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПовторноеВыключениеФорсажаРУД115РУД67изд39 - 27
    ''' </summary>
    Friend Class CreatorAnalysisПовторноеВыключениеФорсажаРУД115РУД67изд39
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПовторноеВыключениеФорсажаРУД115РУД67изд39(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' ПриемистостьАИ222 - 28
    ''' </summary>
    Friend Class CreatorAnalysisПриемистостьАИ222
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisПриемистостьАИ222(name, Mediator)
        End Function
    End Class

    ''' <summary>
    ''' СбросАИ222 - 29
    ''' </summary>
    Friend Class CreatorAnalysisСбросАИ222
        Inherits CreatorAnalysis

        Protected Overrides Function CreateAnalysis(name As String, Mediator As FormSnapshotViewingDiagram) As Analysis
            Return New AnalysisСбросАИ222(name, Mediator)
        End Function
    End Class

#End Region

End Module
