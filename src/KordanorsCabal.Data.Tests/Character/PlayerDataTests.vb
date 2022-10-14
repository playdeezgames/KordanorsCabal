Public Class PlayerDataTests
    Inherits WorldDataSubobjectTests(Of IPlayerData)
    Sub New()
        MyBase.New(Function(x) x.Player)
    End Sub
    <Fact>
    Sub ShouldClearTheStoreOfPlayerDataAssociatedWithAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(
                    Sub(x) x.Clear.ForValue(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedCharacterOfThePlayer()
        WithSubobject(
            Sub(store, checker, subject)
                Const playerId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read().ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.CharacterIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedDirectionOfThePlayer()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadDirection().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.DirectionIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAssociatedModeOfThePlayer()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadPlayerMode().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.PlayerModeIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithPlayerInformation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim direction = 2L
                Dim mode = 3L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, direction, mode)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Replace.ReplaceRecord(Of Long, Long, Long, Long)(
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
            Sub(store, checker, subject)
                Dim direction = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteDirection(direction)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Column.WriteColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.DirectionIdColumn, direction),
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithAModeForThePlayer()
        WithSubobject(
            Sub(store, checker, subject)
                Dim mode = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WritePlayerMode(mode)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Column.WriteColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.PlayerModeIdColumn, mode),
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
End Class
