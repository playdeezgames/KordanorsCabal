Imports System.Runtime.CompilerServices

Public Enum OldFeatureType
    None
    Elder
    InnKeeper
    TownDrunk
    Chicken
    BlackMarketeer
    BlackMage
    Blacksmith
    Healer
    Constable
End Enum
Public Module FeatureTypeUtility
    Friend FeatureTypeDescriptors As IReadOnlyDictionary(Of OldFeatureType, FeatureType) =
        New Dictionary(Of OldFeatureType, FeatureType) From
        {
            {OldFeatureType.BlackMage, New FeatureType(OldFeatureType.BlackMage)},
            {OldFeatureType.BlackMarketeer, New FeatureType(OldFeatureType.BlackMarketeer)},
            {OldFeatureType.Blacksmith, New FeatureType(OldFeatureType.Blacksmith)},
            {OldFeatureType.Chicken, New FeatureType(OldFeatureType.Chicken)},
            {OldFeatureType.Constable, New FeatureType(OldFeatureType.Constable)},
            {OldFeatureType.Elder, New FeatureType(OldFeatureType.Elder)},
            {OldFeatureType.Healer, New FeatureType(OldFeatureType.Healer)},
            {OldFeatureType.InnKeeper, New FeatureType(OldFeatureType.InnKeeper)},
            {OldFeatureType.TownDrunk, New FeatureType(OldFeatureType.TownDrunk)}
        }
    Public ReadOnly Property AllFeatureTypes As IEnumerable(Of OldFeatureType)
        Get
            Return FeatureTypeDescriptors.Keys
        End Get
    End Property
    <Extension>
    Public Function ToNew(oldFeatureType As OldFeatureType) As FeatureType
        Return New FeatureType(oldFeatureType)
    End Function
    Public Const Elder = 1L
    Public Const InnKeeper = 2L
    Public Const TownDrunk = 3L
    Public Const Chicken = 4L
    Public Const BlackMarketeer = 5L
    Public Const BlackMage = 6L
    Public Const Blacksmith = 7L
    Public Const Healer = 8L
    Public Const Constable = 9L
End Module
