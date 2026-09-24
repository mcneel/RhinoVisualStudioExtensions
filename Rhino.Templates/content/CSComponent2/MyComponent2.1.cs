using System;
using Rhino.Geometry;
using GrasshopperIO;
using Grasshopper2.UI;
using Grasshopper2.Components;

namespace MyNamespace
{
    [IoId("b4cfdc9a-d6d2-4228-8158-8fc30298fef8")]
    public sealed class MyComponent2__1 : Component
    {
        public MyComponent2__1() : base(new Nomen(
            "MyComponent2.1",
            "Description",
            "Chapter",
            "Section"))
        {
        }

        public MyComponent2__1(IReader reader) : base(reader) { }

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
    }
}
