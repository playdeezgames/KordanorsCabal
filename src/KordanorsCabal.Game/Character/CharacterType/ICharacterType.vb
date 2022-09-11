Public Interface ICharacterType
    Inherits IBaseThingie
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean
    Function GenerateAttackType() As AttackType
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (CharacterStatisticType, Long))
    Function IsEnemy(character As ICharacter) As Boolean
    ReadOnly Property IsUndead As Boolean
    Function MaximumEncumbrance(worldData As IWorldData, character As ICharacter) As Long
    ReadOnly Property Name As String
    Function PartingShot() As String
    Function RollMoneyDrop() As Long
    Function SpawnCount(level As IDungeonLevel) As Long
    ReadOnly Property XPValue As Long
    Sub DropLoot(location As Location)
End Interface
