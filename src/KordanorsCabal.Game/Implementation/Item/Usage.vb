Public Class Usage
    Inherits BaseThingie
    Implements IUsage

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IUsage
        Return If(id.HasValue, New Usage(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property CanUse(character As ICharacter) As Boolean Implements IUsage.CanUse
        Get
            Return Item.FromId(WorldData, Id).ItemType.CanUse(character)
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IUsage.IsConsumed
        Get
            Return Item.FromId(WorldData, Id).ItemType.IsConsumed
        End Get
    End Property
    Friend Sub Use(character As ICharacter) Implements IUsage.Use
        Item.FromId(WorldData, Id).ItemType.Use(character)
    End Sub
End Class
