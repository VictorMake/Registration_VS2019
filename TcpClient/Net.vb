Imports System.Collections.Generic

''' <summary>
''' Функции поддержки сетевого взаимодействия с compactRio и Сервером.
''' </summary>
Module Net
    Public Enum CommandSetServer As Byte
        Default_0 = 0
        Byte_1 = 1
        Integer_2 = 2
        Double_3 = 3
        Anything_4 = 4
        SmallString_51 = 51
        LongString_52 = 52
        ChannelProperty_53 = 53
        Description_54 = 54
        GetData_100 = 100
    End Enum

    Public gHashStatusPin As UInt32
    Public gWatchdogChannelValue As Integer = 0
    Public gClientSendData As Boolean = False
    Public gPacketArraySmallString As New List(Of Byte) ' для передачи Серверу сообщений
    Public gPacketArray As New List(Of Byte) ' для накопления пакета собранных данных и передачи Серверу
    Public gHashСommandCh As UInt32

    Private HashTime As UInt32
    'Private arrayLengthAllChassis As Integer ' кол. всех каналов ССД (сумма по всем шасси)
    ' Этот канал ССД является счётчиком для оценки работы и частоты сбора
    'Private WatchdogChannel As String = String.Empty
    Private HashWatchdogPin As UInt32

    ''' <summary>
    ''' Формирование пакетов для отправки их на сервер (реализация потока формирования сетевых пакетов).
    ''' </summary>
    ''' <param name="PacketArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ComposePacket(ByRef packetArray As List(Of Byte)) As Byte()
        'Public Function ComposePacket(ByRef packetArray As List(Of Byte), ByVal acquisitionDataAllShassis As Double(), ByVal chPinHashOneShassis As UInt32()) As Byte()
        If Not gClientSendData Then Return New Byte() {}

        Dim offset As Integer
        ' Определяем локальное время
        'Dim m As Integer = 0
        ' в потоке задачи DoMonitorConnections отправка командных каналов делается только раз
        'If AddCommVal Then
        Dim sysTime As DateTime = Now
        Dim timeString As String = Nothing

        packetArray.Clear()
        ' Канал управления  
        'hash_reqCh = NameToHash(cfg.ssd.opts.commandCh) 'MyNameToHash
        ' Расчитываем HASH сумму канала WatchdogChannel
        'hashPin = NameToHash(cfg.ssd.wd_ch.pin) 'MyNameToHash

        ' m = 0 'канал времени с индексом 0
        'timeString = "26.07.2011 09:00:32.93" 'для теста2
        timeString = $"{sysTime.ToShortDateString} {sysTime.ToLongTimeString}.{sysTime.Millisecond}"

        ''tasks(i).hash(k) канал времени  2TIME001 
        ChannelPacket(packetArray, HashTime, CommandSetServer.SmallString_51, 0, Nothing, timeString, 0, offset)
        'm += 1
        ' Запрос канала статуса необходимо делать в каждом цикле, чтобы не пропустить ответ от сервера, пересылающей команду с панели управления
        ChannelPacket(packetArray, 0, CommandSetServer.GetData_100, 0, BitConverter.GetBytes(gHashСommandCh), Nothing, 1, offset)

        ' Отправляем сторожевой таймер WatchdogChannel наверно последний канал
        If HashWatchdogPin <> 0 Then
            gWatchdogChannelValue += 1
            ChannelPacket(packetArray, HashWatchdogPin, CommandSetServer.Integer_2, CUInt(gWatchdogChannelValue), BitConverter.GetBytes(gWatchdogChannelValue), Nothing, 0, offset)
        End If
        'AddCommVal = False

        'If 0 = Interlocked.Exchange(usingResource, 1) Then
        '    'не используется
        '    AddCommVal = False
        '    Interlocked.Exchange(usingResource, 0)
        'Else
        '    'отвегнуто
        '    Stop
        'End If
        'End If

        'For k As Integer = 0 To acquisitionDataAllShassis.Length - 1
        '    'ChannelPacket(ChPinHashOneShassis(k), CommandSetServer.Double_3, k, BitConverter.GetBytes(AcquisitionDataOneShassis(k)), Nothing, 0, offset)
        '    ChannelPacket(packetArray, chPinHashOneShassis(k), CommandSetServer.Double_3, 0, BitConverter.GetBytes(acquisitionDataAllShassis(k)), Nothing, 0, offset)
        '    'm += 1
        'Next

        Return packetArray.ToArray
    End Function

    ''' <summary>
    ''' Функция отправляет сообщение на канал статуса ССД.
    ''' </summary>
    ''' <param name="PacketArraySmallString"></param>
    ''' <param name="chNameHash"></param>
    ''' <param name="_string"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SendStatus(ByRef packetArraySmallString As List(Of Byte), chNameHash As UInt32, _string As String) As Byte()
        Dim offset As Integer
        'Dim subpacketSize As Integer = 0
        packetArraySmallString.Clear()

        If chNameHash <> 0 Then
            ChannelPacket(packetArraySmallString, chNameHash, CommandSetServer.SmallString_51, 0, Nothing, _string, 0, offset)
        End If

        Return packetArraySmallString.ToArray
    End Function

    ''' <summary>
    ''' Функция отправляет описание ССД на сервер отправляет описание клиента на сервер.
    ''' </summary>
    ''' <param name="PacketArraySmallString">пакет байт по ссылке</param>
    ''' <param name="SSDName">имя клиента</param>
    ''' <param name="SSDdescription">описание клиента</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SendSSDDescription(ByRef packetArraySmallString As List(Of Byte), SSDName As String, SSDdescription As String) As Byte()
        Dim offset As Integer
        'Dim subpacketSize As Integer = 0
        packetArraySmallString.Clear()

        If SSDName IsNot Nothing AndAlso SSDdescription IsNot Nothing Then
            Dim hash As UInteger = NameToHash(SSDName)

            If CBool(hash) Then
                ChannelPacket(packetArraySmallString, hash, CommandSetServer.Description_54, 0, Nothing, SSDdescription, 0, offset)
            End If
        End If

        Return packetArraySmallString.ToArray()
    End Function

    ''' <summary>
    ''' Функция формирует пакет.
    ''' </summary>
    ''' <param name="hash"></param>
    ''' <param name="dataType"></param>
    ''' <param name="time"></param>
    ''' <param name="value"></param>
    ''' <param name="_string"></param>
    ''' <param name="numberOfHashes"></param>
    ''' <param name="offset"></param>
    ''' <remarks></remarks>
    Public Sub ChannelPacket(ByRef PacketArray As List(Of Byte),
                             hash As UInt32,
                             dataType As CommandSetServer,
                             time As UInt32,
                             value() As Byte,
                             _string As String,
                             numberOfHashes As UShort,
                             ByRef offset As Integer)
        Dim sLen As Byte
        Dim aByteCommand() As Byte = BitConverter.GetBytes(dataType)

        ' Номер команды
        PacketArray.Add(aByteCommand(0))
        offset += 1
        ' Hash канала
        PacketArray.AddRange(BitConverter.GetBytes(hash))
        offset += 4
        ' Код ошибки всегда равно 0
        PacketArray.Add(New Byte)
        offset += 1
        ' Время пакета
        PacketArray.AddRange(BitConverter.GetBytes(time))
        offset += 4

        Select Case dataType
            Case CommandSetServer.Byte_1 ' Пакет byte  1 byte
                PacketArray.Add(value(0))
                offset += 1
                Exit Select
            Case CommandSetServer.Integer_2 ' Пакет integer  4 byte
                PacketArray.AddRange(value)
                offset += 4
                Exit Select
            Case CommandSetServer.Double_3 ' Пакет double 8 byte
                PacketArray.AddRange(value)
                offset += 8
                Exit Select
            Case CommandSetServer.SmallString_51 ' Пакет Small string
                sLen = CByte(_string.Length)
                PacketArray.Add(sLen)
                offset += 1

                'PacketArray.AddRange(System.Text.Encoding.ASCII.GetBytes(_string))
                PacketArray.AddRange(ConvertUnicodeToWindows1251Byte(_string))
                offset += sLen
                Exit Select
            Case CommandSetServer.Description_54 ' Пакет Description
                sLen = CByte(_string.Length)
                PacketArray.Add(sLen)
                offset += 1

                'PacketArray.AddRange(System.Text.Encoding.ASCII.GetBytes(_string))
                PacketArray.AddRange(ConvertUnicodeToWindows1251Byte(_string))
                offset += sLen
                Exit Select
            Case CommandSetServer.GetData_100 ' Пакет Get data
                PacketArray.AddRange(BitConverter.GetBytes(numberOfHashes))
                offset += 2

                Dim arrByteTemp(3) As Byte
                For I As Integer = 0 To numberOfHashes - 1
                    Array.Copy(value, I * 4, arrByteTemp, 0, 4)
                    PacketArray.AddRange(arrByteTemp)
                    offset += 4
                Next

                Exit Select
            Case Else
                Exit Select
        End Select
    End Sub

    Private ReadOnly u8 As System.Text.Encoding = Text.Encoding.UTF8
    'Тест: имя - TIME001 результат - 148996177
    ''' <summary>
    ''' Получить Хэш сумму из имени канала.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NameToHash(name As String) As UInt32
        If name.Length = 0 Then Return 0

        Const c As UInt32 = CType(&HF0000000UI, UInt32)
        'Const c As UInt32 = 4026531840 ' тот же результат
        Dim prev As UInt32 = 0
        Dim var1, var2 As UInt32
        Dim _sizeOf As Integer = Len(prev) * 8
        Dim bytes As Byte() = u8.GetBytes(name.ToUpper)

        For I As Integer = 0 To bytes.Length - 1
            var1 = bytes(I) + (prev << 4) + (prev >> _sizeOf - 4)
            var2 = var1 And c
            If var2 <> 0 Then
                prev = (Not c) And (((var2 >> 24) + (var2 << _sizeOf - 24)) Xor var1)
            Else
                prev = var1
            End If
        Next

        Return prev
    End Function

    ''Тест: имя - TIME001 результат - 148996177
    ' ''' <summary>
    ' ''' Получить Хэш сумму из имени канала
    ' ''' </summary>
    ' ''' <param name="Name"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function Name_To_Hash(Name As String) As UInt32
    '    If Name.Length = 0 Then Return 0

    '    Const c As UInt32 = CType(&HF0000000UI, UInt32)
    '    'Const c As UInt32 = 4026531840 ' тот же результат
    '    Dim prev As UInt32 = 0
    '    Dim var1, var2 As UInt32
    '    Dim _sizeOf As Integer = Len(prev) * 8
    '    Dim u8 As System.Text.Encoding = Text.Encoding.UTF8
    '    Dim bytes As Byte() = u8.GetBytes(Name.ToUpper)

    '    For i As Integer = 0 To bytes.Length - 1
    '        var1 = bytes(i) + (prev << 4) + (prev >> _sizeOf - 4)
    '        var2 = var1 And c
    '        If var2 <> 0 Then
    '            prev = (Not c) And (((var2 >> 24) + (var2 << _sizeOf - 24)) Xor var1)
    '        Else
    '            prev = var1
    '        End If
    '    Next

    '    Return prev
    'End Function

    ''' <summary>
    ''' Получить представление строки в байтах
    ''' </summary>
    ''' <param name="unicodeString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertUnicodeToWindows1251Byte(unicodeString As String) As Byte()
        ' Все строчки в .net хранятся в юникоде. 
        ' Надо взять строку, и с помощью метода GetBytes нужной кодировки получить представление строки в байтах для данной кодировки. 
        ' Дальше берется этот массив байт и преобразуется в строку с помощью Encoding.GetString.
        ' Вызывать метод той кодировки, к которой необходимо преобразовать строку.
        ' Создать 2 различные кодировки.
        ' Encoding.GetEncoding("cp866")'Encoding.UTF7 ' Encoding.UTF8 'Encoding.ASCII 'Encoding.GetEncoding("koi8-r")'As New UTF8Encoding
        Dim asciiEncoding As Text.Encoding = System.Text.Encoding.GetEncoding("windows-1251")
        Dim unicodeEncoding As Text.Encoding = System.Text.Encoding.Unicode 'As New UnicodeEncoding()
        ' Конвертировать строку в массив byte[].
        Dim unicodeBytes As Byte() = unicodeEncoding.GetBytes(unicodeString)
        ' Произвести конвертацию из одной кодировки в другую.
        Dim asciiBytes As Byte() = System.Text.Encoding.Convert(unicodeEncoding, asciiEncoding, unicodeBytes)
        ' Конвертировать новый byte[] в char[] а затем в строку.
        ' Немного другой подход конвертации  GetCharCount/GetChars.
        Dim asciiChars(asciiEncoding.GetCharCount(asciiBytes, 0, asciiBytes.Length) - 1) As Char
        asciiEncoding.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)

        'Dim asciiString As New String(asciiChars)

        ' Тест показа строк до и после конвертации, чтобы показать, что обратная конвертация правильна.
        'Console.WriteLine("Original string: {0}", unicodeString)
        'Console.WriteLine("Ascii converted string: {0}", asciiString)

        ' Записывать надо байты, а не строку
        'System.IO.File.WriteAllBytes(FileCfg, asciiBytes)

        'System.IO.File.WriteAllText(FileCfg, asciiString)
        'Dim sNewText As String = StrConv(asciiString, VbStrConv.Lowercase)
        'Return asciiString
        Return asciiBytes
    End Function
End Module
