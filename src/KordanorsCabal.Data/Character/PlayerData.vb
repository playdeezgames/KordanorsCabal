Public Module PlayerData
    Friend Const TableName = "Players"
    Friend Const PlayerIdColumn = "PlayerId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const DirectionColumn = "Direction"
    Friend Const ModeColumn = "Mode"
    Const FixedPlayerId = 1
    Friend Sub Initialize()
        CharacterData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{PlayerIdColumn}] INT NOT NULL UNIQUE CHECK([{PlayerIdColumn}]={FixedPlayerId}),
                [{CharacterIdColumn}] INT NOT NULL,
                [{DirectionColumn}] INT NOT NULL,
                [{ModeColumn}] INT NOT NULL,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub WriteDirection(direction As Long)
        WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (DirectionColumn, direction),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadMode() As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName, ModeColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub WriteMode(mode As Long)
        WriteColumnValue(
            AddressOf Initialize,
            TableName,
            (ModeColumn, mode),
            (PlayerIdColumn, FixedPlayerId))
    End Sub

    Public Function ReadDirection() As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DirectionColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Function Read() As Long?
        Return ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long, direction As Long, mode As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (PlayerIdColumn, FixedPlayerId),
            (CharacterIdColumn, characterId),
            (DirectionColumn, direction),
            (ModeColumn, mode))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Module
