Friend Interface ISpellType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property MaximumLevel As Long
    ReadOnly Property RequiredPower(level As Long) As Long
    ReadOnly Property CanCast(character As ICharacter) As Boolean
    Sub Cast(character As ICharacter)
End Interface
