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
                    {ItemType.BatWing, 3},
                    {ItemType.GoblinEar, 5},
                    {ItemType.MagicEgg, 10},
                    {ItemType.Mushroom, 25},
                    {ItemType.RatTail, 1},
                    {ItemType.SkullFragment, 5},
                    {ItemType.SnakeFang, 3},
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
                    {ItemType.MoonPortal, 5000},
                    {ItemType.TownPortal, 50}
                }
        End Get
    End Property
End Class
