Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From
                {
                    LocationType.Dungeon
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Potion"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property PurchasePrice As Long?
        Get
            Return 25
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim healRoll = RNG.RollDice("2d4")
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        If character.Id = World.PlayerCharacter.Id Then
            PlayerCharacter.Messages.Enqueue(
                New Message(
                New List(Of String) From
                {
                    $"Potion heals up to {healRoll} HP!",
                    $"You now have {character.CurrentHP} HP!"
                }))
        End If
    End Sub

    Public Overrides Function RollSpawnCount(level As Long) As Long
        Return RNG.RollDice("3d6")
    End Function
End Class
