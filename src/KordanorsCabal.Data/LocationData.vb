Public Module LocationData
    Friend Const TableName = "Locations"
    Friend Const LocationIdColumn = "LocationId"
    Friend Sub Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT
            );")
    End Sub
    Public Function Create() As Long
        Return CreateRecord(AddressOf Initialize, TableName)
    End Function
End Module
