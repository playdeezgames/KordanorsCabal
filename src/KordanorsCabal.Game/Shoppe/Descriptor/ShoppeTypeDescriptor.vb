MustInherit Class ShoppeTypeDescriptor
    MustOverride ReadOnly Property Name As String

    MustOverride ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
End Class
Module ShopeTypeDescriptorUtility
    Friend ReadOnly ShoppeTypeDescriptors As IReadOnlyDictionary(Of ShoppeType, ShoppeTypeDescriptor) =
        New Dictionary(Of ShoppeType, ShoppeTypeDescriptor) From
        {
            {ShoppeType.Trophy, New TrophyShoppeDescriptor},
            {ShoppeType.UsedMetal, New UsedMetalShoppeDescriptor}
        }
End Module
