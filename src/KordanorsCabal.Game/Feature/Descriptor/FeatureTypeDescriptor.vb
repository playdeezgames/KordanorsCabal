﻿Public MustInherit Class FeatureTypeDescriptor
    MustOverride ReadOnly Property Name As String
    MustOverride ReadOnly Property LocationType As LocationType
    MustOverride Function CanInteract(player As Character) As Boolean
    MustOverride Function InteractionMode(player As Character) As PlayerMode
End Class
Public Module FeatureTypeDescriptorUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of FeatureType, FeatureTypeDescriptor) =
        New Dictionary(Of FeatureType, FeatureTypeDescriptor) From
        {
            {FeatureType.BlackMage, New BlackMageDescriptor},
            {FeatureType.BlackMarketeer, New BlackMarketeerDescriptor},
            {FeatureType.Blacksmith, New BlacksmithDescriptor},
            {FeatureType.Chicken, New ChickenDescriptor},
            {FeatureType.Constable, New ConstableDescriptor},
            {FeatureType.Elder, New ElderDescriptor},
            {FeatureType.Healer, New HealerDescriptor},
            {FeatureType.InnKeeper, New InnKeeperDescriptor},
            {FeatureType.TownDrunk, New TownDrunkDescriptor}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of FeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
End Module
