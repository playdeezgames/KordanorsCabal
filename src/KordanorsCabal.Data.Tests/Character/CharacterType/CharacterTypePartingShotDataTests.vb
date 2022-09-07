Public Class CharacterTypePartingShotDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypePartingShotData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypePartingShot)
    End Sub

    <Fact>
    Sub ShouldReadAPartingShotGeneratorForAGivenCharacterType()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.Read(characterType).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValue(Of Long, String, Long)(
                    It.IsAny(Of Action),
                    Tables.CharacterTypePartingShots,
                    (Columns.PartingShotColumn, Columns.WeightColumn),
                    (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
