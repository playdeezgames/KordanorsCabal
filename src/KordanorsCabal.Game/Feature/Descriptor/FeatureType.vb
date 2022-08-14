Public Class FeatureType
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
    ReadOnly Property LocationType As LocationType
    ReadOnly Property InteractionMode As PlayerMode
    Sub New(featureTypeId As Long, name As String, locationType As LocationType, mode As PlayerMode)
        Me.Id = featureTypeId
        Me.Name = name
        Me.LocationType = locationType
        Me.InteractionMode = mode
    End Sub
End Class
Public Module FeatureTypeUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of OldFeatureType, FeatureType) =
        New Dictionary(Of OldFeatureType, FeatureType) From
        {
            {OldFeatureType.BlackMage, New FeatureType(OldFeatureType.BlackMage, "Marcus the Black Mage", LocationType.FromId(Town), PlayerMode.BlackMage)},
            {OldFeatureType.BlackMarketeer, New FeatureType(OldFeatureType.BlackMarketeer, """Honest"" Dan", LocationType.FromId(Town), PlayerMode.BlackMarket)},
            {OldFeatureType.Blacksmith, New FeatureType(OldFeatureType.Blacksmith, "Samuli the Blacksmith", LocationType.FromId(Town), PlayerMode.Blacksmith)},
            {OldFeatureType.Chicken, New FeatureType(OldFeatureType.Chicken, "Sander the Chicken", LocationType.FromId(Town), PlayerMode.Chicken)},
            {OldFeatureType.Constable, New FeatureType(OldFeatureType.Constable, "David the Constable", LocationType.FromId(Town), PlayerMode.Constable)},
            {OldFeatureType.Elder, New FeatureType(OldFeatureType.Elder, "Zooperdan the Elder", LocationType.FromId(TownSquare), PlayerMode.Elder)},
            {OldFeatureType.Healer, New FeatureType(OldFeatureType.Healer, "Nihilist Healer Marten", LocationType.FromId(Town), PlayerMode.Healer)},
            {OldFeatureType.InnKeeper, New FeatureType(OldFeatureType.InnKeeper, "Graham the Innkeeper", LocationType.FromId(Town), PlayerMode.InnKeeper)},
            {OldFeatureType.TownDrunk, New FeatureType(OldFeatureType.TownDrunk, "Yermom the Drunk", LocationType.FromId(Town), PlayerMode.TownDrunk)}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of OldFeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
End Module
