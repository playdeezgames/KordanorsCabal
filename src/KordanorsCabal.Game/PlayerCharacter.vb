Public Class PlayerCharacter
    Inherits Character

    Sub New()
        MyBase.New(PlayerData.Read())
    End Sub

    ReadOnly Property IsFullyBaked As Boolean
        Get
            Throw New NotImplementedException
        End Get
    End Property
End Class
