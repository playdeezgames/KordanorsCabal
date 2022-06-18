Public Module PlayerData
    Friend Const TableName = "Players"
    Friend Const PlayerIdColumn = "PlayerId"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Const FixedPlayerId = 1
    Friend Sub Initialize()
        CharacterData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{PlayerIdColumn}] INT NOT NULL CHECK([{PlayerIdColumn}]={FixedPlayerId}),
                [{CharacterIdColumn}] INT NOT NULL,
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Public Function Read() As Long?
        Return ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, CharacterIdColumn, (PlayerIdColumn, FixedPlayerId))
    End Function

    Public Sub Write(characterId As Long)
        ClearForColumnValue(AddressOf Initialize, TableName, (PlayerIdColumn, FixedPlayerId))
        CreateRecord(AddressOf Initialize, TableName, (PlayerIdColumn, FixedPlayerId), (CharacterIdColumn, characterId))
    End Sub
End Module
