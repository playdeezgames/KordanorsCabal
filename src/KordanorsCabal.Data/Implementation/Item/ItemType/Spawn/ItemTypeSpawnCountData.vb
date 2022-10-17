Public Class ItemTypeSpawnCountData
    Inherits BaseData
    Implements IItemTypeSpawnCountData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, dungeonLevelId As Long) As String Implements IItemTypeSpawnCountData.Read
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            ItemTypeSpawnCounts,
            SpawnDiceColumn,
            (ItemTypeIdColumn, itemTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
