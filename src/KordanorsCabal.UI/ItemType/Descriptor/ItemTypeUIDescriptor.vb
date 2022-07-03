Public MustInherit Class ItemTypeUIDescriptor
    MustOverride ReadOnly Property DisplayPattern As Pattern?
    MustOverride ReadOnly Property DisplayXY() As (Integer, Integer)?
    MustOverride ReadOnly Property DisplayHue() As Hue?
End Class
Module ItemTypeUIDescriptorUtility
    Friend ReadOnly ItemTypeUIDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeUIDescriptor) =
        New Dictionary(Of ItemType, ItemTypeUIDescriptor) From
        {
            {ItemType.CopperKey, New CopperKeyUIDescriptor},
            {ItemType.Dagger, New DaggerUIDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbUIDescriptor},
            {ItemType.GoblinEar, New GoblinEarUIDescriptor},
            {ItemType.GoldKey, New GoldKeyUIDescriptor},
            {ItemType.IronKey, New IronKeyUIDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyUIDescriptor},
            {ItemType.Potion, New PotionUIDescriptor},
            {ItemType.SilverKey, New SilverKeyUIDescriptor},
            {ItemType.SkullFragment, New SkullFragmentUIDescriptor}
        }
End Module