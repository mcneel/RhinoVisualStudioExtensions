Imports System
Imports System.Collections.Generic
Imports Rhino
Imports Rhino.Commands
#If TypeUtility Then
Imports Rhino.Geometry
#End If
Imports Rhino.Input
Imports Rhino.Input.Custom

Public Class MyRhino__1Command
    Inherits Command

    Shared _instance As MyRhino__1Command

    Public Sub New()
        ' Rhino only creates one instance of each command class defined in a
        ' plug-in, so it is safe to store a refence in a static field.
        _instance = Me
    End Sub

    '''<summary>The only instance of this command.</summary>
    Public Shared ReadOnly Property Instance() As MyRhino__1Command
        Get
            Return _instance
        End Get
    End Property

    '''<returns>The command name as it appears on the Rhino command line.</returns>
    Public Overrides ReadOnly Property EnglishName() As String
        Get
            Return "MyRhino.1Command"
        End Get
    End Property

    Protected Overrides Function RunCommand(ByVal doc As RhinoDoc, ByVal mode As RunMode) As Result
#If TypeUtility Then
#If IncludeSample Then
        ' TODO: start here modifying the behaviour of your command.
        ' ---
        RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName)

        Dim pt0 As Point3d
        Using getPointAction As New GetPoint()
            getPointAction.SetCommandPrompt("Please select the start point")
            If getPointAction.[Get]() <> GetResult.Point Then
                RhinoApp.WriteLine("No start point was selected.")
                Return getPointAction.CommandResult()
            End If
            pt0 = getPointAction.Point()
        End Using

        Dim pt1 As Point3d
        Using getPointAction As New GetPoint()
            getPointAction.SetCommandPrompt("Please select the end point")
            getPointAction.SetBasePoint(pt0, True)
            AddHandler getPointAction.DynamicDraw,
        Sub(sender, e) e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.DarkRed)
            If getPointAction.[Get]() <> GetResult.Point Then
                RhinoApp.WriteLine("No end point was selected.")
                Return getPointAction.CommandResult()
            End If
            pt1 = getPointAction.Point()
        End Using

        doc.Objects.AddLine(pt0, pt1)
        doc.Views.Redraw()
        RhinoApp.WriteLine("The {0} command added one line to the document.", EnglishName)
        '' ---
#Else
        ' TODO: modify command.
        RhinoApp.WriteLine("The {0} command is under construction.", EnglishName)
#End If
#Else
        ' Usually commands in utility plug-ins are used to modify settings and behavior.
        ' The utility work itself is performed by the MyRhino__1Plugin class.
#End If
        Return Result.Success
    End Function
End Class
