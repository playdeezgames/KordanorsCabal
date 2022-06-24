Imports System.Runtime.CompilerServices

Public Enum LocationType
    None
    TownSquare
    Town
    ChurchEntrance
    Dungeon
End Enum
Module LocationTypeExtensions
    <Extension>
    Function Name(locationType As LocationType) As String
        Return LocationDescriptors(locationType).Name
    End Function
End Module
