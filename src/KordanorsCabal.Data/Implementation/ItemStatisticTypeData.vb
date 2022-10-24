Public Class ItemStatisticTypeData
    Inherits BaseData
    Implements IItemStatisticTypeData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements IItemStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            DefaultValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function
End Class
