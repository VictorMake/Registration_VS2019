<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormProtocol
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormProtocol))
        Me.DGVProtocol = New System.Windows.Forms.DataGridView()
        Me.ColumnПараметр = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnЗначениеПараметра = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnНормаТУ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DGVProtocol, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGVПротокол
        '
        Me.DGVProtocol.AllowUserToAddRows = False
        Me.DGVProtocol.AllowUserToDeleteRows = False
        Me.DGVProtocol.AllowUserToResizeColumns = False
        Me.DGVProtocol.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.DGVProtocol.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DGVProtocol.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DGVProtocol.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVProtocol.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DGVProtocol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVProtocol.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnПараметр, Me.ColumnЗначениеПараметра, Me.ColumnНормаТУ})
        Me.DGVProtocol.Dock = System.Windows.Forms.DockStyle.Top
        Me.DGVProtocol.Location = New System.Drawing.Point(0, 0)
        Me.DGVProtocol.MultiSelect = False
        Me.DGVProtocol.Name = "DGVПротокол"
        Me.DGVProtocol.RowHeadersVisible = False
        Me.DGVProtocol.Size = New System.Drawing.Size(677, 471)
        Me.DGVProtocol.TabIndex = 7
        '
        'ColumnПараметр
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ColumnПараметр.DefaultCellStyle = DataGridViewCellStyle3
        Me.ColumnПараметр.FillWeight = 108.4038!
        Me.ColumnПараметр.HeaderText = "Оцениваемый параметр"
        Me.ColumnПараметр.Name = "ColumnПараметр"
        Me.ColumnПараметр.ReadOnly = True
        Me.ColumnПараметр.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ColumnПараметр.Width = 230
        '
        'ColumnЗначениеПараметра
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ColumnЗначениеПараметра.DefaultCellStyle = DataGridViewCellStyle4
        Me.ColumnЗначениеПараметра.FillWeight = 73.83957!
        Me.ColumnЗначениеПараметра.HeaderText = "Значение параметра"
        Me.ColumnЗначениеПараметра.Name = "ColumnЗначениеПараметра"
        Me.ColumnЗначениеПараметра.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ColumnЗначениеПараметра.Width = 156
        '
        'ColumnНормаТУ
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Red
        Me.ColumnНормаТУ.DefaultCellStyle = DataGridViewCellStyle5
        Me.ColumnНормаТУ.FillWeight = 74.15202!
        Me.ColumnНормаТУ.HeaderText = "Норма ТУ"
        Me.ColumnНормаТУ.Name = "ColumnНормаТУ"
        Me.ColumnНормаТУ.ReadOnly = True
        Me.ColumnНормаТУ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ColumnНормаТУ.Width = 156
        '
        'frmПротокол
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(677, 516)
        Me.Controls.Add(Me.DGVProtocol)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmПротокол"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Протокол"
        Me.TopMost = True
        CType(Me.DGVProtocol, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DGVProtocol As System.Windows.Forms.DataGridView
    Friend WithEvents ColumnПараметр As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnЗначениеПараметра As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnНормаТУ As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
