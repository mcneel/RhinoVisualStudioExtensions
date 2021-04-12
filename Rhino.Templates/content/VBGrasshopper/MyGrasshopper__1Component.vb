Imports System
Imports System.Collections.Generic

Imports Grasshopper.Kernel
Imports Rhino.Geometry

'' In order to load the result of this wizard, you will also need to
'' add the output bin/ folder of this project to the list of loaded
'' folder in Grasshopper.
'' You can use the _GrasshopperDeveloperSettings Rhino command for that.

Public Class MyGrasshopper__1Component
  Inherits GH_Component
  ''' <summary>
  ''' Each implementation of GH_Component must provide a public 
  ''' constructor without any arguments.
  ''' Category represents the Tab in which the component will appear, 
  ''' Subcategory the panel. If you use non-existing tab or panel names, 
  ''' new tabs/panels will automatically be created.
  ''' </summary>
  Public Sub New()
    MyBase.New("MyGrasshopper.1Component", "ComponentNickName", _
            "ComponentDescription", _
            "ComponentCategory", "ComponentSubcategory")
  End Sub

  ''' <summary>
  ''' Registers all the input parameters for this component.
  ''' </summary>
  Protected Overrides Sub RegisterInputParams(pManager As GH_Component.GH_InputParamManager)
#If IncludeSample
    ' Use the pManager object to register your input parameters.
    ' You can often supply default values when creating parameters.
    ' All parameters must have the correct access type. If you want 
    ' to import lists or trees of values, modify the ParamAccess flag.
    pManager.AddPlaneParameter("Plane", "P", "Base plane for spiral", GH_ParamAccess.item, Plane.WorldXY)
    pManager.AddNumberParameter("Inner Radius", "R0", "Inner radius for spiral", GH_ParamAccess.item, 1.0)
    pManager.AddNumberParameter("Outer Radius", "R1", "Outer radius for spiral", GH_ParamAccess.item, 10.0)
    pManager.AddIntegerParameter("Turns", "T", "Number of turns between radii", GH_ParamAccess.item, 10)

    ' If you want to change properties of certain parameters, 
    ' you can use the pManager instance to access them by index:
    'pManager(0).Optional = True
#End If
	End Sub

    ''' <summary>
    ''' Registers all the output parameters for this component.
    ''' </summary>
  Protected Overrides Sub RegisterOutputParams(pManager As GH_Component.GH_OutputParamManager)
#If IncludeSample
    ' Use the pManager object to register your output parameters.
    ' Output parameters do not have default values, but they too must have the correct access type.
    pManager.AddCurveParameter("Spiral", "S", "Spiral curve", GH_ParamAccess.item)

    ' Sometimes you want to hide a specific parameter from the Rhino preview.
    ' You can use the HideParameter() method to do this easily
    'pManager.HideParameter(0)
#End If
  End Sub

    ''' <summary>
    ''' This is the method that actually does the work.
    ''' </summary>
    ''' <param name="DA">The DA object can be used to retrieve data from input parameters and 
    ''' to store data in output parameters.</param>
  Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
#If IncludeSample
    ' First, we need to retrieve all data from the input parameters.
    ' We'll start by declaring variables and assigning them starting values.
    Dim plane As Plane = plane.WorldXY
    Dim radius0 As Double = 0.0
    Dim radius1 As Double = 0.0
    Dim turns As Int32 = 0

    ' Then we need to access the input parameters individually. 
    ' When data cannot be extracted from a parameter, we should abort this method.
    If (Not DA.GetData(0, plane)) Then Return
    If (Not DA.GetData(1, radius0)) Then Return
    If (Not DA.GetData(2, radius1)) Then Return
    If (Not DA.GetData(3, turns)) Then Return

    ' We should now validate the data and warn the user if invalid data is supplied.
    If (radius0 < 0.0) Then
      AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radius must be bigger than or equal to zero")
      Return
    End If
    If (radius1 <= radius0) Then
      AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radius must be bigger than the inner radius")
      Return
    End If
    If (turns < 1) Then
      AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spiral turn count must be bigger than or equal to one")
      Return
    End If

    ' We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
    ' The actual functionality will be in a different method:
    Dim spiral As Curve = CreateSpiral(plane, radius0, radius1, turns)

    ' Finally assign the spiral to the output parameter.
    DA.SetData(0, spiral)
#End If
  End Sub
#If IncludeSample

  Private Function CreateSpiral(plane As Plane, r0 As Double, r1 As Double, turns As Int32) As Curve
    Dim l0 As New Line(plane.Origin + r0 * plane.XAxis, plane.Origin + r1 * plane.XAxis)
    Dim l1 As New Line(plane.Origin - r0 * plane.XAxis, plane.Origin - r1 * plane.XAxis)

    Dim p0 As Point3d() = Nothing
    Dim p1 As Point3d() = Nothing

    l0.ToNurbsCurve().DivideByCount(turns, True, p0)
    l1.ToNurbsCurve().DivideByCount(turns, True, p1)

    Dim spiral As New PolyCurve()
    For i As Integer = 0 To p0.Length - 2
      Dim arc0 As New Arc(p0(i), plane.YAxis, p1(i + 1))
      Dim arc1 As New Arc(p1(i + 1), -plane.YAxis, p0(i + 1))

      spiral.Append(arc0)
      spiral.Append(arc1)
    Next

    Return spiral
  End Function

  ''' <summary>
  ''' The Exposure property controls where in the panel a component icon 
  ''' will appear. There are seven possible locations (primary to septenary), 
  ''' each of which can be combined with the GH_Exposure.obscure flag, which 
  ''' ensures the component will only be visible on panel dropdowns.
  ''' </summary>
  Public Overrides ReadOnly Property Exposure() As GH_Exposure
    Get
      Return GH_Exposure.primary
    End Get
  End Property
#End If

  ''' <summary>
  ''' Provides an Icon for every component that will be visible in the User Interface.
  ''' Icons need to be 24x24 pixels.
  ''' </summary>
  Protected Overrides ReadOnly Property Icon() As System.Drawing.Bitmap
    Get
      'You can add image files to your project resources and access them like this:
      ' return Resources.IconForThisComponent;
      Return Nothing
    End Get
  End Property

  ''' <summary>
  ''' Each component must have a unique Guid to identify it. 
  ''' It is vital this Guid doesn't change otherwise old ghx files 
  ''' that use the old ID will partially fail during loading.
  ''' </summary>
  Public Overrides ReadOnly Property ComponentGuid() As Guid
    Get
      Return New Guid("e79cd2b5-cb9c-4d08-93ec-446cc1f6d923")
    End Get
  End Property
End Class