MustInherit Class ShoppeTypeDescriptor
    MustOverride ReadOnly Property Name As String

    MustOverride ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
    MustOverride ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)

    Overridable ReadOnly Property Repairs() As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long)
        End Get
    End Property
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
