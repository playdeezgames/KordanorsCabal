Friend MustInherit Class CharacterType
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
    MustOverride ReadOnly Property MaximumEncumbrance(character As Character) As Long
    MustOverride Function IsEnemy(character As Character) As Boolean
    MustOverride ReadOnly Property SpawnCount(level As DungeonLevel) As Long
    Overridable Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
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

    Overridable ReadOnly Property IsUndead As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return False
    End Function

    Overridable Function GenerateAttackType(character As Character) As AttackType
        Return AttackType.Physical
    End Function
End Class
Friend Module CharacterTypeDescriptorUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of OldCharacterType, CharacterType) =
        New Dictionary(Of OldCharacterType, CharacterType) From
        {
            {OldCharacterType.Acolyte, New AcolyteDescriptor},
            {OldCharacterType.Badger, New BadgerDescriptor},
            {OldCharacterType.Bat, New BatDescriptor},
            {OldCharacterType.Bishop, New BishopDescriptor},
            {OldCharacterType.CabalLeader, New CabalLeaderDescriptor},
            {OldCharacterType.Goblin, New GoblinDescriptor},
            {OldCharacterType.GoblinElite, New GoblinEliteDescriptor},
            {OldCharacterType.Kordanor, New KordanorDescriptor},
            {OldCharacterType.Malcontent, New MalcontentDescriptor},
            {OldCharacterType.MoonPerson, New MoonPersonDescriptor},
            {OldCharacterType.N00b, New N00bDescriptor},
            {OldCharacterType.Priest, New PriestDescriptor},
            {OldCharacterType.Rat, New RatDescriptor},
            {OldCharacterType.Skeleton, New SkeletonDescriptor},
            {OldCharacterType.Snake, New SnakeDescriptor},
            {OldCharacterType.Zombie, New ZombieDescriptor}
        }
    Function AllCharacterTypes() As IEnumerable(Of OldCharacterType)
        Return CharacterTypeDescriptors.Keys
    End Function
End Module
