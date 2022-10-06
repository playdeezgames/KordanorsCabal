Public MustInherit Class NameCacheData
    Inherits BaseData
    Implements INameCacheData
    Protected Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Private ReadOnly nameLookUp As New Dictionary(Of String, Long)
    Protected lookUpByName As Func(Of String, Long?)
    Public Function ReadForName(name As String) As Long? Implements INameCacheData.ReadForName
        Dim candidate As Long = 0
        If nameLookUp.TryGetValue(name, candidate) Then
            Return candidate
        End If
        Dim result = lookUpByName(name)
        If result.HasValue Then
            nameLookUp(name) = result.Value
        End If
        Return result
    End Function
End Class
