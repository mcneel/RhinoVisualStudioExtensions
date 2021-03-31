Imports System
Imports Rhino
Imports Rhino.Commands

Public Class MyCommand__1
    Inherits Command

    Shared _instance As MyCommand__1

    Public Sub New()
        ' Rhino only creates one instance of each command class defined in a
        ' plug-in, so it is safe to store a refence in a static field.
        _instance = Me
    End Sub

    '''<summary>The only instance of this command.</summary>
    Public Shared ReadOnly Property Instance() As MyCommand__1
        Get
            Return _instance
        End Get
    End Property

    '''<returns>The command name as it appears on the Rhino command line.</returns>
    Public Overrides ReadOnly Property EnglishName() As String
        Get
            Return "MyCommand.1"
        End Get
    End Property

    Protected Overrides Function RunCommand(ByVal doc As RhinoDoc, ByVal mode As RunMode) As Result

        ' TODO: complete command.
        Return Result.Success

    End Function
End Class
