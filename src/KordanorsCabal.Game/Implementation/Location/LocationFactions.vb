Public Class LocationFactions
    Inherits BaseThingie
    Implements ILocationFactions
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ILocationFactions
        Return If(id.HasValue, New LocationFactions(worldData, id.Value), Nothing)
    End Function
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocationFactions.Enemies
        Return Characters.Where(Function(x) x.IsEnemy(character))
    End Function
    Function Enemy(character As ICharacter) As ICharacter Implements ILocationFactions.Enemy
        Return Enemies(character).FirstOrDefault
    End Function
    Function Allies(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocationFactions.Allies
        Return Characters.Where(Function(x) Not x.IsEnemy(character))
    End Function
    Private ReadOnly Property Characters As IEnumerable(Of ICharacter)
        Get
            Return WorldData.Character.ReadForLocation(Id).Select(Function(x) Character.FromId(WorldData, x))
        End Get
    End Property
End Class
