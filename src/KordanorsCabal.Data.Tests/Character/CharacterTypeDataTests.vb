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
End Class
