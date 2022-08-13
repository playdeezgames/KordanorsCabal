Friend MustInherit Class CharacterType
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.CharacterType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property XPValue As Long
        Get
            Return StaticWorldData.World.CharacterType.ReadXPValue(Id).Value
        End Get
    End Property
    ReadOnly Property IsUndead As Boolean
        Get
            Return If(StaticWorldData.World.CharacterType.ReadIsUndead(Id), 0) > 0
        End Get
    End Property
    Sub New(characterTypeId As Long)
        Id = characterTypeId
    End Sub

    Function InitialStatistic(statisticType As CharacterStatisticType) As Long?
        Return StaticWorldData.World.CharacterTypeInitialStatistic.Read(Id, statisticType.Id)
    End Function
    Function InitialStatistics() As IReadOnlyList(Of CharacterStatisticType)
        Return StaticWorldData.World.
            CharacterTypeInitialStatistic.ReadForCharacterType(Id).
            Select(Function(x) New CharacterStatisticType(x)).ToList
    End Function
    Function MaximumEncumbrance(character As Character) As Long
        Return If(
            character.GetStatistic(CharacterStatisticType.FromName(BaseLift)), 0) +
            If(
                character.GetStatistic(CharacterStatisticType.FromName(BonusLift)), 0) *
            If(
                character.GetStatistic(CharacterStatisticType.FromName(Strength)), 0)
    End Function
    Function IsEnemy(character As Character) As Boolean
        Return StaticWorldData.World.CharacterTypeEnemy.Read(Id, character.CharacterType)
    End Function
    Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return StaticWorldData.World.CharacterTypeSpawnLocation.Read(Id, level.Id, location.LocationType.Id)
    End Function

    Function PartingShot() As String
        Dim partingShotTable = StaticWorldData.World.CharacterTypePartingShot.Read(Id)
        If Not partingShotTable.Any Then
            Return ""
        End If
        Return RNG.FromGenerator(partingShotTable)
    End Function
    Sub DropLoot(location As Location)
        Dim lootTable = StaticWorldData.World.CharacterTypeLoot.Read(Id)
        If Not lootTable.Any Then
            Return
        End If
        Dim itemType = CType(RNG.FromGenerator(lootTable), ItemType)
        If itemType <> ItemType.None Then
            location.Inventory.Add(Item.Create(itemType))
        End If
    End Sub
    Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return StaticWorldData.World.CharacterTypeBribe.Read(Id, itemType)
    End Function
    Function GenerateAttackType() As AttackType
        Return CType(RNG.FromGenerator(StaticWorldData.World.CharacterTypeAttackType.Read(Id)), AttackType)
    End Function
    Function RollMoneyDrop() As Long
        Return RNG.RollDice(StaticWorldData.World.CharacterType.ReadMoneyDropDice(Id))
    End Function
    Function SpawnCount(level As DungeonLevel) As Long
        Return If(StaticWorldData.World.CharacterTypeSpawnCount.ReadSpawnCount(Id, level.Id), 0)
    End Function
End Class
