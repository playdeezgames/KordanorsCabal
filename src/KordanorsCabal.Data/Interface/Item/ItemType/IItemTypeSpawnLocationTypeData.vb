Public Interface IItemTypeSpawnLocationTypeData
    Function ReadAll(itemTypeId As Long, dungeonLevelId As Long) As IEnumerable(Of Long)
End Interface
