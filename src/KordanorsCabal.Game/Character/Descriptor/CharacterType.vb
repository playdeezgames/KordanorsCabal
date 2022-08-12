Friend MustInherit Class CharacterType
    ReadOnly Property Id As Long
    Sub New(characterTypeId As Long)
        Id = characterTypeId
    End Sub
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
            {OldCharacterType.Acolyte, New AcolyteDescriptor(OldCharacterType.Acolyte)},
            {OldCharacterType.Badger, New BadgerDescriptor(OldCharacterType.Badger)},
            {OldCharacterType.Bat, New BatDescriptor(OldCharacterType.Bat)},
            {OldCharacterType.Bishop, New BishopDescriptor(OldCharacterType.Bishop)},
            {OldCharacterType.CabalLeader, New CabalLeaderDescriptor(OldCharacterType.CabalLeader)},
            {OldCharacterType.Goblin, New GoblinDescriptor(OldCharacterType.Goblin)},
            {OldCharacterType.GoblinElite, New GoblinEliteDescriptor(OldCharacterType.GoblinElite)},
            {OldCharacterType.Kordanor, New KordanorDescriptor(OldCharacterType.Kordanor)},
            {OldCharacterType.Malcontent, New MalcontentDescriptor(OldCharacterType.Malcontent)},
            {OldCharacterType.MoonPerson, New MoonPersonDescriptor(OldCharacterType.MoonPerson)},
            {OldCharacterType.N00b, New N00bDescriptor(OldCharacterType.N00b)},
            {OldCharacterType.Priest, New PriestDescriptor(OldCharacterType.Priest)},
            {OldCharacterType.Rat, New RatDescriptor(OldCharacterType.Rat)},
            {OldCharacterType.Skeleton, New SkeletonDescriptor(OldCharacterType.Skeleton)},
            {OldCharacterType.Snake, New SnakeDescriptor(OldCharacterType.Snake)},
            {OldCharacterType.Zombie, New ZombieDescriptor(OldCharacterType.Zombie)}
        }
    Function AllCharacterTypes() As IEnumerable(Of OldCharacterType)
        Return CharacterTypeDescriptors.Keys
    End Function
End Module
