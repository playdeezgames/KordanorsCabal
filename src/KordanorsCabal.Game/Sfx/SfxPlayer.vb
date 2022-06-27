Public Module SfxPlayer
    Public Event PlaySfx As Action(Of Sfx)
    Sub Play(sfx As Sfx)
        RaiseEvent PlaySfx(sfx)
    End Sub
End Module