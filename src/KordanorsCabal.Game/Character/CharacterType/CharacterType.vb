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

    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
    MustOverride ReadOnly Property MaximumEncumbrance(character As Character) As Long
    MustOverride Function IsEnemy(character As Character) As Boolean
    Overridable Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return False
    End Function

    Overridable Function PartingShot() As String
        Return ""
    End Function


    MustOverride Sub DropLoot(location As Location)

    Overridable Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return False
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
