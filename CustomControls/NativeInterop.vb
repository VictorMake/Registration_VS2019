
''' <summary>
''' Используется в контролах с буфереизованной перерисовкой
''' </summary>
''' <remarks></remarks>
Public Class NativeInterop
    Public Const WM_PRINTCLIENT As Integer = &H318
    Public Const PRF_CLIENT As Integer = &H4

    '<DllImport("user32.dll")> _
    'Public Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    'End Function

    Public Shared ReadOnly Property IsWinXP() As Boolean
        Get
            Dim OS As OperatingSystem = Environment.OSVersion
            Return (OS.Platform = PlatformID.Win32NT) AndAlso ((OS.Version.Major > 5) OrElse ((OS.Version.Major = 5) AndAlso (OS.Version.Minor = 1)))
        End Get
    End Property

    Public Shared ReadOnly Property IsWinVista() As Boolean
        Get
            Dim OS As OperatingSystem = Environment.OSVersion
            Return (OS.Platform = PlatformID.Win32NT) AndAlso (OS.Version.Major >= 6)
        End Get
    End Property
End Class
