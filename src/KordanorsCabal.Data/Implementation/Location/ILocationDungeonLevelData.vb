Public Interface ILocationDungeonLevelData
    Function Read(locationId As Long) As Long?
    Sub Write(locationId As Long, dungeonLevelId As Long)
    Function ReadForDungeonLevel(dungeonLevelId As Long) As IEnumerable(Of Long)
End Interface
