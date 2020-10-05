Imports System.Math
Imports MathematicalLibrary

''' <summary>
''' 17 '"Отладочный режим"
''' </summary>
Friend Class AnalysisОтладочныйРежим
    Inherits Analysis

    Public Sub New(name As String, inFormMain As FormSnapshotViewingDiagram)
        MyBase.New(name, inFormMain)
    End Sub

    Private Sub AllocateProtocol()
        EngineDefineTU()
        'ReDim_Protocol(2, 3)
        Re.Dim(Protocol, 2, 3)
        Protocol(1, 1) = "Контрольный лист №"
        Protocol(2, 1) = "Кадр предъявляется"

        Protocol(1, 2) = CStr(NumberEngine)
        Protocol(2, 2) = "п/заказчика"

        Protocol(1, 3) = ""
        Protocol(2, 3) = ""
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()

        MessageBox.Show("Для режима: <Отладочный режим> автоматическая расшифровка не предусмотрена." & Environment.NewLine &
                        "Произведите расшифровку вручную.", "Расшифровка режима", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
End Class

