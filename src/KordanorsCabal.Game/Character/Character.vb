Public Class Character
    ReadOnly Property Id As Long
    Sub New(characterId As Long)
        Id = characterId
    End Sub
    ReadOnly Property CharacterType As CharacterType
        Get
            Return CType(CharacterData.ReadCharacterType(Id).Value, CharacterType)
        End Get
    End Property

    ReadOnly Property CanFight As Boolean
        Get
            Return Location.Enemies(Me).Any
        End Get
    End Property

    Friend Shared Function Create(characterType As CharacterType, location As Location) As Character
        Dim character = FromId(CharacterData.Create(characterType, location.Id))
        For Each entry In characterType.InitialStatistics
            character.SetStatistic(entry.Key, entry.Value)
        Next
        Return character
    End Function

    Public Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
        CharacterStatisticData.Write(Id, statisticType, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub

    Friend Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType).Value + delta)
    End Sub

    Property Location As Location
        Get
            Return Location.FromId(CharacterData.ReadLocation(Id).Value)
        End Get
        Set(value As Location)
            CharacterData.WriteLocation(Id, value.Id)
            CharacterLocationData.Write(Id, value.Id)
        End Set
    End Property
    Shared Function FromId(characterId As Long) As Character
        Return New Character(characterId)
    End Function

    Public Function GetStatistic(statisticType As CharacterStatisticType) As Long?
        Return If(CharacterStatisticData.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = CharacterInventoryData.Read(Id)
            If Not inventoryId.HasValue Then
                inventoryId = InventoryData.Create()
                CharacterInventoryData.Write(Id, inventoryId.Value)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property

    ReadOnly Property IsEncumbered As Boolean
        Get
            Return Encumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property Encumbrance As Single
        Get
            Return Inventory.TotalEncumbrance
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Single
        Get
            Return CharacterType.MaximumEncumbrance(Me)
        End Get
    End Property
    Public Function HasVisited(location As Location) As Boolean
        Return CharacterLocationData.Read(Id, location.Id)
    End Function

    Friend Function IsEnemy(character As Character) As Boolean
        Return CharacterType.IsEnemy(character)
    End Function

    Friend Function RollDefend() As Long
        Dim dice = GetStatistic(CharacterStatisticType.Dexterity)
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Dim maximumDefend = GetStatistic(CharacterStatisticType.BaseMaximumDefend).Value
        Return Math.Min(result, maximumDefend)
    End Function

    Friend Function RollAttack() As Long
        Dim dice = GetAttackDice()
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function

    Private Function GetAttackDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.Strength).Value
        For Each entry In Equipment
            dice += entry.Value.AttackDice
        Next
        Return dice
    End Function

    ReadOnly Property Name As String
        Get
            Return CharacterType.Name
        End Get
    End Property

    ReadOnly Property CurrentHP As Long
        Get
            Return Math.Max(0, MaximumHP - GetStatistic(CharacterStatisticType.Wounds).Value)
        End Get
    End Property

    ReadOnly Property MaximumHP As Long
        Get
            Return GetStatistic(CharacterStatisticType.HP).Value
        End Get
    End Property

    ReadOnly Property PartingShot As String
        Get
            Return CharacterType.PartingShot
        End Get
    End Property

    ReadOnly Property CurrentMP As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.MP).Value - GetStatistic(CharacterStatisticType.Stress).Value)
        End Get
    End Property

    ReadOnly Property CurrentMana As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.Mana).Value - GetStatistic(CharacterStatisticType.Fatigue).Value)
        End Get
    End Property


    Friend Sub DoDamage(damage As Long)
        ChangeStatistic(CharacterStatisticType.Wounds, damage)
    End Sub

    Friend Sub Destroy()
        CharacterData.Clear(Id)
    End Sub

    ReadOnly Property IsDead As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.Wounds).Value >= GetStatistic(CharacterStatisticType.HP).Value
        End Get
    End Property

    Function DetermineDamage(value As Long) As Long
        Dim maximumDamage As Long? = Nothing
        For Each entry In Equipment
            Dim item = entry.Value
            Dim itemMaximumDamage = item.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, GetStatistic(CharacterStatisticType.UnarmedMaximumDamage).Value)
        Return If(value < 0, 0, If(value > maximumDamage.Value, maximumDamage.Value, value))
    End Function

    ReadOnly Property RollMoneyDrop As Long
        Get
            Return CharacterType.RollMoneyDrop
        End Get
    End Property

    ReadOnly Property XPValue As Long
        Get
            Return CharacterType.XPValue
        End Get
    End Property

    ReadOnly Property NeedsHealing As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.Wounds).Value > 0
        End Get
    End Property

    Friend Sub DoWeaponWear(wear As Long)
        While wear > 0
            DoOneWeaponWear()
            wear -= 1
        End While
    End Sub

    Private Sub DoOneWeaponWear()
        Dim item = RNG.FromEnumerable(Equipment.Values.Where(Function(x) x.MaximumDurability IsNot Nothing))
        item.ReduceDurability(1)
        If item.IsBroken Then
            item.Destroy()
        End If
    End Sub

    Friend Sub DropLoot()
        'TODO: unequip everything
        For Each item In Inventory.Items
            Location.Inventory.Add(item)
        Next
        CharacterType.DropLoot(Location)
    End Sub

    ReadOnly Property Equipment As IReadOnlyDictionary(Of EquipSlot, Item)
        Get
            Return CharacterEquipSlotData.Read(Id).
                ToDictionary(
                    Function(x) CType(x.Item1, EquipSlot),
                    Function(x) Item.FromId(x.Item2))
        End Get
    End Property
End Class
