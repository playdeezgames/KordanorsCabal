Public Module ItemData
    Friend Const TableName = "Items"
    Friend Const ItemIdColumn = "ItemId"
    Friend Const ItemTypeColumn = "ItemType"
    Friend Sub Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{ItemIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{ItemTypeColumn}] INT NOT NULL
            );")
    End Sub
    Public Function Create(itemType As Long) As Long
        Return CreateRecord(AddressOf Initialize, TableName, (ItemTypeColumn, itemType))
    End Function
End Module
