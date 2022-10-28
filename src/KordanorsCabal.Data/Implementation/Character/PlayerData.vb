Public Class PlayerData
    Inherits BaseData
    Implements IPlayerData
    Const FixedPlayerId = 1L

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub WriteDirection(direction As Long) Implements IPlayerData.WriteDirection
        Store.Column.Write(
            AddressOf NoInitializer,
            Players,
            (DirectionIdColumn, direction),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadPlayerMode() As Long? Implements IPlayerData.ReadPlayerMode
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Players, PlayerModeIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub WritePlayerMode(mode As Long) Implements IPlayerData.WritePlayerMode
        Store.Column.Write(
            AddressOf NoInitializer,
            Players,
            (PlayerModeIdColumn, mode),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadDirection() As Long? Implements IPlayerData.ReadDirection
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Players,
            DirectionIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Function Read() As Long? Implements IPlayerData.Read
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Players,
            CharacterIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long, direction As Long, mode As Long, shoppeType As Long?) Implements IPlayerData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            Players,
            (PlayerIdColumn, FixedPlayerId),
            (CharacterIdColumn, characterId),
            (DirectionIdColumn, direction),
            (ShoppeTypeIdColumn, shoppeType),
            (PlayerModeIdColumn, mode))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements IPlayerData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            Players,
            (CharacterIdColumn, characterId))
    End Sub

    Public Function ReadShoppeType() As Long? Implements IPlayerData.ReadShoppeType
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            Players,
            ShoppeTypeIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub WriteShoppeType(shoppeTypeId As Long?) Implements IPlayerData.WriteShoppeType
        Store.Column.Write(
            AddressOf NoInitializer,
            Players,
            (ShoppeTypeIdColumn, shoppeTypeId),
            (PlayerIdColumn, FixedPlayerId))
    End Sub
End Class
