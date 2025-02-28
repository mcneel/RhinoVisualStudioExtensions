// MyRhino.1.h : main header file for the MyRhino.1 DLL.
//

#pragma once

//-:cnd:noEmit
#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif
//+:cnd:noEmit

#include "resource.h" // main symbols

// CMyRhino__1App
// See MyRhino.1App.cpp for the implementation of this class
//

class CMyRhino__1App : public CWinApp
{
public:
  // CRITICAL: DO NOT CALL RHINO SDK FUNCTIONS HERE!
  // Only standard MFC DLL instance construction belongs here. 
  // All other significant initialization should take place in
  // CMyRhino__1PlugIn::OnLoadPlugIn().
	CMyRhino__1App() = default;

  // CRITICAL: DO NOT CALL RHINO SDK FUNCTIONS HERE!
  // Only standard MFC DLL instance initialization belongs here. 
  // All other significant initialization should take place in
  // CMyRhino__1PlugIn::OnLoadPlugIn().
	BOOL InitInstance() override;
  
  // CRITICAL: DO NOT CALL RHINO SDK FUNCTIONS HERE!
  // Only standard MFC DLL instance clean up belongs here. 
  // All other significant cleanup should take place in either
  // CMyRhino__1PlugIn::OnSaveAllSettings() or
  // CMyRhino__1PlugIn::OnUnloadPlugIn().  
	int ExitInstance() override;
  
	DECLARE_MESSAGE_MAP()
};
