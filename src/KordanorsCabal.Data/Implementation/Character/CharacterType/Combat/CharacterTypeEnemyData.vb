Public Class CharacterTypeEnemyData
    Inherits BaseData
    Implements ICharacterTypeEnemyData
    Friend Const TableName = "CharacterTypeEnemies"
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const EnemyCharacterTypeIdColumn = "EnemyCharacterTypeId"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, enemyCharacterTypeId As Long) As Boolean Implements ICharacterTypeEnemyData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            EnemyCharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (EnemyCharacterTypeIdColumn, enemyCharacterTypeId)).HasValue
    End Function
End Class
