Friend Class SpellTypeRequiredPowerData
    Inherits BaseData
    Implements ISpellTypeRequiredPowerData

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(spellTypeId As Long, level As Long) As Long? Implements ISpellTypeRequiredPowerData.Read
        Throw New NotImplementedException()
    End Function
End Class
