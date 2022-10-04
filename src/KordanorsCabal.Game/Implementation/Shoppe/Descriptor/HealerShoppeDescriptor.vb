Friend Class HealerShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, "Healer")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.ToNew(WorldData).HasPrice(ShoppeType.Healer)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(WorldData).Price)
        End Get
    End Property
End Class
