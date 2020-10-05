Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

''' <summary>
''' Класс отвечает за название компонента, когда они создаются.
''' Класс добавлен как сервис посредством HostSurfaceManager.
''' Предоставляет службу для генерации уникальных имен объектов.
''' </summary>
Public Class NameCreationService
    Implements INameCreationService

    'Public Sub New()
    'End Sub

    ''' <summary>
    ''' Создает новое имя, которое уникально для всех компонентов в указанном контейнере.
    ''' </summary>
    ''' <param name="container"></param>
    ''' <param name="type"></param>
    ''' <returns></returns>
    Private Function INameCreationService_CreateName(ByVal container As IContainer, ByVal type As Type) As String Implements INameCreationService.CreateName
        Dim cc As ComponentCollection = container.Components
        Dim min As Integer = Int32.MaxValue
        Dim max As Integer = Int32.MinValue
        Dim count As Integer = 0

        For i As Integer = 0 To cc.Count - 1
            Dim comp As Component = TryCast(cc(i), Component)

            If comp.[GetType]() Is type Then
                count += 1

                Dim name As String = comp.Site.Name
                If name.StartsWith(type.Name) Then
                    Try
                        Dim value As Integer = Int32.Parse(name.Substring(type.Name.Length))

                        If value < min Then
                            min = value
                        End If

                        If value > max Then
                            max = value
                        End If
                    Catch ex As Exception
                        'Trace.WriteLine(ex.ToString())
                        RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{NameOf(INameCreationService_CreateName)}> {ex}")
                    End Try
                End If
            End If
        Next

        If count = 0 Then
            Return type.Name & "1"
        ElseIf min > 1 Then
            Return type.Name & (min - 1).ToString()
        Else
            Return type.Name & (max + 1).ToString()
        End If
    End Function

    Private Function INameCreationService_IsValidName(ByVal name As String) As Boolean Implements INameCreationService.IsValidName
        Return True
    End Function

    Private Sub INameCreationService_ValidateName(ByVal name As String) Implements INameCreationService.ValidateName
        Return
    End Sub
End Class


'''' <summary>
'''' Генерация случайного цвета. Это используется MyRootDesigner
'''' </summary>
'Public Class RandomUtil
'    Friend Const MaxRGBInt As Integer = 255
'    Private Shared rand As Random = Nothing

'    Public Sub New()
'        If rand Is Nothing Then
'            InitializeRandoms(New Random().Next())
'        End If
'    End Sub

'    Private Sub InitializeRandoms(ByVal seed As Integer)
'        rand = New Random(seed)
'    End Sub

'    Public Overridable Function GetColor() As Color
'        Dim rval As Byte, gval As Byte, bval As Byte

'        rval = CByte(GetRange(0, MaxRGBInt))
'        gval = CByte(GetRange(0, MaxRGBInt))
'        bval = CByte(GetRange(0, MaxRGBInt))

'        Dim c As Color = Color.FromArgb(rval, gval, bval)

'        Return c
'    End Function

'    Public Function GetRange(ByVal nMin As Integer, ByVal nMax As Integer) As Integer
'        ' Поменять max и min если min > max
'        If nMin > nMax Then
'            Dim nTemp As Integer = nMin

'            nMin = nMax
'            nMax = nTemp
'        End If

'        If nMax <> Int32.MaxValue Then
'            nMax += 1
'        End If

'        Dim retVal As Integer = rand.[Next](nMin, nMax)

'        Return retVal
'    End Function
'End Class
