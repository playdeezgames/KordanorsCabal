Imports System.Runtime.CompilerServices

Public Enum OldSpellType
    None
    HolyBolt
    Purify
End Enum
Public Module SpellTypeExtensions
    <Extension>
    Function Name(spellType As OldSpellType, worldData As IWorldData) As String
        Return SpellDescriptors(spellType)(worldData).Name
    End Function
    <Extension>
    Sub Cast(spellType As OldSpellType, worldData As IWorldData, character As ICharacter)
        SpellDescriptors(spellType)(worldData).Cast(character)
    End Sub
End Module