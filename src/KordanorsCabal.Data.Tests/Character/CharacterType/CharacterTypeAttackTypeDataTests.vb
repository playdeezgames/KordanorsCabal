Public Class CharacterTypeAttackTypeDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeAttackTypeData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypeAttackType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAnAttackTypeGeneratorForACharacterType()
        WithSubobject(
            Sub(store, subject)
                Dim characterTypeId = 1L
                subject.Read(characterTypeId).ShouldBeNull
                store.Verify(Function(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeAttackTypes,
                                 (AttackTypeColumn, WeightColumn),
                                 (CharacterTypeIdColumn, characterTypeId)))
            End Sub)
    End Sub
End Class
