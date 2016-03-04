using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace ${Namespace}
{
  public class ${ProjectName}Info : GH_AssemblyInfo
  {
    public override string Name
    {
      get
      {
        return "${ProjectName} Info";
      }
    }
    public override Bitmap Icon
    {
      get
      {
        //Return a 24x24 pixel bitmap to represent this GHA library.
        return null;
      }
    }
    public override string Description
    {
      get
      {
        //Return a short string describing the purpose of this GHA library.
        return "";
      }
    }
    public override Guid Id
    {
      get
      {
        return new Guid("${Guid2}");
      }
    }

    public override string AuthorName
    {
      get
      {
        //Return a string identifying you or your company.
        return "${AuthorCompany}";
      }
    }
    public override string AuthorContact
    {
      get
      {
        //Return a string representing your preferred contact details.
        return "";
      }
    }
  }
}