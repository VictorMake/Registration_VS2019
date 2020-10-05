<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormParameterInRange
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormParameterInRange))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtКаналВДиапвзоне = New System.Windows.Forms.TextBox()
        Me.CWNumMax = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.CWNumMin = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.CWNumMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CWNumMin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtКаналВДиапвзоне)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 105)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(309, 304)
        Me.GroupBox1.TabIndex = 19
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Параметры соответсвующие условию"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(240, 24)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Имя:         Модуль:          Канал:"
        '
        'txtКаналВДиапвзоне
        '
        Me.txtКаналВДиапвзоне.BackColor = System.Drawing.SystemColors.Control
        Me.txtКаналВДиапвзоне.Location = New System.Drawing.Point(8, 40)
        Me.txtКаналВДиапвзоне.Multiline = True
        Me.txtКаналВДиапвзоне.Name = "txtКаналВДиапвзоне"
        Me.txtКаналВДиапвзоне.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtКаналВДиапвзоне.Size = New System.Drawing.Size(295, 256)
        Me.txtКаналВДиапвзоне.TabIndex = 6
        Me.txtКаналВДиапвзоне.WordWrap = False
        '
        'CWNumMax
        '
        Me.CWNumMax.CoercionInterval = 0.1R
        Me.CWNumMax.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval
        Me.CWNumMax.Location = New System.Drawing.Point(245, 73)
        Me.CWNumMax.Name = "CWNumMax"
        Me.CWNumMax.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.CWNumMax.Size = New System.Drawing.Size(74, 20)
        Me.CWNumMax.TabIndex = 21
        Me.CWNumMax.Value = 10.0R
        '
        'CWNumMin
        '
        Me.CWNumMin.CoercionInterval = 0.1R
        Me.CWNumMin.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval
        Me.CWNumMin.Location = New System.Drawing.Point(245, 41)
        Me.CWNumMin.Name = "CWNumMin"
        Me.CWNumMin.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange
        Me.CWNumMin.Size = New System.Drawing.Size(74, 20)
        Me.CWNumMin.TabIndex = 20
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(10, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(229, 24)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Максимальное значение (вольт)"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(10, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(229, 24)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Минимальное значение (вольт)"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(2, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(325, 32)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Введите диапазон напряжений в котором предполагается нужный параметр"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormParameterInRange2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(328, 411)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CWNumMax)
        Me.Controls.Add(Me.CWNumMin)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormParameterInRange2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Параметры в выбранном диапазоне"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.CWNumMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CWNumMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label4 As Label
    Public WithEvents txtКаналВДиапвзоне As TextBox
    Friend WithEvents CWNumMax As WindowsForms.NumericEdit
    Friend WithEvents CWNumMin As WindowsForms.NumericEdit
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
