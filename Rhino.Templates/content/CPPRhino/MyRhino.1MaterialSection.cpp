// MyRhino.1MaterialSection.cpp
//

#include "stdafx.h"
#include "MyRhino.1MaterialSection.h"
#include "MyRhino.1Material.h"

#if defined (RHINO_SDK_MFC)
CMyRhino__1MaterialSection::CMyRhino__1MaterialSection()
	:
	CRhRdkMaterialUISection_MFC(IDD)
{
}

UUID CMyRhino__1MaterialSection::Uuid() const
{
	// {C9B426F6-A09A-410E-842E-296D084AE6A7}
	static const GUID uuid = 
	{0xc9b426f6,0xa09a,0x410e,{0x84,0x2e,0x29,0x6d,0x08,0x4a,0xe6,0xa7}};
	return uuid;
}

void CMyRhino__1MaterialSection::DoDataExchange(CDataExchange* pDX)
{
	__super::DoDataExchange(pDX);

	// TODO: Add your controls here.

//	DDX_Control(pDX, IDC_XXXX, m_xxxx);
}

BEGIN_MESSAGE_MAP(CMyRhino__1MaterialSection, CRhRdkMaterialUISection_MFC)
END_MESSAGE_MAP()

BOOL CMyRhino__1MaterialSection::OnInitDialog()
{
	__super::OnInitDialog();

	DisplayData();
	EnableDisableControls();

	return FALSE;
}

const wchar_t* CMyRhino__1MaterialSection::Caption(bool bAlwaysEnglish) const
{
	UNREFERENCED_PARAMETER(bAlwaysEnglish);

	return L"MyRhino.1 Material";
}

void CMyRhino__1MaterialSection::DisplayData()
{
	CRhRdkMaterialArray a;
	GetMaterialList(a);

	if (0 == a.Count())
	{
		return;
	}

	// TODO: transfer editable material state to UI controls.
}

void CMyRhino__1MaterialSection::EnableDisableControls()
{
	// TODO: enable and disable controls based on material state.
}
#endif
