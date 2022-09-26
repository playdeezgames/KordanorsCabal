Friend Class RingOfHPDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RingOfHP,
            New Dictionary(Of Long, Long) From {{6, 1}})
    End Sub
End Class
