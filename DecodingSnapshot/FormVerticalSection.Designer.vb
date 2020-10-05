<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormVerticalSection
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListViewGrid = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'ListViewGrid
        '
        Me.ListViewGrid.GridLines = True
        Me.ListViewGrid.Location = New System.Drawing.Point(12, 12)
        Me.ListViewGrid.MultiSelect = False
        Me.ListViewGrid.Name = "ListViewGrid"
        Me.ListViewGrid.Size = New System.Drawing.Size(330, 103)
        Me.ListViewGrid.TabIndex = 0
        Me.ListViewGrid.UseCompatibleStateImageBehavior = False
        Me.ListViewGrid.View = System.Windows.Forms.View.Details
        '
        'FormVerticalSection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 127)
        Me.Controls.Add(Me.ListViewGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(22, 443)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormVerticalSection"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Сечения"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Public WithEvents ListViewGrid As ListView
End Class
