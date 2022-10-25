Public Class StatisticType
    Inherits BaseThingie
    Implements IStatisticType
    Sub New(worldData As IWorldData, characterStatisticTypeId As Long)
        MyBase.New(worldData, characterStatisticTypeId)
    End Sub
    Private Sub New(worldData As IWorldData, characterStatisticTypeName As String)
        Me.New(worldData, worldData.StatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, statisticTypeId As Long?) As IStatisticType
        Return If(statisticTypeId.HasValue, New StatisticType(worldData, statisticTypeId.Value), Nothing)
    End Function
    ReadOnly Property DefaultValue As Long? Implements IStatisticType.DefaultValue
        Get
            Return WorldData.StatisticType.ReadDefaultValue(Id)
        End Get
    End Property

    ReadOnly Property Name As String Implements IStatisticType.Name
        Get
            Return WorldData.StatisticType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MinimumValue() As Long Implements IStatisticType.MinimumValue
        Get
            Return WorldData.StatisticType.ReadMinimumValue(Id).Value
        End Get
    End Property

    ReadOnly Property MaximumValue As Long Implements IStatisticType.MaximumValue
        Get
            Return WorldData.StatisticType.ReadMaximumValue(Id).Value
        End Get
    End Property

    ReadOnly Property Abbreviation As String Implements IStatisticType.Abbreviation
        Get
            Return WorldData.StatisticType.ReadAbbreviation(Id)
        End Get
    End Property
End Class
