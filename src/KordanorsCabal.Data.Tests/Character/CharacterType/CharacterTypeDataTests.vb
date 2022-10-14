Public Class CharacterTypeDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeData)
    Public Sub New()
        MyBase.New(Function(x) x.CharacterType)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForAllCharacterTypes()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecords(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.CharacterTypeIdColumn))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToDetermineWhetherOrNotACharacterTypeIsUndead()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadIsUndead(characterType).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.IsUndeadColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersMoneyDropDice()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadMoneyDropDice(characterType).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.MoneyDropDiceColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersName()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(characterType).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.CharacterTypeNameColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersXPValue()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadXPValue(characterType).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.XPValueColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
