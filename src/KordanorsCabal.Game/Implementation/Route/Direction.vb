Public Class Direction
    Inherits BaseThingie
    Implements IDirection
    Sub New(worldData As IWorldData, directionId As Long)
        MyBase.New(worldData, directionId)
    End Sub
    Shared Function FromId(worldData As IWorldData, directionId As Long?) As IDirection
        Return If(directionId.HasValue, New Direction(worldData, directionId.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements IDirection.Name
        Get
            Return WorldData.Direction.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Abbreviation As String Implements IDirection.Abbreviation
        Get
            Return WorldData.Direction.ReadAbbreviation(Id)
        End Get
    End Property
    ReadOnly Property Opposite As IDirection Implements IDirection.Opposite
        Get
            Return FromId(WorldData, WorldData.Direction.ReadOpposite(Id))
        End Get
    End Property
    ReadOnly Property IsCardinal As Boolean
        Get
            Return WorldData.Direction.ReadIsCardinal(Id)
        End Get
    End Property
    ReadOnly Property NextDirection As IDirection Implements IDirection.NextDirection
        Get
            Dim result = WorldData.Direction.ReadNext(Id)
            Return If(result.HasValue, New Direction(WorldData, result.Value), Nothing)
        End Get
    End Property
    ReadOnly Property PreviousDirection As IDirection Implements IDirection.PreviousDirection
        Get
            Dim result = WorldData.Direction.ReadPrevious(Id)
            Return If(result.HasValue, New Direction(WorldData, result.Value), Nothing)
        End Get
    End Property
End Class
