﻿MustInherit Class ShoppeTypeDescriptor
    ReadOnly Property Name As String

    ReadOnly Property Offers As IReadOnlyDictionary(Of OldItemType, Long)
    MustOverride ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)

    Overridable ReadOnly Property Repairs() As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return New Dictionary(Of OldItemType, Long)
        End Get
    End Property
    Sub New(
           name As String,
           Optional offers As IReadOnlyDictionary(Of OldItemType, Long) = Nothing)
        Me.Name = name
        Me.Offers = If(offers, New Dictionary(Of OldItemType, Long))
    End Sub
End Class
Module ShopeTypeDescriptorUtility
    Friend ReadOnly ShoppeTypeDescriptors As IReadOnlyDictionary(Of ShoppeType, ShoppeTypeDescriptor) =
        New Dictionary(Of ShoppeType, ShoppeTypeDescriptor) From
        {
            {ShoppeType.BlackMage, New BlackMageShoppeDescriptor},
            {ShoppeType.BlackMarket, New BlackMarketShoppeDescriptor},
            {ShoppeType.Blacksmith, New BlacksmithShoppeDescriptor},
            {ShoppeType.Healer, New HealerShoppeDescriptor},
            {ShoppeType.InnKeeper, New InnkeeperShoppeDescriptor}
        }
End Module
