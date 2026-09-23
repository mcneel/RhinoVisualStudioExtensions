using Grasshopper2.UI.Icon;

namespace MyGrasshopper._1
{
  public sealed class MyGrasshopper__1PluginInfo : Grasshopper2.Framework.Plugin
  {
    // Id, name, description, version, author and copyright are read from the assembly
    // attributes in the project file and AssemblyInfo.cs. Override the matching
    // properties here to supply them yourself.
    public MyGrasshopper__1PluginInfo()
    {
      Icon = AbstractIcon.FromResource("MyGrasshopper.1Plugin", typeof(MyGrasshopper__1PluginInfo));
    }

    public override string Author => GetAttribute<AssemblyCompanyAttribute>()?.Company;

    public override sealed IIcon Icon { get; }

    // public override sealed string Website => "https://mywebsite.example.com";

    // public override sealed string Contact => "myemail@example.com";

    // public override sealed string LicenceAgreement => "license or URL";
  }
}
