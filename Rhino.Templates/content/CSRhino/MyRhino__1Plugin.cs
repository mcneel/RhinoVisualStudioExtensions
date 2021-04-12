using System;
using Rhino;

namespace MyRhino._1
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
#if TypeUtility
    public class MyRhino__1Plugin : Rhino.PlugIns.PlugIn
#elif TypeDigitize
    public class MyRhino__1Plugin : Rhino.PlugIns.DigitizerPlugIn
#elif TypeImport
    public class MyRhino__1Plugin : Rhino.PlugIns.FileImportPlugIn
#elif TypeExport
    public class MyRhino__1Plugin : Rhino.PlugIns.FileExportPlugIn
#elif TypeRender
    public class MyRhino__1Plugin : Rhino.PlugIns.RenderPlugIn
#endif
    {
        public MyRhino__1Plugin()
        {
            Instance = this;
        }
        
        ///<summary>Gets the only instance of the MyRhino__1Plugin plug-in.</summary>
        public static MyRhino__1Plugin Instance { get; private set; }

#if TypeDigitize
        ///<summary>
        /// Defines the behavior in response to a request of a user to either enable or disable
        /// input from the digitizer.
        /// It is called by Rhino. If enable is true and EnableDigitizer() returns false,
        /// then Rhino will not calibrate the digitizer.
        ///</summary>
        ///<param name="enable">If true, enable the digitizer. If false, disable the digitizer.</param>
        ///<returns>You should return true if the digitizer should be calibrated. Otherwise, false.</returns>
        protected override bool EnableDigitizer(bool enable)
        {
            throw new NotImplementedException("EnableDigitizer has no defined behavior in the MyRhino._1.MyRhino__1Plugin class.");
        }

        ///<summary>
        /// Gets the unit system in which the digitizer works.
        /// passes points to SendPoint().  Rhino uses this value when it calibrates a digitizer.
        /// This unit system must not change.
        /// </summary>
        protected override UnitSystem DigitizerUnitSystem
        {
            get { throw new NotImplementedException("DigitizerUnitSystem is not implemented in the MyRhino._1.MyRhino__1Plugin class."); }
        }

        /// <summary>
        /// Gets the point tolerance, or the distance the digitizer must move (in digitizer
        /// coordinates) for a new point to be considered real rather than noise. Small
        /// desktop digitizer arms have values like 0.001 inches and 0.01 millimeters.
        /// This value should never be smaller than the accuracy of the digitizing device.
        /// </summary>
        protected override double PointTolerance
        {
            get { throw new NotImplementedException("PointTolerance is unknown in the MyRhino._1.MyRhino__1Plugin class."); }
        }
#elif TypeImport
        ///<summary>Defines file extensions that this import plug-in is designed to read.</summary>
        /// <param name="options">Options that specify how to read files.</param>
        /// <returns>A list of file types that can be imported.</returns>
        protected override Rhino.PlugIns.FileTypeList AddFileTypes(Rhino.FileIO.FileReadOptions options)
        {
            var result = new Rhino.PlugIns.FileTypeList();
            result.AddFileType("MyFileDescription (*.myext)", "myext");
            return result;
        }

        /// <summary>
        /// Is called when a user requests to import a .myext file.
        /// It is actually up to this method to read the file itself.
        /// </summary>
        /// <param name="filename">The complete path to the new file.</param>
        /// <param name="index">The index of the file type as it had been specified by the AddFileTypes method.</param>
        /// <param name="doc">The document to be written.</param>
        /// <param name="options">Options that specify how to write file.</param>
        /// <returns>A value that defines success or a specific failure.</returns>
        protected override bool ReadFile(string filename, int index, RhinoDoc doc, Rhino.FileIO.FileReadOptions options)
        {
            bool read_success = false;
            // TODO: Add code for reading file
            return read_success;
        }
#elif TypeExport
        /// <summary>Defines file extensions that this export plug-in is designed to write.</summary>
        /// <param name="options">Options that specify how to write files.</param>
        /// <returns>A list of file types that can be exported.</returns>
        protected override Rhino.PlugIns.FileTypeList AddFileTypes(Rhino.FileIO.FileWriteOptions options)
        {
            var result = new Rhino.PlugIns.FileTypeList();
            result.AddFileType("MyFileDescription (*.myext)", "myext");
            return result;
        }

        /// <summary>
        /// Is called when a user requests to export a .myext file.
        /// It is actually up to this method to write the file itself.
        /// </summary>
        /// <param name="filename">The complete path to the new file.</param>
        /// <param name="index">The index of the file type as it had been specified by the AddFileTypes method.</param>
        /// <param name="doc">The document to be written.</param>
        /// <param name="options">Options that specify how to write file.</param>
        /// <returns>A value that defines success or a specific failure.</returns>
        protected override Rhino.PlugIns.WriteFileResult WriteFile(string filename, int index, RhinoDoc doc, Rhino.FileIO.FileWriteOptions options)
        {
            return Rhino.PlugIns.WriteFileResult.Failure;
        }
#elif TypeRender
        /// <summary>
        /// Is called when the user calls the _Render command.
        /// </summary>
        /// <param name="doc">The document to be rendered.</param>
        /// <param name="mode">The run mode: interactive or scripted.</param>
        /// <param name="fastPreview">Whether the render is in preview-mode.</param>
        /// <returns>The result of the command.</returns>
        protected override Rhino.Commands.Result Render(RhinoDoc doc, Rhino.Commands.RunMode mode, bool fastPreview)
        {
            throw new NotImplementedException("Render is not implemented in the MyRhino._1.MyRhino__1Plugin class.");
        }

        /// <summary>
        /// Is called when the user calls the _RenderWindow command.
        /// </summary>
        /// <param name="doc">The document to be rendered.</param>
        /// <param name="mode">The run mode: interactive or scripted.</param>
        /// <param name="fastPreview">Whether the render is in preview-mode.</param>
        /// <param name="view">The view being rendered.</param>
        /// <param name="rect">The rendering rectangle.</param>
        /// <param name="inWindow">Whether rendering should appear in the window.</param>
        /// <returns>The result of the command.</returns>
        protected override Rhino.Commands.Result RenderWindow(RhinoDoc doc, Rhino.Commands.RunMode mode, bool fastPreview, Rhino.Display.RhinoView view, System.Drawing.Rectangle rect, bool inWindow)
        {
            throw new NotImplementedException("RenderWindow is not implemented by the MyRhino._1.MyRhino__1Plugin class.");
        }
#endif
        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
    }
}