Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property Encumbrance As Long
    'spawn
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType)
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long
    'equipment
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long?
    'attacking
    ReadOnly Property AttackDice As Long
    ReadOnly Property IsWeapon As Boolean
    ReadOnly Property MaximumDamage As Long?
    'defending
    ReadOnly Property DefendDice As Long
    ReadOnly Property IsArmor As Boolean
    'offers
    ReadOnly Property Offer As Long
    ReadOnly Property HasOffer(shoppeType As IShoppeType) As Boolean
    'prices
    ReadOnly Property Price As Long
    ReadOnly Property HasPrice(shoppeType As IShoppeType) As Boolean
    'repairs
    ReadOnly Property RepairPrice As Long
    ReadOnly Property CanRepair(shoppeType As IShoppeType) As Boolean
    ReadOnly Property MaximumDurability As Long?
    'events
    Sub Purify(item As IItem)
    Sub Decay(item As IItem)
    Sub Use(character As ICharacter)
    Function CanUse(character As ICharacter) As Boolean
End Interface
