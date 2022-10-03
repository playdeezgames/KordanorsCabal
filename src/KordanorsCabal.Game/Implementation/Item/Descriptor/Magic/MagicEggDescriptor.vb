Friend Class MagicEggDescriptor
    Inherits TrophyDescriptor

    Private Shared table As IReadOnlyDictionary(Of OldItemType, Integer) =
        New Dictionary(Of OldItemType, Integer) From
        {
            {OldItemType.Beer, 500},
            {OldItemType.BrodeSode, 8},
            {OldItemType.ChainMail, 4},
            {OldItemType.Dagger, 250},
            {OldItemType.Food, 1000},
            {OldItemType.Helmet, 125},
            {OldItemType.HolyWater, 64},
            {OldItemType.MoonPortal, 1},
            {OldItemType.PlateMail, 2},
            {OldItemType.Potion, 125},
            {OldItemType.Shield, 64},
            {OldItemType.Shortsword, 16},
            {OldItemType.TownPortal, 8},
            {OldItemType.Trousers, 1}
        }

    Public Sub New()
        MyBase.New(
            OldItemType.MagicEgg,
            MakeList(ShoppeType.BlackMage),,,
            "AlwaysTrue",
            "UseMagicEgg")
    End Sub
End Class
