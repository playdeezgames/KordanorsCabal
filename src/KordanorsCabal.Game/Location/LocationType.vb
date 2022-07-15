Imports System.Runtime.CompilerServices

Public Enum LocationType
    None
    TownSquare
    Town
    ChurchEntrance
    Dungeon
    DungeonDeadEnd
    DungeonBoss
    Cellar
End Enum
Public Module LocationTypeExtensions
    <Extension>
    Function Name(locationType As LocationType) As String
        Return LocationDescriptors(locationType).Name
    End Function
    <Extension>
    Public Function IsDungeon(locationType As LocationType) As Boolean
        Return LocationDescriptors(locationType).IsDungeon
    End Function
    <Extension>
    Public Function CanMap(locationType As LocationType) As Boolean
        Return LocationDescriptors(locationType).CanMap
    End Function
    <Extension>
    Public Function RequiresMP(locationType As LocationType) As Boolean
        Return LocationDescriptors(locationType).RequiresMP
    End Function
End Module
