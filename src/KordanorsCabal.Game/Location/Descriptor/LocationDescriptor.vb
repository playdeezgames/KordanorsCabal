Public MustInherit Class LocationDescriptor
    MustOverride ReadOnly Property Name As String
    MustOverride ReadOnly Property IsDungeon As Boolean
    MustOverride ReadOnly Property CanMap As Boolean
    MustOverride ReadOnly Property RequiresMP() As Boolean
End Class
Friend Module LocationDescriptorUtility
    Friend ReadOnly LocationDescriptors As IReadOnlyDictionary(Of LocationType, LocationDescriptor) =
        New Dictionary(Of LocationType, LocationDescriptor) From
        {
            {LocationType.Cellar, New CellarDescriptor},
            {LocationType.ChurchEntrance, New ChurchEntranceDescriptor},
            {LocationType.Dungeon, New DungeonLocationTypeDescriptor},
            {LocationType.DungeonBoss, New DungeonBossLocationTypeDescriptor},
            {LocationType.DungeonDeadEnd, New DungeonDeadEndLocationTypeDescriptor},
            {LocationType.Town, New TownDescriptor},
            {LocationType.TownSquare, New TownSquareDescriptor}
        }
End Module
