Public Class WorldData
    Inherits BaseData
    Public ReadOnly Character As CharacterData

    Public Sub New(store As Store)
        MyBase.New(store)
        Character = New CharacterData(store)
    End Sub
End Class
