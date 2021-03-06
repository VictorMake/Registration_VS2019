﻿Imports MathematicalLibrary

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
        Re.Dim(Protocol, 2, 3)
        PopulateProtocol(1, {"Контрольный лист №", CStr(NumberEngine), ""})
        PopulateProtocol(2, {"Кадр предъявляется", "п/заказчика", ""})
    End Sub

    Public Overrides Sub DecodingRegimeSnapshot()
        AllocateProtocol()
        MessageBox.Show($"Для режима: <{cОтладочныйРежим}> автоматическая расшифровка не предусмотрена." & Environment.NewLine &
                        "Произведите расшифровку вручную.", "Расшифровка режима", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
End Class