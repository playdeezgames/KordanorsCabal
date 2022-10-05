﻿Friend Class BlackMarketShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New(worldData As IWorldData)
        MyBase.New(worldData, "Black Market")
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of OldItemType, Long)
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.ToNew(WorldData).HasPrice(ShoppeType.BlackMarket)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.ToNew(WorldData).Price)
        End Get
    End Property
End Class
