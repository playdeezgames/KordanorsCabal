﻿Public MustInherit Class LocationDescriptor
    MustOverride ReadOnly Property Name As String
End Class
Friend Module LocationDescriptorUtility
    Friend ReadOnly LocationDescriptors As IReadOnlyDictionary(Of LocationType, LocationDescriptor) =
        New Dictionary(Of LocationType, LocationDescriptor) From
        {
            {LocationType.Town, New TownDescriptor},
            {LocationType.TownSquare, New TownSquareDescriptor}
        }
End Module