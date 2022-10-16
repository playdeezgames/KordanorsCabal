Public Class Character
    Inherits BaseThingie
    Implements ICharacter
    Sub New(worldData As IWorldData, characterId As Long)
        MyBase.New(worldData, characterId)
    End Sub

    ReadOnly Property CharacterType As ICharacterType Implements ICharacter.CharacterType
        Get
            Dim result = WorldData.Character.ReadCharacterType(Id)
            If result Is Nothing Then
                Return Nothing
            End If
            Return Game.CharacterType.FromId(WorldData, result.Value)
        End Get
    End Property
    Overridable Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String) Implements ICharacter.EnqueueMessage
        'do nothing!
    End Sub
    Friend Shared Function Create(worldData As IWorldData, characterType As ICharacterType, location As ILocation, initialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long))) As ICharacter
        Dim character = FromId(worldData, worldData.Character.Create(characterType.Id, location.Id))
        If initialStatistics IsNot Nothing Then
            For Each entry In initialStatistics
                character.Statistics.SetStatistic(entry.Item1, entry.Item2)
            Next
        End If
        Return character
    End Function
    Shared Function FromId(worldData As IWorldData, characterId As Long?) As ICharacter
        Return If(characterId.HasValue, New Character(worldData, characterId.Value), Nothing)
    End Function
    Sub Destroy() Implements ICharacter.Destroy
        WorldData.Character.Clear(Id)
    End Sub
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    Private Function WearOneWeapon() As IItemType
        WearOneWeapon = Nothing
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Weapon.IsWeapon).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneWeapon = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Public Property Mode As Long Implements ICharacter.Mode
        Get
            Return WorldData.Player.ReadPlayerMode().Value
        End Get
        Set(value As Long)
            WorldData.Player.WritePlayerMode(value)
        End Set
    End Property
    Public ReadOnly Property Movement As ICharacterMovement Implements ICharacter.Movement
        Get
            Return CharacterMovement.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Quest As ICharacterQuest Implements ICharacter.Quest
        Get
            Return CharacterQuest.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Health As ICharacterHealth Implements ICharacter.Health
        Get
            Return CharacterHealth.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property PhysicalCombat As ICharacterPhysicalCombat Implements ICharacter.PhysicalCombat
        Get
            Return CharacterPhysicalCombat.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Advancement As ICharacterAdvancement Implements ICharacter.Advancement
        Get
            Return CharacterAdvancement.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property MentalCombat As ICharacterMentalCombat Implements ICharacter.MentalCombat
        Get
            Return CharacterMentalCombat.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Spellbook As ICharacterSpellbook Implements ICharacter.Spellbook
        Get
            Return CharacterSpellbook.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Mana As ICharacterMana Implements ICharacter.Mana
        Get
            Return CharacterMana.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Statistics As ICharacterStatistics Implements ICharacter.Statistics
        Get
            Return CharacterStatistics.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Items As ICharacterItems Implements ICharacter.Items
        Get
            Return CharacterItems.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Repair As ICharacterRepair Implements ICharacter.Repair
        Get
            Return CharacterRepair.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Encumbrance As ICharacterEncumbrance Implements ICharacter.Encumbrance
        Get
            Return CharacterEncumbrance.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Statuses As ICharacterStatuses Implements ICharacter.Statuses
        Get
            Return CharacterStatuses.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Interaction As ICharacterInteraction Implements ICharacter.Interaction
        Get
            Return CharacterInteraction.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Equipment As ICharacterEquipment Implements ICharacter.Equipment
        Get
            Return CharacterEquipment.FromCharacter(WorldData, Me)
        End Get
    End Property
End Class
