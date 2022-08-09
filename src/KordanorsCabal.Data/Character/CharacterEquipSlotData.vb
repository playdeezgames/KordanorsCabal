Public Class CharacterEquipSlotData
    Inherits BaseData
    Friend Const TableName = "CharacterEquipSlots"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const EquipSlotColumn = "EquipSlot"
    Friend Const ItemIdColumn = ItemData.ItemIdColumn

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Friend Sub Initialize()
        StaticWorldData.World.Character.Initialize()
        StaticWorldData.World.Item.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{EquipSlotColumn}] INT NOT NULL,
                [{ItemIdColumn}] INT NOT NULL UNIQUE,
                UNIQUE([{CharacterIdColumn}],[{EquipSlotColumn}]),
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}]),
                FOREIGN KEY ([{ItemIdColumn}]) REFERENCES [{ItemData.TableName}]([{ItemData.ItemIdColumn}])
            );")
    End Sub
    Public Function Read(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return Store.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (EquipSlotColumn, ItemIdColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Sub Clear(characterId As Long, equipSlot As Long)
        Store.ClearForColumnValues(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (EquipSlotColumn, equipSlot))
    End Sub

    Public Sub Write(characterId As Long, equipSlot As Long, itemId As Long)
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (EquipSlotColumn, equipSlot),
            (ItemIdColumn, itemId))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub

    Friend Sub ClearForItem(itemId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub
End Class
