Public Class CharacterStatisticType
    Inherits BaseThingie
    Implements ICharacterStatisticType
    Sub New(worldData As IWorldData, characterStatisticTypeId As Long)
        MyBase.New(worldData, characterStatisticTypeId)
    End Sub
    Private Sub New(worldData As IWorldData, characterStatisticTypeName As String)
        Me.New(worldData, worldData.CharacterStatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, statisticTypeId As Long?) As ICharacterStatisticType
        Return If(statisticTypeId.HasValue, New CharacterStatisticType(worldData, statisticTypeId.Value), Nothing)
    End Function
    ReadOnly Property DefaultValue As Long? Implements ICharacterStatisticType.DefaultValue
        Get
            Return WorldData.CharacterStatisticType.ReadDefaultValue(Id)
        End Get
    End Property

    ReadOnly Property Name As String Implements ICharacterStatisticType.Name
        Get
            Return WorldData.CharacterStatisticType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MinimumValue() As Long Implements ICharacterStatisticType.MinimumValue
        Get
            Return WorldData.CharacterStatisticType.ReadMinimumValue(Id).Value
        End Get
    End Property

    ReadOnly Property MaximumValue As Long Implements ICharacterStatisticType.MaximumValue
        Get
            Return WorldData.CharacterStatisticType.ReadMaximumValue(Id).Value
        End Get
    End Property

    ReadOnly Property Abbreviation As String Implements ICharacterStatisticType.Abbreviation
        Get
            Return WorldData.CharacterStatisticType.ReadAbbreviation(Id)
        End Get
    End Property
End Class
