Public Class Kanamozic
    Public Const ConvertEngLetters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
    Public Const ConvertJpnLetters As String = "あいうえおかきくけこさしすせそたちつてとなにぬねのはアイウエオカキクケコサシスセソタチツテトナニヌネノハんをわろれるりらよゆンワ"
    Public Const FullJpnLetters As String = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン"
    Public Shared Function IsOnlyKanas(s As String) As Boolean
        For Each i In s
            If Not FullJpnLetters.Contains(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function Encode(s As String, key As SByte) As String
        If IsOnlyKanas(s) Then
            Return "が"
        Else
            Return "ぎ"
        End If
    End Function
    Private Shared Function EncodeKana(s As String, key As SByte) As String
        Dim buf = New Char(s.Length) {}
        For i = 0 To s.Length - 1
            Dim value As Char = s(i)
            Dim offset = FullJpnLetters.IndexOf(value)
            Dim requiredOffset = (offset + key) Mod FullJpnLetters.Length
            buf(i) = FullJpnLetters(requiredOffset)
        Next
        Return buf
    End Function

End Class
