Imports System.Text

Public Class Kanamozic
    Public Const ConvertEngLetters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
    Public Const ConvertJpnLetters As String = "あいうえおかきくけこさしすせそたちつてとなにぬねのはアイウエオカキクケコサシスセソタチツテトナニヌネノハんをわろれるりらよゆンワ"
    Public Const FullJpnLetters As String = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン"
    Private Shared Function IsOnlyKanas(s As String) As Boolean
        For Each i In s
            If Not FullJpnLetters.Contains(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function Encode(s As String, key As SByte) As String
        If IsOnlyKanas(s) Then
            Return "が" & EncodeKana(s, key)
        Else
            Return "ぎ" & EncodeBinary(s, key)
        End If
    End Function
    Private Shared Function EncodeKana(s As String, key As SByte) As String
        Dim buf = New Char(s.Length - 1) {}
        For i = 0 To s.Length - 1
            Dim value As Char = s(i)
            Dim offset = FullJpnLetters.IndexOf(value)
            Dim requiredOffset = (offset + key) Mod FullJpnLetters.Length
            buf(i) = FullJpnLetters(requiredOffset)
        Next
        Return buf
    End Function
    Private Shared Function EncodeBinary(s As String, key As SByte) As String
        s = EncodeBase64(s)
        Dim buf = New Char(s.Length - 1) {}
        For i = 0 To s.Length - 1
            Dim value As Char = s(i)
            Dim offset = ConvertEngLetters.IndexOf(value)
            Dim requiredOffset = (offset + key) Mod ConvertJpnLetters.Length
            buf(i) = ConvertJpnLetters(requiredOffset)
        Next
        Return buf
    End Function

    Public Shared Function Decode(s As String, key As SByte) As String
        If s(0) = "が"c Then
            Return DecodeKana(s.Substring(1), key)
        ElseIf s(0) = "ぎ"c Then
            Return DecodeBinary(s.Substring(1), key)
        Else
            Throw New ArgumentException(s + " is incorrect format. The string must start with 'が'(ga) or 'ぎ'(gi).")
        End If
    End Function
    Private Shared Function DecodeKana(s As String, key As SByte) As String
        Dim buf = New Char(s.Length - 1) {}
        For i = 0 To s.Length - 1
            Dim value As Char = s(i)
            Dim offset = FullJpnLetters.IndexOf(value)
            Dim requiredOffset = IncreaseIfNegative(offset - key, FullJpnLetters.Length) Mod FullJpnLetters.Length
            buf(i) = FullJpnLetters(requiredOffset)
        Next
        Return buf
    End Function
    Private Shared Function DecodeBinary(s As String, key As SByte) As String
        Dim buf = New Char(s.Length - 1) {}
        For i = 0 To s.Length - 1
            Dim value As Char = s(i)
            Dim offset = ConvertJpnLetters.IndexOf(value)
            Dim requiredOffset = IncreaseIfNegative(offset - key, ConvertEngLetters.Length) Mod ConvertEngLetters.Length
            buf(i) = ConvertEngLetters(requiredOffset)
        Next
        Return DecodeBase64(buf)
    End Function

    Private Shared Function IncreaseIfNegative(value As Integer, once As Integer) As Integer
        While value < 0
            value += once
        End While
        Return value
    End Function

    Private Shared Function ToByteArray(s As String) As Byte()
        Return Encoding.UTF8.GetBytes(s)
    End Function
    Private Shared Function ToString(b As Byte()) As String
        Return Encoding.UTF8.GetString(b)
    End Function

    Private Shared Function EncodeBase64(d As String) As String
        Return Convert.ToBase64String(ToByteArray(d), Base64FormattingOptions.None).TrimEnd("=")
    End Function
    Private Shared Function DecodeBase64(d As String) As String
        Return ToString(Convert.FromBase64String(d))
    End Function
End Class
