Public Class ShoppeType
    Inherits BaseThingie
    Implements IShoppeType
    ReadOnly Property Name As String Implements IShoppeType.Name
        Get
            Return WorldData.ShoppeType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Offers As IReadOnlyDictionary(Of IItemType, Long)
    Overridable ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return New Dictionary(Of IItemType, Long)
        End Get
    End Property
    Overridable ReadOnly Property Repairs() As IReadOnlyDictionary(Of IItemType, Long)
        Get
            Return New Dictionary(Of IItemType, Long)
        End Get
    End Property
    Sub New(
           worldData As IWorldData,
           id As Long,
           Optional offers As IReadOnlyDictionary(Of IItemType, Long) = Nothing)
        MyBase.New(worldData, id)
        Me.Offers = If(offers, New Dictionary(Of IItemType, Long))
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IShoppeType
        Return If(id.HasValue, New ShoppeType(worldData, id.Value), Nothing)
    End Function
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
