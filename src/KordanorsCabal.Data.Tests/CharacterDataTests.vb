Public Class CharacterDataTests
    <Fact>
    Sub ShouldClearOutDataWhenClearIsCalled()
        Dim store As New Mock(Of IStore)
        Dim worldData As New WorldData(store.Object)
        Dim subject = worldData.Character
        Const characterId = 1L
        subject.Clear(characterId)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterQuests", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterQuestCompletions", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterEquipSlots", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "Inventories", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterLocations", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterStatistics", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "Players", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "CharacterSpells", ("CharacterId", characterId)), Times.Once)
        store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), "Characters", ("CharacterId", characterId)), Times.Once)
        store.VerifyNoOtherCalls()
    End Sub
End Class
