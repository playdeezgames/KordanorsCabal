Public MustInherit Class ItemStatisticType
    MustOverride ReadOnly Property DefaultValue As Long
End Class
Module ItemStatisticTypeDescriptorUtility
    Friend ReadOnly ItemStatisticTypeDescriptors As IReadOnlyDictionary(Of OldItemStatisticType, ItemStatisticType) =
        New Dictionary(Of OldItemStatisticType, ItemStatisticType) From
        {
            {OldItemStatisticType.Wear, New WearItemStatisticTypeDescriptor}
        }
End Module