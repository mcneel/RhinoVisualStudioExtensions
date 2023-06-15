// MyRhino.1.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "MyRhino.1App.h"

//
//	Note!
//
//    A Rhino plug-in is an MFC DLL.
//
//		If this DLL is dynamically linked against the MFC
//		DLLs, any functions exported from this DLL which
//		call into MFC must have the AFX_MANAGE_STATE macro
//		added at the very beginning of the function.
//
//		For example:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// normal function body here
//		}
//
//		It is very important that this macro appear in each
//		function, prior to any calls into MFC.  This means that
//		it must appear as the first statement within the 
//		function, even before any object variable declarations
//		as their constructors may generate calls into the MFC
//		DLL.
//
//		Please see MFC Technical Notes 33 and 58 for additional
//		details.
//

// CMyRhino__1App

BEGIN_MESSAGE_MAP(CMyRhino__1App, CWinApp)
END_MESSAGE_MAP()

// The one and only CMyRhino__1App object
CMyRhino__1App theApp;

#if Automation
const GUID CDECL _tlid = { 0xc5e2b8aa, 0xdbc9, 0x4935, { 0x9f, 0xef, 0x4c, 0xe1, 0x41, 0xd0, 0xde, 0x4d } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;

#endif
// CMyRhino__1App initialization

BOOL CMyRhino__1App::InitInstance()
{
  // CRITICAL: DO NOT CALL RHINO SDK FUNCTIONS HERE!
  // Only standard MFC DLL instance initialization belongs here. 
  // All other significant initialization should take place in
  // CMyRhino__1PlugIn::OnLoadPlugIn().

	CWinApp::InitInstance();
#if Sockets

	if (!AfxSocketInit())
	{
		AfxMessageBox(IDP_SOCKETS_INIT_FAILED);
		return FALSE;
	}
#endif
#if Automation

	// Register all OLE server (factories) as running.  This enables the
	// OLE libraries to create objects from other applications.
	COleObjectFactory::RegisterAll();
#endif

	return TRUE;
}

int CMyRhino__1App::ExitInstance()
{
  // CRITICAL: DO NOT CALL RHINO SDK FUNCTIONS HERE!
  // Only standard MFC DLL instance clean up belongs here. 
  // All other significant cleanup should take place in either
  // CMyRhino__1PlugIn::OnSaveAllSettings() or
  // CMyRhino__1PlugIn::OnUnloadPlugIn().
  return CWinApp::ExitInstance();
}
#if Automation

// DllGetClassObject - Returns class factory
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return AfxDllGetClassObject(rclsid, riid, ppv);
}

// DllCanUnloadNow - Allows COM to unload DLL
STDAPI DllCanUnloadNow()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return AfxDllCanUnloadNow();
}

// DllRegisterServer - Adds entries to the system registry
STDAPI DllRegisterServer()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return SELFREG_E_TYPELIB;

	if (!COleObjectFactory::UpdateRegistryAll())
		return SELFREG_E_CLASS;

	return S_OK;
}

// DllUnregisterServer - Removes entries from the system registry
STDAPI DllUnregisterServer()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return SELFREG_E_TYPELIB;

	if (!COleObjectFactory::UpdateRegistryAll(FALSE))
		return SELFREG_E_CLASS;

	return S_OK;
}
#endif