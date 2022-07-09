Public MustInherit Class QuestDescriptor
    MustOverride ReadOnly Property Name As String
    MustOverride Function CanAccept(character As Character) As Boolean
    MustOverride Function CanComplete(character As Character) As Boolean
    MustOverride Sub Complete(character As Character)
End Class
Friend Module QuestDescriptorUtility
    Friend ReadOnly QuestDescriptors As IReadOnlyDictionary(Of Quest, QuestDescriptor) =
        New Dictionary(Of Quest, QuestDescriptor) From
        {
            {Quest.CellarRats, New CellarRatsQuestDescriptor}
        }
End Module
