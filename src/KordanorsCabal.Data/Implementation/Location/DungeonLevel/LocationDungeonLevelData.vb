Public Class LocationDungeonLevelData
    Inherits BaseData
    Implements ILocationDungeonLevelData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function Read(locationId As Long) As Long? Implements ILocationDungeonLevelData.Read
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            LocationDungeonLevels,
            DungeonLevelIdColumn,
            (LocationIdColumn, locationId))
    End Function

    Public Sub Write(locationId As Long, dungeonLevel As Long) Implements ILocationDungeonLevelData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            LocationDungeonLevels,
            (LocationIdColumn, locationId),
            (DungeonLevelIdColumn, dungeonLevel))
    End Sub

    Public Function ReadForDungeonLevel(dungeonLevel As Long) As IEnumerable(Of Long) Implements ILocationDungeonLevelData.ReadForDungeonLevel
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            LocationDungeonLevels,
            LocationIdColumn,
            (DungeonLevelIdColumn, dungeonLevel))
    End Function
End Class
