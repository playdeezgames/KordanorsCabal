Public Interface IDurability
    Inherits IBaseThingie
    ReadOnly Property CurrentDurability As Long?
    ReadOnly Property MaximumDurability As Long?
    Sub ReduceDurability(amount As Long)
End Interface
