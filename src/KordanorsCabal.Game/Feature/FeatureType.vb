Imports System.Runtime.CompilerServices

Public Enum FeatureType
    None
    Elder
    InnKeeper
    TownDrunk
End Enum
Public Module FeatureTypeExtensions
    <Extension>
    Public Function Name(featureType As FeatureType) As String
        Return FeatureTypeDescriptors(featureType).Name
    End Function
    <Extension>
    Public Function LocationType(featureType As FeatureType) As LocationType
        Return FeatureTypeDescriptors(featureType).LocationType
    End Function
    <Extension>
    Public Function CanInteract(featureType As FeatureType, player As PlayerCharacter) As Boolean
        Return FeatureTypeDescriptors(featureType).CanInteract(player)
    End Function
    <Extension>
    Public Function InteractionMode(featureType As FeatureType, player As PlayerCharacter) As PlayerMode
        Return FeatureTypeDescriptors(featureType).InteractionMode(player)
    End Function
End Module
