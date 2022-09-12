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
            Return If(WorldData.CharacterType.ReadXPValue(Id), 0L)
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

    Public ReadOnly Property Spawning As ICharacterTypeSpawning Implements ICharacterType.Spawning
        Get
            Return CharacterTypeSpawning.FromId(WorldData, Id)
        End Get
    End Property

    Function IsEnemy(otherCharacterType As ICharacterType) As Boolean Implements ICharacterType.IsEnemy
        Return WorldData.CharacterTypeEnemy.Read(Id, otherCharacterType.Id)
    End Function
    Function PartingShot() As String Implements ICharacterType.PartingShot
        Dim partingShotTable = WorldData.CharacterTypePartingShot.Read(Id)
        If partingShotTable Is Nothing Then
            Return Nothing
        End If
        If Not partingShotTable.Any Then
            Return ""
        End If
        Return RNG.FromGenerator(partingShotTable)
    End Function
    Sub DropLoot(location As ILocation) Implements ICharacterType.DropLoot
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If lootTable Is Nothing OrElse Not lootTable.Any Then
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
    Shared Function FromId(worldData As IWorldData, characterTypeId As Long) As ICharacterType
        Return New CharacterType(worldData, characterTypeId)
    End Function
End Class
