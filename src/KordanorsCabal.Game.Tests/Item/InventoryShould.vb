Public Class InventoryShould
    Inherits ThingieShould(Of IInventory)

    Public Sub New()
        MyBase.New(Function(w, i) Inventory.FromId(w, i))
    End Sub
    <Fact>
    Sub have_location()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadLocation(It.IsAny(Of Long)))
                subject.Location.ShouldBeNull
                worldData.Verify(Function(x) x.Inventory.ReadLocation(id))
            End Sub)
    End Sub
End Class
