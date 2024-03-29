﻿Public Interface IItemType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property Spawn As IItemTypeSpawn
    ReadOnly Property Equip As IItemTypeEquip
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
End Interface
