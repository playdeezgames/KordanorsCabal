Public Class CharacterType
    Inherits BaseThingie
    Implements ICharacterType
    ReadOnly Property Name As String Implements ICharacterType.Name
        Get
            Return WorldData.CharacterType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property XPValue As Long Implements ICharacterType.XPValue
        Get
            Return WorldData.CharacterType.ReadXPValue(Id).Value
        End Get
    End Property
    ReadOnly Property IsUndead As Boolean Implements ICharacterType.IsUndead
        Get
            Return If(WorldData.CharacterType.ReadIsUndead(Id), 0) > 0
        End Get
    End Property
    Friend Sub New(worldData As IWorldData, characterTypeId As Long)
        MyBase.New(worldData, characterTypeId)
    End Sub
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (CharacterStatisticType, Long)) Implements ICharacterType.InitialStatistics
        Get
            Dim results = WorldData.CharacterTypeInitialStatistic.ReadAllForCharacterType(Id)
            If results Is Nothing Then
                Return Nothing
            End If
            Return results.
            Select(Function(x) (CharacterStatisticType.FromId(WorldData, x.Item1), x.Item2)).ToList
        End Get
    End Property
    Function MaximumEncumbrance(worldData As IWorldData, character As ICharacter) As Long Implements ICharacterType.MaximumEncumbrance
        Return If(
            character.GetStatistic(CharacterStatisticType.FromId(worldData, 24L)), 0) +
            If(
                character.GetStatistic(CharacterStatisticType.FromId(worldData, 25L)), 0) *
            If(
                character.GetStatistic(CharacterStatisticType.FromId(worldData, 1L)), 0)
    End Function
    Function IsEnemy(character As ICharacter) As Boolean Implements ICharacterType.IsEnemy
        Return WorldData.CharacterTypeEnemy.Read(Id, character.CharacterType.Id)
    End Function
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean Implements ICharacterType.CanSpawn
        Return WorldData.CharacterTypeSpawnLocation.Read(Id, level.Id, locationType.Id)
    End Function
    Function PartingShot() As String Implements ICharacterType.PartingShot
        Dim partingShotTable = WorldData.CharacterTypePartingShot.Read(Id)
        If Not partingShotTable.Any Then
            Return ""
        End If
        Return RNG.FromGenerator(partingShotTable)
    End Function
    Sub DropLoot(location As Location) Implements ICharacterType.DropLoot
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If Not lootTable.Any Then
            Return
        End If
        Dim itemType = CType(RNG.FromGenerator(lootTable), OldItemType)
        If itemType <> OldItemType.None Then
            location.Inventory.Add(Item.Create(WorldData, itemType))
        End If
    End Sub
    Function CanBeBribedWith(itemType As OldItemType) As Boolean Implements ICharacterType.CanBeBribedWith
        Return WorldData.CharacterTypeBribe.Read(Id, itemType)
    End Function
    Function GenerateAttackType() As AttackType Implements ICharacterType.GenerateAttackType
        Dim table = WorldData.CharacterTypeAttackType.Read(Id)
        If table Is Nothing Then
            Return AttackType.None
        End If
        Return CType(RNG.FromGenerator(table), AttackType)
    End Function
    Function RollMoneyDrop() As Long Implements ICharacterType.RollMoneyDrop
        Return RNG.RollDice(WorldData.CharacterType.ReadMoneyDropDice(Id))
    End Function
    Function SpawnCount(level As IDungeonLevel) As Long Implements ICharacterType.SpawnCount
        Return If(WorldData.CharacterTypeSpawnCount.ReadSpawnCount(Id, level.Id), 0)
    End Function
    Shared Function FromId(worldData As IWorldData, characterTypeId As Long) As ICharacterType
        Return New CharacterType(worldData, characterTypeId)
    End Function
End Class
