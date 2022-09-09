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
                subject.ReadPlayerMode().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.PlayerModeIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithPlayerInformation()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                Dim direction = 2L
                Dim mode = 3L
                subject.Write(characterId, direction, mode)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.ReplaceRecord(Of Long, Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.PlayerIdColumn, playerId),
                    (Columns.CharacterIdColumn, characterId),
                    (Columns.DirectionIdColumn, direction),
                    (Columns.PlayerModeIdColumn, mode)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithADirectionForThePlayer()
        WithSubobject(
            Sub(store, subject)
                Dim direction = 2L
                subject.WriteDirection(direction)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.WriteColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.DirectionIdColumn, direction),
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
End Class
