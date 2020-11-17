Imports System.Math
Imports System.Collections.Generic
Imports Registration.FormMain
Imports MathematicalLibrary

Friend Class FormTextControl
    Implements IUpdateSelectiveControls

    Private Structure ParameterStruct
        Dim numberParameter As Integer
        Dim nameParameter As String
    End Structure
    Private arrParameterStruct() As ParameterStruct

    ' ссылка на форму откуда вызвана
    Private parentFormRegistration As FormRegistrationBase
    Private mcolControls As List(Of Panel)
    Private isFormLoaded As Boolean
    Private countControl As Integer

    Private decFactorX As Double
    Private decFactorY As Double
    Private mptDesignScreenX As Integer
    Private mptDesignScreenY As Integer
    Private mptCurrentScreenX As Integer
    Private mptCurrentScreenY As Integer
    Private panels() As Panel
    Private isPanels As Boolean

    Private Const CtrlMask As Byte = 8
    Private Const fontSizeDesign As Single = 8.25
    Private Const tillOneColumn As Single = 0.2 ' до одного столбца
    Private Const tillTwoColumns As Single = 0.6 ' до двух столбцов
    Private Const tillSquare As Single = 2.5 ' до квадрата
    Private Const tillTwoRows As Single = 8 ' до двух строк
    ''' <summary>
    ''' менеджер настроек
    ''' </summary>
    Private mSettingSelectedParameters As SettingSelectedParameters

    Public Sub New(ByVal inParentForm As FormRegistrationBase)
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        Me.parentFormRegistration = inParentForm
    End Sub

    ''' <summary>
    ''' Задать значение параметра для индикатора с последующим обновлением
    ''' </summary>
    ''' <param name="arrСреднее"></param>
    ''' <param name="x"></param>
    Friend Sub UpdateValuesTextControls(ByRef arrСреднее(,) As Double, ByVal x As Integer)
        If isPanels Then
            Dim I As Integer
            Dim value As Double

            For Each ControlPanel As Panel In panels
                value = arrСреднее(arrParameterStruct(I).numberParameter, x)
                ControlPanel.Controls(0).Text = Round(value, Precision).ToString ' Format(value, "F" & Precision) 
                'ControlPanel.Controls(1) это NewPanelTop, ControlPanel.Controls(1).Controls(0) это NewLabel
                'CType(ControlPanel.Controls(1).Controls(0), LabelMinMax).CheckValueInRange(value)
                CType(ControlPanel.Controls(0), TextBoxMinMax).CheckValueInRange(value)
                I += 1
            Next
        End If
    End Sub

    Private Sub FormTextControl_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        ' 1 Считывается строка с параметрами если есть
        ' 2 Проверяется присутствие параметров в текущей конфигурации сбора
        ' 3 Последовательно добавляются панели с настройками и цветами как и на листе
        ' 4 Какое-то соответствие панелей и индексами в массиве собранных параметров
        ' при сборе в регистраторе или от сервера при изменении внешних  приходящих данных или
        ' строки конфигурации перезагружать форму.
        ' 5 В процессе сбора если форма загружена в цикле по панелям (или из индексам соответствия) заполняются текстовые поля
        ' 6 При добавлении новый параметр проверка на отсутствие копии, добавляется в конец, заново инициализируется массив соответствия номеров панелей и индексов массива средних
        ' 7 То же при удалении
        ' 8 При закрытии формы строка с параметрами записывается

        Left = CInt(CSng(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextLeft", CStr(0))))
        Top = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextTop", CStr(0)))
        Width = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextWidth", CStr(640)))
        Height = CInt(GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextHeight", CStr(480)))

        mptDesignScreenX = 52 'TextBox1.Size.Width
        mptDesignScreenY = 20 'TextBox1.Size.Height

        isFormLoaded = True
        PopulateTextControl()
        FormTextControl_Resize(Me, New EventArgs)
        parentFormRegistration.IsShowTextControl = True
    End Sub

    Private Sub FormTextControl_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)

        If WindowState <> FormWindowState.Minimized Then
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextLeft", CStr(Left))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextTop", CStr(Top))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextWidth", CStr(Width))
            SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "ViewTextHeight", CStr(Height))
        End If

        parentFormRegistration.IsShowTextControl = False
        parentFormRegistration.MenuShowTextControl.Enabled = True
        parentFormRegistration = Nothing
        SavePanelsToSetting()
    End Sub

    Private Sub FormTextControl_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Resize
        If isFormLoaded AndAlso isPanels Then
            Dim row, col As Integer
            Dim rowPanel As Integer = 1
            Dim colPanel As Integer = 1
            Dim ratio As Single ' ширина К высоте
            Dim index As Integer
            Dim rectangleWidth As Single = ClientRectangle.Width
            Dim rectangleHeight As Single = ClientRectangle.Height

            countControl = panels.Length

            If countControl > 1 Then
                ratio = rectangleWidth / rectangleHeight

                If ratio < tillOneColumn Then
                    ' 1 столбец
                    rowPanel = countControl
                    colPanel = 1
                ElseIf ratio >= tillOneColumn AndAlso ratio < tillTwoColumns Then
                    ' 2 столбца
                    colPanel = 2
                    rowPanel = Convert.ToInt32(countControl / colPanel)
                    If rowPanel * colPanel < countControl Then rowPanel += 1
                ElseIf ratio >= tillTwoColumns AndAlso ratio < tillSquare Then
                    ' квадрат
                    rowPanel = Convert.ToInt32(Sqrt(countControl))
                    colPanel = Convert.ToInt32(countControl / rowPanel)
                    If rowPanel * colPanel < countControl Then colPanel += 1
                ElseIf ratio >= tillSquare AndAlso ratio < tillTwoRows Then
                    ' 2 строки
                    rowPanel = 2
                    colPanel = Convert.ToInt32(countControl / rowPanel)
                    If rowPanel * colPanel < countControl Then colPanel += 1
                ElseIf ratio >= tillTwoRows Then
                    ' 1 строка
                    rowPanel = 1
                    colPanel = countControl
                End If
            End If

            ' идет от 0
            rowPanel -= 1
            colPanel -= 1

            Dim cx As Integer = ClientRectangle.Width \ (colPanel + 1)
            Dim cy As Integer = ClientRectangle.Height \ (rowPanel + 1)

            For row = 0 To rowPanel
                For col = 0 To colPanel
                    If ratio < tillOneColumn Then
                        ' 1 столбец
                        'Index = col * (colPanel + 1) + row
                        If index <= countControl - 1 Then panels(index).SetBounds(cx * col, cy * row, cx, cy)
                    ElseIf ratio >= tillOneColumn AndAlso ratio < tillTwoColumns Then
                        ' 2 столбца
                        'Index = col + row * (rowPanel + 1)
                        If index <= countControl - 1 Then panels(index).SetBounds(cx * col, cy * row, cx, cy)
                    ElseIf ratio >= tillTwoColumns AndAlso ratio < tillSquare Then
                        ' квадрат
                        'Index = col + row * (rowPanel + 1)
                        If index <= countControl - 1 Then panels(index).SetBounds(cx * col, cy * row, cx, cy)
                    ElseIf ratio >= tillSquare AndAlso ratio < tillTwoRows Then
                        ' 2 строки
                        'Index = col + row * (rowPanel + 1)
                        If index <= countControl - 1 Then panels(index).SetBounds(cx * col, cy * row, cx, cy)
                    ElseIf ratio >= tillTwoRows Then
                        ' 1 строка
                        'Index = col + row * (rowPanel + 1)
                        If index <= countControl - 1 Then panels(index).SetBounds(cx * col, cy * row, cx, cy)
                    End If
                    index += 1
                    'Console.WriteLine(Index)
                Next
            Next

            For Each ControlPanel As Panel In panels
                For Each MyControl As Control In ControlPanel.Controls
                    If TypeOf MyControl Is TextBox Then
                        SizeControl(MyControl)
                        Exit For
                    End If
                Next
            Next
        End If
    End Sub

    ''' <summary>
    ''' Промасштабировать Контрол
    ''' </summary>
    ''' <param name="ctl"></param>
    Public Sub SizeControl(ByRef ctl As Control)
        Dim decFactor As Double
        Dim decFontSize As Double

        mptCurrentScreenX = ctl.Size.Width
        mptCurrentScreenY = ctl.Size.Height

        If mptDesignScreenX = 0 Then mptDesignScreenX = mptCurrentScreenX
        If mptDesignScreenY = 0 Then mptDesignScreenY = mptCurrentScreenY

        decFactorX = (mptCurrentScreenX / mptDesignScreenX)
        decFactorY = (mptCurrentScreenY / mptDesignScreenY)

        decFactor = Min(decFactorX, decFactorY)
        decFontSize = Round(fontSizeDesign * decFactor * 1.5)

        If decFontSize < 1.0 Then decFontSize = 1.0
        If decFontSize > 127.0 Then decFontSize = 127.0

        ctl.Font = New Font("Microsoft Sans Serif", CSng(decFontSize))
    End Sub

    Private Sub FormTextControl_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles MyBase.DragEnter
        If (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem")) Then
            ' если Ctrl был нажат в течении операции перемещения произвести копирование, если нет - перемещение.
            If (e.KeyState And CtrlMask) = CtrlMask Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.Move
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub FormTextControl_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles MyBase.DragDrop
        ' если Ctrl был нажат, удалить эффект текста источника при перетаскивании 
        If (e.KeyState And CtrlMask) <> CtrlMask Then
            Dim NewListViewItem As ListViewItem = CType(e.Data.GetData(DataFormats.Serializable), ListViewItem)
            ' найти имя параметра из данных аргумента события перетаскивания
            Dim NameOfParametr As String = NewListViewItem.SubItems(0).Text

            If CheckOnRepetition(NameOfParametr) Then
                MessageBox.Show(Me, $"Параметр: <{NameOfParametr}> уже содержится в панели текстового контроля!",
                "Добавление параметра для контроля.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            AddPanel(NameOfParametr, NewListViewItem.ForeColor)
            ConfigureParameterStruct()
            FormTextControl_Resize(Me, New EventArgs)
            SavePanelsToSetting()
        End If
    End Sub

    ''' <summary>
    ''' Добавить Панель
    ''' </summary>
    ''' <param name="nameOfParametr"></param>
    ''' <param name="mBackColor"></param>
    Private Sub AddPanel(ByVal nameOfParametr As String, ByVal mBackColor As Color)
        Dim newPanelName As String = "Panel" & nameOfParametr
        Dim newLabelName As String = "Label" & nameOfParametr
        Dim newPanelTopName As String = "PanelTop" & nameOfParametr
        Dim newPictureBox1Name As String = "PictureBox1" & nameOfParametr
        Dim newPictureBox2Name As String = "PictureBox2" & nameOfParametr
        Dim newTextBoxName As String = "TextBox" & nameOfParametr

        Dim newPanel As Panel = New Panel
        Dim newLabel As Label = New Label ' LabelMinMax 
        Dim newPanelTop As Panel = New Panel
        Dim pictureBox1 As PictureBox = New PictureBox
        Dim pictureBox2 As PictureBox = New PictureBox
        Dim newTextBox As TextBoxMinMax = New TextBoxMinMax

        newPanel.SuspendLayout()
        newPanelTop.SuspendLayout()
        SuspendLayout()
        '
        'NewPanel
        '
        newPanel.BackColor = Color.Gray
        newPanel.BorderStyle = BorderStyle.Fixed3D
        newPanel.Controls.Add(newTextBox)
        newPanel.Controls.Add(newPanelTop)
        newPanel.Location = New Point(8, 8)
        newPanel.Name = newPanelName
        newPanel.Size = New Size(56, 48)
        '
        'NewTextBox
        '
        newTextBox.AutoSize = False
        newTextBox.BackColor = SystemColors.Control
        newTextBox.Dock = DockStyle.Fill
        newTextBox.Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType(204, Byte))
        newTextBox.ForeColor = SystemColors.WindowText
        newTextBox.Location = New Point(0, 24)
        newTextBox.Name = newTextBoxName
        newTextBox.ReadOnly = True
        newTextBox.Size = New Size(52, 20)
        newTextBox.TabStop = False
        newTextBox.Text = "0.00"
        newTextBox.TextAlign = HorizontalAlignment.Center
        newTextBox.WordWrap = False
        '
        'NewPanelTop
        '
        newPanelTop.BackColor = Color.White
        newPanelTop.Controls.Add(newLabel)
        newPanelTop.Controls.Add(pictureBox2)
        newPanelTop.Controls.Add(pictureBox1)
        newPanelTop.Dock = DockStyle.Top
        newPanelTop.Location = New Point(0, 0)
        newPanelTop.Name = newPanelTopName
        newPanelTop.Size = New Size(52, 24)
        '
        'PictureBox2
        '
        pictureBox2.BorderStyle = BorderStyle.Fixed3D
        pictureBox2.Dock = DockStyle.Left
        'PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        pictureBox2.Location = New Point(0, 0)
        pictureBox2.Name = newPictureBox2Name
        pictureBox2.Size = New Size(24, 24)
        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
        pictureBox2.TabStop = False
        '
        'PictureBox1
        '
        pictureBox1.BorderStyle = BorderStyle.Fixed3D
        pictureBox1.Dock = DockStyle.Right
        'PictureBox1.Image = CType(Resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        pictureBox1.Image = CType(ImageList1.Images(0), Image)
        pictureBox1.Location = New Point(28, 0)
        pictureBox1.Name = newPictureBox1Name
        pictureBox1.Size = New Size(24, 24)
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        pictureBox1.TabStop = False
        AddHandler pictureBox1.Click, AddressOf PictureBox_Click
        AddHandler pictureBox1.MouseEnter, AddressOf PictureBox_MouseEnter
        AddHandler pictureBox1.MouseLeave, AddressOf PictureBox_MouseLeave
        AddHandler pictureBox1.MouseDown, AddressOf PictureBox_MouseDown
        '
        'NewLabel
        '
        'поменять цвет
        newLabel.BackColor = mBackColor 'Color.Navy 
        newLabel.BorderStyle = BorderStyle.Fixed3D
        newLabel.Dock = DockStyle.Fill
        newLabel.Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType(204, Byte))
        newLabel.ForeColor = SystemColors.ControlText 'Color.Yellow 
        newLabel.Location = New Point(0, 0)
        newLabel.Name = newLabelName
        newLabel.Size = New Size(4, 24)
        newLabel.Text = nameOfParametr
        newLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        'FormTextControl
        '
        Controls.Add(newPanel)
        newPanel.ResumeLayout(False)
        newPanelTop.ResumeLayout(False)
        ResumeLayout(False)
        Refresh()
    End Sub

    ''' <summary>
    ''' Удалить панель
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PictureBox_Click(ByVal sender As Object, ByVal e As EventArgs)
        isPanels = False
        Controls.Remove(CType(sender, PictureBox).Parent.Parent)
        ConfigureParameterStruct()
        FormTextControl_Resize(Me, New EventArgs)
        SavePanelsToSetting()
    End Sub

    Private Sub PictureBox_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, PictureBox).Image = CType(ImageList1.Images(1), Image)
    End Sub

    Private Sub PictureBox_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, PictureBox).Image = CType(ImageList1.Images(0), Image)
    End Sub

    Private Sub PictureBox_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        CType(sender, PictureBox).Image = CType(ImageList1.Images(2), Image)
    End Sub

    ''' <summary>
    ''' Загрузить
    ''' </summary>
    Private Sub PopulateTextControl()
        ' считать из файла строку с параметрами контроля и расшифровать ее в массив
        mSettingSelectedParameters = New SettingSelectedParameters(PathResourses, [Enum].GetName(GetType(TypeFormTuningSelective), TypeFormTuningSelective.TextControl))

        ' создается массив индексов и конфигуриуется Контроль(i)
        For J As Integer = 0 To UBound(mSettingSelectedParameters.SelectedParametersAsString)
            For I As Integer = 1 To UBound(IndexParametersForControl)
                If ParametersType(IndexParametersForControl(I)).NameParameter = mSettingSelectedParameters.SelectedParametersAsString(J) Then
                    AddPanel(mSettingSelectedParameters.SelectedParametersAsString(J), ColorsNet((I - 1) Mod 7))
                    Exit For
                End If
            Next
        Next

        ConfigureParameterStruct()
    End Sub

    ''' <summary>
    ''' Запись Текстового Контроля
    ''' </summary>
    Private Sub SavePanelsToSetting()
        ' создать новый список
        Dim inNames As New List(Of String)

        If isPanels Then
            For I As Integer = 0 To UBound(arrParameterStruct)
                inNames.Add(arrParameterStruct(I).nameParameter)
            Next
        End If
        ' если переписать то удаление из списка старых и запись новых
        mSettingSelectedParameters.UpdateListParameter(inNames)
    End Sub

    ''' <summary>
    ''' Обновить Массивы
    ''' </summary>
    Private Sub ConfigureParameterStruct()
        Dim I, J As Integer
        Dim index As Integer
        Dim name As String
        Dim unitOfMeasure As String

        mcolControls = New List(Of Panel)

        For Each ctl As Control In Controls ' 1 уровень
            'If ctl.GetType Is GetType(System.Windows.Forms.Panel) Then
            If TypeOf ctl Is Panel Then
                mcolControls.Add(CType(ctl, Panel))
                I += 1
            End If
        Next

        panels = mcolControls.ToArray
        'ReDim_arrParameterStruct(panels.Length - 1)
        Re.Dim(arrParameterStruct, panels.Length - 1)

        If I > 0 Then
            isPanels = True
        Else
            Exit Sub
        End If

        I = 0

        For Each ControlPanel As Panel In panels
            For Each MyControl As Control In ControlPanel.Controls(1).Controls
                If TypeOf MyControl Is Label Then ' LabelMinMax
                    name = MyControl.Text
                    arrParameterStruct(I).nameParameter = name

                    For J = 1 To UBound(IndexParametersForControl)
                        If ParametersType(IndexParametersForControl(J)).NameParameter = name Then
                            unitOfMeasure = ParametersType(IndexParametersForControl(J)).UnitOfMeasure
                            index = Array.IndexOf(UnitOfMeasureArray, unitOfMeasure)

                            If index = -1 Then index = 6

                            'CType(ControlPanel.Controls(1).Controls(1), PictureBox).Image = CType(ФормаЛокальнаяСсылка.ImageList2.Images(Index), Image)
                            CType(ControlPanel.Controls(1).Controls(1), PictureBox).Image = CType(ImageList2.Images(index), Image)
                            arrParameterStruct(I).numberParameter = J - 1

                            'CType(MyControl, LabelMinMax).ДопускМинимум = ParametersType(ArrСписокПараметровКонтрол(J)).LowerLimit
                            'CType(MyControl, LabelMinMax).ДопускМаксимум = ParametersType(ArrСписокПараметровКонтрол(J)).UpperLimit
                            'CType(MyControl, LabelMinMax).АварийноеЗначениеМин = ParametersType(ArrСписокПараметровКонтрол(J)).AlarmValueMin
                            'CType(MyControl, LabelMinMax).АварийноеЗначениеМакс = ParametersType(ArrСписокПараметровКонтрол(J)).AlarmValueMax

                            Dim mTextBoxMinMax As TextBoxMinMax = CType(ControlPanel.Controls(0), TextBoxMinMax)
                            mTextBoxMinMax.LimitMin = ParametersType(IndexParametersForControl(J)).LowerLimit
                            mTextBoxMinMax.LimitMax = ParametersType(IndexParametersForControl(J)).UpperLimit
                            mTextBoxMinMax.AlarmMin = ParametersType(IndexParametersForControl(J)).AlarmValueMin
                            mTextBoxMinMax.AlarmMax = ParametersType(IndexParametersForControl(J)).AlarmValueMax

                            Exit For
                        End If
                    Next

                    I += 1
                    Exit For
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Проверка На Повтор
    ''' </summary>
    ''' <param name="nameOfParametr"></param>
    ''' <returns></returns>
    Private Function CheckOnRepetition(ByVal nameOfParametr As String) As Boolean
        If isPanels Then
            For I As Integer = 0 To arrParameterStruct.Length - 1
                If arrParameterStruct(I).nameParameter = nameOfParametr Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    Public Sub UpdateControls() Implements IUpdateSelectiveControls.UpdateControls
        Controls.Clear()
        PopulateTextControl()
        FormTextControl_Resize(Me, New EventArgs)
    End Sub

    Private Class TextBoxMinMax
        Inherits TextBox

        Public Property LimitMin As Double
        Public Property LimitMax As Double
        Public Property AlarmMin As Double ' АварийноеЗначениеМин
        Public Property AlarmMax As Double ' АварийноеЗначениеМакс

        ''' <summary>
        ''' Проверка нахождения значения параметра в диапазоне
        ''' и изменение цвета цифрового индикатора.
        ''' </summary>
        ''' <param name="value"></param>
        Public Sub CheckValueInRange(value As Double)
            Dim isAlarm As Boolean ' для аварийных индикаторов

            With Me
                ' 1. Заданы аварийные пределы
                If (AlarmMin <> 0 OrElse AlarmMax <> 0) Then
                    isAlarm = False

                    If value > AlarmMax Then
                        isAlarm = True
                        If .BackColor <> Color.LightSalmon Then
                            .BackColor = Color.LightSalmon
                            .ForeColor = Color.DarkViolet
                        End If
                    ElseIf value < AlarmMin Then
                        isAlarm = True
                        If .BackColor <> Color.Orange Then
                            .BackColor = Color.Orange
                            .ForeColor = Color.Maroon
                        End If
                    End If

                    If isAlarm = False Then
                        If .BackColor <> Color.White Then
                            .BackColor = Color.White
                            .ForeColor = Color.Black
                        End If
                    End If

                    Exit Sub
                End If

                ' 2. Проверка на выход из рабочего диапазона
                If value < LimitMin OrElse value > LimitMax Then
                    If .BackColor <> Color.LightYellow Then
                        .BackColor = Color.LightYellow
                        .ForeColor = Color.Blue
                    End If

                    Exit Sub
                End If

                ' 3. Не превысил аварийные допуски и находится в рабочем диапазоне
                If .BackColor <> Color.White Then
                    .BackColor = Color.White
                    .ForeColor = Color.Black
                End If

            End With
        End Sub
    End Class

End Class

'Private Class LabelMinMax
'    Inherits Label

'    Public Property ДопускМинимум As Double
'    Public Property ДопускМаксимум As Double
'    Public Property АварийноеЗначениеМин As Double
'    Public Property АварийноеЗначениеМакс As Double

'    ''' <summary>
'    ''' Проверка нахождения значения параметра в диапазоне
'    ''' и изменение цвета цифрового индикатора.
'    ''' </summary>
'    ''' <param name="value"></param>
'    Public Sub CheckValueInRange(value As Double)
'        Dim isAlarm As Boolean ' для аварийных индикаторов

'        With Me
'            ' 1. Заданы аварийные пределы
'            If (АварийноеЗначениеМин <> 0 OrElse АварийноеЗначениеМакс <> 0) Then
'                isAlarm = False

'                If value > АварийноеЗначениеМакс Then
'                    isAlarm = True
'                    If .BackColor <> Color.Red Then
'                        .BackColor = Color.Red
'                        .ForeColor = Color.Black
'                    End If
'                ElseIf value < АварийноеЗначениеМин Then
'                    isAlarm = True
'                    If .BackColor <> Color.Orange Then
'                        .BackColor = Color.Orange
'                        .ForeColor = Color.Black
'                    End If
'                End If

'                If isAlarm = False Then
'                    If .BackColor <> Color.Navy Then
'                        .BackColor = Color.Navy
'                        .ForeColor = Color.Yellow
'                    End If
'                End If

'                Exit Sub
'            End If

'            ' 2. Проверка на выход из рабочего диапазона
'            If value < ДопускМинимум OrElse value > ДопускМаксимум Then
'                If .BackColor <> Color.Yellow Then
'                    .BackColor = Color.Yellow
'                    .ForeColor = Color.Red
'                End If

'                Exit Sub
'            End If

'            ' 3. Не превысил аварийные допуски и находится в рабочем диапазоне
'            If .BackColor <> Color.Navy Then
'                .BackColor = Color.Navy
'                .ForeColor = Color.Yellow
'            End If

'        End With
'    End Sub
'End Class

'Private strРежим As String = "ТекстовыйКонтроль"
'Private blnЗаписьКофигурацииСуществует As Boolean

'Dim nameControl As String = Nothing

'If isPanels Then
'    ' сначало делается запись а затем считывание по новой
'    For I As Integer = 0 To UBound(arrParameterStruct)
'        If nameControl = vbNullString Then
'            nameControl = arrParameterStruct(I).nameParameter
'        Else
'            nameControl &= "\" & arrParameterStruct(I).nameParameter
'        End If
'    Next
'    nameControl &= "\"
'Else
'    nameControl = ""
'End If

''теперь запись
'Dim strSQL As String
'Dim cn As New OleDbConnection(BuildCnnStr(strProviderJet, strПутьChannels))
'Dim cmd As OleDbCommand = cn.CreateCommand
'cmd.CommandType = CommandType.Text
'cn.Open()

''strSQL = "SELECT COUNT(*) FROM [Режимы" & strНомерСтенда & "] WHERE [Наименование]= '" & strРежим & "'"
''cmd.CommandType = CommandType.Text
''cmd.CommandText = strSQL
''If CInt(cmd.ExecuteScalar) = 0 Then
''    'записи нет, надо создать
''    cmd.CommandText = "INSERT INTO [Режимы" & strНомерСтенда & "] (НомерРежима, Наименование, ПереченьПараметров, ТекстСправки) VALUES (100, '" & strРежим & "', 'аРУД\', 'Параметры для тектового контоля');"
''    cmd.ExecuteNonQuery()
''End If
'If blnЗаписьКофигурацииСуществует = False Then
'    'записи нет, надо создать
'    cmd.CommandText = "INSERT INTO [Режимы" & strНомерСтенда & "] (НомерРежима, Наименование, ПереченьПараметров, ТекстСправки) VALUES (100, '" & strРежим & "', 'аРУД\', 'Параметры для тектового контоля');"
'    cmd.ExecuteNonQuery()
'End If

'strSQL = "Update Режимы" & strНомерСтенда & " SET ПереченьПараметров = '" & strСуммарнаяСтрока & "' WHERE ([Наименование]= '" & strРежим & "'" & ")"
'cmd.CommandText = strSQL
'cmd.ExecuteNonQuery()
'cn.Close()

'SaveSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "TextOfControl", nameControl)
'Dim strSetting As String = GetSetting(Reflection.Assembly.GetExecutingAssembly.GetName.Name, "Settings", "TextOfControl", "")
