Public Class CharacterTypeLootData
    Inherits BaseData
    Implements ICharacterTypeLootData
    Friend Const TableName = "CharacterTypeLoots"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const WeightColumn = "Weight"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer) Implements ICharacterTypeLootData.Read
        Dim results = Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            (ItemTypeIdColumn, WeightColumn),
            (CharacterTypeIdColumn, characterTypeId))
        If results Is Nothing Then
            Return Nothing
        End If
        Return results.ToDictionary(
                Function(x) x.Item1,
                Function(x) CInt(x.Item2))
    End Function
End Class
