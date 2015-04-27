using System;
using System.Collections.Generic;
using Rhino.Commands;
using Rhino.UI;
using Eto.Forms;
using Eto.Drawing;

namespace ${Namespace}
{	
  public class ${ProjectName}Plugin : Rhino.PlugIns.PlugIn
	{	
    static public ${ProjectName}Plugin Instance { get; private set; }


    public ${ProjectName}Plugin()
		{
      Instance = this;
		}
	}
}
