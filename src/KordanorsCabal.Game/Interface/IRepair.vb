Public Interface IRepair
    Inherits IBaseThingie
    ReadOnly Property NeedsRepair As Boolean
    Sub DoRepair()
End Interface
