//___FILEHEADER___

#include "rhinoSdkPlugInDeclare.h"
#include "___PACKAGENAMEASIDENTIFIER___PlugIn.h"

// Rhino plug-in declaration
RHINO_PLUG_IN_DECLARE

// Rhino plug-in name
// Provide a short, friendly name for this plug-in.
RHINO_PLUG_IN_NAME(L"___PACKAGENAME___");

// Rhino plug-in id
// Provide a unique uuid for this plug-in.
RHINO_PLUG_IN_ID(L"___UUID___");

// Rhino plug-in version
// Provide a version number string for this plug-in.
RHINO_PLUG_IN_VERSION(__DATE__ "  " __TIME__)

// Rhino plug-in description
// Provide a description of this plug-in.
RHINO_PLUG_IN_DESCRIPTION(L"___PACKAGENAME___ plug-in for Rhinoceros®");

// Rhino plug-in developer declarations
// TODO: fill in the following developer declarations with
// your company information. Note, all of these declarations
// must be present or your plug-in will not load.
//
// When completed, delete the following #warning directive.
#warning Developer declarations block is incomplete!
RHINO_PLUG_IN_DEVELOPER_ORGANIZATION(L"My Company Name");
RHINO_PLUG_IN_DEVELOPER_ADDRESS(L"123 Developer Street\r\nCity State 12345-6789");
RHINO_PLUG_IN_DEVELOPER_COUNTRY(L"My Country");
RHINO_PLUG_IN_DEVELOPER_PHONE(L"123.456.7890");
RHINO_PLUG_IN_DEVELOPER_FAX(L"123.456.7891");
RHINO_PLUG_IN_DEVELOPER_EMAIL(L"support@mycompany.com");
RHINO_PLUG_IN_DEVELOPER_WEBSITE(L"http://www.mycompany.com");
RHINO_PLUG_IN_UPDATE_URL(L"http://www.mycompany.com/support");

// The one and only C___PACKAGENAMEASIDENTIFIER___PlugIn object
static class C___PACKAGENAMEASIDENTIFIER___PlugIn thePlugIn;

/////////////////////////////////////////////////////////////////////////////
// C___PACKAGENAMEASIDENTIFIER___PlugIn definition

C___PACKAGENAMEASIDENTIFIER___PlugIn& ___PACKAGENAMEASIDENTIFIER___PlugIn()
{
  // Return a reference to the one and only C___PACKAGENAMEASIDENTIFIER___PlugIn object
  return thePlugIn;
}

C___PACKAGENAMEASIDENTIFIER___PlugIn::C___PACKAGENAMEASIDENTIFIER___PlugIn()
{
  // TODO: Add construction code here
  m_plugin_version = RhinoPlugInVersion();
}

/////////////////////////////////////////////////////////////////////////////
// Required overrides

const wchar_t* C___PACKAGENAMEASIDENTIFIER___PlugIn::PlugInName() const
{
  // TODO: Return a short, friendly name for the plug-in.
  return RhinoPlugInName();
}

const wchar_t* C___PACKAGENAMEASIDENTIFIER___PlugIn::PlugInVersion() const
{
  // TODO: Return the version number of the plug-in.
  return m_plugin_version;
}

GUID C___PACKAGENAMEASIDENTIFIER___PlugIn::PlugInID() const
{
  // Must match RHINO_PLUG_IN_ID above.
  return ON_UuidFromString(RhinoPlugInId());
}

/////////////////////////////////////////////////////////////////////////////
// Additional overrides

int C___PACKAGENAMEASIDENTIFIER___PlugIn::OnLoadPlugIn()
{
  // Plug-ins are not loaded until after Rhino is started and a default document
  // is created. Because the default document already exists,
  // CRhinoEventWatcher::On????Document() functions are not called for it.
  // If you need to do any document initialization/synchronization then
  // do it here.
  // NOTE: DO NOT enable your digitizer here!

  return TRUE;
}

void C___PACKAGENAMEASIDENTIFIER___PlugIn::OnUnloadPlugIn()
{
  // TODO: Add plug-in cleanup code here.
}

/////////////////////////////////////////////////////////////////////////////
// Digitizer overrides

bool C___PACKAGENAMEASIDENTIFIER___PlugIn::EnableDigitizer(bool bEnable)
{
  // If bEnable is true and EnableDigitizer() returns false,
  // then Rhino will not calibrate the digitizer.
  if (bEnable)
  {
    // TODO: Set up your digitizer.
    // Use CRhinoDigitizerPlugIn::SendPoint() to stream points to Rhino.
  }
  else
  {
    // TODO: Stop using SendPoint().
    // Shut down the connection with the digitizer.
  }
  return true;
}

ON::LengthUnitSystem C___PACKAGENAMEASIDENTIFIER___PlugIn::UnitSystem() const
{
  // TODO: Return the digitizer's unit system.
  return ON::LengthUnitSystem::Millimeters;
}

double C___PACKAGENAMEASIDENTIFIER___PlugIn::PointTolerance() const
{
  // Precision of the digitizer in the unit system returned by UnitSystem().
  // If this number is too small, digitizer noise will cause GetPoint() to jitter.

  // TODO: Return the digitizer's point tolerance.
  return 0.01;
}
