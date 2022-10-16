Public Class CharacterStatisticTypeData
    Inherits NameCacheData
    Implements ICharacterStatisticTypeData
    Friend Const TableName = "CharacterStatisticTypes"
    Friend Const CharacterStatisticTypeIdColumn = "CharacterStatisticTypeId"
    Friend Const CharacterStatisticTypeNameColumn = "CharacterStatisticTypeName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const MinimumValueColumn = "MinimumValue"
    Friend Const DefaultValueColumn = "DefaultValue"
    Friend Const MaximumValueColumn = "MaximumValue"

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            DefaultValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function


    Public Function ReadName(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            CharacterStatisticTypeNameColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function
    Public Function ReadMaximumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMaximumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            MaximumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadMinimumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMinimumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            MinimumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadAbbreviation(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            AbbreviationColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
        lookUpByName = Function(name) store.Column.ReadValue(Of String, Long)(
            AddressOf NoInitializer,
            TableName,
            CharacterStatisticTypeIdColumn,
            (CharacterStatisticTypeNameColumn, name))
    End Sub
End Class
