Public Class PlayerData
    Inherits BaseData
    Implements IPlayerData
    Friend Const TableName = "Players"
    Friend Const PlayerIdColumn = "PlayerId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const DirectionColumn = "Direction"
    Friend Const ModeColumn = "Mode"
    Const FixedPlayerId = 1

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{PlayerIdColumn}] INT NOT NULL UNIQUE CHECK([{PlayerIdColumn}]={FixedPlayerId}),
                [{CharacterIdColumn}] INT NOT NULL,
                [{DirectionColumn}] INT NOT NULL,
                [{ModeColumn}] INT NOT NULL,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub WriteDirection(direction As Long) Implements IPlayerData.WriteDirection
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (DirectionColumn, direction),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadMode() As Long? Implements IPlayerData.ReadMode
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName, ModeColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub WriteMode(mode As Long) Implements IPlayerData.WriteMode
        Store.WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (ModeColumn, mode),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadDirection() As Long? Implements IPlayerData.ReadDirection
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DirectionColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Function Read() As Long? Implements IPlayerData.Read
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long, direction As Long, mode As Long) Implements IPlayerData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (PlayerIdColumn, FixedPlayerId),
            (CharacterIdColumn, characterId),
            (DirectionColumn, direction),
            (ModeColumn, mode))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements IPlayerData.ClearForCharacter
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
