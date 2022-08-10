Public Class FeatureTypeDescriptor
    ReadOnly Property Name As String
    ReadOnly Property LocationType As OldLocationType
    ReadOnly Property InteractionMode As PlayerMode
    Sub New(name As String, locationType As OldLocationType, mode As PlayerMode)
        Me.Name = name
        Me.LocationType = locationType
        Me.InteractionMode = mode
    End Sub
End Class
Public Module FeatureTypeDescriptorUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of FeatureType, FeatureTypeDescriptor) =
        New Dictionary(Of FeatureType, FeatureTypeDescriptor) From
        {
            {FeatureType.BlackMage, New FeatureTypeDescriptor("Marcus the Black Mage", OldLocationType.Town, PlayerMode.BlackMage)},
            {FeatureType.BlackMarketeer, New FeatureTypeDescriptor("""Honest"" Dan", OldLocationType.Town, PlayerMode.BlackMarket)},
            {FeatureType.Blacksmith, New FeatureTypeDescriptor("Samuli the Blacksmith", OldLocationType.Town, PlayerMode.Blacksmith)},
            {FeatureType.Chicken, New FeatureTypeDescriptor("Sander the Chicken", OldLocationType.Town, PlayerMode.Chicken)},
            {FeatureType.Constable, New FeatureTypeDescriptor("David the Constable", OldLocationType.Town, PlayerMode.Constable)},
            {FeatureType.Elder, New FeatureTypeDescriptor("Zooperdan the Elder", OldLocationType.TownSquare, PlayerMode.Elder)},
            {FeatureType.Healer, New FeatureTypeDescriptor("Nihilist Healer Marten", OldLocationType.Town, PlayerMode.Healer)},
            {FeatureType.InnKeeper, New FeatureTypeDescriptor("Graham the Innkeeper", OldLocationType.Town, PlayerMode.InnKeeper)},
            {FeatureType.TownDrunk, New FeatureTypeDescriptor("Yermom the Drunk", OldLocationType.Town, PlayerMode.TownDrunk)}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of FeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
End Module
