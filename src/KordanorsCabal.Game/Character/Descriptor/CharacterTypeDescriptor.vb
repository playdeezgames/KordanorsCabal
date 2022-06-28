﻿Friend MustInherit Class CharacterTypeDescriptor
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
    MustOverride ReadOnly Property MaximumEncumbrance(character As Character) As Single
    MustOverride Function IsEnemy(character As Character) As Boolean
    MustOverride ReadOnly Property SpawnCount(level As Long) As Long
    Overridable Function CanSpawn(location As Location) As Boolean
        Return False
    End Function
End Class
Friend Module CharacterTypeDescriptorUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {CharacterType.N00b, New N00bDescriptor},
            {CharacterType.Skeleton, New SkeletonDescriptor}
        }
    ReadOnly Property AllCharacterTypes As IEnumerable(Of CharacterType)
        Get
            Return CharacterTypeDescriptors.Keys
        End Get
    End Property
End Module
