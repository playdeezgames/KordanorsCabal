Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property Spawn As IItemTypeSpawn
    ReadOnly Property Equip As IItemTypeEquip
    ReadOnly Property Combat As IItemTypeCombat
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
    Sub Use(character As ICharacter, item As IItem)
    Function CanUse(character As ICharacter) As Boolean
End Interface
