Public Class LocationType
    Inherits BaseThingie
    Implements ILocationType
    Sub New(worldData As IWorldData, locationTypeId As Long)
        MyBase.New(worldData, locationTypeId)
    End Sub
    ReadOnly Property Name As String Implements ILocationType.Name
        Get
            Return WorldData.LocationType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsDungeon As Boolean Implements ILocationType.IsDungeon
        Get
            Return WorldData.LocationType.ReadIsDungeon(Id)
        End Get
    End Property
    ReadOnly Property CanMap As Boolean Implements ILocationType.CanMap
        Get
            Return WorldData.LocationType.ReadCanMap(Id)
        End Get
    End Property
    ReadOnly Property RequiresMP() As Boolean Implements ILocationType.RequiresMP
        Get
            Return WorldData.LocationType.ReadRequiresMP(Id)
        End Get
    End Property
    Shared Function FromId(worldData As IWorldData, locationTypeId As Long?) As ILocationType
        Return If(locationTypeId.HasValue, New LocationType(worldData, locationTypeId.Value), Nothing)
    End Function
End Class
