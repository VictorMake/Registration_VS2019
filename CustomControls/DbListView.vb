Public Class DbListView
    Inherits ListView

    'Public Sub New()
    '' Включить внутреннюю ListView двойную буферизацию
    '    SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
    '' Выключить по умолчанию CommCtrl перерисовку для не XP систем
    '    If Not NativeInterop.IsWinXP Then
    '        SetStyle(ControlStyles.UserPaint, True)
    '    End If
    'End Sub

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        'Этот вызов является обязательным для конструктора компонентов.
        InitializeComponent()
        ' Включить внутреннюю ListView двойную буферизацию
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        ' Выключить по умолчанию CommCtrl перерисовку для не XP систем
        If Not NativeInterop.IsWinXP Then
            SetStyle(ControlStyles.UserPaint, True)
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If GetStyle(ControlStyles.UserPaint) Then
            Dim m As New Message() With {.HWnd = Handle, .Msg = NativeInterop.WM_PRINTCLIENT, .WParam = e.Graphics.GetHdc(), .LParam = CType(NativeInterop.PRF_CLIENT, IntPtr)}
            DefWndProc(m)
            e.Graphics.ReleaseHdc(m.WParam)
        End If
        MyBase.OnPaint(e)
    End Sub
End Class
