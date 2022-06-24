Public Module InventoryData
    Friend Const TableName = "Inventories"
    Friend Const InventoryIdColumn = "InventoryId"
    Friend Sub Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{InventoryIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT
            );")
    End Sub
    Public Function Create() As Long
        Return CreateRecord(AddressOf Initialize, TableName)
    End Function
End Module
