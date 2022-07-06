Friend MustInherit Class CharacterTypeDescriptor
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
    MustOverride ReadOnly Property MaximumEncumbrance(character As Character) As Single
    MustOverride Function IsEnemy(character As Character) As Boolean
    MustOverride ReadOnly Property SpawnCount(level As Long) As Long
    Overridable Function CanSpawn(location As Location, level As Long) As Boolean
        Return False
    End Function
    MustOverride ReadOnly Property Name As String

    Overridable Function RollMoneyDrop() As Long
        Return 0
    End Function

    Overridable Function PartingShot() As String
        Return ""
    End Function

    MustOverride ReadOnly Property XPValue As Long

    MustOverride Sub DropLoot(location As Location)
End Class
Friend Module CharacterTypeDescriptorUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {CharacterType.Acolyte, New AcolyteDescriptor},
            {CharacterType.Bishop, New BishopDescriptor},
            {CharacterType.CabalLeader, New CabalLeaderDescriptor},
            {CharacterType.Goblin, New GoblinDescriptor},
            {CharacterType.GoblinElite, New GoblinEliteDescriptor},
            {CharacterType.Kordanor, New KordanorDescriptor},
            {CharacterType.N00b, New N00bDescriptor},
            {CharacterType.Priest, New PriestDescriptor},
            {CharacterType.Skeleton, New SkeletonDescriptor}
        }
    ReadOnly Property AllCharacterTypes As IEnumerable(Of CharacterType)
        Get
            Return CharacterTypeDescriptors.Keys
        End Get
    End Property
End Module
