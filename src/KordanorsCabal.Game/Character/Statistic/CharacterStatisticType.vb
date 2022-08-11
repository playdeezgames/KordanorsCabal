Imports System.Runtime.CompilerServices

Public Class CharacterStatisticType
    ReadOnly Property Id As Long
    Sub New(characterStatisticTypeId As Long)
        Id = characterStatisticTypeId
    End Sub
    Private Sub New(characterStatisticTypeName As String)
        Me.New(StaticWorldData.World.CharacterStatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromName(characterStatisticTypeName As String) As CharacterStatisticType
        Return New CharacterStatisticType(characterStatisticTypeName)
    End Function
    ReadOnly Property DefaultValue As Long?
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadDefaultValue(Id)
        End Get
    End Property

    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MinimumValue() As Long
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadMinimumValue(Id).Value
        End Get
    End Property

    ReadOnly Property MaximumValue As Long
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadMaximumValue(Id).Value
        End Get
    End Property

    ReadOnly Property Abbreviation As String
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadAbbreviation(Id)
        End Get
    End Property
End Class
Public Module CharacterStatisticTypeUtility
    <Extension>
    Public Function ToOld(characterStatisticType As CharacterStatisticType) As OldCharacterStatisticType
        Return CType(characterStatisticType.Id, OldCharacterStatisticType)
    End Function
    <Extension>
    Public Function ToNew(oldCharacterStatisticType As OldCharacterStatisticType) As CharacterStatisticType
        Return New CharacterStatisticType(oldCharacterStatisticType)
    End Function
End Module
