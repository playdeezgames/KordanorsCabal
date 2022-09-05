Public Class CharacterLocationDataTests
    Inherits WorldDataSubobjectTests
    <Fact>
    Sub ShouldClearLocationsForACharacter()
        WithSubobject(
            Function(x) x.CharacterLocation,
            Sub(store, subject)
                Const characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterLocations,
                                 (Columns.CharacterId, characterId)))
            End Sub)
    End Sub
End Class
