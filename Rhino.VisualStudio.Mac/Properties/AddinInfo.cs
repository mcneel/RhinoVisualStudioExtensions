using System;
using Mono.Addins;

[assembly: Addin(
    "Mac",
    Namespace = "Rhino.VisualStudio",
    Version = "7.28.0.0"
)]

// This controls the displayed name in the Xamarin Studio Add-In Manager...
[assembly: AddinName("RhinoCommon Plugin Support")]

// This entry controls which of the Add-In categories this get filed under in Xamarin Studio's Add-In Manager...
[assembly: AddinCategory("Debugging")]

// This is the sub-description displayed under the AddinName in the XS Add-In Manager...
[assembly: AddinDescription("Rhino plugin templates and debugger")]

// Not displayed, AFAIK...
[assembly: AddinAuthor("Robert McNeel and Associates")]

// The link used by the More Info button in the Xamarin Studio Add-In Manager...
[assembly: AddinUrl("https://github.com/mcneel/RhinoVisualStudioExtensions")]

#if VS2022
[assembly: ImportAddinAssembly("Eto.macOS.dll")]
#elif VS2019
[assembly: ImportAddinAssembly("Eto.XamMac2.dll")]
#endif
[assembly: ImportAddinAssembly("Eto.dll")]
[assembly: ImportAddinAssembly("Rhino.VisualStudio.dll")]
[assembly: ImportAddinAssembly("Rhino.VisualStudio.Mac.dll")]
