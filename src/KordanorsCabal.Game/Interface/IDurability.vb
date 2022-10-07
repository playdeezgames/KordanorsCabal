Public Interface IDurability
    Inherits IBaseThingie
    ReadOnly Property Current As Long?
    ReadOnly Property Maximum As Long?
    Sub Reduce(amount As Long)
    ReadOnly Property IsBroken As Boolean
End Interface
