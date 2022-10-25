Public Class StatisticTypeData
    Inherits NameCacheData
    Implements IStatisticTypeData
    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements IStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            StatisticTypes,
            DefaultValueColumn,
            (StatisticTypeIdColumn, statisticTypeId))
    End Function


    Public Function ReadName(statisticTypeId As Long) As String Implements IStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            StatisticTypes,
            CharacterStatisticTypeNameColumn,
            (StatisticTypeIdColumn, statisticTypeId))
    End Function
    Public Function ReadMaximumValue(statisticTypeId As Long) As Long? Implements IStatisticTypeData.ReadMaximumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            StatisticTypes,
            MaximumValueColumn,
            (StatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadMinimumValue(statisticTypeId As Long) As Long? Implements IStatisticTypeData.ReadMinimumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            StatisticTypes,
            MinimumValueColumn,
            (StatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadAbbreviation(statisticTypeId As Long) As String Implements IStatisticTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            StatisticTypes,
            AbbreviationColumn,
            (StatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
        lookUpByName = Function(name) store.Column.ReadValue(Of String, Long)(
            AddressOf NoInitializer,
            StatisticTypes,
            StatisticTypeIdColumn,
            (CharacterStatisticTypeNameColumn, name))
    End Sub
End Class
