Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Rhino.PlugIns

' Plug-in Description Attributes - all of these are optional.
' These will show in Rhino's option dialog, in the tab Plug-ins.
<Assembly: PlugInDescription(DescriptionType.Address, "-")>
<Assembly: PlugInDescription(DescriptionType.Country, "-")>
<Assembly: PlugInDescription(DescriptionType.Email, "-")>
<Assembly: PlugInDescription(DescriptionType.Phone, "-")>
<Assembly: PlugInDescription(DescriptionType.Fax, "-")>
<Assembly: PlugInDescription(DescriptionType.Organization, "-")>
<Assembly: PlugInDescription(DescriptionType.UpdateUrl, "-")>
<Assembly: PlugInDescription(DescriptionType.WebSite, "-")>

' Icons should be Windows .ico files And contain 32-bit images in the following sizes 16, 24, 32, 48, And 256.
<Assembly: PlugInDescription(DescriptionType.Icon, "MyRhino._1.EmbeddedResources.plugin-utility.ico")>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("ee4e2b39-d96b-4e4c-8f9d-9b6561e61b64")> ' This will also be the Guid of the Rhino plug-in