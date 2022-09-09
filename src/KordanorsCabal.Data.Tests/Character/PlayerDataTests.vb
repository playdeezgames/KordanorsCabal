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
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedCharacterOfThePlayer()
        WithSubobject(
            Sub(store, subject)
                subject.Read().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.CharacterIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedDirectionOfThePlayer()
        WithSubobject(
            Sub(store, subject)
                subject.ReadDirection().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.DirectionIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedModeOfThePlayer()
        WithSubobject(
            Sub(store, subject)
                subject.ReadMode().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.ModeColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
End Class
