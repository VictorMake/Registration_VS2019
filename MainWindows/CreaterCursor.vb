Imports System.Runtime.InteropServices

''' <summary>
''' содержит API + локальные функции для создания
''' курсора из bitmap при перемещениии
''' </summary>
''' <remarks></remarks>
Public Class CreaterCursor

#Region "CreateIconIndirect"

    Private Structure IconInfo
        Public fIcon As Boolean
        Public xHotspot As Int32
        Public yHotspot As Int32
        Public hbmMask As IntPtr
        Public hbmColor As IntPtr
    End Structure

    '<DllImport("user32.dll", EntryPoint:="CreateIconIndirect")> _
    'Private Shared Function CreateIconIndirect(ByVal iconInfo As IntPtr) As IntPtr
    'End Function

    '<DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    'Private Shared Function DestroyIcon(ByVal handle As IntPtr) As Boolean
    'End Function

    '<DllImport("gdi32.dll")> _
    'Private Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
    'End Function

    ''' <summary>
    ''' Создать Cursor
    ''' </summary>
    ''' <param name="bmp"></param>
    ''' <returns>пользовательский Cursor</returns>
    ''' <remarks>создать пользовательский курсор из bitmap</remarks>
    Public Shared Function CreateCursor(ByVal bmp As Bitmap, ByVal xHotspot As Integer, ByVal yHotspot As Integer) As Windows.Forms.Cursor
        ' Настроить Cursors IconInfo
        Dim tmp As New IconInfo() With {.xHotspot = xHotspot,
                                        .yHotspot = yHotspot,
                                        .fIcon = False,
                                        .hbmMask = bmp.GetHbitmap(),
                                        .hbmColor = bmp.GetHbitmap()}

        ' Создать указатель из иконки курсора
        Dim pnt As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(tmp))
        Marshal.StructureToPtr(tmp, pnt, True)
        Dim curPtr As IntPtr = NativeMethods.CreateIconIndirect(pnt)

        ' Очистить Up
        NativeMethods.DestroyIcon(pnt)
        NativeMethods.DeleteObject(tmp.hbmMask)
        NativeMethods.DeleteObject(tmp.hbmColor)

        Return New Windows.Forms.Cursor(curPtr)
    End Function

#End Region

End Class
