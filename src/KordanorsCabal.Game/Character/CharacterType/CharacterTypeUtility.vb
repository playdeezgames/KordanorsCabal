Public Module CharacterTypeUtility
    Function AllCharacterTypes(worldData As WorldData) As IEnumerable(Of CharacterType)
        Return StaticWorldData.World.CharacterType.ReadAll().Select(Function(x) CharacterType.FromId(worldData, x))
    End Function
End Module