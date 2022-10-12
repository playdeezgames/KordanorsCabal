Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
    ReadOnly Property AttackDice As Long
    ReadOnly Property Encumbrance As Long
    ReadOnly Property MaximumDamage As Long?
    ReadOnly Property DefendDice As Long
    ReadOnly Property MaximumDurability As Long?
    ReadOnly Property Offer As Long
    ReadOnly Property Price As Long
    ReadOnly Property RepairPrice As Long
    Sub Purify(item As IItem)
    Sub Use(character As ICharacter)
    Function CanUse(character As ICharacter) As Boolean
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
    ReadOnly Property IsWeapon As Boolean
    ReadOnly Property IsArmor As Boolean
    ReadOnly Property HasOffer(shoppeType As IShoppeType) As Boolean
    ReadOnly Property HasPrice(shoppeType As IShoppeType) As Boolean
    ReadOnly Property CanRepair(shoppeType As IShoppeType) As Boolean
End Interface
