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
    Overridable Sub EnqueueMessage(ParamArray lines() As String) Implements ICharacter.EnqueueMessage
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
    ReadOnly Property Name As String Implements ICharacter.Name
        Get
            Return CharacterType.Name
        End Get
    End Property
    Sub Destroy() Implements ICharacter.Destroy
        WorldData.Character.Clear(Id)
    End Sub
    Function DoWeaponWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacter.DoWeaponWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneWeapon()
            If brokenItemType IsNot Nothing Then
                result.Add(brokenItemType)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Function DoArmorWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacter.DoArmorWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneArmor()
            If brokenItemType.HasValue Then
                result.Add(ItemType.FromId(WorldData, brokenItemType.Value))
            End If
            wear -= 1
        End While
        Return result
    End Function
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
    Private Function WearOneArmor() As Long?
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Armor.IsArmor).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneArmor = If(item.ItemType IsNot Nothing, item.ItemType.Id, Nothing)
                item.Destroy()
            End If
        End If
    End Function
    Function Equipment(equipSlot As IEquipSlot) As IItem Implements ICharacter.Equipment
        Return Item.FromId(WorldData, WorldData.CharacterEquipSlot.ReadForCharacterEquipSlot(Id, equipSlot.Id))
    End Function
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot) Implements ICharacter.EquippedSlots
        Get
            Return WorldData.CharacterEquipSlot.ReadEquipSlotsForCharacter(Id).Select(Function(x) New EquipSlot(WorldData, x))
        End Get
    End Property
    Public Sub Unequip(equipSlot As IEquipSlot) Implements ICharacter.Unequip
        Dim item = Equipment(equipSlot)
        If item IsNot Nothing Then
            Me.Items.Inventory.Add(item)
            WorldData.CharacterEquipSlot.Clear(Id, equipSlot.Id)
        End If
    End Sub
    Public ReadOnly Property CanGamble As Boolean Implements ICharacter.CanGamble
        Get
            Return Statuses.Money >= 5
        End Get
    End Property
    Public Sub Gamble() Implements ICharacter.Gamble
        If Not CanGamble Then
            EnqueueMessage("You cannot gamble at this time!")
            Return
        End If
        Dim lines As New List(Of String)
        lines.Add("You flip the two coins!")
        Dim firstCoin = RNG.FromRange(0, 1)
        lines.Add($"The first coin comes up {If(firstCoin > 0, "heads", "tails")}!")
        Dim secondCoin = RNG.FromRange(0, 1)
        lines.Add($"The second coin comes up {If(secondCoin > 0, "heads", "tails")}!")
        Dim winner = firstCoin > 0 AndAlso secondCoin > 0
        If winner Then
            lines.Add("You win and receive 15 money!")
            Statuses.Money += 15
        Else
            lines.Add("You lose and must pay 5 money!")
            Statuses.Money -= 5
        End If
        'TODO: sound effect
        EnqueueMessage(lines.ToArray)
    End Sub
    Public Sub Equip(item As IItem) Implements ICharacter.Equip
        If item.Equipment.CanEquip Then
            WorldData.InventoryItem.ClearForItem(item.Id)
            Dim equipSlots = item.Equipment.EquipSlots
            Dim availableEquipSlots = equipSlots.Where(Function(x) Equipment(x) Is Nothing)
            Dim equipSlot = If(availableEquipSlots.Any, availableEquipSlots.First, equipSlots.First)
            Dim oldItem = Equipment(equipSlot)
            If oldItem IsNot Nothing Then
                Me.Items.Inventory.Add(oldItem)
            End If
            WorldData.CharacterEquipSlot.Write(Id, equipSlot.Id, item.Id)
            EnqueueMessage($"You equip {item.Name} to {equipSlot.Name}.")
            Return
        End If
        EnqueueMessage($"You cannot equip {item.Name}!")
    End Sub
    Public Property Mode As Long Implements ICharacter.Mode
        Get
            Return WorldData.Player.ReadPlayerMode().Value
        End Get
        Set(value As Long)
            WorldData.Player.WritePlayerMode(value)
        End Set
    End Property
    Public ReadOnly Property CanInteract As Boolean Implements ICharacter.CanInteract
        Get
            Return (Movement.Location?.Feature?.Id).HasValue
        End Get
    End Property
    Public Sub Interact() Implements ICharacter.Interact
        If CanInteract Then
            Mode = Movement.Location.Feature.InteractionMode()
        End If
    End Sub

    Public ReadOnly Property HasEquipment As Boolean Implements ICharacter.HasEquipment
        Get
            Return EquippedItems.Any
        End Get
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
            Return CharacterHealth.FromId(WorldData, Id)
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
End Class
