Public Interface IRepair
    Inherits IBaseThingie
    ReadOnly Property IsNeeded As Boolean
    Sub Perform()
End Interface
