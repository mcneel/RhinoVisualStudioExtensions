using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
#if TypeUtility
using Rhino.Geometry;
#endif
using Rhino.Input;
using Rhino.Input.Custom;

namespace MyRhino._1
{
    public class MyRhino__1Command : Command
    {
        public MyRhino__1Command()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MyRhino__1Command Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "MyRhino__1Command";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
#if TypeUtility
#if IncludeSample
            // TODO: start here modifying the behaviour of your command.
            // ---
            RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName);

            Point3d pt0;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the start point");
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No start point was selected.");
                    return getPointAction.CommandResult();
                }
                pt0 = getPointAction.Point();
            }

            Point3d pt1;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DynamicDraw +=
                  (sender, e) => e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.DarkRed);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return getPointAction.CommandResult();
                }
                pt1 = getPointAction.Point();
            }

            doc.Objects.AddLine(pt0, pt1);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one line to the document.", EnglishName);

            // ---
#else
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);
#endif
#else
            // Usually commands in utility plug-ins are used to modify settings and behavior.
            // The utility work itself is performed by the MyRhino__1Plugin class.
#endif
            return Result.Success;
        }
    }
}
