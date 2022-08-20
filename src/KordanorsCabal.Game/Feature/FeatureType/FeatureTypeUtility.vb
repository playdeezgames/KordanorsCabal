Public Module FeatureTypeUtility
    Public ReadOnly Property AllFeatureTypes(worldData As WorldData) As IEnumerable(Of FeatureType)
        Get
            Return worldData.FeatureType.ReadAll().Select(Function(x) New FeatureType(x))
        End Get
    End Property
    Public Const Constable = 9L
End Module
