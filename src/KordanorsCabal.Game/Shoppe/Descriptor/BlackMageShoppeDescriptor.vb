Friend Class BlackMageShoppeDescriptor
    Inherits ShoppeTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Magic"
        End Get
    End Property

    Public Overrides ReadOnly Property Offers As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.GoblinEar, 5},
                    {ItemType.MagicEgg, 5},
                    {ItemType.Mushroom, 5},
                    {ItemType.RatTail, 1},
                    {ItemType.SkullFragment, 5},
                    {ItemType.ZombieTaint, 5}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Prices As IReadOnlyDictionary(Of ItemType, Long)
        Get
            Return New Dictionary(Of ItemType, Long) From
                {
                    {ItemType.Bong, 50},
                    {ItemType.BookOfHolyBolt, 100},
                    {ItemType.Herb, 10},
                    {ItemType.TownPortal, 50}
                }
        End Get
    End Property
End Class
