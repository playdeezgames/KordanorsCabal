﻿Public Class CharacterTypeCombat
    Inherits BaseThingie
    Implements ICharacterTypeCombat

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Shared Function FromId(worldData As IWorldData, id As Long) As ICharacterTypeCombat
        Return New CharacterTypeCombat(worldData, id)
    End Function
    Function CanBeBribedWith(itemType As OldItemType) As Boolean Implements ICharacterTypeCombat.CanBeBribedWith
        Return WorldData.CharacterTypeBribe.Read(Id, itemType)
    End Function
    Sub DropLoot(location As ILocation) Implements ICharacterTypeCombat.DropLoot
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If lootTable Is Nothing OrElse Not lootTable.Any Then
            Return
        End If
        Dim itemType = CType(RNG.FromGenerator(lootTable), OldItemType)
        If itemType <> OldItemType.None Then
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
End Class
