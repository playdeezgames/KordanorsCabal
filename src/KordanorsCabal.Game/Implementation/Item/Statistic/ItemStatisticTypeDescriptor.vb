Public MustInherit Class ItemStatisticTypeDescriptor
    MustOverride ReadOnly Property DefaultValue As Long
End Class
Module ItemStatisticTypeDescriptorUtility
    Friend ReadOnly ItemStatisticTypeDescriptors As IReadOnlyDictionary(Of ItemStatisticType, ItemStatisticTypeDescriptor) =
        New Dictionary(Of ItemStatisticType, ItemStatisticTypeDescriptor) From
        {
            {ItemStatisticType.Wear, New WearItemStatisticTypeDescriptor}
        }
End Module