Public Class ItemStatisticType
    Inherits BaseThingie
    Implements IItemStatisticType
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IItemStatisticType
        Return If(id.HasValue, New ItemStatisticType(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property DefaultValue As Long? Implements IItemStatisticType.DefaultValue
        Get
            Return WorldData.ItemStatisticType.ReadDefaultValue(Id)
        End Get
    End Property
End Class
Module ItemStatisticTypeDescriptorUtility
    Friend ReadOnly Property ItemStatisticTypeDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of OldItemStatisticType, ItemStatisticType)
        Get
            Return New Dictionary(Of OldItemStatisticType, ItemStatisticType) From
            {
                {OldItemStatisticType.Wear, New WearItemStatisticTypeDescriptor(worldData, OldItemStatisticType.Wear)}
            }
        End Get
    End Property
End Module