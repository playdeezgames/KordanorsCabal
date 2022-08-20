Public Class LocationType
    Inherits BaseThingie
    Sub New(worldData As WorldData, locationTypeId As Long)
        MyBase.New(worldData, locationTypeId)
    End Sub
    Private Sub New(worldData As WorldData, name As String)
        Me.New(worldData, worldData.LocationType.ReadForName(name).Value)
    End Sub
    ReadOnly Property Name As String
        Get
            Return WorldData.LocationType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsDungeon As Boolean
        Get
            Return WorldData.LocationType.ReadIsDungeon(Id)
        End Get
    End Property
    ReadOnly Property CanMap As Boolean
        Get
            Return WorldData.LocationType.ReadCanMap(Id)
        End Get
    End Property
    ReadOnly Property RequiresMP() As Boolean
        Get
            Return WorldData.LocationType.ReadRequiresMP(Id)
        End Get
    End Property
    Shared Function FromId(worldData As WorldData, locationTypeId As Long?) As LocationType
        Return If(locationTypeId.HasValue, New LocationType(worldData, locationTypeId.Value), Nothing)
    End Function
    Public Shared Operator =(first As LocationType, second As LocationType) As Boolean
        Return first.Id = second.Id
    End Operator
    Public Shared Operator <>(first As LocationType, second As LocationType) As Boolean
        Return first.Id <> second.Id
    End Operator
End Class
