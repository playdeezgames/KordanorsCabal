Imports System.Runtime.CompilerServices

Public Enum SpellType
    None
    HolyBolt
End Enum
Public Module SpellTypeExtensions
    <Extension>
    Function Name(spellType As SpellType) As String
        Return SpellDescriptors(spellType).Name
    End Function
End Module