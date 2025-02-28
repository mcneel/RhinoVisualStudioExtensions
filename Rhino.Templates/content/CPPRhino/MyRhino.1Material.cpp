// MyRhino.1Material.cpp
//

#include "stdafx.h"
#include "MyRhino.1Material.h"
#include "MyRhino.1MaterialSection.h"
#include "MyRhino.1PlugIn.h"

CRhRdkMaterial* CMyRhino__1MaterialFactory::NewMaterial() const
{
	return new CMyRhino__1Material();
}

UUID CMyRhino__1Material::RenderEngineId() const
{
	return ::MyRhino__1PlugIn().PlugInID();
}

UUID CMyRhino__1Material::PlugInId() const
{
	return ::MyRhino__1PlugIn().PlugInID();
}

UUID CMyRhino__1Material::TypeId() const
{
	// {F31C1E52-8156-44DB-B912-0F86B8766E78}
	static const GUID MyRhino__1Material_UUID = 
	{0xf31c1e52,0x8156,0x44db,{0xb9,0x12,0xf,0x86,0xb8,0x76,0x6e,0x78}};
	return MyRhino__1Material_UUID;
}

ON_wString CMyRhino__1Material::TypeName() const
{
	// TODO: Return the user-friendly type name of the material.
	return L"MyRhino.1 Material";
}

ON_wString CMyRhino__1Material::TypeDescription() const
{
	// TODO: Return a description of the material.
	return "Demo material for MyRhino.1";
}

ON_wString CMyRhino__1Material::InternalName() const
{
	// TODO: Return the material's internal name (not usually seen by the user).
	return L"your-unique-material-name-goes-here";
}

void CMyRhino__1Material::SimulateMaterial(ON_Material& matOut, CRhRdkTexture::TextureGeneration tg, int iSimulatedTextureSize, const CRhinoObject* pObject) const
{
	// TODO: Set up the ON_Material based on your material state so that it looks as much
	//       as possible like your material will look when it is rendered.
	UNREFERENCED_PARAMETER(matOut);
	UNREFERENCED_PARAMETER(tg);
	UNREFERENCED_PARAMETER(iSimulatedTextureSize);
	UNREFERENCED_PARAMETER(pObject);
}

void CMyRhino__1Material::AddUISections(IRhRdkExpandableContentUI& ui)
{
//-:cnd:noEmit
#if defined (RHINO_SDK_MFC)
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	ui.AddSection(new CPlugIn1MaterialSection);
#endif
//+:cnd:noEmit

	AddAutomaticUISection(ui, L"Parameters", L"Parameters");

	// TODO: Add any further UI sections.

	__super::AddUISections(ui);
}

void* CMyRhino__1Material::GetShader(const UUID& uuidRenderEngine, void* pvData) const
{
	UNREFERENCED_PARAMETER(pvData);

	if (!IsCompatible(uuidRenderEngine))
		return nullptr;

	void* pShader = nullptr;

	// TODO: Get a pointer to the shader used to render this material.

	return pShader;
}

bool CMyRhino__1Material::IsFactoryProductAcceptableAsChild(const CRhRdkContentFactory& factory, const wchar_t* wszChildSlotName) const
{
	// TODO: Determine if pFactory produces content suitable as a child for a particular child slot.
	//       If so, return true, otherwise return false.
	UNREFERENCED_PARAMETER(factory);
	UNREFERENCED_PARAMETER(wszChildSlotName);

	return false;
}

unsigned int CMyRhino__1Material::BitFlags() const
{
	auto flags = __super::BitFlags();

	flags &= ~bfTextureSummary; // No texture summary required.

	// TODO: Modify flags to customize how RDK uses your material.

	return flags;
}
