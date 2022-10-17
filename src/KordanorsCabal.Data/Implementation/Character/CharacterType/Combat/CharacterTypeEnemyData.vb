Public Class CharacterTypeEnemyData
    Inherits BaseData
    Implements ICharacterTypeEnemyData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, enemyCharacterTypeId As Long) As Boolean Implements ICharacterTypeEnemyData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeEnemies,
            EnemyCharacterTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (EnemyCharacterTypeIdColumn, enemyCharacterTypeId)).HasValue
    End Function
End Class
