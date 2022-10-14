Public Interface ICharacterQuest
    Inherits IBaseThingie
    Sub AcceptQuest(quest As IQuestType)
    Function CanAcceptQuest(quest As IQuestType) As Boolean
    Sub CompleteQuest(quest As IQuestType)
    Function HasQuest(quest As IQuestType) As Boolean
End Interface
