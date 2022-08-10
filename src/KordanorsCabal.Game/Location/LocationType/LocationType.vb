Public MustInherit Class LocationType
    ReadOnly Property Id As Long
    Sub New(locationTypeId As Long)
        Id = locationTypeId
    End Sub
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.LocationType.ReadName(Id)
        End Get
    End Property
    MustOverride ReadOnly Property IsDungeon As Boolean
    MustOverride ReadOnly Property CanMap As Boolean
    MustOverride ReadOnly Property RequiresMP() As Boolean
End Class
Friend Module LocationDescriptorUtility
    Friend ReadOnly LocationDescriptors As IReadOnlyDictionary(Of OldLocationType, LocationType) =
        New Dictionary(Of OldLocationType, LocationType) From
        {
            {OldLocationType.Cellar, New CellarDescriptor},
            {OldLocationType.ChurchEntrance, New ChurchEntranceDescriptor},
            {OldLocationType.Dungeon, New DungeonLocationTypeDescriptor},
            {OldLocationType.DungeonBoss, New DungeonBossLocationTypeDescriptor},
            {OldLocationType.DungeonDeadEnd, New DungeonDeadEndLocationTypeDescriptor},
            {OldLocationType.Moon, New MoonLocationTypeDescriptor},
            {OldLocationType.Town, New TownDescriptor},
            {OldLocationType.TownSquare, New TownSquareDescriptor}
        }
End Module
