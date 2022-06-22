Imports System.Runtime.CompilerServices

Public Enum FeatureType
    None
    Elder
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
End Module
