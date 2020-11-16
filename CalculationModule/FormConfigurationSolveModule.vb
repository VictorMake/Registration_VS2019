Imports System.Collections.Generic

Public Class FormConfigurationSolveModule
    Private passportModules As Dictionary(Of String, PassportModule)
    Private managerCalculation As CalculationModuleManager

    Friend Sub New(ByVal inCalculationModuleManager As CalculationModuleManager)
        ' This is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        managerCalculation = inCalculationModuleManager
        passportModules = inCalculationModuleManager.PassportModuleDictionary
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        managerCalculation.DisenableModules()

        For I As Integer = 0 To CheckedListBoxModule.Items.Count - 1
            passportModules(CType(CheckedListBoxModule.Items(I), PassportModule).NameModule).Enable = CheckedListBoxModule.GetItemChecked(I)
        Next

        managerCalculation.SaveNameDllsToConfigurationXML()
        DialogResult = DialogResult.OK
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Закрытие окна " & Text)
        Close()
    End Sub

    Private Sub FormConfigurationSolveModule_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegistrationEventLog.EventLog_AUDIT_SUCCESS("Загрузка окна " & Text)
        LinkCheckedListBox()
        CheckedListBoxModule.Items.AddRange(passportModules.Values.ToArray)

        For I As Integer = 0 To CheckedListBoxModule.Items.Count - 1
            If managerCalculation.NameModulesInConfiguration.Contains(CType(CheckedListBoxModule.Items(I), PassportModule).NameModule) Then
                CheckedListBoxModule.SetItemChecked(I, True)
            End If
        Next
    End Sub

    Private Sub LinkCheckedListBox()
        ' создать BindingSource класса
        'Dim classBindingSource As New BindingSource()
        'classBindingSource.DataSource = ПаспортаМодулей
        'classBindingSource.AllowNew = False

        '' создать BindingNavigator для навигации сквозь класс
        ''Dim classBindingNavigator As New BindingNavigator(True)
        ''classBindingNavigator.BindingSource = classBindingSource

        '' связать controls с BindingSource
        ''CheckedListBoxModule.Text
        'CheckedListBoxModule.DataBindings.Add(New System.Windows.Forms.Binding("Text", classBindingSource, "ОписаниеМодуля")) ', True))

        '' заполнить control. показать какой член элемента коллекции добавленнный в контрол должен выводиться
        CheckedListBoxModule.DisplayMember = "DescriptionModule" '"ОписаниеМодуля"

        'Me.classEmployeeIdTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", classBindingSource, "EmployeeId", True))
        ' разместить навигатор по коллекции
        'Me.tabPage3.Controls.Add(classBindingNavigator)
        'classBindingNavigator.Dock = DockStyle.Top
    End Sub
End Class
