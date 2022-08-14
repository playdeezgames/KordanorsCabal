Public Class CharacterStatisticType
    ReadOnly Property Id As Long
    Sub New(characterStatisticTypeId As Long)
        Id = characterStatisticTypeId
    End Sub
    Private Sub New(characterStatisticTypeName As String)
        Me.New(StaticWorldData.World.CharacterStatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromId(statisticTypeId As Long) As CharacterStatisticType
        Return New CharacterStatisticType(statisticTypeId)
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
