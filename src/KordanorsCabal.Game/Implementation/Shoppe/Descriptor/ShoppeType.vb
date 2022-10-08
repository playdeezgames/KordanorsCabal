﻿MustInherit Class ShoppeType
    Inherits BaseThingie
    ReadOnly Property Name As String
    ReadOnly Property Offers As IReadOnlyDictionary(Of IItemType, Long)
    MustOverride ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)

    Overridable ReadOnly Property Repairs() As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return New Dictionary(Of IItemType, Long)
        End Get
    End Property
    Sub New(
           worldData As IWorldData,
           id As Long,
           name As String,
           Optional offers As IReadOnlyDictionary(Of IItemType, Long) = Nothing)
        MyBase.New(worldData, id)
        Me.Name = name
        Me.Offers = If(offers, New Dictionary(Of IItemType, Long))
    End Sub
End Class
Module ShopeTypeDescriptorUtility
    Friend ReadOnly AllShoppeTypes As IReadOnlyDictionary(Of OldShoppeType, ShoppeType) =
        New Dictionary(Of OldShoppeType, ShoppeType) From
        {
            {OldShoppeType.BlackMage, New BlackMageShoppeDescriptor(StaticWorldData.World)},
            {OldShoppeType.BlackMarket, New BlackMarketShoppeDescriptor(StaticWorldData.World)},
            {OldShoppeType.Blacksmith, New BlacksmithShoppeDescriptor(StaticWorldData.World)},
            {OldShoppeType.Healer, New HealerShoppeDescriptor(StaticWorldData.World)},
            {OldShoppeType.InnKeeper, New InnkeeperShoppeDescriptor(StaticWorldData.World)}
        }
End Module