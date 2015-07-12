Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Kanamozic
'The result is from http://nao20010128nao.github.io/kanamozic
<TestClass()>
Public Class UnitTest1

    <TestMethod()>
    Public Sub 日本語EncodeTest()
        If Kanamozic.Kanamozic.Encode("日本語", 0) <> "ぎるタオシるタノテりさチオ" Then
            Throw New Exception
        End If
    End Sub

    <TestMethod()>
    Public Sub ぎるタオシるタノテりさチオDecodeTest()
        If Kanamozic.Kanamozic.Decode("ぎるタオシるタノテりさチオ", 0) <> "日本語" Then
            Throw New Exception
        End If
    End Sub

End Class