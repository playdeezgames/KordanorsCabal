Public Class WornItemDescriptor
    Inherits ItemTypeDescriptor
    Public Sub New(
                  name As String,
                  equipSlots As IEnumerable(Of EquipSlot),
                  Optional spawnLocationTypes As IReadOnlyDictionary(Of OldDungeonLevel, HashSet(Of LocationType)) = Nothing,
                  Optional spawnCounts As IReadOnlyDictionary(Of OldDungeonLevel, String) = Nothing)
        MyBase.New(
            name,, spawnLocationTypes, spawnCounts)
    End Sub
End Class
