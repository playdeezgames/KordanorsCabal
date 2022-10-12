Public Class LocationFactions
    Inherits BaseThingie
    Implements ILocationFactions
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ILocationFactions
        Return If(id.HasValue, New LocationFactions(worldData, id.Value), Nothing)
    End Function
    Function EnemiesOf(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocationFactions.EnemiesOf
        Return Characters.Where(Function(x) x.IsEnemy(character))
    End Function
    Function FirstEnemy(character As ICharacter) As ICharacter Implements ILocationFactions.FirstEnemy
        Return EnemiesOf(character).FirstOrDefault
    End Function
    Function AlliesOf(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocationFactions.AlliesOf
        Return Characters.Where(Function(x) Not x.IsEnemy(character))
    End Function
    Private ReadOnly Property Characters As IEnumerable(Of ICharacter)
        Get
            Return WorldData.Character.ReadForLocation(Id).Select(Function(x) Character.FromId(WorldData, x))
        End Get
    End Property
End Class
