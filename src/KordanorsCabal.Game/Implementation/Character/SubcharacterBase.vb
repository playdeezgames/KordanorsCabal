Public MustInherit Class SubcharacterBase
    Inherits BaseThingie
    Protected ReadOnly Property Character As ICharacter

    Protected Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.Character = character
    End Sub
    Protected ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.Character.EquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    Protected Function RollDice(dice As Long) As Long
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function
    Protected Function NegativeInfluence() As Long
        Return If(Character.Statuses.Drunkenness > 0 OrElse Character.Statuses.Highness > 0 OrElse Character.Statuses.Chafing > 0, -1, 0)
    End Function
End Class
