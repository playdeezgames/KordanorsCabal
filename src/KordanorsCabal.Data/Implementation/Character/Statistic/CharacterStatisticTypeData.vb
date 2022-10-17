Public Class CharacterStatisticTypeData
    Inherits NameCacheData
    Implements ICharacterStatisticTypeData
    Friend Const CharacterStatisticTypeIdColumn = "CharacterStatisticTypeId"
    Friend Const CharacterStatisticTypeNameColumn = "CharacterStatisticTypeName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const MinimumValueColumn = "MinimumValue"
    Friend Const DefaultValueColumn = "DefaultValue"
    Friend Const MaximumValueColumn = "MaximumValue"

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            DefaultValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function


    Public Function ReadName(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            CharacterStatisticTypeNameColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function
    Public Function ReadMaximumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMaximumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            MaximumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadMinimumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMinimumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            MinimumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadAbbreviation(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            AbbreviationColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
        lookUpByName = Function(name) store.Column.ReadValue(Of String, Long)(
            AddressOf NoInitializer,
            CharacterStatisticTypes,
            CharacterStatisticTypeIdColumn,
            (CharacterStatisticTypeNameColumn, name))
    End Sub
End Class
