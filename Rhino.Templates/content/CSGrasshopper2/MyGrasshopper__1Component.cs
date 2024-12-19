using System;
using Rhino.Geometry;
using GrasshopperIO;
using Grasshopper.UI;
using Grasshopper.Components;

namespace MyGrasshopper._1
{
    [IoId("e79cd2b5-cb9c-4d08-93ec-446cc1f6d923")]
    public sealed class MyGrasshopper__1Component : Component
    {
        public MyGrasshopper__1Component() : base(new Nomen(
            "MyGrasshopper.1 Component",
            "ComponentInfo",
            "ComponentChapter",
            "ComponentSection"))
        {

        }

        public MyGrasshopper__1Component(IReader reader) : base(reader) { }

#if IncludeSample
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void AddInputs(InputAdder inputs)
        {
            inputs.AddPlane("Plane", "P", "Base plane for spiral").Set(Plane.WorldXY);
            inputs.AddNumber("Inner Radius", "R0", "Inner radius for spiral").Set(1.0);
            inputs.AddNumber("Outer Radius", "R1", "Outer radius for spiral").Set(10.0);
            inputs.AddInteger("Turns", "T", "Number of turns between radii").Set(10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void AddOutputs(OutputAdder outputs)
        {
            outputs.AddCurve("Spiral", "S", "Spiral curve");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="access">The IDataAccess object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void Process(IDataAccess access)
        {
            // First, we need to retrieve all data from the input parameters.
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!access.GetItem(0, out Plane plane)) return;
            if (!access.GetItem(1, out double radius0)) return;
            if (!access.GetItem(2, out double radius1)) return;
            if (!access.GetItem(3, out int turns)) return;

            // We should now validate the data and warn the user if invalid data is supplied.
            if (radius0 < 0.0)
            {
                access.AddError("Inner radius must be bigger than or equal to zero", "");
                return;
            }
            if (radius1 <= radius0)
            {
                access.AddError("Outer radius must be bigger than the inner radius", "");
                return;
            }
            if (turns <= 0)
            {
                access.AddError("Spiral turn count must be bigger than or equal to one", "");
                return;
            }

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            Curve spiral = CreateSpiral(plane, radius0, radius1, turns);

            // Finally assign the spiral to the output parameter.
            access.SetItem(0, spiral);
        }

        Curve CreateSpiral(Plane plane, double r0, double r1, Int32 turns)
        {
            Line l0 = new Line(plane.Origin + r0 * plane.XAxis, plane.Origin + r1 * plane.XAxis);
            Line l1 = new Line(plane.Origin - r0 * plane.XAxis, plane.Origin - r1 * plane.XAxis);

            Point3d[] p0;
            Point3d[] p1;

            l0.ToNurbsCurve().DivideByCount(turns, true, out p0);
            l1.ToNurbsCurve().DivideByCount(turns, true, out p1);

            PolyCurve spiral = new PolyCurve();

            for (int i = 0; i < p0.Length - 1; i++)
            {
                Arc arc0 = new Arc(p0[i], plane.YAxis, p1[i + 1]);
                Arc arc1 = new Arc(p1[i + 1], -plane.YAxis, p0[i + 1]);

                spiral.Append(arc0);
                spiral.Append(arc1);
            }

            return spiral;
        }
#else
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void AddInputs(InputAdder inputs)
        {
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void AddOutputs(OutputAdder outputs)
        {
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="access">The IDataAccess object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void Process(IDataAccess access)
        {
            
        }
#endif
    }
}
