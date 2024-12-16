using System;
using Grasshopper.UI;
using Grasshopper.UI.Icon;

namespace MyGrasshopper._1
{
  public sealed class MyGrasshopper__1PluginInfo : Grasshopper.Framework.StandardRmaPlugin
  {
    public MyGrasshopper__1PluginInfo()
      : base(new Guid("cd826b9b-8dbe-4c31-aac1-6fc7ea2bcfb7"),
             new Nomen("MyGrasshopper.1 Info", ""),
             AbstractIcon.FromResource("MyGrasshopper__1Plugin", typeof(MyGrasshopper__1PluginInfo)))
    { }
  }
}