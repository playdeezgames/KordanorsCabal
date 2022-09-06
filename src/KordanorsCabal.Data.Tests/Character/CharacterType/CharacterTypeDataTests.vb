Public Class CharacterTypeDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeData)
    Public Sub New()
        MyBase.New(Function(x) x.CharacterType)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheStoreForAllCharacterTypes()
        WithSubobject(
            Sub(store, subject)
                subject.ReadAll().ShouldBeNull
                store.Verify(Function(x) x.ReadRecords(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.CharacterTypeIdColumn))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToDetermineWhetherOrNotACharacterTypeIsUndead()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.ReadIsUndead(characterType).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.IsUndeadColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersMoneyDropDice()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.ReadMoneyDropDice(characterType).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.MoneyDropDiceColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersName()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.ReadName(characterType).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.CharacterTypeNameColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
    <Fact>
    Public Sub ShouldQueryTheDataStoreToFindACharactersXPValue()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.ReadXPValue(characterType).ShouldBeNull
                store.Verify(Function(x) x.ReadColumnValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypes,
                                 Columns.XPValueColumn,
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
