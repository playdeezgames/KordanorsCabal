Imports System.Runtime.CompilerServices

Public Class LocationType
    ReadOnly Property Id As Long
    Sub New(locationTypeId As Long)
        Id = locationTypeId
    End Sub
    Private Sub New(name As String)
        Me.New(StaticWorldData.World.LocationType.ReadForName(name).Value)
    End Sub
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.LocationType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsDungeon As Boolean
        Get
            Return StaticWorldData.World.LocationType.ReadIsDungeon(Id)
        End Get
    End Property
    ReadOnly Property CanMap As Boolean
        Get
            Return StaticWorldData.World.LocationType.ReadCanMap(Id)
        End Get
    End Property
    ReadOnly Property RequiresMP() As Boolean
        Get
            Return StaticWorldData.World.LocationType.ReadRequiresMP(Id)
        End Get
    End Property
    Shared Function FromId(locationTypeId As Long) As LocationType
        Return New LocationType(locationTypeId)
    End Function
    Public Shared Operator =(first As LocationType, second As LocationType) As Boolean
        Return first.Id = second.Id
    End Operator
    Public Shared Operator <>(first As LocationType, second As LocationType) As Boolean
        Return first.Id <> second.Id
    End Operator
End Class
