// MySkin.1.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "rhinoSdkPlugInDeclare.h"
#include "MySkin.1App.h"

// Rhino plug-in declaration
RHINO_PLUG_IN_DECLARE

// Rhino developer declarations
// TODO: fill in the following developer declarations 
// with your company information. When completed,
// delete the following #error directive.
// #error Developer declarations block is incomplete!
RHINO_PLUG_IN_DEVELOPER_ORGANIZATION( L"My Company Name" );
RHINO_PLUG_IN_DEVELOPER_ADDRESS( L"123 Developer Street\r\nCity State 12345-6789" );
RHINO_PLUG_IN_DEVELOPER_COUNTRY( L"My Country" );
RHINO_PLUG_IN_DEVELOPER_PHONE( L"123.456.7890" );
RHINO_PLUG_IN_DEVELOPER_FAX( L"123.456.7891" );
RHINO_PLUG_IN_DEVELOPER_EMAIL( L"support@mycompany.com" );
RHINO_PLUG_IN_DEVELOPER_WEBSITE( L"http://www.mycompany.com" );
RHINO_PLUG_IN_UPDATE_URL( L"http://www.mycompany.com/support" );

//
//	Note!
//
//    A Rhino Skin DLL is an MFC DLL.
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

// CMySkin__1App

BEGIN_MESSAGE_MAP(CMySkin__1App, CWinApp)
END_MESSAGE_MAP()

// The one and only CMySkin__1App object
static class CMySkin__1App theApp;

// CMySkin__1App initialization

BOOL CMySkin__1App::InitInstance()
{
	CWinApp::InitInstance();

	return TRUE;
}

int CMySkin__1App::ExitInstance()
{
  return CWinApp::ExitInstance();
}

// CSplashWnd

IMPLEMENT_DYNAMIC(CSplashWnd, CWnd)

CSplashWnd::CSplashWnd()
{
}

CSplashWnd::~CSplashWnd()
{
}

BEGIN_MESSAGE_MAP(CSplashWnd, CWnd)
  ON_WM_PAINT()
  ON_WM_CREATE()
END_MESSAGE_MAP()

// CSplashWnd message handlers

void CSplashWnd::OnPaint()
{
  CPaintDC dc(this); // device context for painting

  CRect r;
  GetClientRect(r);

  if ((HBITMAP)m_splash_bitmap)
  {
    CDC memDC;
    memDC.CreateCompatibleDC(NULL);
    memDC.SelectObject(&m_splash_bitmap);
    dc.BitBlt(0, 0, r.Width(), r.Height(), &memDC, 0, 0, SRCCOPY);
  }
  else
  {
    dc.FillSolidRect(r, ::GetSysColor(COLOR_WINDOW));
    COLORREF cr = dc.SetTextColor(::GetSysColor(COLOR_WINDOWTEXT));
    int iBkMode = dc.SetBkMode( TRANSPARENT );
    CString s = L"Sample Splash Screen";
    dc.DrawText(s, r, DT_SINGLELINE | DT_CENTER | DT_VCENTER);
    dc.SetTextColor(cr);
    dc.SetBkMode(iBkMode);
  }
}

int CSplashWnd::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
  if (CWnd::OnCreate(lpCreateStruct) == -1)
    return -1;

  AFX_MANAGE_STATE(::AfxGetStaticModuleState());
  m_splash_bitmap.LoadBitmap(IDB_SPLASH);

  CRect rW, rC;
  GetWindowRect(rW);
  GetClientRect(rC);

  BITMAP bmp;
  m_splash_bitmap.GetBitmap(&bmp);

  int cx = bmp.bmWidth + (rW.Width() - rC.Width());
  int cy = bmp.bmHeight + (rW.Height() - rC.Height());
  SetWindowPos(NULL, 0, 0, cx, cy, SWP_NOMOVE | SWP_NOZORDER);
  CenterWindow();

  return 0;
}

/////////////////////////////////////////////////////////////////////////////
// class CMySkin__1SkinDLL
//
class CMySkin__1SkinDLL : public CRhinoSkinDLL
{
public:
  CMySkin__1SkinDLL();
  ~CMySkin__1SkinDLL();

  // Required overrides
  HICON MainRhinoIcon() override;
  const wchar_t* ApplicationName() override;
  UUID SkinPlugInID() override;
  void ShowSplash(bool bShow) override;
#if CustomMenus

  // Optional overrides
  HMENU MainRhinoMenu() override;
#endif

private:
  HICON m_hIcon; // Used to save the skin icon handle so the destructor can delete it later.
  CMenu m_menu;  // Used to save the menu handle so the destructor can delete it later.
  CSplashWnd m_wndSplash; // Splash window to create and show when ShowSplash(true) is called.
};

// The one and only CMySkin__1SkinDLL object. 
// This must be created for the skin to load.
static class CMySkin__1SkinDLL theSkin;

CMySkin__1SkinDLL::CMySkin__1SkinDLL()
: m_hIcon(NULL)
{
}

CMySkin__1SkinDLL::~CMySkin__1SkinDLL()
{
  // Make sure the DLL module instance is active since this is a DLL which is being loaded
  // by the Rhino executable.
  //
  // For more information on module states and MFC, see 
  // "Managing the State Data of MFC Modules" in Creating
  // New Documents, Windows, and Views and Technical Note 58.
  // 
  AFX_MANAGE_STATE(::AfxGetStaticModuleState());

  // Destroy the splash window if necessary.
  if (::IsWindow(m_wndSplash.m_hWnd))
    m_wndSplash.DestroyWindow();

  // Destroy the applicaion icon if necessary.
  if (m_hIcon)
    ::DestroyIcon(m_hIcon);
  m_hIcon = NULL;
}

const wchar_t* CMySkin__1SkinDLL::ApplicationName()
{
  // Return application name string used to replace the string "Rhino".
  // this must return a non NULL string or the skin DLL will fail to load.
  return L"MySkin.1";
}

// Return CSkinPlugInSamplePlugIn::PlugInID()
UUID CMySkin__1SkinDLL::SkinPlugInID()
{
  // Returns the UUID of the companion Rhino plug-in that is used
  // to manage this DLL's menu and provided other extensions to 
  // Rhino. If this plug-in is not going to provide a custom menu, 
  // then is must return ON_nil_uuid.

  static const GUID MySkin__1PlugIn_UUID = ON_nil_uuid;
  return MySkin__1PlugIn_UUID;
}

void CMySkin__1SkinDLL::ShowSplash( bool bShow )
{
  // This method will be called when Rhino wants to display or hide a splash screen
  // on startup.  If you do not provide a splash screen then none will appear.  
  // This will not be called if Rhino is started with the "/nosplash" option.

  // For more information on module states and MFC, see 
  // "Managing the State Data of MFC Modules" in Creating
  // New Documents, Windows, and Views and Technical Note 58.
  AFX_MANAGE_STATE(::AfxGetStaticModuleState());

  if (bShow && FALSE == ::IsWindow(m_wndSplash.m_hWnd))
  {
    CSize size(::GetSystemMetrics(SM_CXFULLSCREEN), ::GetSystemMetrics(SM_CYFULLSCREEN));
    CRect r(CPoint(0, 0), size);
    r.DeflateRect(r.Width() / 3, r.Height() / 3);
    m_wndSplash.CreateEx(WS_EX_TOPMOST, AfxRegisterWndClass(NULL), ApplicationName(), WS_POPUP | WS_VISIBLE | WS_BORDER, r, NULL, 0, NULL);
  }
  else if (bShow)
  {
    m_wndSplash.ShowWindow(SW_SHOW);
    m_wndSplash.UpdateWindow();
  }
  else if (!bShow && ::IsWindow(m_wndSplash.m_hWnd))
  {
    m_wndSplash.DestroyWindow();
  }
}

HICON CMySkin__1SkinDLL::MainRhinoIcon()
{
  // Return HICON to be used by Rhino main frame and dialog boxes, 
  // the skin DLL will fail to load if this returns NULL.

  // (Extracted from the Platform SDK help file)
  // For more information on module states and MFC, see 
  // "Managing the State Data of MFC Modules" in Creating
  // New Documents, Windows, and Views and Technical Note 58.
  AFX_MANAGE_STATE(::AfxGetStaticModuleState());

  if (NULL == m_hIcon)
    m_hIcon = theApp.LoadIcon(IDI_ICON);

  return m_hIcon;
}
#if CustomMenus

HMENU CMySkin__1SkinDLL::MainRhinoMenu()
{
  // Override this and return a valid HMENU if you want to replace
  // the main Rhino menu otherwise Rhino will use its default menu.

  // (Extracted from the Platform SDK help file)
  // For more information on module states and MFC, see 
  // "Managing the State Data of MFC Modules" in Creating
  // New Documents, Windows, and Views and Technical Note 58.
  AFX_MANAGE_STATE(::AfxGetStaticModuleState());

  // TODO: load your menu resource here. 
  //   For example:
  //     if( NULL == m_menu.GetSafeHmenu() )
  //     m_menu.LoadMenu( IDR_MENU );

  return m_menu.GetSafeHmenu();
}
#endif