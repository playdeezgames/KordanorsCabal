Imports System.Runtime.CompilerServices

Public Enum OldLocationType
    None
    TownSquare
    Town
    ChurchEntrance
    Dungeon
    DungeonDeadEnd
    DungeonBoss
    Cellar
    Moon
End Enum
Public Module LocationTypeExtensions
    <Extension>
    Function Name(locationType As OldLocationType) As String
        Return LocationDescriptors(locationType).Name
    End Function
    <Extension>
    Public Function IsDungeon(locationType As OldLocationType) As Boolean
        Return LocationDescriptors(locationType).IsDungeon
    End Function
    <Extension>
    Public Function CanMap(locationType As OldLocationType) As Boolean
        Return LocationDescriptors(locationType).CanMap
    End Function
    <Extension>
    Public Function RequiresMP(locationType As OldLocationType) As Boolean
        Return LocationDescriptors(locationType).RequiresMP
    End Function
End Module
