Friend MustInherit Class CharacterTypeDescriptor
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of OldCharacterStatisticType, Long)
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
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {CharacterType.Acolyte, New AcolyteDescriptor},
            {CharacterType.Badger, New BadgerDescriptor},
            {CharacterType.Bat, New BatDescriptor},
            {CharacterType.Bishop, New BishopDescriptor},
            {CharacterType.CabalLeader, New CabalLeaderDescriptor},
            {CharacterType.Goblin, New GoblinDescriptor},
            {CharacterType.GoblinElite, New GoblinEliteDescriptor},
            {CharacterType.Kordanor, New KordanorDescriptor},
            {CharacterType.Malcontent, New MalcontentDescriptor},
            {CharacterType.MoonPerson, New MoonPersonDescriptor},
            {CharacterType.N00b, New N00bDescriptor},
            {CharacterType.Priest, New PriestDescriptor},
            {CharacterType.Rat, New RatDescriptor},
            {CharacterType.Skeleton, New SkeletonDescriptor},
            {CharacterType.Snake, New SnakeDescriptor},
            {CharacterType.Zombie, New ZombieDescriptor}
        }
    Function AllCharacterTypes() As IEnumerable(Of CharacterType)
        Return CharacterTypeDescriptors.Keys
    End Function
End Module
