Friend Class HealerShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New("Healer")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.ToNew(StaticWorldData.World).HasPrice(ShoppeType.Healer)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(StaticWorldData.World).Price)
        End Get
    End Property
End Class
