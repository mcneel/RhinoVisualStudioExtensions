// MySkin.1.h : main header file for the MySkin.1 DLL.
//

#pragma once

//-:cnd:noEmit
#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif
//+:cnd:noEmit

#include "resource.h"		// main symbols

// CMySkin__1App
// See MySkin.1App.cpp for the implementation of this class
//

class CMySkin__1App : public CWinApp
{
public:
	CMySkin__1App() = default;

// Overrides
public:
	BOOL InitInstance() override;
	int ExitInstance() override;
	DECLARE_MESSAGE_MAP()
};

// CSplashWnd
// See MySkin.1App.cpp for the implementation of this class
//

class CSplashWnd : public CWnd
{
	DECLARE_DYNAMIC(CSplashWnd)

public:
	CSplashWnd();
	virtual ~CSplashWnd();

protected:
  CBitmap m_splash_bitmap;

protected:
	DECLARE_MESSAGE_MAP()

public:
  afx_msg void OnPaint();
  afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};
