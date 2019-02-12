using System;
using System.Diagnostics;
using Mono.Debugging.Soft;

namespace MonoDevelop.RhinoDebug
{

  public class RhinoSoftDebuggerSession : SoftDebuggerSession
  {
    Process m_rhino_app;
    const string DEFAULT_PROFILE = "monodevelop-rhino-debug";

    protected override void OnRun(Mono.Debugging.Client.DebuggerStartInfo startInfo)
    {
      var dsi = startInfo as RhinoDebuggerStartInfo;
      if (dsi == null)
      {
        base.OnRun(startInfo);
      }
      int assignedDebugPort;
      StartListening(dsi, out assignedDebugPort);

      // Start the Rhinoceros.app process
      if (m_rhino_app != null)
        throw new InvalidOperationException("Rhino already started");

      string process_path = dsi.ExecutablePath;

      if (string.IsNullOrEmpty(process_path))
        throw new InvalidOperationException("Could not find the correct Rhinoceros.app to start");
      
      string process_args = "";
      if (process_path.StartsWith("arch ", StringComparison.Ordinal))
      {
        process_args = process_path.Substring("arch ".Length).Trim();
        process_path = "arch";
      }

      process_args += " " + dsi.Arguments;

      MonoDevelop.Core.LoggingService.LogInfo("Starting Rhino for debugging");
      MonoDevelop.Core.LoggingService.LogInfo("Start app = " + dsi.ApplicationPath);

      var psi = new ProcessStartInfo(process_path);
      psi.UseShellExecute = false;
      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError = true;
      psi.Arguments = process_args;
			
      var args = (Mono.Debugging.Soft.SoftDebuggerRemoteArgs)dsi.StartArgs;
      string envvar = string.Format("transport=dt_socket,address={0}:{1}", args.Address, assignedDebugPort);
      psi.EnvironmentVariables["RHINO_SOFT_DEBUG"] = envvar;

      foreach (var env in dsi.EnvironmentVariables)
      {
        psi.EnvironmentVariables[env.Key] = env.Value;
      }

      m_rhino_app = Process.Start(psi);

      ConnectOutput(m_rhino_app.StandardOutput, false);
      ConnectOutput(m_rhino_app.StandardError, true);
      m_rhino_app.EnableRaisingEvents = true;
      m_rhino_app.Exited += delegate
      {
        EndSession();
      };
    }

    protected override void EndSession()
    {
      base.EndSession();
      EndRhinoProcess();
    }

    protected override void OnExit()
    {
      base.OnExit();
      EndRhinoProcess();
    }

    void EndRhinoProcess()
    {
      if (m_rhino_app != null && !m_rhino_app.HasExited)
      {
        m_rhino_app.Kill();
      }
      m_rhino_app = null;
    }
  }
}

