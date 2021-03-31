using System;
using Rhino;
using Rhino.Commands;

namespace MyNamespace
{
    public class MyCommand__1 : Command
    {
        public MyCommand__1()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static MyCommand__1 Instance { get; private set; }

        public override string EnglishName => "MyCommand.1";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}