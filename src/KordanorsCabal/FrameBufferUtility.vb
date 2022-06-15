Imports System.Runtime.CompilerServices
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Module FrameBufferUtility
    <Extension>
    Sub UpdateTexture(buffer As FrameBuffer(Of Color), texture As Texture2D)
        texture.SetData(buffer.Pixels)
    End Sub
End Module