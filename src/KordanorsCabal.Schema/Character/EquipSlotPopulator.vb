Module EquipSlotPopulator
    Private Sub PopulateEquipSlotsRecord(connection As SqliteConnection, EquipSlotId As Long, EquipSlotName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.EquipSlots}]([{Columns.EquipSlotIdColumn}], [{Columns.EquipSlotNameColumn}]) VALUES (@{Columns.EquipSlotIdColumn}, @{Columns.EquipSlotNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.EquipSlotIdColumn}", EquipSlotId)
            command.Parameters.AddWithValue($"@{Columns.EquipSlotNameColumn}", EquipSlotName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const EquipSlot1 = 1L
    Friend Const EquipSlot2 = 2L
    Friend Const EquipSlot3 = 3L
    Friend Const EquipSlot4 = 4L
    Friend Const EquipSlot5 = 5L
    Friend Const EquipSlot6 = 6L
    Friend Const EquipSlot7 = 7L
    Friend Const EquipSlot8 = 8L
    Friend Sub PopulateEquipSlots(connection As SqliteConnection)
        PopulateEquipSlotsRecord(connection, EquipSlot1, "Weapon")
        PopulateEquipSlotsRecord(connection, EquipSlot2, "Shield")
        PopulateEquipSlotsRecord(connection, EquipSlot3, "Head")
        PopulateEquipSlotsRecord(connection, EquipSlot4, "Torso")
        PopulateEquipSlotsRecord(connection, EquipSlot5, "Legs")
        PopulateEquipSlotsRecord(connection, EquipSlot6, "Neck")
        PopulateEquipSlotsRecord(connection, EquipSlot7, "LHand")
        PopulateEquipSlotsRecord(connection, EquipSlot8, "RHand")
    End Sub
End Module
