Public Class CharacterType
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return WorldData.CharacterType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property XPValue As Long
        Get
            Return WorldData.CharacterType.ReadXPValue(Id).Value
        End Get
    End Property
    ReadOnly Property IsUndead As Boolean
        Get
            Return If(WorldData.CharacterType.ReadIsUndead(Id), 0) > 0
        End Get
    End Property
    Sub New(worldData As WorldData, characterTypeId As Long)
        MyBase.New(worldData, characterTypeId)
    End Sub

    Function OldInitialStatistic(statisticType As CharacterStatisticType) As Long?
        Return WorldData.CharacterTypeInitialStatistic.Read(Id, statisticType.Id)
    End Function
    Function OldInitialStatistics() As IReadOnlyList(Of CharacterStatisticType)
        Return WorldData.
            CharacterTypeInitialStatistic.ReadForCharacterType(Id).
            Select(Function(x) New CharacterStatisticType(x)).ToList
    End Function
    Function InitialStatistics() As IReadOnlyList(Of (CharacterStatisticType, Long))
        Return WorldData.
            CharacterTypeInitialStatistic.ReadAllForCharacterType(Id).
            Select(Function(x) (CharacterStatisticType.FromId(x.Item1), x.Item2)).ToList
    End Function
    Function MaximumEncumbrance(character As Character) As Long
        Return If(
            character.GetStatistic(CharacterStatisticType.FromId(BaseLift)), 0) +
            If(
                character.GetStatistic(CharacterStatisticType.FromId(BonusLift)), 0) *
            If(
                character.GetStatistic(CharacterStatisticType.FromId(Strength)), 0)
    End Function
    Function IsEnemy(character As Character) As Boolean
        Return WorldData.CharacterTypeEnemy.Read(Id, character.CharacterType.Id)
    End Function
    Function CanSpawn(locationType As LocationType, level As DungeonLevel) As Boolean
        Return WorldData.CharacterTypeSpawnLocation.Read(Id, level.Id, locationType.Id)
    End Function

    Function PartingShot() As String
        Dim partingShotTable = WorldData.CharacterTypePartingShot.Read(Id)
        If Not partingShotTable.Any Then
            Return ""
        End If
        Return RNG.FromGenerator(partingShotTable)
    End Function
    Sub DropLoot(location As Location)
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If Not lootTable.Any Then
            Return
        End If
        Dim itemType = CType(RNG.FromGenerator(lootTable), ItemType)
        If itemType <> ItemType.None Then
            location.Inventory.Add(Item.Create(itemType))
        End If
    End Sub
    Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return WorldData.CharacterTypeBribe.Read(Id, itemType)
    End Function
    Function GenerateAttackType() As AttackType
        Return CType(RNG.FromGenerator(WorldData.CharacterTypeAttackType.Read(Id)), AttackType)
    End Function
    Function RollMoneyDrop() As Long
        Return RNG.RollDice(WorldData.CharacterType.ReadMoneyDropDice(Id))
    End Function
    Function SpawnCount(level As DungeonLevel) As Long
        Return If(WorldData.CharacterTypeSpawnCount.ReadSpawnCount(Id, level.Id), 0)
    End Function
    Shared Function FromId(worldData As WorldData, characterTypeId As Long) As CharacterType
        Return New CharacterType(worldData, characterTypeId)
    End Function
End Class
