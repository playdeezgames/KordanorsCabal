Friend Class MagicEggDescriptor
    Inherits TrophyDescriptor

    Private Shared table As IReadOnlyDictionary(Of ItemType, Integer) =
        New Dictionary(Of ItemType, Integer) From
        {
            {ItemType.Beer, 500},
            {ItemType.BrodeSode, 8},
            {ItemType.ChainMail, 4},
            {ItemType.Dagger, 250},
            {ItemType.Food, 1000},
            {ItemType.Helmet, 125},
            {ItemType.HolyWater, 64},
            {ItemType.MoonPortal, 1},
            {ItemType.PlateMail, 2},
            {ItemType.Potion, 125},
            {ItemType.Shield, 64},
            {ItemType.Shortsword, 16},
            {ItemType.TownPortal, 8},
            {ItemType.Trousers, 1}
        }

    Public Sub New()
        MyBase.New(
            ItemType.MagicEgg,
            100,
            MakeList(ShoppeType.BlackMage),,,,,,
            Function(character) True,
            "UseMagicEgg")
    End Sub
End Class
