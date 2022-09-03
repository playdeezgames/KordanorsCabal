Public Class Direction
    Inherits BaseThingie
    Sub New(worldData As IWorldData, directionId As Long)
        MyBase.New(worldData, directionId)
    End Sub
    Private Sub New(worldData As IWorldData, directionName As String)
        Me.New(worldData, worldData.Direction.ReadForName(directionName).Value)
    End Sub
    Shared Function FromId(worldData As IWorldData, directionId As Long?) As Direction
        Return If(directionId.HasValue, New Direction(worldData, directionId.Value), Nothing)
    End Function
    ReadOnly Property Name As String
        Get
            Return WorldData.Direction.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Abbreviation As String
        Get
            Return WorldData.Direction.ReadAbbreviation(Id)
        End Get
    End Property
    ReadOnly Property Opposite As Direction
        Get
            Return New Direction(WorldData, WorldData.Direction.ReadOpposite(Id).Value)
        End Get
    End Property
    ReadOnly Property IsCardinal As Boolean
        Get
            Return WorldData.Direction.ReadIsCardinal(Id)
        End Get
    End Property
    ReadOnly Property NextDirection As Direction
        Get
            Dim result = WorldData.Direction.ReadNext(Id)
            Return If(result.HasValue, New Direction(WorldData, result.Value), Nothing)
        End Get
    End Property
    ReadOnly Property PreviousDirection As Direction
        Get
            Dim result = WorldData.Direction.ReadPrevious(Id)
            Return If(result.HasValue, New Direction(WorldData, result.Value), Nothing)
        End Get
    End Property
End Class
