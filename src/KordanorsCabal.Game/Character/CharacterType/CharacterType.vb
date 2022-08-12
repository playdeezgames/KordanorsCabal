Friend MustInherit Class CharacterType
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.CharacterType.ReadName(Id)
        End Get
    End Property

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
