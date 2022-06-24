Public Module LocationInventoryData
    Friend Const TableName = "LocationInventories"
    Friend Const LocationIdColumn = LocationData.LocationIdColumn
    Friend Const InventoryIdColumn = InventoryData.InventoryIdColumn
    Friend Sub Initialize()
        LocationData.Initialize()
        InventoryData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{LocationIdColumn}] INT NOT NULL UNIQUE,
                [{InventoryIdColumn}] INT NOT NULL,
                FOREIGN KEY([{LocationIdColumn}]) REFERENCES [{LocationData.TableName}]([{LocationData.LocationIdColumn}]),
                FOREIGN KEY([{InventoryIdColumn}]) REFERENCES [{InventoryData.TableName}]([{InventoryData.InventoryIdColumn}])
            );")
    End Sub
    Public Function Read(locationId As Long) As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            InventoryIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Write(locationId As Long, inventoryId As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (LocationIdColumn, locationId),
            (InventoryIdColumn, inventoryId))
    End Sub
End Module
