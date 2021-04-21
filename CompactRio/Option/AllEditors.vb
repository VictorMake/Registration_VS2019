Imports System.Drawing.Design
Imports System.ComponentModel
Imports System.Windows.Forms.Design
Imports System.ComponentModel.Design
Imports Registration.My

Module AllEditors

#Region "Редакторы типа UITypeEditor"

#Region "EnumModeWorkEditor"
    ''' <summary>
    ''' Добавляет картинки, соответствующие каждому члену перечисления (для свойства ModeWork() As EnumModeWork)
    ''' Наследует базовый класс, используемый для конструирования редакторов значений, которые обеспечивают интерфейс пользователя визуализацией и редактированием значений объектов поддерживаемых типов данных.
    ''' </summary>
    Public Class EnumModeWorkEditor
        Inherits UITypeEditor

        ''' <summary>
        ''' Указывает, поддерживает ли указанный контекст художественное оформление значения объекта в пределах определенного контекста.
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function GetPaintValueSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Рисует представление значения объекта с помощью указанного аргумента System.Drawing.Design.PaintValueEventArgs.
        ''' </summary>
        ''' <param name="e">Аргумент System.Drawing.Design.PaintValueEventArgs указывает предмет и место рисования.</param>
        ''' <remarks></remarks>
        Public Overloads Overrides Sub PaintValue(ByVal e As PaintValueEventArgs)
            ' картинки хранятся в ресурсах с именами, соответствующими
            ' именам каждого члена перечисления EnumModeWork
            Dim resourcename As String = DirectCast(e.Value, EnumModeWork).ToString()

            ' достаем картинку из ресурсов
            Dim enumImage As Bitmap = DirectCast(Resources.ResourceManager.GetObject(resourcename), Bitmap)
            Dim destRect As Rectangle = e.Bounds ' Возвращает прямоугольник, определяющий область, в котором должно выполняться рисование.
            enumImage.MakeTransparent() ' Делает стандартно прозрачные цвета прозрачными для данного изображения System.Drawing.Bitmap.

            ' и отрисовываем
            e.Graphics.DrawImage(enumImage, destRect)
        End Sub
    End Class
#End Region

#Region "IPAddressEditor"
    ''' <summary>
    ''' Модальный редактор IP адреса для PropertyGrid
    ''' </summary>
    Public Class IPAddressEditor
        Inherits UITypeEditor

        ''' <summary>
        ''' Реализация метода редактирования
        ''' </summary>
        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            If (context IsNot Nothing) AndAlso (provider IsNot Nothing) Then
                Dim svc As IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

                If svc IsNot Nothing Then
                    Using ipfrm As New IPAddressEditorForm((DirectCast(value, IPAddressCls)))
                        If svc.ShowDialog(ipfrm) = DialogResult.OK Then
                            value = ipfrm.IP
                        End If
                    End Using
                End If
            End If

            Return MyBase.EditValue(context, provider, value)
        End Function

        ''' <summary>
        ''' Возвращаем стиль редактора - модальное окно
        ''' </summary>
        Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If context IsNot Nothing Then
                Return UITypeEditorEditStyle.Modal
            Else
                Return MyBase.GetEditStyle(context)
            End If
        End Function

    End Class
#End Region
#End Region

    ''' <summary>
    ''' Класс для установки фильтра по расширению при выборе Конфигурационного файла Сервера (*.xml).
    ''' При редактировании свойства, вызывается OpenFileDialog.
    ''' </summary>
    Class ChannelsFileEditor
        Inherits FileNameEditor
        ''' <summary>
        ''' Настройка фильтра расширений 
        ''' </summary>
        Protected Overloads Overrides Sub InitializeDialog(ByVal ofd As OpenFileDialog)
            ofd.CheckFileExists = False
            ofd.Filter = "База данных каналов (*.mdb)|*.mdb"
        End Sub
    End Class

    ''' <summary>
    ''' Класс для установки каталога исполняемых файлов для копирования.
    ''' При редактировании свойства, вызывается FolderBrowser.
    ''' </summary>
    Class CatalogEditor
        Inherits FolderNameEditor

        ''' <summary>
        ''' Диалог выбора папки 
        ''' </summary>
        Protected Overloads Overrides Sub InitializeDialog(folderBrowser As FolderBrowser)
            ' или
            'Protected Overloads Sub InitializeDialog(folderBrowser As FolderNameEditor) 
            folderBrowser.Description = "Укажите папку, содержащую исполняемые файлы"
            folderBrowser.Style = FolderBrowserStyles.RestrictToFilesystem ' только локальные системные каталоги
            folderBrowser.StartLocation = FolderBrowserFolder.MyComputer 'gПутьРесурсы ' начальное значение корневого узла
        End Sub
    End Class

#Region "ConstraintIrregularityCollectionEditor"
    ' Реализация CollectionEditor -> ChassisCollectionEditor, умеет запоминать положение и размеры своего окна, 
    ' меняет стандартные подписи на соответствующие редактируемым данным и добавляет окно с расширенной подсказкой по свойствам:

    ''' <summary>
    ''' Свойтво CollectionEditor для редактирования списка подключённых шасси -
    ''' для задания заголовка и запоминания положения окна.
    ''' Реализует пользовательский интерфейс, позволяющий редактировать коллекции большинства типов во время разработки.
    ''' </summary>
    Class ChassisCollectionEditor
        Inherits CollectionEditor

        ''' <summary>
        ''' Конструктор
        ''' </summary>
        Public Sub New(ByVal t As Type)
            MyBase.New(t)
        End Sub

        Private collForm As CollectionForm ' = MyBase.CreateCollectionForm()' Предоставляет модальное диалоговое окно для редактирования содержимого коллекции с помощью System.Drawing.Design.UITypeEditor.
        Private labelDescriptionProperties As New Label()
        Const PropertiesText As String = "&Список шасси cRio в ИВК:"
        Const MembersText As String = "&Target:"

        ''' <summary>
        ''' Перекрытый метод создания формы редактора - для сохранения/восстановления
        ''' размеров и положения окна
        ''' </summary>
        Protected Overloads Overrides Function CreateCollectionForm() As CollectionForm
            collForm = MyBase.CreateCollectionForm() ' Создает новую форму для отображения и редактирования текущей коллекции.

            ' подключаем свой обработчик открытия и в нем 
            ' восстанавливаем положение и размер окна
            AddHandler collForm.Load, AddressOf CollectionForm_Load
            ' подключаем свой обработчик закрытия и в нем сохраняем положение и размер окна
            AddHandler collForm.FormClosing, AddressOf CollectionForm_FormClosing

            Return collForm
        End Function

        Private Sub CollectionForm_Load(ByVal sender As Object, ByVal e As EventArgs)
            collForm.HelpButton = False ' кнопку справки не отображать

            '--- Восстановление положения окна и смена заголовка -------------
            ' пока это положение первый раз не сохранено, при 
            ' попытке чтения будет кирдык
            Try
                collForm.Size = MySettings.[Default].TargetGridCollectFormSize
                collForm.Location = MySettings.[Default].TargetGridCollectFormLocation
                collForm.WindowState = MySettings.[Default].TargetGridCollectFormState
            Catch
                'Trace.WriteLine("ConstraintIrregularityCollectionEditor::CreateCollectionForm() exception")
            End Try
            collForm.Text = "Редактор настроек шасси и каталога с программой для закачки"

            '--- Русификация и кастомизация надписей на форме ----------------
            ' перебираем все контролы на форме и заменяем
            ' неправильные надписи
            For Each ctrl As Control In collForm.Controls
                For Each ctrl1 As Control In ctrl.Controls

                    If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Label" AndAlso (ctrl1.Text = "&Members:" OrElse ctrl1.Text = "&Члены:") Then
                        ctrl1.Text = MembersText
                    End If

                    If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Label" AndAlso (ctrl1.Text.Contains("&properties") OrElse ctrl1.Text.Contains("&Cвойства")) Then
                        labelDescriptionProperties = DirectCast(ctrl1, Label)
                        labelDescriptionProperties.Text = PropertiesText
                    End If

                    If ctrl1.[GetType]().ToString() = "System.ComponentModel.Design.CollectionEditor+FilterListBox" Then
                        ' это самый правильный обработчик, но после него
                        ' срабатывает обработчик формы и меняет надпись на свою

                        ' вместо одного правильного - два обходных - 
                        ' на смену индекса в листбоксе:

                        AddHandler DirectCast(ctrl1, ListBox).SelectedIndexChanged, AddressOf ListBox_SelectedIndexChanged
                    End If

                    If ctrl1.[GetType]().ToString() = "System.Windows.Forms.Design.VsPropertyGrid" Then
                        ' и на редактирование в PropertyGrid
                        AddHandler DirectCast(ctrl1, PropertyGrid).SelectedGridItemChanged, AddressOf PropertyGrid_SelectedGridItemChanged
                        ' также сделать доступным окно с подсказками по параметрам в нижней части 
                        DirectCast(ctrl1, PropertyGrid).HelpVisible = True
                        DirectCast(ctrl1, PropertyGrid).HelpBackColor = System.Drawing.SystemColors.Info
                    End If
                Next
            Next
        End Sub

        Private Sub ListBox_SelectedIndexChanged(ByVal sndr As Object, ByVal ea As EventArgs)
            labelDescriptionProperties.Text = PropertiesText
        End Sub

        Private Sub PropertyGrid_SelectedGridItemChanged(ByVal sndr As Object, ByVal segichd As SelectedGridItemChangedEventArgs)
            labelDescriptionProperties.Text = PropertiesText
        End Sub

        Private Sub CollectionForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            ' Сохранение положения окна
            MySettings.[Default].TargetGridCollectFormState = collForm.WindowState
            If collForm.WindowState = FormWindowState.Normal Then
                MySettings.[Default].TargetGridCollectFormSize = collForm.Size
                MySettings.[Default].TargetGridCollectFormLocation = collForm.Location
            Else
                MySettings.[Default].TargetGridCollectFormSize = collForm.RestoreBounds.Size
                MySettings.[Default].TargetGridCollectFormLocation = collForm.RestoreBounds.Location
            End If
        End Sub
    End Class
#End Region
End Module