Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)), ,
            2,
            1,,
            3,,,,,,,,
            "CanUseBottle",
            "UseBotttle")
    End Sub
End Class
