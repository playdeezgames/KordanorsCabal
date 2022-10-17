Friend Class ItemTypeCharacterStatisticBuffData
    Inherits BaseData
    Implements IItemTypeCharacterStatisticBuffData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, characterStatisticTypeId As Long) As Long? Implements IItemTypeCharacterStatisticBuffData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeCharacterStatisticBuffs,
            BuffColumn,
            (ItemTypeIdColumn, itemTypeId),
            (CharacterStatisticTypeIdColumn, characterStatisticTypeId))
    End Function
End Class
