''' <summary>
''' Описывает свойства канала из двух ветвей конфигурационного файла.
''' </summary>
''' <remarks></remarks>
Public Class ChannelRio
    ''' <summary>
    ''' Увеличивающийся счётчик канала в шасси
    ''' </summary>
    ''' <returns></returns>
    Public Property IndexOnChassis As Integer
    Public Property Group As String

    '--- "ClassHwCh" каналы ИВК -----------------------------------------------
    Public Property NumberParameter As String
    Public Property HwBoardType As String
    Public Property ChId As String
    Public Property MinVal As Double
    Public Property MaxVal As Double
    Public Property UnitOfMeasure As String ' ЕдИзмер

    ''' <summary>
    ''' Terminal Mode для термопар
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TerminalMode As String

    '--- "ClassCh" каналы Сервера ---------------------------------------------
    Public Property Name As String ' Серверное имя канала
    Public Property MinLim As Double
    Public Property MaxLim As Double
    Public Property CoefficientsPolynomial As Double() = {0.0, 1.0, 0.0, 0.0, 0.0, 0.0}
    ''' <summary>
    ''' Степень Аппроксимации
    ''' </summary>
    ''' <returns></returns>
    Public Property LevelOfApproximation As Integer

    ''' <summary>
    ''' Запись заголовка аттрибутов для понимания столбцов CSV файла
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPropertyString() As String
        Return $"Number,{NameOf(Group)}," &
            $"Pin,{NameOf(HwBoardType)},{NameOf(ChId)},State,{NameOf(MinVal)},{NameOf(MaxVal)},Units,{NameOf(TerminalMode)}," &
            $"{NameOf(Name)},{NameOf(Name)},StateServer,{NameOf(MinLim)},{NameOf(MaxLim)},Scr_EdIzm,Ch_Unit," &
            "Polynome(0),Polynome(1),Polynome(2),Polynome(3),Polynome(4),Polynome(5)"
    End Function
End Class