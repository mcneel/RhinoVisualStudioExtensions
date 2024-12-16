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
            inputs.AddPlane("Centre Point", "Cn", "Arc base plane.").Set(Plane.WorldXY);
            inputs.AddPoint("Start Point", "Pa", "Arc start point.").Set(new Point3d(1, 0, 0));
            inputs.AddPoint("End Point", "Pb", "Arc end point").Set(new Point3d(-2, 2, 0));
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void AddOutputs(OutputAdder outputs)
        {
            outputs.AddArc("Clockwise Arc", "Cw", "Arc from start to end, travelling clockwise in the base plane.");
            outputs.AddArc("Anti-clockwise Arc", "Aw", "Arc from start to end, travelling anti-clockwise in the base plane.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="access">The IDataAccess object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void Process(IDataAccess access)
        {
            access.GetItem(0, out Plane plane);
            access.GetItem(1, out Point3d pa);
            access.GetItem(2, out Point3d pb);

            pa = plane.ClosestPoint(pa);
            pb = plane.ClosestPoint(pb);

            access.VerifyNonCoincident(plane.Origin, pa, "centre", "start point");
            access.VerifyNonCoincident(plane.Origin, pb, "centre", "end point");

            var radius = plane.Origin.DistanceTo(pa);
            var sb = (pb - plane.Origin); sb.Unitize();
            pb = plane.Origin + sb * radius;

            plane.ClosestParameter(pa, out var ua, out var va);
            var α = Math.Atan2(va, ua);

            var circle = new Circle(plane, radius);
            var arc0 = new Arc(pa, -circle.TangentAt(α), pb);
            var arc1 = new Arc(pa, circle.TangentAt(α), pb);

            access.SetItem(0, arc0);
            access.SetItem(1, arc1);
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
