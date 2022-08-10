Imports System.Runtime.CompilerServices

Public Class LocationType
    ReadOnly Property Id As Long
    Sub New(locationTypeId As Long)
        Id = locationTypeId
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
End Class
Public Module LocationTypeUtility
    <Extension>
    Public Function ToOld(locationType As LocationType) As OldLocationType
        Return CType(locationType.Id, OldLocationType)
    End Function
    <Extension>
    Public Function ToNew(oldLocationType As OldLocationType) As LocationType
        Return New LocationType(oldLocationType)
    End Function
    Public Const TownSquare = "Town Square"
    Public Const Town = "Town"
    Public Const ChurchEntrance = "Church Entrance"
    Public Const Dungeon = "Dungeon"
    Public Const DungeonDeadEnd = "Dungeon Dead End"
    Public Const DungeonBoss = "Dungeon Boss"
    Public Const Cellar = "Cellar"
    Public Const Moon = "Moon"
End Module
