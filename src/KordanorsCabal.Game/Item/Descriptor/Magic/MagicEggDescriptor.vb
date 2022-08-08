Friend Class MagicEggDescriptor
    Inherits TrophyDescriptor

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return True
        End Get
    End Property

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
        MyBase.New("Magic Egg", 100, New List(Of ShoppeType) From {ShoppeType.BlackMage})
    End Sub

    Public Overrides Sub Use(character As Character)
        Dim item = Game.Item.Create(RNG.FromGenerator(table))
        character.EnqueueMessage($"You crack open the {ItemType.MagicEgg.Name} and find {item.Name} inside!")
        character.Inventory.Add(item)
    End Sub
End Class
