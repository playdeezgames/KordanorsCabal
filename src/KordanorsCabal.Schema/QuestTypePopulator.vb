Module QuestTypePopulator
    Private Sub PopulateQuestTypesRecord(connection As SqliteConnection, QuestTypeId As Long, CanAcceptEventName As String, AcceptEventName As String, CanCompleteEventName As String, CompleteEventName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.QuestTypes}]([{Columns.QuestTypeIdColumn}], [{Columns.CanAcceptEventNameColumn}], [{Columns.AcceptEventNameColumn}], [{Columns.CanCompleteEventNameColumn}], [{Columns.CompleteEventNameColumn}]) VALUES (@{Columns.QuestTypeIdColumn}, @{Columns.CanAcceptEventNameColumn}, @{Columns.AcceptEventNameColumn}, @{Columns.CanCompleteEventNameColumn}, @{Columns.CompleteEventNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.QuestTypeIdColumn}", QuestTypeId)
            command.Parameters.AddWithValue($"@{Columns.CanAcceptEventNameColumn}", CanAcceptEventName)
            command.Parameters.AddWithValue($"@{Columns.AcceptEventNameColumn}", AcceptEventName)
            command.Parameters.AddWithValue($"@{Columns.CanCompleteEventNameColumn}", CanCompleteEventName)
            command.Parameters.AddWithValue($"@{Columns.CompleteEventNameColumn}", CompleteEventName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Const QuestType1 = 1L
    Friend Sub PopulateQuestTypes(connection As SqliteConnection)
        PopulateQuestTypesRecord(connection, QuestType1, "CharacterCanAcceptCellarRatsQuest", "CharacterAcceptCellarRatsQuest", "CharacterCanCompleteCellarRatsQuest", "CharacterCompleteCellarRatsQuest")
    End Sub

End Module
