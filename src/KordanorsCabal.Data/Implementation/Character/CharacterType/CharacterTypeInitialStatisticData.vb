Public Class CharacterTypeInitialStatisticData
    Inherits BaseData
    Implements ICharacterTypeInitialStatisticData
    Friend Const CharacterTypeIdColumn = CharacterTypeData.CharacterTypeIdColumn
    Friend Const CharacterStatisticTypeIdColumn = CharacterStatisticTypeData.CharacterStatisticTypeIdColumn
    Friend Const InitialValueColumn = "InitialValue"
    Public Function ReadAllForCharacterType(characterTypeId As Long) As List(Of Tuple(Of Long, Long)) Implements ICharacterTypeInitialStatisticData.ReadAllForCharacterType
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeInitialStatistics,
            (CharacterStatisticTypeIdColumn, InitialValueColumn),
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(characterTypeId As Long, statisticTypeId As Long) As Long?
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeInitialStatistics,
            InitialValueColumn,
            (CharacterTypeIdColumn, characterTypeId),
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadForCharacterType(characterTypeId As Long) As IEnumerable(Of Long)
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterTypeInitialStatistics,
            CharacterStatisticTypeIdColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function
End Class
