Public Interface ICharacterEncumbrance
    Inherits IBaseThingie
    ReadOnly Property IsEncumbered As Boolean
    ReadOnly Property CurrentEncumbrance As Long
    ReadOnly Property MaximumEncumbrance As Long
End Interface
