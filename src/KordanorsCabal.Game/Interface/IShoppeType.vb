Public Interface IShoppeType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property Offers As IReadOnlyDictionary(Of IItemType, Long)
    ReadOnly Property Prices As IReadOnlyDictionary(Of IItemType, Long)
    ReadOnly Property Repairs As IReadOnlyDictionary(Of IItemType, Long)
End Interface
