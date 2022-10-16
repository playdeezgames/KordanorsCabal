Friend Class ItemTypeCharacterStatisticBuffData
    Inherits BaseData
    Implements IItemTypeCharacterStatisticBuffData
    Friend Const TableName = "ItemTypeCharacterStatisticBuffs"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const CharacterStatisticTypeIdColumn = CharacterStatisticTypeData.CharacterStatisticTypeIdColumn
    Friend Const BuffColumn = "Buff"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, characterStatisticTypeId As Long) As Long? Implements IItemTypeCharacterStatisticBuffData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            BuffColumn,
            (ItemTypeIdColumn, itemTypeId),
            (CharacterStatisticTypeIdColumn, characterStatisticTypeId))
    End Function
End Class
