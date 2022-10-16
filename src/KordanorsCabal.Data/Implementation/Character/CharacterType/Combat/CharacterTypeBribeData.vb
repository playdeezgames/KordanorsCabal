Public Class CharacterTypeBribeData
    Inherits BaseData
    Implements ICharacterTypeBribeData
    Friend Const TableName = "CharacterTypeBribes"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, itemType As Long) As Boolean Implements ICharacterTypeBribeData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            ItemTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (ItemTypeIdColumn, itemType)).HasValue
    End Function
End Class
