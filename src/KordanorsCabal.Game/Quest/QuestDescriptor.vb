Public MustInherit Class QuestDescriptor
    MustOverride ReadOnly Property Name As String
End Class
Friend Module QuestDescriptorUtility
    Friend ReadOnly QuestDescriptors As IReadOnlyDictionary(Of Quest, QuestDescriptor) =
        New Dictionary(Of Quest, QuestDescriptor) From
        {
            {Quest.CellarRats, New CellarRatsQuestDescriptor}
        }
End Module
