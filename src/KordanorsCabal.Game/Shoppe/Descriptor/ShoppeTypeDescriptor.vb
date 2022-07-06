MustInherit Class ShoppeTypeDescriptor
    MustOverride ReadOnly Property Name As String
End Class
Module ShopeTypeDescriptorUtility
    Friend ReadOnly ShoppeTypeDescriptors As IReadOnlyDictionary(Of ShoppeType, ShoppeTypeDescriptor) =
        New Dictionary(Of ShoppeType, ShoppeTypeDescriptor) From
        {
            {ShoppeType.Trophy, New TrophyShoppeDescriptor}
        }
End Module
