Public Class DungeonLevelShould
    Inherits ThingieShould(Of IDungeonLevel)

    Public Sub New()
        MyBase.New(AddressOf DungeonLevel.FromId)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.DungeonLevel.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.DungeonLevel.ReadName(id))
            End Sub)
    End Sub
End Class
