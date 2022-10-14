Public Class CharacterLocationDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterLocationData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterLocation)
    End Sub

    <Fact>
    Sub ShouldClearLocationsForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                                 It.IsAny(Of Action),
                                 Tables.CharacterLocations,
                                 (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForACharacterAndLocationCombination()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const locationId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(characterId, locationId).ShouldBeFalse
                store.Verify(Function(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                                It.IsAny(Of Action),
                                Tables.CharacterLocations,
                                CharacterIdColumn,
                                (CharacterIdColumn, characterId),
                                (LocationIdColumn, locationId)),
                                Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreSoThatACharacterIsAssociatedWithALocation()
        WithSubobject(
            Sub(store, checker, subject)
                Const characterId = 1L
                Const locationId = 2L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, locationId)
                store.Verify(Sub(x) x.Replace.ReplaceRecord(Of Long, Long)(
                                It.IsAny(Of Action),
                                Tables.CharacterLocations,
                                (CharacterIdColumn, characterId),
                                (LocationIdColumn, locationId)),
                                Times.Once)
            End Sub)
    End Sub
End Class
