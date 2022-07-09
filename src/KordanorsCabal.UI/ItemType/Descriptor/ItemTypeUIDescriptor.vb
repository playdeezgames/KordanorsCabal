Public MustInherit Class ItemTypeUIDescriptor
    MustOverride ReadOnly Property DisplayPattern As Pattern?
    MustOverride ReadOnly Property DisplayXY() As (Integer, Integer)?
    MustOverride ReadOnly Property DisplayHue() As Hue?
End Class
Module ItemTypeUIDescriptorUtility
    Friend ReadOnly ItemTypeUIDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeUIDescriptor) =
        New Dictionary(Of ItemType, ItemTypeUIDescriptor) From
        {
            {ItemType.AirShard, New AirShardUIDescriptor},
            {ItemType.BrodeSode, New BrodeSodeUIDescriptor},
            {ItemType.ChainMail, New ChainMainUIDescriptor},
            {ItemType.CopperKey, New CopperKeyUIDescriptor},
            {ItemType.Dagger, New DaggerUIDescriptor},
            {ItemType.EarthShard, New EarthShardUIDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbUIDescriptor},
            {ItemType.FireShard, New FireShardUIDescriptor},
            {ItemType.GoblinEar, New GoblinEarUIDescriptor},
            {ItemType.GoldKey, New GoldKeyUIDescriptor},
            {ItemType.Helmet, New HelmetUIDescriptor},
            {ItemType.IronKey, New IronKeyUIDescriptor},
            {ItemType.PlateMail, New PlateMailUIDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyUIDescriptor},
            {ItemType.Potion, New PotionUIDescriptor},
            {ItemType.RatTail, New RatTailUIDescriptor},
            {ItemType.Shield, New ShieldUIDescriptor},
            {ItemType.Shortsword, New ShortswordUIDescriptor},
            {ItemType.SilverKey, New SilverKeyUIDescriptor},
            {ItemType.SkullFragment, New SkullFragmentUIDescriptor},
            {ItemType.WaterShard, New WaterShardUIDescriptor}
        }
End Module