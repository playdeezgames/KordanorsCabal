Public Class DirectionDataTests
    Inherits WorldDataSubobjectTests(Of IDirectionData)
    Sub New()
        MyBase.New(Function(x) x.Direction)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheAbbreviationOfADirection()
        WithSubobject(
            Sub(store, subject)
                Dim direction = 1L
                subject.ReadAbbreviation(direction).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.Directions,
                    Columns.AbbreviationColumn,
                    (Columns.DirectionIdColumn, direction)))
            End Sub)
    End Sub
End Class
