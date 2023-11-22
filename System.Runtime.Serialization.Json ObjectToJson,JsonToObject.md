##System.Runtime.Serialization.Json.DataContractJsonSerializer


###แปลง Object เป็น JsonString

```
Using ms As New System.IO.MemoryStream()
    Dim jsonSerializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(inVo.GetType())
    jsonSerializer.WriteObject(ms, inVo)
    ms.Position = 0
    Dim buff(CInt(ms.Length)) As Byte
    ms.Read(buff, 0, buff.Length)
    Dim jsonStr As String = System.Text.UTF8Encoding.UTF8.GetString(buff)
    ms.Close()
End Using
```


###แปลง Json String ให้เป็น Object
```
Public Shared Function JsonStringTo(Of T)(ByVal jsonStr As String) As T
    If String.IsNullOrEmpty(jsonStr) Then Return Nothing

    Using ms As New System.IO.MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(jsonStr))
        Dim jsonSerializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(GetType(T))
        Return DirectCast(jsonSerializer.ReadObject(ms), T)
        ms.Close()
        End Using
End Function
```