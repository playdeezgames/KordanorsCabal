Public Class CharacterTypeEnemyDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeEnemyData)
    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypeEnemy)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheEnemyStatusOfTwoCharacterTypes()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                Dim enemyCharacterType = 2L
                subject.Read(characterType, enemyCharacterType)
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeEnemies,
                                 Columns.EnemyCharacterTypeIdColumn,
                                 (Columns.CharacterTypeIdColumn, characterType),
                                 (Columns.EnemyCharacterTypeIdColumn, enemyCharacterType)))
            End Sub)
    End Sub
End Class
