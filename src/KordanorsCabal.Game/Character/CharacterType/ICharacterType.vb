Public Interface ICharacterType
    Inherits IBaseThingie
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Function CanSpawn(locationType As LocationType, level As DungeonLevel) As Boolean
    Function GenerateAttackType() As AttackType
    Function InitialStatistics(worldData As WorldData) As IReadOnlyList(Of (CharacterStatisticType, Long))
    Function IsEnemy(character As Character) As Boolean
    ReadOnly Property IsUndead As Boolean
    Function MaximumEncumbrance(worldData As IWorldData, character As Character) As Long
    ReadOnly Property Name As String
    Function PartingShot() As String
    Function RollMoneyDrop() As Long
    Function SpawnCount(level As DungeonLevel) As Long
    ReadOnly Property XPValue As Long
    Sub DropLoot(location As Location)
End Interface
