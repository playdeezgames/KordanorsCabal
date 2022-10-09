Imports SQLitePCL

Public Class ShoppeType
    Inherits BaseThingie
    Implements IShoppeType
    ReadOnly Property Name As String Implements IShoppeType.Name
        Get
            Return WorldData.ShoppeType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Offers As IReadOnlyDictionary(Of IItemType, Long) Implements IShoppeType.Offers
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasOffer(Me)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer)
        End Get
    End Property
    ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long) Implements IShoppeType.Prices
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.HasPrice(Me)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
        End Get
    End Property
    ReadOnly Property Repairs As IReadOnlyDictionary(Of IItemType, Long) Implements IShoppeType.Repairs
        Get
            Return AllItemTypes(WorldData).Where(
                Function(x) x.CanRepair(Me)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.RepairPrice)
        End Get
    End Property

    Sub New(
           worldData As IWorldData,
           id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IShoppeType
        Return If(id.HasValue, New ShoppeType(worldData, id.Value), Nothing)
    End Function

    Public Function BuyPrice(itemType As IItemType) As Long? Implements IShoppeType.BuyPrice
        Dim value As Long
        If Offers.ToDictionary(Function(x) x.Key.Id, Function(x) x.Value).TryGetValue(itemType.Id, value) Then
            Return value
        End If
        Return Nothing
    End Function

    Public Function WillBuy(itemType As IItemType) As Boolean Implements IShoppeType.WillBuy
        Return BuyPrice(itemType).HasValue
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
