﻿Imports System.Runtime.CompilerServices

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
    Public ReadOnly Property AllFeatureTypes(worldData As WorldData) As IEnumerable(Of FeatureType)
        Get
            Return worldData.FeatureType.ReadAll().Select(Function(x) New FeatureType(x))
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
