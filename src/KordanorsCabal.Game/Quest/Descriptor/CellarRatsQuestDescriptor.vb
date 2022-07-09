Friend Class CellarRatsQuestDescriptor
    Inherits QuestDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Cellar Rats"
        End Get
    End Property

    Public Overrides Sub Complete(character As Character)
        If CanComplete(character) Then
            Return
        End If
    End Sub

    Public Overrides Function CanAccept(character As Character) As Boolean
        Return True
    End Function

    Public Overrides Function CanComplete(character As Character) As Boolean
        Return character.Inventory.ItemsOfType(ItemType.RatTail).Count >= 10
    End Function
End Class
