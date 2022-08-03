Public Interface ICharacter(Of TLocationType, TLocation As ILocation(Of TLocationType), TCharacterType)
    ReadOnly Property Id As Long
    ReadOnly Property CharacterType As TCharacterType
    ReadOnly Property Name As String
    Property Location As TLocation
End Interface
