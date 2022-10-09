Public Class QuestType
    Inherits BaseThingie
    Implements IQuestType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IQuestType
        Return If(id.HasValue, New QuestType(worldData, id.Value), Nothing)
    End Function

    Overridable ReadOnly Property Name As String
    Overridable Function CanAccept(character As ICharacter) As Boolean
        Throw New NotImplementedException
    End Function
    Overridable Function CanComplete(character As ICharacter) As Boolean
        Throw New NotImplementedException
    End Function
    Overridable Sub Complete(worldData As IWorldData, character As ICharacter)
        Throw New NotImplementedException
    End Sub
    Overridable Sub Accept(worldData As IWorldData, character As ICharacter)
        Throw New NotImplementedException
    End Sub
End Class
Friend Module QuestDescriptorUtility
    Public ReadOnly Property QuestDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of OldQuestType, QuestType)
        Get
            Return New Dictionary(Of OldQuestType, QuestType) From
            {
                {OldQuestType.CellarRats, New CellarRatsQuestDescriptor(worldData)}
            }
        End Get
    End Property
End Module
