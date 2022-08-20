Public Class CharacterStatisticType
    Inherits BaseThingie
    Sub New(worldData As WorldData, characterStatisticTypeId As Long)
        MyBase.New(worldData, characterStatisticTypeId)
    End Sub
    Private Sub New(worldData As WorldData, characterStatisticTypeName As String)
        Me.New(worldData, worldData.CharacterStatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromId(worldData As WorldData, statisticTypeId As Long) As CharacterStatisticType
        Return New CharacterStatisticType(worldData, statisticTypeId)
    End Function
    ReadOnly Property DefaultValue As Long?
        Get
            Return WorldData.CharacterStatisticType.ReadDefaultValue(Id)
        End Get
    End Property

    ReadOnly Property Name As String
        Get
            Return WorldData.CharacterStatisticType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MinimumValue() As Long
        Get
            Return WorldData.CharacterStatisticType.ReadMinimumValue(Id).Value
        End Get
    End Property

    ReadOnly Property MaximumValue As Long
        Get
            Return WorldData.CharacterStatisticType.ReadMaximumValue(Id).Value
        End Get
    End Property

    ReadOnly Property Abbreviation As String
        Get
            Return WorldData.CharacterStatisticType.ReadAbbreviation(Id)
        End Get
    End Property
End Class
