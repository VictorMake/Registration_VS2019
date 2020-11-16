﻿Imports System.Threading

''' <summary>
''' Модальный диалог с индикатором прогресса для пакетной замены
''' </summary>
''' <remarks></remarks>
Public Class frmBatchReplace
    Private m_bWorckDone As Boolean = False
    Private m_Thread As Thread
    Private m_iFiles As Integer

    ' Приложение позволяет ввести/сохранить/загрузить настройки, а также обладает режимом предварительного просмотра. Но главной целью этого приложения является пакетная замена. 
    ' Созданы два режима пакетной замены. Один должен вызываться из GUI (при нажатии кнопки  в ToolBar-е или при выборе пункта меню "Run\Go"). 
    ' Второй должен позволять производить замену из командной строки. При этом пользователь вообще не обязан интерактивно взаимодействовать с приложением 
    ' (все необходимые настройки должны передаваться через командную строку).
    ' Во время замены будет выводить модальный диалог с индикатором прогресса и кнопкой, позволяющей прервать замену, блокируя тем самым пользовательский интерфейс.
    ' Это диалог оформлен как отдельная форма.

    ' При открытии этого диалога запускается долгая (возможно) операция, во время которой нельзя изменять настройки поиска/замены. 
    ' Но у пользователя должна быть возможность прервать эту долгую работу в любой момент. Например, он может понять, что созданное им регулярное выражение неверно, 
    ' и что дальнейшее продолжение процесса поиска и замены будет бесполезным. Именно для этого на форме frmBatchReplace имеется кнопка pbCancel. 
    ' Но замена происходит в глухом цикле, и сообщения Windows (такие сообщения от клавиатуры и мыши) не будут обработаны до окончания этого цикла.
    ' Есть два способа заставить работать пользовательский интерфейс. Первый – проталкивать сообщения, например, вызвать функции Windows API PeekMessage, TranslateMessage, DispatchMessage. 
    ' Но при этом придется влезать в подробности Windows, да и код окажется довольно большим. 
    ' Второй способ – создать дополнительный рабочий поток и выполнить всю работу в нем.
    ' Раньше работал первый подход, но теперь, когда в .Net работа с потоками так упростилась, а обработка сообщений Windows столь сильно завуалирована, втором варианте проще.
    ' Итак, чтобы пользователь мог нажать кнопку, остановив тем самым пакетную замену, нужно производить поиск и замену в отдельном потоке. 
    ' Этот поток должен также обновлять состояние полосы прогресса и содержимое элемента управления Label. Потоку потребуются данные, которые находятся в форме. 
    ' Можно просто скопировать их в отдельную структуру и передать потоку (в принципе, так было бы правильнее). Но создавать промежуточные структуры – это немалый труд. 
    ' Лень, двигатель прогресса, подтолкнула испробовать устойчивость и надежность WinForms и вместо создания промежуточной структуры вызывать из рабочего потока функцию главной формы.

    ' До этого момента запускать поток опасно, так как он должен периодически обновлять состояние, модифицируя расположенные на этой форме элементы управления.
    ' Создаваемому потоку нужно передать делегат, содержащий ссылку на метод, являющийся главным методом потока (его стартовой точкой). 
    ' Делегат может содержать ссылку на метод экземпляра объекта (т.е. фактически содержать адрес метода и ссылку на экземпляр объекта). 
    ' Это делает работу с потоком похожей на вызов метода, только вызов происходит в контексте другого потока, и сразу после вызова управление возвращается в основной поток.
    ' Обработчик frmBatchReplace_Load приводит ссылку на родительское окно к ссылке на основную форму приложения и передает в делегат ссылку на его метод BatchReplase. 
    ' Теперь можно запускать поток.

    ''' <summary>
    ''' При загрузке форма генерирует событие Load. В нем создадан и запущен рабочий поток.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmBatchReplace_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim frmMain As frmRegularExpressionsReplace = DirectCast(Owner, frmRegularExpressionsReplace)

        m_bWorckDone = False
        ' Для запуска потока используя статическую процедуру потока, использовать
        ' имя класса и имя метода когда создаёте ThreadStart
        ' делегат. Visual Basic расширяет AddressOf выражение 
        ' к соответствующему делегату используя синтаксис: New ThreadStart(AddressOf Work.DoWork)
        '    
        'Dim newThread As New Thread(AddressOf Work.DoWork)
        'newThread.Start()
        '
        ' Для запуска потока используя метод экземпляра для процедуры потока 
        ' использовать экземпляр переменной и имя метода
        ' при создании ThreadStart делегата. Visual Basic расширяет AddressOf выражение 
        ' к соответствующему делегату используя синтаксис: New ThreadStart(AddressOf Work.DoWork)
        '
        'Dim w As New Work()
        'newThread = New Thread(AddressOf w.DoMoreWork)
        'newThread.Start()

        ' Для создания потоко-защищённого вызова (Thread-Safe Calls) для Windows Forms Controls
        ' можно выключить исключение установив значение для CheckForIllegalCrossThreadCalls свойства в false. 
        ' В этом случае контрол запускается тем же самым путём как бы был запущен под Visual Studio 2003.
        'Control.CheckForIllegalCrossThreadCalls = False

        m_Thread = New Thread(AddressOf frmMain.BatchReplase)
        'то же самое  m_Thread = New Thread(New ThreadStart(AddressOf frmMain.BatchReplase))

        m_Thread.Start()
    End Sub

    ''' <summary>
    ''' Информация о количестве файлов задается перед началом обработки функцией SetInfo.
    ''' </summary>
    ''' <param name="iFiles"></param>
    ''' <remarks></remarks>
    Public Sub SetInfo(iFiles As Integer)
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() SetInfo(iFiles)))
        Else
            m_iFiles = iFiles
            ProgBar.Maximum = iFiles
            UpdateInfo(0)
        End If
    End Sub

    ''' <summary>
    ''' Этот метод вызывается основным рабочим методом потока
    ''' когда оканчивается очередная итерация.
    ''' </summary>
    ''' <param name="iFile"></param>
    ''' <remarks></remarks>
    Public Sub UpdateInfo(iFile As Integer)
        ' Передаваемый в этот метод делегат вызывается для каждого найденного файла. 
        ' ProcessFile формирует полный путь к файлу и производит в нем замену. 
        ' После этого вызывается метод UpdateInfo формы индикации прогресса (m_frmBatchRep).
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() UpdateInfo(iFile)))
        Else
            ProgBar.Value = iFile
            lbInfo.Text = String.Format("Files: {0} of {1}", iFile, m_iFiles)
            If m_iFiles = iFile Then
                ' Выставляем флаг говорящий том, что работа потока завершена.
                m_bWorckDone = True
                ' Закрываем окно.
                Close()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Событие Closing, вызываемого у формы, когда пользователь (или программа) пытается закрыть окно.
    ''' Обрабатывать нажатие кнопки pbCancel нет необходимости.
    ''' В свойстве DialogResult формы значение Cancel установлено на pbCancel 
    ''' и она будет пытаться закрыть окно при нажатии на нее или нажатии кнопки ESC.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmBatchReplace_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ' При создании многопоточных приложений другими средствами разработки (до .Net) использование исключений 
        ' для завершения работы потока считалось очень нежелательным.
        ' В то же время логика «аварийного выхода» во многих случаях оказывается чересчур громоздкой и сложной для реализации.
        ' В .Net прерывание потока по исключению не является опасной ситуацией.
        ' GC освобождает разработчиков от контроля за правильностью освобождения памяти, тем самым упрощая работу с потоками.
        ' Для аварийной остановки работы потока в .Net предусмотрен даже специальный тип исключения – System.Threading.ThreadAbortException.
        ' Его не нужно возбуждать вручную. Это делает метод Abort объекта Thread. Этот метод может быть вызван из любого потока.
        ' Воспользуемся им для остановки рабочего потоке в случае, если пользователь захочет закрыть диалог отображения прогресса 
        ' (нажав крестик в правом верхнем углу окна). 

        ' В качестве метода потока используется метод основной формы,
        ' которая является родительской по отношению к этой.
        Dim frmMain As frmRegularExpressionsReplace = DirectCast(Owner, frmRegularExpressionsReplace)
        ' Оповещаем поток о том, что ему нужно закрыться.
        'frmMain.StopBatchReplase = true так не надо делать
        If Not m_bWorckDone Then
            frmMain.m_bCancelBatch = True
            m_Thread.Abort()
        End If
    End Sub

End Class