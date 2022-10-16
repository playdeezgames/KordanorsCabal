Public Class CharacterTypeAttackTypeData
    Inherits BaseData
    Implements ICharacterTypeAttackTypeData
    Friend Const TableName = "CharacterTypeAttackTypes"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const AttackTypeColumn = "AttackType"
    Friend Const WeightColumn = "Weight"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer) Implements ICharacterTypeAttackTypeData.Read
        Dim results = Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            (AttackTypeColumn, WeightColumn),
            (CharacterTypeIdColumn, characterTypeId))
        If results Is Nothing Then
            Return Nothing
        End If
        Return results.ToDictionary(
                Function(x) x.Item1,
                Function(x) CInt(x.Item2))
    End Function
End Class
