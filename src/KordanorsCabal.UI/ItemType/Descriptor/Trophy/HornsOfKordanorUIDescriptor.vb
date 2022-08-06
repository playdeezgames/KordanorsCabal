Friend Class HornsOfKordanorUIDescriptor
    Inherits ItemTypeUIDescriptor
    Public Sub New()
        MyBase.New(Pattern.LessThan, (10, 17), Hue.Red)
    End Sub
End Class
