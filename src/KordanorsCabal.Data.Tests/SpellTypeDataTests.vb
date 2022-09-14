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
                subject.ReadName(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long)(
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
                subject.ReadMaximumLevel(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
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
                subject.ReadCastCheck(spellTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypes,
                    Columns.CastCheckColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId)))
            End Sub)
    End Sub
End Class
