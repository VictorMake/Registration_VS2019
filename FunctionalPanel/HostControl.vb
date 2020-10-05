Imports System.ComponentModel.Design

''' <summary>
''' Хост HostSurface который наследует из DesignSurface
''' </summary>
Friend Class HostControl 'Public Class HostControl
    Inherits UserControl

    Private _hostSurface As HostSurface
    ''' <summary>
    ''' необходимые переменные дизайнера
    ''' </summary>
    Public Sub New(ByVal hostSurface As HostSurface)
        ' This call is required by the Windows.Forms Form Designer.
        InitializeComponent()
        InitializeHost(hostSurface)
    End Sub

    Friend Sub InitializeHost(ByVal hostSurface As HostSurface)
        Try
            If hostSurface Is Nothing Then
                Return
            End If

            _hostSurface = hostSurface

            Dim control As Control = TryCast(_hostSurface.View, Control)

            control.Parent = Me
            control.Dock = DockStyle.Fill
            control.Visible = True
        Catch ex As Exception
            Trace.WriteLine(ex.ToString())
             Const caption As String = "InitializeHost"
            Dim text As String = ex.ToString
            RegistrationEventLog.EventLog_MSG_EXCEPTION($"<{caption}> {text}")
        End Try
    End Sub

    Public ReadOnly Property HostSurface() As HostSurface
        Get
            Return _hostSurface
        End Get
    End Property

    Public ReadOnly Property DesignerHost() As IDesignerHost
        Get
            Return DirectCast(_hostSurface.GetService(GetType(IDesignerHost)), IDesignerHost)
        End Get
    End Property
End Class

