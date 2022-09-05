Public Class CharacterSpellDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterSpellData)

    Sub New()
        MyBase.New(Function(x) x.CharacterSpell)
    End Sub
    <Fact>
    Sub ShouldRemoveAllSpellLevelsForACharacterFromTheStore()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 CharacterSpells,
                                 (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
End Class
