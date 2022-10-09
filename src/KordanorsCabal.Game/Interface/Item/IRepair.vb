Public Interface IRepair
    Inherits IBaseThingie
    ReadOnly Property IsNeeded As Boolean
    Sub Perform()
    Function RepairCost(shoppeType As IShoppeType) As Long
End Interface
