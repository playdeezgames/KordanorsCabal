Friend Class MalcontentDescriptor
    Inherits CharacterType
    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 2},
                    {1, 6},
                    {2, 4},
                    {6, 2},
                    {23, 0},
                    {3, 1},
                    {7, 1},
                    {13, 0},
                    {10, 3},
                    {4, 2},
                    {12, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property


    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id <> 1 OrElse location.LocationType = LocationType.FromName(DungeonDeadEnd)
    End Function

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
End Class
