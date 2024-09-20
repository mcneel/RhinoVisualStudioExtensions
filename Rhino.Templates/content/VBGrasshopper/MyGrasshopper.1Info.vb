Imports System
Imports Grasshopper.Kernel

Public Class MyGrasshopper__1Info
	Inherits GH_AssemblyInfo

	Public Overrides ReadOnly Property Name() As String
		Get
			Return "MyGrasshopper.1 Info"
		End Get
	End Property
	Public Overrides ReadOnly Property Icon As System.Drawing.Bitmap
		Get
			'Return a 24x24 pixel bitmap to represent this GHA library.
			Return Nothing
		End Get
	End Property
	Public Overrides ReadOnly Property Description As String
		Get
			'Return a short string describing the purpose of this GHA library.
			Return ""
		End Get
	End Property
	Public Overrides ReadOnly Property Id As System.Guid
		Get
			Return New System.Guid("cd826b9b-8dbe-4c31-aac1-6fc7ea2bcfb7")
		End Get
	End Property

	Public Overrides ReadOnly Property AuthorName As String
		Get
			'Return a string identifying you or your company.
			Return ""
		End Get
	End Property
	Public Overrides ReadOnly Property AuthorContact As String
		Get
			'Return a string representing your preferred contact details.
			Return ""
		End Get
	End Property
	Public Overrides ReadOnly Property AssemblyVersion As String
		Get
			'Return a string representing the version.  This returns the same version as the assembly.
			Return [GetType]().Assembly.GetName().Version.ToString()
		End Get
	End Property
End Class

