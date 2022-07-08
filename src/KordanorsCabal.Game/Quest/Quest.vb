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
End Module
