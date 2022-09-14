Public Class CharacterLocationDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterLocationData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterLocation)
    End Sub

    <Fact>
    Sub ShouldClearLocationsForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterLocations,
                                 (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForACharacterAndLocationCombination()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const locationId = 2L
                subject.Read(characterId, locationId).ShouldBeFalse
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                                It.IsAny(Of Action),
                                Tables.CharacterLocations,
                                CharacterIdColumn,
                                (CharacterIdColumn, characterId),
                                (LocationIdColumn, locationId)),
                                Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreSoThatACharacterIsAssociatedWithALocation()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const locationId = 2L
                subject.Write(characterId, locationId)
                store.Verify(Sub(x) x.ReplaceRecord(Of Long, Long)(
                                It.IsAny(Of Action),
                                Tables.CharacterLocations,
                                (CharacterIdColumn, characterId),
                                (LocationIdColumn, locationId)),
                                Times.Once)
            End Sub)
    End Sub
End Class
