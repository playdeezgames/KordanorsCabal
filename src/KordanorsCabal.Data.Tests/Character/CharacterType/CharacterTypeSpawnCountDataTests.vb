﻿Public Class CharacterTypeSpawnCountDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeSpawnCountData)
    Sub New()
        MyBase.New(Function(x) x.CharacterTypeSpawnCount)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForASpawnCountOfAParticularCharacterTypeOnAParticularDungeonLevel()
        WithSubobject(
            Sub(store, subject)
                Dim characterTypeId = 1L
                Dim dungeonLevelId = 2L
                subject.ReadSpawnCount(characterTypeId, dungeonLevelId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.CharacterTypeSpawnCounts,
                    Columns.SpawnCountColumn,
                    (Columns.CharacterTypeIdColumn, characterTypeId),
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class