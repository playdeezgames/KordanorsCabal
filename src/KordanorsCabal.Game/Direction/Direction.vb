﻿Public Class Direction
    Sub New(directionId As Long)
        Id = directionId
    End Sub
    Private Sub New(directionName As String)
        Me.New(StaticWorldData.World.Direction.ReadForName(directionName).Value)
    End Sub
    Shared Function FromName(directionName As String) As Direction
        Return New Direction(directionName)
    End Function
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.Direction.ReadName(Id)
        End Get
    End Property
    ReadOnly Property Abbreviation As String
        Get
            Return StaticWorldData.World.Direction.ReadAbbreviation(Id)
        End Get
    End Property
    ReadOnly Property Opposite As Direction
        Get
            Return New Direction(StaticWorldData.World.Direction.ReadOpposite(Id).Value)
        End Get
    End Property
    ReadOnly Property IsCardinal As Boolean
        Get
            Return StaticWorldData.World.Direction.ReadIsCardinal(Id)
        End Get
    End Property
    ReadOnly Property NextDirection As Direction
        Get
            Dim result = StaticWorldData.World.Direction.ReadNext(Id)
            Return If(result.HasValue, New Direction(result.Value), Nothing)
        End Get
    End Property
    ReadOnly Property PreviousDirection As Direction
        Get
            Dim result = StaticWorldData.World.Direction.ReadPrevious(Id)
            Return If(result.HasValue, New Direction(result.Value), Nothing)
        End Get
    End Property
End Class
Public Module DirectionUtility
    Friend ReadOnly Property AllDirections As IEnumerable(Of Direction)
        Get
            Return StaticWorldData.World.Direction.ReadAll.Select(Function(x) New Direction(x))
        End Get
    End Property
    Friend ReadOnly Property CardinalDirections As IEnumerable(Of Direction)
        Get
            Return AllDirections.Where(Function(x) x.IsCardinal)
        End Get
    End Property
    Public Const North = "North"
    Public Const East = "East"
    Public Const South = "South"
    Public Const West = "West"
    Public Const Up = "Up"
    Public Const Down = "Down"
    Public Const Inward = "In"
    Public Const Outward = "Out"
End Module
