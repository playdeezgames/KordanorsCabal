Public Class StoreMeta
    Inherits StoreBase
    Implements IStoreMeta
    Private templateFilename As String

    Public Sub New(backer As IBacker, templateFilename As String)
        MyBase.New(backer)
        Me.templateFilename = templateFilename
    End Sub

    Public Sub Reset() Implements IStoreMeta.Reset
        ShutDown()
        backer.Connect(":memory:")
        Dim loadBacker As New Backer()
        loadBacker.Connect(templateFilename)
        loadBacker.BackupTo(backer)
    End Sub

    Public Sub Restore(oldBacker As IBacker) Implements IStoreMeta.Restore
        ShutDown()
        backer.TakeConnection(oldBacker)
    End Sub

    Public Sub ShutDown() Implements IStoreMeta.ShutDown
        backer.ShutDown()
    End Sub

    Public Sub Save(filename As String) Implements IStoreMeta.Save
        Dim saveBacker As New Backer
        saveBacker.Connect(filename)
        backer.BackupTo(saveBacker)
    End Sub

    Public Sub Load(filename As String) Implements IStoreMeta.Load
        Dim oldFilename = templateFilename
        templateFilename = filename
        Reset()
        templateFilename = oldFilename
    End Sub

    Public Function Renew() As IBacker Implements IStoreMeta.Renew
        Dim result As IBacker = New Backer()
        result.TakeConnection(backer)
        backer.Disconnect()
        Reset()
        Return result
    End Function
End Class
