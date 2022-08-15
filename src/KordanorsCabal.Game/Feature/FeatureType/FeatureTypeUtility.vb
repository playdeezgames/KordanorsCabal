Imports System.Runtime.CompilerServices

Public Enum OldFeatureType
    None
    Elder
    InnKeeper
    TownDrunk
    Chicken
    BlackMarketeer
    BlackMage
    Blacksmith
    Healer
    Constable
End Enum
Public Module FeatureTypeExtensions
    <Extension>
    Public Function Name(featureType As OldFeatureType) As String
        Return FeatureTypeDescriptors(featureType).Name
    End Function
    <Extension>
    Public Function LocationType(featureType As OldFeatureType) As LocationType
        Return FeatureTypeDescriptors(featureType).LocationType
    End Function
    <Extension>
    Public Function InteractionMode(featureType As OldFeatureType) As PlayerMode
        Return FeatureTypeDescriptors(featureType).InteractionMode
    End Function
End Module
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
    Public Const Elder = 1L
    Public Const InnKeeper = 2L
    Public Const TownDrunk = 3L
    Public Const Chicken = 4L
    Public Const BlackMarketeer = 5L
    Public Const BlackMage = 6L
    Public Const Blacksmith = 7L
    Public Const Healer = 8L
    Public Const Constable = 9L
End Module
