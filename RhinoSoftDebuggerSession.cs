// To build the add-in for MonoDevelop 2.8+
// go to the build directory with terminal and run the following
// /Applications/Xamarin Studio.app/Contents/MacOS/mdtool setup pack MonoDevelop.RhinoDebug.dll
//
// This will generate a .mpack file that you can distribute to users

using System;
using System.Diagnostics;

namespace MonoDevelop.Debugger.Soft.Rhino
{
	public class RhinoSoftDebuggerSession : Mono.Debugging.Soft.SoftDebuggerSession
	{
		Process m_rhino_app;
		const string DEFAULT_PROFILE="monodevelop-rhino-debug";
		
		protected override void OnRun (Mono.Debugging.Client.DebuggerStartInfo startInfo)
		{
			var dsi = startInfo as RhinoDebuggerStartInfo;
      int assignedDebugPort;
      StartListening(dsi, out assignedDebugPort);
			StartRhinoProcess(dsi, assignedDebugPort);
		}

		void StartRhinoProcess(RhinoDebuggerStartInfo dsi, int assignedDebugPort)
		{
			if( m_rhino_app!=null )
				throw new InvalidOperationException("Rhino already started");

      string process_path = dsi.GetApplicationPath();
      string process_args = "";
      if( process_path.StartsWith("arch ") )
      {
        process_args = process_path.Substring("arch ".Length).Trim();
        process_path = "arch";
      }
      
      MonoDevelop.Core.LoggingService.LogInfo("Starting Rhino for debugging");
      if( dsi.ContainsCustomStartArgs )
        MonoDevelop.Core.LoggingService.LogInfo("--custom start args = " + dsi.GetApplicationPath());

      var psi = new ProcessStartInfo(process_path);
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
      psi.Arguments = process_args;
      //psi.Arguments = "/Applications/Rhinoceros.app/Contents/MacOS/Rhino";
      //psi.Arguments = "-i386 /Applications/Rhinoceros.app/Contents/MacOS/Rhino";
			
      var args = (Mono.Debugging.Soft.SoftDebuggerRemoteArgs) dsi.StartArgs;
			string envvar = string.Format("transport=dt_socket,address={0}:{1}", args.Address, assignedDebugPort);
			psi.EnvironmentVariables.Add("RHINO_SOFT_DEBUG", envvar);
						
			m_rhino_app = Process.Start(psi);
			ConnectOutput(m_rhino_app.StandardOutput, false);
			ConnectOutput(m_rhino_app.StandardError, true);
			m_rhino_app.EnableRaisingEvents = true;
			m_rhino_app.Exited += delegate { EndSession(); };
		}
		
		protected override void EndSession ()
		{
			EndRhinoProcess();
			base.EndSession ();
		}
		protected override void OnExit ()
		{
			EndRhinoProcess();
			base.OnExit();
		}
		void EndRhinoProcess()
		{
			if( m_rhino_app!=null && !m_rhino_app.HasExited )
			{
				m_rhino_app.Kill();
			}
			m_rhino_app = null;
		}
	}
}

