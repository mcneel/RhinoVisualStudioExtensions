using Grasshopper2.UI.Icon;

namespace MyGrasshopper._1
{
  public sealed class MyGrasshopper__1PluginInfo : Grasshopper2.Framework.Plugin
    {
    // Id, name, description, version, author and copyright are read from the assembly
    // attributes in the project file and AssemblyInfo.cs. Override the matching
    // properties here to supply them yourself.
    
    public override string Name => "MyGrasshopper.1";
    public override IIcon Icon => AbstractIcon.FromResource("MyGrasshopper.1Plugin", typeof(MyGrasshopper__1PluginInfo));
  }
}
