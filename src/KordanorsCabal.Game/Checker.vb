Friend Class Checker
    Implements IChecker
    Private ReadOnly checkerTable As IReadOnlyDictionary(Of String, Func(Of IWorldData, Long, Boolean)) =
        New Dictionary(Of String, Func(Of IWorldData, Long, Boolean)) From
        {
            {"CharacterCanCastHolyBolt",
                Function(worldData, characterId)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy Is Nothing OrElse Not enemy.IsUndead Then
                        Return False
                    End If
                    Return character.CurrentMana > 0
                End Function},
            {"CharacterCanCastPurify",
                Function(worldData, characterId)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return character.CurrentMana > 0
                End Function}
        }

    Public Function Check(worldData As IWorldData, checkType As String, id As Long) As Boolean Implements IChecker.Check
        Return checkerTable(checkType)(worldData, id)
    End Function
End Class
