Imports System.Runtime.CompilerServices

Public Enum OldQuestType
    None
    CellarRats
End Enum
Public Module QuestExtensions
    <Extension>
    Function CanAccept(quest As OldQuestType, worldData As IWorldData, character As ICharacter) As Boolean
        Return QuestDescriptors(worldData)(quest).CanAccept(character)
    End Function
    <Extension>
    Sub Accept(quest As OldQuestType, worldData As IWorldData, character As ICharacter)
        QuestDescriptors(worldData)(quest).Accept(worldData, character)
    End Sub
    <Extension>
    Function CanComplete(quest As OldQuestType, worldData As IWorldData, character As ICharacter) As Boolean
        Return QuestDescriptors(worldData)(quest).CanComplete(character)
    End Function
    <Extension>
    Sub Complete(quest As OldQuestType, worldData As IWorldData, character As ICharacter)
        QuestDescriptors(worldData)(quest).Complete(worldData, character)
    End Sub
End Module
