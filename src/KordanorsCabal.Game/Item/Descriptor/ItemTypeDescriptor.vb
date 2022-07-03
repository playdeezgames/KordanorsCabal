Public MustInherit Class ItemTypeDescriptor
    MustOverride ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
    MustOverride ReadOnly Property Name As String
    Overridable ReadOnly Property Encumbrance As Single
        Get
            Return 0!
        End Get
    End Property
    Overridable ReadOnly Property PurchasePrice() As Long?
        Get
            Return Nothing
        End Get
    End Property

    Overridable ReadOnly Property CanUse As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Sub Use(character As Character)
        'nothing, by default
    End Sub

    Overridable Function RollSpawnCount(level As Long) As Long
        Return 0
    End Function
End Class
Public Module ItemTypeDescriptorUtility
    Friend ReadOnly ItemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {ItemType.CopperKey, New CopperKeyDescriptor},
            {ItemType.Dagger, New DaggerDescriptor},
            {ItemType.ElementalOrb, New ElementalOrbDescriptor},
            {ItemType.GoblinEar, New GoblinEarDescriptor},
            {ItemType.GoldKey, New GoldKeyDescriptor},
            {ItemType.IronKey, New IronKeyDescriptor},
            {ItemType.PlatinumKey, New PlatinumKeyDescriptor},
            {ItemType.Potion, New PotionDescriptor},
            {ItemType.SilverKey, New SilverKeyDescriptor},
            {ItemType.SkullFragment, New SkullFragmentDescriptor}
        }
    Public ReadOnly Property AllItemTypes As IEnumerable(Of ItemType)
        Get
            Return ItemTypeDescriptors.Keys
        End Get
    End Property
End Module
