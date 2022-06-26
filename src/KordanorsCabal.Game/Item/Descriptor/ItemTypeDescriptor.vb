﻿Public MustInherit Class ItemTypeDescriptor
    MustOverride ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
    MustOverride ReadOnly Property Name() As String
End Class
Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {ItemType.CopperKey, New CopperKeyDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbDescriptor},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor}
        }
End Module
