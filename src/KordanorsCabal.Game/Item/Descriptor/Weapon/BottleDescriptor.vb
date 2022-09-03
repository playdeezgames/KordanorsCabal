Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Bottle,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)), ,
            2,
            1,,
            3,,,,,,,,
            "CanUseBottle",
            "UseBotttle")
    End Sub
End Class
