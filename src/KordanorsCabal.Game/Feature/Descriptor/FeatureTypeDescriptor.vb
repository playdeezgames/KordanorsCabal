Public Class FeatureTypeDescriptor
    ReadOnly Property Name As String
    ReadOnly Property LocationType As LocationType
    ReadOnly Property InteractionMode As PlayerMode
    Sub New(name As String, locationType As LocationType, mode As PlayerMode)
        Me.Name = name
        Me.LocationType = locationType
        Me.InteractionMode = mode
    End Sub
End Class
Public Module FeatureTypeDescriptorUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of FeatureType, FeatureTypeDescriptor) =
        New Dictionary(Of FeatureType, FeatureTypeDescriptor) From
        {
            {FeatureType.BlackMage, New FeatureTypeDescriptor("Marcus the Black Mage", LocationType.FromName(Town), PlayerMode.BlackMage)},
            {FeatureType.BlackMarketeer, New FeatureTypeDescriptor("""Honest"" Dan", LocationType.FromName(Town), PlayerMode.BlackMarket)},
            {FeatureType.Blacksmith, New FeatureTypeDescriptor("Samuli the Blacksmith", LocationType.FromName(Town), PlayerMode.Blacksmith)},
            {FeatureType.Chicken, New FeatureTypeDescriptor("Sander the Chicken", LocationType.FromName(Town), PlayerMode.Chicken)},
            {FeatureType.Constable, New FeatureTypeDescriptor("David the Constable", LocationType.FromName(Town), PlayerMode.Constable)},
            {FeatureType.Elder, New FeatureTypeDescriptor("Zooperdan the Elder", LocationType.FromName(TownSquare), PlayerMode.Elder)},
            {FeatureType.Healer, New FeatureTypeDescriptor("Nihilist Healer Marten", LocationType.FromName(Town), PlayerMode.Healer)},
            {FeatureType.InnKeeper, New FeatureTypeDescriptor("Graham the Innkeeper", LocationType.FromName(Town), PlayerMode.InnKeeper)},
            {FeatureType.TownDrunk, New FeatureTypeDescriptor("Yermom the Drunk", LocationType.FromName(Town), PlayerMode.TownDrunk)}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of FeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
End Module
