using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace MyGrasshopper
{
  public class MyGrasshopperInfo : GH_AssemblyInfo
  {
    public override string Name => "MyGrasshopper Info";

    //Return a 24x24 pixel bitmap to represent this GHA library.
    public override Bitmap Icon => null;

    //Return a short string describing the purpose of this GHA library.
    public override string Description => "";

    public override Guid Id => new Guid("cd826b9b-8dbe-4c31-aac1-6fc7ea2bcfb7");

    //Return a string identifying you or your company.
    public override string AuthorName => "";

    //Return a string representing your preferred contact details.
    public override string AuthorContact => "";
  }
}