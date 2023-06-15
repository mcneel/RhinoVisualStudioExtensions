// MyRhino.1RdkPlugIn.cpp
//

#include "stdafx.h"
#include "MyRhino.1RdkPlugIn.h"
#include "MyRhino.1PlugIn.h"
#if TypeRender
#include "MyRhino.1Material.h"
#endif

UUID CMyRhino__1RdkPlugIn::PlugInId() const
{
	return ::MyRhino__1PlugIn().PlugInID();
}

CRhinoPlugIn& CMyRhino__1RdkPlugIn::RhinoPlugIn() const
{
	return ::MyRhino__1PlugIn();
}

bool CMyRhino__1RdkPlugIn::Initialize()
{
	// TODO: Initialize your plug-in. Return false on failure.

	return __super::Initialize();
}

void CMyRhino__1RdkPlugIn::Uninitialize()
{
	// TODO: Do any necessary plug-in clean-up here.

	__super::Uninitialize();
}
#if TypeRender

void CMyRhino__1RdkPlugIn::RegisterExtensions() const
{
	AddExtension(new CMyRhino__1MaterialFactory);

	// TODO: Add further material/environment/texture factories.

	__super::RegisterExtensions();
}

void CMyRhino__1RdkPlugIn::AbortRender()
{
	// TODO:
}

bool CMyRhino__1RdkPlugIn::CreatePreview(const ON_2iSize& sizeImage, RhRdkPreviewQuality quality, const IRhRdkPreviewSceneServer* pSceneServer, IRhRdkPreviewCallbacks* pNotify, CRhinoDib& dibOut)
{
	// TODO: Create a rendered preview of the specified scene at the specified size and quality.
	UNREFERENCED_PARAMETER(sizeImage);
	UNREFERENCED_PARAMETER(quality);
	UNREFERENCED_PARAMETER(pSceneServer);
	UNREFERENCED_PARAMETER(pNotify);
	UNREFERENCED_PARAMETER(dibOut);

	return false;
}

bool CMyRhino__1RdkPlugIn::CreatePreview(const ON_2iSize& sizeImage, const CRhRdkTexture& texture, CRhinoDib& dibOut)
{
	// TODO: Optionally create a preview of the texture.
	// Return false to allow RDK to produce all texture previews.
	UNREFERENCED_PARAMETER(sizeImage);
	UNREFERENCED_PARAMETER(texture);
	UNREFERENCED_PARAMETER(dibOut);

	return false;
}
#endif

bool CMyRhino__1RdkPlugIn::SupportsFeature(const UUID& uuidFeature) const
{
	// TODO: Determine which features of the RDK are exposed while this is the current renderer.

	if (uuidFeature == uuidFeatureCustomRenderMeshes)
		return true; // This renderer supports custom render meshes (because it uses the iterator).

	if (uuidFeature == uuidFeatureDecals)
		return false; // This renderer does not support decals.

	if (uuidFeature == uuidFeatureGroundPlane)
		return false; // This renderer does not support a ground plane.

	if (uuidFeature == uuidFeatureSun)
		return false; // This renderer does not support the Sun.

	return true;
}
