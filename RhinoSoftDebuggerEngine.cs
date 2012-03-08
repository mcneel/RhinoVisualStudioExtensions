using System;

namespace MonoDevelop.Debugger.Soft.Rhino
{
	public class RhinoSoftDebuggerEngine : MonoDevelop.Debugger.IDebuggerEngine
	{
		public string Id { get{ return "Mono.Debugger.Soft.Rhino"; } }
		public string Name { get { return "Mono Soft Debugger for Rhino"; }	}
		public bool CanDebugCommand (Core.Execution.ExecutionCommand cmd)	{	return true;}
    
		public Mono.Debugging.Client.DebuggerStartInfo CreateDebuggerStartInfo (Core.Execution.ExecutionCommand cmd)
		{
      string start_args = String.Empty;
      var execution_cmd = cmd as MonoDevelop.Core.Execution.DotNetExecutionCommand;
      if( execution_cmd!=null )
      {
        start_args = execution_cmd.Arguments;
      }
			return new RhinoDebuggerStartInfo("Rhino", start_args);
		}

		public Mono.Debugging.Client.DebuggerSession CreateSession ()
		{
			return new RhinoSoftDebuggerSession();
		}

		public Mono.Debugging.Client.ProcessInfo[] GetAttachableProcesses ()
		{
			return new Mono.Debugging.Client.ProcessInfo[0];
		}
	}
	
	class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
	{
    string m_start_args;
    public RhinoDebuggerStartInfo(string appName, string start_args)
			: base(new Mono.Debugging.Soft.SoftDebuggerListenArgs(appName, System.Net.IPAddress.Loopback, 0))
		{
      m_start_args = start_args;
		}
    
    public bool ContainsCustomStartArgs
    {
      get
      {
        return !string.IsNullOrWhiteSpace(m_start_args);
      }
    }
    
    public string GetApplicationPath()
    {
      string app_path = "/Applications/Rhinoceros.app/Contents/MacOS/Rhino"; //"arch";
      if( !string.IsNullOrWhiteSpace(m_start_args) && m_start_args.StartsWith("-app_path=") )
      {
        string path = m_start_args.Substring("-app_path=".Length);
        path = path.Trim(new char[]{'\"', ' '});
        app_path = path;
      }
      return app_path;
    }
	}
}
