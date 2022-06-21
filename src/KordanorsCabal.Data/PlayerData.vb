Public Module PlayerData
    Friend Const TableName = "Players"
    Friend Const PlayerIdColumn = "PlayerId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const DirectionColumn = "Direction"
    Const FixedPlayerId = 1
    Friend Sub Initialize()
        CharacterData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{PlayerIdColumn}] INT NOT NULL UNIQUE CHECK([{PlayerIdColumn}]={FixedPlayerId}),
                [{CharacterIdColumn}] INT NOT NULL,
                [{DirectionColumn}] INT NOT NULL,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Function ReadDirection() As Long?
        Return ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, DirectionColumn, (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Function Read() As Long?
        Return ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, CharacterIdColumn, (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long, direction As Long)
        ReplaceRecord(AddressOf Initialize, TableName, (PlayerIdColumn, FixedPlayerId), (CharacterIdColumn, characterId), (DirectionColumn, direction))
    End Sub
End Module
