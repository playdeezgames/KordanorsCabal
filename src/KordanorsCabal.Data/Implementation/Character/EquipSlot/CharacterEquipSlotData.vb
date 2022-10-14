Public Class CharacterEquipSlotData
    Inherits BaseData
    Implements ICharacterEquipSlotData
    Friend Const TableName = "CharacterEquipSlots"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const EquipSlotColumn = "EquipSlot"
    Friend Const ItemIdColumn = ItemData.ItemIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        CType(World.Item, ItemData).Initialize()
        Store.Primitive.ExecuteNonQuery(
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
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (EquipSlotColumn, ItemIdColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Sub Clear(characterId As Long, equipSlot As Long) Implements ICharacterEquipSlotData.Clear
        Store.Clear.ClearForColumnValues(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (EquipSlotColumn, equipSlot))
    End Sub

    Public Sub Write(characterId As Long, equipSlot As Long, itemId As Long) Implements ICharacterEquipSlotData.Write
        Store.Replace.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (EquipSlotColumn, equipSlot),
            (ItemIdColumn, itemId))
    End Sub

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterEquipSlotData.ClearForCharacter
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub

    Public Sub ClearForItem(itemId As Long) Implements ICharacterEquipSlotData.ClearForItem
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadForCharacterEquipSlot(characterId As Long, equipSlotId As Long) As Long? Implements ICharacterEquipSlotData.ReadForCharacterEquipSlot
        Return Store.Column.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ItemIdColumn,
            (CharacterIdColumn, characterId),
            (EquipSlotColumn, equipSlotId))
    End Function
    Public Function ReadItemsForCharacter(characterId As Long) As IEnumerable(Of Long) Implements ICharacterEquipSlotData.ReadItemsForCharacter
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long)(AddressOf Initialize, TableName, ItemIdColumn, (CharacterIdColumn, characterId))
    End Function
    Public Function ReadEquipSlotsForCharacter(characterId As Long) As IEnumerable(Of Long) Implements ICharacterEquipSlotData.ReadEquipSlotsForCharacter
        Return Store.Record.ReadRecordsWithColumnValue(Of Long, Long)(AddressOf Initialize, TableName, EquipSlotColumn, (CharacterIdColumn, characterId))
    End Function
End Class
