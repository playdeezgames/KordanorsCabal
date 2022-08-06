Imports System.Runtime.CompilerServices

Public Enum SpellType
    None
    HolyBolt
    Purify
End Enum
Public Module SpellTypeExtensions
    <Extension>
    Function Name(spellType As SpellType) As String
        Return SpellDescriptors(spellType).Name
    End Function
    <Extension>
    Function MaximumLevel(spellType As SpellType) As Long
        Return SpellDescriptors(spellType).MaximumLevel
    End Function
    <Extension>
    Function RequiredPower(spellType As SpellType, level As Long) As Long
        Return SpellDescriptors(spellType).RequiredPower(level)
    End Function
    <Extension>
    Function CanCast(spellType As SpellType, character As Character) As Boolean
        Return SpellDescriptors(spellType).CanCast(character)
    End Function
    <Extension>
    Sub Cast(spellType As SpellType, character As Character)
        SpellDescriptors(spellType).Cast(character)
    End Sub
End Module