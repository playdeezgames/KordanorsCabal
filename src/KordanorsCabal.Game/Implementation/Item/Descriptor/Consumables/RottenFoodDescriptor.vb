﻿Friend Class RottenFoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenFood)
    End Sub
End Class
