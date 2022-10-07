Public Class Weapon
    Inherits BaseThingie
    Implements IWeapon
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IWeapon
        Return If(id.HasValue, New Weapon(worldData, id.Value), Nothing)
    End Function
End Class
