Public Interface ICharacterType
    Inherits IBaseThingie
    'metadata
    ReadOnly Property IsUndead As Boolean
    ReadOnly Property Name As String
    'spawning
    ReadOnly Property Spawning As ICharacterTypeSpawning
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long))
    Function SpawnCount(level As IDungeonLevel) As Long
    'combat
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Sub DropLoot(location As ILocation)
    Function GenerateAttackType() As AttackType
    Function IsEnemy(character As ICharacterType) As Boolean
    Function PartingShot() As String
    Function RollMoneyDrop() As Long
    ReadOnly Property XPValue As Long
End Interface
