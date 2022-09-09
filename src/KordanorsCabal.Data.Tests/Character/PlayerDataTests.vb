Public Class PlayerDataTests
    Inherits WorldDataSubobjectTests(Of IPlayerData)
    Sub New()
        MyBase.New(Function(x) x.Player)
    End Sub
    <Fact>
    Sub ShouldClearTheStoreOfPlayerDataAssociatedWithAGivenCharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(
                    Sub(x) x.ClearForColumnValue(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
End Class
