using System;
using Mono.Addins;

// This controls the final name of the .mpack file that is generated.
// e.g.: the following becomes RhinoXamarinStudioAddIn_x.x.x.x.mpack.
[assembly:Addin(
  "RhinoXamarinStudioAddIn",
	Version = "5.9.7.0"
)]

// This controls the displayed name in the Xamarin Studio Add-In Manager...
[assembly:AddinName("RhinoCommon Plugin Support")]

// This entry controls which of the Add-In categories this get filed under in Xamarin Studio's Add-In Manager...
[assembly:AddinCategory("Debugging")]

// This is the sub-description displayed under the AddinName in the XS Add-In Manager...
[assembly:AddinDescription("Rhino plugin templates and debugger")]

// Not displayed, AFAIK...
[assembly:AddinAuthor("Robert McNeel and Associates")]

// The link used by the More Info button in the Xamarin Studio Add-In Manager...
[assembly:AddinUrl("https://github.com/mcneel/RhinoCommonXamarinStudioAddin")]
