﻿Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.SpaceSord,
            5,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            10,
            5,,
            100)
    End Sub
End Class
