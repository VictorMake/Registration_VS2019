
'Friend Class StringIntObject
'    Private mNameChannel As String
'    Private mImageIndex As Integer

'    Public Sub New(ByVal nameChannel As String, ByVal imageIndex As Integer)
'        mNameChannel = nameChannel
'        mImageIndex = imageIndex
'    End Sub

'    Public Property ImageIndex As Integer
'        Get
'            Return mImageIndex
'        End Get
'        Set(ByVal value As Integer)
'            mImageIndex = value
'        End Set
'    End Property

'    Public Property NameChannel As String
'        Get
'            Return mNameChannel
'        End Get
'        Set(ByVal value As String)
'            mNameChannel = value
'        End Set
'    End Property

'    Public Overrides Function ToString() As String
'        Return mNameChannel
'    End Function
'End Class

''' <summary>
''' Вспомогательный класс для заполнения списка
''' </summary>
''' <remarks></remarks>
Friend Class StringIntObject
    Public s As String
    Public i As Integer

    Public Sub New(ByVal sz As String, ByVal n As Integer)
        s = sz
        i = n
    End Sub

    Public Overrides Function ToString() As String
        Return s
    End Function
End Class