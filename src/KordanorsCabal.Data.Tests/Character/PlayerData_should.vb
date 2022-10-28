Public Class PlayerData_should
    Inherits WorldDataSubobjectTests(Of IPlayerData)
    Sub New()
        MyBase.New(Function(x) x.Player)
    End Sub
    <Fact>
    Sub clear_for_character()
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
    Sub have_character()
        WithSubobject(
            Sub(store, checker, subject)
                Const playerId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read().ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.CharacterIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_direction()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadDirection().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.DirectionIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_mode()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadPlayerMode().ShouldBeNull
                Dim playerId = 1
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.PlayerModeIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_shoppe_type()
        WithSubobject(
            Sub(store, checker, subject)
                Const playerId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadShoppeType().ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    Columns.ShoppeTypeIdColumn,
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub set_mode_and_direction_and_shoppe_type()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const direction = 2L
                Const mode = 3L
                Const shoppeType = 4L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, direction, mode, shoppeType)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Replace.Entry(Of Long, Long, Long, Long?, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.PlayerIdColumn, playerId),
                    (Columns.CharacterIdColumn, characterId),
                    (Columns.DirectionIdColumn, direction),
                    (Columns.ShoppeTypeIdColumn, shoppeType),
                    (Columns.PlayerModeIdColumn, mode)))
            End Sub)
    End Sub
    <Fact>
    Sub set_direction()
        WithSubobject(
            Sub(store, checker, subject)
                Dim direction = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteDirection(direction)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Column.Write(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.DirectionIdColumn, direction),
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub set_mode()
        WithSubobject(
            Sub(store, checker, subject)
                Dim mode = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WritePlayerMode(mode)
                Dim playerId = 1
                store.Verify(
                    Sub(x) x.Column.Write(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Players,
                    (Columns.PlayerModeIdColumn, mode),
                    (Columns.PlayerIdColumn, playerId)))
            End Sub)
    End Sub
    <Fact>
    Sub set_shoppe_type()
        WithSubobject(
            Sub(store, checker, subject)
                Const playerId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                Dim shoppeType = 2L
                subject.WriteShoppeType(shoppeType)
                store.Verify(
                    Sub(x) x.Column.Write(Of Long, Long?)(
                        It.IsAny(Of Action),
                        Tables.Players,
                        (ShoppeTypeIdColumn, shoppeType),
                        (PlayerIdColumn, playerId)))
            End Sub)
    End Sub
End Class
