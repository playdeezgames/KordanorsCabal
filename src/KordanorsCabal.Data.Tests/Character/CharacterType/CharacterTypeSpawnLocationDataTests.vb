Public Class CharacterTypeSpawnLocationDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeSpawnLocationData)
    Sub New()
        MyBase.New(Function(x) x.CharacterTypeSpawnLocation)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForValidCombinationsOfCharacterAndDungeonLevelAndLocationType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterTypeId = 1L
                Dim dungeonLevel = 2L
                Dim locationTYpe = 3L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(characterTypeId, dungeonLevel, locationTYpe).ShouldBeFalse
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.CharacterTypeSpawnLocations,
                    Columns.CharacterTypeIdColumn,
                    (Columns.CharacterTypeIdColumn, characterTypeId),
                    (Columns.DungeonLevelIdColumn, dungeonLevel),
                    (Columns.LocationTypeIdColumn, locationTYpe)))
            End Sub)
    End Sub
End Class
