Imports System.Runtime.CompilerServices

Public Enum OldSpellType
    None
    HolyBolt
    Purify
End Enum
Public Module SpellTypeExtensions
    <Extension>
    Function Name(spellType As OldSpellType) As String
        Return SpellDescriptors(spellType).Name
    End Function
    <Extension>
    Function MaximumLevel(spellType As OldSpellType) As Long
        Return SpellDescriptors(spellType).MaximumLevel
    End Function
    <Extension>
    Function RequiredPower(spellType As OldSpellType, level As Long) As Long
        Return SpellDescriptors(spellType).RequiredPower(level)
    End Function
    <Extension>
    Function CanCast(spellType As OldSpellType, character As ICharacter) As Boolean
        Return SpellDescriptors(spellType).CanCast(character)
    End Function
    <Extension>
    Sub Cast(spellType As OldSpellType, character As ICharacter)
        SpellDescriptors(spellType).Cast(character)
    End Sub
End Module