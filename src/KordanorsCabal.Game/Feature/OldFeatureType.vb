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
