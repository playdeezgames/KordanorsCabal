﻿Friend Class FoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Food,,
            "AlwaysTrue")
    End Sub
End Class
