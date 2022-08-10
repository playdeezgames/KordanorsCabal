Imports System.Runtime.CompilerServices

Public Enum FeatureType
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
    Public Function Name(featureType As FeatureType) As String
        Return FeatureTypeDescriptors(featureType).Name
    End Function
    <Extension>
    Public Function LocationType(featureType As FeatureType) As OldLocationType
        Return FeatureTypeDescriptors(featureType).LocationType
    End Function
    <Extension>
    Public Function InteractionMode(featureType As FeatureType) As PlayerMode
        Return FeatureTypeDescriptors(featureType).InteractionMode
    End Function
End Module
