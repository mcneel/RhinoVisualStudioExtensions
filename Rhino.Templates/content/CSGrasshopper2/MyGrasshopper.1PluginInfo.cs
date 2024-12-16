using System;
using System.Reflection;
using Grasshopper.UI;
using Grasshopper.UI.Icon;

namespace MyGrasshopper._1
{
  public sealed class MyGrasshopper__1PluginInfo : Grasshopper.Framework.Plugin
  {
    static T GetAttribute<T>() where T : Attribute => typeof(MyGrasshopper__1PluginInfo).Assembly.GetCustomAttribute<T>();
    
    public MyGrasshopper__1PluginInfo()
      : base(new Guid("cd826b9b-8dbe-4c31-aac1-6fc7ea2bcfb7"),
             new Nomen(
                GetAttribute<AssemblyTitleAttribute>()?.Title, 
                GetAttribute<AssemblyDescriptionAttribute>()?.Description),
             typeof(MyGrasshopper__1PluginInfo).Assembly.GetName().Version)
    { 
      Icon = AbstractIcon.FromResource("MyGrasshopper__1Plugin", typeof(MyGrasshopper__1PluginInfo));
    }
    
    public override string Author => GetAttribute<AssemblyCompanyAttribute>()?.Company;

    public override sealed IIcon Icon { get; }

    public override sealed string Copyright => GetAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? base.Copyright;

    // public override sealed string Website => "https://mywebsite.example.com";

    // public override sealed string Contact => "myemail@example.com";

    // public override sealed string LicenceAgreement => "license or URL";
    
  }
}