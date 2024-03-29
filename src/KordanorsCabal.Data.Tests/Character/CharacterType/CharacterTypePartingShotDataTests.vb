﻿Public Class CharacterTypePartingShotDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypePartingShotData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypePartingShot)
    End Sub

    <Fact>
    Sub ShouldReadAPartingShotGeneratorForAGivenCharacterType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.Read(characterType).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.WithValue(Of Long, String, Long)(
                    It.IsAny(Of Action),
                    Tables.CharacterTypePartingShots,
                    (Columns.PartingShotColumn, Columns.WeightColumn),
                    (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
