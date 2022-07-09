Imports System.Runtime.CompilerServices

Public Enum Quest
    None
    CellarRats
End Enum
Public Module QuestExtensions
    <Extension>
    Function Name(quest As Quest) As String
        Return QuestDescriptors(quest).Name
    End Function
    <Extension>
    Function CanAccept(quest As Quest, character As Character) As Boolean
        Return QuestDescriptors(quest).CanAccept(character)
    End Function
    <Extension>
    Function CanComplete(quest As Quest, character As Character) As Boolean
        Return QuestDescriptors(quest).CanComplete(character)
    End Function
    <Extension>
    Sub Complete(quest As Quest, character As Character)
        QuestDescriptors(quest).Complete(character)
    End Sub
End Module
