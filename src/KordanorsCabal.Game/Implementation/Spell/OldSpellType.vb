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
    Function MaximumLevel(spellType As OldSpellType, worldData As IWorldData) As Long
        Return SpellDescriptors(spellType)(worldData).MaximumLevel
    End Function
    <Extension>
    Function RequiredPower(spellType As OldSpellType, worldData As IWorldData, level As Long) As Long
        Return SpellDescriptors(spellType)(worldData).RequiredPower(level)
    End Function
    <Extension>
    Function CanCast(spellType As OldSpellType, worldData As IWorldData, character As ICharacter) As Boolean
        Return SpellDescriptors(spellType)(worldData).CanCast(character)
    End Function
    <Extension>
    Sub Cast(spellType As OldSpellType, worldData As IWorldData, character As ICharacter)
        SpellDescriptors(spellType)(worldData).Cast(character)
    End Sub
End Module