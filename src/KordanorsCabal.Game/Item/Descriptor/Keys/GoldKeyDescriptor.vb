﻿Friend Class GoldKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.GoldKey,
            "AU Key",,
            MakeDictionary(
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L)))))
    End Sub
End Class
