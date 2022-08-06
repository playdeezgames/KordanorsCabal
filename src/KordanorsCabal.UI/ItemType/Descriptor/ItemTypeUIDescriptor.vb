Public Class ItemTypeUIDescriptor
    Sub New(pattern As Pattern?, xy As (Integer, Integer)?, hue As Hue?)
        DisplayPattern = pattern
        DisplayXY = xy
        DisplayHue = hue
    End Sub
    Overridable ReadOnly Property DisplayPattern As Pattern?
    Overridable ReadOnly Property DisplayXY As (Integer, Integer)?
    Overridable ReadOnly Property DisplayHue As Hue?
End Class
Module ItemTypeUIDescriptorUtility
    Friend ReadOnly ItemTypeUIDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeUIDescriptor) =
        New Dictionary(Of ItemType, ItemTypeUIDescriptor) From
        {
            {ItemType.AirShard, New ItemTypeUIDescriptor(Pattern.A, (10, 15), Hue.Cyan)},
            {ItemType.AmuletOfHP, New AmuletOfHPUIDescriptor},
            {ItemType.BatWing, New BatWingUIDescriptor},
            {ItemType.Beer, New BeerUIDescriptor},
            {ItemType.Bong, New BongUIDescriptor},
            {ItemType.BookOfHolyBolt, New BookOfHolyBoltUIDescriptor},
            {ItemType.BookOfPurify, New BookOfPurifyUIDescriptor},
            {ItemType.Bottle, New BottleUIDescriptor},
            {ItemType.BrodeSode, New BrodeSodeUIDescriptor},
            {ItemType.ChainMail, New ChainMainUIDescriptor},
            {ItemType.CopperKey, New CopperKeyUIDescriptor},
            {ItemType.Dagger, New DaggerUIDescriptor},
            {ItemType.EarthShard, New EarthShardUIDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbUIDescriptor},
            {ItemType.FireShard, New FireShardUIDescriptor},
            {ItemType.Food, New FoodUIDescriptor},
            {ItemType.GoblinEar, New GoblinEarUIDescriptor},
            {ItemType.GoldKey, New GoldKeyUIDescriptor},
            {ItemType.Helmet, New HelmetUIDescriptor},
            {ItemType.Herb, New HerbUIDescriptor},
            {ItemType.HolyWater, New HolyWaterUIDescriptor},
            {ItemType.HornsOfKordanor, New HornsOfKordanorUIDescriptor},
            {ItemType.IronKey, New IronKeyUIDescriptor},
            {ItemType.Lotion, New LotionUIDescriptor},
            {ItemType.MagicEgg, New MagicEggUIDescriptor},
            {ItemType.MembershipCard, New MembershipCardUIDescriptor},
            {ItemType.MoonPortal, New MoonPortalUIDescriptor},
            {ItemType.Mushroom, New MushroomUIDescriptor},
            {ItemType.PlateMail, New PlateMailUIDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyUIDescriptor},
            {ItemType.Potion, New PotionUIDescriptor},
            {ItemType.Pr0n, New Pr0nUIDescriptor},
            {ItemType.RatTail, New RatTailUIDescriptor},
            {ItemType.RingOfHP, New RingOfHPUIDescriptor},
            {ItemType.RottenEgg, New RottenEggUIDescriptor},
            {ItemType.RottenFood, New RottenFoodUIDescriptor},
            {ItemType.Shield, New ShieldUIDescriptor},
            {ItemType.ShoeLaces, New ShoeLacesUIDescriptor},
            {ItemType.Shortsword, New ShortswordUIDescriptor},
            {ItemType.SilverKey, New SilverKeyUIDescriptor},
            {ItemType.SkullFragment, New SkullFragmentUIDescriptor},
            {ItemType.SnakeFang, New SnakeFangUIDescriptor},
            {ItemType.SpaceSord, New SpaceSordUIDescriptor},
            {ItemType.TownPortal, New TownPortalUIDescriptor},
            {ItemType.Trousers, New TrousersUIDescriptor},
            {ItemType.WaterShard, New WaterShardUIDescriptor},
            {ItemType.ZombieTaint, New ZombieTaintUIDescriptor}
        }
End Module