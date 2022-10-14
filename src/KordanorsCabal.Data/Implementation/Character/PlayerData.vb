Public Class PlayerData
    Inherits BaseData
    Implements IPlayerData
    Friend Const TableName = "Players"
    Friend Const PlayerIdColumn = "PlayerId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const DirectionIdColumn = "DirectionId"
    Friend Const PlayerModeIdColumn = "PlayerModeId"
    Const FixedPlayerId = 1L

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.Primitive.Execute(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{PlayerIdColumn}] INT NOT NULL UNIQUE CHECK([{PlayerIdColumn}]={FixedPlayerId}),
                [{CharacterIdColumn}] INT NOT NULL,
                [{DirectionIdColumn}] INT NOT NULL,
                [{PlayerModeIdColumn}] INT NOT NULL,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub WriteDirection(direction As Long) Implements IPlayerData.WriteDirection
        Store.Column.Write(
            AddressOf Initialize,
            TableName,
            (DirectionIdColumn, direction),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadPlayerMode() As Long? Implements IPlayerData.ReadPlayerMode
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName, PlayerModeIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub WritePlayerMode(mode As Long) Implements IPlayerData.WritePlayerMode
        Store.Column.Write(
            AddressOf Initialize,
            TableName,
            (PlayerModeIdColumn, mode),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadDirection() As Long? Implements IPlayerData.ReadDirection
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DirectionIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Function Read() As Long? Implements IPlayerData.Read
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long, direction As Long, mode As Long) Implements IPlayerData.Write
        Store.Replace.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (PlayerIdColumn, FixedPlayerId),
            (CharacterIdColumn, characterId),
            (DirectionIdColumn, direction),
            (PlayerModeIdColumn, mode))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements IPlayerData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
