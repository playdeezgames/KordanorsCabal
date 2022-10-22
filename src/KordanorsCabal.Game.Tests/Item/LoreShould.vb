Public Class LoreShould
    Inherits ThingieShould(Of ILore)
    Public Sub New()
        MyBase.New(Function(w, i) Lore.FromId(w, i))
    End Sub
    <Fact>
    Public Sub have_text()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Lore.ReadText(It.IsAny(Of Long)))
                subject.Text.ShouldBeNull
                worldData.Verify(Function(x) x.Lore.ReadText(id))
            End Sub)
    End Sub
    <Fact>
    Public Sub have_item_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Lore.ReadItemName(It.IsAny(Of Long)))
                subject.ItemName.ShouldBeNull
                worldData.Verify(Function(x) x.Lore.ReadItemName(id))
            End Sub)
    End Sub
End Class
