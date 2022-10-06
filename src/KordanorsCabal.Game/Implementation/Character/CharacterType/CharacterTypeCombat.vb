Public Class CharacterTypeCombat
    Inherits BaseThingie
    Implements ICharacterTypeCombat
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterTypeCombat
        Return If(id.HasValue, New CharacterTypeCombat(worldData, id.Value), Nothing)
    End Function
    Function CanBeBribedWith(itemType As IItemType) As Boolean Implements ICharacterTypeCombat.CanBeBribedWith
        Return WorldData.CharacterTypeBribe.Read(Id, itemType.Id)
    End Function
    Sub DropLoot(location As ILocation) Implements ICharacterTypeCombat.DropLoot
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If lootTable Is Nothing OrElse Not lootTable.Any Then
            Return
        End If
        Dim itemType = RNG.FromGenerator(lootTable)
        If itemType <> 0L Then
            location.Inventory.Add(Item.Create(WorldData, itemType))
        End If
    End Sub
    Function GenerateAttackType() As AttackType Implements ICharacterTypeCombat.GenerateAttackType
        Dim table = WorldData.CharacterTypeAttackType.Read(Id)
        If table Is Nothing Then
            Return AttackType.None
        End If
        Return CType(RNG.FromGenerator(table), AttackType)
    End Function
    Function IsEnemy(otherCharacterType As ICharacterType) As Boolean Implements ICharacterTypeCombat.IsEnemy
        Return WorldData.CharacterTypeEnemy.Read(Id, otherCharacterType.Id)
    End Function
    Function PartingShot() As String Implements ICharacterTypeCombat.PartingShot
        Dim partingShotTable = WorldData.CharacterTypePartingShot.Read(Id)
        If partingShotTable Is Nothing Then
            Return Nothing
        End If
        If Not partingShotTable.Any Then
            Return ""
        End If
        Return RNG.FromGenerator(partingShotTable)
    End Function
    Function RollMoneyDrop() As Long Implements ICharacterTypeCombat.RollMoneyDrop
        Return RNG.RollDice(WorldData.CharacterType.ReadMoneyDropDice(Id))
    End Function
    ReadOnly Property XPValue As Long Implements ICharacterTypeCombat.XPValue
        Get
            Return If(WorldData.CharacterType.ReadXPValue(Id), 0L)
        End Get
    End Property
End Class
