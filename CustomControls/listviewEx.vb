Namespace ListViewCustomReorder
    ''' <summary> 
    ''' ������� ����������� ������ ListView. 
    ''' </summary> 
    Public Class ListViewEx
        Inherits ListView

        Public Property dropIndex() As Integer
        Private Const WM_PAINT As Integer = &HF

        ' ���� ��� LVItem
        Dim itemOver As ListViewItem = Nothing
        Dim isDragging As Boolean = False

        Public Sub New()
            ' ����� �������� 
            DoubleBuffered = True
        End Sub

        Protected Overloads Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)

            ' ���� (������ ��������������� OnPaint()), ������ ��� ListView ������� ��������
            ' ������ ������ �������� ListView � � ��������� �� �������� OnPaint ������������
            If m.Msg = WM_PAINT Then
                If isDragging Then
                    If itemOver IsNot Nothing Then
                        If itemOver.Index >= 0 AndAlso itemOver.Index < Items.Count Then
                            Dim p As Point = PointToClient(New Point(MousePosition.X, MousePosition.Y))
                            Dim rc As Rectangle = Items(itemOver.Index).GetBounds(ItemBoundsPortion.Entire)
                            rc.Width = MyBase.ClientRectangle.Width

                            If p.Y - Items(itemOver.Index).Bounds.Top < Items(itemOver.Index).Bounds.Height / 2 Then
                                dropIndex = itemOver.Index
                                DrawInsertionLine(rc.Left, rc.Right, rc.Top)
                            Else
                                dropIndex = itemOver.Index + 1
                                DrawInsertionLine(rc.Left, rc.Right, rc.Bottom)
                            End If
                        End If
                    Else
                        dropIndex = -1
                        Dim rc As Rectangle

                        If MyBase.Items.Count > 0 Then
                            rc = Items(MyBase.Items.Count - 1).GetBounds(ItemBoundsPortion.Entire)
                        Else
                            rc = New Rectangle(0, 0, MyBase.ClientRectangle.Width, 0)
                        End If

                        rc.Width = MyBase.ClientRectangle.Width
                        DrawInsertionLine(rc.Left, rc.Right, rc.Bottom)
                    End If
                End If
            End If
        End Sub

        ''' <summary> 
        ''' �������� ����� � ����������� �������� � ������ �����
        ''' </summary> 
        ''' <param name="X1">��������� ������� (X) �����</param> 
        ''' <param name="X2">�������� ������� (X) �����</param> 
        ''' <param name="Y">������� (Y) ������</param> 
        Private Sub DrawInsertionLine(ByVal X1 As Integer, ByVal X2 As Integer, ByVal Y As Integer)
            Using g As Graphics = CreateGraphics()
                g.DrawLine(Pens.Red, X1, Y, X2 - 1, Y)

                Dim leftTriangle As Point() = New Point(2) {New Point(X1, Y - 4), New Point(X1 + 7, Y), New Point(X1, Y + 4)}
                Dim rightTriangle As Point() = New Point(2) {New Point(X2, Y - 4), New Point(X2 - 8, Y), New Point(X2, Y + 4)}
                g.FillPolygon(Brushes.Red, leftTriangle)
                g.FillPolygon(Brushes.Red, rightTriangle)
            End Using
        End Sub

        'Dim effectCursor As System.Windows.Forms.Cursor

        Protected Overrides Sub OnDragOver(ByVal drgevent As DragEventArgs)
            MyBase.OnDragOver(drgevent)

            isDragging = True

            Dim p As Point = PointToClient(New Point(drgevent.X, drgevent.Y))
            ' ������������ 0 ������ e.X ��� ��� ���������� ��������� ������ ������� ���� ���������� �������������
            itemOver = MyBase.GetItemAt(0, p.Y)

            ' ������� ����������� LV ��� ��� ����������� ����� ������ ���� ��������
            MyBase.Invalidate()
        End Sub

        Protected Overrides Sub OnDragDrop(ByVal drgevent As DragEventArgs)
            MyBase.OnDragDrop(drgevent)

            isDragging = False
            ' ������� ����������� LV ��� ��� ����������� ����� ������ ���� ��������
            MyBase.Invalidate()
        End Sub

        Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            itemOver = Nothing
            ' ������� ����������� LV ��� ��� ����������� ����� ������ ���� �������
            MyBase.Invalidate()
            MyBase.OnMouseLeave(e)
        End Sub
    End Class
End Namespace

