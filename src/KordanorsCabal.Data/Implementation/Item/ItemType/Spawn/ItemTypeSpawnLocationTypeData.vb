Public Class ItemTypeSpawnLocationTypeData
    Inherits BaseData
    Implements IItemTypeSpawnLocationTypeData
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const DungeonLevelIdColumn = DungeonLevelData.DungeonLevelIdColumn
    Friend Const LocationTypeIdColumn = LocationTypeData.LocationTypeIdColumn
    Public Function ReadAll(itemTypeId As Long, dungeonLevelId As Long) As IEnumerable(Of Long) Implements IItemTypeSpawnLocationTypeData.ReadAll
        Return Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeSpawnLocationTypes,
            LocationTypeIdColumn,
            (ItemTypeIdColumn, itemTypeId),
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
