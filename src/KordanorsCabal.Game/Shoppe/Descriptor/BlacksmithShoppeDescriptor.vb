Friend Class BlacksmithShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Sub New()
        MyBase.New(
            "Blacksmith",
            AllItemTypes.Where(
                Function(x) x.HasOffer(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Offer))
    End Sub

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return AllItemTypes.Where(
                Function(x) x.HasPrice(ShoppeType.Blacksmith)).
                ToDictionary(
                    Function(x) x,
                    Function(x) x.Price)
            'Return New Dictionary(Of ItemType, Long) From
            '    {
            '        {ItemType.BrodeSode, 100},
            '        {ItemType.ChainMail, 50},
            '        {ItemType.Dagger, 5},
            '        {ItemType.Helmet, 10},
            '        {ItemType.PlateMail, 250},
            '        {ItemType.Shield, 15},
            '        {ItemType.Shortsword, 25}
            '    }
        End Get
    End Property

    Public Overrides ReadOnly Property Repairs As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.BrodeSode, 40},
                    {ItemType.ChainMail, 20},
                    {ItemType.Dagger, 2},
                    {ItemType.Helmet, 4},
                    {ItemType.PlateMail, 100},
                    {ItemType.Shield, 6},
                    {ItemType.Shortsword, 10}
                }
        End Get
    End Property
End Class
