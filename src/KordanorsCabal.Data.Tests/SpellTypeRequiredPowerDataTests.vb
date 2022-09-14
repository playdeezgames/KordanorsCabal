﻿Public Class SpellTypeRequiredPowerDataTests
    Inherits WorldDataSubobjectTests(Of ISpellTypeRequiredPowerData)

    Sub New()
        MyBase.New(Function(x) x.SpellTypeRequiredPower)
    End Sub

    <Fact>
    Sub spell_type_required_power_data_associates_a_power_level_with_a_spell_level_for_a_given_spell_type()
        WithSubobject(
            Sub(store, subject)
                Const spellTypeId = 1L
                Const level = 2L
                subject.Read(spellTypeId, level)
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.SpellTypeRequiredPowers,
                    Columns.PowerColumn,
                    (Columns.SpellTypeIdColumn, spellTypeId),
                    (Columns.SpellLevelColumn, level)))
            End Sub)
    End Sub
End Class
