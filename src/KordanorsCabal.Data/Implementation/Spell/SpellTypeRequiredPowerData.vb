Friend Class SpellTypeRequiredPowerData
    Inherits BaseData
    Implements ISpellTypeRequiredPowerData
    Friend Const SpellTypeIdColumn = SpellTypeData.SpellTypeIdColumn
    Friend Const SpellLevelColumn = "SpellLevel"
    Friend Const PowerColumn = "Power"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(spellTypeId As Long, level As Long) As Long? Implements ISpellTypeRequiredPowerData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            SpellTypeRequiredPowers,
            PowerColumn,
            (SpellTypeIdColumn, spellTypeId),
            (SpellLevelColumn, level))
    End Function
End Class
