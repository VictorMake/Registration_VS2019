Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text

Module ModuleCompactRio

    ''' <summary>
    ''' ������������� ������ ������ �����.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EnumModeWork
        <Description("���������")>
        Measurement
        <Description("��������� � ����������")>
        Control
        <Description("�����.")>
        Unknown
    End Enum

#Region "���� � ������"
    Public Const FPGA_BITFILES As String = "FPGA Bitfiles"
    Public Const NI_RT As String = "ni-rt"
    Public Const STARTUP As String = "startup" ' "startupTest"
    Public Const NI_RT_STARTUP As String = NI_RT & "/" & STARTUP
    Public Const SEARCH_PATTERN_LVBITX As String = "*.lvbitx"
    Public Const CHANNELS_FILE_CSV As String = "Channels.csv"
    Public Const ATTRIBUTE_CHANNELS_FILE_CSV As String = "AttributeChannels.csv"
    Public Const CHASSIS_OPTION_INI As String = "ChassisOption.ini"
    Public Const C_NI_RT_STARTUP As String = "c\ni-rt\startup"
    Public Const NI_RT_INI As String = "ni-rt.ini"
    Public Const Path_Server_Data_Base As String = "����_�_����_������_�������"

    Public PathServerDataBase As String ' ���� � ���� ������ �������
    Public PathOptions_xml As String
#End Region

    <DllImport("RestartTargetChassisDLL.dll")>
    Public Sub TryRestartChassis(<MarshalAs(UnmanagedType.LPStr)> StringIn As String, <MarshalAs(UnmanagedType.LPStr)> Result As StringBuilder, le2 As Integer, <MarshalAs(UnmanagedType.LPStr)> SourseError As StringBuilder, len As Integer)
    End Sub
End Module