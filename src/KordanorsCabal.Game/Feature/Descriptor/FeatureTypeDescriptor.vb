Public MustInherit Class FeatureTypeDescriptor
    MustOverride ReadOnly Property Name As String
    MustOverride ReadOnly Property LocationType As LocationType
End Class
Public Module FeatureTypeDescriptorUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of FeatureType, FeatureTypeDescriptor) =
        New Dictionary(Of FeatureType, FeatureTypeDescriptor) From
        {
            {FeatureType.Elder, New ElderDescriptor}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of FeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
End Module
