Public Class SpellTypeDataTests
    Inherits WorldDataSubobjectTests(Of ISpellTypeData)

    Sub New()
        MyBase.New(Function(x) x.SpellType)
    End Sub
    <Fact>
    Sub spell_type_data_associates_a_name_with_a_spell_type_in_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const spellTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypes,
                    Columns.SpellTypeNameColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub spell_type_data_associates_a_maximum_level_with_a_spell_type_in_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const spellTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadMaximumLevel(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypes,
                    Columns.MaximumLevelColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub spell_type_data_associates_a_cast_check_with_a_spell_type_in_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const spellTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCastCheck(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypes,
                    Columns.CastCheckColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub spell_type_data_associates_a_cast_with_a_spell_type_in_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const spellTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCast(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypes,
                    Columns.CastColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId)))
            End Sub)
    End Sub
End Class
