Public Class ItemTypeSpawnCountData
    Inherits BaseData
    Implements IItemTypeSpawnCountData
    Friend Const TableName = "ItemTypeSpawnCounts"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const SpawnDiceColumn = "SpawnDice"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, dungeonLevelId As Long) As String Implements IItemTypeSpawnCountData.Read
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            SpawnDiceColumn,
            (ItemTypeIdColumn, itemTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
